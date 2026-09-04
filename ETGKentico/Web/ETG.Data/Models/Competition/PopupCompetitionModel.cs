using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Competition
{
    public class PopupCompetitionModel : IDataModel
    {
        public string PopupTitle { get; set; }
        public string PopupSubTitle { get; set; }
        public bool ShowCompetitionPopup { get; set; }
        public string CompetitionCookieName { get; set; }
        public string CompetitionTermsUrl { get; set; }
        public string PopupThankYouMessage { get; set; }
    }
}
