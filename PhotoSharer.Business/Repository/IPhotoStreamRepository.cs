using PhotoSharer.Business.Entities;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Repository
{
    public interface IPhotoStreamRepository : IRepository<PhotoStream>
    {
        IList<PhotoStream> GetGroupPhotoStreams(Guid groupId);
        IList<PhotoStream> GetGroupPhotoStreamsByUserId(Guid groupId, Guid userId);
        bool IsExists(Guid groupId, string provider, string url);
        Guid GetCreatorId(Guid photoStreamId);
    }
}
