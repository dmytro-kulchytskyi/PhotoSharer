using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSharer.Nhibernate.Repository
{
    public class GroupRepository : Repository<AppGroup>, IGroupRepository
    {
        private ISessionFactory sessionFactory;

        public GroupRepository(ISessionFactory sessionFactory) 
            : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }



        public IList<AppGroup> GetByUserId(Guid userId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.Get<AppUser>(userId);
                if (user == null) return null;
                return user.Groups.ToList();
            }
        }
    }
}