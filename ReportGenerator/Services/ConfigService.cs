using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReportGenerator.Models;
using MsBox.Avalonia;

namespace ReportGenerator.Services
{
    public class ConfigService
    {
        public ConfigSettings configSettings;

        public ConfigReadData configReadData;

        private string _templateFilePath;

        public ConfigService(string templateFilePath) {
            _templateFilePath = templateFilePath;
            //configSettings = new ConfigSettings();
            configReadData = new ConfigReadData();
            
        }



        /// <summary>
        /// Метод загрузки параметров о подключении c файла надстройки
        /// </summary>
        //public void LoadSettings()
        //{
        //    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string name_file_settings = "settings.json";
        //    string path_settings = Path.Combine(appDataPath, name_file_settings);

        //    if (File.Exists(path_settings))
        //    {
        //        string json = File.ReadAllText(path_settings);
        //        configSettings = JsonConvert.DeserializeObject<ConfigSettings>(json);
        //    }
        //    else
        //    {
        //        //ошибка - файла не существует
        //        // это не работает - MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нет файла с настройками подключения").ShowAsync().Wait();
        //    }
        //}


        /// <summary>
        /// Метод загрузки данных с окна "Чтение данных"
        /// </summary>
        public bool LoadConfigData()
        {
            //string template_name = "ExampleReport.xlsx";
            string template_path = _templateFilePath;
            // получаем только имя файла без пути
            string template_name = Path.GetFileName(template_path);

            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                .ApplicationData), template_name + ".conf");

            if (File.Exists(configFilePath) && File.ReadAllText(configFilePath) != "{}")
            {
                string json = File.ReadAllText(configFilePath);
                configReadData = JsonConvert.DeserializeObject<ConfigReadData>(json);
            }
            else
            {
                // файл с данными "Чтение данных" пуст или не существует
                MessageBoxService.ShowError("Ошибка в файле конфигурации", "Файл с данными из окна \"Чтение данных\" пуст или не существует. " +
                    $"\nПроверьте настройки шаблона {template_name}");
                return false;
            }
            return true;
        }

    }
}
