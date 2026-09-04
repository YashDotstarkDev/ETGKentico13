using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepExtrasModel : BookNowStepModel, IDataModel
    {
        public bool? ExtrasSelectNow { get; set; }
        public List<RoomOption> ExtraOptions { get; set; }
        public bool DisableSelectNow { get; set; }
    }
}