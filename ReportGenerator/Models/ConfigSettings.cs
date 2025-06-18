using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Models
{
    public class ConfigSettings
    {
        public string DataServerAddress { get; set; }
        public string DataServerPort { get; set; }
        public string KeyCloakAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Realm { get; set; }
        public string Scope { get; set; }
        public string ClientId { get; set; }

    }
}
