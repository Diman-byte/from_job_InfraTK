namespace Common
{
    /// <summary>
    /// Статус модели
    /// </summary>
    public enum ModelStatusEnum
    {
        /// <summary>
        /// Готов
        /// </summary>
        isReady,

        /// <summary>
        /// Не сконфигурирована
        /// </summary>
        notConfig,

        /// <summary>
        /// Ошибка конфигурации
        /// </summary>
        errorConfig,

        /// <summary>
        /// Не обучена
        /// </summary>
        notTrain,

        /// <summary>
        /// В процессе обучения
        /// </summary>
        training,

        /// <summary>
        /// Ошибка обучения
        /// </summary>
        errorTrain,

        /// <summary>
        /// Не создана
        /// </summary>
        notCreate,

        /// <summary>
        /// Не определен
        /// </summary>
        notDefine
    }

    /// <summary>
    /// Тип модели
    /// </summary>
    public enum ModelTypeEnum
    {
        /// <summary>
        /// Модель прогнозирования
        /// </summary>
        predictModel,

        /// <summary>
        /// Модель прогнозирования времени до критического состояния
        /// </summary>
        mttfModel,

        /// <summary>
        /// Модель классификации
        /// </summary>
        classificModel,        
    }
}