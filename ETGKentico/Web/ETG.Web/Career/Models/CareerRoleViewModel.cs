using ETG.Web.Models.Base;

namespace ETG.Web.Career.Models
{
    public class CareerRoleViewModel : PageNodeViewModel
    {
        public CareerRoleBasicInfoViewModel BasicInfo { get; set; }
        public string Description { get; set; }
        public string SummaryOfPosition { get; set; }
        public string DutiesAndResponsibilities { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
    }
}
