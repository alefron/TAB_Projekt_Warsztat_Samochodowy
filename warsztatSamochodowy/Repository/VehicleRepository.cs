using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{

    public class VehicleRepository : RepositoryBase<Vehicle>
    {
        public VehicleRepository()
        {
            base.dbSet = context.Vehicles;
        }

        public async Task<Vehicle> GetVehicleByRegNumAsync(string reg)
        {
            return await context.Vehicles
                .Where((vehicle) => vehicle.RegNumber == reg)
                .FirstOrDefaultAsync();
        }

        public Vehicle GetVehicleByRegNum(string reg)
        {
            return Task.Run(() => { return GetVehicleByRegNumAsync(reg); }).Result;
        }

        public async Task<int> InsertVehicleAsync(Vehicle vehicle)
        {
            await context.Vehicles.AddAsync(vehicle);
            return await context.SaveChangesAsync();
        }

        public int InsertVehicle(Vehicle vehicle)
        {
            Task<int> t = Task.Run(() => { return InsertVehicleAsync(vehicle); });
            return t.Result;
        }

        public async Task<List<Vehicle>> GetJoinedVehiclesAsync()
        {
            /*
            SELECT * FROM Personel
            JOIN Clients
            JOIN VehicleTypes
            JOIN Brands
            */

            var queryResult = await context.Vehicles.Join<Vehicle, Client, int, Vehicle>(
                    context.Clients,
                    vehicle => vehicle.ClientId,
                    client => client.Id,
                    (vehicle, client) => new Vehicle
                    {
                        //Jak ktoś wie jak to zrobić lepiej to niech podzieli się wiedzą
                        //Bo wychodzi dużo przepisywania a ludzka lmbda nie działa
                        RegNumber = vehicle.RegNumber,
                        Name = vehicle.Name,
                        ClientId = vehicle.ClientId,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        BrandId = vehicle.BrandId,

                        Client = client,

                    }
                ).Join<Vehicle, VehicleType, string, Vehicle>(
                    context.VehicleTypes,
                    vehicle => vehicle.VehicleTypeId,
                    vehicleType => vehicleType.CodeType,
                    (vehicle, vehicleType) => new Vehicle
                    {
                        RegNumber = vehicle.RegNumber,
                        Name = vehicle.Name,
                        ClientId = vehicle.ClientId,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        BrandId = vehicle.BrandId,

                        Client = vehicle.Client,
                        VehicleType = vehicleType,
                    }
                ).Join<Vehicle, Brand, string, Vehicle>(
                    context.Brands,
                    vehicle => vehicle.BrandId,
                    brand => brand.CodeBrand,
                    (vehicle, brand) => new Vehicle
                    {
                        RegNumber = vehicle.RegNumber,
                        Name = vehicle.Name,
                        ClientId = vehicle.ClientId,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        BrandId = vehicle.BrandId,

                        Client = vehicle.Client,
                        VehicleType = vehicle.VehicleType,
                        Brand = brand,
                    }
                ).ToListAsync();

            var vehicleList = new List<Vehicle>();

            if (queryResult?.Any() == true)
            {
                foreach (var vehicle in queryResult)
                {
                    vehicleList.Add(new Vehicle()
                    {
                        RegNumber = vehicle.RegNumber,
                        Name = vehicle.Name,
                        ClientId = vehicle.ClientId,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        BrandId = vehicle.BrandId,

                        Client = vehicle.Client,
                        VehicleType = vehicle.VehicleType,
                        Brand = vehicle.Brand

                    });
                }
            }
            return vehicleList;
        }
    }
}
