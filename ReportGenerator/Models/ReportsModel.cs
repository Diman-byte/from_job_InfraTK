using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReportGenerator.Models
{
    internal class ReportsModel
    {
    }

    /// <summary>
    /// возврат с диалогового окна
    /// </summary>
    public class AddReportDialogResult
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class Reports : ReactiveObject
    {
        private string _nameReport;
        private string? _description;
        private string? _templateFilePath;
        private string? _reportFolderPath;
        private ScheduleParamClass _scheduleParam = new ScheduleParamClass();
        private bool _sendByEmail = false;
        private bool _modeEmailRole;
        private string? _selectedRole;
        private bool _modeEmailAddress = true;
        private string? _emailTo;

        /// <summary>
        /// название отчета
        /// </summary>
        public string NameReport
        {
            get => _nameReport;
            set => this.RaiseAndSetIfChanged(ref _nameReport, value);
        }

        /// <summary>
        /// описание отчета
        /// </summary>
        public string? Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        /// <summary>
        /// путь до файла шаблона
        /// </summary>
        public string? TemplateFilePath
        {
            get => _templateFilePath;
            set => this.RaiseAndSetIfChanged(ref _templateFilePath, value);
        }

        /// <summary>
        /// каталог вывода отчета
        /// </summary>
        public string ReportFolderPath
        {
            get => _reportFolderPath;
            set => this.RaiseAndSetIfChanged(ref _reportFolderPath, value);
        }

        /// <summary>
        /// класс с параметрами расписания
        /// </summary>
        public ScheduleParamClass ScheduleParam { get; set; } = new ScheduleParamClass();

        /// <summary>
        /// отправлять ли по почте
        /// </summary>
        public bool SendByEmail
        {
            get => _sendByEmail;
            set => this.RaiseAndSetIfChanged(ref _sendByEmail, value);
        }

        /// <summary>
        /// отправлять email всем пользователям указанной роли
        /// </summary>
        public bool ModeEmailRole
        {
            get => _modeEmailRole;
            set
            {
                _modeEmailRole = value;
                //if (_modeEmailRole != value) { _modeEmailRole = value; }
                //this.RaiseAndSetIfChanged(ref _modeEmailRole, value);
                //ModeEmailAddress = !value;
            }
        }

        /// <summary>
        /// пользователям какой роли нужно отправлять по email
        /// </summary>
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedRole, value);
            }
        }


        /// <summary>
        /// отправлять email на указанный адрес
        /// </summary>
        public bool ModeEmailAddress
        {
            get => _modeEmailAddress;
            set
            {
                _modeEmailAddress = value;
                //if (_modeEmailAddress != value) { _modeEmailAddress = value; }
                //this.RaiseAndSetIfChanged(ref _modeEmailAddress, value);
                //ModeEmailRole = !value;
            }
        }

        /// <summary>
        /// почта, куда отправлять на "указанный адрес"
        /// </summary>
        public string? EmailTo
        {
            get => _emailTo;
            set => this.RaiseAndSetIfChanged(ref _emailTo, value);
        }
    }

    public class ScheduleParamClass : ReactiveObject
    {
        private string? _typeSchedule = "Каждый день";
        private string? _month = "0";
        private string? _day = "0";
        private string? _hour = "0";
        private string? _minute = "0";
        private string? _seconds = "0";


        /// <summary>
        /// тип - каждый месяц, каждый день и тд
        /// </summary>
        public string? TypeSchedule
        {
            get => _typeSchedule;
            set => this.RaiseAndSetIfChanged(ref _typeSchedule, value);
        }
        public string? Month
        {
            get => _month;
            set => this.RaiseAndSetIfChanged(ref _month, value);
        }
        public string? Day
        {
            get => _day;
            set => this.RaiseAndSetIfChanged(ref _day, value);
        }

        public string? Hour
        {
            get => _hour;
            set => this.RaiseAndSetIfChanged(ref _hour, value);
        }

        public string? Minute
        {
            get => _minute;
            set => this.RaiseAndSetIfChanged(ref _minute, value);
        }

        public string? Seconds
        {
            get => _seconds;
            set => this.RaiseAndSetIfChanged(ref _seconds, value);
        }
    }

}
