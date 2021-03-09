using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio
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

        float GetCurrentSlowdown()
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
            float[] data = audioData.Data;
            //ensre we dont read past the end of our data buffer
            if (audioData.Position + count >= data.Length)
            {
                count = data.Length - 1 - audioData.Position;
            }

            //return 0 if there is nothing to read
            if (count <= 0) return 0;

            double slowdown = GetCurrentSlowdown();

            for (int i = 0; i < count; i+=audioData.Channels)
            {
                int currentIndex = (int)((double)i * slowdown);
                int nextIndex = Math.Min(currentIndex + audioData.Channels, data.Length);
                float t = (float)(((double)i * slowdown) % 1.0);

                for (int j = 0; j < audioData.Channels; j++)
                {
                    float thisSample = data[audioData.Position + currentIndex + j];
                    float nextSample = data[audioData.Position + nextIndex + j];

                    buffer[offset + i + j] = QuickMafs.Lerp(thisSample, nextSample, t);
                }
            }

            audioData.Position += (int)(slowdown*count);
            return count;
        }
    }
}
