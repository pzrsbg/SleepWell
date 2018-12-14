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
    [Authorize]
    public class ReclamationController : Controller
    {
        private SleepWellContext db = new SleepWellContext();

        [HttpGet]
        public ActionResult NewReclamation()
        {
            var reclamation = new Reclamation
            {
                
            };

            return View();
        }

        public ActionResult EditReclamation()
        {
            return View();
        }

        public ActionResult DeleteReclamation()
        {
            return View();
        }

        public ActionResult ViewReclamation(int reservationId)
        {
            return View();
        }

        public ActionResult AllReclamations()
        {
            if (User.IsInRole("Manager"))
            {
                var allReclamations = db.Reclamations.OrderByDescending(r => r.DateCreated).ToList();
                var model = new AllReclamationsViewModel
                {
                    Reclamations = allReclamations
                };

                return View(model);
            }
            else if (!(User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Receptionist")))
            {
                var userReclamations = db.Reclamations.Where(r => r.Reservation.UserId == User.Identity.GetUserId()).OrderByDescending(r => r.DateCreated).ToList();
                var userReservations = db.Reservations.Where(r => r.UserId == User.Identity.GetUserId()).OrderByDescending(r => r.ReservationId).ToList();
                var newReclamation = new NewReclamationViewModel
                {
                    Reservations = userReservations,
                };
                var model = new AllReclamationsViewModel
                {
                    Reclamations = userReclamations,
                    NewReclamation = newReclamation
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}