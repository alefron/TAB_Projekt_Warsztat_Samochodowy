using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ClientRepository
    {
        private MyDbContext context;
        public ClientRepository()
        {
                var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            context = new MyDbContext(contextOptions);
        }

        
        public async Task<List<Client>> GetAllClients()
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
    }
}
