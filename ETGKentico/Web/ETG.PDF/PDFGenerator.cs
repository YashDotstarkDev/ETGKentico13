using System.IO;
using SelectPdf;

namespace ETG.PDF
{
    public class PDFGenerator : IPDFGenerator
    {
        public MemoryStream GeneratePDF(string fullPath)
        {
            var converter = new HtmlToPdf();
            SelectPdf.GlobalProperties.LicenseKey = "2PPp+Ort6fjp7OH46eH26Pjr6fbp6vbh4eHh";
            converter.Options.JavaScriptEnabled = true;
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayCutText = true;
            converter.Options.DisplayFooter = false;
            converter.Options.DisplayHeader = false;
            converter.Options.DrawBackground = true;
            converter.Options.InternalLinksEnabled = true;
            converter.Options.EmbedFonts = true;
            converter.Options.KeepImagesTogether = true;
            converter.Options.JpegCompressionEnabled = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.PageBreaksEnhancedAlgorithm = true;
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MinPageLoadTime = 3;
            MemoryStream ms = new MemoryStream();
            var doc = converter.ConvertUrl(fullPath);
            doc.Save(ms);
            doc.Close();

            return ms;
        }
    }
}
