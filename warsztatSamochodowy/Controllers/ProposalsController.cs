using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ProposalsController : Controller
    {
        private ProposalRepository proposalRepository = new ProposalRepository();
        private List<Proposal> proposals { get; set; }
        private List<FormProposals> model { get; set; } = new List<FormProposals>();

        public ProposalsController()
        {
            this.proposals = proposalRepository.GetJoinedProposals();
            this.model.Add(new FormProposals(proposals));
        }

        [HttpGet("Proposals/proposalsList")]
        public IActionResult ProposalList()
        {
            this.model.Add(new FormProposals(proposals));
            return View(model);
        }

        /*[HttpGet("Proposals/getProposalActionsComplete")]
        public IActionResult getProposalActionsComplete(int proposal_id)
        {

        }*/

    }
}
