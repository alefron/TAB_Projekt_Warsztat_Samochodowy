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
        private VehicleRepository vehicleRepository = new VehicleRepository();
        [HttpGet("Vehicle/vehicleList")]
        public IActionResult VehicleList()
        {
            List<Brand> brands = brandRepository.GetList();
            List<Vehicle> vehicles = vehicleRepository.GetJoinedVehicles();
            List<FormVehicles> model = new List<FormVehicles>();
            model.Add(new FormVehicles(brands, vehicles));
            return View(model);
        }

        [HttpGet("Vehicle/getVehiclesFilteredByBrand")]
        public IActionResult getVehiclesFilteredByBrand(string brandName)
        {
            List<Brand> brands = brandRepository.GetList();
            List<Vehicle> old_vehicles = vehicleRepository.GetJoinedVehicles();
            List<FormVehicles> model = new List<FormVehicles>();

            if (brandName == "wszystkie marki")
            {
                model.Add(new FormVehicles(brands, old_vehicles));
            }
            else
            {
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                foreach (var vehicle in old_vehicles)
                {
                    if (vehicle.Brand.Name == brandName)
                    {
                        vehiclesFiltered.Add(vehicle);
                    }
                }
                model.Add(new FormVehicles(brands, vehiclesFiltered));
                
            }
            return View("vehicleList", model);
        }

        [HttpGet("Vehicle/getVehiclesFilteredBySearch")]
        public IActionResult getVehiclesFilteredBySearch(string searching)
        {
            List<Brand> brands = brandRepository.GetList();
            List<Vehicle> old_vehicles = vehicleRepository.GetJoinedVehicles();
            List<FormVehicles> model = new List<FormVehicles>();

            if (searching == null)
            {
                model.Add(new FormVehicles(brands, old_vehicles));
            }
            else
            {
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                foreach (var vehicle in old_vehicles)
                {
                    if (vehicle.Name.Contains(searching))
                    {
                        vehiclesFiltered.Add(vehicle);
                    }
                }
                model.Add(new FormVehicles(brands, vehiclesFiltered));

            }
            return View("vehicleList", model);
        }
    }


    /*[HttpPost("/Vehicle/vehicleList")]
    public IActionResult getVehicleListSortedByBrand(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }*/
}
