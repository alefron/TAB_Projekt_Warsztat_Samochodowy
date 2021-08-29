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
                )
                .ToListAsync();

            List<Models.Action> actionList = new List<Models.Action>();

            foreach (var queryItem in queryResult)
            {
                var action = queryItem.action;
                action.Proposal = queryItem.proposal;
                action.Proposal.Vehicle = queryItem.vehicle;

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
                )
                .FirstOrDefaultAsync();

            Models.Action action = null;

            if (queryResult != null)
            {
                action = queryResult.action;
                action.Proposal = queryResult.proposal;
                action.Proposal.Vehicle = queryResult.vehicle;
            }

            return action;
        }


        public Models.Action GetActionById(int actionId)
        {
            return Task.Run(() => { return GetActionByIdAsync(actionId); }).Result;
        }



    }




}
