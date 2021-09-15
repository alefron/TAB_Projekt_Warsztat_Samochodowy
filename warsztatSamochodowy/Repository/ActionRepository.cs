using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{


    class ActionsQuery
    {
        public Models.Action action;
        public Proposal proposal;
        public Vehicle vehicle;
        public ActionType actionType;
    }



    public class ActionRepository:RepositoryBase<Models.Action>
    {
        public ActionRepository()
        {
            base.dbSet = context.Actions;
        }

        public async Task<int> AddActionAsync(int proposalId, string type, int worker, int sequenceNumber, string description)
        {
            Models.Action action = new Models.Action
            {
                SequenceNumber = sequenceNumber,
                Description = description,
                Status = (int)StatusEnum.OPEN,
                StartDate = DateTime.Now,
                WorkerId = worker,
                ProposalId = proposalId,
                ActionTypeId = type
            };

            context.Add<Models.Action>(action);
            await context.SaveChangesAsync();

            int newId = action.Id;

            return newId;
        }

        public int AddAction(int proposalId, string type, int worker, int sequenceNumber, string description)
        {
            Task<int> t = Task.Run(() => { return AddActionAsync(proposalId, type, worker, sequenceNumber, description); });
            var res = t.Result;
            return t.Result;
        }

        public async Task<int> UpdateActionAsync(int actionId, string type, int worker, int sequenceNumber, string description, string result)
        {
            Models.Action toUpdate = this.GetActionById(actionId);
            if (type != null && worker != 0 && sequenceNumber != 0)
            {
                toUpdate.SequenceNumber = sequenceNumber;
                toUpdate.Description = description;
                toUpdate.Result = result;
                toUpdate.WorkerId = worker;
                toUpdate.ActionTypeId = type;
            }
            else
            {
                toUpdate.Description = description;
                toUpdate.Result = result;
            }

            context.Update<Models.Action>(toUpdate);
            await context.SaveChangesAsync();

            int newId = toUpdate.Id;

            return newId;
        }

        public int UpdateAction(int actionId, string type, int worker, int sequenceNumber, string description, string result)
        {
            Task<int> t = Task.Run(() => { return UpdateActionAsync(actionId, type, worker, sequenceNumber, description, result); });
            var res = t.Result;
            return t.Result;
        }

        public async Task<List<Models.Action>> GetActionsForWorkerAsync(int WorkerId)
        {
            /*
             SELECT * FROM Actions AS A
             JOIN Personel AS P
             ON A.WorkerId = P.Id
             */


            var queryResult = await context.Actions
                .Where((action) => action.WorkerId == WorkerId)
                .Join(
                    context.Proposals,
                    action => action.ProposalId,
                    proposal => proposal.Id,
                    (action, proposal) => new ActionsQuery
                    {
                        action = action,
                        proposal = proposal
                    }
                ).Join(
                    context.Vehicles,
                    actionQuery => actionQuery.proposal.VehicleId,
                    vehicle => vehicle.RegNumber,
                    (actionQuery, vehicle) => new ActionsQuery
                    {
                        action = actionQuery.action,
                        proposal = actionQuery.proposal,
                        vehicle = vehicle
                    }
                ).Join(
                    context.ActionTypes,
                    actionQuery => actionQuery.action.ActionTypeId,
                    actionType => actionType.CodeAction,
                    (actionQuery, actionType) => new ActionsQuery
                    {
                        action = actionQuery.action,
                        proposal = actionQuery.proposal,
                        vehicle = actionQuery.vehicle,
                        actionType = actionType

                    }
                 )
                .ToListAsync();

            List<Models.Action> actionList = new List<Models.Action>();

            foreach (var queryItem in queryResult)
            {
                var action = queryItem.action;
                action.Proposal = queryItem.proposal;
                action.Proposal.Vehicle = queryItem.vehicle;
                action.ActionType = queryItem.actionType;
                actionList.Add(action);

            }

            return actionList;
        }




        public List<Models.Action> GetActionsForWorker(int WorkerId)
        {
            return Task.Run(() => { return GetActionsForWorkerAsync(WorkerId); }).Result;
        }


        public async Task<Models.Action> GetActionByIdAsync(int ActionId)
        {
            /*
             SELECT * FROM Actions AS A
             JOIN Personel AS P
             ON A.WorkerId = P.Id
             JOIN ActionType
             ON ActionType.CodeAction = A.ActionTypeId
             */


            ActionsQuery queryResult = await context.Actions
                .Where((action) => action.Id == ActionId)
                .Join(
                    context.Proposals,
                    action => action.ProposalId,
                    proposal => proposal.Id,
                    (action, proposal) => new ActionsQuery
                    {
                        action = action,
                        proposal = proposal
                    }
                ).Join(
                    context.Vehicles,
                    actionQuery => actionQuery.proposal.VehicleId,
                    vehicle => vehicle.RegNumber,
                    (actionQuery, vehicle) => new ActionsQuery
                    {
                        action = actionQuery.action,
                        proposal = actionQuery.proposal,
                        vehicle = vehicle
                    }
                ).Join(
                    context.ActionTypes,
                    actionQuery => actionQuery.action.ActionTypeId,
                    actionType => actionType.CodeAction,
                    (actionQuery, actionType) => new ActionsQuery
                    {
                        action = actionQuery.action,
                        proposal = actionQuery.proposal,
                        vehicle = actionQuery.vehicle,
                        actionType = actionType

                    }
                 )
                .FirstOrDefaultAsync();

            Models.Action action = null;

            if (queryResult != null)
            {
                action = queryResult.action;
                action.Proposal = queryResult.proposal;
                action.Proposal.Vehicle = queryResult.vehicle;
                action.ActionType = queryResult.actionType;
            }

            return action;
        }


        public Models.Action GetActionById(int actionId)
        {
            return Task.Run(() => { return GetActionByIdAsync(actionId); }).Result;
        }


        public async Task<List<Models.Action>> GetActionsForProposalAsync(int proposalId)
        {
            var queryResult = await context.Actions
                .Where((action) => action.ProposalId == proposalId)
                .Join(
                    context.ActionTypes,
                    action => action.ActionTypeId,
                    type => type.CodeAction,
                    (action, type) => new ActionsQuery
                    {
                        action = action,
                        actionType = type
                    }
                )
                .ToListAsync();

            List<Models.Action> actionList = new List<Models.Action>();

            foreach (var queryItem in queryResult)
            {
                var action = queryItem.action;
                action.ActionType = queryItem.actionType;
                actionList.Add(action);

            }

            return actionList;
        }


        public List<Models.Action> GetActionsForProposal(int proposalId)
        {
            return Task.Run(() => { return GetActionsForProposalAsync(proposalId); }).Result;
        }




        private async Task onActionStatusChangedAsync(Models.Action action, StatusEnum oldStatus, StatusEnum newStatus)
        {

            if (newStatus == StatusEnum.FINAL)
            {
                var sameProposalList = await GetActionsForProposalAsync(action.ProposalId);

                bool anyNotFinal = false;

                foreach (var actionItem in sameProposalList)
                {
                    if(actionItem.Status != StatusEnum.FINAL)
                    {
                        anyNotFinal = true;
                        break;
                    }
                }

                if (anyNotFinal == false) // All are final so propsal is final
                {
                    var proposal = await context.Proposals
                        .Where(proposal => proposal.Id == action.ProposalId)
                        .FirstOrDefaultAsync();

                    proposal.Status = StatusEnum.FINAL;
                    context.Proposals.Update(proposal);
                }

            }/*else
            if (newStatus == StatusEnum.CANCELED)
            {
                var proposal = await context.Proposals
                       .Where(proposal => proposal.Id == action.ProposalId)
                       .FirstOrDefaultAsync();

                proposal.Status = StatusEnum.CANCELED; //Any action is cancelled so propsoal is cancelled
                context.Proposals.Update(proposal);

            }*/
        }

        public async Task< Models.Action> SetActionStatusAsync(int actionId,StatusEnum status)
        {
            var queryItem = await context.Actions
                .Where((action) => action.Id == actionId)
                 .Join(
                    context.Proposals,
                    action => action.ProposalId,
                    proposal => proposal.Id,
                    (action, proposal) => new ActionsQuery
                    {
                        action = action,
                        proposal = proposal
                    }
                )
                .FirstOrDefaultAsync();

            Models.Action action = queryItem.action;
            action.Proposal = queryItem.proposal;

            StatusEnum oldStatus = action.Status;
            action.Status = status;
            context.Actions.Update(action);

            await onActionStatusChangedAsync(action, oldStatus, status);
            await context.SaveChangesAsync();



            return action;

        }


        public Models.Action SetActionStatus(int actionId, StatusEnum status)
        {
            return Task.Run(()=> { return SetActionStatusAsync(actionId, status); }).Result;
        }

        public async Task<List<Models.Action>> getAllActionByActionTypeIdAsync(string actionTypeId)
        {
            return await context.Actions
                .Where((action) => action.ActionType.CodeAction == actionTypeId)
                .ToListAsync();
        }

        public async Task<int> DeleteActionAsync(Models.Action action)
        {
            var actionFromDb = context.Actions.Where((a) => a.Id == action.Id).FirstOrDefault();
            int actionId = actionFromDb.Id;
            var actions = context.Actions.Where((a) => a.ProposalId == actionFromDb.ProposalId).ToList();
            foreach(var act in actions)
            {
                if (act.SequenceNumber == actionId)
                {
                    act.SequenceNumber = -1;
                }
            }


            foreach(var act in actions)
            {
                if (act.Id != actionFromDb.Id)
                {
                    context.Actions.Update(act);
                }
            }
            context.Actions.Remove(actionFromDb);
            return await context.SaveChangesAsync();
        }
        public int DeleteAction(Models.Action action)
        {
            Task<int> t = Task.Run(() => { return DeleteActionAsync(action); });
            return t.Result;
        }


        public List<Models.Action> getAllActionByActionTypeId(string actionTypeId)
        {
            return Task.Run(() => { return getAllActionByActionTypeIdAsync(actionTypeId); }).Result;
        }


    }




}
