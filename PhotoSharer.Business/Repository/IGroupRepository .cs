using PhotoSharer.Business.Entities;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Repository
{
    public interface IGroupRepository : IRepository<AppGroup>
    {
        IList<AppGroup> GetGroupsByUserId(Guid userId, int skip = 0, int take = 0);
        void AddUser(Guid userId, Guid groupId);
        void RemoveUserFromGroup(Guid userId, Guid groupId);
        AppGroup GetGroupByGroupInfo(Guid groupId, string groupLink);
        IList<AppGroup> GetGroupsCreatedByUser(Guid userId, int skip = 0, int take = 0);
    }
}
