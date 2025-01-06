using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Common.MsgLog
{
    /// <summary>
    /// Перечень возможных сообщений
    /// </summary>
    public static class ListMsg
    {
        /// <summary>
        /// Выполнено успешно
        /// </summary>
        public static string Done
        {
            get { return "Выполнено успешно"; }
        }

        /// <summary>
        /// Выполнение запускается
        /// </summary>
        public static string ProjIsInit
        {
            get { return "Выполнение запускается"; }
        }

        /// <summary>
        /// Выполнение остановлено
        /// </summary>
        public static string ProjIsStop
        {
            get { return "Выполнение остановлено"; }
        }

        /// <summary>
        /// Выполнение остановлено
        /// </summary>
        public static string ProjIsPause
        {
            get { return "Выполнение поставлено на паузу"; }
        }

        /// <summary>
        /// Выполняется
        /// </summary>
        public static string ProjIsRun
        {
            get { return "Выполняется"; }
        }

        /// <summary>
        /// Система (обратитесь к разработчику)
        /// </summary>
        public static string SystemError
        {
            get { return "Системная ошибка (обратитесь к разработчику). "; }
        }

        /// <summary>
        /// Ошибка выполения запроса по Api
        /// </summary>
        public static string ApiError
        {
            get { return "Ошибка выполения запроса по Api"; }
        }
    }
}
