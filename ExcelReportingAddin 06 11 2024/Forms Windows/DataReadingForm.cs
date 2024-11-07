using ExcelReportingAddin.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelReportingAddin
{
    public partial class DataReadingForm : Form
    {
        //string selectedSign = StartSign.Item.ToString();

        private GetLogic _getLogic;

        // хранит выбранные в окне теги
        public List<Guid> ListSelectTags;

        public DataReadingForm()
        {
            InitializeComponent();
            _getLogic = new GetLogic();

            PopulateTreeView();

            // инициализируем 
            ListSelectTags = new List<Guid>();

            // Чтобы добавить обработчик события NodeMouseDoubleClick для TreeView
            // Подписываемся на событие NodeMouseDoubleClick для treeAssets
            treeAssets.NodeMouseDoubleClick += treeAssets_NodeMouseDoubleClick;


        }

        /// <summary>
        /// Заполнить TreeView (treeAssets)
        /// </summary>
        private async void PopulateTreeView()
        {
            ShowLoadingIndicator();

            treeAssets.Nodes.Clear(); // Перед обновлением данных очищай дерево, чтобы избежать дублирования
            listTags.Items.Clear(); // всегда в начале очищаем правое окошко с тегами

            List<Guid?> list_groups = await GetAssetGroupId();

            foreach(var group in list_groups)
            {
                string name_group = group.ToString();
                // Добавляем корневые узлы (группы активов)
                if (name_group == "00000000-0000-0000-0000-000000000000")
                {
                    name_group = "Активы без группы";
                }

                TreeNode rootNood = new TreeNode(name_group) { Tag = "group" };
                treeAssets.Nodes.Add(rootNood);
                // TreeView состоит из узлов (Nodes), которые могут содержать вложенные узлы.
                // Это позволяет отображать иерархическую структуру данных.
                // Каждый узел представлен объектом типа TreeNode

                // Используй свойство Nodes для добавления узлов в корень дерева,
                // а также для добавления дочерних узлов к конкретным узлам

                // добавляю дочерние узлы (то есть активы, которые входят в текущую группу активов)
                AddAssets(rootNood, group);
            }

            HideLoadingIndicator();
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
                var assets = await _getLogic.GetAllAssets();
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
            var assets = await _getLogic.GetAllAssets();

            foreach (var asset in assets)
            {
                if (asset.AssetGroupId == asset_group_id)
                {
                    TreeNode subNode = new TreeNode(asset.Id.ToString()) { Tag = asset};
                    // свойство Tag для хранения связанного объекта Asset.
                    // Это поможет получать данные об активе при взаимодействии с узлом.
                    parentNode.Nodes.Add(subNode);
                }
            }
        }

        /// <summary>
        /// Обработчик события. Вызывается при выборе узла в TreeView
        /// </summary>
        private void treeAssets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        /// <summary>
        /// Обработчик события. вызывается при двойном щелчке по узлу TreeView
        /// </summary>
        private void treeAssets_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString() != "group") { // двойное нажатие по группе активов не будем обрабатывать
                // Получаем объект актива, связанный с выбранным узлом
                var selectedAsset = e.Node.Tag as AssetDto;
                listTags.Items.Clear(); // если выбрали другой актив, нужно очистить прошлые теги

                PopulateListTags(selectedAsset);
            }
        }

        /// <summary>
        /// Заполнить список тегов справа
        /// </summary>
        private async void PopulateListTags(AssetDto selectedAsset)
        {
            ShowLoadingIndicator();

            var list = await _getLogic.GetTagsByAssetId(selectedAsset.Id);

            if (list != null) // если есть теги в выбранном активе
            {
                foreach (var tag in list)
                {
                    listTags.Items.Add(tag.Id);
                }
            }

            HideLoadingIndicator();
        }

        /// <summary>
        /// Обработчик события нажатия по кнопке "Сохранить выбранные теги"
        /// </summary>
        private void buttonSelectTags_Click(object sender, EventArgs e)
        {
            // проходим по выбранным тегам
            foreach (var item in listTags.CheckedItems)
            {
                string tag = item.ToString();
                Guid tag_guid = Guid.Parse(tag);

                if (!ListSelectTags.Contains(tag_guid))
                {
                    ListSelectTags.Add(tag_guid);
                }

            }
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
            
        }

        private void rbSlices_CheckedChanged(object sender, EventArgs e)
        {
            if(rbSlices.Checked)
            {
                txtSliceCount.Visible = true;
            }
        }

        private void rbAllValues_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllValues.Checked)
            {
                txtSliceCount.Visible = false;
            }
        }
    }
}
