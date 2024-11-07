using HistoryDB;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Cas_1._4.ViewModels;

namespace Cas_1._4.Views
{
    /// <summary>
    /// Логика взаимодействия для HistDataQueryWindow.xaml
    /// </summary>
    public partial class HistDataQueryWindow : UserControl
    {
        public HistDataQueryWindow(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory;
        }
        private CassandraHistory _cassandraHistory;

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            // Парсинг и валидация введенных данных
            var database = databaseTextBox.Text;
            if (!int.TryParse(nodeIdTextBox.Text, out int nodeId))
            {
                MessageBox.Show("Node ID must be an integer.");
                return;
            }
            int.TryParse(DataTimeBeginTextBox.Text, out int beginMonth);
            int.TryParse(DataTimeEndTextBox.Text, out int endMonth);

            DateTime beginDateTime_0 = DateTime.Now;
            DateTime endDateTime_0 = DateTime.Now;

            DateTime beginDateTime = beginDateTime_0.AddMonths(beginMonth - beginDateTime_0.Month);
            DateTime endDateTime = endDateTime_0.AddMonths(endMonth - endDateTime_0.Month);





            int.TryParse(idTagsFromTextBox.Text, out int Tag_id_start);
            int.TryParse(idTagsToTextBox.Text, out int Tag_id_stop);


            List<int> idTags = new List<int>();
            for (int i = Tag_id_start; i <= Tag_id_stop; i++)
            {
                idTags.Add(i);
            }

            int.TryParse(maxRowsTextBox.Text, out int max_rows_output);


            // Вызов функции для получения исторических данных
            if (_cassandraHistory.TryReadHistData(database, nodeId, beginDateTime, endDateTime, idTags, out var result, out var msgLog))
            {
                // Преобразование результатов для отображения в DataGrid
                var displayResults = new List<HistDataResult>();
                foreach (var tagId in result.Keys)
                {
                    displayResults.AddRange(result[tagId].Select(r => new HistDataResult
                    {
                        TagId = tagId,
                        DateTime = r.dateTime,
                        IsGood = r.isGood,
                        Value = r.value
                    }));
                }

                resultsDataGrid.ItemsSource = displayResults;
            }
            


            }

    }
}
