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
using Common;

namespace Cas_1._4.Views
{
    /// <summary>
    /// Логика взаимодействия для TryInitializeHistDBColumns.xaml
    /// </summary>
    public partial class InitializeHistDBColumnsWindow : Window
    {
        private CassandraHistory _cassandraHistory;
        public InitializeHistDBColumnsWindow(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory; // Создание экземпляра класса CassandraHistory
        }

        private void InitializeHistDBColumns_Click(object sender, RoutedEventArgs e)
        {
            string histDBName = DatabaseNameTextBox.Text;
            List<int> nodeIds = ParseNodeIds(NodeIdsTextBox.Text);

            if (nodeIds.Count == 0)
            {
                ResultTextBox.Text = "Please enter valid Node IDs.";
                return;
            }

            // Обратите внимание на использование Common.MsgLog.MsgLogClass вместо Cas_1._4.Views.MsgLogClass
            bool success = _cassandraHistory.TryInitializeHistDBColumns(histDBName, nodeIds, out Common.MsgLog.MsgLogClass msgLog);

            if (success)
            {
                ResultTextBox.Text = "Columns initialization successful.";
            }
            else
            {
                ResultTextBox.Text = "Columns initialization failed. Message: " + (msgLog != null ? msgLog.LogText : "Unknown error.");
            }
        }


        // Метод для парсинга Node IDs из строки
        private List<int> ParseNodeIds(string nodeIdsString)
        {
            List<int> nodeIds = new List<int>();
            string[] nodeIdsArray = nodeIdsString.Split(',');

            foreach (string nodeIdStr in nodeIdsArray)
            {
                if (int.TryParse(nodeIdStr.Trim(), out int nodeId))
                {
                    nodeIds.Add(nodeId);
                }
            }

            return nodeIds;
        }



    }


}