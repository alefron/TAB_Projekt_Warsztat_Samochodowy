using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class PersonelRepository: RepositoryBase<Personel>
    {
        public PersonelRepository()
        {
            base.dbSet = context.Personel;
        }

        

        public async Task<Personel> GetPersonelByIDAsync(int id)
        {

            return await context.Personel
                .Where((personel) => personel.Id == id)
                .FirstOrDefaultAsync();
        }

        public Personel GetPersonelByID(int id)
        {
            return Task.Run(() => { return GetPersonelByIDAsync(id); }).Result;
        }


        public async Task<List<Personel>> GetPersonelListByRoleAsync(string roleId)
        {
            return await context.Personel
                .Where((personel) => personel.RoleId == roleId)
                .ToListAsync();
        }

        public List<Personel> GetPersonelListByRole(string roleId)
        {
            return Task.Run(() => { return GetPersonelListByRoleAsync(roleId); }).Result;
        }


        public async Task<List<Personel>> GetJoinedPersonelAsync()
        {
            /*
            SELECT * FROM Personel
            JOIN Role
            JOIN Addresses
            */

            var queryResult =await context.Personel.Join<Personel, Role, string, Personel>(
                    context.Role,
                    personel => personel.RoleId,
                    role => role.CodeRole,
                    (personel, role) => new Personel
                    {
                        //Jak ktoś wie jak to zrobić lepiej to niech podzieli się wiedzą
                        //Bo wychodzi dużo przepisywania a ludzka lmbda nie działa
                        Id = personel.Id,
                        FirstName = personel.FirstName,
                        LastName = personel.LastName,
                        RoleId = personel.RoleId,
                        Email = personel.Email,
                        HashPassword = personel.HashPassword,//Chyba to jest niebezpieczne
                        PhoneNumber = personel.PhoneNumber,
                        AddressId = personel.AddressId,

                        Role = role, //I tutaj walimy tym joinem
                    }
                ).Join<Personel, Address, int, Personel>(
                    context.Addresses,
                    personel => personel.AddressId,
                    address => address.Id,
                    (personel, addresss) => new Personel
                    {
                        Id = personel.Id,
                        FirstName = personel.FirstName,
                        LastName = personel.LastName,
                        RoleId = personel.RoleId,
                        Email = personel.Email,
                        HashPassword = personel.HashPassword,//Chyba to jest niebezpieczne
                        PhoneNumber = personel.PhoneNumber,
                        AddressId = personel.AddressId,

                        Role = personel.Role, //I tutaj walimy tym joinem x2
                        Address = addresss
                    }
                ).ToListAsync();

            var personelList = new List<Personel>();

            if (queryResult?.Any() == true)
            {
                foreach (var person in queryResult)
                {
                    personelList.Add(new Personel()
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        RoleId = person.RoleId,
                        Role = person.Role,
                        PhoneNumber = person.PhoneNumber,
                        Email = person.Email,
                        AddressId = person.AddressId,
                        Address = person.Address,
                        HashPassword = person.HashPassword,
                        Proposals = person.Proposals,
                        Actions = person.Actions

                    });
                }
            }
            return personelList;
        }

        public List<Personel> GetJoinedPersonel()
        {
            return Task.Run(GetJoinedPersonelAsync).Result;
        }
    }
}

