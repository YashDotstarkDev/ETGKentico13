using Devotion.Automapper.Common;

namespace ETG.Data.Models.Forms
{
    public class ContactUsFormModel : IDataModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PreferredContactType { get; set; }

        public string Message { get; set; }
        public bool SubscribeToNewsletter { get; set; }
    }
}