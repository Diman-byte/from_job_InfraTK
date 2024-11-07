using System.IO;
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


namespace Spravochnik_Dima_WPF_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            // Получаем путь к папке, где хранятся данные приложения
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory; //  возвращает путь к папке, в которой находится исполняемый файл программы.
            string relativeFilePath = System.IO.Path.Combine(appDataPath, "connection_string.txt");

            // Проверяем, существует ли файл с сохранённой строкой подключения
            if (File.Exists(relativeFilePath))
            {
                // Читаем строку подключения из файла и устанавливаем в текстовое поле
                string savedConnectionString = File.ReadAllText(relativeFilePath);
                ConnectionStringTextBox.Text = savedConnectionString;
            }
            else
            {
                // Создаём файл, если он не существует, и записываем пустую строку
                File.WriteAllText(relativeFilePath, string.Empty);
            }

        }

        private void OnRunMainFunction(object sender, RoutedEventArgs e)
        {
            string connectionString = ConnectionStringTextBox.Text;

            string relativeFilePath = @"connection_string.txt"; // Относительный путь



            if (!string.IsNullOrEmpty(connectionString))
            {

                File.WriteAllText(relativeFilePath, connectionString);
                // Host=localhost;Username=postgres;Password=postgres;Database=PA_Project
                Program.SetConnectionString(connectionString);

                if (Program.TestConnection() == true)
                {

                    // проверка на существование записей в UnitGroups
                    if (Program.TestEmptyUnitGroups() == true)
                    {
                        string groups_path = @"UnitGroups.csv";
                        var UnitGroups = Program.ParseUnitGroups(groups_path);
                        Program.InsertUnitGroups(UnitGroups);
                        MessageBox.Show("Таблица UnitGroups пустая, выполняю заполнение этой таблицы. Повторите попытку заполнения");
                    }
                    else
                    {
                        // Host=localhost;Username=postgres;Password=postgres;Database=PA_Project

                        //string basic_path = @"C:\Users\Dmitriy.Simonov\Desktop\C#\Справочник_Шамиль\Базовые переменные\1.4.txt";
                        //string deriveds_path = @"C:\Users\Dmitriy.Simonov\Desktop\C#\Справочник_Шамиль\Производные переменные\okei(ver3).xlsx";

                        string basic_path = @"1.4.txt";
                        string deriveds_path = @"okei(ver4).xlsx";

                        var Basicunits = Program.ParseBasicUnits(basic_path);
                        Program.InsertBasicUnits(Basicunits);

                        List<DerivedsUnit> DerivedsUnits = Program.ParseDerivedsUnits(deriveds_path);
                        Program.InsertDerivedsUnits(DerivedsUnits);

                        MessageBox.Show("Функция выполнена успешно");
                    }

                }
                else
                {
                    MessageBox.Show("Неверные данные для подключения");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите строку подключения.");
            }
        }

    }
}