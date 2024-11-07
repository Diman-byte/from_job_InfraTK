using System;
using System.Collections.Generic;

namespace Common.Configs
{
    /// <summary>
    /// Настройки источника CsvReader
    /// </summary>
    public class CsvReaderConfig
    {
        /// <summary>
        /// Файлы
        /// </summary>
        public List<ReadFile> ReadFiles = new List<ReadFile>();

        /// <summary>
        /// Период дискретизации
        /// </summary>
        public uint DiscretizationPeriod = 1;

        /// <summary>
        /// Разделитель данных (Обязательное поле)
        /// </summary>
        public char Separator = ';';

        /// <summary>
        ///  Экранирование разделителя
        /// </summary>
        public char? EscapingSeparator;

        /// <summary>
        /// Разделитель дробной части (Обязательное поле)
        /// </summary>
        public char FractionalPartSeparator = '.';

        /// <summary>
        /// Индекс колонки даты и времени
        /// </summary>
        public uint ColIndexOfDateTime = 1;

        /// <summary>
        /// Индекс колонки даты и времени
        /// </summary>
        public uint ColIndexOfDate = 1;

        /// <summary>
        /// Индекс колонки даты и времени
        /// </summary>
        public uint ColIndexOfTime = 1;

        /// <summary>
        /// Индекс строки начала имен переменных(Обязательное поле)
        /// </summary>
        public uint RowIndexOfTagNames = 1;

        /// <summary>
        /// Индекс строки начала данных (Обязательное поле)
        /// </summary>
        public uint RowIndexOfDataStart = 2;

        /// <summary>
        /// Индекс колонки начала данных (Обязательное поле)
        /// </summary>
        public uint ColIndexOfDataStart = 2;

        /// <summary>
        /// Индекс строки описания
        /// </summary>
        public uint? RowIndexOfTagDescriptions = null;

        /// <summary>
        /// Индекс строки единиц измерения
        /// </summary>
        public uint? RowIndexOfTagUnits = null;

        public uint? RowIndexOfTagMinValues;
        public uint? RowIndexOfTagMaxValues;

        public string DateFormat = "dd.MM.yyyy";
        public string TimeFormat = "H:mm:ss";

        public bool IsDateAndTimeFormatCombined = true;
        public string CombinedDateAndTimeFormat = "dd.MM.yyyy HH:mm:ss";
    }

    public class ReadFile
    {
        public string Name;

        public string NameView 
        {
            get 
            { 
                return Name; 
            }
        }

        public ReadFileStatusEnum Status;
        public string StatusView 
        { 
            get 
            {
                string status = "";
                switch (Status)
                {
                    case ReadFileStatusEnum.idle:
                        status = "ожидание чтения";
                        break;
                    case ReadFileStatusEnum.isReading:
                        status = "в процессе чтения";
                        break;
                    case ReadFileStatusEnum.isReaded:
                        status = "прочитан";
                        break;
                    case ReadFileStatusEnum.errorRead:
                        status = "ошибка чтения";
                        break;
                    default:
                        break;
                }

                return status; 
            }
        }
        public DateTime DataFrom { get; set; }
        public DateTime DataTo { get; set; }
        public uint CountTags { get; set; }
    }
}
