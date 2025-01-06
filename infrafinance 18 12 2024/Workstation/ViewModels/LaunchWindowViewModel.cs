using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Workstation.Views;
using Workstation.Models.CommonModels;
using Workstation.Models.CommonModels.MsgLog;
using Workstation.Models;

namespace Workstation.ViewModels
{
    public class LaunchWindowViewModel : ViewModelBase
    {
        // ReactiveCommand — это компонент из библиотеки ReactiveUI, которая предназначена для
        // построения реактивных пользовательских интерфейсов.

        // ReactiveCommand используется для:
        //Описания действия, которое будет выполнено, например, при нажатии кнопки.
        //Управления состоянием (например, можно задавать, доступна ли команда).
        //Упрощения обработки асинхронных операций.

        // Это свойство только для чтения (get), то есть его значение задаётся только в конструкторе.
        public ReactiveCommand<IManageWindow, Unit> AuthUserCommand { get; }


        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool IsSavePass { get; set; }

        string serverPort;

        public LaunchWindowViewModel()
        {
            // Здесь создаётся команда, которая вызывает метод AuthUser.
            AuthUserCommand = ReactiveCommand.Create<IManageWindow>(AuthUser);

            LaunchWindowModel.TryReadConfig(out string serverHost, out serverPort);
            Host = serverHost;

            LaunchWindowModel.TryReadUserCredential(out string login, out string pass, out bool isSave);
            Login = login;
            Password = pass;
            IsSavePass = isSave;
        }

        /// <summary>
        /// метод авторизации
        /// </summary>
        /// <param name="manageWindow">Принимает в качестве параметра объект IManageWindow 
        /// — интерфейс для управления окном (например, чтобы закрыть окно после авторизации).</param>
        private void AuthUser(IManageWindow manageWindow)
        {
            try
            {
                if (!LaunchWindowModel.TryGetKeyCloakInfo(Host, serverPort)) return;

                if (!LaunchWindowModel.TryGetUserAccessToken(Login, Password)) return;

                if (!LaunchWindowModel.TryGetDBInfo(Host, serverPort)) return;

                //LaunchWindowModel.TryWriteConfig(Host, serverPort);

                if (IsSavePass) LaunchWindowModel.TryWriteUserCredential(Login, Password, IsSavePass);

                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();

                MsgLogShow.ShowMsg(new Common.MsgLog.MsgLogClass() { LogText = "успех, здесь должно открываться окно MainWindow", LogDetails = "detali", TypeLog = LogLevel.Info });


                //manageWindow.CloseWindow();
            }
            catch (Exception ex)
            {
                MsgLogShow.ShowMsg(new Common.MsgLog.MsgLogClass() { LogText = "Ошибка", LogDetails = ex.ToString(), TypeLog = LogLevel.Error });
            }
        }
    }
}
