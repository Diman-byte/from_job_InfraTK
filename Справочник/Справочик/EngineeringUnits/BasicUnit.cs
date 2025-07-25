﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.Models.Main.EngineeringUnits
{

    // Определим класс для хранения данных о единицах измерения
    public class BasicUnit
    {
        // public int Id { get; set; }
        public int CollectionId { get; set; }
        public Guid? AssetId { get; set; }
        public Guid? AssetGroupId { get; set; }
        public int UnitGroupId { get; set; }
        public string Name { get; set; }
        public string National { get; set; }
        public string? International { get; set; }
        public string? NationalCode { get; set; }
        public string? InternationalCode { get; set; }
        public string? Code { get; set; }
    }
}
