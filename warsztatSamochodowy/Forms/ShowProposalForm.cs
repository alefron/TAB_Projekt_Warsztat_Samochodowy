using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class ShowProposalForm
    { 
        ProposalRepository proposalRepository = new ProposalRepository();
        ActionRepository actionRepository = new ActionRepository();
        ClientRepository clientRepository = new ClientRepository();
        VehicleRepository vehicleRepository = new VehicleRepository();
        PersonelRepository personelRepository = new PersonelRepository();

        public Proposal proposal { get; set; } = new Proposal();
        public List<Models.Action> actions { get; set; } = new List<Models.Action>();
        public Vehicle vehicle { get; set; } = new Vehicle();
        public Client client { get; set; } = new Client();
        public Personel meneger { get; set; } = new Personel();


        public int proposalId { get; set; }
        public ShowProposalForm(int proposalId)
        {
            this.proposalId = proposalId;
            this.proposal = proposalRepository.GetProposalById(proposalId);
            this.meneger = personelRepository.GetPersonelByID(this.proposal.ManagerId);
            this.actions = actionRepository.GetActionsForProposal(proposalId);
            this.vehicle = vehicleRepository.GetVehicleByRegNum(this.proposal.VehicleId);
            this.client = clientRepository.getClientById(this.vehicle.ClientId);
        }
    }
}
