using System;
using System.Linq;
using System.Threading;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;
using Unity.ServiceLocation;

namespace WpfApp1.Configuration
{
    public static class UnityConfiguration
    {
        private static readonly Lazy<IUnityContainer> containerFactory = new Lazy<IUnityContainer>(GetConfiguredContainer);

        /// <summary>
        /// Gets the configured container
        /// </summary>
        public static IUnityContainer Container => containerFactory.Value;

        private static IUnityContainer GetConfiguredContainer()
        {
            // configure singleton container
            var container = new UnityContainer();
            DesignTimeConfiguration(container);

            // configure service locator
            var unityServiceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            return container;
        }

        private static void DesignTimeConfiguration(IUnityContainer container)
        {
            // register services
            container
                .RegisterType<ApplicationContext>(new ContainerControlledLifetimeManager())
                .RegisterType<CancellationTokenSource>(new HierarchicalLifetimeManager(), new InjectionFactory(c => CancellationTokenSource.CreateLinkedTokenSource(c.Resolve<ApplicationContext>().CancellationTokenSource.Token)))
                .RegisterType<CancellationToken>(new InjectionFactory(c => c.Resolve<CancellationTokenSource>().Token))
                .RegisterType<IDependencyResolver, DependencyResolver>(new ContainerControlledLifetimeManager())
                .RegisterType<INumberServiceClient, NumberServiceClient>(new ContainerControlledLifetimeManager());

            // register all viewmodels as hierarchical
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(p => p.IsSubclassOf(typeof(ViewModelBase))),
                WithMappings.None,
                WithName.Default,
                WithLifetime.Hierarchical);
        }
    }
}