
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ETG.Core.Http;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Services;
using ETG.Data.Models.Common;
using ETG.Data.Repositories;
using ETG.Web.Models.Common;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Partial;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ETG.Web.Controllers.Partial
{
    public class GenericEnquireSidebarController : Controller
    {
        private readonly ILogger _logger;
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public GenericEnquireSidebarController(ILogger logger, IMapper mapper, IContactRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [ChildActionOnly]
        public ActionResult GetEnquireSidebar()
        {
            GenericEnquireSidebarViewModel model = null;
            try
            {
                var enquiryContacts = _repository.GenericEnquireSidebar();
                if (enquiryContacts != null && enquiryContacts.EnquiryContacts != null && enquiryContacts.EnquiryContacts.Any())
                {
                    model = _mapper.Map<GenericEnquireSidebarViewModel>(enquiryContacts);

                }
                ViewBag.HideFaqsButton = true;
                ViewBag.ShowEmailTo = true;
            }
            catch (System.Exception ex)
            {
                _logger.LogException("GenericEnquireSidebarController", "GetEnquireSidebar", ex, string.Empty);
            }
            return View("Partial/Common/_GenericEnquireSidebar", model);

        }
    }
}