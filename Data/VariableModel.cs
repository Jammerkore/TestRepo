//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Data;
//using System.IO;  
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    #region FormulaElement
//    /// <summary>
//    /// Identifies the valid formula element types
//    /// </summary>
//    public enum  eFormulaElementType : byte
//    {
//        Variable = 0,
//        Operation = 1,
//        IntegerConstant = 2,
//        RealConstant = 3
//    }
//    public enum eFormulaOperation : byte
//    {
//        Add = 0,
//        Subtract = 1,
//        Multiply = 2,
//        Divide = 3,
//        Power = 4,
//        Modulo = 5
//    }
//    /// <summary>
//    /// Identifies a formula element
//    /// </summary>
//    public class FormulaElement
//    {
//        private eFormulaElementType _elementType;
//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aElementType">Formula element type</param>
//        public FormulaElement(eFormulaElementType aElementType)
//        {
//            _elementType = aElementType;
//        }
//        /// <summary>
//        /// Gets Formula Element Type
//        /// </summary>
//        public eFormulaElementType FormulaElementType
//        {
//            get { return _elementType; }
//        }
//    }
//    /// <summary>
//    /// Describes a formula element
//    /// </summary>
//    /// <typeparam name="T">T must be int, double or string.</typeparam>
//    public class FormulaValue<T>:FormulaElement 
//    {
//        private T _elementValue;
//        /// <summary>
//        /// Instantiates an instance of this object
//        /// </summary>
//        /// <param name="aFormulaElementType">Element type</param>
//        /// <param name="aElementValue">Elemnt value</param>
//        public FormulaValue(eFormulaElementType aFormulaElementType, T aElementValue)
//            : base(aFormulaElementType)
//        {
//            _elementValue = aElementValue;
//        }
//        /// <summary>
//        /// Gets Element Value
//        /// </summary>
//        public T ElementValue
//        {
//            get { return _elementValue; }
//        }
//    }
//    /// <summary>
//    /// Describes a formula
//    /// </summary>
//    public class FormulaExpression
//    {
//        private FormulaElement[] _formulaElements;
//        private string _operation;
//        /// <summary>
//        /// Creates an instance of the FormulaExpression class
//        /// </summary>
//        /// <param name="aExpression">An algebraic or reverse polish notation expression</param>
//        /// <param name="aAlgebraic">True: expression is an algebraic expression; False: expression is in reverse polish notation</param>
//        /// <remarks>Variables are identified using the at-sign @ preceding an integer identifier of the variable, valid operations are +, -, *, /, and ^. Algebraic expression may include parenthesis, ().</remarks>
//        public FormulaExpression(string aExpression, bool aAlgebraic)
//        {
//            string expression;
//            char[] delimiter = { '(' };
//            if (aAlgebraic)
//            {
//                expression = MIDMath.ConvertInFixToPostFixNotation(aExpression);
//            }
//            else
//            {
//                expression = aExpression;
//            }
//            delimiter[0] = ' ';
//            string[] formulaStrings = expression.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

//            _formulaElements = new FormulaElement[formulaStrings.Length];
//            int i=0;
//            foreach (string element in formulaStrings)
//            {
//                switch (element)
//                {
//                    case "+":
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Add);
//                            break;
//                        }
//                    case "-":
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Subtract);
//                            break;
//                        }
//                    case "*":   
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Multiply);
//                            break;
//                        }
//                    case "/":
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Divide);
//                            break;
//                        }
//                    case "^":
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Power);
//                            break;
//                        }
//                    case "%":
//                        {
//                            _formulaElements[i] = new FormulaValue<eFormulaOperation>(eFormulaElementType.Operation, eFormulaOperation.Modulo);
//                            break;
//                        }
//                    default:
//                        {
//                            if (MIDMath.IsNumber(element))
//                            {

//                                _formulaElements[i] = new FormulaValue<double>(eFormulaElementType.RealConstant, Convert.ToDouble(element));
//                            }
//                            else if (element[0] == '@')
//                            {
//                                _formulaElements[i] = new FormulaValue<int>(eFormulaElementType.Variable, Convert.ToInt32(element.Substring(1, element.Length - 1)));
//                                break;
//                            }
//                            else
//                            {
//                                throw new ArgumentException("Invalid Reverse Polish Notation [" + expression + "]");
//                            }
//                            break;
//                        }
//                }
//                i++;
//            }
//        }
//        /// <summary>
//        /// Creates an instance of the Formula Expression
//        /// </summary>
//        /// <param name="aFormulaElements"></param>
//        public FormulaExpression(FormulaElement[] aFormulaElements)
//        {
//            _formulaElements = aFormulaElements;
//        }


//        /// <summary>
//        /// Gets the reverse Polish Notation string for this formula expression
//        /// </summary>
//        public string ReversePolishNotation
//        {
//            get
//            {
//                StringBuilder sb = new StringBuilder();
//                foreach (object obj in _formulaElements)
//                {
//                    if (obj is FormulaValue<eFormulaOperation>)
//                    {
//                        FormulaValue<eFormulaOperation> fev = (FormulaValue<eFormulaOperation>)obj;
//                        switch (fev.ElementValue)
//                        {
//                            case (eFormulaOperation.Add):
//                                {
//                                    sb.Append(" +");
//                                    break;
//                                }
//                            case (eFormulaOperation.Subtract):
//                                {
//                                    sb.Append(" -");
//                                    break;
//                                }
//                            case (eFormulaOperation.Multiply):
//                                {
//                                    sb.Append(" *");
//                                    break;
//                                }
//                            case (eFormulaOperation.Divide):
//                                {
//                                    sb.Append(" /");
//                                    break;
//                                }
//                            case (eFormulaOperation.Power):
//                                {
//                                    sb.Append(" ^");
//                                    break;
//                                }
//                            case (eFormulaOperation.Modulo):
//                                {
//                                    sb.Append(" %");
//                                    break;
//                                }
//                            default:
//                                {
//                                    throw new ArgumentException("Unknown RPN Operation [" + (int)fev.ElementValue + "]");
//                                }
//                        }
//                    }
//                    else if (obj is FormulaValue<int>)
//                    {
//                        FormulaValue<int> fev = (FormulaValue<int>)obj;
//                        switch (fev.FormulaElementType)
//                        {
//                            case eFormulaElementType.Variable:
//                                {
//                                    sb.Append(" @" + fev.ElementValue.ToString());
//                                    break;
//                                }
//                            case eFormulaElementType.IntegerConstant:
//                                {
//                                    sb.Append(" " + fev.ElementValue.ToString());
//                                    break;
//                                }
//                        }
//                    }
//                    else if (obj is FormulaValue<string>)
//                    {
//                        FormulaValue<string> fev = (FormulaValue<string>)obj;
//                        sb.Append(" " + fev.ElementValue.ToString());
//                    }
//                    else 
//                    {
//                        FormulaValue<double> fev = (FormulaValue<double>)obj;
//                        sb.Append(" " + fev.ToString());
//                    }
//                }
//                return sb.ToString();
//            }
//        }
//        /// <summary>
//        /// Gets the formula as an array of FormulaElement in reverse Polish Notation order.
//        /// </summary>
//        public FormulaElement[] Formula
//        {
//            get
//            {
//                return _formulaElements;
//            }
//        }
//    }
//    #endregion

//    #region vidTableEntry
//    /// <summary>
//    /// Structure to describe a variable
//    /// </summary>
//    [Serializable]
//    public struct vidTableEntry
//    {
//        private string _name;
//        private int _variableID;
//        private short _variableIDX;
//        private byte _decimalPrecision;
//        private byte _rollType;
//        private FormulaExpression _formulaExpression; // TT#555 Total variables should be aggregates of other variables
//        // Creates an instance of a variable
//        public vidTableEntry(int aVariableID, short aVariableIDX, byte aDecimalPrecision, byte aRollType, string aExpression, bool aAlgebraicExpression) // TT#555 Total variables should be aggregates of other variables
//        {
//            _variableID = aVariableID;
//            _name = MIDText.GetTextOnly(aVariableID);
//            _variableIDX = aVariableIDX;
//            _decimalPrecision = aDecimalPrecision;
//            _rollType = aRollType;
//            // begin TT#555 Total variables should be aggregates of other variables
//            if (aExpression != null)
//            {
//                _formulaExpression = new FormulaExpression(aExpression, aAlgebraicExpression);
//            }
//            else
//            {
//                _formulaExpression = null;
//            }
//        }
//        /// <summary>
//        /// Gets variable name
//        /// </summary>
//        public string VariableName
//        {
//            get { return _name; }
//        }
//        /// <summary>
//        /// Gets the variable ID for this variable (the variable ID is used to retrieve the variable name from the database).
//        /// </summary>
//        public int VariableID
//        {
//            get { return _variableID; }
//        }
//        /// <summary>
//        /// Gets Variable Index used to reference within a BIN table
//        /// </summary>
//        public short VariableIDX
//        {
//            get { return _variableIDX; }
//        }
//        /// <summary>
//        /// Gets number of decimal places (base 10) 
//        /// </summary>
//        public byte DecimalPrecision
//        {
//            get { return _decimalPrecision; }
//        }
//        /// <summary>
//        /// Identifies the roll type of the variable
//        /// </summary>
//        public byte RollType
//        {
//            get { return _rollType; }
//            set { _rollType = value; }
//        }
//        // begin TT#555 Total variables should be aggregate of other variables
//        public bool VariableIsCalculated
//        {
//            get
//            {
//                return _formulaExpression != null;
//            }
//        }
//        public FormulaExpression VariableCalculationFormula
//        {
//            get
//            {
//                return _formulaExpression;
//            }
//        }
//        public string VariableReversePolishNotationFormula
//        {
//            get
//            {
//                return _formulaExpression.ReversePolishNotation;
//            }
//            set
//            {
//                if (value == null)
//                {
//                    _formulaExpression = null;
//                }
//                else
//                {
//                    _formulaExpression = new FormulaExpression(value, false);
//                }
//            }
//        }
//        public string VariableAlgebraicFormula
//        {
//            set
//            {
//                if (value == null)
//                {
//                    _formulaExpression = null;
//                }
//                else
//                {
//                    _formulaExpression = new FormulaExpression(value, true);
//                }
//            }
//        }
//        // end TT#555 Total variables should be aggregate of other variables
//    }
//    #endregion vidTableEntry

//    #region Variable Xref
//    /// <summary>
//    /// A cross reference of the Variable Name, Variable ID  to the Variable IDX
//    /// </summary>
//    public struct VariableXref
//    {
//        private eMIDVariableModelType _variableModelType;
//        private string _variableName;
//        private int _variableID;
//        private short _variableIDX;
//        /// <summary>
//        /// Creates an instance of the structure
//        /// </summary>
//        /// <param name="aVariableModelType">The Variable Model Type of the table where this variable resides</param>
//        /// <param name="aVariableName">Variable Name</param>
//        /// <param name="aVariableID">ID that uniquely identifies this variable</param>
//        /// <param name="aVariableIDX">Index of this variable within the variable model</param>
//        public VariableXref(eMIDVariableModelType aVariableModelType, string aVariableName, int aVariableID, short aVariableIDX)
//        {
//            _variableModelType = aVariableModelType;
//            _variableName = aVariableName;
//            _variableID = aVariableID;
//            _variableIDX = aVariableIDX;
//        }
//        /// <summary>
//        /// Gets Variable model type associated with this cross reference
//        /// </summary>
//        public eMIDVariableModelType VariableModelType
//        {
//            get { return _variableModelType; }
//        }
//        /// <summary>
//        /// Gets the name for this variable
//        /// </summary>
//        public string VariableName
//        {
//            get { return _variableName; }
//        }
//        /// <summary>
//        /// Gets the unique ID for this variable
//        /// </summary>
//        public int VariableID
//        {
//            get { return _variableID; }
//        }
//        /// <summary>
//        /// Gets the index associated with this variable within the variable model type
//        /// </summary>
//        public short VariableIDX
//        {
//            get { return _variableIDX; }
//        }
//    }
//    #endregion Variable Xref

//    // begin TT#555 Total variable is aggregate of other variables
//    /// <summary>
//    /// Structure to hold the cacluation priority of a variable
//    /// </summary>
//    public struct VariableFlow
//    {
//        private short _idx;
//        private short _sequence;
//        public VariableFlow(short aVariableIDX)
//        {
//            _idx = aVariableIDX;
//            _sequence = 0;
//        }
//        public short IDX
//        {
//            get { return _idx; }
//        }
//        public short CalcSequence
//        {
//            get { return _sequence; }
//            set { _sequence = value; }
//        }
//    }
//    // end TT#555 Total variable is aggregate of other variables

//    #region VariableModel
//    /// <summary>
//    /// Variable Model Basic information
//    /// </summary>
//    [Serializable]
//    public class VariableModel : DataLayer
//    {
//        private int _variableModelKey;
//        private int _variableModelID;
//        private string _variableModelName;

//        private vidTableEntry[] _variableModelColumnInfo;
       
//        private Dictionary<string, Nullable<vidTableEntry>> _variableName_IDX_Dictionary;
//        private Dictionary<int, Nullable<vidTableEntry>> _variableID_IDX_Dictionary;
//        private Nullable<vidTableEntry> _currentVid;
//        private VariableFlow[] _variableFlow;   // TT#555 Total variable is aggregate of other variables

//        // begin fields ONLY VALID when instantiated by BIN Table Name
//        private string _binTableName;
//        private string _normalizedTableName;
//        private eDatabaseBinKeyType _databaseBinKeyType;
//        private string _updateBinStoredProcedureName;
//        // end  fields ONLY VALID when instantiated by BIN Table Name

//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aVariableModelRID">Existing Variable Model Key</param>
//        public VariableModel(int aVariableModelRID)
//        {
//            _variableModelKey = aVariableModelRID;
//            _variableModelName = string.Empty;
//            _binTableName = string.Empty;
//            _normalizedTableName = string.Empty;
//            _updateBinStoredProcedureName = string.Empty;
//            _databaseBinKeyType = eDatabaseBinKeyType.None;
//            string SQLCommand =
//                 "Select m.*, d.* "
//               + " FROM VARIABLE_MODEL m, VARIABLE_MODEL_ENTRY d "
//               + " where m.VARIABLE_MODEL_RID = " + aVariableModelRID.ToString()
//               + " and  m.VARIABLE_MODEL_RID = d.VARIABLE_MODEL_RID "
//               + " ORDER BY VARIABLE_IDX";
//            DataTable dt = _dba.ExecuteQuery(SQLCommand);
//            Construct(dt);
//        }
//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aVariableModelType">Existing Variable Model Type</param>
//        public VariableModel(eMIDVariableModelType aVariableModelType)
//        {
//            _variableModelKey = Include.NoRID;
//            _variableModelName = string.Empty;
//            _binTableName = string.Empty;
//            _normalizedTableName = string.Empty;
//            _updateBinStoredProcedureName = string.Empty;
//            _databaseBinKeyType = eDatabaseBinKeyType.None;
//            string SQLCommand =
//                "Select m.*, d.* "
//                + " FROM VARIABLE_MODEL m, VARIABLE_MODEL_ENTRY d "
//                + " where m.VARIABLE_MODEL_ID = " + aVariableModelType.ToString()
//                + " and m.VARIABLE_MODEL_RID = d.VARIABLE_MODEL_RID "
//                + " ORDER BY VARIABLE_IDX";
//            DataTable dt = _dba.ExecuteQuery(SQLCommand);
//            Construct(dt);
//        }
//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aIsBinTableName">TRUE:  specified string is a BIN table name; FALSE: specified name is a Variable Model Name</param>
//        /// <param name="aVariableBinTableOrModelName">BIN Table name OR Variable Model Name</param>
//        public VariableModel(bool aIsBinTableName, string aVariableBinTableOrModelName)
//        {
//            string SQLCommand;
//            DataTable dt;
//            if (aIsBinTableName)
//            {
//                _variableModelKey = Include.NoRID;
//                _variableModelName = string.Empty;
//                _binTableName = aVariableBinTableOrModelName;
//                _normalizedTableName = string.Empty;
//                _updateBinStoredProcedureName = string.Empty;
//                _databaseBinKeyType = eDatabaseBinKeyType.None;
//                SQLCommand =
//                   "Select m.*, d.*, t.* "
//                   + " FROM VARIABLE_MODEL m, VARIABLE_MODEL_ENTRY d, VARIABLE_MAP_BIN_TO_MODEL t "
//                   + " where t.BIN_TABLE_NAME = '" + aVariableBinTableOrModelName + "'"
//                   + " and  t.VARIABLE_MODEL_RID = m.VARIABLE_MODEL_RID "
//                   + " and  t.VARIABLE_MODEL_RID = d.VARIABLE_MODEL_RID "
//                   + " ORDER BY VARIABLE_IDX";
//                dt = _dba.ExecuteQuery(SQLCommand);
//                if (dt.Rows.Count > 0)
//                {
//                    DataRow dr = dt.Rows[0];
//                    if (dr["NORMALIZED_TABLE_NAME"] != DBNull.Value)
//                    {
//                        _normalizedTableName = Convert.ToString(dr["NORMALIZED_TABLE_NAME"], CultureInfo.CurrentUICulture);
//                    }
//                    if (dr["BIN_UPDATE_STORED_PROCEDURE"] != DBNull.Value)
//                    {
//                        _updateBinStoredProcedureName = Convert.ToString(dr["BIN_UPDATE_STORED_PROCEDURE"], CultureInfo.CurrentUICulture);
//                    }
//                    _databaseBinKeyType = (eDatabaseBinKeyType)Convert.ToInt32(dr["DATABASE_BIN_KEY_TYPE"], CultureInfo.CurrentUICulture);
//                }
//            }
//            else
//            {
//                _variableModelKey = Include.NoRID;
//                _variableModelName = aVariableBinTableOrModelName;
//                _binTableName = string.Empty;
//                _normalizedTableName = string.Empty;
//                _updateBinStoredProcedureName = string.Empty;
//                _databaseBinKeyType = eDatabaseBinKeyType.None;
//                SQLCommand =
//                     "Select m.*, d.* "
//                   + " FROM VARIABLE_MODEL m, VARIABLE_MODEL_ENTRY d "
//                   + " where m.VARIABLE_MODEL_Name = '" + aVariableBinTableOrModelName + "'"
//                   + " ORDER BY VARIABLE_IDX";
//                dt = _dba.ExecuteQuery(SQLCommand);
//            }
//            Construct(dt);
//        }
//        /// <summary>
//        /// Builds Variable Model from database
//        /// </summary>
//        /// <param name="aDataTable">DataTable from constructor</param>
//        private void Construct(DataTable aDataTable)
//        {
//            _variableName_IDX_Dictionary = new Dictionary<string, Nullable<vidTableEntry>>();
//            _variableID_IDX_Dictionary = new Dictionary<int, Nullable<vidTableEntry>>();

//            if (aDataTable.Rows.Count > 0)
//            {
//                DataRow dr = aDataTable.Rows[0];
//                _variableModelKey = Convert.ToUInt16(dr["VARIABLE_MODEL_RID"], CultureInfo.CurrentUICulture);
//                _variableModelName = Convert.ToString(dr["VARIABLE_MODEL_NAME"], CultureInfo.CurrentUICulture);
//                _variableModelID = Convert.ToInt32(dr["VARIABLE_MODEL_ID"], CultureInfo.CurrentUICulture);

//                _variableModelColumnInfo = new vidTableEntry[aDataTable.Rows.Count];
//                string variableRPN_Expression;  // TT#555 Total variable should be aggregate of other variables 
//                for (int i = 0; i < aDataTable.Rows.Count; i++)
//                {
//                    dr = aDataTable.Rows[i];
//                    short vid = Convert.ToInt16(dr["VARIABLE_IDX"], CultureInfo.CurrentUICulture);
//                    if (vid != i)
//                    {
//                        throw new Exception("Missing variable id " + i.ToString() + "; Found " + vid.ToString());
//                    }
//                    // begin TT#555 Total variable should be an aggregate of other variables
//                    if (dr["VARIABLE_RPN_CALC_EXPRESSION"] is DBNull)
//                    {
//                        variableRPN_Expression = null;
//                    }
//                    else
//                    {
//                        variableRPN_Expression = Convert.ToString(dr["VARIABLE_RPN_CALC_EXPRESSION"]);
//                    }
//                    // end TT#555 Total variable should be an aggregate of other variables
//                    _variableModelColumnInfo[vid] =
//                        new vidTableEntry
//                            (Convert.ToInt32(dr["VARIABLE_ID"], CultureInfo.CurrentUICulture),
//                            vid,
//                            Convert.ToByte(dr["VARIABLE_DECIMAL_PRECISION"], CultureInfo.CurrentUICulture),
//                            Convert.ToByte(dr["VARIABLE_ROLL_TYPE"], CultureInfo.CurrentUICulture),            // TT#555 Total variable should be aggregate of other variables 
//                            variableRPN_Expression,                                                            // TT#555 Total variable should be aggregate of other variables 
//                            false);                                                                            // TT#555 Total variable should be aggregate of other variables 
//                    _variableName_IDX_Dictionary.Add(_variableModelColumnInfo[vid].VariableName, _variableModelColumnInfo[vid]);
//                    _variableID_IDX_Dictionary.Add(_variableModelColumnInfo[vid].VariableID, _variableModelColumnInfo[vid]);
//                }
//            }
//            _currentVid = null;
//            _variableFlow = null;  // TT#555 Total variable is aggregate of other variables
//        }
//        /// <summary>
//        /// Gets the list of columns in ordinal (IDX) sequence for this Variable Model
//        /// </summary>
//        public vidTableEntry[] VariableModelColumnInfo
//        {
//            get { return _variableModelColumnInfo; }
//        }
//        /// <summary>
//        /// Gets a cross referenced array of variables and their associated indices for this Variable Model
//        /// </summary>
//        public VariableXref[] VariableXrefArray
//        {
//            get
//            {
//                VariableXref[] variableArray = new VariableXref[_variableModelColumnInfo.Length];
//                for (int i = 0; i < _variableModelColumnInfo.Length; i++)
//                {
//                    vidTableEntry variableEntry = _variableModelColumnInfo[i];
//                    variableArray[i] = new VariableXref(
//                                              (eMIDVariableModelType)this._variableModelKey,
//                                              variableEntry.VariableName,
//                                              variableEntry.VariableID,
//                                              variableEntry.VariableIDX);
//                }
//                return variableArray;
//            }
//        }
//        /// <summary>
//        /// Gets the Variable Model ID
//        /// </summary>
//        public int VariableModelID
//        {
//            get { return _variableModelID; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Name
//        /// </summary>
//        public string VariableModelName
//        {
//            get { return _variableModelName; }
//        }
//        /// <summary>
//        /// Gets the BIN table name that is using this variable model
//        /// </summary>
//        /// <remarks>BIN table name is only available if this class is instantiated using the BIN table name</remarks>
//        public string BinTableName
//        {
//            get { return _binTableName; }
//        }
//        /// <summary>
//        /// Gets the Normalized table name associated with the BIN table name
//        /// </summary>
//        /// <remarks>
//        /// Normalized table name is only available if this class is instantiated using the BIN table Name,
//        /// The Normalized table name is optional (it is not required to exist). If it exists, it is assumed
//        /// the variables associated with the BIN table name are also associated with the Normalized table.
//        /// </remarks>
//        public string NormalizedTableName
//        {
//            get { return _normalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the name of the Update Bin Stored Procedure
//        /// </summary>
//        /// <remarks>The UpdateBinStoredProcedure is only available if this class is instantiated using the BIN table Name.</remarks>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _updateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the database Bin Key Type that describes the key structure on the BIN table.
//        /// </summary>
//        /// <remarks>
//        /// The Database Bin Key Type is only available if this class is instantiated using the BIN table name.
//        /// It is assumed that when a Normalized table is associated with the BIN table then this key structure is also
//        /// used by the Normalized table.
//        /// </remarks>
//        public eDatabaseBinKeyType DatabaseBinKeyType
//        {
//            get { return _databaseBinKeyType; }
//        }

//        // begin TT#555 total variable is aggregate of other variables
//        public VariableFlow[] VariableCalculationFlow
//        {
//            get
//            {
//                if (_variableFlow == null)
//                {
//                    this.PrioritizeVariables();
//                }
//                return _variableFlow;
//            }
//        }
//        // end TT#555 total variable is aggregate of other variables

//        /// <summary>
//        /// Gets the variable descriptive information for the given Variable Name
//        /// </summary>
//        /// <param name="aVariableName">Variable Name for which descriptive information is required</param>
//        /// <returns>vidTableEntry for the given variable</returns>
//        public vidTableEntry GetVariableInfo(string aVariableName)
//        {
//            return GetVariableInfo(GetVariableIDX(aVariableName));
//        }
//        /// <summary>
//        /// Gets the variable descriptive information for the given variable index
//        /// </summary>
//        /// <param name="aVariableIDX">Index for the variable within this variable model</param>
//        /// <returns>vidTableEntry for the given variable index</returns>
//        public vidTableEntry GetVariableInfo(short aVariableIDX)
//        {
//            return _variableModelColumnInfo[aVariableIDX];
//        }
//        /// <summary>
//        /// Gets the variable index for the given variable name
//        /// </summary>
//        /// <param name="aVariableName">Variable Name within this model for which the variable index is required</param>
//        /// <returns>Variable Index within this model</returns>
//        public short GetVariableIDX(string aVariableName)
//        {
//            short variableIDX;
//            if (TryGetVariableIDX(aVariableName, out variableIDX))
//            {
//                return variableIDX;
//            }
//            throw new Exception("Variable Name ='" + aVariableName + "' not found for Variable Model = '" + this._variableModelName + "'");
//        }

//        /// <summary>
//        /// Gets the variable UInt16 index for the given veriable integer ID
//        /// </summary>
//        /// <param name="aVariableID">Variable ID</param>
//        /// <returns>Variable IDX</returns>
//        public short GetVariableIDX(Int32 aVariableID)
//        {
//            short aVariableIDX;
//            if (TryGetVariableIDX(aVariableID, out aVariableIDX))
//            {
//                return aVariableIDX;
//            }
//            throw new Exception("Variable ID ='" + aVariableID.ToString() + "' not found for Variable Model = '" + _variableModelName + "'");
//        }
//        /// <summary>
//        /// Trys to get VariableIDX for the given variable name
//        /// </summary>
//        /// <param name="aVariableName">Variable Name</param>
//        /// <param name="aVariableIDX">VariableIDX</param>
//        /// <returns>TRUE: aVariableIDX contains the desired index; FALSE: the specified VariableName was not found</returns>
//        public bool TryGetVariableIDX(string aVariableName, out short aVariableIDX)
//        {
//            if (_currentVid == null
//                || ((vidTableEntry)_currentVid).VariableName != aVariableName)
//            {
//                if (!_variableName_IDX_Dictionary.TryGetValue(aVariableName, out _currentVid))
//                {
//                    aVariableIDX = 0;
//                    return false;
//                }
//            }
//            aVariableIDX = ((vidTableEntry)_currentVid).VariableIDX;
//            return true;
//        }
//        /// <summary>
//        /// Tries to get the variable UInt16 index for the given variable integer ID
//        /// </summary>
//        /// <param name="aVariableID">Integer Variable ID</param>
//        /// <param name="aVariableIDX">UInt16 Variable IDX</param>
//        /// <returns>TRUE: returns aVariableIDX corresponding to the integer variable ID; FALSE: Variable ID does not exist in the list of variables</returns>
//        public bool TryGetVariableIDX(int aVariableID, out short aVariableIDX)
//        {
//            if (_currentVid == null
//                || ((vidTableEntry)_currentVid).VariableID != aVariableID)
//            {
//                if (!_variableID_IDX_Dictionary.TryGetValue(aVariableID, out _currentVid))
//                {
//                    aVariableIDX = 0;
//                    return false;
//                }
//            }
//            aVariableIDX = ((vidTableEntry)_currentVid).VariableIDX;
//            return true;
//        }
//        /// <summary>
//        /// Adds variables to the variable model defaulting precision and roll type to 0
//        /// </summary>
//        /// <param name="aVariableID">Array of variable names to add</param>
//        /// <returns>True: if add succsessful; False if add fails</returns>
//        public bool AddVariable(int[] aVariableID)
//        {
//            return AddVariable(aVariableID, 0, 0);
//        }
//        /// <summary>
//        /// Adds variables to the variable model where each variable has the same specified precision and roll type
//        /// </summary>
//        /// <param name="aVariableID">Array of variable IDs to add</param>
//        /// <param name="aVariablePrecision">Decimal precision shared by each variable</param>
//        /// <param name="aVariableRollType">Roll type shared by each variable</param>
//        /// <returns></returns>
//        public bool AddVariable(int[] aVariableID, byte aVariablePrecision, byte aVariableRollType)
//        {
//            byte[] variablePrecision = new byte[aVariableID.Length];
//            byte[] variableRollType = new byte[aVariableID.Length];
//            for (int i = 0; i < aVariableID.Length; i++)
//            {
//                variablePrecision[i] = aVariablePrecision;

//            }
//            return AddVariable(aVariableID, aVariablePrecision, aVariableRollType);
//        }
//        /// <summary>
//        /// Adds variables to the variable model 
//        /// </summary>
//        /// <param name="aVariableID">Variable name to add</param>
//        /// <param name="aVariablePrecision">Precision of the given variable</param>
//        /// <param name="aVariableRollType">Roll type for the given variable</param>
//        /// <returns>True: if add is successful; False: if add fails</returns>
//        public bool AddVariable(int aVariableID, byte aVariablePrecision, byte aVariableRollType, string aExpression, bool aAlgebraicExpression) // TT#555 Total Variable is aggregate of other variables
//        {
//            int[] variableID = new int[1];
//            variableID[0] = aVariableID;
//            byte[] variablePrecision = new byte[1];
//            variablePrecision[0] = aVariablePrecision;
//            byte[] variableRollType = new byte[1];
//            variableRollType[0] = aVariableRollType;
//            // begin TT#555 Total variable is aggregate of other variables
//            string[] expression = new string[1];
//            expression[0] = aExpression;
//            bool[] algebraicExpression = new bool[1];
//            algebraicExpression[0] = aAlgebraicExpression;
//            // end TT#555 Total variable is aggregate of other variables
//            return AddVariable(variableID, variablePrecision, variableRollType, expression, algebraicExpression); // TT#555 Total variable is aggregate of other variables.
//        }
//        /// <summary>
//        /// Adds variables to the variable model
//        /// </summary>
//        /// <param name="aVariableID">Array of variable IDs to add</param>
//        /// <param name="aVariablePrecision">Array giving the precision of each specified variable (there must be a 1-to-1 correspondence between this array and the variable names)</param>
//        /// <param name="aVariableRollType">Array giving the roll type of each specified variable (there must be a 1-to-1 correspondence between this array and the variable names)</param>
//        /// <returns>True: if all adds are successful; False: if any add fails</returns>
//        public bool AddVariable(int[] aVariableID, byte[] aVariablePrecision, byte[] aVariableRollType, string[] aExpression, bool[] aAlgebraicExpression)  //  TT#555 Total variable is aggregate of reg, promo, mkdn
//        {
//            if (aVariableID.Length != aVariablePrecision.Length)
//            {
//                throw new Exception("VariableID array length of " + aVariableID.Length.ToString() + " not equal to VariablePrecision array length of " + aVariablePrecision.Length.ToString());
//            }
//            if (aVariableID.Length != aVariableRollType.Length)
//            {
//                throw new Exception("VariableID array length of " + aVariableID.Length.ToString() + " not equal to VariableRollType array length of " + aVariableRollType.Length.ToString());
//            }
//            string[] variableNames;
//            if (Duplicates(aVariableID, out variableNames))
//            {
//                throw new Exception("duplicate variable name not allowed");
//            }
//            vidTableEntry[] variableModelColumnInfo;
//            short nextVIDX = 0;
//            if (_variableModelColumnInfo == null)
//            {
//                variableModelColumnInfo = new vidTableEntry[aVariableID.Length];
//            }
//            else
//            {
//                variableModelColumnInfo = new vidTableEntry[_variableModelColumnInfo.Length + aVariableID.Length];
//                for (int i = 0; i < variableModelColumnInfo.Length; i++)
//                {
//                    variableModelColumnInfo[i] = _variableModelColumnInfo[i];
//                }
//                nextVIDX = (short)_variableModelColumnInfo.Length;
//            }
//            _variableModelColumnInfo = variableModelColumnInfo;

//            for (int i = 0; i < aVariableID.Length; i++)
//            {
//                _variableModelColumnInfo[nextVIDX] =
//                    new vidTableEntry(aVariableID[i], nextVIDX, aVariablePrecision[i], aVariableRollType[i], aExpression[i], aAlgebraicExpression[i]); // TT#555 Total variable is aggregate of other variables
//                this._variableName_IDX_Dictionary.Add(variableNames[i], _variableModelColumnInfo[nextVIDX]);
//                nextVIDX++;
//            }
//            _variableFlow = null; // TT#555 Total variable is aggregate of other variables
//            return true;
//        }
//        /// <summary>
//        /// Checks the supplied array for duplicate names both within the given array and when compared to the current variables within this model
//        /// </summary>
//        /// <param name="aVariableName">Array of variable names to check</param>
//        /// <returns>TRUE: if duplicate names are found; FALSE: if no duplicates found</returns>
//        private bool Duplicates(int[] aVariableID, out string[] aVariableName)
//        {
//            aVariableName = new string[aVariableID.Length];

//            for (int i = 0; i < aVariableID.Length; i++)
//            {
//                aVariableName[i] = MIDText.GetTextOnly(aVariableID[i]);
//                if (Duplicates(aVariableName[i]))
//                {
//                    return true;
//                }
//            }
//            for (int i = 0; i < aVariableName.Length; i++)
//            {
//                for (int j = aVariableName.Length; j > i; j--)
//                {
//                    if (aVariableName[i] == aVariableName[j])
//                    {
//                        return true;
//                    }
//                }
//            }
//            return false;
//        }
//        /// <summary>
//        /// Determines if the specified variable name is a duplicate of any existing variable within the model
//        /// </summary>
//        /// <param name="aVariableName">Variable Name to check</param>
//        /// <returns>TRUE:  duplicate found; FALSE: no duplicate found</returns>
//        private bool Duplicates(string aVariableName)
//        {
//            Nullable<vidTableEntry> testID;
//            return this._variableName_IDX_Dictionary.TryGetValue(aVariableName, out testID);
//        }

//        //Begin TT#739-MD -jsobek -Delete Stores
//        /// <summary>
//        /// Writes this Variable Model Base to the database
//        /// </summary>
//        /// <returns>TRUE: write is successfult; FALSE: write failed</returns>
//        //public bool WriteVariableModel()
//        //{
//        //    try
//        //    {
//        //        _dba.OpenUpdateConnection();
//        //        string SQLCommand;
//        //        if (_variableModelKey == Include.NoRID)
//        //        {
//        //            MIDDbParameter[] InParams = 
//        //               {new MIDDbParameter("@VARIABLE_MODEL_ID", _variableModelID, eDbType.Int, eParameterDirection.Input),
//        //                new MIDDbParameter("@VARIABLE_MODEL_NAME", _variableModelName, eDbType.VarChar, eParameterDirection.Input) 
//        //               };
//        //            MIDDbParameter[] OutParams = { new MIDDbParameter("@VARIABLE_MODEL_RID", DBNull.Value, eDbType.Int, eParameterDirection.Output) };
//        //            _variableModelKey = _dba.ExecuteStoredProcedure("SP_MID_VARIABLE_MODEL_INSERT", InParams, OutParams);
//        //        }
//        //        else
//        //        {
//        //            SQLCommand = "UPDATE VARIABLE_MODEL with (rowlocks) "
//        //                         + "   SET VARIABLE_MODEL_NAME = @VARIABLE_MODEL_NAME"
//        //                         + "  WHERE VARIABLE_MODEL_ID = @VARIABLE_MODEL_ID";
//        //            MIDDbParameter[] InParams = 
//        //                {new MIDDbParameter("@VARIABLE_MODEL_NAME", _variableModelName, eDbType.VarChar, eParameterDirection.Input),
//        //                 new MIDDbParameter("@VARIABLE_MODEL_ID", _variableModelID, eDbType.Int, eParameterDirection.Input)
//        //                };
//        //            if (_dba.ExecuteNonQuery(SQLCommand, InParams) < 0)
//        //            {
//        //                throw new Exception("database error while updating Variable Model table");
//        //            }
//        //        }



//        //        SQLCommand = "DELETE from VARIABLE_MODEL_ENTRY where VARIABLE_MODEL_KEY = " + _variableModelKey.ToString();
//        //        if (_dba.ExecuteNonQuery(SQLCommand) >= 0)
//        //        {
//        //            SQLCommand = "INSERT INTO VARIABLE_MODEL_ENTRY with (rowlock) "
//        //                + " (VARIABLE_MODEL_RID, VARIABLE_IDX, VARIABLE_ID, VARIABLE_DECIMAL_PRECISION, VARIABLE_ROLL_TYPE, VARIABLE_RPN_CALC_EXPRESSION) "  // TT#555 Total variable is aggregate of other variables
//        //                + "  VALUES(@VARIABLE_MODEL_RID, @VARIABLE_IDX, @VARIABLE_ID, @VARIABLE_DECIMAL_PRECISION, @VARIABLE_ROLL_TYPE, @VARIABLE_RPN_CALC_EXPRESSION)"; // TT#555 Total variable is aggregate of other variables
//        //            for (int i = 0; i < _variableModelColumnInfo.Length; i++)
//        //            {
//        //                MIDDbParameter[] InParams = 
//        //                   {new MIDDbParameter("@VARIABLE_MODEL_RID", _variableModelKey, eDbType.Int, eParameterDirection.Input),
//        //                    new MIDDbParameter("@VARIABLE_IDX", _variableModelColumnInfo[i].VariableIDX, eDbType.Int, eParameterDirection.Input),
//        //                    new MIDDbParameter("@VARIABLE_ID", _variableModelColumnInfo[i].VariableName, eDbType.Int, eParameterDirection.Input),
//        //                    new MIDDbParameter("@VARIABLE_DECIMAL_PRECISION", _variableModelColumnInfo[i].DecimalPrecision, eDbType.tinyint, eParameterDirection.Input),
//        //                    new MIDDbParameter("@VARIABLE_ROLL_TYPE", _variableModelColumnInfo[i].RollType, eDbType.Int, eParameterDirection.Input), // TT#555 Total variable is aggregate of other variables
//        //                    new MIDDbParameter("@VARIABLE_RPN_CALC_EXPRESSION", _variableModelColumnInfo[i].VariableReversePolishNotationFormula, eDbType.VarChar,eParameterDirection.Input) //TT#555 Total Variable is aggregate of other variables
//        //                   };

//        //                if (!(_dba.ExecuteNonQuery(SQLCommand, InParams) > 0))
//        //                {
//        //                    return false;
//        //                }
//        //            }
//        //            _dba.CommitData();
//        //            return true;
//        //        }
//        //        return false;
//        //    }
//        //    finally
//        //    {
//        //        _dba.CloseUpdateConnection();
//        //    }
//        //}
//        //End TT#739-MD -jsobek -Delete Stores

//        //begin TT#555 Total variable is aggregate of other variables
//        private void PrioritizeVariables()
//        {
//            _variableFlow = new VariableFlow[this.VariableXrefArray.Length];
//            short[] variableSeq = new short[VariableXrefArray.Length];
//            short varIDX;
//            for (varIDX = 0; varIDX < _variableFlow.Length; varIDX++)
//            {
//                _variableFlow[varIDX] = new VariableFlow(varIDX);
//            }
//            for (varIDX = 0; varIDX < _variableFlow.Length; varIDX++)
//            {
//                SetVariableCalcSequence(varIDX);
//                variableSeq[varIDX] = _variableFlow[varIDX].CalcSequence;
//            }
//            Array.Sort(variableSeq, _variableFlow);
//        }
//        private void SetVariableCalcSequence(short varIDX)
//        {
//            if (_variableFlow[varIDX].CalcSequence == 0)
//            {
//                if (GetVariableInfo(varIDX).VariableIsCalculated)
//                {
//                    // variable is pending calculation (-1)
//                    _variableFlow[varIDX].CalcSequence = -1;
//                    // variable is calculated using non-calculated variables (2)
//                    short calcSequence = 2;
//                    FormulaExpression fex = GetVariableInfo(varIDX).VariableCalculationFormula;
//                    foreach (FormulaElement fe in fex.Formula)
//                    {
//                        if (fe.FormulaElementType == eFormulaElementType.Variable)
//                        {
//                            short feVarIDX;
//                            if (!TryGetVariableIDX(((FormulaValue<int>)fe).ElementValue, out feVarIDX))
//                            {
//                                throw new ArithmeticException("invalid variable [" + ((FormulaValue<int>)fe).ElementValue + "]");
//                            }
//                            SetVariableCalcSequence(feVarIDX);
//                            if (_variableFlow[feVarIDX].CalcSequence >= calcSequence)
//                            {
//                                // variable is calculated using multiple levels of calculated variables
//                                calcSequence = _variableFlow[feVarIDX].CalcSequence;
//                                calcSequence++;
//                            }
//                        }
//                    }
//                    _variableFlow[varIDX].CalcSequence = calcSequence;
//                }
//                else
//                {
//                    // variable is not calculated (1)
//                    _variableFlow[varIDX].CalcSequence = 1;
//                }
//            }
//            else if (_variableFlow[varIDX].CalcSequence == -1)
//            {
//                throw new ArithmeticException(
//                    "Circular reference on variable ["
//                    + GetVariableInfo(varIDX).VariableName
//                    + "("
//                    + GetVariableInfo(varIDX).VariableID
//                    + ")]");
//            }
//        }
//        // end TT#555 Total variable is aggregate of other variables
//    }
//    #endregion VariableModel
//}