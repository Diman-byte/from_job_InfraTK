using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    public class ClassificModelConfig
    {
        /// <summary>
        /// Включить сглаживающий фильтр вероятности класса
        /// </summary>
        public bool UseDevSmoothFilter = true;

        /// <summary>
        /// Постоянная времени сглаживающий фильтр вероятности класса
        /// </summary>
        public uint TimeDevSmoothFilter = 300;
    }
}
