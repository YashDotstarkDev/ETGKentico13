using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Forms;
using ETG.Web.Models.Base;
using ETG.Web.Models.Newsletter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace ETG.Web.Models.Forms
{
    public class UnsubscribeFromViewModel : BaseFormViewModel, IViewModel
    {
        public string Email { get; set; }
        public Guid NewsletterGuid { get; set; }
        public Guid IssueGuid { get; set; }
        public string Hash { get; set; }
        public bool UnsubscribeFromAll { get; set; }

        public int ReasonId { get; set; }
        public List<SelectListItem> Reasons { get; set; }
    }
}