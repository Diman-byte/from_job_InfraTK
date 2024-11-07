using System.Text.Json.Serialization;

namespace ExcelReportingAddin.KeyCloak
{
    public sealed class JwtToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("id_token")]
        public string TokenId { get; set; }

        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
