using NHibernate;
using PhotoSharer.Models.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models.Repository
{
    public class GroupRepository : Repositiry<Group>, IGroupRepository
    {
        public GroupRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }



    }
}