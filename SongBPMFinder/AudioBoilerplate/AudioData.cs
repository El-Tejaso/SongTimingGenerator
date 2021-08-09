using NAudio.Wave;
using System;

namespace SongBPMFinder
{
    public class AudioData
    {
        AudioSlice[] data;

        int sampleRate;
        int numChannels;

        private int currentSample = 0;
        private int len;

        public int Length => len;


        public int CurrentSample {
            get => currentSample;
            set {
                currentSample = value;
                if (currentSample < 0)
                    currentSample = 0;
                if (currentSample >= Length)
                    currentSample = Length - 1;
            }
        }

        public double CurrentSampleSeconds {
            get {
                return SampleToSeconds(CurrentSample);
            }
        }

        public AudioSlice GetChannel(int c)
        {
            return data[c];
        }

        public int SampleRate {
            get => sampleRate;
        }

        public int Channels {
            get => numChannels;
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
            return data[0].GetSecond(sample);
        }

        public int ToSample(double seconds)
        {
            return data[0].GetSample(seconds);
        }

        public float GetSample(int sample, int channel)
        {
            return data[channel][sample];
        }

        private void initialize(AudioSlice[] channelSeperatedData, int sampleRate)
        {
            this.data = channelSeperatedData;

            this.sampleRate = sampleRate;
            this.numChannels = channelSeperatedData.Length;
            this.metadata = new WaveFormat(sampleRate, numChannels);
            this.len = this.data[0].Length;
        }

        public AudioData(AudioSlice[] channelSeperatedData, int sampleRate)
        {
            initialize(channelSeperatedData, sampleRate);
        }

        public static AudioData FromFile(string filepath)
        {
            try
            {
                using (MediaFoundationReader media = new MediaFoundationReader(filepath))
                {
                    return loadFromFileNoErrorHandling(filepath, media);
                }
            }
            catch(Exception e)
            {
                Logger.Log("Failed to open [" + filepath + "]");
                return null;
            }
        }

        private static AudioData loadFromFileNoErrorHandling(string filepath, MediaFoundationReader media)
        {
            WaveFormat metadata = media.WaveFormat;
            int sampleRate = metadata.SampleRate;
            int numChannels = metadata.Channels;

            float[] rawData = audioFileToFloatArray(media, sampleRate, numChannels);
            AudioSlice[] channelSeperatedData = floatArrayToChannelSeperatedData(sampleRate, numChannels, rawData);

            AudioData result = new AudioData(channelSeperatedData, sampleRate);

            logSuccessfulOpening(filepath, result);

            return result;
        }

        //Cheers https://stackoverflow.com/questions/42483778/how-to-get-float-array-of-samples-from-audio-file
        private static float[] audioFileToFloatArray(MediaFoundationReader media, int sampleRate, int numChannels)
        {
            ISampleProvider isp = media.ToSampleProvider();

            int numSamples = (int)(media.TotalTime.TotalSeconds * sampleRate * numChannels);
            float[] rawData = new float[numSamples];
            isp.Read(rawData, 0, rawData.Length);
            return rawData;
        }

        private static AudioSlice[] floatArrayToChannelSeperatedData(int sampleRate, int numChannels, float[] rawData)
        {
            Slice<float> rawDataSlice = new Slice<float>(rawData);

            AudioSlice[] channelSeperatedData = new AudioSlice[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                Slice<float> currentChannel = rawDataSlice.GetSlice(i, rawData.Length - numChannels + i + 1, numChannels);
                channelSeperatedData[i] = new AudioSlice(currentChannel, sampleRate);
            }

            return channelSeperatedData;
        }

        private static void logSuccessfulOpening(string filepath, AudioData result)
        {
            Logger.Log("Opened [" + filepath + "]");
            Logger.Log("Sample rate: [" + result.SampleRate + "]");
            Logger.Log("Duration: [" + result.Duration + "]");
        }


        public AudioData DeepCopy()
        {
            AudioSlice[] deepCopyOfData = new AudioSlice[Channels];
            for(int i = 0; i < data.Length; i++)
            {
                deepCopyOfData[i] = data[i].DeepCopy();
            }

            return new AudioData(deepCopyOfData, SampleRate);
        }
    }
}
