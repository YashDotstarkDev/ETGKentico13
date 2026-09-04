using ETG.Module.Booking.Models;
using System.Collections.Generic;
using ETG.Module.Booking.Models.Steps;

namespace ETG.WebAPI.Models.Booking.Responses
{

    public class SubmitBookingDateStepResponse : BaseResponse
    {
        public string SelectedDepartureDisplayDate { get; set; }
        public bool HasTwinShareOptions { get; set; }
        public bool HasSingleRoomOptions { get; set; }
        public bool HasSingleSupplementOption { get; set; }
        public List<TravellersCountOption> TwinShareOptions { get; set; }
        
        public List<TravellersCountOption> SingleRoomOptions { get; set; }

        public List<PrePostNightLabels> PreNights { get; set; }
        public List<PrePostNightLabels> PostNights { get; set; }
    }
}