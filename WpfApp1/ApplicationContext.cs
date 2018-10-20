namespace WpfApp1
{
    using System;
    using System.Threading;
    using WpfApp1.Configuration;

    public class ApplicationContext
    {
        private static readonly Lazy<ApplicationContext> applicationContextFactory = new Lazy<ApplicationContext>(() => new ApplicationContext());

        private ApplicationContext()
        {
        }

        public void SetDependencyResolver(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
        }

        public static ApplicationContext Instance => applicationContextFactory.Value;

        public IDependencyResolver DependencyResolver { get; private set; }

        public IDependencyScope CurrentPageScope { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    }
}