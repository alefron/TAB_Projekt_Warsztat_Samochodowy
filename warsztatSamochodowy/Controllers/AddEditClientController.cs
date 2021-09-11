﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class AddEditClientController : Controller
    {
        public IActionResult AddClient()
        {
            return View();
        }
    }
}
