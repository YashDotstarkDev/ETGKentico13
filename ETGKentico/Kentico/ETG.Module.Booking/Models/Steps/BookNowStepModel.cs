using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepModel: IDataModel
    {

        public string MainLabel { get; set; }
        public string SubLabel { get; set; }
        public bool IsHidden { get; set; }
    }
}