using CMS.DocumentEngine;
using System;

namespace ETG.Data.Services
{
    public interface IDocumentService
    {
        TreeNode GetSingleDocument(Guid guid);
        TreeNode GetSingleDocument(string path);
    }
}
