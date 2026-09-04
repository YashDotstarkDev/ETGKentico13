using Algolia.Search.Models.ApiKeys;
using CMS.DataEngine;
using CMS.Globalization;
using DocumentFormat.OpenXml.Wordprocessing;
using ETG.Data.Models.Api;
using ETG.Data.Repositories._Interfaces;
using ETG.Data.Settings;
using LinqToTwitter;
using MaxMind.GeoIP2;
using Microsoft.Azure.KeyVault.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CMS.Helpers;
using ETG.Data.Helpers;
using CMS.WebAnalytics;

namespace ETG.Data.Repositories
{
    public class CountryChangeRepository : ICountryChangeRepository
    {
        public async Task<CountryInforModel> GetCountryInfoAsync(string IpAddress)
        {
            CountryInfo countryInfo = new CountryInfo();
            if (!string.IsNullOrEmpty(IpAddress))
            {
                var currUserLoc = GeoIPHelper.GetLocationByIp(IpAddress);
                if (currUserLoc != null)
                {
                    return new CountryInforModel
                    {
                        Status = true,
                        Code = ValidationHelper.GetString(currUserLoc.CountryCode, string.Empty),
                        Country = ValidationHelper.GetString(currUserLoc.CountryName, string.Empty),
                        //Ip = IpAddress
                    };
                }
            }
            return new CountryInforModel
            {
                Country = "Country not found",
                Code = "N/A",
                //Ip = IpAddress
            };
        }

        private CountryInforModel GetCountryInfo(JArray addrComponents)
        {
            foreach (var component in addrComponents)
            {
                var types = component["types"]?.ToObject<JArray>();
                if (types != null && types.Count > 0)
                {
                    if (types[0].ToString() == "country")
                    {
                        return new CountryInforModel
                        {
                            Status = true,
                            Country = component["long_name"]?.ToString(),
                            Code = component["short_name"]?.ToString()
                        };
                    }
                    if (types.Count == 2 && types[0].ToString() == "political")
                    {
                        // Optional: Check if the political type is indicative of a country
                        return new CountryInforModel
                        {
                            Status = true,
                            Country = component["long_name"]?.ToString(),
                            Code = component["short_name"]?.ToString()
                        };
                    }
                }
            }
            return null; // Return null if country information is not found
        }
    }
}

