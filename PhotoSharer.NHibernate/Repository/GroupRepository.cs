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



        public AppGroup GetByUrl(string groupLink)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var group = session.QueryOver<AppGroup>().Where(url => url.Url == groupLink).SingleOrDefault();
                return group;
            }

        }

        public Guid GetIdGyUrl(string groupLink)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groupId = session.Query<AppGroup>().Where(_group => _group.Url == groupLink)
                            .Select(_group => _group.Id).SingleOrDefault();

                return groupId;
            }
        }


        public bool AddUser(Guid userId, Guid groupId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var isUniq = session.QueryOver<GroupMember>().Where(_groupMember =>
                          _groupMember.UserId == userId &&
                          _groupMember.GroupId == groupId).RowCount() == 0;

                if (!isUniq)
                {
                    return false;
                }

                var groupMember = new GroupMember
                {
                    UserId = userId,
                    GroupId = groupId
                };

                session.Save(groupMember);
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