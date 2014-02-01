﻿//******************************************************************************************************
//  GetSignalMethods.cs - Gbtc
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
//  12/12/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using GSF.SortedTreeStore;
using GSF.SortedTreeStore.Engine;
using GSF.SortedTreeStore.Engine.Reader;
using GSF.SortedTreeStore.Filters;
using openHistorian.Collections;
using GSF.SortedTreeStore.Tree;
using openHistorian.Data.Types;

namespace openHistorian.Data.Query
{
    /// <summary>
    /// Queries a historian database for a set of signals. 
    /// </summary>
    public static class GetSignalMethods
    {
        /// <summary>
        /// Queries all of the signals at the given time.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="time">the time to query</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, ulong time)
        {
            return database.GetSignals(time, time);
        }

        /// <summary>
        /// Queries all of the signals within a the provided time window [Inclusive]
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startTime">the lower bound of the time</param>
        /// <param name="endTime">the upper bound of the time. [Inclusive]</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, ulong startTime, ulong endTime)
        {
            Dictionary<ulong, SignalDataBase> results = new Dictionary<ulong, SignalDataBase>();

            using (SortedTreeEngineReaderBase<HistorianKey, HistorianValue> reader = database.OpenDataReader())
            {
                TreeStream<HistorianKey, HistorianValue> stream = reader.Read(startTime, endTime);
                ulong time, point, quality, value;
                while (stream.Read())
                {
                    time = stream.CurrentKey.Timestamp;
                    point = stream.CurrentKey.PointID;
                    quality = stream.CurrentValue.Value3;
                    value = stream.CurrentValue.Value1;
                    results.AddSignal(time, point, value);
                }
            }
            foreach (SignalDataBase signal in results.Values)
                signal.Completed();
            return results;
        }

        /// <summary>
        /// Queries the provided signals within a the provided time window [Inclusive]
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startTime">the lower bound of the time</param>
        /// <param name="endTime">the upper bound of the time. [Inclusive]</param>
        /// <param name="signals">an IEnumerable of all of the signals to query as part of the results set.</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, ulong startTime, ulong endTime, IEnumerable<ulong> signals)
        {
            Dictionary<ulong, SignalDataBase> results = signals.ToDictionary((x) => x, (x) => (SignalDataBase)new SignalDataUnknown());

            using (SortedTreeEngineReaderBase<HistorianKey, HistorianValue> reader = database.OpenDataReader())
            {
                TreeStream<HistorianKey, HistorianValue> stream = reader.Read(startTime, endTime, signals);
                ulong time, point, quality, value;
                while (stream.Read())
                {
                    time = stream.CurrentKey.Timestamp;
                    point = stream.CurrentKey.PointID;
                    quality = stream.CurrentValue.Value3;
                    value = stream.CurrentValue.Value1;
                    results.AddSignalIfExists(time, point, value);
                }
            }
            foreach (SignalDataBase signal in results.Values)
                signal.Completed();
            return results;
        }

        /// <summary>
        /// Queries the provided signals within a the provided time window [Inclusive]
        /// This method will strong type the signals, but all signals must be of the same type for this to work.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startTime">the lower bound of the time</param>
        /// <param name="endTime">the upper bound of the time. [Inclusive]</param>
        /// <param name="signals">an IEnumerable of all of the signals to query as part of the results set.</param>
        /// <param name="conversion">a single conversion method to use for all signals</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, ulong startTime, ulong endTime, IEnumerable<ulong> signals, TypeBase conversion)
        {
            Dictionary<ulong, SignalDataBase> results = signals.ToDictionary((x) => x, (x) => (SignalDataBase)new SignalData(conversion));

            using (SortedTreeEngineReaderBase<HistorianKey, HistorianValue> reader = database.OpenDataReader())
            {
                TreeStream<HistorianKey, HistorianValue> stream = reader.Read(startTime, endTime, signals);
                ulong time, point, quality, value;
                while (stream.Read())
                {
                    time = stream.CurrentKey.Timestamp;
                    point = stream.CurrentKey.PointID;
                    quality = stream.CurrentValue.Value3;
                    value = stream.CurrentValue.Value1;
                    results.AddSignalIfExists(time, point, value);
                }
            }
            foreach (SignalDataBase signal in results.Values)
                signal.Completed();
            return results;
        }

        /// <summary>
        /// Queries the provided signals within a the provided time window [Inclusive].
        /// With this method, the signals will be strong typed and therefore can be converted.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startTime">the lower bound of the time</param>
        /// <param name="endTime">the upper bound of the time. [Inclusive]</param>
        /// <param name="signals">an IEnumerable of all of the signals to query as part of the results set.</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, ulong startTime, ulong endTime, IEnumerable<ISignalWithType> signals)
        {
            return database.GetSignals(TimestampFilter.CreateFromRange<HistorianKey>(startTime, endTime), signals, SortedTreeEngineReaderOptions.Default);
        }

        /// <summary>
        /// Queries the provided signals within a the time described by the <see cref="QueryFilterTimestamp"/>.
        /// With this method, the signals will be strong typed and therefore can be converted.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="timestamps">a <see cref="QueryFilterTimestamp"/> that describes how a signal will be parsed</param>
        /// <param name="signals">an IEnumerable of all of the signals to query as part of the results set.</param>
        /// <param name="readerOptions">The options that will be used when querying this data.</param>
        /// <returns></returns>
        public static Dictionary<ulong, SignalDataBase> GetSignals(this SortedTreeEngineBase<HistorianKey, HistorianValue> database, KeySeekFilterBase<HistorianKey> timestamps, IEnumerable<ISignalWithType> signals, SortedTreeEngineReaderOptions readerOptions)
        {
            Dictionary<ulong, SignalDataBase> results = new Dictionary<ulong, SignalDataBase>();

            foreach (ISignalWithType point in signals)
            {
                if (point.HistorianId.HasValue)
                {
                    if (!results.ContainsKey(point.HistorianId.Value))
                    {
                        results.Add(point.HistorianId.Value, new SignalData(point.Functions));
                    }
                }
            }

            using (SortedTreeEngineReaderBase<HistorianKey, HistorianValue> reader = database.OpenDataReader())
            {
                var keyParser = PointIDFilter.CreateFromList<HistorianKey>(signals.Where((x) => x.HistorianId.HasValue).Select((x) => x.HistorianId.Value));
                TreeStream<HistorianKey, HistorianValue> stream = reader.Read(readerOptions, timestamps, keyParser, null);
                ulong time, point, quality, value;
                while (stream.Read())
                {
                    time = stream.CurrentKey.Timestamp;
                    point = stream.CurrentKey.PointID;
                    quality = stream.CurrentValue.Value3;
                    value = stream.CurrentValue.Value1;
                    results.AddSignalIfExists(time, point, value);
                }
            }

            foreach (SignalDataBase signal in results.Values)
            {
                signal.Completed();
            }
            return results;
        }

        /// <summary>
        /// Adds the following signal to the dictionary. If the signal is
        /// not part of the dictionary, it is added automatically.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="time"></param>
        /// <param name="point"></param>
        /// <param name="value"></param>
        private static void AddSignal(this Dictionary<ulong, SignalDataBase> results, ulong time, ulong point, ulong value)
        {
            SignalDataBase signalData;
            if (!results.TryGetValue(point, out signalData))
            {
                signalData = new SignalDataUnknown();
                results.Add(point, signalData);
            }
            signalData.AddDataRaw(time, value);
        }

        /// <summary>
        /// Adds the provided signal to the dictionary unless the signal is not
        /// already part of the dictionary.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="time"></param>
        /// <param name="point"></param>
        /// <param name="value"></param>
        private static void AddSignalIfExists(this Dictionary<ulong, SignalDataBase> results, ulong time, ulong point, ulong value)
        {
            SignalDataBase signalData;
            if (results.TryGetValue(point, out signalData))
                signalData.AddDataRaw(time, value);
        }
    }
}