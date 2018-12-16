using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SleepWell.ViewModels
{
    public class NewReclamationViewModel
    {
        public Reclamation Reclamation { get; set; }
        public List<Reservation> Reservations { get; set; }
    }

    public class AllReclamationsViewModel {
        public NewReclamationViewModel NewReclamation { get; set; }
        public List<Reclamation> Reclamations { get; set; }
        public bool? Success { get; set; }
    }
}