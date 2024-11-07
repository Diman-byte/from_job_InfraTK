using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    public class ClassificModelTrainSettings
    {
        /// <summary>
        /// Использовать максимальное количество значений выборки
        /// </summary>
        public bool UseTrainMaxDataPoint = true;

        /// <summary>
        /// Максимальное количество значений выборки
        /// </summary>
        public int TrainMaxDataPoint = 3000;

        /// <summary>
        /// Доля обучающей выборки
        /// </summary>
        public int TrainPart = 70;

        /// <summary>
        /// Длительность обучения
        /// </summary>
       // public uint AutoMLExperimentTime = 30;

        /// <summary>
        /// Использовать нормализацию
        /// </summary>
        public bool UseNormalize = false;
    }
}
