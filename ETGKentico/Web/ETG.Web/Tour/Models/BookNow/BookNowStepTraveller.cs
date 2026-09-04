using ETG.Web.Tour.BookNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Tour.Models.BookNow
{
    public class BookNowStepTraveller : BookNowStep
    {
        public BookNowStepTraveller(BookNowViewModel viewModel) : base(viewModel)
        {

        }

        public override string MainLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? "Number of travellers"
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.StepTravellersLabel;
            }
        }

        public override string SubLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? string.Empty
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.StepTravellersSubLabel;
            }
        }
    }
}