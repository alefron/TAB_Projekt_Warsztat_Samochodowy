using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public interface ISelectListItem
    {
        string GetSelectListValue();
        string GetSelectListText();

    }
}
