using LOBTask.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOBTask.ViewModels
{
    public class MDPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string UserPhoto { get; set; } 

        public string UserName { get; set; }

        public bool 管理者模式 { get; set; }

        public DrawerMenuVM DrawerMenuVM代處理工作 { get; set; } = new DrawerMenuVM();
        public DrawerMenuVM DrawerMenuVM歷史工作 { get; set; } = new DrawerMenuVM();
        public DrawerMenuVM DrawerMenuVM模擬可掃描QRCode { get; set; } = new DrawerMenuVM();
        public DrawerMenuVM DrawerMenuVM更新App { get; set; } = new DrawerMenuVM();
        public DrawerMenuVM DrawerMenuVM登出 { get; set; } = new DrawerMenuVM();

        public DelegateCommand 尚未完成派工單Command { get; set; }
        public DelegateCommand 歷史派工單Command { get; set; }
        public DelegateCommand 管理者模式命令Command { get; set; }
        public DelegateCommand 模擬可掃描的QRCodeCommand { get; set; }
        public DelegateCommand 更新AppCommand { get; set; }
        public DelegateCommand 登出Command { get; set; }

        #region 需要透過建構式注入方式，取得 Prism 提供的三個服務實作物件
        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        public MDPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
            IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            #region 選單項目初始化
            DrawerMenuVM代處理工作.MenuName = "待處理工作";
            DrawerMenuVM代處理工作.IconName = "fa-tasks";
            DrawerMenuVM代處理工作.DrawMenuCommand = new DelegateCommand(async () =>
            {
                // 這裡使用絕對 URI 的導航路徑
                await _navigationService.NavigateAsync("xf:///MDPage/NaviPage/HomePage");
            });

            DrawerMenuVM歷史工作.MenuName = "歷史工作";
            DrawerMenuVM歷史工作.IconName = "fa-history";
            DrawerMenuVM歷史工作.DrawMenuCommand = new DelegateCommand(async () =>
            {
                // 這裡使用絕對 URI 的導航路徑
                await _navigationService.NavigateAsync("xf:///MDPage/NaviPage/TaskHistoryPage");
            });

            DrawerMenuVM模擬可掃描QRCode.MenuName = "QR Code網頁";
            DrawerMenuVM模擬可掃描QRCode.IconName = "fa-qrcode";
            DrawerMenuVM模擬可掃描QRCode.DrawMenuCommand = new DelegateCommand(() =>
            {
                //顯示網頁，裡面有每個工作打卡會用到的 QRCode 圖片
                Device.OpenUri(new Uri($"http://xamarinlobtask.azurewebsites.net/DoTasks?account={MainHelper.UserLoginService.Item.Account}"));
            });

            DrawerMenuVM更新App.MenuName = "更新App";
            DrawerMenuVM更新App.IconName = "fa-plus-square";
            DrawerMenuVM更新App.DrawMenuCommand = new DelegateCommand(() =>
            {
                //顯示網頁，裡面有每個工作打卡會用到的 QRCode 圖片
                Device.OpenUri(new Uri($"http://bit.ly/2nUjgUq"));
            });

            DrawerMenuVM登出.MenuName = "登出";
            DrawerMenuVM登出.IconName = "fa-sign-out";
            DrawerMenuVM登出.DrawMenuCommand = new DelegateCommand(async () =>
            {
                //切換到登出頁面，並且清空已經登入的使用者資訊與本機快取資料
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，正在清空快取資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    await MainHelper.UserLoginService.Read();
                    MainHelper.UserLoginService.Item = new Models.Users();
                    MainHelper.UserLoginService.Item.Account = "";
                    await MainHelper.UserLoginService.Write();

                    MainHelper.UserTasksService.Items = new List<Models.UserTasks>();
                    await MainHelper.UserTasksService.Write();
                    MainHelper.UserHistoryTaskService.Items = new List<Models.UserTasks>();
                    await MainHelper.UserHistoryTaskService.Write();
                }

                await _navigationService.NavigateAsync("xf:///UserLoginPage");
            });

            #endregion
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (string.IsNullOrEmpty(MainHelper.UserLoginService.Item.PhotoUrl) == false)
            {
                UserPhoto = MainHelper.UserLoginService.Item.PhotoUrl;
            }
            UserName = MainHelper.UserLoginService.Item.Name;
            if (MainHelper.UserLoginService.Item.Account.ToLower() == "admin")
            {
                管理者模式 = true;
            }
            else
            {
                管理者模式 = false;
            }

            await Task.Delay(100);
        }

    }
}
