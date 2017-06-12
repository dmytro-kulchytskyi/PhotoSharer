using System;
using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using PhotoSharer.Business;

namespace PhotoSharer.Nhibernate.Repository
{
    public class PhotoStreamRepository : Repository<PhotoStream>, IPhotoStreamRepository
    {
        public PhotoStreamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        private UnitOfWork unitOfWork;

        private ISession session { get => unitOfWork.Session; }

        public Guid GetCreatorId(Guid photoStreamId)
        {
            return session.Query<PhotoStream>().Where(it => it.Id == photoStreamId).Select(it => it.OwnerId).SingleOrDefault();
        }

        public IList<PhotoStream> GetGroupPhotoStreams(Guid groupId)
        {
            return session.QueryOver<PhotoStream>().Where(it => it.GroupId == groupId).List();
        }

        public IList<PhotoStream> GetGroupPhotoStreamsByUserId(Guid groupId, Guid userId)
        {
            return session.Query<PhotoStream>()
                             .Where(it => it.GroupId == groupId && it.OwnerId == userId).ToList();
        }

        public bool IsExists(Guid groupId, string provider, string url)
        {
               return session.Query<PhotoStream>()
                    .Any(it => it.GroupId == groupId && it.Provider == provider && it.Url == url);
        }
    }
}