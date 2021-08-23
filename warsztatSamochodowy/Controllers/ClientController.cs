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
    public class ClientController : Controller
    {
        private ClientRepository clientRepository = new ClientRepository();



        public IActionResult Index()
        {
            List<Client> clients = clientRepository.GetAllClients();

            return View(clients);
        }
    }
}
