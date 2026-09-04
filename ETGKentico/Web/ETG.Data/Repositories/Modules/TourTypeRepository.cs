using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Models.Modules;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Repositories.Modules
{
    public class TourTypeRepository : ITourTypeRepository
    {
        private readonly ICacheService _cacheService;
        public TourTypeRepository(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        private List<TourTypeModel> GetAllTourTypesInternal()
        {
            return TourTypeInfoProvider.GetTourTypes().OrderBy("TourTypeName").Select(
                a => new TourTypeModel
                {
                    ItemGuid = a.TourTypeGuid,
                    Name = a.TourTypeName,
                    IconClass = a.TourTypeIcon,
                    CodeName = a.TourTypeCodeName,
                    IconWhiteImage = a.TourTypeWhiteIconImage,
                    IconBlackImage = a.TourTypeBlackIconImage,
                    TourTypeDescription = a.TourTypeDescription,
                    Url = a.TourTypeUrl
                }).ToList();
        }

        public List<TourTypeModel> GetAllTourTypes()
        {
            return _cacheService.GetAllObjectDependency(GetAllTourTypesInternal,
                "alltourtypes", TourTypeInfo.OBJECT_TYPE);
        }
        
        public IEnumerable<TourTypeModel> GetTourTypes(List<string> codeNames)
        {
            if (codeNames.IsNullOrEmpty())
            {
                return Enumerable.Empty<TourTypeModel>();

            }
            return GetAllTourTypes().Where(a => codeNames.Contains(a.CodeName)).ToList();
        }
        public TourTypeModel GetTourType(string codeName)
        {
            if (codeName.IsNullOrEmpty())
            {
                return null;

            }
            return GetAllTourTypes().Where(a => a.CodeName == codeName).FirstOrDefault();
        }
    }
}