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
            return View("AddEditAction", model);
        }
    }
}
