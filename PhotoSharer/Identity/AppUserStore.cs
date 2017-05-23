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
    public class AppUserStore : IUserStore<AppUser, Guid>
    {
        private readonly IUserRepository UserRepository;

        public AppUserStore(IUserRepository userRepository)
        {
            UserRepository = userRepository;
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

        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            return Task.FromResult(UserRepository.GetById(userId));
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            var user = UserRepository.GetByUserName(userName);
            return Task.FromResult(user);
        }

        public Task UpdateAsync(AppUser user)
        {
            UserRepository.Save(user);
            return Task.FromResult(true);
        }
    }
}