using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configs
{
    public class OleDbClientConfig
    {
        /// <summary>
        /// Строка подключения к серверу
        /// </summary>
        public string ConnectionString;

        /// <summary>
        /// Запрос для чтения текущих данных
        /// </summary>
        public string ReadCurrentDataQuery;

        /// <summary>
        /// Запрос для чтения исторических данных
        /// </summary>
        public string ReadHistoryDataQuery;

        /// <summary>
        /// Запрос для записи данных
        /// </summary>
        public string WriteDataQuery;

        /// <summary>
        /// Такт опроса переменных
        /// </summary>
        public uint TactReadingTags=5;

        /// <summary>
        /// Формат даты и времени для текущих и исторических данных
        /// </summary>
        public string ReadDataDateTimeFormat = "yyyy.MM.dd HH:mm:ss";
    }
}
