using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Security;

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

        [HttpGet("AddProposal/AddProposalToDb")]
        public IActionResult AddProposalToDb(string client, string vehicle, string description, string result, int proposalId, int proposalIdAction)
        {
            if (client != null && vehicle != null && description != null)
            {
                var managerId = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                                        .Select(c => c.Value).SingleOrDefault();
                int newProposalId = 0;
                if (proposalId == -1 || proposalIdAction == -1)
                { 
                    newProposalId = proposalRepository.AddProposal(vehicle, description, Int32.Parse(managerId));
                }
                else
                {
                    if (proposalId == 0)
                    {
                        proposalId = proposalIdAction;
                    }
                    if (result == "Aby wpisać rezultat, ustaw stan zlecenia na \"zakończone\".")
                    {
                        result = null;
                    }
                    newProposalId = proposalRepository.UpdateProposal(vehicle, description, result, Int32.Parse(managerId), proposalId);
                }
               
                if (proposalIdAction != 0)
                {
                    return RedirectToAction("addAction", "Action", new { proposalId = newProposalId});
                }
                if (newProposalId != 0)
                {
                    return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = newProposalId });
                }
                else if (proposalId != 0)
                {
                    return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = proposalId });
                }
            }
            return RedirectToAction("proposalList", "Proposals");

        }
    }
}
