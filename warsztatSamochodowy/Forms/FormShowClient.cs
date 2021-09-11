using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormShowClient
    {
        ClientRepository clientRepository = new ClientRepository();
        ProposalRepository proposalRepository = new ProposalRepository();
        AddressRepository addressRepository = new AddressRepository();
        public Client client { get; set; }
        public List<Proposal> proposals { get; set; } = new List<Proposal>();
        public Address address { get; set; }

        public FormShowClient(Client client)
        {
            this.client = client;
            this.proposals = proposalRepository.GetProposalByClient(client.Id);
            this.address = addressRepository.GetAddressById(client.AddressId);
        }

        public FormShowClient(int clientId)
        {
            this.client = clientRepository.getClientById(clientId);
            this.proposals = proposalRepository.GetProposalByClient(this.client.Id);
            this.address = addressRepository.GetAddressById(this.client.AddressId);
        }
    }
}
