using NAudio.Wave;
using System;

namespace SongBPMFinder
{
    /// <summary>
    /// A class for loading, storing and keeping track of playback within an audio file.
    /// 
    /// </summary>
    public class AudioData
    {
        AudioSlice[] data;
        private int len;
        private int sampleRate;
        private int currentSample = 0;
        WaveFormat nAudioWaveformat;


        public int Length {
            get {
                return len;
            }
        }

        public AudioSlice this[int channel] {
            get {
                return data[channel];
            }
        }

        public int CurrentSample {
            get => currentSample;
            private set {
                currentSample = value;
                if (currentSample < 0)
                    currentSample = 0;
                if (currentSample >= Length)
                    currentSample = Length - 1;
            }
        }

        public event Action OnPositionManuallyChanged;

        /// <summary>
        /// This should be used from an audio thread.
        /// No events will be invoked.
        /// </summary>
        internal void SetCurrentSampleNoEvent(int value)
        {
            CurrentSample = value;
        }

        /// <summary>
        /// This will be called from the main thread in response to user input or something.
        /// It should NOT be called form an audio thread
        /// </summary>
        public void SetCurrentSampleWithEvent(int value)
        {
            CurrentSample = value;
            OnPositionManuallyChanged?.Invoke();
        }


        public double CurrentSampleSeconds {
            get {
                return SampleToSeconds(CurrentSample);
            }
        }

        public double SampleToSeconds(int sample)
        {
            return sample / (double)sampleRate;
        }

        public int ToSample(double seconds)
        {
            return (int)(seconds * sampleRate);
        }


        public int SampleRate {
            get => sampleRate;
        }

        public int NumChannels {
            get => data.Length;
        }

        public double Duration {
            get => SampleToSeconds(Length);
        }

        public WaveFormat WaveFormat {
            get => nAudioWaveformat;
        }

        /// <summary>
        /// the sampleRate provided to this class's constructor will override the sample rates
        /// used in either channel of channelSeperatedData
        /// </summary>
        public AudioData(AudioSlice[] channelSeperatedData, int sampleRate)
        {
            this.data = channelSeperatedData;
            this.len = channelSeperatedData[0].Length;

            this.sampleRate = sampleRate;
            this.nAudioWaveformat = new WaveFormat(sampleRate, NumChannels);
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
            AudioSlice[] deepCopyOfData = new AudioSlice[NumChannels];
            for(int i = 0; i < data.Length; i++)
            {
                deepCopyOfData[i] = data[i].DeepCopy();
            }

            return new AudioData(deepCopyOfData, SampleRate);
        }
    }
}
