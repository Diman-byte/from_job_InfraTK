using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.Models.Main.EngineeringUnits
{
    public class UnitGroup
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public Guid? AssetId { get; set; }
        public Guid? AssetGroupId { get; set; }
        public string GroupName { get; set; }
    }
}
