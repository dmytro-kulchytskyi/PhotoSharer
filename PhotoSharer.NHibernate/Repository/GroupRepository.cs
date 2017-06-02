using NHibernate;
using NHibernate.Criterion;
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



        public AppGroup GetByLink(string groupLink)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var group = session.QueryOver<AppGroup>().Where(url => url.Link == groupLink).SingleOrDefault();
                return group;
            }

        }

        public Guid GetIdByLink(string groupLink)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groupId = session.Query<AppGroup>().Where(_group => _group.Link == groupLink)
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

                using (var transaction = session.BeginTransaction())
                {
                    session.Save(groupMember);
                    transaction.Commit();
                }

                return true;
            }
        }

        public IList<AppGroup> GetByUserId(Guid userId, int skip = 0, int take = 0)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groupIds = DetachedCriteria.For<GroupMember>()
                    .SetProjection(Projections.Property("GroupId"))
                    .Add(Restrictions.Eq("UserId", userId));

                var groupsCriteria = session.CreateCriteria<AppGroup>()
                    .Add(Subqueries.PropertyIn("Id", groupIds));

                if (skip > 0)
                {
                    groupsCriteria.SetFirstResult(skip);
                }

                if (take > 0)
                {
                    groupsCriteria.SetMaxResults(take);
                }

                return groupsCriteria.List<AppGroup>();
            }
        }

        public IList<AppGroup> GetCreatedByUser(Guid userId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groups = session.QueryOver<AppGroup>()
                        .Where(group => group.CreatorId == userId).List();

                return groups;
            }
        }


        public IList<AppGroup> GetByUserIdAndCheckIfCreator(Guid userId, int skip = 0, int take = 0)
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
                var filteredGroups = groups.Where(g => g.CreatorId == user.Id).ToList();

                return filteredGroups;

            }
        }
    }
}