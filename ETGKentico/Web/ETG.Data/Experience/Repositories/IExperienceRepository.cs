using ETG.Data.Experience.Models;
using System.Collections.Generic;

namespace ETG.Data.Experience.Repositories
{
    public interface IExperienceRepository
    {
        ExperienceModel GetExperience(string path);

        List<ExperienceSummaryModel> GetExperiences(string path);
    }
}
