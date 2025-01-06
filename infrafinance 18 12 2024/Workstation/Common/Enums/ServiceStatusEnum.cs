using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workstation.Common.Enums
{
    public enum ServiceStatusEnum
    {
        [Description("Не используется")] NotUsed,
        [Description("Загрузка конфигурации")] ConfigurationLoading,
        [Description("Остановлен")] Stopped,
        [Description("Исполняется")] Running,
        [Description("Инициализация")] IsInit,
        [Description("Не инициализирована")] IsNotInit,
        [Description("На паузе")] Paused,
        [Description("Нет соединения")] NoConnection,
        [Description("Не активен")] NotActive,
        [Description("Не определен")] NotDef,
        [Description("Занят другим проектом")] BusyByAnotherProject,
    }
}
