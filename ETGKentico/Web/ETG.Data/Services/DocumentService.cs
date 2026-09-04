using System;
using System.Linq;
using CMS.DocumentEngine;
using ETG.Data.Cache;

namespace ETG.Data.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ICacheService _cacheService;
        public DocumentService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        private TreeNode GetSingleDocumentInternal(Guid guid)
        {
            return DocumentHelper.GetDocuments().OnCurrentSite().WhereEquals("NodeGuid", guid).FirstOrDefault();
        }
        private TreeNode GetSingleDocumentInternal(string path)
        {
            return DocumentHelper.GetDocuments().Path(path).OnCurrentSite().FirstOrDefault();
        }

        public TreeNode GetSingleDocument(Guid guid)
        {
            return _cacheService.GetDocumentDependentOnGuid(() => GetSingleDocumentInternal(guid), "document", guid);
        }
        public TreeNode GetSingleDocument(string path)
        {
            return _cacheService.GetDocumentDependentOnPath(() => GetSingleDocumentInternal(path), "document", path);
        }
    }
}
