using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio
{
    public class AudioData 
    {
        float[] data;
        int sampleRate;
        int channels;
        private int playbackPosition = 0;

        public int Position {
            get => playbackPosition;
            set {
                playbackPosition = value;
                if (playbackPosition < 0) playbackPosition = 0;
                if (playbackPosition >= Data.Length) playbackPosition = Data.Length - 1;
            }
        }

        public double PositionSeconds {
            get {
                return IndexToSeconds(Position);
            }
        }

        public float[] Data {
            get => data;
        }

        public int SampleRate {
            get => sampleRate;
        }

        public int Channels {
            get => channels;
        }

        public double Duration {
            get => IndexToSeconds(data.Length);
        }

        WaveFormat metadata;
        public WaveFormat WaveFormat {
            get => metadata;
        }

        public double SampleToSeconds(int sample)
        {
            return sample / (double)SampleRate;
        }

        public double IndexToSeconds(int arrayIndex){
			return (arrayIndex / (double)Channels)/(double)SampleRate;
		}

		public int ToArrayIndex(double seconds){
			return (int)(seconds * SampleRate * Channels);
		}

        public int ToSample(double seconds)
        {
            return (int)(seconds * SampleRate);
        }

        private void initialize(float[] data, int sampleRate, int numChannels)
        {
            this.data = data;
            this.sampleRate = sampleRate;
            this.channels = numChannels;
            this.metadata = new WaveFormat(sampleRate, numChannels);
        }

        public AudioData(float[] data, int sampleRate, int numChannels)
        {
            initialize(data, sampleRate, numChannels);
        }

        public AudioData(string filepath) 
        {
            //Cheers https://stackoverflow.com/questions/42483778/how-to-get-float-array-of-samples-from-audio-file
            using (MediaFoundationReader media = new MediaFoundationReader(filepath))
            {
                metadata = media.WaveFormat;
                sampleRate = metadata.SampleRate;
                channels = metadata.Channels;

                ISampleProvider isp = media.ToSampleProvider();

                int numSamples = (int)(media.TotalTime.TotalSeconds * sampleRate * channels);
                data = new float[numSamples];
                isp.Read(data, 0, data.Length);

                initialize(data, sampleRate, channels);
            }

            Logger.Log("Opened [" + filepath + "]");
            Logger.Log("Sample rate: [" + SampleRate + "]");
            Logger.Log("Duration: [" + Duration + "]");
        }
    }
}
