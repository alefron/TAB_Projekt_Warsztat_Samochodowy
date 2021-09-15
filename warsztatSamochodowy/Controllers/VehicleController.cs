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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace warsztatSamochodowy.Controllers
{
    public class VehicleController : Controller
    {
        private BrandRepository brandRepository = new BrandRepository();
        private VehicleRepository vehicleRepository = new VehicleRepository();
        private ClientRepository clientRepository = new ClientRepository();
        private ProposalRepository proposalRepository = new ProposalRepository();

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

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/vehicleList")]
        public IActionResult VehicleList()
        {
            model.Add(new FormVehicles(brands, vehicles, clients));
            return View(model);
        }

        [Authorize(Roles = "manager")]
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

        [Authorize(Roles = "manager")]
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

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/vehicleListFilteredByOwner")]
        public IActionResult vehicleListFilteredByOwner(string owner)
        {
            if (owner != "wszyscy")
            {
                List<FormVehicles> modelFiltered = new List<FormVehicles>();
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                foreach (var vehicle in this.vehicles)
                {
                    string names = vehicle.Client.FirstName + " " + vehicle.Client.LastName;
                    if (vehicle.Client.CompanyName == owner || names == owner)
                    {
                        vehiclesFiltered.Add(vehicle);
                    }
                }
                modelFiltered.Add(new FormVehicles(brands, vehiclesFiltered, clients));
                return View("vehicleList", modelFiltered);
            }
            return View("vehicleList", model);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/vehicleListFilteredByStatus")]
        public IActionResult vehicleListFilteredByStatus(string status)
        {
            if (status != "wszystkie")
            {
                List<FormVehicles> modelFiltered = new List<FormVehicles>();
                List<Vehicle> vehiclesFiltered = new List<Vehicle>();
                List<Proposal> all_proposals = new List<Proposal>();
                foreach (var vehicle in this.vehicles)
                {
                    all_proposals = proposalRepository.GetProposalByVehicle(vehicle.RegNumber);
                    foreach (var proposal in all_proposals)
                    {
                        if ((proposal.Status == StatusEnum.OPEN || proposal.Status == StatusEnum.PROCESSING) && status == "w trakcie naprawy")
                        {
                            vehiclesFiltered.Add(vehicle);
                            break;
                        }
                        else if ((proposal.Status == StatusEnum.CANCELED || proposal.Status == StatusEnum.FINAL) && status == "dawniej naprawiane")
                        {
                            vehiclesFiltered.Add(vehicle);
                            break;
                        }
                        break;
                    }
                }
                modelFiltered.Add(new FormVehicles(brands, vehiclesFiltered, clients));
                return View("vehicleList", modelFiltered);
            }
            return View("vehicleList", model);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/vehicleListFilteredByProposalsCount")]
        public IActionResult vehicleListFilteredByProposalsCount(string sort)
        {
            if (sort != "wszystkie")
            {
                List<FormVehicles> modelFiltered = new List<FormVehicles>();
                List<Vehicle> vehiclesSorted = this.vehicles;

                vehiclesSorted.Sort((a, b) =>
                {
                    var proposalsA = proposalRepository.GetProposalByVehicle(a.RegNumber);
                    var proposalsB = proposalRepository.GetProposalByVehicle(b.RegNumber);
                    if (proposalsA.Count > proposalsB.Count)
                    {
                        if (sort == "sortuj rosnąco")
                            return 1;
                        return -1;
                    }
                    if (proposalsA.Count < proposalsB.Count)
                    {
                        if (sort == "sortuj malejąco")
                            return 1;
                        return -1;
                    }
                    return 0;
                });

                modelFiltered.Add(new FormVehicles(brands, vehiclesSorted, clients));
                return View("vehicleList", modelFiltered);
            }
            return View("vehicleList", model);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/getVehicleProposalsCount")]
        public string getVehicleProposalsCount(string regNumber)
        {
            int propCount = -1;
            if (regNumber != null)
            {
                propCount = proposalRepository.GetProposalByVehicle(regNumber).Count;
            }
            return propCount.ToString();
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/showVehicle")]
        public IActionResult showVehicle(string regNumber)
        {
            List<FormShowVehicle> modelToShowVechcle = new List<FormShowVehicle>();
            modelToShowVechcle.Add(new FormShowVehicle(regNumber));
            return View(modelToShowVechcle);
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/getVehicleStatus")]
        public bool getVehicleStatus(string regNumber)
        {
            bool status = false;
            if (regNumber != null)
            {
                List<Proposal> proposals = proposalRepository.GetProposalByVehicle(regNumber);
                foreach (var prop in proposals)
                {
                    if (prop.Status == StatusEnum.OPEN || prop.Status == StatusEnum.PROCESSING)
                    {
                        status = true;
                    }
                }
            }
            return status;
        }

        [Authorize(Roles = "manager")]
        [HttpGet("Vehicle/deleteVehicle")]
        public IActionResult deleteVehicle(string regNumber)
        {
            Vehicle vehicle = vehicleRepository.GetVehicleByRegNum(regNumber);
            vehicleRepository.DeleteVehicle(vehicle);
            this.vehicles = vehicleRepository.GetJoinedVehicles();
            this.model = new List<FormVehicles>();
            this.model.Add(new FormVehicles(this.brands, this.vehicles, this.clients));
            return RedirectToAction("VehicleList", "Vehicle");
        }

        [Authorize(Roles = "manager")]
        public IActionResult addNewBrand(bool isEdit, string regNumber)
        {
            List<FormAddNewBrand> list = new List<FormAddNewBrand>();
            list.Add(new FormAddNewBrand());
            list[0].isEdit = isEdit;
            list[0].regNumber = regNumber;
            return View(list);
        }

        [Authorize(Roles = "manager")]
        public IActionResult addNewBrandToDB(string brandToAdd, bool isEdit, string regNumber)
        {
            
            if (brandToAdd != null )
            {
                // dodawnaie marki
                string startIndex = brandToAdd.ToUpper().Substring(0, 3);
                if(brandRepository.GetByID(startIndex) != null)
                {
                    bool good = true;
                    while (brandRepository.GetByID(startIndex) != null)
                    {
                        if (brandRepository.GetByID(startIndex).Name.ToUpper() == brandToAdd.ToUpper())
                            good = false;
                        startIndex += "1";
                    }
                    if (good)
                    {
                        Brand brand = new Brand();
                        brand.CodeBrand = startIndex;
                        brand.Name = brandToAdd;
                        brandRepository.Add(brand);
                    }                    
                }
                else
                {
                    Brand brand = new Brand();
                    brand.CodeBrand = startIndex;
                    brand.Name = brandToAdd;
                    brandRepository.Add(brand);
                }
            }

            if (isEdit)
            {
                return RedirectToAction("EditVehicle", "AddVehicle", new { @regNumber = regNumber });
            }
            else
            {
                return RedirectToAction("AddVehicle", "AddVehicle");
            }
        }

        [Authorize(Roles = "manager")]
        public IActionResult deleteBrandFromDB(string brandToDel, bool isEdit, string regNumber)
        {
            if (brandToDel != null)
            {
                // usuwanie marki\
                var brand = brandRepository.GetByID(brandToDel);
                if (brand != null)
                {
                    brandRepository.Remove(brand);
                }
            }

            if (isEdit)
            {
                return RedirectToAction("EditVehicle", "AddVehicle", new { @regNumber = regNumber });
            }
            else
            {
                return RedirectToAction("AddVehicle", "AddVehicle");
            }
        }
        /*[HttpPost("/Vehicle/vehicleList")]
        public IActionResult getVehicleListSortedByBrand(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }*/
    }
}
