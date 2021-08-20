using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public List<Personel> GetAllPersonel()
        {
            var data = Task.Run(() => personelRepository.GetAllPersonel()).Result;
            return data;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string password, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            List<Personel> personel = GetAllPersonel();
            foreach (var person in personel)
            {
                if (person.Email == email)
                {
                    if (person.HashPassword == password)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim("email", email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
                        if (person.RoleId == "MAN")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "manager"));
                        }
                        else if (person.RoleId == "ADM")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "admin"));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "worker"));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        Thread.CurrentPrincipal = claimsPrincipal;

                        return Redirect(returnUrl);

                    }
                }
            }
            TempData["Error"] = "Niepoprawny login lub hasło.";
            return View("~/Views/Login/Login.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
        
}
