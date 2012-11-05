﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using openHistorian.V2.Collections;

namespace openHistorian.V2.Collections.Test
{
    [TestClass]
    public class LargeArray
    {
        [TestMethod]
        public void TestLargeArray()
        {
            TestArray(new LargeArray<int>());
        }

        [TestMethod]
        public unsafe void TestLargeUnmanagedArray()
        {
            Assert.AreEqual(Globals.BufferPool.AllocatedBytes, 0);
            using (var array = new LargeUnmanagedArray<int>(4, Globals.BufferPool, ptr => *(int*)ptr, (ptr, v) => *(int*)ptr = v))
            {
                TestArray(array);
            }
            Assert.AreEqual(Globals.BufferPool.AllocatedBytes, 0);

        }

        public void TestArray(ILargeArray<int> array)
        {
            for (int x = 0; x < 2500000; x++)
            {
                if (x >= array.Capacity)
                {
                    HelperFunctions.ExpectError(() => array[x] = x);
                    array.SetCapacity(array.Capacity + 1);
                }
                array[x] = x;
            }

            for (int x = 0; x < 2500000; x++)
            {
                Assert.AreEqual(array[x], x);
            }
        }
    }
}