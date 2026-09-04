using AutoMapper;
using Devotion.Data;
using Devotion.Web.Base.Providers;
using ETG.Web.Common.Models.Authentication;

namespace ETG.Web.Controllers.Base
{
    public class PageController<TRepository, TModel, TViewModel> : Devotion.Web.Base.Controllers.PageController<TRepository, TModel, TViewModel, UserModel>
        where TRepository : IRepository<TModel>
    {
        public PageController(IMapper mapper, TRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider) 
            : base(mapper, repository, baseAuthenticationProvider)
        {
        }
    }
}