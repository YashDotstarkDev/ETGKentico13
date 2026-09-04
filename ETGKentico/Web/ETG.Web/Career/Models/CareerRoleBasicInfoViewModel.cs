using ETG.Web.Models;

namespace ETG.Web.Career.Models
{
    public class CareerRoleBasicInfoViewModel : IViewModel
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string PositionType { get; set; }
        public string Path { get; set; }
    }
}
