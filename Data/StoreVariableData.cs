//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Data;
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    #region StoreVariableDataDictionary
//    /// <summary>
//    /// Store Variable Data Dictionary
//    /// </summary>
//    /// <typeparam name="T">DatabaseBinKey used to access the dictionary</typeparam>
//    [Serializable]
//    public class StoreVariableDataDictionary<T> : Dictionary<T, StoreVariableData<T>>
//        where T : DatabaseBinKey
//    {
//    }
//    #endregion StoreVariableDataDictionary

//    #region StoreVariableData
//    /// <summary>
//    /// A container to hold store variable data for a specific key value (T)
//    /// </summary>
//    /// <typeparam name="T">DatabaseBinKey which describes the key of the data within the container</typeparam>
//    [Serializable]
//    public class StoreVariableData<T>
//        where T : DatabaseBinKey
//    {
//        #region Fields
//        bool _fromNormalizedTable;
//        bool _initializeCalcVariables;  //  TT#555 Total variable is aggregate of other variables
//        VariableModel _variableModel;   //  TT#555 Total variable is aggregate of other variables
//        Byte _status;
//        T _databaseBinKey;
//        StoreVariableVectorContainer _svvc;
//        #endregion Fields

//        #region Constructor
//        /// <summary>
//        /// Creates an instance of StoreVariableData
//        /// </summary>
//        /// <param name="aStatus">Status code for this instance of the data (updated each time data written to database)</param>
//        /// <param name="aDatabaseBinKey">DatabaseBinKey that identifies the database location of this data</param>
//        /// <param name="aStoreVariableVectorContainer">StoreVariableVectorContainer that contains the store data</param>
//        public StoreVariableData(Byte aStatus, T aDatabaseBinKey, StoreVariableVectorContainer aStoreVariableVectorContainer)
//        {
//            _fromNormalizedTable = false;
//            _status = aStatus;
//            _databaseBinKey = aDatabaseBinKey;
//            _svvc = aStoreVariableVectorContainer;
//            _initializeCalcVariables = true; // TT#555 Total variable is aggregate of other variables
//            _variableModel = null;           // TT#555 Total Variable is aggregate of other variables
//        }
//        /// <summary>
//        /// Creates an instance of StoreVariableData
//        /// </summary>
//        /// <param name="aStatus">Status code for this instance of the data (updated each time data written to database)</param>
//        /// <param name="aDatabaseBinKey">DatabaseBinKey that identifies the database location of this data</param>
//        /// <param name="aStoreVariableVectorContainer">StoreVariableVectorContainer that contains the store data</param>
//        /// <param name="aFromNormalizedTable">Indicates if Store Data is from the normalized or the BIN table.</param>
//        public StoreVariableData(bool aFromNormalizedTable, Byte aStatus, T aDatabaseBinKey, StoreVariableVectorContainer aStoreVariableVectorContainer)
//        {
//            _fromNormalizedTable = aFromNormalizedTable;
//            _status = aStatus;
//            _databaseBinKey = aDatabaseBinKey;
//            _svvc = aStoreVariableVectorContainer;
//            _initializeCalcVariables = true; // TT#555 Total variable is aggregate of other variables
//            _variableModel = null;           // TT#555 Total Variable is aggregate of other variables
//        }
//        #endregion Constructor

//        #region Properties
//        /// <summary>
//        /// Gets a bool that indicates whether the store data is from NormalizedTable
//        /// </summary> 
//        public bool StoreDataFromNormalizedTable
//        {
//            get { return _fromNormalizedTable; }
//        }
//        /// <summary>
//        /// Gets the status code of this data
//        /// </summary>
//        public Byte Status
//        {
//            get { return _status; }
//        }
//        /// <summary>
//        /// Gets the DatabaseBinKey for this store data
//        /// </summary>
//        public T DatabaseBinKey
//        {
//            get { return _databaseBinKey; }
//        }
//        /// <summary>
//        /// Gets the StoreVariableVectorContainer containing the store data
//        /// </summary>
//        public StoreVariableVectorContainer StoreVariableVectorContainer
//        {
//            get { return _svvc; }
//        }

//        // begin TT#555 Total variable is aggregate of other variables
//        public bool InitialCalcVariables
//        {
//            get { return _initializeCalcVariables; }
//            set { _initializeCalcVariables = value; }
//        }
//        // end TT#555 Total variable is aggregate of other variables

//        #endregion Properties

//        #region Methods
//        public void CommitStatus()
//        {
//            if (_status == 255)
//            {
//                _status = 1;  // status = 0 is intended for new StoreVariableData
//            }
//            else
//            {
//                _status++;
//            }
//        }
//        // begin TT#555 Total variable is aggregate of other variables
//        /// <summary>
//        /// Calculates any variables that are not stored on the database. NOTE: variables must reside within same "key" on database
//        /// </summary>
//        /// <param name="aVariableModel">Variable Model</param>
//        public void CalculateVariables(VariableModel aVariableModel)
//        {
//            _variableModel = aVariableModel;
//            VariableFlow[] vf = _variableModel.VariableCalculationFlow;
//            short varIDX;
//            for (int i = 0; i < vf.Length; i++)
//            {
//                varIDX = vf[i].IDX;
//                if (vf[i].CalcSequence > 1)
//                {
//                    FormulaExpression fex = _variableModel.GetVariableInfo(varIDX).VariableCalculationFormula;
//                    double multiplier = Math.Pow(10, _variableModel.GetVariableInfo(varIDX).DecimalPrecision);
//                    for (int storeRID = MIDStorageTypeInfo.GetStoreMaxRID(1); storeRID > 0; storeRID--)
//                    {
//                        Stack<double> calculationStack = new Stack<double>();
//                        foreach (FormulaElement fe in fex.Formula)
//                        {
//                            switch (fe.FormulaElementType)
//                            {
//                                case eFormulaElementType.Variable:
//                                    {
//                                        short feVarIDX;
//                                        if (!_variableModel.TryGetVariableIDX(((FormulaValue<int>)fe).ElementValue, out feVarIDX))
//                                        {
//                                            throw new ArithmeticException("invalid variable [" + ((FormulaValue<int>)fe).ElementValue + "]");
//                                        }
//                                        calculationStack.Push(_svvc.GetStoreVariableValue(storeRID, feVarIDX) / multiplier);
//                                        break;
//                                    }
//                                case eFormulaElementType.IntegerConstant:
//                                    {
//                                        calculationStack.Push((double)((FormulaValue<int>)fe).ElementValue);
//                                        break;
//                                    }
//                                case eFormulaElementType.RealConstant:
//                                    {
//                                        calculationStack.Push(((FormulaValue<double>)fe).ElementValue);
//                                        break;
//                                    }
//                                case eFormulaElementType.Operation:
//                                    {
//                                        double operand1 = calculationStack.Pop();
//                                        double operand2 = calculationStack.Pop();
//                                        switch (((FormulaValue<eFormulaOperation>)fe).ElementValue)
//                                        {
//                                            case eFormulaOperation.Add:
//                                                {
//                                                    calculationStack.Push(operand1 + operand2);
//                                                    break;
//                                                }
//                                            case eFormulaOperation.Subtract:
//                                                {
//                                                    calculationStack.Push(operand2 - operand1);
//                                                    break;
//                                                }
//                                            case eFormulaOperation.Divide:
//                                                {
//                                                    if (operand2 == 0)
//                                                    {
//                                                        calculationStack.Push(0);
//                                                    }
//                                                    else
//                                                    {
//                                                        calculationStack.Push(operand1 / operand2);
//                                                    }
//                                                    break;
//                                                }
//                                            case eFormulaOperation.Modulo:
//                                                {
//                                                    if (operand2 == 0)
//                                                    {
//                                                        calculationStack.Push(0);
//                                                    }
//                                                    else
//                                                    {
//                                                        calculationStack.Push(operand1 % operand2);
//                                                    }
//                                                    break;
//                                                }
//                                            case eFormulaOperation.Multiply:
//                                                {
//                                                    calculationStack.Push(operand1 * operand2);
//                                                    break;
//                                                }
//                                            case eFormulaOperation.Power:
//                                                {
//                                                    calculationStack.Push(Math.Pow(operand1, operand2));
//                                                    break;
//                                                }
//                                            default:
//                                                {
//                                                    throw new ArithmeticException("Unknown eFormulaOperation [" + ((FormulaValue<eFormulaOperation>)fe).ElementValue + "]");
//                                                }
//                                        }
//                                        break;
//                                    }
//                                default:
//                                    {
//                                        throw new ArithmeticException("Unknown eFormulaType [" + fe.FormulaElementType.ToString() + "]");
//                                    }
//                            }
//                        }
//                        double storeValue = calculationStack.Pop();
//                        long storeValueLong;
//                        if (storeValue < 0)
//                        {
//                            storeValueLong =
//                               (Int64)(storeValue
//                                       * multiplier
//                                       - .5);   // rounding should use same sign as original value
//                        }
//                        else
//                        {
//                            storeValueLong =
//                                (Int64)(storeValue
//                                        * multiplier
//                                        + .5);
//                        }
//                        _svvc.SetStoreVariableValue(storeRID, eMIDVariableModelType.HistoryVariableModelType, ref _svvc, varIDX, storeValueLong);
//                    }
//                }
//            }
//            _initializeCalcVariables = false;
//        }
//        /// <summary>
//        /// Resets calculated variables to 0 so they compress out on a save.
//        /// </summary>
//        public void ResetCalcVariables()
//        {
//            if (_variableModel != null)
//            {
//                VariableFlow[] vf = _variableModel.VariableCalculationFlow;
//                short varIDX;
//                for (int i = 0; i < vf.Length; i++)
//                {
//                    if (vf[i].CalcSequence > 1) // 1=no calc; 2 or above = calc'd variable
//                    {
//                        varIDX = vf[i].IDX;
//                        for (int storeRID = MIDStorageTypeInfo.GetStoreMaxRID(1); storeRID > 0; storeRID--)
//                        {
//                            _svvc.SetStoreVariableValue(storeRID, eMIDVariableModelType.HistoryVariableModelType, ref _svvc, varIDX, 0);
//                        }
//                    }
//                }
//            }
//            _initializeCalcVariables = true;
//        }
//        // end TT#555 Total variable is aggregate of other variables
//        #endregion Methods
//    }
//    #endregion StoreVariableData

//}