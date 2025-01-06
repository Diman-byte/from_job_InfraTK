using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReportingAddin.DTOs
{
    // используем для организации данных о группах в иерархическую структуру
    public class AssetGroupTreeNodeDto
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список потомков(детей) группы
        /// </summary>
        public List<AssetGroupTreeNodeDto> Children { get; set; } = new List<AssetGroupTreeNodeDto>();
    }
}
