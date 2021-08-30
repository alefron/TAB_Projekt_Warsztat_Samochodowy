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
            return await context.Actions
                .Where(action => action.ProposalId == proposalId)
                .ToListAsync();
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

            }else
            if (newStatus == StatusEnum.CANCELED)
            {
                var proposal = await context.Proposals
                       .Where(proposal => proposal.Id == action.ProposalId)
                       .FirstOrDefaultAsync();

                proposal.Status = StatusEnum.CANCELED; //Any action is cancelled so propsoal is cancelled
                context.Proposals.Update(proposal);

            }
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




    }




}
