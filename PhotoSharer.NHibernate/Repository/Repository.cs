using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using PhotoSharer.Business.Entities.Interfaces;
using PhotoSharer.Business.Repository;
using System;
using System.Linq;

namespace PhotoSharer.Nhibernate.Repository
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private ISessionFactory sessionFactory;

        public Repository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public virtual T GetById(Guid id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public virtual void Save(T instance)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(instance);
                    transaction.Commit();
                }
            }
        }
        
        public virtual void Update(T instance)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(instance);
                    transaction.Commit();
                }
            }
        }
        
        public virtual void Delete(T instance)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(instance);
                    transaction.Commit();
                }
            }
        }

        public virtual bool IsExists(Guid id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.Query<T>().Any(it => it.Id == id);

                return result;
            }
        }

        public void Delete(Guid id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var instance = session.Get<T>(id);
                    session.Delete(instance);

                    transaction.Commit();
                }
            }
        }
    }
}