using System;
using System.Collections;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for HierarchyLoadProcessBase.
	/// </summary>
	public class HierarchyLoadProcessBase
	{
		//=======
		// FIELDS
		//=======
		private SessionAddressBlock _SAB;
		private HierarchyMaintenance _hm = null;
		private int _recordsWithErrors = 0;
		private int _recordsNotCommitted = 0;
		private int _recordsRead = 0;
		private int _hierarchyRecs = 0;
		private int _levelRecs = 0;
		private int _productRecs = 0;
        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
        private int _productRecsAdded = 0;
        private int _productRecsUpdated = 0;
        //ENd TT#106 MD
		private int _moveRecs = 0;
		private int _renameRecs = 0;
		private int _deleteRecs = 0;
		private int _commitLimit = 1000;
		private Hashtable _forecastVersions;
		private Hashtable _rollupForecastVersions;
		private bool _reclassPreview = false;
		// Begin Track #5259 - JSmith - Add new reclass roll options
		private bool _rollExternalIntransit = false;
		private bool _rollAlternateHierarchies = false;
		// End Track #5259
        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        private string _characteristicDelimiter;
        // End TT#167
		private string _transactionLabel;
		private string _moveMessage;
		private string _renameMessage;
		private string _deleteMessage;
		private string _actionDelete = null;
		private string _actionMove = null;
		private string _actionRename = null;
		private bool _addCharacteristicGroups;
		private bool _addCharacteristicValues;

		//=============
		// CONSTRUCTORS
		//=============

		public HierarchyLoadProcessBase(SessionAddressBlock SAB, int commitLimit, bool addCharacteristicGroups, bool addCharacteristicValues)
		{
			_commitLimit = commitLimit;
			_SAB = SAB;
			_addCharacteristicGroups = addCharacteristicGroups;
            // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
            // Always allow values to be added
            //_addCharacteristicValues = addCharacteristicValues;
            _addCharacteristicValues = true;
            // End TT#2010
			
            // Begin TT#4053 - JSmith - Hierarchy Timeout
            //_hm = new HierarchyMaintenance(SAB, SAB.HierarchyServerSession);
            _hm = new HierarchyMaintenance(SAB: SAB, aSession: SAB.HierarchyServerSession, aLookupAlternateAPIRollupExists: true);
            // End TT#4053 - JSmith - Hierarchy Timeout

			ForecastVersion forecastVersionData = new ForecastVersion();
			_forecastVersions = forecastVersionData.GetForecastVersionsHashtable(false, true);
			_rollupForecastVersions = new Hashtable();
			_transactionLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Transaction);
			_moveMessage = MIDText.GetText(eMIDTextCode.msg_ReclassMove);
			_renameMessage = MIDText.GetText(eMIDTextCode.msg_ReclassRename);
			_deleteMessage = MIDText.GetText(eMIDTextCode.msg_ReclassDelete);
			_actionDelete = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Delete);
			_actionMove = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Move);
			_actionRename = MIDText.GetTextOnly(eMIDTextCode.lbl_Action_Rename);
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the SessionAddressBlock
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		/// <summary>
		/// Gets the CommitLimit
		/// </summary>

		public int CommitLimit
		{
			get
			{
				return _commitLimit;
			}
		}

		/// <summary>
		/// Gets the flag identifying if characteristic groups can be added
		/// </summary>

		public bool AutoAddCharacteristics
		{
			get
			{
				return _addCharacteristicGroups;
			}
            // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
            set
            {
                _addCharacteristicGroups = value;
            }
            // End TT#167
		}

		/// <summary>
		/// Gets the flag identifying if characteristic values can be added
		/// </summary>

		public bool AutoAddCharacteristicValues
		{
			get
			{
				return _addCharacteristicValues;
			}
            // Begin TT#224 - JSmith - Options=True does not allow dynamic characteristics
            set
            {
                // Begin TT#2010 - JSmith - Split merchandise characteristics to separate delimited transaction layout
                // Always allow values to be added
                //_addCharacteristicValues = value;
                _addCharacteristicValues = true;
                // End TT#2010
            }
            // End TT#224
		}

		/// <summary>
		/// Gets the HierarchyMaintenance object
		/// </summary>

		public HierarchyMaintenance HierarchyMaintenance
		{
			get
			{
				return _hm;
			}
		}

		/// <summary>
		/// Gets or sets the number of records with errors
		/// </summary>

		public int RecordsWithErrors
		{
			get
			{
				return _recordsWithErrors;
			}
			set
			{
				_recordsWithErrors = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of records that have not been committed
		/// </summary>

		public int RecordsNotCommitted
		{
			get
			{
				return _recordsNotCommitted;
			}
			set
			{
				_recordsNotCommitted = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of records that have not been read
		/// </summary>

		public int RecordsRead
		{
			get
			{
				return _recordsRead;
			}
			set
			{
				_recordsRead = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of hierarchy records
		/// </summary>

		public int HierarchyRecs
		{
			get
			{
				return _hierarchyRecs;
			}
			set
			{
				_hierarchyRecs = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of level records
		/// </summary>

		public int LevelRecs
		{
			get
			{
				return _levelRecs;
			}
			set
			{
				_levelRecs = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of product records
		/// </summary>

		public int ProductRecs
		{
			get
			{
				return _productRecs;
			}
			set
			{
				_productRecs = value;
			}
		}

        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
        /// <summary>
        /// Gets or sets the number of product records added
        /// </summary>

        public int ProductRecsAdded
        {
            get
            {
                return _productRecsAdded;
            }
            set
            {
                _productRecsAdded = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of product records updated
        /// </summary>

        public int ProductRecsUpdated
        {
            get
            {
                return _productRecsUpdated;
            }
            set
            {
                _productRecsUpdated = value;
            }
        }
        //End TT#106 MD

		/// <summary>
		/// Gets or sets the number of reclass move records
		/// </summary>

		public int MoveRecs
		{
			get
			{
				return _moveRecs;
			}
			set
			{
				_moveRecs = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of reclass rename records
		/// </summary>

		public int RenameRecs
		{
			get
			{
				return _renameRecs;
			}
			set
			{
				_renameRecs = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of reclass delete records
		/// </summary>

		public int DeleteRecs
		{
			get
			{
				return _deleteRecs;
			}
			set
			{
				_deleteRecs = value;
			}
		}

		/// <summary>
		/// Gets the Hashtable containing all forecast version
		/// </summary>

		public Hashtable ForecastVersions
		{
			get
			{
				return _forecastVersions;
			}
		}

		/// <summary>
		/// Gets or sets the Hashtable containing the forecast versions that are to be rolled
		/// </summary>

		public Hashtable RollupForecastVersions
		{
			get
			{
				return _rollupForecastVersions;
			}
			set
			{
				_rollupForecastVersions = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the reclass process is preview only
		/// </summary>

		public bool ReclassPreview
		{
			get
			{
				return _reclassPreview;
			}
			set
			{
				_reclassPreview = value;
				if (_reclassPreview)
				{
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_ReclassPreview, GetType().Name, true);
					SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(null, "Options","Reclass Preview", MIDText.GetText(eMIDTextCode.msg_ReclassPreview));
				}
//				else
//				{
//					SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(null, "Options","Reclass Preview", MIDText.GetText(eMIDTextCode.msg_ReclassPreviewTurnedOff));
//				}
			}
		}

		// Begin Track #5259 - JSmith - Add new reclass roll options
		/// <summary>
		/// Gets or sets the flag identifying if the reclass process is to schedule external intransit to roll
		/// </summary>

		public bool RollExternalIntransit
		{
			get
			{
				return _rollExternalIntransit;
			}
			set
			{
				_rollExternalIntransit = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the reclass process is to schedule alternate hierarchies to roll
		/// </summary>

		public bool RollAlternateHierarchies
		{
			get
			{
				return _rollAlternateHierarchies;
			}
			set
			{
				_rollAlternateHierarchies = value;
			}
		}
		// End Track #5259

        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        /// <summary>
        /// Gets or sets the field used to delimit the characteristic information
        /// </summary>

        public string CharacteristicDelimiter
        {
            get
            {
                return _characteristicDelimiter;
            }
            set
            {
                _characteristicDelimiter = value;
            }
        }
        // End TT#167

		//========
		// METHODS
		//========

		protected int AddHierarchyRecord(string aHierarchyID, string aHierarchyType, string aColor)
		{
			EditMsgs em = new EditMsgs();
			string hierarchyID = null;
			string hierarchyType = null;
			string hierarchyColor = null;
			int returnCode = 0;

			try
			{
				++HierarchyRecs;

				if (aHierarchyID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, "Record=" + HierarchyRecs.ToString(CultureInfo.CurrentUICulture), GetType().Name);
				}
				else
				{
					hierarchyID = aHierarchyID;
				}
				
				hierarchyType = aHierarchyType;
				
				if (aColor == null)   
				{
					em.AddMsg(eMIDMessageLevel.Information, eMIDTextCode.msg_HierarchyColorNotFound, "Record=" + HierarchyRecs.ToString(CultureInfo.CurrentUICulture), GetType().Name);
					hierarchyColor = Include.MIDDefaultColor;
				}
				else
				{
					hierarchyColor = aColor;
				}
					
				HierarchyMaintenance.ProcessHierarchyData(ref em, hierarchyID, hierarchyType, hierarchyColor);
				
				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					string msgDetails = "Hierarchy=" + hierarchyID + " : " + emm.msg;
					SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
						emm.module);
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			 
			return returnCode;

		}

		protected int AddLevelRecord(string aHierarchyID, int aLevelSequence, string aLevelName, 
			string aLevelType, string aPlanLevelType, string aLevelLengthType,
			int aRequiredSize, int aRangeFrom, int aRangeTo, string aColor, bool aIsOTSForecastLevel)
		{
			EditMsgs em = new EditMsgs();
			string hierarchyID = null;
			string levelID = null;
			string levelLengthType = null;
			string levelType = null;
			string OTSPlanLevelType = "Undefined";
			string levelColor = null;
			int requiredSize = 0;
			int rangeFrom = 0;
			int rangeTo = 0;
			int returnCode = 0;

			try
			{
				++LevelRecs;
				if (aHierarchyID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, "Record=" + LevelRecs.ToString(CultureInfo.CurrentUICulture), GetType().Name);
				}
				else
				{
					hierarchyID = aHierarchyID;
				}
				
				if (aLevelName == null)   
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_LevelNameRequired, "Record=" + LevelRecs.ToString(CultureInfo.CurrentUICulture), GetType().Name);
				}
				else
				{
					levelID = aLevelName;
				}

				levelType = aLevelType;
				OTSPlanLevelType = aPlanLevelType;
				levelLengthType = aLevelLengthType;			
				requiredSize = aRequiredSize;
				rangeFrom = aRangeFrom;
				rangeTo = aRangeTo;


				if (aColor == null)   
				{
					em.AddMsg(eMIDMessageLevel.Information, eMIDTextCode.msg_LevelColorNotFound, "Record=" + LevelRecs.ToString(CultureInfo.CurrentUICulture), GetType().Name);
					levelColor = Include.MIDDefaultColor;
				}
				else
				{
					levelColor = aColor;
				}
				
				HierarchyMaintenance.ProcessLevelData(ref em, hierarchyID, aLevelSequence, levelID, levelType, OTSPlanLevelType, levelLengthType, 
					levelColor, requiredSize, rangeFrom, rangeTo, aIsOTSForecastLevel);
				
				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					string msgDetails = "Hierarchy=" + hierarchyID + " : " + "Level=" + levelID + " : " + emm.msg;
					SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
						emm.module);
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			
			return returnCode;
		}

				
		protected int AddMerchandiseRecord(string aHierarchyID, string aParent, string aID, string aName, 
			string aDescription, string aProductType,
			string aSizeProductCategory, string aSizeCodePrimary, string aSizeCodeSecondary,
			string aOTSForecastLevel, string aOTSForecastLevelHierarchy, string aOTSForecastLevelType,
            string aOTSForecastNodeSelect, string aOTSForecastStartsWith, string aApplyNodeProperties, out bool aProcessRecord, bool aBuildCacheOnly)  // TT#3546 - JSmith - Alternate Hierarchy Load Error  
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string hierarchyID = null;
			string parentID = null;
			string nodeID = null;
			string nodeName = null;
			string description = null;
			string productType = null;
			string message = null;
            aProcessRecord = false;  // TT#3546 - JSmith - Alternate Hierarchy Load Error  

			try
			{
                if (aBuildCacheOnly)  // TT#3708 - JSmith - Summary total Product Records processed is incorrect
                {
                    ++ProductRecs;
                }
				message =  "Hierarchy: " + aHierarchyID + "; Parent: " + aParent + "; ID: " + aID + "; Name: " + aName + "; Description: " + aDescription;
				if (aHierarchyID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					hierarchyID = aHierarchyID;
				}
				
				if (aParent == null)   
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_ParentRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					parentID = aParent;
				}

                if ((aID == null) || (aID.Trim() == String.Empty))  //Track #6241 - KJohnson - Incomplete message when product not provided (Blank Specifically)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_ProductRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					nodeID = aID;
					//BEGIN TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                    //if (aName == null)
                    //{
                    //    nodeName = nodeID;
                    //}
                    //else
                    //{
                    //    nodeName = aName;
                    //}
                    //BEGIN TT#4488 - DOConnell - Node name does not default to ID if not provided
                    //if (aName != null)
                    //{
                    if (aName == null || aName == string.Empty)
                    //if (aName == string.Empty)
                    {
                        nodeName = nodeID;
                    }
                    else
                    {
                        nodeName = aName;
                    }
                    //}
                    //END TT#4488 - DOConnell - Node name does not default to ID if not provided
					//END TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
					if (aDescription == null)   
					{
                        // Begin TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
                        //description = nodeName;
                        description = null;
                        // End TT#2773 - JSmith - Color Description does not appear to be adding correctly as part of auto-add
					}
					else
					{
						description = aDescription;
					}
				}

				if (aProductType == null)
				{
					productType = "undefined";
				}
				else
				{
					productType = aProductType;
				}
			
				try
				{
                    //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                    //HierarchyMaintenance.ProcessNodeProfileInfo(ref em, hierarchyID, parentID, nodeID, nodeName, description, productType,
                    //    aSizeProductCategory, aSizeCodePrimary, aSizeCodeSecondary, aOTSForecastLevel,
                    //    aOTSForecastLevelHierarchy, aOTSForecastLevelType, aOTSForecastNodeSelect, aOTSForecastStartsWith, aApplyNodeProperties);
                    eChangeType changeType;
                    HierarchyMaintenance.ProcessNodeProfileInfo(ref em, hierarchyID, parentID, nodeID, nodeName, description, productType,
                        aSizeProductCategory, aSizeCodePrimary, aSizeCodeSecondary, aOTSForecastLevel,
                        aOTSForecastLevelHierarchy, aOTSForecastLevelType, aOTSForecastNodeSelect, aOTSForecastStartsWith, aApplyNodeProperties,
                        out changeType, out aProcessRecord, aBuildCacheOnly);  // TT#3546 - JSmith - Alternate Hierarchy Load Error
                    if (!aBuildCacheOnly)  // TT#3546 - JSmith - Alternate Hierarchy Load Error
                    {
                        switch (changeType)
                        {
                            case eChangeType.add:
                                ++ProductRecsAdded;
                                break;
                            case eChangeType.update:
                                ++ProductRecsUpdated;
                                break;
                        }
                    }
                    //End TT#106 MD - JSmith
				}
				catch (Exception err)
				{
					em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
				}

                if (!aBuildCacheOnly)  // TT#3546 - JSmith - Alternate Hierarchy Load Error
                {
                    for (int e = 0; e < em.EditMessages.Count; e++)
                    {
                        EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
                        string msgDetails = "Hierarchy=" + hierarchyID + " : " + "Product=" + nodeID + " : " + emm.msg;
                        SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
                            emm.module);
                        if (emm.messageLevel > eMIDMessageLevel.Information)
                        {
                            returnCode = 1;
                        }
                    }
                    if (returnCode != 0)
                    {
                        ++RecordsWithErrors;
                    }
				//BEGIN TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
                }
                else
                {
                    if (em.EditMessages.Count > 0)
                    {
                        aProcessRecord = true;
                    }
                }
				//END TT#3869 - DOConnell - Cannot set OTS Forecast Default in Hierarchy Load API
			}
			catch ( Exception err )
			{
				message = err.ToString();
				throw;
			}
			
			return returnCode;

		}

		/// <summary>
		/// Adds a characteristic to a product
		/// </summary>
		/// <param name="aID">The ID of the product</param>
		/// <param name="aCharacteristic">User defined characteristic</param>
		/// <param name="aValue">Value of the characteristic</param>
		/// <returns></returns>
        // Begin TT#167 - JSmith - Product Hierarchy characteristics auto add
        //protected int AddProductCharacteristic(string aHierarchyID, string aID, string aCharacteristic, string aValue)
        // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
        //protected int AddProductCharacteristic(string aHierarchyID, string aID, string aCharacteristic, string aValue, string aType)
        protected int AddProductCharacteristic(string aHierarchyID, string aParent, string aID, string aCharacteristic, string aValue, string aType)
        // End TT#298
        // End TT#167
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string message = null;
			string hierarchyID = null;
			string nodeID = null;
			try
			{
                // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                //message = "Hierarchy: " + aHierarchyID + "; ID: " + aID + "; Characteristic: " + aCharacteristic + "; Value: " + aValue;
                message = "Hierarchy: " + aHierarchyID + "; Parent: " + aParent + "; ID: " + aID + "; Characteristic: " + aCharacteristic + "; Value: " + aValue;
                // End TT#298
				if (aHierarchyID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					hierarchyID = aHierarchyID;
				}

				if (aID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_ProductRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					nodeID = aID;
				}

				if (aCharacteristic != null)
				{
					aCharacteristic = aCharacteristic.Trim();
				}
				if (aValue != null)
				{
					aValue = aValue.Trim();
				}

				if (!em.ErrorFound)
				{
					try
					{
                        // Begin TT#298 - JSmith - Transactions with color as the ID parent is style) reject with "Product not on file" followed by "Object reference" error.
                        //HierarchyMaintenance.AddProductCharacteristicValue(ref em, AutoAddCharacteristics,
                        //    AutoAddCharacteristicValues, aID, aCharacteristic, aValue);
						HierarchyMaintenance.AddProductCharacteristicValue(ref em, AutoAddCharacteristics,
                            AutoAddCharacteristicValues, aParent, aID, aCharacteristic, aValue);
                        // End TT#298
					}
					catch (Exception err)
					{
						em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
					}
				}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
					string msgDetails = "Hierarchy:" + hierarchyID 
						+ "; Product:" + nodeID 
						+ "; Characteristic: " + aCharacteristic 
						+ "; Value: " + aValue 
						+ " : " + emm.msg;
					SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
						emm.module);
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
				return returnCode;
			}
			catch (Exception err)
			{
				message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Writes the characteristics for a product
		/// </summary>
		/// <param name="aID">The ID of the product</param>
		/// <returns></returns>
		protected int WriteProductCharacteristics(string aHierarchyID, string aID)
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string message = null;
			string hierarchyID = null;
			string nodeID = null;
			try
			{
				message = "Hierarchy: " + aHierarchyID + "; ID: " + aID;
				if (aHierarchyID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_HierarchyRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					hierarchyID = aHierarchyID;
				}

				if (aID == null)
				{
					em.AddMsg(eMIDMessageLevel.Severe, eMIDTextCode.msg_ProductRequired, "Record=" + ProductRecs.ToString(CultureInfo.CurrentUICulture) + "  " + message, GetType().Name);
				}
				else
				{
					nodeID = aID;
				}

				
					try
					{
						HierarchyMaintenance.WriteProductCharacteristicValues(ref em, aID);
					}
					catch (Exception err)
					{
						em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
					}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
					string msgDetails = "Hierarchy=" + hierarchyID + " : " + "Product=" + nodeID + " : " + emm.msg;
					SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
						emm.module);
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				return returnCode;
			}
			catch (Exception err)
			{
				message = err.ToString();
				throw;
			}
		}


		// Begin Track #5259 - JSmith - Add new reclass roll options
//		protected int MoveMerchandiseRecord(string aHierarchyID, string aParent, string aNodeID, string aToParent,
//			bool aReclassPreview, Hashtable aRollupForecastVersions)
		protected int MoveMerchandiseRecord(string aHierarchyID, string aParent, string aNodeID, string aToParent,
			bool aReclassPreview, Hashtable aRollupForecastVersions, bool aRollExternalIntransit, bool aRollAlternateHierarchies)
		// End Track #5259
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string message = null;

			try
			{
				++_moveRecs;
				message = (string)_moveMessage.Clone();
				message = message.Replace("{0}", aNodeID);
				message = message.Replace("{1}", aParent);
				message = message.Replace("{2}", aToParent);
				message = message.Replace("{3}", aHierarchyID);
				if (!aReclassPreview)
				{
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true); 
				}
				SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionMove, _transactionLabel, null, message);

				try
				{
					// Begin Track #5259 - JSmith - Add new reclass roll options
//					HierarchyMaintenance.MoveNode(ref em, aHierarchyID, aParent, aNodeID, aToParent, aReclassPreview, aRollupForecastVersions);
					HierarchyMaintenance.MoveNode(ref em, aHierarchyID, aParent, aNodeID, aToParent, aReclassPreview, aRollupForecastVersions, 
						aRollExternalIntransit, aRollAlternateHierarchies);
					// End Track #5259
				}
				catch (Exception err)
				{
					em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
				}
				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					string msgDetails = "Hierarchy=" + aHierarchyID + " : " + "Parent=" + aParent + " : "+ "Product=" + aNodeID + " : ";
//					string msgText = null;
					if (emm.msg != null)
					{
						msgDetails += emm.msg;
					}
					else if (emm.code != eMIDTextCode.Unassigned)
					{
						msgDetails += SAB.HierarchyServerSession.Audit.GetText(emm.code, false);
					}
					SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionMove, _transactionLabel, aNodeID, msgDetails);
					if (!aReclassPreview)
					{
						SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
							emm.module);
					}
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
			}
			catch ( Exception err )
			{
				message = err.ToString();
				throw;
			}
			
			return returnCode;

		}

		// Begin Track #5259 - JSmith - Add new reclass roll options
//		protected int DeleteMerchandiseRecord(string aHierarchyID, string aParent, string aNodeID,
//			string aReplaceWith, bool aForceDelete,  bool aReclassPreview, Hashtable aRollupForecastVersions)
		protected int DeleteMerchandiseRecord(string aHierarchyID, string aParent, string aNodeID,
			string aReplaceWith, bool aForceDelete,  bool aReclassPreview, Hashtable aRollupForecastVersions,
			bool aRollExternalIntransit, bool aRollAlternateHierarchies)
		// End Track #5259
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string message = null;

			try
			{
				++_deleteRecs;
				message = (string)_deleteMessage.Clone();
				message = message.Replace("{0}", aNodeID);
				message = message.Replace("{1}", aParent);
				message = message.Replace("{2}", aHierarchyID);
				if (!aReclassPreview)
				{
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true); 
				}
				SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionDelete, _transactionLabel, null, message);
				try
				{
					// Begin Track #5259 - JSmith - Add new reclass roll options
//					HierarchyMaintenance.DeleteNode(ref em, aHierarchyID, aParent, aNodeID, aReplaceWith, 
//						aForceDelete, aReclassPreview, aRollupForecastVersions);
					HierarchyMaintenance.DeleteNode(ref em, aHierarchyID, aParent, aNodeID, aReplaceWith, 
						aForceDelete, aReclassPreview, aRollupForecastVersions, aRollExternalIntransit,
						aRollAlternateHierarchies);
					// End Track #5259
				}
				catch (Exception err)
				{
					em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
				}
				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					string msgDetails = "Hierarchy=" + aHierarchyID + " : " + "Parent=" + aParent + " : "+ "Product=" + aNodeID + " : ";
//					string msgText = null;
					if (emm.msg != null)
					{
						msgDetails += emm.msg;
					}
					else if (emm.code != eMIDTextCode.Unassigned)
					{
						msgDetails += SAB.HierarchyServerSession.Audit.GetText(emm.code, false);
					}
					SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionDelete, _transactionLabel, aNodeID, msgDetails);
					if (!aReclassPreview)
					{
						SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
							emm.module);
					}
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
			}
			catch ( Exception err )
			{
				message = err.ToString();
				throw;
			}
			
			return returnCode;

		}

		protected int RenameMerchandiseRecord(string aHierarchyID, string aParent, string aNodeID, string aNewID, 
			string aName, string aDescription, bool aReclassPreview, bool aRenameDuringMove)
		{
			int returnCode = 0;
			EditMsgs em = new EditMsgs();
			string message = null;

			try
			{
				if (!aRenameDuringMove)
				{
					++_renameRecs;
				}
				message = (string)_renameMessage.Clone();
				message = message.Replace("{0}", aNodeID);
				message = message.Replace("{1}", aNewID);
				message = message.Replace("{2}", aName);
				message = message.Replace("{3}", aDescription);
				if (!aReclassPreview)
				{
					SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true); 
				}
				SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionRename, _transactionLabel, null, message);
			
				try
				{
					HierarchyMaintenance.RenameNode(ref em, aHierarchyID,  aParent,  aNodeID,  aNewID, 
						aName,  aDescription, aReclassPreview);

				}
				catch (Exception err)
				{
					em.AddMsg(eMIDMessageLevel.Error, err.Message, GetType().Name);
				}
				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					string msgDetails = "Hierarchy=" + aHierarchyID + " : " + "Parent=" + aParent + " : "+ "Product=" + aNodeID + " : ";
//					string msgText = null;
					if (emm.msg != null)
					{
						msgDetails += emm.msg;
					}
					else if (emm.code != eMIDTextCode.Unassigned)
					{
						msgDetails += SAB.HierarchyServerSession.Audit.GetText(emm.code, false);
					}
					SAB.HierarchyServerSession.Audit.AddReclassAuditMsg(_actionRename, _transactionLabel, aNodeID, msgDetails);
					if (!aReclassPreview)
					{
						SAB.HierarchyServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, msgDetails, emm.lineNumber,
							emm.module);
					}
					if (emm.messageLevel > eMIDMessageLevel.Information)
					{
						returnCode = 1;
					}
				}
				if (returnCode != 0)
				{
					++RecordsWithErrors;
				}
			}
			catch ( Exception err )
			{
				message = err.ToString();
				throw;
			}
			
			return returnCode;

		}
	}
}
