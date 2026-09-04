using CMS.DocumentEngine;

namespace Devotion.Data.Pages
{
    public class PageMetadataProvider : IPageMetadataProvider
    {
        public PageMetadata GetPageMetadata(TreeNode node)
        {
            return new PageMetadata
            {
                PageTitle = node.GetStringValue(nameof(node.DocumentPageTitle), null) ??
                            (string) node.GetInheritedValue(nameof(node.DocumentPageTitle)),
                PageDescription = node.GetStringValue(nameof(node.DocumentPageDescription), null) ??
                                  (string) node.GetInheritedValue(nameof(node.DocumentPageDescription)),
                PageKeywords = node.GetStringValue(nameof(node.DocumentPageKeyWords), null) ??
                               (string) node.GetInheritedValue(nameof(node.DocumentPageKeyWords))
            };
        }
    }
}