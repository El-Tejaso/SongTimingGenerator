using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public class WaveformCoordinates
    {
        const float MINIMUM_ZOOM = 0.000001f;
        const float MAXIMUM_ZOOM = 20;

        AudioData audioData;
        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                audioData = value;

                if (SecondsPerPixel < 0)
                    secondsPerPixel = MAXIMUM_ZOOM;
            }
        }


        Control client;
        Rectangle ClientRectangle { get => client.ClientRectangle; }


        //these are recalculated with RecalculateExtents()
        float viewportMax = 1;

        public float ViewportMax {
            get {
                return viewportMax;
            }
        }

        public int VeryLeftSampleNotClamped {
            get {
                return audioData.CurrentSample - WindowLengthSamples / 2;
            }
        }

        public int VeryLeftSample {
            get {
                return Math.Max(0, VeryLeftSampleNotClamped);
            }
        }

        public int VeryRightSampleNotClamped {
            get {
                return audioData.CurrentSample + WindowLengthSamples / 2;
            }
        }

        public int VeryRightSample { 
            get { 
                return Math.Min(audioData.Length, VeryRightSampleNotClamped); 
            }
        }

        public Span<float> GetActiveAudioSlice(int channel) {
            return audioData[channel].GetSlice(VeryLeftSample, VeryRightSample);
        }


        double secondsPerPixel = 0;
        /// <summary>
        /// Can only be changed using Zoom()
        /// </summary>
        public double SecondsPerPixel {
            get {
                return secondsPerPixel;
            }
            set {
                secondsPerPixel = value;

                if (secondsPerPixel < MINIMUM_ZOOM)
                {
                    secondsPerPixel = MINIMUM_ZOOM;
                }


                if(audioData != null)
                {
                    //DotNet CORE is the one with MathF
                    float maxZoom = (float)Math.Min(MAXIMUM_ZOOM, audioData.Duration) / ClientRectangle.Width;

                    if(secondsPerPixel > maxZoom){
                        secondsPerPixel = maxZoom;
                    }
                }
            }
        }

        public void Zoom(int dir, float amount)
        {
            SecondsPerPixel *= Math.Pow(amount, -dir);
        }


        public WaveformCoordinates(AudioData audioData, Control client)
        {
            this.audioData = audioData;
            this.client = client;
        }

        public void RecalculateExtents()
        {
            if (audioData == null)
                return;

            float biggestVisibleSample = MathUtilSpanF.Max(GetActiveAudioSlice(0));
            viewportMax = Math.Max(0.00001f, biggestVisibleSample);
        }

        public double WindowLengthSeconds {
            get {
                return this.SecondsPerPixel * this.ClientRectangle.Width;
            }
            set {
                this.SecondsPerPixel = value / ClientRectangle.Width;
            }
        }


        //Time is read as Left --to-> Right
        public double WindowLeftSeconds {
            get {
                return audioData.CurrentSampleSeconds - WindowLengthSeconds / 2.0;
            }
        }

        //Time is read as Left --to-> Right
        public double WindowRightSeconds {
            get {
                return audioData.CurrentSampleSeconds + WindowLengthSeconds / 2.0;
            }
        }

        public int WindowLengthSamples {
            get {
                if (audioData == null)
                    return 0;

                return (int)(audioData.ToSample(WindowLengthSeconds));
            }
            set {
                if (audioData == null)
                    return;

                WindowLengthSeconds = audioData.SampleToSeconds(value);
            }
        }


        public float GetWaveformY(float sample, float rectTop, float rectBottom)
        {
            float percent = -0.5f * MathUtilF.Clamp(sample / Math.Abs(viewportMax), -1, 1) + 0.5f;

            float y = rectTop + (rectBottom - rectTop) * percent;

            return y;
        }

        public float GetWaveformXSeconds(double time)
        {
            return GetWaveformXSamples(audioData.ToSample(time));
        }

        public int WaveformToDataX(float waveformX)
        {
            float normalizedX = (waveformX - ClientRectangle.Left) / (float)ClientRectangle.Width;
            int samplesFromLeftOfScreen = (int)(normalizedX * WindowLengthSamples);

            return VeryLeftSampleNotClamped + samplesFromLeftOfScreen;
        }

        public float GetWaveformXSamples(int samples)
        {
            int samplesFromLeftOfScreen = (samples - VeryLeftSampleNotClamped);

            float pixelsFromLeftOfScreen = ((float)samplesFromLeftOfScreen / (float)WindowLengthSamples) * ClientRectangle.Width;
            return ClientRectangle.Left + pixelsFromLeftOfScreen;
        }
    }
}
