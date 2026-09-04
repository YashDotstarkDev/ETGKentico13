using Devotion.Automapper.Common;
using System.Collections.Generic;

namespace ETG.Data.Brochure.Models
{
    public class BrochureOrderFormModel: IDataModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public List<BrochureModel> BrochureOrders { get; set; }
        public string BrochureGuids { get; set; }
    }
}
