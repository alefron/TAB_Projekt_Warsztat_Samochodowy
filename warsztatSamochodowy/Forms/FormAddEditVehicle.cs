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
        ClientRepository clientRepository = new ClientRepository();
        VehicleRepository vehicleRepository = new VehicleRepository();
        BrandRepository brandRepository = new BrandRepository();
        VehicleTypeRepository vehicleTypeRepository = new VehicleTypeRepository();


        public List<Client> clients { get; set; } = new List<Client>();
        public Vehicle vehicle { get; set; } = new Vehicle();
        public Client client { get; set; } = new Client();
        public bool is_editable { get; set; }
        public List<Brand> brands { get; set; }
        public Brand brand { get; set; }
        public List<VehicleType> vehicleTypes { get; set; }
        public VehicleType vehicleType { get; set; }
        public int proposalId { get; set; }

        public FormAddEditVehicle()
        {
            this.is_editable = false;
            this.clients = clientRepository.GetAllClients();
            this.brands = brandRepository.GetList();
            this.vehicleTypes = vehicleTypeRepository.GetList();
        }

        public FormAddEditVehicle(string regNumber)
        {
            this.is_editable = true;

            this.vehicle = vehicleRepository.GetVehicleByRegNum(regNumber);
            this.client = clientRepository.getClientById(this.vehicle.ClientId);
            this.brand = brandRepository.GetByID(this.vehicle.BrandId);
            this.vehicleType = vehicleTypeRepository.GetVehicleByCode(this.vehicle.VehicleTypeId);

            this.clients = clientRepository.GetAllClients();
            this.brands = brandRepository.GetList();
            this.vehicleTypes = vehicleTypeRepository.GetList();
        }
    }
}
