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
        private ProposalRepository proposalRepository = new ProposalRepository();
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

        [HttpGet("Client/getClientsFilteredBySearch")]
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

        
        [HttpGet("Client/getClientsFilteredByType")]
        public IActionResult getClientsFilteredByType(string type)
        {
            if (type == "osoba prywatna")
            {
                this.clients = clientRepository.getPersonsClient();
            }
            else if (type == "firma")
            {
                this.clients = clientRepository.getCompaniesClient();
            }
            this.model = new List<FormClients>();
            FormClients form = new FormClients(this.clients);
            this.model.Add(form);
            return View("Index", model);
        }

        [HttpGet("Client/clientListFilteredByProposalCount")]
        public IActionResult clientListFilteredByProposalCount(string sortingType)
        {
            if (sortingType != "wszyscy klienci")
            {
                List<FormClients> modelFiltered = new List<FormClients>();
                List<Client> clientsSorted = this.clients;

                clientsSorted.Sort((a, b) =>
                {
                    var proposalsA = proposalRepository.GetProposalByClient(a.Id);
                    var proposalsB = proposalRepository.GetProposalByClient(b.Id);
                    if (proposalsA.Count > proposalsB.Count)
                    {
                        if (sortingType == "sortuj rosnąco")
                            return 1;
                        return -1;
                    }
                    if (proposalsA.Count < proposalsB.Count)
                    {
                        if (sortingType == "sortuj malejąco")
                            return 1;
                        return -1;
                    }
                    return 0;
                });

                modelFiltered.Add(new FormClients(clients));
                return View("Index", modelFiltered);
            }
            return View("Index", model);
        }

        [HttpGet("Client/clientListFilteredByStatus")]
        public IActionResult clientListFilteredByStatus(string status)
        {
            if (status != "wszyscy klienci")
            {
                List<FormClients> modelFiltered = new List<FormClients>();
                List<Client> clientsFiltered = new List<Client>();
                List<Proposal> all_proposals = new List<Proposal>();
                foreach (var client in this.clients)
                {
                    all_proposals = proposalRepository.GetProposalByClient(client.Id);
                    foreach (var proposal in all_proposals)
                    {
                        if ((proposal.Status == StatusEnum.OPEN || proposal.Status == StatusEnum.PROCESSING) && status == "aktualnie obsługiwani")
                        {
                            clientsFiltered.Add(client);
                            break;
                        }
                        else if ((proposal.Status == StatusEnum.CANCELED || proposal.Status == StatusEnum.FINAL) && status == "dawniej obsługiwani")
                        {
                            clientsFiltered.Add(client);
                            break;
                        }
                        break;
                    }
                }
                modelFiltered.Add(new FormClients(clientsFiltered));
                return View("Index", modelFiltered);
            }
            return View("Index", model);
        }

        [HttpGet("Client/getProposalsCount")]
        public string getProposalsCount(int clientId)
        {
            return this.proposalRepository.GetProposalByClient(clientId).Count.ToString();
        }

        [HttpGet("Client/getClientStatus")]
        public string getClientStatus(int clientId)
        {
            List<Proposal> proposals = proposalRepository.GetProposalByClient(clientId);
            string current = "false";
            foreach(var proposal in proposals)
            {
                if (proposal.Status == StatusEnum.OPEN || proposal.Status == StatusEnum.PROCESSING)
                {
                    current = "true";
                    break;
                }
            }
            return current;
        }


        [HttpGet("Client/deleteClient")]
        public IActionResult deleteClient(int clientId)
        {
            Client client = clientRepository.getClientById(clientId);
            clientRepository.DeleteClient(client);
            this.clients = clientRepository.GetJoinedClients();
            this.model = new List<FormClients>();
            this.model.Add(new FormClients(this.clients));
            return RedirectToAction("Index", "Client");
        }

        [HttpGet("Client/showClient")]
        public IActionResult ShowClient(int clientId)
        {
            return View("Index", model);
        }
    }
}
