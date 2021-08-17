using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    /// <summary>
    /// A hack that uses the existing waveform drawing code to plot anything else
    /// </summary>
    internal static class Plotting
    {
        private static List<CustomWaveViewer> plots = new List<CustomWaveViewer>();
        private static List<TabPage> plotContainers = new List<TabPage>();

        internal static void LinkPlottingGraph(CustomWaveViewer viewer, TabPage tabPage)
        {
            plots.Add(viewer);
            plotContainers.Add(tabPage);
        }


        public static void Plot(int graphNumber, string title, AudioChannel data)
        {
            AudioData audioData = new AudioData(new AudioChannel[] { data }, 2*data.Length);
            Plot(graphNumber, title, audioData);
        }

        public static void Plot(int graphNumber, string title, AudioData audioData)
        {
            plots[graphNumber].AudioData = audioData;
            plotContainers[graphNumber].Text = title;
        }
    }
}
