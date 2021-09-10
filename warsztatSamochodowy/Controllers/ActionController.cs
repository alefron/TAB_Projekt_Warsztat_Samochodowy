﻿using System;
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
    public class ActionController : Controller
    {
        private ActionRepository actionRepository = new ActionRepository();
        private List<FormAddEditAction> model { get; set; } = new List<FormAddEditAction>();

        [HttpGet("Action/addAction")]
        public IActionResult addAction(int proposalId)
        {
            FormAddEditAction form = new FormAddEditAction(proposalId);
            this.model.Add(form);
            return View("AddEditAction", model);
        }
        [HttpGet("Action/editAction")]

        public IActionResult editAction(int actionId)
        {
            Models.Action actToUpdate = actionRepository.GetActionById(actionId);
            FormAddEditAction form = new FormAddEditAction(actToUpdate);
            this.model.Add(form);
            return View("AddEditAction", model);
        }

        public IActionResult AddActionToDb(int proposalId, string type, int worker, int sequenceNumber, string description, string result)
        {
            int insertActionId = actionRepository.AddAction(proposalId, type, worker, sequenceNumber, description);
            return RedirectToAction("addProposal", "AddProposal", new { proposalId = proposalId });
        }

        bool isSequenceNumberOk(int sequenceNumber, int proposalId, int actionId)
        {
            int nextActionId = sequenceNumber;
            while (nextActionId != 0)
            {
                Models.Action a = actionRepository.GetActionById(nextActionId);
                if (a.SequenceNumber != actionId)
                {
                    if (a.SequenceNumber != 0)
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
