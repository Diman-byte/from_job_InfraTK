namespace Common
{
    /// <summary>
    /// Статус чтения файла
    /// </summary>
    public enum ReadFileStatusEnum
    {
        idle,
        isReading,
        isReaded,
        errorRead,
    }
}