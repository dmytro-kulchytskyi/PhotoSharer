using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using PhotoSharer.Business;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSharer.Nhibernate.Repository
{
    public class GroupRepository : Repository<AppGroup>, IGroupRepository
    {
        public GroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        private UnitOfWork unitOfWork;

        private ISession session { get => unitOfWork.Session; }

        
 
        public void AddUser(Guid userId, Guid groupId)
        {
            var groupMember = new GroupMember
            {
                UserId = userId,
                GroupId = groupId
            };

            session.Save(groupMember);
        }

        public IList<AppGroup> GetGroupsByUserId(Guid userId, int skip = 0, int take = 0)
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

        public IList<AppGroup> GetGroupsCreatedByUser(Guid userId, int skip = 0, int take = 0)
        {
            IQueryOver<AppGroup> groups = session.QueryOver<AppGroup>()
                    .Where(it => it.OwnerId == userId);

            if (skip > 0)
                groups = groups.Skip(skip);

            if (take > 0)
                groups = groups.Take(take);

            return groups.List();
        }

        public void RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            var member = session.Query<GroupMember>().SingleOrDefault(it => it.UserId == userId && it.GroupId == groupId);
            if (member == null)
                throw new ArgumentException("There is no such bundle of userId and groupId");

            session.Delete(member);
        }
    }
}