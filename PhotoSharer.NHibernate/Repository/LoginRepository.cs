using System;
using System.Collections.Generic;
using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;

namespace PhotoSharer.Nhibernate.Repository
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        private ISessionFactory sessionFactory;

        public LoginRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }



        public IList<Login> GetByUserId(Guid userId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var logins = session.QueryOver<Login>()
                    .Where(login => login.User.Id == userId).List();

                return logins;
            }
        }

        public Login GetByLoginInfo(Guid userId, string loginProvider, string providerKey)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var login = session.QueryOver<Login>().Where(_login =>
                    _login.User.Id == userId &&
                    _login.LoginProvider == loginProvider && 
                    _login.ProviderKey == providerKey)
                        .SingleOrDefault();

                return login;
            }
        }
    }
}