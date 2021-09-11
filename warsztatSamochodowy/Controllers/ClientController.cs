using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Utils;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class ClientController : Controller
    {
        private ClientRepository clientRepository = new ClientRepository();
        private List<FormClients> model { get; set; } = new List<FormClients>();
        private List<Client> clients { get; set; } = new List<Client>();

        public ClientController()
        {
            this.clients = clientRepository.GetJoinedClients();
            this.model.Add(new FormClients(clients));
        }

        public IActionResult Index()
        {

            return View(model);
        }

        [HttpGet("Clients/getClientsFilteredBySearch")]
        public IActionResult getClientsFilteredBySearch(string searching)
        {
            if (searching != null)
            {
                List<FormClients> modelFiltered = new List<FormClients>();
                List<Client> clientsFiltered = new List<Client>();
                foreach (var cli in this.clients)
                {
                    string name = "";
                    if (cli.CompanyName == null)
                    {
                        name = cli.FirstName + " " + cli.LastName;
                    }
                    else
                    {
                        name = cli.CompanyName;
                    }
                    if (name.CaseInsensitiveContains(searching))
                    {
                        clientsFiltered.Add(cli);
                    }
                }
                modelFiltered.Add(new FormClients(clientsFiltered));
                return View("Index", modelFiltered);
            }
            return View("Index", model);
        }
    }
}
