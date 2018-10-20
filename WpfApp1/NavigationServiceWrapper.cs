using System;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Views;

namespace WpfApp1
{
    public class NavigationServiceWrapper : INavigationService
    {
        private readonly NavigationService navigationService;

        public NavigationServiceWrapper(NavigationService navigationService)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public string CurrentPageKey => this.navigationService.CurrentSource?.ToString();

        public void GoBack()
        {
            if (this.navigationService.CanGoBack)
            {
                this.navigationService.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            this.navigationService.Navigate(new Uri(pageKey, UriKind.RelativeOrAbsolute));
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            this.navigationService.Navigate(new Uri(pageKey, UriKind.RelativeOrAbsolute), parameter);
        }
    }
}