using System;
using System.Collections.Generic;
using Unity;
using Unity.Exceptions;

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
            try
            {
                return this.unityContainer.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                // TODO: log
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.unityContainer.ResolveAll(serviceType);
        }

        public object BuildUp(Type serviceType, object existing)
        {
            return this.unityContainer.BuildUp(serviceType, existing);
        }
    }
}