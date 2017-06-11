using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using PhotoSharer.Business;
using PhotoSharer.Business.Entities.Interfaces;
using PhotoSharer.Business.Repository;
using System;
using System.Linq;

namespace PhotoSharer.Nhibernate.Repository
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        private UnitOfWork unitOfWork;

        private ISession session { get => unitOfWork.Session; }

        public virtual T GetById(Guid id)
        {
            return session.Get<T>(id);
        }

        public virtual void Save(T instance)
        {
            session.Save(instance);
        }


        public virtual void Update(T instance)
        {
            session.Update(instance);
        }

        public virtual void Delete(T instance)
        {
            session.Delete(instance);
        }

        public virtual bool IsExists(Guid id)
        {
            return session.Query<T>().Any(it => it.Id == id);
        }

        public void Delete(Guid id)
        {
            var instance = session.Load<T>(id);
            session.Delete(instance);
        }
    }
}