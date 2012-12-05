﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using openHistorian.V2.FileStructure;
using openHistorian.V2.IO.Unmanaged;

namespace openHistorian.V2.Collections.KeyValue
{
    [TestFixture]
    public class SortedTree256Test
    {
        const uint Count = 100000;

        [Test]
        public void SortedTree256Archive()
        {
            using (TransactionalFileStructure file = TransactionalFileStructure.CreateInMemory(4096))
            using (var edit = file.BeginEdit())
            using (var stream = edit.CreateFile(Guid.NewGuid(), 12))
            using (BinaryStream bs = new BinaryStream(stream))
            {
                SortedTree256BaseTest.BenchmarkTree(() => new SortedTree256(bs, 4096), Count);
            }
            Console.WriteLine((DiskIo.ChecksumCount/11).ToString("N0"));
        }

        [Test]
        public void SortedTree256()
        {
            using (BinaryStream bs = new BinaryStream())
            {
                SortedTree256BaseTest.BenchmarkTree(() => new SortedTree256(bs, 4096), Count);
            }
        }

        [Test]
        public void SortedTree256Delta()
        {
            using (BinaryStream bs = new BinaryStream())
            {
                SortedTree256BaseTest.BenchmarkTree(() => new SortedTree256DeltaEncoded(bs, 4096), Count);
            }
        }

        [Test]
        public void SortedTree256TS()
        {
            using (BinaryStream bs = new BinaryStream())
            {
                SortedTree256BaseTest.BenchmarkTree(() => new SortedTree256TSEncoded(bs, 4096), Count);
            }
        }
    }
}