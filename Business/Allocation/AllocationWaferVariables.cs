using System;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{

	/// <summary>
	/// Summary description for AllocationWaferVariables.
	/// </summary>
	public class AllocationWaferVariable: Profile
	{
		private string _name;
		private eAllocationWaferVariableFormat _format;
		private int _numDecimals;

//		public AllocationWaferVariable(eAllocationWaferVariable aKey):
//			base((int)aKey)
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//		}

//		public AllocationWaferVariable(eAllocationWaferVariable aKey,string aName, eAllocationWaferVariableFormat aFormat, int aNumDecimals)
		public AllocationWaferVariable(eAllocationWaferVariable aKey, eAllocationWaferVariableFormat aFormat, int aNumDecimals)
		:base ((int)aKey)
		{
//			_name = aName;
			_name = MIDText.GetTextOnly((int)aKey);
			_format = aFormat;
			_numDecimals = aNumDecimals;
		}

		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.AllocationVariables;
			}
		}

		public int NumDecimals
		{
			get
			{
				return _numDecimals;
			}
		}
	
		public eAllocationWaferVariableFormat Format
		{
			get
			{
				return _format;
			}
		}
		public string DefaultLabel
		{
			get { return _name; }
		}
	
	}

	public class AllocationWaferVariables
	{
		private static ProfileList _profileList;
		
		private static AllocationWaferVariable _none = new AllocationWaferVariable(
			eAllocationWaferVariable.None,
//			"None",
			eAllocationWaferVariableFormat.String,
			0);
		private static AllocationWaferVariable _need = new AllocationWaferVariable(
			eAllocationWaferVariable.Need,
//			"Need",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _percentNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.PercentNeed,
//			"%",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _origUnitsToAllocate = new AllocationWaferVariable(
			eAllocationWaferVariable.OriginalQuantityAllocated,
//			"Original Units Allocated",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _quantityAllocated = new AllocationWaferVariable(
			eAllocationWaferVariable.QuantityAllocated,
//			"Quantity Allocated",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _onHand = new AllocationWaferVariable(
			eAllocationWaferVariable.OnHand,
//			"On Hand",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sales = new AllocationWaferVariable(
			eAllocationWaferVariable.Sales,
//			"Plan Sales",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _stock = new AllocationWaferVariable(
			eAllocationWaferVariable.Stock,
//			"Plan Stock",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _intransit = new AllocationWaferVariable(
			eAllocationWaferVariable.InTransit,
//			MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit),
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _storeGrade = new AllocationWaferVariable(
			eAllocationWaferVariable.StoreGrade,
//			"Grade",
			eAllocationWaferVariableFormat.String,
			0);
		private static AllocationWaferVariable _appliedRule = new AllocationWaferVariable(
			eAllocationWaferVariable.AppliedRule,
//			"Rule",
//			eAllocationWaferVariableFormat.eRuleType,
			eAllocationWaferVariableFormat.String,
			0);
		private static AllocationWaferVariable _ruleResults = new AllocationWaferVariable(
			eAllocationWaferVariable.RuleResults,
//			"Rule Qty",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _openToShip = new AllocationWaferVariable(
			eAllocationWaferVariable.OpenToShip,
//			"Open to ship (need)",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _OTSVariance = new AllocationWaferVariable(
			eAllocationWaferVariable.OTSVariance,
//			"OTS Variance",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _qtyReceived = new AllocationWaferVariable(
			eAllocationWaferVariable.QtyReceived,
//			"Received",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _storeCount = new AllocationWaferVariable(
			eAllocationWaferVariable.StoreCount,
//			"# Stores",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _avgStore = new AllocationWaferVariable(
			eAllocationWaferVariable.AverageStore,
//			"Avg",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _balance = new AllocationWaferVariable(
			eAllocationWaferVariable.Balance,
//			"Balance",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _total = new AllocationWaferVariable(
			eAllocationWaferVariable.Total,
//			"Total",
			eAllocationWaferVariableFormat.Number,
			0);
        private static AllocationWaferVariable _avgWklySales = new AllocationWaferVariable(
			eAllocationWaferVariable.AvgWeeklySales,
//			"Avg Weekly Sales",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _avgWklyStock = new AllocationWaferVariable(
			eAllocationWaferVariable.AvgWeeklyStock,
//			"Avg Weekly Stock",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _basisInTransit = new AllocationWaferVariable(
			eAllocationWaferVariable.BasisInTransit,
//			"Basis " + MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit),
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _basisOnHand = new AllocationWaferVariable(
			eAllocationWaferVariable.BasisOnHand,
//			"Basis OnHand",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _basisSales = new AllocationWaferVariable(
			eAllocationWaferVariable.BasisSales,
//			"Basis Sales",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _basisStock = new AllocationWaferVariable(
			eAllocationWaferVariable.BasisStock,
//			"Basis Stock",
			eAllocationWaferVariableFormat.Number,
			0);
        //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
        private static AllocationWaferVariable _basisVSWOnHand = new AllocationWaferVariable(
            eAllocationWaferVariable.BasisVSWOnHand,
            //			"Basis VSW OnHand",
            eAllocationWaferVariableFormat.Number,
            0);
        //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
        private static AllocationWaferVariable _pctSellThru = new AllocationWaferVariable(
			eAllocationWaferVariable.PctSellThru,
//			"Pct SellThru",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _pctSellThruIdx = new AllocationWaferVariable(
			eAllocationWaferVariable.PctSellThruIdx,
//			"Pct SellThru Index",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _velocityGrade = new AllocationWaferVariable(
			eAllocationWaferVariable.VelocityGrade,
//			"Velocity Grade",
			eAllocationWaferVariableFormat.String,
			0);
        // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
        private static AllocationWaferVariable _assortmentGrade = new AllocationWaferVariable(
            eAllocationWaferVariable.AssortmentGrade,
            //			"Assortment Grade",
            eAllocationWaferVariableFormat.String,
            0);
        // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
		private static AllocationWaferVariable _velocityRuleType = new AllocationWaferVariable(
			eAllocationWaferVariable.VelocityRuleType,
//			"Velocity Rule",
			eAllocationWaferVariableFormat.eRuleType,
//			eAllocationWaferVariableFormat.String,
			0);
		private static AllocationWaferVariable _velocityRuleQty = new AllocationWaferVariable(
			eAllocationWaferVariable.VelocityRuleQty,
//			"Velocity Rule Qty",
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _velocityRuleResult = new AllocationWaferVariable(
			eAllocationWaferVariable.VelocityRuleResult,
//			"Velocity Result",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _styleOnHand = new AllocationWaferVariable(
			eAllocationWaferVariable.StyleOnHand,
//			"Header OnHand",
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _styleInTransit = new AllocationWaferVariable(
			eAllocationWaferVariable.StyleInTransit,
//			"Header " + MIDText.GetTextOnly(eMIDTextCode.lbl_Intransit),
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _transfer = new AllocationWaferVariable(
			eAllocationWaferVariable.Transfer,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizeTotalAllocated = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeTotalAllocated,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizeInTransit = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeInTransit,
			eAllocationWaferVariableFormat.Number,
			0);
        // BEGIN TT#1401 - AGallagher - VSW
        private static AllocationWaferVariable _sizeVSWOnHand = new AllocationWaferVariable(
            eAllocationWaferVariable.SizeVSWOnHand,
            eAllocationWaferVariableFormat.Number,
            0);
        // END TT#1401 - AGallagher - VSW
		private static AllocationWaferVariable _sizeOnHand = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeOnHand,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizeOnHandIT = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeOnHandPlusIT,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizePlan = new AllocationWaferVariable(
			eAllocationWaferVariable.SizePlan,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizeCurvePct = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeCurvePct,
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _sizeNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeNeed,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizePosNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.SizePositiveNeed,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizePctNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.SizePctNeed,
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _sizePosPctNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.SizePositivePctNeed,
			eAllocationWaferVariableFormat.Number,
			2);
		private static AllocationWaferVariable _pctToTotal = new AllocationWaferVariable(
			eAllocationWaferVariable.PctToTotal,
			eAllocationWaferVariableFormat.Number,
			Include.DecimalPositionsInStoreSizePctToColor);
		private static AllocationWaferVariable _currentWeekToDaySales = new AllocationWaferVariable(
			eAllocationWaferVariable.CurrentWeekToDaySales,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _sizeSales = new AllocationWaferVariable(
			eAllocationWaferVariable.SizeSales,
			eAllocationWaferVariableFormat.Number,
			0);
		// begin MID Track 3209 Show Actual OnHand and InTransit on Size Review (not curve adjusted)
		private static AllocationWaferVariable _curveAdjdSizeOnHand = new AllocationWaferVariable(
			eAllocationWaferVariable.CurveAdjdSizeOnHand,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _curveAdjdSizeInTransit = new AllocationWaferVariable(
			eAllocationWaferVariable.CurveAdjdSizeInTransit,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _curveAdjdSizeOnHandPlusIT = new AllocationWaferVariable(
			eAllocationWaferVariable.CurveAdjdSizeOnHandPlusIT,
			eAllocationWaferVariableFormat.Number,
			0);
		// end MID Track 3209 Show Actual OnHand and InTransit on Size Review (not curve adjusted)
		// begin MID Track 3880 Add Ship Day as variable for Style and Size Review
		private static AllocationWaferVariable _shipDay = new AllocationWaferVariable(
			eAllocationWaferVariable.ShipToDay,
			eAllocationWaferVariableFormat.String,
			0);
		// end MID Track 3880 Add Ship Day as variable for Style and Size Review
		// begin MID Track 4291 Enhancement Add Fill Variables to Size Review
		private static AllocationWaferVariable _needDay = new AllocationWaferVariable(
			eAllocationWaferVariable.NeedDay,
			eAllocationWaferVariableFormat.String,
			0);
		private static AllocationWaferVariable _fillSizeOwnPlan = new AllocationWaferVariable(   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariable.FillSizeOwnPlan,                                            // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _fillSizeOwnNeed = new AllocationWaferVariable(   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariable.FillSizeOwnNeed,                                            // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _fillSizeOwnPctNeed = new AllocationWaferVariable(  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariable.FillSizeOwnPctNeed,                                           // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			eAllocationWaferVariableFormat.Number,
			2);
		// end MID Track 4291 Enhancement Add Fill Variables to Size Review
		// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
		private static AllocationWaferVariable _preSizeAllocated = new AllocationWaferVariable(
			eAllocationWaferVariable.PreSizeAllocated,
			eAllocationWaferVariableFormat.Number,
			0);
		// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

		// begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
		private static AllocationWaferVariable _fillSizeFwdForecastSales = new AllocationWaferVariable(
			eAllocationWaferVariable.FillSizeFwdForecastSales,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _fillSizeFwdForecastStock = new AllocationWaferVariable(
			eAllocationWaferVariable.FillSizeFwdForecastStock,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _fillSizeFwdForecastPlan = new AllocationWaferVariable(
			eAllocationWaferVariable.FillSizeFwdForecastPlan,
			eAllocationWaferVariableFormat.Number,
			0);
		private static AllocationWaferVariable _fillSizeFwdForecastNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.FillSizeFwdForecastNeed,
			eAllocationWaferVariableFormat.Number,
			0);
        private static AllocationWaferVariable _fillSizeFwdForecastPctNeed = new AllocationWaferVariable(
			eAllocationWaferVariable.FillSizeFwdForecastPctNeed,
			eAllocationWaferVariableFormat.Number,
			2);
		// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        // begin MID Track 6079 Zero Quantity not accepted after Sort
        private static AllocationWaferVariable _sortSequence = new AllocationWaferVariable(
            eAllocationWaferVariable.SortSequence,
            eAllocationWaferVariableFormat.Number,
            0);
        // end MID Track 6079 Zero Quantity not accepted after Sort
        // begin TT#59 Implement Temp Locks
        private static AllocationWaferVariable _storeAllocationPriority = new AllocationWaferVariable(
            eAllocationWaferVariable.StorePriority,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _availableCapacity = new AllocationWaferVariable(
            eAllocationWaferVariable.AvailableCapacity,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _capacityExceedPct = new AllocationWaferVariable(
            eAllocationWaferVariable.CapacityExceedByPct,
            eAllocationWaferVariableFormat.Number,
            2);
        private static AllocationWaferVariable _capacityMaximum = new AllocationWaferVariable(
            eAllocationWaferVariable.CapacityMaximum,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _capacityMaximumReached = new AllocationWaferVariable(
            eAllocationWaferVariable.CapacityMaximumReached,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _mayExceedCapacity = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreMayExceedCapacity,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _mayExceedMaximum = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreMayExceedMaximum,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _usedCapacity = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreUsedCapacity,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _pctNeedLimitReached = new AllocationWaferVariable(
            eAllocationWaferVariable.StorePercentNeedLimitReached,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _aloctnFromBottomUpSize = new AllocationWaferVariable(
            eAllocationWaferVariable.AllocationFromBottomUpSize,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _aloctnFromSizeBreakout = new AllocationWaferVariable(
            eAllocationWaferVariable.AllocationFromSizeBreakout,
            eAllocationWaferVariableFormat.String,
            0);  
        private static AllocationWaferVariable _aloctnFromPackNeed = new AllocationWaferVariable(
            eAllocationWaferVariable.AllocationFromPackNeed,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _aloctnModifiedAftMultiSplit = new AllocationWaferVariable(
            eAllocationWaferVariable.AllocationModifiedAftMultiSplit,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _colorMaximum = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreColorMaximum,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _colorMinimum = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreColorMinimum,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _filledSizeHoles = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreFilledSizeHoles,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _hadNeed = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreHadNeed,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _manuallyAllocated = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreManuallyAllocated,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _storeMaximum = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreMaximum,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _storeMinimum = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreMinimum,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _ruleFromChild = new AllocationWaferVariable(
            eAllocationWaferVariable.RuleAllocationFromChild,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _ruleFromChosenRule = new AllocationWaferVariable(
            eAllocationWaferVariable.RuleAllocationFromChosenRule,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _ruleFromParent = new AllocationWaferVariable(
            eAllocationWaferVariable.RuleAllocationFromParent,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _shippingStatus = new AllocationWaferVariable(
            eAllocationWaferVariable.ShippingStatus,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _unitNeedBefore = new AllocationWaferVariable(
            eAllocationWaferVariable.UnitNeedBefore,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _wasAutoAllocated = new AllocationWaferVariable(
            eAllocationWaferVariable.WasAutoAllocated,
            eAllocationWaferVariableFormat.String,
            0);
        private static AllocationWaferVariable _qtyAllocatedByAuto = new AllocationWaferVariable(
            eAllocationWaferVariable.QtyAllocatedByAuto,
            eAllocationWaferVariableFormat.Number,
            0); 
        private static AllocationWaferVariable _qtyAllocatedByRule = new AllocationWaferVariable(
            eAllocationWaferVariable.QtyAllocatedByRule,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _qtyShipped = new AllocationWaferVariable(
            eAllocationWaferVariable.QtyShipped,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _pctNeedBefore = new AllocationWaferVariable(
            eAllocationWaferVariable.PercentNeedBefore,
            eAllocationWaferVariableFormat.Number,
            2);
        // end TT#59 Implement Temp Locks


        //  begin tt#152 Velocity Balance - apicchetti
        private static AllocationWaferVariable _initialRuleQty = new AllocationWaferVariable(
            eAllocationWaferVariable.VelocityInitialRuleQty,
            eAllocationWaferVariableFormat.Number, 0);

        private static AllocationWaferVariable _initialRuleType = new AllocationWaferVariable(
            eAllocationWaferVariable.VelocityInitialRuleType,
            eAllocationWaferVariableFormat.eRuleType, 0);

        private static AllocationWaferVariable _initialWillShip = new AllocationWaferVariable(
            eAllocationWaferVariable.VelocityInitialWillShip,
            eAllocationWaferVariableFormat.Number, 0);

        //  end tt#152 Velocity Balance - apicchetti

        // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

        private static AllocationWaferVariable _AvgWeeksOfSupply = new AllocationWaferVariable(
           eAllocationWaferVariable.AvgWeeksOfSupply,
           eAllocationWaferVariableFormat.Number, 2);
		
        // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        private static AllocationWaferVariable _VelocityRuleTypeQty = new AllocationWaferVariable(
           eAllocationWaferVariable.VelocityRuleTypeQty,
           eAllocationWaferVariableFormat.Number, 2);

        private static AllocationWaferVariable _VelocityInitialRuleTypeQty = new AllocationWaferVariable(
           eAllocationWaferVariable.VelocityInitialRuleTypeQty,
           eAllocationWaferVariableFormat.Number, 2);
        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        private static AllocationWaferVariable _basisGrade = new AllocationWaferVariable(
        eAllocationWaferVariable.BasisGrade,
        eAllocationWaferVariableFormat.String, 0);
        // End TT#638  

        // BEGIN TT#1401 - AGallagher - Reservation Stores
        private static AllocationWaferVariable _storeItemQuantityAllocated = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreItemQuantityAllocated,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _storeIMOQuantityAllocated = new AllocationWaferVariable(
            eAllocationWaferVariable.StoreIMOQuantityAllocated,
            eAllocationWaferVariableFormat.Number,
            0);
        private static AllocationWaferVariable _storeIMOMaxQuantityAllocated = new AllocationWaferVariable(
           eAllocationWaferVariable.StoreIMOMaxQuantityAllocated,
            // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
           //eAllocationWaferVariableFormat.String, 
           eAllocationWaferVariableFormat.Number,
            // End TT#2225 
           0);
        private static AllocationWaferVariable _storeIMOHistoryMaxQuantityAllocated = new AllocationWaferVariable(
           eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated,
           eAllocationWaferVariableFormat.Number,
           0);
        // END TT#1401 - AGallagher - Reservation Stores


		static AllocationWaferVariables()
		{
			_profileList = new ProfileList(eProfileType.AllocationVariables);
			_profileList.Add(_none);
			_profileList.Add(_need);
			_profileList.Add(_percentNeed);
			_profileList.Add(_quantityAllocated);
			_profileList.Add(_origUnitsToAllocate);
			_profileList.Add(_onHand);
			_profileList.Add(_intransit);
			_profileList.Add(_sales);
			_profileList.Add(_stock);
			_profileList.Add(_storeGrade);
			_profileList.Add(_appliedRule);
			_profileList.Add(_ruleResults);
			_profileList.Add(_openToShip);
			_profileList.Add(_OTSVariance);
			_profileList.Add(_qtyReceived);
			_profileList.Add(_storeCount);
			_profileList.Add(_avgStore);
			_profileList.Add(_balance);
			_profileList.Add(_total);
			_profileList.Add(_avgWklySales);
			_profileList.Add(_avgWklyStock);
			_profileList.Add(_basisInTransit);
			_profileList.Add(_basisOnHand);
			_profileList.Add(_basisSales);
            _profileList.Add(_basisStock);
            _profileList.Add(_basisVSWOnHand);  //TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
            _profileList.Add(_pctSellThru);
			_profileList.Add(_pctSellThruIdx);
			_profileList.Add(_velocityGrade);
			_profileList.Add(_velocityRuleType);
			_profileList.Add(_velocityRuleQty);
			_profileList.Add(_velocityRuleResult);
			_profileList.Add(_styleOnHand);
			_profileList.Add(_styleInTransit);
			_profileList.Add(_transfer);
			_profileList.Add(_sizeTotalAllocated);
			_profileList.Add(_sizeInTransit);
			_profileList.Add(_sizeOnHand);
            _profileList.Add(_sizeVSWOnHand);   // TT#1401 - AGallagher - VSW
			_profileList.Add(_sizeOnHandIT);
			_profileList.Add(_sizePlan);
			_profileList.Add(_sizeCurvePct);
			_profileList.Add(_sizeNeed);
			_profileList.Add(_sizePosNeed);
			_profileList.Add(_sizePctNeed);
			_profileList.Add(_sizePosPctNeed);
			_profileList.Add(_pctToTotal);
			_profileList.Add(_currentWeekToDaySales);
			_profileList.Add(_sizeSales);
		// begin MID Track 3209 Show Actual OnHand and InTransit on Size Review (not curve adjusted)
			_profileList.Add(_curveAdjdSizeOnHand);
			_profileList.Add(_curveAdjdSizeInTransit);
			_profileList.Add(_curveAdjdSizeOnHandPlusIT);
		// end MID Track 3209 Show Actual OnHand and InTransit on Size Review (not curve adjusted)
			// begin MID Track 3880 Add Ship Day as variable in Style and Size Review
			_profileList.Add(_shipDay);
			// end MID Track 3880 Add Ship Day as variable in Style and Size Review
			// begin MID Track 4291 Add Fill Variables to Size Review
			_profileList.Add(_needDay);
			_profileList.Add(_fillSizeOwnPlan);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			_profileList.Add(_fillSizeOwnNeed);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			_profileList.Add(_fillSizeOwnPctNeed);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
			// end MID Track 4291 Add Fill Variables to Size Review
			_profileList.Add(_preSizeAllocated); // MID Track 4282 Velocity overlays Fill Size Holes Allocation
			// begin MID track 4921 AnF#666 Fill to Size Plan Enhancement
			_profileList.Add(_fillSizeFwdForecastSales); 
			_profileList.Add(_fillSizeFwdForecastStock); 
			_profileList.Add(_fillSizeFwdForecastPlan); 
			_profileList.Add(_fillSizeFwdForecastNeed); 
			_profileList.Add(_fillSizeFwdForecastPctNeed); 
			// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            _profileList.Add(_sortSequence); // MID Track 6079 Zero Quantity not accepted after Sort
            // begin TT#59 Implement Temp Locks
            _profileList.Add(_storeAllocationPriority);
            _profileList.Add(_availableCapacity);
            _profileList.Add(_capacityExceedPct);
            _profileList.Add(_capacityMaximum);
            _profileList.Add(_capacityMaximumReached);
            _profileList.Add(_mayExceedCapacity);
            _profileList.Add(_mayExceedMaximum);
            _profileList.Add(_usedCapacity);
            _profileList.Add(_pctNeedLimitReached);
            _profileList.Add(_aloctnFromBottomUpSize);
            _profileList.Add(_aloctnFromSizeBreakout);
            _profileList.Add(_aloctnFromPackNeed);
            _profileList.Add(_aloctnModifiedAftMultiSplit);
            _profileList.Add(_colorMaximum);
            _profileList.Add(_colorMinimum);
            _profileList.Add(_filledSizeHoles);
            _profileList.Add(_hadNeed);
            _profileList.Add(_manuallyAllocated);
            _profileList.Add(_storeMaximum);
            _profileList.Add(_storeMinimum);
            _profileList.Add(_ruleFromChild);
            _profileList.Add(_ruleFromChosenRule);
            _profileList.Add(_ruleFromParent);
            _profileList.Add(_shippingStatus);
            _profileList.Add(_unitNeedBefore);
            _profileList.Add(_wasAutoAllocated);
            _profileList.Add(_qtyAllocatedByAuto);
            _profileList.Add(_qtyAllocatedByRule);
            _profileList.Add(_qtyShipped);
            _profileList.Add(_pctNeedBefore);
        // end TT#59 Implement Temp Locks

            //begin tt#152 - Velocity balance - apicchetti
            _profileList.Add(_initialRuleQty);
            _profileList.Add(_initialRuleType);
            _profileList.Add(_initialWillShip);
            //end tt#152 - Velocity balance - apicchetti

            _profileList.Add(_AvgWeeksOfSupply); // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
            _profileList.Add(_VelocityRuleTypeQty); // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            _profileList.Add(_VelocityInitialRuleTypeQty); // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            _profileList.Add(_basisGrade);                 // TT#638 - RMatelic - Style Review - Add Basis Variables
            _profileList.Add(_storeItemQuantityAllocated); // TT#1401 - AGallagher - Reservation Stores
            _profileList.Add(_storeIMOQuantityAllocated); // TT#1401 - AGallagher - Reservation Stores
            _profileList.Add(_storeIMOMaxQuantityAllocated); // TT#1401 - AGallagher - Reservation Stores
            _profileList.Add(_storeIMOHistoryMaxQuantityAllocated); // TT#1401 - AGallagher - Reservation Stores
            _profileList.Add(_assortmentGrade);   // TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
		}

		public static AllocationWaferVariable GetVariableProfile(eAllocationWaferVariable aKey)
		{
			return (AllocationWaferVariable)_profileList.FindKey((int)aKey);
		}

		static eAllocationWaferVariableFormat GetFormatType(eAllocationWaferVariable aKey)
		{
			return ((AllocationWaferVariable)(_profileList.FindKey((int)aKey))).Format;
		}

	}
}
