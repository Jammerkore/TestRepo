using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Globalization;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
	public delegate void MIDSearchEventHandler(object sender, MIDSearchEventArgs e);
	public delegate void MIDSearchCompletedEventHandler(object sender, MIDSearchCompletedEventArgs e);

	abstract public class BaseSearchEngine
	{
		//=======
		// FIELDS
		//=======

		protected SessionAddressBlock _SAB;
		protected char _wildcardChar;
		protected char _separatorChar;
		protected string _wildcardStr;
		protected string _separatorStr;
		protected bool _caseSensitive;
		protected bool _matchWholeWord;
        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
        protected bool _stopSearch;
        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BaseSearchEngine.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock.
		/// </param>

		public BaseSearchEngine(SessionAddressBlock aSAB, char aWildcard,
			char aSeparator, bool aCaseSensitive, bool aMatchWholeWord)
		{
			_SAB = aSAB;
			_wildcardChar = aWildcard;
			_separatorChar = aSeparator;
			_wildcardStr = Convert.ToString(aWildcard);
			_separatorStr = Convert.ToString(aSeparator);
			_caseSensitive = aCaseSensitive;
			_matchWholeWord = aMatchWholeWord;
            // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
            _stopSearch = false;
            // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the SessionAddressBlock object.
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		/// <summary>
		/// Returns the wildcard character for text values.
		/// </summary>

		public char WildcardChar
		{
			get
			{
				return _wildcardChar;
			}
		}

		/// <summary>
		/// Returns the wildcard string for text values.
		/// </summary>

		public string WildcardStr
		{
			get
			{
				return _wildcardStr;
			}
		}

		/// <summary>
		/// Returns the separator character for text values.
		/// </summary>

		public char SeparatorChar
		{
			get
			{
				return _separatorChar;
			}
		}

		/// <summary>
		/// Returns the separator string for text values.
		/// </summary>

		public string SeparatorStr
		{
			get
			{
				return _separatorStr;
			}
		}

		/// <summary>
		/// Returns the flag identifying if the search is case sensitive.
		/// </summary>

		public bool CaseSensitive
		{
			get
			{
				return _caseSensitive;
			}
		}

		/// <summary>
		/// Returns the flag identifying if the search is match the whole word.
		/// </summary>

		public bool MatchWholeWord
		{
			get
			{
				return _matchWholeWord;
			}
		}

        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
        /// <summary>
        /// Returns the flag identifying if the search is to be stopped.
        /// </summary>
        protected bool StopSearch
        {
            get
            {
                return _stopSearch;
            }
        }
        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

		abstract public void GetSearchResults();

		protected bool TextMatch(ArrayList aPatterns, string aText)
		{
			try
			{
				if (!CaseSensitive)
				{
					aText = aText.ToUpper();
				}
				bool textMatch = false;
				foreach (string pattern in aPatterns)
				{
					if (MatchWholeWord)
					{
						if (aText == pattern)
						{
							textMatch = true;
							break;
						}
					}
					// see if pattern contain wildcard
					else if (pattern.Contains(WildcardStr))
					{
						if (Microsoft.VisualBasic.CompilerServices.Operators.LikeString(aText, pattern, Microsoft.VisualBasic.CompareMethod.Text))
						{
							textMatch = true;
							break;
						}
					}
					else if (aText.Contains(pattern))
					{
						textMatch = true;
						break;
					}
				}
				return textMatch;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
        public void RequestStop()
        {
            _stopSearch = true;
        }
        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client

	}

	public class MIDSearchEventArgs : EventArgs
	{
		public MIDSearchEventArgs()
		{
			
		}
	}

	public class MIDSearchCompletedEventArgs : EventArgs
	{
		public MIDSearchCompletedEventArgs()
		{

		}
	}

    //Begin TT#1388-MD -jsobek -Product Filters
    //public class ProductSearchEngine : BaseSearchEngine
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    public event MIDSearchEventHandler MIDSearchEvent;
    //    public event MIDSearchCompletedEventHandler MIDSearchCompletedEvent;


    //    private ProductSearchInfo _searchInfo;
    //    private Hashtable _hierarchies = new Hashtable();
    //    private Hashtable _colors = new Hashtable();
    //    private Hashtable _sizes = new Hashtable();
    //    //private ProductSearchFilter _productSearchFilter;

    //    //========
    //    // CLASSES
    //    //========

    //    public class ProductSearchInfo
    //    {
    //        //=======
    //        // FIELDS
    //        //=======

    //        private SessionAddressBlock _SAB;
    //        private ArrayList _nodeList;
    //        private string _ID;
    //        private string _name;
    //        private string _description;
    //        private Hashtable _levels;
    //        private ArrayList _IDList = new ArrayList();
    //        private ArrayList _NameList = new ArrayList();
    //        private ArrayList _DescriptionList = new ArrayList();
    //        private char _wildcard;
    //        private char _separator;
    //        private bool _caseSensitive;
    //        private bool _applyCharacteristics;
    //        //private ProductSearchFilterDefinition _characteristicFilter;
    //        private int _lowestLevel = 99999;

    //        //=============
    //        // CONSTRUCTORS
    //        //=============

    //        public ProductSearchInfo(
    //            SessionAddressBlock aSAB,
    //            ArrayList aNodeList,
    //            char aWildcard,
    //            char aSeparator,
    //            bool aCaseSensitive,
    //            string aID,
    //            string aName,
    //            string aDescription,
    //            Hashtable aLevels,
    //            bool aApplyCharacteristics)//,
    //            //ProductSearchFilterDefinition aCharacteristicFilter)
    //        {
    //            _SAB = aSAB;
    //            _wildcard = aWildcard;
    //            _separator = aSeparator;
    //            _caseSensitive = aCaseSensitive;
    //            _nodeList = aNodeList;
    //            _ID = aID;
    //            _name = aName;
    //            _description = aDescription;
    //            _levels = aLevels;
    //            _applyCharacteristics = aApplyCharacteristics;
    //            //_characteristicFilter = aCharacteristicFilter;

    //            if (aID.Trim().Length > 0)
    //            {
    //                if (!_caseSensitive)
    //                {
    //                    aID = aID.ToUpper();
    //                }
    //                string[] fields = MIDstringTools.Split(aID, _separator, true);
    //                foreach (string field in fields)
    //                {
    //                    _IDList.Add(field);
    //                }
    //            }

    //            if (aName.Trim().Length > 0)
    //            {
    //                if (!_caseSensitive)
    //                {
    //                    aName = aName.ToUpper();
    //                }
    //                string[] fields = MIDstringTools.Split(aName, _separator, true);
    //                foreach (string field in fields)
    //                {
    //                    _NameList.Add(field);
    //                }
    //            }

    //            if (aDescription.Trim().Length > 0)
    //            {
    //                if (!_caseSensitive)
    //                {
    //                    aDescription = aDescription.ToUpper();
    //                }
    //                string[] fields = MIDstringTools.Split(aDescription, _separator, true);
    //                foreach (string field in fields)
    //                {
    //                    _DescriptionList.Add(field);
    //                }
    //            }

    //            if (_levels.Count > 0)
    //            {
    //                _lowestLevel = 0;
    //                foreach (int level in _levels.Keys)
    //                {
    //                    if (level > _lowestLevel)
    //                    {
    //                        _lowestLevel = level;
    //                    }
    //                }
    //            }
    //        }

    //        //===========
    //        // PROPERTIES
    //        //===========

    //        public ArrayList NodeList
    //        {
    //            get { return _nodeList; }
    //        }

    //        public string ID
    //        {
    //            get { return _ID; }
    //        }

    //        public string Name
    //        {
    //            get { return _name; }
    //        }

    //        public string Description
    //        {
    //            get { return _description; }
    //        }

    //        public Hashtable Levels
    //        {
    //            get { return _levels; }
    //        }

    //        public int LowestLevel
    //        {
    //            get { return _lowestLevel; }
    //        }

    //        public ArrayList IDList
    //        {
    //            get { return _IDList; }
    //        }

    //        public ArrayList NameList
    //        {
    //            get { return _NameList; }
    //        }

    //        public ArrayList DescriptionList
    //        {
    //            get { return _DescriptionList; }
    //        }

    //        public bool ApplyCharacteristics
    //        {
    //            get { return _applyCharacteristics; }
    //        }

    //        //public ProductSearchFilterDefinition CharacteristicFilter
    //        //{
    //        //    get { return _characteristicFilter; }
    //        //}

    //        //========
    //        // METHODS
    //        //========
    //    }

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    /// <summary>
    //    /// Creates a new instance of ProductSearchEngine.
    //    /// </summary>
    //    /// <param name="aSAB">
    //    /// The SessionAddressBlock.
    //    /// </param>

    //    public ProductSearchEngine(
    //        SessionAddressBlock aSAB,
    //        ArrayList aNodeList,
    //        char aWildcard,
    //        char aSeparator,
    //        bool aCaseSensitive,
    //        bool aMatchWholeWord,
    //        string aID,
    //        string aName,
    //        string aDescription,
    //        Hashtable aLevels,
    //        bool aApplyCharacteristics) //,
    //        //ProductSearchFilterDefinition aCharacteristicFilter)
    //        : base(aSAB, aWildcard, aSeparator, aCaseSensitive, aMatchWholeWord)
    //    {
    //        _searchInfo = new ProductSearchInfo(aSAB, aNodeList, aWildcard, aSeparator, aCaseSensitive,
    //            aID, aName, aDescription, aLevels, aApplyCharacteristics); //, aCharacteristicFilter);

    //        //_productSearchFilter = new ProductSearchFilter(aSAB);
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    public ProductSearchInfo SearchInfo
    //    {
    //        get { return _searchInfo; }
    //    }

    //    //public ProductSearchFilter ProductSearchFilter
    //    //{
    //    //    get { return _productSearchFilter; }
    //    //}

    //    //========
    //    // METHODS
    //    //========

    //    public override void GetSearchResults()
    //    {
    //        try
    //        {
    //            if (SearchInfo.NodeList.Count == 0)
    //            {
    //                GetAllNodes();
    //            }
    //            else
    //            {
    //                foreach (SelectedHierarchyNode shn in SearchInfo.NodeList)
    //                {
    //                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                    if (StopSearch)
    //                    {
    //                        break;
    //                    }
    //                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                    GetNodes(shn.NodeType, shn.NodeProfile.Key, shn.NodeProfile.NodeID,
    //                                shn.NodeProfile.NodeName, shn.NodeProfile.NodeDescription,
    //                                shn.NodeProfile.HomeHierarchyRID, shn.NodeProfile.HomeHierarchyLevel);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //            //MIDSearchCompletedEvent(this, new MIDSearchCompletedEventArgs());
    //            if (!StopSearch)
    //            {
    //                MIDSearchCompletedEvent(this, new MIDSearchCompletedEventArgs());
    //            }
    //            // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //        }
    //    }

    //    public void GetAllNodes()
    //    {
    //        bool search = true;
    //        MerchandiseHierarchyData hierarchyData = new MerchandiseHierarchyData();
    //        int key;
    //        string ID;
    //        string name;
    //        string description;
    //        string level;
    //        string folderColor;
    //        int homeHierarchyRID;
    //        int homeHierarchyLevel;
    //        NodeCharProfileList nodeCharProfileList;

    //        try
    //        {
    //            while (search)
    //            {
    //                DataTable dt = hierarchyData.Hierarchy_Node_Read();
    //                foreach (DataRow dr in dt.Rows)
    //                {
    //                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                    if (StopSearch)
    //                    {
    //                        break;
    //                    }
    //                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                    key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
    //                    ID = (string)dr["HN_ID"];
    //                    homeHierarchyRID = Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
    //                    homeHierarchyLevel = Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);
    //                    name = (string)dr["HN_NAME"];
    //                    description = (string)dr["DESCRIPTION"];

    //                    if (SelectItem(key, ID, name, description, homeHierarchyRID, homeHierarchyLevel, out nodeCharProfileList))
    //                    {
    //                        GetNodeInformation(homeHierarchyRID, homeHierarchyLevel, out level, out folderColor);

    //                        MIDSearchEvent(this, new MIDProductSearchEventArgs(key, ID, name, description, level, folderColor, homeHierarchyRID, homeHierarchyLevel, nodeCharProfileList, key));
    //                    }
    //                    else
    //                    {
    //                        MIDSearchEvent(this, new MIDProductSearchEventArgs(Include.NoRID, ID, name, description, null, null, -1, -1, null, key));
    //                    }
    //                    Thread.Sleep(100);
    //                }
    //                search = false;
    //            }
    //        }
    //        catch (ThreadAbortException)
    //        {
    //            MIDSearchEvent(this, new MIDProductSearchEventArgs(Include.NoRID, null, null, null, null, null, -1, -1, null, -1));
    //        }
    //        catch (Exception exc)
    //        {
    //            System.Windows.Forms.MessageBox.Show(exc.Message);
    //        }
    //    }

    //    // Begin Track #5005 - JSmith - Explorer Organization
    //    //public void GetNodes(eHierarchyNodeType aNodeType, int aNodeRID, string aID, string aName, 
    //    //    string aDescription, int aHomeHierarchyRID, int aHomeHierarchyLevel)
    //    public void GetNodes(eHierarchySelectType aNodeType, int aNodeRID, string aID, string aName,
    //        string aDescription, int aHomeHierarchyRID, int aHomeHierarchyLevel)
    //    // End Track #5005
    //    {
    //        MerchandiseHierarchyData hierarchyData = new MerchandiseHierarchyData();
    //        HierarchyLevelProfile hierarchyLevelProfile;
    //        eHierarchyLevelType levelType = eHierarchyLevelType.Undefined;
    //        HierarchyNodeList nodeList = null;
    //        int key;
    //        string ID;
    //        string name;
    //        string description;
    //        string level;
    //        string folderColor;
    //        int homeHierarchyRID;
    //        int homeHierarchyLevel;
    //        DataTable dt = null;
    //        NodeCharProfileList nodeCharProfileList;
    //        bool processRecords = false;

    //        try
    //        {
    //            switch (aNodeType)
    //            {
    //                // Begin Track #5005 - JSmith - Explorer Organization
    //                //case eHierarchyNodeType.MyHierarchyFolder:
    //                case eHierarchySelectType.MyHierarchyFolder:
    //                // End Track #5005
    //                    // Begin Track #5005 - JSmith - Explorer Organization
    //                    //nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchyNodeType.MyHierarchyRoot);
    //                    nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchySelectType.MyHierarchyRoot);
    //                    // End Track #5005
    //                    levelType = eHierarchyLevelType.Undefined;
    //                    foreach (HierarchyNodeProfile hnp in nodeList)
    //                    {
    //                        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        if (StopSearch)
    //                        {
    //                            break;
    //                        }
    //                        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        key = hnp.Key;
    //                        homeHierarchyRID = hnp.HomeHierarchyRID;
    //                        homeHierarchyLevel = hnp.HomeHierarchyLevel;

    //                        ID = hnp.NodeID;
    //                        name = hnp.NodeName;
    //                        description = hnp.NodeDescription;

    //                        // Begin Track #5005 - JSmith - Explorer Organization
    //                        //GetNodes(eHierarchyNodeType.TreeNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        GetNodes(eHierarchySelectType.HierarchyNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        // End Track #5005
    //                    }
    //                    break;
    //                // Begin Track #5005 - JSmith - Explorer Organization
    //                //case eHierarchyNodeType.OrganizationalHierarchyFolder:
    //                case eHierarchySelectType.OrganizationalHierarchyFolder:
    //                // End Track #5005
    //                    // Begin Track #5005 - JSmith - Explorer Organization
    //                    //nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchyNodeType.OrganizationalHierarchyRoot);
    //                    nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchySelectType.OrganizationalHierarchyRoot);
    //                    // End Track #5005
    //                    levelType = eHierarchyLevelType.Undefined;
    //                    foreach (HierarchyNodeProfile hnp in nodeList)
    //                    {
    //                        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        if (StopSearch)
    //                        {
    //                            break;
    //                        }
    //                        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        key = hnp.Key;
    //                        homeHierarchyRID = hnp.HomeHierarchyRID;
    //                        homeHierarchyLevel = hnp.HomeHierarchyLevel;

    //                        ID = hnp.NodeID;
    //                        name = hnp.NodeName;
    //                        description = hnp.NodeDescription;

    //                        // Begin Track #5005 - JSmith - Explorer Organization
    //                        //GetNodes(eHierarchyNodeType.TreeNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        GetNodes(eHierarchySelectType.HierarchyNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        // End Track #5005
    //                    }
    //                    break;
    //                // Begin Track #5005 - JSmith - Explorer Organization
    //                //case eHierarchyNodeType.AlternateHierarchyFolder:
    //                case eHierarchySelectType.AlternateHierarchyFolder:
    //                // End Track #5005
    //                    // Begin Track #5005 - JSmith - Explorer Organization
    //                    //nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchyNodeType.AlternateHierarchyRoot);
    //                    nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchySelectType.AlternateHierarchyRoot);
    //                    // End Track #5005
    //                    levelType = eHierarchyLevelType.Undefined;
    //                    foreach (HierarchyNodeProfile hnp in nodeList)
    //                    {
    //                        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        if (StopSearch)
    //                        {
    //                            break;
    //                        }
    //                        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                        key = hnp.Key;
    //                        homeHierarchyRID = hnp.HomeHierarchyRID;
    //                        homeHierarchyLevel = hnp.HomeHierarchyLevel;

    //                        ID = hnp.NodeID;
    //                        name = hnp.NodeName;
    //                        description = hnp.NodeDescription;

    //                        // Begin Track #5005 - JSmith - Explorer Organization
    //                        //GetNodes(eHierarchyNodeType.TreeNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        GetNodes(eHierarchySelectType.HierarchyNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                        // End Track #5005
    //                    }
    //                    break;
    //                default:
    //                    HierarchyProfile hp = GetHierarchyProfile(aHomeHierarchyRID);

    //                    // add node
    //                    if (SelectItem(aNodeRID, aID, aName, aDescription, aHomeHierarchyRID, aHomeHierarchyLevel, out nodeCharProfileList))
    //                    {
    //                        GetNodeInformation(aHomeHierarchyRID, aHomeHierarchyLevel, out level, out folderColor);

    //                        MIDSearchEvent(this, new MIDProductSearchEventArgs(aNodeRID,
    //                            aID, aName, aDescription, level, folderColor, aHomeHierarchyRID, aHomeHierarchyLevel, nodeCharProfileList, aNodeRID));
    //                    }
    //                    else
    //                    {
    //                        MIDSearchEvent(this, new MIDProductSearchEventArgs(Include.NoRID, null, null, null, null, null, -1, -1, null, aNodeRID));
    //                    }
    //                    Thread.Sleep(100);

    //                    if (hp.HierarchyType == eHierarchyType.organizational)
    //                    {
    //                        processRecords = false;
    //                        if (aHomeHierarchyLevel < SearchInfo.LowestLevel)
    //                        {
    //                            processRecords = true;
    //                            if (aHomeHierarchyLevel > 0)
    //                            {
    //                                hierarchyLevelProfile = (HierarchyLevelProfile)hp.HierarchyLevels[aHomeHierarchyLevel];
    //                                if (hierarchyLevelProfile.LevelType == eHierarchyLevelType.Style)
    //                                {
    //                                    dt = hierarchyData.Hierarchy_ColorNode_Read(aHomeHierarchyRID, aNodeRID);
    //                                    levelType = eHierarchyLevelType.Color;
    //                                }
    //                                else if (hierarchyLevelProfile.LevelType == eHierarchyLevelType.Color)
    //                                {
    //                                    dt = hierarchyData.Hierarchy_SizeNode_Read(aHomeHierarchyRID, aNodeRID);
    //                                    levelType = eHierarchyLevelType.Size;
    //                                }
    //                                else
    //                                {
    //                                    dt = hierarchyData.Hierarchy_ChildNodes_Read(aHomeHierarchyRID, aNodeRID);
    //                                    levelType = eHierarchyLevelType.Undefined;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                dt = hierarchyData.Hierarchy_ChildNodes_Read(aHomeHierarchyRID, aNodeRID);
    //                                levelType = eHierarchyLevelType.Undefined;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        dt = hierarchyData.Hierarchy_ChildNodes_Read(aHomeHierarchyRID, aNodeRID);
    //                        processRecords = true;
    //                    }

    //                    if (processRecords)
    //                    {
    //                        foreach (DataRow dr in dt.Rows)
    //                        {
    //                            // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                            if (StopSearch)
    //                            {
    //                                break;
    //                            }
    //                            // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
    //                            key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
    //                            homeHierarchyRID = Convert.ToInt32(dr["HOME_PH_RID"], CultureInfo.CurrentUICulture);
    //                            homeHierarchyLevel = Convert.ToInt32(dr["HOME_LEVEL"], CultureInfo.CurrentUICulture);
    //                            if (levelType == eHierarchyLevelType.Color)
    //                            {
    //                                // Begin TT#459 - JSmith - Hierarchy search displays unknown color
    //                                // skip unknown color
    //                                if (Convert.ToInt32(dr["COLOR_CODE_RID"]) == 0)
    //                                {
    //                                    continue;
    //                                }
    //                                // End TT#459
    //                                ID = (string)dr["COLOR_CODE_ID"];
    //                                name = (string)dr["COLOR_CODE_NAME"];
    //                                description = (string)dr["COLOR_DESCRIPTION"];
    //                            }
    //                            else if (levelType == eHierarchyLevelType.Size)
    //                            {
    //                                ID = (string)dr["SIZE_CODE_ID"];
    //                                int sizeCodeRID = (int)dr["SIZE_CODE_RID"];
    //                                SizeCodeProfile scp = (SizeCodeProfile)_sizes[sizeCodeRID];
    //                                if (scp == null)
    //                                {
    //                                    scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeRID);
    //                                    _sizes.Add(scp.Key, scp);
    //                                }
    //                                name = scp.SizeCodeName;
    //                                description = scp.SizeCodeName;
    //                            }
    //                            else
    //                            {
    //                                ID = (string)dr["HN_ID"];
    //                                name = (string)dr["HN_NAME"];
    //                                description = (string)dr["DESCRIPTION"];
    //                            }

    //                            // Begin Track #5005 - JSmith - Explorer Organization
    //                            //GetNodes(eHierarchyNodeType.TreeNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                            GetNodes(eHierarchySelectType.HierarchyNode, key, ID, name, description, homeHierarchyRID, homeHierarchyLevel);
    //                            // End Track #5005
    //                        }
    //                    }
    //                    break;
    //            }
    //        }
    //        catch (ThreadAbortException)
    //        {
    //            MIDSearchEvent(this, new MIDProductSearchEventArgs(Include.NoRID, null, null, null, null, null, -1, -1, null, -1));
    //        }

    //        //Begin TT#826 - Error produced when closing the hierarchy search while the search is running - - apicchetti - 1/19/2011
    //        catch(InvalidOperationException)
    //        {
    //            MIDSearchEvent(this, new MIDProductSearchEventArgs(Include.NoRID, null, null, null, null, null, -1, -1, null, -1));
    //        }
    //        //End TT#826 - Error produced when closing the hierarchy search while the search is running - apicchetti - 1/19/2011

    //        catch
    //        {
    //            throw;
    //        }
    //    }

    //    private void GetNodeInformation(int aHomeHierarchyRID, int aHomeHierarchyLevel,
    //                out string aLevel, out string aFolderColor)
    //    {
    //        try
    //        {
    //            HierarchyLevelProfile hierarchyLevelProfile;

    //            HierarchyProfile hp = GetHierarchyProfile(aHomeHierarchyRID);
				
    //            if (hp.HierarchyType == eHierarchyType.organizational)
    //            {
    //                if (aHomeHierarchyLevel > 0)
    //                {
    //                    hierarchyLevelProfile = (HierarchyLevelProfile)hp.HierarchyLevels[aHomeHierarchyLevel];
    //                    aLevel = hierarchyLevelProfile.LevelID;
    //                    aFolderColor = hierarchyLevelProfile.LevelColor;
    //                }
    //                else
    //                {
    //                    aLevel = hp.HierarchyID;
    //                    aFolderColor = hp.HierarchyColor;
    //                }
    //            }
    //            else
    //            {
    //                aLevel = null;
    //                aFolderColor = hp.HierarchyColor;
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }

    //    private bool SelectItem(int aKey, string aID, string aName, string aDescription,
    //        int aHomeHierarchyRID, int aHomeHierarchyLevel, out NodeCharProfileList aNodeCharProfileList)
    //    {
    //        try
    //        {
    //            aNodeCharProfileList = null;

    //            bool selectItem = true;
    //            if (SearchInfo.IDList.Count > 0)
    //            {
    //                if (!TextMatch(SearchInfo.IDList, aID))
    //                {
    //                    selectItem = false;
    //                }
    //            }

    //            if (selectItem)
    //            {
    //                if (SearchInfo.NameList.Count > 0)
    //                {
    //                    if (!TextMatch(SearchInfo.NameList, aName))
    //                    {
    //                        selectItem = false;
    //                    }
    //                }
    //            }

    //            if (selectItem)
    //            {
    //                if (SearchInfo.DescriptionList.Count > 0)
    //                {
    //                    if (!TextMatch(SearchInfo.DescriptionList, aDescription))
    //                    {
    //                        selectItem = false;
    //                    }
    //                }
    //            }

    //            // check levels if node is in organizational hierarchy
    //            if (selectItem)
    //            {
    //                HierarchyProfile hp = GetHierarchyProfile(aHomeHierarchyRID);
    //                if (hp.HierarchyType == eHierarchyType.organizational)
    //                {
    //                    if (!SearchInfo.Levels.Contains(aHomeHierarchyLevel))
    //                    {
    //                        selectItem = false;
    //                    }
    //                }
    //            }

    //            if (selectItem)
    //            {
    //                aNodeCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics(aKey);
    //                if (SearchInfo.ApplyCharacteristics)
    //                {
    //                    //if (!ProductSearchFilter.ApplyFilter(aNodeCharProfileList))
    //                    //{
    //                    //    selectItem = false;
    //                    //}
    //                }
    //            }

    //            return selectItem;
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }

    //    private HierarchyProfile GetHierarchyProfile(int aHierarchyRID)
    //    {
    //        try
    //        {
    //            HierarchyProfile hp = (HierarchyProfile)_hierarchies[aHierarchyRID];
    //            if (hp == null)
    //            {
    //                hp = _SAB.HierarchyServerSession.GetHierarchyData(aHierarchyRID);
    //                _hierarchies.Add(hp.Key, hp);
    //            }
    //            return hp;
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }
    //}

    //public class MIDProductSearchEventArgs : MIDSearchEventArgs
    //{
    //    private int _key;
    //    private string _ID;
    //    private string _name;
    //    private string _description;
    //    private string _levelName;
    //    private string _folderColor;
    //    private int _hierarchyRID;
    //    private int _hierarchyLevel;
    //    private NodeCharProfileList _nodeCharProfileList;
    //    private int _nodeRID;

    //    public MIDProductSearchEventArgs(int aKey, string aID, string aName, string aDescription,
    //        string aLevelName, string aFolderColor, int aHierarchyRID, int aHierarchyLevel,
    //        NodeCharProfileList aNodeCharProfileList, int aNodeRID)
    //    {
    //        _key = aKey;
    //        _ID = aID;
    //        _name = aName;
    //        _description = aDescription;
    //        _levelName = aLevelName;
    //        _folderColor = aFolderColor;
    //        _hierarchyRID = aHierarchyRID;
    //        _hierarchyLevel = aHierarchyLevel;
    //        _nodeCharProfileList = aNodeCharProfileList;
    //        _nodeRID = aNodeRID;
    //    }

    //    public int Key
    //    {
    //        get { return _key; }
    //    }

    //    public string ID
    //    {
    //        get { return _ID; }
    //    }

    //    public string Name
    //    {
    //        get { return _name; }
    //    }

    //    public string Description
    //    {
    //        get { return _description; }
    //    }

    //    public string LevelName
    //    {
    //        get { return _levelName; }
    //    }

    //    public string FolderColor
    //    {
    //        get { return _folderColor; }
    //    }

    //    public int HierarchyRID
    //    {
    //        get { return _hierarchyRID; }
    //    }

    //    public int HierarchyLevel
    //    {
    //        get { return _hierarchyLevel; }
    //    }

    //    public NodeCharProfileList NodeCharProfileList
    //    {
    //        get { return _nodeCharProfileList; }
    //    }

    //    public int NodeRID
    //    {
    //        get { return _nodeRID; }
    //    }
    //}
    //End TT#1388-MD -jsobek -Product Filters

	public class ColorSearchEngine : BaseSearchEngine
	{
		//=======
		// FIELDS
		//=======

		public event MIDSearchEventHandler MIDSearchEvent;
		public event MIDSearchCompletedEventHandler MIDSearchCompletedEvent;


		private ColorSearchInfo _searchInfo;
		private Hashtable _colors = new Hashtable();

		//========
		// CLASSES
		//========

		public class ColorSearchInfo
		{
			//=======
			// FIELDS
			//=======

			private SessionAddressBlock _SAB;
			private ArrayList _nodeList;
			private string _ID;
			private string _name;
			private string _groupName;
            //private Hashtable _levels;
			private ArrayList _IDList = new ArrayList();
			private ArrayList _nameList = new ArrayList();
			private ArrayList _groupList = new ArrayList();
			private char _wildcard;
			private char _separator;
			private bool _caseSensitive;

			//=============
			// CONSTRUCTORS
			//=============

			public ColorSearchInfo(
				SessionAddressBlock aSAB,
				ArrayList aNodeList,
				char aWildcard,
				char aSeparator,
				bool aCaseSensitive,
				string aID,
				string aName,
				string aGroupName)
			{
				_SAB = aSAB;
				_wildcard = aWildcard;
				_separator = aSeparator;
				_caseSensitive = aCaseSensitive;
				_nodeList = aNodeList;
				_ID = aID;
				_name = aName;
				_groupName = aGroupName;
				
				if (aID.Trim().Length > 0)
				{
					if (!_caseSensitive)
					{
						aID = aID.ToUpper();
					}
					string[] fields = MIDstringTools.Split(aID, _separator, true);
					foreach (string field in fields)
					{
						_IDList.Add(field);
					}
				}

				if (aName.Trim().Length > 0)
				{
					if (!_caseSensitive)
					{
						aName = aName.ToUpper();
					}
					string[] fields = MIDstringTools.Split(aName, _separator, true);
					foreach (string field in fields)
					{
						_nameList.Add(field);
					}
				}

				if (aGroupName.Trim().Length > 0)
				{
					if (!_caseSensitive)
					{
						aGroupName = aGroupName.ToUpper();
					}
					string[] fields = MIDstringTools.Split(aGroupName, _separator, true);
					foreach (string field in fields)
					{
						_groupList.Add(field);
					}
				}
			}

			//===========
			// PROPERTIES
			//===========

			public ArrayList NodeList
			{
				get { return _nodeList; }
			}

			public string ID
			{
				get { return _ID; }
			}

			public string Name
			{
				get { return _name; }
			}

			public ArrayList IDList
			{
				get { return _IDList; }
			}

			public ArrayList NameList
			{
				get { return _nameList; }
			}

			public ArrayList GroupList
			{
				get { return _groupList; }
			}

			//========
			// METHODS
			//========
		}

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ColorSearchEngine.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock.
		/// </param>

		public ColorSearchEngine(
			SessionAddressBlock aSAB,
			ArrayList aNodeList,
			char aWildcard,
			char aSeparator,
			bool aCaseSensitive,
			bool aMatchWholeWord,
			string aID,
			string aName,
			string aGroupName)
			: base(aSAB, aWildcard, aSeparator, aCaseSensitive, aMatchWholeWord)
		{
			_searchInfo = new ColorSearchInfo(aSAB, aNodeList, aWildcard, aSeparator, aCaseSensitive,
				aID, aName, aGroupName);
		}

		//===========
		// PROPERTIES
		//===========

		public ColorSearchInfo SearchInfo
		{
			get { return _searchInfo; }
		}

		//========
		// METHODS
		//========

		public override void GetSearchResults()
		{
			try
			{
				GetAllColors();
			}
			catch
			{
				throw;
			}
			finally
			{
				MIDSearchCompletedEvent(this, new MIDSearchCompletedEventArgs());
			}
		}

		public void GetAllColors()
		{
			bool search = true;
			ColorData colorData = new ColorData();
			int key;
			string ID;
			string name;
			string groupName;

			try
			{
				string unassignedGroupName = MIDText.GetTextOnly(eMIDTextCode.Unassigned);
				while (search)
				{
					DataTable dt = colorData.Colors_Read();
					foreach (DataRow dr in dt.Rows)
					{
                        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                        if (StopSearch)
                        {
                            break;
                        }
                        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
						key = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
						ID = (string)dr["COLOR_CODE_ID"];
						name = (string)dr["COLOR_CODE_NAME"];
						groupName = (string)dr["COLOR_CODE_GROUP"];
						if (groupName.Trim().Length == 0)
						{
							groupName = unassignedGroupName;
						}
						if (SelectItem(key, ID, name, groupName))
						{
							MIDSearchEvent(this, new MIDColorSearchEventArgs(key, ID, name, groupName));
							Thread.Sleep(100);
						}
					}
					search = false;
				}
			}
			catch (ThreadAbortException)
			{
				MIDSearchEvent(this, new MIDColorSearchEventArgs(-1, null, null, null));
			}
			catch (Exception exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
		}

		private bool SelectItem(int aKey, string aID, string aName, string aGroupName)
		{
			try
			{
				bool selectItem = true;
				if (SearchInfo.IDList.Count > 0)
				{
					if (!TextMatch(SearchInfo.IDList, aID))
					{
						selectItem = false;
					}
				}

				if (selectItem)
				{
					if (SearchInfo.NameList.Count > 0)
					{
						if (!TextMatch(SearchInfo.NameList, aName))
						{
							selectItem = false;
						}
					}
				}

				if (selectItem)
				{
					if (SearchInfo.GroupList.Count > 0)
					{
						if (!TextMatch(SearchInfo.GroupList, aGroupName))
						{
							selectItem = false;
						}
					}
				}

				return selectItem;
			}
			catch
			{
				throw;
			}
		}

		private ColorCodeProfile GetColorCodeProfile(int aColorCodeRID)
		{
			try
			{
				ColorCodeProfile ccp = (ColorCodeProfile)_colors[aColorCodeRID];
				if (ccp == null)
				{
					ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(aColorCodeRID);
					_colors.Add(ccp.Key, ccp);
				}
				return ccp;
			}
			catch
			{
				throw;
			}
		}
	}

	public class MIDColorSearchEventArgs : MIDSearchEventArgs
	{
		private int _key;
		private string _ID;
		private string _name;
		private string _groupName;

		public MIDColorSearchEventArgs(int aKey, string aID, string aName, string aGroupName)
		{
			_key = aKey;
			_ID = aID;
			_name = aName;
			_groupName = aGroupName;
		}

		public int Key
		{
			get { return _key; }
		}

		public string ID
		{
			get { return _ID; }
		}

		public string Name
		{
			get { return _name; }
		}

		public string GroupName
		{
			get { return _groupName; }
		}
	}

	public class ProductCharSearchEngine : BaseSearchEngine
	{
		//=======
		// FIELDS
		//=======

		public event MIDSearchEventHandler MIDSearchEvent;
		public event MIDSearchCompletedEventHandler MIDSearchCompletedEvent;


		private ProductCharSearchInfo _searchInfo;
		private Hashtable _ProductChars = new Hashtable();

		//========
		// CLASSES
		//========

		public class ProductCharSearchInfo
		{
			//=======
			// FIELDS
			//=======

			private SessionAddressBlock _SAB;
			private ArrayList _nodeList;
			private string _value;
			private string _characteristic;
			private ArrayList _valueList = new ArrayList();
			private ArrayList _characteristicList = new ArrayList();
			private char _wildcard;
			private char _separator;
			private bool _caseSensitive;

			//=============
			// CONSTRUCTORS
			//=============

			public ProductCharSearchInfo(
				SessionAddressBlock aSAB,
				ArrayList aNodeList,
				char aWildcard,
				char aSeparator,
				bool aCaseSensitive,
				string aValue,
				string aCharacteristic)
			{
				_SAB = aSAB;
				_wildcard = aWildcard;
				_separator = aSeparator;
				_caseSensitive = aCaseSensitive;
				_nodeList = aNodeList;
				_value = aValue;
				_characteristic = aCharacteristic;

				if (aValue.Trim().Length > 0)
				{
					if (!_caseSensitive)
					{
						aValue = aValue.ToUpper();
					}
					string[] fields = MIDstringTools.Split(aValue, _separator, true);
					foreach (string field in fields)
					{
						_valueList.Add(field);
					}
				}

				if (aCharacteristic.Trim().Length > 0)
				{
					if (!_caseSensitive)
					{
						aCharacteristic = aCharacteristic.ToUpper();
					}
					string[] fields = MIDstringTools.Split(aCharacteristic, _separator, true);
					foreach (string field in fields)
					{
						_characteristicList.Add(field);
					}
				}
			}

			//===========
			// PROPERTIES
			//===========

			public ArrayList NodeList
			{
				get { return _nodeList; }
			}

			public string Value
			{
				get { return _value; }
			}

			public string Characteristic
			{
				get { return _characteristic; }
			}

			public ArrayList ValueList
			{
				get { return _valueList; }
			}

			public ArrayList CharacteristicList
			{
				get { return _characteristicList; }
			}

			//========
			// METHODS
			//========
		}

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProductCharSearchEngine.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock.
		/// </param>

		public ProductCharSearchEngine(
			SessionAddressBlock aSAB,
			ArrayList aNodeList,
			char aWildcard,
			char aSeparator,
			bool aCaseSensitive,
			bool aMatchWholeWord,
			string aValue,
			string aCharacteristic)
			: base(aSAB, aWildcard, aSeparator, aCaseSensitive, aMatchWholeWord)
		{
			_searchInfo = new ProductCharSearchInfo(aSAB, aNodeList, aWildcard, aSeparator, aCaseSensitive,
				aValue, aCharacteristic);
		}

		//===========
		// PROPERTIES
		//===========

		public ProductCharSearchInfo SearchInfo
		{
			get { return _searchInfo; }
		}

		//========
		// METHODS
		//========

		public override void GetSearchResults()
		{
			try
			{
				GetAllProductChars();
			}
			catch
			{
				throw;
			}
			finally
			{
				MIDSearchCompletedEvent(this, new MIDSearchCompletedEventArgs());
			}
		}

		public void GetAllProductChars()
		{
			bool search = true;
			MerchandiseHierarchyData data = new MerchandiseHierarchyData();
			int valueKey;
			string value;
			int characteristicKey;
			string characteristic;

			try
			{
				while (search)
				{
					DataTable dt = data.Hierarchy_Char_Read();
					foreach (DataRow dr in dt.Rows)
					{
                        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
                        if (StopSearch)
                        {
                            break;
                        }
                        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
						valueKey = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);
						value = (string)dr["HC_ID"];
						characteristicKey = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
						characteristic = (string)dr["HCG_ID"];

						if (SelectItem(valueKey, value, characteristic))
						{

							MIDSearchEvent(this, new MIDProductCharSearchEventArgs(valueKey, value, characteristicKey, characteristic));
							Thread.Sleep(100);
						}
					}
					search = false;
				}
			}
			catch (ThreadAbortException)
			{
				MIDSearchEvent(this, new MIDProductCharSearchEventArgs(-1, null, -1, null));
			}
			catch (Exception exc)
			{
				System.Windows.Forms.MessageBox.Show(exc.Message);
			}
		}

		private bool SelectItem(int aKey, string aValue, string aCharacteristic)
		{
			try
			{
				bool selectItem = true;
				if (SearchInfo.ValueList.Count > 0)
				{
					if (!TextMatch(SearchInfo.ValueList, aValue))
					{
						selectItem = false;
					}
				}

				if (selectItem)
				{
					if (SearchInfo.CharacteristicList.Count > 0)
					{
						if (!TextMatch(SearchInfo.CharacteristicList, aCharacteristic))
						{
							selectItem = false;
						}
					}
				}

				return selectItem;
			}
			catch
			{
				throw;
			}
		}

		private ProductCharValueProfile GetProductCharValueProfile(int aProductCharValueRID)
		{
			try
			{
				return _SAB.HierarchyServerSession.GetProductCharValueProfile(aProductCharValueRID);
			}
			catch
			{
				throw;
			}
		}
	}

	public class MIDProductCharSearchEventArgs : MIDSearchEventArgs
	{
		private int _valueKey;
		private string _value;
		private int _characteristicKey;
		private string _characteristic;

		public MIDProductCharSearchEventArgs(int aValueKey, string aValue, int CharacteristicKey, string aCharacteristic)
		{
			_valueKey = aValueKey;
			_value = aValue;
			_characteristicKey = CharacteristicKey;
			_characteristic = aCharacteristic;
		}

		public int ValueKey
		{
			get { return _valueKey; }
		}

		public string Value
		{
			get { return _value; }
		}

		public int CharacteristicKey
		{
			get { return _characteristicKey; }
		}

		public string Characteristic
		{
			get { return _characteristic; }
		}
	}
}
