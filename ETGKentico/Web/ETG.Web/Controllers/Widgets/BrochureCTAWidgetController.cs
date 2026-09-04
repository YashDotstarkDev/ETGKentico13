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
using ETG.Data.Repositories.Common;
using ETG.Data.Services;
using ETG.Web.Models.Widgets.BrochureCTAWidget;

[assembly: RegisterWidget("ETG.Web.Widget.BrochureCTAWidget", typeof(BrochureCTAWidgetController), "Brochure CTA Widget")]

namespace ETG.Web.Controllers.Widgets
{
    public class BrochureCTAWidgetController : WidgetController<BrochureCTAWidgetProperties>
    {
        private readonly IFAQRepository _faqRepository;
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private IContactService _contactService;

        public BrochureCTAWidgetController(IMapper mapper, IFAQRepository faqRepository, ICacheService cacheService,
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

            if (properties.CtaLabel.IsNullOrEmpty())
            {
                return null;
            }

            var viewModel = new BrochureCTAWidgetViewModel()
            {
                Heading = properties.Heading,
                CTALabel = properties.CtaLabel,
                CTAUrl = properties.CtaUrl
            };
            return PartialView("Widgets/_BrochureCTAWidget", viewModel);
        }
    }
}