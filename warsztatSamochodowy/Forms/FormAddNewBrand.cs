using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Forms
{
    public class FormAddNewBrand
    {

        BrandRepository brandRepository = new BrandRepository();

        public List<Brand> brands { get; set; }
        public bool isEdit { get; set; }
        public string regNumber { get; set; }
        public FormAddNewBrand()
        {
            this.brands = brandRepository.GetList();
            this.isEdit = false;
        }
    }
}
