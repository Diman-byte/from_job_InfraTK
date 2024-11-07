using System;

namespace Common.Configs
{
    /// <summary>
    /// Интервал обучающих данных
    /// </summary>
    [Serializable]
    public class TrainDateTimeSet
    {
        /// <summary>
        /// Идентификатор источника обучающих данных
        /// </summary>
        public int SourceId;

        /// <summary>
        /// Дата начала данных
        /// </summary>
        public DateTime dateTimeFrom;

        /// <summary>
        /// Дата окончания данных
        /// </summary>
        public DateTime dateTimeTo;
    }

    /// <summary>
    /// Интервал обучающих данных
    /// </summary>
    [Serializable]
    public class MTTFTrainDateTimeSet: TrainDateTimeSet
    {
        /// <summary>
        /// Дата критического состояния
        /// </summary>
        public DateTime CriticalDateTime;       
    }


}
