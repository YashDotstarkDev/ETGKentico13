using CMS.DocumentEngine;
using CMS.Search;
using System.Collections.Generic;

namespace ETG.Core.Search
{
    public class ETGSearchTaskCreator : IETGSearchTaskCreator
    {
        public void CreateSearchTask(TreeNode treeNode, SearchTaskTypeEnum taskType, string objectType = "cms.document")
        {
            List<SearchTaskCreationParameters> creationParametersList = new List<SearchTaskCreationParameters>();

            if (treeNode != null && treeNode.PublishedVersionExists)
            {
                SearchTaskCreationParameters creationParameters = new SearchTaskCreationParameters()
                {
                    TaskType = taskType,
                    ObjectType = objectType,
                    ObjectField = "_id",
                    TaskValue = treeNode.GetSearchID(),
                    RelatedObjectID = treeNode.DocumentID
                };
                creationParametersList.Add(creationParameters);
            }
            SearchTaskInfoProvider.CreateTasks((ICollection<SearchTaskCreationParameters>)creationParametersList, new bool?(true));
        }
    }
}
