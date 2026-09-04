using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Modules
{
    public class TourTypeModel : IDataModel
    {
        public Guid ItemGuid { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string IconClass { get; set; }
        public string IconWhiteImage { get; set; }
        public string IconBlackImage { get; set; }
        public string TourTypeDescription { get; set; }
        public string Url { get; set; }
    }
}
