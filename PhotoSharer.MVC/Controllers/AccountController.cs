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
using System.Linq;
using System.Security.Claims;

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
                            if (loginInfo.ExternalIdentity != null)
                            {
                                if (!string.IsNullOrEmpty(loginInfo.ExternalIdentity.Name))
                                {
                                    userName = loginInfo.ExternalIdentity.Name;
                                }


                                if (loginInfo.ExternalIdentity.Claims != null && loginInfo.ExternalIdentity.Claims.Count() > 0)
                                {
                                    var userExternalIdClaim = loginInfo.ExternalIdentity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                                    if (userExternalIdClaim != null)
                                    {
                                        var userExternalId = userExternalIdClaim.Value;
                                        if (!string.IsNullOrEmpty(userExternalId))
                                        {
                                            //TODO
                                            //add search by externalId and update providerKey(token)


                                            var user = await userService.SaveUserAsync(userName);
                                            if (user != null)
                                            {
                                                var addLoginResult = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                                                if (addLoginResult.Succeeded)
                                                {
                                                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                                    return RedirectToLocal(returnUrl);
                                                }
                                            }
                                        }
                                    }
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
            private readonly string XsrfKey = System.Configuration.ConfigurationManager.AppSettings["XsrfKey"];

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