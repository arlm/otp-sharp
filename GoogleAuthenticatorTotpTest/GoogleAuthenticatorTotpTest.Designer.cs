namespace GoogleAuthenticatorTotpTest
{
    partial class GoogleAuthenticatorTotpTest
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTotp = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.labelDescription = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelRemaining = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonVerify = new System.Windows.Forms.Button();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.labelPeriod = new System.Windows.Forms.Label();
            this.labelKeyLabel = new System.Windows.Forms.Label();
            this.textBoxKeyLabel = new System.Windows.Forms.TextBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.radioButtonSix = new System.Windows.Forms.RadioButton();
            this.radioButtonEight = new System.Windows.Forms.RadioButton();
            this.labelWarning = new System.Windows.Forms.Label();
            this.textBoxIssuer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(212, 92);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(204, 207);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelTotp
            // 
            this.labelTotp.AutoSize = true;
            this.labelTotp.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotp.Location = new System.Drawing.Point(32, 14);
            this.labelTotp.Name = "labelTotp";
            this.labelTotp.Size = new System.Drawing.Size(93, 36);
            this.labelTotp.TabIndex = 1;
            this.labelTotp.Text = "TOTP";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(9, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(407, 30);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Download the Google Authenticator app to your smartphone and scan the QR code to " +
    "demonstrate the OtpSharp TOTP implementation.";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 316);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(408, 210);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelRemaining);
            this.tabPage1.Controls.Add(this.labelTotp);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(400, 184);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Display";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // labelRemaining
            // 
            this.labelRemaining.AutoSize = true;
            this.labelRemaining.Location = new System.Drawing.Point(58, 50);
            this.labelRemaining.Name = "labelRemaining";
            this.labelRemaining.Size = new System.Drawing.Size(19, 13);
            this.labelRemaining.TabIndex = 2;
            this.labelRemaining.Text = "30";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonVerify);
            this.tabPage2.Controls.Add(this.textBoxValue);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(400, 184);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Verify";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonVerify
            // 
            this.buttonVerify.Location = new System.Drawing.Point(122, 21);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(48, 23);
            this.buttonVerify.TabIndex = 1;
            this.buttonVerify.Text = "Verify";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Click += new System.EventHandler(this.buttonVerify_Click);
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(16, 23);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 0;
            this.textBoxValue.TextChanged += new System.EventHandler(this.textBoxValue_TextChanged);
            this.textBoxValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxValue_KeyDown);
            // 
            // textBoxPeriod
            // 
            this.textBoxPeriod.Location = new System.Drawing.Point(138, 169);
            this.textBoxPeriod.Name = "textBoxPeriod";
            this.textBoxPeriod.Size = new System.Drawing.Size(45, 20);
            this.textBoxPeriod.TabIndex = 3;
            this.textBoxPeriod.Text = "30";
            this.textBoxPeriod.TextChanged += new System.EventHandler(this.textBoxPeriod_TextChanged);
            // 
            // labelPeriod
            // 
            this.labelPeriod.AutoSize = true;
            this.labelPeriod.Location = new System.Drawing.Point(18, 171);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(114, 13);
            this.labelPeriod.TabIndex = 4;
            this.labelPeriod.Text = "Time Period (Seconds)";
            // 
            // labelKeyLabel
            // 
            this.labelKeyLabel.AutoSize = true;
            this.labelKeyLabel.Location = new System.Drawing.Point(19, 92);
            this.labelKeyLabel.Name = "labelKeyLabel";
            this.labelKeyLabel.Size = new System.Drawing.Size(54, 13);
            this.labelKeyLabel.TabIndex = 5;
            this.labelKeyLabel.Text = "Key Label";
            // 
            // textBoxKeyLabel
            // 
            this.textBoxKeyLabel.Location = new System.Drawing.Point(22, 108);
            this.textBoxKeyLabel.Name = "textBoxKeyLabel";
            this.textBoxKeyLabel.Size = new System.Drawing.Size(162, 20);
            this.textBoxKeyLabel.TabIndex = 6;
            this.textBoxKeyLabel.Text = "OtpSharp@test.com";
            this.textBoxKeyLabel.TextChanged += new System.EventHandler(this.textBoxKeyLabel_TextChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(18, 197);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(84, 13);
            this.labelSize.TabIndex = 8;
            this.labelSize.Text = "TOTP code size";
            // 
            // radioButtonSix
            // 
            this.radioButtonSix.AutoSize = true;
            this.radioButtonSix.Checked = true;
            this.radioButtonSix.Location = new System.Drawing.Point(21, 214);
            this.radioButtonSix.Name = "radioButtonSix";
            this.radioButtonSix.Size = new System.Drawing.Size(60, 17);
            this.radioButtonSix.TabIndex = 9;
            this.radioButtonSix.TabStop = true;
            this.radioButtonSix.Text = "6 Digits";
            this.radioButtonSix.UseVisualStyleBackColor = true;
            this.radioButtonSix.CheckedChanged += new System.EventHandler(this.radioButtonSix_CheckedChanged);
            // 
            // radioButtonEight
            // 
            this.radioButtonEight.AutoSize = true;
            this.radioButtonEight.Location = new System.Drawing.Point(21, 238);
            this.radioButtonEight.Name = "radioButtonEight";
            this.radioButtonEight.Size = new System.Drawing.Size(60, 17);
            this.radioButtonEight.TabIndex = 10;
            this.radioButtonEight.Text = "8 Digits";
            this.radioButtonEight.UseVisualStyleBackColor = true;
            this.radioButtonEight.CheckedChanged += new System.EventHandler(this.radioButtonEight_CheckedChanged);
            // 
            // labelWarning
            // 
            this.labelWarning.Location = new System.Drawing.Point(19, 51);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(401, 26);
            this.labelWarning.TabIndex = 11;
            this.labelWarning.Text = "Warning.  The Google Authenticator doesn\'t currently support anything except thir" +
    "ty second, six digit codes.  Other third party apps may however.\r\n";
            // 
            // textBoxIssuer
            // 
            this.textBoxIssuer.Location = new System.Drawing.Point(22, 147);
            this.textBoxIssuer.Name = "textBoxIssuer";
            this.textBoxIssuer.Size = new System.Drawing.Size(162, 20);
            this.textBoxIssuer.TabIndex = 13;
            this.textBoxIssuer.Text = "OtpSharp-Test";
            this.textBoxIssuer.TextChanged += new System.EventHandler(this.textBoxIssuer_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Issuer";
            // 
            // GoogleAuthenticatorTotpTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 538);
            this.Controls.Add(this.textBoxIssuer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.radioButtonEight);
            this.Controls.Add(this.radioButtonSix);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.textBoxKeyLabel);
            this.Controls.Add(this.labelKeyLabel);
            this.Controls.Add(this.labelPeriod);
            this.Controls.Add(this.textBoxPeriod);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.pictureBox1);
            this.Name = "GoogleAuthenticatorTotpTest";
            this.Text = "Google Authenticator Totp Demonstration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTotp;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label labelRemaining;
        private System.Windows.Forms.TextBox textBoxPeriod;
        private System.Windows.Forms.Label labelPeriod;
        private System.Windows.Forms.Label labelKeyLabel;
        private System.Windows.Forms.TextBox textBoxKeyLabel;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.RadioButton radioButtonSix;
        private System.Windows.Forms.RadioButton radioButtonEight;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.TextBox textBoxIssuer;
        private System.Windows.Forms.Label label1;
    }
}

