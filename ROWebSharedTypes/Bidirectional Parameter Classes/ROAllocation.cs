using MIDRetail.DataCommon;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROAttributeSetAllocationStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROAttributeSetAllocationStoreGrade
    {
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> AttributeSet { get; set; }
        [DataMember(IsRequired = true)]
        public List<ROAllocationStoreGrade> StoreGrades { get; set; }

        public ROAttributeSetAllocationStoreGrade()
        {
            StoreGrades = new List<ROAllocationStoreGrade>();
        }
    }

    [DataContract(Name = "ROAllocationReviewSelectionProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewSelectionProperties
    {

        [DataMember(IsRequired = true)]
        private List<ROAllocationReviewSelectionBasis> _allocationReviewSelectionBasis;
        [DataMember(IsRequired = true)]
        private List<ROAllocationReviewSelectionGridData> _allocationReviewSelectionGridData;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _filters;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeProfileValue;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _groupbyValue;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeAttributeValue;
        [DataMember(IsRequired = true)]
        bool _chkIneligibleStore;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dateRangeBeginRID;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dateRangeEndRID;
        [DataMember(IsRequired = true)]
        string _needAnalysisBasis;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeCurveValue;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _views;

        [DataMember(IsRequired = true)]
        eAllocationSelectionViewType _viewType;

        #region Public Properties
        public List<ROAllocationReviewSelectionBasis> ReviewSelectionBasis
        {
            get { return _allocationReviewSelectionBasis; }
            set { _allocationReviewSelectionBasis = value; }
        }
        public List<ROAllocationReviewSelectionGridData> ReviewSelectionGridData
        {
            get { return _allocationReviewSelectionGridData; }
            set { _allocationReviewSelectionGridData = value; }
        }
        public KeyValuePair<int, string> Filters
        {
            get { return _filters; }
            set { _filters = value; }
        }
        public KeyValuePair<int, string> StoreProfileList
        {
            get { return _storeProfileValue; }
            set { _storeProfileValue = value; }
        }

        public KeyValuePair<int, string> GroupbyList
        {
            get { return _groupbyValue; }
            set { _groupbyValue = value; }
        }
        public KeyValuePair<int, string> StoreAttributeList
        {
            get { return _storeAttributeValue; }
            set { _storeAttributeValue = value; }
        }

        public KeyValuePair<int, string> Views
        {
            get { return _views; }
            set { _views = value; }
        }
        public bool ChkIneligibleStore
        {
            get { return _chkIneligibleStore; }
            set { _chkIneligibleStore = value; }
        }

        public KeyValuePair<int, string> DateRangeBeginRID
        {
            get { return _dateRangeBeginRID; }
            set { _dateRangeBeginRID = value; }
        }
        public KeyValuePair<int, string> DateRangeEndRID
        {
            get { return _dateRangeEndRID; }
            set { _dateRangeEndRID = value; }
        }
        public string NeedAnalysisBasis
        {
            get { return _needAnalysisBasis; }
            set { _needAnalysisBasis = value; }
        }

        public KeyValuePair<int, string> SizeCurve
        {
            get { return _sizeCurveValue; }
            set { _sizeCurveValue = value; }
        }

        public eAllocationSelectionViewType ViewType
        {
            get { return _viewType; }
            set
            {
                _viewType = value;
            }
        }
        #endregion
        public ROAllocationReviewSelectionProperties(eAllocationSelectionViewType viewType, List<ROAllocationReviewSelectionBasis> allocationReviewSelectionBasis,
                                                     List<ROAllocationReviewSelectionGridData> allocationReviewSelectionGridData,
                                                    KeyValuePair<int, string> filters, KeyValuePair<int, string> storeProfileValue,
                                                    KeyValuePair<int, string> groupbyValue, KeyValuePair<int, string> storeAttributeValue,
                                                    KeyValuePair<int, string> views, bool chkIneligibleStore,
                                                    KeyValuePair<int, string> dateRangeBeginRID, KeyValuePair<int, string> dateRangeEndRID,
                                                    string needAnalysisBasis,
                                                    KeyValuePair<int, string> sizeCurveValue)
        {

            _allocationReviewSelectionBasis = allocationReviewSelectionBasis;
            _allocationReviewSelectionGridData = allocationReviewSelectionGridData;
            _filters = filters;
            _storeProfileValue = storeProfileValue;
            _groupbyValue = groupbyValue;
            _storeAttributeValue = storeAttributeValue;
            _chkIneligibleStore = chkIneligibleStore;
            _dateRangeBeginRID = dateRangeBeginRID;
            _dateRangeEndRID = dateRangeEndRID;
            _needAnalysisBasis = needAnalysisBasis;
            _sizeCurveValue = sizeCurveValue;
             _viewType = viewType;
            _views = views;
        }

    }

    [DataContract(Name = "ROMethodGeneralAllocationProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodGeneralAllocationProperties : ROMethodProperties
    {
        // fields specific to General Allocation method 
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _begin_CDR;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _shipTo_CDR;
        [DataMember(IsRequired = true)]
        bool _percentInd;
        [DataMember(IsRequired = true)]
        double _reserve;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merch_HN;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _merch_PH;
        [DataMember(IsRequired = true)]
        eMerchandiseType _merchandiseType;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _hierarchyLevels;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _genAlloc_HDR;
        [DataMember(IsRequired = true)]
        double _reserveAsBulk;
        [DataMember(IsRequired = true)]
        double _reserveAsPacks;

        #region Public Properties
        public KeyValuePair<int, string> Begin_CDR
        {
            get { return _begin_CDR; }
            set { _begin_CDR = value; }
        }
        public KeyValuePair<int, string> ShipTo_CDR
        {
            get { return _shipTo_CDR; }
            set { _shipTo_CDR = value; }
        }

        public bool PercentInd
        {
            get { return _percentInd; }
            set { _percentInd = value; }
        }
        public double Reserve
        {
            get { return _reserve; }
            set { _reserve = value; }
        }

        public KeyValuePair<int, string> Merch_HN
        {
            get { return _merch_HN; }
            set { _merch_HN = value; }
        }
        public KeyValuePair<int, int> Merch_PH
        {
            get { return _merch_PH; }
            set { _merch_PH = value; }
        }
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set
            {
                _merchandiseType = value;
            }
        }

        public KeyValuePair<int, string> GenAlloc_HDR
        {
            get { return _genAlloc_HDR; }
            set { _genAlloc_HDR = value; }
        }
        public double ReserveAsBulk
        {
            get { return _reserveAsBulk; }
            set { _reserveAsBulk = value; }
        }

        public double ReserveAsPacks
        {
            get { return _reserveAsPacks; }
            set { _reserveAsPacks = value; }
        }

        /// <summary>
        /// Gets the list of hierarchy levels
        /// </summary>
        public List<KeyValuePair<int, string>> HierarchyLevels
        {
            get { return _hierarchyLevels; }
            set { _hierarchyLevels = value; }
        }
        #endregion
        public ROMethodGeneralAllocationProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> begin_CDR, 
            KeyValuePair<int, string> shipTo_CDR, 
            bool percentInd,
            double reserve, 
            KeyValuePair<int, string> merch_HN, 
            int merch_PH_RID, 
            int merch_PHL_SEQ, 
            eMerchandiseType merchandiseType, 
            KeyValuePair<int, string> genAlloc_HDR,
            double reserveAsBulk, 
            double reserveAsPacks,
            bool isTemplate = false) 
            : base(
                eMethodType.GeneralAllocation,
                method, 
                description, 
                userKey,
                isTemplate
                )

        {
            // fields specific to General Allocation method
            _begin_CDR = begin_CDR;
            _shipTo_CDR = shipTo_CDR;
            _percentInd = percentInd;
            _reserve = reserve;
            _merch_HN = merch_HN;
            _merch_PH = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            _merchandiseType = merchandiseType;
            _genAlloc_HDR = genAlloc_HDR;
            _reserveAsBulk = reserveAsBulk;
            _reserveAsPacks = reserveAsPacks;
            _hierarchyLevels = new List<KeyValuePair<int, string>>();
        }
    }

    [DataContract(Name = "ROMethodFillSizeHolesProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodFillSizeHolesProperties : ROMethodProperties
    {
        // fields specific to Fill Size Holes method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;
        [DataMember(IsRequired = true)]
        double _available;
        [DataMember(IsRequired = true)]
        bool _percentInd;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merch_HN;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _merch_PH;
        [DataMember(IsRequired = true)]
        eMerchandiseType _merchandiseType;
        [DataMember(IsRequired = true)]
        bool _normalizeSizeCurvesDefaultIsOverridden;
        [DataMember(IsRequired = true)]
        bool _normalizeSizeCurves;
        [DataMember(IsRequired = true)]
        eFillSizesToType _fillSizesToType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeAlternateModel;
        [DataMember(IsRequired = true)]
        ROSizeCurveProperties _rOSizeCurveProperties;
        [DataMember(IsRequired = true)]
        ROSizeConstraintProperties _rOSizeConstraintProperties;
        [DataMember(IsRequired = true)]
        bool _overrideVSWSizeConstraints;
        [DataMember(IsRequired = true)]
        eVSWSizeConstraints _vSWSizeConstraints;
        [DataMember(IsRequired = true)]
        bool _overrideAvgPackDevTolerance;
        [DataMember(IsRequired = true)]
        double _avgPackDeviationTolerance;
        [DataMember(IsRequired = true)]
        bool _overrideMaxPackNeedTolerance;
        [DataMember(IsRequired = true)]
        bool _packToleranceStepped;
        [DataMember(IsRequired = true)]
        bool _packToleranceNoMaxStep;
        [DataMember(IsRequired = true)]
        double _maxPackNeedTolerance;
        //Rule tab properties
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        ROMethodSizeRuleProperties _sizeRuleProperties;


        #region Public Properties

        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public double Available
        {
            get { return _available; }
            set { _available = value; }
        }

        public bool PercentInd
        {
            get { return _percentInd; }
            set { _percentInd = value; }
        }

        public KeyValuePair<int, string> Merch_HN
        {
            get { return _merch_HN; }
            set { _merch_HN = value; }
        }
        public KeyValuePair<int, int> Merch_PH
        {
            get { return _merch_PH; }
            set { _merch_PH = value; }
        }

        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set
            {
                _merchandiseType = value;
            }
        }

        public bool NormalizeSizeCurvesDefaultIsOverridden
        {
            get { return _normalizeSizeCurvesDefaultIsOverridden; }
            set { _normalizeSizeCurvesDefaultIsOverridden = value; }
        }

        public bool NormalizeSizeCurves
        {
            get { return _normalizeSizeCurves; }
            set { _normalizeSizeCurves = value; }
        }

        public eFillSizesToType FillSizesToType
        {
            get { return _fillSizesToType; }
            set
            {
                _fillSizesToType = value;
            }
        }

        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        public KeyValuePair<int, string> SizeAlternateModel
        {
            get { return _sizeAlternateModel; }
            set { _sizeAlternateModel = value; }
        }
        public ROSizeCurveProperties ROSizeCurveProperties
        {
            get { return _rOSizeCurveProperties; }
            set { _rOSizeCurveProperties = value; }
        }
        public ROSizeConstraintProperties ROSizeConstraintProperties
        {
            get { return _rOSizeConstraintProperties; }
            set { _rOSizeConstraintProperties = value; }
        }
        public bool OverrideVSWSizeConstraints
        {
            get { return _overrideVSWSizeConstraints; }
            set { _overrideVSWSizeConstraints = value; }
        }

        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set
            {
                _vSWSizeConstraints = value;
            }
        }
        public bool OverrideAvgPackDevTolerance
        {
            get { return _overrideAvgPackDevTolerance; }
            set { _overrideAvgPackDevTolerance = value; }
        }
        public double AvgPackDeviationTolerance
        {
            get { return _avgPackDeviationTolerance; }
            set { _avgPackDeviationTolerance = value; }
        }
        public bool OverrideMaxPackNeedTolerance
        {
            get { return _overrideMaxPackNeedTolerance; }
            set { _overrideMaxPackNeedTolerance = value; }
        }
        public bool PackToleranceStepped
        {
            get { return _packToleranceStepped; }
            set { _packToleranceStepped = value; }
        }
        public bool PackToleranceNoMaxStep
        {
            get { return _packToleranceNoMaxStep; }
            set { _packToleranceNoMaxStep = value; }
        }
        public double MaxPackNeedTolerance
        {
            get { return _maxPackNeedTolerance; }
            set { _maxPackNeedTolerance = value; }
        }
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        public ROMethodSizeRuleProperties SizeRuleProperties
        {
            get { return _sizeRuleProperties; }
            set { _sizeRuleProperties = value; }
        }

        #endregion
        public ROMethodFillSizeHolesProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> filter,
            double available, 
            bool percentInd, 
            KeyValuePair<int, string> merch_HN, 
            int merch_PH_RID, 
            int merch_PHL_SEQ, 
            eMerchandiseType merchandiseType,
            bool normalizeSizeCurvesDefaultIsOverridden, 
            bool normalizeSizeCurves, 
            eFillSizesToType fillSizesToType, 
            KeyValuePair<int, string> sizeGroup,
            KeyValuePair<int, string> sizeAlternateModel, 
            ROSizeCurveProperties rOSizeCurveProperties, 
            ROSizeConstraintProperties rOSizeConstraintProperties,
            bool overrideVSWSizeConstraints, 
            eVSWSizeConstraints vSWSizeConstraints, 
            bool overrideAvgPackDevTolerance, 
            double avgPackDeviationTolerance,
            bool overrideMaxPackNeedTolerance, 
            bool packToleranceStepped, 
            bool packToleranceNoMaxStep, 
            double maxPackNeedTolerance,
            KeyValuePair<int, string> attribute, 
            ROMethodSizeRuleProperties sizeRuleProperties,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.FillSizeHolesAllocation, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Fill Size Holes method
            _filter = filter;
            _available = available;
            _percentInd = percentInd;
            _merch_HN = merch_HN;
            _merch_PH = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            _merchandiseType = merchandiseType;
            _normalizeSizeCurvesDefaultIsOverridden = normalizeSizeCurvesDefaultIsOverridden;
            _normalizeSizeCurves = normalizeSizeCurves;
            _fillSizesToType = fillSizesToType;
            _sizeGroup = sizeGroup;
            _sizeAlternateModel = sizeAlternateModel;
            _rOSizeCurveProperties = rOSizeCurveProperties;
            _rOSizeConstraintProperties = rOSizeConstraintProperties;
            _overrideVSWSizeConstraints = overrideVSWSizeConstraints;
            _vSWSizeConstraints = vSWSizeConstraints;
            _overrideAvgPackDevTolerance = overrideAvgPackDevTolerance;
            _avgPackDeviationTolerance = avgPackDeviationTolerance;
            _overrideMaxPackNeedTolerance = overrideMaxPackNeedTolerance;
            _packToleranceStepped = packToleranceStepped;
            _packToleranceNoMaxStep = packToleranceNoMaxStep;
            _maxPackNeedTolerance = maxPackNeedTolerance;
            _attribute = attribute;
            _sizeRuleProperties = sizeRuleProperties;
        }
    }
    [DataContract(Name = "ROMethodSizeNeedProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeNeedProperties : ROMethodProperties
    {
        // fields specific to Size Need Method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _merchandiseHierarchy;
        [DataMember(IsRequired = true)]
        eMerchandiseType _merchandiseType;
        [DataMember(IsRequired = true)]
        bool _normalizeSizeCurvesDefaultIsOverridden;
        [DataMember(IsRequired = true)]
        bool _normalizeSizeCurves;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeAlternateModel;
        [DataMember(IsRequired = true)]
        ROSizeCurveProperties _rOSizeCurveProperties;
        [DataMember(IsRequired = true)]
        ROSizeConstraintProperties _rOSizeConstraintProperties;
        [DataMember(IsRequired = true)]
        bool _overrideVSWSizeConstraints;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _vSWSizeConstraints;
        [DataMember(IsRequired = true)]
        bool _overrideAvgPackDevTolerance;
        [DataMember(IsRequired = true)]
        double _avgPackDeviationTolerance;
        [DataMember(IsRequired = true)]
        bool _overrideMaxPackNeedTolerance;
        [DataMember(IsRequired = true)]
        bool _packToleranceStepped;
        [DataMember(IsRequired = true)]
        bool _packToleranceNoMaxStep;
        [DataMember(IsRequired = true)]
        double _maxPackNeedTolerance;
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _merchandiseBasis;
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeGroups;
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _vSWSizeConstraintRules;
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeAlternateModels;
        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeConstraintRules;

        //Rule tab properties
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        ROMethodSizeRuleProperties _sizeRuleProperties;


        #region Public Properties
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        public KeyValuePair<int, int> MerchandiseHierarchy
        {
            get { return _merchandiseHierarchy; }
            set { _merchandiseHierarchy = value; }
        }

        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set
            {
                _merchandiseType = value;
            }
        }

        public bool NormalizeSizeCurvesDefaultIsOverridden
        {
            get { return _normalizeSizeCurvesDefaultIsOverridden; }
            set { _normalizeSizeCurvesDefaultIsOverridden = value; }
        }

        public bool NormalizeSizeCurves
        {
            get { return _normalizeSizeCurves; }
            set { _normalizeSizeCurves = value; }
        }

        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        public KeyValuePair<int, string> SizeAlternateModel
        {
            get { return _sizeAlternateModel; }
            set { _sizeAlternateModel = value; }
        }
        public ROSizeCurveProperties ROSizeCurveProperties
        {
            get { return _rOSizeCurveProperties; }
            set { _rOSizeCurveProperties = value; }
        }
        public ROSizeConstraintProperties ROSizeConstraintProperties
        {
            get { return _rOSizeConstraintProperties; }
            set { _rOSizeConstraintProperties = value; }
        }
        public bool OverrideVSWSizeConstraints
        {
            get { return _overrideVSWSizeConstraints; }
            set { _overrideVSWSizeConstraints = value; }
        }
        public KeyValuePair<int, string> VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set
            {
                _vSWSizeConstraints = value;
            }
        }
        public bool OverrideAvgPackDevTolerance
        {
            get { return _overrideAvgPackDevTolerance; }
            set { _overrideAvgPackDevTolerance = value; }
        }
        public double AvgPackDeviationTolerance
        {
            get { return _avgPackDeviationTolerance; }
            set { _avgPackDeviationTolerance = value; }
        }
        public bool OverrideMaxPackNeedTolerance
        {
            get { return _overrideMaxPackNeedTolerance; }
            set { _overrideMaxPackNeedTolerance = value; }
        }
        public bool PackToleranceStepped
        {
            get { return _packToleranceStepped; }
            set { _packToleranceStepped = value; }
        }
        public bool PackToleranceNoMaxStep
        {
            get { return _packToleranceNoMaxStep; }
            set { _packToleranceNoMaxStep = value; }
        }
        public double MaxPackNeedTolerance
        {
            get { return _maxPackNeedTolerance; }
            set { _maxPackNeedTolerance = value; }
        }
        public List<KeyValuePair<int, string>> MerchandiseBasis
        {
            get { return _merchandiseBasis; }
        }
        public List<KeyValuePair<int, string>> SizeGroups
        {
            get { return _sizeGroups; }
        }
        public List<KeyValuePair<int, string>> VSWSizeConstraintRules
        {
            get { return _vSWSizeConstraintRules; }
        }
        public List<KeyValuePair<int, string>> SizeAlternateModels
        {
            get { return _sizeAlternateModels; }
        }

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        public ROMethodSizeRuleProperties SizeRuleProperties
        {
            get { return _sizeRuleProperties; }
            set { _sizeRuleProperties = value; }
        }

        public List<KeyValuePair<int, string>> SizeConstraintRules
        {
            get { return _sizeConstraintRules; }
        }

        #endregion
        public ROMethodSizeNeedProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey,
            KeyValuePair<int, string> merch_HN, 
            int merch_PH_RID, 
            int merch_PHL_SEQ, 
            eMerchandiseType merchandiseType,
            bool normalizeSizeCurvesDefaultIsOverridden, 
            bool normalizeSizeCurves, 
            KeyValuePair<int, string> sizeGroup,
            KeyValuePair<int, string> sizeAlternateModel,
            ROSizeCurveProperties rOSizeCurveProperties, 
            ROSizeConstraintProperties rOSizeConstraintProperties,
            bool overrideVSWSizeConstraints,
            KeyValuePair<int, string> vSWSizeConstraints, 
            bool overrideAvgPackDevTolerance, 
            double avgPackDeviationTolerance,
            bool overrideMaxPackNeedTolerance, 
            bool packToleranceStepped, 
            bool packToleranceNoMaxStep, 
            double maxPackNeedTolerance,
            KeyValuePair<int, string> attribute, 
            ROMethodSizeRuleProperties sizeRuleProperties,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.SizeNeedAllocation, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Size Need method
            _merchandise = merch_HN;
            _merchandiseHierarchy = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            _merchandiseType = merchandiseType;
            _normalizeSizeCurvesDefaultIsOverridden = normalizeSizeCurvesDefaultIsOverridden;
            _normalizeSizeCurves = normalizeSizeCurves;
            _sizeGroup = sizeGroup;
            _sizeAlternateModel = sizeAlternateModel;
            _rOSizeCurveProperties = rOSizeCurveProperties;
            _rOSizeConstraintProperties = rOSizeConstraintProperties;
            _overrideVSWSizeConstraints = overrideVSWSizeConstraints;
            _vSWSizeConstraints = vSWSizeConstraints;
            _overrideAvgPackDevTolerance = overrideAvgPackDevTolerance;
            _avgPackDeviationTolerance = avgPackDeviationTolerance;
            _overrideMaxPackNeedTolerance = overrideMaxPackNeedTolerance;
            _packToleranceStepped = packToleranceStepped;
            _packToleranceNoMaxStep = packToleranceNoMaxStep;
            _maxPackNeedTolerance = maxPackNeedTolerance;
            _attribute = attribute;
            _sizeRuleProperties = sizeRuleProperties;
            _merchandiseBasis = new List<KeyValuePair<int, string>>();
            _sizeGroups = new List<KeyValuePair<int, string>>();
            _vSWSizeConstraintRules = new List<KeyValuePair<int, string>>();
            _sizeAlternateModels = new List<KeyValuePair<int, string>>();
            _sizeConstraintRules = new List<KeyValuePair<int, string>>();
        }
    }
    [DataContract(Name = "ROMethodBasisSizeProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodBasisSizeProperties : ROMethodProperties
    {
        // fields specific to Basis Size method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        ROSizeCurveProperties _rOSizeCurveProperties;
        [DataMember(IsRequired = true)]
        ROSizeConstraintProperties _rOSizeConstraintProperties;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _header;
        [DataMember(IsRequired = true)]
        bool _includeReserve;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _colorComponent;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _color;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _rule;
        [DataMember(IsRequired = true)]
        int _ruleQuantity;
        //Rule tab properties
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        ROMethodSizeRuleProperties _sizeRuleProperties;
        //basis substitute list
        [DataMember(IsRequired = true)]
        ROMethodBasisSizeSubstituteSet _basisSizeSubstituteSet;

        #region Public Properties

        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        public ROSizeCurveProperties ROSizeCurveProperties
        {
            get { return _rOSizeCurveProperties; }
            set { _rOSizeCurveProperties = value; }
        }

        public ROSizeConstraintProperties ROSizeConstraintProperties
        {
            get { return _rOSizeConstraintProperties; }
            set { _rOSizeConstraintProperties = value; }
        }

        public KeyValuePair<int, string> Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public bool IncludeReserve
        {
            get { return _includeReserve; }
            set { _includeReserve = value; }
        }
        public KeyValuePair<int, string> ColorComponent
        {
            get { return _colorComponent; }
            set { _colorComponent = value; }
        }
        public KeyValuePair<int, string> Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public KeyValuePair<int, string> Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }
        public int RuleQuantity
        {
            get { return _ruleQuantity; }
            set { _ruleQuantity = value; }
        }
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }
        public ROMethodSizeRuleProperties SizeRuleProperties
        {
            get { return _sizeRuleProperties; }
            set { _sizeRuleProperties = value; }
        }

        public ROMethodBasisSizeSubstituteSet BasisSizeSubstituteSet
        {
            get { return _basisSizeSubstituteSet; }
            set { _basisSizeSubstituteSet = value; }
        }
        #endregion
        public ROMethodBasisSizeProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> filter,
            KeyValuePair<int, string> sizeGroup, 
            ROSizeCurveProperties rOSizeCurveProperties, 
            ROSizeConstraintProperties rOSizeConstraintProperties,
            KeyValuePair<int, string> header, 
            bool includeReserve, 
            KeyValuePair<int, string> colorComponent, 
            KeyValuePair<int, string> color,
            KeyValuePair<int, string> rule, 
            int ruleQuantity,
            KeyValuePair<int, string> attribute, 
            ROMethodSizeRuleProperties sizeRuleProperties, 
            ROMethodBasisSizeSubstituteSet basisSizeSubstituteSet,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.BasisSizeAllocation, 
                  method, 
                  description, 
                  userKey,
                  isTemplate)

        {
            // fields specific to Fill Size Holes method
            _filter = filter;
            _sizeGroup = sizeGroup;
            _rOSizeCurveProperties = rOSizeCurveProperties;
            _rOSizeConstraintProperties = rOSizeConstraintProperties;
            _header = header;
            _includeReserve = includeReserve;
            _colorComponent = colorComponent;
            _color = color;
            _rule = rule;
            _ruleQuantity = ruleQuantity;
            _attribute = attribute;
            _sizeRuleProperties = sizeRuleProperties;
            _basisSizeSubstituteSet = basisSizeSubstituteSet;
        }
    }

    [DataContract(Name = "ROMethodBasisSizeSubstituteSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodBasisSizeSubstituteSet
    {
        [DataMember(IsRequired = true)]
        public List<ROMethodBasisSizeSubstituteProperties> _basisSizeSubstituteRowsValues;


        public List<ROMethodBasisSizeSubstituteProperties> BasisSizeSubstituteRowsValues
        {
            get
            {
                if (_basisSizeSubstituteRowsValues == null)
                {
                    _basisSizeSubstituteRowsValues = new List<ROMethodBasisSizeSubstituteProperties>();
                }
                return _basisSizeSubstituteRowsValues;
            }
            set { _basisSizeSubstituteRowsValues = value; }
        }

    }

    [DataContract(Name = "ROMethodBasisSizeSubstituteProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodBasisSizeSubstituteProperties
    {
        [DataMember(IsRequired = true)]
        bool _updated;
        [DataMember(IsRequired = true)]
        bool _inserted;
        [DataMember(IsRequired = true)]
        bool _deleted;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _substitue;
        [DataMember(IsRequired = true)]
        eEquateOverrideSizeType _overrideSizeType;

        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public bool Inserted
        {
            get { return _inserted; }
            set { _inserted = value; }
        }
        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public KeyValuePair<int, string> SizeType
        {
            get { return _sizeType; }
            set { _sizeType = value; }
        }
        public KeyValuePair<int, string> Substitue
        {
            get { return _substitue; }
            set { _substitue = value; }
        }

        public eEquateOverrideSizeType OverrideSizeType
        {
            get { return _overrideSizeType; }
            set
            {
                _overrideSizeType = value;
            }
        }

        public ROMethodBasisSizeSubstituteProperties(bool updated, bool inserted, bool deleted, KeyValuePair<int, string> sizeType, KeyValuePair<int, string> substitue,
             eEquateOverrideSizeType overrideSizeType)
        {
            // fields specific to Size Rule Method Constraints
            _updated = updated;
            _inserted = inserted;
            _deleted = deleted;
            _sizeType = sizeType;
            _substitue = substitue;
            _overrideSizeType = overrideSizeType;
        }
    }

    [DataContract(Name = "ROMethodSizeCurveProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeCurveProperties : ROMethodProperties
    {
        // fields specific to Size Curve Method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        eSizeCurvesByType _sizeCurvesByType;
        [DataMember(IsRequired = true)]
        bool _merchBasisEqualizeWeight;
        [DataMember(IsRequired = true)]
        ROMethodSizeCurveMerchBasisSet _sizeCurveMerchBasisSet;
        [DataMember(IsRequired = true)]
        double _tolerMinAvgPerSize;
        [DataMember(IsRequired = true)]
        double _tolerSalesTolerance;
        [DataMember(IsRequired = true)]
        eNodeChainSalesType _tolerIndexUnitsType;
        [DataMember(IsRequired = true)]
        double _tolerMinTolerancePct;
        [DataMember(IsRequired = true)]
        double _tolerMaxTolerancePct;
        [DataMember(IsRequired = true)]
        bool _applyMinToZeroTolerance;

        #region Public Properties
        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public eSizeCurvesByType SizeCurvesByType
        {
            get { return _sizeCurvesByType; }
            set
            {
                _sizeCurvesByType = value;
            }
        }

        public bool MerchBasisEqualizeWeight
        {
            get { return _merchBasisEqualizeWeight; }
            set { _merchBasisEqualizeWeight = value; }
        }

        public ROMethodSizeCurveMerchBasisSet SizeCurveMerchBasisSet
        {
            get { return _sizeCurveMerchBasisSet; }
            set { _sizeCurveMerchBasisSet = value; }
        }
        
        public double TolerMinAvgPerSize
        {
            get { return _tolerMinAvgPerSize; }
            set { _tolerMinAvgPerSize = value; }
        }
        public double TolerSalesTolerance
        {
            get { return _tolerSalesTolerance; }
            set { _tolerSalesTolerance = value; }
        }
        public eNodeChainSalesType TolerIndexUnitsType
        {
            get { return _tolerIndexUnitsType; }
            set
            {
                _tolerIndexUnitsType = value;
            }
        }
        public double TolerMinTolerancePct
        {
            get { return _tolerMinTolerancePct; }
            set { _tolerMinTolerancePct = value; }
        }
        public double TolerMaxTolerancePct
        {
            get { return _tolerMaxTolerancePct; }
            set { _tolerMaxTolerancePct = value; }
        }
        public bool ApplyMinToZeroTolerance
        {
            get { return _applyMinToZeroTolerance; }
            set { _applyMinToZeroTolerance = value; }
        }

        #endregion
        public ROMethodSizeCurveProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey,
            KeyValuePair<int, string> sizeGroup, 
            KeyValuePair<int, string> attribute, 
            eSizeCurvesByType sizeCurvesByType,
            bool merchBasisEqualizeWeight, 
            ROMethodSizeCurveMerchBasisSet sizeCurveMerchBasisSet, 
            double tolerMinAvgPerSize, 
            double tolerSalesTolerance,
            eNodeChainSalesType tolerIndexUnitsType, 
            double tolerMinTolerancePct, 
            double tolerMaxTolerancePct, 
            bool applyMinToZeroTolerance,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.SizeCurve, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Size Curve method
            _sizeGroup = sizeGroup;
            _attribute = attribute;
            _sizeCurvesByType = sizeCurvesByType;
            _merchBasisEqualizeWeight = merchBasisEqualizeWeight;
            _sizeCurveMerchBasisSet = sizeCurveMerchBasisSet;
            _tolerMinAvgPerSize = tolerMinAvgPerSize;
            _tolerSalesTolerance = tolerSalesTolerance;
            _tolerIndexUnitsType = tolerIndexUnitsType;
            _tolerMinTolerancePct = tolerMinTolerancePct;
            _tolerMaxTolerancePct = tolerMaxTolerancePct;
            _applyMinToZeroTolerance = applyMinToZeroTolerance;
        }
    }
    [DataContract(Name = "ROMethodSizeCurveMerchBasisSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeCurveMerchBasisSet
    {
        [DataMember(IsRequired = true)]
        public List<ROMethodSizeCurveMerchBasisProperties> _SizeCurveMerchBasisRowsValues;


        public List<ROMethodSizeCurveMerchBasisProperties> SizeCurveMerchBasisRowsValues
        {
            get
            {
                if (_SizeCurveMerchBasisRowsValues == null)
                {
                    _SizeCurveMerchBasisRowsValues = new List<ROMethodSizeCurveMerchBasisProperties>();
                }
                return _SizeCurveMerchBasisRowsValues;
            }
            set { _SizeCurveMerchBasisRowsValues = value; }
        }

    }

    [DataContract(Name = "ROMethodSizeCurveMerchBasisProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeCurveMerchBasisProperties
    {
        [DataMember(IsRequired = true)]
        bool _updated;
        [DataMember(IsRequired = true)]
        bool _inserted;
        [DataMember(IsRequired = true)]
        bool _deleted;
        [DataMember(IsRequired = true)]
        int _basis_SEQ; //key
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string>  _hn; 
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _fv;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _cdr;
        [DataMember(IsRequired = true)]
        eMerchandiseType _merchType;
        [DataMember(IsRequired = true)]
        decimal _weight;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _oll;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _customOll;
        
        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public bool Inserted
        {
            get { return _inserted; }
            set { _inserted = value; }
        }
        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }
        public int Basis_SEQ
        {
            get { return _basis_SEQ; }
            set { _basis_SEQ = value; }
        }
        public KeyValuePair<int,string> HN
        {
            get { return _hn; }
            set { _hn = value; }
        }
        public KeyValuePair<int, string> FV
        {
            get { return _fv; }
            set { _fv = value; }
        }
        public KeyValuePair<int, string> CDR
        {
            get { return _cdr; }
            set { _cdr = value; }
        }
        public eMerchandiseType MerchType
        {
            get { return _merchType; }
            set
            {
                _merchType = value;
            }
        }

        public decimal Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        public KeyValuePair<int, string> OLL
        {
            get { return _oll; }
            set { _oll = value; }
        }
        public KeyValuePair<int, string> CustomOll
        {
            get { return _customOll; }
            set { _customOll = value; }
        }
        public ROMethodSizeCurveMerchBasisProperties(bool updated, bool inserted, bool deleted, int basis_SEQ 
            ,KeyValuePair<int, string> hn, KeyValuePair<int, string> fv, KeyValuePair<int, string> cdr
            ,eMerchandiseType merchType, decimal weight, KeyValuePair<int, string> oll, KeyValuePair<int, string> customOll
            )
        {
            // fields specific to SizeCurve Merch Basis
            _updated = updated;
            _inserted = inserted;
            _deleted = deleted;
            _basis_SEQ = basis_SEQ;
            _hn = hn;
            _fv = fv;
            _cdr = cdr;
            _merchType = merchType;
            _weight = weight;
            _oll = oll;
            _customOll = customOll;
        }
    }

    [DataContract(Name = "ROMethodDCFulfillmentProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodDCFulfillmentProperties : ROMethodProperties
    {
        // fields specific to DC Fulfillment method
        [DataMember(IsRequired = true)]
        eDCFulfillmentSplitOption _dCFulfillmentSplitOption; // default DCFulfillment from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        bool _applyMinimumsInd; // default = false from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _prioritizeType; // default = "C" use HCG_ID from MethodDCFulfillment.cs or "H" use header filed
        [DataMember(IsRequired = true)]
        eDCFulfillmentHeadersOrder _headersOrder; // default Ascending from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        eDCFulfillmentSplitByOption _split_By_Option; // default SplitByDC from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        eDCFulfillmentWithinDC _within_Dc; // default Proportional from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        eDCFulfillmentReserve _split_By_Reserve; // default ReservePreSplit from MethodDCFulfillment.cs
        [DataMember(IsRequired = true)]
        eDCFulfillmentStoresOrder _storesOrder; // default Ascending from MethodDCFulfillment.cs
        //basis DCStoreCharacteristic list
        [DataMember(IsRequired = true)]
        ROMethodDCStoreCharacteristicSet _dCStoreCharacteristicSet;

        #region Public Properties

        public eDCFulfillmentSplitOption DCFulfillmentSplitOption
        {
            get { return _dCFulfillmentSplitOption; }
            set
            {
                _dCFulfillmentSplitOption = value;
            }
        }
        public bool ApplyMinimumsInd
        {
            get { return _applyMinimumsInd; }
            set { _applyMinimumsInd = value; }
        }
        public KeyValuePair<int, string> PrioritizeType
        {
            get { return _prioritizeType; }
            set { _prioritizeType = value;}
        }

        public eDCFulfillmentHeadersOrder HeadersOrder
        {
            get { return _headersOrder; }
            set
            {
                _headersOrder = value;
            }
        }
        public eDCFulfillmentSplitByOption Split_By_Option
        {
            get { return _split_By_Option; }
            set
            {
                _split_By_Option = value;
            }
        }
        public eDCFulfillmentWithinDC Within_Dc
        {
            get { return _within_Dc; }
            set
            {
                _within_Dc = value;
            }
        }
        public eDCFulfillmentReserve Split_By_Reserve
        {
            get { return _split_By_Reserve; }
            set
            {
                _split_By_Reserve = value;
            }
        }
        public eDCFulfillmentStoresOrder StoresOrder
        {
            get { return _storesOrder; }
            set
            {
                _storesOrder = value;
            }
        }
        public ROMethodDCStoreCharacteristicSet DCStoreCharacteristicSet
        {
            get { return _dCStoreCharacteristicSet; }
            set { _dCStoreCharacteristicSet = value; }
        }
        #endregion
        public ROMethodDCFulfillmentProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            eDCFulfillmentSplitOption dCFulfillmentSplitOption, 
            bool applyMinimumsInd,
            KeyValuePair<int, string> prioritizeType, 
            eDCFulfillmentHeadersOrder headersOrder, 
            eDCFulfillmentSplitByOption split_By_Option, 
            eDCFulfillmentWithinDC within_Dc,
            eDCFulfillmentReserve split_By_Reserve, 
            eDCFulfillmentStoresOrder storesOrder, 
            ROMethodDCStoreCharacteristicSet dCStoreCharacteristicSet,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.DCFulfillment, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to DC Fulfillment method
            _dCFulfillmentSplitOption = dCFulfillmentSplitOption;
            _applyMinimumsInd = applyMinimumsInd;
            _prioritizeType = prioritizeType;
            _headersOrder = headersOrder;
            _split_By_Option = split_By_Option;
            _within_Dc = within_Dc;
            _split_By_Reserve = split_By_Reserve;
            _storesOrder = storesOrder;
            _dCStoreCharacteristicSet = dCStoreCharacteristicSet;
        }
    }

    [DataContract(Name = "ROMethodDCStoreCharacteristicSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodDCStoreCharacteristicSet
    {
        [DataMember(IsRequired = true)]
        public List<ROMethodDCStoreCharacteristicProperties> _dCStoreCharacteristicRowsValues;


        public List<ROMethodDCStoreCharacteristicProperties> DCStoreCharacteristicRowsValues
        {
            get
            {
                if (_dCStoreCharacteristicRowsValues == null)
                {
                    _dCStoreCharacteristicRowsValues = new List<ROMethodDCStoreCharacteristicProperties>();
                }
                return _dCStoreCharacteristicRowsValues;
            }
            set { _dCStoreCharacteristicRowsValues = value; }
        }

    }

    [DataContract(Name = "ROMethodDCStoreCharacteristicProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodDCStoreCharacteristicProperties
    {
        [DataMember(IsRequired = true)]
        bool _updated;
        [DataMember(IsRequired = true)]
        bool _inserted;
        [DataMember(IsRequired = true)]
        bool _deleted;
        [DataMember(IsRequired = true)]
        int _seq;
        [DataMember(IsRequired = true)]
        string _distCenter;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeCharacteristics; //SCG_RID key
    
        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public bool Inserted
        {
            get { return _inserted; }
            set { _inserted = value; }
        }
        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }
        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }
        }
        public string DistCenter
        {
            get { return _distCenter; }
            set { _distCenter = value; }
        }
        public KeyValuePair<int, string> storeCharacteristics
        {
            get { return _storeCharacteristics; }
            set { _storeCharacteristics = value; }
        }

        public ROMethodDCStoreCharacteristicProperties(bool updated, bool inserted, bool deleted, int seq, string distCenter,
            KeyValuePair<int, string> storeCharacteristics)
        {
            // fields specific to Size Rule Method Constraints
            _updated = updated;
            _inserted = inserted;
            _deleted = deleted;
            _seq = seq;
            _distCenter = distCenter;
            _storeCharacteristics = storeCharacteristics;
        }
    }

    [DataContract(Name = "ROMethodBuildPacksProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodBuildPacksProperties : ROMethodProperties
    {
        // fields specific to Build Packs method
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _vendor;
        [DataMember(IsRequired = true)]
        int _packMinOrder;
        [DataMember(IsRequired = true)]
        int _sizeMultiple;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeCurve;
        // pack combination list excisting abstract class in RO
        [DataMember(IsRequired = true)]
        List<PackPatternCombo> _packCombination;
        // Constraints Tab
        [DataMember(IsRequired = true)]
        double _reserveTotalQty;
        [DataMember(IsRequired = true)]
        bool _reserveTotalIsPercent;
        [DataMember(IsRequired = true)]
        double _reserveBulkQty;
        [DataMember(IsRequired = true)]
        bool _reserveBulkIsPercent;
        [DataMember(IsRequired = true)]
        double _reservePacksQty;
        [DataMember(IsRequired = true)]
        bool _reservePacksIsPercent;
        [DataMember(IsRequired = true)]
        bool _removeBulkInd;
        [DataMember(IsRequired = true)]
        double _avgPackErrorDevTolerance;
        [DataMember(IsRequired = true)]
        double _maxPackErrorDevTolerance;
        [DataMember(IsRequired = true)]
        bool _depleteReserveSelected;
        [DataMember(IsRequired = true)]
        bool _increaseBuySelected;
        [DataMember(IsRequired = true)]
        double _increaseBuyPct;

        #region Public Properties

        public KeyValuePair<int, string> Vendor
        {
            get { return _vendor; }
            set { _vendor = value; }
        }
        public int PackMinOrder
        {
            get { return _packMinOrder; }
            set { _packMinOrder = value; }
        }
        public int SizeMultiple
        {
            get { return _sizeMultiple; }
            set { _sizeMultiple = value; }
        }
        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }
        public KeyValuePair<int, string> SizeCurve
        {
            get { return _sizeCurve; }
            set { _sizeCurve = value; }
        }
        public List<PackPatternCombo> PackCombination
        {
            get { return _packCombination; }
            set { _packCombination = value; }
        }
        public double ReserveTotalQty
        {
            get { return _reserveTotalQty; }
            set { _reserveTotalQty = value; }
        }
        public bool ReserveTotalIsPercent
        {
            get { return _reserveTotalIsPercent; }
            set { _reserveTotalIsPercent = value; }
        }
        public double ReserveBulkQty
        {
            get { return _reserveBulkQty; }
            set { _reserveBulkQty = value; }
        }
        public bool ReserveBulkIsPercent
        {
            get { return _reserveBulkIsPercent; }
            set { _reserveBulkIsPercent = value; }
        }
        public double ReservePacksQty
        {
            get { return _reservePacksQty; }
            set { _reservePacksQty = value; }
        }
        public bool ReservePacksIsPercent
        {
            get { return _reservePacksIsPercent; }
            set { _reservePacksIsPercent = value; }
        }
        public bool RemoveBulkInd
        {
            get { return _removeBulkInd; }
            set { _removeBulkInd = value; }
        }
        public double AvgPackErrorDevTolerance
        {
            get { return _avgPackErrorDevTolerance; }
            set { _avgPackErrorDevTolerance = value; }
        }
        public double MaxPackErrorDevTolerance
        {
            get { return _maxPackErrorDevTolerance; }
            set { _maxPackErrorDevTolerance = value; }
        }
        public bool DepleteReserveSelected
        {
            get { return _depleteReserveSelected; }
            set { _depleteReserveSelected = value; }
        }
        public bool IncreaseBuySelected
        {
            get { return _increaseBuySelected; }
            set { _increaseBuySelected = value; }
        }
        public double IncreaseBuyPct
        {
            get { return _increaseBuyPct; }
            set { _increaseBuyPct = value; }
        }

        #endregion
        public ROMethodBuildPacksProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> vendor,
            int packMinOrder, 
            int sizeMultiple, 
            KeyValuePair<int, string> sizeGroup, 
            KeyValuePair<int, string> sizeCurve, 
            List<PackPatternCombo> packCombination,
            double reserveTotalQty, 
            bool reserveTotalIsPercent, 
            double reserveBulkQty, 
            bool reserveBulkIsPercent, 
            double reservePacksQty,
            bool reservePacksIsPercent,
            bool removeBulkInd, 
            double avgPackErrorDevTolerance, 
            double maxPackErrorDevTolerance, 
            bool depleteReserveSelected, 
            bool increaseBuySelected, 
            double increaseBuyPct,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.BuildPacks, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Build Packs method

            _vendor = vendor;
            _packMinOrder = packMinOrder;
            _sizeMultiple = sizeMultiple;
            _sizeGroup = sizeGroup;
            _sizeCurve = sizeCurve;
            _packCombination = packCombination;
            _reserveTotalQty = reserveTotalQty;
            _reserveTotalIsPercent = reserveTotalIsPercent;
            _reserveBulkQty = reserveBulkQty;
            _reserveBulkIsPercent = reserveBulkIsPercent;
            _reservePacksQty = reservePacksQty;
            _reservePacksIsPercent = reservePacksIsPercent;
            _removeBulkInd = removeBulkInd;
            _avgPackErrorDevTolerance = avgPackErrorDevTolerance;
            _maxPackErrorDevTolerance = maxPackErrorDevTolerance;
            _depleteReserveSelected = depleteReserveSelected;
            _increaseBuySelected = increaseBuySelected;
            _increaseBuyPct = increaseBuyPct;
        }
    }

    [DataContract(Name = "ROSizeCurveProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveProperties
    {
        [DataMember(IsRequired = true)]
        private int _sizeCurveGroupKey;

        [DataMember(IsRequired = true)]
        private bool _isUseDefault;

        [DataMember(IsRequired = true)]
        private bool _isApplyRulesOnly;

        [DataMember(IsRequired = true)]
        private bool _isColorSelected;

        [DataMember(IsRequired = true)]
        eGenericSizeCurveNameType _genericSizeCurveNameType;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _genericMerchandiseType;

        [DataMember(IsRequired = true)]
        private int _genericHierarchyLevelKey;

        [DataMember(IsRequired = true)]
        private int _genericHeaderCharacteristicsOrNameExtensionKey;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeCurveGroups;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _genericHierarchyLevels;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _genericHeaderCharacteristicsOrNameExtensions;

        public ROSizeCurveProperties(
            int sizeCurveGroupKey,
            eGenericSizeCurveNameType genericSizeCurveNameType,
            eMerchandiseType genericMerchandiseType,
            int genericHierarchyLevelKey,
            bool isUseDefault, 
            bool isApplyRulesOnly, 
            bool isColorSelected,
            int genericHeaderCharacteristicsOrNameExtensionKey
            )
        {
            _sizeCurveGroupKey = sizeCurveGroupKey;
            _genericMerchandiseType = genericMerchandiseType;
            _genericHierarchyLevelKey = genericHierarchyLevelKey;
            _genericSizeCurveNameType = genericSizeCurveNameType;
            _isUseDefault = isUseDefault;
            _isApplyRulesOnly = isApplyRulesOnly;
            _isColorSelected = isColorSelected;
            _genericHeaderCharacteristicsOrNameExtensionKey = genericHeaderCharacteristicsOrNameExtensionKey;
            _sizeCurveGroups = new List<KeyValuePair<int, string>>();
            _genericHierarchyLevels = new List<KeyValuePair<int, string>>();
            _genericHeaderCharacteristicsOrNameExtensions = new List<KeyValuePair<int, string>>();
        }

        public int SizeCurveGroupKey { get { return _sizeCurveGroupKey; } set { _sizeCurveGroupKey = value; } }
        public int GenericHierarchyLevelKey { get { return _genericHierarchyLevelKey; } set { _genericHierarchyLevelKey = value; } }

        public eGenericSizeCurveNameType GenericSizeCurveNameType
        {
            get { return _genericSizeCurveNameType; }
            set { _genericSizeCurveNameType = value; }
        }
        public eMerchandiseType GenericMerchandiseType
        {
            get { return _genericMerchandiseType; }
            set { _genericMerchandiseType = value; }
        }
        public bool IsUseDefault { get { return _isUseDefault; } set { _isUseDefault = value; } }
        public bool IsApplyRulesOnly { get { return _isApplyRulesOnly; } set { _isApplyRulesOnly = value; } }
        public bool IsColorSelected { get { return _isColorSelected; } set { _isColorSelected = value; } }
        public int GenericHeaderCharacteristicsOrNameExtensionKey
        {
            get { return _genericHeaderCharacteristicsOrNameExtensionKey; }
            set { _genericHeaderCharacteristicsOrNameExtensionKey = value; }
        }
        public List<KeyValuePair<int, string>> SizeCurveGroups { get { return _sizeCurveGroups; } }
        public List<KeyValuePair<int, string>> GenericHierarchyLevels { get { return _genericHierarchyLevels; } }
        public List<KeyValuePair<int, string>> GenericHeaderCharacteristicsOrNameExtensions { get { return _genericHeaderCharacteristicsOrNameExtensions; } }

    }

    [DataContract(Name = "ROSizeConstraintProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintProperties
    {
        [DataMember(IsRequired = true)]
        private int _sizeConstraintKey;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _inventoryBasisMerchandiseType;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _inventoryBasisMerchandise;

        [DataMember(IsRequired = true)]
        private int _inventoryBasisHierarchyLevelKey;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _genericMerchandiseType;

        [DataMember(IsRequired = true)]
        private int _genericHierarchyLevelKey;

        [DataMember(IsRequired = true)]
        private int _genericHeaderCharacteristicsKey;

        [DataMember(IsRequired = true)]
        private bool _isColorSelected;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeConstraints;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _inventoryBasisHierarchyLevels;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _genericHeaderCharacteristics;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _genericHierarchyLevels;


        public ROSizeConstraintProperties(
            int sizeConstraintKey,
            eMerchandiseType inventoryBasisMerchandiseType,
            KeyValuePair<int, string> inventoryBasisMerchandise,
            int inventoryBasisHierarchyLevelKey,
            eMerchandiseType genericMerchandiseType,
            int genericHierarchyLevelKey,
            int genericHeaderCharacteristicsKey,
            bool isColorSelected
            )
        {
            _sizeConstraintKey = sizeConstraintKey;
            _inventoryBasisMerchandiseType = inventoryBasisMerchandiseType;
            _inventoryBasisMerchandise = inventoryBasisMerchandise;
            _inventoryBasisHierarchyLevelKey = inventoryBasisHierarchyLevelKey;
            _inventoryBasisHierarchyLevelKey = inventoryBasisHierarchyLevelKey;
            _genericMerchandiseType = genericMerchandiseType;
            _genericHierarchyLevelKey = genericHierarchyLevelKey;
            _genericHeaderCharacteristicsKey = genericHeaderCharacteristicsKey;
            _isColorSelected = isColorSelected;

            _sizeConstraints = new List<KeyValuePair<int, string>>();
            _inventoryBasisHierarchyLevels = new List<KeyValuePair<int, string>>();
            _genericHierarchyLevels = new List<KeyValuePair<int, string>>();
            _genericHeaderCharacteristics = new List<KeyValuePair<int, string>>();
        }

        public int SizeConstraintKey { get { return _sizeConstraintKey; } set { _sizeConstraintKey = value; } }
        public eMerchandiseType InventoryBasisMerchandiseType
        {
            get { return _inventoryBasisMerchandiseType; }
            set
            {
                _inventoryBasisMerchandiseType = value;
            }
        }
        public KeyValuePair<int, string> InventoryBasisMerchandise { get { return _inventoryBasisMerchandise; } set { _inventoryBasisMerchandise = value; } }
        public int InventoryBasisHierarchyLevelKey { get { return _inventoryBasisHierarchyLevelKey; } set { _inventoryBasisHierarchyLevelKey = value; } }
        public eMerchandiseType GenericMerchandiseType
        {
            get { return _genericMerchandiseType; }
            set
            {
                _genericMerchandiseType = value;
            }
        }
        public int GenericHierarchyLevelKey { get { return _genericHierarchyLevelKey; } set { _genericHierarchyLevelKey = value; } }
        public int GenericHeaderCharacteristicsKey { get { return _genericHeaderCharacteristicsKey; } set { _genericHeaderCharacteristicsKey = value; } }
        public bool IsColorSelected { get { return _isColorSelected; } set { _isColorSelected = value; } }
        
        public List<KeyValuePair<int, string>> SizeConstraints { get { return _sizeConstraints; } }
        public List<KeyValuePair<int, string>> InventoryBasisHierarchyLevels { get { return _inventoryBasisHierarchyLevels; } }
        public List<KeyValuePair<int, string>> GenericHierarchyLevels { get { return _genericHierarchyLevels; } }
        public List<KeyValuePair<int, string>> GenericHeaderCharacteristics { get { return _genericHeaderCharacteristics; } }

    }

    [DataContract(Name = "ROMethodSizeRuleProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeRuleProperties
    {
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeRuleItem;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeRule;
        [DataMember(IsRequired = true)]
        private double _sizeQuantity;
        [DataMember(IsRequired = true)]
        private List<ROMethodSizeRuleProperties> _children;


        public KeyValuePair<int, string> SizeRuleItem
        {
            get { return _sizeRuleItem; }
            set { _sizeRuleItem = value; }
        }
        public KeyValuePair<int, string> SizeRule
        {
            get { return _sizeRule; }
            set { _sizeRule = value; }
        }
        public double SizeQuantity
        {
            get { return _sizeQuantity; }
            set { _sizeQuantity = value; }
        }
        public List<ROMethodSizeRuleProperties> Children
        {
            get { return _children; }
        }

        public ROMethodSizeRuleProperties( 
            KeyValuePair<int, string> sizeRuleItem, 
            KeyValuePair<int, string> sizeRule,
            double sizeQuantity,
            List<ROMethodSizeRuleProperties> children = null
            )
        {
            _sizeRuleItem = sizeRuleItem;;
            _sizeRule = sizeRule;
            _sizeQuantity = sizeQuantity;
            _children = children;
        }
    }


    [DataContract(Name = "ROMethodRuleProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodRuleProperties : ROMethodProperties
    {
        // fields specific to General Allocation method 
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _header;
        [DataMember(IsRequired = true)]
        bool _isHeaderMaster;
        [DataMember(IsRequired = true)]
        eSortDirection _sortDirection;
        [DataMember(IsRequired = true)]
        bool _includeReserveInd;
        [DataMember(IsRequired = true)]
        eComponentType _componentType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _pack;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _color;
        [DataMember(IsRequired = true)]
        eRuleMethod _includeRuleMethod;
        [DataMember(IsRequired = true)]
        double _includeQuantity;
        [DataMember(IsRequired = true)]
        eRuleMethod _excludeRuleMethod;
        [DataMember(IsRequired = true)]
        double _excludeQuantity;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _hdr_BC;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _components;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _packs;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _colors;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _includedStoresRules;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _excludedStoresRules;

        #region Public Properties
        public KeyValuePair<int, string> Filter 
        {
            get { return _filter; }
            set { _filter = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the filter is set.
		/// </summary>
		public bool FilterIsSet
        {
            get { return !_filter.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Header
        {
            get { return _header; }
            set { _header = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Header is set.
		/// </summary>
		public bool HeaderIsSet
        {
            get { return !_header.Equals(default(KeyValuePair<int, string>)); }
        }

        public bool IsHeaderMaster
        {
            get { return _isHeaderMaster; }
            set { _isHeaderMaster = value; }
        }

        public eSortDirection SortDirection
        {
            get { return _sortDirection; }
            set
            {
                _sortDirection = value;
            }
        }

        /// <summary>
		/// Gets a flag identifying if the SortDirection is set.
		/// </summary>
		public bool SortDirectionIsSet
        {
            get { return _sortDirection != eSortDirection.None; }
        }

        public bool IncludeReserveInd
        {
            get { return _includeReserveInd; }
            set { _includeReserveInd = value; }
        }

        public eComponentType ComponentType
        {
            get { return _componentType; }
            set
            {
                _componentType = value;
            }
        }

        public KeyValuePair<int, string> Pack
        {
            get { return _pack; }
            set { _pack = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Pack is set.
		/// </summary>
		public bool PackIsSet
        {
            get { return !_pack.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Color is set.
		/// </summary>
		public bool ColorIsSet
        {
            get { return !_color.Equals(default(KeyValuePair<int, string>)); }
        }

        public eRuleMethod IncludeRuleMethod
        {
            get { return _includeRuleMethod; }
            set
            {
                _includeRuleMethod = value;
            }
        }

        /// <summary>
		/// Gets a flag identifying if the IncludeRuleMethod is set.
		/// </summary>
		public bool IncludeRuleMethodIsSet
        {
            get { return _includeRuleMethod != eRuleMethod.None; }
        }

        public double IncludeQuantity
        {
            get { return _includeQuantity; }
            set { _includeQuantity = value; }
        }

        public eRuleMethod ExcludeRuleMethod
        {
            get { return _excludeRuleMethod; }
            set
            {
                _excludeRuleMethod = value;
            }
        }

        /// <summary>
		/// Gets a flag identifying if the ExcludeRuleMethod is set.
		/// </summary>
		public bool ExcludeRuleMethodIsSet
        {
            get { return _excludeRuleMethod != eRuleMethod.None; }
        }

        public double ExcludeQuantity
        {
            get { return _excludeQuantity; }
            set { _excludeQuantity = value; }
        }

        public KeyValuePair<int, string> Hdr_BC
        {
            get { return _hdr_BC; }
            set { _hdr_BC = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Hdr_BC is set.
		/// </summary>
		public bool Hdr_BCIsSet
        {
            get { return !_hdr_BC.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Attribute is set.
		/// </summary>
		public bool AttributeIsSet
        {
            get { return !_attribute.Equals(default(KeyValuePair<int, string>)); }
        }

        [ObsoleteAttribute("This property is obsolete. Use AttributeSet instead.", false)]
        public KeyValuePair<int, string> StoreGroupLevel
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the StoreGroupLevel is set.
		/// </summary>
		public bool StoreGroupLevelIsSet
        {
            get { return !_attributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the StoreGroupLevel is set.
		/// </summary>
		public bool AttributeSetIsSet
        {
            get { return !_attributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the list of components
        /// </summary>
        public List<KeyValuePair<int, string>> Components
        {
            get { return _components; }
        }

        /// <summary>
        /// Gets the list of packs
        /// </summary>
        public List<KeyValuePair<int, string>> Packs
        {
            get { return _packs; }
        }

        /// <summary>
        /// Gets the list of colors
        /// </summary>
        public List<KeyValuePair<int, string>> Colors
        {
            get { return _colors; }
        }

        /// <summary>
        /// Gets the list of rules for included stores
        /// </summary>
        public List<KeyValuePair<int, string>> IncludedStoresRules
        {
            get { return _includedStoresRules; }
        }

        /// <summary>
        /// Gets the list of rules for excluded stores
        /// </summary>
        public List<KeyValuePair<int, string>> ExcludedStoresRules
        {
            get { return _excludedStoresRules; }
        }

        #endregion
        public ROMethodRuleProperties(
            KeyValuePair<int, string> method = default(KeyValuePair<int, string>), 
            string description = null, 
            int userKey = Include.Undefined, 
            KeyValuePair<int, string> filter = default(KeyValuePair<int, string>), 
            KeyValuePair<int, string> header = default(KeyValuePair<int, string>), 
            bool isHeaderMaster = false,
            eSortDirection sortDirection = eSortDirection.None, 
            bool includeReserveInd = false, 
            eComponentType componentType = eComponentType.Total, 
            KeyValuePair<int, string> pack = default(KeyValuePair<int, string>), 
            KeyValuePair<int, string> color = default(KeyValuePair<int, string>), 
            eRuleMethod includeRuleMethod = eRuleMethod.None,
            double includeQuantity = 0, 
            eRuleMethod excludeRuleMethod = eRuleMethod.None, 
            double excludeQuantity = 0, 
            KeyValuePair<int, string> hdr_BC = default(KeyValuePair<int, string>), 
            KeyValuePair<int, string> attribute = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> attributeSet = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> storeGroupLevel = default(KeyValuePair<int, string>),
            bool isTemplate = false
            ) 
            : base(
                  
                  eMethodType.Rule, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )

        {
            // fields specific to Rule method
            _filter = filter;
            _header = header;
            _isHeaderMaster = isHeaderMaster;
            _sortDirection = sortDirection;
            _includeReserveInd = includeReserveInd;
            _componentType = componentType;
            _pack = pack;
            _color = color;
            _includeRuleMethod = includeRuleMethod;
            _includeQuantity = includeQuantity;
            _excludeRuleMethod = excludeRuleMethod;
            _excludeQuantity = excludeQuantity;
            _hdr_BC = hdr_BC;
			_attribute = attribute;
            _attributeSet = attributeSet;
            _attributeSet = storeGroupLevel;

            _components = new List<KeyValuePair<int, string>>();
            _packs = new List<KeyValuePair<int, string>>();
            _colors = new List<KeyValuePair<int, string>>();
            _includedStoresRules = new List<KeyValuePair<int, string>>();
            _excludedStoresRules = new List<KeyValuePair<int, string>>();
        }
    }

    [DataContract(Name = "ROAllocationStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationStoreGrade : ROStoreGrade
    {
        [DataMember(IsRequired = true)]
        public int? AdMinimum { get; set; }
        [DataMember(IsRequired = true)]
        public int? ColorMinimum { get; set; }
        [DataMember(IsRequired = true)]
        public int? ColorMaximum { get; set; }
        [DataMember(IsRequired = true)]
        public int? ShipUpTo { get; set; }

        public bool AdMinimumIsSet { get { return AdMinimum != null; } }
        public bool ColorMinimumIsSet { get { return ColorMinimum != null; } }
        public bool ColorMaximumIsSet { get { return ColorMaximum != null; } }
        public bool ShipUpToIsSet { get { return ShipUpTo != null; } }
    }

    [DataContract(Name = "ROAllocationVelocityGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationVelocityGrade : ROStoreGrade
    {
        [DataMember(IsRequired = true)]
        public int? AdMinimum { get; set; }

        public bool AdMinimumIsSet { get { return AdMinimum != null; } }
    }

    [DataContract(Name = "ROMethodAllocationOverrideProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationOverrideProperties : ROMethodProperties
    {
        // fields specific to Allocation Override method
        [DataMember(IsRequired = true)]
        int? _storeGradeWeekCount;
        [DataMember(IsRequired = true)]
        double? _percentNeedLimit;
        [DataMember(IsRequired = true)]
        bool _exceedMaxInd;
        [DataMember(IsRequired = true)]
        double? _reserve;
        [DataMember(IsRequired = true)]
        bool _percentInd;
        [DataMember(IsRequired = true)]
        double? _reserveAsBulk;
        [DataMember(IsRequired = true)]
        double? _reserveAsPacks;
        [DataMember(IsRequired = true)]
        eMerchandiseType _merchandiseType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _merchandiseHierarchy;
        [DataMember(IsRequired = true)]
        eMerchandiseType _onHandMerchandiseType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _onHandMerchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _onHandMerchandiseHierarchy;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _hierarchyLevels;
        [DataMember(IsRequired = true)]
        double? _onHandFactor;
        [DataMember(IsRequired = true)]
        int? _colorMult;
        [DataMember(IsRequired = true)]
        int? _sizeMult;
        [DataMember(IsRequired = true)]
        int? _allColorMin;
        [DataMember(IsRequired = true)]
        int? _allColorMax;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _capacityAttribute;
        [DataMember(IsRequired = true)]
        bool _exceedCapacity;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeGradesAttribute;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeGradesAttributeSet;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeGradesMerchandise;
        [DataMember(IsRequired = true)]
        eMinMaxType _inventoryIndicator;
        [DataMember(IsRequired = true)]
        eMerchandiseType _inventoryBasisMerchType;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _inventoryBasisMerchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _inventoryBasisMerchandiseHierarchy;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _vswAttribute;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _vswAttributeSet;
        [DataMember(IsRequired = true)]
        bool _doNotApplyVSW;
        [DataMember(IsRequired = true)]
        private ROAttributeSetAllocationStoreGrade _storeGradeValues;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideCapacityProperties> _capacity;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideColorProperties> _colorMinMax;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverridePackRoundingProperties> _packRounding;
        [DataMember(IsRequired = true)]
        private ROMethodOverrideVSWAttributeSet _vswAttributeSetValues;

        #region Public Properties

        public bool StoreGradeWeekCountIsSet
        {
            get { return _storeGradeWeekCount != null; }
        }

        public int? StoreGradeWeekCount
        {
            get { return _storeGradeWeekCount; }
            set { _storeGradeWeekCount = value; }
        }

        public bool PercentNeedLimitIsSet
        {
            get { return _percentNeedLimit != null; }
        }

        public double? PercentNeedLimit
        {
            get { return _percentNeedLimit; }
            set { _percentNeedLimit = value; }
        }

        public bool ExceedMaxInd
        {
            get { return _exceedMaxInd; }
            set { _exceedMaxInd = value; }
        }

        public bool ReserveIsSet
        {
            get { return _reserve != null; }
        }

        public double? Reserve
        {
            get { return _reserve; }
            set { _reserve = value; }
        }

        public bool PercentInd
        {
            get { return _percentInd; }
            set { _percentInd = value; }
        }

        public bool ReserveAsBulkIsSet
        {
            get { return _reserveAsBulk != null; }
        }

        public double? ReserveAsBulk
        {
            get { return _reserveAsBulk; }
            set { _reserveAsBulk = value; }
        }

        public bool ReserveAsPacksIsSet
        {
            get { return _reserveAsPacks != null; }
        }

        public double? ReserveAsPacks
        {
            get { return _reserveAsPacks; }
            set { _reserveAsPacks = value; }
        }

        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the  merchandise is set.
		/// </summary>
		public bool MerchandiseIsSet
        {
            get { return !_merchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the merchandise hierarchy is set.
		/// </summary>
		public bool MerchandiseHierarchyIsSet
        {
            get { return !_merchandiseHierarchy.Equals(default(KeyValuePair<int, int>)); }
        }

        public KeyValuePair<int, int> MerchandiseHierarchy
        {
            get { return _merchandiseHierarchy; }
            set { _merchandiseHierarchy = value; }
        }

        public eMerchandiseType OnHandMerchandiseType
        {
            get { return _onHandMerchandiseType; }
            set { _onHandMerchandiseType = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the on hand merchandise is set.
		/// </summary>
		public bool OnHandMerchandiseIsSet
        {
            get { return !_onHandMerchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> OnHandMerchandise
        {
            get { return _onHandMerchandise; }
            set { _onHandMerchandise = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the on hand merchandise hierarchy is set.
		/// </summary>
		public bool OnHandMerchandiseHierarchyIsSet
        {
            get { return !_onHandMerchandiseHierarchy.Equals(default(KeyValuePair<int, int>)); }
        }

        public KeyValuePair<int, int> OnHandMerchandiseHierarchy
        {
            get { return _onHandMerchandiseHierarchy; }
            set { _onHandMerchandiseHierarchy = value; }
        }

        /// <summary>
        /// Gets the list of hierarchy levels
        /// </summary>
        public List<KeyValuePair<int, string>> HierarchyLevels
        {
            get { return _hierarchyLevels; }
            set { _hierarchyLevels = value; }
        }

        public bool OnHandFactorIsSet
        {
            get { return _onHandFactor != null; }
        }

        public double? OnHandFactor
        {
            get { return _onHandFactor; }
            set { _onHandFactor = value; }
        }

        public bool ColorMultIsSet
        {
            get { return _colorMult != null; }
        }

        public int? ColorMult
        {
            get { return _colorMult; }
            set { _colorMult = value; }
        }

        public bool SizeMultIsSet
        {
            get { return _sizeMult != null; }
        }

        public int? SizeMult
        {
            get { return _sizeMult; }
            set { _sizeMult = value; }
        }

        public bool AllColorMinIsSet
        {
            get { return _allColorMin != null; }
        }

        public int? AllColorMin
        {
            get { return _allColorMin; }
            set { _allColorMin = value; }
        }

        public bool AllColorMaxIsSet
        {
            get { return _allColorMax != null; }
        }

        public int? AllColorMax
        {
            get { return _allColorMax; }
            set { _allColorMax = value; }
        }

        public KeyValuePair<int, string> CapacityAttribute
        {
            get { return _capacityAttribute; }
            set { _capacityAttribute = value; }
        }

        public bool ExceedCapacity
        {
            get { return _exceedCapacity; }
            set { _exceedCapacity = value; }
        }

        public KeyValuePair<int, string> StoreGradesAttribute
        {
            get { return _storeGradesAttribute; }
            set { _storeGradesAttribute = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the store grades attribute set is set.
		/// </summary>
		public bool StoreGradesAttributeSetIsSet
        {
            get { return !_storeGradesAttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> StoreGradesAttributeSet
        {
            get { return _storeGradesAttributeSet; }
            set { _storeGradesAttributeSet = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the store grades merchandise is set.
		/// </summary>
		public bool StoreGradesMerchandiseIsSet
        {
            get { return !_storeGradesMerchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> StoreGradesMerchandise
        {
            get { return _storeGradesMerchandise; }
            set { _storeGradesMerchandise = value; }
        }

        public eMinMaxType InventoryIndicator
        {
            get { return _inventoryIndicator; }
            set
            {
                _inventoryIndicator = value;
            }
        }

        public eMerchandiseType InventoryBasisMerchType
        {
            get { return _inventoryBasisMerchType; }
            set
            {
                _inventoryBasisMerchType = value;
            }
        }

        public KeyValuePair<int, string> InventoryBasisMerchandise
        {
            get { return _inventoryBasisMerchandise; }
            set { _inventoryBasisMerchandise = value; }
        }

        public KeyValuePair<int, int> InventoryBasisMerchandiseHierarchy
        {
            get { return _inventoryBasisMerchandiseHierarchy; }
            set { _inventoryBasisMerchandiseHierarchy = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the VSW attribute is set.
		/// </summary>
		public bool VSWAttributeIsSet
        {
            get { return !_vswAttribute.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> VSWAttribute
        {
            get { return _vswAttribute; }
            set { _vswAttribute = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the store grades attribute set is set.
		/// </summary>
		public bool VSWAttributeSetIsSet
        {
            get { return !_vswAttributeSet.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> VSWAttributeSet
        {
            get { return _vswAttributeSet; }
            set { _vswAttributeSet = value; }
        }

        public bool DoNotApplyVSW
        {
            get { return _doNotApplyVSW; }
            set { _doNotApplyVSW = value; }
        }

        public ROAttributeSetAllocationStoreGrade StoreGradeValues
        {
            get { return _storeGradeValues; }
            set { _storeGradeValues = value; }
        }

        public List<ROMethodOverrideCapacityProperties> Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        public List<ROMethodOverrideColorProperties> ColorMinMax
        {
            get { return _colorMinMax; }
            set { _colorMinMax = value; }
        }

        public List<ROMethodOverridePackRoundingProperties> PackRounding
        {
            get { return _packRounding; }
            set { _packRounding = value; }
        }

        public ROMethodOverrideVSWAttributeSet VSWAttributeSetValues
        {
            get { return _vswAttributeSetValues; }
            set { _vswAttributeSetValues = value; }
        }

        #endregion
        public ROMethodAllocationOverrideProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            int? storeGradeWeekCount, 
            double? percentNeedLimit,
            bool exceedMaxInd, 
            double? reserve, 
            bool percentInd, 
            double? reserveAsBulk, 
            double? reserveAsPacks,
            eMerchandiseType merchandiseType,
            KeyValuePair<int, string> merchandise, 
            KeyValuePair<int, int> merchandiseHierarchy,
            eMerchandiseType onHandMerchandiseType,
            KeyValuePair<int, string> onHandMerchandise, 
            KeyValuePair<int, int> onHandMerchandiseHierarchy, 
            double? onHandFactor, 
            int? colorMult, 
            int? sizeMult, 
            int? allColorMin, 
            int? allColorMax,
            KeyValuePair<int, string> capacityAttribute, 
            bool exceedCapacity, 
            KeyValuePair<int, string> storeGradesAttribute,
            eMinMaxType inventoryIndicator, 
            eMerchandiseType inventoryBasisMerchType, 
            KeyValuePair<int, string> inventoryBasisMerchandise, 
            KeyValuePair<int, int> inventoryBasisMerchandiseHierarchy,
            KeyValuePair<int, string> vswAttribute, 
            bool doNotApplyVSW, 
            ROAttributeSetAllocationStoreGrade storeGradeValues, 
            List<ROMethodOverrideCapacityProperties> capacity,
            List<ROMethodOverrideColorProperties> colorMinMax, 
            List<ROMethodOverridePackRoundingProperties> packRounding, 
            ROMethodOverrideVSWAttributeSet vswAttributeSetValues,
            KeyValuePair<int, string> storeGradesAttributeSet = default(KeyValuePair<int, string>),
            bool isTemplate = false,
            KeyValuePair<int, string> storeGradesMerchandise = default(KeyValuePair<int, string>),
            KeyValuePair<int, string> vswAttributeSet = default(KeyValuePair<int, string>)
            ) 
            : base(
                  eMethodType.AllocationOverride, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )
        {
            // fields specific to Allocation Override method
            _storeGradeWeekCount = storeGradeWeekCount;
            _percentNeedLimit = percentNeedLimit;
            _exceedMaxInd = exceedMaxInd;
            _reserve = reserve;
            _percentInd = percentInd;
            _reserveAsBulk = reserveAsBulk;
            _reserveAsPacks = reserveAsPacks;
            _merchandiseType = merchandiseType;
            _merchandise = merchandise;
            _merchandiseHierarchy = merchandiseHierarchy;
            _onHandMerchandiseType = onHandMerchandiseType;
            _onHandMerchandise = onHandMerchandise;
            _onHandMerchandiseHierarchy = onHandMerchandiseHierarchy;
            _onHandFactor = onHandFactor;
            _colorMult = colorMult;
            _sizeMult = sizeMult;
            _allColorMin = allColorMin;
            _allColorMax = allColorMax;
            _capacityAttribute = capacityAttribute;
            _exceedCapacity = exceedCapacity;
            _storeGradesAttribute = storeGradesAttribute;
            _storeGradesAttributeSet = storeGradesAttributeSet;
            _storeGradesMerchandise = storeGradesMerchandise;
            _inventoryIndicator = inventoryIndicator;
            _inventoryBasisMerchType = inventoryBasisMerchType;
            _inventoryBasisMerchandise = inventoryBasisMerchandise;
            _inventoryBasisMerchandiseHierarchy = inventoryBasisMerchandiseHierarchy;
            _vswAttribute = vswAttribute;
            _vswAttributeSet = vswAttributeSet;
            _doNotApplyVSW = doNotApplyVSW;
            _storeGradeValues = storeGradeValues;
            _capacity = capacity;
            _colorMinMax = colorMinMax;
            _packRounding = packRounding;
            _vswAttributeSetValues = vswAttributeSetValues;
            _hierarchyLevels = new List<KeyValuePair<int, string>>();
        }

    }

    //[DataContract(Name = "ROMethodOverrideStoreGradeProperties", Namespace = "http://Logility.ROWeb/")]
    //public class ROMethodOverrideStoreGradeProperties : ROAllocationStoreGrade
    //{
    //    [DataMember(IsRequired = true)]
    //    KeyValuePair<int, string> _attributeSet;   // store group
    //    [DataMember(IsRequired = true)]
    //    int _shipUpTo;
    //
    //    #region Public Properties
    //    public KeyValuePair<int, string> AttributeSet
    //    {
    //        get { return _attributeSet; }
    //        set { _attributeSet = value; }
    //    }
    //
    //    public int ShipUpTo
    //    {
    //        get { return _shipUpTo; }
    //        set { _shipUpTo = value; }
    //    }

    //    #endregion

    //    public ROMethodOverrideStoreGradeProperties(KeyValuePair<int, string> attributeSet, int shipUpTo) : base()
    //    //base(storeGrade, )
    //    {
    //        // fields specific to Allocation Override Store Grade
    //        ROAllocationStoreGrade sg = new ROAllocationStoreGrade();
    //        sg.StoreGrade = new System.Collections.Generic.KeyValuePair<int, string>(1, "A");
    //        _attributeSet = attributeSet;
    //        _shipUpTo = shipUpTo;
    //    }
    //}

    [DataContract(Name = "ROMethodOverrideCapacityProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverrideCapacityProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;     //storeGroupLevel
        [DataMember(IsRequired = true)]
        bool _exceedCapacity;
        [DataMember(IsRequired = true)]
        double? _exceedByPct;

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public bool ExceedCapacity
        {
            get { return _exceedCapacity; }
            set { _exceedCapacity = value; }
        }

        public bool ExceedByPctIsSet
        {
            get { return _exceedByPct != null; }
        }

        public double? ExceedByPct
        {
            get { return _exceedByPct; }
            set { _exceedByPct = value; }
        }

        public ROMethodOverrideCapacityProperties(KeyValuePair<int, string> attributeSet, bool exceedCapacity, double? exceedByPct)
        {
            _attributeSet = attributeSet;
            _exceedCapacity = exceedCapacity;
            _exceedByPct = exceedByPct;
        }
    }

    [DataContract(Name = "ROMethodOverrideColorProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverrideColorProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _colorCode;
        [DataMember(IsRequired = true)]
        int? _colorMinimum;
        [DataMember(IsRequired = true)]
        int? _colorMaximum;

        public KeyValuePair<int, string> ColorCode
        {
            get { return _colorCode; }
            set { _colorCode = value; }
        }

        public bool ColorMinimumIsSet
        {
            get { return _colorMinimum != null; }
        }

        public int? ColorMinimum
        {
            get { return _colorMinimum; }
            set { _colorMinimum = value; }
        }

        public bool ColorMaximumIsSet
        {
            get { return _colorMaximum != null; }
        }

        public int? ColorMaximum
        {
            get { return _colorMaximum; }
            set { _colorMaximum = value; }
        }

        public ROMethodOverrideColorProperties(KeyValuePair<int, string> colorCode, int? colorMinimum, int? colorMaximum)
        {
            // fields specific to Allocation Override Color
            _colorCode = colorCode;
            _colorMinimum = colorMinimum;
            _colorMaximum = colorMaximum;
        }
    }

    [DataContract(Name = "ROMethodOverridePackRoundingProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverridePackRoundingProperties
    {
        [DataMember(IsRequired = true)]
        string _packName;
        [DataMember(IsRequired = true)]
        int _packMultiple;
        [DataMember(IsRequired = true)]
        double? _firstPackPct;
        [DataMember(IsRequired = true)]
        double? _nthPackPct;

        public string PackName
        {
            get { return _packName; }
            set { _packName = value; }
        }

        public int PackMultiple
        {
            get { return _packMultiple; }
            set { _packMultiple = value; }
        }

        public bool FirstPackPctIsSet
        {
            get { return _firstPackPct != null; }
        }

        public double? FirstPackPct
        {
            get { return _firstPackPct; }
            set { _firstPackPct = value; }
        }

        public bool NthPackPctIsSet
        {
            get { return _nthPackPct != null; }
        }

        public double? NthPackPct
        {
            get { return _nthPackPct; }
            set { _nthPackPct = value; }
        }

        public ROMethodOverridePackRoundingProperties(
            int packMultiple, 
            double? firstPackPct = null, 
            double? nthPackPct = null,
            string packName = null
            )
        {
            // fields specific to Allocation Override Pack Rounding
            _packName = packName;
            _packMultiple = packMultiple;
            _firstPackPct = firstPackPct;
            _nthPackPct = nthPackPct;
        }
    }


    [DataContract(Name = "ROMethodOverrideVSWAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverrideVSWAttributeSet
    {
        [DataMember(IsRequired = true)]
        ROMethodOverrideVSW _vswAttributeSetValues;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideVSW> _vswStoresValues;

        public ROMethodOverrideVSW VSWAttributeSetValues
        {
            get { return _vswAttributeSetValues; }
            set { _vswAttributeSetValues = value; }
        }

        public List<ROMethodOverrideVSW> VSWStoresValues
        {
            get
            {
                if (_vswStoresValues == null)
                {
                    _vswStoresValues = new List<ROMethodOverrideVSW>();
                }
                return _vswStoresValues;
            }
            set { _vswStoresValues = value; }
        }

    }

    [DataContract(Name = "ROMethodOverrideVSW", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverrideVSW
    {
        [DataMember(IsRequired = true)]
        bool _updated;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _entry;
        [DataMember(IsRequired = true)]
        string _reservationStore;
        [DataMember(IsRequired = true)]
        int? _minimumShipQuantity;
        [DataMember(IsRequired = true)]
        double? _pctPackThreshold;
        [DataMember(IsRequired = true)]
        int? _itemMaximum;

        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        /// <summary>
        /// Gets a flag identifying if the entry is set.
        /// </summary>
        public bool EntryIsSet
        {
            get { return !Entry.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }

        public string ReservationStore
        {
            get { return _reservationStore; }
            set { _reservationStore = value; }
        }

        public bool MinimumShipQuantityIsSet
        {
            get { return _minimumShipQuantity != null; }
        }

        public int? MinimumShipQuantity
        {
            get { return _minimumShipQuantity; }
            set { _minimumShipQuantity = value; }
        }

        public bool PctPackThresholdIsSet
        {
            get { return _pctPackThreshold != null; }
        }

        public double? PctPackThreshold
        {
            get { return _pctPackThreshold; }
            set { _pctPackThreshold = value; }
        }

        public bool ItemMaximumIsSet
        {
            get { return _itemMaximum != null; }
        }

        public int? ItemMaximum
        {
            get { return _itemMaximum; }
            set { _itemMaximum = value; }
        }

        public ROMethodOverrideVSW(
            bool updated, 
            KeyValuePair<int, string> entry = default(KeyValuePair<int, string>), 
            string reservationStore = null, 
            int? minimumShipQuantity = null, 
            double? pctPackThreshold = null, 
            int? itemMaximum = null
            )
        {
            // fields specific to Allocation Override Color
            _updated = updated;
            _entry = entry;
            _reservationStore = reservationStore;
            _minimumShipQuantity = minimumShipQuantity;
            _pctPackThreshold = pctPackThreshold;
            _itemMaximum = itemMaximum;
        }
    }

     [DataContract(Name = "ROMethodCreateMasterHeadersProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodCreateMasterHeadersProperties : ROMethodProperties
    {
        // fields specific to Create Master Headers method 
        [DataMember(IsRequired = true)]
        bool _useSelectedHeaders;
        [DataMember(IsRequired = true)]
        private List<ROMethodCreateMasterHeadersMerchandise> _listMerchandise;

        public bool UseSelectedHeaders
        {
            get { return _useSelectedHeaders; }
            set { _useSelectedHeaders = value; }
        }

        public List<ROMethodCreateMasterHeadersMerchandise> ListMerchandise
        {
            get { return _listMerchandise; }
            set { _listMerchandise = value; }
        }

        public ROMethodCreateMasterHeadersProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            bool useSelectedHeaders, 
            List<ROMethodCreateMasterHeadersMerchandise> listMerchandise,
            bool isTemplate = false
            ) 
            : base(
                  eMethodType.CreateMasterHeaders, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )
        {
            _useSelectedHeaders = useSelectedHeaders;
            _listMerchandise = listMerchandise;
        }
    }

    [DataContract(Name = "ROMethodCreateMasterHeadersMerchandise", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodCreateMasterHeadersMerchandise
    {
        [DataMember(IsRequired = true)]
        int _sequence;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;

        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }
        public KeyValuePair<int, string> Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }
        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public ROMethodCreateMasterHeadersMerchandise(int sequence, KeyValuePair<int, string> merchandise, KeyValuePair<int, string> filter)
        {
            _sequence = sequence;
            _merchandise = merchandise;
            _filter = filter;
        }
    }

    [DataContract(Name = "ROMethodDCCartonRoundingProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodDCCartonRoundingProperties : ROMethodProperties
    {
        // fields specific to DC Carton Rounding method 
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        eAllocateOverageTo _allocateOverageTo;

        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public eAllocateOverageTo AllocateOverageTo
        {
            get { return _allocateOverageTo; }
            set { _allocateOverageTo = value; }
        }

        public ROMethodDCCartonRoundingProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey, 
            KeyValuePair<int, string> attribute, 
            eAllocateOverageTo allocateOverageTo,
            bool isTemplate = false) 
            : base(
                eMethodType.DCCartonRounding, 
                method, 
                description, 
                userKey,
                isTemplate)
        {
            _attribute = attribute;
            _allocateOverageTo = allocateOverageTo;
        }
    }


    [DataContract(Name = "ROMethodAllocationVelocityProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationVelocityProperties : ROMethodProperties
    {
        // fields specific to Allocation Velocity method
        [DataMember(IsRequired = true)]
        private eVelocityCalculateAverageUsing _calculateAverageUsing;
        [DataMember(IsRequired = true)]
        private eVelocityDetermineShipQtyUsing _determineShipQtyUsing;
        [DataMember(IsRequired = true)]
        private eVelocityApplyMinMaxType _applyMinMaxType;
        [DataMember(IsRequired = true)]
        private eVelocityMethodGradeVariableType _gradeVariableType;
        [DataMember(IsRequired = true)]
        private bool _useSimilarStoreHistory;
        [DataMember(IsRequired = true)]
        private bool _balance;
        [DataMember(IsRequired = true)]
        private bool _balanceToHeader;
        [DataMember(IsRequired = true)]
        private bool _reconcile;

        // Basis fields
        [DataMember(IsRequired = true)]
        private List<ROBasisWithLevelDetailProfile> _basisProfiles;
        [DataMember(IsRequired = true)]
        List<ROMerchandiseListEntry> _merchandiseList;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _basisVersions;

        // Store Grade fields
        [DataMember(IsRequired = true)]
        private eMinMaxType _inventoryIndicator;
        [DataMember(IsRequired = true)]
        private eMerchandiseType _inventoryMinMaxMerchType;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _inventoryMinMaxMerchandise;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, int> _inventoryMinMaxMerchandiseHierarchy;
        [DataMember(IsRequired = true)]
        private List<ROAllocationVelocityGrade> _velocityGradeList;
        [DataMember(IsRequired = true)]
        private List<ROSellThruList> _sellThruList;

        // Matrix fields
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attribute;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _attributeSetList;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _noOnHandRuleList;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _onHandRuleList;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _matrixModeRuleList;
        [DataMember(IsRequired = true)]
        private ROMethodAllocationVelocityAttributeSet _matrixAttributeSetValues;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _matrixViews;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _matrixSelectedView;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<string, bool>> _matrixViewColumns;
        [DataMember(IsRequired = true)]
        private bool _interactive;
        [DataMember(IsRequired = true)]
        private eVelocityAction _velocityAction;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _components;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _selectedComponent;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _velocityGradesMerchandise;
        

        #region Public Properties
        /// <summary>
        /// Gets or sets the Calculate Average Using value
        /// </summary>
        public eVelocityCalculateAverageUsing CalculateAverageUsing
        {
            get { return _calculateAverageUsing; }
            set
            {
                _calculateAverageUsing = value;
            }
        }

        /// <summary>
		/// Gets or sets the Determine Ship Qty Using value
		/// </summary>
		public eVelocityDetermineShipQtyUsing DetermineShipQtyUsing
        {
            get { return _determineShipQtyUsing; }
            set
            {
                _determineShipQtyUsing = value;
            }
        }

        /// <summary>
		/// Gets or sets the Apply Min Max Type
		/// </summary>
		public eVelocityApplyMinMaxType ApplyMinMaxType
        {
            get { return _applyMinMaxType; }
            set
            {
                _applyMinMaxType = value;
            }
        }

        /// <summary>
		/// Gets or sets the Grade Variable Type
		/// </summary>
		public eVelocityMethodGradeVariableType GradeVariableType
        {
            get { return _gradeVariableType; }
            set
            {
                _gradeVariableType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Use Similar Store History flag
        /// </summary>
        public bool UseSimilarStoreHistory
        {
            get { return _useSimilarStoreHistory; }
            set { _useSimilarStoreHistory = value; }
        }

        /// <summary>
        /// Gets or sets the flag that will be analyzed to balance the velocity results or not
        /// </summary>
        public bool Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        /// <summary>
        /// Gets or sets the flag that will be analyzed to determine if the velocity results will be balanced to the headers
        /// </summary>
        public bool BalanceToHeader
        {
            get { return _balanceToHeader; }
            set { _balanceToHeader = value; }
        }

        /// <summary>
        /// Gets or sets the flag that will be analyzed to determine if the velocity results will be reconciled
        /// </summary>
        public bool Reconcile
        {
            get { return _reconcile; }
            set { _reconcile = value; }
        }

        // Basis properties
        /// <summary>
        /// Gets the list of basis profiles
        /// </summary>
        public List<ROBasisWithLevelDetailProfile> BasisProfiles
        {
            get { return _basisProfiles; }
        }

        /// <summary>
        /// Gets the list of merchandise entries for the merchandise dropdown list
        /// </summary>
        public List<ROMerchandiseListEntry> MerchandiseList
        {
            get { return _merchandiseList; }
        }

        /// <summary>
        /// List of KeyValuePairs for basis version with version key and name
        /// </summary>
        public List<KeyValuePair<int, string>> BasisVersions { get { return _basisVersions; } }

        /// <summary>
        /// Gets the flag identifying if there are basis versions.
        /// </summary>
        public bool HasBasisVersions
        {
            get { return _basisVersions.Count > 0; }
        }

        // Store Grade properties
        public eMinMaxType InventoryIndicator
        {
            get { return _inventoryIndicator; }
            set
            {
                _inventoryIndicator = value;
            }
        }

        public eMerchandiseType InventoryMinMaxMerchType
        {
            get { return _inventoryMinMaxMerchType; }
            set
            {
                _inventoryMinMaxMerchType = value;
            }
        }

        public KeyValuePair<int, string> InventoryMinMaxMerchandise
        {
            get { return _inventoryMinMaxMerchandise; }
            set { _inventoryMinMaxMerchandise = value; }
        }

        public KeyValuePair<int, int> InventoryMinMaxMerchandiseHierarchy
        {
            get { return _inventoryMinMaxMerchandiseHierarchy; }
            set { _inventoryMinMaxMerchandiseHierarchy = value; }
        }

        /// <summary>
        /// Gets the list of store grades
        /// </summary>
        public List<ROAllocationVelocityGrade> VelocityGradeList
        {
            get { return _velocityGradeList; }
        }

        /// <summary>
        /// Gets the list of sell thru values
        /// </summary>
        public List<ROSellThruList> SellThruList
        {
            get { return _sellThruList; }
        }

        // Matrix properties
        /// <summary>
        /// Gets or sets the attribute to use for matrix data
        /// </summary
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        /// Gets or sets the attribute set to use for matrix data
        /// </summary
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        /// <summary>
        /// Gets the list of attribute sets
        /// </summary>
        public List<KeyValuePair<int, string>> AttributeSetList
        {
            get { return _attributeSetList; }
        }

        /// <summary>
        /// Gets the list of rules for stores with no on hand values
        /// </summary>
        public List<KeyValuePair<int, string>> NoOnHandRuleList
        {
            get { return _noOnHandRuleList; }
        }

        /// <summary>
        /// Gets the list of rules on hand values
        /// </summary>
        public List<KeyValuePair<int, string>> OnHandRuleList
        {
            get { return _onHandRuleList; }
        }

        /// <summary>
        /// Gets the list of matrix mode rule values
        /// </summary>
        public List<KeyValuePair<int, string>> MatrixModeRuleList
        {
            get { return _matrixModeRuleList; }
        }

        /// <summary>
        /// Gets the attribute set values
        /// </summary>
        public ROMethodAllocationVelocityAttributeSet MatrixAttributeSetValues
        {
            get { return _matrixAttributeSetValues; }
            set { _matrixAttributeSetValues = value; }
        }

        /// <summary>
        /// Gets or sets the list of matrix views
        /// </summary>
        public List<KeyValuePair<int, string>> MatrixViews
        {
            get { return _matrixViews; }
            set { _matrixViews = value; }
        }

        /// <summary>
        /// Gets or sets the selected matrix view
        /// </summary>
        public KeyValuePair<int, string> MatrixSelectedView
        {
            get { return _matrixSelectedView; }
            set { _matrixSelectedView = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the matrix selected view is set.
		/// </summary>
		public bool MatrixSelectedViewIsSet
        {
            get { return !MatrixSelectedView.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets the list of matrix view view columns with a flag identifying if the column is visible
        /// </summary>
        public List<KeyValuePair<string, bool>> MatrixViewColumns
        {
            get { return _matrixViewColumns; }
        }

        /// <summary>
        /// Gets or sets the Interactive flag
        /// </summary>
        public bool Interactive
        {
            get { return _interactive; }
            set { _interactive = value; }
        }

        /// <summary>
        /// Gets or sets the velocity action to perform
        /// </summary>
        public eVelocityAction VelocityAction
        {
            get { return _velocityAction; }
            set { _velocityAction = value; }
        }

        

        /// <summary>
        /// Gets or sets the list of components
        /// </summary>
        public List<KeyValuePair<int, string>> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        /// <summary>
        /// Gets or sets the selected component
        /// </summary>
        public KeyValuePair<int, string> SelectedComponent
        {
            get { return _selectedComponent; }
            set { _selectedComponent = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the selected component is set.
		/// </summary>
		public bool SelectedComponentIsSet
        {
            get { return !SelectedComponent.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets a flag identifying if the velocity grades merchandise is set.
        /// </summary>
        public bool VelocityGradesMerchandiseIsSet
        {
            get { return !_velocityGradesMerchandise.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> VelocityGradesMerchandise
        {
            get { return _velocityGradesMerchandise; }
            set { _velocityGradesMerchandise = value; }
        }

        #endregion
        public ROMethodAllocationVelocityProperties(
            KeyValuePair<int, string> method, 
            string description, 
            int userKey,
            eVelocityCalculateAverageUsing calculateAverageUsing,
            eVelocityDetermineShipQtyUsing determineShipQtyUsing,
            eVelocityApplyMinMaxType applyMinMaxType,
            eVelocityMethodGradeVariableType gradeVariableType,
            bool useSimilarStoreHistory,
            bool balance,
            bool balanceToHeader,
            bool reconcile,
            eMinMaxType inventoryIndicator, 
            eMerchandiseType inventoryMinMaxMerchType, 
            KeyValuePair<int, string> inventoryMinMaxMerchandise, 
            KeyValuePair<int, int> inventoryMinMaxMerchandiseHierarchy,
            KeyValuePair<int, string> attribute,
            KeyValuePair<int, string> attributeSet,
            bool isTemplate = false,
            KeyValuePair<int, string> velocityGradesMerchandise = default(KeyValuePair<int, string>)
            ) 
            : base(
                  eMethodType.Velocity, 
                  method, 
                  description, 
                  userKey,
                  isTemplate
                  )
        {
            // fields specific to Allocation Velocity method
            
            CalculateAverageUsing = calculateAverageUsing;
            DetermineShipQtyUsing = determineShipQtyUsing;
            ApplyMinMaxType = applyMinMaxType;
            GradeVariableType = gradeVariableType;

            _useSimilarStoreHistory = useSimilarStoreHistory;
            _balance = balance;
            _balanceToHeader = balanceToHeader;
            _reconcile = reconcile;

            _basisProfiles = new List<ROBasisWithLevelDetailProfile>();
            _merchandiseList = new List<ROMerchandiseListEntry>();
            _basisVersions = new List<KeyValuePair<int, string>>();

            InventoryIndicator = inventoryIndicator;
            InventoryMinMaxMerchType = inventoryMinMaxMerchType;
            _inventoryMinMaxMerchandise = inventoryMinMaxMerchandise;
            _inventoryMinMaxMerchandiseHierarchy = inventoryMinMaxMerchandiseHierarchy;

            _velocityGradeList = new List<ROAllocationVelocityGrade>();
            _sellThruList = new List<ROSellThruList>();

            _attribute = attribute;
            _attributeSet = attributeSet;
            _attributeSetList = new List<KeyValuePair<int, string>>();
            _noOnHandRuleList = new List<KeyValuePair<int, string>>();
            _onHandRuleList = new List<KeyValuePair<int, string>>();
            _matrixModeRuleList = new List<KeyValuePair<int, string>>();
            _matrixViews = new List<KeyValuePair<int, string>>();
            _matrixSelectedView = default(KeyValuePair<int, string>);
            _matrixViewColumns = new List<KeyValuePair<string, bool>>();
            _interactive = false;
            _velocityAction = eVelocityAction.None;
            _components = new List<KeyValuePair<int, string>>();
            _selectedComponent = default(KeyValuePair<int, string>);
            _velocityGradesMerchandise = velocityGradesMerchandise;
        }

    }

    [DataContract(Name = "ROMethodAllocationVelocityAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationVelocityAttributeSet
    {
        // fields specific to Allocation Velocify attribute set
        
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _attributeSet;
        [DataMember(IsRequired = true)]
        private eVelocityRuleType _noOnHandRule;
        [DataMember(IsRequired = true)]
        private double? _noOnHandRuleValue;
        [DataMember(IsRequired = true)]
        private eVelocityRuleType _onHandRule;
        [DataMember(IsRequired = true)]
        private double? _onHandRuleValue;
        [DataMember(IsRequired = true)]
        private eVelocityMatrixMode _matrixMode;
        [DataMember(IsRequired = true)]
        private eVelocityRuleRequiresQuantity _matrixModeAverageRule;
        [DataMember(IsRequired = true)]
        private double? _matrixModeAverageRuleValue;
        [DataMember(IsRequired = true)]
        private eVelocitySpreadOption _spreadOption;
        [DataMember(IsRequired = true)]
        private double? _allStoresAverageWOS;
        [DataMember(IsRequired = true)]
        private double? _allStoresSellThruPercent;
        [DataMember(IsRequired = true)]
        private double? _averageWOS;
        [DataMember(IsRequired = true)]
        private double? _sellThruPercent;
        [DataMember(IsRequired = true)]
        private List<ROMethodAllocationVelocityMatrixVelocityGrade> _matrixGradeValues;

        #region Public Properties

        /// <summary>
        /// Gets or sets the attribute set to use for matrix data
        /// </summary
        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        /// <summary>
		/// Gets or sets the value of the matrix mode
		/// </summary>
		public eVelocityMatrixMode MatrixMode
        {
            get { return _matrixMode; }
            set
            {
                _matrixMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the key of the rule selected for stores with no onhand values
        /// </summary
        public eVelocityRuleType NoOnHandRule
        {
            get { return _noOnHandRule; }
            set
            {
                _noOnHandRule = value;
            }
        }
        /// <summary>
        /// Gets a flag to identify if the key of the rule selected for stores with no onhand is set
        /// </summary
        public bool NoOnHandRuleIsSet
        {
            get { return _noOnHandRule != eVelocityRuleType.None; }
        }

        /// <summary>
        /// Gets or sets the value of the rule selected for stores with no onhand
        /// </summary
        public double? NoOnHandRuleValue
        {
            get { return _noOnHandRuleValue; }
            set { _noOnHandRuleValue = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the value of the rule selected for stores with no onhand is set
        /// </summary
        public bool NoOnHandRuleValueIsSet
        {
            get { return _noOnHandRuleValue != null; }
        }

        /// <summary>
        /// Gets or sets the type of the rule selected for stores with onhand
        /// </summary
        public eVelocityRuleType OnHandRule
        {
            get { return _onHandRule; }
            set { _onHandRule = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the key of the rule selected for stores with onhand is set
        /// </summary
        public bool OnHandRuleIsSet
        {
            get { return _onHandRule != eVelocityRuleType.None; }
        }

        /// <summary>
        /// Gets or sets the value of the rule selected for stores with onhand
        /// </summary
        public double? OnHandRuleValue
        {
            get { return _onHandRuleValue; }
            set { _onHandRuleValue = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the value of the rule selected for stores with onhand is set
        /// </summary
        public bool OnHandRuleValueIsSet
        {
            get { return _onHandRuleValue != null; }
        }

        /// <summary>
        /// Gets or sets the key of the rule selected for matrix mode average
        /// </summary
        public eVelocityRuleRequiresQuantity MatrixModeAverageRule
        {
            get { return _matrixModeAverageRule; }
            set
            {
                _matrixModeAverageRule = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the rule selected for matrix mode average
        /// </summary
        public double? MatrixModeAverageRuleValue
        {
            get { return _matrixModeAverageRuleValue; }
            set { _matrixModeAverageRuleValue = value; }
        }

        /// <summary>
		/// Gets or sets the value of the spread option
		/// </summary>
		public eVelocitySpreadOption SpreadOption
        {
            get { return _spreadOption; }
            set
            {
                _spreadOption = value;
            }
        }

        /// <summary>
		/// Gets or sets the average WOS for the all stores when calculated interactively
		/// </summary>
		public double? AllStoresAverageWOS
        {
            get { return _allStoresAverageWOS; }
            set { _allStoresAverageWOS = value; }
        }

        /// <summary>
        /// Gets a flag to identify if the all stores average weeks of supply for all stores is set
        /// </summary
        public bool AllStoresAverageWOSIsSet
        {
            get { return _allStoresAverageWOS != null; }
        }

        /// <summary>
		/// Gets or sets the sell thru percent for the all stores when calculated interactively
		/// </summary>
		public double? AllStoresSellThruPercent
        {
            get { return _allStoresSellThruPercent; }
            set { _allStoresSellThruPercent = value; }
        }

        /// <summary>
        /// Gets a flag to identify if the sell thru percent for all stores is set
        /// </summary
        public bool AllStoresSellThruPercentIsSet
        {
            get { return _allStoresSellThruPercent != null; }
        }

        /// <summary>
		/// Gets or sets the average WOS for the set when calculated interactively
		/// </summary>
		public double? AverageWOS
        {
            get { return _averageWOS; }
            set { _averageWOS = value; }
        }

        /// <summary>
        /// Gets a flag to identify if the average weeks of supply for set is set
        /// </summary
        public bool AverageWOSIsSet
        {
            get { return _averageWOS != null; }
        }

        /// <summary>
		/// Gets or sets the sell thru percent for the set when calculated interactively
		/// </summary>
		public double? SellThruPercent
        {
            get { return _sellThruPercent; }
            set { _sellThruPercent = value; }
        }

        /// <summary>
        /// Gets a flag to identify if the sell thru percent for set is set
        /// </summary
        public bool SellThruPercentIsSet
        {
            get { return _sellThruPercent != null; }
        }

        public List<ROMethodAllocationVelocityMatrixVelocityGrade> MatrixGradeValues
        {
            get { return _matrixGradeValues; }
        }

        #endregion
        public ROMethodAllocationVelocityAttributeSet(KeyValuePair<int, string> attributeSet,
            eVelocityRuleType noOnHandRule,
            double? noOnHandRuleValue,
            eVelocityMatrixMode matrixMode,
            eVelocityRuleRequiresQuantity matrixModeAverageRule,
            double? matrixModeAverageRuleValue,
            eVelocitySpreadOption spreadOption,
            double? allStoresAverageWOS,
            double? allStoresSellThruPercent,
            double? averageWOS,
            double? sellThruPercent
            )
        {
            // fields specific to Allocation Velocity attribute set

            AttributeSet = attributeSet;
            NoOnHandRule = noOnHandRule;
            NoOnHandRuleValue = noOnHandRuleValue;
            _onHandRule = eVelocityRuleType.None;
            _onHandRuleValue = null;
            MatrixMode = matrixMode;
            MatrixModeAverageRule = matrixModeAverageRule;
            MatrixModeAverageRuleValue = matrixModeAverageRuleValue;
            SpreadOption = spreadOption;
            _allStoresAverageWOS = allStoresAverageWOS;
            _allStoresSellThruPercent = allStoresSellThruPercent;
            _averageWOS = averageWOS;
            _sellThruPercent = sellThruPercent;

            _matrixGradeValues = new List<ROMethodAllocationVelocityMatrixVelocityGrade>();

        }

    }

    [DataContract(Name = "ROMethodAllocationVelocityMatrixVelocityGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationVelocityMatrixVelocityGrade
    {
        // fields specific to Allocation Velocify attribute set

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _velocityGrade;
        [DataMember(IsRequired = true)]
        private int? _totalSales;
        [DataMember(IsRequired = true)]
        private double? _avgSales;
        [DataMember(IsRequired = true)]
        private double? _pctTotalSales;
        [DataMember(IsRequired = true)]
        private double? _avgSalesIdx;
        [DataMember(IsRequired = true)]
        private double? _totalNumStores;
        [DataMember(IsRequired = true)]
        private double? _avgStock;
        [DataMember(IsRequired = true)]
        private double? _stockPercentOfTotal;
        [DataMember(IsRequired = true)]
        private double? _allocationPercentOfTotal;


        [DataMember(IsRequired = true)]
        private List<ROMethodAllocationVelocityMatrixCell> _matrixGradeCells;

        #region Public Properties

        /// <summary>
        /// Gets or sets the velocity grade to use for matrix data
        /// </summary
        public KeyValuePair<int, string> VelocityGrade
        {
            get { return _velocityGrade; }
            set { _velocityGrade = value; }
        }

        /// <summary>
        /// Gets or sets the total sales for the velocity grade
        /// </summary
        public int? TotalSales
        {
            get { return _totalSales; }
            set { _totalSales = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the total sales for the velocity grade is set
        /// </summary
        public bool TotalSalesIsSet
        {
            get { return _totalSales != null; }
        }

        /// <summary>
        /// Gets or sets the average sales for the velocity grade
        /// </summary
        public double? AvgSales
        {
            get { return _avgSales; }
            set { _avgSales = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the average sales for the velocity grade is set
        /// </summary
        public bool AvgSalesIsSet
        {
            get { return _avgSales != null; }
        }

        /// <summary>
        /// Gets or sets the percent of total sales for the velocity grade
        /// </summary
        public double? PctTotalSales
        {
            get { return _pctTotalSales; }
            set { _pctTotalSales = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the percent of total sales for the velocity grade is set
        /// </summary
        public bool PctTotalSalesIsSet
        {
            get { return _pctTotalSales != null; }
        }

        /// <summary>
        /// Gets or sets the average sales index for the velocity grade
        /// </summary
        public double? AvgSalesIdx
        {
            get { return _avgSalesIdx; }
            set { _avgSalesIdx = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the average sales index for the velocity grade is set
        /// </summary
        public bool AvgSalesIdxIsSet
        {
            get { return _avgSalesIdx != null; }
        }

        /// <summary>
        /// Gets or sets the total number of stores for the velocity grade
        /// </summary
        public double? TotalNumStores
        {
            get { return _totalNumStores; }
            set { _totalNumStores = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the total number of stores for the velocity grade is set
        /// </summary
        public bool TotalNumStoresIsSet
        {
            get { return _totalNumStores != null; }
        }

        /// <summary>
        /// Gets or sets the average stock for the velocity grade
        /// </summary
        public double? AvgStock
        {
            get { return _avgStock; }
            set { _avgStock = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the average stock for the velocity grade is set
        /// </summary
        public bool AvgStockIsSet
        {
            get { return _avgStock != null; }
        }

        /// <summary>
        /// Gets or sets the stock percent of total for the velocity grade
        /// </summary
        public double? StockPercentOfTotal
        {
            get { return _stockPercentOfTotal; }
            set { _stockPercentOfTotal = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the stock percent of total for the velocity grade is set
        /// </summary
        public bool StockPercentOfTotalIsSet
        {
            get { return _stockPercentOfTotal != null; }
        }

        /// <summary>
        /// Gets or sets the allocation percent of total for the velocity grade
        /// </summary
        public double? AllocationPercentOfTotal
        {
            get { return _allocationPercentOfTotal; }
            set { _allocationPercentOfTotal = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the allocation percent of total for the velocity grade is set
        /// </summary
        public bool AllocationPercentOfTotalIsSet
        {
            get { return _allocationPercentOfTotal != null; }
        }

        public List<ROMethodAllocationVelocityMatrixCell> MatrixGradeCells
        {
            get { return _matrixGradeCells; }
        }

        #endregion
        public ROMethodAllocationVelocityMatrixVelocityGrade(KeyValuePair<int, string> velocityGrade,
            int? totalSales,
            double? avgSales,
            double? pctTotalSales,
            double? avgSalesIdx,
            double? totalNumStores,
            double? avgStock,
            double? stockPercentOfTotal,
            double? allocationPercentOfTotal
            )
        {
            // fields specific to Allocation Velocity matrix grade values

            _velocityGrade = velocityGrade;
            _totalSales = totalSales;
            _avgSales = avgSales;
            _pctTotalSales = pctTotalSales;
            _avgSalesIdx = avgSalesIdx;
            _totalNumStores = totalNumStores;
            _avgStock = avgStock;
            _stockPercentOfTotal = stockPercentOfTotal;
            _allocationPercentOfTotal = allocationPercentOfTotal;
            _matrixGradeCells = new List<ROMethodAllocationVelocityMatrixCell>();
        }

    }

    [DataContract(Name = "ROMethodAllocationVelocityMatrixCell", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationVelocityMatrixCell
    {
        // fields specific to Allocation Velocify attribute set

        [DataMember(IsRequired = true)]
        private int _boundary;
        [DataMember(IsRequired = true)]
        private int _sellThruIndex;
        [DataMember(IsRequired = true)]
        private eVelocityRuleType _ruleType;
        [DataMember(IsRequired = true)]
        private double? _ruleValue;
        [DataMember(IsRequired = true)]
        private int? _numberOfStores;
        [DataMember(IsRequired = true)]
        private double? _averageWOS;

        #region Public Properties

        /// <summary>
        /// Gets or sets the velocity grade to use for matrix data
        /// </summary
        public int Boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }

        /// <summary>
        /// Gets or sets the sell thru index for the cell of the for the velocity grade
        /// </summary
        public int SellThruIndex
        {
            get { return _sellThruIndex; }
            set { _sellThruIndex = value; }
        }

        /// <summary>
        /// Gets or sets the rule type for the cell of the for the velocity grade
        /// </summary
        public eVelocityRuleType RuleType
        {
            get { return _ruleType; }
            set
            {
                _ruleType = value;
            }
        }
        /// <summary>
        /// Gets a flag to identify if the rule type for the cell of the for the velocity grade is set
        /// </summary
        public bool RuleTypeIsSet
        {
            get { return _ruleType != eVelocityRuleType.None; }
        }

        /// <summary>
        /// Gets or sets the rule value for the cell of the for the velocity grade
        /// </summary
        public double? RuleValue
        {
            get { return _ruleValue; }
            set { _ruleValue = value; }
        }
        /// <summary>
        /// Gets a flag to identify if the rule value for the cell of the for the velocity grade is set
        /// </summary
        public bool RuleValueIsSet
        {
            get { return _ruleValue != null; }
        }

        /// <summary>
        /// Gets or sets the number of stores for the cell of the for the velocity grade
        /// </summary
        public int? NumberOfStores
        {
            get { return _numberOfStores; }
            set { _numberOfStores = value; }
        }

        /// <summary>
        /// Gets or sets the average weeks of supply for the cell of the for the velocity grade
        /// </summary
        public double? AverageWOS
        {
            get { return _averageWOS; }
            set { _averageWOS = value; }
        }

        #endregion
        public ROMethodAllocationVelocityMatrixCell(int boundary,
            int sellThruIndex,
            eVelocityRuleType ruleType,
            double? ruleValue,
            int? numberOfStores,
            double? averageWOS
            )
        {
            // fields specific to Allocation Velocity matrix grade values

            _boundary = boundary;
            _sellThruIndex = sellThruIndex;
            RuleType = ruleType;
            _ruleValue = ruleValue;
            _numberOfStores = numberOfStores;
            _averageWOS = averageWOS;
        }

    }

    [DataContract(Name = "ROAllocationReviewViewDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationReviewViewDetails : ROViewDetails
    {
        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _summaryColumns;

        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _totalColumns;

        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _detailColumns;

        [DataMember(IsRequired = true)]
        private List<double> _verticalSplitterPercentages;

        [DataMember(IsRequired = true)]
        private List<double> _horizontalSplitterPercentages;

        [DataMember(IsRequired = true)]
        private int _groupBy;

        [DataMember(IsRequired = true)]
        private int _secondaryGroupBy;

        [DataMember(IsRequired = true)]
        private bool _isSequential;

        public ROAllocationReviewViewDetails(KeyValuePair<int, string> view) :
            base(view)
        {
            _summaryColumns = new List<ROSelectedField>();
            _totalColumns = new List<ROSelectedField>();
            _detailColumns = new List<ROSelectedField>();
            _verticalSplitterPercentages = new List<double>();
            _horizontalSplitterPercentages = new List<double>();
        }

        public List<ROSelectedField> SummaryColumns { get { return _summaryColumns; } }
        public List<ROSelectedField> TotalColumns { get { return _totalColumns; } }
        public List<ROSelectedField> DetailColumns { get { return _detailColumns; } }

        public List<ROSelectedField> SummaryColumnsByPosition { get { return _summaryColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }
        public List<ROSelectedField> TotalColumnsByPosition { get { return _totalColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }
        public List<ROSelectedField> DetailColumnsByPosition { get { return _detailColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }

        public List<double> VerticalSplitterPercentages { get { return _verticalSplitterPercentages; } }

        public List<double> HorizontalSplitterPercentages { get { return _horizontalSplitterPercentages; } }

        public int GroupBy { get { return _groupBy; } set { _groupBy = value; } }
        public int SecondaryGroupBy { get { return _secondaryGroupBy; } set { _secondaryGroupBy = value; } }
        public bool IsSequential { get { return _isSequential; } set { _isSequential = value; } }

    }

    /// <summary>
    /// Allocation Worklist View Column
    /// </summary>
    [DataContract(Name = "ROAllocationWorklistEntry", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistEntry
    {

        #region MemberVariables
        [DataMember(IsRequired = true)]
        private int _viewKey;

        [DataMember(IsRequired = true)]
        private string _bandKey;

        [DataMember(IsRequired = true)]
        private string _columnKey;

        [DataMember(IsRequired = true)]
        private int _visiblePosition;

        [DataMember(IsRequired = true)]
        private bool _isHidden;

        [DataMember(IsRequired = true)]
        private bool _isGroupByColumn;

        [DataMember(IsRequired = true)]
        private int _sortDirection;

        [DataMember(IsRequired = true)]
        private int _sortSequence;

        [DataMember(IsRequired = true)]
        private int _width;

        [DataMember(IsRequired = true)]
        private string _columnType;

        [DataMember(IsRequired = true)]
        private string _headerCharacteristicGroupKey;

        [DataMember(IsRequired = true)]
        private string _label;

        [DataMember(IsRequired = true)]
        private string _itemField;

        [DataMember(IsRequired = true)]
        private eHeaderCharType _dataType;

        #endregion

        #region Constructor
        public ROAllocationWorklistEntry(int viewKey, string bandKey, string columnKey, int visiblePosition, bool isHidden, bool isGroupByColumn,
               int sortDirection, int sortSequence, int width, string columnType, string headerCharacteristicGroupKey, string label, string itemField = null,
               eHeaderCharType dataType = eHeaderCharType.unknown)
        {
            _viewKey = viewKey;
            _bandKey = bandKey;
            _columnKey = columnKey;
            _visiblePosition = visiblePosition;
            _isHidden = isHidden;
            _isGroupByColumn = isGroupByColumn;
            _sortDirection = sortDirection;
            _sortSequence = sortSequence;
            _width = width;
            _columnType = columnType;
            _headerCharacteristicGroupKey = headerCharacteristicGroupKey;
            _label = label;
            _itemField = itemField;
            _dataType = dataType;
        }
        #endregion  

        #region Public Properties
        public int ViewRID
        {
            get { return _viewKey; }
            set { _viewKey = value; }
        }

        public string BandKey
        {
            get { return _bandKey; }
            set { _bandKey = value; }
        }

        public string ColumnKey
        {
            get { return _columnKey; }
            set { _columnKey = value; }
        }

        public int VisiblePosition
        {
            get { return _visiblePosition; }
            set { _visiblePosition = value; }
        }

        public bool IsHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }

        public bool IsGroupByColumn
        {
            get { return _isGroupByColumn; }
            set { _isGroupByColumn = value; }
        }

        public int SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        public int SortSequence
        {
            get { return _sortSequence; }
            set { _sortSequence = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public bool IsCharacteristsicColumn
        {
            get { return _columnType == "C"; }
        }

        public string ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        public string HeaderCharacteristicGroupKey
        {
            get { return _headerCharacteristicGroupKey; }
            set { _headerCharacteristicGroupKey = value; }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public string ItemField
        {
            get { return _itemField; }
            set { _itemField = value; }
        }

        public eHeaderCharType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        #endregion  
    }

    [DataContract(Name = "ROAllocationWorklistViewDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorklistViewDetails : ROViewDetails
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _filter;

        [DataMember(IsRequired = true)]
        private List<ROAllocationWorklistEntry> _viewDetails;

        public ROAllocationWorklistViewDetails(KeyValuePair<int, string> view, KeyValuePair<int, string> filter = default(KeyValuePair<int, string>)) :
            base(view)
        {
            _viewDetails = new List<ROAllocationWorklistEntry>();
            _filter = filter;
        }

        /// <summary>
        /// Gets the flag identifying if the filter has been set.
        /// </summary>
        public bool FilterIsSet
        {
            get { return !_filter.Equals(default(KeyValuePair<int, string>)); }
        }

        /// <summary>
        /// Gets or set the Key Value Pair for the filter.
        /// </summary>
		public KeyValuePair<int, string> Filter { get { return _filter; } set { _filter = value; } }

        /// <summary>
        /// Gets the list of view details.
        /// </summary>
        public List<ROAllocationWorklistEntry> ViewDetails { get { return _viewDetails; } }

    }
}