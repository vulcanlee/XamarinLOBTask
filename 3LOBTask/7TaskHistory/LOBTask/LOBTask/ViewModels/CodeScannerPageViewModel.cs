using LOBTask.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LOBTask.ViewModels
{
    public class CodeScannerPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAnalyzing { get; set; } = true;
        public bool IsScanning { get; set; } = true;
        public ZXing.Result ScanResult { get; set; }

        public DelegateCommand ScanResultCommand { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public CodeScannerPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            ScanResultCommand = new DelegateCommand(() =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;
                    //var fooPara = new NavigationParameters();
                    //fooPara.Add("Result", ScanResult);
                    //// 回到上頁，並且把掃描結果帶回去
                    //await _navigationService.GoBackAsync(fooPara);

                    _eventAggregator.GetEvent<ScanResultEvent>().Publish(new ScanResultPayload
                    {
                        Result = ScanResult.Text,
                    });
                    // 回到上一頁
                    await _navigationService.GoBackAsync();

                });
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

    }
}
