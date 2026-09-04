using System.Linq;
using Devotion.Cache;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Pages;

namespace ETG.Data.Repositories.Pages
{
    public class OrderCancelPageRepository : IOrderCancelPageRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ISiteContext _siteContext;
        public OrderCancelPageRepository(ICacheProvider cacheProvider, ISiteContext siteContext)
        {
            _cacheProvider = cacheProvider;
            _siteContext = siteContext;
        }
        
        private OrderCancelPageModel GetContentsInternal()
        {
            return OrderCancelPageProvider.GetOrderCancelPages()
                .OnCurrentSite().Select(page => new OrderCancelPageModel
                {
                    Contents = page.OrderCancelPageContent
                }).FirstOrDefault();
        }
        public OrderCancelPageModel GetContents()
        {
            return _cacheProvider.GetCached(() => GetContentsInternal(),
                "getordercancelpage",
                new GenericDependencyBuilder<OrderCancelPage>(_siteContext.SiteName).DependsOnAllNodesOfPageType());

        }
    }
}