using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Manager/pojazdy")]
        public async Task<IActionResult> devicesPage()
        {
            return View("~/Views/Manager/ManagerPojazdy.cshtml");
        }
    }
}
