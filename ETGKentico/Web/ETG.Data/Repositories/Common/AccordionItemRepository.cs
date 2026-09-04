using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DocumentEngine;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public class AccordionItemRepository : IAccordionItemRepository
    {
        public List<AccordionItemModel> GetAccorionItems(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return null;
            }

            var nodeLevel = path.Split('/').Length;

            return AccordionItemProvider.GetAccordionItems().Path(path, PathTypeEnum.Children).WhereEquals("NodeLevel", nodeLevel).OnCurrentSite()
                .Select(a => new AccordionItemModel
                {
                    Heading = a.AccordionHeading,
                    Contents = a.AccordionContents
                }).ToList();
        }
    }
}
