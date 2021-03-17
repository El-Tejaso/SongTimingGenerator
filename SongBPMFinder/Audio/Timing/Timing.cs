using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;
using SongBPMFinder.Audio.BeatDetection;

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

        static List<TimingPoint> testBeatfindingInternal(List<TimingPoint> timingPoints, AudioData audioData, double t, double windowSize, float resolution, int numLevels)
        {
            int a = audioData.ToSample(t);

            int windowLength = audioData.ToSample(windowSize);
            windowLength = QuickMafs.NearestDivisor(windowLength, QuickMafs.Pow(2, numLevels)); 

            if (a+windowLength >= audioData.Length)
                return new List<TimingPoint>();

            Slice<float> data = audioData.GetChannel(0).GetSlice(a, a+windowLength).DeepCopy();

            //Window bounds
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(a), Color.Cyan));
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds(a+windowLength), Color.Cyan));


            //Final beat position
            int beatPosition = BeatDetector.DetectBeat(audioData, data, data.DeepCopy(), resolution, numLevels, true);

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

        public static TimingPointList TestBeatFinding(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();
            float res = 0.0005f;
            float coalesceWindow = res * 2;

            Form1.Instance.IsTesting = true;

            double t = audioData.CurrentSampleSeconds;

            //double t = 0.31471655328798187;
            //double t = 0.30471655328798187;

            double windowLength = 0.2;
            testBeatfindingInternal(timingPoints, audioData, t, windowLength, res, 4);

            return new TimingPointList(timingPoints, false);
        }

        public static TimingPointList GenerateMultiBPMTiming(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();
			float res = 0.0005f;
			float beatSize = 0.01f;

            timingPoints = MultiBeatDetector.DetectAllBeats(audioData, 0.2, beatSize, res);

            timingPoints = TimingPointList.RemoveDebugPoints(timingPoints);
            double tol = res/2.0;

            timingPoints.Sort();
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);

            return new TimingPointList(timingPoints, false);
        }
    }
}
