using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    /// <summary>
    /// Конфигурация входных тегов
    /// </summary>
    [Serializable]
    public class InputTagConfig
    {
        /// <summary>
        /// Идентификатор источника данных из которого будут читаться входные текущие данные
        /// </summary>
        public int DataSourceId;

        /// <summary>
        /// Имя тега у усточника данных из которого будут читаться входные текущие данные
        /// </summary>
        public int DataSourceTagId;

        /// <summary>
        /// Имена переменных у источников из которых будут читаться входные исторические данные
        /// Используется для автоматического опеределения привязок в случае смены источника 
        /// </summary>
        public List<SourcesTag> SourcesTags = new List<SourcesTag>();

        /// <summary>
        /// Режим сжатия
        /// </summary>
        public CompressionModeEnum CompressionMode = CompressionModeEnum.NotUse;

        /// <summary>
        /// Допустимое значение изменения данных при сжатии в инженерных единицах
        /// </summary>
        public float CompressionToleranceEng;

        /// <summary>
        /// Допустимое значение изменения данных при сжатии в процентах от диапазона
        /// </summary>
        public float CompressionTolerancePerc;
    }

    public class SourcesTag
    {
        public int SourceId;
        public int TagId;
        public string TagName;
    }
}
