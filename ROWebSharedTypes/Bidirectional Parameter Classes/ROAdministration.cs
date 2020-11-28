
using System;
using MIDRetail.DataCommon;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    
    [DataContract(Name = "ROUserInformation", Namespace = "http://Logility.ROWeb/")]
    public class ROUserInformation
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _user;

        [DataMember(IsRequired = true)]
        bool _lookupUserKey;

        [DataMember(IsRequired = true)]
        string _fullName;

        [DataMember(IsRequired = true)]
        string _description;

        [DataMember(IsRequired = true)]
        bool _isActive;

        [DataMember(IsRequired = true)]
        bool _isSetToBeDeleted;

        [DataMember(IsRequired = true)]
        DateTime _dateTimeWhenDeleted;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _createUserLike;

        [DataMember(IsRequired = true)]
        string _addToGroup;

        #region Public Properties
        public KeyValuePair<int, string> User
        {
            get { return _user; }
            set { _user = value; }
        }

        public bool LookupUserKey
        {
            get { return _lookupUserKey; }
            set { _lookupUserKey = value; }
        }

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsSetToBeDeleted
        {
            get { return _isSetToBeDeleted; }
            set { _isSetToBeDeleted = value; }
        }

        public DateTime DateTimeWhenDeleted
        {
            get { return _dateTimeWhenDeleted; }
            set { _dateTimeWhenDeleted = value; }
        }

        public KeyValuePair<int, string> CreateUserLike
        {
            get { return _createUserLike; }
            set { _createUserLike = value; }
        }

        public string AddToGroup
        {
            get { return _addToGroup; }
            set { _addToGroup = value; }
        }

        #endregion
        public ROUserInformation(KeyValuePair<int, string> user, 
            bool lookupUserKey = true,
            string userName = null, 
            string userFullName = null, 
            string userDescription = null, 
            bool isActive = true, 
            bool isSetToBeDeleted = false, 
            DateTime dateTimeWhenDeleted = default(DateTime), 
            KeyValuePair<int, string> createUserLike = default(KeyValuePair<int, string>),
            string addToGroup = null)
        {
            _user = user;
            _lookupUserKey = lookupUserKey;
            _fullName = userFullName;
            _description = userDescription;
            _isActive = isActive;
            _isSetToBeDeleted = isSetToBeDeleted;
            _dateTimeWhenDeleted = dateTimeWhenDeleted;
            _createUserLike = createUserLike;
            _addToGroup = addToGroup;
        }
    }

    [DataContract(Name = "ROCharacteristicsProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristicsProperties
    {
        [DataMember(IsRequired = true)]
        eProfileType _profileType;

        [DataMember(IsRequired = true)]
        List<ROCharacteristicDefinition> _characteristics;

        #region Public Properties


        public eProfileType ProfileType
        {
            get { return _profileType; }
            set { _profileType = value; }
        }

        public List<ROCharacteristicDefinition> Characteristics
        {
            get { return _characteristics; }
            set { _characteristics = value; }
        }

        #endregion

        public ROCharacteristicsProperties(eProfileType profileType)
        {
            _profileType = profileType;
            _characteristics = new List<ROCharacteristicDefinition>();
        }
    }

    [DataContract(Name = "ROCharacteristicDefinition", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristicDefinition : IComparable<ROCharacteristicDefinition>
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _characteristic;

        [DataMember(IsRequired = true)]
        eCharacteristicValueType _characteristicValueType;

        [DataMember(IsRequired = true)]
        bool _isList;

        [DataMember(IsRequired = true)]
        bool _isProtect;

        [DataMember(IsRequired = true)]
        List<ROCharacteristicValue> _characteristicsValues;

        #region Public Properties

        public KeyValuePair<int, string> Characteristic
        {
            get { return _characteristic; }
            set { _characteristic = value; }
        }

        public eCharacteristicValueType CharacteristicValueType
        {
            get { return _characteristicValueType; }
            set { _characteristicValueType = value; }
        }

        public bool IsList
        {
            get { return _isList; }
            set { _isList = value; }
        }

        public bool IsProtect
        {
            get { return _isProtect; }
            set { _isProtect = value; }
        }

        public List<ROCharacteristicValue> CharacteristicsValues
        {
            get { return _characteristicsValues; }
            set { _characteristicsValues = value; }
        }

        #endregion
        public ROCharacteristicDefinition(KeyValuePair<int, string> characteristic, eCharacteristicValueType characteristicValueType, bool isList, bool isProtect)
        {
            _characteristic = characteristic;
            _characteristicValueType = characteristicValueType;
            _isList = isList;
            _isProtect = isProtect;
            _characteristicsValues = new List<ROCharacteristicValue>();
        }

        // Default comparer for ROCharacteristicDefinition type.
        public int CompareTo(ROCharacteristicDefinition compareCharacteristic)
        {
            // A null value means that this object is greater.
            if (compareCharacteristic == null)
                return 1;

            else
                return this.Characteristic.Value.CompareTo(compareCharacteristic.Characteristic.Value);
        }

    }

    [DataContract(Name = "ROCharacteristicValue", Namespace = "http://Logility.ROWeb/")]
    public class ROCharacteristicValue : IComparable<ROCharacteristicValue>
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, object> _characteristicValue;

        [DataMember(IsRequired = true)]
        eCharacteristicValueType _characteristicValueType;

        [DataMember(IsRequired = true)]
        int _maximumLength;

        #region Public Properties


        public KeyValuePair<int, object> CharacteristicValue
        {
            get { return _characteristicValue; }
            set { _characteristicValue = value; }
        }

        public eCharacteristicValueType CharacteristicValueType
        {
            get { return _characteristicValueType; }
            set { _characteristicValueType = value; }
        }

        public int MaximumLength
        {
            get { return _maximumLength; }
            set { _maximumLength = value; }
        }

        #endregion
        public ROCharacteristicValue(KeyValuePair<int, object> characteristicValue, eCharacteristicValueType characteristicValueType = eCharacteristicValueType.unknown, int maximumLength = 50)
        {
            _characteristicValue = characteristicValue;
            _characteristicValueType = characteristicValueType;
            _maximumLength = maximumLength;
        }

        // Default comparer for ROCharacteristicDefinition type.
        public int CompareTo(ROCharacteristicValue compareCharacteristicValue)
        {
            // A null value means that this object is greater.
            if (compareCharacteristicValue == null)
                return 1;

            else
            {
                if (this.CharacteristicValueType == eCharacteristicValueType.text)
                {
                    return Convert.ToString(this.CharacteristicValue.Value).CompareTo(Convert.ToString(compareCharacteristicValue.CharacteristicValue.Value));
                }
                else if (this.CharacteristicValueType == eCharacteristicValueType.date)
                {
                    return Convert.ToDateTime(this.CharacteristicValue.Value).CompareTo(Convert.ToDateTime(compareCharacteristicValue.CharacteristicValue.Value));
                }
                else if (this.CharacteristicValueType == eCharacteristicValueType.number)
                {
                    return Convert.ToSingle(this.CharacteristicValue.Value).CompareTo(Convert.ToSingle(compareCharacteristicValue.CharacteristicValue.Value));
                }
                else if (this.CharacteristicValueType == eCharacteristicValueType.dollar)
                {
                    return Convert.ToSingle(this.CharacteristicValue.Value).CompareTo(Convert.ToSingle(compareCharacteristicValue.CharacteristicValue.Value));
                }

                // return default string compare
                return Convert.ToString(this.CharacteristicValue.Value).CompareTo(Convert.ToString(compareCharacteristicValue.CharacteristicValue.Value));
            }

        }
    }

    [DataContract(Name = "ROModelProperties", Namespace = "http://Logility.ROWeb/")]

    public abstract class ROModelProperties : ROBaseProperties
    {
        [DataMember(IsRequired = true)]
        protected KeyValuePair<int, string> _model;


        [DataMember(IsRequired = true)]
        protected eModelType _modelType;

        public ROModelProperties(eModelType modelType, KeyValuePair<int, string> model)
        {
            _model = model;
            _modelType = modelType;
        }

        public KeyValuePair<int, string> Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public eModelType ModelType
        {
            get { return _modelType; }
            set { _modelType = value; }
        }

    }

    [DataContract(Name = "ROModelEligibilityProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelEligibilityProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        protected List<ROCalendarDateInfo> _modelStockEligibilityEntry;

        [DataMember(IsRequired = true)]
        protected List<ROCalendarDateInfo> _modelSalesEligibilityEntry;

        [DataMember(IsRequired = true)]
        protected List<ROCalendarDateInfo> _modelPriorityShippingEntry;


        #region Public Properties

        public List<ROCalendarDateInfo> ModelStockEligibilityEntry
        {
            get { return _modelStockEligibilityEntry; }
        }

        public List<ROCalendarDateInfo> ModelSalesEligibilityEntry
        {
            get { return _modelSalesEligibilityEntry; }
        }

        public List<ROCalendarDateInfo> ModelPriorityShippingEntry
        {
            get { return _modelPriorityShippingEntry; }
        }

        #endregion
        public ROModelEligibilityProperties(KeyValuePair<int, string> model) :
            base(eModelType.Eligibility, model)

        {
            _modelStockEligibilityEntry = new List<ROCalendarDateInfo>();
            _modelSalesEligibilityEntry = new List<ROCalendarDateInfo>();
            _modelPriorityShippingEntry = new List<ROCalendarDateInfo>();
        }
    }

    [DataContract(Name = "ROModelValue", Namespace = "http://Logility.ROWeb/")]
    public class ROModelValue
    {
        [DataMember(IsRequired = true)]
        private double _modelValue;

        [DataMember(IsRequired = true)]
        private ROCalendarDateInfo _calendarDateInfo;


        #region Public Properties

        public double ModelValue
        {
            get { return _modelValue; }
            set { _modelValue = value; }
        }
        public ROCalendarDateInfo CalendarDateInfo
        {
            get { return _calendarDateInfo; }
            set { _calendarDateInfo = value; }
        }

        #endregion
        public ROModelValue(double modelValue, ROCalendarDateInfo calendarDateInfo)
        {
            _modelValue = modelValue;
            _calendarDateInfo = calendarDateInfo;

        }
    }

    [DataContract(Name = "ROModelValuesProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelValuesProperties
    {
        [DataMember(IsRequired = true)]
        private double _defaultValue;

        [DataMember(IsRequired = true)]
        private List<ROModelValue> _modelValues;

        #region Public Properties

        public double DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public bool DefaultValueIsSet
        {
            get
            {
                if (_defaultValue > Include.Undefined)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<ROModelValue> ModelValues
        {
            get { return _modelValues; }
        }

        #endregion
        public ROModelValuesProperties(double defaultValue)
        {
            _defaultValue = defaultValue;
            _modelValues = new List<ROModelValue>();
        }
    }

    [DataContract(Name = "ROModelStockModifierProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelStockModifierProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        private ROModelValuesProperties _modelValuesProperties;

        #region Public Properties

        public ROModelValuesProperties ModelValuesProperties
        {
            get { return _modelValuesProperties; }
            set { _modelValuesProperties = value; }
        }

        #endregion

        public ROModelStockModifierProperties(KeyValuePair<int, string> model) :
            base(eModelType.StockModifier, model)

        {
            
        }
    }

    [DataContract(Name = "ROModelSalesModifierProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelSalesModifierProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        private ROModelValuesProperties _modelValuesProperties;

        #region Public Properties

        public ROModelValuesProperties ModelValuesProperties
        {
            get { return _modelValuesProperties; }
            set { _modelValuesProperties = value; }
        }

        #endregion

        public ROModelSalesModifierProperties(KeyValuePair<int, string> model) :
            base(eModelType.SalesModifier, model)

        {
            
        }
    }

    [DataContract(Name = "ROModelOverrideLowLevelsProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelOverrideLowLevelsProperties : ROModelProperties
    {
        // fields specific to Model


        #region Public Properties

        #endregion
        public ROModelOverrideLowLevelsProperties(KeyValuePair<int, string> model) :
            base(eModelType.OverrideLowLevel, model)

        {
            // fields specific to Model

        }
    }

    [DataContract(Name = "ROModelFWOSModifierProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelFWOSModifierProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        private ROModelValuesProperties _modelValuesProperties;

        #region Public Properties

        public ROModelValuesProperties ModelValuesProperties
        {
            get { return _modelValuesProperties; }
            set { _modelValuesProperties = value; }
        }

        #endregion
        public ROModelFWOSModifierProperties(KeyValuePair<int, string> model) :
            base(eModelType.FWOSModifier, model)

        {

        }
    }

    [DataContract(Name = "ROModelFWOSMaxProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelFWOSMaxProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        private ROModelValuesProperties _modelValuesProperties;

        #region Public Properties

        public ROModelValuesProperties ModelValuesProperties
        {
            get { return _modelValuesProperties; }
            set { _modelValuesProperties = value; }
        }

        #endregion

        public ROModelFWOSMaxProperties(KeyValuePair<int, string> model) :
            base(eModelType.FWOSMax, model)

        {
            // fields specific to Model

        }
    }

    [DataContract(Name = "ROSizeConstraintValues", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintValues
    {
        [DataMember(IsRequired = true)]
        int _minimum;

        [DataMember(IsRequired = true)]
        int _maximum;

        [DataMember(IsRequired = true)]
        int _multiple;

        #region Public Properties

        public bool MinimumSet
        {
            get { return _minimum != 0; }
        }

        public int Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        public bool MaximumSet
        {
            get { return _maximum != int.MaxValue; }
        }

        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        public bool MultipleSet
        {
            get { return _multiple != Include.Undefined; }
        }

        public int Multiple
        {
            get { return _multiple; }
            set { _multiple = value; }
        }

        #endregion

        public ROSizeConstraintValues(int minimum = Include.UndefinedMinimum,
            int maximum = Include.UndefinedMaximum,
            int multiple = Include.UndefinedMultiple)
        {
            _minimum = minimum;
            _maximum = maximum;
            _multiple = multiple;
        }
    }

    [DataContract(Name = "ROSizeConstraintSize", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintSize
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _size;

        [DataMember(IsRequired = true)]
        ROSizeConstraintValues _constraintValues;

        #region Public Properties

        public KeyValuePair<int, string> Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public ROSizeConstraintValues ConstraintValues
        {
            get { return _constraintValues; }
            set { _constraintValues = value; }
        }

        public bool MinimumSet
        {
            get { return _constraintValues.MinimumSet; }
        }

        public int Minimum
        {
            get { return _constraintValues.Minimum; }
        }

        public bool MaximumSet
        {
            get { return _constraintValues.MaximumSet; }
        }

        public int Maximum
        {
            get { return _constraintValues.Maximum; }
        }

        public bool MultipleSet
        {
            get { return _constraintValues.MultipleSet; }
        }

        public int Multiple
        {
            get { return _constraintValues.Multiple; }
        }

        #endregion

        public ROSizeConstraintSize(KeyValuePair<int, string> size,
            int minimum = Include.UndefinedMinimum,
            int maximum = Include.UndefinedMaximum,
            int multiple = Include.UndefinedMultiple)
        {
            _size = size;
            _constraintValues = new ROSizeConstraintValues(minimum: minimum, maximum: maximum, multiple: multiple);
        }
    }

    [DataContract(Name = "ROSizeConstraintDimension", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintDimension
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dimension;

        [DataMember(IsRequired = true)]
        ROSizeConstraintValues _constraintValues;

        [DataMember(IsRequired = true)]
        List<ROSizeConstraintSize> _sizeConstraintSize;

        #region Public Properties

        public KeyValuePair<int, string> Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        public ROSizeConstraintValues ConstraintValues
        {
            get { return _constraintValues; }
        }

        public bool MinimumSet
        {
            get { return _constraintValues.MinimumSet; }
        }

        public int Minimum
        {
            get { return _constraintValues.Minimum; }
        }

        public bool MaximumSet
        {
            get { return _constraintValues.MaximumSet; }
        }

        public int Maximum
        {
            get { return _constraintValues.Maximum; }
        }

        public bool MultipleSet
        {
            get { return _constraintValues.MultipleSet; }
        }

        public int Multiple
        {
            get { return _constraintValues.Multiple; }
        }

        public List<ROSizeConstraintSize> SizeConstraintSize
        {
            get { return _sizeConstraintSize; }
        }

        #endregion

        public ROSizeConstraintDimension(KeyValuePair<int, string> dimension,
            int minimum = Include.UndefinedMinimum,
            int maximum = Include.UndefinedMaximum,
            int multiple = Include.UndefinedMultiple)
        {
            _dimension = dimension;
            _constraintValues = new ROSizeConstraintValues(minimum: minimum, maximum: maximum, multiple: multiple);
            _sizeConstraintSize = new List<ROSizeConstraintSize>();
        }
    }

    [DataContract(Name = "ROSizeConstraintColor", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintColor
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _color;

        [DataMember(IsRequired = true)]
        ROSizeConstraintValues _constraintValues;

        [DataMember(IsRequired = true)]
        List<ROSizeConstraintDimension> _sizeConstraintDimension;

        #region Public Properties

        public KeyValuePair<int, string> Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public ROSizeConstraintValues ConstraintValues
        {
            get { return _constraintValues; }
        }

        public bool MinimumSet
        {
            get { return _constraintValues.MinimumSet; }
        }

        public int Minimum
        {
            get { return _constraintValues.Minimum; }
        }

        public bool MaximumSet
        {
            get { return _constraintValues.MaximumSet; }
        }

        public int Maximum
        {
            get { return _constraintValues.Maximum; }
        }

        public bool MultipleSet
        {
            get { return _constraintValues.MultipleSet; }
        }

        public int Multiple
        {
            get { return _constraintValues.Multiple; }
        }

        public List<ROSizeConstraintDimension> SizeConstraintDimension
        {
            get { return _sizeConstraintDimension; }
        }

        #endregion

        public ROSizeConstraintColor(KeyValuePair<int, string> color,
            int minimum = Include.UndefinedMinimum,
            int maximum = Include.UndefinedMaximum,
            int multiple = Include.UndefinedMultiple)
        {
            _color = color;
            _constraintValues = new ROSizeConstraintValues(minimum: minimum, maximum: maximum, multiple: multiple);
            _sizeConstraintDimension = new List<ROSizeConstraintDimension>();
        }
    }

    [DataContract(Name = "ROSizeConstraints", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraints
    {
        [DataMember(IsRequired = true)]
        ROSizeConstraintValues _constraintValues;

        [DataMember(IsRequired = true)]
        List<ROSizeConstraintColor> _sizeConstraintColor;

        #region Public Properties

        public ROSizeConstraintValues ConstraintValues
        {
            get { return _constraintValues; }
        }

        public bool MinimumSet
        {
            get { return _constraintValues.MinimumSet; }
        }

        public int Minimum
        {
            get { return _constraintValues.Minimum; }
        }

        public bool MaximumSet
        {
            get { return _constraintValues.MaximumSet; }
        }

        public int Maximum
        {
            get { return _constraintValues.Maximum; }
        }

        public bool MultipleSet
        {
            get { return _constraintValues.MultipleSet; }
        }

        public int Multiple
        {
            get { return _constraintValues.Multiple; }
        }

        public List<ROSizeConstraintColor> SizeConstraintColor
        {
            get { return _sizeConstraintColor; }
        }

        #endregion

        public ROSizeConstraints(int minimum = Include.UndefinedMinimum,
            int maximum = Include.UndefinedMaximum,
            int multiple = Include.UndefinedMultiple)
        {
            _constraintValues = new ROSizeConstraintValues(minimum: minimum, maximum: maximum, multiple: multiple);
            _sizeConstraintColor = new List<ROSizeConstraintColor>();
        }
    }

    [DataContract(Name = "ROSizeConstraintAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        ROSizeConstraints _sizeConstraints;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public ROSizeConstraints SizeConstraints
        {
            get { return _sizeConstraints; }
        }

        public bool MinimumSet
        {
            get { return _sizeConstraints.MinimumSet; }
        }

        public int Minimum
        {
            get { return _sizeConstraints.Minimum; }
        }

        public bool MaximumSet
        {
            get { return _sizeConstraints.MaximumSet; }
        }

        public int Maximum
        {
            get { return _sizeConstraints.Maximum; }
        }

        public bool MultipleSet
        {
            get { return _sizeConstraints.MultipleSet; }
        }

        public int Multiple
        {
            get { return _sizeConstraints.Multiple; }
        }

        #endregion

        public ROSizeConstraintAttributeSet(KeyValuePair<int, string> attributeSet,
            ROSizeConstraints sizeConstraints = null)
        {
            _attributeSet = attributeSet;
            _sizeConstraints = sizeConstraints;
        }
    }

    [DataContract(Name = "ROSizeConstraintDimensionSizes", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeConstraintDimensionSizes
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dimension;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizes;

        #region Public Properties

        public KeyValuePair<int, string> Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        public List<KeyValuePair<int, string>> Sizes
        {
            get { return _sizes; }
        }

        #endregion

        public ROSizeConstraintDimensionSizes(KeyValuePair<int, string> dimension)
        {
            _dimension = dimension;
            _sizes = new List<KeyValuePair<int, string>>();
        }
    }

    [DataContract(Name = "ROModelSizeConstraintProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelSizeConstraintProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeCurveGroup;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;

        [DataMember(IsRequired = true)]
        string _defaultLabel;

        [DataMember(IsRequired = true)]
        ROSizeConstraints _defaultSizeConstraints;

        [DataMember(IsRequired = true)]
        List<ROSizeConstraintAttributeSet> _sizeConstraintAttributeSet;

        [DataMember(IsRequired = true)]
        List<ROSizeConstraintDimensionSizes> _sizeConstraintDimensionSizes;

        #region Public Properties
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public KeyValuePair<int, string> SizeCurveGroup
        {
            get { return _sizeCurveGroup; }
            set { _sizeCurveGroup = value; }
        }

        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        public string DefaultLabel
        {
            get { return _defaultLabel; }
            set { _defaultLabel = value; }
        }

        public ROSizeConstraints DefaultSizeConstraints
        {
            get { return _defaultSizeConstraints; }
            set { _defaultSizeConstraints = value; }
        }

        public List<ROSizeConstraintAttributeSet> SizeConstraintAttributeSet
        {
            get { return _sizeConstraintAttributeSet; }
        }

        public List<ROSizeConstraintDimensionSizes> SizeConstraintDimensionSizes
        {
            get { return _sizeConstraintDimensionSizes; }
        }

        #endregion

        public ROModelSizeConstraintProperties(KeyValuePair<int, string> model,
            KeyValuePair<int, string> attribute,
            KeyValuePair<int, string> sizeCurveGroup,
            KeyValuePair<int, string> sizeGroup)
            : base(eModelType.SizeConstraints, model)

        {
            _attribute = attribute;
            _sizeCurveGroup = sizeCurveGroup;
            _sizeGroup = sizeGroup;
            _sizeConstraintAttributeSet = new List<ROSizeConstraintAttributeSet>();
            _sizeConstraintDimensionSizes = new List<ROSizeConstraintDimensionSizes>();
        }
    }

    [DataContract(Name = "ROModelSizeAlternateProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelSizeAlternateProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _primarySizeCurve;
    
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _alternateSizeCurve;

        [DataMember(IsRequired = true)]
        List<ROSizeAlternatePrimarySet> _sizeAlternatePrimarySet;

    #region Public Properties

    public KeyValuePair<int, string> PrimarySizeCurve
    {
        get { return _primarySizeCurve; }
        set { _primarySizeCurve = value; }
    }

    public KeyValuePair<int, string> AlternateSizeCurve
    {
        get { return _alternateSizeCurve; }
        set { _alternateSizeCurve = value; }
    } 

    public List<ROSizeAlternatePrimarySet> SizeAlternatePrimarySet
        {
        get { return _sizeAlternatePrimarySet; }
    }

    #endregion

    public ROModelSizeAlternateProperties(KeyValuePair<int, string> model,
        KeyValuePair<int, string> primarySizeCurve,
        KeyValuePair<int, string> alternateSizeCurve)
        //int sizeCurveKeyToCopyToDefault = Include.NoRID)
        : base(eModelType.SizeAlternates, model)

    {
        _primarySizeCurve = primarySizeCurve;
        _alternateSizeCurve = alternateSizeCurve;
        _sizeAlternatePrimarySet = new List<ROSizeAlternatePrimarySet>();
    }
}
    [DataContract(Name = "ROSizeAlternatePrimarySet", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeAlternatePrimarySet
    {
        [DataMember(IsRequired = true)]
        int _seq;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _size;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dimension;

        [DataMember(IsRequired = true)]
        ROSizeAlternateSecondarySets _sizeAlternateSecondarySets;

        #region Public Properties

        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }
        }
        public KeyValuePair<int, string> Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public KeyValuePair<int, string> Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }
        public ROSizeAlternateSecondarySets SizeAlternateSecondarySets
        {
            get { return _sizeAlternateSecondarySets; }
            set { _sizeAlternateSecondarySets = value; }
        }

        #endregion

        public ROSizeAlternatePrimarySet(int seq, KeyValuePair<int, string> size, KeyValuePair<int, string> dimension, ROSizeAlternateSecondarySets sizeAlternateSecondarySets)
        {
            _seq = seq;
            _size = size;
            _dimension = dimension;
            _sizeAlternateSecondarySets = sizeAlternateSecondarySets;
        }
    }

    [DataContract(Name = "ROSizeAlternateSecondarySets", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeAlternateSecondarySets
    {
        [DataMember(IsRequired = true)]
        public List<ROSizeAlternateSecondaryValues> _sizeAlternateSecondarySets;


        public List<ROSizeAlternateSecondaryValues> SizeAlternateSecondarySets
        {
            get
            {
                if (_sizeAlternateSecondarySets == null)
                {
                    _sizeAlternateSecondarySets = new List<ROSizeAlternateSecondaryValues>();
                }
                return _sizeAlternateSecondarySets;
            }
            set { _sizeAlternateSecondarySets = value; }
        }

    }

    [DataContract(Name = "ROSizeAlternateSecondaryValues", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeAlternateSecondaryValues
    {
        [DataMember(IsRequired = true)]
        int _seq;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _size;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _dimension;


        #region Public Properties

        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }
        }
        public KeyValuePair<int, string> Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public KeyValuePair<int, string> Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }
        
        #endregion

        public ROSizeAlternateSecondaryValues(int seq, KeyValuePair<int, string> size, KeyValuePair<int, string> dimension)
        {
            _seq = seq;
            _size = size;
            _dimension = dimension;
        }
    }

    [DataContract(Name = "ROModelSizeGroupProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelSizeGroupProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        List<string> _productCategories;

        [DataMember(IsRequired = true)]
        string _productCategory;

        [DataMember(IsRequired = true)]
        string _description;

        [DataMember(IsRequired = true)]
        eSearchContent _verifyCriteria;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _size;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _width;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string>[][] _sizeMatrix;

        [DataMember(IsRequired = true)]
        List<KeyValuePair<int, string>> _sizeSelection;

        [DataMember(IsRequired = true)]
        int _messageRow;

        [DataMember(IsRequired = true)]
        int _messageColumn;

        #region Public Properties
        public List<string> ProductCategories
        {
            get { return _productCategories; }
        }

        public string ProductCategory
        {
            get { return _productCategory; }
        }

        public string Description
        {
            get { return _description; }
        }

        public eSearchContent VerifyCriteria
        {
            get { return _verifyCriteria; }
            set { _verifyCriteria = value; }
        }

        public List<KeyValuePair<int, string>> Size
        {
            get { return _size; }
        }

        public List<KeyValuePair<int, string>> Width
        {
            get { return _width; }
        }

        public KeyValuePair<int, string>[][] SizeMatrix
        {
            get { return _sizeMatrix; }
        }

        public List<KeyValuePair<int, string>> SizeSelection
        {
            get { return _sizeSelection; }
        }

        public bool SizeSelectionIsSet
        {
            get { return _sizeSelection.Count > 0; }
        }

        public int MessageRow
        {
            get { return _messageRow; }
            set { _messageRow = value; }
        }

        public bool MessageRowIsSet
        {
            get { return _messageRow != Include.Undefined; }
        }

        public int MessageColumn
        {
            get { return _messageColumn; }
            set { _messageColumn = value; }
        }

        public bool MessageColumnIsSet
        {
            get { return _messageColumn != Include.Undefined; }
        }

        #endregion

        public ROModelSizeGroupProperties(KeyValuePair<int, string> model,
            string productCategory,
            string description) :
            base(eModelType.SizeGroup, model)

        {
            _productCategory = productCategory;
            _description = description;
            _productCategories = new List<string>();
            _verifyCriteria = eSearchContent.WholeField;
            _size = new List<KeyValuePair<int, string>>();
            _width = new List<KeyValuePair<int, string>>();
            _sizeSelection = new List<KeyValuePair<int, string>>();
            _messageRow = Include.Undefined;
            _messageColumn = Include.Undefined;
        }

        public void DefineSizeMatrix(int numberOfRows, int numberOfColumns)
        {
            if (_width.Count > numberOfRows)
            {
                numberOfRows = _width.Count;
            }

            if (_size.Count > numberOfColumns)
            {
                numberOfColumns = _size.Count;
            }

            _sizeMatrix = new KeyValuePair<int, string>[numberOfRows][];
            for (int row = 0; row < numberOfRows; row++)
            {
                _sizeMatrix[row] = new KeyValuePair<int, string>[numberOfColumns];
            }
        }
    }

    [DataContract(Name = "ROSizeCurveEntry", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveEntry
    {
        [DataMember(IsRequired = true)]
        int _sizeKey;

        [DataMember(IsRequired = true)]
        decimal _percent;

        #region Public Properties

        public int SizeKey
        {
            get { return _sizeKey; }
            set { _sizeKey = value; }
        }

        public decimal Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        #endregion

        public ROSizeCurveEntry(int sizeKey,
            decimal percent)

        {
            _sizeKey = sizeKey;
            _percent = percent;
        }
    }

    [DataContract(Name = "ROSizeCurve", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurve
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeCurve;

        [DataMember(IsRequired = true)]
        ROSizeCurveEntry[][] _sizeCurveEntry;

        #region Public Properties

        public KeyValuePair<int, string> SizeCurve
        {
            get { return _sizeCurve; }
            set { _sizeCurve = value; }
        }

        public ROSizeCurveEntry[][] SizeCurveEntry
        {
            get { return _sizeCurveEntry; }
            set { _sizeCurveEntry = value; }
        }

        #endregion

        public ROSizeCurve(KeyValuePair<int, string> sizeCurve)
        {
            _sizeCurve = sizeCurve;
        }
    }

    [DataContract(Name = "ROSizeCurveStore", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveStore
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _store;

        [DataMember(IsRequired = true)]
        ROSizeCurve _sizeCurve;

        #region Public Properties

        public KeyValuePair<int, string> Store
        {
            get { return _store; }
            set { _store = value; }
        }

        public ROSizeCurve SizeCurve
        {
            get { return _sizeCurve; }
            set { _sizeCurve = value; }
        }

        #endregion

        public ROSizeCurveStore(KeyValuePair<int, string> store)
        {
            _store = store;
        }
    }

    [DataContract(Name = "ROSizeCurveAttributeSet", Namespace = "http://Logility.ROWeb/")]
    public class ROSizeCurveAttributeSet
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attributeSet;

        [DataMember(IsRequired = true)]
        List<ROSizeCurveStore> _storeSizeCurve;

        #region Public Properties

        public KeyValuePair<int, string> AttributeSet
        {
            get { return _attributeSet; }
            set { _attributeSet = value; }
        }

        public List<ROSizeCurveStore> StoreSizeCurve
        {
            get { return _storeSizeCurve; }
            set { _storeSizeCurve = value; }
        }

        #endregion

        public ROSizeCurveAttributeSet(KeyValuePair<int, string> attributeSet)
        {
            _attributeSet = attributeSet;
            _storeSizeCurve = new List<ROSizeCurveStore>();
        }
    }

    [DataContract(Name = "ROModelSizeCurveProperties", Namespace = "http://Logility.ROWeb/")]
    public class ROModelSizeCurveProperties : ROModelProperties
    {
        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _attribute;

        [DataMember(IsRequired = true)]
        KeyValuePair<int, string> _sizeGroup;

        [DataMember(IsRequired = true)]
        string _defaultLabel;

        [DataMember(IsRequired = true)]
        ROSizeCurve _defaultSizeCurve;

        [DataMember(IsRequired = true)]
        List<ROSizeCurveAttributeSet> _attributeSetSizeCurve;

        [DataMember(IsRequired = true)]
        Dictionary<int, ROSize> _sizeCurveSizes;

        [DataMember(IsRequired = true)]
        int _sizeCurveKeyToCopyToDefault;

        #region Public Properties
        public KeyValuePair<int, string> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        public KeyValuePair<int, string> SizeGroup
        {
            get { return _sizeGroup; }
            set { _sizeGroup = value; }
        }

        public string DefaultLabel
        {
            get { return _defaultLabel; }
            set { _defaultLabel = value; }
        }

        public ROSizeCurve DefaultSizeCurve
        {
            get { return _defaultSizeCurve; }
            set { _defaultSizeCurve = value; }
        }

        public List<ROSizeCurveAttributeSet> AttributeSetSizeCurve
        {
            get { return _attributeSetSizeCurve; }
        }

        public Dictionary<int, ROSize> SizeCurveSizes
        {
            get { return _sizeCurveSizes; }
        }

        public int SizeCurveKeyToCopyToDefault
        {
            get { return _sizeCurveKeyToCopyToDefault; }
            set { _sizeCurveKeyToCopyToDefault = value; }
        }

        #endregion

        public ROModelSizeCurveProperties(KeyValuePair<int, string> model,
            KeyValuePair<int, string> attribute,
            KeyValuePair<int, string> sizeGroup,
            int sizeCurveKeyToCopyToDefault = Include.NoRID)
            : base(eModelType.SizeCurve, model)

        {
            _attribute = attribute;
            _sizeGroup = sizeGroup;
            _attributeSetSizeCurve = new List<ROSizeCurveAttributeSet>();
            _sizeCurveSizes = new Dictionary<int, ROSize>();
            _sizeCurveKeyToCopyToDefault = sizeCurveKeyToCopyToDefault;
        }
    }

}