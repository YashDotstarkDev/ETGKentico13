using ETG.Data.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using Devotion.Cache;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Booking;
using ETG.Data.Tour.Services;

namespace ETG.Data.Repositories.Pages
{
    public class PaymentPageRepository : IPaymentPageRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ISiteContext _siteContext;
        public PaymentPageRepository(ICacheProvider cacheProvider, ISiteContext siteContext)
        {
            _cacheProvider = cacheProvider;
            _siteContext = siteContext;
        }

        private PaymentPageContentsModel GetPaymentPageContentsInternal()
        {
            return 
             PaymentPageProvider.GetPaymentPages().OnCurrentSite().Select(page => new PaymentPageContentsModel
            {
                PaymentPageDocumentID = page.DocumentID,
                Heading = page.Heading,
                PageBodyContent = page.PaymentPageContent,
                ThankyouPageBodyContent = page.PaymentThankyoupageContent,
                ThankyouPageHeroImage = page.PaymentThankyouPageHeroImage,
                PaymentOptions = PaymentOptionProvider.GetPaymentOptions().OnCurrentSite().Path(page.NodeAliasPath, PathTypeEnum.Children)
                    .Select(opt => new PaymentOptionModel{
                    Name = opt.PaymentOptionName,
                    Description = opt.PaymentOptionDescription
                    }).ToList()
                
            }).FirstOrDefault();
        }

        public PaymentPageContentsModel GetPaymentPageContents()
        {
            return _cacheProvider.GetCached(() => GetPaymentPageContentsInternal(),
                "getpaymentpagecontents",
                $"{new GenericDependencyBuilder<PaymentPage>(_siteContext.SiteName).DependsOnAllNodesOfPageType()}\n{new GenericDependencyBuilder<PaymentOption>(_siteContext.SiteName).DependsOnAllNodesOfPageType()}");
        }
    }
}
