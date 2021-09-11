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
    public class ShowClientController : Controller
    {
        private ClientRepository clientRepository = new ClientRepository();
        private List<FormShowClient> model { get; set; } = new List<FormShowClient>();
        private Client client { get; set; } = new Client();

        public IActionResult ShowClient(int clientId)
        {
            this.client = clientRepository.getClientById(clientId);
            FormShowClient form = new FormShowClient(clientId);
            this.model.Add(form);
            return View(model);
        }
    }
}
