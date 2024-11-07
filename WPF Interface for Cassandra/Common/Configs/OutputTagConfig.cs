using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    public class OutputTagConfig
    {        
        /// <summary>
        /// Идентификатор источника данных в который будут записываться выходные данные
        /// </summary>
        public int DataSourceId;

        /// <summary>
        /// Id тега у усточника данных в который будут записываться выходные данные
        /// </summary>
        public int DataSourceTagId;

        /// <summary>
        /// Имена переменных у источников 
        /// Используется для автоматического опеределения привязок в случае смены источника 
        /// </summary>
        public List<SourcesTag> SourcesTags = new List<SourcesTag>();    
    }
}
