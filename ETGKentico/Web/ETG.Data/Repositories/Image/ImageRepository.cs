using System.Collections.Generic;
using System.Linq;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Image
{
    public class ImageRepository : IImageRepository
    {
        public List<ImageModel> GetImages(string path)
        {
            return ImageProvider.GetImages()
                .OnCurrentSite()
                .Path(path, CMS.DocumentEngine.PathTypeEnum.Children)
                .OrderBy(nameof(Core.PageTypes.Image.NodeOrder))
                .Select(a => new ImageModel
                {
                    ImagePath = a.ImageFilePath,
                    ImageAltText = a.ImageAltText,
                    ImageCaption = a.ImageCaption
                }).ToList();
        }
    }
}