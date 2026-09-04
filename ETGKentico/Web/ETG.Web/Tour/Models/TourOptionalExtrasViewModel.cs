using System;
using System.Linq;
using Castle.Core.Internal;
using ETG.Web.Models;

namespace ETG.Web.Tour.Models
{
    public class TourOptionalExtrasViewModel : IViewModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string ImageCaption { get; set; }

        public string Description
        {
            get; set; 
        }
        public string Duration { get; set; }
        public string PriceStatements { get; set; }
        public string CostPerPerson { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }

        public string AvailableDaysOfWeek { get; set; }
        public string AvailableDateRange
        {
            get
            {
                if (AvailableFrom == DateTime.MinValue || AvailableTo == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return $"{AvailableFrom:dd MMMM yyyy} to {AvailableTo:dd MMMM yyyy}";
            }
        }

        public string AvailabledaysOfWeekText
        {
            get
            {
                if (AvailableDaysOfWeek.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                var arr = AvailableDaysOfWeek.Split('|');

                if (arr.Length == 7)
                {
                    return string.Empty;
                }

                for (var i = 0; i < arr.Length; i++)
                {
                    arr[i] = ConvertDayToText(arr[i]);
                }

                return string.Join(", ", arr);
            }
        }

        private string ConvertDayToText(string s)
        {
            switch (s)
            {
                case "1":
                    return "Monday";
                case "2":
                    return "Tuesday";
                case "3":
                    return "Wednesday";
                case "4":
                    return "Thursday";
                case "5":
                    return "Friday";
                case "6":
                    return "Saturday";
                case "0":
                    return "Sunday";
                default:
                    return string.Empty;
            }
        }
    }
}
