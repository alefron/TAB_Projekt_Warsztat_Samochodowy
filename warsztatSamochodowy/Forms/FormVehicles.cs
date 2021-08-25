using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;


namespace warsztatSamochodowy.Forms
{
    public class FormVehicles : FormBase
    {
        VehicleRepository vehicleRepository = new VehicleRepository();
        BrandRepository brandRepository = new BrandRepository();
        ClientRepository clientRepository = new ClientRepository();
        private List<Brand> brands { get; set; }
        private List<Vehicle> vehicles { get; set; }
        public List<Client> clients { get; set; }
        public string userToken { get; set; }

        public FormVehicles(List<Brand> brands, List<Vehicle> vehicles, List<Client> clients, string token)
        {
            this.clients = clients;
            this.brands = brands;
            this.vehicles = vehicles;
            this.userToken = token;
        }
        public List<Brand>getBrands()
        {
            return brands;
        }
        public List<Vehicle> getVehicles()
        {
            return vehicles;
        }
    }
}
