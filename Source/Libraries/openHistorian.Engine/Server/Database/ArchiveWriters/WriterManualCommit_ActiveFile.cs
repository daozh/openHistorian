﻿//******************************************************************************************************
//  WriterManualCommit_ActiveFile.cs - Gbtc
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
using openHistorian.IO.Unmanaged;
using openHistorian.Server.Database.Archive;

namespace openHistorian.Server.Database.ArchiveWriters
{
    /// <summary>
    /// Responsible for getting data into the database. This class will prebuffer
    /// points and commit them in bulk operations.
    /// </summary>
    public partial class WriterManualCommit
    {
        internal class ActiveFile
        {
            ArchiveList m_archiveList;
            ArchiveFile m_archiveFile;
            ArchiveFile.ArchiveFileEditor m_editor;
            Action<ArchiveFile, long> m_callbackFileComplete;

            public ActiveFile(ArchiveList archiveList, Action<ArchiveFile, long> callbackFileComplete)
            {
                m_archiveList = archiveList;
                m_callbackFileComplete = callbackFileComplete;
            }

            public void CreateIfNotExists()
            {
                if (m_archiveFile == null)
                {
                    m_archiveFile = ArchiveFile.CreateInMemory();
                    using (var edit = m_archiveList.AcquireEditLock())
                    {
                        //Add the newly created file.
                        edit.Add(m_archiveFile, true);
                    }
                    m_editor = m_archiveFile.BeginEdit();
                }
            }

            public void Append(ulong key1, ulong key2, ulong value1, ulong value2)
            {
                m_editor.AddPoint(key1, key2, value1, value2);
            }

            public void RefreshSnapshot()
            {
                if (m_archiveFile == null)
                    return;
                m_editor.Commit();
                m_editor.Dispose();
                using (var edit = m_archiveList.AcquireEditLock())
                {
                    edit.RenewSnapshot(m_archiveFile);
                }
                m_editor = m_archiveFile.BeginEdit();

            }

            public void RefreshAndRolloverFile(long sequenceId)
            {
                if (m_archiveFile == null)
                    return;
                m_editor.Commit();
                m_editor.Dispose();
                using (var edit = m_archiveList.AcquireEditLock())
                {
                    edit.RenewSnapshot(m_archiveFile);
                }
                m_callbackFileComplete(m_archiveFile, sequenceId);
                m_archiveFile = null;
                m_editor = null;
            }
        }
    }
}