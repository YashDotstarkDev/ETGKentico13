using ETG.Web.Models.Common;
using ETG.Web.Models.Menu;
using System.Collections.Generic;
using Castle.Core.Internal;

namespace ETG.Web.Models.Layout
{
    public class LabelUrl
    {
        public string url { get; set; }
        public string label { get; set; }
    }

    public class FooterViewModel
    {
        public ContactViewModel Contact { get; set; }
        public List<MenuGroup> TopMenus;
        public List<LinkViewModel> BottomMenus;
        public List<ImageViewModel> BottomLogos { get; set; }

        public Dictionary<string, object> GetMenuAstroProps(MenuGroup menuGroup)
        {
            if (menuGroup == null) { return new Dictionary<string, object>(); }

            var urls = new List<LabelUrl>();

            if (menuGroup.Link != null)
            {
                urls.Add(new LabelUrl
                {
                    label = menuGroup.Link.Label,
                    url = string.Empty,
                });
            }

            if (!menuGroup.ChildLinks.IsNullOrEmpty())
            {
                foreach (var link in menuGroup.ChildLinks)
                {
                    urls.Add(new LabelUrl { label = link.Label, url = link.Path });
                }
            }

            var props = new Dictionary<string, object>
            {
                { "options", urls }
            };

            return props;

        }
    }
}