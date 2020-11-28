using System;
using System.Data;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
    /// <summary>
    /// Inherits MethodBase containing all properties for a Method.
    /// Adds properties for BuildPacks
    /// </summary>
    public class BuildPacksMethodData : MethodBaseData
    {
        private    int _bPC_RID;
        private string _bPC_Name;
        private    int _bPC_Pack_Min; // TT#787 Vendor Min Order applies only to packs
        private    int _bPC_Size_Multiple;
        private    int _pack_Min;     // TT#787 Vendor Min Order applies only to packs
        private    int _size_Multiple;
        private    int _sizeGroupRID;
        private    int _sizeCurveGroupRID;
        private double _reserveTotalQty;
        private   bool _reserveTotalIsPercent;
        private double _reserveBulkQty;
        private   bool _reserveBulkIsPercent;
        private double _reservePackQty;
        private   bool _reservePackIsPercent;
        private double _avg_Pack_Dev_Tolerance; //
        private    int _max_Pack_Need_Tolerance;  // Correction
        private   bool _depleteReserveSelected;  // TT#669 Build Packs Variance Enhancement
        private   bool _increaseBuySelected;     // TT#669 Build Packs Variance Enhancement
        private double _increaseBuyPct;          // TT#669 Build Packs Variance Enhancement
        private   bool _removeBulkInd;           // TT#744 - JEllis - Use Orig Pack Fit Logic; Option No Bulk                 
        private List<PackPatternCombo> _packCombination;
        private List<int> _deletePackComboRIDList; // TT#607 BP Delete Combo not working
        private List<int> _deletePackPatternRIDList; // TT#749 - SQL "error near ')' when updt or process BP" - JEllis
        private Dictionary<int, PackPatternCombo> _vendorComboDictionary;
        private Dictionary<int, PackPatternCombo> _buildPacksComboDictionary;
        private Dictionary<int, PackPattern> _buildPacksPatternDictionary; // TT#749 - SQL "error near ')' when updt or process BP" - JEllis
        /// <summary>
		/// Creates an instance of the BuildPacksMethodData class.
		/// </summary>
        public BuildPacksMethodData()
		{
            _bPC_RID = Include.NoRID;
            _bPC_Name = String.Empty;
            _bPC_Pack_Min = 0;       // TT#787 Vendor Min Order applies only to packs
            _bPC_Size_Multiple = 1;
            _pack_Min = 0;           // TT#787 Vendor Min Order applies only to packs
            _size_Multiple = 1;
            _sizeGroupRID = Include.NoRID; 
            _sizeCurveGroupRID = Include.NoRID;
            _reserveTotalQty = 0;
            _reserveTotalIsPercent = false;
            _reserveBulkQty = 0;
            _reserveBulkIsPercent = false;
            _reservePackQty = 0;
            _reservePackIsPercent = false;
            _avg_Pack_Dev_Tolerance = Include.DefaultPackSizeErrorPercent;
            _max_Pack_Need_Tolerance = Include.DefaultBldPacksMaxPackNeedTol;  // Correction
            _packCombination = new List<PackPatternCombo>();
            _vendorComboDictionary = new Dictionary<int, PackPatternCombo>();
            _buildPacksComboDictionary = new Dictionary<int, PackPatternCombo>();
            _buildPacksPatternDictionary = new Dictionary<int, PackPattern>(); // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            _deletePackComboRIDList = new List<int>();  // TT#607 BP delete combo not working
            _deletePackPatternRIDList = new List<int>(); // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            _depleteReserveSelected = true; // TT#669 Build Packs Ehancement
            _increaseBuySelected = false;   // TT#669 Build Packs Enhancement
            _increaseBuyPct = double.MaxValue; // TT#669 Build Packs Enhancement
            _removeBulkInd = false; // TT#744 - JEllis - Use Orig Pack Fit Logic; Option No Bulk
		}

        /// <summary>
        /// Gets the BPC_Name (Vendor Name)
        /// </summary>
        public string BPC_Name
        {
            get { return _bPC_Name; }
        }
        /// <summary>
        /// Gets the Pack Minimum Order: if the BPC_Name is present, this value is the BPC_Pack_Min; otherwise, it is a value specified by the user for this method. // TT#787 Vendor Min Order applies only to packs
        /// </summary>
        public int Pack_MIN // TT#787 Vendor Min Order applies only to packs
        {
            get 
            {
                if (_bPC_RID == Include.NoRID)
                {
                    return _pack_Min; // TT#787 Vendor Min Order applies only to packs
                }
                return _bPC_Pack_Min; // TT#787 Vendor Min Order applies only to packs
            }
        }
        /// <summary>
        /// Gets the size multiple for the Bulk order: if the BPC_Name is present, this value is the BPC_Size_Mult; otherwise, it is a value specified by the user for this method.
        /// </summary>
        public int SizeMultiple
        {
            get
            {
                if (_bPC_RID == Include.NoRID)
                {
                    return _size_Multiple;
                }
                return _bPC_Size_Multiple;
            }
        }
        /// <summary>
        /// Gets the SizeGroupRID associated with this method: Include.NoRID is returned if there is no associated SizeGroupRID
        /// </summary>
        public int SizeGroupRID
        {
            get { return _sizeGroupRID; }
        }
        /// <summary>
        /// Gets the SizeCurveGroupRID associated with this method:  Include.NoRID is returned if there is no associated SizeCurveGroupRID
        /// </summary>
        public int SizeCurveGroupRID
        {
            get { return _sizeCurveGroupRID; }
        }
        /// <summary>
        /// Gets the ReserveTotal specified for this method. Quantity returned is a percent when ReserveTotalIsPercent is true; it is units otherwise.
        /// </summary>
        public double ReserveTotal
        {
            get { return _reserveTotalQty; }
        }
        /// <summary>
        /// Gets or sets the ReserveTotalIsPercent indicator.  True:  indicates ReserveTotal is a percent; False: indicates ReserveTotal is units.
        /// </summary>
        public bool ReserveTotalIsPercent
        {
            get { return _reserveTotalIsPercent; }
            set { _reserveTotalIsPercent = value; }
        }
        /// <summary>
        /// Gets the ReserveBulk specified for this method.  Quantity returned is a percent when ReserveBulkIsPercent is true; it is units otherwise.
        /// </summary>
        public double ReserveBulk
        {
            get { return _reserveBulkQty; }
        }
        /// <summary>
        /// Gets or sets the ReserveBulkIsPercent indicator.  True:  indicates ReserveBulk is a percent; False: indicates ReserveBulk is units.
        /// </summary>
        public bool ReserveBulkIsPercent
        {
            get { return _reserveBulkIsPercent; }
            set { _reserveBulkIsPercent = value; }
        }
        /// <summary>
        /// Gets ReservePacks specified for this method.  Quantity returned is a percent when ReservePackIsPercent is true; it is units otherwise.
        /// </summary>
        public double ReservePacks
        {
            get { return _reservePackQty; }
        }
        /// <summary>
        /// Gets the ReservePacksIsPercent indicator.  True:  indicates ReservePacks is a percent; False: indicates ReservePacks is units.
        /// </summary>
        public bool ReservePacksIsPercent
        {
            get { return _reservePackIsPercent; }
            set { _reservePackIsPercent = value; }
        }
        /// <summary>
        /// Gets or sets the AvgPackErrorDevTolerance.  This is the Average Size Error a pack may introduce when fitting a pack to a store's size allocation.
        /// </summary>
        public double AvgPackErrorDevTolerance             // correction 
        {
            get { return _avg_Pack_Dev_Tolerance; }        // correction
            set { _avg_Pack_Dev_Tolerance = value; }       // correction
        }
        /// <summary>
        /// Gets or sets the MaxPackErrorDevTolerance (aka Ship Variance):  This is the maximum size error that a pack may introduce when fitting a pack to a store's size allocation.
        /// </summary>
        public uint MaxPackErrorDevTolerance
        {
            get { return Convert.ToUInt32(_max_Pack_Need_Tolerance); }  // Correction
            set { _max_Pack_Need_Tolerance = (int)value; }              // Correction 2
        }
        public int CombinationCount
        {
            get { return CombinationList.Count; }
        }

        // begin TT#669 BUild Packs Variance Enhancement
        /// <summary>
        /// Gets or sets whether the Reserve is depleted when fitting a pack to a store that introduces a variance from the store's plan
        /// </summary>
        public bool DepleteReserveSelected
        {
            get { return _depleteReserveSelected; }
            set { _depleteReserveSelected = value; }
        }
        /// <summary>
        /// Gets or sets whether the buy can be increased in order to fit a pack to a store that introduces a variance from the store's plan
        /// </summary>
        public bool IncreaseBuySelected
        {
            get { return _increaseBuySelected; }
            set { _increaseBuySelected = value; }
        }
        /// <summary>
        /// Gets the percentage that a buy may increase when fitting packs to the stores.
        /// </summary>
        public double IncreaseBuyPct
        {
            get { return _increaseBuyPct; }
        }
        // end TT#669 Build Packs Variance enhancement

        // Begin TT#744 - JEllis - Use Orig Pack Fit Logic; Option No Bulk
        /// <summary>
        /// Gets or sets whether to remove or keep the bulk order after fitting packs 
        /// </summary>
        public bool RemoveBulkAfterFittingPacks
        {
            get { return _removeBulkInd; }
            set { _removeBulkInd = value; }
        }
        // End TT#744 - JEllis - Use Orig Pack Fit Logic; Option No Bulk
        /// <summary>
        /// Gets a copy of the list of pack pattern combinations associated with this BuildPacksMethod.
        /// </summary>
        public List<PackPatternCombo> CombinationList
        {
            get
            {
                List<PackPatternCombo> combinationList = new List<PackPatternCombo>();
                foreach (PackPatternCombo ppc in _packCombination)
                {
                    combinationList.Add((PackPatternCombo)ppc.Clone());
                }
                return combinationList;
            }
        }

        /// <summary>
        /// Sets the BPC_Name (aka Vendor Name).  Setting this name will cause BPC_RID, Pack_MIN, and Size_Multiple to be set to the appropriate BPC (Vendor) values as well as add/replace any BPC (Vendor) Pack Combinations (Combinations specific to this method will remain in the list).  // TT#787 Vendor Min Order applies only to packs
        /// </summary>
        /// <param name="aBPC_Name">BPC Name (Vendor Name) to retrieve.</param>
        /// <param name="aStatusReason">If set "fails", this message will give a reason for the failure; message is empty when set is successful.</param>
        /// <returns>True: set was successful; False: set failed (aStatus Msg contains a "reason" for the failure in this case.</returns>
        public bool Set_BPC_Name(string aBPC_Name, out MIDException aStatusReason)
        {
            try
            {
                if (aBPC_Name == null
                    || aBPC_Name == String.Empty)
                {
                    List<PackPatternCombo> packCombination = new List<PackPatternCombo>();
                    foreach (PackPatternCombo ppc in _packCombination)
                    {
                        if (ppc.PackPatternType == ePackPatternType.BuildPacksMethod)
                        {
                            packCombination.Add(ppc);
                        }
                    }
                    if (packCombination.Count < 1)
                    {
                        aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_RemovingVendorCausesInvalidPackCombo, MIDText.GetTextOnly(eMIDTextCode.msg_al_RemovingVendorCausesInvalidPackCombo));
                        return false;
                    }
                    _bPC_RID = Include.NoRID;
                    _bPC_Pack_Min = 0; // TT#787 Vendor Min Order applies only to packs
                    _bPC_Name = null;
                    _bPC_Size_Multiple = 1;

                    _packCombination = packCombination;
                    _vendorComboDictionary = new Dictionary<int, PackPatternCombo>();
                    aStatusReason = null;
                    return true;
                }
                return
                    //Set_BPC_Data(
                    //    Get_BPC_Table_SQL_Select().Append(" where bpc.BPC_NAME = '").Append(aBPC_Name.ToString(CultureInfo.CurrentUICulture).Replace("'", "''")).Append("' "),   // TT#670 Error when vendor name contains apostrophe
                    //    out aStatusReason);
                    Set_BPC_Data(out aStatusReason,
                           aBPC_Name //aBPC_Name.Replace("'", "''")).Append("' "),   // TT#670 Error when vendor name contains apostrophe
                           );
            }
            catch (MIDException e)
            {
                aStatusReason = e;
                return false;
            }
            catch (Exception e)
            {
                aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                return false;
            }
        }
        /// <summary>
        /// Sets the Pack Minimum Order for this BuildPacksMethod. // TT#787 Vendor Min Order applies only to packs
        /// </summary>
        /// <param name="aPackMinOrder">Desired pack minimum order.</param> // TT#787 Vendor Min Order applies only to packs
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetPackMin(int aPackMinOrder, out MIDException aStatusReason) // TT#787 Vendor Min Order applies only to packs
        {
            if (_bPC_RID != Include.NoRID)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_CannotModifyPackMinWhenVendor, MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotModifyPackMinWhenVendor)); // TT#787 Vendor Min Order applies only to packs
                return false;
            }
            if (aPackMinOrder < 0) // TT#787 Vendor Min Order applies only to packs
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_PackMinOrderMustBeNonNegative, MIDText.GetTextOnly(eMIDTextCode.msg_al_PackMinOrderMustBeNonNegative)); // TT#787 Vendor Min Order applies only to packs
                return false;
            }
            _pack_Min = aPackMinOrder; // TT#787 Vendor Min Order applies only to packs // TT#787 Vendor Min Order applies only to packs
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the bulk Size Multiple for this BuildPacksMethod. This ia the multiple in which each bulk size must be ordered.
        /// </summary>
        /// <param name="aSizeMultipler">Desired size multiple.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetSizeMultiple(int aSizeMultiple, out MIDException aStatusReason)
        {
            if (_bPC_RID != Include.NoRID)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_CannotModifySzMultWhenVendor, MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotModifySzMultWhenVendor));
                return false;
            }
            if (aSizeMultiple < 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_SzMultMustBeGT_0, MIDText.GetTextOnly(eMIDTextCode.msg_al_SzMultMustBeGT_0));
                return false;
            }
            _size_Multiple = aSizeMultiple;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the Size Group RID for this BuildPacksMethod. Size Group is mutually exclusive with SizeCurveGroup.
        /// </summary>
        /// <param name="aSizeGroupRID">RID of the desired Size Group.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetSizeGroupRID(int aSizeGroupRID, out MIDException aStatusReason)
        {
            if (_sizeCurveGroupRID != Include.NoRID
                && aSizeGroupRID != Include.NoRID)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_SizeGroupAndCurveGroupMutuallyExcl, MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeGroupAndCurveGroupMutuallyExcl));
                return false;
            }
            _sizeGroupRID = aSizeGroupRID;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the Size Curve Group RID for this BuildPacksMethod. Size Group is mutually exclusive with SizeCurveGroup.
        /// </summary>
        /// <param name="aSizeCurveGroupRID">RID of the desired Size Curve Group.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetSizeCurveGroupRID(int aSizeCurveGroupRID, out MIDException aStatusReason)
        {
            if (_sizeGroupRID != Include.NoRID
                && aSizeCurveGroupRID != Include.NoRID)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_SizeGroupAndCurveGroupMutuallyExcl, MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeGroupAndCurveGroupMutuallyExcl));
                return false;
            }

            _sizeCurveGroupRID = aSizeCurveGroupRID;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the reserve total quantity.
        /// </summary>
        /// <param name="aReserveTotal">Desired quantity to put in reserve.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetReserveTotal(double aReserveTotal, out MIDException aStatusReason)
        {
            if (aReserveTotal < 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative, MIDText.GetTextOnly(eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative));
                return false;
            }
            _reserveTotalQty = aReserveTotal;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the bulk reserve quantity.
        /// </summary>
        /// <param name="aReserveBulk">Desired bulk quantity to put in reserve.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetReserveBulk(double aReserveBulk, out MIDException aStatusReason)
        {
            if (aReserveBulk < 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative, MIDText.GetTextOnly(eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative));
                return false;
            }
            _reserveBulkQty = aReserveBulk;
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Sets the pack reserve quantity.
        /// </summary>
        /// <param name="aReserveBulk">Desired pack quantity to put in reserve.</param>
        /// <param name="aStatusReason">The reason the set fails; when the set is successful, the status reason is null.</param>
        /// <returns>True: set was successful, aStatusReason is null in this case; False: set failed in which case aStatusReason gives a reason for the failure.</returns>
        public bool SetReservePacks(double aReservePacks, out MIDException aStatusReason)
        {
            if (aReservePacks < 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative, MIDText.GetTextOnly(eMIDTextCode.msg_al_ReserveQtyMustBeNonNegative));
                return false;
            }
            _reservePackQty = aReservePacks;
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
            aStatusReason = null;
            PackPatternCombo ppc;
            if (aPackPatternType == ePackPatternType.Vendor)
            {
                if (!_vendorComboDictionary.TryGetValue(aPatternComboRID, out ppc))
                {
                    aStatusReason =
                        new MIDException(
                            eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_ComboDoesNotExist,
                            string.Format(
                               MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboDoesNotExist),
                               Enum.GetName(aPackPatternType.GetType(), aPackPatternType),
                               aPatternComboRID));
                    return false;
                }
            }
            else
            {
                if (!_buildPacksComboDictionary.TryGetValue(aPatternComboRID, out ppc))
                {
                    aStatusReason =
                        new MIDException(
                            eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_ComboDoesNotExist,
                            string.Format(
                                MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboDoesNotExist),
                                Enum.GetName(aPackPatternType.GetType(), aPackPatternType),
                                aPatternComboRID));
                }
            }
            ppc.ComboSelected = aValue;
            return true;
        }
        /// <summary>
        /// Adds a list of non-vendor type PackPatternCombo's to the method
        /// </summary>
        /// <param name="aCombinationList">PackPatternCombo list to add to the method.</param>
        /// <param name="aStatusReason">The reason the add failed.</param>
        /// <returns>True: Add was successful, aStatusReason will be null in this case; False: Add failed, aStatusReason will give a reason for the failure.</returns>
        public bool AddCombinations(List<PackPatternCombo> aCombinationList, out MIDException aStatusReason)
        {
            if (aCombinationList != null)
            {
                foreach (PackPatternCombo ppc in aCombinationList)
                {
                    if (ppc.PackPatternType != ePackPatternType.BuildPacksMethod)
                    {
                        aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustBeBuildPacksMethodType, MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboMustBeBuildPacksMethodType));
                        return false;
                    }
                }
                PackPatternCombo ppcClone; // TT#749 - SQL "error near ')' when updt or process bld pack" - Jellis
                foreach (PackPatternCombo ppc in aCombinationList)
                {
                    // begin TT#749 - SQL "error near ')' when updt or process bld pack" - J.Ellis
                    ppcClone = (PackPatternCombo)ppc.Clone();
                    _buildPacksComboDictionary.Add(ppc.ComboRID, ppcClone);
                    foreach (PackPattern pp in ppcClone.PackPatternList)
                    {
                        _buildPacksPatternDictionary.Add(pp.PackPatternRID, pp);
                    }
                    _packCombination.Add(ppcClone);
                    //_packCombination.Add((PackPatternCombo)ppc.Clone());
                    // end TT#749 - SQL "error near ')' when updt or process bld pack" - J.Ellis
                }
            }
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Replaces the non-Vendor PackPatternCombo's in the method with the new list of pack pattern combinations.  The vendor pack pattern combinations, if any, will remain attached to the method.
        /// </summary>
        /// <param name="aCombinationList">List of PackPatternCombo's that will replace the existing list of non-vendor PackPatternCombo's (List may be empty if there is a vendor pack pattern combination attached to this method)</param>
        /// <param name="aStatusReason">The reason the replace failed.</param>
        /// <returns>True: Replace was successful, in this case aStatusReason will be null; False: Replace failed, aStatusReason will give a reason for the failure.</returns>
        public bool ReplaceCombinations(List<PackPatternCombo> aCombinationList, out MIDException aStatusReason)
        {
            List<PackPatternCombo> combinationList = new List<PackPatternCombo>();
            Dictionary<int, PackPatternCombo> buildPackCombination = new Dictionary<int, PackPatternCombo>();  // TT#607 Delete Pack Combo not working
            Dictionary<int, PackPattern> buildPackPattern = new Dictionary<int,PackPattern>();                 // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            foreach (PackPatternCombo ppc in _packCombination)
            {
                if (ppc.PackPatternType == ePackPatternType.Vendor)
                {
                    combinationList.Add(ppc);
                }
            }
            if (aCombinationList != null)
            {
                foreach (PackPatternCombo ppc in aCombinationList)
                {
                    if (ppc.PackPatternType != ePackPatternType.BuildPacksMethod)
                    {
                        aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustBeBuildPacksMethodType, MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboMustBeBuildPacksMethodType));
                        return false;
                    }
                    // begin TT#607 Delete Pack Combo not working
                    PackPatternCombo ppcClone = (PackPatternCombo)ppc.Clone();
                    combinationList.Add(ppcClone);
                    buildPackCombination.Add(ppcClone.ComboRID, ppcClone);
                    //combinationList.Add((PackPatternCombo)ppc.Clone());
                    // end TT#607 Delete Pack Combo not working
                    // begin TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
                    foreach (PackPattern pp in ppcClone.PackPatternList)
                    {
                        buildPackPattern.Add(pp.PackPatternRID, pp);
                    }
                    // end TT#749 - SQL "error near ')' when updt or process Bld Pack" - Jellis
                }
            }
            if (combinationList.Count == 0)
            {
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern, MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern));
                return false;
            }
            // begin TT#607 Delete Pack Combo not working
            PackPatternCombo newPackPatternCombo;  // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            int comboRID;                          // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            //foreach (int comboRID in _buildPacksComboDictionary.Keys) // TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
            foreach (PackPatternCombo oldPackPatternCombo in _buildPacksComboDictionary.Values)
            {
                // begin TT#749 - SQL "error near ')' when updt or process Bld Pack" - JEllis
                //if (!buildPackCombination.ContainsKey(comboRID))
                comboRID = oldPackPatternCombo.ComboRID;
                if (buildPackCombination.TryGetValue(comboRID, out newPackPatternCombo))
                {
                    foreach (int packPatternRID in _buildPacksPatternDictionary.Keys)
                    {
                        if (!buildPackPattern.ContainsKey(packPatternRID))
                        {
                            if (packPatternRID > -1)
                            {
                                _deletePackPatternRIDList.Add(packPatternRID);
                            }
                        }
                    }
                }
                else
                    // end TT#749 -SQL "error near ')' when updt or process Bld Pack" - JEllis
                {
                    // begin TT#747 - JEllis - Cannot modify custom pack combination
                    if (comboRID > -1)
                    {
                        _deletePackComboRIDList.Add(comboRID);
                    }
                    // end TT#747 - JEllis - Cannot modify custom pack combination
                }
            }
            // end TT#607 Delete Pack Combo not working
            _packCombination = combinationList;
            _buildPacksComboDictionary = buildPackCombination;  // TT#607 Delete Pack Combo not working
            _buildPacksPatternDictionary = buildPackPattern;    // TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
            aStatusReason = null;
            return true;
        }
        /// <summary>
        /// Removes the non-Vendor PackPatternCombo's from the method. 
        /// </summary>
        /// <param name="aStatusReason">The reason the remove failed</param>
        /// <remarks>The removal is valid only if a vendor is attached to the method and that vendor has an existing PackPatternCombo attached to it.</remarks>
        /// <returns>True: Removal of all non-Vendor PackPatternCombo's from this method, in this case aStatusReason will be null; False: Removal failed, in this case aStatusReason will give the reason for the failure</returns>
        public bool RemoveCombinations(out MIDException aStatusReason)
        {
            bool combinationsIncludeVendor = false;
            List<PackPatternCombo> combinationList = new List<PackPatternCombo>();
            List<int> deletePackComboRIDList = new List<int>(); // TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
            foreach (PackPatternCombo ppc in _packCombination)
            {
                if (ppc.PackPatternType == ePackPatternType.Vendor)
                {
                    combinationsIncludeVendor = true;
                    combinationList.Add(ppc);
                }
                    // begin TT#607 BP Delete Combo not working
                else
                {
                    if (ppc.ComboRID > -1)
                    {
                        deletePackComboRIDList.Add(ppc.ComboRID); // TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
                    }
                }
                // end TT#607 BP Delete Combo not working
            }
            if (combinationsIncludeVendor)
            {
                _packCombination = combinationList;
                _buildPacksComboDictionary = new Dictionary<int, PackPatternCombo>(); // TT#607 Delete Pack Combo not working
                _buildPacksPatternDictionary = new Dictionary<int, PackPattern>();    // TT#749 - SQL "error near ')' when updt or process bld pack" - Jellis
                _deletePackComboRIDList.AddRange(deletePackComboRIDList);             // TT#749 - SQL "error near ')' when updt or process bld pack" - Jellis
                aStatusReason = null;
                return true;
            }
            aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern, MIDText.GetTextOnly(eMIDTextCode.msg_al_ComboMustContainAtLeast1Pattern));
            return false;
        }

        // begin TT#669 Build Pack Variance Enhancement
        public bool SetIncreaseBuyPct(double aIncreaseBuyPct, out MIDException aStatusReason)
        {
            if (aIncreaseBuyPct < 0)
            {
                aStatusReason = 
                    new MIDException(eErrorLevel.warning, 
                        (int)eMIDTextCode.msg_al_IncreaseBuyPctCannotBeNegative, 
                        MIDText.GetTextOnly(eMIDTextCode.msg_al_IncreaseBuyPctCannotBeNegative));
                return false;
            }
            _increaseBuyPct = aIncreaseBuyPct;
            aStatusReason = null;
            return true;
        }
        // end TT#669 Build Pack Variance Enhancement

        /// <summary>
        /// Populates BuildPacks Method from the database
        /// </summary>
        /// <param name="aMethodRID">The BuildPacks Method RID</param>
        /// <param name="aStatusReason">If populate fails, this mesage will contain a "reason" for the failure; If the populate succeeds, this message will be empty.</param>
        /// <returns>True: Method was populated successfully; False: Method population failed. </returns>
        public bool PopulateBuildPacks(int aMethodRID, out MIDException aStatusReason)
        {
            try
            {
                if (PopulateMethod(aMethodRID))
                {
        
                    DataTable dt = StoredProcedures.MID_METHOD_BLD_PACKS_READ.Read(_dba, METHOD_RID: aMethodRID);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        _bPC_RID = Convert.ToInt32(dr["BPC_RID"], CultureInfo.CurrentUICulture);
                        _pack_Min = Convert.ToInt32(dr["PATTERN_COMP_MIN"], CultureInfo.CurrentUICulture);   // TT#787 Vendor Min Order applies only to packs
                        _size_Multiple = Convert.ToInt32(dr["PATTERN_SIZE_MULTIPLE"], CultureInfo.CurrentUICulture);  // Correction
                        _sizeGroupRID = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
                        _sizeCurveGroupRID = Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"], CultureInfo.CurrentUICulture);
                        _reserveTotalQty = Convert.ToInt32(dr["RESERVE_TOTAL"], CultureInfo.CurrentUICulture);
                        _reserveTotalIsPercent = Include.ConvertCharToBool(Convert.ToChar(dr["RESERVE_TOTAL_PERCENT_IND"], CultureInfo.CurrentUICulture)); // Correction
                        _reserveBulkQty = Convert.ToInt32(dr["RESERVE_BULK"], CultureInfo.CurrentUICulture);
                        _reserveBulkIsPercent = Include.ConvertCharToBool(Convert.ToChar(dr["RESERVE_BULK_PERCENT_IND"], CultureInfo.CurrentUICulture));   // Correction
                        _reservePackQty = Convert.ToInt32(dr["RESERVE_PACKS"], CultureInfo.CurrentUICulture);
                        _reservePackIsPercent = Include.ConvertCharToBool(Convert.ToChar(dr["RESERVE_PACKS_PERCENT_IND"], CultureInfo.CurrentUICulture));  // Correction
                        // begin Correction
                        if (dr["AVG_PACK_DEV_TOLERANCE"] == System.DBNull.Value)
                        {
                            _avg_Pack_Dev_Tolerance = Include.DefaultPackSizeErrorPercent;
                        }
                        else
                        {
                            _avg_Pack_Dev_Tolerance = Convert.ToDouble(dr["AVG_PACK_DEV_TOLERANCE"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["MAX_PACK_NEED_TOLERANCE"] == System.DBNull.Value)
                        {
                            _max_Pack_Need_Tolerance = Include.DefaultBldPacksMaxPackNeedTol;  // Correction
                        }
                        else
                        {
                            double intermNeedTol = Convert.ToDouble(dr["MAX_PACK_NEED_TOLERANCE"], CultureInfo.CurrentUICulture);    // Correction 2
                            if (intermNeedTol < int.MaxValue)
                            {
                                _max_Pack_Need_Tolerance = (int)intermNeedTol; // Correction 2
                            }
                            else
                            {
                                _max_Pack_Need_Tolerance = Include.DefaultBldPacksMaxPackNeedTol; // Correction
                            }
                        }
                        // end Correction
                        // begin TT#669 Build Packs Variance Enhancement
                        _depleteReserveSelected = Include.ConvertCharToBool(Convert.ToChar(dr["DEPLETE_RESERVE_SELECTED"], CultureInfo.CurrentUICulture));
                        _increaseBuySelected = Include.ConvertCharToBool(Convert.ToChar(dr["INCREASE_BUY_SELECTED"], CultureInfo.CurrentUICulture));
                        if (dr["INCREASE_BUY_PCT"] == System.DBNull.Value)
                        {
                            _increaseBuyPct = double.MaxValue;
                        }
                        else
                        {
                            _increaseBuyPct = Convert.ToDouble(dr["INCREASE_BUY_PCT"], CultureInfo.CurrentUICulture);
                        }
                        // end TT#669 Build Packs Variance Enhancement
                        _removeBulkInd = Include.ConvertCharToBool(Convert.ToChar(dr["REMOVE_BULK_IND"], CultureInfo.CurrentUICulture)); // TT#744 - JEllis - Use Orig Pack Fit Logic; Option Bulk
                    }
                    _packCombination = new List<PackPatternCombo>();
                    if (_bPC_RID != Include.NoRID)
                    {
                        //if (!Set_BPC_Data(Get_BPC_Table_SQL_Select().Append(" where bpc.BPC_RID = '").Append(_bPC_RID.ToString(CultureInfo.CurrentUICulture)).Append("' "), out aStatusReason))
                        if (!Set_BPC_Data(out aStatusReason, "", _bPC_RID))
                        {
                            return false;
                        }
                    }

                    dt = StoredProcedures.MID_METHOD_BLD_PACKS_BPC_SELECT_READ.Read(_dba, METHOD_RID: aMethodRID);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            // intentionally drop information messages
                            if (!this.SetCombinationSelected(
                                ePackPatternType.Vendor,
                                Convert.ToInt32(dr["BPC_COMBO_RID"], CultureInfo.CurrentCulture),
                                true,
                                out aStatusReason))
                            {
                                if (aStatusReason.ErrorLevel != eErrorLevel.information)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return Set_Method_Combo_Pattern_Data(aMethodRID, out aStatusReason); 
                }
            }
            catch (MIDException e)
            {
                aStatusReason = e;
                return false;
            }
            catch (Exception e)
            {
                aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                return false;
            }
            aStatusReason = null;
            return true;

        }

        /// <summary>
        /// Sets the BPC (aka Vendor) data in this object (BPC_RID, BPC_Name, BPC_Comp_Min, BPC_Size_Multiple and updates the PackCombinations appropriately (ie. removes any existing BPC PackCombinations and adds any PackCombinations associated with this BPC_Name or Vendor).
        /// </summary>
        /// <param name="aSQL_BPC_GetCommand">A Select SQL Command constructed to retrieve the data--see also Get_BPC_Table_SQL_Select.</param>
        /// <param name="aStatusReason">When the Set fails, this message will contain a "reason" for the failure; When the Set succeeds, this message will be empty.</param>
        /// <returns>True: Set succeeded; False: Set Failed in which case aStatusReason will contain a resaon for the failure.</returns>
        private bool Set_BPC_Data(out MIDException aStatusReason, string aBPC_NAME = "", int? aBPC_RID = null)
        {
            try
            {
                //Begin TT#762 - Changed the component on Nike vendor and did buildpack load-> error message when opening application - apicchetti - 7/2/2010
                DataTable dt;
         
                if (aBPC_RID != null)
                {
                    dt = StoredProcedures.MID_BUILD_PACK_CRITERIA_READ.Read(_dba, BPC_RID: aBPC_RID);
                }
                else
                {
                    dt = StoredProcedures.MID_BUILD_PACK_CRITERIA_READ_FROM_NAME.Read(_dba, BPC_NAME: aBPC_NAME);
                }
                //End TT#762 - Changed the component on Nike vendor and did buildpack load-> error message when opening application - apicchetti - 7/2/2010

                if (dt.Rows.Count > 0)
                {
                    List<PackPatternCombo> packCombination = new List<PackPatternCombo>();
                    Dictionary<int, PackPatternCombo> vendorComboDictionary = new Dictionary<int, PackPatternCombo>();
                    int i = 0;
                    DataRow dr = dt.Rows[0];
                    int bPC_RID = Convert.ToInt32(dr["BPC_RID"], CultureInfo.CurrentUICulture);
                    string bPC_Name = Convert.ToString(dr["BPC_Name"], CultureInfo.CurrentUICulture);
                    int bPC_Pack_Min = Convert.ToInt32(dr["PACK_MIN"], CultureInfo.CurrentUICulture); // TT#787 Vendor Min Order applies only to packs
                    int bPC_Size_Multiple = Convert.ToInt32(dr["SIZE_MULT"], CultureInfo.CurrentUICulture);
                    int currentComboRID, newComboRID;
                    string comboName;
                    newComboRID = Convert.ToInt32(dr["COMBO_RID"], CultureInfo.CurrentUICulture);
                    while (i < dt.Rows.Count)
                    {
                        currentComboRID = newComboRID;
                        comboName = Convert.ToString(dr["COMBO_NAME"], CultureInfo.CurrentUICulture);
                        List<PackPattern> ppList = new List<PackPattern>();
                        while (currentComboRID == newComboRID)
                        {
                            Vendor_PackPattern vpp =
                                new Vendor_PackPattern(
                                    bPC_RID,
                                    Convert.ToInt32(dr["PATTERN_RID"], CultureInfo.CurrentUICulture),
                                    "v_" + i.ToString(CultureInfo.CurrentUICulture),
                                    Convert.ToInt32(dr["PATTERN_MULT"], CultureInfo.CurrentUICulture),
                                    Convert.ToInt32(dr["COMBO_MAX_PACKS"], CultureInfo.CurrentUICulture));
                            ppList.Add(vpp);
                            i++;
                            if (i < dt.Rows.Count)
                            {
                                dr = dt.Rows[i];
                                newComboRID = Convert.ToInt32(dr["COMBO_RID"], CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                newComboRID = -1;
                            }
                        }
                        Vendor_Combo vc =
                            new Vendor_Combo(
                                _bPC_RID,
                                currentComboRID,
                                comboName,
                                false,
                                ppList);
                        packCombination.Add(vc);
                        vendorComboDictionary.Add(vc.ComboRID, vc);
                    }
                    _bPC_RID = bPC_RID;
                    _bPC_Name = bPC_Name;
                    _bPC_Pack_Min = bPC_Pack_Min; // TT#787 Vendor Min Order applies only to packs
                    _bPC_Size_Multiple =     // TT#518 Size Multiple Incorrect
                        bPC_Size_Multiple;   // TT#518 Size Multiple Incorrect
                    if (_bPC_RID == Include.NoRID)
                    {
                        _pack_Min = 0; // TT#787 Vendor Min Order applies only to packs
                        _size_Multiple = 1;
                    }
                    foreach (PackPatternCombo ppc in _packCombination)
                    {
                        // begin TT#518 Switching Vendor Retains old vendor info
                        if (ppc.PackPatternType != ePackPatternType.Vendor)
                        {
                            packCombination.Add(ppc);
                        }
                        // end TT#518 Switching Vendor Retains old vendor infor
                    }
                    _packCombination = packCombination;
                    _vendorComboDictionary = vendorComboDictionary;
                    aStatusReason = null;
                    return true;
                }
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_VendorIsNotValid, MIDText.GetTextOnly(eMIDTextCode.msg_al_VendorIsNotValid));
                return false;
            }
            catch (MIDException e)
            {
                aStatusReason = e;
                return false;
            }
            catch (Exception e)
            {
                aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                return false;
            }
        }

    
        /// <summary>
        /// Sets the Method Pack Pattern data in this object.
        /// </summary>
        /// <param name="aSQL_BPC_GetCommand">A Select SQL Command constructed to retrieve the data--see also Get_Method_Combo_Pattern_Select.</param>
        /// <param name="aStatusReason">When the Set fails, this message will contain a "reason" for the failure; When the Set succeeds, this message will be empty.</param>
        /// <returns>True: Set succeeded; False: Set Failed in which case aStatusReason will contain a resaon for the failure.</returns>
        private bool Set_Method_Combo_Pattern_Data(int aMETHOD_RID, out MIDException aStatusReason)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_READ.Read(_dba, METHOD_RID: aMETHOD_RID);
                if (dt.Rows.Count > 0)
                {
                    List<PackPatternCombo> packCombination = new List<PackPatternCombo>();
                    Dictionary<int, PackPatternCombo> buildPacksComboDictionary = new Dictionary<int, PackPatternCombo>();
                    Dictionary<int, PackPattern> buildPacksPatternDictionary = new Dictionary<int, PackPattern>(); // TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
                    foreach (PackPatternCombo ppc in _packCombination)
                    {
                        if (ppc.PackPatternType == ePackPatternType.Vendor)
                        {
                            packCombination.Add(ppc);
                        }
                    }
                    int i = 0;
                    DataRow dr = dt.Rows[0];
                    int methodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
                    bool comboSelected; 
                    int currentComboRID, newComboRID, currentPackPatternRID, newPackPatternRID, patternPackMult, maxPacks, sizeCodeRID, sizeUnits;
                    string comboName, packPatternName;
                    List<SizeUnits> sizeUnitsList;
                    newComboRID = Convert.ToInt32(dr["COMBO_RID"], CultureInfo.CurrentUICulture);
                    newPackPatternRID = Convert.ToInt32(dr["PATTERN_PACK_RID"], CultureInfo.CurrentUICulture);
                    BuildPacksMethod_PackPattern bpp;
                    while (i < dt.Rows.Count)
                    {
                        currentComboRID = newComboRID;
                        comboName = Convert.ToString(dr["COMBO_NAME"], CultureInfo.CurrentUICulture);
                        comboSelected = Include.ConvertCharToBool(Convert.ToChar(dr["COMBO_SELECTED_IND"], CultureInfo.CurrentUICulture));  // Correction
                        List<PackPattern> ppList = new List<PackPattern>();
                        while (currentComboRID == newComboRID)
                        {
                            sizeUnitsList = new List<SizeUnits>();
                            currentPackPatternRID = newPackPatternRID;
                            packPatternName = Convert.ToString(dr["PATTERN_NAME"], CultureInfo.CurrentUICulture);
                            patternPackMult = Convert.ToInt32(dr["PATTERN_PACK_MULT"], CultureInfo.CurrentUICulture);
                            maxPacks = Convert.ToInt32(dr["COMBO_MAX_PATTERN_PACKS"], CultureInfo.CurrentUICulture);

                            while (currentComboRID == newComboRID
                                   && currentPackPatternRID == newPackPatternRID)
                            {
                                sizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
                                if (sizeCodeRID != Include.NoRID)
                                {
                                    sizeUnits = Convert.ToInt32(dr["PATTERN_SIZE_UNITS"], CultureInfo.CurrentUICulture);
                                    sizeUnitsList.Add(new SizeUnits(sizeCodeRID, sizeUnits));
                                }
                                i++;
                                if (i < dt.Rows.Count)
                                {
                                    dr = dt.Rows[i];
                                    newComboRID = Convert.ToInt32(dr["COMBO_RID"], CultureInfo.CurrentUICulture);
                                    newPackPatternRID = Convert.ToInt32(dr["PATTERN_PACK_RID"], CultureInfo.CurrentUICulture);
                                }
                                else
                                {
                                    newComboRID = Include.NoRID;
                                    newPackPatternRID = Include.NoRID;
                                }
                            }
                            if (sizeUnitsList.Count > 0)
                            {
                                bpp = 
                                    new BuildPacksMethod_PackPattern(
                                        methodRID,
                                        currentPackPatternRID,
                                        packPatternName,
                                        sizeUnitsList);
                            }
                            else
                            {
                               bpp =
                                    new BuildPacksMethod_PackPattern(
                                        methodRID,
                                        currentPackPatternRID,
                                        packPatternName,
                                        patternPackMult,
                                        maxPacks);
                            }
                            ppList.Add(bpp);
                            buildPacksPatternDictionary.Add(bpp.PackPatternRID, bpp); // TT#749 - SQL "error near ')' when upd or process bld pack" - JEllis
                        }
                        BuildPacksMethod_Combo bc =
                            new BuildPacksMethod_Combo(
                                methodRID,
                                currentComboRID,
                                comboName,
                                comboSelected,
                                ppList);
                        packCombination.Add(bc);
                        buildPacksComboDictionary.Add(bc.ComboRID, bc);
                    }
                    _packCombination = packCombination;
                    _buildPacksComboDictionary = buildPacksComboDictionary;
                    _buildPacksPatternDictionary = buildPacksPatternDictionary; // TT#749 - SQL "error near ')' when upd or process bld pack" - JEllis
                }
                if (_packCombination.Count > 0)
                {
                    aStatusReason = null; 
                    return true;
                }
                aStatusReason = new MIDException(eErrorLevel.warning, (int)eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod, string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_NoPatternsFoundForBuildPacksMethod),this.Method_Name));
                return false;
            }
            catch (MIDException e)
            {
                aStatusReason = e;
                return false;
            }
            catch (Exception e)
            {
                aStatusReason = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
                return false;
            }
        }
        public bool InsertMethod(int aMethod_RID, TransactionData td)
        {
            bool InsertSuccessful = true;
            _dba = td.DBA;
            try
            {
                base.Method_RID = aMethod_RID;
               
                int? BPC_RID_Nullable = null;
                if (_bPC_RID != Include.NoRID) BPC_RID_Nullable = _bPC_RID;

                int? SIZE_GROUP_RID_Nullable = null;
                if (_sizeGroupRID != Include.NoRID) SIZE_GROUP_RID_Nullable = _sizeGroupRID;

                int? SIZE_CURVE_GROUP_RID_Nullable = null;
                if (_sizeCurveGroupRID != Include.NoRID) SIZE_CURVE_GROUP_RID_Nullable = _sizeCurveGroupRID;

                StoredProcedures.MID_METHOD_BLD_PACKS_INSERT.Insert(_dba,
                                                                    METHOD_RID: aMethod_RID,
                                                                    BPC_RID: BPC_RID_Nullable,
                                                                    PATTERN_COMP_MIN: _pack_Min,
                                                                    PATTERN_SIZE_MULTIPLE: _size_Multiple,
                                                                    SIZE_GROUP_RID: SIZE_GROUP_RID_Nullable,
                                                                    SIZE_CURVE_GROUP_RID: SIZE_CURVE_GROUP_RID_Nullable,
                                                                    RESERVE_TOTAL: _reserveTotalQty,
                                                                    RESERVE_TOTAL_PERCENT_IND: Include.ConvertBoolToChar(_reserveTotalIsPercent),
                                                                    RESERVE_BULK: _reserveBulkQty,
                                                                    RESERVE_BULK_PERCENT_IND: Include.ConvertBoolToChar(_reserveBulkIsPercent),
                                                                    RESERVE_PACKS: _reservePackQty,
                                                                    RESERVE_PACKS_PERCENT_IND: Include.ConvertBoolToChar(_reservePackIsPercent),
                                                                    AVG_PACK_DEV_TOLERANCE: _avg_Pack_Dev_Tolerance,
                                                                    MAX_PACK_NEED_TOLERANCE: _max_Pack_Need_Tolerance,
                                                                    DEPLETE_RESERVE_SELECTED: Include.ConvertBoolToChar(_depleteReserveSelected),
                                                                    INCREASE_BUY_SELECTED: Include.ConvertBoolToChar(_increaseBuySelected),
                                                                    INCREASE_BUY_PCT: _increaseBuyPct,
                                                                    REMOVE_BULK_IND: Include.ConvertBoolToChar(_removeBulkInd)
                                                                    );


                // Begin TT#5089 - JSmith - Copy does not copy all pack combinations
                //InsertSuccessful = UpdateChildData();
                InsertSuccessful = UpdateChildData(isInsert : true);
                // End TT#5089 - JSmith - Copy does not copy all pack combinations
            }
            catch (MIDException)
            {
                InsertSuccessful = false;
                throw;
            }
            catch (Exception e)
            {
                InsertSuccessful = false;
                throw new MIDException(eErrorLevel.severe,(int)eMIDTextCode.systemError,MIDText.GetTextOnly(eMIDTextCode.systemError),e);
            }
            return InsertSuccessful;
        }
        public bool UpdateMethod(int aMethodRID, TransactionData td)
        {
			_dba = td.DBA;

			bool UpdateSuccessful = true;

			try
			{
                UpdateSuccessful = UpdateBuildPacksMethod(aMethodRID);
			}
            catch (MIDException)
            {
                UpdateSuccessful = false;
                throw;
            }
			catch (Exception e)
			{
				UpdateSuccessful = false;
                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
			}
			finally
			{
			}
			return UpdateSuccessful;
		}
        private bool UpdateBuildPacksMethod(int aMethod_RID)
        {
            bool UpdateSuccessful = true;
            try
            {
                
                int? BPC_RID_Nullable = null;
                if (_bPC_RID != Include.NoRID) BPC_RID_Nullable = _bPC_RID;

                int? SIZE_GROUP_RID_Nullable = null;
                if (_sizeGroupRID != Include.NoRID) SIZE_GROUP_RID_Nullable = _sizeGroupRID;

                int? SIZE_CURVE_GROUP_RID_Nullable = null;
                if (_sizeCurveGroupRID != Include.NoRID) SIZE_CURVE_GROUP_RID_Nullable = _sizeCurveGroupRID;

                StoredProcedures.MID_METHOD_BLD_PACKS_UPDATE.Update(_dba,
                                                                    METHOD_RID: aMethod_RID,
                                                                    BPC_RID: BPC_RID_Nullable,
                                                                    PATTERN_COMP_MIN: _pack_Min,
                                                                    PATTERN_SIZE_MULTIPLE: _size_Multiple,
                                                                    SIZE_GROUP_RID: SIZE_GROUP_RID_Nullable,
                                                                    SIZE_CURVE_GROUP_RID: SIZE_CURVE_GROUP_RID_Nullable,
                                                                    RESERVE_TOTAL: _reserveTotalQty,
                                                                    RESERVE_TOTAL_PERCENT_IND: Include.ConvertBoolToChar(_reserveTotalIsPercent),
                                                                    RESERVE_BULK: _reserveBulkQty,
                                                                    RESERVE_BULK_PERCENT_IND: Include.ConvertBoolToChar(_reserveBulkIsPercent),
                                                                    RESERVE_PACKS: _reservePackQty,
                                                                    RESERVE_PACKS_PERCENT_IND: Include.ConvertBoolToChar(_reservePackIsPercent),
                                                                    AVG_PACK_DEV_TOLERANCE: _avg_Pack_Dev_Tolerance,
                                                                    MAX_PACK_NEED_TOLERANCE: _max_Pack_Need_Tolerance,
                                                                    DEPLETE_RESERVE_SELECTED: Include.ConvertBoolToChar(_depleteReserveSelected),
                                                                    INCREASE_BUY_SELECTED: Include.ConvertBoolToChar(_increaseBuySelected),
                                                                    INCREASE_BUY_PCT: _increaseBuyPct,
                                                                    REMOVE_BULK_IND: Include.ConvertBoolToChar(_removeBulkInd)
                                                                    );
                UpdateSuccessful = UpdateChildData();
            }
            catch (MIDException)
            {
                UpdateSuccessful = false;
                throw;
            }
            catch (Exception e)
            {
                UpdateSuccessful = false;
                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
            }
            finally
            {
            }
            return UpdateSuccessful;
        }

        // Begin TT#5089 - JSmith - Copy does not copy all pack combinations
        //private bool UpdateChildData()
        private bool UpdateChildData(bool isInsert = false)
        // End TT#5089 - JSmith - Copy does not copy all pack combinations
        {
            try
            {
               
                StoredProcedures.MID_METHOD_BLD_PACKS_BPC_SELECT_DELETE.Delete(_dba, METHOD_RID: Method_RID);

               
                StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_DELETE.Delete(_dba, METHOD_RID: Method_RID);
                
               

                DataTable dtComboList = null;
                if (_deletePackComboRIDList.Count > 0)
                {
                    dtComboList = new DataTable();
                    dtComboList.Columns.Add("COMBO_RID", typeof(int));
                    foreach (int comboRID in _deletePackComboRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtComboList.Select("COMBO_RID=" + comboRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtComboList.NewRow();
                            dr["COMBO_RID"] = comboRID;
                            dtComboList.Rows.Add(dr);
                        }
                    }
                    StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_DELETE.Delete(_dba,
                                                                          METHOD_RID: Method_RID,
                                                                          COMBO_RID_LIST: dtComboList
                                                                          );
                }
               
                DataTable dtPatternPackList = null;
                if (_deletePackPatternRIDList.Count > 0)
                {
                    dtPatternPackList = new DataTable();
                    dtPatternPackList.Columns.Add("PATTERN_PACK_RID", typeof(int));
                    foreach (int comboRID in _deletePackPatternRIDList)
                    {
                        //ensure userRIDs are distinct, and only added to the datatable one time
                        if (dtPatternPackList.Select("PATTERN_PACK_RID=" + comboRID.ToString()).Length == 0)
                        {
                            DataRow dr = dtPatternPackList.NewRow();
                            dr["PATTERN_PACK_RID"] = comboRID;
                            dtPatternPackList.Rows.Add(dr);
                        }
                    }
                    StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_PATTERN_DELETE.Delete(_dba,
                                                                                      METHOD_RID: Method_RID,
                                                                                      PATTERN_PACK_RID_LIST: dtPatternPackList
                                                                                      );
                }


                foreach (PackPatternCombo ppc in this._packCombination)
                {
                    if (ppc.PackPatternType == ePackPatternType.Vendor)
                    {
                        if (ppc.ComboSelected)
                        {
                            StoredProcedures.MID_METHOD_BLD_PACKS_BPC_SELECT_INSERT.Insert(_dba,
                                                                               METHOD_RID: Method_RID,
                                                                               BPC_COMBO_RID: ppc.ComboRID
                                                                               );
                        }
                    }
                    else
                    {
                        // Begin TT#5089 - JSmith - Copy does not copy all pack combinations
                        //if (ppc.ComboRID < 0)
                        if (ppc.ComboRID < 0 ||
                            isInsert)
                        // ENd TT#5089 - JSmith - Copy does not copy all pack combinations
                        {
                            ppc.ComboRID = StoredProcedures.SP_MID_MTHD_BPCOMBO_INSERT.InsertAndReturnRID(_dba,
                                                                                                        METHOD_RID: Method_RID,
                                                                                                        COMBO_NAME: ppc.ComboName,
                                                                                                        COMBO_SELECTED_IND: Include.ConvertBoolToChar(ppc.ComboSelected)
                                                                                                        );
                        }
                        else
                        {
                            StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_UPDATE.Update(_dba,
                                                                          METHOD_RID: Method_RID,
                                                                          COMBO_RID: ppc.ComboRID,
                                                                          COMBO_NAME: ppc.ComboName,
                                                                          COMBO_SELECTED_IND: Include.ConvertBoolToChar(ppc.ComboSelected)
                                                                          );
                        }
                        List<PackPattern> packPatternList = ppc.PackPatternList;
                        foreach (PackPattern pp in packPatternList)
                        {
                            // Begin TT#5089 - JSmith - Copy does not copy all pack combinations
                            //if (pp.PackPatternRID < 0)
                            if (pp.PackPatternRID < 0 ||
                                isInsert)
                            // End TT#5089 - JSmith - Copy does not copy all pack combinations
                            {
                                pp.PackPatternRID = StoredProcedures.SP_MID_MTHD_BPCPATTERN_INSERT.InsertAndReturnRID(_dba,
                                                                                                                   METHOD_RID: Method_RID,
                                                                                                                   COMBO_RID: ppc.ComboRID,
                                                                                                                   PATTERN_NAME: pp.PatternName,
                                                                                                                   PATTERN_PACK_MULT: pp.PackMultiple,
                                                                                                                   COMBO_MAX_PATTERN_PACKS: pp.MaxPatternPacks
                                                                                                                   );
                            }
                            else
                            {
                                StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_PATTERN_UPDATE.Update(_dba,
                                                                                  METHOD_RID: Method_RID,
                                                                                  COMBO_RID: ppc.ComboRID,
                                                                                  PATTERN_PACK_RID: pp.PackPatternRID,
                                                                                  PATTERN_NAME: pp.PatternName,
                                                                                  PATTERN_PACK_MULT: pp.PackMultiple,
                                                                                  COMBO_MAX_PATTERN_PACKS: pp.MaxPatternPacks
                                                                                  );
                            }
                            List<SizeUnits> sizeRun = pp.SizeUnitsList;
                            foreach (SizeUnits su in sizeRun)
                            {
                                StoredProcedures.MID_METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN_INSERT.Insert(_dba,
                                                                                                        METHOD_RID: Method_RID,
                                                                                                        COMBO_RID: ppc.ComboRID,
                                                                                                        PATTERN_PACK_RID: pp.PackPatternRID,
                                                                                                        SIZE_CODE_RID: su.RID,
                                                                                                        PATTERN_SIZE_UNITS: su.Units
                                                                                                        );
                            }
                        }
                        MIDException statusReason = null;
                        if (!ppc.SetPackPatternList(packPatternList, out statusReason))
                        {
                            return false;
                        }
                    }
                }
            }
            catch (MIDException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, MIDText.GetTextOnly(eMIDTextCode.systemError), e);
            }
            finally
            {
                // begin TT#747 - JEllis - Cannot modify Custom Pack Combo
                _buildPacksComboDictionary.Clear();
                _buildPacksPatternDictionary.Clear(); // TT#749 - SQL "error near ')' when upd or process bld pack" - JEllis
                foreach (PackPatternCombo ppc in this._packCombination)
                {
                    _buildPacksComboDictionary.Add(ppc.ComboRID, ppc);
                    // begin TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
                    foreach (PackPattern pp in ppc.PackPatternList)
                    {
                        _buildPacksPatternDictionary.Add(pp.PackPatternRID, pp);
                    }
                    // end TT#749 - SQL "error near ')' when updt or process bld pack" - JEllis
                }
                // end TT#747 - JEllis - Cannot modify Custom Pack Combo
            }
            return true;
        }

        /// <summary>
        /// Returns the list of BPCs from the database
        /// </summary>
        /// <returns>BPC_NAMES in a single column DataTable</returns>
        public DataTable GetBPCList()
        {
            return StoredProcedures.MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES.Read(_dba);
        }

    }
}
