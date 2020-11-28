//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Runtime.Serialization; 
//using System.Runtime.Serialization.Formatters.Binary; 
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;  
//using System.Text;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Data
//{
//    #region DatabaseBinTable
//    /// <summary>
//    /// Describes the variables associated with a table BIN.
//    /// </summary>
//    [Serializable]
//    public class DatabaseBinTable<T>
//        where T : DatabaseBinKey
//    {
//        #region Fields
//        private VariableModel _VariableModel;
//        #endregion Fields

//        #region Contructor
//        /// <summary>
//        /// Creates an instance of this class
//        /// </summary>
//        /// <param name="aBinTableName">Name of the BIN table</param>
//        public DatabaseBinTable(string aBinTableName)
//        {
//            _VariableModel = new VariableModel(true, aBinTableName);
//        }
//        #endregion Constructor

//        #region Properties
//        /// <summary>
//        /// Gets the name of the table BIN
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _VariableModel.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the name of the normalized table name (if it exists) associated with the table BIN name 
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _VariableModel.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the name of the stored procedure that updates the table BIN
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _VariableModel.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// True: normalized table exists; False: Normalized table does not exist
//        /// </summary>
//        public bool NormalizedTableExists
//        {
//            get { return (_VariableModel.NormalizedTableName != null && _VariableModel.NormalizedTableName != string.Empty); }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base that is associated with this table BIN
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _VariableModel; }
//        }
//        #endregion Properties

//        #region Methods

//        #region Read StoreVariableVectorContainer
//        /// <summary>
//        /// Reads a store variable vector array from the table BIN on the database for the given key parameters
//        /// </summary>
//        /// <param name="aDBA">Database Access to use in accessing the database</param>
//        /// <param name="aDatabaseBinKey">Database BIN Key that identifies the StoreVariableVectorContainer(s) to be read</param>
//        /// <param name="aStoreVariableDataDictionary">Existing StoreVariableDataDictionary<T> in which to store the found StoreVariableVectorContainer(s)"</param>
//        /// <returns>StoreVariableDataDictionary<T> with results of the read</returns>
//        public List<StoreVariableData<T>> ReadStoreVariables(DatabaseAccess aDBA, T aDatabaseBinKey)
//        {
//            List<StoreVariableData<T>> storeVariableDataList = new List<StoreVariableData<T>>();
//            StringBuilder sqlCommand = new StringBuilder("Select * from ");
//            sqlCommand.Append(_VariableModel.BinTableName);
//            sqlCommand.Append(" where ");
//            sqlCommand.Append(aDatabaseBinKey.SQL_WhereClauseSegment);
//            sqlCommand.Append(" order by ");
//            for (int i = 0; i < aDatabaseBinKey.Count; i++)
//            {
//                if (i > 0)
//                {
//                    sqlCommand.Append(",");
//                }
//                sqlCommand.Append(aDatabaseBinKey.GetKeyWord(i));
//            }
//            DataTable dt = aDBA.ExecuteSQLQuery(sqlCommand.ToString(), "READ_STORE_VARIABLES");
//            T currentDatabaseBinKey;
//            T thisDatabaseBinKey;
//            int[] thisKey = new int[aDatabaseBinKey.Count];

//            byte status = 0;
//            if (dt.Rows.Count > 0)
//            {
//                //DataRow dr = dt.Rows[j];
//                foreach (DataRow dr in dt.Rows)
//                {
//                    for (int i = 0; i < aDatabaseBinKey.Count; i++)
//                    {
//                        thisKey[i] = Convert.ToInt32(dr[aDatabaseBinKey.GetKeyWord(i)]);
//                    }
//                    thisDatabaseBinKey = (T)aDatabaseBinKey.CreateNewInstance(thisKey);
//                    if (dr["USTAT"] is DBNull)
//                    {
//                        status = 0;
//                    }
//                    else
//                    {
//                        status = Convert.ToByte(dr["USTAT"], CultureInfo.CurrentCulture);
//                    }
//                    //StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                    //svvc.ExpandVectorContainer((byte[])dr["STORE_DATA"]);
//                    StoreVariableVectorContainer svvc = new StoreVariableVectorContainer((byte[])dr["STORE_DATA"]);
//                    storeVariableDataList.Add(new StoreVariableData<T>(status, thisDatabaseBinKey, svvc));
//                }
//            }
//            else
//            {
//                StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//                status = 0;
//                if (this.NormalizedTableExists)
//                {
//                    // Only Allocation has an associated normalized table
//                    if (_VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderTotalDatabaseBinKey
//                        || _VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderDetailDatabaseBinKey
//                        || _VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderBulkDatabaseBinKey
//                        || _VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderPackDatabaseBinKey
//                        || _VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderColorDatabaseBinKey
//                        || _VariableModel.DatabaseBinKeyType == eDatabaseBinKeyType.HeaderColorSizeDatabaseBinKey)
//                    {
//                        short variableIDX;

//                        sqlCommand = new StringBuilder("Select ");
//                        sqlCommand.Append(aDatabaseBinKey.GetKeyWord(0));
//                        for (int i = 1; i < aDatabaseBinKey.Count; i++)
//                        {
//                            sqlCommand.Append(", " + aDatabaseBinKey.GetKeyWord(1));
//                        }
//                        sqlCommand.Append(", ST_RID ");

//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipToDay, out variableIDX))
//                        {
//                            sqlCommand.Append(",SHIP_DAY");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GradeIndex, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(STORE_GRADE_INDEX,0) as STORE_GRADE_INDEX");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityUnits, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(STORE_CAPACITY,0) as STORE_CAPACITY");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityExceedByPercent, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(EXCEED_CAPACITY_PERCENT,'-1') as EXCEED_CAPACITY_PERCENT");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GeneralAuditFlags, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(ALLOC_STORE_GEN_AUDIT_FLAGS,0) as ALLOC_STORE_GEN_AUDIT_FLAGS");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocated, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(UNITS_ALLOCATED,0) as UNITS_ALLOCATED");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocated, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PACKS_ALLOCATED,0) as PACKS_ALLOCATED");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleType, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(CHOSEN_RULE_TYPE_ID," + ((int)eRuleType.None).ToString(CultureInfo.CurrentUICulture) + ") as CHOSEN_RULE_TYPE_ID");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleLayer, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(CHOSEN_RULE_LAYER_ID,0) as CHOSEN_RULE_LAYER_ID");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleUnits, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(CHOSEN_RULE_UNITS,0) as CHOSEN_RULE_UNITS");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRulePacks, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(CHOSEN_RULE_PACKS,0) as CHOSEN_RULE_PACKS");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByAuto, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(UNITS_ALLOCATED_BY_AUTO,0) as UNITS_ALLOCATED_BY_AUTO");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByAuto, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PACKS_ALLOCATED_BY_AUTO,0) as PACKS_ALLOCATED_BY_AUTO");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByRule, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(UNITS_ALLOCATED_BY_RULE,0) as UNITS_ALLOCATED_BY_RULE");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByRule, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PACKS_ALLOCATED_BY_RULE,0) as PACKS_ALLOCATED_BY_RULE");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.NeedDay, out variableIDX))
//                        {
//                            sqlCommand.Append(",NEED_DAY");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitNeedBefore, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(UNIT_NEED_BEFORE,0) as UNIT_NEED_BEFORE");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitPlanBefore, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PERCENT_NEED_BEFORE,0) as PERCENT_NEED_BEFORE");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Minimum, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(MINIMUM,0) as MINIMUM");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Maximum, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(MAXIMUM,0) as MAXIMUM");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PrimaryMaximum, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PRIMARY_MAX,0) as PRIMARY_MAX");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.DetailAuditFlags, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(ALLOC_STORE_DET_AUDIT_FLAGS,0) as ALLOC_STORE_DET_AUDIT_FLAGS");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipStatusFlags, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(SHIPPING_STATUS_FLAGS,0) as SHIPPING_STATUS_FLAGS");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsShipped, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(UNITS_SHIPPED,0) as UNITS_SHIPPED");
//                        }
//                        if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksShipped, out variableIDX))
//                        {
//                            sqlCommand.Append(",COALESCE(PACKSS_SHIPPED,0) as PACKS_SHIPPED");
//                        }
//                        sqlCommand.Append(" from ");
//                        sqlCommand.Append(_VariableModel.NormalizedTableName);
//                        sqlCommand.Append(" where ");
//                        sqlCommand.Append(aDatabaseBinKey.SQL_WhereClauseSegment);
//                        sqlCommand.Append(" order by ");
//                        for (int i = 0; i < aDatabaseBinKey.Count; i++)
//                        {
//                            if (i > 0)
//                            {
//                                sqlCommand.Append(",");
//                            }
//                            sqlCommand.Append(aDatabaseBinKey.GetKeyWord(i));
//                        }
//                        dt = aDBA.ExecuteSQLQuery(sqlCommand.ToString(), "READ_STORE_VARIABLES");
//                        int j=0;
//                        if (dt.Rows.Count > 0)
//                        {
//                            DataRow dr = dt.Rows[j];
//                            for (int i = 0; i < aDatabaseBinKey.Count; i++)
//                            {
//                                thisKey[i] = Convert.ToInt32(dr[aDatabaseBinKey.GetKeyWord(i)]);
//                            }
//                            thisDatabaseBinKey = (T)aDatabaseBinKey.CreateNewInstance(thisKey);
//                            while (j < dt.Rows.Count)
//                            {
//                                currentDatabaseBinKey = thisDatabaseBinKey;
//                                bool continueLoop = true;
//                                while (continueLoop == true)
//                                {
//                                    continueLoop = false;
//                                    if (j < dt.Rows.Count)
//                                    {
//                                        dr = dt.Rows[j];
//                                        for (int i = 0; i < aDatabaseBinKey.Count; i++)
//                                        {
//                                            thisKey[i] = Convert.ToInt32(dr[aDatabaseBinKey.GetKeyValue(i)]);
//                                        }
//                                        thisDatabaseBinKey = (T)thisDatabaseBinKey.CreateNewInstance(thisKey);
//                                        if (thisDatabaseBinKey.Equals(currentDatabaseBinKey)) // must use the value "Equals" rather than the reference "=="
//                                        {
//                                            continueLoop = true;
//                                            svvc = this.ConvertAllocationNormalizedTable(dr, svvc);
//                                        }
//                                        storeVariableDataList.Add(new StoreVariableData<T>(true, status, currentDatabaseBinKey, svvc));
//                                        j++;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return storeVariableDataList;
//        }
//        #endregion Read StoreVariableVectorContainer

//        #region Convert
//        /// <summary>
//        /// Converts prior AllocaitonVersion Bin Tables (StoreAllocationQuickRequest) to StorVariableVectorArray
//        /// </summary>
//        /// <param name="aStoreAllocationQuickRequest"></param>
//        /// <returns></returns>
//        private StoreVariableVectorContainer ConvertPriorVersions(StoreAllocationQuickRequest aStoreAllocationQuickRequest)
//        {
//            StoreVariableVectorContainer svvc = new StoreVariableVectorContainer();
//            int maxStoreRID = MIDStorageTypeInfo.GetStoreMaxRID(1) + 1;
//            eMIDVariableModelType variableModelType =
//                (eMIDVariableModelType)_VariableModel.VariableModelID;
//            short variableIDX;
//            double powerOf10;

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityExceedByPercent, out variableIDX))
//            {
//                powerOf10 =
//                    Math.Pow(10, _VariableModel.GetVariableInfo(variableIDX).DecimalPrecision);
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                        (Int64)(aStoreAllocationQuickRequest.GetStoreExceedCapacityByPercent(storeRID) * powerOf10));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityUnits, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)(aStoreAllocationQuickRequest.GetStoreCapacity(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleLayer, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreChosenRuleLayerID(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleType, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreChosenRule(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleUnits, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreChosenRuleQty(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRulePacks, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreChosenRuleQty(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.DetailAuditFlags, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreDetailAudit(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GeneralAuditFlags, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)(aStoreAllocationQuickRequest.GetStoreGeneralAudit(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GradeIndex, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)(aStoreAllocationQuickRequest.GetStoreGradeIndex(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Maximum, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreMaximum(storeRID)));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Minimum, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)(aStoreAllocationQuickRequest.GetStoreMinimum(storeRID)));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.NeedDay, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    DateTime date = aStoreAllocationQuickRequest.GetStoreNeedDay(storeRID);
//                    int YYYYDDD = date.Year * 1000 + date.DayOfYear;
//                    SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, YYYYDDD);
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)sqlDate.TimeID);
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PrimaryMaximum, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)aStoreAllocationQuickRequest.GetStorePrimaryMax(storeRID));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipStatusFlags, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)aStoreAllocationQuickRequest.GetStoreShippingStatus(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipToDay, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    DateTime date = aStoreAllocationQuickRequest.GetStoreShipToDay(storeRID);
//                    int YYYYDDD = date.Year * 1000 + date.DayOfYear;
//                    SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, YYYYDDD);
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)sqlDate.TimeID);
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitNeedBefore, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreNeed(storeRID));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitPlanBefore, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    double percentNeed = aStoreAllocationQuickRequest.GetStorePercentNeed(storeRID);
//                    if (percentNeed != 0)
//                    {
//                        svvc.SetStoreVariableValue
//                           (storeRID,
//                            variableModelType,
//                            ref svvc,
//                            variableIDX,
//                            (Int64)((aStoreAllocationQuickRequest.GetStoreNeed(storeRID) * 100 / percentNeed) + .5));
//                    }
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocated, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocated(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocated, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref svvc,
//                         variableIDX,
//                         (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocated(storeRID));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByAuto, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocatedByAuto(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByAuto, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocatedByAuto(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByRule, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocatedByRule(storeRID));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByRule, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyAllocatedByRule(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsShipped, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyShipped(storeRID));
//                }
//            }

//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksShipped, out variableIDX))
//            {
//                for (int storeRID = 1; storeRID < maxStoreRID; storeRID++)
//                {
//                    svvc.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref svvc,
//                        variableIDX,
//                        (Int64)aStoreAllocationQuickRequest.GetStoreQtyShipped(storeRID));
//                }
//            }
//            return svvc;
//        }

//        /// <summary>
//        /// Converts Normalized Store Allocation to StoreVariableVectorContainer
//        /// </summary>
//        /// <param name="aDataRow"></param>
//        /// <param name="aStoreVariableVectorContainer"></param>
//        /// <returns></returns>
//        public StoreVariableVectorContainer ConvertAllocationNormalizedTable(
//            DataRow aDataRow,
//            StoreVariableVectorContainer aStoreVariableVectorContainer)
//        {
//            eMIDVariableModelType variableModelType = (eMIDVariableModelType)_VariableModel.VariableModelID;
//            short variableIDX;
//            int storeRID = Convert.ToInt32(aDataRow["ST_RID"], CultureInfo.CurrentUICulture);
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipToDay, out variableIDX))
//            {
//                if (aDataRow["SHIP_DAY"] != DBNull.Value)
//                {
//                    DateTime date = Convert.ToDateTime(aDataRow["SHIP_DAY"], CultureInfo.CurrentUICulture);
//                    int YYYYDDD = date.Year * 1000 + date.DayOfYear;
//                    SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, YYYYDDD);
//                    aStoreVariableVectorContainer.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref aStoreVariableVectorContainer,
//                        variableIDX,
//                        (Int64)sqlDate.TimeID);
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GradeIndex, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["STORE_GRADE_INDEX"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityUnits, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["STORE_CAPACITY"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.CapacityExceedByPercent, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["EXCEED_CAPACITY_PERCENT"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.GeneralAuditFlags, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["ALLOC_STORE_GEN_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocated, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["UNITS_ALLOCATED"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocated, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PACKS_ALLOCATED"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleType, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["CHOSEN_RULE_TYPE_ID"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleLayer, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["CHOSEN_RULE_LAYER_ID"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRuleUnits, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["CHOSEN_RULE_UNITS"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ChosenRulePacks, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["CHOSEN_RULE_PACKS"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByAuto, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["UNITS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByAuto, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PACKS_ALLOCATED_BY_AUTO"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsAllocatedByRule, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["UNITS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByRule, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PACKS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksAllocatedByRule, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PACKS_ALLOCATED_BY_RULE"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.NeedDay, out variableIDX))
//            {
//                if (aDataRow["NEED_DAY"] != DBNull.Value)
//                {
//                    DateTime date = Convert.ToDateTime(aDataRow["NEED_DAY"], CultureInfo.CurrentUICulture);
//                    int YYYYDDD = date.Year * 1000 + date.DayOfYear;
//                    SQL_TimeID sqlDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, YYYYDDD);
//                    aStoreVariableVectorContainer.SetStoreVariableValue
//                       (storeRID,
//                        variableModelType,
//                        ref aStoreVariableVectorContainer,
//                        variableIDX,
//                       (Int64)sqlDate.TimeID);
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitNeedBefore, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["UNIT_NEED_BEFORE"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitPlanBefore, out variableIDX))
//            {
//                double percentNeedBefore = Convert.ToDouble(aDataRow["PERCENT_NEED_BEFORE"], CultureInfo.CurrentUICulture);
//                if (percentNeedBefore != 0)
//                {
//                    Int64 unitNeedBefore = aStoreVariableVectorContainer.GetStoreVariableValue(storeRID, variableIDX);
//                    aStoreVariableVectorContainer.SetStoreVariableValue
//                        (storeRID,
//                         variableModelType,
//                         ref aStoreVariableVectorContainer,
//                         variableIDX,
//                         (Int64)((unitNeedBefore / percentNeedBefore) + .5));
//                }
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Minimum, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["MINIMUM"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.Maximum, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["MAXIMUM"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PrimaryMaximum, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PRIMARY_MAX"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.DetailAuditFlags, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["ALLOC_STORE_DET_AUDIT_FLAGS"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.UnitsShipped, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["UNITS_SHIPPED"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.PacksShipped, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["PACKS_SHIPPED"], CultureInfo.CurrentUICulture));
//            }
//            if (_VariableModel.TryGetVariableIDX((int)eAllocationDatabaseStoreVariables.ShipStatusFlags, out variableIDX))
//            {
//                aStoreVariableVectorContainer.SetStoreVariableValue
//                    (storeRID,
//                     variableModelType,
//                     ref aStoreVariableVectorContainer,
//                     variableIDX,
//                     Convert.ToInt64(aDataRow["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture));
//            }
//            return aStoreVariableVectorContainer;
//        }
//        #endregion Convert

//        // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//        #region Delete StoreVariableVector
//        /// <summary>
//        /// Deletes the store variables from the database associated with the given key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database key to delete</param>
//        public void DeleteStoreVariableVectorContainer(
//            DatabaseAccess aDBA,
//            T aDatabaseBinKey)
//        {
//            StringBuilder sqlCommand = new StringBuilder("Delete from ");
//            sqlCommand.Append(_VariableModel.BinTableName);
//            sqlCommand.Append(" where ");
//            sqlCommand.Append(aDatabaseBinKey.SQL_WhereClauseSegment);
//            aDBA.ExecuteSQLQuery(sqlCommand.ToString(), "DELETE_STORE_VARIABLES");
//        }
//        #endregion Delete StoreVariableVector
//        // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4

//        #region Write StoreVariableVectorContainer
//        /// <summary>
//        /// Writes a store variable vector array to the table BIN on the database for the given Key parameters
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aStoreVariableDataDictionary">The store container to write</param>
//        /// <param name="aKeysToWrite">The Keys to write.</param>
//        /// <par
//        public void WriteStoreVariableVectorContainer(
//            DatabaseAccess aDBA,                                          // TT#467 Change Store Variable Container Enqueue -- Part 2
//            StoreVariableDataDictionary<T> aStoreVariableDataDictionary, // TT#467 Change Store Variable Container Enqueue -- Part 2
//            T[] aKeysToWrite)                                // TT#467 Change Store Variable Container Enqueue -- Part 2
//        {
//            try
//            {
//                // begin TT#467 Change Store Variable Container Enqueue -- Part 2
//                //foreach (StoreVariableData<T> svd in aStoreVariableDataDictionary.Values)
//                //{
//                StoreVariableData<T> svd;
//                foreach (T dbk in aKeysToWrite)
//                {
//                    if (aStoreVariableDataDictionary.TryGetValue(dbk,out svd))
//                    {
//                        // end TT#467 Change Store Variable Container Enqueue -- Part 2
//                        if (svd.DatabaseBinKey.DatabaseBinKeyType == this.VariableModel.DatabaseBinKeyType)
//                        {

//                            MIDDbParameter[] inputParms = new MIDDbParameter[svd.DatabaseBinKey.Count + 2];
//                            Array.Copy(svd.DatabaseBinKey.SQL_UpdtProcedureInputParms, 0, inputParms, 0, svd.DatabaseBinKey.Count);
//                            //byte[] serializedArray = svd.StoreVariableVectorContainer.CompressVectorContainer();
//                            byte[] serializedArray = svd.StoreVariableVectorContainer.ToArray();
//                            if (serializedArray.LongLength > int.MaxValue)
//                            {
//                                throw new Exception("Serialized Array exceeded 2 GB maximum length");
//                            }
//                            inputParms[svd.DatabaseBinKey.Count + 0] = new MIDDbParameter("USTAT", svd.Status, eDbType.tinyint, eParameterDirection.Input);
//                            inputParms[svd.DatabaseBinKey.Count + 1] = new MIDDbParameter("STORE_DATA", serializedArray, eDbType.VarBinary, eParameterDirection.Input);
//                            if (aDBA.UpdateStoredProcedure(_VariableModel.UpdateBinStoredProcedureName, inputParms, null) < 0)
//                            {
//                                throw new Exception("Database error writing  Serialized Structure table '" + _VariableModel.BinTableName + "'");
//                            }
//                        }
//                    }  // TT#467 Change Store Variable Container Enqueue -- Part 2
//                }
//            }
//            catch (SqlException se)
//            {
//                throw;
//            }
//        }
//        #endregion Write StoreVariableVectorContainer

//        #endregion Methods
//    }
//    #endregion DatabaseBinTable

//    #region HistoryDayDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the History Day Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class HistoryDayDatabaseBin
//    {
//        private DatabaseBinTable<HistoryDatabaseBinKey> _historyBin;
//        /// <summary>
//        /// constructor for the History Day Variables Database BIN
//        /// </summary>
//        public HistoryDayDatabaseBin()
//        {
//            _historyBin = new DatabaseBinTable<HistoryDatabaseBinKey>("STORE_DAY_HISTORY_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _historyBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _historyBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            //get { return _historyBin.NormalizedTableName; }         // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _historyBin.UpdateBinStoredProcedureName; }  // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _historyBin.VariableModel; }
//        }
//        //private static object _historyDayBinReadLock = new object(); // TT#707 - JEllis - Container not thread safe
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>StoreVariableDictioinary containing the history variables read</returns>
//        public List<StoreVariableData<HistoryDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HistoryDatabaseBinKey aDatabaseBinKey)
//        {
//            return _historyBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of History Database Bin Keys to be written.</param>
//        public void WriteStoreVariables(                                               // TT#467 Change Store Container Enqueue -- Part 2
//            DatabaseAccess aDBA,                                                              // TT#467 Change Store Container Enqueue -- Part 2
//            StoreVariableDataDictionary<HistoryDatabaseBinKey> aStoreVariableDataDictionary,  // TT#467 Change Store Container Enqueue -- Part 2
//            HistoryDatabaseBinKey[] aKeysToWrite)  // TT#467 Change Store Container Enqueue -- Part 2
//        {
//            _historyBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,          // TT#467 Change Store Container Enqueue -- Part 2
//                aKeysToWrite); // TT#467 Change Store Container Enqueue -- Part 2
//        }
//    }
//    #endregion HistoryDayDatabaseBin

//    #region HistoryWeekDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the History Day Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class HistoryWeekDatabaseBin
//    {
//        private DatabaseBinTable<HistoryDatabaseBinKey> _historyBin;
//        /// <summary>
//        /// constructor for the History Day Variables Database BIN
//        /// </summary>
//        public HistoryWeekDatabaseBin()
//        {
//            _historyBin = new DatabaseBinTable<HistoryDatabaseBinKey>("STORE_WEEK_HISTORY_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _historyBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _historyBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            // get { return _historyBin.NormalizedTableName; }       // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _historyBin.UpdateBinStoredProcedureName; } // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _historyBin.VariableModel; }
//        }
//        //private static object _historyWeekBinReadLock = new object(); //  TT#707 - JEllis - Container not thread safe (Part 2)
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData having the given key</returns>
//        public List<StoreVariableData<HistoryDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HistoryDatabaseBinKey aDatabaseBinKey)
//        {
//            return _historyBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of the keys to be written</param>
//        public void WriteStoreVariables(                                                  // TT#467 Store Container Enqueue changes -- part 2
//            DatabaseAccess aDBA,                                                                 // TT#467 Store Container Enqueue changes -- part 2
//            StoreVariableDataDictionary<HistoryDatabaseBinKey> aStoreVariableDataDictionary,     // TT#467 Store Container Enqueue changes -- part 2 
//            HistoryDatabaseBinKey[] aKeysToWrite)                                                // TT#467 Store Container Enqueue changes -- part 2
//        {
//            _historyBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,   // TT#467 Store Container Enqueue changes -- part 2
//                aKeysToWrite);                  // TT#467 Store Container enqueue changes -- part 2
//        }
//    }
//    #endregion HistoryWeekDatabaseBin

//    #region TotalAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Total Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class TotalAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderDatabaseBinKey> _totalAllocationBin;
//        /// <summary>
//        /// constructor for the Total Allocation Variables Database BIN
//        /// </summary>
//        public TotalAllocationDatabaseBin()
//        {
//            _totalAllocationBin = new DatabaseBinTable<HeaderDatabaseBinKey>("TOTAL_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _totalAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _totalAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            // get { return _totalAllocationBin.NormalizedTableName; }       // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _totalAllocationBin.UpdateBinStoredProcedureName; } // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public  VariableModel VariableModel
//        {
//            get { return _totalAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>SList of StoreVariableData having the given key</returns>
//        public List<StoreVariableData<HeaderDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderDatabaseBinKey aDatabaseBinKey)
//        {
//            return _totalAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Keys to write</param>
//        public void WriteStoreVariables(                                               // TT#467 Store Container Enqueue changes -- Part 2
//            DatabaseAccess aDBA,                                                              // TT#467 Store Container Enqueue changes -- Part 2 
//            StoreVariableDataDictionary<HeaderDatabaseBinKey> aStoreVariableDataDictionary,   // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderDatabaseBinKey[] aKeysToWrite)
//        {
//            _totalAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);    // TT#467 Store Container Enqueue changes -- Part 2   
//        }
//    }
//    #endregion TotalAllocationDatabaseBin

//    #region DetailAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Detail Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class DetailAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderDatabaseBinKey> _detailAllocationBin;
//        /// <summary>
//        /// constructor for the Detail Allocation Variables Database BIN
//        /// </summary>
//        public DetailAllocationDatabaseBin()
//        {
//            _detailAllocationBin = new DatabaseBinTable<HeaderDatabaseBinKey>("DETAIL_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _detailAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _detailAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            //get { return _detailAllocationBin.NormalizedTableName; }       // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _detailAllocationBin.UpdateBinStoredProcedureName; } // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _detailAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData having the given key</returns>
//        public List<StoreVariableData<HeaderDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderDatabaseBinKey aDatabaseBinKey)
//        {
//            return _detailAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        public void WriteStoreVariables(                                              // TT#467 Store Container Enqueue changes -- Part 2
//            DatabaseAccess aDBA,                                                             // TT#467 Store Container Enqueue changes -- Part 2
//            StoreVariableDataDictionary<HeaderDatabaseBinKey> aStoreVariableDataDictionary,  // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderDatabaseBinKey[] aKeysToWrite)         // TT#467 Store Container Enqueue changes -- Part 2
//        {
//            _detailAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,       // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);                      // TT#467 Store Container Enqueue changes -- Part 2
//        }
//    }
//    #endregion DetailAllocationDatabaseBin

//    #region BulkAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Bulk Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class BulkAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderDatabaseBinKey> _bulkAllocationBin;
//        /// <summary>
//        /// constructor for the Bulk Allocation Variables Database BIN
//        /// </summary>
//        public BulkAllocationDatabaseBin()
//        {
//            _bulkAllocationBin = new DatabaseBinTable<HeaderDatabaseBinKey>("BULK_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _bulkAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _bulkAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            //get { return _bulkAllocationBin.NormalizedTableName; }        // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _bulkAllocationBin.UpdateBinStoredProcedureName; } // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _bulkAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData having the given key</returns>
//        public List<StoreVariableData<HeaderDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderDatabaseBinKey aDatabaseBinKey)
//        {
//            return _bulkAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Keys to write to the database</param>
//        public void WriteStoreVariables(
//            DatabaseAccess aDBA,                                                            // TT#467 Store Container Enqueue changes -- Part 2
//            StoreVariableDataDictionary<HeaderDatabaseBinKey> aStoreVariableDataDictionary,  // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderDatabaseBinKey[] aKeysToWrite)         // TT#467 Store Container Enqueue changes -- Part 2
//        {
//            _bulkAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);                // TT#467 Store Container Enqueue changes -- Part 2 
//        }
//    }
//    #endregion BulkAllocationDatabaseBin

//    #region PackAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Pack Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class PackAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderPackDatabaseBinKey> _packAllocationBin;
//        /// <summary>
//        /// constructor for the Pack Allocation Variables Database BIN
//        /// </summary>
//        public PackAllocationDatabaseBin()
//        {
//            _packAllocationBin = new DatabaseBinTable<HeaderPackDatabaseBinKey>("PACK_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _packAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _packAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            //get { return _packAllocationBin.NormalizedTableName; }         // TT#370 Build Packs Enhancement (Correction to this property)
//            get { return _packAllocationBin.UpdateBinStoredProcedureName; }  // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _packAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData with the given key</returns>
//        public List<StoreVariableData<HeaderPackDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderPackDatabaseBinKey aDatabaseBinKey)
//        {
//            return _packAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of the Header Pack Database Bin Keys to write</param>
//        public void WriteStoreVariables(                                                  // TT#467 Store Container Enqueue changes -- Part 2
//            DatabaseAccess aDBA,                                                                 // TT#467 Store Container Enqueue changes -- Part 2
//            StoreVariableDataDictionary<HeaderPackDatabaseBinKey> aStoreVariableDataDictionary,  // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderPackDatabaseBinKey[] aKeysToWrite)                                             // TT#467 Store Container Enqueue changes -- Part 2
//        {
//            _packAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);                // TT#467 Store Container Enqueue changes -- Part 2 
//        }
//    }
//    #endregion PackAllocationDatabaseBin

//    #region ColorAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Bulk Color Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class ColorAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderColorDatabaseBinKey> _colorAllocationBin;
//        /// <summary>
//        /// constructor for the Bulk Color Allocation Variables Database BIN
//        /// </summary>
//        public ColorAllocationDatabaseBin()
//        {
//            _colorAllocationBin = new DatabaseBinTable<HeaderColorDatabaseBinKey>("COLOR_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _colorAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _colorAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _colorAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _colorAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData havingin the given key</returns>
//        public List<StoreVariableData<HeaderColorDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderColorDatabaseBinKey aDatabaseBinKey)
//        {
//            return _colorAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Keys to write.</param>
//        public void WriteStoreVariables(                                                  // TT#467 Store Container Enqueue changes -- Part 2
//            DatabaseAccess aDBA,                                                                 // TT#467 Store Container Enqueue changes -- Part 2    
//            StoreVariableDataDictionary<HeaderColorDatabaseBinKey> aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderColorDatabaseBinKey[] aKeysToWrite)                                            // TT#467 Store Container Enqueue changes -- Part 2
//        {
//            _colorAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);                // TT#467 Store Container Enqueue changes -- Part 2
//        }
//    }
//    #endregion ColorAllocationDatabaseBin

//    #region SizeAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Bulk Color Size Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class SizeAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderColorSizeDatabaseBinKey> _sizeAllocationBin;
//        /// <summary>
//        /// constructor for the Bulk Color Size Allocation Variables Database BIN
//        /// </summary>
//        public SizeAllocationDatabaseBin()
//        {
//            _sizeAllocationBin = new DatabaseBinTable<HeaderColorSizeDatabaseBinKey>("COLOR_SIZE_ALLOCATION_BIN");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _sizeAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _sizeAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            //get { return _sizeAllocationBin.NormalizedTableName; }         // TT#370 Build Packs Enhancement (Correction to this property) 
//            get { return _sizeAllocationBin.UpdateBinStoredProcedureName; }  // TT#370 Build Packs Enhancement (Correction to this property)
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _sizeAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given ColorSizeDatabaseBinKey</returns>
//        public List<StoreVariableData<HeaderColorSizeDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderColorSizeDatabaseBinKey aDatabaseBinKey)
//        {
//            return _sizeAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of keys to write</param>
//        public void WriteStoreVariables(                                                      // TT#467 Store Container Enqueue changes -- Part 2
//            DatabaseAccess aDBA,                                                                     // TT#467 Store Container Enqueue changes -- Part 2
//            StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey> aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//            HeaderColorSizeDatabaseBinKey[] aKeysToWrite)                                            // TT#467 Store Container Enqueue changes -- Part 2
//        {
//            _sizeAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary, // TT#467 Store Container Enqueue changes -- Part 2
//                aKeysToWrite);                // TT#467 Store Container Enqueue changes -- Part 2
//        }
//    }
//    #endregion SizeAllocationDatabaseBin

//    // begin TT#370 Build Packs Enhancement
//    #region ArchiveSummaryAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Archive Summary Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class ArchiveSummaryAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderSummaryDatabaseBinKey> _archiveSummaryAllocationBin;
//        /// <summary>
//        /// constructor for the Archive Summary Allocation Variables Database BIN
//        /// </summary>
//        public ArchiveSummaryAllocationDatabaseBin()
//        {
//            _archiveSummaryAllocationBin = new DatabaseBinTable<HeaderSummaryDatabaseBinKey>("ARCHIVE_SUMMARY_ALLOCATION");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _archiveSummaryAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _archiveSummaryAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _archiveSummaryAllocationBin.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _archiveSummaryAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given ColorSizeDatabaseBinKey</returns>
//        public List<StoreVariableData<HeaderSummaryDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderSummaryDatabaseBinKey aDatabaseBinKey)
//        {
//            return _archiveSummaryAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of Header summary database bin keys to write</param>
//        public void WriteStoreVariables(
//            DatabaseAccess aDBA, 
//            StoreVariableDataDictionary<HeaderSummaryDatabaseBinKey> aStoreVariableDataDictionary,
//            HeaderSummaryDatabaseBinKey[] aKeysToWrite)     // TT#467 Store Container Enqueue changes
//        {
//            _archiveSummaryAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,
//                aKeysToWrite);              // TT#467 Store Container Enqueue changes
//        }
//    }
//    #endregion ArchiveSummaryAllocationDatabaseBin

//    #region ArchivePackAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Archive Pack Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class ArchivePackAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderPackDatabaseBinKey> _archivePackAllocationBin;
//        /// <summary>
//        /// constructor for the Archive Pack Allocation Variables Database BIN
//        /// </summary>
//        public ArchivePackAllocationDatabaseBin()
//        {
//            _archivePackAllocationBin = new DatabaseBinTable<HeaderPackDatabaseBinKey>("ARCHIVE_PACK_ALLOCATION");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _archivePackAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _archivePackAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _archivePackAllocationBin.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _archivePackAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given ColorSizeDatabaseBinKey</returns>
//        public List<StoreVariableData<HeaderPackDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderPackDatabaseBinKey aDatabaseBinKey)
//        {
//            return _archivePackAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of Header Pack Database Bin keys to write</param>
//        public void WriteStoreVariables(
//            DatabaseAccess aDBA, 
//            StoreVariableDataDictionary<HeaderPackDatabaseBinKey> aStoreVariableDataDictionary,
//            HeaderPackDatabaseBinKey[] aKeysToWrite)   // TT#467 Store Container Enqueue changes
//        {
//            _archivePackAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,
//                aKeysToWrite);
//        }
//    }
//    #endregion ArchivePackAllocationDatabaseBin

//    #region ArchiveColorAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Archive Pack Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class ArchiveColorAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderColorDatabaseBinKey> _archiveColorAllocationBin;
//        /// <summary>
//        /// constructor for the Archive Color Allocation Variables Database BIN
//        /// </summary>
//        public ArchiveColorAllocationDatabaseBin()
//        {
//            _archiveColorAllocationBin = new DatabaseBinTable<HeaderColorDatabaseBinKey>("ARCHIVE_BULK_COLOR_ALLOCATION");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _archiveColorAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _archiveColorAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _archiveColorAllocationBin.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _archiveColorAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given ColorSizeDatabaseBinKey</returns>
//        public List<StoreVariableData<HeaderColorDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderColorDatabaseBinKey aDatabaseBinKey)
//        {
//            return _archiveColorAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">List of the header color keys to write</param>
//        public  void WriteStoreVariables(
//            DatabaseAccess aDBA, 
//            StoreVariableDataDictionary<HeaderColorDatabaseBinKey> aStoreVariableDataDictionary,
//            HeaderColorDatabaseBinKey[] aKeysToWrite)  // TT#467 Store Container Enqueue changes
//        {
//                _archiveColorAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,
//                aKeysToWrite);   // TT#467 Store Container Enqueue changes
//        }
//    }
//    #endregion ArchiveColorAllocationDatabaseBin

//    #region ArchiveColorSizeAllocationDatabaseBin
//    // removed "static" from this class  // TT#707 - JEllis - Container not thread safe (part 2)
//    /// <summary>
//    /// Access to/from the Archive Pack Allocation  Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class ArchiveColorSizeAllocationDatabaseBin
//    {
//        private DatabaseBinTable<HeaderColorSizeDatabaseBinKey> _archiveColorSizeAllocationBin;
//        /// <summary>
//        /// constructor for the Archive Color Allocation Variables Database BIN
//        /// </summary>
//        public ArchiveColorSizeAllocationDatabaseBin()
//        {
//            _archiveColorSizeAllocationBin = new DatabaseBinTable<HeaderColorSizeDatabaseBinKey>("ARCHIVE_BULK_SIZE_ALLOCATION");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _archiveColorSizeAllocationBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _archiveColorSizeAllocationBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _archiveColorSizeAllocationBin.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _archiveColorSizeAllocationBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given ColorSizeDatabaseBinKey</returns>
//        public List<StoreVariableData<HeaderColorSizeDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            HeaderColorSizeDatabaseBinKey aDatabaseBinKey)
//        {
//            return _archiveColorSizeAllocationBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">An array of the Header, color, size keys to write.</param>
//        public void WriteStoreVariables(
//            DatabaseAccess aDBA, 
//            StoreVariableDataDictionary<HeaderColorSizeDatabaseBinKey> aStoreVariableDataDictionary,
//            HeaderColorSizeDatabaseBinKey[] aKeysToWrite)   // TT#467 Store Container Enqueue changes
//        {
//            _archiveColorSizeAllocationBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aStoreVariableDataDictionary,
//                aKeysToWrite);   // TT#467 Store Container Enqueue changes
//        }
//    }
//    #endregion ArchiveColorSizeAllocationDatabaseBin
//    //
//    //
//    // end TT#370 Build Packs Enhancement

//    // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//    #region VswReverseOnhandDatabaseBin
//    /// <summary>
//    /// Access to/from the VSW Reverse Onhand Variable Database Bin
//    /// </summary>
//    [Serializable]
//    public class VswReverseOnhandDatabaseBin
//    {
//        private DatabaseBinTable<VswReverseOnhandDatabaseBinKey> _vswReverseOnhandBin;
//        /// <summary>
//        /// constructor for the VSW Reverse Onhand Variables Database BIN
//        /// </summary>
//        public VswReverseOnhandDatabaseBin()
//        {
//            _vswReverseOnhandBin = new DatabaseBinTable<VswReverseOnhandDatabaseBinKey>("VSW_REVERSE_ONHAND");
//        }
//        /// <summary>
//        /// Gets the BinTableName 
//        /// </summary>
//        public string BinTableName
//        {
//            get { return _vswReverseOnhandBin.BinTableName; }
//        }
//        /// <summary>
//        /// Gets the NormalizedTableName IF one is associated with the BinTableName
//        /// </summary>
//        public string NormalizedTableName
//        {
//            get { return _vswReverseOnhandBin.NormalizedTableName; }
//        }
//        /// <summary>
//        /// Gets the Update Bin Stored Procedure Name for the Bin Table
//        /// </summary>
//        public string UpdateBinStoredProcedureName
//        {
//            get { return _vswReverseOnhandBin.UpdateBinStoredProcedureName; }
//        }
//        /// <summary>
//        /// Gets the Variable Model Base assoociated with the BinTableName
//        /// </summary>
//        public VariableModel VariableModel
//        {
//            get { return _vswReverseOnhandBin.VariableModel; }
//        }
//        /// <summary>
//        /// Reads the store variables for the given database Bin key
//        /// </summary>
//        /// <param name="aDBA">Database Access</param>
//        /// <param name="aDatabaseBinKey">Database Bin Key</param>
//        /// <returns>List of StoreVariableData items with the given VswReverseOnhandDatabaseBinKey</returns>
//        public List<StoreVariableData<VswReverseOnhandDatabaseBinKey>>
//            ReadStoreVariables
//            (DatabaseAccess aDBA,
//            VswReverseOnhandDatabaseBinKey aDatabaseBinKey)
//        {
//            return _vswReverseOnhandBin.ReadStoreVariables
//                (aDBA,
//                aDatabaseBinKey);
//        }
//        /// <summary>
//        /// Writes the store variables within the given dictionary to the Bin table
//        /// </summary>
//        /// <param name="aDBA">Database Access (an open connection must already be present)</param>
//        /// <param name="aStoreVariableDataDictionary">The Store Variable Dictionary containing the store variables to be written</param>
//        /// <param name="aKeysToWrite">Array of Header summary database bin keys to write</param>
//        public void WriteStoreVariables(
//            DatabaseAccess aDBA,
//            StoreVariableDataDictionary<VswReverseOnhandDatabaseBinKey> aVswReverseOnhandDataDictionary,
//            VswReverseOnhandDatabaseBinKey[] aKeysToWrite)
//        {
//            _vswReverseOnhandBin.WriteStoreVariableVectorContainer
//                (aDBA,
//                aVswReverseOnhandDataDictionary,
//                aKeysToWrite);  
//        }
//        // begin TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//        public void DeleteStoreVariables(
//            DatabaseAccess aDBA,
//            VswReverseOnhandDatabaseBinKey aDatabaseBinKey)
//        {
//            _vswReverseOnhandBin.DeleteStoreVariableVectorContainer(aDBA, aDatabaseBinKey);
//        }
//        // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//    }
//    #endregion VswReverseOnhandDatabaseBin
//    // end TT#2225 - JEllis - AnF VSW FWOS Enhancement pt 4
//}
