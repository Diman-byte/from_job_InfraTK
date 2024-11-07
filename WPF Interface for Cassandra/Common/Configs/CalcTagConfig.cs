using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    public class CalcTagConfig
    {
        /// <summary>
        /// Идентификаторы глобальных тегов
        /// </summary>
        public List<int> GlobalTagsId = new List<int>();

        /// <summary>
        /// Локальные теги
        /// </summary>
        public List<LocalTagConfig> LocalTags;

        /// <summary>
        /// Разобранные выражения состоящие из имени тега и выражения
        /// </summary>
        public List<TagExpression> Expressions;

        /// <summary>
        /// Режим вычислений
        /// </summary>
        public CalcTagModeEnum CalcMode;

        /// <summary>
        /// Активировать вычисление
        /// </summary>
        public bool IsActive = true;
        
        /// <summary>
        /// Вычислять исторические данные по запросу
        /// </summary>
        public bool UseCalcHistory;

        /// <summary>
        /// Период расчета в секундах по таймеру
        /// </summary>
        public uint TimerPeriod;

        /// <summary>
        /// Режим расчета по расписанию 
        /// </summary>
        public CalcTagScheduleMode SelectedScheduleMode;

        /// <summary>
        /// Период времени расчета по расписанию
        /// </summary>
        public DateTime ScheduleTime = DateTime.Now;
    }

    public class LocalTagConfig 
    {
        public int Id;
        public string Name;
        public string Desc;

        /// <summary>
        ///Начальное значение переменной
        /// </summary>
        public float InitValue = 0;

        /// <summary>
        /// Метка времени значения
        /// </summary>
        public DateTime InitValueDateTime = DateTime.Now;

        /// <summary>
        /// Хорошее ли значение
        /// </summary>
        public bool InitValueIsGood;

        /// <summary>
        /// Необходимо ли менять начальное значение переменной при расчете выражения
        /// </summary>
        //public bool IsChangingInitValue;

        /// <summary>
        ///Значение для тестирования
        /// </summary>
        /*[field: NonSerialized]
        public float TestValue = 0;*/

        /// <summary>
        /// Метка времени для тестирования
        /// </summary>
        /*[field: NonSerialized]
        public DateTime TestDateTime = DateTime.Now;*/
       
    }

    public sealed class TagExpression 
    {
        /// <summary>
        /// Выражение для расчета
        /// </summary>
        public string Expression;

        /// <summary>
        /// Id тэга для записи
        /// </summary>
        public int TagId;

        /// <summary>
        /// True - Локальный тэг False - Глобальный тэг 
        /// </summary>
        public bool IsLocalTag;

       
        /// <summary>
        /// Список имен всех тэгов,которые используются в выражении
        /// </summary>
        //public List<string> UsedTagsInExpression { get; set; }

        /// <summary>
        /// Список объектов специальных функций в выражении, где индекс - номер функции в выражении
        /// </summary>
        //public List<CustomFunction> CustomFunctions { get; private set; }       
    }
}
