using System;
using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using NHibernate.Linq;
using System.Linq;

namespace PhotoSharer.Nhibernate.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private ISessionFactory sessionFactory;

        public UserRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public AppUser GetUserByUserName(string userName)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.QueryOver<AppUser>()
                    .Where(u => u.UserName == userName).SingleOrDefault();

                return user;
            }
        }
        
        public AppUser GetUserByLoginInfo(string loginProvider, string providerKey)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.QueryOver<AppUser>()
                    .Where(it => it.LoginProvider == loginProvider && it.ProviderKey == providerKey).SingleOrDefault();

                return user;
            }
        }

        public bool IsUserInGroup(Guid userId, Guid groupId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.Query<GroupMember>().Any(it => it.UserId == userId && it.GroupId == groupId);

                return result;
            }
        }
    }
}