using CMS.DocumentEngine;
using CMS.Search;

namespace ETG.Core.Search
{
    public interface IETGSearchTaskCreator
    {
        void CreateSearchTask(TreeNode treeNode, SearchTaskTypeEnum taskType, string objectType = "cms.document");
    }
}
