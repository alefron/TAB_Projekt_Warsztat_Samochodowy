using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Security;


#if DEBUG //Only build for debug
namespace warsztatSamochodowy.Controllers
{

    //Ten kontroler jest do debugowania
    public class DebugController : Controller
    {
        private RoleRepository roleRepository = new RoleRepository();
        private PersonelRepository personelRepository = new PersonelRepository();
        private AddressRepository addressRepository = new AddressRepository();

        private ActionRepository actionRepository = new ActionRepository();

        private void ClearDatabase(MyDbContext dbc)
        {
            dbc.Actions.RemoveRange(dbc.Actions);
            dbc.ActionTypes.RemoveRange(dbc.ActionTypes);
            dbc.Addresses.RemoveRange(dbc.Addresses);
            dbc.Brands.RemoveRange(dbc.Brands);
            dbc.Clients.RemoveRange(dbc.Clients);
            dbc.Personel.RemoveRange(dbc.Personel);
            dbc.Proposals.RemoveRange(dbc.Proposals);
            dbc.Role.RemoveRange(dbc.Role);
            dbc.Vehicles.RemoveRange(dbc.Vehicles);
            dbc.VehicleTypes.RemoveRange(dbc.VehicleTypes);

            dbc.SaveChanges();

        }

        public IActionResult ClearDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
               .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options;
            MyDbContext dbContext = new MyDbContext(contextOptions);

            ClearDatabase(dbContext);
            return Content("Database Cleared");
        }

        public IActionResult PopulateDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
               .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options;
            MyDbContext dbContext = new MyDbContext(contextOptions);

            ClearDatabase(dbContext);

            roleRepository.AddRange(defaultRoles());
            addressRepository.AddRange(defaultAddresses());
            List<Address> addresList = addressRepository.GetList();
            personelRepository.AddRange(defaultPersonel(addresList));




            return Content("Database Populated");
        }


       

        
        public IActionResult ActionsForWorker(int id)
        {
            var actions = actionRepository.GetActionsForWorker(id);
            return View(actions);
        }

        public IActionResult Index()
        {
            return View();
        }


        private IEnumerable<Role> defaultRoles()
        {
            yield return new Role
            {
                CodeRole = "MAN",
                Name = "Manager",
            };
            yield return new Role
            {
                CodeRole = "ADM",
                Name = "Admin",
            };
            yield return new Role
            {
                CodeRole = "WOR",
                Name = "Worker",
            };
        }

        private IEnumerable<Address> defaultAddresses()
        {
            yield return new Address
            {
                Street = "Pracownicza",
                HouseNumber = "1",
                LocalNumber = "1",
                City = "Miasto",
                Postal = "69-420",
            };
            yield return new Address
            {
                Street = "Kierownicza",
                HouseNumber = "2",
                LocalNumber = "2",
                City = "Miasto",
                Postal = "69-420",
            };
            yield return new Address
            {
                Street = "Administracyjna",
                HouseNumber = "3",
                LocalNumber = "3",
                City = "Miasto",
                Postal = "69-420",
            };
            yield return new Address
            {
                Street = "Kliencka",
                HouseNumber = "4",
                LocalNumber = "4",
                City = "Miasto",
                Postal = "69-420",
            };
        }
        private IEnumerable<Personel> defaultPersonel(List<Address> addresList)
        {
            int workerCount = 6;
            int managerCount = 3;
            int adminCount = 2;
            //Add Workers
            for (int i = 0; i < workerCount; i++)
            {
                yield return new Personel
                {
                    FirstName = $"Imie_Worker{i}",
                    LastName = $"Nazw_Worker{i}",
                    RoleId = "WOR",
                    Email = $"wor{i}@tab.pl",
                    HashPassword = SecurityUtils.Hasher.GetHash($"worker{i}"),
                    PhoneNumber = "111111111",
                    AddressId = addresList[0].Id,
                };
            }
            //Add Managers
            for (int i = 0; i < managerCount; i++)
            {
                yield return new Personel
                {
                    FirstName = $"Imie_Manager{i}",
                    LastName = $"Nazw_Manager{i}",
                    RoleId = "MAN",
                    Email = $"man{i}@tab.pl",
                    HashPassword = SecurityUtils.Hasher.GetHash($"manager{i}"),
                    PhoneNumber = "222222222",
                    AddressId = addresList[1].Id,
                };
            }
            //Add Admins
            for (int i = 0; i < adminCount; i++)
            {
                yield return new Personel
                {
                    FirstName = $"Imie_Admin{i}",
                    LastName = $"Nazw_Admin{i}",
                    RoleId = "ADM",
                    Email = $"adm{i}@tab.pl",
                    HashPassword = SecurityUtils.Hasher.GetHash($"admin{i}"),
                    PhoneNumber = "333333333",
                    AddressId = addresList[2].Id,
                };
            }
        }


    }
}
#endif //DEBUG