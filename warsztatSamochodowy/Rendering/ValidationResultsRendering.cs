using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Rendering
{
    public static class ValidationResultsRendering
    {
        public static string ToContentString(this IEnumerable<ValidationResult> validationResults)
        {
            StringBuilder sb = new StringBuilder();


            foreach (var result in validationResults)
            {
                sb.Append(result.ErrorMessage).Append(" \n");
            }

            return sb.ToString();
        }
    }
}
