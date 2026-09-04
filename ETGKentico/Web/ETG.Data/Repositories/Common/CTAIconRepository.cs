using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using Devotion.Web.Base.Extensions;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public class CTAIconRepository : ICTAIconRepository
    {
        public List<CTAIconModel> Get(string url, string path = "")
        {
            return CTAIconProvider.GetCTAIcons()
                .Path(path.BeginWithSlash(), PathTypeEnum.Children)
                .OrderBy("NodeOrder")
                .OnCurrentSite()
                .FilterDuplicates()
                .Published()
                .Select(s => new CTAIconModel
                {
                    Label = s.CTAIconLabel,
                    Url = s.CTAIconUrl,
                    IconClass = s.CTAIconIcon,
                    SvgIcon = s.CtaSvgIcon,
                })
                .ToList();
        }
    }
}
