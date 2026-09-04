namespace ETG.Booking.Pricing.Enums
{
    
    public static class CurrencyConstants
    {
        public const string CODE_AUD = "AUD";
        public const string CODE_NZD = "NZD";
        public const string CODE_GBP = "GBP";
        public const string CODE_EUR = "EUR";
        public const string CODE_USD = "USD";

        public const string SYMBOL_GBP = "£";
        public const string SYMBOL_NZD = "$";
        public const string SYMBOL_AUD = "$";
        public const string SYMBOL_EUR = "€";
        public const string SYMBOL_USD = "$";

        public static string GetSymbol(string currencyCode)
        {
            switch (currencyCode)
            {
                case CODE_EUR:
                    return SYMBOL_EUR;
                case CODE_USD:
                    return SYMBOL_USD;
                case CODE_NZD:
                    return SYMBOL_NZD;
                case CODE_GBP:
                    return SYMBOL_GBP;
                default:
                    return SYMBOL_AUD;

            }
        }
    }

}