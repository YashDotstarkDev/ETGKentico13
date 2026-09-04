using ETG.Data.Models.Base;

namespace ETG.Data.Career.Models
{
    public class CareerRoleModel : PageNodeModel
    {
        public CareerRoleBasicInfoModel BasicInfo { get; set; }
        public string Description { get; set; }
        public string SummaryOfPosition { get; set; }
        public string DutiesAndResponsibilities { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
    }
}
