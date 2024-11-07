namespace Common
{
    /// <summary>
    /// Режим выполнения
    /// </summary>
    public enum TransformModeEnum
    {
        /// <summary>
        /// при измении данных
        /// </summary>
        DataChange,

        /// <summary>
        /// по такту 
        /// </summary>
        Tact,

        /// <summary>
        /// по такту и при изменении данных
        /// </summary>
        TactDataChange
    }
}