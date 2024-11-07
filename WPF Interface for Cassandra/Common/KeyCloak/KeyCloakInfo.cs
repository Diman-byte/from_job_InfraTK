namespace Common.KeyCloak
{
    public sealed class KeyCloakInfo
    {
        public string AuthorizationUrl = "http://localhost:8080/realms/master/protocol/openid-connect/token"; //todo может стоит выделить протокол, ip, порт, реалм 
        public string ClientId = "ClientTestId"; //todo это имя приложения которое обращается для аутентификации НЕ стоит его передавать
        public string Scope = "openid";  //todo его можно тоже не передавать так как он не меняется для OAuth 2.0
    }
}