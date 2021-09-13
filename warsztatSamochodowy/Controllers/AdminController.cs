using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;
using warsztatSamochodowy.Rendering;
using warsztatSamochodowy.Forms;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.ComponentModel.DataAnnotations;
using warsztatSamochodowy.Security;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

        private PersonelRepository personelRepository = new PersonelRepository();
        private RoleRepository roleRepository = new RoleRepository();
        private AddressRepository addressRepository = new AddressRepository();
        private ProposalRepository proposalRepository = new ProposalRepository();
        //private AddressRepository addressesRepository = new AddressRepository();
        
        public IActionResult Index()
        {

            List<Personel> allPersonel = personelRepository.GetJoinedPersonel();
            return View(allPersonel);
        }



        [HttpGet("Admin/PersonelEdit")]
        public IActionResult PersonelEdit(int id)
        {

            Personel theChosenOne = personelRepository.GetJoinedPersonelById(id);
            if (theChosenOne == null)
            {
                //If it returns null just yeet 404 on this mo-fo
                return new NotFoundResult();
            }
            theChosenOne.HashPassword = "";
            PersonelEditForm personelForm = new PersonelEditForm(theChosenOne);

            //Get posible values for roleID dropdown
            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View(personelForm);


        }

        [HttpPost]
        [ActionName("PersonelEdit")]
        public IActionResult PersonelEdit_Post(PersonelEditForm personelForm)
        {
            
            List<ValidationResult> validationResults = new List<ValidationResult>(); ;
            if (personelForm.Validate(validationResults)==false)
            {
                //this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(validationResults.ToContentString());
            }
            Personel personel = personelForm.GetPersonel();
            personel.Address = personelForm.GetAddres();

            Personel currentEntry= personelRepository.GetJoinedPersonelById(personel.Id);

            if (personelForm.Password== null)
            {
                personel.HashPassword = currentEntry.HashPassword;
            }
            else
            {
                personel.HashPassword = SecurityUtils.Hasher.GetHash(personelForm.Password);
            }

            Address address = addressRepository.GetMatchcingAddress(personel.Address);

            if(address == null)
            {
                addressRepository.Add(personel.Address);
                address = addressRepository.GetMatchcingAddress(personel.Address);
            }

            personel.AddressId = address.Id;

            personelRepository.Update(personel);


            return View("PersonelEditConfirm", personel);


        }



        [HttpGet]
        public IActionResult PersonelCreate()
        {
            //Get posible values for roleID dropdown
            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View();
        }

        [HttpPost]
        public IActionResult PersonelCreate(PersonelCreateForm personelForm)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>(); ;
            if (personelForm.Validate(validationResults) == false)
            {
                //this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(validationResults.ToContentString());
            }
            Personel personel = personelForm.GetPersonel();
            personel.Address = personelForm.GetAddres();

            if (personelRepository.CanInsert(personel) == false)
            {
                ViewData["message"] = "Email already exists";
                return View("PersonelEdit", personelForm);
            }




            personel.HashPassword = SecurityUtils.Hasher.GetHash(personelForm.Password);

            Address address = addressRepository.GetMatchcingAddress(personel.Address);

            if (address != null)
            {
                personel.AddressId = address.Id;
                personel.Address = null;
            }

            personelRepository.Add(personel);


            ViewData["RoleId"] = roleRepository.GetList().ToSelectListItems();
            return View("PersonelCreateConfirm", personel);
        }

        [ActionName("PersonelDelete")]
        [HttpGet]
        public IActionResult PersonelDelete_Get(int id)
        {

            Personel theChosenOne = personelRepository.GetJoinedPersonelById(id);
            if (theChosenOne == null)
            {
                //If it returns null just yeet 404 on this mo-fo
                return new NotFoundResult();
            }
            theChosenOne.HashPassword = "";


            return View("PersonelDelete",theChosenOne);
        }
        [HttpDelete]
        [HttpPost]
        [ActionName("PersonelDelete")]
        public IActionResult PersonelDelete_Delete(int id)
        {
            Personel theChosenOne = personelRepository.GetJoinedPersonelById(id);
            if (theChosenOne == null)
            {
                //If it returns null just yeet 404 on this mo-fo
                return new NotFoundResult();
            }
            theChosenOne.HashPassword = "";

            try
            {
                personelRepository.Remove(theChosenOne);
            }
            catch (Exception e)
            {
                this.HttpContext.Response.StatusCode= (int)HttpStatusCode.ExpectationFailed;
                return  Content("Failed to delete from DB "+e.Message);
            }
            


            return View("PersonelDeleteConfirm", theChosenOne);
        }


        [HttpGet("Admin/getPersonelStatus")]
        public string getPersonelStatus(int personelId)
        {
            /*List<Proposal> proposals = proposalRepository.
            string current = "false";
            foreach (var proposal in proposals)
            {
                if (proposal.Status == StatusEnum.OPEN || proposal.Status == StatusEnum.PROCESSING)
                {
                    current = "true";
                    break;
                }
            }*/
            return "true";
        }

    }
}