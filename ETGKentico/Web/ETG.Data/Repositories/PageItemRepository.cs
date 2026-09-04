using System.Linq;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Repositories
{
    public class PageItemRepository : IPageItemRepository
    {
        private readonly ISiteContext _siteContext;
        public PageItemRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public PageItemModel GetPage(string path)
        {
            return PageProvider.GetPage(path, _siteContext.CurrentCultureCode, _siteContext.SiteName)
                                .Select(a => new PageItemModel
                                {
                                    
                                    PageHero = new Models.Common.PageHeroModel
                                    {
                                        Heading = a.PageHeading,
                                        HeroCaption = a.PageHeroCaption,
                                        HeroImage = a.PageHeroImage,
                                        HideShareButton = a.PageHideShareButton
                                    },
                                    Page = new PageNodeModel
                                    {

                                        DocumentID = a.DocumentID,
                                        PageTitle = a.DocumentPageTitle,
                                        PageDescription = a.DocumentPageDescription,
                                        PageAliasPath = a.NodeAliasPath,
                                        PageAlias = a.NodeAlias,
                                        PageKeywords = a.DocumentPageKeyWords,
                                        ShareTitle = a.DocumentPageTitle,
                                        ShareDescription = a.DocumentPageDescription,
                                        ShareImage = a.PageHeroImage,
                                        ExcludedFromSearch = a.DocumentSearchExcluded
                                    },
                                    Content = a.PageContent
                                }).FirstOrDefault();
        }
    }
}
