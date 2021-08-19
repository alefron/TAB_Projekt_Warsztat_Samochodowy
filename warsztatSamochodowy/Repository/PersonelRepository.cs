using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class PersonelRepository
    {
        private MyDbContext context;
        public PersonelRepository()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
        .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
        .Options;

            context = new MyDbContext(contextOptions);
        }


        public async Task<List<Personel>> GetAllPersonel()
        {
            var personel = new List<Personel>();
            var allpersonel = await context.Personel.ToListAsync();
            if (allpersonel?.Any() == true)
            {
                foreach (var person in allpersonel)
                {
                    personel.Add(new Personel()
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
            return personel;
        }

        /*
            SELECT * FROM Personel
            LEFT JOIN Role
            LEFT JOIN Addresses
         */
        public async Task<List<Personel>> GetJoinedPersonelAsync()
        {
            
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

