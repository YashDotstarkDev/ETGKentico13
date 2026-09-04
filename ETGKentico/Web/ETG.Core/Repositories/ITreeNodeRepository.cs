using CMS.DocumentEngine;

namespace ETG.Core.Repositories
{
    public interface ITreeNodeRepository
    {
        TreeNode CreateNewPage(string className, string documentName, TreeNode parentNode, string templateName);
        
     }
}
