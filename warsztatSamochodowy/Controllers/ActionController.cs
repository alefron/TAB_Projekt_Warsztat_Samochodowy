using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Security;

namespace warsztatSamochodowy.Controllers
{
    
    public class ActionController : Controller
    {
        private ActionRepository actionRepository = new ActionRepository();
        private ProposalRepository proposalRepository = new ProposalRepository();
        private ActionTypeRepository actionTypeRepository = new ActionTypeRepository();

        private List<FormAddEditAction> model { get; set; } = new List<FormAddEditAction>();

        [Authorize(Roles = "manager")]
        [HttpGet("Action/addAction")]
        public IActionResult addAction(int proposalId)
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            FormAddEditAction form = new FormAddEditAction(proposalId, Int32.Parse(id));
            this.model.Add(form);
            return View("AddEditAction", model);
        }

        [Authorize(Roles = "manager,worker")]
        [HttpGet("Action/editAction")]

        public IActionResult editAction(int actionId)
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            Models.Action actToUpdate = actionRepository.GetActionById(actionId);
            FormAddEditAction form = new FormAddEditAction(actToUpdate, Int32.Parse(id));
            this.model.Add(form);
            return View("AddEditAction", model);
        }

        [Authorize(Roles = "manager")]
        public IActionResult AddActionToDb(int proposalId, string type, int worker, int sequenceNumber, string description, string result)
        {
            int insertActionId = actionRepository.AddAction(proposalId, type, worker, sequenceNumber, description);
            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = proposalId });
        }


        [Authorize(Roles = "worker,manager")]

        public IActionResult UpdateActionInDb(int actionId, string type, int worker, int sequenceNumber, string description, string result)
        {
            int insertActionId = actionRepository.UpdateAction(actionId, type, worker, sequenceNumber, description, result);
            Models.Action updatedAction = actionRepository.GetActionById(insertActionId);
            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = updatedAction.ProposalId });
        }


        [Authorize(Roles = "manager,worker")]

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






        private string buildHeader(Models.Action action)
        {
            StringBuilder headerSb = new StringBuilder();


            if (action?.Proposal?.Vehicle != null)
            {
                headerSb.Append(action.Proposal.Vehicle.RegNumber);
                headerSb.Append(" - ");
            }
            if (action?.ActionType != null)
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


        [Authorize(Roles = "manager,worker")]

        [HttpGet]
        public IActionResult SetFinal(int id)
        {
            var action = actionRepository.GetActionById(id);

            if (action.Status == StatusEnum.CANCELED || action.Status == StatusEnum.FINAL)
            {
                return View("BrowseAction", action);
            }

            ViewData["Header"] = buildHeader(action);

            return View("~/Views/Worker/SetFinal.cshtml", new ActionSetFinalForm(action));
        }

        [Authorize(Roles = "manager,worker")]

        [HttpPost]
        public IActionResult SetFinal(ActionSetFinalForm form)
        {
            var action = actionRepository.GetActionById(form.Id);



            action.EndDate = DateTime.Now;
            action.Result = form.ResultText;
            actionRepository.Update(action);
            action = actionRepository.SetActionStatus(form.Id, StatusEnum.FINAL);


            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = action.ProposalId });
        }

        [Authorize(Roles = "manager")]
        [HttpGet]
        public IActionResult SetCancelled(int id)
        {

            var action = actionRepository.GetActionById(id);

            if (action.Status == StatusEnum.CANCELED || action.Status == StatusEnum.FINAL)
            {
                return View("BrowseAction", action);
            }
            ViewData["Header"] = buildHeader(action);

            return View("~/Views/Worker/SetCancelled.cshtml", new ActionSetCancelledForm(action));
        }

        [Authorize(Roles = "manager")]
        [HttpPost]
        public IActionResult SetCancelled(ActionSetCancelledForm form)
        {
            var action = actionRepository.GetActionById(form.Id);

            action.EndDate = DateTime.Now;
            action.Result = form.ResultText;
            actionRepository.Update(action);
            action = actionRepository.SetActionStatus(form.Id, StatusEnum.CANCELED);

            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = action.ProposalId });
        }

        [Authorize(Roles = "manager")]
        [HttpGet]
        public IActionResult SetProcessingAgain(int id)
        {
            var action = actionRepository.GetActionById(id);

            action.EndDate = null;
            action.StartDate = DateTime.Now;
            actionRepository.Update(action);
            action = actionRepository.SetActionStatus(id, StatusEnum.PROCESSING);

            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = action.ProposalId });
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Action/addActionType")]
        public IActionResult addActionType(int proposalId, int actionId, bool isEdit)
        {
            List<FormAddActionType> list = new List<FormAddActionType>();
            list.Add(new FormAddActionType(proposalId,actionId, isEdit));
            return View(list);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Action/addActionTypeToDB")]
        public IActionResult addActionTypeToDB(int proposalId, int actionId, bool isEdit, string actionToAdd)
        {
            if(!String.IsNullOrEmpty(actionToAdd) && actionToAdd.Length >= 5 && actionTypeRepository.GetActionTypeById(actionToAdd.ToUpper().Substring(0, 5)) == null)
            {
                ActionType actrionTypeToAdd = new ActionType();
                actrionTypeToAdd.CodeAction = actionToAdd.ToUpper().Substring(0, 5);
                actrionTypeToAdd.Name = actionToAdd;
                actionTypeRepository.Add(actrionTypeToAdd);
            }

            if(isEdit)
                return RedirectToAction("editAction", "Action", new { actionId = actionId });
            else
                return RedirectToAction("addAction", "Action", new { proposalId = proposalId });
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Action/deleteActionFromDB")]
        public IActionResult deleteActionFromDB(int proposalId, int actionId, bool isEdit, string actionTodel)
        {
            ActionType action = actionTypeRepository.GetActionTypeById(actionTodel);

            if(action != null)
            {
                actionTypeRepository.Remove(action);
            }

            if (isEdit)
                return RedirectToAction("editAction", "Action", new { actionId = actionId });
            else
                return RedirectToAction("addAction", "Action", new { proposalId = proposalId });
        }


        [Authorize(Roles = "manager")]
        [HttpGet("Action/deleteAction")]
        public IActionResult deleteAction(int actionId)
        {
            Models.Action actionToUpdate = actionRepository.GetActionById(actionId);
            Proposal proposal = proposalRepository.GetProposalById(actionToUpdate.ProposalId);
            actionRepository.DeleteAction(actionToUpdate);

            return RedirectToAction("ShowProposal", "ShowProposal", new { proposalId = actionToUpdate.ProposalId});
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Action/actionTypeInActions")]
        public string actionTypeInActions(string actionId)
        {
            if (actionRepository.getAllActionByActionTypeId(actionId).Count == 0)
                return "false";
            else
                return "true";
        }

    }
}
