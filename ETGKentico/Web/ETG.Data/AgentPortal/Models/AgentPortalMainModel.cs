using ETG.Data.Models.Base;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentPortalMainModel : PageNodeModel
    {
        public string MainHeading { get; set; }
        public string MainDescription { get; set; }
        
        public string ToolkitHeading { get; set; }
        public string ToolkitDescription { get; set; }
        
        public string IncentiveHeading { get; set; }
        public string IncentiveDescription { get; set; }
        
        public string WebinarHeading { get; set; }
        public string WebinarDescription { get; set; }
        
        public string BookingHeading { get; set; }
        public string BookingDescription { get; set; }
        
        public string BookingOptionsHeading { get; set; }
        public string BookingOptionsDescription { get; set; }
        
        public string AgentInstructionHeading { get; set; }
        public string AgentInstructionDescription { get; set; }
        
        public string PaymentsHeading { get; set; }
        public string PaymentsDescription { get; set; }
    }
}
