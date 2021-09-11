using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Forms;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ShowProposalController : Controller
    {

        public List<ShowProposalForm> model { get; set; } = new List<ShowProposalForm>();

        [HttpGet("ShowProposal/ShowProposal")]
        public IActionResult ShowProposal(int proposalId)
        {
            this.model.Add(new ShowProposalForm(7));
            return View(model);
        }
    }
}
