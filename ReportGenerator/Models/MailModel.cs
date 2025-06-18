using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReportGenerator.Models
{
    public class MailModel : ReactiveObject
    {
        private string? _addressServer = "localhost";
        private string? _portServer = "25";
        private string? _login = "admin";
        private string? _password = "admin";

        private string? _senderName = "Система аналитики";
        private string? _senderMail = "example@mail.ru";
        private string? _messageSubject = "Автоматически сгенерированный отчет";
        private string? _messageHeader = "Здравствуйте!";
        private string? _messageText = "Направляется автоматически сгенерированный отчет: ";
        private string? _messageEnding = "Данное сообщение сформировано автоматически";

        /// <summary>
        /// адрес сервера
        /// </summary>
        public string AddressServer
        {
            get => _addressServer;
            set => this.RaiseAndSetIfChanged(ref _addressServer, value);
        }

        /// <summary>
        /// порт сервера
        /// </summary>
        public string PortServer
        {
            get => _portServer;
            set => this.RaiseAndSetIfChanged(ref _portServer, value);
        }

        /// <summary>
        /// логин на сервере
        /// </summary>
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        /// <summary>
        /// пароль на сервере
        /// </summary>
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        /// <summary> Отображаемое имя отправителя </summary>
        public string SenderName
        {
            get => _senderName;
            set => this.RaiseAndSetIfChanged(ref _senderName, value);          
        }

        /// <summary> Почтовый адрес отправителя </summary>
        public string SenderMail
        {
            get => _senderMail;
            set => this.RaiseAndSetIfChanged(ref _senderMail, value);
        }

        /// <summary> Тема письма </summary>
        public string MessageSubject
        {
            get => _messageSubject;
            set => this.RaiseAndSetIfChanged(ref _messageSubject, value);
        }

        /// <summary>
        /// вступление письма
        /// </summary>
        public string MessageHeader
        {
            get => _messageHeader;
            set => this.RaiseAndSetIfChanged(ref _messageHeader, value);
        }

        /// <summary>
        /// текст письма
        /// </summary>
        public string MessageText
        {
            get => _messageText;
            set => this.RaiseAndSetIfChanged(ref _messageText, value);
        }

        /// <summary> Заключение письма </summary>
        public string MessageEnding
        {
            get => _messageEnding;
            set => this.RaiseAndSetIfChanged(ref _messageEnding, value);
        }
    }
}
