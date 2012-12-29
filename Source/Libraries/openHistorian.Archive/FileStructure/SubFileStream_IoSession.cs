﻿//******************************************************************************************************
//  SubFileStream_IoSession.cs - Gbtc
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
//  6/15/2012 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using openHistorian.IO.Unmanaged;

namespace openHistorian.FileStructure
{
    unsafe partial class SubFileStream
    {
        class IoSession : IBinaryStreamIoSession
        {
            #region [ Members ]

            bool m_disposed;

            /// <summary>
            /// The translation information for the most recent block looked up.
            /// </summary>
            PositionData m_positionBlock = default(PositionData);

            /// <summary>
            /// Used to convert physical addresses into virtual addresses.
            /// </summary>
            SubFileAddressTranslation m_addressTranslation;

            SubFileStream m_stream;

            /// <summary>
            /// Contains the read/write buffer.
            /// </summary>
            DiskIoSession m_buffer;

            int m_blockDataLength;

            #endregion

            #region [ Constructors ]

            public IoSession(int blockSize, SubFileStream stream)
            {
                m_blockDataLength = blockSize - FileStructureConstants.BlockFooterLength;
                m_stream = stream;
                m_addressTranslation = new SubFileAddressTranslation(blockSize, stream.m_subFile, stream.m_dataReader, stream.m_fileHeaderBlock, stream.m_isReadOnly ? AccessMode.ReadOnly : AccessMode.ReadWrite);
                m_buffer = stream.m_dataReader.CreateDiskIoSession();
            }

            #endregion

            #region [ Properties ]

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
            /// Releases all the resources used by the <see cref="IoSession"/> object.
            /// </summary>
            public void Dispose()
            {
                if (!m_disposed)
                {
                    try
                    {
                        EndPendingWrites();
                        if (m_buffer != null)
                        {
                            m_buffer.Dispose();
                            m_buffer = null;
                        }
                        if (m_addressTranslation != null)
                            m_addressTranslation .Dispose();
                        m_addressTranslation = null;
                    }
                    finally
                    {
                        m_addressTranslation = null;
                        m_disposed = true;  // Prevent duplicate dispose.
                    }
                }
            }

            public void Clear()
            {
                if (m_disposed || m_stream.IsDisposed || m_buffer.IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                EndPendingWrites();
                m_buffer.Clear();
            }

            /// <summary>
            /// Completes any pending writes to the file system.
            /// </summary>
            void EndPendingWrites()
            {
                if (m_buffer.IsValid && m_buffer.IsPendingWriteComplete)
                {
                    int indexValue = (int)(m_positionBlock.VirtualPosition / m_blockDataLength);
                    int fileIdNumber = m_stream.m_subFile.FileIdNumber;
                    int snapshotSequenceNumber = m_stream.m_fileHeaderBlock.SnapshotSequenceNumber;
                    m_buffer.EndWrite(BlockType.DataBlock, indexValue, fileIdNumber, snapshotSequenceNumber);
                }
            }

            /// <summary>
            /// Looks up the position data and prepares the current block to be written to.
            /// </summary>
            void PrepareBlockForRead(long position)
            {
                if (!m_positionBlock.Containts(position) || !m_buffer.IsValid)
                {
                    EndPendingWrites();
                    m_positionBlock = m_addressTranslation.VirtualToPhysical(position);
                    if (m_positionBlock.PhysicalBlockIndex == 0)
                        throw new Exception("Failure to shadow copy the page.");
                    int indexValue = (int)(m_positionBlock.VirtualPosition / m_blockDataLength);
                    int featureSequenceNumber = m_stream.m_subFile.FileIdNumber;
                    int revisionSequenceNumber = m_stream.m_fileHeaderBlock.SnapshotSequenceNumber;
                    m_buffer.Read(m_positionBlock.PhysicalBlockIndex, BlockType.DataBlock, indexValue, featureSequenceNumber, revisionSequenceNumber);
                }
            }

            /// <summary>
            /// Looks up the position data and prepares the current block to be written to.
            /// </summary>
            void PrepareBlockForWrite(long position)
            {
                if (!m_positionBlock.Containts(position) || m_positionBlock.PhysicalBlockIndex <= m_stream.m_lastReadOnlyBlock || !m_buffer.IsValid || m_buffer.IsReadOnly)
                {
                    EndPendingWrites();
                    m_positionBlock = m_addressTranslation.VirtualToShadowPagePhysical(position);
                    if (m_positionBlock.PhysicalBlockIndex == 0)
                        throw new Exception("Failure to shadow copy the page.");
                    int indexValue = (int)(m_positionBlock.VirtualPosition / m_blockDataLength);
                    int featureSequenceNumber = m_stream.m_subFile.FileIdNumber;
                    int revisionSequenceNumber = m_stream.m_fileHeaderBlock.SnapshotSequenceNumber;
                    m_buffer.BeginWriteToExistingBlock(m_positionBlock.PhysicalBlockIndex, BlockType.DataBlock, indexValue, featureSequenceNumber, revisionSequenceNumber);
                }
            }

            public void GetBlock(long position, bool isWriting, out IntPtr firstPointer, out long firstPosition, out int length, out bool supportsWriting)
            {
                if (m_disposed || m_stream.IsDisposed || m_buffer.IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (isWriting)
                {
                    if (m_stream.m_isReadOnly)
                        throw new Exception("File is read only");

                    PrepareBlockForWrite(position);
                    firstPosition = m_positionBlock.VirtualPosition;
                    length = (int)m_positionBlock.Length;
                    firstPointer = (IntPtr)m_buffer.Pointer;
                    supportsWriting = m_buffer.IsPendingWriteComplete;
                }
                else
                {
                    PrepareBlockForRead(position);
                    firstPosition = m_positionBlock.VirtualPosition;
                    length = (int)m_positionBlock.Length;
                    firstPointer = (IntPtr)m_buffer.Pointer;
                    supportsWriting = m_buffer.IsPendingWriteComplete;
                }
            }
            #endregion

        }
    }
}