using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Settings
{
    public interface ISettingsRepository
    {
        Dictionary<string, string> GetETGSettings();
    }
}
