using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Collections;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
    [Serializable]
    public class BuildPacksMethod : AllocationBaseMethod
    {
        #region Fields
        private Audit _audit;
        private MIDException _statusReason;

        private BuildPacksMethodData _bpmData;
        private bool _isInteractive;

        // Calculate Pack Options work fields
        private MIDGenericSortDescendingComparer _midSortDescendComparer;
        private ApplicationSessionTransaction _appTran;
        private GeneralComponent _component;
        private AllocationProfile _workUpBulkSizeBuy;
        private HdrColorBin _workUpColorBin;
        MIDGenericSortItem[] _sortWorkUpBuyDescendSizeUnits;
        MIDGenericSortItem[] _sortWorkUpBuyCurvePosOrder;
        SizeUnits[] _workUpBuySizeUnits;
        Dictionary<int, int> _sizeRIDCurvePosIdxXref;  // RID, Index xref to _sortWorkUpBuyCurvePosOrder

        private int _sizeCount;
        private int _desiredReserveTotalUnits;
        private int _desiredReserveBulkUnits;
        private int _desiredReservePacksUnits;
        private int _reserveStoreRID;
        private Index_RID[] _storeIdxRIDArray;
        private int[] _nonReserveStoreRID;
        private OptionPackProfileList _oPPL;

        #endregion Fields
        
        #region Constructor
        public BuildPacksMethod(SessionAddressBlock SAB, int aMethodRID) : 
        //Begin TT#523 - JScott - Duplicate folder when new folder added
            //base(SAB, aMethodRID, eMethodType.BuildPacks)
            base(SAB, aMethodRID, eMethodType.BuildPacks, eProfileType.MethodBuildPacks)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _audit = SAB.ApplicationServerSession.Audit;
            _bpmData = new BuildPacksMethodData();
            _isInteractive = false;
            if (base.Filled)
            {
                MIDException aStatusReason = null;
                if (!_bpmData.PopulateBuildPacks(aMethodRID, out aStatusReason))
                {
                    if (aStatusReason.ExpandMidErrorMessage)
                    {
                        aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, aStatusReason.InnerException);
                    }
                    throw aStatusReason;
                }
            }
            _sizeCount = 0;
            _desiredReserveTotalUnits = 0;
            _desiredReserveBulkUnits  = 0;
            _desiredReservePacksUnits = 0;
            _midSortDescendComparer = new MIDGenericSortDescendingComparer();
        }
        #endregion Constructor

        #region Properties
        public override eProfileType ProfileType
        {
            get { return eProfileType.MethodBuildPacks; }
        }

        // begin TT#689 Pack Coverage is too small
        public ApplicationSessionTransaction AppTransaction
        {
            get { return _appTran; }
        }
        // end TT#689 Pack Coverage is too small

        /// <summary>
        /// Gets or sets the Interactive flag
        /// </summary>
        public bool IsInteractive
        {
            get
            {
                return _isInteractive;
            }

            set
            {
                _isInteractive = value;
            }
        }
        /// <summary>
		/// Gets or sets the Component Type
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				if (_component == null)
				{
					_component = new GeneralComponent(eGeneralComponentType.Total);
				}

				return _component;
			}

			set
			{
				_component = value;
			}
		}
        /// <summary>
        /// Gets or sets the name of the BuildPacksMethod
        /// </summary>
        public string BuildPacksMethodName
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        /// <summary>
        /// Gets or sets the description of the BuildPacksMethod
        /// </summary>
        public string BuildPacksMethodDescription
        {
            get
            {
                return base.Method_Description;
            }
            set
            {
                base.Method_Description = value;
            }
        }
        /// <summary>
        /// Gets the BPC_Name (aka Vendor Name) associated with this BuildPacksMethod.
        /// </summary>
        /// <remarks>To set or change the BPC_Name use SetBPC_Name method.</remarks>
        public string BPC_Name
        {
            get { return _bpmData.BPC_Name; }
        }
        /// <summary>
        /// Gets the VendorName (aka BPC_Name) associated with the BuildPacksMethod.
        /// </summary>
        /// <remarks>To set or change the vendor name use SetVendorName method.</remarks>
        public string VendorName
        {
            get { return BPC_Name; }
        }
        /// <summary>
        /// Gets pack minimum order for this BuildPacksMethod     // TT#787 Vendor Min Order applies only to packs
        /// </summary>
        /// <remarks>To set or change the pack minimum order use SetPackMinOrder method.</remarks> // TT#787 Vendor Min Order applies only to packs
        public int PackMinOrder               // TT#787 Vendor Min Order applies only to packs
        {
            get { return _bpmData.Pack_MIN; } // TT#787 Vendor Min Order applies only to packs
        }
        /// <summary>
        /// Gets the bulk size multiple for this BuildPacksMethod (identifies the multiple in which each bulk size must be ordered).
        /// </summary>
        /// <remarks>To set or change the size multiple use SetSizeMultiple method</remarks>
        public int SizeMultiple
        {
            get { return _bpmData.SizeMultiple; }
        }
        /// <summary>
        /// Gets the size group RID associated with this BuildPacksMethod (mutually exclusive with SizeCurveGroupRID).
        /// </summary>
        /// <remarks>To set or change the Size Group use the SetSizeGroupRID method</remarks>
        public int SizeGroupRID
        {
            get { return _bpmData.SizeGroupRID; }
        }
        /// <summary>
        /// Gets the size curve group RID associated with this BuildPacksMethod (mutually exclusive with SizeGroupRID).
        /// </summary>
        /// <remarks>To set or change the Size Curve Group use the SetSizeCurveGroupRID method</remarks>
        public int SizeCurveGroupRID
        {
            get { return _bpmData.SizeCurveGroupRID; }
        }
        public int PackCombinationCount
        {
            get { return _bpmData.CombinationCount; }
        }
        /// <summary>
        /// Gets a copy of the Pack Combinations associated with this BuildPacksMethod.
        /// </summary>
        public List<PackPatternCombo> PackCombination
        {
            get { return _bpmData.CombinationList; }
        }
        /// <summary>
        /// Gets the Reserve Total Quantity
        /// </summary>
        /// <remarks>To set or change the Reserve Total, use the SetReserveTotal method</remarks>
        public double ReserveTotal
        {
            get { return _bpmData.ReserveTotal; }
        }
        /// <summary>
        /// Gets or sets whether the Reserve Total Quantity is a percent.
        /// </summary>
        public bool ReserveTotalIsPercent
        {
            get { return _bpmData.ReserveTotalIsPercent; }
            set { _bpmData.ReserveTotalIsPercent = value; }
        }
        /// <summary>
        /// Gets the Reserve Bulk Quantity.
        /// </summary>
        /// <remarks>To set or change the Reserve Bulk, use the SetReserveBulk method</remarks>
        public double ReserveBulk
        {
            get { return _bpmData.ReserveBulk; }
        }
        /// <summary>
        /// Gets or sets whether the Reserve Bulk Quantity is a percent.
        /// </summary>
        public bool ReserveBulkIsPercent
        {
            get { return _bpmData.ReserveBulkIsPercent; }
            set { _bpmData.ReserveBulkIsPercent = value; }
        }
        /// <summary>
        /// Gets the Reserve Packs Quantity.
        /// </summary>
        public double ReservePacks
        {
            get { return _bpmData.ReservePacks; }
        }
        /// <summary>
        /// Gets or sets whether the Reserve Packs Quantity is a percent or units.
        /// </summary>
        public bool ReservePacksIsPercent
        {
            get { return _bpmData.ReservePacksIsPercent; }
            set { _bpmData.ReservePacksIsPercent = value; }
        }
        // begin TT#744 - JEllis - Use Orig Pack FItting Logic; REmove Bulk From Header
        /// <summary>
        /// Gets or sets option to remove bulk after fitting packs
        /// </summary>
        public bool RemoveBulkAfterFittingPacks
        {
            get { return _bpmData.RemoveBulkAfterFittingPacks; }
            set { _bpmData.RemoveBulkAfterFittingPacks = value; }
        }
        // end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk From Header
        /// <summary>
        /// Gets or sets the average pack size error deviation tolerance.
        /// </summary>
        public double AvgPackErrorDevTolerance      // Correction
        {
            get
            {
                return _bpmData.AvgPackErrorDevTolerance;
            }
            set
            {
                _bpmData.AvgPackErrorDevTolerance = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum pack size error deviation tolerance (aka Ship Variance).
        /// </summary>
        public uint MaxPackErrorDevTolerance
        {
            get
            {
                return _bpmData.MaxPackErrorDevTolerance;
            }
            set
            {
                _bpmData.MaxPackErrorDevTolerance = value;
            }
        }
        /// <summary>
        /// Gets or sets the ship vaiance (aka maximum pack size error deviation tolerance).
        /// </summary>
        public uint ShipVariance
        {
            get
            {
                return MaxPackErrorDevTolerance;
            }
            set
            {
                MaxPackErrorDevTolerance = value;
            }
        }
        // begin TT#669 Build Packs Variance Enhancement
        public bool DepleteReserveSelected
        {
            get { return _bpmData.DepleteReserveSelected; }
            set { _bpmData.DepleteReserveSelected = value; }
        }
        public bool IncreaseBuySelected
        {
            get { return _bpmData.IncreaseBuySelected; }
            set { _bpmData.IncreaseBuySelected = value; }
        }
        public double IncreaseBuyPct
        {
            get { return _bpmData.IncreaseBuyPct; }
        }
        // end TT#xxx Build Packs Variance Enhancement

        /// <summary>
        /// Gets a list of BPC names.
        /// </summary>
        public DataTable BPCNames
        {
            get
            {
                return _bpmData.GetBPCList();
            }
        }
        /// <summary>
        /// Gets the Work Up Buy for which packs are being built
        /// </summary>
        public AllocationProfile WorkUpSizeBuy
        {
            get
            {
                return _workUpBulkSizeBuy;
            }
        }
        /// <summary>
        /// Gets an Option Pack Profile List containing the results of the BuildPacks Method
        /// </summary>
        public OptionPackProfileList OptionPackProfiles
        {
            get
            {
                if (_oPPL == null)
                {
                    return new OptionPackProfileList();
                }
                return (OptionPackProfileList)_oPPL.Clone();
            }
        }
        /// <summary>
        /// Gets a list of the Option Pack Profiles in "best" to "least" order
        /// </summary>
        public List<OptionPackProfile> OptionPackBestToLeastOrder
        {
            get
            {
                List<OptionPackProfile> oppL = new List<OptionPackProfile>();
                if (_oPPL != null)
                {
                    MIDGenericSortItem[] sortOptionPatterns = new MIDGenericSortItem[_oPPL.Count];
                    int optionPosition = 0;
                    double[] sortKey;
                    foreach (OptionPackProfile opp in _oPPL)
                    {
                        // begin TT#801 - BP Need more pack select criteria
                        sortKey = new double[9];
                        sortKey[0] = opp.PercentNonReserveUnitsInPacks;     // descending
                        sortKey[1] = opp.CountOfNonReserveStoresWithPacks;  // descending
                        sortKey[2] = opp.PercentOriginalBuyPackaged;        // descending
                        sortKey[3] = opp.OriginalBuyPackUnits;              // descending
                        sortKey[4] = -opp.ShipVariance;                     // ascending
                        sortKey[5] = -opp.AvgErrorPerSizeInError;           // ascending
                        sortKey[6] = -opp.AvgErrorPerPack;                  // ascending
                        sortKey[7] = -opp.StorePackBuy.Length;              // ascending (number unique packs)
                        sortKey[8] = _appTran.GetRandomDouble();
                        //sortKey = new double[6];                  
                        //sortKey[0] = opp.PercentNonReserveUnitsInPacks;     // descending
                        //sortKey[1] = opp.CountOfNonReserveStoresWithPacks;  // descending
                        //sortKey[2] = -opp.ShipVariance;                     // ascending
                        //sortKey[3] = -opp.AvgErrorPerSizeInError;           // ascending
                        //sortKey[4] = -opp.AvgErrorPerPack;                  // ascending
                        //sortKey[5] = _appTran.GetRandomDouble();
                        // end TT#801 - BP Need more pack select criteria
                        sortOptionPatterns[optionPosition].Item = opp.Key;
                        sortOptionPatterns[optionPosition].SortKey = sortKey;
                        optionPosition++;
                    }
                    Array.Sort(sortOptionPatterns, _midSortDescendComparer);
                    for (int i = 0; i < sortOptionPatterns.Length; i++)
                    {
                        oppL.Add((OptionPackProfile)_oPPL.FindKey(sortOptionPatterns[i].Item));
                    }
                }
                return oppL;
            }


        }
        /// <summary>
        /// Gets the size code list of sizes in build packs method
        /// </summary>
        public SizeCodeList SizeCodeList
        {
            get
            {
                // begin TT#615 Size Group, Size Curve and Size Run issues
                //SizeCodeList scl = new SizeCodeList(eProfileType.SizeCode);
                SizeCodeList scl;
                if (_bpmData.SizeCurveGroupRID != Include.NoRID)
                {
                    SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(_bpmData.SizeCurveGroupRID);
                    ProfileList pl = scgp.GetSizeCodeList();
                    scl = new SizeCodeList(eProfileType.SizeCode);
                    foreach (SizeCodeProfile scp in pl)
                    {
                        scl.Add(scp);
                    }
                }
                else if (_bpmData.SizeGroupRID != Include.NoRID)
                {
                    SizeGroupProfile sgp = new SizeGroupProfile(_bpmData.SizeGroupRID);
                    scl = sgp.SizeCodeList;
                }
                else
                {
                    scl = new SizeCodeList(eProfileType.SizeCode);
                }
                // end TT#615 Size Group, Size Curve and Size Run issues

                foreach (PackPatternCombo ppc in _bpmData.CombinationList)
                {
                    foreach (PackPattern pp in ppc.PackPatternList)
                    {
                        if (pp.PatternIncludesSizeRun)
                        {
                            foreach (int sizeCodeRID in pp.SizeRIDs)
                            {
                                if (scl.FindKey(sizeCodeRID) == null)
                                {
                                    SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeRID);
                                    scl.Add(scp);
                                }
                            }
                        }
                    }
                }
                return scl;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Sets the Vendor Name (aka BPC_Name)
        /// </summary>
        /// <param name="aVendorName">Desired vendor name.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetVendorName(string aVendorName, out MIDException aStatusReason)
        {
            return SetBPC_Name(aVendorName, out aStatusReason);
        }
        /// <summary>
        /// Sets the BPC_Name (aka Vendor Name)
        /// </summary>
        /// <param name="aBPC_Name">Desired BPC_Name.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetBPC_Name(string aBPC_Name, out MIDException aStatusReason)
        {
            if (!_bpmData.Set_BPC_Name(aBPC_Name, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the Pack minimum order for this Build Packs Method // TT#787 Vendor Min Order applies only to packs
        /// </summary>
        /// <param name="aPackMinOrder">Pack minimum order</param> // TT#787 Vendor Min Order applies only to packs
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <remarks>The component minimum order cannot be modified when the BPC_Name(aka Vendor) is set becasue in that case the BPC table provides this value.</remarks>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetPackMinOrder(int aPackMinOrder, out MIDException aStatusReason) // TT#787 Vendor Min Order applies only to packs
        {
            if (!_bpmData.SetPackMin(aPackMinOrder, out aStatusReason)) // TT#787 Vendor Min Order applies only to packs
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the multiple in which each bulk size must be ordered.
        /// </summary>
        /// <param name="aSizeMultiple">Size Multiple</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful</param>
        /// <remarks>The size multiple cannot be modified when the BPC_Name(aka Vendor) is set because in that case the BPC table provides this value.</remarks>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetSizeMultiple(int aSizeMultiple, out MIDException aStatusReason)
        {
            if (!_bpmData.SetSizeMultiple(aSizeMultiple, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the size group RID for this Build Pack Method.  A Size Group RID identifies the valid sizes for the method.
        /// </summary>
        /// <param name="aSizeGroupRID">Size Group RID that identifies the valid sizes for this Build Pack Method.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <remarks>Size Group RID is mutually exclusive with Size Curve Group RID.</remarks>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetSizeGroupRID (int aSizeGroupRID, out MIDException aStatusReason)
        {
            if (!_bpmData.SetSizeGroupRID(aSizeGroupRID, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the size curve group RID for this Build Pack Method.  A Size Curve Group RID identifies the valid sizes for the method.
        /// </summary>
        /// <param name="aSizeCurveGroupRID">Size Curve Group RID that identifies the valid sizes for this Build Pack Method.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <remarks>Size Curve Group RID is mutually exclusive with Size Group RID.</remarks>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a rason for the failure.</returns>
        public bool SetSizeCurveGroupRID (int aSizeCurveGroupRID, out MIDException aStatusReason)
        {
            if (!_bpmData.SetSizeCurveGroupRID(aSizeCurveGroupRID, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the selection property of the identified Combination
        /// </summary>
        /// <param name="aPackPatternType">Type of Combination: Vendor or BuildPacksMethod</param>
        /// <param name="aPatternComboRID">Combo RID associated with this combination</param>
        /// <param name="aValue">True: Combination is selected; False: Combination is not selected</param>
        /// <param name="aStatusReason">Reason for failure if the set fails</param>
        /// <returns>True: Set was successful; False: Set was not successful in which case aStatusReason wiil give a reason for the failure</returns>
        public bool SetCombinationSelected(ePackPatternType aPackPatternType, int aPatternComboRID, bool aValue, out MIDException aStatusReason)
        {
            return _bpmData.SetCombinationSelected(aPackPatternType, aPatternComboRID, aValue, out aStatusReason);
        }

        // begin TT#669 Build Packs Variance Enhancement
        /// <summary>
        /// Sets the Increase Buy Pct that is used to control how much the buy can increase when fitting packs to the stores
        /// </summary>
        /// <param name="aIncreaseBuyPct">Increase Buy Percentage (Non-negative)</param>
        /// <param name="aStatusReason">Message describing what is wrong if the set fails</param>
        /// <returns>True: set was successful; False: Set was not successful in which case aStatusReason will give a reason for the failure</returns>
        public bool SetIncreaseBuyPct(double aIncreaseBuyPct, out MIDException aStatusReason)
        {
            return _bpmData.SetIncreaseBuyPct(aIncreaseBuyPct, out aStatusReason);
        }
        // end TT#669 Build Packs Variance Enhancement

        /// <summary>
        /// Sets the Pack Pattern Combination List for this Build Packs Method. Each combination list contains a list of valid pack patterns that may be generated on the target header.
        /// </summary>
        /// <param name="aCombinationList">A list of pack pattern combinations.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool AddCombinations (List<PackPatternCombo> aCombinationList, out MIDException aStatusReason)
        {
            if (!_bpmData.AddCombinations(aCombinationList, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Replaces the current Pack Pattern Combo List with a new list (only the non-vendor pattern combo types will be replaced).
        /// </summary>
        /// <param name="aCombinationList">List of pack pattern combinations that will replace the existing list (this list may be null or empty provided there is a vendor attached to the method having at least one pack pattern combination. </param>
        /// <param name="aStatusReason">The reason the replace failed.</param>
        /// <returns>True: Replace was successful, in this case aStatusReason is null; False: Replace failed, in this case aStatusReason will give a reason for the failure.</returns>
        public bool ReplaceCombinations(List<PackPatternCombo> aCombinationList, out MIDException aStatusReason)
        {
            if (!_bpmData.ReplaceCombinations(aCombinationList, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Removes the existing non-vendor pack pattern combinations from the method; the vendor pattern combos will remain attached; there must be at least one vendor pack pattern in order for the remove to be a success.
        /// </summary>
        /// <param name="aStatusReason">The reason the remove failed.</param>
        /// <returns>True: Remove was successful, in this case aStatusReason will be null; False: Remove failed, in this case aStatusReason will give a reason for the failure.</returns>
        public bool RemoveCombinations(out MIDException aStatusReason)
        {
            if (!_bpmData.RemoveCombinations(out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the reserve total quatity either as number of units or as a percent of the total allocation on the target header.  The property "ReserveTotalAsPercent" determines whether this value is a percent or is number of units.
        /// </summary>
        /// <param name="aReserveTotal">Total quantity to place in reserve.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetReserveTotal (double aReserveTotal, out MIDException aStatusReason)
        {
            if (!_bpmData.SetReserveTotal(aReserveTotal, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the bulk reserve quantity either as a number of units or as a percent of the total reserve unit quantity.  The property "ReserveBulkAsPercent" determines whether this value is a percent or is number of units.
        /// </summary>
        /// <param name="aReserveBulk">Bulk quantity to place in reserve.</param>
        /// <param name="aStatusReason">The reason the set failed; this fies is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetReserveBulk (double aReserveBulk, out MIDException aStatusReason)
        {
            if (!_bpmData.SetReserveBulk(aReserveBulk, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the pack reserve quantity either as a number of units (not packs) or as a percent of the total reserve unit quantity.  The property "ReservePacksPercent" determines whether this value is a percent or is number of units.
        /// </summary>
        /// <param name="aReservePacks">Pack quantity to plack in reserve.</param>
        /// <param name="aStatusReason">The reason the set failed; this field is null when the set is successful.</param>
        /// <returns>True: Set was successful, aStatusReason is null in this case; False: Set failed, aStatusReason gives a reason for the failure.</returns>
        public bool SetReservePacks (double aReservePacks, out MIDException aStatusReason)
        {
            if (!_bpmData.SetReservePacks(aReservePacks, out aStatusReason))
            {
                if (aStatusReason.ExpandMidErrorMessage)
                {
                    aStatusReason = new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                return false;
            }
            aStatusReason = null;
            return true;
        }
      
        /// <summary>
        /// Processes the Build Packs Method for each Bulk Work Up Buy in the Transaction Master Profile List.
        /// </summary>
        /// <param name="aApplicationTransaction">Application Session Transaction attached to this process.</param>
        /// <param name="aStoreFilter">Store Filter attached to this process.</param>
        /// <param name="methodProfile">Build Packs Method Profile.</param>
        public override void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction, 
            int aStoreFilter, 
            Profile methodProfile)
        {
            aApplicationTransaction.ResetAllocationActionStatus();

            AllocationProfileList apl = 
                aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation) as AllocationProfileList;

            if (apl == null
                || apl.Count == 0)
            {
                SAB.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    eMIDTextCode.msg_NoHeaderSelectedOnWorkspace,
                    MIDText.GetTextOnly(eMIDTextCode.frm_BuildPacksMethod) 
                        + " " + Name + ": " 
                        + MIDText.GetTextOnly(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace),
                    this.GetType().Name,
                    true);
            }
            else
            {
                foreach (AllocationProfile ap in apl)
                {
                    AllocationWorkFlowStep awfs = new AllocationWorkFlowStep(this,
                        this.Component,
                        false,
                        true,
                        aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
                        aStoreFilter,
                        -1);

                    ProcessAction(aApplicationTransaction.SAB, aApplicationTransaction, awfs, ap, true, Include.NoRID);
                }
            }
        }
        /// <summary>
        /// Executes the Build Packs Method for a single Bulk Work Up Buy.
        /// </summary>
        /// <param name="aSAB">Session Address Block associated with this action.</param>
        /// <param name="aApplicationTransaction">Application Session Transaction associated with this action.</param>
        /// <param name="aWorkFlowStep">Allocation workflow step to execute.</param>
        /// <param name="aProfile">AllocationProfile with the Bulk Work Up Buy input for this action.</param>
        /// <param name="aWriteToDB">Not used.</param>
        /// <param name="aStoreFilterRID">Store Filter</param>
        public override void ProcessAction(
            SessionAddressBlock aSAB, 
            ApplicationSessionTransaction aApplicationTransaction, 
            ApplicationWorkFlowStep aWorkFlowStep, 
            Profile aProfile, 
            bool aWriteToDB, 
            int aStoreFilterRID)
        {
            if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aWorkFlowStep._method.MethodType))
            {
                throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_WorkflowTypeInvalid), MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
            }
            if (aStoreFilterRID != Include.AllStoreFilterRID
                && aStoreFilterRID != Include.NoRID)
            {
                _audit.Log_Exception(new NotSupportedException("Store Filters not supported by Build Pack Method; Store filter ignored"), GetType().Name);
            }
            AllocationProfile ap = aProfile as AllocationProfile;
            string auditMessage;
            if (ap == null)
            {
                auditMessage = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                _audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMessage,
                    GetType().Name,
                    true);
                throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_NotAllocationProfile), auditMessage);
            }
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                _audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
            {
                string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                _audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                    this.Name + " " + errorMessage,
                    this.GetType().Name);
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
            }
            else
            {
                // End TT#1966-MD - JSmith- DC Fulfillment
                ProcessAction(
                    aApplicationTransaction,
                    (AllocationWorkFlowStep)aWorkFlowStep,
                    (AllocationProfile)aProfile);
            }
        }
        /// <summary>
        /// Executes the Build Packs Method for a single Bulk Work Up Buy.
        /// </summary>
        /// <param name="aApplicationTransaction">Application Session Transaction associted with this action.</param>
        /// <param name="aWorkFlowStep">Allocation workflow step to execute.</param>
        /// <param name="aWorkUpColorSizeBuy">AllocationProfile with the Bulk Work Up Buy input for this action.</param>
        private void ProcessAction(
            ApplicationSessionTransaction aApplicationTransaction,
            AllocationWorkFlowStep aWorkFlowStep,
            AllocationProfile aWorkUpColorSizeBuy)
         {
            eAllocationActionStatus allocationActionStatus = eAllocationActionStatus.ActionFailed;
            this.Component = aWorkFlowStep.Component; 
            string auditMessage = "";
            _oPPL = new OptionPackProfileList();   // Correction
            try
			{
                // Begin TT#2512 - JSmith - Action Failed - Build Pack Method
                //auditMessage =
                //   string.Format(
                //       MIDText.GetTextOnly(eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod),
                //       aWorkUpColorSizeBuy.HeaderID,
                //       this.Name);
                //_audit.Add_Msg(
                //    eMIDMessageLevel.Error,
                //    eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod,
                //    auditMessage,
                //    GetType().Name,
                //    true);
                // End TT#2512
                if (_bpmData.CombinationCount == 0)
                {
                    auditMessage = 
                        string.Format(
                           MIDText.GetTextOnly(eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod), 
                           aWorkUpColorSizeBuy.HeaderID, 
                           this.Name);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error,
                        eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod,
                        auditMessage,
                        GetType().Name,
                        true);
                }
				//BEGIN TT#795 -  MD - DOConnell - Build Packs not working on a Placeholder in an assortment.
                //else if (!aWorkUpColorSizeBuy.WorkUpBulkSizeBuy
				else if ((!aWorkUpColorSizeBuy.Placeholder && !aWorkUpColorSizeBuy.WorkUpBulkSizeBuy)
				//END TT#795 -  MD - DOConnell - Build Packs not working on a Placeholder in an assortment.
                         || aWorkUpColorSizeBuy.BulkColors.Count == 0
                         || aWorkUpColorSizeBuy.GetSizesOnHeader().Count == 0)
                {
                    auditMessage =
                        string.Format(
                        MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy),
                        Name,
                        aWorkUpColorSizeBuy.HeaderID,
                        MIDText.GetTextOnly((int)aWorkUpColorSizeBuy.HeaderType),
                        aWorkUpColorSizeBuy.BulkColors.Count.ToString(CultureInfo.CurrentUICulture),
                        aWorkUpColorSizeBuy.GetSizesOnHeader().Count.ToString(CultureInfo.CurrentUICulture));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Edit,
                        eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy,
                        auditMessage,
                        this.GetType().Name,
                        true);
                }
                else if (aWorkUpColorSizeBuy.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance)
                {
                    auditMessage = string.Format(
                        _audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), 
                        MIDText.GetTextOnly((int)aWorkUpColorSizeBuy.HeaderAllocationStatus));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning, eMIDTextCode.msg_HeaderStatusDisallowsAction,
                        this.Name + " " + (aWorkUpColorSizeBuy.HeaderID + " " + auditMessage),
                        this.GetType().Name,
                        true);
                }
                else if (aWorkUpColorSizeBuy.StyleIntransitUpdated)
                {
                    auditMessage = string.Format(
                        _audit.GetText(eMIDTextCode.msg_al_BuildPacksMethodHeaderIsChargedToIntransit),
                        Name,
                        aWorkUpColorSizeBuy.HeaderID);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_MethodIgnored,
                        auditMessage,
                        this.GetType().Name,
                        true);
                    auditMessage =
                        string.Format(
                        MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
                        aWorkUpColorSizeBuy.HeaderID,
                        MIDText.GetTextOnly(eMIDTextCode.frm_BuildPacksMethod),
                        this.Name);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_MethodIgnored,
                        auditMessage,
                        this.GetType().Name,
                        true);

                }
                else if (aWorkUpColorSizeBuy.WorkUpPackBuy
                         || aWorkUpColorSizeBuy.Packs.Count > 0)
                {
                    auditMessage =
                        string.Format(
                        MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksAlreadyDoneOnHeader),
                        Name,
                        aWorkUpColorSizeBuy.HeaderID,
                        MIDText.GetTextOnly((int)aWorkUpColorSizeBuy.HeaderType),
                        aWorkUpColorSizeBuy.PackCount.ToString(CultureInfo.CurrentUICulture));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Edit,
                        eMIDTextCode.msg_al_BuildPacksAlreadyDoneOnHeader,
                        auditMessage,
                        this.GetType().Name,
                        true);
                }
                else
                {
                    //_oPPL = new OptionPackProfileList();  // Correction
                    if (!aWorkUpColorSizeBuy.StoresLoaded)
                    {
                        aWorkUpColorSizeBuy.LoadStores(false);
                    }
                    _workUpBulkSizeBuy = aWorkUpColorSizeBuy;
                    _appTran = aApplicationTransaction;
                    _reserveStoreRID = _appTran.ReserveStore.RID;
                    _storeIdxRIDArray = _appTran.StoreIndexRIDArray();
                    if (_reserveStoreRID > 0)
                    {
                        _nonReserveStoreRID = new int[_storeIdxRIDArray.Length - 1];
                    }
                    else
                    {
                        _nonReserveStoreRID = new int[_storeIdxRIDArray.Length];
                    }
                    int i = 0;
                    int j = 0;   // Correction
                    for (i = 0; i < _storeIdxRIDArray.Length; i++)
                    {
                        if (_storeIdxRIDArray[i].RID != _reserveStoreRID)
                        {
                            _nonReserveStoreRID[j] = _storeIdxRIDArray[i].RID;
                            j++;  // Correction
                        }
                    }

                    switch (Component.ComponentType)
                    {
                        case (eComponentType.Total):
                        case (eComponentType.Bulk):
                        case (eComponentType.AllColors):
                        case (eComponentType.DetailType):
                            {
                                if (aWorkUpColorSizeBuy.BulkColors.Count != 1)
                                {
                                    // Begin TT#235 MD - JSmith - Action Failed - Build Pack Method
                                    //auditMessage =
                                    //    string.Format(
                                    //    MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy),
                                    //    Name,
                                    //    aWorkUpColorSizeBuy.HeaderID,
                                    //    MIDText.GetTextOnly((int)aWorkUpColorSizeBuy.HeaderType),
                                    //    aWorkUpColorSizeBuy.BulkColors.Count.ToString(CultureInfo.CurrentUICulture));
                                    auditMessage =
                                        string.Format(
                                        MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy),
                                        Name,
                                        aWorkUpColorSizeBuy.HeaderID,
                                        MIDText.GetTextOnly((int)aWorkUpColorSizeBuy.HeaderType),
                                        aWorkUpColorSizeBuy.BulkColors.Count.ToString(CultureInfo.CurrentUICulture),
                                        aWorkUpColorSizeBuy.GetSizesOnHeader().Count.ToString(CultureInfo.CurrentUICulture));
                                    // End TT#235 MD
                                    _audit.Add_Msg(
                                        eMIDMessageLevel.Edit,
                                        eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy,
                                        auditMessage,
                                        GetType().Name,
                                        true);
                                }
                                else
                                {
                                    allocationActionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    foreach (HdrColorBin hcb in aWorkUpColorSizeBuy.BulkColors.Values)
                                    {
                                        allocationActionStatus =
                                            CalculateOptionPacks 
                                            (hcb);
                                        if (allocationActionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case (eComponentType.SpecificColor):
                            {
                                AllocationColorOrSizeComponent colorComponent = Component as AllocationColorOrSizeComponent;
                                if (colorComponent == null)
                                {
                                    auditMessage =
                                        aWorkUpColorSizeBuy.HeaderID
                                        + ": "
                                        + MIDText.GetTextOnly(eMIDTextCode.msg_al_UnknownComponentType);
                                    _audit.Add_Msg(
                                        eMIDMessageLevel.Severe,
                                        eMIDTextCode.msg_al_BuildPacksRequiresWorkUpSizeBuy,
                                        auditMessage,
                                        this.GetType().Name);
                                }
                                else
                                {
                                    HdrColorBin hcb = aWorkUpColorSizeBuy.BulkColors[colorComponent.ColorRID] as HdrColorBin;
                                    if (hcb == null)
                                    {
                                        auditMessage =
                                            aWorkUpColorSizeBuy.HeaderID
                                            + ": "
                                            + MIDText.GetTextOnly(eMIDTextCode.msg_ColorNotDefinedInBulk)
                                            + ": "
                                            + SAB.HierarchyServerSession.GetColorCodeProfile(colorComponent.ColorRID).ColorCodeName
                                            + " (RID=" + colorComponent.ColorRID.ToString() + ")";
                                        _audit.Add_Msg(
                                            eMIDMessageLevel.Warning,
                                            eMIDTextCode.msg_ColorNotDefinedInBulk,
                                            auditMessage,
                                            this.GetType().Name);
                                    }
                                    else
                                    {
                                        allocationActionStatus =
                                            CalculateOptionPacks
                                               (hcb);
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                auditMessage =
                                    aWorkUpColorSizeBuy.HeaderID
                                    + ": "
                                    + MIDText.GetTextOnly(eMIDTextCode.msg_al_UnknownComponentType);
                                _audit.Add_Msg(
                                    eMIDMessageLevel.Severe,
                                    eMIDTextCode.msg_al_UnknownComponentType,
                                    auditMessage,
                                    this.GetType().Name);
                                break;
                            }
                    }
                }
			}
			catch (Exception e)
			{
				allocationActionStatus = eAllocationActionStatus.ActionFailed;
                _audit.Log_Exception(e, GetType().Name, eExceptionLogging.logAllInnerExceptions);
			}
			finally
			{
                aApplicationTransaction.SetAllocationActionStatus(aWorkUpColorSizeBuy.Key, allocationActionStatus);
			}
        }
        /// <summary>
        /// Calculates Option Pack Solutions
        /// </summary>
        /// <param name="aColorBin">Header Color Bin</param>
        /// <returns>AllocationActionStatus</returns>
        private eAllocationActionStatus CalculateOptionPacks(
            HdrColorBin aColorBin)
        {
            eAllocationActionStatus actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
            _workUpColorBin = aColorBin;
            // begin TT#669 Build Packs Method Variance Enhancement
            int increaseBuyQty = 0;
            if (_bpmData.IncreaseBuySelected)
            {
                if (_bpmData.IncreaseBuyPct < double.MaxValue)
                {
                    increaseBuyQty =
                    (int)((double)_workUpColorBin.ColorUnitsAllocated
                                  * _bpmData.IncreaseBuyPct
                                  / 100 + .5d);
                }
                else
                {
                    increaseBuyQty = int.MaxValue;
                }
            }
            // end TT#669 Build Packs Method Variance Enhancement

            StoreSizeVector[] basisStoreBulkSizeBuy =
                GetBasisWorkUpBulkBuy();
            SizeUnits[] basisBulkSizeBuy = new SizeUnits[basisStoreBulkSizeBuy.Length];
            int basisBulkSizeBuyTotal = 0;   // TT#536 Error when no Pack options generated.
            for (int i = 0; i < basisBulkSizeBuy.Length; i++)
            {
                if (_reserveStoreRID > 0)
                {
                    basisBulkSizeBuy[i] = new SizeUnits(basisStoreBulkSizeBuy[i].SizeCodeRID, (int)(basisStoreBulkSizeBuy[i].AllStoreTotalValue - basisStoreBulkSizeBuy[i].GetStoreValue(_reserveStoreRID)));    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                else
                {
                    basisBulkSizeBuy[i] = new SizeUnits(basisStoreBulkSizeBuy[i].SizeCodeRID, (int)basisStoreBulkSizeBuy[i].AllStoreTotalValue);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                }
                basisBulkSizeBuyTotal += basisBulkSizeBuy[i].Units;  // TT#536 Error when no Pack options generated
            }
            // begin TT#536 Error when no Pack options generated
            if (basisBulkSizeBuyTotal == 0)
            {
                _audit.Add_Msg(
                    eMIDMessageLevel.Warning, 
                    eMIDTextCode.msg_al_AllUnitsInReserve, 
                    string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_AllUnitsInReserve), Name, _workUpBulkSizeBuy.HeaderID), 
                    GetType().Name, true);
                actionStatus = eAllocationActionStatus.ActionFailed;
            }
            else
            {
                // end TT#536 Error when no Pack Options generated

                List<PackPatternCombo> ppcList = _bpmData.CombinationList;
                // begin TT#886 - Distinct Packs have same size runs
                foreach (PackPatternCombo ppc in ppcList)
                {
                    if (ppc.ComboSelected)
                    {
                        foreach (PackPattern pp in ppc.PackPatternList)
                        {
                            if (pp.PatternIncludesSizeRun)
                            {
                                int[] sizeRIDs = pp.SizeRIDs;
                                foreach (int sizeRID in sizeRIDs)
                                {
                                    if (!aColorBin.SizeIsInColor(sizeRID))
                                    {
                                        //ppc.ComboName
                                        _audit.Add_Msg(
                                            eMIDMessageLevel.Error,
                                            eMIDTextCode.msg_al_BP_SizeRunContainsSizesNotInHeader,
                                            string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BP_SizeRunContainsSizesNotInHeader),
                                               Name,
                                               _workUpBulkSizeBuy.HeaderID,
                                               ppc.ComboName, 
                                               pp.PatternName,
                                               AppTransaction.GetColorCodeProfile(aColorBin.ColorCodeRID).ColorCodeName,
                                               AppTransaction.GetSizeCodeProfile(sizeRID).SizeCodeName),
                                            GetType().Name, true);
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                }
                            }
                        }
                    }
                }
                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    // end TT#886 - Distinct Packs have same size runs
                    int packPatternComboID = 0;
                    int optionPackKey = 1;
                    int version = 0;
                    int sizeCodeIdx;
                    string optionPackID;
                    int optionPatternKey = 0;
                    foreach (PackPatternCombo ppc in ppcList)
                    {
                        packPatternComboID++;

                        if (ppc.ComboSelected)
                        {
                            for (int variance = (int)_bpmData.MaxPackErrorDevTolerance; variance > -1; variance--)
                            {
                                PackPatternList packPatternList = ppc.PackPatternList;
                                MIDGenericSortItem[] sortPatterns = new MIDGenericSortItem[packPatternList.Count];
                                PackPattern pp;
                                int sortKeyCount = 2 + _sizeCount;  // allow for PackMultiple, each size and a random tie breaker
                                int randomSortPosition = sortKeyCount - 1;
                                double[] sortKey;
                                for (int i = 0; i < packPatternList.Count; i++)
                                {
                                    sortPatterns[i].Item = i;
                                    sortKey = new double[sortKeyCount];
                                    pp = packPatternList[i];
                                    sortKey[0] = pp.PackMultiple;
                                    sizeCodeIdx = 0;
                                    if (pp.PatternIncludesSizeRun)
                                    {
                                        for (int j = 1; j < randomSortPosition; j++)
                                        {
                                            sortKey[j] = pp.GetSizeUnits(_sortWorkUpBuyDescendSizeUnits[sizeCodeIdx].Item); // Item is the RID
                                            sizeCodeIdx = j;
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 1; j < randomSortPosition; j++)
                                        {
                                            sortKey[j] = _sortWorkUpBuyDescendSizeUnits[sizeCodeIdx].SortKey[0] + 1;  // SortKey[0]=NonReserve Size Units Allocated; want this pack pattern to sort before all others with same multiple
                                            sizeCodeIdx = j;
                                        }
                                    }
                                    sortKey[randomSortPosition] = _appTran.GetRandomDouble();
                                    sortPatterns[i].SortKey = sortKey;
                                }
                                Array.Sort(sortPatterns, _midSortDescendComparer);
                                // Pack Patterns are now in dominant size order where dominant means (Process largest pack multiple first; when equal pack multiples, process the packs with largest dominant sizes first)

                                OptionPackList optionPackVersionList = new OptionPackList();
                                pp = packPatternList[sortPatterns[0].Item];
                                if (pp.PatternIncludesSizeRun)
                                {
                                    optionPackVersionList.Add(packPatternList);
                                }
                                else
                                {
                                    PackPatternList optionPatternsList;
                                    SizeUnits[] sizeRun = GetPatternSizeRun(pp.PackMultiple, basisBulkSizeBuy);
                                    OptionPack_PackPattern oppp;
                                    MIDException statusReason;
                                    for (version = 0; version < 2; version++)
                                    {
                                        optionPatternsList = new PackPatternList();
                                        optionPatternKey++;
                                        oppp =
                                           new OptionPack_PackPattern(
                                               optionPatternKey.ToString(CultureInfo.CurrentCulture),
                                               -optionPatternKey,
                                               pp.PatternName,
                                               sizeRun);
                                        optionPatternsList.Add(oppp);  // starting pattern
                                        if (pp.MaxPatternPacks > 1)    // subsequent patterns using original pattern as base
                                        {
                                            int maxPatternPacks = pp.MaxPatternPacks - 1;
                                            PackPattern pp2 =
                                                new BuildPacksMethod_PackPattern(
                                                    this.Key,
                                                    pp.PackPatternRID * -1,
                                                    pp.PatternName,
                                                    pp.PackMultiple,
                                                    maxPatternPacks);
                                            if (!pp2.SetMaxPatternPacks(maxPatternPacks, out statusReason))
                                            {
                                                throw statusReason;
                                            }
                                            optionPatternsList.Add(pp2);
                                        }
                                        for (int i = 1; i < sortPatterns.Length; i++)  // other subsequent patterns that do not derive from starting pattern
                                        {
                                            optionPatternsList.Add(packPatternList[sortPatterns[i].Item]);
                                        }
                                        optionPackVersionList.Add(optionPatternsList);
                                        sizeRun = ModifyPatternSizeRun(sizeRun, basisBulkSizeBuy);
                                    }
                                }
                                version = 0;
                                OptionPack_PackPattern optionPattern;
                                foreach (PackPatternList ppl in optionPackVersionList)
                                {
                                    version++;
                                    optionPackID =
                                    packPatternComboID.ToString(CultureInfo.CurrentUICulture)
                                    + "." + variance.ToString(CultureInfo.CurrentUICulture)
                                    + "." + version.ToString(CultureInfo.CurrentUICulture);
                                    OptionPackProfile opp =
                                        new OptionPackProfile(
                                        _appTran,                              // TT#744 Use Orig Pack Fit algorithm; remove bulk
                                             this,
                                             ppc.ComboName,
                                             ppc.ComboRID,
                                             optionPackKey,
                                             optionPackID,
                                             basisStoreBulkSizeBuy,
                                             _reserveStoreRID,
                                             (uint)variance,
                                             this._bpmData.AvgPackErrorDevTolerance, // TT#535 negative reserve
                                             version,                               // TT#535 negative reserve // TT#669 Build Pack Variance Enhancement
                                             _bpmData.DepleteReserveSelected,       // TT#669 Build Pack Variance Enhancement
                                             increaseBuyQty);                       // TT#669 Build Pack Variance Enhancement
                                    int packID = 0;
                                // begin TT#744 - JEllis - use orig pack fitting logic; remove bulk
                                if (ppl.AllPacksIncludeSizeRun)
                                {
                                    List<OptionPack_PackPattern> pplWithSizeRun = new List<OptionPack_PackPattern>();
                                    foreach (PackPattern packPattern in ppl)
                                    {
                                        packID++;
                                        optionPattern = new OptionPack_PackPattern(
                                            packID.ToString(CultureInfo.CurrentUICulture),
                                            -packID,
                                            optionPackID + "." + packID.ToString(CultureInfo.CurrentUICulture),
                                            packPattern.SizeRun);
                                        pplWithSizeRun.Add(optionPattern);
                                    }
                                    List<MIDException> statusReasonList;
                                    if (!opp.AllocatePacksToStores(pplWithSizeRun, this.PackMinOrder, out statusReasonList))  // TT#849 - Move BP MID Dots enhancement from 3.2 to 4.0
                                    {
                                        foreach (MIDException statusReason in statusReasonList)
                                        {
                                            // begin TT#488 - MD - JEllis - Group Allocation (unrelated: Use Common Translator in Include)
                                            //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(statusReason.ErrorLevel), (eMIDTextCode)statusReason.ErrorNumber, statusReason.ErrorMessage, GetType().Name);
                                            _audit.Add_Msg(Include.TranslateErrorLevel(statusReason.ErrorLevel), (eMIDTextCode)statusReason.ErrorNumber, statusReason.ErrorMessage, GetType().Name);
                                            // end TT#488 - MD - JEllis - Group Allocation (unrelated: Use Common Translator in Include)
                                            if (statusReason.ErrorLevel != eErrorLevel.information)
                                            {
                                                _appTran.SetAllocationActionStatus(_workUpBulkSizeBuy.HeaderRID, eAllocationActionStatus.ActionFailed);
                                                actionStatus = eAllocationActionStatus.ActionFailed;
                                            }
                                        }
                                        if (actionStatus == eAllocationActionStatus.ActionFailed)
                                        {
                                            return actionStatus;
                                        }
                                    }
                                }
                                else
                                {
                                    // end TT#744 - JEllis - use orig pack fitting logic; remove bulk
                                    foreach (PackPattern packPattern in ppl)
                                    {
                                        if (packPattern.PatternIncludesSizeRun)
                                        {
                                            packID++;
                                            optionPattern =
                                                new OptionPack_PackPattern(
                                                    packID.ToString(CultureInfo.CurrentUICulture),
                                                    -packID,
                                                    optionPackID + "." + packID.ToString(CultureInfo.CurrentUICulture),
                                                    packPattern.SizeRun);


                                            if (!opp.ApplyPackToStores(optionPattern, PackMinOrder, out _statusReason)) // TT#787 Vendor Min Order applies only to packs
                                            {
                                                // begin TT#488 - MD - JEllis - Group Allocation (unrelated:  Use Common translator in include)
                                                //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                                                ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                                                _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                // end TT#488 - MD - JEllis - Group Allocation (unrelated:  Use Common Translator in include)
                                                if (_statusReason.ErrorLevel != eErrorLevel.information)
                                                {
                                                    _appTran.SetAllocationActionStatus(_workUpBulkSizeBuy.HeaderRID, eAllocationActionStatus.ActionFailed);
                                                    return eAllocationActionStatus.ActionFailed;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int countPacks = 0; countPacks < packPattern.MaxPatternPacks; countPacks++)
                                            {
                                                // begin TT#612 BP WUB refresh not done after Apply -- unrelated issue: packs allocated all bulk units
                                                packID++;
                                                if (opp.NonReserveBulkBuy == 0)
                                                {
                                                    _audit.Add_Msg(
                                                        eMIDMessageLevel.Information,
                                                        eMIDTextCode.msg_al_BuildPacksAllUnitsPackaged,
                                                        string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksAllUnitsPackaged), Name, _workUpBulkSizeBuy.HeaderID, optionPackID, packID, packPattern.PackMultiple),
                                                        GetType().Name, true);
                                                }
                                                else
                                                {
                                                    // end TT#612 BP WUB refresh not done after Apply -- unrelated issue: packs allocated all bulk units

                                                    SizeUnits[] sur = GetPatternSizeRun(packPattern.PackMultiple, opp.NonReserveBulkSizeBuy);
                                                    //packID++; // TT#612 BP WUB refresh not done after Apply -- unrelated issue: packs allocated all bulk units
                                                    optionPattern =
                                                        new OptionPack_PackPattern(
                                                            packID.ToString(CultureInfo.CurrentUICulture),
                                                            -packID,
                                                            optionPackID + "." + packID.ToString(CultureInfo.CurrentUICulture),
                                                            sur);
                                                    if (!opp.ApplyPackToStores(optionPattern, PackMinOrder, out _statusReason)) // TT#787 Vendor Min Order applies only to packs
                                                    {
                                                        // begin TT#488 - MD - JEllis - Group Allocation (unrelated: Use Common Translator in Include)
                                                        //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                                                        ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                        //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                        //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                                                        _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                        // end TT#488 - MD - Jellis - Group Allocation (unrelated: Use Common Translator in Include)
                                                        if (_statusReason.ErrorLevel != eErrorLevel.information)
                                                        {
                                                            _appTran.SetAllocationActionStatus(_workUpBulkSizeBuy.HeaderRID, eAllocationActionStatus.ActionFailed);
                                                            actionStatus = eAllocationActionStatus.ActionFailed;
                                                            break;
                                                        }
                                                    }
                                                }  // TT#612 BP WUB refresh not done after Apply -- unrelated issue: packs allocated all bulk units
                                            }
                                        }

                                    }
                                }  // TT#744 - JEllis - Use orig pack fitting logic; remove bulk

                                    if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                    {
                                        //if (!opp.AdjustBulkTotals(_appTran, CompMinOrder, SizeMultiple, out _statusReason)) // TT#787 - Vendor Minimum Applies to Packs Only
                                        if (!opp.AdjustBulkTotals(_appTran, 0, SizeMultiple, out _statusReason))              // TT#787 - Vendor Minimum Applies to Packs Only
                                        {
                                            // begin TT#488 - MD - JEllis - Group Allocation (unrelated: Use common translator in include)
                                            //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                                            ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                            //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                            //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                                            _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                            // end TT#488 - MD - JEllis - Group Allocation (unrelated: Use common translator in include)
                                            if (_statusReason.ErrorLevel != eErrorLevel.information)
                                            {
                                                _appTran.SetAllocationActionStatus(_workUpBulkSizeBuy.HeaderRID, eAllocationActionStatus.ActionFailed);
                                                actionStatus = eAllocationActionStatus.ActionFailed;
                                            }
                                        }
                                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                        {
                                            if (_desiredReservePacksUnits > 0)
                                            {
                                                if (!opp.CalculatePackReserve(_appTran, _desiredReservePacksUnits, out _statusReason))
                                                {
                                                    // begin TT#488 - MD - JEllis - Group Allocation (unrelated: Use common translator in include)
                                                    //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                                                    ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                                                    //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                    //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                                                    _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                                                    // end TT#488 - MD - JEllis - Group Allocation (unrelated: Use common translator in include)
                                                    if (_statusReason.ErrorLevel != eErrorLevel.information)
                                                    {
                                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                                    }
                                                }
                                            }
                                        }
                                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                        {
                                            if (opp.AllStoreTotalPackUnits > 0)
                                            {
                                                _oPPL.Add(opp);
                                            }
                                            else
                                            {
                                                _audit.Add_Msg(
                                                    eMIDMessageLevel.Information,
                                                    eMIDTextCode.msg_al_NoPacksGeneratedForOption,
                                                    string.Format(MIDText.GetTextOnly(
                                                           eMIDTextCode.msg_al_NoPacksGeneratedForOption),
                                                           Name,
                                                           _workUpBulkSizeBuy.HeaderID,
                                                           opp.FromPackPatternComboName,
                                                           opp.ShipVariance.ToString(),
                                                           opp.Version,
                                                           opp.OptionPackID),
                                                    GetType().Name);
                                            }
                                            //version++;  // TT#599 BP not all versions of options are generated
                                            optionPackKey++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                                {
                                    break;
                                }
                            }
                            if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                            {
                                break;
                            }
                        }
                    }
                }  // TT#886 - Distinct Packs have same size runs
            }  // #TT 536 Error when no pack options generated
            if (_oPPL.Count == 0)
            {
                _audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_al_NoPackPatternsGenerated,
                    string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_NoPackPatternsGenerated), Name, _workUpBulkSizeBuy.HeaderID),
                    GetType().Name, true);
                actionStatus = eAllocationActionStatus.ActionFailed;
                // begin TT#580 build packs creates duplicate solutions
                //} else if (!this._isInteractive) 
            }
            else
            {
                List<OptionPackProfile> oppL = OptionPackBestToLeastOrder;
                // end TT#580 build packs creates duplicate solutions
                if (!this._isInteractive)
                {
                    MIDException statusReason;
                    //if (!this.ApplySelectedOptionPackProfile(OptionPackBestToLeastOrder[0].Key, out statusReason)) // TT#580 build packs creates duplicate solutions
                    if (!this.ApplySelectedOptionPackProfile(oppL[0].Key, out statusReason))                         // TT#580 build packs creates duplicate solutions
                    {
                        // begin TT#488 - MD - JEllis - Group Allocation (unrelated: Use common translator in Include)
                        //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                        ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                        //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                        //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
                        _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                        // end TT#488 - MD - JEllis - Group Allocation (unrelated: use common translator in Include)
                        actionStatus = eAllocationActionStatus.ActionFailed;
                    }
                    // begin TT#580 build packs creates duplicate solutions
                }
                else
                {
                    _oPPL = EliminateDuplicateSolutions(oppL);
                }
                // end TT#580 build packs creates duplicate solutions
            }
            return actionStatus;
        }
        // begin TT#488 - MD - JEllis - Group ALlocation (unrelated:  use "common" translator in Include) 
        //// begin TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
        //private eMIDMessageLevel ConvertErrorLevelToMessageLevel(eErrorLevel aErrorLevel)
        //{
        //    // begin TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
        //    //switch (_statusReason.ErrorLevel) //TT#801 - BP need more pack select criteria (unrelated issue)
        //    switch (aErrorLevel)                 // TT#801 - BP need more pack select criteria (unrelated issue)
        //    {
        //        case eErrorLevel.information:
        //            {
        //                return eMIDMessageLevel.Information;
        //            }
        //        case eErrorLevel.warning:
        //            {
        //                return eMIDMessageLevel.Warning;
        //            }
        //        case eErrorLevel.severe:
        //        case eErrorLevel.fatal:
        //            {
        //                return eMIDMessageLevel.Severe;
        //            }
        //        default:
        //            {
        //                return eMIDMessageLevel.None;
        //            }
        //    }
        //}
        // end TT#488 - MD - JEllis -Group Allocation (unrelated:  use "common" translator in Include)
        //// end TT#744 - Jellis - Use Orig Pack Fitting Logic; REmove Bulk
        // end TT#1403 - JEllis -Group Allocation (unrelated:  use "common" translator in Include)
        /// <summary>
        /// Calculates the reserve quantity by size in the work up buy and returns the adjusted quantities by store-size.
        /// </summary>
        /// <returns></returns>
        private StoreSizeVector[] GetBasisWorkUpBulkBuy()
        {
            _sizeCount = _workUpColorBin.ColorSizes.Count;
            _sortWorkUpBuyDescendSizeUnits = new MIDGenericSortItem[_sizeCount];
            _sortWorkUpBuyCurvePosOrder = new MIDGenericSortItem[_sizeCount];
            _workUpBuySizeUnits = new SizeUnits[_sizeCount];
            StoreSizeVector[] ssv = new StoreSizeVector[_sizeCount];
            MIDGenericSortItem[] sortStores = new MIDGenericSortItem[_nonReserveStoreRID.Length];
            StoreVector storeTotalUnits = new StoreVector(); // TT#689 Pack coverage too small

            int sizeCodeIdx = 0;
            foreach (HdrSizeBin hsb in _workUpColorBin.ColorSizes.Values)
            {
                _workUpBuySizeUnits[sizeCodeIdx] = new SizeUnits(hsb.SizeCodeRID, hsb.SizeUnitsAllocated);
                _sortWorkUpBuyCurvePosOrder[sizeCodeIdx].Item = hsb.SizeCodeRID;
                _sortWorkUpBuyCurvePosOrder[sizeCodeIdx].SortKey = new double[2];
                _sortWorkUpBuyCurvePosOrder[sizeCodeIdx].SortKey[0] = -hsb.SizeSequence; // want sizes in ascending sequence by sequence number
                _sortWorkUpBuyCurvePosOrder[sizeCodeIdx].SortKey[1] = -1;   // keep sizes with same sequence in same order after the sort.
                sizeCodeIdx++;
            }
            // use size group or curve if present to sequence the sizes
            Array.Sort(_sortWorkUpBuyCurvePosOrder, _midSortDescendComparer);
            _sizeRIDCurvePosIdxXref = new Dictionary<int, int>();
            for (sizeCodeIdx = 0; sizeCodeIdx < _sizeCount; sizeCodeIdx++)
            {
                ssv[sizeCodeIdx] = new StoreSizeVector(_sortWorkUpBuyCurvePosOrder[sizeCodeIdx].Item);
                _sizeRIDCurvePosIdxXref.Add(_sortWorkUpBuyCurvePosOrder[sizeCodeIdx].Item, sizeCodeIdx);
            }

            int sizePosition;
            int[] nonReserveSizeTotal = new int[_sizeCount];
            int nonReserveTotal = 0;
            int reserveTotal = 0;
            int storeSizeValue;
            foreach (HdrSizeBin hsb in _workUpColorBin.ColorSizes.Values)
            {
                sizePosition = _sizeRIDCurvePosIdxXref[hsb.SizeCodeRID];
                ssv[sizePosition] = new StoreSizeVector(hsb.SizeCodeRID);
                foreach (Index_RID storeIdxRID in _storeIdxRIDArray)
                {
                    storeSizeValue = hsb.GetStoreSizeUnitsAllocated(storeIdxRID.Index);
                    ssv[sizePosition].SetStoreValue(storeIdxRID.RID, storeSizeValue);
                    storeTotalUnits.SetStoreValue(storeIdxRID.RID, storeTotalUnits.GetStoreValue(storeIdxRID.RID) + storeSizeValue); // TT#689 Pack coverage too small
                }
                nonReserveSizeTotal[sizePosition] = (int)ssv[sizePosition].AllStoreTotalValue;    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                if (_reserveStoreRID > 0)
                {
                    int reserveValue = (int)ssv[sizePosition].GetStoreValue(_reserveStoreRID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                    reserveTotal += reserveValue;
                    nonReserveSizeTotal[sizePosition] -= reserveValue;
                }
                nonReserveTotal += nonReserveSizeTotal[sizePosition];
                _sortWorkUpBuyDescendSizeUnits[sizePosition].Item = hsb.SizeCodeRID;
                _sortWorkUpBuyDescendSizeUnits[sizePosition].SortKey = new double[2];
                _sortWorkUpBuyDescendSizeUnits[sizePosition].SortKey[0] = 
                    - nonReserveSizeTotal[sizePosition];                     // TT#689 Pack coverage too small
                _sortWorkUpBuyDescendSizeUnits[sizePosition].SortKey[1] = _appTran.GetRandomDouble();
            }
            // begin TT#689 Pack coverage too small
            //Array.Sort(_sortWorkUpBuyDescendSizeUnits, _midSortDescendComparer);  // puts sizes in descending sequence by non-reserve size total order  
            Array.Sort(_sortWorkUpBuyDescendSizeUnits, _midSortDescendComparer);  // puts sizes in ASCENDING sequence by non-reserve size total order (subtle diff in how reserve units are calculated)
            // end TT#689 Pack coverage too small

            if (_reserveStoreRID > 0)
            {
                if (_bpmData.ReserveTotalIsPercent)
                {
                    _desiredReserveTotalUnits =
                        (int)(((double)(nonReserveTotal + reserveTotal)
                         * this._bpmData.ReserveTotal
                         / 100d) + .5d);
                }
                else
                {
                    _desiredReserveTotalUnits = (int)_bpmData.ReserveTotal;
                }
                if (_desiredReserveTotalUnits < reserveTotal)
                {
                    string message =
                        string.Format(
                           MIDText.GetTextOnly(eMIDTextCode.msg_al_WorkUpBuyReserveExceedsBuildPacksReserve),
                           Name, 
                           _workUpBulkSizeBuy.HeaderID,
                           reserveTotal,                           
                           _desiredReserveTotalUnits);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Information,
                        eMIDTextCode.msg_al_WorkUpBuyReserveExceedsBuildPacksReserve,
                        message,
                        GetType().Name,
                        false);
                    _desiredReserveTotalUnits = reserveTotal; // Cannot recover "extra" reserve (spreading will distort Size Need by store in this case).
                }
                if (_bpmData.ReserveBulkIsPercent)
                {
                    _desiredReserveBulkUnits  =
                        (int)(((double)(_desiredReserveTotalUnits)
                        * _bpmData.ReserveBulk
                        / 100d) + .5d);
                }
                else
                {
                    _desiredReserveBulkUnits  = (int)_bpmData.ReserveBulk;
                }
                if (_desiredReserveBulkUnits  < reserveTotal)
                {
                    string message =
                        string.Format(
                           MIDText.GetTextOnly(eMIDTextCode.msg_al_WorkUpBuyBulkReserveExceedsBuildPacksReserve),
                           Name,
                           _workUpBulkSizeBuy.HeaderID,
                           reserveTotal,
                           _desiredReserveBulkUnits );
                    _audit.Add_Msg(
                        eMIDMessageLevel.Information,
                        eMIDTextCode.msg_al_WorkUpBuyBulkReserveExceedsBuildPacksReserve,
                        message,
                        GetType().Name,
                        false);
                    _desiredReserveBulkUnits  = reserveTotal; // Cannot recover "extra" reserve (spreading will distort Size Need by store in this case).
                }
                if (_desiredReserveBulkUnits  > _desiredReserveTotalUnits)
                {
                    string message =
                        string.Format(
                           MIDText.GetTextOnly(eMIDTextCode.msg_al_WorkUpBuyDesiredBulkExceedsDesiredTotalReserve),
                           Name,
                           _workUpBulkSizeBuy.HeaderID,
                           _desiredReserveTotalUnits,
                           _desiredReserveBulkUnits );
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_WorkUpBuyDesiredBulkExceedsDesiredTotalReserve,
                        message,
                        GetType().Name,
                        false);
                    _desiredReserveTotalUnits = _desiredReserveBulkUnits ;
                }
                if (_bpmData.ReservePacksIsPercent)
                {
                    _desiredReservePacksUnits =
                        (int)(((double)(_desiredReserveTotalUnits)
                        * _bpmData.ReservePacks
                        / 100d) + .5d);
                }
                else
                {
                    _desiredReservePacksUnits = (int)_bpmData.ReservePacks;
                }
                if (_desiredReservePacksUnits > _desiredReserveTotalUnits - _desiredReserveBulkUnits )
                {
                    string message =
                        string.Format(
                           MIDText.GetTextOnly(eMIDTextCode.msg_al_WorkUpBuyDesiredPacksExceedsDesiredTotalBulkReserveDiff),
                           Name,
                           _workUpBulkSizeBuy.HeaderID,
                           _desiredReserveTotalUnits,
                           _desiredReserveBulkUnits ,
                           _desiredReservePacksUnits);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_WorkUpBuyDesiredPacksExceedsDesiredTotalBulkReserveDiff,
                        message,
                        GetType().Name,
                        false);
                    _desiredReserveTotalUnits = _desiredReserveBulkUnits  + _desiredReservePacksUnits;
                }
                else
                {
                    _desiredReservePacksUnits = _desiredReserveTotalUnits - _desiredReserveBulkUnits ;
                }
                if (_desiredReserveBulkUnits  > reserveTotal)
                {
                    // put more "bulk" in reserve (packs are put in reserve at end when number packs known)
                    // begin TT#689 Pack Coverage is too small
                    #region TT#689--deleted code
                    //int addToReserve = _desiredReserveBulkUnits  - reserveTotal;
                    //int newNonReserveTotal;
                    //int oldNonReserveTotal = nonReserveTotal;
                    //int newSizeTotal = 0;
                    //int oldSizeTotal = 0;
                    //int newStoreValue;
                    //int oldStoreValue;
                    //if (nonReserveTotal > addToReserve)
                    //{
                    //    newNonReserveTotal = nonReserveTotal - addToReserve;
                    //}
                    //else
                    //{
                    //    newNonReserveTotal = 0;
                    //    string message =
                    //        string.Format(
                    //           MIDText.GetTextOnly(eMIDTextCode.msg_al_BulkPacksApplyPackOptionPutAllInReserve),
                    //           _workUpBulkSizeBuy.HeaderID,
                    //           Name,
                    //           _workUpBulkSizeBuy.TotalUnitsAllocated);
                    //    _audit.Add_Msg(
                    //        eMIDMessageLevel.Information,
                    //        eMIDTextCode.msg_al_BulkPacksApplyPackOptionPutAllInReserve,
                    //        message,
                    //        GetType().Name,
                    //        false);
                    //}
                    //for (sizeCodeIdx = 0; sizeCodeIdx < _sizeCount; sizeCodeIdx++)
                    //{
                    //    // following statement gets next largest size by units and finds its position in the curve!
                    //    sizePosition = _sizeRIDCurvePosIdxXref[_sortWorkUpBuyDescendSizeUnits[sizeCodeIdx].Item];
                    //    oldSizeTotal =
                    //        ssv[sizePosition].AllStoreTotalValue
                    //        - ssv[sizePosition].GetStoreValue(_reserveStoreRID);
                    //    if (oldNonReserveTotal > 0)
                    //    {
                    //        newSizeTotal =
                    //            (int)(((double)(oldSizeTotal)
                    //             * (double)newNonReserveTotal
                    //             / (double)oldNonReserveTotal) + .5d);
                    //        if (newSizeTotal > newNonReserveTotal)
                    //        {
                    //            newSizeTotal = newNonReserveTotal;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        newSizeTotal = 0;
                    //    }
                    //    oldNonReserveTotal -= oldSizeTotal;
                    //    newNonReserveTotal -= newSizeTotal;
                    //    for (int j = 0; j < _nonReserveStoreRID.Length; j++)
                    //    {
                    //        sortStores[j].Item = _nonReserveStoreRID[j];
                    //        sortStores[j].SortKey = new double[2];
                    //        sortStores[j].SortKey[0] = ssv[sizePosition].GetStoreValue(_nonReserveStoreRID[j]);
                    //        sortStores[j].SortKey[1] = _appTran.GetRandomDouble();
                    //    }
                    //    Array.Sort(sortStores, _midSortDescendComparer);
                    //    int storeRID;
                    //    for (int j = 0; j < _nonReserveStoreRID.Length; j++)
                    //    {
                    //        storeRID = sortStores[j].Item;
                    //        oldStoreValue = ssv[sizePosition].GetStoreValue(storeRID);
                    //        if (oldSizeTotal > 0)
                    //        {
                    //            newStoreValue =
                    //                (int)(((double)oldStoreValue
                    //                        * (double)newSizeTotal
                    //                        / oldSizeTotal) + .5d);
                    //            if (newStoreValue > newSizeTotal)
                    //            {
                    //                newStoreValue = newSizeTotal;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            newStoreValue = 0;
                    //        }
                    //        newSizeTotal -= newStoreValue;
                    //        oldSizeTotal -= oldStoreValue;
                    //        ssv[sizePosition].SetStoreValue(storeRID, newStoreValue);
                    //        // put the difference to reserve
                    //        if (oldStoreValue > newStoreValue)
                    //        {
                    //            ssv[sizePosition].SetStoreValue(
                    //                _reserveStoreRID,
                    //                ssv[sizePosition].GetStoreValue(_reserveStoreRID) + oldStoreValue - newStoreValue);
                    //        }
                    //    }
                    //}
                    #endregion TT#689 deleted code
                    int addToReserve = _desiredReserveBulkUnits - reserveTotal;
                    int addToSizeReserve = 0;
                    int oldNonReserveTotal = nonReserveTotal;
                    if (nonReserveTotal < addToReserve)
                    {
                        string message =
                            string.Format(
                               MIDText.GetTextOnly(eMIDTextCode.msg_al_BulkPacksApplyPackOptionPutAllInReserve),
                               _workUpBulkSizeBuy.HeaderID,
                               Name,
                               _workUpBulkSizeBuy.TotalUnitsAllocated);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_BulkPacksApplyPackOptionPutAllInReserve,
                            message,
                            GetType().Name,
                            false);
                    }
                    int storeNonReserveTotal;
                    for (sizeCodeIdx = 0; sizeCodeIdx < _sizeCount; sizeCodeIdx++)
                    {
                        // following statement gets next largest size by units and finds its position in the curve!
                        sizePosition = _sizeRIDCurvePosIdxXref[_sortWorkUpBuyDescendSizeUnits[sizeCodeIdx].Item];
                        if (oldNonReserveTotal > 0)
                        {
                            storeNonReserveTotal =
                                (int)ssv[sizePosition].AllStoreTotalValue                                         // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                                - (int)ssv[sizePosition].GetStoreValue(_reserveStoreRID);         // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk  
                            addToSizeReserve =
                                (int)(((double)(storeNonReserveTotal
                                                * (double)addToReserve)
                                                / (double)oldNonReserveTotal + .5d));
                            if (addToSizeReserve  > storeNonReserveTotal)
                            {
                                addToSizeReserve  = storeNonReserveTotal;
                            }
                            if (addToSizeReserve > addToReserve)
                            {
                                addToSizeReserve = addToReserve;
                            }
                            oldNonReserveTotal -= storeNonReserveTotal;
                            addToReserve -= addToSizeReserve;
                        }
                        else
                        {
                            addToSizeReserve = 0;
                        }
                        for (int j = 0; j < _nonReserveStoreRID.Length; j++)
                        {
                            sortStores[j].Item = _nonReserveStoreRID[j];
                            sortStores[j].SortKey = new double[3];
                            sortStores[j].SortKey[0] = ssv[sizePosition].GetStoreValue(_nonReserveStoreRID[j]);
                            sortStores[j].SortKey[1] = storeTotalUnits.GetStoreValue(_nonReserveStoreRID[j]); 
                            sortStores[j].SortKey[2] = _appTran.GetRandomDouble();
                        }
                        int storeRID_0;
                        // put the difference to reserve
                        ssv[sizePosition].SetStoreValue(
                           _reserveStoreRID,
                           ssv[sizePosition].GetStoreValue(_reserveStoreRID) + addToSizeReserve);
                        int unitsToReserve;
                        if (sortStores.Length > 1)
                        {
                            int storeRID_1;
                            int store_0_SizeUnits;
                            while (addToSizeReserve > 0)
                            {
                                Array.Sort(sortStores, _midSortDescendComparer);
                                storeRID_0 = sortStores[0].Item;
                                storeRID_1 = sortStores[1].Item;
                                store_0_SizeUnits = (int)ssv[sizePosition].GetStoreValue(storeRID_0);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                unitsToReserve =
                                    store_0_SizeUnits
                                    - (int)ssv[sizePosition].GetStoreValue(storeRID_1)    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                                    + 1;
                                if (unitsToReserve > addToSizeReserve)
                                {
                                    unitsToReserve = addToSizeReserve;
                                }
                                if (unitsToReserve > store_0_SizeUnits)
                                {
                                    unitsToReserve = store_0_SizeUnits;
                                }
                                store_0_SizeUnits -= unitsToReserve;
                                ssv[sizePosition].SetStoreValue(storeRID_0, store_0_SizeUnits);
                                storeTotalUnits.SetStoreValue(storeRID_0, storeTotalUnits.GetStoreValue(storeRID_0) - unitsToReserve);
                                sortStores[0].SortKey[0] = store_0_SizeUnits;
                                sortStores[1].SortKey[1] -= unitsToReserve;
                                addToSizeReserve -= unitsToReserve;
                            }
                        }
                        else
                        {
                            storeRID_0 = sortStores[0].Item;
                            ssv[sizePosition].SetStoreValue(storeRID_0, ssv[sizePosition].GetStoreValue(storeRID_0) - addToSizeReserve);
                        }
                    }
                    // end TT#689 Pack Coverage is too small
                }
            }
            else
            {
                // no reserve store
                string message =
                    MIDText.GetTextOnly((int)eMethodType.BuildPacks) + " [" + Name + "] "
                    + MIDText.GetTextOnly((int)eMIDTextCode.lbl_HeaderID) + " [" + _workUpBulkSizeBuy.HeaderID + "] "
                    + MIDText.GetTextOnly(eMIDTextCode.msg_al_NoReserveStore);
                _audit.Add_Msg(
                    eMIDMessageLevel.Information,
                    eMIDTextCode.msg_al_NoReserveStore,
                    message,
                    GetType().Name,
                    false);
            }
            return ssv;
        }
        /// <summary>
        /// Calculates a Size Run based on a Bulk Size Buy and desired multiple
        /// </summary>
        /// <param name="aPackMultiple">Desired pack multiple</param>
        /// <param name="aBulkSizeBuy">Bulk Size Buy</param>
        /// <returns>Pack Size Run with desired multiple</returns>
        private SizeUnits[] GetPatternSizeRun(int aPackMultiple, SizeUnits[] aBulkSizeBuy)
        {
            SizeUnits[] candidate_1 = new SizeUnits[aBulkSizeBuy.Length];
            SizeUnits[] candidate_2 = new SizeUnits[aBulkSizeBuy.Length];
            int pack_1_Mult = 0;
            int pack_2_Mult = 0;
            int bulkBuy = 0;
            for (int i = 0; i < aBulkSizeBuy.Length; i++)
            {
                bulkBuy += aBulkSizeBuy[i].Units;
            }
            for (int i = 0; i < aBulkSizeBuy.Length; i++)
            {
                if (bulkBuy == 0)
                {
                    candidate_1[i] = new SizeUnits(aBulkSizeBuy[i].RID, 0);
                    candidate_2[i] = new SizeUnits(aBulkSizeBuy[i].RID, 0);
                }
                else
                {
                    candidate_1[i] =
                        new SizeUnits(
                            aBulkSizeBuy[i].RID,
                            (int)Math.Round((
                                  (double)(aBulkSizeBuy[i].Units * aPackMultiple) 
                                  / (double) bulkBuy) + .5d, 0)
                                     );
                    pack_1_Mult += candidate_1[i].Units;
                    candidate_2[i] =
                        new SizeUnits(
                            aBulkSizeBuy[i].RID,
                            (int)Math.Round((
                                  (double)(aBulkSizeBuy[i].Units * aPackMultiple)
                                  / (double)bulkBuy), 0)
                                     );
                    pack_2_Mult += candidate_2[i].Units;
                }
            }
            if (pack_1_Mult == aPackMultiple)
            {
                return candidate_1;
            }
            if (pack_2_Mult == aPackMultiple)
            {
                return candidate_2;
            }
            if (pack_2_Mult > aPackMultiple)
            {
                return ReduceSizeRun(candidate_2, aPackMultiple, aBulkSizeBuy);
            }
            if ((pack_1_Mult - aPackMultiple)
                > (aPackMultiple - pack_2_Mult))
            {
                return IncreaseSizeRun(candidate_2, aPackMultiple, aBulkSizeBuy);
            }
            if (pack_1_Mult > aPackMultiple)
            {
                return ReduceSizeRun(candidate_1, aPackMultiple, aBulkSizeBuy);
            }
            return IncreaseSizeRun(candidate_1, aPackMultiple, aBulkSizeBuy);
        }
        /// <summary>
        /// Modifies a pack size run by reducing up to 2 low-end sizes by 1 unit and correspondingly increasing 2 high end sizes
        /// </summary>
        /// <param name="aPackSizeRun">Pack Size Run</param>
        /// <param name="aBulkSizeBuy">Bulk Size Buy</param>
        /// <returns>Modified Pack Size Run</returns>
        private SizeUnits[] ModifyPatternSizeRun(SizeUnits[] aPackSizeRun, SizeUnits[] aBulkSizeBuy)
        {
            if (aPackSizeRun.Length != aBulkSizeBuy.Length)
            {
                throw new ArgumentException("ModifyPatternSizeRun: PackSizeRun Length must equal BulkSizeBuy length");
            }
            MIDGenericSortItem[] sortedSizes = new MIDGenericSortItem[aBulkSizeBuy.Length];
            SizeUnits[] candidatePack = new SizeUnits[aPackSizeRun.Length];
            for (int i=0; i<aPackSizeRun.Length; i++)
            {
                if (aPackSizeRun[i].RID != aBulkSizeBuy[i].RID)
                {
                    throw new ArgumentException("ModifyPatternSizeRun: PackSizeRun size RIDs must match BulkSizeBuy size RIDs");
                }
                candidatePack[i] = new SizeUnits(aPackSizeRun[i].RID, aPackSizeRun[i].Units);
                sortedSizes[i].Item = i;
                sortedSizes[i].SortKey = new double[2];
                sortedSizes[i].SortKey[0] = -aBulkSizeBuy[i].Units;   // sort in ascending sequence
                sortedSizes[i].SortKey[1] = _appTran.GetRandomDouble();
            }
            Array.Sort(sortedSizes, _midSortDescendComparer);
            int sizeIdx;
            int sizeModifyCount = 0;
            for (int i = 0; i < aPackSizeRun.Length; i++)
            {
                sizeIdx = sortedSizes[i].Item;
                if (candidatePack[sizeIdx].Units > 0)
                {
                    candidatePack[sizeIdx] = new SizeUnits(candidatePack[sizeIdx].RID, candidatePack[sizeIdx].Units - 1);
                    sizeIdx = sortedSizes[aPackSizeRun.Length - 1 - sizeModifyCount].Item;
                    candidatePack[sizeIdx] = new SizeUnits(candidatePack[sizeIdx].RID, candidatePack[sizeIdx].Units + 1);
                    sizeModifyCount++;
                }
                if (sizeModifyCount == 2)
                {
                    break;
                }
            }
            return candidatePack;
        }
        /// <summary>
        /// Modifies a Size Run whose multiple is smaller than the desired multiple
        /// </summary>
        /// <param name="aPackSizeRun">Size Run</param>
        /// <param name="aMultiple">Desired Multiple</param>
        /// <param name="aBulkSizeBuy">Bulk Size Buy</param>
        /// <returns>Modified Size Run</returns>
        private SizeUnits[] IncreaseSizeRun(SizeUnits[] aPackSizeRun, int aMultiple, SizeUnits[] aBulkSizeBuy)
        {
            if (aPackSizeRun.Length != aBulkSizeBuy.Length)
            {
                throw new ArgumentException("IncreaseSizeRun: PackSizeRun Length must equal BulkSizeBuy length");
            }
            MIDGenericSortItem[] bulkSizesDescending = new MIDGenericSortItem[aBulkSizeBuy.Length];
            int sizeRID;
            int sizeIdx;
            int packSizeTotal = 0;
            SizeUnits[] packSizeRun = aPackSizeRun;
            for (int i = 0; i < aBulkSizeBuy.Length; i++)
            {
                bulkSizesDescending[i].Item = i;
                bulkSizesDescending[i].SortKey = new double[2];
                bulkSizesDescending[i].SortKey[0] = aBulkSizeBuy[i].Units;
                bulkSizesDescending[i].SortKey[1] = _appTran.GetRandomDouble();
                if (aPackSizeRun[i].RID != aBulkSizeBuy[i].RID)
                {
                    throw new ArgumentException("IncreaseSizeRun: PackSizeRun size RIDs must match BulkSizeBuy size RIDs");
                }
                packSizeTotal += aPackSizeRun[i].Units;
            }
            Array.Sort(bulkSizesDescending, _midSortDescendComparer);

            int sortIdx = 0;
            while (packSizeTotal < aMultiple)
            {
                sizeIdx = bulkSizesDescending[sortIdx].Item;
                sizeRID = aBulkSizeBuy[sizeIdx].RID;
                packSizeRun[sizeIdx] = new SizeUnits(sizeRID, packSizeRun[sizeIdx].Units + 1);
                packSizeTotal++;
                if (sortIdx < _sizeCount)
                {
                    sortIdx++;
                }
                else
                {
                    sortIdx = 0;
                }
            }
            return packSizeRun;
        }
        /// <summary>
        /// Modifies a size run whose multiple is larger than the desired multiple
        /// </summary>
        /// <param name="aPackSizeRun">Packs' Size Runs</param>
        /// <param name="aMultiple">Desired Multiple</param>
        /// <param name="aBulkSizeBuy">Bulk Size Buy</param>
        /// <returns>Modified Size Run</returns>
        private SizeUnits[] ReduceSizeRun(SizeUnits[] aPackSizeRun, int aMultiple, SizeUnits[] aBulkSizeBuy)
        {
            if (aPackSizeRun.Length != aBulkSizeBuy.Length)
            {
                throw new ArgumentException("ReduceSizeRun: PackSizeRun Length must equal BulkSizeBuy length");
            }
            bool allUnitsEq1 = true;
            int packMultiple = 0;
            for (int i=0; i < aPackSizeRun.Length; i++)
            {
                if (aPackSizeRun[i].RID != aBulkSizeBuy[i].RID)
                {
                    throw new ArgumentException("ReduceSizeRun: PackSizeRun size RIDs must match BulkSizeBuy size RIDs");
                }
                packMultiple += aPackSizeRun[i].Units;
                if (aPackSizeRun[i].Units > 1)
                {
                    allUnitsEq1 = false;
                }
            }

            bool firstPass = true;
            bool forward = true;
            int takeFromSize;
            double minRatio = double.MaxValue;
            //int minSizeAloctn; // TT#660 - JEllis - Negative Value in Packs
            int minSizeAloctn = int.MaxValue; // TT#660 - JEllis - Negative Value in Packs
            SizeUnits[] su = aPackSizeRun;
            while (packMultiple > aMultiple)
            {
                if (forward)
                {
                    takeFromSize = 0;
                    for (int i = 0; i < aPackSizeRun.Length; i++)
                    {
                        if (firstPass)
                        {
                            if (allUnitsEq1)
                            {
                                //minSizeAloctn = int.MaxValue;  // TT#660 - JEllis - Negative Value in Packs
                                if (su[i].Units == 1)
                                {
                                    if (aBulkSizeBuy[i].Units < minSizeAloctn)
                                    {
                                        minSizeAloctn = aBulkSizeBuy[i].Units; // TT#660 - JEllis - Negative Value in Packs
                                        takeFromSize = i;
                                    }
                                }
                            }
                            else
                            {
                                if (su[i].Units > 1)
                                {
                                    int sizeRatio = aBulkSizeBuy[i].Units / (su[i].Units - 1);
                                    if (sizeRatio < minRatio)
                                    {
                                        minRatio = sizeRatio;  // TT#660 - JEllis - Negative Value in Packs
                                        takeFromSize = i;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (su[i].Units > su[takeFromSize].Units)
                            {
                                takeFromSize = i;
                            }
                        }
                    }
                }
                else
                {
                    takeFromSize = aPackSizeRun.Length - 1;
                    //for (int i = aPackSizeRun.Length; i < -1; i++)  // TT#660 - JEllis - Negative Value in Packs
                    for (int i = aPackSizeRun.Length - 1; i > -1; i--)      // TT#660 - JEllis - Negative Value in Packs
                    {
                        if (su[i].Units > su[takeFromSize].Units)
                        {
                            takeFromSize = i;
                        }
                    }
                }
                su[takeFromSize] = new SizeUnits(su[takeFromSize].RID, su[takeFromSize].Units - 1);
                packMultiple--;
                if (forward)
                {
                    forward = false;
                }
                else
                {
                    forward = true;
                }
                firstPass = false;
            }
            return su;
        }
        // begin TT#580 build packs creates duplicate solutions
        public OptionPackProfileList EliminateDuplicateSolutions(List<OptionPackProfile> aOptionPackProfileList)
        {
            OptionPackProfileList oppL = new OptionPackProfileList();
            OptionPackProfile oppI;
            OptionPackProfile oppJ;
            bool optionsEqual;
            oppL.Add(aOptionPackProfileList[0]);
            for (int i=1; i<aOptionPackProfileList.Count; i++)
            {
                oppI = aOptionPackProfileList[i];
                optionsEqual = false;
                for (int j = 0; j < oppL.Count; j++)
                {
                    oppJ = (OptionPackProfile)oppL.ArrayList[j];
                    if (oppJ.IsOptionPackSolutionEqual(oppI))
                    {
                        optionsEqual = true;
                        string message =
                            string.Format(
                                MIDText.GetTextOnly(eMIDTextCode.msg_al_DupBuildPacksOptionRemoved),
                                Name,
                                _workUpBulkSizeBuy.HeaderID,
                                oppI.OptionPackID,
                                oppJ.OptionPackID);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_DupBuildPacksOptionRemoved,
                            message,
                            GetType().Name,
                            false);
                        break;
                    }
                }
                if (!optionsEqual)
                {
                    oppL.Add(oppI);
                }
            }
            return oppL;
        }
        // end TT#580 build packs creates duplicate solutions
        public OptionPackProfile GetOptionPackProfile(int aOptionPackProfileRID)
        {
            if (_oPPL == null
                || _oPPL.Count == 0)
            {
                return null;
            }
            return _oPPL.FindKey(aOptionPackProfileRID) as OptionPackProfile;
        }
        /// <summary>
        /// Apply the selected option as a Work Up Pack Buy (overlaying the input Work Up Bulk Size Buy
        /// </summary>
        /// <param name="aOptionPackProfileID">Option Pack Profile to save</param>
        /// <param name="aStatusReason">Status of the save</param>
        /// <returns>True: Save successful; False: Save Failed in which case aStatusReason contains a reason for the failure.</returns>
        public bool ApplySelectedOptionPackProfile(int aOptionPackProfileID, out MIDException aStatusReason)
        {
            bool success = true;
            OptionPackProfile opp = null;
            aStatusReason = null;
            if (_oPPL == null
                || _oPPL.Count == 0)
            {
                aStatusReason = new MIDException(
                    eErrorLevel.warning,
                    (int)eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,
                    string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed), _workUpBulkSizeBuy.HeaderID, Name)
                    + "Option Packs have not been created.");
                success = false;
            }
            else if (_workUpBulkSizeBuy == null)
            {
                aStatusReason = new MIDException(
                    eErrorLevel.warning,
                    (int)eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,
                    string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed), _workUpBulkSizeBuy.HeaderID, Name)
                    + "No header selected.");
                success = false;
            }
            else if (_workUpBulkSizeBuy.PackCount !=0)
            {
                aStatusReason = new MIDException(
                    eErrorLevel.warning,
                    (int)eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,
                    string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed), _workUpBulkSizeBuy.HeaderID, Name)
                    + string.Format("Option packs already applied to header; header contains {0} packs.", _workUpBulkSizeBuy.PackCount.ToString(CultureInfo.CurrentUICulture)));
                success = false;
            }
            else
            {
                opp =_oPPL.FindKey(aOptionPackProfileID) as OptionPackProfile;
                if (opp == null)
                {
                    aStatusReason = new MIDException(
                        eErrorLevel.warning,
                        (int)eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,
                        string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed), _workUpBulkSizeBuy.HeaderID, Name)
                        + "Build Pack Option ID [" + aOptionPackProfileID.ToString(CultureInfo.CurrentCulture) + "] not found");
                    success = false;
                }
            }
            if (success)
            {
                Header header = _workUpBulkSizeBuy.HeaderDataRecord;
                header.OpenUpdateConnection();
                try
                {
                    
                    //_workUpBulkSizeBuy.Archive(header); //TT#739-MD -jsobek -Delete Stores -Remove Archive
                    foreach (Index_RID storeIdxRID in _storeIdxRIDArray)
                    {
                        SizeUnits[] bulkSizeBuy = opp.GetStoreBulkSizeBuy(storeIdxRID.RID);
                        foreach (SizeUnits su in bulkSizeBuy)
                        {
                            _workUpBulkSizeBuy.SetStoreQtyAllocated(_workUpColorBin, su.RID, storeIdxRID, su.Units, eDistributeChange.ToAll, false);
                        }
                    }
                    StorePackVector[] storePackBuy = opp.StorePackBuy;
                    foreach (StorePackVector spv in storePackBuy)
                    {
                        string packName = spv.PackID.ToString(CultureInfo.CurrentUICulture);
                        _workUpBulkSizeBuy.AddPack(packName, eAllocationType.DetailType, spv.PackMultiple, (int)spv.AllStoreTotalValue, spv.PackID);    // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        PackHdr ph = _workUpBulkSizeBuy.GetPackHdr(packName);
                        //_workUpBulkSizeBuy.TotalUnitsToAllocate += spv.AllStorePackTotalUnits;
                        _workUpBulkSizeBuy.AddColorToPack(packName, _workUpColorBin.ColorCodeRID, spv.PackMultiple, 0);
                        List<SizeUnits> packSizes = spv.PackSizeUnits;
                        int seq = 0;
                        foreach (SizeUnits su in packSizes)
                        {
                            _workUpBulkSizeBuy.AddSizeToPackColor(packName, _workUpColorBin.ColorCodeRID, su.RID, su.Units, seq);
                            seq++;
                        }
                        
                        foreach (Index_RID storeIdxRID in _storeIdxRIDArray)
                        {
                            _workUpBulkSizeBuy.SetStoreQtyAllocated(ph, storeIdxRID, (int)spv.GetStoreValue(storeIdxRID.RID), eDistributeChange.ToAll, false);     // TT#744 - JEllis - Use Orig Pack Fit Logic; Remove Bulk
                        }
                    }
                    // begin TT#651 BP After Apply button processed, WUB status is Sizes Out of Balance
                    List<HdrColorBin> colorToRemove = new List<HdrColorBin>();
                    foreach (HdrColorBin hcb in _workUpBulkSizeBuy.BulkColors.Values)
                    {
                        if (hcb.ColorUnitsAllocated == 0)
                        {
                            colorToRemove.Add(hcb);
                        }
                    }
                    foreach (HdrColorBin colorBin in colorToRemove)
                    {
                        _workUpBulkSizeBuy.RemoveBulkColor(colorBin);
                    }
                    // end TT#651 BP After Apply button processed, WUB status is Sizes Out of Balance
                    if (_workUpBulkSizeBuy.WriteHeaderData(header))
                    {
                        header.CommitData();
                        success = true;
                        _audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_BuildPacksApplyPackOptionSuccess,
                            string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionSuccess), _workUpBulkSizeBuy.HeaderID, Name), GetType().Name, true);
                    }
                    else
                    {
                        success = false;
                        string failMsg =
                            string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed, _workUpBulkSizeBuy.HeaderID, Name));
                        _audit.Add_Msg(
                             eMIDMessageLevel.Information,
                             eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,
                            failMsg, GetType().Name, true);
                        aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_al_BuildPacksApplyPackOptionFailed,failMsg);
                    }
                }
                catch (MIDException e)
                {
                    success = false;
                    aStatusReason = e;
                }
                catch (Exception e)
                {
                    success = false;
                    aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                }
                finally
                {
                    header.CloseUpdateConnection();
                    if (!success)
                    {
                        _workUpBulkSizeBuy.ReReadHeader();
                        // begin TT#1488 - MD - JEllis - Group Allocation (unrelated: Use common translator in Include)
                        //// begin TT#744 - JEllis - Use Orig Pack Fitting logic; Remove Bulk
                        ////_audit.Log_MIDException(_statusReason, GetType().Name, eExceptionLogging.logAllInnerExceptions);
                        if (_statusReason != null)     // TT#1185 - Verify ENQ before Update   
                        {                              // TT#1185 - Verify ENQ before Update   
                              //_audit.Add_Msg(ConvertErrorLevelToMessageLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                              _audit.Add_Msg(Include.TranslateErrorLevel(_statusReason.ErrorLevel), (eMIDTextCode)_statusReason.ErrorNumber, _statusReason.ErrorMessage, GetType().Name);
                        }                             // TT#1185 - Verify ENQ before Update
                        //// end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
						// end TT#488 - MD - JEllis - Group Allocation (unrelted: Use Common Translator in Include)
                    }
                }
            }
            return success;
        }
        public override bool WithinTolerance(double aTolerancePercent)
        {
            return true;
        }
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
        public override bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            return true;
        }

        public override ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, 
            bool aCloneCustomOverrideModels)
        {
            BuildPacksMethod newBuildPacksMethod = null;
            try
            {
                newBuildPacksMethod = (BuildPacksMethod)this.MemberwiseClone();
                newBuildPacksMethod.IsInteractive = IsInteractive;
                newBuildPacksMethod.Method_Change_Type = eChangeType.none;
                newBuildPacksMethod.Method_Description = Method_Description;
                newBuildPacksMethod.MethodStatus = MethodStatus;
                newBuildPacksMethod.Name = Name;
                return newBuildPacksMethod;
            }
            catch 
            {
                throw;
            }
        }

        public override void Update(TransactionData td)
        {
            if (_bpmData == null)
            {
                throw new NullReferenceException("Must instantiate Build Packs Method before saving it");
            }
            if (_bpmData.CombinationList.Count == 0
                && base.Method_Change_Type != eChangeType.delete)
            {
                throw new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern, MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern));
            }
            try
            {
                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _bpmData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _bpmData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        //_bpmData.DeleteMethod(base.Key, td);    // delete handled in Stored Procedure
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
            }
        }
        #endregion Methods
    }
}
