using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Rendering
{
    public static class RoleRendering
    {
        public static SelectListItem ToSelectListItem(this Role role)
        {
            return new SelectListItem
            {
                Value = role.CodeRole,
                Text = role.Name,
            };
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Role> roles)
        {
            foreach (var role in roles)
            {
                yield return role.ToSelectListItem();
            }
        }

    }
}
