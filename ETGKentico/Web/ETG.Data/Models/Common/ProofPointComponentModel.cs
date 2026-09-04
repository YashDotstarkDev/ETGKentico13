using Devotion.Automapper.Common;
using System.Collections.Generic;

namespace ETG.Data.Models.Common
{
    public class ProofPointComponentModel : IDataModel
    {
        public string Title { get; set; }
        public List<CTAIconModel> ProofPoints { get; set; }
    }
}
