using System;
using ETG.Web.Models;

namespace ETG.Web.Experience.Models
{
    public class ExperienceSummaryViewModel : IViewModel
    {
        public Guid NodeGuid { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }

        public string HeadingIconImage { get; set; }
        public string IconClass { get; set; }
        public string SVGIcon { get; set; }
    }
}
