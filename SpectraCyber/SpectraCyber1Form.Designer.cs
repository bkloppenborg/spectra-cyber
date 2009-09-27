namespace SpectraCyber
{
    partial class SpectraCyber1Form
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
            this.grpSCSettings = new System.Windows.Forms.GroupBox();
            this.grpFrequencyBounds = new System.Windows.Forms.GroupBox();
            this.nudPLLUpperFrequency = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLowerFrequency = new System.Windows.Forms.Label();
            this.nudPLLLowerFrequency = new System.Windows.Forms.NumericUpDown();
            this.nudDCOffset = new System.Windows.Forms.NumericUpDown();
            this.nudIFGain = new System.Windows.Forms.NumericUpDown();
            this.cboIntegration = new System.Windows.Forms.ComboBox();
            this.cboGain = new System.Windows.Forms.ComboBox();
            this.grpNoiseSource = new System.Windows.Forms.GroupBox();
            this.rdoNoiseSourceOff = new System.Windows.Forms.RadioButton();
            this.rdoNoiseSourceOn = new System.Windows.Forms.RadioButton();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtVoltage = new System.Windows.Forms.TextBox();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblVoltage = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.lblDCOffset = new System.Windows.Forms.Label();
            this.lblIntegration = new System.Windows.Forms.Label();
            this.lblGain = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblCommPort = new System.Windows.Forms.Label();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.txtCommPort = new System.Windows.Forms.TextBox();
            this.lblIFGain = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnScanStop = new System.Windows.Forms.Button();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstCommandWindow = new System.Windows.Forms.ListBox();
            this.chkExpectReply = new System.Windows.Forms.CheckBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grpSCSettings.SuspendLayout();
            this.grpFrequencyBounds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPLLUpperFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPLLLowerFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDCOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIFGain)).BeginInit();
            this.grpNoiseSource.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSCSettings
            // 
            this.grpSCSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSCSettings.Controls.Add(this.grpFrequencyBounds);
            this.grpSCSettings.Controls.Add(this.nudDCOffset);
            this.grpSCSettings.Controls.Add(this.nudIFGain);
            this.grpSCSettings.Controls.Add(this.cboIntegration);
            this.grpSCSettings.Controls.Add(this.cboGain);
            this.grpSCSettings.Controls.Add(this.grpNoiseSource);
            this.grpSCSettings.Controls.Add(this.grpStatus);
            this.grpSCSettings.Controls.Add(this.lblDCOffset);
            this.grpSCSettings.Controls.Add(this.lblIntegration);
            this.grpSCSettings.Controls.Add(this.lblGain);
            this.grpSCSettings.Controls.Add(this.lblMode);
            this.grpSCSettings.Controls.Add(this.lblCommPort);
            this.grpSCSettings.Controls.Add(this.cboMode);
            this.grpSCSettings.Controls.Add(this.txtCommPort);
            this.grpSCSettings.Controls.Add(this.lblIFGain);
            this.grpSCSettings.Controls.Add(this.btnReset);
            this.grpSCSettings.Controls.Add(this.btnScanStop);
            this.grpSCSettings.Location = new System.Drawing.Point(416, 27);
            this.grpSCSettings.Name = "grpSCSettings";
            this.grpSCSettings.Size = new System.Drawing.Size(206, 485);
            this.grpSCSettings.TabIndex = 0;
            this.grpSCSettings.TabStop = false;
            this.grpSCSettings.Text = "Spectra Cyber Settings";
            // 
            // grpFrequencyBounds
            // 
            this.grpFrequencyBounds.Controls.Add(this.nudPLLUpperFrequency);
            this.grpFrequencyBounds.Controls.Add(this.label1);
            this.grpFrequencyBounds.Controls.Add(this.lblLowerFrequency);
            this.grpFrequencyBounds.Controls.Add(this.nudPLLLowerFrequency);
            this.grpFrequencyBounds.Location = new System.Drawing.Point(7, 230);
            this.grpFrequencyBounds.Name = "grpFrequencyBounds";
            this.grpFrequencyBounds.Size = new System.Drawing.Size(193, 66);
            this.grpFrequencyBounds.TabIndex = 17;
            this.grpFrequencyBounds.TabStop = false;
            this.grpFrequencyBounds.Text = "Scan Range (kHz)";
            // 
            // nudPLLUpperFrequency
            // 
            this.nudPLLUpperFrequency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.nudPLLUpperFrequency.Location = new System.Drawing.Point(87, 40);
            this.nudPLLUpperFrequency.Name = "nudPLLUpperFrequency";
            this.nudPLLUpperFrequency.Size = new System.Drawing.Size(100, 20);
            this.nudPLLUpperFrequency.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Upper Bound";
            // 
            // lblLowerFrequency
            // 
            this.lblLowerFrequency.AutoSize = true;
            this.lblLowerFrequency.Location = new System.Drawing.Point(6, 16);
            this.lblLowerFrequency.Name = "lblLowerFrequency";
            this.lblLowerFrequency.Size = new System.Drawing.Size(70, 13);
            this.lblLowerFrequency.TabIndex = 1;
            this.lblLowerFrequency.Text = "Lower Bound";
            // 
            // nudPLLLowerFrequency
            // 
            this.nudPLLLowerFrequency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.nudPLLLowerFrequency.Location = new System.Drawing.Point(87, 14);
            this.nudPLLLowerFrequency.Name = "nudPLLLowerFrequency";
            this.nudPLLLowerFrequency.Size = new System.Drawing.Size(100, 20);
            this.nudPLLLowerFrequency.TabIndex = 0;
            // 
            // nudDCOffset
            // 
            this.nudDCOffset.DecimalPlaces = 3;
            this.nudDCOffset.Location = new System.Drawing.Point(94, 151);
            this.nudDCOffset.Name = "nudDCOffset";
            this.nudDCOffset.Size = new System.Drawing.Size(100, 20);
            this.nudDCOffset.TabIndex = 5;
            this.nudDCOffset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nudDCOffset_MouseDown);
            this.nudDCOffset.ValueChanged += new System.EventHandler(this.nudDCOffset_ValueChanged);
            this.nudDCOffset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.nudDCOffset_MouseUp);
            // 
            // nudIFGain
            // 
            this.nudIFGain.DecimalPlaces = 2;
            this.nudIFGain.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudIFGain.Location = new System.Drawing.Point(94, 73);
            this.nudIFGain.Name = "nudIFGain";
            this.nudIFGain.Size = new System.Drawing.Size(100, 20);
            this.nudIFGain.TabIndex = 5;
            this.nudIFGain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nudIFGain_MouseDown);
            this.nudIFGain.ValueChanged += new System.EventHandler(this.nudIFGain_ValueChanged);
            this.nudIFGain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.nudIFGain_MouseUp);
            // 
            // cboIntegration
            // 
            this.cboIntegration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIntegration.FormattingEnabled = true;
            this.cboIntegration.Location = new System.Drawing.Point(94, 124);
            this.cboIntegration.Name = "cboIntegration";
            this.cboIntegration.Size = new System.Drawing.Size(100, 21);
            this.cboIntegration.TabIndex = 16;
            this.cboIntegration.SelectedIndexChanged += new System.EventHandler(this.cboIntegration_SelectedIndexChanged);
            // 
            // cboGain
            // 
            this.cboGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGain.FormattingEnabled = true;
            this.cboGain.Location = new System.Drawing.Point(94, 98);
            this.cboGain.Name = "cboGain";
            this.cboGain.Size = new System.Drawing.Size(100, 21);
            this.cboGain.TabIndex = 15;
            this.cboGain.SelectedIndexChanged += new System.EventHandler(this.cboGain_SelectedIndexChanged);
            // 
            // grpNoiseSource
            // 
            this.grpNoiseSource.Controls.Add(this.rdoNoiseSourceOff);
            this.grpNoiseSource.Controls.Add(this.rdoNoiseSourceOn);
            this.grpNoiseSource.Location = new System.Drawing.Point(6, 177);
            this.grpNoiseSource.Name = "grpNoiseSource";
            this.grpNoiseSource.Size = new System.Drawing.Size(194, 46);
            this.grpNoiseSource.TabIndex = 2;
            this.grpNoiseSource.TabStop = false;
            this.grpNoiseSource.Text = "Noise Source";
            // 
            // rdoNoiseSourceOff
            // 
            this.rdoNoiseSourceOff.AutoSize = true;
            this.rdoNoiseSourceOff.Location = new System.Drawing.Point(52, 20);
            this.rdoNoiseSourceOff.Name = "rdoNoiseSourceOff";
            this.rdoNoiseSourceOff.Size = new System.Drawing.Size(39, 17);
            this.rdoNoiseSourceOff.TabIndex = 1;
            this.rdoNoiseSourceOff.TabStop = true;
            this.rdoNoiseSourceOff.Text = "Off";
            this.rdoNoiseSourceOff.UseVisualStyleBackColor = true;
            // 
            // rdoNoiseSourceOn
            // 
            this.rdoNoiseSourceOn.AutoSize = true;
            this.rdoNoiseSourceOn.Location = new System.Drawing.Point(7, 20);
            this.rdoNoiseSourceOn.Name = "rdoNoiseSourceOn";
            this.rdoNoiseSourceOn.Size = new System.Drawing.Size(39, 17);
            this.rdoNoiseSourceOn.TabIndex = 0;
            this.rdoNoiseSourceOn.TabStop = true;
            this.rdoNoiseSourceOn.Text = "On";
            this.rdoNoiseSourceOn.UseVisualStyleBackColor = true;
            this.rdoNoiseSourceOn.CheckedChanged += new System.EventHandler(this.rdoNoiseSourceOn_CheckedChanged);
            // 
            // grpStatus
            // 
            this.grpStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpStatus.Controls.Add(this.progressBar1);
            this.grpStatus.Controls.Add(this.txtVoltage);
            this.grpStatus.Controls.Add(this.txtFrequency);
            this.grpStatus.Controls.Add(this.lblProgress);
            this.grpStatus.Controls.Add(this.lblVoltage);
            this.grpStatus.Controls.Add(this.lblFrequency);
            this.grpStatus.Location = new System.Drawing.Point(9, 350);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(191, 100);
            this.grpStatus.TabIndex = 1;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "Status";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(71, 68);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // txtVoltage
            // 
            this.txtVoltage.Location = new System.Drawing.Point(71, 42);
            this.txtVoltage.Name = "txtVoltage";
            this.txtVoltage.ReadOnly = true;
            this.txtVoltage.Size = new System.Drawing.Size(100, 20);
            this.txtVoltage.TabIndex = 4;
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(71, 16);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.ReadOnly = true;
            this.txtFrequency.Size = new System.Drawing.Size(100, 20);
            this.txtFrequency.TabIndex = 3;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(7, 68);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(48, 13);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "Progress";
            // 
            // lblVoltage
            // 
            this.lblVoltage.AutoSize = true;
            this.lblVoltage.Location = new System.Drawing.Point(7, 45);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(43, 13);
            this.lblVoltage.TabIndex = 1;
            this.lblVoltage.Text = "Voltage";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(7, 20);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(57, 13);
            this.lblFrequency.TabIndex = 0;
            this.lblFrequency.Text = "Frequency";
            // 
            // lblDCOffset
            // 
            this.lblDCOffset.AutoSize = true;
            this.lblDCOffset.Location = new System.Drawing.Point(8, 153);
            this.lblDCOffset.Name = "lblDCOffset";
            this.lblDCOffset.Size = new System.Drawing.Size(53, 13);
            this.lblDCOffset.TabIndex = 13;
            this.lblDCOffset.Text = "DC Offset";
            // 
            // lblIntegration
            // 
            this.lblIntegration.AutoSize = true;
            this.lblIntegration.Location = new System.Drawing.Point(8, 127);
            this.lblIntegration.Name = "lblIntegration";
            this.lblIntegration.Size = new System.Drawing.Size(83, 13);
            this.lblIntegration.TabIndex = 11;
            this.lblIntegration.Text = "Integration (sec)";
            // 
            // lblGain
            // 
            this.lblGain.AutoSize = true;
            this.lblGain.Location = new System.Drawing.Point(8, 101);
            this.lblGain.Name = "lblGain";
            this.lblGain.Size = new System.Drawing.Size(29, 13);
            this.lblGain.TabIndex = 9;
            this.lblGain.Text = "Gain";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(8, 49);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(34, 13);
            this.lblMode.TabIndex = 8;
            this.lblMode.Text = "Mode";
            // 
            // lblCommPort
            // 
            this.lblCommPort.AutoSize = true;
            this.lblCommPort.Location = new System.Drawing.Point(6, 23);
            this.lblCommPort.Name = "lblCommPort";
            this.lblCommPort.Size = new System.Drawing.Size(58, 13);
            this.lblCommPort.TabIndex = 3;
            this.lblCommPort.Text = "Comm Port";
            // 
            // cboMode
            // 
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Location = new System.Drawing.Point(94, 46);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(100, 21);
            this.cboMode.TabIndex = 7;
            this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
            // 
            // txtCommPort
            // 
            this.txtCommPort.Location = new System.Drawing.Point(94, 20);
            this.txtCommPort.Name = "txtCommPort";
            this.txtCommPort.ReadOnly = true;
            this.txtCommPort.Size = new System.Drawing.Size(100, 20);
            this.txtCommPort.TabIndex = 2;
            // 
            // lblIFGain
            // 
            this.lblIFGain.AutoSize = true;
            this.lblIFGain.Location = new System.Drawing.Point(8, 75);
            this.lblIFGain.Name = "lblIFGain";
            this.lblIFGain.Size = new System.Drawing.Size(41, 13);
            this.lblIFGain.TabIndex = 5;
            this.lblIFGain.Text = "IF Gain";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.Location = new System.Drawing.Point(107, 456);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnScanStop
            // 
            this.btnScanStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnScanStop.Location = new System.Drawing.Point(25, 456);
            this.btnScanStop.Name = "btnScanStop";
            this.btnScanStop.Size = new System.Drawing.Size(75, 23);
            this.btnScanStop.TabIndex = 0;
            this.btnScanStop.Text = "Scan";
            this.btnScanStop.UseVisualStyleBackColor = true;
            this.btnScanStop.Click += new System.EventHandler(this.btnScanStop_Click);
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Location = new System.Drawing.Point(158, 213);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(90, 23);
            this.btnSendCommand.TabIndex = 2;
            this.btnSendCommand.Text = "Send Command";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstCommandWindow);
            this.groupBox1.Controls.Add(this.chkExpectReply);
            this.groupBox1.Controls.Add(this.txtCommand);
            this.groupBox1.Controls.Add(this.btnSendCommand);
            this.groupBox1.Location = new System.Drawing.Point(12, 244);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 242);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Temp Command Window";
            // 
            // lstCommandWindow
            // 
            this.lstCommandWindow.FormattingEnabled = true;
            this.lstCommandWindow.Location = new System.Drawing.Point(7, 19);
            this.lstCommandWindow.Name = "lstCommandWindow";
            this.lstCommandWindow.Size = new System.Drawing.Size(294, 160);
            this.lstCommandWindow.TabIndex = 8;
            // 
            // chkExpectReply
            // 
            this.chkExpectReply.AutoSize = true;
            this.chkExpectReply.Location = new System.Drawing.Point(7, 186);
            this.chkExpectReply.Name = "chkExpectReply";
            this.chkExpectReply.Size = new System.Drawing.Size(86, 17);
            this.chkExpectReply.TabIndex = 7;
            this.chkExpectReply.Text = "ExpectReply";
            this.chkExpectReply.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(6, 213);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(146, 20);
            this.txtCommand.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(634, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuConnect,
            this.menuReset});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.connectionToolStripMenuItem.Text = "Connection";
            // 
            // menuConnect
            // 
            this.menuConnect.Name = "menuConnect";
            this.menuConnect.Size = new System.Drawing.Size(125, 22);
            this.menuConnect.Text = "Connect";
            this.menuConnect.Click += new System.EventHandler(this.menuConnect_Click);
            // 
            // menuReset
            // 
            this.menuReset.Name = "menuReset";
            this.menuReset.Size = new System.Drawing.Size(125, 22);
            this.menuReset.Text = "Reset";
            this.menuReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // SpectraCyber1Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 524);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpSCSettings);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpectraCyber1Form";
            this.Text = "Spectra Cyber Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpectraCyber1Form_FormClosing);
            this.Load += new System.EventHandler(this.SpectraCyber1Form_Load);
            this.grpSCSettings.ResumeLayout(false);
            this.grpSCSettings.PerformLayout();
            this.grpFrequencyBounds.ResumeLayout(false);
            this.grpFrequencyBounds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPLLUpperFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPLLLowerFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDCOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIFGain)).EndInit();
            this.grpNoiseSource.ResumeLayout(false);
            this.grpNoiseSource.PerformLayout();
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSCSettings;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.GroupBox grpNoiseSource;
        private System.Windows.Forms.RadioButton rdoNoiseSourceOff;
        private System.Windows.Forms.RadioButton rdoNoiseSourceOn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtVoltage;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblVoltage;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblCommPort;
        private System.Windows.Forms.TextBox txtCommPort;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnScanStop;
        private System.Windows.Forms.Label lblIFGain;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.Label lblDCOffset;
        private System.Windows.Forms.Label lblIntegration;
        private System.Windows.Forms.ComboBox cboIntegration;
        private System.Windows.Forms.ComboBox cboGain;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.CheckBox chkExpectReply;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuConnect;
        private System.Windows.Forms.ToolStripMenuItem menuReset;
        private System.Windows.Forms.NumericUpDown nudIFGain;
        private System.Windows.Forms.NumericUpDown nudDCOffset;
        private System.Windows.Forms.GroupBox grpFrequencyBounds;
        private System.Windows.Forms.NumericUpDown nudPLLUpperFrequency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLowerFrequency;
        private System.Windows.Forms.NumericUpDown nudPLLLowerFrequency;
        private System.Windows.Forms.ListBox lstCommandWindow;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}