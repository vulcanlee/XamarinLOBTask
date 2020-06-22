using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LOBTask.Models
{
    /// <summary>
    /// 需要請首頁 ListView 控制項，進行更新資料的事件
    /// </summary>
    public class TaskRefreshEventEvent : PubSubEvent<TaskRefreshEventPayload>
    {

    }

    /// <summary>
    /// 需要請首頁 ListView 控制項，進行更新資料的事件時候會傳遞的內容
    /// </summary>
    public class TaskRefreshEventPayload
    {
        public string Account { get; set; } = "";
    }
}
