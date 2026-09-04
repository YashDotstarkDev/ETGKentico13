using Devotion.DI.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ETG.Web
{
    public static class BootstrapHelper
    {
        public static StandardKernel LoadNinjectKernel(StandardKernel kernel, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                assembly
                    .GetTypes()
                    .Where(t =>
                        t.GetInterfaces()
                            .Any(i =>
                                i.Name == typeof(INinjectModuleBootstrapper).Name))
                    .ToList()
                    .ForEach(t =>
                    {
                        var ninjectModuleBootstrapper = (INinjectModuleBootstrapper)Activator.CreateInstance(t);

                        kernel.Load(ninjectModuleBootstrapper.GetModules());
                    });
            }
            return kernel;
        }
    }
}