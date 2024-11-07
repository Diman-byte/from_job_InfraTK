namespace Common
{
    /// <summary>
    /// Режим сжатия
    /// </summary>
    public enum CompressionModeEnum
    {
        /// <summary>
        /// Сжатие не используется
        /// </summary>
        NotUse,

        /// <summary>
        /// Исключить повторяющиеся данные 
        /// </summary>
        Duplicate,

        /// <summary>
        /// Исключить данные, изменения которых не превышают допустимое в инженерных единицах 
        /// </summary>
        ToleranceEng,

        /// <summary>
        /// Исключить данные, изменения которых не превышают допустимое в процентах от диапазона 
        /// </summary>
        TolerancePerc,
    }
}