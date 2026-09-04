using System.Collections.Generic;
using ETG.Module.Booking.Constants;

namespace ETG.Module.Booking.Booking
{
    public class PrePostNightsPriceContainer
    {
        private Dictionary<string, double> _priceDictionary;
        public PrePostNightsPriceContainer(double twinPrePrice, double twinPostPrice, double singlePrePrice,
            double singlePostPrice)
        {
            _priceDictionary = new Dictionary<string, double>();
                
            _priceDictionary.Add(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE, twinPrePrice);
            _priceDictionary.Add(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE, twinPostPrice);
            _priceDictionary.Add(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE, singlePrePrice);
            _priceDictionary.Add(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE, singlePostPrice);
        }

        public double TwinPrePrice
        {
            get
            {
                if (_priceDictionary.ContainsKey(BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE))
                    return _priceDictionary[BookingConstants.CUSTOM_COLUMN_TWINPRENIGHTS_BASEPRICE];

                return 0;
            }
        }
        
        public double TwinPostPrice
        {
            get
            {
                if (_priceDictionary.ContainsKey(BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE))
                    return _priceDictionary[BookingConstants.CUSTOM_COLUMN_TWINPOSTNIGHTS_BASEPRICE];

                return 0;
            }
        }
        
        public double SinglePrePrice
        {
            get
            {
                if (_priceDictionary.ContainsKey(BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE))
                    return _priceDictionary[BookingConstants.CUSTOM_COLUMN_SINGLEPRENIGHTS_BASEPRICE];

                return 0;
            }
        }
        
        public double SinglePostPrice
        {
            get
            {
                if (_priceDictionary.ContainsKey(BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE))
                    return _priceDictionary[BookingConstants.CUSTOM_COLUMN_SINGLEPOSTNIGHTS_BASEPRICE];

                return 0;
            }
        }
    }
}