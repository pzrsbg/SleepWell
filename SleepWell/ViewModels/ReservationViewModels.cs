using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SleepWell.ViewModels
{
    public class RoomSearch
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Persons { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class EditReservationViewModel
    {
        public int ReservationId { get; set; }
        public int Persons { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AllReservationsViewModel
    {
        public List<Reservation> MyReservations { get; set; }
        public List<Reservation> NewReservations { get; set; }
        public List<Reservation> AcceptedReservations { get; set; }
        public List<Reservation> InProgressReservations { get; set; }
        public List<Reservation> CompletedReservations { get; set; }
    }
}