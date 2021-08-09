using System;
using System.Drawing;
using System.Windows.Forms;

namespace SongBPMFinder
{
    /// <summary>
    /// Control for viewing waveforms. Initially Naudio.Gui.WaveViewer from the NAudio Library, it has been adapted to suit
	/// my own neeeds
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        #region Required Designer Variables
        private System.ComponentModel.Container components = null;
        #endregion

        private readonly Pen WAVEPEN = Pens.Black;

        private AudioData audioData;
        public AudioData Data {
            get {
                return audioData;
            }
            set {
                if (value == audioData)
                    return;

                audioData = value;
                coordinates.AudioData = value;
                waveformDrawer.AudioData = value;
                timingPointDrawer.TimingPoints = null;

                Invalidate();
            }
        }

        private TimingPointList timingPoints;


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
        TimingPointDrawer timingPointDrawer;

        public CustomWaveViewer()
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

            //might need injection
            coordinates = new WaveformCoordinates(audioData, this);

            waveformDrawer = new WaveformDrawer(this, textFont, audioData, coordinates);
            timingPointDrawer = new TimingPointDrawer(this, textFont, coordinates, timingPoints);
        }

        public void ShowTimingPoints(TimingPointList list)
        {
            timingPoints = list;
            timingPointDrawer.TimingPoints = list;
            Invalidate();
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

            timingPointDrawer.DrawTimingPoints(e.Graphics);

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
            // WaveViewer
            // 
            this.Name = "WaveViewer";
            this.ResumeLayout(false);

        }
        #endregion
    }
}
