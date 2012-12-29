﻿//******************************************************************************************************
//  IHistorianDatabase.cs - Gbtc
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
//  12/8/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;

namespace openHistorian
{

    /// <summary>
    /// Represents a single historian database.
    /// </summary>
    public interface IHistorianDatabase
    {
        /// <summary>
        /// Opens a stream connection that can be used to read 
        /// and write data to the current historian database.
        /// </summary>
        /// <param name="timeout">the duration in milliseconds to wait before prematurely canceling the read.
        /// A value of zero means there is no timeout.</param>
        /// <returns></returns>
        IHistorianDataReader OpenDataReader(long timeout = 0);

        /// <summary>
        /// Writes the point stream to the database. 
        /// </summary>
        /// <param name="points"></param>
        void Write(IPointStream points);
        
        /// <summary>
        /// Writes an individual point to the database.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        void Write(ulong key1, ulong key2, ulong value1, ulong value2);
        
        /// <summary>
        /// Forces a soft commit on the database. A soft commit 
        /// only commits data to memory. This allows other clients to read the data.
        /// While soft committed, this data could be lost during an unexpected shutdown.
        /// Soft commits usually occur within microseconds. 
        /// </summary>
        void SoftCommit();

        /// <summary>
        /// Forces a commit to the disk subsystem. Once this returns, the data will not
        /// be lost due to an application crash or unexpected shutdown.
        /// Hard commits can take 100ms or longer depending on how much data has to be committed. 
        /// This requires two consecutive hardware cache flushes.
        ///  </summary>
        void HardCommit();

        /// <summary>
        /// Disconnects from the current database. 
        /// </summary>
        void Disconnect();

    }
}