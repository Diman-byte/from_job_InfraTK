using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Common.MsgLog
{
    public class MsgLogClass
    {
        /// <summary>
        /// Метка времени первого сообщения 
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Источник сообщения
        /// </summary>
        public string SourceLog;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string LogText;

        /// <summary>
        /// Детали сообщения
        /// </summary>
        public string LogDetails;

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public LogLevel TypeLog;

        /// <summary>
        /// Метка времени последнего сообщения
        /// </summary>
        public DateTime EndTimeStamp;

        /// <summary>
        /// Кол-во повторов сообщения
        /// </summary>
        public int Repeats;

        public MsgLogClass()
        {
            TypeLog = LogLevel.Error;
            TimeStamp = DateTime.Now;
            EndTimeStamp = DateTime.Now;
            SourceLog = string.Empty;
            LogText = string.Empty;
            LogDetails = string.Empty;
        }
    }
}
