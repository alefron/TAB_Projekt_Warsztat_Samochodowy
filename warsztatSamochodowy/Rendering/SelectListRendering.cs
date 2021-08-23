using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Rendering
{
    public static class SelectListRendering
    {

        public static SelectListItem ToSelectListItem(this ISelectListItem item)
        {
            return new SelectListItem
            {
                Value = item.GetSelectListValue(),
                Text = item.GetSelectListText(),
            };
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<ISelectListItem> items)
        {
            foreach (var item in items)
            {
                yield return item.ToSelectListItem();
            }
        }
    }
}
