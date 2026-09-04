using Castle.Core.Internal;
using ETG.WebAPI.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.GoogleReviews
{
    public class GoogleReviewsViewModel
    {
        public ReviewListingResponse ReviewListingResponse { get; set; }

        public string GetDateText(DateTime dt)
        {
            TimeSpan timeDifference = DateTime.Now - dt;

            if (timeDifference.TotalSeconds < 60)
                return "Just now";
            if (timeDifference.TotalMinutes < 60)
                return $"{(int)timeDifference.TotalMinutes} minutes ago";
            if (timeDifference.TotalHours < 24)
                return $"{(int)timeDifference.TotalHours} hours ago";
            if (timeDifference.TotalDays < 7)
                return $"{(int)timeDifference.TotalDays} days ago";
            if (timeDifference.TotalDays < 30)
                return $"{(int)(timeDifference.TotalDays / 7)} weeks ago";
            if (timeDifference.TotalDays < 365)
                return $"{(int)(timeDifference.TotalDays / 30)} months ago";

            return $"{(int)(timeDifference.TotalDays / 365)} years ago";

        }
    }
}