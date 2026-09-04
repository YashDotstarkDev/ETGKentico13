using CMS.DocumentEngine;

namespace Devotion.Data.Pages
{
    public interface IPageMetadataProvider
    {
        PageMetadata GetPageMetadata(TreeNode node);
    }
}
