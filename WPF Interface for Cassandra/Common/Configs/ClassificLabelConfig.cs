using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    /// <summary>
    /// Конфигурация класса
    /// </summary>
    public class ClassificLabelConfig
    {
        /// <summary>
        /// Индентификатор класса (т.к. для классов не предусмотрена отдельная таблица генерируется при создании класса в формате Guid)
        /// </summary>
        public Guid Id;

        /// <summary>
        /// Имя класса
        /// </summary>
        public string Name;

        /// <summary>
        /// Интервалы обучающих данных
        /// </summary>
        public List<TrainDateTimeSet> TrainDataSets = new List<TrainDateTimeSet>();

        /// <summary>
        /// Идентификатор выходной переменной вероятности данного класса  
        /// </summary>
        public int ProbOfClassTagId;
    }
}
