using System;

namespace Common.Configs
{
    public class PaAssetConfig
    {
        /// <summary>
        /// Такт моделирования актива 
        /// </summary>
        public int TactPeriod = 5;

        /// <summary>
        /// Режим выполнения
        /// </summary>
        public TransformModeEnum TransformMode;

        /// <summary>
        /// Режим чтения данных
        /// </summary>
        public ReadModeEnum ReadMode;

        /// <summary>
        /// Метка времени чтения исторических данных
        /// </summary>
        public string HistDataTimeStamp;

        /// <summary>
        /// Период чтения исторических данных
        /// </summary>
        public string ReadHistDataPeriod;

        /// <summary>
        ///Очищать БД истории при остановке чнеия исторических данных
        /// </summary>
        public bool ClearHistDataWhenStopRead;
    }

    [Serializable]
    public sealed class PaAssetTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PaAssetConfig Config { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PaAssetTemplate(string name, string description, int id, PaAssetConfig config)
        {
            Name = name;
            Description = description;
            Id = id;
            Config = config;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PaAssetTemplate()
        {

        }
    }
}
