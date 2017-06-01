﻿using PhotoSharer.Business.Entities;
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

            AppGroup group = new AppGroup()
            {
                Name = groupName,
                Url = "group-" + Guid.NewGuid().ToString(),
                CreatorId = creatorId
            };
            
            var groupId = groupRepository.Save(group);

            if (groupId == null || groupId == Guid.Empty)
            {
                return null;
            }

            var addUserResult = groupRepository.AddUser(creatorId, groupId);

            if (!addUserResult)
            {
                groupRepository.Delete(group);
                return null;
            }

            return group;
        }


        public bool AddUser(Guid userId, string groupUrl)
        {
            var groupId = groupRepository.GetIdGyUrl(groupUrl);
            if (groupId == null || groupId == Guid.Empty)
            {
                return false;
            }

            return groupRepository.AddUser(userId, groupId);
        }
    }
}