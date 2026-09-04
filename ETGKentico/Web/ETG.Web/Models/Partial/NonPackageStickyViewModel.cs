using ETG.WebAPI.Models.Reviews;
using System.Text.RegularExpressions;

namespace ETG.Web.Models.Partial
{
    public class NonPackageStickyViewModel
    {
        public string CurrentCountryCode { get; set; }
        public string PhoneNumber => Regex.Replace(PhoneNumberDisplay, @"\D", "");
        public string PhoneNumberDisplay { get; set; }
        public string PhoneHRef
        {
            get
            {
                if (CurrentCountryCode == null)
                {
                    return PhoneNumber;
                }

                return $"{Regex.Replace(CurrentCountryCode, @"\D", "")}{PhoneNumber}";
            }
        }

        public string EnquiryUrl { get; set; }
        public ReviewListingResponse GoogleReview { get; set; }
    }
}