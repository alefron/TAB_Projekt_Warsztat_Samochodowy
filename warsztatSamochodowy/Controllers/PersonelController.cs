using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Forms;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Security;

namespace warsztatSamochodowy.Controllers
{
    public class PersonelController : Controller
    {

        private AddressRepository addressRepository = new AddressRepository();
        private PersonelRepository personelRepository = new PersonelRepository();

        private List<FormAddEditPersonel> model { get; set; } = new List<FormAddEditPersonel>();
        public IActionResult AddEditPersonel()
        {
            return editMyaAccoount();
        }

        //editMyaAccoount
        public IActionResult editMyaAccoount()
        {
            int Id = Int32.Parse(User.Claims.Where(c => c.Type == CustomClaims.Identifier)
                                        .Select(c => c.Value).SingleOrDefault());
            model.Add(new FormAddEditPersonel(Id,false));
            return View(model);
        }
        [HttpGet]
        public IActionResult updateMyAccount(
            int addresId,
            int personelId,
            int role,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string street,
            string houseNumber,
            string localNumber,
            string city,
            string postal)
        {
            //  int addressId = addressRepository.;
            // int clientId = clientRepository.AddClient(firstName, lastName, companyName, phoneNumber, email, addressId);
            Address adres =  addressRepository.GetAddressById(addresId);
            adres.City = city;
            adres.HouseNumber = houseNumber;
            adres.LocalNumber = localNumber;
            adres.Postal = postal;
            adres.Street = street;
            addressRepository.Update(adres);

            Personel personel = personelRepository.GetJoinedPersonelById(personelId);
            personel.Email = email;
            personel.FirstName = firstName;
            personel.PhoneNumber = phoneNumber;
            personel.LastName = lastName;
            personelRepository.Update(personel);

            return RedirectToAction("Index", "Home") ;
        }

        //[Authorize(Roles = "administrator")]
        public IActionResult addNewAccount()
        {
            model.Add(new FormAddEditPersonel());
            return View(model);
        }

        //[Authorize(Roles = "administrator")]
        public IActionResult editAccount(int personelId)
        {
            model.Add(new FormAddEditPersonel(personelId,true));
            return View(model);
        }
    }
}
