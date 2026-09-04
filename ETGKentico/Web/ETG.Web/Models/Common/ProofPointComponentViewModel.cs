using System.Collections.Generic;

namespace ETG.Web.Models.Common
{
    public class ProofPointComponentViewModel : IViewModel
    {
        public string Title { get; set; }
        public List<CTAIconViewModel> ProofPoints { get; set; }
    }
}
