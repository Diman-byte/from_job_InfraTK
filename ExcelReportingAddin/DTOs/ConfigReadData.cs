﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReportingAddin.DTOs
{
    // класс для сохранения настроек окна "Чтение данных"
    public class ConfigReadData
    {
        /// <summary>
        /// выбранные в окне теги
        /// </summary>
        public List<Guid> SelectedTagIds {  get; set; } = new List<Guid>();

        /// <summary>
        /// выбранный актив(объект)
        /// </summary>
        public Guid SelectedAsset {  get; set; }

        /// <summary>
        /// параметры времени начала диапазона
        /// </summary>
        public TimeSettings TimeSettingsStart { get; set; } = new TimeSettings();

        /// <summary>
        /// параметры времени конца диапазона
        /// </summary>
        public TimeSettings TimeSettingsEnd { get; set; } = new TimeSettings();

        /// <summary>
        /// тип запрашиваемых исторических данных(все значения или срезы)
        /// </summary>
        public string ParametrGettingHistoricalData { get; set; }

        /// <summary>
        /// количество точек среза
        /// </summary>
        public int PointsAmount { get; set; }

        /// <summary>
        /// тип среза
        /// </summary>
        public string SliceType { get; set; } = "FirstPoint";

        /// <summary>
        /// вкладка "Формат"
        /// </summary>
        public TabFormat TabFormat { get; set; } = new TabFormat();
    }

    public class TimeSettings
    {
        /// <summary>
        /// режим даты
        /// </summary>
        public string Mode { get; set; } = "Relative"; // по умолчанию "Относительное"

        /// <summary>
        /// тип относительного времени(в КомбоБокс). по умолчанию "Сейчас"
        /// </summary>
        public string Sign { get; set; } = "Сейчас";

        /// <summary>
        /// значение сдвига относительного времени
        /// </summary>
        public RelativeOffset RelativeOffset { get; set; } = new RelativeOffset();

        /// <summary>
        /// время абсолютное
        /// </summary>
        public DateTime? AbsolutTime { get; set; }
    }

    public class RelativeOffset
    {
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    /// <summary>
    /// Класс, отвечающий за параметры вкладки "Формат"
    /// </summary>
    public class TabFormat
    {
        /// <summary>
        /// имя листа Excel, куда будем выгружать данные
        /// </summary>
        public string NameSheet { get; set; } = "Data";

        /// <summary>
        /// формат даты и времени
        /// </summary>
        public string Format { get; set; } = "dd.MM.yyyy HH:mm:ss";

        /// <summary>
        /// отображать имена тегов(да или нет)
        /// </summary>
        public bool IsDisplayTagNames { get; set; } = true;

        /// <summary>
        /// отображать описание тегов(да или нет)
        /// </summary>
        public bool IsDisplayTagDescription { get; set; }

        /// <summary>
        /// заполнять автоматически данными при открытии книги
        /// </summary>
        public bool AutoFillingData { get; set; }
    }

}
