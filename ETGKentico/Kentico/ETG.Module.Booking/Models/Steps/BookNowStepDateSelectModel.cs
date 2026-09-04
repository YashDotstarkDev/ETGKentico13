using System;
using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepDateSelectModel : BookNowStepModel, IDataModel
    {
        public DateTime SelectedDate { get; set; }

        public string FormattedSelectedDate
        {
            get
            {
                if (SelectedDate == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return SelectedDate.ToString("dd/MM/yyyy");
            }
        }
        
        public string DisplaySelectedDate
        {
            get
            {
                if (SelectedDate == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return SelectedDate.ToString("dddd, dd MMM yyyy");
            }
        }
    }
}