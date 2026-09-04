using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CMS.DocumentEngine;
using Devotion.Web.Base.Extensions;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Global;

namespace ETG.Data.Global
{
    public class LinkRepository : ILinkRepository
    {
        public List<LinkModel> GetLinks(string path)
        {
            return LinkProvider
                .GetLinks()
                .Path(path.BeginWithSlash(), PathTypeEnum.Section)
                .OrderBy("NodeLevel,NodeOrder")
                .OnCurrentSite()
                .Select(s => new LinkModel
                {
                    Id = s.LinkID,
                    NodeId = s.NodeID,
                    NodeParentId = s.NodeParentID,
                    NodeLevel = s.NodeLevel,
                    NodeOrder = s.NodeOrder,
                    Label = s.LinkLabel,
                    NodeAliasPath = s.NodeAliasPath,
                    Path = s.LinkPath.IsNullOrEmpty() ? "" : s.LinkPath.ToLower(),
                    CommonLinkCaption = s.LinkCommonCaption,
                    CommonLinkUrl = s.LinkCommonUrl,
                    LinkGroupType = s.LinkGroupType,
                    IconClass = s.LinkIconClass,
                    SVGIcon = s.LinkSVGIcon
                }).ToList();
        }
    }
}
