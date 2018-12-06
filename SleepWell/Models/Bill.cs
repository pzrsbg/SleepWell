using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        [Required]
        public int ReservationId { get; set; }
        public decimal Total { get; set; }
        public PaymentState PaymentState { get; set; }

        public virtual Reservation Reservation { get; set; }
    }

    public enum PaymentState
    {
        Paid,
        Unpaid
    }
}