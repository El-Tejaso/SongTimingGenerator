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

        static Slice<float> PrepareData(AudioData audioData, bool copy = true)
        {
            float[] dataArray;

            //Delete this line in production
            dataArray = audioData.Data;

            Slice<float> dataOrig = new Slice<float>(dataArray);

            Slice<float> data;

            if (copy)
            {
                data = new Slice<float>(new float[dataOrig.Length / audioData.Channels]);
            } 
            else
            {
                data = dataOrig.GetSlice(0, dataOrig.Length / audioData.Channels);
            }

            //use just one channel
            FloatArrays.ExtractChannel(dataOrig, data, audioData.Channels, 0);
            return data;
        }

        static List<TimingPoint> FindAllBeats(AudioData audioData, double windowSize, double beatSize, float resolution = 0.001f)
        {
            Slice<float> data = PrepareData(audioData, true);

            List<TimingPoint> timingPoints = new List<TimingPoint>();

            int doubleWindowLength = audioData.ToSample(windowSize) * 2;
            int beatWindowLength = audioData.ToSample(beatSize);

            float[] windowBuffer = new float[doubleWindowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[doubleWindowLength]);

            //*
            int pos = 0;
            while (pos < data.Length)
            {
                double currentTime = audioData.SampleToSeconds(pos);
                
                Slice<float> windowSlice = data.GetSlice(pos, Math.Min(pos + doubleWindowLength, data.Length)).DeepCopy(windowBuffer);
                int beatPosition = BeatFinder.FindBeat(audioData, windowSlice, tempBuffer, timingPoints, resolution, 4);
                double beatTime = audioData.SampleToSeconds(pos + beatPosition);

                if (beatTime > 1.2)
                {
                    //Breakpoint
                }

                if (beatPosition == -1)
                {
                    //There was no 'beat' in this position
                    pos += doubleWindowLength / 4;
                    continue;
                }

                timingPoints.Add(new TimingPoint(120, beatTime));
                pos += beatPosition + beatWindowLength;
            }
            //*/

            return timingPoints;
        }

        static List<TimingPoint> TestBeatfinding(bool destructive, List<TimingPoint> timingPoints, AudioData audioData, double t, double windowSize, double beatSize, float resolution = 0.001f)
        {
            Slice<float> data = PrepareData(audioData, !destructive);

            int doubleWindowLength = audioData.ToSample(windowSize) * 2;
            int beatWindowLength = audioData.ToSample(beatSize);

            int pos = audioData.ToSample(t);
            Slice<float> s = data.GetSlice(pos, pos + doubleWindowLength);


            //Window bounds
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds((s.Start/2)), Color.Cyan));
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds((s.Start + s.Length/2)/2), Color.Cyan));
            timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds((s.Start + s.Length)/2), Color.Cyan));


            //Final beat position
            int beatPosition = BeatFinder.FindBeat(audioData, s, s.DeepCopy(), timingPoints, resolution, 4);

            if(beatPosition == -1)
            {
                timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds((pos + beatPosition) / 2), Color.Lime));
            }
            else
            {
                timingPoints.Add(new TimingPoint(120, audioData.SampleToSeconds((pos + beatPosition) / 2), Color.Red));
            }

            return timingPoints;
        }

        public static TimingPointList GenerateMultiBPMTiming(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();

            /*
            timingPoints = FindAllBeats(audioData, 0.2, 0.01, 0.0005f);
            timingPoints = TimingPointList.RemoveDebugPoints(timingPoints);
            //*/

            //*
            //double t = 1.4550340136054423;
            double t = 1.2863945578231293;
            //double t = 3.6992290249433109;
            //double t = 3.1992290249433109;

            bool destructive = false;
            TestBeatfinding(destructive, timingPoints, audioData, t, 0.2, 0.01, 0.0005f);

            if (!destructive)
            {
                for (int i = 0; i < timingPoints.Count; i++)
                {
                    timingPoints[i].OffsetSeconds *= 2;
                }
            }

            //*/

            /*
            double tol = 0.001;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);
            //*/

            return new TimingPointList(timingPoints, false);
        }
    }
}
