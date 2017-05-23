using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using NHibernate;
using PhotoSharer.Models.Repository.Interface;

namespace PhotoSharer.Models.Repository
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        private ISessionFactory SessionFactory;

        public LoginRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            SessionFactory = sessionFactory;
        }
 
        
    }
}