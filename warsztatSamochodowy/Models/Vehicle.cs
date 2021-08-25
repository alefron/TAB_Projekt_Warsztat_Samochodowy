using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Repository;

namespace warsztatSamochodowy.Models
{
    public class Vehicle
    {
        [Key]
        [Column(TypeName = "nvarchar(15)")]
        public string RegNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        //pojazd ma jednego właściciela
        public int ClientId { get; set; }
        public Client Client { get; set; }

        //pojazd jest jednego typu
        public string VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        //pojazd jest jednej marki
        public string BrandId { get; set; }
        public Brand Brand { get; set; }

        //pojazd może mieć wiele wniosków (w różnym czasie)
        public ICollection<Proposal> Proposals { get; set; }
    }
}
