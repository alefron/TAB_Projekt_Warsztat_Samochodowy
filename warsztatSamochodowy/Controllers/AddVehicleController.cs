using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class AddVehicleController : Controller
    {
        public List<FormAddEditVehicle> model { get; set; } = new List<FormAddEditVehicle>();

        VehicleRepository vehicleRepository = new VehicleRepository();
        BrandRepository brandRepository = new BrandRepository();

        public AddVehicleController()
        {
            
        }

        [HttpGet("AddVehicle/AddVehicle")]
        public IActionResult AddVehicle(int propasalId)
        {
            var added = new FormAddEditVehicle();
            added.proposalId = propasalId;
            this.model.Add(added);
            return View(model);
        }

        [HttpGet("AddVehicle/EditVehicle")]
        public IActionResult EditVehicle(string regNumber)
        {
            // ustaiwc editable na true
            // i jakos dostac sie do proposala
            var editVehicle = new FormAddEditVehicle(regNumber);
            this.model.Add(editVehicle);
            return View("AddVehicle", model);
        }

        [HttpGet("AddVehicle/AddVehicleToDB")]
        public IActionResult AddVehicleToDB(string brand, string regNumber, string type, int client, int propasalId)
        {
            // dodac zapisanie do bazy
            Vehicle vechicle = vehicleRepository.GetVehicleByRegNum(regNumber);
            if (vechicle == null)
            {
                //add
                Vehicle newVehicle = new Vehicle();
                newVehicle.BrandId = brand;
                newVehicle.RegNumber = regNumber;
                newVehicle.VehicleTypeId = type;
                newVehicle.ClientId = client;
                newVehicle.Name = regNumber + "_" + brandRepository.GetByID(brand).Name;
            vehicleRepository.Add(newVehicle);
            }
            else
            {
                vechicle.BrandId = brand;
                vechicle.RegNumber = regNumber;
                vechicle.VehicleTypeId = type;
                vechicle.ClientId = client;
                vechicle.Name = regNumber + "_" + brandRepository.GetByID(brand);
                vehicleRepository.Update(vechicle);
            }
            return RedirectToAction("AddProposal", "AddProposal", new { proposalId = propasalId });
        }


   
    public IActionResult UpdateVehicle(string brand, string regNumber, string type, int client)
        {
            // dodac zapisanie do bazy
            Vehicle vechicle = vehicleRepository.GetVehicleByRegNum(regNumber);
            
            vechicle.BrandId = brand;
            vechicle.RegNumber = regNumber;
            vechicle.VehicleTypeId = type;
            vechicle.ClientId = client;
            vechicle.Name = regNumber + "_" + brand;
            vehicleRepository.Update(vechicle);
            
            return RedirectToAction("showVehicle", "Vehicle", new { regNumber = regNumber });
        }
    }
}
