using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Security;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using System.Text;
using warsztatSamochodowy.Utils;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "worker")]
    public class WorkerController : Controller
    {

        ActionRepository actionRepository = new ActionRepository();
        ProposalRepository proposalRepository = new ProposalRepository();
        private List<Proposal> proposalsForWorker = new List<Proposal>();

        public IActionResult Index()
        {
            string? idnetifierString = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            if (idnetifierString == null)
            {
                return RedirectToAction("Logout", "Login");
            }

            int id = Int32.Parse(idnetifierString);



            var proposals = proposalRepository.GetProposalByWorker(id);
            //var actions= actionRepository.GetActionsForWorker(id);
            
            return View(proposals);
        }


        private string buildHeader(Models.Action action)
        {
            StringBuilder headerSb = new StringBuilder();


            if (action?.Proposal?.Vehicle != null)
            {
                headerSb.Append(action.Proposal.Vehicle.RegNumber);
                headerSb.Append(" - ");
            }
            if(action?.ActionType != null)
            {
                headerSb.Append(action.ActionType.Name);
                headerSb.Append(" - ");
            }
            if (action?.Proposal != null)
            {
                headerSb.Append(action.Description);
            }

            return headerSb.ToString();
        }



        public IActionResult BrowseAction(int id)
        {
            var action = actionRepository.GetActionById(id);

            return View(action);
        }

        [HttpGet]
        public IActionResult SetFinal(int id)
        {
            var action = actionRepository.GetActionById(id);

            if(action.Status == StatusEnum.CANCELED|| action.Status == StatusEnum.FINAL)
            {
                return View("BrowseAction", action);
            }

            ViewData["Header"] = buildHeader(action);

            return View(new ActionSetFinalForm(action));
        }

        [HttpPost]
        public IActionResult SetFinal(ActionSetFinalForm form)
        {
            var action = actionRepository.GetActionById(form.Id);



            action.EndDate = DateTime.Now;
            action.Result = form.ResultText;
            actionRepository.Update(action);
            action = actionRepository.SetActionStatus(form.Id, StatusEnum.FINAL);


            return View("BrowseAction", action);
        }


        [HttpGet]
        public IActionResult SetCancelled(int id)
        {
            
            var action = actionRepository.GetActionById(id);

            if (action.Status == StatusEnum.CANCELED || action.Status == StatusEnum.FINAL)
            {
                return View("BrowseAction", action);
            }
            ViewData["Header"] = buildHeader(action);

            return View(new ActionSetCancelledForm(action));
        }

        [HttpPost]
        public IActionResult SetCancelled(ActionSetCancelledForm form)
        {
            var action = actionRepository.GetActionById(form.Id);

            action.EndDate = DateTime.Now;
            action.Result = form.ResultText;
            actionRepository.Update(action);
            action = actionRepository.SetActionStatus(form.Id, StatusEnum.CANCELED);

            return View("BrowseAction", action);
        }













        [HttpGet("Worker/getProposalActionsComplete")]
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


        [HttpGet("Worker/getProposalsFilteredBySearch")]
        public IActionResult getProposalsFilteredBySearch(string searching)
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            this.proposalsForWorker = proposalRepository.GetProposalByWorker(Int32.Parse(id));
            if (searching != null)
            {
                List<Proposal> proposalsFiltered = new List<Proposal>();
                foreach (var prop in this.proposalsForWorker)
                {
                    string name = prop.VehicleId + " " + prop.StartDate.ToString();
                    if (name.CaseInsensitiveContains(searching))
                    {
                        proposalsFiltered.Add(prop);
                    }
                }

                return View("Index", proposalsFiltered);
            }
            return View("Index", this.proposalsForWorker);
        }

        [HttpGet("Worker/getProposalsFilteredBySatus")]
        public IActionResult getProposalsFilteredBySatus(int status)
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            this.proposalsForWorker = proposalRepository.GetProposalByWorker(Int32.Parse(id));
            List<Proposal> proposalsFiltered = new List<Proposal>();
            foreach (var prop in this.proposalsForWorker)
            {
                if ((int)prop.Status == status)
                {
                    proposalsFiltered.Add(prop);
                }
            }
            return View("Index", proposalsFiltered);
        }

    }
}
