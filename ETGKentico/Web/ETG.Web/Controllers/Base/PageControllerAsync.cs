using AutoMapper;
using Devotion.Data;
using Devotion.Web.Base.Providers;
using ETG.Web.Common.Models.Authentication;

namespace ETG.Web.Controllers.Base
{
    public class PageControllerAsync<TRepository, TModel, TViewModel> : Devotion.Web.Base.Controllers.
        PageControllerAsync<TRepository, TModel, TViewModel, UserModel>
        where TRepository : IRepositoryAsync<TModel>
    {
        public PageControllerAsync(IMapper mapper, TRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider) : base(mapper, repository,
            baseAuthenticationProvider)
        {
        }
    }
}