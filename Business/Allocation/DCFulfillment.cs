using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
    /// <summary>
    /// Defines the general criteria that drives the allocation process.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DCFulfillmentMethod : AllocationBaseMethod
    {
        //=======
        // FIELDS
        //=======
        private DCFulfillmentMethodData _methodData;
        private eDCFulfillmentSplitOption _splitOption = eDCFulfillmentSplitOption.DCFulfillment;
        private bool _applyMinimumsInd = false;
        private char _prioritizeType = 'C';
        private int _headerField = Include.Undefined;
        private int _hcg_RID = Include.NoRID;
        private eDCFulfillmentHeadersOrder _headersOrder = eDCFulfillmentHeadersOrder.Ascending;
        private eDCFulfillmentStoresOrder _storesOrder = eDCFulfillmentStoresOrder.Ascending;
        private eDCFulfillmentSplitByOption _split_By_Option = eDCFulfillmentSplitByOption.SplitByDC;
        private eDCFulfillmentReserve _split_By_Reserve = eDCFulfillmentReserve.ReservePreSplit;
        private eDCFulfillmentMinimums _apply_By = eDCFulfillmentMinimums.ApplyFirst;
        private eDCFulfillmentWithinDC _within_Dc = eDCFulfillmentWithinDC.Proportional;
        private eHeaderCharType _fieldDataType = eHeaderCharType.text;
        private DataTable _dtStoreOrder;
        private Audit _audit;

        //=============
        // CONSTRUCTORS
        //=============
        public DCFulfillmentMethod(SessionAddressBlock SAB, int aMethodRID)
            : base(SAB, aMethodRID, eMethodType.DCFulfillment, eProfileType.MethodDCFulfillment)
        {
            if (base.Filled)
            {
                _methodData = new DCFulfillmentMethodData(base.Key, eChangeType.populate);
                _splitOption = _methodData.SplitOption;
                _applyMinimumsInd = _methodData.ApplyMinimumsInd;
                _prioritizeType = _methodData.PrioritizeType;
                _headerField = _methodData.HeaderField;
                _hcg_RID = _methodData.Hcg_RID;
                _headersOrder = _methodData.HeadersOrder;
                _storesOrder = _methodData.StoresOrder;
                _dtStoreOrder = _methodData.dtStoreOrder;
                _fieldDataType = _methodData.FieldDataType;
                _split_By_Option = _methodData.Split_By_Option;
                _split_By_Reserve = _methodData.Split_By_Reserve;
                _apply_By = _methodData.Apply_By;
                _within_Dc = _methodData.Within_Dc;
            }
            else
            {
                _methodData = new DCFulfillmentMethodData(base.Key, eChangeType.populate);
                _splitOption = _methodData.SplitOption;
                _applyMinimumsInd = _methodData.ApplyMinimumsInd;
                _prioritizeType = _methodData.PrioritizeType;
                _headerField = _methodData.HeaderField;
                _hcg_RID = _methodData.Hcg_RID;
                _headersOrder = _methodData.HeadersOrder;
                _storesOrder = _methodData.StoresOrder;
                _dtStoreOrder = _methodData.dtStoreOrder;
                _fieldDataType = _methodData.FieldDataType;
                _split_By_Option = _methodData.Split_By_Option;
                _split_By_Reserve = _methodData.Split_By_Reserve;
                _apply_By = _methodData.Apply_By;
                _within_Dc = _methodData.Within_Dc;
                ApplySystemDefaults();
            }
        }

        //============
        // PROPERTIES
        //============
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodDCFulfillment;
            }
        }

        public eDCFulfillmentSplitOption SplitOption
        {
            get { return _splitOption; }
            set { _splitOption = value; }
        }

        public bool ApplyMinimumsInd
        {
            get { return _applyMinimumsInd; }
            set { _applyMinimumsInd = value; }
        }

        public char PrioritizeType
        {
            get { return _prioritizeType; }
            set { _prioritizeType = value; }
        }

        public int PrioritizeBy
        {
            get 
            {
                if (PrioritizeType == 'H')
                {
                    return _headerField;
                }
                else
                {
                    return _hcg_RID;
                }
            }

        }

        public int HeaderField
        {
            get { return _headerField; }
            set { _headerField = value; }
        }

        public int HeaderFieldIndex
        {
            get { return _headerField * -1; }
        }

        public int Hcg_RID
        {
            get { return _hcg_RID; }
            set { _hcg_RID = value; }
        }

        public eDCFulfillmentHeadersOrder HeadersOrder
        {
            get { return _headersOrder; }
            set { _headersOrder = value; }
        }

        public eDCFulfillmentStoresOrder StoresOrder
        {
            get { return _storesOrder; }
            set { _storesOrder = value; }
        }

        public eDCFulfillmentSplitByOption Split_By_Option
        {
            get { return _split_By_Option; }
            set { _split_By_Option = value; }
        }

        public eDCFulfillmentReserve Split_By_Reserve
        {
            get { return _split_By_Reserve; }
            set { _split_By_Reserve = value; }
        }

        public eDCFulfillmentMinimums Apply_By
        {
            get { return _apply_By; }
            set { _apply_By = value; }
        }

        public eDCFulfillmentWithinDC Within_Dc
        {
            get { return _within_Dc; }
            set { _within_Dc = value; }
        }

        public eHeaderCharType FieldDataType
        {
            get { return _fieldDataType; }
            set { _fieldDataType = value; }
        }

        public DataTable dtStoreOrder
        {
            get { return _dtStoreOrder; }
            set { _dtStoreOrder = value; }
        }

        //========
        // METHODS
        //========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        private void ApplySystemDefaults()
        {
            GlobalOptionsProfile gop = SAB.ApplicationServerSession.GlobalOptions;

            _splitOption = gop.Split_Option;
            _applyMinimumsInd = Include.ConvertCharToBool(gop.Apply_Minimum_IND);
            _prioritizeType = gop.Prioritize_Type;
            _headerField = gop.Header_Field;
            _hcg_RID = gop.HCG_Rid;
            _headersOrder = gop.Header_Order;
            _storesOrder = gop.STORE_ORDER;
            _split_By_Option = gop.Split_BY_Option;
            _split_By_Reserve = gop.Split_By_Reserve;
            _apply_By = gop.Apply_By;
            _within_Dc = gop.Within_Dc;

            GlobalOptions opts = new GlobalOptions();
            DataTable dtdcf = opts.GetDCFStoreOrderInfo(1);
            if (dtdcf.Rows.Count > 0)
            {
                foreach (DataRow dr in dtdcf.Rows)
                {
                    DataRow newRow = _dtStoreOrder.NewRow();
                    newRow["METHOD_RID"] = -1;
                    newRow["SEQ"] = dr["SEQ"];
                    newRow["DIST_CENTER"] = dr["DIST_CENTER"];
                    newRow["SCG_RID"] = dr["SCG_RID"];
                    _dtStoreOrder.Rows.Add(newRow);
                }
            }
        }

        public override void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction,
            int aStoreFilter, Profile methodProfile)
        {
            WriteBeginProcessingMessage();

            aApplicationTransaction.ResetAllocationActionStatus();

            foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
            {
                AllocationWorkFlowStep awfs =
                    new AllocationWorkFlowStep(
                    this,
                    new GeneralComponent(eGeneralComponentType.Total),
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
                    Include.NoRID);
            }

            WriteEndProcessingMessage(aApplicationTransaction);
        }

        /// <summary>
        /// Processes the action associated with this method.
        /// </summary>
        /// <param name="aSAB">Session Address Block</param>
        /// <param name="aApplicationTransaction">An instance of the Application Transaction object</param>
        /// <param name="aAllocationWorkFlowStep">Workflow Step that describes parameters associated with this action.</param>
        /// <param name="aAllocationProfile">Allocation Profile to which to apply this action</param>
        /// <param name="WriteToDB">True: write results of action to database; False: Do not write results of action to database.</param>
        public override void ProcessAction(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aAllocationWorkFlowStep,
            Profile aAllocationProfile,
            bool WriteToDB,
            int aStoreFilterRID)
        {
            string message;
            MasterHeaderProfile mhp = null;
            AllocationProfileList subordinateList = null;
            bool actionSuccess = true;
            bool lockedHeaders = false;
            AllocationProfile ap = aAllocationProfile as AllocationProfile;
            _audit = aSAB.ApplicationServerSession.Audit;
            List<string> DCOrder = null;
            Dictionary<string, List<int>> DCStoreOrder = null;
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder = null;
            List<int> headerEnqueueList = new List<int>();

            if (ap == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                _audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }

            try
            {
                aApplicationTransaction.ClearInstanceTracker();
                ap.ResetTempLocks(false);
                if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aAllocationWorkFlowStep._method.MethodType))
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_WorkflowTypeInvalid),
                        MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
                }

                // Is Header Valid for DC Fulfillment?
                bool isOKToSkip;
                if (isValidHeader(ap, aApplicationTransaction, aAllocationWorkFlowStep, out isOKToSkip))
                {
                    // Get Master Header and subordinates
                    mhp = aApplicationTransaction.GetMasterHeaderProfile(ap.Key);
                    

                    // Enqueue subordinate headers not already locked
                    foreach (int subordinateRID in mhp.SubordinateRIDs)
                    {
                        if (!aApplicationTransaction.IsHeaderEnqueued(subordinateRID))
                        {
                            headerEnqueueList.Add(subordinateRID);
                        }
                    }

                    if (headerEnqueueList.Count > 0)
                    {
                        if (!aApplicationTransaction.EnqueueHeaders(headerEnqueueList, out message))
                        {
                            _audit.Add_Msg(
                                eMIDMessageLevel.Warning,
                                message,
                                this.GetType().Name);
                            actionSuccess = false;
                        }
                    }

                    if (actionSuccess)	
                    {
                        mhp.ProcessingMasterHeader = true;
                        subordinateList = mhp.GetSubordinates(aApplicationTransaction, mhp);
                        // must reread headers to insure have latest data and to set flag that header was enqueued at read time
                        foreach (AllocationProfile subordinate in subordinateList)
                        {
                            subordinate.ReReadHeader();
                            subordinate.HeaderDataRecord = mhp.HeaderDataRecord;
                            subordinate.ProcessingMasterHeader = true;
                        }
                        lockedHeaders = true;
                        //==================================
                        // Finally Do DC Fulfillment
                        //==================================
                        int workFlowRID = ap.API_WorkflowRID;
                        bool workFlowTrigger = ap.API_WorkflowTrigger;
                        ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList();

                        //=================================================
                        // Determine the order of the stores for each DC
                        //=================================================
                        // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                        bool allHeadersContainDC = true;
                        bool allDCsInMethod = true;
                        // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                        DCOrder = BuildDCOrder(ref actionSuccess);
                        if (actionSuccess)
                        {
                            // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                            //DCHeaderOrder = BuildDCHeaderOrder(subordinateList,  ref actionSuccess);
                            //DCStoreOrder = BuildDCStoreOrder(subordinateList, storeList, DCHeaderOrder);
                            DCHeaderOrder = BuildDCHeaderOrder(subordinateList, DCOrder, ref allHeadersContainDC, ref allDCsInMethod);
                            if (allHeadersContainDC
                                && allDCsInMethod)
                            {
                                DCStoreOrder = BuildDCStoreOrder(subordinateList, storeList, DCHeaderOrder);
                            }
                            else
                            {
                                actionSuccess = false;
                            }
                            // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance 
                        }

                        // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                        //if (!actionSuccess)
                        if (!allHeadersContainDC)
                        // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                        {
                            string msg = string.Format(
                            _audit.GetText(eMIDTextCode.msg_al_HeaderMustHaveDCAssigned, false));
                            _audit.Add_Msg(
                                eMIDMessageLevel.Error, eMIDTextCode.msg_al_HeaderMustHaveDCAssigned,
                                "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                                this.GetType().Name);
                            actionSuccess = false;   // TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                        }

                        if (actionSuccess)
                        {
                            try
                            {
                                actionSuccess = mhp.ProcessDCFulfillment(SAB, aApplicationTransaction, SplitOption, ApplyMinimumsInd, Split_By_Option, Split_By_Reserve, Apply_By, Within_Dc, subordinateList, DCOrder, DCStoreOrder, DCHeaderOrder, this);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            if (actionSuccess)
                            {
                                SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionCompletedSuccessfully, ap.Key, subordinateList);

                                aApplicationTransaction.WriteAllocationAuditInfo
                                    (mhp.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null,
                                    0,
                                    0
                                    );
                                foreach (AllocationProfile subordinate in subordinateList)
                                {
                                    aApplicationTransaction.WriteAllocationAuditInfo
                                    (subordinate.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null,
                                    0,
                                    0
                                    );
                                }
                            }
                            else
                            {
                                SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionFailed, mhp.Key, subordinateList);
                            }
                        }
                        else
                        {
                            SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionFailed, mhp.Key, subordinateList);
                        }
                    }
                    else
                    {
                        SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionFailed, mhp.Key, subordinateList);
                    }
                }
                else
                {
                    if (aAllocationWorkFlowStep.Key == Include.NoRID ||
                        !isOKToSkip)
                    {
                        SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionFailed, ap.Key, subordinateList);
                    }
                    else
                    {
                        SetAllocationActionStatus(aApplicationTransaction, eAllocationActionStatus.ActionCompletedSuccessfully, ap.Key, subordinateList);
                    }
                }
            }
            catch (Exception error)
            {
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                foreach (AllocationProfile subordinate in subordinateList)
                {
                    aApplicationTransaction.SetAllocationActionStatus(subordinate.Key, eAllocationActionStatus.ActionFailed);
                }
                message = error.ToString();
                throw;
            }
            finally
            {
                ap.ResetTempLocks(true);
                ap.RebuildSubtotals = true;
                aApplicationTransaction.RemoveAllocationProfileFromGrandTotal(ap);
                if (mhp != null
                    && lockedHeaders
                    && headerEnqueueList.Count > 0)
                {
                    // Only dequeue headers locked by DCF
                    aApplicationTransaction.DequeueHeaders(headerEnqueueList);
                }
                if (mhp != null)
                {
                    mhp.ProcessingMasterHeader = false;
                    subordinateList = mhp.GetSubordinates(aApplicationTransaction, mhp);
                    foreach (AllocationProfile subordinate in subordinateList)
                    {
                        subordinate.ProcessingMasterHeader = false;
                    }
                }
            }
        }

        private void SetAllocationActionStatus (ApplicationSessionTransaction aApplicationTransaction, eAllocationActionStatus allocationActionStatus, int Key, AllocationProfileList subordinateList)
        {
            aApplicationTransaction.SetAllocationActionStatus(Key, allocationActionStatus);
            if (subordinateList != null)
            {
                foreach (AllocationProfile subordinate in subordinateList)
                {
                    aApplicationTransaction.SetAllocationActionStatus(subordinate.Key, allocationActionStatus);
                }
            }
        }

        private bool isValidHeader(AllocationProfile ap, ApplicationSessionTransaction aApplicationTransaction, ApplicationWorkFlowStep aAllocationWorkFlowStep, out bool isOKToSkip)
        {
            bool isValid = true;
            isOKToSkip = false; 
            try
            {
                //===================================
                // Header must be Master Header
                //===================================
                if (!ap.IsMasterHeader)
                {
                    isValid = false;
                    if (aAllocationWorkFlowStep.Key == Include.NoRID)  // not part of workflow
                    {
                        string msg = string.Format(
                            _audit.GetText(eMIDTextCode.msg_al_MustBeMasterHeader, false));
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_MustBeMasterHeader,
                            "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }
                    else
                    {
                        isOKToSkip = true;
                    }
                }

                //===================================
                // Header must be All in Balance
                //===================================
                else if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance)
                {
                    isValid = false;
                    string msg = string.Format(
                        _audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_HeaderStatusDisallowsAction,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }

                //===================================
                // DC Fulfillment already processed
                //===================================
                else if (ap.DCFulfillmentProcessed)
                {
                    isValid = false;
                    string msg = string.Format(
                        _audit.GetText(eMIDTextCode.msg_al_DCFulfillmentAlreadyProcessed, false));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCFulfillmentAlreadyProcessed,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }

                if (!isOKToSkip)
                {
                    //========================================
                    // Intransit not charged to all components
                    //========================================
                    bool addIntransitError = false;
                    if ((ap.BulkColorCount > 0)
                        && (ap.StyleIntransitUpdated != ap.BulkColorIntransitUpdated))
                    {
                        addIntransitError = true;
                    }
                    else if (ap.HasSizes
                        && (ap.StyleIntransitUpdated != ap.BulkSizeIntransitUpdated))
                    {
                        addIntransitError = true;
                    }

                    if (addIntransitError)
                    {
                        isValid = false;
                        string msg = string.Format(
                            _audit.GetText(eMIDTextCode.msg_al_MasterHeaderIntransitNotCharged, false));
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_MasterHeaderIntransitNotCharged,
                            "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }
                }

                return isValid;
            }
            catch
            {
                throw;
            }

        }

        private List<string> BuildDCOrder(ref bool actionSuccess)
        {
            List<string> DCOrder = new List<string>();
            string distributionCenter;

            // Add distribution centers in order of the method
            foreach (DataRow dr in dtStoreOrder.Rows)
            {
                distributionCenter = Convert.ToString(dr["DIST_CENTER"]);
                DCOrder.Add(distributionCenter);
            }

            return DCOrder;
        }

        private Dictionary<string, List<int>> BuildDCStoreOrder(AllocationProfileList subordinateList, ProfileList storeList, Dictionary<string, List<AllocationProfile>> DCHeaderOrder)
        {
            Dictionary<string, List<int>> DCStoreOrder = new Dictionary<string, List<int>>();
            List<int> storeOrder;
            string distributionCenter;
            int SCG_RID = Include.NoRID;

            // Add distribution centers in order of the method
            foreach (DataRow dr in dtStoreOrder.Rows)
            {
                distributionCenter = Convert.ToString(dr["DIST_CENTER"]);
                SCG_RID = Convert.ToInt32(dr["SCG_RID"]);
                if (distributionCenter == null)
                {
                    distributionCenter = string.Empty;
                }
                // skip DC if not on any header
                if (DCHeaderOrder.ContainsKey(distributionCenter))
                {
                    if (!DCStoreOrder.ContainsKey(distributionCenter))
                    {
                        storeOrder = new List<int>();
                        DCStoreOrder.Add(distributionCenter, storeOrder);
                        BuildStoreOrder(distributionCenter, SCG_RID, storeList, storeOrder);
                    }
                    else
                    {
                        _audit.Add_Msg(
                                   eMIDMessageLevel.Warning, eMIDTextCode.msg_al_DCFulfillmentDuplicateDCIgnored,
                                   this.GetType().Name);
                    }
                }
            }

            // add any additional distribution centers not on the method
            foreach (AllocationProfile ap in subordinateList)
            {
                if (ap.DistributionCenter == null)
                {
                    ap.DistributionCenter = string.Empty;
                }
                if (!DCStoreOrder.ContainsKey(ap.DistributionCenter))
                {
                    _audit.Add_Msg(
                       eMIDMessageLevel.Information, eMIDTextCode.msg_al_DCFulfillmentDCOrderNotFound,
                       this.GetType().Name);
                    storeOrder = new List<int>();
                    DCStoreOrder.Add(ap.DistributionCenter, storeOrder);
                    BuildStoreOrder(ap.DistributionCenter, Include.NoRID, storeList, storeOrder);
                }
            }

            return DCStoreOrder;
        }

        private void BuildStoreOrder(string distributionCenter, int SCG_RID, ProfileList storeList, List<int> storeOrder)
        {
            SortedList<int, int> sortedList;
            if (StoresOrder == eDCFulfillmentStoresOrder.Ascending)
            {
                sortedList = new SortedList<int, int>(new AscendedIntComparer());
            }
            else
            {
                sortedList = new SortedList<int, int>(new DescendedIntComparer());
            }
            
            // Entry not found for DC.  Use store RID order
            if (SCG_RID == Include.NoRID)
            {
                foreach (StoreProfile sp in storeList)
                {
                    sortedList.Add(sp.Key, sp.Key);
                }
            }
            // Use order defined in the characteristic
            else
            {
                FilterData fd = new FilterData();
                DataTable dt = fd.StoreCharacteristicsGetValuesForFilter(SCG_RID);

                foreach (DataRow dr in dt.Rows)
                {
                    sortedList.Add(Convert.ToInt32(dr["CHAR_VALUE"]), Convert.ToInt32(dr["ST_RID"]));
                }
            }

            foreach (int ST_RID in sortedList.Values)
            {
                storeOrder.Add(ST_RID);
            }

            // add active stores not in the characteristic to the end
            if (SCG_RID != Include.NoRID)
            {
                foreach (StoreProfile sp in storeList)
                {
                    if (!storeOrder.Contains(sp.Key))
                    {
                        storeOrder.Add(sp.Key);
                    }
                }
            }
        }

        // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
        //private Dictionary<string, List<AllocationProfile>> BuildDCHeaderOrder(AllocationProfileList subordinateList, ref bool actionSuccess)
        private Dictionary<string, List<AllocationProfile>> BuildDCHeaderOrder(AllocationProfileList subordinateList, List<string> DCOrder, ref bool allHeadersContainDC, ref bool allDCsInMethod)
        // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
        {
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder = new Dictionary<string, List<AllocationProfile>>();
            List<AllocationProfile> headerOrder;

            // add any additional distribution centers not on the method
            foreach (AllocationProfile ap in subordinateList)
            {
                if (ap.DistributionCenter == null
                    || string.IsNullOrWhiteSpace(ap.DistributionCenter))
                {
                    ap.DistributionCenter = string.Empty;
                    // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
					//actionSuccess = false;
                    allHeadersContainDC = false;
                    // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                }
                // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                if (DCOrder.Contains(ap.DistributionCenter))
                {
                // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                    if (!DCHeaderOrder.ContainsKey(ap.DistributionCenter))
                    {
                        headerOrder = new List<AllocationProfile>();
                        DCHeaderOrder.Add(ap.DistributionCenter, headerOrder);
                        BuildHeaderOrder(ap.DistributionCenter, subordinateList, headerOrder);
                    }
                // Begin TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
                }
                else if (!string.IsNullOrEmpty(ap.DistributionCenter))
                {
                    allDCsInMethod = false;
                    string msg = string.Format(
                            _audit.GetText(eMIDTextCode.msg_al_MethodDoesNotContainHeaderDC, false, ap.DistributionCenter));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_MethodDoesNotContainHeaderDC,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                }
                // End TT#2112-MD - JSmith - Subordinate Headers are allocated out of balance
            }

            return DCHeaderOrder;
        }

        private void BuildHeaderOrder(string distributionCenter, AllocationProfileList subordinateList, List<AllocationProfile> headerOrder)
        {
            
            HeaderCharList hcl = new HeaderCharList();
            ArrayList headerCharList = hcl.BuildHeaderCharList();

            switch (FieldDataType)
            {
                case eHeaderCharType.number:
                case eHeaderCharType.dollar:
                    BuildHeaderOrderByNumber(distributionCenter, subordinateList, hcl, headerCharList, headerOrder);
                    break;
                case eHeaderCharType.date:
                    BuildHeaderOrderByDate(distributionCenter, subordinateList, hcl, headerCharList, headerOrder);
                    break;
                default:
                    BuildHeaderOrderByText(distributionCenter, subordinateList, hcl, headerCharList, headerOrder);
                    break;
            }
        }

        private void BuildHeaderOrderByNumber(string distributionCenter, AllocationProfileList subordinateList, HeaderCharList hcl, ArrayList headerCharList, List<AllocationProfile> headerOrder)
        {
            SortedList<double, List<AllocationProfile>> sortedList;
            double noNumber = double.MinValue;
            if (HeadersOrder == eDCFulfillmentHeadersOrder.Ascending)
            {
                sortedList = new SortedList<double, List<AllocationProfile>>(new AscendedDoubleComparer());
                noNumber = double.MinValue;
            }
            else
            {
                sortedList = new SortedList<double, List<AllocationProfile>>(new DescendedDoubleComparer());
                noNumber = double.MaxValue;
            }

            string HCG_ID = string.Empty;
            if (PrioritizeType == 'C')
            {
                HCG_ID = GetHeaderCharID(headerCharList);
            }

            foreach (AllocationProfile ap in subordinateList)
            {
                if (ap.DistributionCenter != distributionCenter)
                {
                    continue;
                }

                double hdrNumber;
                if (PrioritizeType == 'H')
                {
                    try
                    {
                        hdrNumber = Convert.ToDouble(hcl.GetHeaderField(SAB, filterHeaderFieldTypes.FromIndex(HeaderFieldIndex), ap));
                    }
                    catch (Exception)
                    {
                        hdrNumber = noNumber;
                    }
                }
                else
                {
                    try
                    {
                        hdrNumber = Convert.ToDouble(hcl.GetHeaderCharacteristic(SAB, HCG_ID, ap));
                    }
                    catch (Exception)
                    {
                        hdrNumber = noNumber;
                    }
                }

                List<AllocationProfile> al;
                if (sortedList.ContainsKey(hdrNumber))
                {
                    al = sortedList[hdrNumber];
                }
                else
                {
                    al = new List<AllocationProfile>();
                    sortedList.Add(hdrNumber, al);
                }
                al.Add(ap);
            }

            foreach (List<AllocationProfile> apl in sortedList.Values)
            {
                foreach (AllocationProfile ap in apl)
                {
                    headerOrder.Add(ap);
                }
            }
        }

        private void BuildHeaderOrderByDate(string distributionCenter, AllocationProfileList subordinateList, HeaderCharList hcl, ArrayList headerCharList, List<AllocationProfile> headerOrder)
        {
            SortedList<DateTime, List<AllocationProfile>> sortedList;
            DateTime noDate = DateTime.MinValue;
            if (HeadersOrder == eDCFulfillmentHeadersOrder.Ascending)
            {
                sortedList = new SortedList<DateTime, List<AllocationProfile>>(new AscendedDateComparer());
                noDate = DateTime.MinValue;
            }
            else
            {
                sortedList = new SortedList<DateTime, List<AllocationProfile>>(new DescendedDateComparer());
                noDate = DateTime.MaxValue;
            }

            string HCG_ID = string.Empty;
            if (PrioritizeType == 'C')
            {
                HCG_ID = GetHeaderCharID(headerCharList);
            }

            DateTime hdrDate;
            foreach (AllocationProfile ap in subordinateList)
            {
                if (ap.DistributionCenter != distributionCenter)
                {
                    continue;
                }

                if (PrioritizeType == 'H')
                {
                    try
                    {
                        hdrDate = Convert.ToDateTime(hcl.GetHeaderField(SAB, filterHeaderFieldTypes.FromIndex(HeaderFieldIndex), ap));
                    }
                    catch (Exception)
                    {
                        hdrDate = noDate;
                    }
                }
                else
                {
                    try
                    {
                        hdrDate = Convert.ToDateTime(hcl.GetHeaderCharacteristic(SAB, HCG_ID, ap));
                    }
                    catch (Exception)
                    {
                        hdrDate = noDate ;
                    }
                }

                List<AllocationProfile> al;
                if (sortedList.ContainsKey(hdrDate))
                {
                    al = sortedList[hdrDate];
                }
                else
                {
                    al = new List<AllocationProfile>();
                    sortedList.Add(hdrDate, al);
                }
                al.Add(ap);
            }

            foreach (List<AllocationProfile> apl in sortedList.Values)
            {
                foreach (AllocationProfile ap in apl)
                {
                    headerOrder.Add(ap);
                }
            }
        }

        private void BuildHeaderOrderByText(string distributionCenter, AllocationProfileList subordinateList, HeaderCharList hcl, ArrayList headerCharList, List<AllocationProfile> headerOrder)
        {
            SortedList<string, List<AllocationProfile>> sortedList;
            if (HeadersOrder == eDCFulfillmentHeadersOrder.Ascending)
            {
                sortedList = new SortedList<string, List<AllocationProfile>>(new AscendedStringComparer());
            }
            else
            {
                sortedList = new SortedList<string, List<AllocationProfile>>(new DescendedStringComparer());
            }

            string HCG_ID = string.Empty;
            if (PrioritizeType == 'C')
            {
                HCG_ID = GetHeaderCharID(headerCharList);
            }

            string hdrText;
            foreach (AllocationProfile ap in subordinateList)
            {
                if (ap.DistributionCenter != distributionCenter)
                {
                    continue;
                }


                if (PrioritizeType == 'H')
                {
                    try
                    {
                        hdrText = Convert.ToString(hcl.GetHeaderField(SAB, filterHeaderFieldTypes.FromIndex(HeaderFieldIndex), ap));
                    }
                    catch (Exception)
                    {
                        hdrText = string.Empty;
                    }
                }
                else
                {
                    try
                    {
                        hdrText = Convert.ToString(hcl.GetHeaderCharacteristic(SAB, HCG_ID, ap));
                    }
                    catch (Exception)
                    {
                        hdrText = string.Empty;
                    }
                }

                List<AllocationProfile> al;
                if (sortedList.ContainsKey(hdrText))
                {
                    al = sortedList[hdrText];
                }
                else
                {
                    al = new List<AllocationProfile>();
                    sortedList.Add(hdrText, al);
                }
                al.Add(ap);
            }

            foreach (List<AllocationProfile> apl in sortedList.Values)
            {
                foreach (AllocationProfile ap in apl)
                {
                    headerOrder.Add(ap);
                }
            }
        }

        private string GetHeaderCharID(ArrayList headerCharList)
        {
            string HCG_ID = string.Empty;

            foreach (HeaderFieldCharEntry hfce in headerCharList)
            {
                if (hfce.Type == 'C'
                    && hfce.Key == Hcg_RID )
                {
                    HCG_ID = hfce.Text;
                    break;
                }
            }

            return HCG_ID;
        }

        override public void Update(TransactionData td)
        {
            if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _methodData = new DCFulfillmentMethodData(td);
            }
            _methodData.SplitOption = SplitOption;
            _methodData.ApplyMinimumsInd = ApplyMinimumsInd;
            _methodData.PrioritizeType = PrioritizeType;
            _methodData.HeaderField = HeaderField;
            _methodData.Hcg_RID = Hcg_RID;
            _methodData.HeadersOrder = HeadersOrder;
            _methodData.StoresOrder = StoresOrder;
            _methodData.FieldDataType = FieldDataType;
            _methodData.Split_By_Option = _split_By_Option;
            _methodData.Split_By_Reserve = _split_By_Reserve;
            _methodData.Apply_By =  _apply_By;
            _methodData.Within_Dc = _within_Dc;
            _methodData.dtStoreOrder = dtStoreOrder;

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
                        // make sure the key in the data layer is the same
                        _methodData.MethodRid = base.Key;
                        _methodData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _methodData.DeleteMethod(base.Key, td);
                        base.Update(td);
                        break;
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
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
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        {
            DCFulfillmentMethod newAllocationGeneralMethod = null;

            try
            {
                newAllocationGeneralMethod = (DCFulfillmentMethod)this.MemberwiseClone();
                newAllocationGeneralMethod.SG_RID = SG_RID;
                newAllocationGeneralMethod.SplitOption = SplitOption;
                newAllocationGeneralMethod.ApplyMinimumsInd = ApplyMinimumsInd;
                newAllocationGeneralMethod.PrioritizeType = PrioritizeType;
                newAllocationGeneralMethod.HeaderField = HeaderField;
                newAllocationGeneralMethod.Hcg_RID = Hcg_RID;
                newAllocationGeneralMethod.HeadersOrder = HeadersOrder;
                newAllocationGeneralMethod.StoresOrder = StoresOrder;
                newAllocationGeneralMethod.dtStoreOrder = dtStoreOrder.Copy();

                return newAllocationGeneralMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            return true;
        }

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
            return true;
        }

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalDCFulfillment);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserDCFulfillment);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROMethodDCStoreCharacteristicSet dCStoreCharacteristicSet = new ROMethodDCStoreCharacteristicSet();
            ROMethodDCFulfillmentProperties method = new ROMethodDCFulfillmentProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                dCFulfillmentSplitOption: EnumTools.VerifyEnumValue(_methodData.SplitOption),
                applyMinimumsInd: _methodData.ApplyMinimumsInd,
                prioritizeType: GetName.GetPrioritizeHeaderBy(_methodData.PrioritizeType, _methodData.Hcg_RID, _methodData.HeaderField),
                headersOrder: EnumTools.VerifyEnumValue(_methodData.HeadersOrder),
                split_By_Option: EnumTools.VerifyEnumValue(_methodData.Split_By_Option),
                within_Dc: EnumTools.VerifyEnumValue(_methodData.Within_Dc),
                split_By_Reserve: EnumTools.VerifyEnumValue(_methodData.Split_By_Reserve),
                storesOrder: _methodData.StoresOrder,
                dCStoreCharacteristicSet: DCStoreCharacteristicSet.BuildDCStoreCharacteristicSet(_methodData.Method_RID, eMethodType.DCFulfillment, dtStoreOrder, SAB),
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
            ROMethodDCFulfillmentProperties rOMethodDCFulfillmentProperties = (ROMethodDCFulfillmentProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                Method_Description = rOMethodDCFulfillmentProperties.Description;
                SplitOption = rOMethodDCFulfillmentProperties.DCFulfillmentSplitOption;
                ApplyMinimumsInd = rOMethodDCFulfillmentProperties.ApplyMinimumsInd;
                if (_methodData.Hcg_RID != Include.NoRID)
                {
                    Hcg_RID = rOMethodDCFulfillmentProperties.PrioritizeType.Key;
                }
                else
                {
                    HeaderField = rOMethodDCFulfillmentProperties.PrioritizeType.Key;
                }
                HeadersOrder = rOMethodDCFulfillmentProperties.HeadersOrder;
                Split_By_Option = rOMethodDCFulfillmentProperties.Split_By_Option;
                Within_Dc = rOMethodDCFulfillmentProperties.Within_Dc;
                Split_By_Reserve = rOMethodDCFulfillmentProperties.Split_By_Reserve;
                StoresOrder = rOMethodDCFulfillmentProperties.StoresOrder;
                dtStoreOrder = DCStoreCharacteristicSet.BuildDTStoreOrder(_methodData.Method_RID, rOMethodDCFulfillmentProperties.DCStoreCharacteristicSet, dtStoreOrder, SAB);
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

