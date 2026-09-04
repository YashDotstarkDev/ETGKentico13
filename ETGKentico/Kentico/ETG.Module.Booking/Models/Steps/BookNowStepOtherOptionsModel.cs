using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepOtherOptionsModel : BookNowStepModel, IDataModel
    {
        public bool? RequireFareAssistance { get; set; }
        public bool? RequireTravelInsuranceAssistance { get; set; }
    }
}