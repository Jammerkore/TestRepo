using System;
using System.Data;
using System.Collections;
using System.Collections.Generic; // TT#1410 - FL Detail Pack Allocations not giving enough packs
using System.ComponentModel;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for SizeNeedMethod.
	/// </summary>
	public class SizeNeedMethod : AllocationSizeBaseMethod
	{

		#region "Member Variables"
		private MethodSizeNeedData _methodData;
		private SizeNeedAlgorithm _sizeNeedAlgorithm;
		private SessionAddressBlock _aSAB;
		private bool _promptAttChange;
		private bool _promptSzChange;
		// private bool _promptFringeChange; // MID Track 3619 Remove Fringe
//		private int _SizeCurveGroupRid;
		private int _MerchHnRid;
		private int _MerchPhRid;
		private int _MerchPhlSequence;
		//private bool _SizeFringeInd;
		//private bool _EquateSizeInd;
		private double _AvgPackDeviationTolerance;
		private double _MaxPackNeedTolerance;
		private eMerchandiseType _MerchType;
		private bool _processCancelled = false;
		private int _componentsProcessed;
		private int _reserveStoreRid = Include.NoRID;
		private int _sizeAlternateRid;
		private int _sizeConstraintRid;
		// private int _sizeFringeRid; // MID Track 3619 Remove Fringe
        // BEGIN TT#41-MD - GTaylor - UC#2
        private eMerchandiseType _IB_MerchandiseType;
        //private char _inventory_Ind;
        private int _IB_MERCH_HN_RID;
        private int _IB_MERCH_PH_RID;
        private int _IB_MERCH_PHL_SEQUENCE;
        // END TT#41-MD - GTaylor - UC#2
        private eVSWSizeConstraints _vSWSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        private eFillSizesToType _FillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
		// counts, indices and other work fields used to allocate packs by need
        private ApplicationSessionTransaction _transaction;
        //private SizeNeedResults _sizeNeedResults;  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        private AllocationProfile _allocationProfile;
		//private ProfileList _storeFilteredList;
		/// <summary>
		/// distinguishes between the client method creating the class and the allocation profile creating the class for data access only.
		/// </summary>
		private bool _createdByClient;

		//private int _storeFilterCount;
		//private int _packCount;
		//private int _sizeCount;

		// 2-dim arrays by pack and store used to allocate packs by need
		//private int[,] _packSizeContent;
		//private double[,] _packSizeCurvePct;

		// 1-dim array by pack used to allocate packs by need
		//private int[] _packsToAllocate;
		//private int[] _packMultiple;
        private AllocationProfile[] _packApHome; // TT#1568 - MD - Jellis - GA Size Need not observing Header min/max


		// 1-dim arrays by store used to allocate packs by need
		//private Index_RID[] _storeIdxRID;
		//private int[] _storeUnitsRemainingToBreakout;
		//private int[] _storeTotalNeedError; 
		//private int[] _storeTotalPosUnitNeed;
		//private int[] _storeCandidateCount;
		//private int[] _storePrimaryPackIDX;
		//private double[] _storeTotalPosPctNeed; 
		//private double[] _storeAvgPosUnitNeed;
		//private double[] _storeAvgPosPctNeed;
		//private double[] _storePackDeviationErrorDiff;
		//private double[] _storePackNeedErrorDiff;
		//private double[] _storePackGapDiff;
		//private double[] _storeDesiredTotalUnits;
    

		// 2-dim arrays by store and pack used to allocate packs by need 
		//private int[,] _storePacksAllocated;
		//private bool [,] _storePackAvailable;
		//private bool [,] _storePackCandidate;
		//private double[,] _storePackDeviationError;
		//private int[,] _storePackDeviationErrorCount;
		//private double[,] _storePackAvgDeviationError;
		//private double[,] _storePackNeedError;
		//private double[,] _storePackGap;
        //private double[,] _storeSizePlan;           // TT#2920 - Jellis - Detail Pack Algorithm Change
        //private double[,] _storePackPctNeedMeasure; // TT#2920 -  Jellis - Detail Pack Algorithm Change

		// 2-dim arrays by store and size used to allocate packs by need
		//private double[,] _storeSizePosNeed;
		//private double[,] _storeSizePosPctNeed;
		//private int[,] _storeSizeNeedError;
		//private double[,] _storeDesiredAllocationCurve;
		//private double[,] _storeDesiredSizeUnits;

        //private Dictionary<int, StoreVector> _headerStoreUnitsAllocated;  // TT#1568 - MD - Jellis - GA Size need not observing Min/Max on member headers
        //private Dictionary<int, StoreVector> _inventoryBasisAllocation;  // TT#1568 - MD - Jellis - GA Size Need not observing Min/Max on member headers

        //private MIDHashtable _sizeNeedResultsHash;  // TT#2155 - JEllis - Fill SIze Holes Null Reference
//		private bool _secondTimeThru;
//		private bool _genCurveError = false;	// A&F Generic Size Curve 
		private ArrayList _actionAuditList;     // MID track 4967 Size Function not showing total qty
        private bool _overrideAvgPackDevTolerance;  // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
        private bool _overrideMaxPackNeedTolerance; // End TT#356
        private bool _packToleranceNoMaxStep;     // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        private bool _packToleranceStepped;       // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        private bool _overrideVSWSizeConstraints; // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

        private DetailPackOptions _pOpt = null; // TT#1386-MD - stodd - Manual Merge 

		#endregion

		#region "Properties"

		/// <summary>
		/// Get Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get	{return eProfileType.MethodSizeNeedAllocation;}
		}

		// BEGIN MID Track #2937 Size Onhand incorrect
		/// <summary>
		/// Gets the allocation profile 
		/// </summary>
		public AllocationProfile AllocationProfile
		{
			get 
			{
				return this._allocationProfile;
			}
		}
		// END MID Track #2937 Size Onhand incorrect
		/// <summary>
		/// Gets or sets Merchandise Type.
		/// </summary>
		public eMerchandiseType MerchType
		{
			get {return _MerchType;}
			set {_MerchType = value;}
		}

//		/// <summary>
//		/// Gets or sets Size Curve Group RID.
//		/// </summary>
//		public int SizeCurveGroupRid
//		{
//			get{return _SizeCurveGroupRid;}
//			set{_SizeCurveGroupRid = value;}
//		}

		/// <summary>
		/// Gets or sets the Maximum Pack Allocation Need Tolerance.
		/// </summary>
		public double MaxPackNeedTolerance
		{
			get {return _MaxPackNeedTolerance;}
			set {_MaxPackNeedTolerance = value;}
		}

		/// <summary>
		/// Gets or sets the Average Pack Deviation Tolerance.
		/// </summary>
		public double AvgPackDeviationTolerance
		{
			get {return _AvgPackDeviationTolerance;}
			set {_AvgPackDeviationTolerance = value;}
		}

		/// <summary>
		/// Gets or sets the Merchandise Product Hierarchy Level Sequence.
		/// </summary>
		public int MerchPhlSequence
		{
			get {return _MerchPhlSequence;}
			set {_MerchPhlSequence = value;}
		}

		/// <summary>
		/// Gets or sets the Merchandise Hierarchy Node RID.
		/// </summary>
		public int MerchHnRid
		{
			get {return _MerchHnRid;}
			set {_MerchHnRid = value;}
		}

		/// <summary>
		/// Gets or sets the Merchandise Product Hierarchy RID.
		/// </summary>
		public int MerchPhRid
		{
			get {return _MerchPhRid;}
			set {_MerchPhRid = value;}
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

		// begin MID Track 3619 Remove Fringe
		//public bool PromptFringeChange
		//{
		//	get {return _promptFringeChange;}
		//	set {_promptFringeChange = value;}
		//}
		// end MID Track 3619 Remove Fringe

		/// <summary>
		/// Gets or sets the Size Alternate RID.
		/// </summary>
		public int SizeAlternateRid
		{
			get {return _sizeAlternateRid;}
			set {_sizeAlternateRid = value;}
		}

		/// <summary>
		/// Gets or sets the Size Constraint RID.
		/// </summary>
		public int SizeConstraintRid
		{
			get {return _sizeConstraintRid;}
			set {_sizeConstraintRid = value;}
		}

		// begin MID Track 3619 Remove Fringe
		// /// <summary>
		// /// Gets or sets the Size Fringe RID.
		// /// </summary>
		//public int SizeFringeRid
		//{
		//	get {return _sizeFringeRid;}
		//	set {_sizeFringeRid = value;}
		//}
		// end MID Track 3619 Remove Fringe

        // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
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
        // End TT#356 

         // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        public bool OverrideVSWSizeConstraints
        {
            get { return _overrideVSWSizeConstraints; }
            set { _overrideVSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
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
        // end TT#1365 - JELlis - FL Detail Pack Size Need Enhancement
        // BEGIN TT#41-MD - GTaylor - UC#2
        public eMerchandiseType IB_MerchandiseType
        {
            get { return _IB_MerchandiseType; }
            set { _IB_MerchandiseType = value; }
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
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        public eFillSizesToType FillSizesToType
        {
            get { return _FillSizesToType; }
            set { _FillSizesToType = value; }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation
        #endregion

		#region "Constructors"
		/// <summary>
		/// Creates an instance of Size Need Method
		/// </summary>
		/// <param name="aSAB">Session Address Block</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public SizeNeedMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.SizeNeedAllocation)
		public SizeNeedMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.SizeNeedAllocation, eProfileType.MethodSizeNeedAllocation)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			Load(aSAB, aMethodRID);
			_createdByClient = true;
		}

		/// <summary>
		/// Creates an instance of Size Need Method for a specific allocation Profile.
		/// Used by allocation when requesting plan and need units for onHand and other calculations. 
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aMethodRID"></param>
		/// <param name="aProfile"></param>
		public SizeNeedMethod(SessionAddressBlock aSAB, int aMethodRID, AllocationProfile aProfile, ApplicationSessionTransaction aApplicationTransaction)
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//: base(aSAB, aMethodRID, eMethodType.SizeNeedAllocation)
			: base(aSAB, aMethodRID, eMethodType.SizeNeedAllocation, eProfileType.MethodSizeNeedAllocation)
			//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_allocationProfile = aProfile;
			_transaction = aApplicationTransaction;
			Load(aSAB, aMethodRID);
			_createdByClient = false;
		}

		private void Load(SessionAddressBlock aSAB, int aMethodRID)
		{
			_aSAB = aSAB;
            //_sizeNeedResultsHash = new MIDHashtable(); // TT#2155 - Jellis - Fill Size Holes Null Reference

			if (base.MethodType != eMethodType.SizeNeedAllocation)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_NotSizeNeedMethod),
					MIDText.GetText(eMIDTextCode.msg_NotSizeNeedMethod));
			}

			if (base.Filled)
			{
				#region METHOD VALUES
				_methodData = new MethodSizeNeedData(base.Key,eChangeType.populate);

                // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                //this._AvgPackDeviationTolerance = _methodData.AvgPackDeviationTolerance;
                //this._MaxPackNeedTolerance = _methodData.MaxPackNeedTolerance;
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
                // End  TT#356 

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

				this._MerchHnRid = _methodData.MerchHnRid;
				this._MerchPhlSequence = _methodData.MerchPhlSequence;
				this._MerchType = _methodData.MerchType;
				this._MerchPhRid = _methodData.MerchPhRid;
				base.SizeCurveGroupRid = _methodData.SizeCurveGroupRid;
				base.SizeGroupRid = _methodData.SizeGroupRid;
				this._sizeAlternateRid = _methodData.SizeAlternateRid;
				this._sizeConstraintRid	= _methodData.SizeConstraintRid;
				// this._sizeFringeRid = _methodData.SizeFringeRid; // MID Track Remove Fringe
				base.SG_RID = _methodData.SG_RID;
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

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.UseDefaultCurve = _methodData.UseDefaultCurve;
                // End TT#413
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                base.ApplyRulesOnly = _methodData.ApplyRulesOnly;
                // end TT#2155 - JEllis - Fill Size holes Null Reference

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
                #endregion
			}
			else
			{
				#region DEFAULT VALUES
				bool styleFound = false;
				_methodData = new MethodSizeNeedData();
				this._AvgPackDeviationTolerance = aSAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent; //Get from global options
				this._MaxPackNeedTolerance = aSAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;
                _packToleranceNoMaxStep = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep; // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                _packToleranceStepped = aSAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;     // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
				base.SizeGroupRid = Include.NoRID;
				base.SizeCurveGroupRid = Include.NoRID;
				this._sizeAlternateRid = Include.NoRID;
				this._sizeConstraintRid	= Include.NoRID;
				// this._sizeFringeRid = Include.NoRID; // MID Track 3619 Remove Fringe
				this.SG_RID = Include.NoRID;
				//HierarchyProfile hp;

				HierarchyProfile hp = aSAB.HierarchyServerSession.GetMainHierarchyData();

				for (int levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
					//hlp.LevelID is level name 
					//hlp.Level is level number 
					//hlp.LevelType is level type 
					if (hlp.LevelType == eHierarchyLevelType.Style)
					{
						this._MerchType = eMerchandiseType.HierarchyLevel;
						this._MerchPhlSequence = hlp.Level;
						this._MerchPhRid = hp.Key;
						this._MerchHnRid = Include.NoRID;
						styleFound = true;
						break;
					}				
				}

				//FALL BACK TO OTS PLAN LEVEL AS DEFAULT IF NO STYLE LEVEL IS FOUND
				//*****************************************************************
				if (!styleFound)
				{
					this._MerchType = eMerchandiseType.OTSPlanLevel;
					this._MerchPhlSequence = 0;
					this._MerchPhRid = Include.NoRID;
					this._MerchHnRid = Include.NoRID;
				}
				//*****************************************************************
				
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

                // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                this.OverrideAvgPackDevTolerance = false;
                this.OverrideMaxPackNeedTolerance = false;
                // End  TT#356 

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                base.UseDefaultCurve = false;
                // End TT#413
                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
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
                #endregion
			}
					
			SMBD = _methodData;

			_promptAttChange = true;
			_promptSzChange = true;
			// _promptFringeChange = true; // MID Track 3619 Remove Fringe

			base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
			base.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
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

            if (IsHierarchyNodeUser(this._MerchHnRid))
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

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction, 
			int aStoreFilter, Profile methodProfile)
		{
			_componentsProcessed = 0;
			aApplicationTransaction.ResetAllocationActionStatus();
			//_reserveStoreRid = aApplicationTransaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes run from Workflow ignores reserve store

			//			if (!base.OkToProcess(aApplicationTransaction))	// Generic Size Curve
			//			{
			//				return;
			//			}

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
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ErrorMessage, this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);
					continue;
				}
				catch (Exception ex)
				{
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.Message, this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(ap2.Key, eAllocationActionStatus.ActionFailed);
					continue;
				}

				// Allocation Component (Pack, BulkColor...)
				if (selectedComponentList.Count > 0)
				{
					AllocationProfileList apl = (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation);
//					AllocationProfile ap = (AllocationProfile)apl[0];
					AllocationProfile ap = ap2;
					// Begin MID Track 3294 Invalid Error Message
					ArrayList modifiedSelectList = new ArrayList();
					Hashtable packNameHash = new Hashtable();
					AllocationPackComponent apc;
					foreach (GeneralComponentWrapper gcw in selectedComponentList)
					{
						// Issue 4108
						GeneralComponent gc = gcw.GeneralComponent;
						// If we don't have the write allcoation profile, get the right one.
						if (gcw.HeaderRID != ap.HeaderRID)
							ap = (AllocationProfile)apl.FindKey(gcw.HeaderRID);
						// End issue 4108

						switch (gc.ComponentType)
						{
							case (eComponentType.SpecificPack):
							{
								apc = (AllocationPackComponent)gc;
								if (ap.PackIsOnHeader(apc.PackName))
								{
									if (!ap.GetPackHdr(apc.PackName).GenericPack)
									{
										if (!packNameHash.Contains(apc.PackName))
										{
											packNameHash.Add(apc.PackName, apc.PackName);
										}
									}
								}
								break;
							}
							default:
							{
								modifiedSelectList.Add(gcw);
								break;
							}
						}
					} // MID Change j.ellis picking multiple detail packs is rejected
					if (packNameHash.Count > 0)
					{
						if (packNameHash.Count == ap.NonGenericPackCount) 
						{
							modifiedSelectList.Add(new GeneralComponent(eGeneralComponentType.DetailType));
						}
						else
						{
							Audit audit = SAB.ApplicationServerSession.Audit; 
							audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_MustSelectAllDetailPacks), this.GetType().Name);
							throw new MIDException(eErrorLevel.warning,(int)(eMIDTextCode.msg_al_MustSelectAllDetailPacks),
								MIDText.GetText(eMIDTextCode.msg_al_MustSelectAllDetailPacks ));
						}
					}   // MID Change j.ellis picking multiple detail packs is rejected
					//} // MID Change j.ellis picking multiple detail packs is rejected
					foreach (GeneralComponentWrapper gcw in modifiedSelectList)
					{
						// Issue 4108
						// If we don't have the write allcoation profile, get the right one.
						if (gcw.HeaderRID != ap.HeaderRID)
							ap = (AllocationProfile)apl.FindKey(gcw.HeaderRID);
						// End issue 4108

						// End MID Track 3294 Invalid Error Message
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
					//				foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
					//				{
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
						Include.NoRID);

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

		/// <summary>
		/// processes a single action/work flow step
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		/// <param name="aApplicationWorkFlowStep"></param>
		/// <param name="aAllocationProfile"></param>
		/// <param name="WriteToDB"></param>
		/// <param name="aStoreFilterRID"></param>
        public override void ProcessAction(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aApplicationWorkFlowStep,
            Profile aAllocationProfile,
            bool WriteToDB,
            int aStoreFilterRID)
        {
            string msg;

            _allocationProfile = (AllocationProfile)aAllocationProfile; // MID Track # 2983 Size Need not performed by workflow  // TT#702 Infinite Loop when begin date set
            Audit audit = aSAB.ApplicationServerSession.Audit;
            if (_allocationProfile == null)
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
            _transaction = aApplicationTransaction; // TT#702 Size Need Method "hangs" when processing hdrs with begin date
            this._actionAuditList = new ArrayList();  // MID Track 4967 Size Functions Not Showing Total Qty
            try
            {
                _allocationProfile.ResetTempLocks(false);  // TTE421 Detail packs/bulknot allocated by Size Need Method.
                _reserveStoreRid = _transaction.GlobalOptions.ReserveStoreRID; // MID Track 4413 Fill Size Holes run from Workflow ignores reserve store // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                _transaction.ProcessingSizeNeedMethod = this; // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                //Audit audit = SAB.ApplicationServerSession.Audit; // MID Track 3011 do not process headers with status rec'd out bal
                if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aApplicationWorkFlowStep._method.MethodType))
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_WorkflowTypeInvalid),
                        MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));

                }
                // begin removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                try
                {
                    if (!base.OkToProcess(_transaction, _allocationProfile))	// Generic Size Curve // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    {
                        _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed);  // MID Track 5183 Workflow should stop after Severe Error // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                        return;
                    }
                }
                catch (MIDException)
                {
                    _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    throw;
                }
                // end removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date


                // begin TT#421 Detail packs/bulk not allocated by Size Need method
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
                //	catch (MIDException)
                //	{
                //		audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile), this.ToString());
                //		throw;
                //	}
                //}
                // end TT#421 Detail packs/bulk not allocated by Size Need method

                // BEGIN MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
                if (_allocationProfile.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)  // TT#702 Size Need Method "hangs" when processing hdrs with begin date

                {
                    msg = string.Format(
                        audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), MIDText.GetTextOnly((int)_allocationProfile.HeaderAllocationStatus)); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning, eMIDTextCode.msg_HeaderStatusDisallowsAction,
                        (this.Name + " " + _allocationProfile.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                        this.GetType().Name);
                    _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.NoActionPerformed); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                }
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                else if (_allocationProfile.BeginDay != Include.UndefinedDate
                    && this.SizeCurveGroupRid == Include.NoRID
                    && !base.GenericSizeCurveDefined())
                {
                    msg = string.Format(
                        audit.GetText(eMIDTextCode.msg_al_SizeNeedMthdInvalidWhenBeginDateAndNoCurves, false), _allocationProfile.HeaderID, Name);
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning, eMIDTextCode.msg_al_SizeNeedMthdInvalidWhenBeginDateAndNoCurves,
                        (msg),
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                else
                {
                    // END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
                    // AllocationProfile ap = (AllocationProfile) aAllocationProfile; // MID Track # 2938 Size Need not performed in workflow

                    ProfileList storeFilterList = null;
                    // BEGIN Issue 5727 stodd
                    bool outdatedFilter = false;
                    // END Issue 5727
                    //*******************************
                    // Apply STORE FILTER if present
                    //*******************************
                    if (aStoreFilterRID != Include.NoRID)
                    {
                        // BEGIN Issue 5727 stodd
                        FilterData storeFilterData = new FilterData();
                        string filterName = storeFilterData.FilterGetName(aStoreFilterRID);
                        storeFilterList = aApplicationTransaction.GetAllocationFilteredStoreList(_allocationProfile, aStoreFilterRID, ref outdatedFilter); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                        if (outdatedFilter)
                        {
                            msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                            msg = msg.Replace("{0}", filterName);
                            string suffix = ". Method " + this.Name + ". Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error in audit // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                            string auditMsg = msg + suffix;
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
                            throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                        }
                        // END Issue 5727
                        msg = audit.GetText(eMIDTextCode.msg_al_StoreFilterAppliedToMethod, false);
                        msg = msg.Replace("{0}", filterName);
                        msg = msg.Replace("{1}", "Size Need Method");
                        msg = msg.Replace("{2}", this.Name);
                        msg = msg.Replace("{3}", _allocationProfile.HeaderID); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                        audit.Add_Msg(
                            eMIDMessageLevel.Information, eMIDTextCode.msg_al_StoreFilterAppliedToMethod,
                            msg, this.GetType().Name);
                        // END Issue 4632
                    }
                    else
                    {
                        storeFilterList = _transaction.GetMasterProfileList(eProfileType.Store); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    }
                    // removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date

                    //==============================
                    // Get componet to process
                    //==============================
                    AllocationWorkFlowStep aAllocationWorkFlowStep = (AllocationWorkFlowStep)aApplicationWorkFlowStep;
                    GeneralComponent aComponent = aAllocationWorkFlowStep.Component;
                    switch (aComponent.ComponentType)
                    {
                        case (eComponentType.SpecificColor):
                        case (eComponentType.DetailType):
                        case (eComponentType.AllNonGenericPacks):
                            {
                                ProcessComponent(_allocationProfile, aComponent, storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date // TT#1436 - MD - JEllis - GA allocates bulk before packs
                                // begin TT#1018 - MD - Jellis - Over ALlocated
                                if (aComponent.ComponentType != eComponentType.SpecificColor)
                                {
                                    PackProcessing.AdjustDetailTotals(_allocationProfile);
                                }
                                // end TT#1018 - MD - JEllis - Over Allocated
                                break;
                            }
                        case (eComponentType.Total):
                            {
                                if (_allocationProfile.NonGenericPackCount > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                {
                                    ProcessComponent(_allocationProfile, new GeneralComponent(eGeneralComponentType.DetailType), storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date // TT#1436 - MD - JEllis - GA allocates bulk before packs
                                    // BEGIN MID Track 3122 Bulk not allocated when bulk and sized packs on same header
                                    if (_allocationProfile.BulkIsDetail   // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                        && _allocationProfile.BulkColors.Count > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                    {
                                        // begin TT#1568 - MD - Jellis -  GA Size need not observing Header min/max
                                        // Begin TT#4988 - BVaughan - Performance
                                        #if DEBUG
                                        if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
                                        {
                                            throw new Exception("Object does not match AssortmentProfile in ProcessAction()");
                                        }
                                        #endif
                                        //if (_allocationProfile is AssortmentProfile)
                                        if (_allocationProfile.isAssortmentProfile)
                                        // End TT#4988 - BVaughan - Performance
                                        {
                                            AllocationProfile[] apList = ((AssortmentProfile)_allocationProfile).AssortmentMembers;
											// Begin TT#1386-MD - stodd - Manual Merge
                                            AllocateGroupBulk((AssortmentProfile)_allocationProfile, apList, storeFilterList);
											//PackProcessing.AllocateBulkAfterPacks(_allocationProfile, _transaction, storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
											// End TT#1386-MD - stodd - Manual Merge
                                        }
                                        else
                                        {
                                            // end TT#1568 - MD - Jellis -  GA Size need not observing Header min/max
                                            //AllocateBulkAfterPacks(storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                            // Begin TT#5269 - JSmith - Size Need not balancing to allocated units
                                            //PackProcessing.AllocateBulkAfterPacks(_allocationProfile, _transaction, storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                            PackProcessing.AllocateBulkAfterPacks(_allocationProfile, _transaction, storeFilterList, false); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                            // End TT#5269 - JSmith - Size Need not balancing to allocated units
                                        }  // TT#1568 - MD - Jellis -  GA Size need not observing Header min/max
										
                                    }
                                    // END MID Track 3122 Bulk not allocated when bulk and sized packs on same header

                                }

                                if (_allocationProfile.BulkColors.Count > 0) // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                {
                                    ProcessBulk(storeFilterList);  // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                }
                                PackProcessing.AdjustDetailTotals(_allocationProfile);  // TT#1018 - MD - Jellis - Over Allocated

                                break;
                            }
                        case (eComponentType.AllColors):
                        case (eComponentType.Bulk):
                            {
                                ProcessBulk(storeFilterList); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                break;
                            }

                        case (eComponentType.SpecificPack):
                        case (eComponentType.SpecificSize):
                        case (eComponentType.GenericType):
                        case (eComponentType.AllPacks):
                        case (eComponentType.AllGenericPacks):
                        case (eComponentType.AllSizes):
                        case (eComponentType.ColorAndSize):
                            {
                                // begin removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                                audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_MustSelectAllDetailPacks), this.GetType().Name);
                                throw new MIDException(eErrorLevel.warning, (int)(eMIDTextCode.msg_al_MustSelectAllDetailPacks),
                                    MIDText.GetText(eMIDTextCode.msg_al_MustSelectAllDetailPacks));
                            }

                        default:
                            {
                                string errorMsg =
                                    MIDText.GetTextOnly(eMIDTextCode.frm_SizeNeedMethod)
                                    + " ["
                                    + this.Name
                                    + "] Header ["
                                    + ((AllocationProfile)aAllocationProfile).HeaderID
                                    + "] "
                                    + MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent);
                                audit.Add_Msg(
                                    eMIDMessageLevel.Severe,
                                    eMIDTextCode.msg_UnknownAllocationComponent,
                                    errorMsg,
                                    this.GetType().Name);
                                throw new MIDException(
                                    eErrorLevel.severe,
                                    (int)eMIDTextCode.msg_UnknownAllocationComponent,
                                    errorMsg);
                            }
                        // end removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    }

                    // BEGIN MID Track # 2983 Size Need not performed in workflow
                    if (WriteToDB)
                    {
                        _allocationProfile.WriteHeader(); // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    }
                    _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);  // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    // END MID Track # 2983 Size Need not performed in workflow
                    msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
                    msg = msg.Replace("{0}", "Size Need");
                    msg = msg.Replace("{1}", this.Name);
                    msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error on audit // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                    audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());
                    // removed unnecessary comments // TT#702 Size Need Method "hangs" when processing hdrs with begin date

                    foreach (AllocationActionAuditStruct auditStruct in this._actionAuditList)
                    {
                        _transaction.WriteAllocationAuditInfo( // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                            _allocationProfile.Key,            // TT#702 Size need Method "hangs" when processing hdrs with begin date
                            0,
                            this.MethodType,
                            this.Key,
                            this.Name,
                            ((AllocationColorSizeComponent)auditStruct.Component).ColorComponent.ComponentType,
                            _transaction.GetColorCodeProfile(((AllocationColorOrSizeComponent)((AllocationColorSizeComponent)auditStruct.Component).ColorComponent).ColorRID).ColorCodeName, // TT#702 Size Need Method "hangs" when processing hdrs with begin date
                            null,
                            _allocationProfile.GetQtyAllocated(auditStruct.Component) - auditStruct.QtyAllocatedBeforeAction,                                   // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                            Math.Abs(_allocationProfile.GetCountOfStoresWithAllocation(auditStruct.Component) - auditStruct.StoresWithAllocationBeforeAction)); // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                    }
                    // end MID track 4967 Size Functions Not Showing Total Qty
                } // MID Track 3011 Do not process headers with status rec'd out bal
            }
            catch (MIDException ex)
            {
                _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ErrorMessage, this.ToString());
                SAB.ApplicationServerSession.Audit.Add_Msg(Include.TranslateErrorLevel(ex.ErrorLevel), ex.ErrorMessage, this.ToString());
                //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                msg = msg.Replace("{0}", "Size Need");
                msg = msg.Replace("{1}", this.Name);
                msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error on audit // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error on audit
                SAB.ApplicationServerSession.Audit.Add_Msg(Include.TranslateErrorLevel(ex.ErrorLevel), msg, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error on audit
                //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                throw;
            }
            catch (Exception ex)
            {
                _transaction.SetAllocationActionStatus(_allocationProfile.Key, eAllocationActionStatus.ActionFailed); // MID Track # 2983 Size Need not performed in workflow // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
                msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                msg = msg.Replace("{0}", "Size Need");
                msg = msg.Replace("{1}", this.Name);
                msg += " Header [" + _allocationProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' feature gets error on audit // TT#702 Size Need Method "hangs" when Processing hdrs with begindate
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error on audit
                throw;
            }
            // begin TT#421 Detail packs/bulk not allocated by Size Need Method
            finally
            {
                _allocationProfile.ResetTempLocks(true);
                _transaction.ProcessingSizeNeedMethod = null;  // TT#2305 - FL Balance does not observe size multiples
            }
            // end TT#421 Detail packs/bulk not allocated by Size Need Method
        }

        // begin TT#702 Infinite Loop when begin date set
        //private void ProcessBulk(
        //    SessionAddressBlock aSAB, 
        //    ApplicationSessionTransaction aTransaction,
        //    AllocationProfile aAllocationProfile,
        //    ProfileList aStoreFilterList)
        //{
        //    SortedList colors = new SortedList(new HdrColorBinProcessOrder());
        //    foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
        //    {
        //        colors.Add(hcb, hcb);
        //    }

        //    if (colors.Count > 0)
        //    {
        //        foreach (HdrColorBin hcb in colors.Values)
        //        {
        //            AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
        //            ProcessComponent(aSAB, aTransaction,  (AllocationProfile)aAllocationProfile, aColor, aStoreFilterList);
        //        }
        //    }
        //}
        ///// <summary>
        ///// processes a single component
        ///// </summary>
        ///// <param name="aSAB"></param>
        ///// <param name="aApplicationTransaction"></param>
        ///// <param name="aAllocationProfile"></param>
        ///// <param name="aComponent"></param>
        //private void ProcessComponent(
        //    SessionAddressBlock aSAB, 
        //    ApplicationSessionTransaction aApplicationTransaction,  
        //    AllocationProfile aAllocationProfile,
        //    GeneralComponent aComponent,
        //    ProfileList aStoreList)

        private void ProcessBulk(ProfileList aStoreFilterList)
        {
            //SortedList colors = new SortedList(new HdrColorBinProcessOrder());  // TT#1436 - MD - JEllis - GA allocates bulk before packs
            // begin TT#1436 - MD - JEllis - GA allocates bulk before packs
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in ProcessBulk()");
            }
            #endif
            //if (_allocationProfile.AssortmentProfile != null
            //    && !(_allocationProfile is AssortmentProfile)
            //    && _allocationProfile.NonGenericPackCount == 0)
            if (_allocationProfile.AssortmentProfile != null
                  && !(_allocationProfile.isAssortmentProfile)
                  && _allocationProfile.NonGenericPackCount == 0)
            // End TT#4988 - BVaughan - Performance
            {
                Index_RID storeIdxRID;
                foreach (StoreProfile sp in aStoreFilterList)
                {
                    storeIdxRID = _allocationProfile.StoreIndex(sp.Key);
                    _allocationProfile.SetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID, _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID), eDistributeChange.ToChildren, false, false, false);
                }
            }
            // end TT#1436 - MD - JEllis - GA allocates bulk before packs

            // begin  TT#1436 - MD - JEllis - GA allocates bulk before packs
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in ProcessBulk()");
            }
            #endif
            // End TT#4988 - BVaughan - Performance
            AllocationProfile[] apList;
            // Begin TT#4988 - BVaughan - Performance
            //if (_allocationProfile is AssortmentProfile)
            if (_allocationProfile.isAssortmentProfile)
            // End TT#4988 - BVaughan - Performance
            {
                apList = ((AssortmentProfile)_allocationProfile).AssortmentMembers;
            }
            else
            {
                apList = new AllocationProfile[1];
                apList[0] = _allocationProfile;
            }
            foreach (AllocationProfile ap in apList)
            {
                SortedList colors = new SortedList(new HdrColorBinProcessOrder());
                // end  TT#1436 - MD - JEllis - GA allocates bulk before packs
                foreach (HdrColorBin hcb in ap.BulkColors.Values)  // TT#1436 - MD - JEllis - GA allocates bulk before packs
                {
                    colors.Add(hcb, hcb);
                }
                if (colors.Count > 0)
                {
                    foreach (HdrColorBin hcb in colors.Values)
                    {
                        AllocationColorOrSizeComponent aColor = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, hcb.ColorCodeRID);
                        ProcessComponent(ap, aColor, aStoreFilterList); // TT#1436 - MD - JEllis - GA allocates bulk before packs
                    }
                }
            }  // TT#1436 - MD - JEllis - GA allocates bulk before packs
        }
        /// <summary>
        /// processes a single component
        /// </summary>
        /// <param name="aComponent">Component to process</param>
        /// <param name="aStoreList">List of stores to process</param>
        private void ProcessComponent(
            AllocationProfile aAllocationProfile,   // TT#1436 - MD - JEllis - GA allocates bulk before packs
            GeneralComponent aComponent,
            ProfileList aStoreList)
            // end TT#702 Infinite loop when begin date set
		{
            // removed all unnecessary comments from this method (TT#702 Infinite Loop when begin date set)
			int genSizeCurveGroupRID = Include.NoRID;	// A&F Generic Size Curve
            // begin TT#3744 - MD - JELlis - Group Allocation Inconsistent Curve Adjd Onhand for 2 member headers with same color
            //=========================================================
            // Set the sizeNeedMethod for this component
            //=========================================================
            if (aComponent.ComponentType == eComponentType.AllNonGenericPacks)
            {
                aAllocationProfile.SetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType), this); // TT#702 Inifinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
            }
            else
            {
                aAllocationProfile.SetSizeNeedMethod(aComponent, this); // TT#702 Inifinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
            }
            // end TT#3744 - MD - JEllis - Group Allocation Inconsistent Curve Adjd Onhand for 2 member headers with same color
			//AllocationProfile ap = null;  // TT#702 Infinite Loop when begin date set
			//===============================================================================// 
			// Trim store list to stores that have an allocation to breakout by size or pack //
			//===============================================================================//
			ProfileList storeList = new ProfileList(eProfileType.Store);
			int storeInCnt = aStoreList.Count;
			for (int i=0;i<storeInCnt;i++)
			{
				StoreProfile sp = (StoreProfile)aStoreList[i];

				if (_createdByClient)
				{
					if (DoesStoreHaveUnitsToAllocate(aComponent, sp.Key)) // TT#702 Infinite Loop when begin date set
					{
						// don't alloc to reserve store either
						// otherwise add store to list
						if (sp.Key != _reserveStoreRid)
							storeList.Add(sp);
					}
				}
				else
				{
					if (sp.Key != _reserveStoreRid)
						storeList.Add(sp);
				}
			}

			// No stores have allocated units to breakout to size. 
			bool continueProcessing = true;
			if (storeList.Count == 0)
			{
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess) + " Header [" + aAllocationProfile.HeaderID + "] ", this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit // TT#702 Infinite Loop when begin date // TT#1436 - MD - JEllis - GA allocates bulk before packs
				continueProcessing = false;
			}

			//=========================================================================
			// Compares Sizes on the Header with sizes in the method Size Curve Group
			//=========================================================================
			if (continueProcessing)
			{
				//ap = (AllocationProfile) aAllocationProfile;  // TT#702 Infinite Loop when begin date
				ArrayList compSizeList = new ArrayList();

				if (aComponent.ComponentType == eComponentType.DetailType)
				{
					// Assume for now NO Color in packs
                    foreach (PackHdr ph in aAllocationProfile.NonGenericPacks.Values) // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
					{
						foreach (PackColorSize pcs in ph.PackColors.Values)
						{
                            foreach (PackSizeBin packSize in pcs.ColorSizes.Values) // Assortment: added pack size bin
							{
								int size = packSize.ContentCodeRID;
								compSizeList.Add(size);
							}
						}
					}
				}
				else if (aComponent.ComponentType == eComponentType.SpecificColor)
				{
					AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                    HdrColorBin colorBin = aAllocationProfile.GetHdrColorBin(colorComponent.ColorRID);   // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs

					foreach (int sizeRID in colorBin.ColorSizes.Keys)
					{
						compSizeList.Add(sizeRID);
					}

				}
				else
				{
                    foreach (int sizeKey in aAllocationProfile.GetSizesOnHeader().Values) // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
					{
						compSizeList.Add(sizeKey);
					}
				}
                // begin TT#1410 - FL Detail Pack Allocation not giving enough packs
                int[] sortedList = new int[compSizeList.Count];
                compSizeList.CopyTo(sortedList);
                Array.Sort(sortedList);
                compSizeList.Clear();
                int lastRID = Include.NoRID;
                foreach (int sizeRID in sortedList)
                {
                    if (sizeRID != lastRID)
                    {
                        compSizeList.Add(sizeRID);
                        lastRID = sizeRID;
                    }
                }
                // end TT#1410 - FL Detail Pack Allocation not giving enough packs

				ProfileList scl;
				if (base.GenericSizeCurveDefined())
				{
					try
					{
                        genSizeCurveGroupRID = (int)base.GenCurveHash[aAllocationProfile.Key]; // TT#702 Size Need Method "hangs" when processing hdrs with begin date // TT#1436 - MD - JEllis - GA allocates bulk before packs
						SizeCurveGroupProfile sizeCurveGroup = new SizeCurveGroupProfile(genSizeCurveGroupRID);
						scl = sizeCurveGroup.SizeCodeList;
					}
					catch
					{
                        string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader), string.Empty, aAllocationProfile.HeaderID); // TT#702 Inifinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoSizeCurveForHeader,errMessage);
					}
				}
				else if (this.SizeCurveGroupRid == Include.NoRID)	
				{	
					SizeGroupProfile sizeGroup = new SizeGroupProfile(this.SizeGroupRid);
					scl = sizeGroup.SizeCodeList;
				}
				else
				{
					SizeCurveGroupProfile sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
					scl = sizeCurveGroup.SizeCodeList;
				}

                // Begin TT#368 - RMatelic - Workup Buy allow sizes not in size curve 
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                if (!aAllocationProfile.WorkUpTotalBuy && !aAllocationProfile.Placeholder) // TT#1436 - MD - JEllis - GA allocates bulk before packs
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    foreach (int sizeKey in compSizeList)
                    {
                        if (!scl.Contains(sizeKey))
                        {
                            continueProcessing = false;
                            break;
                        }
                    }
                }
                // End TT#368  
							
				//=====================================================================
				// Sizes on the Header and sizes in the Size Curve Group do not match.
				//=====================================================================
				if (!continueProcessing)
				{
					//Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
					//throw new MIDException(eErrorLevel.severe,     // MID Track 5374 Workflow Errors do not stop process
					throw new MIDException(eErrorLevel.error,
					//End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
						(int)eMIDTextCode.msg_al_HeaderSizesDoNotMatchSizeCurve,
                        MIDText.GetText(eMIDTextCode.msg_al_HeaderSizesDoNotMatchSizeCurve) + " Header [" + aAllocationProfile.HeaderID + "] "); // MID Track 5778 Scheduler 'Run Now' feature gets error in audit // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
				}
			}

			//=================================//
			// Allocate Component by Size Need //
			//=================================//
			if (continueProcessing)
			{
				try
				{
					_componentsProcessed++;
					
					SizeCurveGroupProfile sizeCurveGroup;
					if (genSizeCurveGroupRID != Include.NoRID)
					{
						sizeCurveGroup = new SizeCurveGroupProfile(genSizeCurveGroupRID);
					}
					else
					{
						sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
					}
					int sizeConstraintRID = this.SizeConstraintRid;
					if (base.GenericSizeConstraintsDefined())
					{
						try
						{
                            sizeConstraintRID = (int)base.GenConstraintHash[aAllocationProfile.Key]; // TT#702 Inifinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
						}
						catch
						{
                            string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeConstraintForHeader), string.Empty, aAllocationProfile.HeaderID); // TT#702 Inifinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_al_NoSizeConstraintForHeader, errMessage);
						}
					}

                    // begin TT#3744 - MD - Curve Adjd OnHand Inconsistent between 2 group allocation headers with same color
                    //// begin TT#2155 - JELlis - Fill Size Holes Null Reference
                    ////=========================================================
                    //// Set the sizeNeedMethod for this component
                    ////=========================================================
                    //if (aComponent.ComponentType == eComponentType.AllNonGenericPacks)
                    //{
                    //    _allocationProfile.SetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType), this); // TT#702 Inifinite Loop when begin date set
                    //}
                    //else
                    //{
                    //    _allocationProfile.SetSizeNeedMethod(aComponent, this); // TT#702 Inifinite Loop when begin date set
                    //}
                    //// end TT#2155 - JEllis - Fill Size Holes Null Reference
                    // end TT#3744 - MD - Curve Adjd Onhand Inconsistent between 2 group allocation headers with same color

                    // begin TT#702 Infinite Loop when begin date set
					//SizeNeedAlgorithm sna = new SizeNeedAlgorithm(_transaction, eMethodType.SizeNeedAllocation); 
                    _sizeNeedAlgorithm = 
                        new SizeNeedAlgorithm(
                            _transaction, 
                            //eMethodType.SizeNeedAllocation,  // TT#2155 - JEllis - Fill Size Holes Null Reference
                            aAllocationProfile, // TT#1436 - MD - JEllis - GA allocates bulk before packs
                            aComponent,
                            RulesCollection,
                            this._sizeAlternateRid,
                            sizeConstraintRID,
                            sizeCurveGroup,
                            storeList,
                            this._MerchType, 
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
                            this._IB_MERCH_PHL_SEQUENCE, // TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
                            this._vSWSizeConstraints, // TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
                            this._FillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
                            );   
                            //this.NormalizeSizeCurves);
                            // end TT#41 - MD - Jellis - Size inventory Min Max
                    //_transaction.SetSizeNeedResults(_allocationProfile.HeaderRID, _sizeNeedAlgorithm.SizeNeedResults);                  // TT#2155 - JEllis - Fill Size Holes Null Reference  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    //_sizeNeedResultsHash.Add(_sizeNeedAlgorithm.SizeNeedResults.HeaderColorRid, _sizeNeedAlgorithm.SizeNeedResults); // TT#2155 - Jellis - Fill Size Holes Null Reference
                    // end TT#702 Infinite Loop when begin date set

                    // begin TT#2155 - JELlis - Fill Size Holes Null Reference
					//=========================================================
					// Set the sizeNeedMethod for this component
					//=========================================================
                    //if (aComponent.ComponentType == eComponentType.AllNonGenericPacks)
                    //{
                    //    _allocationProfile.SetSizeNeedMethod(new GeneralComponent(eGeneralComponentType.DetailType), this); // TT#702 Inifinite Loop when begin date set
                    //}
                    //else
                    //{
                    //    _allocationProfile.SetSizeNeedMethod(aComponent, this); // TT#702 Inifinite Loop when begin date set
                    //} 
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference

					//================================
					// call Size Need Processing
					// this returns on plan filled in
					//================================

                    // begin TT#702 Infinite Loop when begin date set
                    //_sizeNeedResults = _sizeNeedAlgorithm.ProcessSizeNeed(eSizeProcessControl.ProcessAll, ApplyRulesOnly); // TT#2155 - JEllis - Fill Size Holes Null Reference  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    // begin TT#2155 - JEllis - Fill Size Holes Null Referenct
                    //_transaction.SetSizeNeedResults(_allocationProfile.HeaderRID, _sizeNeedResults);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    //_sizeNeedResultsHash.Remove(_sizeNeedResults.HeaderColorRid);
                    //_sizeNeedResultsHash.Add(_sizeNeedResults.HeaderColorRid, _sizeNeedResults);
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
					//SizeNeedResults sizeResults =  
                        //_sizeNeedAlgorithm.ProcessSizeNeed( // TT#702 Infinite Loop when begin date set
                        //_allocationProfile,  // TT#702 Infinite Loop when begin date set 
                        //aComponent,
                        //sizeCurveGroup, 
                        //storeList,
                        //this._MerchType, this._MerchHnRid, this._MerchPhRid, this._MerchPhlSequence,
                        //this.SG_RID,
                        //sizeConstraintRID, this._sizeAlternateRid, this.RulesCollection,         // MID Track 4372 Generic Size Constraint
                        //eSizeNeedColorPlan.PlanForSpecificColorOnly, eSizeProcessControl.ProcessAll, // MID Track 4861 Size Curve Normalization
                        //this.NormalizeSizeCurves); // MID Track 4861 Size Normalization
                    // end TT#702 Infinite Loop when begin date set

                    SizeNeedResults sizeNeedResults = _sizeNeedAlgorithm.ProcessSizeNeed(eSizeProcessControl.ProcessAll, ApplyRulesOnly); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations

					int storeCnt = storeList.Count;
					if (aComponent.ComponentType == eComponentType.SpecificColor)
					{
						AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)aComponent;
                        HdrColorBin colorBin = aAllocationProfile.GetHdrColorBin(colorComponent.ColorRID);  // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
                        //_sizeNeedResultsHash.Add(colorComponent.ColorRID, _sizeNeedResults);    // TT#702 Infinite Loop when begin date set
						// begin MID track 4967 Size Functions Not Showing Total Qty
						GeneralComponent sizeComponent = new GeneralComponent(eGeneralComponentType.AllSizes);
						AllocationColorSizeComponent acsc = new AllocationColorSizeComponent(colorComponent, sizeComponent);
						this._actionAuditList.Add(new AllocationActionAuditStruct
							((int)eAllocationMethodType.SizeNeedAllocation,
                            aAllocationProfile,    // TT#1199 - MD - NEED allocation # of stores is zero - rbeck  // TT#1436 - MD - JEllis - GA allocates bulk before packs
							acsc,
                            aAllocationProfile.GetQtyAllocated(acsc),                // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
                            aAllocationProfile.GetCountOfStoresWithAllocation(acsc), // TT#702 Infinite Loop when begin date set  // TT#1436 - MD - JEllis - GA allocates bulk before packs
							true,
							false));
						// end MID track 4967 Size Functions Not Showing Total Qty
						for (int i=0;i<storeCnt;i++)
						{
							StoreProfile sp = (StoreProfile)storeList[i];
							SizeCurveProfile scp = sizeCurveGroup.GetStoreSizeCurveProfile(sp.Key);
                            Index_RID storeIndex = (Index_RID)aAllocationProfile.StoreIndex(sp.Key); // TT#702 Size Need Method "hangs" when processing hdrs with begin date // TT#1436 - MD - JEllis - GA allocates bulk before packs
							//BEGIN TT#211-MD - stodd - size information not refreshing after size need is run on placeholder
                            if (aAllocationProfile.WorkUpBulkSizeBuy || _allocationProfile.Placeholder)   // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
							//END TT#211-MD - stodd - size information not refreshing after size need is run on placeholder
							{
								int units;
                                //foreach (int sizeCodeKey in _sizeNeedResults.Sizes) // TT#702 Infinite Loop when begin date set  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                foreach (int sizeCodeKey in sizeNeedResults.Sizes) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                {
                                    //units = _sizeNeedResults.GetAllocatedUnits(sp.Key, sizeCodeKey); // TT#702 Infinite Loop when begin date set  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    units = sizeNeedResults.GetAllocatedUnits(sp.Key, sizeCodeKey); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    if (units > 0)
									{
										if (!colorBin.ColorSizes.ContainsKey(sizeCodeKey))
										{ 
											_allocationProfile.AddBulkSizeToColor(colorBin, sizeCodeKey, 0, 0);    // TT#702 Infinite Loop when begin date set
										}
										_allocationProfile.SetStoreQtyAllocated(colorBin, sizeCodeKey, storeIndex, units, eDistributeChange.ToNone, false); // TT#702 Infinite Loop when begin date set
									}
								}
							}
							else
							{
								foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                {
                                    //    _allocationProfile.SetStoreQtyAllocated(hsb, storeIndex, _sizeNeedResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID), eDistributeChange.ToNone, false); // Assortment: color/size changes  // TT#702 Infinite Loop when begin date set //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    aAllocationProfile.SetStoreQtyAllocated(hsb, storeIndex, sizeNeedResults.GetAllocatedUnits(sp.Key, hsb.SizeCodeRID), eDistributeChange.ToNone, false); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1436 - MD - JEllis - GA allocates bulk before packs
								}
							}
						}
                        aAllocationProfile.BulkSizeBreakoutPerformed = true; // MID Track # 2983 Workflow not processing size need // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
					}
					else if (aComponent.ComponentType == eComponentType.DetailType // MID Track 3294 Invalid Error Message
						|| aComponent.ComponentType == eComponentType.AllNonGenericPacks) // MID Track 3294 Invalid Error Message
					{
                        //_sizeNeedResultsHash.Add(Include.DummyColorRID, _sizeNeedResults); // TT#702 Infinite Loop when begin date set

                        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                        //AllocateNonGenericPacks
                        //    (
                        //    //_transaction,           // TT#702 Infinite Loop when begin date set
                        //    //_allocationProfile,     // TT#702 Infinite Loop when begin date set
                        //    //sizeResults,            // TT#702 Infinite Loop when begin date set
                        //    sizeNeedResults, //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                        //    storeList
                        //    );

                        // Begin TT#1386-MD - stodd - Manual Merge 
                        //DetailPackOptions pOpt = new DetailPackOptions();
                        _pOpt = new DetailPackOptions();
                        _pOpt._storeFilteredList = storeList;
                        _pOpt.maxPackNeedTolerance = this._MaxPackNeedTolerance;
                        _pOpt.avgPackDeviationTolerance = this._AvgPackDeviationTolerance;
                        _pOpt.overrideAvgPackDevTolerance = this._overrideAvgPackDevTolerance;
                        _pOpt.overrideMaxPackNeedTolerance = this._overrideMaxPackNeedTolerance;
                        _pOpt.packToleranceNoMaxStep = this._packToleranceNoMaxStep;
                        _pOpt.packToleranceStepped = this._packToleranceStepped;
                        int totalPackUnitsAllocated = 0;
                        PackProcessing.AllocateNonGenericPacks(sizeNeedResults, _pOpt, _allocationProfile, _transaction, out totalPackUnitsAllocated);
                        // End TT#1386-MD - stodd - Manual Merge 
                        //End TT#1636-MD -jsobek -Pre-Pack Fill Size

						_allocationProfile.PackBreakoutByContent = true; // MID Track # 2983 Size Need not performed in workflow // TT#702 Infinite Loop when begin date set
					}

					// removed unnecessary comments // TT#702 Infinite Loop when begin date set

				}
				catch (Exception ex)
				{
                    _transaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed); // TT#702 Infinite Loop when begin date set // TT#1436 - MD - JEllis - GA allocates bulk before packs
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
					throw;
				}
			}
		}
          
        //// begin TT#1018 - MD - JEllis - Group ALlocation Over Allocates Detail
        //private void AdjustDetailTotals()
        //{
        //    if (_allocationProfile is AssortmentProfile)
        //    {
        //        AssortmentProfile assrtMent = _allocationProfile as AssortmentProfile;
        //        int detailDifference;
        //        int bulkDifference;
        //        int remainingNegativeGap;
        //        List<AllocationProfile> positiveProfile = new List<AllocationProfile>();
        //        List<AllocationProfile> negativeProfile = new List<AllocationProfile>();

        //        foreach (Index_RID storeIdxRID in _allocationProfile.AppSessionTransaction.StoreIndexRIDArray())
        //        {
        //            // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need 
        //            // Adjust Detail first
        //            positiveProfile.Clear();
        //            negativeProfile.Clear();

        //            foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
        //            {
        //                detailDifference =
        //                    ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
        //                    - ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
        //                if (detailDifference < 0)
        //                {
        //                    negativeProfile.Add(ap);
        //                }
        //                else if (detailDifference > 0)
        //                {
        //                    positiveProfile.Add(ap);
        //                }
        //            }
                    
        //            foreach (AllocationProfile negAp in negativeProfile)
        //            {
        //                remainingNegativeGap =
        //                    negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID)
        //                    - negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID);
        //                foreach (AllocationProfile posAp in positiveProfile)
        //                {
        //                    detailDifference =
        //                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
        //                        - posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
        //                    if (detailDifference > 0)
        //                    {
        //                        if (remainingNegativeGap >
        //                            detailDifference)
        //                        {
        //                            posAp.SetStoreQtyAllocated(
        //                                eAllocationSummaryNode.DetailType,
        //                                storeIdxRID,
        //                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID),
        //                                eDistributeChange.ToParent,
        //                                false,
        //                                false,
        //                                false);
        //                            remainingNegativeGap -= detailDifference;
        //                        }
        //                        else
        //                        {
        //                            posAp.SetStoreQtyAllocated(
        //                                eAllocationSummaryNode.DetailType,
        //                                storeIdxRID,
        //                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID) + (detailDifference - remainingNegativeGap),
        //                                eDistributeChange.ToParent,
        //                                false,
        //                                false,
        //                                false);
        //                            remainingNegativeGap = 0;
        //                            break;
        //                        }
        //                    }
        //                    if (remainingNegativeGap == 0)
        //                    {
        //                        break;
        //                    }
        //                }
        //                negAp.SetStoreQtyAllocated(
        //                    eAllocationSummaryNode.DetailType,
        //                    storeIdxRID,
        //                    negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID)
        //                    - remainingNegativeGap,
        //                    eDistributeChange.ToParent,
        //                    false,
        //                    false,
        //                    false);
        //            }


        //            // adjust bulk LAST (It affects the detail totals)
        //            // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
        //            positiveProfile.Clear();
        //            negativeProfile.Clear();
        //            foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
        //            {
        //                bulkDifference =
        //                    ap.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID)
        //                    - ap.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID);
        //                if (bulkDifference < 0)
        //                {
        //                    negativeProfile.Add(ap);
        //                }
        //                else if (bulkDifference > 0)
        //                {
        //                    positiveProfile.Add(ap);
        //                }
        //            }
        //            foreach (AllocationProfile negAp in negativeProfile)
        //            {
        //                remainingNegativeGap =
        //                    negAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID)
        //                    - negAp.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID);
        //                foreach (AllocationProfile posAp in positiveProfile)
        //                {
        //                    bulkDifference =
        //                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID)
        //                        - posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID);
        //                    if (bulkDifference > 0)
        //                    {
        //                        if (remainingNegativeGap >
        //                            bulkDifference)
        //                        {
        //                            posAp.SetStoreQtyAllocated(
        //                                eAllocationSummaryNode.Bulk,
        //                                storeIdxRID,
        //                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID),
        //                                eDistributeChange.ToParent,
        //                                false,
        //                                false,
        //                                false);
        //                            remainingNegativeGap -= bulkDifference;
        //                        }
        //                        else
        //                        {
        //                            posAp.SetStoreQtyAllocated(
        //                                eAllocationSummaryNode.Bulk,
        //                                storeIdxRID,
        //                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID) + (bulkDifference - remainingNegativeGap),
        //                                eDistributeChange.ToParent,
        //                                false,
        //                                false,
        //                                false);
        //                            remainingNegativeGap = 0;
        //                            break;
        //                        }
        //                    }
        //                    // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
        //                    if (remainingNegativeGap == 0)
        //                    {
        //                        break;
        //                    }
        //                    // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
        //                }
        //                negAp.SetStoreQtyAllocated(
        //                    eAllocationSummaryNode.Bulk,
        //                    storeIdxRID,
        //                    negAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID)
        //                    - remainingNegativeGap,
        //                    eDistributeChange.ToParent,
        //                    false,
        //                    false,
        //                    false);
        //            }
        //        }
        //    }
        //}
        //// end TT#1018 - MD - Jellis - Group Allocation Over Allocates Detail

        private bool DoesStoreHaveUnitsToAllocate(GeneralComponent aComponent, int storeKey) // TT#702 Infinite Loop when begin date set
		{
			bool hasUnits = false;
			// Begin MIS Issue # 2971 - stodd
			//int totalSizeAllocated = 0; // MID Track 4121 Size Need Overallocates stores; 
			if (aComponent.ComponentType == eComponentType.SpecificColor)
			{
                int colorUnitsAllocated = _allocationProfile.GetStoreQtyAllocated(aComponent, storeKey); // TT#702 Infinite Loop when begin date set
				GeneralComponent allSizeComponent = new GeneralComponent(eGeneralComponentType.AllSizes);
				AllocationColorSizeComponent colorSizeComponent = new AllocationColorSizeComponent (aComponent, allSizeComponent);
                int allSizesUnitsAllocated = _allocationProfile.GetStoreQtyAllocated(colorSizeComponent, storeKey); // TT#702 Infinite Loop when begin date set
			// End issue # 2971
				if (colorUnitsAllocated > allSizesUnitsAllocated) // MID Track 4121 Size Need Overallocates stores
					hasUnits = true;
			}
			else if (aComponent.ComponentType == eComponentType.DetailType          // MID Track 3294 Invalid Error Message
				|| aComponent.ComponentType == eComponentType.AllNonGenericPacks)   // MID Track 3294 Invalid Error Message
			{
				GeneralComponent detailComponent = new GeneralComponent(eGeneralComponentType.DetailType); // MID Track 3294 invalid Error Message
                int detailTypeAllocated = _allocationProfile.GetStoreQtyAllocated(detailComponent, storeKey); // MID Track 3294 Invalid Error Message // TT#702 Infinite Loop when begin date set
				GeneralComponent subDetailTypeComponent = new GeneralComponent(eGeneralComponentType.DetailSubType);
                int subDetailUnitsAllocated = _allocationProfile.GetStoreQtyAllocated(subDetailTypeComponent, storeKey); // TT#702 Infinite Loop when begin date set
				if (detailTypeAllocated > subDetailUnitsAllocated)
					hasUnits = true;
			}
			return hasUnits;
		}


        // begin TT#702 Infinite loop when begin date set
        //private void AllocateNonGenericPacks(
        //    ApplicationSessionTransaction aTransaction,
        //    AllocationProfile aAllocationProfile, 
        //    SizeNeedResults aSizeNeedResults, 
        //    ProfileList aStoreFilteredList)
        //{
            //_transaction = aTransaction;
            //_allocationProfile = aAllocationProfile;
            //_sizeNeedResults = aSizeNeedResults;
            //_storeFilteredList = aStoreFilteredList;
            //_storeFilterCount = aStoreFilteredList.Count;
            //_packCount = aAllocationProfile.NonGenericPackCount;
            //_sizeCount = aSizeNeedResults.Sizes.Count;
        //private void AllocateNonGenericPacks(
        //    SizeNeedResults aSizeNeedResults, //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    ProfileList aStoreFilteredList)
        //{
        //    _storeFilteredList = aStoreFilteredList;
        //    _storeFilterCount = aStoreFilteredList.Count;
        //    _packCount = _allocationProfile.NonGenericPackCount;
        //    _sizeCount = aSizeNeedResults.Sizes.Count;    //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    //_sizeCount = _sizeNeedResults.Sizes.Count;  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    // end TT#702 Infinite Loop when begin date set

        //    //======================//
        //    //  Build Pack Curves   //
        //    //======================//
        //    _packSizeContent = new int[_packCount, _sizeCount];
        //    _packSizeContent.Initialize();
        //    _packSizeCurvePct = new double[_packCount, _sizeCount];
        //    _packSizeCurvePct.Initialize();
        //    _packsToAllocate = new int[_packCount];
        //    _packMultiple = new int[_packCount];

        //    int sizeIDX;
        //    int packIDX = 0;
        //    int sizeRID;
        //    foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values) // TT#702 Infinite Loop when begin date set
        //    {
        //        int totalSizeUnits = 0;
        //        _packMultiple[packIDX] = ph.PackMultiple;
        //        _packsToAllocate[packIDX] = 
        //            ph.PacksToAllocate - ph.PacksAllocated;
        //        if (_packsToAllocate[packIDX] < 0)
        //        {
        //            _packsToAllocate[packIDX] = 0;
        //        }
        //        foreach (PackColorSize pcs in ph.PackColors.Values)
        //        {
        //            for(sizeIDX=0; sizeIDX < _sizeCount; sizeIDX++)
        //            {
        //                //sizeRID = (int)_sizeNeedResults.Sizes[sizeIDX]; // TT#702 Infinite Loop when begin date set //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //                sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //                if (pcs.SizeIsInColor(sizeRID))
        //                {
        //                    _packSizeContent[packIDX, sizeIDX] += pcs.GetSizeBin(sizeRID).ContentUnits;
        //                    totalSizeUnits += pcs.GetSizeBin(sizeRID).ContentUnits;
        //                }
        //            }
        //        }
        //        // Note:  we are not doing the typical spread to 100 because
        //        // we just require a reasonable estimate to use as a comparison
        //        if (totalSizeUnits > 0)
        //        {
        //            for(sizeIDX=0; sizeIDX < _sizeCount; sizeIDX++)
        //            {
        //                _packSizeCurvePct[packIDX, sizeIDX] = 
        //                    _packSizeContent[packIDX, sizeIDX] * 100 / totalSizeUnits;
        //            }
        //        }
        //        packIDX++;
        //    }

        //    _storeIdxRID = new Index_RID[_storeFilterCount];
        //    _storePrimaryPackIDX = new int[_storeFilterCount];
        //    _storeCandidateCount = new int[_storeFilterCount];
        //    _storeUnitsRemainingToBreakout = new int[_storeFilterCount];
        //    _storeUnitsRemainingToBreakout.Initialize();
        //    _storeTotalNeedError = new int[_storeFilterCount]; 
        //    _storeTotalNeedError.Initialize();
        //    _storeTotalPosUnitNeed = new int[_storeFilterCount];
        //    _storeTotalPosUnitNeed.Initialize();
        //    _storeTotalPosPctNeed = new double[_storeFilterCount];
        //    _storeTotalPosPctNeed.Initialize();
        //    _storeAvgPosUnitNeed = new double[_storeFilterCount];
        //    _storeAvgPosUnitNeed.Initialize();
        //    _storeAvgPosPctNeed = new double[_storeFilterCount];
        //    _storeAvgPosPctNeed.Initialize();
        //    _storePackDeviationErrorDiff = new double[_storeFilterCount];
        //    _storePackDeviationErrorDiff.Initialize();
        //    _storePackNeedErrorDiff = new double[_storeFilterCount];
        //    _storePackNeedErrorDiff.Initialize();
        //    _storePackGapDiff = new double[_storeFilterCount]; 
        //    _storePackGapDiff.Initialize();
        //    _storeDesiredTotalUnits = new double[_storeFilterCount];
        //    _storeDesiredTotalUnits.Initialize();

        //    _storePacksAllocated = new int[_storeFilterCount, _packCount];
        //    _storePacksAllocated.Initialize();
        //    _storePackAvailable = new bool[_storeFilterCount,_packCount];
        //    _storePackAvailable.Initialize();
        //    _storePackCandidate = new bool[_storeFilterCount, _packCount];
        //    _storePackCandidate.Initialize();
        //    _storePackDeviationError= new double[_storeFilterCount, _packCount];
        //    _storePackDeviationError.Initialize();
        //    _storePackDeviationErrorCount = new int[_storeFilterCount, _packCount];
        //    _storePackDeviationErrorCount.Initialize();
        //    _storePackAvgDeviationError = new double[_storeFilterCount, _packCount];
        //    _storePackAvgDeviationError.Initialize();
        //    _storePackNeedError= new double[_storeFilterCount, _packCount];
        //    _storePackNeedError.Initialize();
        //    _storePackGap = new double[_storeFilterCount, _packCount];
        //    _storePackGap.Initialize();
        //    _storeSizePlan = new double[_storeFilterCount, _sizeCount];  // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    _storePackPctNeedMeasure = new double[_storeFilterCount, _packCount]; // TT#2920 -  Jellis - Detail Pack Algorithm Change

        //    _storeSizePosNeed = new double[_storeFilterCount, _sizeCount];
        //    _storeSizePosPctNeed = new double[_storeFilterCount, _sizeCount];
        //    _storeSizeNeedError = new int[_storeFilterCount, _sizeCount];
        //    _storeSizeNeedError.Initialize();

        //    _storeDesiredSizeUnits = new double[_storeFilterCount, _sizeCount];
        //    _storeDesiredSizeUnits.Initialize();
        //    _storeDesiredAllocationCurve = new double[_storeFilterCount, _sizeCount];
        //    _storeDesiredAllocationCurve.Initialize();

        //    int storeFilterIDX; 
        //    StoreProfile storeProfile;
        //    for (storeFilterIDX = 0; storeFilterIDX < _storeFilterCount; storeFilterIDX++)
        //    {
        //        storeProfile = (StoreProfile)_storeFilteredList[storeFilterIDX];
        //        _storeIdxRID[storeFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key); // TT#702 Infinite Loop when begin date set
        //        _storeUnitsRemainingToBreakout[storeFilterIDX] =
        //            _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, _storeIdxRID[storeFilterIDX]) // TT#702 Infinite Loop when begin date set
        //            - _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, _storeIdxRID[storeFilterIDX]); // TT#702 Infinite Loop when begin date set
        //        if (_storeUnitsRemainingToBreakout[storeFilterIDX] < 0)
        //        {
        //            _storeUnitsRemainingToBreakout[storeFilterIDX] = 0;
        //        }
        //        //BuildStorePackCriteria(storeFilterIDX);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        BuildStorePackCriteria(aSizeNeedResults, storeFilterIDX); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    }
        //    // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //    if (_packToleranceStepped
        //        && MaxPackNeedTolerance < Include.MaxPackNeedTolerance + 1) // TT#1410 (related to TT#1412 - FL Pack Allocation Exceeds tolerance)
        //    {
        //        int maxTolerance = (int)MaxPackNeedTolerance + 1; 
        //        int maxPackNeedTolerance = 0;
        //        while (maxPackNeedTolerance < maxTolerance)
        //        {
        //            FitPacksToStores(aSizeNeedResults, maxPackNeedTolerance); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //            maxPackNeedTolerance++;
        //        }
        //    }
        //    else
        //    {
        //        if (MaxPackNeedTolerance < int.MaxValue)   
        //        {
        //            FitPacksToStores(aSizeNeedResults, (int)MaxPackNeedTolerance); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        }                                          // begin TT#1365 part 2
        //        else                   
        //        {
        //            FitPacksToStores(aSizeNeedResults, int.MaxValue); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations    
        //        }                                         // end TT#1365 part 2
        //    }
        //    if (_packToleranceNoMaxStep && MaxPackNeedTolerance < int.MaxValue) // TT#1365 part 2
        //    {
        //        FitPacksToStores(aSizeNeedResults, int.MaxValue); // TT#1365 part 2 //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    }
           
        //    for (storeFilterIDX = 0; storeFilterIDX < _storeFilterCount; storeFilterIDX++)
        //    {
        //        packIDX = 0;
        //        foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
        //        {
        //            // TT#1021 - MD - Jellis - Header Status Wrong
        //            //_allocationProfile.SetStoreQtyAllocated
        //            //    (ph,
        //            //    _storeIdxRID[storeFilterIDX],
        //            //    _storePacksAllocated[storeFilterIDX, packIDX],
        //            //    eDistributeChange.ToNone,
        //            //    false);
        //            if (_storePacksAllocated[storeFilterIDX, packIDX] !=
        //                _allocationProfile.GetStoreQtyAllocated(ph, _storeIdxRID[storeFilterIDX]))
        //            {
        //                _allocationProfile.SetStoreQtyAllocated
        //                    (ph,
        //                    _storeIdxRID[storeFilterIDX],
        //                    _storePacksAllocated[storeFilterIDX, packIDX],
        //                    eDistributeChange.ToNone,
        //                    false);
        //                _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Total, _storeIdxRID[storeFilterIDX], true);
        //                _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, _storeIdxRID[storeFilterIDX], true);
        //                _allocationProfile.SetStoreAllocationFromPackContentBreakOut(ph, _storeIdxRID[storeFilterIDX], true);
        //            }
        //            // TT#1021 - MD - Jellis - Header Status Wrong
        //            packIDX++;
        //        }
        //    }
        //}
        /// <summary>
        /// Determines "best fit" of packs to remaining store detail allocation
        /// </summary>
        /// <param name="aMaxPackNeedTolerance">Maximum Pack Need Tolerance to use for best fit</param>
        //private void FitPacksToStores(SizeNeedResults aSizeNeedResults, int aMaxPackNeedTolerance) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //{
        //    int storeFilterIDX;
        //    int packIDX;
        //    for (storeFilterIDX = 0; storeFilterIDX < _storeFilterCount; storeFilterIDX++)
        //    {
        //        ReBuildStorePackCriteria(aSizeNeedResults, aMaxPackNeedTolerance, storeFilterIDX); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    }
        //    bool candidateStores = false;
        //    bool packsAllocated;
        //    Index_RID storeIdxRID;

        //    ArrayList candidateStoreIDX = new ArrayList();
        //    for (int storeIDX = 0; storeIDX < _storeFilterCount; storeIDX++)
        //    {
        //        storeIdxRID = _storeIdxRID[storeIDX];
        //        if (_storeCandidateCount[storeIDX] > 0
        //            && _storeUnitsRemainingToBreakout[storeIDX] > 0) 
        //        {
        //            candidateStores = true;
        //            candidateStoreIDX.Add(storeIDX);
        //        }
        //    }
        //    if (candidateStores)
        //    {
        //        int sortIDX = 0;
        //        int startSortAtIDX = 0;
        //        int sortEntryCount;
        //        MIDGenericSortItem[] candidateStore = new MIDGenericSortItem[candidateStoreIDX.Count];
        //        foreach (int storeIDX in candidateStoreIDX)
        //        {
        //            candidateStore[sortIDX] = BuildCandidateStore(storeIDX);
        //            sortIDX++;
        //        }
        //        packsAllocated = true;
        //        MIDGenericSortAscendingComparer sac = new MIDGenericSortAscendingComparer();  // TT#1143 - MD- Jellis - Group Allocation - Min broken
        //        while
        //            (startSortAtIDX < candidateStoreIDX.Count
        //            && packsAllocated)
        //        {
        //            packsAllocated = false;
        //            sortEntryCount = candidateStoreIDX.Count - startSortAtIDX;
        //            Array.Sort(candidateStore, startSortAtIDX, sortEntryCount, sac);
        //            while
        //                (startSortAtIDX < candidateStoreIDX.Count
        //                &&
        //                (_storeCandidateCount[candidateStore[startSortAtIDX].Item] <= 0
        //                || _storeUnitsRemainingToBreakout[candidateStore[startSortAtIDX].Item] <= 0
        //                || _storePrimaryPackIDX[candidateStore[startSortAtIDX].Item] < 0)
        //                )
        //            {
        //                startSortAtIDX++;
        //            }
        //            if (startSortAtIDX < candidateStoreIDX.Count)
        //            {
        //                packsAllocated = true;
        //                storeFilterIDX = candidateStore[startSortAtIDX].Item;
        //                packIDX = _storePrimaryPackIDX[storeFilterIDX];
        //                this.UpdateStorePackCriteria(
        //                    aSizeNeedResults, //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //                    aMaxPackNeedTolerance,
        //                    storeFilterIDX,
        //                    packIDX,
        //                    1);
        //                candidateStore[startSortAtIDX] = BuildCandidateStore(storeFilterIDX);
        //                if (_packsToAllocate[packIDX] <= 0)
        //                {
        //                    for (sortIDX = startSortAtIDX + 1; sortIDX < candidateStoreIDX.Count; sortIDX++)
        //                    {
        //                        storeFilterIDX = candidateStore[sortIDX].Item;
        //                        if (packIDX == _storePrimaryPackIDX[storeFilterIDX])
        //                        {
        //                            this.ReBuildStorePackCriteria(aSizeNeedResults, aMaxPackNeedTolerance, storeFilterIDX); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //                            candidateStore[sortIDX] = BuildCandidateStore(storeFilterIDX);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}
        
        //    bool candidateStores = false;
        //    bool packsAllocated;
        //    Index_RID storeIdxRID;

        //    ArrayList candidateStoreIDX = new ArrayList();
        //    for (int storeIDX = 0; storeIDX < _storeFilterCount; storeIDX++)
        //    {
        //        storeIdxRID = _storeIdxRID[storeIDX];
        //        if (_storeCandidateCount[storeIDX] > 0
        //            && _storeUnitsRemainingToBreakout[storeIDX] > 0)
        //        {
        //            candidateStores = true;
        //            candidateStoreIDX.Add(storeIDX);
        //        }
        //    }
        //    if (candidateStores)
        //    {
        //        int sortIDX = 0;
        //        int startSortAtIDX = 0;
        //        int sortEntryCount;
        //        MIDGenericSortItem[] candidateStore = new MIDGenericSortItem[candidateStoreIDX.Count];
        //        foreach (int storeIDX in  candidateStoreIDX)
        //        {
        //            candidateStore[sortIDX] = BuildCandidateStore(storeIDX);
        //            sortIDX++;
        //        }
        //        packsAllocated = true;
        //        SortAscendingComparer sac = new SortAscendingComparer();
        //        while 
        //            (startSortAtIDX < candidateStoreIDX.Count
        //            && packsAllocated)
        //        {
        //            packsAllocated = false;
        //            sortEntryCount = candidateStoreIDX.Count - startSortAtIDX;
        //            Array.Sort(candidateStore,startSortAtIDX, sortEntryCount, sac);
        //            while 
        //                (startSortAtIDX < candidateStoreIDX.Count
        //                && 
        //                (_storeCandidateCount[candidateStore[startSortAtIDX].Item] <= 0
        //                || _storeUnitsRemainingToBreakout[candidateStore[startSortAtIDX].Item] <= 0
        //                || _storePrimaryPackIDX[candidateStore[startSortAtIDX].Item] < 0)
        //                )
        //            {
        //                startSortAtIDX++;
        //            }
        //            if (startSortAtIDX < candidateStoreIDX.Count)
        //            {
        //                packsAllocated = true;
        //                storeFilterIDX = candidateStore[startSortAtIDX].Item;
        //                packIDX = _storePrimaryPackIDX[storeFilterIDX];
        //                this.UpdateStorePackCriteria(
        //                    storeFilterIDX,
        //                    packIDX,
        //                    1);
        //                candidateStore[startSortAtIDX] = BuildCandidateStore(storeFilterIDX);
        //                if (_packsToAllocate[packIDX] <= 0)
        //                {
        //                    for (sortIDX = startSortAtIDX + 1; sortIDX < candidateStoreIDX.Count; sortIDX++)
        //                    {
        //                        storeFilterIDX = candidateStore[sortIDX].Item;
        //                        if (packIDX == _storePrimaryPackIDX[storeFilterIDX])
        //                        {
        //                            this.ReBuildStorePackCriteria(storeFilterIDX);
        //                            candidateStore[sortIDX] = BuildCandidateStore(storeFilterIDX);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    for (storeFilterIDX = 0; storeFilterIDX < _storeFilterCount; storeFilterIDX++)
        //    {
        //        packIDX = 0;
        //        foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
        //        {
        //            _allocationProfile.SetStoreQtyAllocated
        //                (ph, 
        //                _storeIdxRID[storeFilterIDX], 
        //                _storePacksAllocated[storeFilterIDX, packIDX],
        //                eDistributeChange.ToNone,
        //                false);
        //            packIDX++;
        //        }
        //    }
        //}
            // end TT#1365 - JEllis - Detail Pack Size Need Enhancement

        // begin TT#1568 - MD - Jellis -  GA Size need not observing Header min/max
        private void AllocateGroupBulk(AssortmentProfile asrtProfile, AllocationProfile[] aApList, ProfileList aStoreFilterList)
        {
            // _allocationProfile MUST be an AssortmentProfile
			Index_RID storeIdxRID;
			int unitsRemainToAllocate;
            int unitsAllocated;
            int i;
            int maximum;
            int minimum;
            int spreadBasis;
            int bulkHdrUnitsRemain; // TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
            Dictionary<int, int> bulkUnitsAllocatedBy = new Dictionary<int, int>();		// TT#4846 - stodd - Size Need on headers with pack and bulk 
            Dictionary<int, int> bulkStoreUnitsAllocatedBy;  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
            StoreVector sv;
            AllocationProfile memberAp;
            MIDGenericSortItem[] mgsi = new MIDGenericSortItem[aApList.Length]; 
            int[] inventoryList;
            ProfileList storeFilterList = new ProfileList(eProfileType.Store);
            foreach (StoreProfile sp in aStoreFilterList)
            {
                bulkStoreUnitsAllocatedBy = new Dictionary<int, int>();  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                storeIdxRID = _transaction.StoreIndexRID(sp.Key);
                bool unitsToAllocate = true;  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                int attempts = 0;  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 

                unitsRemainToAllocate =
                    asrtProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
                    - asrtProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
                // Begin TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                while (unitsToAllocate)
                {
                    ++attempts;
                    // End TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                    i = 0;
                    spreadBasis = 0;
                    if (unitsRemainToAllocate > 0)
                    {
                        storeFilterList.Add(sp);
                        foreach (AllocationProfile ap in aApList)
                        {
                            // begin TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
                            if (ap.GetStoreOut(eAllocationSummaryNode.Bulk, storeIdxRID))
                            {
                                maximum = 0;
                                minimum = 0;
                                bulkHdrUnitsRemain = 0;
                            }
                            else
                            {
                                // Begin TT#4846 - stodd - Size Need on headers with pack and bulk 
                                //==========================================================================================
                                // Changed this because at this point, no bulk is actually being allocated.
                                // This is simply to set the detail total. The bulk will be allocated after this is done.
                                //==========================================================================================
                                //bulkHdrUnitsRemain = Math.Max(0, ap.BulkUnitsToAllocate - ap.BulkUnitsAllocated);
                                if (bulkUnitsAllocatedBy.ContainsKey(ap.Key))
                                {
                                    bulkHdrUnitsRemain = Math.Max(0, ap.BulkUnitsToAllocate - bulkUnitsAllocatedBy[ap.Key]);
                                }
                                else
                                {
                                    bulkHdrUnitsRemain = Math.Max(0, ap.BulkUnitsToAllocate);
                                }
                                // End TT#4846 - stodd - Size Need on headers with pack and bulk


                                // end TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
                                maximum = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                minimum = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                if (ap.GradeInventoryBasisHnRID != Include.NoRID)
                                {
                                    if (_pOpt != null && _pOpt._inventoryBasisAllocation.TryGetValue(ap.GradeInventoryBasisHnRID, out sv))  // TT#1386-MD - stodd - Manual Merge 
                                    {
                                        maximum =
                                            Math.Max(
                                                 0,
                                                 maximum - (int)sv.GetStoreValue(storeIdxRID.RID));
                                        minimum =
                                            Math.Max(
                                                 0,
                                                 minimum - (int)sv.GetStoreValue(storeIdxRID.RID));
                                    }
                                }
                                // Begin TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                else
                                {
                                    // End TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                    // Begin TT#4880 - stodd - Header Max not being held after size need processed
                                    int prevUnitsAllocated = ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
                                    minimum = Math.Max(0, minimum - prevUnitsAllocated);   // TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                    maximum = Math.Max(0, maximum - prevUnitsAllocated);
                                    // Begin TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                }
                                // End TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                if (minimum > maximum)
                                {
                                    minimum = maximum;
                                }
                                // End TT#4880 - stodd - Header Max not being held after size need processed

                            }  // TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
                            mgsi[i].Item = i;
                            mgsi[i].SortKey = new double[4];
                            mgsi[i].SortKey[0] = maximum;
                            mgsi[i].SortKey[1] = Math.Max(0, maximum - minimum);
                            //mgsi[i].SortKey[2] = Math.Max(0, ap.BulkUnitsToAllocate - ap.BulkUnitsAllocated); // TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
                            mgsi[i].SortKey[2] = bulkHdrUnitsRemain; // TT#1594 - MD - Jellis - Size Need Allocates to OUT stores
                            mgsi[i].SortKey[3] = ap.AppSessionTransaction.GetRandomDouble();
                            spreadBasis += (int)mgsi[i].SortKey[2];
                            i++;
                        }
                        Array.Sort(mgsi, new MIDGenericSortAscendingComparer());

                        foreach (MIDGenericSortItem sortItem in mgsi)
                        {
                            memberAp = aApList[sortItem.Item];
                            maximum = memberAp.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            minimum = memberAp.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            if (memberAp.GradeInventoryBasisHnRID != Include.NoRID)
                            {
                                if (_pOpt != null && _pOpt._inventoryBasisAllocation.TryGetValue(memberAp.GradeInventoryBasisHnRID, out sv))    // TT#1386-MD - stodd - Manual Merge 
                                {
                                    maximum =
                                        Math.Max(
                                             0,
                                             maximum - (int)sv.GetStoreValue(storeIdxRID.RID));
                                    minimum =
                                        Math.Max(
                                             0,
                                             minimum - (int)sv.GetStoreValue(storeIdxRID.RID));
                                }
                            }
                            // Begin TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                            else
                            {
                                // End TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                                // Begin TT#4880 - stodd - Header Max not being held after size need processed
                                int prevUnitsAllocated = memberAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
                                // Begin TT#4962 - JSmith - Header min not held after size need processed on group
                                // adjust minimum by previously allocated pack quantities
                                minimum = Math.Max(0, minimum - prevUnitsAllocated);
                                // End TT#4962 - JSmith - Header min not held after size need processed on group
                                maximum = Math.Max(0, maximum - prevUnitsAllocated);
                                // Begin TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                            }
                            // End TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                            if (minimum > maximum)
                            {
                                minimum = maximum;
                            }
                            // End TT#4880 - stodd - Header Max not being held after size need processed

                            if (spreadBasis > 0)
                            {
                                unitsAllocated =
                                    (int)((sortItem.SortKey[2]
                                    * (double)unitsRemainToAllocate
                                    / (double)spreadBasis) + .5);
                                unitsAllocated =
                                   ((int)(((double)unitsAllocated
                                    / (double)memberAp.AllocationMultiple) + .5))
                                   * memberAp.AllocationMultiple;
                                if (unitsAllocated > maximum)
                                {
                                    unitsAllocated = maximum;
                                }
                                if (unitsAllocated < minimum)
                                {
                                    unitsAllocated = 0;
                                }
                            }
                            else
                            {
                                unitsAllocated = 0;
                            }
                            // Begin TT#4846 - stodd - Size Need on headers with pack and bulk
                            if (bulkUnitsAllocatedBy.ContainsKey(memberAp.Key))
                            {
                                bulkUnitsAllocatedBy[memberAp.Key] += unitsAllocated;
                            }
                            else
                            {
                                bulkUnitsAllocatedBy.Add(memberAp.Key, unitsAllocated);
                            }
                            // Begin TT#4846 - stodd - Size Need on headers with pack and bulk

                            // Begin TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                            if (bulkStoreUnitsAllocatedBy.ContainsKey(memberAp.Key))
                            {
                                bulkStoreUnitsAllocatedBy[memberAp.Key] += unitsAllocated;
                            }
                            else
                            {
                                bulkStoreUnitsAllocatedBy.Add(memberAp.Key, unitsAllocated);
                            }
                            // End TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 

                            memberAp.SetStoreQtyAllocated(
                                eAllocationSummaryNode.DetailType,
                                storeIdxRID,
                                memberAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID)
                                //+ unitsAllocated,  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                                + bulkStoreUnitsAllocatedBy[memberAp.Key],  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                                eDistributeChange.ToParent,
                                false,
                                false,
                                false);
                            if (memberAp.HeaderColorCodeRID != Include.IntransitKeyTypeNoColor)
                            {
                                inventoryList = asrtProfile.GetInventoryUpdateList(memberAp.StyleHnRID, memberAp.HeaderColorCodeRID, false);
                                foreach (int inventoryRID in inventoryList)
                                {
                                    if (_pOpt != null && _pOpt._inventoryBasisAllocation.TryGetValue(inventoryRID, out sv))     // TT#1386-MD - stodd - Manual Merge 
                                    {
                                        sv.SetStoreValue(
                                            storeIdxRID.RID,
                                            sv.GetStoreValue(storeIdxRID.RID)
                                            + unitsAllocated);
                                    }
                                }
                            }
                            spreadBasis -= (int)sortItem.SortKey[2];
                            unitsRemainToAllocate -= unitsAllocated;
                        }  
                    }

                    // Begin TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                    if (unitsRemainToAllocate == 0 ||
                            attempts > 10)
                    {
                        unitsToAllocate = false;
                    }
                    // End TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
                }  // TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance. 
            }

            if (storeFilterList.Count > 0)
            {
                foreach (AllocationProfile ap in aApList)
                {
                    if (ap.BulkColors.Count > 0)
                    {
						// Begin TT#1386-MD - stodd - Manual Merge
                        //AllocateBulkAfterPacks(ap, storeFilterList);
                        PackProcessing.AllocateBulkAfterPacks(ap, _transaction, storeFilterList);
						// End TT#1386-MD - stodd - Manual Merge
                    }
                }
            }
        }
        // end TT#1568 - MD - Jellis -  GA Size need not observing Header min/max

        // begin TT#702 Infinite Loop when begin date set
        //// BEGIN MID Track 3122 Bulk not allocated when bulk and sized packs on same header
        ///// <summary>
        ///// Allocates bulk units after "sized" pack units have been allocated
        ///// </summary>
        ///// <param name="aTransaction">ApplicationSessionTransaction</param>
        ///// <param name="aAllocationProfile">AllocationProfile</param>
        ///// <param name="aStoreFilterList">Profile list of stores</param>
        //private void AllocateBulkAfterPacks(
        //    ApplicationSessionTransaction aTransaction,
        //    AllocationProfile aAllocationProfile,
        //    ProfileList aStoreFilterList)
        ///// <summary>
        ///// Allocates bulk units after "sized" pack units have been allocated
        ///// </summary>
        ///// <param name="aStoreFilterList">Profile list of stores</param>
        //private void AllocateBulkAfterPacks(
        //    ProfileList aStoreFilterList)
        //    // end TT#702 Infinite Loop when begin date set
        //{
        //    int unitsToAllocate;
        //    Index_RID storeIdxRID;
        //    int unitsRemainToAllocate;
        //    int unitsAllocated; // TT#1021 - MD - Jellis - Header Status Wrong
        //    foreach (StoreProfile sp in aStoreFilterList)
        //    {
        //        storeIdxRID = _transaction.StoreIndexRID(sp.Key); // TT#702 Infinite loop when begin date set
        //        unitsToAllocate =
        //            _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID) // TT#702 Infinite loop when begin date set
        //            - _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID); // TT#702 Infinite loop when begin date set
        //        unitsRemainToAllocate =
        //            _allocationProfile.SpreadStoreBulkRuleAllocation(_allocationProfile, storeIdxRID, unitsToAllocate, true); // TT#702 Infinite loop when begin date set  // TT#488 - MD - Jellis - Group Allocation
        //        // begin TT#1021 - MD - Jellis - Header Status Wrong
        //        //_allocationProfile.SetStoreQtyAllocated // TT#702 Infinite loop when begin date set
        //        //     (
        //        //     eAllocationSummaryNode.Bulk,
        //        //     storeIdxRID,
        //        //     unitsToAllocate - unitsRemainToAllocate + _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID), // TT#702 Infinite loop when begin date set
        //        //     eDistributeChange.ToNone,
        //        //     false
        //        //     );
        //        unitsAllocated = 
        //            unitsToAllocate 
        //            - unitsRemainToAllocate;
        //        if (unitsAllocated > 0)
        //        {
        //            _allocationProfile.SetStoreQtyAllocated
        //                (
        //                eAllocationSummaryNode.Bulk,
        //                storeIdxRID,
        //                unitsAllocated
        //                + _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID),
        //                eDistributeChange.ToNone,
        //                false
        //                );
        //            _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Total, storeIdxRID, true);
        //            _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, storeIdxRID, true);
        //            _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Bulk, storeIdxRID, true);
        //        }
        //        // TT#1021 - MD - Jellis - Header Status Wrong
        //    }
        //}
        //// END MID Track 3122 Bulk not allocated when bulk and sized packs on same header

        //private MIDGenericSortItem BuildCandidateStore(int aStoreIDX)
        //{
        //    MIDGenericSortItem candidateStoreSortEntry = new MIDGenericSortItem();
        //    candidateStoreSortEntry.Item = aStoreIDX;			
        //    candidateStoreSortEntry.SortKey = new double[13]; // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    candidateStoreSortEntry.SortKey[0] = - _storeAvgPosPctNeed[aStoreIDX];
        //    candidateStoreSortEntry.SortKey[01] = - _storeAvgPosUnitNeed[aStoreIDX];
        //    if (_storeCandidateCount[aStoreIDX] > 1)
        //    {									
        //        candidateStoreSortEntry.SortKey[02] = 2;
        //    }
        //    else 
        //    {
        //        candidateStoreSortEntry.SortKey[02] = _storeCandidateCount[aStoreIDX];
        //    }
        //    if (_storePrimaryPackIDX[aStoreIDX] < 0)
        //    {
        //        candidateStoreSortEntry.SortKey[03] = double.MinValue;
        //        candidateStoreSortEntry.SortKey[06] = double.MinValue;
        //        candidateStoreSortEntry.SortKey[07] = double.MinValue;
        //        candidateStoreSortEntry.SortKey[08] = double.MinValue;
        //        candidateStoreSortEntry.SortKey[09] = double.MinValue;
        //        // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
        //        candidateStoreSortEntry.SortKey[10] = double.MinValue;
        //        // end TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    }
        //    else
        //    {
        //        candidateStoreSortEntry.SortKey[03] = _storePackDeviationError[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]];  //  TT#1410 - FL System Not Allocating ENOUGH packs within given variance - should be ascending sequence not descending
        //        //candidateStoreSortEntry.SortKey[03] = -_storePackDeviationError[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]]; // TT#1410 - FL System Not Allocting ENOUGH packs within given variance - should be ascending sequence not descending
        //        candidateStoreSortEntry.SortKey[06] = _storePackAvgDeviationError[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]];
        //        candidateStoreSortEntry.SortKey[07] = _storePackNeedError[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]];
        //        candidateStoreSortEntry.SortKey[08] = _storePackPctNeedMeasure[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]]; // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //        candidateStoreSortEntry.SortKey[09] = _storePackGap[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]];  // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //        candidateStoreSortEntry.SortKey[10] = -_packMultiple[_storePrimaryPackIDX[aStoreIDX]];           // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    }
        //    candidateStoreSortEntry.SortKey[04] = - _storePackNeedErrorDiff[aStoreIDX];
        //    candidateStoreSortEntry.SortKey[05] = - _storePackGapDiff[aStoreIDX];
        //    // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    candidateStoreSortEntry.SortKey[11] = _storeCandidateCount[aStoreIDX];
        //    candidateStoreSortEntry.SortKey[12] = _transaction.GetRandomDouble();
        //    //candidateStoreSortEntry.SortKey[10] = _storeCandidateCount[aStoreIDX];
        //    //candidateStoreSortEntry.SortKey[11] = _transaction.GetRandomDouble();
        //    // end TT#2920 -  Jellis - Detail Pack Algorithm Change
        //    return candidateStoreSortEntry;
        //}

        //private void BuildStorePackCriteria(int aStoreFilterIDX)  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //private void BuildStorePackCriteria(SizeNeedResults aSizeNeedResults, int aStoreFilterIDX) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //{
        //    StoreProfile storeProfile = (StoreProfile)_storeFilteredList.ArrayList[aStoreFilterIDX];
        //    int sizeRID;
        //    int sizeIDX;
        //    int packIDX;
        //    for (sizeIDX = 0; sizeIDX < _sizeCount; sizeIDX++)
        //    {
        //        sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX];  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] =
        //            aSizeNeedResults.GetAllocatedUnits(storeProfile.Key, sizeRID);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        _storeDesiredTotalUnits[aStoreFilterIDX] += 
        //            (int)_storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
        //        _storeSizePosNeed[aStoreFilterIDX, sizeIDX] =
        //            aSizeNeedResults.GetSizeNeedUnits(storeProfile.Key, sizeRID, true); // TT#2332 - Jellis - Pack Fitting Algorithm Broken  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        _storeSizePlan[aStoreFilterIDX, sizeIDX] = // TT#2920 -  Jellis - Detail Pack Algorithm Change
        //            aSizeNeedResults.GetSizeNeed_PlanUnits(storeProfile.Key, sizeRID); // TT#2920 -  Jellis - Detail Pack Algorithm Change  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    }
        //    packIDX = 0;
        //    foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
        //    {
        //        _storePacksAllocated[aStoreFilterIDX, packIDX] =
        //            _allocationProfile.GetStoreQtyAllocated(ph, _storeIdxRID[aStoreFilterIDX]);

        //        _storePackCandidate[aStoreFilterIDX, packIDX] = false;
        //        if (_packsToAllocate[packIDX] <= 0
        //            || ph.PackMultiple > _storeUnitsRemainingToBreakout[aStoreFilterIDX]
        //            || _allocationProfile.GetStoreIsManuallyAllocated(ph, _storeIdxRID[aStoreFilterIDX])
        //            || _allocationProfile.GetStoreOut(ph, _storeIdxRID[aStoreFilterIDX])
        //            || _allocationProfile.GetStoreLocked(ph, _storeIdxRID[aStoreFilterIDX])
        //            //|| _allocationProfile.GetStoreTempLock(ph, _storeIdxRID[aStoreFilterIDX])      // TT#421 Detail Packs fail to allocate due to temp locks
        //            || !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation),(int)_allocationProfile.GetStoreChosenRuleType(ph, _storeIdxRID[aStoreFilterIDX]))
        //            )
        //        {
        //            _storePackAvailable[aStoreFilterIDX, packIDX] = false;
        //        }
        //        else
        //        {
        //            _storePackAvailable[aStoreFilterIDX, packIDX] = true;
        //        }
        //        packIDX++;
        //    }
        //    //ReBuildStorePackCriteria(aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement  (moved as 1st thing to do in FitPacksToStores)
        //}

        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //private void ReBuildStorePackCriteria(int aStoreFilterIDX)
        ///// <summary>
        ///// ReBuilds a given store's pack criteria based on the current Maximum Pack Need Tolerance 
        ///// </summary>
        ///// <param name="aMaxPackNeedTolerance">Max Pack Need Tolerance (Ship Variance).  This value varies from 0 upto the specified maximum when the tolerance is "stepped"; does not vary when "stepped" is off</param>
        ///// <param name="aStoreFilterIDX">Identifies the store</param>
        //private void ReBuildStorePackCriteria(SizeNeedResults aSizeNeedResults, int aMaxPackNeedTolerance, int aStoreFilterIDX) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //{
        //    StoreProfile storeProfile = (StoreProfile)_storeFilteredList.ArrayList[aStoreFilterIDX];
        //    int sizeRID;
        //    int sizeIDX;
        //    int sizesWithPosNeed = 0;
        //    double planUnits;
        //    _storeTotalNeedError[aStoreFilterIDX] = 0;
        //    _storeTotalPosPctNeed[aStoreFilterIDX] = 0;
        //    _storeTotalPosUnitNeed[aStoreFilterIDX] = 0;
        //    _storeAvgPosUnitNeed[aStoreFilterIDX] = 0;
        //    _storeAvgPosPctNeed[aStoreFilterIDX] = 0;
        //    _storePrimaryPackIDX[aStoreFilterIDX] = Include.NoRID;
        //    int sizeNeedError;  // TT#1410 - OVER ALLOCATE MAX TOLERANCE
        //    for (sizeIDX = 0; sizeIDX < _sizeCount; sizeIDX++)
        //    {
        //        sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //        if (_storeSizePosNeed[aStoreFilterIDX, sizeIDX] > 0)
        //        {
        //            sizeNeedError = 0; // TT#1410 - OVER ALLOCATE MAX TOLERANCE
        //            sizesWithPosNeed++;
        //            _storeTotalPosUnitNeed[aStoreFilterIDX] += (int)_storeSizePosNeed[aStoreFilterIDX, sizeIDX];
        //            _storeSizeNeedError[aStoreFilterIDX, sizeIDX] = 0;
        //            planUnits = aSizeNeedResults.GetSizeNeed_PlanUnits(storeProfile.Key, sizeRID);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //            if (planUnits > 0)
        //            {
        //                _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] =
        //                    (double)_storeSizePosNeed[aStoreFilterIDX, sizeIDX] * 100.0d
        //                    / planUnits;
        //                _storeTotalPosPctNeed[aStoreFilterIDX] += _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX];
        //            }
        //            else
        //            {
        //                _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] = 0;
        //            }
        //        }
        //        else
        //        {
        //            sizeNeedError = Math.Abs((int)_storeSizePosNeed[aStoreFilterIDX, sizeIDX]); // TT#1410 OVER ALLCOATE MAX TOLERANCE
        //            //_storeSizeNeedError[aStoreFilterIDX, sizeIDX] =                           // TT#1410 OVER ALLCOATE MAX TOLERANCE 
        //            //    Math.Abs((int)_storeSizePosNeed[aStoreFilterIDX, sizeIDX]);           // TT#1410 OVER ALLCOATE MAX TOLERANCE
        //            _storeSizePosNeed[aStoreFilterIDX, sizeIDX] = 0;
        //            _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] = 0;
        //        }
        //        // NOTE:  we just want a reasonable percent to total to use for comparison purposes
        //        // to find the best fit packs. 
        //        _storeSizeNeedError[aStoreFilterIDX, sizeIDX] += sizeNeedError;               // TT#1410 OVER ALLOCATE MAX TOLERANCE
        //        _storeTotalNeedError[aStoreFilterIDX] += _storeSizeNeedError[aStoreFilterIDX, sizeIDX]; 
        //        if (_storeDesiredTotalUnits[aStoreFilterIDX] > 0)
        //        {
        //            _storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX] =
        //                _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX]
        //                * 100 / _storeDesiredTotalUnits[aStoreFilterIDX];
        //        }
        //        else
        //        {
        //            _storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX] = 0;
        //        }
        //    }

        //    if (sizesWithPosNeed > 0)
        //    {
        //        _storeAvgPosUnitNeed[aStoreFilterIDX] = 
        //            (double) _storeTotalPosUnitNeed[aStoreFilterIDX]
        //            / (double) sizesWithPosNeed;
        //        _storeAvgPosPctNeed[aStoreFilterIDX] =
        //            (double) _storeTotalPosPctNeed[aStoreFilterIDX]
        //            / (double) sizesWithPosNeed;
        //    }
        //    IdentifyCandidatePacks(aSizeNeedResults, aMaxPackNeedTolerance, aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    //IdentifyCandidatePacks(aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //}
        
        
        // begin TT#1365 - JEllis - FL Detail Pack SIze Need Enhancement
        //private void UpdateStorePackCriteria(int aStoreFilterIDX, int aPackIDX, int aPacksAllocated)
        ///// <summary>
        ///// Updates a store's pack criteria (typically after a store has been allocated a pack)
        ///// </summary>
        ///// <param name="aMaxPackNeedTolerance">Maximum Pack Need Tolerance (Ship Variance). This varies when stepped tolerances are active (ie. the tolerances are varied from 0 to maximum specified tolerance)</param>
        ///// <param name="aStoreFilterIDX">Identifies the store</param>
        ///// <param name="aPackIDX">Identifies the pack that was allocated</param>
        ///// <param name="aPacksAllocated">Number of packs allocated</param>
        //private void UpdateStorePackCriteria(SizeNeedResults aSizeNeedResults, int aMaxPackNeedTolerance, int aStoreFilterIDX, int aPackIDX, int aPacksAllocated) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //    // end TT#1365 - JEllis - FL Detail Pack Size Need ENhancement
        //{
        //    _packsToAllocate[aPackIDX] -= aPacksAllocated;
        //    _storePacksAllocated[aStoreFilterIDX, aPackIDX] += aPacksAllocated;
        //    _storeUnitsRemainingToBreakout[aStoreFilterIDX] -=
        //        aPacksAllocated
        //        * _packMultiple[aPackIDX];
        //    int sizeIDX;
        //    int packSizeUnitsAllocated; // TT#1410 - FL Packs Allocated Exceed Tolerance
        //    for(sizeIDX = 0; sizeIDX < _sizeCount; sizeIDX++)
        //    {
        //        // begin TT#1410 - FL Packs Allocated Exceed Tolerance
        //        packSizeUnitsAllocated =
        //            _packSizeContent[aPackIDX, sizeIDX]
        //            * aPacksAllocated;
        //        if (packSizeUnitsAllocated >
        //            _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
        //        //if (_packSizeContent[aPackIDX, sizeIDX] > 
        //        //    _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
        //        // end TT#1410 - FL Packs Allocated Exceed Tolerance
        //        {
        //            _storeDesiredTotalUnits[aStoreFilterIDX] -= _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
        //            _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] = 0;
        //        }
        //        else
        //        {
        //            // begin TT#1410 - FL Packs Allocated Exceed Tolerance
        //            _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] -=
        //                packSizeUnitsAllocated;
        //            _storeDesiredTotalUnits[aStoreFilterIDX] -=
        //                packSizeUnitsAllocated;
        //        }
        //        _storeSizePosNeed[aStoreFilterIDX, sizeIDX] -=
        //            packSizeUnitsAllocated;  
        //    }
        //    ReBuildStorePackCriteria(aSizeNeedResults, aMaxPackNeedTolerance, aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //}
                    //_storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] -= 
                    //    _packSizeContent[aPackIDX, sizeIDX]
                    //    * aPacksAllocated;
                    //_storeDesiredTotalUnits[aStoreFilterIDX] -= 
                    //    _packSizeContent[aPackIDX, sizeIDX]
                    //    * aPacksAllocated;
                //}
                //_storeSizePosNeed[aStoreFilterIDX, sizeIDX] -= 
                //    aPacksAllocated
                //    * _packSizeContent[aPackIDX, sizeIDX];
                // end TT#1410 - FL Packs Allocated Exceed Tolerance
            //}
            //ReBuildStorePackCriteria(aMaxPackNeedTolerance, aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            // end TT#1410 - FL Packs Allocated Exceed Tolerance
            //ReBuildStorePackCriteria(aStoreFilterIDX);                      // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
//			if (_packsToAllocate[aPackIDX] < 1)
//			{
//				for(int storeFilterIDX = 0; storeFilterIDX < _storeFilterCount; storeFilterIDX++)
//				{
//					if (storeFilterIDX != aStoreFilterIDX
//						&& _storePackAvailable[storeFilterIDX, aPackIDX])
//					{
//						ReBuildStorePackCriteria(storeFilterIDX);
//						IdentifyCandidatePacks(storeFilterIDX);
//					}
//				}
//			}
        //}  
		// end TT#1410 - FL Packs Allocated Exceed Tolerance
        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //private void IdentifyCandidatePacks(int aStoreFilterIDX)
        ///// <summary>
        ///// Identifies candidate packs
        ///// </summary>
        ///// <param name="aMaxPackNeedTolerance">Max Pack Need Tolerance (Ship Variance). For Stepped tolerances, this varies from 0 upto the specified maximum depending on which "step" the algorithm is on</param>
        ///// <param name="aStoreFilterIDX">Identifies the store</param>
        //private void IdentifyCandidatePacks(SizeNeedResults aSizeNeedResults, int aMaxPackNeedTolerance, int aStoreFilterIDX) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //{
        //    int packIDX = 0;
        //    int sizeIDX;
        //    int sizeRID;
        //    int sortIDX;
        //    int potentialCandidateCount = 0;
        //    foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
        //    {
        //        _storePackCandidate[aStoreFilterIDX, packIDX] = false;
        //        if (_storePackAvailable[aStoreFilterIDX, packIDX])
        //        {
        //            if (_packsToAllocate[packIDX] <= 0
        //                || ph.PackMultiple > _storeUnitsRemainingToBreakout[aStoreFilterIDX]
        //                )
        //            {
        //                _storePackAvailable[aStoreFilterIDX, packIDX] = false;
        //            }
        //            else
        //            {
        //                _storePackGap[aStoreFilterIDX, packIDX] = -1;
        //                _storePackPctNeedMeasure[aStoreFilterIDX, packIDX] = 0;  // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                _storePackDeviationError[aStoreFilterIDX, packIDX] = 0;
        //                _storePackDeviationErrorCount[aStoreFilterIDX, packIDX] = 0;
        //                _storePackNeedError[aStoreFilterIDX, packIDX] = 0;
        //                int storeNeedErrorInPackSizes = 0;  // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                for (sizeIDX = 0; sizeIDX < _sizeCount; sizeIDX++)
        //                {
        //                    sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //                    if (_packSizeContent[packIDX, sizeIDX] >
        //                        this._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
        //                        //							_sizeNeedResults.GetAllocatedUnits(_storeIdxRID[aStoreFilterIDX].RID, sizeRID))
        //                    {
        //                        _storePackDeviationError[aStoreFilterIDX, packIDX] +=
        //                            _packSizeContent[packIDX, sizeIDX]
        //                            - _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
        //                        //								- _sizeNeedResults.GetAllocatedUnits(_storeIdxRID[aStoreFilterIDX].RID, sizeRID);
        //                        _storePackDeviationErrorCount[aStoreFilterIDX, packIDX]++;
        //                    }
        //                    if (_packSizeContent[packIDX, sizeIDX] >
        //                        _storeSizePosNeed[aStoreFilterIDX, sizeIDX])
        //                    {
        //                        _storePackNeedError[aStoreFilterIDX, packIDX] += 
        //                            _packSizeContent[packIDX, sizeIDX]
        //                            - _storeSizePosNeed[aStoreFilterIDX, sizeIDX];
        //                    }
        //                    // begin TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    if (_packSizeContent[packIDX, sizeIDX] > 0)
        //                    {
        //                        storeNeedErrorInPackSizes += _storeSizeNeedError[aStoreFilterIDX, sizeIDX];
        //                        //if (_storeSizePlan[aStoreFilterIDX, packIDX] > 0)  // TT#1106 - MD - Jellis - Size Need - Index Out of Range
        //                        if (_storeSizePlan[aStoreFilterIDX, sizeIDX] > 0)    // TT#1106 - MD - Jellis- Size Need - Index Out of Range
        //                        {
        //                            _storePackPctNeedMeasure[aStoreFilterIDX, packIDX] +=
        //                                Math.Abs(
        //                                    _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX]
        //                                    - (100 * (_storeSizePosNeed[aStoreFilterIDX, sizeIDX] - _packSizeContent[packIDX, sizeIDX])
        //                                        / _storeSizePlan[aStoreFilterIDX, sizeIDX])
        //                                        );
        //                        }
        //                    }
        //                    // Begin TT#4109 - JSmith - Size Need Method-> Pack Tolerance= 0 ; results ignore the 12 pc pack
        //                    else
        //                    {
        //                        if (_storeSizePlan[aStoreFilterIDX, sizeIDX] > 0)    // TT#1106 - MD - Jellis- Size Need - Index Out of Range
        //                        {
        //                            _storePackPctNeedMeasure[aStoreFilterIDX, packIDX] +=
        //                                    _storeSizePosPctNeed[aStoreFilterIDX, sizeIDX];
        //                        }
        //                    }
        //                    // End TT#4109 - JSmith - Size Need Method-> Pack Tolerance= 0 ; results ignore the 12 pc pack
        //                    // end TT#2920 - Jellis - Detail Pack Algorithm Change
        //                }
        //                if (_storePackDeviationErrorCount[aStoreFilterIDX, packIDX] > 0)
        //                {
        //                    _storePackAvgDeviationError[aStoreFilterIDX, packIDX] =
        //                        (double)_storePackDeviationError[aStoreFilterIDX, packIDX]
        //                        / (double)_storePackDeviationErrorCount[aStoreFilterIDX, packIDX];
        //                }
        //                else
        //                {
        //                    _storePackAvgDeviationError[aStoreFilterIDX, packIDX] = 0;
        //                }
        //                if (_storePackAvgDeviationError[aStoreFilterIDX, packIDX] <=
        //                    this.AvgPackDeviationTolerance
        //                    && 
        //                    (
        //                    (_storePackNeedError[aStoreFilterIDX, packIDX]
        //                    // begin TT#2920 - Jellis - Detail Pack Algorithm Change
        //                        + storeNeedErrorInPackSizes <=
        //                        aMaxPackNeedTolerance
        //                    )))
        //                    // + _storeTotalNeedError[aStoreFilterIDX]) <=
        //                    //aMaxPackNeedTolerance       // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //                    ////this.MaxPackNeedTolerance // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        //                    //)))
        //                    // end TT#2920 - Jellis - Detail Pack Algorithm Change
        //                {
        //                    _storePackCandidate[aStoreFilterIDX, packIDX] = true;
        //                    potentialCandidateCount++;
        //                }
        //            }
        //        }
        //        packIDX++;
        //    }
        //    if (potentialCandidateCount > 0)
        //    {
        //        MIDGenericSortItem[] sortedPack = new MIDGenericSortItem[potentialCandidateCount];
        //        packIDX = 0;
        //        sortIDX = 0;
        //        for(packIDX=0; packIDX < _packCount; packIDX++)
        //        {
        //            if (_storePackCandidate[aStoreFilterIDX, packIDX])
        //            {
        //                sortedPack[sortIDX].Item = packIDX;
        //                sortedPack[sortIDX].SortKey = new double[6];  // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                sortedPack[sortIDX].SortKey[0] = _storePackDeviationError[aStoreFilterIDX, packIDX];
        //                sortedPack[sortIDX].SortKey[1] = _storePackNeedError[aStoreFilterIDX, packIDX];
        //                sortedPack[sortIDX].SortKey[2] = _storePackPctNeedMeasure[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                sortedPack[sortIDX].SortKey[3] = -_packMultiple[packIDX];  // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                sortedPack[sortIDX].SortKey[4] = -_packsToAllocate[packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                sortedPack[sortIDX].SortKey[5] = _transaction.GetRandomDouble();
        //                sortIDX++;
        //            }
        //            _storePackCandidate[aStoreFilterIDX, packIDX] = false;
        //        }
        //        Array.Sort(sortedPack, new MIDGenericSortAscendingComparer());  // TT#1143 - MD - Jellis - Group ALlocation Min Broken
        //        packIDX = sortedPack[0].Item;
        //        int targetPackNeedError = (int)_storePackNeedError[aStoreFilterIDX, packIDX];
        //        double targetPackAvgDeviationError = _storePackAvgDeviationError[aStoreFilterIDX, packIDX];
        //        _storeCandidateCount[aStoreFilterIDX] = 0;
        //        double totalCurveUnits; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //        for(sortIDX=0; sortIDX < potentialCandidateCount; sortIDX++)
        //        {
        //            packIDX = sortedPack[sortIDX].Item;
        //            if (_storePackNeedError[aStoreFilterIDX, packIDX] == targetPackNeedError
        //                && _storePackAvgDeviationError[aStoreFilterIDX, packIDX] == targetPackAvgDeviationError)
        //            {
        //                _storePackCandidate[aStoreFilterIDX, packIDX] = true;
        //                _storeCandidateCount[aStoreFilterIDX]++;
        //            }
        //            _storePackGap[aStoreFilterIDX, packIDX] = 0;
        //            // begin TT#2920 -  Jellis - Detail Pack Algorithm Change 
        //            totalCurveUnits =
        //                _storeDesiredTotalUnits[aStoreFilterIDX]
        //                - (double)_packMultiple[packIDX];
        //            // END TT#2920 -  Jellis - Detail Pack Algorithm Change
        //            for(sizeIDX=0; sizeIDX < _sizeCount; sizeIDX++)
        //            {
        //                // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
        //                //_storePackGap[aStoreFilterIDX, packIDX] +=
        //                //    Math.Abs
        //                //    (
        //                //    _storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX]
        //                //    - _packSizeCurvePct[packIDX, sizeIDX]
        //                //    );
        //                if (_packSizeContent[packIDX, sizeIDX] > 0)
        //                {
        //                    if (totalCurveUnits > 0)
        //                    {
        //                        _storePackGap[aStoreFilterIDX, packIDX] +=
        //                            Math.Abs
        //                            (
        //                             ((_storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] - (double)_packSizeContent[packIDX, sizeIDX])
        //                              / totalCurveUnits)
        //                             );
        //                    }
        //                }
        //                // END TT#2920 -  Jellis - Detail Pack Algorithm Change

        //            }					
        //        }
        //        if (_storeCandidateCount[aStoreFilterIDX] == 1
        //            && potentialCandidateCount > 1)
        //        {
        //            packIDX = sortedPack[1].Item;
        //            targetPackNeedError = (int)_storePackNeedError[aStoreFilterIDX, packIDX];
        //            targetPackAvgDeviationError = _storePackAvgDeviationError[aStoreFilterIDX, packIDX];
        //            for(sortIDX=1; sortIDX < potentialCandidateCount; sortIDX++)
        //            {
        //                packIDX = sortedPack[sortIDX].Item;
        //                if (_storePackNeedError[aStoreFilterIDX, packIDX] == targetPackNeedError
        //                    && _storePackAvgDeviationError[aStoreFilterIDX, packIDX] == targetPackAvgDeviationError)
        //                {
        //                    _storePackCandidate[aStoreFilterIDX, packIDX] = true;
        //                    _storeCandidateCount[aStoreFilterIDX]++;
        //                }
        //            }
        //        }
        //        if (_storeCandidateCount[aStoreFilterIDX] > 0)
        //        {
        //            MIDGenericSortItem[] candidatePack = new MIDGenericSortItem[_storeCandidateCount[aStoreFilterIDX]];
        //            packIDX = 0;
        //            sortIDX = 0;
        //            for(packIDX=0; packIDX < _packCount; packIDX++)
        //            {
        //                if (_storePackCandidate[aStoreFilterIDX, packIDX])
        //                {
        //                    candidatePack[sortIDX].Item = packIDX;
        //                    candidatePack[sortIDX].SortKey = new double[7]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    candidatePack[sortIDX].SortKey[0] = _storePackDeviationError[aStoreFilterIDX, packIDX];
        //                    candidatePack[sortIDX].SortKey[1] = _storePackNeedError[aStoreFilterIDX, packIDX];
        //                    candidatePack[sortIDX].SortKey[2] = _storePackPctNeedMeasure[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    candidatePack[sortIDX].SortKey[3] = _storePackGap[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    candidatePack[sortIDX].SortKey[4] = -_packMultiple[packIDX];  // descending order // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    candidatePack[sortIDX].SortKey[5] = -_packsToAllocate[packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    candidatePack[sortIDX].SortKey[6] = _transaction.GetRandomDouble(); // TT#2920 - Jellis - Detail Pack Algorithm Change
        //                    sortIDX++;
        //                }
        //            }
        //            Array.Sort(candidatePack, new MIDGenericSortAscendingComparer()); // TT#1143 - MD - Jellis - Group ALlocation Min Broken
        //            int packIDX_1;
        //            int packIDX_2;
        //            if (_storeCandidateCount[aStoreFilterIDX] > 1)
        //            {
        //                packIDX_1 = candidatePack[0].Item;
        //                _storePrimaryPackIDX[aStoreFilterIDX] = packIDX_1;
        //                packIDX_2 = candidatePack[1].Item;
        //                _storePackDeviationErrorDiff[aStoreFilterIDX] =
        //                    Math.Abs
        //                    (
        //                    _storePackAvgDeviationError[aStoreFilterIDX, packIDX_1]
        //                    - _storePackAvgDeviationError[aStoreFilterIDX, packIDX_2]
        //                    );
        //                _storePackNeedErrorDiff[aStoreFilterIDX] =
        //                    Math.Abs
        //                    (
        //                    _storePackNeedError[aStoreFilterIDX, packIDX_1]
        //                    - _storePackNeedError[aStoreFilterIDX, packIDX_2]
        //                    );
        //                _storePackGapDiff[aStoreFilterIDX] =
        //                    Math.Abs
        //                    (
        //                    _storePackGap[aStoreFilterIDX, packIDX_1]
        //                    - _storePackGap[aStoreFilterIDX, packIDX_2]
        //                    );
        //            }
        //            else
        //            {
        //                _storePrimaryPackIDX[aStoreFilterIDX] = candidatePack[0].Item;
        //                _storePackDeviationErrorDiff [aStoreFilterIDX] = 0;
        //                _storePackNeedErrorDiff [aStoreFilterIDX] = 0;
        //                _storePackGapDiff [aStoreFilterIDX] = 0;
        //            }
        //        }   
        //    }
        //}

        // begin TT#1074 - MD - Jellis - Inventory Min Max incorrect for group  allocation
        public int GetInventoryMdseBasisRID(int aColorRID)
        {

            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return result.InventoryMdseBasisRID;
        }
        // end TT#1074 - MD - Jellis - Inentory Min Max incorrect for Group Allocation

        // begin TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
        public int GetMerchandiseBasisRID(int aColorRID)
        {

            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return result.MerchandiseBasisRID;
        }
        // end TT#1176 - MD - JEllis - Group Allocation Size need not observing inv min max

		/// <summary>
		/// Returns a Size Curve Profile that may have alternates applied. 
		/// This curve is restricted to those sizes allocated to on the header. 
		/// This was the curve used for creating the Plan.
		/// </summary>
		/// <param name="storeRid"></param>
		/// <returns></returns>
		public SizeCurveProfile GetSizeCurve(int colorRid, int storeRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            Hashtable ht = GetSizeCurve(colorRid);
            return (SizeCurveProfile)ht[storeRid];
            //try
            //{
            //    SizeCurveProfile scp = null;
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        scp = aResult.GetRestrictedSizeCurve(storeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        scp = GetSizeCurve(colorRid, storeRid);
            //    }
            //    return scp;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set
		}

		/// <summary>
		/// Returns a store/size curve hashtable.  the size curves may have alternates applied.
		/// This curve is restricted to those sizes allocated to on the header. 
		/// These are the size curves used to create the plan.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <returns></returns>
		public Hashtable GetSizeCurve(int colorRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            // end TT#2155 - JEllis - Fill Size Holes Null Reference

            if (result == null)
            {
                result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)
            //// begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            //else if (result.GetOnHandAndIT == true)
            //{
            //    result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //}
            // end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            // end TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)

            return result.StoreRestrictedSizeCurveHash;
            //try
            //{
            //    Hashtable ht = null;
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        ht = aResult.StoreRestrictedSizeCurveHash;
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        ht = GetSizeCurve(colorRid);
            //    }
            //    return ht;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set

		}

		/// <summary>
		/// Returns a Size Curve Profile that may have alternates applied. 
		/// </summary>
		/// <param name="storeRid"></param>
		/// <returns></returns>
		public SizeCurveProfile GetFullSizeCurve(int colorRid, int storeRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            Hashtable ht = GetFullSizeCurve(colorRid);
            return (SizeCurveProfile)ht[storeRid];

            //try
            //{
            //    SizeCurveProfile scp = null;
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        scp = aResult.GetSizeCurve(storeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        scp = GetFullSizeCurve(colorRid, storeRid);
            //    }
            //    return scp;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set
		}

		/// <summary>
		/// Returns a store/size curve hashtable.  the size curves may have alternates applied.
		/// This curve is restricted to those sizes allocated to on the header. 
		/// These are the size curves used to create the plan.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <returns></returns>
		public Hashtable GetFullSizeCurve(int colorRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
            // end TT32155 - JEllis - Fill Size Holes Null Reference
            if (result == null)
            {
                result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)
            //// begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            //else if (result.GetOnHandAndIT == true)
            //{
            //    result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //}
            //// end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            // end TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)

            return result.StoreSizeCurveHash;
            //try
            //{
            //    Hashtable ht = null;
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        ht = aResult.StoreSizeCurveHash;
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        ht = GetFullSizeCurve(colorRid);
            //    }
            //    return ht;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set
		}

		/// <summary>
		/// Returns the Original Size Curve Profile before any alternates have been assigned.
		/// </summary>
		/// <param name="storeRid"></param>
		/// <returns></returns>
		public SizeCurveProfile GetOriginalSizeCurve(int colorRid, int storeRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            Hashtable ht = GetOriginalSizeCurve(colorRid);
            return (SizeCurveProfile)ht[storeRid];
            //try
            //{
            //    SizeCurveProfile scp = null;
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        scp = aResult.GetOriginalSizeCurve(storeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        scp = GetOriginalSizeCurve(colorRid, storeRid);
            //    }
            //    return scp;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set
		}

		public Hashtable GetOriginalSizeCurve(int colorRid)
		{
            // begin TT#702 Infinite Loop when begin date set
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference 
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
            // end  TT#2155 - JEllis - Fill Size Holes Null Reference
            if (result == null)
            {
                result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)
            //// begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            //else if (result.GetOnHandAndIT == true)
            //{
            //    result = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //}
            //// end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            // end TT#3060 - Loehmans - Jellis - Argument Exception (Loop causing Null Reference -- result and curves are now created at instantiation)

            Hashtable ht = new Hashtable();
            int cnt = result.Stores.Count;
            for (int s = 0; s < cnt; s++)
            {
                StoreProfile sp = (StoreProfile)result.Stores[s];
                SizeCurveProfile scp = (SizeCurveProfile)result.GetOriginalSizeCurve(sp.Key);
                ht.Add(sp.Key, scp);
            }
            return ht;
            //try
            //{
            //    Hashtable ht = new Hashtable();
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        int cnt = aResult.Stores.Count;
            //        for (int s=0;s<cnt;s++)
            //        {
            //            StoreProfile sp = (StoreProfile)aResult.Stores[s];
            //            SizeCurveProfile scp = (SizeCurveProfile)aResult.GetOriginalSizeCurve(sp.Key);
            //            ht.Add(sp.Key, scp);
            //        }
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesOnly);
            //        _sizeNeedResultsHash.Add(colorRid, aResult);
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        ht = GetOriginalSizeCurve(colorRid);
            //    }
            //    return ht;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#702 Infinite Loop when begin date set
		}

		public int GetPlanUnits(int storeRid, int colorRid, int sizeRid, bool aReturnFillPlan, bool aUseBasisPlan) // MID Track 4291 add fill variables to size review  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
		{
			int units = 0;
			try
			{
				// begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
				//if (this._sizeNeedResultsHash.ContainsKey(colorRid))
				//{
				//	SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                //SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
				{
					aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                    //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
                }
				// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
				if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
				{
					if (_sizeNeedAlgorithm != null)
					{
						_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, aUseBasisPlan); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
					}
					else
					{
						this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
							MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
					}
				}
				// begin MID Track 4291 add fill variables to size review
				if (aReturnFillPlan)
				{
					// begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
					//if (aResult.CalculateFillPlan)
					//{
					//	_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true);
					//}
					//units = aResult.GetFillToOwn_PlanUnits(storeRid, sizeRid); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
					if (aUseBasisPlan)
					{
						if (aResult.CalculateFillToPlan_Plan)
						{
							_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, true);
						}
						units = aResult.GetFillToPlan_PlanUnits(storeRid, sizeRid);
					}
					else
					{
						if (aResult.CalculateFillToOwn_Plan)
						{
							_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, false);
						}
						units = aResult.GetFillToOwn_PlanUnits(storeRid, sizeRid); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
					}
					// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				}
				else
				{
					// end MID Track 4291 add fill variables to size review
					units = aResult.GetSizeNeed_PlanUnits(storeRid, sizeRid);  // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
				} // MID track 4291 add fill variables to size review
			    // begin MID Track 4921 AnF#666 Fill to Size Plan (Performance)
			    return units;
				//}
			    //else
				//{
				//	//=========================================================
				//	// fills in the SizeNeedResults with the plan information
				//	//=========================================================
				//	SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
				//	_sizeNeedResultsHash.Add(colorRid, aResult);
				//	//==========================================================================
				//	// call this same method again.  This time the result is in the hash list.
				//	//==========================================================================
				//	units = GetPlanUnits(storeRid, colorRid, sizeRid, aReturnFillPlan); // MID Track 4291 add fill variables to size review
				//}
                // end MID Track 4921 AnF#666 Fill to Size Plan (Performance)
				//return units;
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function
		/// <summary>
		/// Returns a Hashtable of storeRids and their plan units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <param name="sizeRid"></param>
		/// <returns></returns>
        //public Hashtable GetPlanUnits(int colorRid, int sizeRid)
        //{
        //    try
        //    {
        //        Hashtable ht = new Hashtable();
        //        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        //        SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
        //        if (aResult == null)
        //        {
        //            aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
        //        }
        //        else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
        //        {
        //            if (_sizeNeedAlgorithm != null)
        //            {
        //                _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //            }
        //            else
        //            {
        //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

        //                throw new MIDException(eErrorLevel.severe,
        //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
        //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
        //            }
        //        }

        //        int cnt = aResult.Stores.Count;
        //        for (int s = 0; s < cnt; s++)
        //        {
        //            StoreProfile sp = (StoreProfile)aResult.Stores[s];
        //            int units = aResult.GetSizeNeed_PlanUnits(sp.Key, sizeRid); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //            ht.Add(sp.Key, units);
        //        }
        //        return ht;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
        //    //    {
        //    //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
        //    //        if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
        //    //        {
        //    //            if (_sizeNeedAlgorithm != null)
        //    //            {
        //    //                _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //    //            }
        //    //            else
        //    //            {
        //    //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

        //    //                throw new MIDException(eErrorLevel.severe,
        //    //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
        //    //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
        //    //            }
        //    //        }

        //    //        int cnt = aResult.Stores.Count;
        //    //        for (int s = 0; s < cnt; s++)
        //    //        {
        //    //            StoreProfile sp = (StoreProfile)aResult.Stores[s];
        //    //            int units = aResult.GetSizeNeed_PlanUnits(sp.Key, sizeRid); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //    //            ht.Add(sp.Key, units);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        //=========================================================
        //    //        // fills in the SizeNeedResults with the plan information
        //    //        //=========================================================
        //    //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
        //    //        //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
        //    //        //==========================================================================
        //    //        // call this same method again.  This time the result is in the hash list.
        //    //        //==========================================================================
        //    //        //					this._secondTimeThru = true;
        //    //        ht = GetPlanUnits(colorRid, sizeRid);
        //    //    }
        //    //    return ht;
        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //        // end TT#2155 - JEllis - Fill Size Holes Null Reference
        //}
        //End TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function

        // begin MID Track 4921 Fill to Size Plan Enhancement
		public int GetSalesBasisPlanUnits(int storeRid, int colorRid)
		{
			try
			{
                // begin TT#2155 - Jellis - Fill Size Holes Null Reference
                //SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                //  end TT#2155 - Jellis - Fill Size Holes Null Reference
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
				{
					aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                    //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
                }
				if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
				{
					if (_sizeNeedAlgorithm != null)
					{
						_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); 
					}
					else
					{
						this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
							MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
					}
				}
				if (aResult.CalculateFillToPlan_Plan)
				{
					_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, true);
				}
				return aResult.GetFillToPlan_SalesUnits(storeRid);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a Hashtable of storeRids and their sales basis plan units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <returns></returns>
		public Hashtable GetSalesBasisPlanStoreHash(int colorRid)
		{
			try
			{
				Hashtable ht = new Hashtable();
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                //SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
				if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
				{
					aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                    //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
                }
				if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
				{
					if (_sizeNeedAlgorithm != null)
					{
						_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
					}
					else
					{
						this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
							throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
							MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
					}
				}
				if (aResult.CalculateFillToPlan_Plan)
				{
					_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, true);
				}
				int cnt = aResult.Stores.Count;
				for (int s=0;s<cnt;s++)
				{
					StoreProfile sp = (StoreProfile)aResult.Stores[s];
					int units = aResult.GetFillToPlan_SalesUnits(sp.Key); 
					ht.Add(sp.Key, units);
				}

				return ht;
			}
			catch
			{
				throw;
			}
		}
		public int GetStockBasisPlanUnits(int storeRid, int colorRid)
		{
			int units = 0;
			try
			{
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                //SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
				{
					aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                    //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
                }
				if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
				{
					if (_sizeNeedAlgorithm != null)
					{
						_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); 
					}
					else
					{
						this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
							MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
					}
				}
				if (aResult.CalculateFillToPlan_Plan)
				{
					_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, true);
				}
				units = aResult.GetFillToPlan_StockUnits(storeRid);
			}
			catch
			{
				throw;
			}
			return units;
		}

		/// <summary>
		/// Returns a Hashtable of storeRids and their stock basis plan units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <returns></returns>
		public Hashtable GetStockBasisPlanStoreHash(int colorRid)
		{
			try
			{
				Hashtable ht = new Hashtable();
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                //SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                // end TT#2155 - JEllis - Fill Size Holes Null Reference	
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
				{
					aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                    //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
                }
				if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
				{
					if (_sizeNeedAlgorithm != null)
					{
						_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); 
					}
					else
					{
						this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
							MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
					}
				}
				if (aResult.CalculateFillToPlan_Plan)
				{
					_sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, true, true);
				}
				int cnt = aResult.Stores.Count;
				for (int s=0;s<cnt;s++)
				{
					StoreProfile sp = (StoreProfile)aResult.Stores[s];
					int units = aResult.GetFillToPlan_StockUnits(sp.Key); 
					ht.Add(sp.Key, units);
				}

				return ht;
			}
			catch
			{
				throw;
			}
		}
		// end MID Track 4921 Fill to Size Plan Enhancement


		public int GetIntransitUnits(int storeRid, int colorRid, int sizeRid)
		{
            //int units = 0;  // TT#488 - MD - Jellis - Group Allocation (field not used)
			try
			{
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
                {
                    aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                }
                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
                {
                    if (_sizeNeedAlgorithm != null)
                    {
                        _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    }
                    else
                    {
                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
                    }
                }
                return aResult.GetIntransitUnits(storeRid, sizeRid);
            }
            catch
            {
                throw;
            }            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];

            //        if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
            //        {
            //            if (_sizeNeedAlgorithm != null)
            //            {
            //                _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //            }
            //            else
            //            {
            //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

            //                throw new MIDException (eErrorLevel.severe,
            //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
            //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
            //            }
            //        }

            //        units = aResult.GetIntransitUnits(storeRid, sizeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
            //        //_sizeNeedResultsHash.Add(colorRid, aResult);  // TT#1552 INFINITE LOOP FIX
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        units = GetIntransitUnits(storeRid, colorRid, sizeRid);
            //    }
            //    return units;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
		}

        //Begin TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function
		/// <summary>
		/// Returns a Hashtable of storeRids and their intransit units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <param name="sizeRid"></param>
		/// <returns></returns>
//        public Hashtable GetIntransitUnits(int colorRid, int sizeRid)
//        {
//            try
//            {
//                Hashtable ht = new Hashtable();
//                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
//                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
//                if (aResult == null)
//                {
//                    aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
//                }
//                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
//                {
//                    if (_sizeNeedAlgorithm != null)
//                    {
//                        _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
//                    }
//                    else
//                    {
//                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

//                        throw new MIDException(eErrorLevel.severe,
//                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
//                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
//                    }
//                }

//                int cnt = aResult.Stores.Count;
//                for (int s = 0; s < cnt; s++)
//                {
//                    StoreProfile sp = (StoreProfile)aResult.Stores[s];
//                    int units = aResult.GetIntransitUnits(sp.Key, sizeRid);
//                    ht.Add(sp.Key, units);
//                }
//                return ht;
//            }
//            catch
//            {
//                throw;
//            }
////                if (this._sizeNeedResultsHash.ContainsKey(colorRid))
////                {
////                    SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
////                    if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
////                    {
////                        if (_sizeNeedAlgorithm != null)
////                        {
////                            _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
////                        }
////                        else
////                        {
////                            this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

////                            throw new MIDException (eErrorLevel.severe,
////                                (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
////                                MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
////                        }
////                    }

////                    int cnt = aResult.Stores.Count;
////                    for (int s=0;s<cnt;s++)
////                    {
////                        StoreProfile sp = (StoreProfile)aResult.Stores[s];
////                        int units = aResult.GetIntransitUnits(sp.Key, sizeRid);
////                        ht.Add(sp.Key, units);
////                    }
////                }
////                else
////                {
////                    //=========================================================
////                    // fills in the SizeNeedResults with the plan information
////                    //=========================================================
////                    SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
////                    //_sizeNeedResultsHash.Add(colorRid, aResult);  // TT#1552 INFINITE LOOP FIX
////                    //==========================================================================
////                    // call this same method again.  This time the result is in the hash list.
////                    //==========================================================================
//////					this._secondTimeThru = true;
////                    ht = GetIntransitUnits(colorRid, sizeRid);
					
////                }
////                return ht;
////            }
////            catch
////            {
////                throw;
////            }
//            // end TT#2155 - JEllis - Fill Size Holes Null Reference
//        }
        //End TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function

		public int GetOnhandUnits(int storeRid, int colorRid, int sizeRid)
		{
            // begin TT#2155 - Jellis - Fill Size Holes Null Reference
            try
            {
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
                {
                    aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                }
                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
                {
                    if (_sizeNeedAlgorithm != null)
                    {
                        _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    }
                    else
                    {
                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
                    }
                }
                return aResult.GetOnhandUnits(storeRid, sizeRid);
            }
            catch
            {
                throw;
            }
            //int units = 0;
            //try
            //{
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];

            //        if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
            //        {
            //            if (_sizeNeedAlgorithm != null)
            //            {
            //                _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //            }
            //            else
            //            {
            //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

            //                throw new MIDException (eErrorLevel.severe,
            //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
            //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
            //            }
            //        }

            //        units = aResult.GetOnhandUnits(storeRid, sizeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
            //        //_sizeNeedResultsHash.Add(colorRid, aResult);   // TT#1552 INFINITE LOOP FIX
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        units = GetOnhandUnits(storeRid, colorRid, sizeRid);
            //    }
            //    return units;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
		}

        // Begin TT#5026 - JSmith - Question about Size Alternates
        public int GetVSWOnhandUnits(int storeRid, int colorRid, int sizeRid)
        {
            try
            {
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                if (aResult == null
                    || _sizeNeedAlgorithm == null)
                {
                    aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
                }
                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
                {
                    if (_sizeNeedAlgorithm != null)
                    {
                        _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false);
                    }
                    else
                    {
                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name);

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
                    }
                }
                return aResult.GetVswOnhandUnits(storeRid, sizeRid);
            }
            catch
            {
                throw;
            }
        }
		// End TT#5026 - JSmith - Question about Size Alternates

        //Begin TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function
		/// <summary>
		/// Returns a Hashtable of storeRids and their Onhand units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <param name="sizeRid"></param>
		/// <returns></returns>
//        public Hashtable GetOnhandUnits(int colorRid, int sizeRid)
//        {
//            // begin TT#2155 - Jellis - Fill Size Holes Null Reference
//            try
//            {
//                Hashtable ht = new Hashtable();
//                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
//                if (aResult == null)
//                {
//                    aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
//                }
//                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
//                {
//                    if (_sizeNeedAlgorithm != null)
//                    {
//                        _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID track 4291 add fill variables to size review  // MID Track 4921 Fill to Size Plan Enhancement
//                    }
//                    else
//                    {
//                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

//                        throw new MIDException(eErrorLevel.severe,
//                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
//                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
//                    }
//                }

//                int cnt = aResult.Stores.Count;
//                for (int s = 0; s < cnt; s++)
//                {
//                    StoreProfile sp = (StoreProfile)aResult.Stores[s];
//                    int units = aResult.GetOnhandUnits(sp.Key, sizeRid);
//                    ht.Add(sp.Key, units);
//                }
//                return ht;
//            }
//            catch
//            {
//                throw;
//            }
////            try
////            {
////                Hashtable ht = new Hashtable();
////                if (this._sizeNeedResultsHash.ContainsKey(colorRid))
////                {
////                    SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
////                    if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
////                    {
////                        if (_sizeNeedAlgorithm != null)
////                        {
////                            _sizeNeedAlgorithm.ProcessSizePlanOnly(aResult, false, false); // MID track 4291 add fill variables to size review  // MID Track 4921 Fill to Size Plan Enhancement
////                        }
////                        else
////                        {
////                            this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

////                            throw new MIDException (eErrorLevel.severe,
////                                (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
////                                MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
////                        }
////                    }

////                    int cnt = aResult.Stores.Count;
////                    for (int s=0;s<cnt;s++)
////                    {
////                        StoreProfile sp = (StoreProfile)aResult.Stores[s];
////                        int units = aResult.GetOnhandUnits(sp.Key, sizeRid);
////                        ht.Add(sp.Key, units);
////                    }
////                }
////                else
////                {
////                    //=========================================================
////                    // fills in the SizeNeedResults with the plan information
////                    //=========================================================
////                    SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.SizeCurvesAndPlanOnly);
////                    //_sizeNeedResultsHash.Add(colorRid, aResult);  //  TT#1552 INFINITE LOOP FIX
////                    //==========================================================================
////                    // call this same method again.  This time the result is in the hash list.
////                    //==========================================================================
//////					this._secondTimeThru = true;
////                    ht = GetOnhandUnits(colorRid, sizeRid);
					
////                }
////                return ht;
////            }
////            catch
////            {
////                throw;
////            }
//            // end TT#2155 - JEllis - Fill Size Holes Null Reference
//        }
        //End TT#846-MD -jsobek -Fill to Size Plan Presentation -unused function

		public int GetNeedUnits(int storeRid, int colorRid, int sizeRid)
		{
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            try
            {
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
                {
                    aResult = PreProcessing(colorRid, eSizeProcessControl.ProcessAll);
                }
                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesAndPlanOnly)
                {
                    if (_sizeNeedAlgorithm != null)
                        _sizeNeedAlgorithm.ProcessSizeNeedOnly(aResult);
                    else
                    {
                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
                    }
                }
                //return aResult.GetAllocatedUnits(storeRid, sizeRid);  // TT#1039 - MD - Jellis - Size Need Missing
                return aResult.GetSizeNeedUnits(storeRid, sizeRid);     // TT#1039 - MD - Jellis - Size need Missing
            }
            catch
            {
                throw;
            }
            //int units = 0;
            //try
            //{
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesAndPlanOnly)
            //        {
            //            if (_sizeNeedAlgorithm != null)
            //                _sizeNeedAlgorithm.ProcessSizeNeedOnly(aResult);
            //            else
            //            {
            //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

            //                throw new MIDException (eErrorLevel.severe,
            //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
            //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
            //            }
            //        }
						
            //        units = aResult.GetAllocatedUnits(storeRid, sizeRid);
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.ProcessAll);
            //        //_sizeNeedResultsHash.Add(colorRid, aResult);  // TT#1552 INFINITE LOOP FIX
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        units = GetNeedUnits(storeRid, colorRid, sizeRid);
            //    }
            //    return units;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
		}

		/// <summary>
		/// Returns a Hashtable of storeRids and their need units.
		/// </summary>
		/// <param name="colorRid"></param>
		/// <param name="sizeRid"></param>
		/// <returns></returns>
		public Hashtable GetNeedUnits(int colorRid, int sizeRid)
		{
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            try
            {
                Hashtable ht = new Hashtable();
                SizeNeedResults aResult = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, colorRid);
                if (aResult == null // TT#1039 - MD - Jellis - Size Need Missing
                    || _sizeNeedAlgorithm == null) // TT#1039 - MD -Jellis - Size Need Missing
                {
                    aResult = PreProcessing(colorRid, eSizeProcessControl.ProcessAll);
                }
                else if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesAndPlanOnly)
                {
                    if (_sizeNeedAlgorithm != null)
                        _sizeNeedAlgorithm.ProcessSizeNeedOnly(aResult);
                    else
                    {
                        this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
                            MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
                    }
                }
                int cnt = aResult.Stores.Count;
                for (int s = 0; s < cnt; s++)
                {
                    StoreProfile sp = (StoreProfile)aResult.Stores[s];
                    int units = aResult.GetAllocatedUnits(sp.Key, sizeRid);
                    ht.Add(sp.Key, units);
                }
                return ht;
            }
            catch
            {
                throw;
            }
            //try
            //{
            //    Hashtable ht = new Hashtable();
            //    if (this._sizeNeedResultsHash.ContainsKey(colorRid))
            //    {
            //        SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //        if (aResult.ProcessControl == eSizeProcessControl.SizeCurvesAndPlanOnly)
            //        {
            //            if (_sizeNeedAlgorithm != null)
            //                _sizeNeedAlgorithm.ProcessSizeNeedOnly(aResult);
            //            else
            //            {
            //                this._aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

            //                throw new MIDException (eErrorLevel.severe,
            //                    (int)eMIDTextCode.msg_al_MissingSizeNeedAlgorithm,
            //                    MIDText.GetText(eMIDTextCode.msg_al_MissingSizeNeedAlgorithm));
            //            }
            //        }
            //        int cnt = aResult.Stores.Count;
            //        for (int s=0;s<cnt;s++)
            //        {
            //            StoreProfile sp = (StoreProfile)aResult.Stores[s];
            //            int units = aResult.GetAllocatedUnits(sp.Key, sizeRid);
            //            ht.Add(sp.Key, units);
            //        }
            //    }
            //    else
            //    {
            //        //=========================================================
            //        // fills in the SizeNeedResults with the plan information
            //        //=========================================================
            //        SizeNeedResults aResult = PreProcessing(colorRid, eSizeProcessControl.ProcessAll);
            //        //_sizeNeedResultsHash.Add(colorRid, aResult);  // TT#1552 INFINITE LOOP FIX
            //        //==========================================================================
            //        // call this same method again.  This time the result is in the hash list.
            //        //==========================================================================
            //        ht = GetNeedUnits(colorRid, sizeRid);
            //    }
            //    return ht;
            //}
            //catch
            //{
            //    throw;
            //}
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
		}
		// begin TT#843 - New Size Constraint Balance
        public int GetStoreSizeMultiple(int aColorRID, int aSizeRID, int aStoreRID)
        {
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[aColorRID];
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            // end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            return result.GetStoreMult(aStoreRID, aSizeRID);      // TT#1391 - TMW New Action 
            //return result.GetStoreMult(aStoreRID, this._transaction.GetSizeCodeProfile(aSizeRID)); // TT#1391 - TMW New Action
        }
        // brgin TT#519 - MD - Jellis - VSW - Minimums not working
        public int GetStoreSizeMinimum(int aColorRID, int aSizeRID, int aStoreRID)
        {
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return result.GetStoreMinimum(aStoreRID, aSizeRID);  
        }
        // end TT#519 - MD - Jellis - VSW - Minimums not working
        public int GetStoreSizeInventoryMinimum(int aColorRID, int aSizeRID, int aStoreRID)
        {
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[aColorRID];
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
			// Begin Development TT#12 - JSmith - Size Review- Processed 'Balance Size With Contraint' action and system never responds
            // end TT#2155 - JEllis - Fill Size Holes Null Reference            if (result == null)
            // end TT#2155 - JEllis - Fill Size Holes Null Reference            
            if (result == null)
			// End Development TT#12
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            // end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
            //return result.GetStoreInventoryMin(aStoreRID, aSizeRID, true);  // TT#1543 - JEllis - Size Mult Broken // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
            return result.GetStoreInventoryMin(aStoreRID, aSizeRID, true, GetAlternateSizeCodes(aStoreRID, aColorRID, aSizeRID));
            // End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
            //return result.GetStoreMin(aStoreRID, aSizeRID);        // TT#1391 - TMW New Action  // TT#1543 - JEllis - Size Mult Broken
            //return result.GetStoreMin(aStoreRID, this._transaction.GetSizeCodeProfile(aSizeRID)); // TT#1391 - TMW New Action
        }
        public int GetStoreSizeInventoryMaximum(int aColorRID, int aSizeRID, int aStoreRID)
        {
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            //SizeNeedResults result = (SizeNeedResults)_sizeNeedResultsHash[aColorRID];
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
			// Begin Development TT#12 - JSmith - Size Review- Processed 'Balance Size With Contraint' action and system never responds
            // end TT#2155 - JEllis - Fill Size Holes Null Reference            if (result == null)
            if (result == null)
			// End Development TT#12
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
                //_sizeNeedResultsHash.Add(colorRid, result);  // TT#1552 INFINITE LOOP FIX
            }
            // begin TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            // end TT#233 - MD - JEllis - Workflow and Manual Stepping get different results when using Fill Size
            // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
            //return result.GetStoreInventoryMax(aStoreRID, aSizeRID, true);                               // TT#1543 - JElllis - Size Mult Broken // TT#1176 - MD _ Jellis - Size Need not observing Inv Min Max for Group
            return result.GetStoreInventoryMax(aStoreRID, aSizeRID, true, GetAlternateSizeCodes(aStoreRID, aColorRID, aSizeRID));
            // End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
            //return result.GetStoreMax(aStoreRID, aSizeRID);                                            // TT#1391 - TMW New Action  // TT#1543 - JEllis - Size Mult Broken
            //return result.GetStoreMax(aStoreRID, this._transaction.GetSizeCodeProfile(aSizeRID));    // TT#1391 - TMW New Action
        }
        // end TT#843 - New Size Constraint Balance

        // begin TT#519 - MD - Jellis - AnF VSW Minimums not working
        public int GetStoreInventoryBasisOnhand(int aColorRID, int aSizeRID, int aStoreRID)
        {
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return result.GetIbOnhandUnits(aStoreRID, aSizeRID);
        }
        public int GetStoreInventoryBasisIntransit(int aColorRID, int aSizeRID, int aStoreRID)
        {
            SizeNeedResults result = _transaction.GetSizeNeedResults(_allocationProfile.HeaderRID, aColorRID);
            if (result == null)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            else if (result.GetOnHandAndIT == true)
            {
                result = PreProcessing(aColorRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return result.GetIbIntransitUnits(aStoreRID, aSizeRID);
        }
        // end TT#519 - MD - Jellis - AnF VSW Minimums not working

		/// <summary>
		/// Clears out the SizeNeedResults for the color leaving only the Size Curves. If no SizeNeedResults is found,
		/// nothing is done.
		/// </summary>
		public void ClearPlan(int colorRid)
		{
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            _transaction.ClearSizeNeedResults_Plan(_allocationProfile.HeaderRID, colorRid, false);
            //if (_sizeNeedResultsHash.ContainsKey(colorRid))
            //{
            //    SizeNeedResults aResult = (SizeNeedResults)_sizeNeedResultsHash[colorRid];
            //    aResult.Clear(false);
            //    aResult.ProcessControl = eSizeProcessControl.SizeCurvesOnly;
            //}
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
		}

		private SizeNeedResults PreProcessing(int colorRid, eSizeProcessControl processControl)
		{
			try
			{
				_reserveStoreRid = this._transaction.GlobalOptions.ReserveStoreRID;
				// Can't do a store filter at this time
				GeneralComponent aComponent = null;
				ProfileList allStoreList = _transaction.GetMasterProfileList(eProfileType.Store); // MID Change j.ellis removal of reserve store changes master list
				if (colorRid == Include.DummyColorRID)
					aComponent = new GeneralComponent(eGeneralComponentType.DetailType);
				else
					aComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRid);

				//=====================================================================================================
				// Allocation may need the plan, need, and size curve information for all stores whether or not they 
				// have units to allocate.  This is slightly different from normal Size Need processing.
				// The Reserve store, if found, IS removed from the list.
				//=====================================================================================================
				// BEGIN MID Change j.ellis removal of reserve store changes Master List.
				// StoreProfile reserveStore = (StoreProfile)storeList.FindKey(_reserveStoreRid);
				// if (reserveStore != null)
				// 	storeList.Remove(reserveStore);
				ProfileList storeList = (ProfileList)allStoreList.Clone(); 
				StoreProfile reserveStore = (StoreProfile)storeList.FindKey(_reserveStoreRid);
				if (reserveStore != null)
				{
					storeList.Remove(reserveStore);
				}
				// END MID Change j.ellis removal of reserve store changes Master List.
				
				// begin A&F Generic Size Curve 							
				//SizeCurveGroupProfile sizeCurveGroup = new SizeCurveGroupProfile(this.SizeCurveGroupRid);
				SizeCurveGroupProfile sizeCurveGroup;
				if (base.GenericSizeCurveDefined())
				{
					int genSizeCurveGroupRID = (int)base.GenCurveHash[this._allocationProfile.Key];
					sizeCurveGroup = new SizeCurveGroupProfile(genSizeCurveGroupRID);
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
						sizeConstraintRID = (int)base.GenConstraintHash[this._allocationProfile.Key];
					}
					catch
					{
						string errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeConstraintForHeader),string.Empty, this._allocationProfile.HeaderID);
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoSizeConstraintForHeader, errMessage);
					}
				}
				// end MID Track 4372 Generic Size Contraint
                // begin TT#702 Infinite Loop when begin date set
                _sizeNeedAlgorithm =
                    new SizeNeedAlgorithm(
                        _transaction,
                        //eMethodType.SizeNeedAllocation,  // TT#2155 - JEllis - Fill Size Null Reference
                        _allocationProfile,
                        aComponent,
                        RulesCollection,
                        _sizeAlternateRid,
                        sizeConstraintRID,
                        sizeCurveGroup,
                        storeList,
                        _MerchType,
                        _MerchHnRid,
                        _MerchPhRid,
                        _MerchPhlSequence,
                        SG_RID,
                        eSizeNeedColorPlan.PlanForSpecificColorOnly,
                        // begin TT#41 - MD - Jellis - Size Inventory Min Max
                        this.NormalizeSizeCurves,
                        this._IB_MerchandiseType,
                        this._IB_MERCH_HN_RID,
                        this._IB_MERCH_PH_RID,
                        this._IB_MERCH_PHL_SEQUENCE,  // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
                        this._vSWSizeConstraints, // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
                        this._FillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
                        );    
                        //this.NormalizeSizeCurves);
                        // end TT#41 - MD - Jellis - Size inventory Min Max
                // begin TT#2155 - JELlis - Fill Size Holes Null Reference
                //_sizeNeedResultsHash.Add(_sizeNeedAlgorithm.SizeNeedResults.HeaderColorRid, _sizeNeedAlgorithm.SizeNeedResults); // TT#1552 INFINITE LOOP FIX
                //_transaction.SetSizeNeedResults(_allocationProfile.HeaderRID, _sizeNeedAlgorithm.SizeNeedResults);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                // end TT#2155 - Jellis - Fill Size holes Null Reference
                return _sizeNeedAlgorithm.ProcessSizeNeed(processControl, ApplyRulesOnly); // TT#2155 - JEllis - Fill Size Holes Null Reference

                //_sizeNeedAlgorithm = new SizeNeedAlgorithm(this._transaction, eMethodType.SizeNeedAllocation);

                ////================================
                //// call Size Need Processing
                //// this returns on plan filled in
                ////================================
                //// begin removed unnecessary comments //TT#702 Infinite Loop when begin date set 
                //SizeNeedResults sizeResults = 
                //    _sizeNeedAlgorithm.ProcessSizeNeed(
                //    this._allocationProfile, 
                //    aComponent,
                //    sizeCurveGroup, 
                //    storeList,
                //    this._MerchType, this._MerchHnRid, this._MerchPhRid, this._MerchPhlSequence,
                //    this.SG_RID,
                //    sizeConstraintRID, this._sizeAlternateRid, this.RulesCollection,         // MID Track 4372 Generic Size Constraint
                //    eSizeNeedColorPlan.PlanForSpecificColorOnly, processControl,             // MID Track 4861 Size Curve Normalization
                //    this.NormalizeSizeCurves);                                               // MID Track 4861 Size Curve Normalization
                //// end removed unnecessary comments // TT#702 Infinitie Loop when begin date set
                //return sizeResults;
                // end TT#702 Infinite Loop when begin date set 
			}
			catch
			{
				throw;
			}

		}

        // Begin TT#5026 - JSmith - Question about Size Alternates
        public ArrayList GetAlternateSizeCodes(int aStoreRID, int aColorCodeRID, int aSizeCodeRID)
        {
            if (_sizeNeedAlgorithm == null)
            {
                PreProcessing(aColorCodeRID, eSizeProcessControl.SizeCurvesOnly);
            }
            return _sizeNeedAlgorithm.GetAlternateSizeCodes(aStoreRID, aSizeCodeRID);
        }
        // End TT#5026 - JSmith - Question about Size Alternates

		override public void Update(TransactionData td)
		{
			//			bool create = false;
			if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodData = new MethodSizeNeedData(td);
				//				create = true;
			}
			_methodData.SizeGroupRid = base.SizeGroupRid;
			_methodData.SizeCurveGroupRid = base.SizeCurveGroupRid;
			_methodData.MerchType = this._MerchType;
			_methodData.MerchPhRid = this._MerchPhRid;
			_methodData.MerchPhlSequence = this._MerchPhlSequence;
			_methodData.MerchHnRid = this._MerchHnRid;
			_methodData.MaxPackNeedTolerance = this._MaxPackNeedTolerance;
			_methodData.SizeAlternateRid = this._sizeAlternateRid;
			_methodData.SizeConstraintRid = this._sizeConstraintRid;
			// _methodData.SizeFringeRid = this._sizeFringeRid; // MID Track 3619 Remove Fringe
			_methodData.AvgPackDeviationTolerance = this._AvgPackDeviationTolerance;
			
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
			// end Generic Size Constraint data

			// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
			_methodData.NormalizeSizeCurves = NormalizeSizeCurves;
			_methodData.NormalizeSizeCurvesDefaultIsOverridden = NormalizeSizeCurvesDefaultIsOverridden;
			// END MID Track #4826

            // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
            _methodData.OverrideAvgPackDevTolerance = OverrideAvgPackDevTolerance;
            _methodData.OverrideMaxPackNeedTolerance = OverrideMaxPackNeedTolerance;
            // End  TT#356 
            // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            _methodData.UseDefaultCurve = base.UseDefaultCurve;
            // End TT#413
            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
            _methodData.ApplyRulesOnly = base.ApplyRulesOnly;
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
            // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            _methodData.GenCurveNsccdRID = base.GenCurveNsccdRID;
            // End TT#413
            // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            _methodData.PackToleranceNoMaxStep = _packToleranceNoMaxStep;
            _methodData.PackToleranceStepped = _packToleranceStepped;
            // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            // BEGIN TT#41-MD - GTaylor - UC#2
            _methodData.IB_MERCH_HN_RID = IB_MERCH_HN_RID;
            _methodData.IB_MERCH_PH_RID = IB_MERCH_PH_RID;
            //_methodData.Inventory_Ind = Inventory_Ind;
            _methodData.IB_MERCH_PHL_SEQ = IB_MERCH_PHL_SEQ;
            _methodData.IB_MerchandiseType = IB_MerchandiseType;
            // END TT#41-MD - GTaylor - UC#2
            // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
            _methodData.OverrideVSWSizeConstraints = OverrideVSWSizeConstraints;
            _methodData.VSWSizeConstraints = (eVSWSizeConstraints) VSWSizeConstraints; 
            // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
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
			catch
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
			SizeNeedMethod newSizeNeedMethod = null;

			try
			{
				newSizeNeedMethod = (SizeNeedMethod)this.MemberwiseClone();
				newSizeNeedMethod.AvgPackDeviationTolerance = AvgPackDeviationTolerance;
				newSizeNeedMethod.ConstraintsLoaded = ConstraintsLoaded;
				newSizeNeedMethod.GenCurveCharGroupRID = GenCurveCharGroupRID;
				newSizeNeedMethod.GenCurveColorInd = GenCurveColorInd;
				newSizeNeedMethod.GenCurveHash = GenCurveHash;
				newSizeNeedMethod.GenCurveHnRID = GenCurveHnRID;
				newSizeNeedMethod.GenCurveMerchType = GenCurveMerchType;
				newSizeNeedMethod.GenCurvePhlSequence = GenCurvePhlSequence;
				newSizeNeedMethod.GenCurvePhRID = GenCurvePhRID;
				newSizeNeedMethod.GetDimensionsUsing = GetDimensionsUsing;
				newSizeNeedMethod.GetSizes = GetSizes;
				newSizeNeedMethod.GetSizesUsing = GetSizesUsing;
				newSizeNeedMethod.MaxPackNeedTolerance = MaxPackNeedTolerance;
				newSizeNeedMethod.MerchHnRid = MerchHnRid;
				newSizeNeedMethod.MerchPhlSequence = MerchPhlSequence;
				newSizeNeedMethod.MerchPhRid = MerchPhRid;
				newSizeNeedMethod.MerchType = MerchType;
				newSizeNeedMethod.Method_Change_Type = eChangeType.none;
				newSizeNeedMethod.Method_Description = Method_Description;
				newSizeNeedMethod.MethodConstraints = MethodConstraints;
				newSizeNeedMethod.MethodStatus = MethodStatus;
				newSizeNeedMethod.Name = Name;
				newSizeNeedMethod.PromptAttributeChange = PromptAttributeChange;
				newSizeNeedMethod.PromptSizeChange = PromptSizeChange;
				newSizeNeedMethod.SG_RID = SG_RID;
				newSizeNeedMethod.SizeAlternateRid = SizeAlternateRid;
				newSizeNeedMethod.SizeConstraintRid = SizeConstraintRid;
				newSizeNeedMethod.SizeCurveGroupRid = SizeCurveGroupRid;
				newSizeNeedMethod.SizeGroupRid = SizeGroupRid;
				newSizeNeedMethod.SMBD = SMBD;
				newSizeNeedMethod.User_RID = User_RID;
				newSizeNeedMethod.Virtual_IND = Virtual_IND;
                newSizeNeedMethod.Template_IND = Template_IND;
                // BEGIN MID Track #4826 - JSmith - Normalize Size Curves
                newSizeNeedMethod.NormalizeSizeCurves = NormalizeSizeCurves;
				newSizeNeedMethod.NormalizeSizeCurvesDefaultIsOverridden = NormalizeSizeCurvesDefaultIsOverridden;
				// END MID Track #4826

				// begin Generic Size Constraint data
				newSizeNeedMethod.GenConstraintCharGroupRID = GenConstraintCharGroupRID;
				newSizeNeedMethod.GenConstraintColorInd = GenConstraintColorInd;
				newSizeNeedMethod.GenConstraintHash = GenConstraintHash;
				newSizeNeedMethod.GenConstraintHnRID = GenConstraintHnRID;
				newSizeNeedMethod.GenConstraintMerchType = GenConstraintMerchType;
				newSizeNeedMethod.GenConstraintPhlSequence = GenConstraintPhlSequence;
				newSizeNeedMethod.GenConstraintPhRID = GenConstraintPhRID;
				// end Generic Size Constraint data
				newSizeNeedMethod.CreateConstraintData();
				newSizeNeedMethod.BuildRulesDecoder(SG_RID);

                // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                newSizeNeedMethod.OverrideAvgPackDevTolerance = OverrideAvgPackDevTolerance;
                newSizeNeedMethod.OverrideMaxPackNeedTolerance = OverrideMaxPackNeedTolerance;
                // End  TT#356 
                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                newSizeNeedMethod.PackToleranceNoMaxStep = _packToleranceNoMaxStep;
                newSizeNeedMethod.PackToleranceStepped = _packToleranceStepped;
                // end TT#1365 - Jellis - FL Detail Pack Size Need Enhancement

                // BEGIN TT#41-MD - GTaylor - UC#2
                newSizeNeedMethod.IB_MERCH_HN_RID = IB_MERCH_HN_RID;
                newSizeNeedMethod.IB_MERCH_PH_RID = IB_MERCH_PH_RID;
                newSizeNeedMethod.IB_MERCH_PHL_SEQ = IB_MERCH_PHL_SEQ;
                //newSizeNeedMethod.Inventory_Ind = Inventory_Ind;
                newSizeNeedMethod.IB_MerchandiseType = IB_MerchandiseType;
                // END TT#41-MD - GTaylor - UC#2
                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                newSizeNeedMethod.OverrideVSWSizeConstraints = OverrideVSWSizeConstraints;
                newSizeNeedMethod.VSWSizeConstraints = (eVSWSizeConstraints)VSWSizeConstraints;
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                return newSizeNeedMethod;
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
            
            if (_MerchType == eMerchandiseType.Node)
            {
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_MerchHnRid, (int)eSecurityTypes.Store);
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
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalSizeNeed);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserSizeNeed);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            //RO-3885 Data Transport for Size Need Method
            //throw new NotImplementedException("MethodGetData is not implemented");
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>();
            ROMethodSizeNeedProperties method = new ROMethodSizeNeedProperties(  
                method: GetName.GetMethod(method: this),
                description: _methodData.Method_Description,
                userKey: User_RID,
                merch_HN: GetName.GetLevelKeyValuePair(_methodData.MerchType, nodeRID: _methodData.MerchHnRid , merchPhRID: _methodData.MerchPhRid, merchPhlSequence: _methodData.MerchPhlSequence, SAB: SAB),
                merch_PH_RID: _methodData.MerchPhRid,
                merch_PHL_SEQ: _methodData.MerchPhlSequence,
                merchandiseType: EnumTools.VerifyEnumValue(_methodData.MerchType),
                normalizeSizeCurvesDefaultIsOverridden: _methodData.NormalizeSizeCurvesDefaultIsOverridden,
                normalizeSizeCurves: _methodData.NormalizeSizeCurves,
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
                sizeRuleAttributeSet: SizeRuleAttributeSet.BuildSizeRuleAttributeSet(_methodData.Method_RID, eMethodType.SizeNeedAllocation, _methodData.SG_RID , _methodData.SizeGroupRid, _methodData.SizeCurveGroupRid, GetSizesUsing, GetDimensionsUsing, MethodConstraints, SAB),
                isTemplate: Template_IND
            );

            return method;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            //RO-3885 Data Transport for Size Need Method
            ROMethodSizeNeedProperties roMethodSizeNeedAllocationProperties = (ROMethodSizeNeedProperties)methodProperties;

            try
            {
                Method_Description = roMethodSizeNeedAllocationProperties.Description;
                User_RID = roMethodSizeNeedAllocationProperties.UserKey;
                _MerchPhRid = roMethodSizeNeedAllocationProperties.Merch_PH.Key;
                _MerchPhlSequence = roMethodSizeNeedAllocationProperties.Merch_PH.Value;
                MerchType = roMethodSizeNeedAllocationProperties.MerchandiseType;
                switch (MerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        _MerchHnRid = Include.Undefined;
                        break;
                    default: //eMerchandiseType.Node
                        _MerchHnRid = roMethodSizeNeedAllocationProperties.Merch_HN.Key;
                        break;
                }
                NormalizeSizeCurvesDefaultIsOverridden = roMethodSizeNeedAllocationProperties.NormalizeSizeCurvesDefaultIsOverridden;
                NormalizeSizeCurves = roMethodSizeNeedAllocationProperties.NormalizeSizeCurves;
                SizeGroupRid  = roMethodSizeNeedAllocationProperties.SizeGroup.Key;
                _sizeAlternateRid = roMethodSizeNeedAllocationProperties.SizeAlternateModel.Key;
                //Size Curve Group Box 
                SizeCurveGroupRid = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.SizeCurve.Key;
                GenCurveNsccdRID = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.SizeCurveGenericNameExtension.Key;
                GenCurveMerchType = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.GenCurveMerchType;
                switch (GenCurveMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenCurveHnRID = Include.NoRID;
                        break;
                    default: //eMerchandiseType.Node
                        GenCurveHnRID = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.SizeCurveGenericHierarchy.Key;
                        break;
                }

                UseDefaultCurve = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.IsUseDefault;
                ApplyRulesOnly = roMethodSizeNeedAllocationProperties.ROSizeCurveProperties.IsApplyRulesOnly;
                // Constraints Group Box
                IB_MerchandiseType = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.InventoryBasisMerchType;
                switch (IB_MerchandiseType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        _IB_MERCH_HN_RID = Include.NoRID;
                        break;
                    default: //eMerchandiseType.Node
                        _IB_MERCH_HN_RID = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.InventoryBasis.Key;
                        break;
                }
                _sizeConstraintRid = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.SizeConstraint.Key;
                GenConstraintMerchType = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.GenConstraintMerchType;
                switch (GenConstraintMerchType)
                {
                    case eMerchandiseType.HierarchyLevel:
                    case eMerchandiseType.LevelOffset:
                    case eMerchandiseType.OTSPlanLevel:
                        GenConstraintHnRID = Include.NoRID; 
                        break;
                    default: //eMerchandiseType.Node
                        GenConstraintHnRID = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHierarchy.Key;
                        break;
                }
                GenConstraintCharGroupRID = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.SizeConstraintGenericHeaderChar.Key;
                GenConstraintColorInd = roMethodSizeNeedAllocationProperties.ROSizeConstraintProperties.GenConstraintColorInd;
                //VSW 
                _overrideVSWSizeConstraints = roMethodSizeNeedAllocationProperties.OverrideVSWSizeConstraints;
                _vSWSizeConstraints = roMethodSizeNeedAllocationProperties.VSWSizeConstraints;
                _overrideAvgPackDevTolerance = roMethodSizeNeedAllocationProperties.OverrideAvgPackDevTolerance;
                if (roMethodSizeNeedAllocationProperties.OverrideAvgPackDevTolerance)
                {
                    AvgPackDeviationTolerance = roMethodSizeNeedAllocationProperties.AvgPackDeviationTolerance;
                }
                _overrideMaxPackNeedTolerance = roMethodSizeNeedAllocationProperties.OverrideMaxPackNeedTolerance;
                _packToleranceStepped = roMethodSizeNeedAllocationProperties.PackToleranceStepped;
                _packToleranceNoMaxStep = roMethodSizeNeedAllocationProperties.PackToleranceNoMaxStep;
                if (roMethodSizeNeedAllocationProperties.OverrideMaxPackNeedTolerance)
                {
                    MaxPackNeedTolerance = roMethodSizeNeedAllocationProperties.MaxPackNeedTolerance;
                }
                //Rules Tab
                SG_RID = roMethodSizeNeedAllocationProperties.Attribute.Key;
                MethodConstraints = SizeRuleAttributeSet.BuildMethodConstrainst( roMethodSizeNeedAllocationProperties.Method.Key, roMethodSizeNeedAllocationProperties.Attribute.Key, 
                    roMethodSizeNeedAllocationProperties.SizeRuleAttributeSet, MethodConstraints, SAB); // MethodConstraints will be regenerated based on above changes
                
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
