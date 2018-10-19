using System;
using System.Collections.Generic;
using Unity;

namespace WpfApp1.Configuration
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer unityContainer;

        public DependencyResolver(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.unityContainer.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyScope(this.unityContainer);
        }

        public object GetService(Type serviceType)
        {
            return this.unityContainer.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.unityContainer.ResolveAll(serviceType);
        }
    }
}