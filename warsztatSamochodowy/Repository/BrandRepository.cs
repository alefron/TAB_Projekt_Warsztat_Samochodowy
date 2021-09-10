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

        public async Task<Brand> GetByIDAsync(string id)
        {

            return await context.Brands
                .Where((brand) => brand.CodeBrand == id)
                .FirstOrDefaultAsync();
        }

        public Brand GetByID(string id)
        {
            return Task.Run(() => { return GetByIDAsync(id); }).Result;
        }



    }
}
