using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;

namespace PhotoSharer.Business.Stores
{
    public class AppUserStore : IUserStore<AppUser, Guid>,
                                IUserLoginStore<AppUser, Guid>,
                                IUserLockoutStore<AppUser, Guid>,
                                IUserTwoFactorStore<AppUser, Guid>
    {
        private IUserRepository userRepository;

        public AppUserStore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task CreateAsync(AppUser user)
        {
            return Task.Run(() => userRepository.Save(user));
        }

        public Task UpdateAsync(AppUser user)
        {
            return Task.Run(() => userRepository.Update(user));
        }

        public Task DeleteAsync(AppUser user)
        {
            return Task.Run(() => userRepository.Delete(user));
        }

        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            return Task.Run(() => userRepository.GetById(userId));
        }

        public Task<AppUser> FindAsync(UserLoginInfo login)
        {
            return Task.Run(() => userRepository.GetUserByLoginInfo(login.LoginProvider, login.ProviderKey));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user)
        {
            var login = new UserLoginInfo(user.LoginProvider, user.ProviderKey);
            IList<UserLoginInfo> logins = new List<UserLoginInfo> { login };

            return Task.FromResult(logins);
        }

        public void Dispose()
        {
            userRepository = null;
        }

        //---
        public Task<AppUser> FindByNameAsync(string userName)
        {
            return Task.FromResult<AppUser>(null);
        }

        public Task AddLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return Task.FromResult(true);
        }

        public Task RemoveLoginAsync(AppUser user, UserLoginInfo loginInfo)
        {
            return Task.FromResult(true);
        }

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