[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PhotoSharer.MVC.NInjectIOC.IOC), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(PhotoSharer.MVC.NInjectIOC.IOC), "Stop")]

namespace PhotoSharer.MVC.NInjectIOC
{
    using System;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Microsoft.Owin.Security;
    using PhotoSharer.Nhibernate.Repository;
    using PhotoSharer.Business.Repository;
    using PhotoSharer.Business.Entities;
    using PhotoSharer.Business.Managers;
    using PhotoSharer.Business.Stores;
    using PhotoSharer.Nhibernate;
    using PhotoSharer.Business;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using System.Web.Mvc;
    using PhotoSharer.MVC.ActionFilters;
    using PhotoSharer.MVC.Attributes;

    public static class IOC
    {
        public static T Resolve<T>()
            where T : class
        {
            return (T)bootstrapper.Kernel.GetService(typeof(T));
        }

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
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            kernel.Bind<IAuthenticationManager>().ToMethod(_ => HttpContext.Current.GetOwinContext().Authentication);

            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IGroupRepository>().To<GroupRepository>();
            kernel.Bind<IPhotoStreamRepository>().To<PhotoStreamRepository>();

            kernel.Bind<IUserStore<AppUser, Guid>>().To<AppUserStore>();

            kernel.Bind<UserManager<AppUser, Guid>>().To<AppUserManager>();
            kernel.Bind<SignInManager<AppUser, Guid>>().To<SignInManager<AppUser, Guid>>();

            kernel.BindFilter<TransactionFilter>(FilterScope.Action, null).WhenActionMethodHas<TransactionAttribute>();
        }
    }
}
