using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PhotoSharer.App_Start;
using PhotoSharer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Identity
{
    public class AppUserManager : UserManager<AppUser, Guid>
    {
        public AppUserManager(IUserStore<AppUser, Guid> store) : base(store)
        {

        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = (AppUserManager)NinjectWebCommon.bootstrapper.Kernel.GetService(typeof(AppUserManager));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AppUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            // manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            // manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AppUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

    }
}