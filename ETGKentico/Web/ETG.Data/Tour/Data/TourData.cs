using System;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Tour.Data
{
    public static class TourData
    {
        public enum FlightClass { ECONOMY, PREMIUM, BUSINESS_CLASS, FIRST_CLASS }

        private static Dictionary<FlightClass, string> FlightDictionary = new Dictionary<FlightClass, string>
        {
            { FlightClass.ECONOMY, "Economy"},
            { FlightClass.PREMIUM, "Premium Economy"},
            { FlightClass.BUSINESS_CLASS, "Business Class"},
            { FlightClass.FIRST_CLASS, "First Class"},
        };

        public static List<Tuple<int, string, string>> CruiseTypes = new List<Tuple<int, string, string>>
        {
            new Tuple<int, string, string>( 1, "Self-Drive", "icon-self-drive-cruising"),
            new Tuple<int, string, string>( 2, "Barge", "icon-barge-cruising"),
            new Tuple<int, string, string>( 3, "River", "icon-river-cruising"),
            new Tuple<int, string, string>( 4, "Ocean", "icon-ocean-cruising"),
        };
        public static KeyValuePair<string,string> GetCruiseType(int cruiseTypeId)
        {
            return CruiseTypes.Where(a => a.Item1 == cruiseTypeId).Select(a=>new KeyValuePair<string, string>(a.Item2, a.Item3)).FirstOrDefault();
        }

        public static KeyValuePair<int, string> GetCruiseType(string cruiseTypeName)
        {
            return CruiseTypes.Where(a => a.Item2.Equals(cruiseTypeName, StringComparison.OrdinalIgnoreCase)).Select(a => new KeyValuePair<int, string>(a.Item1, a.Item2)).FirstOrDefault();
        }

        public static string GetFlightClassName(FlightClass flightClass)
        {
            if (FlightDictionary.ContainsKey(flightClass))
            {
                return FlightDictionary[flightClass];
            }

            return string.Empty;
        }
    }
}
