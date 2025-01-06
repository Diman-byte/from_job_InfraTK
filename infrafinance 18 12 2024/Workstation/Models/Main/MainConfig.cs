using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Models.Main
{
    /// <summary>
    /// Настройки
    /// </summary>
    [Serializable]
    // Атрибут [Serializable] в C# указывает, что класс может быть сериализован.
    // Это значит, что объект этого класса может быть преобразован в поток байтов (или другой формат)
    public sealed class ConfigXML
    {
        /// <summary>
        /// Имя настройки 
        /// </summary>
        public string Name;

        /// <summary>
        /// Описание настройки 
        /// </summary>
        public string Desc;

        /// <summary>
        /// Тит настройки
        /// </summary>
        public string ValType;

        /// <summary>
        /// Значение настройки
        /// </summary>
        public string Val;
    }

    /// <summary>
    /// Настройки
    /// </summary>
    [Serializable]
    public sealed class UserCredentialXML
    {
        /// <summary>
        /// Логин  
        /// </summary>
        public string Login;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Pass;

        /// <summary>
        /// Сохранить 
        /// </summary>
        public bool IsSave;
    }
}
