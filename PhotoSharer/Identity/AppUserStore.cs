using NHibernate;
using Microsoft.AspNet.Identity;
using PhotoSharer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using PhotoSharer.Models.Repository.Interface;

namespace PhotoSharer.Identity
{
    public class AppUserStore : IUserStore<AppUser, Guid>, IUserLoginStore<AppUser, Guid>, IUserLockoutStore<AppUser, Guid>, IUserTwoFactorStore<AppUser, Guid>
    {
        private readonly IUserRepository UserRepository;
        private readonly ILoginRepository LoginRepository;

        public AppUserStore(IUserRepository userRepository,
            ILoginRepository loginRepository)
        {
            UserRepository = userRepository;
            LoginRepository = loginRepository;
        }

        public Task AddLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            var login = new Login()
            {
                LoginProvider = loginInfo.LoginProvider,
                ProviderKey = loginInfo.ProviderKey,
                User = user
            };
            user.Logins.Add(login);
            LoginRepository.Save(login);
            return Task.FromResult(true);
        }

        public Task CreateAsync(AppUser user)
        {
            UserRepository.Save(user);
            return Task.FromResult(true);
        }

        public Task DeleteAsync(AppUser user)
        {
            UserRepository.Delete(user);
            return Task.FromResult(true);
        }

        public void Dispose()
        {

        }

        public Task<AppUser> FindAsync(UserLoginInfo login)
        {
            var user = LoginRepository.GetUserByLoginInfo(login);
            return Task.FromResult(user);
        }

        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            return Task.FromResult(UserRepository.GetById(userId));
        }


        public Task<AppUser> FindByNameAsync(string userName)
        {
            var user = UserRepository.GetByUserName(userName);
            return Task.FromResult(user);
        }

        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {

            //TODO
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            //TODO
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            //TODO
            return Task.FromResult(new DateTimeOffset());
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user)
        {
            IList<UserLoginInfo> logins = user.Logins.Select(login => new UserLoginInfo(login.LoginProvider, login.ProviderKey)).ToList();
            return Task.FromResult(logins);
        }

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            //TODO
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            var login = user.Logins.Where(l => l.LoginProvider == loginInfo.LoginProvider && l.ProviderKey == loginInfo.ProviderKey).ToList().FirstOrDefault();
            if (login != null)
            {
                LoginRepository.Delete(login);
            }
            return Task.FromResult(true);
        }

        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            //TODO
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            //TODO
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            //TODO
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            return Task.FromResult(true);
        }

        public Task UpdateAsync(AppUser user)
        {
            UserRepository.Update(user);
            return Task.FromResult(true);
        }
    }
}