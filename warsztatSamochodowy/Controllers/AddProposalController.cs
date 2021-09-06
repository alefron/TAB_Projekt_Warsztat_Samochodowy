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
    [Authorize(Roles="manager")]
    public class AddProposalController : Controller
    {
        public ProposalRepository proposalRepository = new ProposalRepository();
        public List<FormAddEditProposal> model { get; set; } = new List<FormAddEditProposal>();

        public AddProposalController()
        {
            this.model.Add(new FormAddEditProposal());
        }

        [HttpGet("AddProposal/AddProposal")]
        public IActionResult AddProposal(int proposalId)
        {
            if (proposalId != 0)
            {
                FormAddEditProposal form = new FormAddEditProposal(proposalId);
                this.model = new List<FormAddEditProposal>();
                this.model.Add(form);
            }
            return View(model);
        }

        [HttpGet("AddProposal/AddProposalPost")]
        public IActionResult AddProposalPost(string client, string vehicle, string description, string result, string addAction)
        {

            return View(model);
        }
    }
}
