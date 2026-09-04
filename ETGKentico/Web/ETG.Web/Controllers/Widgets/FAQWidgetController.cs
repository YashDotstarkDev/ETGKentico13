using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Repositories.Image;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Common;
using ETG.Web.Models.Widgets.FAQWidget;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using CMS.DocumentEngine;
using ETG.Data.Repositories.Common;
using ETG.Data.Services;
using GlobalLink.Connect.TargetServiceRef;
using Kentico.Content.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.FAQWidget", typeof(FAQWidgetController), "FAQ Widget")]

namespace ETG.Web.Controllers.Widgets
{
    public class FAQWidgetController : WidgetController<FAQWidgetProperties>
    {
        private readonly IFAQRepository _faqRepository;
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private IContactService _contactService;

        public FAQWidgetController(IMapper mapper, IFAQRepository faqRepository, ICacheService cacheService,
            IContactService contactService)
        {
            _faqRepository = faqRepository;
            _cacheService = cacheService;
            _mapper = mapper;
            _contactService = contactService;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();

            if (properties.Path.IsNullOrEmpty())
            {
                return null;
            }

            var path = properties.Path[0].NodeAliasPath;
            var faqs = _cacheService.GetDocumentMultipleDependency(() => _faqRepository.GetFAQs(path, properties.Count),
                $"faqwidget_{path}_{properties.Count}", path);
            var contactModel = _contactService.GetETGContactInfo();
            var viewModel = new FAQWidgetViewModel
            {
                ShowViewAllButton = !properties.HideViewAll,
                FAQsPageUrl = contactModel.FAQsUrl,
                FAQs = _mapper.Map<List<FAQViewModel>>(faqs),
                Heading = properties.Heading,
            };
            return PartialView("Widgets/_FAQWidget", viewModel);
        }
    }
}