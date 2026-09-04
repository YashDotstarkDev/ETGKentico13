using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class ContactModel : IDataModel
    {
        public string OpeningHours { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string HeaderPhoneAu { get; set; }
        public string HeaderPhoneNz { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string YouTubeUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string DisplayAddress { get; set; }
        public string DestinationExpertInstruction { get; set; }
        public string PreferPhoneBackgroundImage { get; set; }
        public string PreferPhoneDescription { get; set; }
        public string PreferPhoneOpeningHoursText { get; set; }
        public string PeaceOfMindUrl { get; set; }
        public string SafeTravelUrl { get; set; }
        public string FreedomOfChoiceUrl { get; set; }
        public string BookNowUrl { get; set; }
        public string FAQsUrl { get; set; }
        public string SafeTravelDescription { get; set; }
        public string ExclusiveProductDescription { get; set; }
        public string FreedomOfChoiceDescription { get; set; }
        public string PeachOfMindCheckList { get; set; }
    }
}
