using CMS.Core;
using Devotion.Web.Base.Providers;
using Ninject.Extensions.Conventions;
using System.Linq;
using CMS.Ecommerce;
using ETG.Core.Services;
using ETG.Data.Career.Repositories;
using ETG.Data.Services;
using ETG.Data.Forms;
using ETG.Data.Search;
using ETG.Data.Tour.Repositories;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ETG.Web.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ETG.Web.NinjectWebCommon), "Stop")]

namespace ETG.Web
{
    using Devotion.Cache;
    using ETG.Core.Forms;
    using ETG.Core.Http;
    using ETG.Core.Kentico;
    using ETG.Web.Authentication;
    using ETG.Web.Common.Authentication;
    using ETG.Data.Validation.Validators;
    using FluentValidation;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using System;
    using System.Web;
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
            var kernel = new StandardKernel(new AutoMapperModule());
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("ETG."));
                BootstrapHelper.LoadNinjectKernel(kernel, assemblies);

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
            kernel.Bind<IAppSettingsService>().ToMethod(x => Service.Resolve<IAppSettingsService>()).InSingletonScope();
            kernel.Bind<ISettingsService>().ToMethod(x => Service.Resolve<ISettingsService>()).InSingletonScope();
            //kernel.Bind<ISettingProvider<KenticoSetting>>().To<KenticoSettingsProvider>().InSingletonScope();
            //kernel.Bind<ISettingProvider<AppSetting>>().To<AppSettingProvider>().InSingletonScope();
            kernel.Bind<IHttpRequest>().To<RealHttpRequest>().InSingletonScope();
            kernel.Bind<IShoppingService>().ToMethod(x => Service.Resolve<IShoppingService>()).InSingletonScope();
            kernel.Bind<ISearchService>().To<AlgoliaSearchService>().InSingletonScope();
            kernel.Bind<ILogger>().To<KenticoLogger>().InSingletonScope();
            kernel.Bind<IBizformEntry<EnquireItem>>().To<BizformEntry<EnquireItem>>().InSingletonScope();
            kernel.Bind<IBizformEntry<CareerRoleApplicationItem>>().To<BizformEntry<CareerRoleApplicationItem>>().InSingletonScope();
            kernel.Bind<IBizformEntry<BrochureOrderItem>>().To<BizformEntry<BrochureOrderItem>>().InSingletonScope();
            kernel.Bind<IBizformEntry<BrochureSignupItem>>().To<BizformEntry<BrochureSignupItem>>().InSingletonScope();
            kernel.Bind<ICacheProvider>().To<KenticoCacheProvider>().InSingletonScope();
            kernel.Bind<ISiteContext>().To<KenticoSiteContext>().InSingletonScope();
            kernel.Bind(typeof(IOwinIdentityProvider<>)).To<OwinIdentityProvider>();
            kernel.Bind(typeof(IAuthenticationProvider<>)).To<AuthenticationProvider>();
            kernel.Bind(typeof(AbstractValidator<EnquireItem>)).To<GenericEnquireValidator>();
            kernel.Bind(typeof(AbstractValidator<CareerRoleApplicationItem>)).To<CareerRoleApplicationValidator>();
            kernel.Bind(typeof(AbstractValidator<BrochureOrderItem>)).To<BrochureOrderValidator>();
            kernel.Bind(typeof(AbstractValidator<BrochureSignupItem>)).To<BrochureSignupValidator>();
            kernel.Bind(typeof(AbstractValidator<CompetitionItem>)).To<CompetitionValidator>();
            kernel.Bind(typeof(AbstractValidator<CompetitionAgentsItem>)).To<CompetitionAgentValidator>();
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

            //kernel.Rebind<IResourceProvider>().To<ResourceProvider>().InSingletonScope();
            /*
            // Config-based bindings
            var appSettingProvider = kernel.Get<ISettingProvider<AppSetting>>();

            if (!appSettingProvider.GetSetting<bool>(AppSetting.SmsGatewayAllowSms))
            {
                var testHttpEndpoint = appSettingProvider.GetSetting(AppSetting.SmsGatewayTestHttpEndpoint);
                if (!string.IsNullOrWhiteSpace(testHttpEndpoint))
                {
                    kernel.Rebind<IMobileVerificationSenderProvider>().To<HttpEndpointMobileVerificationSenderProvider>();
                }
            }*/
        }
    }
}