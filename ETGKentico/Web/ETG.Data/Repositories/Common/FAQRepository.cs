using ETG.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;

namespace ETG.Data.Repositories.Common
{
    public class FAQRepository : IFAQRepository
    {
        public List<FAQModel> GetFAQs(string path, int? count = null)
        {
            var query = FAQProvider
                .GetFAQs()
                .Path(path, PathTypeEnum.Children)
                .OnCurrentSite()
                .OrderBy(nameof(FAQ.NodeOrder));

            if (count.HasValue)
            {
                query.TopN(count.Value);
            }

            return query.Select(
                a => new FAQModel
                {
                    Question = a.FAQQuestion,
                    Answer = a.FAQAnswer
                }).ToList();
        }
    }
}