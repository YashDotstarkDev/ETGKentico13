using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Data.TravelDeals.Models;
using ETG.Data.TravelDeals.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.TravelDeals.Models;

namespace ETG.Web.TravelDeals
{
    public class TravelDealsController : PageController<ITravelDealsPageRepository, TravelDealsPageModel, TravelDealsPageViewModel>
    {
        private readonly IMapper _mapper;
        public TravelDealsController(IMapper mapper,
            ITravelDealsPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
        }
    }
}