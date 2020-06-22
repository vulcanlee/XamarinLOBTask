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
    public class TaskHistoryPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsRefreshing { get; set; }
        public APIResult fooAPIResult { get; set; } = new APIResult();
        public UserTasks UserTasksListSelected { get; set; } = new UserTasks();
        public ObservableCollection<UserTasks> UserTasksList { get; set; } = new ObservableCollection<UserTasks>();

        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand DoRefreshCommand { get; set; }

        public readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public TaskHistoryPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
            IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            #region 頁面中綁定的命令
            DoRefreshCommand = new DelegateCommand(async () =>
            {
                // 這裡使用 UserDialogs 顯示一個正在忙碌中的處理遮罩
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，正在取回最新派工歷史紀錄中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    fooAPIResult = await MainHelper.UserHistoryTaskService.GetDateRangeAsync(
                        MainHelper.UserLoginService.Item.Account, DateTime.Now.AddDays(-7), DateTime.Now);

                    if (fooAPIResult.Success == true)
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
                await _navigationService.NavigateAsync($"TaskHistoryDetailPage?ID={UserTasksListSelected.Id}");
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

        #region 其他方法

        /// <summary>
        /// ViewModel 資料初始化
        /// </summary>
        /// <returns></returns>
        private async Task ViewModelInit()
        {
            // 在這裡，將只會顯示已經完工的紀錄清單
            var fooItems = (await MainHelper.UserHistoryTaskService.Read()).Where(x => x.Status == Models.TaskStatus.REPORTED);
            UserTasksList.Clear();
            await Task.Delay(500);
            foreach (var item in fooItems)
            {
                AddViewModel(item);
            }
        }

        void AddViewModel(UserTasks userTask)
        {
            UserTasksList.Add(userTask);
        }
        #endregion
    }
}
