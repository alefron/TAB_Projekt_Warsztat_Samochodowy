using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Street { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string HouseNumber { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string LocalNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(6)")]
        public string Postal { get; set; }


        //jeden adres dla wielu klientów może być
        public ICollection<Client> Clients { get; set; }

        public string FormatedAddress
        {
            get => $"ul. {Street} {HouseNumber}/{LocalNumber} {City} {Postal}";
        }


    }
}
