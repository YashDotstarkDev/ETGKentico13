using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Experience.Models;

namespace ETG.Data.Experience.Services
{
    public interface IExperienceService
    {
        List<ExperienceSummaryModel> GetAllExperienceSummaries();
        List<ExperienceSummaryModel> GetPreviewableAllExperienceSummaries();
        ExperienceSummaryModel GetExperienceSummary(Guid guid);
        IEnumerable<ExperienceSummaryModel> GetExperienceSummaries(List<Guid> guids);
        IEnumerable<ExperienceSummaryModel> GetPreviewableExperienceSummaries(List<Guid> guids);
        bool IsMainExperience(string experienceName);
    }
}
