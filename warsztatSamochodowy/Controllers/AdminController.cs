using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Rendering;

namespace warsztatSamochodowy.Controllers
{
    public class AdminController : Controller
    {

        private PersonelRepository personelRepository = new PersonelRepository();
        private RoleRepository roleRepository = new RoleRepository();
        //private AddressRepository addressesRepository = new AddressRepository();
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PersonelList()
        {
          
            List<Personel> allPersonel = personelRepository.GetJoinedPersonel();

           
            return View(allPersonel);
        }

        public IActionResult PersonelEdit(int id)
        {

            ViewData["RoleId"] =roleRepository.GetAllRoles().ToSelectListItems();
            return View(new Personel { Id = id,FirstName ="Janusz",LastName="Boss"});
        }
    }
}
