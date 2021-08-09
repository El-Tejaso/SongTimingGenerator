namespace SongBPMFinder
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textOutput = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.playbackScrollbar = new System.Windows.Forms.HScrollBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.buttonSpeed1x = new System.Windows.Forms.Button();
            this.buttonSpeed075x = new System.Windows.Forms.Button();
            this.buttonSpeed050x = new System.Windows.Forms.Button();
            this.buttonSpeed025x = new System.Windows.Forms.Button();
            this.waveformTabs = new System.Windows.Forms.TabControl();
            this.songWaveformTab = new System.Windows.Forms.TabPage();
            this.audioViewer = new SongBPMFinder.CustomWaveViewer();
            this.testWaveformTab = new System.Windows.Forms.TabPage();
            this.plotWaveViewer = new SongBPMFinder.CustomWaveViewer();
            this.testWaveformTab2 = new System.Windows.Forms.TabPage();
            this.plotWaveViewer2 = new SongBPMFinder.CustomWaveViewer();
            this.testWaveformTab3 = new System.Windows.Forms.TabPage();
            this.plotWaveViewer3 = new SongBPMFinder.CustomWaveViewer();
            this.testWaveformTab4 = new System.Windows.Forms.TabPage();
            this.plotWaveViewer4 = new SongBPMFinder.CustomWaveViewer();
            this.testWaveformTab5 = new System.Windows.Forms.TabPage();
            this.plotWaveViewer5 = new SongBPMFinder.CustomWaveViewer();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.testButton = new System.Windows.Forms.Button();
            this.copyTimingButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.calcTimingButton = new System.Windows.Forms.Button();
            this.clearOutputButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.openFileDialogue = new System.Windows.Forms.OpenFileDialog();
            this.songPositionChangedInterrupt = new System.Windows.Forms.Timer(this.components);
            this.freezeView = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.waveformTabs.SuspendLayout();
            this.songWaveformTab.SuspendLayout();
            this.testWaveformTab.SuspendLayout();
            this.testWaveformTab2.SuspendLayout();
            this.testWaveformTab3.SuspendLayout();
            this.testWaveformTab4.SuspendLayout();
            this.testWaveformTab5.SuspendLayout();
            this.toolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.55795F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.44205F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolPanel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1113, 585);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textOutput, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.1019F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.8981F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(924, 579);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // textOutput
            // 
            this.textOutput.BackColor = System.Drawing.Color.Black;
            this.textOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOutput.ForeColor = System.Drawing.Color.White;
            this.textOutput.Location = new System.Drawing.Point(3, 460);
            this.textOutput.Name = "textOutput";
            this.textOutput.ReadOnly = true;
            this.textOutput.Size = new System.Drawing.Size(918, 116);
            this.textOutput.TabIndex = 1;
            this.textOutput.Text = "awdwdadw";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.playbackScrollbar, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.waveformTabs, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(918, 451);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // playbackScrollbar
            // 
            this.playbackScrollbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playbackScrollbar.Location = new System.Drawing.Point(0, 427);
            this.playbackScrollbar.Name = "playbackScrollbar";
            this.playbackScrollbar.Size = new System.Drawing.Size(918, 24);
            this.playbackScrollbar.TabIndex = 5;
            this.playbackScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.playbackScrollbar_Scroll);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.playPauseButton);
            this.flowLayoutPanel1.Controls.Add(this.buttonSpeed1x);
            this.flowLayoutPanel1.Controls.Add(this.buttonSpeed075x);
            this.flowLayoutPanel1.Controls.Add(this.buttonSpeed050x);
            this.flowLayoutPanel1.Controls.Add(this.buttonSpeed025x);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(912, 40);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // playPauseButton
            // 
            this.playPauseButton.Font = new System.Drawing.Font("Webdings", 16F);
            this.playPauseButton.Location = new System.Drawing.Point(0, 0);
            this.playPauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.Size = new System.Drawing.Size(69, 40);
            this.playPauseButton.TabIndex = 0;
            this.playPauseButton.Text = "4";
            this.playPauseButton.UseVisualStyleBackColor = true;
            this.playPauseButton.Click += new System.EventHandler(this.playPauseButton_Click);
            // 
            // buttonSpeed1x
            // 
            this.buttonSpeed1x.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSpeed1x.Location = new System.Drawing.Point(69, 0);
            this.buttonSpeed1x.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSpeed1x.Name = "buttonSpeed1x";
            this.buttonSpeed1x.Size = new System.Drawing.Size(110, 40);
            this.buttonSpeed1x.TabIndex = 1;
            this.buttonSpeed1x.Text = "1x";
            this.buttonSpeed1x.UseVisualStyleBackColor = true;
            this.buttonSpeed1x.Click += new System.EventHandler(this.buttonSpeed1x_Click);
            // 
            // buttonSpeed075x
            // 
            this.buttonSpeed075x.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSpeed075x.Location = new System.Drawing.Point(179, 0);
            this.buttonSpeed075x.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSpeed075x.Name = "buttonSpeed075x";
            this.buttonSpeed075x.Size = new System.Drawing.Size(110, 40);
            this.buttonSpeed075x.TabIndex = 2;
            this.buttonSpeed075x.Text = "0.75x";
            this.buttonSpeed075x.UseVisualStyleBackColor = true;
            this.buttonSpeed075x.Click += new System.EventHandler(this.buttonSpeed075x_Click);
            // 
            // buttonSpeed050x
            // 
            this.buttonSpeed050x.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSpeed050x.Location = new System.Drawing.Point(289, 0);
            this.buttonSpeed050x.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSpeed050x.Name = "buttonSpeed050x";
            this.buttonSpeed050x.Size = new System.Drawing.Size(110, 40);
            this.buttonSpeed050x.TabIndex = 3;
            this.buttonSpeed050x.Text = "0.5x";
            this.buttonSpeed050x.UseVisualStyleBackColor = true;
            this.buttonSpeed050x.Click += new System.EventHandler(this.buttonSpeed050x_Click);
            // 
            // buttonSpeed025x
            // 
            this.buttonSpeed025x.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSpeed025x.Location = new System.Drawing.Point(399, 0);
            this.buttonSpeed025x.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSpeed025x.Name = "buttonSpeed025x";
            this.buttonSpeed025x.Size = new System.Drawing.Size(110, 40);
            this.buttonSpeed025x.TabIndex = 4;
            this.buttonSpeed025x.Text = "0.25x";
            this.buttonSpeed025x.UseVisualStyleBackColor = true;
            this.buttonSpeed025x.Click += new System.EventHandler(this.buttonSpeed025x_Click);
            // 
            // waveformTabs
            // 
            this.waveformTabs.Controls.Add(this.songWaveformTab);
            this.waveformTabs.Controls.Add(this.testWaveformTab);
            this.waveformTabs.Controls.Add(this.testWaveformTab2);
            this.waveformTabs.Controls.Add(this.testWaveformTab3);
            this.waveformTabs.Controls.Add(this.testWaveformTab4);
            this.waveformTabs.Controls.Add(this.testWaveformTab5);
            this.waveformTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waveformTabs.Location = new System.Drawing.Point(3, 49);
            this.waveformTabs.Name = "waveformTabs";
            this.waveformTabs.SelectedIndex = 0;
            this.waveformTabs.Size = new System.Drawing.Size(912, 375);
            this.waveformTabs.TabIndex = 7;
            // 
            // songWaveformTab
            // 
            this.songWaveformTab.Controls.Add(this.audioViewer);
            this.songWaveformTab.Location = new System.Drawing.Point(4, 22);
            this.songWaveformTab.Name = "songWaveformTab";
            this.songWaveformTab.Padding = new System.Windows.Forms.Padding(3);
            this.songWaveformTab.Size = new System.Drawing.Size(904, 349);
            this.songWaveformTab.TabIndex = 0;
            this.songWaveformTab.Text = "Song Waveform";
            this.songWaveformTab.UseVisualStyleBackColor = true;
            // 
            // audioViewer
            // 
            this.audioViewer.Data = null;
            this.audioViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioViewer.ForceIndividualView = false;
            this.audioViewer.Location = new System.Drawing.Point(3, 3);
            this.audioViewer.Name = "audioViewer";
            this.audioViewer.SecondsPerPixel = 0D;
            this.audioViewer.Size = new System.Drawing.Size(898, 343);
            this.audioViewer.StartTimeSeconds = 0D;
            this.audioViewer.TabIndex = 5;
            this.audioViewer.WindowLengthSamples = 0;
            this.audioViewer.WindowLengthSeconds = 0D;
            // 
            // testWaveformTab
            // 
            this.testWaveformTab.Controls.Add(this.plotWaveViewer);
            this.testWaveformTab.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab.Name = "testWaveformTab";
            this.testWaveformTab.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab.Size = new System.Drawing.Size(904, 349);
            this.testWaveformTab.TabIndex = 1;
            this.testWaveformTab.Text = "Debug plot";
            this.testWaveformTab.UseVisualStyleBackColor = true;
            // 
            // plotWaveViewer
            // 
            this.plotWaveViewer.Data = null;
            this.plotWaveViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotWaveViewer.ForceIndividualView = true;
            this.plotWaveViewer.Location = new System.Drawing.Point(3, 3);
            this.plotWaveViewer.Name = "plotWaveViewer";
            this.plotWaveViewer.SecondsPerPixel = 0D;
            this.plotWaveViewer.Size = new System.Drawing.Size(898, 343);
            this.plotWaveViewer.StartTimeSeconds = 0D;
            this.plotWaveViewer.TabIndex = 0;
            this.plotWaveViewer.WindowLengthSamples = 0;
            this.plotWaveViewer.WindowLengthSeconds = 0D;
            // 
            // testWaveformTab2
            // 
            this.testWaveformTab2.Controls.Add(this.plotWaveViewer2);
            this.testWaveformTab2.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab2.Name = "testWaveformTab2";
            this.testWaveformTab2.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab2.Size = new System.Drawing.Size(904, 349);
            this.testWaveformTab2.TabIndex = 2;
            this.testWaveformTab2.Text = "Debug Plot 2";
            this.testWaveformTab2.UseVisualStyleBackColor = true;
            // 
            // plotWaveViewer2
            // 
            this.plotWaveViewer2.Data = null;
            this.plotWaveViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotWaveViewer2.ForceIndividualView = true;
            this.plotWaveViewer2.Location = new System.Drawing.Point(3, 3);
            this.plotWaveViewer2.Name = "plotWaveViewer2";
            this.plotWaveViewer2.SecondsPerPixel = 0D;
            this.plotWaveViewer2.Size = new System.Drawing.Size(898, 343);
            this.plotWaveViewer2.StartTimeSeconds = 0D;
            this.plotWaveViewer2.TabIndex = 1;
            this.plotWaveViewer2.WindowLengthSamples = 0;
            this.plotWaveViewer2.WindowLengthSeconds = 0D;
            // 
            // testWaveformTab3
            // 
            this.testWaveformTab3.Controls.Add(this.plotWaveViewer3);
            this.testWaveformTab3.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab3.Name = "testWaveformTab3";
            this.testWaveformTab3.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab3.Size = new System.Drawing.Size(904, 349);
            this.testWaveformTab3.TabIndex = 3;
            this.testWaveformTab3.Text = "Debug plot 3";
            this.testWaveformTab3.UseVisualStyleBackColor = true;
            // 
            // plotWaveViewer3
            // 
            this.plotWaveViewer3.Data = null;
            this.plotWaveViewer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotWaveViewer3.ForceIndividualView = true;
            this.plotWaveViewer3.Location = new System.Drawing.Point(3, 3);
            this.plotWaveViewer3.Name = "plotWaveViewer3";
            this.plotWaveViewer3.SecondsPerPixel = 0D;
            this.plotWaveViewer3.Size = new System.Drawing.Size(898, 343);
            this.plotWaveViewer3.StartTimeSeconds = 0D;
            this.plotWaveViewer3.TabIndex = 2;
            this.plotWaveViewer3.WindowLengthSamples = 0;
            this.plotWaveViewer3.WindowLengthSeconds = 0D;
            // 
            // testWaveformTab4
            // 
            this.testWaveformTab4.Controls.Add(this.plotWaveViewer4);
            this.testWaveformTab4.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab4.Name = "testWaveformTab4";
            this.testWaveformTab4.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab4.Size = new System.Drawing.Size(904, 349);
            this.testWaveformTab4.TabIndex = 4;
            this.testWaveformTab4.Text = "Debug plot 4";
            this.testWaveformTab4.UseVisualStyleBackColor = true;
            // 
            // plotWaveViewer4
            // 
            this.plotWaveViewer4.Data = null;
            this.plotWaveViewer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotWaveViewer4.ForceIndividualView = true;
            this.plotWaveViewer4.Location = new System.Drawing.Point(3, 3);
            this.plotWaveViewer4.Name = "plotWaveViewer4";
            this.plotWaveViewer4.SecondsPerPixel = 0D;
            this.plotWaveViewer4.Size = new System.Drawing.Size(898, 343);
            this.plotWaveViewer4.StartTimeSeconds = 0D;
            this.plotWaveViewer4.TabIndex = 2;
            this.plotWaveViewer4.WindowLengthSamples = 0;
            this.plotWaveViewer4.WindowLengthSeconds = 0D;
            // 
            // testWaveformTab5
            // 
            this.testWaveformTab5.Controls.Add(this.plotWaveViewer5);
            this.testWaveformTab5.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab5.Name = "testWaveformTab5";
            this.testWaveformTab5.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab5.Size = new System.Drawing.Size(904, 349);
            this.testWaveformTab5.TabIndex = 5;
            this.testWaveformTab5.Text = "Debug plot 5";
            this.testWaveformTab5.UseVisualStyleBackColor = true;
            // 
            // plotWaveViewer5
            // 
            this.plotWaveViewer5.Data = null;
            this.plotWaveViewer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotWaveViewer5.ForceIndividualView = true;
            this.plotWaveViewer5.Location = new System.Drawing.Point(3, 3);
            this.plotWaveViewer5.Name = "plotWaveViewer5";
            this.plotWaveViewer5.SecondsPerPixel = 0D;
            this.plotWaveViewer5.Size = new System.Drawing.Size(898, 343);
            this.plotWaveViewer5.StartTimeSeconds = 0D;
            this.plotWaveViewer5.TabIndex = 2;
            this.plotWaveViewer5.WindowLengthSamples = 0;
            this.plotWaveViewer5.WindowLengthSeconds = 0D;
            // 
            // toolPanel
            // 
            this.toolPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.toolPanel.Controls.Add(this.freezeView);
            this.toolPanel.Controls.Add(this.testButton);
            this.toolPanel.Controls.Add(this.copyTimingButton);
            this.toolPanel.Controls.Add(this.label4);
            this.toolPanel.Controls.Add(this.label3);
            this.toolPanel.Controls.Add(this.label2);
            this.toolPanel.Controls.Add(this.label1);
            this.toolPanel.Controls.Add(this.calcTimingButton);
            this.toolPanel.Controls.Add(this.clearOutputButton);
            this.toolPanel.Controls.Add(this.openButton);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolPanel.Location = new System.Drawing.Point(933, 3);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(177, 579);
            this.toolPanel.TabIndex = 1;
            // 
            // testButton
            // 
            this.testButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.testButton.Location = new System.Drawing.Point(6, 475);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(165, 29);
            this.testButton.TabIndex = 8;
            this.testButton.Text = "TestButton";
            this.testButton.UseVisualStyleBackColor = false;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // copyTimingButton
            // 
            this.copyTimingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copyTimingButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.copyTimingButton.Enabled = false;
            this.copyTimingButton.Location = new System.Drawing.Point(3, 133);
            this.copyTimingButton.Name = "copyTimingButton";
            this.copyTimingButton.Size = new System.Drawing.Size(165, 29);
            this.copyTimingButton.TabIndex = 7;
            this.copyTimingButton.Text = "Copy timing";
            this.copyTimingButton.UseVisualStyleBackColor = false;
            this.copyTimingButton.Click += new System.EventHandler(this.copyTimingButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File Related";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 528);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Console";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Timing Related";
            // 
            // calcTimingButton
            // 
            this.calcTimingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calcTimingButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.calcTimingButton.Enabled = false;
            this.calcTimingButton.Location = new System.Drawing.Point(3, 98);
            this.calcTimingButton.Name = "calcTimingButton";
            this.calcTimingButton.Size = new System.Drawing.Size(165, 29);
            this.calcTimingButton.TabIndex = 2;
            this.calcTimingButton.TabStop = false;
            this.calcTimingButton.Text = "Calculate timing";
            this.calcTimingButton.UseVisualStyleBackColor = false;
            this.calcTimingButton.Click += new System.EventHandler(this.calcTimingButton_Click);
            // 
            // clearOutputButton
            // 
            this.clearOutputButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clearOutputButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.clearOutputButton.Location = new System.Drawing.Point(6, 544);
            this.clearOutputButton.Name = "clearOutputButton";
            this.clearOutputButton.Size = new System.Drawing.Size(165, 26);
            this.clearOutputButton.TabIndex = 1;
            this.clearOutputButton.Text = "Clear output";
            this.clearOutputButton.UseVisualStyleBackColor = false;
            this.clearOutputButton.Click += new System.EventHandler(this.clearOutputButton_Click);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.openButton.Location = new System.Drawing.Point(3, 29);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(165, 29);
            this.openButton.TabIndex = 0;
            this.openButton.Text = "Open Audio";
            this.openButton.UseVisualStyleBackColor = false;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // openFileDialogue
            // 
            this.openFileDialogue.FileName = "openFileDialog1";
            // 
            // songPositionChangedInterrupt
            // 
            this.songPositionChangedInterrupt.Tick += new System.EventHandler(this.songPositionChangedInterrupt_Tick);
            // 
            // freezeView
            // 
            this.freezeView.AutoSize = true;
            this.freezeView.Location = new System.Drawing.Point(6, 437);
            this.freezeView.Name = "freezeView";
            this.freezeView.Size = new System.Drawing.Size(83, 17);
            this.freezeView.TabIndex = 9;
            this.freezeView.Text = "Freeze view";
            this.freezeView.UseVisualStyleBackColor = true;
            this.freezeView.CheckedChanged += new System.EventHandler(this.freezeView_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 585);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Music Timer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.waveformTabs.ResumeLayout(false);
            this.songWaveformTab.ResumeLayout(false);
            this.testWaveformTab.ResumeLayout(false);
            this.testWaveformTab2.ResumeLayout(false);
            this.testWaveformTab3.ResumeLayout(false);
            this.testWaveformTab4.ResumeLayout(false);
            this.testWaveformTab5.ResumeLayout(false);
            this.toolPanel.ResumeLayout(false);
            this.toolPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialogue;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.RichTextBox textOutput;
        private System.Windows.Forms.Button clearOutputButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Timer songPositionChangedInterrupt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button calcTimingButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button copyTimingButton;
        private System.Windows.Forms.HScrollBar playbackScrollbar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonSpeed1x;
        private System.Windows.Forms.Button buttonSpeed075x;
        private System.Windows.Forms.Button buttonSpeed050x;
        private System.Windows.Forms.Button buttonSpeed025x;
        private System.Windows.Forms.TabControl waveformTabs;
        private System.Windows.Forms.TabPage songWaveformTab;
        private CustomWaveViewer audioViewer;
        private System.Windows.Forms.TabPage testWaveformTab;
        private CustomWaveViewer plotWaveViewer;
        private System.Windows.Forms.TabPage testWaveformTab2;
        private CustomWaveViewer plotWaveViewer2;
        private System.Windows.Forms.TabPage testWaveformTab3;
        private CustomWaveViewer plotWaveViewer3;
        private System.Windows.Forms.TabPage testWaveformTab4;
        private CustomWaveViewer plotWaveViewer4;
        private System.Windows.Forms.TabPage testWaveformTab5;
        private CustomWaveViewer plotWaveViewer5;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.CheckBox freezeView;
    }
}

