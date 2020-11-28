using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
   
    public enum storedProcedureTypes
    {
        Read,
        ReadAsDataset,
        Insert,
        InsertAndReturnRID,
        Update,
        UpdateWithReturnCode,
        Delete,
        RecordCount,
        ScalarValue,
        Maintenance,
        OutputOnly
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class UnitTestMethodAttribute : System.Attribute
    {
        /// <summary>
        /// Used for unit testing stored procedures that update rows.
        /// This select statement will be executed before an update to compare results.
        /// The results of the select statement will be used to reset rows after an update is performed.
        /// </summary>
        public string SelectStatement { get; set; }
        //public string DB { get; set; }
        //public storedProcedureTestKinds testKinds { get; set; }
        //public int expectedCount { get; set; }
        //public string scalarValue { get; set; }
        public bool BypassValidation = false;
        public string Notes { get; set; }
        public UnitTestMethodAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class UnitTestParameterAttribute : System.Attribute
    {
        public string DefaultValue { get; set; }
        public string DB { get; set; }
        public UnitTestParameterAttribute()
        {
        }
    }


    public class baseStoredProcedure
    {
        public string procedureName;
        public List<baseParameter> inputParameterList = new List<baseParameter>();
        public List<baseParameter> outputParameterList = new List<baseParameter>();
        public storedProcedureTypes procedureType;
        public List<string> tableNames = new List<string>();

        private int commandTimeout = -1;

        /// <summary>
        /// The time in seconds to wait for a command to execute.  Default is 30 seconds.
        /// </summary>
        /// <param name="commandTimeOut"></param>
        protected void SetCommandTimeout(int commandTimeOut)
        {
            this.commandTimeout = commandTimeOut;
        }

        protected DataTable ExecuteStoredProcedureForRead(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            DataTable dt = dba.ExecuteStoredProcedureForRead(procedureName, InputParameters, outParams, commandTimeout);
            SetValuesOnOutputParameters(outParams);
            return dt;
        }
        protected DataTable ExecuteStoredProcedureForInsertAndRead(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            DataTable dt = dba.ExecuteStoredProcedureForInsertAndRead(procedureName, InputParameters, outParams, commandTimeout);
            SetValuesOnOutputParameters(outParams);
            return dt;
        }
        protected DataSet ExecuteStoredProcedureForReadAsDataSet(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            DataSet ds = dba.ExecuteStoredProcedureForReadAsDataSet(procedureName, InputParameters, outParams, commandTimeout);
            SetValuesOnOutputParameters(outParams);
            return ds;
        }
        protected int ExecuteStoredProcedureForInsertAndReturnRID(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            int rid = dba.ExecuteStoredProcedureForInsertAndReturnRID(procedureName, InputParameters, outParams);
            SetValuesOnOutputParameters(outParams);
            return rid;
        }
        protected int ExecuteStoredProcedureForInsert(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            int rowsInserted = dba.ExecuteStoredProcedureForInsert(procedureName, InputParameters, outParams);
            SetValuesOnOutputParameters(outParams);
            return rowsInserted;
        }
        protected int ExecuteStoredProcedureForUpdate(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            int rowsUpdated = dba.ExecuteStoredProcedureForUpdate(procedureName, InputParameters, outParams, commandTimeout);
            SetValuesOnOutputParameters(outParams);
            return rowsUpdated;
        }
        protected int ExecuteStoredProcedureForDelete(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            int rowsDeleted = dba.ExecuteStoredProcedureForDelete(procedureName, InputParameters, outParams);
            SetValuesOnOutputParameters(outParams);
            return rowsDeleted;
        }
        protected int ExecuteStoredProcedureForRecordCount(DatabaseAccess dba)
        {
            return dba.ExecuteStoredProcedureForRecordCount(procedureName, InputParameters);
        }
        protected object ExecuteStoredProcedureForScalarValue(DatabaseAccess dba)
        {
            return dba.ExecuteStoredProcedureForScalarValue(procedureName, InputParameters);
        }
        //protected int ExecuteStoredProcedureForUpdateWithReturnCode(DatabaseAccess dba)
        //{
        //    return dba.ExecuteStoredProcedureForUpdateWithReturnCode(procedureName, InputParameters, OutputParameters);
        //}
        protected void ExecuteStoredProcedureForMaintenance(DatabaseAccess dba)
        {
            dba.ExecuteStoredProcedureForMaintenance(procedureName, InputParameters, commandTimeout);
        }
        //protected void ExecuteStoredProcedureForMaintenanceWithTimeOut(DatabaseAccess dba)
        //{
        //    dba.ExecuteStoredProcedureForMaintenance(procedureName, InputParameters, commandTimeout);
        //}
        protected void ExecuteStoredProcedureForOutputParameters(DatabaseAccess dba)
        {
            MIDDbParameter[] outParams = OutputParameters;
            //dba.ReadOnlyStoredProcedure(procedureName, InputParameters, outParams);
            //SetValuesOnOutputParameters(outParams);
            DataTable dt = dba.ExecuteStoredProcedureForRead(procedureName, InputParameters, outParams, commandTimeout);
            SetValuesOnOutputParameters(outParams);
        }
        private void SetValuesOnOutputParameters(MIDDbParameter[] outParams)
        {
            int i = 0;
            foreach (baseParameter param in outputParameterList)
            {
                if (param.DBType == MIDRetail.DataCommon.eDbType.Int)
                {
                    MIDRetail.Data.intParameter p = (MIDRetail.Data.intParameter)param;
                    p.SetValue(Convert.ToInt32(outParams[i].Value));
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.Float)
                {
                    MIDRetail.Data.floatParameter p = (MIDRetail.Data.floatParameter)param;
                    p.SetValue(Convert.ToDouble(outParams[i].Value));
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.VarChar)
                {
                    MIDRetail.Data.stringParameter p = (MIDRetail.Data.stringParameter)param;
                    p.SetValue(Convert.ToString(outParams[i].Value));
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.Char)
                {
                    MIDRetail.Data.charParameter p = (MIDRetail.Data.charParameter)param;
                    p.SetValue(Convert.ToChar(outParams[i].Value));
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.DateTime)
                {
                    MIDRetail.Data.datetimeParameter p = (MIDRetail.Data.datetimeParameter)param;
                    p.SetValue(Convert.ToDateTime(outParams[i].Value));
                }
                i++;
            }
        }
        public DataSet GetOutputParametersAsDataSet()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add("OutputParameters");
            ds.Tables[0].Columns.Add("parameterName");
            ds.Tables[0].Columns.Add("parameterValue");

            foreach (baseParameter param in outputParameterList)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["parameterName"] = param.parameterName;
                if (param.DBType == MIDRetail.DataCommon.eDbType.Int)
                {
                    MIDRetail.Data.intParameter p = (MIDRetail.Data.intParameter)param;
                    dr["parameterValue"] = p.Value.ToString();
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.Float)
                {
                    MIDRetail.Data.floatParameter p = (MIDRetail.Data.floatParameter)param;
                    dr["parameterValue"] = p.Value.ToString();
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.VarChar)
                {
                    MIDRetail.Data.stringParameter p = (MIDRetail.Data.stringParameter)param;
                    dr["parameterValue"] = p.Value.ToString();
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.Char)
                {
                    MIDRetail.Data.charParameter p = (MIDRetail.Data.charParameter)param;
                    dr["parameterValue"] = p.Value.ToString();
                }
                else if (param.DBType == MIDRetail.DataCommon.eDbType.DateTime)
                {
                    MIDRetail.Data.datetimeParameter p = (MIDRetail.Data.datetimeParameter)param;
                    dr["parameterValue"] = p.Value.ToString();
                }
                
                ds.Tables[0].Rows.Add(dr);
            }
            return ds;
        }
      

        protected MIDDbParameter[] InputParameters
        {
            get { return GetInputParameters(); }
        }
        private MIDDbParameter[] GetInputParameters()
        {
            if (inputParameterList.Count == 0)
            {
                return null;
            }
            else
            {
                MIDDbParameter[] InParams = new MIDDbParameter[inputParameterList.Count];
                int i = 0;
                foreach (baseParameter p in inputParameterList)
                {
                    InParams[i] = p.MakeSQLParameter();
                    i++;
                }
                return InParams;
            }
        }
        public MIDDbParameter[] OutputParameters
        {
            get { return GetOutputParameters(); }
        }
        private MIDDbParameter[] GetOutputParameters()
        {
            if (outputParameterList.Count == 0)
            {
                return null;
            }
            else
            {
                MIDDbParameter[] OutParams = new MIDDbParameter[outputParameterList.Count];
                int i = 0;
                foreach (baseParameter p in outputParameterList)
                {
                    OutParams[i] = p.MakeSQLParameter();
                    i++;
                }
                return OutParams;
            }
        }
    }

    public abstract class baseParameter
    {
        public string parameterName;
        public eDbType DBType;
        public Type parameterType;
        public abstract MIDDbParameter MakeSQLParameter();
    }

    public class intParameter : baseParameter
    {
        public intParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Int;
            base.parameterType = typeof(int);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(intParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Int);
        }


        private int? _Value;
        public int? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(int? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;
            
                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(int? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }
        
    }
    public class longParameter : baseParameter
    {
        public longParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Int64;
            base.parameterType = typeof(long);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(longParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Int64);
        }


        private long? _Value;
        public long? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(long? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(long? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class floatParameter : baseParameter
    {
        public floatParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Float;
            base.parameterType = typeof(double);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(floatParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Float);
        }


        private double? _Value;
        public double? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(double? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(double? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }
    public class decimalParameter : baseParameter
    {
        public decimalParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Decimal;
            base.parameterType = typeof(decimal);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(decimalParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Float);
        }


        private decimal? _Value;
        public decimal? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(decimal? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(decimal? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class stringParameter : baseParameter
    {
        public stringParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.VarChar;
            base.parameterType = typeof(string);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(stringParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            if (_Value == Include.NullForStringValue) //TT#1310-MD -jsobek -Error when adding a new Store
            {
                return new MIDDbParameter(parameterName, null, eDbType.VarChar);
            }
            else
            {
                return new MIDDbParameter(parameterName, _Value, eDbType.VarChar);
            }
        }


        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(string newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(string newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    /// <summary>
    /// text and ntext are being depreciated - needs to eventually replaced on the tables with varchar(max)
    /// http://technet.microsoft.com/en-us/library/ms187993.aspx
    /// </summary>
    public class textParameter : baseParameter
    {
        public textParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Text;
            base.parameterType = typeof(string);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(textParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            if (_Value == "null")
            {
                return new MIDDbParameter(parameterName, null, eDbType.VarChar);
            }
            else
            {
                return new MIDDbParameter(parameterName, _Value, eDbType.VarChar);
            }
        }


        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(string newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(string newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class byteArrayParameter : baseParameter
    {
        public byteArrayParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Image;
            base.parameterType = typeof(byte[]);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(byteArrayParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            if (_Value == null)
            {
                return new MIDDbParameter(parameterName, null, eDbType.Image);
            }
            else
            {
                return new MIDDbParameter(parameterName, _Value, eDbType.Image);
            }
        }


        private byte[] _Value;
        public byte[] Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(byte[] newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(byte[] newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class charParameter : baseParameter
    {
        public charParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Char;
            base.parameterType = typeof(char);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(charParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Char);
        }


        private char? _Value;
        public char? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(char? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(char? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class datetimeParameter : baseParameter
    {
        public datetimeParameter(string parameterName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.DateTime;
            base.parameterType = typeof(DateTime);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(datetimeParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.DateTime);
        }


        private DateTime? _Value;
        public DateTime? Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(DateTime? newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(DateTime? newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }

    public class tableParameter : baseParameter
    {
        public tableParameter(string parameterName, string typeName, List<baseParameter> parameterList = null)
        {
            base.parameterName = parameterName;
            base.DBType = eDbType.Structured;
            this.typeName = typeName;
            base.parameterType = typeof(DataTable);

            if (parameterList != null)
            {
                parameterList.Add(this);
            }
        }
        public static implicit operator string(tableParameter p) { return p.parameterName; }
        public override MIDDbParameter MakeSQLParameter()
        {
            return new MIDDbParameter(parameterName, _Value, eDbType.Structured);
        }
        public string typeName;
        //public string columnNames;  //list of column names in the datatable, delimited by semicolon 
        //public string columnTypes;  //list of column types in the datatable, delimited by semicolon. Current supported types: int, varchar

        private DataTable _Value;
        public DataTable Value
        {
            get { return _Value; }
            set { this.SetValue(value); }
        }
        public virtual bool SetValue(DataTable newValue)
        {
            bool valid = IsNewValueValid(newValue);

            if (valid == true)
            {
                //Definition.RaiseBeforePropertyChanged(_Value, newValue);

                _Value = newValue;

                //HasChanged = true;

                //Definition.RaiseAfterPropertyChanged(_Value, newValue);
            }

            return valid;

        }
        public bool IsNewValueValid(DataTable newValue)
        {
            //base.InvalidMessage = string.Empty;
            //base.InvalidTitle = string.Empty;
            //base.HasInvalidMessage = false;
            return true;

        }

    }
}
