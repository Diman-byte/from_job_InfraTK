namespace ExcelReportingAddin
{
    partial class DataReadingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataReadingForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listTags = new System.Windows.Forms.CheckedListBox();
            this.treeAssets = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labelSlice = new System.Windows.Forms.Label();
            this.comboBoxTypeSlice = new System.Windows.Forms.ComboBox();
            this.txtSliceCount = new System.Windows.Forms.TextBox();
            this.rbSlices = new System.Windows.Forms.RadioButton();
            this.rbAllValues = new System.Windows.Forms.RadioButton();
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
            this.chkShowTagDescriptions = new System.Windows.Forms.CheckBox();
            this.chkShowTagNames = new System.Windows.Forms.CheckBox();
            this.txtDateTimeFormat = new System.Windows.Forms.TextBox();
            this.txtSheetName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelLoading = new System.Windows.Forms.Label();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.LoadTagsButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupEndDate.SuspendLayout();
            this.groupStartDate.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(837, 573);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listTags);
            this.tabPage1.Controls.Add(this.treeAssets);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(829, 544);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Выбор активов/тегов";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // listTags
            // 
            this.listTags.CheckOnClick = true;
            this.listTags.FormattingEnabled = true;
            this.listTags.Location = new System.Drawing.Point(415, 6);
            this.listTags.Name = "listTags";
            this.listTags.Size = new System.Drawing.Size(408, 531);
            this.listTags.TabIndex = 2;
            this.listTags.SelectedIndexChanged += new System.EventHandler(this.listTags_CheckedChanged);
            // 
            // treeAssets
            // 
            this.treeAssets.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeAssets.Location = new System.Drawing.Point(6, 6);
            this.treeAssets.Name = "treeAssets";
            this.treeAssets.Size = new System.Drawing.Size(403, 532);
            this.treeAssets.TabIndex = 0;
            this.treeAssets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAssets_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labelSlice);
            this.tabPage2.Controls.Add(this.comboBoxTypeSlice);
            this.tabPage2.Controls.Add(this.txtSliceCount);
            this.tabPage2.Controls.Add(this.rbSlices);
            this.tabPage2.Controls.Add(this.rbAllValues);
            this.tabPage2.Controls.Add(this.groupEndDate);
            this.tabPage2.Controls.Add(this.groupStartDate);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(829, 544);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Выбор интервала";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // labelSlice
            // 
            this.labelSlice.AutoSize = true;
            this.labelSlice.Location = new System.Drawing.Point(147, 320);
            this.labelSlice.Name = "labelSlice";
            this.labelSlice.Size = new System.Drawing.Size(74, 16);
            this.labelSlice.TabIndex = 5;
            this.labelSlice.Text = "Тип среза";
            this.labelSlice.Visible = false;
            this.labelSlice.Click += new System.EventHandler(this.label15_Click);
            // 
            // comboBoxTypeSlice
            // 
            this.comboBoxTypeSlice.Items.AddRange(new object[] {
            "FirstPoint",
            "LastPoint",
            "MinMaxPoints"});
            this.comboBoxTypeSlice.Location = new System.Drawing.Point(150, 343);
            this.comboBoxTypeSlice.Name = "comboBoxTypeSlice";
            this.comboBoxTypeSlice.Size = new System.Drawing.Size(139, 24);
            this.comboBoxTypeSlice.TabIndex = 4;
            this.comboBoxTypeSlice.Visible = false;
            // 
            // txtSliceCount
            // 
            this.txtSliceCount.Location = new System.Drawing.Point(232, 280);
            this.txtSliceCount.Name = "txtSliceCount";
            this.txtSliceCount.Size = new System.Drawing.Size(57, 22);
            this.txtSliceCount.TabIndex = 3;
            this.txtSliceCount.Text = "1000";
            this.txtSliceCount.Visible = false;
            // 
            // rbSlices
            // 
            this.rbSlices.AutoSize = true;
            this.rbSlices.Location = new System.Drawing.Point(150, 280);
            this.rbSlices.Name = "rbSlices";
            this.rbSlices.Size = new System.Drawing.Size(70, 20);
            this.rbSlices.TabIndex = 2;
            this.rbSlices.Text = "Срезы";
            this.rbSlices.UseVisualStyleBackColor = true;
            this.rbSlices.CheckedChanged += new System.EventHandler(this.rbSlices_CheckedChanged);
            // 
            // rbAllValues
            // 
            this.rbAllValues.AutoSize = true;
            this.rbAllValues.Checked = true;
            this.rbAllValues.Location = new System.Drawing.Point(26, 280);
            this.rbAllValues.Name = "rbAllValues";
            this.rbAllValues.Size = new System.Drawing.Size(118, 20);
            this.rbAllValues.TabIndex = 2;
            this.rbAllValues.TabStop = true;
            this.rbAllValues.Text = "Все значения";
            this.rbAllValues.UseVisualStyleBackColor = true;
            this.rbAllValues.CheckedChanged += new System.EventHandler(this.rbAllValues_CheckedChanged);
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
            this.groupEndDate.Location = new System.Drawing.Point(17, 139);
            this.groupEndDate.Name = "groupEndDate";
            this.groupEndDate.Size = new System.Drawing.Size(636, 135);
            this.groupEndDate.TabIndex = 1;
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
            "Сейчас плюс",
            "Сейчас минус",
            "Сегодня плюс",
            "Сегодня минус"});
            this.EndSign.Location = new System.Drawing.Point(113, 89);
            this.EndSign.Name = "EndSign";
            this.EndSign.Size = new System.Drawing.Size(136, 24);
            this.EndSign.TabIndex = 6;
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
            this.groupStartDate.Location = new System.Drawing.Point(17, 6);
            this.groupStartDate.Name = "groupStartDate";
            this.groupStartDate.Size = new System.Drawing.Size(636, 127);
            this.groupStartDate.TabIndex = 0;
            this.groupStartDate.TabStop = false;
            this.groupStartDate.Text = "Начальная дата";
            this.groupStartDate.Enter += new System.EventHandler(this.groupStartDate_Enter);
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
            this.SecStart.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // MinStart
            // 
            this.MinStart.Location = new System.Drawing.Point(347, 84);
            this.MinStart.Name = "MinStart";
            this.MinStart.Size = new System.Drawing.Size(36, 22);
            this.MinStart.TabIndex = 5;
            this.MinStart.Text = "0";
            this.MinStart.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // HourStart
            // 
            this.HourStart.Location = new System.Drawing.Point(302, 84);
            this.HourStart.Name = "HourStart";
            this.HourStart.Size = new System.Drawing.Size(36, 22);
            this.HourStart.TabIndex = 5;
            this.HourStart.Text = "1";
            this.HourStart.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DayStart
            // 
            this.DayStart.Location = new System.Drawing.Point(256, 83);
            this.DayStart.Name = "DayStart";
            this.DayStart.Size = new System.Drawing.Size(36, 22);
            this.DayStart.TabIndex = 5;
            this.DayStart.Text = "0";
            this.DayStart.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Часы";
            this.label5.Click += new System.EventHandler(this.label5_Click);
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
            this.tabPage3.Controls.Add(this.chkShowTagDescriptions);
            this.tabPage3.Controls.Add(this.chkShowTagNames);
            this.tabPage3.Controls.Add(this.txtDateTimeFormat);
            this.tabPage3.Controls.Add(this.txtSheetName);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(829, 544);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Формат";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkShowTagDescriptions
            // 
            this.chkShowTagDescriptions.AutoSize = true;
            this.chkShowTagDescriptions.Location = new System.Drawing.Point(85, 168);
            this.chkShowTagDescriptions.Name = "chkShowTagDescriptions";
            this.chkShowTagDescriptions.Size = new System.Drawing.Size(215, 20);
            this.chkShowTagDescriptions.TabIndex = 3;
            this.chkShowTagDescriptions.Text = "Отображать описание тегов";
            this.chkShowTagDescriptions.UseVisualStyleBackColor = true;
            // 
            // chkShowTagNames
            // 
            this.chkShowTagNames.AutoSize = true;
            this.chkShowTagNames.Checked = true;
            this.chkShowTagNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowTagNames.Location = new System.Drawing.Point(85, 129);
            this.chkShowTagNames.Name = "chkShowTagNames";
            this.chkShowTagNames.Size = new System.Drawing.Size(193, 20);
            this.chkShowTagNames.TabIndex = 2;
            this.chkShowTagNames.Text = "Отображать имена тегов";
            this.chkShowTagNames.UseVisualStyleBackColor = true;
            // 
            // txtDateTimeFormat
            // 
            this.txtDateTimeFormat.Location = new System.Drawing.Point(318, 77);
            this.txtDateTimeFormat.Name = "txtDateTimeFormat";
            this.txtDateTimeFormat.Size = new System.Drawing.Size(158, 22);
            this.txtDateTimeFormat.TabIndex = 1;
            this.txtDateTimeFormat.Text = "dd.MM.yyyy HH:mm:ss";
            // 
            // txtSheetName
            // 
            this.txtSheetName.Location = new System.Drawing.Point(318, 40);
            this.txtSheetName.Name = "txtSheetName";
            this.txtSheetName.Size = new System.Drawing.Size(158, 22);
            this.txtSheetName.TabIndex = 1;
            this.txtSheetName.Text = "Data";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(53, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(164, 16);
            this.label14.TabIndex = 0;
            this.label14.Text = "Формат даты и времени";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(240, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Имя страницы для выгрузки данных";
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Location = new System.Drawing.Point(359, 605);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(78, 16);
            this.labelLoading.TabIndex = 5;
            this.labelLoading.Text = "Загрузка...";
            this.labelLoading.Visible = false;
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(443, 607);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(85, 14);
            this.progressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoading.TabIndex = 4;
            this.progressBarLoading.Visible = false;
            // 
            // LoadTagsButton
            // 
            this.LoadTagsButton.Location = new System.Drawing.Point(635, 587);
            this.LoadTagsButton.Name = "LoadTagsButton";
            this.LoadTagsButton.Size = new System.Drawing.Size(204, 36);
            this.LoadTagsButton.TabIndex = 6;
            this.LoadTagsButton.Text = "Загрузить данные";
            this.LoadTagsButton.UseCompatibleTextRendering = true;
            this.LoadTagsButton.UseVisualStyleBackColor = true;
            this.LoadTagsButton.Click += new System.EventHandler(this.LoadTagsButton_Click);
            // 
            // DataReadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 630);
            this.Controls.Add(this.LoadTagsButton);
            this.Controls.Add(this.labelLoading);
            this.Controls.Add(this.progressBarLoading);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataReadingForm";
            this.Text = "Чтение данных";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TreeView treeAssets;
        private System.Windows.Forms.GroupBox groupStartDate;
        private System.Windows.Forms.GroupBox groupEndDate;
        private System.Windows.Forms.RadioButton StartDataOtnosit;
        private System.Windows.Forms.RadioButton StartDateAbs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton EndDateAbs;
        private System.Windows.Forms.RadioButton EndDataOtnosit;
        private System.Windows.Forms.ComboBox StartSign;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox DayStart;
        private System.Windows.Forms.TextBox SecStart;
        private System.Windows.Forms.TextBox MinStart;
        private System.Windows.Forms.TextBox HourStart;
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
        private System.Windows.Forms.RadioButton rbAllValues;
        private System.Windows.Forms.RadioButton rbSlices;
        private System.Windows.Forms.TextBox txtSliceCount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSheetName;
        private System.Windows.Forms.TextBox txtDateTimeFormat;
        private System.Windows.Forms.CheckBox chkShowTagNames;
        private System.Windows.Forms.CheckBox chkShowTagDescriptions;
        private System.Windows.Forms.CheckedListBox listTags;
        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.Button LoadTagsButton;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerStop;
        private System.Windows.Forms.ComboBox comboBoxTypeSlice;
        private System.Windows.Forms.Label labelSlice;
    }
}