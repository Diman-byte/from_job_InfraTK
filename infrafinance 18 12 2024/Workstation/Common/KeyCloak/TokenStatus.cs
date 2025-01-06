using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Workstation.Common.KeyCloak
{
    public sealed class TokenStatus
    {
        /// <summary>
        /// валиден ли еще токен
        /// </summary>
        [JsonPropertyName("active")]
        public bool IsActive { get; set; }
    }
}
