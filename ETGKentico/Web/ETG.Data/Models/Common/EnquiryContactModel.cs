using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class EnquiryContactModel : IDataModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneDigits { get; set; }
    }
}
