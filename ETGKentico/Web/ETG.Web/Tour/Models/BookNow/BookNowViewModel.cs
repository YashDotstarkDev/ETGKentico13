using ETG.Module.Booking.Models;
using ETG.Web.Models.Forms;
using ETG.Web.Tour.Models;
using System.Collections.Generic;

namespace ETG.Web.Tour.BookNow.Models
{
    public class BookNowViewModel
    {
        public BookNowViewModel()
        {
        }
        public TourViewModel Tour { get; set; }
        public bool HasFreedomOfChoice { get; set; }
        public bool HasNoRoomUpgrades { get; set; }

        public List<DayFreedomOfChoiceViewModel> BookingFreedomOfChoices { get; set; }

        public BookNowContentsViewModel BookNowContents
        {
            get;
            set;
        }

        public BookingAgentFormViewModel BookingForm { get; set; }
        public bool BookingCartIsComplete { get; set; }

        public string StepCompletedCssClass => BookingCartIsComplete ? " complete" : string.Empty;
        public BookNowStepsDetailsModel BookNowStepsDetails { get; set; }
        public int EntireFlexAmountPerPerson { get; set; }
        public int EntireFlexThresholdDays { get; set; }
        public int DaysFromDepartureDateForFullPayment { get; set; }
        public bool HideFlightsOtherOptions { get; set; }

    }
}
