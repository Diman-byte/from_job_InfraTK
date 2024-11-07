using System.Collections.Generic;

namespace Common.Configs
{
    public class TrainSettings
    {       
        /// <summary>
        /// Интервалы обучающих данных
        /// </summary>
        public List<TrainDateTimeSet> TrainDataSets = new List<TrainDateTimeSet>();

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
        public uint AutoMLExperimentTime = 30;

        /// <summary>
        /// Использовать нормализацию
        /// </summary>
        public bool UseNormalize = false;
    }
}
