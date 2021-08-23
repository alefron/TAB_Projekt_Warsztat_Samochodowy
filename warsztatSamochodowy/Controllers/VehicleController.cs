using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Forms;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class VehicleController : Controller
    {
        private BrandRepository brandRepository = new BrandRepository();
        private PersonelRepository personelRepository = new PersonelRepository();
        [HttpGet("Vehicle/vehicleList")]
        public IActionResult VehicleList(string firstName)
        {
            List<Brand> list = brandRepository.GetAllBrands();
            List<FormVehicles> model = new List<FormVehicles>();
            model.Add(new FormVehicles(list));
            return View(model);
        }

        [HttpGet("Vehicle/vehicleListRemoved")]
        public IActionResult VehicleListRemoved(string brandName)
        {
            List<Personel> list = personelRepository.GetJoinedPersonel();
            list.Remove(list.Find(e =>e.FirstName == brandName));
            return View("vehicleList", list);
        }
    }


    /*[HttpPost("/Vehicle/vehicleList")]
    public IActionResult getVehicleListSortedByBrand(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }*/
}
