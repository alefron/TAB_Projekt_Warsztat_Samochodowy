using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ActionTypeRepository:RepositoryBase<ActionType>
    {
        public ActionTypeRepository()
        {
            dbSet = context.ActionTypes;
        }
    }
}
