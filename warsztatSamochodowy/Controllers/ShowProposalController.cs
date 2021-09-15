using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Security;

namespace warsztatSamochodowy.Controllers
{
    public class ShowProposalController : Controller
    {

        public List<ShowProposalForm> model { get; set; } = new List<ShowProposalForm>();

        [Authorize(Roles = "manager,worker")]
        [HttpGet("ShowProposal/ShowProposal")]
        public IActionResult ShowProposal(int proposalId)
        {
            var id = User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                .Select(c => c.Value).SingleOrDefault();

            this.model.Add(new ShowProposalForm(proposalId, Int32.Parse(id)));
            return View(model);
        }
    }
}
