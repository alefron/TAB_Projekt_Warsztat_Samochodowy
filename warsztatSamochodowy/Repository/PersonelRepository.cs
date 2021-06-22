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
    }
}

