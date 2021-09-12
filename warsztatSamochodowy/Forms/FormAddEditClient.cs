using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddEditClient
    {
        ClientRepository clientRepository = new ClientRepository();
        AddressRepository addressRepository = new AddressRepository();


        public Client client { get; set; } = new Client();
        public Address address { get; set; } = new Address();
        public bool is_editable { get; set; }


        public FormAddEditClient(int clientId)
        {
            this.is_editable = true;
            this.client = this.clientRepository.getClientById(clientId);
            this.address = this.addressRepository.GetAddressById(this.client.AddressId);
        }

        public FormAddEditClient()
        {
            this.is_editable = false;
        }
    }
}
