using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LOBTask.Converters
{
    /// <summary>
    /// 工作紀錄狀態可見度數值轉換器，當工作紀錄狀態較低的時候，無法看到工作紀錄狀態較高的欄位資料
    /// </summary>
    public class TaskStatusToVisibleConverter : IValueConverter
    {
        /// <summary>
        /// 要比對的狀態
        /// </summary>
        public Models.TaskStatus WatchStatus { get; set; } = Models.TaskStatus.REPORTED;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool fooResult = true;
            Models.TaskStatus CurrentStatus = Models.TaskStatus.NOT_START;

            if (value is Models.TaskStatus)
            {
                CurrentStatus = (Models.TaskStatus)value;
            }
            int fooCurrent = (int)CurrentStatus;
            int fooWatch = (int)WatchStatus;

            if (fooCurrent >= fooWatch)
            {
                fooResult = true;
            }
            else
            {
                fooResult = false;
            }
            return fooResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
