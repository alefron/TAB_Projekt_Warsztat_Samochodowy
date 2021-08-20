using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Forms
{
    public class PersonelForm
    {
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string ConfrimEmail { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfrimPassword { get; set; }

        [Required]
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
                HashPassword = this.Password,
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
        [Required]
        public string LocalNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Postal { get; set; }

        Address GetAddres()
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
