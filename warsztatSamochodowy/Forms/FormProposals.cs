using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormProposals
    {
        ProposalRepository vehicleRepository = new ProposalRepository();
        public List<Proposal> proposals { get; set; }

        public FormProposals(List<Proposal> proposals)
        {
            this.proposals = proposals;
        }
    }
}
