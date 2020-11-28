using System;
using System.Collections;
using System.Data;

namespace MIDRetail.DataCommon
{
	public class OTSPlanSelectionData
	{
		#region Fields
		//==============
		// Saved Fields
		//==============

		private int _userID;
		private ePlanSessionType _planSessionType;
		private int _storeAttributeID;
		private int _filterID;
		private eStorePlanSelectedGroupBy _groupBy;
		private int _viewID;
		private int _storeNodeID;
		private string _storeNodeName;
		private int _storeVersionID;
		private string _storeVersionName;
		private int _timePeriodID;
		private string _timePeriodName;
		private eDisplayTimeBy _displayTimeBy;
		private int _chainNodeID;
		private string _chainNodeName;
		private int _chainVersionID;
		private string _chainVersionName;
		private bool _includeIneligibleStores;
		private bool _includeSimilarStores;
		private int _lowLevelVersionID;
		private string _lowLevelVersionName;
		private eLowLevelsType _lowLevelsType;
		private int _lowLevelsOffset;
		private int _lowLevelsSequence;
		private DataTable _viewTable;
		private ArrayList _userRIDList;
		private LowLevelVersionOverrideProfileList _versionOverrideList = null;

		// Fields passed for information
		private string _viewName;
		private int _viewUserID;
		#endregion Fields

		#region Properties

		//============
		//Properties
		//============

		
		public int UserID
		{
			get	{return _userID;}
			set	{_userID = value;}
		}

		public ePlanSessionType PlanSessionType
		{
			get	{return _planSessionType;}
			set	{_planSessionType = value;}
		}

		public int StoreAttributeID
		{
			get	{return _storeAttributeID;}
			set	{_storeAttributeID = value;}
		}

		public int FilterID
		{
			get	{return _filterID;}
			set	{_filterID = value;}
		}

		public int ViewID
		{
			get	{return _viewID;}
			set	{_viewID = value;}
		}

		public int StoreNodeID
		{
			get	{return _storeNodeID;}
			set	{_storeNodeID = value;}
		}

		public string StoreNodeName
		{
			get {return _storeNodeName;}
			set {_storeNodeName = value;}
		}

		public int StoreVersionID
		{
			get	{return _storeVersionID;}
			set	{_storeVersionID = value;}
		}

		public string StoreVersionName
		{
			get	{return _storeVersionName;}
			set	{_storeVersionName = value;}
		}

		public int TimePeriodID
		{
			get	{return _timePeriodID;}
			set	{_timePeriodID = value;}
		}

		public string TimePeriodName
		{
			get	{return _timePeriodName;}
			set	{_timePeriodName = value;}
		}

		public int ChainNodeID
		{
			get	{return _chainNodeID;}
			set	{_chainNodeID = value;}
		}

		public string ChainNodeName
		{
			get {return _chainNodeName;}
			set {_chainNodeName = value;}
		}

		public int ChainVersionID
		{
			get	{return _chainVersionID;}
			set	{_chainVersionID = value;}
		}

		public string ChainVersionName
		{
			get	{return _chainVersionName;}
			set	{_chainVersionName = value;}
		}

		public bool IncludeIneligibleStores
		{
			get	{return _includeIneligibleStores;}
			set	{_includeIneligibleStores = value;}
		}

		public string ViewName
		{
			get	{return _viewName;}
			set	{_viewName = value;}
		}

		public int ViewUserID
		{
			get	{return _viewUserID;}
			set	{_viewUserID = value;}
		}

		public bool IncludeSimilarStores
		{
			get	{return _includeSimilarStores;}
			set	{_includeSimilarStores = value;}
		}

		public int LowLevelVersionID
		{
			get	{return _lowLevelVersionID;}
			set	{_lowLevelVersionID = value;}
		}

		public string LowLevelVersionName
		{
			get	{return _lowLevelVersionName;}
			set	{_lowLevelVersionName = value;}
		}

		public eLowLevelsType LowLevelsType
		{
			get	{return _lowLevelsType;}
			set	{_lowLevelsType = value;}
		}

		public int LowLevelsOffset
		{
			get	{return _lowLevelsOffset;}
			set	{_lowLevelsOffset = value;}
		}

		public int LowLevelsSequence
		{
			get	{return _lowLevelsSequence;}
			set	{_lowLevelsSequence = value;}
		}

		public eStorePlanSelectedGroupBy GroupBy
		{
			get	{return _groupBy;}
			set {_groupBy = value;}
		}

		public eDisplayTimeBy DisplayTimeBy
		{
			get	{return _displayTimeBy;}
			set {_displayTimeBy = value;}
		}

		public DataTable ViewTable
		{
			get	{return _viewTable;}
			set {_viewTable = value;}
		}

		public ArrayList UserRIDList
		{
			get	{return _userRIDList;}
			set {_userRIDList = value;}
		}

		// BEGIN Override Low Level Enhancement
		//public LowLevelVersionOverrideProfileList VersionOverrideList
		//{
		//    get	
		//    {
		//        if (_versionOverrideList == null)
		//        {
		//            _versionOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
		//        }
		//        return _versionOverrideList;
		//    }
		//}
		// END Override Low Level Enhancement

		#endregion Properties
	}
}
