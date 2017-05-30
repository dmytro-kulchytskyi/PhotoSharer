using PhotoSharer.Business.Entities;
using System;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Services
{

    public interface IUserService
    {
        Task<AppUser> GetByIdAsync(Guid id);
        Task<AppUser> SaveUserAsync(string userName);
        Task<AppUser> UpdateUserAsync(string externalId, Login loginInfo);
        Task<bool> AddLoginAsync(Guid userid, Login login);
        Task<Login> GetlLoginInfoAsync();
        void SingOut();
        bool IsAuthenticated { get;  }
        string UserName { get; }


    }
}