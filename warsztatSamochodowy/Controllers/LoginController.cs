using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Controllers
{
    public class LoginController : Controller
    {
        private PersonelRepository personelRepository = new PersonelRepository();


        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        public List<Personel> GetAllPersonel()
        {
            var data = Task.Run(() => personelRepository.GetAllPersonel()).Result;
            return data;
        }

        [HttpPost("login")]
        public IActionResult Validate(string email, string password)
        {
            List<Personel> personel = GetAllPersonel();
            foreach (var person in personel)
            {
                if (person.Email == email)
                {
                    if (person.HashPassword == password)
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest();

        }
    }
        
}
