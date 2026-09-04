using System.Collections.Generic;
using Castle.Core.Internal;
using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepTravellerModel : BookNowStepModel, IDataModel
    {
        public string ConcatenatedRoomsType
        {
            get
            {
                if (SelectedTwinShareRoomsType.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                return string.Join(",", SelectedTwinShareRoomsType);
            }
        }
        public List<TravellersCountOption> TwinShareOptions { get; set; }
        public List<string> SelectedTwinShareRoomsType { get; set; }
        public List<TravellersCountOption> SingleRoomOptions { get; set; }
        public bool HideSingleRoomDropdown { get; set; }
    }
}