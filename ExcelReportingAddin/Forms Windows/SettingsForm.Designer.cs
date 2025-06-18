namespace ExcelReportingAddin
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.txtDataServerAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDataServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyCloakAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRealm = new System.Windows.Forms.TextBox();
            this.txtScope = new System.Windows.Forms.TextBox();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDataServerAddress
            // 
            this.txtDataServerAddress.Location = new System.Drawing.Point(217, 17);
            this.txtDataServerAddress.Name = "txtDataServerAddress";
            this.txtDataServerAddress.Size = new System.Drawing.Size(152, 22);
            this.txtDataServerAddress.TabIndex = 0;
            this.txtDataServerAddress.Text = "localhost";
            this.txtDataServerAddress.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Адрес сервера DataServer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт сервера DataServer";
            // 
            // txtDataServerPort
            // 
            this.txtDataServerPort.Location = new System.Drawing.Point(217, 60);
            this.txtDataServerPort.Name = "txtDataServerPort";
            this.txtDataServerPort.Size = new System.Drawing.Size(152, 22);
            this.txtDataServerPort.TabIndex = 3;
            this.txtDataServerPort.Text = "5100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Адрес сервера KeyCloak";
            // 
            // txtKeyCloakAddress
            // 
            this.txtKeyCloakAddress.Location = new System.Drawing.Point(215, 130);
            this.txtKeyCloakAddress.Name = "txtKeyCloakAddress";
            this.txtKeyCloakAddress.Size = new System.Drawing.Size(152, 22);
            this.txtKeyCloakAddress.TabIndex = 5;
            this.txtKeyCloakAddress.Text = "localhost:8080";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Логин";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(215, 165);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(152, 22);
            this.txtUsername.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Пароль";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(215, 200);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(152, 22);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Location = new System.Drawing.Point(26, 345);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(341, 23);
            this.btnCheckConnection.TabIndex = 10;
            this.btnCheckConnection.Text = "Проверка подключения";
            this.btnCheckConnection.UseVisualStyleBackColor = true;
            this.btnCheckConnection.Click += new System.EventHandler(this.btnCheckConnection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Параметры Keycloak";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "Realm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 269);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 16);
            this.label8.TabIndex = 13;
            this.label8.Text = "Scope";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 16);
            this.label9.TabIndex = 14;
            this.label9.Text = "ClientId";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // txtRealm
            // 
            this.txtRealm.Location = new System.Drawing.Point(215, 234);
            this.txtRealm.Name = "txtRealm";
            this.txtRealm.Size = new System.Drawing.Size(152, 22);
            this.txtRealm.TabIndex = 15;
            // 
            // txtScope
            // 
            this.txtScope.Location = new System.Drawing.Point(215, 269);
            this.txtScope.Name = "txtScope";
            this.txtScope.Size = new System.Drawing.Size(152, 22);
            this.txtScope.TabIndex = 16;
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(215, 306);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(152, 22);
            this.txtClientId.TabIndex = 17;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 383);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.txtScope);
            this.Controls.Add(this.txtRealm);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCheckConnection);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKeyCloakAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDataServerPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDataServerAddress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Настройка подключения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDataServerAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDataServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyCloakAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCheckConnection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRealm;
        private System.Windows.Forms.TextBox txtScope;
        private System.Windows.Forms.TextBox txtClientId;
    }
}