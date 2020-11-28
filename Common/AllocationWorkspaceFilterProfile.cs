//using System;
//using System.Collections;
//using System.Data;
//using System.Globalization;

//using MIDRetail.Data;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Common
//{
//    /// <summary>
//    /// Summary description for AllocationWorkspaceFilterProfile.
//    /// </summary>
//    [Serializable()]
//    public class AllocationWorkspaceFilterProfile  : Profile
//    {
//        //=======
//        // FIELDS
//        //=======
//        private bool _filterFound;
//        private int _userRID;
//        private int _hnRID;
//        private Hashtable _selectedTypes;
//        private Hashtable _selectedStatuses;
//        private eFilterDateType _headerDateType;
//        private int _headerDateBetweenFrom;
//        private int _headerDateBetweenTo;
//        private DateTime _headerDateFrom;
//        private DateTime _headerDateTo;
//        private eFilterDateType _releaseDateType;
//        private int _releaseDateBetweenFrom;
//        private int _releaseDateBetweenTo;
//        private DateTime _releaseDateFrom;
//        private DateTime _releaseDateTo;
//        private int _maximumHeaders;

//        //=============
//        // CONSTRUCTORS
//        //=============

//        public AllocationWorkspaceFilterProfile(int aKey)
//            : base(aKey)
//        {
//            _filterFound = false;
//            _userRID = aKey;
//            _hnRID = Include.NoRID;
//            _selectedTypes = new Hashtable();
//            _selectedStatuses = new Hashtable();
//            LoadFilter();
//        }

//        //===========
//        // PROPERTIES
//        //===========

//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.AllocationWorkspaceFilter;
//            }
//        }

//        public int UserRID
//        {
//            get
//            {
//                return _userRID;
//            }
//            set
//            {
//                _userRID = value;
//            }
//        }

//        public int HnRID
//        {
//            get
//            {
//                return _hnRID;
//            }
//            set
//            {
//                _hnRID = value;
//            }
//        }

//        public Hashtable SelectedTypes
//        {
//            get
//            {
//                return _selectedTypes;
//            }
//            set
//            {
//                _selectedTypes = value;
//            }
//        }

//        public Hashtable SelectedStatuses
//        {
//            get
//            {
//                return _selectedStatuses;
//            }
//            set
//            {
//                _selectedStatuses = value;
//            }
//        }

//        public eFilterDateType HeaderDateType
//        {
//            get
//            {
//                return _headerDateType;
//            }
//            set
//            {
//                _headerDateType = value;
//            }
//        }

//        public int HeaderDateBetweenFrom
//        {
//            get
//            {
//                return _headerDateBetweenFrom;
//            }
//            set
//            {
//                _headerDateBetweenFrom = value;
//            }
//        }

//        public int HeaderDateBetweenTo
//        {
//            get
//            {
//                return _headerDateBetweenTo;
//            }
//            set
//            {
//                _headerDateBetweenTo = value;
//            }
//        }

//        public DateTime HeaderDateFrom
//        {
//            get
//            {
//                return _headerDateFrom;
//            }
//            set
//            {
//                _headerDateFrom = value;
//            }
//        }

//        public DateTime HeaderDateTo
//        {
//            get
//            {
//                return _headerDateTo;
//            }
//            set
//            {
//                _headerDateTo = value;
//            }
//        }

//        public eFilterDateType ReleaseDateType
//        {
//            get
//            {
//                return _releaseDateType;
//            }
//            set
//            {
//                _releaseDateType = value;
//            }
//        }

//        public int ReleaseDateBetweenFrom
//        {
//            get
//            {
//                return _releaseDateBetweenFrom;
//            }
//            set
//            {
//                _releaseDateBetweenFrom = value;
//            }
//        }

//        public int ReleaseDateBetweenTo
//        {
//            get
//            {
//                return _releaseDateBetweenTo;
//            }
//            set
//            {
//                _releaseDateBetweenTo = value;
//            }
//        }

//        public DateTime ReleaseDateFrom
//        {
//            get
//            {
//                return _releaseDateFrom;
//            }
//            set
//            {
//                _releaseDateFrom = value;
//            }
//        }

//        public DateTime ReleaseDateTo
//        {
//            get
//            {
//                return _releaseDateTo;
//            }
//            set
//            {
//                _releaseDateTo = value;
//            }
//        }

//        public int MaximumHeaders
//        {
//            get
//            {
//                return _maximumHeaders;
//            }
//            set
//            {
//                _maximumHeaders = value;
//            }
//        }

//        //========
//        // METHODS
//        //========

//        private void LoadFilter()
//        {
//            try
//            {
//                SetDefaults();

//                AllocationWorkspaceFilterData awfd = new AllocationWorkspaceFilterData();
//                DataTable dt = awfd.AllocationWorkspaceFilter_Read(_userRID);
//                if (dt.Rows.Count > 0)
//                {
//                    _filterFound = true;
//                    DataRow dr = dt.Rows[0];

//                    if (dr["HN_RID"] == DBNull.Value)
//                    {
//                        _hnRID = Include.NoRID;
//                    }
//                    else
//                    {
//                        _hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["HEADER_DATE_TYPE"] == DBNull.Value)
//                    {
//                        _headerDateType = eFilterDateType.between;
//                    }
//                    else
//                    {
//                        _headerDateType = (eFilterDateType)(Convert.ToInt32(dr["HEADER_DATE_TYPE"], CultureInfo.CurrentUICulture));
//                    }

//                    if (dr["HEADER_DATE_BETWEEN_FROM"] == DBNull.Value)
//                    {
//                        _headerDateBetweenFrom = -7;
//                    }
//                    else
//                    {
//                        _headerDateBetweenFrom = Convert.ToInt32(dr["HEADER_DATE_BETWEEN_FROM"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["HEADER_DATE_BETWEEN_TO"] == DBNull.Value)
//                    {
//                        _headerDateBetweenTo = 0;
//                    }
//                    else
//                    {
//                        _headerDateBetweenTo = Convert.ToInt32(dr["HEADER_DATE_BETWEEN_TO"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["HEADER_DATE_FROM"] == DBNull.Value)
//                    {
//                        _headerDateFrom = DateTime.Now;
//                    }
//                    else
//                    {
//                        _headerDateFrom = Convert.ToDateTime(dr["HEADER_DATE_FROM"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["HEADER_DATE_TO"] == DBNull.Value)
//                    {
//                        _headerDateTo = DateTime.Now;
//                    }
//                    else
//                    {
//                        _headerDateTo = Convert.ToDateTime(dr["HEADER_DATE_TO"], CultureInfo.CurrentUICulture);
//                    }


//                    if (dr["RELEASE_DATE_TYPE"] == DBNull.Value)
//                    {
//                        _releaseDateType = eFilterDateType.between;
//                    }
//                    else
//                    {
//                        _releaseDateType = (eFilterDateType)(Convert.ToInt32(dr["RELEASE_DATE_TYPE"], CultureInfo.CurrentUICulture));
//                    }

//                    if (dr["RELEASE_DATE_BETWEEN_FROM"] == DBNull.Value)
//                    {
//                        _releaseDateBetweenFrom = -7;
//                    }
//                    else
//                    {
//                        _releaseDateBetweenFrom = Convert.ToInt32(dr["RELEASE_DATE_BETWEEN_FROM"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["RELEASE_DATE_BETWEEN_TO"] == DBNull.Value)
//                    {
//                        _releaseDateBetweenTo = 0;
//                    }
//                    else
//                    {
//                        _releaseDateBetweenTo = Convert.ToInt32(dr["RELEASE_DATE_BETWEEN_TO"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["RELEASE_DATE_FROM"] == DBNull.Value)
//                    {
//                        _releaseDateFrom = DateTime.Now;
//                    }
//                    else
//                    {
//                        _releaseDateFrom = Convert.ToDateTime(dr["RELEASE_DATE_FROM"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["RELEASE_DATE_TO"] == DBNull.Value)
//                    {
//                        _releaseDateTo = DateTime.Now;
//                    }
//                    else
//                    {
//                        _releaseDateTo = Convert.ToDateTime(dr["RELEASE_DATE_TO"], CultureInfo.CurrentUICulture);
//                    }

//                    if (dr["MAXIMUM_HEADERS"] == DBNull.Value)
//                    {
//                        _maximumHeaders = 5000;
//                    }
//                    else
//                    {
//                        _maximumHeaders = Convert.ToInt32(dr["MAXIMUM_HEADERS"], CultureInfo.CurrentUICulture);
//                    }

//                    HeaderType ht = null;
//                    eHeaderType type;
//                    dt = awfd.AllocationWorkspaceFilterTypes_Read(_userRID);
//                    foreach(DataRow typeRow in dt.Rows)
//                    {
//                        type = (eHeaderType)(Convert.ToInt32(typeRow["HEADER_TYPE"], CultureInfo.CurrentUICulture));
//                        ht = (HeaderType)SelectedTypes[type];
//                        if (ht != null)
//                        {
//                            ht.IsDisplayed = Include.ConvertCharToBool(Convert.ToChar(typeRow["TYPE_SELECTED"], CultureInfo.CurrentUICulture));
//                        }
//                    }

//                    HeaderStatus hs = null;
//                    eHeaderAllocationStatus status;
//                    dt = awfd.AllocationWorkspaceFilterStatus_Read(_userRID);
//                    foreach(DataRow statusRow in dt.Rows)
//                    {
//                        status = (eHeaderAllocationStatus)(Convert.ToInt32(statusRow["HEADER_STATUS"], CultureInfo.CurrentUICulture));
//                        hs = (HeaderStatus)SelectedStatuses[status];
//                        if (hs != null)
//                        {
//                            hs.IsDisplayed = Include.ConvertCharToBool(Convert.ToChar(statusRow["STATUS_SELECTED"], CultureInfo.CurrentUICulture));
//                        }
//                    }

//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void SetDefaults()
//        {
//            try
//            {
//                _maximumHeaders = 5000;
//                _hnRID = Include.NoRID;
//                _selectedTypes.Clear();
//                _selectedStatuses.Clear();

//                HeaderType ht = null;
//                // Begin TT#1043 – JSmith - Assortment and Placeholder show as header types when should not.
//                //DataTable dtTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue);
//                DataTable dtTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));
//                // End TT#1043
//                foreach (DataRow dr in dtTypes.Rows)
//                {
//                    ht = new HeaderType((eHeaderType)Convert.ToInt32(dr["TEXT_CODE"],CultureInfo.CurrentUICulture),true);
//                    ht.DefaultValue = true;
//                    _selectedTypes.Add(ht.Type, ht);
//                }
	
//                HeaderStatus hs = null;
//                DataTable dtStatus = MIDText.GetTextType(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
//                foreach (DataRow dr in dtStatus.Rows)
//                {
//                    hs = new HeaderStatus();
//                    hs.Status = (eHeaderAllocationStatus)Convert.ToInt32(dr["TEXT_CODE"],CultureInfo.CurrentUICulture);
//                    if (hs.Status == eHeaderAllocationStatus.Released)
//                    {
//                        hs.IsDisplayed = false;
//                        hs.DefaultValue = false;
//                    }
//                    else
//                    {
//                        hs.IsDisplayed = true;
//                        hs.DefaultValue = true;
//                    }
//                    _selectedStatuses.Add(hs.Status, hs);

//                }

//                _headerDateType = eFilterDateType.between;
//                _headerDateBetweenFrom = -7;
//                _headerDateBetweenTo = 0;
//                _releaseDateType = eFilterDateType.between;
//                _releaseDateBetweenFrom = -7;
//                _releaseDateBetweenTo = 0;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public void WriteFilter()
//        {
//            AllocationWorkspaceFilterData awfd = null;
//            try
//            {
//                awfd = new AllocationWorkspaceFilterData();
//                awfd.OpenUpdateConnection();
//                if (_filterFound)
//                {
//                    awfd.AllocationWorkspaceFilter_Update(_key, _hnRID, _headerDateType, _headerDateBetweenFrom,
//                        _headerDateBetweenTo, _headerDateFrom, _headerDateTo, _releaseDateType,
//                        _releaseDateBetweenFrom, _releaseDateBetweenTo, _releaseDateFrom, _releaseDateTo, _maximumHeaders);
//                }
//                else
//                {
//                    awfd.AllocationWorkspaceFilter_Insert(_key, _hnRID, _headerDateType, _headerDateBetweenFrom,
//                        _headerDateBetweenTo, _headerDateFrom, _headerDateTo, _releaseDateType,
//                        _releaseDateBetweenFrom, _releaseDateBetweenTo, _releaseDateFrom, _releaseDateTo, _maximumHeaders);
//                }

//                awfd.AllocationWorkspaceFilterTypes_DeleteAll(_key);
//                foreach (HeaderType ht in _selectedTypes.Values)
//                {
//                    awfd.AllocationWorkspaceFilterTypes_Insert(_key, ht.Type, ht.IsDisplayed);
//                }

//                awfd.AllocationWorkspaceFilterStatus_DeleteAll(_key);
//                foreach (HeaderStatus hs in this._selectedStatuses.Values)
//                {
//                    awfd.AllocationWorkspaceFilterStatus_Insert(_key, hs.Status, hs.IsDisplayed);
//                }

//                awfd.CommitData();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (awfd != null)
//                {
//                    awfd.CloseUpdateConnection();
//                }
//            }
//        }
//    }

//    public class HeaderType
//    {
//        //=======
//        // FIELDS
//        //=======

//        private eHeaderType _headerType;	
//        private bool _isDisplayed;	
//        private bool _defaultValue;

//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of HeaderType.
//        /// </summary>

//        public HeaderType()
//        {
//            _headerType = eHeaderType.Receipt;
//            _isDisplayed = false;
//        }

//        /// <summary>
//        /// Creates a new instance of HeaderType using the given type and boolean.
//        /// </summary>
//        /// <param name="aHeaderType">
//        /// The header type.
//        /// </param>
//        /// <param name="aIsDisplayed">
//        /// A boolean indicating if the header type is currently displayed.
//        /// </param>
		
//        public HeaderType(eHeaderType aHeaderType, bool aIsDisplayed)
//        {
//            _headerType = aHeaderType;
//            _isDisplayed = aIsDisplayed;
//        }

//        //===========
//        // PROPERTIES
//        //===========

//        /// <summary>
//        /// Gets or sets the type of the header.
//        /// </summary>

//        public eHeaderType Type
//        {
//            get
//            {
//                return _headerType;
//            }
//            set
//            {
//                _headerType = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the boolean indicating if the header type is displayed.
//        /// </summary>

//        public bool IsDisplayed
//        {
//            get
//            {
//                return _isDisplayed;
//            }
//            set
//            {
//                _isDisplayed = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the boolean indicating if the default value if the header type is displayed.
//        /// </summary>

//        public bool DefaultValue
//        {
//            get
//            {
//                return _defaultValue;
//            }
//            set
//            {
//                _defaultValue = value;
//            }
//        }

//        //========
//        // METHODS
//        //========
//    }


//    public class HeaderStatus
//    {
//        //=======
//        // FIELDS
//        //=======

//        private eHeaderAllocationStatus _headerStatus;	
//        private bool _isDisplayed;	
//        private bool _defaultValue;

//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of HeaderStatus.
//        /// </summary>

//        public HeaderStatus()
//        {
//            _headerStatus = eHeaderAllocationStatus.ReceivedOutOfBalance;
//            _isDisplayed = false;
//        }

//        /// <summary>
//        /// Creates a new instance of HeaderStatus using the given status and boolean.
//        /// </summary>
//        /// <param name="aHeaderStatus">
//        /// The header status.
//        /// </param>
//        /// <param name="aIsDisplayed">
//        /// A boolean indicating if the header status is currently displayed.
//        /// </param>
		
//        public HeaderStatus(eHeaderAllocationStatus aHeaderStatus, bool aIsDisplayed)
//        {
//            _headerStatus = aHeaderStatus;
//            _isDisplayed = aIsDisplayed;
//        }

//        //===========
//        // PROPERTIES
//        //===========

//        /// <summary>
//        /// Gets or sets the status of the header.
//        /// </summary>

//        public eHeaderAllocationStatus Status
//        {
//            get
//            {
//                return _headerStatus;
//            }
//            set
//            {
//                _headerStatus = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the boolean indicating if the header status is displayed.
//        /// </summary>

//        public bool IsDisplayed
//        {
//            get
//            {
//                return _isDisplayed;
//            }
//            set
//            {
//                _isDisplayed = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the boolean indicating if the default value if the header status is displayed.
//        /// </summary>

//        public bool DefaultValue
//        {
//            get
//            {
//                return _defaultValue;
//            }
//            set
//            {
//                _defaultValue = value;
//            }
//        }

//        //========
//        // METHODS
//        //========
//    }


//}
