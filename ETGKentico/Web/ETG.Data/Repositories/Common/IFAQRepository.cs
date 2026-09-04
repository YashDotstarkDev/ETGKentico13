using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public interface IFAQRepository
    {
        List<FAQModel> GetFAQs(string path, int? count = null);
    }
}
