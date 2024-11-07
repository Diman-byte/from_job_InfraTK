namespace Common.Configs
{
    /// <summary>
    /// Результаты обучения
    /// </summary>
    public class EvaluationReport
    {        
        /// <summary>
        /// коэф. детерминации
        /// </summary>
        public double R2;

        /// <summary>
        /// Строка даты обучения
        /// </summary>
        public string TrainDate;
                
        /// <summary>
        /// Средняя квадратичная ошибка обучения
        /// </summary>
        public double MeanSquaredError;

        /// <summary>
        /// Средняя абсолютная ошибка обучения
        /// </summary>
        public double MeanAbsoluteError;

        /// <summary>
        /// Стандартное отклонение Y
        /// </summary>
        public double StandartLossY;
                
        /// <summary>
        /// Допустимое отклонение прогноза от факта, расчитанное алгоритмом
        /// </summary>
        public double CountUpLimit;

        /// <summary>
        /// Допустимое отклонение прогноза от факта, расчитанное алгоритмом
        /// </summary>
        public double CountLowLimit;

        /// <summary>
        /// объем обучающей выборки
        /// </summary>
        public int TrainSize;

        /// <summary>
        /// объем оценочной выборки
        /// </summary>
        public int EvaluateSize;
        
        /// <summary>
        /// Имя алгоритма автоматизированного машинного обучения
        /// </summary>
        public string AutoMLTrainerName;
    }
}
