﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Threading.Tasks;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApp1.ViewModel;

namespace WpfApp1.Components
{
    public class FirstDataInputViewModel : ComponentViewModel
    {
        private readonly INumberServiceClient numberServiceClient;
        private readonly NavigationService navigationService;

        public FirstDataInputViewModel(INumberServiceClient numberServiceClient, NavigationService navigationService)
        {
            this.numberServiceClient = numberServiceClient ?? throw new ArgumentNullException(nameof(numberServiceClient));
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnInit()
        {
            this.numberServiceClient.GetNumberAsync(this.CancellationToken)
               .ToObservable()
               .Subscribe(r => this.DataList = new ObservableCollection<int>(r), this.CancellationToken);
        }

        private ObservableCollection<int> dataList;

        public ObservableCollection<int> DataList
        {
            get => this.dataList;
            set
            {
                this.Set(() => this.DataList, ref this.dataList, value);
                this.RaisePropertyChanged(() => this.DataListLoaded);
            }
        }

        public bool DataListLoaded => this.DataList?.Count > 0;

        private RelayCommand goToSecondPage;

        public RelayCommand GoToSecondPage => this.goToSecondPage ?? (this.goToSecondPage = new RelayCommand(() =>
        {
            this.navigationService.Navigate(new Uri("Components/SecondPage.xaml", UriKind.RelativeOrAbsolute));
        }));
    }
}