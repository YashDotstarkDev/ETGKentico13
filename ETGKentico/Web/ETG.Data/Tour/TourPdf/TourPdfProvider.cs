using CMS.Core;
using CMS.Helpers;
using CMS.IO;
using CMS.SiteProvider;
using ETG.Data.Tour.Services;
using ETG.PDF;

namespace ETG.Data.Tour.TourPdf
{
    public class TourPdfProvider : ITourPdfProvider
    {
        private const string PDF_DIRECTORY_PATH = "/etg/pdf/";
        private readonly ITourService _tourService;
        private readonly IPDFGenerator _pdfGenerator;

        public TourPdfProvider(ITourService tourService, IPDFGenerator pdfGenerator)
        {
            _tourService = tourService;
            _pdfGenerator = pdfGenerator;
        }

        public TourPdf GetTourPdf(string tourCode, bool forceRegenerate)
        {
            var pdfPath = $"{PDF_DIRECTORY_PATH}{tourCode}.pdf";

            if (!forceRegenerate)
            {
                var existingFile = StorageHelper.GetFileInfo(pdfPath);
                if (existingFile.Exists)
                {
                    return new TourPdf(pdfPath);
                }
            }

            var tour = _tourService.GetTourByTourCode(tourCode);
            if (tour == null)
            {
                return null;
            }

            var url = URLHelper.GetAbsoluteUrl($"~/tourprint/{tour.PageAlias?.ToLower()}", SiteContext.CurrentSite.SitePresentationURL);

            var ms = _pdfGenerator.GeneratePDF(url);
            StorageHelper.SaveFileToDisk(pdfPath, BinaryData.GetByteArrayFromStream(ms));

            return new TourPdf(pdfPath);
        }
    }
}