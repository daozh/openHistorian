﻿////******************************************************************************************************
////  BufferPoolKernel.cs - Gbtc
////
////  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
////
////  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
////  the NOTICE file distributed with this work for additional information regarding copyright ownership.
////  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
////  not use this file except in compliance with the License. You may obtain a copy of the License at:
////
////      http://www.opensource.org/licenses/eclipse-1.0.php
////
////  Unless agreed to in writing, the subject software distributed under the License is distributed on an
////  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
////  License for the specific language governing permissions and limitations.
////
////  Code Modification History:
////  ----------------------------------------------------------------------------------------------------
////  10/5/2012 - Steven E. Chisholm
////       Generated original version of source code. 
////       
////
////******************************************************************************************************

//using System;
//using System.Collections.Generic;

//namespace openHistorian.V2.UnmanagedMemory
//{
//    /// <summary>
//    /// Provides the fundamental functionality of many different <see cref="BufferPool"/> classes.
//    /// </summary>
//    /// <remarks>
//    /// All of the <see cref="BufferPool"/> classes should reside in one <see cref="BufferPoolKernel"/>.
//    /// This allows for the classes to share a common set of resources. Thus allowing for a true maximum amount of allocated memory
//    /// that can be shared across all pools.  Once the resource pool starts to become low. This modifies the behavior of all <see cref="BufferPool"/> 
//    /// classes that share this kernel.
//    /// </remarks>
//    public partial class BufferPoolKernel : IDisposable
//    {
        
//        /// <summary>
//        /// Deteremines the desired buffer pool utilization level.
//        /// Setting to Low will cause collection cycles to occur more often to keep the 
//        /// utilization level to low. 
//        /// </summary>
//        public enum TargetUtilizationLevels
//        {
//            Low = 0,
//            Medium = 1,
//            High = 2
//        }

//        /// <summary>
//        /// Contains 7 different levels of garbage collection that <see cref="BufferPool"/> classes
//        /// will need to consider when allocating new space.
//        /// </summary>
//        internal enum CollectionModes
//        {
//            None = 0,
//            Low = 1,
//            Normal = 2,
//            High = 3,
//            VeryHigh = 4,
//            Critical = 5,
//            Full = 6
//        }

//        #region [ Members ]

//        /// <summary>
//        /// Represents the ceiling for the amount of memory the buffer pool can use (124GB)
//        /// </summary>
//        public const long MaximumTestedSupportedMemoryCeiling = 124 * 1024 * 1024 * 1024L;
//        /// <summary>
//        /// Represents the minimum amount of memory that the buffer pool needs to work properly (128MB)
//        /// </summary>
//        public const long MinimumTestedSupportedMemoryFloor = 128 * 1024 * 1024;

//        /// <summary>
//        /// Gets the current <see cref="TargetUtilizationLevels"/>.
//        /// </summary>
//        public TargetUtilizationLevels TargetUtilizationLevel { get; private set; }


//        internal CollectionModes CollectionMode { get; private set; }

//        /// <summary>
//        /// Each page will be exactly this size (Based on RAM)
//        /// </summary>
//        public int PageSize { get; private set; }

//        /// <summary>
//        /// Provides a mask that the user can apply that can 
//        /// be used to get the offset position of a page.
//        /// </summary>
//        public int PageMask { get; private set; }

//        /// <summary>
//        /// Gets the number of bits that must be shifted to calculate an index of a position.
//        /// This is not the same as a page index that is returned by the allocate functions.
//        /// </summary>
//        public int PageShiftBits { get; private set; }

//        /// <summary>
//        /// When a buffer pool is completely full, a forced collection will occur.
//        /// The severity of the collection is specified as an integer passed in this event.
//        /// When Zero, the user initialized the collection.
//        /// When 1-10 the collection is due to out of memory.
//        /// If no more space is freed after the 10 is called, an out of memory exception occurs.
//        /// </summary>
//        internal event Action<int> ForceCollection;

//        /// <summary>
//        /// Notifies the <see cref="BufferPool"/> that the collection mode should be modified
//        /// </summary>
//        internal event Action<CollectionModes> CollectionModeChanged;

//        /// <summary>
//        /// Used for synchronizing access to this class.
//        /// </summary>
//        object m_syncRoot;

//        bool m_disposed;

//        #endregion

//        #region [ Constructors ]

//        /// <summary>
//        /// Creates a new <see cref="BufferPoolKernel"/> that can be used for user defined <see cref="BufferPool"/>.
//        /// </summary>
//        /// <param name="pageSize">The desired page size. This must be at least the size of the largest 
//        /// <see cref="BufferPool.PageSize"/> that will be created using this kernel. The recommeneded size is 1MB.</param>
//        /// <param name="maximumBufferSize">The desired maximum size of the allocation. Note: could be less if there is not enough system memory.</param>
//        /// <param name="utilizationLevel">Specifies the desired utilization level of the allocated space.</param>
//        public BufferPoolKernel(int pageSize = 1*1024*1024, long maximumBufferSize = MaximumTestedSupportedMemoryCeiling, TargetUtilizationLevels utilizationLevel = TargetUtilizationLevels.Low)
//        {
//            m_syncRoot = new object();
//            PageSize = pageSize;
//            PageMask = PageSize - 1;
//            PageShiftBits = BitMath.CountBitsSet((uint)PageMask);

//            InitializeSettings();
//            InitializeList();

//            SetMaximumBufferSize(maximumBufferSize);
//            SetTargetUtilizationLevel(utilizationLevel);
//        }



//        #endregion

//        #region [ Properties ]

//        /// <summary>
//        /// The number maximum supported number of bytes that can be allocated based
//        /// on the amount of RAM in the system.  This is not user configurable.
//        /// </summary>
//        public long SystemBufferCeiling { get; private set; }

//        /// <summary>
//        /// The number of bytes per Windows API allocation block
//        /// </summary>
//        public int MemoryBlockSize { get; private set; }

//        /// <summary>
//        /// The maximum amount of RAM that this buffer pool is configured to support
//        /// Attempting to allocate more than this will cause an out of memory exception
//        /// </summary>
//        public long MaximumBufferSize { get; private set; }

//        /// <summary>
//        /// Returns the number of bytes allocated by all buffer pools.
//        /// This does not include any pages that have been allocated but are not in use.
//        /// </summary>
//        public long CurrentAllocatedSize
//        {
//            get
//            {
//                return m_isPageAllocated.SetCount * (long)PageSize;
//            }
//        }

//        /// <summary>
//        /// Gets the number of bytes that have been allocated to this buffer pool 
//        /// by the OS.
//        /// </summary>
//        public long CurrentCapacity
//        {
//            get
//            {
//                return m_memoryBlocks.CountUsed * (long)MemoryBlockSize;
//            }
//        }

//        public bool IsDisposed
//        {
//            get
//            {
//                return m_disposed;
//            }
//        }

//        #endregion

//        #region [ Methods ]

//        /// <summary>
//        /// Requests a page from the buffered pool.
//        /// If there is not a free one available, method will block
//        /// and request a collection of unused pages by raising 
//        /// <see cref="ForceCollection"/> event.
//        /// </summary>
//        /// <param name="index">the index id of the page that was allocated</param>
//        /// <param name="addressPointer"> outputs a address that can be used
//        /// to access this memory address.  You cannot call release with this parameter.
//        /// Use the returned index to release pages.</param>
//        /// <remarks>The page allocated will not be initialized, 
//        /// so assume that the data is garbage.</remarks>
//        public void AllocatePage(out int index, out IntPtr addressPointer)
//        {
//            lock (m_syncRoot)
//            {
//                if (CurrentAllocatedSize == CurrentCapacity)
//                {
//                    //Grow the allocated pool

//                    long newSize = CurrentAllocatedSize;
//                    GrowBufferToSize(newSize + (long)(0.1 * MaximumBufferSize));

//                }
//                GetNextPage(out index, out addressPointer);
//            }
//        }

//        /// <summary>
//        /// Releases the page back to the buffer pool for reallocation.
//        /// </summary>
//        /// <param name="pageIndex"></param>
//        /// <remarks>The page released will not be initialized.
//        /// Releasing a page is on the honor system.  
//        /// Rereferencing a released page will most certainly cause 
//        /// unexpected crashing or data corruption or any other unexplained behavior.
//        /// </remarks>
//        public void ReleasePage(int pageIndex)
//        {
//            lock (m_syncRoot)
//            {
//                if (TryReleasePage(pageIndex))
//                {
//                    //ToDo: Consider calling the garbage collection routine and allow it to consider shrinking the pool.
//                }
//            }
//        }

//        public void ReleasePages(IEnumerable<int> pageIndexes)
//        {
//            lock (m_syncRoot)
//            {
//                foreach (int x in pageIndexes)
//                {
//                    TryReleasePage(x);
//                }
//            }
//        }

//        /// <summary>
//        /// Changes the allowable buffer size
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public long SetMaximumBufferSize(long value)
//        {
//            lock (m_syncRoot)
//            {
//                MaximumBufferSize = Math.Max(Math.Min(MaximumTestedSupportedMemoryCeiling, value), MinimumTestedSupportedMemoryFloor);
//                CalculateThresholds(MaximumBufferSize, TargetUtilizationLevel);
//                return MaximumBufferSize;
//            }
//        }
//        /// <summary>
//        /// Changes the utilization level
//        /// </summary>
//        /// <param name="utilizationLevel"></param>
//        /// <returns></returns>
//        public void SetTargetUtilizationLevel(TargetUtilizationLevels utilizationLevel)
//        {
//            lock (m_syncRoot)
//            {
//                TargetUtilizationLevel = utilizationLevel;
//                CalculateThresholds(MaximumBufferSize, TargetUtilizationLevel);
//            }
//        }

//        /// <summary>
//        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//        /// </summary>
//        /// <filterpriority>2</filterpriority>
//        public void Dispose()
//        {
//            m_disposed = true;
//        }

//        /// <summary>
//        /// Grows the buffer pool to have the desired size
//        /// </summary>
//        void GrowBufferToSize(long size)
//        {
//            size = Math.Min(size, MaximumBufferSize);
//            while (CurrentCapacity < size)
//            {
//                int pageIndex = m_memoryBlocks.AddValue(new Memory(MemoryBlockSize));
//                m_isPageAllocated.ClearBits(pageIndex * m_pagesPerMemoryBlock, m_pagesPerMemoryBlock);
//            }
//        }

//        #endregion

//    }
//}