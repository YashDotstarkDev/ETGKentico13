using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Settings.Models;

namespace ETG.Data.Settings
{
    public interface IETGSettingsService
    {
        ETGSettings GetSettings();
    }
}
