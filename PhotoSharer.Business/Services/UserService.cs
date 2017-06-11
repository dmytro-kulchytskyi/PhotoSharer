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

        public UserService(IUserRepository userRepository, AppUserManager userManager, AppUserStore userStore)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.userStore = userStore;
        }

        public AppUser GetById(Guid id)
        {
            return userRepository.GetById(id);
        }

        public AppUser CreateUser(string userName, UserLoginInfo loginInfo)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                LoginProvider = loginInfo.LoginProvider,
                ProviderKey = loginInfo.ProviderKey
            };
            userRepository.Save(user);

            return user;
        }

        public bool IsUserExists(Guid userId)
        {
            return userRepository.IsExists(userId);
        }

        public bool IsInGroup(Guid userId, Guid groupId)
        {
            return userRepository.IsUserInGroup(userId, groupId);
        }
    }
}