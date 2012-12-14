﻿//******************************************************************************************************
//  SortedTree256.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  4/5/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//     
//******************************************************************************************************

using System;
using System.Collections.Generic;
using openHistorian.IO;

namespace openHistorian.Collections.KeyValue
{
    /// <summary>
    /// Represents a collection of 128-bit key/128-bit values pairs that is very similiar to a <see cref="SortedList{int128,int128}"/> 
    /// except it is optimal for storing millions to billions of entries and doing sequential scan of the data.
    /// </summary>
    public class SortedTree256 : SortedTree256LeafNodeBase
    {
        // {C4BB5945-A6DA-4634-A8A9-F74C5FB4A052}
        static Guid s_fileType = new Guid(0xc4bb5945, 0xa6da, 0x4634, 0xa8, 0xa9, 0xf7, 0x4c, 0x5f, 0xb4, 0xa0, 0x52);
        public static Guid GetFileType()
        {
            return s_fileType;
        }
        /// <summary>
        /// Loads an existing <see cref="SortedTree256"/>
        /// from the provided stream.
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        public SortedTree256(BinaryStreamBase stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Creates an empty <see cref="SortedTree256"/> 
        /// and writes the data to the provided stream. 
        /// </summary>
        /// <param name="stream">The stream to use to store the tree.</param>
        /// <param name="blockSize">The size in bytes of a single block.</param>
        public SortedTree256(BinaryStreamBase stream, int blockSize)
            : base(stream, blockSize)
        {
        }

        protected override Guid FileType
        {
            get
            {
                return s_fileType;
            }
        }
    }
}