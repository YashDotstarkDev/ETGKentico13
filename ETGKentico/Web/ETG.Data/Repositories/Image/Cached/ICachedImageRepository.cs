using System.Collections.Generic;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Image.Cached
{
    public interface ICachedImageRepository
    {
        List<ImageModel> GetImages(string path);
    }
}