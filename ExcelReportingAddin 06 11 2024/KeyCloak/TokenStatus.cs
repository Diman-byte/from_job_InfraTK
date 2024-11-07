using System.Text.Json.Serialization;


namespace ExcelReportingAddin.KeyCloak
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
