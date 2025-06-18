using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Models
{
    // модель данных, в таком виде получаем пo REST
    public class AssetGroupDto
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id родителя группы
        /// </summary>
        public Guid? ParentId { get; set; }
        public Guid ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrgStructureId { get; set; }
    }
}
