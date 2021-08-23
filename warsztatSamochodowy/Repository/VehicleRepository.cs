using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class VehicleRepository:RepositoryBase<Vehicle>
    {
        public VehicleRepository()
        {
            dbSet = context.Vehicles;
        }
    }
}
