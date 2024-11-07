namespace Common
{
    /// <summary>
    /// Тип изменения элемента
    /// </summary>
    public enum ChangeTypeEnum
    {
        /// <summary>
        /// Нет изменений
        /// </summary>
        None,
        /// <summary>
        /// Создание элемента
        /// </summary>
        Create,

        /// <summary>
        /// Обновление элемента
        /// </summary>
        Update,

        /// <summary>
        /// Обновление свойств элемента
        /// </summary>
        PropertyUpdate,

        /// <summary>
        /// Удаление элемента
        /// </summary>
        Delete
    }
}