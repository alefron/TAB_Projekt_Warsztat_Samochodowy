using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Security;
using warsztatSamochodowy.Utils;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ProposalsController : Controller
    {
        private ProposalRepository proposalRepository = new ProposalRepository();
        private ActionRepository actionRepository = new ActionRepository();
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

        [HttpGet("Proposals/getProposalActionsComplete")]
        public string getProposalActionsComplete(int proposalId)
        {
            List<Models.Action> actions = actionRepository.GetActionsForProposal(proposalId);
            float countAll = actions.Count();
            int percent = 0;
            if (countAll != 0)
            {
                float countComplete = 0;
                foreach (var act in actions)
                {
                    if (act.Status == StatusEnum.CANCELED || act.Status == StatusEnum.FINAL)
                    {
                        countComplete++;
                    }
                }

                percent = (int)Math.Round(countComplete * 100 / countAll);
            }
            return percent.ToString() + "%";
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Proposals/getProposalsFilteredBySearch")]
        public IActionResult getProposalsFilteredBySearch(string searching)
        {
            if (searching != null)
            {
                List<FormProposals> modelFiltered = new List<FormProposals>();
                List<Proposal> proposalsFiltered = new List<Proposal>();
                foreach (var prop in this.proposals)
                {
                    string name = prop.VehicleId + " " + prop.StartDate.ToString();
                    if (name.CaseInsensitiveContains(searching))
                    {
                        proposalsFiltered.Add(prop);
                    }
                }
                modelFiltered.Add(new FormProposals(proposalsFiltered));
                return View("proposalList", modelFiltered);
            }
            return View("proposalList", model);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Proposals/getProposalsFilteredBySatus")]
        public IActionResult getProposalsFilteredBySatus(int status)
        {
            List<FormProposals> modelFiltered = new List<FormProposals>();
            List<Proposal> proposalsFiltered = new List<Proposal>();
            foreach (var prop in this.proposals)
            {
                if ((int)prop.Status == status)
                {
                    proposalsFiltered.Add(prop);
                }
            }
            modelFiltered.Add(new FormProposals(proposalsFiltered));
            return View("proposalList", modelFiltered);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Proposals/deleteProposal")]
        public IActionResult deleteProposal(int proposalId)
        {
            Proposal proposal = proposalRepository.GetProposalById(proposalId);
            proposalRepository.DeleteProposal(proposal);
            this.proposals = proposalRepository.GetJoinedProposals();
            this.model = new List<FormProposals>();
            this.model.Add(new FormProposals(this.proposals));
            return View("ProposalList", model);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Proposals/getOnlyManagerProposals")]
        public IActionResult getOnlyManagerProposals()
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            List<FormProposals> modelFiltered = new List<FormProposals>();
            List<Proposal> proposalsFiltered = new List<Proposal>();
            foreach (var prop in this.proposals)
            {
                if (prop.ManagerId.ToString() == id)
                {
                    proposalsFiltered.Add(prop);
                }
            }
            modelFiltered.Add(new FormProposals(proposalsFiltered));
            return View("proposalList", modelFiltered);
        }
        
    }
}
