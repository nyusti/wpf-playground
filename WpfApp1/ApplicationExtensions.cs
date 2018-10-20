namespace WpfApp1
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Navigation;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using Unity;
    using Unity.Injection;
    using Unity.Lifetime;
    using Unity.RegistrationByConvention;
    using WpfApp1.Configuration;

    public static class ApplicationExtensions
    {
        public static Application UseUnity(this Application app, IUnityContainer unityContainer)
        {
            // create a cancellation token scoped for the application
            var mainCancellationTokenSource = ApplicationContext.Instance.CancellationTokenSource;
            // cancel everything when the application shuts down
            Observable.FromEventPattern<ExitEventArgs>(app, nameof(app.Exit))
                .Select(p => p.EventArgs)
                .Subscribe(p =>
                {
                    if (!mainCancellationTokenSource.IsCancellationRequested)
                    {
                        mainCancellationTokenSource.Cancel();
                    }
                });

            unityContainer
                .RegisterInstance(typeof(Application), app, new ExternallyControlledLifetimeManager())
                .RegisterInstance(typeof(ApplicationContext), ApplicationContext.Instance)
                .RegisterInstance(ApplicationContext.Instance.DependencyResolver, new ContainerControlledLifetimeManager())
                // create a linked token source for each view model
                .RegisterType<CancellationTokenSource>(new HierarchicalLifetimeManager(), new InjectionFactory(c => CancellationTokenSource.CreateLinkedTokenSource(mainCancellationTokenSource.Token)))
                .RegisterType<CancellationToken>(new InjectionFactory(c => c.Resolve<CancellationTokenSource>().Token))
                // register all view models as hierarchical
                .RegisterTypes(
                    AllClasses.FromLoadedAssemblies().Where(p => p.IsSubclassOf(typeof(ViewModelBase))),
                    WithMappings.None,
                    WithName.Default,
                    WithLifetime.Hierarchical);

            return app;
        }

        public static Application UseLogging(this Application app)
        {
            Logging.LoggingService.EnableLogging();
            return app;
        }

        public static Application UseNavigationWindow<TWindow>(this Application app)
            where TWindow : NavigationWindow
        {
            var mainWindow = ApplicationContext.Instance.DependencyResolver.Resolve<TWindow>();
            var navigationService = mainWindow.NavigationService;

            UnityConfiguration.Container.RegisterInstance<INavigationService>(new NavigationServiceWrapper(navigationService));

            var onLoaded = Observable.FromEventPattern<NavigationEventArgs>(
                    navigationService,
                    nameof(navigationService.LoadCompleted))
                .Select(p => p.EventArgs);

            onLoaded.Subscribe(args =>
            {
                var currentScope = ApplicationContext.Instance.CurrentPageScope;
                if (currentScope != null)
                {
                    currentScope.Resolve<CancellationTokenSource>().Cancel();
                    currentScope.Dispose();
                }
            });

            onLoaded.Subscribe(args =>
            {
                var currentScope = ApplicationContext.Instance.DependencyResolver.BeginScope();
                ApplicationContext.Instance.CurrentPageScope = currentScope;
                currentScope.BuildUp(args.Content.GetType(), args.Content);
            });

            app.MainWindow = mainWindow;
            app.MainWindow.ShowActivated = true;

            app.MainWindow.Show();

            return app;
        }
    }
}