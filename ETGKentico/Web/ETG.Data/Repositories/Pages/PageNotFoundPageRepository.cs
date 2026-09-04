using ETG.Data.Article.Services;
using ETG.Data.Models.Pages;
using ETG.Data.Services;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;

namespace ETG.Data.Repositories.Pages
{
    public class PageNotFoundPageRepository : IPageNotFoundPageRepository
    {
        private readonly IArticleService _articleService;
        private readonly ITourService _tourService;
        private readonly IContactService _contactService;
        public PageNotFoundPageRepository(IArticleService articleService, ITourService tourService,
            IContactService contactService)
        {
            _articleService = articleService;
            _tourService = tourService;
            _contactService = contactService;
        }

        public PageNotFoundPageModel GetPageNotFoundPage()
        {
            return new PageNotFoundPageModel
            {
                Articles = _articleService.GetLatestArticles(4),
                Tours = new TourListingModel
                {
                    Tours = _tourService.GetTiledTours(8),
                },
                Contact = _contactService.GetETGContactInfo()
            };


        }
    }
}
