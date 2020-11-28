// Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
//
// Too many changes to mark. Compare for differences
// Also removed old commend code for readability
//
// End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;



namespace MIDRetail.Data
{
    public partial class ROExtractData : DataLayer
    {
        private DataTable _dtValues = null;
        private DataTable _dtVariableValues = null;

        public ROExtractData(string aConnectionString)
            : base(aConnectionString)
        {
            try
            {

            }
            catch
            {
                throw;
            }
        }

        public void Initialize()
        {
            _dtValues = null;
            _dtVariableValues = null;
        }

        public bool RO_Extract_Exists()
        {
            try
            {
                return (StoredProcedures.RO_Database_Exists.ReadRecordCount(_dba) > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Stores_Data_Insert(StoreProfile sp)
        {
            try
            {
                DataRow drData;
                if (_dtValues == null)
                {
                    _dtValues = GetTableSchemaFromTableName(Include.DBROExtractStoresDataTable);
                }

                drData = _dtValues.NewRow();
                _dtValues.Rows.Add(drData);

                string shipDate = null;
                drData["StoreDisplay"] = sp.Text;
                drData["ID"] = sp.StoreId;
                drData["Name"] = sp.StoreName;
                drData["Description"] = sp.StoreDescription;
                drData["City"] = sp.City;
                drData["State"] = sp.State;
                drData["SellingSquareFeet"] = sp.SellingSqFt;
                drData["SellingOpenDate"] = sp.SellingOpenDt.ToShortDateString();
                drData["SellingCloseDate"] = sp.SellingCloseDt.ToShortDateString();
                drData["StockOpenDate"] = sp.StockOpenDt.ToShortDateString();
                drData["StockCloseDate"] = sp.StockCloseDt.ToShortDateString();
                drData["ActiveIndicator"] = sp.ActiveInd.ToString();
                drData["Status"] = sp.Status.ToString();
                drData["LeadTime"] = sp.LeadTime;
                if (sp.ShipOnMonday)
                {
                    shipDate += "M ";
                }
                if (sp.ShipOnTuesday)
                {
                    shipDate += "Tu ";
                }
                if (sp.ShipOnWednesday)
                {
                    shipDate += "W ";
                }
                if (sp.ShipOnThursday)
                {
                    shipDate += "Th ";
                }
                if (sp.ShipOnFriday)
                {
                    shipDate += "F ";
                }
                if (sp.ShipOnSaturday)
                {
                    shipDate += "Sa ";
                }
                if (sp.ShipOnSunday)
                {
                    shipDate += "Su ";
                }
                drData["ShipDate"] = shipDate;
                drData["VSWID"] = sp.IMO_ID;

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Stores_Data_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Stores_Data_Write.Insert(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Store_Data_Delete()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Stores_Data_Delete.Delete(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Stores_Characteristics_Insert(string StoreId, string Characteristic, string Value)
        {
            try
            {
                DataRow drData;
                if (_dtValues == null)
                {
                    _dtValues = GetTableSchemaFromTableName(Include.DBROExtractStoresCharacteristicsDataTable);
                }

                drData = _dtValues.NewRow();
                _dtValues.Rows.Add(drData);

                drData["ID"] = StoreId;
                drData["Characteristic"] = Characteristic;
                drData["Value"] = Value;
                
                return;
            }
            catch
            {
                throw;
            }
        }

        public void Stores_Characteristics_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Stores_Characteristics_Write.Insert(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Stores_Characteristics_Delete(string Characteristic)
        {
            try
            {
                StoredProcedures.RO_Stores_Characteristics_Delete.Delete(_dba, Characteristic: Characteristic);
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Definition_Insert(string aHierarchyID)
        {
            try
            {
                DataRow drData;
                if (_dtValues == null)
                {
                    _dtValues = GetTableSchemaFromTableName(Include.DBROExtractHierarchyDefinitionTable);
                }

                drData = _dtValues.NewRow();
                _dtValues.Rows.Add(drData);

                drData["Type"] = "H";
                drData["Level_Number"] = 0;
                drData["Hierarchy_Level"] = aHierarchyID;

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Definition_Level_Insert(
            HierarchyLevelProfile hlp, 
            int aLevelNumber
            )
        {
            try
            {
                DataRow drData;
                if (_dtValues == null)
                {
                    _dtValues = GetTableSchemaFromTableName(Include.DBROExtractHierarchyDefinitionTable);
                }

                drData = _dtValues.NewRow();
                _dtValues.Rows.Add(drData);

                drData["Type"] = "L";
                drData["Level_Number"] = aLevelNumber;
                drData["Hierarchy_Level"] = hlp.LevelID;

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Definition_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Hierarchy_Definition_Write.Insert(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Data_Insert(
            HierarchyNodeProfile hnp, 
            string aHierarchy, 
            string aParent
            )
        {
            try
            {
                DataRow drData;
                if (_dtValues == null)
                {
                    _dtValues = GetTableSchemaFromTableName(Include.DBROExtractHierarchyDataTable);
                }

                drData = _dtValues.NewRow();
                _dtValues.Rows.Add(drData);

                drData["ProductType"] = "P";
                drData["Hierarchy"] = aHierarchy;
                drData["Parent"] = aParent;
                drData["ID"] = hnp.NodeID;
                drData["Name"] = hnp.NodeName;
                drData["Description"] = hnp.NodeDescription;
                drData["NodeDisplay"] = hnp.Text;


                return;
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Data_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Hierarchy_Data_Write.Insert(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Hierarchy_Data_Delete()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtValues != null &&
                   _dtValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Hierarchy_Data_Delete.Delete(_dba, dt: _dtValues);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Variable_Init()
        {
            try
            {
                _dtVariableValues = null;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Chain_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Version, 
            Dictionary<VariableProfile, double> values, 
            Dictionary<VariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningChainTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["TimePeriod"] = TimePeriod;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, double> val in values)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, string> val in strings)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Chain_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Chain_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Chain_Total_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Version, 
            Dictionary<TimeTotalVariableProfile, double> values,
            Dictionary<TimeTotalVariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningChainTotalTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["TimePeriod"] = TimePeriod;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, double> val in values)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, string> val in strings)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Chain_Total_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Chain_Total_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Stores_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Store, 
            string Version, 
            string Attribute,
            string AttributeSet,
            string FilterName, 
            Dictionary<VariableProfile, double> values, 
            Dictionary<VariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningStoresTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["Store"] = Store;
                drVariableValues["TimePeriod"] = TimePeriod;
                drVariableValues["Attribute"] = Attribute;
                drVariableValues["AttributeSet"] = AttributeSet;
                drVariableValues["Filter"] = FilterName;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, double> val in values)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, string> val in strings)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Stores_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Stores_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Stores_Total_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Store, 
            string Version, 
            string Attribute,
            string AttributeSet,
            string FilterName, 
            Dictionary<TimeTotalVariableProfile, double> values, 
            Dictionary<TimeTotalVariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningStoresTotalTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["Store"] = Store;
                drVariableValues["TimePeriod"] = TimePeriod;
                drVariableValues["Attribute"] = Attribute;
                drVariableValues["AttributeSet"] = AttributeSet;
                drVariableValues["Filter"] = FilterName;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, double> val in values)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, string> val in strings)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Stores_Total_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Stores_Total_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }


        public void Planning_Attributes_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Attribute, 
            string AttributeSet, 
            string Version, 
            string FilterName, 
            Dictionary<VariableProfile, double> values, 
            Dictionary<VariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningAttributesTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["Attribute"] = Attribute;
                drVariableValues["AttributeSet"] = AttributeSet;
                drVariableValues["TimePeriod"] = TimePeriod;
                drVariableValues["Filter"] = FilterName;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, double> val in values)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<VariableProfile, string> val in strings)
                    {
                        drVariableValues[((VariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Attributes_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Attributes_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Attributes_Total_Insert(
            string Merchandise, 
            string TimePeriod, 
            string Attribute, 
            string AttributeSet, 
            string Version, 
            string FilterName, 
            Dictionary<TimeTotalVariableProfile, double> values, 
            Dictionary<TimeTotalVariableProfile, string> strings
            )
        {
            try
            {

                DataRow drVariableValues;
                if (_dtVariableValues == null)
                {
                    _dtVariableValues = GetTableSchemaFromTableName(Include.DBROExtractPlanningAttributesTotalTable);
                }

                drVariableValues = _dtVariableValues.NewRow();
                _dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["Merchandise"] = Merchandise;
                drVariableValues["Version"] = Version;
                drVariableValues["Attribute"] = Attribute;
                drVariableValues["AttributeSet"] = AttributeSet;
                drVariableValues["TimePeriod"] = TimePeriod;
                drVariableValues["Filter"] = FilterName;
                if (values.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, double> val in values)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }
                if (strings.Count > 0)
                {
                    foreach (KeyValuePair<TimeTotalVariableProfile, string> val in strings)
                    {
                        drVariableValues[((TimeTotalVariableProfile)val.Key).VariableName] = val.Value;
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        public void Planning_Attributes_Total_Update()
        {
            try
            {
                // only send document if values or flags were sent
                if (_dtVariableValues != null &&
                   _dtVariableValues.Rows.Count > 0)
                {
                    StoredProcedures.RO_Planning_Attributes_Total_Write.Insert(_dba,
                                                                    dt: _dtVariableValues
                                                                    );
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable Extract_Session_Read(int sessionKey)
        {
            try
            {
                return StoredProcedures.RO_Extract_Session_Read.Read(_dba, SessionKey: sessionKey);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable Extract_Session_Read_All()
        {
            try
            {
                return StoredProcedures.RO_Extract_Session_Read_All.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool Extract_Session_Active()
        {
            try
            {
                int activeSession = Include.Undefined;
                StoredProcedures.RO_Extract_Session_Active.GetOutput(_dba, ref activeSession);
                return Include.ConvertIntToBool(activeSession);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Extract_Session_Update(long sessionKey, bool sessionActiveInd, DateTime sessionStartDateTime, DateTime? sessionEndDateTime)
        {
            try
            {
                StoredProcedures.RO_Extract_Session_Write.Insert(_dba,
                                                                     SessionKey: sessionKey,
                                                                     SessionActiveInd: Include.ConvertBoolToChar(sessionActiveInd),
                                                                     SessionStartDateTime: sessionStartDateTime,
                                                                     SessionEndDateTime: sessionEndDateTime
                                                                     );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
		/// Close the session for all extracts.
		/// </summary>
		public void CloseAllExtractSessions()
        {
            try
            {
                StoredProcedures.RO_Extract_Session_Update_All_For_Close.Update(_dba, sessionEndDateTime: DateTime.Now);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Get the columns defined to a table.
        /// </summary>
        /// <param name="aTableName"></param>
        /// <returns>Returns an empty DataTable containing the columns of the table</returns>
        public DataTable GetTableSchemaFromTableName(string aTableName)
        {
            try
            {
                return StoredProcedures.MID_TABLE_READ_SCHEMA.Read(_dba, TABLE_NAME: aTableName);

            }
            catch
            {
                throw;
            }
        }
    }
}
