using NAudio.Wave;

namespace SongBPMFinder
{
    public class AudioPlayer
    {
        WaveOut output = null;
        AudioDataStream audio;
        bool isPlaying = false;

        public bool IsPlaying {
            get => isPlaying;
        }

        public void Play()
        {
            if (isPlaying)
                return;
            if (output == null)
                return;

            isPlaying = true;
            output.Play();
        }

        public void Pause()
        {
            if (!isPlaying)
                return;
            if (output == null)
                return;

            isPlaying = false;
            output.Stop();
        }

        public void SetAudio(AudioDataStream audio)
        {
            if (output != null)
            {
                Pause();

                output.Stop();
                output.Dispose();
                output = null;
            }

            this.audio = audio;

            output = new WaveOut(WaveCallbackInfo.FunctionCallback());
            output.Init(audio);
        }
    }
}
