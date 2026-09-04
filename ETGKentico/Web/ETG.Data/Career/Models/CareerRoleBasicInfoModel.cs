using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Career.Models
{
    public class CareerRoleBasicInfoModel : IDataModel
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string PositionType { get; set; }
        public string Path { get; set; }
        public DateTime DateModified { get; set; }
        public bool DocumentSearchExcluded { get; set; }
    }
}
