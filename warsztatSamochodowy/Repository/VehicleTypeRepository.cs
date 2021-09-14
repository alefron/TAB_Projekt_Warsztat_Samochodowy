using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class VehicleTypeRepository:RepositoryBase<VehicleType>
    {
        public VehicleTypeRepository()
        {
            dbSet = context.VehicleTypes;
        }


        public async Task<VehicleType> GetVehicleByCodeAsynch(string CodeType)
        {
            return await context.VehicleTypes
                .Where((vehicle) => vehicle.CodeType == CodeType)
                .FirstOrDefaultAsync();
        }

        public VehicleType GetVehicleByCode(string CodeType)
        {
            return Task.Run(() => { return GetVehicleByCodeAsynch(CodeType); }).Result;
        }
    }
}
