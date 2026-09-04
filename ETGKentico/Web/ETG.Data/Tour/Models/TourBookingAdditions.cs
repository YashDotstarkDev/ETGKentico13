using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.PageTypes;

namespace ETG.Data.Tour.Models
{
    public class TourBookingAdditions
    {
        public string tourCode { get; set; }
        public string tourNodeAliasPath { get; set; }
        public List<RoomUpgrade> RoomUpgrades { get; set; }
        public List<OptionalExtras> OptionalExtras { get; set; }
    }
}
