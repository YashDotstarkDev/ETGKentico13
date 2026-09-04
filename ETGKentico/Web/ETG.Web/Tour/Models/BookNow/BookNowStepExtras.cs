using ETG.Web.Tour.BookNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Tour.Models.BookNow
{
    public class BookNowStepExtras : BookNowStep
    {
        public BookNowStepExtras(BookNowViewModel viewModel) : base(viewModel)
        {

        }
        public override string MainLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? "Optional Extras"
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.ExtrasOptionsLabel;
            }
        }

        public override string SubLabel
        {
            get
            {
                return !_viewModel.BookingCartIsComplete
                    ? string.Empty
                    : _viewModel.BookNowOptions?.StepCompletedLabels?.ExtrasOptionsSubLabel;
            }
        }
    }
}