using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Threading.Tasks;
using PhotoSharer.Models;
using Microsoft.AspNet.Identity.Owin;

namespace PhotoSharer.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAuthenticationManager AuthenticationManager;
        private readonly SignInManager<AppUser, Guid> SignInManager;


        public ActionResult Login()
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
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)

            {

                return RedirectToAction("Login");

            }



            // Sign in the user with this external login provider if the user already has a login

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)

            {

                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.RequiresVerification:

                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

                case SignInStatus.Failure:

                default:

                    // If the user does not have an account, then prompt the user to create an account

                    ViewBag.ReturnUrl = returnUrl;

                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });

            }

        }


        #region Helpers
        private readonly string XsrfKey = System.Configuration.ConfigurationManager.AppSettings["XsrfKey"];

        internal class ChallengeResult : HttpUnauthorizedResult
        {
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
            #endregion

        }
    }
}