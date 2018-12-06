using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int Persons { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ReservationState ReservationState { get; set; }

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }

    public enum ReservationState
    {
        New,
        Accepted,
        InProgress,
        Completed
    }
}