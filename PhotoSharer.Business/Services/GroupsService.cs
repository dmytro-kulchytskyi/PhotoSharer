using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;

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
            var creator = userService.GetById(creatorId);
            if (creator == null)
            {
                return null;
            }
            Guid guid = Guid.NewGuid();
            AppGroup group = new AppGroup()
            {
                Name = groupName,
                InviteCode = guid.ToString(),
                Url = guid.ToString(),
                CreatorId = creatorId
            };

            group.Users = new List<AppUser> { creator };
            var groupId = groupRepository.Save(group);

            if (groupId == null)
            {
                return null;
            }

            return group;
        }

        public bool AddUserToGroup(Guid userId, string groupUrl)
        {

            return groupRepository.AddUser(userId,groupUrl) ;
        }

    }
}