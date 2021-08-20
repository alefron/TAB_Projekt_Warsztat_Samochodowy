using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class AddressRepository
    {
        private MyDbContext context;
        public AddressRepository()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
        .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
        .Options;

            context = new MyDbContext(contextOptions);
        }


        public async Task<Dictionary<int, Address>> GetAddressDictionaryAsync()
        {
            var addressDictionary = new Dictionary<int, Address>();
            var allAddresses = await context.Addresses.ToListAsync();
            if (allAddresses?.Any() == true)
            {
                foreach (var address in allAddresses)
                {
                    addressDictionary.Add(address.Id,new Address()
                    {
                        Id = address.Id,
                        Street = address.Street,
                        HouseNumber = address.HouseNumber,
                        LocalNumber = address.LocalNumber,
                        City = address.City,
                        Postal = address.Postal,
                    });
                }
            }
            return addressDictionary;
        }

        public Dictionary<int, Address> GetAddressDictionary()
        {
            var data = Task.Run(GetAddressDictionaryAsync).Result;
            return data;
        }
    }
}
