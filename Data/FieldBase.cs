using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    //Used to provide common support for validation and isolate business logic from presentation
    //Definitions are used so they can be static, to reduce memory usage if there are like 100,000 objects in an array or list

    #region "Field Definitions"

    /// <summary>
    /// Field Definitions are passed into fields, for easy reference and to keep them static
    /// </summary>
    public abstract class FieldDefinition
    {
        public string DatabaseName;
        public string Description;
        public Type FieldType;
        public bool IsRequired;
        public FieldDefinition(string DatabaseName, string Description, bool IsRequired)
        {
            this.DatabaseName = DatabaseName;
            this.Description = Description;
            this.IsRequired = IsRequired;

        }
        public event BeforePropertyChangedEventHandler BeforePropertyChangedEvent;
        public event AfterPropertyChangedEventHandler AfterPropertyChangedEvent;
        public virtual void RaiseBeforePropertyChanged(object aOldValue, object aNewValue)
        {
            if (BeforePropertyChangedEvent != null)
                BeforePropertyChangedEvent(this, new BeforePropertyChangedEventArgs(aOldValue, aNewValue));
        }
        public virtual void RaiseAfterPropertyChanged(object aOldValue, object aNewValue)
        {
            if (AfterPropertyChangedEvent != null)
                AfterPropertyChangedEvent(this, new AfterPropertyChangedEventArgs(aOldValue, aNewValue));
        }
    }

    public class BoolFieldDefinition : FieldDefinition
    {
        public bool? defaultValue;
        public BoolFieldDefinition(string DatabaseName, string Description, bool IsRequired, bool? DefaultValue = null)
            : base(DatabaseName, Description, IsRequired)
        {
            this.defaultValue = DefaultValue;
        }
    }
    public class StringFieldDefinition : FieldDefinition
    {
        public int MaxLength;
        public bool AllowEmptyString;
        public string DefaultValue;
        public StringFieldDefinition(string DatabaseName, string Description, bool IsRequired, int MaxLength, bool AllowEmptyString, string DefaultValue = null)
            : base(DatabaseName, Description, IsRequired)
        {
            this.MaxLength = MaxLength;
            this.AllowEmptyString = AllowEmptyString;
            this.DefaultValue = DefaultValue;
        }
    }
    public class IntFieldDefinition : FieldDefinition
    {
        public bool AllowZero;
        public bool AllowNegative;
        public int? DefaultValue;
        public int? MinValue; //does not replace allowZero or allowNegative, but is an additional check
        public int? MaxValue; //does not replace allowZero or allowNegative, but is an additional check
        public IntFieldDefinition(string DatabaseName, string Description, bool IsRequired, bool AllowZero, bool AllowNegative, int? DefaultValue = null, int? MinValue = null, int? MaxValue = null)
            : base(DatabaseName, Description, IsRequired)
        {
            this.AllowZero = AllowZero;
            this.AllowNegative = AllowNegative;
            this.DefaultValue = DefaultValue;
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
        }
    }
    #endregion

    #region "Base Fields"

    /// <summary>
    /// Used for all field types: int, string, bool, etc
    /// Making a field class for each type so values can be type safe
    /// </summary>
    public abstract class baseField
    {

        public abstract FieldDefinition Definition
        {
            get;
        }

        public abstract void SetValueFromDataRow(DataRow dr);
        public abstract void UpdateDataRowWithValue(DataRow dr);
        public abstract MIDDbParameter MakeSQLParameter();

        public virtual string MakeSQLUpdateCommand()
        {


            string updateCommand = Definition.DatabaseName + " = @" + Definition.DatabaseName + ", ";
            return updateCommand;
        }
        public bool HasChanged = false;
        //public bool hasValue = false;

        public string InvalidMessage;
        public string InvalidTitle;
        public bool HasInvalidMessage = false;

        public void ResetState()
        {
            HasChanged = false;
            InvalidMessage = string.Empty;
            InvalidTitle = string.Empty;
            HasInvalidMessage = false;
        }
        public abstract bool HasRequiredValue();

    }

    /// <summary>
    /// This implementation of the bool field is stored as char(1) in the database
    ///     0=false
    ///     1=true
    /// </summary>
    public class boolField : baseField
    {
        private bool _Value;
        public bool Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// attempts to set the value to the default value, if a default value was provided
        /// </summary>
        /// <returns>returns true if value has been set to the default, otherwise returns false</returns>
        //public bool SetToDefaultValueWithValidation()
        //{

        //    if (_definition.defaultValue == null)
        //    {
        //        base.invalidMessage = definition.description + ": " + fieldMessages.DEFAULT_VALUE_NOT_PROVIDED;
        //        base.invalidTitle = fieldMessages.DEFAULT_VALUE_NOT_PROVIDED_TITLE;
        //        base.hasInvalidMessage = true;
        //        return false;
        //    }
        //    else
        //    {
        //        bool newValue;
        //        if (_definition.defaultValue == false)
        //            newValue = false;
        //        else
        //        {
        //            newValue = true;
        //        }

        //        return SetValue(newValue);
        //    }
        //}

        private void ResetToDefault()
        {
            if (_definition.defaultValue != null)
            {
                if (_definition.defaultValue == false)
                    SetValue(false);
                else
                {
                    SetValue(true);
                }
            }
        }

        public boolField(BoolFieldDefinition def, List<baseField> fieldList = null)
        {
            def.FieldType = typeof(bool);
            _definition = def;

            ResetToDefault();

            if (fieldList != null)
            {
                fieldList.Add(this);
            }
        }
        private BoolFieldDefinition _definition;
        public override FieldDefinition Definition
        {
            get { return _definition; }
        }


        public override void SetValueFromDataRow(DataRow dr)
        {
            if (dr[Definition.DatabaseName] != DBNull.Value)
            {
                if ((string)dr[Definition.DatabaseName] == "0")
                {
                    SetValue(false);
                }
                else
                {
                    SetValue(true);
                }

            }
            else
            {
                //hasValue = false;
                ResetToDefault();
            }
            ResetState();
        }
        public override void UpdateDataRowWithValue(DataRow dr)
        {
            dr[Definition.DatabaseName] = _Value;
        }
        public bool IsNewValueValid(bool newValue)
        {
            //if (hasValue == false && definition.isRequired == true)
            //{
            //    base.invalidMessage = definition.description + " " + fieldMessages.IS_REQUIRED;
            //    base.invalidTitle = fieldMessages.IS_REQUIRED_TITLE;
            //    base.hasInvalidMessage = true;
            //    return false;
            //}

            base.InvalidMessage = string.Empty;
            base.InvalidTitle = string.Empty;
            base.HasInvalidMessage = false;
            return true;

        }
        public virtual bool SetValue(bool newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;
                //hasValue = true;
                HasChanged = true;

                Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public override bool HasRequiredValue()
        {
            bool hasRequiredValue;
            if (Definition.IsRequired == true && HasChanged == false)
            {
                InvalidMessage = Definition.Description + " " + fieldMessages.IS_REQUIRED;
                InvalidTitle = fieldMessages.IS_REQUIRED_TITLE;
                HasInvalidMessage = true;
                hasRequiredValue = false;
            }
            else
            {
                hasRequiredValue = true;
            }
            return hasRequiredValue;
        }

        public override MIDDbParameter MakeSQLParameter()
        {
            string tempValue;
            if (_Value == false)
            {
                tempValue = "0";
            }
            else
            {
                tempValue = "1";
            }
            return new MIDDbParameter("@" + Definition.DatabaseName, tempValue, eDbType.Char);
        }

    }

    /// <summary>
    /// When using isRequired, the HasRequiredValue function will produce false, regardless of the allowEmpty flag in the definition
    /// </summary>
    public class stringField : baseField
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }

        private void ResetToDefault()
        {
            if (_definition.DefaultValue != null)
            {
                SetValue(_definition.DefaultValue);
            }
        }

        public stringField(StringFieldDefinition def, List<baseField> fieldList = null)
        {
            def.FieldType = typeof(string);
            _definition = def;

            ResetToDefault();


            if (fieldList != null)
            {
                fieldList.Add(this);
            }
        }
        private StringFieldDefinition _definition;
        public override FieldDefinition Definition
        {
            get { return _definition; }
        }
        public override void SetValueFromDataRow(DataRow dr)
        {
            if (dr[Definition.DatabaseName] != DBNull.Value)
            {
                SetValue((string)dr[Definition.DatabaseName]);

            }
            else
            {
                //hasValue = false;
                ResetToDefault();
            }
            ResetState();
        }
        public override void UpdateDataRowWithValue(DataRow dr)
        {
            dr[Definition.DatabaseName] = _Value;
        }
        public bool IsNewValueValid(string newValue)
        {
            //if (hasValue == false && definition.isRequired == true)
            //{
            //    base.invalidMessage = definition.description + " " + fieldMessages.IS_REQUIRED;
            //    base.invalidTitle = fieldMessages.IS_REQUIRED_TITLE;
            //    base.hasInvalidMessage = true;
            //    return false;
            //}

            if (newValue.Length > _definition.MaxLength)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.STRING_MAX_LENGTH_EXCEEDED.Replace("{0}", _definition.MaxLength.ToString());
                base.InvalidTitle = fieldMessages.STRING_MAX_LENGTH_EXCEEDED_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }

            if (_definition.AllowEmptyString == false && newValue == String.Empty)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.STRING_EMPTY;
                base.InvalidTitle = fieldMessages.STRING_EMPTY_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }

            base.InvalidMessage = string.Empty;
            base.InvalidTitle = string.Empty;
            base.HasInvalidMessage = false;
            return true;

        }
        public virtual bool SetValue(string newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;
                //hasValue = true;
                HasChanged = true;

                Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;
        }
        public override bool HasRequiredValue()
        {
            bool hasRequiredValue;
            if (Definition.IsRequired == true && (_Value == null || _Value == String.Empty))
            {
                InvalidMessage = Definition.Description + " " + fieldMessages.IS_REQUIRED;
                InvalidTitle = fieldMessages.IS_REQUIRED_TITLE;
                HasInvalidMessage = true;
                hasRequiredValue = false;
            }
            else
            {
                hasRequiredValue = true;
            }
            return hasRequiredValue;
        }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter("@" + Definition.DatabaseName, _Value, eDbType.VarChar);
        }

    }

    /// <summary>
    /// Int field class
    /// </summary>
    public class intField : baseField
    {
        private int _Value;
        public int Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }

        private void ResetToDefault()
        {
            if (_definition.DefaultValue != null)
            {
                SetValue(_definition.DefaultValue.Value);
            }
        }
        public intField(IntFieldDefinition def, List<baseField> fieldList = null)
        {
            def.FieldType = typeof(int);
            _definition = def;

            ResetToDefault();


            if (fieldList != null)
            {
                fieldList.Add(this);
            }
        }
        private IntFieldDefinition _definition;
        public override FieldDefinition Definition
        {
            get { return _definition; }
        }
        public override void SetValueFromDataRow(DataRow dr)
        {
            if (dr[Definition.DatabaseName] != DBNull.Value)
            {
                SetValue((int)dr[Definition.DatabaseName]);

            }
            else
            {
                //hasValue = false;
                ResetToDefault();

            }
            ResetState();
        }
        public override void UpdateDataRowWithValue(DataRow dr)
        {
            dr[Definition.DatabaseName] = _Value;
        }

        public bool IsNewValueValid(string newValue)
        {
            if (newValue == null || newValue == String.Empty)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_REQUIRED;
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }
            int tempValue;
            if (int.TryParse(newValue, out tempValue) == false)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_INVALID_NUMBER;
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }

            return IsNewValueValid(tempValue);
        }
        public bool IsNewValueValid(int newValue)
        {
            //if (hasValue == false && definition.isRequired == true)
            //{
            //    base.invalidMessage = definition.description + " " + fieldMessages.IS_REQUIRED;
            //    base.invalidTitle = fieldMessages.IS_REQUIRED_TITLE;
            //    base.hasInvalidMessage = true;
            //    return false;
            //}

            if (_definition.AllowZero == false && newValue == 0)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_INVALID_NUMBER_ZERO;
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_ZERO_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }
            if (_definition.AllowNegative == false && newValue < 0)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_INVALID_NUMBER_NEGATIVE;
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_NEGATIVE_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }

            if (_definition.MinValue.HasValue == true && newValue < _definition.MinValue)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_INVALID_NUMBER_LESS_THAN_MINIMUM.Replace("{0}", _definition.MinValue.ToString());
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_LESS_THAN_MINIMUM_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }

            if (_definition.MaxValue.HasValue == true && newValue > _definition.MaxValue)
            {
                base.InvalidMessage = Definition.Description + " " + fieldMessages.IS_INVALID_NUMBER_LESS_THAN_MAXIMUM.Replace("{0}", _definition.MaxValue.ToString());
                base.InvalidTitle = fieldMessages.IS_INVALID_NUMBER_LESS_THAN_MAXIMUM_TITLE;
                base.HasInvalidMessage = true;
                return false;
            }
           

            base.InvalidMessage = string.Empty;
            base.InvalidTitle = string.Empty;
            base.HasInvalidMessage = false;
            return true;

        }

        public bool SetValue(string newValue)
        {


            if (IsNewValueValid(newValue) == true)
            {
                int tempValue;
                int.TryParse(newValue, out tempValue);
                return SetValue(tempValue);
            }
            else
            {
                return false;
            }
        }
        public virtual bool SetValue(int newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;
                //hasValue = true;
                HasChanged = true;

                Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;
        }
        public override bool HasRequiredValue()
        {
            bool hasRequiredValue;
            if (Definition.IsRequired == true && HasChanged == false)
            {
                InvalidMessage = Definition.Description + " " + fieldMessages.IS_REQUIRED;
                InvalidTitle = fieldMessages.IS_REQUIRED_TITLE;
                HasInvalidMessage = true;
                hasRequiredValue = false;
            }
            else
            {
                hasRequiredValue = true;
            }
            return hasRequiredValue;
        }



        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter("@" + Definition.DatabaseName, _Value, eDbType.Int);
        }

    }

    #endregion

    #region "Base Field Business Layer"
    /// <summary>
    /// Allows common functions to be written for all business layer classes
    /// </summary>
    public abstract class baseBL
    {
        public List<baseField> Fields = new List<baseField>();

        /// <summary>
        /// Loads values from a datarow into the fields
        /// </summary>
        /// <param name="dr"></param>
        public void LoadFromDataRow(DataRow dr)
        {
            foreach (baseField f in Fields)
            {
                f.SetValueFromDataRow(dr);
            }
        }

        /// <summary>
        /// Appends to a SQL update string for use when saving
        /// Replaces: updateCommand += "COMPANY_FAX = @COMPANY_FAX, ";
        /// </summary>
        /// <returns></returns>
        public string MakeSQLUpdateCommand()
        {
            string s = String.Empty;
            foreach (baseField f in Fields)
            {
                s += f.MakeSQLUpdateCommand();
            }
            return s;

        }

        /// <summary>
        /// Creates parameters for fields for use when saving
        /// The parameter name will equal the sql_name of the field prefixed with an @ symbol
        /// Replaces: InParamList.Add(new MIDDbParameter("@COMPANY_TELEPHONE", companyPhone, eDbType.VarChar ));
        /// </summary>
        /// <param name="InParamList"></param>
        public void MakeSQLParameterList(System.Collections.ArrayList InParamList)
        {
            foreach (baseField f in Fields)
            {
                InParamList.Add(f.MakeSQLParameter());
            }
        }
    }
    #endregion

    #region "Text Messages for Fields"

    public struct fieldMessages
    {
        public static string IS_REQUIRED = " is required."; //MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_SendingEmailIssue);
        public static string IS_REQUIRED_TITLE = "Missing Field"; //MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_SendingEmailIssue);
        public static string IS_INVALID_NUMBER = " is invalid.";
        public static string IS_INVALID_NUMBER_TITLE = "Invalid Number";
        public static string IS_INVALID_NUMBER_ZERO = " cannot be zero.";
        public static string IS_INVALID_NUMBER_ZERO_TITLE = "Invalid Number";
        public static string IS_INVALID_NUMBER_NEGATIVE = " cannot be less than zero.";
        public static string IS_INVALID_NUMBER_NEGATIVE_TITLE = "Invalid Number";
        public static string IS_INVALID_NUMBER_LESS_THAN_MINIMUM = " cannot be less than {0}";
        public static string IS_INVALID_NUMBER_LESS_THAN_MINIMUM_TITLE = "Invalid Number";
        public static string IS_INVALID_NUMBER_LESS_THAN_MAXIMUM = " cannot be more than {0}";
        public static string IS_INVALID_NUMBER_LESS_THAN_MAXIMUM_TITLE = "Invalid Number";
        public static string STRING_MAX_LENGTH_EXCEEDED = " has exceded the max length of {0}.";
        public static string STRING_MAX_LENGTH_EXCEEDED_TITLE = "Invalid Number";
        public static string STRING_EMPTY = " is required.";
        public static string STRING_EMPTY_TITLE = "Missing Field";
        public static string DEFAULT_VALUE_NOT_PROVIDED = "No default value was specified.";
        public static string DEFAULT_VALUE_NOT_PROVIDED_TITLE = "Missing Default Value";
    }
    #endregion

    #region "Field Events"
    public class BeforePropertyChangedEventArgs
        {
            public BeforePropertyChangedEventArgs(object aOldValue, object aNewValue) { oldValue = aOldValue; newValue = aNewValue; }
            public object oldValue { get; private set; } // readonly
            public object newValue { get; private set; } // readonly
        }
        public class AfterPropertyChangedEventArgs
        {
            public AfterPropertyChangedEventArgs(object aOldValue, object aNewValue) { oldValue = aOldValue; newValue = aNewValue; }
            public object oldValue { get; private set; } // readonly
            public object newValue { get; private set; } // readonly
        }
        public delegate void BeforePropertyChangedEventHandler(object sender, BeforePropertyChangedEventArgs e);
        public delegate void AfterPropertyChangedEventHandler(object sender, AfterPropertyChangedEventArgs e);
    #endregion

}
