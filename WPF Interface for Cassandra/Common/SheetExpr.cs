using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Условие сегментации
    /// </summary>
    public class SheetExpr
    {
        public List<int> Param = new List<int>();
        public string exp;
    }

    /// <summary>
    /// выражение для расчетов
    /// </summary>
    public class CalcExpr
    {
        public List<int> GlobalParam = new List<int>();
        public List<int> LocalParam = new List<int>();
        public string exp;
    }
}
