using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using ExcelReportingAddin.DTOs;
using Microsoft.Office.Interop.Excel;

namespace ExcelReportingAddin
{
    // класс управляет жизненным циклом надстройки и позволяет получить доступ к
    // активной книге Excel, в которой надстройка запущена
    public partial class ThisAddIn
    {

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region Код, автоматически созданный VSTO

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        /// <summary>
        /// Создать или вернуть, если существует, лист.
        /// </summary>
        /// <param name="sheetname">имя листа</param>
        /// <returns>Объект класса Excel.Worksheet (лист)</returns>
        private Excel.Worksheet GetOrCreateWorksheet(string sheetname)
        {
            foreach (Excel.Worksheet worksheet in this.Application.Worksheets)
            {
                if (worksheet.Name == sheetname)
                {
                    return worksheet;
                }
            }

            Excel.Worksheet newSheet = this.Application.Worksheets.Add();
            newSheet.Name = sheetname;
            return newSheet;
        }

        /// <summary>
        /// Выводит исторические данные в Excel
        /// </summary>
        /// <param name="tagData">список TagValDto со значениями тегов</param>
        /// <param name="tags">список TagDto, откуда возьмем имена тегов</param>
        /// <param name="nameSheet">имя листа, куда будем выводить данные</param>
        /// <param name="format">формат даты, который выводиться в таблицу в первый столбец</param>
        /// <param name="showTagNames">булево значение, выводить ли имена тегов в таблицу</param>
        /// <param name="showTagDescription">булево значение, выводить ли описание тегов в таблицу</param>
        public void ExportDataToExcel(List<TagValDto> tagData, List<TagDto> tags, string nameSheet, string format, bool showTagNames, bool showTagDescription)
        {
            Excel.Worksheet dataSheet = GetOrCreateWorksheet(nameSheet); // Проверяет, есть ли лист с именем "Data". Если нет, создаёт новый лист с этим именем.

            // Очистка старых данных
            dataSheet.Cells.Clear();

            // Лист Excel может вместить 1 048 576 строк и 16 384 столбца

            int cnt_str = 1; // счетчик строк для функционала показа тегов и описания тегов

            // показать только имена тегов
            if(showTagNames && !showTagDescription)
            {
                dataSheet.Cells[1, 1].Value = "Дата и время";
                cnt_str = cnt_str + 1;
            }
            // показать только описание тегов
            else if (!showTagNames && showTagDescription)
            {
                dataSheet.Cells[1, 1].Value = "Дата и время";
                cnt_str = cnt_str + 1;
            }
            // показать имена тегов и их описание
            else if (showTagNames && showTagDescription)
            {
                dataSheet.Cells[1, 1].Value = "Дата и время";              
                cnt_str = cnt_str + 2; // увеличиваем на 2 строки, тк на второй строке будет описание тегов
            }

            // заполняю столбец времени
            List<DateTime> datesUniq = UniqDateTime(tagData);
            int cnt_1 = cnt_str; // будет показывать количество строк
            foreach (var item in datesUniq)
            {                
                dataSheet.Cells[cnt_1, 1].Value = item.ToString(format);
                cnt_1++;
            }

            // это нужно для того, чтобы подтянуть имя к тегам
            Dictionary<Guid, string> nameTags = GetNameTag(tags);
            // это нужно для того, чтобы подтянуть описанание к тегам
            Dictionary<Guid, string> descTags = GetDescriptionTag(tags);

            // заполняю столбцы дальше, вывожу значения тегов
            int j = 2; // начинаю отчет со второго слолбца
            List<Guid> tagsId = UniqTagsId(tagData);
            foreach (var item in tagsId)
            {
                int i = 1;

                // показать только имена тегов
                if (showTagNames && !showTagDescription) {
                    dataSheet.Cells[1, j].Value = nameTags[item];
                    i = i + 1;
                }
                // показать только описания тегов
                if (!showTagNames && showTagDescription)
                {
                    dataSheet.Cells[1, j].Value = descTags[item];
                    i = i + 1;
                }
                // показать имена тегов и их описание
                else if (showTagNames && showTagDescription)
                {
                    dataSheet.Cells[1, j].Value = nameTags[item];
                    dataSheet.Cells[2, j].Value = descTags[item];
                    i = i + 2;
                }

                while (i < cnt_1) // нужно заполнить все строки с временем
                {
                    // нахожу время в текущей строке
                    //Range cell_data = dataSheet.Cells[i, 1]; - старый метод
                    //object cellValueData = cell_data.Value;
                    //string data_str = cellValueData.ToString();
                    //DateTime date = DateTime.Parse(data_str);
                    DateTime date = datesUniq[i - cnt_str]; // находим значение времени для этой строчки из списка дат datesUniq, индекс = номер строки - сдвиг

                    // достаю значение тега по временной метке
                    double value;
                    if (GetValTagFromDateAndId(tagData, item, date, out value)) // если есть значение по этой временной метке, записываем это в ячейку
                    {
                        dataSheet.Cells[i, j].Value = value;
                    }
                    else
                    {
                        // если значения нет
                        // Устанавливаем цвет фона ячейки (ставим красный)
                        Range cell_without_value = dataSheet.Cells[i, j];
                        cell_without_value.Interior.Color = System.Drawing.Color.Red; 
                    }

                    i++;
                }
                j++;
            }

            // Настройка ширины столбцов
            dataSheet.Columns.AutoFit();

        }

        /// <summary>
        /// Получаем список с отсортированными уникальными значениями времени
        /// </summary>
        /// <param name="tagData">список TagValDto со значениями тегов</param>
        /// <returns> Список с отсортированными уникальными значениями времени </returns>
        private List<DateTime> UniqDateTime(List<TagValDto> tagData)
        {
            List<DateTime> result = new List<DateTime>();

            foreach (TagValDto tagVal in tagData)
            {
                if (!result.Contains(tagVal.Timestamp))
                {
                    result.Add(tagVal.Timestamp);
                }
            }

            result.Sort();
            return result;
        }

        /// <summary>
        /// Получаем список с уникальными id тегов
        /// </summary>
        /// <param name="tagData">список TagValDto со значениями тегов</param>
        /// <returns> Список с уникальными id тегов </returns>
        private List<Guid> UniqTagsId(List<TagValDto> tagData)
        {
            List<Guid> result = new List<Guid>();

            foreach (var tagVal in tagData)
            {
                if (!result.Contains(tagVal.TagId))
                {
                    result.Add(tagVal.TagId);
                }
            }

            return result;
        }

        /// <summary>
        /// Получаем значение тега по его временной метке и его id
        /// </summary>
        /// <returns> Значение тега и наличие/отсутствие значения тега по временной метке </returns>
        private bool GetValTagFromDateAndId(List<TagValDto> tagData, Guid idTag, DateTime date, out double value) { 
            foreach(TagValDto tagVal in tagData)
            {
                if (tagVal.TagId == idTag && tagVal.Timestamp == date)
                {
                    value = tagVal.Value;
                    return true;
                }
            }

            value = 0;
            return false;
        }

        /// <summary>
        /// Получаем словарь с парами "Id тега" и "его имя name"
        /// </summary>
        /// <param name="tags">список TagVal с тегами</param>
        /// <returns> Словарь с парами ключ "Id тега" значение "его имя name" </returns>
        private Dictionary<Guid, string> GetNameTag(List<TagDto> tags)
        {
            Dictionary<Guid, string> result = new Dictionary<Guid, string>();

            foreach (var tag in tags)
            {
                result.Add(tag.Id, tag.Name);
            }

            return result;
        }

        private Dictionary<Guid, string> GetDescriptionTag(List<TagDto> tags)
        {
            Dictionary<Guid, string> result = new Dictionary<Guid, string>();

            foreach(TagDto tag in tags)
            {
                if (!result.ContainsKey(tag.Id))
                {
                    result.Add(tag.Id, tag.Description);
                }
            }

            return result;
        }

    }
}
