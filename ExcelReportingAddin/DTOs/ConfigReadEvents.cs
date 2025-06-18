using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReportingAddin.DTOs
{
    public class ConfigReadEvents
    {
        public SelectTypeEventsClass SelectTypeEvents { get; set; } = new SelectTypeEventsClass();

        public TimeSettingsEvents TimeSettingsStart { get; set; } = new TimeSettingsEvents();

        public TimeSettingsEvents TimeSettingsEnd { get; set; } = new TimeSettingsEvents();
        // описание этих классов в ConfigReadData

        public FormatEventsClass FormatEvents { get; set; } = new FormatEventsClass();

        public List<Asset_Class> SelectedAsset {  get; set; } = new List<Asset_Class>();
    }

    public class SelectTypeEventsClass
    {
        /// <summary>
        /// уведомления
        /// </summary>
        public bool Notifications { get; set; } = true;

        /// <summary>
        /// сигнализации
        /// </summary>
        public bool Alarms { get; set; }

        /// <summary>
        /// деблокировочные ключи
        /// </summary>
        public bool UnlockingKeys { get; set; }

        /// <summary>
        /// нарушения HTP
        /// </summary>
        public bool ViolationsHTP { get; set; }

        /// <summary>
        /// действия оператора
        /// </summary>
        public bool OperatorActions { get; set; }
    }

    public class TimeSettingsEvents
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
        // описание этого класса в ConfigReadData

        /// <summary>
        /// время абсолютное
        /// </summary>
        public DateTime? AbsolutTime { get; set; }
    }

   public class FormatEventsClass
    {
        public string NamePageNotifications { get; set; } = "Notifications";
        public string NamePageAlarms { get; set; } = "Alarms";
        public string NamePageUnlockingKeys { get; set; } = "DK";
        public string NamePageViolations { get; set; }
        public string NamePageOperatorActions { get; set; }
        public string Format { get; set; } = "dd.MM.yyyy HH:mm:ss";
        public bool NameAttributes { get; set; } = true;
        public bool AutoFillingEvents { get; set; }
    }

    // для соотвествия тега и его имени и чтобы выводилось в SelectedAssetsCLB только имя
    public class Asset_Class
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
