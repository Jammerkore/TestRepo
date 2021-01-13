using MIDRetail.DataCommon;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Logility.ROWebSharedTypes
{

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
                if (!Enum.IsDefined(typeof(eAllocationSelectionViewType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _viewType = value;
                }
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
            if (!Enum.IsDefined(typeof(eAllocationSelectionViewType), viewType))
            {
                throw new Exception("Value " + viewType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _viewType = viewType;
            }
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
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _merchandiseType = value;
                }
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
        public ROMethodGeneralAllocationProperties(KeyValuePair<int, string> method, string description, int userKey, KeyValuePair<int, string> begin_CDR, KeyValuePair<int, string> shipTo_CDR, bool percentInd,
            double reserve, KeyValuePair<int, string> merch_HN, int merch_PH_RID, int merch_PHL_SEQ, eMerchandiseType merchandiseType, KeyValuePair<int, string> genAlloc_HDR,
            double reserveAsBulk, double reserveAsPacks) :
            base(eMethodType.GeneralAllocation, method, description, userKey)

        {
            // fields specific to General Allocation method
            _begin_CDR = begin_CDR;
            _shipTo_CDR = shipTo_CDR;
            _percentInd = percentInd;
            _reserve = reserve;
            _merch_HN = merch_HN;
            _merch_PH = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            if (!Enum.IsDefined(typeof(eMerchandiseType), merchandiseType))
            {
                throw new Exception("Value " + merchandiseType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _merchandiseType = merchandiseType;
            }
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
        ROMethodSizeRuleAttributeSet _sizeRuleAttributeSet;


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
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _merchandiseType = value;
                }
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
                if (!Enum.IsDefined(typeof(eFillSizesToType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _fillSizesToType = value;
                }
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
                if (!Enum.IsDefined(typeof(eVSWSizeConstraints), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _vSWSizeConstraints = value;
                }
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
        public ROMethodSizeRuleAttributeSet SizeRuleAttributeSet
        {
            get { return _sizeRuleAttributeSet; }
            set { _sizeRuleAttributeSet = value; }
        }

        #endregion
        public ROMethodFillSizeHolesProperties(KeyValuePair<int, string> method, string description, int userKey, KeyValuePair<int, string> filter
            , double available, bool percentInd, KeyValuePair<int, string> merch_HN, int merch_PH_RID, int merch_PHL_SEQ, eMerchandiseType merchandiseType
            , bool normalizeSizeCurvesDefaultIsOverridden, bool normalizeSizeCurves, eFillSizesToType fillSizesToType, KeyValuePair<int, string> sizeGroup
            , KeyValuePair<int, string> sizeAlternateModel, ROSizeCurveProperties rOSizeCurveProperties, ROSizeConstraintProperties rOSizeConstraintProperties
            , bool overrideVSWSizeConstraints, eVSWSizeConstraints vSWSizeConstraints, bool overrideAvgPackDevTolerance, double avgPackDeviationTolerance
            , bool overrideMaxPackNeedTolerance, bool packToleranceStepped, bool packToleranceNoMaxStep, double maxPackNeedTolerance
            , KeyValuePair<int, string> attribute, ROMethodSizeRuleAttributeSet sizeRuleAttributeSet
            ) :
            base(eMethodType.FillSizeHolesAllocation, method, description, userKey)

        {
            // fields specific to Fill Size Holes method
            _filter = filter;
            _available = available;
            _percentInd = percentInd;
            _merch_HN = merch_HN;
            _merch_PH = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            if (!Enum.IsDefined(typeof(eMerchandiseType), merchandiseType))
            {
                throw new Exception("Value " + merchandiseType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _merchandiseType = merchandiseType;
            }
            _normalizeSizeCurvesDefaultIsOverridden = normalizeSizeCurvesDefaultIsOverridden;
            _normalizeSizeCurves = normalizeSizeCurves;
            if (!Enum.IsDefined(typeof(eFillSizesToType), fillSizesToType))
            {
                throw new Exception("Value " + fillSizesToType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _fillSizesToType = fillSizesToType;
            }
            _sizeGroup = sizeGroup;
            _sizeAlternateModel = sizeAlternateModel;
            _rOSizeCurveProperties = rOSizeCurveProperties;
            _rOSizeConstraintProperties = rOSizeConstraintProperties;
            _overrideVSWSizeConstraints = overrideVSWSizeConstraints;
            if (!Enum.IsDefined(typeof(eVSWSizeConstraints), vSWSizeConstraints))
            {
                throw new Exception("Value " + vSWSizeConstraints.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _vSWSizeConstraints = vSWSizeConstraints;
            }
            _overrideAvgPackDevTolerance = overrideAvgPackDevTolerance;
            _avgPackDeviationTolerance = avgPackDeviationTolerance;
            _overrideMaxPackNeedTolerance = overrideMaxPackNeedTolerance;
            _packToleranceStepped = packToleranceStepped;
            _packToleranceNoMaxStep = packToleranceNoMaxStep;
            _maxPackNeedTolerance = maxPackNeedTolerance;
            _attribute = attribute;
            _sizeRuleAttributeSet = sizeRuleAttributeSet;
        }
    }
    [DataContract(Name = "ROMethodSizeNeedProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeNeedProperties : ROMethodProperties
    {
        // fields specific to Size Need Method
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
        ROMethodSizeRuleAttributeSet _sizeRuleAttributeSet;


        #region Public Properties
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
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _merchandiseType = value;
                }
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
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set
            {
                if (!Enum.IsDefined(typeof(eVSWSizeConstraints), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _vSWSizeConstraints = value;
                }
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
        public ROMethodSizeRuleAttributeSet SizeRuleAttributeSet
        {
            get { return _sizeRuleAttributeSet; }
            set { _sizeRuleAttributeSet = value; }
        }

        #endregion
        public ROMethodSizeNeedProperties(KeyValuePair<int, string> method, string description, int userKey
            , KeyValuePair<int, string> merch_HN, int merch_PH_RID, int merch_PHL_SEQ, eMerchandiseType merchandiseType
            , bool normalizeSizeCurvesDefaultIsOverridden, bool normalizeSizeCurves, KeyValuePair<int, string> sizeGroup
            , KeyValuePair<int, string> sizeAlternateModel, ROSizeCurveProperties rOSizeCurveProperties, ROSizeConstraintProperties rOSizeConstraintProperties
            , bool overrideVSWSizeConstraints, eVSWSizeConstraints vSWSizeConstraints, bool overrideAvgPackDevTolerance, double avgPackDeviationTolerance
            , bool overrideMaxPackNeedTolerance, bool packToleranceStepped, bool packToleranceNoMaxStep, double maxPackNeedTolerance
            , KeyValuePair<int, string> attribute, ROMethodSizeRuleAttributeSet sizeRuleAttributeSet
            ) :
            base(eMethodType.SizeNeedAllocation, method, description, userKey)

        {
            // fields specific to Size Need method
            _merch_HN = merch_HN;
            _merch_PH = new KeyValuePair<int, int>(merch_PH_RID, merch_PHL_SEQ);
            if (!Enum.IsDefined(typeof(eMerchandiseType), merchandiseType))
            {
                throw new Exception("Value " + merchandiseType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _merchandiseType = merchandiseType;
            }
            _normalizeSizeCurvesDefaultIsOverridden = normalizeSizeCurvesDefaultIsOverridden;
            _normalizeSizeCurves = normalizeSizeCurves;
            _sizeGroup = sizeGroup;
            _sizeAlternateModel = sizeAlternateModel;
            _rOSizeCurveProperties = rOSizeCurveProperties;
            _rOSizeConstraintProperties = rOSizeConstraintProperties;
            _overrideVSWSizeConstraints = overrideVSWSizeConstraints;
            if (!Enum.IsDefined(typeof(eVSWSizeConstraints), vSWSizeConstraints))
            {
                throw new Exception("Value " + vSWSizeConstraints.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _vSWSizeConstraints = vSWSizeConstraints;
            }
            _overrideAvgPackDevTolerance = overrideAvgPackDevTolerance;
            _avgPackDeviationTolerance = avgPackDeviationTolerance;
            _overrideMaxPackNeedTolerance = overrideMaxPackNeedTolerance;
            _packToleranceStepped = packToleranceStepped;
            _packToleranceNoMaxStep = packToleranceNoMaxStep;
            _maxPackNeedTolerance = maxPackNeedTolerance;
            _attribute = attribute;
            _sizeRuleAttributeSet = sizeRuleAttributeSet;
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
        ROMethodSizeRuleAttributeSet _sizeRuleAttributeSet;
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
        public ROMethodSizeRuleAttributeSet SizeRuleAttributeSet
        {
            get { return _sizeRuleAttributeSet; }
            set { _sizeRuleAttributeSet = value; }
        }

        public ROMethodBasisSizeSubstituteSet BasisSizeSubstituteSet
        {
            get { return _basisSizeSubstituteSet; }
            set { _basisSizeSubstituteSet = value; }
        }
        #endregion
        public ROMethodBasisSizeProperties(KeyValuePair<int, string> method, string description, int userKey, KeyValuePair<int, string> filter
            , KeyValuePair<int, string> sizeGroup, ROSizeCurveProperties rOSizeCurveProperties, ROSizeConstraintProperties rOSizeConstraintProperties
            , KeyValuePair<int, string> header, bool includeReserve, KeyValuePair<int, string> colorComponent, KeyValuePair<int, string> color
            , KeyValuePair<int, string> rule, int ruleQuantity
            , KeyValuePair<int, string> attribute, ROMethodSizeRuleAttributeSet sizeRuleAttributeSet, ROMethodBasisSizeSubstituteSet basisSizeSubstituteSet
            ) :
            base(eMethodType.BasisSizeAllocation, method, description, userKey)

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
            _sizeRuleAttributeSet = sizeRuleAttributeSet;
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
                if (!Enum.IsDefined(typeof(eEquateOverrideSizeType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _overrideSizeType = value;
                }
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
            if (!Enum.IsDefined(typeof(eEquateOverrideSizeType), overrideSizeType))
            {
                throw new Exception("Value " + overrideSizeType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _overrideSizeType = overrideSizeType;
            }
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
                if (!Enum.IsDefined(typeof(eSizeCurvesByType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _sizeCurvesByType = value;
                }
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
                if (!Enum.IsDefined(typeof(eNodeChainSalesType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _tolerIndexUnitsType = value;
                }
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
        public ROMethodSizeCurveProperties(KeyValuePair<int, string> method, string description, int userKey
            , KeyValuePair<int, string> sizeGroup, KeyValuePair<int, string> attribute, eSizeCurvesByType sizeCurvesByType
            , bool merchBasisEqualizeWeight, ROMethodSizeCurveMerchBasisSet sizeCurveMerchBasisSet, double tolerMinAvgPerSize , double tolerSalesTolerance
            , eNodeChainSalesType tolerIndexUnitsType , double tolerMinTolerancePct, double tolerMaxTolerancePct, bool applyMinToZeroTolerance
            ) :
            base(eMethodType.SizeCurve, method, description, userKey)

        {
            // fields specific to Size Curve method
            _sizeGroup = sizeGroup;
            _attribute = attribute;
            if (!Enum.IsDefined(typeof(eSizeCurvesByType), sizeCurvesByType))
            {
                throw new Exception("Value " + sizeCurvesByType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _sizeCurvesByType = sizeCurvesByType;
            }
            _merchBasisEqualizeWeight = merchBasisEqualizeWeight;
            _sizeCurveMerchBasisSet = sizeCurveMerchBasisSet;
            _tolerMinAvgPerSize = tolerMinAvgPerSize;
            _tolerSalesTolerance = tolerSalesTolerance;
            if (!Enum.IsDefined(typeof(eNodeChainSalesType), tolerIndexUnitsType))
            {
                throw new Exception("Value " + tolerIndexUnitsType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _tolerIndexUnitsType = tolerIndexUnitsType;
            }
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
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _merchType = value;
                }
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
            if (!Enum.IsDefined(typeof(eMerchandiseType), merchType))
            {
                throw new Exception("Value " + merchType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _merchType = merchType;
            }
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
                if (!Enum.IsDefined(typeof(eDCFulfillmentSplitOption), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _dCFulfillmentSplitOption = value;
                }
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
                if (!Enum.IsDefined(typeof(eDCFulfillmentHeadersOrder), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _headersOrder = value;
                }
            }
        }
        public eDCFulfillmentSplitByOption Split_By_Option
        {
            get { return _split_By_Option; }
            set
            {
                if (!Enum.IsDefined(typeof(eDCFulfillmentSplitByOption), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _split_By_Option = value;
                }
            }
        }
        public eDCFulfillmentWithinDC Within_Dc
        {
            get { return _within_Dc; }
            set
            {
                if (!Enum.IsDefined(typeof(eDCFulfillmentWithinDC), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _within_Dc = value;
                }
            }
        }
        public eDCFulfillmentReserve Split_By_Reserve
        {
            get { return _split_By_Reserve; }
            set
            {
                if (!Enum.IsDefined(typeof(eDCFulfillmentReserve), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _split_By_Reserve = value;
                }
            }
        }
        public eDCFulfillmentStoresOrder StoresOrder
        {
            get { return _storesOrder; }
            set
            {
                if (!Enum.IsDefined(typeof(eDCFulfillmentStoresOrder), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _storesOrder = value;
                }
            }
        }
        public ROMethodDCStoreCharacteristicSet DCStoreCharacteristicSet
        {
            get { return _dCStoreCharacteristicSet; }
            set { _dCStoreCharacteristicSet = value; }
        }
        #endregion
        public ROMethodDCFulfillmentProperties(KeyValuePair<int, string> method, string description, int userKey, eDCFulfillmentSplitOption dCFulfillmentSplitOption, bool applyMinimumsInd
            , KeyValuePair<int, string> prioritizeType, eDCFulfillmentHeadersOrder headersOrder, eDCFulfillmentSplitByOption split_By_Option, eDCFulfillmentWithinDC within_Dc
            , eDCFulfillmentReserve split_By_Reserve, eDCFulfillmentStoresOrder storesOrder, ROMethodDCStoreCharacteristicSet dCStoreCharacteristicSet
            ) :
            base(eMethodType.DCFulfillment, method, description, userKey)

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
        public ROMethodBuildPacksProperties(KeyValuePair<int, string> method, string description, int userKey, KeyValuePair<int, string> vendor
            , int packMinOrder, int sizeMultiple, KeyValuePair<int, string> sizeGroup, KeyValuePair<int, string> sizeCurve, List<PackPatternCombo> packCombination
            , double reserveTotalQty, bool reserveTotalIsPercent, double reserveBulkQty, bool reserveBulkIsPercent, double reservePacksQty,bool reservePacksIsPercent
            , bool removeBulkInd, double avgPackErrorDevTolerance, double maxPackErrorDevTolerance, bool depleteReserveSelected, bool increaseBuySelected, double increaseBuyPct
            ) :
            base(eMethodType.BuildPacks, method, description, userKey)

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
        // input parameters
        [DataMember(IsRequired = true)]
        private int _sizeCurveGroupRID;

        [DataMember(IsRequired = true)]
        private int _genCurveNsccdRID;

        [DataMember(IsRequired = true)]
        private int _genCurveHcgRID;

        [DataMember(IsRequired = true)]
        private int _genCurveHnRID;

        [DataMember(IsRequired = true)]
        private int _genCurvePhRID;

        [DataMember(IsRequired = true)]
        private int _genCurvePhlSequence;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _genCurveMerchType;

        //output parameters
        [DataMember(IsRequired = true)]
        private bool _isUseDefault;

        [DataMember(IsRequired = true)]
        private bool _isApplyRulesOnly;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeCurve;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeCurveGenericHierarchy;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeCurveGenericNameExtension;



        public ROSizeCurveProperties(int sizeCurveGroupRID, int genCurveNsccdRID, int genCurveHcgRID, int genCurveHnRID, int genCurvePhRID, int genCurvePhlSequence, eMerchandiseType genCurveMerchType,
            bool isUseDefault, bool isApplyRulesOnly, KeyValuePair<int, string> sizeCurve, KeyValuePair<int, string> sizeCurveGenericHierarchy, KeyValuePair<int, string> sizeCurveGenericNameExtension)
        {
            _sizeCurveGroupRID = sizeCurveGroupRID;
            _genCurveNsccdRID = genCurveNsccdRID;
            _genCurveHcgRID = genCurveHcgRID;
            _genCurveHnRID = genCurveHnRID;
            _genCurvePhRID = genCurvePhRID;
            _genCurvePhlSequence = genCurvePhlSequence;
            if (!Enum.IsDefined(typeof(eMerchandiseType), genCurveMerchType))
            {
                throw new Exception("Value " + genCurveMerchType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _genCurveMerchType = genCurveMerchType;
            }
            _isUseDefault = isUseDefault;
            _isApplyRulesOnly = isApplyRulesOnly;
            _sizeCurve = sizeCurve;
            _sizeCurveGenericHierarchy = sizeCurveGenericHierarchy;
            _sizeCurveGenericNameExtension = sizeCurveGenericNameExtension;
        }

        public int SizeCurveGroupRID { get { return _sizeCurveGroupRID; } set { _sizeCurveGroupRID = value; } }
        public int GenCurveNsccdRID { get { return _genCurveNsccdRID; } set { _genCurveNsccdRID = value; } }
        public int GenCurveHcgRID { get { return _genCurveHcgRID; } set { _genCurveHcgRID = value; } }
        public int GenCurveHnRID { get { return _genCurveHnRID; } set { _genCurveHnRID = value; } }
        public int GenCurvePhRID { get { return _genCurvePhRID; } set { _genCurvePhRID = value; } }
        public int GenCurvePhlSequence { get { return _genCurvePhlSequence; } set { _genCurvePhlSequence = value; } }
        public eMerchandiseType GenCurveMerchType
        {
            get { return _genCurveMerchType; }
            set
            {
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _genCurveMerchType = value;
                }
            }
        }
        public bool IsUseDefault { get { return _isUseDefault; } set { _isUseDefault = value; } }
        public bool IsApplyRulesOnly { get { return _isApplyRulesOnly; } set { _isApplyRulesOnly = value; } }
        public KeyValuePair<int, string> SizeCurve { get { return _sizeCurve; } set { _sizeCurve = value; } }
        public KeyValuePair<int, string> SizeCurveGenericHierarchy { get { return _sizeCurveGenericHierarchy; } set { _sizeCurveGenericHierarchy = value; } }
        public KeyValuePair<int, string> SizeCurveGenericNameExtension { get { return _sizeCurveGenericNameExtension; } set { _sizeCurveGenericNameExtension = value; } }

    }

    [DataContract(Name = "ROSizeConstraintProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintProperties
    {
        // input parameters

        [DataMember(IsRequired = true)]
        private int _inventoryBasisMerchHnRID;

        [DataMember(IsRequired = true)]
        private int _inventoryBasisMerchPhRID;

        [DataMember(IsRequired = true)]
        private int _inventoryBasisMerchPhlSequence;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _inventoryBasisMerchType;

        [DataMember(IsRequired = true)]
        private int _sizeConstraintRID;

        [DataMember(IsRequired = true)]
        private int _genConstraintHcgRID;

        [DataMember(IsRequired = true)]
        private int _genConstraintHnRID;

        [DataMember(IsRequired = true)]
        private int _genConstraintPhRID;

        [DataMember(IsRequired = true)]
        private int _genConstraintPhlSequence;

        [DataMember(IsRequired = true)]
        private eMerchandiseType _genConstraintMerchType;

        [DataMember(IsRequired = true)]
        private bool _genConstraintColorInd;

        //output parameters

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _inventoryBasis;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeConstraint;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeConstraintGenericHierarchy;

        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _sizeConstraintGenericHeaderChar;



        public ROSizeConstraintProperties(int inventoryBasisMerchHnRID, int inventoryBasisMerchPhRID, int inventoryBasisMerchPhlSequence, eMerchandiseType inventoryBasisMerchType,
            int sizeConstraintRID, int genConstraintHcgRID, int genConstraintHnRID, int genConstraintPhRID, int genConstraintPhlSequence, eMerchandiseType genConstraintMerchType,
            bool genConstraintColorInd, KeyValuePair<int, string> inventoryBasis, KeyValuePair<int, string> sizeConstraint, KeyValuePair<int, string> sizeConstraintGenericHierarchy, KeyValuePair<int, string> sizeConstraintGenericHeaderChar)
        {
            _inventoryBasisMerchHnRID = inventoryBasisMerchHnRID;
            _inventoryBasisMerchPhRID = inventoryBasisMerchPhRID;
            _inventoryBasisMerchPhlSequence = inventoryBasisMerchPhlSequence;
            if (!Enum.IsDefined(typeof(eMerchandiseType), inventoryBasisMerchType))
            {
                throw new Exception("Value " + inventoryBasisMerchType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _inventoryBasisMerchType = inventoryBasisMerchType;
            }
            _sizeConstraintRID = sizeConstraintRID;
            _genConstraintHcgRID = genConstraintHcgRID;
            _genConstraintHnRID = genConstraintHnRID;
            _genConstraintPhRID = genConstraintPhRID;
            _genConstraintPhlSequence = genConstraintPhlSequence;
            if (!Enum.IsDefined(typeof(eMerchandiseType), genConstraintMerchType))
            {
                throw new Exception("Value " + genConstraintMerchType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _genConstraintMerchType = genConstraintMerchType;
            }
            _genConstraintColorInd = genConstraintColorInd;
            _inventoryBasis = inventoryBasis;
            _sizeConstraint = sizeConstraint;
            _sizeConstraintGenericHierarchy = sizeConstraintGenericHierarchy;
            _sizeConstraintGenericHeaderChar = sizeConstraintGenericHeaderChar;
        }
        public int InventoryBasisMerchHnRID { get { return _inventoryBasisMerchHnRID; } set { _inventoryBasisMerchHnRID = value; } }
        public int InventoryBasisMerchPhRID { get { return _inventoryBasisMerchPhRID; } set { _inventoryBasisMerchPhRID = value; } }
        public int InventoryBasisMerchPhlSequence { get { return _inventoryBasisMerchPhlSequence; } set { _inventoryBasisMerchPhlSequence = value; } }
        public eMerchandiseType InventoryBasisMerchType
            {
            get { return _inventoryBasisMerchType; }
            set
            {
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _inventoryBasisMerchType = value;
                }
            }
        }
        public int SizeConstraintRID { get { return _sizeConstraintRID; } set { _sizeConstraintRID = value; } }
        public int GenConstraintHcgRID { get { return _genConstraintHcgRID; } set { _genConstraintHcgRID = value; } }
        public int GenConstraintHnRID { get { return _genConstraintHnRID; } set { _genConstraintHnRID = value; } }
        public int GenConstraintPhRID { get { return _genConstraintPhRID; } set { _genConstraintPhRID = value; } }
        public int GenConstraintPhlSequence { get { return _genConstraintPhlSequence; } set { _genConstraintPhlSequence = value; } }
        public eMerchandiseType GenConstraintMerchType
        {
            get { return _genConstraintMerchType; }
            set
            {
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _genConstraintMerchType = value;
                }
            }
        }
        public bool GenConstraintColorInd { get { return _genConstraintColorInd; } set { _genConstraintColorInd = value; } }
        public KeyValuePair<int, string> InventoryBasis { get { return _inventoryBasis; } set { _inventoryBasis = value; } }
        public KeyValuePair<int, string> SizeConstraint { get { return _sizeConstraint; } set { _sizeConstraint = value; } }
        public KeyValuePair<int, string> SizeConstraintGenericHierarchy { get { return _sizeConstraintGenericHierarchy; } set { _sizeConstraintGenericHierarchy = value; } }
        public KeyValuePair<int, string> SizeConstraintGenericHeaderChar { get { return _sizeConstraintGenericHeaderChar; } set { _sizeConstraintGenericHeaderChar = value; } }

    }
    [DataContract(Name = "ROMethodSizeRuleAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeRuleAttributeSet
    {
        [DataMember(IsRequired = true)]
        public List<ROMethodSizeRuleProperties> _sizeRuleRowsValues;


        public List<ROMethodSizeRuleProperties> SizeRuleRowsValues
        {
            get
            {
                if (_sizeRuleRowsValues == null)
                {
                    _sizeRuleRowsValues = new List<ROMethodSizeRuleProperties>();
                }
                return _sizeRuleRowsValues;
            }
            set { _sizeRuleRowsValues = value; }
        }

    }

    [DataContract(Name = "ROMethodSizeRuleProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodSizeRuleProperties
    {
        [DataMember(IsRequired = true)]
        bool _updated;
        [DataMember(IsRequired = true)]
        bool _inserted;
        [DataMember(IsRequired = true)]
        bool _deleted;
        [DataMember(IsRequired = true)]
        string _bandDsc;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sgl;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _colorCode;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizes;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dimensions;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeCode;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeRule;
        [DataMember(IsRequired = true)]
        double _sizeQuantity;
        [DataMember(IsRequired = true)]
        eSizeMethodRowType _rowTypeID;
        [DataMember(IsRequired = true)]
        int _sizeSeq;

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
        public string BandDsc
        {
            get { return _bandDsc; }
            set { _bandDsc = value; }
        }
        public KeyValuePair<int, string> Sgl
        {
            get { return _sgl; }
            set { _sgl = value; }
        }
        public KeyValuePair<int, string> ColorCode
        {
            get { return _colorCode; }
            set { _colorCode = value; }
        }
        public KeyValuePair<int, string> Sizes
        {
            get { return _sizes; }
            set { _sizes = value; }
        }
        public KeyValuePair<int, string> Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }
        public KeyValuePair<int, string> SizeCode
        {
            get { return _sizeCode; }
            set { _sizeCode = value; }
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
        public eSizeMethodRowType RowTypeID
        {
            get { return _rowTypeID; }
            set
            {
                if (!Enum.IsDefined(typeof(eSizeMethodRowType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _rowTypeID = value;
                }
            }
        }
        public int SizeSeq
        {
            get { return _sizeSeq; }
            set { _sizeSeq = value; }
        }

        public ROMethodSizeRuleProperties(bool updated, bool inserted, bool deleted, string bandDsc, KeyValuePair<int, string> sgl, KeyValuePair<int, string> colorCode,
            KeyValuePair<int, string> sizes, KeyValuePair<int, string> dimensions, KeyValuePair<int, string> sizeCode, KeyValuePair<int, string> sizeRule,
            double sizeQuantity, eSizeMethodRowType rowTypeID, int sizeSeq)
        {
            // fields specific to Size Rule Method Constraints
            _updated = updated;
            _inserted = inserted;
            _deleted = deleted;
            _bandDsc = bandDsc;
            _sgl = sgl;
            _colorCode = colorCode;
            _sizes = sizes;
            _dimensions = dimensions;
            _sizeCode = sizeCode;
            _sizeRule = sizeRule;
            _sizeQuantity = sizeQuantity;
            if (!Enum.IsDefined(typeof(eSizeMethodRowType), rowTypeID))
            {
                throw new Exception("Value " + rowTypeID.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _rowTypeID = rowTypeID;
            }
            _sizeSeq = sizeSeq;
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
                if (!Enum.IsDefined(typeof(eSortDirection), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _sortDirection = value;
                }
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
                if (!Enum.IsDefined(typeof(eComponentType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _componentType = value;
                }
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
                if (!Enum.IsDefined(typeof(eRuleMethod), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _includeRuleMethod = value;
                }
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
                if (!Enum.IsDefined(typeof(eRuleMethod), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _excludeRuleMethod = value;
                }
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
            KeyValuePair<int, string> storeGroupLevel = default(KeyValuePair<int, string>)
            ) :
            base(eMethodType.Rule, method, description, userKey)

        {
            // fields specific to Rule method
            _filter = filter;
            _header = header;
            _isHeaderMaster = isHeaderMaster;
            if (!Enum.IsDefined(typeof(eSortDirection), sortDirection))
            {
                throw new Exception("Value " + sortDirection.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _sortDirection = sortDirection;
            }
            _includeReserveInd = includeReserveInd;
            if (!Enum.IsDefined(typeof(eComponentType), componentType))
            {
                throw new Exception("Value " + componentType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _componentType = componentType;
            }
            _pack = pack;
            _color = color;
            if (!Enum.IsDefined(typeof(eRuleMethod), includeRuleMethod))
            {
                throw new Exception("Value " + includeRuleMethod.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _includeRuleMethod = includeRuleMethod;
            }
            _includeQuantity = includeQuantity;
            if (!Enum.IsDefined(typeof(eRuleMethod), excludeRuleMethod))
            {
                throw new Exception("Value " + excludeRuleMethod.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _excludeRuleMethod = excludeRuleMethod;
            }
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
        public int AdMinimum { get; set; }
        [DataMember(IsRequired = true)]
        public int ColorMinimum { get; set; }
        [DataMember(IsRequired = true)]
        public int ColorMaximum { get; set; }
        [DataMember(IsRequired = true)]
        public int ShipUpTo { get; set; }

        public bool AdMinimumIsSet { get { return AdMinimum != int.MinValue; } }
        public bool ColorMinimumIsSet { get { return ColorMinimum != int.MinValue; } }
        public bool ColorMaximumIsSet { get { return ColorMaximum != int.MaxValue; } }
        public bool ShipUpToIsSet { get { return ShipUpTo != int.MinValue; } }
    }

    [DataContract(Name = "ROAllocationVelocityGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationVelocityGrade : ROStoreGrade
    {
        [DataMember(IsRequired = true)]
        public int AdMinimum { get; set; }

        public bool AdMinimumIsSet { get { return AdMinimum != int.MinValue; } }
    }

    [DataContract(Name = "ROMethodAllocationOverrideProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodAllocationOverrideProperties : ROMethodProperties
    {
        // fields specific to Allocation Override method
        [DataMember(IsRequired = true)]
        int _storeGradeWeekCount;
        [DataMember(IsRequired = true)]
        double _percentNeedLimit;
        [DataMember(IsRequired = true)]
        bool _exceedMaxInd;
        [DataMember(IsRequired = true)]
        double _reserve;
        [DataMember(IsRequired = true)]
        bool _percentInd;
        [DataMember(IsRequired = true)]
        double _reserveAsBulk;
        [DataMember(IsRequired = true)]
        double _reserveAsPacks;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _merchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _merchandiseHierarchy;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _onHandMerchandise;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, int> _onHandMerchandiseHierarchy;
        [DataMember(IsRequired = true)]
        private List<KeyValuePair<int, string>> _hierarchyLevels;
        [DataMember(IsRequired = true)]
        double _onHandFactor;
        [DataMember(IsRequired = true)]
        int _colorMult;
        [DataMember(IsRequired = true)]
        int _sizeMult;
        [DataMember(IsRequired = true)]
        int _allColorMin;
        [DataMember(IsRequired = true)]
        int _allColorMax;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _capacityAttribute;
        [DataMember(IsRequired = true)]
        bool _exceedCapacity;
        [DataMember(IsRequired = true)]
        bool _merchPlanUnspecified;
        [DataMember(IsRequired = true)]
        bool _merchOnHandUnspecified;
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _storeGradesAttribute;
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
        bool _doNotApplyVSW;
        [DataMember(IsRequired = true)]
        private List<ROAttributeSetStoreGrade> _storeGradeValues;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideCapacityProperties> _capacity;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideColorProperties> _colorMinMax;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverridePackRoundingProperties> _packRounding;
        [DataMember(IsRequired = true)]
        private List<ROMethodOverrideVSWAttributeSet> _vswAttributeSet;

        #region Public Properties
        public int StoreGradeWeekCount
        {
            get { return _storeGradeWeekCount; }
            set { _storeGradeWeekCount = value; }
        }

        public double PercentNeedLimit
        {
            get { return _percentNeedLimit; }
            set { _percentNeedLimit = value; }
        }

        public bool ExceedMaxInd
        {
            get { return _exceedMaxInd; }
            set { _exceedMaxInd = value; }
        }

        public double Reserve
        {
            get { return _reserve; }
            set { _reserve = value; }
        }

        public bool PercentInd
        {
            get { return _percentInd; }
            set { _percentInd = value; }
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

        public KeyValuePair<int, string> OnHandMerchandise
        {
            get { return _onHandMerchandise; }
            set { _onHandMerchandise = value; }
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

        public double OnHandFactor
        {
            get { return _onHandFactor; }
            set { _onHandFactor = value; }
        }

        public int ColorMult
        {
            get { return _colorMult; }
            set { _colorMult = value; }
        }

        public int SizeMult
        {
            get { return _sizeMult; }
            set { _sizeMult = value; }
        }

        public int AllColorMin
        {
            get { return _allColorMin; }
            set { _allColorMin = value; }
        }

        public int AllColorMax
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

        public bool MerchPlanUnspecified
        {
            get { return _merchPlanUnspecified; }
            set { _merchPlanUnspecified = value; }
        }

        public bool MerchOnHandUnspecified
        {
            get { return _merchOnHandUnspecified; }
            set { _merchOnHandUnspecified = value; }
        }

        public KeyValuePair<int, string> StoreGradesAttribute
        {
            get { return _storeGradesAttribute; }
            set { _storeGradesAttribute = value; }
        }

        public eMinMaxType InventoryIndicator
        {
            get { return _inventoryIndicator; }
            set
            {
                if (!Enum.IsDefined(typeof(eMinMaxType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _inventoryIndicator = value;
                }
            }
        }

        public eMerchandiseType InventoryBasisMerchType
        {
            get { return _inventoryBasisMerchType; }
            set
            {
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _inventoryBasisMerchType = value;
                }
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

        public KeyValuePair<int, string> VSWAttribute
        {
            get { return _vswAttribute; }
            set { _vswAttribute = value; }
        }

        public bool DoNotApplyVSW
        {
            get { return _doNotApplyVSW; }
            set { _doNotApplyVSW = value; }
        }

        public List<ROAttributeSetStoreGrade> StoreGradeValues
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

        public List<ROMethodOverrideVSWAttributeSet> VSWAttributeSet
        {
            get { return _vswAttributeSet; }
            set { _vswAttributeSet = value; }
        }

        #endregion
        public ROMethodAllocationOverrideProperties(KeyValuePair<int, string> method, string description, int userKey, int storeGradeWeekCount, double percentNeedLimit,
            bool exceedMaxInd, double reserve, bool percentInd, double reserveAsBulk, double reserveAsPacks, KeyValuePair<int, string> merchandise, KeyValuePair<int, int> merchandiseHierarchy,
            KeyValuePair<int, string> onHandMerchandise, KeyValuePair<int, int> onHandMerchandiseHierarchy, double onHandFactor, int colorMult, int sizeMult, int allColorMin, int allColorMax,
            KeyValuePair<int, string> capacityAttribute, bool exceedCapacity, bool merchPlanUnspecified, bool merchOnHandUnspecified, KeyValuePair<int, string> storeGradesAttribute,
            eMinMaxType inventoryIndicator, eMerchandiseType inventoryBasisMerchType, KeyValuePair<int, string> inventoryBasisMerchandise, KeyValuePair<int, int> inventoryBasisMerchandiseHierarchy,
            KeyValuePair<int, string> vswAttribute, bool doNotApplyVSW, List<ROAttributeSetStoreGrade> storeGradeValues, List<ROMethodOverrideCapacityProperties> capacity,
            List<ROMethodOverrideColorProperties> colorMinMax, List<ROMethodOverridePackRoundingProperties> packRounding, List<ROMethodOverrideVSWAttributeSet> vswAttributeSet) :
            base(eMethodType.AllocationOverride, method, description, userKey)
        {
            // fields specific to Allocation Override method
            _storeGradeWeekCount = storeGradeWeekCount;
            _percentNeedLimit = percentNeedLimit;
            _exceedMaxInd = exceedMaxInd;
            _reserve = reserve;
            _percentInd = percentInd;
            _reserveAsBulk = reserveAsBulk;
            _reserveAsPacks = reserveAsPacks;
            _merchandise = merchandise;
            _merchandiseHierarchy = merchandiseHierarchy;
            _onHandMerchandise = onHandMerchandise;
            _onHandMerchandiseHierarchy = onHandMerchandiseHierarchy;
            _onHandFactor = onHandFactor;
            _colorMult = colorMult;
            _sizeMult = sizeMult;
            _allColorMin = allColorMin;
            _allColorMax = allColorMax;
            _capacityAttribute = capacityAttribute;
            _exceedCapacity = exceedCapacity;
            _merchPlanUnspecified = merchPlanUnspecified;
            _merchOnHandUnspecified = merchOnHandUnspecified;
            _storeGradesAttribute = storeGradesAttribute;
            if (!Enum.IsDefined(typeof(eMinMaxType), inventoryIndicator))
            {
                throw new Exception("Value " + inventoryIndicator.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _inventoryIndicator = inventoryIndicator;
            }
            if (!Enum.IsDefined(typeof(eMerchandiseType), inventoryBasisMerchType))
            {
                throw new Exception("Value " + inventoryBasisMerchType.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
            {
                _inventoryBasisMerchType = inventoryBasisMerchType;
            }
            _inventoryBasisMerchandise = inventoryBasisMerchandise;
            _inventoryBasisMerchandiseHierarchy = inventoryBasisMerchandiseHierarchy;
            _vswAttribute = vswAttribute;
            _doNotApplyVSW = doNotApplyVSW;
            _storeGradeValues = storeGradeValues;
            _capacity = capacity;
            _colorMinMax = colorMinMax;
            _packRounding = packRounding;
            _vswAttributeSet = vswAttributeSet;
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
        double _exceedByPct;

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

        public double ExceedByPct
        {
            get { return _exceedByPct; }
            set { _exceedByPct = value; }
        }

        public ROMethodOverrideCapacityProperties(KeyValuePair<int, string> attributeSet, bool exceedCapacity, double exceedByPct)
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
        int _colorMinimum;
        [DataMember(IsRequired = true)]
        int _colorMaximum;

        public KeyValuePair<int, string> ColorCode
        {
            get { return _colorCode; }
            set { _colorCode = value; }
        }

        public int ColorMinimum
        {
            get { return _colorMinimum; }
            set { _colorMinimum = value; }
        }

        public int ColorMaximum
        {
            get { return _colorMaximum; }
            set { _colorMaximum = value; }
        }

        public ROMethodOverrideColorProperties(KeyValuePair<int, string> colorCode, int colorMinimum, int colorMaximum)
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
        int _packMultiple;
        [DataMember(IsRequired = true)]
        double _firstPackPct;
        [DataMember(IsRequired = true)]
        double _nthPackPct;

        public int PackMultiple
        {
            get { return _packMultiple; }
            set { _packMultiple = value; }
        }

        public double FirstPackPct
        {
            get { return _firstPackPct; }
            set { _firstPackPct = value; }
        }

        public double NthPackPct
        {
            get { return _nthPackPct; }
            set { _nthPackPct = value; }
        }

        public ROMethodOverridePackRoundingProperties(int packMultiple, double firstPackPct, double nthPackPct)
        {
            // fields specific to Allocation Override Color
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
        int _minimumShipQuantity;
        [DataMember(IsRequired = true)]
        double _pctPackThreshold;
        [DataMember(IsRequired = true)]
        int _itemMaximum;

        public bool Updated
        {
            get { return _updated; }
            set { _updated = value; }
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

        public int MinimumShipQuantity
        {
            get { return _minimumShipQuantity; }
            set { _minimumShipQuantity = value; }
        }

        public double PctPackThreshold
        {
            get { return _pctPackThreshold; }
            set { _pctPackThreshold = value; }
        }

        public int ItemMaximum
        {
            get { return _itemMaximum; }
            set { _itemMaximum = value; }
        }

        public ROMethodOverrideVSW(bool updated, KeyValuePair<int, string> entry, string reservationStore, int minimumShipQuantity, double pctPackThreshold, int itemMaximum)
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

        public ROMethodCreateMasterHeadersProperties(KeyValuePair<int, string> method, string description, int userKey, bool useSelectedHeaders, List<ROMethodCreateMasterHeadersMerchandise> listMerchandise) :
            base(eMethodType.CreateMasterHeaders, method, description, userKey)
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

        public ROMethodDCCartonRoundingProperties(KeyValuePair<int, string> method, string description, int userKey, KeyValuePair<int, string> attribute, eAllocateOverageTo allocateOverageTo) :
            base(eMethodType.DCCartonRounding, method, description, userKey)
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
        private Dictionary<int, ROMethodAllocationVelocityAttributeSet> _matrixAttributeSetValues;
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

        #region Public Properties
        /// <summary>
        /// Gets or sets the Calculate Average Using value
        /// </summary>
        public eVelocityCalculateAverageUsing CalculateAverageUsing
        {
            get { return _calculateAverageUsing; }
            set
            {
                if (!Enum.IsDefined(typeof(eVelocityCalculateAverageUsing), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _calculateAverageUsing = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityDetermineShipQtyUsing), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _determineShipQtyUsing = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityApplyMinMaxType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _applyMinMaxType = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityMethodGradeVariableType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _gradeVariableType = value;
                }
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

        // Store Grade properties
        public eMinMaxType InventoryIndicator
        {
            get { return _inventoryIndicator; }
            set
            {
                if (!Enum.IsDefined(typeof(eMinMaxType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _inventoryIndicator = value;
                }
            }
        }

        public eMerchandiseType InventoryMinMaxMerchType
        {
            get { return _inventoryMinMaxMerchType; }
            set
            {
                if (!Enum.IsDefined(typeof(eMerchandiseType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _inventoryMinMaxMerchType = value;
                }
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
        /// Gets the dictionary of attribute set values
        /// </summary>
        public Dictionary<int, ROMethodAllocationVelocityAttributeSet> MatrixAttributeSetValues
        {
            get { return _matrixAttributeSetValues; }
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

        #endregion
        public ROMethodAllocationVelocityProperties(KeyValuePair<int, string> method, string description, int userKey,
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
            KeyValuePair<int, string> attributeSet
            ) :
            base(eMethodType.Velocity, method, description, userKey)
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
            _matrixAttributeSetValues = new Dictionary<int, ROMethodAllocationVelocityAttributeSet>();
            _matrixViews = new List<KeyValuePair<int, string>>();
            _matrixSelectedView = default(KeyValuePair<int, string>);
            _matrixViewColumns = new List<KeyValuePair<string, bool>>();
            _interactive = false;
            _velocityAction = eVelocityAction.None;
            _components = new List<KeyValuePair<int, string>>();
            _selectedComponent = default(KeyValuePair<int, string>);
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
                if (!Enum.IsDefined(typeof(eVelocityMatrixMode), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _matrixMode = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityRuleType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _noOnHandRule = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _matrixModeAverageRule = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocitySpreadOption), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _spreadOption = value;
                }
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
                if (!Enum.IsDefined(typeof(eVelocityRuleType), value))
                {
                    throw new Exception("Value " + value.ToString() + " is not valid for " + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    _ruleType = value;
                }
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

        public ROAllocationReviewViewDetails(KeyValuePair<int, string> view) :
            base(view)
        {
            _summaryColumns = new List<ROSelectedField>();
            _totalColumns = new List<ROSelectedField>();
            _detailColumns = new List<ROSelectedField>();
        }

        public List<ROSelectedField> SummaryColumns { get { return _summaryColumns; } }
        public List<ROSelectedField> TotalColumns { get { return _totalColumns; } }
        public List<ROSelectedField> DetailColumns { get { return _detailColumns; } }

        public List<ROSelectedField> SummaryColumnsByPosition { get { return _summaryColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }
        public List<ROSelectedField> TotalColumnsByPosition { get { return _totalColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }
        public List<ROSelectedField> DetailColumnsByPosition { get { return _detailColumns.OrderBy(hdr => hdr.VisiblePosition).ToList(); } }

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

        #endregion

        #region Constructor
        public ROAllocationWorklistEntry(int viewKey, string bandKey, string columnKey, int visiblePosition, bool isHidden, bool isGroupByColumn,
               int sortDirection, int sortSequence, int width, string columnType, string headerCharacteristicGroupKey, string label, string itemField = null)
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