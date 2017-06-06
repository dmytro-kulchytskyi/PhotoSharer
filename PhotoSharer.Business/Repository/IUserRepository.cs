using PhotoSharer.Business.Entities;
using System;

namespace PhotoSharer.Business.Repository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        AppUser GetUserByUserName(string userName);
        AppUser GetUserByLoginInfo(string loginProvider, string providerKey);
        bool IsUserInGroup(Guid userId, Guid groupId);
    }
}
