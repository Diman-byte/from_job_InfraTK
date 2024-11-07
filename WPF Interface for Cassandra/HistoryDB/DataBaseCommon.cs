using Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HistoryDB
{
    /// <summary>
    /// Класс общих алгоритмов обработки данных
    /// </summary>
    public sealed class DataBaseCommon
    {
        private readonly GetTagReaderMethod _getTagReaderMethod;

        /// <summary>
        /// Делегат получения ридера отсортированных данных для тэга
        /// </summary>
        /// <param name="tagId">id тэга для которого открывается reader</param>
        /// <param name="sortOrder">ASC - по возрастанию, DESC - по убыванию</param>
        public delegate DbDataReader GetTagReaderMethod(int tagId, string sortOrder);


        /// <summary>
        /// Конструктор
        /// </summary>
        public DataBaseCommon(GetTagReaderMethod getTagReaderMethod)
        {
            _getTagReaderMethod = getTagReaderMethod;
        }

        public Dictionary<int, List<DataVal>> ReadSliceTagData(int nodeId, DateTime dateTimeFrom, DateTime dateTimeTo, List<int> idTags, int pointsAmount, AlgSliceEnum algSlice)
        {
            var interval = dateTimeTo - dateTimeFrom;
            ReadSliceTagDataMethod readSliceTagData = algSlice switch
            {
                AlgSliceEnum.FirstPoint => SliceDataAlgorithmFirst,
                AlgSliceEnum.LastPoint => SliceDataAlgorithmLast,
                AlgSliceEnum.MinMaxPoints => SliceDataAlgorithmMinMax,
                _ => throw new ArgumentOutOfRangeException(nameof(algSlice), algSlice, null)
            };
            return readSliceTagData(nodeId, idTags, interval, dateTimeFrom, dateTimeTo, pointsAmount);
        }

        private Dictionary<int, List<DataVal>> SliceDataAlgorithmFirst(int nodeId, IReadOnlyList<int> idTags, TimeSpan interval, DateTime dateTimeFrom, DateTime dateTimeTo, int pointsAmount)
        {
            var secondsInterval = interval.TotalSeconds / pointsAmount;
            secondsInterval = Math.Ceiling(secondsInterval);//округляем минимум до 1 секунды

            var result = new Dictionary<int, List<DataVal>>(idTags.Count);
            var subRangeStart = dateTimeFrom;
            var subRangeEnd = subRangeStart.AddSeconds(secondsInterval);
            foreach (var tagId in idTags)
            {
                var tagSliceData = new List<DataVal>(pointsAmount);
                using var reader = _getTagReaderMethod(tagId, "ASC");
                ReadTagSubRangesSliceData(reader, subRangeStart, subRangeEnd, secondsInterval, ref tagSliceData);
                result.Add(tagId, tagSliceData);
            }
            return result;
        }

        private Dictionary<int, List<DataVal>> SliceDataAlgorithmLast(int nodeId, IReadOnlyList<int> idTags, TimeSpan interval, DateTime dateTimeFrom, DateTime dateTimeTo, int pointsAmount)
        {
            var secondsInterval = interval.TotalSeconds / pointsAmount;
            secondsInterval = Math.Ceiling(secondsInterval);//округляем минимум до 1 секунды
            secondsInterval *= -1;

            var result = new Dictionary<int, List<DataVal>>(idTags.Count);
            var subRangeStart = dateTimeTo;
            var subRangeEnd = subRangeStart.AddSeconds(secondsInterval);
            foreach (var tagId in idTags)
            {
                var tagSliceData = new List<DataVal>(pointsAmount);
                using var reader = _getTagReaderMethod(tagId, "DESC");
                ReadTagSubRangesSliceData(reader, subRangeStart, subRangeEnd, secondsInterval, ref tagSliceData);
                result.Add(tagId, tagSliceData);
            }
            return result;
        }

        private Dictionary<int, List<DataVal>> SliceDataAlgorithmMinMax(int nodeId, IReadOnlyList<int> idTags, TimeSpan interval, DateTime dateTimeFrom, DateTime dateTimeTo, int pointsAmount)
        {
            void AddMaxMinItems(List<DataVal> rangeItemsBuffer, List<DataVal> tagSliceData)
            {
                if (rangeItemsBuffer.Count == 0) return;

                rangeItemsBuffer.Sort((x, y) => x.Val.CompareTo(y.Val));

                var min = rangeItemsBuffer[0];
                var max = rangeItemsBuffer[rangeItemsBuffer.Count - 1];
                var results = new[]
                {
                    min, max
                }.Distinct().OrderBy(t => t.DateTime);

                tagSliceData.AddRange(results);
                rangeItemsBuffer.Clear();
            }

            var halfPoints = (double)pointsAmount / 2; // делим на два так как берем на поддиапазоне по две точки
            var secondsInterval = interval.TotalSeconds / halfPoints;
            secondsInterval = Math.Ceiling(secondsInterval);//округляем минимум до 1 секунды

            var result = new Dictionary<int, List<DataVal>>(idTags.Count);
            foreach (var tagId in idTags)
            {
                var tagSliceData = new List<DataVal>(pointsAmount);
                var rangeItemsBuffer = new List<DataVal>();
                using var reader = _getTagReaderMethod(tagId, "ASC");
                if (!reader.Read()) continue;

                var subRangeStart = DateTime.Parse(reader["DateTime"].ToString());
                var subRangeEnd = subRangeStart.AddSeconds(secondsInterval);
                do
                {
                    var readDateTime = DateTime.Parse(reader["DateTime"].ToString());

                    if (!IsDateTimeBetween(subRangeStart, subRangeEnd, readDateTime))
                    {
                        //передвинем поддиапазон на метку времени пришедших данных
                        while (!IsDateTimeBetween(subRangeStart, subRangeEnd, readDateTime))
                        {
                            subRangeStart = subRangeEnd;
                            subRangeEnd = subRangeEnd.AddSeconds(secondsInterval);
                        }

                        //высчитаем min max в поддиапазоне
                        AddMaxMinItems(rangeItemsBuffer, tagSliceData);
                    }

                    /*var strValue = reader["Val"].ToString();
                    var isGood = float.TryParse(strValue as string, out var parsedFloat);
                    var value = isGood ? parsedFloat : 0;

                    var tag = new DataVal
                    {
                        DateTime = readDateTime,
                        IsGood = isGood,
                        Val = value
                    };
                    rangeItemsBuffer.Add(tag);*/


                    object strValue = reader["Val"];
                    if (strValue != null)
                    {
                        var isGood = float.TryParse(strValue.ToString(), out var parsedFloat);
                        var value = isGood ? parsedFloat : 0;

                        var tag = new DataVal
                        {
                            DateTime = readDateTime,
                            IsGood = isGood,
                            Val = value
                        };
                        rangeItemsBuffer.Add(tag);
                    }
                }
                while (reader.Read());

                //добавим последний диапазон
                AddMaxMinItems(rangeItemsBuffer, tagSliceData);

                result.Add(tagId, tagSliceData);
            }

            return result;
        }


        /// <summary>
        /// Читает срез данных тэга
        /// </summary>
        /// <param name="reader">считыватель записей тэга</param>
        /// <param name="subRangeStart">метка времени начала поддиапазона</param>
        /// <param name="subRangeEnd">метка времени конца поддиапазона</param>
        /// <param name="secondsInterval">интервал поддиапазона в секундах</param>
        /// <param name="tagSliceData">срез данных для тэга</param>
        private void ReadTagSubRangesSliceData(
            DbDataReader reader, DateTime subRangeStart, DateTime subRangeEnd, double secondsInterval, ref List<DataVal> tagSliceData)
        {
            if (!reader.Read()) return;

            var startDataDateTime = DateTime.Parse(reader["DateTime"].ToString());
            //передвинем поддиапазон на метку времени пришедших данных
            while (!IsDateTimeBetween(subRangeStart, subRangeEnd, startDataDateTime))
            {
                subRangeStart = subRangeEnd;
                subRangeEnd = subRangeEnd.AddSeconds(secondsInterval);
            }

            do
            {
                var readDateTime = DateTime.Parse(reader["DateTime"].ToString());

                //если пришли данные не в нашем поддиапазоне
                if (!IsDateTimeBetween(subRangeStart, subRangeEnd, readDateTime)) continue;

                //берем только первое значение из поддиапазона
                var strValue = reader["Val"].ToString();
                var isGood = float.TryParse(strValue, out var parsedFloat);
                var value = isGood ? parsedFloat : 0;

                tagSliceData.Add(new DataVal
                {
                    DateTime = readDateTime,
                    IsGood = isGood,
                    Val = value
                });

                subRangeStart = subRangeEnd;
                subRangeEnd = subRangeEnd.AddSeconds(secondsInterval);
            }
            while (reader.Read());
        }

        /// <summary>
        /// Проверяет находится ли метка времени в диапазоне между двумя другими метками
        /// </summary>
        /// <param name="dateTimeOne">метка времени 1</param>
        /// <param name="dateTineTwo"> метка времени 2</param>
        /// <param name="checkDateTime">метка времни для проверки на принадлежность диапазону</param>
        /// <returns>true - входит в диапазон,false - не входит</returns>
        private bool IsDateTimeBetween(DateTime dateTimeOne, DateTime dateTineTwo, DateTime checkDateTime)
        {
            DateTime max;
            DateTime min;
            if (dateTimeOne > dateTineTwo)
            {
                max = dateTimeOne;
                min = dateTineTwo;
            }
            else
            {
                min = dateTimeOne;
                max = dateTineTwo;
            }
            return checkDateTime < max && checkDateTime >= min;
        }

        private delegate Dictionary<int, List<DataVal>> ReadSliceTagDataMethod(int nodeId, List<int> idTags, TimeSpan interval, DateTime dateTimeFrom, DateTime dateTimeTo, int pointsAmount);
    }
}
