using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Models
{
    public class TagValDto
    {
        /// <summary>
        /// Id тега
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Хорошее значение
        /// </summary>
        public bool IsGood { get; set; }
    }
}
