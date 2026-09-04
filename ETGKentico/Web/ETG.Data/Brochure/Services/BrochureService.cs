using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CMS.Base;
using Devotion.Web.Base.Extensions;
using ETG.Core.Constants;
using ETG.Core.Services;
using ETG.Data.Brochure.Models;
using ETG.Data.Brochure.Repositories;
using ETG.Data.Cache;

namespace ETG.Data.Brochure.Services
{
    public class BrochureService : IBrochureService
    {
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;
        private readonly IBrochureRepository _brochureRepository;

        public BrochureService(ICacheService cacheService, IBrochureRepository brochureRepository, IEmailService emailService)
        {
            _cacheService = cacheService;
            _brochureRepository = brochureRepository;
            _emailService = emailService;
        }

        private List<BrochureModel> GetAllBrochuresInternal()
        {
            return _brochureRepository.GetBrochures(PathConstants.PATH_TOUR_BROCHURES);
        }

        public List<BrochureModel> GetAllBrochures()
        {
            return _cacheService.GetDocumentDependentOnAll(GetAllBrochuresInternal,
                "GetAllBrochures", Core.PageTypes.Brochure.CLASS_NAME);
        }

        public BrochureModel GetBrochure(Guid guid)
        {
            return GetAllBrochures().FirstOrDefault(a => a.BrochureNodeGuid == guid);
        }

        public IEnumerable<BrochureModel> GetBrochures(List<Guid> guids)
        {
            return guids.IsNullOrEmpty()
                ? Enumerable.Empty<BrochureModel>()
                : GetAllBrochures().Where(a => guids.Contains(a.BrochureNodeGuid)).ToList();
        }

        public BrochureModel GetBrochureByDestinationGuid(Guid guid)
        {

            return GetAllBrochures()
                .FirstOrDefault(a =>
                    a.DestinationGuids != null && a.DestinationGuids.ToLower().Contains(guid.ToString()));
        }

        public BrochureModel GetBrochure(string alias)
        {
            return alias.IsNullOrEmpty()
                ? null
                : GetAllBrochures().FirstOrDefault(a => a.PageAlias.ToLower() == alias.ToLower());
        }

        public void SendOrderResponse(string firstName, string email, string brochureGuids)
        {

            if (brochureGuids.IsNullOrEmpty())
            {
                return;
            }

            var links = GetBrochureLinks(brochureGuids);

            var param  = new Dictionary<string,string>();
            param.Add("firstname", firstName);
            param.Add("links", links);
            _emailService.SendEmail("BrochureOrderTemplate", email, param);
        }

        private string GetBrochureLinks(string brochureGuids)
        {
            if (brochureGuids.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var brochures = GetBrochures(brochureGuids.Split(',').Select(a => a.ToGuid()).ToList()).ToList();

            if (!brochures.Any())
            {
                return string.Empty;
            }

            var list = brochures.Select(CreateLink).ToList();
            if (list.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return string.Join("<br>", list);
        }

        private string CreateLink(BrochureModel brochure)
        {
            return $"<a href=\"https://www.entiretravel.com.au/brochures/{brochure.PageAlias?.ToLower()}\">{brochure.Name}</a>";
        }
    }
}