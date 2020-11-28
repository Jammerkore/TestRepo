using System;
using System.Collections;
using System.Data;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
    ///// <summary>
    ///// Summary description for AuditFilterProfile.
    ///// </summary>
    //[Serializable()]
    //public class AuditFilterProfile  : Profile
    //{
    //    //=======
    //    // FIELDS
    //    //=======
    //    private bool _filterFound;
    //    private int _userRID;
    //    private int _duration;
    //    private int _highestProcessMessageLevel;
    //    private int _highestDetailMessageLevel;
    //    private eFilterDateType _runDateType;
    //    private int _runDateBetweenFrom;
    //    private int _runDateBetweenTo;
    //    private DateTime _runDateFrom;
    //    private DateTime _runDateTo;
    //    private bool _showRunningProcesses;
    //    private bool _showCompletedProcesses;
    //    private bool _showMyTasksOnly;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public AuditFilterProfile(int aKey)
    //        : base(aKey)
    //    {
    //        _filterFound = false;
    //        _userRID = aKey;
    //        LoadFilter();
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    override public eProfileType ProfileType
    //    {
    //        get
    //        {
    //            return eProfileType.AuditFilter;
    //        }
    //    }

    //    public int UserRID
    //    {
    //        get
    //        {
    //            return _userRID;
    //        }
    //        set
    //        {
    //            _userRID = value;
    //        }
    //    }

    //    public int Duration
    //    {
    //        get
    //        {
    //            return _duration;
    //        }
    //        set
    //        {
    //            _duration = value;
    //        }
    //    }

    //    public int HighestProcessMessageLevel
    //    {
    //        get
    //        {
    //            return _highestProcessMessageLevel;
    //        }
    //        set
    //        {
    //            _highestProcessMessageLevel = value;
    //        }
    //    }

    //    public int HighestDetailMessageLevel
    //    {
    //        get
    //        {
    //            return _highestDetailMessageLevel;
    //        }
    //        set
    //        {
    //            _highestDetailMessageLevel = value;
    //        }
    //    }

    //    public eFilterDateType RunDateType
    //    {
    //        get
    //        {
    //            return _runDateType;
    //        }
    //        set
    //        {
    //            _runDateType = value;
    //        }
    //    }

    //    public int RunDateBetweenFrom
    //    {
    //        get
    //        {
    //            return _runDateBetweenFrom;
    //        }
    //        set
    //        {
    //            _runDateBetweenFrom = value;
    //        }
    //    }

    //    public int RunDateBetweenTo
    //    {
    //        get
    //        {
    //            return _runDateBetweenTo;
    //        }
    //        set
    //        {
    //            _runDateBetweenTo = value;
    //        }
    //    }

    //    public DateTime RunDateFrom
    //    {
    //        get
    //        {
    //            return _runDateFrom;
    //        }
    //        set
    //        {
    //            _runDateFrom = value;
    //        }
    //    }

    //    public DateTime RunDateTo
    //    {
    //        get
    //        {
    //            return _runDateTo;
    //        }
    //        set
    //        {
    //            _runDateTo = value;
    //        }
    //    }

    //    public bool ShowRunningProcesses
    //    {
    //        get
    //        {
    //            return _showRunningProcesses;
    //        }
    //        set
    //        {
    //            _showRunningProcesses = value;
    //        }
    //    }

    //    public bool ShowCompletedProcesses
    //    {
    //        get
    //        {
    //            return _showCompletedProcesses;
    //        }
    //        set
    //        {
    //            _showCompletedProcesses = value;
    //        }
    //    }

    //    public bool ShowMyTasksOnly
    //    {
    //        get
    //        {
    //            return _showMyTasksOnly;
    //        }
    //        set
    //        {
    //            _showMyTasksOnly = value;
    //        }
    //    }

    //    //========
    //    // METHODS
    //    //========

    //    //private void LoadFilter()
    //    //{
    //    //    try
    //    //    {
    //    //        SetDefaults();

    //    //        AuditFilterData awfd = new AuditFilterData();
    //    //        DataTable dt = awfd.AuditFilter_Read(_userRID);
    //    //        if (dt.Rows.Count > 0)
    //    //        {
    //    //            _filterFound = true;
    //    //            DataRow dr = dt.Rows[0];

    //    //            if (dr["RUN_DATE_TYPE"] != DBNull.Value)
    //    //            {
    //    //                _runDateType = (eFilterDateType)(Convert.ToInt32(dr["RUN_DATE_TYPE"]));
    //    //            }

    //    //            if (dr["RUN_DATE_BETWEEN_FROM"] != DBNull.Value)
    //    //            {
    //    //                _runDateBetweenFrom = Convert.ToInt32(dr["RUN_DATE_BETWEEN_FROM"]);
    //    //            }

    //    //            if (dr["RUN_DATE_BETWEEN_TO"] != DBNull.Value)
    //    //            {
    //    //                _runDateBetweenTo = Convert.ToInt32(dr["RUN_DATE_BETWEEN_TO"]);
    //    //            }

    //    //            if (dr["RUN_DATE_FROM"] != DBNull.Value)
    //    //            {
    //    //                _runDateFrom = Convert.ToDateTime(dr["RUN_DATE_FROM"], CultureInfo.CurrentUICulture);
    //    //            }

    //    //            if (dr["RUN_DATE_TO"] != DBNull.Value)
    //    //            {
    //    //                _runDateTo = Convert.ToDateTime(dr["RUN_DATE_TO"], CultureInfo.CurrentUICulture);
    //    //            }

    //    //            if (dr["PROCESS_HIGHEST_MESSAGE_LEVEL"] != DBNull.Value)
    //    //            {
    //    //                _highestProcessMessageLevel = Convert.ToInt32(dr["PROCESS_HIGHEST_MESSAGE_LEVEL"]);
    //    //            }

    //    //            if (dr["DETAIL_HIGHEST_MESSAGE_LEVEL"] != DBNull.Value)
    //    //            {
    //    //                _highestDetailMessageLevel = Convert.ToInt32(dr["DETAIL_HIGHEST_MESSAGE_LEVEL"]);
    //    //            }

    //    //            if (dr["DURATION"] != DBNull.Value)
    //    //            {
    //    //                _duration = Convert.ToInt32(dr["DURATION"]);
    //    //            }

    //    //            if (dr["SHOW_RUNNING"] != DBNull.Value)
    //    //            {
    //    //                _showRunningProcesses = Include.ConvertCharToBool(Convert.ToChar(dr["SHOW_RUNNING"]));
    //    //            }

    //    //            if (dr["SHOW_COMPLETED"] != DBNull.Value)
    //    //            {
    //    //                _showCompletedProcesses = Include.ConvertCharToBool(Convert.ToChar(dr["SHOW_COMPLETED"]));
    //    //            }

    //    //            if (dr["SHOW_MY_TASKS_ONLY"] != DBNull.Value)
    //    //            {
    //    //                _showMyTasksOnly = Include.ConvertCharToBool(Convert.ToChar(dr["SHOW_MY_TASKS_ONLY"]));
    //    //            }
    //    //        }
    //    //    }
    //    //    catch
    //    //    {
    //    //        throw;
    //    //    }
    //    //}

    //    private void SetDefaults()
    //    {
    //        try
    //        {
    //            _highestProcessMessageLevel = Convert.ToInt32(eMIDMessageLevel.None);
    //            _highestDetailMessageLevel = Convert.ToInt32(eMIDMessageLevel.None);
    //            _runDateType = eFilterDateType.all;
    //            _duration = 0;
    //            _showRunningProcesses = true;
    //            _showCompletedProcesses = true;
    //            _showMyTasksOnly = false;
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }

    //    public void WriteFilter()
    //    {
    //        AuditFilterData awfd = null;
    //        try
    //        {
    //            awfd = new AuditFilterData();
    //            awfd.OpenUpdateConnection();
    //            if (_filterFound)
    //            {
    //                awfd.AuditFilter_Update(_key, _runDateType, _runDateBetweenFrom,
    //                    _runDateBetweenTo, _runDateFrom, _runDateTo, _highestProcessMessageLevel,
    //                    _highestDetailMessageLevel, _duration, _showRunningProcesses, _showCompletedProcesses,
    //                    _showMyTasksOnly);
    //            }
    //            else
    //            {
    //                awfd.AuditFilter_Insert(_key, _runDateType, _runDateBetweenFrom,
    //                    _runDateBetweenTo, _runDateFrom, _runDateTo, _highestDetailMessageLevel,
    //                    _highestDetailMessageLevel, _duration, _showRunningProcesses, _showCompletedProcesses,
    //                    _showMyTasksOnly);
    //            }

    //            awfd.CommitData();
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            if (awfd != null)
    //            {
    //                awfd.CloseUpdateConnection();
    //            }
    //        }
    //    }
    //}
}
