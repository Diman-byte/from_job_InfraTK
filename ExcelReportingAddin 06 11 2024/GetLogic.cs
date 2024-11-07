using ExcelReportingAddin.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ExcelReportingAddin
{
    public class GetLogic
    {
        public SettingsForm _settingsForm;
        public string base_url;

        public GetLogic()
        {
            var _settingsForm = new SettingsForm();
            base_url = $"http://{_settingsForm.DataServerAddress}:{_settingsForm.DataServerPort}/api";
        }

        /// <summary>
        /// Получить все активы
        /// </summary>
        /// <returns>Списов всех активов</returns>
        public async Task<List<AssetDto>> GetAllAssets()
        {
            var assets = new List<AssetDto>();

            string url = base_url + "/GetAllAssets";

            using (var client = new HttpClient()) // cоздаем новый экземпляр HttpClient, который будет использоваться для отправки HTTP-запросов
            {
                // для авторизации
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                // устанавливает заголовок авторизации для клиента, используя токен доступа _accessToken (в формате Bearer).
                // Это нужно для доступа к защищенным ресурсам API

                HttpResponseMessage response = await client.GetAsync(url); //  Отправляет асинхронный GET-запрос на указанный URL.
                                                                           //  await приостанавливает выполнение метода до получения ответа, не блокируя поток выполнения.


                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(); // Читает содержимое ответа в виде строки JSON асинхронно
                    assets = JsonConvert.DeserializeObject<List<AssetDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }

            return assets;
        }

        /// <summary>
        /// Получить все теги
        /// </summary>
        /// <returns>Списов всех тегов</returns>
        public async Task<List<TagDto>> GetAllTags()
        {
            var tags = new List<TagDto>();

            var url = base_url + "/GetAllTags";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tags = JsonConvert.DeserializeObject<List<TagDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }

            }

            return tags;
        }

        /// <summary>
        /// Получить теги по активу
        /// </summary>
        /// <param name="assetId">id тега</param>
        /// <returns>Списов тегов по активу</returns>
        public async Task<List<TagDto>> GetTagsByAssetId(Guid assetId)
        {
            var tags = new List <TagDto>();

            var url = base_url + "/GetTagsByAssetId/" + assetId.ToString();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tags = JsonConvert.DeserializeObject<List<TagDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }

            return tags;
        }



    }
}
