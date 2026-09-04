using Castle.Core.Internal;
using ETG.Booking.Pricing.Enums;

namespace ETG.Data.Extensions
{
    public static class CurrencyExtensions
    {
        public static string CurrencySymbol(this string currency)
        {
            return CurrencyConstants.GetSymbol(currency);
        }

        public static string DefaultCurrencyIfBlank(this string currency)
        {
            if (currency.IsNullOrEmpty())
            {
                return CurrencyConstants.CODE_AUD;
            }
            return currency;
        }

        public static string FormatPrice(this int price, bool showZero =false, string currencySymbol = "$")
        {
            if (currencySymbol.Length > 1)
            {
                currencySymbol = $"{currencySymbol} ";
            }
            if (price == 0)
            {
                if (!showZero)
                {
                    return string.Empty;
                }

                return $"{currencySymbol}0";
            }
            return $"{currencySymbol}{price:#,###}";
        }
        
        public static string FormatPrice(this double price, bool showZero =false, string currencySymbol = "$")
        {
            if (currencySymbol.Length > 1)
            {
                currencySymbol = $"{currencySymbol} ";
            }
            if (price == 0)
            {
                if (!showZero)
                {
                    return string.Empty;
                }

                return $"{currencySymbol}0";
            }
            return $"{currencySymbol}{price:#,###}";
        }

        public static string FormatDecimalPrice(this double price, bool showZero = false, string currencySymbol = "$")
        {
            if (currencySymbol.Length > 1)
            {
                currencySymbol = $"{currencySymbol} ";
            }
            if (price == 0)
            {
                if (!showZero)
                {
                    return string.Empty;
                }

                return $"{currencySymbol}0.00";
            }
            return $"{currencySymbol}{price:#,##0.00}";
        }

        public static string FormatDecimalPrice(this int price, bool showZero = false, string currencySymbol = "$")
        {
            if (currencySymbol.Length > 1)
            {
                currencySymbol = $"{currencySymbol} ";
            }
            if (price == 0)
            {
                if (!showZero)
                {
                    return string.Empty;
                }

                return $"{currencySymbol}0.00";
            }
            return $"{currencySymbol}{price:#,##0.00}";
        }
        
        public static string FormatPrice(this decimal price, string currencySymbol = "$")
        {
            if (currencySymbol.Length > 1)
            {
                currencySymbol = $"{currencySymbol} ";
            }
            if (price == 0)
            {
                return string.Empty;
            }
            return $"{currencySymbol}{price:#,##0.00}";
        }
    }
}
