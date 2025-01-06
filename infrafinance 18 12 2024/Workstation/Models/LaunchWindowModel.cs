using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workstation.Common.MsgLog;
using Workstation.Common.CommonModels;
using Workstation.Common.KeyCloak;
using Workstation.Models.CommonModels.MsgLog;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;
using Workstation.Models.Main;
using Grpc.Net.Client;
using Grpc.InfoApi;
using System.Threading;


//using Grpc.InfoApi;

namespace Workstation.Models
{
    public static class LaunchWindowModel
    {
        /// <summary>
        /// Чтение конфигурации системы из файла Config.xml 
        /// </summary>
        /// <returns></returns>
        public static bool TryReadConfig(out string serverHost, out string serverPort)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            // System.Reflection.Assembly.GetEntryAssembly():
            //Возвращает объект, представляющий сборку, которая содержит метод Main().
            //Это та сборка, с которой начинается выполнение программы.
            // .Location: Возвращает полный путь(включая имя файла) к файлу сборки на диске.

            try
            {
                // Инициализация выходных параметров (out) значениями по умолчанию (string.Empty).
                // Это необходимо для предотвращения ошибок использования неинициализированных переменных.
                serverHost = string.Empty;
                serverPort = string.Empty;

                var sharedConfigPath = Path.Combine(path, "SharedAppSettings.json");
                // JObject.Parse преобразует строку JSON в объект типа JObject, который представляет собой универсальную, гибкую структуру для работы с JSON.
                // Позволяет работать с JSON-данными как с динамическими структурами, где можно обращаться к свойствам, даже если их структура заранее неизвестна.

                // в сравнении с JsonConvert.DeserializeObject<T>(), преобразует строку JSON в объект определённого типа (T),
                // который должен быть известен на этапе компиляции.
                var deserializeSettings = JObject.Parse(File.ReadAllText(sharedConfigPath));
                // Извлечение секции InfoServiceConfiguration из JSON-файла
                var jTokenInfoSettings = deserializeSettings["InfoServiceConfiguration"];

                if (jTokenInfoSettings is null) return false;

                // Извлечение значений Host и Port из секции InfoServiceConfiguration.
                // .Value<string>() для преобразования Jtoken в строки
                // Оператор ! указывает, что значение не может быть null (требует осторожности).
                serverHost = jTokenInfoSettings["Host"]!.Value<string>();
                serverPort = jTokenInfoSettings["Port"]!.Value<string>();

                return true;
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Ошибка чтения файла SharedAppSettings.json", TypeLog = LogLevel.Error });

                serverHost = string.Empty;
                serverPort = string.Empty;

                return false;
            }
        }

        /// <summary>
        /// Запись конфигурации системы в файл Config.xml 
        /// </summary>
        /// <returns></returns>
        public static bool TryWriteConfig(string serverHost, string serverPort)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            var configPath = Path.Combine(path, "Config.xml");
            try
            {
                if (File.Exists(configPath))
                {
                    List<ConfigXML> configXML = new List<ConfigXML>();
                    configXML.Add(new ConfigXML()
                    { Name = "AnalyticsServerHost", Desc = "Ip адрес сервера Analytics Server", ValType = "string", Val = serverHost });
                    configXML.Add(new ConfigXML()
                    { Name = "AnalyticsServerPort", Desc = "Ip порт сервера Analytics Server", ValType = "string", Val = serverPort });

                    // Создается объект XmlSerializer, который будет использоваться для сериализации (преобразования) списка configXML в формат XML.
                    XmlSerializer formatter = new XmlSerializer(typeof(List<ConfigXML>));

                    // Открывается поток для записи данных в файл Config.xml
                    using (FileStream fs = new FileStream(configPath, FileMode.Create))
                    {
                        // FileMode.Create: Если файл уже существует, он будет перезаписан. Если нет — будет создан новый файл.

                        formatter.Serialize(fs, configXML);
                        // Метод Serialize объекта formatter записывает данные из configXML в файл в формате XML.
                    }

                    return true;
                }
                else
                {
                    MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Файл Config.xml \n не найден", TypeLog = LogLevel.Error });
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Ошибка записи в \n файл Config.xml", TypeLog = LogLevel.Error });
                return false;
            }
        }

        /// <summary>
        /// Чтение учетных данных пользователя
        /// </summary>
        /// <returns></returns>
        public static bool TryReadUserCredential(out string login, out string pass, out bool isSave)
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            try
            {
                login = string.Empty;
                pass = string.Empty;
                isSave = default;

                var userCredentialPath = Path.Combine(path, "UserCredential.xml");

                if (!File.Exists(userCredentialPath))
                {
                    UserCredentialXML configXML = new UserCredentialXML();
                    /* configXML.Login = "usertest";
                     configXML.Pass = "test1234";*/

                    XmlSerializer formatter = new XmlSerializer(typeof(UserCredentialXML));
                    using (FileStream fs = new FileStream(userCredentialPath, FileMode.CreateNew))
                    {
                        // FileMode.CreateNew - Если файла с указанным именем не существует, то он будет создан.
                        // перезаписываться существующий не будет
                        formatter.Serialize(fs, configXML);
                    }
                }

                if (File.Exists(userCredentialPath))
                {
                    UserCredentialXML configXML;

                    XmlSerializer formatter = new XmlSerializer(typeof(UserCredentialXML));
                    using (FileStream fs = new FileStream(userCredentialPath, FileMode.Open))
                    {
                        // FileMode.Open - если файла не существует, будет выброшена ошибка
                        configXML = (UserCredentialXML)formatter.Deserialize(fs);
                    }

                    login = configXML.Login;
                    pass = configXML.Pass;
                    isSave = configXML.IsSave;
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Ошибка чтения файла \n UserCredential.xml", TypeLog = LogLevel.Error });
                login = string.Empty;
                pass = string.Empty;
                isSave = default;

                return false;
            }
        }

        /// <summary>
        /// Запись учетных данных пользователя
        /// </summary>
        /// <returns></returns>
        public static bool TryWriteUserCredential(string login, string pass, bool isSave)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            var userCredentialPath = Path.Combine(path, "UserCredential.xml");
            try
            {
                if (File.Exists(userCredentialPath))
                {
                    var configXML = new UserCredentialXML
                    {
                        Login = login,
                        Pass = pass,
                        IsSave = isSave
                    };

                    XmlSerializer formatter = new XmlSerializer(typeof(UserCredentialXML));
                    using (FileStream fs = new FileStream(userCredentialPath, FileMode.Create))
                    {
                        formatter.Serialize(fs, configXML);
                    }
                }
                else
                {
                    MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Файл UserCredential.xml \n не найден", TypeLog = LogLevel.Error });
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Ошибка записи файла \n UserCredential.xml", TypeLog = LogLevel.Error });
                return false;
            }
        }

        /// <summary>
        /// запрос информации о сервисе KeyCloak
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool TryGetKeyCloakInfo(string host, string port)
        {
            try
            {
                // Создает канал связи между клиентом и сервером, нужен для взаимодействия с сервером через gRPC.
                var channel = GrpcChannel.ForAddress($"http://{host}:{port}"); // для этого нужен Grpc.Net.Client;

                // InfoApi.InfoApiClient: Это клиентский класс, сгенерированный из файла .proto
                // Он используется для вызова методов gRPC-сервиса, описанных в этом .proto
                var client = new  InfoApi.InfoApiClient(channel); // для этого нужен был файл прото InfoApi, который скопировал в проект и добавил в файле проекта
                // а также еще 2 пакета nuget


                var keyCloakInfoApi = client.GetKeyCloakInfo(new EmptyArgRequest());
                // new EmptyArgRequest(): Пустой запрос, если метод не требует входных параметров.

                StaticData.KeyCloakInfo = new Common.KeyCloak.KeyCloakInfo()
                {
                    AuthorizationUrl = $"{keyCloakInfoApi.BaseUri}/realms/{keyCloakInfoApi.Realm}/protocol/openid-connect/token",
                    ClientId = keyCloakInfoApi.ClientId,
                    Scope = keyCloakInfoApi.Scope
                };

                return true;
            }
            catch (Exception exception)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = "Ошибка соединения с \n InfoService", TypeLog = LogLevel.Error });
                return false;
            }
        }

        /// <summary>
        /// Запрос токена доступа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool TryGetUserAccessToken(string login, string pass)
        {
            try
            {
                var keyCloakApi = new KeyCloakApi();
                StaticData.UserAccessToken = keyCloakApi.GetToken(StaticData.KeyCloakInfo, login, pass, CancellationToken.None).Result;
                // CancellationToken используется для управления и отслеживания отмены асинхронных операций. Он позволяет передать в метод сигнал об отмене операции.
                // CancellationToken.None указывает, что операция не может быть отменена, и токен всегда находится в состоянии IsCancellationRequested == false.

                 StaticData.User = login;

                return true;
            }
            catch (Exception exception)
            {
                MsgLogShow.ShowMsg(new MsgLogClass()
                {
                    LogText = "Неверно указан логин/пароль или настройки пользователя",
                    TypeLog = LogLevel.Warn,
                    LogDetails = exception.InnerException.Message
                });
                return false;
            }
        }

        /// <summary>
        /// Запрос информации о базе конфигурации и базе проекта
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool TryGetDBInfo(string host, string port)
        {
            try
            {
                var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
                var client = new InfoApi.InfoApiClient(channel);

                var dbConfigInfo = client.GetConfigDbInfo(new ArgRequest());

                StaticData.ConfigDBInfo = new Common.CommonModels.DataBaseInfo()
                {
                    DataBase = dbConfigInfo.DataBase,
                    Host = dbConfigInfo.DbConfig.Host,
                    Port = dbConfigInfo.DbConfig.Port.ToString(),
                    Password = dbConfigInfo.DbConfig.Password,
                    User = dbConfigInfo.DbConfig.UserName,
                    CommandTimeout = 30,
                    IsPoolingEnabled = true
                };

                var dbProjInfo = client.GetProjectDbServerInfo(new ArgRequest());

                StaticData.ProjDBInfo = new Common.CommonModels.DataBaseInfo()
                {
                    DataBase = dbProjInfo.Prefix,
                    Host = dbProjInfo.DbConfig.Host,
                    Port = dbProjInfo.DbConfig.Port.ToString(),
                    Password = dbProjInfo.DbConfig.Password,
                    User = dbProjInfo.DbConfig.UserName,
                    CommandTimeout = 30,
                    IsPoolingEnabled = true
                };

                var dbMessageInfo = client.GetMessageLogDbServerInfo(new ArgRequest());

                StaticData.MessageLogDBInfo = new Common.CommonModels.DataBaseInfo()
                {
                    DataBase = dbMessageInfo.Prefix,
                    Host = dbMessageInfo.DbConfig.Host,
                    Port = dbMessageInfo.DbConfig.Port.ToString(),
                    Password = dbConfigInfo.DbConfig.Password,
                    User = dbMessageInfo.DbConfig.UserName,
                    CommandTimeout = 30,
                    IsPoolingEnabled = true
                };

                return true;
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new MsgLogClass() { LogText = ListMsg.ApiError, TypeLog = LogLevel.Error, LogDetails = ex.ToString() });
                return false;
            }
        }
    }
}
