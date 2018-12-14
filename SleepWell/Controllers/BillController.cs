using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace SleepWell.Controllers
{

    public class BillController : Controller
    {

        public ActionResult BillsList()
        {
            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Bill> userBills;

            // For admin users - return all bills
            if (isAdmin)
            {
                userBills = db.Bills.Include("BillItems").
                    BillByDescending(o => o.DateCreated).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                userBills = db.Bills.Where(o => o.UserId == userId).Include("BillItems").
                    BillByDescending(o => o.DateCreated).ToArray();
            }

            return View(userBills);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public BillState ChangeBillState(Bill bill)
        {
            Bill billToModify = db.Bills.Find(bill.BillId);
            billToModify.BillState = bill.BillState;
            db.SaveChanges();

            if (billToModify.BillState == BillState.Shipped)
            {
      
            }


            return bill.BillState;
        }

    }


}
