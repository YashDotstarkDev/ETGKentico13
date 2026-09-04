using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Web.Models.Competition;

namespace ETG.Web.Models.Forms
{
    public class NewsletterSubscriptionViewModel
    {
        public PopupCompetitionViewModel CompetitionForm { get; set; }
        public string Email { get; set; }
        public bool IsAgent { get; set; }
    }
}