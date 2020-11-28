//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;
//using MIDRetail.Business;

//namespace MIDRetail.Windows.Controls
//{
//    public class optionDefinition
//    {
//        public bool ShowGroupingLabels = false;
//        public bool ShowEstimatedCost = false;
//        public bool DisplayWarningBeforeRemovingCondition = true;
//        public bool ShowToolTips = true;
//        public bool VariableDisplayNameOnly = false;
//        public bool VariableDisplayDescriptive = true;
//        public bool ColorFormatBlackBlueRed = true;
//        public bool ColorFormatGreenBlueRed = false;
//        public bool ColorFormatBlackRed = false;
//    }
//    public static class filterMessages
//    {
//        public static string maxLimitInvalidNumber = "Please enter a valid number for the result limit.";
//        public static string maxLimitRestoringOriginal = "Restoring original value.";
//        public static string maxLimitInvalidNumberTitle = "Invalid Result Limit";
//        public static string maxLimitHighestValue = "Result limit cannot be greater than {0}.";
//        public static string maxLimitLowestValue = "Result limit cannot be less than 50.";
//    }
//    public static class filterUtility
//    {

//        public static void DataRowCopy(DataRow drOld, DataRow drNew)
//        {
//            foreach (DataColumn col in drOld.Table.Columns)
//            {
//                if (drNew.Table.Columns.Contains(col.ColumnName))
//                {
//                    drNew[col.ColumnName] = drOld[col.ColumnName];
//                }
//            }
//        }
//        public const float MaxFormattingWidth = 400;
//        private const string greenFont = "<span style='color:Green;'>";
//        private const string blueFont = "<span style='color:Blue;'>";
//        private const string redFont = "<span style='color:Red;'>";
//        private const string backcolor = "<span style='background-color:#cc99ff;'>";
//        private const string endSpan = "</span>";
//        public static string FormatLogic(optionDefinition options, string stringToFormat)
//        {
//            if (options.ColorFormatGreenBlueRed)
//            {
//                return blueFont + stringToFormat + endSpan;
//            }
//            else if (options.ColorFormatBlackBlueRed)
//            {
//                return blueFont + stringToFormat + endSpan;
//            }
//            else if (options.ColorFormatBlackRed)
//            {
//                return stringToFormat;
//            }
//            else
//            {
//                return stringToFormat;
//            }
//        }
//        public static string FormatOperator(optionDefinition options, string stringToFormat)
//        {
//            if (options.ColorFormatGreenBlueRed)
//            {
//                return blueFont + stringToFormat.Replace("<", " &lt;").Replace(">", " &gt;") + endSpan;
//            }
//            else if (options.ColorFormatBlackBlueRed)
//            {
//                return blueFont + stringToFormat.Replace("<", " &lt;").Replace(">", " &gt;") + endSpan;
//            }
//            else if (options.ColorFormatBlackRed)
//            {
//                return stringToFormat.Replace("<", " &lt;").Replace(">", " &gt;");
//            }
//            else
//            {
//                return stringToFormat;
//            }
//        }
//        public static string FormatNormal(optionDefinition options, string stringToFormat)
//        {
//            if (options.ColorFormatGreenBlueRed)
//            {
//                return greenFont + stringToFormat + endSpan;
//            }
//            else if (options.ColorFormatBlackBlueRed)
//            {
//                return stringToFormat;
//            }
//            else if (options.ColorFormatBlackRed)
//            {
//                return stringToFormat;
//            }
//            else
//            {
//                return stringToFormat;
//            }
//        }
//        public static string FormatValue(optionDefinition options, string stringToFormat)
//        {
//            if (options.ColorFormatGreenBlueRed)
//            {
//                return redFont + stringToFormat + endSpan;
//            }
//            else if (options.ColorFormatBlackBlueRed)
//            {
//                return redFont + stringToFormat + endSpan;
//            }
//            else if (options.ColorFormatBlackRed)
//            {
//                return redFont + stringToFormat + endSpan;
//            }
//            else
//            {
//                return stringToFormat;
//            }
//        }
//        public static string FormatBlack(optionDefinition options, string stringToFormat)
//        {
//            return stringToFormat;
//        }
//        //public static string ShowEstimatedCost(filterManager manager, filterCondition fc, string stringToDisplay)
//        //{
//        //    if (manager.Options.ShowEstimatedCost)
//        //    {
//        //        ConditionBase cb = DefinitionList.FromString(manager.currentFilter.filterType, fc.conditionType);
//        //        stringToDisplay = " (Cost = " + cb.costToRunEstimate.ToString() + ")";
//        //        return stringToDisplay;
//        //    }
//        //    else
//        //    {
//        //        return string.Empty;
//        //    }
//        //}
//        public static GetDateRangeTextForPlanDelegate getDateRangeTextForPlanDelegate = null;
//        public static GetTextFromHierarchyNodeDelegate getTextFromHierarchyNodeDelegate = null;
//        public static string BuildNodeTextForVariable(optionDefinition options, filterCondition fc, bool useVariable1, bool useVariable2)
//        {
//            string variableID = string.Empty;
//            string merchandiseID = string.Empty;
//            string versionID = string.Empty;
//            string calendarID = string.Empty;
//            string valueTypeID = string.Empty;
//            string timeID = string.Empty;


//            if (useVariable1)
//            {
//                variableID = filterDataHelper.VariablesGetNameFromIndex(fc.variable1_Index);
//                merchandiseID = getTextFromHierarchyNodeDelegate(fc.variable1_HN_RID);
//                versionID = filterDataHelper.VersionsGetNameFromIndex(fc.variable1_VersionIndex);
//                calendarID = getDateRangeTextForPlanDelegate(fc.variable1_CDR_RID);
              
               
//                valueTypeID = variableValueTypes.FromIndex(fc.variable1_VariableValueTypeIndex).Name;
//                timeID = variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex).Name;
//            }
//            else if (useVariable2)
//            {
//                variableID = filterDataHelper.VariablesGetNameFromIndex(fc.variable2_Index);
//                merchandiseID = getTextFromHierarchyNodeDelegate(fc.variable2_HN_RID);
//                versionID = filterDataHelper.VersionsGetNameFromIndex(fc.variable2_VersionIndex);
//                calendarID = getDateRangeTextForPlanDelegate(fc.variable2_CDR_RID);
              
//                valueTypeID = variableValueTypes.FromIndex(fc.variable2_VariableValueTypeIndex).Name;
//                timeID = variableTimeTypes.FromIndex(fc.variable2_TimeTypeIndex).Name;
//            }

//            string nodeText = string.Empty;

//            nodeText += variableID;
//            if (options.VariableDisplayDescriptive)
//            {
//                nodeText += "[";
//                if (merchandiseID != string.Empty)
//                {
//                    nodeText += merchandiseID + ", ";
//                }
//                if (versionID != string.Empty)
//                {
//                    nodeText += versionID + ", ";
//                }
//                if (calendarID != string.Empty)
//                {
//                    nodeText += calendarID + ", ";
//                }
//                nodeText += valueTypeID + ", ";
//                nodeText += timeID;
//                nodeText += "]";
//            }
//            return nodeText;
//        }
//        public static string BuildFormattedTextForVariable(optionDefinition options, filterCondition fc, bool useVariable1, bool useVariable2)
//        {
//            string formattedText = FormatNormal(options, BuildNodeTextForVariable(options, fc, useVariable1, useVariable2));

//            return formattedText;
//        }


//        //public static string GetDisplayList(string delimitedListFromCondition, DataTable dt, string fieldRID, string fieldDisplay)
//        //{
//        //    string sList = string.Empty;
//        //    if (delimitedListFromCondition == "All")
//        //    {
//        //        sList = "All";
//        //    }
//        //    else if (delimitedListFromCondition == null || delimitedListFromCondition == string.Empty || delimitedListFromCondition == "None")
//        //    {
//        //        sList = "None";
//        //    }
//        //    else
//        //    {
//        //        string[] delimitedList = delimitedListFromCondition.Split(',');
//        //        bool isOverWidth = false;
//        //        foreach (string tagToCompare in delimitedList)
//        //        {
//        //            if (isOverWidth == false)
//        //            {
//        //                //assume tag is integer
//        //                int RID;
//        //                int.TryParse(tagToCompare, out RID);
//        //                //lookup display value based on RID
//        //                //DataRow[] drFind = StoreDataSet.Tables[0].Select("STORE_RID = " + RID);
//        //                DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
//        //                if (drFind.Length > 0)
//        //                {
//        //                    //string displayValue = (string)drFind[0]["Store"];
//        //                    string displayValue = (string)drFind[0][fieldDisplay];

//        //                    //compare string to see if it goes over the max size
//        //                    string proposedDisplayList = sList;

//        //                    if (proposedDisplayList == string.Empty)
//        //                    {
//        //                        proposedDisplayList = displayValue;
//        //                    }
//        //                    else
//        //                    {
//        //                        proposedDisplayList += ", " + displayValue;
//        //                    }

//        //                    if (IsDisplayStringOverMaxWidth(proposedDisplayList))
//        //                    {
//        //                        isOverWidth = true;
//        //                        proposedDisplayList = sList + "...";
//        //                    }

//        //                    sList = proposedDisplayList;
//        //                }
//        //            }
//        //        }
//        //    }
//        //    return sList;
//        //}
//        public static string GetDisplayList(listConstantTypes constantType, DataRow[] drListValues, DataTable dt, string fieldRID, string fieldDisplay)
//        {
//            string sList = string.Empty;
//            if (constantType == listConstantTypes.All)
//            {
//                sList = listConstantTypes.All.Name;
//            }
//            else if (constantType == listConstantTypes.None)
//            {
//                sList = listConstantTypes.None.Name;
//            }
//            else
//            {
//                //string[] delimitedList = delimitedListFromCondition.Split(',');
//                bool isOverWidth = false;
//                foreach (DataRow drListValue in drListValues)
//                {
//                    if (isOverWidth == false)
//                    {
//                        //assume tag is integer
//                        int RID;
//                        string listValueIndexField = filterCondition.GetListValueIndexField();
//                        RID = (int)drListValue[listValueIndexField];
//                        //lookup display value based on RID
//                        //DataRow[] drFind = StoreDataSet.Tables[0].Select("STORE_RID = " + RID);
//                        DataRow[] drFind = dt.Select(fieldRID + " = " + RID);
//                        if (drFind.Length > 0)
//                        {
//                            //string displayValue = (string)drFind[0]["Store"];
//                            string displayValue = (string)drFind[0][fieldDisplay];

//                            //compare string to see if it goes over the max size
//                            string proposedDisplayList = sList;

//                            if (proposedDisplayList == string.Empty)
//                            {
//                                proposedDisplayList = displayValue;
//                            }
//                            else
//                            {
//                                proposedDisplayList += ", " + displayValue;
//                            }

//                            if (IsDisplayStringOverMaxWidth(proposedDisplayList))
//                            {
//                                isOverWidth = true;
//                                proposedDisplayList = sList + "...";
//                            }

//                            sList = proposedDisplayList;
//                        }
//                    }
//                }
//            }
//            return sList;
//        }

//        private static bool IsDisplayStringOverMaxWidth(string stringToDisplay)
//        {
//            System.Drawing.Bitmap b = new System.Drawing.Bitmap(1, 1);
//            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

//            // font data should match the font in the filterBuilderListNode
//            System.Drawing.Font stringFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);

//            // Measure string.
//            System.Drawing.SizeF stringSize = new System.Drawing.SizeF();
//            stringSize = g.MeasureString(stringToDisplay, stringFont);
//            float stringWidth = stringSize.Width;

//            b.Dispose();
//            g.Dispose();

//            if (stringWidth > filterUtility.MaxFormattingWidth)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static void SetMaskForNumericEditor(numericTypes nType, Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditor)
//        {
//            if (nType == numericTypes.Dollar)
//            {
//                numericEditor.MaskInput = "{LOC}$nnn,nnn,nnn,nnn.nn";
//            }
//            else if (nType == numericTypes.DoubleFreeForm)
//            {
//                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
//                //this.numericEditor.ResetMaskInput();
//                numericEditor.MaskInput = "{LOC}nn,nnn,nnn.nnnn";
//            }
//            else if (nType == numericTypes.Integer)
//            {
//                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
//                numericEditor.ResetMaskInput();
//                numericEditor.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Integer;
//                numericEditor.MaskInput = "{LOC}nn,nnn,nnn";
//            }
//        }
//    }
//}
