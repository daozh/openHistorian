﻿//******************************************************************************************************
//  TimeSingleMeasurement.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openVisN
{
    public class TimeSingleMeasurement : IMeasurement
    {
        List<KeyValuePair<ulong, ulong>> m_data;

        public TimeSingleMeasurement(List<KeyValuePair<ulong, ulong>> data)
        {
            m_data = data;
        }

        public int PointCounts
        {
            get
            {
                return m_data.Count;
            }
        }

        public int[] LineBreaks
        {
            get
            {
                return new int[0];
            }
        }

        public unsafe KeyValuePair<double, double> this[int index]
        {
            get
            {
                var kvp = m_data[index];
                ulong yvalue = kvp.Value;
                return new KeyValuePair<double, double>((double)kvp.Key, (double)*(float*)&yvalue);
            }
        }
    }
}