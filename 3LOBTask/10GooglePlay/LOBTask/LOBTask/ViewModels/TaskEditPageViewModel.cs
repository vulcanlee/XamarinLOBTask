﻿using LOBTask.Helpers;
using LOBTask.Models;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public class TaskEditPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id = 0;
        public ImageSource MyImageSource;
        public APIResult fooAPIResult { get; set; } = new APIResult();
        public UserTasksVM CurrentUserTasksVM { get; set; } = new UserTasksVM();

        public DelegateCommand GPS打卡Command { get; set; }
        public DelegateCommand QRCode打卡Command { get; set; }
        public DelegateCommand 工作資料儲存Command { get; set; }
        public DelegateCommand 直接拍照Command { get; set; }
        public DelegateCommand 相片庫挑選Command { get; set; }
        public DelegateCommand 工作回報Command { get; set; }

        public readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public TaskEditPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator,
            IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            #region 頁面中綁定的命令
            QRCode打卡Command = new DelegateCommand(async () =>
            {
                // 切換到條碼掃描頁面
                await _navigationService.NavigateAsync("CodeScannerPage");
            });

            GPS打卡Command = new DelegateCommand(async () =>
            {
                #region 使用 Geolocaor Plugin 取得當時手機所在的 GPS 位置座標
                // https://github.com/jamesmontemagno/GeolocatorPlugin
                try
                {
                    if (CrossGeolocator.IsSupported == false)
                    {
                        using (Acr.UserDialogs.UserDialogs.Instance.Toast("這個裝置並不支援 GPS 定位功能", TimeSpan.FromSeconds(2)))
                        {
                        }
                        return;
                    }
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    Plugin.Geolocator.Abstractions.Position position;
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後...", null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    }
                    if (position == null)
                    {
                        await _dialogService.DisplayAlertAsync("警告", "無法取得 GPS 位置", "確定");
                        return;
                    }

                    CurrentUserTasksVM.Checkin_Longitude = position.Longitude;
                    CurrentUserTasksVM.Checkin_Latitude = position.Latitude;

                    var fooUserTasks = UpdateUserTasks(CurrentUserTasksVM).Clone();
                    fooUserTasks.Status = Models.TaskStatus.CHECKIN;
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，回報GPS打卡資訊...", null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        fooAPIResult = await MainHelper.UserTasksService.PutAsync(fooUserTasks);
                    }
                    if (fooAPIResult.Success == true)
                    {
                        using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，取得該使用者的派工資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                        {
                            fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(CurrentUserTasksVM.Account);
                        }
                        if (fooAPIResult.Success == true)
                        {
                            await ViewModelInit();
                            _eventAggregator.GetEvent<TaskRefreshEventEvent>().Publish(new TaskRefreshEventPayload
                            {
                                Account = CurrentUserTasksVM.Account,
                            });
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                    }
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("警告", $"Unable to get location, may need to increase timeout. 發生異常: {ex.Message}", "確定");
                }
                #endregion
            });

            工作資料儲存Command = new DelegateCommand(async () =>
            {
                #region 儲存使用者輸入的資料
                var fooUserTasks = UpdateUserTasks(CurrentUserTasksVM).Clone();
                fooUserTasks.Status = Models.TaskStatus.INPUT;
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，回報輸入工作內容...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    fooAPIResult = await MainHelper.UserTasksService.PutAsync(fooUserTasks);
                }
                if (fooAPIResult.Success == true)
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，取得該使用者的派工資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(CurrentUserTasksVM.Account);
                    }
                    if (fooAPIResult.Success == true)
                    {
                        await ViewModelInit();
                        _eventAggregator.GetEvent<TaskRefreshEventEvent>().Publish(new TaskRefreshEventPayload
                        {
                            Account = CurrentUserTasksVM.Account,
                        });
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                }
                #endregion
            });

            直接拍照Command = new DelegateCommand(async () =>
            {
                await 拍照與上傳("TakePhoto");
            });

            相片庫挑選Command = new DelegateCommand(async () =>
            {
                await 拍照與上傳("PickPhoto");
            });

            工作回報Command = new DelegateCommand(async () =>
            {
                #region 工作回報
                var fooUserTasks = UpdateUserTasks(CurrentUserTasksVM).Clone();
                fooUserTasks.Status = Models.TaskStatus.REPORTED;
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，回報工作已完成...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    fooAPIResult = await MainHelper.UserTasksService.PutAsync(fooUserTasks);
                }
                if (fooAPIResult.Success == true)
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，取得該使用者的派工資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(CurrentUserTasksVM.Account);
                    }
                    if (fooAPIResult.Success == true)
                    {
                        await ViewModelInit();
                        _eventAggregator.GetEvent<TaskRefreshEventEvent>().Publish(new TaskRefreshEventPayload
                        {
                            Account = CurrentUserTasksVM.Account,
                        });
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                }
                #endregion
            });
            #endregion

            #region 事件聚合器訂閱
            //訂閱當QR Code 頁面有掃描結果，就會呼叫這個訂閱的委派方法
            _eventAggregator.GetEvent<ScanResultEvent>().Subscribe(async x =>
            {
                if (CurrentUserTasksVM.CheckinId == x.Result)
                {
                    #region 使用 QR Code 打卡 檢查所掃描的 QR Code 是否符合條件
                    try
                    {
                        var fooUserTasks = UpdateUserTasks(CurrentUserTasksVM).Clone();
                        fooUserTasks.Status = Models.TaskStatus.CHECKIN;
                        using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，回報QR Code 打卡工作內容...", null, null, true, Acr.UserDialogs.MaskType.Black))
                        {
                            fooAPIResult = await MainHelper.UserTasksService.PutAsync(fooUserTasks);
                        }
                        if (fooAPIResult.Success == true)
                        {
                            using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，取得該使用者的派工資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                            {
                                fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(CurrentUserTasksVM.Account);
                            }
                            if (fooAPIResult.Success == true)
                            {
                                await ViewModelInit();
                                _eventAggregator.GetEvent<TaskRefreshEventEvent>().Publish(new TaskRefreshEventPayload
                                {
                                    Account = CurrentUserTasksVM.Account,
                                });
                            }
                            else
                            {
                                await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                            }
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                        }
                    }
                    catch (Exception ex)
                    {
                        await _dialogService.DisplayAlertAsync("警告", $"發生異常: {ex.Message}", "確定");
                    }
                    #endregion
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("警告", "QR Code 不正確", "確定");
                }
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
            var fooItem = MainHelper.UserTasksService.Items.FirstOrDefault(x => x.Id == Id);
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

        /// <summary>
        /// 將ViewModel 使用的工作紀錄屬性物件，更新到 API 的工作紀錄模型資料上
        /// </summary>
        /// <param name="userTaskVM"></param>
        /// <returns></returns>
        UserTasks UpdateUserTasks(UserTasksVM userTaskVM)
        {
            var fooUserTasks = new UserTasks();
            fooUserTasks.Id = userTaskVM.Id;
            fooUserTasks.Account = userTaskVM.Account;
            fooUserTasks.CheckinDatetime = userTaskVM.CheckinDatetime;
            fooUserTasks.CheckinId = userTaskVM.CheckinId;
            fooUserTasks.Checkin_Latitude = userTaskVM.Checkin_Latitude;
            fooUserTasks.Checkin_Longitude = userTaskVM.Checkin_Longitude;
            fooUserTasks.Condition1_Ttile = userTaskVM.Condition1_Ttile;
            fooUserTasks.Condition1_Result = userTaskVM.Condition1_Result;
            fooUserTasks.Condition2_Ttile = userTaskVM.Condition2_Ttile;
            fooUserTasks.Condition2_Result = userTaskVM.Condition2_Result;
            fooUserTasks.Condition3_Ttile = userTaskVM.Condition3_Ttile;
            fooUserTasks.Condition3_Result = userTaskVM.Condition3_Result;
            fooUserTasks.Description = userTaskVM.Description;
            fooUserTasks.PhotoURL = userTaskVM.PhotoURL;
            fooUserTasks.Reported = userTaskVM.Reported;
            fooUserTasks.ReportedDatetime = userTaskVM.ReportedDatetime;
            fooUserTasks.Status = userTaskVM.Status;
            fooUserTasks.TaskDateTime = userTaskVM.TaskDateTime;
            fooUserTasks.Title = userTaskVM.Title;
            return fooUserTasks;
        }

        /// <summary>
        /// 將圖片上傳到後端 Web API 主機上
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task 拍照與上傳(string action)
        {
            #region 拍照與上傳
            // https://github.com/jamesmontemagno/MediaPlugin
            // https://github.com/dsplaisted/PCLStorage
            // 進行 Plugin.Media 套件的初始化動作
            await CrossMedia.Current.Initialize();

            // 確認這個裝置是否具有拍照的功能
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _dialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                return;
            }

            // 啟動拍照功能，並且儲存到指定的路徑與檔案中
            MediaFile file;
            if (action == "TakePhoto")
            {
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "Sample.jpg",
                    CompressionQuality = 92,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                });
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    CompressionQuality = 92,
                    PhotoSize = PhotoSize.Small,
                });
            }
            if (file == null)
                return;


            // 讀取剛剛拍照的檔案內容，轉換成為 ImageSource，如此，就可以顯示到螢幕上了
            // 要這麼做的話，是因為圖片檔案是儲存在手機端的永久儲存體中，不是隨著專案安裝時候，就部署上去的
            // 因此，需要透過 ImageSource.FromStream 來讀取圖片檔案內容，產生出 ImageSource 物件，
            // 再透過資料繫節綁訂到 View 上的 Image 控制項
            MyImageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            #region 將剛剛拍照的檔案，上傳到網路伺服器上

            using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，上傳照片中...", null, null, true, Acr.UserDialogs.MaskType.Black))
            {
                fooAPIResult = await MainHelper.UserTasksService.UploadImageAsync(file);
            }
            if (fooAPIResult.Success == true)
            {
                var fooUserTasks = UpdateUserTasks(CurrentUserTasksVM).Clone();
                fooUserTasks.Status = Models.TaskStatus.UPLOAD_IMAGE;
                fooUserTasks.PhotoURL = fooAPIResult.Payload as string;
                using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，更新工作內容中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                {
                    fooAPIResult = await MainHelper.UserTasksService.PutAsync(fooUserTasks);
                }
                if (fooAPIResult.Success == true)
                {
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading($"請稍後，取得該使用者的派工資料中...", null, null, true, Acr.UserDialogs.MaskType.Black))
                    {
                        fooAPIResult = await MainHelper.UserTasksService.GetDateRangeAsync(CurrentUserTasksVM.Account);
                    }

                    if (fooAPIResult.Success == true)
                    {
                        await ViewModelInit();
                        _eventAggregator.GetEvent<TaskRefreshEventEvent>().Publish(new TaskRefreshEventPayload
                        {
                            Account = CurrentUserTasksVM.Account,
                        });
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("警告", fooAPIResult.Message, "確定");
                }
            }
            else
            {

            }
            #endregion
            #endregion
        }
        #endregion
    }
}
