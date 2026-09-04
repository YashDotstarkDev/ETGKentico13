using System.Collections.Generic;
using Ninject.Modules;

namespace Devotion.DI.Common
{
    public interface INinjectModuleBootstrapper
    {
        IList<INinjectModule> GetModules();
    }
}
