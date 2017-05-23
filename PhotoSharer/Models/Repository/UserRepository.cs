using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using NHibernate;
using PhotoSharer.Models.Repository.Interface;

namespace PhotoSharer.Models.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private ISessionFactory SessionFactory;

        public UserRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            SessionFactory = sessionFactory;
        }
 
        public AppUser GetByUserName(string userName)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var user = session.QueryOver<AppUser>().Where(u => u.UserName == userName).List().FirstOrDefault();
                return user;
            }
        }
    }
}