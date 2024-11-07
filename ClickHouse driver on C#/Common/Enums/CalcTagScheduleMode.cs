using System.ComponentModel;

namespace Common
{
    /// <summary>
    /// Способ вычисления тегов по рассписанию
    /// </summary>
    public enum CalcTagScheduleMode
    {
        [Description("Каждый день")]
        EveryDay,
        [Description("Каждый месяц")]
        EveryMonth,
        [Description("Каждый год")]
        EveryYear,
        [Description("На календарный конец месяца")]
        EndOfMonth,

    }
}