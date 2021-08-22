using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SongBPMFinder
{
    /// <summary>
    /// Part of CustomWaveViewer 
    /// </summary>
    public class CustomWaveViewerViewport : System.Windows.Forms.UserControl
    {
        #region Required Designer Variables
        private System.ComponentModel.Container components = null;
        #endregion

        private readonly Pen WAVEPEN = Pens.Black;

        private AudioData audioData;

        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                if (value != audioData)
                {
                    audioData = value;
                    coordinates.AudioData = value;
                    waveformDrawer.AudioData = value;

                    ClearDrawables();
                }

                Invalidate();
            }
        }


        public bool ForceIndividualView {
            get {
                return waveformDrawer.ForceIndividualView;
            }
            set {
                waveformDrawer.ForceIndividualView = value;
                Invalidate();
            }
        }


        Font textFont;
        StringFormat format;
        Pen drawingPen;
        SolidBrush textBrush;

        WaveformCoordinates coordinates;
        public WaveformCoordinates Coordinates { get => coordinates; }

        WaveformDrawer waveformDrawer;

        List<IDrawable> drawables = new List<IDrawable>();
       

        public CustomWaveViewerViewport()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;

            textFont = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f, FontStyle.Regular);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            drawingPen = new Pen(Color.Red, 3);
            textBrush = new SolidBrush(Color.Red);

            coordinates = new WaveformCoordinates(audioData, this);

            waveformDrawer = new WaveformDrawer(this, textFont, audioData, coordinates);
        }


        public void AddDrawableItem(IDrawable d)
        {
            drawables.Add(d);
        }

        public void RemoveDrawableItem(IDrawable d)
        {
            drawables.Remove(d);
        }

        public void ClearDrawables()
        {
            drawables.Clear();
        }


        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (audioData == null)
                return;

            coordinates.RecalculateExtents();

            waveformDrawer.DrawAudioWaveform(e.Graphics);

            for(int i = 0; i < drawables.Count; i++)
            {
                drawables[i].Draw(this, coordinates, e.Graphics);
            }


            drawCurrentPositionText(e);

            drawMidLine(e);

            base.OnPaint(e);
        }


        private void drawCurrentPositionText(PaintEventArgs e)
        {
            string currentTime = audioData.CurrentSampleSeconds.ToString("0.000") + "seconds" +
                            " / " + audioData.CurrentSample + "samples";

            e.Graphics.DrawString(currentTime, textFont, Brushes.Blue, new PointF(ClientRectangle.X + ClientRectangle.Width / 2, ClientRectangle.Top));
        }


        private void drawMidLine(PaintEventArgs e)
        {
            int mid = ClientRectangle.X + ClientRectangle.Width / 2;
            e.Graphics.DrawLine(Pens.Black, mid - 1, ClientRectangle.Top, mid - 1, ClientRectangle.Bottom);
            e.Graphics.DrawLine(Pens.Yellow, mid, ClientRectangle.Top, mid, ClientRectangle.Bottom);
            e.Graphics.DrawLine(Pens.Black, mid + 1, ClientRectangle.Top, mid + 1, ClientRectangle.Bottom);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomWaveViewer
            // 
            this.Name = "CustomWaveViewer";
            this.Size = new System.Drawing.Size(1001, 417);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
