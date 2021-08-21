using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        { 
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).SingleOrDefault();


            if (role == "manager")
            {
                return Redirect("Manager");
            }
            if (role == "admin")
            {
                return Redirect("Admin");
            }
            if (role == "worker")
            {
                return Redirect("Worker");
            }
            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
