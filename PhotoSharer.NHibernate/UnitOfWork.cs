using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PhotoSharer.Business;
using PhotoSharer.Business.Entities;
using System;

namespace PhotoSharer.Nhibernate
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private static readonly ISessionFactory sessionFactory;

        private ITransaction transaction;

        private ISession session;

        private bool useTransaction = false;

        public ISession Session
        {
            get
            {
                if (session == null || !session.IsOpen)
                    session = sessionFactory.OpenSession();

                if (useTransaction)
                {
                    if (transaction == null || !transaction.IsActive)
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
            useTransaction = true;
        }

        public void Commit()
        {
            if (!useTransaction)
                throw new InvalidOperationException("Transaction was not started");

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
                if (session != null)
                    session.Dispose();
            }
        }

        void RollbackTransaction()
        {
            if (!useTransaction)
                throw new InvalidOperationException("Transaction was not started");

            try
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
            finally
            {
                if (session != null)
                    session.Dispose();
            }
        }

        public void Dispose()
        {
            if (transaction != null && transaction.IsActive)
                transaction.Dispose();

            if (session != null && session.IsOpen)
                session.Dispose();

            session = null;
            transaction = null;
        }
    }
}
