/*
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PhotoSharer.MVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(PhotoSharer.MVC.App_Start.NinjectWebCommon), "Stop")]

namespace PhotoSharer.MVC.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using NHibernate.Cfg;
    using PhotoSharer.Business.Entities;
    using NHibernate.Tool.hbm2ddl;
    using Microsoft.AspNet.Identity;
    using PhotoSharer.Business.Managers;
    using Microsoft.AspNet.Identity.Owin;
    using PhotoSharer.Business.Repository;
    using PhotoSharer.Nhibernate.Repository;
    using Microsoft.Owin.Security;
    using PhotoSharer.MVC.Stores;
    using NHibernate;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISessionFactory>().ToMethod(_ =>
            {
                var configuration = new Configuration();
                configuration.Configure();
                configuration.AddAssembly(typeof(AppUser).Assembly);
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
                var sessionFactory = configuration.BuildSessionFactory();
                new SchemaUpdate(configuration).Execute(true, true);
                return sessionFactory;

            }).InSingletonScope();

            kernel.Bind<IAuthenticationManager>().ToMethod(_ => HttpContext.Current.GetOwinContext().Authentication);

            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IGroupRepository>().To<GroupRepository>();
            kernel.Bind<ILoginRepository>().To<LoginRepository>();

            kernel.Bind<IUserStore<AppUser, Guid>>().To<AppUserStore>();

            kernel.Bind<UserManager<AppUser, Guid>>().To<AppUserManager>();
            kernel.Bind<SignInManager<AppUser, Guid>>().To<SignInManager<AppUser, Guid>>();
        }        
    }
}*/
