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
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices.ComTypes;

namespace ExcelReportingAddin
{
    public partial class SettingsForm : Form
    {
        // поля для хранения данных подключения
        public string DataServerAddress => txtDataServerAddress.Text; // Выражение => в C# называется лямбда-оператором (или оператором лямбда-выражения).
                                                                      // В данном случае оно используется для создания свойства только для чтения с помощью выражения.
                                                                      // => указывает, что свойство будет вычисляться и возвращать результат выражения txtDataServerAddress.Text
                                                                      // каждый раз, когда к нему обращаются.
        public int DataServerPort => int.Parse(txtDataServerPort.Text);
        public string KeyCloakAddress => txtKeyCloakAddress.Text;
        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;
        public string Realm => txtRealm.Text;
        public string Scope => txtScope.Text;
        public string ClientId => txtClientId.Text;



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

            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запрет изменения размера
            this.MaximizeBox = false; // Убираем кнопку максимизации

            LoadSettings(); // чтобы настройки автоматически подгружались при открытии окна

            this.FormClosed += SettingsForm_FormClosed;

        }

        /// <summary>
        /// Обработка события при закрывании окна(формы)
        /// </summary>
        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Обработка нажатия кнопки "Проверка подключения"
        /// </summary>
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Проверка подключения...");
            SaveSettings();

            GetInfoKeyCloak();

            //подключение и авторизация к keycloak
            if (TryGetUserAccessToken(Username, Password))
            {
                MessageBox.Show("Подключение keycloak успешно");
            }
            else
            {
                MessageBox.Show("Ошибка подключения keycloak");
            }
        }

        /// <summary>
        /// Проверка подключения при нажатии кнопок "Загрузить"
        /// </summary>
        public bool CheckConnection()
        {
            GetInfoKeyCloak();
            //подключение и авторизация к keycloak
            if (TryGetUserAccessToken(Username, Password))
            {
                //MessageBox.Show("Подключение keycloak успешно");
                return true;
            }
            else
            {
                MessageBox.Show("Ошибка подключения keycloak");
                return false;
            }
        }

        /// <summary>
        /// Метод сохранения параметров о подключении
        /// сохранение данных подключения можем реализовать через сохрание json-файла
        /// </summary>
        private void SaveSettings()
        {
            var settings = new
            {
                DataServerAddress = txtDataServerAddress.Text,
                DataServerPort = txtDataServerPort.Text,
                KeyCloakAddress = txtKeyCloakAddress.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Text,
                Realm = txtRealm.Text,
                Scope = txtScope.Text,
                ClientId = txtClientId.Text,
            };


            string appDataPath_1 = AppDomain.CurrentDomain.BaseDirectory; // Возвращает путь к папке, в которой запущен текущий исполняемый файл
            // Ориентирована на приложение; подходит для ресурсов и файлов, связанных с текущей программой

            string appDataPath_2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // AppData/Roaming

            string name_file_settings = "settings.json";
            string path_settings_1 = Path.Combine(appDataPath_1, name_file_settings);
            string path_settings_2 = Path.Combine(appDataPath_2, name_file_settings);

            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(path_settings_1, json);
            File.WriteAllText(path_settings_2, json);
        }

        /// <summary>
        /// Метод загрузки параметров о подключении
        /// </summary>
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
                txtPassword.Text = settings.Password;
                txtRealm.Text = settings.Realm;
                txtScope.Text = settings.Scope;
                txtClientId.Text = settings.ClientId;
                
            }
        }

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
            catch (Exception exception)
            {
                //потом добавить норм обработку ошибок
                MessageBox.Show("Неверно указан логин/пароль или другие настройки (Realm, Scope, ClientId)");
                return false;
            }
        }

        /// <summary>
        /// запрос информации о сервисе KeyCloak
        /// </summary>
        private void GetInfoKeyCloak()
        {
            string realm_ = string.IsNullOrEmpty(Realm) ? "master" : Realm;
            string clientId_ = string.IsNullOrEmpty(ClientId) ? "ClientTestId" : ClientId;
            string scope_ = string.IsNullOrEmpty(Scope) ? "openid" : Scope;

            // инициализируем поле
            KeyCloakInfo = new KeyCloakInfo()
            {
                AuthorizationUrl = $"http://{KeyCloakAddress}/realms/{realm_}/protocol/openid-connect/token",
                ClientId = clientId_,
                Scope = scope_
            };
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //AutomaticFilling = cbAutoFilling.Checked;
        }
    }
}
