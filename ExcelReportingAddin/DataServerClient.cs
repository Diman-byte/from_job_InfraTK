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
using System.Windows.Controls;
using static System.Net.WebRequestMethods;

namespace ExcelReportingAddin
{
    public class DataServerClient
    {
        private SettingsForm _settingsForm;
        private string _baseUrl;

        public DataServerClient()
        {
            _settingsForm = new SettingsForm();
            _baseUrl = $"http://{_settingsForm.DataServerAddress}:{_settingsForm.DataServerPort}/api";
        }

        /// <summary>
        /// Получить все активы
        /// </summary>
        /// <returns>Списов всех активов</returns>
        public async Task<List<AssetDto>> GetAllAssets()
        {
            var assets = new List<AssetDto>();

            string url = _baseUrl + "/GetAllAssets";

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

            var url = _baseUrl + "/GetAllTags";

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
        /// <param name="assetId">id актива</param>
        /// <returns>Списов тегов по активу</returns>
        public async Task<List<TagDto>> GetTagsByAssetId(Guid assetId)
        {
            var tags = new List <TagDto>();

            var url = _baseUrl + "/GetTagsByAssetId/" + assetId.ToString();

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
        /// Получить исторические значения тегов
        /// </summary>
        /// <param name="assetId">id актива</param>
        /// <param name="tagsId">список id тегов</param>
        /// <param name="startDate">Объект времени с началом диапазона</param>
        /// <param name="endDate">Объект времени с концом диапазона</param>
        /// <returns>Списов тегов со значениями</returns>
        public async Task<List<TagValDto>> GetTagsHistoricalData(Guid assetId, List<Guid> tagsId, DateTime startDate, DateTime endDate)
        {
            var tagsVal = new List<TagValDto>();

            // URL-кодирование, также известное как процентное кодирование.
            // Он используется для представления символов, которые нельзя использовать
            // в URL напрямую, путем их кодирования в специальные последовательности символов с префиксом %.
            //%20 — кодирует пробел
            //%3A — кодирует двоеточие

            // Чтобы закодировать объект DateTime в URL-кодированный формат, сначала преобразуйте его в строку(отформатировав его в нужный вид ISO "yyyy-MM-ddTHH:mm:ss"),
            // а затем примените URL-кодирование с помощью Uri.EscapeDataString

            // Форматируем дату в строку в формате ISO 8601
            string formattedDateTimeStart = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string formattedDateTimeStop = endDate.ToString("yyyy-MM-ddTHH:mm:ss");

            string encodedDateTimeStart = Uri.EscapeDataString(formattedDateTimeStart);
            string encodedDateTimeEnd = Uri.EscapeDataString(formattedDateTimeStop);

            string assetUrl = assetId.ToString();
            string tagsUrl = String.Join("%2C", tagsId); // между id тегов в url идет "%2C"
            var url = _baseUrl + "/GetTagsHistoricalData?assetId=" + assetUrl + "&tagsId=" + tagsUrl + "&timeFrom=" + encodedDateTimeStart + "&timeTo=" + encodedDateTimeEnd;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tagsVal = JsonConvert.DeserializeObject<List<TagValDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }

            return tagsVal;
        }

        /// <summary>
        /// Получить все группы активов
        /// </summary>
        /// <returns>Списов всех групп активов</returns>
        public async Task<List<AssetGroupDto>> GetAllAssetGroups()
        {
            List<AssetGroupDto> groups = new List<AssetGroupDto>();

            string url = _baseUrl + "/GetAllAssetGroups";

            using(var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    groups = JsonConvert.DeserializeObject<List<AssetGroupDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            return groups;
        }

        /// <summary>
        /// Получить исторические значения тегов в срезе
        /// </summary>
        /// <param name="assetId">id актива</param>
        /// <param name="tagsId">список id тегов</param>
        /// <param name="startDate">Объект времени с началом диапазона</param>
        /// <param name="endDate">Объект времени с концом диапазона</param>
        /// <param name="pointsAmount">Количество точек среза</param>
        /// <param name="sliceType">Тип среза</param>
        /// <returns>Списов тегов со значениями среза</returns>
        public async Task<List<TagValDto>> GetTagsHistoricalSliceData(Guid assetId, List<Guid> tagsId, DateTime startDate, DateTime endDate, int pointsAmount, string sliceType)
        {
            List<TagValDto> tagsVal = new List<TagValDto>();

            string formattedDateTimeStart = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string formattedDateTimeStop = endDate.ToString("yyyy-MM-ddTHH:mm:ss");

            string encodedDateTimeStart = Uri.EscapeDataString(formattedDateTimeStart);
            string encodedDateTimeEnd = Uri.EscapeDataString(formattedDateTimeStop);

            string assetUrl = assetId.ToString();
            string tagsUrl = String.Join("%2C", tagsId); // между id тегов в url идет "%2C"
            var url = _baseUrl + "/GetTagsHistoricalSliceData?assetId=" + assetUrl + "&tagsId=" + tagsUrl + "&timeFrom=" + encodedDateTimeStart + "&timeTo=" + encodedDateTimeEnd +
                "&pointsAmount=" + pointsAmount + "&sliceType" + sliceType;

            using(var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tagsVal = JsonConvert.DeserializeObject<List<TagValDto>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }

            return tagsVal;
        }


    }
}
