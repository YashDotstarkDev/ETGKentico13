using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Models;

namespace ETG.Data.Repositories
{
    public interface ISitemapRepository
    {
        List<SiteMapItemModel> GetSiteMapItems();
    }
}
