using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    /// <summary>
    /// HierarchyServerSession is a class that contains fields, properties, and methods that are available to other sessions
    /// of the system.
    /// </summary>
    /// <remarks>
    /// The HierarchyServerSession class is the interface to the HierarchyServer functionality.  All requests for functionality
    /// or information in the HierarchyServer should be made through methods and properties in this class.
    /// </remarks>
    public class HierarchyServerSessionRemote : SessionRemote
	{
		private MerchandiseHierarchyData _mhd = null;
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        private ChainSetPercentCriteriaData _cspcd = null;
        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        private DailyPercentagesCriteriaData _dpcd = null; // TT#43 - MD - DOConnell - Projected Sales Enhancement
		private SizeGroup _sgd = null;
		private ColorData _cd = null;
		private int _userID = Include.NoRID;
        private Dictionary<eProfileType, ProfileList> _profileHash;
        private int _maxStoreRID = 0;
        private Dictionary<int, HierarchyNodeProfile> _availableNodeList;
        private Dictionary<int, HierarchyNodeSecurityProfile> _nodeSecurityAssignments;
        private bool _LookupIsHierarchyAdministrator = true;
		private bool _isHierarchyAdministrator = false;
		// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
		private char _nodeDelimiter;
		// End Track #5307
        // Begin Track #5247 - JSmith - null reference during QuickAdd
        private HierarchyProfile _hp;
        // End Track #5247
        private int _auditProcessRID = Include.NoRID; // TT#739-MD - JSmith - Delete Stores
        // Begin TT#4988 - JSmith - Performance
        private Dictionary<int, Dictionary<int, eStoreStatus>> _storeSalesStatusHash;
        private Dictionary<int, Dictionary<int, eStoreStatus>> _storeStockStatusHash;
        // End TT#4988 - JSmith - Performance
		
		/// <summary>
		/// Creates a new instance of HierarchySessionGlobal as either local or remote, depending on the value of aLocal
		/// </summary>
		/// <param name="aLocal">
		/// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
		/// </param>

		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public HierarchyServerSession(bool aLocal)
		public HierarchyServerSessionRemote(bool aLocal)
		//End TT#708 - JScott - Services need a Retry availalbe.
			: base(aLocal)
		{
			_mhd = new MerchandiseHierarchyData();  // create data layer object for the session
			_sgd = new SizeGroup();  // create data layer object for the session
			_cd = new ColorData();  // create data layer object for the session
            _profileHash = new Dictionary<eProfileType, ProfileList>();
            _availableNodeList = new Dictionary<int, HierarchyNodeProfile>();
            _nodeSecurityAssignments = new Dictionary<int, HierarchyNodeSecurityProfile>();
            // Begin Track #5307 - JSmith - duplicate nodes on hierarchy
			_nodeDelimiter = GetProductLevelDelimiter();
			// End Track #5307
            //StoreMgmt.LoadInitialStoresAndGroups(SessionAddressBlock);
           
		}

		// Session Properties
		public int UserID
		{
			get
			{
				if (_userID == Include.NoRID)
				{
					_userID = SessionAddressBlock.ClientServerSession.UserRID;
				}
				return _userID;
			}
		}

		public string GetQualifiedNodeID(int aNodeRID)
		{
			try
			{
				return GetQualifiedNodeID(aNodeRID, false, string.Empty);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				throw;
			}
		}

		public string GetQualifiedNodeID(int aNodeRID, string aOverrideDelimeter)
		{
			try
			{
				return GetQualifiedNodeID(aNodeRID, true, aOverrideDelimeter);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				throw;
			}
		}

		private string GetQualifiedNodeID(int aNodeRID, bool aOverrideDelimeter, string aDelimeterString)
		{
			HierarchyNodeProfile hnp;
			NodeAncestorList nal;
			NodeAncestorProfile nap;
			NodeInfo ni;
			int ancestor;
			string nodeID = string.Empty;

			try
			{
				hnp = HierarchyServerGlobal.GetNodeData(aNodeRID, false);
				
				if (hnp.LevelType == eHierarchyLevelType.Color ||
					hnp.LevelType == eHierarchyLevelType.Size)
				{
					nal = GetNodeAncestorList(aNodeRID, hnp.HomeHierarchyRID);
					nodeID = hnp.NodeID;

					for (ancestor = 1; ancestor < nal.Count; ancestor++)
					{
						nap = (NodeAncestorProfile)nal[ancestor];
						ni = HierarchyServerGlobal.GetNodeInfoByRID(nap.Key, false);

						if (aOverrideDelimeter)
						{
							nodeID = ni.NodeID + aDelimeterString + nodeID;
						}
						else
						{
							nodeID = ni.NodeID + GlobalOptions.ProductLevelDelimiter + nodeID;
						}

						if (ni.LevelType == eHierarchyLevelType.Style)
						{
							break;
						}
					}
				}
				else
				{
					nodeID = hnp.NodeID;
				}

				return nodeID;
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				throw;
			}
		}

		public bool IsHierarchyAdministrator
		{
			get
			{
				if (_LookupIsHierarchyAdministrator)
				{
					_LookupIsHierarchyAdministrator = false;
					if (UserID == Include.AdministratorUserRID)
					{
						_isHierarchyAdministrator = true;
					}
				}
				return _isHierarchyAdministrator;
			}
		}

		public MerchandiseHierarchyData MHD
		{
			get
			{
				if (_mhd == null)
				{
					_mhd = new MerchandiseHierarchyData();
				}
				return _mhd;
			}
		}

        // Begin TT#4988 - JSmith - Performance
        public Dictionary<int, Dictionary<int, eStoreStatus>> StoreSalesStatusHash
        {
            get
            {
                if (_storeSalesStatusHash == null)
                {
                    _storeSalesStatusHash = new Dictionary<int, Dictionary<int, eStoreStatus>>();
                }
                return _storeSalesStatusHash;
            }
            set
            {
                _storeSalesStatusHash = value;
            }
        }

        public Dictionary<int, Dictionary<int, eStoreStatus>> StoreStockStatusHash
        {
            get
            {
                if (_storeStockStatusHash == null)
                {
                    _storeStockStatusHash = new Dictionary<int, Dictionary<int, eStoreStatus>>();
                }
                return _storeStockStatusHash;
            }
            set
            {
                _storeStockStatusHash = value;
            }
        }
        // End TT#4988 - JSmith - Performance

		// Session Methods

		/// <summary>
		/// Initializes the session.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			Calendar = HierarchyServerGlobal.Calendar;
			CreateAudit();
            // Begin TT#739-MD - JSmith - Delete Stores
            if (Audit != null)
            {
                _auditProcessRID = Audit.ProcessRID;
            }
            // End TT#739-MD - JSmith - Delete Stores
			Calendar.SetPostingDate( this.GetPostingDate() );
            // Begin TT#1861-MD - JSmith - Serialization error accessing the Audit
            //StoreMgmt.LoadInitialStoresAndGroups(SessionAddressBlock);
			// Begin TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
            //StoreMgmt.LoadInitialStoresAndGroups(SessionAddressBlock, SessionAddressBlock.HierarchyServerSession);
			// End TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
            // End TT#1861-MD - JSmith - Serialization error accessing the Audit
            // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
            if (SessionAddressBlock.StoreServerSession != null)
            {
                ProfileList storeList = GetProfileList(eProfileType.Store);
            }
            // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
            // Begin TT#1808-MD - JSmith - Store Load Error
            ExceptionHandler.Initialize(SessionAddressBlock.HierarchyServerSession, false);
            // End TT#1808-MD - JSmith - Store Load Error
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                HierarchyServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

		/// <summary>
		/// Identifies resources to release as the session expires.
		/// </summary>
		protected override void ExpiredCleanup()
		{
			try
			{
                // Begin TT#739-MD - JSmith - Delete Stores
                if (_auditProcessRID != Include.NoRID)
                {
                    AuditData auditData = new AuditData();
                    auditData.CloseAuditHeaderIfUnexpected(_auditProcessRID);
                }
                // End TT#739-MD - JSmith - Delete Stores

                // Begin TT#1243 - JSmith - Audit Performance
                base.ExpiredCleanup();
                // End TT#1243

				if (_mhd != null)
				{
					if (_mhd.ConnectionIsOpen)
					{
						CloseUpdateConnection();
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				HierarchyServerGlobal.GarbageCollect();
			}

		}

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public override void CloseSession()
        {
            try
            {
                base.CloseSession();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flush buffer and close audit
        /// </summary>
        public override void CloseAudit()
        {
            try
            {
                base.CloseAudit();
                HierarchyServerGlobal.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

        // Begin TT#1440 - JSmith - Memory Issues
        override public void CleanUpSession()
        {
            _mhd = null;
            _sgd = null;
            _cd = null;
            if (_profileHash != null)
            {
                _profileHash.Clear();
                _profileHash = null;
            }
            if (_availableNodeList != null)
            {
                _availableNodeList.Clear();
                _availableNodeList = null;
            }
            if (_nodeSecurityAssignments != null)
            {
                _nodeSecurityAssignments.Clear();
                _nodeSecurityAssignments = null;
            }
            // Begin TT#4988 - JSmith - Performance
            if (_storeSalesStatusHash != null)
            {
                _storeSalesStatusHash.Clear();
                _storeSalesStatusHash = null;
            }
            if (_storeStockStatusHash != null)
            {
                _storeStockStatusHash.Clear();
                _storeStockStatusHash = null;
            }
            // End TT#4988 - JSmith - Performance
            _hp = null;

            base.CleanUpSession();
        }
        // End TT#1440

		/// <summary>
		/// Clean up the global resources
		/// </summary>
		public void CleanUpGlobal()
		{
			try
			{
				HierarchyServerGlobal.CleanUp();
			}
			catch
			{
				throw;
			}
		}

        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        public void ClearCache(Dictionary<int, Dictionary<long, object>> nodeChanges)
        {
            try
            {
                HierarchyServerGlobal.ClearCache(nodeChanges);
            }
            catch
            {
                throw;
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels

		//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
		public void ClearNodeCache()
		{
			try
			{
				HierarchyServerGlobal.ClearNodeCache();
			}
			catch
			{
				throw;
			}
		}

		//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model

        // Begin TT#4988 - JSmith - Performance
        public Dictionary<int, eStoreStatus> GetStoreSalesStatusHash(int aFirstDayOfWeek)
        {
            try
            {

                Dictionary<int, eStoreStatus> yearWeekHash;
                if (!StoreSalesStatusHash.TryGetValue(aFirstDayOfWeek, out yearWeekHash))
                {
                    //yearWeekHash = SessionAddressBlock.StoreServerSession.GetStoreSalesStatusHash(aFirstDayOfWeek);
                    // Begin TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                    //yearWeekHash = StoreMgmt.GetStoreSalesStatusHash(aFirstDayOfWeek, SessionAddressBlock.HierarchyServerSession);
                    yearWeekHash = StoreMgmt.GetStoreSalesStatusHash(aFirstDayOfWeek, SessionAddressBlock.HierarchyServerSession, SessionAddressBlock, GetProfileList(eProfileType.Store));
                    // End TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                    StoreSalesStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
                }
                return yearWeekHash;
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<int, eStoreStatus> GetStoreStockStatusHash(int aFirstDayOfWeek)
        {
            try
            {
                Dictionary<int, eStoreStatus> yearWeekHash;
                if (!StoreStockStatusHash.TryGetValue(aFirstDayOfWeek, out yearWeekHash))
                {
                    //yearWeekHash = SessionAddressBlock.StoreServerSession.GetStoreStockStatusHash(aFirstDayOfWeek);
                    // Begin TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                    //yearWeekHash = StoreMgmt.GetStoreStockStatusHash(aFirstDayOfWeek, SessionAddressBlock.HierarchyServerSession);
                    yearWeekHash = StoreMgmt.GetStoreStockStatusHash(aFirstDayOfWeek, SessionAddressBlock.HierarchyServerSession, SessionAddressBlock, GetProfileList(eProfileType.Store));
                    // End TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                    StoreStockStatusHash.Add(aFirstDayOfWeek, yearWeekHash);
                }
                return yearWeekHash;
            }
            catch
            {
                throw;
            }
        }
        // End TT#4988 - JSmith - Performance
		/// <summary>
		/// Reads the hierarchies from the database and rebuilds the hierarchies by user.  
		/// </summary>
		/// 
		public void ReBuildUserHierarchies()
		{
			try
			{
				HierarchyServerGlobal.ReBuildUserHierarchies();
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Request that global rebuild the color and size IDs
		/// </summary>
		/// <param name="aHierarchyLevelType">
		/// Identifies the type of level for the IDs to be loaded.
		/// </param>
		public void BuildColorSizeIDs(eHierarchyLevelType aHierarchyLevelType)
		{
			try
			{
				HierarchyServerGlobal.BuildColorSizeIDs(aHierarchyLevelType);
			}
			catch
			{
				throw;
			}
		}

		public bool ColorSizesCacheUsed()
		{
			return HierarchyServerGlobal.ColorSizesCacheUsed();
		}

		/// <summary>
		/// Dump memory to event viewer
		/// </summary>
		public string ShowMemory()
		{
			try
			{
				return HierarchyServerGlobal.ShowMemory();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Clears all cached areas in the session.
		/// </summary>
		public void Refresh()
		{
			try
			{
				if (_profileHash != null)
				{
                    _profileHash = new Dictionary<eProfileType, ProfileList>();
				}
                // Begin TT#4988 - JSmith - Performance
                if (_storeSalesStatusHash != null)
                {
                    _storeSalesStatusHash.Clear();
                }
                if (_storeStockStatusHash != null)
                {
                    _storeStockStatusHash.Clear();
                }
                // End TT#4988 - JSmith - Performance

                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                if (SessionAddressBlock.StoreServerSession != null)
                {
                    ProfileList storeList = GetProfileList(eProfileType.Store);
                }
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
				RefreshBase();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Return flag identifying if update connection is open
		/// </summary>
		public bool UpdateConnectionIsOpen()
		{
			try
			{
				if (_mhd == null ||
					!_mhd.ConnectionIsOpen)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Opens an update connection for the hierarchy data layer.
		/// </summary>
		public void OpenUpdateConnection()
		{
			try
			{
				if (!_mhd.ConnectionIsOpen)
				{
					_mhd.OpenUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Opens an update connection for the hierarchy data layer and locks the value of lockID.
		/// </summary>
		/// <param name="lockType">
		/// The type of lock>
		/// </param>
		/// <param name="lockID">
		/// The string to lock against
		/// </param>
		public void OpenUpdateConnection(eLockType lockType, string lockID)
		{
			try
			{
				_mhd.OpenUpdateConnection(lockType, lockID);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Commits data for the database transaction for the hierarchy data layer.
		/// </summary>
		public void CommitData()
		{
			try
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CommitData();
					CloseUpdateConnection();
				}
				OpenUpdateConnection();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Closes an update connection for the hierarchy data layer.
		/// </summary>
		public void CloseUpdateConnection()
		{
			try
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Rolls back the data for the database transaction.
		/// </summary>
		public void Rollback()
		{
			try
			{
				_mhd.Rollback();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}

		}

		/// <summary>
		/// Creates and returns a new Transaction object.
		/// </summary>
		/// <returns>
		/// The newly created Transaction object that points to this Session.
		/// </returns>
		public HierarchySessionTransaction CreateTransaction()
		{
			try
			{
				return new HierarchySessionTransaction(SessionAddressBlock);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void BuildAvailableNodeList()
		{
			try
			{
                _availableNodeList = new Dictionary<int, HierarchyNodeProfile>();
                _nodeSecurityAssignments = new Dictionary<int, HierarchyNodeSecurityProfile>();
				//			ArrayList securityList = SessionAddressBlock.ClientServerSession.Security.GetUserNodesAssignment(UserID);
				HierarchyNodeSecurityProfileList securityList = SessionAddressBlock.ClientServerSession.GetUserNodesSecurityAssignment(UserID, (int)eSecurityTypes.All);
				HierarchyProfile mainHierarchy = HierarchyServerGlobal.GetMainHierarchyData();
				foreach (HierarchyNodeSecurityProfile hnsp in securityList)
				{
					// add node to security list
                    if (!_nodeSecurityAssignments.ContainsKey(hnsp.Key))
                    {
						_nodeSecurityAssignments.Add(hnsp.Key, hnsp);
					}
					if (hnsp.AllowUpdate || hnsp.AllowView)
					{
						HierarchyNodeProfile hnp = GetNodeData(hnsp.Key, false);
						// add node to available list
                        if (!_availableNodeList.ContainsKey(hnsp.Key))
                        {
							_availableNodeList.Add(hnsp.Key, null);
						}
						// If main hierarchy, Get list of ancestors for each node
						if (hnp.HomeHierarchyRID == mainHierarchy.Key)
						{
							NodeAncestorList nal = GetNodeAncestorList(hnsp.Key, hnp.HomeHierarchyRID);
							foreach (NodeAncestorProfile nap in nal)
							{
                                if (!_availableNodeList.ContainsKey(nap.Key))
                                {
									_availableNodeList.Add(nap.Key, null);
								}
							}
						}
					}
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void AddToSecurityNodeList(int nodeRID)
		{
			try
			{
				_nodeSecurityAssignments.Add(nodeRID, null);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		public ArrayList GetMyAllocationStyles(ArrayList aStyles)
		{
			try
			{
                Dictionary<int, HierarchyNodeSecurityProfile> allocationNodeList = new Dictionary<int,HierarchyNodeSecurityProfile>();
                HierarchyNodeSecurityProfileList securityList = SessionAddressBlock.ClientServerSession.GetUserNodesSecurityAssignment(UserID, (int)eSecurityTypes.Allocation);
				HierarchyProfile mainHierarchy = HierarchyServerGlobal.GetMainHierarchyData();
				foreach (HierarchyNodeSecurityProfile hnsp in securityList)
				{
					if (hnsp.AllowUpdate || hnsp.AllowView)
					{
						// add node to list
                        if (!allocationNodeList.ContainsKey(hnsp.Key))
                        {
							allocationNodeList.Add(hnsp.Key, null);
						}
					}
				}

				ArrayList nodeSecurity = new ArrayList();
				
				bool addNode = false;
				foreach (int nodeRID in aStyles)
				{
					addNode = false;
                    if (allocationNodeList.ContainsKey(nodeRID) || IsHierarchyAdministrator)
                    {
						addNode = true;
					}
					else
					{
						ArrayList ancestorRIDs = HierarchyServerGlobal.GetNodeAncestorRIDs(nodeRID, mainHierarchy.Key);
						foreach (int ancestorRID in ancestorRIDs)
						{
                            if (allocationNodeList.ContainsKey(ancestorRID))
                            {
								addNode = true;
								break;
							}
						}
					}
					if (addNode)
					{
						nodeSecurity.Add(nodeRID);
					}
				}

				return nodeSecurity;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		//Begin TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.
		public string GetSizeCurveCriteriaProfileCurveName(HierarchyNodeProfile aNodeProf, SizeCurveCriteriaProfile aSccp)
		{
			string name;

			try
			{
				name = HierarchyServerGlobal.BuildQualifiedNode(aNodeProf);

				if (aSccp.CriteriaCurveName.Length > 0)
				{
					name += "-" + aSccp.CriteriaCurveName;
				}

				return name;
			}
			catch
			{
				throw;
			}
		}
		//End TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.

        // Begin TT#487 - RMatelic - Size Curve Node Properties Default setting.  When using the Size Curve method with the default set not getting expected results.
        public HierarchyNodeProfile GetDefaultSizeCurveCriteriaNode(HierarchyNodeProfile aNodeProf, SizeCurveCriteriaProfile aSccp, int aHdrColorCount)
        {
            HierarchyNodeProfile hnp = null;
            NodeAncestorList nal;
			//Begin TT#806 - JScott - Alternate size default is not working in Allocation
			int fromNodeIdx;
			int nodeIdx;
			NodeAncestorProfile nap;

			//End TT#806 - JScott - Alternate size default is not working in Allocation
			try
            {
                switch (aSccp.CriteriaLevelType)
                {
                    case eLowLevelsType.HierarchyLevel:
                        if (aNodeProf.HomeHierarchyLevel == aSccp.CriteriaLevelSequence)
                        {
                            hnp = aNodeProf;
                        }
                        else if (aSccp.CriteriaLevelSequence < aNodeProf.HomeHierarchyLevel)
                        {
                            hnp = GetAncestorDataByLevel(aNodeProf.HierarchyRID, aNodeProf.Key, aSccp.CriteriaLevelSequence);
                        }
                        break;

                    case eLowLevelsType.LevelOffset:
                        // Begin TT#5284 - JSmith - Default Size Curve node not found for header
                        //ArrayList al = GetAllNodeAncestorLists(aNodeProf.Key);
                        //nal = GetNodeAncestorList(aNodeProf.Key, aSccp.CriteriaLevelRID);
                        nal = GetNodeAncestorList(aNodeProf.Key, aSccp.CriteriaLevelRID, eHierarchySearchType.SpecificHierarchy);
                        // End TT#5284 - JSmith - Default Size Curve node not found for header
						//Begin TT#806 - JScott - Alternate size default is not working in Allocation
						//int offSet = (nal.ArrayList.Count - 1) - aSccp.CriteriaLevelOffset;
						//if (offSet >= 0)
						//{
						//    NodeAncestorProfile nap = (NodeAncestorProfile)nal.ArrayList[offSet];
						//    hnp = GetNodeData(nap.Key);
						//}

						if (aSccp.CriteriaInheritedFromNodeRID != Include.NoRID)
						{
							for (fromNodeIdx = nal.ArrayList.Count - 1; fromNodeIdx >= 0; fromNodeIdx--)
							{
								nap = (NodeAncestorProfile)nal.ArrayList[fromNodeIdx];

								if (nap.Key == aSccp.CriteriaInheritedFromNodeRID)
								{
									break;
								}
							}
						}
						else
						{
							fromNodeIdx = 0;
						}

						if (fromNodeIdx >= 0)
						{
							nodeIdx = fromNodeIdx - aSccp.CriteriaLevelOffset;

							if (nodeIdx >= 0 && nodeIdx < nal.ArrayList.Count)
							{
								nap = (NodeAncestorProfile)nal.ArrayList[nodeIdx];
								hnp = GetNodeData(nap.Key);
							}
						}

						//Begin TT#806 - JScott - Alternate size default is not working in Allocation
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
            return hnp;
        }
        // End TT#487

        /// <summary>
		/// This method will retrieve the current ProfileList stored in this session.  If the ProfileList has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList to retieve.
		/// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>

		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				ProfileList profileList;

                if (!_profileHash.TryGetValue(aProfileType, out profileList))
				{
					switch (aProfileType)
					{
						case eProfileType.Store:

                            // Begin TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
							//profileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SessionAddressBlock.StoreServerSession.GetActiveStoresList();
                            profileList = StoreProfiles_GetActiveStoresList(); 
							// End TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
							_profileHash.Add(profileList.ProfileType, profileList);
							_maxStoreRID = profileList.MaxValue;

							break;

						case eProfileType.StoreGroup:

							profileList = new ProfileList(eProfileType.StoreGroup);

							// TODO: Load StoreGroup Profiles with dummy values for now.  Add retrieval from StoreSession
							profileList.Add(new StoreGroupProfile(0));
							// End TODO

							_profileHash.Add(profileList.ProfileType, profileList);

							break;
					}
				}

				return profileList;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

        // Begin TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
		public ProfileList StoreProfiles_GetActiveStoresList()
        {
            try
            {
                ProfileList allStoresList = SessionAddressBlock.StoreServerSession.GetAllStoresList();
                ProfileList activeList = new ProfileList(eProfileType.Store);
                foreach (StoreProfile sp in allStoresList)
                {
                    if (sp.ActiveInd)
                    {
                        activeList.Add(sp);
                    }
                }
                activeList.ArrayList.Sort();  
                return activeList;
            }
            catch (Exception err)
            {
                throw;
            }
        }
		// End TT#1872-MD - JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.

		public char GetProductLevelDelimiter()
		{
			try
			{
				return HierarchyServerGlobal.GetProductLevelDelimiter();
			}
			catch
			{
				throw;
			}
		}

		public bool AlternateAPIRollupExists()
		{
			try
			{
				return HierarchyServerGlobal.AlternateAPIRollupExists();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session update the users personal hierarchy information to the hierarchy global area
		/// </summary>
		/// <param name="hp">An instance of the HierarchyProfile containing data used to update a hierarchy's definition</param>
		/// <returns></returns>
        // Begin TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
        //public HierarchyProfile HierarchyUpdate(HierarchyProfile hp)
        // Begin TT#1911-MD - JSmith - Database Error on Update
        //public HierarchyProfile HierarchyUpdate(HierarchyProfile hp, bool updateLevels)
        public HierarchyProfile HierarchyUpdate(HierarchyProfile hp)
        // End TT#1911-MD - JSmith - Database Error on Update
        // End TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
        {
			try
			{
				HierarchyLevelProfile hlp;
				switch (hp.HierarchyChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_mhd.OpenUpdateConnection();
						int hierarchyRID = _mhd.Hierarchy_Add(hp.HierarchyID, hp.HierarchyType, hp.HierarchyRollupOption, hp.HierarchyColor, hp.Owner, hp.OTSPlanLevelType);
						hp.Key = hierarchyRID;
						int	OTSPlanLevelHierarchyRID = Include.NoRID;
						int	OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
						int	OTSPlanLevelAnchorNode = Include.NoRID;
						eMaskField maskField = eMaskField.Undefined;
						string OTSPlanLevelMask = null;
						bool OTSPlanLevelIsOverridden = false;
						if (hp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined)
						{
							OTSPlanLevelHierarchyRID = hp.Key;
							OTSPlanLevelHierarchyLevelSequence = hp.OTSPlanLevelHierarchyLevelSequence;
							OTSPlanLevelIsOverridden = true;
							OTSPlanLevelAnchorNode = hp.HierarchyRootNodeRID;
						}
                        // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                        //int hierarchyNodeRID = _mhd.Hierarchy_Node_Add(hierarchyRID, 0, hierarchyRID, 0, 
                        //    eHierarchyLevelType.Undefined, false, eOTSPlanLevelType.Undefined, false, 
                        //    OTSPlanLevelIsOverridden, ePlanLevelSelectType.HierarchyLevel,
                        //    ePlanLevelLevelType.HierarchyLevel, OTSPlanLevelHierarchyRID, OTSPlanLevelHierarchyLevelSequence,
                        //    OTSPlanLevelAnchorNode, maskField, OTSPlanLevelMask, false, ePurpose.Default);
                        //_mhd.Hierarchy_BasicNode_Add(hierarchyNodeRID, hp.HierarchyID, hp.HierarchyID, 
                        //    hp.HierarchyID, false, eProductType.Undefined);
                        int hierarchyNodeRID = _mhd.Hierarchy_BasicNode_Add(hierarchyRID, 0, hierarchyRID, 0,
                                                     eHierarchyLevelType.Undefined, false, eOTSPlanLevelType.Undefined, false,
                                                     OTSPlanLevelIsOverridden, ePlanLevelSelectType.HierarchyLevel,
                                                     ePlanLevelLevelType.HierarchyLevel, OTSPlanLevelHierarchyRID, OTSPlanLevelHierarchyLevelSequence,
                                                     OTSPlanLevelAnchorNode, maskField, OTSPlanLevelMask, false, ePurpose.Default, hp.HierarchyID, hp.HierarchyID,
                                                     hp.HierarchyID, false, eProductType.Undefined);
                        // End TT#1453
						hp.HierarchyRootNodeRID = hierarchyNodeRID;
						for (int level = 1; level <= hp.HierarchyLevels.Count; level++) // levels begin at 1 since the hierarchy occupies level 0
						{
							hlp = (HierarchyLevelProfile)hp.HierarchyLevels[level];
							HierarchyLevel_Add(hierarchyRID, level, hlp);
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						hp.HierarchyDBLevelsCount = hp.HierarchyLevels.Count;
						break;
					}
					case eChangeType.update: 
					{
						_mhd.OpenUpdateConnection();
                        // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
                        //_mhd.Hierarchy_Update(hp.Key, hp.HierarchyID,
                        _mhd.Hierarchy_Update(hp.Key, hp.HierarchyID, hp.Owner,
                        // End TT#72
							hp.HierarchyType, hp.HierarchyRollupOption, hp.HierarchyColor, hp.OTSPlanLevelType);
						int	OTSPlanLevelHierarchyRID = Include.NoRID;
						int	OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
						int	OTSPlanLevelAnchorNode = Include.Undefined;
						eMaskField maskField = eMaskField.Undefined;
						string OTSPlanLevelMask = null;
						bool OTSPlanLevelIsOverridden = false;
						if (hp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined)
						{
							OTSPlanLevelHierarchyRID = hp.Key;
							OTSPlanLevelHierarchyLevelSequence = hp.OTSPlanLevelHierarchyLevelSequence;
							OTSPlanLevelAnchorNode = hp.HierarchyRootNodeRID;
							OTSPlanLevelIsOverridden = true;
						}

                        // Begin TT#1911-MD - JSmith - Database Error on Update
                        //// Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                        //// Begin TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
                        //if (updateLevels)
                        //{
                        //    // End TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
                        //    HierarchyProfile currhp = HierarchyServerGlobal.GetHierarchyData(hp.Key);
                        //    for (int level = 1; level <= currhp.HierarchyLevels.Count; level++) // levels begin at 1 since the hierarchy occupies level 0
                        //    {
                        //        hlp = (HierarchyLevelProfile)currhp.HierarchyLevels[level];
                        //        if (!hlp.LevelNodesExist)
                        //        {
                        //            _mhd.Hierarchy_Level_Delete(hp.Key, level);
                        //        }
                        //    }
                        //    // Begin TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
                        //}
                        //// End TT#5384 - JSmith - Size folder disappearing from Hierarchy properties after completion of step 13 to 15 of set up guide and restarting the MID services
                        //// End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                        // End TT#1911-MD - JSmith - Database Error on Update

                        for (int level = 1; level <= hp.HierarchyLevels.Count; level++) // levels begin at 1 since the hierarchy occupies level 0
                        {
                            hlp = (HierarchyLevelProfile)hp.HierarchyLevels[level];
                            switch (hlp.LevelChangeType)
                            {
                                case eChangeType.add:
                                    HierarchyLevel_Add(hp.Key, level, hlp);
                                    break;
                                case eChangeType.update:
                                    HierarchyLevel_Update(hp.Key, level, hlp);
                                    break;
                                case eChangeType.delete:
                                    _mhd.Hierarchy_Level_Delete(hp.Key, level);
                                    break;
                            }
                        }

						_mhd.Hierarchy_Node_Update(hp.HierarchyRootNodeRID, hp.Key, 0,
							eHierarchyLevelType.Undefined, false, eOTSPlanLevelType.Undefined, false, 
							OTSPlanLevelIsOverridden, ePlanLevelSelectType.HierarchyLevel, 
							ePlanLevelLevelType.HierarchyLevel, OTSPlanLevelHierarchyRID, OTSPlanLevelHierarchyLevelSequence,
                            OTSPlanLevelAnchorNode, maskField, OTSPlanLevelMask, false, ePurpose.Default);
						
						_mhd.Hierarchy_BasicNode_Update(hp.HierarchyRootNodeRID, 
							hp.HierarchyID, hp.HierarchyID, hp.HierarchyID, false, eProductType.Undefined);

						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						hp.HierarchyDBLevelsCount = hp.HierarchyLevels.Count;
						break;
					}
					case eChangeType.delete: 
					{
						HierarchyInfo hi = HierarchyServerGlobal.GetHierarchyInfoByRID(hp.Key, true);
						_mhd.OpenUpdateConnection();
						HierarchyNodeProfile hnp = GetNodeData(hi.HierarchyRootNodeRID);
						DeleteNodeData(hnp);  // delete all information for the root node
						if(hnp.NodeChangeSuccessful)
						{
							_mhd.Hierarchy_Level_DeleteAll(hp.Key);
							hi.HierarchyDBLevelsCount = 0;
							_mhd.Hierarchy_Delete(hp.Key);
							_mhd.CommitData();
							_mhd.CloseUpdateConnection();
						}
						break;
					}
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    case eChangeType.markedForDelete:
                    {
                        _mhd.OpenUpdateConnection();
                        _mhd.Hierarchy_Update(hp.Key, hp.HierarchyID, hp.Owner,
                            hp.HierarchyType, hp.HierarchyRollupOption, hp.HierarchyColor, hp.OTSPlanLevelType);
                        HierarchyNodeProfile hnp = GetNodeData(hp.HierarchyRootNodeRID);
                        hnp.NodeChangeType = hp.HierarchyChangeType;
                        hnp.NodeID = hp.HierarchyID;
                        hnp.DeleteNode = true;
                        NodeUpdateProfileInfo(hnp);
                        if (hnp.NodeChangeSuccessful)
                        {
                            _mhd.CommitData();
                            _mhd.CloseUpdateConnection();
                        }
                        break;
                    }
                    // End TT#3630 - JSmith - Delete My Hierarchy
				}
				return HierarchyServerGlobal.HierarchyUpdate(hp);
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Refreshes the Global's calendar and this Session's calendar.
		/// </summary>
		/// <remarks>
		/// Since the calendar is static, it may have already been refreshed.
		/// That's why we check the refresh date.
		/// </remarks>
		/// <param name="refreshDate"></param>
		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				if (refreshDate != HierarchyServerGlobal.CalendarRefreshDate)
				{
					HierarchyServerGlobal.Calendar.Refresh();
					HierarchyServerGlobal.CalendarRefreshDate = refreshDate;
				}

				// Refresh the Calendar of THIS session
				Calendar.Refresh();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the posting date.
		/// </summary>
		/// <returns>The posting date for the organizational hierarchy</returns>
		public DateTime GetPostingDate()
		{
			try
			{
				HierarchyProfile hp = HierarchyServerGlobal.GetMainHierarchyData();
				return hp.PostingDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Updates the posting date.
		/// </summary>
		/// <param name="postingDate">The posting date</param>
		/// <remarks>Currently updates the posting date for the organizational hierarchy</remarks>
		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				Calendar.SetPostingDate(postingDate);
				DayProfile postingDay = Calendar.GetDay(postingDate);
				DayProfile currentDay = Calendar.CurrentDate;
				
				HierarchyProfile hp = HierarchyServerGlobal.GetMainHierarchyData();
				_mhd.OpenUpdateConnection();
                // Begin TT#460 - JSmith - Add size daily blob tables to Purge
                //if (!_mhd.PostingDate_Exists(hp.Key))
                //{
                //    _mhd.PostingDate_Add(hp.Key, postingDate, postingDay.YearDay, currentDay.Date, currentDay.YearDay);
                //}
                //else
                //{
                //    _mhd.PostingDate_Update(hp.Key, postingDate, postingDay.YearDay, currentDay.Date, currentDay.YearDay);
                //}

                SQL_TimeID sqlPostingDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, postingDay.Key);
                SQL_TimeID sqlCurrentDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, currentDay.Key);

				if (!_mhd.PostingDate_Exists(hp.Key))
				{
                    _mhd.PostingDate_Add(hp.Key, postingDate, postingDay.YearDay, sqlPostingDate.SqlTimeID, currentDay.Date, currentDay.YearDay, sqlCurrentDate.SqlTimeID);
				}
				else
				{
                    _mhd.PostingDate_Update(hp.Key, postingDate, postingDay.YearDay, sqlPostingDate.SqlTimeID, currentDay.Date, currentDay.YearDay, sqlCurrentDate.SqlTimeID);
				}
				_mhd.CommitData();

				HierarchyServerGlobal.PostingDateUpdate(hp.Key, postingDate);
			}
			catch ( Exception ex )
			{
                // Begin TT#2 -  RMatelic - Assortment Planning >> correct service name
                //EventLog.WriteEntry("MIDHierarchySession", "PostingDateUpdate error:" + ex.Message, EventLogEntryType.Error);
                EventLog.WriteEntry("MIDHierarchyService", "PostingDateUpdate error:" + ex.Message, EventLogEntryType.Error);
                // End TT#2
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the current date.
		/// </summary>
		/// <returns>The current date for the organizational hierarchy</returns>
		public DayProfile GetCurrentDate()
		{
			try
			{
				return HierarchyServerGlobal.Calendar.CurrentDate;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a level to a hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy.</param>
		/// <param name="level">The relative level of the information to be added to the hierarchy.</param>
		/// <param name="hlp">An instance of the HierarchyLevelProfile class containing level information.</param>
		private void HierarchyLevel_Add(int hierarchyRID, int level, HierarchyLevelProfile hlp)
		{
			try
			{
                // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
				_mhd.Hierarchy_Level_Add(hierarchyRID, level, hlp.LevelID, 
					hlp.LevelColor, hlp.LevelLengthType, hlp.LevelRequiredSize, hlp.LevelSizeRangeFrom, 
					hlp.LevelSizeRangeTo, hlp.LevelType,
					hlp.LevelOTSPlanLevelType, hlp.LevelDisplayOption, hlp.LevelIDFormat, 
					hlp.PurgeDailyHistoryTimeframe, hlp.PurgeDailyHistory,
					hlp.PurgeWeeklyHistoryTimeframe, hlp.PurgeWeeklyHistory, 
					hlp.PurgePlansTimeframe, hlp.PurgePlans);  // add other header types
                
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.ASN, hlp.PurgeHtASNTimeframe, hlp.PurgeHtASN);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.DropShip, hlp.PurgeHtDropShipTimeframe, hlp.PurgeHtDropShip);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Dummy, hlp.PurgeHtDummyTimeframe, hlp.PurgeHtDummy);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.PurchaseOrder, hlp.PurgeHtPurchaseOrderTimeframe, hlp.PurgeHtPurchaseOrder);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Receipt, hlp.PurgeHtReceiptTimeframe, hlp.PurgeHtReceipt);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Reserve, hlp.PurgeHtReserveTimeframe, hlp.PurgeHtReserve);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.IMO, hlp.PurgeHtVSWTimeframe, hlp.PurgeHtVSW);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.WorkupTotalBuy, hlp.PurgeHtWorkUpTotTimeframe, hlp.PurgeHtWorkUpTot);
                // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Updates a level in the hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy.</param>
		/// <param name="level">The relative level of the information to be added to the hierarchy.</param>
		/// <param name="hlp">An instance of the HierarchyLevelProfile class containing level information.</param>
		private void HierarchyLevel_Update(int hierarchyRID, int level, HierarchyLevelProfile hlp)
		{
			try
			{
                // Begin TT#400-MD - JSmith - Add Header Purge criteria by Header Type
                //_mhd.Hierarchy_Level_Update(hierarchyRID, level, hlp.LevelID, 
                //    hlp.LevelColor, hlp.LevelLengthType, hlp.LevelRequiredSize, hlp.LevelSizeRangeFrom, 
                //    hlp.LevelSizeRangeTo, hlp.LevelType,
                //    hlp.LevelOTSPlanLevelType, hlp.LevelDisplayOption, hlp.LevelIDFormat, 
                //    hlp.PurgeDailyHistoryTimeframe, hlp.PurgeDailyHistory,
                //    hlp.PurgeWeeklyHistoryTimeframe, hlp.PurgeWeeklyHistory, 
                //    hlp.PurgePlansTimeframe, hlp.PurgePlans,
                //    hlp.PurgeHeadersTimeframe, hlp.PurgeHeaders);
                _mhd.Hierarchy_Level_Update(hierarchyRID, level, hlp.LevelID,
                    hlp.LevelColor, hlp.LevelLengthType, hlp.LevelRequiredSize, hlp.LevelSizeRangeFrom,
                    hlp.LevelSizeRangeTo, hlp.LevelType,
                    hlp.LevelOTSPlanLevelType, hlp.LevelDisplayOption, hlp.LevelIDFormat,
                    hlp.PurgeDailyHistoryTimeframe, hlp.PurgeDailyHistory,
                    hlp.PurgeWeeklyHistoryTimeframe, hlp.PurgeWeeklyHistory,
                    hlp.PurgePlansTimeframe, hlp.PurgePlans);
                
                _mhd.Hierarchy_Level_Delete_HeaderPurge(hierarchyRID, level);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.ASN, hlp.PurgeHtASNTimeframe, hlp.PurgeHtASN);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.DropShip, hlp.PurgeHtDropShipTimeframe, hlp.PurgeHtDropShip);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Dummy, hlp.PurgeHtDummyTimeframe, hlp.PurgeHtDummy);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.PurchaseOrder, hlp.PurgeHtPurchaseOrderTimeframe, hlp.PurgeHtPurchaseOrder);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Receipt, hlp.PurgeHtReceiptTimeframe, hlp.PurgeHtReceipt);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.Reserve, hlp.PurgeHtReserveTimeframe, hlp.PurgeHtReserve);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.IMO, hlp.PurgeHtVSWTimeframe, hlp.PurgeHtVSW);
                _mhd.Hierarchy_Level_Add_HeaderPurge(hierarchyRID, level, eHeaderType.WorkupTotalBuy, hlp.PurgeHtWorkUpTotTimeframe, hlp.PurgeHtWorkUpTot);
                // End TT#400-MD - JSmith - Add Header Purge criteria by Header Type
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the session get all information associated with the main organizational hierarchy.
		/// </summary>
		/// <returns></returns>
		public HierarchyProfile GetMainHierarchyData()
		{
			try
			{
				return HierarchyServerGlobal.GetMainHierarchyData();
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the session get all information associated with a hierarchy's definition from the 
		/// hierarchy global area using the hierarchy record id.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns></returns>
		public HierarchyProfile GetHierarchyData(int hierarchyRID)
		{
			try
			{
				return HierarchyServerGlobal.GetHierarchyData(hierarchyRID);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the session get hierarchy information for update.
		/// </summary>
		/// <param name="aHierarchyRID">The record id of the hierarchy</param>
		/// <param name="aAllowReadOnly">This flag identifies if the lock type can be changed to read only if an update
		/// lock can not be obtained</param>
		/// <returns>An instance of the HierarchyProfile object containing information about the hierarchy</returns>
		public HierarchyProfile GetHierarchyDataForUpdate(int aHierarchyRID, bool aAllowReadOnly)
		{
			string errMsg;
			bool enqueueWriteLocked = false;
			System.Windows.Forms.DialogResult diagResult;
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;

				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(aHierarchyRID);

				HierarchyEnqueue hierarchyEnqueue = new HierarchyEnqueue(
					aHierarchyRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					hierarchyEnqueue.EnqueueHierarchy();
					hp.HierarchyLockStatus = eLockStatus.Locked;
				}
				catch (HierarchyConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
					enqueueWriteLocked = false;
					errMsg = "The following hierarchy requested:" + System.Environment.NewLine;
					foreach (HierarchyConflict HCon in hierarchyEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Hierarchy: " + hp.HierarchyID + ", User: " + HCon.UserName;
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
						hp.InUseUserID = HCon.UserName;
						//End Track #4815
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					if (aAllowReadOnly)
					{
						hp.HierarchyLockStatus = eLockStatus.ReadOnly;
						errMsg += "Do you wish to continue with the hierarchy as read-only?";

						diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Hierarchy Conflict",
							System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);
						if (diagResult == System.Windows.Forms.DialogResult.Cancel)
						{
							hp.HierarchyLockStatus = eLockStatus.Cancel;
						}
					}
					else
					{
						//errMsg += "Can not be updated at this time";
                        errMsg += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
						diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Hierarchy Conflict",
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
						hp.HierarchyLockStatus = eLockStatus.Cancel;
					}
				}
				return hp;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session dequeue the hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		public void DequeueHierarchy(int hierarchyRID)
		{
			try
			{
				HierarchyEnqueue hierarchyEnqueue = new HierarchyEnqueue(
					hierarchyRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					hierarchyEnqueue.DequeueHierarchy();

				}
				catch 
				{
					throw;
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the session get all information associated with a hierarchy's definition from the 
		/// hierarchy global area using the hierarchy name.
		/// </summary>
		/// <param name="hierarchyID">The id of the hierarchy</param>
		/// <returns></returns>
		public HierarchyProfile GetHierarchyData(string hierarchyID)
		{
			try
			{
				return HierarchyServerGlobal.GetHierarchyData(hierarchyID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the hierarchy type from the hierarchy global area.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns></returns>
		public eHierarchyType GetHierarchyType(int hierarchyRID)
		{
			try
			{
				return HierarchyServerGlobal.GetHierarchyType(hierarchyRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the owner of the hierarchy.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns></returns>
		public int GetHierarchyOwner(int hierarchyRID)
		{
			try
			{
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(hierarchyRID);
				return hp.Owner;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2064 - JSmith - Add message to Rollup when hierarchy dependency build fails
        //static public HierarchyProfileList GetHierarchiesByDependency()
        ///// <summary>
        ///// Requests the session get all hierarchies in dependency order.  This means that a hierarchy in
        ///// the list does not contain a reference to any other hierarchy later in the list. 
        ///// </summary>
        ///// <returns></returns>
        //public HierarchyProfileList GetHierarchiesByDependency()
        //{
        //    try
        //    {
        //        return HierarchyServerGlobal.GetHierarchiesByDependency();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
		/// Requests the session get all hierarchies in dependency order.  This means that a hierarchy in
		/// the list does not contain a reference to any other hierarchy later in the list. 
		/// </summary>
		/// <returns></returns>
        // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
        //public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage)
        public HierarchyProfileList GetHierarchiesByDependency(ref string aReturnMessage, ref eMIDMessageLevel aMessageLevel)
        // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
		{
			try
			{
                // Begin TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
                //return HierarchyServerGlobal.GetHierarchiesByDependency(ref aReturnMessage);
                return HierarchyServerGlobal.GetHierarchiesByDependency(ref aReturnMessage, ref aMessageLevel);
                // End TT#746-MD - JSmith - Unable to override message level for Circular Hierarchy Relationship
			}
			catch
			{
				throw;
			}
		}
        // End TT#2064



        /// <summary>
		/// Requests the session get all hierarchies in for a user. 
		/// </summary>
		/// <returns></returns>
		public HierarchyProfileList GetHierarchiesForUser(int aUserRID)
		{
			try
			{
				return HierarchyServerGlobal.GetHierarchiesForUser(aUserRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the count of the largest branch below the node in the hierarchy.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns></returns>
        public int GetLongestBranch(int aNodeRID)
		{
			try
			{
                // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
                //return HierarchyServerGlobal.GetLongestBranch(aNodeRID);
                return HierarchyServerGlobal.GetLongestBranch(aNodeRID, false);
                // End Track #5960
			}
			catch
			{
				throw;
			}
		}

        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
        /// <summary>
        /// Requests the session get the count of the largest branch below the node in the hierarchy.
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aHomeHierarchyOnly">A flag indicating that only the home hierarchy should be searched</param>
        /// <returns></returns>
        public int GetLongestBranch(int aNodeRID, bool aHomeHierarchyOnly)
        {
            try
            {
                return HierarchyServerGlobal.GetLongestBranch(aNodeRID, aHomeHierarchyOnly);
            }
            catch
            {
                throw;
            }
        }
        // End Track #5960

		/// <summary>
		/// Requests the session get the highest guest level below the node in the hierarchy.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns></returns>
		public int GetHighestGuestLevel(int aNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetHighestGuestLevel(aNodeRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
        /// <summary>
        /// Gets a list of all guest levels 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>An ArrayList containing HierarchyLevelInfo objects for all guest levels</returns>
        public ArrayList GetAllGuestLevels(int aNodeRID)
        {
            try
            {
                return HierarchyServerGlobal.GetAllGuestLevels(aNodeRID);
            }
            catch
            {
                throw;
            }
        }
        // End Track #5960

        /// <summary>
        /// Gets a list of all guest levels 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>An ArrayList containing HierarchyLevelInfo objects for all guest levels</returns>
        public DataTable GetHierarchyDescendantLevels(int aNodeRID)
        {
            try
            {
                return HierarchyServerGlobal.GetHierarchyDescendantLevels(aNodeRID);
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Requests the session get the owner of the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns></returns>
		public int GetNodeOwner(int nodeRID)
		{
			try
			{
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(nodeRID, false);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(hnp.HomeHierarchyRID);
				return hp.Owner;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session update node profile information in the hierarchy global area.
		/// </summary>
		/// <param name="hnp">An instance of the HierarchyNodeProfile class that contains information for a node</param>
		public HierarchyNodeProfile NodeUpdateProfileInfo(HierarchyNodeProfile hnp)
		{
			// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
			MerchandiseHierarchyData dataLock = null;
			HierarchyNodeProfile lookupNode = null;
			string parentID = null;
			string lookupID = null;
			// End Track #5307
			try
			{
                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                // determine if node already added after lock is applied
                dataLock = new MerchandiseHierarchyData();
                // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                //dataLock.OpenUpdateConnection(eLockType.HierarchyNode, hnp.NodeID);
                // End TT#1453

                //lookupNode = NodeLookup(lookupID, _nodeDelimiter, true, false, false, out parentID);
                //if (lookupNode.Key != Include.NoRID)
                //{
                //    return lookupNode;
                //}
                // End Track #6015

				switch (hnp.LevelType)
				{
					case eHierarchyLevelType.Color:
					{
						if (hnp.QualifiedNodeID == null)
						{
							hnp.QualifiedNodeID = HierarchyServerGlobal.BuildQualifiedNode(hnp);
						}
						// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
						lookupID = hnp.QualifiedNodeID;
						// End Track #5307

						break;
					}
					case eHierarchyLevelType.Size:
					{
						if (hnp.QualifiedNodeID == null)
						{
							hnp.QualifiedNodeID = HierarchyServerGlobal.BuildQualifiedNode(hnp);
						}
						// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
						lookupID = hnp.QualifiedNodeID;
						// End Track #5307

						break;
					}
					// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
					default:
					{
						lookupID = hnp.NodeID;

						break;
					}
					// End Track #5307
				}

                // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                dataLock.OpenUpdateConnection(eLockType.HierarchyNode, lookupID);
                // End TT#1453

                try
                {
                    //  //Begin Track #5307 - JSmith - duplicate nodes on hierarchy
                    if (hnp.NodeChangeType == eChangeType.add)
                      {
                        // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                         // determine if node already added after lock is applied
                         // dataLock = new MerchandiseHierarchyData();
                         // dataLock.OpenUpdateConnection(eLockType.HierarchyNode, Include.CreateNodeLockKey(hnp.HomeHierarchyParentRID, hnp.NodeID));
                        // End Track #6015

                        lookupNode = NodeLookup(lookupID, _nodeDelimiter, true, false, false, out parentID);
                        if (lookupNode.Key != Include.NoRID)
                        {
                            return lookupNode;
                        }
                    }
                    //End Track #5307

                    switch (hnp.NodeChangeType)
                    {
                        case eChangeType.none:
                            {
                                break;
                            }
                        case eChangeType.add:
                            {
                                // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors -> Missing Size Node
                                //hnp.Key = _mhd.Hierarchy_Node_Add(hnp.HierarchyRID, hnp.HomeHierarchyParentRID,
                                //    hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel,
                                //    hnp.LevelType, hnp.OTSPlanLevelTypeIsOverridden, hnp.OTSPlanLevelType, hnp.UseBasicReplenishment,
                                //    hnp.OTSPlanLevelIsOverridden, hnp.OTSPlanLevelSelectType, hnp.OTSPlanLevelLevelType,
                                //    hnp.OTSPlanLevelHierarchyRID, hnp.OTSPlanLevelHierarchyLevelSequence,
                                //    hnp.OTSPlanLevelAnchorNode, hnp.OTSPlanLevelMaskField, hnp.OTSPlanLevelMask, hnp.IsVirtual, hnp.Purpose);
                                //switch (hnp.LevelType)
                                //{
                                //    case eHierarchyLevelType.Color:
                                //        {
                                //            string[] fields = null;
                                //            if (hnp.QualifiedNodeID != null)
                                //            {
                                //                fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                //            }
                                //            else
                                //            {
                                //                fields = new string[2];
                                //            }
                                //            _mhd.Hierarchy_ColorNode_Add(hnp.Key, hnp.ColorOrSizeCodeRID, hnp.NodeDescription, fields[0]);
                                //            break;
                                //        }
                                //    case eHierarchyLevelType.Size:
                                //        {
                                //            string[] fields = null;
                                //            if (hnp.QualifiedNodeID != null)
                                //            {
                                //                fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                //            }
                                //            else
                                //            {
                                //                fields = new string[3];
                                //            }
                                //            _mhd.Hierarchy_SizeNode_Add(hnp.Key, hnp.ColorOrSizeCodeRID, fields[0], fields[1]);
                                //            break;
                                //        }
                                //    default:
                                //        {
                                //            _mhd.Hierarchy_BasicNode_Add(hnp.Key, hnp.NodeID, hnp.NodeName,
                                //                hnp.NodeDescription, hnp.ProductTypeIsOverridden, hnp.ProductType);
                                //            break;
                                //        }
                                //}
                                switch (hnp.LevelType)
                                {
                                    case eHierarchyLevelType.Color:
                                        {
                                            string[] fields = null;
                                            if (hnp.QualifiedNodeID != null)
                                            {
                                                fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                            }
                                            else
                                            {
                                                fields = new string[2];
                                            }
                                            // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            if (hnp.NodeDescription == null || hnp.NodeDescription == string.Empty)
                                            {
                                                ColorCodeProfile ccp = GetColorCodeProfile(hnp.ColorOrSizeCodeRID);
                                                hnp.NodeDescription = ccp.ColorCodeName;
                                            }
                                            // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            hnp.Key = _mhd.Hierarchy_ColorNode_Add(hnp.HierarchyRID, hnp.HomeHierarchyParentRID,
                                                                                   hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel,
                                                                                   hnp.LevelType, hnp.OTSPlanLevelTypeIsOverridden, hnp.OTSPlanLevelType, hnp.UseBasicReplenishment,
                                                                                   hnp.OTSPlanLevelIsOverridden, hnp.OTSPlanLevelSelectType, hnp.OTSPlanLevelLevelType,
                                                                                   hnp.OTSPlanLevelHierarchyRID, hnp.OTSPlanLevelHierarchyLevelSequence,
                                                                                   hnp.OTSPlanLevelAnchorNode, hnp.OTSPlanLevelMaskField, hnp.OTSPlanLevelMask, hnp.IsVirtual, hnp.Purpose, 
                                                                                   hnp.ColorOrSizeCodeRID, hnp.NodeDescription, fields[0]);
                                            break;
                                        }
                                    case eHierarchyLevelType.Size:
                                        {
                                            string[] fields = null;
                                            if (hnp.QualifiedNodeID != null)
                                            {
                                                fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                            }
                                            else
                                            {
                                                fields = new string[3];
                                            }
                                            // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            if (hnp.NodeDescription == null || hnp.NodeDescription == string.Empty)
                                            {
                                                SizeCodeProfile scp = GetSizeCodeProfile(hnp.ColorOrSizeCodeRID);
                                                hnp.NodeDescription = scp.SizeCodeName;
                                            }
                                            // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            hnp.Key = _mhd.Hierarchy_SizeNode_Add(hnp.HierarchyRID, hnp.HomeHierarchyParentRID,
                                                                                  hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel,
                                                                                  hnp.LevelType, hnp.OTSPlanLevelTypeIsOverridden, hnp.OTSPlanLevelType, hnp.UseBasicReplenishment,
                                                                                  hnp.OTSPlanLevelIsOverridden, hnp.OTSPlanLevelSelectType, hnp.OTSPlanLevelLevelType,
                                                                                  hnp.OTSPlanLevelHierarchyRID, hnp.OTSPlanLevelHierarchyLevelSequence,
                                                                                  hnp.OTSPlanLevelAnchorNode, hnp.OTSPlanLevelMaskField, hnp.OTSPlanLevelMask, hnp.IsVirtual, hnp.Purpose, 
                                                                                  hnp.ColorOrSizeCodeRID, fields[0], fields[1]);
                                            break;
                                        }
                                    default:
                                        {
                                            // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            if (hnp.NodeDescription == null || hnp.NodeDescription == string.Empty)
                                            {
                                                hnp.NodeDescription = hnp.NodeName;
                                            }
                                            // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                            hnp.Key = _mhd.Hierarchy_BasicNode_Add(hnp.HierarchyRID, hnp.HomeHierarchyParentRID,
                                                                                   hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel,
                                                                                   hnp.LevelType, hnp.OTSPlanLevelTypeIsOverridden, hnp.OTSPlanLevelType, hnp.UseBasicReplenishment,
                                                                                   hnp.OTSPlanLevelIsOverridden, hnp.OTSPlanLevelSelectType, hnp.OTSPlanLevelLevelType,
                                                                                   hnp.OTSPlanLevelHierarchyRID, hnp.OTSPlanLevelHierarchyLevelSequence,
                                                                                   hnp.OTSPlanLevelAnchorNode, hnp.OTSPlanLevelMaskField, hnp.OTSPlanLevelMask, hnp.IsVirtual, hnp.Purpose, 
                                                                                   hnp.NodeID, hnp.NodeName,
                                                hnp.NodeDescription, hnp.ProductTypeIsOverridden, hnp.ProductType);
                                            break;
                                        }
                                }
                                // End TT#1453
                                if (hnp.PurgeCriteriaChangeType == eChangeType.add)
                                {
                                    _mhd.PurgeCriteria_DeleteAll(hnp.Key);
                                    _mhd.PurgeCriteria_Add(hnp.Key, hnp.PurgeDailyHistoryAfter, hnp.PurgeWeeklyHistoryAfter,
                                        hnp.PurgeOTSPlansAfter);
                                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.ASN, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtASNAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.DropShip, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtDropShipAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Dummy, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtDummyAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.IMO, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtVSWAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.PurchaseOrder, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtPurchaseOrderAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Receipt, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtReceiptAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Reserve, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtReserveAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.WorkupTotalBuy, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtWorkUpTotAfter);
                                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                }
                                // BEGIN TT#1399
                                if (hnp.ApplyHNRIDFrom != Include.NoRID)
                                {
                                    _mhd.Hierarchy_Properties_Update(hnp.Key, hnp.ApplyHNRIDFrom);
                                }
                                // END TT#1399
                                break;
                            }
                        case eChangeType.update:
                            {
                                _mhd.Hierarchy_Node_Update(hnp.Key, hnp.HomeHierarchyRID, hnp.HomeHierarchyLevel,
                                    hnp.LevelType, hnp.OTSPlanLevelTypeIsOverridden,
                                    hnp.OTSPlanLevelType, hnp.UseBasicReplenishment,
                                    hnp.OTSPlanLevelIsOverridden, hnp.OTSPlanLevelSelectType, hnp.OTSPlanLevelLevelType,
                                    hnp.OTSPlanLevelHierarchyRID, hnp.OTSPlanLevelHierarchyLevelSequence,
                                    hnp.OTSPlanLevelAnchorNode, hnp.OTSPlanLevelMaskField, hnp.OTSPlanLevelMask, hnp.IsVirtual, hnp.Purpose);
                                // BEGIN TT#1399
                                _mhd.Hierarchy_Properties_Update(hnp.Key, hnp.ApplyHNRIDFrom);
                                // END TT#1399
                                switch (hnp.LevelType)
                                {
                                    case eHierarchyLevelType.Color:
                                        {
                                            string[] fields = null;
                                            if (hnp.QualifiedNodeID != null)
                                            {
                                                fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                            }
                                            else
                                            {
                                                fields = new string[2];
                                            }
                                            _mhd.Hierarchy_ColorNode_Update(hnp.Key, hnp.ColorOrSizeCodeRID, hnp.NodeDescription, fields[0]);
                                            // check to see if style was renamed
                                            NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(hnp.Key, false);
                                            if (ni.NodeID != hnp.NodeID)
                                            {
                                                _mhd.RenameStyleIDOnIDs(ni.NodeID, hnp.NodeID);
                                            }
                                            break;
                                        }
                                    case eHierarchyLevelType.Size:
                                        {
                                            //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -dead function
                                            //string[] fields = null;
                                            //if (hnp.QualifiedNodeID != null)
                                            //{
                                            //    fields = MIDstringTools.Split(hnp.QualifiedNodeID, GetProductLevelDelimiter(), true);
                                            //}
                                            //else
                                            //{
                                            //    fields = new string[2];
                                            //}
                                            //_mhd.Hierarchy_SizeNode_Update(hnp.Key, hnp.ColorOrSizeCodeRID, fields[0], fields[1]);
                                            //End TT#846-MD -jsobek -New Stored Procedures for Performance -dead function
                                            break;
                                        }
                                    default:
                                        {
                                            _mhd.Hierarchy_BasicNode_Update(hnp.Key, hnp.NodeID, hnp.NodeName,
                                                hnp.NodeDescription, hnp.ProductTypeIsOverridden, hnp.ProductType);
                                            // check to see if style was renamed
                                            if (hnp.LevelType == eHierarchyLevelType.Style)
                                            {
                                                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(hnp.Key, false);
                                                if (ni.NodeID != hnp.NodeID)
                                                {
                                                    _mhd.RenameStyleIDOnIDs(ni.NodeID, hnp.NodeID);
                                                }
                                            }
                                            break;
                                        }
                                }
                                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                if ((hnp.PurgeCriteriaChangeType == eChangeType.add) || (hnp.PurgeCriteriaChangeType == eChangeType.update))
                                {
                                    _mhd.PurgeCriteria_DeleteAll(hnp.Key);
                                    _mhd.PurgeCriteria_Add(hnp.Key, hnp.PurgeDailyHistoryAfter, hnp.PurgeWeeklyHistoryAfter,
                                        hnp.PurgeOTSPlansAfter);
                                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.ASN, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtASNAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.DropShip, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtDropShipAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Dummy, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtDummyAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.IMO, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtVSWAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.PurchaseOrder, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtPurchaseOrderAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Receipt, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtReceiptAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.Reserve, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtReserveAfter);
                                    _mhd.PurgeCriteria_Add_HeaderType(hnp.Key, eHeaderType.WorkupTotalBuy, (int)ePurgeTimeframe.Weeks, hnp.PurgeHtWorkUpTotAfter);
                                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                }
                                else
                                    //if (hnp.PurgeCriteriaChangeType == eChangeType.update)
                                    //{
                                    //    _mhd.PurgeCriteria_Update(hnp.Key, hnp.PurgeDailyHistoryAfter, hnp.PurgeWeeklyHistoryAfter,
                                    //        hnp.PurgeOTSPlansAfter);
                                    //    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.ASN, ePurgeTimeframe.Weeks, hnp.PurgeHtASNAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.DropShip, ePurgeTimeframe.Weeks, hnp.PurgeHtDropShipAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.Dummy, ePurgeTimeframe.Weeks, hnp.PurgeHtDummyAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.IMO, ePurgeTimeframe.Weeks, hnp.PurgeHtVSWAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.PurchaseOrder, ePurgeTimeframe.Weeks, hnp.PurgeHtPurchaseOrderAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.Receipt, ePurgeTimeframe.Weeks, hnp.PurgeHtReceiptAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.Reserve, ePurgeTimeframe.Weeks, hnp.PurgeHtReserveAfter);
                                    //    _mhd.PurgeCriteria_Update_HeaderPurge(hnp.Key, eHeaderType.WorkupTotalBuy, ePurgeTimeframe.Weeks, hnp.PurgeHtWorkUpTotAfter);
                                    //    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    //}
                                    //else
                                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                                    if (hnp.PurgeCriteriaChangeType == eChangeType.delete)
                                        {
                                            _mhd.PurgeCriteria_DeleteAll(hnp.Key);
                                        }

                                break;
                            }
                        case eChangeType.delete:
                            {
                                if (!DeleteNodeData(hnp))
                                {
                                    hnp.NodeChangeSuccessful = false;
                                }
                                break;
                            }
                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                        case eChangeType.markedForDelete:
                            {
                                _mhd.Hierarchy_Node_MarkForDelete(hnp.Key, hnp.DeleteNode, hnp.NodeID); 
                                break;
                            }
                        // End TT#3630 - JSmith - Delete My Hierarchy
                    }
                    if (hnp.NodeChangeSuccessful)
                    {
                        // Begin TT#3173 - JSmith - Severe Error during History Load
                        if (hnp.CommitOnSuccessfulUpdate)
                        {
                            CommitData();
                            hnp.CommitOnSuccessfulUpdate = false;
                        }
                        // End TT#3173 - JSmith - Severe Error during History Load
                        HierarchyServerGlobal.NodeUpdateProfileInfo(hnp);
                    }
                }
                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                catch (Exception ex) 
                {
                    throw;
                }
                // End Track #6015

				return hnp;
			}
			catch ( Exception ex )
			{
                // Begin TT#2 -  RMatelic - Assortment Planning >> correct service name
                //EventLog.WriteEntry("MIDHierarchySession", "NodeUpdateProfileInfo error:" + ex.Message, EventLogEntryType.Error);
                EventLog.WriteEntry("MIDHierarchyService", "NodeUpdateProfileInfo error:" + ex.Message, EventLogEntryType.Error);
                // End TT#2
				throw;
			}
            // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
            finally
            {
                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    // Begin TT#2255 - JSmith - Getting an error code loading running the hierarchy load
                    dataLock.RemoveLocks();
                    // End TT#2255
                    dataLock.CloseUpdateConnection();
                }
            }
            // End Track #6015
        }

		/// <summary>
		/// Deletes information associated with a node from the database
		/// </summary>
		/// <param name="hnp">An instance of the HierarchyNodeProfile class containing information about the node</param>
		/// <returns>A flag if the delete was successful</returns>
		private bool DeleteNodeData(HierarchyNodeProfile hnp)
		{
			try
			{
				_mhd.Hierarchy_Node_Delete(hnp.Key);
				return true;
			}
			catch ( Exception ex )
			{
				string exceptionMessage = ex.Message;
				throw;
			}
		}

		/// <summary>
		/// Selectively deletes information associated with a node from the database
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="deleteNode">A flag to identify if the node is to be deleted.  This will automatically delete all other data.</param>
		/// <param name="deleteEligibility">A flag to identify if the eligibility is to be deleted.</param>
		/// <param name="deleteStoreGrades">A flag to identify if the store grages are to be deleted.</param>
		/// <param name="deleteVelocityGrades">A flag to identify if the velocity grades are to be deleted.</param>
		/// <param name="deleteStoreCapacity">A flag to identify if the store capacities are to be deleted.</param>
		/// <param name="deletePurgeCriteria">A flag to identify if the purge criteria is to be deleted.</param>
		/// <param name="deleteDailyPercentages">A flag to identify if the daily percentages are to be deleted.</param>
		/// <param name="deleteSellThruPcts">A flag to identify if the sell thru percentages are to be deleted.</param>
		/// <param name="deleteStockMinMaxes">A flag to identify if the stock min/maxes are to be deleted.</param>
        /// <param name="aDeleteCharacteristics">A flag to identify if the characteristics are to be deleted.</param>
		public void DeleteNodeData(int nodeRID, bool deleteNode, bool deleteEligibility, bool deleteStoreGrades,
			bool deleteVelocityGrades, bool deleteStoreCapacity, bool deletePurgeCriteria,
			//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
			//Begin TT#155 - JScott - Add Size Curve info to Node Properties
			////bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes, bool aDeleteCharacteristics)
			//bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes,
			//bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance, bool deleteSizeCurveSimilarStore, bool aDeleteCharacteristics)
			//End TT#155 - JScott - Add Size Curve info to Node Properties
			bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes,
            //Begin TT#1671 - DOConnell - Chain Set Percent Node Properties
			bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance, bool deleteSizeCurveSimilarStore, 
			bool deleteSizeOutOfStock, bool deleteSizeSellThru, bool aDeleteCharacteristics, bool deleteChainSetPercent, bool deleteIMO)
            //End TT#1671 - DOConnell - Chain Set Percent Node Properties
			//End TT#483 - JScott - Add Size Lost Sales criteria and processing
		{
			try
			{
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(nodeRID, false);
				if (hnp.Key != Include.NoRID)
				{
					_mhd.OpenUpdateConnection();
                    //BEGIN TT#1401 - Reservation Stores - gtaylor
                    if (deleteIMO || deleteNode)
                    {
                        _mhd.Node_IMO_Delete(hnp.Key);
                    }
                    //END TT#1401 - Reservation Stores - gtaylor
					if (deleteStoreGrades || deleteNode)
					{
						_mhd.StoreGrade_DeleteAll(hnp.Key);
					}
					if (deleteVelocityGrades || deleteNode)
					{
						_mhd.VelocityGrade_DeleteAll(hnp.Key);
					}
					if (deleteSellThruPcts || deleteNode)
					{
						_mhd.SellThruPcts_DeleteAll(hnp.Key);
					}
					if (deletePurgeCriteria || deleteNode)
					{
						_mhd.PurgeCriteria_DeleteAll(hnp.Key);
					}
					if (deleteStoreCapacity || deleteNode)
					{
						_mhd.StoreCapacity_DeleteAll(hnp.Key);
					}
					if (deleteEligibility || deleteNode)
					{
						_mhd.SimilarStore_DeleteAll(hnp.Key);
						_mhd.StoreEligibility_DeleteAll(hnp.Key);
					}
					if (deleteDailyPercentages || deleteNode)
					{
						_mhd.DailyPercentagesDefaults_DeleteAll(hnp.Key);
						_mhd.DailyPercentages_DeleteAll(hnp.Key);
					}
					if (deleteStockMinMaxes || deleteNode)
					{
						_mhd.StockMinMaxes_DeleteAll(hnp.Key);
					}
					if (aDeleteCharacteristics || deleteNode)
					{
						_mhd.Hier_Char_Join_DeleteAll(hnp.Key);
					}
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					if (deleteSizeCurveCriteria || deleteNode)
					{
						_mhd.SizeCurveCriteria_DeleteAll(hnp.Key);
					}
					if (deleteSizeCurveTolerance || deleteNode)
					{
						_mhd.SizeCurveTolerance_DeleteAll(hnp.Key);
					}
					if (deleteSizeCurveSimilarStore || deleteNode)
					{
						_mhd.SizeCurveSimilarStore_DeleteAll(hnp.Key);
					}
                    //Begin TT#1671 - DOConnell - Chain Set Percent Node Properties
                    if (deleteChainSetPercent || deleteNode)
                    {
                        _mhd.ChainSetPercentSet_DeleteAll(hnp.Key);
                    }
                    //End TT#1671 - DOConnell - Chain Set Percent Node Properties
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
					if (deleteSizeOutOfStock || deleteNode)
					{
						_mhd.SizeOutOfStock_DeleteAll(hnp.Key);
					}
					if (deleteSizeSellThru || deleteNode)
					{
						_mhd.SizeSellThru_DeleteAll(hnp.Key);
					}
					//End TT#483 - JScott - Add Size Lost Sales criteria and processing

					if (deleteNode)
					{
						_mhd.Hierarchy_Join_DeleteAll(hnp.Key);
						switch (hnp.LevelType)
						{
							case eHierarchyLevelType.Color:
							{
								_mhd.Hierarchy_ColorNode_Delete(hnp.Key, hnp.ColorOrSizeCodeRID);
								break;
							}
							case eHierarchyLevelType.Size:
							{
								_mhd.Hierarchy_SizeNode_Delete(hnp.Key, hnp.ColorOrSizeCodeRID);
								break;
							}
							default:
							{
								_mhd.Hierarchy_BasicNode_Delete(hnp.Key);
								break;
							}
						}
						_mhd.Hierarchy_Node_Delete(hnp.Key);
					}
					_mhd.CommitData();
					_mhd.CloseUpdateConnection();
			
					HierarchyServerGlobal.DeleteNodeData(nodeRID, deleteNode, deleteEligibility, deleteStoreGrades,
						//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
						////deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria, deleteDailyPercentages, hnp.QualifiedNodeID);
						//deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria, deleteDailyPercentages,
						//deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore, hnp.QualifiedNodeID);
						//End TT#155 - JScott - Add Size Curve info to Node Properties
						deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria, deleteDailyPercentages,
						deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore, 
						deleteSizeOutOfStock, deleteSizeSellThru, 
                        aDeleteCharacteristics, 
                        deleteIMO,
                        deleteSellThruPcts, deleteStockMinMaxes,  // TT#3982 - JSmith - Node Properties – Stock Min/Max Apply to lower levels not working
                        hnp.QualifiedNodeID);
						//End TT#483 - JScott - Add Size Lost Sales criteria and processing
				}
			}
			catch ( Exception ex )
			{
				string exceptionMessage = ex.Message;
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Replaces the RID of a node with a new RID
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aReplaceWithNodeRID">The record id of the new node.</param>
		public void ReplaceNode(int aNodeRID, int aReplaceWithNodeRID)
		{
			bool previousConnection = true;
			try
			{
				if (!MHD.ConnectionIsOpen)
				{
					MHD.OpenUpdateConnection();
					previousConnection = false;
				}
				MHD.Hierarchy_Node_Replace(aNodeRID, aReplaceWithNodeRID);
				if (!previousConnection)
				{
					MHD.CommitData();
				}
			}
			catch ( Exception ex )
			{
				string exceptionMessage = ex.Message;
				throw;
			}
			finally
			{ 
				if (!previousConnection &&
					MHD.ConnectionIsOpen)
				{
					MHD.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node record ID.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <remarks>
		/// This method will populate the parentRID with the parent in the home hierarchy
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		public HierarchyNodeProfile GetNodeData(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node record ID.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <remarks>
		/// This method will populate the parentRID with the parent in the home hierarchy
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		public HierarchyNodeProfile GetNodeData(int nodeRID, bool chaseHierarchy)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(nodeRID, chaseHierarchy);
			}
			catch
			{
				throw;
			}
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node record ID.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aChaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <remarks>This method will populate the parentRID with the parent in the home hierarchy</remarks>
		public HierarchyNodeProfile GetNodeData(int aNodeRID, bool aChaseHierarchy, bool aBuildQualifiedID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(aNodeRID, aChaseHierarchy, aBuildQualifiedID);
			}
			catch
			{
				throw;
			}
		}
		//End Track #5378

        //Begin TT#1313-MD -jsobek -Header Filters
        public string GetHierarchyIdByRID(int hierarchyRID)
        {
            try
            {
                return HierarchyServerGlobal.GetHierarchyIdByRID(hierarchyRID);
            }
            catch
            {
                throw;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

		/// <summary>
		/// Requests the session get node information for update.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <param name="aAllowReadOnly">This flag identifies if the lock type can be changed to read only if an update
		/// lock can not be obtained</param>
		/// <remarks>This method will populate the parentRID with the parent in the home hierarchy</remarks>
		public HierarchyNodeProfile GetNodeDataForUpdate(int aNodeRID, bool aAllowReadOnly)
		{
			string errMsg;
			bool enqueueWriteLocked = false;
			System.Windows.Forms.DialogResult diagResult;
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;

				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(aNodeRID);

                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                //HierarchyNodeEnqueue hierarchyNodeEnqueue = new HierarchyNodeEnqueue(
                //    hnp.HomeHierarchyRID,
                //    aNodeRID,
                //    SessionAddressBlock.ClientServerSession.UserRID,
                //    SessionAddressBlock.ClientServerSession.ThreadID,
                //    HierarchyServerGlobal.GetNodeAncestorList(aNodeRID, hnp.HomeHierarchyRID));
                HierarchyNodeEnqueue hierarchyNodeEnqueue = new HierarchyNodeEnqueue(
                    hnp.HomeHierarchyRID,
                    aNodeRID,
                    SessionAddressBlock.ClientServerSession.UserRID,
                    SessionAddressBlock.ClientServerSession.ThreadID);
                // End TT#2015

				try
				{
					hierarchyNodeEnqueue.EnqueueHierarchyNode();
					hnp.NodeLockStatus = eLockStatus.Locked;
				}
				catch (HierarchyNodeConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
					enqueueWriteLocked = false;
					errMsg = "The following hierarchy node requested:" + System.Environment.NewLine;
					foreach (HierarchyNodeConflict HNodeCon in hierarchyNodeEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Node: " + hnp.Text + ", User: " + HNodeCon.UserName;
						hnp.InUseUserID = HNodeCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					if (aAllowReadOnly)
					{
						hnp.NodeLockStatus = eLockStatus.ReadOnly;
						errMsg += "Do you wish to continue with the hierarchy node as read-only?";

						diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Hierarchy Node Conflict",
							System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

						if (diagResult == System.Windows.Forms.DialogResult.Cancel)
						{
							hnp.NodeLockStatus = eLockStatus.Cancel;
						}
					}
					else
					{
						//errMsg += "Can not be updated at this time";
                        errMsg += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
						diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Hierarchy Node Conflict",
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
						hnp.NodeLockStatus = eLockStatus.Cancel;
					}
				}
				return hnp;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session dequeue the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		public void DequeueNode(int nodeRID)
		{
			try
			{
				HierarchyNodeEnqueue hierarchyNodeEnqueue = new HierarchyNodeEnqueue(
					nodeRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					hierarchyNodeEnqueue.DequeueHierarchyNode();

				}
				catch 
				{
					throw;
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
        }

        #region Apply Changes To Lower Levels
        //  BEGIN TT#2015 - gtaylor - apply changes to lower levels
        //  update the nodes with the changes
        public bool UpdateLowerLevelNodes(NodeChangeProfile _nodeChangeProfile)
        {
            int node = Include.Undefined;
            int storeRID = Include.Undefined;
            int sglRID = Include.Undefined;
            int timeID = Include.Undefined;
            int returnCode = Include.Undefined;
            
            bool success = false;
            bool withStoreRID = true;
            string tableName = "";
            string columnName = "";
            int colvalue = 0;
            //MIDDbParameter[] dbParams = null;
            string[] othercols = new string[] {};
            DataTable _alldescendants;
            try
            {
                _alldescendants = _mhd.GetAllAffectedDescendants(_nodeChangeProfile.NodeRID,
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.IMO) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.StoreEligibility) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.StoreCharacteristics) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.StoreCapacity) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.DailyPercentages) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.PurgeCriteria) ? 1 : 0),
                    (_nodeChangeProfile.NodeChanges.ContainsKey((int)eProfileType.ChainSetPercent) ? 1 : 0)
                    );
                _mhd.OpenUpdateConnection();
                //  loop through the tab types in the change list
                foreach (int key in _nodeChangeProfile.NodeChanges.Keys)
                {
                    //  loop through each change
                    foreach (KeyValuePair<long, object> _kvp in _nodeChangeProfile.NodeChanges[key])
                    {
                        //  loop through each descendant and remove from table
                        foreach (DataRow _row in _alldescendants.Rows)
                        {
                            node = ((int)_row["HN_RID"]);
                            switch ((eProfileType)key)
                            {                                     
                                case eProfileType.IMO:
                                    storeRID = ((ChangeProfileVSW)_kvp.Value).StoreRID;
                                    tableName = "NODE_IMO";
                                    withStoreRID = true;
                                    break;
                                case eProfileType.StoreCapacity:
                                    storeRID = ((ChangeProfileSC)_kvp.Value).StoreRID;
                                    tableName = "STORE_CAPACITY";
                                    withStoreRID = true;
                                    break;
                                case eProfileType.DailyPercentages:
                                    //  Daily Percentages has two tables to update
                                    storeRID = ((ChangeProfileDailyPct)_kvp.Value).StoreRID;
                                    tableName = "DAILY_PERCENTAGES";
                                    //dbParams = new MIDDbParameter[] {
                                    //    new MIDDbParameter("@HN_RID", node, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input)
                                    //};
                                    //_mhd.DeleteFromTableWithStoreRID(ref dbParams);
                                    _mhd.DeleteFromTableWithStoreRID(HN_RID: node, ST_RID: storeRID, TABLENAME: tableName);
                                    tableName = "DAILY_PERCENTAGES_DEFAULTS";
                                    withStoreRID = true;
                                    break;
                                case eProfileType.ChainSetPercent:
                                    tableName = "CHAIN_SET_PERCENT_SET";
                                    withStoreRID = false;
                                    //  this just uses the HN_RID for the row deletion, Store RID is not needed but populated with undefined.
                                    storeRID = Include.Undefined;
                                    sglRID = ((ChangeProfileChainSet)_kvp.Value).SGL_RID;
                                    timeID = ((ChangeProfileChainSet)_kvp.Value).TimeID;
                                    returnCode = _mhd.DeleteCSPSet(node, sglRID, timeID);
                                    break;
                                case eProfileType.PurgeCriteria:
                                    //  this uses HN_RID for deletion
                                    tableName = "PURGE_CRITERIA";
                                    //  clear the cells that were affected by this change ONLY
                                    columnName = ((ChangeProfilePurge)_kvp.Value).ChangedObjectName;
                                    storeRID = Include.Undefined;
                                    withStoreRID = false;
                                    //dbParams = new MIDDbParameter[] {
                                    //    new MIDDbParameter("@HN_RID", node, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@COLUMNNAME", columnName, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@COLVALUE", Include.Undefined, eDbType.Int, eParameterDirection.Input)
                                    //};
                                    //  udpate tablename set columnname = value where hn_rid = hn_rid
                                    //_mhd.UpdateColumnByTable(ref dbParams);
                                    _mhd.UpdateColumnByTable(HN_RID: node,
                                                             ST_RID: storeRID,
                                                             COLUMNNAME: columnName,
                                                             TABLENAME: tableName,
                                                             COLVALUE: Include.Undefined
                                                             );
                                    break;
                                case eProfileType.StoreCharacteristics:
                                    //  tableName = STORE_CHAR
                                    //  this uses the SC_RID 
                                    tableName = "STORE_CHAR";
                                    withStoreRID = false;
                                    //dbParams = new MIDDbParameter[] {
                                    //    new MIDDbParameter("@RID", node, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@COLUMNNAME", columnName, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input)
                                    //};
                                    //  delete from tablename where SC_RID = SC_RID
                                    //_mhd.DeleteFromTableWithColumnName(ref dbParams);
                                    _mhd.DeleteFromTableWithColumnName(RID: node, COLUMNAME: columnName, TABLENAME: tableName);
                                    break;
                                case eProfileType.StoreEligibility:
                                    withStoreRID = false;
                                    tableName = "STORE_ELIGIBILITY";
                                    storeRID = ((ChangeProfileElig)_kvp.Value).StoreRID;
                                    columnName = ((ChangeProfileElig)_kvp.Value).ChangedObjectName;
                                    switch (columnName)
                                    {
                                        case "STKMOD_TYPE":
                                            othercols = new string[] { "STKMOD_RID", "STKMOD_PCT" };
                                            colvalue = 0;
                                            break;
                                        case "STKMOD_RID":
                                            othercols = new string[] { "STKMOD_TYPE", "STKMOD_PCT" };
                                            colvalue = Include.Undefined;
                                            break;
                                        case "SLSMOD_TYPE":
                                            othercols = new string[] { "SLSMOD_RID", "SLSMOD_PCT" };
                                            colvalue = 0; 
                                            break;
                                        case "SLSMOD_RID":
                                            othercols = new string[] { "SLSMOD_TYPE", "SLSMOD_PCT" };
                                            colvalue = Include.Undefined;
                                            break;
                                        case "FWOSMOD_TYPE":
                                            othercols = new string[] { "FWOSMOD_RID", "FWOSMOD_PCT" };
                                            colvalue = 0; 
                                            break;
                                        case "FWOSMOD_RID":
                                            othercols = new string[] { "FWOSMOD_TYPE", "FWOSMOD_PCT" };
                                            colvalue = Include.Undefined;
                                            break;
                                        case "EM_RID":
                                            othercols = new string[] { "STKMOD_RID", "STKMOD_PCT", "STKMOD_TYPE", "SLSMOD_RID", "SLSMOD_PCT", 
                                                "SLSMOD_TYPE", "FWOSMOD_RID", "FWOSMOD_PCT", "FWOSMOD_TYPE"
                                            };
                                            colvalue = Include.Undefined;
                                            break;
                                        case "SIMILAR_STORE_TYPE":
                                            othercols = new string[] { "SIMILAR_STORE_RATIO", "UNTIL_DATE" };
                                            colvalue = 0;
                                            break;
                                        case "SIMILAR_STORE_RATIO":
                                            othercols = new string[] { "SIMILAR_STORE_TYPE", "UNTIL_DATE" };
                                            colvalue = 0;
                                            break;
                                        case "UNTIL_DATE":
                                            othercols = new string[] { "SIMILAR_STORE_RATIO", "SIMILAR_STORE_TYPE" };
                                            colvalue = Include.Undefined;
                                            break;
                                    }
                                    //dbParams = new MIDDbParameter[] {
                                    //    new MIDDbParameter("@HN_RID", node, eDbType.Int, eParameterDirection.Input),
                                    //    new MIDDbParameter("@ST_RID", storeRID, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@COLUMNNAME", columnName, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input),
                                    //    new MIDDbParameter("@COLVALUE", colvalue, eDbType.Int, eParameterDirection.Input)
                                    //};
                                    //  udpate tablename set columnname = value where hn_rid = hn_rid
                                    //_mhd.UpdateColumnByTable(ref dbParams);
                                    _mhd.UpdateColumnByTable(HN_RID: node,
                                                             ST_RID: storeRID,
                                                             COLUMNNAME: columnName,
                                                             TABLENAME: tableName,
                                                             COLVALUE: colvalue
                                                             );

                                    if (othercols.Length > 0)
                                    {
                                        foreach (string col in othercols)
                                        {

                                            switch (col)
                                            {
                                                case "STKMOD_TYPE":
                                                    colvalue = 0;
                                                    break;
                                                case "STKMOD_PCT":
                                                    colvalue = 0;
                                                    break;
                                                case "STKMOD_RID":
                                                    colvalue = Include.Undefined;
                                                    break;
                                                case "SLSMOD_TYPE":
                                                    colvalue = 0;
                                                    break;
                                                case "SLSMOD_PCT":
                                                    colvalue = 0;
                                                    break;
                                                case "SLSMOD_RID":
                                                    colvalue = Include.Undefined;
                                                    break;
                                                case "FWOSMOD_TYPE":
                                                    colvalue = 0;
                                                    break;
                                                case "FWOSMOD_PCT":
                                                    colvalue = 0;
                                                    break;
                                                case "FWOSMOD_RID":
                                                    colvalue = Include.Undefined;
                                                    break;
                                                case "UNTIL_DATE":
                                                    colvalue = Include.Undefined;
                                                    break;
                                                case "SIMILAR_STORE_RATIO":
                                                    colvalue = 0;
                                                    break;
                                                case "SIMILAR_STORE_TYPE":
                                                    colvalue = 0;
                                                    break;
                                            }
                                            //dbParams = new MIDDbParameter[] {
                                            //    new MIDDbParameter("@HN_RID", node, eDbType.Int, eParameterDirection.Input),
                                            //    new MIDDbParameter("@ST_RID", storeRID, eDbType.VarChar, eParameterDirection.Input),
                                            //    new MIDDbParameter("@COLUMNNAME", col, eDbType.VarChar, eParameterDirection.Input),
                                            //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input),
                                            //    new MIDDbParameter("@COLVALUE", colvalue, eDbType.Int, eParameterDirection.Input)
                                            //};
                                            //_mhd.UpdateColumnByTable(ref dbParams);
                                            //Begin TT#1291-MD -jsobek -Store Eligibility Apply Changes to Lower Levels Error
                                            _mhd.UpdateColumnByTable(HN_RID: node,
                                                                     ST_RID: storeRID,
                                                                     COLUMNNAME: col,
                                                                     TABLENAME: tableName,
                                                                     COLVALUE: colvalue
                                                                     );
                                            //End TT#1291-MD -jsobek -Store Eligibility Apply Changes to Lower Levels Error
                                        }
                                    }
                                    break;
                            }
                            //  remove data in the database for each descendant
                            //  if the table CAN use the StoreRID then it will call this
                            if (withStoreRID)
                            {
                                //dbParams = new MIDDbParameter[] {
                                //    new MIDDbParameter("@HN_RID", node, eDbType.Int, eParameterDirection.Input),
                                //    new MIDDbParameter("@ST_RID", storeRID, eDbType.Int, eParameterDirection.Input),
                                //    new MIDDbParameter("@TABLENAME", tableName, eDbType.VarChar, eParameterDirection.Input)
                                //};
                                //_mhd.DeleteFromTableWithStoreRID(ref dbParams);
                                _mhd.DeleteFromTableWithStoreRID(HN_RID: node, ST_RID: storeRID, TABLENAME: tableName);
                            }
                        }
                    }
                    //  remove the items
                    _nodeChangeProfile.NodeChanges[key].Clear();
                }
                _mhd.CommitData();
                
                success = true;
            }
            catch (Exception ex)
            {
                //  if there is an error, close up the connection
                success = false;
                EventLog.WriteEntry("MIDHierarchySession", "UpdateLowerLevelNodes error:" + ex.Message, EventLogEntryType.Error);
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
            }
            return success;
        }

        //  return the total number of descendants for this nodeRID
        public int TotalDescendants(int nodeRID)
        {
            try
            {
                return _mhd.GetTotalNumberDescendants(nodeRID);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetAllDescendants(int nodeRID)
        {
            try
            {
                return _mhd.GetAllDescendants(nodeRID);
            }
            catch
            {
                throw;
            }
        }

        public int TotalAffectedDescendants(int nodeRID, int IMO, int SE, int CHR, int SC, int DP, int PC, int CSP)
        {
            try
            {
                return _mhd.GetTotalAffectedDescendants(nodeRID, IMO, SE, CHR, SC, DP, PC, CSP);
            }
            catch
            {
                throw;
            }
        }

        public int LockedDescendants(int nodeRID)
        {
            try
            {
                return _mhd.GetLockedDescendants(nodeRID).Rows.Count;
            }
            catch
            {
                throw;
            }
        }

        public int LockedAncestors(int nodeRID)
        {
            try
            {
                return _mhd.GetLockedAncestors(nodeRID).Rows.Count;
            }
            catch
            {
                throw;
            }
        }
        //  END TT#2015 - gtaylor - apply changes to lower levels
        #endregion

		/// <summary>
		/// Requests the session lock a branch of a hierarchy for update.
		/// </summary>
		/// <param name="aHierarchyRID">The record id of the hierarchy that contains the branch</param>
		/// <param name="aNodeRID">The record id of the root node of the branch</param>
		/// <param name="aAllowReadOnly">This flag identifies if the lock type can be changed to read only if an update
		/// lock can not be obtained</param>
		/// <returns>A flag identifying if the branch has been locked for update</returns>
		public bool LockHierarchyBranchForUpdate(int aHierarchyRID, int aNodeRID, bool aAllowReadOnly)
		{
            string errMsg;
			bool enqueueWriteLocked = false;
			bool branchedLocked = false;
            System.Windows.Forms.DialogResult diagResult;
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;

                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                //HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                //    aHierarchyRID,
                //    aNodeRID,
                //    SessionAddressBlock.ClientServerSession.UserRID,
                //    SessionAddressBlock.ClientServerSession.ThreadID,
                //    HierarchyServerGlobal.GetNodeAncestorList(aNodeRID, aHierarchyRID),
                //    HierarchyServerGlobal.GetNodeDescendantList(aNodeRID, aHierarchyRID, eNodeSelectType.All));
                HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                    aHierarchyRID,
                    aNodeRID,
                    SessionAddressBlock.ClientServerSession.UserRID,
                    SessionAddressBlock.ClientServerSession.ThreadID);
                // End TT#2015

				try
				{
					hierarchyBranchEnqueue.EnqueueHierarchyBranch();
					branchedLocked = true;
				}
				catch (HierarchyBranchConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
					enqueueWriteLocked = false;

                    errMsg = "The following hierarchy node requested:" + System.Environment.NewLine;
                    foreach (HierarchyNodeConflict HBranchCon in hierarchyBranchEnqueue.ConflictList)
                    {
                        HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(HBranchCon.HnRID);
                        errMsg += System.Environment.NewLine + "Node: " + hnp.Text + ", User: " + HBranchCon.UserName;
                    }
                    errMsg += System.Environment.NewLine + System.Environment.NewLine;
                    //errMsg += "Can not be deleted at this time";
                    errMsg += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
                    diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
                        errMsg,
                        "Hierarchy Node Conflict",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);

                }
				return branchedLocked;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session lock a branch of a hierarchy for delete.
		/// </summary>
		/// <param name="aHierarchyRID">The record id of the hierarchy that contains the branch</param>
		/// <param name="aNodeRID">The record id of the root node of the branch</param>
		/// <returns>A flag identifying if the branch has been locked for delete</returns>
		public bool LockHierarchyBranchForDelete(int aHierarchyRID, int aNodeRID)
		{
			string errMsg;
			bool enqueueWriteLocked = false;
			System.Windows.Forms.DialogResult diagResult;
			bool branchedLocked = false;
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;

                // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                //HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                //    aHierarchyRID,
                //    aNodeRID,
                //    SessionAddressBlock.ClientServerSession.UserRID,
                //    SessionAddressBlock.ClientServerSession.ThreadID,
                //    HierarchyServerGlobal.GetNodeAncestorList(aNodeRID, aHierarchyRID),
                //    HierarchyServerGlobal.GetNodeDescendantList(aNodeRID, aHierarchyRID, eNodeSelectType.All));
                HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                    aHierarchyRID,
                    aNodeRID,
                    SessionAddressBlock.ClientServerSession.UserRID,
                    SessionAddressBlock.ClientServerSession.ThreadID);
                // End TT#2015

				try
				{
					hierarchyBranchEnqueue.EnqueueHierarchyBranchForDelete();
					branchedLocked = true;
				}
				catch (HierarchyBranchConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
					enqueueWriteLocked = false;
					errMsg = "The following hierarchy node requested:" + System.Environment.NewLine;
					foreach (HierarchyBranchConflict HBranchCon in hierarchyBranchEnqueue.ConflictList)
					{
						HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(HBranchCon.NodeRID);
						errMsg += System.Environment.NewLine + "Node: " + hnp.Text + ", User: " + HBranchCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
                    errMsg += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
                    ;
					diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
						errMsg,
						"Hierarchy Node Conflict",
						System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
				}
				return branchedLocked;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session lock a branch of a hierarchy for delete.
		/// </summary>
		/// <param name="aNodeList">
		/// An instance of the HierarchyNodeList class containing an instance of the HierarchyNodeProfile class
		/// for each branch to lock.
		/// </param>
		/// <returns>
		/// An instance of the NodeLockConflictList class containing instances of the NodeLockConflictProfile
		/// class for each lock conflict.  If the NodeLockConflictList is empty, all locks were successful.
		/// </returns>
		public NodeLockConflictList LockHierarchyBranchForDelete(NodeLockRequestList aNodeList)
		{
			bool enqueueWriteLocked = false;
            //bool branchedLocked = true;
			NodeLockConflictProfile conflictProfile;
			NodeLockConflictList conflictList = new NodeLockConflictList(eProfileType.NodeLockConflict);
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;

				foreach (NodeLockRequestProfile lockRequest in aNodeList)
				{
                    if (lockRequest.NodeType == eProfileType.HierarchyNode)
                    {
                        // Begin TT#2015 - JSmith - Apply Changes to Low Levels in Node Properties
                        //HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                        //    lockRequest.HierarchyRID,
                        //    lockRequest.Key,
                        //    SessionAddressBlock.ClientServerSession.UserRID,
                        //    SessionAddressBlock.ClientServerSession.ThreadID,
                        //    HierarchyServerGlobal.GetNodeAncestorList(lockRequest.Key, lockRequest.HierarchyRID),
                        //    HierarchyServerGlobal.GetNodeDescendantList(lockRequest.Key, lockRequest.HierarchyRID, eNodeSelectType.All));
                        HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
                            lockRequest.HierarchyRID,
                            lockRequest.Key,
                            SessionAddressBlock.ClientServerSession.UserRID,
                            SessionAddressBlock.ClientServerSession.ThreadID);
                        // End TT#2015

                        try
                        {
                            hierarchyBranchEnqueue.EnqueueHierarchyBranchForDelete();
                        }
                        catch (HierarchyBranchConflictException)
                        {
                            foreach (HierarchyBranchConflict HBranchCon in hierarchyBranchEnqueue.ConflictList)
                            {
                                if (!conflictList.Contains(HBranchCon.NodeRID))
                                {
                                    conflictProfile = new NodeLockConflictProfile(HBranchCon.NodeRID);
                                    conflictProfile.BranchHierarchyRID = lockRequest.HierarchyRID;
                                    conflictProfile.BranchNodeRID = lockRequest.Key;
                                    conflictProfile.InUseByUserRID = HBranchCon.UserRID;
                                    conflictProfile.InUseByUserName = HBranchCon.UserName;
                                    HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(HBranchCon.NodeRID, false);
                                    conflictProfile.InUseNodeName = hnp.Text;
                                    conflictList.Add(conflictProfile);
                                }
                            }
                        }
                    }
				}
				return conflictList;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session dequeue the branch of the node.
		/// </summary>
		/// <param name="aHierarchyRID">The record id of the hierarchy that contains the branch</param>
		/// <param name="aNodeRID">The record id of the node</param>
		public void DequeueBranch(int aHierarchyRID, int aNodeRID)
		{
			try
			{
				HierarchyBranchEnqueue hierarchyBranchEnqueue = new HierarchyBranchEnqueue(
					aHierarchyRID,
					aNodeRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					hierarchyBranchEnqueue.DequeueHierarchyBranch();

				}
				catch 
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node record ID.
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <remarks>
		/// This method will populate parentRID
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		public HierarchyNodeProfile GetNodeData(int hierarchyRID, int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(hierarchyRID, nodeRID, true);
			}
			catch
			{
				throw;
			}
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node record ID.
		/// </summary>
		/// <param name="aHierarchyRID">The record id of the hierarchy</param>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
		/// <remarks>This method will populate parentRID</remarks>
		public HierarchyNodeProfile GetNodeData(int aHierarchyRID, int aNodeRID, bool aBuildQualifiedID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(aHierarchyRID, aNodeRID, true, aBuildQualifiedID);
			}
			catch
			{
				throw;
			}
		}
		//End Track #5378

        // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
        /// <summary>
        /// Requests the session get node information from the hierarchy global area using the node record ID.
        /// </summary>
        /// <param name="aHierarchyRID">The record id of the hierarchy</param>
        /// <param name="aNodeRID">The record id of the node</param>
        /// /// <param name="aChaseHierarchy">
        /// This switch identifies if the routine is to chase the hierarchy
        /// for settings if they are not set on the requested node
        /// </param>
        /// <param name="aBuildQualifiedID">
        /// This switch identifies that the node is being looked up should
        /// have the ID include parents when necessary to identify
        /// </param>
        /// <returns>An instance of the HierarchyNodeProfile object containing information about the node</returns>
        public HierarchyNodeProfile GetNodeData(int aHierarchyRID, int aNodeRID, bool aChaseHierarchy, bool aBuildQualifiedID)
        {
            try
            {
                return HierarchyServerGlobal.GetNodeData(aHierarchyRID, aNodeRID, aChaseHierarchy, aBuildQualifiedID);
            }
            catch
            {
                throw;
            }
        }
        //End TT#668

		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node id.
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <returns>An instance of the HierarchyNodeProfile class containing information about the node</returns>
		/// <remarks>
		/// This method will populate the parentRID with the parent in the home hierarchy.
		/// This method will chase the hierarchy
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		public HierarchyNodeProfile GetNodeData(string nodeID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(nodeID, true);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node id.
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="chaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile class containing information about the node</returns>
		/// <remarks>
		/// This method will populate the parentRID with the parent in the home hierarchy
		/// This will not return a qualified node ID for colors and sizes
		/// </remarks>
		public HierarchyNodeProfile GetNodeData(string nodeID, bool chaseHierarchy)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(nodeID, chaseHierarchy);
			}
			catch
			{
				throw;
			}
		}
        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details
        public HierarchyNodeProfile GetNodeDataWithQualifiedID(string nodeID, bool chaseHierarchy)
		{
			try
			{
                return HierarchyServerGlobal.GetNodeDataWithQualifiedID(nodeID, chaseHierarchy);
			}
			catch
			{
				throw;
			}
		}
        //End TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details

        //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
        public HierarchyNodeProfile GetNodeDataFromBaseSearchString(string searchString)
        {
            try
            {
                return HierarchyServerGlobal.GetNodeDataFromBaseSearchString(searchString);
            }
            catch
            {
                throw;
            }
        }
        
        //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Requests the session get node information from the hierarchy global area using the node id.
		/// </summary>
		/// <param name="aNodeID">The id of the node</param>
		/// <param name="aChaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for settings if they are not set on the requested node
		/// </param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>An instance of the HierarchyNodeProfile class containing information about the node</returns>
		/// <remarks>This method will populate the parentRID with the parent in the home hierarchy</remarks>
		public HierarchyNodeProfile GetNodeData(string aNodeID, bool aChaseHierarchy, bool aBuildQualifiedID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeData(aNodeID, aChaseHierarchy, aBuildQualifiedID);
			}
			catch
			{
				throw;
			}
		}
		//End Track #5378


        /// <summary>
        /// Retrieve assortment node
        /// </summary>
        /// <returns>HierarchyNodeProfile of the assortment node</returns>
        public HierarchyNodeProfile GetAssortmentNode()
        {
            try
            {
                return HierarchyServerGlobal.GetAssortmentNode();
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Requests the session get the record id of the node from the hierarchy global area using the node id.
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <returns>The record ID of the node or -1 if the node ID is not found</returns>
		public int GetNodeRID(string nodeID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeRID(nodeID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Determines the record id of the node in the hierarchy
		/// </summary>
		/// <param name="aHnp">A HierarchyNodeProfile containing information of the node</param>
		/// <returns>The record ID of the node or -1 if the node is not found</returns>
		public int GetNodeRID(HierarchyNodeProfile aHnp)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeRID(aHnp);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the id of the node from the hierarchy global area using the node record id.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public string GetNodeID(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeID(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the text of the node from the hierarchy global area using the node record id.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public string GetNodeText(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeText(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the id of the node and the parent node IDfrom the hierarchy global area using the node record id.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node</param>
		public HierarchyNodeAndParentIdsProfile GetNodeIDAndParentID(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeIDAndParentID(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the record id of the node from the hierarchy global area using the node id.
		/// </summary>
		/// <param name="aNodeID">The id of the node</param>
		/// <param name="aLevelDelimiter">The delimiter used to separate the levels of the node</param>
		/// <param name="aParentID">The id of the parent</param>
		/// <returns>The record ID of the node or -1 if the node ID is not found</returns>
		public int GetNodeRID(string aNodeID, char aLevelDelimiter, string aParentID)
		{
			try
			{
				int nodeRID = Include.NoRID;
				int parentRID = GetNodeRID(aParentID);
				// check for compound node
				string[] fields = MIDstringTools.Split(aNodeID,aLevelDelimiter,true);
				if (fields.Length == 1)
				{
				}
				else
					if (fields.Length > 1)
				{
				}
				return nodeRID;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a Hashtable of the nodes in the system keyed by ID.
		/// </summary>
		public Hashtable GetNodeIDHash()
		{
			try
			{
				return HierarchyServerGlobal.GetNodeListByID();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Determines if the node exists in the hierarchy
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>A flag identifying if the node exists</returns>
		public bool NodeExists(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.NodeExists(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Lookup node profile information for a nodeID
		/// </summary>
		/// <param name="aNodeID">The ID of the node</param>
		/// <param name="aNodeDelimiter">The character used to separate levels of the hierarchy</param>
		/// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
		/// <param name="aLookupParent">A flag identifying if the parentID is to be returned</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="oParentID">Returns the ID of the parent</param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the node information 
		/// and the ID of the parent of the first node.</returns>
		/// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
		public HierarchyNodeProfile NodeLookup(string aNodeID, char aNodeDelimiter, bool aProcessingAutoAdd, bool aLookupParent, bool aChaseHierarchy, out string oParentID)
		{
			try
			{
				oParentID = null;
				int colorNodeRID = -1;
				int sizeNodeRID = -1;
				string[] fields = null;
				HierarchyNodeProfile hnp = new HierarchyNodeProfile(-1);
				if (aNodeID.IndexOf(aNodeDelimiter) > -1)  // compound node using variations of style\color\size
				{
					NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByID(aNodeID);
					if (ni.NodeRID != Include.NoRID)
					{
						//Begin Track #5378 - color and size not qualified
//						hnp = GetNodeData(ni.NodeRID, aChaseHierarchy);
						hnp = GetNodeData(ni.NodeRID, aChaseHierarchy, true);
						//End Track #5378
					}
					else
					{
						fields = MIDstringTools.Split(aNodeID, aNodeDelimiter, true);
						if (fields.Length > 3)  
						{
							throw new FormatInvalidException();
						}
						else
						{
							//Begin Track #5378 - color and size not qualified
//							HierarchyNodeProfile style_hnp = GetNodeData(fields[0], aChaseHierarchy);
							HierarchyNodeProfile style_hnp = GetNodeData(fields[0], aChaseHierarchy, true);
							//End Track #5378
							if (style_hnp.Key == -1)
							{
								if (!aProcessingAutoAdd)
								{
									throw new ProductNotFoundException();
								}
							}
							else
								if (style_hnp.LevelType != eHierarchyLevelType.Style)
							{
								throw new FirstLevelNotStyleException();
							}
							else
								if(ColorExistsForStyle(style_hnp.HomeHierarchyRID, style_hnp.Key, fields[1], aNodeID,  ref colorNodeRID))
							{
								//Begin Track #5378 - color and size not qualified
//								HierarchyNodeProfile color_hnp = GetNodeData(colorNodeRID, aChaseHierarchy);
								HierarchyNodeProfile color_hnp = GetNodeData(colorNodeRID, aChaseHierarchy, true);
								//End Track #5378
								if (fields.Length == 2)  //  color node so return color node profile
								{
									hnp = color_hnp;
								}
								else  // size node
								{
									if(SizeExistsForColor(color_hnp.HomeHierarchyRID, color_hnp.Key, fields[2], aNodeID, ref sizeNodeRID))
									{
										//Begin Track #5378 - color and size not qualified
//										hnp = GetNodeData(sizeNodeRID, aChaseHierarchy);
										hnp = GetNodeData(sizeNodeRID, aChaseHierarchy, true);
										//End Track #5378
									}
									else if (!aProcessingAutoAdd)
									{
										throw new ProductNotFoundException();
									}
								}
							}
							else if (!aProcessingAutoAdd)
							{
								throw new ProductNotFoundException();
							}
						}
					}
				}
				else
				{
					//Begin Track #5378 - color and size not qualified
//					hnp = GetNodeData(aNodeID, aChaseHierarchy);
					hnp = GetNodeData(aNodeID, aChaseHierarchy, true);
					//End Track #5378
				}

				// lookup parent if needed
				if (aLookupParent &&
					hnp.Key == Include.NoRID)
				{
					string ID;
					if (fields == null)
					{
						ID = aNodeID;
					}
					else
					{
						ID = fields[0];
					}
					HierarchyNodeProfile parent_hnp = GetAncestorData(ID, 1);
					oParentID = parent_hnp.NodeID; 
				}

				return hnp;
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Looks up the record IDs of a list of nodes
		/// </summary>
		/// <param name="aSAB">The SessionAddressBlock object</param>
		/// <param name="aNodeHash">A Hashtable containing NodeLookup objects keyed by the product ID</param>
		/// <param name="aAllowAutoAdds">A flag identifying if products can be auto added</param>
		/// <param name="aCommitLimit">The number of records to process before a commit is executed</param>
		/// <returns>Updated Hashtable with the record IDs of valid nodes</returns>
		public Hashtable LookupNodes(SessionAddressBlock aSAB, Hashtable aNodeHash, bool aAllowAutoAdds,
			int aCommitLimit)
		{
			try
			{
				int nodesAdded = 0;
				HierarchyMaintenance hm = new HierarchyMaintenance(aSAB, aSAB.HierarchyServerSession);
				HierarchyNodeProfile parent_hnp = null;
				if (aNodeHash.Count > 0) 
				{
					// do not open connection until needed
					string parentID = null;
					char nodeDelimiter = GetProductLevelDelimiter();
					bool lookupParent = true;
					foreach (NodeLookup nl in aNodeHash.Values)
					{
						nl.NodeRID = HierarchyServerGlobal.GetNodeRID(nl.NodeID);
						if (nl.NodeRID == Include.NoRID)
						{
							if (nl.ParentID == null || nl.ParentID.Trim().Length == 0)
							{
								lookupParent = true;
							}
							else
							{
								lookupParent = false;
							}
							parentID = nl.ParentID;
							HierarchyNodeProfile hnp = null;
							bool continueProcessing = true;
							try
							{
								hnp = NodeLookup(nl.NodeID, nodeDelimiter, aAllowAutoAdds, lookupParent, false, out parentID);
							}
							catch(FirstLevelNotStyleException)
							{
								continueProcessing = false;
							}
							catch(FormatInvalidException)
							{
								continueProcessing = false;
							}
							catch(ProductNotFoundException)
							{
								continueProcessing = true;
							}
							catch
							{
								continueProcessing = false;
							}

							if (continueProcessing )
							{
								// found node, so use key
								if (hnp != null &&
									hnp.Key != Include.NoRID)
								{
									nl.NodeRID = hnp.Key;
								}
								else
								{
									if (lookupParent)
									{
										// use parent from lookup
										nl.ParentID = parentID;
									}
									// make sure there is a parent
									if (nl.ParentID != null && nl.ParentID.Trim().Length > 0)
									{
										// verify and get parent data
										if (parent_hnp == null || parent_hnp.NodeID != nl.ParentID)
										{
											parent_hnp = GetNodeData(nl.ParentID, false);
										}

										// parent is valid
										if (parent_hnp.Key != Include.NoRID &&
											aAllowAutoAdds)
										{
											try
											{
												nl.EditMsgs = new EditMsgs();

												// Begin Track #5307 - JSmith - duplicate nodes on hierarchy
//												OpenUpdateConnection(eLockType.HierarchyNode, Include.CreateNodeLockKey(parent_hnp.Key, nl.NodeID));
//												// recheck for node now that lock is set to see if node was added
//												// by another process
//												try
//												{
//													hnp = NodeLookup(nl.NodeID, nodeDelimiter, aAllowAutoAdds, lookupParent, false, out parentID);
//												}
//												catch(ProductNotFoundException)
//												{
//													continueProcessing = true;
//												}
//												catch
//												{
//													continueProcessing = false;
//												}
												// moved locking and recheck
												OpenUpdateConnection();
												// End Track #5307
												// found node, so use key
												if (continueProcessing)
												{
													if (hnp != null &&
														hnp.Key != Include.NoRID)
													{
														nl.NodeRID = hnp.Key;
													}
													else
													{
														nl.NodeRID = AutoAddMerchandise(aSAB, hm, parent_hnp, nodeDelimiter, nl);
														CommitData();
													}
												}
											}
											catch
											{
												throw;
											}
											finally
											{
												if (UpdateConnectionIsOpen())
												{
													CloseUpdateConnection();
												}
											}
											nodesAdded += nl.NodesAdded;
										}
									}
								}
							}

							// record ID determined, add to global cache
							if (nl.NodeRID != Include.NoRID)
							{
								HierarchyServerGlobal.AddNodeRIDByID(nl.NodeID, nl.NodeRID);
							}
						}
					}
				}

                return aNodeHash;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (UpdateConnectionIsOpen())
				{
					CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Looks up the record IDs of a list of nodes
		/// </summary>
		/// <param name="aNodeList">A Hashtable containing the node RIDs to lookup</param>
		/// <returns>A Hashtable with the ancestor lists of the nodes</returns>
		public Hashtable LookupAncestors(ArrayList aNodeList)
		{
			try
			{
				Hashtable ht = new Hashtable();
				foreach (int nodeRID in aNodeList)
				{
					if (!ht.Contains(nodeRID))
					{
						NodeAncestorList nal = GetNodeAncestorList(nodeRID);
						ht[nodeRID] = nal;
					}
				}
				return ht;
			}
			catch
			{
				throw;
			}
		}

		private int AutoAddMerchandise(SessionAddressBlock aSAB, HierarchyMaintenance hm, 
			HierarchyNodeProfile aParent_hnp, char aNodeDelimiter, NodeLookup aNodeLookup)
		{
			int nodeRID = Include.NoRID;
			// Begin TT#2888 - JSmith - Nightly Sales and Inv Load Failure
			// This is temporary code until a permanent locking manager can be developed
			// which will maintain all locks while a connection is open
            MerchandiseHierarchyData dataLock = null;
			// End TT#2888 - JSmith - Nightly Sales and Inv Load Failure
			try
			{
			    // Begin TT#2888 - JSmith - Nightly Sales and Inv Load Failure
                dataLock = new MerchandiseHierarchyData();
				// End TT#2888 - JSmith - Nightly Sales and Inv Load Failure
				EditMsgs em = new EditMsgs();
				// check for style\color\size
				int parentRID = Include.NoRID;
				int startLevel = 0;
				string[] fields = MIDstringTools.Split(aNodeLookup.NodeID,aNodeDelimiter,true);
				string description = null;
				string[] descriptions = null;
				if (aNodeLookup.NodeDescription != null)
				{
					descriptions = MIDstringTools.Split(aNodeLookup.NodeDescription,aNodeDelimiter,true);
				}
				string name = null;
				string[] names = null;
				if (aNodeLookup.NodeName != null)
				{
					names = MIDstringTools.Split(aNodeLookup.NodeName,aNodeDelimiter,true);
				}
				
				parentRID = aParent_hnp.Key;
                // Begin Track #5247 - JSmith - null reference during QuickAdd
                if (aParent_hnp.HomeHierarchyType == eHierarchyType.organizational)
                {
                    if (_hp == null)
                    {
                        _hp = HierarchyServerGlobal.GetHierarchyData(aParent_hnp.HomeHierarchyRID);
                    }
                    if (aParent_hnp.HomeHierarchyLevel + fields.Length > _hp.HierarchyLevels.Count)
                    {
                        aNodeLookup.EditMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AddBeyondLevels, this.GetType().Name);
                        return Include.NoRID;
                    }
                }
                // End Track #5247

                try
                {
				    // Begin TT#2888 - JSmith - Nightly Sales and Inv Load Failure
                    dataLock.OpenUpdateConnection(eLockType.HierarchyNode, "AA_" + fields[0]);
					// End TT#2888 - JSmith - Nightly Sales and Inv Load Failure
                    // autoadd levels
                    for (int i = startLevel; i < fields.Length; i++)
                    {
                        if (aNodeLookup.NodeDescription != null &&
                            i < descriptions.Length)
                        {
                            description = descriptions[i];
                        }
                        else
                        {
                            description = null;
                        }
                        if (aNodeLookup.NodeName != null &&
                            i < names.Length)
                        {
                            name = names[i];
                        }
                        else
                        {
                            name = null;
                        }
                        bool nodeAdded = false;
                        nodeRID = hm.QuickAdd(ref em, parentRID, fields[i],
                            description, name,
                            aNodeLookup.SizeProductCategory, aNodeLookup.SizePrimary,
                            aNodeLookup.SizeSecondary, ref nodeAdded);
                        if (!em.ErrorFound)
                        {
                            if (nodeAdded)
                            {
                                ++aNodeLookup.NodesAdded;
                            }
                            parentRID = nodeRID;
                        }
                        else
                        {
                            // transfer errors to main error class
                            for (int e = 0; e < em.EditMessages.Count; e++)
                            {
                                EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
                                aNodeLookup.EditMsgs.AddMsg(emm.messageLevel, emm.code, emm.msg, emm.module);
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
				// Begin TT#2888 - JSmith - Nightly Sales and Inv Load Failure
                finally
                {
                    if (dataLock != null && dataLock.ConnectionIsOpen)
                    {
                        dataLock.RemoveLocks();
                        dataLock.CloseUpdateConnection();
                    }
                }
				// End TT#2888 - JSmith - Nightly Sales and Inv Load Failure
			}
			catch (System.Net.Sockets.SocketException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
			}
			return nodeRID;
		}


        /// <summary>
        /// Requests the session count to number of descendants under a node.
        /// </summary>
		/// <param name="HierarchyRID">The record ID of the hierarchy to be counted</param>
		/// <param name="nodeRID">The record ID of the node to be counted</param>
		/// <remarks>The count does not include the node</remarks>
		/// <returns></returns>
		public int GetDescendantCount(int HierarchyRID, int nodeRID)
		{
			try
			{
                // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
                //return HierarchyServerGlobal.GetDescendantCount(HierarchyRID, nodeRID);
                return HierarchyServerGlobal.GetDescendantCount(HierarchyRID, nodeRID, false);
                // End TT#222
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#222 - JSmith - Apply to lower levels in alternate affects guest nodes
        /// <summary>
        /// Requests the session count to number of descendants under a node.
        /// </summary>
        /// <param name="HierarchyRID">The record ID of the hierarchy to be counted</param>
        /// <param name="nodeRID">The record ID of the node to be counted</param>
        /// <param name="aHomeHierarchyOnly">Flag identifies that only home nodes are to be counted</param>
        /// <remarks>The count does not include the node</remarks>
        /// <returns></returns>
        public int GetDescendantCount(int HierarchyRID, int nodeRID, bool aHomeHierarchyOnly)
        {
            try
            {
                return HierarchyServerGlobal.GetDescendantCount(HierarchyRID, nodeRID, aHomeHierarchyOnly);
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#222

		/// <summary>
		/// Retrieves the ancestor node information based on a level sequence of a hierarchy
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelSequence">The sequence of the level to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public HierarchyNodeProfile GetAncestorDataByLevel(int hierarchyRID, int nodeRID, int levelSequence)
		{
			try
			{
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(hierarchyRID, nodeRID, true);
				int levelOffset = levelSequence - hnp.NodeLevel;
				return HierarchyServerGlobal.GetAncestorData(hierarchyRID, nodeRID, levelOffset);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, int levelOffset)
		{
			try
			{
				return HierarchyServerGlobal.GetAncestorData(hierarchyRID, nodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeProfile GetAncestorData(int nodeRID, int levelOffset)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				return HierarchyServerGlobal.GetAncestorData(ni.HomeHierarchyRID, ni.NodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeProfile GetAncestorData(string nodeID, int levelOffset)
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByID(nodeID);
				return HierarchyServerGlobal.GetAncestorData(ni.HomeHierarchyRID, ni.NodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile class containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, eHierarchyLevelType levelType)
		{
			try
			{
				int levelIndex = 0;
				int levelOffset = 0;
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(hierarchyRID, nodeRID, true);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(hierarchyRID);
				for (levelIndex = hnp.NodeLevel; levelIndex > 0; levelIndex--)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
					if (hlp.LevelType == levelType)
					{
						break;
					}
					else
					{
						++levelOffset;
					}
				}
				return HierarchyServerGlobal.GetAncestorData(hierarchyRID, nodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeProfile GetAncestorData(int nodeRID, eHierarchyLevelType levelType)
		{
			try
			{
				int levelIndex = 0;
				int levelOffset = 0;
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(ni.HomeHierarchyRID);
				for (levelIndex = ni.HomeHierarchyLevel; levelIndex > 0; levelIndex--)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
					if (hlp.LevelType == levelType)
					{
						break;
					}
					else
					{
						++levelOffset;
					}
				}
				return HierarchyServerGlobal.GetAncestorData(ni.HomeHierarchyRID, ni.NodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeProfile GetAncestorData(string nodeID, eHierarchyLevelType levelType)
		{
			try
			{
				int levelIndex = 0;
				int levelOffset = 0;
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByID(nodeID);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(ni.HomeHierarchyRID);
				for (levelIndex = ni.HomeHierarchyLevel; levelIndex > 0; levelIndex--)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
					if (hlp.LevelType == levelType)
					{
						break;
					}
					else
					{
						++levelOffset;
					}
				}
				return HierarchyServerGlobal.GetAncestorData(ni.HomeHierarchyRID, ni.NodeRID, levelOffset);
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="idPrefix">
		/// The prefix of the Node ID that will be selected. The first node encountered in the ancestor 
		/// list whose ID matches this string will be selected.
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public HierarchyNodeProfile GetAncestorData(int hierarchyRID, int nodeRID, string idPrefix)
		{
			try
			{
				return HierarchyServerGlobal.GetAncestorData(hierarchyRID, nodeRID, idPrefix);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the ancestor node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="idPrefix">
		/// The prefix of the Node ID that will be selected. The first node encountered in the ancestor 
		/// list whose ID matches this string will be selected.
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile object containing information about the ancestor node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeProfile GetAncestorData(int nodeRID, string idPrefix)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				return HierarchyServerGlobal.GetAncestorData(ni.HomeHierarchyRID, ni.NodeRID, idPrefix);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information based on a level in the main hierarchy
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelSequence">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
			public HierarchyNodeList GetDescendantDataByLevel(int aNodeRID, int aLevelSequence, bool aChaseHierarchy,
				eNodeSelectType aNodeSelectType)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(aNodeRID, true);
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, aNodeRID, 
					eHierarchyDescendantType.levelType, aLevelSequence, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Retrieves descendant node information based on a level in the main hierarchy
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelSequence">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		public HierarchyNodeList GetDescendantDataByLevel(int aNodeRID, int aLevelSequence, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType, bool aBuildQualifiedID)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(aNodeRID, true);
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, aNodeRID, 
					eHierarchyDescendantType.levelType, aLevelSequence, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType, aBuildQualifiedID);
			}
			catch
			{
				throw;
			}
		}
		//End Track #5378

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		public HierarchyNodeList GetDescendantData(int hierarchyRID, int nodeRID, int levelOffset, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				return HierarchyServerGlobal.GetDescendantData(hierarchyRID, nodeRID, 
					eHierarchyDescendantType.offset, levelOffset, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(int nodeRID, int levelOffset, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, true);
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, ni.NodeRID, 
					eHierarchyDescendantType.offset, levelOffset, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		//Begin Track #5378 - color and size not qualified
		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="aBuildQualifiedID">
		/// This switch identifies that the node is being looked up should
		/// have the ID include parents when necessary to identify
		/// </param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		public HierarchyNodeList GetDescendantData(int nodeRID, int levelOffset, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType, bool aBuildQualifiedID)
		{
			try
			{
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, true);
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, ni.NodeRID, 
					eHierarchyDescendantType.offset, levelOffset, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType, aBuildQualifiedID);
			}
			catch
			{
				throw;
			}
		}
		//End Track #5378

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="levelOffset">The number of levels offset of the node's level to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(string nodeID, int levelOffset, bool aChaseHierarchy,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByID(nodeID);
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, ni.NodeRID, 
					eHierarchyDescendantType.offset, levelOffset, eHierarchyLevelMasterType.Undefined,
					aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		public HierarchyNodeList GetDescendantData(int hierarchyRID, int nodeRID, eHierarchyLevelType levelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				eHierarchyDescendantType getByType = eHierarchyDescendantType.offset;
				int levelIndex = 0;
				int levelOffset = 0;
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(hierarchyRID, nodeRID, true);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(hierarchyRID);
				// must always search alternates by type since there are no level definitions
				if (hp.HierarchyType == eHierarchyType.alternate)
				{
					getByType = eHierarchyDescendantType.masterType;
				}
				else
				{
					for (levelIndex = hnp.NodeLevel; levelIndex < hp.HierarchyLevels.Count; levelIndex++)
					{
						HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
						if (hlp.LevelType == levelType)
						{
							break;
						}
						else
						{
							++levelOffset;
						}
					}
				}
				return HierarchyServerGlobal.GetDescendantData(hierarchyRID, nodeRID, getByType, levelOffset, 
					eHierarchyLevelMasterType.Undefined, aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(int nodeRID, eHierarchyLevelType levelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				return GetDescendantData(nodeRID, (eHierarchyLevelMasterType)levelType, true, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="aLevelType">The level master type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(int nodeRID, eHierarchyLevelMasterType aLevelType,
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				eHierarchyLevelType levelType;
				eHierarchyDescendantType getByType = eHierarchyDescendantType.offset;
				if (aLevelType == eHierarchyLevelMasterType.ParentOfStyle)
				{
					levelType = eHierarchyLevelType.Style;
				}
				else
				{
					levelType = (eHierarchyLevelType)aLevelType;
				}
				int levelIndex = 0;
				int levelOffset = 0;
				NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, true);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(ni.HomeHierarchyRID);
				// must always search alternates by type since there are no level definitions
				if (hp.HierarchyType == eHierarchyType.alternate)
				{
					getByType = eHierarchyDescendantType.masterType;
				}
				else
				{
					for (levelIndex = ni.HomeHierarchyLevel; levelIndex < hp.HierarchyLevels.Count; levelIndex++)
					{
						if (levelIndex == 0)
						{
							++levelOffset;
						}
						else
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
							if (hlp.LevelType == levelType)
							{
								break;
							}
							else
							{
								++levelOffset;
							}
						}
					}
					if (aLevelType == eHierarchyLevelMasterType.ParentOfStyle)
					{
						if (hp.HierarchyType == eHierarchyType.organizational)
						{
							--levelOffset;
						}
					}
				}
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, ni.NodeRID, getByType, 
					levelOffset, aLevelType, aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="levelType">The level type of the node to retrieve</param>
		/// <param name="aNodeSelectType">The type of nodes to select</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(string nodeID, eHierarchyLevelType levelType, eNodeSelectType aNodeSelectType)
		{
			try
			{
				return GetDescendantData(nodeID, (eHierarchyLevelMasterType)levelType, true, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves descendant node information
		/// </summary>
		/// <param name="nodeID">The id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <returns>
		/// An instance of the HierarchyNodeList object containing HierarchyNodeProfile objects for each descendant.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public HierarchyNodeList GetDescendantData(string nodeID, eHierarchyLevelMasterType aLevelType, 
			bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				eHierarchyLevelType levelType;
				eHierarchyDescendantType getByType = eHierarchyDescendantType.offset;
				if (aLevelType == eHierarchyLevelMasterType.ParentOfStyle)
				{
					levelType = eHierarchyLevelType.Style;
				}
				else
				{
					levelType = (eHierarchyLevelType)aLevelType;
				}
				int levelIndex = 0;
				int levelOffset = 0;
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByID(nodeID);
				HierarchyProfile hp = HierarchyServerGlobal.GetHierarchyData(ni.HomeHierarchyRID);
				// must always search alternates by type since there are no level definitions
				if (hp.HierarchyType == eHierarchyType.alternate)
				{
					getByType = eHierarchyDescendantType.masterType;
				}
				else
				{
					for (levelIndex = ni.HomeHierarchyLevel; levelIndex < hp.HierarchyLevels.Count; levelIndex++)
					{
						HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
						if (hlp.LevelType == levelType)
						{
							break;
						}
						else
						{
							++levelOffset;
						}
					}
					if (aLevelType == eHierarchyLevelMasterType.ParentOfStyle && levelOffset > 0)
					{
						--levelOffset;
					}
				}
				return HierarchyServerGlobal.GetDescendantData(ni.HomeHierarchyRID, ni.NodeRID, getByType, levelOffset, 
					eHierarchyLevelMasterType.Undefined, aChaseHierarchy, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#155 - JSmith - Size Curve Method
        /// <summary>
        /// Retrieves the descendant node list
        /// </summary>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aGetByType">Identifies the type check to use to determine the descendant</param>
        /// <param name="level">The level or number of levels offset of the node's level to retrieve</param>
        /// <param name="aLevelType">The level master type of the node to retrieve</param>
        /// <param name="aNodeSelectType">
        /// The indicator of type eNodeSelectType that identifies which type of nodes to select
        /// </param>
        /// <returns>
        /// An instance of the NodeDescendantList object containing NodeDescendantProfile objects for each descendant.
        /// </returns>
        public NodeDescendantList GetNodeDescendantList(int hierarchyRID, int aNodeRID,
            eHierarchyDescendantType aGetByType, int level, eHierarchyLevelMasterType aLevelType,
            eNodeSelectType aNodeSelectType)
        {
            try
            {
                return HierarchyServerGlobal.GetNodeDescendantList(hierarchyRID, aNodeRID,
                            aGetByType, level, aLevelType, aNodeSelectType);
            }
            catch
            {
                throw;
            }
        }

        // End TT#155

		/// <summary>
		/// Retrieves a list of keys to read to get intransit for the node
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the ArrayList object containing IntransitReadNodeProfile objects for each key.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public ArrayList GetIntransitReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				return HierarchyServerGlobal.GetIntransitReadNodes(aNodeRID, aLevelType);
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		/// <summary>
		/// Retrieves a list of keys to read to get IMO for the node
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aLevelType">The master level type of the node to retrieve</param>
		/// <returns>
		/// An instance of the ArrayList object containing IMOReadNodeProfile objects for each key.
		/// </returns>
		/// <remarks>The node's home hierarchy will be used</remarks>
		public ArrayList GetIMOReadNodes(int aNodeRID, eHierarchyLevelMasterType aLevelType)
		{
			try
			{
				return HierarchyServerGlobal.GetIMOReadNodes(aNodeRID, aLevelType);
			}
			catch
			{
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

        // BEGIN TT#1401 - gtaylor - reservation stores
        /// <summary>
        /// retrieves a node's IMO data
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <returns>
        /// a datatable containing the rows of IMO data
        /// </returns>
        /// <remarks></remarks>
        public IMOProfileList GetNodeIMOList(
            ProfileList profileList,
            int aNodeRID
            )
        {
            try
            {
                return HierarchyServerGlobal.GetNodeIMOList(
                    profileList,
                    aNodeRID,
                    true, 
                    false
                );
            }
            catch
            {
                throw;
            }
        }

        public IMOProfileList GetNodeIMOList(ProfileList storeList, int aNodeRID, bool forInheritance, bool forCopy) 
        {
            try
            {
                return HierarchyServerGlobal.GetNodeIMOList(                    
                    storeList,
                    aNodeRID, 
                    forInheritance,
                    forCopy
                );
            }
            catch
            {
                throw;
            }

        }

        public IMOProfileList GetMethodOverrideIMOList(ProfileList storeList, bool forCopy)
        {
            try
            {
                return HierarchyServerGlobal.GetMethodOverrideIMOList(
                    storeList,
                    forCopy
                );
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateNodeIMOStore(int storeRID)
        {
            try
            {
                return HierarchyServerGlobal.UpdateNodeIMOStore(storeRID);
            }
            catch
            {
                throw;
            }
        }

        // END TT#1401 - gtaylor - reservation stores

		/// <summary>
		/// Determines if the node is a child of the parent RID
		/// </summary>
		/// <param name="aHierarchyRID">The record ID of the hierarchy</param>
		/// <param name="aParentRID">The record ID of the parent</param>
		/// <param name="aNodeRID">The record ID of the child</param>
		/// <returns>
		/// A boolean indicating if the node is a child of the parent in any hierarchy
		/// </returns>
		public bool IsParentChild(int aHierarchyRID, int aParentRID, int aNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.IsParentChild(aHierarchyRID, aParentRID, aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the node information of plan level
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile class containing information about the plan level node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public NodeAncestorList GetPlanLevelPath(int nodeRID)
		{
			try
			{
				HierarchyNodeProfile anchorNode = null;
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(nodeRID);
				return HierarchyServerGlobal.DetermineOTSPlanLevelPath(hnp, out anchorNode);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the node information of plan level
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>
		/// An instance of the HierarchyNodeProfile class containing information about the plan level node.
		/// Key contains Include.Undefined if ancestor is not found
		/// </returns>
		public HierarchyNodeProfile GetPlanLevelData(int nodeRID)
		{
			try
			{
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(nodeRID);
				int OTSPlanLevelRID = HierarchyServerGlobal.DetermineOTSPlanLevelRid(hnp);
				// BEGIN MID Track #3872 - use color or style node for plan level lookup
				if (OTSPlanLevelRID == Include.NoRID) 
				{
					return null;
				}	

				if (hnp.OTSPlanLevelIsOverridden &&
					hnp.OTSPlanLevelInherited == eInheritedFrom.None)
				{
					if (OTSPlanLevelRID == hnp.Key)
					{
						return hnp;
					}
					else
					{
						return HierarchyServerGlobal.GetNodeData(OTSPlanLevelRID);
					}	
				}
				else
					if (hnp.OTSPlanLevelInherited == eInheritedFrom.Node)
				{
					return HierarchyServerGlobal.GetNodeData(OTSPlanLevelRID);
				}
				else
				{
					return new HierarchyNodeProfile(Include.NoRID);
				}
			}
			catch
			{
				throw;
			}
		}
		

		/// <summary>
		/// Requests the session make a copy of the node and add it to the provided parent.
		/// </summary>
		/// <param name="nodeRID">The record ID of the node to be copied</param>
		/// <param name="parentRID">The record ID of the parent where the node is to be added</param>
		/// <returns>An instance of the HierarchyNodeProfile class containing the new node's information</returns>
		public HierarchyNodeProfile CopyNode(int nodeRID, int parentRID)
		{
			try
			{
				HierarchyNodeProfile hnp = HierarchyServerGlobal.GetNodeData(nodeRID, false);
				if (hnp.Key == Include.NoRID)
				{
					//TODO throw exception
				}
				else
				{
					HierarchyNodeProfile parent_hnp = HierarchyServerGlobal.GetNodeData(parentRID, false);
					HierarchyNodeProfile newNode = new HierarchyNodeProfile(0);  // prime key to not be = Include.NoRID
					int copyCntr = 0;
					string newName = null;
					// determine new name
					while (newNode.Key != Include.NoRID)
					{
						copyCntr++;
						if (copyCntr > 1)
						{
							newName = "Copy" + copyCntr.ToString(CultureInfo.CurrentUICulture) + " of " + hnp.NodeID;
						}
						else
						{
							newName = "Copy of " + hnp.NodeID;
						}
						newNode = GetNodeData(newName);
					}
					hnp.NodeChangeType = eChangeType.add;
					hnp.HomeHierarchyParentRID = parentRID;
					if (hnp.LevelType == eHierarchyLevelType.Color ||
						hnp.LevelType == eHierarchyLevelType.Size)
					{
						hnp.NodeDescription = "Copy of " + hnp.NodeDescription;
					}
					else
					{
						hnp.NodeID = newName;
						hnp.NodeDescription = newName;
						hnp.NodeName = newName;
					}
					hnp.HierarchyRID = parent_hnp.HomeHierarchyRID;
					hnp.HomeHierarchyRID = parent_hnp.HomeHierarchyRID;
					hnp.HomeHierarchyLevel = parent_hnp.HomeHierarchyLevel + 1;
					hnp.HomeHierarchyType = parent_hnp.HomeHierarchyType;
                    // Begin Track #5005 - JSmith - Explorer Organization
                    hnp.HomeHierarchyOwner = parent_hnp.HomeHierarchyOwner;
                    // End Track #5005
					hnp.RollupOption = parent_hnp.RollupOption;
					_mhd.OpenUpdateConnection();		// need to open database connection
					hnp = NodeUpdateProfileInfo(hnp);
					if (hnp.NodeChangeSuccessful)
					{
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						ProfileList storeList = GetProfileList(eProfileType.Store);
						StoreEligibilityList storeEligList = GetStoreEligibilityList(storeList, nodeRID, false, true);
						StoreEligibilityUpdate(hnp.Key, storeEligList, false);
						StoreGradeList storeGradeList = this.GetStoreGradeList(nodeRID, true);
						if (storeGradeList.Count > 0)
						{
							// only copy if either grades or min/maxes are not inherited
							StoreGradeProfile sgp = (StoreGradeProfile)storeGradeList[0];
							if (sgp.StoreGradesInheritedFromNodeRID != Include.NoRID ||
								sgp.MinMaxesInheritedFromNodeRID != Include.NoRID)
							{
								StoreGradesUpdate(hnp.Key, storeGradeList);
							}
						}
						StoreCapacityList storeCapacityList = this.GetStoreCapacityList(storeList, nodeRID, false, true);
						StoreCapacityUpdate(hnp.Key, storeCapacityList, false);
						VelocityGradeList velocityGradeList = this.GetVelocityGradeList(nodeRID, true);
						VelocityGradesUpdate(hnp.Key, velocityGradeList);
						SellThruPctList sellThruPctList = this.GetSellThruPctList(nodeRID, true);
						SellThruPctsUpdate(hnp.Key, sellThruPctList);
						StoreDailyPercentagesList storeDailyPercentagesList = HierarchyServerGlobal.GetStoreDailyPercentagesList(storeList, nodeRID, true);
						StoreDailyPercentagesUpdate(hnp.Key, storeDailyPercentagesList, false);
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
						SizeCurveCriteriaList sizeCurveCriteriaList = HierarchyServerGlobal.GetSizeCurveCriteriaList(nodeRID, true, true);
						SizeCurveCriteriaUpdate(hnp.Key, sizeCurveCriteriaList);
						SizeCurveDefaultCriteriaProfile sizeCurveDefaultCriteriaProfile = HierarchyServerGlobal.GetSizeCurveDefaultCriteriaProfile(nodeRID, sizeCurveCriteriaList, true);
						SizeCurveDefaultCriteriaUpdate(hnp.Key, sizeCurveDefaultCriteriaProfile);
						SizeCurveToleranceProfile sizeCurveToleranceProfile = HierarchyServerGlobal.GetSizeCurveToleranceProfile(nodeRID, true);
						SizeCurveToleranceUpdate(hnp.Key, sizeCurveToleranceProfile);
						SizeCurveSimilarStoreList sizeCurveSimilarStoreList = HierarchyServerGlobal.GetSizeCurveSimilarStoreList(storeList, nodeRID, false, true);
						SizeCurveSimilarStoreUpdate(hnp.Key, sizeCurveSimilarStoreList);
						//End TT#155 - JScott - Add Size Curve info to Node Properties
                        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
                        // Begin TT#3281 - JSmith - Copying node in Merchandise Explorer fails with object reference error
                        if (hnp.Begin_CDR_RID != Include.NoRID)
                        {
                        // End TT#3281 - JSmith - Copying node in Merchandise Explorer fails with object reference error
                            ProfileList _CSPweeks = GetWeeks(hnp.Begin_CDR_RID);
                            ChainSetPercentList chainSetPercentList = this.GetChainSetPercentList(storeList, nodeRID, false, true, true, _CSPweeks);
                            ChainSetPercentUpdate(hnp.Key, chainSetPercentList, hnp.Begin_CDR_RID, false);
                        // Begin TT#3281 - JSmith - Copying node in Merchandise Explorer fails with object reference error
                        }
                        // End TT#3281 - JSmith - Copying node in Merchandise Explorer fails with object reference error
                        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
						//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
                        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        SizeOutOfStockProfile sizeOutOfStockProfile = HierarchyServerGlobal.GetSizeOutOfStockProfile(nodeRID, true, Include.Undefined);
                        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                        SizeOutOfStockUpdate(hnp.Key, sizeOutOfStockProfile);
						SizeSellThruProfile sizeSellThruProfile = HierarchyServerGlobal.GetSizeSellThruProfile(nodeRID, true);
						SizeSellThruUpdate(hnp.Key, sizeSellThruProfile);
						//End TT#483 - JScott - Add Size Lost Sales criteria and processing
					}
				}
				hnp = GetNodeData(hnp.Key);
				return hnp;
			}
			catch ( Exception ex )
			{
                // Begin TT#2 -  RMatelic - Assortment Planning >> correct service name
                //EventLog.WriteEntry("MIDHierarchySession", "CopyNode error:" + ex.Message, EventLogEntryType.Error);
                EventLog.WriteEntry("MIDHierarchyService", "CopyNode error:" + ex.Message, EventLogEntryType.Error);
                // End TT#2
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
			
		}

		/// <summary>
		/// Requests the session get the root nodes from the hierarchy global area.
		/// </summary>
		/// <returns>An instance of the HierarchyNodeList class containing a HierarchyNodeProfile object for each root node</returns>
		public HierarchyNodeList GetRootNodes()
		{
			try
			{
				return HierarchyServerGlobal.GetRootNodes(UserID, IsHierarchyAdministrator, _availableNodeList);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the root nodes from the hierarchy global area.
		/// </summary>
		/// <returns>An instance of the HierarchyNodeList class containing a HierarchyNodeProfile object for each root node</returns>
        // Begin Track #5005 - JSmith - Explorer Organization
        //public HierarchyNodeList GetRootNodes(eHierarchyNodeType aHierarchyNodeType)
        public HierarchyNodeList GetRootNodes(eHierarchySelectType aHierarchyNodeType)
        // End Track #5005
		{
			try
			{
				return HierarchyServerGlobal.GetRootNodes(UserID, IsHierarchyAdministrator, _availableNodeList, aHierarchyNodeType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the information for all the children of a parent in a hierarchy 
		/// from the hierarchy global area.
		/// </summary>
		/// <param name="parentNodeLevel">The relative level of the parent node in the hierarchy</param>
		/// <param name="currentHierarchyRID">The record id of the hierarchy where the children are currently found</param>
		/// <param name="homeHierarchyRID">The record id of the home hierarchy of the parent node</param>
		/// <param name="nodeRID">The record id of the parent</param>
		/// <returns>An instance of the HierarchyNodeList class containing a HierarchyNodeProfile object for each child node</returns>
		public HierarchyNodeList GetHierarchyChildren(int parentNodeLevel, int currentHierarchyRID, 
			int homeHierarchyRID, int nodeRID, bool aChaseHierarchy, eNodeSelectType aNodeSelectType)
		{
			try
			{
				return GetHierarchyChildren(parentNodeLevel, currentHierarchyRID,
                    homeHierarchyRID, nodeRID, aChaseHierarchy, aNodeSelectType, false);
			}

			catch
			{
				throw;
			}
		}

        /// <summary>
        /// Requests the session get the information for all the children of a parent in a hierarchy 
        /// from the hierarchy global area.
        /// </summary>
        /// <param name="parentNodeLevel">The relative level of the parent node in the hierarchy</param>
        /// <param name="currentHierarchyRID">The record id of the hierarchy where the children are currently found</param>
        /// <param name="homeHierarchyRID">The record id of the home hierarchy of the parent node</param>
        /// <param name="nodeRID">The record id of the parent</param>
        /// <returns>An instance of the HierarchyNodeList class containing a HierarchyNodeProfile object for each child node</returns>
        public HierarchyNodeList GetHierarchyChildren(int parentNodeLevel, int currentHierarchyRID,
            int homeHierarchyRID, int nodeRID, bool aChaseHierarchy, eNodeSelectType aNodeSelectType, bool aAccessDenied)
        {
            try
            {
                return HierarchyServerGlobal.GetHierarchyChildren(parentNodeLevel, currentHierarchyRID,
                    homeHierarchyRID, nodeRID, IsHierarchyAdministrator, _availableNodeList, _nodeSecurityAssignments,
                    aChaseHierarchy, aNodeSelectType, aAccessDenied);
            }

            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Requests the session add or update relationship information for parents and children in a hierarchy 
		/// in the hierarchy global area.
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile class which contains node relationship information</param>
		public void JoinUpdate(HierarchyJoinProfile hjp)
		{
			bool previousConnection = false;
			try
			{
				if (_mhd.ConnectionIsOpen)
				{
					previousConnection = true;
				}
				switch (hjp.JoinChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						if (!previousConnection)
						{
							_mhd.OpenUpdateConnection();
						}
						_mhd.Hierarchy_Join_Add(hjp.NewHierarchyRID, hjp.NewParentRID, hjp.Key);
						if (!previousConnection)
						{
							_mhd.CommitData();
							_mhd.CloseUpdateConnection();
						}
						break;
					}
					case eChangeType.update: 
					{
						if (!previousConnection)
						{
							_mhd.OpenUpdateConnection();
						}
						_mhd.Hierarchy_Join_Update(hjp.Key, hjp.OldHierarchyRID,
							hjp.OldParentRID, hjp.NewHierarchyRID,  hjp.NewParentRID);
						if (!previousConnection)
						{
							_mhd.CommitData();
							_mhd.CloseUpdateConnection();
						}	
						break;
					}
					case eChangeType.delete: 
					{
						if (!previousConnection)
						{
							_mhd.OpenUpdateConnection();
						}
						_mhd.Hierarchy_Join_Delete(hjp.OldHierarchyRID, hjp.OldParentRID, hjp.Key);
						if (!previousConnection)
						{
							_mhd.CommitData();
							_mhd.CloseUpdateConnection();
						}	
						break;
					}
				}
				HierarchyServerGlobal.JoinUpdate(hjp);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen && !previousConnection)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session check to determine if a node is already a child for a specified parent 
		/// within a hierarchy in the hierarchy global area.
		/// </summary>
		/// <param name="hjp">An instance of the HierarchyJoinProfile which contains node relationship information</param>
		/// <returns>A flag identifying if the node is already a child of the parent</returns>
		public bool JoinExists(HierarchyJoinProfile hjp)
		{
			try
			{
				return HierarchyServerGlobal.JoinExists(hjp);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update eligibility model information 
		/// </summary>
		/// <param name="emp">An instance of the EligModelInterfaceData class which contains eligibility model information</param>
		public int EligModelUpdate(EligModelProfile emp)
		{
			int salesEntrySequence = 0;
			int stockEntrySequence = 0;
			int priorityShippingSequence = 0;
			try
			{
				switch (emp.ModelChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_mhd.OpenUpdateConnection();
						emp.Key = _mhd.EligModel_Add(emp.ModelID);
						foreach(EligModelEntry eme in emp.ModelEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.StockEligibility:
									_mhd.EligModelStockEntry_Add(emp.Key, stockEntrySequence, eme.DateRange.Key);
									++stockEntrySequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						foreach(EligModelEntry eme in emp.SalesEligibilityEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.SalesEligibility:
									_mhd.EligModelSalesEntry_Add(emp.Key, salesEntrySequence, eme.DateRange.Key);
									++salesEntrySequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						foreach(EligModelEntry eme in emp.PriorityShippingEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.PriorityShipping:
									_mhd.EligModelPriShipEntry_Add(emp.Key, priorityShippingSequence, eme.DateRange.Key);
									++priorityShippingSequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.EligModelSalesEntry_Delete(emp.Key);
						_mhd.EligModelStockEntry_Delete(emp.Key);
						_mhd.EligModelPriShipEntry_Delete(emp.Key);
						foreach(EligModelEntry eme in emp.ModelEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.StockEligibility:
									_mhd.EligModelStockEntry_Add(emp.Key, stockEntrySequence, eme.DateRange.Key);
									++stockEntrySequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						foreach(EligModelEntry eme in emp.SalesEligibilityEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.SalesEligibility:
									_mhd.EligModelSalesEntry_Add(emp.Key, salesEntrySequence, eme.DateRange.Key);
									++salesEntrySequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						foreach(EligModelEntry eme in emp.PriorityShippingEntries)
						{
							switch (eme.EligModelEntryType)
							{
								case eEligModelEntryType.PriorityShipping:
									_mhd.EligModelPriShipEntry_Add(emp.Key, priorityShippingSequence, eme.DateRange.Key);
									++priorityShippingSequence;
									break;
								default:
									//TODO throw exception
									break;
							}
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
					case eChangeType.delete: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.EligModelSalesEntry_Delete(emp.Key);
						_mhd.EligModelStockEntry_Delete(emp.Key);
						_mhd.EligModelPriShipEntry_Delete(emp.Key);
						_mhd.EligModel_Delete(emp.Key);
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
				}
				HierarchyServerGlobal.EligModelUpdate(emp);
				return emp.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the eligibility models.
		/// </summary>
		/// <returns>A ProfileList of eligibility models containing "Model RID" and "Model Name"</returns>
		public ProfileList GetEligModels()
		{
			try
			{
				return HierarchyServerGlobal.PopulateEligModels();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get model information for update.
		/// </summary>
		/// <param name="aModelType">The type of the model</param>
		/// <param name="aModelRID">The record id of the model</param>
		/// <param name="aAllowReadOnly">This flag identifies if the lock type can be changed to read only if an update
		/// lock can not be obtained</param>
		/// <returns>An instance of the HierarchyProfile object containing information about the hierarchy</returns>
		public ModelProfile GetModelDataForUpdate(eModelType aModelType, int aModelRID, bool aAllowReadOnly)
		{
			string errMsg;
			bool enqueueWriteLocked = false;
			System.Windows.Forms.DialogResult diagResult;
			try
			{
				HierarchyServerGlobal.AcquireEnqueueWriterLock();
				enqueueWriteLocked = true;
				ModelProfile mp = null;

				switch (aModelType)
				{
					case eModelType.Eligibility:
						mp = HierarchyServerGlobal.GetEligModelData(aModelRID);
						break;
					case eModelType.SalesModifier:
						mp = HierarchyServerGlobal.GetSlsModModelData(aModelRID);
						break;
					case eModelType.StockModifier:
						mp = HierarchyServerGlobal.GetStkModModelData(aModelRID);
						break;
					case eModelType.FWOSModifier:
						mp = HierarchyServerGlobal.GetFWOSModModelData(aModelRID);
						break;
					case eModelType.SizeAlternates:
						mp = HierarchyServerGlobal.GetSizeAltModelData(aModelRID);
						break;
					case eModelType.SizeConstraints:
						mp = HierarchyServerGlobal.GetSizeConstraintModelData(aModelRID);
						break;
					case eModelType.SizeCurve:
						mp = HierarchyServerGlobal.GetSizeCurveGroupData(aModelRID);
						break;
					case eModelType.SizeGroup:
						mp = HierarchyServerGlobal.GetSizeGroupData(aModelRID);
						break;
                    case eModelType.FWOSMax:
                        mp = HierarchyServerGlobal.GetFWOSMaxModelData(aModelRID);
                        break;
				}


				ModelEnqueue modelEnqueue = new ModelEnqueue(
					aModelType,
					aModelRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					modelEnqueue.EnqueueModel();
					mp.ModelLockStatus = eLockStatus.Locked;
				}
				catch (ModelConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
					enqueueWriteLocked = false;
					errMsg = "The following model(s) requested:" + System.Environment.NewLine;
					foreach (ModelConflict MCon in modelEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Model: " + mp.ModelID + ", User: " + MCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					if (aAllowReadOnly)
					{
						mp.ModelLockStatus = eLockStatus.ReadOnly;
						errMsg += "Do you wish to continue with the model as read-only?";

						diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Model Conflict",
							System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);
						if (diagResult == System.Windows.Forms.DialogResult.Cancel)
						{
							mp.ModelLockStatus = eLockStatus.Cancel;
						}
					}
					else
					{
						//errMsg += "Can not be updated at this time";
                        errMsg += MIDText.GetText(eMIDTextCode.msg_NodeNotAffected);
                        diagResult = SessionAddressBlock.MessageCallback.HandleMessage(
							errMsg,
							"Model Conflict",
							System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
						mp.ModelLockStatus = eLockStatus.Cancel;
					}
				}
				return mp;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (enqueueWriteLocked)
				{
					HierarchyServerGlobal.ReleaseEnqueueWriterLock();
				}
			}
		}

		/// <summary>
		/// Requests the session dequeue the model.
		/// </summary>
		/// <param name="aModelType">The type of the model</param>
		/// <param name="aModelRID">The record id of the model</param>
		public void DequeueModel(eModelType aModelType, int aModelRID)
		{
			try
			{
				ModelEnqueue modelEnqueue = new ModelEnqueue(
					aModelType,
					aModelRID,
					SessionAddressBlock.ClientServerSession.UserRID,
					SessionAddressBlock.ClientServerSession.ThreadID);

				try
				{
					modelEnqueue.DequeueModel();

				}
				catch 
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
		}

		private DataTable DefineModelDataTable()
		{
			try
			{
				DataTable modelTable = MIDEnvironment.CreateDataTable();
				DataColumn emDataColumn;

				//Create Columns and rows for datatable
				emDataColumn = new DataColumn();
				emDataColumn.DataType = System.Type.GetType("System.String");
				emDataColumn.ColumnName = "Model RID";
				emDataColumn.Caption = "Model RID";
				emDataColumn.ReadOnly = true;
				emDataColumn.Unique = true;
				modelTable.Columns.Add(emDataColumn);

				emDataColumn = new DataColumn();
				emDataColumn.DataType = System.Type.GetType("System.String");
				emDataColumn.ColumnName = "Model Name";
				emDataColumn.Caption = "Model Name";
				emDataColumn.ReadOnly = true;
				emDataColumn.Unique = true;
				modelTable.Columns.Add(emDataColumn);

				DataColumn[] PrimaryKeyColumn = new DataColumn[1];
				PrimaryKeyColumn[0] = modelTable.Columns["Model Name"];
				modelTable.PrimaryKey = PrimaryKeyColumn;
				return modelTable;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the eligibility model data.
		/// </summary>
		/// <param name="ModelID">The name the eligibility model</param>
		/// <returns>A instance of the EligModelProfile class containing information about an eligibility model</returns>
		public EligModelProfile GetEligModelData(string ModelID)
		{
			try
			{
				return HierarchyServerGlobal.GetEligModelData(ModelID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the eligibility model data.
		/// </summary>
		/// <param name="ModelRID">The record id of the eligibility model</param>
		/// <returns>A instance of the EligModelProfile class containing information about an eligibility model</returns>
		public EligModelProfile GetEligModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetEligModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the session get the FWOS modifier model data.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>A instance of the FWOSModModelProfile class containing information about a FWOS modifier model</returns>
        public EligModelProfile GetEligModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                return HierarchyServerGlobal.GetEligModelData(ModelRID, aStoreList);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Requests the session add or update stock modifier model information 
		/// </summary>
		/// <param name="smmp">An instance of the StkModModelProfile class which contains stock modifier model information</param>
		public int StkModModelUpdate(StkModModelProfile smmp)
		{
			int stkModEntrySequence = 0;
			try
			{
				switch (smmp.ModelChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_mhd.OpenUpdateConnection();
						smmp.Key = _mhd.StkModModel_Add(smmp.ModelID, smmp.StkModModelDefault);
						foreach(StkModModelEntry smme in smmp.ModelEntries)
						{
							_mhd.StkModModelEntry_Add(smmp.Key, stkModEntrySequence, smme.StkModModelEntryValue, smme.DateRange.Key);
							++stkModEntrySequence;
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.StkModModel_Update(smmp.Key, smmp.ModelID, smmp.StkModModelDefault);
						_mhd.StkModModelEntry_Delete(smmp.Key);
						foreach(StkModModelEntry smme in smmp.ModelEntries)
						{
							_mhd.StkModModelEntry_Add(smmp.Key, stkModEntrySequence, smme.StkModModelEntryValue, smme.DateRange.Key);
							++stkModEntrySequence;
									
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
					case eChangeType.delete: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.StkModModelEntry_Delete(smmp.Key);
						_mhd.StkModModel_Delete(smmp.Key);
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
				}
				HierarchyServerGlobal.StkModModelUpdate(smmp);
				return smmp.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the stock modifier models.
		/// </summary>
		/// <returns>A ProfileList of stock modifier models containing "Model RID" and "Model Name"</returns>
		public ProfileList GetStkModModels()
		{
			try
			{
				return HierarchyServerGlobal.PopulateStkModModels();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the stock modifier model data.
		/// </summary>
		/// <param name="ModelID">The name the stock modifier model</param>
		/// <returns>A instance of the StkModModelProfile class containing information about a stock modifier model</returns>
		public StkModModelProfile GetStkModModelData(string ModelID)
		{
			try
			{
				return HierarchyServerGlobal.GetStkModModelData(ModelID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the stock modifier model data.
		/// </summary>
		/// <param name="ModelRID">The id of the stock modifier model</param>
		/// <returns>A instance of the StkModModelProfile class containing information about a stock modifier model</returns>
		public StkModModelProfile GetStkModModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetStkModModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the session get the stock modifier model data.
        /// </summary>
        /// <param name="ModelRID">The id of the stock modifier model</param>
        /// <returns>A instance of the StkModModelProfile class containing information about a stock modifier model</returns>
        public StkModModelProfile GetStkModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                return HierarchyServerGlobal.GetStkModModelData(ModelRID, aStoreList);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Requests the session add or update sales modifier model information 
		/// </summary>
		/// <param name="smmp">An instance of the SlsModModelProfile class which contains sales modifier model information</param>
		public int SlsModModelUpdate(SlsModModelProfile smmp)
		{
			int slsModEntrySequence = 0;
			try
			{
				switch (smmp.ModelChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_mhd.OpenUpdateConnection();
						smmp.Key = _mhd.SlsModModel_Add(smmp.ModelID, smmp.SlsModModelDefault);
						foreach(SlsModModelEntry smme in smmp.ModelEntries)
						{
							_mhd.SlsModModelEntry_Add(smmp.Key, slsModEntrySequence, smme.SlsModModelEntryValue, smme.DateRange.Key);
							++slsModEntrySequence;
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.SlsModModel_Update(smmp.Key, smmp.ModelID, smmp.SlsModModelDefault);
						_mhd.SlsModModelEntry_Delete(smmp.Key);
						foreach(SlsModModelEntry smme in smmp.ModelEntries)
						{
							_mhd.SlsModModelEntry_Add(smmp.Key, slsModEntrySequence, smme.SlsModModelEntryValue, smme.DateRange.Key);
							++slsModEntrySequence;
									
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
					case eChangeType.delete: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.SlsModModelEntry_Delete(smmp.Key);
						_mhd.SlsModModel_Delete(smmp.Key);
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
				}
				HierarchyServerGlobal.SlsModModelUpdate(smmp);
				return smmp.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the sales modifier models.
		/// </summary>
		/// <returns>A ProfileList of sales modifier models containing "Model RID" and "Model Name"</returns>
		public ProfileList GetSlsModModels()
		{
			try
			{
				return HierarchyServerGlobal.PopulateSlsModModels();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the sales modifier model data.
		/// </summary>
		/// <param name="ModelID">The name of the sales modifier model</param>
		/// <returns>A instance of the SlsModModelProfile class containing information about a sales modifier model</returns>
		public SlsModModelProfile GetSlsModModelData(string ModelID)
		{
			try
			{
				return HierarchyServerGlobal.GetSlsModModelData(ModelID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the sales modifier model data.
		/// </summary>
		/// <param name="ModelRID">The record id of the sales modifier model</param>
		/// <returns>A instance of the SlsModModelProfile class containing information about a sales modifier model</returns>
		public SlsModModelProfile GetSlsModModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSlsModModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the session get the sales modifier model data.
        /// </summary>
        /// <param name="ModelRID">The record id of the sales modifier model</param>
        /// <returns>A instance of the SlsModModelProfile class containing information about a sales modifier model</returns>
        public SlsModModelProfile GetSlsModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                return HierarchyServerGlobal.GetSlsModModelData(ModelRID, aStoreList);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

		/// <summary>
		/// Requests the session add or update FWOS modifier model information 
		/// </summary>
		/// <param name="mmp">An instance of the FWOSModModelProfile class which contains FWOS modifier model information</param>
		public int FWOSModModelUpdate(FWOSModModelProfile mmp)
		{
			int FWOSModEntrySequence = 0;
			try
			{
				switch (mmp.ModelChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_mhd.OpenUpdateConnection();
						mmp.Key = _mhd.FWOSModModel_Add(mmp.ModelID, mmp.FWOSModModelDefault);
						foreach(FWOSModModelEntry smme in mmp.ModelEntries)
						{
							_mhd.FWOSModModelEntry_Add(mmp.Key, FWOSModEntrySequence, smme.FWOSModModelEntryValue, smme.DateRange.Key);
							++FWOSModEntrySequence;
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.FWOSModModel_Update(mmp.Key, mmp.ModelID, mmp.FWOSModModelDefault);
						_mhd.FWOSModModelEntry_Delete(mmp.Key);
						foreach(FWOSModModelEntry smme in mmp.ModelEntries)
						{
							_mhd.FWOSModModelEntry_Add(mmp.Key, FWOSModEntrySequence, smme.FWOSModModelEntryValue, smme.DateRange.Key);
							++FWOSModEntrySequence;
									
						}
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
					case eChangeType.delete: 
					{
						_mhd.OpenUpdateConnection();
						_mhd.FWOSModModelEntry_Delete(mmp.Key);
						_mhd.FWOSModModel_Delete(mmp.Key);
						_mhd.CommitData();
						_mhd.CloseUpdateConnection();
							
						break;
					}
				}
				HierarchyServerGlobal.FWOSModModelUpdate(mmp);
				return mmp.Key;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the FWOS modifier models.
		/// </summary>
		/// <returns>A ProfileList of FWOS modifier models containing "Model RID" and "Model Name"</returns>
		public ProfileList GetFWOSModModels()
		{
			try
			{
				return HierarchyServerGlobal.PopulateFWOSModModels();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the FWOS modifier model data.
		/// </summary>
		/// <param name="ModelID">The name of the FWOS modifier model</param>
		/// <returns>A instance of the SlsModModelProfile class containing information about a FWOS modifier model</returns>
		public FWOSModModelProfile GetFWOSModModelData(string ModelID)
		{
			try
			{
				return HierarchyServerGlobal.GetFWOSModModelData(ModelID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the FWOS modifier model data.
		/// </summary>
		/// <param name="ModelRID">The record id of the FWOS modifier model</param>
		/// <returns>A instance of the FWOSModModelProfile class containing information about a FWOS modifier model</returns>
		public FWOSModModelProfile GetFWOSModModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetFWOSModModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the session get the FWOS modifier model data.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>A instance of the FWOSModModelProfile class containing information about a FWOS modifier model</returns>
        public FWOSModModelProfile GetFWOSModModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                return HierarchyServerGlobal.GetFWOSModModelData(ModelRID, aStoreList);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

        //BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancement
        /// <summary>
        /// Requests the session add or update FWOS max model information 
        /// </summary>
        /// <param name="mmp">An instance of the FWOSMaxModelProfile class which contains FWOS modifier model information</param>
        public int FWOSMaxModelUpdate(FWOSMaxModelProfile mmp)
        {
            int FWOSMaxEntrySequence = 0;
            try
            {
                switch (mmp.ModelChangeType)
                {
                    case eChangeType.none:
                        {
                            break;
                        }
                    case eChangeType.add:
                        {
                            _mhd.OpenUpdateConnection();
                            mmp.Key = _mhd.FWOSMaxModel_Add(mmp.ModelID, mmp.FWOSMaxModelDefault);
                            foreach (FWOSMaxModelEntry smme in mmp.ModelEntries)
                            {
                                _mhd.FWOSMaxModelEntry_Add(mmp.Key, FWOSMaxEntrySequence, smme.FWOSMaxModelEntryValue, smme.DateRange.Key);
                                ++FWOSMaxEntrySequence;
                            }
                            _mhd.CommitData();
                            _mhd.CloseUpdateConnection();
                            break;
                        }
                    case eChangeType.update:
                        {
                            _mhd.OpenUpdateConnection();
                            _mhd.FWOSMaxModel_Update(mmp.Key, mmp.ModelID, mmp.FWOSMaxModelDefault);
                            _mhd.FWOSMaxModelEntry_Delete(mmp.Key);
                            foreach (FWOSMaxModelEntry smme in mmp.ModelEntries)
                            {
                                _mhd.FWOSMaxModelEntry_Add(mmp.Key, FWOSMaxEntrySequence, smme.FWOSMaxModelEntryValue, smme.DateRange.Key);
                                ++FWOSMaxEntrySequence;

                            }
                            _mhd.CommitData();
                            _mhd.CloseUpdateConnection();

                            break;
                        }
                    case eChangeType.delete:
                        {
                            _mhd.OpenUpdateConnection();
                            _mhd.FWOSMaxModelEntry_Delete(mmp.Key);
                            _mhd.FWOSMaxModel_Delete(mmp.Key);
                            _mhd.CommitData();
                            _mhd.CloseUpdateConnection();

                            break;
                        }
                }
                HierarchyServerGlobal.FWOSMaxModelUpdate(mmp);
                return mmp.Key;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
            }
        }

        /// <summary>
        /// Requests the session get the FWOS modifier models.
        /// </summary>
        /// <returns>A ProfileList of FWOS modifier models containing "Model RID" and "Model Name"</returns>
        public ProfileList GetFWOSMaxModels()
        {
            try
            {
                return HierarchyServerGlobal.PopulateFWOSMaxModels();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the FWOS modifier model data.
        /// </summary>
        /// <param name="ModelID">The name of the FWOS modifier model</param>
        /// <returns>A instance of the SlsModModelProfile class containing information about a FWOS modifier model</returns>
        public FWOSMaxModelProfile GetFWOSMaxModelData(string ModelID)
        {
            try
            {
                return HierarchyServerGlobal.GetFWOSMaxModelData(ModelID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the FWOS modifier model data.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>A instance of the FWOSModModelProfile class containing information about a FWOS modifier model</returns>
        public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID)
        {
            try
            {
				//BEGIN TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.
                ProfileList storeList = GetProfileList(eProfileType.Store);
                //return HierarchyServerGlobal.GetFWOSMaxModelData(ModelRID);
                return HierarchyServerGlobal.GetFWOSMaxModelData(ModelRID, storeList);
				//END TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        /// <summary>
        /// Requests the session get the FWOS modifier model data.
        /// </summary>
        /// <param name="ModelRID">The record id of the FWOS modifier model</param>
        /// <returns>A instance of the FWOSModModelProfile class containing information about a FWOS modifier model</returns>
        public FWOSMaxModelProfile GetFWOSMaxModelData(int ModelRID, ProfileList aStoreList)
        {
            try
            {
                return HierarchyServerGlobal.GetFWOSMaxModelData(ModelRID, aStoreList);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307
        //END TT#108 - MD - DOConnell - FWOS Max Model Enhancement

        //BEGIN TT#1401 - gtaylor - Reservation Stores
        /// <summary>
        /// 
        /// 
        /// 
        /// </summary>
        /// 
        ///
        public void IMOUpdate(int nodeRID, IMOProfileList imopl, bool cacheCleared)
        {
            try
            {
                _mhd.OpenUpdateConnection();
                foreach (IMOProfile imop in imopl)
                {

                    switch (imop.IMOChangeType)
                    {
                        case eChangeType.none:
                            {
                                break;
                            }
                        case eChangeType.add:
                            {
                                if ((imop.IMONodeRID != Include.NoRID) && (!imop.IsDefaultValues))
                                //if ((imop.IMONodeRID != Include.NoRID) && (!imop.IsIMOBaseDefault))
                                {
								//BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                                    //_mhd.Node_IMO_Insert(imop.IMONodeRID, imop.IMOStoreRID, imop.IMOMinShipQty, imop.IMOPackQty, imop.IMOMaxValue, 
                                    //    imop.IMOFWOS_Max, imop.IMOPshToBackStock);  // TT#2225 - gtaylor - ANF VSW

                                    _mhd.Node_IMO_Insert(imop.IMONodeRID, imop.IMOStoreRID, imop.IMOMinShipQty, imop.IMOPackQty, imop.IMOMaxValue,
                                        imop.IMOFWOS_Max, imop.IMOFWOS_MaxModelRID, imop.IMOFWOS_MaxType, imop.IMOPshToBackStock);  // TT#2225 - gtaylor - ANF VSW
								//END
                                }
                                break;
                            }
                        case eChangeType.update:
                            {
                                if ((imop.IMONodeRID != Include.NoRID) && (imop.IMOStoreRID != Include.NoRID))
                                {
                                    _mhd.Node_IMO_Delete(imop.IMONodeRID, imop.IMOStoreRID);
                                    if (!imop.IsDefaultValues)
                                    //if (!imop.IsIMOBaseDefault)
                                    {
									//BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                                        //_mhd.Node_IMO_Insert(imop.IMONodeRID, imop.IMOStoreRID, imop.IMOMinShipQty, imop.IMOPackQty, imop.IMOMaxValue,
                                        //    imop.IMOFWOS_Max, imop.IMOPshToBackStock); // TT#2225 - gtaylor - ANF VSW
                                        _mhd.Node_IMO_Insert(imop.IMONodeRID, imop.IMOStoreRID, imop.IMOMinShipQty, imop.IMOPackQty, imop.IMOMaxValue,
                                                imop.IMOFWOS_Max, imop.IMOFWOS_MaxModelRID, imop.IMOFWOS_MaxType, imop.IMOPshToBackStock); // TT#2225 - gtaylor - ANF VSW
                                        //_mhd.Node_IMO_Update(imop.IMONodeRID, imop.IMOStoreRID, imop.IMOMinShipQty, imop.IMOPackQty, imop.IMOMaxValue, imop.IMOPshToBackStock);
                                    //END TT#108 - MD - DOConnell - FWOS Max Model
									}
                                }
                                break;
                            }
                        case eChangeType.delete:
                            {
                                if ((imop.IMONodeRID != Include.NoRID) && (imop.IMOStoreRID != Include.NoRID))
                                {
                                    _mhd.Node_IMO_Delete(imop.IMONodeRID, imop.IMOStoreRID);
                                }
                                break;
                            }
                    }
                }
                _mhd.CommitData();
                _mhd.CloseUpdateConnection();

                if (!cacheCleared)  HierarchyServerGlobal.UpdateNodeIMO(nodeRID, imopl); //TT#2015 - gtaylor - apply changes to lower levels
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
            }
        }
        // END TT#1401 - gtaylor - Reservation Stores

		/// <summary>
		/// Requests the session add or update store eligibility information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sel">A instance of StoreEligibilityList class which contains StoreEligibilityProfile objects</param>
		public void StoreEligibilityUpdate(int nodeRID, StoreEligibilityList sel, bool cacheCleared)
		{
            MerchandiseHierarchyData dataLock = null;
			try
			{
                // determine if store eligibility already added by other process
                dataLock = new MerchandiseHierarchyData();
                dataLock.OpenUpdateConnection(lockType: eLockType.StoreEligibility, lockRID: nodeRID);

                ProfileList storeList = GetProfileList(aProfileType: eProfileType.Store);
                StoreEligibilityList storeEligList = GetStoreEligibilityList(storeList:storeList, nodeRID:nodeRID, chaseHierarchy:false, forCopy: false);
                // if already exists, override add to update
                foreach (StoreEligibilityProfile sep in sel)
                {
                    if (sep.StoreEligChangeType == eChangeType.add
                        && storeEligList.Contains(sep.Key))
                    {
                        StoreEligibilityProfile currentSep = (StoreEligibilityProfile)storeEligList.FindKey(aKey: sep.Key);
                        if (currentSep.RecordExists)
                        {
                            sep.StoreEligChangeType = eChangeType.update;
                        }
                    }
                }

				_mhd.OpenUpdateConnection();
				foreach (StoreEligibilityProfile sep in sel)
				{

					switch (sep.StoreEligChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							_mhd.StoreEligibility_Add(nodeRID, sep.Key, sep.EligIsInherited,
								sep.EligModelRID, sep.StoreIneligible, 
								sep.StkModType, sep.StkModModelRID, sep.StkModPct,
								sep.SlsModType, sep.SlsModModelRID, sep.SlsModPct,
								sep.FWOSModType, sep.FWOSModModelRID, sep.FWOSModPct,
								sep.SimStoreType, sep.SimStoreRatio, sep.SimStoreUntilDateRangeRID,
								sep.PresPlusSalesIsSet, sep.PresPlusSalesInd
								//BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
								, sep.StkLeadWeeks);
								//END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
							switch (sep.SimStoreType)
							{
								case eSimilarStoreType.Stores:
                                    _mhd.SimilarStore_Add(nodeRID, sep.Key, sep.SimStores);
									break;
								default:
									break;
							}
							break;
						}
						case eChangeType.update: 
						{
							_mhd.StoreEligibility_Update(nodeRID, sep.Key, sep.EligIsInherited, 
								sep.EligModelRID, sep.StoreIneligible, 
								sep.StkModType, sep.StkModModelRID, sep.StkModPct,
								sep.SlsModType, sep.SlsModModelRID, sep.SlsModPct,
								sep.FWOSModType, sep.FWOSModModelRID, sep.FWOSModPct,
								sep.SimStoreType, sep.SimStoreRatio, sep.SimStoreUntilDateRangeRID,
								sep.PresPlusSalesIsSet, sep.PresPlusSalesInd
								//BEGIN TT#44 - MD - DOConnell - New Store Forecasting Enhancement
								, sep.StkLeadWeeks);
								//END TT#44 - MD - DOConnell - New Store Forecasting Enhancement
							if (sep.SimStoresChanged)
							{
								_mhd.SimilarStore_Delete(nodeRID, sep.Key);
								switch (sep.SimStoreType)
								{
									case eSimilarStoreType.Stores:
										_mhd.SimilarStore_Add(nodeRID, sep.Key, sep.SimStores);
										break;
									default:
										break;
								}
							}

							break;
						}
						case eChangeType.delete: 
						{
							_mhd.SimilarStore_Delete(nodeRID, sep.Key);
							_mhd.StoreEligibility_Delete(nodeRID, sep.Key);
							break;
						}
					}
				}
				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

                if (!cacheCleared) HierarchyServerGlobal.StoreEligibilityUpdate(nodeRID, sel); //TT#2015 - gtaylor - apply changes to lower levels
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}

                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
			}
		}

		/// <summary>
		/// Requests the session get the store eligibility list for the node.
		/// </summary>
		/// <param name="storeList">The list of store for which eligibility is to be looked up</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>A instance of the StoreEligibilityList class containing a StoreEligibilityProfile object for each store</returns>
		public StoreEligibilityList GetStoreEligibilityList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetStoreEligibilityList(storeList, nodeRID, chaseHierarchy, forCopy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the similar stores list for the node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which similar stores are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the SimilarStoreList class that contains a SimilarStoreProfile object for each store with a capacity</returns>
		public SimilarStoreList GetSimilarStoreList(ProfileList storeList, int nodeRID)
		{
			try
			{
				StoreEligibilityList sel = HierarchyServerGlobal.GetStoreEligibilityList(storeList, nodeRID, true, false);
				SimilarStoreList ssl = new SimilarStoreList(eProfileType.SimilarStore);
				SimilarStoreProfile ssp = null;
				foreach (StoreEligibilityProfile sep in sel)
				{
					if (sep.SimStores.Count > 0)
					{
						ssp = new SimilarStoreProfile(sep.Key);
						ssp.SimStoreType = sep.SimStoreType;
						ssp.SimStores = sep.SimStores;
						ssp.SimStoreRatio = sep.SimStoreRatio;
						ssp.SimStoreUntilDateRangeRID = sep.SimStoreUntilDateRangeRID;
						ssp.SimStoreWeekList = sep.SimStoreWeekList;
						ssl.Add(ssp);
					}
				}
				return ssl;
			}
			catch
			{
				throw;
			}
		}

		public SizeAltModelProfile GetSizeAltModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeAltModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}
		
		public SizeConstraintModelProfile GetSizeConstraintModelData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeConstraintModelData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveGroupProfile GetSizeCurveGrouplData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCurveGroupData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

		public SizeGroupProfile GetSizeGroupData(int ModelRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeGroupData(ModelRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update store grade information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sgl">A instance of StoreGradesList class which contains StoreGradeProfile objects</param>
		public void StoreGradesUpdate(int nodeRID, StoreGradeList sgl)
		{
            bool boundaryChanged = false;  // TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
			try
			{
				_mhd.OpenUpdateConnection();
				_mhd.StoreGrade_DeleteAll(nodeRID);
				foreach (StoreGradeProfile sgp in sgl)
				{

					switch (sgp.StoreGradeChangeType)
					{
						case eChangeType.none: 
						{
							// if no change, re-add the store grade information since it was deleted above
							if (sgp.StoreGradesIsInherited == false)
							{
								_mhd.StoreGrade_Add(nodeRID, sgp.StoreGrade, sgp.Boundary, sgp.WosIndex);
							}
							break;
						}
						case eChangeType.add: 
						{
							_mhd.StoreGrade_Add(nodeRID, sgp.StoreGrade, sgp.Boundary, sgp.WosIndex);
							if (sgp.MinMaxChangeType == eChangeType.add)
							{
								sgp.MinMaxChangeType = eChangeType.update;
							}
                            // Begin TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
                            if (sgp.Boundary != sgp.OriginalBoundary)
                            {
                                boundaryChanged = true;
                                _mhd.StoreGrade_UpdateBoundary(nodeRID, sgp.Boundary, sgp.OriginalBoundary);
                            }
                            // End TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
							
							break;
						}
						case eChangeType.update: 
						{
							_mhd.StoreGrade_Update(nodeRID, sgp.StoreGrade, sgp.Boundary, sgp.WosIndex);
							if (sgp.MinMaxChangeType == eChangeType.add)
							{
								sgp.MinMaxChangeType = eChangeType.update;
							}
                            // Begin TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
                            if (sgp.Boundary != sgp.OriginalBoundary)
                            {
                                boundaryChanged = true;
                                _mhd.StoreGrade_UpdateBoundary(nodeRID, sgp.Boundary, sgp.OriginalBoundary);
                            }
                            // End TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review

							break;
						}
						case eChangeType.delete: 
						{
							_mhd.StoreGrade_Delete(nodeRID, sgp.Boundary);

							break;
						}
					}

					switch (sgp.MinMaxChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							_mhd.MinMax_Add(nodeRID, sgp.Boundary,
                                sgp.MinStock, sgp.MaxStock, sgp.MinAd, sgp.MinColor, sgp.MaxColor, sgp.ShipUpTo);  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
							
							break;
						}
						case eChangeType.update: 
						{
							_mhd.MinMax_Update(nodeRID, sgp.Boundary,
                                sgp.MinStock, sgp.MaxStock, sgp.MinAd, sgp.MinColor, sgp.MaxColor, sgp.ShipUpTo); // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
							

							break;
						}
						case eChangeType.delete: 
						{
							_mhd.MinMax_Update(nodeRID, sgp.Boundary,
                                Include.Undefined, Include.Undefined, Include.Undefined, Include.Undefined,
                                Include.Undefined, Include.Undefined);  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)

							break;
						}
					}
				}
				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.StoreGradesUpdate(nodeRID, sgl, boundaryChanged); // TT#2404 - JSmith - Questions on Store Grades in OTS Forecast Review
			}

			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store grade list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the StoreGradeList class containing a StoreGradeProfile for each boundary</returns>
		public StoreGradeList GetStoreGradeList(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetStoreGradeList(nodeRID, forCopy, false);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update store grade information 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aMinMaxesProfile">A instance of NodeStockMinMaxesProfile class which contains stock min/max information</param>
		public void StockMinMaxUpdate(int aNodeRID, NodeStockMinMaxesProfile aMinMaxesProfile)
		{
			try
			{
				_mhd.OpenUpdateConnection();
				_mhd.StockMinMaxes_DeleteAll(aNodeRID);
				switch (aMinMaxesProfile.NodeStockMinMaxChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					case eChangeType.update: 
					{
						foreach (NodeStockMinMaxSetProfile minMaxSetProfile in aMinMaxesProfile.NodeSetList)
						{
							foreach (NodeStockMinMaxProfile minMaxProfile in minMaxSetProfile.Defaults.MinMaxList.ArrayList)
							{
								_mhd.StockMinMaxes_Add(aNodeRID, minMaxSetProfile.Key, Include.Undefined,
									minMaxProfile.Key, minMaxProfile.Minimum, minMaxProfile.Maximum);
							}

							foreach (NodeStockMinMaxBoundaryProfile minMaxBoundaryProfile in minMaxSetProfile.BoundaryList.ArrayList)
							{
								foreach (NodeStockMinMaxProfile minMaxProfile in minMaxBoundaryProfile.MinMaxList.ArrayList)
								{
									_mhd.StockMinMaxes_Add(aNodeRID, minMaxSetProfile.Key, minMaxBoundaryProfile.Key,
										minMaxProfile.Key, minMaxProfile.Minimum, minMaxProfile.Maximum);
								}
							}
						}
						_mhd.StockMinMaxes_GroupRIDUpdate(aNodeRID, aMinMaxesProfile.NodeStockStoreGroupRID);

						break;
					}
					case eChangeType.delete: 
					{
						_mhd.StockMinMaxes_GroupRIDUpdate(aNodeRID, Include.NoRID);
						break;
					}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.StockMinMaxUpdate(aNodeRID, aMinMaxesProfile);
			}

			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the stock min/maxes for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the NodeStockMinMaxesProfile class containing min/maxes by set</returns>
		public NodeStockMinMaxesProfile GetStockMinMaxes(int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetStockMinMaxes(nodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the store grade list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <returns>An instance of the StoreGradeList class containing a StoreGradeProfile for each boundary</returns>
		public StoreGradeList GetStoreGradeList(int nodeRID, bool forCopy, bool forAdmin)
		{
			try
			{
				return HierarchyServerGlobal.GetStoreGradeList(nodeRID, forCopy, forAdmin);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update store capacity information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="scl">A instance of the StoreCapacityList class which contains StoreCapacityProfile objects for each store</param>
		public void StoreCapacityUpdate(int nodeRID, StoreCapacityList scl, bool cacheCleared)
		{
			try
			{
				_mhd.OpenUpdateConnection();
				foreach (StoreCapacityProfile scp in scl)
				{

					switch (scp.StoreCapacityChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							_mhd.StoreCapacity_Add(nodeRID, scp.Key, scp.StoreCapacity);
							
							break;
						}
						case eChangeType.update: 
						{
							_mhd.StoreCapacity_Delete(nodeRID, scp.Key);
							_mhd.StoreCapacity_Add(nodeRID, scp.Key, scp.StoreCapacity);

							break;
						}
						case eChangeType.delete: 
						{
							_mhd.StoreCapacity_Delete(nodeRID, scp.Key);

							break;
						}
					}
				}
				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

                if (!cacheCleared) HierarchyServerGlobal.StoreCapacityUpdate(nodeRID, scl); // TT#201t - gtaylor - apply changes to lower levels
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store capacity list for the node.
		/// </summary>
		/// <param name="storeList">The ProfileList of store for which capacities are to be determined</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="stopOnFind">This switch tells the routine to stop when the first node with capacities is found</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the StoreCapacityList class that contains a StoreCapacityProfile object for each store with a capacity</returns>
		public StoreCapacityList GetStoreCapacityList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetStoreCapacityList(storeList, nodeRID, stopOnFind, forCopy);
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Requests the session add or update size curve criteria information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sccl">A instance of the SizeCurveCriteriaList class which contains SizeCurveCriteriaProfile objects</param>
		public void SizeCurveCriteriaUpdate(int nodeRID, SizeCurveCriteriaList sccl)
		{
			int key;

			try
			{
				_mhd.OpenUpdateConnection();

				foreach (SizeCurveCriteriaProfile sccp in sccl)
				{
					switch (sccp.CriteriaChangeType)
					{
						case eChangeType.add:
						{
							key = _mhd.SizeCurveCriteria_Add(nodeRID, sccp.CriteriaIsInherited, sccp.CriteriaLevelType, sccp.CriteriaLevelRID, sccp.CriteriaLevelSequence, sccp.CriteriaLevelOffset, sccp.CriteriaDateRID,
								//Begin TT#1076 - JScott - Size Curves by Set
								//sccp.CriteriaApplyLostSalesInd, sccp.CriteriaOLLRID, sccp.CriteriaCustomOLLRID, sccp.CriteriaSizeGroupRID, sccp.CriteriaCurveName);
								sccp.CriteriaApplyLostSalesInd, sccp.CriteriaOLLRID, sccp.CriteriaCustomOLLRID, sccp.CriteriaSizeGroupRID, sccp.CriteriaCurveName, sccp.CriteriaSgRID);
								//End TT#1076 - JScott - Size Curves by Set
							sccp.Key = key;

							break;
						}
						case eChangeType.update:
						{
							_mhd.SizeCurveCriteria_Update(nodeRID, sccp.Key, sccp.CriteriaIsInherited, sccp.CriteriaLevelType, sccp.CriteriaLevelRID, sccp.CriteriaLevelSequence, sccp.CriteriaLevelOffset, sccp.CriteriaDateRID,
								//Begin TT#1076 - JScott - Size Curves by Set
								//sccp.CriteriaApplyLostSalesInd, sccp.CriteriaOLLRID, sccp.CriteriaCustomOLLRID, sccp.CriteriaSizeGroupRID, sccp.CriteriaCurveName);
								sccp.CriteriaApplyLostSalesInd, sccp.CriteriaOLLRID, sccp.CriteriaCustomOLLRID, sccp.CriteriaSizeGroupRID, sccp.CriteriaCurveName, sccp.CriteriaSgRID);
								//End TT#1076 - JScott - Size Curves by Set

							break;
						}
						case eChangeType.delete:
						{
							_mhd.SizeCurveCriteria_Delete(nodeRID, sccp.Key);

							break;
						}
					}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SizeCurveCriteriaUpdate(nodeRID, sccl);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

        //  BEGIN TT#1572 - GRT - Urban Inheritence
        /// <summary>
        /// Requests the session get the store size curve criteria list for the node.
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <param name="forCurve">A flag identifying if the list is being retrieved from the Size Curve Tab Load</param>
        /// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
        public SizeCurveCriteriaList GetSizeCurveCriteriaList(int nodeRID, bool forCopy, bool useApplyFrom)
        {
            try
            {
                return HierarchyServerGlobal.GetSizeCurveCriteriaList(nodeRID, forCopy, useApplyFrom);
            }
            catch
            {
                throw;
            }
        }
        //  END TT#1572 - GRT - Urban Inheritence

		/// <summary>
		/// Requests the session get the store size curve criteria list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
		public SizeCurveCriteriaList GetSizeCurveCriteriaList(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCurveCriteriaList(nodeRID, forCopy, false); // TT#1572 - GRT - added FALSE
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#438 - JScott - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
		/// <summary>
		/// Requests the session default SizeCurveCriteriaProfile for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the SizeCurveCriteriaProfile class containing the default Size Curve Criteria information</returns>
		public SizeCurveCriteriaProfile GetDefaultSizeCurveCriteriaProfile(int nodeRID)
		{
			SizeCurveCriteriaList sccl;
			SizeCurveDefaultCriteriaProfile scdcp;
			SizeCurveCriteriaProfile sccp;

			try
			{
				sccl = HierarchyServerGlobal.GetSizeCurveCriteriaList(nodeRID, false, true);  // TT#1572 - GRT - Urban Alternate Hierarchy
				scdcp = HierarchyServerGlobal.GetSizeCurveDefaultCriteriaProfile(nodeRID, sccl, false);
				return (SizeCurveCriteriaProfile)sccl.FindKey(scdcp.DefaultRID);
			}
			catch
			{
				throw;
			}
		}

		//End TT#438 - JScott - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
		/// <summary>
		/// Requests the session add or update size curve criteria information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sccl">A instance of the SizeCurveDefaultCriteriaList class which contains SizeCurveDefaultCriteriaProfile objects</param>
		public void SizeCurveDefaultCriteriaUpdate(int nodeRID, SizeCurveDefaultCriteriaProfile scdcp)
		{
			try
			{
				_mhd.OpenUpdateConnection();

				switch (scdcp.DefaultChangeType)
				{
					case eChangeType.add:
					case eChangeType.update:
					{
						_mhd.SizeCurveDefaultCriteria_Update(nodeRID, scdcp.DefaultRID);

						break;
					}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SizeCurveDefaultCriteriaUpdate(nodeRID, scdcp);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store size curve criteria list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
		public SizeCurveDefaultCriteriaProfile GetSizeCurveDefaultCriteriaProfile(int nodeRID, SizeCurveCriteriaList sccl, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCurveDefaultCriteriaProfile(nodeRID, sccl, forCopy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update size curve tolerance information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sctl">A instance of the SizeCurveToleranceProfile class which contains SizeCurveToleranceProfile objects</param>
		public void SizeCurveToleranceUpdate(int nodeRID, SizeCurveToleranceProfile sctp)
		{
			try
			{
				_mhd.OpenUpdateConnection();

				_mhd.SizeCurveTolerance_DeleteAll(nodeRID);

				switch (sctp.ToleranceChangeType)
				{
					case eChangeType.add:
					case eChangeType.update:
						{
							_mhd.SizeCurveTolerance_Add(nodeRID, sctp.ToleranceMinAvg, sctp.ToleranceLevelType, sctp.ToleranceLevelRID, sctp.ToleranceLevelSeq, sctp.ToleranceLevelOffset,
                                //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                                //sctp.ToleranceSalesTolerance, sctp.ToleranceIdxUnitsInd, sctp.ToleranceMinTolerance, sctp.ToleranceMaxTolerance);
                                sctp.ToleranceSalesTolerance, sctp.ToleranceIdxUnitsInd, sctp.ToleranceMinTolerance, sctp.ToleranceMaxTolerance, sctp.ApplyMinToZeroTolerance);
                            //End TT#2079

							break;
						}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SizeCurveToleranceUpdate(nodeRID, sctp);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store size curve criteria list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
		public SizeCurveToleranceProfile GetSizeCurveToleranceProfile(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCurveToleranceProfile(nodeRID, forCopy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update size curve similar store information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="scssl">A instance of the SizeCurveSimilarStoreList class which contains SizeCurveSimilarStoreProfile objects</param>
		public void SizeCurveSimilarStoreUpdate(int nodeRID, SizeCurveSimilarStoreList scssl)
		{
			try
			{
				_mhd.OpenUpdateConnection();

				foreach (SizeCurveSimilarStoreProfile scssp in scssl)
				{
					switch (scssp.SimilarStoreChangeType)
					{
						case eChangeType.add:
							{
								_mhd.SizeCurveSimilarStore_Add(nodeRID, scssp.Key, scssp.SimStoreRID, scssp.SimStoreUntilDateRangeRID);

								break;
							}
						case eChangeType.update:
							{
								_mhd.SizeCurveSimilarStore_Update(nodeRID, scssp.Key, scssp.SimStoreRID, scssp.SimStoreUntilDateRangeRID);

								break;
							}
						case eChangeType.delete:
							{
								_mhd.SizeCurveSimilarStore_Delete(nodeRID, scssp.Key);

								break;
							}
					}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SizeCurveSimilarStoreUpdate(nodeRID, scssl);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store eligibility list for the node.
		/// </summary>
		/// <param name="storeList">The list of store for which eligibility is to be looked up</param>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="chaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>A instance of the StoreEligibilityList class containing a StoreEligibilityProfile object for each store</returns>
		public SizeCurveSimilarStoreList GetSizeCurveSimilarStoreList(ProfileList storeList, int nodeRID, bool chaseHierarchy, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCurveSimilarStoreList(storeList, nodeRID, chaseHierarchy, forCopy);
			}
			catch
			{
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
		/// <summary>
		/// Requests the session add or update size out of stock information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="soosp">A instance of the SizeOutOfStockProfile class which contains SizeOutOfStockProfile objects</param>
		public void SizeOutOfStockUpdate(int nodeRID, SizeOutOfStockProfile soosp)
		{
			try
			{
				_mhd.OpenUpdateConnection();

				_mhd.SizeOutOfStock_DeleteAll(nodeRID);

				switch (soosp.ChangeType)
				{
					case eChangeType.add:
					case eChangeType.update:
						{
							_mhd.SizeOutOfStock_Add(nodeRID, soosp.StoreGrpRID, soosp.SizeGrpRID);
							_mhd.SizeOutOfStockGroupLevel_Add(nodeRID, soosp.dsValues.Tables["SetLevelValues"]);
							_mhd.SizeOutOfStockQuantity_Add(nodeRID, soosp.dsValues.Tables["ColorSizeValues"]);
							break;
						}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				//HierarchyServerGlobal.SizeOutOfStockUpdate(nodeRID, soosp);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the size out of stock list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
		public SizeOutOfStockHeaderProfile GetSizeOutOfStockHeaderProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeOutOfStockHeaderProfile(nodeRID, strGrpRID, szGrpRID, forCopy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the size out of stock list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy)
        public SizeOutOfStockProfile GetSizeOutOfStockProfile(int nodeRID, int strGrpRID, int szGrpRID, bool forCopy, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
		{
			try
			{
                // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                //return HierarchyServerGlobal.GetSizeOutOfStockProfile(nodeRID, strGrpRID, szGrpRID, forCopy);
                return HierarchyServerGlobal.GetSizeOutOfStockProfile(nodeRID, strGrpRID, szGrpRID, forCopy, sg_Version);
                // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update size sell thrue information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="soosp">A instance of the SizeSellThruProfile class which contains SizeSellThruProfile objects</param>
		public void SizeSellThruUpdate(int nodeRID, SizeSellThruProfile soosp)
		{
			try
			{
				_mhd.OpenUpdateConnection();

				_mhd.SizeSellThru_DeleteAll(nodeRID);

				switch (soosp.ChangeType)
				{
					case eChangeType.add:
					case eChangeType.update:
						{
							_mhd.SizeSellThru_Add(nodeRID, soosp.SellThruLimit);
							break;
						}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SizeSellThruUpdate(nodeRID, soosp);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the size sell thru list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SizeCurveCriteriaList class containing a SizeCurveCriteriaProfile for each boundary</returns>
		public SizeSellThruProfile GetSizeSellThruProfile(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeSellThruProfile(nodeRID, forCopy);
			}
			catch
			{
				throw;
			}
		}

		//End TT#483 - JScott - Add Size Lost Sales criteria and processing

        //Begin TT#739-MD -jsobek -Delete Stores
		//Begin TT#483 - STodd - Add Size Lost Sales criteria and processing
        //public DataTable GetSizeOutOfStockColorNodes()
        //{
        //    try
        //    {
        //        return HierarchyServerGlobal.GetSizeOutOfStockColorNodes();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetSizeSellThruLimitColorNodes()
        //{
        //    try
        //    {
        //        return HierarchyServerGlobal.GetSizeSellThruLimitColorNodes();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public DataTable ExecuteSizeDayToWeekSummary(int aNodeRID, int startSQLTimeID, int endSQLTimeID, int storeRID = -1)
        {
             return HierarchyServerGlobal.ExecuteSizeDayToWeekSummary(aNodeRID, startSQLTimeID, endSQLTimeID, storeRID);
        }
		//End TT#483 - JScott - Add Size Lost Sales criteria and processing
        //End TT#739-MD -jsobek -Delete Stores

		/// <summary>
		/// Requests the session add or update velocity grade information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="vgl">A instance of the VelocityGradesList class which contains VelocityGradeProfile objects</param>
		public void VelocityGradesUpdate(int nodeRID, VelocityGradeList vgl)
		{
			try
			{
				_mhd.OpenUpdateConnection();
				_mhd.VelocityGrade_DeleteAll(nodeRID);
				foreach (VelocityGradeProfile vgp in vgl)
				{
					//Begin TT#505 - JScott - Velocity - Apply Min/Max
					//switch (vgp.VelocityGradeChangeType)
					//{
					//    case eChangeType.none: 
					//    {
					//        break;
					//    }
					//    case eChangeType.add: 
					//    {
					//        _mhd.VelocityGrade_Add(nodeRID, vgp.VelocityGrade, vgp.Boundary);
							
					//        break;
					//    }
					//    case eChangeType.update: 
					//    {
					//        _mhd.VelocityGrade_Add(nodeRID, vgp.VelocityGrade, vgp.Boundary);
							

					//        break;
					//    }
					//    case eChangeType.delete: 
					//    {
					//        break;
					//    }
					//}
					switch (vgp.VelocityGradeChangeType)
					{
						case eChangeType.none:
						{
							// if no change, re-add the store grade information since it was deleted above
							if (vgp.VelocityGradeIsInherited == false)
							{
								_mhd.VelocityGrade_Add(nodeRID, vgp.VelocityGrade, vgp.Boundary);
							}
							break;
						}
						case eChangeType.add:
						{
							_mhd.VelocityGrade_Add(nodeRID, vgp.VelocityGrade, vgp.Boundary);
							if (vgp.VelocityMinMaxChangeType == eChangeType.add)
							{
								vgp.VelocityMinMaxChangeType = eChangeType.update;
							}
							break;
						}
						case eChangeType.update:
						{
							_mhd.VelocityGrade_Update(nodeRID, vgp.VelocityGrade, vgp.Boundary);
							if (vgp.VelocityMinMaxChangeType == eChangeType.add)
							{
								vgp.VelocityMinMaxChangeType = eChangeType.update;
							}
							break;
						}
						case eChangeType.delete:
						{
							_mhd.VelocityGrade_Delete(nodeRID, vgp.Boundary);

							break;
						}
					}

					switch (vgp.VelocityMinMaxChangeType)
					{
						case eChangeType.none:
						{
							break;
						}
						case eChangeType.add:
						{
							_mhd.VelocityMinMax_Add(nodeRID, vgp.Boundary,
								vgp.VelocityMinStock, vgp.VelocityMaxStock, vgp.VelocityMinAd);
							break;
						}
						case eChangeType.update:
						{
							_mhd.VelocityMinMax_Update(nodeRID, vgp.Boundary,
								vgp.VelocityMinStock, vgp.VelocityMaxStock, vgp.VelocityMinAd);
							break;
						}
						case eChangeType.delete:
						{
							_mhd.VelocityMinMax_Update(nodeRID, vgp.Boundary,
								Include.Undefined, Include.Undefined, Include.Undefined);
							break;
						}
					}
					//End TT#505 - JScott - Velocity - Apply Min/Max
				}
				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.VelocityGradesUpdate(nodeRID, vgl);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the velocity grade list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the VelocityGradeList class containing a VelocityGradeProfile for each boundary</returns>
		//Begin TT#505 - JScott - Velocity - Apply Min/Max
		public VelocityGradeList GetVelocityGradeList(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetVelocityGradeList(nodeRID, forCopy, false);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the velocity grade list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <param name="forAdmin">A flag identifying if the list is being retrieved for use by the admin app</param>
		/// <returns>An instance of the VelocityGradeList class containing a VelocityGradeProfile for each boundary</returns>
		public VelocityGradeList GetVelocityGradeList(int nodeRID, bool forCopy, bool forAdmin)
		//End TT#505 - JScott - Velocity - Apply Min/Max
		{
			try
			{
				//Begin TT#505 - JScott - Velocity - Apply Min/Max
				//return HierarchyServerGlobal.GetVelocityGradeList(nodeRID, forCopy);
				return HierarchyServerGlobal.GetVelocityGradeList(nodeRID, forCopy, forAdmin);
				//End TT#505 - JScott - Velocity - Apply Min/Max
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update sell thru percentages information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="stpl">A instance of the SellThruPctList class which contains a SellThruPctProfile for each percentage</param>
		public void SellThruPctsUpdate(int nodeRID, SellThruPctList stpl)
		{
			try
			{
				_mhd.OpenUpdateConnection();
				_mhd.SellThruPcts_DeleteAll(nodeRID);
				foreach (SellThruPctProfile stpp in stpl)
				{

					switch (stpp.SellThruPctChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							_mhd.SellThruPct_Add(nodeRID, stpp.SellThruPct);
							
							break;
						}
						case eChangeType.update: 
						{
							_mhd.SellThruPct_Add(nodeRID, stpp.SellThruPct);

							break;
						}
						case eChangeType.delete: 
						{

							break;
						}
					}
				}
				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

				HierarchyServerGlobal.SellThruPctsUpdate(nodeRID, stpl);
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the sell thru percent list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
		/// <returns>An instance of the SellThruPctList class which contains a SellThruPctProfile object for each percentage</returns>
		public SellThruPctList GetSellThruPctList(int nodeRID, bool forCopy)
		{
			try
			{
				return HierarchyServerGlobal.GetSellThruPctList(nodeRID, forCopy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update store daily percentages information 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="sdpl">A instance of the StoreDailyPercentagesList class which contains StoreDailyPercentagesProfile objects</param>
		public void StoreDailyPercentagesUpdate(int nodeRID, StoreDailyPercentagesList sdpl, bool cacheCleared)
		{
			try
			{
				_mhd.OpenUpdateConnection();
				foreach (StoreDailyPercentagesProfile sdpp in sdpl)
				{

					// update store daily percentages defaults
					switch (sdpp.StoreDailyPercentagesDefaultChangeType)
					{
						case eChangeType.none: 
						{
							break;
						}
						case eChangeType.add: 
						{
							_mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);
							if (sdpp.Day1Default != 0 ||
								sdpp.Day2Default != 0 || 
								sdpp.Day3Default != 0 || 
								sdpp.Day4Default != 0 ||
								sdpp.Day5Default != 0 ||
								sdpp.Day6Default != 0 || 
								sdpp.Day7Default != 0)
							{
								_mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
									sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
									sdpp.Day6Default, sdpp.Day7Default);
							}
			
							break;
						}
						case eChangeType.update: 
						{
							_mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);
							if (sdpp.Day1Default != 0 ||
								sdpp.Day2Default != 0 || 
								sdpp.Day3Default != 0 || 
								sdpp.Day4Default != 0 ||
								sdpp.Day5Default != 0 ||
								sdpp.Day6Default != 0 || 
								sdpp.Day7Default != 0)
							{
								_mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
									sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
									sdpp.Day6Default, sdpp.Day7Default);
							}
							break;
						}
						case eChangeType.delete: 
						{
							_mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);

							break;
						}
					}

					// update store daily percentages by time
					//_mhd.DailyPercentages_DeleteAll(nodeRID, sdpp.Key); //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
					foreach (DailyPercentagesProfile dpp in sdpp.DailyPercentagesList)
					{
						switch (dpp.DailyPercentagesChangeType)
						{
							case eChangeType.none: 
							{
								//BEGIN TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
                                if (sdpp.StoreDailyPercentagesIsInherited)
                                {
                                    if (dpp.Day1 != 0 ||
                                        dpp.Day2 != 0 ||
                                        dpp.Day3 != 0 ||
                                        dpp.Day4 != 0 ||
                                        dpp.Day5 != 0 ||
                                        dpp.Day6 != 0 ||
                                        dpp.Day7 != 0)
                                    {
                                        _mhd.DailyPercentages_Add(nodeRID, sdpp.Key, dpp.DateRange.Key, dpp.Day1,
                                            dpp.Day2, dpp.Day3, dpp.Day4, dpp.Day5, dpp.Day6, dpp.Day7);
                                    }
                                }
								//END TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
								
								break;
							}
							case eChangeType.add: 
							{
								if (dpp.Day1 != 0 ||
									dpp.Day2 != 0 || 
									dpp.Day3 != 0 ||
									dpp.Day4 != 0 || 
									dpp.Day5 != 0 ||
									dpp.Day6 != 0 || 
									dpp.Day7 != 0)
								{
									_mhd.DailyPercentages_Add(nodeRID, sdpp.Key, dpp.DateRange.Key, dpp.Day1,
										dpp.Day2, dpp.Day3, dpp.Day4, dpp.Day5, dpp.Day6, dpp.Day7);
								}
								//BEGIN TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
                                if (sdpp.StoreDailyPercentagesIsInherited)
                                {
                                    _mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
                                    sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
                                    sdpp.Day6Default, sdpp.Day7Default);
                                }
                                //END TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
								
								break;
							}
							case eChangeType.update: 
							{
								_mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, dpp.DateRange.Key);
								if (dpp.Day1 != 0 ||
									dpp.Day2 != 0 || 
									dpp.Day3 != 0 ||
									dpp.Day4 != 0 || 
									dpp.Day5 != 0 ||
									dpp.Day6 != 0 || 
									dpp.Day7 != 0)
								{
									_mhd.DailyPercentages_Add(nodeRID, sdpp.Key, dpp.DateRange.Key, dpp.Day1,
										dpp.Day2, dpp.Day3, dpp.Day4, dpp.Day5, dpp.Day6, dpp.Day7);
								}
								//BEGIN TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
                                if (sdpp.StoreDailyPercentagesIsInherited)
                                {
                                    _mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
                                    sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
                                    sdpp.Day6Default, sdpp.Day7Default);
                                }
								//END TT#4123 - DOConnell - Daily Percentages API not loading all transactions in file
								
								break;
							}
							case eChangeType.delete: 
							{
								_mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, dpp.DateRange.Key);
								break;
							}
						}
					}
				}

				_mhd.CommitData();
				_mhd.CloseUpdateConnection();

                if (!cacheCleared) HierarchyServerGlobal.StoreDailyPercentagesUpdate(nodeRID, sdpl); // TT#2015 - gtaylor - apply changes to lower levels
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_mhd.ConnectionIsOpen)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get the store daily percentages list for the node.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the StoreDailyPercentagesList class which contains a StoreDailyPercentagesProfile object for each store with daily percentages.</returns>
		public StoreDailyPercentagesList GetStoreDailyPercentagesList(ProfileList storeList, int nodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetStoreDailyPercentagesList(storeList, nodeRID, false);
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        /// <summary>
        /// Requests the session get the store daily percentages list for the node.
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <returns>An instance of the StoreDailyPercentagesList class which contains a StoreDailyPercentagesProfile object for each store with daily percentages.</returns>
        public StoreDailyPercentagesList GetStoreDailyPercentagesList(StoreProfile storeProf, int nodeRID)
        {
            try
            {
                return HierarchyServerGlobal.GetStoreDailyPercentagesList(storeProf, nodeRID, false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session add or update store capacity information 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="scl">A instance of the StoreCapacityList class which contains StoreCapacityProfile objects for each store</param>
        public eReturnCode StoreDailyPercentagesUpdate(ProfileList datelist, int nodeRID, StoreDailyPercentagesList sdpl, DateRangeProfile drp, bool cacheCleared)
        {
            
            try
            {
                _mhd.OpenUpdateConnection();
                foreach (StoreDailyPercentagesProfile sdpp in sdpl)
                {

                    // update store daily percentages defaults
                    switch (sdpp.StoreDailyPercentagesDefaultChangeType)
                    {
                        case eChangeType.none:
                            {
                                break;
                            }
                        case eChangeType.add:
                            {
                                _mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);
                                if (sdpp.Day1Default != 0 ||
                                    sdpp.Day2Default != 0 ||
                                    sdpp.Day3Default != 0 ||
                                    sdpp.Day4Default != 0 ||
                                    sdpp.Day5Default != 0 ||
                                    sdpp.Day6Default != 0 ||
                                    sdpp.Day7Default != 0)
                                {
                                    _mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
                                        sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
                                        sdpp.Day6Default, sdpp.Day7Default);
                                }

                                break;
                            }
                        case eChangeType.update:
                            {
                                _mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);
                                if (sdpp.Day1Default != 0 ||
                                    sdpp.Day2Default != 0 ||
                                    sdpp.Day3Default != 0 ||
                                    sdpp.Day4Default != 0 ||
                                    sdpp.Day5Default != 0 ||
                                    sdpp.Day6Default != 0 ||
                                    sdpp.Day7Default != 0)
                                {
                                    _mhd.DailyPercentagesDefaults_Add(nodeRID, sdpp.Key, sdpp.Day1Default,
                                        sdpp.Day2Default, sdpp.Day3Default, sdpp.Day4Default, sdpp.Day5Default,
                                        sdpp.Day6Default, sdpp.Day7Default);
                                }
                                break;
                            }
                        case eChangeType.delete:
                            {
                                _mhd.DailyPercentagesDefaults_Delete(nodeRID, sdpp.Key);

                                break;
                            }
                    }

                    // update store daily percentages by time
                   // _mhd.DailyPercentages_DeleteAll(nodeRID, sdpp.Key);
                    //_mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, drp.Key);
                    foreach (DailyPercentagesProfile dpp in sdpp.DailyPercentagesList)
                    {

                        switch (dpp.DailyPercentagesChangeType)
                        {
                            case eChangeType.none:
                                {
                                    break;
                                }
                            case eChangeType.add:
                                {
                                    

                                    _mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, drp.Key);


                                    if (dpp.Day1 != 0 ||
                                        dpp.Day2 != 0 ||
                                        dpp.Day3 != 0 ||
                                        dpp.Day4 != 0 ||
                                        dpp.Day5 != 0 ||
                                        dpp.Day6 != 0 ||
                                        dpp.Day7 != 0)
                                    {
                                        _mhd.DailyPercentages_Add(nodeRID, sdpp.Key, drp.Key, dpp.Day1,
                                            dpp.Day2, dpp.Day3, dpp.Day4, dpp.Day5, dpp.Day6, dpp.Day7);
                                    }
                                    break;
                                }
                            case eChangeType.update:
                                {
                                    _mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, drp.Key);
                                    if (dpp.Day1 != 0 ||
                                        dpp.Day2 != 0 ||
                                        dpp.Day3 != 0 ||
                                        dpp.Day4 != 0 ||
                                        dpp.Day5 != 0 ||
                                        dpp.Day6 != 0 ||
                                        dpp.Day7 != 0)
                                    {
                                        _mhd.DailyPercentages_Add(nodeRID, sdpp.Key, dpp.DateRange.Key, dpp.Day1,
                                            dpp.Day2, dpp.Day3, dpp.Day4, dpp.Day5, dpp.Day6, dpp.Day7);
                                    }
                                    break;
                                }
                            case eChangeType.delete:
                                {
                                    _mhd.DailyPercentages_Delete(nodeRID, sdpp.Key, dpp.DateRange.Key);
                                    break;
                                }
                        }
                    }
                }

                _mhd.CommitData();
                _mhd.CloseUpdateConnection();

                if (!cacheCleared) HierarchyServerGlobal.StoreDailyPercentagesUpdate(nodeRID, sdpl); // TT#2015 - gtaylor - apply changes to lower levels
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
            }
            return eReturnCode.successful;
        }


        //End TT#43 - MD - DOConnell - Projected Sales Enhancement

		/// <summary>
		/// Requests the session get the store daily percentages for all stores for a node and week.
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="yearWeek">The year/week for which eligibility is to be determined</param>
		/// <returns>An instance of the StoreWeekDailyPercentagesList containing StoreWeekDailyPercentagesProfile objects</returns>
		public StoreWeekDailyPercentagesList GetStoreDailyPercentages(int nodeRID, int yearWeek)
		{
			try
			{
				StoreWeekDailyPercentagesProfile swdpp;
				bool storeValuesSet = false;

				// calculate an even spread
				BasicSpread spreader = new BasicSpread();
				ArrayList inValues = new ArrayList();
				ArrayList outValues;

				// add a zero for each day for an even spread
				for(int i=0; i<7; i++)
				{
					inValues.Add(0);
				}
				 
				spreader.ExecuteSimpleSpread(100, inValues, 3, out outValues);

				ProfileList storeList = GetProfileList(eProfileType.Store);
				StoreWeekDailyPercentagesList swdpl = new StoreWeekDailyPercentagesList(eProfileType.DailyPercentages);	
				StoreDailyPercentagesList sdpl = GetStoreDailyPercentagesList(storeList, nodeRID);
				foreach(StoreDailyPercentagesProfile sdpp in sdpl)	// only process store that have daily percentages
				{
					storeValuesSet = false;
					swdpp = new StoreWeekDailyPercentagesProfile(sdpp.Key);
					swdpp.YearWeek = yearWeek;
					// load default values
					if (sdpp.HasDefaultValues)
					{
						storeValuesSet = true;
						swdpp.DailyPercentages[0] = sdpp.Day1Default;
						swdpp.DailyPercentages[1] = sdpp.Day2Default;
						swdpp.DailyPercentages[2] = sdpp.Day3Default;
						swdpp.DailyPercentages[3] = sdpp.Day4Default;
						swdpp.DailyPercentages[4] = sdpp.Day5Default;
						swdpp.DailyPercentages[5] = sdpp.Day6Default;
						swdpp.DailyPercentages[6] = sdpp.Day7Default;
					}

					foreach(DailyPercentagesProfile dpp in sdpp.DailyPercentagesList)
					{
						//storeValuesSet = true; // MID Track 4461 Index Out of Range / Null Reference error when using daily percentages
						ProfileList weekProfileList = Calendar.GetWeekRange(dpp.DateRange, null);
						foreach (WeekProfile weekProfile in weekProfileList)
						{
							if (weekProfile.YearWeek == yearWeek)
							{
								storeValuesSet = true; // MID Track 4461 Index Out of Range / Null Reference error when using daily percentages
								swdpp.DailyPercentages[0] = dpp.Day1;
								swdpp.DailyPercentages[1] = dpp.Day2;
								swdpp.DailyPercentages[2] = dpp.Day3;
								swdpp.DailyPercentages[3] = dpp.Day4;
								swdpp.DailyPercentages[4] = dpp.Day5;
								swdpp.DailyPercentages[5] = dpp.Day6;
								swdpp.DailyPercentages[6] = dpp.Day7;
								break;
							}
						}
					}
					if (storeValuesSet)
					{
						swdpl.Add(swdpp);
					}
				}
				//  set stores with daily percentages to an even spread
				if (swdpl.Count != storeList.Count)  // MID Track 4461 Index Out of Range / Null Reference error when using daily percentages
				{
					foreach (StoreProfile sp in storeList)
					{
						if (!swdpl.Contains(sp.Key)) // MID Track 4461 Index Out of Range / Null Reference error when using daily percentages
						{
							swdpp = new StoreWeekDailyPercentagesProfile(sp.Key);
							swdpp.DailyPercentages[0] = (double)outValues[0];
							swdpp.DailyPercentages[1] = (double)outValues[1];
							swdpp.DailyPercentages[2] = (double)outValues[2];
							swdpp.DailyPercentages[3] = (double)outValues[3];
							swdpp.DailyPercentages[4] = (double)outValues[4];
							swdpp.DailyPercentages[5] = (double)outValues[5];
							swdpp.DailyPercentages[6] = (double)outValues[6];
							swdpl.Add(swdpp);
						}
					}
				}

				return swdpl;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#634 - JSmith - Color rename
        /// <summary>
        /// Requests the session get all styles that contain a color code.
        /// </summary>
        /// <param name="colorCodeRID">The record id of the color code</param>
        /// <returns>An instance of the HierarchyNodeList class which contains a HierarchyNodeProfile class for each style</returns>
        public HierarchyNodeList GetStylesForColor(int colorCodeRID)
        {
            try
            {
                return HierarchyServerGlobal.GetStylesForColor(colorCodeRID);
            }
            catch
            {
                throw;
            }
        }
        // End TT#634

		/// <summary>
		/// Requests the session get information about a color code.
		/// </summary>
		/// <param name="colorCodeRID">The record id of the color code</param>
		/// <returns>An instance of the ColorCodeProfile class which contains information for the color</returns>
		public ColorCodeProfile GetColorCodeProfile(int colorCodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetColorCodeProfile(colorCodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get information about a color code.
		/// </summary>
		/// <param name="colorCodeID">The id of the color code</param>
		/// <returns>An instance of the ColorCodeProfile class which contains information for the color</returns>
		public ColorCodeProfile GetColorCodeProfile(string colorCodeID)
		{
			try
			{
				return HierarchyServerGlobal.GetColorCodeProfile(colorCodeID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update color information 
		/// </summary>
		/// <param name="ccp">A instance of the ColorCodeProfile class containing information about the color</param>
		/// <returns>An updated instance of the ColorCodeProfile class containing the record id of the color with the other information</returns>
		public ColorCodeProfile ColorCodeUpdate(ColorCodeProfile ccp)
		{
            // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
            MerchandiseHierarchyData dataLock = null;
            // End Track #6015

			try
			{
                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                // determine if node already added after lock is applied
                dataLock = new MerchandiseHierarchyData();
                dataLock.OpenUpdateConnection(eLockType.ColorCode, "lock");

                if (ccp.ColorCodeChangeType == eChangeType.add)
                {
                    ColorCodeProfile colorCodeProfile = GetColorCodeProfile(ccp.ColorCodeID);
                    if (colorCodeProfile.Key != Include.NoRID)
                    {
                        return colorCodeProfile;
                    }
                }
                // End Track #6015

				switch (ccp.ColorCodeChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_cd.OpenUpdateConnection(eLockType.ColorCode, ccp.ColorCodeID);
						if (!_cd.Color_Exists(ccp.ColorCodeID))
						{
                            ccp.Key = _cd.Color_Add(ccp.ColorCodeID, ccp.ColorCodeName, ccp.ColorCodeGroup, false, ePurpose.Default);
							_cd.CommitData();
						}
						else
						{
							ccp.ColorCodeChangeType = eChangeType.update;
							ccp.Key = _cd.GetColorCodeRID(ccp.ColorCodeID);
						}
						_cd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_cd.OpenUpdateConnection();
                        _cd.Color_Update(ccp.Key, ccp.ColorCodeID, ccp.ColorCodeName, ccp.ColorCodeGroup, false, ePurpose.Default);
						_cd.CommitData();
						_cd.CloseUpdateConnection();
						break;
					}
					case eChangeType.delete: 
					{
						_cd.OpenUpdateConnection();
						_cd.Color_Delete(ccp.Key);
						_cd.CommitData();
						_cd.CloseUpdateConnection();
						break;
					}
				}

				HierarchyServerGlobal.ColorCodeUpdate(ccp);
				return ccp;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_cd.ConnectionIsOpen)
				{
					_cd.CloseUpdateConnection();
				}

                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
                // End Track #6015
            }
		}

		/// <summary>
		/// Requests the session get a list of the colors in the system.
		/// </summary>
		/// <returns>An instance of the ColorCodeList class containing an ColorCodeProfile for each color in the system.</returns>
		public ColorCodeList GetColorCodeList()
		{
			try
			{
				return HierarchyServerGlobal.GetColorCodeList();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a Hashtable of the colors in the system keyed by ID.
		/// </summary>
        public Dictionary<string, int> GetColorCodeListByID()
		{
			try
			{
				return HierarchyServerGlobal.GetColorCodeListByID();
			}
			catch
			{
				throw;
			}
		}

        /// <summary>
        /// Retrieve a list of place holder color keys
        /// </summary>
        /// <param name="aNumberOfPlaceholderColors">
        /// The number of placeholder color profiles to retrieve
        /// </param>
        /// <param name="aCurrentPlaceholderColors">
        /// The list of current placeholder color keys.  This is used to filter the list
        /// </param>
        /// <returns>ColorCodeList of placeholder color code profiles</returns>
        public ColorCodeList GetPlaceholderColors(int aNumberOfPlaceholderColors, ArrayList aCurrentPlaceholderColors)
        {
            try
            {
                return HierarchyServerGlobal.GetPlaceholderColors(aNumberOfPlaceholderColors, aCurrentPlaceholderColors);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve placeholder style nodes
        /// </summary>
        /// <param name="aAnchorNode">
        /// The key of the anchor node for the placeholder style.
        /// </param>
        /// <param name="aNumberOfPlaceholderStyles">
        /// The number of placeholder styles to create
        /// </param>
        /// <param name="aCurrentNumberOfPlaceholderStyles">
        /// The current number of placeholder styles already defined to the assortment
        /// </param>
        /// <param name="aAssortmentID">
        /// The ID of the assortment to use in the placeholder styles ID
        /// </param>
        /// <returns>HierarchyNodeList of HierarchyNodeProfiles with the placeholder style nodes</returns>
        public HierarchyNodeList GetPlaceholderStyles(int aAnchorNode, int aNumberOfPlaceholderStyles,
            int aCurrentNumberOfPlaceholderStyles, int aAssortmentRID)
        {
            try
            {
                return HierarchyServerGlobal.GetPlaceholderStyles(aAnchorNode, aNumberOfPlaceholderStyles,
                        aCurrentNumberOfPlaceholderStyles, aAssortmentRID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the lowest connector node for the anchor node
        /// </summary>
        /// <param name="aAnchorNode">
        /// The key of the anchor node.
        /// </param>
        ///<returns>
        ///An instance of the HierarchyNodeProfile with the connector node.
        ///</returns>
        public HierarchyNodeProfile GetLowestConnectorNode(int aAnchorNode)
        {
            try
            {
                return HierarchyServerGlobal.GetLowestConnectorNode(aAnchorNode);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve the anchor node for a node
        /// </summary>
        /// <param name="aNodeRID">
        /// The key of the node for which the anchor node is to be retrieved.
        /// </param>
        /// <returns>HierarchyNodeProfile with the anchor node. Key has -1 if no anchor node is found.</returns>
        public HierarchyNodeProfile GetAnchorNode(int aNodeRID)
        {
            try
            {
                return HierarchyServerGlobal.GetAnchorNode(aNodeRID);
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Requests the session get information about a size code.
		/// </summary>
		/// <param name="sizeCodeRID">The record id of the size code</param>
		/// <returns>An instance of the SizeCodeProfile class containing information about the size</returns>
		public SizeCodeProfile GetSizeCodeProfile(int sizeCodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeProfile(sizeCodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get information about a size code.
		/// </summary>
		/// <param name="sizeCodeID">The id of the size code</param>
		/// <returns>An instance of the SizeCodeProfile class containing information about the size</returns>
		public SizeCodeProfile GetSizeCodeProfile(string sizeCodeID)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeProfile(sizeCodeID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the RID of the size code with the given primary and secondary size codes.  If it does not exist, Include.NoRID is returned.
		/// </summary>
		/// <param name="aSizeCodePrimary">The primary size code.</param>
		/// <param name="aSizeCodeSecondary">The secondary size code.</param>
		/// <returns>The RID of the size code or Include.NoRID if it doesn't exist.</returns>
		public int GetSizeCodeRID(string aProductCategory, string aSizeCodePrimary, string aSizeCodeSecondary)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeRID(aProductCategory, aSizeCodePrimary, aSizeCodeSecondary);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session add or update size information 
		/// </summary>
		/// <param name="scp">A instance of the SizeCodeProfile class containing information about the size</param>
		/// <returns>An updated instance of the SizeCodeProfile class containing the record id of the size with the other information</returns>
		public SizeCodeProfile SizeCodeUpdate(SizeCodeProfile scp)
		{
            // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
            MerchandiseHierarchyData dataLock = null;
            // End Track #6015

			try
			{
                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                // determine if node already added after lock is applied
                dataLock = new MerchandiseHierarchyData();
                dataLock.OpenUpdateConnection(eLockType.SizeCode, "lock");

                if (scp.SizeCodeChangeType == eChangeType.add)
                {
                    SizeCodeProfile sizeCodeProfile = GetSizeCodeProfile(scp.SizeCodeID);
                    if (sizeCodeProfile.Key != Include.NoRID)
                    {
                        return sizeCodeProfile;
                    }
                }
                // End Track #6015

				ValidateSizeCodeProfile(scp);
				string noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
				switch (scp.SizeCodeChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						_sgd.OpenUpdateConnection(eLockType.SizeCode, scp.SizeCodeID);
						if (!_sgd.Size_Exists(scp.SizeCodeID))
						{
							scp.Key = _sgd.Size_Add(scp.SizeCodeID, scp.SizeCodePrimary, scp.SizeCodeSecondary,
								scp.SizeCodeProductCategory);
							_sgd.CommitData();
							{
								if (scp.SizeCodeSecondary == null ||
									scp.SizeCodeSecondary.Trim().Length == 0)
								{
									scp.SizeCodeSecondary = noSizeDimensionLbl;
								}
							}
						}
						else
						{
							scp.SizeCodeChangeType = eChangeType.update;
							scp.Key = _sgd.GetSizeCodeRID(scp.SizeCodeID);
						}
						_sgd.CloseUpdateConnection();
						break;
					}
					case eChangeType.update: 
					{
						_sgd.OpenUpdateConnection();
						_sgd.Size_Update(scp.Key, scp.SizeCodeID, scp.SizeCodePrimary, scp.SizeCodeSecondary,
							scp.SizeCodeProductCategory);
						_sgd.CommitData();
						_sgd.CloseUpdateConnection();
						break;
					}
					case eChangeType.delete: 
					{
						_sgd.OpenUpdateConnection();
						_sgd.Size_Delete(scp.Key);
						_sgd.CommitData();
						_sgd.CloseUpdateConnection();
						break;
					}
				}

				HierarchyServerGlobal.SizeCodeUpdate(scp);
				return scp;
			}
			catch (SizeCatgPriSecNotUniqueException)
			{
				throw;
			}
			catch (SizePrimaryRequiredException)
			{
				throw;
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{ 
				if (_sgd.ConnectionIsOpen)
				{
					_sgd.CloseUpdateConnection();
				}

                // Begin Track #6015 - KJohnson - ForeignKey Violation Database Error
                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
                // End Track #6015
            }
		}

		/// <summary>
		/// Validates the information about a size code
		/// </summary>
		/// <param name="scp">An instance of the SizeCodeProfile class containing information for a size code</param>
		private void ValidateSizeCodeProfile(SizeCodeProfile scp)
		{
			try
			{
				if (scp.SizeCodePrimary == null ||
					scp.SizeCodePrimary.Trim().Length == 0 ||
					scp.SizeCodePrimary == string.Empty)
				{
					throw new SizePrimaryRequiredException();
				}

				if (scp.SizeCodeProductCategory == null ||
					scp.SizeCodeProductCategory.Trim().Length == 0 ||
					scp.SizeCodeProductCategory == string.Empty)
				{
					throw new SizeCodeNotValidException(MIDText.GetTextOnly(eMIDTextCode.ProductCategoryRequired));
				}

				int sizeCodeRID = HierarchyServerGlobal.GetSizeCodeRID(scp.SizeCodeProductCategory, scp.SizeCodePrimary, scp.SizeCodeSecondary);
				switch (scp.SizeCodeChangeType)
				{
					case eChangeType.none: 
					{
						break;
					}
					case eChangeType.add: 
					{
						if (sizeCodeRID != Include.NoRID)
						{
							SizeCodeProfile scp2 = GetSizeCodeProfile(sizeCodeRID);
							throw new SizeCatgPriSecNotUniqueException(scp2.SizeCodeID);
						}
						break;
					}
					case eChangeType.update:
					{
						if (sizeCodeRID != Include.NoRID)
						{
							if (scp.Key != sizeCodeRID)
							{
								SizeCodeProfile scp2 = GetSizeCodeProfile(sizeCodeRID);
								throw new SizeCatgPriSecNotUniqueException(scp2.SizeCodeID);
							}
						}
						break;
					}
					case eChangeType.delete: 
					{
						break;
					}
				}
			}
			catch ( Exception ex )
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a list of the product categories for the sizes in the system.
		/// </summary>
		/// <returns>An ArrayList of unique size product category names.</returns>
		public ArrayList GetSizeProductCategoryList()
		{
			try
			{
				return HierarchyServerGlobal.GetSizeProductCategoryList();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a list of all size codess in the system.
		/// </summary>
		/// <returns>
		/// A SizeCodeList object that contains SizeCodeProfile objects for each size code in the system.
		/// </returns>
		public SizeCodeList GetSizeCodeList()
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeList();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the list of size codes in the system for the product category.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <returns>
		/// A SizeCodeList object that contains SizeCodeProfile objects for each size code in the system
		/// that match the given product category.
		/// </returns>
		public SizeCodeList GetSizeCodeList(string productCategory)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeList(productCategory);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get information about the size codes that are within the product category
		/// and contains the characters in the primary and secondary fields anywhere in the size code fields.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="primary">The primary description of the size.  Use null if not provided.</param>
		/// <param name="secondary">The secondary description of the size.  Use null if not provided.</param>
		/// <returns>
		/// A SizeCodeList object that contains SizeCodeProfile objects that match the query
		/// </returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		public SizeCodeList GetSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeList(productCategory, primary, secondary);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get information about the size codes that are within the product category
		/// and contains the exact primary and secondary fields.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="primary">The primary description of the size.  Use null if not provided.</param>
		/// <param name="secondary">The secondary description of the size.  Use null if not provided.</param>
		/// <returns>
		/// A SizeCodeList object that contains SizeCodeProfile objects that match the query
		/// </returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		public SizeCodeList GetExactSizeCodeList(string productCategory, string primary, string secondary)
		{
			try
			{
				return HierarchyServerGlobal.GetExactSizeCodeList(productCategory, primary, secondary);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns an instance of a SizeCodeList object with the size codes that match the
		/// query of productCategory, primary, and secondary.
		/// </summary>
		/// <param name="productCategory">The product category of the size.</param>
		/// <param name="productCategorySearchContent">The type of search for product category.</param>
		/// <param name="primary">The primary description of the size.</param>
		/// <param name="primarySearchContent">The type of search for primary.</param>
		/// <param name="secondary">The secondary description of the size.</param>
		/// <param name="secondarySearchContent">The type of search secondary.</param>
		/// <returns>A SizeCodeList object that contains SizeCodeProfile objects that match the query</returns>
		/// <remarks>
		/// Returns empty SizeCodeList if no size codes match the query.
		/// </remarks>
		public SizeCodeList GetSizeCodeList(string productCategory, eSearchContent productCategorySearchContent,
			string primary, eSearchContent primarySearchContent, 
			string secondary, eSearchContent secondarySearchContent)
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeList(productCategory, productCategorySearchContent,
					primary, primarySearchContent, secondary, secondarySearchContent);
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Requests the session get a Hashtable of the sizes in the system keyed by ID.
		/// </summary>
        public Dictionary<string, int> GetSizeCodeListByID()
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeListByID();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a Hashtable of the sizes in the system keyed by ID.
		/// </summary>
        public Dictionary<string, int> GetSizeCodeListByPriSec()
		{
			try
			{
				return HierarchyServerGlobal.GetSizeCodeListByPriSec();
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
        // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
        //public ArrayList GetAggregateSizeNodeList(HierarchyNodeProfile aNodeProf, int aVersionRID, int aLowLvlOverRID)
        public ArrayList GetAggregateSizeNodeList(HierarchyNodeProfile aNodeProf, int aVersionRID, int aLowLvlOverRID, ArrayList aSizeCodeFilter)
        // ENd TT#1952
		{
			//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
			//int lowLevelSeq;
			//HierarchyProfile hierProf;
			//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
            // Begin TT#2231 - JSmith - Size curve build failing
            MerchandiseHierarchyData mhd;
            //int i;
            //HierarchyLevelProfile hlp;
            //LowLevelVersionOverrideProfileList llvopl;
            //HierarchyNodeProfile styleNodeProf;
            //HierarchyNodeProfile colorNodeProf;
            //Hashtable styleHash;
            //Hashtable colorHash;
            // End TT#2231

            // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
            Hashtable sizeCodeHash = null;
            // End TT#1952
			ArrayList sizeKeyList;

			try
			{
				// Build Hierarchy Node list to retrieve

                // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
                if (aSizeCodeFilter != null)
                {
                    sizeCodeHash = new Hashtable();
                    foreach (SizeCodeProfile scp in aSizeCodeFilter)
                    {
                        sizeCodeHash[scp.Key] = null;
                    }
                }
                // End TT#1952

				//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
				//lowLevelSeq = 0;
				//hierProf = GetHierarchyData(GetNodeData(aNodeProf.Key).HomeHierarchyRID);

				//for (i = aNodeProf.HomeHierarchyRID + 1; i <= hierProf.HierarchyLevels.Count; i++)
				//{
				//    hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];

				//    if (hlp.LevelType == eHierarchyLevelType.Size)
				//    {
				//        lowLevelSeq = hlp.Level;
				//        break;
				//    }
				//}

				//llvopl = GetOverrideList(
				//    aLowLvlOverRID,
				//    aNodeProf.Key,
				//    aVersionRID,
				//    eHierarchyDescendantType.levelType,
				//    lowLevelSeq,
				//    Include.NoRID,
				//    false,
				//    false,
				//    false);
                // Begin TT#2231 - JSmith - Size curve build failing
                //llvopl = GetOverrideList(
                //    aLowLvlOverRID,
                //    aNodeProf.Key,
                //    aVersionRID,
                //    eHierarchyLevelType.Size,
                //    Include.NoRID,
                //    false,
                //    false,
                //    false);
                ////End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed

               

                //styleHash = new Hashtable();
                //colorHash = new Hashtable();
                //sizeKeyList = new ArrayList();

                //foreach (LowLevelVersionOverrideProfile llvop in llvopl)
                //{
                //    // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
                //    if (sizeCodeHash != null &&
                //        !sizeCodeHash.ContainsKey(llvop.NodeProfile.ColorOrSizeCodeRID))
                //    {
                //        continue;
                //    }
                //    // End TT#1952

                //    colorNodeProf = (HierarchyNodeProfile)colorHash[llvop.NodeProfile.HomeHierarchyParentRID];

                //    if (colorNodeProf == null)
                //    {
                //        colorNodeProf = GetNodeData(llvop.NodeProfile.HomeHierarchyParentRID, false);
                //        colorHash[llvop.NodeProfile.HomeHierarchyParentRID] = colorNodeProf;
                //    }

                //    styleNodeProf = (HierarchyNodeProfile)styleHash[colorNodeProf.HomeHierarchyParentRID];

                //    if (styleNodeProf == null)
                //    {
                //        styleNodeProf = GetNodeData(colorNodeProf.HomeHierarchyParentRID, false);
                //        colorHash[colorNodeProf.HomeHierarchyParentRID] = styleNodeProf;
                //    }

                //    sizeKeyList.Add(new SizeAggregateBasisKey(styleNodeProf.Key, colorNodeProf.ColorOrSizeCodeRID, llvop.NodeProfile.ColorOrSizeCodeRID));
                //}

                LowLevelVersionOverrideProfileList llvopl2 = GetOverrideListOfSizes(aLowLvlOverRID, aNodeProf.Key, true);
                mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.GetStyleColorSizes(aNodeProf.Key);
                sizeKeyList = new ArrayList();
                DataRow[] rows;
                foreach (LowLevelVersionOverrideProfile llvop in llvopl2)
                {
                    if (sizeCodeHash != null &&
                        !sizeCodeHash.ContainsKey(llvop.NodeProfile.ColorOrSizeCodeRID))
                    {
                        continue;
                    }

                    rows = dt.Select("SIZE_HN_RID = " + llvop.NodeProfile.Key.ToString(CultureInfo.CurrentUICulture));

                    sizeKeyList.Add(new SizeAggregateBasisKey(
                        Convert.ToInt32(rows[0]["STYLE_HN_RID"]), 
                        Convert.ToInt32(rows[0]["COLOR_HN_RID"]), 
                        Convert.ToInt32(rows[0]["COLOR_CODE_RID"]), 
                        Convert.ToInt32(rows[0]["SIZE_HN_RID"]), 
                        Convert.ToInt32(rows[0]["SIZE_CODE_RID"]),
                        Include.ConvertCharToBool(Convert.ToChar(rows[0]["ACTIVE_IND"], CultureInfo.CurrentUICulture))
                        ));
                }
                // End TT#2231

				return sizeKeyList;
			}
			catch
			{
				throw;
			}
		}

		//End TT#155 - JScott - Size Curve e Curve Method
		/// <summary>
		/// Requests the session check to determine if a color is already assigned to a style
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the style</param>
		/// <param name="colorCode">The code associated with the color</param>
		/// <param name="colorNodeRID">The record ID of the color node if it wxists</param>
		/// <returns>A flag identifying if the color is already defined to the style</returns>
		public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, ref int colorNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, null, ref colorNodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session check to determine if a color is already assigned to a style
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the style</param>
		/// <param name="colorCode">The code associated with the color</param>
		/// <param name="aQualifiedNodeID">The fully qualified node ID including parents</param>
		/// <param name="colorNodeRID">The record ID of the color node if it wxists</param>
		/// <returns>A flag identifying if the color is already defined to the style</returns>
		public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, aQualifiedNodeID, ref colorNodeRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2763 - JSmith - Hierarchy Color descriptions not updating
        /// <summary>
        /// Requests the session check to determine if a color is already assigned to a style
        /// </summary>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="nodeRID">The record id of the style</param>
        /// <param name="colorCode">The code associated with the color</param>
        /// <param name="aQualifiedNodeID">The fully qualified node ID including parents</param>
        /// <param name="colorNodeRID">Output parameter containing the record ID of the color node if it exists</param>
        /// <param name="aColorDescription">Output parameter containing the color description</param>
        /// <returns>A flag identifying if the color is already defined to the style</returns>
        public bool ColorExistsForStyle(int hierarchyRID, int nodeRID, string colorCode, string aQualifiedNodeID, ref int colorNodeRID, out string aColorDescription)
        {
            try
            {
                return HierarchyServerGlobal.ColorExistsForStyle(hierarchyRID, nodeRID, colorCode, aQualifiedNodeID, ref colorNodeRID, out aColorDescription);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2763 - JSmith - Hierarchy Color descriptions not updating

		/// <summary>
		/// Requests the session check to determine if a size is already assigned to a color
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the color</param>
		/// <param name="sizeCode">The code associated with the size</param>
		public bool SizeExistsForColor(int hierarchyRID, int nodeRID, string sizeCode, ref int sizeNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.SizeExistsForColor(hierarchyRID, nodeRID, sizeCode, null, ref sizeNodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session check to determine if a size is already assigned to a color
		/// </summary>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="nodeRID">The record id of the color</param>
		/// <param name="sizeCode">The code associated with the size</param>
		/// <param name="aQualifiedNodeID">The fully qualified node ID including parents</param>
		/// <returns>A flag identifying if the size is already defined to the color</returns>
		public bool SizeExistsForColor(int hierarchyRID, int nodeRID, string sizeCode, string aQualifiedNodeID, ref int sizeNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.SizeExistsForColor(hierarchyRID, nodeRID, sizeCode, aQualifiedNodeID, ref sizeNodeRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node in its home hierarchy. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the NodeAncestorList class which contains a NodeAncestorProfile for each ancestor node</returns>
		public NodeAncestorList GetNodeAncestorList(int nodeRID)
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				return HierarchyServerGlobal.GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node in the requested hierarchy. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <returns></returns>
		public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeAncestorList(nodeRID, hierarchyRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node in the requested hierarchy. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="aHierarchySearchType">The type of hierarchies to search for ancestors</param>
		/// <returns></returns>
		public NodeAncestorList GetNodeAncestorList(int nodeRID, eHierarchySearchType aHierarchySearchType)
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
				return HierarchyServerGlobal.GetNodeAncestorList(nodeRID, ni.HomeHierarchyRID, aHierarchySearchType, true);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the a list of NodeAncestorProfiles which contain information about
		/// the ancestors of the node in the requested hierarchy. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="hierarchyRID">The record id of the hierarchy</param>
		/// <param name="aHierarchySearchType">The type of hierarchies to search for ancestors</param>
		/// <returns></returns>
		public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, eHierarchySearchType aHierarchySearchType)
		{
			try
			{
				return HierarchyServerGlobal.GetNodeAncestorList(nodeRID, hierarchyRID, aHierarchySearchType, true);
			}
			catch
			{
				throw;
			}
		}

		//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
        // Begin TT#4271 - JSmith - User Release Errors Logged 01202015
        /// <summary>
        /// Requests the session get the a list of NodeAncestorProfiles which contain information about
        /// the ancestors of the node in the requested hierarchy. 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="hierarchyRID">The record id of the hierarchy</param>
        /// <param name="aHierarchySearchType">The type of hierarchies to search for ancestors</param>
        /// <param name="UseApplyFrom">Identifies if Node Properties Apply From is to be used</param>
        /// <returns>NodeAncestorList containing ancestors for the requested node</returns>
        public NodeAncestorList GetNodeAncestorList(int nodeRID, int hierarchyRID, eHierarchySearchType aHierarchySearchType, bool UseApplyFrom)
        {
            try
            {
                // Begin TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
				if (aHierarchySearchType == eHierarchySearchType.HomeHierarchyOnly
                    && hierarchyRID == Include.NoRID)
                {
                    NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, false);
                    hierarchyRID = ni.HomeHierarchyRID;
                }
				// End TT#5556 - JSmith - Global Interfaced Article List Edit Button Removed from Node Properties
                return HierarchyServerGlobal.GetNodeAncestorList(nodeRID, hierarchyRID, aHierarchySearchType, UseApplyFrom);
            }
            catch
            {
                throw;
            }
        }
        // End TT#4271 - JSmith - User Release Errors Logged 01202015
		//END TT#3962-VStuart-Dragged Values never allowed to drop-MID

		/// <summary>
		/// Requests the session get the a list of all NodeAncestorList ancestor lists for each reference
		/// of the node in all hierarchies. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of an ArrayList containing a NodeAncestorList class for each reference.</returns>
		public ArrayList GetAllNodeAncestorLists(int aNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetAllNodeAncestorLists(aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Requests the session get the a list of all NodeAncestorProfiles for each reference
		/// of the node in all hierarchies. 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <returns>An instance of an NodeAncestorList containing NodeAncestorProfile objects for each reference.</returns>
		public SortedList GetAllNodeAncestors(int aNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetAllNodeAncestors(aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetAncestorPath(int aNodeRID, int aToHierarchyRID, eLowLevelsType aToLowLevelType, int aToLevelRID, int aToOffset)
		{
			try
			{
				return HierarchyServerGlobal.GetAncestorPath(aNodeRID, aToHierarchyRID, aToLowLevelType, aToLevelRID, aToOffset);
			}
			catch
			{
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		/// <summary>
		/// Requests the session get the a list of NodeDescendantProfile which contain information about
		/// the descendants of the node in its home hierarchy. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <returns>An instance of the NodeDescendantList class which contains a NodeDescendantProfile for each descendant node</returns>
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//		public NodeDescendantList GetNodeDescendantList(int nodeRID)
		public NodeDescendantList GetNodeDescendantList(int nodeRID, eNodeSelectType aNodeSelectType)
//End Track #4037
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, true);
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
//				return HierarchyServerGlobal.GetNodeDescendantList(nodeRID, ni.HomeHierarchyRID);
				return HierarchyServerGlobal.GetNodeDescendantList(nodeRID, ni.HomeHierarchyRID,
					aNodeSelectType);
//End Track #4037
			}
			catch
			{
				throw;
			}
		}

        //Begin Track #5004 - JSmith - Global Unlock
        /// <summary>
        /// Requests the session get the a list of NodeDescendantProfile which contain information about
        /// the descendants of the node in its home hierarchy. 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aNodeSelectType">The type of nodes to select</param>
        /// <param name="aLevelOffset">The offset of the level</param>
        /// <returns>An instance of the NodeDescendantList class which contains a NodeDescendantProfile for each descendant node</returns>
        public NodeDescendantList GetNodeDescendantList(int aNodeRID, eNodeSelectType aNodeSelectType, int aLevelOffset)
        {
            try
            {
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(aNodeRID, true);
                return HierarchyServerGlobal.GetNodeDescendantList(aNodeRID, ni.HomeHierarchyRID,
                    aNodeSelectType, aLevelOffset, aLevelOffset);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get the a list of NodeDescendantProfile which contain information about
        /// the descendants of the node in its home hierarchy. 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aNodeSelectType">The type of nodes to select</param>
        /// <param name="aFromLevelOffset">The offset of the level to begin retrieving descendants</param>
        /// <param name="aToLevelOffset">The offset of the level to end retrieving descendants</param>
        /// <returns>An instance of the NodeDescendantList class which contains a NodeDescendantProfile for each descendant node</returns>
        public NodeDescendantList GetNodeDescendantList(int aNodeRID, eNodeSelectType aNodeSelectType, int aFromLevelOffset,
            int aToLevelOffset)
        {
            try
            {
                NodeInfo ni = HierarchyServerGlobal.GetNodeInfoByRID(aNodeRID, true);
                return HierarchyServerGlobal.GetNodeDescendantList(aNodeRID, ni.HomeHierarchyRID,
                    aNodeSelectType, aFromLevelOffset, aToLevelOffset);
            }
            catch
            {
                throw;
            }
        }
        //End Track #5004

		/// <summary>
		/// Recursively updates the home level of all child nodes 
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aHomeHierarchyRID">The home hierarchy record id of the node</param>
		/// <param name="aHomeLevel">The home level of the node</param>
		public void UpdateHomeLevel(int aNodeRID, int aHomeHierarchyRID, int aHomeLevel)
		{
			bool previousConnection = false;
			try
			{
				if (MHD.ConnectionIsOpen)
				{
					previousConnection = true;
				}
				else
				{
					MHD.OpenUpdateConnection();
				}
				EditRecursiveHomeLevel(aNodeRID, aHomeHierarchyRID, aHomeLevel);
				if (!previousConnection)
				{
					MHD.CommitData();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (MHD.ConnectionIsOpen && !previousConnection)
				{
					MHD.CloseUpdateConnection();
				}
			}
		}

		private void EditRecursiveHomeLevel(int aNodeRID, int aHomeHierarchyRID, int aHomeLevel)
		{
			try
			{
				HierarchyNodeProfile hnp =  GetNodeData(aNodeRID, false);
				if (hnp.HomeHierarchyRID == aHomeHierarchyRID)
				{
					hnp.HomeHierarchyLevel = aHomeLevel;
					hnp.NodeChangeType = eChangeType.update;
					NodeUpdateProfileInfo(hnp);
					NodeDescendantList ndl = GetNodeDescendantList(aNodeRID, eNodeSelectType.All);
					// Check each child recursively.
					foreach (NodeDescendantProfile ndp in ndl)
					{
						EditRecursiveHomeLevel(ndp.Key, aHomeHierarchyRID, aHomeLevel + 1);
					}
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin Track #6318 - JSmith - Tried to drag node with the same name into another user 
        /// <summary>
        /// Recursively updates the home hierarchy of all child nodes 
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aHierarchyProfile">The profile of the home hierarchy record id of the node</param>
        public void UpdateHomeHierarchy(HierarchyNodeProfile aHierarchyNodeProfile, HierarchyProfile aOldHierarchyProfile, HierarchyProfile aNewHierarchyProfile, int aHomeLevel)
        {
            bool previousConnection = false;
            try
            {
                if (MHD.ConnectionIsOpen)
                {
                    previousConnection = true;
                }
                else
                {
                    MHD.OpenUpdateConnection();
                }
                EditRecursiveHomeHierarchy(aHierarchyNodeProfile, aOldHierarchyProfile, aNewHierarchyProfile, aHomeLevel);
                if (!previousConnection)
                {
                    MHD.CommitData();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (MHD.ConnectionIsOpen && !previousConnection)
                {
                    MHD.CloseUpdateConnection();
                }
            }
        }

        private void EditRecursiveHomeHierarchy(HierarchyNodeProfile aHierarchyNodeProfile, HierarchyProfile aOldHierarchyProfile, HierarchyProfile aNewHierarchyProfile, int aHomeLevel)
        {
            HierarchyJoinProfile hjp;
            HierarchyLevelProfile hlp;
            HierarchyNodeList children;

            try
            {
                if (aHierarchyNodeProfile.HomeHierarchyRID == aOldHierarchyProfile.Key)
                {
                    children = GetHierarchyChildren(aHierarchyNodeProfile.HomeHierarchyLevel,
                                aHierarchyNodeProfile.HomeHierarchyRID, aHierarchyNodeProfile.HomeHierarchyRID,
                                aHierarchyNodeProfile.Key, false, eNodeSelectType.All, false);
                    
                    // Check each child recursively.
                    foreach (HierarchyNodeProfile hnp in children)
                    {
                        EditRecursiveHomeHierarchy(hnp, aOldHierarchyProfile, aNewHierarchyProfile, aHomeLevel + 1);
                        hjp = new HierarchyJoinProfile(hnp.Key);
                        hjp.JoinChangeType = eChangeType.update;
                        hjp.OldHierarchyRID = aOldHierarchyProfile.Key;
                        hjp.OldParentRID = aHierarchyNodeProfile.Key;
                        hjp.NewHierarchyRID = aNewHierarchyProfile.Key;
                        hjp.NewParentRID = aHierarchyNodeProfile.Key;
                        if (aNewHierarchyProfile.HierarchyType == eHierarchyType.organizational)
                        {
                            hlp = (HierarchyLevelProfile)aNewHierarchyProfile.HierarchyLevels[aHomeLevel];
                            hjp.LevelType = hlp.LevelType;
                        }
                        JoinUpdate(hjp);
                    }

                    aHierarchyNodeProfile.HomeHierarchyRID = aNewHierarchyProfile.Key;
                    aHierarchyNodeProfile.HomeHierarchyLevel = aHomeLevel;
                    aHierarchyNodeProfile.NodeChangeType = eChangeType.update;
                    NodeUpdateProfileInfo(aHierarchyNodeProfile);
                }
            }
            catch
            {
                throw;
            }
        }
        // End Track #6318

		/// <summary>
		/// Requests the session get the a list of NodeDescendantProfile which contain information about
		/// the descendants at a specified hierarchy level type. 
		/// </summary>
		/// <param name="nodeRID">The record id of the node</param>
		/// <param name="aHierarchyLevelType">The eHierarchyLevelType of descendants to retrieve</param>
		/// <returns>An instance of the NodeDescendantList class which contains a NodeDescendantProfile for each descendant node</returns>
		public NodeDescendantList GetNodeDescendantList(int nodeRID, eHierarchyLevelType aHierarchyLevelType,
			eNodeSelectType aNodeSelectType)
		{
			try
			{
				NodeInfo ni =  HierarchyServerGlobal.GetNodeInfoByRID(nodeRID, true);
				return HierarchyServerGlobal.GetNodeDescendantList(nodeRID, ni.HomeHierarchyRID, 
					aHierarchyLevelType, aNodeSelectType);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        //public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
        //    bool addCharacteristicGroups, bool addCharacteristicValues)
        //public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
        //    bool addCharacteristicGroups, bool addCharacteristicValues, string characteristicDelimiter)
        public bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
            bool addCharacteristicGroups, bool addCharacteristicValues, string characteristicDelimiter, bool useCharacteristicTransaction)
        // End TT#2010
        // End TT#167
		{
			try
			{
				bool errorFound = false;
                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                //HierarchyLoadProcess hlp = new HierarchyLoadProcess(SAB, commitLimit, ref errorFound, 
                //    addCharacteristicGroups, addCharacteristicValues);
                HierarchyLoadProcess hlp = new HierarchyLoadProcess(SAB, commitLimit, ref errorFound,
                    addCharacteristicGroups, addCharacteristicValues, useCharacteristicTransaction);
                // End TT#2010
                // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
                //hlp.ProcessHierarchyFile(fileLocation, delimiter, ref errorFound, exceptionFile);
                hlp.ProcessHierarchyFile(fileLocation, delimiter, characteristicDelimiter, ref errorFound, exceptionFile);
                // End TT#167
				return errorFound;
			}
			catch
			{
				throw;
			}
		}

		public bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile,
			bool addCharacteristicGroups, bool addCharacteristicValues)
		{
			try
			{
				bool errorFound = false;
				XMLHierarchyLoadProcess XMLhlp = new XMLHierarchyLoadProcess(SAB, commitLimit, ref errorFound,
					addCharacteristicGroups, addCharacteristicValues);
				XMLhlp.ProcessHierarchyFile(fileLocation, ref errorFound, exceptionFile);
				return errorFound;
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#3625 - DOConnell - performance issue with Store Eligibility Load

        public bool LoadXMLEligibilityFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                errorFound = false;
                StoreEligibilityCriteriaLoadProcess dpclp = new StoreEligibilityCriteriaLoadProcess(SAB, ref errorFound);
                dpclp.ProcessXMLTransFile(fileLocation, delimiter, ref errorFound);
                return errorFound;
            }
            catch
            {
                throw;
            }
        }

        public bool LoadDelimitedEligibilityFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                errorFound = false;
                StoreEligibilityCriteriaLoadProcess dpclp = new StoreEligibilityCriteriaLoadProcess(SAB, ref errorFound);
                dpclp.ProcessVariableFile(fileLocation, delimiter, ref errorFound);
                return errorFound;
            }
            catch
            {
                throw;
            }
        }

        public bool LoadDelimitedIMOFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                errorFound = false;
                VSWCriteriaLoadProcess dpclp = new VSWCriteriaLoadProcess(SAB, ref errorFound);
                dpclp.ProcessVariableFile(fileLocation, delimiter, ref errorFound);
                return errorFound;
            }
            catch
            {
                throw;
            }
        }

        public bool LoadXML_IMOFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, ref bool errorFound)
        {
            try
            {
                errorFound = false;
                VSWCriteriaLoadProcess dpclp = new VSWCriteriaLoadProcess(SAB, ref errorFound);
                dpclp.ProcessXMLTransFile(fileLocation, delimiter, ref errorFound);
                return errorFound;
            }
            catch
            {
                throw;
            }
        }
        //END TT#3625 - DOConnell - performance issue with Store Eligibility Load

        // Begin TT#460 - JSmith - Add size daily blob tables to Purge
        //public bool BuildPurgeDates()
        //{
        //    try
        //    {
        //        return HierarchyServerGlobal.BuildPurgeDates();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public bool BuildPurgeDates(int aWeeksToKeepDailySizeOnhand)
        {
            try
            {
                return HierarchyServerGlobal.BuildPurgeDates(aWeeksToKeepDailySizeOnhand);
            }
            catch
            {
                throw;
            }
        }
        // End TT#460

		/// <summary>
		/// Requests the session get a product characteristic profile for the given key.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		public ProductCharProfile GetProductCharProfile(int aProductCharRID)
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharProfile(aProductCharRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get a product characteristic profile for the given ID.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		public ProductCharProfile GetProductCharProfile(string aProductCharID)
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharProfile(aProductCharID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#287 - JSmith - Characteristic value not auto-adding
        ///// <summary>
        ///// Requests the session add or update product characteristic information 
        ///// </summary>
        ///// <param name="aProductCharProfile">An instance of the ProductCharProfile class which contains product characteristic information</param>
        //public ProductCharProfile ProductCharUpdate(ProductCharProfile aProductCharProfile)
        //{
        //    // Begin TT#287 - JSmith - Characteristic value not auto-adding 
        //    MerchandiseHierarchyData dataLock = null;
        //    // End TT#287
        //    bool previousConnection = false;
        //    try
        //    {
        //        // Begin TT#287 - JSmith - Characteristic value not auto-adding
        //        // determine if characteristic already added after lock is applied
        //        dataLock = new MerchandiseHierarchyData();
        //        dataLock.OpenUpdateConnection(eLockType.ProductCharacteristic, aProductCharProfile.ProductCharID);

        //        ProductCharProfile productCharProfile = HierarchyServerGlobal.GetProductCharProfile(aProductCharProfile.ProductCharID);
        //        // if already exists, override add to update
        //        if (aProductCharProfile.ProductCharChangeType == eChangeType.add &&
        //            productCharProfile.Key != Include.NoRID)
        //        {
        //            aProductCharProfile.ProductCharChangeType = eChangeType.update;
        //            aProductCharProfile.Key = productCharProfile.Key;
        //        }
        //        foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
        //        {
        //            if (pcvp.ProductCharValueChangeType == eChangeType.add)
        //            {
        //                foreach (ProductCharValueProfile pcvp2 in productCharProfile.ProductCharValues)
        //                {
        //                    if (pcvp.ProductCharValue == pcvp2.ProductCharValue)
        //                    {
        //                        pcvp.ProductCharValueChangeType = eChangeType.update;
        //                        pcvp.Key = pcvp2.Key;
        //                        pcvp.ProductCharRID = pcvp2.ProductCharRID;
        //                    }
        //                }
        //            }
        //        }
        //        // End TT#287

        //        if (_mhd.ConnectionIsOpen)
        //        {
        //            previousConnection = true;
        //        }
        //        switch (aProductCharProfile.ProductCharChangeType)
        //        {
        //            case eChangeType.none:
        //                {
        //                    break;
        //                }
        //            case eChangeType.add:
        //                {
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.OpenUpdateConnection();
        //                    }
        //                    aProductCharProfile.Key = _mhd.Hierarchy_CharGroup_Add(aProductCharProfile.ProductCharID);
        //                    foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
        //                    {
        //                        if (pcvp.ProductCharValueChangeType != eChangeType.delete)
        //                        {
        //                            pcvp.Key = _mhd.Hierarchy_Char_Add(aProductCharProfile.Key, pcvp.ProductCharValue);
        //                        }
        //                    }
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.CommitData();
        //                        _mhd.CloseUpdateConnection();
        //                    }
        //                    break;
        //                }
        //            case eChangeType.update:
        //                {
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.OpenUpdateConnection();
        //                    }
        //                    _mhd.Hierarchy_CharGroup_Update(aProductCharProfile.Key, aProductCharProfile.ProductCharID);
        //                    foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
        //                    {
        //                        switch (pcvp.ProductCharValueChangeType)
        //                        {
        //                            case eChangeType.none:
        //                                {
        //                                    break;
        //                                }
        //                            case eChangeType.add:
        //                                {
        //                                    pcvp.Key = _mhd.Hierarchy_Char_Add(aProductCharProfile.Key, pcvp.ProductCharValue);
        //                                    break;
        //                                }
        //                            case eChangeType.update:
        //                                {
        //                                    _mhd.Hierarchy_Char_Update(pcvp.Key, pcvp.ProductCharValue);
        //                                    break;
        //                                }
        //                            case eChangeType.delete:
        //                                {
        //                                    _mhd.Hierarchy_Char_Delete(pcvp.Key);
        //                                    break;
        //                                }
        //                        }

        //                    }
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.CommitData();
        //                        _mhd.CloseUpdateConnection();
        //                    }

        //                    break;
        //                }
        //            case eChangeType.delete:
        //                {
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.OpenUpdateConnection();
        //                    }
        //                    foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
        //                    {
        //                        _mhd.Hierarchy_Char_Delete(pcvp.Key);
        //                    }
        //                    _mhd.Hierarchy_CharGroup_Delete(aProductCharProfile.Key);
        //                    if (!previousConnection)
        //                    {
        //                        _mhd.CommitData();
        //                        _mhd.CloseUpdateConnection();
        //                    }

        //                    break;
        //                }
        //        }
        //        HierarchyServerGlobal.ProductCharUpdate(aProductCharProfile);
        //        return aProductCharProfile;
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.ToString();
        //        throw;
        //    }
        //    finally
        //    {
        //        if (!previousConnection &&
        //            _mhd.ConnectionIsOpen)
        //        {
        //            _mhd.CloseUpdateConnection();
        //        }

        //        // Begin TT#287 - JSmith - Characteristic value not auto-adding
        //        // Ensure that the lock is released.
        //        if (dataLock != null && dataLock.ConnectionIsOpen)
        //        {
        //            dataLock.CloseUpdateConnection();
        //        }
        //        // End TT#287
        //    }
        //}

		/// <summary>
		/// Requests the session add or update product characteristic information 
		/// </summary>
		/// <param name="aProductCharProfile">An instance of the ProductCharProfile class which contains product characteristic information</param>
        public ProductCharProfile ProductCharUpdate(ProductCharProfile aProductCharProfile, bool isReloadAllValues)  // TT#3558 - JSmith - Perf of Hierarchy Load
		{
            MerchandiseHierarchyData dataLock = null;
            MerchandiseHierarchyData mhd = null;
			try
			{
                mhd = new MerchandiseHierarchyData();
                // determine if characteristic already added after lock is applied
                dataLock = new MerchandiseHierarchyData();
                dataLock.OpenUpdateConnection(eLockType.ProductCharacteristic, aProductCharProfile.ProductCharID);

                ProductCharProfile productCharProfile = HierarchyServerGlobal.GetProductCharProfile(aProductCharProfile.ProductCharID);
                // if already exists, override add to update
                if (aProductCharProfile.ProductCharChangeType == eChangeType.add &&
                    productCharProfile.Key != Include.NoRID)
                {
                    aProductCharProfile.ProductCharChangeType = eChangeType.update;
                    aProductCharProfile.Key = productCharProfile.Key;
                }
                foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
                {
                    if (pcvp.ProductCharValueChangeType == eChangeType.add)
                    {
                        foreach (ProductCharValueProfile pcvp2 in productCharProfile.ProductCharValues)
                        {
                            if (pcvp.ProductCharValue == pcvp2.ProductCharValue)
                            {
                                pcvp.ProductCharValueChangeType = eChangeType.update;
                                pcvp.Key = pcvp2.Key;
                                pcvp.ProductCharRID = pcvp2.ProductCharRID;
                            }
                        }
                    }
                }

				switch (aProductCharProfile.ProductCharChangeType)
				{
					case eChangeType.none:
						{
                            //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
                            mhd.OpenUpdateConnection();
                            foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
                            {
                                switch (pcvp.ProductCharValueChangeType)
                                {
                                    case eChangeType.none:
                                        {
                                            break;
                                        }
                                    case eChangeType.add:
                                        {
                                            pcvp.Key = mhd.Hierarchy_Char_Add(aProductCharProfile.Key, pcvp.ProductCharValue);
                                            // Begin TT#2056 - JSmith - Auto added characteristic and values not showing
                                            aProductCharProfile.ProductCharChangeType = eChangeType.update;
                                            // End TT#2056
                                            break;
                                        }
                                    case eChangeType.update:
                                        {
											//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            //mhd.Hierarchy_Char_Update(pcvp.Key, pcvp.ProductCharValue, aProductCharProfile.Key);
											//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            // Begin TT#2056 - JSmith - Auto added characteristic and values not showing
                                            aProductCharProfile.ProductCharChangeType = eChangeType.update;
                                            // End TT#2056
                                            break;
                                        }
                                    case eChangeType.delete:
                                        {
											//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            if (!pcvp.HasBeenMoved)
                                            {
                                                mhd.Hierarchy_Char_Delete(pcvp.Key);
                                            }
											//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            // Begin TT#2056 - JSmith - Auto added characteristic and values not showing
                                            aProductCharProfile.ProductCharChangeType = eChangeType.update;
                                            // End TT#2056
                                            break;
                                        }
                                }

                            }
                            mhd.CommitData();
                            //End TT#1681
							break;
						}
					case eChangeType.add:
						{
                            mhd.OpenUpdateConnection();
							aProductCharProfile.Key = mhd.Hierarchy_CharGroup_Add(aProductCharProfile.ProductCharID);
							foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
							{
								if (pcvp.ProductCharValueChangeType != eChangeType.delete)
								{
									pcvp.Key = mhd.Hierarchy_Char_Add(aProductCharProfile.Key, pcvp.ProductCharValue);
								}
							}
                            mhd.CommitData();
							break;
						}
					case eChangeType.update:
						{
                            mhd.OpenUpdateConnection();
							mhd.Hierarchy_CharGroup_Update(aProductCharProfile.Key, aProductCharProfile.ProductCharID);
                            foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
                            {
                                switch (pcvp.ProductCharValueChangeType)
                                {
                                    case eChangeType.none:
                                        {
                                            break;
                                        }
                                    case eChangeType.add:
                                        {
                                            pcvp.Key = mhd.Hierarchy_Char_Add(aProductCharProfile.Key, pcvp.ProductCharValue);
                                            break;
                                        }
                                    case eChangeType.update:
                                        {
											//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            //mhd.Hierarchy_Char_Update(pcvp.Key, pcvp.ProductCharValue, aProductCharProfile.Key);
											//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            break;
                                        }
                                    case eChangeType.delete:
                                        {
											//BEGIN TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            if (!pcvp.HasBeenMoved)
                                            {
                                                mhd.Hierarchy_Char_Delete(pcvp.Key);
                                            }
											//END TT#3962-VStuart-Dragged Values never allowed to drop-MID
                                            break;
                                        }
                                }

                            }
                            mhd.CommitData();

							break;
						}
					case eChangeType.delete:
						{
                            mhd.OpenUpdateConnection();
							foreach (ProductCharValueProfile pcvp in aProductCharProfile.ProductCharValues)
							{
								mhd.Hierarchy_Char_Delete(pcvp.Key);
							}

                            mhd.Hierarchy_CharGroup_Delete(aProductCharProfile.Key);
                            mhd.CommitData();

							break;
						}
				}
                HierarchyServerGlobal.ProductCharUpdate(aProductCharProfile, isReloadAllValues);  // TT#3558 - JSmith - Perf of Hierarchy Load
				return aProductCharProfile;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
                if (mhd != null &&
                    mhd.ConnectionIsOpen)
                {
                    mhd.CloseUpdateConnection();
                }

                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
			}
		}
        // End TT#287

		/// <summary>
		/// Requests the session get the product characteristics.
		/// </summary>
		/// <returns>
		/// A ProductCharProfileList of ProductCharProfile
		/// </returns>
		public ProductCharProfileList GetProductCharacteristics()
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharacteristics();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the product characteristics assigned to the node.
		/// </summary>
		/// <returns>
		/// A NodeCharProfileList of NodeCharProfile
		/// </returns>
		/// <remarks>
		/// Chases the hierarchy for the characteristics of all ancestors
		/// </remarks>
		public NodeCharProfileList GetProductCharacteristics(int aNodeRID)
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharacteristics(aNodeRID, true);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session get the product characteristics assigned to the node.
		/// </summary>
		/// <param name="aNodeRID">The key of the node</param>
		/// <param name="aChaseHierarchy">
		/// This switch identifies if the routine is to chase the hierarchy
		/// for the characteristics of all ancestors
		/// </param>
		/// <returns>
		/// A NodeCharProfileList of NodeCharProfile
		/// </returns>
		public NodeCharProfileList GetProductCharacteristics(int aNodeRID, bool aChaseHierarchy)
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharacteristics(aNodeRID, aChaseHierarchy);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Requests the session update the product characteristics assigned to the node.
		/// </summary>
		/// <param name="aNodeRID">The key of the node to be updated</param>
		/// <param name="aNodeCharProfileList">
		/// An instance of the NodeCharProfileList class containing instances of the NodeCharProfile class.
		/// </param>
		public void UpdateProductCharacteristics(int aNodeRID, NodeCharProfileList aNodeCharProfileList)
		{
			bool previousConnection = false;
			try
			{
				if (_mhd.ConnectionIsOpen)
				{
					previousConnection = true;
				}
				else
				{
					_mhd.OpenUpdateConnection();
				}
				_mhd.Hier_Char_Join_DeleteAll(aNodeRID);
				foreach (NodeCharProfile ncp in aNodeCharProfileList)
				{
					switch (ncp.ProductCharChangeType)
					{
						case eChangeType.none:
						case eChangeType.add:
						case eChangeType.update:
							{
								_mhd.Hier_Char_Join_Add(aNodeRID, ncp.ProductCharValueRID);
								break;
							}
						case eChangeType.delete:
							{
								break;
							}
					}
				}
				if (!previousConnection)
				{
					_mhd.CommitData();
					_mhd.CloseUpdateConnection();
				}
				HierarchyServerGlobal.UpdateProductCharacteristics(aNodeRID, aNodeCharProfileList);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_mhd.ConnectionIsOpen &&
					!previousConnection)
				{
					_mhd.CloseUpdateConnection();
				}
			}
		}

		/// <summary>
		/// Requests the session get a product characteristic value profile for the given key.
		/// </summary>
		/// <returns>
		/// A ProductCharValueProfile
		/// </returns>
		public ProductCharValueProfile GetProductCharValueProfile(int aProductCharValueRID)
		{
			try
			{
				return HierarchyServerGlobal.GetProductCharValueProfile(aProductCharValueRID);
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
		//// BEGIN Track #4985 - John Smith - Override Models
		//// BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
		////public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
		////    int aLevelOffset, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
		////    bool aMaintainingModels)
		//public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
		//    eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
		//    bool aMaintainingModels)
		//// END Track #6107
		public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
			eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
			bool aMaintainingModels)
		{
			try
			{
                // Begin TT#2231 - JSmith - Size curve build failing
                //return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, eHierarchyDescendantType.levelType, Include.Undefined, aLevelType, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels);
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, eHierarchyDescendantType.levelType, Include.Undefined, aLevelType, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels, false, false);
                // End TT#2281
                // End TT#2231
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
            eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
            bool aMaintainingModels, bool aIgnoreDuplicates)
        {
            try
            {
                return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, eHierarchyDescendantType.levelType, Include.Undefined, aLevelType, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels, aIgnoreDuplicates, false);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2281

		public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
			eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
			bool aMaintainingModels)
		{
			try
			{
                // Begin TT#2231 - JSmith - Size curve build failing
                //return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, aGetByType, aLevel, eHierarchyLevelType.Undefined, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels);
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                //return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, aGetByType, aLevel, eHierarchyLevelType.Undefined, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels, false);
                return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, aGetByType, aLevel, eHierarchyLevelType.Undefined, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels, true, false);
                // End TT#2281
                // End TT#2231
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public LowLevelVersionOverrideProfileList GetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
            eHierarchyDescendantType aGetByType, int aLevel, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
            bool aMaintainingModels, bool aIgnoreDuplicates)
        {
            try
            {
                return lGetOverrideList(aModelRID, aNodeRID, aVersionRID, aGetByType, aLevel, eHierarchyLevelType.Undefined, aHighLevelNodeRID, aIncludeNodeProfile, IncludeUnknownColor, aMaintainingModels, aIgnoreDuplicates, false);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2281
		
        // Begin TT#2231 - JSmith - Size curve build failing
        public LowLevelVersionOverrideProfileList GetOverrideListOfSizes(int aModelRID, int aNodeRID, bool aGetSizeCodeRIDOnly)
        {
            try
            {
                return lGetOverrideList(aModelRID, aNodeRID, Include.NoRID, eHierarchyDescendantType.masterType, Include.Undefined, eHierarchyLevelType.Size, Include.NoRID, true, false, false, false, aGetSizeCodeRIDOnly);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2231

        // Begin TT#2231 - JSmith - Size curve build failing
        //private LowLevelVersionOverrideProfileList lGetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
        //    eHierarchyDescendantType aGetByType, int aLevel, eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
        //    bool aMaintainingModels)
        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        //private LowLevelVersionOverrideProfileList lGetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
        //    eHierarchyDescendantType aGetByType, int aLevel, eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
        //    bool aMaintainingModels, bool aGetSizeCodeRIDOnly)
        private LowLevelVersionOverrideProfileList lGetOverrideList(int aModelRID, int aNodeRID, int aVersionRID,
            eHierarchyDescendantType aGetByType, int aLevel, eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aIncludeNodeProfile, bool IncludeUnknownColor,
            bool aMaintainingModels, bool aIgnoreDuplicates, bool aGetSizeCodeRIDOnly)
        // End TT#2281
        // End TT#2231
		//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
		{
            ModelsData modelsData;
            DataTable dt;
            int nodeRID, versionRID, inheritNodeRID;
            LowLevelVersionOverrideProfileList overrideList;
            ForecastVersionProfileBuilder fvpb;
            VersionProfile vp;
            Dictionary<int, VersionProfile> versionList = new Dictionary<int, VersionProfile>();
            HierarchyNodeProfile hnp;
            Dictionary<int, HierarchyNodeProfile> nodeProfileList = new Dictionary<int, HierarchyNodeProfile>();
            bool found, exclude;

            try
            {
                modelsData = new ModelsData();
                overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                fvpb = new ForecastVersionProfileBuilder();
				//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
				//// BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
				////dt = modelsData.GetOverrides(aModelRID, aNodeRID, aLevelOffset, aHighLevelNodeRID, aMaintainingModels);
				//if (aGetByType == eHierarchyDescendantType.levelType)
				//{
				//    dt = modelsData.GetOverridesByLevel(aModelRID, aNodeRID, aLevel, aHighLevelNodeRID, aMaintainingModels);
				//}
				//else
				//{
				//    dt = modelsData.GetOverridesByOffset(aModelRID, aNodeRID, aLevel, aHighLevelNodeRID, aMaintainingModels);
				//}
				//// END Track #6107
				if (aLevelType == eHierarchyLevelType.Undefined)
				{
					if (aGetByType == eHierarchyDescendantType.levelType)
					{
						dt = modelsData.GetOverridesByLevel(aModelRID, aNodeRID, aLevel, aHighLevelNodeRID, aMaintainingModels);
					}
					else
					{
						dt = modelsData.GetOverridesByOffset(aModelRID, aNodeRID, aLevel, aHighLevelNodeRID, aMaintainingModels);
					}
				}
				else
				{
					dt = modelsData.GetOverridesByType(aModelRID, aNodeRID, aLevelType, aHighLevelNodeRID, aMaintainingModels);
				}
				//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed

                foreach (DataRow dr in dt.Rows)
                {
                    versionRID = aVersionRID;
                    exclude = false;

                    if (dr["HN_RID"] != DBNull.Value)
                    {
                        nodeRID = Convert.ToInt32(dr["HN_RID"]);
                        inheritNodeRID = nodeRID;
                        LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(nodeRID);
                        // Begin TT#2231 - JSmith - Size curve build failing
                        if (aGetSizeCodeRIDOnly)
                        {
                            lvop.NodeProfile = new HierarchyNodeProfile(nodeRID);
                            lvop.NodeProfile.ColorOrSizeCodeRID = Convert.ToInt32(dr["ColorOrSizeCodeRID"]);
                        }
                        else
                        {
                        // End TT#2231
                            if (aIncludeNodeProfile ||
                                !IncludeUnknownColor)
                            {
                                //Begin Track #5569 - JScott - The All Stores and Store Set Low Levels do not appear
                                //lvop.NodeProfile = HierarchyServerGlobal.GetNodeData(nodeRID, false, true);
                                lvop.NodeProfile = HierarchyServerGlobal.GetNodeData(nodeRID, true, true);
                                //End Track #5569 - JScott - The All Stores and Store Set Low Levels do not appear
                                if (!IncludeUnknownColor)
                                {
                                    if (lvop.NodeProfile.LevelType == eHierarchyLevelType.Color)
                                    {
                                        if (lvop.NodeProfile.ColorOrSizeCodeRID == 0)
                                        {
                                            continue;
                                        }
                                    }
                                    else if (lvop.NodeProfile.LevelType == eHierarchyLevelType.Size)
                                    {
                                        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
                                        //Per John Smith -- this was Ok to change to make Fully Qualified consistent
                                        //hnp = HierarchyServerGlobal.GetNodeData(lvop.NodeProfile.HomeHierarchyParentRID, false, false);
                                        hnp = HierarchyServerGlobal.GetNodeData(lvop.NodeProfile.HomeHierarchyParentRID, false, true);
                                        //End TT#155 - JScott - Add Size Curve info to Node Properties
                                        if (hnp.ColorOrSizeCodeRID == 0)
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dr["VERSION_RID"] != DBNull.Value)
                            {
                                if (dr["VERSION_INHERIT_HN_RID"] != DBNull.Value)
                                {
                                    inheritNodeRID = Convert.ToInt32(dr["VERSION_INHERIT_HN_RID"]);
                                    if (inheritNodeRID != nodeRID)
                                    {
                                        lvop.VersionIsOverridden = true;
                                        found = nodeProfileList.TryGetValue(inheritNodeRID, out hnp);
                                        if (!found)
                                        {
                                            hnp = HierarchyServerGlobal.GetNodeData(inheritNodeRID, false, true);
                                            nodeProfileList.Add(hnp.Key, hnp);
                                        }
                                        lvop.VersionOverrideNodeProfile = hnp;
                                    }
                                }

                                versionRID = Convert.ToInt32(dr["VERSION_RID"]);
                            }

                            found = versionList.TryGetValue(versionRID, out vp);
                            if (!found)
                            {
                                vp = fvpb.Build(versionRID);
                                versionList.Add(versionRID, vp);
                            }
                            lvop.VersionProfile = vp;
                        // Begin TT#2231 - JSmith - Size curve build failing
                        }
                        // End TT#2231

                        if (dr["EXCLUDE_IND"] != DBNull.Value)
                        {
                            if (dr["EXCLUDE_IND_INHERIT_HN_RID"] != DBNull.Value)
                            {
                                inheritNodeRID = Convert.ToInt32(dr["EXCLUDE_IND_INHERIT_HN_RID"]);
                                if (inheritNodeRID != nodeRID)
                                {
                                    lvop.ExcludeIsOverridden = true;
                                    found = nodeProfileList.TryGetValue(inheritNodeRID, out hnp);
                                    if (!found)
                                    {
                                        hnp = HierarchyServerGlobal.GetNodeData(inheritNodeRID, false, true);
                                        nodeProfileList.Add(hnp.Key, hnp);
                                    }
                                    lvop.ExcludeOverrideNodeProfile = hnp;
                                }
                            }                  
                  
                            exclude = Include.ConvertCharToBool(Convert.ToChar(dr["EXCLUDE_IND"], CultureInfo.CurrentUICulture));
                        }

                        lvop.Exclude = exclude;

                        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                        if (overrideList.Contains(lvop.Key))
                        {
                            if (aIgnoreDuplicates)
                            {
                                continue;
                            }
                            else
                            {
                                throw new DuplicateOverrideListEntry(lvop.NodeProfile.Text);
                            }
                        }
                        // End TT#2281
                    
                        overrideList.Add(lvop);
                    }

                }

                return overrideList;
            }
            catch
            {
                throw;
            }
        }
        // End Track #4985

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return HierarchyServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD

        #region Apply Changes To Lower Levels (TT#2015)


        #endregion

        #region Chain Set Percent
        //Begin TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3

        /// <summary>
        /// Requests the session add or update store capacity information 
        /// </summary>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="scl">A instance of the StoreCapacityList class which contains StoreCapacityProfile objects for each store</param>
        public int ChainSetPercentUpdate(int nodeRID, ChainSetPercentList scl, int CDR_RID, bool cacheCleared)
        {
            bool updated = false;
            int returnCode = -1;
            try
            {
                _mhd.OpenUpdateConnection();
                _cspcd = new ChainSetPercentCriteriaData();
                _cspcd.OpenUpdateConnection();
                foreach (ChainSetPercentProfiles scp in scl)
                {             
                    switch (scp.ChainSetPercentChangeType)
                    {
                        case eChangeType.none:
                            {
                                break;
                            }
                        case eChangeType.add:
                            {
                                if (!updated)
                                {
                                    //_mhd.ChainSetPercentSet_DeleteAll(scp.NodeRID, scp.TimeID);
                                    updated = true;
                                }
                                if ((Convert.ToString(scp.ChainSetPercent) != string.Empty) && (scp.ChainSetPercent != 0))
                                {
                                    // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                    //returnCode = _mhd.ChainSetPercentSet_Add(nodeRID, scp.StoreGroupID, scp.StoreGroupLevelID, scp.TimeID, scp.ChainSetPercent, _cspcd);
                                    returnCode = _mhd.ChainSetPercentSet_Add(nodeRID, scp.StoreGroupID, scp.StoreGroupLevelID, scp.TimeID, scp.ChainSetPercent, _cspcd, scp.StoreGroupVersion);
                                    // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                }
                                else
                                {
                                    returnCode = _mhd.ChainSetPercentSet_Delete(nodeRID, scp.TimeID, scp.StoreGroupLevelRID);
                                }
                                break;
                            }
                        case eChangeType.update:
                            {
                                if (!updated)
                                {
                                    //_mhd.ChainSetPercentSet_DeleteAll(scp.NodeRID, scp.TimeID);
                                    updated = true;
                                }

                                if ((Convert.ToString(scp.ChainSetPercent) != string.Empty) && (scp.ChainSetPercent != 0))
                                {
                                    // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                    //returnCode = _mhd.ChainSetPercentSet_Add(nodeRID, scp.StoreGroupID, scp.StoreGroupLevelID, scp.TimeID, scp.ChainSetPercent, _cspcd);
                                    returnCode = _mhd.ChainSetPercentSet_Add(nodeRID, scp.StoreGroupID, scp.StoreGroupLevelID, scp.TimeID, scp.ChainSetPercent, _cspcd, scp.StoreGroupVersion);
                                    // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                }

                                else
                                {
                                    returnCode = _mhd.ChainSetPercentSet_Delete(nodeRID, scp.TimeID, scp.StoreGroupLevelRID);
                                }
                                break;
                            }
                        case eChangeType.delete:
                            {
                                if (!updated)
                                {
                                    returnCode = _mhd.ChainSetPercentSet_DeleteAll(nodeRID, scp.TimeID);
                                    updated = true;

                                }
                                break;
                            }
                    }
                }
                _mhd.CommitData();
                _mhd.CloseUpdateConnection();
                _cspcd.CommitData();
                _cspcd.CloseUpdateConnection();

                if (!cacheCleared) HierarchyServerGlobal.ChainSetPercentUpdate(nodeRID, scl); // TT#2015 - gtaylor - apply changes to lower levels
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
                if (_cspcd.ConnectionIsOpen)
                {
                    _cspcd.CloseUpdateConnection();
                }
            }
            return returnCode;
        }

        /// <summary>
        /// Requests the session get the store capacity list for the node.
        /// </summary>
        /// <param name="storeList">The ProfileList of store for which capacities are to be determined</param>
        /// <param name="nodeRID">The record id of the node</param>
        /// <param name="stopOnFind">This switch tells the routine to stop when the first node with capacities is found</param>
        /// <param name="forCopy">A flag identifying if the list is being retrieved to be copied to another node</param>
        /// <returns>An instance of the StoreCapacityList class that contains a StoreCapacityProfile object for each store with a capacity</returns>
        public ChainSetPercentList GetChainSetPercentList(ProfileList storeList, int nodeRID, bool stopOnFind, bool forCopy, bool forAdmin, ProfileList WeekList)
        {
            try
            {
                return HierarchyServerGlobal.GetChainSetPercentList(storeList, nodeRID, stopOnFind, forCopy, forAdmin, SessionAddressBlock, WeekList);
            }
            catch
            {
                throw;
            }
        }

        public ProfileList GetWeeks(int CDR_RID)
        {
            try
            {
                return HierarchyServerGlobal.GetWeeks(CDR_RID, SessionAddressBlock);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteChainSetPercentData(int NodeRID, int TimeID)
        {
            try
            {
                _mhd.OpenUpdateConnection();
                _cspcd = new ChainSetPercentCriteriaData();
                _cspcd.OpenUpdateConnection();
                _mhd.ChainSetPercentSet_DeleteAll(NodeRID, TimeID);
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                if (_mhd.ConnectionIsOpen)
                {
                    _mhd.CloseUpdateConnection();
                }
                if (_cspcd.ConnectionIsOpen)
                {
                    _cspcd.CloseUpdateConnection();
                }
            }
        }

        //End TT#1498 - DOConnell - Chain Plan Set Percentages Phase 3
        #endregion

    }
	//Begin TT#708 - JScott - Services need a Retry availalbe.
}
