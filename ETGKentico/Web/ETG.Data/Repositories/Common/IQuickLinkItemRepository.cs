using System.Collections.Generic;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public interface IQuickLinkItemRepository
    {
        List<QuickLinkItemModel> GetQuickLinkItems(string path);
    }
}