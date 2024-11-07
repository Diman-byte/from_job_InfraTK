using Common.MsgLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common
{
    public static class DataProcessing
    {
        /// <summary>
        /// Предварительная обработка данных
        /// </summary>
        /// <param name="paramItems">Перечень параметров</param>        
        /// <param name="data">Данные для обработки</param>
        /// <param name="lastNormalizeData">Последние обработанные данные</param>
        /// <param name="lastInpuDataValTimeStamp">Метка времени последних обработанных данных</param>
        /// <param name="nextInpuDataValTimeStamp">Метка времени до которой будут обработаны данные</param>
        /// <param name="timePeriodNormalize">Диапазон врвмени дискретизации данных</param>
        public static Dictionary<int, List<DataVal>> PreparationData(Dictionary<int, List<DataVal>> data, List<int> tagsId, ref Dictionary<int, DataVal> lastNormalizeData, DateTime lastInpuDataValTimeStamp, DateTime nextInpuDataValTimeStamp, int timePeriodNormalize, StructuringMode normalizeType)
        {
            Dictionary<int, List<DataVal>> Result = new Dictionary<int, List<DataVal>>();
            if (data.Count == 0) return Result;
            foreach (var item in tagsId)
                Result.Add(item, new List<DataVal>());   //Создание ключей выходого словаря аналогично ключам входных данных   

            try
            {
                List<DateTime> NumderDateTimes = new List<DateTime>();

                if (normalizeType == StructuringMode.continuousRealTime)
                {
                    for (DateTime datetime = lastInpuDataValTimeStamp.AddSeconds(timePeriodNormalize); datetime < nextInpuDataValTimeStamp; datetime = datetime.AddSeconds(timePeriodNormalize))   //Задание нормированного ряда времени              
                                                                                                                                                                                               // for (DateTime datetime = LastInpuDataValTimeStamp; datetime < NextInpuDataValTimeStamp; datetime = datetime.AddSeconds(TimePeriodNormalize))    
                        NumderDateTimes.Add(datetime);
                }

                if (normalizeType == StructuringMode.slicesFromLastData)
                {
                    //Определение конечной даты выборки                    
                    List<DateTime> dateTimes0 = new List<DateTime>();
                    List<DateTime> dateTimes = new List<DateTime>();
                    foreach (KeyValuePair<int, List<DataVal>> keyValuePair in data)
                        for (int i = 0; i < keyValuePair.Value.Count; i++)
                            dateTimes0.Add(keyValuePair.Value[i].DateTime);
                    dateTimes = dateTimes0.Distinct().ToList().OrderBy(x => x.Ticks).ToList(); //Distinct - исключение повторяющихся дат/времени, OrderBy - сортировка дат/времени                    
                    DateTime DateLast = dateTimes.Last();

                    DateTime DateCounter = dateTimes.First();
                    for (DateTime datetime = lastInpuDataValTimeStamp.AddSeconds(timePeriodNormalize); datetime < DateLast; datetime = datetime.AddSeconds(timePeriodNormalize))   //Задание нормированного ряда времени               
                        foreach (var item in dateTimes)
                            if (item < datetime && item > datetime.AddSeconds(-timePeriodNormalize))
                            {
                                NumderDateTimes.Add(datetime);
                                break;
                            }
                }

                if (normalizeType == StructuringMode.continuousSlicesFromLastData)
                {
                    //Определение конечной даты выборки                   
                    DateTime DateLast = new DateTime();
                    List<DateTime> dateTimes0 = new List<DateTime>();
                    List<DateTime> dateTimes = new List<DateTime>();
                    foreach (KeyValuePair<int, List<DataVal>> keyValuePair in data)
                        for (int i = 0; i < keyValuePair.Value.Count; i++)
                            dateTimes0.Add(keyValuePair.Value[i].DateTime);
                    dateTimes = dateTimes0.Distinct().ToList().OrderBy(x => x.Ticks).ToList(); //Distinct - исключение повторяющихся дат/времени, OrderBy - сортировка дат/времени                    
                    DateLast = dateTimes.Last();

                    for (DateTime datetime = lastInpuDataValTimeStamp.AddSeconds(timePeriodNormalize); datetime < DateLast; datetime = datetime.AddSeconds(timePeriodNormalize))   //Задание нормированного ряда времени               
                        NumderDateTimes.Add(datetime);
                }

                if (normalizeType == StructuringMode.allData)
                {
                    List<DateTime> dateTimes0 = new List<DateTime>();
                    foreach (KeyValuePair<int, List<DataVal>> keyValuePair in data)
                        for (int i = 0; i < keyValuePair.Value.Count; i++)
                            dateTimes0.Add(keyValuePair.Value[i].DateTime);
                    NumderDateTimes = dateTimes0.Distinct().ToList().OrderBy(x => x.Ticks).ToList(); //Distinct - исключение повторяющихся дат/времени, OrderBy - сортировка дат/времени 
                }


                if (normalizeType == StructuringMode.continuousSlicesAllData)
                {
                    //Определение начальной и конечной даты выборки
                    DateTime DateFirst = new DateTime();
                    DateTime DateLast = new DateTime();
                    List<DateTime> dateTimes0 = new List<DateTime>();
                    List<DateTime> dateTimes = new List<DateTime>();
                    foreach (KeyValuePair<int, List<DataVal>> keyValuePair in data)
                        for (int i = 0; i < keyValuePair.Value.Count; i++)
                            dateTimes0.Add(keyValuePair.Value[i].DateTime);
                    dateTimes = dateTimes0.Distinct().ToList().OrderBy(x => x.Ticks).ToList(); //Distinct - исключение повторяющихся дат/времени, OrderBy - сортировка дат/времени
                    DateFirst = dateTimes.First();
                    DateLast = dateTimes.Last();

                    //  for (DateTime datetime = DateFirst.AddSeconds(timePeriodNormalize); datetime < DateLast; datetime = datetime.AddSeconds(timePeriodNormalize))   //Задание нормированного ряда времени (данная реализация не формирует ряд времени при чтении только одной точки данных т.к. DateFirst == DateLast)
                    for (DateTime datetime = DateFirst; datetime <= DateLast; datetime = datetime.AddSeconds(timePeriodNormalize))   //Задание нормированного ряда времени   
                        NumderDateTimes.Add(datetime);
                }

                //Нормализация данных по ряду времени с использований функций выбора                
                if (NumderDateTimes.Count != 0)
                    for (int i = 0; i < NumderDateTimes.Count; i++)   //Движение по ряду времени
                    {
                        foreach (var tagId in tagsId)  //Движение по перечню входных параметров
                        {
                            DataVal tagListItem = new DataVal();
                            tagListItem.DateTime = NumderDateTimes[i];
                            tagListItem.IsGood = false;
                            tagListItem.Val = 0;

                            bool IsFound = false;

                            //Поиcк совпадения точки нормированного ряда времени с точкой времени ряда данных
                            if (data.ContainsKey(tagId))
                            {
                                DataVal findItem = data[tagId].Find(x => Truncate(x.DateTime, TimeSpan.FromSeconds(1)) == Truncate(NumderDateTimes[i], TimeSpan.FromSeconds(1)));
                                if (findItem != null)
                                    if (findItem.DateTime != DateTime.MinValue) //т.е. точка найдена
                                    {
                                        tagListItem.IsGood = findItem.IsGood;
                                        tagListItem.Val = findItem.Val;
                                        IsFound = true;
                                    }
                                    else
                                    {
                                    }
                            }

                            //Поиск последней точки ряда данных внутри диапазоне времени среза
                            if (IsFound == false)
                                if (data.ContainsKey(tagId))
                                    if (i != 0)
                                    {
                                        List<DataVal> selectItems = new List<DataVal>();
                                        foreach (var item in data[tagId])
                                        {
                                            if (item.DateTime < NumderDateTimes[i])
                                                selectItems.Add(item);
                                            else break;
                                        }

                                        if (selectItems.Count() != 0)
                                        {
                                            DataVal selectItem = selectItems.Last();
                                            if (selectItem.DateTime != DateTime.MinValue)
                                            {
                                                tagListItem.IsGood = selectItem.IsGood;
                                                tagListItem.Val = selectItem.Val;
                                                IsFound = true;
                                            }
                                        }
                                    }

                            //  Для данного значения ряда времени в наборе данных точка не найдена -  берем точку, найденную на предыдущем значении ряда времени
                            if (IsFound == false)
                            {
                                if (lastNormalizeData.ContainsKey(tagId))
                                {
                                    tagListItem.IsGood = lastNormalizeData[tagId].IsGood;
                                    tagListItem.Val = lastNormalizeData[tagId].Val;
                                }
                            }

                            Result[tagId].Add(tagListItem);
                        }

                        //Обновляем последние найденные точки
                        //lastNormalizeData.Clear();
                        foreach (var item in Result)
                        {
                            if (!lastNormalizeData.ContainsKey(item.Key))
                                lastNormalizeData.Add(item.Key, new DataVal());

                            lastNormalizeData[item.Key] = item.Value.Last();
                        }
                    }

            }
            catch (Exception ex)
            {
               // AddLog(new MsgLogClass() { LogText = ListMsg.SystemError, LogDetails = ex.ToString() });
                return new Dictionary<int, List<DataVal>>();
            }

            return Result;
        }

        //Округление даты и времени
        private static DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        /// <summary>
        /// Удаление недостоверных значений переменных XY
        /// </summary>
        /// <param name="xData"></param>
        /// <param name="yData"></param>
        /// <returns></returns>
        public static bool RemoveFalseXY(ref SortedDictionary<int, List<DataVal>> xData, ref List<DataVal> yData)
        {
            if (xData.Count == 0)
            {                
                return false;
            }

            if (yData.Count == 0)
            {                
                return false;
            }

            try
            {
                for (int i = yData.Count - 1; i >= 0; i--)
                {
                    if (yData[i].IsGood == false)
                    {
                        yData.RemoveAt(i);
                        foreach (var item in xData)
                            item.Value.RemoveAt(i);
                    }
                }

                foreach (var item in xData)
                {
                    for (int i = item.Value.Count - 1; i >= 0; i--)
                    {
                        if (item.Value[i].IsGood == false)
                        {
                            foreach (var itemX in xData)
                                itemX.Value.RemoveAt(i);
                            yData.RemoveAt(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверка условия работы
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="sheetExpr"></param>
        /// <param name="expressionData"></param>
        /// <returns></returns>
        public static bool GenerateExpressionData(
            Dictionary<int, List<DataVal>> inputData, SheetExpr sheetExpr, out Dictionary<int, List<DataVal>> expressionData, out string err)
        {
            expressionData = new Dictionary<int, List<DataVal>>();

            if (inputData.Count == 0)
            {
                err = "Входные данные отсутствуют";
                return false;
            }

            if (inputData.First().Value.Count == 0)
            {
                err = "Входные данные отсутствуют";
                return false;
            }

            try
            {
                //Выборка переменных соответствующих условию регресии                
                foreach (var keyValue in inputData)
                    expressionData.Add(keyValue.Key, new List<DataVal>()); //создание ключей

                int firstKey =
                    inputData.Keys
                        .First(); //Определение первого ключа (используется для определения количества элементов в выборке) 
                for (int i = 0;
                     i < inputData[firstKey].Count;
                     i++) //Перебор всех значений  Х для включения в словарь тех столбцов (параметров с одной меткой времени) которые соответствуют логическому выражению (условию) 
                {
                    bool expParamsIsGood = true; //флаг, указывающий что значения всех параметров участвующих в условии достоверны 
                    NCalc.Expression expr = new NCalc.Expression(sheetExpr.exp); //Создание условия 
                    foreach (int item in sheetExpr.Param)
                    {
                        expr.Parameters[item.ToString()] = inputData[item][i].Val; //Заполнение значений параметров условий

                        if (inputData[item][i].IsGood == false) expParamsIsGood = false; //значение параметра участвующего в условии не достоверное 
                    }

                    if (expParamsIsGood) //проверка что значения всех параметров участвующих в условии достоверны
                        if ((bool)expr
                                .Evaluate()) //в словарь попадают только те параметры кот. соответствуют логическому выражению (условию) 
                            foreach (var item in inputData)
                            {
                                DataVal data = new DataVal();
                                data.Val = item.Value[i].Val;
                                data.DateTime = item.Value[i].DateTime;
                                data.IsGood = item.Value[i].IsGood;

                                expressionData[item.Key].Add(data);
                            }
                }                
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return false;
            }

            err = "";
            return true;
        }

        /// <summary>
        /// Расчет математического выражения
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="sheetExpr"></param>
        /// <param name="expressionData"></param>
        /// <returns></returns>
        public static bool ExpressionProcessing(
            Dictionary<int, List<DataVal>> inputData, int dataIndex, Dictionary<int, DataVal> localTagData, CalcExpr sheetExpr, out DataVal expressionData, out string err)
        {
            expressionData = new DataVal();
            err = "";

            if (inputData.Count == 0)
            {                
                return false;
            }

            if (inputData.First().Value.Count == 0)
            {                
                return false;
            }

            try
            {
                int firstKey = inputData.Keys.First(); //Определение первого ключа (используется для определения количества элементов в выборке) 


                bool expParamsIsGood = true; //флаг, указывающий что значения всех параметров участвующих в условии достоверны 
                NCalc.Expression expr = new NCalc.Expression(sheetExpr.exp); //Создание условия 

                //Заполнение значений глобальных параметров
                foreach (var item in sheetExpr.GlobalParam)
                {
                    expr.Parameters[item.ToString()] = inputData[item][dataIndex].Val;

                    if (inputData[item][dataIndex].IsGood == false) expParamsIsGood = false; //значение параметра участвующего в условии не достоверное 
                }

                //Заполнение значений локальных параметров
                foreach (var item in sheetExpr.LocalParam)
                {
                    if (localTagData.ContainsKey(item))
                        expr.Parameters[$"loc{item}"] = localTagData[item].Val;
                    else
                    {
                        expr.Parameters[$"loc{item}"] = 0;
                        expParamsIsGood = false; //значение параметра участвующего в условии не достоверное 
                    }
                }

                var resEvaluate = expr.Evaluate();
                
                DataVal data = new DataVal();
               /* if (resEvaluate is decimal)
                    data.Val = decimal.ToDouble((decimal)resEvaluate);
                if (resEvaluate is Int32)*/
                    data.Val = Convert.ToDouble(resEvaluate);

                //data.Val = (double)resEvaluate;

                data.DateTime = inputData[firstKey][dataIndex].DateTime;
                data.IsGood = expParamsIsGood;

                expressionData = data;

            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return false;
            }
            
            return true;
        }
    }
}
