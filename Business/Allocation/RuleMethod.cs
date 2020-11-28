using System;
using System.Collections;
using System.Collections.Generic; // TT#575 - MD - Assortment Jellis
using System.Data;
using System.Globalization;
using System.Data.SqlTypes;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Defines the rule allocation method.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class RuleMethod:AllocationBaseMethod
	{
		//=======
		// FIELDS
		//=======
		private RuleMethodData				_methodData;
		private int							_filterRID; 
        private int							_headerRID;       
		private string						_headerID;               
		// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		private bool						_isHeaderMaster;
		// (CSMITH) - END MID Track #3219
		private eSortDirection				_sortDirection;
		private eComponentType				_componentType;
		private int							_packRID;
		private string						_packName;
		private int							_colorCodeRID;
		private eRuleMethod					_includeRuleMethod;
		private double						_includeQuantity;
		private eRuleMethod					_excludeRuleMethod;
		private double						_excludeQuantity;
		private int							_storeGroupLevelRID;
		private RuleAllocationProfile		_ruleAllocProfile;
		private AllocationProfile			_basisAllocProfile;
        private bool                        _includeReserve;      //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
//		private GeneralComponent			_targetComponent;
		private GeneralComponent			_basisComponent;
		private string                      _globalUserTypeText;  // MID Change j.ellis Add Audit Message
		private Hashtable					_masterKeysSubKeys;
		private DataTable					_masterPacks;
		private DataView					_masterView;
		private DataTable					_compPacks;
		private DataView					_compView;
		private ArrayList					_masterRemoveList; 
		private ArrayList					_subRemoveList; 
        private int                         _hdrBCRID;		      // Assortment  

		//=============
		// CONSTRUCTORS
		//=============
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public RuleMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.Rule)
		public RuleMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.Rule, eProfileType.MethodRule)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			if (base.Filled)
			{
				if (base.MethodType != eMethodType.Rule)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_NotRuleMethod),
						MIDText.GetText(eMIDTextCode.msg_NotRuleMethod));
				}
				_methodData = new RuleMethodData(base.Key, eChangeType.populate);
				_filterRID = _methodData.Store_Filter_RID; 
        		_headerRID = _methodData.HDR_RID;       
				if (_headerRID != Include.NoRID)
				{
					Header header = new Header();
					DataTable dt = header.GetHeader(_headerRID);
					if(dt.Rows.Count != 0)
					{
						DataRow dr = dt.Rows[0];
						_headerID = Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentUICulture);
					}
				}
				_sortDirection = _methodData.Store_Order;
				_componentType = _methodData.Header_Component;
				_packRID = _methodData.Header_Pack_RID;
				_colorCodeRID = _methodData.Color_Code_RID;
                // Assortment BEGIN
                _hdrBCRID = _methodData.Hdr_BC_RID;
                // Assortment END
				_includeRuleMethod = _methodData.Included_Stores;
				_includeQuantity = _methodData.Included_Quantity;
				_excludeRuleMethod = _methodData.Excluded_Stores;
				_excludeQuantity = _methodData.Excluded_Quantity;
				_storeGroupLevelRID = _methodData.SGL_RID;
                IncludeReserve = Include.ConvertCharToBool(_methodData.Include_Reserve_Ind); ////TT#1608-MD - SRisch - Prior Header
				// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				// begin MID Track 3623 Port Master Allocation
				//_isHeaderMaster = Include.ConvertCharToBool(_methodData.Is_Header_Master);
				if (this.SAB.ApplicationServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
				{
					_isHeaderMaster = Include.ConvertCharToBool(_methodData.Is_Header_Master);
				}
				else
				{
					_isHeaderMaster = false;
				}
				// end MID Track 3623 Port Master Allocation
				// (CSMITH) - END MID Track #3219
			}
			else
			{
				_filterRID = Include.NoRID; 
				_headerRID = Include.NoRID;       
				_sortDirection = eSortDirection.Descending;
				_componentType = eComponentType.Total;
				_packRID = Include.NoRID;
				_colorCodeRID = Include.NoRID;
                // Assortment BEGIN
                _hdrBCRID = Include.NoRID;
                // Assortment END
				_includeRuleMethod = eRuleMethod.None;
                IncludeReserve = SAB.ApplicationServerSession.GlobalOptions.PriorHeaderIncludeReserve; //TT#1608-MD - SRisch - Prior Header
				_includeQuantity = 0;
				_excludeRuleMethod = eRuleMethod.None;
				_excludeQuantity = 0;
				_storeGroupLevelRID  = Include.NoRID;
				// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				_isHeaderMaster = false;
				// (CSMITH) - END MID Track #3219
			}
            _globalUserTypeText = null; // MID Change j.ellis Add Audit Message
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Get Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodRule;
			}
		}

		/// <summary>
		/// Gets or sets the record ID of the filter to use while applying the rule.
		/// </summary>
		public int FilterRID
		{
			get	{return _filterRID;}
			set	{_filterRID = value;}
		}
		/// <summary>
		/// Gets or sets the record ID of the header.
		/// </summary>
		public int HeaderRID
		{
			get	{return _headerRID;}
			set	{_headerRID = value;}
		}
		/// <summary>
		/// Gets or sets the ID of the header.
		/// </summary>
		public string HeaderID
		{
			get	{return _headerID;}
			set	{_headerID = value;}
		}
		/// <summary>
		/// Gets or sets the sort direction to use for applying the rule.
		/// </summary>
		public eSortDirection SortDirection
		{
			get	{return _sortDirection;}
			set	{_sortDirection = value;}
		}
		/// <summary>
		/// Gets or sets the type of component to use for applying the rule.
		/// </summary>
		public eComponentType ComponentType
		{
			get	{return _componentType;}
			set	{_componentType = value;}
		}
		/// <summary>
		/// Gets or sets the record ID of the pack to use for applying the rule.
		/// </summary>
		public int PackRID
		{
			get	{return _packRID;}
			set	{_packRID = value;}
		}
		/// <summary>
		/// Gets or sets the name of the pack to use for applying the rule.
		/// </summary>
		public string PackName
		{
			get	{return _packName;}
			set	{_packName = value;}
		}
		/// <summary>
		/// Gets or sets the record ID of the color code to use for applying the rule.
		/// </summary>
		public int ColorCodeRID
		{
			get	{return _colorCodeRID;}
			set	{_colorCodeRID = value;}
		}
        /// <summary>
        /// Gets or sets the record ID of the color code to use for applying the rule.
        /// </summary>
        public int HdrBCRID
        {
            get { return _hdrBCRID; }
            set { _hdrBCRID = value; }
        }
		/// <summary>
		/// Gets or sets the rule method to use for the included stores.
		/// </summary>
		public eRuleMethod IncludeRuleMethod
		{
			get	{return _includeRuleMethod;}
			set	{_includeRuleMethod = value;}
		}
		/// <summary>
		/// Gets or sets the quantity to use for all rule methods that require a quantity for the included stores.
		/// </summary>
		public double IncludeQuantity
		{
			get	{return _includeQuantity;}
			set	{_includeQuantity = value;}
		}

        //Begin TT#1608-MD - Prior Header - Include Reserve -SRisch  06/02/2015
        /// <summary>
        /// Gets or sets the Include Reserve Prior Header
        /// </summary>
        public bool IncludeReserve
        {
            get { return _includeReserve; }
            set { _includeReserve = value; }
        }
        //END TT#1608-MD - Prior Header - Include Reserve -SRisch  06/02/2015
		/// <summary>
		/// Gets or sets the rule method to use for the excluded stores.
		/// </summary>
		public eRuleMethod ExcludeRuleMethod
		{
			get	{return _excludeRuleMethod;}
			set	{_excludeRuleMethod = value;}
		}
		/// <summary>
		/// Gets or sets the quantity to use for all rule methods that require a quantity for the excluded stores.
		/// </summary>
		public double ExcludeQuantity
		{
			get	{return _excludeQuantity;}
			set	{_excludeQuantity = value;}
		}
		/// <summary>
		/// Gets or sets the store group record ID to use for the method.
		/// </summary>
		public int StoreGroupLevelRID
		{
			get	{return _storeGroupLevelRID;}
			set	{_storeGroupLevelRID = value;}
		}
		// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		/// <summary>
		/// Gets or sets the Is Header Master indicator.
		/// </summary>
		public bool IsHeaderMaster
		{
			get
			{
				return _isHeaderMaster;
			}
			set
			{
				_isHeaderMaster = value;
			}
		}
		// (CSMITH) - END MID Track #3219

		//========
		// METHODS
		//========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(FilterRID))
            {
                return true;
            }

            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			// Begin Track #5727 stodd
			try
			{
				_basisAllocProfile = null;

				aApplicationTransaction.ResetAllocationActionStatus();

				ArrayList selectedComponentList = aApplicationTransaction.GetSelectedComponentList();

				//============================================
				// Allocation Component (Pack, BulkColor...)
				//============================================
				if (selectedComponentList.Count > 0)
				{
					AllocationProfileList apl = (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation);
					AllocationProfile ap = (AllocationProfile)apl[0];

					foreach (GeneralComponentWrapper gcw in selectedComponentList)
					{
						// Issue 4108
						// If we don't have the write allcoation profile, get the right one.
						if (gcw.HeaderRID != ap.HeaderRID)
							ap = (AllocationProfile)apl.FindKey(gcw.HeaderRID);
						// End issue 4108

						AllocationWorkFlowStep awfs = 
							new AllocationWorkFlowStep(
							this,
							gcw.GeneralComponent,  // Issue 4108
							false,
							true,
							aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
							aStoreFilter,
							-1);
						this.ProcessAction(
							aApplicationTransaction.SAB,
							aApplicationTransaction,
							awfs,
							ap,  // Issue 4108
							true,
							aStoreFilter);
					}

				}
				else
				{
					//==============================
					// headers
					//==============================
                    // begin TT#988 - MD - Jellis - Rule Method Gets Invalid Cast
                    GeneralComponent gc;
                    switch (ComponentType)
                    {
                        case (eComponentType.SpecificColor):
                            {
                                gc = (GeneralComponent)(new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, ColorCodeRID));
                                break;
                            }
                        case (eComponentType.SpecificPack):
                            {
                                //BEGIN TT#1624 - MD - DOConnell - Key Cannot be Null in Rule Method Specific Pack
                                Header hpn = new Header();
                                DataTable pndt = hpn.GetPack(PackRID);
                                PackName = pndt.Rows[0]["HDR_PACK_NAME"].ToString();
                                //END TT#1624 - MD - DOConnell - Key Cannot be Null in Rule Method Specific Pack
                                gc = (GeneralComponent)(new AllocationPackComponent(PackName));
                                break;
                            }
                        default:
                            {
                                gc = new GeneralComponent(ComponentType);
                                break;
                            }
                    }
                    // end TT#988 - MD - Jellis - Rule Method Gets Invalid Cast
					foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
					{
                        
						AllocationWorkFlowStep awfs = 
							new AllocationWorkFlowStep(
							this,
                            //new GeneralComponent(eGeneralComponentType.Total), // TT#488 - MD - Jellis - Group Allocation -- unrelated issue cannot apply bulk rules
                            //new GeneralComponent(this.ComponentType),            // TT#488 - MD - Jellis - Group Allocation -- unrelated issue cannot apply bulk rules // TT#988 - MD - Jellis - Rule Method Gets Invalid Cast
                            gc, // TT#988 - MD - Jellis - Rule Method Gets Invalid Cast
							false,
							true,
							aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
							aStoreFilter,
							-1);
						this.ProcessAction(
							aApplicationTransaction.SAB,
							aApplicationTransaction,
							awfs,
							ap,
							true,
							aStoreFilter);
					}
				}
			}
			catch (Exception err)
			{
				SAB.ApplicationServerSession.Audit.Log_Exception(err);
				// Begin Track #6286 - JSmith - Using Copy Prior - missing notification message
				throw err;
				// End Track #6286
			}
			// END Track #5727
		}

		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aApplicationWorkFlowStep, 
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
			Audit audit = aSAB.ApplicationServerSession.Audit;
            // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
            //AllocationProfile ap = (AllocationProfile) aAllocationProfile;
            AllocationProfile ap = aAllocationProfile as AllocationProfile;
            if (ap == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }
            // end TT#421 Detail packs/bulk not allocated by Size Need Method.
            // BEGIN MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
			if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
			{
				string msg = string.Format(
					audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction,false), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
				audit.Add_Msg(
					eMIDMessageLevel.Warning,eMIDTextCode.msg_HeaderStatusDisallowsAction,
					(this.Name + " " + ap.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
					this.GetType().Name);
				aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
			}
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            // End TT#1966-MD - JSmith- DC Fulfillment
			else
			{
				// END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
				//				_targetComponent = null;
				_basisComponent = null;
				// BEGIN MID Change j.ellis Add Audit Message
				string infoMsg;
				ApplicationServerSession session;
				session = SAB.ApplicationServerSession;
				// END MID Change j.ellis Add Audit Message
				_masterKeysSubKeys = new Hashtable();

				// BEGIN Issue 4632 - stodd 08.31.07
				if (aStoreFilterRID == Include.UndefinedStoreFilter
					|| aStoreFilterRID == Include.NoRID)
				{
					// No filter override
				}
				else
				{
					FilterData storeFilterData = new FilterData();
					this.FilterRID = aStoreFilterRID;
					string msg = audit.GetText(eMIDTextCode.msg_al_StoreFilterAppliedToMethod,false);
					msg = msg.Replace("{0}",storeFilterData.FilterGetName(aStoreFilterRID));
					msg = msg.Replace("{1}","Rule Method");
					msg = msg.Replace("{2}",this.Name);
					msg = msg.Replace("{3}",ap.HeaderID);
					audit.Add_Msg(
						eMIDMessageLevel.Information,eMIDTextCode.msg_al_StoreFilterAppliedToMethod,
						msg, this.GetType().Name);
				}
				// END Issue 4632 - stodd 08.31.07

				try 
				{
					// BEGIN MID Change j.ellis Add Audit Message
					if (_globalUserTypeText == null)
					{
						if (this.GlobalUserType == eGlobalUserType.Global)
						{
							_globalUserTypeText = 
								MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsGlobalRule);
						}
						else
						{
							_globalUserTypeText =
								MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsUserRule);
						}
					}
					infoMsg =
						string.Format(
						session.Audit.GetText(eMIDTextCode.msg_al_BeginProcessRule, false),
						_globalUserTypeText,
						this.Name, 
						ap.HeaderID);
					session.Audit.Add_Msg(
						eMIDMessageLevel.Information,
						eMIDTextCode.msg_al_BeginProcessRule,
						infoMsg,
						this.GetType().Name);
					// END MID Change j.ellis Add Audit Message
					//				if (!Enum.IsDefined(typeof(eAllocationMethodType),aAllocationWorkFlowStep._method.MethodType))
					//				{
					//					throw new MIDException(eErrorLevel.severe,
					//						(int)(eMIDTextCode.msg_WorkflowTypeInvalid),
					//						MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
					//				}
					if (this.MethodStatus == eMethodStatus.InvalidMethod)
					{
                        // begin MID Track 5778 Scheduler 'Run Now' feature gets error in audit
						string errorMessage = string.Empty;
						//try
						//{
						//	errorMessage = MIDText.GetText(eMIDTextCode.msg_MethodInvalid);
						//	errorMessage = errorMessage.Replace("{0}","Rule");
						//	errorMessage = errorMessage.Replace("{1}",this.Name);
						//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
						//}
						//catch (MIDException ex)
						//{
						//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
						//	string message = ex.ToString();
						//	throw;
						//}
						errorMessage = MIDText.GetText(eMIDTextCode.msg_MethodInvalid);
						errorMessage = errorMessage.Replace("{0}","Rule");
						errorMessage = errorMessage.Replace("{1}",this.Name);
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
						throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
						// end MID Track 5778 Scheduler 'Run Now' feature gets error in audit

					
					}

                    // begin TT#421 Detail packs/bulk not allocated by Size Need
					//if (eProfileType.Allocation != aAllocationProfile.ProfileType)
					//{
					//	//===========================================================================================================
					//	// WHY ARE WE THROWING AND CATCHING RIGHT AFTER?
					//	// The Exception is missing critical info until it's gets thrown.  Once thrown (and caught) we can then add
					//	// it to the Audit log.
					//	//===========================================================================================================
					//	try
					//	{
					//		throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_NotAllocationProfile),
					//			MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
					//	}
					//	catch (MIDException ex)
					//	{
					//		SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile), this.GetType().Name);
					//		string message = ex.ToString();
					//		throw;
					//	}
					//}
                    // end TT#421 Detail packs/bulk not allocated by Size Need

					AllocationWorkFlowStep aAllocationWorkFlowStep = (AllocationWorkFlowStep)aApplicationWorkFlowStep;
					GeneralComponent targetComponent = aAllocationWorkFlowStep.Component;

					//===================================================
					// If this Rule Method contains a prior HeaderRID, 
					// lets get the allocation profile for it.
					//===================================================

					AllocationProfileList basisApList = null;
					_basisAllocProfile = null;
					if (this.HeaderRID != Include.NoRID)
					{
                        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
                        ApplicationSessionTransaction ruleTransaction = SAB.ApplicationServerSession.CreateTransaction();
                        //ApplicationSessionTransaction ruleTransaction = new ApplicationSessionTransaction(this.SAB);
                        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

						// Look in current transaction for prior Header
						_basisAllocProfile = aApplicationTransaction.GetAllocationProfile(this.HeaderRID);
						// if not in current transaction, build a transaction and add it to it.
						if (_basisAllocProfile == null)
						{
							int[] headerList = {this.HeaderRID};
							basisApList = new AllocationProfileList(eProfileType.Allocation);
							basisApList.LoadHeaders(ruleTransaction, null, headerList, aApplicationTransaction.SAB.ApplicationServerSession); // TT#488 - MD - Jellis - Group Allocation					
							_basisAllocProfile = (AllocationProfile)basisApList[0];
						}

						// Begin ISSUE # 3975 - stodd
						if (_basisAllocProfile.Key == ap.Key)
						{
							aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                            string errorMessage = // MID Track 5778 Scheduler 'Run Now' feature gets error in audit   
                                 MIDText.GetText(eMIDTextCode.msg_al_PriorAndTargetHeaderEqual) + " Header [" + ap.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit   
							SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
							throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_PriorAndTargetHeaderEqual),
								errorMessage); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit 
						}
						// End ISSUE # 3975 - stodd

						ruleTransaction.AddAllocationProfile(_basisAllocProfile);

						// set pack name
						if (this.PackRID != Include.NoRID)
						{
							string [] packNames = _basisAllocProfile.GetPackNames();
							foreach (string packName in packNames)
							{
								PackHdr packHeader = _basisAllocProfile.GetPackHdr(packName);
								if (packHeader.PackRID == this.PackRID)
								{
									this.PackName = packHeader.PackName;
									break;
								}
							}
						}
					}

					//======================================================================================================
					// before beginning MatchingPacks logic, be sure if target is MatchingPacks that we have a Basis Header
					//======================================================================================================
					if (targetComponent.ComponentType == eComponentType.MatchingPack && _basisAllocProfile == null)
					{
						// begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
						//try
						//{
						//	// missing basis header - cannot process request
						//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_BasisHeaderNotDeclared),
						//		MIDText.GetText(eMIDTextCode.msg_al_BasisHeaderNotDeclared));
						//}
						//catch (MIDException ex)
						//{
						//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_BasisHeaderNotDeclared), this.GetType().Name);
						//	string message = ex.ToString();
						//	throw;
						//}
						string message =
							MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
							+ " ["
							+ this.Name
						    + "] Target Header ["
						    + ap.HeaderID
						    + "] "
                            + MIDText.GetText(eMIDTextCode.msg_al_BasisHeaderNotDeclared);
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Severe,
							eMIDTextCode.msg_al_BasisHeaderNotDeclared,
							message,
							this.GetType().Name);
						throw new MIDException(
							eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_BasisHeaderNotDeclared,
							message);
						// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
					}

					//====================================================================
					// Is either the Target or the Basis component = MatchingPacks?
					//====================================================================
					if (targetComponent.ComponentType == eComponentType.MatchingPack || this.ComponentType == eComponentType.MatchingPack)
					{
						ProcessMatchingPacks(ap, targetComponent, aSAB, aApplicationTransaction);
					}
						//====================================================================
						// Is either the Target or the Basis component = MatchingCOlorss?
						//====================================================================
					else if (targetComponent.ComponentType == eComponentType.MatchingColor || this.ComponentType == eComponentType.MatchingColor)
					{
						ProcessMatchingColors(ap, targetComponent, aSAB, aApplicationTransaction);
					}
						//====================================================================
						// process other types of components
						//====================================================================
					else 
					{
						//=============================================
						// create Basis Component if one is designated
						//=============================================
						if (_basisAllocProfile != null)
						{
							_basisComponent = BuildComponentForBasisHeader(targetComponent, this.ComponentType, this.ColorCodeRID);
						}

						switch (targetComponent.ComponentType)
						{
							case (eComponentType.Total):
							case (eComponentType.Bulk):
							case (eComponentType.DetailType):
							case (eComponentType.SpecificColor):
							case (eComponentType.SpecificPack):
								//case (eComponentType.SpecificSize):
								//case (eComponentType.ColorAndSize):				
							case (eComponentType.GenericType):
							{
								ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, targetComponent);
								break;
							}
							case (eComponentType.AllPacks):
							{
								string [] packNames = ap.GetPackNames();
								foreach (string packName in packNames)
								{
									AllocationPackComponent aPack = new AllocationPackComponent(packName);
									//GeneralComponent aSingleComponent = aPack;
									ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aPack);
								}
								break;
							}
							case (eComponentType.AllGenericPacks):
							{
								string [] packNames = ap.GetPackNames();
								foreach (string packName in packNames)
								{
									PackHdr packHeader = ap.GetPackHdr(packName);
									if (packHeader.GenericPack)
									{
										AllocationPackComponent aPack = new AllocationPackComponent(packName);
										ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aPack);
									}
								}						
								break;
							}
							case (eComponentType.AllNonGenericPacks):
							{
								string [] packNames = ap.GetPackNames();
								foreach (string packName in packNames)
								{
									PackHdr packHeader = ap.GetPackHdr(packName);
									if (!packHeader.GenericPack)
									{
										AllocationPackComponent aPack = new AllocationPackComponent(packName);
										ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aPack);
									}
								}								
								break;
							}
							case (eComponentType.AllSizes):
							{
								// not valid
								break;
							}
							case (eComponentType.AllColors):
							{
								Hashtable colors = ap.BulkColors;
								foreach (HdrColorBin hcb in colors.Values)
								{
									AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
									ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aColor);
								}
								break;
							}
							default:
							{
                                // begin MID TRack 5778 Schedule 'Run Now' feature gets error in audit
								//try
								//{
								//	// unsupported target component
								//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_UnsupportedTargetComponent),
								//		MIDText.GetText(eMIDTextCode.msg_al_UnsupportedTargetComponent));
								//}
								//catch (MIDException ex)
								//{
								//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_UnsupportedTargetComponent), this.GetType().Name);
								//	string message = ex.ToString();
								//	throw;
								//}
								string message =
									MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
									+ " ["
									+ this.Name
									+ "] Target Header ["
									+ ap.HeaderID
									+ "] "
									+ MIDText.GetText(eMIDTextCode.msg_al_UnsupportedTargetComponent);
								SAB.ApplicationServerSession.Audit.Add_Msg(
									eMIDMessageLevel.Severe,
									eMIDTextCode.msg_al_UnsupportedTargetComponent,
									message,
									this.GetType().Name);
								throw new MIDException(
									eErrorLevel.severe,
									(int)eMIDTextCode.msg_al_UnsupportedTargetComponent,
									message);
							}
						}
					}

					//   BEG MID Track #3219: PO / Master Release Enhancement
					if (this.IsHeaderMaster)
					{
						bool subordStatus = false;
					
						//string [] targetPackNames = ap.GetPackNames();
						int singleColorRID = Include.NoRID;

						switch (this.ComponentType)
						{
							case eComponentType.Total:
							case eComponentType.MatchingPack:
							{
								switch (targetComponent.ComponentType)
								{		
									case eComponentType.SpecificPack:
									case eComponentType.Total:
									{
										int subPackRID;
										foreach (int mastPackRID in _masterKeysSubKeys.Keys)
										{
											subPackRID = (int)_masterKeysSubKeys[mastPackRID];
											subordStatus = UpdateMasterHeader(ap.HeaderRID, _basisAllocProfile.HeaderRID, (int)this.ComponentType, mastPackRID, this.ColorCodeRID, (int)targetComponent.ComponentType, subPackRID, singleColorRID);
										}
										if (subordStatus)
										{
											aApplicationTransaction.SetAllocationActionStatus(ap.HeaderRID, eAllocationActionStatus.ActionCompletedSuccessfully);
										}
										break;
									}

									case eComponentType.SpecificColor:
									{
										//AllocationColorOrSizeComponent acsc = (AllocationColorOrSizeComponent)targetComponent;
										//targetColorRID[0] = acsc.ColorRID;
										break;
									}

									default:
									{
										break;
									}
								}
								break;
							}
							case eComponentType.SpecificPack:
							{
								switch (targetComponent.ComponentType)
								{		
									case eComponentType.SpecificPack:
									case eComponentType.Total:
									{
										foreach (int key in _masterKeysSubKeys.Keys)
										{
											subordStatus = UpdateMasterHeader(ap.HeaderRID, _basisAllocProfile.HeaderRID, (int)this.ComponentType, this.PackRID, this.ColorCodeRID, (int)this.ComponentType, key, singleColorRID);
										}
										if (subordStatus)
										{
											aApplicationTransaction.SetAllocationActionStatus(ap.HeaderRID, eAllocationActionStatus.ActionCompletedSuccessfully);
										}
										break;
									}

									case eComponentType.SpecificColor:
									{
										//AllocationColorOrSizeComponent acsc = (AllocationColorOrSizeComponent)targetComponent;
										//targetColorRID[0] = acsc.ColorRID;
										break;
									}

									default:
									{
										break;
									}
								}
								break;
							}
						}
					}
					// - END MID Track #3219

				}
				catch (Exception error)
				{
					aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
					string message = error.ToString();
					throw;
				}
				finally
				{
					// BEGIN MID Change j.ellis Add Audit Message
					infoMsg =
						string.Format(
						session.Audit.GetText(eMIDTextCode.msg_al_EndProcessRule, false),
						_globalUserTypeText,
						this.Name, 
						ap.HeaderID);
					session.Audit.Add_Msg(
						eMIDMessageLevel.Information,
						eMIDTextCode.msg_al_EndProcessRule,
						infoMsg,
						this.GetType().Name);
					// END MID Change j.ellis Add Audit Message
				}
			}
		}
		private bool UpdateMasterHeader(int aSubordRID, int masterRID,int aMasterComponent, int aMasterPackRID, int aMasterColorRID,
			int aSubordComponent, int aSubordPackRID, int aSubordColorRID)
		{
			bool subordStatus = false;
		
			Header header = new Header();
            //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -duplicate function
			//int subordRID = header.GetSubordinateRID(this.HeaderRID);
            int subordRID = header.GetSubordForMaster(this.HeaderRID);
            //End TT#846-MD -jsobek -New Stored Procedures for Performance -duplicate function
			header.OpenUpdateConnection();
			
			if (subordRID == Include.NoRID)
			{
				try
				{
					subordStatus = header.CreateSubordMaster(aSubordRID, masterRID, aMasterComponent, aMasterPackRID, aMasterColorRID, aSubordComponent, aSubordPackRID, aSubordColorRID);
					header.CommitData();
				}

				catch (Exception)
				{
					throw;
				}
				finally
				{
					header.CloseUpdateConnection();
				}
			}
			else
			{
				try
				{
					bool rowFound = false;
					DataTable dt = header.GetComponentsForSubord(aSubordRID);
					foreach (DataRow dr in dt.Rows)
					{
						if ((int)dr["MASTER_PACK_RID"] == aMasterPackRID &&
							(int)dr["SUBORD_PACK_RID"] == aSubordPackRID )
						{
							rowFound = true;
							subordStatus = header.UpdateSubordMaster(aSubordRID, masterRID, aMasterComponent, aMasterPackRID, aMasterColorRID, aSubordComponent, aSubordPackRID, aSubordColorRID);
						}
					}
					if (!rowFound)
						subordStatus = header.CreateSubordMaster(aSubordRID, masterRID, aMasterComponent, aMasterPackRID, aMasterColorRID, aSubordComponent, aSubordPackRID, aSubordColorRID);
					header.CommitData();	
				}

				catch (Exception)
				{
					throw;
				}
				finally
				{
					header.CloseUpdateConnection();
				}
			}
			return subordStatus;
		}	

		/// <summary>
		/// handles the processing flow for matching packs
		/// </summary>
		/// <param name="ap"></param>
		/// <param name="targetComponent"></param>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		private void ProcessMatchingPacks(AllocationProfile ap, 
			GeneralComponent targetComponent,
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction)
		{

			if ((targetComponent.ComponentType == eComponentType.MatchingPack && this.ComponentType == eComponentType.MatchingPack)
				|| (targetComponent.ComponentType == eComponentType.Total && this.ComponentType == eComponentType.MatchingPack)
				|| (targetComponent.ComponentType == eComponentType.AllNonGenericPacks && this.ComponentType == eComponentType.MatchingPack)
				|| (targetComponent.ComponentType == eComponentType.AllGenericPacks && this.ComponentType == eComponentType.MatchingPack)
				|| (targetComponent.ComponentType == eComponentType.AllPacks && this.ComponentType == eComponentType.MatchingPack))

			{
				bool foundPack = false;
				string [] packNames = ap.GetPackNames();
				foreach (string packName in packNames)
				{
					PackHdr targetPackHeader = ap.GetPackHdr(packName);
					if ((targetPackHeader.GenericPack && targetComponent.ComponentType == eComponentType.AllGenericPacks)
						|| ((!targetPackHeader.GenericPack) && targetComponent.ComponentType == eComponentType.AllNonGenericPacks)
						|| (targetComponent.ComponentType != eComponentType.AllNonGenericPacks &&
						targetComponent.ComponentType != eComponentType.AllGenericPacks))
					{

						PackHdr basisPackHeader = null;
						try
						{
							basisPackHeader = _basisAllocProfile.GetPackHdr(packName);
						}
						// we have to swallow the exception throw when pack isn't found.
						catch (Exception)
						{

						}
						
						if (basisPackHeader != null)
						{
							foundPack = true;
							AllocationPackComponent aPackComponent = new AllocationPackComponent(packName);
							_basisComponent = BuildComponentForBasisHeader(aPackComponent, eComponentType.MatchingPack, this.ColorCodeRID);
							ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)ap, aPackComponent);
						}
					}
				}
				if (!foundPack)
				{
					// begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingPacks),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] Target Header ["
						+ ap.HeaderID
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingPacks,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_NoMatchingPacks,
						message);
					// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
				}
			} 
			//=========================
			// TARGET is Specific Pack
			//=========================
			else if (targetComponent.ComponentType == eComponentType.SpecificPack)
			{
				AllocationPackComponent aPackComponent = (AllocationPackComponent)targetComponent;
				PackHdr packHeader = _basisAllocProfile.GetPackHdr(aPackComponent.PackName);
				if (packHeader != null)
				{
					_basisComponent = BuildComponentForBasisHeader(aPackComponent, eComponentType.MatchingPack, this.ColorCodeRID);
					ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)ap, aPackComponent);
				}
				else
				{
					// begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingPacks),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] Target Header ["
						+ ap.HeaderID
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingPacks,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_NoMatchingPacks,
						message);
					// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
				}
			}
			//=========================
			// BASIS is Specific Pack
			//=========================
			else if (this.ComponentType == eComponentType.SpecificPack)
			{
				_basisComponent = BuildComponentForBasisHeader(null, eComponentType.SpecificPack, this.ColorCodeRID);						
				PackHdr packHeader = ap.GetPackHdr(this.PackName);
				if (packHeader != null)
				{
					AllocationPackComponent aPackComponent = new AllocationPackComponent(this.PackName);
					ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)ap, aPackComponent);
				}
				else
				{
					// begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingPacks),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] Target Header ["
						+ ap.HeaderID
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingPacks,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_NoMatchingPacks,
						message);
					// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
				}
			}
			else
			{
                // begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
				//try
				//{
				//	// incompatable components between basis and target
				//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched),
				//		MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched));
				//}
				//catch (MIDException ex)
				//{
				//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched), this.GetType().Name);
				//	string message = ex.ToString();
				//	throw;
				//}
				string message =
					MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
					+ " ["
					+ this.Name
					+ "] Target Header ["
					+ ap.HeaderID
					+ "] "
					+ MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched);
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Severe,
					eMIDTextCode.msg_al_RuleAllocationComponentsMismatched,
					message,
					this.GetType().Name);
				throw new MIDException(
					eErrorLevel.severe,
					(int)eMIDTextCode.msg_al_RuleAllocationComponentsMismatched,
					message);
				// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
			}
		}

		/// <summary>
		/// handles the processing flow for match colors
		/// </summary>
		/// <param name="ap"></param>
		/// <param name="targetComponent"></param>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		private void ProcessMatchingColors(AllocationProfile ap, 
			GeneralComponent targetComponent,
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction)
		{
			if ((targetComponent.ComponentType == eComponentType.MatchingColor && this.ComponentType == eComponentType.MatchingColor)
				|| (targetComponent.ComponentType == eComponentType.Total && this.ComponentType == eComponentType.MatchingColor)
				|| (targetComponent.ComponentType == eComponentType.AllColors && this.ComponentType == eComponentType.MatchingColor))

			{		
				bool foundColor = false;   
				Hashtable colors = ap.BulkColors;
				foreach (HdrColorBin hcb in colors.Values)
				{
					HdrColorBin aColor = null;
					try
					{
						aColor = _basisAllocProfile.GetHdrColorBin(hcb.ColorCodeRID);
					}
					// we must swallow the exception thrown when a color isn't found
					catch (Exception)
					{
					}

					if (aColor != null)
					{
						foundColor = true;
						AllocationColorOrSizeComponent aColorComp = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
						_basisComponent = BuildComponentForBasisHeader(aColorComp, eComponentType.SpecificColor, aColorComp.ColorRID);
						ProcessComponent(aSAB, aApplicationTransaction, ap, aColorComp);
					}
				}
				if (!foundColor)
				{
					// begin MID Track 5778 Schedule 'Run Now' gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingColors),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] Target Header ["
						+ ap.HeaderID
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingColors,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_NoMatchingColors,
						message);
					// end MID Track 5778 Schedule 'Run Now' gets error in audit
				}
			}
			else if (targetComponent.ComponentType == eComponentType.SpecificColor)
			{
				AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)targetComponent;
				if (this._basisAllocProfile.BulkColorIsOnHeader(colorComponent.ColorRID))
				{
					_basisComponent = BuildComponentForBasisHeader(colorComponent, eComponentType.MatchingColor, colorComponent.ColorRID);
					ProcessComponent(aSAB, aApplicationTransaction,  ap, colorComponent);	
				}
				else
				{
					// begin MID Track 5778 Schedule 'Run Now' gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingColors),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] Target Header ["
						+ ap.HeaderID
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingColors,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_NoMatchingColors,
						message);
					// end MID Track 5778 Schedule 'Run Now' gets error in audit
				}

			} 
			else if (this.ComponentType == eComponentType.SpecificColor)
			{
				if (ap.BulkColorIsOnHeader(this.ColorCodeRID))
				{
					_basisComponent = BuildComponentForBasisHeader(null, eComponentType.SpecificColor, this.ColorCodeRID);
					ProcessComponent(aSAB, aApplicationTransaction,  ap, _basisComponent);	
				}
			}									   
			else
			{
				// begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
				//try
				//{
				//	// incompatable components between basis and target
				//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched),
				//		MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched));
				//}
				//catch (MIDException ex)
				//{
				//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched), this.GetType().Name);
				//	string message = ex.ToString();
				//	throw;
				//}
				string message =
					MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
					+ " ["
					+ this.Name
					+ "] Target Header ["
					+ ap.HeaderID
					+ "] "
					+ MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched);
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Severe,
					eMIDTextCode.msg_al_RuleAllocationComponentsMismatched,
					message,
					this.GetType().Name);
				throw new MIDException(
					eErrorLevel.severe,
					(int)eMIDTextCode.msg_al_RuleAllocationComponentsMismatched,
					message);
				// end MID Track 5778 Schedule 'Run Now' feature gets error in audit
			}
		}

		private void ProcessComponent(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction,  
			AllocationProfile aAllocationProfile,
			GeneralComponent aComponent)
		{              
            bool isSuccessful = true; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
			try 
			{
				// begin MID Track 4442 Ad Min, Min and Max Rules only valid for Total Component
				if (aComponent.ComponentType != eComponentType.Total)
				{
					string message;
					bool totalComponentRule = false;
					if (Enum.IsDefined(typeof(eRuleMethodOnlyValidOnTotalComponent),(eRuleMethodOnlyValidOnTotalComponent)this.IncludeRuleMethod))
					{
						totalComponentRule = true;
						message = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal),aAllocationProfile.HeaderID, this.Name, MIDText.GetTextOnly((int)this.IncludeRuleMethod));
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal,message,this.GetType().Name,false);
					}
					if (Enum.IsDefined(typeof(eRuleMethodOnlyValidOnTotalComponent),(eRuleMethodOnlyValidOnTotalComponent)this.ExcludeRuleMethod))
					{
						totalComponentRule = true;
						message = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal),aAllocationProfile.HeaderID, this.Name, MIDText.GetTextOnly((int)this.ExcludeRuleMethod));
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal,message,this.GetType().Name,false);
					}
					if (totalComponentRule)
					{
                        isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_FoundInvalidRuleForNonTotalComponent,
							MIDText.GetTextOnly(eMIDTextCode.msg_al_FoundInvalidRuleForNonTotalComponent));
					}
				}
				// end MID Track 4442 Ad Min, Min and Max Rules only valid for Total Component
				// BEGIN MID Track #3219
				if (this.IsHeaderMaster)
				{
					string errorMessage; 
					int masterMultiple, subordMultiple;
					 
					Header header = new Header();
					
					switch (this.ComponentType)
					{
						case (eComponentType.SpecificPack):
						{
							DataTable dtPack = header.GetPack(PackRID);
							DataRow dr = dtPack.Rows[0]; 
							masterMultiple = (int)dr["MULTIPLE"];
							int key;
							switch (aComponent.ComponentType)
							{
								case eComponentType.SpecificPack:
								{
									AllocationPackComponent apc = (AllocationPackComponent)aComponent;
									subordMultiple = aAllocationProfile.GetPackMultiple(apc.PackName);
									if (masterMultiple > subordMultiple)
									{
	                                    isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
										errorMessage = MIDText.GetText(eMIDTextCode.msg_al_PackMultipleGreaterThanSubordinate);   // Issue 4061
										throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
									else if (SqlInt32.Mod(subordMultiple,masterMultiple) > 0) // Issue 4029 Master rewrite
									{
                                        isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
										errorMessage = MIDText.GetText(eMIDTextCode.msg_al_PackMultipleMustDivideSubordinateEvenly);   // Issue 4061
										throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
									else
										key = aAllocationProfile.GetPackRID(apc.PackName);
									_masterKeysSubKeys.Add(key,key);
									break;
								}
							
								case eComponentType.Total:
								{	
									bool multipleFound = false;  
									foreach(PackHdr ph in aAllocationProfile.Packs.Values)
									{
										if (SqlInt32.Mod(ph.PackMultiple,masterMultiple) == 0) // Issue 4061
										{
											_masterKeysSubKeys.Add(ph.PackRID,ph.PackRID);
											multipleFound = true;
										}	
									}	
									if (!multipleFound)
									{
										errorMessage = MIDText.GetText(eMIDTextCode.msg_al_PackMultipleMustDivideSubordinateEvenly);  // Issue 4061
										isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
                                        throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
								
									break;
								}
							}
							break;
						}
						case eComponentType.Total:
						case eComponentType.MatchingPack:
						{
							if (_basisAllocProfile.PackCount == 0)
							{
                                isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
								errorMessage = 	string.Format
									(MIDText.GetText(eMIDTextCode.msg_al_NoPacksOnHeader),
									MIDText.GetTextOnly(eMIDTextCode.lbl_Master));
								throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
							}
							else if (aAllocationProfile.PackCount == 0)
							{
                                isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
								errorMessage = 	string.Format
									(MIDText.GetText(eMIDTextCode.msg_al_NoPacksOnHeader),
									MIDText.GetTextOnly(eMIDTextCode.lbl_Subordinate));
								throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
							}
							else if (aAllocationProfile.PackCount > _basisAllocProfile.PackCount 
								&& this.ComponentType == eComponentType.Total 
								&& aComponent.ComponentType == eComponentType.Total)
							{
                                isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
								errorMessage = 	string.Format
									(MIDText.GetText(eMIDTextCode.msg_al_NumberOfPacksExceeded),
									MIDText.GetTextOnly(eMIDTextCode.lbl_Subordinate),MIDText.GetTextOnly(eMIDTextCode.lbl_Master));
								throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
							}
							switch (aComponent.ComponentType)
							{
								case eComponentType.SpecificPack:
								{
									AllocationPackComponent apc = (AllocationPackComponent)aComponent;
									subordMultiple = aAllocationProfile.GetPackMultiple(apc.PackName);
									bool multipleFound = false;
									foreach(PackHdr ph in _basisAllocProfile.Packs.Values)
									{
										if (SqlInt32.Mod(ph.PackMultiple,subordMultiple) == 0)
										{
											if ((this.ComponentType == eComponentType.Total) ||
												(this.ComponentType == eComponentType.MatchingPack 
												&& apc.PackName == ph.PackName) )	
											{
												_masterKeysSubKeys.Add(ph.PackRID,aAllocationProfile.GetPackRID(apc.PackName));
												multipleFound = true;
												break;
											}				
										}	
									}	
									if (!multipleFound)
									{
                                        isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
										errorMessage = MIDText.GetText(eMIDTextCode.msg_al_NoMatchingPacks);
										throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
									break;
								}
								case eComponentType.Total:
								{	
									_masterPacks = header.GetPacks(_basisAllocProfile.Key);
									_masterPacks.PrimaryKey = new DataColumn[] {_masterPacks.Columns["HDR_PACK_RID"]};
									_masterView = new DataView(_masterPacks);
									_masterView.Sort = "MULTIPLE ASC";

									_compPacks = header.GetPacks(aAllocationProfile.Key);
									_compPacks.PrimaryKey = new DataColumn[] {_compPacks.Columns["HDR_PACK_RID"]};
									_compView = new DataView(_compPacks);
									_compView.Sort = "MULTIPLE DESC";
																	
									_masterRemoveList = new ArrayList(); 
									_subRemoveList = new ArrayList(); 
					
									int origSubPackTotal = _compPacks.Rows.Count;
									MatchSubordinateToMasterPacks(true);
							
									if (_masterPacks.Rows.Count == 0 && _compPacks.Rows.Count > 0)
									{
                                        isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
										errorMessage = 	string.Format
											(MIDText.GetText(eMIDTextCode.msg_al_NotAllPacksMatch),
											MIDText.GetTextOnly(eMIDTextCode.lbl_Subordinate),MIDText.GetTextOnly(eMIDTextCode.lbl_Master));
										throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
									else if (_masterPacks.Rows.Count > 0 && _compPacks.Rows.Count > 0)
									{
										_masterRemoveList.Clear();
										_subRemoveList.Clear();
										MatchSubordinateToMasterPacks(false);
									}
 
									if (   (_compPacks.Rows.Count > 0 && this.ComponentType == eComponentType.Total) 
										|| (_compPacks.Rows.Count == origSubPackTotal) )
									{
                                        isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
										errorMessage = 	string.Format
											(MIDText.GetText(eMIDTextCode.msg_al_NotAllPacksMatch),
											MIDText.GetTextOnly(eMIDTextCode.lbl_Subordinate),MIDText.GetTextOnly(eMIDTextCode.lbl_Master));
										throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_MethodInvalid),errorMessage);
									}
									break;
								}
							}
							break;
						}
					}
				}
				// END MID Track #3219

				//*******************************************
				// Get List of Stores to Process
				//*******************************************
				// begin MID Track 3430 Proportional Allocation not correct when units allocated to reserve
				//ProfileList storeList = aApplicationTransaction.GetMasterProfileList(eProfileType.Store);
				int reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 3430 Proportional Allocation wrong when reserve is allocated
				ProfileList storeList = (ProfileList)(aApplicationTransaction.GetMasterProfileList(eProfileType.Store)).Clone();
				if (storeList.Contains(reserveStoreRid) && !_includeReserve ) //TT#1608-MD - Prior Header - Include Reserve - SRisch 6/02/2015
				{
					storeList.Remove((Profile)storeList.FindKey(reserveStoreRid));
				}
				// end MID Track 3430 Proportional Allocation not correct when units allocated to reserve
				ProfileList storeFilterList, storeGroupList;
				ProfileList storesOnHeaderList = new ProfileList(eProfileType.Store);
				//*******************************
				// Apply STORE FILTER if present
				//*******************************
				bool outdatedFilter = false;
				if (this.FilterRID != Include.NoRID)
				{
					storeFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(aAllocationProfile, FilterRID, ref outdatedFilter);
				}
				else
				{
					storeFilterList = storeList;
				}

				if (outdatedFilter)
				{
					FilterData storeFilterData = new FilterData();
					string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
					msg = msg.Replace("{0}",storeFilterData.FilterGetName(this.FilterRID));
					string suffix = ". Method " + this.Name + ". Header [" + aAllocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit
					string auditMsg = msg + suffix;
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
                    isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
					throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
				}

				//================================
				// Apply store SET if present
				//================================
				if (this.SG_RID != Include.NoRID)
				{
					if (this.StoreGroupLevelRID == Include.AllStoreFilterRID
						|| this.StoreGroupLevelRID == Include.NoRID)
					{
						storeGroupList = storeList;
					}
					else
					{
						storeGroupList = aApplicationTransaction.GetActiveStoresInGroup(this.SG_RID, this.StoreGroupLevelRID); //MID Track 5820 - Unhandled Exception After Store Activation
					}
				}
					// otherwise we do ALL the stores
				else
				{
					storeGroupList = storeList;
				}

				//*******************************************
				// apply PRIOR HEADER Attr Set to Store list
				//*******************************************
				if (_basisAllocProfile != null)
				{
                    // begin MID Track 5759 Give Warning when Basis Header has zero units allocated
                    if (_basisAllocProfile.TotalUnitsAllocated == 0)
                    {
                        string warningMessage = string.Format(
                            SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_RuleMethodPriorHeaderHasZeroAllocation, false),
                            this.Name,
                            _basisAllocProfile.HeaderID,
                            aAllocationProfile.HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Warning, 
                            warningMessage, 
                            this.GetType().Name);
                    }
                    // end MID Track 5759 Give Warning when Basis Header has zero units allocated

					foreach (StoreProfile sp in storeList)
					{
						int qty = _basisAllocProfile.GetStoreQtyAllocated(_basisComponent,sp.Key);
						if (qty > 0)
							storesOnHeaderList.Add(sp);
					}
				}
				else
				{
					storesOnHeaderList = storeList;
				}


				//int reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 3430 Proportional Allocation wrong when reserve is allocated
				//***************************************************************
				// Merge all store lists, taking the intersection of the lists
				//***************************************************************
                // begin TT#575 - MD - Assortment - Jellis - Redesign Rule Method so that Assortment can easily call to get similar results
                //ArrayList includeStoreList = new ArrayList();
                //ArrayList excludeStoreList = new ArrayList();
                List<Index_RID> includeStoreList = new List<Index_RID>();
                List<Index_RID> excludeStoreList = new List<Index_RID>();
                // end TT#575 - MD - Assorment - Jellis - Redesign Rule Method so that Assortment can easily call to get similar results
				foreach (Index_RID storeIdxRID in aApplicationTransaction.StoreIndexRIDArray())
				{
					//=================================================
					// The reserve store should not be in EITHER list.
					//=================================================
                    if (storeIdxRID.RID == reserveStoreRid && !_includeReserve // TT#1401 - JEllis - Urban Reservation Store pt 11   //TT#1608-MD - Prior Header - Include Reserve -SRisch  06/02/2015
                        || !aAllocationProfile.GetIncludeStoreInAllocation(storeIdxRID)) // TT#1401 - JEllis - Urban Reservation Store pt 11
						continue;

					if (storeList.Contains(storeIdxRID.RID) &&
						storeFilterList.Contains(storeIdxRID.RID) &&
						storeGroupList.Contains(storeIdxRID.RID) &&
						storesOnHeaderList.Contains(storeIdxRID.RID))
					{
						includeStoreList.Add(storeIdxRID);
					}
					else
					{
						excludeStoreList.Add(storeIdxRID);
					}
				}

				//********************
				// Generate Rules
				//********************

				_ruleAllocProfile = new RuleAllocationProfile(this.Key, aApplicationTransaction, aAllocationProfile.HeaderRID, aComponent);

                // begin TT#575 - MD - Assortment - Jellis - redesign Rule Method so that assortment can easily call to get similar results
                //ApplyRuleToStores(includeStoreList, this.IncludeRuleMethod, this.IncludeQuantity, aAllocationProfile); 
                //ApplyRuleToStores(excludeStoreList, this.ExcludeRuleMethod, this.ExcludeQuantity, aAllocationProfile); 
                RuleBasedAllocation rba =
                    aApplicationTransaction.RuleBasedAllocation;        // TT#586 - MD - Jellis - Assortment 2 dimensional Spread
                    //new RuleBasedAllocation(aApplicationTransaction); // TT#586 - MD - Jellis - Assortment 2 dimensional Spread
                MIDException statusMsg;
                isSuccessful = rba.CalculateRuleAllocation(
                    includeStoreList,
                    _includeRuleMethod,
                    (int)_includeQuantity,
                    _basisAllocProfile,
                    _basisComponent,
                    aAllocationProfile,
                    aComponent,
                    _sortDirection,
                    ref _ruleAllocProfile,
                    out statusMsg);
                if (!isSuccessful)
                {
                    HandleStatusMessage (statusMsg);
                }
                else
                {
                    isSuccessful = rba.CalculateRuleAllocation(
                        excludeStoreList,
                        _excludeRuleMethod,     // TT#488 - MD - Jellis - Group Allocation -- unrelated error  (exclude rule not working)
                        (int)_excludeQuantity,  // TT#488 - MD - Jellis - Group Allocation--  unrelated error  (exclude rule not working)
                        _basisAllocProfile,
                        _basisComponent,
                        aAllocationProfile,
                        aComponent,
                        _sortDirection,
                        ref _ruleAllocProfile,
                        out statusMsg);
                    if (!isSuccessful)
                    {
                        HandleStatusMessage(statusMsg);
                    }
                }
		
                if (isSuccessful)
                {
                    // end TT#575 - MD - Jellis - Assortment - redesign Rule Method so that assorment can easily call to get similar results
				    _ruleAllocProfile.WriteRuleAllocation();
				
				    int auditQtyAllocatedByProcess = aAllocationProfile.GetQtyAllocated(aComponent); // MID Track 4448 AnF Audit Report Enhancement
                    //bool isSuccessful = aAllocationProfile.DetermineChosenRule(aComponent, _ruleAllocProfile.LayerID);  // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
                    isSuccessful = aAllocationProfile.DetermineChosenRule(aComponent, _ruleAllocProfile.LayerID, false, _includeReserve);  // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results //TT#1608-MD - Prior Header - Include Reserve -SRisch  06/02/2015
				    if (isSuccessful)
				    {                            // MID Track 4448 AnF Audit Enhancement
					    aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					    // begin MID Track 4448 AnF Audit Enhancement
					    string packOrColorComponentName = null;
					    switch (aComponent.ComponentType)
					    {
						    case (eComponentType.Total):
						    case (eComponentType.GenericType):
						    case (eComponentType.DetailType):
						    case (eComponentType.Bulk):
						    {
							    break;
						    }
						    case (eComponentType.SpecificColor):
						    {
							    packOrColorComponentName = aApplicationTransaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)aComponent).ColorRID).ColorCodeName;
							    break;
						    }
						    case (eComponentType.SpecificPack):
						    {
                                packOrColorComponentName = ((AllocationPackComponent)aComponent).PackName;
							    break;
						    }
						    default:
						    {
							    break;
						    }
					    }
					    aApplicationTransaction.WriteAllocationAuditInfo
						    (aAllocationProfile.Key,
						    0,
						    this.MethodType,
						    this.Key,
						    this.Name,
						    aComponent.ComponentType,
						    packOrColorComponentName,
						    null,                                                                        // MID Track 4448 AnF Audit Report Enhancement
						    aAllocationProfile.GetQtyAllocated(aComponent) - auditQtyAllocatedByProcess, // MID Track 4448 AnF Audit Report Enhancement
						    aAllocationProfile.GetCountStoresWithRuleLayer(aComponent, _ruleAllocProfile.LayerID)); // MID Track 4448 AnF Audit Report Enhancement
					    // end MID Track 4448 AnF Audit Enhancement	
				    }                            // MID Track 4448 AnF Audit Enhancement
				    else
				    {                            // MID Track 4448 AnF Audit Enhancement
					    aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
				    }                            // MID Track 4448 AnF Audit Enhancment
                }  // TT#575 - MD - Jellis - Assortment - redesign Rule Method so that assortment can easily call to get similar results.
			}
			catch (Exception error)
			{
                isSuccessful = false; // TT#575 - MD - Jellis - Assortment redesign Rule Method so that assortment can easily call getting simililar results
				aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
				string message = error.ToString();
				throw;
			}
			finally
			{
                // begin TT#575 - MD - Jellis - Assortment redesign Rule Method so assortment can easily call to get similar results
                if (isSuccessful)
                {
                    aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                }
                else
                {
                    aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
                }
                // end TT#575 - MD - Jellis - Assortment redesign Rule Method so assortment can easily call to get similar results
			}
		}
        // begin TT#575 - MD - Jellis - Assortment redesign Rule Method so assortment can easily call to get similar results
        private void HandleStatusMessage(MIDException aStatusMessage)
        {
            Include.TranslateErrorLevel(aStatusMessage.ErrorLevel);
            if (aStatusMessage.ErrorLevel == eErrorLevel.fatal
                || aStatusMessage.ErrorLevel == eErrorLevel.severe)
            {
                throw aStatusMessage;
            }
            SAB.ApplicationServerSession.Audit.Add_Msg(Include.TranslateErrorLevel(aStatusMessage.ErrorLevel), aStatusMessage.ErrorMessage, this.GetType().Name);
        }
        // end TT#575 - MD - Jellis - Assortment  redesign Rule Method so assortment can easily call to get similar results

		private void MatchSubordinateToMasterPacks(bool checkForEqual)
		{
			try
			{
				foreach(DataRowView drvComp in _compView)
				{
					int subMultiple = Convert.ToInt32(drvComp["MULTIPLE"],CultureInfo.CurrentUICulture);
					int subPackRID = Convert.ToInt32(drvComp["HDR_PACK_RID"],CultureInfo.CurrentUICulture);
					foreach (DataRowView drvMaster in _masterView)
					{
						int mastMultiple = Convert.ToInt32(drvMaster["MULTIPLE"],CultureInfo.CurrentUICulture);
						int mastPackRID = Convert.ToInt32(drvMaster["HDR_PACK_RID"],CultureInfo.CurrentUICulture);
						if (checkForEqual)  
						{
							if (subMultiple == mastMultiple)
							{
								if (this.ComponentType == eComponentType.Total)  
								{
									if (!_masterKeysSubKeys.ContainsKey(mastPackRID))
									{
										_masterKeysSubKeys.Add(mastPackRID,subPackRID);
										_masterRemoveList.Add(mastPackRID);
										_subRemoveList.Add(subPackRID);
										break;
									}
								}
								else
								{
									string cName = Convert.ToString(drvComp["HDR_PACK_NAME"],CultureInfo.CurrentUICulture);
									string pName = Convert.ToString(drvMaster["HDR_PACK_NAME"],CultureInfo.CurrentUICulture);
									if (pName == cName)
									{
										if (!_masterKeysSubKeys.ContainsKey(mastPackRID))
										{
											_masterKeysSubKeys.Add(mastPackRID,subPackRID);
											_masterRemoveList.Add(mastPackRID);
											_subRemoveList.Add(subPackRID);
											break;
										}
									}
								}
							}
						}
						else if (SqlInt32.Mod(subMultiple,mastMultiple) == 0)  // Issue 4029 Master rewrite
						{
							if (this.ComponentType == eComponentType.Total)  
							{
								if (!_masterKeysSubKeys.ContainsKey(mastPackRID))
								{
									_masterKeysSubKeys.Add(mastPackRID,subPackRID);
									_masterRemoveList.Add(mastPackRID);
									_subRemoveList.Add(subPackRID);
									break;
								}
							}
							else
							{
								string cName = Convert.ToString(drvComp["HDR_PACK_NAME"],CultureInfo.CurrentUICulture);
								string pName = Convert.ToString(drvMaster["HDR_PACK_NAME"],CultureInfo.CurrentUICulture);
								if (pName == cName)
								{
									if (!_masterKeysSubKeys.ContainsKey(mastPackRID))
									{
										_masterKeysSubKeys.Add(mastPackRID,subPackRID);
										_masterRemoveList.Add(mastPackRID);
										_subRemoveList.Add(subPackRID);
										break;
									}
								}
							}
						}
					} 
				}
	
				foreach( int key in _masterRemoveList)
				{
					DataRow delRow = _masterPacks.Rows.Find(key);
					_masterPacks.Rows.Remove(delRow);
				}	
									
				foreach( int key in _subRemoveList)
				{
					DataRow delRow = _compPacks.Rows.Find(key);
					_compPacks.Rows.Remove(delRow);
				}	
			}
			catch  
			{
				throw;
			}
		}

        // begin TT#575 - MD - Jellis - Assortment redesign rule method so assortment can easily call to get similar results
        //       Moved this code to RuleBasedAllocation.cs
//        /// <summary>
//        /// Using the RuleMethod, applies the rule to the stores in the storeList sent.
//        /// </summary>
//        /// <param name="storeList">ArrayList of Index_RIDs identifying the stores getting the rule</param>
//        /// <param name="aRuleMethod">Rule Method Type</param>
//        /// <param name="aRuleQuantity">Rule Quantity</param>
//        /// <param name="allocHeader">Header Allocation Profile where the rule will be applied</param>
//        private void ApplyRuleToStores(ArrayList storeList,
//            eRuleMethod aRuleMethod, 
//            double aRuleQuantity, 
//            AllocationProfile allocHeader)
//        {
//            eRuleType aRuleType = eRuleType.None;
//            int qty = 0, priorQty = 0;
//            try
//            {

//                switch (aRuleMethod)
//                {
//                    case (eRuleMethod.Exact):
//                    {
//                        aRuleType = eRuleType.Exact;

//                        AllocationWorkMultiple priorHeaderWorkMultiple = GetQtyPerPack(_basisComponent, _basisAllocProfile);
//                        AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(_ruleAllocProfile.Component, allocHeader);

//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            priorQty = _basisAllocProfile.GetStoreQtyAllocated(_basisComponent,storeIdxRID);
//                            if (_basisComponent.ComponentType == eComponentType.SpecificPack)
//                                priorQty = priorQty * priorHeaderWorkMultiple.Multiple;

//                            int remainder = priorQty % currHeaderWorkMultiple.Multiple;
//                            if (remainder > 0)
//                            {
//                                throw new MIDException(eErrorLevel.severe,
//                                    (int)(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule),
//                                    MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule));
//                            }
//                            else
//                            {
//                                if (_ruleAllocProfile.Component.ComponentType == eComponentType.SpecificPack)
//                                    qty = priorQty / currHeaderWorkMultiple.Multiple;
//                                else
//                                    qty = priorQty;
//                            }

//                            // begin MID Track 4388 Exact not working (should not be constrained by grade max, only capacity)
//                            //int max = allocHeader.GetStoreMaximum(_ruleAllocProfile.Component,storeIdxRID);
//                            int max = allocHeader.GetStoreCapacityMaximum(storeIdxRID);
//                            // end MID Track 4388 Exact not working (should not be constrained by grade max, only capacity)
//                            if (qty > max)  // Test for greater than MAX
//                            {
//                                // TODO 
//                                // put some kind of message to the log
//                                // IF (and that’ a big “if”) we put any message out, it should go to the audit log;  
//                                // in that case, there should only be one message that basically says exact 
//                                // rejected because of MAX constraint (if we identify the stores, 
//                                // they should be in a list within this one message).
//                            }
//                            else
//                                _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, qty);
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.Fill):
//                    {
//                        aRuleType = eRuleType.Fill;

//                        AllocationWorkMultiple priorHeaderWorkMultiple = GetQtyPerPack(_basisComponent, _basisAllocProfile);
//                        AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(_ruleAllocProfile.Component, allocHeader);

//                        //**************
//                        // Sort stores
//                        //**************
//                        ArrayList summandList = new ArrayList();
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            Summand aSummand = new Summand();
//                            aSummand.Eligible = allocHeader.GetStoreIsEligible(storeIdxRID);
//                            aSummand.Item = storeIdxRID.RID;
//                            aSummand.ItemIdx = storeIdxRID.Index;

//                            priorQty = _basisAllocProfile.GetStoreQtyAllocated(_basisComponent,storeIdxRID);
//                            if (_basisComponent.ComponentType == eComponentType.SpecificPack)
//                                priorQty = priorQty * priorHeaderWorkMultiple.Multiple;

//                            int remainder = priorQty % currHeaderWorkMultiple.Multiple;
//                            if (remainder > 0)
//                            {
//                                throw new MIDException(eErrorLevel.severe,
//                                    (int)(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule),
//                                    MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule));
//                            }
//                            else
//                            {
//                                if (_ruleAllocProfile.Component.ComponentType == eComponentType.SpecificPack)
//                                    qty = priorQty / currHeaderWorkMultiple.Multiple;
//                                else
//                                    qty = priorQty;
//                            }

//                            // begin MID track 4092 Fill Rule not observing Maximum
//                            //int max = allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component,storeIdxRID);
//                            // begin MID track 4388 Exact not working (should not be constrained by grade max, only capacity)
//                            //           Also, Fill should be same
//                            //int max = allocHeader.GetStoreMaximum(_ruleAllocProfile.Component,storeIdxRID);
//                            int max = allocHeader.GetStoreCapacityMaximum(storeIdxRID);
//                            // end MID Track 4388 Exact not working (should not be constrained by grade max, only capacity)
//                            // end MID track 4092 Fill Rule not observing Maximum
//                            if (qty > max)
//                                aSummand.Quantity = (double)max;
//                            else
//                                aSummand.Quantity = (double)qty;
//                            summandList.Add(aSummand);
//                        }
//                        if (this.SortDirection == eSortDirection.Descending)
//                            summandList.Sort(new SummandDescendingComparer());
//                        else
//                            summandList.Sort(new SummandAscendingComparer());

//                        //*****************************
//                        // Get total units to allocate
//                        //*****************************
////						int totalUnits = allocHeader.GetAllocatedBalance(_ruleAllocProfile.Component);
//                        int totalUnits = allocHeader.GetRuleUnitsToAllocate(_ruleAllocProfile.Component);
//                        int remainingUnits = totalUnits;

//                        //************************
//                        // Allocate to each store
//                        //************************
//                        foreach (Summand aSummand  in summandList)
//                        {
//                            if (aSummand.Quantity < remainingUnits)
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), aRuleType, (int)aSummand.Quantity);
//                                remainingUnits -= (int)aSummand.Quantity;
//                            }
//                            else
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), aRuleType, remainingUnits);
//                                remainingUnits = 0;
//                            }
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.None):
//                    {
//                        //					aRuleType = eRuleType.None;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        //					{
//                        //						_ruleAllocProfile.SetStoreRuleAllocation(sp.Key, aRuleType, 0);
//                        //					}
//                        break;	
//                    }
//                    case (eRuleMethod.Out):
//                    {
//                        aRuleType = eRuleType.Exclude;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.Proportional):
//                    {	
//                        aRuleType = eRuleType.ProportionalAllocated;
//                        // AllocationWorkMultiple priorHeaderWorkMultiple = GetQtyPerPack(_basisComponent, _basisAllocProfile);
//                        AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(_ruleAllocProfile.Component, allocHeader);

//                        ArrayList summandList = new ArrayList();
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            Summand aSummand = new Summand();
//                            aSummand.Eligible = allocHeader.GetStoreIsEligible(storeIdxRID);
//                            aSummand.Item = storeIdxRID.RID;
//                            aSummand.ItemIdx = storeIdxRID.Index;
//                            qty = _basisAllocProfile.GetStoreQtyAllocated(_basisComponent,storeIdxRID);
//                            aSummand.Quantity = (double)qty;
//                            // begin MID Track 5738 Proportional Rule Allocation Enforces Minimum Allocation
//                            //aSummand.Min = currHeaderWorkMultiple.Minimum;
//                            aSummand.Min = 0;
//                            aSummand.Max = allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component,storeIdxRID); 
//                             // end MID Track 5738 Proportional Rule Allocation Enforces Minimum Allocation

//                            summandList.Add(aSummand);
//                        }

//                        ProportionalSpread aSpread = new ProportionalSpread(this.SAB);
//                        aSpread.SummandList = summandList;
////						aSpread.RequestedTotal = allocHeader.GetAllocatedBalance(_ruleAllocProfile.Component);
//                        aSpread.RequestedTotal = allocHeader.GetRuleUnitsToAllocate(_ruleAllocProfile.Component);
//                        aSpread.Precision = 0;

//                        // If the current componenet is a pack, then the Requested Total is already in packs (not units)
//                        // so the multiple becomes 1.
//                        if (_ruleAllocProfile.Component.ComponentType != eComponentType.SpecificPack)
//                            aSpread.Multiple = currHeaderWorkMultiple.Multiple;
//                        aSpread.Calculate();
					
//                        foreach (Summand aSummand  in summandList)
//                        {
//                            _ruleAllocProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), aRuleType, (int)aSummand.Result);
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.AdMinimum):
//                    {
//                        aRuleType = eRuleType.AdMinimum;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            //int min = allocHeader.GetGradeAdMinimum(allocHeader.GetStoreGradeIdx(storeIdxRID));  // TT#1287 - JEllis - Inventory Minimum Maximum
//                            int min = allocHeader.GetGradeAdMinimum(allocHeader.GetStoreGradeIdx(storeIdxRID));    // TT#1287 - JEllis - Invnetory Minimum Maximum
//                            if (allocHeader.GradeInventoryMinimumMaximum)                                          // TT#1287 - JEllis - Inventory Minimum Maximum
//                            {                                                                                      // TT#1287 - JEllis - Inventory Minimum Maximum
//                                min = Math.Max(0, min - allocHeader.GetStoreInventoryBasis(allocHeader.GradeInventoryBasisHnRID, storeIdxRID)); // TT#1287 - JEllis -Inventory Minimum Maximum
//                            }                                                                                      // TT#1287 - JEllis - Inventory Minimum Maximum
//                            if (min <= allocHeader.GetStoreCapacityMaximum(storeIdxRID)
//                                && min <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID))
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                            }
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.ColorMaximum):
//                    {
//                        aRuleType = eRuleType.ColorMaximum;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        // begin MID Track 3578 invalid cast
//                        //foreach (Index_RID storeIdxRID in storeList)
//                        //{
//                        //	int max = allocHeader.GetStoreColorMaximum(((AllocationColorOrSizeComponent)_ruleAllocProfile.Component).ColorRID, storeIdxRID);
//                        //	if (max <= allocHeader.GetStoreCapacityMaximum(storeIdxRID) 
//                        //		&& max <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID))
//                        //	{
//                        //		_ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                        //	}
//                        //}
//                        int max;
//                        GeneralComponent colorComponent;
//                        switch (_ruleAllocProfile.Component.ComponentType)
//                        {
//                            case (eComponentType.ColorAndSize):
//                            {
//                                colorComponent = ((AllocationColorSizeComponent)_ruleAllocProfile.Component).ColorComponent;
//                                break;
//                            }
//                            case (eComponentType.SpecificColor):
//                            {
//                                colorComponent = (AllocationColorOrSizeComponent)_ruleAllocProfile.Component;
//                                break;
//                            }
//                            default:
//                            {
//                                colorComponent = null;
//                                break;
//                            }
//                        }
//                        if (colorComponent != null
//                            && colorComponent.ComponentType == eComponentType.SpecificColor)
//                        {
//                            foreach (Index_RID storeIdxRID in storeList)
//                            {
//                                max = allocHeader.GetStoreColorMaximum(((AllocationColorOrSizeComponent)colorComponent).ColorRID, storeIdxRID); 
//                                if (max <= allocHeader.GetStoreCapacityMaximum(storeIdxRID) 
//                                    && max <= allocHeader.GetStorePrimaryMaximum(colorComponent, storeIdxRID))
//                                {
//                                    _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                                }

//                            }
//                        }
//                        else
//                        {
//                            string colorMaximum = MIDText.GetTextOnly((int)eRuleType.ColorMaximum);
//                            string ruleComponent = MIDText.GetTextOnly((int)_ruleAllocProfile.Component.ComponentType);
//                            string specificColor = MIDText.GetTextOnly((int)eComponentType.SpecificColor);
//                            string message = string.Format
//                                (SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent, false),
//                                allocHeader.HeaderID,
//                                this.Name,
//                                colorMaximum,
//                                ruleComponent,
//                                specificColor);
//                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
//                            throw new MIDException(eErrorLevel.warning,(int)(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent),
//                                message);
//                        }
//                        // end MID Track 3578 invalid cast
//                        break;
//                    }
//                    case (eRuleMethod.ColorMinimum):
//                    {
//                        aRuleType = eRuleType.ColorMinimum;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        // begin MID Track 3578 invalid cast
//                        //foreach (Index_RID storeIdxRID in storeList)
//                        //{
//                        //	int min = allocHeader.GetStoreColorMinimum(((AllocationColorOrSizeComponent)_ruleAllocProfile.Component).ColorRID, storeIdxRID);
//                        //	if (min <= allocHeader.GetStoreCapacityMaximum(storeIdxRID)
//                        //		&& min <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID ))
//                        //	{
//                        //		_ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                        //	}
//                        //}
//                        int min;
//                        GeneralComponent colorComponent;
//                        switch (_ruleAllocProfile.Component.ComponentType)
//                        {
//                            case (eComponentType.ColorAndSize):
//                            {
//                                colorComponent = ((AllocationColorSizeComponent)_ruleAllocProfile.Component).ColorComponent;
//                                break;
//                            }
//                            case (eComponentType.SpecificColor):
//                            {
//                                colorComponent = (AllocationColorOrSizeComponent)_ruleAllocProfile.Component;
//                                break;
//                            }
//                            default:
//                            {
//                                colorComponent = null;
//                                break;
//                            }
//                        }
//                        if (colorComponent != null
//                            && colorComponent.ComponentType == eComponentType.SpecificColor)
//                        {
//                            foreach (Index_RID storeIdxRID in storeList)
//                            {
//                                min = allocHeader.GetStoreColorMinimum(((AllocationColorOrSizeComponent)colorComponent).ColorRID, storeIdxRID); 
//                                if (min <= allocHeader.GetStoreCapacityMaximum(storeIdxRID) 
//                                    && min <= allocHeader.GetStorePrimaryMaximum(colorComponent, storeIdxRID))
//                                {
//                                    _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                                }

//                            }
//                        }
//                        else
//                        {
//                            string colorMinimum = MIDText.GetTextOnly((int)eRuleType.ColorMinimum);
//                            string ruleComponent = MIDText.GetTextOnly((int)_ruleAllocProfile.Component.ComponentType);
//                            string specificColor = MIDText.GetTextOnly((int)eComponentType.SpecificColor);
//                            string message = string.Format
//                                (SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent, false),
//                                allocHeader.HeaderID,
//                                this.Name,
//                                colorMinimum,
//                                ruleComponent,
//                                specificColor);
//                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
//                            throw new MIDException(eErrorLevel.warning,(int)(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent),
//                                message);
//                        }

//                        // end MID Track 3578 invalid cast
//                        break;
//                    }
//                    case (eRuleMethod.Quantity):
//                    {
//                        aRuleType = eRuleType.AbsoluteQuantity;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            qty = (int)aRuleQuantity;
//                            if (qty <= allocHeader.GetStoreCapacityMaximum(storeIdxRID)
//                                && qty <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID))
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, (int)aRuleQuantity);
//                            }
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.StockMaximum):  // same as Allocation Max
//                    {
//                        aRuleType = eRuleType.Maximum;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            //int max = allocHeader.GetGradeMaximum(allocHeader.GetStoreGradeIdx(storeIdxRID));  // TT#1287 - JEllis - Inventory Minimum Maximum
//                            int max = allocHeader.GetStoreMaximum(_ruleAllocProfile.Component, storeIdxRID);    // TT#1287 - JEllis - Inventory Minimum Maximum
//                            if (max <= allocHeader.GetStoreCapacityMaximum(storeIdxRID)
//                                && max <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID))
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                            }
//                        }
//                        break;
//                    }
//                    case (eRuleMethod.StockMinimum):  // same as Allocation Min
//                    {
//                        aRuleType = eRuleType.Minimum;
//                        //					foreach (StoreProfile sp in storeList.ArrayList)
//                        foreach (Index_RID storeIdxRID in storeList)
//                        {
//                            //int min = allocHeader.GetGradeMinimum(allocHeader.GetStoreGradeIdx(storeIdxRID));  // TT#1287 - JEllis - Inventory Minimum Maximum
//                            int min = allocHeader.GetStoreMinimum(_ruleAllocProfile.Component, storeIdxRID);    // TT#1287 - JEllis - Inventory Minimum Maximum
//                            if (min <= allocHeader.GetStoreCapacityMaximum(storeIdxRID)
//                                && min <= allocHeader.GetStorePrimaryMaximum(_ruleAllocProfile.Component, storeIdxRID))
//                            {
//                                _ruleAllocProfile.SetStoreRuleAllocation(storeIdxRID, aRuleType, 0);
//                            }
//                        }
//                        break;

//                    }
//                    default:
//                    {
//                        string errorMessage = 	string.Format
//                            (MIDText.GetText(eMIDTextCode.msg_InvalidRule),
//                            aRuleMethod.ToString());

//                        //					SAB.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.severe,
//                        //							(int)(eMIDTextCode.msg_InvalidRule),
//                        //							errorMessage));

//                        // begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
//                        //try
//                        //{
//                        //	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_InvalidRule),
//                        //		errorMessage);
//                        //}
//                        //catch (MIDException ex)
//                        //{
//                        //	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
//                        //	string message = ex.ToString();
//                        //	throw;
//                        //}
//                        errorMessage =
//                            MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
//                            + " ["
//                            + this.Name
//                            + "] Header ["
//                            + allocHeader.HeaderID
//                            + "] "
//                            + errorMessage;
//                        SAB.ApplicationServerSession.Audit.Add_Msg(
//                            eMIDMessageLevel.Severe,
//                            eMIDTextCode.msg_InvalidRule,
//                            errorMessage,
//                            this.GetType().Name);
//                        throw new MIDException(
//                            eErrorLevel.severe,
//                            (int)eMIDTextCode.msg_InvalidRule,
//                            errorMessage);
//                        // end MID Track 5778 Schedule 'Run Now' feature gets error in audit
//                    }
//                }


//                //			// Check store MINs and MAXs.  (Proportional already has this figured in)
//                //			if (aRuleMethod != eRuleMethod.Proportional)
//                //			{
//                //				foreach (StoreProfile sp in storeList.ArrayList)
//                //				{
//                //					_ruleAllocProfile.SetStoreRuleAllocation(sp.Key, aRuleType, 0);
//                //				}	
//                //
//                //				int min = allocHeader.GetStoreMinimum(_ruleAllocProfile.Component,sp.Key);				 //no min
//                //				int max = allocHeader.GetStoreMaximum(_ruleAllocProfile.Component,sp.Key);  //no max
//                //			}
//            }
//            catch (Exception ex)
//            {
//                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.GetType().Name);
//                string message = ex.ToString();
//                throw;
//            }
//        }

//        private AllocationWorkMultiple GetQtyPerPack(GeneralComponent aComponent, AllocationProfile aAllocProfile)
//        {
//            AllocationWorkMultiple awm = new AllocationWorkMultiple(1,1);

//            if (aComponent.ComponentType == eComponentType.SpecificPack)
//            {
//                AllocationPackComponent apc = (AllocationPackComponent)aComponent;
//                // Begin Issue - 4108 stodd 
//                try
//                {
//                    if (aAllocProfile.PackIsOnHeader(apc.PackName))
//                    {
//                        awm.Multiple = aAllocProfile.GetPackMultiple(apc.PackName);
//                        awm.Minimum = awm.Multiple;
//                    }
//                }
//                catch
//                {

//                }
//                // end issue 4108
//            }
//            else if (aComponent.ComponentType == eComponentType.Total 
//                || aComponent.ComponentType == eComponentType.Bulk
//                || aComponent.ComponentType == eComponentType.DetailType)
//            {
//                switch (aComponent.ComponentType)
//                {
//                    case (eComponentType.Total):
//                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Total);
//                        break;
//                    case (eComponentType.Bulk):
//                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Bulk);
//                        break;
//                    case (eComponentType.DetailType):
//                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.DetailType);
//                        break;
//                }
//            }
			
//            return awm;
//        }
        // end TT#575 - MD - Assortment - Jellis - redesign Rule Method so assortment can call easily to get similar results.


		private int ConvertPackMultible(int priorPackMult, int priorQty, int packMult, bool exactMatch)
		{
			int convertedPriorQty = 0;
			int convertedQty = 0;

			// NO packs involved...or packs multibles are 1's
			if (priorPackMult == 1 && packMult == 1)
			{
				convertedQty = priorQty;
			}
			else
			{
				convertedPriorQty = priorPackMult * priorQty;
				int remainder = convertedPriorQty % packMult;
				if (exactMatch && remainder > 0)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule),
						MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule));
				}
				convertedQty = convertedPriorQty / packMult;
			}

			return convertedQty;
		}


		private GeneralComponent BuildComponentForBasisHeader(GeneralComponent targetComponent, eComponentType componentType, int colorRid)
		{
			GeneralComponent basisComponent = null;

			switch (componentType)
			{
				case (eComponentType.Bulk):
				{
					basisComponent = new GeneralComponent(eComponentType.Bulk);
					break;
				}
				case (eComponentType.SpecificColor):
				{
					AllocationColorOrSizeComponent color = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRid);
					basisComponent = color;
					break;
				}
				case (eComponentType.SpecificPack):
				{
					AllocationPackComponent pack = new AllocationPackComponent(this.PackName);
					basisComponent = pack;
					break;
				}
				case (eComponentType.Total):
				{
					basisComponent = new GeneralComponent(eComponentType.Total);

					break;
				}
				case (eComponentType.MatchingColor):
				{
					basisComponent = targetComponent;
					break;
				}
				case (eComponentType.MatchingPack):
				{
					basisComponent = targetComponent;
					break;
				}
				default:
				{
					//  unknown component
                    // begin MID Track 5778 Scheduler 'Run Now' feature gets error in audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_InvalidBasisComponent),
					//		MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent));
					//}
					//catch (MIDException ex)
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent), this.GetType().Name);
					//	string message = ex.ToString();
					//	throw;
					//}
					string message =
						MIDText.GetTextOnly(eMIDTextCode.frm_RuleMethod)
						+ " ["
						+ this.Name
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent);
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_InvalidBasisComponent,
						message,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_InvalidBasisComponent,
						message);
					// end MID Track 5778 Scheduler 'Run Now' feature gets error in audit
				}
			}

			return basisComponent;
		}

        //Begin TT#1237-MD -jsobek -Size Rule Error -Code is not dead, it is an override

		override public void Update(TransactionData td)
		{
//			bool create = false;
			if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodData = new RuleMethodData(td);
//				create = true;
			}
			 _methodData.Store_Filter_RID = _filterRID; 
			 _methodData.HDR_RID = _headerRID;       
			 _methodData.Store_Order = _sortDirection;
			 _methodData.Header_Component = _componentType;
			 _methodData.Header_Pack_RID = _packRID;
			 _methodData.Color_Code_RID = _colorCodeRID;
             // Assortment BEGIN 
             _methodData.Hdr_BC_RID = _hdrBCRID;
             // Assortment END 
             _methodData.Include_Reserve_Ind = Include.ConvertBoolToChar(_includeReserve); //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
			 _methodData.Included_Stores = _includeRuleMethod;
			 _methodData.Included_Quantity = _includeQuantity;
			 _methodData.Excluded_Stores = _excludeRuleMethod;
			 _methodData.Excluded_Quantity = _excludeQuantity;
			 _methodData.SGL_RID = _storeGroupLevelRID;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
			_methodData.Is_Header_Master = Include.ConvertBoolToChar(_isHeaderMaster);
// (CSMITH) - END MID Track #3219
			// methods created through here are always valid
			 _methodData.Method_Status = eMethodStatus.ValidMethod;
			try
			{
				
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_methodData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_methodData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_methodData.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				//TO DO:  whatever has to be done after an update or exception.
			}
		}
		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			RuleMethod newRuleMethod = null;

			try
			{
				newRuleMethod = (RuleMethod)this.MemberwiseClone();
				newRuleMethod.ColorCodeRID = ColorCodeRID;
				newRuleMethod.ComponentType = ComponentType;
				newRuleMethod.ExcludeQuantity = ExcludeQuantity;
				newRuleMethod.ExcludeRuleMethod = ExcludeRuleMethod;
				newRuleMethod.FilterRID = FilterRID;
				newRuleMethod.HeaderID = HeaderID;
				newRuleMethod.HeaderRID = HeaderRID;
				newRuleMethod.IncludeQuantity = IncludeQuantity;
                newRuleMethod.IncludeReserve = IncludeReserve; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
				newRuleMethod.IncludeRuleMethod = IncludeRuleMethod;
				newRuleMethod.IsHeaderMaster = IsHeaderMaster;
				newRuleMethod.Method_Change_Type = eChangeType.none;
				newRuleMethod.Method_Description = Method_Description;
				newRuleMethod.MethodStatus = MethodStatus;
				newRuleMethod.Name = Name;
				newRuleMethod.PackName = PackName;
				newRuleMethod.PackRID = PackRID;
				newRuleMethod.SG_RID = SG_RID;
				newRuleMethod.SortDirection = SortDirection;
				newRuleMethod.StoreGroupLevelRID = StoreGroupLevelRID;
				newRuleMethod.User_RID = User_RID;
				newRuleMethod.Virtual_IND = Virtual_IND;

				return newRuleMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
		{
			return true;
		}
        // End MID Track 4858

        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalRule);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserRule);
            }

        }
        // BEGIN RO-2845 Data transport for Rule Method - get data - MGage
        override public ROMethodProperties MethodGetData(bool processingApply)
        {
            ROMethodRuleProperties method = new ROMethodRuleProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                filter: GetName.GetFilterName(FilterRID),
                header: GetName.GetHeader(HeaderRID, SAB: SAB),
                isHeaderMaster: IsHeaderMaster,
                sortDirection: SortDirection,
                includeReserveInd: IncludeReserve,
                componentType: ComponentType,
                pack: GetName.GetHeaderPack(HeaderRID, PackRID),
                color: GetName.GetColor(ColorCodeRID, SAB: SAB),
                includeRuleMethod: IncludeRuleMethod,
                includeQuantity: IncludeQuantity,
                excludeRuleMethod: ExcludeRuleMethod,
                excludeQuantity: ExcludeQuantity,
                hdr_BC: GetName.GetHeader(HdrBCRID, SAB: SAB),
                storeGroupLevel: GetName.GetAttributeSetName(StoreGroupLevelRID)
                );
            return method;
        }
        // END RO-2845 Data transport for Rule Method - get data  - MGage
        override public bool MethodSetData(ROMethodProperties methodProperties, bool processingApply)
        {
            ROMethodRuleProperties roMethodRuleAllocationProperties = (ROMethodRuleProperties)methodProperties;

            try
            {
                Method_Description = roMethodRuleAllocationProperties.Description;
                User_RID = roMethodRuleAllocationProperties.UserKey;
                _filterRID = roMethodRuleAllocationProperties.Filter.Key;
                _headerRID = roMethodRuleAllocationProperties.Header.Key;
                _isHeaderMaster = roMethodRuleAllocationProperties.IsHeaderMaster;
                _sortDirection = roMethodRuleAllocationProperties.SortDirection;
                _includeReserve = roMethodRuleAllocationProperties.IncludeReserveInd;
                _componentType = roMethodRuleAllocationProperties.ComponentType;
                _packRID = roMethodRuleAllocationProperties.Pack.Key;
                _colorCodeRID = roMethodRuleAllocationProperties.Color.Key;
                _includeRuleMethod = roMethodRuleAllocationProperties.IncludeRuleMethod;
                _includeQuantity = roMethodRuleAllocationProperties.IncludeQuantity;
                _excludeRuleMethod = roMethodRuleAllocationProperties.ExcludeRuleMethod;
                _excludeQuantity = roMethodRuleAllocationProperties.ExcludeQuantity;
                _hdrBCRID = roMethodRuleAllocationProperties.Hdr_BC.Key;
                _storeGroupLevelRID = roMethodRuleAllocationProperties.StoreGroupLevel.Key;

                return true;
            }
            catch
            {
                return false;
            }
            //throw new NotImplementedException("MethodSaveData is not implemented");
            
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
	
