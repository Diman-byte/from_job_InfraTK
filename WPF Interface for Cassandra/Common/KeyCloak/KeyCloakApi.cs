using System;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Common.KeyCloak
{
    /// <summary>
    /// Класс для работы с сервером авторизации
    /// </summary>
    public sealed class KeyCloakApi
    {
        /// <summary>
        /// Получает новый JWT токен доступа, если он неверен или истек
        /// </summary>
        /// <exception cref="ArgumentNullException">если login или password is null</exception>
        /// <exception cref="InvalidCredentialException">Были предоставлены неверные учетные данные(логин или пароль)</exception>
        public async Task<JwtToken> GetToken(KeyCloakInfo keyCloakInfo, string login, string password, CancellationToken cancellationToken)
        {
            try
            {
                //todo возможно стоит добавить client secretдля приложений
                var restClient = new RestClient();
                var request = new RestRequest(keyCloakInfo.AuthorizationUrl, Method.Post);

                var authorizationHeaderEncoded = BuildAuthorizationHeaderEncoded(keyCloakInfo.ClientId, login);
                request.AddHeader("Authorization", authorizationHeaderEncoded);
                request.AddHeader("Connection", "keep-alive");

                request.AddParameter("grant_type", "password");
                request.AddParameter("username", login);
                request.AddParameter("password", password);
                request.AddParameter("scope", keyCloakInfo.Scope);
                var response = await restClient.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
                if (response.IsSuccessful)
                {
                    return JsonSerializer.Deserialize<JwtToken>(response.Content);
                }

                throw new Exception(response.Content);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<bool> CheckToken(string tokenUrl, JwtToken jwtToken, CancellationToken cancellationToken)
        {
            //todo можно делать online и offline проверку токенов
            //нужно публичный ключ запрашивать для offline https://habr.com/ru/company/alfastrah/blog/651861/

            //todo для провреки токенов можно использовать только confidential client
            //todo т.е. нужно дополнительно создать ClientId для сonfidential client и хранить сгенерированный client_secret,чтобы проверять токены
            try
            {
                //todo их надо видимо добавить в конфиги и ограничить бы этого клиента ТОЛЬКО проверкой JWTtoken 
                var verifyClientId = "VerifyTokenClientId";
                var clientSecret = "UehYBDoXCbnwG23wKpTktY4ps5HqGRzG";
                var restClient = new RestClient();
                var request = new RestRequest(tokenUrl, Method.Post);
                request.AddParameter("token", jwtToken.AccessToken);
                request.AddParameter("grant_type", "client_credentials");
                request.AddParameter("client_id", verifyClientId);
                request.AddParameter("client_secret", clientSecret);
                var response = await restClient.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
                var status = JsonSerializer.Deserialize<TokenStatus>(response.Content);
                return status.IsActive;
            }
            catch (Exception exception)
            {
                //todo логгер ошибок
                return false;
            }
        }

        /// <summary>
        /// Производит кодирования строки в Base64 формат
        /// </summary>
        /// <param name="plainText">строка для кодироования</param>
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Создает заголовок Authorization для получение токена доступа через Password Credentials
        /// </summary>
        /// <param name="clientId">id клиента</param>
        /// <param name="login">логин в F5Platform</param>
        /// <returns>Заголовок Authorization закодированный в Base64</returns>
        /// <exception cref="ArgumentNullException">Когда clientId или login is null</exception>
        private string BuildAuthorizationHeaderEncoded(string clientId, string login)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (login == null) throw new ArgumentNullException(nameof(login));

            var authorizationHeaderEncoded = clientId + ":" + login;
            authorizationHeaderEncoded = "Basic " + Base64Encode(authorizationHeaderEncoded);
            return authorizationHeaderEncoded;
        }
    }
}