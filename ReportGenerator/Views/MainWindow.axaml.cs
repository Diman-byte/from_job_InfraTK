using Avalonia.Controls;
using ReportGenerator.ViewModels;

namespace ReportGenerator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ������������ ������ �� ������� ���� � VM
            //DataContext = new MainWindowViewModel(this);
            // -> ������-�� �� �������� �������
        }
    }
}