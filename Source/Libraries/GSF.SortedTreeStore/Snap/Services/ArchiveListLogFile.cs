﻿//******************************************************************************************************
//  ArchiveListLogFile.cs - Gbtc
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
//  10/02/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using GSF.Diagnostics;
using GSF.IO;

namespace GSF.Snap.Services
{
    /// <summary>
    /// A individual archive list log file
    /// </summary>
    internal class ArchiveListLogFile
    {
        private static readonly LogType Log = Logger.LookupType(typeof(ArchiveListLogFile));

        /// <summary>
        /// Gets if the file is valid and not corrupt.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the list of all files that are pending deletion.
        /// </summary>
        public readonly List<Guid> FilesToDelete = new List<Guid>();

        /// <summary>
        /// Gets the filename of this log file. String.Empty if not currently associated with a file.
        /// </summary>
        public string FileName = string.Empty;

        public ArchiveListLogFile()
        {
            IsValid = true;
        }

        public ArchiveListLogFile(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Removes any files that have already been deleted from this log file.
        /// </summary>
        /// <remarks>
        /// Note, the log file should not be modified to prevent corrupting the log file.
        /// </remarks>
        public void RemoveDeletedFiles(HashSet<Guid> allFiles)
        {
            for (int x = FilesToDelete.Count - 1; x >= 0; x--)
            {
                if (!allFiles.Contains(FilesToDelete[x]))
                {
                    FilesToDelete.RemoveAt(x);
                }
            }
        }

        /// <summary>
        /// Loads from the disk
        /// </summary>
        /// <param name="fileName">the name of the log file</param>
        public void Load(string fileName)
        {
            FileName = fileName;
            FilesToDelete.Clear();
            IsValid = false;

            try
            {
                byte[] data = File.ReadAllBytes(fileName);
                if (data.Length < Header.Length + 1 + 20) //Header + Version + SHA1
                {
                    Log.Publish(VerboseLevel.Warning, "Failed to load file.", "Expected file lenth is not long enough");
                    return;
                }
                for (int x = 0; x < Header.Length; x++)
                {
                    if (data[x] != Header[x])
                    {
                        Log.Publish(VerboseLevel.Warning, "Failed to load file.", "Incorrect File Header");
                        return;
                    }
                }

                byte[] hash = new byte[20];
                Array.Copy(data, data.Length - 20, hash, 0, 20);
                using (var sha = new SHA1Managed())
                {
                    var checksum = sha.ComputeHash(data, 0, data.Length - 20);
                    if (!hash.SequenceEqual(checksum))
                    {
                        Log.Publish(VerboseLevel.Warning, "Failed to load file.", "Hashsum failed.");
                        return;
                    }
                }

                var stream = new MemoryStream(data);

                stream.Position = Header.Length;

                int version = stream.ReadNextByte();
                switch (version)
                {
                    case 1:
                        int count = stream.ReadInt32();
                        while (count > 0)
                        {
                            count--;
                            FilesToDelete.Add(stream.ReadGuid());
                        }
                        IsValid = true;
                        return;
                    default:
                        Log.Publish(VerboseLevel.Warning, "Failed to load file.", "Version Not Recgonized.");
                        FilesToDelete.Clear();
                        return;
                }
            }
            catch (Exception ex)
            {
                Log.Publish(VerboseLevel.Warning, "Failed to load file.", "Unexpected Error", null, ex);
                FilesToDelete.Clear();
                return;
            }
        }

        /// <summary>
        /// Saves to the disk
        /// </summary>
        public void Save(string fileName)
        {
            var stream = new MemoryStream();
            stream.Write(Header);
            stream.Write((byte)1);
            stream.Write(FilesToDelete.Count);
            foreach (var file in FilesToDelete)
            {
                stream.Write(file);
            }
            using (var sha = new SHA1Managed())
            {
                stream.Write(sha.ComputeHash(stream.ToArray()));
            }
            File.WriteAllBytes(fileName, stream.ToArray());
            FileName = fileName;
        }

        /// <summary>
        /// Deletes the file associated with this ArchiveLog
        /// </summary>
        public void Delete()
        {
            try
            {
                FilesToDelete.Clear();
                IsValid = false;
                if (FileName != string.Empty)
                    File.Delete(FileName);
                FileName = string.Empty;
            }
            catch (Exception ex)
            {
                Log.Publish(VerboseLevel.Error, "Could not delete file: " + FileName, null, null, ex);
            }
        }

        static readonly byte[] Header;
        static ArchiveListLogFile()
        {
            Header = System.Text.Encoding.UTF8.GetBytes("openHistorian 2.0 Archive List Log");
        }

    }
}
