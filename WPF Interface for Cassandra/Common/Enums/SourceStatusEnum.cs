namespace Common
{
    /// <summary>
    /// Статус источника данных
    /// </summary>
    public enum SourceStatusEnum
    {
        /// <summary>
        /// Готов
        /// </summary>
        isReady,
                
        /// <summary>
        /// Не сконфигурирован
        /// </summary>
        notConfig,

        /// <summary>
        /// Ошибка конфигурации
        /// </summary>
        errorConfig
    }
}