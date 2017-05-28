using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;

namespace PhotoSharer.MVC.Stores
{
    public class AppUserStore : IUserStore<AppUser, Guid>, IUserLoginStore<AppUser, Guid>, IUserLockoutStore<AppUser, Guid>, IUserTwoFactorStore<AppUser, Guid>
    {
        private IUserRepository userRepository;
        private ILoginRepository loginRepository;

        public AppUserStore(IUserRepository userRepository,
            ILoginRepository loginRepository)
        {
            this.userRepository = userRepository;
            this.loginRepository = loginRepository;
        }



        public Task CreateAsync(AppUser user)
        {
            return Task.Run(() =>
            {
                userRepository.Save(user);
            });
        }


        public Task UpdateAsync(AppUser user)
        {
            return Task.Run(() =>
            {
                userRepository.Update(user);
            });
        }


        public Task DeleteAsync(AppUser user)
        {
            return Task.Run(() =>
            {
                userRepository.Delete(user);
            });
        }


        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            return Task.Run(() =>
            {
                var user = userRepository.GetById(userId);
                return user;
            });
        }


        public Task<AppUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var user = userRepository.GetByUserName(userName);
                return user;
            });
        }


        public Task AddLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return Task.Run(() =>
            {
                var login = new Login()
                {
                    LoginProvider = loginInfo.LoginProvider,
                    ProviderKey = loginInfo.ProviderKey,
                    User = user
                };
                loginRepository.Save(login);
            });
        }
       

        public Task<AppUser> FindAsync(UserLoginInfo login)
        {
            return Task.Run(() => 
            {
                var user = userRepository.GetByLoginInfo(login.LoginProvider, login.ProviderKey);
                return user;
            });  
        }


        public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user)
        {
            return Task.Run(() =>
            {
                var logins = loginRepository.GetByUserId(user.Id);
                IList<UserLoginInfo> loginInfoList = logins.Select(login => 
                    new UserLoginInfo(login.LoginProvider, login.ProviderKey)).ToList();

                return loginInfoList;
            });
        }
              

        public Task RemoveLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return Task.Run(() =>
            {
                var login = loginRepository.GetByLoginInfo(user.Id, loginInfo.LoginProvider, loginInfo.ProviderKey);
                if (login != null)
                {
                    loginRepository.Delete(login);
                }
            });
        }

        
        public void Dispose()
        {
            userRepository = null;
            loginRepository = null;
        }


        //---

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            return Task.FromResult(new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(0);
        }
    }
}