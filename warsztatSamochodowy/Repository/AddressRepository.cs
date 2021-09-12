using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class AddressRepository: RepositoryBase<Address>
    {
        public AddressRepository()
        {
            base.dbSet = context.Addresses;
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await context.Addresses
                .Where((address) => address.Id == addressId)
                .FirstOrDefaultAsync();
        }

        public Address GetAddressById(int addressId)
        {
            return Task.Run(() => { return GetAddressByIdAsync(addressId); }).Result;
        }

        public async Task<Address> GetMatchcingAddressAwait(Address address)
        {
            address.FormatMe();

            return await dbSet.Where(
                adr =>
                    adr.City == address.City &&
                    adr.Street == address.Street &&
                    adr.HouseNumber == address.HouseNumber &&
                    adr.LocalNumber == address.LocalNumber &&
                    adr.Postal == address.Postal
                ).FirstOrDefaultAsync();
        }

        public Address GetMatchcingAddress(Address address)
        {
            return Task.Run(() => { return GetMatchcingAddressAwait(address); }).Result;
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

        public async Task<int> AddAddressAsync(string street, string houseNumber, string localNumber, string city, string postal)
        {
            if (localNumber == "")
            {
                localNumber = "NULL";
            }
            Address address = new Address
            {
                Street = street,
                HouseNumber = houseNumber,
                LocalNumber = localNumber,
                City = city,
                Postal = postal
            };
            
            context.Add<Address>(address);
            await context.SaveChangesAsync();

            int newId = address.Id;

            return newId;
        }

        public int AddAddress(string street, string houseNumber, string localNumber, string city, string postal)
        {
            Task<int> t = Task.Run(() => { return AddAddressAsync(street, houseNumber, localNumber, city, postal); });
            var res = t.Result;
            return t.Result;
        }
    }
}
