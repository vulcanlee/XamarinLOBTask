using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBTask.ViewModels
{
    public class DrawerMenuVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Font Awesome 的圖示名稱
        /// </summary>
        public string IconName { get; set; }
        /// <summary>
        /// 功能表項目要顯示的文字
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 按下這個功能表項目之後，要執行的命令
        /// </summary>
        public DelegateCommand DrawMenuCommand { get; set; }
    }
}
