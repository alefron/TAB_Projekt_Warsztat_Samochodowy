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

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "worker")]
    public class WorkerController : Controller
    {

        ActionRepository actionRepository = new ActionRepository();
        ProposalRepository proposalRepository = new ProposalRepository();




        public IActionResult Index()
        {
            string? idnetifierString = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            if (idnetifierString == null)
            {
                return RedirectToAction("Logout", "Login");
            }

            int id = Int32.Parse(idnetifierString);




            var actions= actionRepository.GetActionsForWorker(id);

            return View(actions);
        }


        private string buildHeader(Models.Action action)
        {
            StringBuilder headerSb = new StringBuilder();


            if (action?.Proposal?.Vehicle != null)
            {
                headerSb.Append(action.Proposal.Vehicle.RegNumber);
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

            return View("BrowseAction", action);
        }




    }
}
