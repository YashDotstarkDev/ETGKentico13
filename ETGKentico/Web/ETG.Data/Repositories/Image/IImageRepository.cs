using System.Collections.Generic;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Image
{
    public interface IImageRepository
    {
        List<ImageModel> GetImages(string path);
    }
}