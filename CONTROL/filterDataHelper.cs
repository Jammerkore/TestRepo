//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Globalization;

//using MIDRetail.Business;
//using MIDRetail.DataCommon;
//using MIDRetail.Data;
////using MIDRetail.ForecastComputations;

//namespace MIDRetail.Windows.Controls
//{
//    public sealed class valueTypes
//    {
//        public static List<valueTypes> valueTypeList = new List<valueTypes>();
//        public static readonly valueTypes Text = new valueTypes("text", 0);
//        public static readonly valueTypes Date = new valueTypes("date", 1);
//        public static readonly valueTypes Numeric = new valueTypes("numeric", 2);
//        public static readonly valueTypes Dollar = new valueTypes("dollar", 3);
//        public static readonly valueTypes List = new valueTypes("list", 5);
//        public static readonly valueTypes Boolean = new valueTypes("boolean", 6);


//        private valueTypes(string Name, int dbIndex)
//        {
//            this.Name = Name;
//            this.dbIndex = dbIndex;

//            valueTypeList.Add(this);
//        }
//        public string Name { get; private set; }
//        public int dbIndex { get; private set; }

//        public static implicit operator int(valueTypes op) { return op.dbIndex; }


//        public static valueTypes FromIndex(int dbIndex)
//        {
//            valueTypes result = valueTypeList.Find(
//               delegate(valueTypes ft)
//               {
//                   return ft.dbIndex == dbIndex;
//               }
//               );
//            if (result != null)
//            {
//                return result;
//            }
//            else
//            {
//                //value type was not found in the list so just return text type
//                return Text;
//            }
//        }
//        public static valueTypes FromString(string valueTypeName)
//        {
//            valueTypes result = valueTypeList.Find(
//              delegate(valueTypes ft)
//              {
//                  return ft.Name == valueTypeName;
//              }
//              );
//            if (result != null)
//            {
//                return result;
//            }
//            else
//            {
//                //value type was not found in the list
//                return null;
//            }
//        }
//    }

//    public sealed class dateTypes
//    {
//        public static List<dateTypes> navTypeList = new List<dateTypes>();
//        public static readonly dateTypes DateAndTime = new dateTypes(1);
//        public static readonly dateTypes DateOnly = new dateTypes(2);
//        public static readonly dateTypes TimeOnly = new dateTypes(3);


//        private dateTypes(int Index)
//        {
//            this.Index = Index;
//            navTypeList.Add(this);
//        }

//        public int Index { get; private set; }
//        public static implicit operator int(dateTypes op) { return op.Index; }


//        public static dateTypes FromIndex(int Index)
//        {
//            dateTypes result = navTypeList.Find(
//               delegate(dateTypes ft)
//               {
//                   return ft.Index == Index;
//               }
//               );
//            return result;
//        }

//    }

//    public sealed class numericTypes
//    {
//        public static List<numericTypes> navTypeList = new List<numericTypes>();
//        public static readonly numericTypes DoubleFreeForm = new numericTypes(1);
//        public static readonly numericTypes Dollar = new numericTypes(2);
//        public static readonly numericTypes Integer = new numericTypes(3);


//        private numericTypes(int Index)
//        {
//            this.Index = Index;
//            navTypeList.Add(this);
//        }

//        public int Index { get; private set; }
//        public static implicit operator int(numericTypes op) { return op.Index; }


//        public static numericTypes FromIndex(int Index)
//        {
//            numericTypes result = navTypeList.Find(
//               delegate(numericTypes ft)
//               {
//                   return ft.Index == Index;
//               }
//               );
//            return result;
//        }

//    }
//    public class valueInfoTypes
//    {
//        public valueTypes valueType;
//        public dateTypes dateType;
//        public numericTypes numericType;

//        public valueInfoTypes(valueTypes valueType)
//        {
//            this.valueType = valueType;
//        }
//        public valueInfoTypes(valueTypes valueType, dateTypes dateType)
//        {
//            this.valueType = valueType;
//            this.dateType = dateType;
//        }
//        public valueInfoTypes(valueTypes valueType, numericTypes numericType)
//        {
//            this.valueType = valueType;
//            this.numericType = numericType;
//        }
//    }
//    public static class filterDataHelper
//    {

//        private static DataTable dtHeaderTypes;
//        public static DataTable HeaderTypesGetDataTable()
//        {
//            if (dtHeaderTypes == null)
//            {
//                dtHeaderTypes = new DataTable();
//                dtHeaderTypes.Columns.Add("FIELD_NAME");
//                dtHeaderTypes.Columns.Add("FIELD_INDEX", typeof(int));

//                //DataRow dr1 = dtHeaderTypes.NewRow();
//                //dr1["FIELD_NAME"] = "ASN";
//                //dr1["FIELD_INDEX"] = 800731;
//                //dtHeaderTypes.Rows.Add(dr1);

//                //DataRow dr2 = dtHeaderTypes.NewRow();
//                //dr2["FIELD_NAME"] = "Drop Ship";
//                //dr2["FIELD_INDEX"] = 800733;
//                //dtHeaderTypes.Rows.Add(dr2);

//                //DataRow dr3 = dtHeaderTypes.NewRow();
//                //dr3["FIELD_NAME"] = "Dummy";
//                //dr3["FIELD_INDEX"] = 800732;
//                //dtHeaderTypes.Rows.Add(dr3);

//                //DataRow dr4 = dtHeaderTypes.NewRow();
//                //dr4["FIELD_NAME"] = "Multi-Header";
//                //dr4["FIELD_INDEX"] = 800734;
//                //dtHeaderTypes.Rows.Add(dr4);

//                //DataRow dr5 = dtHeaderTypes.NewRow();
//                //dr5["FIELD_NAME"] = "Purchase Order";
//                //dr5["FIELD_INDEX"] = 800738;
//                //dtHeaderTypes.Rows.Add(dr5);

//                //DataRow dr6 = dtHeaderTypes.NewRow();
//                //dr6["FIELD_NAME"] = "Receipt";
//                //dr6["FIELD_INDEX"] = 800730;
//                //dtHeaderTypes.Rows.Add(dr6);

//                //DataRow dr7 = dtHeaderTypes.NewRow();
//                //dr7["FIELD_NAME"] = "Reserve";
//                //dr7["FIELD_INDEX"] = 800735;
//                //dtHeaderTypes.Rows.Add(dr7);

//                //DataRow dr8 = dtHeaderTypes.NewRow();
//                //dr8["FIELD_NAME"] = "VSW";
//                //dr8["FIELD_INDEX"] = 800741;
//                //dtHeaderTypes.Rows.Add(dr8);

//                //DataRow dr9 = dtHeaderTypes.NewRow();
//                //dr9["FIELD_NAME"] = "Workup Total Buy";
//                //dr9["FIELD_INDEX"] = 800737;
//                //dtHeaderTypes.Rows.Add(dr9);




//                 //load header type
//                eHeaderType type;
//                DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));

//                for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
//                {
//                    DataRow dRow = dtTypes.Rows[i];
//                    if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
//                    {
//                        if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
//                        {
//                            dtTypes.Rows.Remove(dRow);
//                        }
//                        else if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
//                        {
//                            dtTypes.Rows.Remove(dRow);
//                        }
//                    }

//                    type = (eHeaderType)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
//                    // if size, use all statuses
//                    if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
//                    {
//                    }
//                    else
//                    {
//                        // remove all size statuses
//                        if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type, CultureInfo.CurrentUICulture)))
//                        {
//                        }
//                        else
//                        {
//                            dtTypes.Rows.Remove(dRow);
//                        }
//                    }
                     
//                }
//                foreach (DataRow dr in dtTypes.Rows)
//                {
//                    DataRow dr1 = dtHeaderTypes.NewRow();
//                    dr1["FIELD_NAME"] = (string)dr["TEXT_VALUE"];
//                    dr1["FIELD_INDEX"] = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
//                    dtHeaderTypes.Rows.Add(dr1);
//                }
//            }
//            return dtHeaderTypes;
//        }

//        private static DataTable dtHeaderStatuses;
//        public static DataTable HeaderStatusesGetDataTable()
//        {
//            if (dtHeaderStatuses == null)
//            {
//                dtHeaderStatuses = new DataTable();
//                dtHeaderStatuses.Columns.Add("FIELD_NAME");
//                dtHeaderStatuses.Columns.Add("FIELD_INDEX", typeof(int));

//                //DataRow dr1 = dtHeaderStatuses.NewRow();
//                //dr1["FIELD_NAME"] = "All In balance";
//                //dr1["FIELD_INDEX"] = 802708;
//                //dtHeaderStatuses.Rows.Add(dr1);

//                //DataRow dr2 = dtHeaderStatuses.NewRow();
//                //dr2["FIELD_NAME"] = "Allocated In Balance";
//                //dr2["FIELD_INDEX"] = 802706;
//                //dtHeaderStatuses.Rows.Add(dr2);

//                //DataRow dr3 = dtHeaderStatuses.NewRow();
//                //dr3["FIELD_NAME"] = "Allocated Out of Balance";
//                //dr3["FIELD_INDEX"] = 802705;
//                //dtHeaderStatuses.Rows.Add(dr3);

//                //DataRow dr4 = dtHeaderStatuses.NewRow();
//                //dr4["FIELD_NAME"] = "Allocation Started";
//                //dr4["FIELD_INDEX"] = 802711;
//                //dtHeaderStatuses.Rows.Add(dr4);

//                //DataRow dr5 = dtHeaderStatuses.NewRow();
//                //dr5["FIELD_NAME"] = "In use by Multi-Header";
//                //dr5["FIELD_INDEX"] = 802702;
//                //dtHeaderStatuses.Rows.Add(dr5);

//                //DataRow dr6 = dtHeaderStatuses.NewRow();
//                //dr6["FIELD_NAME"] = "Pre-Size In Balance";
//                //dr6["FIELD_INDEX"] = 802704;
//                //dtHeaderStatuses.Rows.Add(dr6);

//                //DataRow dr7 = dtHeaderStatuses.NewRow();
//                //dr7["FIELD_NAME"] = "Pre-Size Out of Balance";
//                //dr7["FIELD_INDEX"] = 802703;
//                //dtHeaderStatuses.Rows.Add(dr7);

//                //DataRow dr8 = dtHeaderStatuses.NewRow();
//                //dr8["FIELD_NAME"] = "Received In Balance";
//                //dr8["FIELD_INDEX"] = 802701;
//                //dtHeaderStatuses.Rows.Add(dr8);

//                //DataRow dr9 = dtHeaderStatuses.NewRow();
//                //dr9["FIELD_NAME"] = "Received Out of Balance";
//                //dr9["FIELD_INDEX"] = 802700;
//                //dtHeaderStatuses.Rows.Add(dr9);

//                //DataRow dr10 = dtHeaderStatuses.NewRow();
//                //dr10["FIELD_NAME"] = "Released Approved";
//                //dr10["FIELD_INDEX"] = 802710;
//                //dtHeaderStatuses.Rows.Add(dr10);

//                //DataRow dr11 = dtHeaderStatuses.NewRow();
//                //dr11["FIELD_NAME"] = "Released";
//                //dr11["FIELD_INDEX"] = 802709;
//                //dtHeaderStatuses.Rows.Add(dr11);

//                //DataRow dr12 = dtHeaderStatuses.NewRow();
//                //dr12["FIELD_NAME"] = "Sizes Out of Balance";
//                //dr12["FIELD_INDEX"] = 802707;
//                //dtHeaderStatuses.Rows.Add(dr12);

//                // load header status
//                eHeaderAllocationStatus status;
//                DataTable dtStatus = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
//                for (int i = dtStatus.Rows.Count - 1; i >= 0; i--)
//                {
//                    DataRow dRow = dtStatus.Rows[i];
//                    status = (eHeaderAllocationStatus)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);

//                    // if size, use all statuses
//                    if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
//                    {
//                    }
//                    else
//                    {
//                        // remove all size statuses
//                        if (Enum.IsDefined(typeof(eNonSizeHeaderAllocationStatus), Convert.ToInt32(status, CultureInfo.CurrentUICulture)))
//                        {
//                        }
//                        else
//                        {
//                            dtStatus.Rows.Remove(dRow);
//                        }
//                    }
//                }
//                foreach (DataRow dr in dtStatus.Rows)
//                {
//                    DataRow dr1 = dtHeaderStatuses.NewRow();
//                    dr1["FIELD_NAME"] = (string)dr["TEXT_VALUE"];
//                    dr1["FIELD_INDEX"] = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
//                    dtHeaderStatuses.Rows.Add(dr1);
//                }

//            }
//            return dtHeaderStatuses;
//        }
//        public static string HeaderStatusGetNameFromIndex(int fieldIndex)
//        {
//            if (dtHeaderStatuses == null)
//            {
//                HeaderStatusesGetDataTable();
//            }
//            DataRow[] drFind = dtHeaderStatuses.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            return (string)drFind[0]["FIELD_NAME"];
//        }


//        private static DataTable dtStores = null;
//        public static DataTable StoresGetDataTable()
//        {
//            if (dtStores == null)
//            {
//                dtStores = new DataTable();
//                dtStores.Columns.Add("STORE_NAME");
//                dtStores.Columns.Add("STORE_RID", typeof(int));

//                //DataRow drVariable = dtStores.NewRow();
//                //drVariable["Store"] = "0101 [Washington Square]";
//                //drVariable["STORE_RID"] = 101;
//                //dtStores.Rows.Add(drVariable);

//                //DataRow drVariable2 = dtStores.NewRow();
//                //drVariable2["Store"] = "0102 [Keystone Fashion Mall]";
//                //drVariable2["STORE_RID"] = 102;
//                //dtStores.Rows.Add(drVariable2);

//                //DataRow drVariable3 = dtStores.NewRow();
//                //drVariable3["Store"] = "0103 [Metropolis]";
//                //drVariable3["STORE_RID"] = 103;
//                //dtStores.Rows.Add(drVariable3);

//                //DataRow drVariable4 = dtStores.NewRow();
//                //drVariable4["Store"] = "0104 [Lafayette Square]";
//                //drVariable4["STORE_RID"] = 104;
//                //dtStores.Rows.Add(drVariable4);

//                //DataRow drVariable5 = dtStores.NewRow();
//                //drVariable5["Store"] = "105 [Reserve]";
//                //drVariable5["STORE_RID"] = 105;
//                //dtStores.Rows.Add(drVariable5);


//                foreach (StoreProfile sp in SAB.StoreServerSession.GetAllStoresList())
//                {
//                    DataRow dr1 = dtStores.NewRow();
//                    dr1["STORE_NAME"] = sp.Text;
//                    dr1["STORE_RID"] = sp.Key;
//                    dtStores.Rows.Add(dr1);
//                }

             
//            }
//            return dtStores;
//        }



//        //private static DataTable dtStoreCharValuesForCharGroup;
//        public static DataTable StoreCharacteristicsGetValuesForGroup(int groupRID)
//        {
//            //SqlParameter InParam = new SqlParameter();
//            //InParam.Direction = ParameterDirection.Input;
//            //InParam.ParameterName = "@SCG_RID";
//            //InParam.SqlDbType = SqlDbType.Int;
//            //InParam.Value = groupRID;
//            //List<SqlParameter> pList = new List<SqlParameter>();
//            //pList.Add(InParam);
//            //dtStoreCharValuesForCharGroup = ExecuteStoredProcedure("MID_STORE_CHAR_READ_FOR_GROUP", pList);
//            //return dtStoreCharValuesForCharGroup;
//            FilterData fd = new FilterData();
//            return fd.StoreCharacteristicsGetValuesForGroup(groupRID);
//        }
//        private static DataTable dtStoreCharacteristics;
//        public static DataTable StoreCharacteristicsGetDataTable()
//        {
//            if (dtStoreCharacteristics == null)
//            {
//                StoreData storeData = new StoreData();
//                //DataTable dt = ExecuteStoredProcedure("MID_STORE_CHAR_GROUP_READ_ALL");
//                DataTable dt = storeData.StoreCharGroup_Read();
//                dtStoreCharacteristics = new DataTable();
//                dtStoreCharacteristics.Columns.Add("FIELD_NAME");
//                dtStoreCharacteristics.Columns.Add("FIELD_INDEX", typeof(int));
//                dtStoreCharacteristics.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

//                foreach (DataRow dr in dt.Select())
//                {
//                    DataRow drNew = dtStoreCharacteristics.NewRow();
//                    drNew["FIELD_NAME"] = dr["SCG_ID"];
//                    drNew["FIELD_INDEX"] = dr["SCG_RID"];

//                    valueTypes vt = StoreCharacteristicsGetValueTypeFromDataRow(dr);
//                    drNew["FIELD_VALUE_TYPE_INDEX"] = vt.dbIndex;
//                    dtStoreCharacteristics.Rows.Add(drNew);
//                }
//            }
//            return dtStoreCharacteristics;
//        }
//        public static valueTypes StoreCharacteristicsGetValueTypeFromDataRow(DataRow dr)
//        {
//            int scgType = (int)dr["SCG_TYPE"];
//            string isStoreCharList = (string)dr["SCG_LIST_IND"];
//            //public enum eStoreCharType
//            //{
//            //    text=0,
//            //    date=1,
//            //    number=2,
//            //    dollar=3,
//            //    unknown=4,
//            //    list = 5,
//            //    boolean = 6
//            //}
//            valueTypes vt = valueTypes.Text;
//            if (isStoreCharList == "1")
//            {
//                vt = valueTypes.List;
//            }
//            else
//            {
//                if (scgType == 0)
//                {
//                    vt = valueTypes.Text;
//                }
//                else if (scgType == 1)
//                {
//                    vt = valueTypes.Date;
//                }
//                else if (scgType == 2)
//                {
//                    vt = valueTypes.Numeric;
//                }
//                else if (scgType == 3)
//                {
//                    vt = valueTypes.Dollar;
//                }
//            }
//            return vt;
//        }
//        public static valueInfoTypes StoreCharacteristicsGetValueInfoType(int fieldIndex)
//        {
//            DataRow[] drFind = dtStoreCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
//            return GetValueInfoTypeForCharacteristics(valueTypes.FromIndex(vtIndex));
//        }
//        public static string StoreCharacteristicsGetNameFromIndex(int fieldIndex)
//        {
//            if (dtStoreCharacteristics == null)
//            {
//                StoreCharacteristicsGetDataTable();
//            }
//            DataRow[] drFind = dtStoreCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            return (string)drFind[0]["FIELD_NAME"];
//        }



//        private static valueInfoTypes GetValueInfoTypeForCharacteristics(valueTypes vt)
//        {
//            valueInfoTypes vInfo;
//            if (vt == valueTypes.Dollar)
//            {
//                vInfo = new valueInfoTypes(vt, numericTypes.Dollar);
//            }
//            else if (vt == valueTypes.Numeric)
//            {
//                vInfo = new valueInfoTypes(vt, numericTypes.DoubleFreeForm);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                vInfo = new valueInfoTypes(vt, dateTypes.DateOnly);
//            }
//            else
//            {
//                vInfo = new valueInfoTypes(vt);
//            }

//            return vInfo;
//        }







//        //private static DataTable dtHeaderCharValuesForCharGroup;
//        public static DataTable HeaderCharacteristicsGetValuesForGroup(int groupRID)
//        {
//            //SqlParameter InParam = new SqlParameter();
//            //InParam.Direction = ParameterDirection.Input;
//            //InParam.ParameterName = "@HCG_RID";
//            //InParam.SqlDbType = SqlDbType.Int;
//            //InParam.Value = groupRID;
//            //List<SqlParameter> pList = new List<SqlParameter>();
//            //pList.Add(InParam);
//            //dtHeaderCharValuesForCharGroup = ExecuteStoredProcedure("MID_HEADER_CHAR_READ_FOR_GROUP", pList);
//            //return dtHeaderCharValuesForCharGroup;
//            FilterData fd = new FilterData();
//            return fd.HeaderCharacteristicsGetValuesForGroup(groupRID);
//        }
//        private static DataTable dtHeaderCharacteristics;
//        public static DataTable HeaderCharacteristicsGetDataTable()
//        {
//            if (dtHeaderCharacteristics == null)
//            {
//                HeaderCharacteristicsData hdcData = new HeaderCharacteristicsData();
//                DataTable dt = hdcData.HeaderCharGroup_Read();
//                //DataTable dt = ExecuteStoredProcedure("MID_HEADER_CHAR_GROUP_READ_ALL");
//                dtHeaderCharacteristics = new DataTable();
//                dtHeaderCharacteristics.Columns.Add("FIELD_NAME");
//                dtHeaderCharacteristics.Columns.Add("FIELD_INDEX", typeof(int));
//                dtHeaderCharacteristics.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

//                foreach (DataRow dr in dt.Select())
//                {
//                    DataRow drNew = dtHeaderCharacteristics.NewRow();
//                    drNew["FIELD_NAME"] = dr["HCG_ID"];
//                    drNew["FIELD_INDEX"] = dr["HCG_RID"];

//                    valueTypes vt = HeaderCharacteristicsGetValueTypeFromDataRow(dr);
//                    drNew["FIELD_VALUE_TYPE_INDEX"] = vt.dbIndex;
//                    dtHeaderCharacteristics.Rows.Add(drNew);
//                }
//            }
//            return dtHeaderCharacteristics;
//        }
//        public static valueTypes HeaderCharacteristicsGetValueTypeFromDataRow(DataRow dr)
//        {
//            int scgType = (int)dr["HCG_TYPE"];
//            string isHeaderCharList = (string)dr["HCG_LIST_IND"];
//            //public enum eHeaderCharType
//            //{
//            //    text=0,
//            //    date=1,
//            //    number=2,
//            //    dollar=3,
//            //    unknown=4,
//            //    list = 5,
//            //    boolean = 6
//            //}
//            valueTypes vt = valueTypes.Text;
//            if (isHeaderCharList == "1")
//            {
//                vt = valueTypes.List;
//            }
//            else
//            {
//                if (scgType == 0)
//                {
//                    vt = valueTypes.Text;
//                }
//                else if (scgType == 1)
//                {
//                    vt = valueTypes.Date;
//                }
//                else if (scgType == 2)
//                {
//                    vt = valueTypes.Numeric;
//                }
//                else if (scgType == 3)
//                {
//                    vt = valueTypes.Dollar;
//                }
//            }
//            return vt;
//        }
//        public static valueInfoTypes HeaderCharacteristicsGetValueInfoType(int fieldIndex)
//        {
//            DataRow[] drFind = dtHeaderCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
//            return GetValueInfoTypeForCharacteristics(valueTypes.FromIndex(vtIndex));
//        }
//        public static string HeaderCharacteristicsGetNameFromIndex(int fieldIndex)
//        {
//            if (dtHeaderCharacteristics == null)
//            {
//                HeaderCharacteristicsGetDataTable();
//            }
//            DataRow[] drFind = dtHeaderCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            return (string)drFind[0]["FIELD_NAME"];
//        }

//        public static SessionAddressBlock SAB = null;

//        private static DataTable dtVariables;
//        public static DataTable VariablesGetDataTable()
//        {
//            if (dtVariables == null)
//            {
//                dtVariables = new DataTable();
//                dtVariables.Columns.Add("FIELD_NAME");
//                dtVariables.Columns.Add("FIELD_INDEX", typeof(int));
//                dtVariables.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));
//                dtVariables.Columns.Add("NUMERIC_TYPE_INDEX", typeof(int));
//                dtVariables.Columns.Add("DATE_TYPE_INDEX", typeof(int));

    
            
//                //PlanComputationsCollection compCollections = new PlanComputationsCollection();
//                //IPlanComputationVariables variables = compCollections.GetDefaultComputations().PlanVariables;
//                //ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();

//                ProfileList variableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
//                ProfileList timeTotalVariableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;

//                foreach (VariableProfile vp in variableProfList)
//                {
//                    //_variableList.Add(new ProfileComboObject(varProf.Key, varProf.VariableName, varProf));
//                    DataRow dr1 = dtVariables.NewRow();
//                    dr1["FIELD_NAME"] = vp.VariableName;
//                    dr1["FIELD_INDEX"] = vp.Key;
//                    valueTypes valueType;
//                    numericTypes numericType;
//                    dateTypes dateType;
//                    GetTypesFromVariableProfile(vp, out valueType, out numericType, out dateType);
//                    dr1["FIELD_VALUE_TYPE_INDEX"] = valueType.dbIndex;
//                    dr1["NUMERIC_TYPE_INDEX"] = numericType.Index;
//                    dr1["DATE_TYPE_INDEX"] = dateType.Index;
//                    dtVariables.Rows.Add(dr1);
//                }

//                foreach (VariableProfile vp in variableProfList)
//                {
//                    for (int i = 0; i < vp.TimeTotalVariables.Count; i++)
//                    {
//                        //_variableList.Add(new TimeTotalComboObject(((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).Key, "[Date Total] " + ((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).VariableName, (Profile)varProf.TimeTotalVariables[i], varProf, i + 1));
//                        DataRow dr1 = dtVariables.NewRow();
//                        dr1["FIELD_NAME"] = "[Date Total] " + ((TimeTotalVariableProfile)vp.TimeTotalVariables[i]).VariableName;
//                        dr1["FIELD_INDEX"] = (vp.Key * 1000) + i;
//                        valueTypes valueType;
//                        numericTypes numericType;
//                        dateTypes dateType;
//                        GetTypesFromVariableProfile(vp, out valueType, out numericType, out dateType);
//                        dr1["FIELD_VALUE_TYPE_INDEX"] = valueType.dbIndex;
//                        dr1["NUMERIC_TYPE_INDEX"] = numericType.Index;
//                        dr1["DATE_TYPE_INDEX"] = dateType.Index;
//                        dtVariables.Rows.Add(dr1);
//                    }
//                }
             
//                //int count = 0;
//                //foreach (VariableProfile vp in aDatabaseVariables)
//                //{
//                //    DataRow dr1 = dtVariables.NewRow();
//                //    dr1["FIELD_NAME"] = vp.VariableName;
//                //    dr1["FIELD_INDEX"] = 1;
//                //    dr1["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //    dr1["NUMERIC_TYPE_INDEX"] = numericTypes.DoubleFreeForm.Index;
//                //    dr1["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //    dtVariables.Rows.Add(dr1);
//                //}

//                //DataRow dr1 = dtVariables.NewRow();
//                //dr1["FIELD_NAME"] = "Sales";
//                //dr1["FIELD_INDEX"] = 1;
//                //dr1["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr1["NUMERIC_TYPE_INDEX"] = numericTypes.DoubleFreeForm.Index;
//                //dr1["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //dtVariables.Rows.Add(dr1);

//                //DataRow dr2 = dtVariables.NewRow();
//                //dr2["FIELD_NAME"] = "Sales Reg";
//                //dr2["FIELD_INDEX"] = 2;
//                //dr2["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr2["NUMERIC_TYPE_INDEX"] = numericTypes.DoubleFreeForm.Index;
//                //dr2["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //dtVariables.Rows.Add(dr2);

//                //DataRow dr3 = dtVariables.NewRow();
//                //dr3["FIELD_NAME"] = "Sales Promo";
//                //dr3["FIELD_INDEX"] = 3;
//                //dr3["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr3["NUMERIC_TYPE_INDEX"] = numericTypes.DoubleFreeForm.Index;
//                //dr3["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //dtVariables.Rows.Add(dr3);

//                //DataRow dr4 = dtVariables.NewRow();
//                //dr4["FIELD_NAME"] = "Stock";
//                //dr4["FIELD_INDEX"] = 4;
//                //dr4["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr4["NUMERIC_TYPE_INDEX"] = numericTypes.Integer.Index;
//                //dr4["DATE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dtVariables.Rows.Add(dr4);

//                //DataRow dr5 = dtVariables.NewRow();
//                //dr5["FIELD_NAME"] = "Stock Reg";
//                //dr5["FIELD_INDEX"] = 5;
//                //dr5["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr5["NUMERIC_TYPE_INDEX"] = numericTypes.Integer.Index;
//                //dr5["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //dtVariables.Rows.Add(dr5);

//                //DataRow dr6 = dtVariables.NewRow();
//                //dr6["FIELD_NAME"] = "Stock Mkdn";
//                //dr6["FIELD_INDEX"] = 6;
//                //dr6["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Numeric.dbIndex;
//                //dr6["NUMERIC_TYPE_INDEX"] = numericTypes.Integer.Index;
//                //dr6["DATE_TYPE_INDEX"] = dateTypes.DateOnly.Index;
//                //dtVariables.Rows.Add(dr6);


//            }
//            return dtVariables;
//        }
//        private static void GetTypesFromVariableProfile(VariableProfile vp, out valueTypes valueType, out numericTypes numericType, out dateTypes dateType)
//        {
//            eVariableDatabaseType dbType = vp.StoreDatabaseVariableType;
//            if (dbType == eVariableDatabaseType.String || dbType == eVariableDatabaseType.Char)
//            {
//                valueType = valueTypes.Text;
//                numericType = numericTypes.Integer;
//                dateType = dateTypes.DateOnly;
//            }
//            else if (dbType == eVariableDatabaseType.DateTime)
//            {
//                valueType = valueTypes.Date;
//                numericType = numericTypes.Integer;
//                dateType = dateTypes.DateOnly;
//            }
//            else
//            {
//                valueType = valueTypes.Numeric;
//                dateType = dateTypes.DateOnly;
//                if (dbType == eVariableDatabaseType.Integer || dbType == eVariableDatabaseType.BigInteger)
//                {                    
//                    numericType = numericTypes.Integer;
//                }
//                else
//                {   
//                    numericType = numericTypes.DoubleFreeForm;
//                }
//            }
//        }
//        public static valueInfoTypes VariablesGetValueInfoType(int fieldIndex)
//        {
//            if (dtVariables == null)
//            {
//                VariablesGetDataTable();
//            }
//            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());


//            valueTypes vt = valueTypes.FromIndex((int)drFind[0]["FIELD_VALUE_TYPE_INDEX"]);


//            valueInfoTypes vInfo;
//            if (vt == valueTypes.Dollar)
//            {
//                vInfo = new valueInfoTypes(vt, numericTypes.Dollar);
//            }
//            else if (vt == valueTypes.Numeric)
//            {
//                numericTypes nt = numericTypes.FromIndex((int)drFind[0]["NUMERIC_TYPE_INDEX"]);
//                vInfo = new valueInfoTypes(vt, nt);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                dateTypes dt = dateTypes.FromIndex((int)drFind[0]["DATE_TYPE_INDEX"]);
//                vInfo = new valueInfoTypes(vt, dt);
//            }
//            else
//            {
//                vInfo = new valueInfoTypes(vt);
//            }
//            return vInfo;
//        }
//        public static string VariablesGetNameFromIndex(int fieldIndex)
//        {
//            if (dtVariables == null)
//            {
//                VariablesGetDataTable();
//            }
//            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            return (string)drFind[0]["FIELD_NAME"];
//        }
//        public static int VariablesGetIndexFromName(string varName)
//        {
//            if (dtVariables == null)
//            {
//                VariablesGetDataTable();
//            }
//            DataRow[] drFind = dtVariables.Select("FIELD_NAME='" + varName + "'");
//            return (int)drFind[0]["FIELD_INDEX"];
//        }


//        private static DataTable dtVersions;
//        public static DataTable VersionsGetDataTable()
//        {
//            if (dtVersions == null)
//            {
//                dtVersions = new DataTable();
//                dtVersions.Columns.Add("FIELD_NAME");
//                dtVersions.Columns.Add("FIELD_INDEX", typeof(int));
//                dtVersions.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

//                ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
//                DataRow dr1 = dtVersions.NewRow();
//                dr1["FIELD_NAME"] = "Default to Plan";
//                dr1["FIELD_INDEX"] = -1;
//                dr1["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                dtVersions.Rows.Add(dr1);
//                foreach (VersionProfile versionProf in versionProfList)
//                {
//                    //_versionList.Add(new ProfileComboObject(versionProf.Key, versionProf.Description, versionProf));
//                    DataRow dr2 = dtVersions.NewRow();
//                    dr2["FIELD_NAME"] = versionProf.Description;
//                    dr2["FIELD_INDEX"] = versionProf.Key;
//                    dr2["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                    dtVersions.Rows.Add(dr2);
//                }

//                //DataRow dr1 = dtVersions.NewRow();
//                //dr1["FIELD_NAME"] = "Default to Plan";
//                //dr1["FIELD_INDEX"] = 1;
//                //dr1["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                //dtVersions.Rows.Add(dr1);

//                //DataRow dr2 = dtVersions.NewRow();
//                //dr2["FIELD_NAME"] = "Actual";
//                //dr2["FIELD_INDEX"] = 2;
//                //dr2["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                //dtVersions.Rows.Add(dr2);

//                //DataRow dr3 = dtVersions.NewRow();
//                //dr3["FIELD_NAME"] = "Action";
//                //dr3["FIELD_INDEX"] = 3;
//                //dr3["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                //dtVersions.Rows.Add(dr3);

//                //DataRow dr4 = dtVersions.NewRow();
//                //dr4["FIELD_NAME"] = "Baseline";
//                //dr4["FIELD_INDEX"] = 4;
//                //dr4["FIELD_VALUE_TYPE_INDEX"] = valueTypes.Text.dbIndex;
//                //dtVersions.Rows.Add(dr4);


//            }
//            return dtVersions;
//        }
//        public static valueTypes VersionsGetValueType(int fieldIndex)
//        {
//            if (dtVersions == null)
//            {
//                VersionsGetDataTable();
//            }
//            DataRow[] drFind = dtVersions.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
//            return valueTypes.FromIndex(vtIndex);
//        }
//        public static string VersionsGetNameFromIndex(int fieldIndex)
//        {
//            if (dtVersions == null)
//            {
//                VersionsGetDataTable();
//            }
//            DataRow[] drFind = dtVersions.Select("FIELD_INDEX=" + fieldIndex.ToString());
//            return (string)drFind[0]["FIELD_NAME"];
//        }
//        public static int VersionsGetIndexFromName(string varName)
//        {
//            if (dtVersions == null)
//            {
//                VersionsGetDataTable();
//            }
//            DataRow[] drFind = dtVersions.Select("FIELD_NAME='" + varName + "'");
//            return (int)drFind[0]["FIELD_INDEX"];
//        }


//    }
//}
