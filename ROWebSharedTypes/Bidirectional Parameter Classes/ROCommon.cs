using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROBaseProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        protected bool _isReadOnly;

        [DataMember(IsRequired = true)]
        protected bool _canBeDeleted;

        [DataMember(IsRequired = true)]
        protected bool _canBeProcessed;

        public ROBaseProperties()
        {
            _isReadOnly = true;
            _canBeDeleted = false;
            _canBeProcessed = false;
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        public bool CanBeDeleted
        {
            get { return _canBeDeleted; }
            set { _canBeDeleted = value; }
        }

        public bool CanBeProcessed
        {
            get { return _canBeProcessed; }
            set { _canBeProcessed = value; }
        }
    }

    [DataContract(Name = "ROMethodProperties", Namespace = "http://Logility.ROWeb/")]

    public class ROMethodProperties : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _method;

        [DataMember(IsRequired = true)]
        protected string _description;

        [DataMember(IsRequired = true)]
        protected int _userKey;

        [DataMember(IsRequired = true)]
        protected eMethodType _methodType;

        [DataMember(IsRequired = true)]
        protected bool _isTemplate;

        public ROMethodProperties(
            eMethodType methodType, 
            KeyValuePair<int, string> method, 
            string description, 
            int userKey,
            bool isTemplate
            )
        {
            _method = method;
            _description = description;
            _userKey = userKey;
            _methodType = methodType;
            _isTemplate = isTemplate;
        }

        public KeyValuePair<int, string> Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Method is set.
		/// </summary>
		public bool MethodIsSet
        {
            get { return !_method.Equals(default(KeyValuePair<int, string>)); }
        }

        public bool AddingMethod
        {
            get { return Method.Key == Include.NoRID; }
        }

        public bool UpdatingMethod
        {
            get { return Method.Key != Include.NoRID; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the Description is set.
		/// </summary>
		public bool DescriptionIsSet
        {
            get { return _description != null; }
        }

        public int UserKey
        {
            get { return _userKey; }
            set { _userKey = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the UserKey is set.
		/// </summary>
		public bool UserKeyIsSet
        {
            get { return _userKey != Include.Undefined; }
        }

        public eMethodType MethodType
        {
            get { return _methodType; }
            set { _methodType = value; }
        }

        /// <summary>
		/// Gets a flag identifying if the MethodType is set.
		/// </summary>
		public bool MethodTypeIsSet
        {
            get { return _methodType != eMethodType.NotSpecified; }
        }

        public eGlobalUserType GlobalUserType
        {
            get
            {
                if (UserKey == Include.GetGlobalUserRID())
                {
                    return eGlobalUserType.Global;
                }
                else
                {
                    return eGlobalUserType.User;
                }
            }
        }

        /// <summary>
        /// Flag identifying if the method is a template method
        /// </summary>
        public bool IsTemplate
        {
            get { return _isTemplate; }
            set { _isTemplate = value; }
        }
    }

    [DataContract(Name = "ROWorkflowStep", Namespace = "http://Logility.ROWeb/")]
    public class ROWorkflowStep
    {
        [DataMember(IsRequired = true)]
        private int _rowPosition;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _methodAction;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _method;


        #region Public Properties
        public int RowPosition
        {
            get { return _rowPosition; }
            set { _rowPosition = value; }
        }

        public KeyValuePair<int, string> MethodAction
        {
            get { return _methodAction; }
            set { _methodAction = value; }
        }
        public KeyValuePair<int, string> Method
        {
            get { return _method; }
            set { _method = value; }
        }


        #endregion
        public ROWorkflowStep(int rowPosition, KeyValuePair<int, string> methodAction, KeyValuePair<int, string> method)
        {
            _rowPosition = rowPosition;
            _methodAction = methodAction;
            _method = method;
        }
    }

    [DataContract(Name = "ROAllocationWorkflowStep", Namespace = "http://Logility.ROWeb/")]
    public class ROAllocationWorkflowStep : ROWorkflowStep
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _specific;

        [DataMember(IsRequired = true)]
        eComponentType _componentType;

        [DataMember(IsRequired = true)]
        bool _review;

        [DataMember(IsRequired = true)]
        double _dTolerancePercent;

        #region Public Properties
        public KeyValuePair<int, string> Specific
        {
            get { return _specific; }
            set { _specific = value; }
        }
        public eComponentType ComponentType
        {
            get { return _componentType; }
            set { _componentType = value; }
        }


        public double TolerancePercent
        {
            get { return _dTolerancePercent; }
            set { _dTolerancePercent = value; }
        }

        public bool Review
        {
            get { return _review; }
            set { _review = value; }
        }

        //aws.Review, tolerancePct

        #endregion
        public ROAllocationWorkflowStep(int rowPosition, KeyValuePair<int, string> methodAction, KeyValuePair<int, string> method,
            KeyValuePair<int, string> specific, eComponentType componentType, bool Review, double TolerancePercent) :
            base(rowPosition, methodAction, method)

        {
            _specific = specific;
            _componentType = componentType;
            _review = Review;
            _dTolerancePercent = TolerancePercent;
        }
    }

    [DataContract(Name = "ROPlanningWorkflowStep", Namespace = "http://Logility.ROWeb/")]
    public class ROPlanningWorkflowStep : ROWorkflowStep
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _specific;

        [DataMember(IsRequired = true)]
        bool _review;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _variable;

        [DataMember(IsRequired = true)]
        double _dTolerancePercent;

        [DataMember(IsRequired = true)]
        bool _balanceIndicator;

        [DataMember(IsRequired = true)]
        string _computationMode;

        #region Public Properties
        public KeyValuePair<int, string> Specific
        {
            get { return _specific; }
            set { _specific = value; }
        }

        public double TolerancePercent
        {
            get { return _dTolerancePercent; }
            set { _dTolerancePercent = value; }
        }

        public bool Review
        {
            get { return _review; }
            set { _review = value; }
        }

        public KeyValuePair<int, string> Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        public bool BalanceIndicator
        {
            get { return _balanceIndicator; }
            set { _balanceIndicator = value; }
        }

        public string ComputationMode
        {
            get { return _computationMode; }
            set { _computationMode = value; }
        }

        #endregion

        public ROPlanningWorkflowStep(int rowPosition, KeyValuePair<int, string> methodAction, KeyValuePair<int, string> method,
            KeyValuePair<int, string> specific, bool Review, double TolerancePercent, KeyValuePair<int, string> variable,
            bool balanceIndicator = true, string computationMode = "Default") :
            base(rowPosition, methodAction, method)
        {
            _specific = specific;
            _variable = variable;
            _balanceIndicator = balanceIndicator;
            _computationMode = computationMode;
            _review = Review;
            _dTolerancePercent = TolerancePercent;
        }
    }

    [DataContract(Name = "ROWorkflow", Namespace = "http://Logility.ROWeb/")]
    public class ROWorkflow : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        eWorkflowType _workflowType;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _workflow;

        [DataMember(IsRequired = true)]
        string _workflowdescription;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _filter;

        [DataMember(IsRequired = true)]
        List<ROWorkflowStep> _workflowSteps;

        [DataMember(IsRequired = true)]
        eGlobalUserType _userType;

        [DataMember(IsRequired = true)]
        int _userId;

        [DataMember(IsRequired = true)]
        eProfileType _profileType;

        [DataMember(IsRequired = true)]
        bool _isFilled;


        #region Public Properties
        public eWorkflowType WorkflowType
        {
            get { return _workflowType; }
            set { _workflowType = value; }
        }

        public KeyValuePair<int, string> Workflow
        {
            get { return _workflow; }
            set { _workflow = value; }
        }

        public string WorkflowDescription
        {
            get { return _workflowdescription; }
            set { _workflowdescription = value; }
        }
        public KeyValuePair<int, string> Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public List<ROWorkflowStep> WorkflowSteps
        {
            get { return _workflowSteps; }
            set { _workflowSteps = value; }
        }


        public eGlobalUserType UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public eProfileType ProfileType
        {
            get { return _profileType; }
            set { _profileType = value; }
        }

        public bool isFilled
        {
            get { return _isFilled; }
            set { _isFilled = value; }
        }


        #endregion
        public ROWorkflow(eWorkflowType workflowType, KeyValuePair<int, string> workflow, string workflowdescription, KeyValuePair<int, string> filter,
            eGlobalUserType usertype, int userid, eProfileType profiletype, bool isfilled, List<ROWorkflowStep> workflowSteps)
        {
            _workflow = workflow;
            _workflowdescription = workflowdescription;
            _filter = filter;
            _workflowType = workflowType;
            _userType = usertype;
            _userId = userid;
            _profileType = profiletype;
            _isFilled = isfilled;
            _workflowSteps = workflowSteps;
        }

        public eGlobalUserType GlobalUserType
        {
            get
            {
                if (_userId == Include.GetGlobalUserRID())
                {
                    return eGlobalUserType.Global;
                }
                else
                {
                    return eGlobalUserType.User;
                }
            }
        }

        //public void addWorkflowStep(ROWorkflowStep workflowStep)
        //{
        //    workflowStep.RowPosition = WorkflowSteps.Count;
        //    _workflowSteps.Add(workflowStep);
        //}
    }

    [DataContract(Name = "ROCharacteristic", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristic
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _characteristicGroup;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _characteristicValue;

        [DataMember(IsRequired = true)]
        string _inheritedFrom;

        public ROCharacteristic(int characteristicGroupKey, string characteristicGroupName, int characteristicValueKey, string characteristicValueName, string inheritedFrom = null)
        {
            _characteristicGroup = new KeyValuePair<int, string>(characteristicGroupKey, characteristicGroupName);
            _characteristicValue = new KeyValuePair<int, string>(characteristicValueKey, characteristicValueName);
            _inheritedFrom = inheritedFrom;
        }

        public KeyValuePair<int, string> CharacteristicGroup { get { return _characteristicGroup; } set { _characteristicGroup = value; } }
        public KeyValuePair<int, string> CharacteristicValue { get { return _characteristicValue; } set { _characteristicValue = value; } }
        public string InheritedFrom { get { return _inheritedFrom; } }

        public string CharacteristicGroupName { get { return _characteristicGroup.Value; } }

        public string CharacteristicValueName { get { return _characteristicValue.Value; } }
    }

    public class ROOverrideLowLevel
    {
        public ROLevelInformation LowLevel { get; set; }

        public KeyValuePair<int, string> OverrideLowLevelsModel { get; set; }
    }

    [DataContract(Name = "ROStoreGrade", Namespace = "http://Logility.ROWeb/")]
    public class ROStoreGrade
    {
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> StoreGrade { get; set; }
        [DataMember(IsRequired = true)]
        public int? Minimum { get; set; }
        [DataMember(IsRequired = true)]
        public int? Maximum { get; set; }

        public bool MinimumIsSet { get { return Minimum != null; } }
        public bool MaximumIsSet { get { return Maximum != null; } }
    }

    [DataContract(Name = "ROAttributeSetStoreGrade", Namespace = "http://Logility.ROWeb/")]
    // Uses ROStoreGrade so can be either ROAllocationStoreGrade or ROPlanningStoreGrade
    public class ROAttributeSetStoreGrade
    {
        [DataMember(IsRequired = true)]
        public KeyValuePair<int, string> AttributeSet { get; set; }
        [DataMember(IsRequired = true)]
        public List<ROStoreGrade> StoreGrades { get; set; }

        public ROAttributeSetStoreGrade()
        {
            StoreGrades = new List<ROStoreGrade>();
        }
    }

    public class ROStockMinMax
    {
        public KeyValuePair<int, string> StoreGrouplevel { get; set; }

        public KeyValuePair<int, string> Merchandise { get; set; }
        public int Boundary { get; set; }

        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public KeyValuePair<int, string> StoreGrade { get; set; }
        public KeyValuePair<int, string> DateRange { get; set; }
        public string Picture { get; set; }

    }

    public class ROLevelInformation
    {
        public eROLevelsType LevelType { get; set; }
        public int LevelSequence { get; set; }
        public string LevelValue { get; set; }
        public int LevelOffset { get; set; }
    }

    #region "Bidirectional Class for ColumnChooser #RO-2772"
    [DataContract(Name = "ROSelectedField", Namespace = "http://Logility.ROWeb/")]
    public class ROSelectedField
    {
        #region MemberVariables

        [DataMember(IsRequired = true)]
        private KeyValuePair<string, string> _field;

        [DataMember(IsRequired = true)]
        private bool _isselected;

        [DataMember(IsRequired = true)]
        private eSortDirection _sortDirection;

        [DataMember(IsRequired = true)]
        private int _width;

        [DataMember(IsRequired = true)]
        private int _visiblePosition;

        #endregion

        #region "Public Properties"
        public KeyValuePair<string, string> Field
        {
            get { return _field; }
            set { _field = value; }
        }

        public bool IsSelected
        {
            get { return _isselected; }
            set { _isselected = value; }
        }

        public eSortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int VisiblePosition
        {
            get { return _visiblePosition; }
            set { _visiblePosition = value; }
        }

        #endregion

        #region Constructor
        public ROSelectedField(
            string fieldkey, 
            string field, 
            bool selected, 
            eSortDirection sortDirection = eSortDirection.None, 
            int width = Include.DefaultColumnWidth, 
            int visiblePosition = 0
            )
        {
            _field = new KeyValuePair<string, string>(fieldkey, field);
            _isselected = selected;
            _sortDirection = sortDirection;
            _width = width;
            _visiblePosition = visiblePosition;
        }
        #endregion  

        override public string ToString()
        {
            return _field.Value;
        }
    }

    #endregion


    [DataContract(Name = "ROVariableGroupings", Namespace = "http://Logility.ROWeb/")]
    public class ROVariableGroupings
    {
        [DataMember(IsRequired = true)]

        private List<ROVariableGrouping> _variableGrouping;

        [DataMember(IsRequired = true)]
        private List<ROSelectedField> _selectedVariables;

        public List<ROVariableGrouping> VariableGrouping
        {
            get { return _variableGrouping; }
            set { _variableGrouping = value; }
        }

        public List<ROSelectedField> SelectedVariables
        {
            get { return _selectedVariables; }
            set { _selectedVariables = value; }
        }

        public ROVariableGroupings(List<ROVariableGrouping> variableGrouping = null)
        {
            _variableGrouping = variableGrouping;
            _selectedVariables = new List<ROSelectedField>();
        }

    }
    [DataContract(Name = "ROVariableGrouping", Namespace = "http://Logility.ROWeb/")]
    public class ROVariableGrouping
    {
        [DataMember(IsRequired = true)]
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        [DataMember(IsRequired = true)]
        private List<ROVariable> _variables;

        public List<ROVariable> Variables
        {
            get { return _variables; }
            set { _variables = value; }
        }

        public ROVariableGrouping(string name, List<ROVariable> variables)
        {
            _name = name;
            _variables = variables;
        }

        override public string ToString()
        {
            return _name;
        }

    }
    [DataContract(Name = "ROVariable", Namespace = "http://Logility.ROWeb/")]
    public class ROVariable
    {
        [DataMember(IsRequired = true)]
        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [DataMember(IsRequired = true)]
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [DataMember(IsRequired = true)]
        private bool _isSelectable;

        public bool IsSelectable
        {
            get { return _isSelectable; }
            set { _isSelectable = value; }
        }

        [DataMember(IsRequired = true)]
        private bool _isDisplayed;

        public bool IsDisplayed
        {
            get { return _isDisplayed; }
            set { _isDisplayed = value; }
        }

        [DataMember(IsRequired = true)]
        private int _sequence;

        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        public ROVariable(int iNumber,string sName,bool bIsSelectable,bool bIsDisplayed, int sequence)
        {
            _number = iNumber;
            _name = sName;
            _isSelectable = bIsSelectable;
            _isDisplayed = bIsDisplayed;
            _sequence = sequence;
        }

        override public string ToString()
        {
            return _name;
        }
    }

    [DataContract(Name = "ROSizeCurveGroupBox", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveGroupBox
    {
        // input parameters
        [DataMember(IsRequired = true)]
        private int _sizeCurveGroupRID;

        [DataMember(IsRequired = true)]
        private int _storeFilterRID;

        [DataMember(IsRequired = true)]
        private int _genCurveHcgRID;

        [DataMember(IsRequired = true)]
        private int _genCurveHnRID;

        [DataMember(IsRequired = true)]
        private int _genCurvePhRID;

        [DataMember(IsRequired = true)]
        private int _genCurvePhlSequence;

        [DataMember(IsRequired = true)]
        private bool _genCurveColorInd;

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

        public ROSizeCurveGroupBox(int sizeCurveGroupRID, int storeFilterRID, int genCurveHcgRID, int genCurveHnRID, int genCurvePhRID, int genCurvePhlSequence, bool genCurveColorInd, eMerchandiseType genCurveMerchType,
            bool isUseDefault, bool isApplyRulesOnly, int sizeCurveKey, string sizeCurveName, int sizeCurveGenericHierarchyKey, string sizeCurveGenericHierarchyName, int sizeCurveGenericNameExtensionKey, string sizeCurveGenericNameExtensionName)
        {
            _sizeCurveGroupRID = sizeCurveGroupRID;
            _storeFilterRID = storeFilterRID;
            _genCurveHcgRID = genCurveHcgRID;
            _genCurveHnRID = genCurveHnRID;
            _genCurvePhRID = genCurvePhRID;
            _genCurvePhlSequence = genCurvePhlSequence;
            _genCurveColorInd = genCurveColorInd;
            _genCurveMerchType = genCurveMerchType;
            _isUseDefault = isUseDefault;
            _isApplyRulesOnly = isApplyRulesOnly;
            _sizeCurve = new KeyValuePair<int, string>(sizeCurveKey, sizeCurveName);
            _sizeCurveGenericHierarchy = new KeyValuePair<int, string>(sizeCurveGenericHierarchyKey, sizeCurveGenericHierarchyName);
            _sizeCurveGenericNameExtension = new KeyValuePair<int, string>(sizeCurveGenericNameExtensionKey, sizeCurveGenericNameExtensionName);
        }

        public int SizeCurveGroupRID { get { return _sizeCurveGroupRID; } set { _sizeCurveGroupRID = value; } }
        public int StoreFilterRID { get { return _storeFilterRID; } set { _storeFilterRID = value; } }
        public int GenCurveHcgRID { get { return _genCurveHcgRID; } set { _genCurveHcgRID = value; } }
        public int GenCurveHnRID { get { return _genCurveHnRID; } set { _genCurveHnRID = value; } }
        public int GenCurvePhRID { get { return _genCurvePhRID; } set { _genCurvePhRID = value; } }
        public int GenCurvePhlSequence { get { return _genCurvePhlSequence; } set { _genCurvePhlSequence = value; } }
        public bool GenCurveColorInd { get { return _genCurveColorInd; } set { _genCurveColorInd = value; } }
        public eMerchandiseType GenCurveMerchType { get { return _genCurveMerchType; } set { _genCurveMerchType = value; } }
        public bool IsUseDefault { get { return _isUseDefault; } set { _isUseDefault = value; } }
        public bool IsApplyRulesOnly { get { return _isApplyRulesOnly; } set { _isApplyRulesOnly = value; } }
        public KeyValuePair<int, string> SizeCurve { get { return _sizeCurve; } set { _sizeCurve = value; } }
        public KeyValuePair<int, string> SizeCurveGenericHierarchy { get { return _sizeCurveGenericHierarchy; } set { _sizeCurveGenericHierarchy = value; } }
        public KeyValuePair<int, string> SizeCurveGroupBoxValue { get { return _sizeCurveGenericNameExtension; } set { _sizeCurveGenericNameExtension = value; } }

    }

    [DataContract(Name = "ROUpdateContent", Namespace = "http://Logility.ROWeb/")]
    public class ROUpdateContent
    {
        [DataMember(IsRequired = true)]
        private eContentType _contentType;
        [DataMember(IsRequired = true)]
        private eChangeType _changeType;
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private int _newKey;
        [DataMember(IsRequired = true)]
        private int _count;

        public ROUpdateContent(eContentType contentType, eChangeType changeType, int count)
        {
            _contentType = contentType;
            _changeType = changeType;
            _count = count;
        }

        public ROUpdateContent(eContentType contentType, eChangeType changeType, int key = Include.NoRID, int newKey = Include.NoRID, int count = 0)
        {
            _contentType = contentType;
            _changeType = changeType;
            _key = key;
            _newKey = newKey;
            _count = count;
        }

        public eContentType ContentType
        {
            get { return _contentType; }
        }

        public eChangeType ChangeType
        {
            get { return _changeType; }
        }

        public int Key
        {
            get { return _key; }
        }

        public int NewKey
        {
            get { return _newKey; }
        }

        public int Count
        {
            get { return _count; }
        }
    }

    [DataContract(Name = "ROProfileKey", Namespace = "http://Logility.ROWeb/")]
    public class ROProfileKey
    {
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;
        [DataMember(IsRequired = true)]
        private int _key;

        public ROProfileKey(eProfileType profileType, int key)
        {
            _profileType = profileType;
            _key = key;
        }


        public eProfileType ProfileType
        {
            get { return _profileType; }
        }

        public int Key
        {
            get { return _key; }
        }

    }

    [DataContract(Name = "ROCalendarDateInfo", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarDateInfo
    {
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private string _displayDate;
        [DataMember(IsRequired = true)]
        private eCalendarRangeType _dateRangeType;

        public ROCalendarDateInfo(int key, string displayDate = null, eCalendarRangeType dateRangeType = eCalendarRangeType.Static)
        {
            _key = key;
            _displayDate = displayDate;
            _dateRangeType = dateRangeType;
        }

        public int Key
        {
            get { return _key; }
        }

        public string DisplayDate
        {
            get { return _displayDate; }
        }

        public eCalendarRangeType DateRangeType
        {
            get { return _dateRangeType; }
        }


    }

    [DataContract(Name = "ROSize", Namespace = "http://Logility.ROWeb/")]
    public class ROSize
    {

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _size;

        [DataMember(IsRequired = true)]
        string _name;

        [DataMember(IsRequired = true)]
        string _productCategory;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _primary;

        [DataMember(IsRequired = true)]
        int _primarySequence;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _secondary;

        [DataMember(IsRequired = true)]
        int _secondarySequence;

        #region Public Property
        public KeyValuePair<int, string> Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ProductCategory
        {
            get { return _productCategory; }
            set { _productCategory = value; }
        }

        public KeyValuePair<int, string> Primary
        {
            get { return _primary; }
            set { _primary = value; }
        }

        public int PrimarySequence
        {
            get { return _primarySequence; }
            set { _primarySequence = value; }
        }

        public KeyValuePair<int, string> Secondary
        {
            get { return _secondary; }
            set { _secondary = value; }
        }

        public int SecondarySequence
        {
            get { return _secondarySequence; }
            set { _secondarySequence = value; }
        }

        #endregion

        public ROSize(KeyValuePair<int, string> size,
            string name,
            string productCategory,
            KeyValuePair<int, string> primary,
            int primarySequence,
            KeyValuePair<int, string> secondary,
            int secondarySequence)

        {
            _size = size;
            _name = name;
            _productCategory = productCategory;
            _primary = primary;
            _primarySequence = primarySequence;
            _secondary = secondary;
            _secondarySequence = secondarySequence;
        }
    }

    [DataContract(Name = "ROBasisWithLevelDetailProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROBasisWithLevelDetailProfile : ROBasisDetailProfile
    {
        [DataMember(IsRequired = true)]
        private eMerchandiseType _merchandiseType;
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

        [DataMember(IsRequired = true)]
        private int _merchPhRid;
        public int MerchPhRId
        {
            get { return _merchPhRid; }
            set { _merchPhRid = value; }
        }

        [DataMember(IsRequired = true)]
        private int _merchPhlSequence;
        public int MerchPhlSequence
        {
            get { return _merchPhlSequence; }
            set { _merchPhlSequence = value; }
        }

        [DataMember(IsRequired = true)]
        private int _merchOffset;
        public int MerchOffset
        {
            get { return _merchOffset; }
            set { _merchOffset = value; }
        }


        public ROBasisWithLevelDetailProfile(int iBasisId, int iMerchandiseId, string sMerchandise, int iVersionId, string sVersion, int iDaterangeId,
            string sDateRange, string sPicture, float fWeight, bool bIsIncluded, string sIncludeButton, eMerchandiseType merchandiseType, int iMerchPhRId,
            int iMerchPhlSequence, int iMerchOffset

            ) : base(iBasisId, iMerchandiseId, sMerchandise, iVersionId, sVersion, iDaterangeId, sDateRange, sPicture, fWeight, bIsIncluded, sIncludeButton)
        {
            MerchandiseType = merchandiseType;
            _merchPhRid = iMerchPhRId;
            _merchPhlSequence = iMerchPhlSequence;
            _merchOffset = iMerchOffset;
        }
    }

    [DataContract(Name = "ROMerchandiseListEntry", Namespace = "http://Logility.ROWeb/")]
    public class ROMerchandiseListEntry
    {
        [DataMember(IsRequired = true)]
        private int _sequenceNumber;
        public int SequenceNumber
        {
            get { return _sequenceNumber; }
            set { _sequenceNumber = value; }
        }

        [DataMember(IsRequired = true)]
        private eMerchandiseType _merchandiseType;
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }

        [DataMember(IsRequired = true)]
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [DataMember(IsRequired = true)]
        private int _key;
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }


        public ROMerchandiseListEntry(int sequenceNumber, eMerchandiseType merchandiseType, string text, int key) 
        {
            _sequenceNumber = sequenceNumber;
            _merchandiseType = merchandiseType;
            _text = text;
            _key = key;
        }
    }

    [DataContract(Name = "ROViewDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROViewDetails
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _view;

        [DataMember(IsRequired = true)]
        protected bool _isUserView;

        public ROViewDetails(KeyValuePair<int, string> view, bool isUserView = false)
        {
            _view = view;
            _isUserView = isUserView;
        }

        /// <summary>
        /// Gets the flag identifying if the view has been set.
        /// </summary>
        public bool ViewIsSet
        {
            get { return !_view.Equals(default(KeyValuePair<int, string>)); }
        }

        public KeyValuePair<int, string> View { get { return _view; } set { _view = value; } }

        public bool IsUserView { get { return _isUserView; } set { _isUserView = value; } }  

    }

    

    
}