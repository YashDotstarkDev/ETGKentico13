using Devotion.Automapper.Common;

namespace ETG.Data.Models.Forms
{
    public class GenericEnquiryFormModel : IDataModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PreferredContactType { get; set; }
        public string PreferredTourType { get; set; }
        public string PreferredDestination { get; set; }
        public string PreferredExperience { get; set; }

        public string Message { get; set; }
        public bool SubscribeToNewsletter { get; set; }
    }
}