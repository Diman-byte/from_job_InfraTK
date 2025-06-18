using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReportGenerator.Services;
using ReportGenerator.Services.KeyCloak;

namespace ReportGenerator.ViewModels.UserControls
{
    public class SettingsUserControlViewModel : ReactiveObject
    {
        private string _dataServerAddress = "localhost";
        private string _dataServerPort = "5100";
        private string _keycloakAddress = "localhost:8080";
        private string _login = "usertest";
        private string _password = "test1234";
        private string _realm = "";
        private string _scope = "";
        private string _clientId = "";
       

        public string DataServerAddress
        {
            get => _dataServerAddress;
            set => this.RaiseAndSetIfChanged(ref _dataServerAddress, value);
        }
        public string DataServerPort
        {
            get => _dataServerPort;
            set => this.RaiseAndSetIfChanged(ref _dataServerPort, value);
        }
        public string KeycloakAddress
        {
            get => _keycloakAddress;
            set => this.RaiseAndSetIfChanged(ref _keycloakAddress, value);
        }
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        public string Realm
        {
            get => _realm;
            set => this.RaiseAndSetIfChanged(ref _realm, value);
        }
        public string Scope
        {
            get => _scope;
            set => this.RaiseAndSetIfChanged(ref _scope, value);
        }
        public string ClientId
        {
            get => _clientId;
            set => this.RaiseAndSetIfChanged(ref _clientId, value);
        }

        public ReactiveCommand<Unit, Unit> CheckConnectionCommand { get; }

        private string _connectionMessage = "проверки соединения не было";
        public string ConnectionMessage
        {
            get => _connectionMessage;
            set => this.RaiseAndSetIfChanged(ref _connectionMessage, value);
        }

        /// <summary>
        /// команда сохранения настроек подключения
        /// </summary>
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }

        /// <summary>
        /// Информация о сервере идентификации KeyCloak
        /// </summary>
        public static KeyCloakInfo KeyCloakInfo;

        /// <summary>
        /// Токен пользователя
        /// </summary>
        public static JwtToken UserAccessToken;


        private string _filePathSettings;


        public SettingsUserControlViewModel()
        {
            InitailizeConfig();
           

            CheckConnectionCommand = ReactiveCommand.Create(CheckConnection);
            SaveSettingsCommand = ReactiveCommand.CreateFromTask(SaveSettings);
        }

        /// <summary>
        /// инициализируем файлик с настройками
        /// </summary>
        private void InitailizeConfig()
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string nameFile = "SettingsConfig.json";
            string filePath = Path.Combine(baseFolder, nameFile);
            _filePathSettings = filePath;

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
            }
            else
            {
                if (File.ReadAllText(filePath) != "")
                {
                    LoadConfig();
                }
               
            }
        }

        public async Task SaveSettings()
        {
            var config = new
            {
                DataServerAddress = DataServerAddress,
                DataServerPort = DataServerPort,
                KeycloakAddress = KeycloakAddress,
                Login = Login,
                Password = Password,
                Realm = Realm,
                Scope = Scope,
                ClientId = ClientId,
            };

            var json = JsonConvert.SerializeObject(config);
            await File.WriteAllTextAsync(_filePathSettings, json);
        }

        private void LoadConfig()
        {
            string json = File.ReadAllText(_filePathSettings);
            var settings = JsonConvert.DeserializeObject<dynamic>(json);

            DataServerAddress = settings.DataServerAddress;
            DataServerPort = settings.DataServerPort;
            KeycloakAddress = settings.KeycloakAddress;
            Login = settings.Login;
            Password = settings.Password;
            Realm = settings.Realm;
            Scope = settings.Scope;
            ClientId = settings.ClientId;
        }

        private void CheckConnection()
        {
            GetInfoKeyCloak();

            //подключение и авторизация к keycloak
            if (TryGetUserAccessToken(Login, Password))
            {
                //MessageBox.Show("Подключение keycloak успешно");
                ConnectionMessage = "Подключение keycloak успешно";
                SaveSettings();
            }
            else
            {
                
            }
        }

        public async Task CheckConnectionAndSave()
        {
            GetInfoKeyCloak();

            //подключение и авторизация к keycloak
            if (TryGetUserAccessToken(Login, Password))
            {
                //MessageBox.Show("Подключение keycloak успешно");
                ConnectionMessage = "Подключение keycloak успешно";
                await SaveSettings();
            }
            else
            {
                MessageBoxService.ShowError("Ошибка keycloak", "Ошибка подключения keycloak. " +
                    "Неверно указан логин / пароль или другие настройки(Realm, Scope, ClientId)");
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
                AuthorizationUrl = $"http://{KeycloakAddress}/realms/{realm_}/protocol/openid-connect/token",
                ClientId = clientId_,
                Scope = scope_
            };
        }

        /// <summary>
        /// Запрос токена доступа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool TryGetUserAccessToken(string login, string password)
        {
            try
            {

                var keyCloakApi = new KeyCloakApi();
                UserAccessToken = keyCloakApi.GetToken(KeyCloakInfo, login, password, CancellationToken.None).Result;
                return true;
            }
            catch (Exception exception)
            {
                //MessageBox.Show("Неверно указан логин/пароль или другие настройки (Realm, Scope, ClientId)");
                ConnectionMessage = "Ошибка подключения keycloak. Неверно указан логин/пароль или другие настройки (Realm, Scope, ClientId)";
                return false;
            }
        }
    }
}
