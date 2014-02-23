﻿//******************************************************************************************************
//  SortedTreeKeyMethodsBase`1.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  4/12/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//     
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using GSF.IO;
using GSF.IO.Unmanaged;

namespace GSF.SortedTreeStore.Tree
{
    /// <summary>
    /// Specifies all of the core methods that need to be implemented for a <see cref="SortedTree"/> to be able
    /// to utilize this type of key.
    /// </summary>
    /// <remarks>
    /// There are many functions that are generically implemented in this class that can be overridden
    /// for vastly superiour performance.
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    public abstract class SortedTreeKeyMethodsBase<TKey>
        : SortedTreeMethodsBase<TKey>, IComparer<TKey>
        where TKey : class, new()
    {

        protected TKey TempKey = new TKey();
        protected TKey TempKey2 = new TKey();
        protected int LastFoundIndex;

        /// <summary>
        /// Sets the provided key to it's minimum value
        /// </summary>
        /// <param name="key"></param>
        public abstract void SetMin(TKey key);
        /// <summary>
        /// Sets the privided key to it's maximum value
        /// </summary>
        /// <param name="key"></param>
        public abstract void SetMax(TKey key);

        /// <summary>
        /// Compares two keys together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public abstract int CompareTo(TKey left, TKey right);

        /// <summary>
        /// Writes the maximum value to the provided stream
        /// </summary>
        /// <param name="stream"></param>
        public virtual unsafe void WriteMax(byte* stream)
        {
            SetMax(TempKey);
            Write(stream, TempKey);
        }
        /// <summary>
        /// Writes the minimum value to the provided stream
        /// </summary>
        /// <param name="stream"></param>
        public virtual unsafe void WriteMin(byte* stream)
        {
            SetMin(TempKey);
            Write(stream, TempKey);
        }
        /// <summary>
        /// Writes null to the provided stream (hint: Clear state)
        /// </summary>
        /// <param name="stream"></param>
        public virtual unsafe void WriteNull(byte* stream)
        {
            Clear(TempKey);
            Write(stream, TempKey);
        }

        /// <summary>
        /// Copies the source to the destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public virtual unsafe void Copy(byte* source, byte* destination)
        {
            Memory.Copy(source, destination, Size);
        }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public virtual unsafe void Copy(TKey source, TKey destination)
        {
            byte* ptr = stackalloc byte[Size];
            Write(ptr, source);
            Read(ptr, destination);
        }
        /// <summary>
        /// Reads the provided key from the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public virtual unsafe void Read(BinaryStreamBase stream, TKey data)
        {
            byte* ptr = stackalloc byte[Size];
            stream.ReadAll(ptr, Size);
            Read(ptr, data);
        }

        /// <summary>
        /// Reads the provided key from the BinaryReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="data"></param>
        public virtual unsafe void Read(BinaryReader reader, TKey data)
        {
            byte* ptr = stackalloc byte[Size];
            for (int x = 0; x < Size; x++)
            {
                ptr[x] = reader.ReadByte();
            }
            Read(ptr, data);
        }
        /// <summary>
        /// Writes the provided data to the BinaryWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        public virtual unsafe void Write(BinaryWriter writer, TKey data)
        {
            byte* ptr = stackalloc byte[Size];
            Write(ptr, data);
            for (int x = 0; x < Size; x++)
            {
                writer.Write(ptr[x]);
            }
        }
        /// <summary>
        /// Writes the provided data to the Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public virtual unsafe void Write(BinaryStreamBase stream, TKey data)
        {
            byte* ptr = stackalloc byte[Size];
            Write(ptr, data);
            stream.Write(ptr, Size);
        }

        /// <summary>
        /// Does a binary search on the data to find the best location for the <see cref="key"/>
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="key"></param>
        /// <param name="recordCount"></param>
        /// <param name="keyValueSize"></param>
        /// <returns></returns>
        public virtual unsafe int BinarySearch(byte* pointer, TKey key, int recordCount, int keyValueSize)
        {
            TKey compareKey = TempKey;
            if (LastFoundIndex == recordCount - 1)
            {
                Read(pointer + keyValueSize * LastFoundIndex, compareKey);
                if (IsGreaterThan(key, compareKey)) //Key > CompareKey
                {
                    LastFoundIndex++;
                    return ~recordCount;
                }
            }
            else if (LastFoundIndex < recordCount)
            {
                Read(pointer + keyValueSize * (LastFoundIndex + 1), compareKey);
                if (IsEqual(key, compareKey))
                {
                    LastFoundIndex++;
                    return LastFoundIndex;
                }
            }
            return BinarySearch2(pointer, key, recordCount, keyValueSize);
        }

        /// <summary>
        /// A catch all BinarySearch.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="key"></param>
        /// <param name="recordCount"></param>
        /// <param name="keyPointerSize"></param>
        /// <returns></returns>
        protected virtual unsafe int BinarySearch2(byte* pointer, TKey key, int recordCount, int keyPointerSize)
        {
            if (recordCount == 0)
                return ~0;
            TKey compareKey = TempKey;
            int searchLowerBoundsIndex = 0;
            int searchHigherBoundsIndex = recordCount - 1;

            if (LastFoundIndex <= recordCount)
            {
                LastFoundIndex = Math.Min(LastFoundIndex, recordCount - 1);
                Read(pointer + keyPointerSize * LastFoundIndex, compareKey);

                if (IsEqual(key, compareKey)) //Are Equal
                    return LastFoundIndex;
                if (IsGreaterThan(key, compareKey)) //Key > CompareKey
                {
                    //Value is greater, check the next key
                    LastFoundIndex++;

                    //There is no greater key
                    if (LastFoundIndex == recordCount)
                        return ~recordCount;

                    Read(pointer + keyPointerSize * LastFoundIndex, compareKey);

                    if (IsEqual(key, compareKey)) //Are Equal
                        return LastFoundIndex;
                    if (IsGreaterThan(key, compareKey)) //Key > CompareKey
                        searchLowerBoundsIndex = LastFoundIndex + 1;
                    else
                        return ~LastFoundIndex;
                }
                else
                {
                    //Value is lesser, check the previous key
                    //There is no lesser key;
                    if (LastFoundIndex == 0)
                        return ~0;

                    LastFoundIndex--;
                    Read(pointer + keyPointerSize * LastFoundIndex, compareKey);

                    if (IsEqual(key, compareKey)) //Are Equal
                        return LastFoundIndex;
                    if (IsGreaterThan(key, compareKey)) //Key > CompareKey
                    {
                        LastFoundIndex++;
                        return ~(LastFoundIndex);
                    }
                    else
                        searchHigherBoundsIndex = LastFoundIndex - 1;
                }
            }

            while (searchLowerBoundsIndex <= searchHigherBoundsIndex)
            {
                int currentTestIndex = searchLowerBoundsIndex + (searchHigherBoundsIndex - searchLowerBoundsIndex >> 1);

                Read(pointer + keyPointerSize * currentTestIndex, compareKey);

                if (IsEqual(key, compareKey)) //Are Equal
                {
                    LastFoundIndex = currentTestIndex;
                    return currentTestIndex;
                }
                if (IsGreaterThan(key, compareKey)) //Key > CompareKey
                    searchLowerBoundsIndex = currentTestIndex + 1;
                else
                    searchHigherBoundsIndex = currentTestIndex - 1;
            }

            LastFoundIndex = searchLowerBoundsIndex;

            return ~searchLowerBoundsIndex;
        }

        /// <summary>
        /// Gets if lowerBounds &lt;= key &lt; upperBounds
        /// </summary>
        /// <param name="lowerBounds"></param>
        /// <param name="key"></param>
        /// <param name="upperBounds"></param>
        /// <returns></returns>
        public virtual bool IsBetween(TKey lowerBounds, TKey key, TKey upperBounds)
        {
            return IsLessThanOrEqualTo(lowerBounds, key) && IsLessThan(key, upperBounds);
        }

        /// <summary>
        /// Gets if left &lt;= right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual bool IsLessThanOrEqualTo(TKey left, TKey right)
        {
            return CompareTo(left, right) <= 0;
        }

        /// <summary>
        /// Gets if left &lt; right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual bool IsLessThan(TKey left, TKey right)
        {
            return CompareTo(left, right) < 0;
        }

        /// <summary>
        /// Gets if left != right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual bool IsNotEqual(TKey left, TKey right)
        {
            return CompareTo(left, right) != 0;
        }

        /// <summary>
        /// Gets if left &gt; right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual bool IsGreaterThan(TKey left, TKey right)
        {
            return CompareTo(left, right) > 0;
        }

        /// <summary>
        /// Gets if left &gt;= right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe bool IsGreaterThan(TKey left, byte* right)
        {
            return CompareTo(left, right) > 0;
        }

        /// <summary>
        /// Gets if left &gt; right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe bool IsGreaterThan(byte* left, TKey right)
        {
            return CompareTo(left, right) > 0;
        }
        /// <summary>
        /// Gets if left &gt;= right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual bool IsGreaterThanOrEqualTo(TKey left, TKey right)
        {
            return CompareTo(left, right) >= 0;
        }
        /// <summary>
        /// Gets if left == right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public override bool IsEqual(TKey left, TKey right)
        {
            return CompareTo(left, right) == 0;
        }
        /// <summary>
        /// Gets if left == right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe bool IsEqual(TKey left, byte* right)
        {
            return CompareTo(left, right) == 0;
        }
        /// <summary>
        /// Compares Left to Right
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe int CompareTo(TKey left, byte* right)
        {
            Read(right, TempKey);
            return CompareTo(left, TempKey);
        }

        /// <summary>
        /// Compares Left to Right
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe int CompareTo(byte* left, byte* right)
        {
            Read(left, TempKey);
            Read(right, TempKey2);
            return CompareTo(TempKey, TempKey2);
        }

        /// <summary>
        /// Compares Left to Right
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public virtual unsafe int CompareTo(byte* left, TKey right)
        {
            Read(left, TempKey);
            return CompareTo(TempKey, right);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public int Compare(TKey x, TKey y)
        {
            return CompareTo(x, y);
        }

    }
}