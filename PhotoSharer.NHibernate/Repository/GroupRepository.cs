using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void Save(AppGroup group, Guid creatorId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                if (group.CreatorId != creatorId)
                    throw new ArgumentException("CreatorId field values ​​don't match");

                var groupMember = new GroupMember
                {
                    UserId = creatorId,
                    GroupId = group.Id
                };

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(group);
                        session.Save(groupMember);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public AppGroup GetGroupByGroupInfo(Guid groupId, string groupLink)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var group = session.QueryOver<AppGroup>().Where(it => it.Id == groupId && it.Link == groupLink)
                    .SingleOrDefault();

                return group;
            }

        }

        public void AddUser(Guid userId, Guid groupId)
        {
            using (var session = sessionFactory.OpenSession())
            {
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
            }
        }

        public IList<AppGroup> GetGroupsByUserId(Guid userId, int skip = 0, int take = 0)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var groupIds = DetachedCriteria.For<GroupMember>()
                    .SetProjection(Projections.Property("GroupId"))
                    .Add(Restrictions.Eq("UserId", userId));

                var groupsCriteria = session.CreateCriteria<AppGroup>()
                    .Add(Subqueries.PropertyIn("Id", groupIds));

                if (skip > 0)
                    groupsCriteria.SetFirstResult(skip);


                if (take > 0)
                    groupsCriteria.SetMaxResults(take);

                return groupsCriteria.List<AppGroup>();
            }
        }

        public IList<AppGroup> GetGroupsCreatedByUser(Guid userId, int skip = 0, int take = 0)
        {
            using (var session = sessionFactory.OpenSession())
            {
                IQueryOver<AppGroup> groups = session.QueryOver<AppGroup>()
                        .Where(it => it.CreatorId == userId);

                if (skip > 0)
                    groups = groups.Skip(skip);

                if (take > 0)
                    groups = groups.Take(take);

                return groups.List();
            }
        }

        public void RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var member = session.Query<GroupMember>().Where(it => it.UserId == userId && it.GroupId == groupId).SingleOrDefault();
                if (member == null)
                    throw new ArgumentException("There is no such bundle of userId and groupId");

                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(member);
                    transaction.Commit();
                }
            }
        }
    }
}