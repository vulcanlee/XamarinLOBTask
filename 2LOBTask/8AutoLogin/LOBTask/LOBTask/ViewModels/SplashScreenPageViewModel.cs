using LOBTask.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LOBTask.ViewModels
{

    public class SplashScreenPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // 顯示 App 初始化的處理進度文字
        public string Loading { get; set; }
        private readonly INavigationService _navigationService;

        public SplashScreenPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            //這裡的事件，將會於該頁面已經顯示在螢幕上之後，就會被呼救
            for (int i = 0; i < 6; i++)
            {
                //每 0.4 秒鐘，將會更換處理進度文字
                Loading = $"請稍後，系統初始化中 {i} ...";

                // 這裡不使用 Thread.Sleep，以避免螢幕被凍結
                //System.Threading.Thread.Sleep(400);

                //我們使用非同步的 Task.Delay(400)，暫停0.4秒，但不會影響螢幕凍結
                await Task.Delay(400);
            }

            try
            {
                var fooIt = 0;
                await MainHelper.UserLoginService.Read();
                if ((string.IsNullOrEmpty(MainHelper.UserLoginService.Item.Account) == true))
                {
                    //使用 Prism 導航服務物件，切換到別的頁面
                    //這裡使用絕對路徑導航，也就是說，導航堆疊的紀錄將會被清除
                    await _navigationService.NavigateAsync("xf:///UserLoginPage");
                }
                else
                {
                    await _navigationService.NavigateAsync("xf:///MDPage/NaviPage/HomePage");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

    }

}
