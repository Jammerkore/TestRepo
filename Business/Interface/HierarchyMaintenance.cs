using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for HierarchyMaintenance.
	/// </summary>
	public class HierarchyMaintenance
	{
		private string sourceModule = "HierarchyMaintenance.cs";
		HierarchyProfile _hp = null;
		Hashtable _hierarchyLevels = null;
		HierarchyNodeProfile _parent = null;
		bool _hierarchyNotFound = false;
		bool _parentNotFound = false;
		SessionAddressBlock _SAB;
		Session _Session;
		char _nodeDelimiter = '\\';  // delimiter default: resolves to a single backslash
		string _noSecondarySizeStr;
        //string _placeholderStyleLabel;     // TT#2 - RMatelic - Assortment Planning : comment out 
		Hashtable _parentIDHash = null;
		string _parentID = null;
		Hashtable _rejectedHeaders = new Hashtable();
		RollupData _rollupData = new RollupData();
		Hashtable _messages = new Hashtable();
		//Begin Track #4454 - JSmith - OTS Forecast Level reset
		HierarchyProfile _mainHierProf = null;
		//End Track #4454
		NodeCharProfileList _nodeCharProfileList = null;
		HierarchyNodeProfile _currentProductCharNode = null;
        HierarchyNodeProfile _hnp = null;
        // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
        Hashtable _OTSHash = null;
        // End TT#668
        //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
        Dictionary<string, ProductCharProfile> _dctCharacteristics = null;
        //End TT#1681
        Dictionary<string, ProductCharValueProfile> _dctCharacteristicValues = null;  // TT#3558 - JSmith - Perf of Hierarchy Load
        // Begin TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API
        ProfileList _storeList = null;
        Hashtable _stores = null;
        // End TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API

        bool _reclassTextRead = false;
        bool _alternateAPIRolllupExists = false;
        // Begin TT#4053 - JSmith - Hierarchy Timeout
        bool _lookupAlternateAPIRollupExists = false;
        // End TT#4053 - JSmith - Hierarchy Timeout
        string _chainLabel = null;
        string _storeLabel = null;
        string _intransitLabel = null;
        string _OTSForecastMethodLabel = null;
        string _allocationViewSelectLabel = null;
        string _OTSForecastViewSelectLabel = null;
        string _methodOverrideLabel = null;
        string _methodSizeNeedLabel = null;
        string _methodVelocityLabel = null;
        string _methodForecastBalanceLabel = null;
        string _methodGeneralAllocationLabel = null;
        string _methodForecastCopyChainLabel = null;
        string _methodForecastCopyStoreLabel = null;
        //Begin Enhancement - JScott - Export Method - Part 2
        string _methodForecastExportLabel = null;
        //End Enhancement - JScott - Export Method - Part 2
        string _methodForecastSpreadLabel = null;
        string _workspaceFilterLabel = null;
        string _productLabel = null;
        string _methodLabel = null;
        string _workflowLabel = null;
        string _tasklistLabel = null;

        string _rollupMessage = null;
        string _deleteSecurityGroupMessage = null;
        string _deleteSecurityUserMessage = null;
        string _deleteProduct = null;
        string _replaceProduct = null;
        string _deleteMethod = null;
        string _deleteFromWorkflow = null;
        string _deleteFromTasklist = null;
        string _inUseByHeader = null;
        string _headerDeleteFailedFromStatus = null;
        string _headerDeleteForced = null;
        string _headerDelete = null;
        string _deleteConfirmed = null;
        string _deleteFailed = null;
        string _removeConfirmed = null;
        string _moveConfirmed = null;
        string _moveFailed = null;
        string _renameConfirmed = null;
        string _renameFailed = null;
        string _headerDeleteFailed = null;
        string _styleHeaderDeleteFailed = null;
        string _styleHeaderMoveFailed = null;
        //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
        string _invalidParentChild = null;
        //Begin TT#266
        // Begin TT#634 - JSmith - Color rename
        string _headerColorRenameFailed = null;
        string _headerRemovedFromMulti = null;
        string _multiHeaderDeleted = null;
        string _colorRenamed = null;
        string _existingColorUsed = null;
        string _colorAdded = null;
        // End TT#634

        string _actionDelete = null;
        string _actionMove = null;
        string _actionRename = null;

        /// <summary>
        /// Creates a new instance of HierarchyMaintenance
        /// </summary>
        /// <param name="SAB">An instance of the SessionAddressBlock which contains pointers to other objects</param>
        /// <remarks>Defaults to use the ClientServerSession</remarks>
        public HierarchyMaintenance(SessionAddressBlock SAB)
        {
            ConstructorCommon(SAB, SAB.ClientServerSession);
        }

        /// <summary>
        /// Creates a new instance of HierarchyMaintenance
        /// </summary>
        /// <param name="SAB">An instance of the SessionAddressBlock which contains pointers to other objects</param>
        /// <param name="aSession">The session under which the object is instantiated</param>
        public HierarchyMaintenance(SessionAddressBlock SAB, Session aSession)
        {
            ConstructorCommon(SAB, aSession);
        }

        // Begin TT#4053 - JSmith - Hierarchy Timeout
        /// <summary>
        /// Creates a new instance of HierarchyMaintenance
        /// </summary>
        /// <param name="SAB">An instance of the SessionAddressBlock which contains pointers to other objects</param>
        /// <param name="aSession">The session under which the object is instantiated</param>
        /// <param name="aLookupAlternateAPIRollupExists">A flag to request AlternateAPIRollupExists info looked up</param>
        /// <remarks>Defaults to use the ClientServerSession</remarks>
        public HierarchyMaintenance(SessionAddressBlock SAB, Session aSession, bool aLookupAlternateAPIRollupExists)
        {
            _lookupAlternateAPIRollupExists = aLookupAlternateAPIRollupExists;
            ConstructorCommon(SAB, aSession);
        }
        // End TT#4053 - JSmith - Hierarchy Timeout

		/// <summary>
		/// Creates a new instance of HierarchyMaintenance
		/// </summary>
		/// <param name="SAB">An instance of the SessionAddressBlock which contains pointers to other objects</param>
		/// <param name="aSession">The session under which the object is instantiated</param>
		private void ConstructorCommon(SessionAddressBlock SAB, Session aSession)
		{
			_SAB = SAB;
			_Session = aSession;
			_nodeDelimiter = _SAB.HierarchyServerSession.GetProductLevelDelimiter();
			_noSecondarySizeStr = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize);
            //_placeholderStyleLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyleID);     // TT#2 - RMatelic - Assortment Planning : comment out 
			_parentIDHash = new Hashtable();
            // Begin TT#4053 - JSmith - Hierarchy Timeout
            //_alternateAPIRolllupExists = _SAB.HierarchyServerSession.AlternateAPIRollupExists();
            if (_lookupAlternateAPIRollupExists)
            {
                _alternateAPIRolllupExists = _SAB.HierarchyServerSession.AlternateAPIRollupExists();
            }
            // End TT#4053 - JSmith - Hierarchy Timeout
            // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
            _OTSHash = new Hashtable();
            // End TT#668
            //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
            //_dctCharacteristics = new Dictionary<string,ProductCharProfile>();
            ////End TT#1681
            //_dctCharacteristicValues = new Dictionary<string, ProductCharValueProfile>();  // TT#3558 - JSmith - Perf of Hierarchy Load
        }

        // Begin TT#5561 - JSmith - Product characteristic update not applied - xml
        public bool IsCharacteristicsUpdated
        {
            get
            {
                if (_currentProductCharNode != null)
                {
                    return _currentProductCharNode.NodeChangeType == eChangeType.update;
                }
                return false;
            }
        }
        // End TT#5561 - JSmith - Product characteristic update not applied - xml

        private void ReadReclassText()
        {
            try
            {
                _rollupMessage = MIDText.GetText(eMIDTextCode.msg_ReclassRollup);
                _chainLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain);
                _storeLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore);
                _intransitLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit);
                _OTSForecastMethodLabel = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanMethod);
                _allocationViewSelectLabel = MIDText.GetTextOnly(eMIDTextCode.frm_AllocationViewSelection);
                _OTSForecastViewSelectLabel = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection);
                _methodOverrideLabel = MIDText.GetTextOnly(eMIDTextCode.frm_OverrideMethod);
                _methodSizeNeedLabel = MIDText.GetTextOnly(eMIDTextCode.frm_SizeNeedMethod);
                _methodVelocityLabel = MIDText.GetTextOnly(eMIDTextCode.frm_VelocityMethod);
                _methodForecastBalanceLabel = MIDText.GetTextOnly(eMIDTextCode.frm_OTSForecastBalanceMethod);
                _methodGeneralAllocationLabel = MIDText.GetTextOnly(eMIDTextCode.frm_GeneralAllocationMethod);
                _methodForecastCopyChainLabel = MIDText.GetTextOnly(eMIDTextCode.frm_CopyChainForecast);
                _methodForecastCopyStoreLabel = MIDText.GetTextOnly(eMIDTextCode.frm_CopyStoreForecast);
                //Begin Enhancement - JScott - Export Method - Part 2
                _methodForecastExportLabel = MIDText.GetTextOnly(eMIDTextCode.frm_ExportMethod);
                //End Enhancement - JScott - Export Method - Part 2
                _methodForecastSpreadLabel = MIDText.GetTextOnly(eMIDTextCode.frm_ForecastChainSpread);
                _workspaceFilterLabel = MIDText.GetTextOnly(eMIDTextCode.frm_WorkspaceExplorerFilter);
                _productLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Product);
                _methodLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
                _workflowLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);
                _tasklistLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Tasklist);

                _deleteSecurityGroupMessage = MIDText.GetText(eMIDTextCode.msg_ReclassSecurityGroupDelete);
                _deleteSecurityUserMessage = MIDText.GetText(eMIDTextCode.msg_ReclassSecurityUserDelete);
                _deleteProduct = MIDText.GetText(eMIDTextCode.msg_ReclassDeleteProductForUser);
                _replaceProduct = MIDText.GetText(eMIDTextCode.msg_ReclassReplaceProductForUser);
                _deleteMethod = MIDText.GetText(eMIDTextCode.msg_DeleteForUser);
                _deleteFromWorkflow = MIDText.GetText(eMIDTextCode.msg_DeleteFromWorkflow);
                _deleteFromTasklist = MIDText.GetText(eMIDTextCode.msg_DeleteFromTasklist);
                _inUseByHeader = MIDText.GetText(eMIDTextCode.msg_ReclassInUseByHeader);
                _headerDeleteFailedFromStatus = MIDText.GetText(eMIDTextCode.msg_HeaderDeleteFailedByStatus);
                _headerDeleteForced = MIDText.GetText(eMIDTextCode.msg_HeaderDeleteForced);
                _headerDelete = MIDText.GetText(eMIDTextCode.msg_HeaderDelete);
                _deleteConfirmed = MIDText.GetText(eMIDTextCode.msg_ReclassDeleteConfirmed);
                _deleteFailed = MIDText.GetText(eMIDTextCode.msg_ReclassDeleteFailed);
                _removeConfirmed = MIDText.GetText(eMIDTextCode.msg_ReclassRemoveConfirmed);
                _moveConfirmed = MIDText.GetText(eMIDTextCode.msg_ReclassMoveConfirmed);
                _moveFailed = MIDText.GetText(eMIDTextCode.msg_ReclassMoveFailed);
                _renameConfirmed = MIDText.GetText(eMIDTextCode.msg_ReclassRenameConfirmed);
                _renameFailed = MIDText.GetText(eMIDTextCode.msg_ReclassRenameFailed);
                _headerDeleteFailed = MIDText.GetText(eMIDTextCode.msg_HeaderDeleteFailed);
                _styleHeaderDeleteFailed = MIDText.GetText(eMIDTextCode.msg_DeleteFailedFromStyleHeaders);
                _styleHeaderMoveFailed = MIDText.GetText(eMIDTextCode.msg_MoveFailedFromStyleHeaders);
                //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                _invalidParentChild = MIDText.GetText(eMIDTextCode.msg_InvalidParentChild);
                //Begin TT#266
                // Begin TT#634 - JSmith - Color rename
                _headerColorRenameFailed = MIDText.GetText(eMIDTextCode.msg_al_HeaderColorRenameFailed);
                _headerRemovedFromMulti = MIDText.GetText(eMIDTextCode.msg_al_HeaderRemovedFromMultiHeader);
                _multiHeaderDeleted = MIDText.GetText(eMIDTextCode.msg_al_MultiHeaderNoChildrenDeleted);
                _colorRenamed = MIDText.GetText(eMIDTextCode.msg_ColorRenamed);
                _existingColorUsed = MIDText.GetText(eMIDTextCode.msg_ExistingColorUsed);
                _colorAdded = MIDText.GetText(eMIDTextCode.msg_ColorAdded);
                // End TT#634

                _actionDelete = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Delete);
                _actionMove = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Move);
                _actionRename = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Rename);
                _reclassTextRead = true;
            }
            catch
            {
                throw;
            }
        }

        private string GetMessage(eMIDTextCode aMIDTextCode)
        {
            try
            {
                string message = (string)_messages[aMIDTextCode];
                if (message == null)
                {
                    message = MIDText.GetText(aMIDTextCode);
                    _messages.Add(aMIDTextCode, message);
                }
                return (string)message.Clone();
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Use to request changes to an existing hierarchy
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hp">An instance of the HierarchyProfile class containing information about the hierarchy</param>
        public void ProcessHierarchyData(ref EditMsgs em, HierarchyProfile hp)
        {

            try
            {
                switch (hp.HierarchyChangeType)
                {
                    case eChangeType.add:
                        {
                            _hp = _SAB.HierarchyServerSession.GetHierarchyData(hp.Key);
                            if (_hp.Key != -1) // hierarchy already exists
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyAlreadyExists, sourceModule);
                            }
                            else
                            {
                                if (hp.HierarchyColor == null)  // use default if color not provided
                                {
                                    hp.HierarchyColor = Include.MIDDefaultColor;
                                }

                                if (!em.ErrorFound)
                                {
                                    _hp.HierarchyID = hp.HierarchyID;
                                    _hp.HierarchyColor = hp.HierarchyColor;
                                    _hp.HierarchyType = hp.HierarchyType;
                                    _hp.HierarchyChangeType = hp.HierarchyChangeType;
                                    _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                                }
                            }
                            break;
                        }
                    case eChangeType.update:
                        {
                            _hp = _SAB.HierarchyServerSession.GetHierarchyData(hp.Key);
                            if (_hp.Key == -1) // hierarchy does not exist
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                            }
                            else
                            {
                                if (hp.HierarchyColor == null)  //  do not change color if not included in request
                                {
                                    hp.HierarchyColor = _hp.HierarchyColor;
                                }

                                if (!em.ErrorFound)
                                {
                                    _hp.HierarchyID = hp.HierarchyID;
                                    _hp.HierarchyColor = hp.HierarchyColor;
                                    _hp.HierarchyType = hp.HierarchyType;
                                    _hp.HierarchyChangeType = hp.HierarchyChangeType;
                                    _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                                }
                            }
                            break;
                        }
                    case eChangeType.delete:
                        {
                            _hp = _SAB.HierarchyServerSession.GetHierarchyData(hp.Key);
                            if (_hp.Key == -1) // hierarchy does not exist
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                            }
                            else
                            {
                                _hp.HierarchyChangeType = hp.HierarchyChangeType;
                                _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                            }
                            break;
                        }
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    case eChangeType.markedForDelete:  
                        {
                            _hp = _SAB.HierarchyServerSession.GetHierarchyData(hp.Key);
                            if (_hp.Key == -1) // hierarchy does not exist
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                            }
                            else
                            {
                                int attempt = 0;
                                int key = 0;
                                HierarchyProfile hp_check;
                                while (key > Include.NoRID)
                                {
                                    string aNewName = _hp.HierarchyID + "#Del" + attempt + "#";
                                    hp_check = _SAB.HierarchyServerSession.GetHierarchyData(aNewName);
                                    key = hp_check.Key;
                                    if (key == Include.NoRID)
                                    {
                                        _hp.HierarchyID = aNewName;
                                    }
                                    else
                                    {
                                        ++attempt;
                                    }
                                }

                                _hp.HierarchyChangeType = hp.HierarchyChangeType;
                                _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                            }
                            break;
                        }
                    // End TT#3630 - JSmith - Delete My Hierarchy
                }
            }
            catch (DatabaseForeignKeyViolation err)
            {
                if (hp != null && hp.HierarchyChangeType == eChangeType.delete)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailedDataInUse, err.Message, sourceModule);
                }
                else
                {
                    em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                }
                throw;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Use to request changes to a hierarchy
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="hierarchyType">The type of the hierarchy</param>
        /// <param name="hierarchyColor">The color to use for the folder of the hierarchy</param>
        public void ProcessHierarchyData(ref EditMsgs em, string hierarchyID, string hierarchyType, string hierarchyColor)
        {

            try
            {
               
                //trim values to make sure no leading or trailing spaces
                if (hierarchyID != null)
                {
                    hierarchyID = hierarchyID.Trim();
                }
                else
                {
                    hierarchyID = string.Empty;
                }
                if (hierarchyType != null)
                {
                    hierarchyType = hierarchyType.Trim();
                }
                else
                {
                    hierarchyType = string.Empty;
                }
                if (hierarchyColor != null)
                {
                    hierarchyColor = hierarchyColor.Trim();
                }
                else
                {
                    hierarchyColor = string.Empty;
                }

                if (_hp == null || hierarchyID != _hp.HierarchyID)
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(hierarchyID);
                }
                if (_hp.Key == -1) // hierarchy does not exist
                {

                    _hp.HierarchyChangeType = eChangeType.add;
                    em.ChangeType = eChangeType.add;
                }
                else
                {
                    _hp.HierarchyChangeType = eChangeType.update;
                    em.ChangeType = eChangeType.update;
                }
                EditHierarchyData(ref em, hierarchyID, hierarchyType, hierarchyColor);

                //Begin Track #4032 - JSmith - hierarchy type changed
                if (_hp.Key == Include.NoRID ||
                    hierarchyColor != Include.MIDDefaultColor)
                {
                    //End Track #4032
                    if (hierarchyColor == null)
                    {
                        hierarchyColor = Include.MIDDefaultColor;
                    }
                    else
                    {
                        _hp.HierarchyColor = hierarchyColor;
                    }
                    //Begin Track #4032 - JSmith - hierarchy type changed
                }
                //End Track #4032
                if (!em.ErrorFound)
                {
                    _hp.HierarchyID = hierarchyID;
					//Begin TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
                    MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                    DataTable dt = mhd.Hierarchy_Read();
                    ArrayList al = new ArrayList();
                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        int HI_RID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
                        string HI_ID = (string)dr["PH_ID"];
                        eHierarchyType HI_TYPE = (eHierarchyType)(Convert.ToInt32(dr["PH_Type"], CultureInfo.CurrentUICulture));
                        if (HI_TYPE == eHierarchyType.organizational)
                        {
                            al.Add(HI_ID);
                        }
                    }
                    if (_hp.HierarchyType == eHierarchyType.organizational)
                    {
                        if (al.Count < 1 || al.Count == 1 && al.Contains(Convert.ToString(_hp.HierarchyID)))
                        {
                            _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                        }
                        else
                        {
                            em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_OneOrganizationalHierarchyAllowed, sourceModule);
                        }
                    }
                    else
                    {
                        _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                    }
					//End TT#1754 - DOConnell - Transactions were incorrectly attempting to load different hierarchy level definitions to an existing hierarchy
                }

            }
            catch (DatabaseForeignKeyViolation err)
            {
                if (_hp != null && _hp.HierarchyChangeType == eChangeType.delete)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailedDataInUse, err.Message, sourceModule);
                }
                else
                {
                    em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                }
                throw;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to edit hierarchy data on a change request
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="hierarchyType">The type of the hierarchy</param>
        /// <param name="hierarchyColor">The color to use for the folder of the hierarchy</param>
        private void EditHierarchyData(ref EditMsgs em, string hierarchyID, string hierarchyType, string hierarchyColor)
        {
            try
            {
                //Begin Track #4032 - JSmith - hierarchy type changed
                if (_hp.Key == Include.NoRID)
                {
                    //End Track #4032
                    switch (hierarchyType.ToUpper(CultureInfo.CurrentUICulture))
                    {
                        case "ORGANIZATIONAL":
                            {
                                _hp.HierarchyType = eHierarchyType.organizational;
                                break;
                            }
                        case "OPEN":
                            {
                                _hp.HierarchyType = eHierarchyType.alternate;
                                break;
                            }
                        default:
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidHierarchyType, sourceModule);
                                break;
                            }
                    }
                    //Begin Track #4032 - JSmith - hierarchy type changed
                }
                //End Track #4032

            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }

        }

        /// <summary>
        /// Use to request changes to levels in an organizational hierarchy
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="levelID">The ID of the level</param>
        /// <param name="levelType">The type of the level</param>
        /// <param name="OTSPlanLevelType">The type of OTS plan level (regular or total)</param>
        /// <param name="levelLengthType">The type of constraint used for product names at this level</param>
        /// <param name="levelColor">The color to use for nodes at this level in a hierarchy</param>
        /// <param name="requiredSize">The required size of product names at this level if the required size constraint is used</param>
        /// <param name="rangeFrom">The minimum size of product names at this level if the range size constraint is used</param>
        /// <param name="rangeTo">The maximum size of product names at this level if the range size constraint is used</param>
        /// <param name="aIsOTSForecastLevel">A flag identifying that this level is the default OTS Forecast level</param>
        public void ProcessLevelData(ref EditMsgs em, string hierarchyID, int levelSequence, string levelID,
                                string levelType, string OTSPlanLevelType, string levelLengthType, string levelColor,
            //Begin Track #3948 - JSmith - add OTS Forecast Level interface
            int requiredSize, int rangeFrom, int rangeTo, bool aIsOTSForecastLevel)
        //								int requiredSize, int rangeFrom, int rangeTo)
        //End Track #3948
        {
            HierarchyLevelProfile hlp = null;
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (hierarchyID != null)
                {
                    hierarchyID = hierarchyID.Trim();
                }
                else
                {
                    hierarchyID = string.Empty;
                }
                if (levelID != null)
                {
                    levelID = levelID.Trim();
                }
                else
                {
                    levelID = string.Empty;
                }
                if (levelType != null)
                {
                    levelType = levelType.Trim();
                }
                else
                {
                    levelType = string.Empty;
                }
                if (OTSPlanLevelType != null)
                {
                    OTSPlanLevelType = OTSPlanLevelType.Trim();
                }
                else
                {
                    OTSPlanLevelType = string.Empty;
                }
                if (levelLengthType != null)
                {
                    levelLengthType = levelLengthType.Trim();
                }
                else
                {
                    levelLengthType = string.Empty;
                }
                if (levelColor != null)
                {
                    levelColor = levelColor.Trim();
                }
                else
                {
                    levelColor = string.Empty;
                }

                //				if (_hp == null || hierarchyID != _hp.HierarchyID)
                //				{
                _hp = _SAB.HierarchyServerSession.GetHierarchyData(hierarchyID);
                //				}
                if (_hp.Key == -1) // hierarchy does not exist
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                }
                else
                    if (_hp.HierarchyType == eHierarchyType.alternate)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NotAddLevelOpenHierarchy, sourceModule);
                    }
                    else
                    {
                        hlp = new HierarchyLevelProfile(-1);
                        foreach (HierarchyLevelProfile xhlp in _hp.HierarchyLevels.Values)
                        {
                            if (xhlp.LevelID == levelID)
                            {
                                hlp = xhlp;
                                //Begin Track #4032 - JSmith - hierarchy type changed
                                levelSequence = xhlp.Key;
                                //End Track #4032
                                break;
                            }
                        }
                        EditLevelData(ref em, _hp, hlp, levelSequence, levelType, OTSPlanLevelType, levelLengthType, requiredSize, rangeFrom, rangeTo);

                        if (levelColor == null)
                        {
                            levelColor = Include.MIDDefaultColor;
                        }
                        else
                        {
                            hlp.LevelColor = levelColor;
                        }

                        // if level added or moved, clear levels below
                        if (!em.ErrorFound)
                        {
                            if (hlp.Key != levelSequence &&
                                levelSequence < _hp.HierarchyLevels.Count)
                            {
                                int noLevels = _hp.HierarchyLevels.Count;
                                for (int i = hlp.Key + 1; i <= noLevels; i++)
                                {
                                    _hp.HierarchyLevels.Remove(i);
                                }
                            }
                        }
                    }

                if (!em.ErrorFound)
                {
                    int i = 0;
                    bool colorLevelFound = false, styleLevelFound = false, sizeLevelFound = false;
                    foreach (DictionaryEntry lvl in _hp.HierarchyLevels)
                    {
                        int level = (int)lvl.Key;
                        HierarchyLevelProfile xhlp = (HierarchyLevelProfile)lvl.Value;
                        switch (xhlp.LevelType)
                        {
                            case eHierarchyLevelType.Style:
                                styleLevelFound = true;
                                break;
                            case eHierarchyLevelType.Color:
                                colorLevelFound = true;
                                break;
                            case eHierarchyLevelType.Size:
                                sizeLevelFound = true;
                                break;
                        }

                        if (xhlp.LevelID == levelID)
                        {
                            hlp.Level = level;
                            hlp.LevelID = levelID;
							//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							//hlp.LevelNodeCount = xhlp.LevelNodeCount;
							hlp.LevelNodesExist = xhlp.LevelNodesExist;
							//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							hlp.LevelChangeType = eChangeType.update;
                            _hp.HierarchyLevels[level] = hlp;
                            break;
                        }
                        i++;
                    }

                    if (i >= _hp.HierarchyLevels.Count)
                    {
                        hlp.Level = _hp.HierarchyLevels.Count + 1;
                        hlp.LevelChangeType = eChangeType.add;
                        hlp.LevelID = levelID;
                        _hp.HierarchyLevels.Add(hlp.Level, hlp);
                        switch (hlp.LevelType)
                        {
                            case eHierarchyLevelType.Style:
                                styleLevelFound = true;
                                break;
                            case eHierarchyLevelType.Color:
                                colorLevelFound = true;
                                break;
                            case eHierarchyLevelType.Size:
                                sizeLevelFound = true;
                                break;
                        }
                    }

                    if (styleLevelFound && !colorLevelFound)
                    {
                        //  Add color level
                        hlp = new HierarchyLevelProfile(_hp.HierarchyLevels.Count + 1);
                        hlp.Level = _hp.HierarchyLevels.Count + 1;
                        hlp.LevelChangeType = eChangeType.add;
                        hlp.LevelID = "Color";
                        hlp.LevelType = eHierarchyLevelType.Color;
                        hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
                        hlp.LevelLengthType = eLevelLengthType.range;
                        hlp.LevelSizeRangeFrom = 1;
                        hlp.LevelSizeRangeTo = 50;
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//hlp.LevelNodeCount = 0;
						hlp.LevelNodesExist = false;
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						hlp.LevelDisplayOption = eHierarchyDisplayOptions.NameOnly;
                        _hp.HierarchyLevels.Add(hlp.Level, hlp);
                        //  Add size level
                        hlp = new HierarchyLevelProfile(_hp.HierarchyLevels.Count + 1);
                        hlp.Level = _hp.HierarchyLevels.Count + 1;
                        hlp.LevelChangeType = eChangeType.add;
                        hlp.LevelID = "Size";
                        hlp.LevelType = eHierarchyLevelType.Size;
                        hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
                        hlp.LevelLengthType = eLevelLengthType.range;
                        hlp.LevelSizeRangeFrom = 1;
                        hlp.LevelSizeRangeTo = 50;
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//hlp.LevelNodeCount = 0;
						hlp.LevelNodesExist = false;
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						hlp.LevelDisplayOption = eHierarchyDisplayOptions.IdOnly;
                        _hp.HierarchyLevels.Add(hlp.Level, hlp);
                    }
                    else
                        if (colorLevelFound && !sizeLevelFound)
                        {
                            //  Add size level
                            hlp = new HierarchyLevelProfile(_hp.HierarchyLevels.Count + 1);
                            hlp.Level = _hp.HierarchyLevels.Count + 1;
                            hlp.LevelChangeType = eChangeType.add;
                            hlp.LevelID = "Size";
                            hlp.LevelType = eHierarchyLevelType.Size;
                            hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
                            hlp.LevelLengthType = eLevelLengthType.range;
                            hlp.LevelSizeRangeFrom = 1;
                            hlp.LevelSizeRangeTo = 50;
							//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							//hlp.LevelNodeCount = 0;
							hlp.LevelNodesExist = false;
							//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							hlp.LevelDisplayOption = eHierarchyDisplayOptions.IdOnly;
                            _hp.HierarchyLevels.Add(hlp.Level, hlp);
                        }

                    //Begin Track #3948 - JSmith - add OTS Forecast Level interface
                    if (aIsOTSForecastLevel)
                    {
                        _hp.OTSPlanLevelHierarchyLevelSequence = levelSequence;
                    }
                    else if (_hp.OTSPlanLevelHierarchyLevelSequence != Include.Undefined &&
                        _hp.OTSPlanLevelHierarchyLevelSequence == levelSequence)
                    {
                        _hp.OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
                    }
                    //End Track #3948

                    _hp.HierarchyChangeType = eChangeType.update;
                    _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }

        }

        /// <summary>
        /// Used to edit level data on a change request
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hlp">An instance of the HierarchyLevelProfile class containing level information if the level is already defined to the hierarchy</param>
        /// <param name="levelType">The type of the level</param>
        /// <param name="OTSPlanLevelType">The type of OTS plan level (regular or total)</param>
        /// <param name="levelLengthType">The type of constraint used for product names at this level</param>
        /// <param name="requiredSize">The required size of product names at this level if the required size constraint is used</param>
        /// <param name="rangeFrom">The minimum size of product names at this level if the range size constraint is used</param>
        /// <param name="rangeTo">The maximum size of product names at this level if the range size constraint is used</param>
        private void EditLevelData(ref EditMsgs em, HierarchyProfile hp, HierarchyLevelProfile hlp, int levelSequence, string levelType, string OTSPlanLevelType,
            string levelLengthType, int requiredSize, int rangeFrom, int rangeTo)
        {
            try
            {
                switch (levelType.ToUpper(CultureInfo.CurrentUICulture))
                {
                    case "UNDEFINED":
                        {
                            hlp.LevelType = eHierarchyLevelType.Undefined;
                            break;
                        }
                    //Begin Track #3863 - JScott - OTS Forecast Level Defaults
                    //					case "PLANLEVEL": 
                    //					{
                    //						hlp.LevelType = eHierarchyLevelType.Planlevel;
                    //						break;
                    //					}
                    //End Track #3863 - JScott - OTS Forecast Level Defaults
                    case "STYLE":
                        {
                            hlp.LevelType = eHierarchyLevelType.Style;
                            break;
                        }
                    case "COLOR":
                        {
                            hlp.LevelType = eHierarchyLevelType.Color;
                            break;
                        }
                    case "SIZE":
                        {
                            hlp.LevelType = eHierarchyLevelType.Size;
                            break;
                        }
                    default:
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelType, sourceModule);
                            break;
                        }
                }

                switch (levelLengthType.ToUpper(CultureInfo.CurrentUICulture))
                {
                    case "UNRESTRICTED":
                        {
                            hlp.LevelLengthType = eLevelLengthType.unrestricted;
                            hlp.LevelRequiredSize = 0;
                            hlp.LevelSizeRangeFrom = 0;
                            hlp.LevelSizeRangeTo = 0;
                            break;
                        }
                    case "REQUIRED":
                        {
                            hlp.LevelLengthType = eLevelLengthType.required;
                            if (requiredSize > 0)
                            {
                                hlp.LevelRequiredSize = requiredSize;
                                hlp.LevelSizeRangeFrom = 0;
                                hlp.LevelSizeRangeTo = 0;
                            }
                            else
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_RequiredSizeGTZero, sourceModule);
                            }
                            break;
                        }
                    case "RANGE":
                        {
                            hlp.LevelLengthType = eLevelLengthType.range;
                            if (rangeFrom > 0 && rangeTo >= rangeFrom)
                            {
                                hlp.LevelRequiredSize = 0;
                                hlp.LevelSizeRangeFrom = rangeFrom;
                                hlp.LevelSizeRangeTo = rangeTo;
                            }
                            else
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelRange, sourceModule);
                            }
                            break;
                        }
                    default:
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                            break;
                        }
                }

                switch (OTSPlanLevelType.ToUpper(CultureInfo.CurrentUICulture))
                {
                    case "REGULAR":
                        {
                            hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Regular;
                            break;
                        }
                    case "TOTAL":
                        {
                            hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Total;
                            break;
                        }
                    case "UNDEFINED":
                        {
                            hlp.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
                            break;
                        }
                    default:
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidOTSPlanLeveltype, sourceModule);
                            break;
                        }
                }

                // override the unlimited level length of color or size to range
                if ((hlp.LevelType == eHierarchyLevelType.Color ||
                    hlp.LevelType == eHierarchyLevelType.Size) &&
                    hlp.LevelLengthType == eLevelLengthType.unrestricted)
                {
                    hlp.LevelLengthType = eLevelLengthType.range;
                    hlp.LevelSizeRangeFrom = 1;
                    hlp.LevelSizeRangeTo = 50;
                }

                if (hlp.Key != levelSequence)
                {
                    // inserting new level
                    if (hlp.Key == Include.NoRID)
                    {
                        if (levelSequence < _hp.HierarchyLevels.Count)
                        {
                            HierarchyLevelProfile nexthlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelSequence];
							//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							//if (nexthlp.LevelNodeCount != 0)
							if (nexthlp.LevelNodesExist)
							//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
							{
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NextLevelHasNodes, sourceModule);
                            }
                        }
                    }
                    // moving existing level - can't move if nodes defined
					//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					//else if (hlp.LevelNodeCount != 0)
					else if (hlp.LevelNodesExist)
					//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					{
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CanNotMoveLevelHasNodes, sourceModule);
                    }
                }

            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a nodeID
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        public HierarchyNodeProfile NodeLookup(ref EditMsgs em, string nodeID, bool aProcessingAutoAdd)
        {
            try
            {
                string oParentID = null;
                HierarchyNodeProfile hnp = NodeLookup(ref em, nodeID, aProcessingAutoAdd, false, true, out oParentID);
                return hnp;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a nodeID
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="oParentID">Returns the ID of the parent</param>
        /// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information 
        /// and the ID of the parent of the first node.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        public HierarchyNodeProfile NodeLookup(ref EditMsgs em, string nodeID, out string oParentID, bool aProcessingAutoAdd)
        {
            try
            {
                oParentID = null;
                HierarchyNodeProfile hnp = NodeLookup(ref em, nodeID, aProcessingAutoAdd, true, true, out oParentID);
                return hnp;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a nodeID
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
        /// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        public HierarchyNodeProfile NodeLookup(ref EditMsgs em, string nodeID, bool aProcessingAutoAdd, bool aChaseHierarchy)
        {
            try
            {
                string oParentID = null;
                HierarchyNodeProfile hnp = NodeLookup(ref em, nodeID, aProcessingAutoAdd, false, aChaseHierarchy, out oParentID);
                return hnp;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a nodeID
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="oParentID">Returns the ID of the parent</param>
        /// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
        /// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information 
        /// and the ID of the parent of the first node.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        public HierarchyNodeProfile NodeLookup(ref EditMsgs em, string nodeID, out string oParentID, bool aProcessingAutoAdd, bool aChaseHierarchy)
        {
            try
            {
                oParentID = null;
                HierarchyNodeProfile hnp = NodeLookup(ref em, nodeID, aProcessingAutoAdd, true, aChaseHierarchy, out oParentID);
                return hnp;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a nodeID
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="aProcessingAutoAdd">This flag identifies if this method is called during autoadd processing</param>
        /// <param name="aLookupParent">A flag identifying if the parentID is to be returned</param>
        /// <param name="aChaseHierarchy">A flag identifying if the hierarchy should be chased for all settings</param>
        /// <param name="oParentID">Returns the ID of the parent</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information 
        /// and the ID of the parent of the first node.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        private HierarchyNodeProfile NodeLookup(ref EditMsgs em, string nodeID, bool aProcessingAutoAdd, bool aLookupParent, bool aChaseHierarchy, out string oParentID)
        {
            HierarchyNodeProfile hnp = new HierarchyNodeProfile(-1);
            oParentID = null;
            try
            {
                return _SAB.HierarchyServerSession.NodeLookup(nodeID, _nodeDelimiter, aProcessingAutoAdd, aLookupParent, aChaseHierarchy, out oParentID);
            }
            catch (FormatInvalidException)
            {
                em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_NodeLookupFormatInvalid, sourceModule);
                return hnp;
            }
            catch (FirstLevelNotStyleException)
            {
                em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_FirstLevelMustBeStyle, sourceModule);
                return hnp;
            }
            catch (ProductNotFoundException)
            {
                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
                return hnp;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Lookup node profile information for a HierarchyNodeProfile
        /// </summary>
        /// <param name="aHierarchyNodeProfile">A HierarchyNodeProfile object containing information about the node</param>
        /// <returns>An instance of the HierarchyNodeProfile class containing the node information 
        /// and the ID of the parent of the first node.</returns>
        /// <remarks>HierarchyNodeProfile Key contains -1 if node is not found</remarks>
        private HierarchyNodeProfile NodeLookup(HierarchyNodeProfile aHierarchyNodeProfile)
        {
            HierarchyNodeProfile hnp = new HierarchyNodeProfile(-1);
            try
            {
                if (_hp == null || aHierarchyNodeProfile.HomeHierarchyRID != _hp.Key)
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HomeHierarchyRID);
                }

                HierarchyNodeProfile parent_hnp = _SAB.HierarchyServerSession.GetNodeData(aHierarchyNodeProfile.HomeHierarchyParentRID);
                HierarchyLevelProfile hlp = null;
                if (parent_hnp.HomeHierarchyType == eHierarchyType.organizational)
                {
                    hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[parent_hnp.HomeHierarchyLevel + 1];
                }
                if (parent_hnp.HomeHierarchyType == eHierarchyType.organizational &&
                    hlp.LevelType == eHierarchyLevelType.Color)
                {
                    int colorNodeRID = -1;
                    if (_SAB.HierarchyServerSession.ColorExistsForStyle(parent_hnp.HomeHierarchyRID, parent_hnp.Key, aHierarchyNodeProfile.NodeID, aHierarchyNodeProfile.QualifiedNodeID, ref colorNodeRID))
                    {
                        hnp = _SAB.HierarchyServerSession.GetNodeData(colorNodeRID);  // color already exists for style so exit
                    }
                }
                else if (parent_hnp.HomeHierarchyType == eHierarchyType.organizational &&
                    hlp.LevelType == eHierarchyLevelType.Size)
                {

                    int sizeNodeRID = -1;
                    if (_SAB.HierarchyServerSession.SizeExistsForColor(parent_hnp.HomeHierarchyRID, parent_hnp.Key, aHierarchyNodeProfile.NodeID, aHierarchyNodeProfile.QualifiedNodeID, ref sizeNodeRID))
                    {
                        hnp = _SAB.HierarchyServerSession.GetNodeData(sizeNodeRID);  // size already exists for color so exit
                    }
                }
                else
                {
                    hnp = _SAB.HierarchyServerSession.GetNodeData(aHierarchyNodeProfile.NodeID);
                    //					HierarchyNodeProfile child_hnp = _SAB.HierarchyServerSession.GetNodeData(aHierarchyNodeProfile.NodeID);
                    //					if (child_hnp.Key != -1)
                    //					{
                    //						hnp = _SAB.HierarchyServerSession.GetNodeData(child_hnp.Key);  // child already on file so exit
                    //					}
                }
                return hnp;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy using the parent ID.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentID">The ID of the parent where the child is to be added</param>
        /// <param name="child">The child</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, string parentID, string child)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (parentID != null)
                {
                    parentID = parentID.Trim();
                }
                else
                {
                    parentID = string.Empty;
                }
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }

                HierarchyNodeProfile parent_hnp = NodeLookup(ref em, parentID, false, true);
                if (parent_hnp.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                    return -1;
                }
                bool aNodeAdded = true;
                return QuickAdd(ref em, parent_hnp, child, null, null, null, null, null, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy using the parent ID.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentID">The ID of the parent where the child is to be added</param>
        /// <param name="child">The child</param>
        /// <param name="description">The description of the child</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, string parentID, string child, string description)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (parentID != null)
                {
                    parentID = parentID.Trim();
                }
                else
                {
                    parentID = string.Empty;
                }
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }
                if (description != null)
                {
                    description = description.Trim();
                }
                else
                {
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                    //description = string.Empty;
                    description = null;
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                }

                HierarchyNodeProfile parent_hnp = NodeLookup(ref em, parentID, false, true);
                if (parent_hnp.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                    return -1;
                }
                bool aNodeAdded = true;
                return QuickAdd(ref em, parent_hnp, child, description, null, null, null, null, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy using the parent record ID.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentRID">The record ID of the parent where the child is to be added</param>
        /// <param name="child">The child</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, int parentRID, string child)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }

                HierarchyNodeProfile parent_hnp = _SAB.HierarchyServerSession.GetNodeData(parentRID);
                if (parent_hnp.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + parentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
                    return -1;
                }
                bool aNodeAdded = true;
                return QuickAdd(ref em, parent_hnp, child, null, null, null, null, null, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy using the parent record ID.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentRID">The record ID of the parent where the child is to be added</param>
        /// <param name="child">The child</param>
        /// <param name="description">The description of the child</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, int parentRID, string child, string description)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }
                if (description != null)
                {
                    description = description.Trim();
                }
                else
                {
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                    //description = string.Empty;
                    description = null;
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                }

                HierarchyNodeProfile parent_hnp = _SAB.HierarchyServerSession.GetNodeData(parentRID);
                if (parent_hnp.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + parentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
                    return -1;
                }
                bool aNodeAdded = true;
                return QuickAdd(ref em, parent_hnp, child, description, null, null, null, null, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentRID">The record ID of the parent</param>
        /// <param name="child">The child</param>
        /// <param name="sizeProductCategory">The product category of a size that is to be added</param>
        /// <param name="sizePrimary">The primary code of a size that is to be added</param>
        /// <param name="sizeSecondary">The secondary code of a size that is to be added</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, int parentRID, string child,
            string sizeProductCategory, string sizePrimary, string sizeSecondary)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }
                if (sizeProductCategory != null)
                {
                    sizeProductCategory = sizeProductCategory.Trim();
                }
                else
                {
                    sizeProductCategory = string.Empty;
                }
                if (sizePrimary != null)
                {
                    sizePrimary = sizePrimary.Trim();
                }
                else
                {
                    sizePrimary = string.Empty;
                }
                if (sizeSecondary != null)
                {
                    sizeSecondary = sizeSecondary.Trim();
                }
                else
                {
                    sizeSecondary = string.Empty;
                }

                HierarchyNodeProfile parent_hnp = _SAB.HierarchyServerSession.GetNodeData(parentRID);
                if (parent_hnp.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + parentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
                    return -1;
                }
                bool aNodeAdded = true;
                return QuickAdd(ref em, parent_hnp, child, null, null, sizeProductCategory, sizePrimary, sizeSecondary, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parentRID">The record ID of the parent</param>
        /// <param name="child">The child</param>
        /// <param name="aProductDescription">The description for the child</param>
        /// <param name="aProductName">The name for the child</param>
        /// <param name="sizeProductCategory">The product category of a size that is to be added</param>
        /// <param name="sizePrimary">The primary code of a size that is to be added</param>
        /// <param name="sizeSecondary">The secondary code of a size that is to be added</param>
        /// <param name="aNodeAdded">A flag identifying if the node was added</param>
        /// <returns>The record id of the child node</returns>
        public int QuickAdd(ref EditMsgs em, int parentRID, string child, string aProductDescription,
            string aProductName, string sizeProductCategory, string sizePrimary, string sizeSecondary,
            ref bool aNodeAdded)
        {
            try
            {
                //trim values to make sure no leading or trailing spaces
                if (child != null)
                {
                    child = child.Trim();
                }
                else
                {
                    child = string.Empty;
                }
                if (sizeProductCategory != null)
                {
                    sizeProductCategory = sizeProductCategory.Trim();
                }
                else
                {
                    sizeProductCategory = string.Empty;
                }
                if (sizePrimary != null)
                {
                    sizePrimary = sizePrimary.Trim();
                }
                else
                {
                    sizePrimary = string.Empty;
                }
                if (sizeSecondary != null)
                {
                    sizeSecondary = sizeSecondary.Trim();
                }
                else
                {
                    sizeSecondary = string.Empty;
                }

                if (aProductDescription != null)
                {
                    aProductDescription = aProductDescription.Trim();
                }
                else
                {
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                    //aProductDescription = child;
                    aProductDescription = null;
                    // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                }

                if (aProductName != null)
                {
                    aProductName = aProductName.Trim();
                }
                else
                {
                    aProductName = child;
                }

                if (_parent == null || parentRID != _parent.Key)
                {
                    _parent = _SAB.HierarchyServerSession.GetNodeData(parentRID);
                    // begin MID Track # 3818 - set field when parent changes
                    _parentID = _parent.NodeID;
                    // end MID Track # 3818 - set field when parent changes
                }
                if (_parent.Key == -1)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + parentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
                    return -1;
                }
                return QuickAdd(ref em, _parent, child, aProductDescription, aProductName, sizeProductCategory, sizePrimary, sizeSecondary, ref aNodeAdded);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Add a child to an organizational hierarchy.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="parent_hnp">The HierarchyNodeProfile of the parent</param>
        /// <param name="child">The child</param>
        /// <param name="description">The description of the child</param>
        /// <param name="aProductName">The name of the child</param>
        /// <param name="sizeProductCategory">The product category of a size that is to be added</param>
        /// <param name="sizePrimary">The primary code of a size that is to be added</param>
        /// <param name="sizeSecondary">The secondary code of a size that is to be added</param>
        /// <param name="aNodeAdded">A flag identifying if the node was added</param>
        /// <returns>The record id of the child node</returns>
        private int QuickAdd(ref EditMsgs em, HierarchyNodeProfile parent_hnp, string child, string description,
            string aProductName, string sizeProductCategory, string sizePrimary, string sizeSecondary,
            ref bool aNodeAdded)
        {
            try
            {
                // Begin MID Track #4721 - JSmith - Error autoadding alternate node
                HierarchyLevelProfile hlp = null;
                // End MID Track #4721
                aNodeAdded = true;
                string wrkNodeID = null;
                string productDescription = null;
                string productName = null;
                if (description == null || description.Trim().Length == 0)
                {
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                    //if (aProductName == null || aProductName.Trim().Length == 0)
                    //{
                    //    // BEGIN MID Track #3638/ #3724 - DB Error when adding style
                    //    //productDescription = aProductName;
                    //    productDescription = child;
                    //    // END MID Track #3638/ #3724
                    //}
                    //else
                    //{
                    //    // BEGIN MID Track #3638/ #3724 - DB Error when adding style
                    //    //productDescription = child;
                    //    productDescription = aProductName;
                    //    // END MID Track #3638/ #3724
                    //}
                    productDescription = null;
                    // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                }
                else
                {
                    productDescription = description;
                }

                if (aProductName == null || aProductName.Trim().Length == 0)
                {
                    productName = child;
                }
                else
                {
                    productName = aProductName;
                }

                if (_hp == null || parent_hnp.HomeHierarchyRID != _hp.Key)
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(parent_hnp.HomeHierarchyRID);
                }

                //Begin Track #4019 - JSmith - add beyond end of hierarchy
                // Begin TT#611 - JSmith - Auto add to alternate fails
                //if (parent_hnp.HomeHierarchyLevel == _hp.HierarchyLevels.Count)
                if (_hp.HierarchyType == eHierarchyType.organizational &&
                    parent_hnp.HomeHierarchyLevel == _hp.HierarchyLevels.Count)
                // End TT#611
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AddBeyondLevels, sourceModule);
                    aNodeAdded = false;
                    return Include.NoRID;
                }
                //End Track #4019

                HierarchyNodeProfile hnp = new HierarchyNodeProfile(-1);
                // Begin MID Track #4721 - JSmith - Error autoadding alternate node
                if (_hp.HierarchyType == eHierarchyType.organizational)
                {
                    // Begin MID Track #5247 - JSmith - Null reference during auto add
                    if (parent_hnp.HomeHierarchyLevel < _hp.HierarchyLevels.Count)
                    {
                        // End MID Track #5247
                        //				HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[parent_hnp.HomeHierarchyLevel + 1];
                        hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[parent_hnp.HomeHierarchyLevel + 1];
                        // End MID Track #4721
                        if (hlp.LevelType == eHierarchyLevelType.Color)
                        {
                            wrkNodeID = child;
                            ColorCodeProfile ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(child);
                            if (ccp.Key == -1)  // autoadd color code if not defined
                            {
                                if (child.Length > Include.ColorIDMaxSize)
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDTooLarge, sourceModule);
                                }
                                else
                                {
                                    ccp = new ColorCodeProfile(-1);
                                    ccp.ColorCodeChangeType = eChangeType.add;
                                    ccp.ColorCodeID = child;
                                    ccp.ColorCodeName = productName;
                                    ccp.ColorCodeGroup = null;
                                    ccp = ColorCodeUpdate(ref em, ccp);
                                }
                            }
                            else
                            {
                                int colorNodeRID = -1;
                                if (_SAB.HierarchyServerSession.ColorExistsForStyle(parent_hnp.HomeHierarchyRID, parent_hnp.Key, child, parent_hnp.QualifiedNodeID, ref colorNodeRID))
                                {
                                    aNodeAdded = false;
                                    return colorNodeRID;  // color already exists for style so exit
                                }
                            }
                            // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                            if (productDescription == null || productDescription == string.Empty)
                            {
                                productDescription = ccp.ColorCodeName;
                            }
                            // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                        }
                        else
                            if (hlp.LevelType == eHierarchyLevelType.Size)
                            {
                                wrkNodeID = child;
                                SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(child);
                                if (scp.Key == -1)  // autoadd size code if not defined
                                {
                                    if (child.Length > Include.SizeIDMaxSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDTooLarge, sourceModule);
                                    }
                                    else
                                    {
                                        scp.SizeCodeChangeType = eChangeType.add;
                                        scp.SizeCodeID = child;
                                        scp.SizeCodeName = productName;
                                        scp.SizeCodeProductCategory = sizeProductCategory;
                                        scp.SizeCodePrimary = sizePrimary;
                                        scp.SizeCodeSecondary = sizeSecondary;
                                        scp = SizeCodeUpdate(ref em, scp);
                                    }
                                }
                                else
                                {
                                    int sizeNodeRID = -1;
                                    if (_SAB.HierarchyServerSession.SizeExistsForColor(parent_hnp.HomeHierarchyRID, parent_hnp.Key, child, null, ref sizeNodeRID))
                                    {
                                        aNodeAdded = false;
                                        return sizeNodeRID;  // size already exists for color so exit
                                    }
                                }
                                // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                if (productDescription == null || productDescription == string.Empty)
                                {
                                    productDescription = scp.SizeCodeName;
                                }
                                // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                            }
                            else
                            {
                                if (hlp.LevelIDFormat == eHierarchyIDFormat.Unique)
                                {
                                    wrkNodeID = child;
                                }
                                else
                                {
                                    wrkNodeID = parent_hnp.NodeID + child;
                                }
                                HierarchyNodeProfile child_hnp = _SAB.HierarchyServerSession.GetNodeData(wrkNodeID);
                                if (child_hnp.Key != -1)
                                {
                                    aNodeAdded = false;
                                    return child_hnp.Key;  // child already on file so exit
                                }
                                // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                                if (productDescription == null || productDescription == string.Empty)
                                {
                                    productDescription = productName;
                                }
                                // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                            }
                        // Begin MID Track #4721 - JSmith - Error autoadding alternate node
                        // Begin MID Track #5247 - JSmith - Null reference during auto add
                    }
                    else
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AddBeyondLevels, sourceModule);
                    }
                    // End MID Track #5247
                }
                else
                {
                    wrkNodeID = child;
                    HierarchyNodeProfile child_hnp = _SAB.HierarchyServerSession.GetNodeData(wrkNodeID);
                    if (child_hnp.Key != -1)
                    {
                        aNodeAdded = false;
                        return child_hnp.Key;  // child already on file so exit
                    }
                }
                // End MID Track #4721

                if (em.ErrorFound)
                {
                    return Include.NoRID;
                }
                else
                {
                    hnp.NodeChangeType = eChangeType.add;
                    hnp.HierarchyRID = parent_hnp.HomeHierarchyRID;
                    hnp.HomeHierarchyRID = parent_hnp.HomeHierarchyRID;
                    //			hnp.ParentRID = parent_hnp.Key;
                    hnp.HomeHierarchyParentRID = parent_hnp.Key;
                    if (!hnp.Parents.Contains(parent_hnp.Key))
                    {
                        hnp.Parents.Add(parent_hnp.Key);
                    }
                    hnp.NodeID = wrkNodeID;
                    hnp.NodeDescription = productDescription;
                    hnp.NodeName = productName;
                    hnp.ProductType = eProductType.Undefined;
                    //			hnp.LevelType = eHierarchyLevelType.Style;
                    // Begin MID Track #4721 - JSmith - Error autoadding alternate node
                    if (hlp != null)
                    {
                        hnp.LevelType = hlp.LevelType;
                    }
                    else
                    {
                        hnp.LevelType = eHierarchyLevelType.Undefined;
                    }
                    // End MID Track #4721
                    hnp.HomeHierarchyLevel = parent_hnp.HomeHierarchyLevel + 1;
                    hnp.NodeLevel = parent_hnp.HomeHierarchyLevel + 1;
                    // Begin TT#3173 - JSmith - Severe Error during History Load
                    hnp.CommitOnSuccessfulUpdate = true;
                    // End TT#3173 - JSmith - Severe Error during History Load
                    return ProcessNodeProfileInfo(ref em, hnp);
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Copies the node and all properties
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="nodeRID">The record ID of the node to be copied</param>
        /// <param name="parentRID">The record ID of the parent where the node is to be added</param>
        /// <returns></returns>
        public HierarchyNodeProfile CopyNode(ref EditMsgs em, int nodeRID, int parentRID)
        {
            try
            {
                int colorNodeRID = -1;
                //Begin Track #5378 - color and size not qualified
                //				HierarchyNodeProfile hnp =  _SAB.HierarchyServerSession.GetNodeData(nodeRID);
                HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(nodeRID, true, true);
                //End Track #5378
                if (hnp.LevelType == eHierarchyLevelType.Color &&
                    _SAB.HierarchyServerSession.ColorExistsForStyle(hnp.HomeHierarchyRID, parentRID, hnp.NodeID, hnp.QualifiedNodeID, ref colorNodeRID))
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ColorAlreadyInStyle, sourceModule);
                }
                else
                {
                    hnp = _SAB.HierarchyServerSession.CopyNode(nodeRID, parentRID);
                    if (hnp.Key == -1)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
                    }
                }
                return hnp;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Use to process product if no record ids are known.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="parentID">The ID of the parent</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="nodeName">The name of the node</param>
        /// <param name="description">The description of the node</param>
        /// <param name="productType">The type of product (undefined, hardline or softline)</param>
        public void ProcessNodeProfileInfo(ref EditMsgs em, string hierarchyID, string parentID,
            string nodeID, string nodeName, string description, string productType)
        {
            try
            {
                //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                //ProcessNodeProfileInfo(ref em, hierarchyID, parentID,
                //    nodeID, nodeName, description, productType,
                //    //Begin Track #3948 - JSmith - add OTS Forecast Level interface
                //    null, null, null, null, null, null, null, null, null);
                //					null, null, null);
                //End Track #3948
                eChangeType changeType;
                bool blHasChanges;
                ProcessNodeProfileInfo(ref em, hierarchyID, parentID,
                    nodeID, nodeName, description, productType,
                    null, null, null, null, null, null, null, null, null,
                    out changeType, out blHasChanges, false); // TT#3546 - JSmith - Alternate Hierarchy Load Error
                //End TT#106 MD
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Use to process product if no record ids are known.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="parentID">The ID of the parent</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="nodeName">The name of the node</param>
        /// <param name="description">The description of the node</param>
        /// <param name="productType">The type of product (undefined, hardline or softline)</param>
        /// <param name="sizeProductCategory">The product category of a size that is to be added</param>
        /// <param name="sizePrimary">The primary code of a size that is to be added</param>
        /// <param name="sizeSecondary">The secondary code of a size that is to be added</param>
        /// <param name="aOTSForecastLevel">The OTS Forecast Level of the node</param>
        public void ProcessNodeProfileInfo(ref EditMsgs em, string hierarchyID, string parentID,
            string nodeID, string nodeName, string description, string productType,
            //Begin Track #3948 - JSmith - add OTS Forecast Level interface
            string sizeProductCategory, string sizePrimary, string sizeSecondary, string aOTSForecastLevel,
            string aOTSForecastLevelHierarchy, string aOTSForecastLevelSelect,
            string aOTSForecastNodeSearch, string aOTSForecastStartsWith,
            //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
            //string aApplyNodeProperties) // TT#1399
            string aApplyNodeProperties, out eChangeType aChangeType, out bool aProcessRecord, bool aBuildCacheOnly)  // TT#3546 - JSmith - Alternate Hierarchy Load Error
            //End TT#106 MD
        //			string sizeProductCategory, string sizePrimary, string sizeSecondary)
        //End Track #3948
        {
            //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
            aChangeType = eChangeType.none;
            //End TT#106 MD
            HierarchyNodeProfile hnp = null;
            //			MerchandiseHierarchyData mhd = null;
            // Begin TT#1148 - JSmith - Performance
            bool blHasChanges = false;
            aProcessRecord = false;  // TT#3546 - JSmith - Alternate Hierarchy Load Error
            // End TT#1148

            try
            {
                //trim values to make sure no leading or trailing spaces
                if (hierarchyID != null)
                {
                    hierarchyID = hierarchyID.Trim();
                }
                else
                {
                    hierarchyID = string.Empty;
                }
                if (parentID != null)
                {
                    parentID = parentID.Trim();
                }
                else
                {
                    parentID = string.Empty;
                }
                if (nodeID != null)
                {
                    nodeID = nodeID.Trim();
                }
                else
                {
                    nodeID = string.Empty;
                }
                if (nodeName != null)
                {
                    nodeName = nodeName.Trim();
                }
                else
                {
					//BEGIN TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                    //nodeName = string.Empty;
                    nodeName = null;
					//END TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                }
                if (description != null)
                {
                    description = description.Trim();
                }
                else
                {
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                    //description = string.Empty;
                    description = null;
                    // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                }
                if (productType != null)
                {
                    productType = productType.Trim();
                }
                else
                {
                    productType = string.Empty;
                }
                if (sizeProductCategory != null)
                {
                    sizeProductCategory = sizeProductCategory.Trim();
                }
                else
                {
                    sizeProductCategory = string.Empty;
                }
                if (sizePrimary != null)
                {
                    sizePrimary = sizePrimary.Trim();
                }
                else
                {
                    sizePrimary = string.Empty;
                }
                if (sizeSecondary != null)
                {
                    sizeSecondary = sizeSecondary.Trim();
                }
                else
                {
                    sizeSecondary = string.Empty;
                }

                //Begin Track #3948 - JSmith - add OTS Forecast Level interface
                if (aOTSForecastLevel != null)
                {
                    aOTSForecastLevel = aOTSForecastLevel.Trim();
                }
                //End Track #3948

                if (aOTSForecastLevelHierarchy != null)
                {
                    aOTSForecastLevelHierarchy = aOTSForecastLevelHierarchy.Trim();
                }

                if (aOTSForecastLevelSelect != null)
                {
                    aOTSForecastLevelSelect = aOTSForecastLevelSelect.Trim();
                }

                if (aOTSForecastNodeSearch != null)
                {
                    aOTSForecastNodeSearch = aOTSForecastNodeSearch.Trim();
                }

                if (aOTSForecastStartsWith != null)
                {
                    aOTSForecastStartsWith = aOTSForecastStartsWith.Trim();
                }
                //  Begin TT#1399 - Alternate Hierarchy
                if (aApplyNodeProperties != null)
                {
                    aApplyNodeProperties = aApplyNodeProperties.Trim();
                }
                //  End TT#1399
                hnp = new HierarchyNodeProfile(-1);

                // Begin TT#1148 - JSmith - Performance
                //                EditMerchandiseData(ref em, ref hnp, hierarchyID, parentID, nodeID, nodeName, description, productType,
                ////Begin Track #3948 - JSmith - add OTS Forecast Level interface
                //                    sizeProductCategory, sizePrimary, sizeSecondary, aOTSForecastLevel,
                //                    aOTSForecastLevelHierarchy, aOTSForecastLevelSelect, aOTSForecastNodeSearch, aOTSForecastStartsWith);
                //					sizeProductCategory, sizePrimary, sizeSecondary);
                //End Track #3948
                //if (!em.ErrorFound)

                EditMerchandiseData(ref em, ref hnp, hierarchyID, parentID, nodeID, nodeName, description, productType,
                    sizeProductCategory, sizePrimary, sizeSecondary, aOTSForecastLevel,
                    aOTSForecastLevelHierarchy, aOTSForecastLevelSelect, aOTSForecastNodeSearch, aOTSForecastStartsWith, 
                    aApplyNodeProperties,   // TT#1399 - Alternate Hierarchy
                    ref blHasChanges);

                _hnp = hnp;

                // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                //if (!em.ErrorFound &&
                //    blHasChanges)
                if (!em.ErrorFound &&
                    blHasChanges &&
                    !aBuildCacheOnly)
                // End T#3546 - JSmith - Alternate Hierarchy Load Error
                // End TT#1148
                {
                    try
                    {
                        // open connection to lock node for updating
                        //						mhd = new MerchandiseHierarchyData();
                        //						mhd.OpenUpdateConnection(eLockType.HierarchyNode, Include.CreateNodeLockKey(hnp.HomeHierarchyParentRID, nodeID));
                        hnp.QualifiedNodeID = parentID + _nodeDelimiter + nodeID;
                        int colorNodeRID = -1;
                        int sizeNodeRID = -1;
                        // Begin TT#2763 - JSmith - Hierarchy Color descriptions not updating
                        //if (hnp.LevelType == eHierarchyLevelType.Color  // check to see if color is already assigned to the style
                        //    && _SAB.HierarchyServerSession.ColorExistsForStyle(hnp.HierarchyRID, hnp.HomeHierarchyParentRID, hnp.NodeID, hnp.QualifiedNodeID, ref colorNodeRID))
                        //{
                        //    // Don't do anything. Color already in style.
                        //    // Begin TT#1148 - JSmith - Performance
                        //    // do not allow updates to color or size through hierarchy load
                        //    //hnp.NodeChangeType = eChangeType.update;
                        //    //em.ChangeType = eChangeType.update;
                        //    //_SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                        //    // End TT#1148
                        //}
                        string aColorDescription;
                        if (hnp.LevelType == eHierarchyLevelType.Color  // check to see if color is already assigned to the style
                            && _SAB.HierarchyServerSession.ColorExistsForStyle(hnp.HierarchyRID, hnp.HomeHierarchyParentRID, hnp.NodeID, hnp.QualifiedNodeID, ref colorNodeRID, out aColorDescription))
                        {
                            if (description != null &&
                                description != aColorDescription)
                            {
                                hnp = NodeLookup(ref em, parentID + _nodeDelimiter + nodeID, true, false);
                                hnp.NodeChangeType = eChangeType.update;
                                hnp.NodeDescription = description;
                                em.ChangeType = eChangeType.update;
                                _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                            }
                        }
                        // End TT#2763 - JSmith - Hierarchy Color descriptions not updating
                        else
                            if (hnp.LevelType == eHierarchyLevelType.Size  // check to see if color is already assigned to the style
                            && _SAB.HierarchyServerSession.SizeExistsForColor(hnp.HierarchyRID, hnp.HomeHierarchyParentRID, hnp.NodeID, hnp.QualifiedNodeID, ref sizeNodeRID))
                            {
                                // Don't do anything. Size already in color.
                            }
                            else
                                if (hnp.Key == -1)
                                {
                                    hnp.NodeChangeType = eChangeType.add;
                                    em.ChangeType = eChangeType.add;
                                    // Begin TT#1148 - JSmith - Performance
                                    //_SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                    hnp = _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                    // End TT#1148
                                }
                                else
                                    if (hnp.HomeHierarchyRID != _hp.Key) //  add relationship
                                    {
                                        HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                                        hjp.JoinChangeType = eChangeType.add;
                                        hjp.NewHierarchyRID = _hp.Key;
                                        hjp.NewParentRID = _parent.Key;
                                        hjp.Key = hnp.Key;
                                        if (_hp.HierarchyType == eHierarchyType.organizational)
                                        {
                                            HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[hnp.NodeLevel];
                                            hjp.LevelType = hlp.LevelType;
                                        }
                                        if (!_SAB.HierarchyServerSession.JoinExists(hjp))
                                        {
                                            _SAB.HierarchyServerSession.JoinUpdate(hjp);
                                            em.ChangeType = eChangeType.add;
                                        }
                                    }
                                    else
                                    {
                                        hnp.NodeChangeType = eChangeType.update;
                                        em.ChangeType = eChangeType.update;
                                        _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                        //Begin Track #4294 - JSmith -  reattach orphaned nodes
                                        if (hnp.HomeHierarchyRID != Include.NoRID &&
                                            hnp.HomeHierarchyParentRID == Include.NoRID &&
                                            hnp.HomeHierarchyLevel > 0)
                                        {
                                            HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                                            hjp.JoinChangeType = eChangeType.add;
                                            hjp.NewHierarchyRID = _hp.Key;
                                            hjp.NewParentRID = _parent.Key;
                                            hjp.Key = hnp.Key;
                                            if (_hp.HierarchyType == eHierarchyType.organizational)
                                            {
                                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[hnp.NodeLevel];
                                                hjp.LevelType = hlp.LevelType;
                                            }
                                            if (!_SAB.HierarchyServerSession.JoinExists(hjp))
                                            {
                                                _SAB.HierarchyServerSession.JoinUpdate(hjp);
                                                em.ChangeType = eChangeType.add;
                                            }
                                        }
                                        //End Track #4294
                                    }

                        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                        if (!em.ErrorFound)
                        {
                            aChangeType = hnp.NodeChangeType;
                        }
                        //End TT#106 MD

                        // check to see if need to auto-add dummy color and size
                        if (_hp.HierarchyType == eHierarchyType.organizational &&
                            hnp.LevelType == eHierarchyLevelType.Size)
                        {
                            AutoAddDummyColorSize(ref em, hnp, hierarchyID, parentID, nodeID, nodeName, description, productType);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        // close connection and release lock
                        //						if (mhd != null &&
                        //							mhd.ConnectionIsOpen)
                        //						{
                        //							mhd.CloseUpdateConnection();
                        //						}
                    }
                }

                // Begin TT#3546 - JSmith - Alternate Hierarchy Load Error
                // if building cache and could not find node, set changes to true so node will be added.
                if (blHasChanges ||
                    (aBuildCacheOnly &&
                    _hnp.Key == Include.NoRID))
                {
                    aProcessRecord = true;
                }
                // End TT#3546 - JSmith - Alternate Hierarchy Load Error
            }
            catch (DatabaseForeignKeyViolation err)
            {
                if (hnp != null && hnp.NodeChangeType == eChangeType.delete)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailedDataInUse, err.Message, sourceModule);
                }
                else
                {
                    em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                }
                throw;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }

        }

        private void AutoAddDummyColorSize(ref EditMsgs em, HierarchyNodeProfile aHnp, string hierarchyID, string parentID,
            string nodeID, string nodeName, string description, string productType)
        {
            // Begin TT#1148 - JSmith - Performance
            bool blHasChanges = true;
            // End TT#1148

            try
            {
                int colorNodeRID = -1;
                int sizeNodeRID = -1;
                // check to see if need to auto-add dummy color and size
                if (aHnp.LevelType == eHierarchyLevelType.Size)
                {
                    string[] fields = MIDstringTools.Split(parentID, _nodeDelimiter, true);
                    HierarchyNodeProfile styleHnp = _SAB.HierarchyServerSession.GetNodeData(fields[0], false);
                    HierarchyNodeProfile dummyColorHnp;
                    // check for dummy color
                    string qualifiedID = fields[0] + _nodeDelimiter + Include.DummyColorID;
                    string dummyParent = qualifiedID; // MID Track 4256 Valid transactions get 50019 Parent Id not found
                    if (!_SAB.HierarchyServerSession.ColorExistsForStyle(styleHnp.HierarchyRID, styleHnp.Key, Include.DummyColorID, qualifiedID, ref colorNodeRID))
                    {
                        dummyColorHnp = new HierarchyNodeProfile(-1);
                        // Begin TT#1148 - JSmith - Performance
                        //                        EditMerchandiseData(ref em, ref dummyColorHnp, hierarchyID, fields[0], Include.DummyColorID, Include.DummyColorID, Include.DummyColorID, productType,
                        ////Begin Track #3948 - JSmith - add OTS Forecast Level interface
                        //                            null, null, null, null, null, null, null, null);
                        //							null, null, null);
                        //End Track #3948
                        EditMerchandiseData(ref em, ref dummyColorHnp, hierarchyID, fields[0], Include.DummyColorID, Include.DummyColorID, Include.DummyColorID, productType,
                            null, null, null, null, null, null, null, null, null, ref blHasChanges);    // TT#1399 - Alternate Hierarchy - added a null for applynodeproperties
                        // End TT#1148

                        if (!em.ErrorFound)
                        {
                            dummyColorHnp.NodeChangeType = eChangeType.add;
                            em.ChangeType = eChangeType.add;
                            dummyColorHnp.QualifiedNodeID = qualifiedID;
                            //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                            dummyColorHnp.IsVirtual = true;
                            //End Track #4037
                            // Begin TT#4434 - JSmith - Error when trying to release - duplicate size
                            dummyColorHnp.CommitOnSuccessfulUpdate = true;
                            // End TT#4434 - JSmith - Error when trying to release - duplicate size
                            dummyColorHnp = _SAB.HierarchyServerSession.NodeUpdateProfileInfo(dummyColorHnp);
                        }
                    }
                    else
                    {
                        dummyColorHnp = _SAB.HierarchyServerSession.GetNodeData(colorNodeRID);
                    }

                    qualifiedID += _nodeDelimiter + nodeID;
                    if (!_SAB.HierarchyServerSession.SizeExistsForColor(dummyColorHnp.HomeHierarchyRID, dummyColorHnp.Key, nodeID, qualifiedID, ref sizeNodeRID))
                    {
                        HierarchyNodeProfile dummyColorSizeHnp = new HierarchyNodeProfile(-1);
                        //string dummyParent = parentID.Replace(fields[1], Include.DummyColorID); // MID Track 4256 Valid transactions get 50019 Parent Id not found
                        // Begin TT#1148 - JSmith - Performance
                        //                        EditMerchandiseData(ref em, ref dummyColorSizeHnp, hierarchyID, dummyParent, nodeID, nodeName, hierarchyID, productType,
                        ////Begin Track #3948 - JSmith - add OTS Forecast Level interface
                        //                            null, null, null, null, null, null, null, null);
                        ////							null, null, null);
                        ////End Track #3948

                        EditMerchandiseData(ref em, ref dummyColorSizeHnp, hierarchyID, dummyParent, nodeID, nodeName, hierarchyID, productType,
                            null, null, null, null, null, null, null, null, null, ref blHasChanges);
                        // End TT#1148

                        if (!em.ErrorFound)
                        {
                            dummyColorSizeHnp.NodeChangeType = eChangeType.add;
                            em.ChangeType = eChangeType.add;
                            dummyColorSizeHnp.QualifiedNodeID = qualifiedID;
                            //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                            dummyColorSizeHnp.IsVirtual = true;
                            //End Track #4037
                            // Begin TT#4434 - JSmith - Error when trying to release - duplicate size
                            dummyColorSizeHnp.CommitOnSuccessfulUpdate = true;
                            // End TT#4434 - JSmith - Error when trying to release - duplicate size
                            _SAB.HierarchyServerSession.NodeUpdateProfileInfo(dummyColorSizeHnp);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Use to process product profile information.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class for errors</param>
        /// <param name="hnp">An instance of the HierarchyNodeProfile class which contains information about the node</param>
        /// <returns>The record id of the node</returns>
        public int ProcessNodeProfileInfo(ref EditMsgs em, HierarchyNodeProfile hnp)
        {
            string message = null;
            // Begin TT#1871 - JSmith - Concurrent header loads experience dead lock
            bool openedUpdateConnection = false;
            MerchandiseHierarchyData dataLock = null;
            // End TT#1871
            try
            {
                // Begin TT#1203 - JSmith - Delete of Key Item Failed
                if (_hp == null ||
                    _hp.Key != hnp.HierarchyRID)
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HierarchyRID);
                }
                // End TT#1203

                if (hnp.NodeChangeType == eChangeType.add ||
                    hnp.NodeChangeType == eChangeType.update)
                {
                    EditMerchandiseData(ref em, ref hnp);
                }

                // Begin TT#3630 - JSmith - Delete My Hierarchy
                //if (hnp.NodeChangeType == eChangeType.delete)
                if (hnp.NodeChangeType == eChangeType.delete ||
                    hnp.NodeChangeType == eChangeType.markedForDelete)
                // End TT#3630 - JSmith - Delete My Hierarchy
                {
                    HierarchyNodeProfile checkExists = _SAB.HierarchyServerSession.GetNodeData(hnp.Key);
                    if (hnp.Key == -1)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
                    }
                    else
                    {
                        if (!_reclassTextRead)
                        {
                            ReadReclassText();
                        }
                        if (DeleteAllowed(ref em, hnp, true))
                        {
                            // Begin TT#3408 - JSmith - PROD Virtual lock \ hung process
                            // Only allow one node delete to occur at a time
                            MerchandiseHierarchyData deleteLock = null;
                            deleteLock = new MerchandiseHierarchyData();
                            try
                            {
                                deleteLock.OpenUpdateConnection(eLockType.HierarchyNode, "HierarchyNodeDelete");
                                // End TT#3408 - JSmith - PROD Virtual lock \ hung process
                                if (DeleteHeaders(ref em, hnp, false, false))
                                {
								    // Begin TT#3630 - JSmith - Delete My Hierarchy
                                    if (hnp.NodeChangeType == eChangeType.markedForDelete)
                                    {
                                        int attempt = 0;
                                        int key = 0;
                                        while (key > Include.NoRID)
                                        {
                                            string aNewName = hnp.NodeID + "#Del" + attempt + "#";
                                            key = _SAB.HierarchyServerSession.GetNodeRID(aNewName);
                                            if (key == Include.NoRID)
                                            {
                                                hnp.NodeID = aNewName;
                                            }
                                            else
                                            {
                                                ++attempt;
                                            }
                                        }
                                    }
									// End TT#3630 - JSmith - Delete My Hierarchy
                                    _SAB.HierarchyServerSession.OpenUpdateConnection();
                                    try
                                    {
                                        hnp = _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                        if (hnp.NodeChangeSuccessful)
                                        {
                                            _SAB.HierarchyServerSession.CommitData();
                                        }
                                        else
                                        {
                                            em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailed, sourceModule);
                                        }
                                    }
                                    catch
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        if (_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                        {
                                            _SAB.HierarchyServerSession.CloseUpdateConnection();
                                        }
                                    }
                                }
                                else
                                {
                                    HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);
                                    HierarchyNodeProfile parent = _SAB.HierarchyServerSession.GetNodeData(hnp.HomeHierarchyParentRID, false);

                                    message = (string)_deleteFailed.Clone();
                                    message = message.Replace("{0}", hnp.Text);
                                    message = message.Replace("{1}", parent.Text);
                                    message = message.Replace("{2}", hp.HierarchyID);
                                    em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                                }
                            // Begin TT#3408 - JSmith - PROD Virtual lock \ hung process
                            }
                            catch
                            {
                                throw;
                            }
                            finally
                            {
                                // Ensure that the lock is released.
                                if (deleteLock != null && deleteLock.ConnectionIsOpen)
                                {
                                    deleteLock.RemoveLocks();
                                    deleteLock.CloseUpdateConnection();
                                }
                            }
                            // End TT#3408 - JSmith - PROD Virtual lock \ hung process
                        }
                    }
                }
                else
                {
                    if (hnp.NodeChangeType == eChangeType.update &&  // this is a hierarchy root node
                        hnp.NodeLevel == 0)
                    {
                        // Begin TT#1203 - JSmith - Delete of Key Item Failed
                        //_hp = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HierarchyRID);
                        // End TT#1203
                        _hp.HierarchyChangeType = eChangeType.update;
                        _hp.HierarchyID = hnp.NodeID;
                        // Begin TT#1911-MD - JSmith - Database Error on Update
                        _hp = _SAB.HierarchyServerSession.HierarchyUpdate(_hp);
                        //_hp = _SAB.HierarchyServerSession.HierarchyUpdate(hp:_hp, updateLevels:false);
                        // End TT#1911-MD - JSmith - Database Error on Update
                        hnp.NodeChangeType = eChangeType.update;
                        em.ChangeType = eChangeType.update;
                        _SAB.HierarchyServerSession.OpenUpdateConnection();
                        try
                        {
                            _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                            _SAB.HierarchyServerSession.CommitData();
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            if (_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                            {
                                _SAB.HierarchyServerSession.CloseUpdateConnection();
                            }
                        }
                    }
                    else
                    {
                        //						EditMerchandiseData(ref em, ref hnp);

                        // Begin TT#1871 - JSmith - Concurrent header loads experience dead lock
                        //bool openedUpdateConnection = false;
                        // End TT#1871
                        if (!em.ErrorFound)
                        {
                            if (!_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                            {
                                _SAB.HierarchyServerSession.OpenUpdateConnection();
                                openedUpdateConnection = true;
                                // Begin TT#1871 - JSmith - Concurrent header loads experience dead lock
                                dataLock = new MerchandiseHierarchyData();
                                dataLock.OpenUpdateConnection(eLockType.HierarchyNode, "##HM##" + hnp.LevelType.ToString() + hnp.NodeID);
                                // End TT#1871
                            }
                            if (hnp.Key == -1)
                            {
                                int nodeRID = _SAB.HierarchyServerSession.GetNodeRID(hnp);
                                if (nodeRID == Include.NoRID)
                                {
                                    hnp.NodeChangeType = eChangeType.add;
                                    em.ChangeType = eChangeType.add;
                                    // Begin TT#2123-MD - JSmith - PH showing in OTS Forecast Reveiw Multi Level down to Size
                                    if (_parent != null
                                        && _parent.IsVirtual)
                                    {
                                        hnp.IsVirtual = true;
                                    }
                                    // End TT#2123-MD - JSmith - PH showing in OTS Forecast Reveiw Multi Level down to Size
                                    hnp = _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                }
                                else
                                {
                                    hnp.Key = nodeRID;
                                }

                            }
                            else
                                if (hnp.HomeHierarchyRID != _hp.Key) //  add relationship
                                {
                                    // make sure node information is up to date
                                    hnp.NodeChangeType = eChangeType.update;
                                    em.ChangeType = eChangeType.update;
                                    _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);

                                    HierarchyJoinProfile hjp = new HierarchyJoinProfile(hnp.Key);
                                    hjp.JoinChangeType = eChangeType.add;
                                    hjp.NewHierarchyRID = _hp.Key;
                                    hjp.NewParentRID = _parent.Key;
                                    hjp.Key = hnp.Key;
                                    if (_hp.HierarchyType == eHierarchyType.organizational)
                                    {
                                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[hnp.NodeLevel];
                                        hjp.LevelType = hlp.LevelType;
                                    }
                                    if (!_SAB.HierarchyServerSession.JoinExists(hjp))
                                    {
                                        _SAB.HierarchyServerSession.JoinUpdate(hjp);
                                        em.ChangeType = eChangeType.add;
                                    }
                                }
                                else
                                {
                                    hnp.NodeChangeType = eChangeType.update;
                                    em.ChangeType = eChangeType.update;
                                    _SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
                                }

                            // check to see if need to auto-add dummy color and size
                            if (hnp.LevelType == eHierarchyLevelType.Size)
                            {
                                HierarchyNodeProfile styleHnp = _SAB.HierarchyServerSession.GetAncestorData(hnp.HomeHierarchyRID, hnp.Key, 2);
                                string hierarchyID = null;
                                if (hnp.HomeHierarchyRID == _hp.Key)
                                {
                                    hierarchyID = _hp.HierarchyID;
                                }
                                else
                                {
                                    hierarchyID = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID).HierarchyID;
                                }
                                string parentID = styleHnp.NodeID + _nodeDelimiter + _SAB.HierarchyServerSession.GetNodeData(hnp.HomeHierarchyParentRID).NodeID;
                                AutoAddDummyColorSize(ref em, hnp, hierarchyID, parentID, hnp.NodeID, hnp.NodeName, hnp.NodeDescription, "UNDEFINED");
                            }

                            // Begin TT#1871 - JSmith - Concurrent header loads experience dead lock
                            //// only commit and close connection if was opened in method
                            //if (openedUpdateConnection)
                            //{
                            //    _SAB.HierarchyServerSession.CommitData();
                            //    _SAB.HierarchyServerSession.CloseUpdateConnection();
                            //}
                            // End TT#1871
                        }
                    }
                }
                return hnp.Key;
            }
            catch (MIDDatabaseUnavailableException err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                _SAB.HierarchyServerSession.CloseUpdateConnection();
                throw;
            }
            catch (DatabaseForeignKeyViolation err)
            {
                if (hnp != null && hnp.NodeChangeType == eChangeType.delete)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailedDataInUse, err.Message, sourceModule);
                }
                else
                {
                    em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                }
                throw;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
            finally
            {
                //				// close connection and release lock
                //				if (mhd != null &&
                //					mhd.ConnectionIsOpen)
                //				{
                //					mhd.CloseUpdateConnection();
                //				}
                // Begin TT#1871 - JSmith - Concurrent header loads experience dead lock
                // only commit and close connection if was opened in method
                if (openedUpdateConnection &&
                    _SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                {
                    _SAB.HierarchyServerSession.CommitData();
                    _SAB.HierarchyServerSession.CloseUpdateConnection();
                }

                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
                // End TT#1871
            }
        }

        /// <summary>
        /// Used to edit node data on a change request
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hnp">An instance of the HierarchyNodeProfile class used to send and receive information about the node</param>
        /// <param name="hierarchyID">The ID of the hierarchy</param>
        /// <param name="parentID">The ID of the parent</param>
        /// <param name="nodeID">The ID of the node</param>
        /// <param name="nodeName">The name of the node</param>
        /// <param name="description">The description of the node</param>
        /// <param name="productType">The type of product (undefined, hardline or softline)</param>
        /// <param name="sizeProductCategory">The product category of a size that is to be added</param>
        /// <param name="sizePrimary">The primary code of a size that is to be added</param>
        /// <param name="sizeSecondary">The secondary code of a size that is to be added</param>
        /// <param name="aOTSForecastLevel">The OTS Forecast Level of the node</param>
        /// <param name="aOTSForecastLevelHierarchy">The OTS Forecast Level hierarchy</param>
        /// <param name="aOTSForecastLevelSelect">The OTS Forecast Level level type</param>
        /// <param name="aOTSForecastNodeSearch">The OTS Forecast Level mask type</param>
        /// <param name="aOTSForecastStartsWith">The OTS Forecast Level node starts with</param>
        private void EditMerchandiseData(ref EditMsgs em, ref HierarchyNodeProfile hnp, string hierarchyID, string parentID,
            string nodeID, string nodeName, string description, string productType,
            //Begin Track #3948 - JSmith - add OTS Forecast Level interface
            string sizeProductCategory, string sizePrimary, string sizeSecondary, string aOTSForecastLevel,
            string aOTSForecastLevelHierarchy, string aOTSForecastLevelSelect,
            // Begin TT#1148 - JSmith - Performance
            //string aOTSForecastNodeSearch, string aOTSForecastStartsWith)
            string aOTSForecastNodeSearch, string aOTSForecastStartsWith, 
            string aApplyNodeProperties,    // TT#1399
            ref bool blHasChanges)
        // End TT#1148
        //			string sizeProductCategory, string sizePrimary, string sizeSecondary)
        //End Track #3948
        {
            // Begin TT#1148 - JSmith - Performance
            HierarchyNodeProfile currentHnp = null;
            blHasChanges = false;
            // End TT#1148

            try
            {
                //Begin Track #4454 - JSmith - OTS Forecast Level reset
                //				// apply defaults
                //				if (aOTSForecastLevelSelect == null)
                //				{
                //					aOTSForecastLevelSelect = "UNDEFINED";
                //				}
                //				if (aOTSForecastNodeSearch == null)
                //				{
                //					aOTSForecastNodeSearch = "UNDEFINED";
                //				}
                //End Track #4454

                HierarchyLevelProfile hlp = null;
                if (_hp == null || hierarchyID != _hp.HierarchyID)
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(hierarchyID);
                    if (_hp.Key == -1)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                        _hierarchyNotFound = true;
                    }
                    else
                    {
                        _hierarchyNotFound = false;
                        _hierarchyLevels = _hp.HierarchyLevels;
                    }
                }
                else
                    if (_hierarchyNotFound)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                    }

                // Begin TT#1148 - JSmith - Performance
                //                hnp = NodeLookup(ref em, nodeID, false, false);
                //                // don't perform parent check if hierarchy root
                //                if (hnp.Key != Include.NoRID && hnp.HomeHierarchyLevel == 0)
                //                {
                //                }
                //                else
                //                {
                //                    if (parentID == null)
                //                    {
                //                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentRequired, sourceModule);
                //                        _parentNotFound = true;
                //                    }
                //                    else if (_parent == null || parentID != _parentID)
                //                    {
                //                        _parent = (HierarchyNodeProfile)_parentIDHash[parentID];
                //                        if (_parent == null)
                //                        {
                //                            _parent = NodeLookup(ref em, parentID, false, false);
                //                            if (_parent.Key != Include.NoRID)
                //                            {
                //                                _parentIDHash[parentID] = _parent;
                //                            }
                //                        }
                //                        _parentID = parentID;
                //                        if (_parent.Key == Include.NoRID)  // parent does not exist
                //                        {
                //                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                //                            _parentNotFound = true;
                //                        }
                //                        else
                //                        {
                //                            _parentNotFound = false; 
                //                        }
                //                    }
                //                    else if (_parentNotFound)
                //                    {
                //                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                //                    }

                //                    if (nodeID != null && parentID != null && _parent.Key != Include.NoRID)  // can't edit child if don't know parent
                //                    {
                //                        // Begin TT#656 - JSmith - Release error
                //                        //if (_parent.LevelType == eHierarchyLevelType.Style ||
                //                        //    _parent.LevelType == eHierarchyLevelType.Color)
                //                        //{
                //                        //    //hnp = new HierarchyNodeProfile(Include.NoRID);
                //                        //    hnp = NodeLookup(ref em, parentID + _nodeDelimiter + nodeID, false, false);
                //                        //}
                //                        if (_parent.LevelType == eHierarchyLevelType.Color)
                //                        {
                //                            hnp = new HierarchyNodeProfile(Include.NoRID);
                //                        }
                //                        else if (_parent.LevelType == eHierarchyLevelType.Style)
                //                        {
                //                            //hnp = new HierarchyNodeProfile(Include.NoRID);
                //                            //hnp = NodeLookup(ref em, parentID + _nodeDelimiter + nodeID, false, false);
                //                            hnp = NodeLookup(ref em, parentID + _nodeDelimiter + nodeID, true, false);
                //                        }
                //                        // End TT#656
                //                        else
                //                        {
                ////							hnp = _SAB.HierarchyServerSession.GetNodeData(nodeID, false);
                //                            if (hnp.Key != Include.NoRID && hnp.NodeLevel > 0)	// check if same parent
                //                            {
                //                                // nodes can only be added in the parent's home hierarchy
                //                                if (_parent.HomeHierarchyRID != _hp.Key)
                //                                {
                //                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NotParentsHomeHierarchy, sourceModule);
                //                                }
                //                                else
                //                                {
                //                                    // see if the node is currently in the hierarchy
                //                                    // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                //                                    //HierarchyNodeProfile currentParent = _SAB.HierarchyServerSession.GetNodeData(_hp.Key, hnp.Key);
                //                                    HierarchyNodeProfile currentParent = _SAB.HierarchyServerSession.GetNodeData(_hp.Key, hnp.Key, false, false);
                //                                    // End TT#668

                //                                    if (currentParent.HomeHierarchyParentRID != Include.NoRID &&
                //                                        currentParent.HomeHierarchyParentRID != _parent.Key)	// different parent 
                //                                    {
                //                                        if (_hp.HierarchyType == eHierarchyType.organizational)
                //                                        {
                //                                            hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[_parent.HomeHierarchyLevel + 1]; 
                //                                            if (hlp.LevelType == eHierarchyLevelType.Color ||
                //                                                hlp.LevelType == eHierarchyLevelType.Size)
                //                                            {
                //                                                // color and size IDs can be duplicated under different parents
                //                                                // reset fields since node that was found is not the right node
                //                                                hnp = new HierarchyNodeProfile(Include.NoRID);
                //                                            }
                //                                            else
                //                                            {
                //                                                string message =  "Hierarchy: " + hierarchyID + "; Parent: " + parentID + "; ID: " + nodeID + "; Name: " + nodeName + "; Description: " + description;
                //                                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                //                                            }
                //                                        }
                //                                        else if (hnp.HomeHierarchyRID != _hp.Key &&
                //                                            _hp.HierarchyType == eHierarchyType.alternate)
                //                                        {
                //                                            // assume adding relationship to alternate hierarchy
                //                                        }
                //                                        else
                //                                        {
                //                                            string message =  "Hierarchy: " + hierarchyID + "; Parent: " + parentID + "; ID: " + nodeID + "; Name: " + nodeName + "; Description: " + description;
                //                                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                            else
                //                                if ((_parent.NodeLevel + 1) != hnp.NodeLevel)	// not the node you think it is. may be color or size with same code as other node
                //                            {
                //                                hnp = new HierarchyNodeProfile(Include.NoRID);	
                //                            }
                //                        }
                //                    }
                //                }

                if (parentID == null)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentRequired, sourceModule);
                    _parentNotFound = true;
                }
                else if (_parent == null || parentID != _parentID)
                {
                    _parent = (HierarchyNodeProfile)_parentIDHash[parentID];
                    if (_parent == null)
                    {
                        _parent = NodeLookup(ref em, parentID, false, false);
                        if (_parent.Key != Include.NoRID)
                        {
                            _parentIDHash[parentID] = _parent;
                        }
                    }
                    _parentID = parentID;
                    if (_parent.Key == Include.NoRID)  // parent does not exist
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                        _parentNotFound = true;
                    }
                    else
                    {
                        _parentNotFound = false;
                    }
                }
                else if (_parentNotFound)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + parentID, sourceModule);
                }


                if (nodeID != null && parentID != null && _parent.Key != Include.NoRID)  // can't edit child if don't know parent
                {
                    if (_parent.LevelType == eHierarchyLevelType.Color)
                    {
                        hnp = new HierarchyNodeProfile(Include.NoRID);
                    }
                    else if (_parent.LevelType == eHierarchyLevelType.Style)
                    {
                        //hnp = NodeLookup(ref em, parentID + _nodeDelimiter + nodeID, true, false);
                        hnp = new HierarchyNodeProfile(Include.NoRID);
                    }
                    else
                    {
                        hnp = NodeLookup(ref em, nodeID, false, false);
                        // Begin TT#2737 - JSmith - Brand Hierarchy Reclass
                        if (hnp.Key != Include.NoRID &&
                            hnp.HomeHierarchyOwner != Include.GlobalUserRID &&
                            hnp.HomeHierarchyOwner != _parent.HomeHierarchyOwner)	// check if same owner
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CrossUserAlternateError, sourceModule);
                        }
                        else if (_parent.Key != Include.NoRID &&
                            _parent.HomeHierarchyRID != _hp.Key)	// check parent in hierarchy
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NotParentsHomeHierarchy, sourceModule);
                        }
                        //if (hnp.Key != Include.NoRID && hnp.NodeLevel > 0)	// check if same parent
                        else if (hnp.Key != Include.NoRID && hnp.NodeLevel > 0)	// check if same parent
                        // End TT#2737 - JSmith - Brand Hierarchy Reclass
                        {
                            // nodes can only be added in the parent's home hierarchy
                            if (_parent.HomeHierarchyRID != _hp.Key)
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NotParentsHomeHierarchy, sourceModule);
                            }
                            else
                            {
                                // see if the node is currently in the hierarchy
                                HierarchyNodeProfile currentParent = _SAB.HierarchyServerSession.GetNodeData(_hp.Key, hnp.Key, false, false);

                                if (currentParent.HomeHierarchyParentRID != Include.NoRID &&
                                    currentParent.HomeHierarchyParentRID != _parent.Key)	// different parent 
                                {
                                    if (_hp.HierarchyType == eHierarchyType.organizational)
                                    {
                                        hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[_parent.HomeHierarchyLevel + 1];
                                        if (hlp.LevelType == eHierarchyLevelType.Color ||
                                            hlp.LevelType == eHierarchyLevelType.Size)
                                        {
                                            // color and size IDs can be duplicated under different parents
                                            // reset fields since node that was found is not the right node
                                            hnp = new HierarchyNodeProfile(Include.NoRID);
                                        }
                                        else
                                        {
                                            string message = "Hierarchy: " + hierarchyID + "; Parent: " + parentID + "; ID: " + nodeID + "; Name: " + nodeName + "; Description: " + description;
                                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                                        }
                                    }
                                    else if (hnp.HomeHierarchyRID != _hp.Key &&
                                        _hp.HierarchyType == eHierarchyType.alternate)
                                    {
                                        // assume adding relationship to alternate hierarchy
                                    }
                                    else
                                    {
                                        string message = "Hierarchy: " + hierarchyID + "; Parent: " + parentID + "; ID: " + nodeID + "; Name: " + nodeName + "; Description: " + description;
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                                    }
                                }
                            }
                        }
                        else if ((_parent.NodeLevel + 1) != hnp.NodeLevel)	// not the node you think it is. may be color or size with same code as other node
                        {
                            hnp = new HierarchyNodeProfile(Include.NoRID);
                        }
                    }
                }

                if (hnp.Key != Include.NoRID)
                {
                    currentHnp = (HierarchyNodeProfile)hnp.Clone();
                    if (!hnp.Equals(currentHnp))
                    {
                        bool stop = true;
                    }
                }
                // End TT#1148

                //Begin Track #3948 - JSmith - add OTS Forecast Level interface
                // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                // only edit OTS Forecast Level fields if home hierarchy
                if (_hp.Key == hnp.HomeHierarchyRID)
                {
                    // End TT#668
                    Hashtable hierarchyLevels = new Hashtable();
                    HierarchyNodeProfile anchorNode = null;
                    if (aOTSForecastLevelHierarchy != null &&
                        aOTSForecastLevelHierarchy.Trim().Length > 0)
                    {
                        //Begin Track #4454 - JSmith - OTS Forecast Level reset
                        if (aOTSForecastLevelHierarchy.ToUpper(CultureInfo.CurrentUICulture) != "UNDEFINED")
                        {
                            //End Track #4454
                            anchorNode = _SAB.HierarchyServerSession.GetNodeData(aOTSForecastLevelHierarchy.Trim());
                            if (anchorNode.HomeHierarchyType == eHierarchyType.alternate)
                            {
                                hnp.OTSPlanLevelAnchorNode = anchorNode.Key;
                                // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                                //BuildAlternateOTSLevelList(anchorNode, hierarchyLevels);
                                hierarchyLevels = (Hashtable)_OTSHash[anchorNode.Key];
                                if (hierarchyLevels == null)
                                {
                                    hierarchyLevels = new Hashtable();
                                    BuildAlternateOTSLevelList(anchorNode, hierarchyLevels);
                                    _OTSHash.Add(anchorNode.Key, hierarchyLevels);
                                }
                                // End TT#668
                            }
                            else if (anchorNode.HomeHierarchyType == eHierarchyType.organizational &&
                                anchorNode.HomeHierarchyLevel == 0)
                            {
                                hnp.OTSPlanLevelAnchorNode = anchorNode.Key;
                                // add root level
                                hlp = new HierarchyLevelProfile(0);
                                hlp.Level = 0;
                                hlp.LevelChangeType = eChangeType.none;
                                hlp.LevelID = _hp.HierarchyID;
                                hlp.LevelType = eHierarchyLevelType.Undefined;
                                hlp.OTSPlanLevelLevelType = ePlanLevelLevelType.HierarchyLevel;
                                hierarchyLevels.Add(0, hlp);
                                for (int levelIndex = 1; levelIndex <= _hp.HierarchyLevels.Count; levelIndex++)
                                {
                                    hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelIndex];
                                    hierarchyLevels.Add(levelIndex, hlp);
                                }
                            }
                            else
                            {
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidOTSHierarchy, sourceModule);
                            }
                        }
                        else
                        {
                            hnp.OTSPlanLevelAnchorNode = Include.NoRID;
                            // add root level
                            hlp = new HierarchyLevelProfile(0);
                            hlp.Level = 0;
                            hlp.LevelChangeType = eChangeType.none;
                            hlp.LevelID = _hp.HierarchyID;
                            hlp.LevelType = eHierarchyLevelType.Undefined;
                            hlp.OTSPlanLevelLevelType = ePlanLevelLevelType.HierarchyLevel;
                            hierarchyLevels.Add(0, hlp);
                            for (int levelIndex = 1; levelIndex <= _hp.HierarchyLevels.Count; levelIndex++)
                            {
                                hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelIndex];
                                hierarchyLevels.Add(levelIndex, hlp);
                            }
                        }
                    }
                    //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                    //else if (hnp.OTSPlanLevelHierarchyRID != Include.NoRID)
                    else if (hnp.OTSPlanLevelHierarchyRID != Include.NoRID &&
                        aOTSForecastLevel != null &&
                        aOTSForecastLevel.Trim().Length > 0)
                    // End TT#668
                    {
                        int anchorNodeRID;
                        if (hnp.OTSPlanLevelAnchorNode == Include.NoRID)
                        {
                            if (_mainHierProf == null)
                            {
                                _mainHierProf = _SAB.HierarchyServerSession.GetMainHierarchyData();
                            }
                            anchorNodeRID = _mainHierProf.HierarchyRootNodeRID;
                        }
                        else
                        {
                            anchorNodeRID = hnp.OTSPlanLevelAnchorNode;
                        }

                        anchorNode = _SAB.HierarchyServerSession.GetNodeData(anchorNodeRID);
                        if (anchorNode.HomeHierarchyType == eHierarchyType.alternate)
                        {
                            hnp.OTSPlanLevelAnchorNode = anchorNode.Key;
                            // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                            //BuildAlternateOTSLevelList(anchorNode, hierarchyLevels);
                            hierarchyLevels = (Hashtable)_OTSHash[anchorNode.Key];
                            if (hierarchyLevels == null)
                            {
                                hierarchyLevels = new Hashtable();
                                BuildAlternateOTSLevelList(anchorNode, hierarchyLevels);
                                _OTSHash.Add(anchorNode.Key, hierarchyLevels);
                            }
                            // End TT#668
                        }
                        else if (anchorNode.HomeHierarchyType == eHierarchyType.organizational &&
                            anchorNode.HomeHierarchyLevel == 0)
                        {
                            //						hnp.OTSPlanLevelAnchorNode = anchorNode.Key;
                            // add root level
                            hlp = new HierarchyLevelProfile(0);
                            hlp.Level = 0;
                            hlp.LevelChangeType = eChangeType.none;
                            hlp.LevelID = _hp.HierarchyID;
                            hlp.LevelType = eHierarchyLevelType.Undefined;
                            hlp.OTSPlanLevelLevelType = ePlanLevelLevelType.HierarchyLevel;
                            hierarchyLevels.Add(0, hlp);
                            for (int levelIndex = 1; levelIndex <= _hp.HierarchyLevels.Count; levelIndex++)
                            {
                                hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelIndex];
                                hierarchyLevels.Add(levelIndex, hlp);
                            }
                        }
                    }
                    //End Track #4454

                    if (aOTSForecastLevel != null &&
                        aOTSForecastLevel.Trim().Length > 0 &&
                        hierarchyLevels.Count > 0)
                    {
                        //Begin Track #4454 - JSmith - OTS Forecast Level reset
                        if (aOTSForecastLevel.ToUpper() != "UNDEFINED")
                        {
                            //End Track #4454
                            bool foundLevel = false;
                            for (int levelIndex = 0; levelIndex <= hierarchyLevels.Count; levelIndex++)
                            {
                                hlp = (HierarchyLevelProfile)hierarchyLevels[levelIndex];
                                if (aOTSForecastLevel.ToUpper() == hlp.LevelID.ToUpper() &&
                                    hlp.LevelType != eHierarchyLevelType.Size)
                                {
                                    foundLevel = true;
                                    break;
                                }
                            }
                            if (!foundLevel)
                            {
                                if (aOTSForecastLevel.StartsWith("+"))
                                {
                                    try
                                    {
                                        int offset = Convert.ToInt32(aOTSForecastLevel);
                                        hnp.OTSPlanLevelLevelType = ePlanLevelLevelType.LevelOffset;
                                        foundLevel = true;
                                    }
                                    catch
                                    {
                                    }
                                }
                            }

                            if (foundLevel)
                            {
                                hnp.OTSPlanLevelHierarchyLevelSequence = hlp.Level;
                                if (hlp.OTSPlanLevelLevelType == ePlanLevelLevelType.LevelOffset)
                                {
                                    if (hnp.OTSPlanLevelHierarchyLevelSequence == 0)
                                    {
                                        hnp.OTSPlanLevelHierarchyRID = anchorNode.Key;
                                    }
                                    else
                                    {
                                        hnp.OTSPlanLevelHierarchyRID = anchorNode.HomeHierarchyRID;
                                    }
                                }
                                else
                                {
                                    hnp.OTSPlanLevelHierarchyRID = _hp.Key;
                                }
                                //						hnp.OTSPlanLevelIsOverridden = true;
                                hnp.OTSPlanLevelLevelType = hlp.OTSPlanLevelLevelType;
                            }
                            else
                            {
                                hnp.OTSPlanLevelHierarchyLevelSequence = Include.NoRID;
                                hnp.OTSPlanLevelHierarchyRID = Include.Undefined;
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidOTSForecastLevel, sourceModule);
                            }
                        }
                        else
                        {
                            hnp.OTSPlanLevelHierarchyLevelSequence = Include.NoRID;
                            hnp.OTSPlanLevelHierarchyRID = Include.NoRID;
                            //					hnp.OTSPlanLevelIsOverridden = false;
                            hnp.OTSPlanLevelLevelType = ePlanLevelLevelType.Undefined;
                        }
                        //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    }
                    //End Track #4454

                    //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    if (aOTSForecastLevelSelect != null &&
                        aOTSForecastLevelSelect.Trim().Length > 0)
                    {
                        //End Track #4454
                        switch (aOTSForecastLevelSelect.ToUpper(CultureInfo.CurrentUICulture))
                        {
                            case "LEVEL":
                                hnp.OTSPlanLevelIsOverridden = true;
                                hnp.OTSPlanLevelSelectType = ePlanLevelSelectType.HierarchyLevel;
                                break;
                            case "NODE":
                                hnp.OTSPlanLevelIsOverridden = true;
                                hnp.OTSPlanLevelSelectType = ePlanLevelSelectType.Node;
                                break;
                            case "UNDEFINED":
                                hnp.OTSPlanLevelIsOverridden = false;
                                hnp.OTSPlanLevelSelectType = ePlanLevelSelectType.Undefined;
                                break;
                            default:
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidOTSSelectType, sourceModule);
                                break;
                        }
                        //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    }
                    //End Track #4454

                    //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    if (aOTSForecastNodeSearch != null &&
                        aOTSForecastNodeSearch.Trim().Length > 0)
                    {
                        //End Track #4454
                        switch (aOTSForecastNodeSearch.ToUpper(CultureInfo.CurrentUICulture))
                        {
                            case "ID":
                                hnp.OTSPlanLevelMaskField = eMaskField.Id;
                                break;
                            case "NAME":
                                hnp.OTSPlanLevelMaskField = eMaskField.Name;
                                break;
                            case "DESCRIPTION":
                                hnp.OTSPlanLevelMaskField = eMaskField.Description;
                                break;
                            case "UNDEFINED":
                                hnp.OTSPlanLevelMaskField = eMaskField.Undefined;
                                break;
                            default:
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidOTSNodeSearchSelectType, sourceModule);
                                break;
                        }
                        //Begin Track #4454 - JSmith - OTS Forecast Level reset
                    }
                    //End Track #4454

                    hnp.OTSPlanLevelMask = aOTSForecastStartsWith;

                    if (hnp.OTSPlanLevelSelectType == ePlanLevelSelectType.HierarchyLevel &&
                        hnp.OTSPlanLevelHierarchyLevelSequence == Include.NoRID)
                    {
                        if (aOTSForecastLevel == null ||
                            aOTSForecastLevel.Trim().Length == 0)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_OTSLevelRequired, sourceModule);
                        }
                    }
                    else if (hnp.OTSPlanLevelSelectType == ePlanLevelSelectType.Node &&
                        (hnp.OTSPlanLevelMask == null || hnp.OTSPlanLevelMask.Trim().Length == 0))
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_OTSLevelStartsWithRequired, sourceModule);
                    }
                }

                // Begin TT#1399 - JSmith - Alternate Hierarchy Inherit Node Properties
                if (aApplyNodeProperties != null &&
                    aApplyNodeProperties.Trim().Length > 0)
                {
                    if ((hnp.isFound && hnp.HomeHierarchyType != eHierarchyType.alternate) ||
                        (!hnp.isFound && _parent.HomeHierarchyType != eHierarchyType.alternate))
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeAlternateForApplyNodeProperties, sourceModule);
                    }
                    else
                    {
                        HierarchyNodeProfile applyNode = NodeLookup(ref em, aApplyNodeProperties, false, false);
                        if (!applyNode.isFound)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ApplyNodeNotFound, sourceModule);
                        }
                        else if (applyNode.HomeHierarchyType != eHierarchyType.organizational)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeOrganizationslForApplyNodeProperties, sourceModule);
                        }
                        else
                        {
                            hnp.ApplyHNRIDFrom = applyNode.Key;
                        }
                    }
                }
                else if (aApplyNodeProperties != null &&
                    aApplyNodeProperties.Trim().Length == 0)
                {
                    hnp.ApplyHNRIDFrom = Include.NoRID;
                }
                // End TT#1399

                //Begin Track #4069 - JSmith - OTS Forecast Level being reset
                //					else
                //					{
                //						hnp.OTSPlanLevelHierarchyLevelSequence = Include.Undefined;
                //						hnp.OTSPlanLevelHierarchyRID = Include.NoRID;
                //					}
                //End Track #4069
                //End Track #3948

                // no errors so update record
                if (!em.ErrorFound)
                {
                    hnp.NodeID = nodeID;
					//BEGIN TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                    if (nodeName != null)
                    { 
                        hnp.NodeName = nodeName;
                    }
					//hnp.NodeName = nodeName;
					//END TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                    // Begin TT#3342 - JSmith - Updating node without description in the trasaction causes database error
                    //hnp.NodeDescription = description;
                    if (description != null ||
                        hnp.LevelType == eHierarchyLevelType.Color)
                    {
                        hnp.NodeDescription = description;
                    }
                    // End TT#3342 - JSmith - Updating node without description in the trasaction causes database error
                    switch (productType.ToUpper(CultureInfo.CurrentUICulture))
                    {
                        case "HARDLINES":
                            hnp.ProductType = eProductType.Hardline;
                            break;
                        case "SOFTLINES":
                            hnp.ProductType = eProductType.Softline;
                            break;
                        case "UNDEFINED":
                            hnp.ProductType = eProductType.Undefined;
                            break;
                        default:
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidProductType, sourceModule);
                            break;
                    }
                    if (hnp.Key == Include.NoRID)  // node does not exist
                    {
                        hnp.HierarchyRID = _hp.Key;
                        hnp.HomeHierarchyRID = _parent.HomeHierarchyRID;
                        //							hnp.ParentRID = _parent.Key;
                        hnp.HomeHierarchyParentRID = _parent.Key;
                        if (!hnp.Parents.Contains(_parent.Key))
                        {
                            hnp.Parents.Add(_parent.Key);
                        }
                        hnp.HomeHierarchyLevel = _parent.HomeHierarchyLevel + 1;
                        EditNewProduct(ref em, ref hnp, sizeProductCategory, sizePrimary, sizeSecondary);
                        // Begin TT#1148 - JSmith - Performance
                        blHasChanges = true;
                        // End TT#1148
                    }
                    // Begin TT#1148 - JSmith - Performance
                    //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                    //else if (hnp.NodeDescription != currentHnp.NodeDescription ||
                    //        hnp.NodeName != currentHnp.NodeName ||
                    //        hnp.ProductType != currentHnp.ProductType ||
                    //        hnp.OTSPlanLevelAnchorNode != currentHnp.OTSPlanLevelAnchorNode ||
                    //        hnp.OTSPlanLevelHierarchyLevelSequence != currentHnp.OTSPlanLevelHierarchyLevelSequence ||
                    //        hnp.OTSPlanLevelHierarchyRID != currentHnp.OTSPlanLevelHierarchyRID ||
                    //        hnp.OTSPlanLevelLevelType != currentHnp.OTSPlanLevelLevelType ||
                    //        hnp.OTSPlanLevelMask != currentHnp.OTSPlanLevelMask ||
                    //        hnp.OTSPlanLevelMaskField != currentHnp.OTSPlanLevelMaskField ||
                    //        hnp.OTSPlanLevelSelectType != currentHnp.OTSPlanLevelSelectType ||
                    //        hnp.OTSPlanLevelType != currentHnp.OTSPlanLevelType ||
                    //        hnp.ApplyHNRIDFrom != currentHnp.ApplyHNRIDFrom || // TT#1399 - JSmith - Alternate Hierarchy Inherit Node Properties
                    //        hnp.NodeColor != currentHnp.NodeColor)
                    else if (!hnp.Equals(currentHnp))
                    //Begin TT#106 MD
                    {
                        blHasChanges = true;
                    }
                    // Begin TT#1727 - JSmith - Alternate Hierarchy Load will not load Organizational Nodes
                    else if (hnp.HierarchyRID != _parent.HierarchyRID)
                    {
                        blHasChanges = true;
                    }
                    // End TT#1727
                    // End TT#1148
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        private void BuildAlternateOTSLevelList(HierarchyNodeProfile aAnchorNodeProfile, Hashtable aHierarchyLevels)
        {
            int i;
            int levelIndex = 0;
            HierarchyLevelProfile hlp = null;

            try
            {
                // add anchor node
                hlp = new HierarchyLevelProfile(levelIndex);
                hlp.Level = 0;
                hlp.LevelChangeType = eChangeType.none;
                hlp.LevelID = aAnchorNodeProfile.NodeID;
                hlp.LevelType = eHierarchyLevelType.Undefined;
                hlp.OTSPlanLevelLevelType = ePlanLevelLevelType.LevelOffset;

                aHierarchyLevels.Add(levelIndex, hlp);
                ++levelIndex;

                HierarchyProfile mainHierProf = _SAB.HierarchyServerSession.GetMainHierarchyData();

                int highestGuestLevel = int.MaxValue;

                if (aAnchorNodeProfile.HomeHierarchyType == eHierarchyType.alternate)
                {
                    highestGuestLevel = _SAB.HierarchyServerSession.GetHighestGuestLevel(aAnchorNodeProfile.Key);
                }

                // add guest levels to list
                if (highestGuestLevel != int.MaxValue)
                {
                    for (i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                    {
                        hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
                        if (hlp.LevelType != eHierarchyLevelType.Size)
                        {
                            aHierarchyLevels.Add(levelIndex, hlp);
                            ++levelIndex;
                        }
                    }
                }

                // add offsets to list
                int longestBranchCount = _SAB.HierarchyServerSession.GetLongestBranch(aAnchorNodeProfile.Key);
                int offset = 0;
                for (i = 0; i < longestBranchCount; i++)
                {
                    ++offset;
                    hlp = new HierarchyLevelProfile(levelIndex);
                    hlp.Level = offset;
                    hlp.LevelChangeType = eChangeType.none;
                    hlp.LevelID = "+" + offset.ToString();
                    hlp.LevelType = eHierarchyLevelType.Undefined;
                    hlp.OTSPlanLevelLevelType = ePlanLevelLevelType.LevelOffset;

                    aHierarchyLevels.Add(levelIndex, hlp);
                    ++levelIndex;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to edit node data on a change request
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hnp">An instance of the HierarchyNodeProfile class which contains node data</param>
        public void EditMerchandiseData(ref EditMsgs em, ref HierarchyNodeProfile hnp)
        {
            try
            {
                if (_hp == null || hnp.HierarchyRID != _hp.Key)  // make sure you have the right hierarchy
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HierarchyRID);
                }

                if (_parent == null || hnp.HomeHierarchyParentRID != _parent.Key)  // make sure you have the right parent
                {
                    _parent = _SAB.HierarchyServerSession.GetNodeData(hnp.HomeHierarchyParentRID);
                    // begin MID Track # 3818 - set field when parent changes
                    _parentID = _parent.NodeID;
                    // end MID Track # 3818 - set field when parent changes
                }

                if (hnp.LevelType == eHierarchyLevelType.Color ||
                    hnp.LevelType == eHierarchyLevelType.Size)
                {
                    hnp.HierarchyRID = _hp.Key;
                    hnp.HomeHierarchyRID = _parent.HomeHierarchyRID;
                    hnp.HomeHierarchyParentRID = _parent.Key;
                    hnp.HomeHierarchyLevel = _parent.HomeHierarchyLevel + 1;
                }
                else
                {
                    HierarchyNodeProfile checkExists = _SAB.HierarchyServerSession.GetNodeData(hnp.NodeID);
                    if (hnp.NodeChangeType == eChangeType.add && checkExists.Key != -1)
                    {
                        // Begin TT#xxx - JSmith - Product ID already exists
                        //string message = "ID: " + hnp.NodeID;
                        //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                        hnp = checkExists;
                        // End TT#xxx
                    }
                    // Begin Track #6348 - JSmith - Create Node under My Hier - get err message
                    else if (hnp.NodeChangeType == eChangeType.update &&
                        checkExists.Key != -1 &&
                        checkExists.Key != hnp.Key)
                    {
                        string message = "ID: " + hnp.NodeID;
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, message, sourceModule);
                    }
                    // End Track #6348
                    else if (hnp.Key == -1)  // node does not exist
                    {
                        hnp.HierarchyRID = _hp.Key;
                        hnp.HomeHierarchyRID = _parent.HomeHierarchyRID;
                        hnp.HomeHierarchyParentRID = _parent.Key;
                        hnp.HomeHierarchyLevel = _parent.HomeHierarchyLevel + 1;
                    }
                }
                EditNewProduct(ref em, ref hnp, null, null, null);
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to validate information before a new product is added.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hnp">An instance of the HierarchyNodeProfile class which contains node data</param>
        private void EditNewProduct(ref EditMsgs em, ref HierarchyNodeProfile hnp,
            string sizeProductCategory, string sizePrimary, string sizeSecondary)
        {
            try
            {
                if (_hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[_parent.HomeHierarchyLevel + 1];
                    if (hlp == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AddBeyondLevels, sourceModule);
                    }
                    else
                    {
                        switch (hlp.LevelLengthType)
                        {
                            case eLevelLengthType.unrestricted:
                                {
                                    break;
                                }
                            case eLevelLengthType.required:
                                {
                                    if (hnp.NodeID.Length != hlp.LevelRequiredSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelRequiredSize, sourceModule);
                                    }
                                    break;
                                }
                            case eLevelLengthType.range:
                                {
                                    if (hnp.NodeID.Length < hlp.LevelSizeRangeFrom || hnp.NodeID.Length > hlp.LevelSizeRangeTo)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelRange, sourceModule);
                                    }
                                    break;
                                }
                            default:
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                        }
                    }
                    switch (hlp.LevelType)
                    {
                        //Begin Track #3863 - JScott - OTS Forecast Level Defaults
                        //						case eHierarchyLevelType.Planlevel:
                        //						{
                        //							hnp.LevelType = eHierarchyLevelType.Planlevel;
                        //							break;
                        //						}
                        //End Track #3863 - JScott - OTS Forecast Level Defaults
                        case eHierarchyLevelType.Style:
                            {
                                hnp.LevelType = eHierarchyLevelType.Style;
                                break;
                            }
                        case eHierarchyLevelType.Color:
                            {
                                ColorCodeProfile ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(hnp.NodeID);
                                hnp.LevelType = eHierarchyLevelType.Color;
                                if (ccp.Key == -1)  // autoadd color code if not defined
                                {
                                    if (hnp.NodeID.Length > Include.ColorIDMaxSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDTooLarge, sourceModule);
                                    }
                                    else
                                    {
                                        ccp = new ColorCodeProfile(-1);
                                        ccp.ColorCodeChangeType = eChangeType.add;
                                        ccp.ColorCodeID = hnp.NodeID;
                                        ccp.ColorCodeName = hnp.NodeName;
                                        ccp.ColorCodeGroup = null;
                                        ccp = ColorCodeUpdate(ref em, ccp);
                                    }
                                }
                                //							int colorNodeRID = -1;
                                //							if (hnp.NodeChangeType == eChangeType.add &&
                                //								_SAB.HierarchyServerSession.ColorExistsForStyle(hnp.HomeHierarchyRID, hnp.HomeHierarchyParentRID, hnp.NodeID, ref colorNodeRID))
                                //							{
                                //								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ColorAlreadyInStyle, sourceModule);
                                //							}
                                hnp.ColorOrSizeCodeRID = ccp.Key;
                                // Begin TT#89 MD - JSmith - Autoadded color showing incorrect name and description until hierarchy service restarted
                                if (hnp.NodeName == hnp.NodeID)
                                {
                                    hnp.NodeName = ccp.ColorCodeName;
                                }

                                if (hnp.NodeDescription == hnp.NodeID)
                                {
                                    hnp.NodeDescription = ccp.ColorCodeName;
                                }
                                // End TT#89
                                break;
                            }
                        case eHierarchyLevelType.Size:
                            {
                                SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(hnp.NodeID);
                                hnp.LevelType = eHierarchyLevelType.Size;
                                if (scp.Key == -1)  // autoadd size code if not defined
                                {
                                    if (hnp.NodeID.Length > Include.SizeIDMaxSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDTooLarge, sourceModule);
                                    }
                                    else
                                    {
                                        scp.SizeCodeChangeType = eChangeType.add;
                                        scp.SizeCodeID = hnp.NodeID;
                                        scp.SizeCodeProductCategory = sizeProductCategory;
                                        scp.SizeCodePrimary = sizePrimary;
                                        scp.SizeCodeSecondary = sizeSecondary;
                                        scp.SizeCodeName = Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID);
                                        scp = SizeCodeUpdate(ref em, scp);
                                    }
                                }
                                hnp.ColorOrSizeCodeRID = scp.Key;
                                break;
                            }
                        default:
                            {
                                if (hnp.NodeID.Length > Include.BaseIDMaxSize)
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeIDTooLarge, sourceModule);
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to validate the product name conforms to the constraints for the level
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="hierarchyRID">The record ID of the hierarchy where the node is to be added</param>
        /// <param name="parentRID">The record ID of the parent where the node is to be added</param>
        /// <param name="nodeID">The ID of the node to be added</param>
        public void NodeIDValid(ref EditMsgs em, int hierarchyRID, int parentRID, string nodeID)
        {
            try
            {
                if (parentRID == 0)  // hierarchy node so do not edit name
                {
                    return;
                }

                if (_hp == null || _hp.Key != hierarchyRID)  // make sure you have the right hierarchy
                {
                    _hp = _SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID);
                }

                if (_parent == null || _parent.Key != parentRID)  // make sure you have the right parent
                {
                    _parent = _SAB.HierarchyServerSession.GetNodeData(parentRID);
                    // begin MID Track # 3818 - set field when parent changes
                    _parentID = _parent.NodeID;
                    // end MID Track # 3818 - set field when parent changes
                }

                if (_hp.HierarchyType == eHierarchyType.organizational)  // only check for level constraint if organizational hierarchy
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[_parent.HomeHierarchyLevel + 1];
                    if (hlp == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AddBeyondLevels, sourceModule);
                    }
                    else
                    {
                        switch (hlp.LevelLengthType)
                        {
                            case eLevelLengthType.unrestricted:
                                {
                                    break;
                                }
                            case eLevelLengthType.required:
                                {
                                    if (nodeID.Length != hlp.LevelRequiredSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelRequiredSize, sourceModule);
                                    }
                                    break;
                                }
                            case eLevelLengthType.range:
                                {
                                    if (nodeID.Length < hlp.LevelSizeRangeFrom || nodeID.Length > hlp.LevelSizeRangeTo)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelRange, sourceModule);
                                    }
                                    break;
                                }
                            default:
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to update color code information.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="ccp">An instance of the ColorCodeProfile class that contains the color code information</param>
        /// <returns></returns>
        public ColorCodeProfile ColorCodeUpdate(ref EditMsgs em, ColorCodeProfile ccp)
        {
            try
            {
                HierarchyLevelProfile hlp = null;
                int levelIndex = 0;
                // get color level information
                if (ccp.ColorCodeChangeType == eChangeType.add)
                {
                    HierarchyProfile hp = _SAB.HierarchyServerSession.GetMainHierarchyData();
                    for (levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
                    {
                        hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
                        if (hlp.LevelType == eHierarchyLevelType.Color)
                        {
                            break;
                        }
                    }
                    if (hlp == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AppErrorColorLevelNotFound, sourceModule);
                    }
                    else
                    {
                        switch (hlp.LevelLengthType)
                        {
                            case eLevelLengthType.unrestricted:
                                {
                                    //								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                            case eLevelLengthType.required:
                                {
                                    if (ccp.ColorCodeID.Length != hlp.LevelRequiredSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ColorCodeNotValid, sourceModule);
                                    }
                                    break;
                                }
                            case eLevelLengthType.range:
                                {
                                    if (ccp.ColorCodeID.Length < hlp.LevelSizeRangeFrom || ccp.ColorCodeID.Length > hlp.LevelSizeRangeTo)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ColorCodeNotValid, sourceModule);
                                    }
                                    break;
                                }
                            default:
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                        }
                    }
                }

                if (!em.ErrorFound)
                {
                    ccp = _SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                }
                return ccp;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to update size code information.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="scp">An instance of the SizeCodeProfile class that contains the size code information</param>
        /// <returns></returns>
        public SizeCodeProfile SizeCodeUpdate(ref EditMsgs em, SizeCodeProfile scp)
        {
            try
            {
                HierarchyLevelProfile hlp = null;
                int levelIndex = 0;
                // get color level information
                if (scp.SizeCodeChangeType == eChangeType.add)
                {
                    HierarchyProfile hp = _SAB.HierarchyServerSession.GetMainHierarchyData();
                    for (levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
                    {
                        hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
                        if (hlp.LevelType == eHierarchyLevelType.Size)
                        {
                            break;
                        }
                    }
                    if (hlp == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_AppErrorColorLevelNotFound, sourceModule);
                    }
                    else
                    {
                        switch (hlp.LevelLengthType)
                        {
                            case eLevelLengthType.unrestricted:
                                {
                                    //								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                            case eLevelLengthType.required:
                                {
                                    if (scp.SizeCodeID.Length != hlp.LevelRequiredSize)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_SizeCodeNotValid, sourceModule);
                                    }
                                    break;
                                }
                            case eLevelLengthType.range:
                                {
                                    if (scp.SizeCodeID.Length < hlp.LevelSizeRangeFrom || scp.SizeCodeID.Length > hlp.LevelSizeRangeTo)
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_SizeCodeNotValid, sourceModule);
                                    }
                                    break;
                                }
                            default:
                                {
                                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidLevelLengthType, sourceModule);
                                    break;
                                }
                        }
                    }
                }

                if (!em.ErrorFound)
                {
                    try
                    {
                        scp = _SAB.HierarchyServerSession.SizeCodeUpdate(scp);
                    }
                    catch (SizePrimaryRequiredException)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_SizePrimaryRequired, sourceModule);
                    }
                    catch (SizeCatgPriSecNotUniqueException exc)
                    {
                        string message = MIDText.GetText((int)eMIDTextCode.msg_SizeCatgPriSecNotUnique);
                        message = message.Replace("{0}", scp.SizeCodeProductCategory);
                        message = message.Replace("{1}", scp.SizeCodePrimary);
                        if (scp.SizeCodeSecondary != null &&
                            scp.SizeCodeSecondary.Trim().Length > 1)
                        {
                            message = message.Replace("{2}", scp.SizeCodeSecondary);
                        }
                        else
                        {
                            message = message.Replace("{2}", _noSecondarySizeStr);
                        }
                        message = message.Replace("{3}", exc.Message);
                        em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
                    }
                }
                return scp;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Used to move a product.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aHierarchyID">The ID of the hierarchy</param>
        /// <param name="aParent">The ID of the parent</param>
        /// <param name="aNodeID">The ID of the node</param>
        /// <param name="aToParent">The ID of the parent where the node is to be moved</param>
        /// <param name="aReclassPreview">a flag identifying if the process is previewing what reclass would do</param>
        /// <param name="aRollupForecastVersions">a Hashtable containing the record IDs of the forecast version to roll</param>
        /// <param name="aRollExternalIntransit">a flag identifying if the process is to schedule external intransit to roll</param>
        /// <param name="aRollAlternateHierarchies">a flag identifying if the process is to schedule alternate hierarchies to roll</param>
        // Begin Track #5259 - JSmith - Add new reclass roll options
        //		public void MoveNode(ref EditMsgs em, string aHierarchyID, string aParent, string aNodeID, string aToParent,
        //			bool aReclassPreview, Hashtable aRollupForecastVersions)
        public void MoveNode(ref EditMsgs em, string aHierarchyID, string aParent, string aNodeID, string aToParent,
            bool aReclassPreview, Hashtable aRollupForecastVersions, bool aRollExternalIntransit, bool aRollAlternateHierarchies)
        // End Track #5259
        {
            try
            {
                DragDropEffects dropAction = DragDropEffects.Move;
                if (!_reclassTextRead)
                {
                    ReadReclassText();
                }

                HierarchyNodeProfile toHnp = null;

                ValidateHierarchyID(ref em, ref aHierarchyID);

                ValidateParentID(ref em, ref aParent);

                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                //HierarchyNodeProfile hnp = ValidateNodeID(ref em, ref aNodeID);
                HierarchyNodeProfile hnp = ValidateNodeID(ref em, aParent, ref aNodeID);
                // End TT#298

                if (aToParent == null)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ToParentRequired, sourceModule);
                }
                else
                {
                    aToParent = aToParent.Trim();
                    toHnp = (HierarchyNodeProfile)_parentIDHash[aToParent];
                    if (toHnp == null)
                    {
                        toHnp = NodeLookup(ref em, aToParent, false, false);
                        if (toHnp.Key != Include.NoRID)
                        {
                            _parentIDHash[aToParent] = toHnp;
                        }
                        else
                        {
                            // Begin TT#3366 - JSmith - Parent not found error in Audit is deceiving
                            //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, sourceModule);
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ToParentNotFound, sourceModule);
                            // End TT#3366 - JSmith - Parent not found error in Audit is deceiving
                        }
                    }
                }

                if (_parent.HomeHierarchyRID != _hp.Key)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotInHierarchy, sourceModule);
                }

                if ((_parent != null && _parent.Key != Include.NoRID) &&
                    (hnp != null && hnp.Key != Include.NoRID))
                {
                    if (!IsChild(_hp.Key, _parent.Key, hnp.Key))
                    {
                        //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                        string message = (string)_invalidParentChild.Clone();
                        message = message.Replace("{0}", aParent);
                        message = message.Replace("{1}", aNodeID);
                        message = message.Replace("{2}", aHierarchyID);
                        em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
                        //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidParentChild, sourceModule);
                        //Begin TT#266
                        return;
                    }
                }

                if (!em.ErrorFound)
                {
                    string message = null;
                    // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                    eDropStates aDropStates;
                        // End TT#218
                    // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                    //if (DropAllowed(hnp, _parent, toHnp, false, out message))
                    if (DropAllowed(hnp, _parent, toHnp, false, out message, out aDropStates))
                    // End TT#2186
                    {
                        bool scheduleNode = true;
                        if (_parent.HomeHierarchyType == eHierarchyType.organizational &&
                            toHnp.HomeHierarchyType == eHierarchyType.alternate)
                        {
                            dropAction = DragDropEffects.Link;
                            scheduleNode = false;
                        }

                        // Begin Track #5259 - JSmith - Add new reclass roll options
                        //						ScheduleReclassRollup(ref em, _actionMove, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, toHnp.Key, toHnp.Text, aReclassPreview, aRollupForecastVersions, scheduleNode);
                        //ScheduleReclassRollup(ref em, _actionMove, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, toHnp.Key, toHnp.Text, aReclassPreview, aRollupForecastVersions, scheduleNode, aRollExternalIntransit, aRollAlternateHierarchies);
                        // Begin TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        //ScheduleReclassRollup(ref em, _actionMove, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, toHnp.Key, toHnp.Text, aReclassPreview, aRollupForecastVersions, scheduleNode, aRollExternalIntransit, aRollAlternateHierarchies);
                        ScheduleReclassRollup(ref em, _actionMove, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, toHnp.Key, toHnp.Text, toHnp.HomeHierarchyType, aReclassPreview, aRollupForecastVersions, scheduleNode, aRollExternalIntransit, aRollAlternateHierarchies);
                        // End TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        // End Track #5259
                        if (!aReclassPreview)
                        {
                            if (dropAction == DragDropEffects.Move)
                            {
                                MoveNode(hnp, toHnp);
                            }
                            else
                            {
                                MakeShortcut(hnp, toHnp);
                            }
                            _SAB.HierarchyServerSession.CommitData();
                        }
                        if (dropAction == DragDropEffects.Move)
                        {
                            message = (string)_moveConfirmed.Clone();
                            message = message.Replace("{0}", hnp.Text);
                            message = message.Replace("{1}", _parent.Text);
                            message = message.Replace("{2}", toHnp.Text);
                            message = message.Replace("{3}", _hp.HierarchyID);
                        }
                        else
                        {
                            message = GetMessage(eMIDTextCode.msg_ReclassShortcutConfirmed);
                            message = message.Replace("{0}", hnp.Text);
                            message = message.Replace("{1}", toHnp.Text);
                            message = message.Replace("{2}", _hp.HierarchyID);
                        }
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionMove, _productLabel, hnp.Text, message);
                    }
                    // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
                    else if (aDropStates == eDropStates.Duplicate &&
                        hnp.HomeHierarchyRID != _parent.HomeHierarchyRID)
                    {
                        DeleteShortcut(hnp, _parent);
                    }
                    // End TT#2186
                    else
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionMove, _productLabel, hnp.Text, message);
                        message = (string)_moveFailed.Clone();
                        message = message.Replace("{0}", hnp.Text);
                        message = message.Replace("{1}", _parent.Text);
                        message = message.Replace("{2}", toHnp.Text);
                        message = message.Replace("{3}", _hp.HierarchyID);
                        em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionMove, _productLabel, hnp.Text, message);
                    }
                }

            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        private void MoveNode(HierarchyNodeProfile hnp, HierarchyNodeProfile toHnp)
        {
            try
            {
                HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                hjp.JoinChangeType = eChangeType.update;
                hjp.OldHierarchyRID = _hp.Key;
                hjp.OldParentRID = _parent.Key;
                hjp.NewHierarchyRID = toHnp.HomeHierarchyRID;
                hjp.NewParentRID = toHnp.Key;
                hjp.Key = hnp.Key;
                if (_hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[hnp.NodeLevel];
                    hjp.LevelType = hlp.LevelType;
                }
                _SAB.HierarchyServerSession.JoinUpdate(hjp);
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
        }

        // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
        private void DeleteShortcut(HierarchyNodeProfile hnp, HierarchyNodeProfile toHnp)
        {
            try
            {
                HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                hjp.JoinChangeType = eChangeType.delete;
                hjp.OldHierarchyRID = _hp.Key;
                hjp.OldParentRID = _parent.Key;
                hjp.NewHierarchyRID = toHnp.HomeHierarchyRID;
                hjp.NewParentRID = toHnp.Key;
                hjp.Key = hnp.Key;
                if (_hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[hnp.NodeLevel];
                    hjp.LevelType = hlp.LevelType;
                }
                _SAB.HierarchyServerSession.JoinUpdate(hjp);
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
        }
        // End TT#2186

        private void MakeShortcut(HierarchyNodeProfile hnp, HierarchyNodeProfile toHnp)
        {
            try
            {
                HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                hjp.JoinChangeType = eChangeType.add;
                hjp.NewHierarchyRID = toHnp.HierarchyRID;
                hjp.NewParentRID = toHnp.Key;
                hjp.Key = hnp.Key;
                HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(toHnp.HierarchyRID);
                if (hp.HierarchyType == eHierarchyType.organizational)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[toHnp.HomeHierarchyLevel];
                    hjp.LevelType = hlp.LevelType;
                }
                _SAB.HierarchyServerSession.JoinUpdate(hjp);
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to rename a product.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aHierarchyID">The ID of the hierarchy</param>
        /// <param name="aParent">The ID of the parent</param>
        /// <param name="aNodeID">The ID of the node</param>
        /// <param name="aNewID">The new ID of the node</param>
        /// <param name="aName">The name of the node</param>
        /// <param name="aDescription">The description of the node</param>
        /// <param name="aReclassPreview">a flag identifying if the process is previewing what reclass would do</param>
        public void RenameNode(ref EditMsgs em, string aHierarchyID, string aParent, string aNodeID, string aNewID,
            string aName, string aDescription, bool aReclassPreview)
        {
            // Begin TT#634 - JSmith - Color rename
            bool renameSuccessful = true;
            // End TT#634
            try
            {
                if (!_reclassTextRead)
                {
                    ReadReclassText();
                }

                ValidateHierarchyID(ref em, ref aHierarchyID);

                ValidateParentID(ref em, ref aParent);

                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                //HierarchyNodeProfile hnp = ValidateNodeID(ref em, ref aNodeID);
                HierarchyNodeProfile hnp = ValidateNodeID(ref em, aParent, ref aNodeID);
                // End TT#298

                if (aNewID == null || aNewID.Trim().Length == 0)
                {
                    em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_NewProductIDRequired, sourceModule);
                }
                else
                {
                    aNewID = aNewID.Trim();
                    //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                    //HierarchyNodeProfile newhnp = NodeLookup(ref em, aNewID, false, false);

                    //if (newhnp.Key != Include.NoRID)
                    //{
                    //    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, sourceModule);
                    //}
                    if (aNodeID != aNewID)
                    {
                        // Begin TT#634 - JSmith - Color rename
                        //HierarchyNodeProfile newhnp = NodeLookup(ref em, aNewID, false, false);
                        string newID = aNewID;
                        if (hnp.LevelType == eHierarchyLevelType.Color)
                        {
                            newID = aParent + _nodeDelimiter + newID;
                        }
                        EditMsgs emtemp = new EditMsgs();
                        // Begin TT#668 - JSmith - Key Item Alternate load taking a long time
                        //HierarchyNodeProfile newhnp = NodeLookup(ref emtemp, newID, true, false);
                        HierarchyNodeProfile newhnp = NodeLookup(ref emtemp, newID, false, false);
                        // End TT#668 

                        if (newhnp.Key != Include.NoRID)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DuplicateProductID, sourceModule);
                        }
                    }
                    //End TT#266
                }

                if (aName != null && aName.Trim().Length > 0)
                {
                    hnp.NodeName = aName.Trim();
                }

                if (aDescription != null && aDescription.Trim().Length > 0)
                {
                    hnp.NodeDescription = aDescription.Trim();
                }

                if ((_parent != null && _parent.Key != Include.NoRID) &&
                    (hnp != null && hnp.Key != Include.NoRID))
                {
                    if (!IsChild(_hp.Key, _parent.Key, hnp.Key))
                    {
                        //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                        string message = (string)_invalidParentChild.Clone();
                        message = message.Replace("{0}", aParent);
                        message = message.Replace("{1}", aNodeID);
                        message = message.Replace("{2}", aHierarchyID);
                        em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
                        //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidParentChild, sourceModule);
                        //Begin TT#266
                    }
                    //Begin Track #4178 - JSmith - Only rename in home relationship
                    else if (_parent.HomeHierarchyRID != hnp.HomeHierarchyRID)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustRenameInHome, sourceModule);
                    }
                    //End Track #4178
                }

                if (_parent.HomeHierarchyRID != _hp.Key)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotInHierarchy, sourceModule);
                }

                if (!em.ErrorFound &&
                    RenameAllowed(ref em, hnp))
                {
                    if (!aReclassPreview)
                    {
                        // Begin TT#634 - JSmith - Color rename
                        //hnp.NodeChangeType = eChangeType.update;
                        //hnp.NodeID = aNewID;
                        //ProcessNodeProfileInfo(ref em, hnp);
                        //_SAB.HierarchyServerSession.CommitData();
                        if (hnp.LevelType == eHierarchyLevelType.Color)
                        {
                            renameSuccessful = RenameColorNode(ref em, hnp, aHierarchyID, aParent, aNodeID, aNewID, aName, aDescription, aReclassPreview);
                        }
                        else
                        {
                            hnp.NodeChangeType = eChangeType.update;
                            hnp.NodeID = aNewID;
                            ProcessNodeProfileInfo(ref em, hnp);
                            _SAB.HierarchyServerSession.CommitData();
                        }
                        // End TT#634
                    }
                    // Begin TT#634 - JSmith - Color rename
                    //                    string message = (string) _renameConfirmed.Clone();
                    //                    message = message.Replace("{0}", hnp.Text);
                    //                    message = message.Replace("{1}", aNewID);
                    //                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    ////					_Session.Audit.AddReclassAuditMsg(_actionRename, _productLabel, hnp.Text, message);
                    if (renameSuccessful)
                    {
                        string message = (string)_renameConfirmed.Clone();
                        message = message.Replace("{0}", hnp.Text);
                        message = message.Replace("{1}", aNewID);
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    }
                    else
                    {
                        string message = (string)_renameFailed.Clone();
                        message = message.Replace("{0}", hnp.Text);
                        message = message.Replace("{1}", aNewID);
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    }
                    // End TT#634
                }
                else
                {
                    string message = (string)_renameFailed.Clone();
                    message = message.Replace("{0}", hnp.Text);
                    message = message.Replace("{1}", aNewID);
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionRename, _productLabel, hnp.Text, message);
                }

            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        // Begin TT#634 - JSmith - Color rename
        /// <summary>
        /// Used to rename a color level product.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aHierarchyID">The ID of the hierarchy</param>
        /// <param name="aParent">The ID of the parent</param>
        /// <param name="aNodeID">The ID of the node</param>
        /// <param name="aNewID">The new ID of the node</param>
        /// <param name="aName">The name of the node</param>
        /// <param name="aDescription">The description of the node</param>
        /// <param name="aReclassPreview">a flag identifying if the process is previewing what reclass would do</param>
        public bool RenameColorNode(ref EditMsgs em, HierarchyNodeProfile aHnp, string aHierarchyID, string aParent, string aNodeID, string aNewID,
            string aName, string aDescription, bool aReclassPreview)
        {
            //HierarchySessionTransaction trans = null;
            string message = null;
            try
            {
                bool undo = false;

                string oldDescription = aHnp.NodeDescription;
                bool updateHeaders = false;
                ColorCodeProfile oldccp = _SAB.HierarchyServerSession.GetColorCodeProfile(aHnp.ColorOrSizeCodeRID);
                ColorCodeProfile ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(aHnp.ColorOrSizeCodeRID);
                ColorCodeProfile newccp = _SAB.HierarchyServerSession.GetColorCodeProfile(aNewID);
                HierarchyNodeList hnl = _SAB.HierarchyServerSession.GetStylesForColor(aHnp.ColorOrSizeCodeRID);
                if (hnl.Count == 1 &&
                    newccp.Key == Include.NoRID) // only style using this color and new color does not exist, so just rename the color
                {
                    ccp.ColorCodeID = aNewID;
                    ccp.ColorCodeName = aNewID;
                    ccp.ColorCodeChangeType = eChangeType.update;
                }
                else if (newccp.Key != Include.NoRID)  // new color exists, switch to new color and update headers
                {
                    ccp = newccp;
                    updateHeaders = true;
                }
                else  // new color does not exists, add new color and update headers
                {
                    ccp = new ColorCodeProfile(Include.NoRID);
                    newccp.ColorCodeChangeType = eChangeType.add;
                    newccp.ColorCodeID = aNewID;
                    newccp.ColorCodeName = aNewID;
                    newccp.ColorCodeGroup = ccp.ColorCodeGroup;
                    ccp = newccp;
                    aHnp.NodeChangeType = eChangeType.update;
                    updateHeaders = true;
                }

                ccp = _SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                aHnp.ColorOrSizeCodeRID = ccp.Key;
                aHnp.NodeID = ccp.ColorCodeID;
                // if description is provided, update description on the node
                if (aDescription != null &&
                    aDescription.Trim().Length > 0)
                {
                    aHnp.NodeChangeType = eChangeType.update;
                    aHnp.NodeDescription = aDescription;
                }
                // must commit color changes before processing headers because different database transaction and database integrity
                ProcessNodeProfileInfo(ref em, aHnp);
                _SAB.HierarchyServerSession.CommitData();

                if (!_SAB.ApplicationServerSession.RenameColor(ref em, updateHeaders, aHnp, oldccp, ccp))
                {
                    undo = true;
                }
                //trans = new HierarchySessionTransaction(_SAB);
                //trans.DataAccess.OpenUpdateConnection();

                //if (updateHeaders)
                //{
                //    if (!UpdateColorOnHeaders(ref em, trans, aHnp, oldccp, ccp))
                //    {
                //        undo = true;
                //    }
                //}

                //if (!undo)
                //{
                //    if (!UpdateBinRecords(ref em, trans, aHnp, oldccp, ccp))
                //    {
                //        undo = true;
                //    }
                //}

                if (undo)
                {
                    //trans.DataAccess.Rollback();
                    // if header update failed, undo color code changes
                    // if added, delete new color
                    if (ccp.ColorCodeChangeType == eChangeType.add)
                    {
                        ccp.ColorCodeChangeType = eChangeType.delete;
                        _SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                    }
                    // if color update, replace with original values 
                    else if (ccp.ColorCodeChangeType == eChangeType.update)
                    {
                        ccp.ColorCodeID = oldccp.ColorCodeID;
                        ccp.ColorCodeName = oldccp.ColorCodeName;
                        _SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                    }
                    aHnp.NodeChangeType = eChangeType.update;
                    aHnp.ColorOrSizeCodeRID = oldccp.Key;
                    aHnp.NodeDescription = oldDescription;
                    ProcessNodeProfileInfo(ref em, aHnp);
                    _SAB.HierarchyServerSession.CommitData();
                    return false;
                }
                else
                {
                    //trans.DataAccess.CommitData();

                    switch (ccp.ColorCodeChangeType)
                    {
                        case eChangeType.add:
                            message = (string)_colorAdded.Clone();
                            message = message.Replace("{0}", ccp.ColorCodeID);
                            em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                            break;
                        case eChangeType.update:
                            message = (string)_existingColorUsed.Clone();
                            message = message.Replace("{0}", ccp.ColorCodeID);
                            message = message.Replace("{1}", oldccp.ColorCodeID);
                            em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                            break;
                        default:
                            message = (string)_colorRenamed.Clone();
                            message = message.Replace("{0}", oldccp.ColorCodeID);
                            message = message.Replace("{1}", ccp.ColorCodeID);
                            em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                            break;
                    }

                }
                return true;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
            //finally
            //{
            //    if (trans != null &&
            //        trans.DataAccess.ConnectionIsOpen)
            //    {
            //        trans.DataAccess.CloseUpdateConnection();
            //    }
            //}
        }
        // End TT#634

        /// <summary>
        /// Used to delete a product.
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aHierarchyID">The ID of the hierarchy</param>
        /// <param name="aParent">The ID of the parent</param>
        /// <param name="aNodeID">The ID of the node</param>
        /// <param name="aReplaceWithNode">The ID of the node to replace the node being delete</param>
        /// <param name="aForceDelete">Delete node even if certain data exists</param>
        /// <param name="aReclassPreview">a flag identifying if the process is previewing what reclass would do</param>
        /// <param name="aRollupForecastVersions">a Hashtable containing the record IDs of the forecast version to roll</param>
        /// <param name="aRollExternalIntransit">a flag identifying if the process is to schedule external intransit to roll</param>
        /// <param name="aRollAlternateHierarchies">a flag identifying if the process is to schedule alternate hierarchies to roll</param>
        // Begin Track #5259 - JSmith - Add new reclass roll options
        //		public void DeleteNode(ref EditMsgs em, string aHierarchyID, string aParent, string aNodeID,
        //			string aReplaceWithNode, bool aForceDelete, bool aReclassPreview, Hashtable aRollupForecastVersions)
        public void DeleteNode(ref EditMsgs em, string aHierarchyID, string aParent, string aNodeID,
            string aReplaceWithNode, bool aForceDelete, bool aReclassPreview, Hashtable aRollupForecastVersions,
            bool aRollExternalIntransit, bool aRollAlternateHierarchies)
        // End Track #5259
        {
            try
            {
                if (!_reclassTextRead)
                {
                    ReadReclassText();
                }

                string message = null;
                HierarchyNodeProfile replaceWithHnp = null;

                ValidateHierarchyID(ref em, ref aHierarchyID);

                ValidateParentID(ref em, ref aParent);

                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                //HierarchyNodeProfile hnp = ValidateNodeID(ref em, ref aNodeID);
                HierarchyNodeProfile hnp = ValidateNodeID(ref em, aParent, ref aNodeID);
                // End TT#298

                if ((_parent != null && _parent.Key != Include.NoRID) &&
                    (hnp != null && hnp.Key != Include.NoRID))
                {
                    if (!IsChild(_hp.Key, _parent.Key, hnp.Key))
                    {
                        //Begin TT#266 - JSmith - Hierarchy Reclass fails on rename during a move action
                        message = (string)_invalidParentChild.Clone();
                        message = message.Replace("{0}", aParent);
                        message = message.Replace("{1}", aNodeID);
                        message = message.Replace("{2}", aHierarchyID);
                        em.AddMsg(eMIDMessageLevel.Edit, message, sourceModule);
                        //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidParentChild, sourceModule);
                        //Begin TT#266
                    }
                }
                if (aReplaceWithNode != null)
                {
                    // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                    //replaceWithHnp = ValidateNodeID(ref em, ref aReplaceWithNode);
                    replaceWithHnp = ValidateNodeID(ref em, aParent, ref aReplaceWithNode);
                    // End TT#298
                }
                else
                {
                    replaceWithHnp = new HierarchyNodeProfile(Include.NoRID);
                }

                if (_parent.HomeHierarchyRID != _hp.Key)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotInHierarchy, sourceModule);
                }

                if (!em.ErrorFound &&
                    DeleteAllowed(ref em, hnp, false))
                {
                    bool continueDelete = true;
                    if (hnp.HomeHierarchyRID == _hp.Key)
                    {
                        // replace node being deleted with new node
                        DeletePreview(ref em, hnp, _parent, replaceWithHnp);
                        if (replaceWithHnp.Key != Include.NoRID &&
                            !aReclassPreview)
                        {
                            _SAB.HierarchyServerSession.ReplaceNode(hnp.Key, replaceWithHnp.Key);
                            _SAB.HierarchyServerSession.CommitData();
                        }

                        // delete descendants
                        //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                        //						NodeDescendantList ndl = _SAB.HierarchyServerSession.GetNodeDescendantList(hnp.Key);
                        NodeDescendantList ndl = _SAB.HierarchyServerSession.GetNodeDescendantList(hnp.Key, eNodeSelectType.All);
                        //End Track #4037
                        // all headers must be deleted first since they many depend on data from multiple nodes 
                        // delete headers of descendants at style level
                        for (int i = ndl.Count - 1; i >= 0; i--)
                        {
                            NodeDescendantProfile ndp = (NodeDescendantProfile)ndl[i];
                            HierarchyNodeProfile delNode = _SAB.HierarchyServerSession.GetNodeData(ndp.Key, false);
                            if (delNode.HomeHierarchyRID == hnp.HomeHierarchyRID &&
                                delNode.LevelType == eHierarchyLevelType.Style)
                            {
                                if (!DeleteHeaders(ref em, delNode, aForceDelete, aReclassPreview))
                                {
                                    continueDelete = false;
                                    message = MIDText.GetText(eMIDTextCode.msg_ReclassFailedShowPreview);
                                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                                    //									_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, hnp.Text, message);
                                    aReclassPreview = true;
                                }
                            }
                        }
                        // headers of main node if style level
                        if (continueDelete &&
                            hnp.LevelType == eHierarchyLevelType.Style)
                        {
                            if (!DeleteHeaders(ref em, hnp, aForceDelete, aReclassPreview))
                            {
                                continueDelete = false;
                                message = MIDText.GetText(eMIDTextCode.msg_ReclassFailedShowPreview);
                                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                                //								_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, hnp.Text, message);
                                aReclassPreview = true;
                            }
                        }

                        // delete other descendant data and nodes
                        for (int i = ndl.Count - 1; i >= 0; i--)
                        {
                            NodeDescendantProfile ndp = (NodeDescendantProfile)ndl[i];
                            HierarchyNodeProfile delNode = _SAB.HierarchyServerSession.GetNodeData(ndp.Key, false);
                            HierarchyNodeProfile delNodeParent = _SAB.HierarchyServerSession.GetNodeData(delNode.HomeHierarchyParentRID, false);
                            if (delNode.HomeHierarchyRID == hnp.HomeHierarchyRID)
                            {
                                DeletePreview(ref em, delNode, delNodeParent, null);
                                if (continueDelete &&
                                    !aReclassPreview)
                                {
                                    delNode.NodeChangeType = eChangeType.delete;
                                    ProcessNodeProfileInfo(ref em, delNode);
                                    if (_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                                    {
                                        _SAB.HierarchyServerSession.CommitData();
                                    }
                                    else
                                    {
                                        _SAB.HierarchyServerSession.OpenUpdateConnection();
                                    }
                                }
                                if (continueDelete)
                                {
                                    message = (string)_deleteConfirmed.Clone();
                                    message = message.Replace("{0}", delNode.Text);
                                    message = message.Replace("{1}", delNodeParent.Text);
                                    message = message.Replace("{2}", _hp.HierarchyID);
                                }
                                else
                                {
                                    message = (string)_deleteFailed.Clone();
                                    message = message.Replace("{0}", delNode.Text);
                                    message = message.Replace("{1}", delNodeParent.Text);
                                    message = message.Replace("{2}", _hp.HierarchyID);
                                }
                                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                                //								_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, hnp.Text, message);
                            }
                        }

                        // delete main node
                        // Begin Track #5259 - JSmith - Add new reclass roll options
                        //						ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, aReclassPreview, aRollupForecastVersions, true);
						// Begin TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        //ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, aReclassPreview, aRollupForecastVersions, true,
                        //    aRollExternalIntransit, aRollAlternateHierarchies);
                        ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, eHierarchyType.None, aReclassPreview, aRollupForecastVersions, true,
                            aRollExternalIntransit, aRollAlternateHierarchies);
                        // End TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        // End Track #5259
                        if (continueDelete)
                        {
                            if (continueDelete &&
                                !aReclassPreview)
                            {
                                hnp.NodeChangeType = eChangeType.delete;
                                try
                                {
                                    ProcessNodeProfileInfo(ref em, hnp);
                                    _SAB.HierarchyServerSession.CommitData();
                                }
                                catch
                                {
                                    continueDelete = false;
                                }
                            }
                        }

                        if (continueDelete)
                        {
                            message = (string)_deleteConfirmed.Clone();
                            message = message.Replace("{0}", hnp.Text);
                            message = message.Replace("{1}", _parent.Text);
                            message = message.Replace("{2}", _hp.HierarchyID);
                        }
                        else
                        {
                            message = (string)_deleteFailed.Clone();
                            message = message.Replace("{0}", hnp.Text);
                            message = message.Replace("{1}", _parent.Text);
                            message = message.Replace("{2}", _hp.HierarchyID);
                        }
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, hnp.Text, message);
                    }
                    // remove reference
                    else
                    {
                        if (!aReclassPreview)
                        {
                            HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                            hjp.JoinChangeType = eChangeType.delete;
                            hjp.OldHierarchyRID = _hp.Key;
                            hjp.OldParentRID = _parent.Key;
                            hjp.Key = hnp.Key;
                            HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(_hp.Key);
                            if (hp.HierarchyType == eHierarchyType.organizational)
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[hnp.NodeLevel];
                                hjp.LevelType = hlp.LevelType;
                            }
                            _SAB.HierarchyServerSession.JoinUpdate(hjp);
                            _SAB.HierarchyServerSession.CommitData();
                        }
                        // Begin Track #5259 - JSmith - Add new reclass roll options
                        //						ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, aReclassPreview, aRollupForecastVersions, true);
                        // Begin TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        //ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, aReclassPreview, aRollupForecastVersions, true,
                        //    aRollExternalIntransit, aRollAlternateHierarchies);
                        ScheduleReclassRollup(ref em, _actionDelete, hnp.Key, hnp.Text, _parent.Key, hnp.LevelType, Include.NoRID, null, eHierarchyType.None, aReclassPreview, aRollupForecastVersions, true,
                            aRollExternalIntransit, aRollAlternateHierarchies);
                        // End TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                        // End Track #5259

                        message = MIDText.GetText(eMIDTextCode.msg_ReclassRemoveConfirmed);
                        message = message.Replace("{0}", hnp.Text);
                        message = message.Replace("{1}", _parent.Text);
                        message = message.Replace("{2}", _hp.HierarchyID);
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, hnp.Text, message);
                    }
                }

            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        // Begin Track #5405 - JSmith - Reclass alternate delete not working
        //		/// <summary>
        //		/// Check to determine if the delete operation is valid
        //		/// </summary>
        //		/// <param name="em">An instance of the EditMsgs class where to put messages</param>
        //		/// <param name="aNode">
        //		/// An instance of the HierarchyNodeProfile class containing the node to be deleted
        //		/// </param>
        //		/// <param name="aOnline">A flag identifying if the method is called by the online client</param>
        //		/// <returns></returns>
        //		public bool DeleteAllowed(ref EditMsgs em, HierarchyNodeProfile aNode, bool aOnline)
        //		{
        //			try
        //			{
        //				string message = null;
        //				if (_parent == null || aNode.HomeHierarchyParentRID != _parent.Key)
        //				{
        //					_parent = _SAB.HierarchyServerSession.GetNodeData(aNode.HomeHierarchyParentRID);
        //					_parentID = _parent.NodeID;
        //				}
        //				if (_parent.Key == Include.NoRID)
        //				{
        //					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + aNode.HomeHierarchyParentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
        //					return false;
        //				}
        //
        //				if (_parent.HomeHierarchyType == eHierarchyType.organizational)
        //				{
        //					// make sure the ancestor style does not have any headers is level is color or size
        //					if (aNode.LevelType == eHierarchyLevelType.Color ||
        //						aNode.LevelType == eHierarchyLevelType.Size)
        //					{
        //						if (!aOnline)
        //						{
        //							message = GetMessage(eMIDTextCode.msg_ReclassNotPermittedBelowStyle);
        //							em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
        //							return false;
        //						}
        //
        //						Header HeaderData = new Header();
        //						HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
        //						DataTable headers = HeaderData.GetHeaders(styleNode.Key);
        //						if (headers.Rows.Count > 0)
        //						{
        //							message = (string)_styleHeaderDeleteFailed.Clone();
        //							message = message.Replace("{0}", aNode.Text);
        //							message = message.Replace("{1}", styleNode.Text);
        //							em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
        //							return false;
        //						}
        //					}
        //				}
        //				
        //				return true;
        //			}
        //			catch ( Exception err )
        //			{
        //				em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
        //				throw;
        //			}
        //		}

        /// <summary>
        /// Check to determine if the delete operation is valid
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aNode">
        /// An instance of the HierarchyNodeProfile class containing the node to be deleted
        /// </param>
        /// <param name="aOnline">A flag identifying if the method is called by the online client</param>
        /// <returns></returns>
        private bool DeleteAllowed(ref EditMsgs em, HierarchyNodeProfile aNode, bool aOnline)
        {
            HierarchyNodeProfile homeParent;
            try
            {
                string message = null;
                homeParent = _SAB.HierarchyServerSession.GetNodeData(aNode.HomeHierarchyParentRID);
                if (homeParent == null || homeParent.Key == Include.NoRID)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentRID=" + aNode.HomeHierarchyParentRID.ToString(CultureInfo.CurrentUICulture), sourceModule);
                    return false;
                }

                // Begin TT#1258 - JSmith - Cannot delete color level for KIP
                //if (homeParent.HomeHierarchyType == eHierarchyType.organizational)
                if (homeParent.HomeHierarchyType == eHierarchyType.organizational &&
                    aNode.HomeHierarchyRID == _hp.Key)
                // End TT#1258
                {
                    // Begin TT#634 - JSmith - Color rename
                    // make sure the ancestor style does not have any headers is level is color or size
                    //if (aNode.LevelType == eHierarchyLevelType.Color ||
                    //    aNode.LevelType == eHierarchyLevelType.Size)
                    //{
                    //    if (!aOnline)
                    //    {
                    //        message = GetMessage(eMIDTextCode.msg_ReclassNotPermittedBelowStyle);
                    //        em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                    //        return false;
                    //    }

                    //    Header HeaderData = new Header();
                    //    HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
                    //    DataTable headers = HeaderData.GetHeaders(styleNode.Key);
                    //    if (headers.Rows.Count > 0)
                    //    {
                    //        message = (string)_styleHeaderDeleteFailed.Clone();
                    //        message = message.Replace("{0}", aNode.Text);
                    //        message = message.Replace("{1}", styleNode.Text);
                    //        em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                    //        return false;
                    //    }
                    //}
                    // Begin TT#1045 - JSmith - No message when deleting style with active headers
                    if (aOnline)
                    {
                        if (aNode.LevelType == eHierarchyLevelType.Style)
                        {
                            Header HeaderData = new Header();
                            DataTable headers = HeaderData.GetHeaders(aNode.Key);
                            if (headers.Rows.Count > 0)
                            {
                                message = (string)_styleHeaderDeleteFailed.Clone();
                                message = message.Replace("{0}", aNode.Text);
                                message = message.Replace("{1}", aNode.Text);
                                em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                                return false;
                            }
                        }
                    }
                    // End TT#1045
                    if (aNode.LevelType == eHierarchyLevelType.Color)
                    {
                        Header HeaderData = new Header();
                        HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
                        DataTable headers = HeaderData.GetHeaders(styleNode.Key);
                        if (headers.Rows.Count > 0)
                        {
                            message = (string)_styleHeaderDeleteFailed.Clone();
                            message = message.Replace("{0}", aNode.Text);
                            message = message.Replace("{1}", styleNode.Text);
                            em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                            return false;
                        }
                    }
                    else if (aNode.LevelType == eHierarchyLevelType.Size)
                    {
                        if (!aOnline)
                        {
                            message = GetMessage(eMIDTextCode.msg_ReclassNotPermittedAtSize);
                            em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                            return false;
                        }

                        Header HeaderData = new Header();
                        HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
                        DataTable headers = HeaderData.GetHeaders(styleNode.Key);
                        if (headers.Rows.Count > 0)
                        {
                            message = (string)_styleHeaderDeleteFailed.Clone();
                            message = message.Replace("{0}", aNode.Text);
                            message = message.Replace("{1}", styleNode.Text);
                            em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                            return false;
                        }
                    }
                    // End TT#634
                }

                return true;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }
        // End Track #5405

        /// <summary>
        /// Check to determine if the rename operation is valid
        /// </summary>
        /// <param name="em">An instance of the EditMsgs class where to put messages</param>
        /// <param name="aNode">
        /// An instance of the HierarchyNodeProfile class containing the node to be deleted
        /// </param>
        /// <returns></returns>
        public bool RenameAllowed(ref EditMsgs em, HierarchyNodeProfile aNode)
        {
            try
            {
                string message = null;

                // Begin TT#634 - JSmith - Color rename
                //if (aNode.LevelType == eHierarchyLevelType.Color ||
                //    aNode.LevelType == eHierarchyLevelType.Size)
                //{
                //    message = GetMessage(eMIDTextCode.msg_ReclassNotPermittedBelowStyle);
                //    em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                //    return false;
                //}
                if (aNode.LevelType == eHierarchyLevelType.Size)
                {
                    message = GetMessage(eMIDTextCode.msg_ReclassNotPermittedAtSize);
                    em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                    return false;
                }
                // End TT#634

                return true;
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                throw;
            }
        }

        /// <summary>
        /// Check to determine if the move or copy operation is valid
        /// </summary>
        /// <param name="aNode">The node being copied or moved</param>
        /// <param name="aParent">The current parent node</param>
        /// <param name="aToParent">The new parent node</param>
        /// <param name="aOnline">A flag identifying if the method is called by the online client</param>
        /// <param name="aMessage">The output message indicating the error encountered, if any</param>
        /// <returns></returns>
        public bool DropAllowed(HierarchyNodeProfile aNode, HierarchyNodeProfile aParent,
            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
            //HierarchyNodeProfile aToParent, bool aOnline, out string aMessage)
            HierarchyNodeProfile aToParent, bool aOnline, out string aMessage, out eDropStates aDropStates)
           // End TT#2186
        {
            aMessage = "";
            HierarchyProfile fromParentHierarchy = null;
            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
            aDropStates = eDropStates.Allowed;
            // End TT#2186

            HierarchyProfile fromHierarchy = _SAB.HierarchyServerSession.GetHierarchyData(aNode.HomeHierarchyRID);
            if (aNode.HomeHierarchyLevel == 0)
            {
                fromParentHierarchy = _SAB.HierarchyServerSession.GetHierarchyData(aNode.HomeHierarchyRID);
            }
            else
            {
                fromParentHierarchy = _SAB.HierarchyServerSession.GetHierarchyData(aParent.HomeHierarchyRID);
            }
            HierarchyProfile toHierarchy = _SAB.HierarchyServerSession.GetHierarchyData(aToParent.HomeHierarchyRID);
            HierarchyLevelProfile fromhlp = (HierarchyLevelProfile)fromHierarchy.HierarchyLevels[aNode.NodeLevel];
            HierarchyLevelProfile tohlp = (HierarchyLevelProfile)toHierarchy.HierarchyLevels[aToParent.NodeLevel];

            if (aNode == null)
            {
                aMessage = _Session.Audit.GetText(eMIDTextCode.msg_NothingToPaste);
                return false;
            }
            // can't drag from Alternate to Organizational
            else if (fromParentHierarchy.HierarchyType == eHierarchyType.alternate &&
                toHierarchy.HierarchyType == eHierarchyType.organizational)
            {
                aMessage = _Session.Audit.GetText(eMIDTextCode.msg_InvalidAlternateCopy);
                return false;
            }
            // nodes must be same level in same Organizational hierarchy
            else if (toHierarchy.HierarchyType == eHierarchyType.organizational &&
                toHierarchy.Key == fromHierarchy.Key &&
                aParent.NodeLevel != aToParent.NodeLevel)
            {
                aMessage = _Session.Audit.GetText(eMIDTextCode.msg_MustBeSameLevel);
                return false;
            }
            // can not drop node in path if not in home hierarchy
            else if (aParent.HierarchyRID != aParent.HomeHierarchyRID)
            {
                aMessage = _Session.Audit.GetText(eMIDTextCode.msg_CanNotDropInSharedPath);
                return false;
            }
            // can not have color or size twice in same parent
            //			else if((aNode.HomeHierarchyParentRID == aToParent.Key) &&
            //				((fromhlp.LevelType == eHierarchyLevelType.Color) ||
            //				(fromhlp.LevelType == eHierarchyLevelType.Size)))
            //			{
            //				aMessage = _Session.Audit.GetText(eMIDTextCode.msg_CanNotCopyColorSizeToSelf);
            //				return false;
            //			}
            else if ((aNode.HomeHierarchyParentRID == aToParent.Key) && aNode.HomeHierarchyType == eHierarchyType.organizational)
            {
                if ((fromhlp.LevelType == eHierarchyLevelType.Color) ||
                    (fromhlp.LevelType == eHierarchyLevelType.Size))
                {
                    aMessage = _Session.Audit.GetText(eMIDTextCode.msg_CanNotCopyColorSizeToSelf);
                    return false;
                }
            }

            // check for circular relationship.  See if from node is in to node path
            else if (aParent.HomeHierarchyRID == aNode.HomeHierarchyRID &&
                toHierarchy.HierarchyType == eHierarchyType.alternate)
            {
                bool circularRelationship = false;
                NodeAncestorList nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToParent.Key);
                foreach (NodeAncestorProfile nap in nal)
                {
                    if (nap.Key == aNode.Key)
                    {
                        circularRelationship = true;
                        break;
                    }
                }
                if (circularRelationship)
                {
                    aMessage = _Session.Audit.GetText(eMIDTextCode.msg_CircularNodeRelationship);
                    return false;
                }
            }
            // can not move node from alternate unless parent is different hierarchy
            else if (aNode.HierarchyRID != aNode.HomeHierarchyRID &&
                aNode.HomeHierarchyRID == aParent.HomeHierarchyRID &&
                aNode.HomeHierarchyRID != aToParent.HomeHierarchyRID)
            {
                aMessage = _Session.Audit.GetText(eMIDTextCode.msg_CanNotMoveFromSharedPath);
                return false;
            }
            // Begin TT#2186 - JSmith - Errors during Hierarchy Load of Reclass files
            else if (_SAB.HierarchyServerSession.IsParentChild(aToParent.HomeHierarchyRID, aToParent.Key, aNode.Key))
            {
                aDropStates = eDropStates.Duplicate;
                return false;
            }
            // End TT#2186

            // make sure node conforms to level edits
            if ((fromHierarchy.HierarchyType == eHierarchyType.organizational &&  // if both organizational 
                toHierarchy.HierarchyType == eHierarchyType.organizational) &&
                (aNode.HomeHierarchyRID != aParent.HomeHierarchyRID)) // and not the same hierarchy, check levels
            {
                bool validLevels = true;
                int fromLevel = aNode.NodeLevel;
                int toLevel = aParent.NodeLevel + 1;  // to node is parent, so start at next level
                if ((fromHierarchy.HierarchyLevels.Count - fromLevel) != (toHierarchy.HierarchyLevels.Count - toLevel))
                {
                    validLevels = false;
                }
                else
                {
                    for (int i = fromLevel; i < fromHierarchy.HierarchyLevels.Count; i++, toLevel++)
                    {
                        fromhlp = (HierarchyLevelProfile)fromHierarchy.HierarchyLevels[i];
                        tohlp = (HierarchyLevelProfile)toHierarchy.HierarchyLevels[toLevel];
                        if (fromhlp.LevelType != tohlp.LevelType)
                        {
                            validLevels = false;
                        }
                        else
                            if (fromhlp.LevelRequiredSize != tohlp.LevelRequiredSize)
                            {
                                validLevels = false;
                            }
                            else
                                if (fromhlp.LevelSizeRangeFrom != tohlp.LevelSizeRangeFrom ||
                                fromhlp.LevelSizeRangeTo != tohlp.LevelSizeRangeTo)
                                {
                                    validLevels = false;
                                }
                    }
                }

                if (!validLevels)
                {
                    aMessage = _Session.Audit.GetText(eMIDTextCode.msg_LevelsDoNotMatch);
                    return false;
                }
            }

            // Begin TT#634 - JSmith - Color rename
            // if online,make sure the ancestor style does not have any headers is level is color or size
            // if reclass, disallow below style
            //if (aNode.LevelType == eHierarchyLevelType.Color ||
            //    aNode.LevelType == eHierarchyLevelType.Size)
            //{
            //    if (aOnline)
            //    {
            //        Header HeaderData = new Header();
            //        HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
            //        DataTable headers = HeaderData.GetHeaders(aNode.Key);
            //        if (headers.Rows.Count > 0)
            //        {
            //            aMessage = _Session.Audit.GetText(eMIDTextCode.msg_MoveFailedFromStyleHeaders, false);
            //            aMessage = aMessage.Replace("{0}", aNode.Text);
            //            aMessage = aMessage.Replace("{1}", styleNode.Text);
            //            return false;
            //        }
            //    }
            //    else if (toHierarchy.HierarchyType == eHierarchyType.organizational)
            //    {
            //        aMessage = _Session.Audit.GetText(eMIDTextCode.msg_ReclassNotPermittedBelowStyle, false);
            //        return false;
            //    }
            //}
            // Begin TT#1113 - JSmith - Reclassing alternate hierarchy with style on existing allocation header
            if (fromParentHierarchy.HierarchyType == eHierarchyType.organizational &&  // if both organizational 
                toHierarchy.HierarchyType == eHierarchyType.organizational)
            {
            // End TT#1113
                if (aNode.LevelType == eHierarchyLevelType.Color)
                {
                    Header HeaderData = new Header();
                    HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
                    DataTable headers = HeaderData.GetHeaders(styleNode.Key);
                    if (headers.Rows.Count > 0)
                    {
                        aMessage = _Session.Audit.GetText(eMIDTextCode.msg_MoveFailedFromStyleHeaders, false);
                        aMessage = aMessage.Replace("{0}", aNode.Text);
                        aMessage = aMessage.Replace("{1}", styleNode.Text);
                        return false;
                    }
                }
                else if (aNode.LevelType == eHierarchyLevelType.Size)
                {
                    if (aOnline)
                    {
                        Header HeaderData = new Header();
                        HierarchyNodeProfile styleNode = _SAB.HierarchyServerSession.GetAncestorData(aNode.HomeHierarchyRID, aNode.Key, eHierarchyLevelType.Style);
                        DataTable headers = HeaderData.GetHeaders(styleNode.Key);
                        if (headers.Rows.Count > 0)
                        {
                            aMessage = _Session.Audit.GetText(eMIDTextCode.msg_MoveFailedFromStyleHeaders, false);
                            aMessage = aMessage.Replace("{0}", aNode.Text);
                            aMessage = aMessage.Replace("{1}", styleNode.Text);
                            return false;
                        }
                    }
                    else if (toHierarchy.HierarchyType == eHierarchyType.organizational)
                    {
                        aMessage = _Session.Audit.GetText(eMIDTextCode.msg_ReclassNotPermittedAtSize, false);
                        return false;
                    }
                }
                // End TT#634
            // Begin TT#1113 - JSmith - Reclassing alternate hierarchy with style on existing allocation header
            }
            // End TT#1113

            return true;
        }

        private bool ValidateHierarchyID(ref EditMsgs em, ref string aHierarchyID)
        {
            try
            {
                if (aHierarchyID == null)
                {
                    em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, sourceModule);
                    _hierarchyNotFound = true;
                }
                else
                {
                    aHierarchyID = aHierarchyID.Trim();
                    if (_hp == null || aHierarchyID != _hp.HierarchyID)
                    {
                        _hp = _SAB.HierarchyServerSession.GetHierarchyData(aHierarchyID);
                        if (_hp.Key == -1)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                            _hierarchyNotFound = true;
                        }
                        else
                        {
                            _hierarchyNotFound = false;
                            _hierarchyLevels = _hp.HierarchyLevels;
                        }
                    }
                    else
                        if (_hierarchyNotFound)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HierarchyNotFound, sourceModule);
                        }
                }

                if (_hierarchyNotFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                return false;
            }
        }

        private bool ValidateParentID(ref EditMsgs em, ref string aParentID)
        {
            try
            {
                if (aParentID == null)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentRequired, sourceModule);
                    _parentNotFound = true;
                }
                else
                {
                    aParentID = aParentID.Trim();
                    if (_parent == null || aParentID != _parentID)
                    {
                        _parent = (HierarchyNodeProfile)_parentIDHash[aParentID];
                        if (_parent == null)
                        {
                            _parent = NodeLookup(ref em, aParentID, false, false);
                            if (_parent.Key != Include.NoRID)
                            {
                                _parentIDHash[aParentID] = _parent;
                            }
                        }
                        _parentID = aParentID;
                        if (_parent.Key == Include.NoRID)  // parent does not exist
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + aParentID, sourceModule);
                            _parentNotFound = true;
                        }
                        else
                        {
                            _parentNotFound = false;
                        }
                    }
                    else
                        if (_parentNotFound)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, "ParentID=" + aParentID, sourceModule);
                        }
                }

                if (_parentNotFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
                return false;
            }
        }

        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
        //// Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
        ////private HierarchyNodeProfile ValidateNodeID(ref EditMsgs em, ref string aNodeID)
        //private HierarchyNodeProfile ValidateNodeID(ref EditMsgs em, string aParent, ref string aNodeID)
        //// End TT#298
        //{
        //    HierarchyNodeProfile hnp = new HierarchyNodeProfile(Include.NoRID);
        //    try
        //    {
        //        if (aNodeID == null)
        //        {
        //            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductRequired, sourceModule);
        //        }
        //        else
        //        {
        //            aNodeID = aNodeID.Trim();
        //            hnp = NodeLookup(ref em, aNodeID, false, false);

        //            // Begin TT#701 - JSmith - Error adding sales and inventory files 
        //            //if (hnp.Key == Include.NoRID)  // node does not exist
        //            //{
        //            //    // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
        //            //    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
        //            //    // check if color or size
        //            //    hnp = NodeLookup(ref em, aParent + _nodeDelimiter + aNodeID, false, false);
        //            //    if (hnp.Key == Include.NoRID)  // node does not exist
        //            //    {
        //            //        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
        //            //    }
        //            //    // End TT#298
        //            //}
        //            // Begin TT#741 - stodd - rename above style not working
        //            if (hnp.Key == Include.NoRID)
        //            {
        //                if (_parent.LevelType == eHierarchyLevelType.Style ||
        //                _parent.LevelType == eHierarchyLevelType.Color)  // node does not exist
        //                {
        //                    // check if color or size
        //                    hnp = NodeLookup(ref em, aParent + _nodeDelimiter + aNodeID, false, false);
        //                    if (hnp.Key == Include.NoRID)  // node does not exist
        //                    {
        //                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
        //                    }
        //                }
        //                else
        //                {
        //                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
        //                }
        //            }
        //            // End TT#741 - stodd - rename above style not working
        //            // End TT#701
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
        //    }
        //    return hnp;
        //}
		
        private HierarchyNodeProfile GetNodeKey(ref EditMsgs em, string aParent, ref string aNodeID)
        {
            HierarchyNodeProfile hnp = null;
            string nodeID;
            int nodeRID;

            if (_parent.LevelType == eHierarchyLevelType.Style ||
                    _parent.LevelType == eHierarchyLevelType.Color)
            {
                nodeID = aParent + _nodeDelimiter + aNodeID;
            }
            else
            {
                nodeID = aNodeID;
            }

            nodeRID = _SAB.HierarchyServerSession.GetNodeRID(nodeID);

            if (nodeRID != Include.NoRID)
            {
                // populate only fields needed for processing
                hnp = new HierarchyNodeProfile(nodeRID);
                hnp.NodeID = aNodeID;
                hnp.HomeHierarchyParentRID = _parent.Key;
                hnp.HomeHierarchyRID = _parent.HomeHierarchyRID;
            }
            else
            {
                hnp = ValidateNodeID(ref em, aParent, ref aNodeID);
            }

            return hnp;
        }

        // Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
		private HierarchyNodeProfile ValidateNodeID(ref EditMsgs em, string aParent, ref string aNodeID)
        {
            HierarchyNodeProfile hnp = null;
            try
            {
                if (aNodeID == null)
                {
                    hnp = new HierarchyNodeProfile(Include.NoRID);
                    em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductRequired, sourceModule);
                }
                else
                {
                    aNodeID = aNodeID.Trim();

                    if (_parent.LevelType == eHierarchyLevelType.Style ||
                    _parent.LevelType == eHierarchyLevelType.Color)  // node does not exist
                    {
                        // check if color or size
                        hnp = NodeLookup(ref em, aParent + _nodeDelimiter + aNodeID, false, false);
                    }
                    else
                    {
                        hnp = NodeLookup(ref em, aNodeID, false, false);
                    }
                    if (hnp.Key == Include.NoRID)
                    {
                        hnp = new HierarchyNodeProfile(Include.NoRID);
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, sourceModule);
                    }
                }
            }
            catch (Exception err)
            {
                em.AddMsg(eMIDMessageLevel.Error, err.Message, sourceModule);
            }
            return hnp;
        }
        // End TT#3523 - JSmith - Performance of Anthro morning processing jobs

        private bool IsChild(int aHierarchyRID, int aParentRID, int aNodeRID)
        {
            try
            {
                return _SAB.HierarchyServerSession.IsParentChild(aHierarchyRID, aParentRID, aNodeRID);
                //				HierarchyNodeList hnl = _SAB.HierarchyServerSession.GetDescendantData(aParentRID, 1);
                //				foreach (HierarchyNodeProfile hnp in hnl)
                //				{
                //					if (hnp.Key == aNodeRID)
                //					{
                //						return true;
                //					}
                //				}
                //				return false;

            }
            catch
            {
                throw;
            }
        }

        private bool DeleteHeaders(ref EditMsgs em, HierarchyNodeProfile aHnp, bool aForceDelete, bool aReclassPreview)
        {
            Header HeaderData = null;
            ApplicationSessionTransaction purgeTrans = null;  // TT#4216 - JSmith - Hierarchy Load Error 
            try
            {
                purgeTrans = _SAB.ApplicationServerSession.CreateTransaction(_SAB.ApplicationServerSession);  // TT#4216 - JSmith - Hierarchy Load Error
                string message = null;
                bool headersDeleted = true;
                Hashtable headers = new Hashtable();

                HeaderData = new Header();
                HeaderData.OpenUpdateConnection();
                DataTable styleHeaders = HeaderData.GetHeaders(aHnp.Key);
                DataTable planHeaders = HeaderData.GetPlanNodeHeaders(aHnp.Key);
                DataTable onHandHeaders = HeaderData.GetOnHandNodeHeaders(aHnp.Key);
                // product is in use and can not be deleted
                if (planHeaders.Rows.Count > 0 ||
                    onHandHeaders.Rows.Count > 0)
                {
                    foreach (DataRow dr in planHeaders.Rows)
                    {
                        // only a problem if plan key not the same as style key
                        int styleHnRID = Convert.ToInt32(dr["STYLE_HNRID"], CultureInfo.CurrentCulture);
                        if (styleHnRID != aHnp.Key)
                        {
                            message = (string)_inUseByHeader.Clone();
                            message = message.Replace("{0}", aHnp.Text);
                            message = message.Replace("{1}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                            em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                            //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                            int headerRID = Convert.ToInt32(dr["HDR_RID"]);
                            if (!_rejectedHeaders.Contains(headerRID))
                            {
                                AllocationProfile ap = new AllocationProfile(_SAB, null, headerRID, _Session);
                                HeaderData.WriteReclassRejectedHeader(_Session.Audit.ProcessRID, ap.Key, ap.HeaderID, MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                                _rejectedHeaders.Add(ap.Key, null);
                            }
                            headersDeleted = false;
                        }
                    }
                    foreach (DataRow dr in onHandHeaders.Rows)
                    {
                        // only a problem if onhand key not the same as style key
                        int styleHnRID = Convert.ToInt32(dr["STYLE_HNRID"], CultureInfo.CurrentCulture);
                        if (styleHnRID != aHnp.Key)
                        {
                            message = (string)_inUseByHeader.Clone();
                            message = message.Replace("{0}", aHnp.Text);
                            message = message.Replace("{1}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                            em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                            //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                            int headerRID = Convert.ToInt32(dr["HDR_RID"]);
                            if (!_rejectedHeaders.Contains(headerRID))
                            {
                                AllocationProfile ap = new AllocationProfile(_SAB, null, headerRID, _Session);
                                HeaderData.WriteReclassRejectedHeader(_Session.Audit.ProcessRID, ap.Key, ap.HeaderID, MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                                _rejectedHeaders.Add(ap.Key, null);
                            }
                            headersDeleted = false;
                        }
                    }
                    //					headersDeleted = false;
                }
                // check if can delete headers. It is all or none
                foreach (DataRow dr in styleHeaders.Rows)
                {
                    int headerRID = Convert.ToInt32(dr["HDR_RID"]);
                    string headerID = Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture);
                    // Begin TT#1031 - JSmith - Serializaion error in audit
                    //ApplicationSessionTransaction trans = new ApplicationSessionTransaction(_SAB);
                    // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
                    // Begin TT#5 MD - JSmith - Error during Hierarchy Load of Reclass Delete Transactions
                    //ApplicationSessionTransaction trans = _SAB.ApplicationServerSession.CreateTransaction(_Session);
                    //ApplicationSessionTransaction trans = _SAB.ApplicationServerSession.CreateTransaction(_SAB.ApplicationServerSession);  // TT#4216 - JSmith - Hierarchy Load Error
                    // End TT#5 MD
                    List<int> hdrRidList = new List<int>();
                    hdrRidList.Add(headerRID);
                    // BEGIN TT#115 - stodd - exception removing placeholder
					// Begin TT#4216 - JSmith - Hierarchy Load Error
					//AllocationProfile ap = new AllocationProfile(trans, headerID, headerRID, _Session);
                    // END TT#115 - stodd - exception removing placeholder
                    //if (trans.EnqueueHeaders(hdrRidList, out message))  
                    AllocationProfile ap = new AllocationProfile(purgeTrans, headerID, headerRID, _Session);
                    if (purgeTrans.EnqueueHeaders(hdrRidList, out message))
					// End TT#4216 - JSmith - Hierarchy Load Error
                    {
                        //ApplicationSessionTransaction trans = new ApplicationSessionTransaction(_SAB, _Session);
                        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)
                        // Begin TT#1031 - JSmith - Serializaion error in audit
                        // BEGIN TT#115 - stodd - exception removing placeholder
                        //AllocationProfile ap = new AllocationProfile(trans, headerID, headerRID, _Session);
                        // END TT#115 - stodd - exception removing placeholder
                        if (ap.InUseByMulti ||
                            ap.MultiHeader ||
                            ap.MasterRID != Include.NoRID ||
                            ap.SubordinateRID != Include.NoRID)
                        {
                            message = GetMessage(eMIDTextCode.msg_HeaderDeleteNotAllowedOnMultiOrMaster);
                            message = message.Replace("{0}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                            em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                            //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                            if (!_rejectedHeaders.Contains(ap.Key))
                            {
                                HeaderData.WriteReclassRejectedHeader(_Session.Audit.ProcessRID, ap.Key, ap.HeaderID, MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                                _rejectedHeaders.Add(ap.Key, null);
                            }
                            headersDeleted = false;
                        }
                        else if ((ap.Released && ap.ShippingComplete) ||
                            ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedInBalance ||
                            aForceDelete)
                        {
                            headers.Add(ap.Key, ap);
                        }
                        else
                        {
                            message = (string)_headerDeleteFailedFromStatus.Clone();
                            message = message.Replace("{0}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                            em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                            //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                            if (!_rejectedHeaders.Contains(ap.Key))
                            {
                                HeaderData.WriteReclassRejectedHeader(_Session.Audit.ProcessRID, ap.Key, ap.HeaderID, MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                                _rejectedHeaders.Add(ap.Key, null);
                            }
                            headersDeleted = false;
                        }
                        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
                    }
                    else
                    {
                        em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                        if (!_rejectedHeaders.Contains(headerRID))
                        {
                            // BEGIN TT#115 - stodd - exception removing placeholder
                            HeaderData.WriteReclassRejectedHeader(_Session.Audit.ProcessRID, headerRID, headerID, MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                            // END TT#115 - stodd - exception removing placeholder
                            _rejectedHeaders.Add(headerRID, null);
                        }
                        headersDeleted = false;
                    }
                    // end TT#1185 - JEllis - Verify ENQ before Update (part 2)
                }
                if (headersDeleted)
                {
                    foreach (DataRow dr in styleHeaders.Rows)
                    {
                        int headerRID = Convert.ToInt32(dr["HDR_RID"]);
                        AllocationProfile ap = (AllocationProfile)headers[headerRID];
                        bool forceDelete = false;
                        if ((ap.Released && ap.ShippingComplete) ||
                            !aForceDelete)
                        {
                            message = (string)_headerDelete.Clone();
                        }
                        else
                        {
                            message = (string)_headerDeleteForced.Clone();
                            forceDelete = true;
                        }
                        message = message.Replace("{0}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        //						_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                        if (!aReclassPreview)
                        {
                            //ApplicationSessionTransaction purgeTrans = _SAB.ApplicationServerSession.CreateTransaction();  // TT#4216 - JSmith - Hierarchy Load Error
                            //purgeTrans.NewAllocationMasterProfileList();  // TT#4344 - JSmith - Deleting Hierarchy Nodes
                            // Begin TT#2252 - JSmith - Error during Hierarchy Load of Reclass Delete Transactions
                            //int[] headerArray = new int[1];
                            //headerArray[0] = ap.Key;

                            SelectedHeaderList headerArray = new SelectedHeaderList(eProfileType.SelectedHeader);
                            SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ap.Key);
                            selectedHeader.HeaderType = ap.HeaderType;
                            headerArray.Add(selectedHeader);
                            // End TT#2252
                            purgeTrans.LoadHeaders(headerArray);
                            if (!purgeTrans.PurgeAllocationHeader(ap.Key, forceDelete))
                            {
                                headersDeleted = false;
                                message = (string)_headerDeleteFailed.Clone();
                                message = message.Replace("{0}", Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture));
                                em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
                                //								_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                            }
                        }
                    }
                }
                //				if (headersDeleted)
                //				{
                HeaderData.CommitData();
                //				}
                return headersDeleted;
            }
            catch
            {
                throw;
            }
            finally
            {
			    // Begin TT#4216 - JSmith - Hierarchy Load Error
                if (purgeTrans != null)
                {
                    purgeTrans.DequeueHeaders();
                }
				// End TT#4216 - JSmith - Hierarchy Load Error
                if (HeaderData != null &&
                    HeaderData.ConnectionIsOpen)
                {
                    HeaderData.CloseUpdateConnection();
                }
            }
        }

        // Begin TT#634 - JSmith - Color rename
        //private bool UpdateBinRecords(ref EditMsgs em, HierarchySessionTransaction aTrans, 
        //    HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        //{
        //    StoreVariableHistoryBin dlStoreVarHist;
        //    try
        //    {
        //        bool updateSuccessful = true;
        //        dlStoreVarHist = new StoreVariableHistoryBin(true, 0, aTrans.DataAccess);
        //        dlStoreVarHist.UpdateStyleColorOnDayBin(aHnp.HomeHierarchyParentRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //        dlStoreVarHist.UpdateStyleColorOnWeekBin(aHnp.HomeHierarchyParentRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);

        //        return updateSuccessful;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private bool UpdateColorOnHeaders(ref EditMsgs em, HierarchySessionTransaction aTrans,
        //    HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        //{
        //    Header HeaderData = null;
        //    HeaderEnqueue headerEnqueue = null;
        //    AllocationHeaderProfile ahp;
        //    AllocationHeaderProfileList headerList;
        //    ApplicationSessionTransaction trans = null;
        //    AllocationProfileList apl;
        //    AllocationProfileList multiHeaderChildrenList;
        //    bool headerContainsColor = false;
        //    bool headerUpdateFailed = false;
        //    bool foundBulkColor = false;
        //    ArrayList packs = null;

        //    try
        //    {
        //        string message = null;
        //        bool headersUpdated = true;
        //        apl = new AllocationProfileList(eProfileType.AllocationHeader);

        //        HeaderData = new Header();

        //        DataTable styleHeaders = HeaderData.GetHeaders(aHnp.HomeHierarchyParentRID);

        //        if (styleHeaders.Rows.Count > 0)
        //        {
        //            // enqueue all headers for style that contain the color to make sure they can be updated
        //            headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
        //            foreach (DataRow dr in styleHeaders.Rows)
        //            {
        //                headerContainsColor = false;
        //                int headerRID = Convert.ToInt32(dr["HDR_RID"]);
        //                string headerID = Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture);
        //                trans = _SAB.ApplicationServerSession.CreateTransaction();
        //                //trans = new ApplicationSessionTransaction(_SAB);
        //                //AllocationProfileList apl2 = new AllocationProfileList(eProfileType.Allocation);
        //                AllocationProfile ap = new AllocationProfile(trans, headerID, headerRID, _Session);
        //                //apl2.Add(ap);
        //                //trans.SetMasterProfileList(apl2);
        //                //ap.LoadStores();

        //                if (ap.MultiHeader)
        //                {
        //                    continue;
        //                }

        //                if (ap.BulkColors != null && ap.BulkColors.Count > 0)
        //                {
        //                    foreach (HdrColorBin aColor in ap.BulkColors.Values)
        //                    {
        //                        if (aColor.ColorCodeRID == aOldColorCodeProfile.Key)
        //                        {
        //                            headerContainsColor = true;
        //                            break;
        //                        }
        //                    }
        //                }

        //                if (!headerContainsColor)
        //                {
        //                    if ((ap.Packs != null && ap.Packs.Count > 0))
        //                    {
        //                        foreach (PackHdr aPack in ap.Packs.Values)
        //                        {
        //                            foreach (PackColorSize pcs in aPack.PackColors.Values)
        //                            {
        //                                if (pcs.ColorCodeRID == aOldColorCodeProfile.Key)
        //                                {
        //                                    headerContainsColor = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                if (headerContainsColor)
        //                {
        //                    // add header to process list
        //                    apl.Add(ap);
        //                    // add header to enqueue list
        //                    ahp = new AllocationHeaderProfile(headerRID);
        //                    headerList.Add(ahp);
        //                    // if in use by multi, add multi to enqueue list
        //                    if (ap.InUseByMulti)
        //                    {
        //                        em.AddMsg(eMIDMessageLevel.Severe, "Cannot rename color on a header that is part of a multi", sourceModule);
        //                        return false;
        //                        //ahp = new AllocationHeaderProfile(ap.HeaderGroupRID);
        //                        //headerList.Add(ahp);
        //                    }
        //                }
        //            }

        //            // no headers contain color, so just return
        //            if (headerList.Count == 0)
        //            {
        //                return true;
        //            }

        //            try
        //            {
        //                trans = _SAB.ApplicationServerSession.CreateTransaction();
        //                headerEnqueue = new HeaderEnqueue(trans, headerList);
        //                headerEnqueue.EnqueueHeaders();
        //            }
        //            catch (HeaderConflictException)
        //            {
        //                DisplayEnqueueConflict(ref em, headerEnqueue, apl);
        //                return false;
        //            }

        //            //HeaderData.OpenUpdateConnection();
        //            // create new object using open database transaction
        //            HeaderData = new Header(aTrans.DataAccess);
        //            // replace colors on header
        //            foreach (AllocationProfile ap in apl)
        //            {
        //                // set the data layer so all use the same database transaction
        //                ap.HeaderDataRecord = HeaderData;

        //                if (ap.InUseByMulti)
        //                {
        //                    em.AddMsg(eMIDMessageLevel.Severe, "Cannot rename color on a header that is part of a multi", sourceModule);
        //                    return false;
        //                    //AllocationProfile multiHeaderProfile = new AllocationProfile(ap.AppSessionTransaction, null, ap.HeaderGroupRID, _Session);
        //                    //multiHeaderProfile.LoadStores();
        //                    //multiHeaderProfile.RemoveHeaderFromMulti(multiHeaderProfile, ap);
        //                    //message = (string)_headerRemovedFromMulti.Clone();
        //                    //message = message.Replace("{0}", ap.HeaderID);
        //                    //message = message.Replace("{1}", multiHeaderProfile.HeaderID);
        //                    //em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);

        //                    //// delete multi if no children left
        //                    //multiHeaderChildrenList = multiHeaderProfile.GetMultiHeaderChildren();
        //                    //if (multiHeaderChildrenList.Count == 0)
        //                    //{
        //                    //    multiHeaderProfile.DeleteHeader();
        //                    //    message = (string)_multiHeaderDeleted.Clone();
        //                    //    message = message.Replace("{0}", multiHeaderProfile.HeaderID);
        //                    //    em.AddMsg(eMIDMessageLevel.Warning, message, sourceModule);
        //                    //}
        //                }

        //                if (ap.BulkColors != null && ap.BulkColors.Count > 0)
        //                {
        //                    foundBulkColor = false;
        //                    foreach (HdrColorBin aColor in ap.BulkColors.Values)
        //                    {
        //                        if (aColor.ColorCodeRID == aOldColorCodeProfile.Key)
        //                        {
        //                            foundBulkColor = true;
        //                        }
        //                        else if (aColor.ColorCodeRID == aNewColorCodeProfile.Key)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, "New color already on header", sourceModule);
        //                            headerUpdateFailed = true;
        //                        }
        //                    }
        //                    if (!headerUpdateFailed &&
        //                        foundBulkColor)
        //                    {
        //                        try
        //                        {
        //                            HeaderData.UpdateBulkColorOnHeader(ap.Key, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //                            //ap.SetBulkColorCodeRID(aOldColorCodeProfile.Key, aNewColorCodeProfile.Key, true);
        //                        }
        //                        catch (MIDException ex)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, sourceModule);
        //                            headerUpdateFailed = true;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, ex.Message, sourceModule);
        //                            headerUpdateFailed = true;
        //                        }
        //                    }
        //                }

        //                if ((ap.Packs != null && ap.Packs.Count > 0))
        //                {
        //                    packs = new ArrayList();
        //                    foreach (PackHdr aPack in ap.Packs.Values)
        //                    {
        //                        foreach (PackColorSize pcs in aPack.PackColors.Values)
        //                        {
        //                            if (pcs.ColorCodeRID == aOldColorCodeProfile.Key)
        //                            {
        //                                packs.Add(aPack);
        //                            }
        //                            else if (pcs.ColorCodeRID == aNewColorCodeProfile.Key)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, "New color already on header", sourceModule);
        //                                headerUpdateFailed = true;
        //                            }
        //                        }
        //                    }
        //                    if (!headerUpdateFailed)
        //                    {
        //                        foreach (PackHdr aPack in packs)
        //                        {
        //                            try
        //                            {
        //                                HeaderData.UpdatePackColorOnHeader(aPack.PackRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //                                //ap.SetPackColorCodeRID(aPack, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key, true);
        //                            }
        //                            catch (MIDException ex)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, sourceModule);
        //                                headerUpdateFailed = true;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, ex.Message, sourceModule);
        //                                headerUpdateFailed = true;
        //                            }
        //                        }
        //                    }
        //                }

        //                if (headerUpdateFailed)
        //                {
        //                    headersUpdated = false;
        //                    message = (string)_headerColorRenameFailed.Clone();
        //                    message = message.Replace("{0}", aOldColorCodeProfile.ColorCodeID);
        //                    message = message.Replace("{1}", ap.HeaderID);
        //                    em.AddMsg(eMIDMessageLevel.Error, message, sourceModule);
        //                }
        //            }
        //        }

        //        return headersUpdated;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        //if (HeaderData != null &&
        //        //    HeaderData.ConnectionIsOpen)
        //        //{
        //        //    HeaderData.CloseUpdateConnection();
        //        //}

        //        if (headerEnqueue != null)
        //        {
        //            DequeueHeaders(headerEnqueue);
        //        }
        //    }
        //}

        //private void DisplayEnqueueConflict(ref EditMsgs em, HeaderEnqueue aHeaderEnqueue, AllocationProfileList apl)
        //{
        //    SecurityAdmin secAdmin = new SecurityAdmin();
        //    string errMsg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_HeadersInUse, false) + ":" + System.Environment.NewLine;

        //    foreach (HeaderConflict hdrCon in aHeaderEnqueue.HeaderConflictList)
        //    {
        //        AllocationProfile ap = (AllocationProfile)apl.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
        //        errMsg += System.Environment.NewLine + ap.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
        //    }
        //    errMsg += System.Environment.NewLine + System.Environment.NewLine;

        //    em.AddMsg(eMIDMessageLevel.Severe, errMsg, sourceModule);

        //}

        //public void DequeueHeaders(HeaderEnqueue aHeaderEnqueue)
        //{
        //    try
        //    {
        //        if (aHeaderEnqueue != null)
        //        {
        //            aHeaderEnqueue.DequeueHeaders();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#634

        private void DeletePreview(ref EditMsgs em, HierarchyNodeProfile aHnp,
            HierarchyNodeProfile aParent, HierarchyNodeProfile aReplaceWithNode)
        {
            try
            {
                string message = null;
                DataTable dt = null;

                // allocation workspace filter
                //Begin TT#1313-MD -jsobek -Header Filters
                //AllocationWorkspaceFilterData AllocationWorkspaceFilterData = new AllocationWorkspaceFilterData();
                //dt = AllocationWorkspaceFilterData.AllocationWorkspaceFilter_NodeRead(aHnp.Key);

                FilterData filterData = new FilterData();
                dt = filterData.WorkspaceFilterNodeRead(aHnp.Key, eWorkspaceType.AllocationWorkspace);           
                foreach (DataRow dr in dt.Rows)
                {
                    string filterNameLabel = _workspaceFilterLabel + " - " + Convert.ToString(dr["FILTER_NAME"], CultureInfo.CurrentCulture);
                    if (aReplaceWithNode != null && aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), filterNameLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), filterNameLabel);
                    }
                }


                //Lets add the assortment workspace filter as well
                dt = filterData.WorkspaceFilterNodeRead(aHnp.Key, eWorkspaceType.AssortmentWorkspace);
                foreach (DataRow dr in dt.Rows)
                {
                    string filterNameLabel = _workspaceFilterLabel + " (Assortment) - " + Convert.ToString(dr["FILTER_NAME"], CultureInfo.CurrentCulture);
                    if (aReplaceWithNode != null && aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), filterNameLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), filterNameLabel);
                    }
                }


                //End TT#1313-MD -jsobek -Header Filters

                // allocation view selection
                AllocationSelection AllocationSelectionData = new AllocationSelection();
                dt = AllocationSelectionData.GetDistinctNodeUsersSelection(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _allocationViewSelectLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _allocationViewSelectLabel);
                    }
                }

                // OTS forecast view selection
                OTSPlanSelection OTSPlanSelectionData = new OTSPlanSelection();
                dt = OTSPlanSelectionData.GetDistinctChainNodeUsersSelection(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                }

                dt = OTSPlanSelectionData.GetDistinctStoreNodeUsersSelection(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                }

                dt = OTSPlanSelectionData.GetDistinctBasisNodeUsersSelection(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastViewSelectLabel);
                    }
                }

                // OTS Plan Method 
                OTSPlanMethodData OTSPlanMethodData = new OTSPlanMethodData();
                dt = OTSPlanMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastMethodLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        string methodName = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture);
                        AddDeleteMethodMessage(ref em, methodName,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), _OTSForecastMethodLabel);
                        DeleteMethodPreview(ref em, Convert.ToInt32(dr["METHOD_RID"]), methodName, _OTSForecastMethodLabel);
                    }
                }

                // BEGIN Issue 4818 stodd 1.9.2008 Discrete Time periods
                GroupLevelBasis BasisPlanData = new GroupLevelBasis();
                // END Issue 4818
                dt = BasisPlanData.GetBasisByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastMethodLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _OTSForecastMethodLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // Override Method 
                AllocationOverrideMethodData AllocationOverrideMethodData = new AllocationOverrideMethodData();
                dt = AllocationOverrideMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodOverrideLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodOverrideLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                dt = AllocationOverrideMethodData.GetMethodsByOnHandNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodOverrideLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodOverrideLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // General Allocation Method 
                GeneralAllocationMethodData GeneralAllocationMethodData = new GeneralAllocationMethodData();
                dt = GeneralAllocationMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodGeneralAllocationLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodGeneralAllocationLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // Size Need Method 
                MethodSizeNeedData MethodSizeNeedData = new MethodSizeNeedData();
                dt = MethodSizeNeedData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodSizeNeedLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodSizeNeedLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // Velocity Method 
                VelocityMethodData VelocityMethodData = new VelocityMethodData();
                dt = VelocityMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodVelocityLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodVelocityLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                dt = VelocityMethodData.GetMethodsBasisByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodVelocityLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodVelocityLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // Matrix Method 
                OTSForecastBalanceMethodData OTSForecastBalanceMethodData = new OTSForecastBalanceMethodData();
                dt = OTSForecastBalanceMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastBalanceLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        //						AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                        //							_methodForecastBalanceLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                        string methodName = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture);
                        AddDeleteMethodMessage(ref em, methodName,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), _methodForecastBalanceLabel);
                        DeleteMethodPreview(ref em, Convert.ToInt32(dr["METHOD_RID"]), methodName, _methodForecastBalanceLabel);
                    }
                }

                // Begin TT#252 - JSmith - Invalid object name 'METHOD_MATRIX_VERSION_OVERRIDE"
                //dt = OTSForecastBalanceMethodData.GetMethodsOverrideByNode(aHnp.Key);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (aReplaceWithNode != null &&
                //        aReplaceWithNode.Key != Include.NoRID)
                //    {
                //        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                //            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                //            _methodForecastBalanceLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                //    }
                //    else
                //    {
                //        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                //            _methodForecastBalanceLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                //    }
                //}
                // End TT#252

                // Copy Method 
                OTSForecastCopyMethodData OTSForecastCopyMethodData = new OTSForecastCopyMethodData();
                dt = OTSForecastCopyMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    string label = ((ePlanType)Convert.ToChar(dr["PLAN_TYPE"], CultureInfo.CurrentCulture) == ePlanType.Chain) ? _methodForecastCopyChainLabel : _methodForecastCopyStoreLabel;
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            label + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        //						AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                        //							label + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                        string methodName = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture);
                        AddDeleteMethodMessage(ref em, methodName,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), label);
                        DeleteMethodPreview(ref em, Convert.ToInt32(dr["METHOD_RID"]), methodName, label);
                    }
                }

                dt = OTSForecastCopyMethodData.GetMethodsBasisByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    string label = ((ePlanType)Convert.ToChar(dr["PLAN_TYPE"], CultureInfo.CurrentCulture) == ePlanType.Chain) ? _methodForecastCopyChainLabel : _methodForecastCopyStoreLabel;
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastCopyChainLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastCopyChainLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                //Begin Enhancement - JScott - Export Method - Part 2
                // Export Method 
                //Begin Track #5300 - JSmith - serialization error
                //				OTSForecastExportMethodData OTSForecastExportMethodData = new OTSForecastExportMethodData(_SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList);
                OTSForecastExportMethodData OTSForecastExportMethodData = new OTSForecastExportMethodData(null);
                //End Track #5300
                dt = OTSForecastExportMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastExportLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastExportLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                //End Enhancement - JScott - Export Method - Part 2
                // Spread Method 
                OTSForecastSpreadMethodData OTSForecastSpreadMethodData = new OTSForecastSpreadMethodData();
                dt = OTSForecastSpreadMethodData.GetMethodsByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _methodForecastSpreadLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        //						AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                        //							_methodForecastSpreadLabel + " " + Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture));
                        string methodName = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentCulture);
                        AddDeleteMethodMessage(ref em, methodName,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture), _methodForecastSpreadLabel);
                        DeleteMethodPreview(ref em, Convert.ToInt32(dr["METHOD_RID"]), methodName, _methodForecastSpreadLabel);
                    }
                }

                // Tasks
                ScheduleData ScheduleData = new ScheduleData();
                dt = ScheduleData.GetAllocationTasksByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                dt = ScheduleData.GetForecastTasksByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                dt = ScheduleData.GetForecastBalanceTasksByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                dt = ScheduleData.GetRollupTasksByNode(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    if (aReplaceWithNode != null &&
                        aReplaceWithNode.Key != Include.NoRID)
                    {
                        AddReplaceProductMessage(ref em, aHnp.Text, aReplaceWithNode.Text,
                            Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        AddDeleteProductMessage(ref em, aHnp.Text, Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture),
                            _tasklistLabel + " " + Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture));
                    }
                }

                // security
                SecurityAdmin SecurityAdmin = new SecurityAdmin();
                dt = SecurityAdmin.GetDistinctNodeGroupsAssignment(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    message = (string)_deleteSecurityGroupMessage.Clone();
                    message = message.Replace("{0}", aHnp.Text);
                    message = message.Replace("{1}", Convert.ToString(dr["GROUP_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                }

                dt = SecurityAdmin.GetDistinctNodeUsersAssignment(aHnp.Key);
                foreach (DataRow dr in dt.Rows)
                {
                    message = (string)_deleteSecurityUserMessage.Clone();
                    message = message.Replace("{0}", aHnp.Text);
                    message = message.Replace("{1}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aHnp.Text, message);
                }

            }
            catch
            {
                throw;
            }
        }

        private void DeleteMethodPreview(ref EditMsgs em, int aMethodRID, string aMethodName, string aMethodType)
        {
            try
            {
                string message = null;
                DataTable dt = null;

                // check workflows

                AllocationWorkflowData AllocationWorkflowData = new AllocationWorkflowData();
                dt = AllocationWorkflowData.GetAllocMethodPropertiesUIWorkflows(aMethodRID); //TT#846-MD -jsobek -New Stored Procedures for Performance
                foreach (DataRow dr in dt.Rows)
                {
                    string workflowName = Convert.ToString(dr["WORKFLOW_NAME"], CultureInfo.CurrentCulture);
                    message = (string)_deleteFromWorkflow.Clone();
                    message = message.Replace("{0}", aMethodType + " " + aMethodName);
                    message = message.Replace("{1}", workflowName);
                    message = message.Replace("{2}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _workflowLabel, workflowName, message);
                }

                OTSPlanWorkflowData OTSPlanWorkflowData = new OTSPlanWorkflowData();
                dt = OTSPlanWorkflowData.GetOTSMethodPropertiesUIWorkflows(aMethodRID);
                foreach (DataRow dr in dt.Rows)
                {
                    string workflowName = Convert.ToString(dr["WORKFLOW_NAME"], CultureInfo.CurrentCulture);
                    message = (string)_deleteFromWorkflow.Clone();
                    message = message.Replace("{0}", aMethodType + " " + aMethodName);
                    message = message.Replace("{1}", workflowName);
                    message = message.Replace("{2}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _workflowLabel, workflowName, message);
                }

                // check tasklists

                ScheduleData ScheduleData = new ScheduleData();
                dt = ScheduleData.TaskAllocateDetail_ReadByMethod(aMethodRID);
                foreach (DataRow dr in dt.Rows)
                {
                    string tasklistName = Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture);
                    message = (string)_deleteFromTasklist.Clone();
                    message = message.Replace("{0}", aMethodType + " " + aMethodName);
                    message = message.Replace("{1}", tasklistName);
                    message = message.Replace("{2}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _tasklistLabel, tasklistName, message);
                }

                dt = ScheduleData.TaskForecastDetail_ReadByMethod(aMethodRID);
                foreach (DataRow dr in dt.Rows)
                {
                    string tasklistName = Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture);
                    message = (string)_deleteFromTasklist.Clone();
                    message = message.Replace("{0}", aMethodType + " " + aMethodName);
                    message = message.Replace("{1}", tasklistName);
                    message = message.Replace("{2}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                    //					_Session.Audit.AddReclassAuditMsg(_actionDelete, _tasklistLabel, tasklistName, message);
                }
                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                // NOTE -- Nothing ever written to TASK_FORECAST_BALANCE_DETAIL

                //                dt = ScheduleData.TaskForecastBalanceDetail_ReadByMethod(aMethodRID);
                //                foreach (DataRow dr in dt.Rows)
                //                {
                //                    string tasklistName = Convert.ToString(dr["TASKLIST_NAME"], CultureInfo.CurrentCulture);
                //                    message = (string)_deleteFromTasklist.Clone();
                //                    message = message.Replace("{0}", aMethodType + " " + aMethodName);
                //                    message = message.Replace("{1}", tasklistName);
                //                    message = message.Replace("{2}", Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentCulture));
                //                    em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                ////					_Session.Audit.AddReclassAuditMsg(_actionDelete, _tasklistLabel, tasklistName, message);
                //                }
                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            }
            catch
            {
                throw;
            }
        }

        private void AddDeleteProductMessage(ref EditMsgs em, string aNodeName, string aUserName,
            string aDataType)
        {
            try
            {
                string message = (string)_deleteProduct.Clone();
                message = message.Replace("{0}", aNodeName);
                message = message.Replace("{1}", aDataType);
                message = message.Replace("{2}", aUserName);
                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                //				_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aNodeName, message);
            }
            catch
            {
                throw;
            }
        }

        private void AddReplaceProductMessage(ref EditMsgs em, string aNodeName, string aNewNodeName,
            string aUserName, string aDataType)
        {
            try
            {
                string message = (string)_replaceProduct.Clone();
                message = message.Replace("{0}", aNodeName);
                message = message.Replace("{1}", aNewNodeName);
                message = message.Replace("{2}", aDataType);
                message = message.Replace("{3}", aUserName);
                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                //				_Session.Audit.AddReclassAuditMsg(_actionDelete, _productLabel, aNodeName, message);
            }
            catch
            {
                throw;
            }
        }

        private void AddDeleteMethodMessage(ref EditMsgs em, string aMethodName,
            string aUserName, string aMethodType)
        {
            try
            {
                string message = (string)_deleteMethod.Clone();
                message = message.Replace("{0}", aMethodType + " " + aMethodName);
                message = message.Replace("{1}", aUserName);
                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                //				_Session.Audit.AddReclassAuditMsg(_actionDelete, _methodLabel, aMethodName, message);
            }
            catch
            {
                throw;
            }
        }

        // Begin Track #5259 - JSmith - Add new reclass roll options
        //		private void ScheduleReclassRollup(ref EditMsgs em, string aAction, int aNodeRID, string aNodeID, 
        //			int aParentRID, eHierarchyLevelType aLevelType, int aToNodeRID, string aToNodeID, bool aReclassPreview, 
        //			Hashtable aRollupForecastVersions, bool aScheduleNode)
        // Begin TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
        //private void ScheduleReclassRollup(ref EditMsgs em, string aAction, int aNodeRID, string aNodeID,
        //    int aParentRID, eHierarchyLevelType aLevelType, int aToNodeRID, string aToNodeID, bool aReclassPreview,
        //    Hashtable aRollupForecastVersions, bool aScheduleNode, bool aRollExternalIntransit, bool aRollAlternateHierarchies)
        private void ScheduleReclassRollup(ref EditMsgs em, string aAction, int aNodeRID, string aNodeID,
            int aParentRID, eHierarchyLevelType aLevelType, int aToNodeRID, string aToNodeID, eHierarchyType aToNodeHierarchyType, bool aReclassPreview,
            Hashtable aRollupForecastVersions, bool aScheduleNode, bool aRollExternalIntransit, bool aRollAlternateHierarchies)
        // End TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
        // Begin Track #5259 - JSmith - Add new reclass roll options
        {
            try
            {
                DataTable dt = null;
                // Begin TT#5124 - JSmith - Performance
                //VariablesData variablesData = new VariablesData();
                VariablesData variablesData = new VariablesData(_SAB.HierarchyServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
                Intransit intransitData = new Intransit();
                NodeAncestorProfile nap = null;
                int forecastRID;
                string forecastDesc;
                int timeID;
                WeekProfile fromWeek, toWeek;
                DayProfile fromDay, toDay;
                bool rollActuals = false;
                // Begin TT#340-MD - JSmith - Reclass duplicate nodes on move
                HierarchyProfile hp = new HierarchyProfile(Include.NoRID);
                // End TT#340-MD - JSmith - Reclass duplicate nodes on move


                NodeAncestorList nal = null;
                NodeAncestorList toNodeNal = null;

                if (aRollupForecastVersions == null)
                {
                    aRollupForecastVersions = new Hashtable();
                }
                if (aRollupForecastVersions.Contains(Include.FV_ActualRID))
                {
                    rollActuals = true;
                }

                _rollupData.Rollup_XMLInit();

                // roll store intransit if node is a style
                if (aLevelType == eHierarchyLevelType.Style &&
                    aToNodeHierarchyType == eHierarchyType.organizational &&  // TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes
                    aToNodeRID != Include.NoRID)
                {
                    // roll store external intransit
                    // Begin Track #5259 - JSmith - Add new reclass roll options
                    if (aRollExternalIntransit)
                    {
                        // End Track #5259
                        forecastDesc = _intransitLabel;
                        dt = intransitData.External_Intransit_TimeIDs(aNodeRID);
                        if (dt.Rows.Count > 0)
                        {
                            if (nal == null)
                            {
                                nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                            }
                            if (toNodeNal == null &&
                                aToNodeRID != Include.NoRID)
                            {
                                toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                            }
                            timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                            fromDay = _Session.Calendar.GetDay(timeID);
                            timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                            toDay = _Session.Calendar.GetDay(timeID);
                            if (!aReclassPreview)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    timeID = Convert.ToInt32(dr["TIME_ID"]);
                                    if (aScheduleNode)
                                    {
                                        // only roll to parent of node (style)
                                        nap = (NodeAncestorProfile)nal[0];
                                        _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeExternalIntransit, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                    }
                                    if (aToNodeRID != Include.NoRID)
                                    {
                                        nap = (NodeAncestorProfile)toNodeNal[0];
                                        _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeExternalIntransit, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                    }
                                }
                            }
                            if (aScheduleNode)
                            {
                                AddReclassRollupMessage(ref em, aAction, _parent.Text, _storeLabel, forecastDesc,
                                    fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                            }
                            if (aToNodeRID != Include.NoRID)
                            {
                                AddReclassRollupMessage(ref em, aAction, aToNodeID, _storeLabel, forecastDesc,
                                    fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                            }
                        }
                        // Begin Track #5259 - JSmith - Add new reclass roll options
                    }
                    // End Track #5259

                    // roll store internal intransit
                    forecastDesc = _intransitLabel;
                    dt = intransitData.Intransit_TimeIDs(aNodeRID);
                    if (dt.Rows.Count > 0)
                    {
                        if (nal == null)
                        {
                            nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                        }
                        if (toNodeNal == null &&
                            aToNodeRID != Include.NoRID)
                        {
                            toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                        }
                        timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                        fromDay = _Session.Calendar.GetDay(timeID);
                        timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                        toDay = _Session.Calendar.GetDay(timeID);
                        if (!aReclassPreview)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                timeID = Convert.ToInt32(dr["TIME_ID"]);
                                if (aScheduleNode)
                                {
                                    // only roll to parent of node (style)
                                    nap = (NodeAncestorProfile)nal[0];
                                    _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeIntransit, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                }
                                if (aToNodeRID != Include.NoRID)
                                {
                                    nap = (NodeAncestorProfile)toNodeNal[0];
                                    _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeIntransit, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                }
                            }
                        }
                        if (aScheduleNode)
                        {
                            AddReclassRollupMessage(ref em, aAction, _parent.Text, _storeLabel, forecastDesc,
                                fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                        }
                        if (aToNodeRID != Include.NoRID)
                        {
                            AddReclassRollupMessage(ref em, aAction, aToNodeID, _storeLabel, forecastDesc,
                                fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                        }
                    }
                }

                // Begin Track #5259 - JSmith - Add new reclass roll options
                //				if ((aRollupForecastVersions != null &&
                //					aRollupForecastVersions.Count > 0) ||
                //					_alternateAPIRolllupExists)
                if ((aRollupForecastVersions != null &&
                    aRollupForecastVersions.Count > 0) ||
                    (_alternateAPIRolllupExists &&
                    aRollAlternateHierarchies))
                // End Track #5259
                {

                    // roll chain weekly history
                    // Begin Track #5259 - JSmith - Add new reclass roll options
                    //					if (rollActuals ||
                    //						_alternateAPIRolllupExists)
                    if (rollActuals ||
                        (_alternateAPIRolllupExists &&
                        aRollAlternateHierarchies))
                    // End Track #5259
                    {
                        forecastDesc = Convert.ToString(aRollupForecastVersions[Include.FV_ActualRID]);
                        dt = variablesData.ChainWeeklyHistory_TimeIDs(aNodeRID);
                        if (dt.Rows.Count > 0)
                        {
                            if (nal == null)
                            {
                                if (rollActuals)
                                {
                                    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AllHierarchies);
                                }
                                else
                                {
                                    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                }
                            }
                            if (toNodeNal == null &&
                                aToNodeRID != Include.NoRID)
                            {
                                if (rollActuals)
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AllHierarchies);
                                }
                                else
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                }
                            }
                            timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                            fromWeek = _Session.Calendar.GetWeek(timeID);
                            timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                            toWeek = _Session.Calendar.GetWeek(timeID);
                            if (!aReclassPreview)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    timeID = Convert.ToInt32(dr["TIME_ID"]);
                                    if (aScheduleNode)
                                    {
                                        for (int i = 0; i < nal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)nal[i];
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.chainWeeklyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                        }
                                    }
                                    if (aToNodeRID != Include.NoRID)
                                    {
                                        for (int i = 0; i < toNodeNal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)toNodeNal[i];
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.chainWeeklyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                        }
                                    }
                                }
                            }
                            if (aScheduleNode)
                            {
                                AddReclassRollupMessage(ref em, aAction, _parent.Text, _chainLabel, forecastDesc,
                                    fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                            }
                            if (aToNodeRID != Include.NoRID)
                            {
                                AddReclassRollupMessage(ref em, aAction, aToNodeID, _chainLabel, forecastDesc,
                                    fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                            }
                        }
                    }

                    // roll chain weekly forecasts
                    foreach (DictionaryEntry forecastVersion in aRollupForecastVersions)
                    {
                        forecastRID = (int)forecastVersion.Key;
                        if (forecastRID != Include.FV_ActualRID)
                        {
                            forecastDesc = Convert.ToString(forecastVersion.Value, CultureInfo.CurrentCulture);
                            dt = variablesData.ChainWeeklyForecast_TimeIDs(aNodeRID, forecastRID);
                            if (dt.Rows.Count > 0)
                            {
                                if (nal == null)
                                {
                                    // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                                    //									nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                                    if (aRollAlternateHierarchies)
                                    {
                                        nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AllHierarchies);
                                    }
                                    else
                                    {
                                        nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                                    }
                                    // End Track #5277
                                }
                                if (toNodeNal == null &&
                                    aToNodeRID != Include.NoRID)
                                {
                                    // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                                    //									toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                                    if (aRollAlternateHierarchies)
                                    {
                                        toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AllHierarchies);
                                    }
                                    else
                                    {
                                        toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                                    }
                                    // End Track #5277
                                }
                                timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                                fromWeek = _Session.Calendar.GetWeek(timeID);
                                timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                                toWeek = _Session.Calendar.GetWeek(timeID);
                                if (!aReclassPreview)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        timeID = Convert.ToInt32(dr["TIME_ID"]);
                                        if (aScheduleNode)
                                        {
                                            for (int i = 0; i < nal.Count; i++)
                                            {
                                                nap = (NodeAncestorProfile)nal[i];
                                                _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.chainWeeklyForecast, timeID, forecastRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                            }
                                        }
                                        if (aToNodeRID != Include.NoRID)
                                        {
                                            for (int i = 0; i < toNodeNal.Count; i++)
                                            {
                                                nap = (NodeAncestorProfile)toNodeNal[i];
                                                _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.chainWeeklyForecast, timeID, forecastRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                            }
                                        }
                                    }
                                }
                                if (aScheduleNode)
                                {
                                    AddReclassRollupMessage(ref em, aAction, _parent.Text, _chainLabel, forecastDesc,
                                        fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                                }
                                if (aToNodeRID != Include.NoRID)
                                {
                                    AddReclassRollupMessage(ref em, aAction, aToNodeID, _chainLabel, forecastDesc,
                                        fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                                }
                            }
                        }
                    }

                    // roll store daily history
                    if (rollActuals)
                    {
                        forecastDesc = Convert.ToString(aRollupForecastVersions[Include.FV_ActualRID]);
                        dt = variablesData.StoreDailyHistory_TimeIDs(aNodeRID);
                        if (dt.Rows.Count > 0)
                        {
                            if (nal == null)
                            {
                                if (rollActuals)
                                {
                                    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AllHierarchies);
                                }
                                //								else
                                //								{
                                //									nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                //								}
                            }
                            if (toNodeNal == null &&
                                aToNodeRID != Include.NoRID)
                            {
                                if (rollActuals)
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AllHierarchies);
                                }
                                else
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                }
                            }
                            timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                            fromDay = _Session.Calendar.GetDay(timeID);
                            timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                            toDay = _Session.Calendar.GetDay(timeID);
                            if (!aReclassPreview)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    timeID = Convert.ToInt32(dr["TIME_ID"]);
                                    DayProfile Day = _Session.Calendar.GetDay(timeID);
                                    int firstDayOfWeek = Day.Week.Days[0].Key;
                                    int lastDayOfWeek = Day.Week.Days[Day.Week.Days.Count - 1].Key;
                                    int firstDayOfNextWeek = _Session.Calendar.AddWeeks(Day.Week.Key, 1);
                                    if (aScheduleNode)
                                    {
                                        for (int i = 0; i < nal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)nal[i];
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeDailyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek, false);
                                        }
                                    }
                                    if (aToNodeRID != Include.NoRID)
                                    {
                                        for (int i = 0; i < toNodeNal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)toNodeNal[i];
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeDailyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek, false);
                                        }
                                    }
                                }
                            }
                            if (aScheduleNode)
                            {
                                AddReclassRollupMessage(ref em, aAction, _parent.Text, _storeLabel, forecastDesc,
                                    fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                            }
                            if (aToNodeRID != Include.NoRID)
                            {
                                AddReclassRollupMessage(ref em, aAction, aToNodeID, _storeLabel, forecastDesc,
                                    fromDay.Date.ToShortDateString(), toDay.Date.ToShortDateString());
                            }
                        }
                    }

                    // roll store weekly history
                    // Begin Track #5259 - JSmith - Add new reclass roll options
                    //					if (rollActuals ||
                    //						_alternateAPIRolllupExists)
                    if (rollActuals ||
                        (_alternateAPIRolllupExists &&
                        aRollAlternateHierarchies))
                    // End Track #5259
                    {
                        forecastDesc = Convert.ToString(aRollupForecastVersions[Include.FV_ActualRID]);
                        dt = variablesData.StoreWeeklyHistory_TimeIDs(aNodeRID);
                        if (dt.Rows.Count > 0)
                        {
                            if (nal == null)
                            {
                                if (rollActuals)
                                {
                                    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AllHierarchies);
                                }
                                else
                                {
                                    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                }
                            }
                            if (toNodeNal == null &&
                                aToNodeRID != Include.NoRID)
                            {
                                if (rollActuals)
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AllHierarchies);
                                }
                                else
                                {
                                    toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AlternateHierarchiesOnly);
                                }
                            }
                            timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                            fromWeek = _Session.Calendar.GetWeek(timeID);
                            timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                            toWeek = _Session.Calendar.GetWeek(timeID);
                            if (!aReclassPreview)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    timeID = Convert.ToInt32(dr["TIME_ID"]);
                                    if (aScheduleNode)
                                    {
                                        for (int i = 0; i < nal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)nal[i];
                                            // Begin TT#340-MD - JSmith - Reclass duplicates nodes on move
                                            if (hp == null ||
                                                hp.Key != nap.HomeHierarchyRID)
                                            {
                                                hp = _SAB.HierarchyServerSession.GetHierarchyData(nap.HomeHierarchyRID);
                                            }
                                            if (hp.HierarchyRollupOption == eHierarchyRollupOption.RealTime)
                                            {
                                                continue;
                                            }
                                            // End TT#340-MD - JSmith - Reclass duplicates nodes on move
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeWeeklyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                        }
                                    }
                                    if (aToNodeRID != Include.NoRID)
                                    {
                                        for (int i = 0; i < toNodeNal.Count; i++)
                                        {
                                            nap = (NodeAncestorProfile)toNodeNal[i];
                                            // Begin TT#340-MD - JSmith - Reclass duplicates nodes on move
                                            if (hp == null ||
                                                hp.Key != nap.HomeHierarchyRID)
                                            {
                                                hp = _SAB.HierarchyServerSession.GetHierarchyData(nap.HomeHierarchyRID);
                                            }
                                            if (hp.HierarchyRollupOption == eHierarchyRollupOption.RealTime)
                                            {
                                                continue;
                                            }
                                            // End TT#340-MD - JSmith - Reclass duplicates nodes on move
                                            _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeWeeklyHistory, timeID, Include.FV_ActualRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                        }
                                    }
                                }
                            }
                            if (aScheduleNode)
                            {
                                AddReclassRollupMessage(ref em, aAction, _parent.Text, _storeLabel, forecastDesc,
                                    fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                            }
                            if (aToNodeRID != Include.NoRID)
                            {
                                AddReclassRollupMessage(ref em, aAction, aToNodeID, _storeLabel, forecastDesc,
                                    fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                            }
                        }
                    }

                    // roll store weekly forecasts
                    foreach (DictionaryEntry forecastVersion in aRollupForecastVersions)
                    {
                        forecastRID = (int)forecastVersion.Key;
                        if (forecastRID != Include.FV_ActualRID)
                        {
                            forecastDesc = Convert.ToString(forecastVersion.Value, CultureInfo.CurrentCulture);
                            dt = variablesData.StoreWeeklyForecast_TimeIDs(aNodeRID, forecastRID);
                            if (dt.Rows.Count > 0)
                            {
                                if (nal == null)
                                {
                                    // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                                    //									nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                                    if (aRollAlternateHierarchies)
                                    {
                                        nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID, eHierarchySearchType.AllHierarchies);
                                    }
                                    else
                                    {
                                        nal = _SAB.HierarchyServerSession.GetNodeAncestorList(aParentRID);
                                    }
                                    // End Track #5277
                                }
                                if (toNodeNal == null &&
                                    aToNodeRID != Include.NoRID)
                                {
                                    // Begin Track #5277 - JSmith - Reclass not rolling alternate hierarchies
                                    //									toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                                    if (aRollAlternateHierarchies)
                                    {
                                        toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID, eHierarchySearchType.AllHierarchies);
                                    }
                                    else
                                    {
                                        toNodeNal = _SAB.HierarchyServerSession.GetNodeAncestorList(aToNodeRID);
                                    }
                                    // End Track #5277
                                }
                                timeID = Convert.ToInt32((dt.Rows[0])["TIME_ID"]);
                                fromWeek = _Session.Calendar.GetWeek(timeID);
                                timeID = Convert.ToInt32((dt.Rows[dt.Rows.Count - 1])["TIME_ID"]);
                                toWeek = _Session.Calendar.GetWeek(timeID);
                                if (!aReclassPreview)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        timeID = Convert.ToInt32(dr["TIME_ID"]);
                                        if (aScheduleNode)
                                        {
                                            for (int i = 0; i < nal.Count; i++)
                                            {
                                                nap = (NodeAncestorProfile)nal[i];
                                                // Begin TT#2139 - JSmith - Reclass on alternates when alternate is set to real time
                                                // Begin TT#340-MD - JSmith - Reclass duplicates nodes on move
                                                //if (_hp == null ||
                                                //    _hp.Key != nap.HomeHierarchyRID)
                                                //{
                                                //    _hp = _SAB.HierarchyServerSession.GetHierarchyData(nap.HomeHierarchyRID);
                                                //}
                                                //if (_hp.HierarchyRollupOption == eHierarchyRollupOption.RealTime)
                                                //{
                                                //    continue;
                                                //}
                                                // End TT#340-MD - JSmith - Reclass duplicates nodes on move
                                                // End TT#2139
                                                _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeWeeklyForecast, timeID, forecastRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                            }
                                        }
                                        if (aToNodeRID != Include.NoRID)
                                        {
                                            for (int i = 0; i < toNodeNal.Count; i++)
                                            {
                                                nap = (NodeAncestorProfile)toNodeNal[i];
                                                // Begin TT#2139 - JSmith - Reclass on alternates when alternate is set to real time
                                                // Begin TT#340-MD - JSmith - Reclass duplicates nodes on move
                                                //if (_hp == null ||
                                                //    _hp.Key != nap.HomeHierarchyRID)
                                                //{
                                                //    _hp = _SAB.HierarchyServerSession.GetHierarchyData(nap.HomeHierarchyRID);
                                                //}
                                                //if (_hp.HierarchyRollupOption == eHierarchyRollupOption.RealTime)
                                                //{
                                                //    continue;
                                                //}
                                                // End TT#340-MD - JSmith - Reclass duplicates nodes on move
                                                // End TT#2139
                                                _rollupData.Rollup_XMLInsert((int)eProcesses.hierarchyLoad, nap.Key, (int)eRollType.storeWeeklyForecast, timeID, forecastRID, nap.HomeHierarchyRID, nap.HomeHierarchyLevel, 0, 0, 0, false);
                                            }
                                        }
                                    }
                                }
                                if (aScheduleNode)
                                {
                                    AddReclassRollupMessage(ref em, aAction, _parent.Text, _storeLabel, forecastDesc,
                                        fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                                }
                                if (aToNodeRID != Include.NoRID)
                                {
                                    AddReclassRollupMessage(ref em, aAction, aToNodeID, _storeLabel, forecastDesc,
                                        fromWeek.YearWeek.ToString(), toWeek.YearWeek.ToString());
                                }
                            }
                        }
                    }
                }

                if (_rollupData.RecordsWritten > 0)
                {
                    _rollupData.OpenUpdateConnection(eLockType.RollupItem);
                    _rollupData.Rollup_XMLWrite();
                    _rollupData.CommitData();
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if (_rollupData != null)
                {
                    _rollupData.CloseUpdateConnection();
                }
            }
        }

        private void AddReclassRollupMessage(ref EditMsgs em, string aAction, string aNodeID, string aDataType,
            string aVersion, string aFromDate, string aToDate)
        {
            try
            {
                string message = (string)_rollupMessage.Clone();
                message = message.Replace("{0}", aDataType);
                message = message.Replace("{1}", aNodeID);
                message = message.Replace("{2}", aVersion);
                message = message.Replace("{3}", aFromDate);
                message = message.Replace("{4}", aToDate);
                em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                //				_Session.Audit.AddReclassAuditMsg(aAction, _productLabel, aNodeID, message);
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
        //public void AddProductCharacteristicValue(ref EditMsgs em, bool aAddCharacteristicGroups, 
        //    bool aAddCharacteristicValues, string aNodeID, string aCharacteristic, string aValue)
        public void AddProductCharacteristicValue(ref EditMsgs em, bool aAddCharacteristicGroups,
            bool aAddCharacteristicValues, string aParent, string aNodeID, string aCharacteristic, string aValue)
        // End TT#298
        {
            try
            {
                if (_dctCharacteristics == null)
                {
                    _dctCharacteristics = new Dictionary<string, ProductCharProfile>();
                    _dctCharacteristicValues = new Dictionary<string, ProductCharValueProfile>(); 
                }

                HierarchyNodeProfile hnp = null;
                //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
                ProductCharProfile wrkProductCharProfile;
                //End TT#1681

                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                ValidateParentID(ref em, ref aParent);
                // End TT#2010

                // validate node
                if (_currentProductCharNode == null ||
                    _currentProductCharNode.Key == Include.NoRID ||
                    _currentProductCharNode.HomeHierarchyParentRID != _parent.Key ||
                    _currentProductCharNode.NodeID != aNodeID)
                {
                    // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                    //hnp = ValidateNodeID(ref em, ref aNodeID);
                    if (_hnp != null &&
                        _hnp.Key != Include.NoRID &&
                        _hnp.HomeHierarchyParentRID == _parent.Key &&
                        _hnp.NodeID == aNodeID)
                    {
                        hnp = _hnp;
                    }
                    else
                    {
                        //hnp = ValidateNodeID(ref em, aParent, ref aNodeID);
                        hnp = GetNodeKey(ref em, aParent, ref aNodeID);
                    }
                    // End TT#298
                }
                else
                {
                    hnp = _currentProductCharNode;
                }

                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                if (em.ErrorFound)
                {
                    return;
                }
                // End TT#2010

                // write product characteristics for previous node if not written
                if (_currentProductCharNode != null &&
                    _currentProductCharNode.NodeID != hnp.NodeID)
                {
                    WriteProductCharacteristicValues(ref em, aNodeID);
                }

                // get product characteristics if new node
                if (hnp.Key != Include.NoRID &&
                    (_currentProductCharNode == null ||
                    (_currentProductCharNode.HomeHierarchyParentRID != _parent.Key ||
                    _currentProductCharNode.NodeID != hnp.NodeID)))
                {
                    _currentProductCharNode = hnp;
                    _nodeCharProfileList = _SAB.HierarchyServerSession.GetProductCharacteristics(hnp.Key, false);
                }

                // validate characteristic
                if (aCharacteristic == null ||
                    aCharacteristic.Trim().Length == 0)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_ProductCharRequired, sourceModule);
                    return;
                }

                //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
                //ProductCharProfile pcp = _SAB.HierarchyServerSession.GetProductCharProfile(aCharacteristic);
                //if (pcp.Key == Include.NoRID)
                //{
                //    if (aAddCharacteristicGroups)
                //    {
                //        pcp.ProductCharChangeType = eChangeType.add;
                //        pcp.ProductCharID = aCharacteristic;
                //        pcp = _SAB.HierarchyServerSession.ProductCharUpdate(pcp);
                //        string message = MIDText.GetText(eMIDTextCode.msg_ProductCharAdded);
                //        message = message.Replace("{0}", aCharacteristic);
                //        em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                //    }
                //    else
                //    {
                //        em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_ProductCharNotFound, sourceModule);
                //        return;
                //    }
                //}
                ProductCharProfile pcp;
                if (!_dctCharacteristics.TryGetValue(aCharacteristic, out pcp))
                {
                    pcp = _SAB.HierarchyServerSession.GetProductCharProfile(aCharacteristic);
                
                    if (pcp.Key == Include.NoRID)
                    {
                        if (aAddCharacteristicGroups)
                        {
                            pcp.ProductCharChangeType = eChangeType.add;
                            pcp.ProductCharID = aCharacteristic;
                            // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                            //pcp = _SAB.HierarchyServerSession.ProductCharUpdate(pcp);
                            pcp = _SAB.HierarchyServerSession.ProductCharUpdate(pcp, false);
                            // End TT#3558 - JSmith - Perf of Hierarchy Load
                            string message = MIDText.GetText(eMIDTextCode.msg_ProductCharAdded);
                            message = message.Replace("{0}", aCharacteristic);
                            em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        }
                        else
                        {
                            em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_ProductCharNotFound, sourceModule);
                            return;
                        }
                    }
                    _dctCharacteristics.Add(aCharacteristic, pcp);
                    // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                    ProductCharValueProfile temppcvp;
                    foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
                    {
                        if (!_dctCharacteristicValues.TryGetValue(pcp.Key.ToString() + ":" + pcvp.ProductCharValue, out temppcvp))
                        {
                            _dctCharacteristicValues.Add(pcp.Key.ToString() + ":" + pcvp.ProductCharValue, pcvp);
                        }
                    }
                    // End TT#3558 - JSmith - Perf of Hierarchy Load
                }
                else if (pcp.Key == Include.NoRID)
                {
                    em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_ProductCharNotFound, sourceModule);
                    return;
                }
                //End TT#1681

                // validate characteristic value
                ProductCharValueProfile characteristicValue = null;
                if (aValue != null &&
                    aValue.Trim().Length > 0)
                {
                    aValue = aValue.Trim();
                    // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                    bool foundValue = true;
                    if (!_dctCharacteristicValues.TryGetValue(pcp.Key.ToString() + ":" + aValue, out characteristicValue))
                    {
                    // End TT#3558 - JSmith - Perf of Hierarchy Load
                        // make sure value is in characteristic.  Add if necessary and allowed
                        foundValue = false;
                        foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
                        {
                            if (pcvp.ProductCharValue.ToLower() == aValue.ToLower())
                            {
                                foundValue = true;
                                characteristicValue = pcvp;
                                break;
                            }
                        }
                    // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                    }
                    // End TT#3558 - JSmith - Perf of Hierarchy Load
                    if (!foundValue)
                    {
                        if (aAddCharacteristicValues)
                        {
                            ProductCharValueProfile pcvp = new ProductCharValueProfile(Include.NoRID);
                            pcvp.ProductCharValueChangeType = eChangeType.add;
                            pcvp.ProductCharValue = aValue;
                            pcvp.ProductCharRID = pcp.Key;
                            //Begin TT#1681 - JSmith - Performance loading hierarchy with characteristics
                            //pcp.ProductCharValues.Add(pcvp);
                            //pcp.ProductCharChangeType = eChangeType.update;
                            //pcp = _SAB.HierarchyServerSession.ProductCharUpdate(pcp);
                            //foreach (ProductCharValueProfile pcvp2 in pcp.ProductCharValues)
                            //{
                            //    if (pcvp2.ProductCharValue.ToLower() == aValue.ToLower())
                            //    {
                            //        characteristicValue = pcvp2;
                            //        break;
                            //    }
                            //}
                            wrkProductCharProfile = new ProductCharProfile(pcp.Key);
                            wrkProductCharProfile.ProductCharID = pcp.ProductCharID;
                            wrkProductCharProfile.ProductCharValues.Add(pcvp);
                            wrkProductCharProfile.ProductCharChangeType = eChangeType.none;
                            // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                            //wrkProductCharProfile = _SAB.HierarchyServerSession.ProductCharUpdate(wrkProductCharProfile);
                            wrkProductCharProfile = _SAB.HierarchyServerSession.ProductCharUpdate(wrkProductCharProfile, false);
                            // End TT#3558 - JSmith - Perf of Hierarchy Load
                            characteristicValue = (ProductCharValueProfile)wrkProductCharProfile.ProductCharValues[0];
                            pcp.ProductCharValues.Add(characteristicValue);
                            _dctCharacteristicValues.Add(pcp.Key.ToString() + ":" + characteristicValue.ProductCharValue, characteristicValue);
                            //End TT#1681
                            string message = MIDText.GetText(eMIDTextCode.msg_ProductCharValueAdded);
                            message = message.Replace("{0}", aValue);
                            em.AddMsg(eMIDMessageLevel.Information, message, sourceModule);
                        }
                        else
                        {
                            em.AddMsg(eMIDMessageLevel.Error, eMIDTextCode.msg_ProductCharValueNotFound, sourceModule);
                            return;
                        }
                    }
                }

                // update characteristic in node
                NodeCharProfile deleteProfile = null;
                bool addCharacteristicValue = true;
                // Begin TT#3558 - JSmith - Perf of Hierarchy Load
                //foreach (NodeCharProfile ncp in _nodeCharProfileList)
                //{
                //    if (ncp.Key == pcp.Key)
                //    {
                //        if (characteristicValue == null)
                //        {
                //            deleteProfile = ncp;
                //            addCharacteristicValue = false;
                //        }
                //        else if (ncp.ProductCharValueRID != characteristicValue.Key)
                //        {
                //            deleteProfile = ncp;
                //        }
                //        else
                //        {
                //            addCharacteristicValue = false;
                //        }
                //        break;
                //    }
                //}
                NodeCharProfile ncp2 = (NodeCharProfile)_nodeCharProfileList.FindKey(pcp.Key);

                if (ncp2 != null)
                {
                    if (characteristicValue == null)
                    {
                        deleteProfile = ncp2;
                        addCharacteristicValue = false;
                    }
                    else if (ncp2.ProductCharValueRID != characteristicValue.Key)
                    {
                        deleteProfile = ncp2;
                    }
                    else
                    {
                        addCharacteristicValue = false;
                    }
                }
                // End TT#3558 - JSmith - Perf of Hierarchy Load

                if (deleteProfile != null)
                {
                    _nodeCharProfileList.Remove(deleteProfile);
                    _currentProductCharNode.NodeChangeType = eChangeType.update;
                }

                //BEGIN TT#780-MD-VStuart-Blank Characteristic value in XML causes error
                if (addCharacteristicValue && 
                    characteristicValue != null &&
                    characteristicValue.ProductCharValue.Trim().Length > 0)
                //END TT#780-MD-VStuart-Blank Characteristic value in XML causes error
                {
                    NodeCharProfile ncp = new NodeCharProfile(pcp.Key);
                    ncp.ProductCharChangeType = eChangeType.add;
                    ncp.ProductCharValueRID = characteristicValue.Key;
                    ncp.ProductCharValue = characteristicValue.ProductCharValue;
                    _nodeCharProfileList.Add(ncp);
                    _currentProductCharNode.NodeChangeType = eChangeType.update;
                }
            }
            catch
            {
                throw;
            }
        }

        public void WriteProductCharacteristicValues(ref EditMsgs em, string aNodeID)
        {
            try
            {
                if (_nodeCharProfileList != null &&
                    _currentProductCharNode.NodeChangeType == eChangeType.update)
                {
                    _SAB.HierarchyServerSession.UpdateProductCharacteristics(_currentProductCharNode.Key, _nodeCharProfileList);
                }
                _currentProductCharNode = null;
            }
            catch
            {
                throw;
            }
        }

        public bool IsProductCharNameValid(int aKey, string aNewName)
        {
            try
            {
                ProductCharProfile pcp = _SAB.HierarchyServerSession.GetProductCharProfile(aNewName);
                if (pcp.Key > Include.NoRID &&
                    pcp.Key != aKey)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool IsProductCharValueValid(int aKey, int aProductCharKey, string aNewValue)
        {
            try
            {
                ProductCharProfile pcp = _SAB.HierarchyServerSession.GetProductCharProfile(aProductCharKey);
                foreach (ProductCharValueProfile pcvp in pcp.ProductCharValues)
                {
                    if (pcvp.ProductCharValue == aNewValue &&
                        pcvp.Key != aKey)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool IsProductCharAlreadyAssigned(int aNodeRID, int aProductCharRID, int aProductCharValueRID,
            out int aValueRID, out string aValue)
        {
            try
            {
                aValueRID = Include.NoRID;
                aValue = string.Empty;
                NodeCharProfileList nodeCharProfileList = _SAB.HierarchyServerSession.GetProductCharacteristics(aNodeRID, false);
                foreach (NodeCharProfile ncp in nodeCharProfileList)
                {
                    if (ncp.Key == aProductCharRID &&
                        ncp.ProductCharValueRID != aProductCharValueRID)
                    {
                        aValueRID = ncp.ProductCharValueRID;
                        aValue = ncp.ProductCharValue;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateProductCharValue(int aNodeRID, int aProductCharRID, int aProductCharValueRID, eChangeType aChangeType)
        {
            try
            {
                NodeCharProfileList nodeCharProfileList = _SAB.HierarchyServerSession.GetProductCharacteristics(aNodeRID, false);
                bool addCharValue = false;
                bool changeMade = false;
                if (aChangeType == eChangeType.add)
                {
                    addCharValue = true;
                }
                foreach (NodeCharProfile ncp in nodeCharProfileList)
                {
                    switch (aChangeType)
                    {
                        case eChangeType.add:
                            if (ncp.ProductCharValueRID == aProductCharValueRID)
                            {
                                addCharValue = false;
                            }
                            break;
                        case eChangeType.update:
                            if (ncp.ProductCharValueRID == aProductCharValueRID)
                            {
                                addCharValue = false;
                            }
                            break;
                        case eChangeType.delete:
                            if (ncp.ProductCharValueRID == aProductCharValueRID)
                            {
                                ncp.ProductCharChangeType = eChangeType.delete;
                                changeMade = true;
                            }
                            break;
                    }
                }
                if (addCharValue)
                {
                    changeMade = true;
                    NodeCharProfile nodeCharProfile = new NodeCharProfile(aProductCharRID);
                    nodeCharProfile.ProductCharChangeType = eChangeType.add;
                    nodeCharProfile.ProductCharValueRID = aProductCharValueRID;
                    nodeCharProfileList.Add(nodeCharProfile);
                }

                if (changeMade)
                {
                    _SAB.HierarchyServerSession.UpdateProductCharacteristics(aNodeRID, nodeCharProfileList);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve place holder colors
        /// </summary>
        /// <param name="_aNumberOfPlaceholderColors">The number of placeholder colors to retrieve</param>
        /// <param name="aCurrentPlaceHolderColors">
        /// The list of current placeholder colors.  This is used to filter the list
        /// </param>
        /// <returns>ColorCodeList of placeholder color code profiles</returns>
        public ColorCodeList GetPlaceholderColors(int _aNumberOfPlaceholderColors, ArrayList aCurrentPlaceHolderColors)
        {
            try
            {
                return _SAB.HierarchyServerSession.GetPlaceholderColors(_aNumberOfPlaceholderColors, aCurrentPlaceHolderColors);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve assortment node
        /// </summary>
        /// <returns>HierarchyNodeProfile of the assortment node</returns>
        public HierarchyNodeProfile GetAssortmentNode()
        {
            try
            {
                return _SAB.HierarchyServerSession.GetAssortmentNode();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve placeholder style node
        /// </summary>
        /// <param name="aAnchorNode">
        /// The key of the anchor node for the placeholder style.
        /// </param>
        /// <param name="aCurrentNumberOfPlaceholderStyles">
        /// The current number of placeholder styles already defined to the assortment
        /// </param>
        /// <param name="aAssortmentID">
        /// The ID of the assortment to use in the placeholder styles ID
        /// </param>
        /// <returns>HierarchyNodeProfiles with the placeholder style node</returns>
        //public HierarchyNodeProfile GetPlaceholderStyle(int aAnchorNode, 
        //    int aCurrentNumberOfPlaceholderStyles, string aAssortmentID)
        public HierarchyNodeProfile GetPlaceholderStyle(int aAnchorNode, 
            int aCurrentNumberOfPlaceholderStyles, int aAssortmentRID)
        {
            try
            {
                HierarchyNodeList nodeList = _SAB.HierarchyServerSession.GetPlaceholderStyles(aAnchorNode, 1,
                         aCurrentNumberOfPlaceholderStyles, aAssortmentRID);
                return (HierarchyNodeProfile)nodeList[0];
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve placeholder style node
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
        /// The RID of the assortment to use in the placeholder styles ID
        /// </param>
        /// <returns>HierarchyNodeList of HierarchyNodeProfiles with the placeholder style nodes</returns>
        public HierarchyNodeList GetPlaceholderStyles(int aAnchorNode, int aNumberOfPlaceholderStyles,
            int aCurrentNumberOfPlaceholderStyles, int aAssortmentRID)
        {
            try
            {
                return _SAB.HierarchyServerSession.GetPlaceholderStyles(aAnchorNode, aNumberOfPlaceholderStyles,
                         aCurrentNumberOfPlaceholderStyles, aAssortmentRID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Move placeholder style node to a different anchor node
        /// </summary>
        /// <param name="aPlacholderStyle">
        /// The key of the placeholder style that is to be moved.
        /// </param>
        /// <param name="aOldAnchorNode">
        /// The key of the anchor node for the placeholder style.
        /// </param>
        /// <param name="aNewAnchorNode">
        /// The key of the anchor node for the placeholder style.
        /// </param>
        public void MovePlaceholderStyleAnchorNode(int aPlacholderStyle, int aOldAnchorNode,
            int aNewAnchorNode)
        {
            try
            {
                HierarchyNodeProfile placeholderStyle = _SAB.HierarchyServerSession.GetNodeData(aPlacholderStyle);
                HierarchyNodeProfile anchorNode = _SAB.HierarchyServerSession.GetNodeData(aNewAnchorNode);
                HierarchyNodeProfile connectorNode = _SAB.HierarchyServerSession.GetLowestConnectorNode(aNewAnchorNode);

                HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
                hjp.JoinChangeType = eChangeType.update;
                hjp.OldHierarchyRID = placeholderStyle.HomeHierarchyRID;
                hjp.OldParentRID = placeholderStyle.HomeHierarchyParentRID;
                hjp.NewHierarchyRID = placeholderStyle.HomeHierarchyRID;
                hjp.NewParentRID = connectorNode.Key;
                hjp.Key = placeholderStyle.Key;
                hjp.LevelType = eHierarchyLevelType.Style;
                _SAB.HierarchyServerSession.OpenUpdateConnection();

                _SAB.HierarchyServerSession.JoinUpdate(hjp);

                _SAB.HierarchyServerSession.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                {
                    _SAB.HierarchyServerSession.CloseUpdateConnection();
                }
            }
        }

        /// <summary>
        /// Delete placeholder style node 
        /// </summary>
        /// <param name="aPlacholderStyle">
        /// The key of the placeholder style that is to be deleted.
        /// </param>
        public void DeletePlaceholderStyleAnchorNode(int aPlacholderStyle, ref EditMsgs em)
        {
            try
            {
                HierarchyNodeProfile placeholderStyle = _SAB.HierarchyServerSession.GetNodeData(aPlacholderStyle);
                placeholderStyle.NodeChangeType = eChangeType.delete;

                _SAB.HierarchyServerSession.OpenUpdateConnection();

                // Begin TT#2 - RMatelic - Assortment Planning: color nodes being orphaned when placeholder style is deleted
                //_SAB.HierarchyServerSession.NodeUpdateProfileInfo(placeholderStyle);
                _hp = _SAB.HierarchyServerSession.GetHierarchyData(placeholderStyle.HomeHierarchyRID);
                int parentRID = Convert.ToInt32(placeholderStyle.Parents[0], CultureInfo.CurrentUICulture);
                _parent = _SAB.HierarchyServerSession.GetNodeData(parentRID);

                string replaceWithNode = null;
                bool forceDelete = false;
                bool reclassPreview = false;
                Hashtable rollupForecastVersions = null;
                bool rollExternalIntransit = false;
                bool rollAlternateHierarchies = false;

                DeleteNode(ref em, _hp.HierarchyID, _parent.NodeID, placeholderStyle.NodeID, replaceWithNode, forceDelete, reclassPreview,
                            rollupForecastVersions, rollExternalIntransit, rollAlternateHierarchies);
                // End TT#2

                _SAB.HierarchyServerSession.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
                {
                    _SAB.HierarchyServerSession.CloseUpdateConnection();
                }
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
                return _SAB.HierarchyServerSession.GetAnchorNode(aNodeRID);
            }
            catch
            {
                throw;
            }
        }

        public ArrayList ValidEligibilityData(ProfileList storeList, DataSet storeEligibilityDataSet, StoreEligibilityList storeEligList, ArrayList errorList)
        {
            bool profileFound = false;
            //bool ErrorFound = false;
            StoreEligibilityErrors see = new StoreEligibilityErrors();
            try
            {
                int storeRID;
                StoreEligibilityProfile sep;
                string exceptionMessage = string.Empty;


                foreach (DataRow storeRow in storeEligibilityDataSet.Tables["Stores"].Rows)
                {
                    //foreach (DataRow storeRow in dr.Table.Rows)
                    //{
                        if ((bool)storeRow["Updated"])
                        {
                            storeRID = (int)storeRow["Store RID"];
                            if (storeEligList.Contains(storeRID))
                            {
                                sep = (StoreEligibilityProfile)storeEligList.FindKey(storeRID);
                                profileFound = true;
                            }
                            else
                            {
                                sep = new StoreEligibilityProfile(storeRID);
                                profileFound = false;
                            }
                            // set change type to update if found profile and something not inherited
                            if (profileFound &&
                                sep.RecordExists)
                            {
                                sep.StoreEligChangeType = eChangeType.update;
                            }
                            else
                            {
                                sep.StoreEligChangeType = eChangeType.add;
                            }

                            // update eligibility fields based on inheritance
                            if (Convert.ToInt32(storeRow["Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.EligModelRID = Include.NoRID;
                                sep.EligModelName = string.Empty;
                                sep.StoreIneligible = false;
                                sep.EligIsInherited = true;
                            }
                            else
                            {
								//BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Eligibility RID"] != DBNull.Value)
                                {
                                    sep.EligModelRID = (int)storeRow["Eligibility RID"];
                                }
								//END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Eligibility"] != DBNull.Value)
                                {
                                    sep.EligModelName = (string)storeRow["Eligibility"];
                                }
                                sep.StoreIneligible = (bool)storeRow["Ineligible"];
                                if (Convert.ToBoolean(storeRow["Inherited"], CultureInfo.CurrentCulture))
                                {
                                    sep.EligIsInherited = true;
                                }
                                else
                                {
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                }
                            }

                            // update stock model fields based on inheritance
                            if (Convert.ToInt32(storeRow["Stock Modifier Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.StkModModelRID = Include.NoRID;
                                sep.StkModModelName = string.Empty;
                                sep.StkModType = eModifierType.None;
                                sep.StkModPct = Include.Undefined;
                                sep.StkModIsInherited = true;
                                //sep.StkLeadWeeks = Include.Undefined; //TT#44 - MD - DOConnell - New Store Forecasting Enhancement
                            }
                            else
                            {
								//BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Stock Modifier RID"] != DBNull.Value)
                                {
                                    sep.StkModModelRID = (int)storeRow["Stock Modifier RID"];
                                }
								//END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Stock Modifier"] != DBNull.Value)
                                {
                                    sep.StkModModelName = (string)storeRow["Stock Modifier"];
                                }
                                sep.StkModType = eModifierType.None;
                                sep.StkModIsInherited = false;
                                sep.StkModInheritedFromNodeRID = Include.NoRID;
                                if (sep.StkModModelName != null &&
                                    sep.StkModModelName.Trim().Length > 0)
                                {
                                    if (sep.StkModModelRID == Include.NoRID)
                                    {
                                        try
                                        {
											//BEGIN TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            if (storeRow["Stock Modifier"] != DBNull.Value)
                                            {
                                                sep.StkModPct = Convert.ToDouble(storeRow["Stock Modifier"], CultureInfo.CurrentUICulture);
                                                sep.StkModType = eModifierType.Percent; //TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            }
                                            //END TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            else
                                            {
                                                sep.StkModType = eModifierType.None;
                                                sep.StkModModelName = string.Empty;
                                                sep.StkModPct = Include.Undefined; 
                                            }
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //BEGIN TT#4375 - DOConnell - Store eligibility error
                                            //exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric, false);
                                            //END TT#4375 - DOConnell - Store eligibility error
                                        	//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											see.typeErr = true;
                                            see.storeRID = storeRID;
                                            see.type = "Stock Modifier";
                                            see.message = exceptionMessage;
                                            see.dataString = Convert.ToString(storeRow["Stock Modifier"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            errorList.Add(see);

                                            //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        }
                                    }
                                    else
                                    {
                                        sep.StkModType = eModifierType.Model;
                                    }
                                }
                            }

                            // update sales model fields based on inheritance
                            if (Convert.ToInt32(storeRow["Sales Modifier Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.SlsModModelRID = Include.NoRID;
                                sep.SlsModModelName = string.Empty;
                                sep.SlsModType = eModifierType.None;
                                sep.SlsModPct = Include.Undefined;
                                sep.SlsModIsInherited = true;
                            }
                            else
                            {
								//BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Sales Modifier RID"] != DBNull.Value)
                                {
                                    sep.SlsModModelRID = (int)storeRow["Sales Modifier RID"];
                                }
								//END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["Sales Modifier"] != DBNull.Value)
                                {
                                    sep.SlsModModelName = (string)storeRow["Sales Modifier"];
                                }
                                sep.SlsModType = eModifierType.None;
                                sep.SlsModIsInherited = false;
                                sep.SlsModInheritedFromNodeRID = Include.NoRID;
                                if (sep.SlsModModelName != null &&
                                    sep.SlsModModelName.Trim().Length > 0)
                                {
                                    if (sep.SlsModModelRID == Include.NoRID)
                                    {
                                        try
                                        {
										//BEGIN TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            if (storeRow["Sales Modifier"] != DBNull.Value)
                                            {
                                                sep.SlsModPct = Convert.ToDouble(storeRow["Sales Modifier"], CultureInfo.CurrentUICulture);
                                                sep.SlsModType = eModifierType.Percent; //TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            }
                                            //END TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            else
                                            {
                                                sep.SlsModType = eModifierType.None;
                                                sep.SlsModModelName = string.Empty;
                                                sep.SlsModPct = Include.Undefined;
                                            }
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //BEGIN TT#4375 - DOConnell - Store eligibility error
                                            //exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric, false);
                                            //END TT#4375 - DOConnell - Store eligibility error
                                            //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											see.storeRID = storeRID;
                                            see.typeErr = true;
                                            see.type = "Sales Modifier";
                                            see.message = exceptionMessage;
                                            see.dataString = Convert.ToString(storeRow["Sales Modifier"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            errorList.Add(see);

                                            //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        }
                                    }
                                    else
                                    {
                                        sep.SlsModType = eModifierType.Model;
                                    }
                                }
                            }

                            // update FWOS model fields based on inheritance
                            if (Convert.ToInt32(storeRow["FWOS Modifier Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.FWOSModModelRID = Include.NoRID;
                                sep.FWOSModModelName = string.Empty;
                                sep.FWOSModType = eModifierType.None;
                                sep.FWOSModPct = Include.Undefined;
                                sep.FWOSModIsInherited = true;
                            }
                            else
                            {
								//BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["FWOS Modifier RID"] != DBNull.Value)
                                {
                                    sep.FWOSModModelRID = (int)storeRow["FWOS Modifier RID"];
                                }
								//END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                if (storeRow["FWOS Modifier"] != DBNull.Value)
                                {
                                    sep.FWOSModModelName = (string)storeRow["FWOS Modifier"];
                                }
                                sep.FWOSModType = eModifierType.None;
                                sep.FWOSModIsInherited = false;
                                sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                                if (sep.FWOSModModelName != null &&
                                    sep.FWOSModModelName.Trim().Length > 0)
                                {
                                    if (sep.FWOSModModelRID == Include.NoRID)
                                    {
                                        try
                                        {
											//BEGIN TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            if (storeRow["FWOS Modifier"] != DBNull.Value)
                                            {
                                                sep.FWOSModPct = Convert.ToDouble(storeRow["FWOS Modifier"], CultureInfo.CurrentUICulture);
                                                sep.FWOSModType = eModifierType.Percent; //TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            }
                                            //END TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            else
                                            {
                                                sep.FWOSModType = eModifierType.None;
                                                sep.FWOSModModelName = string.Empty;
                                                sep.FWOSModPct = Include.Undefined;
                                            }
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //BEGIN TT#4375 - DOConnell - Store eligibility error
                                            //exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric, false);
                                            //END TT#4375 - DOConnell - Store eligibility error
                                            //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											see.typeErr = true;
                                            see.storeRID = storeRID;
                                            see.type = "FWOS Modifier";
                                            see.message = exceptionMessage;
                                            see.dataString = Convert.ToString(storeRow["FWOS Modifier"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            errorList.Add(see);

                                            //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        }
                                    }
                                    else
                                    {
                                        sep.FWOSModType = eModifierType.Model;
                                    }
                                }
                            }

                            // update similar store fields based on inheritance
                            if (Convert.ToInt32(storeRow["Similar Store Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.SimStoresChanged = false;
                                sep.SimStoreType = eSimilarStoreType.None;
                                sep.SimStores = null;
                                sep.SimStoreRatio = Include.Undefined;
                                sep.SimStoreUntilDateRangeRID = Include.NoRID;
                                sep.SimStoreIsInherited = false;
                            }
                            else
                                if (Convert.ToString(storeRow["Similar Store"]).Length > 0 ||
                                Convert.ToString(storeRow["Ratio"]).Length > 0 ||
                                Convert.ToString(storeRow["Until"]).Length > 0)
                                {
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                    if (Convert.ToBoolean(storeRow["Similar Store Changed"], CultureInfo.CurrentUICulture))
                                    {
                                        sep.SimStoresChanged = true;
                                        sep.SimStoreType = eSimilarStoreType.Stores;
                                        if (Convert.ToBoolean(storeRow["Used Store Selector"], CultureInfo.CurrentUICulture))
                                        {
                                            sep.SimStores = (ArrayList)storeRow["SimilarStoresArrayList"];
                                        }
                                        else
                                            if (Convert.ToString(storeRow["Similar Store"]).Length > 0)
                                            {
                                                bool simStoreError = false;
                                                ArrayList SimilarStoresList = new ArrayList();
                                                string[] fields = Convert.ToString(storeRow["Similar Store"]).Split(',');
                                                for (int i = 0; i < fields.Length; i++)
                                                {
                                                    // Begin TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API
                                                    //StoreProfile sp = StoreMgmt.StoreProfile_Get(fields[i].Trim()); //_SAB.StoreServerSession.GetStoreProfile(fields[i].Trim());
                                                    StoreProfile sp = null;
                                                    if (_stores == null)
                                                    {
                                                        _stores = _SAB.StoreServerSession.GetStoreIDHash();
                                                        _storeList = _SAB.StoreServerSession.GetActiveStoresList();
                                                    }
                                                    object objRID = _stores[fields[i].Trim()];
                                                    if (objRID != null)
                                                    {
                                                        sp = (StoreProfile)_storeList.FindKey(Convert.ToInt32(objRID));
                                                    }
                                                    // End TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API

                                                    // Begin TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API
                                                    //if (sp.ActiveInd)
                                                    if (sp != null
                                                        && sp.ActiveInd)
                                                    // End TT#1916-MD - JSmith - Store Eligibility - Similar Stores not displaying when running SE API
                                                    {
                                                        if (sp.Key == Include.NoRID)
                                                        {
                                                            sp.Key = 1;
                                                        }
                                                        SimilarStoresList.Add(sp.Key);
                                                    }
                                                    else
                                                    {
                                                        simStoreError = true;
                                                    }
                                                }
                                                if (simStoreError)
                                                {
													//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                    //exceptionMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotFound);
                                                    //BEGIN TT#4375 - DOConnell - Store eligibility error
                                                    //exceptionMessage = _Session.Audit.GetText(eMIDTextCode.msg_StoreNotFound);
                                                    exceptionMessage = _Session.Audit.GetText(eMIDTextCode.msg_InvalidSimilarStore, false);
                                                    //END TT#4375 - DOConnell - Store eligibility error
													//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                    see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                    see.typeErr = false;
                                                    see.simStoreErr = true;
                                                    see.storeRID = storeRID;
                                                    see.type = "";
                                                    see.message = exceptionMessage;
                                                    see.dataString = Convert.ToString(storeRow["Similar Store"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                    errorList.Add(see);

                                                    //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                    //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotFound), this.Text,
                                                    //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //    ErrorFound = true;
                                                }
                                                else
                                                {
                                                    sep.SimStores = SimilarStoresList;
                                                }
                                            }
                                            else
                                            {
                                                sep.SimStores = null;
                                            }
                                    }

                                    if (storeRow["Ratio"] != System.DBNull.Value) //TT#821 - MD - DOConnell - Node Properties - similar store index cannot be deleted in edit mode.
                                    //if (storeRow["Ratio"] != string.Empty)
									{
                                        try
                                        {
											//BEGIN TT#821 - MD - DOConnell - Node Properties - similar store index cannot be deleted in edit mode.
                                            if ((String)storeRow["Ratio"] != string.Empty)
                                            {
                                                sep.SimStoreRatio = Convert.ToDouble(storeRow["Ratio"], CultureInfo.CurrentUICulture);
                                            }
                                            else
                                            {
                                                sep.SimStoreRatio = Include.DefaultSimilarStoreRatio;
                                            }
											//END TT#821 - MD - DOConnell - Node Properties - similar store index cannot be deleted in edit mode.
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //BEGIN TT#4375 - DOConnell - Store eligibility error
                                            //exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric, false);
                                            //END TT#4375 - DOConnell - Store eligibility error
                                            //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											see.typeErr = true;
                                            see.storeRID = storeRID;
                                            see.type = "Ratio";
                                            see.message = exceptionMessage;
                                            see.dataString = Convert.ToString(storeRow["Ratio"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            errorList.Add(see);

                                            //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file

                                        }
                                    }
                                    else
                                    {
                                        sep.SimStoreRatio = Include.DefaultSimilarStoreRatio;
                                    }

                                    if (storeRow["Until"] != System.DBNull.Value) //TT#821 - MD - DOConnell - Node Properties - similar store index cannot be deleted in edit mode.
                                    //if (storeRow["Ratio"] != string.Empty)
									{
                                        try
                                        {
                                            sep.SimStoreUntilDateRangeRID = Convert.ToInt32(storeRow["Until RID"], CultureInfo.CurrentUICulture);
                                            sep.SimStoreDisplayDate = (string)storeRow["Until"];
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //BEGIN TT#4375 - DOConnell - Store eligibility error
                                            //exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric, false);
                                            //END TT#4375 - DOConnell - Store eligibility error
                                            //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            see = new StoreEligibilityErrors(); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											see.typeErr = true;
                                            see.storeRID = storeRID;
                                            see.type = "Until RID";
                                            see.message = exceptionMessage;
                                            see.dataString = Convert.ToString(storeRow["Until"]); //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            errorList.Add(see);

                                            //return errorList; //TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        }
                                    }
                                    else
                                    {
                                        sep.SimStoreUntilDateRangeRID = Include.NoRID;
                                    }
                                }
                                else if (sep.SimStoreType != eSimilarStoreType.None)
                                {
                                    sep.SimStoresChanged = true;
                                    sep.SimStoreType = eSimilarStoreType.None;
                                    sep.SimStores = null;
                                    sep.SimStoreRatio = Include.Undefined;
                                    sep.SimStoreUntilDateRangeRID = Include.NoRID;
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                }

							//BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                            //If storeRow["PMPlusSales"] is set to 1 or true then the checkbox will be set which means that Min Plus sales will be included in Forecasting
                            //If storeRow["PMPlusSales"] is set to 0 or false the checkbox will be greyed out on the screen and any inheritance will be broken. This means that 
                            //any inheritance is ignored and Min Plus Sales will not be included in Forecasting. 
                            //If storeRow["PMPlusSales"] is set to “” or “ “ the checkbox will be cleared and any inheritance will be reestablished.
                            if (Convert.ToInt32(storeRow["PMPlusSales Inherited RID"], CultureInfo.CurrentCulture) != Include.NoRID)
                            {
                                sep.PresPlusSalesIsInherited = true;
								sep.PresPlusSalesIsSet = false;
                            }
                            else
                            {
                                if (storeRow["PMPlusSales"] == System.DBNull.Value)
                                {
                                    sep.PresPlusSalesInd = false;
                                    sep.PresPlusSalesIsSet = true;
                                    sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                                    sep.PresPlusSalesIsInherited = false;
                                }
                                else if (Convert.ToBoolean(storeRow["PMPlusSales"]))
                                {
                                    sep.PresPlusSalesInd = true;
                                    sep.PresPlusSalesIsSet = true;
                                    sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                                    sep.PresPlusSalesIsInherited = false;
                                }
                                else
                                {
                                    sep.PresPlusSalesInd = false;
                                    sep.PresPlusSalesIsSet = false;
                                    sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                                    sep.PresPlusSalesIsInherited = false;
                                }
                            }
							//END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales

                            //            // alter change type if record does not contain any data needed to be saved
                            if (sep.EligIsInherited == false &&
                                sep.StkModIsInherited == false &&
                                sep.SlsModIsInherited == false &&
                                sep.SimStoreIsInherited == false &&
                                sep.EligModelRID == Include.NoRID &&
                                !sep.StoreIneligible &&
                                (sep.SlsModModelRID == Include.NoRID && sep.SlsModType == eModifierType.Model) &&
                                (sep.StkModModelRID == Include.NoRID && sep.StkModType == eModifierType.Model) &&
                                (sep.SimStores == null || sep.SimStores.Count == 0))
                            {
                                if (sep.StoreEligChangeType == eChangeType.update)
                                {
                                    sep.StoreEligChangeType = eChangeType.delete;
                                }
                                else
                                    if (sep.StoreEligChangeType == eChangeType.add)
                                    {
                                        sep.StoreEligChangeType = eChangeType.none;
                                    }
                            }

                            if (!profileFound)
                            {
                                if (sep.StoreEligChangeType != eChangeType.none)
                                {
                                    storeEligList.Add(sep);
									//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                    //see.sel = storeEligList;
                                    //errorList.Add(see);
									//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                }
                            }
                            else
                            {
                                storeEligList.Update(sep);
								//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                //see.simStoreErr = false;
                                //see.typeErr = false;
                                //see.sel = storeEligList;
                                //errorList.Add(see);
								//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                            }
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDHierarchyService", ex.Message, EventLogEntryType.Error);
            }
            return errorList;
        }

        public ArrayList ValidIMOData(int nodeRID, ProfileList storeList, DataSet IMODataSet, IMOProfileList IMOprofList, ArrayList errorList)
        {
            try
            {
                bool profileFound = false;
                int storeRID;
                IMOProfile imop;
                string exceptionMessage = string.Empty;
                IMOErrors IMOe = new IMOErrors();
                foreach (DataRow storeRow in IMODataSet.Tables["Stores"].Rows)
                {
                    //foreach (DataRow storeRow in dr.Table.Rows)
                    //{
                        if ((bool)storeRow["Updated"])
                        {
                            storeRID = (int)storeRow["Store RID"];
                            if ((IMOprofList != null) && (IMOprofList.Contains(storeRID)))
                            {
                                imop = (IMOProfile)IMOprofList.FindKey(storeRID);
                                imop.IMONodeRID = nodeRID;
                                imop.IMOChangeType = eChangeType.update;
                                profileFound = true;
                            }
                            else
                            {
                                profileFound = false;
                                imop = new IMOProfile(storeRID);
                                imop.IMOStoreRID = storeRID;
                                imop.IMONodeRID = nodeRID;
                                imop.IMOChangeType = eChangeType.add;
                            }
                            //BEGIN TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            //if (storeRow["Item Max"] != string.Empty)
                            if (storeRow["Item Max"] != DBNull.Value)
                            //END TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            {
                                // Begin TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                //if ((String)storeRow["Item Max"] != string.Empty)
                                if (Convert.ToString(storeRow["Item Max"]).Trim() != string.Empty)
                                // End TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                {
                                    // Begin TT#3680 - JSmith - VSW fields accepting invalid values
                                    //imop.IMOMaxValue = Convert.ToInt32(storeRow["Item Max"], CultureInfo.CurrentUICulture);
                                    try
                                    {
                                        imop.IMOMaxValue = Convert.ToInt32(storeRow["Item Max"], CultureInfo.CurrentUICulture);
                                    }
                                    catch (Exception error)
                                    {
										//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        //exceptionMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        exceptionMessage = _Session.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
										//END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        IMOe = new IMOErrors();
                                        IMOe.Error = true;
                                        IMOe.StoreRID = storeRID;
                                        IMOe.Message = exceptionMessage;
                                        IMOe.type = "Item Max";
                                        errorList.Add(IMOe);
                                    }
                                    // End TT#3680 - JSmith - VSW fields accepting invalid values
                                }
                                else
                                {
                                    imop.IMOMaxValue = int.MaxValue;
                                }
                            }
                            else
                            {
                                imop.IMOMaxValue = int.MaxValue;
                            }
                            //BEGIN TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            //if (storeRow["Min Ship Qty"] != string.Empty)
                            if (storeRow["Min Ship Qty"] != System.DBNull.Value)
                            //END TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            {
                                // Begin TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                //if ((String)storeRow["Min Ship Qty"] != string.Empty)
                                if (Convert.ToString(storeRow["Min Ship Qty"]).Trim() != string.Empty)
                                // End TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                {
                                    // Begin TT#3680 - JSmith - VSW fields accepting invalid values
                                    //imop.IMOMinShipQty = Convert.ToInt32(storeRow["Min Ship Qty"], CultureInfo.CurrentUICulture);
                                    try
                                    {
                                        imop.IMOMinShipQty = Convert.ToInt32(storeRow["Min Ship Qty"], CultureInfo.CurrentUICulture);
                                    }
                                    catch (Exception error)
                                    {
										//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        //exceptionMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        exceptionMessage = _Session.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
										IMOe = new IMOErrors();
                                        IMOe.Error = true;
                                        IMOe.StoreRID = storeRID;
                                        IMOe.Message = exceptionMessage;
                                        IMOe.type = "Min Ship Qty";
                                        errorList.Add(IMOe);
                                    }
                                    // End TT#3680 - JSmith - VSW fields accepting invalid values
                                }
                                else
                                { 
                                    imop.IMOMinShipQty = 0;
                                }
                            }
                            else
                            {
                                imop.IMOMinShipQty = 0;
                            }
                            // Begin TT#492-MD - JSmith - FWOS Max errors when removed with space bar
                            //if (storeRow.Cells["FWOS Max"].Text != string.Empty)
                            //BEGIN TT#3635 - DOConnell - Entering space in VSW field break VSW for key item
                            //if (storeRow["FWOS Max"] != DBNull.Value && (String)storeRow["FWOS Max"] != string.Empty)
                            if (storeRow["FWOS Max"] != DBNull.Value && ((String)storeRow["FWOS Max"]).Trim() != string.Empty)
                            //END TT#3635 - DOConnell - Entering space in VSW field break VSW for key item
                            // End TT#492-MD - JSmith - FWOS Max errors when removed with space bar
                            {
                                //BEGIN TT#108 - MD - DOConnell - FWOS Max Model 
                                bool modelYes = false;

                                try
                                {
                                    double FWOSMax = Convert.ToDouble(storeRow["FWOS Max"], CultureInfo.CurrentCulture);
                                    modelYes = false;
                                    imop.IMOFWOS_Max = FWOSMax;
                                    imop.IMOFWOS_MaxModelRID = Include.NoRID;
                                    imop.IMOFWOS_MaxModelName = string.Empty;
                                    imop.IMOFWOS_MaxType = eModifierType.Percent;
                                }
                                catch
                                {
                                    modelYes = true;
                                }

                                if (modelYes)
                                {
                                    imop.IMOFWOS_MaxType = eModifierType.Model;
                                    imop.IMOFWOS_MaxModelName = (string)storeRow["FWOS Max"];
                                    imop.IMOFWOS_MaxModelRID = Convert.ToInt32(storeRow["FWOS Max RID"]);
                                    if (imop.IMOFWOS_MaxModelName != null &&
                                        imop.IMOFWOS_MaxModelName.Trim().Length > 0)
                                    {
                                        if (imop.IMOFWOS_MaxModelRID == Include.NoRID)
                                        {
                                            try
                                            {
                                                //BEGIN TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                                if (storeRow["FWOS Max"] != DBNull.Value)
                                                {
                                                    imop.IMOFWOS_Max = Convert.ToDouble(storeRow["FWOS Max"], CultureInfo.CurrentUICulture);
                                                    imop.IMOFWOS_MaxType = eModifierType.Percent; //TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                                }
                                                //END TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                                //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                                else
                                                {
                                                    imop.IMOFWOS_MaxType = eModifierType.None;
                                                    imop.IMOFWOS_MaxModelName = (string)storeRow["FWOS Max"];
                                                }
                                                //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            }
                                            catch (Exception error)
                                            {
												//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                                //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                                exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                                //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
												IMOe = new IMOErrors();
                                                IMOe.Error = true;
                                                IMOe.StoreRID = storeRID;
                                                IMOe.Message = exceptionMessage;
                                                IMOe.type = "FWOS Max";
                                                errorList.Add(IMOe);
                                                //return errorList;
                                            }
                                        }
                                        else
                                        {
                                            imop.IMOFWOS_MaxType = eModifierType.Model;
                                        }
                                    }
                                }
                                //END TT#108 - MD - DOConnell - FWOS Max Model 
                            }
                            //BEGIN TT#3637 - DOConnell - FWOS Max not saving for VSW Upload
							//BEGIN TT#3655 - DOConnell - Uploading Sales Modifier Value breaks inheritance for other Models
							//else if (storeRow["FWOS Max RID"] != DBNull.Value && ((String)storeRow["FWOS Max RID"]).Trim().Length > 0
                            else if (storeRow["FWOS Max RID"] != DBNull.Value && (Convert.ToString(storeRow["FWOS Max RID"])).Trim().Length > 0
							//END TT#3655 - DOConnell - Uploading Sales Modifier Value breaks inheritance for other Models
                                && Convert.ToInt32(storeRow["FWOS Max RID"]) != Include.NoRID) //TT#3647 - DOConnell - VSW TAB - type in an item max and receive an error.
                            {
                                imop.IMOFWOS_MaxType = eModifierType.Model;
                                imop.IMOFWOS_MaxModelName = (string)storeRow["FWOS Max"];
                                imop.IMOFWOS_MaxModelRID = Convert.ToInt32(storeRow["FWOS Max RID"]);
                                if (imop.IMOFWOS_MaxModelName != null &&
                                    imop.IMOFWOS_MaxModelName.Trim().Length > 0)
                                {
                                    if (imop.IMOFWOS_MaxModelRID == Include.NoRID)
                                    {
                                        try
                                        {
                                            //BEGIN TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            if (storeRow["FWOS Max"] != DBNull.Value)
                                            {
                                                imop.IMOFWOS_Max = Convert.ToDouble(storeRow["FWOS Max"], CultureInfo.CurrentUICulture);
                                            }
                                            //END TT#822 - MD - DOConnell - Node Prop - FWOS Override model cannot be deleted.
                                            imop.IMOFWOS_MaxType = eModifierType.Percent;
                                            imop.IMOFWOS_MaxModelName = (string)storeRow["FWOS Max"];
                                        }
                                        catch (Exception error)
                                        {
											//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                            //exceptionMessage = error.Message + " " + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            exceptionMessage = error.Message + " " + _Session.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
                                            //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
											IMOe = new IMOErrors();
                                            IMOe.Error = true;
                                            IMOe.StoreRID = storeRID;
                                            IMOe.Message = exceptionMessage;
                                            IMOe.type = "FWOS Max";
                                            errorList.Add(IMOe);
                                            //return errorList;
                                        }
                                    }
                                    else
                                    {
                                        imop.IMOFWOS_MaxType = eModifierType.Model;
                                    }
                                }
                            }
                            //END TT#3637 - DOConnell - FWOS Max not saving for VSW Upload
                            else
                            {
                                imop.IMOFWOS_Max = int.MaxValue;
                                // Begin TT#270-MD - JSmith -  FWOS Max Model not holding in Node Properties
                                imop.IMOFWOS_MaxModelRID = Include.NoRID;
                                imop.IMOFWOS_MaxType = eModifierType.None;
                                // End TT#270-MD - JSmith -  FWOS Max Model not holding in Node Properties
                            }
                            //BEGIN TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            if (storeRow["Pct Pack Threshold"] != DBNull.Value)
                            //if (storeRow["Pct Pack Threshold"] != string.Empty)
                            //END TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            {
                                // Begin TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                //if ((String)storeRow["Pct Pack Threshold"] != string.Empty)
                                if (Convert.ToString(storeRow["Pct Pack Threshold"]).Trim() != string.Empty)
                                // End TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                {
                                    imop.IMOPackQty = (Convert.ToDouble(storeRow["Pct Pack Threshold"], CultureInfo.CurrentUICulture) / 100);
                                }
                                else
                                {
                                    imop.IMOPackQty = Include.PercentPackThresholdDefault;
                                }
                            }
                            else
                            {
                                imop.IMOPackQty = Include.PercentPackThresholdDefault;
                            }
                            //BEGIN TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            if (storeRow["Push to Backstock"] != DBNull.Value)
                            //if (storeRow["Push to Backstock"] != string.Empty )
                            //BEGIN TT#798 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 -  Relocate UpdateIMOData to hierarchy service 
                            {
                                // Begin TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                //if ((String)storeRow["Push to Backstock"] != string.Empty)
                                if (Convert.ToString(storeRow["Push to Backstock"]).Trim() != string.Empty)
                                // End TT#3671 - JSmith - VSW - Cannot clear Min Ship Qty adn % Pack Threshold
                                {
                                    // Begin TT#3680 - JSmith - VSW fields accepting invalid values
                                    //imop.IMOPshToBackStock = Convert.ToInt32(storeRow["Push to Backstock"], CultureInfo.CurrentUICulture);
                                    try
                                    {
                                        imop.IMOPshToBackStock = Convert.ToInt32(storeRow["Push to Backstock"], CultureInfo.CurrentUICulture);
                                    }
                                    catch (Exception error)
                                    {
										//BEGIN TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
                                        //exceptionMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        exceptionMessage = _Session.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
                                        //END TT#3707 - DOConnell - Store Eligibility - Problem processing transaction file
										IMOe = new IMOErrors();
                                        IMOe.Error = true;
                                        IMOe.StoreRID = storeRID;
                                        IMOe.Message = exceptionMessage;
                                        IMOe.type = "Push to Backstock";
                                        errorList.Add(IMOe);
                                    }
                                    // End TT#3680 - JSmith - VSW fields accepting invalid values
                                }
                                else
                                {
                                    imop.IMOPshToBackStock = Include.NoRID;
                                }
                            }
                            else
                            {
                                imop.IMOPshToBackStock = Include.NoRID;
                            }
                            if (imop.IsDefaultValues)
                                imop.IMOChangeType = eChangeType.delete;

                            imop.IMOIsInherited = false;
                            imop.IMOInheritedFromNodeRID = Include.NoRID;
                            if (!profileFound)
                            {
                                IMOprofList.Add(imop);
                                IMOe = new IMOErrors();
                                IMOe.Error = false;
                                IMOe.ProfList = IMOprofList;
                                errorList.Add(IMOe);
                            }
                            else
                            {
                                IMOprofList.Update(imop);
                                IMOe = new IMOErrors();
                                IMOe.Error = false;
                                IMOe.ProfList = IMOprofList;
                                errorList.Add(IMOe);
                            }
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDHierarchyService", ex.Message, EventLogEntryType.Error);
            }
            return errorList;
        }

    }
}
