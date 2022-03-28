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
		
        public static KeyValuePair<int, string> GetText(int key)
        {
            string name = string.Empty;

            name = MIDText.GetTextOnly((eMIDTextCode)key);
            return new KeyValuePair<int, string>(key, name);
        }

        public static KeyValuePair<int, string> GetFilterName(int key)
        {
            string name = string.Empty;
            if (key > Include.UndefinedStoreFilter)
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
                    //name = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                    name = null;
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
        /// <param name="isColorSelected"></param> 
        /// <param name="SAB"></param>

        public static ROSizeCurveProperties BuildSizeCurveProperties(
            int sizeCurveGroupRID, 
            int genCurveNsccdRID, 
            int genCurveHcgRID, 
            int genCurveHnRID, 
            int genCurvePhRID, 
            int genCurvePhlSequence, 
            eMerchandiseType genCurveMerchType,
            bool isUseDefault, 
            bool isApplyRulesOnly, 
            bool isColorSelected,
            SessionAddressBlock SAB
            )
        {
            int headerCharacteristicsOrNameExtensionKey = Include.Undefined;
            if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.HeaderCharacteristic)
            {
                headerCharacteristicsOrNameExtensionKey = genCurveHcgRID;
            }
            else if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
            {
                headerCharacteristicsOrNameExtensionKey = genCurveNsccdRID;
            }

            ROSizeCurveProperties sizeCurveProperties = new ROSizeCurveProperties(
                sizeCurveGroupKey: sizeCurveGroupRID,
                genericSizeCurveNameType: SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType,
                genericMerchandiseType: EnumTools.VerifyEnumValue(genCurveMerchType),
                genericHierarchyLevelKey: genCurvePhlSequence,
                isUseDefault: isUseDefault,
                isApplyRulesOnly: isApplyRulesOnly,
                isColorSelected: isColorSelected,
                genericHeaderCharacteristicsOrNameExtensionKey: headerCharacteristicsOrNameExtensionKey
                );

            ListGenerator.FillSizeCurveGroupList(
                sizeCurveGroups: sizeCurveProperties.SizeCurveGroups
                );
            ListGenerator.FillOrganizationalHierarchyLevelList(
                hierarchyLevels: sizeCurveProperties.GenericHierarchyLevels,
                sessionAddressBlock: SAB
                );
            if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.HeaderCharacteristic)
            {
                ListGenerator.FillHeaderCharacteristicList(
                    headerCharacteristics: sizeCurveProperties.GenericHeaderCharacteristicsOrNameExtensions,
                    sessionAddressBlock: SAB
                    );
            }
            else if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
            {
                ListGenerator.FillNameExtensionList(
                    nameExtensions: sizeCurveProperties.GenericHeaderCharacteristicsOrNameExtensions,
                    sessionAddressBlock: SAB
                    );
            }

            return sizeCurveProperties;
        }
    }

    public class SizeConstraintProperties
    {
        /// <summary>
        /// Builds the variables for the Size Constraint Group box
        /// </summary>
        
        public static ROSizeConstraintProperties BuildSizeConstraintProperties(
            int inventoryBasisMerchHnRID, 
            int inventoryBasisMerchPhRID, 
            int inventoryBasisMerchPhlSequence, 
            eMerchandiseType inventoryBasisMerchType,
            int sizeConstraintRID, 
            int genConstraintHcgRID, 
            int genConstraintHnRID, 
            int genConstraintPhRID, 
            int genConstraintPhlSequence, 
            eMerchandiseType genConstraintMerchType,
            bool genConstraintColorInd, 
            KeyValuePair<int, string> inventoryBasis, 
            KeyValuePair<int, string> sizeConstraint, 
            KeyValuePair<int, string> sizeConstraintGenericHierarchy, 
            KeyValuePair<int, string> sizeConstraintGenericHeaderChar, 
            SessionAddressBlock SAB
            )
        {
            if (inventoryBasisMerchType == eMerchandiseType.Node)
            {
                inventoryBasis = GetName.GetLevelKeyValuePair(
                    merchandiseType: inventoryBasisMerchType,
                    nodeRID: inventoryBasisMerchHnRID,
                    merchPhRID: inventoryBasisMerchPhRID,
                    merchPhlSequence: inventoryBasisMerchPhlSequence,
                    SAB: SAB
                    );
            }
            else
            {
                inventoryBasis = new KeyValuePair<int, string>(-1, "");
            }
            sizeConstraint  = GetName.GetSizeConstraint(sizeConstraintRID);
            sizeConstraintGenericHierarchy = GetName.GetLevelKeyValuePair(genConstraintMerchType, genConstraintHnRID, genConstraintPhRID, genConstraintPhlSequence, SAB);
            sizeConstraintGenericHeaderChar = GetName.GetHeaderCharGroupProfile(genConstraintHcgRID);
            
            ROSizeConstraintProperties sizeConstraintProperties = new ROSizeConstraintProperties(
                sizeConstraintKey: sizeConstraintRID,
                inventoryBasisMerchandiseType: EnumTools.VerifyEnumValue(inventoryBasisMerchType),
                inventoryBasisMerchandise: inventoryBasis,
                inventoryBasisHierarchyLevelKey: inventoryBasisMerchPhlSequence,
                genericMerchandiseType: EnumTools.VerifyEnumValue(genConstraintMerchType),
                genericHierarchyLevelKey: genConstraintPhlSequence,
                genericHeaderCharacteristicsKey: genConstraintHcgRID,
                isColorSelected: genConstraintColorInd
                );

            ListGenerator.FillSizeConstraintList(
                sizeConstraints: sizeConstraintProperties.SizeConstraints
                );
            ListGenerator.FillOrganizationalHierarchyLevelList(
                hierarchyLevels: sizeConstraintProperties.InventoryBasisHierarchyLevels,
                sessionAddressBlock: SAB
                );
            ListGenerator.FillOrganizationalHierarchyLevelList(
                hierarchyLevels: sizeConstraintProperties.GenericHierarchyLevels,
                sessionAddressBlock: SAB
                );
            ListGenerator.FillHeaderCharacteristicList(
                headerCharacteristics: sizeConstraintProperties.GenericHeaderCharacteristics,
                sessionAddressBlock: SAB
                );
 
            return sizeConstraintProperties;
        }
    }

    public class SizeRuleProperties
    {
        /// <summary>
        /// Builds the RO Size Rule Attribute Set lists for the Size Methods' Rule Tab from the Dataset MethodConstraints
        /// </summary>

        public static List<ROMethodSizeRuleProperties> BuildSizeRuleProperties(
            int methodRID, 
            eMethodType methodType, 
            int attributeRID, 
            int sizeGroupRID, 
            int sizeCurveGroupRID, 
            eGetSizes getSizesUsing, 
            eGetDimensions getDimensionsUsing, 
            DataSet methodConstraints,
            SessionAddressBlock SAB,
            bool addColorDimensionSizes = true
            )
        {

            List<ROMethodSizeRuleProperties> attributeSetRuleProperties = new List<ROMethodSizeRuleProperties>();

            ROMethodSizeRuleProperties attributeSetSizeRule;

            // build attribute sets
            if (methodConstraints.Tables.Contains("SetLevel"))
            {
                foreach (DataRow row in methodConstraints.Tables["SetLevel"].Rows)
                {
                    attributeSetSizeRule = new ROMethodSizeRuleProperties(
                        sizeRuleItem: new KeyValuePair<int, string>(
                            GetKeyValue(row: row, columnName: "SGL_RID"),  // attribute set key
                            GetStringValue(row: row, columnName: "BAND_DSC")  // attribute name
                            ),
                        sizeRule: GetIntegerValue(row: row, columnName: "SIZE_RULE"),
                        sizeQuantity: GetIntegerValue(row: row, columnName: "SIZE_QUANTITY"),
                        children: new List<ROMethodSizeRuleProperties>()
                        );

                    if (addColorDimensionSizes)
                    {
                        bool foundColorTable = false;

                        // Get All Colors for attribute set
                        if (methodConstraints.Tables.Contains("AllColor"))
                        {
                            foundColorTable = true;
                            BuildSizeRuleColorProperties(
                                tableName: "AllColor",
                                attributeSetKey: attributeSetSizeRule.SizeRuleItem.Key,
                                colorRuleProperties: attributeSetSizeRule.Children,
                                methodConstraints: methodConstraints
                                );
                        }

                        // Get Colors for attribute set
                        if (methodConstraints.Tables.Contains("Color"))
                        {
                            foundColorTable = true;
                            BuildSizeRuleColorProperties(
                                tableName: "Color",
                                attributeSetKey: attributeSetSizeRule.SizeRuleItem.Key,
                                colorRuleProperties: attributeSetSizeRule.Children,
                                methodConstraints: methodConstraints
                                );
                        }

                        // set children to null if no color tables
                        if (!foundColorTable)
                        {
                            attributeSetSizeRule.Children = null;
                        }
                    }
                    // set children to null if do not include other levels
                    else
                    {
                        attributeSetSizeRule.Children = null;
                    }

                    // Add attribute set to rule list
                    attributeSetRuleProperties.Add(attributeSetSizeRule);
                }
            }
            // set to null if does not contain attribute set table
            else
            {
                attributeSetRuleProperties = null;
            }

            return attributeSetRuleProperties;
            
        }

        private static void BuildSizeRuleColorProperties(
            string tableName,
            int attributeSetKey,
            List<ROMethodSizeRuleProperties> colorRuleProperties,
            DataSet methodConstraints
            )
        {
            ROMethodSizeRuleProperties colorSizeRule;
            int ruleAttributeSetKey;
            string dimensionTableName;
            if (tableName == "AllColor")
            {
                dimensionTableName = "AllColorSizeDimension";
            }
            else
            {
                dimensionTableName = "ColorSizeDimension";
            }

            foreach (DataRow row in methodConstraints.Tables[tableName].Rows)
            {
                ruleAttributeSetKey = GetKeyValue(row: row, columnName: "SGL_RID");
                if (ruleAttributeSetKey == attributeSetKey)
                {
                    colorSizeRule = new ROMethodSizeRuleProperties(
                        sizeRuleItem: new KeyValuePair<int, string>(
                            GetKeyValue(row: row, columnName: "COLOR_CODE_RID"),  // Color key
                            " "  // name determined from list at presentation
                            ),
                        sizeRule: GetIntegerValue(row: row, columnName: "SIZE_RULE"),
                        sizeQuantity: GetIntegerValue(row: row, columnName: "SIZE_QUANTITY"),
                        children: new List<ROMethodSizeRuleProperties>()
                        );

                    // set children to null if does not contain dimension table
                    if (methodConstraints.Tables.Contains(dimensionTableName))
                    {
                        BuildSizeRuleDimensionProperties(
                            tableName: dimensionTableName,
                            attributeSetKey: attributeSetKey,
                            colorKey: colorSizeRule.SizeRuleItem.Key,
                            dimensionRuleProperties: colorSizeRule.Children,
                            methodConstraints: methodConstraints
                            );
                    }
                    else
                    {
                        colorSizeRule.Children = null;
                    }

                    colorRuleProperties.Add(colorSizeRule);
                }
            }
        }

        private static void BuildSizeRuleDimensionProperties(
            string tableName,
            int attributeSetKey,
            int colorKey,
            List<ROMethodSizeRuleProperties> dimensionRuleProperties,
            DataSet methodConstraints
            )
        {
            ROMethodSizeRuleProperties dimensionSizeRule;
            int ruleAttributeSetKey, ruleColorKey;
            string sizeTableName;
            if (tableName == "AllColorSizeDimension")
            {
                sizeTableName = "AllColorSize";
            }
            else
            {
                sizeTableName = "ColorSize";
            }

            foreach (DataRow row in methodConstraints.Tables[tableName].Rows)
            {
                ruleAttributeSetKey = GetKeyValue(row: row, columnName: "SGL_RID");
                ruleColorKey = GetKeyValue(row: row, columnName: "COLOR_CODE_RID");
                if (ruleAttributeSetKey == attributeSetKey
                    && ruleColorKey == colorKey)
                {
                    dimensionSizeRule = new ROMethodSizeRuleProperties(
                        sizeRuleItem: new KeyValuePair<int, string>(
                            GetKeyValue(row: row, columnName: "DIMENSIONS_RID"),  //dimension key
                            " "  // name determined from list at presentation
                            ),
                        sizeRule: GetIntegerValue(row: row, columnName: "SIZE_RULE"),
                        sizeQuantity: GetIntegerValue(row: row, columnName: "SIZE_QUANTITY"),
                        children: new List<ROMethodSizeRuleProperties>()
                        );

                    // set children to null if does not contain color table
                    if (methodConstraints.Tables.Contains(sizeTableName))
                    {
                        BuildSizeRuleSizeProperties(
                            tableName: sizeTableName,
                            attributeSetKey: attributeSetKey,
                            colorKey: colorKey,
                            dimensionKey: dimensionSizeRule.SizeRuleItem.Key,
                            sizeRuleProperties: dimensionSizeRule.Children,
                            methodConstraints: methodConstraints
                            );
                    }
                    else
                    {
                        dimensionSizeRule.Children = null;
                    }

                    dimensionRuleProperties.Add(dimensionSizeRule);
                }
            }
        }

        private static void BuildSizeRuleSizeProperties(
            string tableName,
            int attributeSetKey,
            int colorKey,
            int dimensionKey,
            List<ROMethodSizeRuleProperties> sizeRuleProperties,
            DataSet methodConstraints
            )
        {
            ROMethodSizeRuleProperties sizeSizeRule;
            int ruleAttributeSetKey, ruleColorKey, ruleDimensionKey;

            foreach (DataRow row in methodConstraints.Tables[tableName].Rows)
            {
                ruleAttributeSetKey = GetKeyValue(row: row, columnName: "SGL_RID");
                ruleColorKey = GetKeyValue(row: row, columnName: "COLOR_CODE_RID");
                ruleDimensionKey = GetKeyValue(row: row, columnName: "DIMENSIONS_RID");
                if (ruleAttributeSetKey == attributeSetKey
                    && ruleColorKey == colorKey
                    && ruleDimensionKey == dimensionKey
                    )
                {
                    sizeSizeRule = new ROMethodSizeRuleProperties(
                        sizeRuleItem: new KeyValuePair<int, string>(
                            GetKeyValue(row: row, columnName: "SIZE_CODE_RID"),  //size key
                            " "  // name determined from list at presentation
                            ),
                        sizeRule: GetIntegerValue(row: row, columnName: "SIZE_RULE"),
                        sizeQuantity: GetIntegerValue(row: row, columnName: "SIZE_QUANTITY"),
                        children: null  // sizes do not children
                        );

                    sizeRuleProperties.Add(sizeSizeRule);
                }
            }
        }

        private static string GetStringValue(DataRow row, string columnName)
        {
            string value = null;

            if (row[columnName] == DBNull.Value)
            {
                value = " ";
            }
            else
            {
                value = row[columnName].ToString();
            }

            return value;
        }

        private static int GetKeyValue(DataRow row, string columnName)
        {
            int value = Include.NoRID;

            if (row[columnName] == DBNull.Value)
            {
                value = Include.NoRID;
            }
            else
            {
                value = Convert.ToInt32(row[columnName]);
            }

            return value;
        }

        private static int? GetIntegerValue(DataRow row, string columnName)
        {
            int? value = Include.NoRID;
            bool success = false;
            int number = 0;

            if (row[columnName] == DBNull.Value)
            {
                value = null;
            }
            else
            {
                success = Int32.TryParse(row[columnName].ToString(), out number);
                if (success)
                {
                    value = Convert.ToInt32(row[columnName]);
                }
                else
                {
                    value = 0;
                }
            }

            return value;
        }

        private static double GetDoubleValue(DataRow row, string columnName)
        {
            double value = 0;

            if (row[columnName] == DBNull.Value)
            {
                value = 0;
            }
            else
            {
                value = Convert.ToDouble(row[columnName]);
            }

            return value;
        }

        /// <summary>
        /// Builds the MethodConstraints Dataset from the  RO Size Rule Attribute Set lists for the Size Methods' Rule Tab 
        /// </summary>

        public static DataSet BuildMethodConstrainst(
            int methodRID, 
            int attributeRID,
            List<ROMethodSizeRuleProperties> rOMethodSizeRuleAttributeSet, 
            DataSet methodConstraintsSV,
            SessionAddressBlock SAB
            )
        {

            DataSet methodConstraints = new DataSet();
            methodConstraints = methodConstraintsSV.Clone();

            if (rOMethodSizeRuleAttributeSet != null)
            {
                foreach (ROMethodSizeRuleProperties attributeSetRuleProperties in rOMethodSizeRuleAttributeSet)
                {
                    AddAttributeSetRow(
                        methodKey: methodRID,
                        attributeSetKey: attributeSetRuleProperties.SizeRuleItem.Key,
                        attributeSetID: attributeSetRuleProperties.SizeRuleItem.Value,
                        sizeRule: attributeSetRuleProperties.SizeRule,
                        sizeQuantity: attributeSetRuleProperties.SizeQuantity,
                        methodConstraints: methodConstraints
                        );
                    if (attributeSetRuleProperties.Children != null)
                    {
                        foreach (ROMethodSizeRuleProperties colorRuleProperties in attributeSetRuleProperties.Children)
                        {
                            AddColorRow(
                                methodKey: methodRID,
                                attributeSetKey: attributeSetRuleProperties.SizeRuleItem.Key,
                                colorKey: colorRuleProperties.SizeRuleItem.Key,
                                sizeRule: colorRuleProperties.SizeRule,
                                sizeQuantity: colorRuleProperties.SizeQuantity,
                                methodConstraints: methodConstraints
                                );
                            if (colorRuleProperties.Children != null)
                            {
                                foreach (ROMethodSizeRuleProperties dimensionRuleProperties in colorRuleProperties.Children)
                                {
                                    
                                    AddDimensionRow(
                                       methodKey: methodRID,
                                       attributeSetKey: attributeSetRuleProperties.SizeRuleItem.Key,
                                       colorKey: colorRuleProperties.SizeRuleItem.Key,
                                       dimensionKey: dimensionRuleProperties.SizeRuleItem.Key,
                                       sizeRule: dimensionRuleProperties.SizeRule,
                                       sizeQuantity: dimensionRuleProperties.SizeQuantity,
                                       methodConstraints: methodConstraints
                                       );
                                    if (dimensionRuleProperties.Children != null)
                                    {
                                        foreach (ROMethodSizeRuleProperties sizeRuleProperties in dimensionRuleProperties.Children)
                                        {
                                            AddSizeRow(
                                               methodKey: methodRID,
                                               attributeSetKey: attributeSetRuleProperties.SizeRuleItem.Key,
                                               colorKey: colorRuleProperties.SizeRuleItem.Key,
                                               dimensionKey: dimensionRuleProperties.SizeRuleItem.Key,
                                               sizeKey: sizeRuleProperties.SizeRuleItem.Key,
                                               sizeRule: sizeRuleProperties.SizeRule,
                                               sizeQuantity: sizeRuleProperties.SizeQuantity,
                                               methodConstraints: methodConstraints
                                               );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            return methodConstraints;
        }

        private static void AddAttributeSetRow(
            int methodKey,
            int attributeSetKey,
            string attributeSetID,
            int? sizeRule,
            int? sizeQuantity,
            DataSet methodConstraints
            )
        {
            int number = 0;
            DataRow dataRow;
            DataTable dataTable = methodConstraints.Tables["SetLevel"];

            dataRow = dataTable.NewRow();
            dataRow["BAND_DSC"] = attributeSetID;
            dataRow["METHOD_RID"] = methodKey;
            dataRow["SGL_RID"] = attributeSetKey;
            if (sizeRule != null
                && Int32.TryParse(sizeRule.ToString(), out number))
            {
                dataRow["SIZE_RULE"] = sizeRule;
            }
            else
            {
                dataRow["SIZE_RULE"] = string.Empty;
            }
            if (sizeQuantity != null
                && Int32.TryParse(sizeQuantity.ToString(), out number))
            {
                dataRow["SIZE_QUANTITY"] = sizeQuantity;
            }
            //else
            //{
            //    dataRow["SIZE_QUANTITY"] = Include.UndefinedQuantity;
            //}
            if (attributeSetKey == Include.Undefined) // Default
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.Default.GetHashCode();
            }
            else
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.Set.GetHashCode();
            }

            dataTable.Rows.Add(dataRow);
        }

        private static void AddColorRow(
            int methodKey,
            int attributeSetKey,
            int colorKey,
            int? sizeRule,
            int? sizeQuantity,
            DataSet methodConstraints
            )
        {
            int number = 0;
            DataRow dataRow;
            DataTable dataTable;
            if (colorKey == Include.Undefined) // All Color
            {
                dataTable = methodConstraints.Tables["AllColor"];
            }
            else
            {
                dataTable = methodConstraints.Tables["Color"];
            }

            dataRow = dataTable.NewRow();
            dataRow["BAND_DSC"] = " ";
            dataRow["METHOD_RID"] = methodKey;
            dataRow["SGL_RID"] = attributeSetKey;
            dataRow["COLOR_CODE_RID"] = colorKey;
            dataRow["SIZES_RID"] = Include.Undefined;
            dataRow["DIMENSIONS_RID"] = Include.Undefined;
            dataRow["SIZE_CODE_RID"] = Include.Undefined;

            if (sizeRule != null
                && Int32.TryParse(sizeRule.ToString(), out number))
            {
                dataRow["SIZE_RULE"] = sizeRule;
            }
            else
            {
                dataRow["SIZE_RULE"] = string.Empty;
            }
            if (sizeQuantity != null
                && Int32.TryParse(sizeQuantity.ToString(), out number))
            {
                dataRow["SIZE_QUANTITY"] = sizeQuantity;
            }
            //else
            //{
            //    dataRow["SIZE_QUANTITY"] = Include.UndefinedQuantity;
            //}
            if (colorKey == Include.Undefined) // All Color
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.AllColor.GetHashCode();
            }
            else
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.Color.GetHashCode();
            }
            dataRow["SIZE_SEQ"] = 0;

            dataTable.Rows.Add(dataRow);
        }

        private static void AddDimensionRow(
            int methodKey,
            int attributeSetKey,
            int colorKey,
            int dimensionKey,
            int? sizeRule,
            int? sizeQuantity,
            DataSet methodConstraints
            )
        {
            int number = 0;
            DataRow dataRow;
            DataTable dataTable;
            if (colorKey == Include.Undefined) // All Color
            {
                dataTable = methodConstraints.Tables["AllColorSizeDimension"];
            }
            else
            {
                dataTable = methodConstraints.Tables["ColorSizeDimension"];
            }

            dataRow = dataTable.NewRow();
            dataRow["BAND_DSC"] = " ";
            dataRow["METHOD_RID"] = methodKey;
            dataRow["SGL_RID"] = attributeSetKey;
            dataRow["COLOR_CODE_RID"] = colorKey;
            dataRow["SIZES_RID"] = Include.Undefined;
            dataRow["DIMENSIONS_RID"] = dimensionKey;
            dataRow["SIZE_CODE_RID"] = Include.Undefined;

            if (sizeRule != null
                && Int32.TryParse(sizeRule.ToString(), out number))
            {
                dataRow["SIZE_RULE"] = sizeRule;
            }
            else
            {
                dataRow["SIZE_RULE"] = string.Empty;
            }
            if (sizeQuantity != null
                && Int32.TryParse(sizeQuantity.ToString(), out number))
            {
                dataRow["SIZE_QUANTITY"] = sizeQuantity;
            }
            //else
            //{
            //    dataRow["SIZE_QUANTITY"] = Include.UndefinedQuantity;
            //}
            if (colorKey == Include.Undefined) // All Color
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.AllColorSizeDimension.GetHashCode();
            }
            else
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.ColorSizeDimension.GetHashCode();
            }
            dataRow["SIZE_SEQ"] = 0;

            dataTable.Rows.Add(dataRow);
        }

        private static void AddSizeRow(
            int methodKey,
            int attributeSetKey,
            int colorKey,
            int dimensionKey,
            int sizeKey,
            int? sizeRule,
            int? sizeQuantity,
            DataSet methodConstraints
            )
        {
            int number = 0;
            DataRow dataRow;
            DataTable dataTable;
            if (colorKey == Include.Undefined) // All Color
            {
                dataTable = methodConstraints.Tables["AllColorSize"];
            }
            else
            {
                dataTable = methodConstraints.Tables["ColorSize"];
            }

            dataRow = dataTable.NewRow();
            dataRow["BAND_DSC"] = " ";
            dataRow["METHOD_RID"] = methodKey;
            dataRow["SGL_RID"] = attributeSetKey;
            dataRow["COLOR_CODE_RID"] = colorKey;
            dataRow["SIZES_RID"] = Include.Undefined;
            dataRow["DIMENSIONS_RID"] = dimensionKey;
            dataRow["SIZE_CODE_RID"] = sizeKey;

            if (sizeRule != null
                && Int32.TryParse(sizeRule.ToString(), out number))
            {
                dataRow["SIZE_RULE"] = sizeRule;
            }
            else
            {
                dataRow["SIZE_RULE"] = string.Empty;
            }
            if (sizeQuantity != null
                && Int32.TryParse(sizeQuantity.ToString(), out number))
            {
                dataRow["SIZE_QUANTITY"] = sizeQuantity;
            }
            //else
            //{
            //    dataRow["SIZE_QUANTITY"] = Include.UndefinedQuantity;
            //}
            if (colorKey == Include.Undefined) // All Color
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.AllColorSize.GetHashCode();
            }
            else
            {
                dataRow["ROW_TYPE_ID"] = eSizeMethodRowType.ColorSize.GetHashCode();
            }
            dataRow["SIZE_SEQ"] = 0;

            dataTable.Rows.Add(dataRow);
        }

    }
    public class BasisSizeSubstituteSet
    {
        /// <summary>
        /// Builds the RO Basis Size Substitute List for the Basis Size Substitute Tab from the _substituteList
        /// </summary>

        public static ROMethodBasisSizeSubstituteSet BuildBasisSizeSubstituteSet(
            int methodRID, 
            eMethodType methodType, 
            int attributeRID, 
            int sizeGroupRID, 
            int sizeCurveGroupRID, 
            eGetSizes getSizesUsing, 
            eGetDimensions getDimensionsUsing, 
            ArrayList substituteList, 
            SessionAddressBlock SAB
            )
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

        public static void BuildLowLevelLists(
            SessionAddressBlock sessionAddressBlock,
            int hierarchyNodeRID,
            List<KeyValuePair<int, string>> fromLevels,
            ref eMerchandiseType fromMerchandiseType,
            List<KeyValuePair<int, string>> toLevels,
            ref eMerchandiseType toMerchandiseType
            )
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = GetLevelsList(
                sessionAddressBlock: sessionAddressBlock,
                nodeKey: hierarchyNodeRID,
                includeHomeLevel: true,
                includeLowestLevel: false,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            fromMerchandiseType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    fromLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    fromLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }

            levelList = GetLevelsList(
                sessionAddressBlock: sessionAddressBlock,
                nodeKey: hierarchyNodeRID,
                includeHomeLevel: false,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            toMerchandiseType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    toLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    toLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }
        }

        public static void AdjustLevelLists(
            SessionAddressBlock sessionAddressBlock,
            ref ROLevelInformation fromLevel,
            List<KeyValuePair<int, string>> fromLevels,
            ref eMerchandiseType fromMerchandiseType,

            ref ROLevelInformation toLevel,
            List<KeyValuePair<int, string>> toLevels,
            ref eMerchandiseType toMerchandiseType
            )
        {
            eFromLevelsType fromLevelType = (eFromLevelsType)fromLevel.LevelType;
            int fromLevelOffset = fromLevel.LevelOffset;
            int fromLevelSequence = fromLevel.LevelSequence;
            eToLevelsType toLevelType = (eToLevelsType)toLevel.LevelType;
            int toLevelOffset = toLevel.LevelOffset;
            int toLevelSequence = toLevel.LevelSequence;
            bool setFromToFirstEntry = false;
            bool setToToFirstEntry = false;
            int toOffset = -1;
            if (fromLevel != null
                && fromLevel.LevelType != eROLevelsType.None)
            {
                // if different hierarchy types, update from level to 1st from entry
                if (fromLevel != null
                    && !LevelTypesSame(
                        merchandiseType: fromMerchandiseType,
                        ROLevelType: fromLevel.LevelType)
                    )
                {
                    setFromToFirstEntry = true;
                }
                else
                {
                    // set from to first entry if no longer in the list
                    // keep track of level offset so know how many to levels to remove
                    setFromToFirstEntry = true;
                    foreach (KeyValuePair<int, string> level in fromLevels)
                    {
                        ++toOffset;
                        if (fromLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (fromLevel.LevelSequence == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                        else if (fromLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (fromLevel.LevelOffset == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                    }
                }

                // if different hierarchy types, update from level to 1st from entry
                if (toLevel != null
                    && !LevelTypesSame(
                        merchandiseType: toMerchandiseType,
                        ROLevelType: toLevel.LevelType)
                    )
                {
                    setToToFirstEntry = true;
                }
                else
                {
                    // remove entries in to level list that are before the selected from level
                    if (!setFromToFirstEntry)
                    {
                        for (int i = 0; i < toOffset; i++)
                        {
                            toLevels.RemoveAt(0);
                        }
                    }

                    // set To to first entry if no longer in the list
                    setToToFirstEntry = true;
                    foreach (KeyValuePair<int, string> level in toLevels)
                    {
                        if (toLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (toLevel.LevelSequence == level.Key)
                            {
                                setToToFirstEntry = false;
                                break;
                            }
                        }
                        else if (toLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (toLevel.LevelOffset == level.Key)
                            {
                                setToToFirstEntry = false;
                                break;
                            }
                        }
                    }
                }

                // set selected values to first entry if no longer in the list
                if (setFromToFirstEntry)
                {
                    if (fromLevels.Count == 0)
                    {
                        fromLevelType = eFromLevelsType.None;
                        fromLevelOffset = -1;
                        fromLevelSequence = -1;
                    }
                    else if (fromMerchandiseType == eMerchandiseType.HierarchyLevel)
                    {
                        fromLevelType = eFromLevelsType.HierarchyLevel;
                        fromLevelOffset = -1;
                        fromLevelSequence = fromLevels[0].Key;
                    }
                    else
                    {
                        fromLevelType = eFromLevelsType.LevelOffset;
                        fromLevelOffset = fromLevels[0].Key;
                        fromLevelSequence = -1;
                    }
                    fromLevel = new ROLevelInformation();
                    fromLevel.LevelType = (eROLevelsType)fromLevelType;
                    fromLevel.LevelOffset = fromLevelOffset;
                    fromLevel.LevelSequence = fromLevelSequence;
                    fromLevel.LevelValue = GetName.GetLevelName(
                        levelType: (eROLevelsType)fromLevelType,
                        levelSequence: fromLevelSequence,
                        levelOffset: fromLevelOffset,
                        SAB: sessionAddressBlock
                        );
                }

                if (setToToFirstEntry)
                {
                    if (toLevels.Count == 0)
                    {
                        toLevelType = eToLevelsType.None;
                        toLevelOffset = -1;
                        toLevelSequence = -1;
                    }
                    else if (toMerchandiseType == eMerchandiseType.HierarchyLevel)
                    {
                        toLevelType = eToLevelsType.HierarchyLevel;
                        toLevelOffset = -1;
                        toLevelSequence = toLevels[0].Key;
                    }
                    else
                    {
                        toLevelType = eToLevelsType.LevelOffset;
                        toLevelOffset = toLevels[0].Key;
                        toLevelSequence = -1;
                    }
                    toLevel = new ROLevelInformation();
                    toLevel.LevelType = (eROLevelsType)toLevelType;
                    toLevel.LevelOffset = toLevelOffset;
                    toLevel.LevelSequence = toLevelSequence;
                    toLevel.LevelValue = GetName.GetLevelName(
                       levelType: (eROLevelsType)toLevelType,
                       levelSequence: toLevelSequence,
                       levelOffset: toLevelOffset,
                       SAB: sessionAddressBlock
                       );
                }
            }
        }

        private static bool LevelTypesSame(eMerchandiseType merchandiseType, eROLevelsType ROLevelType)
        {
            if (merchandiseType == eMerchandiseType.HierarchyLevel
                && ROLevelType == eROLevelsType.HierarchyLevel)
            {
                return true;
            }

            if (merchandiseType == eMerchandiseType.LevelOffset
                && ROLevelType == eROLevelsType.LevelOffset)
            {
                return true;
            }

            return false;
        }

        public static eHighLevelsType ConvertToHighLevelsType (eMerchandiseType levelType)
        {
            switch (levelType)
            {
                case eMerchandiseType.HierarchyLevel:
                     return eHighLevelsType.HierarchyLevel;
                case eMerchandiseType.LevelOffset:
                    return eHighLevelsType.LevelOffset;
                default:
                    return eHighLevelsType.None;
            }
        }

        public static eLowLevelsType ConvertToLowLevelsType(eMerchandiseType levelType)
        {
            switch (levelType)
            {
                case eMerchandiseType.HierarchyLevel:
                    return eLowLevelsType.HierarchyLevel;
                case eMerchandiseType.LevelOffset:
                    return eLowLevelsType.LevelOffset;
                default:
                    return eLowLevelsType.None;
            }
        }

    }

    public class ListGenerator
    {
        /// <summary>
		/// Fills a KeyValuePair List with colors.
		/// </summary>
		/// <param name="colorList">KeyValuePair object to fill</param>
        /// <remarks>
        /// -1: All Colors
        /// -2: Default
        /// </remarks>
		public static void FillColorList(
            List<KeyValuePair<int, string>> colorList,
            bool addDefaultColor = false,
            bool addAllColors = false
            )
        {
            ColorData colorData = new ColorData();
            DataTable dataTableColors = colorData.Colors_Read();

            if (addDefaultColor)
            {
                colorList.Add(new KeyValuePair<int, string>(
                        -2,
                        "Default")
                        );
            }

            if (addAllColors)
            {
                colorList.Add(new KeyValuePair<int, string>(
                        -1,
                        "All Colors")
                        );
            }

            // sort colors by ID
            dataTableColors = DataTableTools.SortDataTable(dataTable: dataTableColors, sColName: "COLOR_CODE_ID", bAscending: true);

            foreach (DataRow dataRow in dataTableColors.Rows)
            {
                colorList.Add(new KeyValuePair<int, string>(
                    Convert.ToInt32(dataRow["COLOR_CODE_RID"], CultureInfo.CurrentUICulture),
                    dataRow["COLOR_CODE_ID"].ToString() + " - " + dataRow["COLOR_CODE_NAME"].ToString())
                    );
            }

        }

        /// <summary>
        /// Fills class with size dimensions.
        /// </summary>
        /// <remarks>Method must be overridden</remarks>
        public static void FillDimensionSizeList(
            List<ROSizeDimension> sizeDimensionSizes,
            int Key,
            eGetDimensions getDimensions,
            eGetSizes getSizes,
            bool includeDefaultDimension = false,
            bool includeDefaultSize = false,
            bool useSizeCodeKey = false
            )
        {
            ROSizeDimension dimensionSizes;
            int dimensionKey;
            string dimension;
            SizeModelData sizeModelData = new SizeModelData();
            MaintainSizeConstraints maint = new MaintainSizeConstraints(sizeModelData);
            DataTable dtDimensions = maint.FillSizeDimensionList(Key, getDimensions);
            DataTable dtSizes = maint.FillSizesList(Key, getSizes);

            if (includeDefaultDimension)
            {
                dimensionSizes = new ROSizeDimension(dimension: new KeyValuePair<int, string>(
                    -1,
                    "Default")
                    );
                sizeDimensionSizes.Add(dimensionSizes
                    );
            }

            foreach (DataRow dr in dtDimensions.Rows)
            {
                dimensionKey = Convert.ToInt32(dr["DIMENSIONS_RID"]);
                dimension = dr["SIZE_CODE_SECONDARY"].ToString();
                dimensionSizes = new ROSizeDimension(dimension: new KeyValuePair<int, string>(
                    dimensionKey,
                    dimension)
                    );
                FillSizesList(
                    dimensionSizes: dimensionSizes,
                    dtSizes: dtSizes,
                    dimensionKey: dimensionKey,
                    includeDefaultSize: includeDefaultSize,
                    useSizeCodeKey: useSizeCodeKey
                    );
                sizeDimensionSizes.Add(dimensionSizes
                    );

            }
        }

        /// <summary>
		/// Fills class with sizes based on a selected Size Group or Size Curve
		/// </summary>
		private static void FillSizesList(
            ROSizeDimension dimensionSizes,
            DataTable dtSizes, int dimensionKey,
            bool includeDefaultSize = false,
            bool useSizeCodeKey = false)
        {
            int sizeKey;
            string size;

            DataRow[] SelectRows = dtSizes.Select("DIMENSIONS_RID = '" + dimensionKey.ToString() + "'");

            if (includeDefaultSize)
            {
                dimensionSizes.Sizes.Add(new KeyValuePair<int, string>(
                    -1,
                    "Default")
                    );
            }

            foreach (DataRow dr in SelectRows)
            {
                if (useSizeCodeKey)
                {
                    sizeKey = Convert.ToInt32(dr["SIZE_CODE_RID"]);
                }
                else
                {
                    sizeKey = Convert.ToInt32(dr["SIZES_RID"]);
                }
                size = dr["SIZE_CODE_PRIMARY"].ToString();
                dimensionSizes.Sizes.Add(new KeyValuePair<int, string>(
                    sizeKey,
                    size)
                    );
            }
        }

        /// <summary>
		/// Fills a KeyValuePair List with size groups.
		/// </summary>
		/// <param name="sizeGroups">KeyValuePair object to fill</param>
		public static void FillSizeGroupList(
            List<KeyValuePair<int, string>> sizeGroups,
            bool includeUndefinedGroup = false)

        {
            SizeGroup dataLayersizeGroupData = new SizeGroup();
            DataTable dataTableSizeGroups = dataLayersizeGroupData.GetSizeGroups(includeUndefinedGroup);
            sizeGroups.AddRange(DataTableTools.DataTableToKeyValues(dataTableSizeGroups, "SIZE_GROUP_RID", "SIZE_GROUP_NAME"));
        }

        /// <summary>
		/// Fills a KeyValuePair List with size curve groups.
		/// </summary>
		/// <param name="sizeCurveGroups">KeyValuePair object to fill</param>
        /// <param name="sizeCurveGroupKey">key if for a specific size curve group</param>
		public static void FillSizeCurveGroupList(
            List<KeyValuePair<int, string>> sizeCurveGroups,
            int sizeCurveGroupKey = Include.NoRID
            )
        {
            SizeCurve sizeCurveData = new SizeCurve();
            DataTable dataTableSizeCurveGroups;
            if (sizeCurveGroupKey != Include.NoRID)
            {
                dataTableSizeCurveGroups = sizeCurveData.GetSpecificSizeCurveGroup(sizeCurveGroupKey);
            }
            else
            {
                dataTableSizeCurveGroups = sizeCurveData.GetSizeCurveGroups();
            }
            sizeCurveGroups.AddRange(DataTableTools.DataTableToKeyValues(dataTableSizeCurveGroups, "SIZE_CURVE_GROUP_RID", "SIZE_CURVE_GROUP_NAME"));

        }

        /// <summary>
		/// Fills a KeyValuePair List with size constraints.
		/// </summary>
		/// <param name="sizeConstraints">KeyValuePair object to fill</param>
        /// <param name="sizeConstraintKey">key if for a specific constraint model</param>
		public static void FillSizeConstraintList(
            List<KeyValuePair<int, string>> sizeConstraints,
            int sizeConstraintKey = Include.NoRID
            )
        {
            SizeModelData sizeModelData = new SizeModelData();
            DataTable dataTableSizeConstraints;
            if (sizeConstraintKey != Include.NoRID)
            {
                dataTableSizeConstraints = sizeModelData.SizeConstraintModel_Read(Include.NoRID);
            }
            else
            {
                dataTableSizeConstraints = sizeModelData.SizeConstraintModel_Read();
            }
            sizeConstraints.AddRange(DataTableTools.DataTableToKeyValues(dataTableSizeConstraints, "SIZE_CONSTRAINT_RID", "SIZE_CONSTRAINT_NAME"));

        }

        /// <summary>
		/// Fills a KeyValuePair List with size alternate models.
		/// </summary>
		/// <param name="sizeAlternateModels">KeyValuePair object to fill</param>
        /// <param name="sizeAlternateKey">key if for a specific constraint model</param>
		public static void FillSizeAlternateModelsList(
            List<KeyValuePair<int, string>> sizeAlternateModels,
            int sizeAlternateKey = Include.NoRID
            )

        {
            SizeModelData sizeModelData = new SizeModelData();
            DataTable dataTableSizeAlternates;
            if (sizeAlternateKey != Include.NoRID)
            {
                dataTableSizeAlternates = sizeModelData.SizeAlternateModel_Read(sizeAlternateKey);
            }
            else
            {
                dataTableSizeAlternates = sizeModelData.SizeAlternateModel_Read();
            }
            sizeAlternateModels.AddRange(DataTableTools.DataTableToKeyValues(dataTableSizeAlternates, "SIZE_ALTERNATE_RID", "SIZE_ALTERNATE_NAME"));
        }

        /// <summary>
		/// Fills class with hierarchy levels
		/// </summary>
		public static void FillOrganizationalHierarchyLevelList(
            List<KeyValuePair<int, string>> hierarchyLevels,
            SessionAddressBlock sessionAddressBlock,
            bool includeSizeLevel = false
            )
        {
            if (hierarchyLevels == null)
            {
                hierarchyLevels = new List<KeyValuePair<int, string>>();
            }

            HierarchyProfile hierarchyProfile = sessionAddressBlock.HierarchyServerSession.GetMainHierarchyData();

            for (int levelIndex = 1; levelIndex <= hierarchyProfile.HierarchyLevels.Count; levelIndex++)
            {
                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[levelIndex];

                if (hlp.LevelType != eHierarchyLevelType.Size
                    || includeSizeLevel
                    )
                {
                    hierarchyLevels.Add(new KeyValuePair<int, string>(
                        hlp.Key,
                        hlp.LevelID)
                        );
                }
            }
        }

        /// <summary>
		/// Fills class with header characteristics
		/// </summary>
		public static void FillHeaderCharacteristicList(
            List<KeyValuePair<int, string>> headerCharacteristics,
            SessionAddressBlock sessionAddressBlock
            )
        {
            if (headerCharacteristics == null)
            {
                headerCharacteristics = new List<KeyValuePair<int, string>>();
            }

            HeaderCharGroupProfileList headerCharGroupProfileList = sessionAddressBlock.HeaderServerSession.GetHeaderCharGroups();
            foreach (HeaderCharGroupProfile hcgp in headerCharGroupProfileList)
            {
                headerCharacteristics.Add(new KeyValuePair<int, string>(
                        hcgp.Key,
                        hcgp.ID)
                        );
            }
        }

        /// <summary>
		/// Fills class with header characteristics
		/// </summary>
		public static void FillNameExtensionList(
            List<KeyValuePair<int, string>> nameExtensions,
            SessionAddressBlock sessionAddressBlock
            )
        {
            if (nameExtensions == null)
            {
                nameExtensions = new List<KeyValuePair<int, string>>();
            }

            MerchandiseHierarchyData merchandiseHierarchyData = new MerchandiseHierarchyData();
            DataTable dataTableNodeSizeCurves = merchandiseHierarchyData.SizeCurveNames_Read();
            if (dataTableNodeSizeCurves.Rows.Count > 0)
            {
                for (int i = 0; i < dataTableNodeSizeCurves.Rows.Count; i++)
                {
                    DataRow dr = dataTableNodeSizeCurves.Rows[i];
                    nameExtensions.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["NSCCD_RID"]),
                        Convert.ToString(dr["CURVE_NAME"]))
                        );
                }
            }
        }

        /// <summary>
		/// Fills class with VSW size constraint rules
		/// </summary>
		public static void FillVSWSizeConstraintRuleList(
            List<KeyValuePair<int, string>> VSWSizeConstraintRules
            )
        {
            if (VSWSizeConstraintRules == null)
            {
                VSWSizeConstraintRules = new List<KeyValuePair<int, string>>();
            }

            DataTable dataTableVSWSizeConstraintRules = MIDText.GetTextType(eMIDTextType.eVSWSizeConstraints, eMIDTextOrderBy.TextCode);

            VSWSizeConstraintRules.AddRange(DataTableTools.DataTableToKeyValues(dataTableVSWSizeConstraintRules, "TEXT_CODE", "TEXT_VALUE"));
        }

        /// <summary>
		/// Fills class with size constraint rules
		/// </summary>
		public static void FillSizeConstraintRuleList(
            List<KeyValuePair<int, string>> sizeConstraintRules,
            bool withQuantity = true
            )
        {
            if (sizeConstraintRules == null)
            {
                sizeConstraintRules = new List<KeyValuePair<int, string>>();
            }

            if (withQuantity) 
            {
                DataTable dataTableRules = MIDText.GetLabels((int)eSizeRuleType.Exclude, (int)eSizeRuleType.AbsoluteQuantity);
                sizeConstraintRules.AddRange(DataTableTools.DataTableToKeyValues(dataTableRules, "TEXT_CODE", "TEXT_VALUE"));

                dataTableRules = MIDText.GetLabels((int)eSizeRuleType.SizeMinimum, (int)eSizeRuleType.SizeMaximum);
                sizeConstraintRules.AddRange(DataTableTools.DataTableToKeyValues(dataTableRules, "TEXT_CODE", "TEXT_VALUE"));

            }
            else
            {
                DataTable dataTableRules = MIDText.GetLabels((int)eSizeRuleType.Exclude, (int)eSizeRuleType.Exclude);
                sizeConstraintRules.AddRange(DataTableTools.DataTableToKeyValues(dataTableRules, "TEXT_CODE", "TEXT_VALUE"));

            }
        }

        /// <summary>
		/// Fills class with allocation filters
		/// </summary>
		public static void FillStoreFilterList(
            List<KeyValuePair<int, string>> filters,
            int userKey
            )
        {
            FilterData storeFilterData = new FilterData();

            if (filters == null)
            {
                filters = new List<KeyValuePair<int, string>>();
            }

            ArrayList userRIDList = new ArrayList();
            userRIDList.Add(Include.GlobalUserRID);
            userRIDList.Add(userKey);

            foreach (DataRow row in storeFilterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList).Rows)
            {
                filters.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(row["FILTER_RID"]),
                        Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture))
                        );
            }
        }
    }
}