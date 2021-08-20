using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Personel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required]
        public string RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string HashPassword { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(9)")]
        public string PhoneNumber { get; set; }

        //pracownik ma jeden adres, adres może należeć do wielu, rel. wiele do jeden
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        //wszystkie przyjęte wnioski - dot. menadżera
        public ICollection<Proposal> Proposals { get; set; }

        //wszystkie wykonane aktywności przy pojazdach - dot. workera
        public ICollection<Action> Actions { get; set; }


    }
}
