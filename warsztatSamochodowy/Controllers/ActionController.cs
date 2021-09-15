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
    
    public class ActionController : Controller
    {
        private ActionRepository actionRepository = new ActionRepository();
        private List<FormAddEditAction> model { get; set; } = new List<FormAddEditAction>();

        [Authorize(Roles = "manager")]
        [HttpGet("Action/addAction")]
        public IActionResult addAction(int proposalId)
        {
            FormAddEditAction form = new FormAddEditAction(proposalId);
            this.model.Add(form);
            return View("AddEditAction", model);
        }

        [Authorize(Roles = "manager")]
        [Authorize(Roles = "worker")]
        [HttpGet("Action/editAction")]

        public IActionResult editAction(int actionId)
        {
            Models.Action actToUpdate = actionRepository.GetActionById(actionId);
            FormAddEditAction form = new FormAddEditAction(actToUpdate);
            this.model.Add(form);
            return View("AddEditAction", model);
        }

        [Authorize(Roles = "manager")]
        public IActionResult AddActionToDb(int proposalId, string type, int worker, int sequenceNumber, string description, string result)
        {
            int insertActionId = actionRepository.AddAction(proposalId, type, worker, sequenceNumber, description);
            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = proposalId });
        }


        [Authorize(Roles = "worker")]
        [Authorize(Roles = "manager")]
        public IActionResult UpdateActionInDb(int actionId, string type, int worker, int sequenceNumber, string description, string result)
        {
            int insertActionId = actionRepository.UpdateAction(actionId, type, worker, sequenceNumber, description, result);
            Models.Action updatedAction = actionRepository.GetActionById(insertActionId);
            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = updatedAction.ProposalId });
        }


        [Authorize(Roles = "manager")]
        [Authorize(Roles = "worker")]
        [HttpGet("Action/isSequenceNumberOk")]
        public bool isSequenceNumberOk(int sequenceNumber, int actionId)
        {
            int nextActionId = sequenceNumber;
            while (nextActionId != 0)
            {
                Models.Action a = actionRepository.GetActionById(nextActionId);
                if (a.SequenceNumber != actionId)
                {
                    if (a.SequenceNumber != null && a.SequenceNumber != -1)
                    {
                        nextActionId = (int)a.SequenceNumber;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        
    }
}
