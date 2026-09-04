using AutoMapper;
using Devotion.Automapper.Common;
using ETG.Web.Models;
using Ninject.Activation;
using System;
using System.Linq;

namespace ETG.Web.AutoMapper
{
    public class ViewModelMapperProfile : Profile
    {
        public ViewModelMapperProfile(IContext context)
        {
            var kernel = context.Kernel;

            var dataModelTypes = kernel.GetBindings(typeof(IDataModel)).Select(x => x.GetProvider(context).Type).ToList();
            var viewModels = kernel.GetBindings(typeof(IViewModel)).Select(x => x.GetProvider(context).Type);

            foreach (var viewModel in viewModels)
            {
                var dataModels = dataModelTypes.Where(x => x.Name.Equals(viewModel.Name.Replace("ViewModel", "Model"), StringComparison.OrdinalIgnoreCase));
                foreach (var dm in dataModels)
                {
                    CreateMap(dm, viewModel, MemberList.None).ReverseMap();
                }
            }
        }
    }
}