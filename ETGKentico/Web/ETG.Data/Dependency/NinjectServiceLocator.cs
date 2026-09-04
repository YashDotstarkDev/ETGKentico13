using Ninject;
using System;
using System.Collections.Generic;
using CommonServiceLocator;

namespace ETG.Data.Dependency
{
    public class NinjectServiceLocator : ServiceLocatorImplBase
    {
        private readonly IKernel kernel;

        public NinjectServiceLocator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return ResolutionExtensions.Get(kernel, serviceType, key);
            }

            return ResolutionExtensions.Get(kernel, serviceType, (string)null);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ResolutionExtensions.GetAll(kernel, serviceType);
        }
    }
}
