using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using ExcelReportingAddin.KeyCloak;
using System.Threading;

namespace ExcelReportingAddin
{
    public partial class SettingsForm : Form
    {
        // поля(свойства) для хранения данных подключения
        public string DataServerAddress => txtDataServerAddress.Text; // Выражение => в C# называется лямбда-оператором (или оператором лямбда-выражения).
                                                                      // В данном случае оно используется для создания свойства только для чтения с помощью выражения.
                                                                      // => указывает, что свойство будет вычисляться и возвращать результат выражения txtDataServerAddress.Text
                                                                      // каждый раз, когда к нему обращаются.
        public int DataServerPort => int.Parse(txtDataServerPort.Text);
        public string KeyCloakAddress => txtKeyCloakAddress.Text;
        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;

        /// <summary>
        /// Информация о сервере идентификации KeyCloak
        /// </summary>
        public static KeyCloakInfo KeyCloakInfo;

        /// <summary>
        /// Токен пользователя
        /// </summary>
        public static JwtToken UserAccessToken;

        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings(); // чтобы настройки автоматически подгружались при открытии окна
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
             MessageBox.Show("Проверка подключения...");
            SaveSettings();

            // подключение и авторизация к keycloak
            //AuthUser();
        }


        // сохранение данных подключения можем реализовать через сохрание json-файла
        private void SaveSettings()
        {
            var settings = new
            {
                DataServerAddress = txtDataServerAddress.Text,
                DataServerPort = txtDataServerPort.Text,
                KeyCloakAddress = txtKeyCloakAddress.Text,
                Username = txtUsername.Text
            };
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string name_file_settings = "settings.json";
            string path_settings = Path.Combine(appDataPath, name_file_settings);
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path_settings, json);
        }

        private void LoadSettings()
        {
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string name_file_settings = "settings.json";
            string path_settings = Path.Combine(appDataPath, name_file_settings);

            if (File.Exists(path_settings))
            {
                string json = File.ReadAllText(path_settings);
                var settings = JsonConvert.DeserializeObject<dynamic>(json); // Тип данных dynamic в C# позволяет объявить переменную, тип которой определяется
                                                                             // во время выполнения программы, а не на этапе компиляции. Это значит,
                                                                             // что компилятор не проверяет корректность типов при работе с переменной
                                                                             // типа dynamic, и любые операции с такой переменной будут проверяться
                                                                             // только во время выполнения программы.
                txtDataServerAddress.Text = settings.DataServerAddress;
                txtDataServerPort.Text = settings.DataServerPort;
                txtKeyCloakAddress.Text = settings.KeyCloakAddress;
                txtUsername.Text = settings.Username;
            }
        }

        private void AuthUser()
        {

        }

        /// <summary>
        /// запрос информации о сервисе KeyCloak
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        //public static bool TryGetKeyCloakInfo(string host, string port)
        //{
        //    try
        //    {
        //        var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
        //        var client = new InfoApi.InfoApiClient(channel);
        //    }
        //    catch(Exception exception)
        //    {
        //        //потом добавить норм обработку ошибок
        //        MessageBox.Show("Неверно указан логин/пароль или настройки пользователя");
        //        return false;
        //    }
        //}

        /// <summary>
        /// Запрос токена доступа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool TryGetUserAccessToken(string login, string password)
        {
            try
            {
                var keyCloakApi = new KeyCloakApi();
                UserAccessToken = keyCloakApi.GetToken(KeyCloakInfo, login, password, CancellationToken.None).Result;
                return true;
            }
            catch(Exception exception)
            {
                //потом добавить норм обработку ошибок
                MessageBox.Show("Неверно указан логин/пароль или настройки пользователя");
                return false;
            }
        }
    }
}
