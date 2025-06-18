using ReactiveUI;
using ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.ViewModels
{
    public class AddReportWindowViewModel : ViewModelBase
    {
        public string ReportName { get; set; } = "Отчет";
        public string ReportDescription { get; set; }



        /// <summary>
        /// Команда OK — возвращает данные и закрывает окно
        /// </summary>
        public ReactiveCommand<Unit, AddReportDialogResult> CommandOk { get; }

        /// <summary>
        /// Команда Cancel — закрывает окно без данных
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public AddReportWindowViewModel()
        {
            var canExecute = this.WhenAnyValue(x => x.ReportName, Name => !string.IsNullOrWhiteSpace(Name));           
           
            
            CommandOk = ReactiveCommand.Create(
                () => { return new AddReportDialogResult { Name = ReportName, Description = ReportDescription}; }, // лямбда-функция без параметров, которая возвращает кортеж из двух значений; () — это синтаксис лямбда-выражения без параметров,
                canExecute); // кнопка работает только тогда, когда пользователь ввел навзание
            // а можно было так:
            //CommandOk = ReactiveCommand.Create(GetParam, canExecute);

            CancelCommand = ReactiveCommand.Create(() => { });
            //Команда "Отмена" ничего не делает, просто используется для закрытия окна без передачи данных.
        }

        private (string Name, string Description) GetParam()
        {
            return (ReportName, ReportDescription);
        }

        
    }
}
