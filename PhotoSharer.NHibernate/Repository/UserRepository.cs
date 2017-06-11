using System;
using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using NHibernate.Linq;
using System.Linq;
using PhotoSharer.Business;

namespace PhotoSharer.Nhibernate.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        private UnitOfWork unitOfWork;

        private ISession session { get => unitOfWork.Session; }

        public AppUser GetUserByUserName(string userName)
        {
            return session.QueryOver<AppUser>()
                .Where(u => u.UserName == userName).SingleOrDefault();
        }

        public AppUser GetUserByLoginInfo(string loginProvider, string providerKey)
        {
            return session.QueryOver<AppUser>()
                .Where(it => it.LoginProvider == loginProvider && it.ProviderKey == providerKey).SingleOrDefault();
        }

        public bool IsUserInGroup(Guid userId, Guid groupId)
        {
            return session.Query<GroupMember>().Any(it => it.UserId == userId && it.GroupId == groupId);
        }
    }
}