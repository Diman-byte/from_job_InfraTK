namespace Common.Configs
{
    public class PredictModelConfig
    {        
        /// <summary>
        /// Включить сглаживающий фильтр отклонения прогноза
        /// </summary>
        public bool UseDevSmoothFilter = true;

        /// <summary>
        /// Постоянная времени сглаживающий фильтр отклонения прогноза
        /// </summary>
        public uint TimeDevSmoothFilter = 300;

        /// <summary>
        /// Включить сглаживающий фильтр прогноза
        /// </summary>
        public bool UsePredictSmoothFilter = false;

        /// <summary>
        /// Постоянная времени сглаживающий фильтр рогноза
        /// </summary>
        public uint TimePredictSmoothFilter = 300;

        /// <summary>
        /// Допустимое отклонение прогноза от факта, заданное пользователем
        /// </summary>
        public double UpLimit = 0;

        /// <summary>
        /// Допустимое отклонение прогноза от факта, заданное пользователем
        /// </summary>
        public double LowLimit = 0;
    }


}
