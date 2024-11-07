using System.Collections.Generic;

namespace Common.Configs
{
    /// <summary>
    /// Результаты обучения модели классификации
    /// </summary>
    public class ClassificEvaluationReport
    {
        /// <summary>
        /// Логарифмическая ошибка
        /// </summary>
        public double LogLoss;

        /// <summary>
        /// Логарифмическая ошибка для каждого класса
        /// </summary>
        public List<double> PerClassLogLoss = new List<double>();

        /// <summary>
        /// Объем обучающей выборки для каждого класса. Ключ - id класса
        /// </summary>
        public List<int> TrainSize = new List<int>();

        /// <summary>
        /// Строка даты обучения
        /// </summary>
        public string TrainDate;

        public double MacroAccuracy;
        public double MicroAccuracy;
    }
}
