using System;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities;
using PhotoSharer.MVC.ViewModels.Account;
using System.Web;
using PhotoSharer.Business.Managers;
using System.Security.Claims;
using System.Linq;
using PhotoSharer.Business.Stores;
using PhotoSharer.Business.Services;

namespace PhotoSharer.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly SignInManager<AppUser, Guid> signInManager;
        private readonly AppUserManager userManager;
        private readonly UserService userService;


        public AccountController(
            IAuthenticationManager authenticationManager,
            SignInManager<AppUser, Guid> signInManager,
            AppUserManager userManager,
            UserService userService)
        {
            this.userService = userService;
            this.authenticationManager = authenticationManager;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        public ActionResult Properties()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AddAccount");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddAccount()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {

            var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            if (!User.Identity.IsAuthenticated)
            {
                var result = await signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    case SignInStatus.Failure:
                    default:
                        {
                            var userName = string.Empty;

                            if (loginInfo.ExternalIdentity != null &&
                               !string.IsNullOrEmpty(loginInfo.ExternalIdentity.Name))
                            {
                                userName = loginInfo.ExternalIdentity.Name;
                            }

                            var user = await userService.CreateUserAsync(userName);
                            if (user != null)
                            {
                                var addLoginResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                                if (addLoginResult.Succeeded)
                                {
                                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                    return RedirectToLocal(returnUrl);
                                }
                            }
                            return RedirectToAction("Index", "Groups");
                        }
                }
            }
            else
            {
                var result = await userManager.AddLoginAsync(Guid.Parse(User.Identity.GetUserId()), loginInfo.Login);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                return RedirectToAction("Index", "Groups");
            }
        }


        /*
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Groups");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await authenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                string fullName;
                if (info.ExternalIdentity != null &&
                    !string.IsNullOrEmpty(info.ExternalIdentity.Name))
                {
                    fullName = info.ExternalIdentity.Name;
                }
                else
                {
                    fullName = model.Email;
                }

                Session["FullName"] = fullName;

                var user = new AppUser { UserName = model.Email, Email = model.Email, FullName = fullName };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        */


        [Authorize]
        public ActionResult LogOff()
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Groups");
        }

        #region Helpers
        //---

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Groups");
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            private readonly string XsrfKey = "XsrfKey";

            //System.Configuration.ConfigurationManager.AppSettings["XsrfKey"];

            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }


            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}