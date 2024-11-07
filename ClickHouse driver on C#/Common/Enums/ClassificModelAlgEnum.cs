namespace Common
{
    /// <summary>
    /// Тип алгоритма модели классификации
    /// </summary>
    public enum ClassificModelAlgEnum
    {
        LightGbm,
        LbfgsMaximumEntropy,
        SdcaMaximumEntropy,
        NaiveBayes,
        SdcaNonCalibrated
    }
}