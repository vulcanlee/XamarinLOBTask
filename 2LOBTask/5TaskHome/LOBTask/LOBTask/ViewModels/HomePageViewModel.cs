using LOBTask.Helpers;
using LOBTask.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LOBTask.ViewModels
{

    public class HomePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 取得呼叫完成 Web API 的回報狀態物件
        /// </summary>
        public APIResult fooAPIResult { get; set; } = new APIResult();
        /// <summary>
        /// UserTasksListSelected 使用者所點選的工作紀錄
        /// </summary>
        public UserTasksVM UserTasksListSelected { get; set; } = new UserTasksVM();
        /// <summary>
        /// IsRefreshing 現在是否正在更新紀錄中
        /// </summary>
        public bool IsRefreshing { get; set; }
        /// <summary>
        /// 顯示可以派工的紀錄集合
        /// </summary>
        public ObservableCollection<UserTasksVM> UserTasksList { get; set; } = new ObservableCollection<UserTasksVM>();

        /// <summary>
        /// 使用者點選清單內的某個紀錄，觸發的命令
        /// </summary>
        public DelegateCommand ItemTappedCommand { get; set; }
        /// <summary>
        /// 需要重新取得最新派工資料的觸發命令
        /// </summary>
        public DelegateCommand DoRefreshCommand { get; set; }

        #region 儲存相依性注入的物件，這裡僅需要使用欄位來保存即可，不需要用到屬性
        public readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        // 建構式的三個參數，將會透過相依性注入服務，自動注入到該建構函式內
        public HomePageViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
            IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            #region 頁面中綁定的命令
            DoRefreshCommand = new DelegateCommand(async () =>
            {
                // 這裡使用 UserDialogs 顯示一個正在忙碌中的處理遮罩
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，正在取回最新派工紀錄中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    // 取得該使用者需要執行的派工紀錄
                    fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(
                        MainHelper.UserLoginService.Item.Account);

                    if (fooAPIResult.Success == true)  // 呼叫該 Web API 是成功的
                    {
                        await ViewModelInit();
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                    }
                    IsRefreshing = false;
                }
            });

            ItemTappedCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync($"TaskEditPage?ID={UserTasksListSelected.Id}");
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
            await ViewModelInit();
        }

        /// <summary>
        /// ViewModel 資料初始化
        /// </summary>
        /// <returns></returns>
        private async Task ViewModelInit()
        {
            var fooItems = await MainHelper.UserTasksService.Read();
            UserTasksList.Clear();
            // 故意暫停半秒鐘，以便觀察集合資料，真的有清除
            await Task.Delay(500);
            foreach (var item in fooItems)
            {
                AddViewModel(item);
            }
            //await Task.Delay(100);
        }

        /// <summary>
        /// 將 Repository 內的某筆紀錄，加入到 ViewModel 的集合物件內
        /// </summary>
        /// <param name="userTask"></param>
        void AddViewModel(UserTasks userTask)
        {
            UserTasksVM fooUserTasksVM = new UserTasksVM
            {
                Account = userTask.Account,
                CheckinDatetime = userTask.CheckinDatetime,
                CheckinId = userTask.CheckinId,
                Checkin_Latitude = userTask.Checkin_Latitude,
                Checkin_Longitude = userTask.Checkin_Longitude,
                Condition1_Result = userTask.Condition1_Result,
                Condition1_Ttile = userTask.Condition1_Ttile,
                Condition2_Result = userTask.Condition2_Result,
                Condition2_Ttile = userTask.Condition2_Ttile,
                Condition3_Result = userTask.Condition3_Result,
                Condition3_Ttile = userTask.Condition3_Ttile,
                Description = userTask.Description,
                Id = userTask.Id,
                PhotoURL = userTask.PhotoURL,
                Reported = userTask.Reported,
                ReportedDatetime = userTask.ReportedDatetime,
                Status = userTask.Status,
                TaskDateTime = userTask.TaskDateTime,
                Title = userTask.Title,
            };
            UserTasksList.Add(fooUserTasksVM);
        }
    }

}
