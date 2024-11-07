using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configs
{
    public class PHDClientConfig
    {
        /// <summary>
        /// Ip адрес сервера
        /// </summary>
        public string PhdHost = "localhost";

        /// <summary>
        /// Логин от PHD сервера
        /// </summary>
        public string PhdLogin;

        /// <summary>
        /// Пароль от PHD сервера
        /// </summary>
        public string PhdPassword;

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
