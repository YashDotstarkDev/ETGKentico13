using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devotion.Automapper.Common;
using ETG.Web.Models;

namespace ETG.Web.Models.Booking
{
    public class PaymentOptionViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
