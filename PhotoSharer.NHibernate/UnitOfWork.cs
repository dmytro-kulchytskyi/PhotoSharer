using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PhotoSharer.Business;
using PhotoSharer.Business.Entities;
using System;

namespace PhotoSharer.Nhibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory sessionFactory;

        private ITransaction transaction;

        private ISession session;

        public ISession Session
        {
            get
            {
                if (session == null || !session.IsOpen)
                {
                    session = sessionFactory.OpenSession();
                    transaction = session.BeginTransaction();
                }

                return session;
            }
            private set
            {
                session = value;
            }
        }

        static UnitOfWork()
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(AppUser).Assembly);
            configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString,
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);

            sessionFactory = configuration.BuildSessionFactory();
            new SchemaUpdate(configuration).Execute(true, true);
        }

        public void BeginTransaction()
        {
            transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Commit();
            }
            catch
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();

                throw;
            }
            finally
            {
                Session.Dispose();
            }
        }

        void RollbackTransaction()
        {
            try
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}
