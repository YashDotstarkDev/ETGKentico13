using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Booking
{
    public class PaymentOptionModel  : IDataModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
