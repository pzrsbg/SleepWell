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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewReclamation(AllReclamationsViewModel model)
        {
            var reclamation = new Reclamation
            {
                DateCreated = DateTime.Now,
                Reason = model.NewReclamation.Reclamation.Reason,
                ReservationId = model.NewReclamation.Reclamation.ReservationId,
                Accepted = false
            };
            db.Reclamations.Add(reclamation);
            db.SaveChanges();

            return RedirectToAction("AllReclamations", true);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReclamation(int reclamationId, string action)
        {
            if(action == "accept")
            {
                var reclamation = db.Reclamations.Find(reclamationId);
                reclamation.DateFinished = DateTime.Now;
                reclamation.Accepted = true;
                db.SaveChanges();

                return RedirectToAction("AllReclamations");
            }
            else if (action == "discard")
            {
                var reclamation = db.Reclamations.Find(reclamationId);
                reclamation.DateFinished = DateTime.Now;
                reclamation.Accepted = false;
                db.SaveChanges();

                return RedirectToAction("AllReclamations");
            }
            else
            {
                return RedirectToAction("AllReclamations", false);
            }
        }

        //public ActionResult DeleteReclamation()
        //{
        //    return View();
        //}

        //public ActionResult ViewReclamation(int reservationId)
        //{
        //    return View();
        //}

        public ActionResult AllReclamations(bool? confirmSuccess)
        {
            var model = new AllReclamationsViewModel();

            if (confirmSuccess.HasValue)
            {
                model.Success = false;
                if (confirmSuccess.Value)
                    model.Success = true;
            }

            if (User.IsInRole("Manager"))
            {
                var allReclamations = db.Reclamations.OrderByDescending(r => r.DateCreated).ToList();
                model.Reclamations = allReclamations;

                return View(model);
            }
            else if (!(User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Receptionist")))
            {
                var userId = User.Identity.GetUserId();
                var userReclamations = db.Reclamations.Where(r => r.Reservation.UserId == userId).OrderByDescending(r => r.DateCreated).ToList();
                var userReservations = db.Reservations.Where(r => r.UserId == userId).OrderByDescending(r => r.ReservationId).ToList();
                var newReclamation = new NewReclamationViewModel
                {
                    Reservations = userReservations,
                };
                model.Reclamations = userReclamations;
                model.NewReclamation = newReclamation;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}