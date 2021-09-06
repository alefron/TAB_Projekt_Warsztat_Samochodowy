using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warsztatSamochodowy.Forms;

namespace warsztatSamochodowy.Controllers
{
    [Authorize(Roles = "manager")]
    public class AddVehicleController : Controller
    {
        public List<FormAddEditVehicle> model { get; set; } = new List<FormAddEditVehicle>();

        public AddVehicleController()
        {
            this.model.Add(new FormAddEditVehicle());
        }

        [HttpGet("AddVehicle/AddVehicle")]
        public IActionResult AddVehicle()
        {
            return View(model);
        }
    }
}
