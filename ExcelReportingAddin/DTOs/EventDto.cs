using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExcelReportingAddin.DTOs
{
    //Атрибут [JsonPropertyName("имя_поля")] используется в System.Text.Json,
    //чтобы явно указать, какое имя JSON-поля соответствует свойству C#-класса при (де)сериализации.

    public class EventData
    {
        [JsonPropertyName("eventsVal")]
        public List<EventDto> Events { get; set; } = new List<EventDto>();
    }

    public class EventDto
    {
        [JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonPropertyName("timeStamp")]
        public long TimeStamp { get; set; }

        [JsonPropertyName("eventType")]
        public string EventType { get; set; }

        [JsonPropertyName("eventAttributes")]
        public List<EventAttribute> EventAttributes { get; set; } = new List<EventAttribute>();
    }

    public class EventAttribute
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }


    /// <summary>
    /// Условия для выборки событий
    /// </summary>
    public class EventCondition
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Знак (больше, меньше, равно)
        /// </summary>
        public string Sign { get; set; }
    }
}
