using System;
using NHibernate;
using PhotoSharer.Business.Entities;
using PhotoSharer.Business.Repository;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;

namespace PhotoSharer.Nhibernate.Repository
{
    public class PhotoStreamRepository : Repository<PhotoStream>, IPhotoStreamRepository
    {
        private ISessionFactory sessionFactory;

        public PhotoStreamRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public Guid GetCreatorId(Guid photoStreamId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.Query<PhotoStream>().Where(it => it.Id == photoStreamId).Select(it => it.CreatorId).SingleOrDefault();

                return result;
            }
        }

        public IList<PhotoStream> GetGroupPhotoStreams(Guid groupId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.QueryOver<PhotoStream>()
                    .Where(it => it.GroupId == groupId).List();

                return result;
            }
        }

        public IList<PhotoStream> GetGroupPhotoStreamsByUserId(Guid groupId, Guid userId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.QueryOver<PhotoStream>()
                    .Where(it => it.GroupId == groupId && it.CreatorId == userId).List();

                return result;
            }
        }

        public bool IsExists(Guid groupId, string provider, string url)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.Query<PhotoStream>()
                    .Any(it => it.GroupId == groupId && it.Provider == provider && it.Url == url);

                return result;
            }
        }
    }
}