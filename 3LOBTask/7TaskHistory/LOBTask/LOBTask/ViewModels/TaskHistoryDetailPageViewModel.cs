using LOBTask.Helpers;
using LOBTask.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LOBTask.ViewModels
{
    public class TaskHistoryDetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id = 0;
        public APIResult fooAPIResult { get; set; } = new APIResult();
        public UserTasksVM CurrentUserTasksVM { get; set; } = new UserTasksVM();

        private readonly INavigationService _navigationService;

        public TaskHistoryDetailPageViewModel(INavigationService navigationService)
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
            if (parameters.ContainsKey("ID"))
            {
                Id = Convert.ToInt64(parameters["ID"] as string);
                await ViewModelInit();
            }
        }

        #region 其他方法

        /// <summary>
        /// ViewModel 資料初始化
        /// </summary>
        /// <returns></returns>
        private async Task ViewModelInit()
        {
            var fooItem = MainHelper.UserHistoryTaskService.Items.FirstOrDefault(x => x.Id == Id);
            if (fooItem != null)
            {
                UpdateCurrentUserTaskVM(fooItem);
            }
            await Task.Delay(100);
        }

        /// <summary>
        /// 將 API 的工作紀錄模型資料，更新到 ViewModel 使用的工作紀錄屬性物件上
        /// </summary>
        /// <param name="userTasks"></param>
        void UpdateCurrentUserTaskVM(UserTasks userTasks)
        {
            CurrentUserTasksVM.Id = userTasks.Id;
            CurrentUserTasksVM.Account = userTasks.Account;
            CurrentUserTasksVM.CheckinDatetime = userTasks.CheckinDatetime;
            CurrentUserTasksVM.CheckinId = userTasks.CheckinId;
            CurrentUserTasksVM.Checkin_Latitude = userTasks.Checkin_Latitude;
            CurrentUserTasksVM.Checkin_Longitude = userTasks.Checkin_Longitude;
            CurrentUserTasksVM.Condition1_Ttile = userTasks.Condition1_Ttile;
            CurrentUserTasksVM.Condition1_Result = userTasks.Condition1_Result;
            CurrentUserTasksVM.Condition2_Ttile = userTasks.Condition2_Ttile;
            CurrentUserTasksVM.Condition2_Result = userTasks.Condition2_Result;
            CurrentUserTasksVM.Condition3_Ttile = userTasks.Condition3_Ttile;
            CurrentUserTasksVM.Condition3_Result = userTasks.Condition3_Result;
            CurrentUserTasksVM.Description = userTasks.Description;
            CurrentUserTasksVM.PhotoURL = userTasks.PhotoURL;
            CurrentUserTasksVM.Reported = userTasks.Reported;
            CurrentUserTasksVM.ReportedDatetime = userTasks.ReportedDatetime;
            CurrentUserTasksVM.Status = userTasks.Status;
            CurrentUserTasksVM.TaskDateTime = userTasks.TaskDateTime;
            CurrentUserTasksVM.Title = userTasks.Title;
        }
        #endregion
    }
}
