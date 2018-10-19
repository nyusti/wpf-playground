using System;
using System.Collections.Generic;

namespace WpfApp1.Configuration
{
    public interface IDependencyScope : IDisposable
    {
        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType);
    }
}