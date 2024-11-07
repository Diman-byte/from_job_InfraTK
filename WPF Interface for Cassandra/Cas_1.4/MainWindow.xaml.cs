
using HistoryDB;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Cas_1._4.Views;
// using Cas_1._4.ViewModels;

using static Cas_1._4.Views.ConnectDialog;

// 172.30.221.120


namespace Cas_1._4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _cassandraHistory = new CassandraHistory();
        }

        private CassandraHistory _cassandraHistory;
       

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var connectDialog = new ConnectDialog(_cassandraHistory); // Инициализация диалога подключения
            var dialogResult = connectDialog.ShowDialog(); // Показать диалог и получить результат

            string OurIP = _cassandraHistory._HOST.ToString();
            OurIP = "IP узла, к которому мы подключены " + OurIP;
            IPTextBox.Text = OurIP;


            if (dialogResult == true)
            {
                // Обработка успешного подключения

            }
            else
            {
                // Обработка отмены подключения или ошибки подключения
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            // Логика отключения от базы данных
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void InsertData_Click(object sender, RoutedEventArgs e)
        {
            var insertDataWindow = new InsertDataWindow(_cassandraHistory);
            Placeholder.Content = insertDataWindow;
            // DataContext = new InsertDataWindow(_cassandraHistory);       
            // InsertDataWindow insertDataWindow = new InsertDataWindow(_cassandraHistory);
        }

        private void InsertDataAsync_Click(object sender, RoutedEventArgs e)
        {
            var insertDataWindow = new InsertDataAsyncWindow(_cassandraHistory);
            Placeholder.Content = insertDataWindow;
        }

        private void ViewData_Click(object sender, RoutedEventArgs e)
        {
            var histDataQueryWindow = new HistDataQueryWindow(_cassandraHistory);
            Placeholder.Content = histDataQueryWindow;
        }

        private void ManageEvents_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна управления событиями
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InitializeHistDBColumns_Click(object sender, RoutedEventArgs e)
        {
            InitializeHistDBColumnsWindow initializeHistDBColumnsWindow = new InitializeHistDBColumnsWindow(_cassandraHistory);
            initializeHistDBColumnsWindow.ShowDialog();
        }

        private void CreateKeySpace_Click(object sender, RoutedEventArgs e)
        {
            var createKeySpaceWindow = new CreateKeySpaceWindow(_cassandraHistory);
            Placeholder.Content = createKeySpaceWindow;
            // DataContext = new CreateKeySpaceWindow(_cassandraHistory);
        }

        private void DeleteNodeTables_Click(object sender, RoutedEventArgs e)
        {
            var truncateNodeTablesWindow = new TruncateNodeTablesWindow(_cassandraHistory);
            Placeholder.Content = truncateNodeTablesWindow;
        }
        private void DropNodeTablesClick(object sender, RoutedEventArgs e)
        {
            var dropNodeTables = new DropNodeTables(_cassandraHistory);
            Placeholder.Content = dropNodeTables;
        }
    }
}
