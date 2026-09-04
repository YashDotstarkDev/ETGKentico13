using System.Collections.Generic;
using CMS.DocumentEngine;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;


namespace ETG.Data.Repositories.Common
{
    public class QuickLinkItemRepository : IQuickLinkItemRepository
    {
        public List<QuickLinkItemModel> GetQuickLinkItems(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return null;
            }

            var nodeLevel = path.Split('/').Length;

            return QuickLinkItemProvider.GetQuickLinkItems().Path(path, PathTypeEnum.Children)
                //.WhereEquals("NodeLevel", nodeLevel).OnCurrentSite()
                .OrderBy("NodeOrder")
                .Select(a => new QuickLinkItemModel
                {
                    Heading = a.Heading,
                    SectionIdName = a.SectionIdName
                }).ToList();
        }
    }
}