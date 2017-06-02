using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Managers;
using PhotoSharer.Business.Repository;
using PhotoSharer.Business.Stores;
using System;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Services
{

    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly AppUserManager userManager;
        private readonly AppUserStore userStore;

        public UserService(IUserRepository userRepository,
                           AppUserManager userManager,
                           AppUserStore userStore)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.userStore = userStore;
        }


        public AppUser GetById(Guid id)
        {
            return userRepository.GetById(id);
        }

        public AppUser GetById(string id)
        {
            var userId = Guid.Parse(id);
            return GetById(id);
        }

        public async Task<AppUser> CreateUserAsync(string userName)
        {
            var user = new AppUser { UserName = userName };
            await userStore.CreateAsync(user);
            if (user.Id == null || user.Id == Guid.Empty)
                return null;

            return user;
        }
    }
}