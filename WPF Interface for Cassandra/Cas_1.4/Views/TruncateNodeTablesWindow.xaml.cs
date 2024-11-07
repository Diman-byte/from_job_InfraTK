using Common.MsgLog;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cas_1._4.Views
{
    /// <summary>
    /// Логика взаимодействия для TruncateNodeTablesWindow.xaml
    /// </summary>
    public partial class TruncateNodeTablesWindow : UserControl
    {

        private CassandraHistory _cassandraHistory;

        public TruncateNodeTablesWindow(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory;
            // Здесь может быть инициализация _cassandraHistory, например, через DI
        }

        private void TruncateTables_Click(object sender, RoutedEventArgs e)
        {
            string database = databaseTextBox.Text;
            if (!int.TryParse(nodeIdTextBox.Text, out int nodeId))
            {
                logTextBox.Text = "Node ID must be an integer.";
                return;
            }

            if (_cassandraHistory.TryTruncateNodeTables(database, nodeId, out MsgLogClass msgLog))
            {
                logTextBox.Text = "Tables truncated successfully.";
            }
            else
            {
                logTextBox.Text = $"Failed to truncate tables. Error: {msgLog?.LogText}";
            }
        }
    }
}
