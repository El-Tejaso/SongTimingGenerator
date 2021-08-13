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
        private float playbackRate;

        public Playback Playback {
            get => currentPlaybackType;
            set {
                currentPlaybackType = value;
                playbackRate = getPlaybackRate(value);
            }
        }

        private float getPlaybackRate(Playback enumeration)
        {
            switch (enumeration)
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


        public AudioDataStream(AudioData data)
            : base(data.SampleRate, data.NumChannels)
        {
            this.audioData = data;
        }
        

        public override int Read(float[] buffer, int offset, int count)
        {
            //calculate in terms of the actual array
            int numChannels = audioData.NumChannels;
            int len = audioData.Length;
            int position = audioData.CurrentSample;

            int floatsRead = 0;

            while(floatsRead < count)
            {
                if (position >= audioData.Length)
                    break;

                int nextPosition = Math.Min(position + 1, len - 1);
                float lerpFactor = (float)(((double)position * playbackRate) % 1.0);

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
                position++;
            }

            audioData.SetCurrentSampleNoEvent(position);
            return floatsRead;
        }
    }
}
