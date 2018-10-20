using System;
using Unity;
using Unity.Lifetime;

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

            return container;
        }

        private static void DesignTimeConfiguration(IUnityContainer container)
        {
            // register services
            container
                .RegisterType<INumberServiceClient, NumberServiceClient>(new ContainerControlledLifetimeManager());
        }
    }
}