using System;
using Microsoft.Extensions.DependencyInjection;
using Unity;
using Unity.Lifetime;

namespace WpfApp1.DependencyInjection
{
    public class ServiceProvider : IServiceProvider, IServiceScopeFactory, IServiceScope, IDisposable

    {
        private readonly IUnityContainer container;

        internal ServiceProvider(IUnityContainer container)
        {
            this.container = container;
            this.container.RegisterInstance((IServiceScope)this, new ExternallyControlledLifetimeManager());
            this.container.RegisterInstance((IServiceProvider)this, new ExternallyControlledLifetimeManager());
            this.container.RegisterInstance((IServiceScopeFactory)this, new ExternallyControlledLifetimeManager());
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch { /* Ignore */}

            return null;
        }

        public IServiceScope CreateScope()

        {
            return new ServiceProvider(container.CreateChildContainer());
        }

        IServiceProvider IServiceScope.ServiceProvider => this;

        public static IServiceProvider ConfigureServices(IServiceCollection services)

        {
            return new ServiceProvider(new UnityContainer().AddServices(services));
        }

        public static explicit operator UnityContainer(ServiceProvider c)

        {
            return (UnityContainer)c.container;
        }

        #region Disposable

        public void Dispose()

        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool _)
        {
            this.container?.Dispose();
        }
    }
}