﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;


namespace warsztatSamochodowy.Forms
{
    public class FormVehicles : FormBase
    {
        VehicleRepository vehicleRepository = new VehicleRepository();
        BrandRepository brandRepository = new BrandRepository();
        private List<Brand> brands { get; set; }
        private List<Vehicle> vehicles { get; set; }
        public FormVehicles(List<Brand> brands, List<Vehicle> vehicles)
        {
            this.brands = brands;
            this.vehicles = vehicles;
        }
        public List<Brand>getBrands()
        {
            return brands;
        }
        public List<Vehicle> getVehicles()
        {
            return vehicles;
        }
    }
}