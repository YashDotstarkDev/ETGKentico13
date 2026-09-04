using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Destination.Models
{
    public class DestinationStickyModel
    {
        public bool DisplaySticky { get; set; }
        public string CampaignDetail1 { get; set; }
        public string CampaignDetail2 { get; set; }
        public string CampaignDetail3 { get; set; }
        public string CountdownLabel { get; set; }
        public string EndOfCampaign { get; set; }
    }
}
