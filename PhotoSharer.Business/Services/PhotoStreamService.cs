using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Services
{
    public class PhotoStreamService
    {
        private readonly UserService userService;

        private readonly GroupsService groupsService;

        private readonly IPhotoStreamRepository photoStreamRepository;

        public PhotoStreamService(UserService userService, GroupsService groupsService, IPhotoStreamRepository photoStreamRepository)
        {
            this.userService = userService;
            this.groupsService = groupsService;
            this.photoStreamRepository = photoStreamRepository;
        }

        public bool IsStreamInGroup(Guid groupId, ProviderInfo providerInfo)
        {
            return photoStreamRepository.IsExists(groupId, providerInfo);
        }

        public PhotoStream CreatePhotoStream(Guid userId, Guid groupId, ProviderInfo providerInfo)
        {
            if (!userService.IsUserExists(userId))
                throw new ArgumentException("No such user");

            if (!groupsService.IsGroupExists(groupId))
                throw new ArgumentException("No such group");

            var photoStream = new PhotoStream
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                Provider = providerInfo.Provider,
                Url = providerInfo.Url,
                OwnerId = userId,
                //-------------TODO----------------
                AccountName = Guid.NewGuid().ToString(),
                ExteralId = Guid.NewGuid().ToString()
            };

            photoStreamRepository.Save(photoStream);

            return photoStream;
        }

        public IList<PhotoStream> GetGroupStreams(Guid groupId)
        {
            var photoStreams = photoStreamRepository.GetGroupPhotoStreams(groupId);

            return photoStreams;
        }

        public IList<PhotoStream> GetGroupStreamsByUserId(Guid groupId, Guid userId)
        {
            var photoStreams = photoStreamRepository.GetGroupPhotoStreamsByUserId(groupId, userId);

            return photoStreams;
        }

        public bool IsUsersPhotoStream(Guid userId, Guid streamId)
        {
            var streamCreatorId = photoStreamRepository.GetCreatorId(streamId);
            return (userId == streamCreatorId);
        }

        public void DeleteStream(Guid streamId)
        {
            photoStreamRepository.Delete(streamId);
        }
    }
}
