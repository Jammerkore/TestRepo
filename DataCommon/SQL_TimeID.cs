using System;
using System.Globalization;
using System.Data;


namespace MIDRetail.DataCommon
{
    /// <summary>
    /// SQL Time ID 
    /// </summary>
    [Serializable]
    public struct SQL_TimeID : IComparable<SQL_TimeID>
    {
        char _timeType;
        Int16 _timeID;
        /// <summary>
        /// Creates an instance of this structure using a TimeID key from database
        /// </summary>
        /// <param name="aSQL_TimeID">TimeID from database</param>
        public SQL_TimeID(char aSQL_TimeType, Int16 aSQL_TimeID)
        {
            _timeType = aSQL_TimeType;
            _timeID = aSQL_TimeID;
        }

        /// <summary>
        /// Creates an instance of this structure
        /// </summary>
        /// <param name="aTimeIdType">Time Id type</param>
        /// <param name="aTimeID">Time ID in format YYYYDDD; for weekly time types, this should be first day of week</param>
        public SQL_TimeID(eSQLTimeIdType aTimeIdType, int aTimeID)
        {
            if (aTimeID < 1000000
                || aTimeID > 9999999)
            {
                throw new Exception("Time ID must have format YYYYDDD");
            }

            _timeID = 0;  // Corrsponds to January 1, 1970;
            Int16 givenYear = (Int16)(aTimeID / 1000);
            if (givenYear < 1970)
            {

				//Begin TT#1069 - JScott - Calendar does not automatically expand correctly
				//for (int year = 1969; year > givenYear; year++)
				for (int year = 1969; year > givenYear; year--)
				//End TT#1069 - JScott - Calendar does not automatically expand correctly
				{
                    if (DateTime.IsLeapYear(year))
                    {
                        _timeID -= 366;
                    }
                    else
                    {
                        _timeID -= 365;
                    }
                }
                if (DateTime.IsLeapYear(givenYear))
                {
                    _timeID -= (Int16)(366 - (aTimeID - givenYear * 1000));
                }
                else
                {
                    _timeID -= (Int16)(365 - (aTimeID - givenYear * 1000));
                }
            }
            else
            {
                for (int year = 1970; year < givenYear; year++)
                {
                    if (DateTime.IsLeapYear(year))
                    {
                        _timeID += 366;
                    }
                    else
                    {
                        _timeID += 365;
                    }
                }
                _timeID += (Int16)(aTimeID - givenYear * 1000);
            }


            if (aTimeIdType == eSQLTimeIdType.TimeIdIsWeekly)
            {
                _timeType = 'W';
            }
            else
            {
                _timeType = 'D';
            }
        }
        /// <summary>
        /// Gets the Time ID Type
        /// </summary>
        public eSQLTimeIdType TimeIdType
        {
            get
            {
                if (_timeType == 'W')
                {
                    return eSQLTimeIdType.TimeIdIsWeekly;
                }
                return eSQLTimeIdType.TimeIdIsDaily;
            }
        }
        /// <summary>
        /// Gets the TimeID in format YYYYDDD
        /// </summary>
        public int TimeID
        {
            get
            {
                int timeIDyear = 1970;
                int timeIDday = 1;
                Int16 dayCount = _timeID;
                if (_timeID < 1)
                {
                    timeIDyear = 1969;
                    timeIDday = 365;
                    while (dayCount < 1)
                    {
                        if (DateTime.IsLeapYear(timeIDyear))
                        {
                            if (dayCount < -366)
                            {
                                dayCount += 366;
                                timeIDyear--;
                            }
                            else
                            {
                                timeIDday = 366 + dayCount;
                                dayCount = 1;
                            }
                        }
                        else
                        {
                            if (dayCount < -365)
                            {
                                dayCount += 365;
                                timeIDyear--;
                            }
                            else
                            {
                                timeIDday = 365 + dayCount;
                                dayCount = 1;
                            }
                        }
                    }
                }
                else
                {
                    while (dayCount > 0)
                    {
                        if (DateTime.IsLeapYear(timeIDyear))
                        {
                            if (dayCount > 366)
                            {
                                dayCount -= 366;
                                timeIDyear++;
                            }
                            else
                            {
                                timeIDday = dayCount;
                                dayCount = 0;
                            }
                        }
                        else
                        {
                            if (dayCount > 365)
                            {
                                dayCount -= 365;
                                timeIDyear++;
                            }
                            else
                            {
                                timeIDday = dayCount;
                                dayCount = 0;
                            }
                        }
                    }
                }
                return timeIDyear * 1000 + timeIDday;
            }
        }
        /// <summary>
        /// Gets the Time Type used as a key on the database
        /// </summary>
        public char SqlTimeType
        {
            get
            {
                return _timeType;
            }
        }
        /// <summary>
        /// Gets the Time ID used as a key on the database
        /// </summary>
        public int SqlTimeID
        {
            get
            {
                return _timeID;
            }
        }/// <summary>
        /// Determines if this TimeID_Struct equals another
        /// </summary>
        /// <param name="obj">TimeID_Struct to be tested for equality</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SQL_TimeID sqlTimeID = (SQL_TimeID)obj;
            if (sqlTimeID.SqlTimeType == this.SqlTimeType
                && sqlTimeID.SqlTimeID == this.SqlTimeID)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Compares this TimeID_Struct to another TimeID_Struct
        /// </summary>
        /// <param name="aSQL_TimeID">The other TimeID_Struct</param>
        /// <returns>0 if equal, 1 if this TimeID_Struct is GT the other, -1 if this TimeID_Struct is less than the other</returns>
        public int CompareTo(SQL_TimeID aSQL_TimeID)
        {
            if (this.SqlTimeType == aSQL_TimeID.SqlTimeType)
            {
                return _timeID.CompareTo(aSQL_TimeID._timeID);
            }
            else
            {
                return this.SqlTimeType.CompareTo(aSQL_TimeID.SqlTimeType);
            }
        }
        /// <summary>
        /// Defines the "Less Than" operator fo TimeID_Struct
        /// </summary>
        /// <param name="aTimeID_1">1st TimeID</param>
        /// <param name="aTimeID_2">2nd TimeID</param>
        /// <returns>True if aTimeID_1 is less than aTimeID_2</returns>
        public static bool operator <(SQL_TimeID aTimeID_1, SQL_TimeID aTimeID_2)
        {
            return ((IComparable<SQL_TimeID>)aTimeID_1).CompareTo(aTimeID_2) < 0;
        }
        /// <summary>
        /// Defines the "Less Than or Equal" operator fo TimeID_Struct
        /// </summary>
        /// <param name="aTimeID_1">1st TimeID/param>
        /// <param name="aTimeID_2">2nd TimeID/param>
        /// <returns>True if aTimeID_1 is less than or equal to aTimeID_2</returns>
        public static bool operator <=(SQL_TimeID aTimeID_1, SQL_TimeID aTimeID_2)
        {
            return ((IComparable<SQL_TimeID>)aTimeID_1).CompareTo(aTimeID_2) <= 0;
        }
        /// <summary>
        /// Defines the ">" operator fo TimeID_Struct
        /// </summary>
        /// <param name="aTimeID_1">1st TimeID</param>
        /// <param name="aTimeID_2">2nd TimeID</param>
        /// <returns>True if aTimeID_1 is greater than aTimeID_2</returns>
        public static bool operator >(SQL_TimeID aTimeID_1, SQL_TimeID aTimeID_2)
        {
            return ((IComparable<SQL_TimeID>)aTimeID_1).CompareTo(aTimeID_2) > 0;
        }
        /// <summary>
        /// Defines the ">=" operator fo TimeID_Struct
        /// </summary>
        /// <param name="aTimeID_1">1st TimeID</param>
        /// <param name="aTimeID_2">2nd TimeID</param>
        /// <returns>True if aTimeID_1 is greater than or equal to aTimeID_2</returns>
        public static bool operator >=(SQL_TimeID aTimeID_1, SQL_TimeID aTimeID_2)
        {
            return ((IComparable<SQL_TimeID>)aTimeID_1).CompareTo(aTimeID_2) >= 0;
        }
        public override int GetHashCode()
        {
            return ((Convert.ToInt32(_timeType) << 16) + (int)_timeID).GetHashCode();
        }
        public override string ToString()
        {
            return _timeType.ToString() + _timeID.ToString();
        }
        /// <summary>
        /// Count number of time IDs (number of days or number of weeks) between two given timeIDs (inclusive of endpoints)
        /// </summary>
        /// <param name="aBeginTimeID">Start Time ID</param>
        /// <param name="aEndTimeID">End Time ID</param>
        /// <returns>Unique Number of time IDs between the two specified time IDs (including the endpoints</returns>
        public static int CountTimeIDs(SQL_TimeID aBeginTimeID, SQL_TimeID aEndTimeID)
        {
            int count = 0;
            if (aEndTimeID > aBeginTimeID)
            {
                count = aEndTimeID._timeID - aBeginTimeID.TimeID + 1;
            }
            else
            {
                count = aBeginTimeID._timeID - aEndTimeID._timeID + 1;
            }
            eSQLTimeIdType timeIDType = aEndTimeID.TimeIdType;
            if (timeIDType == eSQLTimeIdType.TimeIdIsWeekly)
            {
                if (aBeginTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsDaily)
                {
                    throw new Exception("Begin and End Time ID's must be same eSQLTimeIdType: Daily or Weekly");
                }
                count = count / 7;
            }
            return count;
        }
        /// <summary>
        /// Add TimeID_Count to a given TimeID
        /// </summary>
        /// <param name="aTimeID">Time ID </param>
        /// <param name="aTimeID_Count">Number of TimeID's to add to the given TimeID</param>
        /// <returns>Resulting SQL_TimeID</returns>
        public static SQL_TimeID AddToTimeID(SQL_TimeID aTimeID, int aTimeID_Count)
        {
            Int16 count;
            if (aTimeID.TimeIdType == eSQLTimeIdType.TimeIdIsWeekly)
            {
                count = (Int16)(aTimeID_Count * 7);
            }
            else
            {
                count = (Int16)aTimeID_Count;
            }
            return new SQL_TimeID(aTimeID.TimeIdType, (Int16)(aTimeID._timeID + count));
        }
    }
}