﻿//******************************************************************************************************
//  ArchiveWriter.cs - Gbtc
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
//  5/29/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.Threading;
using openHistorian.V2.IO.Unmanaged;
using openHistorian.V2.Server.Configuration;
using openHistorian.V2.Server.Database.Archive;

namespace openHistorian.V2.Server.Database
{
    /// <summary>
    /// Responsible for getting data into the database. This class will prebuffer
    /// points and commit them in bulk operations.
    /// </summary>
    public class ArchiveWriter : IDisposable
    {
        ArchiveWriterSettings m_settings;

        ArchiveInitializer m_archiveInitializer;

        volatile bool m_disposed;

        ArchiveList m_archiveList;

        ArchiveFile m_activeFile;

        PointQueue m_pointQueue;

        Thread m_insertThread;

        ManualResetEvent m_waitTimer;

        int m_commitCount;

        Stopwatch m_lastCommitTime;

        /// <summary>
        /// Creates a new <see cref="ArchiveWriter"/>.
        /// </summary>
        /// <param name="settings">The settings for this class.</param>
        /// <param name="archiveList">The list used to attach newly created file.</param>
        public ArchiveWriter(ArchiveWriterSettings settings, ArchiveList archiveList)
        {
            if (!settings.IsReadOnly)
                throw new ArgumentException("Must be set to read only before passing to this function", "settings");

            m_archiveInitializer = new ArchiveInitializer(settings.Initializer);

            m_settings = settings;

            m_archiveList = archiveList;
            
            if (settings.CommitOnPointCount.HasValue)
                m_pointQueue = new PointQueue(settings.CommitOnPointCount.Value, SignalInitialInsert);
            else
                m_pointQueue = new PointQueue();

            m_lastCommitTime = Stopwatch.StartNew();

            m_waitTimer = new ManualResetEvent(false);
            m_insertThread = new Thread(ProcessInsertingData);
            m_insertThread.Start();
        }

        /// <summary>
        /// This is executed by a dedicated thread and moves data from the point queue to the database.
        /// </summary>
        void ProcessInsertingData()
        {
            while (!m_disposed)
            {
                if (m_settings.CommitOnInterval.HasValue)
                    m_waitTimer.WaitOne(m_settings.CommitOnInterval.Value);
                else
                    m_waitTimer.WaitOne(10000);

                m_waitTimer.Reset();

                BinaryStream stream;
                int pointCount;
                m_pointQueue.GetPointBlock(out stream, out pointCount);
                if (pointCount > 0)
                {
                    if (m_activeFile == null ||
                        (m_settings.NewFileOnCommitCount.HasValue && m_commitCount >= m_settings.NewFileOnCommitCount.Value) ||
                        (m_settings.NewFileOnInterval.HasValue && m_lastCommitTime.Elapsed >= m_settings.NewFileOnInterval.Value) ||
                        (m_settings.NewFileOnSize.HasValue && m_activeFile.FileSize >= m_settings.NewFileOnSize.Value))
                    {
                        m_commitCount = 0;
                        m_lastCommitTime.Restart();
                        var newFile = m_archiveInitializer.CreateArchiveFile();
                        using (var edit = m_archiveList.AcquireEditLock())
                        {
                            //Create a new file.
                            if (m_activeFile != null)
                            {
                                edit.ReleaseEditLock(m_activeFile);
                            }
                            edit.Add(newFile, new ArchiveFileStateInformation(false, true, m_settings.DestinationName));
                        }
                        m_activeFile = newFile;
                    }
                    m_activeFile.BeginEdit();
                    while (pointCount > 0)
                    {
                        pointCount--;

                        ulong time = stream.ReadUInt64();
                        ulong id = stream.ReadUInt64();
                        ulong flags = stream.ReadUInt64();
                        ulong value = stream.ReadUInt64();

                        m_activeFile.AddPoint(time, id, flags, value);
                    }
                    m_commitCount++;
                    m_activeFile.CommitEdit();
                    using (var editor = m_archiveList.AcquireEditLock())
                    {
                        editor.RenewSnapshot(m_activeFile);
                    }
                }
            }
        }

        public void WriteData(ulong key1, ulong key2, ulong value1, ulong value2)
        {
            m_pointQueue.WriteData(key1, key2, value1, value2);
        }

        /// <summary>
        /// Moves data from the queue and inserts it into Generation 0's Archive.
        /// </summary>
        public void SignalInitialInsert()
        {
            m_waitTimer.Set();
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                SignalInitialInsert();
            }
        }



    }
}