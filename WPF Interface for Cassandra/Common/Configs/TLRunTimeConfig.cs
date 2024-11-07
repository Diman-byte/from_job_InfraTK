using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configs
{
    public class TLRunTimeConfig
    {
        /// <summary>
        /// Ip адрес сервера
        /// </summary>
        public string TLRunTimeHost = "localhost";

        /// <summary>
        /// Логин от TLRunTime сервера
        /// </summary>
        public string TLRunTimeLogin;

        /// <summary>
        /// Пароль от TLRunTime сервера
        /// </summary>
        public string TLRunTimePassword;

        /// <summary>
        /// Размер буфера данных
        /// </summary>
        public const int MaxCountDataBuffer = 3600;

        /// <summary>
        /// Такт опроса переменных
        /// </summary>
        public uint TactReadingTags;
    }
}
