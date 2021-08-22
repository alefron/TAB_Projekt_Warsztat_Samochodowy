using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Rendering;
using warsztatSamochodowy.Forms;
using Microsoft.AspNetCore.Authorization;


namespace warsztatSamochodowy.Controllers
{
    public class AdminController : Controller
    {

        private PersonelRepository personelRepository = new PersonelRepository();
        private RoleRepository roleRepository = new RoleRepository();
        //private AddressRepository addressesRepository = new AddressRepository();
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PersonelList()
        {

            List<Personel> allPersonel = personelRepository.GetJoinedPersonel();


            return View(allPersonel);
        }

        [HttpGet]
        public IActionResult PersonelEdit(int id)
        {



            Personel theChosenOne = personelRepository.GetPersonelByID(id);
            if (theChosenOne == null)
            {
                //If it returns null just yeet 404 on this mo-fo
                return new NotFoundResult();
            }

            //Get posible values for roleID dropdown
            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View(theChosenOne);


        }

        [HttpGet]
        public IActionResult PersonelCreate()
        {
            //Get posible values for roleID dropdown
            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View();
        }

        [HttpPost]
        public IActionResult PersonelCreate(PersonelForm personel)
        {
            //Get posible values for roleID dropdown
            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View();
        }



    }
}