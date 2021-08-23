using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;


namespace warsztatSamochodowy.Forms
{
    public class FormVehicles : FormBase
    {
        BrandRepository BrandRepository = new BrandRepository();
        private List<Brand> brands { get; set; }
        private List<Vehicle> vehicles { get; set; }
        public FormVehicles(List<Brand> brands)
        {
            this.brands = brands;
        }
        public List<Brand>getBrands()
        {
            return brands;
        }
    }
}
