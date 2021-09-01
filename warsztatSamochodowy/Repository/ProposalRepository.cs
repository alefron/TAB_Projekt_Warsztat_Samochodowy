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
    }
}
