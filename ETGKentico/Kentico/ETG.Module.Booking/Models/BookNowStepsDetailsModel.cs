using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Castle.Core.Internal;
using Devotion.Automapper.Common;
using ETG.Module.Booking.Models.Steps;

namespace ETG.Module.Booking.Models
{
    public class BookNowStepsDetailsModel : IDataModel
    {
        public BookNowStepsDetailsModel()
        {
            StepDateSelect = new BookNowStepDateSelectModel
            {
                MainLabel = "Choose your Start Date"
            };
            StepTraveller = new BookNowStepTravellerModel
            {
                MainLabel = "Number of Travellers",
                TwinShareOptions = new List<TravellersCountOption>(),
                SingleRoomOptions = new List<TravellersCountOption>()
                
            };
            StepRoomOptions = new BookNowStepRoomOptionsModel
            {
                MainLabel = "Room Upgrades",
                RoomOptions = new List<RoomOptionDropdown>()
            };
            StepExtras = new BookNowStepExtrasModel { MainLabel = "Optional Extras" };
            StepPrePostNights = new BookNowStepPrePostNights { MainLabel = "Add Pre/Post Nights" };
            StepFreedomOfChoice = new BookNowStepFreedomOfChoiceModel();
            //StepEntireFlex = new BookNowStepEntireFlexModel();
            StepOtherOptions = new BookNowStepOtherOptionsModel();
        }
        
        public BookNowStepDateSelectModel StepDateSelect { get; set; }
        public BookNowStepTravellerModel StepTraveller { get; set; }
        public BookNowStepRoomOptionsModel StepRoomOptions { get; set; }
        public BookNowStepPrePostNights StepPrePostNights { get; set; }
        public BookNowStepExtrasModel StepExtras { get; set; }
        public BookNowStepFreedomOfChoiceModel StepFreedomOfChoice { get; set; }
        //public BookNowStepEntireFlexModel StepEntireFlex { get; set; }
        public BookNowStepOtherOptionsModel StepOtherOptions { get; set; }
        public bool EntireFlexIsOffered { get; set; }

    }
}