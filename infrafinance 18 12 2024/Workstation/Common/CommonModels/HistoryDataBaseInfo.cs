using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Common.CommonModels
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

        /// <summary>
        /// Использовать историческую БД
        /// </summary>
        public bool UseHistoryDb { get; set; }

        /// <summary>
        /// Использовать резервную историческую БД
        /// </summary>
        public bool UseRedundantHistoryDb { get; set; }

        /// <summary>
        /// Резервный узел исторической БД
        /// </summary>
        public string RedundantHistoryDbHost { get; set; }

        /// <summary>
        /// Длительность ожидания повторного подключения к базе (сек)
        /// </summary>
        public uint HistoryDbReconnectInterval { get; set; }
    }
}
