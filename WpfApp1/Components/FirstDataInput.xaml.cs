using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Unity.Attributes;

namespace WpfApp1.Components
{
    /// <summary>
    /// Interaction logic for FirstDataInput.xaml
    /// </summary>
    public partial class FirstDataInput : Page
    {
        public FirstDataInput()
        {
            InitializeComponent();
        }

        [Dependency]
        public FirstDataInputViewModel ViewModel
        {
            get => this.DataContext as FirstDataInputViewModel;
            set => this.DataContext = value;
        }

        public IObservable<NavigationEventArgs> OnNavigated
        {
            get
            {
                return Observable
                    .FromEventPattern<NavigatedEventHandler, NavigationEventArgs>(
                        h => this.NavigationService.Navigated += h,
                        h => this.NavigationService.Navigated -= h)
                    .Select(x => x.EventArgs);
            }
        }

        ~FirstDataInput()
        {
            Debug.WriteLine("Page 1 finalized");
        }
    }
}