
using HistoryDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Cas_1._4.Views
{
    public partial class ConnectDialog : Window
    {
        private CassandraHistory _cassandraHistory;
        public ConnectDialog(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение данных подключения
            var host = HostTextBox.Text;
            var port = int.Parse(PortTextBox.Text);
            var database = DatabaseTextBox.Text;
            var user = UserTextBox.Text;
            var password = PasswordBox.Password;

            var connectionInfo = new HistoryDB.HistoryDataBaseInfo()
            {
                Host = host,
                Port = port,
                DataBase = database,
                User = user,
                Password = password,
                CommandTimeout = 30
            };


           
                // Тут можно добавить логику подключения к Cassandra, используя connectionInfo
                // Например, использовать CassandraHistory.TryConnect()

               
                bool success = _cassandraHistory.TryConnect(connectionInfo, out var msgLog);

                if (success)
                {
                    StatusBarText.Text = "Connected";
                  
                // Console.WriteLine("Подлючение к БД выполнено");
                }

                else
                {
                    StatusBarText.Text = "NO Connected !";
                }


                // Закрытие диалога с результатом OK
               //  this.DialogResult = true;
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие диалога с результатом Cancel
            this.DialogResult = false;
        }
    }
}
