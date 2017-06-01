using NHibernate;
using NHibernate.Linq;
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



        public AppGroup GetByUrl(string groupUrl)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var group = session.QueryOver<AppGroup>().Where(url => url.Url == groupUrl).SingleOrDefault();
                return group;
            }

        }


        public bool AddUser(Guid userId, string groupUrl)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groupId = session.Query<AppGroup>().Where(_group => _group.Url == groupUrl).Select(_group => _group.Id).SingleOrDefault();
                if (groupId == Guid.Empty)
                {
                    return false;
                }

                if (session.CreateSQLQuery("SELECT * FROM User_Group Where GroupId=? AND UserId=?").SetParameter(0, groupId).SetParameter(1, userId).List().Count != 0)
                {
                    return false;
                }

                session.CreateSQLQuery("INSERT INTO User_Group ( GroupId, UserId) VALUES (?,?)").SetParameter(0, groupId).SetParameter(1, userId).ExecuteUpdate();
                return true;
            }
        }


        public IList<AppGroup> GetByUserId(Guid userId, int skip = 0, int take = 0)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.Get<AppUser>(userId);
                if (user == null) return null;

                var groups = user.Groups.Skip(skip);
                if (take > 0)
                {
                    groups = groups.Take(take);
                }

                return groups.ToList();
            }
        }
    }
}