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
    }
}
