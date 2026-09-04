using ETG.Data.Models.Common;
using System.Collections.Generic;

namespace ETG.Data.Repositories.Common
{
    public interface ICTAImageRepository
    {
        List<CTAImageModel> GetCTAImages(string path);

        List<ImageTileCtaModel> GetImageTileCTAs(string path);
        
    }
}
