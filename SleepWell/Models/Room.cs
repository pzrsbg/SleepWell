using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [Required]
        public int MaxPeople { get; set; }
        public string RoomName { get; set; }
        public decimal UnitCost { get; set; }
        public string PhotoUrl { get; set; }
        public RoomStandard RoomStandard { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }

    public enum RoomStandard
    {
        Economic,
        BedAndBreakfast,
        Exclusive
    }
}