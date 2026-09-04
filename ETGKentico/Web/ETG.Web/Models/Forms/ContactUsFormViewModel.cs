using System.Text.RegularExpressions;
using Castle.Core.Internal;
using ETG.Web.Models.Base;

namespace ETG.Web.Models.Forms
{
    public class ContactUsFormViewModel : BaseFormViewModel, IViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PreferredContactType { get; set; }
        public string Message { get; set; }
        public bool SubscribeToNewsletter { get; set; }
        public string Friend { get; set; }

        public bool Validate()
        {
            // Honeypot
            if (!string.IsNullOrWhiteSpace(Friend))
            {
                ErrorMessage = "Please enter valid values.";
                return false;
            }
            
            if (FirstName.IsNullOrEmpty() || LastName.IsNullOrEmpty() || (PreferredContactType == "Email" && Email.IsNullOrEmpty())
                || (PreferredContactType == "Phone" && Phone.IsNullOrEmpty()))
            {
                ErrorMessage = "Please enter values to mandatory fields.";
                return false;
            }

            if (FirstName == LastName)
            {
                ErrorMessage = "First and last name cannot be the same";
                return false;
            }

            Regex regex = new Regex("^[0-9]+$");
            if (FirstName.Contains("http") || LastName.Contains("http") ||
                (!Phone.IsNullOrEmpty() && !regex.Match(Phone.Trim()).Success))
            {
                ErrorMessage = "Please enter valid values.";
                return false;
            }

            return true;

        }
            
    }
}