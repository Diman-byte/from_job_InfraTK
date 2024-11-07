using Common.MsgLog;
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
using HistoryDB;

namespace Cas_1._4.Views
{
    /// <summary>
    /// Логика взаимодействия для DropNodeTables.xaml
    /// </summary>
    public partial class DropNodeTables : UserControl
    {
        private CassandraHistory _cassandraHistory;
        public DropNodeTables(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory;
        }
        private void DropNodeTablesClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtNodeId.Text, out int nodeId))
            {
                var database = txtDatabase.Text;
                if (!string.IsNullOrWhiteSpace(database))
                {
                    // Предположим, что у вас есть метод TryDropNodeTables и класс MsgLogClass в вашем проекте
                    if (_cassandraHistory.TryDropNodeTables(database, nodeId, out MsgLogClass msgLog))
                    {
                        txtResult.Text = "Tables dropped successfully.";
                    }
                    else
                    {
                        txtResult.Text = $"Failed to drop tables: {msgLog?.LogText}";
                    }
                }
                else
                {
                    txtResult.Text = "Please enter a valid database name.";
                }
            }
            else
            {
                txtResult.Text = "Please enter a valid node ID.";
            }
        }
    }
}

