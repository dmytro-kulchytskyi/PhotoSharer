using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PhotoSharer.Business.Entities;
using System;

namespace PhotoSharer.Business.Managers
{
    public class AppUserManager : UserManager<AppUser, Guid>
    {
        public AppUserManager(IUserStore<AppUser, Guid> store) : base(store)
        {
            UserValidator = new UserValidator<AppUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            UserLockoutEnabledByDefault = false;
        }      
    }
}