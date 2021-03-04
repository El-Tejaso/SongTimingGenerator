using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;


namespace SongBPMFinder.Audio
{
    public class AudioDataStream : WaveProvider32
    {
        private AudioData audioData;

        public AudioDataStream(AudioData data)
            : base(data.SampleRate, data.Channels) 
        {
            this.audioData = data;
        }

        public WaveFormat WaveFormat {
            get => audioData.WaveFormat;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            float[] data = audioData.Data;
            if (audioData.Position + count >= data.Length)
            {
                count = data.Length - 1 - audioData.Position;
            }

            if (count <= 0) return 0;

            for (int i = 0; i < count; i++)
            {
                buffer[i + offset] = data[audioData.Position + i];
            }

            audioData.Position += count;
            return count;
        }
    }
}
