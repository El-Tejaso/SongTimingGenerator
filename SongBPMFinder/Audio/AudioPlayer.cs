using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Timers;

namespace SongBPMFinder.Audio
{
    public class AudioPlayer
    {
        WaveOut output = null;
        WaveProvider32 audio;
        bool isPlaying = false;

        public bool IsPlaying {
            get => isPlaying;
        }

        public void Play()
        {
            if (isPlaying) return;
            if (output == null) return;

            isPlaying = true;
            output.Play();
        }

        public void Pause()
        {
            if (!isPlaying) return;
            if (output == null) return;

            isPlaying = false;
            output.Stop();
        }

        public void SetAudio(WaveProvider32 audio)
        {
            if(output != null)
            {
                Pause();

                output.Stop();
                output.Dispose();
                output = null;
            }

            this.audio = audio;
            output = new WaveOut();
            output.Init(audio);
        }
    }
}
