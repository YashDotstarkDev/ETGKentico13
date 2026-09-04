using Devotion.Cache;
using ETG.Core.Http;
using ETG.Core.Kentico;
using ETG.Core.Services;
using ETG.Core.Utility;
using ETG.Data.Cache;
using ETG.Data.Currency;
using ETG.Data.Destination.Repositories;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Modules;
using ETG.Data.Repositories.Tour;
using ETG.Data.Tour.Factories;
using ETG.Data.Tour.Repositories;
using ETG.Data.Tour.Services;

namespace ETG.Data.Factories
{
    public static class ServiceFactory
    {
        private static ICacheService GetCacheServiceObject()
        {
            var kenticoSiteContext = new KenticoSiteContext();
            var dependencyFactory = new CacheDependencyFactory(kenticoSiteContext);
            return new CacheService(kenticoSiteContext, new CacheProviderDecorator(
                new KenticoCacheProvider(), dependencyFactory
            ), dependencyFactory);
        }

        public static IDestinationService GetDestinationServiceObject()
        {
            return new DestinationService(GetCacheServiceObject(), new DestinationRepository());
        }

        public static IArticleCategoryRepository GetArticleCategoryRepositoryObject()
        {
            return new ArticleCategoryRepository(GetCacheServiceObject());
        }

        public static IExperienceService GetExperienceServiceObject()
        {

            return new ExperienceService(GetCacheServiceObject());
        }

        public static ITourTypeRepository GetTourTypeRepositoryObject()
        {

            return new TourTypeRepository(GetCacheServiceObject());
        }

        public static IDiscountService GetDiscountServiceObject()
        {

            return new DiscountService(GetCacheServiceObject(), new Clock());
        }

        public static ITourService GetTourServiceObject()
        {

            var tourService = new TourService(GetCacheServiceObject(), GetDestinationServiceObject(), GetExperienceServiceObject(), GetTourTypeRepositoryObject(),
             GetDiscountServiceObject(), new PriceInclusionRepository(), new CruiseTypeRepository(),  new TourPricingRepository(), new TourRepositoryFactory(),
             new TourListingService(new TourRepositoryFactory()), new RealHttpRequest(), new CurrencyService(new CurrencyConversionRetriever(), new FexRepository(), new KenticoLogger(new KenticoSiteContext())));
            tourService.SetTourRepository(new TourRepository(new KenticoSiteContext(), new TourModelFactory(GetDestinationServiceObject())));
            return tourService;
        }
    }
}
