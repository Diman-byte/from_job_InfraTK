using Avalonia.Controls;
using ReportGenerator.ViewModels;

namespace ReportGenerator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // пробрасываем ссылку на главное окно в VM
            //DataContext = new MainWindowViewModel(this);
            // -> почему-то не работает проброс
        }
    }
}