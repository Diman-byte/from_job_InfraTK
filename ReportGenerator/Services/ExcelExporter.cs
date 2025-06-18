using Avalonia.Controls.Shapes;
using Microsoft.Office.Interop.Excel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Services
{
    public class ExcelExporter
    {
        // путь куда загрузим готовый отчет, содержащий название файла
        private string _pathReportData;

        // папка куда будем загружать
        private string _folderReportData;

        // путь для шаблона отчета c названием
        private string _pathTemplateData;

        // имя генерируемого отчета
        private string _nameReport;

        private XSSFWorkbook _workbook;

        public ExcelExporter(string pathTemplateData, string pathReportData, string nameReport)
        {
            _folderReportData = pathReportData;
            _pathTemplateData = pathTemplateData;
            _nameReport = nameReport;
        }

        private ISheet GetOrCreateSheet(string sheetName)
        {
            ISheet sheet = _workbook.GetSheet(sheetName);
            if (sheet == null)
                sheet = _workbook.CreateSheet(sheetName);
            return sheet;
        }


        public void ExportDataToExcel(
        List<TagValDto> tagData,
        List<TagDto> tags,
        string nameSheet,
        string format,
        bool showTagNames,
        bool showTagDescription,
        out string filePathReport)
        {
            GetFolderFiles();

            using var fsTemplate = new FileStream(_pathTemplateData, FileMode.Open, FileAccess.Read);

            _workbook = new XSSFWorkbook(fsTemplate);

            var _sheet = GetOrCreateSheet(nameSheet);

            var nameTags = GetNameTag(tags);
            var descTags = GetDescriptionTag(tags);
            var datesUniq = UniqDateTime(tagData);
            var tagsId = UniqTagsId(tagData);

            if(_sheet.LastRowNum > 0)
            {
                // Очистка старого содержимого листа
                for (int i = _sheet.LastRowNum; i >= 0; i--)
                    _sheet.RemoveRow(_sheet.GetRow(i));
            }
            

            int cnt_str = 1;

            if (showTagNames || showTagDescription)
            {
                IRow headerRow = _sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Дата и время");

                if (showTagNames && showTagDescription)
                {
                    _sheet.CreateRow(1).CreateCell(0).SetCellValue("Описание");
                    cnt_str += 2;
                }
                else
                {
                    cnt_str += 1;
                }
            }

            int rowCounter = cnt_str;

            foreach (var date in datesUniq)
            {
                IRow row = _sheet.GetRow(rowCounter) ?? _sheet.CreateRow(rowCounter);
                row.CreateCell(0).SetCellValue(date.ToString(format));
                rowCounter++;
            }

            int columnIndex = 1;

            foreach (var tagId in tagsId)
            {
                int rowIndex = 0;

                if (showTagNames)
                {
                    IRow row = _sheet.GetRow(rowIndex);
                    if (row == null)
                    {
                        row = _sheet.CreateRow(rowIndex);
                    }

                    _sheet.GetRow(rowIndex).CreateCell(columnIndex).SetCellValue(nameTags[tagId]);
                    rowIndex++;
                }

                if (showTagDescription)
                {
                    IRow row = _sheet.GetRow(rowIndex);
                    if (row == null)
                    {
                        row = _sheet.CreateRow(rowIndex);
                    }
                    _sheet.GetRow(rowIndex).CreateCell(columnIndex).SetCellValue(descTags[tagId]);
                    rowIndex++;
                }

                for (int i = 0; i < datesUniq.Count; i++)
                {
                    DateTime date = datesUniq[i];
                    double value;

                    IRow row = _sheet.GetRow(i + cnt_str) ?? _sheet.CreateRow(i + cnt_str);
                    ICell cell = row.CreateCell(columnIndex);

                    if (GetValTagFromDateAndId(tagData, tagId, date, out value))
                    {
                        cell.SetCellValue(value);
                    }
                    else
                    {
                        // Установка красного фона
                        var style = _workbook.CreateCellStyle();
                        style.FillForegroundColor = IndexedColors.Red.Index;
                        style.FillPattern = FillPattern.SolidForeground;
                        cell.CellStyle = style;
                    }
                }

                columnIndex++;
            }

            // Автоширина столбцов
            for (int col = 0; col < columnIndex; col++)
            {
                _sheet.AutoSizeColumn(col);
            }

            using var fsOut = new FileStream(_pathReportData, FileMode.Create, FileAccess.Write);
            _workbook.Write(fsOut);

            filePathReport = _pathReportData;
        }

        /// <summary>
        /// получаем все пути для файлов
        /// </summary>
        private void GetFolderFiles()
        {
            //string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            //string folderTemplates = System.IO.Path.Combine(baseFolder, "Assets\\TemplatesExcel");

            //string reports = System.IO.Path.Combine(baseFolder, "Reports");
            //if (!Directory.Exists(reports))
            //{
            //    Directory.CreateDirectory(reports);
            //}

            //string nameTemplateData = "ExampleReport";

            
            string nameReport = _nameReport + "_" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".xlsx";
            _pathReportData = System.IO.Path.Combine(_folderReportData, nameReport);
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
        private bool GetValTagFromDateAndId(List<TagValDto> tagData, Guid idTag, DateTime date, out double value)
        {
            foreach (TagValDto tagVal in tagData)
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

            foreach (TagDto tag in tags)
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
