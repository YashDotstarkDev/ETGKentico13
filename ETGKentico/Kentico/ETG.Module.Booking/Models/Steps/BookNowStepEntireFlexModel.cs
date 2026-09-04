using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepEntireFlexModel : BookNowStepModel, IDataModel
    {
        public bool AvailFlexOptions { get; set; }
        public bool DisableCheckbox { get; set; }
    }
}