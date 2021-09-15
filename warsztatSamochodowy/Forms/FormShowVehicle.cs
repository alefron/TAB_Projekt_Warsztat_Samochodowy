using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormShowVehicle
    {

        VehicleRepository vehicleRepository = new VehicleRepository();
        BrandRepository brandRepository = new BrandRepository();
        ClientRepository clientRepository = new ClientRepository();
        VehicleTypeRepository vehicleTypeRepository = new VehicleTypeRepository();
        ProposalRepository proposalRepository = new ProposalRepository();
        public List<Proposal> proposals { get; set; }
        public Brand brand { get; set; }
        public Vehicle vehicle { get; set; }
        public Client client { get; set; }
        public VehicleType vehicleType { get; set; }

        public FormShowVehicle(string regNumber)
        {
            this.vehicle = vehicleRepository.GetVehicleByRegNum(regNumber);
            
            this.client = clientRepository.getClientById(vehicle.ClientId);
            this.brand = brandRepository.GetByID(vehicle.BrandId);
            this.proposals = proposalRepository.GetProposalByVehicle(regNumber);
            this.vehicleType = vehicleTypeRepository.GetVehicleByCode(vehicle.VehicleTypeId);
        }
    }
}
