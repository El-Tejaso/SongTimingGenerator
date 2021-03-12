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
        float[] rawData;
        Slice<float> data;
        int sampleRate;
        int channels;

        private int currentSample = 0;
        private int len;

        public int Length => len;

        public int CurrentSample {
            get => currentSample;
            set {
                currentSample = value;
                if (currentSample < 0) currentSample = 0;
                if (currentSample >= Length) currentSample = Length - 1;
            }
        }

        public double CurrentSampleSeconds {
            get {
                return SampleToSeconds(CurrentSample);
            }
        }

        public float[] RawData {
            get => data.GetInternalArray();
        }

        public Slice<float> Data {
            get => data;
        }

        public Slice<float> GetChannel(int c)
        {
            return data.GetSlice(c % Channels, data.Length, Channels);
        }

        public int SampleRate {
            get => sampleRate;
        }

        public int Channels {
            get => channels;
        }

        public double Duration {
            get => SampleToSeconds(Length);
        }

        WaveFormat metadata;
        public WaveFormat WaveFormat {
            get => metadata;
        }

        public double SampleToSeconds(int sample)
        {
            return sample / (double)SampleRate;
        }

        public int ToSample(double seconds)
        {
            return (int)(seconds * SampleRate);
        }

		public float GetSample(int sample, int channel){
            return data[channel % Channels + sample * Channels];
		}

        private void initialize(float[] data, int sampleRate, int numChannels)
        {
            this.rawData = data;
            this.data = new Slice<float>(data);

            this.sampleRate = sampleRate;
            this.channels = numChannels;
            this.metadata = new WaveFormat(sampleRate, numChannels);
            this.len = data.Length / numChannels;
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
                rawData = new float[numSamples];
                isp.Read(rawData, 0, rawData.Length);

                initialize(rawData, sampleRate, channels);
            }

            Logger.Log("Opened [" + filepath + "]");
            Logger.Log("Sample rate: [" + SampleRate + "]");
            Logger.Log("Duration: [" + Duration + "]");
        }
    }
}
