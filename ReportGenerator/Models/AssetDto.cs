using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Models
{
    public class AssetDto
    {
        /// <summary>
        /// Id актива
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id группы активов
        /// </summary>
        public Guid? AssetGroupId { get; set; }

        /// <summary>
        /// Id службы обработки
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
