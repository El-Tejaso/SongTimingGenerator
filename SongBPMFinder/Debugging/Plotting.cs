using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    internal static class Plotting
    {
        private static List<CustomWaveViewer> plots = new List<CustomWaveViewer>();
        private static List<TabPage> plotContainers = new List<TabPage>();

        internal static void LinkPlottingGraph(CustomWaveViewer viewer, TabPage tabPage)
        {
            plots.Add(viewer);
            plotContainers.Add(tabPage);
        }


        //A hack that uses the existing waveform drawing code to plot anything else
        public static void Plot(int graphNumber, string title, AudioSlice data)
        {
            AudioData audioData = new AudioData(new AudioSlice[] { data }, 2*data.Length);
            plots[graphNumber].AudioData = audioData;
            plotContainers[graphNumber].Text = title;
        }
    }
}
