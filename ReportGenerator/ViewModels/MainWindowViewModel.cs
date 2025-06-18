using ReactiveUI;
using ReportGenerator.Services;
using System;
using System.IO;
using System.Reactive;
using Excel = Microsoft.Office.Interop.Excel;
using ReportGenerator.Models;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Linq;
using ReportGenerator.ViewModels.UserControls;
using System.Threading;
using System.Threading.Tasks;



namespace ReportGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> GenerateCommand { get; }

        private ConfigReadData _configReadData = new ConfigReadData();
        private DataServerClient _dataServerClient;

        public SettingsUserControlViewModel SettingsVM { get; }
        public ReportsUserControlViewModel ReportsVM { get;}
        public SendingReportsUserControlViewModel SendingVM { get; }

        //private Timer? _schduleTimer;
        public ReactiveCommand <Unit,Unit> ScheduleGenerateCommand { get; }

        public ReactiveCommand<Unit, Unit> SaveCommand {  get; }


        public MainWindowViewModel() {
            
            SettingsVM = new SettingsUserControlViewModel();
            ReportsVM = new ReportsUserControlViewModel();
            SendingVM = new SendingReportsUserControlViewModel();

            ScheduleGenerateCommand = ReactiveCommand.CreateFromTask(ScheduleGenerateData);
            SaveCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                // пробрасываем вызов метода в ReportsUserControlViewModel
                await ReportsVM.SaveConfig();
                await SettingsVM.CheckConnectionAndSave();
                await SendingVM.SaveConfigMail();
                await ScheduleGenerateData();
            });


        }


        /// <summary>
        /// метод генерации отчета данных
        /// </summary>
        private async void GenerateData(Reports report)
        {
            
            // работа с конфигом данных
            ConfigService configService = new ConfigService(report.TemplateFilePath);
            if(!configService.LoadConfigData())
            {
                return;
            }
            _configReadData = configService.configReadData;

            // работа с конфигом настроек
            //SettingsVM = new SettingsUserControlViewModel();

            _dataServerClient = new DataServerClient(SettingsVM.DataServerAddress, SettingsVM.DataServerPort);

            DateTime startDate = StartDate();
            DateTime endDate = EndDate();

            List<TagValDto> list_tags_val = null;

            if (_configReadData.ParametrGettingHistoricalData == "AllValues")
            {
                // если выбрана кнопка "Все значения"
                try
                {
                    list_tags_val = await _dataServerClient.GetTagsHistoricalData(_configReadData.SelectedAsset, _configReadData.SelectedTagIds, startDate, endDate);
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowError("Ошибка", "Произошла ошибка в работе Rest метода по получению исторических данных" +
                        "\nПроверьте настройки DataServer" +
                        $"\n{ex.Message}");
                    return;
                }
            }
            if(_configReadData.ParametrGettingHistoricalData == "Slices")
            {
                // если выбрана кнопка "Срезы"
                try
                {
                    list_tags_val = await _dataServerClient.GetTagsHistoricalSliceData(_configReadData.SelectedAsset, _configReadData.SelectedTagIds,
                    startDate, endDate, _configReadData.PointsAmount, _configReadData.SliceType);
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowError("Ошибка", "Произошла ошибка в работе Rest метода по получению исторических данных в срезах" +
                        "\nПроверьте настройки DataServer" +
                        $"\n{ex.Message}");
                    return;
                }
            }

            // это нужно для того, чтобы подтянуть имя к тегам
            var listTags = await _dataServerClient.GetTagsByAssetId(_configReadData.SelectedAsset);

            string filePathReport = "";

            if (list_tags_val != null && list_tags_val.Any())
            {
                try
                {
                    ExcelExporter excelReporter = new ExcelExporter(report.TemplateFilePath, report.ReportFolderPath, report.NameReport);
                    excelReporter.ExportDataToExcel(list_tags_val, listTags, _configReadData.TabFormat.NameSheet, _configReadData.TabFormat.Format,
                        _configReadData.TabFormat.IsDisplayTagNames, _configReadData.TabFormat.IsDisplayTagDescription, out filePathReport);
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowError("Ошибка", $"Ошибка в экспорте в Excel: {ex.Message}");
                    return;
                }
            }
            else
            {
                MessageBoxService.ShowError("Ошибка в загрузке данных", "Ошибка загрузки исторических данных (возможно теги за выбранный интервал отсутствуют)");
                return;
            }

            //вызов функции отправки по почте
            SendEmail(report, filePathReport);       
        }


        #region работа с началом и концом интервала

        /// <summary>
        /// Метод получения получения даты начала диапазона
        /// </summary>
        /// <returns>Объект времени начала диапазона</returns>
        private DateTime StartDate()
        {
            // сейчас без проверок на ввод

            // асболютный режим ввода даты
            if (_configReadData.TimeSettingsStart.Mode == "Absolute")
            {
                DateTime dateTime = (DateTime)_configReadData.TimeSettingsStart.AbsolutTime;
                return dateTime;
            }
            // относительный
            else
            {
                int day = _configReadData.TimeSettingsStart.RelativeOffset.Days;
                int hour = _configReadData.TimeSettingsStart.RelativeOffset.Hours;
                int minute = _configReadData.TimeSettingsStart.RelativeOffset.Minutes;
                int second = _configReadData.TimeSettingsStart.RelativeOffset.Seconds;

                DateTime now = DateTime.Now;
                DateTime res = DateTime.Now;

                // если "Сейчас"
                if (_configReadData.TimeSettingsStart.Sign == "Сейчас")
                {
                    res = now;
                }

                // если "Текущая смена"
                if (_configReadData.TimeSettingsStart.Sign == "Текущая смена")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        res = DateTime.Today.AddHours(8);
                    }
                    else
                    {
                        // ночная смена
                        res = DateTime.Today.AddHours(19);
                    }
                }

                // если "Смена плюс"
                if (_configReadData.TimeSettingsStart.Sign == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(8);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                }

                // если "Смена минус"
                if (_configReadData.TimeSettingsStart.Sign == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(8);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                }

                // если "Сейчас плюс"
                if (_configReadData.TimeSettingsStart.Sign == "Сейчас плюс")
                {
                    res = now.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сейчас минус"
                if (_configReadData.TimeSettingsStart.Sign == "Сейчас минус")
                {
                    res = now.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }
                // если "Сегодня плюс"
                if (_configReadData.TimeSettingsStart.Sign == "Сегодня плюс")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня минус"
                if (_configReadData.TimeSettingsStart.Sign == "Сегодня минус")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }

                return res;
            }
        }


        /// <summary>
        /// Метод получения получения даты конца диапазона
        /// </summary>
        /// <returns> Объект времени конца диапазона </returns>
        private DateTime EndDate()
        {
            if (_configReadData.TimeSettingsEnd.Mode == "Absolute")
            {
                DateTime dateTime = (DateTime)_configReadData.TimeSettingsEnd.AbsolutTime;
                return dateTime;
            }
            // относительный
            else
            {
                int day = _configReadData.TimeSettingsEnd.RelativeOffset.Days;
                int hour = _configReadData.TimeSettingsEnd.RelativeOffset.Hours;
                int minute = _configReadData.TimeSettingsEnd.RelativeOffset.Minutes;
                int second = _configReadData.TimeSettingsEnd.RelativeOffset.Seconds;

                DateTime now = DateTime.Now;
                DateTime res = DateTime.Now;

                // если "Сейчас"
                if (_configReadData.TimeSettingsEnd.Sign == "Сейчас")
                {
                    res = now;
                }

                // если "Текущая смена"
                if (_configReadData.TimeSettingsEnd.Sign == "Текущая смена")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        res = DateTime.Today.AddHours(19);
                    }
                    else
                    {
                        // ночная смена
                        res = DateTime.Today.AddDays(1).AddHours(8);
                    }
                }

                // если "Смена плюс"
                if (_configReadData.TimeSettingsEnd.Sign == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddDays(1).AddHours(8);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                }

                // если "Смена минус"
                if (_configReadData.TimeSettingsEnd.Sign  == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddDays(1).AddHours(8);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                }

                // если "Сейчас минус"
                if (_configReadData.TimeSettingsEnd.Sign == "Сейчас минус")
                {
                    res = now.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }
                // если "Сейчас плюс"
                if (_configReadData.TimeSettingsEnd.Sign == "Сейчас плюс")
                {
                    res = now.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня плюс"
                if (_configReadData.TimeSettingsEnd.Sign == "Сегодня плюс")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня минус"
                if (_configReadData.TimeSettingsEnd.Sign == "Сегодня минус")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }

                return res;
            }
        }

        #endregion

        #region расписание

        private async Task ScheduleGenerateData()
        {
            foreach(var report in ReportsVM.ReportsList)
            {
                switch(report.ScheduleParam.TypeSchedule)
                {
                    case "Каждый день":
                        ScheduleGenerateEveryDay(report);
                        break;
                    case "Каждый месяц":
                        ScheduleGenerateEveryMonth(report);
                        break;
                    case "Каждый год":
                        ScheduleGenerateEveryYear(report);
                        break;   
                    case "На календарный конец месяца":
                        ScheduleGenerateEndMonth(report);
                        break;
                }
            }

        }

        /// <summary>
        /// если генерация "каждый день"
        /// </summary>
        private void ScheduleGenerateEveryDay(Reports report)
        {
            var r = report.ScheduleParam;
            // когда хотим запускать задачу
            TimeSpan targetTime = new TimeSpan(int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds)); // задаем время суток, с которого начнем отсчет

            DateTime now = DateTime.Now;
            DateTime nextRunTime = DateTime.Today.Add(targetTime);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // следующий запуск — завтра
            }

            TimeSpan initialDelay = nextRunTime - now;
            TimeSpan interval = TimeSpan.FromDays(1);

            Timer schduleTimer = new Timer(async _ =>
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GenerateData(report));
            }, null, initialDelay, interval);
            // запускает заданный метод (делегат) по расписанию
            // вызывает метод GenerateData через UI-диспетчер Avalonia
            // Объект состояния (здесь null)
            // initialDelay — сколько ждать перед первым запуском
            // interval — через какой промежуток запускать повторно
        }

        /// <summary>
        /// если генерация "каждый месяц"
        /// </summary>
        /// <param name="report"></param>
        private void ScheduleGenerateEveryMonth(Reports report)
        {
            var r = report.ScheduleParam;

            if (int.Parse(r.Day) == 0)
            {
                MessageBoxService.ShowInfo("Уведомление", $"День не может быть равен 0 при генерации каждый месяц в отчете {report.NameReport}");
                return;
            }

            DateTime now = DateTime.Now;
            DateTime nextRunTime;
            try
            {
                nextRunTime = new DateTime(now.Year, now.Month, int.Parse(r.Day), int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }
            catch
            {
                // Если, например, 31 февраля — берем последнее число текущего месяца
                int lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
                nextRunTime = new DateTime(now.Year, now.Month, lastDayOfMonth, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }

            if (now > nextRunTime)
            {
                // Переход к следующему месяцу
                var nextMonth = now.AddMonths(1);
                int nextDay = int.Parse(r.Day);

                // если день больше чем существующий в след месяце
                if(nextDay > DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month))
                {
                    nextDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                }

                nextRunTime = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }

            TimeSpan initialDelay = nextRunTime - now;

            Timer schduleTimer = null;

            schduleTimer = new Timer(async _ =>
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GenerateData(report));

                // Перенастраиваем таймер на следующий месяц
                var nextMonth = DateTime.Now.AddMonths(1);
                int nextDay = int.Parse(r.Day);

                if (nextDay > DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month))
                {
                    nextDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                }

                DateTime nextRun = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
                TimeSpan delay = nextRun - DateTime.Now;

                // для ручного перенастроя таймера после каждого запуска, потому что System.Threading.Timer не поддерживает переменные интервалы напрямую
                schduleTimer?.Change(delay, Timeout.InfiniteTimeSpan);
            },
            null,
            initialDelay,
            Timeout.InfiniteTimeSpan);
            // Timeout.InfiniteTimeSpan используется для отключения повторного автоматического запуска таймера, чтобы мы могли вручную перенастроить его на следующую нужную дату.

            // перенастраиваем таймер вручную после каждого срабатывания, чтобы не было накопленной ошибки
            // Если день (например, 31) недопустим в каком-либо месяце — ставим последний возможный день
        }

        /// <summary>
        /// генерация "Каждый год"
        /// </summary>
        /// <param name="report"></param>
        //private void ScheduleGenerateEveryYear(Reports report)
        //{
        //    var r = report.ScheduleParam;

        //    if (int.Parse(r.Day) == 0 || int.Parse(r.Month) == 0)
        //    {
        //        MessageBoxService.ShowInfo("Уведомление", $"День или месяц не может быть равен 0 при генерации каждый год в отчете {report.NameReport}");
        //        return;
        //    }

        //    DateTime now = DateTime.Now;
        //    DateTime nextRunTime;
        //    try
        //    {
        //        nextRunTime = new DateTime(now.Year, int.Parse(r.Month), int.Parse(r.Day), int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
        //    }
        //    catch
        //    {
        //        // Если, например, ввели 31 февраля — берем последнее число текущего месяца
        //        int Day = DateTime.DaysInMonth(now.Year, now.Month);
        //        nextRunTime = new DateTime(now.Year, int.Parse(r.Month), Day, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
        //    }

        //    if(now > nextRunTime)
        //    {
        //        DateTime nextYear = nextRunTime.AddYears(1);
        //        int nextDay = int.Parse(r.Day);

        //        // вдруг год был високосным и месяц февраль, то в нем уже будет 28 дней, а не 29
        //        if (nextDay >  DateTime.DaysInMonth(nextYear.Year, nextYear.Month))
        //        {
        //            nextDay = DateTime.DaysInMonth(nextYear.Year, nextYear.Month);
        //        }

        //        nextRunTime = new DateTime(now.Year, int.Parse(r.Month), nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
        //    }

        //    TimeSpan initialDelay = nextRunTime - now;

        //    Timer scheduleTimer = null;

        //    scheduleTimer = new Timer(async _ =>
        //    {
        //        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GenerateData(report));

        //        // Перенастраиваем таймер на следующий месяц
        //        DateTime nextYear = DateTime.Now.AddYears(1);
        //        int day = int.Parse(r.Day);

        //        if (day > DateTime.DaysInMonth(nextYear.Year, nextYear.Month)) {
        //            day = DateTime.DaysInMonth(nextYear.Year, nextYear.Month);
        //        }

        //        DateTime nextRun = new DateTime(nextYear.Year, nextYear.Month, day, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
        //        TimeSpan delay = nextRun - now;

        //        scheduleTimer?.Change(delay, Timeout.InfiniteTimeSpan);
        //    },
        //    null,
        //    initialDelay,
        //    Timeout.InfiniteTimeSpan);
        //}


        /// <summary>
        /// Генерация отчета "Каждый год" с обходом ограничения Timer на максимальную задержку
        /// </summary>
        private void ScheduleGenerateEveryYear(Reports report)
        {
            // по-хорошему нужно использовать библиотеку Hangfire или Quartz.NET, которые поддерживают длительные интервалы и критические случаи (например, 29 февраля)
            var r = report.ScheduleParam;

            if (int.Parse(r.Day) == 0 || int.Parse(r.Month) == 0)
            {
                MessageBoxService.ShowInfo("Уведомление", $"День и месяц не могут быть равны 0 при генерации каждый год в отчете {report.NameReport}");
                return;
            }

            DateTime now = DateTime.Now;
            DateTime nextRunTime;
            try
            {
                nextRunTime = new DateTime(now.Year, int.Parse(r.Month), int.Parse(r.Day),
                               int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }
            catch
            {
                int lastDayOfMonth = DateTime.DaysInMonth(now.Year, int.Parse(r.Month));
                nextRunTime = new DateTime(now.Year, int.Parse(r.Month), lastDayOfMonth,
                               int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }

            // Если выбранная дата уже прошла — планируем на следующий год
            if (now > nextRunTime)
            {
                int nextYear = now.Year + 1;
                int day = int.Parse(r.Day);
                int month = int.Parse(r.Month);

                if (day > DateTime.DaysInMonth(nextYear, month))
                {
                    day = DateTime.DaysInMonth(nextYear, month);
                }

                nextRunTime = new DateTime(nextYear, month, day,
                               int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }

            TimeSpan initialDelay = nextRunTime - now;

            // Ограничение Timer: максимальная задержка = ~24.8 дней (Int32.MaxValue миллисекунд)
            TimeSpan maxDelay = TimeSpan.FromMilliseconds(Int32.MaxValue - 1);

            if (initialDelay > maxDelay)
            {
                // Если задержка больше максимальной — разбиваем её на части
                ScheduleWithRecursiveDelay(report, maxDelay, initialDelay - maxDelay);
            }
            else
            {
                // Если задержка допустимая — создаем таймер сразу
                StartYearlyTimer(report, initialDelay);
            }
        }

        /// <summary>
        /// Рекурсивно планирует запуск с разбивкой большой задержки на части
        /// </summary>
        private void ScheduleWithRecursiveDelay(Reports report, TimeSpan currentDelay, TimeSpan remainingDelay)
        {
            Timer tempTimer = null;
            tempTimer = new Timer(_ =>
            {
                tempTimer?.Dispose();

                if (remainingDelay > TimeSpan.FromMilliseconds(Int32.MaxValue - 1))
                {
                    ScheduleWithRecursiveDelay(report,
                        TimeSpan.FromMilliseconds(Int32.MaxValue - 1),
                        remainingDelay - TimeSpan.FromMilliseconds(Int32.MaxValue - 1));
                }
                else
                {
                    StartYearlyTimer(report, remainingDelay);
                }
            }, null, currentDelay, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Запускает таймер для ежегодной генерации
        /// </summary>
        private void StartYearlyTimer(Reports report, TimeSpan delay)
        {
            Timer scheduleTimer = null;
            scheduleTimer = new Timer(async _ =>
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GenerateData(report));

                // Перенастраиваем таймер на следующий год
                DateTime now = DateTime.Now;
                int nextYear = now.Year + 1;
                int day = int.Parse(report.ScheduleParam.Day);
                int month = int.Parse(report.ScheduleParam.Month);

                if (day > DateTime.DaysInMonth(nextYear, month))
                {
                    day = DateTime.DaysInMonth(nextYear, month);
                }

                DateTime nextRun = new DateTime(nextYear, month, day,
                                  int.Parse(report.ScheduleParam.Hour),
                                  int.Parse(report.ScheduleParam.Minute),
                                  int.Parse(report.ScheduleParam.Seconds));

                TimeSpan nextDelay = nextRun - now;

                if (nextDelay > TimeSpan.FromMilliseconds(Int32.MaxValue - 1))
                {
                    ScheduleWithRecursiveDelay(report,
                        TimeSpan.FromMilliseconds(Int32.MaxValue - 1),
                        nextDelay - TimeSpan.FromMilliseconds(Int32.MaxValue - 1));
                }
                else
                {
                    scheduleTimer?.Change(nextDelay, Timeout.InfiniteTimeSpan);
                }
            }, null, delay, Timeout.InfiniteTimeSpan);
        }



        /// <summary>
        /// генерация "На календарный конец месяца"
        /// </summary>
        /// <param name="report"></param>
        private void ScheduleGenerateEndMonth(Reports report)
        {
            var r = report.ScheduleParam;

            DateTime now = DateTime.Now;
            DateTime nextRunTime;

            // берем последнее число текущего месяца
            int nextDay = DateTime.DaysInMonth(now.Year, now.Month);
            nextRunTime = new DateTime(now.Year, now.Month, nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));

            if(now > nextRunTime)
            {
                DateTime nextMonth = now.AddMonths(1);
                nextDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                nextRunTime = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));
            }

            TimeSpan initialDelay = nextRunTime - now;

            Timer schuleTimer = null;

            schuleTimer = new Timer(async _ =>
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GenerateData(report));

                // перенастраиваем на след месяц
                DateTime nextMonth = DateTime.Now.AddMonths(1);
                int nextDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                DateTime nextRun = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, int.Parse(r.Hour), int.Parse(r.Minute), int.Parse(r.Seconds));

                TimeSpan delay = nextRun - DateTime.Now;

                schuleTimer?.Change(delay, Timeout.InfiniteTimeSpan);
            },
            null,
            initialDelay,
            Timeout.InfiniteTimeSpan);
        }

        #endregion

        private async void SendEmail(Reports report, string filePathReport)
        {
            if (report.SendByEmail)
            {
                // по конкретному адресу
                if (report.ModeEmailAddress)
                {
                    if (report.EmailTo != null || report.EmailTo != "")
                    {
                        await SendingVM.SendReportsToAddress(report.EmailTo, filePathReport);
                    }
                    else
                    {
                        MessageBoxService.ShowError("Ошибка", "Введите почту, кому отправить письмо");
                        return;
                    }
                }
            }
        }
    }
}
