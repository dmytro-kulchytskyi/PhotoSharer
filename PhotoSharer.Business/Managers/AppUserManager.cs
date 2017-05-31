using Microsoft.AspNet.Identity;
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
                RequireUniqueEmail = false
            };
            
            UserLockoutEnabledByDefault = false;
        }      
    }
}