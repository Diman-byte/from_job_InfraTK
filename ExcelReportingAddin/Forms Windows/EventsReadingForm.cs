using ExcelReportingAddin.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using static ExcelReportingAddin.DataReadingForm;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

namespace ExcelReportingAddin.Forms_Windows
{
    public partial class EventsReadingForm : Form
    {
        // хранит путь файла с данными о диапазоне времени и других параметрах
        private string _configFilePath;

        // объект, который содержит данные о диапазоне времени
        private ConfigReadEvents _configReadEvents;

        private DataServerClient _dataServerClient;

        // хранит выбранный актив
        //private List<Guid> _selectedAssets = new List<Guid>();
        // для хранения имени актива
        List <Asset_Class> _selectedAssetClass = new List<Asset_Class>();
        //private Dictionary<Guid, string> _selectedAssetsDict = new Dictionary<Guid, string>();

        public EventsReadingForm()
        {
            InitializeComponent();

            _dataServerClient = new DataServerClient();

            PopulateTreeView();

            // Чтобы добавить обработчик события NodeMouseClick для TreeView
            // Подписываемся на событие NodeMouseDoubleClick для treeAssets
            treeAssets.NodeMouseClick += treeAssets_NodeMouseClick;

            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запрет изменения размера
            this.MaximizeBox = false; // Убираем кнопку максимизации

            // обновляю всегда параметры в окошке
            UpdateStartDateInputControls();
            UpdateStopDateInputControls();
            //ParamStartSign();
            //ParamEndSign();

            SelectedAssetsCLB.ValueMember = "Id";
            SelectedAssetsCLB.DisplayMember = "Name";



            InitializeConfigFile();
            LoadConfig();

           

            // добавляю обработчик события закрытия окна
            this.FormClosing += DataReadingForm_FormClosing;

        }

        /// <summary>
        /// Заполнить TreeView (treeAssets)
        /// </summary>
        private async void PopulateTreeView()
        {
            ShowLoadingIndicator();

            treeAssets.Nodes.Clear(); // Перед обновлением данных очищай дерево, чтобы избежать дублирования      

            // добавляю активы без группы
            List<Guid?> listGroups = await GetAssetGroupId();
            if (listGroups.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                TreeNode rootNood = new TreeNode("Объекты без группы") { Tag = "group" };
                treeAssets.Nodes.Add(rootNood);
                AddAssets(rootNood, Guid.Parse("00000000-0000-0000-0000-000000000000"));
            }

            // добавляю корневые группы
            List<AssetGroupDto> groupsAll = await _dataServerClient.GetAllAssetGroups();
            List<AssetGroupTreeNodeDto> nodes = BuildTree(groupsAll); // получаем все корневые группы (у которых нет родителя)

            foreach (var rootNode in nodes)
            {
                TreeNode treeNode = new TreeNode(rootNode.Name) { Tag = "group" };
                // добавляю активы из этой группы
                AddAssets(treeNode, rootNode.Id);
                // добавляю дочерние узлы (группы)
                AddChildNodes(treeNode, rootNode.Children);
                treeAssets.Nodes.Add(treeNode);
            }

            HideLoadingIndicator();
            // TreeView состоит из узлов (Nodes), которые могут содержать вложенные узлы.
            // Это позволяет отображать иерархическую структуру данных.
            // Каждый узел представлен объектом типа TreeNode

            // Используй свойство Nodes для добавления узлов в корень дерева,
            // а также для добавления дочерних узлов к конкретным узлам
        }

        /// <summary>
        /// Рекурсивно добавляем дочерние узлы к корневым в treeView
        /// </summary>
        /// <param name="parentNode">родительская нода (группа актива)</param>
        /// <param name="children">список потомков этой группы</param>
        private void AddChildNodes(TreeNode parentNode, List<AssetGroupTreeNodeDto> children)
        {
            foreach (var child in children)
            {
                TreeNode childNode = new TreeNode(child.Name) { Tag = "group" };
                AddAssets(childNode, child.Id);
                // Рекурсивно добавляем дочерние элементы
                AddChildNodes(childNode, child.Children);
                // Добавляем дочерний узел к родительскому
                parentNode.Nodes.Add(childNode);
            }
        }

        /// <summary>
        /// Добавить активы к группам активов в TreeView
        /// </summary>
        /// <param name="parentNode">родительская нода (группа актива)</param>
        /// <param name="asset_group_id">id этой группы актива</param>
        private async void AddAssets(TreeNode parentNode, Guid? asset_group_id)
        {
            var assets = await _dataServerClient.GetAllAssets();

            foreach (var asset in assets)
            {
                if (asset.AssetGroupId == asset_group_id)
                {
                    TreeNode subNode = new TreeNode(asset.Name.ToString()) { Tag = asset };
                    // свойство Tag для хранения связанного объекта Asset.
                    // Это поможет получать данные об активе при взаимодействии с узлом.
                    parentNode.Nodes.Add(subNode);
                }
            }
        }

        /// <summary>
        /// Этот метод преобразует List<AssetGroupDto> в список корневых узлов. Метод строит дерево на основе списка групп, 
        /// связывая их по принципу «родитель-дочерний» и возвращая корневые элементы.
        /// </summary>
        /// <param name="groups">список групп, полученных по REST</param>
        /// <returns>Списов групп активов в иерархическом виде</returns>
        private List<AssetGroupTreeNodeDto> BuildTree(List<AssetGroupDto> groups)
        {
            Dictionary<Guid, AssetGroupDto> groupDictionary = groups.ToDictionary(g => g.Id); // Создается словарь (Dictionary), где ключами являются идентификаторы групп (g.Id),
                                                                                              // а значениями – сами объекты AssetGroupDto.Это позволяет быстро находить группу по её идентификатору.

            // Словарь для связи ID -> дочерние узлы
            Dictionary<Guid, AssetGroupTreeNodeDto> treeNods = groups.ToDictionary(
                g => g.Id,
                g => new AssetGroupTreeNodeDto { Id = g.Id, Name = g.Name });

            foreach (var group in groups)
            {
                if (groupDictionary.ContainsKey((Guid)group.ParentId) && group.ParentId != Guid.Empty) //  проверяет наличие родителя этой группы в словаре groupDictionary; у группы есть родительский элемент
                {
                    treeNods[(Guid)group.ParentId].Children.Add(treeNods[group.Id]); // Если группа имеет родителя, то текущий объект добавляется к списку детей этого родителя
                }
            }

            return treeNods.Values.Where(node => groupDictionary[node.Id].ParentId == Guid.Empty).ToList();
            // Возвращается список узлов, являющихся корнями дерева. Для этого выбираются те узлы, у которых значение поля ParentId
            // равно Guid.Empty (то есть они не имеют родителей), используя LINQ-запросы через словарь groupDictionary.
        }

        /// <summary>
        /// Получить все группы активов
        /// </summary>
        /// <returns>Списов всех групп активов</returns>
        private async Task<List<Guid?>> GetAssetGroupId()
        {
            var list_asset_group_id = new List<Guid?>();

            try
            {
                var assets = await _dataServerClient.GetAllAssets();
                //MessageBox.Show($"Активы получены");

                // из списка активов создаю список групп активов, но так, чтобы они не повторялись(были уникальными)
                foreach (var asset in assets)
                {
                    // если не содержит еще такой группы, то добавляю
                    if (!list_asset_group_id.Contains(asset.AssetGroupId))
                    {
                        list_asset_group_id.Add(asset.AssetGroupId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении активов(групп активов): {ex.Message}");
            }

            return list_asset_group_id;
        }

        /// <summary>
        /// Обработчик события. вызывается при одном щелчке по узлу TreeView
        /// </summary>
        private void treeAssets_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() != "group")
            { // двойное нажатие по группе активов не будем обрабатывать
                // Получаем объект актива, связанный с выбранным узлом
                var selectedAsset = e.Node.Tag as AssetDto;

                

                // если еще не содержит
                if (!_selectedAssetClass.Any(a => a.Id == selectedAsset.Id))
                {
                    // записываем выбранный актив в наше поле
                    //_selectedAssetClass.Add(selectedAsset.Id);
                    _selectedAssetClass.Add(
                        new Asset_Class{ Id = selectedAsset.Id , Name = selectedAsset.Name });
                   
                }
                else
                {
                    MessageBox.Show("Выбранный актив уже добавлен");
                    return;
                }


                
                // очистим перед изменением
                SelectedAssetsCLB.Items.Clear();
                //обновить check list box с выбранными активами
                PopulateSelectedAssets();
            }
        }

        private void PopulateSelectedAssets()
        {
            ShowLoadingIndicator();

            if(_selectedAssetClass.Count > 0)
            {
                int i = 0;
                foreach(var asset in _selectedAssetClass)
                {
                    SelectedAssetsCLB.Items.Add(asset);
                    SelectedAssetsCLB.SetItemCheckState(i++, CheckState.Checked);
                }

            }

            HideLoadingIndicator();
        }

        /// <summary>
        /// Метод получения получения даты начала диапазона с окна
        /// </summary>
        /// <returns>Объект времени начала диапазона</returns>
        private DateTime StartDate()
        {
            // сейчас без проверок на ввод

            // асболютный режим ввода даты
            if (StartDateAbs.Checked)
            {
                DateTime date = dateTimePickerStart.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourStart.Text);
                int minute = int.Parse(MinStart.Text);
                int second = int.Parse(SecStart.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                return dateTime;
            }
            // относительный
            else
            {
                int day = int.Parse(DayStart.Text);
                int hour = int.Parse(HourStart.Text);
                int minute = int.Parse(MinStart.Text);
                int second = int.Parse(SecStart.Text);

                DateTime now = DateTime.Now;
                DateTime res = DateTime.Now;

                // если "Сейчас"
                if (StartSign.SelectedItem.ToString() == "Сейчас")
                {
                    res = now;
                }

                // если "Текущая смена"
                if (StartSign.SelectedItem.ToString() == "Текущая смена")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19) {
                        res = DateTime.Today.AddHours(8);
                    }
                    else
                    {
                        // ночная смена
                        res = DateTime.Today.AddHours(19);
                    }
                }

                // если "Смена плюс"
                if (StartSign.SelectedItem.ToString() == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(8);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                }

                // если "Смена минус"
                if (StartSign.SelectedItem.ToString() == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(8);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                }

                // если "Сейчас плюс"
                if (StartSign.SelectedItem.ToString() == "Сейчас плюс")
                {
                    res = now.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сейчас минус"
                if (StartSign.SelectedItem.ToString() == "Сейчас минус")
                {
                    res = now.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }
                // если "Сегодня плюс"
                if (StartSign.SelectedItem.ToString() == "Сегодня плюс")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня минус"
                if (StartSign.SelectedItem.ToString() == "Сегодня минус")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }

                return res;
            }
        }

        /// <summary>
        /// Метод получения получения даты конца диапазона с окна
        /// </summary>
        /// <returns>Объект времени конца диапазона</returns>
        private DateTime EndDate()
        {
            if (EndDateAbs.Checked)
            {
                DateTime date = dateTimePickerStop.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourEnd.Text);
                int minute = int.Parse(MinEnd.Text);
                int second = int.Parse(SecEnd.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                return dateTime;
            }
            // относительный
            else
            {
                int day = int.Parse(DayEnd.Text);
                int hour = int.Parse(HourEnd.Text);
                int minute = int.Parse(MinEnd.Text);
                int second = int.Parse(SecEnd.Text);

                DateTime now = DateTime.Now;
                DateTime res = DateTime.Now;

                // если "Сейчас"
                if (StartSign.SelectedItem.ToString() == "Сейчас")
                {
                    res = now;
                }

                // если "Текущая смена"
                if (StartSign.SelectedItem.ToString() == "Текущая смена")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        res = DateTime.Today.AddHours(19);
                    }
                    else
                    {
                        // ночная смена
                        res = DateTime.Today.AddDays(1).AddHours(8);
                    }
                }

                // если "Смена плюс"
                if (StartSign.SelectedItem.ToString() == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddDays(1).AddHours(8);
                        res = res_1.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                    }
                }

                // если "Смена минус"
                if (StartSign.SelectedItem.ToString() == "Смена плюс")
                {
                    // дневная смена
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
                        var res_1 = DateTime.Today.AddHours(19);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                    else
                    {
                        // ночная смена
                        var res_1 = DateTime.Today.AddDays(1).AddHours(8);
                        res = res_1.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                    }
                }

                // если "Сейчас минус"
                if (EndSign.SelectedItem.ToString() == "Сейчас минус")
                {
                    res = now.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }
                // если "Сейчас плюс"
                if (EndSign.SelectedItem.ToString() == "Сейчас плюс")
                {
                    res = now.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня плюс"
                if (EndSign.SelectedItem.ToString() == "Сегодня плюс")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(day).AddHours(hour).AddMinutes(minute).AddSeconds(second);
                }
                // если "Сегодня минус"
                if (EndSign.SelectedItem.ToString() == "Сегодня минус")
                {
                    DateTime tmp = DateTime.Today;
                    res = tmp.AddDays(-day).AddHours(-hour).AddMinutes(-minute).AddSeconds(-second);
                }

                return res;
            }
        }

        /// <summary>
        /// Метод задание поля _configFilePath и создания файла(если не существует), который будет содержать параметры окна "Чтение данных"
        /// </summary>
        private void InitializeConfigFile()
        {
            string excelFileName = Globals.ThisAddIn.Application.ActiveWorkbook?.Name ?? "default"; // объект Application предоставляет доступ к активному экземпляру Excel
                                                                                                    // nullable-оператор (?), Если ActiveWorkbook не null (т.е. активная книга существует),
                                                                                                    // тогда будет возвращено значение свойства Name, Если же ActiveWorkbook равен null, то оператор ?.
                                                                                                    // просто вернет null и предотвратит выброс исключения
                                                                                                    // (??) оператор объединения с null. Если значение слева от ?? равно null, будет использовано значение справа — строка "default".
            _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                .ApplicationData), excelFileName + "_Events.conf"); // Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) Возвращает путь к папке AppData\Roaming текущего пользователя.
                                                                    // Используется для хранения пользовательских настроек и данных, которые должны быть доступны для приложения независимо от его местоположения.
                                                                    // Это путь, где приложение может сохранять конфигурационные файлы, данные или кеш.

            if (!File.Exists(_configFilePath))
            {
                // Создаем пустой JSON-файл, если его нет
                File.WriteAllText(_configFilePath, "{}");

                // Делаем файл скрытым
                // File.SetAttributes(_configFilePath, FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// Метод загрузки файла, который будет содержать параметры окна "Чтение данных"
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(_configFilePath) && File.ReadAllText(_configFilePath) != "{}")
            {
                string json = File.ReadAllText(_configFilePath);
                _configReadEvents = JsonConvert.DeserializeObject<ConfigReadEvents>(json);
            }
            else
            {
                _configReadEvents = new ConfigReadEvents();

                //задаю по умолчанию параметры начала интервала как минус 1 день
                _configReadEvents.TimeSettingsStart.Sign = "Сейчас минус";
                _configReadEvents.TimeSettingsStart.RelativeOffset.Days = 1;
            }

            ApplyConfigToUI();
        }

        /// <summary>
        /// Метод применения файла конфигурации к элементам интерфейса
        /// </summary>
        private void ApplyConfigToUI()
        {
            // работа с началом диапазона
            string modeStart = _configReadEvents.TimeSettingsStart.Mode;
            if (modeStart == "Relative")
            {
                StartDataOtnosit.Checked = true;
                StartSign.SelectedItem = _configReadEvents.TimeSettingsStart.Sign;

                DayStart.Text = _configReadEvents.TimeSettingsStart.RelativeOffset.Days.ToString();
                HourStart.Text = _configReadEvents.TimeSettingsStart.RelativeOffset.Hours.ToString();
                MinStart.Text = _configReadEvents.TimeSettingsStart.RelativeOffset.Minutes.ToString();
                SecStart.Text = _configReadEvents.TimeSettingsStart.RelativeOffset.Seconds.ToString();

            }
            if (modeStart == "Absolute")
            {
                StartDateAbs.Checked = true;
                dateTimePickerStart.Value = (DateTime)_configReadEvents.TimeSettingsStart.AbsolutTime;
                HourStart.Text = _configReadEvents.TimeSettingsStart.AbsolutTime.Value.Hour.ToString();
                MinStart.Text = _configReadEvents.TimeSettingsStart.AbsolutTime.Value.Minute.ToString();
                SecStart.Text = _configReadEvents.TimeSettingsStart.AbsolutTime.Value.Second.ToString();
            }

            // работа с концом диапазона
            string modeEnd = _configReadEvents.TimeSettingsEnd.Mode;
            if (modeEnd == "Relative")
            {
                EndDataOtnosit.Checked = true;
                EndSign.SelectedItem = _configReadEvents.TimeSettingsEnd.Sign;

                DayEnd.Text = _configReadEvents.TimeSettingsEnd.RelativeOffset.Days.ToString();
                HourEnd.Text = _configReadEvents.TimeSettingsEnd.RelativeOffset.Hours.ToString();
                MinEnd.Text = _configReadEvents.TimeSettingsEnd.RelativeOffset.Minutes.ToString();
                SecEnd.Text = _configReadEvents.TimeSettingsEnd.RelativeOffset.Seconds.ToString();

            }
            if (modeEnd == "Absolute")
            {
                EndDateAbs.Checked = true;
                dateTimePickerStop.Value = (DateTime)_configReadEvents.TimeSettingsEnd.AbsolutTime;
                HourEnd.Text = _configReadEvents.TimeSettingsEnd.AbsolutTime.Value.Hour.ToString();
                MinEnd.Text = _configReadEvents.TimeSettingsEnd.AbsolutTime.Value.Minute.ToString();
                SecEnd.Text = _configReadEvents.TimeSettingsEnd.AbsolutTime.Value.Second.ToString();
            }

            // работа с "Выбор типа события"
            NotificationsCB.Checked = _configReadEvents.SelectTypeEvents.Notifications;
            AlarmsCB.Checked = _configReadEvents.SelectTypeEvents.Alarms;
            UnlockingKeysCB.Checked = _configReadEvents.SelectTypeEvents.UnlockingKeys;
            ViolationsHTP_CB.Checked = _configReadEvents.SelectTypeEvents.ViolationsHTP;
            OperatorActionsCB.Checked = _configReadEvents.SelectTypeEvents.OperatorActions;

            // работа с "Формат"
            NotificationsTB.Text = _configReadEvents.FormatEvents.NamePageNotifications;
            AlarmsTB.Text = _configReadEvents.FormatEvents.NamePageAlarms;
            UnlockingKeysTB.Text = _configReadEvents.FormatEvents.NamePageUnlockingKeys;
            ViolationsTB.Text = _configReadEvents.FormatEvents.NamePageViolations;
            OperatorActionsTB.Text = _configReadEvents.FormatEvents.NamePageOperatorActions;
            txtDateTimeFormat.Text = _configReadEvents.FormatEvents.Format;
            NameAtributsCB.Checked = _configReadEvents.FormatEvents.NameAttributes;
            cbAutoFillingEvents.Checked = _configReadEvents.FormatEvents.AutoFillingEvents;

            Properties.Settings.Default.RunOnOpenEvents = cbAutoFillingEvents.Checked;
            Properties.Settings.Default.Save();

            // работа с выбранными активами(объектами)
            if ( _configReadEvents.SelectedAsset.Count > 0 )
            {
                int cnt = 0;
                foreach (var asset in _configReadEvents.SelectedAsset)
                {
                    SelectedAssetsCLB.Items.Add(asset);
                    SelectedAssetsCLB.SetItemCheckState(cnt++, CheckState.Checked);
                }
            }
            
        }

        /// <summary>
        /// Метод сохранения конфигурации в файл
        /// </summary>
        private void SaveConfig()
        {
            // перед тем, как записать, очищу объект класса ConfigReadData 
            ClearConfigReadData(_configReadEvents);

            // работа с началом диапазона
            _configReadEvents.TimeSettingsStart.Mode = StartDataOtnosit.Checked ? "Relative" : "Absolute";
            if (StartDataOtnosit.Checked)
            {
                _configReadEvents.TimeSettingsStart.Sign = StartSign.SelectedItem.ToString();

                _configReadEvents.TimeSettingsStart.RelativeOffset.Days = int.Parse(DayStart.Text);
                _configReadEvents.TimeSettingsStart.RelativeOffset.Hours = int.Parse(HourStart.Text);
                _configReadEvents.TimeSettingsStart.RelativeOffset.Minutes = int.Parse(MinStart.Text);
                _configReadEvents.TimeSettingsStart.RelativeOffset.Seconds = int.Parse(SecStart.Text);

            }
            if (StartDateAbs.Checked)
            {
                DateTime date = dateTimePickerStart.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourStart.Text);
                int minute = int.Parse(MinStart.Text);
                int second = int.Parse(SecStart.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                _configReadEvents.TimeSettingsStart.AbsolutTime = dateTime;
            }

            // работа с концом диапазона
            _configReadEvents.TimeSettingsEnd.Mode = EndDataOtnosit.Checked ? "Relative" : "Absolute";
            if (EndDataOtnosit.Checked)
            {
                _configReadEvents.TimeSettingsEnd.Sign = EndSign.SelectedItem.ToString();

                _configReadEvents.TimeSettingsEnd.RelativeOffset.Days = int.Parse(DayEnd.Text);
                _configReadEvents.TimeSettingsEnd.RelativeOffset.Hours = int.Parse(HourEnd.Text);
                _configReadEvents.TimeSettingsEnd.RelativeOffset.Minutes = int.Parse(MinEnd.Text);
                _configReadEvents.TimeSettingsEnd.RelativeOffset.Seconds = int.Parse(SecEnd.Text);

            }
            if (EndDateAbs.Checked)
            {
                DateTime date = dateTimePickerStop.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourEnd.Text);
                int minute = int.Parse(MinEnd.Text);
                int second = int.Parse(SecEnd.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                _configReadEvents.TimeSettingsEnd.AbsolutTime = dateTime;
            }

            // работа с "Выбор типа события"
            _configReadEvents.SelectTypeEvents.Notifications = NotificationsCB.Checked;
            _configReadEvents.SelectTypeEvents.Alarms = AlarmsCB.Checked;
            _configReadEvents.SelectTypeEvents.UnlockingKeys = UnlockingKeysCB.Checked;
            _configReadEvents.SelectTypeEvents.ViolationsHTP = ViolationsHTP_CB.Checked;
            _configReadEvents.SelectTypeEvents.OperatorActions = OperatorActionsCB.Checked;

            // работа с "Формат"
            _configReadEvents.FormatEvents.NamePageNotifications = NotificationsTB.Text;
            _configReadEvents.FormatEvents.NamePageAlarms = AlarmsTB.Text;
            _configReadEvents.FormatEvents.NamePageUnlockingKeys = UnlockingKeysTB.Text;
            _configReadEvents.FormatEvents.NamePageViolations = ViolationsTB.Text;
            _configReadEvents.FormatEvents.NamePageOperatorActions = OperatorActionsTB.Text;
            _configReadEvents.FormatEvents.Format = txtDateTimeFormat.Text;
            _configReadEvents.FormatEvents.NameAttributes = NameAtributsCB.Checked;
            _configReadEvents.FormatEvents.AutoFillingEvents = cbAutoFillingEvents.Checked;

            // работа с выбранными активами(объектами)
            _configReadEvents.SelectedAsset = GetSelectedAsset();

            // Сериализуем в JSON и записываем в файл
            string json = JsonConvert.SerializeObject(_configReadEvents, Formatting.Indented);
            File.WriteAllText(_configFilePath, json);

        }

        /// <summary>
        /// Метод очистки объекта класса ConfigReadData
        /// </summary>
        public void ClearConfigReadData(ConfigReadEvents config)
        {
            config.TimeSettingsStart = new TimeSettingsEvents();
            config.TimeSettingsEnd = new TimeSettingsEvents();
            config.SelectTypeEvents = new SelectTypeEventsClass();
            config.FormatEvents = new FormatEventsClass();
        }

        /// <summary>
        /// Обработка события при закрывании окна(формы)
        /// </summary>
        private void DataReadingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// Метод для показа индикатора ожидания
        /// </summary>
        private void ShowLoadingIndicator()
        {
            progressBarLoading.Visible = true;
            labelLoading.Visible = true;
        }

        /// <summary>
        /// Метод для скрытия индикатора ожидания
        /// </summary>
        private void HideLoadingIndicator()
        {
            progressBarLoading.Visible = false;
            labelLoading.Visible = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void txtDateTimeFormat_TextChanged(object sender, EventArgs e)
        {

        }

        private void StartDataOtnosit_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStartDateInputControls();
            ParamStartSign();
        }

        private void EndDataOtnosit_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStopDateInputControls();
            ParamEndSign();
        }

        private void UpdateStartDateInputControls()
        {
            if (StartDataOtnosit.Checked) // когда выбрана радио кнопка "Относительного текущего"
            {
                label4.Visible = true;
                StartSign.Visible = true;
                DayStart.Visible = true;

                dateTimePickerStart.Visible = false;
            }
            else if (StartDateAbs.Checked) // когда выбрана радио кнопка "Абсолютное"
            {
                label4.Visible = false;
                StartSign.Visible = false;
                DayStart.Visible = false;

                dateTimePickerStart.Visible = true;

                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;

                HourStart.Visible = true;
                MinStart.Visible = true;
                SecStart.Visible = true;

            }
        }

        private void UpdateStopDateInputControls()
        {
            if (EndDataOtnosit.Checked)
            {
                label11.Visible = true;
                EndSign.Visible = true;
                DayEnd.Visible = true;

                dateTimePickerStop.Visible = false;
            }
            else if (EndDateAbs.Checked)
            {
                label11.Visible = false;
                EndSign.Visible = false;
                DayEnd.Visible = false;

                dateTimePickerStop.Visible = true;

                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;

                HourEnd.Visible = true;
                MinEnd.Visible = true;
                SecEnd.Visible = true;
            }
        }

        private void ParamStartSign()
        {
            // сейчас, текущая смена
            if (StartDataOtnosit.Checked && (StartSign.SelectedItem.ToString() == "Сейчас" || StartSign.SelectedItem.ToString() == "Текущая смена"))
            {
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                DayStart.Visible = false;
                HourStart.Visible = false;
                MinStart.Visible = false;
                SecStart.Visible = false;
            }

            // Смена плюс, Смена , Сейчас плюс, Сейчас минус, Сегодня плюс, Сегодня минус
            if (StartDataOtnosit.Checked && (StartSign.SelectedItem.ToString() == "Смена плюс" || StartSign.SelectedItem.ToString() == "Смена минус" || 
                StartSign.SelectedItem.ToString() == "Сейчас плюс" || StartSign.SelectedItem.ToString() == "Сейчас минус" ||
                    StartSign.SelectedItem.ToString() == "Сегодня плюс" || StartSign.SelectedItem.ToString() == "Сегодня минус"))
            {
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;

                DayStart.Visible = true;
                HourStart.Visible = true;
                MinStart.Visible = true;
                SecStart.Visible = true;
            }

        }

        private void ParamEndSign()
        {
            // сейчас, текущая смена
            if (EndSign.SelectedItem.ToString() == "Сейчас" || EndSign.SelectedItem.ToString() == "Текущая смена")
            {
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;

                DayEnd.Visible = false;
                HourEnd.Visible = false;
                MinEnd.Visible = false;
                SecEnd.Visible = false;
            }

            // Смена плюс, Смена , Сейчас плюс, Сейчас минус, Сегодня плюс, Сегодня минус
            if (EndSign.SelectedItem.ToString() == "Смена плюс" || EndSign.SelectedItem.ToString() == "Смена минус" ||
                EndSign.SelectedItem.ToString() == "Сейчас плюс" || EndSign.SelectedItem.ToString() == "Сейчас минус" ||
                    EndSign.SelectedItem.ToString() == "Сегодня плюс" || EndSign.SelectedItem.ToString() == "Сегодня минус")
            {
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;

                DayEnd.Visible = true;
                HourEnd.Visible = true;
                MinEnd.Visible = true;
                SecEnd.Visible = true;
            }
      
        }

        private void StartSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamStartSign();
        }

        private void StartDateAbs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStartDateInputControls();
        }

        private void EndSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamEndSign();
        }

        private void EndDateAbs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStopDateInputControls();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Метод получения выбраннных объектов с галочками
        /// </summary>
        private List<Asset_Class> GetSelectedAsset()
        {
            var list = SelectedAssetsCLB.CheckedItems.Cast<Asset_Class>().ToList();
            return list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateReportEvents();
        }

        public async void GenerateReportEvents()
        {
            // var asset = "46cae1c4-2563-48c4-9054-27f2d5e42821";
            // "2025-03-20 01:01:01"
            // var startDate = new DateTime(2025, 03, 04, 16,0,0);
            // var endDate = new DateTime(2025, 03, 07, 16, 0, 0);
            // 2025-03-03 16:00:00

            ShowLoadingIndicator();

            var setForm = new SettingsForm();
            if (!setForm.CheckConnection())
            {
                HideLoadingIndicator();
                return;
            }

            DateTime startDate = StartDate();
            DateTime endDate = EndDate();

            if (startDate > endDate)
            {
                MessageBox.Show("Время начала диапазона должно быть меньше времени конца", "Загрузка событий");
                HideLoadingIndicator();
                return;
            }

            List<Asset_Class> selectedAssestInLCB = GetSelectedAsset();


            if (selectedAssestInLCB.Count != 0)
            {
                // работаем с событиями "Уведомления(Notification)"
                if (NotificationsCB.Checked)
                {
                    Dictionary<string, string> assets_event = new Dictionary<string, string>();
                    // каждому событию будет соответствовать свой актив
                    List<EventDto> events_notification = new List<EventDto>();

                    foreach (var asset in selectedAssestInLCB)
                    {
                        List<EventDto> events = await _dataServerClient.GetEvent(asset.Id, startDate, endDate, "Notification");
                        events_notification.AddRange(events);

                        foreach (var _event in events)
                        {
                            assets_event.Add(_event.EventId, asset.Name);
                        }
                    }
                    if (events_notification.Count != 0)
                    {
                        Globals.ThisAddIn.ExportEventToExcel(events_notification, assets_event, NotificationsTB.Text, txtDateTimeFormat.Text, NameAtributsCB.Checked);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки событий Уведомления (возможно отсутствуют события в данных объектах)", "Загрузка событий");
                        HideLoadingIndicator();
                        //return;
                    }
                }

                // работаем с событиями "Сигнализация(Alarm)"
                if (AlarmsCB.Checked)
                {
                    Dictionary<string, string> assets_event = new Dictionary<string, string>();
                    // каждому событию будет соответствовать свой актив
                    List<EventDto> events_alarms = new List<EventDto>();

                    foreach (var asset in selectedAssestInLCB)
                    {
                        List<EventDto> events = await _dataServerClient.GetEvent(asset.Id, startDate, endDate, "Alarm");
                        events_alarms.AddRange(events);

                        foreach (var _event in events)
                        {
                            if (!assets_event.TryGetValue(_event.EventId, out string value) || value != asset.Name)
                            {
                                assets_event.Add(_event.EventId, asset.Name);
                            }

                        }
                    }
                    if (events_alarms.Count != 0)
                    {
                        Globals.ThisAddIn.ExportEventToExcel(events_alarms, assets_event, AlarmsTB.Text, txtDateTimeFormat.Text, NameAtributsCB.Checked);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки событий Сигнализация(возможно отсутствуют события в данных объектах)", "Загрузка событий");
                        HideLoadingIndicator();
                        //return;
                    }
                }

                // работаем с событиями "Деблокирочные ключи(DK)"
                if (UnlockingKeysCB.Checked)
                {
                    Dictionary<string, string> assets_event = new Dictionary<string, string>();
                    // каждому событию будет соответствовать свой актив
                    List<EventDto> events_dk = new List<EventDto>();

                    foreach (var asset in selectedAssestInLCB)
                    {
                        List<EventDto> events = await _dataServerClient.GetEvent(asset.Id, startDate, endDate, "DK");
                        events_dk.AddRange(events);

                        foreach (var _event in events)
                        {
                            assets_event.Add(_event.EventId, asset.Name);
                        }
                    }
                    if (events_dk.Count != 0)
                    {
                        Globals.ThisAddIn.ExportEventToExcel(events_dk, assets_event, UnlockingKeysTB.Text, txtDateTimeFormat.Text, NameAtributsCB.Checked);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки событий Деблокировочные ключи (возможно отсутствуют события в данных объектах)", "Загрузка событий");
                        HideLoadingIndicator();
                        //return;
                    }
                }

            }
            else
            {
                MessageBox.Show("Выберите объект для загрузки событий", "Загрузка событий");
                HideLoadingIndicator();
                return;
            }

            // вызываем метод сохранения конфиг файла при нажатии кнопки
            SaveConfig();

            HideLoadingIndicator();
        }

        private void listTags_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void treeAssets_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void rbSlices_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbAllValues_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void treeAssets_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }
    }
}
