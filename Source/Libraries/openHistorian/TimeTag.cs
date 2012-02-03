//******************************************************************************************************
//  TimeTag.cs - Gbtc
//
//  Tennessee Valley Authority
//  No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  05/03/2006 - J. Ritchie Carroll
//       Generated original version of code based on DatAWare system specifications by Brian B. Fox, TVA.
//  07/12/2006 - J. Ritchie Carroll
//       Modified class to be derived from new "TimeTagBase" class.
//  04/20/2009 - Pinal C. Patel
//       Converted to C#.
//  09/02/2009 - Pinal C. Patel
//       Added Parse() static method to allow conversion of string to TimeTag.
//  09/15/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  09/25/2009 - Pinal C. Patel
//       Added overloaded constructor that take ticks.
//       Added Now and UtcNow static properties for ease-of-use.
//
//******************************************************************************************************

using System;
using TVA;

namespace openHistorian
{
    /// <summary>
    /// Represents a historian time tag as number of seconds from the <see cref="BaseDate"/>.
    /// </summary>
    public class TimeTag : TimeTagBase
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTag"/> class.
        /// </summary>
        /// <param name="ticks">Number of ticks since the <see cref="BaseDate"/>.</param>
        public TimeTag(long ticks)
            : base(BaseDate.Ticks, Ticks.ToSeconds(ticks))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTag"/> class.
        /// </summary>
        /// <param name="seconds">Number of seconds since the <see cref="BaseDate"/>.</param>
        public TimeTag(double seconds)
            : base(BaseDate.Ticks, seconds)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTag"/> class.
        /// </summary>
        /// <param name="timestamp"><see cref="DateTime"/> value on or past the <see cref="BaseDate"/>.</param>
        public TimeTag(DateTime timestamp)
            : base(BaseDate.Ticks, timestamp)
        {
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns the default text representation of <see cref="TimeTag"/>.
        /// </summary>
        /// <returns><see cref="string"/> that represents <see cref="TimeTag"/>.</returns>
        public override string ToString()
        {
            return ToString("dd-MMM-yyyy HH:mm:ss.fff");
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Represents the smallest possible value of <see cref="TimeTag"/>.
        /// </summary>
        public static readonly TimeTag MinValue;

        /// <summary>
        /// Represents the largest possible value of <see cref="TimeTag"/>.
        /// </summary>
        public static readonly TimeTag MaxValue;

        /// <summary>
        /// Represents the base <see cref="DateTime"/> (01/01/1995) for <see cref="TimeTag"/>.
        /// </summary>
        public static readonly DateTime BaseDate;

        // Static Constructor

        static TimeTag()
        {
            BaseDate = new DateTime(1995, 1, 1, 0, 0, 0);
            MinValue = new TimeTag(0.0);
            MaxValue = new TimeTag(2147483647.999);
        }

        // Static Properties

        /// <summary>
        /// Gets a <see cref="TimeTag"/> object that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        public static TimeTag Now
        {
            get 
            {
                return new TimeTag(PrecisionTimer.Now.Ticks - BaseDate.Ticks);
            }
        }

        /// <summary>
        /// Gets a <see cref="TimeTag"/> object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public static TimeTag UtcNow
        {
            get
            {
                return new TimeTag(PrecisionTimer.UtcNow.Ticks - BaseDate.Ticks);
            }
        }

        // Static Methods

        /// <summary>
        /// Converts the specified string representation of a date and time to its <see cref="TimeTag"/> equivalent.
        /// </summary>
        /// <param name="timetag">A string containing the date and time to convert.</param>
        /// <returns>A <see cref="TimeTag"/> object.</returns>
        /// <remarks>
        /// <paramref name="timetag"/> can be specified in one of the following format:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Time Format</term>
        ///         <description>Format Description</description>
        ///     </listheader>
        ///     <item>
        ///         <term>12-30-2000 23:59:59</term>
        ///         <description>Absolute date and time.</description>
        ///     </item>
        ///     <item>
        ///         <term>*</term>
        ///         <description>Evaluates to <see cref="DateTime.UtcNow"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>*-20s</term>
        ///         <description>Evaluates to 20 seconds before <see cref="DateTime.UtcNow"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>*-10m</term>
        ///         <description>Evaluates to 10 minutes before <see cref="DateTime.UtcNow"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>*-1h</term>
        ///         <description>Evaluates to 1 hour before <see cref="DateTime.UtcNow"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term>*-1d</term>
        ///         <description>Evaluates to 1 day before <see cref="DateTime.UtcNow"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static TimeTag Parse(string timetag)
        {
            DateTime dateTime;
            timetag = timetag.ToLower();
            if (timetag.Contains("*"))
            {
                // Relative time is specified.
                // Examples:
                // 1) * (Now)
                // 2) *-20s (20 seconds ago)
                // 3) *-10m (10 minutes ago)
                // 4) *-1h (1 hour ago)
                // 5) *-1d (1 day ago)
                dateTime = DateTime.UtcNow;
                if (timetag.Length > 1)
                {
                    string unit = timetag.Substring(timetag.Length - 1);
                    int adjustment = int.Parse(timetag.Substring(1, timetag.Length - 2));
                    switch (unit)
                    {
                        case "s":
                            dateTime = dateTime.AddSeconds(adjustment);
                            break;
                        case "m":
                            dateTime = dateTime.AddMinutes(adjustment);
                            break;
                        case "h":
                            dateTime = dateTime.AddHours(adjustment);
                            break;
                        case "d":
                            dateTime = dateTime.AddDays(adjustment);
                            break;
                    }
                }
            }
            else
            {
                // Absolute time is specified.
                dateTime = DateTime.Parse(timetag);
            }

            return new TimeTag(dateTime);
        }

        #endregion
    }
}