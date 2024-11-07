namespace Common.MsgLog
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

        /// <summary>
        /// Ошибка выполения запроса PostgreSQL
        /// </summary>
        public static string PgsqlError
        {
            get { return "Ошибка выполения запроса PostgreSQL"; }
        }

        /// <summary>
        /// Ошибка соединения с БД
        /// </summary>
        public static string PgsqlConnectError
        {
            get { return "Ошибка соединения с базой данных конфигурации. Убедитесь, что установлена СУБД PostgreSQL"; }
        }

        /// <summary>
        /// Соединение с БД PostgreSQL уже открыто
        /// </summary>
        public static string PgsqlConnectAlreadyOpen
        {
            get { return "Соединение с БД PostgreSQL уже открыто"; }
        }

        /// <summary>
        /// Ошибка Backup с использованием утилиты pg_dump.exe PostgreSQL
        /// </summary>
        public static string PgsqlBackupError
        {
            get { return "Ошибка Backup с использованием утилиты pg_dump.exe PostgreSQL"; }
        }

        /// <summary>
        /// Ошибка Restore с использованием утилиты pg_restore.exe PostgreSQL
        /// </summary>
        public static string PgsqlRestoreError
        {
            get { return "Ошибка Restore с использованием утилиты pg_restore.exe PostgreSQL"; }
        }

        /// <summary>
        /// Ошибка конфигурации
        /// </summary>
        public static string ConfigError
        {
            get { return "Ошибка конфигурации"; }
        }

        /// <summary>
        /// Запущено исполнение проекта
        /// </summary>
        public static string RunDataAnalytics
        {
            get { return "Запущена обработка данных"; }
        }

        /// <summary>
        /// Остановлено исполнение проекта
        /// </summary>
        public static string StopDataAnalytics
        {
            get { return "Остановлена обработка данных"; }
        }
    }
}
