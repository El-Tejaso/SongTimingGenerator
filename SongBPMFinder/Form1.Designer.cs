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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.buttonSpeed1x = new System.Windows.Forms.Button();
            this.buttonSpeed075x = new System.Windows.Forms.Button();
            this.buttonSpeed050x = new System.Windows.Forms.Button();
            this.buttonSpeed025x = new System.Windows.Forms.Button();
            this.waveformTabs = new System.Windows.Forms.TabControl();
            this.songWaveformTab = new System.Windows.Forms.TabPage();
            this.testWaveformTab = new System.Windows.Forms.TabPage();
            this.testWaveformTab2 = new System.Windows.Forms.TabPage();
            this.testWaveformTab3 = new System.Windows.Forms.TabPage();
            this.testWaveformTab4 = new System.Windows.Forms.TabPage();
            this.testWaveformTab5 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textOutput = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.peakDetectWindowSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.peakDetectInfluenceNumeric = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.peakDetectStdDevThresholdNumeric = new System.Windows.Forms.NumericUpDown();
            this.localizedTimingCheckbox = new System.Windows.Forms.CheckBox();
            this.differenceFunctionCombobox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.strideNumeric = new System.Windows.Forms.NumericUpDown();
            this.rightChannelCheckbox = new System.Windows.Forms.CheckBox();
            this.leftChannelCheckbox = new System.Windows.Forms.CheckBox();
            this.binaryPeakCheckbox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numFreqBandsNumeric = new System.Windows.Forms.NumericUpDown();
            this.addAllFreqCheckbox = new System.Windows.Forms.CheckBox();
            this.evalDistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fourierWindowCombobox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAudioForTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.osuTimingPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songPositionChangedInterrupt = new System.Windows.Forms.Timer(this.components);
            this.audioViewer = new SongBPMFinder.CustomWaveViewer();
            this.debugPlot1 = new SongBPMFinder.CustomWaveViewer();
            this.debugPlot2 = new SongBPMFinder.CustomWaveViewer();
            this.debugPlot3 = new SongBPMFinder.CustomWaveViewer();
            this.debugPlot4 = new SongBPMFinder.CustomWaveViewer();
            this.debugPlot5 = new SongBPMFinder.CustomWaveViewer();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.waveformTabs.SuspendLayout();
            this.songWaveformTab.SuspendLayout();
            this.testWaveformTab.SuspendLayout();
            this.testWaveformTab2.SuspendLayout();
            this.testWaveformTab3.SuspendLayout();
            this.testWaveformTab4.SuspendLayout();
            this.testWaveformTab5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectWindowSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectInfluenceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectStdDevThresholdNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strideNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFreqBandsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.evalDistanceNumeric)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1113, 585);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 26);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1107, 556);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.waveformTabs, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1107, 320);
            this.tableLayoutPanel3.TabIndex = 3;
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1101, 40);
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
            this.waveformTabs.Size = new System.Drawing.Size(1101, 268);
            this.waveformTabs.TabIndex = 7;
            // 
            // songWaveformTab
            // 
            this.songWaveformTab.Controls.Add(this.audioViewer);
            this.songWaveformTab.Location = new System.Drawing.Point(4, 22);
            this.songWaveformTab.Name = "songWaveformTab";
            this.songWaveformTab.Padding = new System.Windows.Forms.Padding(3);
            this.songWaveformTab.Size = new System.Drawing.Size(1093, 242);
            this.songWaveformTab.TabIndex = 0;
            this.songWaveformTab.Text = "Song Waveform";
            this.songWaveformTab.UseVisualStyleBackColor = true;
            // 
            // testWaveformTab
            // 
            this.testWaveformTab.Controls.Add(this.debugPlot1);
            this.testWaveformTab.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab.Name = "testWaveformTab";
            this.testWaveformTab.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab.Size = new System.Drawing.Size(1093, 272);
            this.testWaveformTab.TabIndex = 1;
            this.testWaveformTab.Text = "Debug plot";
            this.testWaveformTab.UseVisualStyleBackColor = true;
            // 
            // testWaveformTab2
            // 
            this.testWaveformTab2.Controls.Add(this.debugPlot2);
            this.testWaveformTab2.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab2.Name = "testWaveformTab2";
            this.testWaveformTab2.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab2.Size = new System.Drawing.Size(1093, 272);
            this.testWaveformTab2.TabIndex = 2;
            this.testWaveformTab2.Text = "Debug Plot 2";
            this.testWaveformTab2.UseVisualStyleBackColor = true;
            // 
            // testWaveformTab3
            // 
            this.testWaveformTab3.Controls.Add(this.debugPlot3);
            this.testWaveformTab3.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab3.Name = "testWaveformTab3";
            this.testWaveformTab3.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab3.Size = new System.Drawing.Size(1093, 272);
            this.testWaveformTab3.TabIndex = 3;
            this.testWaveformTab3.Text = "Debug plot 3";
            this.testWaveformTab3.UseVisualStyleBackColor = true;
            // 
            // testWaveformTab4
            // 
            this.testWaveformTab4.Controls.Add(this.debugPlot4);
            this.testWaveformTab4.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab4.Name = "testWaveformTab4";
            this.testWaveformTab4.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab4.Size = new System.Drawing.Size(1093, 272);
            this.testWaveformTab4.TabIndex = 4;
            this.testWaveformTab4.Text = "Debug plot 4";
            this.testWaveformTab4.UseVisualStyleBackColor = true;
            // 
            // testWaveformTab5
            // 
            this.testWaveformTab5.Controls.Add(this.debugPlot5);
            this.testWaveformTab5.Location = new System.Drawing.Point(4, 22);
            this.testWaveformTab5.Name = "testWaveformTab5";
            this.testWaveformTab5.Padding = new System.Windows.Forms.Padding(3);
            this.testWaveformTab5.Size = new System.Drawing.Size(1093, 272);
            this.testWaveformTab5.TabIndex = 5;
            this.testWaveformTab5.Text = "Debug plot 5";
            this.testWaveformTab5.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textOutput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(1107, 232);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 0;
            // 
            // textOutput
            // 
            this.textOutput.BackColor = System.Drawing.Color.Black;
            this.textOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOutput.ForeColor = System.Drawing.Color.White;
            this.textOutput.Location = new System.Drawing.Point(0, 0);
            this.textOutput.Name = "textOutput";
            this.textOutput.ReadOnly = true;
            this.textOutput.Size = new System.Drawing.Size(400, 232);
            this.textOutput.TabIndex = 2;
            this.textOutput.Text = "Woah, someone actually downloaded this";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(703, 232);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.peakDetectWindowSizeNumeric);
            this.tabPage1.Controls.Add(this.peakDetectInfluenceNumeric);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.peakDetectStdDevThresholdNumeric);
            this.tabPage1.Controls.Add(this.localizedTimingCheckbox);
            this.tabPage1.Controls.Add(this.differenceFunctionCombobox);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.strideNumeric);
            this.tabPage1.Controls.Add(this.rightChannelCheckbox);
            this.tabPage1.Controls.Add(this.leftChannelCheckbox);
            this.tabPage1.Controls.Add(this.binaryPeakCheckbox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.numFreqBandsNumeric);
            this.tabPage1.Controls.Add(this.addAllFreqCheckbox);
            this.tabPage1.Controls.Add(this.evalDistanceNumeric);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.fourierWindowCombobox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(695, 206);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Beat detection";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(535, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Peak detection";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(465, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Window size (sec)";
            // 
            // peakDetectWindowSizeNumeric
            // 
            this.peakDetectWindowSizeNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.peakDetectWindowSizeNumeric.DecimalPlaces = 3;
            this.peakDetectWindowSizeNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.peakDetectWindowSizeNumeric.Location = new System.Drawing.Point(564, 77);
            this.peakDetectWindowSizeNumeric.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.peakDetectWindowSizeNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.peakDetectWindowSizeNumeric.Name = "peakDetectWindowSizeNumeric";
            this.peakDetectWindowSizeNumeric.Size = new System.Drawing.Size(120, 20);
            this.peakDetectWindowSizeNumeric.TabIndex = 24;
            this.peakDetectWindowSizeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.peakDetectWindowSizeNumeric.ValueChanged += new System.EventHandler(this.peakDetectWindowSizeNumeric_ValueChanged);
            // 
            // peakDetectInfluenceNumeric
            // 
            this.peakDetectInfluenceNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.peakDetectInfluenceNumeric.DecimalPlaces = 4;
            this.peakDetectInfluenceNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.peakDetectInfluenceNumeric.Location = new System.Drawing.Point(564, 51);
            this.peakDetectInfluenceNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.peakDetectInfluenceNumeric.Name = "peakDetectInfluenceNumeric";
            this.peakDetectInfluenceNumeric.Size = new System.Drawing.Size(120, 20);
            this.peakDetectInfluenceNumeric.TabIndex = 23;
            this.peakDetectInfluenceNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.peakDetectInfluenceNumeric.ValueChanged += new System.EventHandler(this.peakDetectInfluenceNumeric_ValueChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(496, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Influence";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(469, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "StdDev threshold";
            // 
            // peakDetectStdDevThresholdNumeric
            // 
            this.peakDetectStdDevThresholdNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.peakDetectStdDevThresholdNumeric.DecimalPlaces = 2;
            this.peakDetectStdDevThresholdNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.peakDetectStdDevThresholdNumeric.Location = new System.Drawing.Point(564, 25);
            this.peakDetectStdDevThresholdNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.peakDetectStdDevThresholdNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.peakDetectStdDevThresholdNumeric.Name = "peakDetectStdDevThresholdNumeric";
            this.peakDetectStdDevThresholdNumeric.Size = new System.Drawing.Size(120, 20);
            this.peakDetectStdDevThresholdNumeric.TabIndex = 18;
            this.peakDetectStdDevThresholdNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.peakDetectStdDevThresholdNumeric.ValueChanged += new System.EventHandler(this.peakDetectStdDevThresholdNumeric_ValueChanged);
            // 
            // localizedTimingCheckbox
            // 
            this.localizedTimingCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.localizedTimingCheckbox.AutoSize = true;
            this.localizedTimingCheckbox.Location = new System.Drawing.Point(495, 160);
            this.localizedTimingCheckbox.Name = "localizedTimingCheckbox";
            this.localizedTimingCheckbox.Size = new System.Drawing.Size(185, 17);
            this.localizedTimingCheckbox.TabIndex = 17;
            this.localizedTimingCheckbox.Text = "Only time the part we\'re looking at";
            this.localizedTimingCheckbox.UseVisualStyleBackColor = true;
            this.localizedTimingCheckbox.CheckedChanged += new System.EventHandler(this.localizedTimingCheckbox_CheckedChanged);
            // 
            // differenceFunctionCombobox
            // 
            this.differenceFunctionCombobox.DisplayMember = "(none)";
            this.differenceFunctionCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.differenceFunctionCombobox.FormattingEnabled = true;
            this.differenceFunctionCombobox.Items.AddRange(new object[] {
            "Sum of Squares Difference,",
            "Max Frequency-Gain Difference"});
            this.differenceFunctionCombobox.Location = new System.Drawing.Point(183, 103);
            this.differenceFunctionCombobox.Name = "differenceFunctionCombobox";
            this.differenceFunctionCombobox.Size = new System.Drawing.Size(121, 21);
            this.differenceFunctionCombobox.TabIndex = 16;
            this.differenceFunctionCombobox.SelectedIndexChanged += new System.EventHandler(this.differenceFunctionCombobox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Difference function";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Stride (seconds)";
            // 
            // strideNumeric
            // 
            this.strideNumeric.Cursor = System.Windows.Forms.Cursors.Default;
            this.strideNumeric.DecimalPlaces = 4;
            this.strideNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.strideNumeric.Location = new System.Drawing.Point(9, 130);
            this.strideNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.strideNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.strideNumeric.Name = "strideNumeric";
            this.strideNumeric.Size = new System.Drawing.Size(144, 20);
            this.strideNumeric.TabIndex = 13;
            this.strideNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.strideNumeric.ValueChanged += new System.EventHandler(this.strideNumeric_ValueChanged);
            // 
            // rightChannelCheckbox
            // 
            this.rightChannelCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rightChannelCheckbox.AutoSize = true;
            this.rightChannelCheckbox.Checked = true;
            this.rightChannelCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rightChannelCheckbox.Location = new System.Drawing.Point(495, 183);
            this.rightChannelCheckbox.Name = "rightChannelCheckbox";
            this.rightChannelCheckbox.Size = new System.Drawing.Size(93, 17);
            this.rightChannelCheckbox.TabIndex = 12;
            this.rightChannelCheckbox.Text = "Right Channel";
            this.rightChannelCheckbox.UseVisualStyleBackColor = true;
            this.rightChannelCheckbox.CheckedChanged += new System.EventHandler(this.rightChannelCheckbox_CheckedChanged);
            // 
            // leftChannelCheckbox
            // 
            this.leftChannelCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.leftChannelCheckbox.AutoSize = true;
            this.leftChannelCheckbox.Checked = true;
            this.leftChannelCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.leftChannelCheckbox.Location = new System.Drawing.Point(603, 183);
            this.leftChannelCheckbox.Name = "leftChannelCheckbox";
            this.leftChannelCheckbox.Size = new System.Drawing.Size(86, 17);
            this.leftChannelCheckbox.TabIndex = 11;
            this.leftChannelCheckbox.Text = "Left Channel";
            this.leftChannelCheckbox.UseVisualStyleBackColor = true;
            this.leftChannelCheckbox.CheckedChanged += new System.EventHandler(this.leftChannelLeckbox_CheckedChanged);
            // 
            // binaryPeakCheckbox
            // 
            this.binaryPeakCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.binaryPeakCheckbox.AutoSize = true;
            this.binaryPeakCheckbox.Location = new System.Drawing.Point(538, 111);
            this.binaryPeakCheckbox.Name = "binaryPeakCheckbox";
            this.binaryPeakCheckbox.Size = new System.Drawing.Size(88, 17);
            this.binaryPeakCheckbox.TabIndex = 10;
            this.binaryPeakCheckbox.Text = "Binary Peaks";
            this.binaryPeakCheckbox.UseVisualStyleBackColor = true;
            this.binaryPeakCheckbox.CheckedChanged += new System.EventHandler(this.binaryPeakCheckbox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Num frequency bands";
            // 
            // numFreqBandsNumeric
            // 
            this.numFreqBandsNumeric.Location = new System.Drawing.Point(184, 20);
            this.numFreqBandsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFreqBandsNumeric.Name = "numFreqBandsNumeric";
            this.numFreqBandsNumeric.Size = new System.Drawing.Size(120, 20);
            this.numFreqBandsNumeric.TabIndex = 8;
            this.numFreqBandsNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFreqBandsNumeric.ValueChanged += new System.EventHandler(this.numFrequNumeric_OnValueChanged);
            // 
            // addAllFreqCheckbox
            // 
            this.addAllFreqCheckbox.AutoSize = true;
            this.addAllFreqCheckbox.Location = new System.Drawing.Point(184, 51);
            this.addAllFreqCheckbox.Name = "addAllFreqCheckbox";
            this.addAllFreqCheckbox.Size = new System.Drawing.Size(140, 17);
            this.addAllFreqCheckbox.TabIndex = 7;
            this.addAllFreqCheckbox.Text = "Add all frequency bands";
            this.addAllFreqCheckbox.UseVisualStyleBackColor = true;
            this.addAllFreqCheckbox.CheckedChanged += new System.EventHandler(this.AddAllFrequ_OnCheckChanded);
            // 
            // evalDistanceNumeric
            // 
            this.evalDistanceNumeric.Cursor = System.Windows.Forms.Cursors.Default;
            this.evalDistanceNumeric.DecimalPlaces = 3;
            this.evalDistanceNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.evalDistanceNumeric.Location = new System.Drawing.Point(6, 68);
            this.evalDistanceNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.evalDistanceNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.evalDistanceNumeric.Name = "evalDistanceNumeric";
            this.evalDistanceNumeric.Size = new System.Drawing.Size(144, 20);
            this.evalDistanceNumeric.TabIndex = 6;
            this.evalDistanceNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.evalDistanceNumeric.ValueChanged += new System.EventHandler(this.evalDistanceNumeric_OnValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Fourier evaluation gap (seconds)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Frequency window";
            // 
            // fourierWindowCombobox
            // 
            this.fourierWindowCombobox.DisplayMember = "(none)";
            this.fourierWindowCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fourierWindowCombobox.FormattingEnabled = true;
            this.fourierWindowCombobox.Items.AddRange(new object[] {
            "256",
            "512",
            "1024",
            "2048",
            "4096"});
            this.fourierWindowCombobox.Location = new System.Drawing.Point(6, 19);
            this.fourierWindowCombobox.Name = "fourierWindowCombobox";
            this.fourierWindowCombobox.Size = new System.Drawing.Size(121, 21);
            this.fourierWindowCombobox.TabIndex = 3;
            this.fourierWindowCombobox.SelectedIndexChanged += new System.EventHandler(this.FourierWindowCombobox_OnSelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(593, 176);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Timing generation";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.timingToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem,
            this.outputToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1113, 23);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAudioForTimingToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 19);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openAudioForTimingToolStripMenuItem
            // 
            this.openAudioForTimingToolStripMenuItem.Name = "openAudioForTimingToolStripMenuItem";
            this.openAudioForTimingToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openAudioForTimingToolStripMenuItem.Text = "Open audio for timing";
            this.openAudioForTimingToolStripMenuItem.Click += new System.EventHandler(this.openAudioForTimingToolStripMenuItem_Click);
            // 
            // timingToolStripMenuItem
            // 
            this.timingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calculateTimingToolStripMenuItem});
            this.timingToolStripMenuItem.Name = "timingToolStripMenuItem";
            this.timingToolStripMenuItem.Size = new System.Drawing.Size(56, 19);
            this.timingToolStripMenuItem.Text = "Timing";
            // 
            // calculateTimingToolStripMenuItem
            // 
            this.calculateTimingToolStripMenuItem.Name = "calculateTimingToolStripMenuItem";
            this.calculateTimingToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.calculateTimingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.calculateTimingToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.calculateTimingToolStripMenuItem.Text = "Calculate timing";
            this.calculateTimingToolStripMenuItem.Click += new System.EventHandler(this.calculateTimingToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLFileToolStripMenuItem,
            this.osuTimingPointsToolStripMenuItem});
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(114, 19);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to clipboard";
            // 
            // xMLFileToolStripMenuItem
            // 
            this.xMLFileToolStripMenuItem.Name = "xMLFileToolStripMenuItem";
            this.xMLFileToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.xMLFileToolStripMenuItem.Text = "XML file";
            this.xMLFileToolStripMenuItem.Click += new System.EventHandler(this.xMLFileToolStripMenuItem_Click);
            // 
            // osuTimingPointsToolStripMenuItem
            // 
            this.osuTimingPointsToolStripMenuItem.Name = "osuTimingPointsToolStripMenuItem";
            this.osuTimingPointsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.osuTimingPointsToolStripMenuItem.Text = "osu! timing points";
            this.osuTimingPointsToolStripMenuItem.Click += new System.EventHandler(this.osuTimingPointsToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem1,
            this.clearToolStripMenuItem});
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(57, 19);
            this.outputToolStripMenuItem.Text = "Output";
            // 
            // copyToClipboardToolStripMenuItem1
            // 
            this.copyToClipboardToolStripMenuItem1.Name = "copyToClipboardToolStripMenuItem1";
            this.copyToClipboardToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.copyToClipboardToolStripMenuItem1.Text = "Copy to clipboard";
            this.copyToClipboardToolStripMenuItem1.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem1_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // audioViewer
            // 
            this.audioViewer.AudioData = null;
            this.audioViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioViewer.Location = new System.Drawing.Point(3, 3);
            this.audioViewer.Name = "audioViewer";
            this.audioViewer.Size = new System.Drawing.Size(1087, 236);
            this.audioViewer.TabIndex = 0;
            // 
            // debugPlot1
            // 
            this.debugPlot1.AudioData = null;
            this.debugPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPlot1.Location = new System.Drawing.Point(3, 3);
            this.debugPlot1.Name = "debugPlot1";
            this.debugPlot1.Size = new System.Drawing.Size(1087, 266);
            this.debugPlot1.TabIndex = 0;
            // 
            // debugPlot2
            // 
            this.debugPlot2.AudioData = null;
            this.debugPlot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPlot2.Location = new System.Drawing.Point(3, 3);
            this.debugPlot2.Name = "debugPlot2";
            this.debugPlot2.Size = new System.Drawing.Size(1087, 266);
            this.debugPlot2.TabIndex = 0;
            // 
            // debugPlot3
            // 
            this.debugPlot3.AudioData = null;
            this.debugPlot3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPlot3.Location = new System.Drawing.Point(3, 3);
            this.debugPlot3.Name = "debugPlot3";
            this.debugPlot3.Size = new System.Drawing.Size(1087, 266);
            this.debugPlot3.TabIndex = 0;
            // 
            // debugPlot4
            // 
            this.debugPlot4.AudioData = null;
            this.debugPlot4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPlot4.Location = new System.Drawing.Point(3, 3);
            this.debugPlot4.Name = "debugPlot4";
            this.debugPlot4.Size = new System.Drawing.Size(1087, 266);
            this.debugPlot4.TabIndex = 0;
            // 
            // debugPlot5
            // 
            this.debugPlot5.AudioData = null;
            this.debugPlot5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPlot5.Location = new System.Drawing.Point(3, 3);
            this.debugPlot5.Name = "debugPlot5";
            this.debugPlot5.Size = new System.Drawing.Size(1087, 266);
            this.debugPlot5.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 585);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Music Timer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.waveformTabs.ResumeLayout(false);
            this.songWaveformTab.ResumeLayout(false);
            this.testWaveformTab.ResumeLayout(false);
            this.testWaveformTab2.ResumeLayout(false);
            this.testWaveformTab3.ResumeLayout(false);
            this.testWaveformTab4.ResumeLayout(false);
            this.testWaveformTab5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectWindowSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectInfluenceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peakDetectStdDevThresholdNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strideNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFreqBandsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.evalDistanceNumeric)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer songPositionChangedInterrupt;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAudioForTimingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateTimingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem osuTimingPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button playPauseButton;
        private System.Windows.Forms.Button buttonSpeed1x;
        private System.Windows.Forms.Button buttonSpeed075x;
        private System.Windows.Forms.Button buttonSpeed050x;
        private System.Windows.Forms.Button buttonSpeed025x;
        private System.Windows.Forms.TabControl waveformTabs;
        private System.Windows.Forms.TabPage songWaveformTab;
        private CustomWaveViewer audioViewer;
        private System.Windows.Forms.TabPage testWaveformTab;
        private CustomWaveViewer debugPlot1;
        private System.Windows.Forms.TabPage testWaveformTab2;
        private CustomWaveViewer debugPlot2;
        private System.Windows.Forms.TabPage testWaveformTab3;
        private CustomWaveViewer debugPlot3;
        private System.Windows.Forms.TabPage testWaveformTab4;
        private CustomWaveViewer debugPlot4;
        private System.Windows.Forms.TabPage testWaveformTab5;
        private CustomWaveViewer debugPlot5;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox textOutput;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox fourierWindowCombobox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown evalDistanceNumeric;
        private System.Windows.Forms.CheckBox addAllFreqCheckbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numFreqBandsNumeric;
        private System.Windows.Forms.CheckBox binaryPeakCheckbox;
        private System.Windows.Forms.CheckBox leftChannelCheckbox;
        private System.Windows.Forms.CheckBox rightChannelCheckbox;
        private System.Windows.Forms.NumericUpDown strideNumeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox differenceFunctionCombobox;
        private System.Windows.Forms.CheckBox localizedTimingCheckbox;
        private System.Windows.Forms.NumericUpDown peakDetectStdDevThresholdNumeric;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown peakDetectWindowSizeNumeric;
        private System.Windows.Forms.NumericUpDown peakDetectInfluenceNumeric;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
    }
}

