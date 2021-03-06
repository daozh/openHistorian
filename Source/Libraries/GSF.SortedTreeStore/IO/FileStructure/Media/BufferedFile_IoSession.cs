﻿//******************************************************************************************************
//  BufferedFile_IoStream.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  02/01/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using GSF.Diagnostics;
using GSF.IO.Unmanaged;

namespace GSF.IO.FileStructure.Media
{
    internal partial class BufferedFile
    {
        /// <summary>
        /// The <see cref="BinaryStreamIoSessionBase"/> utilized by the <see cref="BufferedFile"/>.
        /// </summary>
        private class IoSession
            : PageReplacementAlgorithm.PageLock
        {
            private static readonly LogType Log = Logger.LookupType(typeof(IoSession));

            /// <summary>
            /// The base stream
            /// </summary>
            private readonly BufferedFile m_stream;

            /// <summary>
            /// Creates a new <see cref="IoSession"/>
            /// </summary>
            /// <param name="stream">the base class</param>
            /// <param name="pageReplacement">The page Replacement Algorithm</param>
            internal IoSession(BufferedFile stream, PageReplacementAlgorithm pageReplacement)
                : base(pageReplacement)
            {
                m_stream = stream;
            }

#if DEBUG
            /// <summary>
            /// Releases the unmanaged resources before the <see cref="IoSession"/> object is reclaimed by <see cref="GC"/>.
            /// </summary>
            ~IoSession()
            {
                Log.Publish(VerboseLevel.Information, "Finalizer Called", GetType().FullName);
                Dispose(false);
            }
#endif

            /// <summary>
            /// Gets a block for the following Io session.
            /// </summary>
            /// <param name="args">the <see cref="BlockArguments"/> to use to read and write to a block</param>
            public override void GetBlock(BlockArguments args)
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                m_stream.GetBlock(this, args);
            }
        }
    }
}