using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business.Allocation 
{
    public class NeedAction
    {
        #region Fields
        private SessionAddressBlock _sab;
        private ApplicationSessionTransaction _tran;
        private Session _session;
        private Audit _audit;
        //private NeedAlgorithms _needAlgorithm;  // TT#488 - MD - Jellis - Group Allocation (field not used)
        private GlobalOptionsProfile _globalOptions;
        private string _infoMessage;
        #endregion Fields

        #region Constructors
        public NeedAction(ApplicationSessionTransaction aAppSessTran, Session aSession)
        {
            _tran = aAppSessTran;
            _session = aSession;
            _audit = _session.Audit;
            _sab = _tran.SAB;
            _globalOptions = _tran.GlobalOptions;
        }
        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Need Allocation
        /// <summary>
        /// Allocate a header or Group of Headers (Assortment) using the Need Algorithm
        /// </summary>
        /// <param name="aAllocationProfile">Allocation or Assortment Profile to be allocated</param>
        /// <param name="aGeneralComponent">Component on which a need allocation is to occur</param>
        /// <param name="aStoreFilterRID">Store Filter identifying the stores for the allocation</param>
        /// <param name="aStatusMsg">Status Message</param>
        /// <returns>True: Allocation was successful--null status message; False: Allocation was not successful, status message will contain a reason for failure</returns>
        public bool NeedAllocation(AllocationProfile aAllocationProfile, GeneralComponent aGeneralComponent, int aStoreFilterRID, out MIDException aStatusMsg)
        {
            bool actionStatus;
            _infoMessage = string.Format(_session.Audit.GetText(eMIDTextCode.msg_al_AllocationNeedActionBegins, false), aAllocationProfile.HeaderID.ToString(), MIDText.GetTextOnly(eMIDTextCode.lbl_Header));
            _session.Audit.Add_Msg(
                MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_AllocationNeedActionBegins),
                eMIDTextCode.msg_al_AllocationNeedActionBegins,
                _infoMessage,
                this.GetType().Name);

            ProfileList storeList;
            actionStatus = TryGetFilteredStoreList(aAllocationProfile, aStoreFilterRID, out storeList, out aStatusMsg); 
            if (actionStatus)
            {
                actionStatus = ActionValid(aAllocationProfile, out aStatusMsg);
                if (actionStatus)
                {
                    actionStatus =
                        ProcessNeedAction(
                            aAllocationProfile,
                            aGeneralComponent,
                            storeList,
                            out aStatusMsg);
                }
                
            }
            if (!actionStatus)
            {
                _session.Audit.Add_Msg(
                       Include.TranslateErrorLevel(aStatusMsg.ErrorLevel),
                       (eMIDTextCode)aStatusMsg.ErrorNumber,
                       aStatusMsg.ErrorMessage,
                       GetType().Name,
                       true);
            }
            _infoMessage = 
                string.Format(
                    _session.Audit.GetText(eMIDTextCode.msg_al_AllocationNeedActionEnds, false), 
                    aAllocationProfile.HeaderID.ToString(), 
                    MIDText.GetTextOnly(eMIDTextCode.lbl_Header));
            _session.Audit.Add_Msg(
                MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_AllocationNeedActionEnds),
                eMIDTextCode.msg_al_AllocationNeedActionEnds,
                _infoMessage,
                this.GetType().Name);
            return actionStatus;
        }
        #endregion Need Allocation

        #region Store Filter
        /// <summary>
        /// Trys to get the list of stores for an allocation
        /// </summary>
        /// <param name="aAllocationProfile">Allocation Profile which is to be allocated</param>
        /// <param name="aStoreFilterRID">Store Filter that identifies the stores</param>
        /// <param name="aStoreList">Returned Store Profile List </param>
        /// <param name="aStatusMsg">Status Message</param>
        /// <returns>True: Store Filter was good and list of store profiles is returned; False: Store Filter was not valid or some other failure, no list of stores returned but status message will contain a reason for failure</returns>
        private bool TryGetFilteredStoreList(AllocationProfile aAllocationProfile, int aStoreFilterRID, out ProfileList aStoreList, out MIDException aStatusMsg)
        {
            bool outdatedFilter = false;
            aStoreList = _tran.GetAllocationFilteredStoreList(aAllocationProfile, aStoreFilterRID, ref outdatedFilter);
            if (outdatedFilter)
            {
                FilterData storeFilterData = new FilterData();
                string filterName = storeFilterData.FilterGetName(aStoreFilterRID);
                string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                msg = msg.Replace("{0}", filterName);
                string suffix;
                if (aAllocationProfile.HeaderType == eHeaderType.Assortment)
                {
                    suffix = " - NeedAction. " + MIDText.GetTextOnly(eMIDTextCode.lbl_Header) + " [" + aAllocationProfile.HeaderID + "]";
                }
                else
                {
                    suffix = " - NeedAction. " + MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment) + " [" + aAllocationProfile.HeaderID + "]";
                }
                string auditMsg = msg + suffix;
                _session.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.GetType().Name);
                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
            }
            aStatusMsg = null;
            if (aStoreList.Count <= 0)
            {
                eMIDMessageLevel msgLevel = MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_NoStoresToProcess);
                string errorMsg =
                    _session.Audit.GetText(eMIDTextCode.msg_al_NoStoresToProcess, false)
                    + " " + aAllocationProfile.HeaderID.ToString();
                if (aAllocationProfile.HeaderType == eHeaderType.Assortment)
                {
                    errorMsg =
                        errorMsg
                        + MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment);
                }
                else
                {
                    errorMsg =
                         errorMsg
                         + MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
                }
                errorMsg =
                    errorMsg
                    + " " + "Action"
                    + " " + MIDText.GetTextOnly((int)eAllocationActionType.StyleNeed);
                aStatusMsg = new MIDException(
                     Include.TranslateMessageLevel(msgLevel),
                    (int)eMIDTextCode.msg_al_NoStoresToProcess,
                errorMsg); _session.Audit.Add_Msg(
                    msgLevel,
                    eMIDTextCode.msg_al_NoStoresToProcess,
                    errorMsg,
                    this.GetType().Name);
                return false;
            }
            return true;
        }
        #endregion Store Filter

        #region Validate Allocation
        /// <summary>
        /// Validates Allocation
        /// </summary>
        /// <param name="aAllocationProfile">Assortment or Allocation Profile for which there is a an allocation request</param>
        /// <param name="aStatusMsg">When an action is not valid, aStatusMsg gives a reason why it is not valid.</param>
        /// <returns>True:  Allocation Action is valid; False:  Allocation Action is not valid, in this case, aStatusMsg gives a reason why</returns>
        private bool ActionValid(AllocationProfile aAllocationProfile, out MIDException aStatusMsg)
        {
            aStatusMsg = null;
            bool actionValid = false;
            eMIDMessageLevel msgLevel;
            string errorMsg;
            AssortmentProfile assortmentProfile = null;
            AllocationProfile[] allocationProfileList = null; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in ActionValid()");
            }
            #endif	
            //if (aAllocationProfile is AssortmentProfile)
            if (aAllocationProfile.isAssortmentProfile)
            // End TT#4988 - BVaughan - Performance
            {
                assortmentProfile = (AssortmentProfile)aAllocationProfile;
                allocationProfileList = assortmentProfile.AssortmentMembers;
            }
            else
            {
                allocationProfileList = new AllocationProfile[1];
                allocationProfileList[0] = aAllocationProfile; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            }
            if (allocationProfileList != null
                 && allocationProfileList.Length > 0) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
            {
                foreach (AllocationProfile ap in allocationProfileList)
                {
                    if (ap.StyleIntransitUpdated
                        || ap.BulkSizeIntransitUpdated)
                    {
                        msgLevel = MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_StyleItChargedActionNotValid);
                        errorMsg = string.Format(_session.Audit.GetText(
                            eMIDTextCode.msg_al_StyleItChargedActionNotValid),
                            MIDText.GetTextOnly(eMIDTextCode.lbl_Header) + " Action",
                            MIDText.GetTextOnly((int)eAllocationActionType.StyleNeed),
                            ap.HeaderID);
                        _session.Audit.Add_Msg(
                            msgLevel,
                            eMIDTextCode.msg_al_StyleItChargedActionNotValid,
                            errorMsg,
                            this.GetType().Name,
                            true);
                        if (assortmentProfile == null)
                        {
                            aStatusMsg =
                                new MIDException(
                                    Include.TranslateMessageLevel(msgLevel),
                                    (int)eMIDTextCode.msg_al_StyleItChargedActionNotValid,
                                    errorMsg);
                        }
                    }
                    else
                    {
                        actionValid = true;
                        break;
                    }
                }
                if (!actionValid
                    && assortmentProfile != null)
                {
                    msgLevel = MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_StyleItChargedActionNotValid);
                    errorMsg = string.Format(_session.Audit.GetText(
                        eMIDTextCode.msg_al_StyleItChargedActionNotValid),
                        MIDText.GetTextOnly(eMIDTextCode.lbl_Header) + " Action",
                        MIDText.GetTextOnly((int)eAllocationActionType.StyleNeed),
                        assortmentProfile.HeaderID);
                    aStatusMsg = new MIDException(
                        Include.TranslateMessageLevel(msgLevel),
                        (int)eMIDTextCode.msg_al_StyleItChargedActionNotValid,
                        errorMsg);
                    _session.Audit.Add_Msg(
                        msgLevel,
                        eMIDTextCode.msg_al_StyleItChargedActionNotValid,
                        errorMsg,
                        this.GetType().Name,
                        true);
                }
            }
            else
            {
                string name = GetType().Name;
                actionValid = false;
                msgLevel = MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_NoHeadersToProcess);
                errorMsg = string.Format(_session.Audit.GetText(
                    eMIDTextCode.msg_al_NoHeadersToProcess),
                    name,
                    "ActionValid");
                _session.Audit.Add_Msg(
                    msgLevel,
                    eMIDTextCode.msg_al_NoHeadersToProcess,
                    errorMsg,
                    name,
                    true);
            }
            return true;
        }
        #endregion Validate Allocation


        #region Process Need Action
        /// <summary>
        /// Sets up and executes an Allocation by Style Need
        /// </summary>
        /// <param name="aAllocationProfile">Assortment or Allocation Profile for which a need allocation is requested</param>
        /// <param name="aComponent">Allocation General Component to allocate</param>
        /// <param name="aStoreList">Stores to include in the allocation</param>
        /// <param name="aStatusMsg"></param>
        /// <returns>Updates store pack and/or color allocations</returns>
        private bool ProcessNeedAction
            (
            AllocationProfile aAllocationProfile,
            GeneralComponent aComponent,
            ProfileList aStoreList,
            out MIDException aStatusMsg
            )
        {

            bool actionSuccess = true;
            aStatusMsg = null;
            NeedAlgorithms needAlgorithms;
            GeneralComponent allocationComponent;

            try
            {
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in ProcessNeedAction()");
                }
                #endif
                //if (aAllocationProfile is AssortmentProfile)
                if (aAllocationProfile.isAssortmentProfile)
                // End TT#4988 - BVaughan - Performance
                {
                    AssortmentProfile assortmentProfile = (AssortmentProfile)aAllocationProfile;
                    actionSuccess =
                        BuildGroupAllocationNodes(
                            assortmentProfile,
                            aComponent,
                            aStoreList,
                            out needAlgorithms,
                            out allocationComponent,
                            out aStatusMsg);
                }
                else
                {
                    actionSuccess =
                        BuildHeaderAllocationNodes(
                            aAllocationProfile,
                            aComponent,
                            aStoreList,
                            out needAlgorithms,
                            out allocationComponent,
                            out aStatusMsg);
                }

                if (actionSuccess)
                {
                    IntransitKeyType ikt;
                    Hashtable iktHash = new Hashtable();
                    Dictionary<long, PackHdr> packsDictionary = new Dictionary<long, PackHdr>();
                    if (allocationComponent.ComponentType == eComponentType.SpecificColor)
                    {
                        AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)allocationComponent;
                        ikt = new IntransitKeyType(colorComponent.ColorRID, Include.IntransitKeyTypeNoSize);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                    }
                    else if (allocationComponent.ComponentType == eComponentType.SpecificPack)
                    {
                        AllocationPackComponent packComponent = (AllocationPackComponent)allocationComponent;
                        AllocationProfile[] apList; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        // Begin TT#4988 - BVaughan - Performance
                        #if DEBUG
                        if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                        {
                            throw new Exception("Object does not match AssortmentProfile in ProcessNeedAction()");
                        }
                        #endif	
                        //if (aAllocationProfile is AssortmentProfile)
                        if (aAllocationProfile.isAssortmentProfile)
                        // End TT#4988 - BVaughan - Performance
                        {
                            apList = ((AssortmentProfile)aAllocationProfile).AssortmentMembers;
                        }
                        else
                        {
                            apList = new AllocationProfile[1]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                            apList[0] = aAllocationProfile;   // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        }
                        List<int> colorRID = new List<int>();
                        foreach (AllocationProfile ap in apList)
                        {
                            if (ap.Packs.Contains(packComponent.PackName))
                            {
                                PackHdr packHdr = ap.GetPackHdr(packComponent.PackName);
                                packsDictionary.Add((ap.HeaderRID << 32) + (long)packHdr.PackRID, packHdr);
                                if (packHdr.PackColorCount > 0)
                                {
                                    foreach (PackColorSize pcs in packHdr.PackColors.Values)
                                    {
                                        colorRID.Add(pcs.ColorCodeRID);
                                    }
                                }
                                else
                                {
                                    ikt = new IntransitKeyType(0, 0);
                                    iktHash.Add(ikt.IntransitTypeKey, ikt);
                                    colorRID.Clear();
                                    break;
                                }
                            }
                        }
                        if (colorRID.Count > 0)
                        {
                            colorRID.Sort();
                            int ridIndex = 0;
                            int lastRid = colorRID[0];
                            while (ridIndex < colorRID.Count)
                            {
                                ikt = new IntransitKeyType(colorRID[ridIndex], Include.IntransitKeyTypeNoSize);
                                iktHash.Add(ikt.IntransitType, ikt);
                                while (ridIndex < colorRID.Count
                                       && ikt.ColorRID == colorRID[ridIndex])
                                {
                                    ridIndex++;
                                }
                            }

                        }
                    }
                    else
                    {
                        ikt = new IntransitKeyType(0, 0);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                    }
                    actionSuccess = DoNeedAllocation
                        (aAllocationProfile,
                        allocationComponent,
                        packsDictionary,
                        iktHash,
                        needAlgorithms,
                        aStoreList,
                        out aStatusMsg);
                }
            }
            catch (MIDException e)
            {
                actionSuccess = false;
                aStatusMsg = e;
            }
            catch (Exception e)
            {
                actionSuccess = false;
                aStatusMsg =
                    new MIDException(
                        eErrorLevel.severe,
                        (int)eMIDTextCode.systemError,
                       e.Message, e); // TT#1173 - MD - JEllis - MID Error #20000 -- no explanation
            }
            finally
            {
            }
            return actionSuccess;
        }

        /// <summary>
        /// Gets an instance of the Need Algorithms class
        /// </summary>
        /// <param name="aAllocationProfile">Allocation Profile on which a need algorithm is to occur</param>
        /// <param name="aStoreList">List of store profiles to which merchandise is to be allocated</param>
        /// <returns></returns>
        private NeedAlgorithms GetNeedAlgorithms(AllocationProfile aAllocationProfile, ProfileList aStoreList)
        {
            NeedAlgorithms NA = new NeedAlgorithms(_tran, aStoreList.Count, "Stores");

            if (aAllocationProfile.PercentNeedLimit == Include.DefaultPercentNeedLimit)
            {
                aAllocationProfile.PercentNeedLimit = this._globalOptions.PercentNeedLimit;
            }
            NA.PercentNeedLimit = aAllocationProfile.PercentNeedLimit;
            NA.Multiple = aAllocationProfile.AllocationMultiple;
            return NA;
        }

        /// <summary>
        /// Builds/populates allocation node structure for a group allocation
        /// </summary>
        /// <param name="aAssortmentProfile">Assortment Profile whose nodes are to be built</param>
        /// <param name="aComponent">Assortment Component whose nodes are to be built</param>
        /// <param name="aStoreList">Stores to include in the allocation</param>
        /// <param name="aNeedAlgorithm">Need Algorithm Object on which the nodes were built</param>
        /// <param name="aAllocationComponent">Allocation Component for which the nodes were actually built</param>
        /// <param name="aStatusMsg">MID Exception Message describing issue when build fails</param>
        /// <returns>True: Build Successful; False: Build Not Successful</returns>
        private bool BuildGroupAllocationNodes(
            AssortmentProfile aAssortmentProfile,
            GeneralComponent aComponent,
            ProfileList aStoreList,
            out NeedAlgorithms aNeedAlgorithm,
            out GeneralComponent aAllocationComponent,
            out MIDException aStatusMsg)
        {
            Index_RID storeIndexRID;
            aNeedAlgorithm = GetNeedAlgorithms(aAssortmentProfile, aStoreList);
            bool buildSuccess = true;
            NodeComponent nodeComponent = aNeedAlgorithm.GetNodeComponent();
            aAllocationComponent = aComponent;
            aStatusMsg = null;
            // begin TT#1074 - MD - Group ALlocation - Inventory Min Max Broken
            Dictionary<int, List<NodeComponent>> inventoryNodes = new Dictionary<int,List<NodeComponent>>();
            Dictionary<int, List<NodeComponent>> capacityNodes = new Dictionary<int,List<NodeComponent>>();
 
            // end TT#1074 - MD - Group Allocation - Inventory Min Max Broken
            // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            int minimum;
            int maximum;
            eMIDTextCode statusReasonCode;
            // end TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            try
            {                
                nodeComponent.NodeType = eNeedAllocationNode.Group;
                nodeComponent.NodeDescription = aAssortmentProfile.HeaderID;
                nodeComponent.AllocateSubNodes = true;
                nodeComponent.NodeUnitsAllocated = 0;
                nodeComponent.NodeUnitsToAllocate = 0;
                AllocationProfile[] apList = aAssortmentProfile.AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                nodeComponent.SetSubNodeDimension(apList.Length); // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                Object allocationSummaryNode = null;
                GeneralComponent gc = null;
                
                switch (aComponent.ComponentType)
                {
                    case (eComponentType.SpecificColor):
                        {
                            gc = aComponent;
                            AllocationColorOrSizeComponent colorComponent = (AllocationColorOrSizeComponent)gc;
                            nodeComponent.NodeMultiple = 1;
                            foreach (AllocationProfile ap in apList)
                            {
                                HdrColorBin hcb = (HdrColorBin)ap.BulkColors[colorComponent.ColorRID];
                                if (hcb != null)
                                {
                                    nodeComponent.NodeUnitsAllocated += hcb.ColorUnitsAllocated;
                                    nodeComponent.NodeUnitsToAllocate += hcb.ColorUnitsToAllocate;
                                }
                            }
                            break;
                        }
                    case (eComponentType.SpecificPack):
                        {
                            gc = aComponent;
                            AllocationPackComponent packComponent = (AllocationPackComponent)gc;
                            nodeComponent.NodeMultiple = 1;
                            foreach (AllocationProfile ap in apList)
                            {
                                PackHdr ph = (PackHdr)ap.Packs[packComponent.PackName];
                                if (ph != null)
                                {
                                    nodeComponent.NodeUnitsAllocated += ph.PacksAllocated * ph.PackMultiple;
                                    nodeComponent.NodeUnitsToAllocate += ph.PacksToAllocate * ph.PackMultiple;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            switch (aComponent.ComponentType)
                            {
                                case (eComponentType.Total):
                                    {
                                        allocationSummaryNode = eAllocationSummaryNode.Total;
                                        //nodeComponent.NodeMultiple = aAssortmentProfile.AllocationMultiple;  // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                                        nodeComponent.NodeMultiple = 1;           // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                                        foreach (AllocationProfile ap in apList)
                                        {
                                            nodeComponent.NodeUnitsAllocated += ap.TotalUnitsAllocated;
                                            nodeComponent.NodeUnitsToAllocate += ap.TotalUnitsToAllocate;
                                        }
                                        break;
                                    }
                                case (eComponentType.GenericType):
                                case (eComponentType.AllGenericPacks):
                                    {
                                        aAllocationComponent = new GeneralComponent(eComponentType.GenericType);
                                        allocationSummaryNode = eAllocationSummaryNode.GenericType;
                                        nodeComponent.NodeMultiple = aAssortmentProfile.GenericMultiple;
                                        foreach (AllocationProfile ap in apList)
                                        {
                                            nodeComponent.NodeUnitsAllocated += ap.GenericUnitsAllocated;
                                            nodeComponent.NodeUnitsToAllocate += ap.GenericUnitsToAllocate;
                                        }
                                        break;
                                    }
                                case (eComponentType.DetailType):
                                    {
                                        allocationSummaryNode = eAllocationSummaryNode.DetailType;
                                        nodeComponent.NodeMultiple = aAssortmentProfile.DetailTypeMultiple;
                                        foreach (AllocationProfile ap in apList)
                                        {
                                            nodeComponent.NodeUnitsAllocated += ap.DetailTypeUnitsAllocated;
                                            nodeComponent.NodeUnitsToAllocate += ap.DetailTypeUnitsToAllocate;
                                        }
                                        break;
                                    }
                                case (eComponentType.DetailSubType):
                                    {
                                        allocationSummaryNode = eAllocationSummaryNode.DetailSubType;
                                        nodeComponent.NodeMultiple = aAssortmentProfile.DetailTypeMultiple;
                                        foreach (AllocationProfile ap in apList)
                                        {
                                            nodeComponent.NodeUnitsAllocated += ap.DetailTypeUnitsAllocated;
                                            nodeComponent.NodeUnitsToAllocate += ap.DetailTypeUnitsToAllocate;
                                        }
                                        break;
                                    }
                                case (eComponentType.Bulk):
                                case (eComponentType.AllColors):
                                    {
                                        aAllocationComponent = new GeneralComponent(eComponentType.Bulk);
                                        allocationSummaryNode = eAllocationSummaryNode.Bulk;
                                        nodeComponent.NodeMultiple = aAssortmentProfile.BulkMultiple;
                                        foreach (AllocationProfile ap in apList)
                                        {
                                            nodeComponent.NodeUnitsAllocated += ap.BulkUnitsAllocated;
                                            nodeComponent.NodeUnitsToAllocate += ap.BulkUnitsToAllocate;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        buildSuccess = false;
                                        aStatusMsg = 
                                            new MIDException(eErrorLevel.severe,
                                                (int)eMIDTextCode.msg_al_UnknownAllocationSummaryNode,
                                                _session.Audit.GetText(eMIDTextCode.msg_al_UnknownAllocationSummaryNode, false));
                                        break;
                                    }
                            }
                            break;
                        }
                }

                if (buildSuccess)
                {
                    int nodePathIndex = 0;
                    int[] storeExcludeCount = new int[aStoreList.Count];
                    storeExcludeCount.Initialize();

                    if (gc != null)
                    {
                        if (gc.ComponentType == eComponentType.SpecificColor)
                        {
                            foreach (AllocationProfile ap in apList)
                            {
                                NodeComponent headerNode = nodeComponent.GetChildNodeComponent(nodeComponent, nodePathIndex);
                                HdrColorBin hcb = (HdrColorBin)ap.BulkColors[((AllocationColorOrSizeComponent)gc).ColorRID];
                                if (hcb != null)
                                {
                                    // begin TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                                    IdentifyInventoryCapacityNodes(
                                        ap,
                                        headerNode,
                                        ref inventoryNodes,
                                        ref capacityNodes);
                                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                                    storeExcludeCount = BuildAllocationNode(
                                        ap,
                                        hcb,
                                        headerNode,
                                        aStoreList,
                                        storeExcludeCount);
                                }
                                nodePathIndex++;
                            }
                        }
                        else
                        {
                            foreach (AllocationProfile ap in apList)
                            {
                                NodeComponent headerNode = nodeComponent.GetChildNodeComponent(nodeComponent, nodePathIndex);
                                PackHdr ph = (PackHdr)ap.Packs[((AllocationPackComponent)gc).PackName];
                                if (ph != null)
                                {
                                    // begin TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                                    IdentifyInventoryCapacityNodes(
                                        ap,
                                        headerNode,
                                        ref inventoryNodes,
                                        ref capacityNodes);
                                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                                    storeExcludeCount = BuildAllocationNode(
                                        ap,
                                        ph,
                                        headerNode,
                                        aStoreList,
                                        storeExcludeCount);
                                }
                                nodePathIndex++;
                            }
                        }
                    }
                    else
                    {
                        foreach (AllocationProfile ap in apList)
                        {
                            NodeComponent headerNode = nodeComponent.GetChildNodeComponent(nodeComponent, nodePathIndex);
                            // begin TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                            IdentifyInventoryCapacityNodes(
                                ap,
                                headerNode,
                                ref inventoryNodes,
                                ref capacityNodes);
                            // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                            storeExcludeCount = BuildAllocationNode(
                                        ap,
                                        (eAllocationSummaryNode)allocationSummaryNode,
                                        headerNode,
                                        aStoreList,
                                        storeExcludeCount);
                            nodePathIndex++;
                        }

                    }
                    // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    if (inventoryNodes.Count > 0
                        || capacityNodes.Count > 0)
                    {
                        BuildHeaderAllocationRelationship(
                            aAssortmentProfile,
                            nodeComponent,
                            inventoryNodes,
                            capacityNodes);
                    }
                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken

                    for (int i = 0; i < aStoreList.Count; i++)
                    {
                        storeIndexRID = aAssortmentProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);

                        bool isItExcluded = false;
                        if (storeExcludeCount[i] == apList.Length) // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                        {
                            isItExcluded = true;
                        }
                        else if (_tran.ReserveStore.RID == storeIndexRID.RID)
                        {
                            isItExcluded = true;
                        }
                        else if (!aAssortmentProfile.GetStoreIsEligible(storeIndexRID))
                        {
                            isItExcluded = true;
                        }
                        else if (aAssortmentProfile.GetStoreOut(aComponent, storeIndexRID))
                        {
                            isItExcluded = true;
                        }
                        else if (aAssortmentProfile.GetStoreIsManuallyAllocated(aComponent, storeIndexRID))
                        {
                            isItExcluded = true;
                        }
                        else if ((aAssortmentProfile.GetStoreChosenRuleAcceptedByGroup(aComponent, storeIndexRID) && // TT#488 - MD - Jellis - Group Allocation
                            !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)aAssortmentProfile.GetStoreChosenRuleType(aComponent, storeIndexRID))))
                        {
                            isItExcluded = true;
                        }
                        else if ((aAssortmentProfile.GetStoreAllocationPriority(storeIndexRID) && aAssortmentProfile.GetStoreHadNeed(aComponent, storeIndexRID)))
                        {
                            isItExcluded = true;
                        }
                        else if (aAssortmentProfile.GetStoreLocked(aComponent, storeIndexRID))
                        {
                            isItExcluded = true;
                        }
                        else if (aAssortmentProfile.GetStoreTempLock(aComponent, storeIndexRID))
                        {
                            isItExcluded = true;
                        }
                        if (isItExcluded)
                        {
                            // Node Essentially out for this store 
                            nodeComponent.SetNomineeRuleExcludedNode(i, true);
                        }
                        int storeTotalQtyAllocated = 0;
                        int storeTypeQtyAllocated = 0;
                        int storeDetailSubTypeQtyAllocated = 0;
                        foreach (AllocationProfile ap in apList)
                        {
                            storeTotalQtyAllocated += ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID);
                            storeTypeQtyAllocated += ap.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID);
                            storeDetailSubTypeQtyAllocated += ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID);
                        }
                        nodeComponent.SetNomineeUnitsAllocated(i, storeTotalQtyAllocated);
                        if (!aAssortmentProfile.GetStoreMayExceedMax(storeIndexRID))
                        {
                            // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                            //nodeComponent.SetNomineeMaximum
                            //    (i, aAssortmentProfile.GetStoreMaximum(aComponent, storeIndexRID, false)); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken                            if (!aAssortmentProfile.TryGetStoreMinimum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode, out minimum, out statusReasonCode))
                            if (!aAssortmentProfile.TryGetStoreMaximum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode, out maximum, out statusReasonCode))
                            {
                                throw new MIDException(
                                    eErrorLevel.fatal,
                                    (int)statusReasonCode,
                                    aAssortmentProfile.Session.Audit.GetText(statusReasonCode, false)
                                    + " : Source/Method [" + GetType().Name + " / BuildGroupAllocationNodes-maximum]");
                            }
                            nodeComponent.SetNomineeMaximum(i, maximum);
                            // end TT#1176 - MD - Jellis- Group Allocation - Size Need not observing inv min max

                            //					aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                            if (nodeComponent.IsTheTopNode)
                            {
                                //int maximum; // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing min max
                                if (aComponent.ComponentType == eComponentType.Bulk)
                                {
                                    if (aAssortmentProfile.BulkIsDetail)
                                    {
                                        maximum = aAssortmentProfile.GetStoreMaximum(eAllocationSummaryNode.DetailType, storeIndexRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                                        if (maximum < int.MaxValue)
                                        {
                                            maximum =
                                                Math.Max(0,
                                                maximum - storeDetailSubTypeQtyAllocated + nodeComponent.GetNomineeUnitsAllocated(i)
                                                );
                                        }
                                        if (maximum < nodeComponent.GetNomineeMaximum(i))
                                        {
                                            nodeComponent.SetNomineeMaximum(i, maximum);
                                        }
                                    }
                                    maximum = aAssortmentProfile.GetStoreMaximum(eAllocationSummaryNode.Total, storeIndexRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                                    if (maximum < int.MaxValue)
                                    {
                                        maximum =
                                            Math.Max(0,
                                            (maximum - storeTypeQtyAllocated + nodeComponent.GetNomineeUnitsAllocated(i))
                                            );
                                    }
                                    if (maximum < nodeComponent.GetNomineeMaximum(i))
                                    {
                                        nodeComponent.SetNomineeMaximum(i, maximum);
                                    }
                                }
                            }
                        }
                        if (!aAssortmentProfile.GetStoreMayExceedPrimaryMaximum(storeIndexRID) &&
                            (aAssortmentProfile.GetStorePrimaryMaximum(aComponent, storeIndexRID)
                            < nodeComponent.GetNomineeMaximum(i)))
                        {
                            nodeComponent.SetNomineeMaximum
                                (i, aAssortmentProfile.GetStorePrimaryMaximum(aComponent, storeIndexRID));
                        }
                        // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                        //nodeComponent.SetNomineeMinimum(i, aAssortmentProfile.GetStoreMinimum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                        if (!aAssortmentProfile.TryGetStoreMinimum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode, out minimum, out statusReasonCode))
                        {
                            throw new MIDException(
                                eErrorLevel.fatal,
                                (int)statusReasonCode,
                                aAssortmentProfile.Session.Audit.GetText(statusReasonCode, false)
                                + " : Source/Method [" + GetType().Name + " / BuildGroupAllocationNodes-minimum]");
                        }
                        nodeComponent.SetNomineeMinimum(i, minimum); 
                        // end TT#1176 - MD - Jellis- Group Allocation - Size Need not observing inv min max
                        if (nodeComponent.IsTheTopNode)
                        {
                            if (nodeComponent.GetNomineeMinimum(i) == 0) // Accept a minimum already set for this component.
                            {
                                //int minimum = 0;  // TT#1176 - MD - JEllis - Group Allocation - Size Need not observing inv min max
                                minimum = 0;        // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                                int allocated = 0;
                                if (aComponent.ComponentType == eComponentType.Bulk)
                                {
                                    if (aAssortmentProfile.BulkIsDetail)
                                    {
                                        minimum = aAssortmentProfile.GetStoreMinimum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                                        if (minimum > 0)
                                        {
                                            allocated = storeDetailSubTypeQtyAllocated;
                                        }
                                    }
                                }
                                if (minimum == 0)  // stop looking for a minimum once we have it
                                {
                                    minimum = aAssortmentProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                                    if (minimum > 0)
                                    {
                                        allocated = storeTypeQtyAllocated;
                                    }
                                }
                                if (minimum > 0)
                                {
                                    minimum = (int)
                                        (((((double)(minimum - allocated + nodeComponent.GetNomineeUnitsAllocated(i))
                                        / (double)nodeComponent.NodeMultiple)) + .5d)
                                        * (double)nodeComponent.NodeMultiple);
                                    nodeComponent.SetNomineeMinimum(i, Math.Max(0, minimum)); // if minimum already met, min is zero
                                }
                            }
                        }
                    }
                }
            }
            catch (MIDException e)
            {
                buildSuccess = false;
                aStatusMsg = e;
            }
            catch (Exception e)
            {
                buildSuccess = false;
                aStatusMsg =
                    new MIDException(
                        eErrorLevel.severe,
                        (int)eMIDTextCode.systemError,
                        e.Message, e); // TT#1173 - MD - JEllis - MID Error #20000 -- no explanation

            }
            finally
            {
            }
            return buildSuccess;
        }

        
        private bool BuildHeaderAllocationNodes(
            AllocationProfile aAllocationProfile,
            GeneralComponent aComponent,
            ProfileList aStoreList,
            out NeedAlgorithms aNeedAlgorithm,
            out GeneralComponent aAllocationComponent,
            out MIDException aStatusMsg)
        {
            bool buildSuccess = true;
            aStatusMsg = null;
            aNeedAlgorithm = GetNeedAlgorithms(aAllocationProfile, aStoreList);
            NodeComponent nodeComponent = aNeedAlgorithm.GetNodeComponent();

            aAllocationComponent = aComponent;
            // Set up initial need criteria
            AllocationPackComponent packComponent;
            AllocationColorOrSizeComponent colorComponent;
            Hashtable packs_hash = new Hashtable();
            Hashtable iktHash = new Hashtable();
            IntransitKeyType ikt;
            int[] storeExcludeCount = new int[aStoreList.Count];
            storeExcludeCount.Initialize();
           
            // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            int minimum;
            eMIDTextCode statusReasonCode;
            // end TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            switch (aComponent.ComponentType)
            {
                case (eComponentType.Total):
                    {
                        // NOTE:  Total by need is an allocation by style need 
                        ikt = new IntransitKeyType(0, 0);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                        aNeedAlgorithm.NodeType = eNeedAllocationNode.Total;
                        if (aAllocationProfile.Packs.Count > 0 || aAllocationProfile.BulkColors.Count > 0)
                        {
                            aNeedAlgorithm.GetNodeComponent().AllocateSubNodes = true;
                        }
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.Total,
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        break;
                    }
                case (eComponentType.DetailType):
                    {
                        // NOTE:  Detail by need is an allocation by style need 
                        ikt = new IntransitKeyType(0, 0);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                        aNeedAlgorithm.NodeType = eNeedAllocationNode.DetailType;
                        if (aAllocationProfile.BulkIsDetail && aAllocationProfile.NonGenericPacks.Count == 0 && aAllocationProfile.BulkColors.Count > 0)
                        {
                            aNeedAlgorithm.GetNodeComponent().AllocateSubNodes = true;
                        }
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.DetailType,
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        break;
                    }
                case (eComponentType.Bulk):
                case (eComponentType.AllColors):
                    {
                        // NOTE:  BULK by need is an allocation by style need 
                        ikt = new IntransitKeyType(0, 0);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                        aNeedAlgorithm.NodeType = eNeedAllocationNode.Bulk; 
                        if (aAllocationProfile.BulkColors.Count > 0)
                        {
                            aNeedAlgorithm.GetNodeComponent().AllocateSubNodes = true;
                        }
                        aAllocationComponent = new GeneralComponent(eComponentType.Bulk);  
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.Bulk,
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        break;
                    }
                case (eComponentType.GenericType):
                case (eComponentType.AllGenericPacks):
                    {
                        // NOTE:  Detail by need is an allocation by style need 
                        ikt = new IntransitKeyType(0, 0);
                        iktHash.Add(ikt.IntransitTypeKey, ikt);
                        aNeedAlgorithm.NodeType = eNeedAllocationNode.GenericType;
                        aAllocationComponent = new GeneralComponent(eComponentType.GenericType); 
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.GenericType,
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        foreach (PackHdr ph in aAllocationProfile.Packs.Values)
                        {
                            if (ph.GenericPack)
                            {
                                packs_hash.Add(ph.PackName, ph);
                            }
                        }
                        break;
                    }
                case (eComponentType.SpecificColor):
                    {
                        // NOTE:  any units allocated to the store out side of this color will not
                        //        affect the Store Need Calculations for this color. In other words,
                        //        if there are 10 units of BLU already allocated and we are now 
                        //        allocating RED, the 10 units of BLU will not prevent a store from 
                        //        getting units of RED.  Intransit used in this case will be the 
                        //        RED intransit.  It is the user's responsibility to provide a plan level
                        //        that represents RED sales and inventory.
                        colorComponent = (AllocationColorOrSizeComponent)aComponent;
                        HdrColorBin colorBin = aAllocationProfile.GetHdrColorBin(colorComponent.ColorRID);
                        if (aAllocationProfile.BulkColors.Contains(colorComponent.ColorRID))
                        {
                            ikt = new IntransitKeyType(colorBin.ColorCodeRID, 0);
                            iktHash.Add(ikt.IntransitTypeKey, ikt);
                        }
                        aNeedAlgorithm.NodeType = eNeedAllocationNode.BulkColor;
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            colorBin,
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        break;
                    }
                case (eComponentType.SpecificPack):
                    {
                        // NOTE:  any units allocated to the store out side of this pack will not
                        //        affect the Store Need Calculations for this pack UNLESS the units 
                        //        were allocated in one of the colors on this pack. In other words,
                        //        if there are 10 units of BLU already allocated and we are now 
                        //        allocating a pack with BLU units, the 10 units already allocated 
                        //        in BLU will be considered by NEED. Intransit (onhand) used in this case will be the 
                        //        intransit (onhand) across the colors in the pack (if any); otherwise style level 
                        //        intransit (onhand) is used.  It is the user's responsibility to provide a plan level
                        //        that represents RED sales and inventory.
                        packComponent = (AllocationPackComponent)aComponent;
                        if (aAllocationProfile.Packs.Contains(packComponent.PackName))
                        {
                            PackHdr packHdr = aAllocationProfile.GetPackHdr(packComponent.PackName);
                            packs_hash.Add(packComponent.PackName, packHdr);
                            if (packHdr.PackColorCount > 0)
                            {
                                foreach (PackColorSize pcs in packHdr.PackColors.Values)
                                {
                                    ikt = new IntransitKeyType(pcs.ColorCodeRID, 0);
                                    iktHash.Add(ikt.IntransitTypeKey, ikt);
                                }
                            }
                            else
                            {
                                ikt = new IntransitKeyType(0, 0);
                                iktHash.Add(ikt.IntransitTypeKey, ikt);
                            }
                        }
                        if (aAllocationProfile.GetPackHdr(packComponent.PackName).GenericPack)
                        {
                            aNeedAlgorithm.NodeType = eNeedAllocationNode.GenericPack;
                        }
                        else
                        {
                            aNeedAlgorithm.NodeType = eNeedAllocationNode.NonGenericPack;
                        }
                        storeExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            aAllocationProfile.GetPackHdr(packComponent.PackName),
                            aNeedAlgorithm.GetNodeComponent(),
                            aStoreList,
                            storeExcludeCount);
                        break;
                    }
                case (eComponentType.AllNonGenericPacks):
                    {
                        // same as Detail with Bulk removed
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_AllNonGenericStyleByNeedNotImplemented,
                            _session.Audit.GetText(eMIDTextCode.msg_al_AllNonGenericStyleByNeedNotImplemented, false));
                        buildSuccess = false;
                        break;
                    }
                case (eComponentType.AllPacks):
                    {
                        // same as Total with Bulk removed
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_AllPacksStyleByNeedNotImplemented,
                            _session.Audit.GetText(eMIDTextCode.msg_al_AllPacksStyleByNeedNotImplemented, false));  // MID Track 5374 Workflow not stopping on error
                        buildSuccess = false;
                        break;
                    }
                case (eComponentType.AllSizes):
                    {
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_AloctnByNeedNotValidForSize,
                            _session.Audit.GetText(eMIDTextCode.msg_al_AloctnByNeedNotValidForSize, false));  
                        buildSuccess = false;
                        break;
                    }
                case (eComponentType.SpecificSize):
                    {
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_AloctnByNeedNotValidForSize,
                            _session.Audit.GetText(eMIDTextCode.msg_al_AloctnByNeedNotValidForSize, false));
                        buildSuccess = false;
                        break;
                    }
                case (eComponentType.ColorAndSize):
                    {
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_AloctnByNeedNotValidForSize,
                            _session.Audit.GetText(eMIDTextCode.msg_al_AloctnByNeedNotValidForSize, false));  
                        buildSuccess = false;
                        break;
                    }
                default:
                    {
                        aStatusMsg = new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_UnknownComponentType,
                            _session.Audit.GetText(eMIDTextCode.msg_al_UnknownComponentType, false));
                        buildSuccess = false;
                        break;
                    }

            }
            if (buildSuccess)
            {
                Index_RID storeIndexRID;
                for (int i = 0; i < aStoreList.Count; i++)
                {
                    storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);

                    bool isItExcluded = false;
                    if (_tran.ReserveStore.RID == storeIndexRID.RID)
                    {
                        isItExcluded = true;
                    }
                    else if (!aAllocationProfile.GetStoreIsEligible(storeIndexRID))
                    {
                        isItExcluded = true;
                    }
                    else if (aAllocationProfile.GetStoreOut(aComponent, storeIndexRID))
                    {
                        isItExcluded = true;
                    }
                    else if (aAllocationProfile.GetStoreIsManuallyAllocated(aComponent, storeIndexRID))
                    {
                        isItExcluded = true;
                    }
                    else if ((aAllocationProfile.GetStoreChosenRuleAcceptedByGroup(aComponent, storeIndexRID) &&  // TT#488 - MD - Jellis - Group Allocation
                        !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)aAllocationProfile.GetStoreChosenRuleType(aComponent, storeIndexRID))))
                    {
                        isItExcluded = true;
                    }
                    else if ((aAllocationProfile.GetStoreAllocationPriority(storeIndexRID) && aAllocationProfile.GetStoreHadNeed(aComponent, storeIndexRID)))
                    {
                        isItExcluded = true;
                    }
                    else if (aAllocationProfile.GetStoreLocked(aComponent, storeIndexRID))
                    {
                        isItExcluded = true;
                    }
                    else if (aAllocationProfile.GetStoreTempLock(aComponent, storeIndexRID))
                    {
                        isItExcluded = true;
                    }
                    if (isItExcluded)
                    {
                        // Node Essentially out for this store 
                        nodeComponent.SetNomineeRuleExcludedNode(i, true);
                    }
                    int storeTotalQtyAllocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID);
                    int storeTypeQtyAllocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID);
                    int storeDetailSubTypeQtyAllocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID);
                    nodeComponent.SetNomineeUnitsAllocated(i, storeTotalQtyAllocated);

                    if (!aAllocationProfile.GetStoreMayExceedMax(storeIndexRID))
                    {
                        // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                        //nodeComponent.SetNomineeMaximum
                        //        (i, aAllocationProfile.GetStoreMaximum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode)); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                        int maximum;
                        if (!aAllocationProfile.TryGetStoreMaximum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode, out maximum, out statusReasonCode))
                        {
                            throw new MIDException(
                                eErrorLevel.fatal,
                                (int)statusReasonCode,
                                aAllocationProfile.Session.Audit.GetText(statusReasonCode, false)
                                + " : Source/Method [" + GetType().Name + " / BuildHeaderAllocationNodes--maximum]");
                        }
                        nodeComponent.SetNomineeMaximum(i, maximum);
                        // end TT#1176 - MD - Jellis- Group Allocation - Size Need not observing inv min max

                        //					aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                        if (nodeComponent.IsTheTopNode)
                        {
                            //int maximum;  // TT#1176 - MD - Jellis - Group ALlocation Size Need not observing inv min max
                            if (aComponent.ComponentType == eComponentType.Bulk)
                            {
                                if (aAllocationProfile.BulkIsDetail)
                                {
                                    maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.DetailType, storeIndexRID, true); // TT#1074 - MD- Jellis- Group ALlocation - Inventory Min Max Broken
                                    if (maximum < int.MaxValue)
                                    {
                                        maximum =
                                            Math.Max(0,
                                            maximum - storeDetailSubTypeQtyAllocated + nodeComponent.GetNomineeUnitsAllocated(i)
                                            );
                                    }
                                    if (maximum < nodeComponent.GetNomineeMaximum(i))
                                    {
                                        nodeComponent.SetNomineeMaximum(i, maximum);
                                    }
                                }
                                maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.Total, storeIndexRID, true); // TT#1074 - MD- Jellis - Group Allocation - Inventory Min Max Broken
                                if (maximum < int.MaxValue)
                                {
                                    maximum =
                                        Math.Max(0,
                                        (maximum - storeTypeQtyAllocated + nodeComponent.GetNomineeUnitsAllocated(i))
                                        );
                                }
                                if (maximum < nodeComponent.GetNomineeMaximum(i))
                                {
                                    nodeComponent.SetNomineeMaximum(i, maximum);
                                }
                            }
                        }
                    }
                    // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    if (aAllocationProfile.AssortmentProfile != null) // only care about this when I have an assortment involved
                    {
                        if (aAllocationProfile.CapacityNodeRID != Include.NoRID)
                        {
                            nodeComponent.SetNomineeCapacityMaximum(i, aAllocationProfile.GetStoreCapacityMaximum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2
                        }
                        if (aAllocationProfile.GradeInventoryMinimumMaximum)
                        {
                            nodeComponent.SetNomineeInventoryMaximum(i, nodeComponent.GetNomineeMaximum(i));  // at this point the values should be the same!
                        }
                    }
                    // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    if (!aAllocationProfile.GetStoreMayExceedPrimaryMaximum(storeIndexRID) &&
                        (aAllocationProfile.GetStorePrimaryMaximum(aComponent, storeIndexRID)
                        < nodeComponent.GetNomineeMaximum(i)))
                    {
                        nodeComponent.SetNomineeMaximum
                            (i, aAllocationProfile.GetStorePrimaryMaximum(aComponent, storeIndexRID));
                    }
                    // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                    if (!aAllocationProfile.TryGetStoreMinimum(aComponent, storeIndexRID, nodeComponent.IsTheTopNode, out minimum, out statusReasonCode))
                    {
                        throw new MIDException(
                            eErrorLevel.fatal,
                            (int)statusReasonCode,
                            aAllocationProfile.Session.Audit.GetText(statusReasonCode, false)
                            + " : Source/Method [" + GetType().Name + " / BuildHeaderAllocationNodes--minimum]");
                    }
                    // end TT#1176 - MD - Jellis- Group Allocation - Size Need not observing inv min max
                    if (aAllocationProfile.AssortmentProfile != null
                        &&aAllocationProfile.GradeInventoryMinimumMaximum)
                    {
                        nodeComponent.SetNomineeInventoryMinimum(i, minimum);    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2 // TT#1176 - MD - JEllis - Group Allocation - Size Need Not observing inv min max
                    }
                    else
                    {
                        // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                        nodeComponent.SetNomineeMinimum(i, minimum);    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken // TT#1176 - MD - Jellis - Group allocation Size Need not observing inv min max
                    }  // TT#1074 - MD - Jellis Group ALlocation Inventory Min Max Broken
                    if (nodeComponent.IsTheTopNode)
                    {
                        if (nodeComponent.GetNomineeMinimum(i) == 0) // Accept a minimum already set for this component.
                        {
                            //int minimum = 0;  // TT#1176 - MD- Jellis - Group ALlocation - Size Need not observing inv min max
                            minimum = 0;        // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                            int allocated = 0;
                            if (aComponent.ComponentType == eComponentType.Bulk)
                            {
                                if (aAllocationProfile.BulkIsDetail)
                                {
                                    minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                                    if (minimum > 0)
                                    {
                                        allocated = storeDetailSubTypeQtyAllocated;
                                    }
                                }
                            }
                            if (minimum == 0)  // stop looking for a minimum once we have it
                            {
                                minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                                if (minimum > 0)
                                {
                                    allocated = storeTypeQtyAllocated;
                                }
                            }
                            if (minimum > 0)
                            {
                                minimum = (int)
                                    (((((double)(minimum - allocated + nodeComponent.GetNomineeUnitsAllocated(i))
                                    / (double)nodeComponent.NodeMultiple)) + .5d)
                                    * (double)nodeComponent.NodeMultiple);
                                nodeComponent.SetNomineeMinimum(i, Math.Max(0, minimum)); // if minimum already met, min is zero
                            }
                        }
                    }
                 }

            }
            return buildSuccess;
        }
 

        /// <summary>
        /// Builds/populates allocation node structure for a style need allocation
        /// </summary>
        /// <param name="aAllocationSummaryNode">Node type to build.</param>
        /// <param name="aNodeComponent">Node to build</param>
        /// <param name="aStoreList">Stores to include in the allocation</param>
        /// <param name="aStoreExcludeCount">Stores excluded by rule on other components at same level</param>
        /// <returns>Store Exclude Count updated for this node</returns>
        private int[] BuildAllocationNode(
            AllocationProfile aAllocationProfile,
            eAllocationSummaryNode aAllocationSummaryNode,
            NodeComponent aNodeComponent,
            ProfileList aStoreList,
            int[] aStoreExcludeCount)
        {
            Index_RID storeIndexRID;

            int genericPackCount = aAllocationProfile.GenericPackCount;
            int nonGenericPackCount = aAllocationProfile.NonGenericPackCount;

            //aNodeComponent.NodeType = (eAllocationNode)aAllocationSummaryNode;  // TT#1403 - JEllis - Group Allocation
            aNodeComponent.NodeID = null;
            aNodeComponent.NodeDescription = aAllocationProfile.HeaderID;

            int dimension = 0;
            switch (aAllocationSummaryNode)
            {
                case (eAllocationSummaryNode.Total):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.Total;         // TT#1403 - JEllis - Group Allocation
                        // begin TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                        //aNodeComponent.NodeMultiple = aAllocationProfile.AllocationMultiple; 
                        if (aAllocationProfile.AssortmentProfile == null
                            || (aAllocationProfile.BulkColors.Count == 0
                                && aAllocationProfile.Packs.Count == 0))
                        {
                            // When no assortment profile or no components on header
                            // use header multiple 
                            aNodeComponent.NodeMultiple = aAllocationProfile.AllocationMultiple;
                        }
                        else
                        {
                            // let the actual component allocations determine the multiple when there are components
                            // (downside is that IF the allocation multiple was set by the user instead of calculated, we will not observe it in some cases.
                            //  using the multiple will cause many GA allocations to be different from a header's allocation where the header is identical to the GA.
                            //  When there is a multiple greater than 1, the need allocation could give less than it would had it been 1; for a GA allocation, this 
                            // could be a significant difference in the desired result
                            aNodeComponent.NodeMultiple = 1;
                        }
                        // end MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.TotalUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.TotalUnitsToAllocate;
                        dimension = genericPackCount;
                        if (aAllocationProfile.NonGenericPackCount > 0
                            || (aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0))
                        {
                            dimension++;
                        }
                        if (!aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0)
                        {
                            dimension++;
                        }
                        if (dimension > 0)
                        {
                            aNodeComponent.AllocateSubNodes = true;
                        }
                        break;
                    }
                case (eAllocationSummaryNode.Type):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.Type;         // TT#1403 - JEllis - Group Allocation
                        aNodeComponent.NodeMultiple = aAllocationProfile.AllocationMultiple;
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.GenericUnitsAllocated + aAllocationProfile.DetailTypeUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.GenericUnitsToAllocate + aAllocationProfile.DetailTypeUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = true;
                        dimension = genericPackCount;
                        if (aAllocationProfile.NonGenericPackCount > 0
                            || (aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0))
                        {
                            dimension++;
                        }
                        if (!aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0)
                        {
                            dimension++;
                        }
                        break;
                    }
                case (eAllocationSummaryNode.GenericType):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.GenericType;         // TT#1403 - JEllis - Group Allocation
                        aNodeComponent.NodeMultiple = aAllocationProfile.GenericMultiple;
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.GenericUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.GenericUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = true;
                        dimension = genericPackCount;
                        break;
                    }
                case (eAllocationSummaryNode.DetailType):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.DetailType;         // TT#1403 - JEllis - Group Allocation
                        // begin TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                        if (aAllocationProfile.AssortmentProfile != null)
                        {
                            aNodeComponent.NodeMultiple = aAllocationProfile.AssortmentProfile.DetailTypeMultiple;
                        }
                        else
                        {
                            // end TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                            aNodeComponent.NodeMultiple = aAllocationProfile.DetailTypeMultiple;
                        } // TT#1565 - MD - JEllis - Allocation for a Header set-up identically to GA different from GA
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.DetailTypeUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.DetailTypeUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = false;
                        dimension = nonGenericPackCount;
                        if (aAllocationProfile.BulkIsDetail)
                        {
                            if (aAllocationProfile.BulkColors.Count > 0)
                            {
                                if (aAllocationProfile.NonGenericPackCount == 0)
                                {
                                    // begin TT#1436 - MD - JEllis - GA allocates bulk before packs
                                    if (aAllocationProfile.AssortmentProfile != null
                                        && aAllocationProfile.AssortmentProfile.NonGenericPackCount > 0
                                        && aAllocationProfile.AssortmentProfile.BulkColors.Count > 0 && aAllocationProfile.BulkIsDetail)
                                    {
                                        // DO NOT ALLOCATE BULK WHEN IN A GROUP ALLOCATION AND THERE ARE DETAIL PACKS on another member header
                                    }
                                    else
                                    {
                                        aNodeComponent.AllocateSubNodes = true;
                                    }
                                    //aNodeComponent.AllocateSubNodes = true;
                                    // end TT#1436 - MD - JEllis - GA allocates bulk before packs

                                }
                                dimension++;
                            }
                        }
                        break;
                    }
                case (eAllocationSummaryNode.DetailSubType):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.DetailSubType;         // TT#1403 - JEllis - Group Allocation
                        aNodeComponent.NodeMultiple = aAllocationProfile.DetailTypeMultiple;
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.DetailTypeUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.DetailTypeUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = false;
                        if (aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0)
                        {
                            aNodeComponent.NodeUnitsAllocated -= aAllocationProfile.BulkUnitsAllocated;
                            aNodeComponent.NodeUnitsToAllocate -= aAllocationProfile.BulkUnitsToAllocate;
                        }
                        dimension = nonGenericPackCount;
                        break;
                    }
                case (eAllocationSummaryNode.BulkColorTotal):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.BulkColorTotal;         // TT#1403 - JEllis - Group Allocation
                        aNodeComponent.NodeMultiple = aAllocationProfile.BulkMultiple;
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.BulkUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.BulkUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = true;
                        dimension = aAllocationProfile.BulkColors.Count;
                        break;
                    }
                case (eAllocationSummaryNode.Bulk):
                    {
                        aNodeComponent.NodeType = eNeedAllocationNode.Bulk;         // TT#1403 - JEllis - Group Allocation
                        aNodeComponent.NodeMultiple = aAllocationProfile.BulkMultiple;
                        aNodeComponent.NodeUnitsAllocated = aAllocationProfile.BulkUnitsAllocated;
                        aNodeComponent.NodeUnitsToAllocate = aAllocationProfile.BulkUnitsToAllocate;
                        aNodeComponent.AllocateSubNodes = true;
                        dimension = aAllocationProfile.BulkColors.Count;
                        break;
                    }
                default:
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_UnknownAllocationSummaryNode,
                            _session.Audit.GetText(eMIDTextCode.msg_al_UnknownAllocationSummaryNode, false));  // MID Track 5374 Workflow not stopping on error
                    }

            }
            if (dimension > 0)
            {
                aNodeComponent.SetSubNodeDimension(dimension);
            }
            int[] nodePathIndex = new int[1];
            nodePathIndex[0] = 0;
            int[] storeExcludeCount = new int[aStoreList.Count];
            storeExcludeCount.Initialize();

            if (aAllocationSummaryNode == eAllocationSummaryNode.Total
                || aAllocationSummaryNode == eAllocationSummaryNode.Type
                || aAllocationSummaryNode == eAllocationSummaryNode.GenericType)
            {
                if (aAllocationProfile.GenericPackCount > 0)
                {
                    foreach (PackHdr ph in aAllocationProfile.GenericPacks.Values)
                    {
                        storeExcludeCount = this.BuildAllocationNode(
                            aAllocationProfile,
                            ph,
                            aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]),
                            aStoreList,
                            storeExcludeCount);
                        nodePathIndex[0]++;
                    }
                }
                if (aAllocationSummaryNode == eAllocationSummaryNode.Total
                    || aAllocationSummaryNode == eAllocationSummaryNode.Type)
                {
                    if (aAllocationProfile.NonGenericPackCount > 0
                        || (aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0))
                    {
                        int[] detailStoreExcludeCount = new int[aStoreExcludeCount.Length];
                        detailStoreExcludeCount.Initialize();
                        NodeComponent detailNode = aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]);
                        detailStoreExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.DetailType,
                            detailNode,
                            aStoreList,
                            detailStoreExcludeCount);
                        nodePathIndex[0]++;
                        for (int i = 0; i < aStoreExcludeCount.Length; i++)
                        {
                            if (detailStoreExcludeCount[i] == detailNode.SubnodeDimension
                                && detailNode.SubnodeDimension > 0)
                            {
                                storeExcludeCount[i]++;
                            }
                        }
                    }

                    if (!aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0)
                    {
                        int[] bulk1StoreExcludeCount = new int[aStoreExcludeCount.Length];
                        bulk1StoreExcludeCount.Initialize();
                        NodeComponent bulk1Node = aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]);
                        bulk1StoreExcludeCount = BuildAllocationNode(
                            aAllocationProfile,
                            eAllocationSummaryNode.Bulk,
                            bulk1Node,
                            aStoreList,
                            bulk1StoreExcludeCount);
                        nodePathIndex[0]++;
                        for (int i = 0; i < aStoreExcludeCount.Length; i++)
                        {
                            if (bulk1StoreExcludeCount[i] == bulk1Node.SubnodeDimension
                                && bulk1Node.SubnodeDimension > 0)
                            {
                                storeExcludeCount[i]++;
                            }
                        }
                    }
                }
            }
            else
            {
                if (aAllocationSummaryNode == eAllocationSummaryNode.DetailType
                    || aAllocationSummaryNode == eAllocationSummaryNode.DetailSubType)
                {
                    foreach (PackHdr ph in aAllocationProfile.NonGenericPacks.Values)
                    {
                        storeExcludeCount = this.BuildAllocationNode(
                            aAllocationProfile,
                            ph,
                            aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]),
                            aStoreList,
                            storeExcludeCount);
                        nodePathIndex[0]++;
                    }
                    if (aAllocationSummaryNode == eAllocationSummaryNode.DetailType)
                    {
                        if (aAllocationProfile.BulkIsDetail && aAllocationProfile.BulkColors.Count > 0)
                        {
                            int[] bulk2StoreExcludeCount = new int[aStoreExcludeCount.Length];
                            bulk2StoreExcludeCount.Initialize();
                            NodeComponent bulk2Node = aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]);
                            bulk2StoreExcludeCount = BuildAllocationNode(
                                aAllocationProfile,
                                eAllocationSummaryNode.Bulk,
                                bulk2Node,
                                aStoreList,
                                bulk2StoreExcludeCount);
                            nodePathIndex[0]++;
                            for (int i = 0; i < aStoreExcludeCount.Length; i++)
                            {
                                if (bulk2StoreExcludeCount[i] == bulk2Node.SubnodeDimension
                                    && bulk2Node.SubnodeDimension > 0)
                                {
                                    storeExcludeCount[i]++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // must be bulk or bulk color
                    if (aAllocationSummaryNode == eAllocationSummaryNode.Bulk)
                    {
                        aNodeComponent.AllocateSubNodes = true;
                    }
                    foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
                    {
                        storeExcludeCount = this.BuildAllocationNode(
                            aAllocationProfile,
                            hcb,
                            aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex[0]),
                            aStoreList,
                            storeExcludeCount);
                        nodePathIndex[0]++;
                    }
                }
            }
            for (int i = 0; i < aStoreList.Count; i++)
            {
                storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);

                bool isItExcluded = false;
                if (dimension > 0 && storeExcludeCount[i] == dimension)
                {
                    isItExcluded = true;
                }
                else if (_tran.ReserveStore.RID == storeIndexRID.RID)
                {
                    isItExcluded = true;
                }
				    // begin TT#1401 - JEllis - Urban Reservation Store pt 11
                else if (!aAllocationProfile.GetIncludeStoreInAllocation(storeIndexRID))
                {
                    isItExcluded = true;
                }
                    // end TT#1401 - JEllis - Urban Reservation Store pt 11
                else if (!aAllocationProfile.GetStoreIsEligible(storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreOut(aAllocationSummaryNode, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreIsManuallyAllocated(aAllocationSummaryNode, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if ((aAllocationProfile.GetStoreChosenRuleAcceptedByGroup(aAllocationSummaryNode, storeIndexRID) && // TT#488 - MD - Jellis - Group Allocatin
                    !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)aAllocationProfile.GetStoreChosenRuleType(aAllocationSummaryNode, storeIndexRID))))
                {
                    isItExcluded = true;
                }
                else if ((aAllocationProfile.GetStoreAllocationPriority(storeIndexRID) && aAllocationProfile.GetStoreHadNeed(aAllocationSummaryNode, storeIndexRID)))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreLocked(aAllocationSummaryNode, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreTempLock(aAllocationSummaryNode, storeIndexRID))
                {
                    isItExcluded = true;
                }
                if (isItExcluded)
                {
                    // Node Essentially out for this store 
                    aNodeComponent.SetNomineeRuleExcludedNode(i, true);
                    aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                }
                aNodeComponent.SetNomineeUnitsAllocated(i, aAllocationProfile.GetStoreQtyAllocated(aAllocationSummaryNode, storeIndexRID));
                if (!aAllocationProfile.GetStoreMayExceedMax(storeIndexRID))
                {
                    aNodeComponent.SetNomineeMaximum
                        (i, aAllocationProfile.GetStoreMaximum(aAllocationSummaryNode, storeIndexRID, aNodeComponent.IsTheTopNode)); // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2 // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    //					aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                    if (aNodeComponent.IsTheTopNode)
                    {
                        int maximum;
                        if (aAllocationSummaryNode == eAllocationSummaryNode.Bulk)
                        {
                            if (aAllocationProfile.BulkIsDetail)
                            {
                                maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.DetailType, storeIndexRID, true); // TT#1074 - MD - Jellis- Group Allocation Inventory Min Max Broken
                                if (maximum < int.MaxValue)
                                {
                                    maximum =
                                        Math.Max(0,
                                        maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i)
                                        );
                                }
                                if (maximum < aNodeComponent.GetNomineeMaximum(i))
                                {
                                    aNodeComponent.SetNomineeMaximum(i, maximum);
                                }
                            }
                            maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.Total, storeIndexRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                            if (maximum < int.MaxValue)
                            {
                                maximum =
                                    Math.Max(0,
                                    (maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i))
                                    );
                            }
                            if (maximum < aNodeComponent.GetNomineeMaximum(i))
                            {
                                aNodeComponent.SetNomineeMaximum(i, maximum);
                            }
                        }
                    }
                    // begin TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    if ((aNodeComponent.IsTheTopNode                                           // TT#1074 - MD- Jellis - Group Allocation Inv Min Max
                         || aNodeComponent.ParentNodeComponent.NodeType == eNeedAllocationNode.Group) // TT#1074 - MD - Jellis - Group Allocation Inv Min Max
                        && aAllocationProfile.AssortmentProfile != null) // only care about this when I have an assortment involved
                    {
                        if (aAllocationProfile.CapacityNodeRID != Include.NoRID)
                        {
                            GeneralComponent gc;
                            switch (aAllocationSummaryNode)
                            {
                                case (eAllocationSummaryNode.GenericType):
                                    {
                                        gc = new GeneralComponent(eComponentType.GenericType);
                                        break;
                                    }
                                case (eAllocationSummaryNode.DetailType):
                                    {
                                        gc = new GeneralComponent(eComponentType.DetailType);
                                        break;
                                    }
                                case (eAllocationSummaryNode.Bulk):
                                    {
                                        gc = new GeneralComponent(eComponentType.Bulk);
                                        break;
                                    }
                                default:
                                    {
                                        gc = new GeneralComponent(eComponentType.Total);
                                        break;
                                    }
                            }
                            aNodeComponent.SetNomineeCapacityMaximum(i, aAllocationProfile.GetStoreCapacityMaximum(gc, storeIndexRID, aNodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2
                        }
                        if (aAllocationProfile.GradeInventoryMinimumMaximum)
                        {
                            aNodeComponent.SetNomineeInventoryMaximum(i, aAllocationProfile.GetStoreMaximum(aAllocationSummaryNode, storeIndexRID, aNodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2
                        }
                    }
                    // end TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                }
                if (!aAllocationProfile.GetStoreMayExceedPrimaryMaximum(storeIndexRID) &&
                    (aAllocationProfile.GetStorePrimaryMaximum(aAllocationSummaryNode, storeIndexRID)
                    < aNodeComponent.GetNomineeMaximum(i)))
                {
                    aNodeComponent.SetNomineeMaximum
                        (i, aAllocationProfile.GetStorePrimaryMaximum(aAllocationSummaryNode, storeIndexRID));
                }
                // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                if (aAllocationProfile.AssortmentProfile != null
                    && aAllocationProfile.GradeInventoryMinimumMaximum)
                {
                    aNodeComponent.SetNomineeInventoryMinimum(i, aAllocationProfile.GetStoreMinimum(aAllocationSummaryNode, storeIndexRID, aNodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2
                }
                else
                {
                    // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    aNodeComponent.SetNomineeMinimum(i, aAllocationProfile.GetStoreMinimum(aAllocationSummaryNode, storeIndexRID, false));  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                }   // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                if (aNodeComponent.IsTheTopNode)
                {
                    if (aNodeComponent.GetNomineeMinimum(i) == 0) // Accept a minimum already set for this component.
                    {
                        int minimum = 0;
                        int allocated = 0;
                        if (aAllocationSummaryNode == eAllocationSummaryNode.Bulk)
                        {
                            if (aAllocationProfile.BulkIsDetail)
                            {
                                minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                                if (minimum > 0)
                                {
                                    allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID);
                                }
                            }
                        }
                        if (minimum == 0)  // stop looking for a minimum once we have it
                        {
                            minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                            if (minimum > 0)
                            {
                                allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID);
                            }
                        }
                        if (minimum > 0)
                        {
                            minimum = (int)
                                (((((double)(minimum - allocated + aNodeComponent.GetNomineeUnitsAllocated(i))
                                / (double)aNodeComponent.NodeMultiple)) + .5d)
                                * (double)aNodeComponent.NodeMultiple);
                            aNodeComponent.SetNomineeMinimum(i, Math.Max(0, minimum)); // if minimum already met, min is zero
                        }
                    }
                }
            }
            return aStoreExcludeCount;
        }

        /// <summary>
        /// Builds/populates allocation node structure for a style need allocation
        /// </summary>
        /// <param name="aPack">Identifies the pack for which a node is to be built.</param>
        /// <param name="aNodeComponent">Node to build</param>
        /// <param name="aStoreList">Stores to include in the allocation</param>
        /// <param name="aStoreExcludeCount">Stores excluded by rule on other components at same level</param>
        /// <returns>Store Exclude Count updated for this node</returns>
        private int[] BuildAllocationNode(
            AllocationProfile aAllocationProfile,
            PackHdr aPack,
            NodeComponent aNodeComponent,
            ProfileList aStoreList,
            int[] aStoreExcludeCount)
        {
            Index_RID storeIndexRID;
            //			aNodeComponent.SetSubNodeDimension(0);
            aNodeComponent.NodeMultiple = aPack.PackMultiple;
            aNodeComponent.NodeID = aPack.PackName;
            aNodeComponent.NodeDescription = aAllocationProfile.HeaderID + "|" + aPack.PackName;
            if (aPack.GenericPack)
            {
                //aNodeComponent.NodeType = eAllocationNode.GenericPack;  // TT#1403 - JEllis - Group Allocation
                aNodeComponent.NodeType = eNeedAllocationNode.GenericPack; // TT#1403 - JEllis - Group Allocation
            }
            else
            {
                //aNodeComponent.NodeType = eAllocationNode.NonGenericPack; // TT#1403 - JEllis - Group Allocation
                aNodeComponent.NodeType = eNeedAllocationNode.NonGenericPack; // TT#1403 - JEllis - Group Allocation
            }
            aNodeComponent.NodeUnitsAllocated = aPack.UnitsAllocated;
            aNodeComponent.NodeUnitsToAllocate = aPack.UnitsToAllocate;

            int primaryMax = 0;
            for (int i = 0; i < aStoreList.Count; i++)
            {
                storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);

                bool isItExcluded = false;
                if (!aAllocationProfile.GetStoreIsEligible(storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (_tran.ReserveStore.RID == storeIndexRID.RID)
                {
                    isItExcluded = true;
                }
                // begin TT#1401 - JEllis - Urban Reservation Store pt 11
                else if (!aAllocationProfile.GetIncludeStoreInAllocation(storeIndexRID))
                {
                    isItExcluded = true;
                }
                // end TT#1401 - JEllis - Urban Reservation Store pt 11
                else if (aAllocationProfile.GetStoreOut(aPack, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreIsManuallyAllocated(aPack, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if ((aAllocationProfile.GetStoreChosenRuleAcceptedByGroup(aPack, storeIndexRID) && // TT#488 - MD - Jellis - Group Allocation
                    !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)aAllocationProfile.GetStoreChosenRuleType(aPack, storeIndexRID))))
                {
                    isItExcluded = true;
                }
                else if ((aAllocationProfile.GetStoreAllocationPriority(storeIndexRID) && aAllocationProfile.GetStoreHadNeed(aPack, storeIndexRID)))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreLocked(aPack, storeIndexRID))
                {
                    isItExcluded = true;
                }
                else if (aAllocationProfile.GetStoreTempLock(aPack, storeIndexRID))
                {
                    isItExcluded = true;
                }
                if (isItExcluded)
                {
                    // Node Essentially out for this store 
                    aNodeComponent.SetNomineeRuleExcludedNode(i, true);
                    aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                }
                aNodeComponent.SetNomineeUnitsAllocated(i, aAllocationProfile.GetStoreQtyAllocated(aPack, storeIndexRID) * aPack.PackMultiple);
                if (!aAllocationProfile.GetStoreMayExceedMax(storeIndexRID))
                {
                    if (aAllocationProfile.GetStoreMaximum(aPack, storeIndexRID, false) == aPack.GetStorePackLargestMaximum()) // TT#1074 - MD - jellis - Group Allocation Inventory Min Max Broken
                    {
                        aNodeComponent.SetNomineeMaximum(i, aPack.GetStorePackLargestMaximum());
                    }
                    else
                    {
                        aNodeComponent.SetNomineeMaximum
                            (i, aAllocationProfile.GetStoreMaximum(aPack, storeIndexRID, false) * aPack.PackMultiple); // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    }
                    if (aNodeComponent.IsTheTopNode)
                    {
                        int maximum;
                        if (!aPack.GenericPack)
                        {
                            maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis Group Allocation Inventory Min Max Broken
                            if (maximum < int.MaxValue)
                            {
                                maximum =
                                    Math.Max(0,
                                    maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i)
                                    );
                            }
                            if (maximum < aNodeComponent.GetNomineeMaximum(i))
                            {
                                aNodeComponent.SetNomineeMaximum(i, maximum);
                            }
                        }
                        maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.Total, storeIndexRID, true); // TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                        if (maximum < int.MaxValue)
                        {
                            maximum =
                                Math.Max(0,
                                maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i)
                                );
                            if (maximum < aNodeComponent.GetNomineeMaximum(i))
                            {
                                aNodeComponent.SetNomineeMaximum(i, maximum);
                            }
                        }
                    }
                }

                if (aAllocationProfile.GetStorePrimaryMaximum(aPack, storeIndexRID) == aPack.GetStorePackLargestMaximum())
                {
                    primaryMax = aPack.GetStorePackLargestMaximum();
                }
                else
                {
                    primaryMax = aAllocationProfile.GetStorePrimaryMaximum(aPack, storeIndexRID) * aPack.PackMultiple;
                }
                if (!aAllocationProfile.GetStoreMayExceedPrimaryMaximum(storeIndexRID) &&
                    (primaryMax < aNodeComponent.GetNomineeMaximum(i)))
                {
                    aNodeComponent.SetNomineeMaximum
                        (i, primaryMax);
                }
                aNodeComponent.SetNomineeMinimum(i, aAllocationProfile.GetStoreMinimum(aPack, storeIndexRID, false) * aPack.PackMultiple);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                if (aNodeComponent.IsTheTopNode)
                {
                    if (aNodeComponent.GetNomineeMinimum(i) == 0) // Accept a minimum already set for this component.
                    {
                        int minimum = 0;
                        int allocated = 0;
                        if (!aPack.GenericPack)
                        {
                            minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                            if (minimum > 0)
                            {
                                allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID);
                            }
                        }
                        if (minimum == 0)  // stop looking for a minimum once we have it
                        {
                            minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                            if (minimum > 0)
                            {
                                allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID);
                            }
                        }
                        if (minimum > 0)
                        {
                            minimum = (int)
                                (((((double)(minimum - allocated + aNodeComponent.GetNomineeUnitsAllocated(i))
                                / (double)aNodeComponent.NodeMultiple)) + .5d)
                                * (double)aNodeComponent.NodeMultiple);

                            aNodeComponent.SetNomineeMinimum(i, Math.Max(0, minimum)); // if minimum already met, min is zero
                        }
                    }
                }
            }
            return aStoreExcludeCount;
        }

        /// <summary>
        /// Builds/populates allocation node structure for a style need allocation
        /// </summary>
        /// <param name="aColor">Identifies the color for which a node is to be built.</param>
        /// <param name="aNodeComponent">Node to build</param>
        /// <param name="aStoreList">Stores to include in the allocation</param>
        /// <param name="aStoreExcludeCount">Stores excluded by rule on other components at same level</param>
        /// <returns>Store Exclude Count updated for this node</returns>
        private int[] BuildAllocationNode(
            AllocationProfile aAllocationProfile,
            HdrColorBin aColor,
            NodeComponent aNodeComponent,
            ProfileList aStoreList,
            int[] aStoreExcludeCount)
        {
            Index_RID storeIndexRID;
            aNodeComponent.NodeMultiple = aColor.ColorMultiple;
            aNodeComponent.NodeID = aColor.ColorCodeRID; // Assortment: Color/Size changes
            aNodeComponent.NodeDescription = aAllocationProfile.HeaderID + "|" + aColor.ColorCodeRID;
            //aNodeComponent.NodeType = eAllocationNode.BulkColor;  // TT#1403 - JEllis - Group Allocation
            aNodeComponent.NodeType = eNeedAllocationNode.BulkColor; // TT#1403 - JEllis - Group Allocation

            aNodeComponent.NodeUnitsAllocated = aColor.ColorUnitsAllocated;
            aNodeComponent.NodeUnitsToAllocate = aColor.ColorUnitsToAllocate;

            for (int i = 0; i < aStoreList.Count; i++)
            {
                storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);

                if (!aAllocationProfile.GetStoreIsEligible(storeIndexRID) ||
                    _tran.ReserveStore.RID == storeIndexRID.RID ||
					// begin TT#1401 - JEllis - Urban Reservation Store pt 11
                    !aAllocationProfile.GetIncludeStoreInAllocation(storeIndexRID) ||
                    // end TT#1401 - JEllis - Urban Reservation Store pt 11
                    aAllocationProfile.GetStoreOut(aColor, storeIndexRID) ||
                    aAllocationProfile.GetStoreIsManuallyAllocated(aColor, storeIndexRID) ||
                    (aAllocationProfile.GetStoreChosenRuleAcceptedByGroup(aColor, storeIndexRID) && // TT#488 - MD - Jellis - Group Allocation
                    !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)aAllocationProfile.GetStoreChosenRuleType(aColor, storeIndexRID))) ||
                    (aAllocationProfile.GetStoreAllocationPriority(storeIndexRID) && aAllocationProfile.GetStoreHadNeed(aColor, storeIndexRID)) ||
                    aAllocationProfile.GetStoreLocked(aColor, storeIndexRID) ||
                    aAllocationProfile.GetStoreTempLock(aColor, storeIndexRID))
                {
                    // Node Essentially out for this store 
                    aNodeComponent.SetNomineeRuleExcludedNode(i, true);
                    aStoreExcludeCount[i]++;   // this array is based on number of stores in filter!
                }
                aNodeComponent.SetNomineeUnitsAllocated(i, aAllocationProfile.GetStoreQtyAllocated(aColor, storeIndexRID));
                if (!aAllocationProfile.GetStoreMayExceedMax(storeIndexRID))
                {
                    aNodeComponent.SetNomineeMaximum
                        (i, aAllocationProfile.GetStoreMaximum(aColor, storeIndexRID, aNodeComponent.IsTheTopNode));  // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                    if (aNodeComponent.IsTheTopNode)
                    {
                        int maximum;
                        maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.Bulk, storeIndexRID, true); // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                        if (maximum < int.MaxValue)
                        {
                            maximum =
                                Math.Max(0,
                                maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i)
                                );
                        }
                        if (maximum < aNodeComponent.GetNomineeMaximum(i))
                        {
                            aNodeComponent.SetNomineeMaximum(i, maximum);
                        }
                        if (aAllocationProfile.BulkIsDetail)
                        {
                            maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.DetailType, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                            if (maximum < int.MaxValue)
                            {
                                maximum =
                                    Math.Max(0,
                                    maximum - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIndexRID) + aNodeComponent.GetNomineeUnitsAllocated(i)
                                    );
                            }
                            if (maximum < aNodeComponent.GetNomineeMaximum(i))
                            {
                                aNodeComponent.SetNomineeMaximum(i, maximum);
                            }
                        }
                        maximum = aAllocationProfile.GetStoreMaximum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                        if (maximum < int.MaxValue)
                        {
                            maximum =
                                maximum
                                - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID)
                                + aNodeComponent.GetNomineeUnitsAllocated(i);
                            if (maximum < aNodeComponent.GetNomineeMaximum(i))
                            {
                                aNodeComponent.SetNomineeMaximum(i, maximum);
                            }
                        }
                    }
                }

                if (!aAllocationProfile.GetStoreMayExceedPrimaryMaximum(storeIndexRID) &&
                    (aAllocationProfile.GetStorePrimaryMaximum(aColor, storeIndexRID) < aNodeComponent.GetNomineeMaximum(i)))
                {
                    aNodeComponent.SetNomineeMaximum
                        (i, aAllocationProfile.GetStorePrimaryMaximum(aColor, storeIndexRID));
                }
                aNodeComponent.SetNomineeMinimum(i, aAllocationProfile.GetStoreMinimum(aColor, storeIndexRID, aNodeComponent.IsTheTopNode));    // TT#1074 - MD - Jellis - Group Allocation Inv Min Max Part 2  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                if (aNodeComponent.IsTheTopNode)
                {
                    if (aNodeComponent.GetNomineeMinimum(i) == 0) // Accept a minimum already set for this component.
                    {
                        int minimum = 0;
                        int allocated = 0;
                        minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Bulk, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                        if (minimum > 0)
                        {
                            allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIndexRID);
                        }
                        if (minimum == 0)  // stop looking for a minimum once we have it
                        {
                            minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIndexRID, true);  // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                            if (minimum > 0)
                            {
                                allocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Type, storeIndexRID);
                            }
                        }
                        if (minimum > 0)
                        {
                            minimum = (int)
                                (((((double)(minimum - allocated + aNodeComponent.GetNomineeUnitsAllocated(i))
                                / (double)aNodeComponent.NodeMultiple)) + .5d)
                                * (double)aNodeComponent.NodeMultiple);
                            aNodeComponent.SetNomineeMinimum(i, Math.Max(0, minimum)); // if minimum already met, min is zero
                        }
                    }
                }
            }
            return aStoreExcludeCount;
        }
        // begin TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken 
        private bool IdentifyInventoryCapacityNodes(
            AllocationProfile aAllocationProfile,
            NodeComponent aHeaderNode,
            ref Dictionary<int, List<NodeComponent>> aInventoryNodeRef,
            ref Dictionary<int, List<NodeComponent>> aCapacityNodeRef)
        {
            bool success = true;
            try
            {
                List<NodeComponent> nodeList;
                if (aAllocationProfile.GradeInventoryMinimumMaximum)
                {
                    if (!aInventoryNodeRef.TryGetValue(aAllocationProfile.GradeInventoryBasisHnRID, out nodeList))
                    {
                        nodeList = new List<NodeComponent>();
                        aInventoryNodeRef.Add(aAllocationProfile.GradeInventoryBasisHnRID, nodeList);
                    }
                    nodeList.Add(aHeaderNode);
                }
                if (aAllocationProfile.CapacityNodeRID != Include.NoRID)
                {
                    if (!aCapacityNodeRef.TryGetValue(aAllocationProfile.CapacityNodeRID, out nodeList))
                    {
                        nodeList = new List<NodeComponent>();
                        aCapacityNodeRef.Add(aAllocationProfile.CapacityNodeRID, nodeList);
                    }
                    nodeList.Add(aHeaderNode);
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
            }
            return success;
        }
        // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken

        /// <summary>
        /// Builds the allocation relationships between nodes to track inventory minimums/maximums and capacity maximums 
        /// </summary>
        /// <param name="aAssortmentProfile">Assortment Profile that identifies the header relationships</param>
        /// <param name="aNodeComponent">Group Allocation Node Component</param>
        /// <param name="aInventoryNodeRef">Grade Inventory Basis HnRIDs and the nodes they are on</param>
        /// <param name="aCapacityNodeRef">Capacity HnRIDS and the nodes they are on</param>
        /// <returns></returns>
        private bool BuildHeaderAllocationRelationship(
            AssortmentProfile aAssortmentProfile,
            NodeComponent aNodeComponent,
            Dictionary<int, List<NodeComponent>> aInventoryNodeRef,
            Dictionary<int, List<NodeComponent>> aCapacityNodeRef)
        {
            int[] inventoryRIDs;
            int nodePathIndex = 0;
            bool success = true;
            try
            {
                AllocationProfile[] apList = aAssortmentProfile.AssortmentMembers;
                foreach (AllocationProfile ap in apList)
                {
                    inventoryRIDs = aAssortmentProfile.GetInventoryUpdateList(ap.StyleHnRID, Include.IntransitKeyTypeNoColor, false);
                    List<NodeComponent> aList = new List<NodeComponent>();
                    NodeComponent headerNode = aNodeComponent.GetChildNodeComponent(aNodeComponent, nodePathIndex);
                    foreach (int inventoryRID in inventoryRIDs)
                    {
                        if (aInventoryNodeRef.TryGetValue(inventoryRID, out aList))
                        {
                            foreach (NodeComponent n in aList)
                            {
                                if (headerNode != n)
                                {
                                    headerNode.AddInventoryAllocationNode(n);
                                }
                            }
                        }
                        if (aCapacityNodeRef.TryGetValue(inventoryRID, out aList))
                        {
                            foreach (NodeComponent n in aList)
                            {
                                if (headerNode != n)
                                {
                                    headerNode.AddCapacityAllocationNode(n);
                                }
                            }
                        }
                    }
                    if (ap.HeaderColorCodeRID != Include.NoRID)
                    {
                        inventoryRIDs = aAssortmentProfile.GetInventoryUpdateList(ap.StyleHnRID, ap.HeaderColorCodeRID, false);
                        foreach (int inventoryRID in inventoryRIDs)
                        {
                            if (aInventoryNodeRef.TryGetValue(inventoryRID, out aList))
                            {
                                foreach (NodeComponent n in aList)
                                {
                                    if (headerNode != n)
                                    {
                                        headerNode.AddInventoryAllocationNode(n);
                                    }
                                }
                            }
                            if (aCapacityNodeRef.TryGetValue(inventoryRID, out aList))
                            {
                                foreach (NodeComponent n in aList)
                                {
                                    if (headerNode != n)
                                    {
                                        headerNode.AddCapacityAllocationNode(n);
                                    }
                                }
                            }
                        }
                    }
                    nodePathIndex++;
                }
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
            }
            return success;
        }
        // end TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken



        /// <summary>
        /// Completes the set-up for a Style Need Allocation and Performs the allocation
        /// </summary>
        /// <param name="aAllocationProfile">AllocationProfile to allocate</param>
        /// <param name="aComponent">Allocation Component to allocate (corresponds with Need Algorithms Node Component)</param>
        /// <param name="aPackDictionary">Packs that will be allocated.</param>
        /// <param name="aIktHash">OnHand and Intransit base of the allocation</param>
        /// <param name="aNA">Need Algorithms parameters</param>
        /// <param name="aStoreList">List of included stores.</param>
        private bool DoNeedAllocation(
            AllocationProfile aAllocationProfile,
            GeneralComponent aComponent,
            Dictionary<long, PackHdr> aPackDictionary,
            Hashtable aIktHash,
            NeedAlgorithms aNA,
            ProfileList aStoreList,
            out MIDException aStatusMsg)
        {
            //  Beware: some store arrays in this method are based on the _allStoreList and some are based on aStoreList
            bool ActionStatus = true;
            aStatusMsg = null;
            try
            {
                StoreProfile sp;
                Index_RID storeIndexRID;
                IntransitKeyType ikt;
                int[] storeUnitsAllocated = new int[aStoreList.Count];
                ArrayList lastNeedDayList = new ArrayList(aStoreList.Count);
                ArrayList nextNeedDayList = new ArrayList(aStoreList.Count);
                ArrayList allocationNeedDayList = new ArrayList(aStoreList.Count); // MID Track 5525 AnF Defect 1618: Rounding Error
                bool ownershipByColor = false;
                bool nextNeedDayExists = false;
                StringBuilder text;

                foreach (IntransitKeyType iktMember in aIktHash.Values)
                {
                    if (iktMember.IntransitType == eIntransitBy.Color)
                    {
                        ownershipByColor = true;
                    }
                }

                // begin MID Track 6001 Header Load runs slow
                DayProfile startNeedDay;
                if (aAllocationProfile.ShipToDay != Include.UndefinedDate)
                {
                    startNeedDay = _session.Calendar.GetDay(aAllocationProfile.ShipToDay);
                }
                else
                {
                    startNeedDay = aAllocationProfile.OnHandDayProfile;
                }
                // end MID Track 6001 Header Load runs slow

                for (int i = 0; i < aStoreList.Count; i++)
                {
                    storeIndexRID =
                        aAllocationProfile.StoreIndex(((StoreProfile)(aStoreList.ArrayList[i])).Key);
                    aNA.SetNomineeHasAllocationPriority(i, aAllocationProfile.GetStoreAllocationPriority(storeIndexRID));
                    storeUnitsAllocated[i] = aNA.GetNomineeUnitsAllocated(i);
                    aNA.SetNomineePlan(i, 0.0);
                    int onhand = 0;
                // begin TT#1401 - JEllis - Urban Reservation Store pt 7
                //foreach(IntransitKeyType iktMember in aIktHash.Values)
                //{
                //    onhand += aAllocationProfile.GetStoreOnHand(iktMember, storeIndexRID);
                //}

                // Begin TT#684 - md - stodd - group allocation merge 
                if (aAllocationProfile.IMO)
                //if (IMO_Data)
                // End TT#684 - md - stodd - group allocation merge 
                {
                    foreach (IntransitKeyType iktMember in aIktHash.Values)
                    {
                        onhand += aAllocationProfile.GetStoreOnHand(iktMember, storeIndexRID);
                    }
                }
                else
                {
                    foreach (IntransitKeyType iktMember in aIktHash.Values)
                    {
                        onhand += aAllocationProfile.GetStoreOnHand(iktMember, storeIndexRID);
                        onhand += aAllocationProfile.GetStoreImoHistory(iktMember, storeIndexRID);
                    }
                }
                // end TT#1401 - JEllis - Urban Reservation Store pt 7
                    // Adjust onhand by any units already allocated 
                    // begin TT#1403 - JEllis - Group Allocation
                    //if (aNA.NodeType != eAllocationNode.Total
                    //    && aNA.NodeType != eAllocationNode.Type)
                    if (aNA.NodeType != eNeedAllocationNode.Group
                        && aNA.NodeType != eNeedAllocationNode.Total
                        && aNA.NodeType != eNeedAllocationNode.Type)
                    // end TT#1403 - JEllis - Group Allocation
                    {
                        foreach (PackHdr ph in aPackDictionary.Values)
                        {
                            if (ownershipByColor)
                            {
                                foreach (PackColorSize pcs in ph.PackColors.Values)
                                {
                                    ikt = new IntransitKeyType(pcs.ColorCodeRID, 0);
                                    if (aIktHash.Contains(ikt.IntransitTypeKey))
                                    {
                                        onhand +=
                                            ph.GetStorePacksAllocated(storeIndexRID.Index)
                                            * ph.GetColorBin(pcs.ColorCodeRID).ColorUnitsInPack;
                                    }
                                }
                            }
                            else
                            {
                                onhand +=
                                    ph.GetStorePacksAllocated(storeIndexRID.Index)
                                    * ph.PackMultiple;
                            }
                        }
                    }
                    // begin TT#1403 - JEllis - Group Allocation
                    //if ((aNA.NodeType != eAllocationNode.BulkColor &&
                    //    aNA.NodeType != eAllocationNode.Bulk &&
                    //    aNA.NodeType != eAllocationNode.Total &&
                    //    aNA.NodeType != eAllocationNode.Type &&
                    //    aNA.NodeType != eAllocationNode.DetailType) ||
                    //    (aNA.NodeType == eAllocationNode.DetailType && !BulkIsDetail))
                    if ((aNA.NodeType != eNeedAllocationNode.Group
                         && aNA.NodeType != eNeedAllocationNode.BulkColor
                         && aNA.NodeType != eNeedAllocationNode.Bulk 
                         && aNA.NodeType != eNeedAllocationNode.Total
                         && aNA.NodeType != eNeedAllocationNode.Type
                         && aNA.NodeType != eNeedAllocationNode.DetailType) 
                         || (aNA.NodeType == eNeedAllocationNode.DetailType && !aAllocationProfile.BulkIsDetail))
                    // end TT#1403 - JEllis - Group Allocation
                    {
                        if (ownershipByColor)
                        {
                            int colorRID = int.MinValue;
                            //if (aNA.NodeType == eAllocationNode.BulkColor)  // TT#1403 - JEllis - Group Allocation
                            if (aNA.NodeType == eNeedAllocationNode.BulkColor)  // TT#1403 - JEllis - Group Allocation
                            {
                                colorRID = (int)(aNA.GetNodeComponent().NodeID);
                            }
                            foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
                            {
                                if (colorRID != hcb.ColorCodeRID) // Assortment: Color/Size changes
                                {
                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, 0); // Assortment: Color/Size changes
                                    if (aIktHash.Contains(ikt.IntransitTypeKey))
                                    {
                                        onhand += aAllocationProfile.GetStoreQtyAllocated(hcb.ColorCodeRID, storeIndexRID); // Assortment: Color/Size changes
                                    }
                                }
                            }
                        }
                        else
                        {
                            onhand += aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIndexRID);
                        }
                    }
                    aNA.SetNomineeOnHand(i, onhand);

                    aNA.SetNomineeInTransit(i, 0);

                    //int capacity = aAllocationProfile.GetStoreCapacityMaximum(storeIndexRID); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                    // begin TT#1148 - MD -Jellis - Group Allocation Enforces capacity on wrong header
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in DoNeedAllocation()");
                    }
                    #endif	
                    //if (aNA.NodeType != eNeedAllocationNode.Group
                    //    || ((aAllocationProfile is AssortmentProfile)
                    //        && ((AssortmentProfile)aAllocationProfile).AllMemberHeadersHaveSameCapacityNode))
                    if (aNA.NodeType != eNeedAllocationNode.Group
                          || ((aAllocationProfile.isAssortmentProfile)
                              && ((AssortmentProfile)aAllocationProfile).AllMemberHeadersHaveSameCapacityNode))
                    // End TT#4988 - BVaughan - Performance
                    {
                        // end TT#1148 - MD - Jellis - Group ALlocaiton Enforces capacity on wrong header
                        int capacity = aAllocationProfile.GetStoreCapacityMaximum(aComponent, storeIndexRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                        if (capacity < int.MaxValue)
                        {
                            capacity =
                                capacity
                                - aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID)
                                + (aNA.GetNomineeUnitsAllocated(i));
                        }
                        if (capacity < 0)
                        {
                            capacity = 0;
                        }

                        if (capacity < aNA.GetNomineeMaximum(i))
                        {
                            aNA.SetNomineeMaximum(i, capacity);
                        }
                    } // TT#1148 - MD - Jellis - Group Allocation Enforces capacity on wrong header
                    // Set up last need day list for an initial need allocation

                    // begin MID Track 6001 Header Load runs slow
                    //if (this.ShipToDay != Include.UndefinedDate)
                    //{
                    //	nextNeedDayList.Add(_session.Calendar.GetDay(this.ShipToDay));
                    //}				
                    //else 
                    //{
                    //	nextNeedDayList.Add(this.OnHandDayProfile);
                    //}
                    nextNeedDayList.Add(startNeedDay);
                    // end MID Track 6001 Header Load runs slow
                }

                // Set up last need day list for an initial need allocation

                DayProfile lastNeedDayProfile;
                if (aAllocationProfile.ShipToDay == Include.UndefinedDate)
                {
                    // Begin TT#697 - JSmith - Performance
                    //nextNeedDayList = 
                    //    _SAB.StoreServerSession.DetermineInStoreOnHandDay
                    //    (nextNeedDayList, aStoreList);
                    int[] storeRIDs = new int[aStoreList.Count];
                    for (int i = 0; i < aStoreList.Count; i++)
                    {
                        sp = (StoreProfile)aStoreList[i];
                        storeRIDs[i] = sp.Key;
                    }

                    nextNeedDayList =
                        StoreMgmt.DetermineInStoreOnHandDay //aAllocationProfile.SAB.StoreServerSession.DetermineInStoreOnHandDay
                        (nextNeedDayList, storeRIDs, _sab.ApplicationServerSession); // TT#1517 - MD - Store Service Optimization - SRISCH
                    // End TT#697
                }
                // End Set up last need day list

                // Set up need target date where we last left off or for first time
                for (int i = 0; i < aStoreList.Count; i++)
                {
                    storeIndexRID =
                        aAllocationProfile.StoreIndex(((StoreProfile)(aStoreList.ArrayList[i])).Key);

                    // Adjust lastNeedDay to where we left off last time
                    // We always want to start where we left off
                    if (aAllocationProfile.ShipToDay == Include.UndefinedDate)
                    {
                        DateTime nextNeedDate = ((DayProfile)nextNeedDayList[i]).Date;

                        DateTime lastNeedDate = aAllocationProfile.GetStoreLastNeedDay(aComponent, storeIndexRID);
                        if (DateTime.Compare(nextNeedDate, lastNeedDate) < 0)
                        {
                            lastNeedDayProfile =
                                _session.Calendar.GetDay(aAllocationProfile.GetStoreLastNeedDay(aComponent, storeIndexRID));
                            nextNeedDayList[i] = lastNeedDayProfile;
                        }
                    }
                    if (((DayProfile)nextNeedDayList[i]).Date != Include.UndefinedDate)
                    {
                        nextNeedDayExists = true;
                    }
                    else
                    {
                        text = new StringBuilder(_session.Audit.GetText(eMIDTextCode.msg_al_NeedHorizonUndefined, false));
                        text.Replace("{0}", aAllocationProfile.HeaderID);
                        _session.Audit.Add_Msg(
                            eMIDMessageLevel.Warning, // MID Track 5778 (Part 2) - Scheduler 'Run Now' feature gets error in audit
                            eMIDTextCode.msg_al_NeedHorizonUndefined,
                            text.ToString(),
                            this.GetType().Name);
                    }
                }

                bool PlanExists = true;
                bool UnitsAllocatedInPass = true;
                bool UnitsAllocatedByNeed = false;
                ArrayList stubArrayList = new ArrayList();
                stubArrayList.Add(Include.NoRID);
                ProfileList hnPL = new ProfileList(eProfileType.HierarchyNode);
                hnPL.Add(_tran.GetNodeData(aAllocationProfile.PlanHnRID));

                int[] storePlans;

                IntransitKeyType[] iktArray = new IntransitKeyType[aIktHash.Count];
                aIktHash.Values.CopyTo(iktArray, 0);
                // begin MID Track 3810 Size Allocation GT Style Allocation
                eWorkUpBuyAllocationType workUpBuy = eWorkUpBuyAllocationType.NotWorkUpAllocationBuy;
                switch (aComponent.ComponentType)
                {
                    case (eComponentType.Total):
                        {
                            if (aAllocationProfile.WorkUpTotalBuy
                                || aAllocationProfile.Placeholder)
                            {
                                workUpBuy = eWorkUpBuyAllocationType.WorkUpTotalAllocationBuy;
                            }
                            break;
                        }
                    case (eComponentType.Bulk):
                    case (eComponentType.AllColors):
                        {
                            if (aAllocationProfile.WorkUpBulkBuy
                                || aAllocationProfile.Placeholder)
                            {
                                workUpBuy = eWorkUpBuyAllocationType.WorkUpTotalAllocationBuy;
                            }
                            break;
                        }
                    case (eComponentType.SpecificColor):
                        {
                            if (aAllocationProfile.WorkUpBulkColorBuy
                                || aAllocationProfile.Placeholder)
                            {
                                workUpBuy = eWorkUpBuyAllocationType.WorkUpTotalAllocationBuy;
                            }
                            break;
                        }
                    default:
                        {
                            workUpBuy = eWorkUpBuyAllocationType.NotWorkUpAllocationBuy;
                            break;
                        }
                }
                int allStoreCount = _tran.StoreIndexRIDArray().Length;
                int[] storeFromDay = new int[allStoreCount];
                int[] storeToDay = new int[allStoreCount];
                DateTime toDay;
                if (allStoreCount != _tran.StoreIndexRIDArray().Length)
                {
                    throw new MIDException(eErrorLevel.fatal,
                        (int)eMIDTextCode.msg_al_StoreIndexRIDOutOfSync,
                        this._session.Audit.GetText(eMIDTextCode.msg_al_StoreIndexRIDOutOfSync, false));  // MID Track 5374 Workflow not stopping on error
                }
                int[] allStoreRIDs = new int[allStoreCount];
                for (int i = 0; i < allStoreCount; i++)
                {
                    storeIndexRID = _tran.StoreIndexRIDArray()[i];
                    allStoreRIDs[i] = storeIndexRID.RID;
                    storeFromDay[storeIndexRID.Index] = aAllocationProfile.OnHandDayProfile.YearDay;
                    DateTime lastNeedDay = aAllocationProfile.GetStoreLastNeedDay(aComponent, storeIndexRID);
                    if (lastNeedDay == Include.UndefinedDate)
                    {
                        lastNeedDay = aAllocationProfile.GetStoreShipDay(storeIndexRID);
                    }
                    toDay = lastNeedDay.AddDays(-1);
                    storeToDay[storeIndexRID.Index] = (toDay.Year) * 1000 + toDay.DayOfYear;
                }
                bool thereWasAPlan = false;
                if ((aNA.UnitsAllocated < aNA.UnitsToAllocate || workUpBuy != eWorkUpBuyAllocationType.NotWorkUpAllocationBuy) && // MID Track 3810 Size Allocation GT Style Allocation
                    nextNeedDayExists)
                {
                    lastNeedDayList = nextNeedDayList; // MID Track 5525 AnF Defect 1618: Rounding Error
                    allocationNeedDayList = lastNeedDayList; // MID Track 5525 AnF Defect 1618: Rounding Error
                    StoreSalesITHorizon ssIH; // TT#4345 - MD - jellis - GA VSW calculated incorrectly
                    Horizon_ID horizonID;     // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
                    while
                        (UnitsAllocatedInPass &&
                        PlanExists &&
                        (aNA.UnitsAllocated < aNA.UnitsToAllocate || workUpBuy != eWorkUpBuyAllocationType.NotWorkUpAllocationBuy) && // MID Track 3810 Size Allocation GT Style Allocation
                        nextNeedDayExists)
                    {
                        PlanExists = false;
                        UnitsAllocatedInPass = false;
                        nextNeedDayExists = false;

                        // begin MID Track 5525 AnF Defect 1618: Rounding Error
                        storePlans =
                            aAllocationProfile.GetStoreOTSPlan(aStoreList.ArrayList, aAllocationProfile.OnHandDayProfile, nextNeedDayList);
                        // end MID Track 5525 AnF Defect 1618: Rounding Error
                        // Set up intransit scope
                        for (int i = 0; i < aStoreList.Count; i++)
                        {
                            // begin MID Track 5525 AnF Defect 1618: Rounding error
                            if (aNA.GetNomineePlan(i) < storePlans[i])
                            {
                                PlanExists = true;
                                thereWasAPlan = true;
                                aNA.SetNomineePlan(i, storePlans[i]);
                            }
                            else
                            {
                                nextNeedDayList[i] = lastNeedDayList[i];  // Reset Need Day because plans did not change
                            }
                            // end MID Track 5525 AnF Defect 1618: Rounding Error
                            sp = (StoreProfile)aStoreList.ArrayList[i];
                            storeIndexRID = aAllocationProfile.StoreIndex(sp.Key);
                            lastNeedDayProfile = (DayProfile)nextNeedDayList[i];
                            toDay = (lastNeedDayProfile.Date.AddDays(-1));
                            storeToDay[storeIndexRID.Index] = (toDay.Year) * 1000 + toDay.DayOfYear;
                            if (lastNeedDayProfile.Date > aAllocationProfile.LastNeedDay)
                            {
                                aAllocationProfile.LastNeedDay = lastNeedDayProfile.Date;
                            }
                        }
                        lastNeedDayProfile =
                            _session.Calendar.GetDay(aAllocationProfile.LastNeedDay);

                        // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
                        //_tran.GetIntransitReader().SetStoreIT_DayRange(
                        //    allStoreRIDs,
                        //    aAllocationProfile.OnHandHnRID,
                        //    storeFromDay,
                        //    storeToDay);
                        horizonID = new Horizon_ID(aAllocationProfile.OnHandDayProfile.Date, aAllocationProfile.ShipToDay);
                        ssIH = new StoreSalesITHorizon(aAllocationProfile.AppSessionTransaction, horizonID, storeFromDay, storeToDay);
                        // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly

                        // begin MID Track 5525 AnF Defect 1618: Rounding Error
                        //storePlans = 
                        //	this.GetStoreOTSPlan(aStoreList.ArrayList, this.OnHandDayProfile, nextNeedDayList);
                        // end MID Track 5525 AnF Defect 1618: Rounding Error
                        for (int i = 0; i < aStoreList.Count; i++)
                        {
                            aNA.SetNomineeInTransit(i,
                                _tran.GetStoreInTransit(
                                aAllocationProfile.OnHandHnRID,
                                ssIH,   // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
                                iktArray,
                                ((StoreProfile)aStoreList.ArrayList[i]).Key));
                            // begin MID Track 5525 AnF Defect 1618: Rounding error
                            //if (aNA.GetNomineePlan(i) < storePlans[i])
                            //{
                            //	PlanExists = true;
                            //	thereWasAPlan = true;
                            //}
                            //aNA.SetNomineePlan(i, storePlans[i]);
                            // end MID Track 5525 AnF Defect 1618: Rounding Error

                            // Begin TT#617 - RMatelic - Allocation Override - Add Ship Up To Rule (#36, #39)
                            storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)(aStoreList.ArrayList[i])).Key);
                            int nomineeMaximum = aNA.GetNomineeMaximum(i);
                            int nomineeShipUpTo = aAllocationProfile.GetStoreShipUpTo(eAllocationSummaryNode.Total, storeIndexRID);
                            if (nomineeShipUpTo > 0)
                            {
                                int nomineeShipUpToMaximum = nomineeShipUpTo - (int)aNA.GetNomineeOnHand(i) - aNA.GetNomineeInTransit(i);
                                if (nomineeShipUpToMaximum < 0)
                                {
                                    nomineeShipUpToMaximum = 0;
                                }
                                aNA.SetNomineeMaximum(i, Math.Min(nomineeShipUpToMaximum, nomineeMaximum));
                            }
                            // End TT#617 - 
                        }
                        if (PlanExists)
                        {
                            // change parameters dynamically later.....
                            bool allocateNewStores = true; // always true!  New stores always get first choice on first consideration!
                            bool equalizePctNeed = true;
                            bool giveMinOrUptoMax = (aAllocationProfile.ShipToDay != Include.UndefinedDate);
                            bool letExceedMax = false;
                            aNA.AllocateByNeed(allocateNewStores, false, equalizePctNeed, giveMinOrUptoMax, letExceedMax, workUpBuy); // MID Track 3786 Change Fill Size holes algorithm // MID Track 3810 Size Allocation GT Style Allocation

                            lastNeedDayList = nextNeedDayList;
                            if (aAllocationProfile.ShipToDay == Include.UndefinedDate)
                            {
                                // Begin TT#697 - JSmith - Performance
                                //nextNeedDayList = 
                                //    _SAB.StoreServerSession.DetermineInStoreOnHandDay
                                //    (nextNeedDayList, aStoreList);
                                int[] storeRIDs = new int[aStoreList.Count];
                                for (int i = 0; i < aStoreList.Count; i++)
                                {
                                    sp = (StoreProfile)aStoreList[i];
                                    storeRIDs[i] = sp.Key;
                                }

                                nextNeedDayList =
                                    StoreMgmt.DetermineInStoreOnHandDay //aAllocationProfile.SAB.StoreServerSession.DetermineInStoreOnHandDay
                                    (nextNeedDayList, storeRIDs, _sab.ApplicationServerSession);   // TT#1517 - MD - Store Service Optimization - SRISCH
                                // End TT#697
                            }

                            for (int i = 0; i < aStoreList.Count; i++)
                            {
                                if (aNA.GetNomineeUnitsAllocated(i) != storeUnitsAllocated[i])
                                {
                                    UnitsAllocatedByNeed = true;
                                    UnitsAllocatedInPass = true;
                                    storeUnitsAllocated[i] = aNA.GetNomineeUnitsAllocated(i);
                                    allocationNeedDayList[i] = lastNeedDayList[i]; // MID Track 5525 AnF Defect 1618: Rounding Error (retain "need day" of last allocation)
                                }
                                if (((DayProfile)nextNeedDayList[i]).YearDay >
                                    ((DayProfile)lastNeedDayList[i]).YearDay)
                                {
                                    // begin MID Track 5525 AnF Defect ID 1618: Rounding Error when applying % Need Limit
                                    //if (aNA.GetNomineePercentNeed(i) > aNA.PercentNeedLimit   // MID Track 4278 Need Day extended after Need Limit met
                                    if ((aNA.GetNomineeUnitNeed(i) > aNA.GetNomineeUnitNeedLimit(i))
                                        && aNA.GetNomineeUnitsAllocated(i) < aNA.GetNomineeMaximum(i) // MID Track 4278 Need Day extended after Need Limit met // MID Track 5525 AnF Defect 1618: Rounding Error
                                        && aNA.IdentifySubNodeCandidates(i, aNA.GetNodeComponent()))  // MID Track 5525 AnF Defect 1618: Rounding Error 
                                    {                                                        // MID Track 4278 Need Day extended after Need Limit met
                                        nextNeedDayExists = true;
                                    }                                                        // MID Track 4278 Need Day extended after Need Limit met
                                    else                                                     // MID Track 4278 Need Day extended after Need Limit met 
                                    {                                                        // MID Track 4278 Need Day extended after Need Limit met
                                        nextNeedDayList[i] = lastNeedDayList[i];             // MID Track 4278 Need Day extended after Need Limit met
                                    }                                                        // MID Track 4278 Need Day extended after Need Limit met
                                }
                            }
                        }
                    }

                    if (UnitsAllocatedByNeed)
                    {
                        aAllocationProfile.NeedAllocationPerformed = true;
                        NodeComponent nc = aNA.GetNodeComponent();
                        switch (nc.NodeType)
                        {
                           case (eNeedAllocationNode.BulkColor):
                                {
                                    HdrColorBin color = aAllocationProfile.GetHdrColorBin((int)nc.NodeID);
                                    for (int i = 0; i < aStoreList.ArrayList.Count; i++)
                                    {
                                        aAllocationProfile.SetStoreLastNeedDay(
                                            color,
                                            aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key),
                                            ((DayProfile)allocationNeedDayList[i]).Date);
                                    }
                                    break;
                                }
                            case (eNeedAllocationNode.GenericPack):
                            case (eNeedAllocationNode.NonGenericPack):
                                {
                                    PackHdr pack = aAllocationProfile.GetPackHdr((string)nc.NodeID);
                                    for (int i = 0; i < aStoreList.ArrayList.Count; i++)
                                    {
                                        aAllocationProfile.SetStoreLastNeedDay(
                                            pack,
                                            aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key),
                                            ((DayProfile)allocationNeedDayList[i]).Date);
                                    }
                                    break;
                                }
                            default:
                                {
                                    Object allocationSummaryNode;
                                    AllocationProfile[] apList; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                    switch (nc.NodeType)
                                    {
                                        case (eNeedAllocationNode.Group):
                                            {
                                                allocationSummaryNode = eAllocationSummaryNode.Total;
                                                apList = ((AssortmentProfile)aAllocationProfile).AssortmentMembers;
                                                break;
                                            }
                                        case (eNeedAllocationNode.Total):
                                            {
                                                allocationSummaryNode = eAllocationSummaryNode.Total;
                                                apList = new AllocationProfile[0]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                                break;
                                            }
                                        case (eNeedAllocationNode.DetailType):
                                            {
                                                allocationSummaryNode = eAllocationSummaryNode.DetailType;
                                                apList = new AllocationProfile[0]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                                break;
                                            }
                                        case (eNeedAllocationNode.Bulk):
                                            {
                                                allocationSummaryNode = eAllocationSummaryNode.Bulk;
                                                apList = new AllocationProfile[0]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                                break;
                                            }
 
                                        default:
                                            {
                                                allocationSummaryNode = null;
                                                apList = new AllocationProfile[0]; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                                                break;
                                            }
                                    }
                                    if (allocationSummaryNode != null)
                                    {
                                        for (int i = 0; i < aStoreList.ArrayList.Count; i++)
                                        {
                                            aAllocationProfile.SetStoreLastNeedDay(
                                                (eAllocationSummaryNode)allocationSummaryNode,
                                                aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key),
                                                ((DayProfile)allocationNeedDayList[i]).Date); 
                                            foreach (AllocationProfile ap in apList)
                                            {
                                                ap.SetStoreLastNeedDay(
                                                    (eAllocationSummaryNode)allocationSummaryNode,
                                                    aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key),
                                                    ((DayProfile)allocationNeedDayList[i]).Date); 
                                            }
                                        }
                                    }
                                    break;
                                }
                        }                        
                        ProcessNeedNodeResult(aAllocationProfile, nc, aStoreList, eDistributeChange.ToParent);
                    }
                    if (!thereWasAPlan)
                    {
                        text = new StringBuilder(_session.Audit.GetText(eMIDTextCode.msg_al_ThereWasNoPlan, false));
                        text.Replace("{0}", aAllocationProfile.HeaderID);
                        _session.Audit.Add_Msg(
                            eMIDMessageLevel.Warning, // MID Track 5778 (Part 2) - Scheduler 'Run Now' feature gets error on audit
                            eMIDTextCode.msg_al_ThereWasNoPlan,
                            text.ToString(),
                            this.GetType().Name);
                    }
                }
                if (!UnitsAllocatedByNeed)
                {
                    if (workUpBuy != eWorkUpBuyAllocationType.NotWorkUpAllocationBuy) // MID Track 3810 Size Allocation GT Style Allocation
                    {
                        if (nextNeedDayExists)
                        {
                            if (thereWasAPlan)
                            {
                                text = new StringBuilder(_session.Audit.GetText(eMIDTextCode.msg_al_NoWorkUpBuyNeed, false));
                                text.Replace("{0}", aAllocationProfile.HeaderID);
                                _session.Audit.Add_Msg(
                                    eMIDMessageLevel.Warning, // MID Track 5778 (Part 2) - Scheduler 'Run Now' Feature gets error on audit
                                    eMIDTextCode.msg_al_NoWorkUpBuyNeed,
                                    text.ToString(),
                                    this.GetType().Name);
                            }
                        }
                    }
                    if (aNA.UnitsAllocated >= aNA.UnitsToAllocate)
                    {
                        text = new StringBuilder(_session.Audit.GetText(eMIDTextCode.msg_al_NoUnitsToAllocateByNeed, false));
                        text.Replace("{0}", aAllocationProfile.HeaderID);
                        _session.Audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_NoUnitsToAllocateByNeed,
                            text.ToString(),
                            this.GetType().Name);
                    }
                    text = new StringBuilder(_session.Audit.GetText(eMIDTextCode.msg_al_NoUnitsAllocatedByNeedAction, false));
                    text.Replace("{0}", aAllocationProfile.HeaderID);
                    _session.Audit.Add_Msg(
                        eMIDMessageLevel.Information,
                        eMIDTextCode.msg_al_NoUnitsAllocatedByNeedAction,
                        text.ToString(),
                        this.GetType().Name);
                }
            }
            catch (MIDException e)
            {
                ActionStatus = false;
                aStatusMsg = e;
            }
            catch (Exception e)
            {
                ActionStatus = false;
                aStatusMsg =
                    new MIDException(
                        eErrorLevel.severe,
                        (int)eMIDTextCode.systemError,
                        e.Message, e); // TT#1173 - MD - JEllis - MID Error #20000 -- no explanation

            }
            finally
            {
            }
            return ActionStatus;
        }

        /// <summary>
        /// Update store allocation with need allocation result
        /// </summary>
        /// <param name="aNode">Detail node to update (result will be reflected in parent but not child component nodes)</param>
        /// <param name="aStoreList">List of included stores.</param>
        private void ProcessNeedNodeResult(AllocationProfile aAllocationProfile, NodeComponent aNode, ProfileList aStoreList, eDistributeChange aDistributeChange)
        {
            int nodeIndex = 0;
            if (aNode.SubnodeDimension > 0 && aNode.AllocateSubNodes == true)
            {
                if (aNode.NodeType == eNeedAllocationNode.Group)
                {
                    AllocationProfile[] apList = ((AssortmentProfile)aAllocationProfile).AssortmentMembers; // TT#891 - MD - Jellis - Group Allocation Need Action Gets Error
                    for (int i = 0; i < aNode.SubnodeDimension; i++)
                    {
                        ProcessNeedNodeResult(
                            apList[i], aNode.GetChildNodeComponent(aNode, nodeIndex), aStoreList, eDistributeChange.ToNone);
                        nodeIndex++;
                    }
                }
                else
                {
                    for (int i = 0; i < aNode.SubnodeDimension; i++)
                    {
                        ProcessNeedNodeResult(
                            aAllocationProfile, aNode.GetChildNodeComponent(aNode, nodeIndex), aStoreList, eDistributeChange.ToNone);
                        nodeIndex++;
                    }
                }
            }

            Index_RID storeIndexRID;
            switch (aNode.NodeType)
            {
                case (eNeedAllocationNode.BulkColor):
                    {
                        HdrColorBin color = aAllocationProfile.GetHdrColorBin((int)aNode.NodeID);
                        for (int i = 0; i < aStoreList.Count; i++)
                        {
                            storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);
                            if (aAllocationProfile.GetStoreQtyAllocated(color, storeIndexRID)
                                != aNode.GetNomineeUnitsAllocated(i))
                            {
                                aAllocationProfile.SetStoreQtyAllocated(
                                    color,
                                    storeIndexRID,
                                    aNode.GetNomineeUnitsAllocated(i),
                                    aDistributeChange,
                                    false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                    false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                    false);  // TT#946 - MD - Jellis - Group Allocation Not Working
                                aAllocationProfile.SetStoreQtyAllocatedByAuto(color, storeIndexRID, aAllocationProfile.GetStoreQtyAllocated(color, storeIndexRID));
                                aAllocationProfile.SetStoreWasAutoAllocated(color, storeIndexRID, true);
                                aAllocationProfile.SetStoreHadNeed(color, storeIndexRID, true);
                            }
                        }
                        break;
                    }
                case (eNeedAllocationNode.GenericPack):
                case (eNeedAllocationNode.NonGenericPack):
                    {
                        PackHdr pack = aAllocationProfile.GetPackHdr((string)aNode.NodeID);
                        int qtyAllocated;
                        for (int i = 0; i < aStoreList.Count; i++)
                        {
                            storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);
                            qtyAllocated = aNode.GetNomineeUnitsAllocated(i) / pack.PackMultiple;
                            if (aAllocationProfile.GetStoreQtyAllocated(pack, storeIndexRID)
                                != qtyAllocated)
                            {
                                aAllocationProfile.SetStoreQtyAllocated(
                                    pack,
                                    storeIndexRID,
                                    qtyAllocated,
                                    aDistributeChange,
                                    false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                    false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                    false);  // TT#946 - MD - Jellis - Group Allocation Not Working
                                aAllocationProfile.SetStoreQtyAllocatedByAuto(pack, storeIndexRID, aAllocationProfile.GetStoreQtyAllocated(pack, storeIndexRID));
                                aAllocationProfile.SetStoreWasAutoAllocated(pack, storeIndexRID, true);
                                aAllocationProfile.SetStoreHadNeed(pack, storeIndexRID, true);
                            }
                        }
                        break;
                    }
                default:
                    {
                        Object allocationSummaryNode = null;
                        switch (aNode.NodeType)
                        {
                            case (eNeedAllocationNode.Group):
                                {
                                    //allocationSummaryNode = eAllocationSummaryNode.Total;  // TT#946 - MD - Jellis - Group Allocation Not Working
                                    break;
                                }
                            case (eNeedAllocationNode.Total):
                                {
                                    allocationSummaryNode = eAllocationSummaryNode.Total;
                                    break;
                                }
                            case (eNeedAllocationNode.DetailType):
                                {
                                    allocationSummaryNode = eAllocationSummaryNode.DetailType;
                                    break;
                                }
                            case (eNeedAllocationNode.Bulk):
                                {
                                    allocationSummaryNode = eAllocationSummaryNode.Bulk;
                                    break;
                                }

                            default:
                                {
                                    //assume color-size
                                    HdrColorBin color = aAllocationProfile.GetHdrColorBin((int)(((long)aNode.NodeID) >> 32));
                                    HdrSizeBin size = color.GetSizeBin((int)(aNode.NodeID));
                                    for (int i = 0; i < aStoreList.Count; i++)
                                    {
                                        storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);
                                        if (aAllocationProfile.GetStoreQtyAllocated(size, storeIndexRID)
                                            != aNode.GetNomineeUnitsAllocated(i))
                                        {
                                            aAllocationProfile.SetStoreQtyAllocated(
                                                size,
                                                storeIndexRID,
                                                aNode.GetNomineeUnitsAllocated(i),
                                                aDistributeChange,
                                                false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                                false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                                false);  // TT#946 - MD - Jellis - Group Allocation Not Working
                                            aAllocationProfile.SetStoreQtyAllocatedByAuto(size, storeIndexRID, aAllocationProfile.GetStoreQtyAllocated(size, storeIndexRID));
                                            aAllocationProfile.SetStoreWasAutoAllocated(size, storeIndexRID, true);
                                            aAllocationProfile.SetStoreHadNeed(size, storeIndexRID, true);
                                        }
                                    }
                                    break;
                                }
                        }
                        if (allocationSummaryNode != null)
                        {
                            for (int i = 0; i < aStoreList.Count; i++)
                            {
                                storeIndexRID = aAllocationProfile.StoreIndex(((StoreProfile)aStoreList.ArrayList[i]).Key);
                                if (aAllocationProfile.GetStoreQtyAllocated((eAllocationSummaryNode)allocationSummaryNode, storeIndexRID)
                                    != aNode.GetNomineeUnitsAllocated(i))
                                {
                                    aAllocationProfile.SetStoreQtyAllocated(
                                        (eAllocationSummaryNode)allocationSummaryNode,
                                        storeIndexRID,
                                        aNode.GetNomineeUnitsAllocated(i),
                                        aDistributeChange,
                                        false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                        false,   // TT#946 - MD - Jellis - Group Allocation Not Working
                                        false);  // TT#946 - MD - Jellis - Group Allocation Not Working
                                    aAllocationProfile.SetStoreQtyAllocatedByAuto((eAllocationSummaryNode)allocationSummaryNode, storeIndexRID, aAllocationProfile.GetStoreQtyAllocated((eAllocationSummaryNode)allocationSummaryNode, storeIndexRID));
                                    aAllocationProfile.SetStoreWasAutoAllocated((eAllocationSummaryNode)allocationSummaryNode, storeIndexRID, true);
                                    aAllocationProfile.SetStoreHadNeed((eAllocationSummaryNode)allocationSummaryNode, storeIndexRID, true);
                                }
                            }
                        }
                        break;
                    }
            }
        }
        #endregion Process Need Action
        #endregion Methods
    }
}