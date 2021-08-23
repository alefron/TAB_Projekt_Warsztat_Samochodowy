﻿using Microsoft.EntityFrameworkCore;
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





    }
}
