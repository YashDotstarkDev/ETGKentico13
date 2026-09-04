using System.Linq;
using System.Reflection;
using AutoMapper;
using Devotion.Automapper.Common;
using Devotion.Web.Base.ViewModels;
using ETG.Web.AutoMapper;
using ETG.Web.Models;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace ETG.Web
{
    public class AutoMapperModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x =>
            {
                x.FromAssembliesMatching("ETG.*")
                    .SelectAllClasses()
                    .InheritedFrom<IDataModel>()
                    .BindAllInterfaces();

                x.FromAssembliesMatching("ETG.*")
                    .SelectAllClasses()
                    .InheritedFrom<ITaxonomyDataModel>()
                    .BindAllInterfaces();

                x.FromAssembliesMatching("ETG.*")
                    .SelectAllClasses()
                    .InheritedFrom<IViewModel>()
                    .BindAllInterfaces();

                x.FromAssembliesMatching("ETG.*")
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IViewModel<>))
                    .BindAllInterfaces();
            });

            //var mapperConfiguration = CreateConfiguration(Kernel);
            //Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();
            Bind<IMapper>().ToMethod(ctx => new Mapper(CreateConfiguration(ctx), type => ctx.Kernel.Get(type))).InSingletonScope();
        }

        private MapperConfiguration CreateConfiguration(IContext ctx)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var types = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(p => typeof(IMappable).IsAssignableFrom(p) && !p.IsAbstract);

                foreach (var type in types)
                {
                    var instance = (IMappable)ctx.Kernel.Get(type);
                    instance.CreateMaps(cfg);
                }

                // Add profiles from assemblies
                //cfg.AddProfile<MapperProfile>();
                cfg.AddProfile(new ViewModelMapperProfile(ctx));
                cfg.AddProfile(new LandingPageMapperProfile(ctx));
                cfg.AddProfile(new WebAPIMapperProfile(ctx));
            });

            return config;
        }
    }
}