namespace Common
{
    public enum DataApiSourceRequestTypeEnum
    {
        /// <summary>
        /// Запрос на чтение файла
        /// </summary>
        readFile,

        /// <summary>
        /// Запрос на чтение доступных тегов источника
        /// </summary>
        readSourceAvailableTags,

        /// <summary>
        /// Запрос на проверку соединения
        /// </summary>
        testConnection,
    }
}