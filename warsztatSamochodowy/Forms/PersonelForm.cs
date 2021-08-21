using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


using warsztatSamochodowy.Models;
using warsztatSamochodowy.Security;

namespace warsztatSamochodowy.Forms
{
    public class PersonelForm: FormBase
    {
        public PersonelForm()
        {

        }
        public PersonelForm(Personel personel)
        {
            Id = personel.Id;
            FirstName = personel.FirstName;
            LastName = personel.LastName;
            RoleId = personel.RoleId;
            Email = personel.Email;
            ConfrimEmail = personel.Email;
            Password = "";
            ConfrimPassword = "";
            PhoneNumber = personel.PhoneNumber;

            Address  address= personel.Address;
            Street = address?.Street ?? "";
            HouseNumber = address?.HouseNumber ?? "";
            LocalNumber = address?.LocalNumber ?? "";
            City = address?.City ?? "";
            Postal = address?.Postal ?? "";

        }

        


        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Imię jest wymagane.")]
        [StringLength(100)]
        [Display()]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(3)]
        public string RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [EmailAddress]
        [Compare("Email")]
        public string ConfrimEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255,MinimumLength =3)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfrimPassword { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        //Hasher is used for hashing password
        Personel GetPersonel()
        {
            return new Personel()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                RoleId = this.RoleId,
                Email = this.Email,
                HashPassword = SecurityUtils.Hasher.GetHash(this.Password),
                PhoneNumber = this.PhoneNumber,
            };
        }


        //Założenie jest takie że przy stworzeniu pracownika 
        // jeśli nie istniał adres zostanie stworzony 
        //A jeśli istniał to zostnaie wzięty z db i przypisany  
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        public string LocalNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string Postal { get; set; }

        public Address GetAddres()
        {
            return new Address
            {
                Street = this.Street,
                HouseNumber = this.HouseNumber,
                LocalNumber = this.LocalNumber,
                City = this.City,
                Postal = this.Postal,

            };
        }


    }
}
