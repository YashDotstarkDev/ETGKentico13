using AutoMapper;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Repositories;
using ETG.Data.Models.Common;

namespace ETG.Data.Services
{
    public class ContactService : IContactService
    {
        public readonly IContactRepository ContactRepository;
        public readonly ICacheService CacheService;

        private readonly IMapper _mapper;

        public ContactService(IMapper mapper,IContactRepository contactRepository, ICacheService cacheService)
        {
            CacheService = cacheService;
            ContactRepository = contactRepository;
            _mapper = mapper;
        }

        public ContactModel GetETGContactInfo()
        {
            return CacheService.GetDocumentDependentOnPath(
                        () => ContactRepository.GetContact(PathConstants.PATH_CONTACT), "ETGContactInfo", PathConstants.PATH_CONTACT);
        }
    }
}