using System.Windows;
using WpfApp1.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ApplicationContext.Instance.SetDependencyResolver(new DependencyResolver(UnityConfiguration.Container));

            var application = this;

            application
                .UseLogging()
                .UseUnity(UnityConfiguration.Container)
                .UseNavigationWindow<MainWindow>();
        }
    }
}