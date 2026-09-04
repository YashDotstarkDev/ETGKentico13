using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Ninject;

namespace CMSApp.Extension
{
    public class BootstrapHelper
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
                                i.Name == typeof(Devotion.DI.Common.INinjectModuleBootstrapper).Name))
                    .ToList()
                    .ForEach(t =>
                    {
                        var ninjectModuleBootstrapper = (Devotion.DI.Common.INinjectModuleBootstrapper)Activator.CreateInstance(t);

                        kernel.Load(ninjectModuleBootstrapper.GetModules());
                    });
            }
            return kernel;
        }
    }
}