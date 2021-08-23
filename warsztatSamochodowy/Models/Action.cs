using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Models
{
    public class Action
    {


        public  Action Join( Personel worker, Proposal proposal, ActionType actionType){
            this.Worker = worker ?? this.Worker;
            this.Proposal = proposal??this.Proposal;
            this.ActionType = actionType ?? this.ActionType;
            return this;
        }

        

        [Key]
        public int Id { get; set; }

        //może być nullem - stąd "int?"
        public int? SequenceNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Result { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public StatusEnum Status { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        //worker który jest odpowiedzialny za tę czynność
        public int? WorkerId { get; set; }
        public Personel Worker { get; set; }

        //Czynność (Action) należy do jednego wniosku (Proposal)
        [Required]
        public int ProposalId { get; set; }
        public Proposal Proposal { get; set; }

        //Czynność jest jednego typu
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string ActionTypeId { get; set; }
        public ActionType ActionType { get; set; }
    }
}
