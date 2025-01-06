using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workstation.Common.KeyCloak;
using Workstation.Common.Enums;

namespace Workstation.Common.CommonModels
{
    public static class StaticData
    {
        /// <summary>
        /// Информация о сервере идентификации KeyCloak
        /// </summary>
        public static KeyCloakInfo KeyCloakInfo;

        /// <summary>
        /// Токен пользователя
        /// </summary>
        public static JwtToken UserAccessToken;

        /// <summary>
        /// Информация о базе конфигурации
        /// </summary>
        public static DataBaseInfo ConfigDBInfo;

        /// <summary>
        /// Информация о базе проекта
        /// </summary>
        public static DataBaseInfo ProjDBInfo;

        /// <summary>
        /// Информация о базе логгирования
        /// </summary>
        public static DataBaseInfo MessageLogDBInfo;

        /// <summary>
        /// Информация о базе исторических данных
        /// </summary>
        public static HistoryDataBaseInfo HistDBInfo;

        /// <summary>
        /// Статус службы
        /// </summary>
        public static ServiceStatusEnum ServiceStatus;

        /// <summary>
        /// Id службы
        /// </summary>
        public static Guid ServiceId;

        /// <summary>
        /// Порт службы
        /// </summary>
        public static int ServicePort = 0;

        /// <summary>
        /// Пользователь службы
        /// </summary>
        public static string User;

        /// <summary>
        /// Путь до утилит базы конфигурации
        /// </summary>
        public static string DbUtilPath = "C:\\Program Files\\PostgreSQL\\14\\bin";
    }
}
