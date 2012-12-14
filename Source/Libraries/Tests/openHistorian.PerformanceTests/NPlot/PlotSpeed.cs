﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPlot;
using NUnit.Framework;
using NPlot.Bitmap;
using PlotSurface2D = NPlot.Bitmap.PlotSurface2D;

namespace openHistorian.PerformanceTests.NPlot
{
    [TestFixture]
    public class PlotSpeed
    {
        [Test]
        public void RefreshSpeed()
        {
            var xVal = new List<double>();
            var yVal = new List<double>();

            for (int x = 0; x < 100000; x++)
            {
                xVal.Add(x);
                yVal.Add(1 - x);
            }
            Stopwatch sw = new Stopwatch();
            LinePlot p1 = new LinePlot(yVal, xVal);

            PlotSurface2D plot = new PlotSurface2D(640, 480);

            sw.Start();

            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);
            plot.Add(p1);

            plot.Refresh();

            sw.Stop();

            Console.WriteLine(sw.Elapsed.TotalSeconds.ToString() + " seconds to refresh");
        }
        [Test]
        public void RefreshSpeedTest()
        {
            DebugStopwatch sw = new DebugStopwatch();
            double time = sw.TimeEvent(RefreshSpeed);
            Console.WriteLine(time.ToString() + " seconds to on average");

        }
    }
}