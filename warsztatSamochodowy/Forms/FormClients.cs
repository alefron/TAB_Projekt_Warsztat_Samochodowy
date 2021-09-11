using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormClients
    {
        ClientRepository clientRepository = new ClientRepository();
        public List<Client> clients { get; set; }

        public FormClients(List<Client> clients)
        {
            this.clients = clients;
        }
    }
}