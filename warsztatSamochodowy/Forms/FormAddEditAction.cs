using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddEditAction
    {
        ProposalRepository proposalRepository = new ProposalRepository();
        ActionRepository actionRepository = new ActionRepository();
        ClientRepository clientRepository = new ClientRepository();
        VehicleRepository vehicleRepository = new VehicleRepository();
        ActionTypeRepository actionTypeRepository = new ActionTypeRepository();
        PersonelRepository personelRepository = new PersonelRepository();

        public Proposal proposal { get; set; } = new Proposal();
        public List<Models.Action> actions { get; set; } = new List<Models.Action>();
        public Models.Action action { get; set; } = new Models.Action();
        public Vehicle vehicle { get; set; } = new Vehicle();
        public Client client { get; set; } = new Client();
        public bool is_editable { get; set; }
        public List<ActionType> types { get; set; } = new List<ActionType>();
        public List<Personel> workers { get; set; } = new List<Personel>();
        

        public FormAddEditAction(int proposalId)
        {
            this.is_editable = false;
            this.proposal = this.proposalRepository.GetProposalById(proposalId);
            this.vehicle = this.vehicleRepository.GetVehicleByRegNum(proposal.VehicleId);
            this.client = this.clientRepository.getClientById(vehicle.ClientId);
            this.actions = this.actionRepository.GetActionsForProposal(proposalId);
            this.types = this.actionTypeRepository.GetActionTypes();
            this.fillWorkers();
        }

        public FormAddEditAction(Models.Action action)
        {
            this.is_editable = true;
            this.action = action;
            this.proposal = this.proposalRepository.GetProposalById(action.ProposalId);
            this.actions = actionRepository.GetActionsForProposal(proposal.Id);
            this.vehicle = vehicleRepository.GetVehicleByRegNum(this.proposal.VehicleId);
            this.client = clientRepository.getClientById(this.vehicle.ClientId);
            this.types = this.actionTypeRepository.GetActionTypes();
            this.fillWorkers();


        }
        private int fillWorkers()
        {
            List<Personel> personel = personelRepository.GetList();
            foreach(var per in personel)
            {
                if (per.RoleId == "WOR")
                {
                    this.workers.Add(per);
                }
            }
            return this.workers.Count;
        }
    }
}
