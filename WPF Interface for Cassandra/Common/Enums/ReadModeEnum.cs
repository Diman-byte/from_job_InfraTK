namespace Common
{
    /// <summary>
    /// Режим чтения данных
    /// </summary>
    public enum ReadModeEnum
    {
        /// <summary>
        /// чтение текущих данных
        /// </summary>
        RealTimeData,

        /// <summary>
        /// чтение исторических данных 
        /// </summary>
        HistoricalData,

        /// <summary>
        /// Не читать данные
        /// </summary>
        NotReadData
    }
}