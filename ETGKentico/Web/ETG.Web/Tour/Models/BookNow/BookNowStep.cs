using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Web.Tour.BookNow.Models;

namespace ETG.Web.Tour.Models.BookNow
{
    public class BookNowStep
    {
        protected BookNowViewModel _viewModel;
        public BookNowStep(BookNowViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public virtual string MainLabel { get; set; }
        public virtual string SubLabel { get; set; }
    }
}