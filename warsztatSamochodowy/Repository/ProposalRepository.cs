using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ProposalRepository:RepositoryBase<Proposal>
    {
        public ProposalRepository()
        {
            dbSet = context.Proposals;
        }

        public async Task<List<Proposal>> GetProposalByVehicleAsync(string regNumber)
        {
            return await context.Proposals
                .Where((proposal) => proposal.VehicleId == regNumber).ToListAsync();
        }

        public List<Proposal> GetProposalByVehicle(string regNumber)
        {
            return Task.Run(() => { return GetProposalByVehicleAsync(regNumber); }).Result;
        }

        public async Task<Proposal> GetProposalByIdAsync(int id)
        {
            return await context.Proposals
                .Where((proposal) => proposal.Id == id).FirstOrDefaultAsync();
        }

        public Proposal GetProposalById(int id)
        {
            return Task.Run(() => { return GetProposalByIdAsync(id); }).Result;
        }

        public async Task<int> DeleteProposalAsync(Proposal proposal)
        {
            var prop = context.Proposals.Single(p => p.Id == proposal.Id);
            var actions = context.Actions.Where((action) => action.ProposalId == proposal.Id).ToList();

            context.Proposals.Remove(proposal);
            return await context.SaveChangesAsync();
        }
        public int DeleteProposal(Proposal proposal)
        {
            Task<int> t = Task.Run(() => { return DeleteProposalAsync(proposal); });
            return t.Result;
        }

        public async Task<List<Proposal>> GetJoinedProposalsAsync()
        {
            /*
            SELECT * FROM Proposal
            JOIN Vehicles
            JOIN Personel
            */

            var queryResult = await context.Proposals.Join<Proposal, Vehicle, string, Proposal>(
                    context.Vehicles,
                    proposal => proposal.VehicleId,
                    vehicle => vehicle.RegNumber,
                    (proposal, vehicle) => new Proposal
                    {
                        //Jak ktoś wie jak to zrobić lepiej to niech podzieli się wiedzą
                        //Bo wychodzi dużo przepisywania a ludzka lmbda nie działa
                        Id = proposal.Id,
                        Description = proposal.Description,
                        Result = proposal.Result,
                        Status = proposal.Status,
                        StartDate = proposal.StartDate.Date,
                        EndDate = proposal.EndDate,
                        VehicleId = proposal.VehicleId,
                        ManagerId = proposal.ManagerId,

                        Vehicle = vehicle, //I tutaj walimy tym joinem
                    }
                ).Join<Proposal, Personel, int, Proposal>(
                    context.Personel,
                    proposal => proposal.ManagerId,
                    personel => personel.Id,
                    (proposal, personel) => new Proposal
                    {
                        Id = proposal.Id,
                        Description = proposal.Description,
                        Result = proposal.Result,
                        Status = proposal.Status,
                        StartDate = proposal.StartDate.Date,
                        EndDate = proposal.EndDate,    //Chyba to jest niebezpieczne
                        VehicleId = proposal.VehicleId,
                        ManagerId = proposal.ManagerId,

                        Vehicle = proposal.Vehicle, 
                        Manager = personel             //I tutaj walimy tym joinem
                    }
                ).ToListAsync();

            var proposalsList = new List<Proposal>();

            if (queryResult?.Any() == true)
            {
                foreach (var proposal in queryResult)
                {
                    proposalsList.Add(new Proposal()
                    {
                        Id = proposal.Id,
                        Description = proposal.Description,
                        Result = proposal.Result,
                        Status = proposal.Status,
                        StartDate = proposal.StartDate.Date,
                        EndDate = proposal.EndDate,   
                        VehicleId = proposal.VehicleId,
                        ManagerId = proposal.ManagerId,

                        Vehicle = proposal.Vehicle,
                        Manager = proposal.Manager
                    });
                }
            }
            return proposalsList;
        }

        public List<Proposal> GetJoinedProposals()
        {
            return Task.Run(GetJoinedProposalsAsync).Result;
        }

        public async Task<int> AddProposalAsync(string regNumber, string description, int managerId)
        {
            Proposal proposal = new Proposal { Description = description,
                Status = StatusEnum.OPEN,
                StartDate = DateTime.Now,
                VehicleId = regNumber,
                ManagerId = managerId };

            context.Add<Proposal>(proposal);
            await context.SaveChangesAsync();

            int newId = proposal.Id;

            return newId;
        }

        public int AddProposal(string regNumber, string description, int managerId)
        {
            Task<int> t = Task.Run(() => { return AddProposalAsync(regNumber, description, managerId); });
            var res = t.Result;
            return t.Result;
        }

        public async Task<int> UpdateProposalAsync(string regNumber, string description, string result, int managerId, int proposalId)
        {
            Proposal toUpdate = this.GetProposalById(proposalId);
            toUpdate.VehicleId = regNumber;
            toUpdate.Description = description;
            toUpdate.Result = result;
            toUpdate.ManagerId = managerId;

            context.Update<Proposal>(toUpdate);
            await context.SaveChangesAsync();

            int newId = toUpdate.Id;

            return newId;
        }

        public int UpdateProposal(string regNumber, string description, string result, int managerId, int proposalId)
        {
            Task<int> t = Task.Run(() => { return UpdateProposalAsync(regNumber, description, result, managerId, proposalId); });
            var res = t.Result;
            return t.Result;
        }
    }
}
