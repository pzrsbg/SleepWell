using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SleepWell.ViewModels
{
    public class AllBillsViewModel
    {
        public List<Bill> UserBills { get; set; }
        public List<Bill> AllPaidBills { get; set; }
        public List<Bill> AllUnPaidBills { get; set; }
    }
}