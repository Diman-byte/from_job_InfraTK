using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportGenerator.Models;
using MailKit;
using MailKitSimplified.Sender.Services;
using System.IO;
using System.Text.Json;
using ReportGenerator.Services;

namespace ReportGenerator.ViewModels.UserControls
{
    public class SendingReportsUserControlViewModel
    {
        
        public MailModel MailModel { get; set; }

        private string _configFilePath;

        public SendingReportsUserControlViewModel()
        {
            MailModel = new MailModel();
            InializeConfigMail();
        }

        public async Task SendReportsToAddress(string destinationAddress, string filepath)
        {
            if(!CheckFields())
            {
                return;
            }
            if (filepath == null || filepath == "")
            {
                MessageBoxService.ShowError("Ошибка", "Отсутствует путь до файла для отправки по email");
                return;
            }

            try
            {
                await using var smtpSender = SmtpSender.Create($"{MailModel.AddressServer}:{MailModel.PortServer}")
                .SetCredential(MailModel.Login, MailModel.Password);


                await smtpSender.WriteEmail
                    .From(MailModel.SenderName, MailModel.SenderMail)
                    .To(destinationAddress)
                    .Subject(MailModel.MessageSubject)
                    .BodyText(FormMailBody(filepath))
                    .Attach(filepath)
                    .SendAsync();
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowError("Ошибка в отправке письма", $"{ex.Message}");
            }
            


        }

        private bool CheckFields()
        {
            bool result = true;

            if(MailModel.AddressServer == "" || MailModel.PortServer == "" || MailModel.Login == "" || MailModel.Password == "")
            {
                MessageBoxService.ShowError("Ошибка", "Введите данные почтового сервера");
                result = false;
            }
            if(MailModel.SenderName == "" || MailModel.SenderMail == "")
            {
                MessageBoxService.ShowError("Ошибка", "Введите параметры письма");
                result = false;
            }

            return result;
        }

        private string FormMailBody(string filepath)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append(MailModel.MessageHeader);
            mailBody.Append(Environment.NewLine);
            mailBody.Append(MailModel.MessageText);

            string nameFile = Path.GetFileName(filepath);

            mailBody.Append(nameFile);
            mailBody.Append(Environment.NewLine);
            mailBody.Append(MailModel.MessageEnding);
            
            return mailBody.ToString();
        }

        private void InializeConfigMail()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string nameConfig = "MailConfig.json";
            _configFilePath = Path.Combine(baseDir, nameConfig);

            if(!File.Exists(_configFilePath))
            {
                File.WriteAllText(_configFilePath, "");
            }
            else
            {
                if(File.ReadAllText(_configFilePath) != "")
                {
                    LoadConfigMail();
                }              
            }
        }

        private void LoadConfigMail()
        { 
            string json = File.ReadAllText(_configFilePath);
            MailModel = JsonSerializer.Deserialize<MailModel>(json);
        }

        public async Task SaveConfigMail()
        {
            if(File.Exists(_configFilePath))
            {
                string json = JsonSerializer.Serialize(MailModel, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_configFilePath, json);
            }
            
        }
    }
}
