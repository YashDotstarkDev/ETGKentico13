using ETG.Data.Cache;
using ETG.Data.Career.Models;
using ETG.Data.Career.Repositories;

namespace ETG.Data.Career.Services
{
    public class CareerService : ICareerService
    {
        private readonly ICacheService _cacheService;
        private readonly ICareerRoleRepository _careerRoleRepository;
        public CareerService(ICacheService cacheService, ICareerRoleRepository careerRoleRepository)
        {
            _cacheService = cacheService;
            _careerRoleRepository = careerRoleRepository;
        }

        public CareerRoleModel GetCareerRole(string path)
        {
            return _cacheService.GetDocumentDependentOnPath(() => _careerRoleRepository.GetCareerRole(path), "CareerRole", path);
        }
    }
}
