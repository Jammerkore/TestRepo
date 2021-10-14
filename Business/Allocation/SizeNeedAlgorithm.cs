using System;
using System.Collections;
using System.Collections.Generic;  // TT#1391 - TMW New Action (Unrelated - Performance)
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
    /// <summary>
    /// Summary description for SizeNeedAlgorithm.
    /// </summary>
    public class SizeNeedAlgorithm
    {
        private ApplicationSessionTransaction _transaction;
        private NeedAlgorithms _needAlgorithm;
        private AllocationProfile _allocProfile;
        public GeneralComponent _targetComponent;   // TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        private SizeCurveGroupProfile _sizeCurveGroup;
        private SizeCurveProfile _sizeCurve;
        private ProfileList _curveStoreList;     // MID Track 3631 Rules apply to all sizes on header
        private ProfileList _ruleStoreList; // MID Track 3631 Rules apply to all sizes on header
        private int _headerColorRid; // header color RID when a "bulk" color is processed; "dummy" color when processing detail packs  // MID Track 3749 "size need mdse basis" s/b detail color for Color level and "all" color for higher levels
        private SizeNeedResults _sizeNeedResults;  // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        private SessionAddressBlock _sab;
        private SizeCodeList _sizeCodeList; // current array of SizeCodeProfiles;
        private HdrColorBin _color;
        private Dictionary<int, PackSizeBin> _packSizes;	// TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
        //private Hashtable _sizeCurveHash;  // TT#2155 - JEllis - Fill Size Null Reference
        private Hashtable _sizeRestrictedCurveHash;
        private HierarchyNodeProfile _mdseSizeBasis;  // MID Track 3749
        private HierarchyNodeProfile _ibMdseSizeBasis; // TT#41 - MD - JEllis - Size Inventory Min Max
        private int _allocationMultiple;
        //private eMethodType _methodType;  // TEMP TT#2155
        private int _constraintModelRid;
        private int _alternateModelRid;
        private CollectionRuleSets _ruleCollection;
        private SizeConstraintModelProfile _constraintModel;
        private SizeAltModelProfile _alternateModel;
        private CollectionDecoder _rulesDecoder;
        private CollectionDecoder _constraintDecoder;
        private Hashtable _constraintStoreGroupLevelHash;
        private Hashtable _rulesStoreGroupLevelHash;
        private Hashtable _sizeAlternateHash;
        private ProfileList _sizeRuleList; // MID Track 3619 Remove Fringe
        //private bool _addSizeCurvePct;    // MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
        private int _basisColorRid;  // determines ownership for a need allocation based on selected MDSE level: color and size levels use that color's size ownership; style level and above use total color size ownership // MID Track 3749
        private int _ibBasisColorRid; // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct results
        private eFillSizesToType _FillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation

        // begin TT#702 Infinite Loop when begin date set
        private eSizeNeedColorPlan _sizeColorPlanType;

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="aTrans"></param>
        /// <param name="aAllocationProfile">Allocation profile of the header for which a size need allocation is being requested</param>
        /// <param name="aComponent">The header component for which a size need allocation is requested</param>
        /// <param name="aSizeCurveGroup">Size Curve Group that will be used to calculate the size plans</param>
        /// <param name="aStoreList">List of store profiles that identify the stores for which a size need allocation is requested</param>
        /// <param name="aMerchandiseType">Merchandise Type</param>
        /// <param name="aMdseHnRID">Merchandise Hierarchy Node RID</param>
        /// <param name="aMdsePhRID">Merchandise Product Hierarchy RID</param>
        /// <param name="aMdsePhlSequence">Product Hierarchy Level Sequence</param>
        /// <param name="aStoreGroupRid">Store Group RID</param>
        /// <param name="aAlternateModelRid">Alternate Size Model RID</param>
        /// <param name="aConstraintModelRid">Constraint model RID</param>
        /// <param name="aRuleCollection">Rule Colection</param>
        /// <param name="aSizeColorPlanType">Size Color Plan Type: One color or All Colors</param>
        /// <param name="aNormalizeSizeCurves">True:  Size curves will be normalized (ie. percentages in sizes missing from header will be pro-rated back to sizes on header); False: Size Curves will not be normalized</param> 
        /// <param name="aIB_MerchandiseType">Merchandise Type of the size constraint inventory basis; when "undefined", inventory basis defaults to size need basis</param>
        /// <param name="aIB_MdseHnRID">Merchandise Hierarchy Node of the size constraint inventory basis</param>
        /// <param name="aIB_MdsePhRID">Product Hierarchy Level of the size constraint inventory basis</param>
        /// <param name="aIB_MdsePhlSequence">Product Hierarchy Level Sequence of the size constrain inventoy basis</param>
        public SizeNeedAlgorithm(
            ApplicationSessionTransaction aTrans,
            //eMethodType aMethodType,  // TT#2155 - JEllis - Fill Size Null Reference
            AllocationProfile aAllocationProfile,
            GeneralComponent aComponent,
            CollectionRuleSets aRuleCollection,
            int aAlternateModelRid,
            int aConstraintModelRid,
            SizeCurveGroupProfile aSizeCurveGroup,
            ProfileList aStoreList,
            eMerchandiseType aMerchandiseType,
            int aMdseHnRID,
            int aMdsePhRID,
            int aMdsePhlSequence,
            int aStoreGroupRid,
            eSizeNeedColorPlan aSizeColorPlanType,
            // begin TT#41 - MD - JEllis - Size Inventory Min Max
            bool aNormalizeSizeCurves,
            eMerchandiseType aIB_MerchandiseType,
            int aIB_MdseHnRID,
            int aIB_MdsePhRID,
            int aIB_MdsePhlSequence, // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
            eVSWSizeConstraints aVswSizeConstraints, // TT#246 - MD Jellis - AnF VSW In Store Minimums pt 5
            eFillSizesToType aFillSizesToType //TT#848-MD -jsobek -Fill to Size Plan Presentation
            ) 
            //bool aNormalizeSizeCurves)
            // end TT#41 - MD - JEllis - Size inventory Min Max
        {
            _transaction = aTrans;
            //_methodType = aMethodType;  // TT#2155 - JEllis - Fill Size Null Reference
            _allocProfile = aAllocationProfile;
            _targetComponent = aComponent;
            _ruleCollection = aRuleCollection;
            _alternateModelRid = aAlternateModelRid;
            _constraintModelRid = aConstraintModelRid;
            _sizeCurveGroup = aSizeCurveGroup;
            _sizeColorPlanType = aSizeColorPlanType;

            _sab = _transaction.SAB;

            // Begin TT#5704 - JSmith - Size Minimums do not Hold when VSW Size Constraint set
			//_sizeNeedResults = new SizeNeedResults(aAllocationProfile, aVswSizeConstraints); // TT#246 - MD - Jellis - AnF VSW In store Minimums pt 5  // TT#519 - MD - JEllis - AnF VSW - Minimums not working
            bool newSizeNeedResults = false;
            int colorRid =  Include.DummyColorRID;
            if (_targetComponent.ComponentType == eComponentType.SpecificColor)
            {
                colorRid = ((AllocationColorOrSizeComponent)_targetComponent).ColorRID;
            }
            _sizeNeedResults = _allocProfile.AppSessionTransaction.GetSizeNeedResults(_allocProfile.HeaderRID, colorRid);

            if (_sizeNeedResults == null)
            {
                _sizeNeedResults = new SizeNeedResults(aAllocationProfile, aVswSizeConstraints);
                newSizeNeedResults = true;
            }
			// End TT#5704 - JSmith - Size Minimums do not Hold when VSW Size Constraint set
            //_sizeNeedResults = new SizeNeedResults();   // TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
            //_sizeCurveHash = new Hashtable();                       // TT#2155 - JEllis - Fill Size Null Reference
            _sizeRestrictedCurveHash = new Hashtable();
            _packSizes = new Dictionary<int, PackSizeBin>();	// TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk

            //_sizeNeedResults.MethodType = eSizeMethodType.SizeNeedAllocation;  // TT#2155 - JEllis - Fill Size Null Reference
            _sizeNeedResults.Stores = aStoreList;
            _sizeNeedResults.ProcessControl = eSizeProcessControl.SizeCurvesOnly;
            _sizeNeedResults.NormalizeSizeCurves = aNormalizeSizeCurves;
            _FillSizesToType = aFillSizesToType; //TT#848-MD -jsobek -Fill to Size Plan Presentation
            _sizeNeedResults.InventoryMdseBasisRID = Include.NoRID; // TT#1074 - MD - Jellis- Inventory Min Max incorrect for Group Allocation

            if (_targetComponent.ComponentType == eComponentType.DetailType)
            {
                // Assume for now NO Color in packs
                //_headerColorRid = Include.DummyColorRID;  // TT#3112 - MD - Jellis - Size Pack Allocation Overallocates Sizes
                _headerColorRid = aAllocationProfile.HeaderColorCodeRID; // TT#3112 - MD - Jellis - Size Pack Allocation Overallocates Sizes
                // begin TT#1436 - MD - JEllis - GA allocates bulk before packs
                if (_headerColorRid < 0)
                {
                    _headerColorRid = Include.DummyColorRID;
                }
                // end TT#1436 - MD - JEllis - GA allocates bulk before packs
                _color = (HdrColorBin)_allocProfile.BulkColors[_headerColorRid]; // TT#4121 - MD - Jellis - Null Reference (original fix moved here from SizeFringeAlgorithm)  //  TT#4150 - MD - Jellis - Re_Do TT#4121 Fix_To Remove TT#1068 Conflict
                _allocationMultiple = 1;
                foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values)
                {
                    foreach (PackColorSize pcs in ph.PackColors.Values)
                    {
                        foreach (PackSizeBin packSize in pcs.ColorSizes.Values)
                        {
                            if (!_sizeNeedResults.Sizes.Contains(packSize.ContentCodeRID))
                            {
                                if (packSize.ContentUnits > 0)
                                {
                                    _packSizes.Add(packSize.ContentCodeRID, packSize);	// TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                    _sizeNeedResults.AddSize(packSize.ContentCodeRID);
                                }
                            }
                        }
                    }
                }
                // begin TT#1410 - FL Detail Pack Allocation not giving all packs
                foreach (HdrColorBin hcb in _allocProfile.BulkColors.Values)
                {
                    foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
                    {
                        if (hsb.SizeUnitsToAllocate > 0)
                        {
                            if (!_sizeNeedResults.Sizes.Contains(hsb.SizeCodeRID))
                            {
                                _sizeNeedResults.AddSize(hsb.SizeCodeRID);
                            }
                        }
                    }
                }
                // end TT#1410 - FL Detail Pack Allocation not giving all packs
            }
            else if (_targetComponent.ComponentType == eComponentType.SpecificColor)
            {
                _headerColorRid = ((AllocationColorOrSizeComponent)_targetComponent).ColorRID;
                _color = (HdrColorBin)_allocProfile.BulkColors[_headerColorRid];

				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpTotalBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    foreach (int sizeRID in _color.ColorSizes.Keys)
                    {
                        _sizeNeedResults.AddSize(sizeRID);
                    }
                }
                else
                {
                    foreach (HdrSizeBin hsb in _color.ColorSizes.Values)
                    {
                        if (hsb.SizeUnitsToAllocate > 0)
                        {
                            _sizeNeedResults.AddSize(hsb.SizeCodeRID);
                        }
                    }
                }
                _allocationMultiple = _color.ColorMultiple;
            }
            else
            {
                throw new Exception("Component for size need must be detail or color");
            }

            _sizeNeedResults.HeaderColorRid = _headerColorRid;
            _sizeNeedResults.Color = _color;
            _sizeNeedResults.Component = _targetComponent;

            BuildSizeModels(_constraintModelRid, _alternateModelRid);
            BuildRulesDecoder(aStoreGroupRid);

            _mdseSizeBasis = this.GetMdseBasis(
                _allocProfile,
                aMerchandiseType,
                aMdseHnRID,
                aMdsePhRID,
                aMdsePhlSequence
                );

            // begin TT#41 - MD - JEllis - Size Inventory Min Max
            if (aIB_MerchandiseType == eMerchandiseType.Undefined)
            {
                _ibMdseSizeBasis = _mdseSizeBasis;
                _ibBasisColorRid = _basisColorRid; // TT#304 - MD - JEllis - Size Inventory Min Max not getting correct results
            }
            else
            {
                _ibMdseSizeBasis = GetIbMdseBasis( // TT#304 - MD - JEllis - Size Inventory Min Max not getting correct results
                    _allocProfile,
                    aIB_MerchandiseType,
                    aIB_MdseHnRID,
                    aIB_MdsePhRID,
                    aIB_MdsePhlSequence
                    );
            }
            // end TT#41 - MD - JEllis - Size Inventory Min Max
			// Begin TT#5704 - JSmith - Size Minimums do not Hold when VSW Size Constraint set
			//_transaction.SetSizeNeedResults(_allocProfile.HeaderRID, _sizeNeedResults); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            if (newSizeNeedResults)
            {
                _transaction.SetSizeNeedResults(_allocProfile.HeaderRID, _sizeNeedResults); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }
			// End TT#5704 - JSmith - Size Minimums do not Hold when VSW Size Constraint set

            CalculateSizeCurves(_sizeCurveGroup);
        }


        // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //public SizeNeedResults SizeNeedResults
        //{
        //    get { return _sizeNeedResults; }
        //}
        // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        /// <summary>
        /// Performs a size need allocation
        /// </summary>
        /// <param name="processControl">Process Type: Size Curves only, Size Curves and Plan or All</param>
        /// <returns>Size allocation based on size need.</returns>
        public SizeNeedResults ProcessSizeNeed(
           eSizeProcessControl processControl,  // TT#2155 - JEllis - Fill SIze Holes Null Reference
            bool aApplyRulesOnly)               // TT#2155 - JEllis - Fill Size Holes Null Reference
        {
            try
            {
                _sizeNeedResults.ProcessControl = processControl;
                // Begin TT#784 - Size analysis shows double intransit - Jellis
                if (this._sizeNeedResults.GetOnHandAndIT)
                {
                    BuildOnHandAndIntransit(); // TT#702 Infinite Loop when begin date set  // TT#766 - JEllis - ?????
                }
                // end TT#784 - Size analysis shows double intransit - Jellis

                // begin TT#1600 - Size Need Algorithm Error
                if (_sizeNeedResults.AccumulatePriorAllocated)
                {
                    BuildPriorAllocated();
                }
                // end TT#1600 - Size Need Algorithm Error

                if (processControl != eSizeProcessControl.SizeCurvesOnly)
                {
                    CalculatePlan(_sizeCurveGroup, _sizeColorPlanType, eSizeMethodType.SizeNeedAllocation, false, false);
                }


                if (processControl == eSizeProcessControl.ProcessAll)
                {
                    if (_allocProfile.BulkColorIsOnHeader(this._headerColorRid))
                    {
                        if (_sizeRuleList != null)
                        {
                            if (_sizeRuleList.Count > 0)
                            {
                                ProcessSizeRule();
                            }
                        }
                    }
                    if (!aApplyRulesOnly                // TT#2155 - JEllis - Fill SIze Holes Null Reference
                        && this._sizeCurveGroup.Key != Include.NoRID)
                    {
                        CalculateSizeNeed(_sizeColorPlanType);
                    }
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    else
                    {
                        _transaction.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_SizeNeedRulesSuccessfullyApplied,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_al_SizeNeedRulesSuccessfullyApplied), _allocProfile.HeaderID),
                            GetType().Name);
                    }
                    // end TT#2155 - JEllis _ Fill Size Holes Null Reference
                }

                return _sizeNeedResults;
            }
            catch (Exception ex)
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    eMIDTextCode.msg_al_SizeNeedAlgorithmFailed,
                    ex.ToString(),
                    this.ToString());
                throw;
            }
        }
        ///// <summary>
        ///// Creates an instance of this class
        ///// </summary>
        ///// <param name="aTrans"></param>
        //public SizeNeedAlgorithm(ApplicationSessionTransaction aTrans, eMethodType methodType)
        //{
        //    _transaction = aTrans;
        //    _sab = aTrans.SAB;
        //    _sizeNeedResults = new SizeNeedResults();
        //    _sizeCurveHash = new Hashtable();
        //    _sizeRestrictedCurveHash = new Hashtable();
        //    _methodType = methodType;
        //}


        ///// <summary>
        ///// Performs a size need allocation
        ///// </summary>
        ///// <param name="allocProfile">Allocation profile of the header for which a size need allocation is being requested</param>
        ///// <param name="aComponent">The header component for which a size need allocation is requested</param>
        ///// <param name="sizeCurveGroup">Size Curve Group that will be used to calculate the size plans</param>
        ///// <param name="aStoreList">List of store profiles that identify the stores for which a size need allocation is requested</param>
        ///// <param name="aMerchandiseType">Merchandise Type</param>
        ///// <param name="aMdseHnRID">Merchandise Hierarchy Node RID</param>
        ///// <param name="aMdsePhRID">Merchandise Product Hierarchy RID</param>
        ///// <param name="aMdsePhlSequence">Product Hierarchy Level Sequence</param>
        ///// <param name="storeGroupRid">Store Group RID</param>
        ///// <param name="alternateModelRid">Alternate Size Model RID</param>
        ///// <param name="constraintModelRid">Constraint model RID</param>
        ///// <param name="ruleCollection">Rule Colection</param>
        ///// <param name="sizeColorPlan">Size Color Plan Type: One color or All Colors</param>
        ///// <param name="processControl">Process Type: Size Curves only, Size Curves and Plan or All</param>
        ///// <param name="aNormalizeSizeCurves">True:  Size curves will be normalized (ie. percentages in sizes missing from header will be pro-rated back to sizes on header); False: Size Curves will not be normalized</param>
        ///// <returns>Size allocation based on size need.</returns>
        //public SizeNeedResults ProcessSizeNeed(
        //    AllocationProfile allocProfile, 
        //    GeneralComponent aComponent, 
        //    SizeCurveGroupProfile sizeCurveGroup, 
        //    ProfileList aStoreList, 
        //    eMerchandiseType aMerchandiseType,
        //    int aMdseHnRID,
        //    int aMdsePhRID,
        //    int aMdsePhlSequence,
        //    int storeGroupRid,
        //    int constraintModelRid, int alternateModelRid,
        //    CollectionRuleSets ruleCollection, eSizeNeedColorPlan sizeColorPlan,
        //    eSizeProcessControl processControl, // MID Track 4861 Size Normalization
        //    bool aNormalizeSizeCurves)          // MID Track 4861 Size Normalization
        //{
        //    try
        //    {
        //        //bool process = true;
        //        _allocProfile = allocProfile;
        //        _targetComponent = aComponent;
        //        _ruleCollection = ruleCollection;

        //        _constraintModelRid = constraintModelRid;
        //        _alternateModelRid = alternateModelRid;
        //        _sizeCurveGroup = sizeCurveGroup;

        //        _sizeNeedResults.Clear(true);
        //        _sizeNeedResults.Sizes.Clear();
        //        _sizeNeedResults.MethodType = eSizeMethodType.SizeNeedAllocation;
        //        _sizeNeedResults.Stores = aStoreList; // MID Track 3631 Size Rules apply to all size on header
        //        _sizeNeedResults.ProcessControl = processControl;
        //        _sizeNeedResults.NormalizeSizeCurves = aNormalizeSizeCurves; // MID Track 4861 Size Normalization


        //        if (_targetComponent.ComponentType == eComponentType.DetailType)
        //        {
        //            // Assume for now NO Color in packs
        //            _headerColorRid = Include.DummyColorRID; // identifies the detail component of header; // MID Track 3749
        //            _allocationMultiple = 1;
        //            foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values)
        //            {
        //                foreach (PackColorSize pcs in ph.PackColors.Values)
        //                {
        //                    foreach (PackSizeBin packSize in pcs.ColorSizes.Values) // Assortment: added pack size bin
        //                    {
        //                        if (!_sizeNeedResults.Sizes.Contains(packSize.ContentCodeRID))
        //                        {
        //                            // begin MID Track 5061 Size Review store size total not correct
        //                            //_sizeNeedResults.AddSize(packSize.ContentKey);
        //                            if (packSize.ContentUnits > 0)
        //                            {
        //                                _sizeNeedResults.AddSize(packSize.ContentCodeRID);
        //                            }
        //                            // end MID Track 5061 Size Review store size total not correct
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (_targetComponent.ComponentType == eComponentType.SpecificColor)
        //        {
        //            _headerColorRid = ((AllocationColorOrSizeComponent)_targetComponent).ColorRID;  // identifies a Bulk Color as component of header; // MID Track 3749
        //            _color = (HdrColorBin)_allocProfile.BulkColors[_headerColorRid]; // MID Track 3749
        //            // begin MID Track 5061 Size Review store size total not correct	
        //            //foreach (int sizeRID in _color.ColorSizes.Keys)
        //            //{
        //            //	_sizeNeedResults.AddSize(sizeRID);
        //            //}
        //            if (_allocProfile.WorkUpTotalBuy)
        //            {
        //                foreach (int sizeRID in _color.ColorSizes.Keys)
        //                {
        //                    _sizeNeedResults.AddSize(sizeRID);
        //                }
        //            }
        //            else
        //            {
        //                foreach (HdrSizeBin hsb in _color.ColorSizes.Values)
        //                {
        //                    if (hsb.SizeUnitsToAllocate > 0)
        //                    {
        //                        _sizeNeedResults.AddSize(hsb.SizeCodeRID);  // Assortment: color/size changes
        //                    }
        //                }
        //            }
        //            // end MID Track 5061 Size Review store size total not correct
        //            _allocationMultiple = _color.ColorMultiple;
        //        }
        //        else
        //        {
        //            throw new Exception("Component for size need must be detail or color");
        //        }

        //        // Begin MID Issue # 3160 
        //        _sizeNeedResults.HeaderColorRid = _headerColorRid; // MID Track 3749
        //        _sizeNeedResults.Color = _color;
        //        _sizeNeedResults.Component = _targetComponent;
        //        // End MID Issue # 3160 

        //        BuildSizeModels(_constraintModelRid, _alternateModelRid); // MID Track 3619 Remove Fringe
        //        BuildRulesDecoder(storeGroupRid);

        //        _mdseSizeBasis = this.GetMdseBasis(  // MID Track 3749
        //            _allocProfile,
        //            aMerchandiseType,
        //            aMdseHnRID,
        //            aMdsePhRID,
        //            aMdsePhlSequence
        //            );

        //        //process = ParmValidation();  // MID Track AnF#666 Fill to Size Plan Enhancement (remove obsolete code)

        //        //if (process)  // MID Track AnF#666 Fill to Size Plan Enhancement (remove obsolete code)
        //        //{             // MID Track AnF#666 Fill to Size Plan Enhancement (remove obsolete code)
        //            CalculateSizeCurves(sizeCurveGroup);

        //            if (processControl != eSizeProcessControl.SizeCurvesOnly)
        //            {
        //                CalculatePlan(sizeCurveGroup, sizeColorPlan, eSizeMethodType.SizeNeedAllocation, false, false); // MID track 4291 add fill variables to size review // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        //            }


        //            if (processControl == eSizeProcessControl.ProcessAll)
        //            {
        //                // begin MID Track 3781 Size Curve Not Required
        //                if (_allocProfile.BulkColorIsOnHeader(this._headerColorRid)) // MID Track 3749 note: rules do not apply to detail packs 
        //                {
        //                    // begin MID Track Remove fringe model
        //                    if (_sizeRuleList != null)
        //                    {
        //                        if (_sizeRuleList.Count > 0)
        //                        {
        //                            ProcessSizeRule();
        //                        }
        //                    }
        //                    // end MID Track Remove fringe model
        //                }
        //                // end MID Track 3781 Size Curve Not Required
        //                // begin MID Track 3835 Error Processing Size Need with Size Group
        //                if (this._sizeCurveGroup.Key != Include.NoRID)
        //                {
        //                    CalculateSizeNeed(sizeColorPlan);
        //                }
        //                // end MID Track 3835 Error Processing size need with size group
        //            }

        //            return _sizeNeedResults;
        //        // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement(Remove obsolete code)
        //        //}
        //        //else
        //        //{
        //        //	throw new MIDException (eErrorLevel.severe,
        //        //		(int)eMIDTextCode.msg_al_SizeNeedAlgorithmFailed,
        //        //		MIDText.GetText(eMIDTextCode.msg_al_SizeNeedAlgorithmFailed));
        //        //}
        //        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Remove obsolete code)

        //    }
        //    catch (Exception ex)
        //    {
        //        _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_SizeNeedAlgorithmFailed, ex.ToString(), this.ToString());
        //        throw;
        //    }
        //}
        // end TT#702 Infinite Loop when begin date set

        // Begin MID issue 3160
        internal void ApplySizeNeedResults(SizeNeedResults sizeNeedResults)
        {
            _sizeNeedResults = sizeNeedResults;
            _color = sizeNeedResults.Color;
            _headerColorRid = sizeNeedResults.HeaderColorRid; // MID Track 3749
            _targetComponent = _sizeNeedResults.Component;
            if (sizeNeedResults.GetOnHandAndIT)
            {
                this.BuildOnHandAndIntransit();
            }
            // begin TT#1600 - Size Need Algorithm Error
            if (_sizeNeedResults.AccumulatePriorAllocated)
            {
                BuildPriorAllocated();
            }
            // end TT#1600 - Size Need Algorithm Error
        }
        // End MID issue 3160

        internal SizeNeedResults ProcessSizePlanOnly(SizeNeedResults sizeNeedResults, bool aCalculateFillPlan, bool aUseBasisPlan) // MID Track 4921 AnF#666 Fill to Size Plan Enhancement 
        {
            try
            {
                // Begin MID issue 3160
                ApplySizeNeedResults(sizeNeedResults);
                // End MID issue 3160
                //=============================================================================================
                // Checks to see what was previously done.  It expects that only curves have been done.
                //=============================================================================================
                // begin MID Track 4291 add fill variabes to size review
                if (aCalculateFillPlan)
                {
                    // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //CalculatePlan(_sizeCurveGroup, eSizeNeedColorPLan.PlanForSpecificColorOnly, eSizeMethodType.SizeNeedAllocation, aCalculateFillPlan); 
                    // this._sizeNeedResults.CalculateFillPlan = false;
                    CalculatePlan(_sizeCurveGroup, eSizeNeedColorPlan.PlanForSpecificColorOnly, eSizeMethodType.SizeNeedAllocation, aCalculateFillPlan, aUseBasisPlan);
                    if (aUseBasisPlan)
                    {
                        this._sizeNeedResults.CalculateFillToPlan_Plan = false;
                    }
                    else
                    {
                        this._sizeNeedResults.CalculateFillToOwn_Plan = false;
                    }
                }
                else
                    // end MID Track 4291 add fill variables to size review
                    if (_sizeNeedResults.ProcessControl == eSizeProcessControl.SizeCurvesOnly)
                    {
                        CalculatePlan(_sizeCurveGroup, eSizeNeedColorPlan.PlanForSpecificColorOnly, eSizeMethodType.SizeNeedAllocation, false, aUseBasisPlan); // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        this._sizeNeedResults.ProcessControl = eSizeProcessControl.SizeCurvesAndPlanOnly;
                    }
                    else
                    {
                        this._sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_al_SizeNeedExpectedSizePlan), this.ToString());

                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_SizeNeedExpectedSizePlan,
                            MIDText.GetText(eMIDTextCode.msg_al_SizeNeedExpectedSizePlan));
                    }
                return _sizeNeedResults;
            }
            catch
            {
                throw;
            }
        }



        internal SizeNeedResults ProcessSizeNeedOnly(SizeNeedResults sizeNeedResults)
        {
            try
            {
                // Begin MID issue 3160
                ApplySizeNeedResults(sizeNeedResults);
                // End MID issue 3160
                //=============================================================================================
                // Checks to see what was previously done.  It expects that only curves and plan has been done.
                //=============================================================================================
                if (_sizeNeedResults.ProcessControl == eSizeProcessControl.SizeCurvesAndPlanOnly)
                {
                    CalculateSizeNeed(eSizeNeedColorPlan.PlanForSpecificColorOnly);
                    this._sizeNeedResults.ProcessControl = eSizeProcessControl.ProcessAll;
                }
                else
                {
                    this._sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, MIDText.GetText(eMIDTextCode.msg_al_SizeNeedExpectedSizePlan), this.GetType().Name); // MID Track 5778 Schedule 'Run Now' feature gets error in audit

                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_SizeNeedExpectedSizePlan,
                        MIDText.GetText(eMIDTextCode.msg_al_SizeNeedExpectedSizePlan));
                }
                return _sizeNeedResults;
            }
            catch
            {
                throw;
            }
        }



        // begin TT#702 Infinite Loop when begin date set
        /// <param name="aFillSizesToType">Type of Fill Sizes Action: Fill to Holes in Ownership (Onhand plus Intransit) or to future Plan</param>
        /// <param name="availableUnits">Units available for Fill Size Holes (ignored by Size Rules)</param>
        /// <param name="processControl">Process Type: Size Curves only, Size Curves and Plan or All</param>
        public SizeNeedResults ProcessFillSize(
            eFillSizesToType aFillSizesToType,
            int availableUnits,
            eSizeProcessControl processControl,
            bool aApplyRulesOnly)  // TT#2155 - JEllis - Fill Size Holes Null Reference
        //// begin MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        ///// <summary>
        ///// Processes a Fill Size Request
        ///// </summary>
        ///// <param name="aFillSizesToType">Type of Fill Sizes Action: Fill to Holes in Ownership (Onhand plus Intransit) or to future Plan</param>
        ///// <param name="availableUnits">Units available for Fill Size Holes (ignored by Size Rules)</param>
        ///// <param name="allocProfile">Allocation Profile</param>
        ///// <param name="colorComponent">Color Component</param>
        ///// <param name="sizeCurveGroup">Size Curve Group to use (may be null in which case only Size Rules will be processed)</param>
        ///// <param name="aStoreList">Store Filter</param>
        ///// <param name="aMerchandiseType">Merchandise Type</param>
        ///// <param name="aMdseHnRID">Merchandise Hierarchy Node RID</param>
        ///// <param name="aMdsePhRID">Merchandise Product Hierarchy RID</param>
        ///// <param name="aMdsePhlSequence">Product Hierarchy Level Sequence</param>
        ///// <param name="storeGroupRid">Store Group RID</param>
        ///// <param name="alternateModelRid">Alternate Size Model RID</param>
        ///// <param name="constraintModelRid">Constraint model RID</param>
        ///// <param name="ruleCollection">Rule Colection</param>
        ///// <param name="sizeColorPlan">Size Color Plan Type: One color or All Colors</param>
        ///// <param name="processControl">Process Type: Size Curves only, Size Curves and Plan or All</param>
        ///// <param name="aNormalizeSizeCurves">True:  Size curves will be normalized (ie. percentages in sizes missing from header will be pro-rated back to sizes on header); False: Size Curves will not be normalized</param>
        ///// <returns></returns>
        //public SizeNeedResults ProcessFillSize(
        //    eFillSizesToType aFillSizesToType,
        //    int availableUnits, 
        //    AllocationProfile allocProfile, 
        //    AllocationColorOrSizeComponent colorComponent, 
        //    SizeCurveGroupProfile sizeCurveGroup, 
        //    ProfileList aStoreList,
        //    eMerchandiseType aMerchandiseType,
        //    int aMdseHnRID,
        //    int aMdsePhRID,
        //    int aMdsePhlSequence,
        //    int storeGroupRid,
        //    int alternateModelRid, int constraintModelRid, 
        //    CollectionRuleSets ruleCollection, eSizeNeedColorPlan sizeColorPlan,
        //    eSizeProcessControl processControl,
        //    bool aNormalizeSizeCurves)           // MID 4861 Size Normalization
        ////public SizeNeedResults ProcessFillSizeHoles(
        ////	int availableUnits, 
        ////	AllocationProfile allocProfile, 
        ////	AllocationColorOrSizeComponent colorComponent, 
        ////	SizeCurveGroupProfile sizeCurveGroup, 
        ////	ProfileList aStoreList,
        ////	eMerchandiseType aMerchandiseType,
        ////	int aMdseHnRID,
        ////	int aMdsePhRID,
        ////	int aMdsePhlSequence,
        ////	int storeGroupRid,
        ////	int alternateModelRid, int constraintModelRid, 
        ////	CollectionRuleSets ruleCollection, eSizeNeedColorPlan sizeColorPlan,
        ////	eSizeProcessControl processControl, // MID 4861 Size Normalization
        ////	bool aNormalizeSizeCurves)           // MID 4861 Size Normalization
        // end TT#702 Infinite Loop when begin date set
        {
            // end MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            try
            {
                //bool process = true;  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement (obsolete code)
                // begin TT#702 Infinite Loop when begin date set
                //_allocProfile = allocProfile;
                //_targetComponent = colorComponent;
                //_sizeCurveGroup = sizeCurveGroup;
                //_headerColorRid = colorComponent.ColorRID;  // MID Track 3749
                //// Begin MID Issue # 3160 
                //_sizeNeedResults.HeaderColorRid = _headerColorRid; // MID Track 3749
                //_sizeNeedResults.Component = colorComponent;
                //_sizeNeedResults.Stores = aStoreList; // MID Track 3631 Size Rules apply to all size on header
                //// End MID Issue # 3160 

                //_ruleCollection = ruleCollection;

                //_constraintModelRid = constraintModelRid;
                //_alternateModelRid = alternateModelRid;

                //                _sizeNeedResults.Clear(true);
                //                _sizeNeedResults.Sizes.Clear();
                //                _sizeNeedResults.MethodType = eSizeMethodType.FillSizeHolesAllocation;
                //                _sizeNeedResults.NormalizeSizeCurves = aNormalizeSizeCurves; // MID Track 4861 Size Normalization

                //                // begin MID Track 3749 Use Total Color-size ownership at Style and above; specific color-size ownership at color and lower
                //                //if (_allocProfile.BulkColors.ContainsKey(_colorRid))
                //                //{
                //                    _color = (HdrColorBin)_allocProfile.BulkColors[_headerColorRid];
                //                    foreach (int sizeRID in _color.ColorSizes.Keys)
                //                    {
                //                        _sizeNeedResults.AddSize(sizeRID);
                //                    }
                //                    // begin MID Tracks 3738, 3811, 3827 Status issues
                //                    if (_sizeNeedResults.Sizes.Count < 1)
                //                    {
                //                        string msg = string.Format(MIDText.GetText(eMIDTextCode.msg_al_ThereAreNoSizesInSelectedColor), this._sab.HierarchyServerSession.GetColorCodeProfile(_color.ColorCodeRID).ColorCodeName) + " Header [" + _allocProfile.HeaderID + "] "; // MID Track 5778 Schedule 'Run Now' gets error in audit  // TT#702 Infinite Loop when begin date set
                //                        this._sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.GetType().Name);
                //                        throw new MIDException(eErrorLevel.warning,(int)(eMIDTextCode.msg_al_ThereAreNoSizesInSelectedColor), msg);
                //                    }
                //                    // end MID Tracks 3738, 3811, 3827 Status issues
                //                //}
                //                //else
                //                //{
                //                //	throw new Exception("color " + _colorRid + " does not exist on header as a bulk color");
                //                //}
                //                // end MID Track 3749  NOTE: error message will occur if color is not bulk color on header

                //                // Begin MID Issue # 3160 
                //                _sizeNeedResults.Color = _color;
                //                // End MID Issue # 3160 
                //                BuildSizeModels(_constraintModelRid, _alternateModelRid); // MID Track 3619 Remove Fringe
                //                BuildRulesDecoder(storeGroupRid);

                //                _mdseSizeBasis = this.GetMdseBasis(  // MID Track 3749
                //                    _allocProfile,
                //                    aMerchandiseType,
                //                    aMdseHnRID,
                //                    aMdsePhRID,
                //                    aMdsePhlSequence
                //                    );
                ////				_sizeNeedResults.Clear();

                //                //process = ParmValidation();  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement

                //                //if (process)   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement(remove obsolete code)
                //                //{              // MID Track 4921 AnF#666 FIll to Size Plan Enhancement(remove obsolete code)

                //                // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                //                //ArrayList fillStoresList = new ArrayList();
                //                //ArrayList fillSizesList = new ArrayList();
                //                Hashtable fillSizeStoresHash = new Hashtable();
                //                ArrayList fillStoresList;
                //                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                //                CalculateSizeCurves(sizeCurveGroup);
                Hashtable fillSizeStoresHash = new Hashtable();
                ArrayList fillStoresList;
                // end TT#702 Infinite Loop when begin date set

                // begin TT#2143 - JEllis - Size Review Infinite Loop
                if (this._sizeNeedResults.GetOnHandAndIT)
                {
                    BuildOnHandAndIntransit(); 
                }
                // end TT#2143 - JEllis - Size Review Infinite Loop

                // begin TT#1600 - Size Need Algorithm Error
                if (_sizeNeedResults.AccumulatePriorAllocated)
                {
                    BuildPriorAllocated();
                }
                // end TT#1600 - Size Need Algorithm Error

                if (processControl != eSizeProcessControl.SizeCurvesOnly)
                {
                    // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //CalculatePlan(sizeCurveGroup, sizeColorPlan, eSizeMethodType.FillSizeHolesAllocation, false); // MID Track 4291 add fill variables to size review

                    

                    if (aFillSizesToType == eFillSizesToType.Holes)
                    {
                        CalculatePlan(_sizeCurveGroup, _sizeColorPlanType, eSizeMethodType.FillSizeHolesAllocation, false, false);  // TT#702 Infinite Loop when begin date set //TT#848-MD -jsobek -Fill to Size Plan Presentation
                    }
                    else
                    {
                        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
                        //bool useSizeMin = false;
                        //if (aFillSizesToType == eFillSizesToType.SizePlanWithMins)
                        //{
                        //    useSizeMin = true;
                        //}
                        CalculatePlan(_sizeCurveGroup, _sizeColorPlanType, eSizeMethodType.FillSizeHolesAllocation, false, true); // TT#702 Infinite Loop when begin date set
                        //End TT#848-MD -jsobek -Fill to Size Plan Presentation
                    }
                    // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                }

                if (processControl == eSizeProcessControl.ProcessAll)
                {
                    // begin MID Track 3778 Size Rules must be applied in Fill Size Holes
                    // Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    // Added this back for fils size packs.
                    if (_allocProfile.BulkColorIsOnHeader(this._headerColorRid))   // MID Track 3749 note we already know color is on header      
                    // End TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    {
                        if (_sizeRuleList != null)
                        {
                            if (_sizeRuleList.Count > 0)
                            {
                                // ????????????????
                                // This eventually gets to SizeFringeAlgorithm
                                ProcessSizeRule();
                            }
                        }
                    }		// TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    // end MID Track 3778 Size Rules must be applied in Fill Size Holes
                    // begin TT#2155 - JEllis - Fill Size HOles Null Reference
                    if (!aApplyRulesOnly)
                    {
                        // end TT#2155 - JEllis - Fill Size Holes Null Reference
                        int storeCnt = _curveStoreList.Count; // MID Track 3631 size rules apply to all sizes on header
                        for (int i = 0; i < storeCnt; i++)
                        {
                            StoreProfile sp = (StoreProfile)_curveStoreList[i]; // MID Track 3631 size rules apply to all sizes on header
                            SizeCurveProfile scp = _sizeCurveGroup.GetStoreSizeCurveProfile(sp.Key);  // TT#702 Infinite Loop when begin date set
                            Index_RID storeIndex = (Index_RID)_allocProfile.StoreIndex(sp.Key);
                            // If store is not OUT and is eligible
                            if ((_allocProfile.GetStoreIsEligible(sp.Key)) // MID Change j.ellis Only eligible stores
                            && _allocProfile.GetIncludeStoreInAllocation(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
                               && _sizeNeedResults.Stores.Contains(sp.Key)       // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                               && !_allocProfile.GetStoreOut(this._targetComponent, sp.Key))   // TT#702 Infinite Loop when begin date set
                            //|| (_allocProfile.GetStoreIsEligible(sp.Key))) // MID Change j.ellis Only eligible stores
                            {
                                foreach (SizeCodeProfile sizeCode in scp.SizeCodeList.ArrayList)
                                {
									//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    // Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                    if ((_color != null && _color.ColorSizes.ContainsKey(sizeCode.Key))
                                        || this._sizeCodeList.Contains(sizeCode.Key)
                                    // End TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                        || _allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder) // MID Track 3786 Change Fill Size Algorithm
									//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    {
                                        // begin MID Track 3781 Size Curve not required
                                        if (!IsStoreExcluded(sp.Key, sizeCode))
                                        {
                                            // end MID Track 3781 Size Curve not required
                                            // begin MID Track 3786 Change Fill Size Holes Algorithm
                                            int units = _sizeNeedResults.GetSizeNeedUnits(sp.Key, sizeCode.Key);
                                            //Debug.WriteLineIf(sp.Key > 22 && sp.Key < 26, "ProcessFillSize() ST: " + sp.Key.ToString() + " SZ: " + sizeCode.Key.ToString() + " NEED: " + units.ToString());
                                            if (units > 0)
                                            {
                                                // begin MID Track 4921 Fill to Size Plan Enhancement
                                                //fillStoresList.Add(sp.Key);
                                                // //fillSizesList.Add(sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly
                                                //fillSizesList.Add(sizeCode); // MID Track 3492 Size Need with constraints not allocating correctly
                                                fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                                if (fillStoresList == null)
                                                {
                                                    fillStoresList = new ArrayList();
                                                    fillSizeStoresHash.Add(sizeCode, fillStoresList);
                                                }
                                                fillStoresList.Add(sp.Key);
                                            }
                                        }  // MID Track 3781 Size Curve not required
                                    }
                                }
                            }
                        }
                        //if (fillStoresList.Count > 0)  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        if (fillSizeStoresHash.Count > 0)  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        {
                            // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                            if (aFillSizesToType == eFillSizesToType.Holes)
                            {
                                CalculateFillSizeHolesAllocation(availableUnits, fillSizeStoresHash, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                //CalculateFillSizeHolesAllocation(availableUnits, fillStoresList, fillSizesList, sizeColorPlan);  // MID Track 4921 AnF#666 Fill Size To Plan Enhancement
                            }
                            else
                            {
								// Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                //if (_color != null)
                                if (_targetComponent.ComponentType != eComponentType.DetailType)
                                {
                                    HdrSizeBin[] hdrSizeBinList = null;
                                    hdrSizeBinList = new HdrSizeBin[_color.ColorSizes.Count];
                                    _color.ColorSizes.Values.CopyTo(hdrSizeBinList, 0);

                                    MIDGenericSortItem[] sortedSizeBinList = this.SortSizes(hdrSizeBinList);
                                    int sizeTotal = 0;
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        sizeTotal += -(int)genericItem.SortKey[0];  // Key is negative
                                    }
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        HdrSizeBin sizeBin = hdrSizeBinList[genericItem.Item];
                                        int sizeUnits = -(int)genericItem.SortKey[0]; // Key is negative
                                        int sizeUnitsAvailable = 0;
                                        if (sizeTotal > 0)
                                        {
                                            sizeUnitsAvailable =
                                                (int)(((double)sizeUnits * (double)availableUnits
                                                / (double)sizeTotal)
                                                + .5d);
                                            sizeTotal -= sizeUnits;
                                            availableUnits -= sizeUnitsAvailable;
                                        }
                                        if (sizeUnitsAvailable > 0)
                                        {
                                            SizeCodeProfile sizeCode = _transaction.GetSizeCodeProfile(sizeBin.SizeCodeRID); // Assortment: color/size changes
                                            fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                            if (fillStoresList != null)
                                            {
                                                CalculateFillSizeToPlanAllocation(sizeUnitsAvailable, fillStoresList, sizeCode, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PackSizeBin[] hdrSizeBinList = null;
                                    hdrSizeBinList = new PackSizeBin[_packSizes.Count];
                                    _packSizes.Values.CopyTo(hdrSizeBinList, 0);

                                    MIDGenericSortItem[] sortedSizeBinList = this.SortPackSizes(hdrSizeBinList);
                                    int sizeTotal = 0;
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        sizeTotal += -(int)genericItem.SortKey[0];  // Key is negative
                                    }
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        PackSizeBin sizeBin = hdrSizeBinList[genericItem.Item];
                                        int sizeUnits = -(int)genericItem.SortKey[0]; // Key is negative
                                        int sizeUnitsAvailable = 0;
                                        if (sizeTotal > 0)
                                        {
                                            sizeUnitsAvailable =
                                                (int)(((double)sizeUnits * (double)availableUnits
                                                / (double)sizeTotal)
                                                + .5d);
                                            sizeTotal -= sizeUnits;
                                            availableUnits -= sizeUnitsAvailable;
                                        }
                                        if (sizeUnitsAvailable > 0)
                                        {
                                            SizeCodeProfile sizeCode = _transaction.GetSizeCodeProfile(sizeBin.ContentCodeRID);
                                            fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                            if (fillStoresList != null)
                                            {
                                                CalculateFillSizeToPlanAllocation(sizeUnitsAvailable, fillStoresList, sizeCode, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                                Debug.WriteLine("ProcessFillSize()  sizeTotal: " + sizeTotal + "  availableUnits: " + availableUnits + "  sizeUnitsAvailable: " + sizeUnitsAvailable);
                                            }
                                        }
                                    }
                                }
                            }
                            // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        }
                        else
                        {
                            this._transaction.SAB.ApplicationServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Warning,
                                eMIDTextCode.msg_al_NoPositivePercentNeed,
                                MIDText.GetText(eMIDTextCode.msg_al_NoPositivePercentNeed) + " Header [" + _allocProfile.HeaderID + "] ", // MID Track 5778 Schedule 'Run Now' gets error in audit // TT#702 Infinite Loop when begin date set
                                this.GetType().Name);
                        }
                        // begin TT#2155 - JEllis - Fill Size HOles Null Reference
                    }
                    else
                    {
                        _transaction.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_FillSizeRulesSuccessfullyApplied,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_al_FillSizeRulesSuccessfullyApplied), _allocProfile.HeaderID),
                            GetType().Name);
                    }
                }

                return _sizeNeedResults;
                // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement (remove obsolete code)
                //}
                //else
                //{
                //	throw new MIDException (eErrorLevel.severe,
                //		(int)eMIDTextCode.msg_al_SizeNeedAlgorithmFailed,
                //		MIDText.GetText(eMIDTextCode.msg_al_SizeNeedAlgorithmFailed));
                //}
                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (remove obsolete code)

            }
            catch (NoProcessingByFillSizeHoles)
            {
                throw;
            }
            catch (Exception ex)
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_SizeNeedAlgorithmFailed, "Header [" + _allocProfile.HeaderID + "] " + ex.Message, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' gets error in audit  // TT#702 Infinite Loop when begin date set
                throw;
            }
        }

		// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        public SizeNeedResults ProcessFillSizeBulkAfterDetail(
            eFillSizesToType aFillSizesToType,
            int availableUnits,
            eSizeProcessControl processControl,
            bool aApplyRulesOnly)
        {
            try
            {
                Hashtable fillSizeStoresHash = new Hashtable();
                ArrayList fillStoresList;
                _sizeNeedResults.ClearStoreAllocated();
                _sizeNeedResults.ClearStoreIntransit();
                _sizeNeedResults.ClearStoreOnHand();
                _sizeNeedResults.ClearStoreRule();   // TT#5063 - JSmith - Fill Size with constraints and rule fails on header with packs and loose
                //if (this._sizeNeedResults.GetOnHandAndIT)
                //{
                    BuildOnHandAndIntransit();
                //}

                if (_sizeNeedResults.AccumulatePriorAllocated)
                {
                    BuildPriorAllocated();
                }

                if (_targetComponent.ComponentType != eComponentType.DetailType)
                {
                    BuildPriorDetailAllocated();
                }


                //if (processControl != eSizeProcessControl.SizeCurvesOnly)
                //{
                //    if (aFillSizesToType == eFillSizesToType.Holes)
                //    {
                //        CalculatePlan(_sizeCurveGroup, _sizeColorPlanType, eSizeMethodType.FillSizeHolesAllocation, false, false);  // TT#702 Infinite Loop when begin date set //TT#848-MD -jsobek -Fill to Size Plan Presentation
                //    }
                //    else
                //    {
                //        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
                //        //bool useSizeMin = false;
                //        //if (aFillSizesToType == eFillSizesToType.SizePlanWithMins)
                //        //{
                //        //    useSizeMin = true;
                //        //}
                //        CalculatePlan(_sizeCurveGroup, _sizeColorPlanType, eSizeMethodType.FillSizeHolesAllocation, false, true); // TT#702 Infinite Loop when begin date set
                //        //End TT#848-MD -jsobek -Fill to Size Plan Presentation
                //    }
                //    // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                //}

                if (processControl == eSizeProcessControl.ProcessAll)
                {
                    // begin MID Track 3778 Size Rules must be applied in Fill Size Holes
                    // Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    // Added this back for fils size packs.
                    if (_allocProfile.BulkColorIsOnHeader(this._headerColorRid))   // MID Track 3749 note we already know color is on header      
                    // End TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    {
                        if (_sizeRuleList != null)
                        {
                            if (_sizeRuleList.Count > 0)
                            {
                                // ????????????????
                                // This eventually gets to SizeFringeAlgorithm
                                ProcessSizeRule();
                            }
                        }
                    }		// TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    // end MID Track 3778 Size Rules must be applied in Fill Size Holes
                    // begin TT#2155 - JEllis - Fill Size HOles Null Reference
                    if (!aApplyRulesOnly)
                    {
                        // end TT#2155 - JEllis - Fill Size Holes Null Reference
                        int storeCnt = _curveStoreList.Count; // MID Track 3631 size rules apply to all sizes on header
                        for (int i = 0; i < storeCnt; i++)
                        {
                            StoreProfile sp = (StoreProfile)_curveStoreList[i]; // MID Track 3631 size rules apply to all sizes on header
                            SizeCurveProfile scp = _sizeCurveGroup.GetStoreSizeCurveProfile(sp.Key);  // TT#702 Infinite Loop when begin date set
                            Index_RID storeIndex = (Index_RID)_allocProfile.StoreIndex(sp.Key);
                            // If store is not OUT and is eligible
                            if ((_allocProfile.GetStoreIsEligible(sp.Key)) // MID Change j.ellis Only eligible stores
                            && _allocProfile.GetIncludeStoreInAllocation(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
                               && _sizeNeedResults.Stores.Contains(sp.Key)       // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                               && !_allocProfile.GetStoreOut(this._targetComponent, sp.Key))   // TT#702 Infinite Loop when begin date set
                            //|| (_allocProfile.GetStoreIsEligible(sp.Key))) // MID Change j.ellis Only eligible stores
                            {
                                foreach (SizeCodeProfile sizeCode in scp.SizeCodeList.ArrayList)
                                {
                                    //BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    // Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                    if ((_color != null && _color.ColorSizes.ContainsKey(sizeCode.Key))
                                        || this._sizeCodeList.Contains(sizeCode.Key)
                                        // End TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                        || _allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder) // MID Track 3786 Change Fill Size Algorithm
                                    //END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    {
                                        // begin MID Track 3781 Size Curve not required
                                        if (!IsStoreExcluded(sp.Key, sizeCode))
                                        {
                                            // end MID Track 3781 Size Curve not required
                                            // begin MID Track 3786 Change Fill Size Holes Algorithm
                                            int units = _sizeNeedResults.GetSizeNeedUnits(sp.Key, sizeCode.Key);
                                            //Debug.WriteLineIf(sp.Key > 22 && sp.Key < 26, "ProcessFillSize() ST: " + sp.Key.ToString() + " SZ: " + sizeCode.Key.ToString() + " NEED: " + units.ToString());
                                            if (units > 0)
                                            {
                                                // begin MID Track 4921 Fill to Size Plan Enhancement
                                                //fillStoresList.Add(sp.Key);
                                                // //fillSizesList.Add(sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly
                                                //fillSizesList.Add(sizeCode); // MID Track 3492 Size Need with constraints not allocating correctly
                                                fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                                if (fillStoresList == null)
                                                {
                                                    fillStoresList = new ArrayList();
                                                    fillSizeStoresHash.Add(sizeCode, fillStoresList);
                                                }
                                                fillStoresList.Add(sp.Key);
                                            }
                                        }  // MID Track 3781 Size Curve not required
                                    }
                                }
                            }
                        }
                        //if (fillStoresList.Count > 0)  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        if (fillSizeStoresHash.Count > 0)  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        {
                            // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                            if (aFillSizesToType == eFillSizesToType.Holes)
                            {
                                CalculateFillSizeHolesAllocation(availableUnits, fillSizeStoresHash, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                //CalculateFillSizeHolesAllocation(availableUnits, fillStoresList, fillSizesList, sizeColorPlan);  // MID Track 4921 AnF#666 Fill Size To Plan Enhancement
                            }
                            else
                            {
                                // Begin TT#1637-MD - stodd - received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                                //if (_color != null)
                                if (_targetComponent.ComponentType != eComponentType.DetailType)
                                {
                                    HdrSizeBin[] hdrSizeBinList = null;
                                    hdrSizeBinList = new HdrSizeBin[_color.ColorSizes.Count];
                                    _color.ColorSizes.Values.CopyTo(hdrSizeBinList, 0);

                                    MIDGenericSortItem[] sortedSizeBinList = this.SortSizes(hdrSizeBinList);
                                    int sizeTotal = 0;
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        sizeTotal += -(int)genericItem.SortKey[0];  // Key is negative
                                    }
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        HdrSizeBin sizeBin = hdrSizeBinList[genericItem.Item];
                                        int sizeUnits = -(int)genericItem.SortKey[0]; // Key is negative
                                        int sizeUnitsAvailable = 0;
                                        if (sizeTotal > 0)
                                        {
                                            sizeUnitsAvailable =
                                                (int)(((double)sizeUnits * (double)availableUnits
                                                / (double)sizeTotal)
                                                + .5d);
                                            sizeTotal -= sizeUnits;
                                            availableUnits -= sizeUnitsAvailable;
                                        }
                                        if (sizeUnitsAvailable > 0)
                                        {
                                            SizeCodeProfile sizeCode = _transaction.GetSizeCodeProfile(sizeBin.SizeCodeRID); // Assortment: color/size changes
                                            fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                            if (fillStoresList != null)
                                            {
                                                Debug.WriteLine("ProcessFillSizeBulkAfterDetail() SZ: " + sizeBin.SizeCodeRID);
                                                CalculateFillSizeToPlanAllocation(sizeUnitsAvailable, fillStoresList, sizeCode, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PackSizeBin[] hdrSizeBinList = null;
                                    hdrSizeBinList = new PackSizeBin[_packSizes.Count];
                                    _packSizes.Values.CopyTo(hdrSizeBinList, 0);

                                    MIDGenericSortItem[] sortedSizeBinList = this.SortPackSizes(hdrSizeBinList);
                                    int sizeTotal = 0;
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        sizeTotal += -(int)genericItem.SortKey[0];  // Key is negative
                                    }
                                    foreach (MIDGenericSortItem genericItem in sortedSizeBinList)
                                    {
                                        PackSizeBin sizeBin = hdrSizeBinList[genericItem.Item];
                                        int sizeUnits = -(int)genericItem.SortKey[0]; // Key is negative
                                        int sizeUnitsAvailable = 0;
                                        if (sizeTotal > 0)
                                        {
                                            sizeUnitsAvailable =
                                                (int)(((double)sizeUnits * (double)availableUnits
                                                / (double)sizeTotal)
                                                + .5d);
                                            sizeTotal -= sizeUnits;
                                            availableUnits -= sizeUnitsAvailable;
                                        }
                                        if (sizeUnitsAvailable > 0)
                                        {
                                            SizeCodeProfile sizeCode = _transaction.GetSizeCodeProfile(sizeBin.ContentCodeRID);
                                            fillStoresList = (ArrayList)fillSizeStoresHash[sizeCode];
                                            if (fillStoresList != null)
                                            {
                                                CalculateFillSizeToPlanAllocation(sizeUnitsAvailable, fillStoresList, sizeCode, _sizeColorPlanType);  // TT#702 Infinite Loop when begin date set
                                                Debug.WriteLine("ProcessFillSize()  sizeTotal: " + sizeTotal + "  availableUnits: " + availableUnits + "  sizeUnitsAvailable: " + sizeUnitsAvailable);
                                            }
                                        }
                                    }
                                }
                            }
                            // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        }
                        else
                        {
                            this._transaction.SAB.ApplicationServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Warning,
                                eMIDTextCode.msg_al_NoPositivePercentNeed,
                                MIDText.GetText(eMIDTextCode.msg_al_NoPositivePercentNeed) + " Header [" + _allocProfile.HeaderID + "] ", // MID Track 5778 Schedule 'Run Now' gets error in audit // TT#702 Infinite Loop when begin date set
                                this.GetType().Name);
                        }
                        // begin TT#2155 - JEllis - Fill Size HOles Null Reference
                    }
                    else
                    {
                        _transaction.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Information,
                            eMIDTextCode.msg_al_FillSizeRulesSuccessfullyApplied,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_al_FillSizeRulesSuccessfullyApplied), _allocProfile.HeaderID),
                            GetType().Name);
                    }
                }

                return _sizeNeedResults;

            }
            catch (NoProcessingByFillSizeHoles)
            {
                throw;
            }
            catch (Exception ex)
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_SizeNeedAlgorithmFailed, "Header [" + _allocProfile.HeaderID + "] " + ex.Message, this.GetType().Name); // MID Track 5778 Schedule 'Run Now' gets error in audit  // TT#702 Infinite Loop when begin date set
                throw;
            }
            finally
            {
            }
        }
		// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

        private void ProcessEquates()
        {
        }

        private void BuildSizeModels(int constraintRid, int alternateRid) // MID Track 3619 Remove Fringe
        {
            try
            {
                //======================================================
                // Build Constraint Model profile (w/ collection sets)
                //======================================================
                if (constraintRid != Include.NoRID)
                {
                    _constraintModel = new SizeConstraintModelProfile(constraintRid);
                    if (_constraintStoreGroupLevelHash == null)
                        _constraintStoreGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(_constraintModel.StoreGroupRid); //_sab.StoreServerSession.GetStoreGroupLevelHashTable(_constraintModel.StoreGroupRid);
					// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
					//_constraintDecoder = new CollectionDecoder(this._transaction, _constraintModel.CollectionSets, _constraintStoreGroupLevelHash);   // TT#1432 - Size Dimension Constraints not working
                    _constraintDecoder = new CollectionDecoder(this._transaction, _constraintModel.CollectionSets(StoreMgmt.StoreGroup_GetVersion(_constraintModel.StoreGroupRid)), _constraintStoreGroupLevelHash);   // TT#1432 - Size Dimension Constraints not working
					// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
                    //_constraintDecoder = new CollectionDecoder(_constraintModel.CollectionSets, _constraintStoreGroupLevelHash);  // TT#1432 - Size Dimension Constraints not working

                }
                else
                {
                    _constraintModel = null;
                    _constraintDecoder = null;
                    _constraintStoreGroupLevelHash = null;
                }


                //==========================================
                // Build Size Alternates Model Hash table
                //==========================================
                _sizeAlternateHash = new Hashtable();
                if (alternateRid != Include.NoRID)
                {
                    BuildAlternateModel(alternateRid);
                }
                else
                {
                    _alternateModel = null;
                }

                // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement -- remove Debug code
                //IDictionaryEnumerator altEnumerator = _sizeAlternateHash.GetEnumerator();
                //while ( altEnumerator.MoveNext() )
                //{
                //	Debug.WriteLine(" "); 
                //	ArrayList anArray = (ArrayList)altEnumerator.Value;
                //	foreach (SizeCodeProfile scp in anArray)
                //	{
                //		Debug.WriteLine("  PRIM " + altEnumerator.Key.ToString() + "  ALT " + scp.Key.ToString());
                //	}
                //}
                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement -- remove Debug code

            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        private void BuildAlternateModel(int alternateRid)
        {
            try
            {
                _alternateModel = new SizeAltModelProfile(alternateRid);
                SizeCurveGroupProfile primSizeCurveGroupProfile = new SizeCurveGroupProfile(_alternateModel.PrimarySizeCurveRid);
                SizeCurveGroupProfile altSizeCurveGroupProfile = new SizeCurveGroupProfile(_alternateModel.AlternateSizeCurveRid);

                foreach (SizeAlternatePrimary sizePrim in _alternateModel.AlternateSizeList)
                {
                    //Build Primary size code list and alternate size code list
                    ArrayList primList = primSizeCurveGroupProfile.GetSizeCodeList(sizePrim.SizeRid, sizePrim.DimensionRid);
                    ArrayList altList = new ArrayList();
                    foreach (SizeAlternate sizeAlt in sizePrim.AlternateList)
                    {
                        ArrayList aList = altSizeCurveGroupProfile.GetSizeCodeList(sizeAlt.SizeRid, sizeAlt.DimensionRid);
                        foreach (SizeCodeProfile scp in aList)
                        {
                            altList.Add(scp);
                        }
                    }
                    // now make a hash table of each primary size code and map it to it's alternate size code list
                    foreach (SizeCodeProfile scp in primList)
                    {
                        // It's possible for a user to designate both a specific primary (size=192, dim=187) and
                        // a general one for the size or dim such as size=192, but dim = -1.  In this case,
                        // the more specific one should replace the general one.
                        if (_sizeAlternateHash.ContainsKey(scp.Key))
                        {
                            if (sizePrim.SizeRid == Include.NoRID || sizePrim.DimensionRid == Include.NoRID)
                            {
                                // We skip it.  The more specific one is already in the hash
                            }
                            else
                            {
                                // otherwise THIS is the more specific entry and we replace what was
                                // previsouly placed in the hash
                                _sizeAlternateHash.Remove(scp.Key);
                                ArrayList finalAltList = new ArrayList();
                                foreach (SizeCodeProfile altScp in altList)
                                {
                                    if (sizePrim.SizeRid == Include.NoRID)
                                    {
                                        if (scp.SizeCodePrimaryRID == altScp.SizeCodePrimaryRID)
                                        {
                                            finalAltList.Add(altScp);
                                        }
                                    }
                                    else if (sizePrim.DimensionRid == Include.NoRID)
                                    {
                                        if (scp.SizeCodeSecondaryRID == altScp.SizeCodeSecondaryRID)
                                        {
                                            finalAltList.Add(altScp);
                                        }
                                    }
                                    else
                                    {
                                        finalAltList.Add(altScp);
                                    }
                                }
                                _sizeAlternateHash.Add(scp.Key, finalAltList);
                            }
                        }
                        else
                        {
                            ArrayList finalAltList = new ArrayList();
                            foreach (SizeCodeProfile altScp in altList)
                            {
                                if (sizePrim.SizeRid == Include.NoRID)
                                {
                                    if (scp.SizeCodePrimaryRID == altScp.SizeCodePrimaryRID)
                                    {
                                        finalAltList.Add(altScp);
                                    }
                                }
                                else if (sizePrim.DimensionRid == Include.NoRID)
                                {
                                    // Begin TT#1839-MD - JSmith - Size Alternates not observed when using different size codes
                                    //if (scp.SizeCodeSecondaryRID == altScp.SizeCodeSecondaryRID)
                                    if (scp.SizeCodeSecondaryRID == altScp.SizeCodeSecondaryRID
                                        || (scp.SizeCodeSecondaryRID == Include.NoRID
                                            && altScp.SizeCodeSecondary == Include.NoSecondarySize))
                                    // End TT#1839-MD - JSmith - Size Alternates not observed when using different size codes
                                    {
                                        finalAltList.Add(altScp);
                                    }
                                }
                                else
                                {
                                    finalAltList.Add(altScp);
                                }
                            }
                            _sizeAlternateHash.Add(scp.Key, finalAltList);
                        }
                    }
                }

                // begin MID Track 3523 Alter size curve percents so size substitution is controlled by user -- j.ellis
                // 07/11/05 We no longer alter the size curve percents - stodd
                //				//============================================================
                //				// preliminary Size Alternate Processing
                //				// We only change the size curve percents when the alternate
                //				// primary and alternate size groups are the same
                //				//============================================================
                //				_addSizeCurvePct = false;
                //				if (_methodType == eMethodType.SizeNeedAllocation ||
                //					_methodType == eMethodType.FillSizeHolesAllocation)
                //				{
                //					if(_alternateModel.PrimarySizeGroupRid == _alternateModel.AlternateSizeGroupRid)
                //					{
                //						_addSizeCurvePct = true;
                //					}
                //				}
                //_addSizeCurvePct = false;
                //if (_methodType == eMethodType.SizeNeedAllocation 
                //    || _methodType == eMethodType.AllocationOverride)
                //{
                //    if (_alternateModel.PrimarySizeCurveRid == _alternateModel.AlternateSizeCurveRid)
                //    {
                //        _addSizeCurvePct = true;
                //    }
                //}
                // end MID Track 3523 Alter size curve percents so size substitution is controlled by user -- j.ellis
            }
            catch
            {
                throw;
            }
        }



        // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement -- Remove Debug Code 
        //private void DebugSizeGroup(SizeGroupProfile sizeGroup)
        //{
        //	foreach(SizeCodeProfile aSizeCode in sizeGroup.SizeCodeList)
        //	{
        //		string aLine = aSizeCode.Key.ToString() + " " +
        //			aSizeCode.SizeCodePrimaryRID.ToString() + " " +
        //			aSizeCode.SizeCodePrimary + " / " +
        //			aSizeCode.SizeCodeSecondaryRID.ToString() + " " +
        //			aSizeCode.SizeCodeSecondary;
        //		Debug.WriteLine(aLine);
        //
        //	}
        //}
        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement -- Remove Debug Code

        private void BuildRulesDecoder(int storeGroupRid)
        {
            try
            {
                if (storeGroupRid != Include.NoRID)
                {
                    if (_rulesStoreGroupLevelHash == null)
                        _rulesStoreGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(storeGroupRid); //_sab.StoreServerSession.GetStoreGroupLevelHashTable(storeGroupRid);
                    _rulesDecoder = new CollectionDecoder(this._transaction, _ruleCollection, _rulesStoreGroupLevelHash); // TT#1432 - Size Dimension Constraints not working
                    //_rulesDecoder = new CollectionDecoder(_ruleCollection, _rulesStoreGroupLevelHash); // TT#1432 - Size Dimension Constraints not working
                }
                else
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_al_SizeNeedAlgorithmInvalidStoreAttr,
                        MIDText.GetText(eMIDTextCode.msg_al_SizeNeedAlgorithmInvalidStoreAttr));
                }
            }
            catch (Exception ex)
            {
                _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_SizeNeedAlgorithmInvalidStoreAttr, ex.Message, this.ToString());
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// checks the rule collection to see if the store is excluded for this size and color
        /// </summary>
        /// <param name="storeRid">RID that identifies the store</param>
        /// <param name="aSizeCodeProfile">SizeCodeProfile that descibes the size</param>
        /// <returns></returns>
        //public bool IsStoreExcluded(int storeRid, int colorRid, int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        private bool IsStoreExcluded(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly
        {
            bool exclude = false;
            // begin MID Track 3781 size curve not required
            //			int sglRid = (int)_rulesStoreGroupLevelHash[storeRid];
            //
            //			if (_rulesDecoder != null)
            //			{
            //				//RuleItemBase aRule = (RuleItemBase)_rulesDecoder.GetItem(sglRid, colorRid , sizeRid); // MID Track 3492 Size Need with constraints not allocating correctly
            //                RuleItemBase aRule = (RuleItemBase)_rulesDecoder.GetItem(sglRid, colorRid, aSizeCodeProfile); // MID Track 3492 Size Need with constraints not allocating correctly
            //				if (aRule.Rule == (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
            //				{
            //					exclude = true;
            //				}
            //			}
            eSizeRuleType sizeRuleType = _sizeNeedResults.GetStoreSizeRule(storeRid, aSizeCodeProfile.Key);
            if (sizeRuleType == eSizeRuleType.Exclude
                || sizeRuleType == eSizeRuleType.AbsoluteQuantity
                || sizeRuleType == eSizeRuleType.SizeMaximum)
            {
                exclude = true;
            }
            // end MID Track 3781 size curve not required

            return exclude;
        }

        private HierarchyNodeProfile GetMdseBasis
            (
            AllocationProfile aAllocationProfile,
            eMerchandiseType aMerchandiseType,
            int aMdseHnRID,
            int aMdsePhRID,
            int aMdsePhlSequence
            )
        {
            //int mdseRID = Include.NoRID;
            HierarchyNodeProfile aNode = null;
            switch (aMerchandiseType)
            {
                case (eMerchandiseType.HierarchyLevel):
                    {
                        if (aMdsePhRID != Include.NoRID)
                        {
                            HierarchyNodeProfile hnp = this._transaction.GetNodeData(aAllocationProfile.StyleHnRID);
                            //Begin Track #4037 - JSmith - Optionally include dummy color in child list
                            HierarchyNodeList styleColorProfiles = this._transaction.GetDescendantData(hnp.Key, eHierarchyLevelType.Color, eNodeSelectType.All);
                            //						HierarchyNodeList styleColorProfiles = this._transaction.GetDescendantData(hnp.Key, eHierarchyLevelType.Color);
                            //End Track #4037
	                        // begin TT#1410 - Size Need Detail Allocation not giving enough packs
	                        int headerColorRID = _headerColorRid;
	                        if (headerColorRID == Include.DummyColorRID)  
	                            // Detail Packs will use the first bulk color as the basis when there is bulk; otherwise, all color ownership is used for detail packs
	                        {
	                            int colorSequence = int.MaxValue;
	                            if (aAllocationProfile.BulkColorCount > 0)
	                            {
	                                foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
	                                {
	                                    if (hcb.ColorSequence < colorSequence)
	                                    {
	                                        headerColorRID = hcb.ColorCodeRID;
	                                        colorSequence = hcb.ColorSequence;
	                                    }
	                                }
	                            }
	                        }
	                        // end TT#1410 - Size Need Detail Allocation not giving enough packs
							
							//BEGIN TT#670 - MD - DOConnell - Size Need fails when processing sizes on a placeholder color
                            if (styleColorProfiles.Count == 0)
                            {

                                aNode = _sab.HierarchyServerSession.GetNodeData(aAllocationProfile.StyleHnRID);
                                if (aNode.Key == Include.NoRID)
                                {
                                    aNode = _sab.HierarchyServerSession.GetNodeData(aAllocationProfile.AsrtAnchorNodeRid);
                                }
                            }
                            else
                            {
                                foreach (HierarchyNodeProfile colorProfile in styleColorProfiles)
                                {
                                    //if (colorProfile.ColorOrSizeCodeRID == this._headerColorRid) // TT#1410 - Size Need Detail Allocation not giving enough packs
                                    if (colorProfile.ColorOrSizeCodeRID == headerColorRID)         // TT#1410 - Size Need Detail Allocation not giving enough packs
                                    {
                                        aNode = this._transaction.GetAncestorDataByLevel(aMdsePhRID, colorProfile.Key, aMdsePhlSequence);
                                        break;
                                    }
                                }
                            }
							//END TT#670 - MD - DOConnell - Size Need fails when processing sizes on a placeholder color
                            //aNode = this._sab.HierarchyServerSession.GetAncestorDataByLevel(aMdsePhRID, aAllocationProfile.StyleHnRID, aMdsePhlSequence); // MID Change j.ellis Performance--cache ancestor data
                            // aNode = this._transaction.GetAncestorDataByLevel(aMdsePhRID, aAllocationProfile.StyleHnRID, aMdsePhlSequence); // MID Change j.ellis Performance--cache ancestor data
                            if (aNode == null)
                            {
                                throw new MIDException(eErrorLevel.severe,
                                    (int)eMIDTextCode.msg_al_ColorHierarchyNodeNotDefined,
	                                MIDText.GetText(eMIDTextCode.msg_al_ColorHierarchyNodeNotDefined) + ": " + hnp.NodeName + "/" + this._transaction.GetColorCodeProfile(headerColorRID).ColorCodeName); // TT#1410 - Size Need Detail Allocation not giving enough packs
	                                //MIDText.GetText(eMIDTextCode.msg_al_ColorHierarchyNodeNotDefined) + ": " + hnp.NodeName + "/" + this._transaction.GetColorCodeProfile(_headerColorRid).ColorCodeName); // TT#1410 - Size Need Detail Allocation not giving enough packs
                            }
                            //mdseRID = aNode.Key;
                        }
                        break;
                    }
                case (eMerchandiseType.Node):
                    {
                        if (aMdseHnRID != Include.NoRID)
                        {
                            //mdseRID = aMdseHnRID;
                            aNode = _sab.HierarchyServerSession.GetNodeData(aMdsePhRID, aMdseHnRID);
                        }
                        break;
                    }
                case (eMerchandiseType.OTSPlanLevel):
                    {
                        // BEGIN MID Track #3872 - use color or style node for plan level lookup
                        //aNode = this._transaction.GetPlanLevelData(aAllocationProfile.StyleHnRID);
                        aNode = this._transaction.GetPlanLevelData(aAllocationProfile.PlanLevelStartHnRID);

                        if (aNode == null)
                        {
                            throw new MIDException(eErrorLevel.severe,
                                (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                                MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                        }
                        //					else
                        //						mdseRID = aNode.Key;
                        // END MID Track #3872
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (aNode != null)
            {
                switch (aNode.LevelType)
                {
                    case (eHierarchyLevelType.Color):
                        _basisColorRid = aNode.ColorOrSizeCodeRID;  // MID Track 3749  note:  this is the ownership we intend to use
                        break;

                    case (eHierarchyLevelType.Size):
                        HierarchyNodeProfile colorNode = _sab.HierarchyServerSession.GetAncestorData(aMdsePhRID, aNode.Key, -1);
                        _basisColorRid = colorNode.ColorOrSizeCodeRID;   // MID Track 3749  note:  this is the ownership we intend to use
                        break;
                    default:
                        _basisColorRid = Include.DummyColorRID;  // MID Track 3749 note:  we intend to use the total ownership
                        break;
                }
            }



            //return mdseRID; // MID Track 3749
            return aNode; // MID Track 3749
        }

        // begin TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
        private HierarchyNodeProfile GetIbMdseBasis
                (
                AllocationProfile aAllocationProfile,
                eMerchandiseType aMerchandiseType,
                int aMdseHnRID,
                int aMdsePhRID,
                int aMdsePhlSequence
                )
        {
            HierarchyNodeProfile aNode = null;
            switch (aMerchandiseType)
            {
                case (eMerchandiseType.HierarchyLevel):
                    {
                        if (aMdsePhRID != Include.NoRID)
                        {
                            HierarchyNodeProfile hnp = this._transaction.GetNodeData(aAllocationProfile.StyleHnRID);
                            HierarchyNodeList styleColorProfiles = this._transaction.GetDescendantData(hnp.Key, eHierarchyLevelType.Color, eNodeSelectType.All);
                            int headerColorRID = _headerColorRid;
                            if (headerColorRID == Include.DummyColorRID)
                            // Detail Packs will use the first bulk color as the basis when there is bulk; otherwise, all color ownership is used for detail packs
                            {
                                int colorSequence = int.MaxValue;
                                if (aAllocationProfile.BulkColorCount > 0)
                                {
                                    foreach (HdrColorBin hcb in aAllocationProfile.BulkColors.Values)
                                    {
                                        if (hcb.ColorSequence < colorSequence)
                                        {
                                            headerColorRID = hcb.ColorCodeRID;
                                            colorSequence = hcb.ColorSequence;
                                        }
                                    }
                                }
                            }
                            foreach (HierarchyNodeProfile colorProfile in styleColorProfiles)
                            {
                                if (colorProfile.ColorOrSizeCodeRID == headerColorRID) 
                                {
                                    aNode = this._transaction.GetAncestorDataByLevel(aMdsePhRID, colorProfile.Key, aMdsePhlSequence);
                                    break;
                                }
                            }
                            if (aNode == null)
                            {
                                throw new MIDException(eErrorLevel.severe,
                                    (int)eMIDTextCode.msg_al_ColorHierarchyNodeNotDefined,
                                    MIDText.GetText(eMIDTextCode.msg_al_ColorHierarchyNodeNotDefined) + ": " + hnp.NodeName + "/" + this._transaction.GetColorCodeProfile(headerColorRID).ColorCodeName);
                            }
                        }
                        break;
                    }
                case (eMerchandiseType.Node):
                    {
                        if (aMdseHnRID != Include.NoRID)
                        {
                            aNode = _sab.HierarchyServerSession.GetNodeData(aMdsePhRID, aMdseHnRID);
                        }
                        break;
                    }
                case (eMerchandiseType.OTSPlanLevel):
                    {
                        aNode = this._transaction.GetPlanLevelData(aAllocationProfile.PlanLevelStartHnRID);

                        if (aNode == null)
                        {
                            throw new MIDException(eErrorLevel.severe,
                                (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                                MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (aNode != null)
            {
                switch (aNode.LevelType)
                {
                    case (eHierarchyLevelType.Color):
                        _ibBasisColorRid = aNode.ColorOrSizeCodeRID;  
                        break;

                    case (eHierarchyLevelType.Size):
                        HierarchyNodeProfile colorNode = _sab.HierarchyServerSession.GetAncestorData(aMdsePhRID, aNode.Key, -1);
                        _ibBasisColorRid = colorNode.ColorOrSizeCodeRID;   
                        break;
                    default:
                        _ibBasisColorRid = Include.DummyColorRID;  
                        break;
                }
            }

            return aNode; 
        }

        // end TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result


        /// <summary>
        /// Returns a Size Cure Profile that may have alternates applied.  
        /// This was the curve used for creating the Plan.
        /// </summary>
        /// <param name="storeRid"></param>
        /// <returns></returns>
        public SizeCurveProfile GetSizeCurve(int storeRid)
        {
            return _sizeNeedResults.GetSizeCurve(storeRid);
        }

        /// <summary>
        /// Returns the Original Size Cure Profile before any alternates have been assigned.
        /// </summary>
        /// <param name="storeRid"></param>
        /// <returns></returns>
        public SizeCurveProfile GetOriginalSizeCurve(int storeRid)
        {
            return _sizeNeedResults.GetOriginalSizeCurve(storeRid);
        }

        public int GetSizeNeed_PlanUnits(int storeRid, int colorRid, int sizeRid) // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            return _sizeNeedResults.GetSizeNeed_PlanUnits(storeRid, sizeRid);   // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        }

        public int GetNeedUnits(int storeRid, int colorRid, int sizeRid)
        {
            return _sizeNeedResults.GetAllocatedUnits(storeRid, sizeRid);
        }



        private void CalculateSizeCurves(SizeCurveGroupProfile sizeCurveGroup)
        {
            //_timer.Start();
            try
            {
                //bool msgSent = false;
                ArrayList storesWithNoSizes = new ArrayList();
                _sizeNeedResults.SizeCurveGroup = sizeCurveGroup;
                //_sizeCurveHash.Clear();  // TT#2155 - JEllis - Fill Size Null Reference
                _sizeRestrictedCurveHash.Clear();
                _curveStoreList = new ProfileList(eProfileType.Store); // MID track 3631 Size Rules apply to all sizes on header
                _ruleStoreList = new ProfileList(eProfileType.Store);  // MID track 3631 Size Rules apply to all sizes on header

                // begin TT#2879 - Jellis - AnF - Null Reference when Fill Size in Workflow with Store Filter
                //int storeCount = _sizeNeedResults.Stores.Count; // MID Track 3631 Size Rules apply to all sizes on header
                //for (int i = 0; i < storeCount; i++)
                //{
                //StoreProfile aStore = (StoreProfile)_sizeNeedResults.Stores[i]; // MID Track 3631 Size Rules apply to all sizes on header
                //_ruleStoreList.Add(aStore);                    // MID track 3631 Size Rules apply to all sizes on header
                ProfileList allStores = (ProfileList)this._transaction.GetMasterProfileList(eProfileType.Store);
                foreach (StoreProfile aStore in allStores)
                {
                    //if (_sizeNeedResults.Stores.Contains(aStore.Key))      // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes 
                    if ((_allocProfile.GetStoreIsEligible(aStore.Key)) // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                    && _allocProfile.GetIncludeStoreInAllocation(aStore.Key) // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                    && _sizeNeedResults.Stores.Contains(aStore.Key)       // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                    && !_allocProfile.GetStoreOut(this._targetComponent, aStore.Key))   // TT#3105 - AnF - Jellis - Fill Size Holes not Filling all holes
                    {
                        _ruleStoreList.Add(aStore);
                    }
                    // end TT#2879 - Jellis - AnF - Null Reference when Fill Size in Workflow with Store Filter
                    //===========================================================
                    // Determines what the size curve is for the store and 
                    // sets it in the SizeNeedResults
                    //===========================================================
                    bool hasSizes = false;
                    if (_sizeCurveGroup.Key != Include.NoRID) // MID Track 3781 Size Curve Not required in which case only rules may be applied
                    {                            // MID Track 3781 Size Curve Not required in which case only rules may be applied
                        SizeCurveProfile sizeCurve = _sizeCurveGroup.GetStoreSizeCurveProfile(aStore.Key);
                        _sizeNeedResults.AddSizeCurve(aStore.Key, sizeCurve);
                        //=======================================================================
                        // Using the size curve is for the store 
                        // it builds a working size curve based upon the sizes on the header and 
                        // sets it in the SizeNeedResults
                        //=======================================================================

                        // Begin MID Issue #3338 - stodd
                        hasSizes = SetStoreRestrictedSizeCurve(aStore.Key);
                    }                            // MID Track 3781 Size Curve Not required in which case only rules may be applied
                    //============================================================================================
                    // If a store has no restricted sizes, it is marked for later removal from the store list.
                    // basically this means that the header contains no units to allocate in any of the store's
                    // sizes in it's size curve.
                    //=============================================================================================
                    if (!hasSizes)
                    {
                        storesWithNoSizes.Add(aStore);
                        // begin MID Track 3631 Size Rules apply to all sizes on header 
                        //if (!msgSent)
                        //{
                        //	_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_al_StoreSizeCurveDoesNotMatchHeader, this.ToString());
                        //	msgSent = true;
                        //}
                    }
                    else
                    {
                        _curveStoreList.Add(aStore);
                    }
                    // end MID Track 3631 Size Rules apply to all sizes on header
                    // End MID Issue #3338 - stodd
                }

                //=============================================
                // Begin MID Issue #3338 - stodd
                // remove stores from storelist with no sizes
                //=============================================
                if (storesWithNoSizes.Count > 0                    // TT#843 New Size Constraint Balance -- unrelated issue (no curve, yet message that curve and header do not match)
                    && _sizeCurveGroup.Key != Include.NoRID)       // TT#843 New Size Constraint Balance -- unrelated issue (no curve, yet message that curve and header do not match)
                {
                    // begin MID Track 3631 Size Rules apply to all sizes on header
                    _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetTextOnly(eMIDTextCode.msg_al_StoreSizeCurveDoesNotMatchHeader) + " Header [" + _allocProfile.HeaderID + "] Curve Group [" + sizeCurveGroup.ModelID + "] ", this.GetType().Name); // MID Track 5778 Schedule 'Run Now' gets error in audit  // TT#843 New Size Balance with constraintes -- Unrelated issue
                    //foreach (StoreProfile aStore in storesWithNoSizes)
                    //{
                    //	_storeList.Remove(aStore);
                    //}
                    // end MID Track 3631 Size Rules apply to all sizes on header
                }
                // begin TT#1669 - JEllis - Size Review Qty Allocated and Size Total Not in Balance
                //_sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, true); // TT#1543 - JEllis - Size Multiple Broken
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                _sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid);
                //if (_methodType == eMethodType.SizeNeedAllocation)
                //    _sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, false); // MID Track 3749 identify header color (not basis)
                //else if (_methodType == eMethodType.FillSizeHolesAllocation)
                //    _sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, true);  // MID Track 3749 identify header color (not basis)
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                // end TT#1669 - JEllis - Size Review Qty Allocated and Size Total Not in Balance
                //BuildOnHandAndIntransit(); // TT#1543 - JEllis - Size Multiple Broken Part 2 - Fill Holes Broken  // TT#2143 - JEllis - Size Review Infinite Loop
                // End MID Issue #3338 - stodd
                // Begin TT#784 - Size analysis shows double intransit - Jellis
                //if (this._sizeNeedResults.GetOnHandAndIT)
                //{
                    //BuildOnHandAndIntransit(); // TT#702 Infinite Loop when begin date set  // TT#766 - JEllis - ?????  // TT#1543 - JEllis - Size Multiple Broken
                //}
                // end TT#784 - Size analysis shows double intransit - Jellis
                // end MID Track 3631 Size Rules apply to all sizes on header.

                // begin TT#843 New Action Size Balance with Constraints
                //=====================================================================================
                // SizeStoreConstraints calulates and holds the store/size min, max, mult
                // by merging the constraint model with the allocation profile values.
                // it's not used in determining the plan, but it is used later for need.
                //
                //  NOTE:  previous to the Size Balance with Constraints, Size Need did not have to limit it
                //         maximums to capacity because Size Need was "spreading" an already allocated qty
                //         to the sizes (so capacity was already observed).  With the new function, we must
                //         guarantee that when the balance occurs the capacity of a store is not violated.
                //         In theory, this should not affect any size need allocations (unless for some 
                //         reason, the color allocation exceeded capacity).
                //=====================================================================================
                //_sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, true); // TT#1543 - JEllis - Size Multiple Broken
                // end TT#843 New Action Size Balance with Constraints

            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        // begin MID Track 3786 Change Fill Size Holes Algorithm
        /// <summary>
        /// Build Size Rule List, Onhand and Intransit
        /// </summary>
        private void BuildOnHandAndIntransit()
        {
            _sizeRuleList = new ProfileList(eProfileType.SizeCode);
            // BEGIN MID Track #3843 - Size Need gives Bulk Not Defined msg for Detail pack
            //HdrColorBin colorBin = _allocProfile.GetHdrColorBin(_colorRid);
            // END MID Track #3843
            // begin #TT185 Fill Size Holes does not work for WorkUpBuy
            //if (_allocProfile.WorkUpBulkSizeBuy
            //	|| _allocProfile.HeaderRID == Include.DefaultHeaderRID) // MID Track 4290 Size Analysis does not show Size Plan
            if (_allocProfile.HeaderRID == Include.DefaultHeaderRID)
            // end #TT185 Fill Size Holes does not work for WorkUpBuy
            {
                foreach (SizeCurveProfile scp in _sizeRestrictedCurveHash.Values)
                {
                    foreach (SizeCodeProfile szProf in scp.SizeCodeList)
                    {
                        if (_sizeRuleList.FindKey(szProf.Key) == null)
                        {
                            _sizeRuleList.Add(szProf);
                            GetSizeOHandIT(szProf.Key); // MID Track 3620 Size Minimum and Maximum are "up to" values
                        }
                    }
                }
            }
            // BEGIN MID Track #3843 - Size Need gives Bulk Not Defined msg for Detail pack
            else if (_targetComponent.ComponentType == eComponentType.SpecificColor)
            {
                // begin #TT185 Fill Size Holes does not work for WorkUpBuy
                // removed unnecessary comments
                //HdrColorBin colorBin = _allocProfile.GetHdrColorBin(_headerColorRid);
                HdrColorBin colorBin;
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    // begin TT#2156 - JEllis - WUB not allocating Size Rules
                    //if (_sizeRestrictedCurveHash.Count == 0)
                    //{
                    //    colorBin = _allocProfile.GetHdrColorBin(_headerColorRid);
                    //    foreach (int sizeCodeRID in colorBin.ColorSizes.Keys)
                    //    {
                    //        _sizeRuleList.Add(this._transaction.GetSizeCodeProfile(sizeCodeRID));
                    //        GetSizeOHandIT(sizeCodeRID);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!this._sizeNeedResults.NormalizeSizeCurves)      // TT#579 WUB not normalizing size curves
                    //    {                                                    // TT#579 WUB not normalizing size curves
                    //        foreach (SizeCurveProfile scp in _sizeRestrictedCurveHash.Values)
                    //        {
                    //            foreach (SizeCodeProfile szProf in scp.SizeCodeList)
                    //            {
                    //                if (_sizeRuleList.FindKey(szProf.Key) == null)
                    //                {
                    //                    _sizeRuleList.Add(szProf);
                    //                    GetSizeOHandIT(szProf.Key); // MID Track 3620 Size Minimum and Maximum are "up to" values
                    //                }
                    //            }
                    //        }
                    //    }                                                   // TT#579 WUB not normalizing size curves
                    //}
                    colorBin = _allocProfile.GetHdrColorBin(_headerColorRid);
                    foreach (int sizeCodeRID in colorBin.ColorSizes.Keys)
                    {
                        _sizeRuleList.Add(this._transaction.GetSizeCodeProfile(sizeCodeRID));
                        GetSizeOHandIT(sizeCodeRID);
                    }
                    if (!this._sizeNeedResults.NormalizeSizeCurves)      // TT#579 WUB not normalizing size curves
                    {                                                    // TT#579 WUB not normalizing size curves
                        foreach (SizeCurveProfile scp in _sizeRestrictedCurveHash.Values)
                        {
                            foreach (SizeCodeProfile szProf in scp.SizeCodeList)
                            {
                                if (_sizeRuleList.FindKey(szProf.Key) == null)
                                {
                                    _sizeRuleList.Add(szProf);
                                    GetSizeOHandIT(szProf.Key); // MID Track 3620 Size Minimum and Maximum are "up to" values
                                }
                            }
                        }
                    }                    
                    // end TT#2156 - JEllis - WUB not allocating Size Rules
                }
                else
                {
                    colorBin = _allocProfile.GetHdrColorBin(_headerColorRid);
                    // end #TT185 Fill Size Holes does not work for WorkUpBuy
                    foreach (int sizeCodeRID in colorBin.ColorSizes.Keys)
                    {
                        if (colorBin.GetSizeBin(sizeCodeRID).SizeUnitsToAllocate > 0)
                        {
                            _sizeRuleList.Add(this._transaction.GetSizeCodeProfile(sizeCodeRID));
                            GetSizeOHandIT(sizeCodeRID);
                        }
                    }
                    if (!this._sizeNeedResults.NormalizeSizeCurves)
                    // end MID Track 5208 Fill Size Constraints Only Not Working
                    {
                        foreach (SizeCurveProfile scp in _sizeRestrictedCurveHash.Values)
                        {
                            foreach (SizeCodeProfile szProf in scp.SizeCodeList)
                            {
                                if (_sizeRuleList.FindKey(szProf.Key) == null)
                                {
                                    _sizeRuleList.Add(szProf);
                                    GetSizeOHandIT(szProf.Key); // MID Track 3620 Size Minimum and Maximum are "up to" values
                                }
                            }
                        }
                    }
                }  // #TT185 Fill Size Holes does not work for WorkUpBuy
                // end MID Track 4861 Size Curve Normalization
            }
            else if (_targetComponent.ComponentType == eComponentType.DetailType)
            {
                foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values)
                {
                    foreach (PackColorSize pcs in ph.PackColors.Values)
                    {
                        foreach (PackSizeBin packSize in pcs.ColorSizes.Values) // Assortment: added pack size bin
                        {
                            if (packSize.ContentUnits > 0)
                            {
                                SizeCodeProfile scp = this._transaction.GetSizeCodeProfile(packSize.ContentCodeRID);
                                if (!_sizeRuleList.Contains(scp))
                                {
                                    // begin MID Track 4861 Size Curve Normalization
                                    //_sizeRuleList.Add(this._transaction.GetSizeCodeProfile(packSize.ContentKey));
                                    _sizeRuleList.Add(scp);
                                    // end MID Track 4861 Size Curve Normalization
                                    GetSizeOHandIT(packSize.ContentCodeRID);
                                }
                            }
                        }
                    }
                }

				// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                HdrColorBin colorBin;
                if (_allocProfile.BulkColorIsOnHeader(_headerColorRid))
                {
                    colorBin = _allocProfile.GetHdrColorBin(_headerColorRid);
                    foreach (int sizeCodeRID in colorBin.ColorSizes.Keys)
                    {
                        if (_sizeRuleList.FindKey(sizeCodeRID) == null)
                        {

                            if (colorBin.GetSizeBin(sizeCodeRID).SizeUnitsToAllocate > 0)
                            {
                                _sizeRuleList.Add(this._transaction.GetSizeCodeProfile(sizeCodeRID));
                                GetSizeOHandIT(sizeCodeRID);
                            }
                        }
                    }
                }


                if (!this._sizeNeedResults.NormalizeSizeCurves)
                {
                    foreach (SizeCurveProfile scp in _sizeRestrictedCurveHash.Values)
                    {
                        foreach (SizeCodeProfile szProf in scp.SizeCodeList)
                        {
                            if (_sizeRuleList.FindKey(szProf.Key) == null)
                            {
                                _sizeRuleList.Add(szProf);
                                GetSizeOHandIT(szProf.Key); 
                            }
                        }
                    }
                }
				// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
            }
            // END MID Track #3843

            // Begin TT#5026 - JSmith - Question about Size Alternates
            if (_sizeAlternateHash != null &&
                _sizeAlternateHash.Count > 0)
            {
                foreach (ArrayList altSizeCodeList in _sizeAlternateHash.Values)
                {
                    foreach (SizeCodeProfile altSizeCode in altSizeCodeList) // MID Track 4121
                    {
                        if (_sizeRuleList.FindKey(altSizeCode.Key) == null)
                        {
                            GetSizeOHandIT(altSizeCode.Key);
                        }
                    }
                }
            }
            // End TT#5026 - JSmith - Question about Size Alternates
        }
        // end MID Track 3687 Change Fill Size Holes Algorithm

        // Begin TT#5026 - JSmith - Question about Size Alternates
        public ArrayList GetAlternateSizeCodes(int aStoreRID, int aSizeCodeRID)
        {
            ArrayList altSizeCodeList = null;
            altSizeCodeList = (ArrayList)_sizeAlternateHash[aSizeCodeRID];
            if (altSizeCodeList == null)
            {
                // Begin TT#1841-MD - JSmith - Select Size Review and Receive a Null Reference Error.
                //if (_sizeCodeList == null)
                //{
                //    SetStoreSizeCodeList(aStoreRID);
                //}
                altSizeCodeList = new ArrayList();
                //SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)_sizeCodeList.FindKey(aSizeCodeRID);
                //if (sizeCodeProfile == null)
                //{
                //    sizeCodeProfile = new SizeCodeProfile(aSizeCodeRID);
                //}
                //altSizeCodeList.Add(sizeCodeProfile);
                // End TT#1841-MD - JSmith - Select Size Review and Receive a Null Reference Error.
            }
            return altSizeCodeList;
        }
        // End TT#5026 - JSmith - Question about Size Alternates

        // begin MID Track 3620 Size Minimum and Maximum are "up to" values
        /// <summary>
        /// Gets Size OnHand and Intransit values for the stores
        /// </summary>
        /// <param name="aSizeCodeRID">RID that identifies the size</param>
        private void GetSizeOHandIT(int aSizeCodeRID)
        {
            AllocationSubtotalProfile subtotalProfile = _transaction.GetAllocationGrandTotalProfile();
            IntransitKeyType iKt = new IntransitKeyType(_basisColorRid, aSizeCodeRID);   // MID Track 3749 Use the basis color ownership
            // begin TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
            //IntransitKeyType[] iKtArray = new IntransitKeyType[1];
            //iKtArray[0] = iKt;
            IntransitKeyType ibIkt = new IntransitKeyType(_ibBasisColorRid, aSizeCodeRID);
            // end TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
            int unitValue;
            int ibUnitValue; // TT#304 - MD - JEllis - Size inventory Min Max not giving correct result

            // begin TT#4177 - MD - Jellis - GA Size Need In Store Minimum not honored
            //// begin MID Track 3749 note:  get intransit/onhand from style or higher node (iKt already contains the color and size information
            //int mdseRID;
            //// BEGIN TT#3078 - MD - JEllis - Size Allocations Wrong when begin date specified
            ////if (_mdseSizeBasis.LevelType == eHierarchyLevelType.Color
            ////    || _mdseSizeBasis.LevelType == eHierarchyLevelType.Size)
            //if (_allocProfile.BeginDay == Include.UndefinedDate
            //    && (_mdseSizeBasis.LevelType == eHierarchyLevelType.Color
            //        || _mdseSizeBasis.LevelType == eHierarchyLevelType.Size))
            //// END TT#3078 - MD - Jellis - Size Allocations wrong when begin date specified
            //{
            //    mdseRID = (this._sab.HierarchyServerSession.GetAncestorData(_mdseSizeBasis.Key, eHierarchyLevelType.Style)).Key;
            //}
            //else
            //{
            //    mdseRID = _mdseSizeBasis.Key;
            //}
            //// end MID Track 3749

            //_sizeNeedResults.MerchandiseBasisRID = mdseRID;  // TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max
            int mdseRID = _mdseSizeBasis.Key;
            _sizeNeedResults.MerchandiseBasisRID = mdseRID;
            // end TT#4177 - MD - Jellis - GA Size Need In Store Minimum not honored

            // begin TT#2313 - JEllis - AnF VSW - Size Need not using VSW OH
            if (!_allocProfile.IMO)
            {
                HdrSizeBin sizeBin = null;
                if (_allocProfile.BulkSizeIntransitUpdated)
                {
                    sizeBin = (HdrSizeBin)_color.ColorSizes[aSizeCodeRID];
                }
                foreach (StoreProfile sp in _sizeNeedResults.Stores)
                {
                    unitValue =
                        _transaction.GetStoreImoHistory(
                        _allocProfile,
                        iKt,
                        sp.Key);
                    // begin TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
                    ibUnitValue =
                         _transaction.GetStoreImoHistory(
                         _allocProfile,
                         ibIkt,
                         sp.Key);
                    // end TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
                    if (sizeBin != null)
                    {
                        unitValue -=
                            sizeBin.GetStoreSizeImoUnitsAllocated(_allocProfile.StoreIndex(sp.Key).Index);
                        // begin TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
                        ibUnitValue -=
                            sizeBin.GetStoreSizeImoUnitsAllocated(_allocProfile.StoreIndex(sp.Key).Index);
                        // end TT#304 - MD - Jellis - Size Inventory Min Max Incorrect
                    }
                    _sizeNeedResults.AddVswOnhandUnits(sp.Key, aSizeCodeRID, unitValue);
                    _sizeNeedResults.AddIbVswOnhandUnits(sp.Key, aSizeCodeRID, ibUnitValue); // TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
                }
            }
            // end TT2313 - JEllis - AnF VSW - Size Need not using VSW OH

            // begin TT#4177 - MD - Jellis - GA Size Need In Store Minimum not honored
            //// begin TT#1074 - MD - Jellis - Inventory Min Max incorrect for Group Allocation
            //// begin TT#41 - MD - Jellis - Size inventory min max
            //int ibMdseRID;
            //// BEGIN TT#3078 - MD - JEllis - Size Allocations Wrong  when begin date specified
            ////if (_ibMdseSizeBasis.LevelType == eHierarchyLevelType.Color
            ////    || _ibMdseSizeBasis.LevelType == eHierarchyLevelType.Size)
            //if (_allocProfile.BeginDay == Include.UndefinedDate
            //    && (_ibMdseSizeBasis.LevelType == eHierarchyLevelType.Color
            //        || _ibMdseSizeBasis.LevelType == eHierarchyLevelType.Size))
            //// END TT#3078 - MD - Jellis - Size Allocations wrong when begin date specified
            //{
            //    ibMdseRID = (this._sab.HierarchyServerSession.GetAncestorData(_ibMdseSizeBasis.Key, eHierarchyLevelType.Style)).Key;
            //}
            //else
            //{
            //    ibMdseRID = _ibMdseSizeBasis.Key;
            //}
            //_sizeNeedResults.InventoryMdseBasisRID = ibMdseRID; 
            //// end TT#1074- MD - Jellis - Inventory Min Max incorrect for Group Allocation
            //// end TT#41 - MD - Jellis - Size Inventory min max
            int ibMdseRID = _ibMdseSizeBasis.Key;
            _sizeNeedResults.InventoryMdseBasisRID = ibMdseRID;
            // end TT#4177 - MD - Jellis - GA Size Need In Store Minimum not honored
            
            // begin TT#1592 - MD - Jellis Bulk Color Size Need using wrong ownership 
            AllocationProfile ap;
            AssortmentProfile asrtP;
            int hdrStyleRID = Include.NoRID;
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            //if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (!(_allocProfile is AllocationProfile && _allocProfile.isAssortmentProfile))
            if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (!(_allocProfile is AssortmentProfile) && _allocProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in GetSizeOHandIT()");
            }
            #endif
            //if (_allocProfile is AssortmentProfile)
            if (_allocProfile.isAssortmentProfile)
            // End TT#4988 - BVaughan - Performance
            {
                ap = _allocProfile;
                asrtP = ap as AssortmentProfile;
                hdrStyleRID = _allocProfile.StyleHnRID;		// TT#1788-MD - stodd - Fill Size not allocating bulk when headers belong to a group allocation
            }
            else if (_allocProfile.AssortmentProfile != null)
            {
                ap = _allocProfile.AssortmentProfile;
                asrtP = _allocProfile.AssortmentProfile;
                hdrStyleRID = _allocProfile.StyleHnRID;
            }
            else
            {
                ap = _allocProfile;
                hdrStyleRID = ap.StyleHnRID;
                asrtP = null;
            }
            int packHomeStyleRID = Include.NoRID;
            // end TT#1567 - MD - Jellis - GA Bulk not allocated as expected 

            foreach (StoreProfile sp in this._sizeNeedResults.Stores)
            {
                unitValue =
                    this._transaction.GetStoreOnHand
                    (_allocProfile,
                    mdseRID,  // MID Track 3749
                    _allocProfile.BeginDay,
                    iKt,
                    sp.Key);
                _sizeNeedResults.AddOnhandUnits(sp.Key, aSizeCodeRID, unitValue);
                unitValue = subtotalProfile.GetStoreInTransit(iKt, sp.Key, mdseRID);  // MID Track 3749
                _sizeNeedResults.AddIntransitUnits(sp.Key, aSizeCodeRID, unitValue);
                // begin TT#41 - MD - JEllis - Size inventory min max
                ibUnitValue =                                 // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                    this._transaction.GetStoreOnHand
                    (_allocProfile,
                    ibMdseRID,  // MID Track 3749
                    _allocProfile.BeginDay,
                    ibIkt,      // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                    sp.Key);
                _sizeNeedResults.AddIbOnhandUnits(sp.Key, aSizeCodeRID, ibUnitValue);     // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                ibUnitValue = subtotalProfile.GetStoreInTransit(ibIkt, sp.Key, ibMdseRID);  // MID Track 3749 // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                _sizeNeedResults.AddIbIntransitUnits(sp.Key, aSizeCodeRID, ibUnitValue);  // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                // end TT#41 - MD - Jellis - Size inventory min max
                // begin MID Track 4121 Size Need Overallocates stores
                unitValue = 0;
                ibUnitValue = 0;
                // begin TT#1592 - MD - Jellis Bulk Color Size Need using wrong ownership 
                //// begin TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                //AllocationProfile ap;
                //AssortmentProfile asrtP;
                //if (_allocProfile is AssortmentProfile)
                //{
                //    ap = _allocProfile;
                //    asrtP = ap as AssortmentProfile;
                //}
                //else if (_allocProfile.AssortmentProfile != null)
                //{
                //    ap = _allocProfile.AssortmentProfile;
                //    asrtP = _allocProfile.AssortmentProfile;
                //}
                //else
                //{
                //    ap = _allocProfile;
                //    asrtP = null;
                //}
                //// end TT#1502 - MD - Jellis - Bulk Color Size Need using wrong ownership
                // end TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                if (_targetComponent.ComponentType == eComponentType.SpecificColor)
                {
                    // begin TT#784 - Size Analysis showing double intransit - Jellis
                    // begin TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                    //if (_allocProfile.StyleIntransitUpdated == false) 
                    //{                                                 
                        //foreach (PackHdr ph in _allocProfile.Packs.Values) 
                    bool styleIntransitUpdated = false;
                    //  end  TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                    // Begin TT#2043-MD - JSmith - In an Asst PPK and Bulk Header Size Allocation - Curve Adj On Hand and Intransit not including PPKS when determining Size Need
                    //foreach (PackHdr ph in ap.Packs.Values)
                    List<PackHdr> packs = new List<PackHdr>();
                    if (asrtP != null
                        && asrtP.AsrtType != (int)eAssortmentType.GroupAllocation)
                    {
                        foreach (AllocationProfile allocationProfile in asrtP.AssortmentMembers)
                        {
                            if (allocationProfile.HeaderType != eHeaderType.Placeholder)
                            {
                                foreach (PackHdr ph in allocationProfile.Packs.Values)
                                {
                                    packs.Add(ph);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (PackHdr ph in ap.Packs.Values)
                        {
                            packs.Add(ph);
                        }
                    }
                    foreach (PackHdr ph in packs)
                    // End TT#2043-MD - JSmith - In an Asst PPK and Bulk Header Size Allocation - Curve Adj On Hand and Intransit not including PPKS when determining Size Need
                    {
                        // begin TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                        if (asrtP != null)
                        {
                            styleIntransitUpdated = asrtP.GetAssortmentPackHome(ph.PackRID).StyleIntransitUpdated;
                            packHomeStyleRID = asrtP.GetAssortmentPackHome(ph.PackRID).StyleHnRID; // TT#1592 - MD - Jellis - Jellis Bulk Color Size neeed using wrong ownership
                        }
                        else
                        {
                            styleIntransitUpdated = ap.StyleIntransitUpdated;
                            packHomeStyleRID = ap.StyleHnRID;
                        }
                        if (styleIntransitUpdated == false)
                        {
                            //  end  TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                            if (packHomeStyleRID == hdrStyleRID) // TT#1592 - MD - Jellis - Jellis Bulk Color Size neeed using wrong ownership
                            {                                    // TT#1592 - MD - Jellis - Jellis Bulk Color Size neeed using wrong ownership 
                                foreach (PackColorSize pcs in ph.PackColors.Values)
                                {
                                    if (_basisColorRid == Include.DummyColorRID
                                        || pcs.ColorCodeRID == _basisColorRid)
                                    {
                                        if (pcs.SizeIsInColor(aSizeCodeRID))
                                        {
                                            unitValue +=
                                                ph.GetStorePacksAllocated(this._transaction.StoreIndexRID(sp.Key).Index)
                                                * pcs.GetSizeBin(aSizeCodeRID).ContentUnits;
                                        }
                                    }
                                    // begin TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                                    if (_ibBasisColorRid == Include.DummyColorRID
                                        || pcs.ColorCodeRID == _ibBasisColorRid)
                                    {
                                        if (pcs.SizeIsInColor(aSizeCodeRID))
                                        {
                                            ibUnitValue +=
                                                ph.GetStorePacksAllocated(this._transaction.StoreIndexRID(sp.Key).Index)
                                                * pcs.GetSizeBin(aSizeCodeRID).ContentUnits;
                                        }
                                    }
                                    // end TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                                }
                            } // TT#1592 - MD - Jellis - Jellis Bulk Color Size neeed using wrong ownership
                        } //  end  TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                    }
                    //} // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    // end TT#784 - Size Analysis showing double intransit - JEllis

                }
                else if (_targetComponent.ComponentType == eComponentType.DetailType)
                {
                    //foreach (HdrColorBin hcb in _allocProfile.BulkColors.Values) // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    foreach (HdrColorBin hcb in ap.BulkColors.Values) // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    {
                        if (hcb.ColorCodeRID == _basisColorRid
                            || _basisColorRid == Include.DummyColorRID)
                        {
                            if (hcb.SizeIsInColor(aSizeCodeRID))
                            {
                                unitValue +=
                                    ap.GetStoreQtyAllocated(hcb, aSizeCodeRID, this._transaction.StoreIndexRID(sp.Key)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                                //_allocProfile.GetStoreQtyAllocated(hcb, aSizeCodeRID, this._transaction.StoreIndexRID(sp.Key)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                            }
                        }
                        // begin TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                        if (hcb.ColorCodeRID == _ibBasisColorRid
                            || _ibBasisColorRid == Include.DummyColorRID)
                        {
                            if (hcb.SizeIsInColor(aSizeCodeRID))
                            {
                                ibUnitValue +=
                                    ap.GetStoreQtyAllocated(hcb, aSizeCodeRID, this._transaction.StoreIndexRID(sp.Key)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                                    //_allocProfile.GetStoreQtyAllocated(hcb, aSizeCodeRID, this._transaction.StoreIndexRID(sp.Key)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                            }
                        }
                        // end TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                    }
                }
                _sizeNeedResults.AddIntransitUnits(sp.Key, aSizeCodeRID, unitValue);
                _sizeNeedResults.AddIbIntransitUnits(sp.Key, aSizeCodeRID, ibUnitValue); // TT#41 - MD - JEllis - Size Inventory Min Max // TT#304 - MD - JEllis - Size Inventory Min Max not giving correct result
                // end MID Track 4121 Size Need Overallocates stores
            }
            // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            _sizeNeedResults.ResetCache();
            // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        }
        // end MID Track 3620 OnHand and Intransit are "up to" values

		// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        private void BuildPriorDetailAllocated()
        {
            Index_RID[] storeIdxRIDArray = _transaction.StoreIndexRIDArray();
            //foreach (PackHdr ph in _allocProfile.GenericPacks.Values)
            //{
            //    foreach (PackColorSize pcs in ph.PackColors.Values)
            //    {
            //        if (pcs.ColorCodeRID == _basisColorRid
            //            || _basisColorRid == Include.DummyColorRID)
            //        {
            //            foreach (PackContentBin pcb in pcs.ColorSizes.Values)
            //            {
            //                foreach (Index_RID storeIdxRID in storeIdxRIDArray)
            //                {
            //                    _sizeNeedResults.AddPriorAllocatedUnits(
            //                        storeIdxRID.RID,
            //                        pcb.ContentCodeRID,
            //                        ph.GetStorePacksAllocated(storeIdxRID.Index) * pcb.ContentUnits);
            //                }
            //            }
            //        }
            //    }
            //}
            foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough
            {
                foreach (PackColorSize pcs in ph.PackColors.Values)
                {
                    if (pcs.ColorCodeRID == _basisColorRid  // MID Track 3749 include pack if basis color in pack
                        || _basisColorRid == Include.DummyColorRID) // MID Track 3749 include pack if basis is all colors
                    {
                        foreach (PackContentBin pcb in pcs.ColorSizes.Values)
                        {
                            foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                            {
                                _sizeNeedResults.AddPriorDetailAllocatedUnits(
                                    storeIdxRID.RID,
                                    pcb.ContentCodeRID,
                                    ph.GetStorePacksAllocated(storeIdxRID.Index) * pcb.ContentUnits);
                            }
                        }
                    }
                }
            }
        }
		// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

        // begin TT#1600 - JEllis - Size Need Algorithm Error
        /// <summary>
        /// Builds the units already allocated by size and puts them in the Size Need Results
        /// </summary>
        private void BuildPriorAllocated()
        {
            if (_sizeNeedResults.AccumulatePriorAllocated)
            {
                // begin TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                //Hashtable bulkColors; // TT#1591 - MD - Jellis - GA - Size Need Over Allocated Size
                SortedList genericPacks;
                SortedList nonGenericPacks;
                AllocationProfile ap;
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                //if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (_allocProfile is AllocationProfile && _allocProfile.isAssortmentProfile))
                if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (!(_allocProfile is AssortmentProfile) && _allocProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in BuildPriorAllocated()");
                }
                #endif
                //if (_allocProfile is AssortmentProfile)
                if (_allocProfile.isAssortmentProfile)
                // End TT#4988 - BVaughan - Performance
                {
                    ap = _allocProfile;
                }
                else if (_allocProfile.AssortmentProfile != null)
                {
                    ap = _allocProfile.AssortmentProfile;
                }
                else
                {
                    ap = _allocProfile;
                }
                //bulkColors = ap.BulkColors; // TT#1591 - MD - Jellis - GA - Size Need Over Allocated Size 
                genericPacks = ap.GenericPacks;
                nonGenericPacks = ap.NonGenericPacks;
                // end TT#1567 - MD - Jellis - GA Bulk not allocated as expected 

                Index_RID[] storeIdxRIDArray = _transaction.StoreIndexRIDArray();
                if (_targetComponent.ComponentType == eComponentType.SpecificColor)
                {
                    // begin TT#1591 - MD - Jellis - GA - Size Need Over Allocated Size
                    List<AllocationProfile> headersWithColor;
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in BuildPriorAllocated()");
                    }
                    #endif
                    //if (ap is AssortmentProfile)
                    if (ap.isAssortmentProfile)
                    // End TT#4988 - BVaughan - Performance
                    {
                        headersWithColor = ((AssortmentProfile)ap).GetHeadersWithColor(_color.ColorCodeRID);
                    }
                    else
                    {
                        headersWithColor = new List<AllocationProfile>();
                        headersWithColor.Add(ap);
                    }
                    foreach (AllocationProfile memberAp in headersWithColor)
                    {
                        if (memberAp.StyleHnRID == _allocProfile.StyleHnRID)
                        {
                            HdrColorBin colorBin = (HdrColorBin)memberAp.BulkColors[_color.ColorCodeRID];
                            if (colorBin == null)
                            {
                                throw new MIDException(eErrorLevel.severe,
                                     (int)(eMIDTextCode.msg_ColorNotDefinedInBulk),
                                     MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInBulk)
                                     + " : Header ID [" + memberAp.HeaderID + "] ColorCodeRID [" + _color.ColorCodeRID.ToString(CultureInfo.CurrentUICulture) + "]"
                                     + " : Source/Method [" + GetType().Name + " / BuildPriorAllocated]");
                            }
                            if (memberAp.HeaderRID == _allocProfile.HeaderRID)
                            {
                                foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                {
                                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                                    {
                                        _sizeNeedResults.AddAllocatedUnits(storeIdxRID.RID, hsb.SizeCodeRID, memberAp.GetStoreQtyAllocated(hsb, storeIdxRID));
                                    }
                                }

                            }
                            else
                            {
                                foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                                {
                                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                                    {
                                        _sizeNeedResults.AddPriorAllocatedUnits(storeIdxRID.RID, hsb.SizeCodeRID, memberAp.GetStoreQtyAllocated(hsb, storeIdxRID));
                                    }
                                }
                            }
                        }
                    }
                    //// begin TT#1567 - MD - Jellis - GA Bulk not allocated as expected 
                    //HdrColorBin colorBin = (HdrColorBin)bulkColors[_color.ColorCodeRID];
                    //if (colorBin == null)
                    //{
                    //    throw new MIDException(eErrorLevel.severe,
                    //         (int)(eMIDTextCode.msg_ColorNotDefinedInBulk),
                    //         MIDText.GetText(eMIDTextCode.msg_ColorNotDefinedInBulk)
                    //         + " : Header ID [" + ap.HeaderID + "] ColorCodeRID [" + _color.ColorCodeRID.ToString(CultureInfo.CurrentUICulture) + "]"
                    //         + " : Source/Method [" + GetType().Name + " / BuildPriorAllocated]");  
                    //}
                    ////foreach (HdrSizeBin hsb in _color.ColorSizes.Values) 
                    //foreach (HdrSizeBin hsb in colorBin.ColorSizes.Values)
                    //// end TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    //{
                    //    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                    //    {
                    //        //_sizeNeedResults.AddAllocatedUnits(storeIdxRID.RID, hsb.SizeCodeRID, _allocProfile.GetStoreQtyAllocated(hsb, storeIdxRID)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    //        _sizeNeedResults.AddAllocatedUnits(storeIdxRID.RID, hsb.SizeCodeRID, ap.GetStoreQtyAllocated(hsb, storeIdxRID)); // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    //    }
                    //}
                    // end TT#1591 - MD - Jellis - GA - Size Need Over Allocated Size
                }
                else if (_targetComponent.ComponentType == eComponentType.DetailType)
                {
                    //foreach (PackHdr ph in _allocProfile.GenericPacks.Values) // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    foreach (PackHdr ph in genericPacks.Values) // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    {
                        foreach (PackColorSize pcs in ph.PackColors.Values)
                        {
                            if (pcs.ColorCodeRID == _basisColorRid
                                || _basisColorRid == Include.DummyColorRID)
                            {
                                foreach (PackContentBin pcb in pcs.ColorSizes.Values)
                                {
                                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                                    {
                                        _sizeNeedResults.AddPriorAllocatedUnits(
                                            storeIdxRID.RID,
                                            pcb.ContentCodeRID,
                                            ph.GetStorePacksAllocated(storeIdxRID.Index) * pcb.ContentUnits);
                                    }
                                }
                            }
                        }
                    }
                    //foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    foreach (PackHdr ph in nonGenericPacks.Values) // TT#1567 - MD - Jellis - GA Bulk not allocated as expected
                    {
                        foreach (PackColorSize pcs in ph.PackColors.Values)
                        {
                            if (pcs.ColorCodeRID == _basisColorRid  // MID Track 3749 include pack if basis color in pack
                                || _basisColorRid == Include.DummyColorRID) // MID Track 3749 include pack if basis is all colors
                            {
                               foreach (PackContentBin pcb in pcs.ColorSizes.Values)
                                {
                                    foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                                    {
                                        _sizeNeedResults.AddAllocatedUnits(
                                            storeIdxRID.RID,
                                            pcb.ContentCodeRID,
                                            ph.GetStorePacksAllocated(storeIdxRID.Index) * pcb.ContentUnits);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("component type not color or detail type");
                }
                // begin TT#1176 - MD - Jellis - Group Allocation Size need Not observing inv min max
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                //if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (_allocProfile is AllocationProfile && _allocProfile.isAssortmentProfile))
                if ((_allocProfile is AssortmentProfile && !_allocProfile.isAssortmentProfile) || (!(_allocProfile is AssortmentProfile) && _allocProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in BuildPriorAllocated()");
                }
                #endif
                //if (!(_allocProfile is AssortmentProfile)
                // && _allocProfile.AssortmentProfile != null)
                if (!(_allocProfile.isAssortmentProfile)
                   && _allocProfile.AssortmentProfile != null)
                // End TT#4988 - BVaughan - Performance
                {
                    Hashtable sizeCodeRIDs = _allocProfile.AssortmentProfile.GetSizesOnHeader();
                    int groupMemberSizeUnitsAllocated;
                    // begin TT#4167 - MD - Jellis - GA Size Need Unexpected Result
                    int inventoryBasisHnRID = _allocProfile.StyleHnRID;
                    if (_basisColorRid != Include.DummyColorRID)
                    {
                        HierarchyNodeList colorHnRID_pl =
                            _transaction.GetDescendantData(_allocProfile.StyleHnRID, eHierarchyLevelType.Color, eNodeSelectType.All);
                        foreach (HierarchyNodeProfile hncp in colorHnRID_pl)
                        {
                            if (hncp.ColorOrSizeCodeRID == _basisColorRid)
                            {
                                // Begin TT#4813 - JSmith - GA- Headers 2 different PPKS and BULK.  2 Headers same style/color.  Size allocation not taking in to consideration prior header allocation.  
                                //inventoryBasisHnRID = hncp.ColorOrSizeCodeRID;
                                inventoryBasisHnRID = hncp.Key;
                                // End TT#4813 - JSmith - GA- Headers 2 different PPKS and BULK.  2 Headers same style/color.  Size allocation not taking in to consideration prior header allocation.  
                                break;
                                // NOTE:  since the _basisColorRid is on this header, expect to find it
                            }
                        }
                    }
                    // end TT#4167 - MD - Jellis - GA Size Need Unexpected Result
                    foreach (int sizeRID in sizeCodeRIDs.Values)
                    {
                        foreach (Index_RID storeIdxRID in storeIdxRIDArray)
                        {
                            groupMemberSizeUnitsAllocated =
                                _sizeNeedResults.GetStoreInventoryAllocationAdjustment(inventoryBasisHnRID, storeIdxRID.RID, sizeRID);  // TT#4167 - MD - Jellis - GA Size Need Unexpected Result
                                //_sizeNeedResults.GetStoreInventoryAllocationAdjustment(_allocProfile.StyleHnRID, storeIdxRID.RID, sizeRID);  // TT#4167 - MD - Jellis - GA Size Need Unexpected Result

                            _sizeNeedResults.AddGroupMemberAllocatedUnits(
                                storeIdxRID.RID,
                                sizeRID,
                                groupMemberSizeUnitsAllocated);

                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            // Get bulk units only to adjust size need since packs are included in the intransit
                            groupMemberSizeUnitsAllocated =
                                _sizeNeedResults.GetStoreInventoryAllocationAdjustment(inventoryBasisHnRID, storeIdxRID.RID, sizeRID, false, true); 
                            
                            _sizeNeedResults.AddGroupMemberAllocatedBulkUnits(
                                storeIdxRID.RID,
                                sizeRID,
                                groupMemberSizeUnitsAllocated);
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
                        }
                    }
                }
                // end TT#1176 - MD - Jellis- Group Allocation Size need not observing inv min max
                _sizeNeedResults.AccumulatePriorAllocated = false;
            }
        }
        // end TT#1600 - JEllis - Size Need Algorithm Error

        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        //private void CalculatePlan(SizeCurveGroupProfile sizeCurveGroup, eSizeNeedColorPlan sizeColorPlan, eSizeMethodType sizeMethodType, bool aCalculateFillPlan, bool aUseBasisPlan)
        //{
            
        //    CalculatePlanWithSizeMinimums(sizeCurveGroup, sizeColorPlan, sizeMethodType, aCalculateFillPlan, aUseBasisPlan, useSizeMins);
        //}
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation

        private void CalculatePlan(SizeCurveGroupProfile sizeCurveGroup, eSizeNeedColorPlan sizeColorPlan, eSizeMethodType sizeMethodType, bool aCalculateFillPlan, bool aUseBasisPlan) // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement //TT#848-MD -jsobek -Fill to Size Plan Presentation
        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            try
            {
                int storeTotalPlan = 0;

                // begin TT#843 New Action Size Balance with Constraints
                ////=====================================================================================
                //// SizeStoreConstraints calulates and holds the store/size min, max, mult
                //// by merging the constraint model with the allocation profile values.
                //// it's not used in determining the plan, but it is used later for need.
                ////=====================================================================================
                //// begin MID track 4291 add fill variables to size review
                //if (aCalculateFillPlan)
                //{
                //}
                //else
                //{
                //    // end MID Track 4291 add fill variables to size review
                //    if (_methodType == eMethodType.SizeNeedAllocation)
                //        _sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, false); // MID Track 3749 identify header color (not basis)
                //    else if (_methodType == eMethodType.FillSizeHolesAllocation)
                //        _sizeNeedResults.StoreSizeConstraints = new StoreSizeConstraints(_sab, _constraintModelRid, _allocProfile, _headerColorRid, true);  // MID Track 3749 identify header color (not basis)
                //} // MID Trck 4291 add fill variables to size review
                // end TT#843 New Action Size Balance with Constraints

                //=================================
                // determine total plan by store
                //=================================
                int storeCount = _curveStoreList.Count; // MID Track 3631 Size Rules apply to all sizes on header
                bool accumulatePriorAllocated = _sizeNeedResults.AccumulatePriorAllocated; // MID Track 4291 add fill variables to size review
                AllocationSubtotalProfile grandTotalProfile = this._transaction.GetAllocationGrandTotalProfile(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                DayProfile onhandDayProfile = grandTotalProfile.OnHandDayProfile; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                if (aUseBasisPlan)
                {
                    if (_sizeNeedResults.LoadFillToPlan_BasisSaleStock)
                    {
                        foreach (Index_RID storeIdxRID in _transaction.StoreIndexRIDArray())
                        {
                            DayProfile shipDayProfile = _transaction.SAB.ApplicationServerSession.Calendar.GetDay(grandTotalProfile.GetStoreShipDay(this._allocProfile, storeIdxRID.RID));
                            int units = this._transaction.GetStoreOTSSalesPlan(
                                storeIdxRID.RID,
                                this._mdseSizeBasis.Key,
                                _allocProfile.GetCubeEligibilityNode(),
                                onhandDayProfile,
                                shipDayProfile,
                                100.00);
                            _sizeNeedResults.AddFillToPlan_SalesUnits(storeIdxRID.RID, units);
                            units = this._transaction.GetStoreOTSStockPlan(
                                storeIdxRID.RID,
                                this._mdseSizeBasis.Key,
                                _allocProfile.GetCubeEligibilityNode(),
                                shipDayProfile,
                                100.00);
                            _sizeNeedResults.AddFillToPlan_StockUnits(storeIdxRID.RID, units);
                        }
                        _sizeNeedResults.LoadFillToPlan_BasisSaleStock = false;
                    }
                }
                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                for (int i = 0; i < storeCount; i++)
                {
                    int sizeCodeCount = 0;
                    int s = 0;
                    storeTotalPlan = 0;

                    StoreProfile aStore = (StoreProfile)_curveStoreList[i]; // MID Track 3631 Size Rules apply to all sizes on header
                    Index_RID storeIdxRID = _transaction.StoreIndexRID(aStore.Key);

                    //===========================================================
                    // This method sets the value of 
                    // _sizeCodeList & _sizeCurve for the store
                    //===========================================================
                    sizeCodeCount = SetStoreSizeCodeList(aStore.Key);

                    if (sizeCodeCount > 0)
                    {
                        //==============================================================
                        // Fill size holes does not use the qty allocated for the color
                        //==============================================================
                        if (sizeMethodType == eSizeMethodType.FillSizeHolesAllocation // MID Track 4291 add fill variables to size review
                            || aCalculateFillPlan) // MID Track 4291 add fill variables to size review
                        {
                            storeTotalPlan = 0;
                        }
                        else
                        {
                            storeTotalPlan = _allocProfile.GetStoreQtyAllocated(_targetComponent, aStore.Key);
                        }
                        // begin MID track 4921 AnF#666 Fill to Size plan Enhancement
                        if (aUseBasisPlan)
                        {
                            storeTotalPlan +=
                                _sizeNeedResults.GetFillToPlan_SalesUnits(aStore.Key)
                                + _sizeNeedResults.GetFillToPlan_StockUnits(aStore.Key);
                        }
                        // end MID track 4921 AnF#666 Fill to Size plan Enhancement
                        //===================================
                        // Gather info for store total plan
                        //===================================
                        if (sizeColorPlan == eSizeNeedColorPlan.PlanForSpecificColorOnly)
                        {
                            ArrayList altSizeCodeList = null; // MID Track 4121
                            for (s = 0; s < sizeCodeCount; s++)
                            {
                                SizeCodeProfile sizeCode = (SizeCodeProfile)_sizeCodeList[s];
                                //ArrayList altSizeCodeList = null; // MID Track 4121
                                //===========================================================================
                                // Size Alternate Processing 
                                // sets up the size code list that used to retrieve the onHand and inTransit
                                // for the alternate sizes defined.
                                //===========================================================================
                                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                                altSizeCodeList = (ArrayList)_sizeAlternateHash[sizeCode.Key];
                                if (altSizeCodeList == null)
                                {
                                    altSizeCodeList = new ArrayList();
                                    altSizeCodeList.Add(sizeCode);
                                }
                                //if (_methodType == eMethodType.SizeNeedAllocation ||
                                //    _methodType == eMethodType.FillSizeHolesAllocation // MID Track 4291 add fill variables to size review
                                //    || aCalculateFillPlan) // MID Track 4291 add fill variables to size review
                                //{
                                //    // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
                                //    //if (this._sizeAlternateHash.ContainsKey(sizeCode.Key))
                                //    //{
                                //    //	altSizeCodeList = (ArrayList)_sizeAlternateHash[sizeCode.Key];		
                                //    //}
                                //    //	// Else use the primary size code only
                                //    //else
                                //    //{
                                //    //	altSizeCodeList = new ArrayList();
                                //    //	altSizeCodeList.Add(sizeCode);
                                //    //}
                                //    altSizeCodeList = (ArrayList)_sizeAlternateHash[sizeCode.Key];
                                //    if (altSizeCodeList == null)
                                //    {
                                //        altSizeCodeList = new ArrayList();
                                //        altSizeCodeList.Add(sizeCode);
                                //    }
                                //    // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
                                //}
                                //else
                                //{
                                //    altSizeCodeList = new ArrayList();
                                //    altSizeCodeList.Add(sizeCode);
                                //}
                                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                                // begin MID track 4921 AnF#666 Fill to Size plan Enhancement
                                if (!aUseBasisPlan)
                                {
                                    // end MID track 4921 AnF#666 Fill to Size plan Enhancement
                                    foreach (SizeCodeProfile altSizeCode in altSizeCodeList) // MID Track 4121
                                    {
                                        // begin removed unnecessary comments
                                        int onhandUnits = _sizeNeedResults.GetOnhandUnits(aStore.Key, altSizeCode.Key);  // MID Track 4121
                                        if (onhandUnits < 0)
                                        {
                                            storeTotalPlan += _sizeNeedResults.GetIntransitUnits(aStore.Key, altSizeCode.Key); // MID Track 4121
                                        }
                                        else
                                        {
                                            storeTotalPlan +=
                                                onhandUnits
                                                + _sizeNeedResults.GetIntransitUnits(aStore.Key, altSizeCode.Key); // MID Track 4121
                                        }
                                        storeTotalPlan += _sizeNeedResults.GetVswOnhandUnits(aStore.Key, altSizeCode.Key); // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand

                                        // Begin TT#5026 - JSmith - Question about Size Alternates
                                        //Debug.WriteLine(
                                        //    "Store:" + aStore
                                        //    + "; Size:" + altSizeCode.SizeCodePrimary
                                        //    + "; OH:" + _sizeNeedResults.GetOnhandUnits(aStore.Key, altSizeCode.Key)
                                        //    + "; IT:" + _sizeNeedResults.GetIntransitUnits(aStore.Key, altSizeCode.Key)
                                        //    + "; OH+IT:" + (_sizeNeedResults.GetOnhandUnits(aStore.Key, altSizeCode.Key) + _sizeNeedResults.GetIntransitUnits(aStore.Key, altSizeCode.Key))
                                        //    + "; VSWOH:" + _sizeNeedResults.GetVswOnhandUnits(aStore.Key, altSizeCode.Key)
                                        //    );
                                        // End TT#5026 - JSmith - Question about Size Alternates
                                    } // end removed unnecessary comments 
                                }  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement

                                //} // MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
                                // Units allocated should be adjusted to the modified curve just like OH and IT
                                //=====================
                                // get units allocated
                                //=====================
                                // begin MID Track 4291 add fill variables to size review
                                // begin TT#1600 - JEllis - Size Need Algorithm Error
                                foreach (SizeCodeProfile altSizeCode in altSizeCodeList) // MID Track 4121
                                {
                                    storeTotalPlan += _sizeNeedResults.GetPriorAllocatedUnits(aStore.Key, altSizeCode.Key);
                                    // Begin TT#1826-MD - JSmith - After run Size Need, Size Results for 2 headers same style color Qty allocted by size is  incorrect.
                                    //storeTotalPlan += _sizeNeedResults.GetGroupMemberAllocatedUnits(aStore.Key, altSizeCode.Key); // TT#1176 - MD - Jellis - Group ALlocation Size Need not observing inv min max
                                    // End TT#1826-MD - JSmith - After run Size Need, Size Results for 2 headers same style color Qty allocted by size is  incorrect.
                                }
                                //if (_sizeNeedResults.AccumulatePriorAllocated)
                                //{
                                //    // end MID Track 4291 add fill variables to size review
                                //    int allocatedUnits = 0;
                                //    int colorSizeUnits = 0; // TT#1410 - FL Pack Allocation Not giving enough
                                //    if (_targetComponent.ComponentType == eComponentType.SpecificColor)
                                //    {
                                //        // begin MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
                                //        //if (_color.ColorSizes.ContainsKey(sizeCode.Key))
                                //        //	allocatedUnits += _allocProfile.GetStoreQtyAllocated(_colorRid, sizeCode.Key, aStore.Key);
                                //        if (_color.ColorSizes.ContainsKey(sizeCode.Key))
                                //        {
                                //            allocatedUnits += _allocProfile.GetStoreQtyAllocated(_color, sizeCode.Key, storeIdxRID); // 3786 Change Fill Size Holes Algorithm
                                //        }
                                //        // begin removed unnecessary comments
                                //    }
                                //    else if (_targetComponent.ComponentType == eComponentType.DetailType)
                                //    {
                                //        // begin TT#1410 - FL Pack Allocation Not giving enough
                                //        foreach (PackHdr ph in _allocProfile.GenericPacks.Values)
                                //        {
                                //            foreach (PackColorSize pcs in ph.PackColors.Values)
                                //            {
                                //                if (pcs.ColorCodeRID == _basisColorRid
                                //                    || _basisColorRid == Include.DummyColorRID)
                                //                {
                                //                    if (pcs.SizeIsInColor(sizeCode.Key))
                                //                    {
                                //                        colorSizeUnits =
                                //                            ph.GetStorePacksAllocated(storeIdxRID.Index)
                                //                            * pcs.GetSizeBin(sizeCode.Key).ContentUnits;
                                //                        allocatedUnits += colorSizeUnits;
                                //                        storeTotalPlan += colorSizeUnits;
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        // end TT#1410 - FL Pack Allocation Not giving enough
                                //        //foreach (PackHdr ph in _allocProfile.Packs.Values) // MID Track 3749  // TT#1410 - FL Pack Allocation Not giving enough
                                //        foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values) // TT#1410 - FL Pack Allocation Not giving enough
                                //        {
                                //            // end removed unnecessary comments
                                //            foreach (PackColorSize pcs in ph.PackColors.Values)
                                //            {
                                //                if (pcs.ColorCodeRID == _basisColorRid  // MID Track 3749 include pack if basis color in pack
                                //                    || _basisColorRid == Include.DummyColorRID) // MID Track 3749 include pack if basis is all colors
                                //                {
                                //                    if (pcs.SizeIsInColor(sizeCode.Key))
                                //                    {
                                //                        allocatedUnits +=
                                //                            ph.GetStorePacksAllocated(storeIdxRID.Index)
                                //                            * pcs.GetSizeBin(sizeCode.Key).ContentUnits;
                                //                    }
                                //                }
                                //                // end MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        throw new Exception("component type not color or detail type");
                                //    }
                                //    // begin TT#1600 - JEllis - Size Need Algorithm Error
                                //    //_sizeNeedResults.AddPriorAllocatedUnits(aStore.Key, sizeCode.Key, allocatedUnits);
                                //    _sizeNeedResults.AddAllocatedUnits(aStore.Key, sizeCode.Key, allocatedUnits);
                                //    // end TT#1600 - JEllis - Size Need Algorithm Error
                                //    accumulatePriorAllocated = false;  // MID track 4291 add fill variables to size review
                                //}
                                // end TT#1600 - JEllis - Size Need Algorithm Error
                            } // MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
                        }
                        else if (sizeColorPlan == eSizeNeedColorPlan.PlanAcrossAllColors)
                        {


                        }

                        //======================
                        // Calculate Plan Units
                        //======================
                        // remove the units already allocated to sizes
                        // begin MID track 4291 add fill variables to size review
                        if (aCalculateFillPlan)
                        {
                            // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                            if (aUseBasisPlan)
                            {
                                _sizeNeedResults.AddFillToPlan_SzTotUnitPlan(aStore.Key, storeTotalPlan);
                            }
                            else
                            {
                                _sizeNeedResults.AddFillToOwn_SzTotUnitPlan(aStore.Key, storeTotalPlan);
                            }
                            // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        }
                        else
                        {
                            _sizeNeedResults.AddTotalPlanUnits(aStore.Key, storeTotalPlan);
                        }
                        this.CalculateStoreSizePlanWithSizeMins(aStore.Key, storeTotalPlan, _sizeCodeList, aCalculateFillPlan, aUseBasisPlan); // MID track 4291 add fill variables to size review //TT#848-MD -jsobek -Fill to Size Plan Presentation
                        // end MID track 4291 add fill variables to size review
                        // removed unnecessary comments
                    }
                }
                _sizeNeedResults.AccumulatePriorAllocated = accumulatePriorAllocated; // MID Track 4291 add fill variables to size review
            }
            catch
            {
                throw;
            }
            finally
            {
                //_timer.Stop("Calc PLAN");
            }
        }
        


        private void ProcessSizeRule()  // MID Track 3619 Remove Fringe
        {
            try
            {
                // removed unnecessary comments
                SizeRuleAlgorithm sizeRuleAlgorithm = new SizeRuleAlgorithm(_sizeRuleList,  // MID Track 3619 Remove Fringe 
                    //_storeList, // MID Track 3631 Size Rules apply to all sizes on header
                    _ruleStoreList, // MID Track 3631 Size Rules apply to all sizes on header
                    _headerColorRid,  // MID Track 3749
                    _allocProfile,
                    _constraintDecoder,
                    _rulesDecoder,
                    _rulesStoreGroupLevelHash,
                    _sizeNeedResults,
                    _sizeAlternateHash);  // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating

                sizeRuleAlgorithm.Process(); // MID Track 3619 Remove Fringe
            }
            catch
            {
                throw;
            }
            finally
            {
            }

        }

        /// <summary>
        /// calculates Size Need.  All results are placed in _sizeNeedResults.
        /// </summary>
        /// <param name="sizeColorPlan"></param>
        private void CalculateSizeNeed(eSizeNeedColorPlan sizeColorPlan)
        {
            try
            {
                //=================
                // Process Stores
                //=================
                int storeCount = _curveStoreList.Count;  // MID Track 3631 Size Rules apply to all sizes on header
                for (int i = 0; i < storeCount; i++)
                {
                    int sizeCodeCount = 0;
                    int s = 0;
                    int sizeTotalAllocated = 0;

                    StoreProfile aStore = (StoreProfile)_curveStoreList[i]; // MID Track 3631 Size Rules apply to all sizes on header
                    Index_RID storeIndex = (Index_RID)_allocProfile.StoreIndex(aStore.Key);


                    //===========================================================
                    // This method sets the value of 
                    // _sizeCodeList & _sizeCurve
                    // for the store
                    //===========================================================
                    sizeCodeCount = SetStoreSizeCodeList(aStore.Key);

                    //========================================
                    // Create Need Algorithm object for store
                    //========================================
                    if (_needAlgorithm == null)
                        _needAlgorithm = new NeedAlgorithms((ApplicationSessionTransaction)_transaction, sizeCodeCount, "Sizes");
                    else if (_sizeCurve.SizeCodeList.Count != _needAlgorithm.NomineeDimension)
                        _needAlgorithm = new NeedAlgorithms((ApplicationSessionTransaction)_transaction, sizeCodeCount, "Sizes");
                    NodeComponent aNodeComponent = _needAlgorithm.GetNodeComponent();
                    //				aNodeComponent.NodeMultiple = _color.ColorMultiple;
                    //				aNodeComponent.NodeID = _color.ColorKey;
                    aNodeComponent.NodeMultiple = this._allocationMultiple; 
                    aNodeComponent.NodeID = this._headerColorRid;  // MID Track 3749
                    //aNodeComponent.NodeType = eAllocationNode.BulkColor;   // TT#488 - MD - JEllis - Group Allocation
                    aNodeComponent.NodeType = eNeedAllocationNode.BulkColor; // TT#488 - MD - JEllis - Group Allocation
                    //				aNodeComponent.NodeUnitsToAllocate = _allocProfile.GetStoreQtyAllocated(_colorRid, aStore.Key);
                    // begin TT#1600 - JEllis - Size Need Algorithm Error
                    //aNodeComponent.NodeUnitsToAllocate = _allocProfile.GetStoreQtyAllocated(_targetComponent, storeIndex);
                    aNodeComponent.NodeUnitsToAllocate =
                        Math.Max(0,     // TT#1067 - MD - Jellis - Qty To Allocate Cannot be Negative  // TT#3819 - Urban - Jellis - Qty to Allocate Cannot be Neg
                        _allocProfile.GetStoreQtyAllocated(_targetComponent, storeIndex)
                        - _sizeNeedResults.GetStoreSizeNotInCurveTotalAllocated(storeIndex.RID)); // TT#1067 - MD - Jellis - Qty To ALlocate Cannot Be Negative   // TT#3819 - Urban - Jellis - Qty to Allocate Cannot be Neg
                    // end TT#1600 - JEllis - Size Need Algorithm Error

                    //_sizeNeedResults.AddInitialPlanUnits(aStore.Key, _sizeNeedResults.GetInitialPlanUnits(aStore.Key));

                    _needAlgorithm.PercentNeedLimit = 0;

                    //================================
                    // Fill need algorithm parameters
                    //================================
                    if (sizeColorPlan == eSizeNeedColorPlan.PlanForSpecificColorOnly)
                    {
                        for (s = 0; s < sizeCodeCount; s++)
                        {
                            _needAlgorithm.SetNomineePercentNeed(s, 0.0);
                            SizeCodeProfile sizeCode = (SizeCodeProfile)_sizeCodeList[s];

                            // Begin TT#5026 - JSmith - Question about Size Alternates
                            ArrayList altSizeCodeList = null;
                            altSizeCodeList = (ArrayList)_sizeAlternateHash[sizeCode.Key];
                            if (altSizeCodeList == null)
                            {
                                altSizeCodeList = new ArrayList();
                                altSizeCodeList.Add(sizeCode);
                            }
                            // End TT#5026 - JSmith - Question about Size Alternates

                            //==============
                            // onHand
                            //==============
                            //						IntransitKeyType iKt = new IntransitKeyType(_colorRid, sizeCode.Key);
                            //						int onHandUnits = _allocProfile.GetStoreOnHand(iKt, aStore.Key);
                            // Begin TT#5026 - JSmith - Question about Size Alternates
                            //int onHandUnits = _sizeNeedResults.GetOnhandUnits(aStore.Key, sizeCode.Key);
                            //onHandUnits += _sizeNeedResults.GetVswOnhandUnits(aStore.Key, sizeCode.Key); // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW onhand

                            int onHandUnits = 0;
                            foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                            {
                                onHandUnits += _sizeNeedResults.GetOnhandUnits(aStore.Key, altSizeCode.Key);
                                onHandUnits += _sizeNeedResults.GetVswOnhandUnits(aStore.Key, altSizeCode.Key);
                            }
                            // End TT#5026 - JSmith - Question about Size Alternates
                            _needAlgorithm.SetNomineeOnHand(s, (double)onHandUnits);

                            //============
                            // intransit
                            //============
                            //						IntransitKeyType [] iKtArray = new IntransitKeyType[1];
                            //						iKtArray[0] = iKt;
                            //						int intransitUnits = _transaction.GetStoreInTransit(_allocProfile.OnHandHnRID, iKtArray, aStore.Key);
                            // Begin TT#5026 - JSmith - Question about Size Alternates
                            //int intransitUnits = _sizeNeedResults.GetIntransitUnits(aStore.Key, sizeCode.Key);

                            int intransitUnits = 0;
                            foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                            {
                                intransitUnits += _sizeNeedResults.GetIntransitUnits(aStore.Key, altSizeCode.Key);
                            }
                            // End TT#5026 - JSmith - Question about Size Alternates
                            _needAlgorithm.SetNomineeInTransit(s, intransitUnits);

                            //=====================
                            // get units allocated
                            //=====================
                            //						int allocatedUnits = _allocProfile.GetStoreQtyAllocated(_colorRid, sizeCode.Key, aStore.Key);
                            //						_sizeNeedResults.AddPriorAllocatedUnits(aStore.Key, sizeCode.Key, allocatedUnits);
                            // begin TT#1600 - JEllis - Size Need Algorithm Error
                            //int allocatedUnits = _sizeNeedResults.GetPriorAllocatedUnits(aStore.Key, sizeCode.Key);
                            int allocatedUnits = _sizeNeedResults.GetAllocatedUnits(aStore.Key, sizeCode.Key);
                            // end TT#1600 - JEllis - Size Need Algorithm Error
                            sizeTotalAllocated += allocatedUnits;
                            _needAlgorithm.SetNomineeUnitsAllocated(s, allocatedUnits);
                            _needAlgorithm.SetNomineeGroupUnitsAlreadyAllocated(s, _sizeNeedResults.GetGroupMemberAllocatedUnits(aStore.Key, sizeCode.Key));
                            _needAlgorithm.SetNomineeGroupBulkUnitsAlreadyAllocated(s, _sizeNeedResults.GetGroupMemberAllocatedBulkUnits(aStore.Key, sizeCode.Key));  // TT#1828 - MD - JSmith - Size Need not allocatde to size
                            //================
                            // Get plan units
                            //================
                            int planUnits = _sizeNeedResults.GetSizeNeed_PlanUnits(aStore.Key, sizeCode.Key);  // MID Track 4921 Anf#666 Fill to Size Plan Enhancement
                            _needAlgorithm.SetNomineePlan(s, planUnits);

                            //==============================================
                            // Set Min/Max/Mult constraints
                            // This is not done for packs: dummy color rid
                            //==============================================
                            //if (_headerColorRid != Include.DummyColorRID) // MID Track 3749  // TT#3112 - MD - Jellis - Size Pack Allocation Overallocates Sizes
                            if (_targetComponent.ComponentType != eComponentType.DetailType)   // TT#3112 - MD - Jellis - Size Pack Allocation Overallocates Sizes
                            {
                                int intransitPlusOnhand =  // MID Track 3620 Size Minimums/maximums are "up to" 
                                    (int)_needAlgorithm.GetNomineeInTransit(s)  // MID Track 3620 Size Minimums/maximums are "up to" 
                                    + (int)_needAlgorithm.GetNomineeOnHand(s);         // MID Track 3620 Size Minimums/maximums are "up to"
                                // begin MID Track 4861 Size Curve Normalization
                                HdrColorBin hcb = _allocProfile.GetHdrColorBin(_headerColorRid);
                                // begin MID track 5061 Size Review Store Size Total wrong
                                //if (hcb.SizeIsInColor(sizeCode.Key))
                                HdrSizeBin hsb = (HdrSizeBin)hcb.ColorSizes[sizeCode.Key];
                                if (hsb != null)
                                // end MID Track 5061 Size Review Store Size Total wrong
                                {
                                    // end MID Track 4861 Size Curve Normalization//int mult = _sizeNeedResults.GetStoreMult(aStore.Key, sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly
                                    // begin MID track 5061 Size Review Store Size Total wrong
									//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    if ((!_allocProfile.WorkUpTotalBuy && !_allocProfile.Placeholder)
                                        && hsb.SizeUnitsToAllocate == 0)
									//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                                    {
                                        planUnits = 0;
                                        _needAlgorithm.SetNomineePlan(s, planUnits);
                                    }
                                    // end MID Track 5061 Size Review Store Size Total wrong
                                    int mult = _sizeNeedResults.GetStoreMult(aStore.Key, sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                                    // begin TT#1478 - Size Multiple Broken
                                    if (mult > 0)
                                    {
                                        aNodeComponent.SetNomineeMultiple(s, mult);
                                    }
                                    //if (mult > 0)
                                    //    aNodeComponent.NodeMultiple = mult;
                                    // begin TT#1543 - JEllis - Size Mult Broken
                                    // begin TT#519 - MD - Jellis - AnF VSW - Minimums not working
                                    //_needAlgorithm.SetNomineeMinimum(s, _sizeNeedResults.GetStoreInventoryMin(aStore.Key, sizeCode.Key));
                                    if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None         // TT#693 - MD - Jellis - VSW stores not holding minimums on balance with constraints.
                                        || _allocProfile.GetStoreImoMaxValue(aStore.Key) == int.MaxValue)  // TT#693 - MD - Jellis - VSW stores not holding minimums on balance with constraints.

                                    {
                                        // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
										//_needAlgorithm.SetNomineeMinimum(s, _sizeNeedResults.GetStoreInventoryMin(aStore.Key, sizeCode.Key, true)); // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
										_needAlgorithm.SetNomineeMinimum(s, _sizeNeedResults.GetStoreInventoryMin(aStore.Key, sizeCode.Key, true, altSizeCodeList));
										// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                                    }
                                    else
                                    {
                                        _needAlgorithm.SetNomineeMinimum(s, _allocProfile.GetStoreItemMinimum(hsb, _allocProfile.AppSessionTransaction.StoreIndexRID(aStore.Key)));
                                    }
                                    // end TT#519 - MD - Jellis - AnF VSW - Minimums not working
                                    // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
									//                                    _needAlgorithm.SetNomineeMaximum(s, _sizeNeedResults.GetStoreInventoryMax(aStore.Key, sizeCode.Key, true)); // TT#1176 - MD - Jellis - Size Need not observing Inventory Min Max on Group
									_needAlgorithm.SetNomineeMaximum(s, _sizeNeedResults.GetStoreInventoryMax(aStore.Key, sizeCode.Key, true, altSizeCodeList));
									// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                                    //int min = _sizeNeedResults.GetStoreMin(aStore.Key, sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                                    //// begin MID Track 3620 Size Minimums/Maximums are "up to" values
                                    ////_needAlgorithm.SetNomineeMinimum(s, min);
                                    ////int max = _sizeNeedResults.GetStoreMax(aStore.Key, sizeCode); // MID Track 3492 Size Need with constraints not allocating correctly
                                    ////_needAlgorithm.SetNomineeMaximum(s, max);
                                    //if (min > intransitPlusOnhand)
                                    //{
                                    //    _needAlgorithm.SetNomineeMinimum(s, min - intransitPlusOnhand);
                                    //}
                                    //else
                                    //{
                                    //    _needAlgorithm.SetNomineeMinimum(s, 0);
                                    //}
                                    //int max = _sizeNeedResults.GetStoreMax(aStore.Key, sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                                    //if (max > intransitPlusOnhand)
                                    //{
                                    //    if (max < int.MaxValue)
                                    //    {
                                    //        _needAlgorithm.SetNomineeMaximum(s, max - intransitPlusOnhand);
                                    //    }
                                    //    else
                                    //    {
                                    //        _needAlgorithm.SetNomineeMaximum(s, max);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    _needAlgorithm.SetNomineeMaximum(s, 0);
                                    //}
                                    //// end MID Track 3620 Size Minimums/Maximums are "up to" values
                                    // end TT#1543 - JEllis - Size Mult Broken
                                }
                                // end MID Track 4861 Size Curve Normalization
                            }

                            //==============================================================
                            // Set Rules constraints  (OUT/EXCLUDE rules only at this time)
                            //==============================================================
                            //if (IsStoreExcluded(aStore.Key, _colorRid , sizeCode.Key)) // MID Track 3492 Size Need with constraints not allocating correctly
                            if (IsStoreExcluded(aStore.Key, sizeCode)) // MID Track 3492 Size Need with constraints not allocating correctly
                            {
                                _needAlgorithm.SetNomineeIsOut(s, true);
                            }
                            else
                            {
                                _needAlgorithm.SetNomineeIsOut(s, false);
                            }

                            // Begin TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
                            //Debug.WriteLine("Calculate Size Need:aStore.Key=" + aStore.Key + ",sizeCode.Key=" + sizeCode.Key + ",onHandUnits=" + onHandUnits + ",intransitUnits=" + intransitUnits + ",allocatedUnits=" + allocatedUnits + ",planUnits=" + planUnits
                            //    + ",Minimum=" + _needAlgorithm.GetNomineeMinimum(s) + ",Maximmum=" + _needAlgorithm.GetNomineeMaximum(s));
							// End TT#4633 - JSmith - GA-Cancel Size Allocation-> Size Need Method again-> Style quantity changes
                        }
                    }
                    else if (sizeColorPlan == eSizeNeedColorPlan.PlanAcrossAllColors)
                    {


                    }

                    aNodeComponent.NodeUnitsAllocated = sizeTotalAllocated;

                    //==================
                    // calc SIZE NEED
                    //==================
                    // begin MID Track 3810 Size Allocation GT Style Allocation
                    eWorkUpBuyAllocationType workUpAllocationBuy = eWorkUpBuyAllocationType.NotWorkUpAllocationBuy;
					//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
					if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
					//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                    {
                        workUpAllocationBuy = eWorkUpBuyAllocationType.WorkUpBulkSizeAllocationBuy;  // MID Track 3810 Size Allocation GT Style Allocation
                    }
                    Debug.WriteLine("CalculateSizeNeed:AllocateByNeed for store " + aStore + " (" + aStore.Key + ")");
                    _needAlgorithm.AllocateByNeed(false, false, true, true, false, workUpAllocationBuy, true); // MID Track 3786 Change Fill Size Holes Algorithm // TT#1478 - Size Multiple Broken
                    // end MID Track 3810 Size Allocation GT Style ALlocation

                    //==================================
                    // place results in SizeNeedResult
                    //==================================
                    for (s = 0; s < sizeCodeCount; s++)
                    {
                        SizeCodeProfile sizeCode = (SizeCodeProfile)_sizeCodeList[s];
                        int unitsAllocated = _needAlgorithm.GetNomineeUnitsAllocated(s);
                        _sizeNeedResults.SetAllocatedUnits(aStore.Key, sizeCode.Key, unitsAllocated);
                    }
                    //============
                    // Debug NEED
                    //============
                    //				_sizeNeedResults.DebugStore(aStore.Key);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                //_timer.Stop("Calc SIZE NEED");
            }
        }

        /// <summary>
        /// calculates Fill Size Holes.  All results are placed in _sizeNeedResults*.
        /// The Store and size arrays line up to product distinct store/size keys.
        /// 
        /// </summary>
        /// <param name="availableUnits">Units available for the fill size holes allocation</param>
        /// <param name="aSizeStoresHash">Hashtable whose keys are the size code profiles. Each size code profile is associated with an arraylist of stores having positive size needs (holes) within the size code key.</param>
        /// <param name="sizeColorPlan"></param>
        //private void CalculateFillSizeHolesAllocation(int availableUnits, ArrayList storeList, ArrayList sizeList, eSizeNeedColorPlan sizeColorPlan) // MID Track 4921 AnF#666 Fill to Size Plan enhancement
        private void CalculateFillSizeHolesAllocation(int availableUnits, Hashtable aSizeStoresHash, eSizeNeedColorPlan sizeColorPlan) //MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            try
            {
                // begin MID Track 4921 AnF#666 Fill to Size Plan ENhancement
                ArrayList storeList = new ArrayList();
                ArrayList sizeList = new ArrayList();
                foreach (SizeCodeProfile sizeCode in aSizeStoresHash.Keys)
                {
                    int k = storeList.Count;
                    storeList.AddRange((ArrayList)aSizeStoresHash[sizeCode]);
                    for (int j = k; j < storeList.Count; j++)
                    {
                        sizeList.Add(sizeCode);
                    }
                }
                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                int s = 0;
                int sizeTotalAllocated = 0;

                //==============================================
                // Create Need Algorithm object for store/sizes
                //==============================================
                _needAlgorithm = new NeedAlgorithms((ApplicationSessionTransaction)_transaction, storeList.Count, "Stores");
                // begin BACK OUT NEED Pack Rounding
                //// BEGIN TT#616 - STodd - pack rounding
                //_needAlgorithm.Stores = storeList;
                //// END TT#616 - STodd - pack rounding
                // end BACK OUT NEED Pack Rounding
                NodeComponent aNodeComponent = _needAlgorithm.GetNodeComponent();

				// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes 
                if (_targetComponent.ComponentType == eComponentType.DetailType)
                {
                    aNodeComponent.NodeMultiple = _allocationMultiple;
                    aNodeComponent.NodeID = _headerColorRid;
                }
                else
                {
                    aNodeComponent.NodeMultiple = _color.ColorMultiple;
                    aNodeComponent.NodeID = _color.ColorCodeRID;
                }
				// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                //aNodeComponent.NodeType = eAllocationNode.BulkColor;   // TT#488 - MD - JEllis - Group Allocation
                aNodeComponent.NodeType = eNeedAllocationNode.BulkColor; // TT#488 - MD - JEllis - Group Allocation
                //_sizeNeedResults.AddInitialPlanUnits(aStore.Key, _sizeNeedResults.GetInitialPlanUnits(aStore.Key));

                _needAlgorithm.PercentNeedLimit = 0;
                //=========================================================================================
                // This gathers the available units by color/size and does two things:
                // 1) it creates the UnitsAvaialbleConstraint array to be assigned to the NeedAlgorithm.
                // 2) Since the above constraints is a list in no particular order, we build a hashtable
                //		that links the size RID to the index in the constraint list.
                //=========================================================================================
                Hashtable htSizeToConstraintIndex = new Hashtable();
				// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes 
                int[] UnitsConstraintList = null;
                if (_targetComponent.ComponentType == eComponentType.DetailType)
                {
                    //foreach (HdrSizeBin aSize in _color.ColorSizes.Values)
                    //{
                    //}
                    // foreach (KeyValuePair<int, PackSizeBin> entry in _packSizes)
                    //    {
                    //        PackSizeBin psb = entry.Value;

                    //UnitsConstraintList = new int[_packSizes.Count];
                    UnitsConstraintList = new int[_sizeNeedResults.Sizes.Count];
                }
                else
                {
                    UnitsConstraintList = new int[_color.ColorSizes.Count];
                }
				// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes

                int i = 0;
                // begin MID Track 3768 Change Fill Size Holes Algorithm
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    int sizeRID;  // MID Track 3882 Invalid Cast
                    foreach (SizeCodeProfile sizeProfile in sizeList) // MID Track 3882 Invalid Cast
                    {
                        sizeRID = sizeProfile.Key;  // MID Track 3882 Invalid Cast
                        if (!htSizeToConstraintIndex.ContainsKey(sizeRID))
                        {
                            UnitsConstraintList[i] = availableUnits;
                            htSizeToConstraintIndex.Add(sizeRID, i);
                            i++;
                        }
                    }
                }
                else
                {
					// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                    if (_targetComponent.ComponentType != eComponentType.DetailType)
                    {
					// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                        // end MID Track 3768 Change Fill Size Holes Algorithm
                        foreach (HdrSizeBin aSize in _color.ColorSizes.Values)
                        {
                            // begin MID Track 3430 Proportional Allocation Wrong when units already allocated to reserve
                            //int totalUnits = _allocProfile.GetRuleUnitsToAllocate(_colorRid, aSize.SizeKey);
                            //UnitsConstraintList[i] = totalUnits;
                            // begin MID Track 4425 Fill Size Holes Need Phase Overallocates size
                            //UnitsConstraintList[i] = _allocProfile.GetSizeUnitsToAllocate(aSize); 
                            UnitsConstraintList[i] =
                                Math.Max(0,
                                _allocProfile.GetAllocatedBalance(aSize) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(aSize.SizeCodeRID)); // Assortment: color/size changes
                            // end MID Track 4425 Fill Size Holes Need Phase Overallocates Size
                            // end MID Track 3430 Proportional Allocation Wrong when units already allocated to reserve
                            htSizeToConstraintIndex.Add(aSize.SizeCodeRID, i); // Assortment: color/size changes
                            i++;
                        }
					// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                    }
                    else
                    {
						// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                        //foreach (KeyValuePair<int, PackSizeBin> entry in _packSizes)
                        //{
                        //    PackSizeBin psb = entry.Value;
                        //    UnitsConstraintList[i] =
                        //        Math.Max(0,
                        //            //psb.ContentUnits - psb.SizeUnitsAllocated - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(psb.ContentCodeRID)); 
                        //            (psb.ContentUnits * ) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(psb.ContentCodeRID)); 

                        //    htSizeToConstraintIndex.Add(psb.ContentCodeRID, i); // Assortment: color/size changes
                        //    i++;
                        //}

                        //if (_color != null)
                        //{
                        //    foreach (HdrSizeBin aSize in _color.ColorSizes.Values)
                        //    {
                        //        if (!htSizeToConstraintIndex.ContainsKey(aSize.SizeKey))
                        //        {
                        //            UnitsConstraintList[i] =
                        //                Math.Max(0,
                        //                _allocProfile.GetAllocatedBalance(aSize) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(aSize.SizeCodeRID)); // Assortment: color/size changes
                        //            htSizeToConstraintIndex.Add(aSize.SizeCodeRID, i); // Assortment: color/size changes
                        //            i++;
                        //        }
                        //    }
                        //}
                        foreach (PackHdr ph in _allocProfile.NonGenericPacks.Values)
                        {
                            foreach (PackColorSize pcs in ph.PackColors.Values)
                            {
                                if (pcs.ColorCodeRID == _headerColorRid)
                                {
                                    foreach (PackSizeBin packSize in pcs.ColorSizes.Values)
                                    {
                                        if (!htSizeToConstraintIndex.ContainsKey(packSize.ContentCodeRID))
                                        {
                                            UnitsConstraintList[i] =
                                                Math.Max(0,
                                                //psb.ContentUnits - psb.SizeUnitsAllocated - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(psb.ContentCodeRID)); 
                                                    (packSize.ContentUnits * ph.PacksToAllocate) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(packSize.ContentCodeRID));

                                            htSizeToConstraintIndex.Add(packSize.ContentCodeRID, i); // Assortment: color/size changes
                                            i++;
                                        }
                                        else
                                        {
                                            int j = (int)htSizeToConstraintIndex[packSize.ContentCodeRID];
                                            UnitsConstraintList[j] +=
                                                Math.Max(0,
                                                //psb.ContentUnits - psb.SizeUnitsAllocated - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(psb.ContentCodeRID)); 
                                                    (packSize.ContentUnits * ph.PacksToAllocate) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(packSize.ContentCodeRID));

                                                //htSizeToConstraintIndex.Add(packSize.ContentCodeRID, i); // Assortment: color/size changes
                                        }
                                    }
                                }
                            }
                        }
						// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
					// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                    }
                } // MID Track 3768 Change Fill Size Holes Algorithm
                _needAlgorithm.UnitsAvailableConstraint = UnitsConstraintList;

                MIDGenericSortItem[] sortedStores = SortStoreSizes(storeList, sizeList);

                //================================
                // Fill need algorithm parameters
                //================================
                int storeRID;    // MID Track 3786 Change Fill Size Holes Algorithm
                int sizeCodeRID; // MID Track 3786 Change FIll Size Holes Algorithm
                SizeCodeProfile sizeCodeProfile; // MID Track 3786 Change Fill Size Holes Algorithm;
                if (sizeColorPlan == eSizeNeedColorPlan.PlanForSpecificColorOnly)
                {
                    // processes each store size in teh correct sorted order
                    for (i = 0; i < sortedStores.Length; i++)
                    {
                        MIDGenericSortItem mgsiStore = (MIDGenericSortItem)sortedStores[i];
                        // s is the index into the store and size lists
                        s = mgsiStore.Item;
                        storeRID = (int)storeList[s]; // MID Track 3786 Change FIll Size Holes Algorithm
                        sizeCodeProfile = (SizeCodeProfile)sizeList[s]; // MID Track 3786 Change Fill Size Holes Algorithm
                        sizeCodeRID = sizeCodeProfile.Key; // MID Track 3786 Change FIll Size Holes Algorithm
                        // Set Constraints
                        int constraintIndex = (int)htSizeToConstraintIndex[sizeCodeRID]; // MID Track 3615 Invalid Cast
                        aNodeComponent.SetNomineeUnitsAvaialbleIndex(i, constraintIndex);

                        // Begin TT#5026 - JSmith - Question about Size Alternates
                        ArrayList altSizeCodeList = null;
                        altSizeCodeList = (ArrayList)_sizeAlternateHash[sizeCodeProfile.Key];
                        if (altSizeCodeList == null)
                        {
                            altSizeCodeList = new ArrayList();
                            altSizeCodeList.Add(sizeCodeProfile);
                        }
                        // End TT#5026 - JSmith - Question about Size Alternates

                        //==============
                        // onHand
                        //==============
                        //					SizeCodeProfile sizeCode = (SizeCodeProfile)_sizeCodeList[s];
                        //					IntransitKeyType iKt = new IntransitKeyType(_colorRid, sizeList[s]);
                        //					int onHandUnits = _allocProfile.GetStoreOnHand(iKt, storeList[s]);
                        // Begin TT#5026 - JSmith - Question about Size Alternates
                        //int onHandUnits = _sizeNeedResults.GetOnhandUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast
                        //onHandUnits += _sizeNeedResults.GetVswOnhandUnits(storeRID, sizeCodeRID); // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW onhand

                        int onHandUnits = 0;
                        foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                        {
                            onHandUnits += _sizeNeedResults.GetOnhandUnits(storeRID, altSizeCode.Key);
                            onHandUnits += _sizeNeedResults.GetVswOnhandUnits(storeRID, altSizeCode.Key);
                        }

                        // End TT#5026 - JSmith - Question about Size Alternates
                        //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                        //    + " OH: " + onHandUnits);
                        _needAlgorithm.SetNomineeOnHand(i, (double)onHandUnits);

                        //============
                        // intransit
                        //============
                        //					IntransitKeyType [] iKtArray = new IntransitKeyType[1];
                        //					iKtArray[0] = iKt;
                        //					int intransitUnits = _transaction.GetStoreInTransit(_allocProfile.OnHandHnRID, iKtArray, aStore.Key);
                        // Begin TT#5026 - JSmith - Question about Size Alternates
                        //int intransitUnits = _sizeNeedResults.GetIntransitUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast
                        int intransitUnits = 0;
                        foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                        {
                            intransitUnits += _sizeNeedResults.GetIntransitUnits(storeRID, altSizeCode.Key);
                        }
                        // End TT#5026 - JSmith - Question about Size Alternates
                        //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                        //  + " IT: " + intransitUnits);
                        _needAlgorithm.SetNomineeInTransit(i, intransitUnits);

                        //=====================
                        // set units allocated
                        //=====================
                        //					int allocatedUnits = _allocProfile.GetStoreQtyAllocated(_colorRid, sizeCode.Key, aStore.Key);
                        //					_sizeNeedResults.AddPriorAllocatedUnits(aStore.Key, sizeCode.Key, allocatedUnits);
                        // begin TT#1600 - JEllis - Size Need Algorithm Error
                        //int allocatedUnits = _sizeNeedResults.GetPriorAllocatedUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast
                        int allocatedUnits = _sizeNeedResults.GetAllocatedUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast
                        // end TT#1600 - JEllis - Size Need Algorithm Error
                        sizeTotalAllocated += allocatedUnits ;

						// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
                        //int priorDetailAllocatedUnits = _sizeNeedResults.GetPriorDetailAllocatedUnits(storeRID, sizeCodeRID);
                        //allocatedUnits += priorDetailAllocatedUnits;
						// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

                        _needAlgorithm.SetNomineeUnitsAllocated(i, allocatedUnits);
                        
                        //================
                        // set plan units
                        //================
                        int planUnits = _sizeNeedResults.GetSizeNeed_PlanUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                        //    + " PLAN UNITS: " + planUnits);
                        _needAlgorithm.SetNomineePlan(i, planUnits);

                        //Debug.WriteLine("CalculateFillSizeHolesAllocation() ST RID: " + storeRID + " SZ CD RID: " + sizeCodeRID + " IT: " + intransitUnits
                        //    + " PRIOR DTL: " + priorDetailAllocatedUnits + " ALLOC: " + allocatedUnits + " PLAN: " + planUnits);

                        //==============================================
                        // Set Min/Max/Mult constraints
                        // This is not done for packs: dummy color rid
                        //==============================================
						// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                        //if (_headerColorRid != Include.DummyColorRID)  // MID track 3749
                        if (_targetComponent.ComponentType != eComponentType.DetailType)
						// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                        {
                            int intransitPlusOnhand =                  // MID Track 3620 Size Minmums/maximums are "up to" values
                                (int)_needAlgorithm.GetNomineeInTransit(i)  // MID Track 3620 Size Minimus/maximums are "up to" values
                                + (int)_needAlgorithm.GetNomineeOnHand(i);  // MID Track 3620 Size Minimus/maximums are "up to" values


                            int mult = _sizeNeedResults.GetStoreMult(storeRID, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
                            // begin TT#1478 - Size Multiple Broken
                            if (mult > 0)
                            {
                                aNodeComponent.SetNomineeMultiple(i, mult);
                            }
                            //if (mult > 0)
                            //    aNodeComponent.NodeMultiple = mult;
                            // end TT#1478 - Size Multiple Broken
                            // begin TT#1543 - JELlis - Size Mult Broken
                            // begin TT#519 - MD - Jellis - AnF VSW - Minimums not working
                            //_needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, sizeCodeProfile.Key));
                            if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None         // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints.
                                || _allocProfile.GetStoreImoMaxValue(storeRID) == int.MaxValue)  // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints.

                            {
                                // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
								// _needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, sizeCodeProfile.Key, true)); // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
								_needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, sizeCodeProfile.Key, true, altSizeCodeList));
								// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                            }
                            else
                            {
                                _needAlgorithm.SetNomineeMinimum(i, _allocProfile.GetStoreItemMinimum(_headerColorRid, sizeCodeProfile.Key, storeRID));
                            }
                            //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                            //    + " MIN: " + _needAlgorithm.GetNomineeMinimum(i));
                            // end TT#519 - MD - Jellis - AnF VSW - Minimums not working
							// Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
							// _needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryCapacityMax(storeRID, sizeCodeProfile.Key, true)); // TT#2155 - JEllis - Fill Size Holes Null Reference // TT#1176 - MD - Jellis - Size Need not observing Inventory Min Max on Group
                            _needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryCapacityMax(storeRID, sizeCodeProfile.Key, true, altSizeCodeList));
							// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                            //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                            //      + " MAX: " + _needAlgorithm.GetNomineeMaximum(i));
                            //_needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryMax(storeRID, sizeCodeProfile.Key));       // TT#2155 - JEllis - Fill Size Holes Null Reference
                            //int min = _sizeNeedResults.GetStoreMin(storeRID, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                            //// begin MID Track 3620 Size Minimums/Maximums are "up to" values
                            ////_needAlgorithm.SetNomineeMinimum(i, min);
                            //// int max = _sizeNeedResults.GetStoreMax((int)storeList[s], (SizeCodeProfile)sizeList[s]); // MID Track 3492 Size Need with constraints not allocating correctly
                            //// _needAlgorithm.SetNomineeMaximum(i, max);
                            //if (min > intransitPlusOnhand)
                            //{
                            //    _needAlgorithm.SetNomineeMinimum(i, min - intransitPlusOnhand);
                            //}
                            //else
                            //{
                            //    _needAlgorithm.SetNomineeMinimum(i, 0);
                            //}
                            //int max = _sizeNeedResults.GetStoreMax(storeRID, sizeCodeProfile.Key); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                            //if (max > intransitPlusOnhand)
                            //{
                            //    if (max < int.MaxValue)
                            //    {
                            //        _needAlgorithm.SetNomineeMaximum(i, max - intransitPlusOnhand);
                            //    }
                            //    else
                            //    {
                            //        _needAlgorithm.SetNomineeMaximum(i, max);
                            //    }
                            //}
                            //else
                            //{
                            //    _needAlgorithm.SetNomineeMaximum(i, 0);
                            //}
                            // end MID Track 3620 Size Minimums/Maximums are "up to" values
                            // end TT#1543 - JELlis - Size Mult Broken
                        }

                        //=============================================================
                        // Set Rules constraints  (only OUT/Exclude rule at this time)
                        //=============================================================
                        //if (IsStoreExcluded((int)storeList[s], _colorRid , (int)sizeList[s])) // MID Track 3492 Size Need with constraints not allocating correctly
                        if (IsStoreExcluded(storeRID, sizeCodeProfile))
                        {
                            _needAlgorithm.SetNomineeIsOut(i, true);
                        }
                        else
                        {
                            _needAlgorithm.SetNomineeIsOut(i, false);
                        }
                    }
                }
                else if (sizeColorPlan == eSizeNeedColorPlan.PlanAcrossAllColors)
                {


                }

                //aNodeComponent.NodeUnitsAllocated = sizeTotalAllocated;  // MID Track 4425 Fill Size Holes Need Phase overallocates 
                //  Prior units allocated is irrelevant for Fill Size Holes; Available units is not constrained by prior allocations including rules
                aNodeComponent.NodeUnitsToAllocate = availableUnits;
                //Debug.WriteLine("CalculateFillSizeHolesAllocation() AVAILABLE UNITS: " + availableUnits);

                //=======================
                // calc FILL SIZE HOLES
                //=======================
                // begin MID Track 3810 Size Allocation GT Style Allocation
                eWorkUpBuyAllocationType workUpAllocationBuy = eWorkUpBuyAllocationType.NotWorkUpAllocationBuy;
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    workUpAllocationBuy = eWorkUpBuyAllocationType.WorkUpBulkSizeAllocationBuy;
                }
                _needAlgorithm.AllocateByNeed(true, true, true, false, false, workUpAllocationBuy, true);  // MID Track 3790 Work Up Size Buy // TT#1478 - Size Multiple Broken
                // end MID Track 3810 Size Allocation GT Style Allocation

                //==================================
                // place results in SizeNeedResult
                //==================================
                //int total = 0;  // MID Track 4425 Fill Size Holes Need Phase Overallocates
                //int oldValue;     // MID Track 4425 Fill Size Holes Need Phase Overallocates  // TT#1600 - JEllis - Size Need Algorithm Error
                for (i = 0; i < sortedStores.Length; i++)
                {
                    MIDGenericSortItem mgsiStore = (MIDGenericSortItem)sortedStores[i];
                    // s is the index into the store and size lists
                    s = mgsiStore.Item;
                    storeRID = (int)storeList[s]; // MID Track 3786 Change Fill Size Holes Algorithm
                    sizeCodeRID = (int)((SizeCodeProfile)sizeList[s]).Key; // MID Track 3786 Change Fill Size Holes Algorithm
                    int unitsAllocated = _needAlgorithm.GetNomineeUnitsAllocated(i);
                    //Debug.WriteLine("CalculateFillSizeHolesAllocation() STORE: " + storeRID + " SIZE CODE RID: " + sizeCodeRID
                    //            + " UNITS ALLOCATED: " + unitsAllocated);

                    //total += unitsAllocated; // MID Track 4425 Fill Size Holes Need Phase Overallocates

                    //int oldValue = _sizeNeedResults.GetAllocatedUnits((int)storeList[s], (int)sizeList[s]); // MID Track 3615 Invalid Cast
                    // begin MID Track 4425 Fill Size Holes Need Phase Overallocates
                    // begin TT#1600 - JEllis - Size Need Algorithm Error
                    //oldValue = _sizeNeedResults.GetAllocatedUnits(storeRID, sizeCodeRID); // MID Track 3615 Invalid Cast
                    //unitsAllocated = unitsAllocated - oldValue;
                    // end TT#1600 - JEllis - Size Need Algorithm Error

                    // removed unnecessary comments for readability

                    //_sizeNeedResults.AddAllocatedUnits(storeRID, sizeCodeRID, unitsAllocated); // MID Track 3615 Invalid Cast

                    if (unitsAllocated > 0)
                    {
                        // begin TT#1600 - JEllis - Size Need Algorithm Error
                        //_sizeNeedResults.AddAllocatedUnits(storeRID, sizeCodeRID, unitsAllocated); // MID Track 3615 Invalid Cast
                        _sizeNeedResults.SetAllocatedUnits(storeRID, sizeCodeRID, unitsAllocated); 
                        // end TT#1600 - JEllis - Size Need Algorithm Error
						// Begin TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                        if (_targetComponent.ComponentType == eComponentType.DetailType)
                        {
                            _allocProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, storeRID, true);
                        }
                        else
                        {
                            _allocProfile.SetStoreAllocationFromBulkSizeBreakOut(_headerColorRid, storeRID, true); // MID Track 3749
                        }
						// End TT#1777-MD - stodd - Null Reference Exception - PrePack Fill to Size Holes
                    }
                    // end MID Track 4425 Fill Size Holes Need Phase Overallocates

                }
                //Debug.WriteLine(total.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                //_timer.Stop("Calc FILL SIZE HOLES");
            }
        }

        // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        /// <summary>
        /// calculates Fill Size to Plan.  All results are placed in _sizeNeedResults*.
        /// The Store and size arrays line up to product distinct store/size keys.
        /// </summary>
        /// <param name="aStoreList">Stores whose size need for the specified size code profile is positive </param>
        /// <param name="aSizeCodeProfile">Size Code Profile for which a Fill Size To Plan Allocation is desired</param>
        /// <param name="sizeColorPlan"></param>
        private void CalculateFillSizeToPlanAllocation(int availableUnits, ArrayList aStoreList, SizeCodeProfile aSizeCodeProfile, eSizeNeedColorPlan sizeColorPlan) // MID Track 4921 AnF#666 Fill to Size Plan enhancement
        {
            try
            {
                int sizeTotalAllocated = 0;

                //==============================================
                // Create Need Algorithm object for store/sizes
                //==============================================
                _needAlgorithm = new NeedAlgorithms((ApplicationSessionTransaction)_transaction, aStoreList.Count, "Stores");
                // begin BACK OUT Need Rounding
                //// BEGIN TT#616 - STodd - pack rounding
                //_needAlgorithm.Stores = aStoreList;
                //// END TT#616 - STodd - pack rounding
                // end BACK OUT Need Rounding
                NodeComponent aNodeComponent = _needAlgorithm.GetNodeComponent();

                // Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                //aNodeComponent.NodeMultiple = _color.ColorMultiple;
                //aNodeComponent.NodeID = _color.ColorCodeRID;
                aNodeComponent.NodeMultiple = _allocationMultiple;
                aNodeComponent.NodeID = _headerColorRid;
                // End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk

                //aNodeComponent.NodeType = eAllocationNode.BulkColor;   // TT#488 - MD - JEllis - Group Allocation
                aNodeComponent.NodeType = eNeedAllocationNode.BulkColor; // TT#488 - MD - JEllis - Group Allocation

                _needAlgorithm.PercentNeedLimit = 0;

                Hashtable htSizeToConstraintIndex = new Hashtable();
                int[] UnitsConstraintList = new int[1];

				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    UnitsConstraintList[0] = availableUnits;
                    htSizeToConstraintIndex.Add(aSizeCodeProfile.Key, 0);
                }
                else
                {
					// Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    if (_targetComponent.ComponentType != eComponentType.DetailType)
                    {
                        HdrSizeBin hsb = (HdrSizeBin)_color.ColorSizes[aSizeCodeProfile.Key];
                        // Begin TT#4989 - JSmith - Fill Size to plan wants to allocate 100% - leaves balance equal to reserve qty
                        //UnitsConstraintList[0] =
                        //    Math.Max(0,
                        //        _allocProfile.GetAllocatedBalance(hsb) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(hsb.SizeCodeRID)); // Assortment: color/size changes
                        // AllocatedBalance already has been reduced by TotalUnitsAllocated.  Subtracting again double dipped the allocated units.
                        UnitsConstraintList[0] =
                            Math.Max(0,
                                _allocProfile.GetAllocatedBalance(hsb));
                        // End TT#4989 - JSmith - Fill Size to plan wants to allocate 100% - leaves balance equal to reserve qty
                        htSizeToConstraintIndex.Add(hsb.SizeCodeRID, 0); // Assortment: color/size changes
                        availableUnits = Math.Min(availableUnits, UnitsConstraintList[0]);
                    }
                    else
                    {
                        // stodd - Possible code
                        //PackSizeBin psb = (PackSizeBin)_packSizes[aSizeCodeProfile.Key];
                        //UnitsConstraintList[0] =
                        //    Math.Max(0,
                        //        _allocProfile.GetAllocatedBalance(psb) - _sizeNeedResults.GetAllStoreTotalAllocatedUnits(psb.ContentCodeRID)); // Assortment: color/size changes
                        //htSizeToConstraintIndex.Add(psb.ContentCodeRID, 0); // Assortment: color/size changes
                        //availableUnits = Math.Min(availableUnits, UnitsConstraintList[0]);

                        UnitsConstraintList[0] = availableUnits;
                        htSizeToConstraintIndex.Add(aSizeCodeProfile.Key, 0);

                    }
					// End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                }
                _needAlgorithm.UnitsAvailableConstraint = UnitsConstraintList;

                // Begin TT#5026 - JSmith - Question about Size Alternates
                ArrayList altSizeCodeList = null;
                altSizeCodeList = (ArrayList)_sizeAlternateHash[aSizeCodeProfile.Key];
                if (altSizeCodeList == null)
                {
                    altSizeCodeList = new ArrayList();
                    SizeCodeProfile sizeCodeProfile = (SizeCodeProfile)_sizeCodeList.FindKey(aSizeCodeProfile.Key);
                    // Begin TT#5505 Jsmith-AGallagher - Fill Size to Plan Method not working for all Styles
                    if (sizeCodeProfile == null)
                    {
                        sizeCodeProfile = _transaction.SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeCodeProfile.Key);
                    }
                    // End TT#5505 Jsmith-AGallagher - Fill Size to Plan Method not working for all Styles
                    altSizeCodeList.Add(sizeCodeProfile);
                }
                // End TT#5026 - JSmith - Question about Size Alternates

                //================================
                // Fill need algorithm parameters
                //================================
                int storeRID;
                if (sizeColorPlan == eSizeNeedColorPlan.PlanForSpecificColorOnly)
                {
                    for (int i = 0; i < aStoreList.Count; i++)
                    {
                        storeRID = (int)aStoreList[i];
                        // Set Constraints
                        aNodeComponent.SetNomineeUnitsAvaialbleIndex(i, 0);
                        _needAlgorithm.SetNomineeHasAllocationPriority(i, _allocProfile.GetStoreAllocationPriority(storeRID));  // MID Track 5168 AnF Defect 1125 Store Allocation Priority not observed by Fill Size to Plan
                        //==============
                        // onHand
                        //==============
                        // Begin TT#5026 - JSmith - Question about Size Alternates
                        //int onHandUnits = _sizeNeedResults.GetOnhandUnits(storeRID, aSizeCodeProfile.Key);
                        //onHandUnits += _sizeNeedResults.GetVswOnhandUnits(storeRID, aSizeCodeProfile.Key); // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW onhand

                        int onHandUnits = 0;
                        foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                        {
                            onHandUnits += _sizeNeedResults.GetOnhandUnits(storeRID, altSizeCode.Key);
                            onHandUnits += _sizeNeedResults.GetVswOnhandUnits(storeRID, altSizeCode.Key);
                        }
                        // End TT#5026 - JSmith - Question about Size Alternates
                        _needAlgorithm.SetNomineeOnHand(i, (double)onHandUnits);

                        //============
                        // intransit
                        //============
                        // Begin TT#5026 - JSmith - Question about Size Alternates
                        //int intransitUnits = _sizeNeedResults.GetIntransitUnits(storeRID, aSizeCodeProfile.Key);
                        int intransitUnits = 0;
                        foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                        {
                            intransitUnits += _sizeNeedResults.GetIntransitUnits(storeRID, altSizeCode.Key);
                        }
                        // End TT#5026 - JSmith - Question about Size Alternates
                        _needAlgorithm.SetNomineeInTransit(i, intransitUnits);

                        //=====================
                        // set units allocated
                        //=====================
                        // begin TT#1600 - JEllis - Size Need Algorithm Error
                        //int allocatedUnits = _sizeNeedResults.GetPriorAllocatedUnits(storeRID, aSizeCodeProfile.Key);
                        int allocatedUnits = _sizeNeedResults.GetAllocatedUnits(storeRID, aSizeCodeProfile.Key);
                        // end TT#1600 - JEllis - Size Need Algorithm Error
                        sizeTotalAllocated += allocatedUnits;
                        _needAlgorithm.SetNomineeUnitsAllocated(i, allocatedUnits);

                        //================
                        // set plan units
                        //================
                        int planUnits = _sizeNeedResults.GetSizeNeed_PlanUnits(storeRID, aSizeCodeProfile.Key);
                        _needAlgorithm.SetNomineePlan(i, planUnits);

                        //==============================================
                        // Set Min/Max/Mult constraints
                        // This is not done for packs: dummy color rid
                        //==============================================
						// Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                        //if (_headerColorRid != Include.DummyColorRID)
                        if (_targetComponent.ComponentType != eComponentType.DetailType)
						// End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                        {
                            int intransitPlusOnhand =
                                (int)_needAlgorithm.GetNomineeInTransit(i)
                                + (int)_needAlgorithm.GetNomineeOnHand(i);
                            int mult = _sizeNeedResults.GetStoreMult(storeRID, aSizeCodeProfile.Key); // TT#1391 - TMW New Action
                            // begin TT#1478 - Size Multiple Broken
                            if (mult > 0)
                            {
                                aNodeComponent.SetNomineeMultiple(i, mult);
                            }
                            //if (mult > 0)
                            //    aNodeComponent.NodeMultiple = mult;
                            // end TT#1478 - Size Multiple Broken
                            // begin TT#1543 - JEllis - Size Multiple Broken
                            // begin TT#519 - MD - Jellis - AnF VSW - Minimums not working
                            //_needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, aSizeCodeProfile.Key));
                            if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None         // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints.
                                || _allocProfile.GetStoreImoMaxValue(storeRID) == int.MaxValue)  // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints.

                            {
                                // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
								// _needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, aSizeCodeProfile.Key, true)); // TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
							    _needAlgorithm.SetNomineeMinimum(i, _sizeNeedResults.GetStoreInventoryMin(storeRID, aSizeCodeProfile.Key, true, altSizeCodeList));
								// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                            }
                            else
                            {
                                _needAlgorithm.SetNomineeMinimum(i, _allocProfile.GetStoreItemMinimum(_headerColorRid, aSizeCodeProfile.Key, storeRID));
                            }
                            // end TT#519 - MD - Jellis - AnF VSW - Minimums not working
							// Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
							//_needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryCapacityMax(storeRID, aSizeCodeProfile.Key, true)); // TT#2155 - JEllis - Fill Size Holes Null Reference // TT#1176 - MD - Jellis - Size need not observing Inv Min Max on Group
                            _needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryCapacityMax(storeRID, aSizeCodeProfile.Key, true, altSizeCodeList));
							// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates

                            
                            //_needAlgorithm.SetNomineeMaximum(i, _sizeNeedResults.GetStoreInventoryMax(storeRID, aSizeCodeProfile.Key));       // TT#2155 - JEllis - Fill Size Holes Null Reference
                            //int min = _sizeNeedResults.GetStoreMin(storeRID, aSizeCodeProfile.Key); // TT#1391 - TMW New Action
                            //if (min > intransitPlusOnhand)
                            //{
                            //    _needAlgorithm.SetNomineeMinimum(i, min - intransitPlusOnhand);
                            //}
                            //else
                            //{
                            //    _needAlgorithm.SetNomineeMinimum(i, 0);
                            //}
                            //int max = _sizeNeedResults.GetStoreMax(storeRID, aSizeCodeProfile.Key); // TT#1391 - TMW New Action
                            //if (max > intransitPlusOnhand)
                            //{
                            //    if (max < int.MaxValue)
                            //    {
                            //        _needAlgorithm.SetNomineeMaximum(i, max - intransitPlusOnhand);
                            //    }
                            //    else
                            //    {
                            //        _needAlgorithm.SetNomineeMaximum(i, max);
                            //    }
                            //}
                            //else
                            //{
                            //    _needAlgorithm.SetNomineeMaximum(i, 0);
                            //}
                            // end TT#1543 - Jellis - Size Multiple Broken
                        }

                        //=============================================================
                        // Set Rules constraints  (only OUT/Exclude rule at this time)
                        //=============================================================
                        if (IsStoreExcluded(storeRID, aSizeCodeProfile))
                        {
                            _needAlgorithm.SetNomineeIsOut(i, true);
                        }
                        else
                        {
                            _needAlgorithm.SetNomineeIsOut(i, false);
                        }
                    }
                }
                else if (sizeColorPlan == eSizeNeedColorPlan.PlanAcrossAllColors)
                {


                }

                aNodeComponent.NodeUnitsToAllocate = availableUnits;
                //=======================
                // calc FILL SIZE HOLES
                //=======================
                eWorkUpBuyAllocationType workUpAllocationBuy = eWorkUpBuyAllocationType.NotWorkUpAllocationBuy;
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (_allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    workUpAllocationBuy = eWorkUpBuyAllocationType.WorkUpBulkSizeAllocationBuy;
                }
                _needAlgorithm.AllocateByNeed(true, false, true, true, false, workUpAllocationBuy, true); // TT#1478 - Size Multple Broken
                //==================================
                // place results in SizeNeedResult
                //==================================
                //int oldValue;  // TT#1600  - JEllis - Size Need Algorithm Error
                for (int i = 0; i < aStoreList.Count; i++)
                {
                    storeRID = (int)aStoreList[i];
                    int unitsAllocated = _needAlgorithm.GetNomineeUnitsAllocated(i);
                    // begin TT#1600 - JEllis - Size Need Algorithm Error
                    //oldValue = _sizeNeedResults.GetAllocatedUnits(storeRID, aSizeCodeProfile.Key);
                    //unitsAllocated = unitsAllocated - oldValue;
                    // end TT#1600 - JEllis - Size Need Algorithm Error

                    if (unitsAllocated > 0)
                    {
                        // begin TT#1600 - JEllis - Size Need Algorithm Error
                        //_sizeNeedResults.AddAllocatedUnits(storeRID, aSizeCodeProfile.Key, unitsAllocated);
                        _sizeNeedResults.SetAllocatedUnits(storeRID, aSizeCodeProfile.Key, unitsAllocated);
                        // end TT#1600 - JEllis - Size Need Algorithm Error
						// Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                        if (_targetComponent.ComponentType == eComponentType.DetailType)
                        {
                            _allocProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, storeRID, true);
                        }
                        else
                        {
                            _allocProfile.SetStoreAllocationFromBulkSizeBreakOut(_headerColorRid, storeRID, true);
                        }
						// End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
                    }

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

        /// <summary>
        /// Sort Sizes
        /// </summary>
        /// <param name="aHdrSizeBinList"></param>
        /// <returns></returns>
        private MIDGenericSortItem[] SortSizes(HdrSizeBin[] aHdrSizeBinList)
        {
            try
            {
                MIDGenericSortItem[] sortedSizeBin = new MIDGenericSortItem[aHdrSizeBinList.Length];
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
				if (this._allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                {
                    for (int s = 0; s < aHdrSizeBinList.Length; s++)
                    {
                        sortedSizeBin[s].Item = s;
                        sortedSizeBin[s].SortKey = new double[2];
						// Begin TT#4238 - JSmith - WUB Header - Fill Size Plan plus minimums is not allocating the Fill Forward Plan 
						//sortedSizeBin[s].SortKey[0] = 1;
                        sortedSizeBin[s].SortKey[0] = -1;
						// End TT#4238 - JSmith - WUB Header - Fill Size Plan plus minimums is not allocating the Fill Forward Plan 
                        sortedSizeBin[s].SortKey[1] = MIDMath.GetRandomDouble();
                    }
                }
                else
                {
                    for (int s = 0; s < aHdrSizeBinList.Length; s++)
                    {
                        HdrSizeBin aSizeBin = (HdrSizeBin)aHdrSizeBinList[s];
                        sortedSizeBin[s].Item = s;
                        sortedSizeBin[s].SortKey = new double[2];
                        sortedSizeBin[s].SortKey[0] = -Math.Max(0, aSizeBin.SizeUnitsToAllocate - aSizeBin.SizeUnitsAllocated); // sort ascending
                        sortedSizeBin[s].SortKey[1] = MIDMath.GetRandomDouble();
                    }
                }

                Array.Sort(sortedSizeBin, new MIDGenericSortDescendingComparer());

                return sortedSizeBin;
            }
            catch
            {
                throw;
            }

        }
        // end   MID Track 4921 AnF#666 Fill to Size Plan Enhancement

		// Begin TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk
        private MIDGenericSortItem[] SortPackSizes(PackSizeBin[] aHdrSizeBinList)
        {
            try
            {
                MIDGenericSortItem[] sortedSizeBin = new MIDGenericSortItem[aHdrSizeBinList.Length];
                if (this._allocProfile.WorkUpBulkSizeBuy || _allocProfile.Placeholder)
                {
                    for (int s = 0; s < aHdrSizeBinList.Length; s++)
                    {
                        sortedSizeBin[s].Item = s;
                        sortedSizeBin[s].SortKey = new double[2];
                        sortedSizeBin[s].SortKey[0] = -1;
                        sortedSizeBin[s].SortKey[1] = MIDMath.GetRandomDouble();
                    }
                }
                else
                {
                    for (int s = 0; s < aHdrSizeBinList.Length; s++)
                    {
                        PackSizeBin aSizeBin = (PackSizeBin)aHdrSizeBinList[s];
                        sortedSizeBin[s].Item = s;
                        sortedSizeBin[s].SortKey = new double[2];
                        sortedSizeBin[s].SortKey[0] = -Math.Max(0, aSizeBin.ContentUnits - aSizeBin.SizeUnitsAllocated); // sort ascending
                        sortedSizeBin[s].SortKey[1] = MIDMath.GetRandomDouble();
                    }
                }

                Array.Sort(sortedSizeBin, new MIDGenericSortDescendingComparer());

                return sortedSizeBin;
            }
            catch
            {
                throw;
            }

        }
		// End TT#1667-MD - stodd - Fill Size for Pack --- received MIDRetail.DataCommon.MIDException: 80036:Color not defined for bulk


        /// <summary>
        /// The Store and Size arrays line up to product distinct store/size keys.
        /// </summary>
        /// <param name="storeList"></param>
        /// <param name="sizeList"></param>
        /// <returns></returns>
        private MIDGenericSortItem[] SortStoreSizes(ArrayList storeList, ArrayList sizeList)
        {
            try
            {
                MIDGenericSortItem[] sortedStore = new MIDGenericSortItem[storeList.Count];
                for (int s = 0; s < storeList.Count; s++)
                {
                    int storeRid = (int)storeList[s];
                    // int sizeRid = (int)sizeList[s];  // MID Track 3615 Invalid Cast
                    int sizeRid = (int)((SizeCodeProfile)sizeList[s]).Key; // MID Track Invalid Cast
                    double need =
                        Need.UnitNeed(_sizeNeedResults.GetSizeNeed_PlanUnits(storeRid, sizeRid),  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                        //_sizeNeedResults.GetOnhandUnits(storeRid, sizeRid),                                                       // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand
                        _sizeNeedResults.GetOnhandUnits(storeRid, sizeRid) + _sizeNeedResults.GetVswOnhandUnits(storeRid, sizeRid), // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand
                        _sizeNeedResults.GetIntransitUnits(storeRid, sizeRid),
                        // begin TT#1600 - JEllis - Size Need Algorithm Error
                        _sizeNeedResults.GetAllocatedUnits(storeRid, sizeRid) // TT#1176 - MD - Jellis- Group Allocation Size Need not observing inv min max
                        + _sizeNeedResults.GetGroupMemberAllocatedUnits(storeRid, sizeRid)); // TT#1176 - MD- Jellis- Group Allocation Size Need not observing inv min max
                        //_sizeNeedResults.GetPriorAllocatedUnits(storeRid, sizeRid));
                        // end TT#1600 - JEllis - Size Need Algorithm Error
                    double needPercent =
                        Need.PctUnitNeed(need, _sizeNeedResults.GetSizeNeed_PlanUnits(storeRid, sizeRid)); // MID track 4921 AnF#666 Fill to Size Plan Enhancement

                    sortedStore[s].Item = s;
                    sortedStore[s].SortKey = new double[4];
                    sortedStore[s].SortKey[0] = needPercent;
                    sortedStore[s].SortKey[1] = need;
                    // ascending seq, thus the -1 multiplier
                    // begin TT#1600 - JEllis - Size Need Algorithm Error
                    //sortedStore[s].SortKey[2] = -1 * (Convert.ToDouble(_sizeNeedResults.GetPriorAllocatedUnits(storeRid, sizeRid), CultureInfo.CurrentUICulture));
                    // begin TT#1176 - Jellis - MD - Group Allocation Size Need not observing inv min max
                    //sortedStore[s].SortKey[2] = -1 * (Convert.ToDouble(_sizeNeedResults.GetAllocatedUnits(storeRid, sizeRid), CultureInfo.CurrentUICulture));
                    sortedStore[s].SortKey[2] = -1 * 
                        (Convert.ToDouble(
                             _sizeNeedResults.GetAllocatedUnits(storeRid, sizeRid)
                             + _sizeNeedResults.GetGroupMemberAllocatedUnits(storeRid, sizeRid),
                             CultureInfo.CurrentUICulture)); 
                    // end TT#1176  - MD - Jellis - Group Allocation Size Need not observing inv min max
                    // end TT#1600 - JEllis - Size Need Algorithm Error
                    sortedStore[s].SortKey[3] = MIDMath.GetRandomDouble();
                }

                Array.Sort(sortedStore, new MIDGenericSortDescendingComparer());

                return sortedStore;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Using the size curve infomation already stored in the SizeNeedResults
        /// it creates a restricted size curve based upon the sizes on the header.
        /// returns number of sizes in _sizeCodeList.
        /// </summary>
        /// <param name="storeKey"></param>
        private bool SetStoreRestrictedSizeCurve(int storeKey)
        {
            bool hasSizes = true;
            SizeCurveProfile aRestrictedSizeCurve = null;
            SizeCurveProfile aSizeCurve = _sizeNeedResults.GetSizeCurve(storeKey);
            if (_sizeRestrictedCurveHash.ContainsKey(aSizeCurve.Key))
            {
                aRestrictedSizeCurve = (SizeCurveProfile)_sizeRestrictedCurveHash[aSizeCurve.Key];
            }
            else
            {
                List<int> sizesNotInCurve = new List<int>(); // TT#1600 - JEllis - Size Need Algorithm Error

                SizeCodeList sizeCodeList = (SizeCodeList)aSizeCurve.SizeCodeList;


                // clone the full size curve, but clear it's sizes.  We'll fill it further on...

                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //aRestrictedSizeCurve = (SizeCurveProfile)aSizeCurve.Clone();

                //So the SizeCurveProfile.Clone() function is actually going to the database and re-reading the size codes
                //Since we are reusing the original values - there is no need to go to the database
                aRestrictedSizeCurve = (SizeCurveProfile)aSizeCurve.CloneNoCopy();
                //End TT#827-MD -jsobek -Allocation Reviews Performance



                // begin TT#579 WUB not normalizing curves
                ////=======================================================================
                //// if this is a Work Up Bulk Size Buy, we use the full size code list so
                //// we don't need to do anything else, otherwise...
                ////=======================================================================
                //if (!_allocProfile.WorkUpBulkSizeBuy // MID TRACK 4261 Size Curves in Size Need Analysis wrong
                //    && _allocProfile.HeaderRID != Include.DefaultHeaderRID)
                if (_allocProfile.HeaderRID != Include.DefaultHeaderRID)
                // end TT#579 WUB not normalizing curves
                {
                    aRestrictedSizeCurve.SizeCodeList.Clear();

                    // begin MID Track 4861 Size Normalization
                    if (this._sizeNeedResults.NormalizeSizeCurves)
                    {
                        // end MID Track 4861 Size Normalization
                        //======================================================================================
                        // This builds a size code list using only those sizes that are used in the color. 
                        //======================================================================================
                        int cnt = _sizeNeedResults.Sizes.Count;
                        for (int s = 0; s < cnt; s++)
                        {
                            int sizeCodeKey = (int)_sizeNeedResults.Sizes[s];
                            // begin MID Track 4861 Size Normalization
                            //if (sizeCodeList.Contains(sizeCodeKey))
                            //{
                            //	SizeCodeProfile scp = (SizeCodeProfile)sizeCodeList.FindKey(sizeCodeKey);
                            //	aRestrictedSizeCurve.AddSizeCode(scp);
                            //}
                            SizeCodeProfile scp = (SizeCodeProfile)sizeCodeList.FindKey(sizeCodeKey);
                            if (scp != null)
                            {
                                aRestrictedSizeCurve.AddSizeCode(scp);
                            }
                            // begin TT#1600 - JEllis - Size Need Algorithm Error
                            else
                            {
                                sizesNotInCurve.Add(sizeCodeKey);
                            }
                            // end TT#1600 - JEllis - Size Need Algorithm Error
                            // end MID Track 4861 Size Normalization
                        }
                        // begin MID Track 4861 Size Normalization
                    }
                    else
                    {
                        foreach (SizeCodeProfile scp in sizeCodeList)
                        {
                            aRestrictedSizeCurve.AddSizeCode(scp);
                        }
                    }
                    // end MID Track 4861 Size Normalization
                }
                // begin MID Track 4261 Size Curves in Size Need Analysis wrong
                else
                {
                    aRestrictedSizeCurve.SizeCodeList.Clear();
                    foreach (SizeCodeProfile scp in sizeCodeList)
                    {
                        aRestrictedSizeCurve.AddSizeCode(scp);
                    }
                }
                // end MID Track 4261 Size Curves in Size Need Analysis wrong
                // Save curve so we don't have to build it again.
                if (aRestrictedSizeCurve.SizeCodeList.Count > 0)
                {
                    // Begin TT#5026 - JSmith - Question about Size Alternates
                    // Adjust curve percents to include size alternates
                    //if (_sizeAlternateHash != null
                    //    && _sizeAlternateHash.Count > 0)
                    //{
                    //    float sizeCodePercent = 0;
                    //    ArrayList altSizeCodeList = null;
                    //    foreach (SizeCodeProfile scp in aRestrictedSizeCurve.SizeCodeList)
                    //    {
                    //        altSizeCodeList = (ArrayList)_sizeAlternateHash[scp.Key];
                    //        if (altSizeCodeList != null)
                    //        {
                    //            sizeCodePercent = 0;
                            
                    //            foreach (SizeCodeProfile altSizeCode in altSizeCodeList)
                    //            {
                    //                sizeCodePercent += altSizeCode.SizeCodePercent;
                    //            }

                    //            scp.SizeCodePercent = sizeCodePercent;
                    //        }
                    //    }
                    //}
                    // End TT#5026 - JSmith - Question about Size Alternates

                    _sizeRestrictedCurveHash.Add(aRestrictedSizeCurve.Key, aRestrictedSizeCurve);
                }
                if (sizesNotInCurve.Count > 0)  // TT#1600 JEllis - Size Need Algorithm Overallocates Size
                {                               // TT#1600 JEllis - Size Need Algorithm Overallocates Size    
                    _sizeNeedResults.SetSizesNotInCurve(aSizeCurve.Key, sizesNotInCurve);  // TT#1600 - JEllis - Size Need Algorithm Error  // TT#1600 JEllis - Size Need Algorithm Overallocates Size
                }                              // TT#1600 JEllis - Size Need Algorithm Overallocates Size
            }

            // Begin MID Issue #3338 - stodd
            if (aRestrictedSizeCurve.SizeCodeList.Count <= 0)
            {
                hasSizes = false;
                //				throw new MIDException(eErrorLevel.warning,
                //					(int)eMIDTextCode.msg_al_StoreSizeCurveDoesNotMatchHeader,
                //					_sab.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_StoreSizeCurveDoesNotMatchHeader));
            }
            else
            {
                _sizeNeedResults.AddRestrictedSizeCurve(storeKey, aRestrictedSizeCurve);
            }

            return hasSizes;
            // End MID Issue #3338 - stodd
        }


        /// <summary>
        /// Using the size curve infomation already stored in the SizeNeedResults
        /// it uppdates the class variable _sizeCodeList with the correct sizes based upon
        /// the sizes on the header.
        /// returns number of sizes in _sizeCodeList.
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        private int SetStoreSizeCodeList(int storeKey)
        {
            _sizeCurve = _sizeNeedResults.GetRestrictedSizeCurve(storeKey);
            _sizeCodeList = _sizeCurve.SizeCodeList;
            return _sizeCodeList.Count;
        }


        // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement (remove obsolete code)
        //		private bool ParmValidation()
        //		{
        //			try
        //			{
        //				bool process = true;
        //				//==============================
        //				// Do we have a valid component
        //				//==============================
        //				//			switch (_targetComponent.ComponentType)
        //				//			{
        //				//				case (eComponentType.SpecificPack):
        //				//				case (eComponentType.SpecificSize):
        //				//				case (eComponentType.Total):
        //				//				case (eComponentType.Bulk):
        //				//				case (eComponentType.ColorAndSize):				
        //				//				case (eComponentType.DetailType):	
        //				//				case (eComponentType.GenericType):
        //				//				case (eComponentType.AllPacks):
        //				//				case (eComponentType.AllGenericPacks):
        //				//				case (eComponentType.AllNonGenericPacks):
        //				//				case (eComponentType.AllSizes):
        //				//				case (eComponentType.AllColors):
        //				//				{
        //				//					//  invalid component
        //				////					_sab.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.warning,
        //				////						(int)(eMIDTextCode.msg_UnknownAllocationComponent),
        //				////						MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent)));
        //				//
        //				//					_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent), this.ToString());
        //				//
        //				//					process = false;
        //				//					break;
        //				//				}
        //				//				case (eComponentType.SpecificColor):
        //				//				{
        //				//					// no exception is thrown and we continue...
        //				//					_colorRid = ((AllocationColorOrSizeComponent)_targetComponent).ColorRID;
        //				//					break;
        //				//				}
        //				//				default:
        //				//				{
        //				//					//  unknown component
        //				////					_sab.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.warning,
        //				////						(int)(eMIDTextCode.msg_UnknownAllocationComponent),
        //				////						MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent)));
        //				//
        //				//					_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_UnknownAllocationComponent), this.ToString());
        //				//
        //				//					process = false;
        //				//					break;
        //				//				}
        //				//			}
        //
        //
        //				//=======================
        //				// Do we have stores?
        //				//=======================
        // //				if (process)
        // //				{
        // //					if (_storeList.Count <= 0)
        // //					{
        // //						//ActionSuccessFlag = false;
        // //
        // //						//					_sab.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.warning,
        // //						//						(int)(eMIDTextCode.msg_al_NoStoresToProcess),
        // //						//						MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess)));
        // //
        // //						_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess), this.ToString());
        // //
        // //						process = false;
        // //					}
        // //				}
        //
        // //				//==========================
        // //				// Do we have Size Curves?
        // //				//==========================
        // //				if (process)
        // //				{
        // //					SizeCurveProfile scp = null;
        // //					if (_sizeCurveGroup.SizeCurveList.Count > 0)
        // //					{
        // //						scp = (SizeCurveProfile)_sizeCurveGroup.SizeCurveList[0];
        // //					}
        // //					else if (_sizeCurveGroup.DefaultSizeCurve != null)
        // //					{
        // //						scp = _sizeCurveGroup.DefaultSizeCurve;
        // //					}
        // //					else
        // //					{
        // //						// NO SIZE CURVES
        // //						//					_sab.ApplicationServerSession.Audit.Add(new MIDException(eErrorLevel.warning,
        // //						//						(int)(eMIDTextCode.msg_al_NoStoresToProcess),
        // //						//						MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess)));
        // //
        // //						_sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_NoStoresToProcess), this.ToString());
        // //
        // //						process = false;
        // //					}
        // //				}
        //
        // //				//============================
        // //				// Is color in alloc profile
        // //				//============================
        // //				if (_allocProfile.BulkColors.ContainsKey(_colorRid))
        // //				{
        // //					_color = (HdrColorBin)_allocProfile.BulkColors[_colorRid];
        // //				}
        // //				else
        // //				{
        // //
        // //					// error
        // //				}
        //				return process;
        //			}
        //			catch (Exception)
        //			{
        //				throw;
        //			}
        //		}
        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (remove obsolete code)

        //private Hashtable SpreadBasedUponSizeCurve(double aNewTotal, SizeCodeList sizeCodeList) // MID Track 3523 Alter Size pct so that user controls size substitution -- j.ellis
        // begin MID Track 3786 Change Fill Size Holes Algorithm
        //private Hashtable SpreadBasedUponSizeCurve(int aNewTotal, SizeCodeList sizeCodeList) // MID Track 3786 Change Fill Size Holes Algorithm
        /// <summary>
        /// Calculates a store's size plan by spreading its total plan to the sizes based on its assigned size curve
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreTotalPlan">Store Total Plan</param>
        /// <param name="aSizeCodeList">SizeCodeList that identifies the sizes and size curve percents</param>
        /// <param name="aCalculateFillPlan">True: Calculates Fill Plan; False: Calculates Size Need plan (includes qty allocated)</param>
        /// <param name="aUseBasisPlan">True: Fill Plan calculated using basis plan; False: Fill Plan calculated using basis ownership</param>
        /// <remarks>Results are placed in the store plan hash of _sizeNeedResults</remarks>
        //private void CalculateStoreSizePlan(int aStoreRID, int aStoreTotalPlan, SizeCodeList aSizeCodeList, bool aCalculateFillPlan, bool aUseBasisPlan) // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        //{
        //    try
        //    {
        //        //Hashtable spreadHash = new Hashtable();

        //        // Sort by Size Curve Percent
        //        // begin TT#2884 - Jellis - AnF Ideal Size Max Size Fill
        //        MIDGenericSortItem[] sortedSizes = new MIDGenericSortItem[aSizeCodeList.Count];
        //        for (int i = 0; i < aSizeCodeList.Count; i++)
        //        {
        //            sortedSizes[i] = new MIDGenericSortItem();
        //            sortedSizes[i].Item = i;
        //            sortedSizes[i].SortKey = new double[2];  //  NOTE:  Want "Equal" to always sort the same; so don't add random here
        //            sortedSizes[i].SortKey[0] = ((SizeCodeProfile)aSizeCodeList[i]).SizeCodePercent;
        //            sortedSizes[i].SortKey[1] = i; // Keep "keys" that are equal in same original order (the plan must be more predictable)
        //        }
        //        SortAscendingComparer sortedAscendingComparer = new SortAscendingComparer();
        //        Array.Sort(sortedSizes, sortedAscendingComparer);
        //        //aSizeCodeList.ArrayList.Sort(new SizeCodePercentComparer()); 
        //        // END TT#2884 - Jellis - AnF Ideal Size Max Size Fill

        //        int unitSizePlan; // MID Track 3786 Change Fill Size Holes Algorithm
        //        double aBasisTotal = 0;

        //        foreach (SizeCodeProfile scp in aSizeCodeList.ArrayList)
        //        {
        //            aBasisTotal += scp.SizeCodePercent;
        //        }

        //        int sizeCount = aSizeCodeList.Count;
        //        for (int i = 0; i < sizeCount; i++)
        //        {
        //            //SizeCodeProfile sizeCode = (SizeCodeProfile)aSizeCodeList[i]; // TT#2884 - Jellis - AnF Ideal Size Max Size Fill 
        //            SizeCodeProfile sizeCode = (SizeCodeProfile)aSizeCodeList[sortedSizes[i].Item];       // TT#2884 - Jellis - AnF Ideal Size Max Size Fill

        //            if (aBasisTotal > 0)
        //            {
        //                unitSizePlan =                              // MID Track 3786 Change Fill Size Holes algorithm
        //                    (int)((sizeCode.SizeCodePercent
        //                    * (double)aStoreTotalPlan                     // MID Track 3786 Change Fill Size Holes algorithm
        //                    / aBasisTotal) + 0.5d);
        //                if (unitSizePlan > aStoreTotalPlan)               // MID Track 3786 Change Fill Size Holes algorithm
        //                {
        //                    unitSizePlan = aStoreTotalPlan; // MID Track 3786 Change Fill Size Holes algorithm
        //                }
        //            }
        //            else
        //            {
        //                unitSizePlan = 0;  // MID Track 3786 Change Fill Size Holes Algorithm
        //            }
        //            aStoreTotalPlan -= unitSizePlan; // MID Track 3786 Change Fill Size Holes algorithm
        //            aBasisTotal -= sizeCode.SizeCodePercent;

        //            //spreadHash.Add(sizeCode.Key, unitSizePlan); 
        //            // begin MID track 4291 add fill variables to size review
        //            if (aCalculateFillPlan)
        //            {
        //                // begin MID track 4921 AnF#666 Fill to Size Plan Enhancement
        //                if (aUseBasisPlan)
        //                {
        //                    this._sizeNeedResults.AddFillToPlan_PlanUnits(aStoreRID, sizeCode.Key, unitSizePlan);
        //                }
        //                else
        //                {
        //                    this._sizeNeedResults.AddFillToOwn_PlanUnits(aStoreRID, sizeCode.Key, unitSizePlan); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        //                }
        //                // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //            }
        //            else
        //            {
        //                // end MID Track 4291 add fill variables to size review
        //                this._sizeNeedResults.AddSizeNeed_PlanUnits(aStoreRID, sizeCode.Key, unitSizePlan); // MID Track 4921 AnF#666 Fill to Size Plan Enhancment
        //            } // MID track 4291 add fill variables to size review
        //        }

        //        //return spreadHash;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        // end MID Track 3786 Change Fill Size Holes Algorithm
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        /// <summary>
        /// Calculates a store's size plan by spreading its total plan to the sizes based on its assigned size curve
        /// </summary>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aStoreTotalPlan">Store Total Plan</param>
        /// <param name="aSizeCodeList">SizeCodeList that identifies the sizes and size curve percents</param>
        /// <param name="aCalculateFillPlan">True: Calculates Fill Plan; False: Calculates Size Need plan (includes qty allocated)</param>
        /// <param name="aUseBasisPlan">True: Fill Plan calculated using basis plan; False: Fill Plan calculated using basis ownership</param>
        /// <param name="aUseSizeMin">True: Subtract out presentation size minimums, then reapply the curve to create an adjusted plan</param>
        /// <remarks>Results are placed in the store plan hash of _sizeNeedResults</remarks>
        private void CalculateStoreSizePlanWithSizeMins(int aStoreRID, int aStoreTotalPlan, SizeCodeList aSizeCodeList, bool aCalculateFillPlan, bool aUseBasisPlan) // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        {
            try
            {
                bool aUseSizeMin = false;
                if (this._FillSizesToType == eFillSizesToType.SizePlanWithMins)
                {
                    aUseSizeMin = true;
                }



                MIDGenericSortItem[] sortedSizes = new MIDGenericSortItem[aSizeCodeList.Count]; 
                for (int i = 0; i < aSizeCodeList.Count; i++)
                {
                    sortedSizes[i] = new MIDGenericSortItem();
                    sortedSizes[i].Item = i;
                    sortedSizes[i].SortKey = new double[2];  //  NOTE:  Want "Equal" to always sort the same; so don't add random here
                    sortedSizes[i].SortKey[0] = ((SizeCodeProfile)aSizeCodeList[i]).SizeCodePercent;
                    sortedSizes[i].SortKey[1] = i; // Keep "keys" that are equal in same original order (the plan must be more predictable)
                }
                MIDGenericSortAscendingComparer sortedAscendingComparer = new MIDGenericSortAscendingComparer();    // TT#1143 - MD - Jellis - Group Allocation Min Broken      
                Array.Sort(sortedSizes, sortedAscendingComparer);


                int unitSizePlan; 
                double aBasisTotal = 0;

                foreach (SizeCodeProfile scp in aSizeCodeList.ArrayList)
                {
                    aBasisTotal += scp.SizeCodePercent;
                }

                int sizeCount = aSizeCodeList.Count;


               
                //Calculate the original plan's units per size by taking the total and multiplying the size curve percentages
                //Store the units per size 

                int[] originalPlanUnits = new int[sizeCount];
                int[] adjustedPlanUnits = new int[sizeCount];
                int[] sizeMinimums = new int[sizeCount];
              
                int totalSizeNeedMinimum = 0;
				int totalRemainingForAllSizes = 0; //TT#3881 -jsobek -Fill to Size Plan PLUS Mins calculation
                //int totalAdjustedRemaining = 0;   //TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.     
                int storeTotalPlanCopy = aStoreTotalPlan;
                double basisTotalCopy = aBasisTotal;

                for (int i = 0; i < sizeCount; i++)
                {
                    SizeCodeProfile sizeCode = (SizeCodeProfile)aSizeCodeList[sortedSizes[i].Item];     

                    if (aBasisTotal > 0)
                    {
                        unitSizePlan =                              
                            (int)((sizeCode.SizeCodePercent
                            * (double)aStoreTotalPlan                     
                            / aBasisTotal) + 0.5d);
                        if (unitSizePlan > aStoreTotalPlan)              
                        {
                            unitSizePlan = aStoreTotalPlan;
                        }
                    }
                    else
                    {
                        unitSizePlan = 0;  
                    }
                    aStoreTotalPlan -= unitSizePlan;
                    aBasisTotal -= sizeCode.SizeCodePercent;

                    //Save the original plan units in an array
                    originalPlanUnits[i] = unitSizePlan;

                    if (aUseSizeMin)
                    {
                        //Get the size minimum (aka presentation minimum)
                        int sizeNeedMinimum = 0;
                        //Begin TT#3441 -jsobek -New Size Method not producing expected results
                        sizeNeedMinimum = _sizeNeedResults.GetStorePresentationMinForSize(aStoreRID, sizeCode.Key);
                        //if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None || _allocProfile.GetStoreImoMaxValue(aStoreRID) == int.MaxValue)
                        //{
                        //    sizeNeedMinimum = _sizeNeedResults.GetStoreInventoryMin(aStoreRID, sizeCode.Key);
                        //}
                        //else
                        //{
                        //    sizeNeedMinimum = _allocProfile.GetStoreItemMinimum(_headerColorRid, sizeCode.Key, aStoreRID);
                        //}
                        //End TT#3441 -jsobek -New Size Method not producing expected results   
 
                        sizeMinimums[i] = sizeNeedMinimum;
                        totalSizeNeedMinimum += sizeNeedMinimum;
                        //Begin TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                        int remaining = unitSizePlan - sizeNeedMinimum;
						totalRemainingForAllSizes += remaining; //TT#3881 -jsobek -Fill to Size Plan PLUS Mins calculation
                        //int adjustedRemaining = remaining;
                        //if (adjustedRemaining < 0)
                        //{
                        //    adjustedRemaining = 0;
                        //}
                        //totalAdjustedRemaining += adjustedRemaining;
                        //End TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                     
                    }
                }

                if (aUseSizeMin)
                {
                    //Apply curve to adjusted remaining 
                    //Begin TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                    //int totalRemaining = storeTotalPlanCopy - totalSizeNeedMinimum;
                    //int totalRemaining = totalAdjustedRemaining;
                    //End TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                      
		    		//Begin TT#3881 -jsobek -Fill to Size Plan PLUS Mins calculation
                    int totalRemaining;

                    if (totalRemainingForAllSizes <= 0)
                    {
                        totalRemaining = 0;
                    }
                    else
                    {
                        totalRemaining = totalRemainingForAllSizes;
                    }
                    //End TT#3881 -jsobek -Fill to Size Plan PLUS Mins calculation

                    int adjustedUnitSizePlan;
                    for (int i = 0; i < sizeCount; i++)
                    {
                        SizeCodeProfile sizeCode = (SizeCodeProfile)aSizeCodeList[sortedSizes[i].Item];
                        if (basisTotalCopy > 0)
                        {
                            adjustedUnitSizePlan =
                                (int)((sizeCode.SizeCodePercent
                                * (double)totalRemaining
                                / basisTotalCopy) + 0.5d);
                            if (adjustedUnitSizePlan > totalRemaining)
                            {
                                adjustedUnitSizePlan = totalRemaining;
                            }
                        }
                        else
                        {
                            adjustedUnitSizePlan = 0;
                        }
                        totalRemaining -= adjustedUnitSizePlan;
                        basisTotalCopy -= sizeCode.SizeCodePercent;

                        //Ensure no adjusted plan units ever go below zero
                        //Begin TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                        //adjustedPlanUnits[i] = adjustedUnitSizePlan + sizeMinimums[i];
                        int adjustedPlanUnitsCalculated = adjustedUnitSizePlan + sizeMinimums[i];
                        if (adjustedPlanUnitsCalculated < 0)
                        {
                            adjustedPlanUnitsCalculated = 0;
                        }
                        adjustedPlanUnits[i] = adjustedPlanUnitsCalculated;
                        //End TT#3493-MD -jsobek -Fill to Size Plan Presentation - The fill fwd plan variable for some sizes is negative (XSMall and XXLarge)  Would not expect this variable to be negative.
                      
                    }
                }
                

                for (int i = 0; i < sizeCount; i++)
                {
                    SizeCodeProfile sizeCode = (SizeCodeProfile)aSizeCodeList[sortedSizes[i].Item];

                    int units;
                    if (aUseSizeMin)
                    {
                        units = adjustedPlanUnits[i];
                    }
                    else
                    {
                        units = originalPlanUnits[i];
                    }


                    if (aCalculateFillPlan)
                    {
                        if (aUseBasisPlan)
                        {
                            this._sizeNeedResults.AddFillToPlan_PlanUnits(aStoreRID, sizeCode.Key, units);
                        }
                        else
                        {
                            this._sizeNeedResults.AddFillToOwn_PlanUnits(aStoreRID, sizeCode.Key, units); 
                        }
                    }
                    else
                    {
                        Debug.WriteLineIf(aStoreRID == Include.FocusStoreRID, "CalculateStoreSizePlan() ST RID: " + aStoreRID + " SZ: " + sizeCode.Key + "(" + sizeCode.SizeCodePrimary + ") PLAN: " + units);
                        this._sizeNeedResults.AddSizeNeed_PlanUnits(aStoreRID, sizeCode.Key, units); 
                    } 
                }
           


            }
            catch (Exception)
            {
                throw;
            }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation
        


        private AllocationWorkMultiple GetQtyPerPack(GeneralComponent aComponent, AllocationProfile aAllocProfile)
        {
            AllocationWorkMultiple awm = new AllocationWorkMultiple(1, 0);

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


    }

    [Serializable()]
    public class SizeNeedResults
    {
        private long _instanceID; // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        //private eSizeMethodType _methodType;  // TT#488 - MD - Jellis - Group Allocation (field not used)
        private bool _getOnhandAndIntransit; // MID Track 3786 Change Fill Size Holes Algorithm
        // Begin MID Issue # 3160 
        private int _headerColorRid;  // dummy color indicates that we are processing detail packs; otherwise, specific bulk color being processed // MID Track 3749  
        private HdrColorBin _color;
        private GeneralComponent _component;
        // End MID Issue # 3160 

        //=============================================================================================
        // holds plan units by store and size
        // Hash by store rid that contains another hash table keyed by size with units as it's value
        //=============================================================================================
        // begin TT#1391 - TMW new Action (unrelated - Performance)
        //private Hashtable _storeSizeNeed_PlanHash;  // MID track 4921 AnF#666 Fill to Size Plan Enhancement 
        //private Hashtable _storeFillToOwn_PlanHash; // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        //private Hashtable _storeFillToPlan_PlanHash; // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        //private Hashtable _storeFillToPlan_SalesHash; // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        //private Hashtable _storeFillToPlan_StockHash; // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        private Dictionary<int, StoreSizeVector> _storeSizeNeed_Plan;
        private Dictionary<int, StoreSizeVector> _storeFillToOwn_Plan;
        private Dictionary<int, StoreSizeVector> _storeFillToPlan_Plan;
        private StoreVector _storeFillToPlan_Sales;
        private StoreVector _storeFillToPlan_Stock;
        // end TT#1391 - TMW new Action (unrelated - Performance)
        private bool _accumulatePriorAllocated; // MID track 4291 add fill variables to size review
        private bool _calculateFillToOwn_Plan;           // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        private bool _calculateFillToPlan_Plan;          // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        private bool _loadFillToPlan_BasisSaleStock;     // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //=============================================================================================
        // holds need allocated units by store and size
        // Hash by store rid that contains another hash table keyed by size with units as it's value
        //=============================================================================================
        // begin TT#1391 - TMW New Action (Unrelated Performance)
        //private Hashtable _storeAllocatedHash;
        //private Hashtable _sizeAllocatedHash; // MID Track 4425 Fill Size Holes Need Phase overallocates size
        private StoreVector _storeSizeNotInCurveTotalAllocated;  // TT#1600 - JEllis - Size Need Algorithm Error
        private Dictionary<int, StoreSizeVector> _storeAllocated;

        SizeCurveGroupProfile _sizeCurveGroup;  // Original Size curves b4 Alternates.
        private MIDHashtable _storeSizeCurveHash;  //Full size curve with Alternates applied.
        private MIDHashtable _storeRestrictedSizeCurveHash; // Partial size curve restricted by sizes on header w/ alts applied.
        private SizeCurveProfile _defaultSizeCurve;  //default full size curve with alternates applied
        private SizeCurveProfile _defaultRestrictedSizeCurve;  //default partial size curve with alternates applied

        private ProfileList _storeList;
        private eSizeProcessControl _processControl = eSizeProcessControl.SizeCurvesOnly;
        private StoreSizeConstraints _storeSizeConstraints;

        // Used with BasisSize
        private ArrayList _sizes;

        // begin TT#1391 - TMW New Action (Unrelated Performance)
        //private Hashtable _storeTotalPlan;
        //private Hashtable _storeFillToOwn_SzTotPlanHash;  // MID Track 4291 add fill variable to size review // MID Track 4921 AnF#666 Fill To Size plan Enhancement
        //private Hashtable _storeFillToPlan_SzTotPlanHash; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        //private Hashtable _storeOnhandHash;
        //private Hashtable _storeIntransitHash;
        //private Hashtable _storePriorAllocatedHash;
        //private Hashtable _storeRuleHash; // MID Track 3781 Size Curve Not Required
        private Dictionary<int, List<int>> _storeSizesNotInCurveDict;
        private StoreVector _storeTotalPlan;
        private StoreVector _storeFillToOwn_SzTotPlan;
        private StoreVector _storeFillToPlan_SzTotPlan;
        private Dictionary<int, StoreSizeVector> _storeOnhand;
        private Dictionary<int, StoreSizeVector> _storeIntransit;
        // begin TT#41 - MD - JEllis - Size Inventory Min Max
        private Dictionary<int, StoreSizeVector> _storeIbOnhand;
        private Dictionary<int, StoreSizeVector> _storeIbIntransit;
        // end TT#41 - MD - JEllis - Size inventory Min Max
        private Dictionary<int, StoreSizeVector> _storePriorAllocated;
        private Dictionary<int, StoreSizeVector> _storePriorDetailAllocated;	//TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        private Dictionary<int, StoreSizeVector> _storeGroupMemberAllocated; // TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
        private Dictionary<int, StoreSizeVector> _storeGroupMemberBulkAllocated;   // TT#1828 - MD - JSmith - Size Need not allocatde to size
        private Dictionary<int, StoreSizeVector> _storeVswOnhand; // TT#2313 - JEllis - AnF VSW -- Size allocation not using VSW OH
        private Dictionary<int, StoreSizeVector> _storeIbVswOnhand; // TT#304 - MD - JEllis - Size Inventory Min Max not correct
        private Dictionary<long, eSizeRuleType> _storeRule; 
        // end TT#1391 - TMW New Action (Unrelated Performance
        // begin TT#1543 - JEllis - Size Multiple Broken
        //Dictionary<int, Dictionary<int, int[]>> _inventoryMinBySize; // TT#246 - MD - JEllis - AnF VSW In Store Minimum prt 4  // TT#519 - MD - Jellis - VSW - Minimums not working
        private Dictionary<int, Dictionary<int, int>> _inventoryMinBySize;  // TT#519 - MD - Jellis - VSW - Minimums not working
        private int _lastMinSizeCodeRID;
        private Dictionary<int, Dictionary<int, int>> _inventoryMaxBySize;
        private Dictionary<int, Dictionary<int, int>> _inventoryCapMaxBySize; // TT#2155 - JEllis - Fill Size Holes Null Reference
        private int _lastMaxSizeCodeRID;
        private int _lastCapMaxSizeCodeRID;                        // TT#2155 - JEllis - Fill Size Holes Null Reference
        private Dictionary<int, int> _lastInventoryMaxByStore;
        private Dictionary<int, int> _lastInventoryCapMaxByStore;  // TT#2155 - JEllis - Fill Size Holes Null Reference
        //Dictionary<int, int[]> _lastInventoryMinByStore;  // TT#246 - MD - JEllis - AnF VSW In Store Minimum prt 4 // TT#519 - MD - Jellis - VSW - Minimums not working
        Dictionary<int, int> _lastInventoryMinByStore;      // TT#519 - MD - Jellis - VSW - Minimums not working
        // end TT#1543 - JEllis - Size Multiple Broken
        private bool _normalizeSizeCurves;  // MID Track 4861 Size Curve Normalization
        private eVSWSizeConstraints _vswSizeConstraints; // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        private AllocationProfile _allocationProfile;    // TT#519 - MD - Jellis - AnF VSW - Minimums not working
        private int _ibMdseBasisRID; // TT#1074 - MD - Jellis - Inventory Min/Max incorrrect for Group Allocation
        private int _mdseBasisRID;   // TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
        //==================
        // Properties
        //==================
        // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        public long InstanceID
        {
            get { return _instanceID; } 
        }
        public void ResetCache()
        {
            _lastMinSizeCodeRID = 0;
            _lastMaxSizeCodeRID = 0;
            _lastCapMaxSizeCodeRID = 0;
            _inventoryMaxBySize = new Dictionary<int,Dictionary<int,int>>();
            _inventoryMinBySize = new Dictionary<int,Dictionary<int,int>>();
            _inventoryCapMaxBySize = new Dictionary<int,Dictionary<int,int>>();
            _lastInventoryMaxByStore = null;
            _lastInventoryMinByStore = null;
            _lastInventoryCapMaxByStore = null;
        }
        // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        public SizeCurveGroupProfile SizeCurveGroup
        {
            set { _sizeCurveGroup = value; }
            get { return _sizeCurveGroup; }
        }

        /// <summary>
        /// A list of Size Keys used during size need.
        /// </summary>
        public ArrayList Sizes
        {
            get { return (ArrayList)_sizes.Clone(); }
        }

        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        //public eSizeMethodType MethodType
        //{
        //    get { return _methodType; }
        //    set { _methodType = value; }
        //}
        // end TT#2155 - JEllis - Fill Size Holes Null Reference

        public ProfileList Stores
        {
            get { return _storeList; }
            set { _storeList = value; }
        }

        public Hashtable StoreSizeCurveHash
        {
            get { return _storeSizeCurveHash; }
        }

        public Hashtable StoreRestrictedSizeCurveHash
        {
            get { return _storeRestrictedSizeCurveHash; }
        }

        // begin TT#1074 - MD - Jellis - Inventory Min Max incorrect for Group Allocation
        public int InventoryMdseBasisRID
        {
            get { return _ibMdseBasisRID; }
            set { _ibMdseBasisRID = value; }
        }
        // end TT#1074 - MD - Jellis- Inventory Min Max incorrect for Group Allocation
 
        // begin TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
        public int MerchandiseBasisRID
        {
            get { return _mdseBasisRID; }
            set { _mdseBasisRID = value; }
        }
        // end TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max

        // begin TT#1600 - JEllis - Size Need Algorithm Error
        public List<int> GetSizesNotInCurve(int aSizeCurveRID)  // TT#1600 - JEllis - Size Need Overallocates Sizes
        {
            List<int> sizesNotInCurve;
            if (_storeSizesNotInCurveDict.TryGetValue(aSizeCurveRID, out sizesNotInCurve)) // TT#1600 - JEllis - Size Need Overallocates Sizes
            {
                return sizesNotInCurve;
            }
            sizesNotInCurve = new List<int>();
            return sizesNotInCurve;
        }
        public void AddSizeToSizesNotInCurve(int aSizeCurveRID, int aSizeRID) // TT#1600 - JEllis - Size Need Overallocates Sizes
        {
            List<int> sizesNotInCurve = GetSizesNotInCurve(aSizeCurveRID); // TT#1600 - JEllis - Size Need Overallocates Sizes
            foreach (int sizeRID in sizesNotInCurve)
            {
                if (sizeRID == aSizeRID)
                {
                    return;
                }
            }
            sizesNotInCurve.Add(aSizeRID);
            SetSizesNotInCurve(aSizeCurveRID, sizesNotInCurve); // TT#1600 - JEllis - Size Need Overallocates Sizes
        }
        public void SetSizesNotInCurve(int aSizeCurveRID, List<int> aSizesNotInCurve) // TT#1600 - JEllis - Size Need Overallocates Sizes
        {
            _storeSizesNotInCurveDict.Remove(aSizeCurveRID); // TT#1600 - JEllis - Size Need Overallocates Sizes
            _storeSizesNotInCurveDict.Add(aSizeCurveRID, aSizesNotInCurve); // TT#1600 - JEllis - Size Need Overallocates Sizes
        }
        // end TT#1600 - JEllis - Size Need Algorithm Error

        /// <summary>
        /// set to TRUE when only the plan values have been figured
        /// </summary>
        public eSizeProcessControl ProcessControl
        {
            get { return _processControl; }
            set { _processControl = value; }
        }

        public StoreSizeConstraints StoreSizeConstraints
        {
            get { return _storeSizeConstraints; }
            set { _storeSizeConstraints = value; }
        }

        // Begin MID Issue # 3160 
        /// <summary>
        /// Gets or sets Header Color RID.  When the dummy color, then processing detail packs.  Specific color indicates processing a bulk color on header
        /// </summary>
        public int HeaderColorRid
        {
            get { return _headerColorRid; } // MID Track 3749
            set { _headerColorRid = value; } // MID Track 3749
        }

        public HdrColorBin Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public GeneralComponent Component
        {
            get { return _component; }
            set { _component = value; }
        }
        // End MID Issue # 3160 
        // begin MID Track 3786 Change Fill Size Holes Algorithm
        public bool GetOnHandAndIT
        {
            get { return _getOnhandAndIntransit; }
        }
        // end MID Track 3786 Change Fill Size Holes Algorithm

        // begin MID track 4291 add fill variables to size review
        public bool CalculateFillToOwn_Plan   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            get { return _calculateFillToOwn_Plan; } // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            set { _calculateFillToOwn_Plan = value; } // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        }
        public bool CalculateFillToPlan_Plan   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            get { return _calculateFillToPlan_Plan; } // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            set { _calculateFillToPlan_Plan = value; } // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
        }
        internal bool LoadFillToPlan_BasisSaleStock
        {
            get { return this._loadFillToPlan_BasisSaleStock; }
            set { this._loadFillToPlan_BasisSaleStock = value; }
        }
        public bool AccumulatePriorAllocated
        {
            get { return _accumulatePriorAllocated; }
            set { _accumulatePriorAllocated = value; }
        }
        // end MID track 4291 add fill variables to size review

        // begin MID Track 4861 Size Normalization
        /// <summary>
        /// Gets or sets Normalize Size Curve flag
        /// </summary>
        public bool NormalizeSizeCurves
        {
            get { return _normalizeSizeCurves; }
            set { _normalizeSizeCurves = value; }
        }
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vswSizeConstraints; }
        }
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        // end MID Track 4861 Size Normalization
        //==================
        // Constructor
        //==================
        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
        public SizeNeedResults()
        {
            Constructor(null, eVSWSizeConstraints.None);  // TT#519 - MD - JEllis - AnF VSW - Minimums not working
        }
        public SizeNeedResults(AllocationProfile aAllocationProfile, eVSWSizeConstraints aVswSizeConstraints)  // TT#519 - MD - JEllis - AnF VSW - Minimums not working
        {
            Constructor(aAllocationProfile, aVswSizeConstraints);  // TT#519 - MD - JEllis - AnF VSW - Minimums not working
        }
        public void Constructor(AllocationProfile aAllocationProfile, eVSWSizeConstraints aVswSizeConstraints)  // TT#519 - MD - JEllis - AnF VSW - Minimums not working
        {
            //end TT#246 - MD - JEllis - AnF VSW In Store Minimums pt 5
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //_storeSizeNeed_PlanHash = new Hashtable();
            //_storeFillToOwn_PlanHash = new Hashtable(); // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //_storeFillToPlan_PlanHash = new Hashtable(); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //_storeFillToPlan_SalesHash = new Hashtable(); // MID track 4921 AnF#666 Fill to Size Plan Enhancement
            //_storeFillToPlan_StockHash = new Hashtable(); // MID track 4921 AnF#666 Fill to Size Plan Enhancement
            _instanceID = DateTime.Now.Ticks;  // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            _storeSizeNeed_Plan = new Dictionary<int, StoreSizeVector>();
            _storeFillToOwn_Plan = new Dictionary<int, StoreSizeVector>();
            _storeFillToPlan_Plan = new Dictionary<int, StoreSizeVector>();
            _storeFillToPlan_Sales = new StoreVector();
            _storeFillToPlan_Stock = new StoreVector();
            // end TT#1391 - TMW New Action (Unrelated - Performance)
            _calculateFillToOwn_Plan = true; // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            _calculateFillToPlan_Plan = true; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            _loadFillToPlan_BasisSaleStock = true; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            _accumulatePriorAllocated = true; // MID track 4291 add fill variables to size review
            // begin TT#1391 - New Action (Unrelated Performance)
            //_storeAllocatedHash = new Hashtable();
            //_sizeAllocatedHash = new Hashtable(); // MID Track 4425 Fill Size Holes Need Phase overallocates size
            _storeAllocated = new Dictionary<int, StoreSizeVector>();
            // end TT#1391 - New Action (Unrelated Performance)
            _sizes = new ArrayList();
            _storeSizeCurveHash = new MIDHashtable();
            _storeRestrictedSizeCurveHash = new MIDHashtable();
            _storeSizesNotInCurveDict = new Dictionary<int, List<int>>(); // TT#1600 - JEllis - Size Need Algorithm Error
            // begin TT#1391 - New Action (Unrelated Performance)
            //_storeTotalPlan = new Hashtable();
            //_storeFillToOwn_SzTotPlanHash = new Hashtable(); //MID track 4291 add fill variables to size review // MID track 4921 AnF#666 Fill To Size Plan Enhancement
            //_storeFillToPlan_SzTotPlanHash = new Hashtable(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //_storeOnhandHash = new Hashtable();
            //_storeIntransitHash = new Hashtable();
            //_storePriorAllocatedHash = new Hashtable();
            _storeTotalPlan = new StoreVector();
            _storeFillToOwn_SzTotPlan = new StoreVector();
            _storeFillToPlan_SzTotPlan = new StoreVector();
            _storeOnhand = new Dictionary<int, StoreSizeVector>();
            _storeVswOnhand = new Dictionary<int, StoreSizeVector>(); // TT#2313 - JEllis - AnF VSW -- Size allocation not using VSW OH
            _storeIbVswOnhand = new Dictionary<int, StoreSizeVector>(); // TT#304 - MD - Jellis - Size Inventory Min Max incorrect
            _storeIntransit = new Dictionary<int, StoreSizeVector>();
            // begin TT#41 - MD - JEllis - Size Inventory Min Max
            _storeIbOnhand = new Dictionary<int, StoreSizeVector>();
            _storeIbIntransit = new Dictionary<int, StoreSizeVector>();
            // end TT#41 - MD - Jellis - Size Inventory Min Max
            _storePriorAllocated = new Dictionary<int, StoreSizeVector>();
            _storePriorDetailAllocated = new Dictionary<int, StoreSizeVector>();	//TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
            _storeGroupMemberAllocated = new Dictionary<int, StoreSizeVector>(); // TT#1176 - MD - Jellis - Group ALlocation Size Need not observing inv min max
            _storeGroupMemberBulkAllocated = new Dictionary<int, StoreSizeVector>();   // TT#1828 - MD - JSmith - Size Need not allocatde to size
            // end TT#1391 - New Action (Unrelated Performance)
            _defaultSizeCurve = new SizeCurveProfile(Include.NoRID);
            _defaultRestrictedSizeCurve = new SizeCurveProfile(Include.NoRID);
            // begin TT#1391 - New Action - (Unrelated Performance)
            //_storeRuleHash = new Hashtable(); // MID Track 3781 Size Curve Not Required
            _storeRule = new Dictionary<long, eSizeRuleType>();
            // end TT#1391 - New Action - (Unrelated Performance)
            _getOnhandAndIntransit = true; // MID Track 3786 Change Fill Size Holes Algorithm
            _normalizeSizeCurves = true;   // MID Track 4861 Size Normalization
            // begin TT#1543 - JEllis - Size Multiple Broken
            //_inventoryMinBySize = new Dictionary<int, Dictionary<int, int[]>>(); // TT#246 - MD - JEllis - AnF VSW In Store Minimum prt 4 // TT#519 - MD - Jellis - VSW - Minimums not working
            _inventoryMinBySize = new Dictionary<int, Dictionary<int, int>>(); // TT#519 - MD - Jellis - VSW - Minimums not working
            _lastMinSizeCodeRID = Include.NoRID;
            _inventoryMaxBySize = new Dictionary<int,Dictionary<int,int>>();
            _lastMaxSizeCodeRID = Include.NoRID;
            // end TT#1543 - JEllis - Size Multiple Broken
            // begom TT#2155 - JEllis - Fill Size Holes Null Reference
            _inventoryCapMaxBySize = new Dictionary<int, Dictionary<int, int>>();
            _lastInventoryCapMaxByStore = null;
            _lastCapMaxSizeCodeRID = 0;
            // end TT#2155 - JEllis - Fill Size Holes Null Reference
            _vswSizeConstraints = aVswSizeConstraints; // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
            _allocationProfile = aAllocationProfile; // TT#519 - MD - Jellis - AnF VSW -- Minimums not working
        }

        //==================
        // Methods
        //==================

		// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        public void ClearStoreAllocated()
        {
            _storeAllocated.Clear();
        }

        public void ClearStoreOnHand()
        {
            _storeOnhand.Clear();
        }

        public void ClearStoreIntransit()
        {
            _storeIntransit.Clear();
        }
		// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

        // Begin TT#5063 - JSmith - Fill Size with constraints and rule fails on header with packs and loose
        public void ClearStoreRule()
        {
            _storeRule.Clear();
        }
        // End TT#5063 - JSmith - Fill Size with constraints and rule fails on header with packs and loose
        public void Clear(bool clearSizeCurves)
        {
            // TT#1391 - TMW New Action (Unrelated - Performance)
            //ClearHashtable(_storeSizeNeed_PlanHash);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //ClearHashtable(_storeFillToOwn_PlanHash); // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //ClearHashtable(_storeFillToPlan_PlanHash); // MID track 4921 AnF#666 Fill To Size Plan Enhancement
            //ClearHashtable(_storeAllocatedHash);
            //_sizeAllocatedHash.Clear();         // MID Track 4425 Fill Size Holes Need Phase overallocates size
            //_storeFillToPlan_SalesHash.Clear(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //_storeFillToPlan_StockHash.Clear(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            _storeSizeNeed_Plan.Clear();
            _storeFillToOwn_Plan.Clear();
            _storeFillToPlan_Plan.Clear();
            _storeAllocated.Clear();
            _storeFillToPlan_Sales.Clear();
            _storeFillToPlan_Stock.Clear();
            // TT#1391 - TMW New Action (Unrelated - Performance
            _processControl = eSizeProcessControl.SizeCurvesOnly;
            if (clearSizeCurves)
            {
                _sizes.Clear();
                _storeSizeCurveHash.Clear();
                _storeRestrictedSizeCurveHash.Clear();
                _defaultSizeCurve = new SizeCurveProfile(Include.NoRID);
                _defaultRestrictedSizeCurve = new SizeCurveProfile(Include.NoRID);
                _storeSizesNotInCurveDict.Clear(); // TT#1600 - JEllis - Size Need Algorithm Error
                _storeSizeNotInCurveTotalAllocated = null; // TT#1600 - JEllis - Size Need Algorithm Error
            }
            // _storeInitialTotalPlan.Clear(); // not used
            // begin TT#1391 - TMW New Actio (Unrelated Performance)
            //_storeTotalPlan.Clear();
            //_storeFillToOwn_SzTotPlanHash.Clear(); // MID track 4291 add fill variable to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //_storeFillToPlan_SzTotPlanHash.Clear(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //_storeFillToPlan_SzTotPlanHash.Clear(); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //ClearHashtable(_storeOnhandHash);
            //ClearHashtable(_storeIntransitHash);
            //ClearHashtable(_storePriorAllocatedHash);
            //ClearHashtable(_storeRuleHash); // MID Track 3781 Size Curve Not Required
            _storeTotalPlan.Clear();
            _storeOnhand.Clear();
            _storeVswOnhand.Clear(); // TT#2313 - JEllis - AnF VSW -- Size allocation not using VSW OH
            _storeIbVswOnhand.Clear(); // TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
            _storeIbOnhand.Clear();  // TT#41 - MD - Jellis - Size Inventory Min Max
            _storeFillToOwn_SzTotPlan.Clear();
            _storeFillToPlan_SzTotPlan.Clear();
            _storeIntransit.Clear();
            _storeIbIntransit.Clear(); // TT#41 - MD - Jellis - Size inventory Min Max
            _storePriorAllocated.Clear();
            _storePriorDetailAllocated.Clear();		// TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
            _storeGroupMemberAllocated.Clear(); // TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
            _storeGroupMemberBulkAllocated.Clear();   // TT#1828 - MD - JSmith - Size Need not allocatde to size
            _storeRule.Clear(); 
            // end TT#1391 - TMW New Action (Unrelated Performance)
            this._getOnhandAndIntransit = true; // MID Track 3786 Change Fill Size Holes Algorithm
            this._calculateFillToOwn_Plan = true; // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            this._calculateFillToPlan_Plan = true; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            this._accumulatePriorAllocated = true; // MID track 4291 add fill variables to size review
            _loadFillToPlan_BasisSaleStock = true; // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            _lastMinSizeCodeRID = Include.NoRID;
            _lastMaxSizeCodeRID = Include.NoRID;
            _lastCapMaxSizeCodeRID = Include.NoRID;
            _inventoryMinBySize = new Dictionary<int, Dictionary<int, int>>();
            _inventoryMaxBySize = new Dictionary<int, Dictionary<int, int>>();
            _inventoryCapMaxBySize = new Dictionary<int, Dictionary<int, int>>();
            _lastInventoryMinByStore = null;
            _lastInventoryMaxByStore = null;
            _lastInventoryCapMaxByStore = null;
            // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        }

        /// <summary>
        /// used by BasisSize to store sizes used during allocaton
        /// </summary>
        /// <param name="sizeRid"></param>
        public void AddSize(int sizeRid)
        {
            _sizes.Add(sizeRid);
        }

        /// <summary>
        /// Adds the units to the plan hash for the store and size given
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <param name="units"></param>
        public void AddSizeNeed_PlanUnits(int storeRid, int sizeRid, int units) // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeSizeNeed_PlanHash); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            StoreSizeVector storeSizeNeedPlanUnits;
            if (!_storeSizeNeed_Plan.TryGetValue(sizeRid, out storeSizeNeedPlanUnits))
            {
                storeSizeNeedPlanUnits = new StoreSizeVector(sizeRid);
                _storeSizeNeed_Plan.Add(sizeRid, storeSizeNeedPlanUnits);
            }
            storeSizeNeedPlanUnits.SetStoreValue(storeRid, storeSizeNeedPlanUnits.GetStoreValue(storeRid) + units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the plan units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <returns></returns>
        public int GetSizeNeed_PlanUnits(int storeRid, int sizeRid) // MID track 4921 AnF#666 Fill to Size Plan Enhancement
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storeSizeNeedPlanUnits;
            if (_storeSizeNeed_Plan.TryGetValue(sizeRid, out storeSizeNeedPlanUnits))
            {
                return (int)storeSizeNeedPlanUnits.GetStoreValue(storeRid);
            }
            return 0;
            //int units = GetUnits(storeRid, sizeRid, _storeSizeNeed_PlanHash); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }

        // begin MID Track 4291 add fill variables to size review
        /// <summary>
        /// Adds the units to the plan hash for the store and size given
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <param name="units"></param>
        public void AddFillToOwn_PlanUnits(int storeRid, int sizeRid, int units) // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        {

            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeFillToOwn_PlanHash);        // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            StoreSizeVector storeFillToOwnPlanUnits;
            if (!_storeFillToOwn_Plan.TryGetValue(sizeRid, out storeFillToOwnPlanUnits))
            {
                storeFillToOwnPlanUnits = new StoreSizeVector(sizeRid);
                _storeFillToOwn_Plan.Add(sizeRid, storeFillToOwnPlanUnits);
            }
            storeFillToOwnPlanUnits.SetStoreValue(storeRid, storeFillToOwnPlanUnits.GetStoreValue(storeRid) + units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the plan units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <returns></returns>
        public int GetFillToOwn_PlanUnits(int storeRid, int sizeRid) // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storeFillToOwnPlanUnits;
            if (_storeFillToOwn_Plan.TryGetValue(sizeRid, out storeFillToOwnPlanUnits))
            {
                return (int)storeFillToOwnPlanUnits.GetStoreValue(storeRid);
            }
            return 0;
            //int units = GetUnits(storeRid, sizeRid, _storeFillToOwn_PlanHash); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)

        }
        // end MID track 4291 add fill variables to size review
        // begin MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        /// <summary>
        /// Adds the units to the plan hash for the store and size given
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <param name="units"></param>
        public void AddFillToPlan_PlanUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeFillToPlan_PlanHash);       // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            StoreSizeVector storeFillToPlanPlanUnits;
            if (!_storeFillToPlan_Plan.TryGetValue(sizeRid, out storeFillToPlanPlanUnits))
            {
                storeFillToPlanPlanUnits = new StoreSizeVector(sizeRid);
                _storeFillToPlan_Plan.Add(sizeRid, storeFillToPlanPlanUnits);
            }
            storeFillToPlanPlanUnits.SetStoreValue(storeRid, storeFillToPlanPlanUnits.GetStoreValue(storeRid) + units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the plan units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <returns></returns>
        public int GetFillToPlan_PlanUnits(int storeRid, int sizeRid)
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storeFillToPlanPlanUnits;
            if (_storeFillToPlan_Plan.TryGetValue(sizeRid, out storeFillToPlanPlanUnits))
            {
                return (int)storeFillToPlanPlanUnits.GetStoreValue(storeRid);
            }
            return 0;
            //int units = GetUnits(storeRid, sizeRid, _storeFillToPlan_PlanHash);
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        /// <summary>
        /// Adds the sales basis units to the sales hash for the store and size given
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="units"></param>
        public void AddFillToPlan_SalesUnits(int storeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //_storeFillToPlan_SalesHash.Add(storeRid, units);
            _storeFillToPlan_Sales.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the sales basis plan units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <returns></returns>
        public int GetFillToPlan_SalesUnits(int storeRid)
        {
            // begin TT#1391 - TMW New Action (Unrelated Perfomance)
            return (int)_storeFillToPlan_Sales.GetStoreValue(storeRid);
            //int units = 0;
            //try
            //{
            //    units = (int)_storeFillToPlan_SalesHash[storeRid]; 
            //}
            //catch(NullReferenceException)
            //{
            //    units = 0;
            //}
            //catch(IndexOutOfRangeException)
            //{
            //    units = 0;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return units;
            // end TT#1391 - TMW New Action (Unrelated Performance)
        }
        /// <summary>
        /// Adds the stock basis plan units to the stock hash for the store and size given
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="units"></param>
        public void AddFillToPlan_StockUnits(int storeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //_storeFillToPlan_StockHash.Add(storeRid, units);
            _storeFillToPlan_Stock.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the stock basis units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <returns></returns>
        public int GetFillToPlan_StockUnits(int storeRid)
        {
            // begin TT#1391 - TMW New Action (Unrelated Perfomance)
            return (int)_storeFillToPlan_Stock.GetStoreValue(storeRid);
            //int units = 0;
            //try
            //{
            //    units = (int)_storeFillToPlan_StockHash[storeRid]; 
            //}
            //catch(NullReferenceException)
            //{
            //    units = 0;
            //}
            //catch(IndexOutOfRangeException)
            //{
            //    units = 0;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //return units;
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }
        // end MID Track 4921 AnF#666 Fill To Size Plan Enhancement

        /// <summary>
        /// Adds the units to the allocated hash for the store and size given
        /// </summary>
        /// <param name="storeRid">RID identifying store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <param name="units">Newly allocated units</param>
        public void AddAllocatedUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeAllocatedHash);
            //AddAllStoreTotalUnits(sizeRid, units, _sizeAllocatedHash);   // MID Track 4425 Fill Size Holes Need Phase overallocates size
            StoreSizeVector storeAllocatedUnits;
            if (!_storeAllocated.TryGetValue(sizeRid, out storeAllocatedUnits))
            {
                storeAllocatedUnits = new StoreSizeVector(sizeRid);
                _storeAllocated.Add(sizeRid, storeAllocatedUnits);
            }
            storeAllocatedUnits.SetStoreValue(storeRid, storeAllocatedUnits.GetStoreValue(storeRid) + units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Sets the units to the allocated hash for the store and size given
        /// </summary>
        /// <param name="storeRid">RID identifying store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <param name="units">Total Allocated Units</param>
        public void SetAllocatedUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //SetUnits(storeRid, sizeRid, units, _storeAllocatedHash);
            //AddAllStoreTotalUnits(sizeRid, units, _sizeAllocatedHash);   // MID Track 4425 Fill Size Holes Need Phase overallocates size
            StoreSizeVector storeAllocatedUnits;
            if (!_storeAllocated.TryGetValue(sizeRid, out storeAllocatedUnits))
            {
                storeAllocatedUnits = new StoreSizeVector(sizeRid);
                _storeAllocated.Add(sizeRid, storeAllocatedUnits);
            }
            storeAllocatedUnits.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        /// <summary>
        /// Gets the allocated units for the given store and size
        /// </summary>
        /// <param name="storeRid"></param>
        /// <param name="sizeRid"></param>
        /// <returns></returns>
        public int GetAllocatedUnits(int storeRid, int sizeRid)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //int units = GetUnits(storeRid, sizeRid, _storeAllocatedHash);
            //return units;
            StoreSizeVector storeAllocatedUnits;
            if (_storeAllocated.TryGetValue(sizeRid, out storeAllocatedUnits))
            {
                return (int)storeAllocatedUnits.GetStoreValue(storeRid);
            }
            return 0;
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }

        // begin MID Track 4425 Fill Size Holes Need Phase overallocates size
        public int GetAllStoreTotalAllocatedUnits(int sizeRid)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //int units = GetAllStoreTotalUnits(sizeRid, _sizeAllocatedHash);
            //return units;
            StoreSizeVector storeAllocatedUnits;
            if (_storeAllocated.TryGetValue(sizeRid, out storeAllocatedUnits))
            {
                return (int)storeAllocatedUnits.AllStoreTotalValue;
            }
            return 0;
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }
        // end MID Track 4425 Fill Size Holes Need Phase overallocates size

        // begin TT#1600 - JEllis - Size Need Algorithm Error
        /// <summary>
        /// Gets a stores total allocation in the sizes that are NOT in the store's size curve
        /// </summary>
        /// <param name="aStoreRID">RID identifying the store.</param>
        /// <returns></returns>
        public int GetStoreSizeNotInCurveTotalAllocated(int aStoreRID)
        {
            if (_storeSizeNotInCurveTotalAllocated == null)
            {
                BuildStoreSizeNotInCurveTotalAllocated();
            }
            return (int)_storeSizeNotInCurveTotalAllocated.GetStoreValue(aStoreRID);
        }
        /// <summary>
        /// Adds units to the stores total allocated in sizes that are NOT in the size curve
        /// </summary>
        /// <param name="aStoreRID">RID identifying the store</param>
        /// <param name="aUnits">Units to increment the store's total allocation in the sizes that are not in the curve</param>
        public void AddStoreSizeNotInCurveTotalAllocated(int aStoreRID, int aUnits)
        {
            if (_storeSizeNotInCurveTotalAllocated == null)
            {
                BuildStoreSizeNotInCurveTotalAllocated();
            }
            _storeSizeNotInCurveTotalAllocated.SetStoreValue(aStoreRID, _storeSizeNotInCurveTotalAllocated.GetStoreValue(aStoreRID) + aUnits);
        }
        /// <summary>
        /// Sets the store's total allocated in sizes that are NOT in the size curve
        /// </summary>
        /// <param name="aStoreRID">RID identifying the store</param>
        /// <param name="aUnits">Units allocated in the size that is not in the curve</param>
        public void SetStoreSizeNotInCurveTotalAllocated(int aStoreRID, int aUnits)
        {
            if (_storeSizeNotInCurveTotalAllocated == null)
            {
                BuildStoreSizeNotInCurveTotalAllocated();
            }
            _storeSizeNotInCurveTotalAllocated.SetStoreValue(aStoreRID, aUnits);
        }
        /// <summary>
        /// Builds total allocated by store for the sizes NOT in the store's curve
        /// </summary>
        private void BuildStoreSizeNotInCurveTotalAllocated()
        {
            _storeSizeNotInCurveTotalAllocated = new StoreVector();
            foreach (Profile sp in _storeList)
            {
                List<int> sizesNotInCurve;
                int allocated = 0;
                SizeCurveProfile sizeCurve = (SizeCurveProfile)_storeSizeCurveHash[sp.Key]; // TT#1600 - JEllis - Size Need Overallocates size 
                if (_storeSizesNotInCurveDict.TryGetValue(sizeCurve.Key, out sizesNotInCurve)) // TT#1600 - JEllis - Size Need Overallocates Size
                {
                    foreach (int sizeCodeRID in sizesNotInCurve)
                    {
                        StoreSizeVector ssv;
                        if (_storeAllocated.TryGetValue(sizeCodeRID, out ssv))
                        {
                            allocated += (int)ssv.GetStoreValue(sp.Key);
                        }
                    }
                }
                _storeSizeNotInCurveTotalAllocated.SetStoreValue(sp.Key, allocated);
            }
        }
        // end TT#1600 - JEllis - Size Need Algorithm Error

        // begin TT#2332 - JEllis - Pack Fitting Algorithm Broken
        public int GetSizeNeedUnits(int storeRid, int sizeRid)
        {
            return GetSizeNeedUnits(storeRid, sizeRid, false);
        }
        // end TT#2332 - JEllis - Pack Fitting Algorithm Broken

        /// <summary>
        /// Calulates need units
        /// </summary>
        /// <param name="storeRid">RID identifies store</param>
        /// <param name="sizeRid">RID identifies size</param>
        /// <param name="aBeforeSizeAllocation">True:  Need is calculated as if no size allocation has occurred; False:  Need is calculated using ALL units allocated by size</param>
        /// <returns></returns>
        public int GetSizeNeedUnits(int storeRid, int sizeRid, bool aBeforeSizeAllocation) // TT#1600 - Jellis - Pack Fitting Algorithm Broken
        {
            int planUnits = GetSizeNeed_PlanUnits(storeRid, sizeRid); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
            int onHandUnits = GetOnhandUnits(storeRid, sizeRid);
            onHandUnits += GetVswOnhandUnits(storeRid, sizeRid); // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW onhand
            int intransitUnits = GetIntransitUnits(storeRid, sizeRid);
            // begin TT#1600 - JEllis - Size Need Algorithm Error
            //int allocatedUnits = this.GetPriorAllocatedUnits(storeRid, sizeRid);
            //int allocatedUnits = this.GetAllocatedUnits(storeRid, sizeRid); // TT#2332 - JEllis - Pack Fitting Algorithm Broken
            // end TT#1600 - Jellis - Size Need Algorithm Error

            // begin TT#2332 - JEllis - Pack Fitting Algorithm Broken
            int allocatedUnits;
            if (aBeforeSizeAllocation)
            {
                allocatedUnits =                              // TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max  
                    GetPriorAllocatedUnits(storeRid, sizeRid) // TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max
                    + GetGroupMemberAllocatedUnits(storeRid, sizeRid);  // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            }
            else
            {
                allocatedUnits =                                        // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max   
                    GetAllocatedUnits(storeRid, sizeRid)                // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max  
                    + GetGroupMemberAllocatedUnits(storeRid, sizeRid);  // TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
            }
            // end TT#2332 - JEllis - Pack Fitting Algorithm Broken
            int needUnits = (int)Need.UnitNeed((double)planUnits, (double)onHandUnits, intransitUnits, allocatedUnits);

            Debug.WriteLineIf(storeRid > 22 && storeRid < 26, "GetSizeNeedUnits() ST: " + storeRid + " SZ: " + sizeRid
                + " OH: " + onHandUnits + " IT: " + intransitUnits
                + " ALLOC: " + allocatedUnits
                + " PLAN: " + planUnits
                + " NEED: " + needUnits
                );

            return needUnits;
        }



        public void AddOnhandUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeOnhandHash);
            //_getOnhandAndIntransit = false;
            StoreSizeVector storeOnhandUnits;
            if (!_storeOnhand.TryGetValue(sizeRid, out storeOnhandUnits))
            {
                storeOnhandUnits = new StoreSizeVector(sizeRid);
                _storeOnhand.Add(sizeRid, storeOnhandUnits);
            }
            storeOnhandUnits.SetStoreValue(storeRid, storeOnhandUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }
        public int GetOnhandUnits(int storeRid, int sizeRid)
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storeOnhandUnits;
            if (_storeOnhand.TryGetValue(sizeRid, out storeOnhandUnits))
            {
                return (int)storeOnhandUnits.GetStoreValue(storeRid);
            }
            return 0;
            //int units = GetUnits(storeRid, sizeRid, _storeOnhandHash);
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }

        // begin TT#2313 - JEllis - AnF VSW -- Size allocation not using VSW OH
        public void AddVswOnhandUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storeVswOnhandUnits;
            if (!_storeVswOnhand.TryGetValue(sizeRid, out storeVswOnhandUnits))
            {
                storeVswOnhandUnits = new StoreSizeVector(sizeRid);
                _storeVswOnhand.Add(sizeRid, storeVswOnhandUnits);
            }
            storeVswOnhandUnits.SetStoreValue(storeRid, storeVswOnhandUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
        }
        public int GetVswOnhandUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeVswOnhandUnits;
            if (_storeVswOnhand.TryGetValue(sizeRid, out storeVswOnhandUnits))
            {
                return (int)storeVswOnhandUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // end TT#2313 - JEllis - AnF VSW -- Size allocation not using VSW OH
        // begin TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
        public void AddIbVswOnhandUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storeIbVswOnhandUnits;
            if (!_storeIbVswOnhand.TryGetValue(sizeRid, out storeIbVswOnhandUnits))
            {
                storeIbVswOnhandUnits = new StoreSizeVector(sizeRid);
                _storeIbVswOnhand.Add(sizeRid, storeIbVswOnhandUnits);
            }
            storeIbVswOnhandUnits.SetStoreValue(storeRid, storeIbVswOnhandUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
        }
        public int GetIbVswOnhandUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeIbVswOnhandUnits;
            if (_storeIbVswOnhand.TryGetValue(sizeRid, out storeIbVswOnhandUnits))
            {
                return (int)storeIbVswOnhandUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // end TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
		
        // begin TT#41 - MD - Jellis - Size Inventory Min Max
        public void AddIbOnhandUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storeOnhandUnits;
            if (!_storeIbOnhand.TryGetValue(sizeRid, out storeOnhandUnits))
            {
                storeOnhandUnits = new StoreSizeVector(sizeRid);
                _storeIbOnhand.Add(sizeRid, storeOnhandUnits);
            }
            storeOnhandUnits.SetStoreValue(storeRid, storeOnhandUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
        }
        public int GetIbOnhandUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeOnhandUnits;
            if (_storeIbOnhand.TryGetValue(sizeRid, out storeOnhandUnits))
            {
                return (int)storeOnhandUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // end TT#41 - MD - Jellis - Size Inventory Min Max
         
        public void AddIntransitUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storeIntransitHash);
            //_getOnhandAndIntransit = false;
            StoreSizeVector storeIntransitUnits;
            if (!_storeIntransit.TryGetValue(sizeRid, out storeIntransitUnits))
            {
                storeIntransitUnits = new StoreSizeVector(sizeRid);
                _storeIntransit.Add(sizeRid, storeIntransitUnits);
            }
            storeIntransitUnits.SetStoreValue(storeRid, storeIntransitUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }
        public int GetIntransitUnits(int storeRid, int sizeRid)
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storeIntransitUnits;
            if (_storeIntransit.TryGetValue(sizeRid, out storeIntransitUnits))
            {
                return (int)storeIntransitUnits.GetStoreValue(storeRid);
            }
            return 0;
            //int units = GetUnits(storeRid, sizeRid, _storeIntransitHash);
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }

        // begin TT#41 - MD - JEllis - Size Inventory Min Max
        public void AddIbIntransitUnits(int storeRid, int sizeRid, int units)
        {
             StoreSizeVector storeIntransitUnits;
            if (!_storeIbIntransit.TryGetValue(sizeRid, out storeIntransitUnits))
            {
                storeIntransitUnits = new StoreSizeVector(sizeRid);
                _storeIbIntransit.Add(sizeRid, storeIntransitUnits);
            }
            storeIntransitUnits.SetStoreValue(storeRid, storeIntransitUnits.GetStoreValue(storeRid) + units);
            _getOnhandAndIntransit = false;
        }
        public int GetIbIntransitUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeIntransitUnits;
            if (_storeIbIntransit.TryGetValue(sizeRid, out storeIntransitUnits))
            {
                return (int)storeIntransitUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // end TT#41 - MD - Jellis - Size Inventory Min Max

        // begin TT#1176 - MD - JEllis - Group Allocation Size Need not observing inv min max
        /// <summary>
        /// Accumulates prior group member allocated units (for "other" members of the group)
        /// </summary>
        /// <param name="storeRid">RID of the store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <param name="units">Units allocated</param>
        public void AddGroupMemberAllocatedUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storeGroupMemberAllocatedUnits;
            if (!_storeGroupMemberAllocated.TryGetValue(sizeRid, out storeGroupMemberAllocatedUnits))
            {
                storeGroupMemberAllocatedUnits = new StoreSizeVector(sizeRid);
                _storeGroupMemberAllocated.Add(sizeRid, storeGroupMemberAllocatedUnits);
            }
            storeGroupMemberAllocatedUnits.SetStoreValue(storeRid, storeGroupMemberAllocatedUnits.GetStoreValue(storeRid) + units);
        }
        /// <summary>
        /// Gets Group Member allocated units (from "other" members of trhe group)
        /// </summary>
        /// <param name="storeRid">RID identfying store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <returns>Allocated units</returns>
        public int GetGroupMemberAllocatedUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeGroupMemberAllocatedUnits;
            if (_storeGroupMemberAllocated.TryGetValue(sizeRid, out storeGroupMemberAllocatedUnits))
            {
                return (int)storeGroupMemberAllocatedUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // end TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max

        // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
        /// <summary>
        /// Accumulates prior group member bulk allocated units (for "other" members of the group)
        /// </summary>
        /// <param name="storeRid">RID of the store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <param name="units">Units allocated</param>
        public void AddGroupMemberAllocatedBulkUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storeGroupMemberAllocatedUnits;
            if (!_storeGroupMemberBulkAllocated.TryGetValue(sizeRid, out storeGroupMemberAllocatedUnits))
            {
                storeGroupMemberAllocatedUnits = new StoreSizeVector(sizeRid);
                _storeGroupMemberBulkAllocated.Add(sizeRid, storeGroupMemberAllocatedUnits);
            }
            storeGroupMemberAllocatedUnits.SetStoreValue(storeRid, storeGroupMemberAllocatedUnits.GetStoreValue(storeRid) + units);
        }
        /// <summary>
        /// Gets Group Member allocated bulk units (from "other" members of trhe group)
        /// </summary>
        /// <param name="storeRid">RID identfying store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <returns>Allocated units</returns>
        public int GetGroupMemberAllocatedBulkUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storeGroupMemberAllocatedUnits;
            if (_storeGroupMemberBulkAllocated.TryGetValue(sizeRid, out storeGroupMemberAllocatedUnits))
            {
                return (int)storeGroupMemberAllocatedUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
        // End TT#1828 - MD - JSmith - Size Need not allocatde to size

        /// <summary>
        /// Accumulates prior allocated units (from packs)
        /// </summary>
        /// <param name="storeRid">RID of the store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <param name="units">Units allocated</param>
        public void AddPriorAllocatedUnits(int storeRid, int sizeRid, int units)
        {
            // begin TT#1391 - TMW New Action (Unrelated Performance)
            //AddUnits(storeRid, sizeRid, units, _storePriorAllocatedHash);
            StoreSizeVector storePriorAllocatedUnits;
            if (!_storePriorAllocated.TryGetValue(sizeRid, out storePriorAllocatedUnits))
            {
                storePriorAllocatedUnits = new StoreSizeVector(sizeRid);
                _storePriorAllocated.Add(sizeRid, storePriorAllocatedUnits);
            }
            storePriorAllocatedUnits.SetStoreValue(storeRid, storePriorAllocatedUnits.GetStoreValue(storeRid) + units);
            // end TT#1391 - TMW new Action (Unrelated Performance)
        }
        /// <summary>
        /// Sets prior allocated units (from packs)
        /// </summary>
        /// <param name="storeRid">RID identfying store</param>
        /// <param name="sizeRid">RID identifying size</param>
        /// <returns>Allocated units</returns>
        public int GetPriorAllocatedUnits(int storeRid, int sizeRid)
        {
            // begin TT#1391  - TMW New Action (Unrelated - Performance)
            StoreSizeVector storePriorAllocatedUnits;
            if (_storePriorAllocated.TryGetValue(sizeRid, out storePriorAllocatedUnits))
            {
                return (int)storePriorAllocatedUnits.GetStoreValue(storeRid);
            }
            return 0;

            //int units = GetUnits(storeRid, sizeRid, _storePriorAllocatedHash);
            //return units;
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }

		// Begin TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk
        public void AddPriorDetailAllocatedUnits(int storeRid, int sizeRid, int units)
        {
            StoreSizeVector storePriorDetailAllocatedUnits;
            if (!_storePriorDetailAllocated.TryGetValue(sizeRid, out storePriorDetailAllocatedUnits))
            {
                storePriorDetailAllocatedUnits = new StoreSizeVector(sizeRid);
                _storePriorDetailAllocated.Add(sizeRid, storePriorDetailAllocatedUnits);
            }
            storePriorDetailAllocatedUnits.SetStoreValue(storeRid, storePriorDetailAllocatedUnits.GetStoreValue(storeRid) + units);
        }

        public int GetPriorDetailAllocatedUnits(int storeRid, int sizeRid)
        {
            StoreSizeVector storePriorDetailAllocatedUnits;
            if (_storePriorDetailAllocated.TryGetValue(sizeRid, out storePriorDetailAllocatedUnits))
            {
                return (int)storePriorDetailAllocatedUnits.GetStoreValue(storeRid);
            }
            return 0;
        }
		// End TT#1784-MD - stodd - Pre-Pack Fill Size over allocates bulk on header with pack and bulk

        public void AddTotalPlanUnits(int storeRid, int units) // MID Track 4291 add fill variables to size review
        {
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //_storeTotalPlan.Add(storeRid, units);
            _storeTotalPlan.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        public int GetTotalPlanUnits(int storeRid)  // MID Track 4291 add fill variables to size review
        {
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //int units = 0;
            //if (_storeTotalPlan.ContainsKey(storeRid))
            //    units = (int)_storeTotalPlan[storeRid];
            //return units;
            return (int)_storeTotalPlan.GetStoreValue(storeRid);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        // begin MID track 4291 add fill variables to size review
        // begin MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        public void AddFillToOwn_SzTotUnitPlan(int storeRid, int units) // MID Track 4291 add fill variables to size review  // MID Track 4921 AnF#666 Fill To Size Plan
        {
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //_storeFillToOwn_SzTotPlanHash.Add(storeRid, units); // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            _storeFillToOwn_SzTotPlan.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        public int GetFillToOwn_SzTotUnitPlan(int storeRid)  // MID Track 4291 add fill variables to size review  // MID Track 4921 AnF#666 Fill To Size Plan
        {

            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //int units = 0;
            //// Begin MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //if (_storeFillToOwn_SzTotPlanHash.ContainsKey(storeRid))
            //{
            //    units = (int)_storeFillToOwn_SzTotPlanHash[storeRid];
            //}
            //// end MID Track 4921 AnF#666 Fill To Size Plan Enhancement
            //return units;
            return (int)_storeFillToOwn_SzTotPlan.GetStoreValue(storeRid);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        public void AddFillToPlan_SzTotUnitPlan(int storeRid, int units) // MID Track 4291 add fill variables to size review  // MID Track 4921 AnF#666 Fill To Size Plan
        {
            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //_storeFillToPlan_SzTotPlanHash.Add(storeRid, units); // MID Track 4921 AnF#666 Fill To SIze Plan Enhancement
            _storeFillToPlan_SzTotPlan.SetStoreValue(storeRid, units);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        public int GetFillToPlan_SzTotUnitPlan(int storeRid)  // MID Track 4291 add fill variables to size review  // MID Track 4921 AnF#666 Fill To Size Plan
        {

            // begin TT#1391 - TMW New Action (Unrelated - Performance)
            //int units = 0;
            //if (_storeFillToPlan_SzTotPlanHash.ContainsKey(storeRid))
            //{
            //    units = (int)_storeFillToPlan_SzTotPlanHash[storeRid];
            //}
            //return units;
            return (int)_storeFillToPlan_SzTotPlan.GetStoreValue(storeRid);
            // end TT#1391 - TMW New Action (Unrelated - Performance)
        }
        // end MID Track 4291 add fill variables to size review

        // begin TT#1543 - JEllis - Size Multiple Broken
        ////public int GetStoreMin(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        //public int GetStoreMin(int storeRid, int aSizeCodeRID)          // TT#1391 - TMW New Action
        //{
        //    //int min = _storeSizeConstraints.GetStoreMin(storeRid, aSizeCodeProfile); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
        //    int min = _storeSizeConstraints.GetStoreMin(storeRid, aSizeCodeRID);      // TT#1391 - TMW New Action
        //    return min;
        //}

        // begin TT#246 - MD - JEllis - AnF VSW In Store Minimum pt 4
        // begin TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5
        ///// <summary>
        ///// Gets the inventory minimum for a store (this value is adjusted by the store's ownership).
        ///// </summary>
        ///// <param name="aStoreRID">Ftore RID</param>
        ///// <param name="aSizeCodeRID">Size Code RID</param>
        ///// <returns>Inventory Minimum adjusted by ownership (so the returned minimum is an allocation minimum)</returns>
        //public int GetStoreInventoryMin(int aStoreRID, int aSizeCodeRID)
        //{
        //    return GetStoreInventoryMin(aStoreRID, aSizeCodeRID, false);
        //}
        //// end TT#246 - MD - JEllis - AnF VSW In STore Minimum pt 4
        ///// <summary>
        ///// Gets the inventory minimum for a store (this value is adjusted by the store's ownership).
        ///// </summary>
        ///// <param name="aStoreRID">Ftore RID</param>
        ///// <param name="aSizeCodeRID">Size Code RID</param>
        ///// <param name="aInStoreMinimum">True: return an in-store minimum; False: return inventory minimum adjusted by VSW</param>
        ///// <returns>Inventory Minimum adjusted by ownership (so the returned minimum is an allocation minimum)</returns>
        //public int GetStoreInventoryMin(int aStoreRID, int aSizeCodeRID,bool aInStoreMinimum) // TT#246 - MD - JEllis - AnF VSW In Store Minimum pt 4
        //{

        // begin TT#519 - MD - Jellis - VSW - Minimums not working
        /// <summary>
        /// Gets the  minimum for a store (this value is NOT adjusted by the store's ownership).
        /// </summary>
        /// <param name="aStoreRID">Ftore RID</param>
        /// <param name="aSizeCodeRID">Size Code RID</param>
        /// <returns>Inventory Minimum adjusted by ownership (so the returned minimum is an allocation minimum)</returns> 
        public int GetStoreMinimum(int aStoreRID, int aSizeCodeRID)
        {
            return _storeSizeConstraints.GetStoreMin(aStoreRID, aSizeCodeRID);
        }
        // end TT#519 - MD - Jellis - VSW - Minimums not working




        //Begin TT#3441 -jsobek -New Size Method not producing expected results
        /// <summary>
        /// Does not subtract out OnHand and Instransit when calculating the minimum
        /// </summary>
        /// <param name="aStoreRID"></param>
        /// <param name="aSizeCodeRID"></param>
        /// <returns></returns>
        public int GetStorePresentationMinForSize(int aStoreRID, int aSizeCodeRID)
        {
            int minimum = GetStoreMinimum(aStoreRID, aSizeCodeRID);
            if (minimum < 0)
            {
                minimum = 0;
            }
            int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
            minimum = (int)(((double)minimum / (double)mult) + .5d);
            minimum = minimum * mult;
            return minimum;
        }
        //End TT#3441 -jsobek -New Size Method not producing expected results

        /// <summary>
        /// Gets the inventory minimum for a store (this value is adjusted by the store's ownership).
        /// </summary>
        /// <param name="aStoreRID">Ftore RID</param>
        /// <param name="aSizeCodeRID">Size Code RID</param>
        /// <param name="aAdjust">Flag identifying if the value is to be adjusted</param>
        /// <param name="aAlternateSizeCodes">Collection contaiining alternate size codes</param>
        /// <returns>Inventory Minimum adjusted by ownership (so the returned minimum is an allocation minimum)</returns> 
		// Begin TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
		//public int GetStoreInventoryMin(int aStoreRID, int aSizeCodeRID, bool aAdjust)
        public int GetStoreInventoryMin(int aStoreRID, int aSizeCodeRID, bool aAdjust, ArrayList aAlternateSizeCodes = null)
		// End TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
        {
            // begin TT#519 - MD - Jellis - AnF VSW Minimums not working
            if (_lastMinSizeCodeRID != aSizeCodeRID)
            {
                _lastMinSizeCodeRID = aSizeCodeRID;
                if (!_inventoryMinBySize.TryGetValue(_lastMinSizeCodeRID, out _lastInventoryMinByStore))
                {
                    _lastInventoryMinByStore = new Dictionary<int, int>();
                    _inventoryMinBySize.Add(_lastMinSizeCodeRID, _lastInventoryMinByStore);

                }
            }
            int minimum;
            if (!_lastInventoryMinByStore.TryGetValue(aStoreRID, out minimum))
            {
                //HdrSizeBin sizeBin = _color.GetSizeBin(aSizeCodeRID);  // TT#1621-MD - JSmith - Null Reference in Basis Size Mehtod
                Index_RID storeIdxRID = _allocationProfile.AppSessionTransaction.GetStoreIndexRID(aStoreRID);
 
                minimum =
                   GetStoreMinimum(aStoreRID, aSizeCodeRID)      
                        - Math.Max(GetIbIntransitUnits(aStoreRID, aSizeCodeRID), 0)   // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                        //- Math.Max(GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID), 0)  // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand // TT#304 - MD - JEllis - Size Inventory Min Max Incorrect // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint  // TT#919 - MD - Jellis Subtracting IbVSW Onhand twice
                        - Math.Max(GetIbOnhandUnits(aStoreRID, aSizeCodeRID), 0);  // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                if (_vswSizeConstraints == eVSWSizeConstraints.None
                    && _allocationProfile.HeaderType != eHeaderType.IMO)
                {
                    minimum -=
                        Math.Max(GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID), 0);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations (unrelated - remove extra minus sign) // TT#3883 - MD - JEllis -  Balance With Constraints Gets Wrong Result
                        //GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID);  // TT#919 - MD - Jellis Subtracting IbVSW Onhand twice
                }

                // Begin TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                // if alternate size codes exist, adjust the minimum
                if (aAlternateSizeCodes != null)
                {
                    foreach (SizeCodeProfile scp in aAlternateSizeCodes)
                    {
                        if (scp.Key != aSizeCodeRID)
                        {
                            minimum = minimum
                                    - Math.Max(GetIbIntransitUnits(aStoreRID, scp.Key), 0)
                                    - Math.Max(GetIbOnhandUnits(aStoreRID, scp.Key), 0);
                            if (_vswSizeConstraints == eVSWSizeConstraints.None
                                && _allocationProfile.HeaderType != eHeaderType.IMO)
                            {
                                minimum -=
                                    Math.Max(GetIbVswOnhandUnits(aStoreRID, scp.Key), 0);

                            }
                        }
                    }
                }
				// End TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating

                // begin TT#1176 - MD - Jellis - Size Need not observing Inv Min Max on Group
                if (aAdjust)
                {
                    minimum -= GetStoreInventoryAllocationAdjustment(InventoryMdseBasisRID, aStoreRID, aSizeCodeRID);
                }
                // end TT#1176 - MD - Jellis - Size Need Not observing Inv Min Max on Group
                if (minimum < 0)
                {
                    minimum = 0;
                }
                int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
                minimum = (int)(((double)minimum / (double)mult) + .5d);
                minimum = minimum * mult;
#if (DEBUG)
                //if (aStoreRID > 1 && aStoreRID < 8)
                //{
                //    Debug.WriteLine("GetStoreInventoryMin() STORE RID: " + aStoreRID + " SZ CODE RID: " + aSizeCodeRID
                //        + " MIN: " + GetStoreMinimum(aStoreRID, aSizeCodeRID)
                //        + " IT: " + GetIbIntransitUnits(aStoreRID, aSizeCodeRID)
                //        + " OH: " + GetIbOnhandUnits(aStoreRID, aSizeCodeRID)
                //        + " VSW OH: " + GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID)
                //        + " FINAL MIN: " + minimum
                //        );
                //}
#endif
                _lastInventoryMinByStore.Add(aStoreRID, minimum);
            }
            return minimum;
            //// end TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5
            //if (_lastMinSizeCodeRID != aSizeCodeRID)
            //{
            //    _lastMinSizeCodeRID = aSizeCodeRID;
            //    if (!_inventoryMinBySize.TryGetValue(_lastMinSizeCodeRID, out _lastInventoryMinByStore))
            //    {
            //        _lastInventoryMinByStore = new Dictionary<int, int[]>(); // TT#246 - MD - JEllis - AnF VSW In Store Minimum
            //        _inventoryMinBySize.Add(_lastMinSizeCodeRID, _lastInventoryMinByStore);

            //    }
            //}
            //// begin TT#246 - MD - JEllis - AnF VSW In Store Minimum prt 4 
            ////int min; 
            ////if (!_lastInventoryMinByStore.TryGetValue(aStoreRID, out min)) 
            //// {
            ////    min = _storeSizeConstraints.GetStoreMin(aStoreRID, aSizeCodeRID)
            ////        // begin TT#41 - MD - JEllis - Size Inventory Min Max
            ////            - GetIbIntransitUnits(aStoreRID, aSizeCodeRID)
            ////          - GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID)  // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand // TT#304 - MD - JEllis - Size Inventory Min Max Incorrect
            ////            - GetIbOnhandUnits(aStoreRID, aSizeCodeRID);
            ////        //- GetIntransitUnits(aStoreRID, aSizeCodeRID)
            ////        //- GetOnhandUnits(aStoreRID, aSizeCodeRID);
            ////        // end TT#41 - MD - Jellis - Size Inventory Min Max
            ////    if (min < 0)
            ////    {
            ////        min = 0;
            ////    }
            ////    else
            ////    {
            ////        int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
            ////        min = (int)(((double)min / (double)mult) + .5d);
            ////        min = min * mult; 
            ////    }
            ////    _lastInventoryMinByStore.Add(aStoreRID, min);
            ////}
            ////return min;

            //int[] minimums;   
            //if (!_lastInventoryMinByStore.TryGetValue(aStoreRID, out minimums))  
            //{
            //    minimums = new int[2]; 
            //    minimums[0] = _storeSizeConstraints.GetStoreMin(aStoreRID, aSizeCodeRID)
            //            - GetIbIntransitUnits(aStoreRID, aSizeCodeRID)
            //            - GetIbOnhandUnits(aStoreRID, aSizeCodeRID);
            //    if (minimums[0] < 0)
            //    {
            //        minimums[0] = 0;
            //    }
            //    minimums[1] = minimums[0]
            //             - GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID);
            //    if (minimums[1] < 0)
            //    {
            //        minimums[1] = 0;
            //    }
            //    int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
            //    minimums[0] = (int)(((double)minimums[0] / (double)mult) + .5d);
            //    minimums[0] = minimums[0] * mult; 
            //    minimums[1] = (int)(((double)minimums[1] / (double)mult) + .5d);
            //    minimums[1] = minimums[1] * mult; 
            //    _lastInventoryMinByStore.Add(aStoreRID, minimums);
            //}
            //if (VSWSizeConstraints == eVSWSizeConstraints.None)
            //{
            //    return minimums[1];
            //}
            //else
            //{
            //    return minimums[0];
            //}
            //// end TT#246 - MD - JEllis - AnF VSW In STore Minimum prt 4
            // end TT#519 - MD - Jellis - VSW - Minimums Not Working
        }

        ////public int GetStoreMax(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size need with constraints not allocating correctly // TT#1391 - TMW New Action
        //public int GetStoreMax(int storeRid, int aSizeCodeRID)                    // TT#1391 - TMW New Action 
        //{
        //    //int max = _storeSizeConstraints.GetStoreMax(storeRid, aSizeCodeProfile); // MID Track 3492 Size Need with Constraints not allocating correctly // TT#1391 - TMW New Action
        //    int max = _storeSizeConstraints.GetStoreMax(storeRid, aSizeCodeRID);      // TT#1391 - TMW New Action

        //    return max;
        //}
        /// <summary>
        /// Gets the inventory maximum for a store (this value is adjusted by the store's ownership).
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <param name="aSizeCodeRID">Size Code RID</param>
        /// <param name="aAdjust">True:  adjust maximum by any allocations that have occurred in a Group Allocation; Do not apply any Group Allocation Adjustments</param>
        /// <returns>Inventory Maximum adjusted by ownership (so the returned maximum is an allocation maximum)</returns>
		// Begin TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
		//public int GetStoreInventoryMax(int aStoreRID, int aSizeCodeRID, bool aAdjust)
        public int GetStoreInventoryMax(int aStoreRID, int aSizeCodeRID, bool aAdjust, ArrayList aAlternateSizeCodes = null)
		// End TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
        {
            if (_lastMaxSizeCodeRID != aSizeCodeRID)
            {
                _lastMaxSizeCodeRID = aSizeCodeRID;
                if (!_inventoryMaxBySize.TryGetValue(_lastMaxSizeCodeRID, out _lastInventoryMaxByStore))
                {
                    _lastInventoryMaxByStore = new Dictionary<int,int>();
                    _inventoryMaxBySize.Add(_lastMaxSizeCodeRID, _lastInventoryMaxByStore);
                }
            }

            int max;
            if (!_lastInventoryMaxByStore.TryGetValue(aStoreRID, out max))
            {
                max = _storeSizeConstraints.GetStoreMax(aStoreRID, aSizeCodeRID);
                if (max < int.MaxValue)
                {
                    max = max
                        // begin TT#41 - MD - JEllis - Size Inventory Min Max
                        - Math.Max(GetIbIntransitUnits(aStoreRID, aSizeCodeRID), 0)    // TT#3369 - TMW - Jellis - Size Prop with Constraint not Observing Constraint
                      - Math.Max(GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID), 0)  // TT#2313 - Jellis - AnF VSW -- Size Need not using VSW Onhand // TT#304 - MD - JEllis - Size Inventory Min Max incorrect  // TT#3369 - TMW - Jellis - Size Prop with Constraint not Observing Constraint
                        - Math.Max(GetIbOnhandUnits(aStoreRID, aSizeCodeRID), 0);  // TT#3369 - TMW - Jellis - Size Prop with Constraint not Observing Constraint

                    // Begin TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                    // if alternate size codes exist, adjust the maximum
                    if (aAlternateSizeCodes != null)
                    {
                        foreach (SizeCodeProfile scp in aAlternateSizeCodes)
                        {
                            if (scp.Key != aSizeCodeRID)
                            {
                                max = max
                                        - Math.Max(GetIbIntransitUnits(aStoreRID, scp.Key), 0)
                                        - Math.Max(GetIbVswOnhandUnits(aStoreRID, scp.Key), 0)
                                        - Math.Max(GetIbOnhandUnits(aStoreRID, scp.Key), 0);
                            }
                        }
                    }
                    // End TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                   
                    // begin TT#1176 - MD - Jellis - Size Need not observing Inventory Min/Max in Group Allocaitons
                    if (aAdjust)
                    {
                        max -= GetStoreInventoryAllocationAdjustment(InventoryMdseBasisRID, aStoreRID, aSizeCodeRID);
                    }
                    // end TT#1176 - MD - Jellis - Size need Not observing Inventory Min/Max in Group Allocations

                      //- GetIntransitUnits(aStoreRID, aSizeCodeRID)
                      //- GetOnhandUnits(aStoreRID, aSizeCodeRID);
                        // end TT#41 - MD - Jellis - Size Inventory Min Max
                    if (max < 0)
                    {
                        max = 0;
                    }
                    else
                    {
                        int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
                        max = (int)(((double)max / (double)mult) + .5d);
                        max = max * mult;
                    }
                }
                _lastInventoryMaxByStore.Add(aStoreRID, max);
            }
            return max;
        }
        // end TT#1543 - JEllis - Size Multiple Broken

        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        /// <summary>
        /// Gets the inventory capacity maximum for a store (this value is adjusted by the store's ownership AND capacity).
        /// </summary>
        /// <param name="aStoreRID">Store RID</param>
        /// <param name="aSizeCodeRID">Size Code RID</param>
        /// <param name="aAdjust">True:  adjust maximum by any allocations that have occurred in a Group Allocation; Do not apply any Group Allocation Adjustments</param>
        /// <returns>Inventory Capacity Maximum adjusted by ownership (so the returned maximum is an allocation maximum)</returns>
        // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
		//public int GetStoreInventoryCapacityMax(int aStoreRID, int aSizeCodeRID, bool aAdjust)
        public int GetStoreInventoryCapacityMax(int aStoreRID, int aSizeCodeRID, bool aAdjust, ArrayList altSizeCodeList = null)
		// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
        {
            if (_lastCapMaxSizeCodeRID != aSizeCodeRID)
            {
                _lastCapMaxSizeCodeRID = aSizeCodeRID;
                if (!_inventoryCapMaxBySize.TryGetValue(_lastCapMaxSizeCodeRID, out _lastInventoryCapMaxByStore))
                {
                    _lastInventoryCapMaxByStore = new Dictionary<int, int>();
                    _inventoryCapMaxBySize.Add(_lastCapMaxSizeCodeRID, _lastInventoryCapMaxByStore);
                }
            }

            int max;
            if (!_lastInventoryCapMaxByStore.TryGetValue(aStoreRID, out max))
            {
                max = _storeSizeConstraints.GetStoreCapacityMax(aStoreRID, aSizeCodeRID);
                if (max < int.MaxValue)
                {
                    // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
					//max = max
                    //      - Math.Max(GetIbIntransitUnits(aStoreRID, aSizeCodeRID), 0) // TT#304 - MD - JEllis - Size Inventory Min Max gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                    //      - Math.Max(GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID), 0) // TT#304 - MD - JEllis - Size Inventory Min Max Gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                    //      - Math.Max(GetIbOnhandUnits(aStoreRID, aSizeCodeRID), 0);  // TT#304 - MD - JEllis - Size inventory Min Max gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint 
                    if (altSizeCodeList != null)
                    {
                        foreach (SizeCodeProfile scp in altSizeCodeList)
                        {
                            max = max
                              - Math.Max(GetIbIntransitUnits(aStoreRID, scp.Key), 0) // TT#304 - MD - JEllis - Size Inventory Min Max gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                              - Math.Max(GetIbVswOnhandUnits(aStoreRID, scp.Key), 0) // TT#304 - MD - JEllis - Size Inventory Min Max Gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                              - Math.Max(GetIbOnhandUnits(aStoreRID, scp.Key), 0);  // TT#304 - MD - JEllis - Size inventory Min Max gets incorrect result // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint 
                        }
                    }
                    else
                    {
                        max = max
                              - Math.Max(GetIbIntransitUnits(aStoreRID, aSizeCodeRID), 0)
                              - Math.Max(GetIbVswOnhandUnits(aStoreRID, aSizeCodeRID), 0)
                              - Math.Max(GetIbOnhandUnits(aStoreRID, aSizeCodeRID), 0); 
                    }
					// End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates


                    // begin TT#1176 - MD - Jellis - Size Need not observing Inventory Min/Max in Group Allocaitons
                    if (aAdjust)
                    {
                        // Begin TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                        //max -= GetStoreInventoryAllocationAdjustment(_allocationProfile.CapacityNodeRID, aStoreRID, aSizeCodeRID);
                        if (altSizeCodeList != null)
                        {
                            foreach (SizeCodeProfile scp in altSizeCodeList)
                            {
                                max -= GetStoreInventoryAllocationAdjustment(_allocationProfile.CapacityNodeRID, aStoreRID, scp.Key);
                            }
                        }
                        else
                        {
                            max -= GetStoreInventoryAllocationAdjustment(_allocationProfile.CapacityNodeRID, aStoreRID, aSizeCodeRID);
                        }
                        // End TT#5638 - JSmith - Max Constraints do not hold with Size Alternates
                    }
                    // end TT#1176 - MD - Jellis - Size need Not observing Inventory Min/Max in Group Allocations


                    if (max < 0)
                    {
                        max = 0;
                    }
                    else
                    {
                        int mult = GetStoreMult(aStoreRID, aSizeCodeRID);
                        max = (int)(((double)max / (double)mult) + .5d);
                        max = max * mult;
                    }
                }
                _lastInventoryCapMaxByStore.Add(aStoreRID, max);
            }
            return max;
        }
        // end TT#1543 - JEllis - Size Multiple Broken
        // end TT#2155 - JEllis - Fill Size Holes Null Reference

        // begin TT#1176 MD - Jellis - Size Need not observing Inventory Min/Max criteria for Group Allocations
        /// <summary>
        /// Gets allocations from related headers that will adjust the inventory or capacity minimum or maximum values
        /// </summary>
        /// <param name="aInventoryMdseBasisRID">Inventory or Capacity Merchandise Basis RID</param>
        /// <param name="aStoreRID">RID that identifies the store</param>
        /// <param name="aSizeCodeRID">RID that identifies the size</param>
        /// <returns></returns>
        // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
        //internal int GetStoreInventoryAllocationAdjustment(int aInventoryMdseBasisRID, int aStoreRID, int aSizeCodeRID)
        internal int GetStoreInventoryAllocationAdjustment(int aInventoryMdseBasisRID, int aStoreRID, int aSizeCodeRID, bool aIncludePacks = true, bool aIncludeBulk = true)
        // End TT#1828 - MD - JSmith - Size Need not allocatde to size
        {
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in GetStoreInventoryAllocationAdjustment()");
            }
            #endif
            // End TT#4988 - BVaughan - Performance
            int allocationAdjustment = 0;
            // Begin TT#4988 - BVaughan - Performance
            //if (!(_allocationProfile is AssortmentProfile)
            //    && _allocationProfile.AssortmentProfile != null)
            if (!(_allocationProfile.isAssortmentProfile)
                  && _allocationProfile.AssortmentProfile != null)
            // End TT#4988 - BVaughan - Performance
            {
                AllocationProfile[] apList = _allocationProfile.AssortmentProfile.AssortmentMembers;
                int k = 0;
                int[] inventoryBasisRIDs;
                Index_RID storeIdxRID = _allocationProfile.StoreIndex(aStoreRID);
                HdrSizeBin sizeBin;
                if (aInventoryMdseBasisRID > 0)
                {
                    foreach (AllocationProfile ap in apList)
                    {
                        if (ap.Key != _allocationProfile.Key)
                        {
                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            if (aIncludePacks)  
                            {
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
                                foreach (PackHdr pack in ap.Packs.Values)
                                {
                                    foreach (PackColorSize pcs in pack.PackColors.Values)
                                    {
                                        inventoryBasisRIDs = _allocationProfile.AssortmentProfile.GetInventoryUpdateList(ap.StyleHnRID, pcs.ColorCodeRID, true);
                                        for (k = 0; k < inventoryBasisRIDs.Length; k++)
                                        {
                                            if (inventoryBasisRIDs[k] == aInventoryMdseBasisRID)
                                            {
                                                foreach (PackContentBin pSizeBin in pcs.ColorSizes.Values)
                                                {
                                                    if (pSizeBin.ContentCodeRID == aSizeCodeRID)
                                                    {
                                                        allocationAdjustment +=
                                                            ap.GetStoreQtyAllocated(pack, storeIdxRID)
                                                            * pSizeBin.ContentUnits;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            }
                            if (aIncludeBulk)
                            {
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
                                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                                {
                                    sizeBin = (HdrSizeBin)hcb.ColorSizes[aSizeCodeRID];
                                    if (sizeBin != null)
                                    {
                                        inventoryBasisRIDs = _allocationProfile.AssortmentProfile.GetInventoryUpdateList(ap.StyleHnRID, hcb.ColorCodeRID, true);
                                        for (k = 0; k < inventoryBasisRIDs.Length; k++)
                                        {
                                            if (inventoryBasisRIDs[k] == aInventoryMdseBasisRID)
                                            {
                                                allocationAdjustment += ap.GetStoreQtyAllocated(sizeBin, storeIdxRID);
                                                break;
                                            }
                                        }
                                    }
                                }
                            // Begin TT#1828 - MD - JSmith - Size Need not allocatde to size
                            }
                            // End TT#1828 - MD - JSmith - Size Need not allocatde to size
                        }
                    }
                }
            }
            return allocationAdjustment;
        }
        // end TT#1176 MD - Jellis - Size Need not observing Inventory Min/Max criteria for Group Allocations

        //public int GetStoreMult(int storeRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with Constraints not allocating correctly // TT#1391 - TMW New Action
        public int GetStoreMult(int storeRid, int aSizeCodeRID)                 // TT#1391 - TMW New Action
        {
            //int mult = _storeSizeConstraints.GetStoreMult(storeRid, aSizeCodeProfile); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
            int mult = _storeSizeConstraints.GetStoreMult(storeRid, aSizeCodeRID);       // TT#1391 - TMW New Action

            return mult;
        }



        public void AddSizeCurve(int storeRid, SizeCurveProfile aSizeCurve)
        {
            try
            {
                _storeSizeCurveHash.Add(storeRid, aSizeCurve);
                // Set Default size curve with Alternates applied
                if (_defaultSizeCurve.Key == Include.NoRID)
                {
                    if (aSizeCurve.Key == SizeCurveGroup.DefaultSizeCurve.Key)
                    {
                        _defaultSizeCurve = aSizeCurve;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddRestrictedSizeCurve(int storeRid, SizeCurveProfile aSizeCurve)
        {
            try
            {
                _storeRestrictedSizeCurveHash.Add(storeRid, aSizeCurve);
                // Set Default size curve with Alternates applied
                if (_defaultRestrictedSizeCurve.Key == Include.NoRID)
                {
                    if (aSizeCurve.Key == SizeCurveGroup.DefaultSizeCurve.Key)
                    {
                        _defaultRestrictedSizeCurve = aSizeCurve;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public SizeCurveProfile GetSizeCurve(int storeRid)
        {
            try
            {
                SizeCurveProfile scp = null;
                if (_storeSizeCurveHash.ContainsKey(storeRid))
                {
                    scp = (SizeCurveProfile)_storeSizeCurveHash[storeRid];

                }
                else
                {
                    if (_defaultSizeCurve.Key == Include.NoRID)
                        scp = SizeCurveGroup.DefaultSizeCurve;
                    else
                        scp = _defaultSizeCurve;
                }
                return scp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SizeCurveProfile GetRestrictedSizeCurve(int storeRid)
        {
            try
            {
                SizeCurveProfile scp = null;
                if (_storeRestrictedSizeCurveHash.ContainsKey(storeRid))
                {
                    scp = (SizeCurveProfile)_storeRestrictedSizeCurveHash[storeRid];

                }
                else
                {
                    if (_defaultRestrictedSizeCurve.Key == Include.NoRID)
                        scp = SizeCurveGroup.DefaultSizeCurve;
                    else
                        scp = _defaultRestrictedSizeCurve;
                }
                return scp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SizeCurveProfile GetOriginalSizeCurve(int storeRid)
        {
            try
            {
                SizeCurveProfile scp = _sizeCurveGroup.GetStoreSizeCurveProfile(storeRid);
                return scp;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement--remove Debug code
        //public void DebugResults()
        //{
        //	Debug.WriteLine("====================================================================");
        //	Debug.WriteLine(" DEBUG SIZE NEED RESULTS  " + DateTime.Now.ToString());
        //	Debug.WriteLine("====================================================================");
        //
        //	foreach (StoreProfile sp in _storeList)		
        //	{
        //		DebugStore(sp.Key);
        //	}
        //}
        //
        //  
        //public void DebugStore(int storeRid)
        //{
        //	float totalPct = 0.0f;
        //
        //	SizeCurveProfile scp = _sizeCurveGroup.GetStoreSizeCurveProfile(storeRid);
        //
        //
        //	foreach(SizeCodeProfile sizeCode in scp.SizeCodeList.ArrayList)
        //	{
        //		totalPct += sizeCode.SizeCodePercent;
        //	}
        //
        //
        //	//Debug.WriteLine("Store Key: " + storeRid.ToString() + " init plan: " + GetInitialPlanUnits(storeRid).ToString() + " total plan: " + 
        //	//	GetTotalPlanUnits(storeRid).ToString() + " total pct: " + totalPct.ToString()); // MID Track 4291 Add Fill Variables to Size Review
        //
        //
        //	foreach(SizeCodeProfile sizeCode in scp.SizeCodeList.ArrayList)
        //	{
        //		string sizeName = sizeCode.SizeCodePrimary + "/" + sizeCode.SizeCodeSecondary;
        //		Debug.WriteLine(sizeName + " pct: " + sizeCode.SizeCodePercent.ToString() +
        //			" prev: " + this.GetPriorAllocatedUnits(storeRid, sizeCode.Key).ToString() +
        //			", OnH: " + this.GetOnhandUnits(storeRid, sizeCode.Key).ToString() +
        //			", InT: " + this.GetIntransitUnits(storeRid, sizeCode.Key).ToString() +
        //			", Plan: " + this.GetSizeNeed_PlanUnits(storeRid, sizeCode.Key).ToString() +  // MID Track 4921 AnF#666 Fill To Size Plan Enhancement
        //			", units: " + this.GetAllocatedUnits(storeRid, sizeCode.Key).ToString() );
        //
        //	}
        //}
        // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement--remove Debug code


        // begin MID Track 3781 Size Curve not required
        /// <summary>
        /// Adds size rule type to hashtable for store and size
        /// </summary>
        /// <param name="aStoreRID">RID identifying store</param>
        /// <param name="aSizeRID">RID identifying size</param>
        /// <param name="aSizeRule">Store size rule</param>
        public void AddStoreSizeRule(int aStoreRID, int aSizeRID, eSizeRuleType aSizeRule)
        {
            long storeSizeID = ((long)aStoreRID << 32) + (long)aSizeRID;
            // begin TT#1391 - New Action (Unrelated Performance)
            //_storeRuleHash.Add(storeSizeID, aSizeRule); 
            _storeRule.Add(storeSizeID, aSizeRule);
            // end TT#1391 - New Action (Unrelated Performance
        }

        /// <summary>
        /// Gets the assigned size rule for the store and size
        /// </summary>
        /// <param name="aStoreRID">RID identifying the store</param>
        /// <param name="aSizeRID">RID identifying the size</param>
        /// <returns>eSizeRuleType for the store and size; NULL indicates no rule assigned</returns>
        public eSizeRuleType GetStoreSizeRule(int aStoreRID, int aSizeRID)
        {
            long storeSizeID = ((long)aStoreRID << 32) + (long)aSizeRID;
            // begin TT#1391 - New Action (Unrelated Performance)
            //if (_storeRuleHash.ContainsKey(storeSizeID))
            //{
            //    return (eSizeRuleType)_storeRuleHash[storeSizeID];
            //}
            eSizeRuleType sizeRule;
            if (_storeRule.TryGetValue(storeSizeID, out sizeRule))
            {
                return sizeRule;
            }
            // end TT#1391 - New Action (unrelated Performance)
            return eSizeRuleType.None;
        }
        // end MID Track 3781 Size Curve not required
        // begin TT#1391 - TMW New Action (unrelated - Performance)
    }
}
        //==========================================================
		// private methods
		//==========================================================
        // begin TT#1391 - TMW New Action (unrelated - Performance)
        ///// <summary>
        ///// Adds units to units already there
        ///// </summary>
        ///// <param name="storeRid"></param>
        ///// <param name="sizeRid"></param>
        ///// <param name="aUnits"></param>
        ///// <param name="aHashtable"></param>
        //private void AddUnits(int storeRid, int sizeRid, int aUnits, Hashtable aHashtable) // MID track 4921 AnF#666 Fill to Size Plan Enhancement (Performance change)
        //{
        //    // begin MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance change)
        //    //try
        //    //{
        //    //	if (!aHashtable.ContainsKey(storeRid))
        //    //	{
        //    //		Hashtable ht = new Hashtable();
        //    //		ht.Add(sizeRid, units);
        //    //		aHashtable.Add(storeRid, ht);
        //    //	}
        //    //	else
        //    //	{
        //    //		Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    //		if (ht.ContainsKey(sizeRid))
        //    //		{
        //    //			// Changed this to continually add units
        //    //			// instead of replacing units value with new one.
        //    //			int prevUnits = (int)ht[sizeRid];
        //    //			ht.Remove(sizeRid);
        //    //			units += prevUnits;
        //    //		}
        //    //		ht.Add(sizeRid, units);
        //    //	}
        //    //}
        //    //catch (Exception)
        //    //{
        //    //	throw;
        //    //}
        //    Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    int units = aUnits;
        //    if (units != 0)
        //    {
        //        int prevUnits = 0;
        //        if (ht == null)
        //        {
        //            ht = new Hashtable();
        //            aHashtable.Add(storeRid, ht);
        //        }
        //        try
        //        {
        //            ht.Add(sizeRid, units);
        //        }
        //        catch(ArgumentException)
        //        {
        //            prevUnits = (int)ht[sizeRid];
        //            ht.Remove(sizeRid);
        //            ht.Add(sizeRid, units + prevUnits);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    // end MID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance change)
        //}

        ///// <summary>
        ///// Sets the units to the desired value
        ///// </summary>
        ///// <param name="storeRid"></param>
        ///// <param name="sizeRid"></param>
        ///// <param name="units"></param>
        ///// <param name="aHashtable"></param>
        //private void SetUnits(int storeRid, int sizeRid, int units, Hashtable aHashtable)
        //{
        //    // begin MID track 4921 AnF#666 Fill To Size Plan Enhancement (Performance)
        //    //try
        //    //{
        //    //	if (!aHashtable.ContainsKey(storeRid))
        //    //	{
        //    //		Hashtable ht = new Hashtable();
        //    //		ht.Add(sizeRid, units);
        //    //		aHashtable.Add(storeRid, ht);
        //    //	}
        //    //	else
        //    //	{
        //    //		Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    //		if (ht.ContainsKey(sizeRid))
        //    //		{
        //    //			//int prevUnits = (int)ht[sizeRid];
        //    //			ht.Remove(sizeRid);
        //    //			//units += prevUnits;
        //    //		}
        //    //		ht.Add(sizeRid, units);
        //    //	}
        //    //}
        //    //catch (Exception)
        //    //{
        //    //	throw;
        //    //}
        //    Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    if (ht == null)
        //    {
        //        ht = new Hashtable();
        //        aHashtable.Add(storeRid, ht);
        //    }
        //    try
        //    {
        //        ht.Add(sizeRid, units);
        //    }
        //    catch (ArgumentException)
        //    {
        //        ht.Remove(sizeRid);
        //        ht.Add(sizeRid, units);
        //    }
        //    catch            // no exceptions thrown if removal key does not exist
        //    {
        //        throw;
        //    }
        //    // end MID track 4921 AnF#666 Fill To Size Plan Enhancement (Performance)
        //}

        //private int GetUnits(int storeRid, int sizeRid, Hashtable aHashtable)
        //{
        //    // beginMID Track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
        //    //try
        //    //{
        //    //	int units = 0;
        //    //	if (aHashtable.ContainsKey(storeRid))
        //    //	{
        //    //		Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    //		if (ht.ContainsKey(sizeRid))
        //    //		{
        //    //			units = (int)ht[sizeRid];
        //    //		}
        //    //	}
        //    //	return units;
        //    //}
        //    //catch (Exception)
        //    //{
        //    //	throw;
        //    //}
        //    Hashtable ht = (Hashtable)aHashtable[storeRid];
        //    int units = 0;
        //    if (ht != null)
        //    {
        //        try 
        //        {
        //            units = (int)ht[sizeRid];
        //        }
        //        catch(NullReferenceException)
        //        {
        //            units = 0;
        //        }
        //        catch(IndexOutOfRangeException)
        //        {
        //            units = 0;
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    return units;
        //    // end MID track 4921 AnF#666 Fill to Size Plan Enhancement (Performance)
        //}

        //// begin MID Track 4425 Fill Size Holes Need Phase overallocates size
        ///// <summary>
        ///// Increment All Store Total Size Units
        ///// </summary>
        ///// <param name="aSizeRID">Size RID</param>
        ///// <param name="aUnits">All Store Total Units to add to existing All Store total</param>
        ///// <param name="aHashtable">Relevant Size Hashtable</param>
        //private void AddAllStoreTotalUnits(int aSizeRID, int aUnits, Hashtable aHashtable)
        //{
        //    int units;
        //    try
        //    {
        //        units = (int)aHashtable[aSizeRID];
        //        aHashtable.Remove(aSizeRID);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        units = 0;
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //        units = 0;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    units += aUnits;
        //    aHashtable.Add(aSizeRID, units);
        //}

        ///// <summary>
        ///// Sets the All Store Total units to the desired value
        ///// </summary>
        ///// <param name="aSizeRID">Size RID</param>
        ///// <param name="aUnits">All Store Total Units</param>
        ///// <param name="aHashtable">Relevant Size Hashtable</param>
        //private void SetAllStoreTotalUnits(int aSizeRID, int aUnits, Hashtable aHashtable)
        //{
        //    try
        //    {
        //        aHashtable.Remove(aSizeRID);
        //    }
        //    catch (NullReferenceException)
        //    {
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    aHashtable.Add(aSizeRID, aUnits);
        //}
        ///// <summary>
        ///// Gets All Store Total Size Units
        ///// </summary>
        ///// <param name="aSizeRID">Size RID</param>
        ///// <param name="aHashtable">Relevant Size Hashtable</param>
        ///// <returns>Total Size Units</returns>
        //private int GetAllStoreTotalUnits(int aSizeRID, Hashtable aHashtable)
        //{
        //    try
        //    {
        //        return (int)aHashtable[aSizeRID];
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return 0;
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //// end MID Track 4425 Fill Size Holes Need Phase overallocates size

        //public void ClearHashtable(Hashtable aHashtable)
        //{	
        //    try
        //    {
        //        // Clear Store Plan Hash
        //        IDictionaryEnumerator minEnumerator = aHashtable.GetEnumerator();
        //        while ( minEnumerator.MoveNext() )
        //        {
        //            Hashtable ht = (Hashtable)minEnumerator.Value;
        //            ht.Clear();
        //        }
        //        aHashtable.Clear();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

//    }
//}
        // end TT#1391 - TMW New Action (Unrelated Performance)
