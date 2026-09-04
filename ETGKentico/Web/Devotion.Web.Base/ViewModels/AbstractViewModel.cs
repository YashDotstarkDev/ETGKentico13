using System;
using AutoMapper;

namespace Devotion.Web.Base.ViewModels
{
    public abstract class AbstractViewModel<TViewModel> : IMappable where TViewModel : AbstractViewModel<TViewModel>
    {
        public abstract Type[] SourceModelTypes { get; }

        public virtual void CreateMaps(IMapperConfigurationExpression config)
        {
            if (SourceModelTypes == null)
            {
                return;
            }

            foreach (var type in SourceModelTypes)
            {
                config.CreateMap(type, typeof(TViewModel), MemberList.None).ConstructUsingServiceLocator();
            }
        }
    }

    public interface IMappable
    {
        void CreateMaps(IMapperConfigurationExpression config);
    }
}
