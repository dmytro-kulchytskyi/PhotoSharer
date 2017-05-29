using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Managers;
using PhotoSharer.Business.Repository;
using System;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Services
{

    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly AppUserManager userManager;

        public UserService(IUserRepository userRepository,
                           AppUserManager userManager)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
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
            var createUserResult = await userManager.CreateAsync(user);
            if (createUserResult.Succeeded) return user;
            return null;
        }
    }
}