using NHibernate;
using PhotoSharer.Business.Entities.Interfaces;
using PhotoSharer.Business.Repository;
using System;

namespace PhotoSharer.Nhibernate.Repository
{
    public abstract class Repository<T> : IRepository<T> 
        where T : class, IEntity
    {
        private ISessionFactory sessionFactory;

        public  Repository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }



        public void Delete(T instance)
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


        public T GetById(Guid id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }


        public Guid Save(T instance)
        {
            using (var session = sessionFactory.OpenSession())
            {
                    using (var transaction = session.BeginTransaction())
                {
                    session.Save(instance);
                    transaction.Commit();
                    return instance.Id;
                }
            }
        }


        public void Update(T instance)
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
    }
}