using System.Text.RegularExpressions;
using Castle.Core.Internal;
using ETG.Web.Models;
using ETG.Web.Models.Base;

namespace ETG.Web.Tour.Models
{
    public class TourEnquireFormViewModel : BaseFormViewModel, IViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PreferredContactType { get; set; }

        public string Message { get; set; }
        public bool SubscribeToNewsletter { get; set; }
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureClass { get; set; }
        public string DepartureCity { get; set; }
        public int QuotedPrice { get; set; }
        public string PriceCurrency { get; set; }
        public string DiscountText { get; set; }

        public bool Validate()
        {

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
            if (FirstName.Contains("http") || LastName.Contains("http"))
            {
                ErrorMessage = "Please enter valid values.";
                return false;
            }

            return true;
        }
    }
}