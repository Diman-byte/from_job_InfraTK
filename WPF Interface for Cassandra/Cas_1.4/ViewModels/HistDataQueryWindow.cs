using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cas_1._4.ViewModels
{
    internal class HistDataQueryWindow
    {
    }
    public class HistDataResult
    {
        public int TagId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsGood { get; set; }
        public double Value { get; set; }
    }
}
