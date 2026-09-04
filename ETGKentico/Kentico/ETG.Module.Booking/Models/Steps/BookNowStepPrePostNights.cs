using System.Collections.Generic;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepPrePostNights : BookNowStepModel
    {
        public List<PrePostNightLabels> PreNightsOptions { get; set; }
        public List<PrePostNightLabels> PostNightsOptions { get; set; }
    }
}