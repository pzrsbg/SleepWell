using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace SleepWell.Controllers
{

    public class BillController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private StoreContext db = new StoreContext();

        private ISessionManager sessionManager { get; set; }

        private ApplicationUserManager _userManager;

        public BillController(IMailService mailService, ISessionManager sessionManager)
        {
            this.mailService = mailService;

            this.sessionManager = sessionManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ShoppingBillManager shoppingBillManager = new ShoppingBillManager(this.sessionManager, this.db);

            var billItems = shoppingBillManager.GetBill();
            var billTotalPrice = shoppingBillManager.GetBillTotalPrice();

            BillViewModel billVM = new BillViewModel() { BillItems = billItems, TotalPrice = billTotalPrice };

            return View(billVM);
        }

        public ActionResult AddToBill(int id)
        {
            ShoppingBillManager shoppingBill = new ShoppingBillManager(this.sessionManager, this.db);
            shoppingBill.AddToBill(id);

            logger.Info("Added product {0} to bill", id);

            return RedirectToAction("Index", "Bill");
        }


        public int GetBillItemsCount()
        {
            ShoppingBillManager shoppingBillManager = new ShoppingBillManager(this.sessionManager, this.db);
            return shoppingBillManager.GetBillItemsCount();
        }

        public ActionResult RemoveFromBill(int roomID)
        {
            ShoppingBillManager shoppingBillManager = new ShoppingBillManager(this.sessionManager, this.db);

            int itemCount = shoppingBillManager.RemoveFromBill(roomID);
            int billItemsCount = shoppingBillManager.GetBillItemsCount();
            decimal billTotal = shoppingBillManager.GetBillTotalPrice();


            var result = new BillRemoveViewModel
            {
                RemoveItemId = roomID,
                RemovedItemCount = itemCount,
                BillTotal = billTotal,
                BillItemsCount = billItemsCount
            };

            return Json(result);
        }

        public async Task<ActionResult> Checkout()
        {
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var order = new Order
                {
                    FirstName = user.UserData.FirstName,
                    LastName = user.UserData.LastName,
                    Address = user.UserData.Address,
                    CodeAndCity = user.UserData.CodeAndCity,
                    Email = user.UserData.Email,
                    PhoneNumber = user.UserData.PhoneNumber
                };

                return View(order);
            }
            else
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Bill") });
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Order orderdetails)
        {
            if (ModelState.IsValid)
            {
                logger.Info("Checking out");

                var userId = User.Identity.GetUserId();

                ShoppingBillManager shoppingBillManager = new ShoppingBillManager(this.sessionManager, this.db);
                var newOrder = shoppingBillManager.CreateOrder(orderdetails, userId);


                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.UserData);
                await UserManager.UpdateAsync(user);

                shoppingBillManager.EmptyBill();


                var order = db.Orders.Include("OrderItems").Include("OrderItems.Room").SingleOrDefault(o => o.OrderId == newOrder.OrderId);


                this.mailService.SendOrderConfirmationEmail(order);

                return RedirectToAction("OrderConfirmation");
            }
            else
                return View(orderdetails);
        }

        public ActionResult OrderConfirmation()
        {
            return View();
        }

    }


}
