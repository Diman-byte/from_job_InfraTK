using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configs
{
    public class FaultDetectorConfig
    {        
        /// <summary>
        /// Использовать условие работы детектора
        /// </summary>
        public bool UseExpression = false;

        /// <summary>
        /// Условие работы детектора
        /// </summary>
        public string Expression = "true";

        /// <summary>
        /// Теги условия работы детектора
        /// </summary>
        public List<int> ExpressionTagsId = new List<int>();        
    }  

}
