using Avalonia.Controls;
using MsBox.Avalonia.Models;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Services
{
    public static class MessageBoxService
    {
        /// <summary>
        /// вывести message Box с ошибкой
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static async void ShowError(string title, string message)
        {
            //var box = MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok);

            var box = MessageBoxManager.GetMessageBoxCustom(new MsBox.Avalonia.Dto.MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "Ok", }
                },
                ContentTitle = title,
                ContentMessage = message,
                Icon = MsBox.Avalonia.Enums.Icon.Error,
                CanResize = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                MaxWidth = 500,
                MaxHeight = 800,
            });
            var result = await box.ShowWindowAsync();
        }

        /// <summary>
        /// вывести message Box с информационным сообщением
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static async void ShowInfo(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxCustom(new MsBox.Avalonia.Dto.MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "Ok", }
                },
                ContentTitle = title,
                ContentMessage = message,
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                CanResize = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                MaxWidth = 500,
                MaxHeight = 800,
            });
            var result = await box.ShowWindowAsync();
        }
    }


}
