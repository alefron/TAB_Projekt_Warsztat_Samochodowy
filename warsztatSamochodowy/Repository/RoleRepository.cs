using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class RoleRepository: RepositoryBase<Role>
    {
        public RoleRepository()
        {
            base.dbSet = base.context.Role;
        }

        public async Task<Dictionary<string,Role>> GetDictionaryAsync()
        {
            var roleList = new Dictionary<string,Role>();
            var allRoles = await context.Role.ToListAsync();
            if (allRoles?.Any() == true)
            {
                foreach (var role in allRoles)
                {
                    roleList.Add(role.CodeRole, new Role()
                    {
                        CodeRole = role.CodeRole,
                        Name = role.Name,
                    });
                }
            }
            return roleList;
        }

        public Dictionary<string,Role> GetRoleDictionary()
        {
            return Task.Run(GetDictionaryAsync).Result;
        }
    }
}
