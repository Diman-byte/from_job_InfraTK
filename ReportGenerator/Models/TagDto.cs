using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Models
{
    public class TagDto
    {
        /// <summary>
        /// Id тега
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id актива
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Id группы тегов
        /// </summary>
        public Guid? TagGroupId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }


        public bool IsSelected { get; set; }
    }
}
