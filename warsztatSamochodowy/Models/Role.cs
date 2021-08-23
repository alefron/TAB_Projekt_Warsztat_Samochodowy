
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Role :ISelectListItem
    {
        [Key]
        [Column(TypeName = "nvarchar(3)")]
        public string CodeRole { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public ICollection<Personel> personel { get; set; }

        public string GetSelectListText()
        {
            return Name;
        }

        public string GetSelectListValue()
        {
            return CodeRole;
        }
    }
}
