using System.Collections.Generic;
using ETG.Core.Constants;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Tour
{
    public class PriceInclusionRepository : IPriceInclusionRepository
    {
        
        public List<IconSVGModel> GetAllPriceInclusionsIconSVG()
        {
            
            return new List<IconSVGModel>
            {
                new IconSVGModel{Name = "Flights", IconClass = "fal fa-plane", SVGCode = SVGConstants.SVG_FLIGHTS},
                new IconSVGModel{Name = "Accommodation", IconClass = "fal fa-bed", SVGCode =  SVGConstants.SVG_ACCOMMODATION},
                new IconSVGModel{Name = "Meals", IconClass = "fal fa-utensils", SVGCode = SVGConstants.SVG_MEALS},
                new IconSVGModel{Name = "Transfer", IconClass = "fal fa-bus", SVGCode = SVGConstants.SVG_TRANSFER},
            };
        }
    }
}