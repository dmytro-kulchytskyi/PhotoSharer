using System;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities;
using System.Web;
using PhotoSharer.Business.Managers;
using PhotoSharer.Business.Services;

namespace PhotoSharer.MVC.Controllers
{
    [Authorize]
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

        public ActionResult Test()
        {
            return View("Properties");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
                return RedirectToAction("MyGroups", "Groups");

            var result = await signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.Failure:
                default:
                    var userName = string.Empty;

                    if (loginInfo.ExternalIdentity != null && !string.IsNullOrEmpty(loginInfo.ExternalIdentity.Name))
                        userName = loginInfo.ExternalIdentity.Name;

                    var user = userService.CreateUser(userName, loginInfo.Login);
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToLocal(returnUrl);
            }
        }

        [Authorize]
        public ActionResult LogOff()
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("MyGroups", "Groups");
        }

        #region Helpers
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
                return Redirect(returnUrl);

            return RedirectToAction("MyGroups", "Groups");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            private static readonly string XsrfKey = System.Configuration.ConfigurationManager.AppSettings["XsrfKey"];

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
                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
                if (this.UserId != null)
                    properties.Dictionary[XsrfKey] = this.UserId;

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }
        #endregion
    }
}