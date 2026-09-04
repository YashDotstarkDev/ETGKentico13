using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Cache;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Experience.Models;
using ETG.Data.Factories;

namespace ETG.Data.Experience.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly ICacheService _cacheService;
        public ExperienceService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }


        private List<ExperienceSummaryModel> GetAllExperiencesInternal()
        {
            return ExperienceProvider.GetExperiences().OnCurrentSite().OrderBy("NodeOrder").Select(
                ModelFactory.CreateExperienceSummaryModel).ToList();
        }

        private List<ExperienceSummaryModel> GetPreviewableAllExperiencesInternal()
        {
            return ExperienceProvider.GetPreviewableExperiences().OnCurrentSite().OrderBy("NodeOrder").Select(
                ModelFactory.CreateExperienceSummaryModel).ToList();
        }
        public List<ExperienceSummaryModel> GetAllExperienceSummaries()
        {
            return _cacheService.GetDocumentDependentOnAll(() => GetAllExperiencesInternal(),
                "GetAllExperienceSummaries", Core.PageTypes.DestinationExpertTeam.CLASS_NAME);
        }
        public List<ExperienceSummaryModel> GetPreviewableAllExperienceSummaries()
        {
            return _cacheService.GetDocumentDependentOnAll(() => GetPreviewableAllExperiencesInternal(),
                "GetPreviewableAllExperienceSummaries", Core.PageTypes.DestinationExpertTeam.CLASS_NAME);
        }
        public ExperienceSummaryModel GetExperienceSummary(Guid guid)
        {
            return GetAllExperienceSummaries().FirstOrDefault(a => a.NodeGuid == guid);
        }

        public IEnumerable<ExperienceSummaryModel> GetExperienceSummaries(List<Guid> guids)
        {
            if (guids.IsNullOrEmpty())
            {
                return Enumerable.Empty<ExperienceSummaryModel>();

            }
            return GetAllExperienceSummaries().Where(a => guids.Contains(a.NodeGuid)).ToList();
        }
        public IEnumerable<ExperienceSummaryModel> GetPreviewableExperienceSummaries(List<Guid> guids)
        {
            if (guids.IsNullOrEmpty())
            {
                return Enumerable.Empty<ExperienceSummaryModel>();

            }
            return GetPreviewableAllExperienceSummaries().Where(a => guids.Contains(a.NodeGuid)).ToList();
        }

        public bool IsMainExperience(string experienceName)
        {
            return GetAllExperienceSummaries().Any(a => a.Path.Split('/')[2].Trim().ToLower().Equals(experienceName.Trim().ToLower()));
        }
    }
}
