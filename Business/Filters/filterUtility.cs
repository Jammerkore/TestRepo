using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public class filterOptionDefinition
    {
        public bool ShowGroupingLabels = false;
        public bool ShowEstimatedCost = false;
        public bool ShowExecutionPlan = false;
        public bool DisplayWarningBeforeRemovingCondition = true;
        public bool ShowToolTips = true;
        public bool VariableDisplayNameOnly = false;
        public bool VariableDisplayDescriptive = true;
        public bool ColorFormatBlackBlueRed = true;
        public bool ColorFormatGreenBlueRed = false;
        public bool ColorFormatBlackRed = false;
    }

    public sealed class filterToolbarImages
    {
        public static List<filterToolbarImages> navTypeList = new List<filterToolbarImages>();
        public static readonly filterToolbarImages None = new filterToolbarImages(-1);
        public static readonly filterToolbarImages StoreList = new filterToolbarImages(0);
        public static readonly filterToolbarImages Field = new filterToolbarImages(1);
        public static readonly filterToolbarImages Status = new filterToolbarImages(2); //blue flag
        public static readonly filterToolbarImages Characteristics = new filterToolbarImages(3);
        public static readonly filterToolbarImages AttributeSet = new filterToolbarImages(4);
        public static readonly filterToolbarImages VariableToVariable = new filterToolbarImages(5);
        public static readonly filterToolbarImages VariablePercentage = new filterToolbarImages(6);
        public static readonly filterToolbarImages VariableToConstant = new filterToolbarImages(7);
        public static readonly filterToolbarImages SortBy = new filterToolbarImages(8);
        public static readonly filterToolbarImages HeaderType = new filterToolbarImages(9);
        public static readonly filterToolbarImages Date1 = new filterToolbarImages(10);
        public static readonly filterToolbarImages Date2 = new filterToolbarImages(11);
        public static readonly filterToolbarImages HeaderMerchandise = new filterToolbarImages(12);
        public static readonly filterToolbarImages ProductAny = new filterToolbarImages(21);
        public static readonly filterToolbarImages ProductID = new filterToolbarImages(22);
        public static readonly filterToolbarImages ProductName = new filterToolbarImages(23);
        public static readonly filterToolbarImages ProductDescrip = new filterToolbarImages(24);
        public static readonly filterToolbarImages ProductActive = new filterToolbarImages(15);
        public static readonly filterToolbarImages ProductLevels = new filterToolbarImages(25);
        public static readonly filterToolbarImages ProductHierarchies = new filterToolbarImages(26);
        public static readonly filterToolbarImages User = new filterToolbarImages(27);
        public static readonly filterToolbarImages Status2 = new filterToolbarImages(28); //green flag

        private filterToolbarImages(int Index)
        {
            this.Index = Index;
            navTypeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterToolbarImages op) { return op.Index; }


        public static filterToolbarImages FromIndex(int Index)
        {
            filterToolbarImages result = navTypeList.Find(
               delegate(filterToolbarImages ft)
               {
                   return ft.Index == Index;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }

    public static class filterMessages
    {
        public static string maxLimitInvalidNumber = "Please enter a valid number for the result limit.";
        public static string maxLimitRestoringOriginal = "Restoring original value.";
        public static string maxLimitInvalidNumberTitle = "Invalid Result Limit";
        public static string maxLimitHighestValue = "Result limit cannot be greater than {0}.";
        public static string maxLimitLowestValue = "Result limit cannot be less than 50.";
        public static string overSortByMaxConditions = "Filters only support a maximum of five sort by conditions.";
    }

    public static class filterUtility
    {

        public static void DataRowCopy(DataRow drOld, DataRow drNew)
        {
            foreach (DataColumn col in drOld.Table.Columns)
            {
                if (drNew.Table.Columns.Contains(col.ColumnName))
                {
                    drNew[col.ColumnName] = drOld[col.ColumnName];
                }
            }
        }
        public const float MaxFormattingWidth = 400;
        private const string greenFont = "<span style='color:Green;'>";
        private const string blueFont = "<span style='color:Blue;'>";
        private const string redFont = "<span style='color:Red;'>";
        private const string backcolor = "<span style='background-color:#cc99ff;'>";
        private const string endSpan = "</span>";

        public static bool showResultsInNewTab = false;
        public static bool auditFilterIncludeSummary = true;
        public static bool auditFilterIncludeDetails = true;
        public static bool auditFilterMergeDetails = false;

        public static string FormatLogic(filterOptionDefinition options, string stringToFormat)
        {
            if (options.ColorFormatGreenBlueRed)
            {
                return blueFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackBlueRed)
            {
                return blueFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackRed)
            {
                return EscapeStringToFormat(stringToFormat);
            }
            else
            {
                return EscapeStringToFormat(stringToFormat);
            }
        }
        public static string FormatOperator(filterOptionDefinition options, string stringToFormat)
        {
            if (options.ColorFormatGreenBlueRed)
            {
                return blueFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackBlueRed)
            {
                return blueFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackRed)
            {
                return EscapeStringToFormat(stringToFormat);
            }
            else
            {
                return EscapeStringToFormat(stringToFormat);
            }
        }
        public static string FormatNormal(filterOptionDefinition options, string stringToFormat)
        {
            if (options.ColorFormatGreenBlueRed)
            {
                return greenFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackBlueRed)
            {
                return EscapeStringToFormat(stringToFormat);
            }
            else if (options.ColorFormatBlackRed)
            {
                return EscapeStringToFormat(stringToFormat);
            }
            else
            {
                return EscapeStringToFormat(stringToFormat);
            }
        }
        public static string FormatValue(filterOptionDefinition options, string stringToFormat)
        {
        
            if (options.ColorFormatGreenBlueRed)
            {
                return redFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackBlueRed)
            {
                return redFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else if (options.ColorFormatBlackRed)
            {
                return redFont + EscapeStringToFormat(stringToFormat) + endSpan;
            }
            else
            {
                return EscapeStringToFormat(stringToFormat);
            }
        }
        public static string EscapeStringToFormat(string stringToFormat)
        {
            return stringToFormat.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"); //TT#1369-MD -jsobek -Header Filter name is unreadable in the "Information" Section (maybe due to special characters?)
        }
        public static string UnEscapeStringToFormat(string stringToFormat)
        {
            return stringToFormat.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">"); 
        }

        public static string FormatBlack(filterOptionDefinition options, string stringToFormat)
        {
            return EscapeStringToFormat(stringToFormat);
        }
        //public static string ShowEstimatedCost(filterManager manager, filterCondition fc, string stringToDisplay)
        //{
        //    if (manager.Options.ShowEstimatedCost)
        //    {
        //        ConditionBase cb = DefinitionList.FromString(manager.currentFilter.filterType, fc.conditionType);
        //        stringToDisplay = " (Cost = " + cb.costToRunEstimate.ToString() + ")";
        //        return stringToDisplay;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        public static GetDateRangeTextForPlanDelegate getDateRangeTextForPlanDelegate = null;
        public static GetTextFromHierarchyNodeDelegate getTextFromHierarchyNodeDelegate = null;

        public delegate HierarchyNodeProfile GetFirstSelectedMerchandiseNodeDelegate();
        public static GetFirstSelectedMerchandiseNodeDelegate GetFirstSelectedMerchandiseNode = null;
        public static string BuildNodeTextForVariable(filterOptionDefinition options, filterCondition fc, bool useVariable1, bool useVariable2)
        {
            string variableID = string.Empty;
            string merchandiseID = string.Empty;
            string versionID = string.Empty;
            string calendarID = string.Empty;
            string valueTypeID = string.Empty;
            string timeID = string.Empty;


            if (useVariable1)
            {
                variableID = filterDataHelper.VariablesGetNameFromIndex(fc.variable1_Index);
                merchandiseID = getTextFromHierarchyNodeDelegate(fc.variable1_HN_RID);
                versionID = filterDataHelper.VersionsGetNameFromIndex(fc.variable1_VersionIndex);
                calendarID = getDateRangeTextForPlanDelegate(fc.variable1_CDR_RID);


                valueTypeID = variableValueTypes.FromIndex(fc.variable1_VariableValueTypeIndex).Name;
                timeID = variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex).Name;
            }
            else if (useVariable2)
            {
                variableID = filterDataHelper.VariablesGetNameFromIndex(fc.variable2_Index);
                merchandiseID = getTextFromHierarchyNodeDelegate(fc.variable2_HN_RID);
                versionID = filterDataHelper.VersionsGetNameFromIndex(fc.variable2_VersionIndex);
                calendarID = getDateRangeTextForPlanDelegate(fc.variable2_CDR_RID);

                valueTypeID = variableValueTypes.FromIndex(fc.variable2_VariableValueTypeIndex).Name;
                timeID = variableTimeTypes.FromIndex(fc.variable2_TimeTypeIndex).Name;
            }

            string nodeText = string.Empty;

            nodeText += variableID;
            if (options.VariableDisplayDescriptive)
            {
                nodeText += "[";
                if (merchandiseID != string.Empty)
                {
                    nodeText += merchandiseID + ", ";
                }
                if (versionID != string.Empty)
                {
                    nodeText += versionID + ", ";
                }
                if (calendarID != string.Empty)
                {
                    nodeText += calendarID + ", ";
                }
                nodeText += valueTypeID + ", ";
                nodeText += timeID;
                nodeText += "]";
            }
            return nodeText;
        }
        public static string BuildFormattedTextForVariable(filterOptionDefinition options, filterCondition fc, bool useVariable1, bool useVariable2)
        {
            string formattedText = FormatNormal(options, BuildNodeTextForVariable(options, fc, useVariable1, useVariable2));

            return formattedText;
        }


        //public static string GetDisplayList(string delimitedListFromCondition, DataTable dt, string fieldRID, string fieldDisplay)
        //{
        //    string sList = string.Empty;
        //    if (delimitedListFromCondition == "All")
        //    {
        //        sList = "All";
        //    }
        //    else if (delimitedListFromCondition == null || delimitedListFromCondition == string.Empty || delimitedListFromCondition == "None")
        //    {
        //        sList = "None";
        //    }
        //    else
        //    {
        //        string[] delimitedList = delimitedListFromCondition.Split(',');
        //        bool isOverWidth = false;
        //        foreach (string tagToCompare in delimitedList)
        //        {
        //            if (isOverWidth == false)
        //            {
        //                //assume tag is integer
        //                int RID;
        //                int.TryParse(tagToCompare, out RID);
        //                //lookup display value based on RID
        //                //DataRow[] drFind = StoreDataSet.Tables[0].Select("STORE_RID = " + RID);
        //                DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
        //                if (drFind.Length > 0)
        //                {
        //                    //string displayValue = (string)drFind[0]["Store"];
        //                    string displayValue = (string)drFind[0][fieldDisplay];

        //                    //compare string to see if it goes over the max size
        //                    string proposedDisplayList = sList;

        //                    if (proposedDisplayList == string.Empty)
        //                    {
        //                        proposedDisplayList = displayValue;
        //                    }
        //                    else
        //                    {
        //                        proposedDisplayList += ", " + displayValue;
        //                    }

        //                    if (IsDisplayStringOverMaxWidth(proposedDisplayList))
        //                    {
        //                        isOverWidth = true;
        //                        proposedDisplayList = sList + "...";
        //                    }

        //                    sList = proposedDisplayList;
        //                }
        //            }
        //        }
        //    }
        //    return sList;
        //}
        public static string GetDisplayList(filterListConstantTypes constantType, DataRow[] drListValues, DataTable dt, string fieldRID, string fieldDisplay)
        {
            string sList = string.Empty;
            if (constantType == filterListConstantTypes.All)
            {
                sList = filterListConstantTypes.All.Name;
            }
            else if (constantType == filterListConstantTypes.None)
            {
                sList = filterListConstantTypes.None.Name;
            }
            else
            {
                //string[] delimitedList = delimitedListFromCondition.Split(',');
                bool isOverWidth = false;
                
                foreach (DataRow drListValue in drListValues)
                {
                    if (isOverWidth == false)
                    {
                        //assume tag is integer
                        int RID;
                        string listValueIndexField = filterCondition.GetListValueIndexField();
                        RID = (int)drListValue[listValueIndexField];
                        //lookup display value based on RID
                        //DataRow[] drFind = StoreDataSet.Tables[0].Select("STORE_RID = " + RID);
                        DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
                        if (drFind.Length > 0)
                        {
                            //string displayValue = (string)drFind[0]["Store"];
                            string displayValue = (string)drFind[0][fieldDisplay];

                            //compare string to see if it goes over the max size
                            string proposedDisplayList = sList;

                            if (proposedDisplayList == string.Empty)
                            {
                                proposedDisplayList = displayValue;
                            }
                            else
                            {
                                proposedDisplayList += ", " + displayValue;
                            }

                            if (IsDisplayStringOverMaxWidth(proposedDisplayList))
                            {
                                isOverWidth = true;
                                proposedDisplayList = sList + "...";
                            }

                            sList = proposedDisplayList;
                        }
                    }
                }
            }
            return sList;
        }
        public static string GetDisplayListWithParent(filterListConstantTypes constantType, DataRow[] drListValues, DataTable dt, string fieldRID, string fieldDisplay)
        {
            string sList = string.Empty;
            if (constantType == filterListConstantTypes.All)
            {
                sList = filterListConstantTypes.All.Name;
            }
            else if (constantType == filterListConstantTypes.None)
            {
                sList = filterListConstantTypes.None.Name;
            }
            else
            {
                //string[] delimitedList = delimitedListFromCondition.Split(',');
                bool isOverWidth = false;
                bool hasOneValue = false;
                foreach (DataRow drListValue in drListValues)
                {
                    if (isOverWidth == false)
                    {
                        //assume tag is integer
                        int RID;
                        string listValueIndexField = filterCondition.GetListValueIndexField();
                        RID = (int)drListValue[listValueIndexField];
                        if (RID != -1)
                        {
                            hasOneValue = true;
                            //lookup display value based on RID
                            //DataRow[] drFind = StoreDataSet.Tables[0].Select("STORE_RID = " + RID);
                            DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
                            if (drFind.Length > 0)
                            {
                                //string displayValue = (string)drFind[0]["Store"];
                                string displayValue = (string)drFind[0][fieldDisplay];

                                //compare string to see if it goes over the max size
                                string proposedDisplayList = sList;

                                if (proposedDisplayList == string.Empty)
                                {
                                    proposedDisplayList = displayValue;
                                }
                                else
                                {
                                    proposedDisplayList += ", " + displayValue;
                                }

                                if (IsDisplayStringOverMaxWidth(proposedDisplayList))
                                {
                                    isOverWidth = true;
                                    proposedDisplayList = sList + "...";
                                }

                                sList = proposedDisplayList;
                            }
                        }
                    }
                }
                if (hasOneValue == false)
                {
                    sList = filterListConstantTypes.None.Name;
                }
            }
            return sList;
        }

        private static bool IsDisplayStringOverMaxWidth(string stringToDisplay)
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(1, 1);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

            // font data should match the font in the filterBuilderListNode
            System.Drawing.Font stringFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);

            // Measure string.
            System.Drawing.SizeF stringSize = new System.Drawing.SizeF();
            stringSize = g.MeasureString(stringToDisplay, stringFont);
            float stringWidth = stringSize.Width;

            b.Dispose();
            g.Dispose();

            if (stringWidth > filterUtility.MaxFormattingWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetDisplayForDropDown(int RID, DataTable dt, int objectType, string fieldRID, string fieldDisplay)
        {
            string displayValue = string.Empty;

     

            DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
            if (drFind.Length > 0)
            {
                displayValue = (string)drFind[0][fieldDisplay];
            }
            return displayValue;
        }
    }

   
    /// <summary>
    /// Class that defines the contents of the FilterName combo box.
    /// </summary>
    public class FilterNameCombo
    {
        //=======
        // FIELDS
        //=======

        private int _filterRID;
        private int _userRID;
        private string _filterName;
        private string _displayName;
        // Begin TT#1125 - JSmith - Global/User should be consistent
        //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
        // End TT#1125

        //=============
        // CONSTRUCTORS
        //=============

        public FilterNameCombo(int aFilterRID)
        {
            _filterRID = aFilterRID;
        }

        // Begin TT#1125 - JSmith - Global/User should be consistent
        //public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        //{
        //    _filterRID = aFilterRID;
        //    _userRID = aUserRID;
        //    _filterName = aFilterName;
        //    if (aUserRID == Include.GlobalUserRID) // Issue 3806
        //    {
        //        _displayName = _filterName;
        //    }
        //    else
        //    {
        //        _displayName = _filterName + " (User)";
        //    }
        //}
        public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        {
            _filterRID = aFilterRID;
            _userRID = aUserRID;
            _filterName = aFilterName;


            //Begin TT#1418-MD -jsobek -Store Filter combo drop down displays user name twice on User filters.
            //if (aUserRID == Include.GlobalUserRID) // Issue 3806
            //{
            _displayName = _filterName;
            //}
            //else
            //{
            //    //Begin TT#827-MD -jsobek -Allocation Reviews Performance
            //    //secAdmin = new SecurityAdmin();
            //    //_displayName = _filterName + " (" + secAdmin.GetUserName(aUserRID) + ")";
            //    _displayName = _filterName + " (" + UserNameStorage.GetUserName(aUserRID) + ")";
            //    //End TT#827-MD -jsobek -Allocation Reviews Performance
            //}
            //End TT#1418-MD -jsobek -Store Filter combo drop down displays user name twice on User filters.
        }
        // End TT#1125

        //===========
        // PROPERTIES
        //===========

        public int FilterRID
        {
            get
            {
                return _filterRID;
            }
        }

        public int UserRID
        {
            get
            {
                return _userRID;
            }
        }

        public string FilterName
        {
            get
            {
                return _filterName;
            }
        }

        //========
        // METHODS
        //========

        override public string ToString()
        {
            return _displayName;
        }

        override public bool Equals(object obj)
        {
            if (((FilterNameCombo)obj).FilterRID == _filterRID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public int GetHashCode()
        {
            return _filterRID;
        }
    }

 
    /// <summary>
    /// Class that defines the FilterDragObject, which is a generic object used during drag events.
    /// </summary>
    public class FilterDragObject
    {
        //=======
        // FIELDS
        //=======

        public object DragObject;
        public string Text;

        //=============
        // CONSTRUCTORS
        //=============

        public FilterDragObject(object aDragObject)
        {
            DragObject = aDragObject;
            Text = null;
        }

        public FilterDragObject(object aDragObject, string aText)
        {
            DragObject = aDragObject;
            Text = aText;
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========
    }

}
