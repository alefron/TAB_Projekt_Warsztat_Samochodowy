using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Utils;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class VehicleController : Controller
    {
        private BrandRepository brandRepository = new BrandRepository();
        private VehicleRepository vehicleRepository = new VehicleRepository();
        private ClientRepository clientRepository = new ClientRepository();

        private List<Brand> brands { get; set; }
        private List<Vehicle> vehicles { get; set; }
        private List<Client> clients { get; set; }
        private List<FormVehicles> model { get; set; } = new List<FormVehicles>();

        public VehicleController()
        {
            this.brands = brandRepository.GetList();
            this.vehicles = vehicleRepository.GetJoinedVehicles();
            this.clients = clientRepository.GetAllClients();
            model.Add(new FormVehicles(brands, vehicles, clients));
        }

        [HttpGet("Vehicle/vehicleList")]
        public IActionResult VehicleList()
        {
            model.Add(new FormVehicles(brands, vehicles, clients));
            return View(model);
        }

        [HttpGet("Vehicle/getVehiclesFilteredByBrand")]
        public IActionResult getVehiclesFilteredByBrand(string brandName)
        {
            if (brandName != "wszystkie marki")
            {
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                foreach (var vehicle in vehicles)
                {
                    if (vehicle.Brand.Name == brandName)
                    {
                        vehiclesFiltered.Add(vehicle);
                    }
                }
                List<FormVehicles> modelFiltered = new List<FormVehicles>();
                modelFiltered.Add(new FormVehicles(brands, vehiclesFiltered, clients));
                return View("vehicleList", modelFiltered);
            }
            return View("vehicleList", this.model);
        }

        [HttpGet("Vehicle/getVehiclesFilteredBySearch")]
        public IActionResult getVehiclesFilteredBySearch(string searching)
        {
            if (searching != null)
            {
                List<FormVehicles> modelFiltered = new List<FormVehicles>();
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                foreach (var vehicle in this.vehicles)
                {
                    if (vehicle.Name.CaseInsensitiveContains(searching))
                    {
                        vehiclesFiltered.Add(vehicle);
                    }
                }
                modelFiltered.Add(new FormVehicles(brands, vehiclesFiltered, clients));
                return View("vehicleList", modelFiltered);
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
