using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using System.Text.RegularExpressions;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
    public static class ExceptionHandler
    {
        // Begin TT#1808-MD - JSmith - Store Load Error
        //Begin TT#1313-MD -jsobek -Header Filters -Allow for common exception handling across application
        //public static SessionAddressBlock SABforExceptions;
        //public static void HandleException(Exception ex, string caption = "Error:")
        //{
        //    //Add to audit log.
        //    if (SABforExceptions != null && SABforExceptions.ClientServerSession != null && SABforExceptions.ClientServerSession.Audit != null)
        //    {
        //        SABforExceptions.ClientServerSession.Audit.Log_Exception(ex);
        //    }

        //    //Show message on screen.
        //    System.Windows.Forms.MessageBox.Show("Error: " + ex.ToString(), caption);
        //}
        //public static void HandleException(string err, string caption = "Error:")
        //{
        //    //Add to audit log.
        //    if (SABforExceptions != null && SABforExceptions.ClientServerSession != null && SABforExceptions.ClientServerSession.Audit != null)
        //    {
        //        SABforExceptions.ClientServerSession.Audit.Log_Exception(err);
        //    }

        //    //Show message on screen.
        //    System.Windows.Forms.MessageBox.Show("Error: " + err, caption);
        //}

        private static Session _session;
        private static bool _showMessageBox;

        public static void Initialize(Session session, bool showMessageBox = false)
        {
            _session = session;
            _showMessageBox = showMessageBox;
        }

        public static void HandleException(Exception ex, string caption = "Error:")
        {
            //Add to audit log.
            if (_session != null && _session.Audit != null)
            {
                _session.Audit.Log_Exception(ex);
            }

            //Show message on screen.
            if (_showMessageBox)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + ex.ToString(), caption);
            }
        }
        public static void HandleException(string err, string caption = "Error:")
        {
            //Add to audit log.
            if (_session != null && _session.Audit != null)
            {
                _session.Audit.Log_Exception(err);
            }

            //Show message on screen.
            if (_showMessageBox)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + err, caption);
            }
        }
        // End TT#1808-MD - JSmith - Store Load Error

        //End TT#1313-MD -jsobek -Header Filters -Allow for common exception handling across application
        public static bool InDebugMode; //TT#1372-MD -jsobek -Filters - Move Condition Display Menu
    }

    //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
    public static class InUseUtility
    {

        public static string GetInUseTitleFromProfileType(eProfileType etype)
        {
            string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
            return inUseTitle;
        }
    }
    //End TT#1532-MD -jsobek -Add In Use for Header Characteristics

    public class HeaderCharList
    {
        #region Fields
        //=======
        // FIELDS
        //=======

        #endregion Fields
        public HeaderCharList()
        {
        }

        /// <summary>
        /// Returns an ArrayList or sorted header and characteristic entries.
        /// </summary>
        /// <remarks>Header fields have negative keys to not duplicate characteristic RIDs</remarks>
        /// <returns></returns>
        public ArrayList BuildHeaderCharList()
        {
            try
            {
                string fieldName;
                HeaderCharacteristicsData hdcData = new HeaderCharacteristicsData();
                DataTable dt = hdcData.HeaderCharGroup_Read();

                SortedList entryList = new SortedList();
                HeaderFieldCharEntry oe;

                foreach (DataRow row in dt.Rows)
                {

                    oe = new HeaderFieldCharEntry('C', Convert.ToInt32(row["HCG_RID"]), Convert.ToString(row["HCG_ID"]), (eHeaderCharType)Convert.ToInt32(row["HCG_TYPE"]));
                    entryList.Add(oe.Text, oe);
                }

                // Add Header fields with negative keys as to not duplicate entries
                eHeaderCharType dataType;
                foreach (filterHeaderFieldTypes fieldType in filterHeaderFieldTypes.fieldTypeList)
                {
                    if (fieldType.dataType.valueType == filterValueTypes.Boolean)
                    {
                        dataType = eHeaderCharType.boolean;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Date)
                    {
                        dataType = eHeaderCharType.date;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Dollar)
                    {
                        dataType = eHeaderCharType.dollar;
                    }
                    else if (fieldType.dataType.valueType == filterValueTypes.Numeric)
                    {
                        dataType = eHeaderCharType.number;
                    }
                    else
                    {
                        dataType = eHeaderCharType.text;
                    }
                    fieldName = fieldType.Name;
                    if (fieldType.dbIndex == 2
                        || fieldType.dbIndex == 3)
                    {
                        fieldName += " [Level]";
                    }
                    oe = new HeaderFieldCharEntry('H', fieldType.dbIndex * -1, fieldName, dataType);
                    entryList.Add(oe.Text, oe);
                }

                ArrayList alEntries = new ArrayList();
                IDictionaryEnumerator dictEnum = entryList.GetEnumerator();

                while (dictEnum.MoveNext())
                {
                    oe = (HeaderFieldCharEntry)dictEnum.Value;
                    alEntries.Add(oe);
                }

                return alEntries;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetHeaderField(SessionAddressBlock SAB, filterHeaderFieldTypes fieldType, AllocationProfile ap)
        {

            if (fieldType == filterHeaderFieldTypes.HeaderID)
            {
                return ap.HeaderID;
            }
            else if (fieldType == filterHeaderFieldTypes.PO) //purchase order
            {
                return ap.PurchaseOrder;
            }
            else if (fieldType == filterHeaderFieldTypes.Vendor)
            {
                return ap.Vendor;
            }
            else if (fieldType == filterHeaderFieldTypes.DC) //distribution center
            {
                return ap.DistributionCenter;
            }
            else if (fieldType == filterHeaderFieldTypes.VswID) //aka IMO ID
            {
                return ap.ImoID;
            }
            else if (fieldType == filterHeaderFieldTypes.ShipStatus)
            {
                return MIDText.GetTextOnly(Convert.ToInt32(ap.HeaderShipStatus));
            }
            else if (fieldType == filterHeaderFieldTypes.IntransitStatus)
            {
                return MIDText.GetTextOnly(Convert.ToInt32(ap.HeaderIntransitStatus));
            }
            else if (fieldType == filterHeaderFieldTypes.QuantityToAllocate)
            {
                return ap.TotalUnitsToAllocate;
            }
            else if (fieldType == filterHeaderFieldTypes.SubClass) //Parent of style base node
            {
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                return SAB.HierarchyServerSession.GetNodeData(hnp.HomeHierarchyParentRID).Text;
            }
            else if (fieldType == filterHeaderFieldTypes.Style)
            {
                return SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID).Text;
            }
            else if (fieldType == filterHeaderFieldTypes.NumPacks)
            {
                return ap.PackCount;
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkColors)
            {
                return ap.BulkColorCount;
            }
            else if (fieldType == filterHeaderFieldTypes.NumBulkSizes)
            {
                return ap.BulkColorSizeCount;
            }
            else if (fieldType == filterHeaderFieldTypes.SizeGroup)
            {
                SizeGroupProfile sgp = new SizeGroupProfile(ap.SizeGroupRID, false);
                return sgp.SizeGroupName;
            }
            else if (fieldType == filterHeaderFieldTypes.VswProcess)
            {
                if (ap.AdjustVSW_OnHand)
                {
                    return "Adjust";
                }
                else
                {
                    return "Replace";
                }
            }

            return null;
        }

        public object GetHeaderCharacteristic(SessionAddressBlock SAB, string HCG_ID, AllocationProfile ap)
        {
            return ((CharacteristicsBin)ap.Characteristics[HCG_ID]).Value;
        }
    }

    public class HeaderFieldCharEntry
    {
        private char _type;
        private int _key;
        private string _text;
        private eHeaderCharType _fieldDataType;

        public HeaderFieldCharEntry(char cType, int iKey, string sText, eHeaderCharType fieldDataType)
        {
            _type = cType;
            _key = iKey;
            _text = sText;
            _fieldDataType = fieldDataType;
        }

        /// <summary>
        /// Gets the type of override entry.
        /// </summary>
        public char Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the key of override entry.
        /// </summary>
        public int Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets the text of override entry.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Gets the data type of the entry.
        /// </summary>
        public eHeaderCharType FieldDataType
        {
            get { return _fieldDataType; }
        }

    }

    public class GetName
    {
        private static HierarchyProfile _mainHierProf = null;
        private static DataTable _merchandiseDataTable = null;
        public static KeyValuePair<int, string> GetFilterName(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                FilterData filterData = new FilterData();
                name = filterData.FilterGetName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetGroupByName(int key)
        {
            string name = string.Empty;

            if (key > Include.UndefinedGroupByRID)
            {
                name = MIDText.GetTextFromCode(key);

            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetStoreName(int key)
        {

            string name = string.Empty;
            if (key > Include.NoRID)
            {
                name = StoreMgmt.StoreProfile_Get(key).Text;
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetStoreStatus(eStoreStatus storeStatus)
        {
            string status = MIDText.GetTextOnly((int)storeStatus);
            return new KeyValuePair<int, string>((int)storeStatus, status);
        }

        public static KeyValuePair<int, string> GetAttributeName(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                name = StoreMgmt.StoreGroup_GetName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetAttributeSetName(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                name = StoreMgmt.StoreGroupLevel_GetName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }


        public static KeyValuePair<int, string> GetAllocationViewName(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                GridViewData gridViewData = new GridViewData();
                DataRow row = gridViewData.GridView_Read(key);
                if (row != null)
                {
                    name = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                }
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetForecastViewName(int key, int userKey)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                PlanViewData planViewData = new PlanViewData();
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(userKey);
                DataTable dt = planViewData.PlanView_Read(userRIDList);
                DataRow[] dr = dt.Select(@"VIEW_RID = '" + key + @"'");
                if (dr.Length > 0)
                {
                    name = Convert.ToString(dr[0]["VIEW_ID"], CultureInfo.CurrentUICulture);
                }
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetSizeGroup(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                SizeGroup sizeGroupData = new SizeGroup(); name = sizeGroupData.GetSizeGroupName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetSizeCurveName(int sizeCurveRID)
        {
            string sizeCurveName = string.Empty;
            if (sizeCurveRID > Include.NoRID)
            {
                MerchandiseHierarchyData merchHierData = new MerchandiseHierarchyData();
                DataTable dt = merchHierData.SizeCurveName_Read(sizeCurveRID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    sizeCurveName = Convert.ToString(dr["CURVE_NAME"], CultureInfo.CurrentUICulture).Trim();
                }
            }
            return new KeyValuePair<int, string>(sizeCurveRID, sizeCurveName);
        }


        public static KeyValuePair<int, string> GetSizeCurveGroupName(int sizeCurveGroupRID)
        {
            string sizeCurveGroupName = string.Empty;

            if (sizeCurveGroupRID > Include.NoRID)

            { SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(sizeCurveGroupRID); sizeCurveGroupName = scgp.SizeCurveGroupName; }
            return new KeyValuePair<int, string>(sizeCurveGroupRID, sizeCurveGroupName);
        }

        public static KeyValuePair<int, string> GetPrioritizeHeaderBy(char prioritizeType, int hcg_RID, int headerField)
        {
            string displayName = string.Empty;
            int key = Include.NoRID;
            if (prioritizeType == 'C')
            {
                key = hcg_RID;
            }
            else
            {
                key = headerField;
            }
            if (key != Include.NoRID)
            {
                HeaderCharList hcl = new HeaderCharList();
                ArrayList array = hcl.BuildHeaderCharList();
                foreach (HeaderFieldCharEntry hfce in array)
                {
                    if (hfce.Key == key)
                    {
                        displayName = hfce.Text.ToString();
                        break;
                    }
                }
            }
            return new KeyValuePair<int, string>(key, displayName);
        }

        public static KeyValuePair<int, string> GetSizeAlternateModel(int sizeAlternateRID)
        {
            string sizeAlternateName = string.Empty;
            if (sizeAlternateRID > Include.NoRID)
            {
                SizeModelData sizeModelData = new SizeModelData();
                DataTable dt = sizeModelData.SizeAlternateModel_Read(sizeAlternateRID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    sizeAlternateName = Convert.ToString(dr["SIZE_ALTERNATE_NAME"], CultureInfo.CurrentUICulture).Trim();
                }
            }
            return new KeyValuePair<int, string>(sizeAlternateRID, sizeAlternateName);
        }

        //works for both primary and alternate
        public static KeyValuePair<int, string> GetSizeAlternateModelSize(int sizeCurveRID, int sizeCodeRID)
        {

            string name = string.Empty;

            if (sizeCurveRID != Include.NoRID)
            {
                SizeCurveGroupProfile sizeCurveGroupProfile = new SizeCurveGroupProfile(sizeCurveRID);
                ProfileList scl = sizeCurveGroupProfile.SizeCodeList;
                if (scl.Count > 0)
                {
                    foreach (SizeCodeProfile scp in scl.ArrayList)
                    {
                        if (scp.Key != -1)
                        {
                            if (scp.SizeCodePrimaryRID == sizeCodeRID)
                            {
                                name = scp.SizeCodePrimary.ToString();
                            }
                        }
                    }
                }
            }
            return new KeyValuePair<int, string>(sizeCodeRID, name);
        }

        //works for primary and alternate
        public static KeyValuePair<int, string> GetSizeAlternateModelDimension(int sizeCurveRID, int dimensionRID)
        {

            string name = string.Empty;

            if (sizeCurveRID != Include.NoRID)
            {
                SizeCurveGroupProfile sizeCurveGroupProfile = new SizeCurveGroupProfile(sizeCurveRID);
                ProfileList scl = sizeCurveGroupProfile.SizeCodeList;
                if (scl.Count > 0)
                {
                    foreach (SizeCodeProfile scp in scl.ArrayList)
                    {
                        if (scp.Key != -1)
                        {
                            if (scp.SizeCodeSecondaryRID == dimensionRID)
                            {
                                name = scp.SizeCodeSecondary.ToString();
                            }
                        }
                    }
                }
            }
            return new KeyValuePair<int, string>(dimensionRID, name);
        }

        public static KeyValuePair<int, string> GetSizeConstraint(int sizeConstraintRID)
        {
            string sizeConstraintName = string.Empty;
            if (sizeConstraintRID > Include.NoRID)
            {
                DataTable dtSizeModel;
                SizeModelData sizeModelData = new SizeModelData();
                dtSizeModel = sizeModelData.SizeConstraintModel_Read(sizeConstraintRID);

                if (dtSizeModel.Rows.Count > 0)
                {
                    DataRow dr = dtSizeModel.Rows[0];
                    sizeConstraintName = Convert.ToString(dr["SIZE_CONSTRAINT_NAME"], CultureInfo.CurrentUICulture).Trim();
                }
            }
            return new KeyValuePair<int, string>(sizeConstraintRID, sizeConstraintName);
        }

        public static KeyValuePair<int, string> GetHeaderCharGroupProfile(int genConstraintHcgRID)
        {
            string constraintGenericHeaderCharName = string.Empty;
            int headerCharKey = Include.NoRID;
            if (genConstraintHcgRID > Include.NoRID)
            {
                HeaderCharacteristicsData _headerCharacteristicsData = new HeaderCharacteristicsData();
                DataTable dt = _headerCharacteristicsData.HeaderCharGroup_Read();
                
                foreach (DataRow dr in dt.Rows)
                {
                    headerCharKey = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
                    if (headerCharKey == genConstraintHcgRID)
                    {
                        constraintGenericHeaderCharName = Convert.ToString(dr["HCG_ID"], CultureInfo.CurrentUICulture);
                    }
                }
            }
            return new KeyValuePair<int, string>(genConstraintHcgRID, constraintGenericHeaderCharName);
        }

        public static KeyValuePair<int, string> GetLevelKeyValuePair(eMerchandiseType merchandiseType, int nodeRID, int merchPhRID, int merchPhlSequence, SessionAddressBlock SAB)
        {
            //load key value pair based on Level Type
            int key = Include.NoRID;
            string name = string.Empty;
            KeyValuePair<int, string> getLevelKeyValuePair;
            switch (merchandiseType)
            {
                case eMerchandiseType.HierarchyLevel:
                case eMerchandiseType.LevelOffset:
                    name = Convert.ToString(GetLevelName(eROLevelsType.HierarchyLevel, merchPhlSequence, 0, SAB));
                    key = merchPhlSequence;
                    break;
                case eMerchandiseType.OTSPlanLevel:
                    name = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                    key = Include.Undefined;
                    break;
                default: //eMerchandiseType.Node
                    getLevelKeyValuePair = (GetMerchandiseName(nodeRID, SAB));
                    key = getLevelKeyValuePair.Key;
                    name = getLevelKeyValuePair.Value;
                    break;
            }
                return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetHierarchyName(int hierarchyRID, SessionAddressBlock SAB)
        {
            string hierarchyName = string.Empty;

            if (hierarchyRID > Include.NoRID)

            {
                hierarchyName = SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID: hierarchyRID).HierarchyID;
            }
            return new KeyValuePair<int, string>(hierarchyRID, hierarchyName);
        }

        public static KeyValuePair<int, string> GetMerchandiseName(int nodeRID, SessionAddressBlock SAB)
        {
            string merchandiseName = string.Empty;

            if (nodeRID > Include.NoRID)

            { merchandiseName = SAB.HierarchyServerSession.GetNodeData(aNodeRID: nodeRID, aChaseHierarchy: true, aBuildQualifiedID: true).Text; }
            return new KeyValuePair<int, string>(nodeRID, merchandiseName);
        }

        public static KeyValuePair<int, string> GetCalendarDateRange(int calendarDateRID, SessionAddressBlock SAB, int anchorDateRID = Include.UndefinedCalendarDateRange)
        {
            string displayDate = string.Empty;

            if (calendarDateRID > Include.NoRID)
            {
                DateRangeProfile drp;
                if (anchorDateRID != Include.UndefinedCalendarDateRange)
                {
                    drp = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateRID, anchorDateRID);
                }
                else
                {
                    drp = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateRID);
                }
                displayDate = drp.DisplayDate;
            }
            return new KeyValuePair<int, string>(calendarDateRID, displayDate);
        }

        public static KeyValuePair<int, string> GetCalendarDateRange(int calendarDateRID, SessionAddressBlock SAB, DayProfile day)
        {
            string displayDate = string.Empty;

            if (calendarDateRID > Include.NoRID)
            {
                DateRangeProfile drp;
                if (day != null)
                {
                    drp = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateRID, day);
                }
                else
                {
                    drp = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateRID);
                }
                displayDate = drp.DisplayDate;
            }
            return new KeyValuePair<int, string>(calendarDateRID, displayDate);
        }

        public static KeyValuePair<int, string> GetMethod(ApplicationBaseMethod method)
        {
            string displayName = string.Empty;
            int key = Include.NoRID;

            if (method.Key > Include.NoRID)

            { key = method.Key; displayName = method.Name; }
            else if (method.Name != null)
            {
                displayName = method.Name;
            }
            return new KeyValuePair<int, string>(key, displayName);
        }

        public static KeyValuePair<int, string> GetMethod(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                WorkflowMethodData methodData = new WorkflowMethodData();
                name = methodData.GetMethodName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetWorkflowName(int key)
        {
            string name = string.Empty;
            if (key > Include.NoRID)
            {
                WorkflowBaseData workflowData = new WorkflowBaseData();
                name = workflowData.GetWorkflowName(key);
            }
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetHeader(int headerRID, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (headerRID > Include.NoRID)

            { AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(headerRID, false, false, true); displayName = ahp.HeaderID; }
            return new KeyValuePair<int, string>(headerRID, displayName);
        }

        public static KeyValuePair<int, string> GetComponent(eComponentType componentType)
        {
            string displayName = string.Empty;
            int component = componentType.GetHashCode();

            displayName = MIDText.GetTextOnly(component);
            return new KeyValuePair<int, string>(component, displayName);
        }

        public static KeyValuePair<int, string> GetRuleMethod(eRuleMethod ruleMethodType)
        {
            string displayName = string.Empty;
            int ruleMethod = ruleMethodType.GetHashCode();

            displayName = MIDText.GetTextOnly(ruleMethod);
            return new KeyValuePair<int, string>(ruleMethod, displayName);
        }

        public static KeyValuePair<int, string> GetColor(int colorCodeRID, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (colorCodeRID > Include.NoRID)
            {
                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeRID);
                if (ccp != null)
                {
                    displayName = ccp.ColorCodeName;
                }
            }
            else
            {
                displayName = MIDText.GetTextOnly(eComponentType.AllColors.GetHashCode());
            }
            return new KeyValuePair<int, string>(colorCodeRID, displayName);
        }

        public static KeyValuePair<int, string> GetSize(int sizesRID, SessionAddressBlock SAB) // input to this can be sizes_rid or size_code_rid
        {
            string displayName = string.Empty;

            if (sizesRID > Include.NoRID)
            {
                SizeCodeProfile sizeCodeProfile  = SAB.HierarchyServerSession.GetSizeCodeProfile(sizesRID);
                if (sizeCodeProfile != null)
                {
                    displayName = sizeCodeProfile.SizeCodePrimary;
                }
            }
            return new KeyValuePair<int, string>(sizesRID, displayName);
        }

        public static KeyValuePair<int, string> GetDimension(int dimensionRID, int sizeGroupRID, int sizeCurveGroupRID,
            eGetSizes getSizesUsing, eGetDimensions getDimensionsUsing, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;
            int RID = Include.NoRID;
            //DataTable dtSizes = new DataTable();

            if (dimensionRID > Include.NoRID)
            {
                if (getSizesUsing == eGetSizes.SizeGroupRID)
                {
                    RID = sizeGroupRID;
                    SizeGroupProfile sgp = new SizeGroupProfile(RID);
                    foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
                    {
                        if (Convert.ToInt32(scp.SizeCodeSecondaryRID) == Convert.ToInt32(dimensionRID))
                        {
                            displayName = scp.SizeCodeSecondary.ToString();
                            break;
                        }
                        if (Convert.ToInt32(scp.SizeCodePrimaryRID) == Convert.ToInt32(dimensionRID))
                        {
                            displayName = scp.SizeCodePrimary.ToString();
                            break;
                        }
                        if (Convert.ToInt32(scp.Key) == Convert.ToInt32(dimensionRID))
                        {
                            if (scp.SizeCodeName.Length > 1)
                            {
                                displayName = scp.SizeCodeName.ToString();
                                break;
                            }
                            else
                            {
                                displayName = scp.SizeCodePrimary.ToString();
                                break;
                            }
                        }
                    }
                }
                else  // (getSizesUsing == eGetSizes.SizeGroupRID)
                {
                    RID = sizeCurveGroupRID;
                    SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(RID);
                    foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
                    {
                        if (Convert.ToInt32(scp.SizeCodeSecondaryRID) == Convert.ToInt32(dimensionRID))
                        {
                            displayName = scp.SizeCodeSecondary.ToString();
                            break;
                        }
                        if (Convert.ToInt32(scp.SizeCodePrimaryRID) == Convert.ToInt32(dimensionRID))
                        {
                            displayName = scp.SizeCodePrimary.ToString();
                            break;
                        }
                        if (Convert.ToInt32(scp.Key) == Convert.ToInt32(dimensionRID))
                        {
                            if (scp.SizeCodeName.Length > 1 )
                            {
                                displayName = scp.SizeCodeName.ToString();
                                break;
                            }
                            else
                            {
                                displayName = scp.SizeCodePrimary.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                displayName = Include.NoneText;
            }
            return new KeyValuePair<int, string>(dimensionRID, displayName);
        }

        public static KeyValuePair<int, string> GetBasisSizeMethodRuleType(int ruleRID)
        {
            string displayName = string.Empty;
            if (ruleRID > Include.NoRID)
            {
                eBasisSizeMethodRuleType bsm = (eBasisSizeMethodRuleType)(Convert.ToInt32(ruleRID, CultureInfo.CurrentUICulture));
                if (Enum.IsDefined(typeof(eBasisSizeMethodRuleType), bsm))
                {
                    displayName = bsm.ToString();
                }
            }
            return new KeyValuePair<int, string>(ruleRID, displayName);
        }

        public static KeyValuePair<int, string> GetColorOrSizeComponent(int componentRID)
        {
            string displayName = string.Empty;
            if (componentRID > Include.NoRID)
            {
                eComponentType ct = (eComponentType)(Convert.ToInt32(componentRID, CultureInfo.CurrentUICulture));
                if (Enum.IsDefined(typeof(eComponentType), ct))
                {
                    displayName = ct.ToString();
                }
            }
            return new KeyValuePair<int, string>(componentRID, displayName);
        }
        public static KeyValuePair<int, string> GetHeaderPack(int headerRID, int packRID)
        {
            string displayName = string.Empty;

            if (headerRID > Include.NoRID)
            {
                int hdrPackRID = -1;
                Header header = new Header();
                DataTable dt = header.GetPacks(headerRID);
                foreach (DataRow dr in dt.Rows)
                {
                    hdrPackRID = Convert.ToInt32(dr["HDR_PACK_RID"]);
                    if (packRID == hdrPackRID)
                    {
                        displayName = Convert.ToString(dr["HDR_PACK_NAME"]);
                        break;
                    }
                }
            }
            return new KeyValuePair<int, string>(packRID, displayName);
        }

        public static KeyValuePair<int, string> GetVersion(int versionRID, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (versionRID > Include.NoRID)
            {
                ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                if (versionProfList != null)
                {
                    VersionProfile vp = (VersionProfile)versionProfList.FindKey(versionRID);
                    if (vp != null)
                    {
                        displayName = vp.Description;
                    }

                }
            }
            return new KeyValuePair<int, string>(versionRID, displayName);
        }

        public static KeyValuePair<int, string> GetOverrideLowLevelsModel(int modelRID, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (modelRID > Include.NoRID)
            {
                ModelsData modelsData = new ModelsData();
                DataTable dt;
                dt = modelsData.OverrideLowLevelsModel_Read(modelRID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    if (dr["NAME"] != DBNull.Value && Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture) != Include.NoRID)
                    {
                        displayName = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture).Trim();
                    }
                }
            }
            return new KeyValuePair<int, string>(modelRID, displayName);
        }
        public static string GetLevelName(eROLevelsType levelType, int levelSequence, int levelOffset, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (levelType == eROLevelsType.LevelOffset)
            {
                displayName = "+" + levelOffset.ToString();
            }
            else if (levelType == eROLevelsType.HierarchyLevel
                && levelSequence >= 0)
            {
                if (_mainHierProf == null)
                {
                    _mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();
                }
                if (levelSequence == 0)
                {
                    displayName = _mainHierProf.HierarchyID;
                }
                else
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_mainHierProf.HierarchyLevels[levelSequence];
                    displayName = hlp.LevelID;
                }
            }
            return displayName;
        }

        public static KeyValuePair<int, string> GetVariable(int variableKey, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;

            if (variableKey > Include.NoRID)
            {
                VariableProfile vp = (VariableProfile)SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList.FindKey(variableKey);
                if (vp != null)
                {
                    displayName = vp.VariableName;
                }
            }
            return new KeyValuePair<int, string>(variableKey, displayName);
        }

        public static KeyValuePair<eModifySalesRuleType, string> GetSalesRule(eModifySalesRuleType salesRuleType)
        {
            string displayName = string.Empty;

            displayName = MIDText.GetTextOnly(salesRuleType.GetHashCode());

            return new KeyValuePair<eModifySalesRuleType, string>(salesRuleType, displayName);
        }

        public static KeyValuePair<int, string> GetForecastBalanceModel(int modelRID, SessionAddressBlock SAB)
        {
            string displayName = string.Empty;
            if (modelRID > Include.NoRID)
            {
                ModelsData modelsData = new ModelsData();
                DataTable dt;
                dt = modelsData.ForecastBalModel_Read(modelRID);
                if (dt.Rows.Count > 0)
                { DataRow dr = dt.Rows[0]; displayName = Convert.ToString(dr["FBMOD_ID"], CultureInfo.CurrentUICulture).Trim(); }
            }
            return new KeyValuePair<int, string>(modelRID, displayName);
        }

        public static KeyValuePair<eGroupLevelFunctionType, string> GetForecastMethodType(eGroupLevelFunctionType functionType)
        {
            string displayName = string.Empty;

            if (!Enum.IsDefined(typeof(eGroupLevelFunctionType), functionType))
            {
                functionType = eGroupLevelFunctionType.PercentContribution;
            }

            displayName = MIDText.GetTextOnly(functionType.GetHashCode());

            return new KeyValuePair<eGroupLevelFunctionType, string>(functionType, displayName);
        }

        public static KeyValuePair<eGroupLevelSmoothBy, string> GetSmoothByType(eGroupLevelSmoothBy smoothByType)
        {
            string displayName = string.Empty;

            if (!Enum.IsDefined(typeof(eGroupLevelSmoothBy), smoothByType))
            {
                smoothByType = eGroupLevelSmoothBy.None;
            }

            displayName = MIDText.GetTextOnly(smoothByType.GetHashCode());

            return new KeyValuePair<eGroupLevelSmoothBy, string>(smoothByType, displayName);
        }

        public static KeyValuePair<int, string> GetUser(SessionAddressBlock SAB)
        {
            return new KeyValuePair<int, string>(SAB.ClientServerSession.UserRID, SAB.ClientServerSession.UserID);
        }

        public static KeyValuePair<int, string> GetUser(int userKey)
        {
            return new KeyValuePair<int, string>(userKey, UserNameStorage.GetUserName(userKey));
        }

        public static KeyValuePair<int, string> GetStoreCharGroup(int scgRID, SessionAddressBlock SAB) 
        {
            string displayName = string.Empty;

            if (scgRID > Include.NoRID)
            {
                StoreCharMaint storeCharMaint = new StoreCharMaint();
                DataTable dt = new DataTable();
                dt = storeCharMaint.StoreCharGroup_Read(scgRID);

                if (dt.Rows.Count > 0)
                {
                    displayName = dt.Rows[0]["SCG_ID"].ToString();
                }
            }
            return new KeyValuePair<int, string>(scgRID, displayName);
        }

        public static void BuildDataTables(int nodeRID, SessionAddressBlock SAB)
        {
            
            try
            {
                _merchandiseDataTable  = MIDEnvironment.CreateDataTable();
                DataColumn myDataColumn;
                DataRow myDataRow;
                // Create new DataColumn, set DataType, ColumnName and add to DataTable.  
                // Level sequence number
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "seqno";
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = true;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create second column - enum name.
                //Create Merchandise types - eMerchandiseType
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "leveltypename";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create third column - text
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.String");
                myDataColumn.ColumnName = "text";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create fourth column - Key
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "key";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                _mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                //Default Selection to OTSPlanLevel
                myDataRow = _merchandiseDataTable.NewRow();
                myDataRow["seqno"] = 0;
                myDataRow["leveltypename"] = eMerchandiseType.OTSPlanLevel;
                myDataRow["text"] = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                myDataRow["key"] = Include.Undefined;
                _merchandiseDataTable.Rows.Add(myDataRow);

                for (int levelIndex = 1; levelIndex <= _mainHierProf.HierarchyLevels.Count; levelIndex++)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_mainHierProf.HierarchyLevels[levelIndex];
                    //hlp.LevelID is level name 
                    //hlp.Level is level number 
                    //hlp.LevelType is level type 
                    // BEGIN MID Track #4094 - as part of this Track, exclude size level from drop down list
                    if (hlp.LevelType != eHierarchyLevelType.Size)
                    {
                        myDataRow = _merchandiseDataTable.NewRow();
                        myDataRow["seqno"] = hlp.Level;
                        myDataRow["leveltypename"] = eMerchandiseType.HierarchyLevel;
                        myDataRow["text"] = hlp.LevelID;
                        myDataRow["key"] = hlp.Key;
                        _merchandiseDataTable.Rows.Add(myDataRow);
                    }
                    // END MID Track #4094
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
    public class VariableGroupings
    {
        /// <summary>
        /// Builds a list of variable by grouping.
        /// </summary>
        /// <param name="planType">The ePlanType of the request</param>
        /// <param name="variables">The list of RowColProfileHeader objects containing the variables</param>
        /// <param name="groupings">The list of strings containing the group names</param> 
        public static ROVariableGroupings BuildVariableGroupings(ePlanType planType, ArrayList variables, ArrayList groupings)
        {
            ROVariable variable;
            List<ROVariable> groupVariables;
            ROVariableGrouping grouping;
            List<ROVariableGrouping> variableGrouping = new List<ROVariableGrouping>();
            ROVariableGroupings variableGroupings = new ROVariableGroupings(variableGrouping);

            MIDRetail.DataCommon.RowColProfileHeader rowColProfileHeader = (MIDRetail.DataCommon.RowColProfileHeader)variables[0];
            if (rowColProfileHeader.Profile.GetType() == typeof(MIDRetail.DataCommon.TimeTotalVariableProfile))
            {
                UpdateSelectableTimeTotalList(planType, variables);
            }
            else
            {
                UpdateSelectableList(planType, variables);
            }


            foreach (string groupName in groupings)
            {
                groupVariables = new List<ROVariable>();
                foreach (RowColProfileHeader vp in variables)
                {
                    if (vp.Grouping == groupName)
                    {
                        variable = new ROVariable(vp.Profile.Key, vp.Name, vp.IsSelectable, vp.IsDisplayed, vp.Sequence);
                        groupVariables.Add(variable);
                    }
                }
                grouping = new ROVariableGrouping(name: groupName, variables: groupVariables);
                variableGroupings.VariableGrouping.Add(grouping);
            }

            return variableGroupings;
        }

        private static void UpdateSelectableList(ePlanType aPlanType, ArrayList aSelectableList)
        {
            try
            {
                switch (aPlanType)
                {
                    case ePlanType.Chain:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Chain)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }

                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                        }

                        break;

                    case ePlanType.Store:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Store)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }
                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                        }

                        break;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private static void UpdateSelectableTimeTotalList(ePlanType aPlanType, ArrayList aSelectableList)
        {
            try
            {
                switch (aPlanType)
                {
                    case ePlanType.Chain:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Chain)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }

                            if (rowHeader.Profile.GetType() == typeof(TimeTotalVariableProfile))
                            {
                                rowHeader.Grouping = ((TimeTotalVariableProfile)rowHeader.Profile).ParentVariableProfile.Groupings;
                            }
                        }

                        break;

                    case ePlanType.Store:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Store)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }
                            if (rowHeader.Profile.GetType() == typeof(TimeTotalVariableProfile))
                            {
                                rowHeader.Grouping = ((TimeTotalVariableProfile)rowHeader.Profile).ParentVariableProfile.Groupings;
                            }
                        }

                        break;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
    }
    public class SizeCurveProperties
    {
        /// <summary>
        /// Builds the variables for the Size Curve Group box
        /// </summary>
        /// input parameters
        /// <param name="sizeCurveGroupRID"></param>
        /// <param name="genCurveNsccdRID"></param>
        /// <param name="genCurveHcgRID"></param> 
        /// <param name="genCurveHnRID"></param> 
        /// <param name="genCurvePhRID"></param> 
        /// <param name="genCurvePhlSequence"></param> 
        /// <param name="genCurveMerchType"></param> 
        /// <param name="isUseDefault"></param> 
        /// <param name="isApplyRulesOnly"></param> 
        /// output parameters
        /// <param name="sizeCurve"></param> 
        /// <param name="sizeCurveGenericHierarchy"></param> 
        /// <param name="sizeCurveGenericNameExtension"></param> 
        /// <param name="SAB"></param>

        public static ROSizeCurveProperties BuildSizeCurveProperties(int sizeCurveGroupRID, int genCurveNsccdRID, int genCurveHcgRID, int genCurveHnRID, int genCurvePhRID, int genCurvePhlSequence, eMerchandiseType genCurveMerchType,
        bool isUseDefault, bool isApplyRulesOnly, KeyValuePair<int, string> sizeCurve, KeyValuePair<int, string> sizeCurveGenericHierarchy, KeyValuePair<int, string> sizeCurveGenericNameExtension, SessionAddressBlock SAB)
        {
            sizeCurve =  GetName.GetSizeCurveGroupName(sizeCurveGroupRID);
            sizeCurveGenericHierarchy = GetName.GetLevelKeyValuePair(genCurveMerchType, genCurveHnRID, genCurvePhRID, genCurvePhlSequence, SAB);
            sizeCurveGenericNameExtension = GetName.GetSizeCurveName(genCurveNsccdRID);
            
            ROSizeCurveProperties sizeCurveProperties = new ROSizeCurveProperties(sizeCurveGroupRID, genCurveNsccdRID, genCurveHcgRID, genCurveHnRID, genCurvePhRID, genCurvePhlSequence, EnumTools.VerifyEnumValue(genCurveMerchType),
            isUseDefault, isApplyRulesOnly, sizeCurve,sizeCurveGenericHierarchy, sizeCurveGenericNameExtension);

            return sizeCurveProperties;
        }
    }

    public class SizeConstraintProperties
    {
        /// <summary>
        /// Builds the variables for the Size Constraint Group box
        /// </summary>
        
        public static ROSizeConstraintProperties BuildSizeConstraintProperties(int inventoryBasisMerchHnRID, int inventoryBasisMerchPhRID, int inventoryBasisMerchPhlSequence, eMerchandiseType inventoryBasisMerchType,
            int sizeConstraintRID, int genConstraintHcgRID, int genConstraintHnRID, int genConstraintPhRID, int genConstraintPhlSequence, eMerchandiseType genConstraintMerchType,
            bool genConstraintColorInd, KeyValuePair<int, string> inventoryBasis, KeyValuePair<int, string> sizeConstraint, KeyValuePair<int, string> sizeConstraintGenericHierarchy, KeyValuePair<int, string> sizeConstraintGenericHeaderChar, SessionAddressBlock SAB)
        {
            inventoryBasis = GetName.GetLevelKeyValuePair(inventoryBasisMerchType, nodeRID: inventoryBasisMerchHnRID, merchPhRID: inventoryBasisMerchPhRID, merchPhlSequence: inventoryBasisMerchPhlSequence, SAB: SAB);
            sizeConstraint  = GetName.GetSizeConstraint(sizeConstraintRID);
            sizeConstraintGenericHierarchy = GetName.GetLevelKeyValuePair(genConstraintMerchType, genConstraintHnRID, genConstraintPhRID, genConstraintPhlSequence, SAB);
            sizeConstraintGenericHeaderChar = GetName.GetHeaderCharGroupProfile(genConstraintHcgRID);
            
            ROSizeConstraintProperties sizeConstraintProperties = new ROSizeConstraintProperties(inventoryBasisMerchHnRID, inventoryBasisMerchPhRID, inventoryBasisMerchPhlSequence, EnumTools.VerifyEnumValue(inventoryBasisMerchType),
            sizeConstraintRID, genConstraintHcgRID, genConstraintHnRID, genConstraintPhRID, genConstraintPhlSequence, EnumTools.VerifyEnumValue(genConstraintMerchType),
            genConstraintColorInd, inventoryBasis, sizeConstraint, sizeConstraintGenericHierarchy, sizeConstraintGenericHeaderChar);

            return sizeConstraintProperties;
        }
    }

    public class SizeRuleAttributeSet
    {
        /// <summary>
        /// Builds the RO Size Rule Attribute Set lists for the Size Methods' Rule Tab from the Dataset MethodConstraints
        /// </summary>

        public static ROMethodSizeRuleAttributeSet BuildSizeRuleAttributeSet(int methodRID, eMethodType methodType, int attributeRID, int sizeGroupRID, int sizeCurveGroupRID, eGetSizes getSizesUsing, eGetDimensions getDimensionsUsing , DataSet methodConstraints,
            SessionAddressBlock SAB)
        {
            
            DataTable combinedMethodConstrints = new DataTable();
            DataColumn dataColumn;
            bool success = false;
            int number = 0;
            eSizeMethodRowType sizeMethodRowType = eSizeMethodRowType.Set;
            string newColumnName = string.Empty;     

            //Create Columns and rows for datatable
            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.String");
            dataColumn.ColumnName = "BAND_DSC";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "METHOD_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "SGL_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "COLOR_CODE_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "SIZES_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "SIZE_CODE_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "DIMENSIONS_RID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.String");
            dataColumn.ColumnName = "SIZE_RULE";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "SIZE_QUANTITY";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "ROW_TYPE_ID";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataColumn.ColumnName = "SIZE_SEQ";
            dataColumn.ReadOnly = false;
            dataColumn.Unique = false;
            combinedMethodConstrints.Columns.Add(dataColumn);

            if (methodConstraints.Tables.Count > 0)
            {
                for (int t = 0; t < methodConstraints.Tables.Count; t++)
                {
                    for (int r = 0; r < methodConstraints.Tables[t].Rows.Count; r++)
                    {
                        DataRow drIn = methodConstraints.Tables[t].Rows[r];
                        DataRow dr = combinedMethodConstrints.NewRow();

                        for (int c = 0; c < methodConstraints.Tables[t].Columns.Count; c++)
                        {
                            newColumnName = methodConstraints.Tables[t].Columns[c].ColumnName.ToString();

                            switch (newColumnName)
                            {
                                case "BAND_DSC":
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = " ";
                                    }
                                    else
                                    {
                                        dr[newColumnName] = drIn[c].ToString();
                                    }

                                    break;

                                case "METHOD_RID":
                                case "SGL_RID":
                                case "COLOR_CODE_RID":
                                case "SIZES_RID":
                                case "DIMENSIONS_RID":
                                case "SIZE_CODE_RID":
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = Include.NoRID;
                                    }
                                    else
                                    {
                                        dr[newColumnName] = Convert.ToInt32(drIn[c]);
                                    }

                                    break;

                                case "SIZE_RULE": // THIE IS EITHER NULL OR NUMERIC BUT TABLE IS VARCHAR
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = " ";
                                    }
                                    else
                                    {
                                        dr[newColumnName] = drIn[c].ToString();
                                    }
                                    break;

                                case "SIZE_QUANTITY": // theis column should be numeric
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = " ";
                                    }
                                    else
                                    {
                                        success = Int32.TryParse(drIn[c].ToString(), out number);
                                        if (success)
                                        {
                                            dr[newColumnName] = Convert.ToInt32(drIn[c]);
                                        }
                                        else
                                        {
                                            dr[newColumnName] = 0;
                                        }
                                    }
                                    break;

                                case "SIZE_SEQ":
                                    if (drIn[c] == null) //default the seq to be = 0 for the sorting of the list (this would be the set records)
                                    {
                                        dr[newColumnName] = 0;
                                    }
                                    else
                                    {
                                        success = Int32.TryParse(drIn[c].ToString(), out number);
                                        if (success)
                                        {
                                            dr[newColumnName] = Convert.ToInt32(drIn[c]);
                                        }
                                        else
                                        {
                                            dr[newColumnName] = 0;
                                        }
                                    }
                                    break;

                                case "ROW_TYPE_ID": // type 8 = default but should sequence to a 1 for the sort
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = eSizeMethodRowType.Set; //default
                                    }
                                    else
                                    {
                                        success = Enum.TryParse((drIn[c].ToString()), out sizeMethodRowType);
                                        if (success)
                                        {
                                            if (sizeMethodRowType == eSizeMethodRowType.Default)
                                            {
                                                dr[newColumnName] = eSizeMethodRowType.Set; // set row type id to 1 for sort
                                            }
                                            else
                                            {
                                                dr[newColumnName] = sizeMethodRowType;
                                            }
                                        }
                                        else
                                        {
                                            dr[newColumnName] = eSizeMethodRowType.Set; //default
                                        }
                                    }

                                    break;
                            
                                default:
                                    if (drIn[c] == null)
                                    {
                                        dr[newColumnName] = " ";
                                    }
                                    else
                                    {
                                        if (dr[newColumnName].GetType() == System.Type.GetType("System.Int32"))
                                        {
                                            success = Int32.TryParse(drIn[c].ToString(), out number);
                                            if (success)
                                            {
                                                dr[newColumnName] = Convert.ToInt32(drIn[c]);
                                            }
                                            else
                                            {
                                                dr[newColumnName] = drIn[c].ToString();
                                            }
                                        }
                                        else
                                        {
                                            dr[newColumnName] = drIn[c].ToString();
                                        }
                                    }
                                    break;
                            }
                        }
                        combinedMethodConstrints.Rows.Add(dr);
                    }
                }
            }

            DataTable sortedMethodConstrints = new DataTable();

            if (combinedMethodConstrints.Rows.Count > 0)
            {
                combinedMethodConstrints.DefaultView.Sort = "METHOD_RID, SGL_RID, ROW_TYPE_ID, SIZE_SEQ";
                sortedMethodConstrints = combinedMethodConstrints.DefaultView.ToTable();
            }

            List<ROMethodSizeRuleProperties> rOMethodSizeRules = new List<ROMethodSizeRuleProperties>();
            KeyValuePair<int, string> sgl = new KeyValuePair<int, string>();
            KeyValuePair<int, string> colorCode = new KeyValuePair<int, string>();
            KeyValuePair<int, string> sizes = new KeyValuePair<int, string>();
            KeyValuePair<int, string> dimensions = new KeyValuePair<int, string>();
            KeyValuePair<int, string> sizeCode = new KeyValuePair<int, string>();
            KeyValuePair<int, string> sizeRule= new KeyValuePair<int, string>();
            eSizeMethodRowType methodRowType = eSizeMethodRowType.Set; //default
            double sizeQuantity = 0;
            Int32 sizeSEQ = 0;
            eSizeRuleType eSizeRuleType = eSizeRuleType.None;
            
            //make output list from storted dataset
            foreach (DataRow row in sortedMethodConstrints.Rows)
            {
                //reset values
                sgl = GetName.GetAttributeSetName(Convert.ToInt32(row["SGL_RID"]));
                colorCode = new KeyValuePair<int, string>();
                sizes = new KeyValuePair<int, string>();
                dimensions = new KeyValuePair<int, string>();
                sizeCode = new KeyValuePair<int, string>();
                sizeRule = new KeyValuePair<int, string>();
                methodRowType = eSizeMethodRowType.Set; //default
                sizeQuantity = 0;
                sizeSEQ = 0;
                
                if (row["COLOR_CODE_RID"] != null && row["COLOR_CODE_RID"].ToString() != "")
                {
                    if (Convert.ToInt32(row["COLOR_CODE_RID"]) > Include.NoRID)
                    {
                        colorCode = GetName.GetColor(Convert.ToInt32(row["COLOR_CODE_RID"]), SAB);
                    }
                    else
                    {
                        colorCode = new KeyValuePair<int, string>(Include.NoRID, "");
                    }
                }
                if (row["SIZES_RID"] != null && row["SIZES_RID"].ToString() != "")
                {
                    if (Convert.ToInt32(row["SIZES_RID"]) > Include.NoRID)
                    {
                        sizes = GetName.GetSize(Convert.ToInt32(row["SIZES_RID"]), SAB);
                    }
                    else
                    {
                        sizes = new KeyValuePair<int, string>(Include.NoRID, "");
                    }
                }
                if (row["DIMENSIONS_RID"] != null && row["DIMENSIONS_RID"].ToString() != "")
                {
                    if (Convert.ToInt32(row["DIMENSIONS_RID"]) > Include.NoRID)
                    {
                        dimensions = GetName.GetDimension(Convert.ToInt32(row["DIMENSIONS_RID"]), sizeGroupRID, sizeCurveGroupRID,
                            getSizesUsing, getDimensionsUsing, SAB);
                    }
                    else
                    {
                        dimensions = new KeyValuePair<int, string>(Include.NoRID, "");
                    }
                }
                if (row["SIZE_CODE_RID"] != null && row["SIZE_CODE_RID"].ToString() != "")
                {
                    if (Convert.ToInt32(row["SIZE_CODE_RID"]) > Include.NoRID)
                    {
                        sizeCode = GetName.GetSize(Convert.ToInt32(row["SIZE_CODE_RID"]), SAB);
                    }
                    else
                    {
                        sizeCode = new KeyValuePair<int, string>(Include.NoRID, "");
                    }
                }

                
                if (row["SIZE_RULE"] == null)
                {
                    sizeRule = new KeyValuePair<int, string>(Include.NoRID, "");
                }
                else
                {
                    success = Enum.TryParse((row["SIZE_RULE"].ToString()), out eSizeRuleType);
                    if (success)
                    {
                        sizeRule = new KeyValuePair<int, string>(Convert.ToInt32(row["SIZE_RULE"]), eSizeRuleType.ToString());
                    }
                    else
                    {
                        sizeRule = new KeyValuePair<int, string>(Include.NoRID, "");
                    }
                }
                
                if (row["SIZE_QUANTITY"] != null && row["SIZE_QUANTITY"].ToString() != "")
                {
                    sizeQuantity = Convert.ToDouble(row["SIZE_QUANTITY"]);
                }
                else
                {
                    sizeQuantity = 0;
                }

                success = Enum.TryParse((row["ROW_TYPE_ID"].ToString()), out sizeMethodRowType);
                if (success)
                {
                    if (sizeMethodRowType == eSizeMethodRowType.Default)
                    {
                        methodRowType = eSizeMethodRowType.Set; // set row type id to 1 for sort
                    }
                    else
                    {
                        methodRowType = sizeMethodRowType;
                    }
                }
                else
                {
                    methodRowType = eSizeMethodRowType.Set; //default
                }
                
                if (row["SIZE_SEQ"] != null && row["SIZE_SEQ"].ToString() != "")
                {
                    sizeSEQ = Convert.ToInt32(row["SIZE_SEQ"]);
                }
                else
                {
                    sizeSEQ = 0;
                }

                ROMethodSizeRuleProperties rOMethodSizeRulesProperties = new ROMethodSizeRuleProperties(false,false,false, row["BAND_DSC"].ToString(), sgl, colorCode, sizes, dimensions, sizeCode, sizeRule, sizeQuantity, EnumTools.VerifyEnumValue(methodRowType), sizeSEQ);
                rOMethodSizeRules.Add(rOMethodSizeRulesProperties);
            }

            ROMethodSizeRuleAttributeSet sizeRuleAttributeSet = new ROMethodSizeRuleAttributeSet();
            sizeRuleAttributeSet.SizeRuleRowsValues = rOMethodSizeRules;
            return sizeRuleAttributeSet;
            
        }
        /// <summary>
        /// Builds the MethodConstraints Dataset from the  RO Size Rule Attribute Set lists for the Size Methods' Rule Tab 
        /// </summary>

        public static DataSet BuildMethodConstrainst(int methodRID, int attributeRID, ROMethodSizeRuleAttributeSet rOMethodSizeRuleAttributeSet, DataSet methodConstraintsSV,
            SessionAddressBlock SAB)
        {

            DataSet methodConstraints = new DataSet();
            methodConstraints = methodConstraintsSV.Clone();
            
            DataRow dataRow;
            DataTable dataTable = methodConstraints.Tables["SetLevel"];

            eSizeMethodRowType sizeMethodRowType = eSizeMethodRowType.Set;
            string newColumnName = string.Empty;
            bool success = false;
            int number = 0;

            //Create dataset from saved RO Size Rule Attribute Set
            if (rOMethodSizeRuleAttributeSet.SizeRuleRowsValues.Count > 0)
            {
                //create table from list
                try
                {
                    foreach (ROMethodSizeRuleProperties rOMethodSizeRules in rOMethodSizeRuleAttributeSet.SizeRuleRowsValues)
                    {
                        sizeMethodRowType = rOMethodSizeRules.RowTypeID;
                        newColumnName = "ROW_TYPE_ID";
                        switch (sizeMethodRowType)
                        {
                            case eSizeMethodRowType.Set:
                                dataTable = methodConstraints.Tables["SetLevel"];
                                dataRow = methodConstraints.Tables["SetLevel"].NewRow();
                                if (rOMethodSizeRules.BandDsc.ToString() == "Default")
                                {
                                    dataRow[newColumnName] = 8; // for default row type id
                                }
                                else
                                {
                                    dataRow[newColumnName] = 1;
                                }
                                break;
                            case eSizeMethodRowType.AllColor:
                                dataTable = methodConstraints.Tables["AllColor"];
                                dataRow = methodConstraints.Tables["AllColor"].NewRow();
                                dataRow[newColumnName] = 2;
                                break;
                            case eSizeMethodRowType.AllColorSize:
                                dataTable = methodConstraints.Tables["AllColorSize"];
                                dataRow = methodConstraints.Tables["AllColorSize"].NewRow();
                                dataRow[newColumnName] = 4;
                                break;
                            case eSizeMethodRowType.AllColorSizeDimension:
                                dataTable = methodConstraints.Tables["AllColorSizeDimension"];
                                dataRow = methodConstraints.Tables["AllColorSizeDimension"].NewRow();
                                dataRow[newColumnName] = 6;
                                break;
                            case eSizeMethodRowType.AllSize:
                                dataTable = methodConstraints.Tables["AllSize"];
                                dataRow = methodConstraints.Tables["AllSize"].NewRow();
                                dataRow[newColumnName] = 0;
                                break;
                            case eSizeMethodRowType.Color:
                                dataTable = methodConstraints.Tables["Color"];
                                dataRow = methodConstraints.Tables["Color"].NewRow();
                                dataRow[newColumnName] = 3;
                                break;
                            case eSizeMethodRowType.ColorSize:
                                dataTable = methodConstraints.Tables["ColorSize"];
                                dataRow = methodConstraints.Tables["ColorSize"].NewRow();
                                dataRow[newColumnName] = 5;
                                break;
                            case eSizeMethodRowType.ColorSizeDimension:
                                dataTable = methodConstraints.Tables["ColorSizeDimension"];
                                dataRow = methodConstraints.Tables["ColorSizeDimension"].NewRow();
                                dataRow[newColumnName] = 7;
                                break;
                            default:
                                dataTable = methodConstraints.Tables["SetLevel"];
                                dataRow = methodConstraints.Tables["SetLevel"].NewRow();
                                dataRow[newColumnName] = 1;
                                break;
                        }

                        if (sizeMethodRowType == eSizeMethodRowType.Set || sizeMethodRowType == eSizeMethodRowType.Default)
                        {
                            newColumnName = "BAND_DSC";
                            if (rOMethodSizeRules.BandDsc == null)
                            {
                                dataRow[newColumnName] = " ";
                            }
                            else
                            {
                                dataRow[newColumnName] = rOMethodSizeRules.BandDsc.ToString();
                            }
                            newColumnName = "METHOD_RID";
                            success = Int32.TryParse(methodRID.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(methodRID);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SGL_RID";
                            success = Int32.TryParse(rOMethodSizeRules.Sgl.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.Sgl.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SIZE_RULE"; // THIE IS EITHER NULL OR NUMERIC BUT TABLE IS VARCHAR
                            success = Int32.TryParse(rOMethodSizeRules.SizeRule.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = rOMethodSizeRules.SizeRule;
                            }
                            else
                            {
                                dataRow[newColumnName] = string.Empty; 
                            }

                            newColumnName = "SIZE_QUANTITY";
                            success = Int32.TryParse(rOMethodSizeRules.SizeQuantity.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.SizeQuantity);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.UndefinedQuantity;
                            }
                            methodConstraints.Tables[dataTable.TableName.ToString()].Rows.Add(dataRow);
                            //dataTable.Rows.Add(dataRow);
                        }
                        else
                        {
                            newColumnName = "BAND_DSC";
                            if (rOMethodSizeRules.BandDsc == null)
                            {
                                dataRow[newColumnName] = " ";
                            }
                            else
                            {
                                dataRow[newColumnName] = rOMethodSizeRules.BandDsc.ToString();
                            }

                            newColumnName = "METHOD_RID";
                            success = Int32.TryParse(methodRID.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(methodRID);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SGL_RID";
                            success = Int32.TryParse(rOMethodSizeRules.Sgl.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.Sgl.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "COLOR_CODE_RID";
                            success = Int32.TryParse(rOMethodSizeRules.ColorCode.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.ColorCode.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SIZES_RID";
                            success = Int32.TryParse(rOMethodSizeRules.Sizes.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.Sizes.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "DIMENSIONS_RID";
                            success = Int32.TryParse(rOMethodSizeRules.Dimensions.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.Dimensions.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SIZE_CODE_RID";
                            success = Int32.TryParse(rOMethodSizeRules.SizeCode.Key.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.SizeCode.Key);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.NoRID;
                            }

                            newColumnName = "SIZE_RULE"; // THIE IS EITHER NULL OR NUMERIC BUT TABLE IS VARCHAR
                            success = Int32.TryParse(rOMethodSizeRules.SizeRule.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = rOMethodSizeRules.SizeRule;
                            }
                            else
                            {
                                dataRow[newColumnName] = string.Empty;
                            }

                            newColumnName = "SIZE_QUANTITY";
                            success = Int32.TryParse(rOMethodSizeRules.SizeQuantity.ToString(), out number);
                            if (success && number > 0)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.SizeQuantity);
                            }
                            else
                            {
                                dataRow[newColumnName] = Include.UndefinedQuantity;
                            }

                            newColumnName = "SIZE_SEQ";
                            success = Int32.TryParse(rOMethodSizeRules.SizeSeq.ToString(), out number);
                            if (success)
                            {
                                dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeRules.SizeSeq);
                            }
                            else
                            {
                                dataRow[newColumnName] = 0;
                            }

                            methodConstraints.Tables[dataTable.TableName.ToString()].Rows.Add(dataRow);
                        }
                    }
                }
                catch (Exception ex)
                { 
                    
                }
            }
            return methodConstraints;
        }
    }
    public class BasisSizeSubstituteSet
    {
        /// <summary>
        /// Builds the RO Basis Size Substitute List for the Basis Size Substitute Tab from the _substituteList
        /// </summary>

        public static ROMethodBasisSizeSubstituteSet BuildBasisSizeSubstituteSet(int methodRID, eMethodType methodType, int attributeRID, int sizeGroupRID, int sizeCurveGroupRID, eGetSizes getSizesUsing, eGetDimensions getDimensionsUsing
            , ArrayList substituteList, SessionAddressBlock SAB)
        {
            List<ROMethodBasisSizeSubstituteProperties> rOMethodBasisSizeSubstitutes = new List<ROMethodBasisSizeSubstituteProperties>();
            KeyValuePair<int, string> sizeType = new KeyValuePair<int, string>();
            KeyValuePair<int, string> substitute = new KeyValuePair<int, string>();
            eEquateOverrideSizeType overrideSizeType = eEquateOverrideSizeType.DimensionSize; //default

            foreach (BasisSizeSubstitute aSizeSub in substituteList)
            {
                overrideSizeType = aSizeSub.SizeType;
                if (!Enum.IsDefined(typeof(eEquateOverrideSizeType), aSizeSub.SizeType))
                {
                    overrideSizeType = eEquateOverrideSizeType.DimensionSize;
                }
                else
                {
                    overrideSizeType = aSizeSub.SizeType;
                }
                
                if (aSizeSub.SizeTypeRid > Include.NoRID)
                {
                    sizeType = GetName.GetDimension(aSizeSub.SizeTypeRid, sizeGroupRID, sizeCurveGroupRID, getSizesUsing, getDimensionsUsing, SAB);
                }
                else
                {
                    sizeType = new KeyValuePair<int, string>(Include.NoRID, "");
                }
                if (aSizeSub.SubstituteRid > Include.NoRID)
                {
                    substitute = GetName.GetDimension(aSizeSub.SubstituteRid, sizeGroupRID, sizeCurveGroupRID, getSizesUsing, getDimensionsUsing, SAB);
                }
                else
                {
                    substitute = new KeyValuePair<int, string>(Include.NoRID, "");
                }
                
                ROMethodBasisSizeSubstituteProperties rOMethodBasisSizeSubstituteProperties = new ROMethodBasisSizeSubstituteProperties(false, false, false, sizeType, substitute, overrideSizeType);
                rOMethodBasisSizeSubstitutes.Add(rOMethodBasisSizeSubstituteProperties);
            }

            ROMethodBasisSizeSubstituteSet basisSizeSubstituteSet = new ROMethodBasisSizeSubstituteSet();
            basisSizeSubstituteSet.BasisSizeSubstituteRowsValues = rOMethodBasisSizeSubstitutes;
            return basisSizeSubstituteSet;
    
        }

        /// <summary>
        /// Builds the MethodConstraints Dataset from the  RO Size Rule Attribute Set lists for the Size Methods' Rule Tab 
        /// </summary>

        public static ArrayList BuildBasisSizeSubstituteList(int methodRID, int attributeRID, ROMethodBasisSizeSubstituteSet rOMethodBasisSizeSubstituteSet)
        {

            ArrayList substituteList  = new ArrayList();
            eEquateOverrideSizeType overrideSizeType = eEquateOverrideSizeType.DimensionSize; //default

            //Create dataset from saved RO Basis Size Substitute Set
            if (rOMethodBasisSizeSubstituteSet.BasisSizeSubstituteRowsValues.Count > 0)
            {
                //create array row from list
                try
                {
                    foreach (ROMethodBasisSizeSubstituteProperties rOMethodBasisSizeSubstitutes in rOMethodBasisSizeSubstituteSet.BasisSizeSubstituteRowsValues)
                    {
                        if (!Enum.IsDefined(typeof(eEquateOverrideSizeType), rOMethodBasisSizeSubstitutes.OverrideSizeType))
                        {
                            overrideSizeType = eEquateOverrideSizeType.DimensionSize;
                        }
                        else
                        {
                            overrideSizeType = rOMethodBasisSizeSubstitutes.OverrideSizeType;
                        }
                        BasisSizeSubstitute aSizeSub = new BasisSizeSubstitute(rOMethodBasisSizeSubstitutes.SizeType.Key, rOMethodBasisSizeSubstitutes.Substitue.Key, overrideSizeType);
                        substituteList.Add(aSizeSub);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return substituteList;
        }
    }
    public class DCStoreCharacteristicSet
    {
        /// <summary>
        /// Builds the RO DC Store Characteristic List from the datatset
        /// </summary>

        public static ROMethodDCStoreCharacteristicSet BuildDCStoreCharacteristicSet(int methodRID, eMethodType methodType, DataTable dtStoreOrder,
            SessionAddressBlock SAB)
        {
            List<ROMethodDCStoreCharacteristicProperties> rOMethodDCStoreCharacteristics = new List<ROMethodDCStoreCharacteristicProperties>();
            KeyValuePair<int, string> scg = new KeyValuePair<int, string>();
            Int32 SEQ = 0;

            foreach (DataRow row in dtStoreOrder.Rows)
            {
                //reset values
                scg = GetName.GetStoreCharGroup(Convert.ToInt32(row["SCG_RID"]), SAB);
                SEQ = 0;

                if (row["SEQ"] != null && row["SEQ"].ToString() != "")
                {
                    SEQ = Convert.ToInt32(row["SEQ"]);
                }
                else
                {
                    SEQ = 0;
                }

                ROMethodDCStoreCharacteristicProperties rOMethodDCStoreCharacteristicProperties = new ROMethodDCStoreCharacteristicProperties(false, false, false, SEQ, row["DIST_CENTER"].ToString(), scg);
                rOMethodDCStoreCharacteristics.Add(rOMethodDCStoreCharacteristicProperties);
            }

            ROMethodDCStoreCharacteristicSet DCStoreCharacteristicSet = new ROMethodDCStoreCharacteristicSet();
            DCStoreCharacteristicSet.DCStoreCharacteristicRowsValues = rOMethodDCStoreCharacteristics;
            return DCStoreCharacteristicSet;
        }
        /// <summary>
        /// Builds the dtStoreOrder Dataset from the RO DCStoreCharacteristic Set list 
        /// </summary>

        public static DataTable BuildDTStoreOrder(int methodRID, ROMethodDCStoreCharacteristicSet rOMethodDCStoreCharacteristicSet, DataTable dtStoreOrderSV,
            SessionAddressBlock SAB)
        {

            DataTable dtStoreOrder = new DataTable();
            dtStoreOrder = dtStoreOrderSV.Clone();
            DataRow dataRow;

            string newColumnName = string.Empty;
            bool success = false;
            int number = 0;

            //Create dataset from saved RO Size Rule Attribute Set
            if (rOMethodDCStoreCharacteristicSet.DCStoreCharacteristicRowsValues.Count > 0)
            {
                //create table from list
                try
                {
                    foreach (ROMethodDCStoreCharacteristicProperties rOMethodDCStoreCharacteristics in rOMethodDCStoreCharacteristicSet.DCStoreCharacteristicRowsValues)
                    {
                        newColumnName = "METHOD_RID";
                        dataRow = dtStoreOrder.NewRow();
                        dataRow[newColumnName] = methodRID ;

                        newColumnName = "DIST_CENTER";
                        if (rOMethodDCStoreCharacteristics.DistCenter == null)
                        {
                            dataRow[newColumnName] = " ";
                        }
                        else
                        {
                            dataRow[newColumnName] = rOMethodDCStoreCharacteristics.DistCenter.ToString();
                        }

                        newColumnName = "SCG_RID";
                        success = Int32.TryParse(rOMethodDCStoreCharacteristics.storeCharacteristics.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodDCStoreCharacteristics.storeCharacteristics.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "SEQ";
                        success = Int32.TryParse(rOMethodDCStoreCharacteristics.Seq.ToString(), out number);
                        if (success)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodDCStoreCharacteristics.Seq);
                        }
                        else
                        {
                            dataRow[newColumnName] = 0;
                        }

                        dtStoreOrder.Rows.Add(dataRow);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return dtStoreOrder;
        }
    }

    public class SizeCurveMerchBasisSet
    {
        /// <summary>
        /// Builds the RO SizeCurveMerchBasisSet List
        /// </summary>

        public static ROMethodSizeCurveMerchBasisSet BuildSizeCurveMerchBasisSet(int methodRID, eMethodType methodType, DataTable dtMerchBasisDetail,
            SessionAddressBlock SAB)
        {
            List<ROMethodSizeCurveMerchBasisProperties> rOMethodSizeCurveMerchBasiss = new List<ROMethodSizeCurveMerchBasisProperties>();
            KeyValuePair<int, string> hn = new KeyValuePair<int, string>();
            KeyValuePair<int, string> fv = new KeyValuePair<int, string>();
            KeyValuePair<int, string> cdr = new KeyValuePair<int, string>();
            KeyValuePair<int, string> oll = new KeyValuePair<int, string>();
            KeyValuePair<int, string> customOll = new KeyValuePair<int, string>();
            decimal weight = 0;
            eMerchandiseType merchType = eMerchandiseType.Node; //default
            Int32 SEQ = 0;

            foreach (DataRow dr in dtMerchBasisDetail.Rows)
            {
                //reset values
                SEQ = 0;

                if (dr["BASIS_SEQ"] != null && dr["BASIS_SEQ"].ToString() != "")
                {
                    SEQ = Convert.ToInt32(dr["BASIS_SEQ"]);
                }
                else
                {
                    SEQ = 0;
                }
                hn = GetName.GetMerchandiseName(Convert.ToInt32(dr["HN_RID"]), SAB);
                fv = GetName.GetVersion(Convert.ToInt32(dr["FV_RID"]), SAB);
                cdr = GetName.GetCalendarDateRange(Convert.ToInt32(dr["CDR_RID"]), SAB);
                oll = GetName.GetOverrideLowLevelsModel(Convert.ToInt32(dr["OLL_RID"]), SAB);
                customOll = GetName.GetOverrideLowLevelsModel(Convert.ToInt32(dr["CUSTOM_OLL_RID"]), SAB);
                weight = Convert.ToDecimal(dr["WEIGHT"]);
                merchType = EnumTools.VerifyEnumValue((eMerchandiseType)Convert.ToInt32(dr["MERCH_TYPE"]));

                ROMethodSizeCurveMerchBasisProperties rOMethodSizeCurveMerchBasisProperties = new ROMethodSizeCurveMerchBasisProperties(false, false, false, SEQ, hn, fv, cdr, merchType, weight, oll, customOll);
                rOMethodSizeCurveMerchBasiss.Add(rOMethodSizeCurveMerchBasisProperties);
            }

            ROMethodSizeCurveMerchBasisSet SizeCurveMerchBasisSet = new ROMethodSizeCurveMerchBasisSet();
            SizeCurveMerchBasisSet.SizeCurveMerchBasisRowsValues = rOMethodSizeCurveMerchBasiss;
            return SizeCurveMerchBasisSet;
        }
        /// <summary>
        /// Builds the dtMerchBasisDetail Dataset from the RO SizeCurveMerchBasis Set list 
        /// </summary>

        public static DataTable BuilddtMerchBasisDetail(int methodRID, ROMethodSizeCurveMerchBasisSet rOMethodSizeCurveMerchBasisSet, DataTable dtMerchBasisDetailSV,
            SessionAddressBlock SAB)
        {

            DataTable dtMerchBasisDetail = new DataTable();
            dtMerchBasisDetail = dtMerchBasisDetailSV.Clone();
            DataRow dataRow;

            string newColumnName = string.Empty;
            bool success = false;
            int number = 0;

            //Create dataset from saved ROMethodSizeCurveMerchBasisSet
            if (rOMethodSizeCurveMerchBasisSet.SizeCurveMerchBasisRowsValues.Count > 0)
            {
                //create table from list
                try
                {
                    foreach (ROMethodSizeCurveMerchBasisProperties rOMethodSizeCurveMerchBasiss in rOMethodSizeCurveMerchBasisSet.SizeCurveMerchBasisRowsValues)
                    {
                        dataRow = dtMerchBasisDetail.NewRow();

                        newColumnName = "HN_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.HN.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.HN.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "BASIS_SEQ";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.Basis_SEQ.ToString(), out number);
                        if (success)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.Basis_SEQ);
                        }
                        else
                        {
                            dataRow[newColumnName] = 0;
                        }

                        newColumnName = "HN_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.HN.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.HN.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "FV_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.FV.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.FV.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "CDR_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.CDR.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.CDR.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "OLL_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.OLL.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.OLL.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "CUSTOM_OLL_RID";
                        success = Int32.TryParse(rOMethodSizeCurveMerchBasiss.CustomOll.Key.ToString(), out number);
                        if (success && number > 0)
                        {
                            dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.CustomOll.Key);
                        }
                        else
                        {
                            dataRow[newColumnName] = Include.NoRID;
                        }

                        newColumnName = "WEIGHT";
                        dataRow[newColumnName] = Convert.ToDecimal(rOMethodSizeCurveMerchBasiss.Weight);

                        //merchType = (eMerchandiseType)Convert.ToInt32(dr["MERCH_TYPE"]);
                        newColumnName = "MERCH_TYPE";
                        dataRow[newColumnName] = Convert.ToInt32(rOMethodSizeCurveMerchBasiss.MerchType);

                        dtMerchBasisDetail.Rows.Add(dataRow);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return dtMerchBasisDetail;
        }
    }

    public class EnumTools
    {
        public static eHierarchyType VerifyEnumValue(eHierarchyType value)
        {
            if (!Enum.IsDefined(typeof(eHierarchyType), value))
            {
                value = eHierarchyType.None;
            }

            return value;
        }

        public static eHierarchyRollupOption VerifyEnumValue(eHierarchyRollupOption value)
        {
            if (!Enum.IsDefined(typeof(eHierarchyRollupOption), value))
            {
                value = eHierarchyRollupOption.Undefined;
            }

            return value;
        }

        public static eOTSPlanLevelType VerifyEnumValue(eOTSPlanLevelType value)
        {
            if (!Enum.IsDefined(typeof(eOTSPlanLevelType), value))
            {
                value = eOTSPlanLevelType.Undefined;
            }

            return value;
        }

        public static eAllocationSelectionViewType VerifyEnumValue(eAllocationSelectionViewType value)
        {
            if (!Enum.IsDefined(typeof(eAllocationSelectionViewType), value))
            {
                value = eAllocationSelectionViewType.None;
            }

            return value;
        }

        public static eMerchandiseType VerifyEnumValue(eMerchandiseType value)
        {
            if (!Enum.IsDefined(typeof(eMerchandiseType), value))
            {
                value = eMerchandiseType.Undefined;
            }

            return value;
        }

        public static eFillSizesToType VerifyEnumValue(eFillSizesToType value)
        {
            if (!Enum.IsDefined(typeof(eFillSizesToType), value))
            {
                value = eFillSizesToType.Holes;
            }

            return value;
        }

        public static eVSWSizeConstraints VerifyEnumValue(eVSWSizeConstraints value)
        {
            if (!Enum.IsDefined(typeof(eVSWSizeConstraints), value))
            {
                value = eVSWSizeConstraints.None;
            }

            return value;
        }

        public static eEquateOverrideSizeType VerifyEnumValue(eEquateOverrideSizeType value)
        {
            if (!Enum.IsDefined(typeof(eEquateOverrideSizeType), value))
            {
                value = eEquateOverrideSizeType.DimensionSize;
            }

            return value;
        }

        public static eSizeCurvesByType VerifyEnumValue(eSizeCurvesByType value)
        {
            if (!Enum.IsDefined(typeof(eSizeCurvesByType), value))
            {
                value = eSizeCurvesByType.None;
            }

            return value;
        }

        public static eNodeChainSalesType VerifyEnumValue(eNodeChainSalesType value)
        {
            if (!Enum.IsDefined(typeof(eNodeChainSalesType), value))
            {
                value = eNodeChainSalesType.None;
            }

            return value;
        }

        public static eDCFulfillmentSplitOption VerifyEnumValue(eDCFulfillmentSplitOption value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentSplitOption), value))
            {
                value = eDCFulfillmentSplitOption.DCFulfillment;
            }

            return value;
        }

        public static eDCFulfillmentHeadersOrder VerifyEnumValue(eDCFulfillmentHeadersOrder value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentHeadersOrder), value))
            {
                value = eDCFulfillmentHeadersOrder.Ascending;
            }

            return value;
        }

        public static eDCFulfillmentSplitByOption VerifyEnumValue(eDCFulfillmentSplitByOption value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentSplitByOption), value))
            {
                value = eDCFulfillmentSplitByOption.SplitByDC;
            }

            return value;
        }

        public static eDCFulfillmentWithinDC VerifyEnumValue(eDCFulfillmentWithinDC value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentWithinDC), value))
            {
                value = eDCFulfillmentWithinDC.Proportional;
            }

            return value;
        }

        public static eDCFulfillmentReserve VerifyEnumValue(eDCFulfillmentReserve value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentReserve), value))
            {
                value = eDCFulfillmentReserve.ReservePreSplit;
            }

            return value;
        }

        public static eDCFulfillmentStoresOrder VerifyEnumValue(eDCFulfillmentStoresOrder value)
        {
            if (!Enum.IsDefined(typeof(eDCFulfillmentStoresOrder), value))
            {
                value = eDCFulfillmentStoresOrder.Descending;
            }

            return value;
        }

        public static eSizeMethodRowType VerifyEnumValue(eSizeMethodRowType value)
        {
            if (!Enum.IsDefined(typeof(eSizeMethodRowType), value))
            {
                value = eSizeMethodRowType.Default;
            }

            return value;
        }

        public static eSortDirection VerifyEnumValue(eSortDirection value)
        {
            if (!Enum.IsDefined(typeof(eSortDirection), value))
            {
                value = eSortDirection.Descending;
            }

            return value;
        }

        public static eComponentType VerifyEnumValue(eComponentType value)
        {
            if (!Enum.IsDefined(typeof(eComponentType), value))
            {
                value = eComponentType.Total;
            }

            return value;
        }

        public static eRuleMethod VerifyEnumValue(eRuleMethod value)
        {
            if (!Enum.IsDefined(typeof(eRuleMethod), value))
            {
                value = eRuleMethod.None;
            }

            return value;
        }

        public static eMinMaxType VerifyEnumValue(eMinMaxType value)
        {
            if (!Enum.IsDefined(typeof(eMinMaxType), value))
            {
                value = eMinMaxType.Allocation;
            }

            return value;
        }

        public static eVelocityCalculateAverageUsing VerifyEnumValue(eVelocityCalculateAverageUsing value)
        {
            if (!Enum.IsDefined(typeof(eVelocityCalculateAverageUsing), value))
            {
                value = eVelocityCalculateAverageUsing.AllStores;
            }

            return value;
        }

        public static eVelocityDetermineShipQtyUsing VerifyEnumValue(eVelocityDetermineShipQtyUsing value)
        {
            if (!Enum.IsDefined(typeof(eVelocityDetermineShipQtyUsing), value))
            {
                value = eVelocityDetermineShipQtyUsing.Basis;
            }

            return value;
        }

        public static eVelocityApplyMinMaxType VerifyEnumValue(eVelocityApplyMinMaxType value)
        {
            if (!Enum.IsDefined(typeof(eVelocityApplyMinMaxType), value))
            {
                value = eVelocityApplyMinMaxType.None;
            }

            return value;
        }

        public static eVelocityMethodGradeVariableType VerifyEnumValue(eVelocityMethodGradeVariableType value)
        {
            if (!Enum.IsDefined(typeof(eVelocityMethodGradeVariableType), value))
            {
                value = eVelocityMethodGradeVariableType.Stock;
            }

            return value;
        }

        public static eVelocityMatrixMode VerifyEnumValue(eVelocityMatrixMode value)
        {
            if (!Enum.IsDefined(typeof(eVelocityMatrixMode), value))
            {
                value = eVelocityMatrixMode.None;
            }

            return value;
        }

        public static eVelocityRuleType VerifyEnumValue(eVelocityRuleType value)
        {
            if (!Enum.IsDefined(typeof(eVelocityRuleType), value))
            {
                value = eVelocityRuleType.None;
            }

            return value;
        }

        public static eVelocityRuleRequiresQuantity VerifyEnumValue(eVelocityRuleRequiresQuantity value)
        {
            if (!Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), value))
            {
                value = eVelocityRuleRequiresQuantity.AbsoluteQuantity;
            }

            return value;
        }

        public static eVelocitySpreadOption VerifyEnumValue(eVelocitySpreadOption value)
        {
            if (!Enum.IsDefined(typeof(eVelocitySpreadOption), value))
            {
                value = eVelocitySpreadOption.None;
            }

            return value;
        }

        public static eHierarchyLevelType VerifyEnumValue(eHierarchyLevelType value)
        {
            if (!Enum.IsDefined(typeof(eHierarchyLevelType), value))
            {
                value = eHierarchyLevelType.Undefined;
            }

            return value;
        }

        public static eLevelLengthType VerifyEnumValue(eLevelLengthType value)
        {
            if (!Enum.IsDefined(typeof(eLevelLengthType), value))
            {
                value = eLevelLengthType.unrestricted;
            }

            return value;
        }

        public static eHierarchyDisplayOptions VerifyEnumValue(eHierarchyDisplayOptions value)
        {
            if (!Enum.IsDefined(typeof(eHierarchyDisplayOptions), value))
            {
                value = eHierarchyDisplayOptions.IdAndName;
            }

            return value;
        }

        public static eHierarchyIDFormat VerifyEnumValue(eHierarchyIDFormat value)
        {
            if (!Enum.IsDefined(typeof(eHierarchyIDFormat), value))
            {
                value = eHierarchyIDFormat.Unique;
            }

            return value;
        }

        public static eProductType VerifyEnumValue(eProductType value)
        {
            if (!Enum.IsDefined(typeof(eProductType), value))
            {
                value = eProductType.Undefined;
            }

            return value;
        }

        public static ePlanLevelSelectType VerifyEnumValue(ePlanLevelSelectType value)
        {
            if (!Enum.IsDefined(typeof(ePlanLevelSelectType), value))
            {
                value = ePlanLevelSelectType.Undefined;
            }

            return value;
        }

        public static eMaskField VerifyEnumValue(eMaskField value)
        {
            if (!Enum.IsDefined(typeof(eMaskField), value))
            {
                value = eMaskField.Undefined;
            }

            return value;
        }

        public static eROLevelsType ConverteHierarchyDescendantTypeToeROLevelsType(eHierarchyDescendantType inParam)
        {
            switch (inParam)
            {
                case eHierarchyDescendantType.offset:
                    return eROLevelsType.LevelOffset;
                case eHierarchyDescendantType.levelType:
                    return eROLevelsType.HierarchyLevel;
                default:
                    return eROLevelsType.None;
            }
        }

        public static eHierarchyDescendantType ConverteROLevelsTypeToeHierarchyDescendantType(eROLevelsType inParam)
        {
            switch (inParam)
            {
                case eROLevelsType.LevelOffset:
                    return eHierarchyDescendantType.offset;
                case eROLevelsType.HierarchyLevel:
                    return eHierarchyDescendantType.levelType;
                default:
                    return eHierarchyDescendantType.masterType;
            }
        }
    }

    public class HierarchyTools
    {
        public static List<HierarchyLevelComboObject> GetLevelsList(
            SessionAddressBlock sessionAddressBlock,
            int nodeKey,
            bool includeHomeLevel,
            bool includeLowestLevel,
            bool includeOrganizationLevelsForAlternate,
            out eMerchandiseType merchandiseType,
            out int homeHierarchyKey
            )
        {
            HierarchyNodeProfile nodeProfile;
            HierarchyProfile hierarchyProfile;
            int startLevel;
            int i;
            HierarchyProfile mainHierarchyProfile;
            int highestGuestLevel;
            int longestBranchCount;
            int offset;
            ArrayList guestLevels;
            HierarchyLevelProfile hierarchyLevelProfile;
            List<HierarchyLevelComboObject> levelList;
            merchandiseType = eMerchandiseType.Undefined;
            homeHierarchyKey = Include.NoRID;
            int limitOffset = 0;

            try
            {
                levelList = new List<HierarchyLevelComboObject>();
                if (nodeKey != Include.NoRID)
                {
                    nodeProfile = sessionAddressBlock.HierarchyServerSession.GetNodeData(nodeKey);
                    hierarchyProfile = sessionAddressBlock.HierarchyServerSession.GetHierarchyData(nodeProfile.HomeHierarchyRID);
                    homeHierarchyKey = nodeProfile.HomeHierarchyRID;

                    if (!includeLowestLevel)
                    {
                        limitOffset = -1;
                    }

                    // Load Level arrays

                    if (hierarchyProfile.HierarchyType == eHierarchyType.organizational)
                    {
                        merchandiseType = eMerchandiseType.HierarchyLevel;
                        if (nodeProfile.HomeHierarchyLevel == 0)
                        {
                            if (includeHomeLevel)
                            {
                                levelList.Add(new HierarchyLevelComboObject(
                                    levelList.Count,
                                    ePlanLevelLevelType.HierarchyLevel,
                                    hierarchyProfile.Key,
                                    0,
                                    hierarchyProfile.HierarchyID
                                    ));
                            }
                            startLevel = 1;
                        }
                        else
                        {
                            if (includeHomeLevel)
                            {
                                levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                hierarchyProfile.Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[nodeProfile.HomeHierarchyLevel]).Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[nodeProfile.HomeHierarchyLevel]).LevelID
                                ));
                            }
                            startLevel = nodeProfile.HomeHierarchyLevel + 1;
                        }

                        for (i = startLevel; i <= hierarchyProfile.HierarchyLevels.Count + limitOffset; i++)
                        {
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                hierarchyProfile.Key, ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[i]).Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[i]).LevelID
                                ));
                        }
                    }
                    else
                    {
                        merchandiseType = eMerchandiseType.LevelOffset;
                        mainHierarchyProfile = sessionAddressBlock.HierarchyServerSession.GetMainHierarchyData();
                        highestGuestLevel = sessionAddressBlock.HierarchyServerSession.GetHighestGuestLevel(nodeProfile.Key);
                        guestLevels = sessionAddressBlock.HierarchyServerSession.GetAllGuestLevels(nodeProfile.Key);

                        if (includeHomeLevel)
                        {
                            levelList.Add(new HierarchyLevelComboObject(
                            levelList.Count,
                            ePlanLevelLevelType.HierarchyLevel,
                            hierarchyProfile.Key,
                            0,
                            //nodeProfile.NodeID
                            "+0"
                            ));
                        }
                        startLevel = 1;

                        if (includeOrganizationLevelsForAlternate)
                        {
                            if (guestLevels.Count == 1)
                            {
                                hierarchyLevelProfile = (HierarchyLevelProfile)guestLevels[0];
                                levelList.Add(new HierarchyLevelComboObject(
                                    levelList.Count,
                                    ePlanLevelLevelType.HierarchyLevel,
                                    mainHierarchyProfile.Key,
                                    hierarchyLevelProfile.Key,
                                    hierarchyLevelProfile.LevelID
                                    ));
                            }
                        }

                        longestBranchCount = sessionAddressBlock.HierarchyServerSession.GetLongestBranch(nodeProfile.Key, true);
                        DataTable hierarchyLevels = sessionAddressBlock.HierarchyServerSession.GetHierarchyDescendantLevels(nodeProfile.Key);
                        longestBranchCount = hierarchyLevels.Rows.Count - 1;


                        offset = 0;

                        for (i = 0; i < longestBranchCount + limitOffset; i++)
                        {
                            offset++;
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.LevelOffset,
                                hierarchyProfile.Key,
                                offset,
                                "+" + offset.ToString()
                                ));
                        }
                    }
                }

                return levelList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

    }
}