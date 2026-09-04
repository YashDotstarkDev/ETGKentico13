using System;
using System.Linq;
using System.Web;
using CMS.Core;
using CommonServiceLocator;
using ETG.Core.Http;
using ETG.Core.Services;
using ETG.Data.Dependency;
using ETG.Data.Services;
using Ninject.Extensions.Conventions;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CMSApp.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CMSApp.NinjectWebCommon), "Stop")]

namespace CMSApp
{
    using Devotion.Cache;
    using ETG.Core.Kentico;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    public static class NinjectWebCommon
    {
        public static readonly Bootstrapper bootstrapper = new Bootstrapper();

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
            //try
            //{
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);

                ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));
            return kernel;
           /* }
            catch
            {
                kernel.Dispose();
                throw;
            }*/
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAppSettingsService>().ToMethod(x => Service.Resolve<IAppSettingsService>()).InSingletonScope();
            kernel.Bind<ISettingsService>().ToMethod(x => Service.Resolve<ISettingsService>()).InSingletonScope();
            //kernel.Bind<ISettingProvider<KenticoSetting>>().To<KenticoSettingsProvider>().InSingletonScope();
            kernel.Bind<IHttpRequest>().To<RealHttpRequest>().InSingletonScope();
            kernel.Bind<ICacheProvider>().To<KenticoCacheProvider>().InSingletonScope();
            kernel.Bind<ISiteContext>().To<KenticoSiteContext>().InSingletonScope();
            kernel.Bind<ILogger>().To<KenticoLogger>().InSingletonScope();
            //kernel.Bind(typeof(IOwinIdentityProvider<>)).To<OwinIdentityProvider>();
            //kernel.Bind(typeof(IAuthenticationProvider<>)).To<AuthenticationProvider>();
            kernel.Bind(x =>
            {
                x.FromAssembliesMatching("Devotion.*")
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });
            kernel.Bind(x =>
            {
                x.FromAssembliesMatching("ETG.*")
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

        }
    }
}