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
// using Cas_1._4.ViewModels;
using Common.MsgLog;
using Common;
using HistoryDB;

namespace Cas_1._4.Views
{
    /// <summary>
    /// Логика взаимодействия для InsertDataWindow.xaml
    /// </summary>
    public partial class InsertDataWindow
    {
        public InsertDataWindow(CassandraHistory cassandraHistory)
        {
            InitializeComponent();
            _cassandraHistory = cassandraHistory;
        }

        private CassandraHistory _cassandraHistory;

        public void InsertData_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из UI
            string database = DatabaseTextBox.Text;
            int nodeId = int.Parse(NodeIdTextBox.Text);
            Dictionary<int, List<DataVal>> data_1 = new Dictionary<int, List<DataVal>>();

            // ручной метод
            // data_1.Add(1, new List<DataVal>() { new DataVal {DateTime = DateTime.Now, Val = 10.5, IsGood = true } });

            // рандом
            int limit_tag = 10000;   // количество тегов для генерации
            int interval = 1000;  // диапазон рандомных чисел от 0 до interval
            int kolvo_val = 1;   // количество значений в каждом теге
            var random = new Random();
            for (int tag_id = 1; tag_id <= limit_tag; tag_id++)
            {
                var new_list = new List<DataVal>();
                for (int i = 0; i < kolvo_val; i++) {
                    new_list.Add(new DataVal
                    {
                        DateTime = DateTime.Now.AddMinutes(-i),
                        Val = (double)(random.NextDouble() * interval), // генерация случайного значения
                        IsGood = random.NextDouble() > 0.1 // 10% шанс на false
                    });
                }
                data_1[tag_id] = new_list;
            }

            
                


            // Вызываем метод TryInsertData
            MsgLogClass msgLog;
            bool success = _cassandraHistory.TryInsertData(database, nodeId, data_1, out msgLog);

            // Обновляем лог
            if (success)
            {
                ResultTextBox.Text = "Рандомные значения добавлены";
            }
            else
            {
                ResultTextBox.Text = "Ошибка. Message: " + (msgLog != null ? msgLog.LogText : "Unknown error.");
            }
        }
    }
}
