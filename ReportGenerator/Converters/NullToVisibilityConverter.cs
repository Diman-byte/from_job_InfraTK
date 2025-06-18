using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using System.Globalization;

namespace ReportGenerator.Converters
{
    // NullToVisibilityConverter — это конвертер данных, используемый в XAML-связывании, чтобы превращать null-значение
    // (или ненулевое) во Visibility.Visible или Visibility.Collapsed. Это удобно, когда ты хочешь показывать/скрывать
    // элементы интерфейса в зависимости от того, установлено ли свойство (например, выбран ли отчет в таблице).
    public class NullToVisibilityConverter : IValueConverter
    {
        // преобразует пришедшее от привязки значение в тот тип, который понимается приёмником привязки.
        //value — значение, произведённое исходной привязкой.
        //targetType — тип целевого свойства привязки.
        //parameter — используемый параметр преобразователя.
        //culture — язык и региональные параметры, используемые в преобразователе.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? parameter = true : parameter = false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
