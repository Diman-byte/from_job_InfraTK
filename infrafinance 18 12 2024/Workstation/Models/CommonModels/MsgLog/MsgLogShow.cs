using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workstation.Common.MsgLog;

namespace Workstation.Models.CommonModels.MsgLog
{
    public static class MsgLogShow
    {
        /// <summary>
        /// Вывод сообщения в окне без сохранения в журнале SystemLog
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowMsg(MsgLogClass msgLog)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                MessageWindow messageWindow = new MessageWindow(msgLog);
                messageWindow.Show();
            });

            /* MessageWindow messageWindow = new MessageWindow(msgLog);
             messageWindow.Show();      */
        }
    }
}
