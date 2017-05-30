using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using PhotoSharer.Business.Services;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Managers
{
    public class GroupsManager
    {
        private readonly UserService userService;
        private readonly IGroupRepository groupRepository;
        public GroupsManager(
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

        public async string CreateGroupAsync(string groupName, Guid creatorId)
        {
            var creator = await userService.GetByIdAsync(creatorId);
            if (creator == null)
            {
                return null;
            }

            AppGroup group = new AppGroup()
            {
                Name = groupName,
                InviteCode = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString(),
                CreatorId = creatorId
            };

            group.Users = new List<AppUser> { creator };
            var groupId = groupRepository.Save(group);

            if (groupId == null)
            {
                return null;
            }

            return group.Url;
        }


    }
}