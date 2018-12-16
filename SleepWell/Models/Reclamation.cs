using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.Models
{
    public class Reclamation
    {
        public int ReclamationId { get; set; }
        [Required]
        public int ReservationId { get; set; }
        [Required]
        [Display(Name = "Treść reklamacji")]
        public string Reason { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateFinished { get; set; }
        public bool Accepted { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}