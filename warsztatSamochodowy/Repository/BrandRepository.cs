using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class BrandRepository :RepositoryBase<Brand>
    {
        public BrandRepository()
        {
            dbSet = context.Brands;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            var brands = await context.Brands.ToListAsync();
            return brands;
        }

        public List<Brand> GetAllBrands()
        {
            return Task.Run(GetAllBrandsAsync).Result;
        }




    }
}
