using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Forms
{


    // Założenie jest takei że Forms będą wykorzystywane do obsługi widoku
    // Models będą wykorzystywane do obsługi bazy danych


    public abstract class FormBase
    {
        virtual public bool Validate( ICollection<ValidationResult>? results = null )
        {
            var validationContext = new ValidationContext(this, null, null);
            return Validator.TryValidateObject(this, validationContext, results, true);
        }
    }
}
