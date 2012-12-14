﻿//******************************************************************************************************
//  ArchiveFileSnapshot.cs - Gbtc
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
//  5/22/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using openHistorian.FileStructure;

namespace openHistorian.Server.Database.Archive
{
    /// <summary>
    /// Aquires a read transaction on the current archive partition. This will allow all user created
    /// transactions to have snapshot isolation of the entire data set.
    /// </summary>
    public class ArchiveFileSnapshot
    {
        #region [ Members ]

        TransactionalFileStructure m_fileStructure;
        TransactionalRead m_currentTransaction;

        #endregion

        #region [ Constructors ]

        public ArchiveFileSnapshot(TransactionalFileStructure fileStructure)
        {
            m_fileStructure = fileStructure;
            m_currentTransaction = m_fileStructure.BeginRead();
        }

        #endregion

        #region [ Methods ]
        
        /// <summary>
        /// Opens an instance of the archive file to allow for concurrent reading of a snapshot.
        /// </summary>
        /// <returns></returns>
        public ArchiveFileReadOnlySnapshotInstance OpenInstance()
        {
            return new ArchiveFileReadOnlySnapshotInstance(m_currentTransaction);
        }

        #endregion

    }
}