using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SleepWell.App_Start;
using SleepWell.Models;
using SleepWell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SleepWell.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController() { }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var profileInfo = new EditProfileViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                IsCompany = user.IsCompany,
                CompanyName = user.CompanyName,
                NIP = user.NIP
            };

            var model = new ManageViewModel
            {
                Message = message,
                EditProfileViewModel = profileInfo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(ManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.Email = model.EditProfileViewModel.Email;
                user.PhoneNumber = model.EditProfileViewModel.PhoneNumber;
                user.FirstName = model.EditProfileViewModel.FirstName;
                user.LastName = model.EditProfileViewModel.LastName;
                user.Address = model.EditProfileViewModel.Address;
                user.City = model.EditProfileViewModel.City;
                user.PostalCode = model.EditProfileViewModel.PostalCode;
                if (model.EditProfileViewModel.IsCompany)
                {
                    user.IsCompany = true;
                    user.CompanyName = model.EditProfileViewModel.CompanyName;
                    user.NIP = model.EditProfileViewModel.NIP;
                }
                var result = await UserManager.UpdateAsync(user);
            }
            else
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPassword([Bind(Prefix = "ChangePasswordViewModel")]EditPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors("password-error", result);

            // jeśli logowanie nie powiodło się:
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        // Inne
        private void AddErrors(string errorType, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(errorType, error);
            }
        }
    }
}