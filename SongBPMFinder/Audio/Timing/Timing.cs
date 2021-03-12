using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class Timing 
    {

        /*
        public static int FindBeat2(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, List<TimingPoint> timingPoints, float instant = 0.001f, int numLevels = 4)
        {
            FloatArrays.DownsampleAverage(slice, tempBuffer, audioData.SampleRate);

            return -1;
        }
        */

        static List<TimingPoint> TestBeatfinding(List<TimingPoint> timingPoints, AudioData audioData, double t, double windowSize, double beatSize, float resolution)
        {
            int windowLength = audioData.ToSample(windowSize);
            int beatWindowLength = audioData.ToSample(beatSize);

            //Slice<float> data = PrepareData(audioData, !destructive);
            int a = audioData.ToSample(t) ;
            int b = audioData.ToSample(t + windowSize);

            if (b >= audioData.Length)
                return new List<TimingPoint>();

            Slice<float> data = audioData.GetChannel(0).GetSlice(a, b).DeepCopy();

            //Window bounds
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(a), Color.Cyan));
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(b), Color.Cyan));


            //Final beat position
            int beatPosition = BeatFinder.FindBeat(audioData, data, data.DeepCopy(), resolution, 4, true);

            if(beatPosition == -1)
            {
                timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(a + beatPosition), Color.Lime));
            }
            else
            {
                timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(a + beatPosition), Color.Red));
            }

            return timingPoints;
        }

        public static TimingPointList GenerateMultiBPMTiming(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();
			float res = 0.0005f;

			float coalesceWindow = res*2;
            /*
			Form1.Instance.IsTesting = false;



            timingPoints = BeatFinder.FindAllBeats(audioData, 0.2, 0.01, res);
            //timingPoints = BeatFinder.FindAllBeatsCoalescing(audioData, 0.2, 0.01, res, coalesceWindow);
            timingPoints = TimingPointList.RemoveDebugPoints(timingPoints);


            //*/

            //*
			Form1.Instance.IsTesting = true;
			
            double t = audioData.CurrentSampleSeconds;
            
            //double t = 0.31471655328798187;
            //double t = 0.30471655328798187;

            TestBeatfinding(timingPoints, audioData, t, 0.2, 0.01, res);

            //*/

            /*
            double tol = res/2.0;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);
            //*/

            return new TimingPointList(timingPoints, false);
        }
    }
}
