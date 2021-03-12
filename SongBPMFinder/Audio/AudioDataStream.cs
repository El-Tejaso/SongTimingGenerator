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
			//calculate in terms of the actual array
			int channels = audioData.Channels;
			int len = audioData.Length * channels;
			int position = audioData.CurrentSample * channels;
			
            //ensre we dont read past the end of our data buffer
            if (position + count >= len)
            {
                count = len - 1 - position;
            }

            //return 0 if there is nothing to read
            if (count <= 0) return 0;

            double slowdown = GetCurrentSlowdown();

            for (int i = 0; i < count; i+=channels)
            {
                int currentIndex = position + (int)((double)i * slowdown);
                int nextIndex = Math.Min(currentIndex + channels, len - channels);

                float t = (float)(((double)i * slowdown) % 1.0);

                for (int j = 0; j < channels; j++)
                {
                    float thisSample = audioData.Data[currentIndex + j];
                    float nextSample =  audioData.Data[nextIndex + j];

                    buffer[offset + i + j] = QuickMafs.Lerp(thisSample, nextSample, t);
                }
            }

            audioData.CurrentSample += (int)(slowdown*(count/channels));
            return count;
        }
    }
}
