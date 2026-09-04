using ETG.Data.Brochure.Models;
using System;
using System.Collections.Generic;

namespace ETG.Data.Brochure.Repositories
{
    public interface IBrochureRepository
    {
        List<BrochureModel> GetBrochures(string path);
        List<BrochureModel> GetBrochures(List<Guid> brochureGuids);
    }
}
