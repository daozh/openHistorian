﻿//******************************************************************************************************
//  ArchiveFileReadOnlySnapshotInstance.cs - Gbtc
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
//  5/22/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using openHistorian.Collections.Generic;
using openHistorian.FileStructure;

namespace openHistorian.Archive
{
    /// <summary>
    /// Provides a user with a read-only instance of an archive.
    /// This class is not thread safe.
    /// </summary>
    public class ArchiveTableReadSnapshot<TKey, TValue>
        : IDisposable
        where TKey : class, new()
        where TValue : class, new()
    {
        #region [ Members ]

        private bool m_disposed;
        private SortedTreeContainer<TKey, TValue> m_dataTree;

        #endregion

        #region [ Constructors ]

        internal ArchiveTableReadSnapshot(TransactionalRead currentTransaction, SubFileName fileName)
        {
            m_dataTree = new SortedTreeContainer<TKey, TValue>(currentTransaction, fileName);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Determines if this read snapshot has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return m_disposed;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets a reader that can be used to parse an archive file.
        /// </summary>
        /// <returns></returns>
        public TreeScannerBase<TKey, TValue> GetTreeScanner()
        {
            return m_dataTree.CreateTreeScanner();
        }
        /// <summary>
        /// Returns the lower and upper bounds of the tree
        /// </summary>
        /// <param name="lowerBounds">the first key in the tree</param>
        /// <param name="upperBounds">the last key in the tree</param>
        /// <remarks>
        /// If the tree is empty, lowerBounds will be greater than upperBounds</remarks>
        public void GetKeyRange(TKey lowerBounds, TKey upperBounds)
        {
            m_dataTree.GetKeyRange(lowerBounds, upperBounds);
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if (m_dataTree != null)
                    {
                        m_dataTree.Dispose();
                        m_dataTree = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        #endregion
    }
}