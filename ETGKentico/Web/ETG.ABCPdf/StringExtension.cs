
namespace ETG.ABCPdf
{
    public static class StringExtension
    {
        public static string AddLineSpacing(this string s, int spacing)
        {
            return $"<stylerun linespacing=\"{spacing}\">{s}</stylerun>";
        }

        public static string FormatText(this string s, int spacing, int hPos = -1)
        {
            if (hPos == -1)
            {
                return $"<stylerun linespacing=\"{spacing}\">{s}</stylerun>";
            }
            
            return $"<stylerun linespacing=\"{spacing}\" hpos=\"{hPos}\">{s}</stylerun>";

        }
        
        public static string RemoveQuery(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var index = s.IndexOf("?");

            if (index == -1 || index + 1 == s.Length)
            {
                return s;
            }
            return s.Substring(0, index);
        }
    }
}
