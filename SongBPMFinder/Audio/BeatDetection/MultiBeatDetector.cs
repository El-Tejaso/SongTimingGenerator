using SongBPMFinder.Audio.Timing;
using SongBPMFinder.Slices;
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
        public static List<TimingPoint> DetectAllBeats(AudioData audioData, double windowSize, double beatSize, float resolution, int levels = 4)
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
                pos = FindNextBeat(pos, audioData, ref data, windowBuffer, spareBuffer, resolution, levels);

                double beatTime = audioData.SampleToSeconds(pos);
                timingPoints.Add(new TimingPoint(120, beatTime));

                pos += beatWindowLength;
            }

            return timingPoints;
        }
        
        //A brute force approach to beat detection. probably takes way longer but may be more accurate
        public static List<TimingPoint> DetectAllBeatsCoalescing(AudioData audioData, double windowSize, double coalesceWindow, float resolution, int levels = 4)
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
                BeatData beatData = DetectBeatInWindow(pos, audioData, ref data, windowBuffer, spareBuffer, resolution, levels);

                double beatTime = audioData.SampleToSeconds(pos + beatData.Position);
                TimingPointList.AddCoalescing(timingPoints, new TimingPoint(120, beatTime, (double)beatData.RelativeStrength), coalesceWindow);

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

        private static int FindNextBeat(int pos, AudioData audioData, ref Slice<float> data, float[] windowBuffer, Slice<float> spareBuffer, float resolution, int levels)
        {
            bool found = false;
            int windowLength = windowBuffer.Length;

            while (!found)
            {
                if (pos + windowLength >= data.Length)
                    break;

                BeatData beatData = DetectBeatInWindow(pos, audioData, ref data, windowBuffer, spareBuffer, resolution, levels);


                if (beatData.RelativeStrength >= 1)
                {
                    pos += beatData.Position;
                    found = true;
                }
                else
                {
                    pos += windowLength / 4;
                }
            }

            return pos;
        }

        private static BeatData DetectBeatInWindow(int pos, AudioData audioData, ref Slice<float> data, float[] windowBuffer, Slice<float> spareBuffer, float resolution, int levels)
        {
            Slice<float> windowSlice = CopyWindowToBuffer(ref data, pos, windowBuffer);
            BeatData beatData = BeatDetector.DetectBeat(audioData, windowSlice, spareBuffer, resolution, levels, false);
            return beatData;
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
