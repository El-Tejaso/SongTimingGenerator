using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class TimingPipeline
    {
        List<TimeSeries> debugTimeSeries = new List<TimeSeries>();

        DefaultBeatDetector beatDetector;
        TimingGenerator timingGenerator;

        public TimingPipeline()
        {
            beatDetector = new DefaultBeatDetector(debugTimeSeries);
            timingGenerator = new TimingGenerator(debugTimeSeries);
        }

        /// <summary>
        /// This MUST be a power of two at the moment.
        /// </summary>
        public int FourierWindow {
            get => beatDetector.FourierWindow;
            set => beatDetector.FourierWindow = value;
        }

        public double EvalDistanceSeconds {
            get => beatDetector.EvalDistance;
            set => beatDetector.EvalDistance = value;
        }

        public int NumFrequencyBands {
            get => beatDetector.FrequencyBands;
            set => beatDetector.FrequencyBands = value;
        }

        public bool AddAllFrequenciesAtTheEnd {
            get => beatDetector.AddSeperateBands;
            set => beatDetector.AddSeperateBands = value;
        }


        public bool BinaryPeaks {
            get => beatDetector.BinaryPeaks;
            set => beatDetector.BinaryPeaks = value;
        }

        public bool LeftChannel {
            get => beatDetector.LeftChannel;
            set => beatDetector.LeftChannel = value;
        }

        public bool RightChannel {
            get => beatDetector.RightChannel;
            set => beatDetector.RightChannel = value;
        }

        public double Stride {
            get => beatDetector.Stride;
            set => beatDetector.Stride = value;
        }

        public FourierDifferenceType DifferenceFunction {
            get => beatDetector.DifferenceFunction;
            set => beatDetector.DifferenceFunction = value;
        }

        public double Start {
            get => beatDetector.Start;
            set {
                beatDetector.Start = value;
                timingGenerator.Start = value;
            }
        }

        public double End {
            get => beatDetector.End;
            set {
                beatDetector.End = value;
                timingGenerator.End = value;
            }
        }

        public float StandardDeviationThreshold {
            get => beatDetector.StandardDeviationThreshold;
            set => beatDetector.StandardDeviationThreshold = value;
        }

        public double PeakDetectWindow {
            get => beatDetector.PeakDetectWindow;
            set => beatDetector.PeakDetectWindow = value;
        }

        public float PeakDetectInfluence {
            get => beatDetector.PeakDetectInfluence;
            set => beatDetector.PeakDetectInfluence = value;
        }

        public List<TimeSeries> DebugTimeSeries {
            get {
                return debugTimeSeries;
            }
        }

        public bool CorrectFrequencies {
            get => beatDetector.CorrectFrequencies;
            set => beatDetector.CorrectFrequencies = value;
        }

        public TimingPointList TimeSong(AudioData audio)
        {
            debugTimeSeries.Clear();

            SortedList<Beat>[] beats = beatDetector.GetEveryBeat(audio);

            TimingPointList timingPoints = new TimingGenerator(debugTimeSeries).GenerateTiming(beats);

            return timingPoints;
        }
    }
}
