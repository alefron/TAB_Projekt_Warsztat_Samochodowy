using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    public class AdminController : Controller
    {

        private PersonelRepository personelRepository = new PersonelRepository();
        //private RoleRepository roleRepository = new RoleRepository();
        //private AddressRepository addressesRepository = new AddressRepository();
        public List<Personel> GetAllPersonel()
        {
            var data = Task.Run(() => personelRepository.GetAllPersonel()).Result;
            return data;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PersonelList()
        {
          
            List<Personel> allPersonel = personelRepository.GetJoinedPersonel();

           
            return View(allPersonel);
        }
    }
}
