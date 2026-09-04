namespace ETG.Data.Tour.TourPdf
{
    public interface ITourPdfProvider
    {
        TourPdf GetTourPdf(string tourCode, bool forceRegenerate);
    }
}