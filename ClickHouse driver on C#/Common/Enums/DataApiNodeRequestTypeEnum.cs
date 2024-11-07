namespace Common
{
    public enum DataApiNodeRequestTypeEnum
    {
        /// <summary>
        /// Запрос на управление режимом чтения исторических данных
        /// </summary>
        setModeReadHistData,

        /// <summary>
        /// Запрос на чтение режима чтения исторических данных
        /// </summary>
        getModeReadHistData,

        /// <summary>
        /// Запрос на очистку базы исторических данных
        /// </summary>
        ClearHistData,

        /// <summary>
        /// Запрос на очистку проекта
        /// </summary>
        ClearProject,        
    }
}