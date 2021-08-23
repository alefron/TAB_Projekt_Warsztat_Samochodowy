using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerController : Controller
    {
        private PersonelRepository personelRepository = new PersonelRepository();

        public IActionResult Index()
        {
            List<Personel> list = personelRepository.GetJoinedPersonel();
            return View(list);
        }

        public IActionResult AddProposal()
        {
            return View();
        }

    }
}
