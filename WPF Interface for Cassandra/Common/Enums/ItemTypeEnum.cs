namespace Common
{
    /// <summary>
    /// Тип элемента
    /// </summary>
    public enum ItemTypeEnum
    {
        /// <summary>
        /// Сервис
        /// </summary>
        Service,
                
        /// <summary>
        /// Группа активов
        /// </summary>
        NodeGroup,

        /// <summary>
        /// Узел
        /// </summary>
        Node,

        /// <summary>
        /// Группа тегов
        /// </summary>
        TagGroup,

        /// <summary>
        /// Тег
        /// </summary>
        Tag,

        /// <summary>
        /// Модуль алогоритма
        /// </summary>
        AlgModule,

        /// <summary>
        /// Уведомление алогоритма
        /// </summary>
        AlgNotification,

        /// <summary>
        /// Уведомление модели прогнозирования
        /// </summary>
        AlgPredictModel,

        /// <summary>
        /// Уведомление модели прогнозирования времени до критического состояния
        /// </summary>
        AlgMTTFModel,

        /// <summary>
        /// Уведомление модели классификации
        /// </summary>
        AlgClassificModel,
    }
}