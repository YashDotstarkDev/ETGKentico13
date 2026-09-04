using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.Forms;
using ETG.Data.Brochure.Models;

namespace ETG.Data.Brochure.Services
{
    public interface IBrochureService
    {
        List<BrochureModel> GetAllBrochures();
        BrochureModel GetBrochure(Guid guid);
        BrochureModel GetBrochure(string alias);
        IEnumerable<BrochureModel> GetBrochures(List<Guid> guids);
        BrochureModel GetBrochureByDestinationGuid(Guid guid);
        void SendOrderResponse(string firstName, string email, string brochureGuids);
    }
}
