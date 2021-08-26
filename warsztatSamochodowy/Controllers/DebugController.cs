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
        
        private ClientRepository clientRepository = new ClientRepository();
        private BrandRepository brandRepository = new BrandRepository();
        private VehicleTypeRepository vehicleTypeRepository = new VehicleTypeRepository();
        private ActionRepository actionRepository = new ActionRepository();
        private VehicleRepository vehicleRepository = new VehicleRepository();
        private ProposalRepository proposalRepository = new ProposalRepository();
        private ActionTypeRepository actionTypeRepository = new ActionTypeRepository();
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
        /*
        [HttpPost]
        public IActionResult ClearDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
               .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options;
            MyDbContext dbContext = new MyDbContext(contextOptions);

            ClearDatabase(dbContext);
            return Content("Database Cleared");
        }
        */


        [HttpPost]
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
            clientRepository.AddRange(defaultClients(addresList[3]));
            brandRepository.AddRange(defaultBrands());
            vehicleTypeRepository.AddRange(defaultVehicleTypes());

            List<Client> clientList = clientRepository.GetList();
            List<Brand> brandList = brandRepository.GetList();
            List<VehicleType> vehicleTypeList = vehicleTypeRepository.GetList();


            vehicleRepository.AddRange(defaultVehicles(clientList, brandList, vehicleTypeList));

            List<Vehicle> vechicleList = vehicleRepository.GetList();
            List<Personel> managerList = personelRepository.GetPersonelListByRole("MAN");
            List<Personel> workerList = personelRepository.GetPersonelListByRole("WOR");

            proposalRepository.AddRange(defaultProposals(vechicleList, managerList));
            List<Proposal> proposalList = proposalRepository.GetList();

            actionTypeRepository.AddRange(defaultActionTypes());
            List<ActionType> actionTypeList = actionTypeRepository.GetList(); 
            actionRepository.AddRange(defaultActions(proposalList, workerList, actionTypeList));


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
        private IEnumerable<Client> defaultClients(Address clientsAddres)
        {
            int clientCount = 6;
            //Add Clients
            for (int i = 0; i < clientCount; i++)
            {
                yield return new Client
                {
                    FirstName = $"Imie_Client{i}",
                    LastName = $"Nazw_Client{i}",
                    CompanyName = $"Firma{i}",
                    Email = $"cli{i}@tab.pl",
                    PhoneNumber = "444444444",
                    AddressId = clientsAddres.Id,
                };
            }
        }
        private IEnumerable<Brand> defaultBrands()
        {
            yield return new Brand
            {
                CodeBrand = "FIAT",
                Name = "Fiat"
            };
            yield return new Brand
            {
                CodeBrand = "OPEL",
                Name = "Opel"
            };
            yield return new Brand
            {
                CodeBrand = "TESLA",
                Name = "Tesla"
            };
            yield return new Brand
            {
                CodeBrand = "SUBARU",
                Name = "Sbaru"
            };
        }
        private IEnumerable<VehicleType> defaultVehicleTypes()
        {
            yield return new VehicleType
            {
                CodeType = "OSOBOWY",
                Name = "Samochód Osobowy"
            };
            yield return new VehicleType
            {
                CodeType = "AUTOBUS",
                Name = "Autobus"
            };
            yield return new VehicleType
            {
                CodeType = "TRICYKL",
                Name = "Tricykl"
            };
            yield return new VehicleType
            {
                CodeType = "BICYKL",
                Name = "Bicykl"
            };
        }
        private IEnumerable<Vehicle> defaultVehicles(List<Client> clinets, List<Brand> brands, List<VehicleType> types)
        {
            //1.5 Veh per client
            int vehicleCount = clinets.Count + clinets.Count / 2;
            for (int i = 0; i < vehicleCount; i++)
            {

                yield return new Vehicle
                {
                    RegNumber = $"REJ-TAB-{i}",
                    Name = $"Pojazd{i}",
                    ClientId = clinets[i % clinets.Count].Id,
                    VehicleTypeId = types[i% types.Count].CodeType,
                    BrandId = brands[i % brands.Count].CodeBrand,
                };
            }
        }
    
        private IEnumerable<Proposal> defaultProposals(List<Vehicle> vehicles,List<Personel> managers)
        {
            int proporsalCount = vehicles.Count;
            for (int i = 0; i < proporsalCount; i++)
            {
                yield return new Proposal
                {
                    Description = $"Opis{i}",
                    Result = $"Rezultat{i}",
                    Status = (StatusEnum)(i%4),
                    StartDate = new DateTime(2000,5,18,8,33,20),
                    EndDate = new DateTime(2005,2,4,21,37,00),
                    VehicleId=vehicles[i%vehicles.Count].RegNumber,
                    ManagerId=managers[i%managers.Count].Id,
                };
            }
        }
    
        private IEnumerable<ActionType> defaultActionTypes()
        {
            yield return new ActionType
            {
                CodeAction = "FIX",
                 Name = "Naprawa",
            };
            yield return new ActionType
            {
                CodeAction = "UPGRADE",
                Name = "Ulepszenie",
            };
            yield return new ActionType
            {
                CodeAction = "INSPECT",
                Name = "Przegląd",
            };
        }

        private IEnumerable<Models.Action> defaultActions(List<Proposal> proposals, List<Personel> workers,List<ActionType> actionTypes)
        {
            int actionCount = proposals.Count + proposals.Count / 2;

            for (int i = 0; i < actionCount; i++)
            {

                yield return new Models.Action
                {
                    Description = $"Opis{1}",
                    Result = $"Resultat{i}",
                    Status = (StatusEnum)( i%4 ),
                    StartDate = new DateTime(2000, 5, 18, 8, 33, 20),
                    EndDate = new DateTime(2005, 2, 4, 21, 37, 00),
                    WorkerId = workers[i % workers.Count].Id,
                    ProposalId = proposals[i % proposals.Count].Id,
                    ActionTypeId = actionTypes[i % actionTypes.Count].CodeAction,
                };
            }



        }





    }
}
#endif //DEBUG