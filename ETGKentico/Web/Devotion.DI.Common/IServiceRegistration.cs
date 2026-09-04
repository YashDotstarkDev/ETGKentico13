using System;
using System.Collections.Generic;

namespace Devotion.DI.Common
{
    public interface IServiceRegistration
    {
        Dictionary<Type, Type> GetServiceMapping();
    }
}
