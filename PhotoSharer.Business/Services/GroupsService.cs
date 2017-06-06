using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Managers;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Services
{
    public class GroupsService
    {
        private readonly UserService userService;

        private readonly IGroupRepository groupRepository;

        public GroupsService(IGroupRepository groupRepository, UserService userService)
        {
            this.groupRepository = groupRepository;
            this.userService = userService;
        }

        public IList<AppGroup> GetUserGroups(Guid userId)
        {
            return groupRepository.GetGroupsByUserId(userId);
        }

        public AppGroup CreateGroup(string groupName, Guid creatorId)
        {
            if (!userService.IsUserExists(creatorId))
                throw new ArgumentException("Can't find owner by id.");

            var group = new AppGroup()
            {
                Id = Guid.NewGuid(),
                Name = groupName,
                Link = UrlManager.GetSafeString(groupName),
                CreatorId = creatorId
            };

            groupRepository.Save(group, creatorId);

            return group;
        }

        public bool IsGroupExists(Guid groupId)
        {
            return groupRepository.IsExists(groupId);
        }

        public void AddUser(Guid userId, Guid groupId)
        {
            if (!IsGroupExists(groupId))
                throw new ArgumentException("No such group");

            if (!userService.IsUserExists(userId))
                throw new ArgumentException("No such user");

            groupRepository.AddUser(userId, groupId);
        }

        public void RemoveUser(Guid userId, Guid groupId)
        {
            groupRepository.RemoveUserFromGroup(userId, groupId);
        }

        public IList<AppGroup> GetCreatedByUser(Guid userId)
        {
            var groups = groupRepository.GetGroupsCreatedByUser(userId);

            return groups;
        }

        public AppGroup GetByGroupInfo(Guid groupId, string groupLink)
        {
            return groupRepository.GetGroupByGroupInfo(groupId, groupLink);
        }
    }
}