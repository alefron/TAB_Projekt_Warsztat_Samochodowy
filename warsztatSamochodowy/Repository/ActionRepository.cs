using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class ActionRepository:RepositoryBase<Models.Action>
    {
        public ActionRepository()
        {
            base.dbSet = context.Actions;
        }



        public async Task<List<Models.Action>> GetActionsForWorkerAsync(int WorkerId)
        {
            /*
             SELECT * FROM Actions AS A
             JOIN Personel AS P
             ON A.WorkerId = P.Id
             */


            var actionList = await context.Actions.Join(
                    context.Personel,
                    action => action.WorkerId,
                    personel => personel.Id,
                    (action, personel) => action.Join(personel, null, null)
                ).ToListAsync();

            return actionList;
        }

        public List<Models.Action> GetActionsForWorker(int WorkerId)
        {
            return Task.Run(() => { return GetActionsForWorkerAsync(WorkerId); }).Result;
        }




    }
}
