﻿using NHibernate;
using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models.Repository
{
    public class Repository<T> : IRepository<T> 
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

        public IQueryOver<T> GetAll()
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.QueryOver<T>();
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
    }
}