﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    
    public class AddEditClientController : Controller
    {
        public ClientRepository clientRepository = new ClientRepository();
        private AddressRepository addressRepository = new AddressRepository();
        public List<FormAddEditClient> model { get; set; } = new List<FormAddEditClient>();

        public AddEditClientController()
        {
            FormAddEditClient form = new FormAddEditClient();
            model.Add(form);
        }

        [HttpGet("AddEditClient/AddClient")]
        public IActionResult AddClient()
        {
            return View("AddEditClient", model);
        }

        [HttpGet("AddEditClient/EditClient")]
        public IActionResult EditClient(int clientId)
        {
            FormAddEditClient form = new FormAddEditClient(clientId);
            this.model = new List<FormAddEditClient>();
            this.model.Add(form);
            return View("AddEditClient", model);
        }

        [HttpGet("AddEditClient/EditClientInDb")]
        public IActionResult EditClientInDb(
            string type,
            string firstName,
            string lastName,
            string companyName,
            string phoneNumber,
            string email,
            string street,
            string houseNumber,
            string localNumber,
            string city,
            string postal)
        {
            
            return RedirectToAction("Index", "Client");
        }

        [HttpGet("AddEditClient/AddClientToDb")]
        public IActionResult AddClientToDb(string type, string firstName, string lastName, string companyName, string phoneNumber, string email, string street, string houseNumber, string localNumber, string city, string postal)
        {
            int addressId = addressRepository.AddAddress(street, houseNumber, localNumber, city, postal);
            int clientId = clientRepository.AddClient(firstName, lastName, companyName, phoneNumber, email, addressId);
            return RedirectToAction("Index", "Client");
        }
    }
}
