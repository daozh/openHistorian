﻿//******************************************************************************************************
//  UnmanagedMemoryStream_IoSession.cs - Gbtc
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
//  9/30/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;

namespace GSF.IO.Unmanaged
{
    /// <summary>
    /// Provides a in memory stream that allocates its own unmanaged memory
    /// </summary>
    public partial class UnmanagedMemoryStream
    {
        // Nested Types
        private class IoSession : BinaryStreamIoSessionBase
        {
            private readonly UnmanagedMemoryStream m_stream;

            public IoSession(UnmanagedMemoryStream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException("stream");
                m_stream = stream;
            }

            public override void GetBlock(BlockArguments args)
            {
                args.SupportsWriting = true;
                m_stream.GetBlock(args);
            }
        }
    }
}