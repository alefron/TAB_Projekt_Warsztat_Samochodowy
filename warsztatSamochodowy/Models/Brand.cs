using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Brand
    {
        [Key]
        [Column(TypeName = "nvarchar(10)")]
        public string CodeBrand { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
