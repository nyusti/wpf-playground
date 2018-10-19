using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Unity;

namespace WpfApp1.Configuration
{
    public class Bootstrapper
    {
        private NavigationServiceWrapper navigationServiceWrapper;

        public void Run(Application application)
        {
            if (application == null)
            {
                throw new System.ArgumentNullException(nameof(application));
            }

            Logging.LoggingService.EnableLogging();

            var container = UnityConfiguration.Container;

            var mainWindow = container.Resolve<MainWindow>();

            container.RegisterInstance(mainWindow.NavigationService);
            this.SubscribeToNavigation();

            application.MainWindow = mainWindow;
            application.MainWindow.ShowActivated = true;
            application.MainWindow.Show();
        }

        private void SubscribeToNavigation()
        {
            var container = UnityConfiguration.Container;
            this.navigationServiceWrapper = container.Resolve<NavigationServiceWrapper>();
        }
    }

    public class NavigationServiceWrapper
    {
        private readonly NavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;

        public NavigationServiceWrapper(NavigationService navigationService, IDependencyResolver dependencyResolver)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));

            this.OnLoaded.Subscribe(arg =>
            {
                var applicationContext = this.dependencyResolver.Resolve<ApplicationContext>();
                if (applicationContext.CurrentPageScope != null)
                {
                    var currentTokenSource = applicationContext.CurrentPageScope.Resolve<CancellationTokenSource>();
                    currentTokenSource.Cancel();

                    applicationContext.CurrentPageScope.Dispose();
                }
            });

            this.OnLoaded.Subscribe(args =>
            {
                var applicationContext = this.dependencyResolver.Resolve<ApplicationContext>();
                var pageScope = this.dependencyResolver.BeginScope();
                applicationContext.CurrentPageScope = pageScope;

                var container = pageScope.Resolve<IUnityContainer>();
                container.BuildUp(args.Content.GetType(), args.Content);
            });
        }

        private IObservable<NavigationEventArgs> OnLoaded
        {
            get
            {
                return Observable
                    .FromEventPattern<LoadCompletedEventHandler, NavigationEventArgs>(
                        h => this.navigationService.LoadCompleted += h,
                        h => this.navigationService.LoadCompleted -= h)
                    .Select(x => x.EventArgs);
            }
        }
    }
}