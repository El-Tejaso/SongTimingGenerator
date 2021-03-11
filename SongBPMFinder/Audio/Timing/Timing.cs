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
				//A fresh slice
                data = new Slice<float>(new float[dataOrig.Length / audioData.Channels]);
            } 
            else
            {
				//Points to half of the original data array
                data = dataOrig.GetSlice(0, dataOrig.Length / audioData.Channels);
            }

            //extract 1 channel from original data to a buffer half the length
            FloatArrays.ExtractChannel(dataOrig, data, audioData.Channels, 0);

            //FloatArrays.DownsampleMax(dataOrig, data, audioData.Channels);
            //FloatArrays.DownsampleAverage(dataOrig, data, audioData.Channels);
            return data;
        }

        static List<TimingPoint> FindAllBeats(AudioData audioData, double windowSize, double beatSize, float resolution)
        {
            Slice<float> data = PrepareData(audioData, true);

            List<TimingPoint> timingPoints = new List<TimingPoint>();

            int windowLength = audioData.ToSample(windowSize);
            int beatWindowLength = audioData.ToSample(beatSize);

            float[] windowBuffer = new float[windowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[windowLength]);

            //*
            int pos = 0;
            while (pos < data.Length)
            {
                double currentTime = audioData.SampleToSeconds(pos);
                
                Slice<float> windowSlice = data.GetSlice(pos, Math.Min(pos + windowLength, data.Length)).DeepCopy(windowBuffer);
                int beatPosition = BeatFinder.FindBeat(audioData, windowSlice, tempBuffer, resolution, 4, false);
                double beatTime = audioData.SampleToSeconds(pos + beatPosition);

                if (beatTime > 1.2)
                {
                    //Breakpoint
                }

                if (beatPosition == -1)
                {
                    //There was no 'beat' in this position
                    pos += windowLength / 4;
                    continue;
                }

                timingPoints.Add(new TimingPoint(120, beatTime));
                pos += beatPosition + beatWindowLength;
            }
            //*/

            return timingPoints;
        }

        static List<TimingPoint> TestBeatfinding(bool destructive, List<TimingPoint> timingPoints, AudioData audioData, double t, double windowSize, double beatSize, float resolution)
        {
            int windowLength = audioData.ToSample(windowSize);
            int beatWindowLength = audioData.ToSample(beatSize);

            //Slice<float> data = PrepareData(audioData, !destructive);
            int a = audioData.ToArrayIndex(t);
            int b = audioData.ToArrayIndex(t + windowSize);
            Slice<float> data = new Slice<float>(audioData.Data).GetSlice(a, b);
            data = data.DeepCopy();

            FloatArrays.ExtractChannel(data, data, audioData.Channels, 0);
            data = data.GetSlice(0, data.Length / audioData.Channels);
            a /= audioData.Channels;
            b /= audioData.Channels;

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

            /*
			Form1.Instance.IsTesting = false;

            timingPoints = FindAllBeats(audioData, 0.2, 0.01, res);
            timingPoints = TimingPointList.RemoveDebugPoints(timingPoints);
            //*/

            //*
			Form1.Instance.IsTesting = true;
			
            double t = audioData.PositionSeconds;
            
            //double t = 0.31471655328798187;
            //double t = 0.30471655328798187;

            bool destructive = false;
            TestBeatfinding(destructive, timingPoints, audioData, t, 0.2, 0.01, res);

            if (destructive)
            {
                for (int i = 0; i < timingPoints.Count; i++)
                {
                    timingPoints[i].OffsetSeconds /= 2.0;
                }
            }

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
