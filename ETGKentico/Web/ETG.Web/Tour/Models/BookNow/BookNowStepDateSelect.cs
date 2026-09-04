using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Web.Tour.BookNow.Models;

namespace ETG.Web.Tour.Models.BookNow
{
    public class BookNowStepDateSelect : BookNowStep
    {
        public BookNowStepDateSelect(BookNowViewModel viewModel) : base(viewModel)
        {
            
        }

        public override string MainLabel { get
        {
            return !_viewModel.BookingCartIsComplete
                ? "Start Date"
                : _viewModel.BookNowOptions?.StepCompletedLabels?.StepDateLabel;
        } }
    }
}