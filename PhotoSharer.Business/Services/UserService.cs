using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;

namespace PhotoSharer.Business.Services
{

    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
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
    }
}