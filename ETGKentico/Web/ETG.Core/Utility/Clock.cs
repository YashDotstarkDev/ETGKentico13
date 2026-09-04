using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Core.Utility
{
    public class Clock : IClock
    {
        public DateTime Today => DateTime.UtcNow.Date;
        public DateTime Now => DateTime.UtcNow;
    }
}
