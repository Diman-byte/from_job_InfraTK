using ExcelReportingAddin.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelReportingAddin
{
    public partial class DataReadingForm : Form
    {
        //string selectedSign = StartSign.Item.ToString();

        private DataServerClient _dataServerClient;

        // хранит выбранные в окне теги
        private List<Guid> _selectedTagIds;

        // хранит выбранный актив (потом это будет список активов)
        private Guid _selectedAsset;

        // хранит путь файла с данными о выбранных тегах и диапазоне времени
        private string _configFilePath;

        // объект, который содержит данные о выбранных тегах и диапазоне времени
        private ConfigReadData _configReadData;

        // объект, который хранит параметры из вкладки "Формат
        private ConfigTabFormat _configTabFormat;

        // поле, отвечающее за автоматический вывод
        //public bool 

        public DataReadingForm()
        {
            InitializeComponent();

            _dataServerClient = new DataServerClient();

            PopulateTreeView();

            // инициализируем 
            _selectedTagIds = new List<Guid>();



            //_selectedTagIds = new List<Guid>
            //{
            //    //потом убрать
            //    Guid.Parse("d5f03588-f002-4aa2-bc18-b95854e9a888")
            //};
            //_selectedAsset = Guid.Parse("1973d52a-af0e-426f-beaf-10936046aa0d");





            // Чтобы добавить обработчик события NodeMouseClick для TreeView
            // Подписываемся на событие NodeMouseDoubleClick для treeAssets
            treeAssets.NodeMouseClick += treeAssets_NodeMouseClick;

            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запрет изменения размера
            this.MaximizeBox = false; // Убираем кнопку максимизации

            //StartSign.SelectedIndex = 0; // выбираем по умолчанию первый элемент в ComboBox (знак + в относительном режиме)
            //EndSign.SelectedIndex = 0;

            listTags.ValueMember = "Id"; // Указывает, какое свойство объекта будет использоваться как скрытое значение для каждого элемента.
            listTags.DisplayMember = "Name"; // Указывает, какое свойство объекта, добавленного в ListBox, будет отображаться пользователю

            //при инициализации формы вызывается методы UpdateStartDateInputControls() и UpdateStopDateInputControls(),
            //чтобы скрыть или отобразить нужные элементы в зависимости от начального состояния.
            UpdateStartDateInputControls();
            UpdateStopDateInputControls();

            // добавляю обработчик события закрытия окна
            this.FormClosing += DataReadingForm_FormClosing;

            InitializeConfigFile();
            LoadConfig();
        }

        /// <summary>
        /// Заполнить TreeView (treeAssets) - старый метод
        /// </summary>
        //private async void PopulateTreeView()
        //{
        //    ShowLoadingIndicator();

        //    treeAssets.Nodes.Clear(); // Перед обновлением данных очищай дерево, чтобы избежать дублирования
        //    listTags.Items.Clear(); // всегда в начале очищаем правое окошко с тегами

        //    List<Guid?> list_groups = await GetAssetGroupId();

        //    foreach(var group in list_groups)
        //    {
        //        string name_group = group.ToString();
        //        // Добавляем корневые узлы (группы активов)
        //        if (name_group == "00000000-0000-0000-0000-000000000000")
        //        {
        //            name_group = "Активы без группы";
        //        }

        //        TreeNode rootNood = new TreeNode(name_group) { Tag = "group" };
        //        treeAssets.Nodes.Add(rootNood);
        //        // TreeView состоит из узлов (Nodes), которые могут содержать вложенные узлы.
        //        // Это позволяет отображать иерархическую структуру данных.
        //        // Каждый узел представлен объектом типа TreeNode

        //        // Используй свойство Nodes для добавления узлов в корень дерева,
        //        // а также для добавления дочерних узлов к конкретным узлам

        //        // добавляю дочерние узлы (то есть активы, которые входят в текущую группу активов)
        //        AddAssets(rootNood, group);
        //    }

        //    HideLoadingIndicator();
        //}

        /// <summary>
        /// Заполнить TreeView (treeAssets)
        /// </summary>
        private async void PopulateTreeView()
        {
            ShowLoadingIndicator();

            treeAssets.Nodes.Clear(); // Перед обновлением данных очищай дерево, чтобы избежать дублирования
            listTags.Items.Clear(); // всегда в начале очищаем правое окошко с тегами

            // добавляю активы без группы
            List<Guid?> listGroups = await GetAssetGroupId();
            if (listGroups.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000"))) {
                TreeNode rootNood = new TreeNode("Активы без группы") { Tag = "group" };
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
                    TreeNode subNode = new TreeNode(asset.Name.ToString()) { Tag = asset};
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
                g => new AssetGroupTreeNodeDto { Id = g.Id, Name = g.Name }) ;

            foreach (var group in groups)
            {
                if(groupDictionary.ContainsKey((Guid)group.ParentId) && group.ParentId != Guid.Empty) //  проверяет наличие родителя этой группы в словаре groupDictionary; у группы есть родительский элемент
                {
                    treeNods[(Guid)group.ParentId].Children.Add(treeNods[group.Id]); // Если группа имеет родителя, то текущий объект добавляется к списку детей этого родителя
                }
            }

            return treeNods.Values.Where(node => groupDictionary[node.Id].ParentId == Guid.Empty).ToList();
            // Возвращается список узлов, являющихся корнями дерева. Для этого выбираются те узлы, у которых значение поля ParentId
            // равно Guid.Empty (то есть они не имеют родителей), используя LINQ-запросы через словарь groupDictionary.
        }


        /// <summary>
        /// Обработчик события. Вызывается при выборе узла в TreeView
        /// </summary>
        private void treeAssets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        /// <summary>
        /// Обработчик события. вызывается при одном щелчке по узлу TreeView
        /// </summary>
        private void treeAssets_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() != "group") { // двойное нажатие по группе активов не будем обрабатывать
                // Получаем объект актива, связанный с выбранным узлом
                var selectedAsset = e.Node.Tag as AssetDto;
                listTags.Items.Clear(); // если выбрали другой актив, нужно очистить прошлые теги

                // записываем выбранный актив в наше поле 
                _selectedAsset = selectedAsset.Id;

                PopulateListTags(selectedAsset);
            }
        }

        /// <summary>
        /// Заполнить список тегов справа
        /// </summary>
        private async void PopulateListTags(AssetDto selectedAsset)
        {
            ShowLoadingIndicator();

            var list = await _dataServerClient.GetTagsByAssetId(selectedAsset.Id);

            if (list != null) // если есть теги в выбранном активе
            {
                foreach (var tag in list)
                {
                    if(_selectedTagIds.Contains(tag.Id))
                    {
                        tag.IsSelected = true;
                    }
                    listTags.Items.Add(tag);
                    listTags.SetItemCheckState(listTags.Items.IndexOf(tag), tag.IsSelected ? CheckState.Checked : CheckState.Unchecked);
                }
            }

            HideLoadingIndicator();
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupStartDate_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void StartSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamStartSign();
        }

        private void rbSlices_CheckedChanged(object sender, EventArgs e)
        {
            if(rbSlices.Checked)
            {
                txtSliceCount.Visible = true;
                labelSlice.Visible = true;
                comboBoxTypeSlice.Visible = true;

                // выбираем по умолчанию тип среза FirstPoint
                comboBoxTypeSlice.SelectedIndex = 0;
            }
        }

        private void rbAllValues_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllValues.Checked)
            {
                txtSliceCount.Visible = false;
                labelSlice.Visible = false;
                comboBoxTypeSlice.Visible = false;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// изменяем список Id выбранных тегов _selectedTagIds
        /// обрабатывает изменения состояния элементов в CheckedListBox 
        /// (например, когда какой-то элемент был отмечен или снят галочкой)
        /// </summary>
        private void listTags_CheckedChanged(object sender, EventArgs e)
        {
            // sender as CheckedListBox — приводит объект sender к типу CheckedListBox.
            //CheckedItems — свойство, которое возвращает все отмеченные(выбранные) элементы.
            //.Cast<TagDto>() — преобразует элементы CheckedItems в тип TagDto.
            //.Select(t => t.Id) — извлекает только Id из каждого объекта TagDto.
            var chechedItemsIds = (sender as CheckedListBox).CheckedItems.Cast<TagDto>().Select(t => t.Id);
            foreach (var tagId in chechedItemsIds)
            {
                if (!_selectedTagIds.Contains(tagId))
                {
                    _selectedTagIds.Add(tagId);
                }
            }

            var itemsToUncheck = new List<Guid>(); //  список, содержащий идентификаторы (Guid) тегов, которые больше не отмечены в CheckedListBox, но все еще находятся в _selectedTagIds
            var allItemsIds = (sender as CheckedListBox).Items.Cast<TagDto>().Select(t => t.Id); // allItemsIds собирает Id всех элементов CheckedListBox
            foreach (var tagId in _selectedTagIds)
            {
                if (allItemsIds.Contains(tagId) && !chechedItemsIds.Contains(tagId))
                {
                    itemsToUncheck.Add(tagId); // элемент нужно убрать из _selectedTagIds
                }
            }
            itemsToUncheck.ForEach(item => _selectedTagIds.Remove(item));
            // Лямбда-выражение item => _selectedTagIds.Remove(item):
            //item — это текущий элемент из itemsToUncheck, для которого выполняется операция.
            //=> указывает на действие, которое будет выполнено с item.
            //_selectedTagIds.Remove(item) — метод Remove удаляет item из списка _selectedTagIds
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
                    if (now.Hour >= 8 && now.Hour <= 19)
                    {
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
        /// <returns> Объект времени конца диапазона </returns>
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

        private void LoadTagsButton_Click(object sender, EventArgs e)
        {
             GenerateReportData();
        }

        public async void GenerateReportData()
        {
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
                MessageBox.Show("Время начала диапазона должно быть меньше времени конца", "Загрузка данных");
                HideLoadingIndicator();
                return;
            }

            if (_selectedTagIds.Count == 0)
            {
                MessageBox.Show("Выберите теги для выгрузки данных и повторите заново", "Загрузка данных");
                HideLoadingIndicator();
                return;
            }

            if (_selectedAsset == Guid.Empty)
            {
                MessageBox.Show("Выберите актив для выгрузки данных и повторите заново", "Загрузка данных");
                HideLoadingIndicator();
                return;
            }

            // вызываем метод сохранения конфиг файла при нажатии кнопки
            SaveConfig();

            // для тестирования моего
            //DateTime startDate = new DateTime(2024, 11, 11, 18, 33, 46);
            //DateTime endDate = new DateTime(2024, 11, 12, 5, 0, 0);

            // пока что можем выводить теги только с одного актива

            List<TagValDto> list_tags_val = null;

            if (rbAllValues.Checked)
            {
                // если выбрана кнопка "Все значения"
                list_tags_val = await _dataServerClient.GetTagsHistoricalData(_selectedAsset, _selectedTagIds, startDate, endDate);
            }
            if (rbSlices.Checked)
            {
                // если выбрана кнопка "Срезы"
                int pointsAmount = int.Parse(txtSliceCount.Text);
                string sliceType = comboBoxTypeSlice.SelectedItem.ToString();
                list_tags_val = await _dataServerClient.GetTagsHistoricalSliceData(_selectedAsset, _selectedTagIds, startDate, endDate, pointsAmount, sliceType);
            }

            // это нужно для того, чтобы подтянуть имя к тегам
            var listTags = await _dataServerClient.GetTagsByAssetId(_selectedAsset);

            // собираем параметры с вкладки "Формат"
            CollectTabFormat();

            if (list_tags_val != null && list_tags_val.Any())
            {
                //MessageBox.Show("Загружаю данные за выбранный интервал и вывожу на лист \"Data\"");

                //Чтобы вызвать метод, можно получить доступ к экземпляру ThisAddIn и вызвать ExportDataToActiveSheet напрямую
                Globals.ThisAddIn.ExportDataToExcel(list_tags_val, listTags, _configTabFormat.NameSheet, _configTabFormat.Format, _configTabFormat.IsDisplayTagNames, _configTabFormat.IsDisplayTagDescription);
                // Globals — это специальный класс, автоматически созданный в проектах надстроек Excel (VSTO),
                // который предоставляет глобальный доступ к экземпляру надстройки. Он позволяет другим частям
                // программы получить доступ к объекту ThisAddIn, который содержит всю логику и настройки надстройки.
                // Через Globals.ThisAddIn можно обращаться к методам и данным, определённым в классе ThisAddIn

                // Globals.ThisAddIn создается автоматически при запуске надстройки, обеспечивая единый доступ к
                // текущему экземпляру класса ThisAddIn
            }
            else
            {
                MessageBox.Show("Ошибка загрузки исторических данных (возможно теги за выбранный интервал отсутствуют)", "Загрузка данных");
                HideLoadingIndicator();
                return;
            }

            HideLoadingIndicator();
        }

        // когда выбрана радио кнопка "Относительного текущего"
        private void StartDataOtnosit_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStartDateInputControls();
            ParamStartSign();
        }

        // когда выбрана радио кнопка "Абсолютное"
        private void StartDateAbs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStartDateInputControls();
            
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
            if(EndDataOtnosit.Checked)
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

        private void EndDataOtnosit_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStopDateInputControls();
            ParamEndSign();
        }

        private void EndDateAbs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStopDateInputControls();
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
                .ApplicationData), excelFileName + ".conf"); // Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) Возвращает путь к папке AppData\Roaming текущего пользователя.
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
            if(File.Exists(_configFilePath) && File.ReadAllText(_configFilePath) != "{}")
            {
                string json = File.ReadAllText(_configFilePath);
                _configReadData = JsonConvert.DeserializeObject<ConfigReadData>(json);
            }
            else
            {
                _configReadData = new ConfigReadData();

                //задаю по умолчанию параметры начала интервала как минус 1 день
                _configReadData.TimeSettingsStart.Sign = "Сейчас минус";
                _configReadData.TimeSettingsStart.RelativeOffset.Days = 1;
            }

            ApplyConfigToUI();
        }

        /// <summary>
        /// Метод применения файла конфигурации к элементам интерфейса
        /// </summary>
        private void ApplyConfigToUI()
        {
            foreach(var tag in _configReadData.SelectedTagIds)
            {
                _selectedTagIds.Add(tag);
            }

            _selectedAsset = _configReadData.SelectedAsset;

            // работа с началом диапазона
            string modeStart = _configReadData.TimeSettingsStart.Mode;
            if (modeStart == "Relative")
            {
                StartDataOtnosit.Checked = true;
                StartSign.SelectedItem = _configReadData.TimeSettingsStart.Sign;

                DayStart.Text = _configReadData.TimeSettingsStart.RelativeOffset.Days.ToString();
                HourStart.Text = _configReadData.TimeSettingsStart.RelativeOffset.Hours.ToString();
                MinStart.Text = _configReadData.TimeSettingsStart.RelativeOffset.Minutes.ToString();
                SecStart.Text = _configReadData.TimeSettingsStart.RelativeOffset.Seconds.ToString();
            }
            if (modeStart == "Absolute")
            {
                StartDateAbs.Checked = true;
                dateTimePickerStart.Value = (DateTime)_configReadData.TimeSettingsStart.AbsolutTime;
                HourStart.Text = _configReadData.TimeSettingsStart.AbsolutTime.Value.Hour.ToString();
                MinStart.Text = _configReadData.TimeSettingsStart.AbsolutTime.Value.Minute.ToString();
                SecStart.Text = _configReadData.TimeSettingsStart.AbsolutTime.Value.Second.ToString();
            }

            // работа с концом диапазона
            string modeEnd = _configReadData.TimeSettingsEnd.Mode;
            if (modeEnd == "Relative")
            {
                EndDataOtnosit.Checked = true;
                EndSign.SelectedItem = _configReadData.TimeSettingsEnd.Sign;
                DayEnd.Text = _configReadData.TimeSettingsEnd.RelativeOffset.Days.ToString();
                HourEnd.Text = _configReadData.TimeSettingsEnd.RelativeOffset.Hours.ToString();
                MinEnd.Text = _configReadData.TimeSettingsEnd.RelativeOffset.Minutes.ToString();
                SecEnd.Text = _configReadData.TimeSettingsEnd.RelativeOffset.Seconds.ToString();
            }
            if (modeEnd == "Absolute")
            {
                EndDateAbs.Checked = true;
                dateTimePickerStop.Value = (DateTime)_configReadData.TimeSettingsEnd.AbsolutTime;
                HourEnd.Text = _configReadData.TimeSettingsEnd.AbsolutTime.Value.Hour.ToString();
                MinEnd.Text = _configReadData.TimeSettingsEnd.AbsolutTime.Value.Minute.ToString();
                SecEnd.Text = _configReadData.TimeSettingsEnd.AbsolutTime.Value.Second.ToString();
            }

            // работа с параметрами исторических данных(все значения или срезы)
            string parametrGettingHistoricalData = _configReadData.ParametrGettingHistoricalData;
            if (parametrGettingHistoricalData == "AllValues")
            {
                rbAllValues.Checked = true;
            }
            if(parametrGettingHistoricalData == "Slices")
            {
                rbSlices.Checked = true;
                txtSliceCount.Text = _configReadData.PointsAmount.ToString();
                comboBoxTypeSlice.SelectedItem = _configReadData.SliceType;
            }

            // работа с вкладкой "Формат"
            txtSheetName.Text = _configReadData.TabFormat.NameSheet;
            txtDateTimeFormat.Text = _configReadData.TabFormat.Format;
            chkShowTagNames.Checked = _configReadData.TabFormat.IsDisplayTagNames;
            chkShowTagDescriptions.Checked = _configReadData.TabFormat.IsDisplayTagDescription;
            cbAutoFillingData.Checked = _configReadData.TabFormat.AutoFillingData;

            Properties.Settings.Default.RunOnOpenData = cbAutoFillingData.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Метод сохранения конфигурации в файл
        /// </summary>
        private void SaveConfig()
        {
            // перед тем, как записать, очищу объект класса ConfigReadData 
            ClearConfigReadData(_configReadData);
            // добавляю выбранные теги в конфигурацию
            _configReadData.SelectedTagIds.AddRange(_selectedTagIds);

            _configReadData.SelectedAsset = _selectedAsset;

            // работа с началом диапазона
            _configReadData.TimeSettingsStart.Mode = StartDataOtnosit.Checked ? "Relative" : "Absolute";
            if (StartDataOtnosit.Checked) {
                _configReadData.TimeSettingsStart.Sign = StartSign.SelectedItem.ToString();
                _configReadData.TimeSettingsStart.RelativeOffset.Days = int.Parse(DayStart.Text);
                _configReadData.TimeSettingsStart.RelativeOffset.Hours = int.Parse(HourStart.Text);
                _configReadData.TimeSettingsStart.RelativeOffset.Minutes = int.Parse(MinStart.Text);
                _configReadData.TimeSettingsStart.RelativeOffset.Seconds = int.Parse(SecStart.Text);
            }
            if(StartDateAbs.Checked)
            {
                DateTime date = dateTimePickerStart.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourStart.Text);
                int minute = int.Parse(MinStart.Text);
                int second = int.Parse(SecStart.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                _configReadData.TimeSettingsStart.AbsolutTime = dateTime;
            }

            // работа с концом диапазона
            _configReadData.TimeSettingsEnd.Mode = EndDataOtnosit.Checked ? "Relative" : "Absolute";
            if (EndDataOtnosit.Checked)
            {
                _configReadData.TimeSettingsEnd.Sign = EndSign.SelectedItem.ToString();
                _configReadData.TimeSettingsEnd.RelativeOffset.Days = int.Parse(DayEnd.Text);
                _configReadData.TimeSettingsEnd.RelativeOffset.Hours = int.Parse(HourEnd.Text);
                _configReadData.TimeSettingsEnd.RelativeOffset.Minutes = int.Parse(MinEnd.Text);
                _configReadData.TimeSettingsEnd.RelativeOffset.Seconds = int.Parse(SecEnd.Text);
            }
            if(EndDateAbs.Checked)
            {
                DateTime date = dateTimePickerStop.Value;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                int hour = int.Parse(HourEnd.Text);
                int minute = int.Parse(MinEnd.Text);
                int second = int.Parse(SecEnd.Text);

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                _configReadData.TimeSettingsEnd.AbsolutTime = dateTime;
            }

            // работа с параметрами исторических данных(все значения или срезы)
            if (rbAllValues.Checked)
            {
                _configReadData.ParametrGettingHistoricalData = "AllValues";
            }
            if(rbSlices.Checked)
            {
                _configReadData.ParametrGettingHistoricalData = "Slices";
                _configReadData.PointsAmount = int.Parse(txtSliceCount.Text);
                _configReadData.SliceType = comboBoxTypeSlice.SelectedItem.ToString();
            }

            // работа с вкладкой "Формат"
            _configReadData.TabFormat.NameSheet = txtSheetName.Text;
            _configReadData.TabFormat.Format = txtDateTimeFormat.Text;
            _configReadData.TabFormat.IsDisplayTagNames = chkShowTagNames.Checked;
            _configReadData.TabFormat.IsDisplayTagDescription = chkShowTagDescriptions.Checked;
            _configReadData.TabFormat.AutoFillingData = cbAutoFillingData.Checked;

            // Сериализуем в JSON и записываем в файл
            string json = JsonConvert.SerializeObject(_configReadData, Formatting.Indented);
            File.WriteAllText(_configFilePath, json);
        }

        /// <summary>
        /// Метод очистки объекта класса ConfigReadData
        /// </summary>
        public void ClearConfigReadData(ConfigReadData config)
        {
            config.SelectedTagIds.Clear();
            config.TimeSettingsStart = new TimeSettings();
            config.TimeSettingsEnd = new TimeSettings();
        }

        /// <summary>
        /// Обработка события при закрывании окна(формы)
        /// </summary>
        private void DataReadingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Класс, отвечающий за параметры вкладки "Формат"
        /// Решаю не добавлять его в Dto"шки
        /// </summary>
        public class ConfigTabFormat
        {
            /// <summary>
            /// имя листа Excel, куда будем выгружать данные
            /// </summary>
            public string NameSheet {  get; set; }

            /// <summary>
            /// формат даты и времени
            /// </summary>
            public string Format { get; set; }

            /// <summary>
            /// отображать имена тегов(да или нет)
            /// </summary>
            public bool IsDisplayTagNames { get; set; }

            /// <summary>
            /// отображать описание тегов(да или нет)
            /// </summary>
            public bool IsDisplayTagDescription { get; set; }
        }

        /// <summary>
        /// метод, который собирает данные с вкладки "Формат"
        /// </summary>
        private void CollectTabFormat()
        {
            _configTabFormat = new ConfigTabFormat();

            string nameSheet = txtSheetName.Text;
            if (nameSheet == "") // если пользователь не ввел имя листа для вывода данных, то называем лист по умолчанию "Data"
            {
                nameSheet = "Data";
            }
            _configTabFormat.NameSheet = nameSheet;

            _configTabFormat.Format = txtDateTimeFormat.Text;
                
            _configTabFormat.IsDisplayTagNames = chkShowTagNames.Checked;
            _configTabFormat.IsDisplayTagDescription = chkShowTagDescriptions.Checked;
        }

        private void EndSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamEndSign();
        }
    }
}
