namespace ETG.Web.Models.Common
{
    public class EnquiryContactViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneDigits { get; set; }
    }
}