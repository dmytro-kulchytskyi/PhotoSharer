using NHibernate;
using PhotoSharer.Models.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models.Repository
{
    public class GroupRepository : Repository<AppGroup>, IGroupRepository
    {
        private ISessionFactory sessionFactory;

        public GroupRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
        public IList<AppGroup> GetAll()
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groups = session.QueryOver<AppGroup>().List();
                return groups;
            }
        }
        public IList<AppGroup> GetUserGroups(Guid UserId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groups = session.QueryOver<AppGroup>().Where(user=>user.Creator.Id==UserId).List();
                return groups;
          }
        }
        public AppGroup GetGroupById(Guid Id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var group = session.QueryOver<AppGroup>().Where(id => id.Id== Id).List().First();
     
                return group;

            }
        }


    }
}