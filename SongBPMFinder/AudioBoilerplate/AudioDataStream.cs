using NAudio.Wave;
using System;

namespace SongBPMFinder
{
    public enum PlaybackRate
    {
        Realtime,
        ThreeFourths,
        Halftime,
        Quartertime
    };

    public class AudioDataStream : WaveProvider32
    {
        private AudioData audioData;

        private PlaybackRate currentPlaybackType = PlaybackRate.Realtime;
        private float playbackRate;

        public PlaybackRate PlaybackRate {
            get => currentPlaybackType;
            set {
                currentPlaybackType = value;
                playbackRate = getPlaybackRate(value);
            }
        }

        private float getPlaybackRate(PlaybackRate enumeration)
        {
            switch (enumeration)
            {
                case PlaybackRate.Realtime:
                    return 1;
                case PlaybackRate.ThreeFourths:
                    return 0.75f;
                case PlaybackRate.Halftime:
                    return 0.5f;
                case PlaybackRate.Quartertime:
                    return 0.25f;
            }
            return -1;
        }


        public AudioDataStream(AudioData data)
            : base(data.SampleRate, data.NumChannels)
        {
            this.audioData = data;
            PlaybackRate = PlaybackRate.Realtime;
        }
        

        public override int Read(float[] buffer, int offset, int count)
        {
            //calculate in terms of the actual array
            int numChannels = audioData.NumChannels;
            int len = audioData.Length;
            double deltaPosition = 0;
            int lastSamplePosition = audioData.CurrentSample;

            int floatsRead = 0;

            while(floatsRead < count)
            {
                int position = lastSamplePosition = audioData.CurrentSample + (int)(deltaPosition);

                if (position >= audioData.Length)
                    break;

                int nextPosition = Math.Min(position + 1, len - 1);
                float lerpFactor = (float)(deltaPosition % 1.0);

                int indexIntoOutputBuffer = offset + floatsRead;

                for (int j = 0; j < numChannels; j++)
                {
                    float interpolatedSample = MathUtilF.Lerp(
                        audioData[j][position],
                        audioData[j][nextPosition], 
                        lerpFactor);

                    buffer[indexIntoOutputBuffer + j] = interpolatedSample;
                }

                floatsRead += numChannels;
                deltaPosition+=playbackRate;
            }

            audioData.SetCurrentSampleNoEvent(lastSamplePosition);
            return floatsRead;
        }
    }
}
