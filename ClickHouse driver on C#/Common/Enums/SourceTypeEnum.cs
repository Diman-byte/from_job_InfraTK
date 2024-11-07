namespace Common
{
    /// <summary>
    /// Тип источника данных
    /// </summary>
    public enum SourceTypeEnum
    {
        /// <summary>
        /// Файлы в формате CSV
        /// </summary>
        CsvFiles,

        /// <summary>
        /// Terralink Run Time
        /// </summary>
        TLRunTime,

        /// <summary>
        /// Клиент OPCDA сервера 
        /// </summary>
        OPCDA,

        /// <summary>
        /// Генератор данных
        /// </summary>
        DataGenerator,

        /// <summary>
        /// Информационная система Honeywell PHD
        /// </summary>
        PHDClient,

        /// <summary>
        /// SQL провайдер для OLEDB
        /// </summary>
        OleDbClient
    }
}