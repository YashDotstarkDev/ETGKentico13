using ETG.Data.Models.Modules;
using System;
using System.Collections.Generic;

namespace ETG.Data.Repositories.Modules
{
    public interface ITourTypeRepository
    {
        IEnumerable<TourTypeModel> GetTourTypes(List<string> codeNames);
        List<TourTypeModel> GetAllTourTypes();
        TourTypeModel GetTourType(string codeName);
    }
}
