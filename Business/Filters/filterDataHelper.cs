using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
//using MIDRetail.ForecastComputations;

namespace MIDRetail.Business
{
    /// <summary>
    /// static class to hold static functions that read data for filters
    /// </summary>
    public static class filterDataHelper
    {
        // Begin TT#1909-MD - JSmith - Str_Vesioning - Interfaced Store not available for selection in the STore List for a Static Store Attribute
        public static void Refresh()
        {
            dsUsersAndGroups = null;
            dtActiveUsersWithGroupRID = null;
            dtHeaderCharacteristics = null;
            dtHeaderStatuses = null;
            dtHeaderTypes = null;
            dtProductCharacteristics = null;
            dtProductHierarchies = null;
            dtProductLevels = null;
            dtStoreCharacteristics = null;
            dtStores = null;
            dtVariables = null;
            dtVersions = null;
        }
        // End TT#1909-MD - JSmith - Str_Vesioning - Interfaced Store not available for selection in the STore List for a Static Store Attribute

        public static int StoreAverageQuantityKey = -1;
        public static int ValueQuantityKey = -1;

        public static void SetVariableKeysFromTransaction(ApplicationSessionTransaction _transaction)
        {
            if (_transaction != null)
            {
                if (StoreAverageQuantityKey == -1)
                {
                    StoreAverageQuantityKey = _transaction.PlanComputations.PlanQuantityVariables.StoreAverageQuantity.Key;
                    ValueQuantityKey = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                }
            }
        }


        public static filter LoadExistingFilter(int filterRID)
        {
            filter f = new filter(-1, -1);
            FilterData fd = new FilterData();
            DataTable dt = fd.FilterRead(filterRID);
            f.LoadFromDataRow(dt.Rows[0]);
            DataTable dtConditions = fd.FilterReadConditions(filterRID);
           
            f.LoadConditionsFromDataTable(dtConditions);
            DataTable dtListValues = fd.FilterReadListValues(filterRID);
            foreach (DataRow drCondition in dtConditions.Rows)
            {
                ConditionNode cn = f.FindConditionNode((int)drCondition["SEQ"]);

                if (cn.condition.listConstantType == filterListConstantTypes.HasValues)
                {
                    DataRow[] drListValues = dtListValues.Select("CONDITION_RID=" + cn.condition.conditionRID);
                    cn.condition.LoadListValuesFromDataRowArray(drListValues);
                }
            }
            return f;
        }



        public static bool CompareInList(filterListValueTypes listValueType, int valueIndexToCompare, filterCondition fc)
        {
            DataRow[] listValues = fc.GetListValues(listValueType);

            filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);

            bool b;
            if (fc.listConstantType == filterListConstantTypes.All)
            {
                b = true;
            }
            else if (fc.listConstantType == filterListConstantTypes.None)
            {
                b = false;
            }
            else
            {
                b = listValues.Any(x => (int)x["LIST_VALUE_INDEX"] == valueIndexToCompare);
            }

            if (listOp == filterListOperatorTypes.Excludes)
            {
                b = !b;
            }
            return b;
        }

        public static bool CompareToString(string val1, filterCondition fc)
        {
            string val2 = fc.valueToCompare;
            filterStringOperatorTypes stringOp = filterStringOperatorTypes.FromIndex(fc.operatorIndex);


            // BEGIN TT#5460 - AGallagher - Null Reference Error with Store Filters
            if (val1 == null)
            { return false; }
            // END TT#5460 - AGallagher - Null Reference Error with Store Filters
            if (stringOp == filterStringOperatorTypes.Contains)
            {
                return val1.ToUpper().Contains(val2.ToUpper());
            }
            else if (stringOp == filterStringOperatorTypes.ContainsExactly)
            {
                return val1.Contains(val2);
            }
            else if (stringOp == filterStringOperatorTypes.StartsWith)
            {
                return val1.ToUpper().StartsWith(val2.ToUpper());
            }
            else if (stringOp == filterStringOperatorTypes.StartsExactlyWith)
            {
                return val1.StartsWith(val2);
            }
            else if (stringOp == filterStringOperatorTypes.EndsWith)
            {
                return val1.ToUpper().EndsWith(val2.ToUpper());
            }
            else if (stringOp == filterStringOperatorTypes.EndsExactlyWith)
            {
                return val1.EndsWith(val2);
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqual)
            {
                return (val1.ToUpper() == val2.ToUpper());
            }
            else if (stringOp == filterStringOperatorTypes.DoesEqualExactly)
            {
                return (val1 == val2);
            }
            else
            {
                return (val1 == val2);
            }
        }
        public static bool CompareToBool(bool val1, filterCondition fc)
        {
            bool val2;
            if (fc.valueToCompareBool != null)
            {
                if (fc.valueToCompareBool == false)
                {
                    val2 = false;
                }
                else
                {
                    val2 = true;
                }
            }
            else
            {
                val2 = false;
            }

            return (val1 == val2);
        }

        public static bool CompareToDate(DateTime val1, filterCondition fc)
        {
            DateTime val2 = (DateTime)fc.valueToCompareDateFrom;
            filterDateOperatorTypes dateOp = filterDateOperatorTypes.FromIndex(fc.operatorIndex);
        
            DateTime dtCompareFrom = (DateTime)fc.valueToCompareDateFrom;
            DateTime dtCompareTo = (DateTime)fc.valueToCompareDateTo;

            filterDateTypes tempFilterDateType = filterDateTypes.FromIndex(fc.dateTypeIndex); //TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option

            //use dynamic dates for Last 24 Hours, Last 7 days, Between
            if (dateOp == filterDateOperatorTypes.Last24Hours)
            {
                DateTime dateFrom;
                DateTime dateTo;

                dateTo = DateTime.Now;
                dateFrom = dateTo.AddHours(-24);

                dtCompareTo = dateTo;
                dtCompareFrom = dateFrom;
            }
            else if (dateOp == filterDateOperatorTypes.Last7Days)
            {
                DateTime dateFrom;
                DateTime dateTo;

                //go from midnight to midnight
                dateTo = DateTime.Today;
                dateFrom = dateTo.AddDays(-7);

                dtCompareTo = dateTo;
                dtCompareFrom = dateFrom;
            }
            else if (dateOp == filterDateOperatorTypes.Between)
            {
                DateTime dtNow = DateTime.Now;
                int daysFrom = fc.valueToCompareDateBetweenFromDays;
                int daysTo = fc.valueToCompareDateBetweenToDays;

                dtCompareFrom = dtNow.AddDays(daysFrom);
                dtCompareTo = dtNow.AddDays(daysTo);

                if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool == false) //TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                {
                    tempFilterDateType = filterDateTypes.DateOnly;
                }
            }

            if (dateOp == filterDateOperatorTypes.Unrestricted)
            {
                return true;
            }
            else
            {
                int i1;
                if (tempFilterDateType == filterDateTypes.DateOnly)
                {
                    i1 = DateTime.Compare(val1.Date, dtCompareFrom.Date);
                }
                else if (tempFilterDateType == filterDateTypes.TimeOnly)
                {
                    i1 = TimeSpan.Compare(val1.TimeOfDay, dtCompareFrom.TimeOfDay);
                }
                else
                {
                    i1 = DateTime.Compare(val1, dtCompareFrom);
                }

                
                if (i1 >= 0)  
                {
                    // val1 is later than or equal to dtCompareFrom
                    //now compare against dtCompareTo
                    int i2;
                    if (tempFilterDateType == filterDateTypes.DateOnly)
                    {
                        i2 = DateTime.Compare(val1.Date, dtCompareTo.Date);
                    }
                    else if (tempFilterDateType == filterDateTypes.TimeOnly)
                    {
                        i2 = TimeSpan.Compare(val1.TimeOfDay, dtCompareTo.TimeOfDay);
                    }
                    else
                    {
                        i2 = DateTime.Compare(val1, dtCompareTo);
                    }

                    if (i2 <= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

           
        }

        public static bool CompareToInt(int val1, filterCondition fc)
        {
            int val2 = (int)fc.valueToCompareInt;
            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);

            if (numericOp == filterNumericOperatorTypes.DoesEqual)
            {
                return (val1 == val2);
            }
            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
            {
                return (val1 != val2);
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
            {
                return (val1 > val2);
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
            {
                return (val1 >= val2);
            }
            else if (numericOp == filterNumericOperatorTypes.LessThan)
            {
                return (val1 < val2);
            }
            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
            {
                return (val1 <= val2);
            }
            else if (numericOp == filterNumericOperatorTypes.Between)
            {
                int val3 = (int)fc.valueToCompareInt2;
                return (val1 >= val2 && val1 <= val3);
            }
            else
            {
                // compare as does equal
                return (val1 == val2);
            }

        }
     
        public static bool CompareToDouble(double val1, filterCondition fc)
        {
            double val2 = (double)fc.valueToCompareDouble;
            filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);

            if (numericOp == filterNumericOperatorTypes.DoesEqual)
            {
                return (val1 == val2);
            }
            else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
            {
                return (val1 != val2);
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThan)
            {
                return (val1 > val2);
            }
            else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
            {
                return (val1 >= val2);
            }
            else if (numericOp == filterNumericOperatorTypes.LessThan)
            {
                return (val1 < val2);
            }
            else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
            {
                return (val1 <= val2);
            }
            else if (numericOp == filterNumericOperatorTypes.Between)
            {
                double val3 = (double)fc.valueToCompareDouble2;
                return (val1 >= val2 && val1 <= val3);            
            }
            else
            {
                // compare as does equal
                return (val1 == val2);
            }

        }


        private static DataTable dtHeaderTypes;
        public static DataTable HeaderTypesGetDataTable()
        {
            if (dtHeaderTypes == null)
            {
                dtHeaderTypes = new DataTable();
                dtHeaderTypes.Columns.Add("FIELD_NAME");
                dtHeaderTypes.Columns.Add("FIELD_INDEX", typeof(int));

                //load header type
                eHeaderType type;
                DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));

                for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dRow = dtTypes.Rows[i];
                    if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                    {
                        if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment, CultureInfo.CurrentUICulture))
                        {
                            dtTypes.Rows.Remove(dRow);
                        }
                        else if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder, CultureInfo.CurrentUICulture))
                        {
                            dtTypes.Rows.Remove(dRow);
                        }
                    }

                    type = (eHeaderType)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    // if size, use all statuses
                    if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                    {
                    }
                    else
                    {
                        // remove all size statuses
                        if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type, CultureInfo.CurrentUICulture)))
                        {
                        }
                        else
                        {
                            dtTypes.Rows.Remove(dRow);
                        }
                    }

                }
                foreach (DataRow dr in dtTypes.Rows)
                {
                    DataRow dr1 = dtHeaderTypes.NewRow();
                    dr1["FIELD_NAME"] = (string)dr["TEXT_VALUE"];
                    dr1["FIELD_INDEX"] = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    dtHeaderTypes.Rows.Add(dr1);
                }
            }
            return dtHeaderTypes;
        }

        private static DataTable dtHeaderStatuses;
        public static DataTable HeaderStatusesGetDataTable()
        {
            if (dtHeaderStatuses == null)
            {
                dtHeaderStatuses = new DataTable();
                dtHeaderStatuses.Columns.Add("FIELD_NAME");
                dtHeaderStatuses.Columns.Add("FIELD_INDEX", typeof(int));

                // load header status
                eHeaderAllocationStatus status;
                DataTable dtStatus = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
                for (int i = dtStatus.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dRow = dtStatus.Rows[i];
                    status = (eHeaderAllocationStatus)Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);

                    // if size, use all statuses
                    if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                    {
                    }
                    else
                    {
                        // remove all size statuses
                        if (Enum.IsDefined(typeof(eNonSizeHeaderAllocationStatus), Convert.ToInt32(status, CultureInfo.CurrentUICulture)))
                        {
                        }
                        else
                        {
                            dtStatus.Rows.Remove(dRow);
                        }
                    }
                }
                foreach (DataRow dr in dtStatus.Rows)
                {
                    //Begin TT#1368-MD -jsobek -Header Filters - Remove InUsebyMulti status option
                    int textCode = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
                    //if (textCode != 802702)  // TT#4796 - JSmith - Reinstate Multi-Header
                    {
                        DataRow dr1 = dtHeaderStatuses.NewRow();
                        dr1["FIELD_NAME"] = (string)dr["TEXT_VALUE"];
                        dr1["FIELD_INDEX"] = textCode;
                        dtHeaderStatuses.Rows.Add(dr1);
                    }
                    //End TT#1368-MD -jsobek -Header Filters - Remove InUsebyMulti status option
                }

            }
            return dtHeaderStatuses;
        }
        public static string HeaderStatusGetNameFromIndex(int fieldIndex)
        {
            if (dtHeaderStatuses == null)
            {
                HeaderStatusesGetDataTable();
            }
            DataRow[] drFind = dtHeaderStatuses.Select("FIELD_INDEX=" + fieldIndex.ToString());
            return (string)drFind[0]["FIELD_NAME"];
        }
        public static filterDataTypes HeaderStatusGetDataType(int fieldIndex)
        {
            return new filterDataTypes(filterValueTypes.Text);
        }


        private static DataTable dtStores = null;
        public static DataTable StoresGetDataTable()
        {
            if (dtStores == null)
            {
                dtStores = new DataTable();
                dtStores.Columns.Add("STORE_NAME");
                dtStores.Columns.Add("STORE_RID", typeof(int));

                foreach (StoreProfile sp in StoreMgmt.StoreProfiles_GetActiveStoresList()) //SAB.StoreServerSession.GetActiveStoresList())
                {
                    DataRow dr1 = dtStores.NewRow();
                    dr1["STORE_NAME"] = sp.Text;
                    dr1["STORE_RID"] = sp.Key;
                    dtStores.Rows.Add(dr1);
                }

                // Begin TT#1853-MD - JSmith - Create Str Attribute - When Store List is selected the stores are not in the correct order.  Expect the stores to be in Numerical then Alphabetical in the selection list.
                dtStores.DefaultView.Sort = "STORE_NAME asc";
                dtStores = dtStores.DefaultView.ToTable();
                // End TT#1853-MD - JSmith - Create Str Attribute - When Store List is selected the stores are not in the correct order.  Expect the stores to be in Numerical then Alphabetical in the selection list.
            }
            return dtStores;
        }

        public static DataTable AttributeSetGetDataTable()
        {
          
            DataTable dtAttributeSet = new DataTable();
            dtAttributeSet.Columns.Add("FIELD_NAME");
            dtAttributeSet.Columns.Add("FIELD_INDEX", typeof(int));

            FunctionSecurityProfile _allocationReviewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
            FunctionSecurityProfile userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
			//Begin TT#1517-MD -jsobek -Store Service Optimization
            //ProfileList a1 = SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView, !userAttrSecLvl.AccessDenied);
            ProfileList a1 = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, !userAttrSecLvl.AccessDenied);
			//End TT#1517-MD -jsobek -Store Service Optimization

            foreach (StoreGroupListViewProfile p in a1)
            {
                DataRow dr1 = dtAttributeSet.NewRow();
                dr1["FIELD_NAME"] = p.Name;
                dr1["FIELD_INDEX"] = p.Key;
                dtAttributeSet.Rows.Add(dr1);
            }
   
            return dtAttributeSet;
        }
        public static DataTable AttributeSetGetValuesForGroup(int key)
        {
            DataTable dtAttributeSetValues = new DataTable();
            dtAttributeSetValues.Columns.Add("GROUP_NAME");
            dtAttributeSetValues.Columns.Add("GROUP_NAME_NO_COUNT");
            dtAttributeSetValues.Columns.Add("GROUP_INDEX", typeof(int));

            //FunctionSecurityProfile _allocationReviewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
            //FunctionSecurityProfile userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);

            //ProfileList a1 = SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView, !userAttrSecLvl.AccessDenied);
            StoreGroupProfile p = StoreMgmt.StoreGroup_Get(key); //SAB.StoreServerSession.GetStoreGroup(key);
            //Profile a = a1.FindKey(key);
            if (p != null)
            {
                //StoreGroupListViewProfile p = (StoreGroupListViewProfile)a;
                foreach (StoreGroupLevelProfile v in p.GroupLevels)
                {
                    DataRow dr1 = dtAttributeSetValues.NewRow();
                    dr1["GROUP_NAME"] = v.Name + " [" + v.Stores.Count.ToString() + "]";
                    dr1["GROUP_NAME_NO_COUNT"] = v.Name;
                    dr1["GROUP_INDEX"] = v.Key;
                    dtAttributeSetValues.Rows.Add(dr1);
                }
            }

            return dtAttributeSetValues;
        }
        public static string AttributeSetGetNameFromIndex(int key)
        {
            FunctionSecurityProfile _allocationReviewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
            FunctionSecurityProfile userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
			//Begin TT#1517-MD -jsobek -Store Service Optimization
            //ProfileList a1 = SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView, !userAttrSecLvl.AccessDenied);
            ProfileList a1 = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, !userAttrSecLvl.AccessDenied);
			//End TT#1517-MD -jsobek -Store Service Optimization
			
            Profile a = a1.FindKey(key);
            if (a != null)
            {
                StoreGroupListViewProfile p = (StoreGroupListViewProfile)a;
                return p.Name;
            }
            else
            {
                return string.Empty;
            }
        }
        public static filterDataTypes AttributeSetGetDataType(int key)
        {
            filterDataTypes v = new filterDataTypes(filterValueTypes.List);
            return v;
        }

        //private static DataTable dtStoreCharValuesForCharGroup;
		// Begin TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
        //public static DataTable StoreCharacteristicsGetValuesForGroup(int groupRID)
        //{
        //    FilterData fd = new FilterData();
        //    return fd.StoreCharacteristicsGetValuesForGroup(groupRID);
        //}
        public static DataTable StoreCharacteristicsGetValuesForGroup(int groupRID)
        {
            return StoreCharacteristicsGetValuesForGroup(groupRID, string.Empty);
        }
        public static DataTable StoreCharacteristicsGetValuesForGroup(int groupRID, string sortString)
        {
            FilterData fd = new FilterData();
            return fd.StoreCharacteristicsGetValuesForGroup(groupRID, sortString);
        }
		// End TT#1925-MD - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
        private static DataTable dtStoreCharacteristics;
        public static DataTable StoreCharacteristicsGetDataTable()
        {
            //if (dtStoreCharacteristics == null) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
            //{
                StoreData storeData = new StoreData();
                DataTable dt = storeData.StoreCharGroup_Read();
                dtStoreCharacteristics = new DataTable();
                dtStoreCharacteristics.Columns.Add("FIELD_NAME");
                dtStoreCharacteristics.Columns.Add("FIELD_INDEX", typeof(int));
                dtStoreCharacteristics.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

                foreach (DataRow dr in dt.Select("", "SCG_ID")) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
                {
                    DataRow drNew = dtStoreCharacteristics.NewRow();
                    drNew["FIELD_NAME"] = dr["SCG_ID"];
                    drNew["FIELD_INDEX"] = dr["SCG_RID"];

                    filterValueTypes vt = StoreCharacteristicsGetValueTypeFromDataRow(dr);
                    drNew["FIELD_VALUE_TYPE_INDEX"] = vt.dbIndex;
                    dtStoreCharacteristics.Rows.Add(drNew);
                }
            //}
            return dtStoreCharacteristics;
        }
        public static DataTable StoreFieldsAndCharGetDataTable()
        {
            DataTable dtFields = filterStoreFieldTypes.ToDataTable();
            StoreData storeData = new StoreData();
            DataTable dtChar = storeData.StoreCharGroup_Read();

            DataTable dtCombined = new DataTable();
            dtCombined.Columns.Add("OBJECT_NAME");
            dtCombined.Columns.Add("FIELD_NAME");
            dtCombined.Columns.Add("OBJECT_TYPE", typeof(int));
            dtCombined.Columns.Add("FIELD_INDEX", typeof(int));

            foreach (DataRow dr in dtFields.Rows) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
            {
                DataRow drNew = dtCombined.NewRow();
                drNew["OBJECT_NAME"] = "Field";
                drNew["FIELD_NAME"] = dr["FIELD_NAME"];
                drNew["OBJECT_TYPE"] = storeObjectTypes.StoreFields.Index;
                drNew["FIELD_INDEX"] = ((int)dr["FIELD_INDEX"]) * -1;
                dtCombined.Rows.Add(drNew);
            }

            foreach (DataRow dr in dtChar.Select("", "SCG_ID")) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
            {
                DataRow drNew = dtCombined.NewRow();
                drNew["OBJECT_NAME"] = "Characteristic";
                drNew["FIELD_NAME"] = dr["SCG_ID"];
                drNew["OBJECT_TYPE"] = storeObjectTypes.StoreCharacteristics.Index;
                drNew["FIELD_INDEX"] = dr["SCG_RID"];
                dtCombined.Rows.Add(drNew);
            }
            return dtCombined;
        }
        public static filterValueTypes StoreCharacteristicsGetValueTypeFromDataRow(DataRow dr)
        {
            int scgType = (int)dr["SCG_TYPE"];
            string isStoreCharList = (string)dr["SCG_LIST_IND"];
            //public enum eStoreCharType
            //{
            //    text=0,
            //    date=1,
            //    number=2,
            //    dollar=3,
            //    unknown=4,
            //    list = 5,
            //    boolean = 6
            //}
            filterValueTypes vt = filterValueTypes.Text;
            if (isStoreCharList == "1")
            {
                vt = filterValueTypes.List;
            }
            else
            {
                if (scgType == 0)
                {
                    vt = filterValueTypes.Text;
                }
                else if (scgType == 1)
                {
                    vt = filterValueTypes.Date;
                }
                else if (scgType == 2)
                {
                    vt = filterValueTypes.Numeric;
                }
                else if (scgType == 3)
                {
                    vt = filterValueTypes.Dollar;
                }
            }
            return vt;
        }
        public static filterDataTypes StoreCharacteristicsGetDataType(int fieldIndex)
        {
            DataRow[] drFind = dtStoreCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
            return GetDataTypeForCharacteristics(filterValueTypes.FromIndex(vtIndex));
        }
        public static string StoreCharacteristicsGetNameFromIndex(int fieldIndex)
        {
            if (dtStoreCharacteristics == null)
            {
                StoreCharacteristicsGetDataTable();
            }
            DataRow[] drFind = dtStoreCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                return (string)drFind[0]["FIELD_NAME"];
            }
            else
            {
                return "Unknown Characteristic";
            }
        }
        public static int StoreCharacteristicsGetFirstIndex()
        {
            if (dtStoreCharacteristics == null)
            {
                StoreCharacteristicsGetDataTable();
            }

            if (dtStoreCharacteristics.Rows.Count > 0)
            {
                return (int)dtStoreCharacteristics.Rows[0]["FIELD_INDEX"];
            }
            else
            {
                return -1;
            }
        }



        private static filterDataTypes GetDataTypeForCharacteristics(filterValueTypes vt)
        {
            filterDataTypes vInfo;
            if (vt == filterValueTypes.Dollar)
            {
                vInfo = new filterDataTypes(vt, filterNumericTypes.Dollar);
            }
            else if (vt == filterValueTypes.Numeric)
            {
                vInfo = new filterDataTypes(vt, filterNumericTypes.DoubleFreeForm);
            }
            else if (vt == filterValueTypes.Date)
            {
                vInfo = new filterDataTypes(vt, filterDateTypes.DateOnly);
            }
            else
            {
                vInfo = new filterDataTypes(vt);
            }

            return vInfo;
        }







        //private static DataTable dtHeaderCharValuesForCharGroup;
        public static DataTable HeaderCharacteristicsGetValuesForGroup(int groupRID)
        {
            FilterData fd = new FilterData();
            return fd.HeaderCharacteristicsGetValuesForGroup(groupRID);
        }
        private static DataTable dtHeaderCharacteristics;
        public static DataTable HeaderCharacteristicsGetDataTable()
        {
            //if (dtHeaderCharacteristics == null) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
            //{
                HeaderCharacteristicsData hdcData = new HeaderCharacteristicsData();
                DataTable dt = hdcData.HeaderCharGroup_Read();
                dtHeaderCharacteristics = new DataTable();
                dtHeaderCharacteristics.Columns.Add("FIELD_NAME");
                dtHeaderCharacteristics.Columns.Add("FIELD_INDEX", typeof(int));
                dtHeaderCharacteristics.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

                foreach (DataRow dr in dt.Select("", "HCG_ID")) //TT#1476-MD -jsobek -Filter - Store and Header Characteristics
                {
                    DataRow drNew = dtHeaderCharacteristics.NewRow();
                    drNew["FIELD_NAME"] = dr["HCG_ID"];
                    drNew["FIELD_INDEX"] = dr["HCG_RID"];

                    filterValueTypes vt = HeaderCharacteristicsGetValueTypeFromDataRow(dr);
                    drNew["FIELD_VALUE_TYPE_INDEX"] = vt.dbIndex;
                    dtHeaderCharacteristics.Rows.Add(drNew);
                }
            //}
            return dtHeaderCharacteristics;
        }
        public static filterValueTypes HeaderCharacteristicsGetValueTypeFromDataRow(DataRow dr)
        {
            int scgType = (int)dr["HCG_TYPE"];
            string isHeaderCharList = (string)dr["HCG_LIST_IND"];
            //public enum eHeaderCharType
            //{
            //    text=0,
            //    date=1,
            //    number=2,
            //    dollar=3,
            //    unknown=4,
            //    list = 5,
            //    boolean = 6
            //}
            filterValueTypes vt = filterValueTypes.Text;
            if (isHeaderCharList == "1")
            {
                vt = filterValueTypes.List;
            }
            else
            {
                if (scgType == 0)
                {
                    vt = filterValueTypes.Text;
                }
                else if (scgType == 1)
                {
                    vt = filterValueTypes.Date;
                }
                else if (scgType == 2)
                {
                    vt = filterValueTypes.Numeric;
                }
                else if (scgType == 3)
                {
                    vt = filterValueTypes.Dollar;
                }
            }
            return vt;
        }
        public static filterDataTypes HeaderCharacteristicsGetDataType(int fieldIndex)
        {
            // Begin TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
            if (dtHeaderCharacteristics == null)
            {
                HeaderCharacteristicsGetDataTable();
            }
            // End TT#5437 - JSmith - Copying Global Header Folder w filters attached to My filters get Null error 
            DataRow[] drFind = dtHeaderCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
            return GetDataTypeForCharacteristics(filterValueTypes.FromIndex(vtIndex));
        }
        public static string HeaderCharacteristicsGetNameFromIndex(int fieldIndex)
        {
            if (dtHeaderCharacteristics == null)
            {
                HeaderCharacteristicsGetDataTable();
            }
            DataRow[] drFind = dtHeaderCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                return (string)drFind[0]["FIELD_NAME"];
            }
            else
            {
                return "Unknown Characteristic";
            }
        }
        public static int HeaderCharacteristicsGetFirstIndex()
        {
            if (dtHeaderCharacteristics == null)
            {
                HeaderCharacteristicsGetDataTable();
            }

            if (dtHeaderCharacteristics.Rows.Count > 0)
            {
                return (int)dtHeaderCharacteristics.Rows[0]["FIELD_INDEX"];
            }
            else
            {
                return -1;
            }
        }

        public static SessionAddressBlock SAB = null;

        private static DataTable dtVariables;
        public static DataTable VariablesGetDataTable()
        {
            if (dtVariables == null)
            {
                dtVariables = new DataTable();
                dtVariables.Columns.Add("FIELD_NAME");
                dtVariables.Columns.Add("FIELD_INDEX", typeof(int));
                dtVariables.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));
                dtVariables.Columns.Add("NUMERIC_TYPE_INDEX", typeof(int));
                dtVariables.Columns.Add("DATE_TYPE_INDEX", typeof(int));
                dtVariables.Columns.Add("IS_TIME_TOTAL_VARIABLE", typeof(int));
                dtVariables.Columns.Add("VARIABLE_KEY", typeof(int));
                dtVariables.Columns.Add("TIME_TOTAL_KEY", typeof(int));
                dtVariables.Columns.Add("IS_GRADE_VARIABLE", typeof(int));
                dtVariables.Columns.Add("IS_STATUS_VARIABLE", typeof(int));
             


                //PlanComputationsCollection compCollections = new PlanComputationsCollection();
                //IPlanComputationVariables variables = compCollections.GetDefaultComputations().PlanVariables;
                //ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();

                ProfileList variableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
                ProfileList timeTotalVariableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;

                foreach (VariableProfile vp in variableProfList)
                {
                    //_variableList.Add(new ProfileComboObject(varProf.Key, varProf.VariableName, varProf));
                    DataRow dr1 = dtVariables.NewRow();
                    dr1["FIELD_NAME"] = vp.VariableName;
                    dr1["FIELD_INDEX"] = vp.Key;
                    filterValueTypes valueType;
                    filterNumericTypes numericType;
                    filterDateTypes dateType;
                    VariablesGetTypesFromVariableProfile(vp, out valueType, out numericType, out dateType);
                    dr1["FIELD_VALUE_TYPE_INDEX"] = valueType.dbIndex;
                    dr1["NUMERIC_TYPE_INDEX"] = numericType.Index;
                    dr1["DATE_TYPE_INDEX"] = dateType.Index;
                    dr1["IS_TIME_TOTAL_VARIABLE"] = 0;
                    dr1["VARIABLE_KEY"] = vp.Key;
                    dr1["TIME_TOTAL_KEY"] = -1;


                    if (vp.FormatType == eValueFormatType.StoreGrade)
                    {
                        dr1["IS_GRADE_VARIABLE"] = 1;
                    }
                    else
                    {
                        dr1["IS_GRADE_VARIABLE"] = 0;
                    }
                    if (vp.FormatType == eValueFormatType.StoreStatus)
                    {
                        dr1["IS_STATUS_VARIABLE"] = 1;
                    }
                    else
                    {
                        dr1["IS_STATUS_VARIABLE"] = 0;
                    }
                    dtVariables.Rows.Add(dr1);
                }

                foreach (VariableProfile vp in variableProfList)
                {
                    for (int i = 0; i < vp.TimeTotalVariables.Count; i++)
                    {
                        //_variableList.Add(new TimeTotalComboObject(((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).Key, "[Date Total] " + ((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).VariableName, (Profile)varProf.TimeTotalVariables[i], varProf, i + 1));
                        DataRow dr1 = dtVariables.NewRow();
                        dr1["FIELD_NAME"] = "[Date Total] " + ((TimeTotalVariableProfile)vp.TimeTotalVariables[i]).VariableName;
                        dr1["FIELD_INDEX"] = (vp.Key * 1000) + i;
                        filterValueTypes valueType;
                        filterNumericTypes numericType;
                        filterDateTypes dateType;
                        VariablesGetTypesFromVariableProfile(vp, out valueType, out numericType, out dateType);
                        dr1["FIELD_VALUE_TYPE_INDEX"] = valueType.dbIndex;
                        dr1["NUMERIC_TYPE_INDEX"] = numericType.Index;
                        dr1["DATE_TYPE_INDEX"] = dateType.Index;
                        dr1["IS_TIME_TOTAL_VARIABLE"] = 1;
                        dr1["VARIABLE_KEY"] = vp.Key;
                        dr1["TIME_TOTAL_KEY"] = ((TimeTotalVariableProfile)vp.TimeTotalVariables[i]).Key;

                        if (vp.FormatType == eValueFormatType.StoreGrade)
                        {
                            dr1["IS_GRADE_VARIABLE"] = 1;
                        }
                        else
                        {
                            dr1["IS_GRADE_VARIABLE"] = 0;
                        }
                        if (vp.FormatType == eValueFormatType.StoreStatus)
                        {
                            dr1["IS_STATUS_VARIABLE"] = 1;
                        }
                        else
                        {
                            dr1["IS_STATUS_VARIABLE"] = 0;
                        }
                        dtVariables.Rows.Add(dr1);
                    }
                }

              


            }
            return dtVariables;
        }
        private static void VariablesGetTypesFromVariableProfile(VariableProfile vp, out filterValueTypes valueType, out filterNumericTypes numericType, out filterDateTypes dateType)
        {
            eVariableDatabaseType dbType = vp.StoreDatabaseVariableType;
            if (dbType == eVariableDatabaseType.String || dbType == eVariableDatabaseType.Char || vp.FormatType == eValueFormatType.StoreGrade)
            {
                valueType = filterValueTypes.Text;
                numericType = filterNumericTypes.Integer;
                dateType = filterDateTypes.DateOnly;
            }
            else if (dbType == eVariableDatabaseType.DateTime)
            {
                valueType = filterValueTypes.Date;
                numericType = filterNumericTypes.Integer;
                dateType = filterDateTypes.DateOnly;
            }
            else
            {
                valueType = filterValueTypes.Numeric;
                dateType = filterDateTypes.DateOnly;

                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (dbType == eVariableDatabaseType.None)
                {
                    //look at number of display decimals to determine type
                    if (vp.NumDisplayDecimals == 0)
                    {
                        numericType = filterNumericTypes.Integer;
                    }
                    else
                    {
                        numericType = filterNumericTypes.DoubleFreeForm;
                    }
                }
                else
                {
                    if (dbType == eVariableDatabaseType.Integer || dbType == eVariableDatabaseType.BigInteger || vp.FormatType == eValueFormatType.StoreStatus)
                    {
                        numericType = filterNumericTypes.Integer;
                    }
                    else
                    {
                        numericType = filterNumericTypes.DoubleFreeForm;
                    }
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
        }
        public static int VariablesGetIsTimeTotal(int fieldIndex)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                return (int)drFind[0]["IS_TIME_TOTAL_VARIABLE"];
            }
            else
            {
                return 0;
            }
        }
        public static bool VariablesGetIsGrade(int fieldIndex)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                int a = (int)drFind[0]["IS_GRADE_VARIABLE"];
                if (a == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public static bool VariablesGetIsStatus(int fieldIndex)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                int a = (int)drFind[0]["IS_STATUS_VARIABLE"];
                if (a == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public static void VariablesGetTimeTotalKeys(int fieldIndex, out int variableKey, out int timeTotalVariableKey)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                variableKey = (int)drFind[0]["VARIABLE_KEY"];
                timeTotalVariableKey = (int)drFind[0]["TIME_TOTAL_KEY"];
            }
            else
            {
                variableKey = -1;
                timeTotalVariableKey = -1;
            }
        }
        public static filterDataTypes VariablesGetDataType(int fieldIndex)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }

            
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());
            filterDataTypes vInfo;
            if (drFind.Length > 0)
            {
                filterValueTypes vt = filterValueTypes.FromIndex((int)drFind[0]["FIELD_VALUE_TYPE_INDEX"]);
                
                if (vt == filterValueTypes.Dollar)
                {
                    vInfo = new filterDataTypes(vt, filterNumericTypes.Dollar);
                }
                else if (vt == filterValueTypes.Numeric)
                {
                    filterNumericTypes nt = filterNumericTypes.FromIndex((int)drFind[0]["NUMERIC_TYPE_INDEX"]);
                    vInfo = new filterDataTypes(vt, nt);
                }
                else if (vt == filterValueTypes.Date)
                {
                    filterDateTypes dt = filterDateTypes.FromIndex((int)drFind[0]["DATE_TYPE_INDEX"]);
                    vInfo = new filterDataTypes(vt, dt);
                }
                else
                {
                    vInfo = new filterDataTypes(vt);
                }
            }
            else
            {
                filterValueTypes vt = filterValueTypes.Numeric;
                filterNumericTypes nt = filterNumericTypes.DoubleFreeForm;
                vInfo = new filterDataTypes(vt, nt);
            }


            return vInfo;
        }
        public static string VariablesGetNameFromIndex(int fieldIndex)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_INDEX=" + fieldIndex.ToString());

            if (drFind.Length > 0)
            {
                return (string)drFind[0]["FIELD_NAME"];
            }
            else
            {
                return "Unknown Variable";
            }
        }
        public static int VariablesGetIndexFromName(string varName)
        {
            if (dtVariables == null)
            {
                VariablesGetDataTable();
            }
            DataRow[] drFind = dtVariables.Select("FIELD_NAME='" + varName + "'");

            if (drFind.Length > 0)
            {
                return (int)drFind[0]["FIELD_INDEX"];
            }
            else
            {
                return -1;
            }
        }


        private static DataTable dtVersions;
        public static DataTable VersionsGetDataTable()
        {
            //if (dtVersions == null)
            //{
                dtVersions = new DataTable();
                dtVersions.Columns.Add("FIELD_NAME");
                dtVersions.Columns.Add("FIELD_INDEX", typeof(int));
                dtVersions.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

                ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                DataRow dr1 = dtVersions.NewRow();
                dr1["FIELD_NAME"] = "Default to Plan";
                dr1["FIELD_INDEX"] = -1;
                dr1["FIELD_VALUE_TYPE_INDEX"] = filterValueTypes.Text.dbIndex;
                dtVersions.Rows.Add(dr1);
                foreach (VersionProfile versionProf in versionProfList)
                {
               
                    DataRow dr2 = dtVersions.NewRow();
                    dr2["FIELD_NAME"] = versionProf.Description;
                    dr2["FIELD_INDEX"] = versionProf.Key;
                    dr2["FIELD_VALUE_TYPE_INDEX"] = filterValueTypes.Text.dbIndex;
                    dtVersions.Rows.Add(dr2);
                }


            //}
            return dtVersions;
        }
        public static filterValueTypes VersionsGetValueType(int fieldIndex)
        {
            if (dtVersions == null)
            {
                VersionsGetDataTable();
            }
            DataRow[] drFind = dtVersions.Select("FIELD_INDEX=" + fieldIndex.ToString());
            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
            return filterValueTypes.FromIndex(vtIndex);
        }
        public static string VersionsGetNameFromIndex(int fieldIndex)
        {
            if (dtVersions == null)
            {
                VersionsGetDataTable();
            }
            DataRow[] drFind = dtVersions.Select("FIELD_INDEX=" + fieldIndex.ToString());
            return (string)drFind[0]["FIELD_NAME"];
        }
        public static int VersionsGetIndexFromName(string varName)
        {
            if (dtVersions == null)
            {
                VersionsGetDataTable();
            }
            DataRow[] drFind = dtVersions.Select("FIELD_NAME='" + varName + "'");
            return (int)drFind[0]["FIELD_INDEX"];
        }


        //Begin TT#1388-MD -jsobek -Product Filters
        public static DataTable ProductCharacteristicsGetValuesForGroup(int groupRID)
        {
            MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
            return mhd.Hierarchy_CharGetValuesForGroup(groupRID);
        }
        private static DataTable dtProductCharacteristics;
        public static DataTable ProductCharacteristicsGetDataTable()
        {
            //always read in case new characteristics were created by the user
            //if (dtProductCharacteristics == null)
            //{
                //ProductData ProductData = new ProductData();
                //DataTable dt = ProductData.ProductCharGroup_Read();
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dt = mhd.Hierarchy_CharGroup_Read();
                dtProductCharacteristics = new DataTable();
                dtProductCharacteristics.Columns.Add("FIELD_NAME");
                dtProductCharacteristics.Columns.Add("FIELD_INDEX", typeof(int));
                dtProductCharacteristics.Columns.Add("FIELD_VALUE_TYPE_INDEX", typeof(int));

                foreach (DataRow dr in dt.Select())
                {
                    DataRow drNew = dtProductCharacteristics.NewRow();
                    drNew["FIELD_NAME"] = dr["HCG_ID"];
                    drNew["FIELD_INDEX"] = dr["HCG_RID"];

                    filterValueTypes vt = ProductCharacteristicsGetValueTypeFromDataRow(dr);
                    drNew["FIELD_VALUE_TYPE_INDEX"] = vt.dbIndex;
                    dtProductCharacteristics.Rows.Add(drNew);
                }
            //}
            return dtProductCharacteristics;
        }
        public static filterValueTypes ProductCharacteristicsGetValueTypeFromDataRow(DataRow dr)
        {
            //int scgType = (int)dr["SCG_TYPE"];
            //string isProductCharList = (string)dr["SCG_LIST_IND"];
            //public enum eProductCharType
            //{
            //    text=0,
            //    date=1,
            //    number=2,
            //    dollar=3,
            //    unknown=4,
            //    list = 5,
            //    boolean = 6
            //}
            filterValueTypes vt = filterValueTypes.List;
            //if (isProductCharList == "1")
            //{
            //    vt = filterValueTypes.List;
            //}
            //else
            //{
            //    if (scgType == 0)
            //    {
            //        vt = filterValueTypes.Text;
            //    }
            //    else if (scgType == 1)
            //    {
            //        vt = filterValueTypes.Date;
            //    }
            //    else if (scgType == 2)
            //    {
            //        vt = filterValueTypes.Numeric;
            //    }
            //    else if (scgType == 3)
            //    {
            //        vt = filterValueTypes.Dollar;
            //    }
            //}
            return vt;
        }
        public static filterDataTypes ProductCharacteristicsGetDataType(int fieldIndex)
        {
            DataRow[] drFind = dtProductCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            int vtIndex = (int)drFind[0]["FIELD_VALUE_TYPE_INDEX"];
            return GetDataTypeForCharacteristics(filterValueTypes.FromIndex(vtIndex));
        }
        public static string ProductCharacteristicsGetNameFromIndex(int fieldIndex)
        {
            if (dtProductCharacteristics == null)
            {
                ProductCharacteristicsGetDataTable();
            }
            DataRow[] drFind = dtProductCharacteristics.Select("FIELD_INDEX=" + fieldIndex.ToString());
            if (drFind.Length > 0)
            {
                return (string)drFind[0]["FIELD_NAME"];
            }
            else
            {
                return "Unknown Characteristic";
            }
        }
        public static int ProductCharacteristicsGetFirstIndex()
        {
            if (dtProductCharacteristics == null)
            {
                ProductCharacteristicsGetDataTable();
            }

            if (dtProductCharacteristics.Rows.Count > 0)
            {
                return (int)dtProductCharacteristics.Rows[0]["FIELD_INDEX"];
            }
            else
            {
                return -1;
            }
        }



        private static DataTable dtProductLevels;
        public static DataTable ProductLevelsGetDataTable()
        {
            if (dtProductLevels == null)
            {
                dtProductLevels = new DataTable();
                dtProductLevels.Columns.Add("FIELD_NAME");
                dtProductLevels.Columns.Add("FIELD_INDEX", typeof(int));

                MerchandiseHierarchyData md = new MerchandiseHierarchyData();
                int organizationalPhRID = md.Hierarchy_Read_Organizational_RID();
                DataTable dt = md.HierarchyLevels_Read(organizationalPhRID);


                foreach (DataRow dr in dt.Rows)
                {
                    DataRow dr1 = dtProductLevels.NewRow();
                    dr1["FIELD_NAME"] = (string)dr["PHL_ID"];
                    dr1["FIELD_INDEX"] = Convert.ToInt32(dr["PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                    dtProductLevels.Rows.Add(dr1);
                }
            }
            return dtProductLevels;
        }


        private static DataTable dtProductHierarchies;
        public static DataTable ProductHierarchiesGetDataTable()
        {
            //always read in case new hierarchies were created by the user
            //if (dtProductHierarchies == null)
            //{
                dtProductHierarchies = new DataTable();
                dtProductHierarchies.Columns.Add("FIELD_NAME");
                dtProductHierarchies.Columns.Add("FIELD_INDEX", typeof(int));

                MerchandiseHierarchyData md = new MerchandiseHierarchyData();
                int organizationalPhRID = md.Hierarchy_Read_Organizational_RID();
                //DataTable dt = md.Hierarchy_Read();

                HierarchyNodeList viewableNodeList = SAB.HierarchyServerSession.GetRootNodes();


                DataTable dtSort = new DataTable();
                dtSort.Columns.Add("PH_RID", typeof(int));
                dtSort.Columns.Add("PH_ID", typeof(string));

                foreach (HierarchyNodeProfile hnp in viewableNodeList)
                {
                    DataRow dr1 = dtSort.NewRow();
                    dr1["PH_RID"] = hnp.HierarchyRID;


                    string hierName = hnp.Text;

                    if (hnp.HomeHierarchyType == eHierarchyType.alternate && hnp.HomeHierarchyOwner != Include.GlobalUserRID)
                    {
                        string ownerName = UserNameStorage.GetUserName(hnp.HomeHierarchyOwner);
                        hierName += " (" + ownerName + ")";
                    }
                    dr1["PH_ID"] = hierName;
                    dtSort.Rows.Add(dr1);
                }

                DataView dv = new DataView(dtSort);
                dv.Sort = "PH_ID";


                foreach (DataRow dr in dtSort.Rows)
                {
                    int phRID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);

                    if (phRID == organizationalPhRID) //show organizational hierarchy first, then sort by id
                    {
                        DataRow dr1 = dtProductHierarchies.NewRow();
                        dr1["FIELD_NAME"] = (string)dr["PH_ID"];
                        dr1["FIELD_INDEX"] = phRID;
                        dtProductHierarchies.Rows.Add(dr1);
                    }
                }

                foreach (DataRowView rowView in dv)
                {
                    int phRID = Convert.ToInt32(rowView.Row["PH_RID"], CultureInfo.CurrentUICulture);
                    //int ownerRID = Convert.ToInt32(rowView.Row["PH_OWNER"], CultureInfo.CurrentUICulture);

                    if (phRID != organizationalPhRID)
                    {
                        DataRow dr1 = dtProductHierarchies.NewRow();
                        dr1["FIELD_NAME"] = (string)rowView.Row["PH_ID"];
                        dr1["FIELD_INDEX"] = phRID;
                        dtProductHierarchies.Rows.Add(dr1);
                    }
                }
            //}
            return dtProductHierarchies;
        }
        //End TT#1388-MD -jsobek -Product Filters

        //Begin TT#1443-MD -jsobek -Audit Filter
        public static DataSet dsUsersAndGroups;
        public static DataTable dtActiveUsersWithGroupRID;
        public static DataSet GetActiveUserDataset()
        {
            if (dsUsersAndGroups == null)
            {
                SecurityAdmin secAdmin = new SecurityAdmin();

                DataTable dtActiveGroups = secAdmin.GetActiveGroupsNameFirst();
                DataRow drSystemGroup = dtActiveGroups.NewRow();
                drSystemGroup["GROUP_NAME"] = "System";
                drSystemGroup["GROUP_RID"] = -2;
                drSystemGroup["GROUP_DESCRIPTION"] = "";
                drSystemGroup["GROUP_ACTIVE_IND"] = "";
                dtActiveGroups.Rows.Add(drSystemGroup);

                dtActiveUsersWithGroupRID = secAdmin.GetActiveUsersWithGroupRID();

                foreach (DataRow drUser in dtActiveUsersWithGroupRID.Rows)
                {
                    string userName = string.Empty;
                    if (drUser["USER_NAME"] != DBNull.Value)
                    {
                        userName = (string)drUser["USER_NAME"];
                    }
                    string fullName = string.Empty;
                    if (drUser["USER_FULLNAME"] != DBNull.Value)
                    {
                        fullName = (string)drUser["USER_FULLNAME"];
                    }

                    if (fullName != string.Empty)
                    {
                        fullName = " (" + fullName + ")";
                    }

                    drUser["USER_FULLNAME"] = userName + fullName;
                }

                DataRow drSystemAdmin = dtActiveUsersWithGroupRID.NewRow();
                drSystemAdmin["USER_FULLNAME"] = "administrator";
                drSystemAdmin["USER_RID"] = 2;
                drSystemAdmin["USER_NAME"] = "administrator";
                drSystemAdmin["USER_DESCRIPTION"] = "";
                drSystemAdmin["GROUP_RID"] = -2;
                dtActiveUsersWithGroupRID.Rows.Add(drSystemAdmin);

                DataRow drGlobalUser = dtActiveUsersWithGroupRID.NewRow();
                drGlobalUser["USER_FULLNAME"] = "global";
                drGlobalUser["USER_RID"] = 4;
                drGlobalUser["USER_NAME"] = "global";
                drGlobalUser["USER_DESCRIPTION"] = "";
                drGlobalUser["GROUP_RID"] = -2;
                dtActiveUsersWithGroupRID.Rows.Add(drGlobalUser);

                DataRow drSystemUser = dtActiveUsersWithGroupRID.NewRow();
                drSystemUser["USER_FULLNAME"] = "system";
                drSystemUser["USER_RID"] = 3;
                drSystemUser["USER_NAME"] = "system";
                drSystemUser["USER_DESCRIPTION"] = "";
                drSystemUser["GROUP_RID"] = -2;
                dtActiveUsersWithGroupRID.Rows.Add(drSystemUser);

                dsUsersAndGroups = new DataSet();

                dsUsersAndGroups.Tables.Add(dtActiveGroups);
                dsUsersAndGroups.Tables.Add(dtActiveUsersWithGroupRID);

                dsUsersAndGroups.Relations.Add("GroupsToUsers", dtActiveGroups.Columns["GROUP_RID"], dtActiveUsersWithGroupRID.Columns["GROUP_RID"], false);

                //this.midSelectMultiNodeControl1.ShowRootLines = true;
                //this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "GroupsToUsers";

                //this.midSelectMultiNodeControl1.FieldToTag = "USER_RID";
                //this.midSelectMultiNodeControl1.BindDataSet(dsUsersAndGroups);
            }
            return dsUsersAndGroups;
        }
        //Begin TT#1443-MD -jsobek -Audit Filter
    }
}
