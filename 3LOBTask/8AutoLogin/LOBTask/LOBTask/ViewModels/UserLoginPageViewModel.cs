using LOBTask.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LOBTask.ViewModels
{

    public class UserLoginPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Account { get; set; }
        public string Password { get; set; }

        public DelegateCommand SigninCommand { get; set; }

        private readonly INavigationService _navigationService;

        public UserLoginPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SigninCommand = new DelegateCommand(async () =>
            {
                //使用者身分驗證：登入
                using (var dlg = Acr.UserDialogs.UserDialogs.Instance.Loading("進行使用者身分驗證"))
                {
                    var fooResult = await MainHelper.UserLoginService.GetAsync(Account, Password);
                    if (fooResult.Success == false)
                    {
                        Acr.UserDialogs.UserDialogs.Instance.Alert(fooResult.Message, "警告", "確定");
                    }
                    else
                    {
                        await _navigationService.NavigateAsync("xf:///MDPage/NaviPage/HomePage");
                    }
                }

            });

#if DEBUG
            #region 這裡的程式碼，只有在除錯模式下，才會執行到
            Account = "user1";
            Password = "pw1";
            #endregion
#endif
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
