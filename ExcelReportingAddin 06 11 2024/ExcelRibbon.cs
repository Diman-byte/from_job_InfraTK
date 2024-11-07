using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ExcelReportingAddin
{
    public partial class ExcelRibbon
    {
        private void ExcelRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            // Вызов окна настройки подключения
            OpenSettingsWindow();
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            // Вызов окна чтения данных
            OpenReadDataWindow();
        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            // Вызов окна чтения событий
            //OpenReadEventsWindow();
            MessageBox.Show("Окно чтения событий");
        }



        private void OpenSettingsWindow()
        {
            var settingForm = new SettingsForm();
            settingForm.ShowDialog();
        }

        private void OpenReadDataWindow()
        {
            var readData = new DataReadingForm();
            readData.ShowDialog();
        }
    }
}
