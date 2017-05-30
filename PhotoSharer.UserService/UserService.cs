using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhotoSharer.Business.Managers;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PhotoSharer.Business.Services
{

    public class UserService<TUser, TUserId, TLogin>
    {
        //  private readonly IUserRepository userRepository;
        //  private readonly ILoginRepository loginRepository;
        private readonly AppUserManager userManager;


        public UserService(IUserRepository userRepository,
                           AppUserManager userManager,
                           ILoginRepository loginRepository
                          )
        {
            this.loginRepository = loginRepository;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }



        public async Task<AppUser> GetByIdAsync(Guid id)
        {
            return await Task.Run(() =>
            {
                return userRepository.GetById(id);
            });
        }

        public Task<Login> GetlLoginInfoAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> SaveUserAsync(string userName)
        {
            var neUser = new AppUser { UserName = userName };
            var createUserResult = await userManager.CreateAsync(neUser);
            if (createUserResult.Succeeded) return neUser;
            return null;
        }

        public AppUser UpdateUser(string externalId, ExternalLoginInfo loginInfo)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> UpdateUserAsync(string externalId, Login loginInfo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddLoginAsync(Guid userid, Login login)
        {
            throw new NotImplementedException();
        }

        public static bool IsAuthenticated
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var principal = context.User;
                    if (principal != null && principal.Identity != null)
                    {
                        return principal.Identity.IsAuthenticated;
                    }
                }

                return false;
            }
        }

        public static string UserName
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var principal = context.User;
                    if (principal != null && principal.Identity != null)
                    {
                        var name = principal.Identity.Name;
                        if (name != null)
                        {
                            return name;
                        }
                    }
                }

                return string.Empty;
            }
        }

        public static Guid GetUserId()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var principal = context.User;
                if (principal != null && principal.Identity != null)
                {
                    var userIdLine = principal.Identity.GetUserId();
                    if (!string.IsNullOrEmpty(userIdLine))
                    {
                        Guid id;
                        if (Guid.TryParse(userIdLine, out id))
                        {
                            return id;
                        }
                    }
                }
            }

            return Guid.Empty;
        }
    }
}