using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PhotoSharer.Business.Services
{
    public class GroupsService
    {
        private readonly UserService userService;
        private readonly IGroupRepository groupRepository;
        public GroupsService(
            IGroupRepository groupRepository,
            UserService userService)
        {
            this.groupRepository = groupRepository;
            this.userService = userService;
        }

        public IList<AppGroup> GetUserGroups(Guid userId)
        {
            return groupRepository.GetByUserId(userId);
        }

        public AppGroup CreateGroup(string groupName, Guid creatorId)
        {
            string pattern = @"[\W\s]+";
            string replacement = "-";
            Regex rgx = new Regex(pattern);
            string groupNameLink = rgx.Replace(groupName, replacement);

            AppGroup group = new AppGroup()
            {
                Name = groupName,
                Link = "group-" + Guid.NewGuid().ToString() + "-" +groupNameLink,
                CreatorId = creatorId
            };
            
            var groupId = groupRepository.Save(group);

            if (groupId == Guid.Empty)
            {
                throw new ArgumentNullException("Group id is null");
            }

            var addUserResult = groupRepository.AddUser(creatorId, groupId);

        return group;
        }


        public bool AddUser(Guid userId, string groupUrl)
        {
            var groupId = groupRepository.GetIdByLink(groupUrl);
            if (groupId == null || groupId == Guid.Empty)
            {
                return false;
            }

            return groupRepository.AddUser(userId, groupId);
        }

        public IList<AppGroup> GetCreatedByUser(Guid userId)
        {
            var groups = groupRepository.GetCreatedByUser(userId);

            return groups;
        }

        public AppGroup GetByLink(string groupLink)
        {
            var group = groupRepository.GetByLink(groupLink);

            return group;
        }
    }
}