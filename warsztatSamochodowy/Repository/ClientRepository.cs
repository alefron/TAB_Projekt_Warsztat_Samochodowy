using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ClientRepository: RepositoryBase<Client>
    {
        public ClientRepository()
        {
            dbSet = context.Clients;
        }

        
        public async Task<List<Client>> GetAllClientsAsync()
        {
            var clients = new List<Client>();
            var allclients = await context.Clients.ToListAsync();
            if (allclients?.Any() == true)
            {
                foreach (var client in allclients)
                {
                    clients.Add(new Client()
                    {
                        Id = client.Id,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        CompanyName = client.CompanyName,
                        PhoneNumber = client.PhoneNumber,
                        Email = client.Email,
                        AddressId = client.AddressId,
                        Address = client.Address,
                        Vehicles = client.Vehicles
                    });
                }
            }
            return clients;
        }

        public List<Client> GetAllClients()
        {
            return Task.Run(GetAllClientsAsync).Result;
        }

        public async Task<Client> getClientByIdAsync(int id)
        {
            return await context.Clients
                .Where((client) => client.Id == id)
                .FirstOrDefaultAsync();
        }

        public Client getClientById(int id)
        {
            return Task.Run(() => { return getClientByIdAsync(id); }).Result;
        }

    }
}
