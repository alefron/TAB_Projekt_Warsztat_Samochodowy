using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Forms
{
    
    public class ActionSetFinalForm : InputFormBase
    {


        public ActionSetFinalForm()
        {

        }
        public ActionSetFinalForm(Models.Action action) {

            Id = action.Id;


        }



        [Key]
        public int Id { get; set; }

        [Display(Description ="Resultat")]
        [DataType(DataType.MultilineText)]
        public string ResultText { get; set; }

    }
}
