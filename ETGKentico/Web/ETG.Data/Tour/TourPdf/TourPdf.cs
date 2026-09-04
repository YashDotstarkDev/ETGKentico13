using CMS.IO;

namespace ETG.Data.Tour.TourPdf
{
    public class TourPdf
    {
        private readonly string _pdfPath;

        public TourPdf(string pdfPath)
        {
            _pdfPath = pdfPath;
        }

        public FileStream GetFileStream()
        {
            return StorageHelper.GetFileStream(_pdfPath, FileMode.Open);
        }
    }
}