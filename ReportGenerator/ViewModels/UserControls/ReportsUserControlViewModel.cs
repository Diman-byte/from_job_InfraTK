using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportGenerator.Models;
using System.Reactive.Linq;
using System.Reactive;
using Avalonia.Controls;
using ReportGenerator.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Models;
using ReportGenerator.Services;


namespace ReportGenerator.ViewModels.UserControls
{
    public class ReportsUserControlViewModel : ReactiveObject
    {
        //  Avalonia Binding работает только с public свойствами

        /// <summary>
        /// список с отчетами, используется в datagrid
        /// </summary>
        public ObservableCollection<Reports> ReportsList { get; set; }

        /// <summary>
        /// команда, которая будет открывать диалоговое окно для добавления отчета
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddReportCommand { get; }

        /// <summary>
        /// объявление для взаимодействия с новым диалоговым окном
        /// Interaction для открытия окна добавления отчета
        /// </summary>
        public Interaction<AddReportWindowViewModel, AddReportDialogResult?> ShowAddReportDialog { get; }

       /// <summary>
       /// удалить выбранный отчет
       /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteReportCommand { get; }

        /// <summary>
        /// combobox с выбором расписания
        /// </summary>
        public ObservableCollection<string> TypeScheduleList { get; set; } = new() {
                "Каждый день",
                "Каждый месяц",
                "Каждый год",
                "На календарный конец месяца"
        };

        public ObservableCollection<string> RolesEmail { get; set; } = new()
        {
            "Отдел кадров",
            "Отдел системных разработок"
        };

        /// <summary>
        /// выбранный отчет в списке
        /// </summary>
        private Reports? _selectedReport;
        public Reports SelectedReport
        {
            get => _selectedReport;
            set => this.RaiseAndSetIfChanged(ref _selectedReport, value);
        }

        /// <summary>
        /// путь до файла с сохраненной конфигурацией
        /// </summary>
        private string _configPath;


        /// <summary>
        /// команда для открытия диалога с выбором файла шаблона
        /// </summary>
        public ReactiveCommand<Unit,Unit> OpenFileTemplateCommand { get; }
        public Interaction<Unit, string?> OpenFileTemplateDialog { get; }

        /// <summary>
        /// команда для выбора папки, куда будут выводиться отчеты
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenFolderReportCommand { get; }
        public Interaction<Unit, string?> OpenFolderReportDialog { get; }

        /// <summary>
        /// конструктор
        /// </summary>
        public ReportsUserControlViewModel()
        {
            ReportsList = new ObservableCollection<Reports>();  

            //диалог с добавлением отчета
            ShowAddReportDialog = new Interaction<AddReportWindowViewModel, AddReportDialogResult?>();
            AddReportCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var addReport = new AddReportWindowViewModel();
                var result = await ShowAddReportDialog.Handle(addReport);

                if (result != null)
                {
                    var report = new Reports();
                    report.NameReport = result.Name;
                    report.Description = result.Description;
                    report.ReportFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                    ReportsList.Add(report);
                }
            });
            DeleteReportCommand = ReactiveCommand.Create(DeleteReport);

            // диалог с получением файла отчета
            OpenFileTemplateDialog = new Interaction<Unit, string?>();
            OpenFileTemplateCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await OpenFileTemplateDialog.Handle(Unit.Default);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    ReportsList[ReportsList.IndexOf(SelectedReport)].TemplateFilePath = result;
                }
            });

            // диалог с получением каталога для вывода отчетов
            OpenFolderReportDialog = new Interaction<Unit, string?>();
            OpenFolderReportCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await OpenFolderReportDialog.Handle(Unit.Default);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    ReportsList[ReportsList.IndexOf(SelectedReport)].ReportFolderPath = result;
                }
            });


            SubscribeForError();

            InitializeConfigReports();
    
        }

        /// <summary>
        /// подпишусь на ошибки
        /// </summary>
        private void SubscribeForError()
        {
            AddReportCommand.ThrownExceptions.Subscribe(async ex => {
                //System.Diagnostics.Debug.WriteLine(ex.ToString());
                MessageBoxService.ShowError("Ошибка", "Ошибка при добавлении отчета");
            });
            OpenFileTemplateCommand.ThrownExceptions.Subscribe(async ex =>
            {
                MessageBoxService.ShowError("Ошибка", "Ошибка в команде с получением каталога для вывода отчетов");
            });
        }

        private async void DeleteReport()
        {
            ReportsList.Remove(SelectedReport);

            //MessageBoxService.ShowError("dsdnmsadbsabdhsbadahjsad", "dsad");
        }

        private void InitializeConfigReports()
        {
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            string nameconfig = "ReportsConfig.json";
            _configPath = Path.Combine(basedir, nameconfig);

            if(!File.Exists(_configPath))
            {
                File.WriteAllText(_configPath, "");
            }
            else
            {
                if(File.ReadAllText(_configPath) != "")
                {
                    LoadConfigReports();
                }
            }
        }

        /// <summary>
        /// загрузка данных с файла
        /// </summary>
        private void LoadConfigReports()
        {
            string res = File.ReadAllText(_configPath);
            var json = JsonSerializer.Deserialize<ObservableCollection<Reports>>(res);       
            ReportsList.Clear();
            foreach(var item in json)
            {
                ReportsList.Add(item);
            }
        }

        /// <summary>
        /// сохранение в файл
        /// </summary>
        public async Task SaveConfig()
        {
            if(File.Exists(_configPath))
            {
                //var json = JsonConvert.SerializeObject(ReportsList.ToList<Reports>, Formatting.Indented);
                var json = JsonSerializer.Serialize(ReportsList, new JsonSerializerOptions
                {
                    WriteIndented = true, // для красивого форматирования
                });
                await File.WriteAllTextAsync(_configPath, json);
            }           
        }


    }
}
