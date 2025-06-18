namespace ExcelReportingAddin.Forms_Windows
{
    partial class EventsReadingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventsReadingForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAssets = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
            this.SelectedAssetsCLB = new System.Windows.Forms.CheckedListBox();
            this.treeAssets = new System.Windows.Forms.TreeView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.OperatorActionsCB = new System.Windows.Forms.CheckBox();
            this.ViolationsHTP_CB = new System.Windows.Forms.CheckBox();
            this.UnlockingKeysCB = new System.Windows.Forms.CheckBox();
            this.AlarmsCB = new System.Windows.Forms.CheckBox();
            this.NotificationsCB = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupEndDate = new System.Windows.Forms.GroupBox();
            this.dateTimePickerStop = new System.Windows.Forms.DateTimePicker();
            this.SecEnd = new System.Windows.Forms.TextBox();
            this.MinEnd = new System.Windows.Forms.TextBox();
            this.HourEnd = new System.Windows.Forms.TextBox();
            this.DayEnd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.EndSign = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EndDateAbs = new System.Windows.Forms.RadioButton();
            this.EndDataOtnosit = new System.Windows.Forms.RadioButton();
            this.groupStartDate = new System.Windows.Forms.GroupBox();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.SecStart = new System.Windows.Forms.TextBox();
            this.MinStart = new System.Windows.Forms.TextBox();
            this.HourStart = new System.Windows.Forms.TextBox();
            this.DayStart = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StartSign = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartDateAbs = new System.Windows.Forms.RadioButton();
            this.StartDataOtnosit = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.OperatorActionsTB = new System.Windows.Forms.TextBox();
            this.ViolationsTB = new System.Windows.Forms.TextBox();
            this.NotificationsTB = new System.Windows.Forms.TextBox();
            this.AlarmsTB = new System.Windows.Forms.TextBox();
            this.UnlockingKeysTB = new System.Windows.Forms.TextBox();
            this.txtDateTimeFormat = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.NameAtributsCB = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.labelLoading = new System.Windows.Forms.Label();
            this.cbAutoFillingEvents = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageAssets.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupEndDate.SuspendLayout();
            this.groupStartDate.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageAssets);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(10, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(656, 427);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageAssets
            // 
            this.tabPageAssets.Controls.Add(this.label19);
            this.tabPageAssets.Controls.Add(this.SelectedAssetsCLB);
            this.tabPageAssets.Controls.Add(this.treeAssets);
            this.tabPageAssets.Location = new System.Drawing.Point(4, 25);
            this.tabPageAssets.Name = "tabPageAssets";
            this.tabPageAssets.Size = new System.Drawing.Size(648, 398);
            this.tabPageAssets.TabIndex = 3;
            this.tabPageAssets.Text = "Изменить выбор";
            this.tabPageAssets.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(350, 41);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(284, 16);
            this.label19.TabIndex = 5;
            this.label19.Text = "Отметьте галочкой необходимые объекты";
            // 
            // SelectedAssetsCLB
            // 
            this.SelectedAssetsCLB.FormattingEnabled = true;
            this.SelectedAssetsCLB.Location = new System.Drawing.Point(353, 75);
            this.SelectedAssetsCLB.Name = "SelectedAssetsCLB";
            this.SelectedAssetsCLB.Size = new System.Drawing.Size(282, 191);
            this.SelectedAssetsCLB.TabIndex = 4;
            // 
            // treeAssets
            // 
            this.treeAssets.Location = new System.Drawing.Point(1, 3);
            this.treeAssets.Name = "treeAssets";
            this.treeAssets.Size = new System.Drawing.Size(337, 392);
            this.treeAssets.TabIndex = 0;
            this.treeAssets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAssets_AfterSelect_1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.OperatorActionsCB);
            this.tabPage1.Controls.Add(this.ViolationsHTP_CB);
            this.tabPage1.Controls.Add(this.UnlockingKeysCB);
            this.tabPage1.Controls.Add(this.AlarmsCB);
            this.tabPage1.Controls.Add(this.NotificationsCB);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(648, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Выбор типа событий";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // OperatorActionsCB
            // 
            this.OperatorActionsCB.AutoSize = true;
            this.OperatorActionsCB.Location = new System.Drawing.Point(65, 235);
            this.OperatorActionsCB.Name = "OperatorActionsCB";
            this.OperatorActionsCB.Size = new System.Drawing.Size(165, 20);
            this.OperatorActionsCB.TabIndex = 4;
            this.OperatorActionsCB.Text = "Действия оператора";
            this.OperatorActionsCB.UseVisualStyleBackColor = true;
            // 
            // ViolationsHTP_CB
            // 
            this.ViolationsHTP_CB.AutoSize = true;
            this.ViolationsHTP_CB.Location = new System.Drawing.Point(65, 181);
            this.ViolationsHTP_CB.Name = "ViolationsHTP_CB";
            this.ViolationsHTP_CB.Size = new System.Drawing.Size(134, 20);
            this.ViolationsHTP_CB.TabIndex = 3;
            this.ViolationsHTP_CB.Text = "Нарушения НТР";
            this.ViolationsHTP_CB.UseVisualStyleBackColor = true;
            // 
            // UnlockingKeysCB
            // 
            this.UnlockingKeysCB.AutoSize = true;
            this.UnlockingKeysCB.Location = new System.Drawing.Point(65, 130);
            this.UnlockingKeysCB.Name = "UnlockingKeysCB";
            this.UnlockingKeysCB.Size = new System.Drawing.Size(194, 20);
            this.UnlockingKeysCB.TabIndex = 2;
            this.UnlockingKeysCB.Text = "Деблокировочные ключи";
            this.UnlockingKeysCB.UseVisualStyleBackColor = true;
            // 
            // AlarmsCB
            // 
            this.AlarmsCB.AutoSize = true;
            this.AlarmsCB.Location = new System.Drawing.Point(65, 77);
            this.AlarmsCB.Name = "AlarmsCB";
            this.AlarmsCB.Size = new System.Drawing.Size(124, 20);
            this.AlarmsCB.TabIndex = 1;
            this.AlarmsCB.Text = "Сигнализации";
            this.AlarmsCB.UseVisualStyleBackColor = true;
            // 
            // NotificationsCB
            // 
            this.NotificationsCB.AutoSize = true;
            this.NotificationsCB.Checked = true;
            this.NotificationsCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NotificationsCB.Location = new System.Drawing.Point(65, 27);
            this.NotificationsCB.Name = "NotificationsCB";
            this.NotificationsCB.Size = new System.Drawing.Size(118, 20);
            this.NotificationsCB.TabIndex = 0;
            this.NotificationsCB.Text = "Уведомления";
            this.NotificationsCB.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupEndDate);
            this.tabPage2.Controls.Add(this.groupStartDate);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(648, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Выбор интервала";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupEndDate
            // 
            this.groupEndDate.Controls.Add(this.dateTimePickerStop);
            this.groupEndDate.Controls.Add(this.SecEnd);
            this.groupEndDate.Controls.Add(this.MinEnd);
            this.groupEndDate.Controls.Add(this.HourEnd);
            this.groupEndDate.Controls.Add(this.DayEnd);
            this.groupEndDate.Controls.Add(this.label8);
            this.groupEndDate.Controls.Add(this.label9);
            this.groupEndDate.Controls.Add(this.label10);
            this.groupEndDate.Controls.Add(this.label11);
            this.groupEndDate.Controls.Add(this.label12);
            this.groupEndDate.Controls.Add(this.EndSign);
            this.groupEndDate.Controls.Add(this.label2);
            this.groupEndDate.Controls.Add(this.EndDateAbs);
            this.groupEndDate.Controls.Add(this.EndDataOtnosit);
            this.groupEndDate.Location = new System.Drawing.Point(16, 139);
            this.groupEndDate.Name = "groupEndDate";
            this.groupEndDate.Size = new System.Drawing.Size(636, 135);
            this.groupEndDate.TabIndex = 3;
            this.groupEndDate.TabStop = false;
            this.groupEndDate.Text = "Конечная дата";
            // 
            // dateTimePickerStop
            // 
            this.dateTimePickerStop.Location = new System.Drawing.Point(143, 92);
            this.dateTimePickerStop.Name = "dateTimePickerStop";
            this.dateTimePickerStop.Size = new System.Drawing.Size(143, 22);
            this.dateTimePickerStop.TabIndex = 16;
            // 
            // SecEnd
            // 
            this.SecEnd.Location = new System.Drawing.Point(389, 92);
            this.SecEnd.Name = "SecEnd";
            this.SecEnd.Size = new System.Drawing.Size(36, 22);
            this.SecEnd.TabIndex = 12;
            this.SecEnd.Text = "0";
            // 
            // MinEnd
            // 
            this.MinEnd.Location = new System.Drawing.Point(346, 92);
            this.MinEnd.Name = "MinEnd";
            this.MinEnd.Size = new System.Drawing.Size(36, 22);
            this.MinEnd.TabIndex = 13;
            this.MinEnd.Text = "0";
            // 
            // HourEnd
            // 
            this.HourEnd.Location = new System.Drawing.Point(301, 92);
            this.HourEnd.Name = "HourEnd";
            this.HourEnd.Size = new System.Drawing.Size(36, 22);
            this.HourEnd.TabIndex = 14;
            this.HourEnd.Text = "0";
            // 
            // DayEnd
            // 
            this.DayEnd.Location = new System.Drawing.Point(255, 91);
            this.DayEnd.Name = "DayEnd";
            this.DayEnd.Size = new System.Drawing.Size(36, 22);
            this.DayEnd.TabIndex = 15;
            this.DayEnd.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(386, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 16);
            this.label8.TabIndex = 8;
            this.label8.Text = "Сек.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(343, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 16);
            this.label9.TabIndex = 9;
            this.label9.Text = "Мин.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(297, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "Часы";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(259, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 11;
            this.label11.Text = "Дни";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 16);
            this.label12.TabIndex = 7;
            this.label12.Text = "Дата, время:";
            // 
            // EndSign
            // 
            this.EndSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EndSign.FormattingEnabled = true;
            this.EndSign.Items.AddRange(new object[] {
            "Сейчас",
            "Текущая смена",
            "Смена плюс",
            "Смена минус",
            "Сейчас плюс",
            "Сейчас минус",
            "Сегодня плюс",
            "Сегодня минус"});
            this.EndSign.Location = new System.Drawing.Point(113, 89);
            this.EndSign.Name = "EndSign";
            this.EndSign.Size = new System.Drawing.Size(136, 24);
            this.EndSign.TabIndex = 6;
            this.EndSign.SelectedIndexChanged += new System.EventHandler(this.EndSign_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Режим даты:";
            // 
            // EndDateAbs
            // 
            this.EndDateAbs.AutoSize = true;
            this.EndDateAbs.Location = new System.Drawing.Point(305, 29);
            this.EndDateAbs.Name = "EndDateAbs";
            this.EndDateAbs.Size = new System.Drawing.Size(109, 20);
            this.EndDateAbs.TabIndex = 4;
            this.EndDateAbs.Text = "Абсолютное";
            this.EndDateAbs.UseVisualStyleBackColor = true;
            this.EndDateAbs.CheckedChanged += new System.EventHandler(this.EndDateAbs_CheckedChanged);
            // 
            // EndDataOtnosit
            // 
            this.EndDataOtnosit.AutoSize = true;
            this.EndDataOtnosit.Checked = true;
            this.EndDataOtnosit.Location = new System.Drawing.Point(113, 29);
            this.EndDataOtnosit.Name = "EndDataOtnosit";
            this.EndDataOtnosit.Size = new System.Drawing.Size(186, 20);
            this.EndDataOtnosit.TabIndex = 3;
            this.EndDataOtnosit.TabStop = true;
            this.EndDataOtnosit.Text = "Относительно текущего";
            this.EndDataOtnosit.UseVisualStyleBackColor = true;
            this.EndDataOtnosit.CheckedChanged += new System.EventHandler(this.EndDataOtnosit_CheckedChanged);
            // 
            // groupStartDate
            // 
            this.groupStartDate.Controls.Add(this.dateTimePickerStart);
            this.groupStartDate.Controls.Add(this.SecStart);
            this.groupStartDate.Controls.Add(this.MinStart);
            this.groupStartDate.Controls.Add(this.HourStart);
            this.groupStartDate.Controls.Add(this.DayStart);
            this.groupStartDate.Controls.Add(this.label7);
            this.groupStartDate.Controls.Add(this.label6);
            this.groupStartDate.Controls.Add(this.label5);
            this.groupStartDate.Controls.Add(this.label4);
            this.groupStartDate.Controls.Add(this.label3);
            this.groupStartDate.Controls.Add(this.StartSign);
            this.groupStartDate.Controls.Add(this.label1);
            this.groupStartDate.Controls.Add(this.StartDateAbs);
            this.groupStartDate.Controls.Add(this.StartDataOtnosit);
            this.groupStartDate.Location = new System.Drawing.Point(16, 6);
            this.groupStartDate.Name = "groupStartDate";
            this.groupStartDate.Size = new System.Drawing.Size(636, 127);
            this.groupStartDate.TabIndex = 2;
            this.groupStartDate.TabStop = false;
            this.groupStartDate.Text = "Начальная дата";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Location = new System.Drawing.Point(144, 84);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(143, 22);
            this.dateTimePickerStart.TabIndex = 6;
            // 
            // SecStart
            // 
            this.SecStart.Location = new System.Drawing.Point(390, 84);
            this.SecStart.Name = "SecStart";
            this.SecStart.Size = new System.Drawing.Size(36, 22);
            this.SecStart.TabIndex = 5;
            this.SecStart.Text = "0";
            // 
            // MinStart
            // 
            this.MinStart.Location = new System.Drawing.Point(347, 84);
            this.MinStart.Name = "MinStart";
            this.MinStart.Size = new System.Drawing.Size(36, 22);
            this.MinStart.TabIndex = 5;
            this.MinStart.Text = "0";
            // 
            // HourStart
            // 
            this.HourStart.Location = new System.Drawing.Point(302, 84);
            this.HourStart.Name = "HourStart";
            this.HourStart.Size = new System.Drawing.Size(36, 22);
            this.HourStart.TabIndex = 5;
            this.HourStart.Text = "0";
            // 
            // DayStart
            // 
            this.DayStart.Location = new System.Drawing.Point(256, 83);
            this.DayStart.Name = "DayStart";
            this.DayStart.Size = new System.Drawing.Size(36, 22);
            this.DayStart.TabIndex = 5;
            this.DayStart.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(387, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Сек.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Мин.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Часы";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Дни";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Дата, время:";
            // 
            // StartSign
            // 
            this.StartSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StartSign.FormattingEnabled = true;
            this.StartSign.Items.AddRange(new object[] {
            "Сейчас",
            "Текущая смена",
            "Смена плюс",
            "Смена минус",
            "Сейчас плюс",
            "Сейчас минус",
            "Сегодня плюс",
            "Сегодня минус"});
            this.StartSign.Location = new System.Drawing.Point(114, 81);
            this.StartSign.Name = "StartSign";
            this.StartSign.Size = new System.Drawing.Size(136, 24);
            this.StartSign.TabIndex = 2;
            this.StartSign.SelectedIndexChanged += new System.EventHandler(this.StartSign_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Режим даты:";
            // 
            // StartDateAbs
            // 
            this.StartDateAbs.AutoSize = true;
            this.StartDateAbs.Location = new System.Drawing.Point(305, 23);
            this.StartDateAbs.Name = "StartDateAbs";
            this.StartDateAbs.Size = new System.Drawing.Size(109, 20);
            this.StartDateAbs.TabIndex = 1;
            this.StartDateAbs.Text = "Абсолютное";
            this.StartDateAbs.UseVisualStyleBackColor = true;
            this.StartDateAbs.CheckedChanged += new System.EventHandler(this.StartDateAbs_CheckedChanged);
            // 
            // StartDataOtnosit
            // 
            this.StartDataOtnosit.AutoSize = true;
            this.StartDataOtnosit.Checked = true;
            this.StartDataOtnosit.Location = new System.Drawing.Point(113, 23);
            this.StartDataOtnosit.Name = "StartDataOtnosit";
            this.StartDataOtnosit.Size = new System.Drawing.Size(186, 20);
            this.StartDataOtnosit.TabIndex = 0;
            this.StartDataOtnosit.TabStop = true;
            this.StartDataOtnosit.Text = "Относительно текущего";
            this.StartDataOtnosit.UseVisualStyleBackColor = true;
            this.StartDataOtnosit.CheckedChanged += new System.EventHandler(this.StartDataOtnosit_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cbAutoFillingEvents);
            this.tabPage3.Controls.Add(this.OperatorActionsTB);
            this.tabPage3.Controls.Add(this.ViolationsTB);
            this.tabPage3.Controls.Add(this.NotificationsTB);
            this.tabPage3.Controls.Add(this.AlarmsTB);
            this.tabPage3.Controls.Add(this.UnlockingKeysTB);
            this.tabPage3.Controls.Add(this.txtDateTimeFormat);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.NameAtributsCB);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(648, 398);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Формат";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // OperatorActionsTB
            // 
            this.OperatorActionsTB.Location = new System.Drawing.Point(393, 190);
            this.OperatorActionsTB.Name = "OperatorActionsTB";
            this.OperatorActionsTB.Size = new System.Drawing.Size(158, 22);
            this.OperatorActionsTB.TabIndex = 12;
            // 
            // ViolationsTB
            // 
            this.ViolationsTB.Location = new System.Drawing.Point(393, 149);
            this.ViolationsTB.Name = "ViolationsTB";
            this.ViolationsTB.Size = new System.Drawing.Size(158, 22);
            this.ViolationsTB.TabIndex = 11;
            // 
            // NotificationsTB
            // 
            this.NotificationsTB.Location = new System.Drawing.Point(393, 24);
            this.NotificationsTB.Name = "NotificationsTB";
            this.NotificationsTB.Size = new System.Drawing.Size(158, 22);
            this.NotificationsTB.TabIndex = 10;
            this.NotificationsTB.Text = "Notification";
            // 
            // AlarmsTB
            // 
            this.AlarmsTB.Location = new System.Drawing.Point(393, 63);
            this.AlarmsTB.Name = "AlarmsTB";
            this.AlarmsTB.Size = new System.Drawing.Size(158, 22);
            this.AlarmsTB.TabIndex = 9;
            this.AlarmsTB.Text = "Alarms";
            // 
            // UnlockingKeysTB
            // 
            this.UnlockingKeysTB.Location = new System.Drawing.Point(393, 109);
            this.UnlockingKeysTB.Name = "UnlockingKeysTB";
            this.UnlockingKeysTB.Size = new System.Drawing.Size(158, 22);
            this.UnlockingKeysTB.TabIndex = 8;
            this.UnlockingKeysTB.Text = "DK";
            // 
            // txtDateTimeFormat
            // 
            this.txtDateTimeFormat.Location = new System.Drawing.Point(393, 233);
            this.txtDateTimeFormat.Name = "txtDateTimeFormat";
            this.txtDateTimeFormat.Size = new System.Drawing.Size(158, 22);
            this.txtDateTimeFormat.TabIndex = 7;
            this.txtDateTimeFormat.Text = "dd.MM.yyyy HH:mm:ss";
            this.txtDateTimeFormat.TextChanged += new System.EventHandler(this.txtDateTimeFormat_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(40, 233);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(164, 16);
            this.label18.TabIndex = 6;
            this.label18.Text = "Формат даты и времени";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // NameAtributsCB
            // 
            this.NameAtributsCB.AutoSize = true;
            this.NameAtributsCB.Checked = true;
            this.NameAtributsCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NameAtributsCB.Location = new System.Drawing.Point(42, 273);
            this.NameAtributsCB.Name = "NameAtributsCB";
            this.NameAtributsCB.Size = new System.Drawing.Size(226, 20);
            this.NameAtributsCB.TabIndex = 5;
            this.NameAtributsCB.Text = "Отображать имена атрибутов";
            this.NameAtributsCB.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(38, 193);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(313, 16);
            this.label17.TabIndex = 4;
            this.label17.Text = "Имя страницы событий «Действия оператора» ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(38, 152);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(282, 16);
            this.label16.TabIndex = 3;
            this.label16.Text = "Имя страницы событий «Нарушения НТР» ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(38, 109);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(342, 16);
            this.label15.TabIndex = 2;
            this.label15.Text = "Имя страницы событий «Деблокировочные ключи» ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(272, 16);
            this.label14.TabIndex = 1;
            this.label14.Text = "Имя страницы событий «Сигнализации» ";
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(38, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(266, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Имя страницы событий «Уведомления» ";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(503, 440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Получить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(285, 444);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(104, 25);
            this.progressBarLoading.TabIndex = 2;
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Location = new System.Drawing.Point(185, 450);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(78, 16);
            this.labelLoading.TabIndex = 3;
            this.labelLoading.Text = "Загрузка...";
            this.labelLoading.Click += new System.EventHandler(this.label19_Click);
            // 
            // cbAutoFillingEvents
            // 
            this.cbAutoFillingEvents.AutoSize = true;
            this.cbAutoFillingEvents.Location = new System.Drawing.Point(41, 312);
            this.cbAutoFillingEvents.Name = "cbAutoFillingEvents";
            this.cbAutoFillingEvents.Size = new System.Drawing.Size(318, 36);
            this.cbAutoFillingEvents.TabIndex = 19;
            this.cbAutoFillingEvents.Text = "Автоматически заполнять отчет событиями\r\nпри открытии файла";
            this.cbAutoFillingEvents.UseVisualStyleBackColor = true;
            // 
            // EventsReadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 482);
            this.Controls.Add(this.labelLoading);
            this.Controls.Add(this.progressBarLoading);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EventsReadingForm";
            this.Text = "Чтение событий";
            this.tabControl1.ResumeLayout(false);
            this.tabPageAssets.ResumeLayout(false);
            this.tabPageAssets.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupEndDate.ResumeLayout(false);
            this.groupEndDate.PerformLayout();
            this.groupStartDate.ResumeLayout(false);
            this.groupStartDate.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox ViolationsHTP_CB;
        private System.Windows.Forms.CheckBox UnlockingKeysCB;
        private System.Windows.Forms.CheckBox AlarmsCB;
        private System.Windows.Forms.CheckBox NotificationsCB;
        private System.Windows.Forms.CheckBox OperatorActionsCB;
        private System.Windows.Forms.GroupBox groupEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStop;
        private System.Windows.Forms.TextBox SecEnd;
        private System.Windows.Forms.TextBox MinEnd;
        private System.Windows.Forms.TextBox HourEnd;
        private System.Windows.Forms.TextBox DayEnd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox EndSign;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton EndDateAbs;
        private System.Windows.Forms.RadioButton EndDataOtnosit;
        private System.Windows.Forms.GroupBox groupStartDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.TextBox SecStart;
        private System.Windows.Forms.TextBox MinStart;
        private System.Windows.Forms.TextBox HourStart;
        private System.Windows.Forms.TextBox DayStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox StartSign;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton StartDateAbs;
        private System.Windows.Forms.RadioButton StartDataOtnosit;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox NameAtributsCB;
        private System.Windows.Forms.TextBox txtDateTimeFormat;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox OperatorActionsTB;
        private System.Windows.Forms.TextBox ViolationsTB;
        private System.Windows.Forms.TextBox NotificationsTB;
        private System.Windows.Forms.TextBox AlarmsTB;
        private System.Windows.Forms.TextBox UnlockingKeysTB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPageAssets;
        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.TreeView treeAssets;
        private System.Windows.Forms.CheckedListBox SelectedAssetsCLB;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox cbAutoFillingEvents;
    }
}