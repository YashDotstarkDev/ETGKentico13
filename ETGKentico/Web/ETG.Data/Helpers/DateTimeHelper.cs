using System;

namespace ETG.Data.Helpers
{
    public static class DateTimeHelper
    {
        public static bool IsDSTDate(DateTime dt)
        {
            if (dt >= new DateTime(2022, 10, 2) && dt < new DateTime(2023, 4, 2) ||
                dt >= new DateTime(2023, 10, 1) && dt < new DateTime(2024, 4, 7) ||
                dt >= new DateTime(2024, 10, 6) && dt < new DateTime(2025, 4, 6))
            {
                return true;
            }
            return false;
        }
        public static string GetGMTTime(DateTime dt)
        {
            var plusTime = "+10:00";

            if (IsDSTDate(dt))
            {
                plusTime = "+11:00";
            }

            return $"{dt.ToString("r")}{plusTime}";
        }
    }
}