using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Experience.Models
{
    public class ExperienceSummaryModel : IDataModel
    {
        public Guid NodeGuid { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public string ForegroundImage { get; set; }
        public bool DisableOverlay { get; set; }
        public string HeroAltText { get; set; }
        public string Path { get; set; }

        public string IconDarkImage { get; set; }
        public string IconWhiteImage { get; set; }
        public string IconClass { get; set; }
        public string SVGIcon { get; set; }
    }
}
