namespace Common.Configs
{
    public class MTTFModelConfig
    {
        /// <summary>
        /// Включить сглаживающий фильтр отклонения прогноза
        /// </summary>
        public bool UseDevSmoothFilter = true;

        /// <summary>
        /// Постоянная времени сглаживающий фильтр отклонения прогноза
        /// </summary>
        public uint TimeDevSmoothFilter = 3600;        
    }


}
