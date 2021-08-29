using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;


namespace warsztatSamochodowy.Forms
{
    public class ActionSetCancelledForm : InputFormBase
    {



        public ActionSetCancelledForm()
        {

        }
        public ActionSetCancelledForm(Models.Action action)
        {
            Id = action.Id;
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [Display(Description = "Resultat")]
        [DataType(DataType.MultilineText)]
        public string ResultText { get; set; }

    }
}
