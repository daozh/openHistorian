﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.IO.FileStructure;
using GSF.IO.Unmanaged;
using GSF.Snap.Collection;
using GSF.Snap.Tree;
using NUnit.Framework;
using openHistorian.Collections;
using openHistorian.Snap;

namespace GSF.Snap.Storage
{
    [TestFixture]
    public class SortedTreeFileSimpleWriterTest
    {
        [Test]
        public void TestOld()
        {
            Test(1000, false);

            int pointCount = 10000000;
            SortedPointBuffer<HistorianKey, HistorianValue> points = new SortedPointBuffer<HistorianKey, HistorianValue>(pointCount, true);

            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();

            for (int x = 0; x < pointCount; x++)
            {
                key.PointID = (ulong)x;
                points.TryEnqueue(key, value);
            }

            points.IsReadingMode = true;

            File.Delete(@"C:\Temp\fileTemp.~d2i");
            File.Delete(@"C:\Temp\fileTemp.d2i");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (var file = SortedTreeFile.CreateFile(@"C:\Temp\fileTemp.~d2i"))
            using (var table = file.OpenOrCreateTable<HistorianKey, HistorianValue>(EncodingDefinition.FixedSizeCombinedEncoding))
            {
                using (var edit = table.BeginEdit())
                {
                    edit.AddPoints(points);
                    edit.Commit();
                }
            }

            //SortedTreeFileSimpleWriter<HistorianKey, HistorianValue>.Create(@"C:\Temp\fileTemp.~d2i", @"C:\Temp\fileTemp.d2i", 4096, SortedTree.FixedSizeNode, points);

            sw.Stop();

            System.Console.WriteLine(SimplifiedSubFileStreamIoSession.ReadBlockCount);
            System.Console.WriteLine(SimplifiedSubFileStreamIoSession.WriteBlockCount);
            System.Console.WriteLine(sw.Elapsed.TotalSeconds.ToString());

        }

        [Test]
        public void CountIO()
        {
            Test(1000, false);

            int pointCount = 10000000;
            SortedPointBuffer<HistorianKey, HistorianValue> points = new SortedPointBuffer<HistorianKey, HistorianValue>(pointCount, true);

            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();

            for (int x = 0; x < pointCount; x++)
            {
                key.PointID = (ulong)x;
                points.TryEnqueue(key, value);
            }

            points.IsReadingMode = true;

            File.Delete(@"C:\Temp\fileTemp.~d2i");
            File.Delete(@"C:\Temp\fileTemp.d2i");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            SortedTreeFileSimpleWriter<HistorianKey, HistorianValue>.Create(@"C:\Temp\fileTemp.~d2i", @"C:\Temp\fileTemp.d2i", 4096, null, EncodingDefinition.FixedSizeCombinedEncoding, points);

            sw.Stop();

            System.Console.WriteLine(SimplifiedSubFileStreamIoSession.ReadBlockCount);
            System.Console.WriteLine(SimplifiedSubFileStreamIoSession.WriteBlockCount);
            System.Console.WriteLine(sw.Elapsed.TotalSeconds.ToString());

        }


        [Test]
        public void Test()
        {
            for (int x = 1; x < 1000000; x *= 2)
            {
                Test(x, true);
                System.Console.WriteLine(x);
            }
        }

        public void Test(int pointCount, bool verify)
        {
            SortedPointBuffer<HistorianKey, HistorianValue> points = new SortedPointBuffer<HistorianKey, HistorianValue>(pointCount, true);

            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();

            for (int x = 0; x < pointCount; x++)
            {
                key.PointID = (ulong)x;
                points.TryEnqueue(key, value);
            }

            points.IsReadingMode = true;

            File.Delete(@"C:\Temp\fileTemp.~d2i");
            File.Delete(@"C:\Temp\fileTemp.d2i");

            SortedTreeFileSimpleWriter<HistorianKey, HistorianValue>.Create(@"C:\Temp\fileTemp.~d2i", @"C:\Temp\fileTemp.d2i", 4096, null, EncodingDefinition.FixedSizeCombinedEncoding, points);
            if (!verify)
                return;
            using (var file = SortedTreeFile.OpenFile(@"C:\Temp\fileTemp.d2i", true))
            using (var table = file.OpenTable<HistorianKey, HistorianValue>())
            using (var read = table.AcquireReadSnapshot().CreateReadSnapshot())
            using (var scanner = read.GetTreeScanner())
            {
                scanner.SeekToStart();
                int cnt = 0;
                while (scanner.Read(key, value))
                {
                    if (key.PointID != (ulong)cnt)
                        throw new Exception();
                    cnt++;

                }
                if (cnt != pointCount)
                    throw new Exception();


            }
        }

        [Test]
        public void TestNonSequential()
        {
            for (int x = 1; x < 1000000; x *= 2)
            {
                TestNonSequential(x, true);
                System.Console.WriteLine(x);
            }
        }

        public void TestNonSequential(int pointCount, bool verify)
        {
            SortedPointBuffer<HistorianKey, HistorianValue> points = new SortedPointBuffer<HistorianKey, HistorianValue>(pointCount, true);

            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();

            for (int x = 0; x < pointCount; x++)
            {
                key.PointID = (ulong)x;
                points.TryEnqueue(key, value);
            }

            points.IsReadingMode = true;

            File.Delete(@"C:\Temp\fileTemp.~d2i");
            File.Delete(@"C:\Temp\fileTemp.d2i");

            SortedTreeFileSimpleWriter<HistorianKey, HistorianValue>.CreateNonSequential(@"C:\Temp\fileTemp.~d2i", @"C:\Temp\fileTemp.d2i", 4096, null, EncodingDefinition.FixedSizeCombinedEncoding, points);
            if (!verify)
                return;
            using (var file = SortedTreeFile.OpenFile(@"C:\Temp\fileTemp.d2i", true))
            using (var table = file.OpenTable<HistorianKey, HistorianValue>())
            using (var read = table.AcquireReadSnapshot().CreateReadSnapshot())
            using (var scanner = read.GetTreeScanner())
            {
                scanner.SeekToStart();
                int cnt = 0;
                while (scanner.Read(key, value))
                {
                    if (key.PointID != (ulong)cnt)
                        throw new Exception();
                    cnt++;

                }
                if (cnt != pointCount)
                    throw new Exception();


            }
        }
    }
}
