using NAudio.Wave;
using System;

namespace SongBPMFinder
{
    public enum Playback
    {
        Realtime,
        ThreeFourths,
        Halftime,
        Quartertime
    };

    public class AudioDataStream : WaveProvider32
    {
        private AudioData audioData;

        private Playback currentPlaybackType = Playback.Realtime;

        public Playback Playback {
            get => currentPlaybackType;
            set {
                currentPlaybackType = value;
            }
        }

        public AudioDataStream(AudioData data)
            : base(data.SampleRate, data.Channels)
        {
            this.audioData = data;
        }

        public WaveFormat WaveFormat {
            get => audioData.WaveFormat;
        }

        float GetPlaybackRate()
        {
            switch (currentPlaybackType)
            {
                case Playback.Realtime:
                    return 1;
                case Playback.ThreeFourths:
                    return 0.75f;
                case Playback.Halftime:
                    return 0.5f;
                case Playback.Quartertime:
                    return 0.25f;
            }
            return -1;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            //calculate in terms of the actual array
            int channels = audioData.Channels;
            int len = audioData.Length;
            int position = audioData.CurrentSample;

            //ensre we dont read past the end of our data buffer
            if (position + count >= len)
            {
                count = len - 1 - position;
            }

            //return 0 if there is nothing to read
            if (count <= 0)
                return 0;

            double playbackRate = GetPlaybackRate();

            int currentIndex = position;
            for (int currentUnit = 0, currentSample = 0; currentUnit < count; currentUnit += channels, currentSample++)
            {
                currentIndex = position + (int)((double)currentSample * playbackRate);
                int nextIndex = Math.Min(currentIndex + 1, len - 1);

                float t = (float)(((double)currentSample * playbackRate) % 1.0);

                int bufferBaseIndex = offset + currentUnit;

                for (int j = 0; j < channels; j++)
                {
                    AudioSlice chanelJ = audioData.GetChannel(j);
                    float thisSample = chanelJ[currentIndex];
                    float nextSample = chanelJ[nextIndex];

                    float interpolatedSample = MathUtilF.Lerp(thisSample, nextSample, t);

                    buffer[bufferBaseIndex + j] = interpolatedSample;
                }
            }

            audioData.CurrentSample = currentIndex;

            return count;
        }
    }
}
