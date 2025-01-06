using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Common.CommonModels
{
    public class DataBaseInfo
    {
        /// <summary>
        /// Ip БД
        /// </summary>
        public string Host;

        /// <summary>
        /// Порт для подключения к БД
        /// </summary>
        public string Port;

        /// <summary>
        /// Имя БД
        /// </summary>
        public string DataBase;

        /// <summary>
        /// Пользовоатель БД
        /// </summary>
        public string User;

        /// <summary>
        /// Пароль к БД
        /// </summary>
        public string Password;

        /// <summary>
        /// Таймаут(время) на выполнение запросов к БД на сервере среды исполнения в секундах
        /// </summary>
        public int CommandTimeout;

        /// <summary>
        /// Включение пула соединений с БД
        /// True - активировать pooling
        /// False - отключить pooling
        /// По умолчанию - true
        /// </summary>
        public bool IsPoolingEnabled;
    }
}
