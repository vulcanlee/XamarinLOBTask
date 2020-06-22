using LOBTask.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOBTask.Helpers
{
    /// <summary>
    /// 這是整個應用程式都可以存取的一個支援類別屬性與方法
    /// </summary>
    public class MainHelper
    {

        #region Azure Mobile App 相關設定
        /// <summary>
        /// 指向 Azure Mobile App 服務的主要網址
        /// </summary>
        public static string MainURL = "http://XamarinLOBTask.azurewebsites.net/";
        /// <summary>
        /// Azure Mobile App 線上版本的用戶端
        /// </summary>
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient AzureMobileClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(MainURL);
        #endregion

        #region 常用的變數 Constant
        /// <summary>
        /// 呼叫 API 的最上層名稱
        /// </summary>
        public static string BaseAPIUrl = $"{MainURL}api/";
        /// <summary>
        /// 使用者身分驗證的 API 名稱
        /// </summary>
        public static string UserLoginAPIName = $"UserLogin";
        public static string UserLoginAPIUrl = $"{BaseAPIUrl}{UserLoginAPIName}";
        /// <summary>
        /// 指派工作紀錄的的 API 名稱
        /// </summary>
        public static string UserTasksAPIName = $"UserTasks";
        /// <summary>
        /// 查詢指定時段內的工作紀錄的 API 名稱
        /// </summary>
        public static string UserTasksHistoryAPIName = $"Filter";
        public static string UserTasksCompletionFileName = $"UserTasksCompletion";
        public static string UserTasksAPIUrl = $"{BaseAPIUrl}{UserTasksAPIName}";
        public static string UserTasksHistoryAPIUrl = $"{BaseAPIUrl}{UserTasksAPIName}/{UserTasksHistoryAPIName}";
        public static string UploadImageAPIName = $"UploadImage";
        public static string UploadImageAPIUrl = $"{BaseAPIUrl}{UploadImageAPIName}";
        public static string 資料主目錄 = $"Data";

        #endregion

        #region Repository (此處為方便開發，所以，所有的 Repository 皆為全域靜態可存取)
        public static UserLoginRepository UserLoginService = new UserLoginRepository();
        #endregion
    }
}
