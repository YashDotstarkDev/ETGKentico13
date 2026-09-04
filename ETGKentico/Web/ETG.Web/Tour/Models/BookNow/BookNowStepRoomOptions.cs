using ETG.Web.Tour.BookNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Tour.Models.BookNow
{
    public class BookNowStepRoomOptions : BookNowStep
    {
        public BookNowStepRoomOptions(BookNowViewModel viewModel) : base(viewModel)
        {
        
        }
        public override string MainLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? "Room Options"
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.RoomOptionsLabel;
            }
        }

        public override string SubLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? string.Empty
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.RoomOptionsSubLabel;
            }
        }
    }
}