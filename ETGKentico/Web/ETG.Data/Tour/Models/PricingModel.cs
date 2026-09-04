using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class PricingModel : IDataModel
    {
        public string NodeAliasPath { get; set; }
        public string TourDate { get; set; }
        public string Currency { get; set; }
        public int OverridePrice { get; set; }
        public List<PricingItemModel> PricingList { get; set; }

        public int LowestPrice
        {
            get
            {
                if (OverridePrice > 0 || PricingList.IsNullOrEmpty())
                {
                    return OverridePrice;
                }

                return PricingList.Where(a=>a.Price > 0).Min(a => a.Price);

            }
        }
    }
}
