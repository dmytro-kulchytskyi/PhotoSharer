using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;

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


        public AppUser GetByUserName(string userName)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.QueryOver<AppUser>()
                    .Where(u => u.UserName == userName).SingleOrDefault();

                return user;
            }
        }


        public AppUser GetByLoginInfo(string loginProvider, string providerKey)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.Query<Login>()
                    .Where(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey)
                        .Select(login => login.User).SingleOrDefault();

                return user;
            }
        }
   
    }
}