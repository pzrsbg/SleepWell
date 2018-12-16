using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SleepWell.DAL;
using SleepWell.Models;
using SleepWell.ViewModels;

namespace SleepWell.Controllers
{
    [Authorize]
    public class BillController : Controller
    {
        private SleepWellContext db = new SleepWellContext();
        
        public ActionResult NewBill(Reservation reservation)
        {
            TimeSpan days = reservation.EndDate - reservation.StartDate;
            decimal calculatedTotal = reservation.Room.UnitCost * reservation.Persons * days.Days;

            var bill = new Bill
            {
                ReservationId = reservation.ReservationId,
                Total = calculatedTotal,
                PaymentState = PaymentState.Unpaid
            };
            db.Bills.Add(bill);
            db.SaveChanges();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EditBill(int billId, EditReservationViewModel reservation)
        {
            var bill = db.Bills.Find(billId);
            TimeSpan days = reservation.EndDate - reservation.StartDate;
            decimal roomUnitCost = db.Reservations.Find(reservation.ReservationId).Room.UnitCost;
            decimal recalculatedTotal = roomUnitCost * reservation.Persons * days.Days;

            bill.Total = recalculatedTotal;
            db.Entry(bill).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return new EmptyResult();
        }

        //public ActionResult DeleteBill()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult BillStatus(int billId, string action)
        {
            var bill = db.Bills.Find(billId);

            if (action == "acceptPayment")
            {
                bill.PaymentState = PaymentState.Paid;
                db.Entry(bill).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else if (action == "declinePayment")
            {
                bill.PaymentState = PaymentState.Unpaid;
                db.Entry(bill).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AllBills");
        }

        public ActionResult ViewBill(int reservationId)
        {
            var model = new Bill();
            model = db.Bills.Where(b => b.ReservationId == reservationId).Single();

            return View(model);
        }

        public ActionResult AllBills()
        {
            if (User.IsInRole("Receptionist"))
            {
                var allBills = db.Bills.OrderByDescending(b => b.BillId).ToList();
                var model = new AllBillsViewModel
                {
                    AllPaidBills = allBills.Where(b => b.PaymentState == PaymentState.Paid).ToList(),
                    AllUnPaidBills = allBills.Where(b => b.PaymentState == PaymentState.Unpaid).ToList()
                };
                return View(model);
            }
            else if (!(User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Receptionist")))
            {
                var UserId = User.Identity.GetUserId();
                var userBills = db.Bills.Where(b => b.Reservation.UserId == UserId).ToList();
                var model = new AllBillsViewModel
                {
                    UserBills = userBills
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }


}
