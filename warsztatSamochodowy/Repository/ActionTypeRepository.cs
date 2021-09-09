using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ActionTypeRepository:RepositoryBase<ActionType>
    {
        public ActionTypeRepository()
        {
            dbSet = context.ActionTypes;
        }

        public List<ActionType> GetActionTypes()
        {
            return Task.Run(GetActionTypesAcync).Result;
        }

        public async Task<List<ActionType>> GetActionTypesAcync()
        {
            var types = new List<ActionType>();
            var typesFromDb = await context.ActionTypes.ToListAsync();
            if (typesFromDb?.Any() == true)
            {
                foreach (var type in typesFromDb)
                {
                    types.Add(new ActionType()
                    {
                        CodeAction = type.CodeAction,
                        Name = type.Name
                    });
                }
            }
            return types;
        }
    }
}
