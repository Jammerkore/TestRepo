using System;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
    /// <summary>
    /// Contains the information about the Chain Set Percentages for a node in a hierarchy for each week.
    /// </summary>
    [Serializable()]
    public class ChainSetPercentProfiles : Profile
    {
        // Fields

        private bool _newRecord;
        private int _StoreWeekId;
        private eChangeType _ChainSetPercentChangeType;
        private bool _ChainSetPercentIsInherited;
        private int _ChainSetPercentInheritedFromNodeRID;
        private int _storeGroupRID;
        private string _storeGroupID;
        private int _storeGroupVersion; // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        private int _storeGroupLevelRID;
        private string _storeGroupLevelID;
        private decimal _ChainSetPercent;
        private int _TimeID;
        private int _nodeRID;
        private string _nodeID;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChainSetPercentProfiles(int aKey)
            : base(aKey)
        {
            _storeGroupLevelRID = 0;
            _ChainSetPercentChangeType = eChangeType.none;
            _newRecord = true;
            _ChainSetPercentIsInherited = false;
            //_ChainSetPercentInheritedFromNodeRID = Include.NoRID;
        }

        // Properties
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.ChainSetPercent;
            }
        }
        public bool NewRecord
        {
            get { return _newRecord; }
            set { _newRecord = value; }
        }
        public eChangeType ChainSetPercentChangeType
        {
            get { return _ChainSetPercentChangeType; }
            set { _ChainSetPercentChangeType = value; }
        }
        public bool ChainSetPercentIsInherited
        {
            get { return _ChainSetPercentIsInherited; }
            set { _ChainSetPercentIsInherited = value; }
        }
        public int ChainSetPercentInheritedFromNodeRID
        {
            get { return _ChainSetPercentInheritedFromNodeRID; }
            set { _ChainSetPercentInheritedFromNodeRID = value; }
        }

        public int StoreWeekID
        {
            get { return _StoreWeekId; }
            set { _StoreWeekId = value; }
        }

        public int StoreGroupRID
        {
            get { return _storeGroupRID; }
            set { _storeGroupRID = value; }
        }

        public string StoreGroupID
        {
            get { return _storeGroupID; }
            set { _storeGroupID = value; }
        }

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        public int StoreGroupVersion
        {
            get { return _storeGroupVersion; }
            set { _storeGroupVersion = value; }
        }
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		
        public int StoreGroupLevelRID
        {
            get { return _storeGroupLevelRID; }
            set { _storeGroupLevelRID = value; }
        }

        public string StoreGroupLevelID
        {
            get { return _storeGroupLevelID; }
            set { _storeGroupLevelID = value; }
        }

        public decimal ChainSetPercent
        {
            get { return _ChainSetPercent; }
            set { _ChainSetPercent = value; }
        }

        public int TimeID
        {
            get { return _TimeID; }
            set { _TimeID = value; }
        }
        public int YearDay
        {
            get
            {
                return Convert.ToInt32(TimeID.ToString().Substring(0, 7));
            }
        }
        public int NodeRID
        {
            get { return _nodeRID; }
            set { _nodeRID = value; }
        }
        public string NodeID
        { 
            get { return _nodeID; }
            set { _nodeID = value; }
        }
    }

    /// <summary>
    /// Used to retrieve a list of chain set percentages
    /// </summary>
    [Serializable()]
    public class ChainSetPercentList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChainSetPercentList(eProfileType aProfileType)
            : base(aProfileType)
        {
            //
            // TODO: Add constructor logic here
            //


        }
    }
    [Serializable()]
    public class StoreGroupListKey : IComparable<StoreGroupListKey>
    {
        private string _Name;
        private int _SGL_RID;

        public int CompareTo(StoreGroupListKey SGL)
        {

            return _SGL_RID.CompareTo(SGL._SGL_RID);
            
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public int SGL_RID
        {
            get { return _SGL_RID; }
            set { _SGL_RID = value; }
        }

        public StoreGroupListKey( string Name, int SGL_RID)
        {
            _Name = Name;
            _SGL_RID = SGL_RID;
        }

    }

    [Serializable()]
    public class StoreGroupWeekList : IComparable<StoreGroupWeekList>
    {
        private string _Name;
        private int _SGL_RID;
        private int _week;

        public int CompareTo(StoreGroupWeekList SGL)
        {

            return _SGL_RID.CompareTo(SGL._SGL_RID);

        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public int SGL_RID
        {
            get { return _SGL_RID; }
            set { _SGL_RID = value; }
        }
        public int Week
        {
            get { return _week; }
            set { _week = value; }
        }


        public StoreGroupWeekList(string Name, int SGL_RID, int Week)
        {
            _Name = Name;
            _SGL_RID = SGL_RID;
            _week = Week;
        }

    }


    [Serializable()]
    public class ChainSetPercentValues : IComparable<ChainSetPercentValues>
    {
        //Declare objects
        private string _week;
        private string _SG_ID;
        private int _SGL_RID;
        private string _SGL_ID;
        private decimal? _Percentage;
        private bool _ChainSetPercentIsInherited;
        private int _ChainSetPercentInheritedFromNodeRID;
        private int _CDR_RID;
        public bool _updated;
        private int _seqId;

        public int CompareTo(ChainSetPercentValues other)
        {

            return _week.CompareTo(other._week);
        }

        //Constructors
        public string Week
        {
            get { return _week; }
            set { _week = value; }
        }
        public string SG_ID
        {
            get { return _SG_ID; }
            set { _SG_ID = value; }
        }
        public int SGL_RID
        {
            get { return _SGL_RID; }
            set { _SGL_RID = value; }
        }
        public string SGL_ID
        {
            get { return _SGL_ID; }
            set { _SGL_ID = value; }
        }
        public decimal? Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; }
        }
        public bool ChainSetPercentIsInherited
        {
            get { return _ChainSetPercentIsInherited; }
            set { _ChainSetPercentIsInherited = value; }
        }
        public int ChainSetPercentInheritedFromNodeRID
        {
            get { return _ChainSetPercentInheritedFromNodeRID; }
            set { _ChainSetPercentInheritedFromNodeRID = value; }
        }
        public int TimeID
        {
            get { return _CDR_RID; }
            set { _CDR_RID = value; }
        }
        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public int seqId
        {
            get { return _seqId; }
            set { _seqId = value; }
        }
        //
        public ChainSetPercentValues(string Week, string StoreGroupId, int storeRID, string storeID, decimal? Pct, bool cspii, int cspiiRID, int CDR_RID, bool updated, int seqId)
        {
            _week = Week;
            _SG_ID = StoreGroupId;
            _SGL_RID = storeRID;
            _SGL_ID = storeID;
            _Percentage = Pct;
            _ChainSetPercentIsInherited = cspii;
            _ChainSetPercentInheritedFromNodeRID = cspiiRID;
            _CDR_RID = CDR_RID;
            _updated = updated;
            _seqId = seqId;
        }

    }

    [Serializable()]
    public class ChainSetPercentWeekProfiles : Profile
    {
        // Fields
        private bool _newRecord;
        private int _StoreWeekId;
        private eChangeType _ChainSetPercentChangeType;
        private bool _ChainSetPercentIsInherited;
        private int _ChainSetPercentInheritedFromNodeRID;
        private int _storeGroupRID;
        private string _storeGroupID;
        private int _storeGroupLevelRID;
        private string _storeGroupLevelID;
        private int _ChainSetPercent;
        private DateTime _CDR_RID;
        private int _nodeRID;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChainSetPercentWeekProfiles(int aKey)
            : base(aKey)
        {
            _StoreWeekId = 0;
            _newRecord = true;
            _ChainSetPercentIsInherited = false;
            //_ChainSetPercentInheritedFromNodeRID = Include.NoRID;
        }

        // Properties
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.ChainSetPercent;
            }
        }
        public bool NewRecord
        {
            get { return _newRecord; }
            set { _newRecord = value; }
        }
        public eChangeType ChainSetPercentChangeType
        {
            get { return _ChainSetPercentChangeType; }
            set { _ChainSetPercentChangeType = value; }
        }
        public bool ChainSetPercentIsInherited
        {
            get { return _ChainSetPercentIsInherited; }
            set { _ChainSetPercentIsInherited = value; }
        }
        public int ChainSetPercentInheritedFromNodeRID
        {
            get { return _ChainSetPercentInheritedFromNodeRID; }
            set { _ChainSetPercentInheritedFromNodeRID = value; }
        }
        public int StoreWeekID
        {
            get { return _StoreWeekId; }
            set { _StoreWeekId = value; }
        }
        public int StoreGroupRID
        {
            get { return _storeGroupRID; }
            set { _storeGroupRID = value; }
        }
        public string StoreGroupID
        {
            get { return _storeGroupID; }
            set { _storeGroupID = value; }
        }
        public int StoreGroupLevelRID
        {
            get { return _storeGroupLevelRID; }
            set { _storeGroupLevelRID = value; }
        }
        public string StoreGroupLevelID
        {
            get { return _storeGroupLevelID; }
            set { _storeGroupLevelID = value; }
        }
        public int ChainSetPercent
        {
            get { return _ChainSetPercent; }
            set { _ChainSetPercent = value; }
        }
        public DateTime TimeID
        {
            get { return _CDR_RID; }
            set { _CDR_RID = value; }
        }
        public int NodeRID
        {
            get { return _nodeRID; }
            set { _nodeRID = value; }
        }
    }

    /// <summary>
    /// Used to retrieve a list of chain set percentages by week
    /// </summary>
    [Serializable()]
    public class ChainSetPercentWeekList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ChainSetPercentWeekList(eProfileType aProfileType)
            : base(aProfileType)
        {
            //
            // TODO: Add constructor logic here
            //


        }
    }

}
