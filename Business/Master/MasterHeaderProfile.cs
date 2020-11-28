using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
    public class MasterHeaderProfile : AllocationProfile, ICloneable
    {
        #region Fields
        //=======
        // FIELDS
        //=======
        private AllocationProfileList _subordinates = null;
        private List<ItemManuallyAllocated> _manuallyAllocatedItems;

        #endregion Fields

        #region Constructors
        //=============
        // CONSTRUCTORS
        //=============
        /// <summary>
        /// Creates a new instance of the Master Header Profile
		/// </summary>
		/// <param name="aTransaction">Transaction associated with this profile.</param>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
        /// A Master Header profile describes the Master Header information of an allocation header.
		/// </remarks>
		public MasterHeaderProfile(Transaction aTransaction, string aHeaderID, int aKey, Session aSession)
			: base(aTransaction, aHeaderID, aKey, aSession)
		{
			Fill(false);
		}
		/// <summary>
		/// Creates a new instance of the Master Header Profile
		/// </summary>
		/// <param name="aSAB">SessionAddressBlock associated with this profile.</param>
		/// <param name="aHeaderID">The user assigned name of the allocation header.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <param name="aSession">The session where the profile is being instantiated</param>
		/// <remarks>
        /// A Master Header profile describes the Master Header information of an allocation header.
		/// </remarks>
        public MasterHeaderProfile(SessionAddressBlock aSAB, string aHeaderID, int aKey, Session aSession)
			: base(aSAB, aHeaderID, aKey, aSession)
		{
			Fill(false);	
		}
        #endregion Constructors

        #region Properties
        //===========
        // PROPERTIES
        //===========

        #endregion Properties

        #region Methods
        //==============
        // Methods
        //==============

        private void Fill(bool buildSummary)
        {
            ProcessingMasterHeader = true;
        }

        /// <summary>
        /// Updates the properties of the Master Header from the AllocationProfile header
        /// </summary>
        /// <param name="ap">The AllocationProfile from which to get the properties to copy.</param>
        /// <returns>A flag indicating if the process was successful</returns>
        public bool UpdatePropertiesFromHeader(List<AllocationProfile> lHeaders)
        {
            MIDException midException;

            StyleHnRID = lHeaders[0].StyleHnRID;
            HeaderDescription = lHeaders[0].HeaderDescription;
            HeaderDay = System.DateTime.Today;
            UnitCost = lHeaders[0].UnitCost;
            UnitRetail = lHeaders[0].UnitRetail;
            Vendor = lHeaders[0].Vendor;
            PurchaseOrder = lHeaders[0].PurchaseOrder;
            SetHeaderType(lHeaders[0].HeaderType, out midException);

            // set fields if all headers have same value
            int tempSizeGroupRID = lHeaders[0].SizeGroupRID;
            string tempPurchaseOrder = lHeaders[0].PurchaseOrder;
            foreach (AllocationProfile ap in lHeaders)
            {
                if (tempSizeGroupRID != Include.UndefinedSizeGroupRID
                    && ap.SizeGroupRID != tempSizeGroupRID)
                {
                    tempSizeGroupRID = Include.UndefinedSizeGroupRID;
                }
                if (tempPurchaseOrder != null
                    && ap.PurchaseOrder != tempPurchaseOrder)
                {
                    tempPurchaseOrder = null;
                }
            }

            if (tempSizeGroupRID != Include.UndefinedSizeGroupRID)
            {
                SizeGroupRID = tempSizeGroupRID;
            }

            if (tempPurchaseOrder != null)
            {
                PurchaseOrder = tempPurchaseOrder;
            }


            return true;
        }

        public bool UpdateCharacteristics(List<AllocationProfile> lHeaders)
        {
            // set characteristics if all headers have same value
            ArrayList charArrayList = new ArrayList();
            string hdrCharGrpID, hdrCharValue;
            eHeaderCharType headerCharType;
            CharacteristicsBin HdrCharBin;

            foreach (CharacteristicsBin CharBin in Characteristics.Values)
            {
                hdrCharGrpID = CharBin.Name;
                headerCharType = (eHeaderCharType)CharBin.DataType;
                hdrCharValue = null;
                foreach (AllocationProfile ap in lHeaders)
                {
                    HdrCharBin = (CharacteristicsBin)ap.Characteristics[hdrCharGrpID];
                    if (hdrCharValue == null)
                    {
                        hdrCharValue = HdrCharBin.Value;
                    }
                    else if (hdrCharValue != HdrCharBin.Value)
                    {
                        hdrCharValue = null;
                        break;
                    }
                }
                if (hdrCharValue != null
                    && hdrCharValue.Length > 0)
                {
                    int charsRtn = ProcessHeaderLoadCharacteristic(ref hdrCharGrpID, hdrCharValue, Key, "CREATE", false, headerCharType, false);
                }
            }

            


            return true;
        }

        /// <summary>
        /// Adds subordinate headers to the Master Header
        /// </summary>
        /// <param name="lSubordinateHeaders">The list of AllocationProfiles</param>
        /// <returns>A flag indicating if the process was successful</returns>
        public bool AddSubordinateHeaderReferences(List<AllocationProfile> lSubordinateHeaders, Header header)
        {
            foreach (AllocationProfile ap in lSubordinateHeaders)
            {
                AddSubordinateHeaderReference(ap, header);
            }
            return true;
        }

        private bool AddSubordinateHeaderReference(AllocationProfile ap, Header header)
        {
            return header.CreateSubordMaster(ap.Key, Key, Include.Undefined, Include.NoRID, Include.NoRID, Include.Undefined, Include.NoRID, Include.NoRID);
        }

        /// <summary>
        /// Adds subordinate headers to the Master Header
        /// </summary>
        /// <param name="lSubordinateHeaders">The list of AllocationProfiles</param>
        /// <returns>A flag indicating if the process was successful</returns>
        public bool AddSubordinateHeaders(List<AllocationProfile> lSubordinateHeaders)
        {
            SizeGroupProfile sizeGroupProfile = null;
            if (SizeGroupRID != Include.UndefinedSizeGroupRID) 
            {
                sizeGroupProfile = new SizeGroupProfile(SizeGroupRID); 
            }

            foreach(AllocationProfile ap in lSubordinateHeaders )
            {
                AddSubordinateHeader(ap);
            }
            return true;
        }

        private bool AddSubordinateHeader(AllocationProfile ap)
        {
            TotalUnitsToAllocate += ap.TotalUnitsToAllocate;

            eAllocationType allocationType;
            SortedList sortedPacks = new SortedList();
            foreach (PackHdr ph in ap.Packs.Values)
            {
                while (sortedPacks.ContainsKey(ph.Sequence))
                {
                    ph.Sequence++;
                }
                sortedPacks.Add(ph.Sequence, ph);
            }
            foreach (PackHdr ph in sortedPacks.Values)
            {
                PackHdr headerPack = null;
                if (PackIsOnHeader(ph, out headerPack))
                {
                    SetPacksToAllocate(headerPack, headerPack.PacksToAllocate + ph.PacksToAllocate);
                    headerPack.SetAssociatedPackRIDs(ph.PackRID);
                }
                else
                {
                    // set the pack name to the original subordinate pack name for matching.
                    ph.SetOriginalPackName(ph.PackName);
                    ph.SetAssociatedPackName(ph.PackName);  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
                    string packName;

                    // determine the next available pack name
                    packName = ph.PackName;
                    int seq = 1;
                    while (PackIsOnHeader(packName))
                    {
                        packName = ph.OriginalPackName + "-" + seq;
                        ++seq;  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
                    }
 
                    if (ph.GenericPack)
                    {
                        allocationType = eAllocationType.GenericType;
                    }
                    else
                    {
                        allocationType = eAllocationType.DetailType;
                    }
                    if (ap.GetPackRID(ph) == Include.NoRID)
                    {
                        SetPackRID(ph, HeaderDataRecord.GetPackRID(ap.HeaderRID, ph.PackName));
                    }
                    AddPack(packName, allocationType, ph.PackMultiple, ph.PacksToAllocate, -1);
                    //SetAssociatedPackRID(packName, ap.GetPackRID(ph));
                    PackHdr masterPack = GetPackHdr(packName);
                    masterPack.SetAssociatedPackRIDs(ph.PackRID);
                    foreach (PackColorSize packColor in ph.PackColors.Values)
                    {
                        AddColorToPack(masterPack.PackName, packColor.ColorCodeRID, packColor.ColorUnitsInPack, packColor.ColorSequenceInPack);
                        foreach (PackSizeBin packSize in packColor.ColorSizes.Values)
                        {
                            AddSizeToPackColor(masterPack.PackName, packColor.ColorCodeRID, packSize.ContentCodeRID, packSize.ContentUnits, packSize.Sequence);
                        }
                    }
                }
            }

            HdrColorBin masterColorBin;
            HdrSizeBin masterSizeBin;
            foreach (HdrColorBin hcb in ap.BulkColors.Values)
            {
                if (BulkColorIsOnHeader(hcb.ColorCodeRID))  
                {
                    masterColorBin = GetHdrColorBin(hcb.ColorCodeRID); 
                    SetColorUnitsToAllocate(  
                        masterColorBin,
                        GetColorUnitsToAllocate(masterColorBin) + hcb.ColorUnitsToAllocate, 
                        false,
                        false);
                }
                else
                {
                    AddBulkColor(hcb.ColorCodeRID, hcb.ColorUnitsToAllocate, hcb.ColorSequence); 
                    masterColorBin = GetHdrColorBin(hcb.ColorCodeRID); 
                }
                SortedList colorSizeSL = new SortedList();
                foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                {
                    colorSizeSL.Add(hsb.SizeSequence, hsb);
                }
                foreach (HdrSizeBin hsb in colorSizeSL.Values)
                {
                    if (masterColorBin.SizeIsInColor(hsb.SizeCodeRID))  
                    {
                        masterSizeBin = masterColorBin.GetSizeBin(hsb.SizeCodeRID);  
                        SetSizeUnitsToAllocate(  
                            masterSizeBin,
                            GetSizeUnitsToAllocate(masterSizeBin) + hsb.SizeUnitsToAllocate,
                            false,
                            false);
                    }
                    else
                    {
                        AddBulkSizeToColor(masterColorBin, hsb.SizeCodeRID, hsb.SizeUnitsToAllocate, hsb.SizeSequence);
                    }
                }
            }
            return true;
        }

        internal struct BulkColorSizeInfo
        {
            public int ColorCodeRID;
            public int SizeCodeRID;

        }
        public bool RebuildMaster(ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {
                if (!StoresLoaded)
                {
                    LoadStores();
                }

                //Index_RID aStore = StoreIndex(4);
                //DebugStoreValues(aStore);

                RebuildingMasterHeader = true;
                if (StyleIntransitUpdated)
                {
                    Action(eAllocationMethodType.BackoutStyleIntransit, new GeneralComponent(eGeneralComponentType.Total), 0.0d, Include.NoRID, true);
                }

                // Clear bulk units
                if (BulkColors != null && BulkColors.Count > 0)
                {
                    foreach (HdrColorBin hcb in BulkColors.Values)
                    {
                        SetColorUnitsToAllocate(hcb.ColorCodeRID, 0);
                        if (hcb.ColorSizes != null)
                        {
                            foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                            {
                                SetSizeUnitsToAllocate(hcb.ColorCodeRID, hsb.SizeCodeRID, 0);
                            }
                        }
                    }
                }
                // Clear pack units and associated packs
                if (Packs != null && Packs.Count > 0)
                {
                    foreach (PackHdr ph in Packs.Values)
                    {
                        ph.AssociatedPackRIDs.Clear();
                        ph.SetPacksToAllocate(0);
                        //foreach (PackColorSize packColor in ph.PackColors.Values)
                        //{
                        //    packColor.SetColorUnitsInPack(0);
                        //    foreach (PackSizeBin packSize in packColor.ColorSizes.Values)
                        //    {
                        //        packSize.SetContentUnits(0);
                        //    }
                        //}
                    }
                }
                base.BulkUnitsToAllocate = 0;
                base.DetailTypeUnitsToAllocate = 0;
                base.GenericUnitsToAllocate = 0;
                base.TotalUnitsToAllocate = 0;
                // retotal values
                foreach (AllocationProfile ap in GetSubordinates(aApplicationTransaction, null))
                {
                    AddSubordinateHeader(ap);
                }

                List<int> bulkColorsToBeDeleted = new List<int>();
                List<BulkColorSizeInfo> bulkColorSizesToBeDeleted = new List<BulkColorSizeInfo>();
                
                List<string> PacksToBeDeleted = new List<string>();

                // Determine if any component values are zero and component needs removed
                // Remove Bulk Colors
                if (BulkColors != null)
                {
                    BulkColorSizeInfo bcsi;
                    foreach (HdrColorBin hcb in BulkColors.Values)
                    {
                        if (hcb.ColorUnitsToAllocate == 0)
                        {
                            //RemoveBulkColor(hcb.ColorCodeRID);
                            bulkColorsToBeDeleted.Add(hcb.ColorCodeRID);
                        }
                        else if (hcb.ColorSizes != null)
                        {
                            foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                            {
                                if (hsb.SizeUnitsToAllocate == 0)
                                {
                                    //RemoveBulkColorSize(hcb.ColorCodeRID, hsb.SizeCodeRID);
                                    bcsi = new BulkColorSizeInfo();
                                    bcsi.ColorCodeRID = hcb.ColorCodeRID;
                                    bcsi.SizeCodeRID = hsb.SizeCodeRID;
                                    bulkColorSizesToBeDeleted.Add(bcsi);
                                }
                            }
                        }
                    }
                }
                // Remove Packs
                if (Packs != null && Packs.Count > 0)
                {
                    foreach (PackHdr ph in Packs.Values)
                    {
                        if (ph.PacksToAllocate == 0)
                        {
                            //RemovePack(ph.PackName);
                            PacksToBeDeleted.Add(ph.PackName);
                        }
                        //else
                        //{
                        //    foreach (PackColorSize packColor in ph.PackColors.Values)
                        //    {
                        //        if (packColor.ColorUnitsInPack == 0)
                        //        {
                        //            RemovePackColor(ph.PackName, packColor.ColorCodeRID);
                        //        }
                        //        else
                        //        {
                        //            foreach (PackSizeBin packSize in packColor.ColorSizes.Values)
                        //            {
                        //                if (packSize.ContentUnits == 0)
                        //                {
                        //                    RemovePackColorSize(ph.PackName, packColor.ColorCodeRID, packSize.ContentCodeRID);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }


                // Remove Bulk Colors
                if (bulkColorsToBeDeleted.Count > 0)
                {
                    foreach (int colorCodeRID in bulkColorsToBeDeleted)
                    {
                        RemoveBulkColor(colorCodeRID);
                    }
                }
                // Remove Bulk Color Sizes
                if (bulkColorSizesToBeDeleted.Count > 0)
                {
                    foreach (BulkColorSizeInfo bcsi in bulkColorSizesToBeDeleted)
                    {
                        RemoveBulkColorSize(bcsi.ColorCodeRID, bcsi.SizeCodeRID);
                    }
                }
                // Remove Packs
                if (PacksToBeDeleted.Count > 0)
                {
                    foreach (string packName in PacksToBeDeleted)
                    {
                        RemovePack(packName);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                aApplicationTransaction.SAB.ApplicationServerSession.Audit.Log_Exception(ex);
                throw;
            }
            finally
            {
                RebuildingMasterHeader = false;
            }
        }

        private void DebugStoreValues(Index_RID aStore)
        {
            int value;
            foreach (eAllocationSummaryNode allocationSummaryNode in Enum.GetValues(typeof(eAllocationSummaryNode)))
            {
                value = GetStoreQtyAllocated(allocationSummaryNode, aStore);
                Debug.WriteLine("Store RID:" + aStore.RID + ";" + allocationSummaryNode + "=" + value);
            }
        }

        public AllocationProfileList GetSubordinates(ApplicationSessionTransaction aApplicationTransaction, AllocationProfile currentHeader)
        {
            if (_subordinates == null)
            {
                if (currentHeader == null)
                {
                    currentHeader = this;
                }

                _subordinates = aApplicationTransaction.GetSubordinates(currentHeader);

                foreach (AllocationProfile subordinate in _subordinates)
                {
                    subordinate.LoadStores(false);
                }
            }

#if (DEBUG)
            foreach (AllocationProfile ap in _subordinates)
            {
                Debug.WriteLine(Key + ":" + HeaderID + ":" + ap.Key + ":" + ap.HeaderID + ":" + ap.InstanceID);
                aApplicationTransaction.CheckInstance(ap.Key, ap.InstanceID);
            }
#endif

            return _subordinates;
        }

        internal bool RemoveSubordinate(int aHeaderRID)
        {
            if (_subordinates != null
                && _subordinates.Contains(aHeaderRID))
            {
                AllocationProfile subordinate = (AllocationProfile)_subordinates.FindKey(aHeaderRID);
                if (subordinate != null)
                {
                    _subordinates.Remove(subordinate);
                }
            }
            if (SubordinateRIDs.Contains(aHeaderRID))
            {
                SubordinateRIDs.Remove(aHeaderRID);
            }
            return true;
        }

        internal bool RemoveAssociatedPackRID(int aPackRID)
        {
            foreach (PackHdr ph in Packs.Values)
            {
                if (ph.AssociatedPackRIDs.Contains(aPackRID))
                {
                    ph.AssociatedPackRIDs.Remove(aPackRID);
                }
            }
            return true;
        }

        #region DC Fulfillment
        /// <summary>
        /// Takes the master header defined in the class and splits the allocated values between
        /// all of the subordinate header's belonging to it. 
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="splitOption">Identifies the method to use to split the master allocation</param>
        /// <param name="bApplyMinimumsInd">Identifies if minimums are to be applied</param>
        /// <param name="SplitByOption">Identifes the order DC Fulfillment is to be applied</param>
        /// <param name="SplitByReserve">Identified when the reserve store allocation is to be applied</param>
        /// <param name="ApplyMinimums">Identifies when minimums are to be applied</param>
        /// <param name="WithinDC">Identifies option to use to allocate headers within the DC</param>
        /// <param name="subordinates">List of headers that belong to the master</param>
        /// <param name="DCProcessingOrder">List containing the order to process DCs</param>
        /// <param name="DCStoreOrder">List containing the order to process stores by DC</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="DCFulfillmentMethod">The calling method</param>
        /// <remarks>
        /// If the header defined within the class is NOT a master header, it returns false.
        /// </remarks>
        /// <returns>A flag indicating if the process was successful</returns>
        public bool ProcessDCFulfillment(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            eDCFulfillmentSplitOption splitOption,
            bool bApplyMinimumsInd,
            eDCFulfillmentSplitByOption SplitByOption,
            eDCFulfillmentReserve SplitByReserve,
            eDCFulfillmentMinimums ApplyMinimums,
            eDCFulfillmentWithinDC WithinDC,
            AllocationProfileList subordinates,
            List<string> DCProcessingOrder,
            Dictionary<string, List<int>> DCStoreOrder,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            DCFulfillmentMethod DCFulfillmentMethod
            )
        {
            DCFulfillmentMonitor DCFulfillmentMonitor = null;
            string message;
            try
            {
                bool successful = false;
                _manuallyAllocatedItems = new List<ItemManuallyAllocated>();
                MasterHeaderProfile masterHeader = this;
                List<AllocationProfile> subordinateHeaders = new List<AllocationProfile>();
                foreach (AllocationProfile ap in subordinates)
                {
                    subordinateHeaders.Add(ap);
                }

                if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                {
                    string fileName = "DCFulfillment_";
                    try
                    {
                        DCFulfillmentMonitor = new DCFulfillmentMonitor(DCFulfillmentMethod, aSAB, fileName, aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorDirectory, aSAB.ClientServerSession.UserRID, DCFulfillmentMethod.Name, masterHeader.HeaderID);
                    }
                    catch (Exception ex)
                    {
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, ex.ToString(), this.GetType().Name);
                        aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive = false;
                        string errMsg = String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_pl_ForecastMonitoringProblem));

                        System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                            errMsg,
                            "Problem Creating DC Fulfillment Monitor Log File",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                }

                if (masterHeader.IsMasterHeader)
                {
                    ProfileList storeList = aApplicationTransaction.GetMasterProfileList(eProfileType.Store);

                    // Force stores to be loaded
                    LoadStores();
                    foreach (AllocationProfile ap in subordinateHeaders)
                    {
                        ap.LoadStores();
                        ap.DCFulfillmentInProgress = true;
                        ap.ClearHeaderVSWTotals();  // TT#5711 - JSmith - Str Alloc U and VSW Alloc U Incorrect after Processing DC fulfillment 
                    }

                    if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                    {
                        DCFulfillmentMonitor.WriteOptions(aSAB, aApplicationTransaction, DCFulfillmentMethod, splitOption, bApplyMinimumsInd, SplitByOption, WithinDC, SplitByReserve, ApplyMinimums, masterHeader, subordinateHeaders, DCProcessingOrder, DCStoreOrder, DCHeaderOrder, storeList);
                    }

                    UpdateSubordinateProperties(subordinateHeaders, eDCFulfillmentPropertyProcessing.PreSplit);

                    if (SplitByReserve == eDCFulfillmentReserve.ReservePreSplit)
                    {
                        message =  "*********************************" + Environment.NewLine;
                        message += "*  Processing PreSplit Reserve  *" + Environment.NewLine;
                        message += "*********************************" + Environment.NewLine;
                        // Begin TT#5712 - JSmith - Dc Fulfillment Error with Pack Only headers
                        //DCFulfillmentMonitor.WriteLine(message);
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        // End TT#5712 - JSmith - Dc Fulfillment Error with Pack Only headers
                        SplitReserveStore(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, subordinateHeaders, DCFulfillmentMonitor);
                    }

                    if (splitOption == eDCFulfillmentSplitOption.Proportional)
                    {
                        SplitProportional(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, DCProcessingOrder, DCStoreOrder, DCHeaderOrder, subordinateHeaders, DCFulfillmentMonitor);
                    }
                    else
                    {
                        SplitDCFulfillment(aSAB, aApplicationTransaction, bApplyMinimumsInd, SplitByOption, SplitByReserve, ApplyMinimums, WithinDC, masterHeader, subordinateHeaders, storeList, DCProcessingOrder, DCStoreOrder, DCHeaderOrder, DCFulfillmentMonitor);
                    }

                    if (SplitByReserve == eDCFulfillmentReserve.ReservePostSplit)
                    {
                        message =  "**********************************" + Environment.NewLine;
                        message += "*  Processing PostSplit Reserve  *" + Environment.NewLine;
                        message += "**********************************" + Environment.NewLine;
                        // Begin TT#5712 - JSmith - Dc Fulfillment Error with Pack Only headers
                        //DCFulfillmentMonitor.WriteLine(message);
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        // End TT#5712 - JSmith - Dc Fulfillment Error with Pack Only headers
                        SplitReserveStore(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, subordinateHeaders, DCFulfillmentMonitor);
                    }

                    UpdateSubordinateProperties(subordinateHeaders, eDCFulfillmentPropertyProcessing.PostSplit);

                    successful = SetFulfillmentFlag(subordinateHeaders);

                    if (successful)
                    {
                        try
                        {
                            HeaderDataRecord.OpenUpdateConnection();
                            foreach (AllocationProfile ap in subordinateHeaders)
                            {
                                ap.HeaderDataRecord = HeaderDataRecord;
                            }
                            successful = CommitChanges(subordinateHeaders: subordinateHeaders, writeSubordinates: true, writeMaster: true);

                            if (StyleIntransitUpdated
                                            || BulkColorIntransitUpdated
                                            || BulkSizeIntransitUpdated)
                            {
                                if (successful)
                                {
                                    if (StyleIntransitUpdated)
                                    {
                                        ProcessingMasterHeader = false;
                                        VerifyAction = false;
                                        Action(eAllocationMethodType.BackoutStyleIntransit, new GeneralComponent(eGeneralComponentType.Total), Include.DefaultBalanceTolerancePercent, Include.AllStoreFilterRID, false);
                                        ProcessingMasterHeader = true;
                                        aApplicationTransaction.ClearVSWCache();
                                        aApplicationTransaction.ResetIntransitReader();
                                        aApplicationTransaction.ResetIMOReader();

                                        subordinateHeaders[0].Build_Item_VSW_Values(forceRebuildVSW: true);

                                        foreach (AllocationProfile ap in subordinateHeaders)
                                        {
                                            ap.ProcessingMasterHeader = false;
                                            VerifyAction = false;
                                            ap.Action(eAllocationMethodType.ChargeIntransit, new GeneralComponent(eGeneralComponentType.Total), Include.DefaultBalanceTolerancePercent, Include.AllStoreFilterRID, false);
                                            ap.ProcessingMasterHeader = true;
                                        }
                                        // Building VSW for any subordinate will rebuild all subordinates
                                        subordinateHeaders[0].Build_Item_VSW_Values(forceRebuildVSW: true);
                                    }
                                    else if (BulkSizeIntransitUpdated)
                                    {
                                        ProcessingMasterHeader = false;
                                        VerifyAction = false;
                                        Action(eAllocationMethodType.BackoutSizeIntransit, new GeneralComponent(eGeneralComponentType.Total), Include.DefaultBalanceTolerancePercent, Include.AllStoreFilterRID, false);
                                        ProcessingMasterHeader = true;
                                        aApplicationTransaction.ClearVSWCache();
                                        aApplicationTransaction.ResetIntransitReader();
                                        aApplicationTransaction.ResetIMOReader();

                                        foreach (AllocationProfile ap in subordinateHeaders)
                                        {
                                            ap.ProcessingMasterHeader = false;
                                            VerifyAction = false;
                                            ap.Action(eAllocationMethodType.ChargeSizeIntransit, new GeneralComponent(eGeneralComponentType.Total), Include.DefaultBalanceTolerancePercent, Include.AllStoreFilterRID, false);
                                            ap.ProcessingMasterHeader = true;
                                        }
                                        // Building VSW for any subordinate will rebuild all subordinates
                                        subordinateHeaders[0].Build_Item_VSW_Values(forceRebuildVSW: true);
                                    }

                                }
                                HeaderDataRecord.OpenUpdateConnection();
                                successful = CommitChanges(subordinateHeaders: subordinateHeaders, writeSubordinates: true, writeMaster: false);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (_manuallyAllocatedItems.Count > 0)
                            {
                                ReApplyItemManuallyAllocated();
                                subordinateHeaders[0].Build_Item_VSW_Values(forceRebuildVSW: true);
                                successful = CommitChanges(subordinateHeaders: subordinateHeaders, writeSubordinates: true, writeMaster: false);
                            }
                            // Begin TT#5753 - JSmith - VSW Units allocated after DC Fulfillment
                            // Copy VSW criteria from the master to the subordinates
                            if (!HeaderDataRecord.ConnectionIsOpen)
                            {
                                HeaderDataRecord.OpenUpdateConnection();
                            }
                            SetStoreImoCriteria(subordinateHeaders);
                            successful = CommitChanges(subordinateHeaders: subordinateHeaders, writeSubordinates: true, writeMaster: false);
                            // End TT#5753 - JSmith - VSW Units allocated after DC Fulfillment
                            if (HeaderDataRecord.ConnectionIsOpen)
                            {
                                HeaderDataRecord.CloseUpdateConnection();
                            }
                        }
                    }

                    aSAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Information,
                        eMIDTextCode.msg_al_DCFulfillmentComplete,
                        SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_DCFulfillmentComplete, false) + " Header [" + masterHeader.HeaderID + "] ",
                        this.GetType().Name);
                }

                return successful;
            }
            catch (Exception)
            {  
                throw;
            }
            finally
            {
                foreach (AllocationProfile ap in subordinates)
                {
                    ap.DCFulfillmentInProgress = false;
                }
                if (DCFulfillmentMonitor != null)
                {
                    DCFulfillmentMonitor.CloseLogFile();
                }
            }
        }

        /// <summary>
        /// Copies necessary properties from the master header to each subordinate headers
        /// </summary>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentPropertyProcessing">Identifies if pre-split or post-split</param>
        /// <returns></returns>
        private bool UpdateSubordinateProperties(List<AllocationProfile> subordinateHeaders, eDCFulfillmentPropertyProcessing DCFulfillmentPropertyProcessing)
        {
            if (DCFulfillmentPropertyProcessing == eDCFulfillmentPropertyProcessing.PreSplit)
            {
                UpdateSubordinateShipDay(subordinateHeaders);
                UpdateSubordinateNeedDay(subordinateHeaders);
            }
            else
            {
                //UpdateSubordinateNeedDay(subordinateHeaders);
                UpdateSubordinateOTSForecastNode(subordinateHeaders);
                UpdateSubordinateOnhandNode(subordinateHeaders);
                UpdateSubordinateInventoryBasisNodes(subordinateHeaders);
                UpdateSubordinateSizeProperties(subordinateHeaders);
            }

            return true;
        }

        private bool UpdateSubordinateShipDay(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile ap in subordinateHeaders)
            {
                ap.SetShipDay(ShipToDay, false);
                foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                {
                    ap.SetStoreShipDay(storeIdxRID, GetStoreShipDay(storeIdxRID));
                }
            }

            return true;
        }

        private bool UpdateSubordinateNeedDay(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile ap in subordinateHeaders)
            {
                ap.LastNeedDay = LastNeedDay;
                
                DateTime needDay, totalNeedDay, genericNeedDay, detailNeedDay, bulkNeedDay;
                foreach (Index_RID storeIdxRID in AppSessionTransaction.StoreIndexRIDArray())
                {
                    // Only update Need Days if component day is not the same as next higher level
                    needDay = ap.LastNeedDay;
                    totalNeedDay = GetStoreLastNeedDay(eAllocationSummaryNode.Total, storeIdxRID);
                    if (totalNeedDay != Include.UndefinedDate
                         && totalNeedDay != needDay)
                    {
                        ap.SetStoreLastNeedDay(eAllocationSummaryNode.Total, storeIdxRID, totalNeedDay);
                        needDay = totalNeedDay;
                    }
                    genericNeedDay = GetStoreLastNeedDay(eAllocationSummaryNode.GenericType, storeIdxRID);
                    if (genericNeedDay != Include.UndefinedDate
                         && genericNeedDay != needDay)
                    {
                        ap.SetStoreLastNeedDay(eAllocationSummaryNode.GenericType, storeIdxRID, genericNeedDay);
                    }
                    detailNeedDay = GetStoreLastNeedDay(eAllocationSummaryNode.DetailType, storeIdxRID);
                    if (detailNeedDay != Include.UndefinedDate
                         && detailNeedDay != needDay)
                    {
                        ap.SetStoreLastNeedDay(eAllocationSummaryNode.DetailType, storeIdxRID, detailNeedDay);
                        needDay = totalNeedDay;
                    }
                    bulkNeedDay = GetStoreLastNeedDay(eAllocationSummaryNode.Bulk, storeIdxRID);
                    if (bulkNeedDay != Include.UndefinedDate
                         && bulkNeedDay != needDay)
                    {
                        ap.SetStoreLastNeedDay(eAllocationSummaryNode.Bulk, storeIdxRID, bulkNeedDay);
                    }
                }
            }

            return true;
        }

        private bool UpdateSubordinateOTSForecastNode(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile ap in subordinateHeaders)
            {
                if (PlanHnRID != Include.DefaultPlanHnRID)
                {
                    ap.PlanHnRID = PlanHnRID;
                } 
            }

            return true;
        }

        private bool UpdateSubordinateOnhandNode(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile ap in subordinateHeaders)
            {
                if (OnHandHnRID != Include.DefaultOnHandHnRID)
                {
                    ap.OnHandHnRID = OnHandHnRID;
                }
            }

            return true;
        }

        private bool UpdateSubordinateInventoryBasisNodes(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile ap in subordinateHeaders)
            {
                if (GradeInventoryBasisHnRID > 0)
                {
                    ap.SetGradeInventoryBasisHnRID(GradeInventoryBasisHnRID);
                }
            }

            return true;
        }

        private bool UpdateSubordinateSizeProperties(List<AllocationProfile> subordinateHeaders)
        {
            HdrColorBin hcb, hbcSubordinate;
            HdrSizeBin hsb, hsbSubordinate;
            if (BulkColors.Count > 0)  // Does the master contain bulk colors
            {
                int[] bulkColorCodeRIDs = GetBulkColorCodeRIDs();  // Get bulk colors on master
                foreach (int colorKey in bulkColorCodeRIDs)
                {
                    hcb = GetHdrColorBin(colorKey);
                    int[] sizeKeys = GetBulkColorSizeCodeRIDs(colorKey);  // Get sizes from bulk color on master
                    foreach (int sizeKey in sizeKeys)
                    {
                        hsb = hcb.GetSizeBin(sizeKey);
                        foreach (AllocationProfile subordinate in subordinateHeaders)
                        {
                            if (subordinate.BulkColorIsOnHeader(colorKey))  // make sure color is on subordinate
                            {
                                hbcSubordinate = subordinate.GetHdrColorBin(colorKey);
                                if (subordinate.SizeIsOnBulkColor(colorKey, sizeKey))  // make sure size is on color
                                {
                                    hsbSubordinate = hcb.GetSizeBin(sizeKey);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        // Begin TT#5753 - JSmith - VSW Units allocated after DC Fulfillment
        private bool SetStoreImoCriteria(List<AllocationProfile> subordinateHeaders)
        {
            IMOProfileList IMOProfileList = GetStoreImoCriteria();

            foreach (AllocationProfile subordinate in subordinateHeaders)
            {
                subordinate.SetStoreImoCriteria(IMOProfileList, true);
            }

            return true;
        }
        // End TT#5753 - JSmith - VSW Units allocated after DC Fulfillment

        private bool SetFulfillmentFlag(List<AllocationProfile> subordinateHeaders)
        {
            foreach (AllocationProfile subordinate in subordinateHeaders)
            {
                subordinate.DCFulfillmentProcessed = true;
            }
            DCFulfillmentProcessed = true;

            return true;
        }

        private bool CommitChanges(List<AllocationProfile> subordinateHeaders, bool writeSubordinates, bool writeMaster)
        {
            bool actionSuccess;
            if (writeSubordinates)
            {
                foreach (AllocationProfile subordinate in subordinateHeaders)
                {
                    actionSuccess = subordinate.WriteHeader();
                    if (!actionSuccess)
                    {
                        return false;
                    }
                }
            }
            if (writeMaster)
            {
                actionSuccess = WriteHeader();
            }
            HeaderDataRecord.CommitData();

            return true;
        }

        /// <summary>
        /// Split the allocation of the reserve store in the master to the reserve store of the subordinates
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storeList">List of stores to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitReserveStore(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storeList,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing Reserve Store");

                ProfileList reserveStore = new ProfileList(eProfileType.Store);
                reserveStore.Add(storeList.FindKey(AppSessionTransaction.ReserveStore.RID));

                if (masterHeader.BulkColors.Count == 0
                    && masterHeader.NonGenericPackCount == 0
                    && masterHeader.GenericPackCount == 0)
                {
                    SplitTotalAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, reserveStore, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.ReserveStore);
                }
                else
                {
                    // PACKS
                    if (masterHeader.NonGenericPackCount > 0
                        || masterHeader.GenericPackCount > 0)
                    {
                        SplitPackAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, reserveStore, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.ReserveStore);
                    }

                    // BULK
                    if (masterHeader.BulkColors.Count > 0)
                    {
                        if (masterHeader.AtLeastOneSizeAllocated &&
                            masterHeader.BulkSizeAllocationInBalance)
                        {
                            SplitBulkSizeAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, reserveStore, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.ReserveStore);
                        }
                        else
                        {
                            SplitBulkAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, reserveStore, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.ReserveStore);
                        }
                    }
                }

            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Split the master header allocation proportionally to the subordinates
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="DCProcessingOrder">List containing the order to process DCs</param>
        /// <param name="DCStoreOrder">List containing the order to process stores by DC</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitProportional(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storeList,
            List<string> DCProcessingOrder,
            Dictionary<string, List<int>> DCStoreOrder,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing Proportional");

                // Use the first list in each order to sequence processing
                ProfileList storeOrder = new ProfileList(eProfileType.Store);
                var storeOrderList = DCStoreOrder.First();
                List<int> stores = storeOrderList.Value;
                foreach (int store_RID in stores)
                {
                    StoreProfile sp = (StoreProfile)storeList.FindKey(store_RID);
                    if (store_RID == AppSessionTransaction.ReserveStore.RID)
                    {
                        continue;
                    }

                    int storeAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, sp.Key);
                    if (storeAllocated == 0)
                    {
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + sp.Text + " was not allocated any units and will be skipped");
                        continue;
                    }
                    if (sp.SimilarStoreModel)
                    {
                        string message = "*** WARNING *** Similar Store Model Store " + sp.Text + " was allocated units but will not participate.  This will leave the subordinates out of balance.";
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
                        continue;
                    }

                    storeOrder.Add(sp);
                }

                List<AllocationProfile> headerOrder = new List<AllocationProfile>();
                List<AllocationProfile> headers;
                foreach (string distributionCenter in DCProcessingOrder)
                {
                    //foreach (KeyValuePair<string, List<AllocationProfile>> keyPair in DCHeaderOrder)
                    if (DCHeaderOrder.TryGetValue(distributionCenter, out headers))
                    {
                        //headers = keyPair.Value;
                        foreach (AllocationProfile ap in headers)
                        {
                            headerOrder.Add(ap);
                        }
                    }
                }

                // Process components
                if (masterHeader.BulkColors.Count == 0
                    && masterHeader.NonGenericPackCount == 0
                    && masterHeader.GenericPackCount == 0)
                {
                    SplitTotalAllocation(aSAB, aApplicationTransaction, masterHeader, headerOrder, storeOrder, subordinateHeaders, DCFulfillmentMonitor);
                }
                else
                {
                    // PACKS
                    if (masterHeader.NonGenericPackCount > 0
                        || masterHeader.GenericPackCount > 0)
                    {
                        SplitPackAllocation(aSAB, aApplicationTransaction, masterHeader, headerOrder, storeOrder, subordinateHeaders, DCFulfillmentMonitor);
                    }

                    // BULK
                    if (masterHeader.BulkColors.Count > 0)
                    {
                        if (masterHeader.AtLeastOneSizeAllocated &&
                            masterHeader.BulkSizeAllocationInBalance)
                        {
                            SplitBulkSizeAllocation(aSAB, aApplicationTransaction, masterHeader, headerOrder, storeOrder, subordinateHeaders, DCFulfillmentMonitor);
                        }
                        else
                        {
                            SplitBulkAllocation(aSAB, aApplicationTransaction, masterHeader, headerOrder, storeOrder, subordinateHeaders, DCFulfillmentMonitor);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Processes DC Fulfillment methodology to split the master header allocation to the subordinates
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="bApplyMinimumsInd">Identifies if minimums are to be applied</param>
        /// <param name="SplitByOption">Identifes the order DC Fulfillment is to be applied</param>
        /// <param name="SplitByReserve">Identified when the reserve store allocation is to be applied</param>
        /// <param name="ApplyMinimums">Identifies when minimums are to be applied</param>
        /// <param name="WithinDC">Identifies option to use to allocate headers within the DC</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="DCProcessingOrder">List containing the order to process DCs</param>
        /// <param name="DCStoreOrder">List containing the order to process stores by DC</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitDCFulfillment(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            bool bApplyMinimumsInd,
            eDCFulfillmentSplitByOption SplitByOption,
            eDCFulfillmentReserve SplitByReserve,
            eDCFulfillmentMinimums ApplyMinimums,
            eDCFulfillmentWithinDC WithinDC,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<string> DCProcessingOrder,
            Dictionary<string, List<int>> DCStoreOrder,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                string message;
                //WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing by DCFulfillment");
                message =  "*********************************" + Environment.NewLine;
                message += "*  Processing By DCFulfillment  *" + Environment.NewLine;
                message += "*********************************" + Environment.NewLine;
                WriteToMonitor(aSAB, DCFulfillmentMonitor, message);

                // build stores list in order by DC/store priority.  Each store has list of the order to process DCs for the store.
                List<string> storeDCOrder = new List<string>();
                //Dictionary<int, List<string>> storesToProcess = new Dictionary<int,List<string>>();
                List<StoreToProcess> storesToProcess = new List<StoreToProcess>();
                List<int> stores;
                StoreProfile store;
                Dictionary<int, string> zeroQuantityStores = new Dictionary<int, string>();

                for (int i = 0; i < storeList.Count; i++)
                {
                    //string distributionCenter;
                    foreach (string distributionCenter in DCProcessingOrder)
                    {
                        //foreach (KeyValuePair<string, List<int>> keyPair in DCStoreOrder)
                        if (DCStoreOrder.TryGetValue(distributionCenter, out stores))
                        {
                            //distributionCenter = keyPair.Key;
                            //stores = keyPair.Value;

                            // do not include special stores
                            store = (StoreProfile)storeList.FindKey(stores[i]);
                            
                            if (stores[i] == AppSessionTransaction.ReserveStore.RID)
                            {
                                continue;
                            }

                            if (zeroQuantityStores.ContainsKey(store.Key))
                            {
                                continue;
                            }
                            else
                            {
                                int storeAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, store.Key);
                                if (storeAllocated == 0)
                                {
                                    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + store.Text + " was not allocated any units and will be skipped");
                                    zeroQuantityStores.Add(store.Key, null);
                                    continue;
                                }

                                if (store.SimilarStoreModel)
                                {
                                    message = "*** WARNING *** Similar Store Model Store " + store.Text + " was allocated units but will not participate.  This will leave the subordinates out of balance.";
                                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                                    aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
                                    continue;
                                }
                            }

                            storeDCOrder = StoresToProcessGetValue(storesToProcess, stores[i]);
                            //if (!storesToProcess.TryGetValue(stores[i], out storeDCOrder))
                            if (storeDCOrder == null)
                            {
                                storeDCOrder = new List<string>();
                                storesToProcess.Add(new StoreToProcess(stores[i], storeDCOrder));
                            }
                            storeDCOrder.Add(distributionCenter);
                        }
                    }
                }

                int totalDCs = 0;

                // get count of most DCs in any store
                //foreach (List<string> storeDCs in storesToProcess.Values)
                foreach (StoreToProcess stp in storesToProcess)
                {
                    if (stp.StoreDCOrder.Count > totalDCs)
                    {
                        totalDCs = stp.StoreDCOrder.Count;
                    }
                }

                if (bApplyMinimumsInd
                    & ApplyMinimums == eDCFulfillmentMinimums.ApplyFirst)
                {
                    message = Environment.NewLine;
                    message += "*************************" + Environment.NewLine;
                    message += "*  Processing Minimums  *" + Environment.NewLine;
                    message += "*************************";
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    if (SplitByOption == eDCFulfillmentSplitByOption.SplitByStore)
                    {
                        ApplyMinimumAllocationFirstStorePriority(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                    }
                    else
                    {
                        ApplyMinimumAllocationFirstDCPriority(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                    }
                }

                List<int> processedStores = new List<int>();
                if (SplitByOption == eDCFulfillmentSplitByOption.SplitByStore)
                {
                    message = Environment.NewLine;
                    message += "*******************************" + Environment.NewLine;
                    message += "*  Processing Split By Store  *" + Environment.NewLine;
                    message += "*******************************" + Environment.NewLine;
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    SplitDCFulfillmentStorePriority(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, subordinateHeaders, storeList, processedStores, storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                }
                else
                {
                    message = Environment.NewLine;
                    message += "****************************" + Environment.NewLine;
                    message += "*  Processing Split By DC  *" + Environment.NewLine;
                    message += "****************************" + Environment.NewLine;
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    SplitDCFulfillmentDCPriority(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, subordinateHeaders, storeList, processedStores, storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                }

                if (processedStores.Count < storesToProcess.Count)
                {
                    message = Environment.NewLine;
                    message += "*************************************************" + Environment.NewLine;
                    message += "*  Processing Split Remaining Stores Across DC  *" + Environment.NewLine;
                    message += "*************************************************" + Environment.NewLine;
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    SplitDCFulfillmentAcrossDCs(aSAB, aApplicationTransaction, masterHeader, subordinateHeaders, storeList, processedStores, storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                }
            }
            catch
            {
                throw;
            }

        }

        private List<string> StoresToProcessGetValue(List<StoreToProcess> StoresToProcess, int aStoreRID)
        {
            foreach (StoreToProcess stp in StoresToProcess)
            {
                if (stp.StoreRID == aStoreRID)
                {
                    return stp.StoreDCOrder;
                }
            }

            return null;
        }

        /// <summary>
        /// Processes all stores for their first DC before going to the next DC
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="bApplyMinimumsInd">Identifies if minimums are to be applied</param>
        /// <param name="ApplyMinimums">Identifies when minimums are to be applied</param>
        /// <param name="WithinDC">Identifies how to allocate headers within the DC</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="processedStores">List of store keys that have been processed</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitDCFulfillmentStorePriority(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            bool bApplyMinimumsInd,
            eDCFulfillmentMinimums ApplyMinimums,
            eDCFulfillmentWithinDC WithinDC,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<int> processedStores,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing DCFulfillment with Store Priority");

                StoreProfile store;
                int storeRID;
                int DCIndex = 0;
                List<string> storeDCOrder;
                
                bool processingStores = true;
                bool processedStore = false;
                List<AllocationProfile> headers;
                List<AllocationProfile> headerToProcess;
                AllocationProfileList headerOrder = new AllocationProfileList(eProfileType.AllocationHeader);

                while (processingStores)
                {
                    foreach (StoreToProcess stp in storesToProcess)
                    {
                        storeRID = stp.StoreRID;
                        storeDCOrder = stp.StoreDCOrder;
                        processedStore = false;

                        // if store already processed go to next store
                        if (processedStores.Contains(storeRID))
                        {
                            continue;
                        }

                        store = (StoreProfile)storeList.FindKey(storeRID);

                        if (DCIndex < storeDCOrder.Count)
                        {
                            // get next DC for store
                            string distributionCenter = storeDCOrder[DCIndex];
                            if (DCHeaderOrder.ContainsKey(distributionCenter))
                            {
                                headers = DCHeaderOrder[distributionCenter];

                                // Try headers individually if process by fill
                                if (WithinDC == eDCFulfillmentWithinDC.Fill)
                                {
                                    int headersProcessed = 0;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    foreach (AllocationProfile ap in headers)
                                    {
                                        headerToProcess = new List<AllocationProfile>();
                                        headerToProcess.Add(ap);
										// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
										//processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headerToProcess, subordinateHeaders, store, processedStores,
                                        //                                                        storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                                        ++headersProcessed;  
                                        processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headerToProcess, subordinateHeaders, store, processedStores,
                                                                                                storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor, headersProcessed == headers.Count);  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                        // End TT#5746 - JSmith - DC Fulfillment Same DC Issue
										if (processedStore)
                                        {
                                            break;
                                        }
                                    }
                                }

                                // If store not processed by Fill or is processed by proportional, try all headers
                                if (!processedStore)
                                {
                                    // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
									//processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headers, subordinateHeaders, store, processedStores,
                                    //                                                        storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                                    processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headers, subordinateHeaders, store, processedStores,
                                                                                            storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor, true);
							        // End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                }
                            }
                        }
                    }

                    ++DCIndex;  // go to next DC in each store list
                    if (processedStores.Count == storesToProcess.Count
                        || DCIndex == totalDCs)
                    {
                        processingStores = false;
                    }
                }

                if (processedStores.Count != storesToProcess.Count)
                {
                    string message = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentFromPrimaryDC, false);
                    message = message.Replace("{0}", masterHeader.HeaderID);
                    SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Severe,
                        eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentFromPrimaryDC,
                        message,
                        this.GetType().Name);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Processes all DCs for a store before going to the next store
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="processedStores">List of store keys that have been processed</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitDCFulfillmentDCPriority(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            bool bApplyMinimumsInd,
            eDCFulfillmentMinimums ApplyMinimums,
            eDCFulfillmentWithinDC WithinDC,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<int> processedStores,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing DCFulfillment with DC Priority");

                StoreProfile store;
                int storeRID;
                List<string> storeDCOrder;
                bool processingStores = true;
                bool processedStore = false;
                List<AllocationProfile> headers;
                List<AllocationProfile> headerToProcess;
                AllocationProfileList headerOrder = new AllocationProfileList(eProfileType.AllocationHeader);

                while (processingStores)
                {
                    foreach (StoreToProcess stp in storesToProcess)
                    {
                        storeRID = stp.StoreRID;
                        storeDCOrder = stp.StoreDCOrder;
                        processedStore = false;

                        // if store already processed go to next store
                        if (processedStores.Contains(storeRID))
                        {
                            continue;
                        }

                        store = (StoreProfile)storeList.FindKey(storeRID);

                        foreach (string distributionCenter in storeDCOrder)
                        {
                            headers = DCHeaderOrder[distributionCenter];

                            WriteToMonitor(aSAB, DCFulfillmentMonitor, "Determine if headers for DC " + distributionCenter + " can fulfill store " + store.Text);

                            // Try headers individually if process by fill
                            if (WithinDC == eDCFulfillmentWithinDC.Fill)
                            {
                                foreach (AllocationProfile ap in headers)
                                {
                                    headerToProcess = new List<AllocationProfile>();
                                    headerToProcess.Add(ap);
                                    processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headerToProcess, subordinateHeaders, store, processedStores,
                                                                                            storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                                    if (processedStore)
                                    {
                                        break;
                                    }
                                }
                            }

                            // If store not processed by Fill or is processed by proportional, try all headers
                            if (!processedStore)
                            {
                                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                //processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headers, subordinateHeaders, store, processedStores,
                                //                                                        storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor);
                                processedStore = SplitDCFulfillmentHeaderProcessings(aSAB, aApplicationTransaction, bApplyMinimumsInd, ApplyMinimums, WithinDC, masterHeader, headers, subordinateHeaders, store, processedStores,
                                                                                        storesToProcess, DCHeaderOrder, totalDCs, DCFulfillmentMonitor, true);
                                // End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                            }

                            // No need to check next DC if processed store
                            if (processedStore)
                            {
                                break;
                            }
                        }
                    }

                    processingStores = false;
                }

                if (processedStores.Count != storesToProcess.Count)
                {
                    string message = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentFromPrimaryDC, false);
                    message = message.Replace("{0}", masterHeader.HeaderID);
                    SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Severe,
                        eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentFromPrimaryDC,
                        message,
                        this.GetType().Name);
                }

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Processes requested headers for a store before going to the next store
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="store">The profile of the store to process</param>
        /// <param name="processedStores">List of store keys that have been processed</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private bool SplitDCFulfillmentHeaderProcessings(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            bool bApplyMinimumsInd,
            eDCFulfillmentMinimums ApplyMinimums,
            eDCFulfillmentWithinDC WithinDC,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            List<AllocationProfile> subordinateHeaders,
            StoreProfile aStore,
            List<int> processedStores,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor,  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
            bool bProcessingLastHeader = false  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
            )
        {
            bool processedStore = false;
            bool bUnitsToAllocate = false;

            if (HeadersCanFulfillStore(aSAB, aApplicationTransaction, masterHeader, aStore, headersToProcess, subordinateHeaders, ref bUnitsToAllocate, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment))
            {
                if (bUnitsToAllocate)
                {
                    processedStore = SplitDCFulfillmentStoreProcessing(aSAB, aApplicationTransaction, masterHeader, headersToProcess, aStore, subordinateHeaders, DCFulfillmentMonitor);
                    if (processedStore)
                    {
                        processedStores.Add(aStore.Key);
                    }
                }
				// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                else if (bProcessingLastHeader)
                {
                    processedStore = true;
                    processedStores.Add(aStore.Key);
                }
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
            }  // see if can apply minimum
            else if (bApplyMinimumsInd
              & ApplyMinimums == eDCFulfillmentMinimums.ApplyByQty)
            {
                processedStore = ApplyMinimumAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, subordinateHeaders, aStore, DCFulfillmentMonitor);
            }

            return processedStore;
        }

        /// <summary>
        /// Process a store across all DCs
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="processedStores">List of store keys that have been processed</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void SplitDCFulfillmentAcrossDCs(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<int> processedStores,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Processing DCFulfillment to all DCs");

                StoreProfile store;
                int storeRID;
                List<string> storeDCOrder;
                bool processingStores = true;
                bool processedStore = false;
                bool bUnitsToAllocate = false;
                List<AllocationProfile> headersToProcess;
                AllocationProfileList headerOrder = new AllocationProfileList(eProfileType.AllocationHeader);

                while (processingStores)
                {
                    foreach (StoreToProcess stp in storesToProcess)
                    {
                        storeRID = stp.StoreRID;
                        storeDCOrder = stp.StoreDCOrder;
                        processedStore = false;

                        // if store already processed go to next store
                        if (processedStores.Contains(storeRID))
                        {
                            continue;
                        }

                        store = (StoreProfile)storeList.FindKey(storeRID);

                        headersToProcess = new List<AllocationProfile>();
                        foreach (string distributionCenter in storeDCOrder)
                        {
                            List<AllocationProfile> DCheaders = DCHeaderOrder[distributionCenter];
                            foreach (AllocationProfile ap in DCheaders)
                            {
                                headersToProcess.Add(ap);
                            }
                        }

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, "Determine if headers for in all DCs can fulfill store " + store.Text);

                        if (HeadersCanFulfillStore(aSAB, aApplicationTransaction, masterHeader, store, headersToProcess, subordinateHeaders, ref bUnitsToAllocate, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment))
                        {
                            if (bUnitsToAllocate)
                            {
                                processedStore = SplitDCFulfillmentStoreProcessing(aSAB, aApplicationTransaction, masterHeader, headersToProcess, store, subordinateHeaders, DCFulfillmentMonitor);
                                if (processedStore)
                                {
                                    processedStores.Add(store.Key);
                                }
                                else
                                {
                                    bool stop = true;
                                }
                            }
                            //else
                            //{
                            //    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + store.Text + " was not allocated any units and will be skipped");
                            //    processedStores.Add(store.Key);
                            //}
                        }
                        else
                        {
                            bool stop = true;
                        }
                    }

                    processingStores = false;

                    if (processedStores.Count != storesToProcess.Count)
                    {
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, Environment.NewLine + "**** ERROR ENCOUNTERED ****");  // TT#5746 - JSmith - DC Fulfillment Same DC Issue

                        string errMsg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentToAllStores, false);
                        errMsg = errMsg.Replace("{0}", masterHeader.HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Severe,
                            eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentToAllStores,
                            errMsg,
                            this.GetType().Name);

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, Environment.NewLine + errMsg);

                        // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
						foreach (StoreToProcess stp in storesToProcess)
                        {
                            if (!processedStores.Contains(stp.StoreRID))
                            {
                                store = (StoreProfile)storeList.FindKey(stp.StoreRID);
                                WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + store.Text + " (" + stp.StoreRID +  ") not processed");
                            }
                        }
						// End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MasterHeaderUnableToApplyDCFulfillmentToAllStores,
                            errMsg);
                    }

                }

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Process a store allocation with DC Fulfillment
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <returns>A flag indicating if the process was successful</returns>
        private bool SplitDCFulfillmentStoreProcessing(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            StoreProfile aStore,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                List<int> processedStores = new List<int>();

                ProfileList storesToProcess = new ProfileList(eProfileType.Store);
                storesToProcess.Add(aStore);

                // Process components
                if (masterHeader.BulkColors.Count == 0
                    && masterHeader.NonGenericPackCount == 0
                    && masterHeader.GenericPackCount == 0)
                {
                    SplitTotalAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storesToProcess, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment);
                }
                else
                {
                    // PACKS
                    if (masterHeader.NonGenericPackCount > 0
                        || masterHeader.GenericPackCount > 0)
                    {
                        SplitPackAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storesToProcess, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment);
                    }

                    // BULK
                    if (masterHeader.BulkColors.Count > 0)
                    {
                        if (masterHeader.AtLeastOneSizeAllocated &&
                            masterHeader.BulkSizeAllocationInBalance)
                        {
                            SplitBulkSizeAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storesToProcess, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment);
                        }
                        else
                        {
                            SplitBulkAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storesToProcess, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Fulfillment);
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Apply allocation minimums before performing DC Fulfillment
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void ApplyMinimumAllocationFirstStorePriority(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                StoreProfile store;
                int storeRID;
                int DCIndex = 0;
                List<string> storeDCOrder;
                List<int> processedStores = new List<int>();
                bool processingStores = true;
                bool processedStore = false;
                List<AllocationProfile> headers;

                while (processingStores)
                {
                    foreach (StoreToProcess stp in storesToProcess)
                    {
                        storeRID = stp.StoreRID;
                        storeDCOrder = stp.StoreDCOrder;
                        processedStore = false;

                        // if store already processed go to next store
                        if (processedStores.Contains(storeRID))
                        {
                            continue;
                        }

                        store = (StoreProfile)storeList.FindKey(storeRID);

                        if (DCIndex < storeDCOrder.Count)
                        {
                            // get next DC for store
                            string distributionCenter = storeDCOrder[DCIndex];
                            headers = DCHeaderOrder[distributionCenter];
                            processedStore = ApplyMinimumAllocation(aSAB, aApplicationTransaction, masterHeader, headers, subordinateHeaders, store, DCFulfillmentMonitor);
                            if (processedStore)
                            {
                                processedStores.Add(store.Key);
                            }
                        }
                    }

                    ++DCIndex;  // go to next DC in each store list
                    if (processedStores.Count == storesToProcess.Count
                        || DCIndex > totalDCs)
                    {
                        processingStores = false;
                        if (processedStores.Count != storesToProcess.Count)
                        {
                            string errMsg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores, false);
                            errMsg = errMsg.Replace("{0}", masterHeader.HeaderID);
                            SAB.ApplicationServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Severe,
                                eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores,
                                errMsg,
                                this.GetType().Name);

                            WriteToMonitor(aSAB, DCFulfillmentMonitor, Environment.NewLine + errMsg);

                            //throw new MIDException(eErrorLevel.severe,
                            //(int)eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores,
                            //errMsg);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Apply allocation minimums before performing DC Fulfillment
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="storeList">List of active stores profiles</param>
        /// <param name="storesToProcess">List of stores to process ordered based on method criteria</param>
        /// <param name="DCHeaderOrder">List containing the order to process headers by DC</param>
        /// <param name="totalDCs">The highest number of DCs on any store</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void ApplyMinimumAllocationFirstDCPriority(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            ProfileList storeList,
            List<StoreToProcess> storesToProcess,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            int totalDCs,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                StoreProfile store;
                int storeRID;
                List<string> storeDCOrder;
                List<int> processedStores = new List<int>();
                bool processingStores = true;
                bool processedStore = false;
                List<AllocationProfile> headers;

                while (processingStores)
                {
                    foreach (StoreToProcess stp in storesToProcess)
                    {
                        storeRID = stp.StoreRID;
                        storeDCOrder = stp.StoreDCOrder;
                        processedStore = false;

                        // if store already processed go to next store
                        if (processedStores.Contains(storeRID))
                        {
                            continue;
                        }

                        store = (StoreProfile)storeList.FindKey(storeRID);

                        foreach (string distributionCenter in storeDCOrder)
                        {
                            headers = DCHeaderOrder[distributionCenter];
                            processedStore = ApplyMinimumAllocation(aSAB, aApplicationTransaction, masterHeader, headers, subordinateHeaders, store, DCFulfillmentMonitor);
                            if (processedStore)
                            {
                                processedStores.Add(store.Key);
                                break;
                            }
                        }
                    }

                    processingStores = false;
                }

                if (processedStores.Count != storesToProcess.Count)
                {
                    string errMsg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores, false);
                    errMsg = errMsg.Replace("{0}", masterHeader.HeaderID);
                    SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Severe,
                        eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores,
                        errMsg,
                        this.GetType().Name);

                    WriteToMonitor(aSAB, DCFulfillmentMonitor, Environment.NewLine + errMsg);

                    //throw new MIDException(eErrorLevel.severe,
                    //    (int)eMIDTextCode.msg_al_MasterHeaderUnableToApplyMinimumToAllStores,
                    //    errMsg);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Apply allocation minimums before performing DC Fulfillment
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="aStore">List of stores to process ordered based on method criteria</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private bool ApplyMinimumAllocation(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            List<AllocationProfile> subordinateHeaders,
            StoreProfile aStore,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                bool processedStore = false;
                // try to process store by each processing method
                foreach (eDCFulfillmentHeaderProcessing DCFulfillmentHeaderProcessing in Enum.GetValues(typeof(eDCFulfillmentHeaderProcessing)))
                {
                    switch (DCFulfillmentHeaderProcessing)
                    {
                        case eDCFulfillmentHeaderProcessing.SingleHeader:
                            processedStore = ApplyMinimumAllocationFirstByHeader(aSAB, aApplicationTransaction, masterHeader, headersToProcess, aStore, subordinateHeaders, DCFulfillmentMonitor);
                            break;
                        case eDCFulfillmentHeaderProcessing.SameHeaderType:
                            processedStore = ApplyMinimumAllocationFirstByHeaderType(aSAB, aApplicationTransaction, masterHeader, headersToProcess, aStore, subordinateHeaders, DCFulfillmentMonitor);
                            break;
                        default:
                            processedStore = ApplyMinimumAllocationFirstByDC(aSAB, aApplicationTransaction, masterHeader, headersToProcess, aStore, subordinateHeaders, DCFulfillmentMonitor);
                            break;
                    }
                    if (processedStore)
                    {
                        break;
                    }
                }

                return processedStore;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Allocate minimums to a single header if possible
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <returns>A flag indicating if the process was successful</returns>
        private bool ApplyMinimumAllocationFirstByHeader(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            StoreProfile aStore,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            bool bUnitsToAllocate = false;
            List<AllocationProfile> headersForProcessing = new List<AllocationProfile>();

            foreach (AllocationProfile ap in headersToProcess)
            {
                headersForProcessing.Clear();
                headersForProcessing.Add(ap);

                if (HeadersCanFulfillStore(aSAB, aApplicationTransaction, masterHeader, aStore, headersForProcessing, subordinateHeaders, ref bUnitsToAllocate, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums))
                {
                    if (bUnitsToAllocate)
                    {
                        ApplyMinimumAllocationFirstProcessing(aSAB, aApplicationTransaction, masterHeader, headersForProcessing, aStore, subordinateHeaders, DCFulfillmentMonitor);
                    }
                    //else
                    //{
                    //    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + aStore.Text + " was not allocated any units and will be skipped");
                    //}

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Apply minimums to all headers of a header type if possible
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <returns>A flag indicating if the process was successful</returns>
        private bool ApplyMinimumAllocationFirstByHeaderType(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            StoreProfile aStore,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            bool bUnitsToAllocate = false;
            List<AllocationProfile> headersForProcessing = new List<AllocationProfile>();

            headersForProcessing.Clear();
            foreach (eHeaderType headerType in Enum.GetValues(typeof(eHeaderType)))
            {
                headersForProcessing.Clear();
                foreach (AllocationProfile ap in headersToProcess)
                {
                    if (ap.HeaderType == headerType)
                    {
                        headersForProcessing.Add(ap);
                    }
                }
                if (headersForProcessing.Count > 0)
                {
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, Environment.NewLine + "Determine if headers of type " + headerType + " can fulfill minimums for store " + aStore.Text);

                    if (HeadersCanFulfillStore(aSAB, aApplicationTransaction, masterHeader, aStore, headersForProcessing, subordinateHeaders, ref bUnitsToAllocate, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums))
                    {
                        if (bUnitsToAllocate)
                        {
                            WriteToMonitor(aSAB, DCFulfillmentMonitor, "Headers of type " + headerType + " can fulfill minimums for store " + aStore.Text);

                            ApplyMinimumAllocationFirstProcessing(aSAB, aApplicationTransaction, masterHeader, headersForProcessing, aStore, subordinateHeaders, DCFulfillmentMonitor);
                        }
                        //else
                        //{
                        //    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + aStore.Text + " was not allocated any units and will be skipped");
                        //}

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Apply minimums to all headers in the DC regardless of header type
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <returns>A flag indicating if the process was successful</returns>
        private bool ApplyMinimumAllocationFirstByDC(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            StoreProfile aStore,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            bool bUnitsToAllocate = false;

            WriteToMonitor(aSAB, DCFulfillmentMonitor, "Determine if headers for DC can fulfill minimums for store " + aStore.Text);
            if (HeadersCanFulfillStore(aSAB, aApplicationTransaction, masterHeader, aStore, headersToProcess, subordinateHeaders, ref bUnitsToAllocate, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums))
            {
                if (bUnitsToAllocate)
                {
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Headers for DC can fulfill minimums for store " + aStore.Text);

                    ApplyMinimumAllocationFirstProcessing(aSAB, aApplicationTransaction, masterHeader, headersToProcess, aStore, subordinateHeaders, DCFulfillmentMonitor);
                }
                //else
                //{
                //    WriteToMonitor(aSAB, DCFulfillmentMonitor, "Store " + aStore.Text + " was not allocated any units and will be skipped");
                //}

                return true;
            }
            return false;
        }

        /// <summary>
        /// Apply minimums to the components of a store
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        private void ApplyMinimumAllocationFirstProcessing(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            StoreProfile aStore,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor
            )
        {
            try
            {
                ProfileList storeList = new ProfileList(eProfileType.Store);
                storeList.Add(aStore);

                if (masterHeader.BulkColors.Count == 0
                     && masterHeader.NonGenericPackCount == 0
                     && masterHeader.GenericPackCount == 0)
                {
                    SplitTotalAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storeList, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums);
                }
                else
                {
                    // PACKS
                    if (masterHeader.NonGenericPackCount > 0
                        || masterHeader.GenericPackCount > 0)
                    {
                        SplitPackAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storeList, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums);
                    }

                    // BULK
                    if (masterHeader.BulkColors.Count > 0)
                    {
                        if (masterHeader.AtLeastOneSizeAllocated &&
                            masterHeader.BulkSizeAllocationInBalance)
                        {
                            SplitBulkSizeAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storeList, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums);
                        }
                        else
                        {
                            SplitBulkAllocation(aSAB, aApplicationTransaction, masterHeader, headersToProcess, storeList, subordinateHeaders, DCFulfillmentMonitor, eDCFulfillmentProcessing.Minimums);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Checks to determine if the provided list of subordinate headers contain 
        /// enough available units for the allocation on the master
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="aStore">The store to process</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="bUnitsToAllocate">A flag identifying if the store has any units to allocate</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="DCFulfillmentProcessing">Flag identifying the stage of the DC Fulfillment process</param>
        /// <returns>A flag indicating if the process was successful</returns>
        private bool HeadersCanFulfillStore(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader, 
            StoreProfile aStore,
            List<AllocationProfile> headersToProcess,
            List<AllocationProfile> subordinateHeaders,
            ref bool bUnitsToAllocate,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            eDCFulfillmentProcessing DCFulfillmentProcessing = eDCFulfillmentProcessing.Proportional
            )
        {
            string message = null;
            int mhStoreAllocated = 0;
            int unitsRemaining = 0;
            Index_RID storeIndex;
            HdrColorBin hcb;
            HdrSizeBin hsb;
            bool canFulfillAllocation = true;

            message = Environment.NewLine + "Determine if master header " + masterHeader.HeaderID + " allocation for store " + aStore.Text + " (" + aStore.Key + ") can be fulfill by the following subordinate headers for " + DCFulfillmentProcessing + Environment.NewLine;
            foreach (AllocationProfile ap in headersToProcess)
            {
                message += "Subordinate:" + ap.HeaderID + Environment.NewLine;
            }

            int storeAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
            if (storeAllocated == 0)
            {
                bUnitsToAllocate = false;
                return true;
            }

            // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
			//bUnitsToAllocate = true;
			bUnitsToAllocate = false;
			// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
            // total header
            if (masterHeader.BulkColors.Count == 0
                        && masterHeader.NonGenericPackCount == 0
                        && masterHeader.GenericPackCount == 0)
            {
                switch (DCFulfillmentProcessing)
                {
                    case eDCFulfillmentProcessing.Minimums:
                        // use the less of the minimum and the value allocated to the store
                        mhStoreAllocated = masterHeader.GetStoreMinimum(eAllocationSummaryNode.Total, aStore.Key, false);
                        storeAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                        if (storeAllocated < mhStoreAllocated)
                        {
                            mhStoreAllocated = storeAllocated;
                        }
                        break;
                    default:
                        mhStoreAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                        // adjust quantity by values already allocated to the store
                        foreach (AllocationProfile childAp in subordinateHeaders)
                        {
                            mhStoreAllocated -= childAp.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                        }
                        break;
                }

                // no need to check if value is zero
                if (mhStoreAllocated > 0)
                {
                    bUnitsToAllocate = true;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                    unitsRemaining = 0;
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        unitsRemaining += childAp.TotalUnitsToAllocate - childAp.TotalUnitsAllocated;
                    }
                    if (mhStoreAllocated > unitsRemaining)
                    {
                        canFulfillAllocation = false;
                        message += "Insufficient total units." + " Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                    }
                    else
                    {
                        message += "Sufficient total units." + " Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                    }
                }
                else
                {
                    message += "Total does not require any units for " + DCFulfillmentProcessing + Environment.NewLine;
                }
            }
            else
            {
                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
				bool canPacksFulfillAllocation = true;
                bool packUnitsToAllocate = false;
                bool canBulkFulfillAllocation = true;
                bool bulkUnitsToAllocate = false;
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                // PACKS
                if (masterHeader.NonGenericPackCount > 0
                    || masterHeader.GenericPackCount > 0)
                {
                    bool packFound = false;
                    string[] PackIds = masterHeader.GetPackNames();
                    List<int> associatedPackRids;
                    Dictionary<AllocationProfile, PackHdr> childPacks = new Dictionary<AllocationProfile, PackHdr> { };
                    Dictionary<AllocationProfile, PackHdr> subordinateChildPacks = new Dictionary<AllocationProfile, PackHdr> { };
                    PackHdr pack;

                    foreach (string packName in PackIds)
                    {
                        associatedPackRids = masterHeader.GetAssociatedPackRIDs(packName);
                        childPacks.Clear();
                        foreach (AllocationProfile childAp in headersToProcess)
                        {
                            string[] childPackIds = childAp.GetPackNames();
                            foreach (string childPackName in childPackIds)
                            {
                                int packRid = childAp.GetPackRID(childPackName);
                                //=======================================================================
                                // If we find a match, keep the pack to allocate to.
                                //=======================================================================
                                if (associatedPackRids.Contains(packRid))
                                {
                                    packFound = true;
                                    childPacks.Add(childAp, childAp.GetPackHdr(packRid));
                                }
                            }
                        }
                        // get list of subordinates that contain pack to back out from total
                        subordinateChildPacks.Clear();
                        foreach (AllocationProfile childAp in subordinateHeaders)
                        {
                            string[] childPackIds = childAp.GetPackNames();
                            foreach (string childPackName in childPackIds)
                            {
                                int packRid = childAp.GetPackRID(childPackName);
                                //=======================================================================
                                // If we find a match, keep the pack to back out from total.
                                //=======================================================================
                                if (associatedPackRids.Contains(packRid))
                                {
                                    subordinateChildPacks.Add(childAp, childAp.GetPackHdr(packRid));
                                }
                            }
                        }

                        // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
						pack = masterHeader.GetPackHdr(packName);
                        switch (DCFulfillmentProcessing)
                        {
                            case eDCFulfillmentProcessing.Minimums:
                                // use the less of the minimum and the value allocated to the store
                                storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                                mhStoreAllocated = masterHeader.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndex, false);
                                storeAllocated = masterHeader.GetStoreQtyAllocated(packName, aStore.Key) * pack.PackMultiple;
                                if (storeAllocated < mhStoreAllocated)
                                {
                                    mhStoreAllocated = storeAllocated;
                                }
                                break;
                            default:
                                mhStoreAllocated = masterHeader.GetStoreQtyAllocated(packName, aStore.Key);
                                // adjust quantity by values already allocated to the store
                                AllocationProfile childAp;
                                foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in subordinateChildPacks)
                                {
                                    childAp = keyPair.Key;
                                    pack = keyPair.Value;
                                    mhStoreAllocated -= childAp.GetStoreQtyAllocated(pack.PackName, aStore.Key);
                                }
                                // get total units for pack
                                mhStoreAllocated = mhStoreAllocated * pack.PackMultiple;
                                break;
                        }
					    // End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                        if (packFound)
                        {
                            AllocationProfile childAp;

                            // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
							//pack = masterHeader.GetPackHdr(packName);
                            //switch (DCFulfillmentProcessing)
                            //{
                            //    case eDCFulfillmentProcessing.Minimums:
                            //        // use the less of the minimum and the value allocated to the store
                            //        storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                            //        mhStoreAllocated = masterHeader.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndex, false);
                            //        storeAllocated = masterHeader.GetStoreQtyAllocated(packName, aStore.Key) * pack.PackMultiple;
                            //        if (storeAllocated < mhStoreAllocated)
                            //        {
                            //            mhStoreAllocated = storeAllocated;
                            //        }
                            //        break;
                            //    default:
                            //        mhStoreAllocated = masterHeader.GetStoreQtyAllocated(packName, aStore.Key);
                            //        // adjust quantity by values already allocated to the store
                            //        foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in subordinateChildPacks)
                            //        {
                            //            childAp = keyPair.Key;
                            //            pack = keyPair.Value;
                            //            mhStoreAllocated -= childAp.GetStoreQtyAllocated(pack.PackName, aStore.Key);
                            //        }
                            //        // get total units for pack
                            //        mhStoreAllocated = mhStoreAllocated * pack.PackMultiple;
                            //        break;
                            //}
						    // End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                            // no need to check if value is zero
                            if (mhStoreAllocated > 0)
                            {
                                bUnitsToAllocate = true;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                packUnitsToAllocate = true;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                unitsRemaining = 0;
                                foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in childPacks)
                                {
                                    childAp = keyPair.Key;
                                    pack = keyPair.Value;

                                    unitsRemaining += childAp.GetUnitsToAllocateByPack(pack.PackName) - childAp.GetUnitsAllocatedByPack(pack.PackName);
                                }
                                if (mhStoreAllocated > unitsRemaining)
                                {
                                    // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
									//canFulfillAllocation = false;
									canPacksFulfillAllocation = false;
									// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    message += "Insufficient units for pack " + packName + ". Require:" + mhStoreAllocated / pack.PackMultiple + ". Remaining:" + unitsRemaining / pack.PackMultiple + Environment.NewLine;
                                }
                                else
                                {
                                    message += "Sufficient units for pack " + packName + ". Require:" + mhStoreAllocated / pack.PackMultiple + ". Remaining:" + unitsRemaining / pack.PackMultiple + Environment.NewLine;
                                }
                            }
                            else
                            {
                                message += "Pack " + packName + " does not require any units for " + DCFulfillmentProcessing + Environment.NewLine;
                            }
                        }
                        else
                        {
                            // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
							//canFulfillAllocation = false;
							if (mhStoreAllocated > 0)
                            {
                                packUnitsToAllocate = true;
                                canPacksFulfillAllocation = false;
                                message += "Insufficient units for pack " + packName + ". Require:" + (int)(((double)mhStoreAllocated / (double)pack.PackMultiple) + .5) + ". Pack not found on subordinate." + Environment.NewLine;
                            }
							// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                        }
                    }
                }
				// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                else  // no packs
                {
                    canPacksFulfillAllocation = false;
                }
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                // BULK
                if (masterHeader.BulkColors.Count > 0)
                {
                    // bulk color size
                    if (masterHeader.AtLeastOneSizeAllocated &&
                        masterHeader.BulkSizeAllocationInBalance)
                    {
                        int[] bulkColorCodeRIDs = masterHeader.GetBulkColorCodeRIDs();
                        ArrayList mcAllocProfileList = new ArrayList();		// alloc profiles w/ matching color
                        ArrayList msAllocProfileList = new ArrayList();		// alloc profiles w/ matching size
                        ArrayList mcSubordinateProfileList = new ArrayList();
                        ArrayList msToAllocateList = new ArrayList();
                        foreach (int colorKey in bulkColorCodeRIDs)
                        {
                            foreach (AllocationProfile childAp in headersToProcess)
                            {
                                if (childAp.BulkColorIsOnHeader(colorKey))
                                {
                                    mcAllocProfileList.Add(childAp);
                                }
                            }

                            // get list of subordinates that contain color to back out from total
                            mcSubordinateProfileList.Clear();
                            foreach (AllocationProfile childAp in subordinateHeaders)
                            {
                                if (childAp.BulkColorIsOnHeader(colorKey))
                                {
                                    mcSubordinateProfileList.Add(childAp);
                                }
                            }

                            hcb = masterHeader.GetHdrColorBin(colorKey);
                            int[] mhSizeKeys = masterHeader.GetBulkColorSizeCodeRIDs(colorKey);
                            foreach (int sizeKey in mhSizeKeys)
                            {
                                switch (DCFulfillmentProcessing)
                                {
                                    case eDCFulfillmentProcessing.Minimums:
                                        // use the less of the minimum and the value allocated to the store
                                        hsb = hcb.GetSizeBin(sizeKey);
                                        storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                                        mhStoreAllocated = masterHeader.GetStoreMinimum(hsb, storeIndex, false);
                                        storeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                        if (storeAllocated < mhStoreAllocated)
                                        {
                                            mhStoreAllocated = storeAllocated;
                                        }
                                        break;
                                    default:
                                        mhStoreAllocated = masterHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                        // adjust quantity by values already allocated to the store
                                        foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                        {
                                            mhStoreAllocated -= childAp.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                        }
                                        break;
                                }
                                // no need to check if value is zero
                                if (mhStoreAllocated > 0)
                                {
                                    bUnitsToAllocate = true;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    bulkUnitsToAllocate = true;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    unitsRemaining = 0;
                                    foreach (AllocationProfile childAp in mcAllocProfileList)
                                    {
                                        if (childAp.SizeIsOnBulkColor(colorKey, sizeKey))
                                        {
                                            unitsRemaining += childAp.GetSizeUnitsToAllocate(colorKey, sizeKey) - childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        }
                                    }
                                    if (mhStoreAllocated > unitsRemaining)
                                    {
                                        // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
										//canFulfillAllocation = false;
										canBulkFulfillAllocation = false;
										// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                        message += "Insufficient units for color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ") size " + AppSessionTransaction.GetSizeCodeProfile(sizeKey).SizeCodeName + " (" + sizeKey + "). Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                                    }
                                    else
                                    {
                                        message += "Sufficient units for color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ") size " + AppSessionTransaction.GetSizeCodeProfile(sizeKey).SizeCodeName + " (" + sizeKey + "). Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    message += "Color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ") size " + AppSessionTransaction.GetSizeCodeProfile(sizeKey).SizeCodeName + " (" + sizeKey + ") does not require any units for " + DCFulfillmentProcessing + Environment.NewLine;
                                }
                            }
                        }
                    }
                    // bulk color
                    else
                    {
                        int[] bulkColorCodeRIDs = masterHeader.GetBulkColorCodeRIDs();
                        ArrayList mcAllocProfileList = new ArrayList();
                        ArrayList mcToAllocateList = new ArrayList();
						ArrayList mcSubordinateProfileList = new ArrayList();

                        foreach (int colorKey in bulkColorCodeRIDs)
                        {
                            // get list of subordinates that contain color to back out from total
                            mcSubordinateProfileList.Clear();
                            foreach (AllocationProfile childAp in subordinateHeaders)
                            {
                                if (childAp.BulkColorIsOnHeader(colorKey))
                                {
                                    mcSubordinateProfileList.Add(childAp);
                                }
                            }

                            switch (DCFulfillmentProcessing)
                            {
                                case eDCFulfillmentProcessing.Minimums:
                                    // use the less of the minimum and the value allocated to the store
                                    hcb = masterHeader.GetHdrColorBin(colorKey);
                                    storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                                    mhStoreAllocated = masterHeader.GetStoreMinimum(hcb, storeIndex, false);
                                    storeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, aStore.Key);
                                    if (storeAllocated < mhStoreAllocated)
                                    {
                                        mhStoreAllocated = storeAllocated;
                                    }
                                    break;
                                default:
                                    mhStoreAllocated = masterHeader.GetStoreQtyAllocated(colorKey, aStore.Key);
                                    // adjust quantity by values already allocated to the store
                                    foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                    {
                                        mhStoreAllocated -= childAp.GetStoreQtyAllocated(colorKey, aStore.Key);
                                    }
                                    break;
                            }
                            // no need to check if value is zero
                            if (mhStoreAllocated > 0)
                            {
                                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
								bUnitsToAllocate = true;
                                bulkUnitsToAllocate = true;
								// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                unitsRemaining = 0;
                                foreach (AllocationProfile childAp in headersToProcess)
                                {
                                    if (childAp.BulkColorIsOnHeader(colorKey))
                                    {
                                        unitsRemaining += childAp.GetColorUnitsToAllocate(colorKey) - childAp.GetColorUnitsAllocated(colorKey);
                                    }
                                }
                                if (mhStoreAllocated > unitsRemaining)
                                {
                                    // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
									//canFulfillAllocation = false;
									canBulkFulfillAllocation = false;
									// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    message += "Insufficient units for color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + "). Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                                }
                                else
                                {
                                    message += "Sufficient units for color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + "). Require:" + mhStoreAllocated + ". Remaining:" + unitsRemaining + Environment.NewLine;
                                }
                            }
                            else
                            {
                                message += "Color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ") does not require any units for " + DCFulfillmentProcessing + Environment.NewLine;
                            }
                        }
                    }
                }
				// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                else  // no bulk
                {
                    canBulkFulfillAllocation = false;
                }

                if ((packUnitsToAllocate && canPacksFulfillAllocation == false)
                    || (bulkUnitsToAllocate && canBulkFulfillAllocation == false)
                   )
                {
                    canFulfillAllocation = false;
                }
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
            }

            // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
			if (bUnitsToAllocate == false)
            {
                message += "Fulfillment not needed for store " + aStore.Text;
            }
            else if (canFulfillAllocation)
			//if (canFulfillAllocation)
			// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
            {
                message += "Headers can fulfill store " + aStore.Text;
            }
            else
            {
                message += "Headers cannot fulfill store " + aStore.Text + Environment.NewLine;
            }
            WriteToMonitor(aSAB, DCFulfillmentMonitor, message);

            return canFulfillAllocation;
        }

        /// <summary>
        /// Splits a master header allocation that is total only
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storesToProcess">List of stores to process</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="DCFulfillmentProcessing">Flag identifying the stage of the DC Fulfillment process</param>
        private void SplitTotalAllocation(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storesToProcess,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            eDCFulfillmentProcessing DCFulfillmentProcessing = eDCFulfillmentProcessing.Proportional
            )
        {
            try
            {
                string message = null;
                Index_RID storeIndex;
                ArrayList mcAllocProfileList = new ArrayList();
                ArrayList mcToAllocateList = new ArrayList();
                BasicSpread spread = new BasicSpread();

                foreach (AllocationProfile childAp in headersToProcess)
                {
                    mcAllocProfileList.Add(childAp);
                    int QtyToAllocate = childAp.TotalUnitsToAllocate - childAp.TotalUnitsAllocated;
                    mcToAllocateList.Add(QtyToAllocate);
                }
                //========================================================================================
                // For each store use the child header's total to allocate  as the basis to
                // spread the Master header's allocated value.
                //========================================================================================
                AllocationSubtotalProfile mhAllocSubtotalProfile = aApplicationTransaction.GetAllocationGrandTotalProfile();
                foreach (StoreProfile aStore in storesToProcess.ArrayList)
                {
                    message = "Processing Total Allocation for Store:" + aStore.Text + " (" + aStore.Key + ") processing mode " + DCFulfillmentProcessing + Environment.NewLine;
                    storeIndex = (Index_RID)aApplicationTransaction.StoreIndexRID(aStore.Key);
                    int mhStoreAllocated;
                    switch (DCFulfillmentProcessing)
                    {
                        case eDCFulfillmentProcessing.ReserveStore:
                            mhStoreAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                            message += "Master Header Store Allocated:" + mhStoreAllocated + Environment.NewLine;
                            message += "Reduce Quantity by Subordinate Values" + Environment.NewLine;
                            // adjust quantity by values already allocated to the store
                            foreach (AllocationProfile childAp in subordinateHeaders)
                            {
                                int subordinateQuantity = childAp.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                                mhStoreAllocated -= subordinateQuantity;
                                message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                            }
                            message += "Master Header store value to allocate after adjusting:" + mhStoreAllocated + Environment.NewLine;
                            break;
                        case eDCFulfillmentProcessing.Minimums:
                            // use the less of the minimum and the value allocated to the store
                            mhStoreAllocated = masterHeader.GetStoreMinimum(eAllocationSummaryNode.Total, aStore.Key, false);
                            int storeAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                            if (storeAllocated < mhStoreAllocated)
                            {
                                mhStoreAllocated = storeAllocated;
                            }
                            message += "Master Header Store Minimum:" + mhStoreAllocated + Environment.NewLine;
                            message += "Master Header Store Allocated:" + storeAllocated;
                            if (mhStoreAllocated == 0)
                            {
                                message += Environment.NewLine + "Minimum of zero. Allocate nothing.";
                                return;
                            }
                            break;
                        default:
                            mhStoreAllocated = masterHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                            message += "Master Header Store Allocated:" + mhStoreAllocated + Environment.NewLine;
                            message += "Reduce Quantity by Subordinate Values" + Environment.NewLine;
                            // adjust quantity by values already allocated to the store
                            foreach (AllocationProfile childAp in subordinateHeaders)
                            {
                                int subordinateQuantity = childAp.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                                mhStoreAllocated -= subordinateQuantity;
                                message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                            }
                            message += "Master Header store value to allocate after adjusting:" + mhStoreAllocated + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                            break;
                    }
                    
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    message = string.Empty;

                    ArrayList spreadToList = new ArrayList();
                    ArrayList changedList = new ArrayList();
                    //==============
                    // SPREAD
                    //==============
                    double spreadValue = Convert.ToDouble(mhStoreAllocated);
                    spread.ExecuteSimpleSpread(spreadValue, mcToAllocateList, 0, out changedList);
                    if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                    {
                        DCFulfillmentMonitor.WriteSpreadValues(spreadValue, mcToAllocateList, changedList);
                    }

                    //================================================
                    // Apply spread to Total/store in child headers
                    //================================================
                    for (int i = 0; i < mcAllocProfileList.Count; i++)
                    {
                        mcToAllocateList[i] = (int)mcToAllocateList[i] - Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture);
                        AllocationProfile childAp = (AllocationProfile)mcAllocProfileList[i];
                        int previousValue = childAp.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndex);
                        int newValue = Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) + previousValue;
                        childAp.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndex, newValue, eDistributeChange.ToParent, false);
                        childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                        LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Total);

                        string result = Convert.ToInt32(changedList[i]) + " (Spread Value) + " + previousValue + " (Previous Value) = " + newValue;
                        message += "Subordinate:" + childAp.HeaderID + " New Value:" + result + Environment.NewLine;
                    }

                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    message = string.Empty;
                }
                foreach (AllocationProfile childAp in headersToProcess)
                {
                    childAp.SetAllocationFromMultiHeader(true);
                    foreach (StoreProfile aStore in storesToProcess.ArrayList)
                    {
                        storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                        childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                        LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Total);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Splits a master header allocation that has bulk/color
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storesToProcess">List of stores to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="DCFulfillmentProcessing">Flag identifying the stage of the DC Fulfillment process</param>
        private void SplitBulkAllocation(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storesToProcess,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            eDCFulfillmentProcessing DCFulfillmentProcessing = eDCFulfillmentProcessing.Proportional
            )
        {
            try
            {
                string message = null;
                int[] bulkColorCodeRIDs = masterHeader.GetBulkColorCodeRIDs();
                ArrayList mcAllocProfileList = new ArrayList();
                ArrayList mcSubordinateProfileList = new ArrayList();
                ArrayList mcToAllocateList = new ArrayList();
                BasicSpread spread = new BasicSpread();
                Index_RID storeIndex;
                HdrColorBin hcb;

                message = Environment.NewLine + "Processing Bulk for Master Header Bulk Colors " + Environment.NewLine;

                foreach (int colorKey in bulkColorCodeRIDs)
                {
                    message += "processing mode " + DCFulfillmentProcessing + " for color code " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ")" + Environment.NewLine;
                    mcAllocProfileList.Clear();
                    mcToAllocateList.Clear();
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        if (childAp.BulkColorIsOnHeader(colorKey))
                        {
                            mcAllocProfileList.Add(childAp);
                            int QtyToAllocate;
                            switch (DCFulfillmentProcessing)
                            {
                                case eDCFulfillmentProcessing.ReserveStore:
                                    QtyToAllocate = childAp.GetColorUnitsToAllocate(colorKey);
                                    break;
                                case eDCFulfillmentProcessing.Minimums:
                                    hcb = childAp.GetHdrColorBin(colorKey);
                                    storeIndex = (Index_RID)childAp.StoreIndex((storesToProcess[0]).Key);
                                    QtyToAllocate = childAp.GetStoreMinimum(hcb, storeIndex, false);
									// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    int remainingUnits = childAp.GetColorUnitsToAllocate(colorKey) - childAp.GetColorUnitsAllocated(colorKey);
                                    if (remainingUnits < QtyToAllocate)
                                    {
                                        QtyToAllocate = remainingUnits;
                                    }
									// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    if (QtyToAllocate == 0)
                                    {
                                        message += "Minimum of zero. Allocate nothing.";
                                        return;
                                    }
                                    break;
                                default:
                                    // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
									//QtyToAllocate = childAp.GetColorUnitsToAllocate(colorKey);
									QtyToAllocate = childAp.GetColorUnitsToAllocate(colorKey) - childAp.GetColorUnitsAllocated(colorKey);
									// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    break;
                            }
                            
                            mcToAllocateList.Add(QtyToAllocate);

                            message += "Subordinate " + childAp.HeaderID + " quanitity to allocate " + QtyToAllocate;
                        }
                        else
                        {
                            message += "Subordinate " + childAp.HeaderID + " does not contain bulk color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + " (" + colorKey + ")";
                        }
                    }
                    // get list of subordinates that contain color to back out from total
                    mcSubordinateProfileList.Clear();
                    foreach (AllocationProfile childAp in subordinateHeaders)
                    {
                        if (childAp.BulkColorIsOnHeader(colorKey))
                        {
                            mcSubordinateProfileList.Add(childAp);
                        }
                    }

                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    message = string.Empty;

                    //========================================================================================
                    // For each store use the child header's total to allocate for the color as the basis to
                    // spread the Master header's allocated color value.
                    //========================================================================================
                    int colorMatchCnt = mcAllocProfileList.Count;
                    AllocationSubtotalProfile mhAllocSubtotalProfile = aApplicationTransaction.GetAllocationGrandTotalProfile();
                    foreach (StoreProfile aStore in storesToProcess.ArrayList)
                    {
                        message = "Processing Bulk for Store:" + aStore.Text + " (" + aStore.Key + ") processing mode " + DCFulfillmentProcessing + Environment.NewLine;
                        int mhStoreAllocated;
                        switch (DCFulfillmentProcessing)
                        {
                            case eDCFulfillmentProcessing.ReserveStore:
                                mhStoreAllocated = masterHeader.GetStoreQtyAllocated(colorKey, aStore.Key);
                                message += "Master Header Store Allocated:" + mhStoreAllocated + Environment.NewLine;
                                message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                // adjust quantity by values already allocated to the store
                                foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                {
                                    int subordinateQuantity = childAp.GetStoreQtyAllocated(colorKey, aStore.Key);
                                    mhStoreAllocated -= subordinateQuantity;
                                    message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                                }
                                message += "Master Header store value to allocate after adjusting:" + mhStoreAllocated + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                break;
                            case eDCFulfillmentProcessing.Minimums:
                                // use the less of the minimum and the value allocated to the store
                                hcb = masterHeader.GetHdrColorBin(colorKey);
                                storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                                mhStoreAllocated = masterHeader.GetStoreMinimum(hcb, storeIndex, false);
                                int storeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, aStore.Key);
                                message += "Master Header Store Minimum:" + mhStoreAllocated + Environment.NewLine;
                                message += "Master Header Store Allocated:" + storeAllocated + Environment.NewLine;
                                if (storeAllocated < mhStoreAllocated)
                                {
                                    message += "Use Master Header Store Allocated since less than minimum";
                                    mhStoreAllocated = storeAllocated;
                                }
                                if (mhStoreAllocated == 0)
                                {
                                    message += "Minimum of zero. Allocate nothing.";
                                    return;
                                }
                                break;
                            default:
                                mhStoreAllocated = masterHeader.GetStoreQtyAllocated(colorKey, aStore.Key);
                                message += "Master Header Store Allocated:" + mhStoreAllocated + Environment.NewLine;
                                message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                // adjust quantity by values already allocated to the store
                                foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                {
                                    int subordinateQuantity = childAp.GetStoreQtyAllocated(colorKey, aStore.Key);
                                    mhStoreAllocated -= subordinateQuantity;
                                    message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                                }
                                message += "Master Header store value to allocate after adjusting:" + mhStoreAllocated + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                break;
                        }
                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        message = string.Empty;

                        // do nothing if no units to spread
                        if (mhStoreAllocated == 0)
                        {
                            continue;
                        }
                        
                        ArrayList spreadToList = new ArrayList();
                        ArrayList changedList = new ArrayList();
                        //==============
                        // SPREAD
                        //==============
                        double spreadValue = Convert.ToDouble(mhStoreAllocated);                        
                        spread.ExecuteSimpleSpread(spreadValue, mcToAllocateList, 0, out changedList);
                        if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                        {
                            DCFulfillmentMonitor.WriteSpreadValues(spreadValue, mcToAllocateList, changedList);
                        }

                        //================================================
                        // Apply spread to color/store in child headers
                        //================================================
                        for (int i = 0; i < colorMatchCnt; i++)
                        {
                            mcToAllocateList[i] = (int)mcToAllocateList[i] - Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture);
                            AllocationProfile childAp = (AllocationProfile)mcAllocProfileList[i];
                            storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
							//int previousValue = Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) + childAp.GetStoreQtyAllocated(colorKey, storeIndex);
                            int previousValue = childAp.GetStoreQtyAllocated(colorKey, storeIndex);
                            int newValue = Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) + previousValue;
                            childAp.SetStoreQtyAllocated(colorKey, storeIndex, newValue, eDistributeChange.ToParent, false);
                            childAp.SetAllDetailAuditFlags(colorKey, storeIndex, masterHeader.GetAllDetailAuditFlags(colorKey, storeIndex));
                            LogAndDisableColorItemManuallyAllocated(childAp, storeIndex, colorKey);

                            string result = Convert.ToInt32(changedList[i]) + " (Spread Value) + " + previousValue + " (Previous Value) = " + newValue;
                            message += "Subordinate:" + childAp.HeaderID + " New Value:" + result + Environment.NewLine;
                        }

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        message = string.Empty;
                    }
                }

                foreach (AllocationProfile childAp in headersToProcess)
                {
                    if (childAp.BulkColors.Count > 0)
                    {
                        childAp.SetAllocationFromMultiHeader(true);
                        foreach (StoreProfile aStore in storesToProcess.ArrayList)
                        {
                            storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                            childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                            LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Total);
                            childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIndex));
                            LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Bulk);
                            if (childAp.BulkIsDetail)
                            {
                                childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex));
                                LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.DetailType);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Splits a master header allocation that has bulk/color/size 
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storesToProcess">List of stores to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="DCFulfillmentProcessing">Flag identifying the stage of the DC Fulfillment process</param>
        private void SplitBulkSizeAllocation(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storesToProcess,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            eDCFulfillmentProcessing DCFulfillmentProcessing = eDCFulfillmentProcessing.Proportional
            )
        {
            try
            {
                string message = null;
                Index_RID storeIndex = new Index_RID();
                HdrColorBin hcb = null;
                HdrSizeBin hsb = null;

                int[] bulkColorCodeRIDs = masterHeader.GetBulkColorCodeRIDs();
                ArrayList mcAllocProfileList = new ArrayList();		// alloc profiles w/ matching color
                ArrayList msAllocProfileList = new ArrayList();		// alloc profiles w/ matching size
                ArrayList mcSubordinateProfileList = new ArrayList();
                ArrayList msToAllocateList = new ArrayList();

                BasicSpread spread = new BasicSpread();

                message = string.Empty;

                foreach (int colorKey in bulkColorCodeRIDs)
                {
                    mcAllocProfileList.Clear();
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        if (childAp.BulkColorIsOnHeader(colorKey))
                        {
                            mcAllocProfileList.Add(childAp);
                        }
                    }
                    // get list of subordinates that contain color to back out from total
                    mcSubordinateProfileList.Clear();
                    foreach (AllocationProfile childAp in subordinateHeaders)
                    {
                        if (childAp.BulkColorIsOnHeader(colorKey))
                        {
                            mcSubordinateProfileList.Add(childAp);
                        }
                    }

                    if (DCFulfillmentProcessing == eDCFulfillmentProcessing.Minimums)
                    {
                        hcb = masterHeader.GetHdrColorBin(colorKey);
                        // only one store in list
                        storeIndex = (Index_RID)masterHeader.StoreIndex((storesToProcess[0]).Key);
                    }

                    int[] mhSizeKeys = masterHeader.GetBulkColorSizeCodeRIDs(colorKey);
                    foreach (int sizeKey in mhSizeKeys)
                    {
                        int mhSizeQtyToAllocate;
                        switch (DCFulfillmentProcessing)
                        {
                            case eDCFulfillmentProcessing.ReserveStore:
                                mhSizeQtyToAllocate = masterHeader.GetSizeUnitsToAllocate(colorKey, sizeKey);
                                break;
                            case eDCFulfillmentProcessing.Minimums:
                                hsb = hcb.GetSizeBin(sizeKey);
                                mhSizeQtyToAllocate = masterHeader.GetStoreMinimum(hsb, storeIndex, false);
                                break;
                            default:
                                mhSizeQtyToAllocate = masterHeader.GetSizeUnitsToAllocate(colorKey, sizeKey);
                                break;
                        }

                        msAllocProfileList.Clear();
                        msToAllocateList.Clear();
                        foreach (AllocationProfile childAp in mcAllocProfileList)
                        {
                            if (childAp.SizeIsOnBulkColor(colorKey, sizeKey))
                            {
                                msAllocProfileList.Add(childAp);
                                int QtyToAllocate;
                                switch (DCFulfillmentProcessing)
                                {
                                    case eDCFulfillmentProcessing.ReserveStore:
                                        QtyToAllocate = childAp.GetSizeUnitsToAllocate(colorKey, sizeKey) - childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        mhSizeQtyToAllocate -= childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        break;
                                    case eDCFulfillmentProcessing.Minimums:
                                        storeIndex = (Index_RID)childAp.StoreIndex((storesToProcess[0]).Key);
                                        hcb = childAp.GetHdrColorBin(colorKey);
                                        QtyToAllocate = childAp.GetStoreMinimum(hsb, storeIndex, false);
										// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                        int remainingUnits = childAp.GetSizeUnitsToAllocate(colorKey, sizeKey) - childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        if (remainingUnits < QtyToAllocate)
                                        {
                                            QtyToAllocate = remainingUnits;
                                        }
										// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                        break;
                                    default:
                                        QtyToAllocate = childAp.GetSizeUnitsToAllocate(colorKey, sizeKey) - childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        mhSizeQtyToAllocate -= childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
                                        break;
                                }
                                
                                msToAllocateList.Add(QtyToAllocate);
                            }
                            else
                            {
                                message += "Subordinate " + childAp.HeaderID + " does not contain bulk color RID " + colorKey + " and size code " + sizeKey + Environment.NewLine;
                            }
                        }

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        message = string.Empty;

                        //========================================================================================
                        // For each store use the child header's size total to allocate for the size as the basis to
                        // spread the Master header's allocated size value.
                        //========================================================================================
                        if (DCFulfillmentProcessing == eDCFulfillmentProcessing.Minimums)
                        {
                            hcb = masterHeader.GetHdrColorBin(colorKey);
                            // only one store in list
                            storeIndex = (Index_RID)masterHeader.StoreIndex((storesToProcess[0]).Key);
                        }
                        int sizeMatchCnt = msAllocProfileList.Count;
                        foreach (StoreProfile aStore in storesToProcess.ArrayList)
                        {
                            message = "Processing Bulk Color " + AppSessionTransaction.GetColorCodeProfile(colorKey).ColorCodeName + "  (" + colorKey + ") and size " + AppSessionTransaction.GetSizeCodeProfile(sizeKey).SizeCodeName + " (" + sizeKey + ") for Store:" + aStore.Text + " (" + aStore.Key + ") processing mode " + DCFulfillmentProcessing + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                            int mhStoreSizeAllocated;
                            switch (DCFulfillmentProcessing)
                            {
                                case eDCFulfillmentProcessing.ReserveStore:
                                    mhStoreSizeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                    message += "Master Header Store Allocated:" + mhStoreSizeAllocated + Environment.NewLine;
                                    message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                    // adjust quantity by values already allocated to the store
                                    foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                    {
                                        int subordinateQuantity = childAp.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                        mhStoreSizeAllocated -= subordinateQuantity;
                                        message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                                    }
                                    message += "Master Header store value to allocate after adjusting:" + mhStoreSizeAllocated + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    break;
                                case eDCFulfillmentProcessing.Minimums:
                                    // use the less of the minimum and the value allocated to the store
                                    storeIndex = (Index_RID)masterHeader.StoreIndex((storesToProcess[0]).Key);
                                    hsb = hcb.GetSizeBin(sizeKey);
                                    mhStoreSizeAllocated = masterHeader.GetStoreMinimum(hsb, storeIndex, false);
                                    int storeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                    message += "Header Store Minimum:" + mhStoreSizeAllocated + Environment.NewLine;
                                    message += "Master Header Store Allocated:" + storeAllocated + Environment.NewLine;
                                    if (storeAllocated < mhStoreSizeAllocated)
                                    {
                                        message += "Use Master Header Store Allocated since less than minimum";
                                        mhStoreSizeAllocated = storeAllocated;
                                    }

                                    if (mhStoreSizeAllocated == 0)
                                    {
                                        message += Environment.NewLine + "Minimum of zero. Allocate nothing.";
										// Begin TT#5718 - JSmith - Manually Adjusted Store Units go to VSW Qty
                                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                                        message = string.Empty;
                                        continue;
										// End TT#5718 - JSmith - Manually Adjusted Store Units go to VSW Qty
                                    }
                                    break;
                                default:
                                    mhStoreSizeAllocated = masterHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                    message += "Master Header Store Allocated:" + mhStoreSizeAllocated + Environment.NewLine;
                                    message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                    // adjust quantity by values already allocated to the store
                                    foreach (AllocationProfile childAp in mcSubordinateProfileList)
                                    {
                                        int subordinateQuantity = childAp.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
                                        mhStoreSizeAllocated -= subordinateQuantity;
                                        message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    }
                                    message += "Master Header store value to allocate after adjusting:" + mhStoreSizeAllocated + Environment.NewLine;  // TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    break;
                            }
                            WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                            message = string.Empty;

                            // do nothing if no units to spread
                            if (mhStoreSizeAllocated == 0)
                            {
                                continue;
                            }

                            ArrayList spreadToList = new ArrayList();
                            ArrayList changedList = new ArrayList();
                            //==============
                            // SPREAD
                            //==============
                            double spreadValue = Convert.ToDouble(mhStoreSizeAllocated);
                            spread.ExecuteSimpleSpread(spreadValue, msToAllocateList, 0, out changedList);
                            if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                            {
                                DCFulfillmentMonitor.WriteSpreadValues(spreadValue, msToAllocateList, changedList);
                            }

                            //===================================================
                            // Apply spread to color/size/store in child headers
                            //===================================================
                            for (int i = 0; i < sizeMatchCnt; i++)
                            {
                                msToAllocateList[i] = (int)msToAllocateList[i] - Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture);
                                AllocationProfile childAp = (AllocationProfile)msAllocProfileList[i];
                                storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                                int previousValue = childAp.GetStoreQtyAllocated(colorKey, sizeKey, storeIndex);
                                int newValue = Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) + previousValue;
                                childAp.SetStoreQtyAllocated(colorKey, sizeKey, storeIndex, newValue, eDistributeChange.ToParent, false);
                                childAp.SetAllDetailAuditFlags(colorKey, storeIndex, masterHeader.GetAllDetailAuditFlags(colorKey, storeIndex));
                                LogAndDisableColorItemManuallyAllocated(childAp, storeIndex, colorKey);
                                childAp.SetAllDetailAuditFlags(colorKey, sizeKey, storeIndex, masterHeader.GetAllDetailAuditFlags(colorKey, sizeKey, storeIndex));
                                LogAndDisableColorSizeItemManuallyAllocated(childAp, storeIndex, colorKey, sizeKey);
 
                                string result = Convert.ToInt32(changedList[i]) + " (Spread Value) + " + previousValue + " (Previous Value) = " + newValue;
                                message += "Subordinate:" + childAp.HeaderID + " New Value:" + result + Environment.NewLine;
                            }

                            WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                            message = string.Empty;
                        }
                    }
                }
                if (DCFulfillmentProcessing == eDCFulfillmentProcessing.Fulfillment)
                {
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        if (childAp.BulkColors.Count > 0)
                        {
                            childAp.SetAllocationFromMultiHeader(true);
                            foreach (StoreProfile aStore in storesToProcess.ArrayList)
                            {
                                storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                                childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                                LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Total);
                                childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIndex));
                                LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Bulk);
                                if (childAp.BulkIsDetail)
                                {
                                    childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex));
                                    LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.DetailType);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Splits a master header allocation that has packs
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="aApplicationTransaction">ApplicationSessionTransaction</param>
        /// <param name="masterHeader">Master Header Profile</param>
        /// <param name="headersToProcess">List of headers to process</param>
        /// <param name="storesToProcess">List of stores to process</param>
        /// <param name="subordinateHeaders">List of headers that belong to the master</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="DCFulfillmentProcessing">Flag identifying the stage of the DC Fulfillment process</param>
        private void SplitPackAllocation(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> headersToProcess,
            ProfileList storesToProcess,
            List<AllocationProfile> subordinateHeaders,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            eDCFulfillmentProcessing DCFulfillmentProcessing = eDCFulfillmentProcessing.Proportional
            )
        {
            try
            {
                string message = null;
                bool packFound = false;
                List<string> PackIds = new List<string>();
				string[] MasterPackIds = masterHeader.GetPackNames();
                List<int> associatedPackRids;
                Dictionary<AllocationProfile, PackHdr> childPacks = new Dictionary<AllocationProfile, PackHdr> { };
                Dictionary<AllocationProfile, PackHdr> subordinateChildPacks = new Dictionary<AllocationProfile, PackHdr> { };
                ArrayList msToAllocateList = new ArrayList();
                PackHdr pack;
                Index_RID storeIndex;

                BasicSpread spread = new BasicSpread();

                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
				//message = Environment.NewLine + "Processing Packs for Master Header " + Environment.NewLine;
                //WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                //message = string.Empty;
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                // Get packs to process from subordinates
                foreach (string masterPackName in MasterPackIds)
                {
                    associatedPackRids = masterHeader.GetAssociatedPackRIDs(masterPackName);
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        foreach (int associatedPackRid in associatedPackRids)
                        {
                            if (childAp.PackIsOnHeader(associatedPackRid))
                            {
                                if (!PackIds.Contains(masterPackName))
                                {
                                    PackIds.Add(masterPackName);
                                }
                            }
                        }
                    }
                }

                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
				if (PackIds.Count > 0)
                {
                    message = Environment.NewLine + "Processing Packs for Master Header " + Environment.NewLine;
                }
                else
                {
                    message = Environment.NewLine + "Packs for Master Header not found on subordinates.  Pack processing will be skipped. " + Environment.NewLine;
                }
                WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                message = string.Empty;
				// End TT#5746 - JSmith - DC Fulfillment Same DC Issue

                //=========================================================================================
                // Foreach pack on the Master-header, search the child headers looking for matching packs. 
                //=========================================================================================
                foreach (string packName in PackIds)
                {
                    message = "Master Header Pack " + packName + Environment.NewLine;
                    childPacks.Clear();
                    associatedPackRids = masterHeader.GetAssociatedPackRIDs(packName);
                    message += "Associated Packs (";
                    bool firstValue = true;
                    foreach (int associatedPackRID in associatedPackRids)
                    {
                        if (!firstValue)
                        {
                            message += ",";
                        }
                        message += associatedPackRID;
                        firstValue = false;
                    }
                    message += ")" + Environment.NewLine;
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    message = string.Empty;

                    packFound = false;
                    foreach (AllocationProfile childAp in headersToProcess)
                    {
                        message = "Subordinate " + childAp.HeaderID + " determine packs to allocate" + Environment.NewLine;
                        string[] childPackIds = childAp.GetPackNames();
                        foreach (string childPackName in childPackIds)
                        {
                            int packRid = childAp.GetPackRID(childPackName);
                            //=======================================================================
                            // If we find a match, keep the pack to allocate to.
                            //=======================================================================
                            if (associatedPackRids.Contains(packRid))
                            {
                                packFound = true;
                                PackHdr ph = childAp.GetPackHdr(packRid);
                                childPacks.Add(childAp, ph);
                                message += "Include pack " + ph.PackName + " (" + packRid + ")";
                            }
                        }
                    }
                    WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                    message = string.Empty;

                    // get list of subordinates that contain pack to back out from total
                    subordinateChildPacks.Clear();
                    foreach (AllocationProfile childAp in subordinateHeaders)
                    {
                        string[] childPackIds = childAp.GetPackNames();
                        foreach (string childPackName in childPackIds)
                        {
                            int packRid = childAp.GetPackRID(childPackName);
                            //=======================================================================
                            // If we find a match, keep the pack to back out from total.
                            //=======================================================================
                            if (associatedPackRids.Contains(packRid))
                            {
                                subordinateChildPacks.Add(childAp, childAp.GetPackHdr(packRid));
                            }
                        }
                    }


                    if (packFound)
                    {
                        AllocationProfile childAp;
                        msToAllocateList.Clear();
                        foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in childPacks)
                        {
                            childAp = keyPair.Key;
                            pack = keyPair.Value;

                            message = "Processing Pack:" + pack.PackName + " (" + pack.PackRID + ") processing mode " + DCFulfillmentProcessing + Environment.NewLine;

                            int QtyToAllocate;
                            switch (DCFulfillmentProcessing)
                            {
                                case eDCFulfillmentProcessing.ReserveStore:
                                    QtyToAllocate = childAp.GetUnitsToAllocateByPack(pack.PackName) - childAp.GetUnitsAllocatedByPack(pack.PackName);
                                    break;
                                case eDCFulfillmentProcessing.Minimums:
                                    storeIndex = (Index_RID)childAp.StoreIndex((storesToProcess[0]).Key);
                                    QtyToAllocate = childAp.GetStoreMinimum(pack, storeIndex, false);
									// Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    int remainingUnits = childAp.GetUnitsToAllocateByPack(pack.PackName) - childAp.GetUnitsAllocatedByPack(pack.PackName);
                                    if (remainingUnits < QtyToAllocate)
                                    {
                                        QtyToAllocate = remainingUnits;
                                    }
									// End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                    break;
                                default:
                                    QtyToAllocate = childAp.GetUnitsToAllocateByPack(pack.PackName) - childAp.GetUnitsAllocatedByPack(pack.PackName);
                                    break;
                            }
                            
                            msToAllocateList.Add(QtyToAllocate);
                            if (DCFulfillmentProcessing == eDCFulfillmentProcessing.Minimums)
                            {
                                message += "Subordinate " + childAp.HeaderID + " quanitity to allocate " + QtyToAllocate / pack.PackMultiple;
                            }
                            else
                            {
                                message += "Subordinate " + childAp.HeaderID + " quanitity to allocate " + QtyToAllocate;
                            }
                        }

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                        message = string.Empty;

                        foreach (StoreProfile aStore in storesToProcess.ArrayList)
                        {
                            message = "Processing SplitPackAllocation for Store:" + aStore.Text + " (" + aStore.Key + ") processing mode " + DCFulfillmentProcessing + Environment.NewLine;
                            int units;
                            switch (DCFulfillmentProcessing)
                            {
                                case eDCFulfillmentProcessing.ReserveStore:
                                    units = masterHeader.GetStoreQtyAllocated(packName, aStore.Key);
                                    message += "Master Header Store Allocated:" + units + Environment.NewLine;
                                    message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                    // adjust quantity by values already allocated to the store
                                    foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in subordinateChildPacks)
                                    {
                                        childAp = keyPair.Key;
                                        pack = keyPair.Value;
                                        int subordinateQuantity = childAp.GetStoreQtyAllocated(pack.PackName, aStore.Key);
                                        units -= subordinateQuantity;
                                        message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                                    }
                                    break;
                                case eDCFulfillmentProcessing.Minimums:
                                    // use the less of the minimum and the value allocated to the store
                                    storeIndex = (Index_RID)masterHeader.StoreIndex(aStore.Key);
                                    pack = masterHeader.GetPackHdr(packName);
                                    units = masterHeader.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndex, false);
                                    int storeAllocated = masterHeader.GetStoreQtyAllocated(packName, aStore.Key) * pack.PackMultiple;
                                    message += "Master Header Store Minimum:" + units / pack.PackMultiple + Environment.NewLine;
                                    message += "Master Header Store Allocated:" + storeAllocated / pack.PackMultiple + Environment.NewLine;
                                    if (storeAllocated < units)
                                    {
                                        message += "Use Master Header Store Allocated since less than minimum" + Environment.NewLine;
                                        units = storeAllocated;
                                    }
                                    units = (int)(((double)units
                                     / (double)pack.PackMultiple) + .5);

                                    message += "Packs to Allocate:" + units + Environment.NewLine;
                                    break;
                                default:
                                    units = masterHeader.GetStoreQtyAllocated(packName, aStore.Key);
                                    message += "Master Header Store Allocated:" + units + Environment.NewLine;
                                    message += "Adjust Master Header value to allocate by previously allocated subordinate values" + Environment.NewLine;
                                    // adjust quantity by values already allocated to the store
                                    foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in subordinateChildPacks)
                                    {
                                        childAp = keyPair.Key;
                                        pack = keyPair.Value;
                                        int subordinateQuantity = childAp.GetStoreQtyAllocated(pack.PackName, aStore.Key);
                                        units -= subordinateQuantity;
                                        message += "Subordinate:" + childAp.HeaderID + " Current Value:" + subordinateQuantity + Environment.NewLine;
                                    }
                                    break;
                            }

                            // do nothing if no units to spread
                            if (units == 0)
                            {
                                continue;
                            }

                            ArrayList spreadToList = new ArrayList();
                            ArrayList changedList = new ArrayList();
                            //==============
                            // SPREAD
                            //==============
                            double spreadValue = Convert.ToDouble(units);
                            spread.ExecuteSimpleSpread(spreadValue, msToAllocateList, 0, out changedList);
                            if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
                            {
                                DCFulfillmentMonitor.WriteSpreadValues(spreadValue, msToAllocateList, changedList);
                            }

                            int i = 0;
                            foreach (KeyValuePair<AllocationProfile, PackHdr> keyPair in childPacks)
                            {
                                childAp = keyPair.Key;
                                pack = keyPair.Value;
                                // Begin TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                //msToAllocateList[i] = (int)msToAllocateList[i] - Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture);
                                msToAllocateList[i] = (int)msToAllocateList[i] - (Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) * pack.PackMultiple);
                                // End TT#5746 - JSmith - DC Fulfillment Same DC Issue
                                storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                                int previousValue = childAp.GetStoreQtyAllocated(pack.PackName, storeIndex);
                                int newValue = Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture) + previousValue;
                                childAp.SetStoreQtyAllocated(pack.PackName, storeIndex, newValue, eDistributeChange.ToParent, false);
                                childAp.SetAllDetailAuditFlags(pack.PackName, storeIndex, masterHeader.GetAllDetailAuditFlags(packName, storeIndex));
                                LogAndDisablePackItemManuallyAllocated(childAp, storeIndex, pack.PackName);
                                string result = Convert.ToInt32(changedList[i]) + " (Spread Value) + " + previousValue + " (Previous Value) = " + newValue;
                                message += "Subordinate:" + childAp.HeaderID + " New Value:" + result + Environment.NewLine;
                                i++; 
                            }

                            WriteToMonitor(aSAB, DCFulfillmentMonitor, message);
                            message = string.Empty;
                        }
                    }
                    if (!packFound)
                    {
                        string errMsg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderNotMatchingPack, false);
                        errMsg = errMsg.Replace("{0}", packName);
                        errMsg = errMsg.Replace("{1}", masterHeader.HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Severe,
                            eMIDTextCode.msg_al_MasterHeaderNotMatchingPack,
                            errMsg,
                            this.GetType().Name);

                        WriteToMonitor(aSAB, DCFulfillmentMonitor, errMsg);

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MasterHeaderNotMatchingPack,
                            errMsg);
                    }
                }
                foreach (AllocationProfile childAp in headersToProcess)
                {
                    if (childAp.Packs.Count > 0)
                    {
                        childAp.SetAllocationFromMultiHeader(true);
                        foreach (StoreProfile aStore in storesToProcess.ArrayList)
                        {
                            storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
                            childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                            LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.Total);
                            if (childAp.NonGenericPackCount > 0)
                            {
                                childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex, masterHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIndex));
                                LogAndDisableSummaryNodeItemManuallyAllocated(childAp, storeIndex, eAllocationSummaryNode.DetailType);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the Item Manually Allocated flag to false and records the cell affected
        /// </summary>
        /// <param name="aAllocationProfile">The AllocationProfile where the item has been manually allocated</param>
        /// <param name="aStoreIdxRID">The store Index_RID where the item has been manually allocated</param>
        /// <param name="aSummaryNode">The summary node type where the item has been manually allocated</param>
        private void LogAndDisableSummaryNodeItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, eAllocationSummaryNode aSummaryNode)
        {
            if (aAllocationProfile.GetStoreItemIsManuallyAllocated(aSummaryNode, aStoreIdxRID))
            {
                aAllocationProfile.SetStoreItemIsManuallyAllocated(aSummaryNode, aStoreIdxRID, false);
                _manuallyAllocatedItems.Add(new SummaryNodeItemManuallyAllocated(aAllocationProfile, aStoreIdxRID, aSummaryNode));
            }
        }

        /// <summary>
        /// Sets the Item Manually Allocated flag to false and records the cell affected
        /// </summary>
        /// <param name="aAllocationProfile">The AllocationProfile where the item has been manually allocated</param>
        /// <param name="aStoreIdxRID">The store Index_RID where the item has been manually allocated</param>
        /// <param name="aPackName">The pack name where the item has been manually allocated</param>
        private void LogAndDisablePackItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, string aPackName)
        {
            if (aAllocationProfile.GetStoreItemIsManuallyAllocated(aPackName, aStoreIdxRID))
            {
                aAllocationProfile.SetStoreItemIsManuallyAllocated(aPackName, aStoreIdxRID, false);
                _manuallyAllocatedItems.Add(new PackItemManuallyAllocated(aAllocationProfile, aStoreIdxRID, aPackName));
            }
        }

        /// <summary>
        /// Sets the Item Manually Allocated flag to false and records the cell affected
        /// </summary>
        /// <param name="aAllocationProfile">The AllocationProfile where the item has been manually allocated</param>
        /// <param name="aStoreIdxRID">The store Index_RID where the item has been manually allocated</param>
        /// <param name="aColorKey">The summary color key the item has been manually allocated</param>
        private void LogAndDisableColorItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, int aColorKey)
        {
            if (aAllocationProfile.GetStoreItemIsManuallyAllocated(aColorKey, aStoreIdxRID))
            {
                aAllocationProfile.SetStoreItemIsManuallyAllocated(aColorKey, aStoreIdxRID, false);
                _manuallyAllocatedItems.Add(new ColorItemManuallyAllocated(aAllocationProfile, aStoreIdxRID, aColorKey));
            }
        }

        /// <summary>
        /// Sets the Item Manually Allocated flag to false and records the cell affected
        /// </summary>
        /// <param name="aAllocationProfile">The AllocationProfile where the item has been manually allocated</param>
        /// <param name="aStoreIdxRID">The store Index_RID where the item has been manually allocated</param>
        /// <param name="aColorKey">The color key where the item has been manually allocated</param>
        /// <param name="aSizeKey">The size key where the item has been manually allocated</param>
        private void LogAndDisableColorSizeItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, int aColorKey, int aSizeKey)
        {
            if (aAllocationProfile.GetStoreItemIsManuallyAllocated(aColorKey, aSizeKey, aStoreIdxRID))
            {
                aAllocationProfile.SetStoreItemIsManuallyAllocated(aColorKey, aSizeKey, aStoreIdxRID, false);
                _manuallyAllocatedItems.Add(new ColorSizeItemManuallyAllocated(aAllocationProfile, aStoreIdxRID, aColorKey, aSizeKey));
            }
        }

        /// <summary>
        /// Reapplies the stored item manually allocated entries
        /// </summary>
        private void ReApplyItemManuallyAllocated()
        {
            foreach (ItemManuallyAllocated item in _manuallyAllocatedItems)
            {
                if (item is SummaryNodeItemManuallyAllocated)
                {
                    SummaryNodeItemManuallyAllocated summaryItem = (SummaryNodeItemManuallyAllocated)item;
                    summaryItem.AllocationProfile.SetStoreItemIsManuallyAllocated(summaryItem.SummaryNode, summaryItem.StoreIdxRID, true);
                }
                else if (item is PackItemManuallyAllocated)
                {
                    PackItemManuallyAllocated packItem = (PackItemManuallyAllocated)item;
                    packItem.AllocationProfile.SetStoreItemIsManuallyAllocated(packItem.PackName, packItem.StoreIdxRID, true);
                }
                else if (item is ColorItemManuallyAllocated)
                {
                    ColorItemManuallyAllocated colorItem = (ColorItemManuallyAllocated)item;
                    colorItem.AllocationProfile.SetStoreItemIsManuallyAllocated(colorItem.ColorKey, colorItem.StoreIdxRID, true);
                }
                else if (item is ColorSizeItemManuallyAllocated)
                {
                    ColorSizeItemManuallyAllocated colorSizeItem = (ColorSizeItemManuallyAllocated)item;
                    colorSizeItem.AllocationProfile.SetStoreItemIsManuallyAllocated(colorSizeItem.ColorKey, colorSizeItem.SizeKey, colorSizeItem.StoreIdxRID, true);
                }
            }
        }

        /// <summary>
        /// Determines if the monitor is active and writes the message
        /// </summary>
        /// <param name="aSAB">SessionAddressBlock</param>
        /// <param name="DCFulfillmentMonitor">The DC Fulfillment Monitor object</param>
        /// <param name="message">The message to write to the monitor</param>
        private void WriteToMonitor(
            SessionAddressBlock aSAB,
            DCFulfillmentMonitor DCFulfillmentMonitor,
            string message
            )
        {
            if (aSAB.ClientServerSession.UserOptions.DCFulfillmentMonitorIsActive)
            {
                DCFulfillmentMonitor.WriteLine(message);
            }
        }

        #endregion DC Fulfillment

        #endregion Methods
    }

    public enum eDCFulfillmentProcessing
    {
        Proportional,
        Minimums,
        ReserveStore,
	    Fulfillment
    }

    public enum eDCFulfillmentHeaderProcessing
    {
        SingleHeader,
        SameHeaderType,
        All
    }

    public enum eDCFulfillmentPropertyProcessing
    {
        PreSplit,
        PostSplit
    }

    public class StoreToProcess
    {
        public int StoreRID;
        public List<string> StoreDCOrder;

        public StoreToProcess (int StoreRID, List<string> StoreDCOrder)
        {
            this.StoreRID = StoreRID;
            this.StoreDCOrder = StoreDCOrder;
        }
    }

    abstract internal class ItemManuallyAllocated
    {
        private AllocationProfile _allocationProfile;
        private Index_RID _storeIdxRID;

        internal ItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID)
		{
            _allocationProfile = aAllocationProfile;
            _storeIdxRID = aStoreIdxRID;
		}

        /// <summary>
        /// Gets the AllocationProfile where the item has been manually allocated.
        /// </summary>
        internal AllocationProfile AllocationProfile
        {
            get { return _allocationProfile; }
        }

        /// <summary>
        /// Gets the store Index_RID where the item has been manually allocated.
        /// </summary>
        internal Index_RID StoreIdxRID
        {
            get { return _storeIdxRID; }
        }
    }

    internal class SummaryNodeItemManuallyAllocated : ItemManuallyAllocated
    {
        private eAllocationSummaryNode _summaryNode;

        internal SummaryNodeItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, eAllocationSummaryNode aSummaryNode)
            : base(aAllocationProfile, aStoreIdxRID)
        {
            _summaryNode = aSummaryNode;
        }

        /// <summary>
        /// Gets the summary node type where the item has been manually allocated.
        /// </summary>
        internal eAllocationSummaryNode SummaryNode
        {
            get { return _summaryNode; }
        }
    }

    internal class PackItemManuallyAllocated : ItemManuallyAllocated
    {
        private string _packName;

        internal PackItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, string aPackName)
            : base(aAllocationProfile, aStoreIdxRID)
        {
            _packName = aPackName;
        }

        /// <summary>
        /// Gets the pack name where the item has been manually allocated.
        /// </summary>
        internal string PackName
        {
            get { return _packName; }
        }
    }

    internal class ColorItemManuallyAllocated : ItemManuallyAllocated
    {
        private int _colorKey;

        internal ColorItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, int aColorKey)
            : base(aAllocationProfile, aStoreIdxRID)
        {
            _colorKey = aColorKey;
        }

        /// <summary>
        /// Gets the color key where the item has been manually allocated.
        /// </summary>
        internal int ColorKey
        {
            get { return _colorKey; }
        }
    }

    internal class ColorSizeItemManuallyAllocated : ItemManuallyAllocated
    {
        private int _colorKey;
        private int _sizeKey;

        internal ColorSizeItemManuallyAllocated(AllocationProfile aAllocationProfile, Index_RID aStoreIdxRID, int aColorKey, int aSizeKey)
            : base(aAllocationProfile, aStoreIdxRID)
        {
            _colorKey = aColorKey;
            _sizeKey = aSizeKey;
        }

        /// <summary>
        /// Gets the color key where the item has been manually allocated.
        /// </summary>
        internal int ColorKey
        {
            get { return _colorKey; }
        }

        /// <summary>
        /// Gets the size key where the item has been manually allocated.
        /// </summary>
        internal int SizeKey
        {
            get { return _sizeKey; }
        }
    }
}
