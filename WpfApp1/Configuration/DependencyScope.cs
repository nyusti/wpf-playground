using System;
using System.Collections.Generic;
using Unity;

namespace WpfApp1.Configuration
{
    internal sealed class DependencyScope : IDependencyScope
    {
        private readonly IUnityContainer unityContainer;

        public DependencyScope(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
            {
                throw new ArgumentNullException(nameof(unityContainer));
            }

            this.unityContainer = unityContainer.CreateChildContainer();
        }

        public object GetService(Type serviceType)
        {
            return this.unityContainer.Resolve(serviceType);
        }

        public void Dispose()
        {
            this.unityContainer.Dispose();
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

    public static class DependencyScopeExtensions
    {
        public static T Resolve<T>(this IDependencyScope dependencyScope)
        {
            return (T)dependencyScope.GetService(typeof(T));
        }

        public static T BuildUp<T>(this IDependencyScope dependencyScope, T existing)
        {
            return (T)dependencyScope.BuildUp(typeof(T), existing);
        }
    }
}