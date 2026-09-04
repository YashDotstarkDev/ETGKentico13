using System.IO;

namespace ETG.PDF
{
    public interface IPDFGenerator
    {
        MemoryStream GeneratePDF(string fullPath);
    }
}
