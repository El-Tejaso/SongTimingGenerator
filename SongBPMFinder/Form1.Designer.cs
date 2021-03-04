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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.playPauseButton = new System.Windows.Forms.Button();
            this.playbackScrollbar = new System.Windows.Forms.HScrollBar();
            this.toolPanel = new System.Windows.Forms.Panel();
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
            this.audioViewer = new SongBPMFinder.Gui.CustomWaveViewer();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
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
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.audioViewer, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(918, 451);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.1639F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.83611F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.playPauseButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.playbackScrollbar, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 386);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(912, 62);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // playPauseButton
            // 
            this.playPauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playPauseButton.Font = new System.Drawing.Font("Webdings", 16F);
            this.playPauseButton.Location = new System.Drawing.Point(7, 7);
            this.playPauseButton.Margin = new System.Windows.Forms.Padding(7);
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.Size = new System.Drawing.Size(87, 48);
            this.playPauseButton.TabIndex = 0;
            this.playPauseButton.Text = "4";
            this.playPauseButton.UseVisualStyleBackColor = true;
            this.playPauseButton.Click += new System.EventHandler(this.playPauseButton_Click);
            // 
            // playbackScrollbar
            // 
            this.playbackScrollbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playbackScrollbar.Location = new System.Drawing.Point(101, 0);
            this.playbackScrollbar.Name = "playbackScrollbar";
            this.playbackScrollbar.Size = new System.Drawing.Size(811, 62);
            this.playbackScrollbar.TabIndex = 1;
            this.playbackScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.playbackScrollbar_Scroll);
            // 
            // toolPanel
            // 
            this.toolPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
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
            this.copyTimingButton.Text = "CopyTiming";
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
            this.label2.Location = new System.Drawing.Point(3, 464);
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
            this.calcTimingButton.Text = "Calculate Timing";
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
            // audioViewer
            // 
            this.audioViewer.Data = null;
            this.audioViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioViewer.Location = new System.Drawing.Point(3, 3);
            this.audioViewer.Name = "audioViewer";
            this.audioViewer.SecondsPerPixel = 0D;
            this.audioViewer.Size = new System.Drawing.Size(912, 377);
            this.audioViewer.StartTime = 0D;
            this.audioViewer.TabIndex = 2;
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
            this.tableLayoutPanel4.ResumeLayout(false);
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
        private Gui.CustomWaveViewer audioViewer;
        private System.Windows.Forms.Timer songPositionChangedInterrupt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button calcTimingButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.HScrollBar playbackScrollbar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button copyTimingButton;
    }
}

