using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Core.Utility
{
    public interface IClock
    {
        DateTime Today { get; }
        DateTime Now { get; }
    }
}
