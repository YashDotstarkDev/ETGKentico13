using ETG.WebAPI;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using Kentico.Newsletters.Web.Mvc;
using WebSupergoo.ABCpdf12;

namespace ETG.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Enables and configures selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Registers routes including system routes for enabled features
            GlobalConfiguration.Configure((config) =>
            {
                var resolver = new Ninject.Web.WebApi.NinjectDependencyResolver(NinjectWebCommon.bootstrapper.Kernel);
                WebApiConfig.Register(config, resolver);
            });

            // Gets the ApplicationBuilder instance
            // Allows you to enable and configure Kentico MVC features
            var builder = ApplicationBuilder.Current;
            builder.UseEmailTracking(new EmailTrackingOptions());

            // Enables the preview feature
            builder.UsePreview();

            // Enables the page builder feature
            builder.UsePageBuilder();

            builder.UseResourceSharingWithAdministration();

            // Enables the alternative URLs feature
            builder.UsePageRouting(new PageRoutingOptions
            {
                EnableAlternativeUrls = true
            });

            // Registers routes including system routes for enabled features
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            XSettings.InstallLicense(
                @"X/VKS0cPn5FgsCJaaamDbY72L7NHQ4MYlq3wxL3
FA0ojxkiVPH3rYMVWQ0lkwg8KCtYz4j5HuSIUr6A
gQbd4xFcifGeZAX073zFMO/XgBjbi1y7S5MlUFrj
UWBKMcmImUL1oUMFb8wtwCFVMoSiSIEERXiebQ2W
5r8l4z1spFM7J3Ls/t3TZ9nfSpr8FriRXdAZazH1
EgV2OF3pIKqVS+UpVwiz2OfsJC8LZBU/ffproABY
GpM+kAGkepgG5KOD+");
            
        }
    }
}
