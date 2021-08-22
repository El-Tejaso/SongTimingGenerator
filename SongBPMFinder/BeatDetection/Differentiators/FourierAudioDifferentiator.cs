using Exocortex.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    /// <summary>
    /// Make sure that you use this to iterate through the fourier transforms in a time series only once, and then
    /// if you need to differentiate it in more than one way, do it while you are iterating, i.e
    /// 
    /// <code>
    /// foreach(FTDelta ft in differentiator){
    ///     timeSeries0.Add(frequencyBand0To100.Differentiate(ft));
    ///     timeSeries1.Add(frequencyBand100To200.Differentiate(ft));
    ///     
    ///     //etc.
    /// }
    /// </code>
    /// </summary>


    public struct FTDelta
    {
        public readonly float[] LastFT;
        public readonly float[] ThisFT;
        public readonly double Time;

        public FTDelta(float[] lastFT, float[] thisFT, double time)
        {
            LastFT = lastFT;
            ThisFT = thisFT;
            Time = time;
        }
    }

    public class FourierAudioDifferentiator
    {
        public int SampleWindow { get => sampleWindow; set => sampleWindow = value; }
        public int Stride { get => stride; set => stride = value; }

        private int sampleWindow = 1024;
        private int stride = 512;
        private int evalDistance = 512;

        float[] lastFT;
        float[] thisFT;

        //TODO: Have an array of these when this becomes multithreaded
        public FourierTransform fourierTransformer = new FourierTransform();

        public FourierAudioDifferentiator(int sampleWindow, int evalDistance, int stride)
        {
            this.SampleWindow = sampleWindow;
            this.Stride = stride;
            this.evalDistance = evalDistance;

            lastFT = new float[sampleWindow];
            thisFT = new float[sampleWindow];
        }


        public IEnumerable<FTDelta> AllFourierTransforms(AudioChannel audioChannel)
        {
            for (int i = evalDistance; i + SampleWindow < audioChannel.Length; i += Stride)
            {
                doFourierTransform(audioChannel, i-evalDistance, lastFT);
                doFourierTransform(audioChannel, i, thisFT);

                yield return new FTDelta(lastFT, thisFT, audioChannel.SamplesToSeconds(i));
            }
        }

        private void doFourierTransform(AudioChannel c, int i, float[] resultBuffer)
        {
            Span<float> slice = c.GetSlice(i, i + SampleWindow);
            fourierTransformer.FFTForwardsMagnitudes(slice, resultBuffer);
        }
    }
}
