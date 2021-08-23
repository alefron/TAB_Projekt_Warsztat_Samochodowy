using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Proposal
    {
        [Key]
        public int Id { get; set; }

        //opis podany przez klienta, to co należy zrobić
        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        //to co rzeczywiście zostało wykonane JĘZYKIEM BIZNESOWYM, DLA KLIENTA, wpisuje menadżer
        [Column(TypeName = "nvarchar(1000)")]
        public string Result { get; set; }

        //więcej w StatusEnum.cs
        public StatusEnum Status { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        //wniosek dotyczy jednego pojazdu
        public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        //wniosek jest przyjmowany przez jedną osobę (menadżer)
        public int ManagerId { get; set; }
        public Personel Manager { get; set; }

        //wniosek może wymagać wykonania wielu czynności
        public ICollection<Action> Actions { get; set; }

    }
}
