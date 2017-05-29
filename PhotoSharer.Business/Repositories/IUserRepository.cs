using PhotoSharer.Business.Entities;

namespace PhotoSharer.Business.Repository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        AppUser GetByUserName(string userName);
        AppUser GetByLoginInfo(string loginProvider, string providerKey);
        AppUser GetByEmail(string email);
    }
}
