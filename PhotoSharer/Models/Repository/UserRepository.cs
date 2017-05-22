using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using PhotoSharer.Models.Repository.Interface;

namespace PhotoSharer.Models.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }



    }
}