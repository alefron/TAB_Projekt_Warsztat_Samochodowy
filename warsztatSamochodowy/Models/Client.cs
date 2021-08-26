using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Client:IModelFormattable
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CompanyName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(9)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        //foreign key
        //klient ma jeden adres, adres może należeć do wielu, rel. wiele do jeden
        public int AddressId { get; set; }
        public Address Address { get; set; }

        //klient ma wiele pojazdów, pojazd ma jednego właściciela, rel. jeden do wielu
        public ICollection<Vehicle> Vehicles { get; set; }

        public void FormatMe()
        {
            Email = Email.ToLower();
        }


        //musi być nazwa firmy lub nazwisko i imie - walidacja
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == null && LastName == null && CompanyName == null)
                yield return new ValidationResult("Należy podać nazwę firmy lub imię i nazwisko.");
        }*/


    }
}
