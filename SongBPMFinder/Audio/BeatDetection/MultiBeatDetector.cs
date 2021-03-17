using SongBPMFinder.Audio.Timing;
using SongBPMFinder.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.BeatDetection
{
    public enum BeatDetectionType
    {
        Default,
        Coalescing
    }

    public class MultiBeatDetector
    {
        //A heuristic based approach to beat detection
        public static List<TimingPoint> DetectAllBeats(AudioData audioData, double windowSize, double beatSize, float resolution)
        {
            //TODO: fix copypaste of this stuff
            Slice<float> data = PrepareData(audioData);
            List<TimingPoint> timingPoints = new List<TimingPoint>();
            int windowLength = audioData.ToSample(windowSize);
            float[] windowBuffer = new float[windowLength];
            Slice<float> spareBuffer = new Slice<float>(new float[windowLength]);


            int beatWindowLength = audioData.ToSample(beatSize);

            int pos = 0;
            while (pos < data.Length)
            {
                pos = FindNextBeat(pos, audioData, ref data, windowBuffer, spareBuffer, resolution);

                double beatTime = audioData.SampleToSeconds(pos);
                timingPoints.Add(new TimingPoint(120, beatTime));

                pos += beatWindowLength;
            }

            return timingPoints;
        }
        
        //A brute force approach to beat detection. probably takes way longer but may be more accurate
        public static List<TimingPoint> DetectAllBeatsCoalescing(AudioData audioData, double windowSize, double coalesceWindow, float resolution)
        {
            //TODO: fix copypaste of this stuff
            Slice<float> data = PrepareData(audioData);
            List<TimingPoint> timingPoints = new List<TimingPoint>();
            int windowLength = audioData.ToSample(windowSize);
            float[] windowBuffer = new float[windowLength];
            Slice<float> spareBuffer = new Slice<float>(new float[windowLength]);


            int pos = 0;
            while (pos < data.Length)
            {
                int beatPosition = DetectBeatInWindow(pos, audioData, ref data, windowBuffer, spareBuffer, resolution);

                if (beatPosition != -1)
                {
                    double beatTime = audioData.SampleToSeconds(pos + beatPosition);
                    TimingPointList.AddCoalescing(timingPoints, new TimingPoint(120, beatTime), coalesceWindow);
                }

                pos += windowBuffer.Length / 10;
            }

            timingPoints = RemoveLowWeightTimingPoints(timingPoints);

            return timingPoints;
        }

        private static List<TimingPoint> RemoveLowWeightTimingPoints(List<TimingPoint> timingPoints)
        {
            List<TimingPoint> cleanList = new List<TimingPoint>();
            for (int i = 0; i < timingPoints.Count; i++)
            {
                if (timingPoints[i].Weight > 2.0)
                {
                    cleanList.Add(timingPoints[i]);
                }
            }

            return cleanList;
        }

        private static int FindNextBeat(int pos, AudioData audioData, ref Slice<float> data, float[] windowBuffer, Slice<float> spareBuffer, float resolution)
        {
            bool found = false;
            int windowLength = windowBuffer.Length;

            while (!found)
            {
                if (pos + windowLength >= data.Length)
                    break;

                int beatPosition = DetectBeatInWindow(pos, audioData, ref data, windowBuffer, spareBuffer, resolution);


                if (beatPosition != -1)
                {
                    pos += beatPosition;
                    found = true;
                }
                else
                {
                    pos += windowLength / 4;
                }
            }

            return pos;
        }

        private static int DetectBeatInWindow(int pos, AudioData audioData, ref Slice<float> data, float[] windowBuffer, Slice<float> spareBuffer, float resolution)
        {
            Slice<float> windowSlice = CopyWindowToBuffer(ref data, pos, windowBuffer);
            int beatPosition = BeatDetector.DetectBeat(audioData, windowSlice, spareBuffer, resolution, 4, false);
            return beatPosition;
        }

        private static Slice<float> CopyWindowToBuffer(ref Slice<float> data, int pos, float[] windowBuffer)
        {
            return data.GetSlice(pos, Math.Min(pos + windowBuffer.Length, data.Length)).DeepCopy(windowBuffer);
        }

        private static Slice<float> PrepareData(AudioData audioData)
        {
            return audioData.GetChannel(0).DeepCopy();
        }
    }
}
