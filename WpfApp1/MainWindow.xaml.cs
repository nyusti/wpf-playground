using System.Windows.Navigation;
using GalaSoft.MvvmLight.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            DispatcherHelper.Initialize();
            this.Loaded += MainWindow_Loaded;
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(ServiceLocator.Current.GetInstance<FirstDataInput>());
            this.NavigationService.Navigate(new System.Uri("Components/FirstDataInput.xaml", System.UriKind.RelativeOrAbsolute));
        }
    }
}