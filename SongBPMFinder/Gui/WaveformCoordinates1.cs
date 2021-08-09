﻿using System;
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
        AudioData audioData;
        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                audioData = value;
                //invoke event or someth
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
                return Math.Min(audioData.GetChannel(0).Length, VeryRightSampleNotClamped); 
            }
        }

        public AudioSlice GetActiveAudioSlice(int channel) {
            return audioData.GetChannel(channel).GetSlice(VeryLeftSample, VeryRightSample);
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

                if (secondsPerPixel < 0.000001f)
                    secondsPerPixel = 0.000001f;
                else if (audioData != null && (secondsPerPixel > audioData.Duration / ClientRectangle.Width))
                    secondsPerPixel = audioData.Duration / ClientRectangle.Width;
            }
        }

        public void Zoom(int dir, float amount)
        {
            SecondsPerPixel *= Math.Pow(amount, -dir);
            client.Invalidate();
        }

        public void ScrollAudio(float amount)
        {
            if (audioData == null)
                return;

            audioData.CurrentSample -= (int)(amount * WindowLengthSamples / 20);
            client.Invalidate();
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

            float biggestVisibleSample = FloatSliceUtil.Max(GetActiveAudioSlice(0).Slice);
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


        public double StartTimeSeconds {
            get {
                if (audioData == null)
                    return 0.0;
                return audioData.SampleToSeconds(audioData.CurrentSample);
            }
            set {
                if (audioData == null)
                    return;
                int pos = audioData.ToSample(value);
                audioData.CurrentSample = pos;
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