using System;
using System.Data;
using System.Collections;
using System.Collections.Generic; 
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
	public class BasisSizeAllocationMethod : AllocationSizeBaseMethod 
	{
		private MethodBasisSizeData _methodData;
		private int _BasisHdrRid;
		private int _BasisClrRid;
		private int _Rule;
		private int _quantity = 0;
		private int _sizeConstraintRid;
		private int _StoreFilterRid;
		private int _HeaderComponent;
		private SessionAddressBlock _aSAB;
		private bool _promptAttChange;
		private bool _promptSzChange;
		private bool _processCancelled = false;
		private int _componentsProcessed;
        private bool _includeReserve; // TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
		private AllocationProfile			_basisAllocProfile;
		private GeneralComponent			_targetComponent;
		private GeneralComponent			_basisComponent;
		private eBasisSizeMethodRuleType					_ruleMethod;
		private eComponentType				_basisComponentType;
		private int _reserveStoreRid = Include.NoRID;
		private SizeNeedResults _sizeNeedResults;
		private eDistributeChange _distributeChange = eDistributeChange.ToNone;
		private ArrayList _substituteList;  // contains list of BasisSizeSubstitute 
		private Hashtable _sizeSubstituteHash;
//		private CollectionDecoder _rulesDecoder;
		private MIDTimer _timer = new MIDTimer();
//		private bool _genCurveError = false;	// A&F Generic Size Curve 
		private ArrayList _actionAuditList;     // MID track 4967 Size Functions nnot showing total quantity
		
		#region "Properties"
		/// <summary>
		/// Gets Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodBasisSizeAllocation;
			}
		}

		public int Rule
		{
			get	{return _Rule;}
			set	{_Rule = value;}
		}

		// Begin MID Issue 2514 stodd
		/// <summary>
		/// Gets or Sets the Rule Quantity
		/// </summary>
		public int RuleQuantity
		{
			get	{return _quantity;}
			set	{_quantity = value;}
		}
		// End MID Issue 2514 stodd

		public int StoreFilterRid
		{
			get	{return _StoreFilterRid;}
			set	{_StoreFilterRid = value;}
		}

		public int HeaderComponent
		{
			get {return _HeaderComponent;}
			set {_HeaderComponent = value;}
		}

		public int BasisHdrRid
		{
			get{ return _BasisHdrRid;}
			set{_BasisHdrRid = value;}
		}


		public int BasisClrRid
		{
			get{ return _BasisClrRid;}
			set{_BasisClrRid = value;}
		}


		public bool PromptAttributeChange
		{
			get {return _promptAttChange;}
			set {_promptAttChange = value;}
		}

        //BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
        public bool IncludeReserve
        {
            get { return _includeReserve; }
            set { _includeReserve = value; }
        }
        //END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 

		public bool PromptSizeChange
		{
			get {return _promptSzChange;}
			set {_promptSzChange = value;}
		}

		/// <summary>
		/// Gets or sets the Size Constraint RID.
		/// </summary>
		public int SizeConstraintRid
		{
			get {return _sizeConstraintRid;}
			set {_sizeConstraintRid = value;}
		}

		public ArrayList SubstituteList
		{
			get {return _substituteList;}
			set {_substituteList = value;}
		}

		#endregion

		#region "Constructors"
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//public BasisSizeAllocationMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.BasisSizeAllocation)
			public BasisSizeAllocationMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.BasisSizeAllocation, eProfileType.MethodBasisSizeAllocation)
			//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_aSAB = aSAB;

			if (base.MethodType != eMethodType.BasisSizeAllocation)
			{
				//Add new enum and change this code
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_NotBasisSizeMethod),
					MIDText.GetText(eMIDTextCode.msg_NotBasisSizeMethod));
			}
                //SMR
			if (base.Filled)
			{
				_methodData = new MethodBasisSizeData(base.Key,eChangeType.populate);
				BasisHdrRid = _methodData.BasisHdrRid;
				BasisClrRid = _methodData.BasisClrRid;
				Rule = _methodData.Rule;
				SG_RID = _methodData.SG_RID;
                IncludeReserve = _methodData.IncludeReserveInd; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 Need Updated
				StoreFilterRid = _methodData.StoreFilterRid;
				base.SizeGroupRid = _methodData.SizeGroupRid;
				HeaderComponent = _methodData.HeaderComponent;
				RuleQuantity = _methodData.RuleQuantity;
				this.SizeConstraintRid = _methodData.SizeConstraintRid;
				_substituteList = _methodData.SubstituteList;
				base.SizeCurveGroupRid = _methodData.SizeCurveGroupRid;
				base.GenCurveCharGroupRID = _methodData.GenCurveHcgRID;
				base.GenCurveHnRID = _methodData.GenCurveHnRID;
				base.GenCurvePhRID = _methodData.GenCurvePhRID;
				base.GenCurvePhlSequence = _methodData.GenCurvePhlSequence;
				base.GenCurveColorInd = _methodData.GenCurveColorInd;
				base.GenCurveMerchType = _methodData.GenCurveMerchType;
				// add Generic Size Constraint
				base.GenConstraintCharGroupRID = _methodData.GenConstraintHcgRID;
				base.GenConstraintHnRID = _methodData.GenConstraintHnRID;
				base.GenConstraintPhRID = _methodData.GenConstraintPhRID;
				base.GenConstraintPhlSequence = _methodData.GenConstraintPhlSequence;
				base.GenConstraintColorInd = _methodData.GenConstraintColorInd;
				base.GenConstraintMerchType = _methodData.GenConstraintMerchType;
                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.UseDefaultCurve = _methodData.UseDefaultCurve;
                // End TT#413
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                base.ApplyRulesOnly = _methodData.ApplyRulesOnly;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.GenCurveNsccdRID = _methodData.GenCurveNsccdRID;
                // End TT#413
			}
			else
			{
				_methodData = new MethodBasisSizeData();
				SG_RID = aSAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
				StoreFilterRid = Include.NoRID;
				base.SizeGroupRid = Include.NoRID;
				Rule = Include.NoRID;
				BasisHdrRid = Include.NoRID;
				BasisClrRid = Include.NoRID;
				HeaderComponent = Include.Undefined;
				RuleQuantity = Include.UndefinedQuantity;
                IncludeReserve = SAB.ApplicationServerSession.GlobalOptions.PriorHeaderIncludeReserve;  //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 Need Updated
				this.SizeConstraintRid = Include.Undefined;
				base.SizeCurveGroupRid = Include.NoRID;
				_substituteList = new ArrayList();
				base.GenCurveCharGroupRID = Include.NoRID;
				base.GenCurveHnRID = Include.NoRID;
				base.GenCurvePhRID = Include.NoRID;
				base.GenCurvePhlSequence = 0;
				base.GenCurveColorInd = false;
				base.GenCurveMerchType = eMerchandiseType.Undefined;
				// add Generic Size Constraint 
				base.GenConstraintCharGroupRID = Include.NoRID;
				base.GenConstraintHnRID = Include.NoRID;
				base.GenConstraintPhRID = Include.NoRID;
				base.GenConstraintPhlSequence = 0;
				base.GenConstraintColorInd = false;
				base.GenConstraintMerchType = eMerchandiseType.Undefined;
                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.UseDefaultCurve = false;
                // End TT#413
                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.GenCurveNsccdRID = Include.NoRID;
                // End TT#413
			}
				
			SMBD = _methodData;
			_promptAttChange = true;
			_promptSzChange = true;

		 
			if (SizeGroupRid != Include.NoRID)
			{
				base.GetSizesUsing = eGetSizes.SizeGroupRID;
				base.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
			}
			else
			{
				base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
				base.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
			}
		
			CreateConstraintData();
		}

		#endregion

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (CheckSizeMethodForUserData())
            {
                return true;
            }

            if (IsFilterUser(StoreFilterRid))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		public void UpdateSubstituteList(int sizeTypeRid, int sizeSubRid, eEquateOverrideSizeType sizeType)
		{
			bool foundSize = false;
			foreach (BasisSizeSubstitute aSizeSub in _substituteList)
			{
				if (sizeTypeRid == aSizeSub.SizeTypeRid && sizeType == aSizeSub.SizeType)
				{
					foundSize = true;
					aSizeSub.SubstituteRid = sizeSubRid;
				}
			}

			if (sizeSubRid != Include.NoRID && !foundSize)
			{
				BasisSizeSubstitute aSizeSub = new BasisSizeSubstitute(sizeTypeRid, sizeSubRid, sizeType);
				_substituteList.Add(aSizeSub);
			}
		}

		public int GetSubstitute(int sizeTypeRid, eEquateOverrideSizeType sizeType)
		{
			int sizeSubRid = Include.NoRID;
			foreach (BasisSizeSubstitute aSizeSub in this._substituteList)
			{
				if (sizeTypeRid == aSizeSub.SizeTypeRid && sizeType == aSizeSub.SizeType)
				{
					sizeSubRid = aSizeSub.SubstituteRid;
					break;
				}
			}

			return sizeSubRid;
		}


		public Hashtable BuildSubstituteHashTable(ProfileList allSizeList)
		{
			int sizeKey = Include.NoRID;
			int sizeSubKey = Include.NoRID;
			Hashtable substituteHash = new Hashtable();

			//===========================================
			// do Size (SizePrimary) only substitutes first
			//===========================================
			foreach( BasisSizeSubstitute aSubstitute in this.SubstituteList)
			{
				if (aSubstitute.SizeType == eEquateOverrideSizeType.Size)
				{
					foreach(SizeCodeProfile aSizeCode in allSizeList)
					{
						if (aSubstitute.SizeTypeRid == aSizeCode.SizeCodePrimaryRID)
						{
							sizeKey = aSizeCode.Key;
							foreach(SizeCodeProfile bSizeCode in allSizeList)
							{
								if (aSubstitute.SubstituteRid == bSizeCode.SizeCodePrimaryRID
									&& aSizeCode.SizeCodeSecondaryRID == bSizeCode.SizeCodeSecondaryRID)
								{
									sizeSubKey = bSizeCode.Key;
									substituteHash.Add(sizeKey, sizeSubKey);
									break;
								}
							}
						}
					}
				}
			}

			//===========================================
			// do Dimension (SizeSecondary) only substitutes 
			//===========================================
			foreach( BasisSizeSubstitute aSubstitute in this.SubstituteList)
			{
				if (aSubstitute.SizeType == eEquateOverrideSizeType.Dimensions)
				{
					foreach(SizeCodeProfile aSizeCode in allSizeList)
					{
						if (aSubstitute.SizeTypeRid == aSizeCode.SizeCodeSecondaryRID)
						{
							sizeKey = aSizeCode.Key;
							foreach(SizeCodeProfile bSizeCode in allSizeList)
							{
								if (aSubstitute.SubstituteRid == bSizeCode.SizeCodeSecondaryRID
									&& aSizeCode.SizeCodePrimaryRID == bSizeCode.SizeCodePrimaryRID)
								{
									if (substituteHash.ContainsKey(sizeKey))
									{
										sizeSubKey = (int)substituteHash[sizeKey];
										SizeCodeProfile tempSizeCode = (SizeCodeProfile)allSizeList.FindKey(sizeSubKey);
										foreach(SizeCodeProfile cSizeCode in allSizeList)
										{
											if (aSubstitute.SubstituteRid == cSizeCode.SizeCodeSecondaryRID
												&& tempSizeCode.SizeCodePrimaryRID == cSizeCode.SizeCodePrimaryRID)
											{
												sizeSubKey = cSizeCode.Key;
												substituteHash.Remove(sizeKey);
												substituteHash.Add(sizeKey, sizeSubKey);
											}
										}
									}
									else
									{
										sizeSubKey = bSizeCode.Key;
										substituteHash.Add(sizeKey, sizeSubKey);
									}
									break;
								}
							}
						}
					}
				}
			}
			//===================================================
			// Finally, do specific Size/Dimension substitutions 
			//===================================================
			foreach( BasisSizeSubstitute aSubstitute in this.SubstituteList)
			{
				if (aSubstitute.SizeType == eEquateOverrideSizeType.DimensionSize)
				{
					if (substituteHash.ContainsKey(aSubstitute.SizeTypeRid))
					{
						substituteHash.Remove(aSubstitute.SizeTypeRid);
						substituteHash.Add(aSubstitute.SizeTypeRid, aSubstitute.SubstituteRid);
					}
					else
					{
						substituteHash.Add(aSubstitute.SizeTypeRid, aSubstitute.SubstituteRid);
					}
				}
			}

//			// Debug
//			IDictionaryEnumerator myEnumerator = substituteHash.GetEnumerator();
//			Debug.WriteLine("HASH");	
//			while ( myEnumerator.MoveNext() )
//			{
//				int from = (int)myEnumerator.Key;
//				int to = (int)myEnumerator.Value;
//				Debug.WriteLine(from.ToString() + " " + to.ToString());
//			}

			return substituteHash;
		}


		private void DebugSizeGroup(SizeGroupProfile sizeGroup)
		{
			foreach(SizeCodeProfile aSizeCode in sizeGroup.SizeCodeList)
			{
				string aLine = aSizeCode.Key.ToString() + " " +
					aSizeCode.SizeCodePrimaryRID.ToString() + " " +
					aSizeCode.SizeCodePrimary + " / " +
					aSizeCode.SizeCodeSecondaryRID.ToString() + " " +
					aSizeCode.SizeCodeSecondary;
				Debug.WriteLine(aLine);

			}
		}

		#region "Methods"


		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			// BEGIN Track #5727 stodd
			try
			{
				_componentsProcessed = 0;
				aApplicationTransaction.ResetAllocationActionStatus();

				//_reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes when run from Workflow does not recognize Reserve Store.
			
				//				if (!base.OkToProcess(aApplicationTransaction))	// Generic Size Curve
				//				{
				//					return;
				//				} 
                // Begin Development TT#13 - JSmith - Basis Size method give Null reference error
                base.Trans = aApplicationTransaction;
                // End Development TT#13
				base.BuildRulesDecoder(this.SG_RID);

				ArrayList selectedComponentList = aApplicationTransaction.GetSelectedComponentList();

				foreach (AllocationProfile ap2 in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
				{
					try
					{
						if (!base.OkToProcess(aApplicationTransaction, ap2))	// Generic Size Curve
						{
							aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);
							continue;
						}
					}
					catch (MIDException ex)
					{
						// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						//SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ErrorMessage, this.GetType().Name);
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod) + " [" + this.Name + "] " + " Header [" + ap2.HeaderID + "] " + ex.ErrorMessage, 
							this.GetType().Name);
						// end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);
						continue;
					}
					catch (Exception ex)
					{
						// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						//SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ErrorMessage, this.GetType().Name);
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod) + " [" + this.Name + "] " + " Header [" + ap2.HeaderID + "] " + ex.Message, 
							this.GetType().Name);
						// end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);
						continue;
					}

					// Allocation Component (Pack, BulkColor...)
					if (selectedComponentList.Count > 0)
					{
						AllocationProfileList apl = (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation);
						//					AllocationProfile ap = (AllocationProfile)apl[0];
						AllocationProfile ap = ap2;
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
								aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,
								aStoreFilter,
								-1);
							this.ProcessAction(
								aApplicationTransaction.SAB,
								aApplicationTransaction,
								awfs,
								ap,
								true,
								Include.NoRID);

							if (_processCancelled)
								break;
						}
						// BEGIN MID Track # 2983 Size Need not being executed in workflow	
						//=============
						// Set status
						//=============
						//if (!_processCancelled)
						//{
						//	ap.BulkSizeBreakoutPerformed = true;
						//	if (this._distributeChange == eDistributeChange.ToParent)
						//		ap.BottomUpSizePerformed = true;
						//	ap.WriteHeader();
						//	aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						//}
						// END MID Track # 2983 Size Need not being executed in workflow

					}
					else
					{
						//					foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
						//					{
						AllocationProfile ap = ap2;
						AllocationWorkFlowStep awfs = 
							new AllocationWorkFlowStep(
							this,
							new GeneralComponent(eGeneralComponentType.Total),
							false,
							true,
							aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,
							aStoreFilter,
							-1);
						this.ProcessAction(
							aApplicationTransaction.SAB,
							aApplicationTransaction,
							awfs,
							ap,
							true,
							aStoreFilter);
						
						// BEGIN MID Track # 2983 Size Need not being executed in workflow	
						//=============
						// Set status
						//=============
						//if (!_processCancelled)
						//{
						//	ap.BulkSizeBreakoutPerformed = true;
						//	if (this._distributeChange == eDistributeChange.ToParent)
						//		ap.BottomUpSizePerformed = true;
						//	ap.WriteHeader();
						//	aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						//}
						// END MID Track # 2983 Size Need not being executed in workflow
					}
				}
			}
			catch (Exception err)
			{
				SAB.ApplicationServerSession.Audit.Log_Exception(err);
				//Begin TT#991 - JScott - Processed without changing focus to the header I wanted to Allocate(still focused on the prior header) selected process and received a null reference exception.
				throw err;
				//End TT#991 - JScott - Processed without changing focus to the header I wanted to Allocate(still focused on the prior header) selected process and received a null reference exception.
			}
			// END Track #5727 stodd
		}

			public override void ProcessAction(
				SessionAddressBlock aSAB, 
				ApplicationSessionTransaction aApplicationTransaction, 
				ApplicationWorkFlowStep aApplicationWorkFlowStep, 
				Profile aAllocationProfile,
				bool WriteToDB,
				int aStoreFilterRID)
			{
				string msg;

				// begin TT#421 Detail packs/bulk not allocated by Size Need Method.
                //AllocationProfile ap = (AllocationProfile) aAllocationProfile; // MID Track # 2983 Size Need not performed by workflow
                AllocationProfile ap = aAllocationProfile as AllocationProfile;
                Audit audit = aSAB.ApplicationServerSession.Audit;
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
                this._actionAuditList = new ArrayList();  // MID Track 4967 Size Functions Not Showing Total Qty
				try
				{
                    ap.ResetTempLocks(false); // TT#421 Detail packs/bulk not allocated by Size Need Method
					_reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes when run from Workflow does not recognize Reserve Store.

					if (!Enum.IsDefined(typeof(eAllocationMethodType),(int)aApplicationWorkFlowStep._method.MethodType))
					{
						throw new MIDException(eErrorLevel.severe,
							(int)(eMIDTextCode.msg_WorkflowTypeInvalid),
							MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));

					}
					
					// BEGIN A&F Generic Size Curve
//					if (_genCurveError)
//					{
//						aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
//						return;
//					}
//					if (base.GenericSizeCurveDefined() && base.GenCurveHash == null)
//					{
						try
						{
							if (!base.OkToProcess(aApplicationTransaction, ap))	// Generic Size Curve
							{
								aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track 5183 Workflow should stop after Severe Error
								return;
							}
						}
						catch (MIDException)
						{
							aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
//							_genCurveError = true;
							throw;
						}
//					}
					// END A&F Generic Size Curve

                    // begin TT#421 Detail packs/bulk not allocated by size need method
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
					//	catch
					//	{
					//		SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile), this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit 
					//		throw;
					//	}
					//}
                    // end TT#421 Detail packs/bulk not allocated by size need method

// (CSMITH) - BEG MID Track #3156: Headers with status Rec'd OUT of Bal are being processed
					if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
					{
						msg = string.Format(
							SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false),
																	   MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
						msg = msg.Replace(" action for headers", " method for header");
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Warning,
							MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod) + ": [" + this.Name + "], Header: [" + ap.HeaderID + "] - " + msg, // MID Track 5778 - Scheduler 'Run Now' feature gets Error in Audit
							this.GetType().Name);
						aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
					}
					else
					{
						// AllocationProfile ap = (AllocationProfile) aAllocationProfile; // MID Track # 2938 Size Need not performed in workflow

						ProfileList tempStoreFilterList = null;
						//*******************************
						// Apply STORE FILTER if present
						//*******************************
						// BEGIN Issue 5727 stodd
						bool outdatedFilter = false;	
						// END Issue 5727
						// Begin TT354 - stodd - Filter in Fill Size Method works when processed as stand alone; but does not work when placed into a workflow
						if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)
						// End TT354 - stodd - Filter in Fill Size Method works when processed as stand alone; but does not work when placed into a workflow
						{
							// BEGIN Issue 5727 stodd
							FilterData storeFilterData = new FilterData();
							string filterName = storeFilterData.FilterGetName(aStoreFilterRID);
							tempStoreFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(ap, aStoreFilterRID, ref outdatedFilter);
							if (outdatedFilter)
							{
								msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
								msg = msg.Replace("{0}",filterName);
								string suffix = ". Method " + this.Name + ". Header [" + ap.HeaderID + "]"; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
								string auditMsg = msg + suffix;
								SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
								throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
							}
							msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_StoreFilterAppliedToMethod,false);
							msg = msg.Replace("{0}",filterName);
							msg = msg.Replace("{1}",MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)); // MID Track 5778 - Scheduler 'Run Now' feature gets Error in Audit
							msg = msg.Replace("{2}",this.Name);
							msg = msg.Replace("{3}",ap.HeaderID);
							SAB.ApplicationServerSession.Audit.Add_Msg(
								eMIDMessageLevel.Information,eMIDTextCode.msg_al_StoreFilterAppliedToMethod,
								msg, this.GetType().Name);
							// END Issue 4632
						}
						else if (this.StoreFilterRid != Include.NoRID)  // try the one ON this method
						{
							// BEGIN Issue 5727 stodd
							FilterData storeFilterData = new FilterData();
							string filterName = storeFilterData.FilterGetName(StoreFilterRid);
							// END Issue 5727 stodd
							tempStoreFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(ap, StoreFilterRid, ref outdatedFilter);
							if (outdatedFilter)
							{
								msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
								msg = msg.Replace("{0}",filterName);
								string suffix = ". Method " + this.Name + ". Header [" + ap.HeaderID + "]"; // MID Track 5778 - Scheduler 'Run Now' gets error in audit
								string auditMsg = msg + suffix;
								SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
								throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
							}
						}
						else
						{
							tempStoreFilterList = aApplicationTransaction.GetMasterProfileList(eProfileType.Store);
						}
						//===================================================
						// Apply Store Eligibility and exclude Reserve Store
						//===================================================
						ProfileList storeFilterList = new ProfileList(eProfileType.Store);
						int storeCnt = tempStoreFilterList.Count;
						for (int i=0;i<storeCnt;i++)
						{
							StoreProfile sp = (StoreProfile)tempStoreFilterList[i];
							//if (!_allocProfile.GetStoreOut(colorComponent, sp.Key)
							if (ap.GetStoreIsEligible(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
                                && ap.GetIncludeStoreInAllocation(sp.Key)) // TT#1401 - JEllis - Urban Reservation Store pt 11
							{
								// don't alloc to reserve store either
								// otherwise add store to list
                                if (sp.Key != _reserveStoreRid || _includeReserve)                     //TT#1608-MD - Prior Header - Include Reserve - SRisch 06/05/2015
									storeFilterList.Add(sp);
							}
						}

						//=======================
						// get target componenet
						//=======================
						AllocationWorkFlowStep aAllocationWorkFlowStep = (AllocationWorkFlowStep)aApplicationWorkFlowStep;
						_targetComponent = aAllocationWorkFlowStep.Component;
						_basisComponent = BuildComponentForBasisHeader(_targetComponent, (eComponentType)_HeaderComponent, this._BasisClrRid);

						//===================================================
						// get Basis Header
						//===================================================
						_basisAllocProfile = null;
						if (this._BasisHdrRid != Include.NoRID)
						{
                            // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
							//ApplicationSessionTransaction basisTransaction = new ApplicationSessionTransaction(this.SAB); 
                            ApplicationSessionTransaction basisTransaction = SAB.ApplicationServerSession.CreateTransaction();
                            // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

							// Look in current transaction for prior Header
							_basisAllocProfile = aApplicationTransaction.GetAllocationProfile(this._BasisHdrRid);
							// if not in current transaction, build a transaction and add it to it.
							if (_basisAllocProfile == null)
							{
								int[] headerList = {this._BasisHdrRid};
								AllocationProfileList basisApList = new AllocationProfileList(eProfileType.Allocation);
								basisApList.LoadHeaders(basisTransaction, null, headerList, aApplicationTransaction.SAB.ApplicationServerSession);	 // TT3488 - MD - Jellis - Gropu Allocation				
								_basisAllocProfile = (AllocationProfile)basisApList[0];
							}

							basisTransaction.AddAllocationProfile(_basisAllocProfile);
						}
                        // Begin TT#212 -  RMatelic - Basis Size Method gets null referrence error when Basis header has been deleted and method is run in a workflow.
                        else
                        {
                            string message =
                            MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod)
                            + " ["
                            + this.Name
                            + "] " + MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderID) + " ["
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
                        }
                        // End TT#212

						// Begin ISSUE # 3114 - stodd
                        if (_basisAllocProfile.Key == ap.Key)
                        {
                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                            // begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
                            //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_BasisAndTargetHeaderEqual), this.GetType().Name);
                            //throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_BasisAndTargetHeaderEqual),
                            //	MIDText.GetText(eMIDTextCode.msg_al_BasisAndTargetHeaderEqual));
                            msg =
                                string.Format(
                                MIDText.GetText(eMIDTextCode.msg_al_BasisAndTargetHeaderEqual),
                                MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod),
                                this.Name,
                                ap.HeaderID);
                            SAB.ApplicationServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Severe,
                                eMIDTextCode.msg_al_BasisAndTargetHeaderEqual,
                                msg,
                                this.GetType().Name);
                            throw new MIDException(eErrorLevel.severe,
                                (int)(eMIDTextCode.msg_al_BasisAndTargetHeaderEqual),
                                msg);
                            // end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
                        }
                        // End ISSUE # 3114 - stodd

						_ruleMethod = (eBasisSizeMethodRuleType)_Rule;

						_basisComponentType = (eComponentType)_HeaderComponent;

					//====================================================================
					// Is either the Target or the Basis component = MatchingColors?
					//====================================================================
						if (_targetComponent.ComponentType == eComponentType.MatchingColor || _basisComponentType == eComponentType.MatchingColor)
						{
							ProcessMatchingColors(ap, _targetComponent, aSAB, aApplicationTransaction, storeFilterList);
						}
						else
						{
							//==============================
							// Get componet to process
							//==============================
							switch (_targetComponent.ComponentType)
							{
								case (eComponentType.SpecificColor):
									_basisComponent = BuildComponentForBasisHeader(_targetComponent, (eComponentType)_HeaderComponent, this._BasisClrRid);
									ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, _targetComponent, storeFilterList);
									break;
								case (eComponentType.SpecificPack):
								case (eComponentType.SpecificSize):
								case (eComponentType.Bulk):
								case (eComponentType.DetailType):	
								case (eComponentType.GenericType):
								case (eComponentType.AllPacks):
								case (eComponentType.AllGenericPacks):
								case (eComponentType.AllNonGenericPacks):
								case (eComponentType.AllSizes):
								{
									// not valid
									// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
									//try
									//{
									//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_ColorInstructionComponentMismatch),
									//		MIDText.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch));
									//}
									//catch
									//{
									//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch), this.ToString());
									//	throw;
									//}
									msg = 
										MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod) 
										+ ": [" + this.Name 
										+ "], Header: [" 
										+ ap.HeaderID + "] - " 
										+ MIDText.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch);
									SAB.ApplicationServerSession.Audit.Add_Msg(
										eMIDMessageLevel.Severe, 
										msg, 
										this.GetType().Name);
									throw new MIDException(
										eErrorLevel.severe,
										(int)(eMIDTextCode.msg_ColorInstructionComponentMismatch),
										msg);
 									// end MID track 5778 - Scheduler 'Run Now' feature gets error in Audit
								}
								case (eComponentType.Total):
								case (eComponentType.AllColors):
								{
									Hashtable colors = ap.BulkColors;
									if (colors != null)
									{
										foreach (HdrColorBin hcb in colors.Values)
										{
											AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
											_basisComponent = BuildComponentForBasisHeader(_targetComponent, (eComponentType)_HeaderComponent, this._BasisClrRid);
											ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aColor, storeFilterList);
										}
									}
									break;
								}
								case (eComponentType.ColorAndSize):				
								{
//									//=============================================================================================
//									// many color/size components can be selected.
//									// Since we are only processing the color, we only want to go through this logic once.
//									// if the user cancels, it's not a problem.
//									// but if the user processes the color, we want to stop after processing the color only once.
//									//=============================================================================================
//									if (_componentsProcessed == 0)
//									{
//										string errMsg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeComponentSelectionInvalid));
//
//										System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
//											errMsg,
//											"Invalid Header Component Selection",
//											System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);
//
//										if (diagResult == System.Windows.Forms.DialogResult.OK)
//										{
//											AllocationColorSizeComponent colorSizeComponent = (AllocationColorSizeComponent)_targetComponent;
//
//											AllocationColorOrSizeComponent aColor = (AllocationColorOrSizeComponent)colorSizeComponent.ColorComponent;
//											ProcessComponent(aSAB, aApplicationTransaction,  (AllocationProfile)aAllocationProfile, aColor, storeFilterList);
//										}
//										else
//										{
//											errMsg += "  Cancel was chosen.";
//											SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errMsg, this.ToString());
//											SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Cancelled, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
//											_processCancelled = true;
//										}
//									}
									break;
								}
								default:
								{
									//  unknown component

									//===========================================================================================================
									// WHY ARE WE THROWING AND CATCHING RIGHT AFTER?
									// The Exception is missing critical info until it's gets thrown.  Once thrown (and caught) we can then add
									// it to the Audit log.
									//===========================================================================================================
									// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
									//try
									//{
									//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_UnknownAllocationComponent),
									//		MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent));
									//}
									//catch
									//{
									//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent), this.ToString());
									//	throw;
									//}
									msg = 
										MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod) 
										+ ": [" + this.Name 
										+ "], Header: [" 
										+ ap.HeaderID + "] - " 
										+ MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent);
									SAB.ApplicationServerSession.Audit.Add_Msg(
										eMIDMessageLevel.Severe,
										eMIDTextCode.msg_UnknownAllocationComponent,
										msg,
										this.GetType().Name);
                                    throw new MIDException(
										eErrorLevel.severe,
										(int)eMIDTextCode.msg_UnknownAllocationComponent,
										msg);
									// end MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
								}

							}
						}
						// BEGIN MID Track # 2983 Size Need not performed in workflow
						if (WriteToDB) 
						{
							ap.WriteHeader();
						}
						aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						// END MID Track # 2983 Size Need not performed in workflow
						msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
						msg = msg.Replace("{0}", MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						msg = msg.Replace("{1}", "[" + this.Name + "] Header: [" + ap.HeaderID + "]"); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						// begin MID Track 4967 Size Functions Not Showing Total Qty
						// // begin MID Track 4448 AnF Audit Enhancement
						//string[] packOrColorComponentName = null;
						//switch (_targetComponent.ComponentType)
						//{
						//	case (eComponentType.Total):
						//	case (eComponentType.Bulk):
						//	{
						//		packOrColorComponentName = new string[1];
						//		packOrColorComponentName[0] = string.Empty;
						//		break;
						//	}
						//	case (eComponentType.SpecificColor):
						//	{
						//		packOrColorComponentName = new string[1];
						//		packOrColorComponentName[0] = aApplicationTransaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)_targetComponent).ColorRID).ColorCodeName;
						//		break;
						//	}
						//	case (eComponentType.ColorAndSize):
						//	{
						//		GeneralComponent colorComponent = 
						//			((AllocationColorSizeComponent)_targetComponent).ColorComponent;
						//		switch (colorComponent.ComponentType)
						//		{
						//			case (eComponentType.SpecificColor):
						//			{
						//				packOrColorComponentName = new string[1];
						//				packOrColorComponentName[0] = 
						//					aApplicationTransaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)colorComponent).ColorRID).ColorCodeName;
						//				break;
						//			}
						//			case (eComponentType.AllColors):
						//			{
						//				packOrColorComponentName = new string[ap.BulkColors.Count];
						//				int i = 0;
						//				foreach (HdrColorBin hcb in ap.BulkColors.Values)
						//				{
						//					packOrColorComponentName[i] = aApplicationTransaction.GetColorCodeProfile(hcb.ColorKey).ColorCodeName;
						//				}
						//				break;
						//			}
						//			default:
						//			{
						//				packOrColorComponentName = new string[1];
						//				packOrColorComponentName[0] = string.Empty;
						//				break;
						//			}
						//		}
						//		break;
						//	}
						//	default:
						//	{
						//		packOrColorComponentName = new string[1];
						//		packOrColorComponentName[0] = string.Empty;
						//		break;
						//	}
						//}
						//for (int j=0; j<packOrColorComponentName.Length; j++)
						//{
						//	aApplicationTransaction.WriteAllocationAuditInfo
						//		(ap.Key,
						//		0,
						//		this.MethodType,
						//		this.Key,
						//		this.Name,
						//		_targetComponent.ComponentType,
						//		packOrColorComponentName[j],
						//		null,   // MID Track 4448 AnF Audit Enhancement
						//		0,      // MID Track 4448 AnF Audit Enhancement
						//		0       // MID Track 4448 AnF Audit Enhancement
						//		);      // MID Track 4448 AnF Audit Enhancement
						//	// NOTE:  Units allocated and store count are not tracked by Basis Size
						//}
						// // end MID Track 4448 AnF Audit Enhancement	
						foreach (AllocationActionAuditStruct auditStruct in this._actionAuditList)
						{

							aApplicationTransaction.WriteAllocationAuditInfo(
								ap.Key,
								0,
								this.MethodType,
								this.Key,
								this.Name,
								((AllocationColorSizeComponent)auditStruct.Component).ColorComponent.ComponentType,
								aApplicationTransaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)((AllocationColorSizeComponent)auditStruct.Component).ColorComponent).ColorRID).ColorCodeName,
								null,
								ap.GetQtyAllocated(auditStruct.Component) - auditStruct.QtyAllocatedBeforeAction,
								Math.Abs(ap.GetCountOfStoresWithAllocation(auditStruct.Component) - auditStruct.StoresWithAllocationBeforeAction));
						}
						// end MID track 4967 Size Functions Not Showing Total Qty
					}
// (CSMITH) - END MID Track #3156
				}
				catch (MIDException ex)
				{
					aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ErrorMessage, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
					msg = msg.Replace("{0}", MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					msg = msg.Replace("{1}", "[" + this.Name + "] Header: [" + ap.HeaderID + "]"); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					throw;
				}
				catch (Exception ex)
				{
					aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
					msg = msg.Replace("{0}", MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					msg = msg.Replace("{1}", "[" + this.Name + "] Header: [" + ap.HeaderID + "]"); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					throw;
				}
                // begin TT#421 Detail packs/bulk not allocated by Size Need Method
                finally
                {
                    ap.ResetTempLocks(true);
                }
                // end TT#421 Detail packs/bulk not allocated by Size Need Method
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
			ApplicationSessionTransaction aApplicationTransaction,
			ProfileList storeFilterList)
		{
			if ((targetComponent.ComponentType == eComponentType.MatchingColor && _basisComponentType == eComponentType.MatchingColor)
				|| (targetComponent.ComponentType == eComponentType.Total && _basisComponentType == eComponentType.MatchingColor)
				|| (targetComponent.ComponentType == eComponentType.AllColors && _basisComponentType == eComponentType.MatchingColor))

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
						// TODO ADD MESSAGE
						ColorCodeProfile cp = aApplicationTransaction.GetColorCodeProfile(hcb.ColorCodeRID);
						// begin mID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
						//SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "No match found for color" + cp.ColorCodeName , this.GetType().Name);
						string msg = string.Format(
							MIDText.GetText(eMIDTextCode.msg_al_TargetHeaderColor_HasNoMatching_BasisColor),
							MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod),
							this.Name,
							ap.HeaderID,
							cp.ColorCodeName,
							_basisAllocProfile.HeaderID);
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Severe,
							eMIDTextCode.msg_al_TargetHeaderColor_HasNoMatching_BasisColor,
							msg,
							this.GetType().Name);
						// end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					}

					if (aColor != null)
					{
						foundColor = true;
						AllocationColorOrSizeComponent aColorComp = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
						_basisComponent = BuildComponentForBasisHeader(aColorComp, eComponentType.SpecificColor, aColorComp.ColorRID);
						ProcessComponent(aSAB, aApplicationTransaction, ap, aColorComp, storeFilterList);
						break;
					}
				}
				if (!foundColor)
				{
					// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingColors),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors));
					//}
					//catch
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors), this.GetType().Name);
					//	throw;
					//}
					string msg = 
						MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)
						+ " ["
						+ this.Name
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors)
						+ " ["
						+ ap.HeaderID
						+ "]";
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingColors,
						msg,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_NoMatchingColors),
						msg);
					// end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
				}
			}
			else if (targetComponent.ComponentType == eComponentType.SpecificColor)
			{
				AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)targetComponent;
				if (this._basisAllocProfile.BulkColorIsOnHeader(colorComponent.ColorRID))
				{
					_basisComponent = BuildComponentForBasisHeader(colorComponent, eComponentType.MatchingColor, colorComponent.ColorRID);
					ProcessComponent(aSAB, aApplicationTransaction,  ap, colorComponent, storeFilterList);	
				}
				else
				{
					// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_NoMatchingColors),
					//		MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors));
					//}
					//catch
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors), this.GetType().Name);
					//	throw;
					//}
					string msg = 
						MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)
						+ " ["
						+ this.Name
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_NoMatchingColors)
						+ " ["
						+ ap.HeaderID
						+ "]";
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_NoMatchingColors,
						msg,
						this.GetType().Name);
					throw new MIDException(
						eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_NoMatchingColors),
						msg);
					// end MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
				}

			} 
			else if (_basisComponentType == eComponentType.SpecificColor)
			{
				if (ap.BulkColorIsOnHeader(this._BasisClrRid))
				{
					_basisComponent = BuildComponentForBasisHeader(null, eComponentType.SpecificColor, this._BasisClrRid);
					ProcessComponent(aSAB, aApplicationTransaction,  ap, _basisComponent, storeFilterList);	
				}
			}									   
			else
			{
                // begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
				//try
				//{
				//	// incompatable components between basis and target
				//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched),
				//		MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched));
				//}
				//catch
				//{
				//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched), this.GetType().Name);
				//	throw;
				//}
				string msg = 
					MIDText.GetTextOnly((int)eMIDTextCode.frm_BasisSizeMethod)
					+ " ["
					+ this.Name
					+ "] Target Header ["
					+ ap.HeaderID
					+ "] " 
					+ MIDText.GetText(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched);
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Severe,
					eMIDTextCode.msg_al_RuleAllocationComponentsMismatched,
					msg,
					this.GetType().Name);
				throw new MIDException(
					eErrorLevel.severe,
					(int)(eMIDTextCode.msg_al_RuleAllocationComponentsMismatched),
					msg);
				// end MID Track 5778 - Scheduler - 'Run Now' feature gets error in Audit
			}
		}

		/// <summary>
		/// processes a single component
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		/// <param name="aAllocationProfile"></param>
		/// <param name="aComponent"></param>
		private void ProcessComponent(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction,  
			AllocationProfile aAllocationProfile,
			GeneralComponent aComponent,
			ProfileList storeList)
		{

			try
			{
				_componentsProcessed++;
				AllocationProfile ap = (AllocationProfile) aAllocationProfile;
				AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
				ProfileList allSizeList = null;

				// begin A&F Generic Size Curve
				//if (SizeCurveGroupRid != Include.NoRID)
				//{
				//	SizeCurveGroupProfile sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
				//	allSizeList = sizeCurveGroup.SizeCodeList;
				//}
				//else
				//{
				//	SizeGroupProfile sizeGroup = new SizeGroupProfile(this.SizeGroupRid);
				//	allSizeList = (ProfileList)sizeGroup.SizeCodeList;
				//}

				SizeCurveGroupProfile sizeCurveGroup;
				if (base.GenericSizeCurveDefined())
				{
					try
					{
						int genSizeCurveGroupRID = (int)base.GenCurveHash[ap.Key];
						sizeCurveGroup = new SizeCurveGroupProfile(genSizeCurveGroupRID);
						allSizeList = sizeCurveGroup.SizeCodeList;
					}
					catch
					{
//						string errMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),string.Empty,ap.HeaderID);
						string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),string.Empty,ap.HeaderID);
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoSizeCurveForHeader,errMessage);
					}
				}
				else if (SizeCurveGroupRid != Include.NoRID)
				{
					sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
					allSizeList = sizeCurveGroup.SizeCodeList;
				}
				else
				{
					SizeGroupProfile sizeGroup = new SizeGroupProfile(this.SizeGroupRid);
					allSizeList = (ProfileList)sizeGroup.SizeCodeList;
				}
				// end A&F Generic Size Curve
				// begin MID Track 4372 Generic Size Constraint
				int sizeConstraintRID = this.SizeConstraintRid;
				if (base.GenericSizeConstraintsDefined())
				{
					try
					{
						sizeConstraintRID = (int)base.GenConstraintHash[ap.Key];
					}
					catch
					{
						string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeConstraintForHeader),string.Empty, ap.HeaderID);
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoSizeConstraintForHeader, errMessage);
					}
				}
				// end MID Track 4372 Generic Size Contraint
				HdrColorBin colorBin = ap.GetHdrColorBin(colorComponent.ColorRID);

//					SizeNeedAlgorithm sna = new SizeNeedAlgorithm(SAB, aApplicationTransaction);

				//================================
				// call Size Need Processing
				// this returns on plan filled in
				//================================
//					SizeNeedResults sizeResults = sna.ProcessBasisSize(ap, _basisAllocProfile, 
//						(AllocationColorOrSizeComponent)_basisComponent, colorComponent, _ruleMethod, _quantity,
//						null, sizeGroup, storeList, false, false, eSizeNeedColorPlan.PlanForSpecificColorOnly);
                SizeNeedResults sizeResults = ProcessBasisSize(ap, _basisAllocProfile, 
					(AllocationColorOrSizeComponent)_basisComponent, colorComponent, _ruleMethod, _quantity,
					//allSizeList, storeList, this.SizeConstraintRid, this.RulesCollection,  // MID Track 4372 Generic Size Constraint
					allSizeList, storeList, sizeConstraintRID, this.RulesCollection,         // MID Track 4372 Generic Size Constraint
					eSizeNeedColorPlan.PlanForSpecificColorOnly);

				// begin MID track 4967 Size Functions Not Showing Total Qty
				GeneralComponent sizeComponent = new GeneralComponent(eGeneralComponentType.AllSizes);
				AllocationColorSizeComponent acsc = new AllocationColorSizeComponent(colorComponent, sizeComponent);
				this._actionAuditList.Add(new AllocationActionAuditStruct
					((int)eAllocationMethodType.BasisSizeAllocation,
                    ap,     //TT#1199 - MD - NEED allocation # of stores is zero - rbeck
					acsc,
					ap.GetQtyAllocated(acsc),
					ap.GetCountOfStoresWithAllocation(acsc),
					true,
					false));
				// end MID track 4967 Size Functions Not Showing Total Qty
				//======================================
				// Set Store, Color, Size allocation
				//======================================
				int storeCnt = storeList.Count;
				for (int i=0;i<storeCnt;i++)
				{
					StoreProfile sp = (StoreProfile)storeList[i];
					Index_RID storeIndex = (Index_RID)ap.StoreIndex(sp.Key);

					ArrayList sizeList = sizeResults.Sizes;
					int sizeCnt = sizeList.Count;
					for (int j=0;j<sizeCnt;j++)
					{
						// begin MID Track 3430 Proportional Allocation wrong when units already allocated
						//int units = sizeResults.GetAllocatedUnits(sp.Key, (int)sizeList[j]);
						int units =
							sizeResults.GetAllocatedUnits(sp.Key, (int)sizeList[j])
							+ ap.GetStoreQtyAllocated(colorBin, (int)sizeList[j], storeIndex);
						// end MID Track 3430 Proportional Allocation wrong when units already allocated
						ap.SetStoreQtyAllocated(colorBin, (int)sizeList[j], storeIndex, units, _distributeChange, false);
					}
				}

				ap.BulkSizeBreakoutPerformed = true; // MID Track # 2983 Workflow not processing size need
			}
			catch (Exception ex)
			{
				aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
				// begin MID Track 5778 - Scheduler 'Run Now' feature gets error on Audit
				//SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Severe, 
					ex.ToString() + " " + MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod) + " [" + this.Name + "] Header +  [" + aAllocationProfile.HeaderID + "]", 
					this.GetType().Name); 
				// end MID Track 5778 - Scheduler 'Run Now' feature gets error on Audit
				throw;
			}
		}


		private GeneralComponent BuildComponentForBasisHeader(GeneralComponent targetComponent, eComponentType componentType, int colorRid)
		{
			GeneralComponent basisComponent = null;

			switch (componentType)
			{
//				case (eComponentType.Bulk):
//				{
//					basisComponent = new GeneralComponent(eComponentType.Bulk);
//					break;
//				}
				case (eComponentType.SpecificColor):
				{
					AllocationColorOrSizeComponent color = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRid);
					basisComponent = color;
					break;
				}
//				case (eComponentType.SpecificPack):
//				{
//					AllocationPackComponent pack = new AllocationPackComponent(this.PackName);
//					basisComponent = pack;
//					break;
//				}
//				case (eComponentType.Total):
//				{
//					basisComponent = new GeneralComponent(eComponentType.Total);
//
//					break;
//				}
				case (eComponentType.MatchingColor):
				{
					basisComponent = targetComponent;
					break;
				}
//				case (eComponentType.MatchingPack):
//				{
//					basisComponent = targetComponent;
//					break;
//				}
				default:
				{
                    // begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					//try
					//{
					//	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_al_InvalidBasisComponent),
					//		MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent));
					//}
					//catch
					//{
					//	SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent), this.GetType().Name);
					//	throw;
					//}
					string msg = 
						MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod)
						+ " [" 
						+ this.Name
						+ "] "
						+ MIDText.GetText(eMIDTextCode.msg_al_InvalidBasisComponent);
						 
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Severe,
						eMIDTextCode.msg_al_InvalidBasisComponent,
						msg,
						this.GetType().Name); 
					throw new MIDException(
						eErrorLevel.severe,
						(int)(eMIDTextCode.msg_al_InvalidBasisComponent),
						msg);
					// end MID Track 5778 - Scheduler 'Run Now' feature gets error on Audit
				}
			}

			return basisComponent;
		}

		// BEGIN Change J.Ellis 1/28/2005 moved this method from size need  
		private SizeNeedResults ProcessBasisSize(
			AllocationProfile allocProfile, 
			AllocationProfile basisAllocProfile,
			AllocationColorOrSizeComponent basisColorComponent, 
			AllocationColorOrSizeComponent targetColorComponent, 
			eBasisSizeMethodRuleType aRuleType, 
			int aRuleQuantity,
			ProfileList allSizeList, 
			ProfileList aStoreList, 
			int constraintModelRid, 
			CollectionRuleSets ruleCollection, eSizeNeedColorPlan sizeColorPlan)
		{
            // Begin TT#1621-MD - JSmith - Null Reference in Basis Size Mehtod
            //_sizeNeedResults = new SizeNeedResults();
            _sizeNeedResults = new SizeNeedResults(allocProfile, eVSWSizeConstraints.None);
            // End TT#1621-MD - JSmith - Null Reference in Basis Size Mehtod
			_sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(SAB, constraintModelRid, allocProfile, targetColorComponent.ColorRID);  // TT#2155 - JEllis - Fill Size Holes Null Reference
            //_sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(SAB, constraintModelRid, allocProfile, targetColorComponent.ColorRID, false); // TT#2155 - JEllis - Fill Size Holes Null Reference
            _sizeSubstituteHash = BuildSubstituteHashTable(allSizeList);

			try
			{
				DateTime beginTime;
				DateTime endTime;
				bool process = true;
				int priorQty = 0, qty =0;
				int storeCnt = 0;
				int sizeCnt = 0;

				if (aStoreList.Count <= 0)
				{
					// MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
					//_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess), this.ToString());
					string msg = 
						MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod)
						+ " [" 
						+ this.Name
						+ "] Target Header ["
						+ allocProfile.HeaderID
						+ "] Basis Header ["
						+ basisAllocProfile.HeaderID
						+ MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess);
						 
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_al_NoStoresToProcess,
						msg,
						this.GetType().Name); 
					// end MID Track 5778 - Scheduler 'Run Now' feature gets error on Audit
					process = false;
				}
				else
				{
					//============================
					// Is color in alloc profile
					//============================
					if (!allocProfile.BulkColors.ContainsKey(targetColorComponent.ColorRID))
					{
						process = false;
					}
				}

				if (process)
				{
					//=============================================================================
					// Build a temp Size list of target sizes on color
					//=============================================================================
					ArrayList tempSizeList = new ArrayList();
					HdrColorBin colorBin = allocProfile.GetHdrColorBin(targetColorComponent.ColorRID);	
					foreach (HdrSizeBin sizeBin in colorBin.ColorSizes.Values)
					{
                        tempSizeList.Add(sizeBin.SizeCodeRID); // Assortment: color/size changes
					}

					//===================================================================
					// finalize Size list making sure target sizes and basis Sizes match
					//===================================================================
					ArrayList sizeList = new ArrayList();
					HdrColorBin basisColorBin = basisAllocProfile.GetHdrColorBin(basisColorComponent.ColorRID);	
					//foreach (HdrSizeBin basisSizeBin in basisColorBin.ColorSizes.Values)
					foreach (int tempSizeKey in tempSizeList)
					{
						//========================================================================
						// Size Substitution
						// If there's been a size substitution, check the substitute size
						// instead of the size from the target header.  BUT it's the target 
						// header's size that gets placed in the sizeList--it'll get substituted 
						// later as needed. 
						//========================================================================
						int checkForSizeKey;
						if (_sizeSubstituteHash.ContainsKey(tempSizeKey))
							checkForSizeKey = (int)_sizeSubstituteHash[tempSizeKey];
						else
							checkForSizeKey = tempSizeKey;
						if (basisColorBin.ColorSizes.Contains(checkForSizeKey))
						{
							//sizeList.Add(tempSizeKey); // MID Track 3492 Size Need with constraint not allocating correctly
							sizeList.Add(this._aSAB.HierarchyServerSession.GetSizeCodeProfile(tempSizeKey)); // MID Track 3492 Size Need with constraint not allocating correctly
							_sizeNeedResults.AddSize(tempSizeKey);
						}
						else
						{
							// begin MID Track 5778 - Scheduler 'Run Now' feature gets error in Audit
							//string warningMsg = MIDText.GetText(eMIDTextCode.msg_al_SizeNotOnBasis);
							//warningMsg = warningMsg.Replace("{0}", tempSizeKey.ToString(CultureInfo.CurrentUICulture));
							//_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, warningMsg, this.ToString());
							string warningMsg = 
								MIDText.GetTextOnly(eMIDTextCode.frm_BasisSizeMethod)
								+ " [" 
								+ this.Name
								+ "] Target Header ["
								+ allocProfile.HeaderID
								+ "] Basis Header ["
								+ basisAllocProfile.HeaderID
	                            + "] Basis Color ["
								+ basisAllocProfile.AppSessionTransaction.GetColorCodeProfile(basisColorBin.ColorCodeRID).ColorCodeID
								+ "] "
								+ string.Format(MIDText.GetText(eMIDTextCode.msg_al_SizeNotOnBasis), basisAllocProfile.AppSessionTransaction.GetSizeCodeProfile(checkForSizeKey).SizeCodeName);
						 
							_aSAB.ApplicationServerSession.Audit.Add_Msg(
								eMIDMessageLevel.Warning,
								eMIDTextCode.msg_al_SizeNotOnBasis,
								warningMsg,
								this.GetType().Name); 
							// end MID Track 5778 - Scheduler 'Run Now' feature gets error on Audit
						}
					}
					beginTime = System.DateTime.Now;

					//============================================================================
					// if the Color total (for any store) > the size total (for any store)
					//		set a switch to indicate  TopDownSizePerformed
					//		eDistributeChange is set to NONE
					//	else
					//		set a switch to indicate  ButtomUpSizePerformed
					//		eDistributeChange is set to TOPARENT
					//============================================================================
					int colorTotalQty = 0;
					int sizeTotalQty = 0;
					storeCnt = aStoreList.Count;
					_distributeChange = eDistributeChange.ToParent;
					for (int i=0;i<storeCnt;i++)
					{
						colorTotalQty = 0;
						sizeTotalQty = 0;
						StoreProfile sp = (StoreProfile)aStoreList[i];
						colorTotalQty = allocProfile.GetStoreQtyAllocated(targetColorComponent.ColorRID, sp.Key);

						for (int j=0;j<sizeCnt;j++)
						{
							sizeTotalQty += basisAllocProfile.GetStoreQtyAllocated(targetColorComponent.ColorRID, ((SizeCodeProfile)sizeList[j]).Key, sp.Key); // MID Track 3492 Size Need with constraints not allocating correctly
						}

						if (colorTotalQty > sizeTotalQty)
							this._distributeChange = eDistributeChange.ToNone;
					}

					try
					{
						if (base.RulesStoreGroupLevelHash == null)	// Generic Size Curve; not related 
						{
							base.BuildRulesDecoder(this.SG_RID);
						}
						switch (aRuleType)
						{
							case (eBasisSizeMethodRuleType.Exact):
							{
								AllocationWorkMultiple basisHeaderWorkMultiple = GetQtyPerPack(basisColorComponent, basisAllocProfile);
								AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(targetColorComponent, allocProfile);

								storeCnt = aStoreList.Count;
								int basisSizeCodeRid = Include.NoRID;
								for (int i=0;i<storeCnt;i++)
								{
									StoreProfile sp = (StoreProfile)aStoreList[i];
									sizeCnt = sizeList.Count;
									for (int j=0;j<sizeCnt;j++)
									{
										SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; // MID Track 3492 Size Need with constraints not allocating correctly
                                        if (!IsStoreExcluded(sp.Key, targetColorComponent.ColorRID, sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action
										{
											//=============================
											// Size Substitution
											//=============================
											if (_sizeSubstituteHash.ContainsKey(sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraint not allocating correctly
												basisSizeCodeRid = (int)_sizeSubstituteHash[sizeCodeProfile.Key]; // MID Track 3492 Size Need with constraint not allocating correctly
											else
												//basisSizeCodeRid = (int)sizeList[j]; // MID Track 3492 Size Need with constraint not allocating correctly
												basisSizeCodeRid = sizeCodeProfile.Key; // MID Track 3492 Size Need with constraint not allocating correctly

											//=============================================
											// override mult if found
											//=============================================
											// int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, (int)sizeList[j]); // MID Track 3492 Size Need with constraint not allocating correctly
                                            int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraint not allocating correctly// TT#1391 - TMW New Action
											if (overrideMult > 0)
												currHeaderWorkMultiple.Multiple = overrideMult;

											// BEGIN MID ISSUE # 3045 stodd
											priorQty = basisAllocProfile.GetStoreQtyAllocated(basisColorComponent.ColorRID, basisSizeCodeRid, sp.Key);
											// END MID ISSUE # 3045 stodd

											if (_basisComponent.ComponentType == eComponentType.SpecificPack)
												priorQty = priorQty * basisHeaderWorkMultiple.Multiple;

											int remainder = priorQty % currHeaderWorkMultiple.Multiple;
											if (remainder > 0)
											{
												throw new MIDException(eErrorLevel.severe,
													(int)(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule),
													MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule));
											}
											else
											{
												if (_targetComponent.ComponentType == eComponentType.SpecificPack)
													qty = priorQty / currHeaderWorkMultiple.Multiple;
												else
													qty = priorQty;
											}
										
											// int max = _sizeNeedResults.GetStoreMax(sp.Key, (int)sizeList[j]); // MID Track 3492 Size Need with constraint not allocating correctly
                                            //int max = _sizeNeedResults.GetStoreMax(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action  // TT#1543 - Jellis - Size Mult Broken
                                            int max = _sizeNeedResults.GetStoreInventoryMax(sp.Key, sizeCodeProfile.Key, true); // TT#1543 - JEllis - Size Mult Broken // TT#1176 - MD - Jellis - Size Need not observing Inventory Min Max on Group
                                            if (qty > max)  // Test for greater than MAX
											{
												// TODO 
												// put some kind of message to the log
												// IF (and that’ a big “if”) we put any message out, it should go to the audit log;  
												// in that case, there should only be one message that basically says exact 
												// rejected because of MAX constraint (if we identify the stores, 
												// they should be in a list within this one message).
											}
											else
											{	// Generic Size Curve - change below is just a fix because of cast error
												//_sizeNeedResults.AddAllocatedUnits(sp.Key, (int)sizeList[j], qty);
												_sizeNeedResults.AddAllocatedUnits(sp.Key, (int)((SizeCodeProfile)sizeList[j]).Key, qty); // MID Track 4283 Invalid Cast
											}
										}
									}
								}
								break;
							}
							case (eBasisSizeMethodRuleType.Fill):
							{
								//aRuleType = eRuleType.Fill;

								AllocationWorkMultiple basisHeaderWorkMultiple = GetQtyPerPack(basisColorComponent, basisAllocProfile);
								AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(targetColorComponent, allocProfile);

								//**************
								// Sort stores
								//**************
								int basisSizeCodeRid = Include.NoRID;
								sizeCnt = sizeList.Count;
								for (int j=0;j<sizeCnt;j++)
								{
									SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; // MID Track 3492 Size Need with constraint not allocating correctly
									//=============================
									// Size Substitution
									//=============================
                                    // begin MID Track 3492 Size Need with constraint not allocating correctly
									//if (_sizeSubstituteHash.ContainsKey((int)sizeList[j]))
									//	basisSizeCodeRid = (int)_sizeSubstituteHash[(int)sizeList[j]];
									//else
									//	basisSizeCodeRid = (int)sizeList[j];
									if (_sizeSubstituteHash.ContainsKey(sizeCodeProfile.Key))
									{
										basisSizeCodeRid = (int)_sizeSubstituteHash[sizeCodeProfile.Key];
									}
									else
									{
										basisSizeCodeRid = sizeCodeProfile.Key;
									}
									// endMID Track 3492 Size Need with constraint not allocating correctly

									ArrayList summandList = new ArrayList();
									storeCnt = aStoreList.Count;
									for (int i=0;i<storeCnt;i++)
									{
										StoreProfile sp = (StoreProfile)aStoreList[i];
										
										//if (!IsStoreExcluded(sp.Key, targetColorComponent.ColorRID , (int)sizeList[j])) // MID Track 3492 Size Need with constraint not allocating correctly
                                        if (!IsStoreExcluded(sp.Key, targetColorComponent.ColorRID, sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action
										{
											Summand aSummand = new Summand();
											aSummand.Eligible = true;  //all are eligible, else they won't be here...
											aSummand.Item = sp.Key;

											//=============================================
											// override mult if found
											//=============================================
											//int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, (int)sizeList[j]); // MID Track 3492 Size Need with constraint not allocating correctly
                                            int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action
											if (overrideMult > 0)
												currHeaderWorkMultiple.Multiple = overrideMult;

											// BEGIN MID ISSUE # 3045 stodd
											priorQty = basisAllocProfile.GetStoreQtyAllocated(basisColorComponent.ColorRID, basisSizeCodeRid, sp.Key);
											// END MID ISSUE # 3045 stodd

											if (basisColorComponent.ComponentType == eComponentType.SpecificPack)
												priorQty = priorQty * basisHeaderWorkMultiple.Multiple;

											int remainder = priorQty % currHeaderWorkMultiple.Multiple;
											if (remainder > 0)
											{
												throw new MIDException(eErrorLevel.severe,
													(int)(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule),
													MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule));
											}
											else
											{
												if (targetColorComponent.ComponentType == eComponentType.SpecificPack)
													qty = priorQty / currHeaderWorkMultiple.Multiple;
												else
													qty = priorQty;
											}
											Debug.WriteLine(sizeList[j].ToString() + " " + sp.StoreId + " " + qty.ToString());

											//int max = _sizeNeedResults.GetStoreMax(sp.Key, (int)sizeList[j]); // MID Track 3492 Size Need with constraint not allocating correctly
                                            //int max = _sizeNeedResults.GetStoreMax(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action // TT#1543 - JEllis - Size Mult Broken
                                            int max = _sizeNeedResults.GetStoreInventoryMax(sp.Key, sizeCodeProfile.Key, true); // TT#1543 - JEllis - Size Mult Broken // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
                                            if (qty > max)
												aSummand.Quantity = (double)max;
											else
												aSummand.Quantity = (double)qty;
											summandList.Add(aSummand);
										}
									}
									//if (this.SortDirection == eSortDirection.Descending)
									summandList.Sort(new SummandDescendingComparer());
									//else
									//	summandList.Sort(new SummandAscendingComparer());

									//*****************************
									// Get total units to allocate
									//*****************************
									// begin MID Track 3430 Proportional Allocation Wrong when reserve already allocated
									//int totalUnits = allocProfile.GetRuleUnitsToAllocate(targetColorComponent.ColorRID, (int)sizeList[j]);
									//int remainingUnits = totalUnits;
                                    //int remainingUnits = allocProfile.GetAllocatedBalance(targetColorComponent.ColorRID, (int)sizeList[j]); // MID Track 3492 Size Need with constraint not allocating correctly
                                    int remainingUnits = allocProfile.GetAllocatedBalance(targetColorComponent.ColorRID, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraint not allocating correctly
									if (remainingUnits < 0)
									{
										remainingUnits = 0;
									}
									// end MID Track 3430 Proportional Allocation Wrong when reserve already allocated

									//************************
									// Allocate to each store
									//************************
									foreach (Summand aSummand  in summandList)
									{
										if (aSummand.Quantity < remainingUnits)
										{
											//_sizeNeedResults.AddAllocatedUnits(aSummand.Item, (int)sizeList[j], (int)aSummand.Quantity); // MID Track 3492 Size Need with constraint not allocating correctly
											_sizeNeedResults.AddAllocatedUnits(aSummand.Item, sizeCodeProfile.Key, (int)aSummand.Quantity); // MID Track 3492 Size Need with constraint not allocating correctly
											remainingUnits -= (int)aSummand.Quantity;
										}
										else
										{
											//_sizeNeedResults.AddAllocatedUnits(aSummand.Item, (int)sizeList[j], remainingUnits); // MID Track 3492 Size Need with constraint not allocating correctly
											_sizeNeedResults.AddAllocatedUnits(aSummand.Item, sizeCodeProfile.Key, remainingUnits); // MID Track 3492 Size Need with constraint not allocating correctly
											remainingUnits = 0;
										}
									}
								}
								break;
							}
							case (eBasisSizeMethodRuleType.Exclude):
							{
								storeCnt = aStoreList.Count;
								for (int i=0;i<storeCnt;i++)
								{
									StoreProfile sp = (StoreProfile)aStoreList[i];

									sizeCnt = sizeList.Count;
									for (int j=0;j<sizeCnt;j++)
									{
										SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; // MID Track 3492 Size Need with constraint not allocating correctly
										//_sizeNeedResults.AddAllocatedUnits(sp.Key, (int)sizeList[j], 0); // MID Track 3492 Size Need with constraint not allocating correctly
										_sizeNeedResults.AddAllocatedUnits(sp.Key, sizeCodeProfile.Key, 0); // MID Track 3492 Size Need with constraint not allocating correctly
									}	
								}
								break;
							}
							case (eBasisSizeMethodRuleType.ProportionalAllocated):
							{	
								// Begin MID Issue 2520 stodd
//								int colorTotal = allocProfile.GetQtyAllocated(targetColorComponent);
//								int storeTotal = 0;
//								storeCnt = aStoreList.Count;
//								for (int i=0;i<storeCnt;i++)
//								{
//									StoreProfile sp = (StoreProfile)aStoreList[i];
//									storeTotal += allocProfile.GetStoreQtyAllocated(targetColorComponent.ColorRID, sp.Key);
//								}
								if (_distributeChange == eDistributeChange.ToNone)
								{
									ProportionalBalanceToColor(			
										allocProfile, 
										basisAllocProfile,
										basisColorComponent, 
										targetColorComponent, 
										aStoreList,
										sizeList );

									string errorMessage = "Header [" + allocProfile.HeaderID + "] Basis Size Proportional balanced to color."; // MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
									_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, errorMessage, this.GetType().Name);

								}
								else
								{
									ProportionalBalanceToSizes(			
										allocProfile, 
										basisAllocProfile,
										basisColorComponent, 
										targetColorComponent, 
										aStoreList,
										sizeList );
									string errorMessage = "Header [" + allocProfile.HeaderID + "] Basis Size Proportional balanced to Size.";  // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
									_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, errorMessage, this.GetType().Name);

								}
								// End MID Issue 2520 stodd
								break;
							}
							case (eBasisSizeMethodRuleType.AbsoluteQuantity):
								storeCnt = aStoreList.Count;
								for (int i=0;i<storeCnt;i++)
								{
									StoreProfile sp = (StoreProfile)aStoreList[i];
									sizeCnt = sizeList.Count;
									for (int j=0;j<sizeCnt;j++)
									{
										SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; // MID Track 3492 Size Need with constraint not allocating correctly
										//if (!IsStoreExcluded(sp.Key, targetColorComponent.ColorRID , (int)sizeList[j])) // MID Track 3492 Size Need with constraint not allocating correctly
                                        if (!IsStoreExcluded(sp.Key, targetColorComponent.ColorRID, sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraint not allocating correctly // TT#1391 - TMW New Action
										{
											qty = (int)aRuleQuantity;
											//if (qty <= _allocProfile.GetStoreCapacityMaximum(sp.Key)
											//	&& qty <= _allocProfile.GetStorePrimaryMaximum(_targetComponent, sp.Key))
											//{
											//_sizeNeedResults.AddAllocatedUnits(sp.Key, (int)sizeList[j], qty); // MID Track 3492 Size need with constraint not allocating correctly
											_sizeNeedResults.AddAllocatedUnits(sp.Key, sizeCodeProfile.Key, qty); // MID Track 3492 Size need with constraint not allocating correctly
											//}
										}
									}
								}
								break;
							default:
							{
								string errorMessage = 	string.Format
									(MIDText.GetText(eMIDTextCode.msg_InvalidRule),
									aRuleType.ToString());

								//					SAB.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.severe,
								//							(int)(eMIDTextCode.msg_InvalidRule),
								//							errorMessage));

								//===========================================================================================================
								// WHY ARE WE THROWING AND CATCHING RIGHT AFTER?
								// The Exception is missing critical info until it's gets thrown.  Once thrown (and caught) we can then add
								// it to the Audit log.
								//===========================================================================================================
								try
								{
									throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_InvalidRule),
										errorMessage);
								}
								catch
								{
									_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
									throw;
								}
							}
						}
					}
					catch (Exception ex)
					{
						_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.GetType().Name);
						throw;
					}

					endTime = System.DateTime.Now;
					Debug.WriteLine("Basis Need Calc -- " + System.Convert.ToString(endTime.Subtract(beginTime), CultureInfo.CurrentUICulture));

					return _sizeNeedResults;
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_SizeNeedAlgorithmFailed,
						MIDText.GetText(eMIDTextCode.msg_al_SizeNeedAlgorithmFailed));
				}

			}
			catch (Exception ex)
			{
				_aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_SizeNeedAlgorithmFailed, "Header [" + allocProfile.HeaderID + "] " + ex.Message, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in ausit
				throw;
			}
		}

		


		// Begin MID Issue 2520 stodd
		private void ProportionalBalanceToSizes(			
			AllocationProfile allocProfile, 
			AllocationProfile basisAllocProfile,
			AllocationColorOrSizeComponent basisColorComponent, 
			AllocationColorOrSizeComponent targetColorComponent, 
			ProfileList aStoreList,
			ArrayList sizeList )
		{
			AllocationWorkMultiple basisHeaderWorkMultiple = GetQtyPerPack(basisColorComponent, basisAllocProfile);
			AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(targetColorComponent, allocProfile);

			ArrayList summandList = new ArrayList();
			int sizeCnt = sizeList.Count;
			int storeCnt = aStoreList.Count;
			int qty = 0;
			int basisSizeCodeRid = 0;
			for (int j=0;j<sizeCnt;j++)
			{
				SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; // MID Track 3492 Size Need with constraint not allocating correctly
				//=============================
				// Size Substitution
				//=============================
				//if (_sizeSubstituteHash.ContainsKey((int)sizeList[j])) // MID Track 3492 Size Need with constraint not allocating correctly
				if (_sizeSubstituteHash.ContainsKey(sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraint not allocating correctly
					basisSizeCodeRid = (int)_sizeSubstituteHash[sizeCodeProfile.Key]; // MID Track 3492 Size Need with constraint not allocating correctly
				else
					basisSizeCodeRid = sizeCodeProfile.Key; // MID Track 3492 Size Need with constraints not allocating correctly

				summandList.Clear();
				for (int i=0;i<storeCnt;i++)
				{
					StoreProfile sp = (StoreProfile)aStoreList[i];
					//=============================================
					// override mult if found
					//=============================================
					//int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, (int)sizeList[j]); // MID Track 3492 Size Need with constraints not allocating correctly
                    int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
					if (overrideMult > 0)
						currHeaderWorkMultiple.Multiple = overrideMult;

					Summand aSummand = new Summand();
					//if (IsStoreExcluded(sp.Key, targetColorComponent.ColorRID , (int)sizeList[j])) // MID Track 3492 Size Need with constraints not allocating correctly
                    if (IsStoreExcluded(sp.Key, targetColorComponent.ColorRID, sizeCodeProfile.Key)) // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
						aSummand.Eligible = false;
					else
						aSummand.Eligible = true;
					aSummand.Item = sp.Key;
					// BEGIN MID ISSUE # 3045 stodd
					qty = basisAllocProfile.GetStoreQtyAllocated(basisColorComponent.ColorRID, basisSizeCodeRid, sp.Key);
					// END MID ISSUE # 3045 stodd
					aSummand.Quantity = (double)qty;
					//aSummand.Min = _sizeNeedResults.GetStoreMin(sp.Key, (int)sizeList[j]);	// MID Track 3492 Size Need with constraints not allocating correctly	 
					//aSummand.Max = _sizeNeedResults.GetStoreMax(sp.Key, (int)sizeList[j]);  // MID Track 3492 Size Need with constraints not allocating correctly
                    // begin TT#1543 - JEllis - Size Mult Broken
                    //aSummand.Min = _sizeNeedResults.GetStoreMin(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                    //aSummand.Max = _sizeNeedResults.GetStoreMax(sp.Key, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                    aSummand.Min = _sizeNeedResults.GetStoreInventoryMin(sp.Key, sizeCodeProfile.Key, true); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
                    aSummand.Max = _sizeNeedResults.GetStoreInventoryMax(sp.Key, sizeCodeProfile.Key, true); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
                    // end TT#1543 - JEllis - Size Mult Broken
                    summandList.Add(aSummand);
				}

				ProportionalSpread aSpread = new ProportionalSpread(this._aSAB);
				aSpread.SummandList = summandList;
				//						aSpread.RequestedTotal = allocHeader.GetAllocatedBalance(_ruleAllocProfile.Component);
				// begin MID Track 3430 Proportional Allocation wrong when units already allocated
				//aSpread.RequestedTotal = allocProfile.GetRuleUnitsToAllocate(targetColorComponent.ColorRID, (int)sizeList[j]);
				// aSpread.RequestedTotal = allocProfile.GetAllocatedBalance(targetColorComponent.ColorRID, (int)sizeList[j]); // MID Track 3492 Size Need with constraints not allocating correctly
				aSpread.RequestedTotal = allocProfile.GetAllocatedBalance(targetColorComponent.ColorRID, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly
				// end MID Track 3430 Proportional Allocation wrong when units already allocated
				aSpread.Precision = 0;

				// If the current componenet is a pack, then the Requested Total is already in packs (not units)
				// so the multiple becomes 1.
				if (targetColorComponent.ComponentType != eComponentType.SpecificPack)
					aSpread.Multiple = currHeaderWorkMultiple.Multiple;
				aSpread.Calculate();
								
				foreach (Summand aSummand  in summandList)
				{
					//_sizeNeedResults.AddAllocatedUnits(aSummand.Item, (int)sizeList[j], (int)aSummand.Result); // MID Track 3492 Size Need with constraints not allocating correctly
					_sizeNeedResults.AddAllocatedUnits(aSummand.Item, sizeCodeProfile.Key, (int)aSummand.Result); // MID Track 3492 Size Need with constraints not allocating correctly
				}
			}
		}


		private void ProportionalBalanceToColor(AllocationProfile allocProfile, 
			AllocationProfile basisAllocProfile,
			AllocationColorOrSizeComponent basisColorComponent, 
			AllocationColorOrSizeComponent targetColorComponent, 
			ProfileList aStoreList,
			ArrayList sizeList )
		{
			AllocationWorkMultiple basisHeaderWorkMultiple = GetQtyPerPack(basisColorComponent, basisAllocProfile);
			AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(targetColorComponent, allocProfile);

			ArrayList summandList = new ArrayList();
			int sizeCnt = sizeList.Count;
			int storeCnt = aStoreList.Count;
			int qty = 0;
			int basisSizeCodeRid = Include.NoRID;
			for (int i=0;i<storeCnt;i++)
			{
				StoreProfile sp = (StoreProfile)aStoreList[i];
				int sglRid = (int)RulesStoreGroupLevelHash[sp.Key];

				summandList.Clear();
				for (int j=0;j<sizeCnt;j++)
				{
					//=============================
					// Size Substitution
					//=============================
                    // begin MID Track 3492 Size Need with constraints not allocating correctly
					SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)sizeList[j]; 
					//if (_sizeSubstituteHash.ContainsKey((int)sizeList[j])) 
					//	basisSizeCodeRid = (int)_sizeSubstituteHash[(int)sizeList[j]];
					//else
					//	basisSizeCodeRid = (int)sizeList[j];
					if (_sizeSubstituteHash.ContainsKey(sizeCodeProfile.Key))
						basisSizeCodeRid = (int)_sizeSubstituteHash[sizeCodeProfile.Key];
					else
						basisSizeCodeRid = sizeCodeProfile.Key;
					// end MID Track 3492 Size Need with constraints not allocating correctly

					//=============================================
					// override mult if found
					//=============================================
					//int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, (int)sizeList[j]);  // MID Track 3492 Size Need with constraints not allocating correctly
                    int overrideMult = _sizeNeedResults.GetStoreMult(sp.Key, sizeCodeProfile.Key);     // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action

					if (overrideMult > 0)
						currHeaderWorkMultiple.Multiple = overrideMult;

					Summand aSummand = new Summand();
					//if (IsStoreExcluded(sp.Key, targetColorComponent.ColorRID , (int)sizeList[j])) // MID Track 3492 Size Need with constraints not allocating correctly
                    if (IsStoreExcluded(sp.Key, targetColorComponent.ColorRID, sizeCodeProfile.Key)) // MID Track 3492 Size Need with Constraints not allocating correctly// TT#1391 - TMW New Action 
						aSummand.Eligible = false;
					else
						aSummand.Eligible = true;
					//aSummand.Item = (int)sizeList[j];  // MID Track 3492 Size Need with constraints not allocating correctly
					aSummand.Item = sizeCodeProfile.Key; // MID Track 3492 Size Need with Constraints not allocating correctly
					// BEGIN MID ISSUE # 3045 stodd
					qty = basisAllocProfile.GetStoreQtyAllocated(basisColorComponent.ColorRID, basisSizeCodeRid, sp.Key);
					// END MID ISSUE # 3045 stodd
					aSummand.Quantity = (double)qty;
					// begin MID Track 3492 Size Need with constraints not allocating correctly
					//aSummand.Min = _sizeNeedResults.GetStoreMin(sp.Key, (int)sizeList[j]);		 
					//aSummand.Max = _sizeNeedResults.GetStoreMax(sp.Key, (int)sizeList[j]);
                    // begin TT#1543 - JEllis - Size Mult Broken
                    //aSummand.Min = _sizeNeedResults.GetStoreMin(sp.Key, sizeCodeProfile.Key); // TT#1391 - TMW New Action
                    //aSummand.Max = _sizeNeedResults.GetStoreMax(sp.Key, sizeCodeProfile.Key); // TT#1391 - TMW New Action
                    aSummand.Min = _sizeNeedResults.GetStoreInventoryMin(sp.Key, sizeCodeProfile.Key, true); // TT#1391 - TMW New Action // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
                    aSummand.Max = _sizeNeedResults.GetStoreInventoryMax(sp.Key, sizeCodeProfile.Key, true); // TT#1391 - TMW New Action // TT#1176 - MD - Jellis - Size Need Not observing Inv Min Max on Group
                    // end TT#1543 - JEllis - Size Mult Broken
                    // end MID Track 3492 Size Need with constraints not allocating correctly

					summandList.Add(aSummand);
				}

				ProportionalSpread aSpread = new ProportionalSpread(this._aSAB);
				aSpread.SummandList = summandList;
				aSpread.RequestedTotal = allocProfile.GetStoreQtyAllocated(targetColorComponent.ColorRID, sp.Key);
				aSpread.Precision = 0;

				// If the current componenet is a pack, then the Requested Total is already in packs (not units)
				// so the multiple becomes 1.
				if (targetColorComponent.ComponentType != eComponentType.SpecificPack)
					aSpread.Multiple = currHeaderWorkMultiple.Multiple;
				aSpread.Calculate();
								
				foreach (Summand aSummand  in summandList)
				{
					_sizeNeedResults.AddAllocatedUnits(sp.Key, aSummand.Item, (int)aSummand.Result);
				}
			}
		}
		// End MID Issue 2520 stodd

		private AllocationWorkMultiple GetQtyPerPack(GeneralComponent aComponent, AllocationProfile aAllocProfile)
		{
			AllocationWorkMultiple awm = new AllocationWorkMultiple(1,0);

			if (aComponent.ComponentType == eComponentType.SpecificPack)
			{
				AllocationPackComponent apc = (AllocationPackComponent)aComponent;
				awm.Multiple = aAllocProfile.GetPackMultiple(apc.PackName);
				awm.Minimum = awm.Multiple;
			}
			else if (aComponent.ComponentType == eComponentType.Total 
				|| aComponent.ComponentType == eComponentType.Bulk
				|| aComponent.ComponentType == eComponentType.DetailType)
			{
				switch (aComponent.ComponentType)
				{
					case (eComponentType.Total):
						awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Total);
						break;
					case (eComponentType.Bulk):
						awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Bulk);
						break;
					case (eComponentType.DetailType):
						awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.DetailType);
						break;
				}
			}
			
			return awm;
		}
        // END J.Ellis Change


		override public void Update(TransactionData td)
		{
			if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodData = new MethodBasisSizeData(td);
			}


			_methodData.SizeGroupRid = SizeGroupRid;  
			_methodData.SizeCurveGroupRid = SizeCurveGroupRid;       
			_methodData.BasisClrRid = BasisClrRid;
			_methodData.BasisHdrRid = BasisHdrRid;
			_methodData.StoreFilterRid = StoreFilterRid;
			_methodData.Rule = Rule;
			_methodData.RuleQuantity = RuleQuantity;
			_methodData.HeaderComponent = _HeaderComponent;
            _methodData.IncludeReserveInd = _includeReserve; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
			_methodData.SizeConstraintRid = this.SizeConstraintRid;
			_methodData.SubstituteList = this._substituteList;
			// begin Generic Size Curve data
			_methodData.GenCurveHcgRID = base.GenCurveCharGroupRID;
			_methodData.GenCurveHnRID = base.GenCurveHnRID;
			_methodData.GenCurvePhRID = base.GenCurvePhRID;
			_methodData.GenCurvePhlSequence = base.GenCurvePhlSequence;
			_methodData.GenCurveColorInd = base.GenCurveColorInd;
			_methodData.GenCurveMerchType = base.GenCurveMerchType;
			// end Generic Size Curve data
			
			// begin Generic Size Constraint data
			_methodData.GenConstraintHcgRID = base.GenConstraintCharGroupRID;
			_methodData.GenConstraintHnRID = base.GenConstraintHnRID;
			_methodData.GenConstraintPhRID = base.GenConstraintPhRID;
			_methodData.GenConstraintPhlSequence = base.GenConstraintPhlSequence;
			_methodData.GenConstraintColorInd = base.GenConstraintColorInd;
			_methodData.GenConstraintMerchType = base.GenConstraintMerchType;
            //_methodData.IncludeReserve = _includeReserve; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
			// end Generic Size Constraint data

            // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            _methodData.UseDefaultCurve = base.UseDefaultCurve;
            // End TT#413
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            _methodData.ApplyRulesOnly = base.ApplyRulesOnly;
            // end TT#2155 - Jellis - Fill Size Holes Null Reference
            // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            _methodData.GenCurveNsccdRID = base.GenCurveNsccdRID;
            // End TT#413

			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_methodData.InsertMethod(base.Key, td);
						InsertUpdateMethodRules(td);
						break;
					case eChangeType.update:
						base.Update(td);
						_methodData.UpdateMethod(base.Key, td);
						InsertUpdateMethodRules(td);
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

		#endregion

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
			BasisSizeAllocationMethod newBasisSizeAllocationMethod = null;

			try
			{
				newBasisSizeAllocationMethod = (BasisSizeAllocationMethod)this.MemberwiseClone();
				newBasisSizeAllocationMethod.BasisClrRid = BasisClrRid;
				newBasisSizeAllocationMethod.BasisHdrRid = BasisHdrRid;
				newBasisSizeAllocationMethod.ConstraintsLoaded = ConstraintsLoaded;
				newBasisSizeAllocationMethod.GenCurveCharGroupRID = GenCurveCharGroupRID;
				newBasisSizeAllocationMethod.GenCurveColorInd = GenCurveColorInd;
				newBasisSizeAllocationMethod.GenCurveHash = GenCurveHash;
				newBasisSizeAllocationMethod.GenCurveHnRID = GenCurveHnRID;
				newBasisSizeAllocationMethod.GenCurveMerchType = GenCurveMerchType;
				newBasisSizeAllocationMethod.GenCurvePhlSequence = GenCurvePhlSequence;
				newBasisSizeAllocationMethod.GenCurvePhRID = GenCurvePhRID;
				newBasisSizeAllocationMethod.GetDimensionsUsing = GetDimensionsUsing;
				newBasisSizeAllocationMethod.GetSizes = GetSizes;
				newBasisSizeAllocationMethod.GetSizesUsing = GetSizesUsing;
				newBasisSizeAllocationMethod.HeaderComponent = HeaderComponent;
				newBasisSizeAllocationMethod.Method_Change_Type = eChangeType.none;
				newBasisSizeAllocationMethod.Method_Description = Method_Description;
				newBasisSizeAllocationMethod.MethodConstraints = MethodConstraints;
				newBasisSizeAllocationMethod.MethodStatus = MethodStatus;
                newBasisSizeAllocationMethod.IncludeReserve = _includeReserve; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
				newBasisSizeAllocationMethod.Name = Name;
				newBasisSizeAllocationMethod.PromptAttributeChange = PromptAttributeChange;
				newBasisSizeAllocationMethod.PromptSizeChange = PromptSizeChange;
				newBasisSizeAllocationMethod.Rule = Rule;
				newBasisSizeAllocationMethod.RuleQuantity = RuleQuantity;
				newBasisSizeAllocationMethod.SG_RID = SG_RID;
				newBasisSizeAllocationMethod.SizeConstraintRid = SizeConstraintRid;
				newBasisSizeAllocationMethod.SizeCurveGroupRid = SizeCurveGroupRid;
				newBasisSizeAllocationMethod.SMBD = SMBD;
				newBasisSizeAllocationMethod.StoreFilterRid = StoreFilterRid;
				newBasisSizeAllocationMethod.User_RID = User_RID;
				newBasisSizeAllocationMethod.Virtual_IND = Virtual_IND;
                newBasisSizeAllocationMethod.Template_IND = Template_IND;
                // begin Generic Size Constraint data
                newBasisSizeAllocationMethod.GenConstraintCharGroupRID = GenConstraintCharGroupRID;
				newBasisSizeAllocationMethod.GenConstraintColorInd = GenConstraintColorInd;
				newBasisSizeAllocationMethod.GenConstraintHash = GenConstraintHash;
				newBasisSizeAllocationMethod.GenConstraintHnRID = GenConstraintHnRID;
				newBasisSizeAllocationMethod.GenConstraintMerchType = GenConstraintMerchType;
				newBasisSizeAllocationMethod.GenConstraintPhlSequence = GenConstraintPhlSequence;
				newBasisSizeAllocationMethod.GenConstraintPhRID = GenConstraintPhRID;
				// end Generic Size Constraint data
				newBasisSizeAllocationMethod.SubstituteList = new ArrayList();
				foreach (BasisSizeSubstitute sizeSub in SubstituteList)
				{
					newBasisSizeAllocationMethod.SubstituteList.Add(new BasisSizeSubstitute(sizeSub.SizeTypeRid, sizeSub.SubstituteRid, sizeSub.SizeType));
				}

				newBasisSizeAllocationMethod.CreateConstraintData();
				newBasisSizeAllocationMethod.BuildRulesDecoder(SG_RID);

				return newBasisSizeAllocationMethod;
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

        // BEGIN RO-642 - RDewey

        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalBasisSize);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserBasisSize);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            //RO-3886 Data Transport for Basis Size Method
            //throw new NotImplementedException("MethodGetData is not implemented");
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>();
            ROMethodBasisSizeSubstituteSet roBasisSizeSubstituteSet = new ROMethodBasisSizeSubstituteSet();
            ROMethodBasisSizeProperties method = new ROMethodBasisSizeProperties(
                method: GetName.GetMethod(method: this),
                description: _methodData.Method_Description,
                userKey: User_RID,
                filter: GetName.GetFilterName(_methodData.StoreFilterRid),
                sizeGroup: GetName.GetSizeGroup(_methodData.SizeGroupRid),
                rOSizeCurveProperties: SizeCurveProperties.BuildSizeCurveProperties(
                    sizeCurveGroupRID: _methodData.SizeCurveGroupRid,
                    genCurveNsccdRID: _methodData.GenCurveNsccdRID,
                    genCurveHcgRID: _methodData.GenCurveHcgRID,
                    genCurveHnRID: _methodData.GenCurveHnRID,
                    genCurvePhRID: _methodData.GenCurvePhRID,
                    genCurvePhlSequence: _methodData.GenCurvePhlSequence,
                    genCurveMerchType: _methodData.GenCurveMerchType,
                    isUseDefault: _methodData.UseDefaultCurve,
                    isApplyRulesOnly: _methodData.ApplyRulesOnly,
                    isColorSelected: _methodData.GenConstraintColorInd,
                    sizeCurve: keyValuePair,
                    sizeCurveGenericHierarchy: keyValuePair,
                    sizeCurveGenericNameExtension: keyValuePair,
                    sizeCurveGenericHeaderCharacteristic: keyValuePair,
                    SAB: SAB
                    ),
                rOSizeConstraintProperties: SizeConstraintProperties.BuildSizeConstraintProperties(
                    inventoryBasisMerchHnRID: -1,   //inventory basis not used for this method
                    inventoryBasisMerchPhRID: -1,
                    inventoryBasisMerchPhlSequence: 0,
                    inventoryBasisMerchType: 0,
                    sizeConstraintRID: _methodData.SizeConstraintRid,
                    genConstraintHcgRID: _methodData.GenConstraintHcgRID,
                    genConstraintHnRID: _methodData.GenConstraintHnRID,
                    genConstraintPhRID: _methodData.GenConstraintPhRID,
                    genConstraintPhlSequence: _methodData.GenConstraintPhlSequence,
                    genConstraintMerchType: _methodData.GenConstraintMerchType,
                    genConstraintColorInd: _methodData.GenConstraintColorInd,
                    inventoryBasis: keyValuePair,
                    sizeConstraint: keyValuePair,
                    sizeConstraintGenericHierarchy: keyValuePair,
                    sizeConstraintGenericHeaderChar: keyValuePair,
                    SAB: SAB
                    ),
                header:GetName.GetHeader(_methodData.BasisHdrRid, SAB),
                includeReserve: _methodData.IncludeReserveInd, 
                colorComponent: GetName.GetColorOrSizeComponent(_methodData.HeaderComponent),
                color: GetName.GetColor(_methodData.BasisClrRid, SAB),
                rule: GetName.GetBasisSizeMethodRuleType(_methodData.Rule),
                ruleQuantity: _methodData.RuleQuantity,
                attribute: GetName.GetAttributeName(_methodData.SG_RID),
                sizeRuleAttributeSet: SizeRuleAttributeSet.BuildSizeRuleAttributeSet(
                    methodRID: _methodData.Method_RID,
                    methodType: eMethodType.BasisSizeAllocation,
                    attributeRID: _methodData.SG_RID,
                    sizeGroupRID: _methodData.SizeGroupRid,
                    sizeCurveGroupRID: _methodData.SizeCurveGroupRid,
                    getSizesUsing: GetSizesUsing,
                    getDimensionsUsing: GetDimensionsUsing,
                    methodConstraints: MethodConstraints,
                    SAB: SAB
                    ),
                basisSizeSubstituteSet: BasisSizeSubstituteSet.BuildBasisSizeSubstituteSet(
                    methodRID: _methodData.Method_RID,
                    methodType: eMethodType.BasisSizeAllocation,
                    attributeRID: _methodData.SG_RID,
                    sizeGroupRID: _methodData.SizeGroupRid,
                    sizeCurveGroupRID: _methodData.SizeCurveGroupRid,
                    getSizesUsing: GetSizesUsing,
                    getDimensionsUsing: GetDimensionsUsing,
                    substituteList: _methodData.SubstituteList,
                    SAB: SAB
                    ),
                isTemplate: Template_IND
            );

            return method;
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(ROOverrideLowLevel overrideLowLevel, out bool successful, ref string message)
        {
            successful = true;

            throw new NotImplementedException("MethodGetOverrideModelList is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            //RO-3886 Data Transport for Size Basis Method
            ROMethodBasisSizeProperties roMethodBasisSizeAllocationProperties = (ROMethodBasisSizeProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                Method_Description = roMethodBasisSizeAllocationProperties.Description;
                User_RID = roMethodBasisSizeAllocationProperties.UserKey;
                _StoreFilterRid = roMethodBasisSizeAllocationProperties.Filter.Key;
                SizeGroupRid = roMethodBasisSizeAllocationProperties.SizeGroup.Key;
                //Size Curve Group Box 
                SizeCurveGroupRid = roMethodBasisSizeAllocationProperties.ROSizeCurveProperties.SizeCurveGroupKey;

                if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
                {
                    GenCurveNsccdRID = roMethodBasisSizeAllocationProperties.ROSizeCurveProperties.HeaderCharacteristicsOrNameExtensionKey;
                }
                GenCurveMerchType = roMethodBasisSizeAllocationProperties.ROSizeCurveProperties.MerchandiseType;
                switch (GenCurveMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenCurveHnRID = Include.NoRID;
                        break;
                    default: //eMerchandiseType.Node
                        GenCurveHnRID = Include.NoRID;
                        break;
                }

                UseDefaultCurve = roMethodBasisSizeAllocationProperties.ROSizeCurveProperties.IsUseDefault;
                ApplyRulesOnly = roMethodBasisSizeAllocationProperties.ROSizeCurveProperties.IsApplyRulesOnly;
                // Constraints Group Box
                // Note Inventory Basis is not used for this method               
                _sizeConstraintRid = roMethodBasisSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraint.Key;
                GenConstraintMerchType = roMethodBasisSizeAllocationProperties.ROSizeConstraintProperties.GenConstraintMerchType;
                switch (GenConstraintMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenConstraintHnRID = Include.NoRID;
                        break;
                    default: //eMerchandiseType.Node
                        GenConstraintHnRID = roMethodBasisSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHierarchy.Key;
                        break;
                }
                GenConstraintCharGroupRID = roMethodBasisSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHeaderChar.Key;
                GenConstraintColorInd = roMethodBasisSizeAllocationProperties.ROSizeConstraintProperties.GenConstraintColorInd;
                _methodData.HeaderComponent = roMethodBasisSizeAllocationProperties.ColorComponent.Key;
                _methodData.IncludeReserveInd = roMethodBasisSizeAllocationProperties.IncludeReserve;
                _methodData.BasisHdrRid = roMethodBasisSizeAllocationProperties.Header.Key;
                _methodData.BasisClrRid = roMethodBasisSizeAllocationProperties.Color.Key;
                _methodData.Rule = roMethodBasisSizeAllocationProperties.Rule.Key;
                _methodData.RuleQuantity = roMethodBasisSizeAllocationProperties.RuleQuantity;
                //Rules Tab
                SG_RID = roMethodBasisSizeAllocationProperties.Attribute.Key;
                MethodConstraints = SizeRuleAttributeSet.BuildMethodConstrainst(roMethodBasisSizeAllocationProperties.Method.Key, roMethodBasisSizeAllocationProperties.Attribute.Key,
                roMethodBasisSizeAllocationProperties.SizeRuleAttributeSet, MethodConstraints, SAB); // MethodConstraints will be regenerated based on above changes
                _methodData.SubstituteList = BasisSizeSubstituteSet.BuildBasisSizeSubstituteList(roMethodBasisSizeAllocationProperties.Method.Key, roMethodBasisSizeAllocationProperties.Attribute.Key, roMethodBasisSizeAllocationProperties.BasisSizeSubstituteSet);
                return true;
            }
            catch (Exception e)
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
