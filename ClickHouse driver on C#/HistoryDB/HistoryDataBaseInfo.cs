using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryDB
{
    public class HistoryDataBaseInfo
    {
        /// <summary>
        /// Ip БД
        /// </summary>
        public string Host;

        /// <summary>
        /// Порт для подключения к БД
        /// </summary>
        public int Port;

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
        public int CommandTimeout { get; set; }
    }
}
