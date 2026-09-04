using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Tour
{
    public interface IPriceInclusionRepository
    {
        List<IconSVGModel> GetAllPriceInclusionsIconSVG();
    }
}
