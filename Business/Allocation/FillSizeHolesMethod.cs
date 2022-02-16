using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;  //TT#1636-MD -jsobek -Pre-Pack Fill Size 
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{



	/// <summary>
	/// Defines the fill size holes allocation method.
	/// </summary>
	/// <remarks>
	/// </remarks>
	//public class FillSizeHolesMethod : AllocationSizeBaseMethod //AllocationBaseMethod  //TT#1636-MD -jsobek -Pre-Pack Fill Size
	public class FillSizeHolesMethod : AllocationSizeBaseMethod //TT#1636-MD -jsobek -Pre-Pack Fill Size
	{

		#region "Member Variables"
		//			private int _unitsAvailable;                
		//			private int _sizeGroupRID;               
		//			private int _sizeCurveGroupRID;
		//			private AllocationMerchBin _OTSPlan;
		//			private bool _useEquateSizes;
		//			private Hashtable _fillHolesBin;

		private MethodFillSizeHolesData _methodData;
		private bool _PercentInd;
		private double _Available;
		private int	_MerchHnRid;
		private int	_MerchPhRid;
		private int	_MerchPhlSequence;
		private int _StoreFilterRid;
		private eMerchandiseType _MerchandiseType;
		private SessionAddressBlock _aSAB;
		private bool _promptAttChange;
		private bool _promptSzChange;
		private bool _processCancelled = false;
		private int _componentsProcessed;
		private int _reserveStoreRid = Include.NoRID;
		private int _sizeAlternateRid;
		private int _sizeConstraintRid;
//		private bool _genCurveError = false;	// A&F Generic Size Curve 
        private bool _overrideVSWSizeConstraints; // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
		private ArrayList _actionAuditList;     // MID track 4967 Size Functions nnot showing total quantity
		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		private eFillSizesToType _fillSizesToType;
		// End MID Track #4921
		// BEGIN TT#41-MD - GTaylor - UC#2
        private eMerchandiseType _IB_MerchandiseType;
        //private char _inventory_Ind;
        private int _IB_MERCH_HN_RID;
        private int _IB_MERCH_PH_RID;
        private int _IB_MERCH_PHL_SEQUENCE;
        // END TT#41-MD - GTaylor - UC#2
        private eVSWSizeConstraints _vSWSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        private double _AvgPackDeviationTolerance;
        private double _MaxPackNeedTolerance;
        private bool _overrideAvgPackDevTolerance;  // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
        private bool _overrideMaxPackNeedTolerance; // End TT#356
        private bool _packToleranceNoMaxStep;     // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        private bool _packToleranceStepped;       // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //End TT#1636-MD -jsobek -Pre-Pack Fill Size
		#endregion

		#region "Properties"

		/// <summary>
		/// Get Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get	{return eProfileType.MethodFillSizeHolesAllocation;}
		}

		public int StoreFilterRid
		{
			get	{return _StoreFilterRid;}
			set	{_StoreFilterRid = value;}
		}

		public double Available
		{
			get	{return _Available;}
			set	{_Available = value;}
		}

		public bool PercentInd
		{
			get	{return _PercentInd;}
			set	{_PercentInd = value;}
		}

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        public bool OverrideVSWSizeConstraints
        {
            get { return _overrideVSWSizeConstraints; }
            set { _overrideVSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

		public int MerchHnRid
		{
			get	{return _MerchHnRid;}
			set {_MerchHnRid = value;}
		}


		public int MerchPhRid
		{
			get {return _MerchPhRid;}
			set {_MerchPhRid = value;}
		}


		public int MerchPhlSequence
		{
			get {return _MerchPhlSequence;}
			set {_MerchPhlSequence = value;}
		}


		public eMerchandiseType MerchandiseType
		{
			get	{return _MerchandiseType;}
			set{_MerchandiseType = value;}
		}

		public bool PromptAttributeChange
		{
			get {return _promptAttChange;}
			set {_promptAttChange = value;}
		}

		public bool PromptSizeChange
		{
			get {return _promptSzChange;}
			set {_promptSzChange = value;}
		}

		public int SizeConstraintRid
		{
			get {return _sizeConstraintRid;}
			set {_sizeConstraintRid = value;}
		}

		public int SizeAlternateRid
		{
			get {return _sizeAlternateRid;}
			set {_sizeAlternateRid = value;}
		}

		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		public eFillSizesToType FillSizesToType
		{
			get	{ return _fillSizesToType;}
			set	{ _fillSizesToType = value;	}
		}
		// End MID Track #4921
        // BEGIN TT#41-MD - GTaylor - UC#2
        public eMerchandiseType IB_MerchandiseType
        {
            get { return _IB_MerchandiseType; }
            set { _IB_MerchandiseType = value;}
        }
        //public char Inventory_Ind
        //{
        //    get { return _inventory_Ind; }
        //    set { _inventory_Ind = value; }
        //}
        public int IB_MERCH_HN_RID
        {
            get { return _IB_MERCH_HN_RID; }
            set { _IB_MERCH_HN_RID = value; }
        }
        public int IB_MERCH_PH_RID
        {
            get { return _IB_MERCH_PH_RID; }
            set { _IB_MERCH_PH_RID = value; }
        }
        public int IB_MERCH_PHL_SEQ
        {
            get { return _IB_MERCH_PHL_SEQUENCE; }
            set { _IB_MERCH_PHL_SEQUENCE = value; }
        }
        // END TT#41-MD - GTaylor - UC#2

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set { _vSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        /// <summary>
        /// Gets or sets the Maximum Pack Allocation Need Tolerance.
        /// </summary>
        public double MaxPackNeedTolerance
        {
            get { return _MaxPackNeedTolerance; }
            set { _MaxPackNeedTolerance = value; }
        }

        /// <summary>
        /// Gets or sets the Average Pack Deviation Tolerance.
        /// </summary>
        public double AvgPackDeviationTolerance
        {
            get { return _AvgPackDeviationTolerance; }
            set { _AvgPackDeviationTolerance = value; }
        }

        public bool OverrideAvgPackDevTolerance
        {
            get { return _overrideAvgPackDevTolerance; }
            set { _overrideAvgPackDevTolerance = value; }
        }

        public bool OverrideMaxPackNeedTolerance
        {
            get { return _overrideMaxPackNeedTolerance; }
            set { _overrideMaxPackNeedTolerance = value; }
        }
        public bool PackToleranceNoMaxStep
        {
            get { return _packToleranceNoMaxStep; }
            set { _packToleranceNoMaxStep = value; }
        }
        public bool PackToleranceStepped
        {
            get { return _packToleranceStepped; }
            set { _packToleranceStepped = value; }
        }
        //End TT#1636-MD -jsobek -Pre-Pack Fill Size

		#endregion

		#region "Constructors"

			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//public FillSizeHolesMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.FillSizeHolesAllocation)
			public FillSizeHolesMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.FillSizeHolesAllocation, eProfileType.MethodFillSizeHolesAllocation)
			//End TT#523 - JScott - Duplicate folder when new folder added
			{
				_aSAB = aSAB;

				if (base.MethodType != eMethodType.FillSizeHolesAllocation)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_NotFillSizeHolesMethod),
						MIDText.GetText(eMIDTextCode.msg_NotFillSizeHolesMethod));
				}

				if (base.Filled)
				{
					#region METHOD VALUES
					_methodData = new MethodFillSizeHolesData(base.Key,eChangeType.populate);
					Available = _methodData.Available;
					MerchandiseType = _methodData.MerchandiseType;
					MerchHnRid = _methodData.MerchHnRid;
					MerchPhlSequence = _methodData.MerchPhlSequence;
					MerchPhRid = _methodData.MerchPhRid;
					PercentInd = _methodData.PercentInd;
					base.SizeGroupRid = _methodData.SizeGroupRid;
					base.SizeCurveGroupRid = _methodData.SizeCurveGroupRid;
					StoreFilterRid = _methodData.StoreFilterRid;
					this.SizeAlternateRid = _methodData.SizeAlternateRid;
					this.SizeConstraintRid = _methodData.SizeConstraintRid;
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
					// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
					base.NormalizeSizeCurvesDefaultIsOverridden = _methodData.NormalizeSizeCurvesDefaultIsOverridden;
					base.NormalizeSizeCurves = _methodData.NormalizeSizeCurves;
					// END MID Track #4826
					// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
					if (_methodData.FillSizesToTypeIsSet)
					{
						this.FillSizesToType = _methodData.FillSizesToType;
					}
					else
					{
						this.FillSizesToType = GlobalOptions.FillSizesToType;
					}
					// END MID Track #4921

                    // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                    base.UseDefaultCurve = _methodData.UseDefaultCurve;
                    // End TT#413
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    base.ApplyRulesOnly = _methodData.ApplyRulesOnly;
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
                    // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                    base.GenCurveNsccdRID = _methodData.GenCurveNsccdRID;
                    // End TT#413
                    // BEGIN TT#41-MD - GTaylor - UC#2
                    IB_MERCH_HN_RID = _methodData.IB_MERCH_HN_RID;
                    IB_MERCH_PH_RID = _methodData.IB_MERCH_PH_RID;
                    //Inventory_Ind = _methodData.Inventory_Ind;
                    IB_MERCH_PHL_SEQ = _methodData.IB_MERCH_PHL_SEQ;
                    IB_MerchandiseType = _methodData.IB_MerchandiseType;
                    // END TT#41-MD - GTaylor - UC#2
                    // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                    this.OverrideVSWSizeConstraints = _methodData.OverrideVSWSizeConstraints;
                    if (this.OverrideVSWSizeConstraints)
                    {
                        this._vSWSizeConstraints = _methodData.VSWSizeConstraints;
                    }
                    else
                    {
                        this._vSWSizeConstraints = aSAB.ApplicationServerSession.GlobalOptions.VSWSizeConstraints;     //Get from global options
                    }
                    // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                    this.OverrideAvgPackDevTolerance = _methodData.OverrideAvgPackDevTolerance;
                    if (this.OverrideAvgPackDevTolerance)
                    {
                        this._AvgPackDeviationTolerance = _methodData.AvgPackDeviationTolerance;
                    }
                    else
                    {
                        this._AvgPackDeviationTolerance = aSAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent; //Get from global options
                    }
                    this.OverrideMaxPackNeedTolerance = _methodData.OverrideMaxPackNeedTolerance;
                    if (this.OverrideMaxPackNeedTolerance)
                    {
                        this._MaxPackNeedTolerance = _methodData.MaxPackNeedTolerance;
                        this._packToleranceNoMaxStep = _methodData.PackToleranceNoMaxStep; // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        this._packToleranceStepped = _methodData.PackToleranceStepped;     // TT#1365 - JEllis - Fl Detail Pack Size Need Enhancement
                    }
                    else
                    {
                        this._MaxPackNeedTolerance = aSAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;      //Get from global options
                        this._packToleranceNoMaxStep = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep; // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        this._packToleranceStepped = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;     // TT#1365 - Jellis - FL Detail Pack Size Need Enhancement
                    }
                    //End TT#1636-MD -jsobek -Pre-Pack Fill Size
					#endregion
				}
				else
				{
					#region DEFAULT VALUES
					_methodData = new MethodFillSizeHolesData();
					Available = aSAB.ApplicationServerSession.GlobalOptions.FillSizeHolesPercent;
					MerchandiseType = eMerchandiseType.OTSPlanLevel;
					MerchHnRid = Include.NoRID;
					MerchPhlSequence = 0;
					MerchPhRid = Include.NoRID;
					PercentInd = true;
					base.SizeGroupRid = Include.NoRID;
					base.SizeCurveGroupRid = Include.NoRID;
					StoreFilterRid = Include.NoRID;
					this.SizeAlternateRid = Include.NoRID;
					this.SizeConstraintRid = Include.NoRID;
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
					// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
					base.NormalizeSizeCurvesDefaultIsOverridden = false;
					base.NormalizeSizeCurves = true;
					// END MID Track #4826
					// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
					this.FillSizesToType = GlobalOptions.FillSizesToType;
					// END MID Track #4921
                    // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                    base.UseDefaultCurve = false;
                    // End TT#413
                    // Begin TT#413 - RMAtelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                    base.GenCurveNsccdRID = Include.NoRID;
                    // End TT#413
                    // BEGIN TT#41-MD - GTaylor - UC#2
                    IB_MERCH_HN_RID = Include.NoRID;
                    IB_MERCH_PH_RID = Include.NoRID;
                    //Inventory_Ind = Convert.ToChar(0);
                    IB_MERCH_PHL_SEQ = Include.NoRID;
                    // BEGIN TT#41-MD -- AGallagher - UC#2
                    //IB_MerchandiseType = eMerchandiseType.OTSPlanLevel;
                    IB_MerchandiseType = eMerchandiseType.Undefined;
                    // END TT#41-MD -- AGallagher - UC#2
                    // END TT#41-MD - GTaylor - UC#2
                    // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                    this.OverrideVSWSizeConstraints = _methodData.OverrideVSWSizeConstraints;
                    this._vSWSizeConstraints = aSAB.ApplicationServerSession.GlobalOptions.VSWSizeConstraints;     //Get from global options
                    // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                    this._AvgPackDeviationTolerance = aSAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent; //Get from global options
                    this._MaxPackNeedTolerance = aSAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;
                    _packToleranceNoMaxStep = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep; // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                    _packToleranceStepped = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;     // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                    //End TT#1636-MD -jsobek -Pre-Pack Fill Size

					#endregion
				}
				
				SMBD = _methodData;
				_promptAttChange = true;
				_promptSzChange = true;

				 
				base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
				base.GetDimensionsUsing  = eGetDimensions.SizeCurveGroupRID;
				// begin MID Track 3781 Size Curve not required
				if (base.SizeGroupRid != Include.NoRID)
				{
					base.GetSizesUsing = eGetSizes.SizeGroupRID;
					base.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
				}
				// end MID Track 3781 Size Curve not required

				CreateConstraintData();
	}

	#endregion

		#region "Override Methods"

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (CheckSizeMethodForUserData())
            {
                return true;
            }
            
            if (IsHierarchyNodeUser(MerchHnRid))
            {
                return true;
            }

            if (IsFilterUser(StoreFilterRid))
            {
                return true;
            }

            if (IsHierarchyNodeUser(IB_MERCH_HN_RID))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
		//public override void ProcessMethod(
		//	ApplicationSessionTransaction aApplicationTransaction, 
		//	int aStoreFilter, Profile methodProfile)
		public override void ProcessMethod(ApplicationSessionTransaction _transaction, int aStoreFilter, Profile methodProfile)
		//End TT#1636-MD -jsobek -Pre-Pack Fill Size
		{
			// BEGIN Track #5727 stodd\
			try
			{
				//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
				//aApplicationTransaction.ResetAllocationActionStatus();
				_transaction.ResetAllocationActionStatus();
				//End TT#1636-MD -jsobek -Pre-Pack Fill Size
				//_reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes run under Workflow ignores Reserve Store
				
				//				if (!base.OkToProcess(aApplicationTransaction))	// Generic Size Curve
				//				{
				//					return;
				//				}

				//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
				//ArrayList selectedComponentList = aApplicationTransaction.GetSelectedComponentList();
				ArrayList selectedComponentList = _transaction.GetSelectedComponentList();
				//foreach (AllocationProfile ap2 in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
				foreach (AllocationProfile ap2 in (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation))
				//End TT#1636-MD -jsobek -Pre-Pack Fill Size
				{

					try
					{
						//if (!base.OkToProcess(aApplicationTransaction, ap2))	// Generic Size Curve  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						if (!base.OkToProcess(_transaction, ap2))	// Generic Size Curve  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						{
							//aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							_transaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							continue;
						}
					}
					catch (MIDException ex)
					{
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Header [" + ap2.HeaderID + "] " + ex.ErrorMessage, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
						//aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						_transaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						continue;
					}
					catch (Exception ex)
					{
						SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "Header [" + ap2.HeaderID + "] " + ex.Message, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
						//aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed); //TT#1636-MD -jsobek -Pre-Pack Fill Size
						_transaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						continue;
					}

					// Allocation Component (Pack, BulkColor...)
					if (selectedComponentList.Count > 0)
					{
						//AllocationProfileList apl = (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
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
								//aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
								_transaction.GlobalOptions.BalanceTolerancePercent,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
								aStoreFilter,
								-1);
							this.ProcessAction(
								//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
								//aApplicationTransaction.SAB,
								//aApplicationTransaction,
								_transaction.SAB,
								_transaction,
								//End TT#1636-MD -jsobek -Pre-Pack Fill Size
								awfs,
								ap,
								true,
								aStoreFilter);

							if (_processCancelled)
								break;
						}

						//=============
						// Set status
						//=============
						// BEGIN MID Track # 2983 Size Need not being executed in workflow	
						//=============
						// Set status
						//=============
						// if (!_processCancelled)
						// {
						//	ap.BulkSizeBreakoutPerformed = true;
						//	ap.WriteHeader();
						//	aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						// }
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
							//new GeneralComponent(eGeneralComponentType.AllColors),  //TT#1636-MD -jsobek
							new GeneralComponent(eGeneralComponentType.Total), //TT#1636-MD -jsobek 
							false,
							true,
							//aApplicationTransaction.GlobalOptions.BalanceTolerancePercent,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							_transaction.GlobalOptions.BalanceTolerancePercent,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							aStoreFilter,
							-1);
						this.ProcessAction(
						    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
							//aApplicationTransaction.SAB,
							//aApplicationTransaction,
							_transaction.SAB,
							_transaction,
							//End TT#1636-MD -jsobek -Pre-Pack Fill Size
							awfs,
							ap,
							true,
							aStoreFilter);

						//=============
						// Set status
						//=============
						// BEGIN MID Track # 2983 Size Need not being executed in workflow	
						//=============
						// Set status
						//=============
						// if (!_processCancelled)
						// {
						//	ap.BulkSizeBreakoutPerformed = true;
						//	ap.WriteHeader();
						//	aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						// }
						// END MID Track # 2983 Size Need not being executed in workflow	
					}
				}
			}
			catch (Exception err)
			{
				SAB.ApplicationServerSession.Audit.Log_Exception(err);
			}
			// END Track #5727 stodd
		}


			/// <summary>
			/// processes a single action/work flow step
			/// </summary>
			/// <param name="aSAB"></param>
			/// <param name="aApplicationTransaction"></param>  //TT#1636-MD -jsobek -Pre-Pack Fill Size
			/// <param name="_transaction"></param>  //TT#1636-MD -jsobek -Pre-Pack Fill Size
			/// <param name="aApplicationWorkFlowStep"></param>
			/// <param name="aAllocationProfile"></param>
			/// <param name="WriteToDB"></param>
			/// <param name="aStoreFilterRID"></param>
			public override void ProcessAction(
				SessionAddressBlock aSAB, 
				//ApplicationSessionTransaction aApplicationTransaction,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
				ApplicationSessionTransaction _transaction,  //TT#1636-MD -jsobek -Pre-Pack Fill Size
				ApplicationWorkFlowStep aApplicationWorkFlowStep, 
				Profile aAllocationProfile,
				bool WriteToDB,
				int aStoreFilterRID)
			{
                // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
                //AllocationProfile ap = (AllocationProfile) aAllocationProfile;
                Audit audit = SAB.ApplicationServerSession.Audit;
				//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
				//AllocationProfile ap = aAllocationProfile as AllocationProfile;
                //if (ap == null)
                AllocationProfile _allocationProfile = aAllocationProfile as AllocationProfile;
                if (_allocationProfile == null)
				//End TT#1636-MD -jsobek -Pre-Pack Fill Size
                {
                    string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                    audit.Add_Msg(
                        eMIDMessageLevel.Severe,
                        eMIDTextCode.msg_NotAllocationProfile,
                        auditMsg,
                        this.GetType().Name);
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_NotAllocationProfile),
                        MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
                }
                // end TT#421 Detail packs/bulk not allocated by Size Need Method.
                this._actionAuditList = new ArrayList();  // MID Track 4967 Size Functions Not Showing Total Qty
                try
                {
                    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
					//ap.ResetTempLocks(false); // TT#421 Detail packs/bulk not allocated by Size Need Method.
					//_reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes run under Workflow ignores Reserve Store
					_allocationProfile.ResetTempLocks(false); // TT#421 Detail packs/bulk not allocated by Size Need Method.
                    _reserveStoreRid = _transaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes run under Workflow ignores Reserve Store
					//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                    //Audit audit = SAB.ApplicationServerSession.Audit; // MID Track #3011 Do not process headers with status rec'd out bal // TT#421 Detail packs/bulk not allocated by Size Need Method.
                    if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aApplicationWorkFlowStep._method.MethodType))
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
                     	//if (!base.OkToProcess(aApplicationTransaction, ap))	// Generic Size Curve   //TT#1636-MD -jsobek -Pre-Pack Fill Size
						if (!base.OkToProcess(_transaction, _allocationProfile))	// Generic Size Curve  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        {
                            //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);  // MID Track 5183 Workflow should fail on severe error  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed);  // MID Track 5183 Workflow should fail on severe error  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            return;
                        }
                    }
                    catch (MIDException)
                    {
                        //aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						_transaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        //							_genCurveError = true;
                        throw;
                    }
                    //					}
                    // END A&F Generic Size Curve

                    // begin TT#421 Detail packs/bulk not allocated by size need method
                    //if (eProfileType.Allocation != aAllocationProfile.ProfileType)
                    //{
                    //   // begin MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
                    //    //try
                    //    //{
                    //    //	throw new MIDException(eErrorLevel.severe,(int)(eMIDTextCode.msg_NotAllocationProfile),
                    //    //		MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
                    //    //}
                    //    //catch
                    //    //{
                    //    //	audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile), this.ToString());
                    //    //	throw;
                    //    //}
                    //    string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                    //    audit.Add_Msg(
                    //        eMIDMessageLevel.Severe,
                    //        eMIDTextCode.msg_NotAllocationProfile,
                    //        auditMsg,
                    //        this.GetType().Name);
                    //    throw new MIDException(
                    //        eErrorLevel.severe,
                    //        (int)eMIDTextCode.msg_NotAllocationProfile,
                    //        auditMsg);
                    //    // end MID Track 5778 - Scheduler 'Run Now' feature gets error in audit
                    //}
                    // end TT#421 Detail packs/bulk not allocated by size need method

                    // BEGIN MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
                    //if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)  //TT#1636-MD -jsobek -Pre-Pack Fill Size
					if (_allocationProfile.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    {
                        string msg = string.Format(
                            audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), MIDText.GetTextOnly((int)_allocationProfile.HeaderAllocationStatus));
                        audit.Add_Msg(
                            eMIDMessageLevel.Warning, eMIDTextCode.msg_HeaderStatusDisallowsAction,
							//(this.Name + " " + ap.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            (this.Name + " " + _allocationProfile.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            this.GetType().Name);
						//aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size	
                        _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.NoActionPerformed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    }
                        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    //else if (ap.BeginDay != Include.UndefinedDate  //TT#1636-MD -jsobek -Pre-Pack Fill Size
					else if (_allocationProfile.BeginDay != Include.UndefinedDate  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        && this.SizeCurveGroupRid == Include.NoRID
                        && !base.GenericSizeCurveDefined())
                    {
                        string msg = string.Format(
                            //audit.GetText(eMIDTextCode.msg_al_FillSizeMthdInvalidWhenBeginDateAndNoCurves, false), ap.HeaderID, Name);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							audit.GetText(eMIDTextCode.msg_al_FillSizeMthdInvalidWhenBeginDateAndNoCurves, false), _allocationProfile.HeaderID, Name);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        audit.Add_Msg(
                            eMIDMessageLevel.Warning, eMIDTextCode.msg_al_FillSizeMthdInvalidWhenBeginDateAndNoCurves,
                            (msg),
                            this.GetType().Name);
                        //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
						_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.NoActionPerformed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    }
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
                    else
                    {
                        // END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
                        // AllocationProfile ap = (AllocationProfile) aAllocationProfile; // MID Track # 2938 Size Need not performed in workflow

                        ProfileList tempStoreFilterList = null;
                        //*******************************
                        // Apply STORE FILTER if present
                        //*******************************
                        // BEGIN Issue 5727 stodd
                        bool outdatedFilter = false;
                        string msg = string.Empty;
                        // END Issue 5727
                        // Begin TT354 - stodd - Filter in Fill Size Method works when processed as stand alone; but does not work when placed into a workflow
                        if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)  // try the one sent in through the method
                        // End TT354 - stodd - Filter in Fill Size Method works when processed as stand alone; but does not work when placed into a workflow
                        {
                            // BEGIN Issue 5727 stodd
                            FilterData storeFilterData = new FilterData();
                            string filterName = storeFilterData.FilterGetName(aStoreFilterRID);
                            //tempStoreFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(ap, aStoreFilterRID, ref outdatedFilter);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							tempStoreFilterList = _transaction.GetAllocationFilteredStoreList(_allocationProfile, aStoreFilterRID, ref outdatedFilter);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            if (outdatedFilter)
                            {
                                msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                                msg = msg.Replace("{0}", filterName);
								//string suffix = ". Method " + this.Name + ". Header [" + ap.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                string suffix = ". Method " + this.Name + ". Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                string auditMsg = msg + suffix;
                                // begin MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                                //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.GetType().Name);
                                // end MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                            }
                            // END Issue 5727
                            msg = audit.GetText(eMIDTextCode.msg_al_StoreFilterAppliedToMethod, false);
                            msg = msg.Replace("{0}", filterName);
                            msg = msg.Replace("{1}", "Fill Size Holes Method");
                            msg = msg.Replace("{2}", this.Name);
							//msg = msg.Replace("{3}", ap.HeaderID);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            msg = msg.Replace("{3}", _allocationProfile.HeaderID);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            audit.Add_Msg(
                                eMIDMessageLevel.Information, eMIDTextCode.msg_al_StoreFilterAppliedToMethod,
                                msg, this.GetType().Name);
                            // END Issue 4632
                        }
                        else if (this.StoreFilterRid != Include.NoRID)  // try the one ON this method
                        {
                            FilterData storeFilterData = new FilterData();
                            string filterName = storeFilterData.FilterGetName(StoreFilterRid);
							//tempStoreFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(ap, StoreFilterRid, ref outdatedFilter);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            tempStoreFilterList = _transaction.GetAllocationFilteredStoreList(_allocationProfile, StoreFilterRid, ref outdatedFilter);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            if (outdatedFilter)
                            {
                                msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                                msg = msg.Replace("{0}", filterName);
                                //string suffix = ". Method " + this.Name + ". Header [" + ap.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
								string suffix = ". Method " + this.Name + ". Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                string auditMsg = msg + suffix;
                                // begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
                                //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.GetType().Name);
                                // end MID Track 5778 Schedule 'Run Now' feature gets error in audit
                                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                            }
                        }
                        else
                        {
                            //tempStoreFilterList = aApplicationTransaction.GetMasterProfileList(eProfileType.Store);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							tempStoreFilterList = _transaction.GetMasterProfileList(eProfileType.Store);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        }
                        //===================================================
                        // Apply Store Eligibility and exclude Reserve Store
                        //===================================================
                        ProfileList storeFilterList = new ProfileList(eProfileType.Store);
                        int storeCnt = tempStoreFilterList.Count;
                        for (int i = 0; i < storeCnt; i++)
                        {
                            StoreProfile sp = (StoreProfile)tempStoreFilterList[i];
                            //if (!_allocProfile.GetStoreOut(colorComponent, sp.Key)
							//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
							//if (ap.GetStoreIsEligible(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
                            //    && ap.GetIncludeStoreInAllocation(sp.Key)) // TT#1401 - JEllis - Urban Reservation Store pt 11
                            if (_allocationProfile.GetStoreIsEligible(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
                                && _allocationProfile.GetIncludeStoreInAllocation(sp.Key)) // TT#1401 - JEllis - Urban Reservation Store pt 11
                            //End TT#1636-MD -jsobek -Pre-Pack Fill Size
							{
                                // don't alloc to reserve store either
                                // otherwise add store to list
                                if (sp.Key != _reserveStoreRid)
                                    storeFilterList.Add(sp);
                            }
                        }

                        //==============================
                        // Get componet to process
                        //==============================
                        AllocationWorkFlowStep aAllocationWorkFlowStep = (AllocationWorkFlowStep)aApplicationWorkFlowStep;
                        GeneralComponent aComponent = aAllocationWorkFlowStep.Component;

                        int totalPackUnitsAllocated  = 0;  //TT#1636-MD -jsobek -Pre-Pack Fill Size

                        switch (aComponent.ComponentType)
                        {
                            case (eComponentType.SpecificColor):
                            case (eComponentType.DetailType): //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            case (eComponentType.AllNonGenericPacks): //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                //ProcessComponent(aSAB, aApplicationTransaction, (AllocationProfile)aAllocationProfile, aComponent, storeFilterList);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
								ProcessComponent(aSAB, _transaction, _allocationProfile, aComponent, storeFilterList, ref totalPackUnitsAllocated);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                // begin TT#1018 - MD - Jellis - Over ALlocated
                                if (aComponent.ComponentType != eComponentType.SpecificColor)
                                {
                                    PackProcessing.AdjustDetailTotals(_allocationProfile);
                                }
                                // end TT#1018 - MD - JEllis - Over Allocated
                                break;
                            case (eComponentType.SpecificPack):
                            case (eComponentType.SpecificSize):
                            //case (eComponentType.Bulk): //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            //case (eComponentType.DetailType): //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            case (eComponentType.GenericType):
                            case (eComponentType.AllPacks):
                            case (eComponentType.AllGenericPacks):
                            //case (eComponentType.AllNonGenericPacks): //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            case (eComponentType.AllSizes):
                                {
                                    // not valid
                                    try
                                    {
                                        throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_ColorInstructionComponentMismatch),
                                            MIDText.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch));
                                    }
                                    catch
                                    {
                                        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch), this.ToString());
                                        throw;
                                    }
                                }
                            case (eComponentType.Total):
								//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                                {
                                    if (_allocationProfile.NonGenericPackCount > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                    {
                                        ProcessComponent(aSAB, _transaction, _allocationProfile, new GeneralComponent(eGeneralComponentType.DetailType), storeFilterList, ref totalPackUnitsAllocated); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                        // BEGIN MID Track 3122 Bulk not allocated when bulk and sized packs on same header
                                        //if (_allocationProfile.BulkIsDetail && _allocationProfile.BulkColors.Count > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                        //{
                                        //    PackProcessing.AllocateBulkAfterPacks(_allocationProfile, _transaction, storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                        //}
                                        // END MID Track 3122 Bulk not allocated when bulk and sized packs on same header

                                    }

                                    if (_allocationProfile.BulkColors.Count > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                    {
										// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                                        ProcessBulk(aSAB, _transaction, _allocationProfile, storeFilterList, ref totalPackUnitsAllocated, true);  // TT#702 Size Need Method "hangs" when processing hdrs with begin date
										// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                                    }
                                    PackProcessing.AdjustDetailTotals(_allocationProfile);  // TT#1018 - MD - Jellis - Over Allocated




                                    break;
                                }
								//End TT#1636-MD -jsobek -Pre-Pack Fill Size
                            case (eComponentType.AllColors):
                            case (eComponentType.Bulk): 
							    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                                {
                                    //Hashtable colors = ap.BulkColors;
                                    //if (colors != null)
                                    //{
                                    //    foreach (HdrColorBin hcb in colors.Values)
                                    //    {
                                    //        AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
                                    //        ProcessComponent(aSAB, aApplicationTransaction, (AllocationProfile)aAllocationProfile, aColor, storeFilterList);
                                    //    }
                                    //}
                                    ProcessBulk(aSAB, _transaction, _allocationProfile, storeFilterList, ref totalPackUnitsAllocated);  
									//End TT#1636-MD -jsobek -Pre-Pack Fill Size
                                    break;
                                }
                            case (eComponentType.ColorAndSize):
                                {
                                    //=============================================================================================
                                    // many color/size components can be selected.
                                    // Since we are only processing the color, we only want to go through this logic once.
                                    // if the user cancels, it's not a problem.
                                    // but if the user processes the color, we want to stop after processing the color only once.
                                    //=============================================================================================
                                    if (_componentsProcessed == 0)
                                    {
                                        string errMsg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeComponentSelectionInvalid));

                                        System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                                            errMsg,
                                            "Invalid Header Component Selection",
                                            System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);

                                        if (diagResult == System.Windows.Forms.DialogResult.OK)
                                        {
                                            AllocationColorSizeComponent colorSizeComponent = (AllocationColorSizeComponent)aComponent;

                                            AllocationColorOrSizeComponent aColor = (AllocationColorOrSizeComponent)colorSizeComponent.ColorComponent;
                                            //ProcessComponent(aSAB, aApplicationTransaction, (AllocationProfile)aAllocationProfile, aColor, storeFilterList);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
											ProcessComponent(aSAB, _transaction, _allocationProfile, aColor, storeFilterList, ref totalPackUnitsAllocated);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                        }
                                        else
                                        {
                                            errMsg += "  Cancel was chosen.";
                                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errMsg, this.ToString());
                                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                            //SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Cancelled, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                            _processCancelled = true;
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    //  begin MID Track 5778 Schedule 'Run Now' feature gets error in audit
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
                                    string auditMsg =
                                        //MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent) + " Header [" + ((AllocationProfile)aAllocationProfile).HeaderID + "]";  //TT#1636-MD -jsobek -Pre-Pack Fill Size
										MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent) + " Header [" + (_allocationProfile).HeaderID + "]";  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                    SAB.ApplicationServerSession.Audit.Add_Msg(
                                        eMIDMessageLevel.Severe,
                                        eMIDTextCode.msg_UnknownAllocationComponent,
                                        auditMsg,
                                        this.GetType().Name);
                                    throw new MIDException(
                                        eErrorLevel.severe,
                                        (int)eMIDTextCode.msg_UnknownAllocationComponent,
                                        auditMsg);
                                    // end MID Track 5778 Schedule 'Run Now' feature gets error in audit 
                                }

                        }
                        // BEGIN MID Track # 2983 Size Need not performed in workflow
                        if (!_processCancelled)
                        {

                            //// begin MID track 4967 Size Functions Not Showing Total Qty
                            //this._actionAuditList.Add(new AllocationActionAuditStruct
                            //    ((int)eAllocationMethodType.FillSizeHolesAllocation,
                            //    _allocationProfile,     // TT#1199 - MD - NEED allocation # of stores is zero - rbeck
                            //    aComponent,
                            //    _allocationProfile.GetQtyAllocated(aComponent),
                            //    _allocationProfile.GetCountOfStoresWithAllocation(aComponent),
                            //    true,
                            //    true));
                            //// end MID track 4967 Size Functions Not Showing Total Qty



                            if (WriteToDB)
                            {
                                //ap.WriteHeader();  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
								_allocationProfile.WriteHeader();  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            }
                            //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
                            msg = msg.Replace("{0}", "Fill Size Holes");
                            msg = msg.Replace("{1}", this.Name);
                            //msg += " Header [" + ap.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            audit.Add_Msg(eMIDMessageLevel.Information, msg, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                            foreach (AllocationActionAuditStruct auditStruct in this._actionAuditList)
                            {
                                //Begin TT#1666-MD -jsobek -Invalid Cast Exception - Fill Size for Bulk Color

                                //_transaction.WriteAllocationAuditInfo(
                                //    _allocationProfile.Key,
                                //    0,
                                //    this.MethodType,
                                //    this.Key,
                                //    this.Name,
                                //    auditStruct.Component.ComponentType,
                                //    _transaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)auditStruct.Component).ColorRID).ColorCodeName,
                                //    null,
                                //    _allocationProfile.GetQtyAllocated(auditStruct.Component) - auditStruct.QtyAllocatedBeforeAction,
                                //    Math.Abs(_allocationProfile.GetCountOfStoresWithAllocation(auditStruct.Component) - auditStruct.StoresWithAllocationBeforeAction));

                                string colorCodeName = string.Empty;

                                if (auditStruct.Component.ComponentType == eComponentType.SpecificColor)
                                {
                                    colorCodeName = _transaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)auditStruct.Component).ColorRID).ColorCodeName;
                                }

                                _transaction.WriteAllocationAuditInfo(
                                    _allocationProfile.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    auditStruct.Component.ComponentType,
                                    colorCodeName,
                                    null,
                                    _allocationProfile.GetQtyAllocated(auditStruct.Component) - auditStruct.QtyAllocatedBeforeAction,
                                    Math.Abs(_allocationProfile.GetCountOfStoresWithAllocation(auditStruct.Component) - auditStruct.StoresWithAllocationBeforeAction));
                                //End TT#1666-MD -jsobek -Invalid Cast Exception - Fill Size for Bulk Color
                            }
                            // end MID track 4967 Size Functions Not Showing Total Qty
                        }
                        else
                        {
                            //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
							_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                            msg = msg.Replace("{0}", "Fill Size Holes");
                            msg = msg.Replace("{1}", this.Name);
							//msg += " Header [" + ap.HeaderID + "] "; // MID track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                        }
                        // END MID Track # 2983 Size Need not performed in workflow
                    }   // MID Track 3011 do not process headers with status rec'd out bal
                }
                catch (NoProcessingByFillSizeHoles)
                {
                    //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
					_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                    msg = msg.Replace("{0}", "Fill Size Holes");
                    msg = msg.Replace("{1}", this.Name);
					//msg += " Header [" + ap.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
                    msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                    throw;
                }
                catch (MIDException ex)
                {
                    //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
					_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ErrorMessage, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' gets error in audit
                    string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                    msg = msg.Replace("{0}", "Fill Size Holes");
                    msg = msg.Replace("{1}", this.Name);
					//msg += " Header [" + ap.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                    throw;
                }
                catch (Exception ex)
                {
                    //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow  
					_transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
                    string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                    msg = msg.Replace("{0}", "Fill Size Holes");
                    msg = msg.Replace("{1}", this.Name);
                    //msg += " Header [" + ap.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit   //TT#1636-MD -jsobek -Pre-Pack Fill Size  
					msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit  
                    throw;
                }
                    // begin TT#421 Detail packs/bulk not allocated by Size Need Method
                finally
                {
                    // ap.ResetTempLocks(true);  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
					_allocationProfile.ResetTempLocks(true);  //TT#1636-MD -jsobek -Pre-Pack Fill Size 
                }
                // end TT#421 Detail packs/bulk not allocated by Size Need Method
			}

			//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
            private void ProcessBulk(SessionAddressBlock aSAB,
                ApplicationSessionTransaction _transaction,
                AllocationProfile _allocationProfile, ProfileList aStoreFilterList, ref int totalPackUnitsAllocated, bool skipPackColors=false)	// TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
            {
                //===========================================================================================================
                // TT#1386-MD - stodd - NOTE: This entire method was hand merged between GA and pre-pack fill size changes
                //===========================================================================================================

				bool processGroupMembers = false;
            	AssortmentProfile assortmentProfile = null;
            	AllocationProfile[] allocationProfileList = null;

            	// process group members if colors on all headers are not on the first header
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in ProcessBulk()");
                }
                #endif	
            	//if (_allocationProfile is AssortmentProfile)
                if (_allocationProfile.isAssortmentProfile)
                // End TT#4988 - BVaughan - Performance
            	{
					// Begin TT#1728-MD - stodd - GA-Fill Size-Different results when running 2 headers in GA and same header individually
                	processGroupMembers = true;
					// End TT#1728-MD - stodd - GA-Fill Size-Different results when running 2 headers in GA and same header individually
            	}

				if (processGroupMembers)
            	{
                	assortmentProfile = (AssortmentProfile)_allocationProfile;
                	allocationProfileList = assortmentProfile.AssortmentMembers;
            	}
            	else
            	{
                	allocationProfileList = new AllocationProfile[1];
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in ProcessBulk()");
                    }
                    #endif		
                    //if (_allocationProfile.isAssortmentProfile)
                	if (_allocationProfile.isAssortmentProfile)
                    // End TT#4988 - BVaughan - Performance
                	{
                    	allocationProfileList[0] = (AllocationProfile)_allocationProfile;
                	}
                	else
                	{
                    	allocationProfileList[0] = _allocationProfile;
                	}
            	}

                if (allocationProfileList != null
                    && allocationProfileList.Length > 0)
                {
                    foreach (AllocationProfile allocationProfile in allocationProfileList)
                    {

                        SortedList colors = new SortedList(new HdrColorBinProcessOrder());
                        //foreach (HdrColorBin hcb in _allocationProfile.BulkColors.Values)
                        foreach (HdrColorBin hcb in allocationProfile.BulkColors.Values)
                        {
                            colors.Add(hcb, hcb);
                        }

                        if (colors.Count > 0)
                        {
                            foreach (HdrColorBin hcb in colors.Values)
                            {
                                // Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                                //if (skipPackColors && _allocationProfile.NonGenericPackCount > 0)
                                if (skipPackColors && allocationProfile.NonGenericPackCount > 0)
                                {
                                    bool skipColor = false;
                                    //foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough
                                    foreach (PackHdr ph in allocationProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough
                                    {
                                        foreach (PackColorSize pcs in ph.PackColors.Values)
                                        {
                                            if (hcb.ColorCodeRID == pcs.ColorCodeRID)
                                            {
                                                skipColor = true;
                                                break;
                                            }

                                            if (!skipColor)
                                            {
                                                AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
                                                //ProcessComponent(aSAB, _transaction, _allocationProfile, aColor, aStoreFilterList, ref totalPackUnitsAllocated);
                                                ProcessComponent(aSAB, _transaction, allocationProfile, aColor, aStoreFilterList, ref totalPackUnitsAllocated);
                                            }
                                            
                                        }
                                        // Begin TT#4932 - JSmith - Size Review Column Chooser options when packs are present
                                        // set the fill size holes information on bulk colors that are also on packs
                                        if (skipColor)
                                        {
                                            allocationProfile.SetSizeFillMethod(hcb.ColorCodeRID, this);
                                        }
                                        // End TT#4932 - JSmith - Size Review Column Chooser options when packs are present
                                    }

                                }
                                else
                                {
                                    AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
                                    //ProcessComponent(aSAB, _transaction, _allocationProfile, aColor, aStoreFilterList, ref totalPackUnitsAllocated);
                                    ProcessComponent(aSAB, _transaction, allocationProfile, aColor, aStoreFilterList, ref totalPackUnitsAllocated);
                                }
                                // End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                            }
                        }
                    }
                }
            }
			//End TT#1636-MD -jsobek -Pre-Pack Fill Size

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
				ProfileList storeList,
                ref int totalPackUnitsAllocated
                )
			{


				try
				{
					int genSizeCurveGroupRID; // A&F Generic Size Curve 
					_componentsProcessed++;
					AllocationProfile _allocationProfile = (AllocationProfile) aAllocationProfile;


               
					// begin A&F Generic Size Curve
					//SizeCurveGroupProfile sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
					SizeCurveGroupProfile sizeCurveGroup;
					if (base.GenericSizeCurveDefined())
					{
						try
						{
							genSizeCurveGroupRID = (int)base.GenCurveHash[_allocationProfile.Key];
							sizeCurveGroup = new SizeCurveGroupProfile(genSizeCurveGroupRID);
						}
						catch
						{
//							string errMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),string.Empty,ap.HeaderID);
							string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),string.Empty,_allocationProfile.HeaderID);
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_al_NoSizeCurveForHeader,errMessage);
						}
					}
					else
					{
						sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
					}
					// end A&F Generic Size Curve
					// begin MID Track 4372 Generic Size Constraint
					int sizeConstraintRID = this.SizeConstraintRid;
					if (base.GenericSizeConstraintsDefined())
					{
						try
						{
							sizeConstraintRID = (int)base.GenConstraintHash[_allocationProfile.Key];
						}
						catch
						{
							string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeConstraintForHeader),string.Empty, _allocationProfile.HeaderID);
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_al_NoSizeConstraintForHeader, errMessage);
						}
					}
					// end MID Track 4372 Generic Size Contraint

                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    HdrColorBin colorBin = null;
                    if (aComponent.ComponentType == eComponentType.DetailType || aComponent.ComponentType == eComponentType.AllNonGenericPacks)
                    {
                        //_allocationProfile.SetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType), this); // TT#702 Inifinite Loop when begin date set
                        //_allocationProfile.SetSizeFillMethod(Include.DummyColorRID, this);
                        _allocationProfile.SetSizeFillMethod(aComponent, this);
                    }
                    else
                    {
                        AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                        // begin MID Tracks 3738, 3811, 3827 Status Issues
                        colorBin = _allocationProfile.GetHdrColorBin(colorComponent.ColorRID);
                        if (colorBin.ColorSizes.Count < 1)
                        {
                            string msg = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ThereAreNoSizesInSelectedColor), this.SAB.HierarchyServerSession.GetColorCodeProfile(colorBin.ColorCodeRID).ColorCodeName);
                            throw new MIDException(eErrorLevel.warning, (int)(eMIDTextCode.msg_al_ThereAreNoSizesInSelectedColor), msg);
                        }
                        //_allocationProfile.SetSizeFillMethod(colorComponent.ColorRID, this);
                        _allocationProfile.SetSizeFillMethod(colorComponent, this);
                    }
                      //TT#1636-MD -jsobek -Pre-Pack Fill Size -Set for color and detail type
                    // end TT#2155 - JElllis - Fill Size Holes Null Reference

                    // begin TT#702 Infinite Loop when begin date set
                    SizeNeedAlgorithm sna =
                        new SizeNeedAlgorithm(
                            aApplicationTransaction,
                            //eMethodType.FillSizeHolesAllocation, // TT#2155 - JEllis - Fill Size Null Reference
                            _allocationProfile,
                            aComponent, //colorComponent,
                            RulesCollection,
                            this._sizeAlternateRid,
                            sizeConstraintRID,
                            sizeCurveGroup,
                            storeList,
                            this._MerchandiseType,
                            this._MerchHnRid,
                            this._MerchPhRid,
                            this._MerchPhlSequence,
                            this.SG_RID,
                            eSizeNeedColorPlan.PlanForSpecificColorOnly,
                            // begin TT#41 - MD - Jellis - Size Inventory Min Max
                            this.NormalizeSizeCurves,
                            this._IB_MerchandiseType,
                            this._IB_MERCH_HN_RID,
                            this._IB_MERCH_PH_RID,
                            this._IB_MERCH_PHL_SEQUENCE, // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
                            this._vSWSizeConstraints, // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
                            this.FillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
                            );   
                            //this.NormalizeSizeCurves);
                            // end TT#41 - MD - Jellis - Size Inventory Min Max
                    // end TT#702 Infinite Loop when begin date set
                    //aApplicationTransaction.SetSizeNeedResults(ap.HeaderRID, sna.SizeNeedResults);                  // TT#2155 - JEllis - Fill Size Holes Null Reference  // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations

					//===========================
					// call Fill Size Holes Processing
					//===========================
					int availableUnits = 0;
                    // begin MID Track 3430 Proportional Allocation Not correct when Reserve Store Allocated
					//int totalUnits = ap.GetRuleUnitsToAllocate(colorComponent.ColorRID); 


                    //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size -Guessing here -Process for color and detail type
					//int totalUnits = ap.GetAllocatedBalance(colorComponent);
                    int totalUnits; //= _allocationProfile.GetAllocatedBalance(aComponent);
                    if (_allocationProfile.WorkUpBulkSizeBuy || _allocationProfile.Placeholder)
                    {
                        availableUnits = int.MaxValue - 1;
                    }
                    else
                    {
                        if (aComponent.ComponentType == eComponentType.DetailType || aComponent.ComponentType == eComponentType.AllNonGenericPacks)
                        {
                            totalUnits = _allocationProfile.GetAllocatedBalance(aComponent);

                            if (totalUnits < 0)
                            {
                                totalUnits = 0;
                            }
                            // end MID Track 3430 Proportional Allocation Not correct when Reserve StoreAllocated
                            // Begin TT#4238 - JSmith - WUB Header - Fill Size Plan plus minimums is not allocating the Fill Forward Plan 
							//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
							//if (ap.WorkUpBulkSizeBuy || ap.Placeholder)
                    		//{
                       		//    availableUnits = int.MaxValue - 1;
                    		//}
                    		//else if (this.PercentInd)
							////if (this.PercentInd)
							//End TT#1636-MD -jsobek -Pre-Pack Fill Size
					        // End TT#4238 - JSmith - WUB Header - Fill Size Plan plus minimums is not allocating the Fill Forward Plan 
                            if (this.PercentInd)
                            {
                                availableUnits = (int)((totalUnits * Available) / 100);
                            }
                            else
                            {
                                if (totalUnits < Available)
                                    availableUnits = totalUnits;
                                else
                                    availableUnits = (int)Available;
                            }
						//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size	
                        }
                        else
                        {
                            totalUnits = _allocationProfile.GetAllocatedBalance(new GeneralComponent(eComponentType.DetailType));

                            if (totalUnits < 0)
                            {
                                totalUnits = 0;
                            }
                           
                            if (this.PercentInd)
                            {
                                totalUnits = (int)((totalUnits * Available) / 100);
                            }
                            else
                            {
                                if (totalUnits > Available)
                                    totalUnits = (int)Available;
                            }



                            int totalDetailAllocated = totalPackUnitsAllocated; //(int)_allocationProfile.GetQtyAllocated(new GeneralComponent(eComponentType.AllNonGenericPacks));

                            int leftOver = totalUnits - totalDetailAllocated;

                            int bulkTotalUnits = _allocationProfile.GetAllocatedBalance(new GeneralComponent(eComponentType.Bulk));

                            if (leftOver < 0)
                            {
                                leftOver = 0;
                            }

                            if (bulkTotalUnits < leftOver)
                            {
                                availableUnits = bulkTotalUnits;
                            }
                            else
                            {
                                availableUnits = leftOver;
                            }


                         
                        
                        }
                    }
					//End TT#1636-MD -jsobek -Pre-Pack Fill Size

					

                    //// begin MID track 4967 Size Functions Not Showing Total Qty
                    //this._actionAuditList.Add(new AllocationActionAuditStruct
                    //    ((int)eAllocationMethodType.FillSizeHolesAllocation,
                    //    _allocationProfile,     // TT#1199 - MD - NEED allocation # of stores is zero - rbeck
                    //    aComponent,
                    //    _allocationProfile.GetQtyAllocated(aComponent),
                    //    _allocationProfile.GetCountOfStoresWithAllocation(aComponent),
                    //    true,
                    //    true));
                    //// end MID track 4967 Size Functions Not Showing Total Qty


					//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                    if (aComponent.ComponentType == eComponentType.SpecificColor)
                    {
                        AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                        //HdrColorBin colorBin = _allocationProfile.GetHdrColorBin(colorComponent.ColorRID);  // TT#702 Infinite Loop when begin date set
                        //_sizeNeedResultsHash.Add(colorComponent.ColorRID, _sizeNeedResults);    // TT#702 Infinite Loop when begin date set
                        // begin MID track 4967 Size Functions Not Showing Total Qty
                        GeneralComponent sizeComponent = new GeneralComponent(eGeneralComponentType.AllSizes);
                        AllocationColorSizeComponent acsc = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					//End TT#1636-MD -jsobek -Pre-Pack Fill Size
		
                        this._actionAuditList.Add(new AllocationActionAuditStruct
                            ((int)eAllocationMethodType.FillSizeHolesAllocation,
							//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
							//ap,     // TT#1199 - MD - NEED allocation # of stores is zero - rbeck
						    //colorComponent,
						    //ap.GetQtyAllocated(colorComponent),
						    //ap.GetCountOfStoresWithAllocation(colorComponent),
                            _allocationProfile,    // TT#1199 - MD - NEED allocation # of stores is zero - rbeck 
                            acsc,
                            _allocationProfile.GetQtyAllocated(acsc),                // TT#702 Infinite Loop when begin date set
                            _allocationProfile.GetCountOfStoresWithAllocation(acsc), // TT#702 Infinite Loop when begin date set 
							//End TT#1636-MD -jsobek -Pre-Pack Fill Size

                            true,
                            true));
                    }  //TT#1636-MD -jsobek -Pre-Pack Fill Size




                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
					//=========================================================
					// Set the sizeNeedMethod for this component
					//=========================================================
                    //ap.SetSizeFillMethod(colorComponent.ColorRID, this);
                    // end TT#2155 - JElllis - Fill Size Holes Null Reference

                    // begin TT#702 Infinite Loop when begin date set
					//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
					//SizeNeedResults sizeResults =
                    //    sna.ProcessFillSize
                    //        (this._fillSizesToType,
                    //         availableUnits,
                    SizeNeedResults sizeResults =  sna.ProcessFillSize(this._fillSizesToType, availableUnits, eSizeProcessControl.ProcessAll, ApplyRulesOnly);                  
					//End TT#1636-MD -jsobek -Pre-Pack Fill Size

       				//SizeNeedResults sizeResults = 
                    //    //sna.ProcessFillSizeHoles  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    //(availableUnits,          // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    sna.ProcessFillSize         // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    (this._fillSizesToType,     // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    availableUnits,             // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    ap, 
                    //    colorComponent, 
                    //    sizeCurveGroup, 
                    //    storeList, 
                    //    this._MerchandiseType, this._MerchHnRid, this._MerchPhRid, this._MerchPhlSequence,
                    //    this.SG_RID,
                    //    //this.SizeAlternateRid, this.SizeConstraintRid, this.RulesCollection, // MID Track 4372 Generic Size Constraints
                    //    this.SizeAlternateRid, sizeConstraintRID, this.RulesCollection,        // MID Track 4372 Generic Size Constraints
                    //    eSizeNeedColorPlan.PlanForSpecificColorOnly,
                    //    eSizeProcessControl.ProcessAll, // MID Track 4861 Size Curve Normalization
                    //    this.NormalizeSizeCurves);      // MID Track 4861 Size Curve Normalization
					if (aComponent.ComponentType == eComponentType.DetailType || aComponent.ComponentType == eComponentType.AllNonGenericPacks) 
                    {
                      
                     //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
       
                        DetailPackOptions pOpt = new DetailPackOptions();
                        pOpt._storeFilteredList = storeList;
                        pOpt.maxPackNeedTolerance = this._MaxPackNeedTolerance;
                        pOpt.avgPackDeviationTolerance = this._AvgPackDeviationTolerance;
                        pOpt.overrideAvgPackDevTolerance = this._overrideAvgPackDevTolerance;
                        pOpt.overrideMaxPackNeedTolerance = this._overrideMaxPackNeedTolerance;
                        pOpt.packToleranceNoMaxStep = this._packToleranceNoMaxStep;
                        pOpt.packToleranceStepped = this._packToleranceStepped;
                        pOpt.isFillSizeHoles = true;
                        PackProcessing.AllocateNonGenericPacks(sizeResults, pOpt, _allocationProfile, aApplicationTransaction, out totalPackUnitsAllocated);  
                       

                        _allocationProfile.PackBreakoutByContent = true; // MID Track # 2983 Size Need not performed in workflow // TT#702 Infinite Loop when begin date set  //TT#1636-MD -jsobek -Pre-Pack Fill Size

						// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                        SortedList colors = new SortedList(new HdrColorBinProcessOrder());
                        foreach (HdrColorBin hcb in _allocationProfile.BulkColors.Values)
                        {
                            colors.Add(hcb, hcb);
                        }

                        foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough
                        {
                            foreach (PackColorSize pcs in ph.PackColors.Values)
                            {

                                foreach (HdrColorBin hcb in colors.Values)
                                {
                                    if (hcb.ColorCodeRID == pcs.ColorCodeRID)
                                    {
                                        AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
                                        colorBin = _allocationProfile.GetHdrColorBin(aColor.ColorRID);

                                        sna._targetComponent = aColor;
                                        Debug.WriteLine("BEGIN FILL SIZE BULK AFTER DETAIL");
                                        sizeResults = sna.ProcessFillSizeBulkAfterDetail(this._fillSizesToType, availableUnits, eSizeProcessControl.ProcessAll, ApplyRulesOnly);

										// Begin TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
                                        bool honorMax = _allocationProfile._honorMaximums;
                                        _allocationProfile._honorMaximums = false;
										// End TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
										
                                        int storeCnt = storeList.Count;
                                        int units; 
                                        for (int i = 0; i < storeCnt; i++)
                                        {
                                            StoreProfile sp = (StoreProfile)storeList[i];
                                            Index_RID storeIndex = (Index_RID)_allocationProfile.StoreIndex(sp.Key);  //TT#1636-MD -jsobek -Pre-Pack Fill Size  
                                            foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                            {
                                                units = sizeResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID);
                                                int priorUnits = _allocationProfile.GetStoreQtyAllocated(hsb, storeIndex);
                                                if (units != _allocationProfile.GetStoreQtyAllocated(hsb, storeIndex))
                                                {
                                                    Debug.WriteLineIf(units > 0, "ProcessComponent() set values ST: " + sp.Key + " SZ: " + hsb.SizeCodeRID + " PRIOR: " + priorUnits + " UNITS: " + units);
                                                    _allocationProfile.SetStoreQtyAllocated(colorBin, hsb.SizeCodeRID, storeIndex, units, eDistributeChange.ToParent, false); // Assortment: color/size changes
                                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Total, storeIndex, true);
                                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.DetailType, storeIndex, true);
                                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Bulk, storeIndex, true);
                                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(colorBin, storeIndex, true);
                                                }
                                            }
                                        }
                                        _allocationProfile.BottomUpSizePerformed = true;
                                        _allocationProfile._honorMaximums = honorMax;	// TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
                                    }

                                }

                                
                            }
                        }
						// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                 
                    }
                    else
                    {
					 //End TT#1636-MD -jsobek -Pre-Pack Fill Size
                        int storeCnt = storeList.Count;
                        int units; // TT#80 - MD- JEllis - Fill Size holes Douubles allocation if applied twice (or applied to header with size allocation)
                        for (int i = 0; i < storeCnt; i++)
                        {
                            StoreProfile sp = (StoreProfile)storeList[i];
                            //SizeCurveProfile scp = sizeCurveGroup.GetStoreSizeCurveProfile(sp.Key); //MID Track 3631 Size Rules apply to all sizes on header
                            //SizeCurveProfile scp; // MID Track 3631 Size Rules Apply to all sizes on header.
							//Index_RID storeIndex = (Index_RID)ap.StoreIndex(sp.Key);  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                            Index_RID storeIndex = (Index_RID)_allocationProfile.StoreIndex(sp.Key);  //TT#1636-MD -jsobek -Pre-Pack Fill Size  

                            // begin MID Track 3631 Size Rules apply to all sizes on header
                            //foreach(SizeCodeProfile sizeCode in scp.SizeCodeList.ArrayList)
                            //{
                            //	if (colorBin.ColorSizes.ContainsKey(sizeCode.Key))
                            //	{
                            //		int units = sizeResults.GetAllocatedUnits(sp.Key, sizeCode.Key);
                            //		ap.SetStoreQtyAllocated(colorBin, sizeCode.Key, storeIndex, units, eDistributeChange.ToParent, false);
                            //	}
                            //}
                            foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                            {
                                // begin TT#80 - MD- JEllis - Fill Size Holes Doubles Allocation if applied twice (or applied to header with size allocation)
                                //int units = sizeResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID)   // MID Track 5349 "Fill Size Holes" wipes out size allocation results from previous actions
                                //            + ap.GetStoreQtyAllocated(hsb, storeIndex);          // MID Track 5349
                                // begin TT#1021 - MD - Jellis - Header Status Wrong
                                //units = sizeResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID);
                                // end TT#80 - MD- JEllis - Fill Size Holes Doubles Allocation if applied twice (or applied to header with size allocation)
                                //ap.SetStoreQtyAllocated(colorBin, hsb.SizeCodeRID, storeIndex, units, eDistributeChange.ToParent, false); // Assortment: color/size changes
                                units = sizeResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID);
								//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
								//if (units != ap.GetStoreQtyAllocated(hsb, storeIndex))
                           		//{
                               	//	  ap.SetStoreQtyAllocated(colorBin, hsb.SizeCodeRID, storeIndex, units, eDistributeChange.ToParent, false); // Assortment: color/size changes
                               	//	  ap.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Total, storeIndex, true);
                               	//	  ap.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.DetailType, storeIndex, true);
                               	//	  ap.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Bulk, storeIndex, true);
                                //	  ap.SetStoreAllocationFromBottomUpSize(colorBin, storeIndex, true);
                                if (units != _allocationProfile.GetStoreQtyAllocated(hsb, storeIndex))
                                {
                                    _allocationProfile.SetStoreQtyAllocated(colorBin, hsb.SizeCodeRID, storeIndex, units, eDistributeChange.ToParent, false); // Assortment: color/size changes
                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Total, storeIndex, true);
                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.DetailType, storeIndex, true);
                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(eAllocationSummaryNode.Bulk, storeIndex, true);
                                    _allocationProfile.SetStoreAllocationFromBottomUpSize(colorBin, storeIndex, true);
								//End TT#1636-MD -jsobek -Pre-Pack Fill Size
                                }
                                // end TT#1021 - MD - Jellis - Header Status Wrong
                            }
                            // end MID Track 3631 Size Rules apply to all sizes on header
                        }
						//ap.BottomUpSizePerformed = true; // MID Track # 2983 Workflow not processing size need  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                        _allocationProfile.BottomUpSizePerformed = true; // MID Track # 2983 Workflow not processing size need  //TT#1636-MD -jsobek -Pre-Pack Fill Size
                    }
				}
				catch (NoProcessingByFillSizeHoles)
				{
					throw;
				}
				catch (Exception ex)
				{
					aApplicationTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Header [" + aAllocationProfile.HeaderID + "] " + ex.Message, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
					throw;
				}
			}


			override public void Update(TransactionData td)
			{
	 
				if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
				{
					_methodData = new MethodFillSizeHolesData(td);

				}
				_methodData.SizeCurveGroupRid = SizeCurveGroupRid; 
				_methodData.SizeGroupRid = SizeGroupRid;       
				_methodData.PercentInd = PercentInd;
				_methodData.MerchPhRid = MerchPhRid;
				_methodData.MerchPhlSequence = MerchPhlSequence;
				_methodData.MerchHnRid = MerchHnRid;
				_methodData.MerchandiseType = MerchandiseType;
				_methodData.Available = Available;
				_methodData.StoreFilterRid = StoreFilterRid;
				_methodData.SizeAlternateRid = this.SizeAlternateRid;
				_methodData.SizeConstraintRid = this.SizeConstraintRid;
				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				_methodData.NormalizeSizeCurves = NormalizeSizeCurves;
				_methodData.NormalizeSizeCurvesDefaultIsOverridden = NormalizeSizeCurvesDefaultIsOverridden;
				// END MID Track #4826
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				_methodData.FillSizesToType = FillSizesToType;
				// End MID Track #4921
				// begin Generic Size Curve data
				_methodData.GenCurveHcgRID = base.GenCurveCharGroupRID;
				_methodData.GenCurveHnRID = base.GenCurveHnRID;
				_methodData.GenCurvePhRID = base.GenCurvePhRID;
				_methodData.GenCurvePhlSequence = base.GenCurvePhlSequence;
				_methodData.GenCurveColorInd = base.GenCurveColorInd;
				_methodData.GenCurveMerchType = base.GenCurveMerchType; 
				// end Generic Size Curve data

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                _methodData.UseDefaultCurve = base.UseDefaultCurve;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                _methodData.ApplyRulesOnly = base.ApplyRulesOnly;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                _methodData.GenCurveNsccdRID = base.GenCurveNsccdRID;
                // End TT#413
				
				// begin Generic Size Constraint data
				_methodData.GenConstraintHcgRID = base.GenConstraintCharGroupRID;
				_methodData.GenConstraintHnRID = base.GenConstraintHnRID;
				_methodData.GenConstraintPhRID = base.GenConstraintPhRID;
				_methodData.GenConstraintPhlSequence = base.GenConstraintPhlSequence;
				_methodData.GenConstraintColorInd = base.GenConstraintColorInd;
				_methodData.GenConstraintMerchType = base.GenConstraintMerchType;
				// end Generic Size Constraint data
                // BEGIN TT#41-MD - GTaylor - UC#2
                _methodData.IB_MERCH_HN_RID = IB_MERCH_HN_RID;
                _methodData.IB_MERCH_PH_RID = IB_MERCH_PH_RID;
                //_methodData.Inventory_Ind = Inventory_Ind;
                _methodData.IB_MERCH_PHL_SEQ = IB_MERCH_PHL_SEQ;
                _methodData.IB_MerchandiseType = IB_MerchandiseType;
                // END TT#41-MD - GTaylor - UC#2
                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                _methodData.OverrideVSWSizeConstraints = OverrideVSWSizeConstraints;
                _methodData.VSWSizeConstraints = (eVSWSizeConstraints)VSWSizeConstraints;
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                _methodData.OverrideAvgPackDevTolerance = OverrideAvgPackDevTolerance;
                _methodData.OverrideMaxPackNeedTolerance = OverrideMaxPackNeedTolerance;
                _methodData.AvgPackDeviationTolerance = this._AvgPackDeviationTolerance;
                _methodData.MaxPackNeedTolerance = this._MaxPackNeedTolerance;
                _methodData.PackToleranceNoMaxStep = _packToleranceNoMaxStep;
                _methodData.PackToleranceStepped = _packToleranceStepped;
                //End TT#1636-MD -jsobek -Pre-Pack Fill Size

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
			FillSizeHolesMethod newFillSizeHolesMethod = null;

			try
			{
				newFillSizeHolesMethod = (FillSizeHolesMethod)this.MemberwiseClone();
				newFillSizeHolesMethod.Available = Available;
				newFillSizeHolesMethod.ConstraintsLoaded = ConstraintsLoaded;
				newFillSizeHolesMethod.GenCurveCharGroupRID = GenCurveCharGroupRID;
				newFillSizeHolesMethod.GenCurveColorInd = GenCurveColorInd;
				newFillSizeHolesMethod.GenCurveHnRID = GenCurveHnRID;
				newFillSizeHolesMethod.GenCurveMerchType = GenCurveMerchType;
				newFillSizeHolesMethod.GenCurvePhlSequence = GenCurvePhlSequence;
				newFillSizeHolesMethod.GenCurvePhRID = GenCurvePhRID;
				newFillSizeHolesMethod.GetDimensionsUsing = GetDimensionsUsing;
				newFillSizeHolesMethod.GetSizes = GetSizes;
				newFillSizeHolesMethod.GetSizesUsing = GetSizesUsing;
				newFillSizeHolesMethod.MerchandiseType = MerchandiseType;
				newFillSizeHolesMethod.MerchHnRid = MerchHnRid;
				newFillSizeHolesMethod.MerchPhlSequence = MerchPhlSequence;
				newFillSizeHolesMethod.MerchPhRid = MerchPhRid;
				newFillSizeHolesMethod.Method_Change_Type = eChangeType.none;
				newFillSizeHolesMethod.Method_Description = Method_Description;
				newFillSizeHolesMethod.MethodStatus = MethodStatus;
				newFillSizeHolesMethod.Name = Name;
				newFillSizeHolesMethod.PercentInd = PercentInd;
				newFillSizeHolesMethod.PromptAttributeChange = PromptAttributeChange;
				newFillSizeHolesMethod.PromptSizeChange = PromptSizeChange;
				newFillSizeHolesMethod.SG_RID = SG_RID;
				newFillSizeHolesMethod.SizeAlternateRid = SizeAlternateRid;
				newFillSizeHolesMethod.SizeConstraintRid = SizeConstraintRid;
				newFillSizeHolesMethod.SizeCurveGroupRid = SizeCurveGroupRid;
				newFillSizeHolesMethod.SMBD = SMBD;
				newFillSizeHolesMethod.StoreFilterRid = StoreFilterRid;
				newFillSizeHolesMethod.User_RID = User_RID;
				newFillSizeHolesMethod.Virtual_IND = Virtual_IND;
                newFillSizeHolesMethod.Template_IND = Template_IND;
                // BEGIN MID Track #4826 - JSmith - Normalize Size Curves
                newFillSizeHolesMethod.NormalizeSizeCurves = NormalizeSizeCurves;
				newFillSizeHolesMethod.NormalizeSizeCurvesDefaultIsOverridden = NormalizeSizeCurvesDefaultIsOverridden;
				// END MID Track #4826
				
				// begin Generic Size Constraint data
				newFillSizeHolesMethod.GenConstraintCharGroupRID = GenConstraintCharGroupRID;
				newFillSizeHolesMethod.GenConstraintColorInd = GenConstraintColorInd;
				newFillSizeHolesMethod.GenConstraintHash = GenConstraintHash;
				newFillSizeHolesMethod.GenConstraintHnRID = GenConstraintHnRID;
				newFillSizeHolesMethod.GenConstraintMerchType = GenConstraintMerchType;
				newFillSizeHolesMethod.GenConstraintPhlSequence = GenConstraintPhlSequence;
				newFillSizeHolesMethod.GenConstraintPhRID = GenConstraintPhRID;

				newFillSizeHolesMethod.CreateConstraintData();
				newFillSizeHolesMethod.BuildRulesDecoder(SG_RID);

                // BEGIN TT#41-MD - GTaylor - UC#2
                newFillSizeHolesMethod.IB_MERCH_HN_RID = IB_MERCH_HN_RID;
                newFillSizeHolesMethod.IB_MERCH_PH_RID = IB_MERCH_PH_RID;
                newFillSizeHolesMethod.IB_MERCH_PHL_SEQ = IB_MERCH_PHL_SEQ;
                //newFillSizeHolesMethod.Inventory_Ind = Inventory_Ind;
                newFillSizeHolesMethod.IB_MerchandiseType = IB_MerchandiseType;
                // END TT#41-MD - GTaylor - UC#2
                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                newFillSizeHolesMethod.OverrideVSWSizeConstraints = OverrideVSWSizeConstraints;
                newFillSizeHolesMethod.VSWSizeConstraints = (eVSWSizeConstraints)VSWSizeConstraints;
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                newFillSizeHolesMethod.MaxPackNeedTolerance = MaxPackNeedTolerance;;
                newFillSizeHolesMethod.OverrideAvgPackDevTolerance = OverrideAvgPackDevTolerance;
                newFillSizeHolesMethod.OverrideMaxPackNeedTolerance = OverrideMaxPackNeedTolerance;
                newFillSizeHolesMethod.PackToleranceNoMaxStep = _packToleranceNoMaxStep;
                newFillSizeHolesMethod.PackToleranceStepped = _packToleranceStepped;
                //End TT#1636-MD -jsobek -Pre-Pack Fill Size


				return newFillSizeHolesMethod;
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

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;

            if (_MerchandiseType == eMerchandiseType.Node)
            {
                //BEGIN TT#4805 - DOConnell - Override Message When Opening Fill Size Method
                //hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_MerchPhRid, (int)eSecurityTypes.Store);
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_MerchHnRid, (int)eSecurityTypes.Store);
                //END TT#4805 - DOConnell - Override Message When Opening Fill Size Method
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }

            }

            if (_IB_MerchandiseType == eMerchandiseType.Node)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_IB_MERCH_HN_RID, (int)eSecurityTypes.Store);
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }

            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserFillSizeHoles);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            //RO-3884 Data Transport for Fill Size Method
            //throw new NotImplementedException("MethodGetData is not implemented");
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>();

            ROMethodFillSizeHolesProperties method = new ROMethodFillSizeHolesProperties(
                method: GetName.GetMethod(method: this),
                description: _methodData.Method_Description,
                userKey: User_RID,
                filter: GetName.GetFilterName(_methodData.StoreFilterRid),
                available: _methodData.Available,
                percentInd: _methodData.PercentInd,
                merch_HN: GetName.GetLevelKeyValuePair(_methodData.MerchandiseType, nodeRID: _methodData.MerchHnRid, merchPhRID: _methodData.MerchPhRid, merchPhlSequence: _methodData.MerchPhlSequence, SAB: SAB),
                merch_PH_RID: _methodData.MerchPhRid,
                merch_PHL_SEQ: _methodData.MerchPhlSequence,
                merchandiseType: EnumTools.VerifyEnumValue(_methodData.MerchandiseType),
                normalizeSizeCurvesDefaultIsOverridden: _methodData.NormalizeSizeCurvesDefaultIsOverridden,
                normalizeSizeCurves: _methodData.NormalizeSizeCurves,
                fillSizesToType: EnumTools.VerifyEnumValue(_methodData.FillSizesToType),
                sizeGroup: GetName.GetSizeGroup(_methodData.SizeGroupRid),
                sizeAlternateModel: GetName.GetSizeAlternateModel(_methodData.SizeAlternateRid),
                rOSizeCurveProperties: SizeCurveProperties.BuildSizeCurveProperties(_methodData.SizeCurveGroupRid, _methodData.GenCurveNsccdRID, _methodData.GenCurveHcgRID,
                    _methodData.GenCurveHnRID, _methodData.GenCurvePhRID, _methodData.GenCurvePhlSequence, _methodData.GenCurveMerchType,
                    _methodData.UseDefaultCurve, _methodData.ApplyRulesOnly, keyValuePair, keyValuePair, keyValuePair, SAB),
                rOSizeConstraintProperties: SizeConstraintProperties.BuildSizeConstraintProperties(_methodData.IB_MERCH_HN_RID, _methodData.IB_MERCH_PH_RID, _methodData.IB_MERCH_PHL_SEQ, _methodData.IB_MerchandiseType,
                    _methodData.SizeConstraintRid, _methodData.GenConstraintHcgRID, _methodData.GenConstraintHnRID, _methodData.GenConstraintPhRID, _methodData.GenConstraintPhlSequence, _methodData.GenConstraintMerchType,
                    _methodData.GenConstraintColorInd, keyValuePair, keyValuePair, keyValuePair, keyValuePair, SAB),
                overrideVSWSizeConstraints: _methodData.OverrideVSWSizeConstraints,
                vSWSizeConstraints: EnumTools.VerifyEnumValue(_methodData.VSWSizeConstraints),
                overrideAvgPackDevTolerance: _methodData.OverrideAvgPackDevTolerance,
                avgPackDeviationTolerance: _methodData.AvgPackDeviationTolerance,
                overrideMaxPackNeedTolerance: _methodData.OverrideMaxPackNeedTolerance,
                packToleranceStepped: _methodData.PackToleranceStepped,
                packToleranceNoMaxStep: _methodData.PackToleranceNoMaxStep,
                maxPackNeedTolerance: _methodData.MaxPackNeedTolerance,
                attribute: GetName.GetAttributeName(_methodData.SG_RID),
                sizeRuleAttributeSet: SizeRuleAttributeSet.BuildSizeRuleAttributeSet(_methodData.Method_RID, eMethodType.FillSizeHolesAllocation, _methodData.SG_RID, _methodData.SizeGroupRid, _methodData.SizeCurveGroupRid, GetSizesUsing, GetDimensionsUsing, MethodConstraints, SAB),
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
            //RO-3884 Data Transport for Fill Size Method
            ROMethodFillSizeHolesProperties roMethodFillSizeAllocationProperties = (ROMethodFillSizeHolesProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                Method_Description = roMethodFillSizeAllocationProperties.Description;
                User_RID = roMethodFillSizeAllocationProperties.UserKey;
                _StoreFilterRid = roMethodFillSizeAllocationProperties.Filter.Key;
                _Available = roMethodFillSizeAllocationProperties.Available;
                _PercentInd = roMethodFillSizeAllocationProperties.PercentInd;
                _MerchPhRid = roMethodFillSizeAllocationProperties.Merch_PH.Key;
                _MerchPhlSequence = roMethodFillSizeAllocationProperties.Merch_PH.Value;
                _MerchandiseType = roMethodFillSizeAllocationProperties.MerchandiseType;
                switch (_MerchandiseType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        _MerchHnRid = Include.Undefined;
                        break;
                    default: //eMerchandiseType.Node
                        _MerchHnRid = roMethodFillSizeAllocationProperties.Merch_HN.Key;
                        break;
                }
                NormalizeSizeCurvesDefaultIsOverridden = roMethodFillSizeAllocationProperties.NormalizeSizeCurvesDefaultIsOverridden;
                NormalizeSizeCurves = roMethodFillSizeAllocationProperties.NormalizeSizeCurves;
                _fillSizesToType = roMethodFillSizeAllocationProperties.FillSizesToType;
                SizeGroupRid = roMethodFillSizeAllocationProperties.SizeGroup.Key;
                _sizeAlternateRid = roMethodFillSizeAllocationProperties.SizeAlternateModel.Key;
                //Size Curve Group Box 
                SizeCurveGroupRid = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.SizeCurve.Key;
                GenCurveNsccdRID = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.SizeCurveGenericNameExtension.Key;
                GenCurveMerchType = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.GenCurveMerchType;
                switch (GenCurveMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenCurveHnRID = Include.NoRID; 
                        break;
                    default: //eMerchandiseType.Node
                        GenCurveHnRID = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.SizeCurveGenericHierarchy.Key;
                        break;
                }
                
                UseDefaultCurve = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.IsUseDefault;
                ApplyRulesOnly = roMethodFillSizeAllocationProperties.ROSizeCurveProperties.IsApplyRulesOnly;
                // Constraints Group Box
                IB_MerchandiseType = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.InventoryBasisMerchType;
                switch (IB_MerchandiseType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        _IB_MERCH_HN_RID = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.InventoryBasis.Key;
                        break;
                    default: //eMerchandiseType.Node
                        _IB_MERCH_HN_RID = Include.NoRID; 
                        break;
                }
                _sizeConstraintRid = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraint.Key;
                GenConstraintMerchType = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.GenConstraintMerchType;
                switch (GenConstraintMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenConstraintHnRID = Include.NoRID; 
                        break;
                    default: //eMerchandiseType.Node
                        GenConstraintHnRID = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHierarchy.Key;
                        break;
                }
                GenConstraintCharGroupRID = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHeaderChar.Key;
                GenConstraintColorInd = roMethodFillSizeAllocationProperties.ROSizeConstraintProperties.GenConstraintColorInd;
                //VSW 
                _overrideVSWSizeConstraints = roMethodFillSizeAllocationProperties.OverrideVSWSizeConstraints;
                _vSWSizeConstraints = roMethodFillSizeAllocationProperties.VSWSizeConstraints;
                _overrideAvgPackDevTolerance = roMethodFillSizeAllocationProperties.OverrideAvgPackDevTolerance;
                if (roMethodFillSizeAllocationProperties.OverrideAvgPackDevTolerance)
                {
                    AvgPackDeviationTolerance = roMethodFillSizeAllocationProperties.AvgPackDeviationTolerance;
                }
                _packToleranceStepped = roMethodFillSizeAllocationProperties.PackToleranceStepped;
                _packToleranceNoMaxStep = roMethodFillSizeAllocationProperties.PackToleranceNoMaxStep;
                _overrideMaxPackNeedTolerance = roMethodFillSizeAllocationProperties.OverrideMaxPackNeedTolerance;
                if (roMethodFillSizeAllocationProperties.OverrideMaxPackNeedTolerance)
                {
                    MaxPackNeedTolerance = roMethodFillSizeAllocationProperties.MaxPackNeedTolerance;
                }
                //Rules Tab
                SG_RID = roMethodFillSizeAllocationProperties.Attribute.Key;
                MethodConstraints = SizeRuleAttributeSet.BuildMethodConstrainst(roMethodFillSizeAllocationProperties.Method.Key, roMethodFillSizeAllocationProperties.Attribute.Key,
                    roMethodFillSizeAllocationProperties.SizeRuleAttributeSet, MethodConstraints, SAB);
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
	
