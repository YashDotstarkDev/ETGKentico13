namespace ETG.Web.Models.Competition
{
    public class PopupCompetitionViewModel : IViewModel
    {
        public string PopupTitle { get; set; }
        public string PopupSubTitle { get; set; }
        public bool ShowCompetitionPopup { get; set; }
        public string CompetitionCookieName { get; set; }
        public string CompetitionTermsUrl { get; set; }
        public string PopupThankYouMessage { get; set; }
    }
}
