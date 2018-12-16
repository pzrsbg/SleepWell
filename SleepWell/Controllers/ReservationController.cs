using Microsoft.AspNet.Identity;
using SleepWell.DAL;
using SleepWell.Models;
using SleepWell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SleepWell.Controllers
{
    public class ReservationController : Controller
    {
        private SleepWellContext db = new SleepWellContext();

        public ActionResult Index(DateTime? startDate, DateTime? endDate, int? persons, bool? error)
        {

            if (startDate.HasValue && endDate.HasValue && persons.HasValue)
            {
                var availableRooms =
                    db.Rooms.Where(r => r.MaxPeople >= persons)
                    .Where(r => r.Reservations.Where(s => (s.EndDate > startDate && s.EndDate <= endDate) || (s.StartDate < endDate && s.StartDate >= startDate))
                    .Count() == 0).ToList();

                var model = new RoomSearch
                {
                    Rooms = availableRooms,
                    StartDate = startDate,
                    EndDate = endDate,
                    Persons = persons
                };

                return View(model);
            }
            else
            {
                var allRooms = db.Rooms.ToList();
                var model = new RoomSearch
                {
                    Rooms = allRooms,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Persons = 2
                };

                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult NewReservation(DateTime startDate, DateTime endDate, int persons)
        {
            return RedirectToAction("Index", new { startDate, endDate, persons });
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult NewReservation(int roomId, DateTime startDate, DateTime endDate, int persons)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", true);
            }

            bool isNotAvailable = db.Reservations.Where(r => r.RoomId == roomId).Any(r => (r.EndDate > startDate && r.EndDate <= endDate) || (r.StartDate < endDate && r.StartDate >= startDate));

            if (!isNotAvailable)
            {
                var reservation = new Reservation
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Persons = persons,
                    RoomId = roomId,
                    UserId = User.Identity.GetUserId(),
                    ReservationState = ReservationState.New
                };
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("AllReservations", true);
            }
            else
            {
                return RedirectToAction("Index", true);
            }
            
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditReservation(int reservationId)
        {
            var reservation = db.Reservations.Find(reservationId);

            var model = new EditReservationViewModel
            {
                ReservationId = reservationId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Persons = reservation.Persons
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReservation(EditReservationViewModel reservation)
        {
            var updateReservation = db.Reservations.Find(reservation.ReservationId);
            bool modified = false;
            if(reservation.Persons != updateReservation.Persons && reservation.Persons <= updateReservation.Room.MaxPeople)
            {
                updateReservation.Persons = reservation.Persons;
                modified = true;
            }
            if(reservation.StartDate != updateReservation.StartDate)
            {
                if(updateReservation.StartDate < reservation.StartDate)
                {
                    updateReservation.StartDate = reservation.StartDate;
                    modified = true;
                }
            }
            if (reservation.EndDate != updateReservation.EndDate)
            {
                if (updateReservation.EndDate > reservation.EndDate)
                {
                    updateReservation.EndDate = reservation.EndDate;
                    modified = true;
                }
            }
            if (modified)
            {
                bool isBillAlreadyCreated = db.Bills.Any(b => b.ReservationId == reservation.ReservationId);
                if (isBillAlreadyCreated)
                {
                    int billId = db.Bills.Where(b => b.ReservationId == reservation.ReservationId).Single().BillId;
                    var controller = DependencyResolver.Current.GetService<BillController>();
                    var result = controller.EditBill(billId, reservation);
                }

                db.Entry(updateReservation).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllReservations");
            }

            return RedirectToAction("EditReservation", reservation.ReservationId);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReservation(int reservationId)
        {
            var reservationToDelete = db.Reservations.Find(reservationId);
            db.Reservations.Remove(reservationToDelete);
            db.SaveChanges();

            return RedirectToAction("AllReservations");
        }

        [Authorize(Roles = "Manager, Receptionist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationStatus(int reservationId, ReservationState reservationState)
        {
            var reservation = db.Reservations.Find(reservationId);
            reservation.ReservationState = reservationState;
            db.SaveChanges();

            if(reservationState == ReservationState.InProgress)
            {
                bool isBillAlreadyCreated = db.Bills.Any(b => b.ReservationId == reservationId);
                if (!isBillAlreadyCreated)
                {
                    var controller = DependencyResolver.Current.GetService<BillController>();
                    var result = controller.NewBill(reservation);
                }
            }

            return RedirectToAction("AllReservations");
        }

        
        public ActionResult ViewReservation()
        {
            

            return View();
        }

        [Authorize]
        public ActionResult AllReservations(bool? success)
        {

            if (User.IsInRole("Receptionist") || User.IsInRole("Manager"))
            {
                var allReservations = db.Reservations.OrderByDescending(r => r.StartDate).ToList();
                var model = new AllReservationsViewModel
                {
                    NewReservations = allReservations.Where(r => r.ReservationState == ReservationState.New).ToList(),
                    AcceptedReservations = allReservations.Where(r => r.ReservationState == ReservationState.Accepted).ToList(),
                    InProgressReservations = allReservations.Where(r => r.ReservationState == ReservationState.InProgress).ToList(),
                    CompletedReservations = allReservations.Where(r => r.ReservationState == ReservationState.Completed).ToList()
                };
                return View(model);
            }
            else if (!(User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Receptionist")))
            {
                var UserId = User.Identity.GetUserId();
                var userReservations = db.Reservations.Where(r => r.UserId == UserId).ToList();
                var model = new AllReservationsViewModel
                {
                    MyReservations = userReservations
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