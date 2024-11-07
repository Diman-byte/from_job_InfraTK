
// тут должно быть много всего с рабочего проекта InfraAnalytics
// сюда лишь вставил метод, который обрабатывает нажатие кнопки "Заполнить инженерные единицы"
public async void InsertEngineeringUnits_Click(object sender, RoutedEventArgs e)
{
    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
        .GetMessageBoxStandardWindow("Подтверждение заполнения",
        $"Вы действительно хотите заполнить справочник инженерных единиц?", MessageBox.Avalonia.Enums.ButtonEnum.YesNo);

    var buttonResult = await messageBoxStandardWindow.ShowDialog(this);
    if (buttonResult == ButtonResult.Yes)
    {
        if(Configurator.Models.Main.EngineeringUnits.EngineeringUnitsUtility.InsertUnits())
        {
            var messageBoxStandardWindow_2 = MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ContentTitle = "Оповещение",
                    ContentMessage = "Функция выполнена успешно",
                    ButtonDefinitions = ButtonEnum.Ok
                });
            messageBoxStandardWindow_2.ShowDialog(this);
        }
    }
}

