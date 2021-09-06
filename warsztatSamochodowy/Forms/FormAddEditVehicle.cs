using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddEditVehicle
    {
        ProposalRepository proposalRepository = new ProposalRepository();
        ActionRepository actionRepository = new ActionRepository();
        ClientRepository clientRepository = new ClientRepository();
        VehicleRepository vehicleRepository = new VehicleRepository();

        public Proposal proposal { get; set; } = new Proposal();
        public List<Models.Action> actions { get; set; } = new List<Models.Action>();
        public List<Client> clients { get; set; } = new List<Client>();
        public List<Vehicle> vehicles { get; set; } = new List<Vehicle>();
        public Vehicle vehicle { get; set; } = new Vehicle();
        public Client client { get; set; } = new Client();
        public bool is_editable { get; set; }

        public FormAddEditVehicle()
        {
            this.is_editable = false;
            this.clients = clientRepository.GetAllClients();
            this.vehicles = vehicleRepository.GetList();
        }

        public FormAddEditVehicle(Proposal proposal)
        {
            this.is_editable = true;
            this.proposal = proposal;
            this.actions = actionRepository.GetActionsForProposal(proposal.Id);

            this.clients = clientRepository.GetAllClients();
            this.vehicles = vehicleRepository.GetList();
            this.vehicle = vehicleRepository.GetVehicleByRegNum(this.proposal.VehicleId);
            this.client = clientRepository.getClientById(this.vehicle.ClientId);
        }

        public FormAddEditVehicle(int proposalId)
        {
            this.is_editable = true;
            this.proposal = proposalRepository.GetProposalById(proposalId);
            this.actions = actionRepository.GetActionsForProposal(proposalId);

            this.clients = clientRepository.GetAllClients();
            this.vehicles = vehicleRepository.GetList();
            this.vehicle = vehicleRepository.GetVehicleByRegNum(this.proposal.VehicleId);
            this.client = clientRepository.getClientById(this.vehicle.ClientId);
        }
    }
}
