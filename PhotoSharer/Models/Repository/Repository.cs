using NHibernate;
using NHibernate.Linq;
using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models.Repository
{
    public abstract class Repository<T> : IRepository<T> 
        where T : class, IEntity
    {
        private ISessionFactory SessionFactory;

        public  Repository(ISessionFactory sessionFactory)
        {
            this.SessionFactory = sessionFactory;
        }

        public void Delete(T instance)
        {
            using (var session = SessionFactory.OpenSession())
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
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public Guid Save(T instance)
        {
                using (var session = SessionFactory.OpenSession())
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
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(instance);
                    transaction.Commit();
                }
            }
        }

        /*
         public IQueryable<T> GetAll()
         {
             using (var session = SessionFactory.OpenSession())
             {
                 return session.Query<T>();
             }
         } 
         */
    }
}