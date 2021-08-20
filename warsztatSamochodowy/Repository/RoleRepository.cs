﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public class RoleRepository
    {
        private MyDbContext context;
        public RoleRepository()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
        .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
        .Options;

            context = new MyDbContext(contextOptions);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            var roleList = new List<Role>();
            var allRoles = await context.Role.ToListAsync();
            if (allRoles?.Any() == true)
            {
                foreach (var role in allRoles)
                {
                    roleList.Add( new Role()
                    {
                        CodeRole = role.CodeRole,
                        Name = role.Name,
                    });
                }
            }
            return roleList;
        }

        public List<Role> GetAllRoles()
        {
            return Task.Run(GetAllRolesAsync).Result;
        }


        public async Task<Dictionary<string,Role>> GetRoleDictionaryAsync()
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
            return Task.Run(GetRoleDictionaryAsync).Result;
        }
    }
}
