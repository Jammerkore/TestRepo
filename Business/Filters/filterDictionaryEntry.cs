using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class filterDictionaryEntry
    {
        public filterTypes filterType { get; set; }
        public filterNavigationTypes navigationType = filterNavigationTypes.Condition;
        public int costToRunEstimate { get; set; }
        public filterEntrySettings groupSettings = new filterEntrySettings();
        public List<elementBase> elementList = new List<elementBase>();

        public void LoadFromCondition(filterManager manager, filterCondition condition)
        {
            foreach (elementBase eb in elementList)
            {
                eb.LoadFromCondition(condition);
            }
        }
        public void SaveToCondition(ref filterCondition condition)
        {
            foreach (elementBase eb in elementList)
            {
                eb.SaveToCondition(ref condition);
            }
        }
        public bool IsValid(filterManager manager, filterCondition condition)
        {
            bool isValid = true;
            foreach (elementBase eb in elementList)
            {
                if (eb.IsValid(condition) == false)
                {
                    isValid = false;
                    break;
                }

            }
            return isValid;
        }


        public virtual void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
        }
        public virtual void SetDefaults(ref filterCondition fc)
        {
        }

        public static void BuildFormattedTextForString(ref string formattedText, filterOptionDefinition options, filterCondition fc)
        {
            formattedText += filterUtility.FormatOperator(options, " " + filterStringOperatorTypes.FromIndex(fc.operatorIndex).description);
            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);
        }
        public static void BuildFormattedTextForBoolean(ref string formattedText, filterOptionDefinition options, filterCondition fc)
        {
            formattedText += filterUtility.FormatOperator(options, " " + filterNumericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompareBool.ToString());
        }
        public static void BuildFormattedTextForDate(ref string formattedText, filterOptionDefinition options, filterCondition fc)
        {
            //formattedText += filterUtility.FormatOperator(options, " " + dateOperatorTypes.FromIndex(fc.operatorIndex).description);
            filterDateOperatorTypes opType = filterDateOperatorTypes.FromIndex(fc.operatorIndex);
            if (opType == filterDateOperatorTypes.Unrestricted)
            {
                formattedText += filterUtility.FormatOperator(options, filterDateOperatorTypes.Unrestricted.description);
            }
            else
            {
                string outputFormat = string.Empty;
                filterDateTypes dType = filterDateTypes.FromIndex(fc.dateTypeIndex);
                if (dType == filterDateTypes.DateAndTime)
                {
                    if (opType == filterDateOperatorTypes.Last7Days)
                    {
                        outputFormat = "MM/dd/yyyy";
                    }
                    else
                    {
                        outputFormat = "MM/dd/yyyy hh:mm tt";
                    }
                }
                else if (dType == filterDateTypes.TimeOnly)
                {
                    outputFormat = "hh:mm tt";
                }
                else //if (dType == dateTypes.DateOnly)
                {
                    outputFormat = "MM/dd/yyyy";
                }

                DateTime dateFrom = DateTime.Now;
                if (fc.valueToCompareDateFrom != null) dateFrom = (DateTime)fc.valueToCompareDateFrom;
                DateTime dateTo = DateTime.Now;
                if (fc.valueToCompareDateTo != null) dateTo = (DateTime)fc.valueToCompareDateTo;

                if (opType == filterDateOperatorTypes.Specify)
                {
                    formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
                    formattedText += filterUtility.FormatOperator(options, " to ");
                    formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
                }
                else if (opType == filterDateOperatorTypes.Between)
                {
                    //formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " to ");
                    //formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " (" + filterDateOperatorTypes.Between.description + ")");

                    formattedText += filterUtility.FormatOperator(options, filterDateOperatorTypes.Between.description + " ");
                    formattedText += filterUtility.FormatValue(options, fc.valueToCompareDateBetweenFromDays.ToString());
                    formattedText += filterUtility.FormatOperator(options, " and ");
                    formattedText += filterUtility.FormatValue(options, fc.valueToCompareDateBetweenToDays.ToString());
                    formattedText += filterUtility.FormatOperator(options, " days");
                    //Begin TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                    if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
                    {
                        formattedText += filterUtility.FormatOperator(options, " (time sensitive)");
                    }
                    //End TT#1660-MD -jsobek -Filters - Between Date - Time Sensitive Option
                }
                else if (opType == filterDateOperatorTypes.Last7Days)
                {
                    //formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " to ");
                    //formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " (" + filterDateOperatorTypes.Last7Days.description + ")");

                    formattedText += filterUtility.FormatOperator(options, filterDateOperatorTypes.Last7Days.description);
                }
                else if (opType == filterDateOperatorTypes.Last24Hours)
                {
                    //formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " to ");
                    //formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
                    //formattedText += filterUtility.FormatOperator(options, " (" + filterDateOperatorTypes.Last24Hours.description + ")");

                    formattedText += filterUtility.FormatOperator(options, filterDateOperatorTypes.Last24Hours.description);
                }
            }
        }

        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
		public static void BuildFormattedTextForCalendar(ref string formattedText, filterOptionDefinition options, filterCondition fc, filterManager manager)
        {
            filterCalendarDateOperatorTypes opType = filterCalendarDateOperatorTypes.FromIndex(fc.operatorIndex);

            if (opType == filterCalendarDateOperatorTypes.Unrestricted)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterCalendarDateOperatorTypes.Unrestricted.description);
            }
            else if (opType == filterCalendarDateOperatorTypes.Last1Week)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterCalendarDateOperatorTypes.Last1Week.description);
            }
            else if (opType == filterCalendarDateOperatorTypes.Next1Week)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterCalendarDateOperatorTypes.Next1Week.description);
            }
            else if (opType == filterCalendarDateOperatorTypes.Next4Weeks)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterCalendarDateOperatorTypes.Next4Weeks.description);
            }
            else if (opType == filterCalendarDateOperatorTypes.Between)
            {
                formattedText += filterUtility.FormatOperator(options, " " + filterCalendarDateOperatorTypes.Between.description + " ");
                formattedText += filterUtility.FormatValue(options, fc.valueToCompareDateBetweenFromDays.ToString());
                formattedText += filterUtility.FormatOperator(options, " and ");
                formattedText += filterUtility.FormatValue(options, fc.valueToCompareDateBetweenToDays.ToString());
                formattedText += filterUtility.FormatOperator(options, " weeks");
                if (fc.valueToCompareBool != null && (bool)fc.valueToCompareBool)
                {
                    formattedText += filterUtility.FormatOperator(options, " (time sensitive)");
                }
            }
            else if (opType == filterCalendarDateOperatorTypes.Specify)
            {
                DateRangeProfile drp = manager.SAB.ClientServerSession.Calendar.GetDateRange((int)fc.date_CDR_RID);

                formattedText += filterUtility.FormatValue(options, " " + drp.DisplayDate);
            }
            else 
            {
                formattedText += filterUtility.FormatOperator(options, " " + opType.symbol);

                DateRangeProfile drp = manager.SAB.ClientServerSession.Calendar.GetDateRange((int)fc.date_CDR_RID);

                formattedText += filterUtility.FormatValue(options, " " + drp.DisplayDate);
            }
        }
		// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

        public static void BuildFormattedTextForNumeric(ref string formattedText, filterOptionDefinition options, filterCondition fc, filterValueTypes vt)
        {
            filterNumericOperatorTypes opType = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
            filterNumericTypes nType = filterNumericTypes.FromIndex(fc.numericTypeIndex);
            formattedText += filterUtility.FormatOperator(options, " " + opType.symbol);

            string outputFormat = string.Empty;
            if (nType == filterNumericTypes.Dollar)
            {
                outputFormat = "$###,###,###,###,##0.00";
            }
            else if (nType == filterNumericTypes.Integer)
            {
                outputFormat = "###,###,###,###,##0";
            }
            else //if (nType == numericTypes.DoubleFreeForm)
            {
                //setting to 4 decimal places as the default
                outputFormat = "###,###,###,###,##0.0000";
            }

            if (nType == filterNumericTypes.Integer)
            {
                if (fc.valueToCompareInt == null)
                {
                    fc.valueToCompare = string.Empty;
                }
                else
                {
                    if (filterNumericOperatorTypes.FromIndex(fc.operatorIndex) == filterNumericOperatorTypes.Between)
                    {
                        int i = (int)fc.valueToCompareInt;
                        int i2 = (int)fc.valueToCompareInt2;
                        fc.valueToCompare = i.ToString(outputFormat);
                        fc.valueToCompare += " and ";
                        fc.valueToCompare += i2.ToString(outputFormat);
                    }
                    else
                    {
                        int i = (int)fc.valueToCompareInt;
                        fc.valueToCompare = i.ToString(outputFormat);
                    }
                }
            }
            else
            {
                if (fc.valueToCompareDouble == null)
                {
                    fc.valueToCompare = string.Empty;
                }
                else
                {
                    if (filterNumericOperatorTypes.FromIndex(fc.operatorIndex) == filterNumericOperatorTypes.Between)
                    {
                        double d = (double)fc.valueToCompareDouble;
                        double d2 = (double)fc.valueToCompareDouble2;
                        fc.valueToCompare = d.ToString(outputFormat);
                        fc.valueToCompare += " and ";
                        fc.valueToCompare += d2.ToString(outputFormat);
                    }
                    else
                    {
                        double d = (double)fc.valueToCompareDouble;
                        fc.valueToCompare = d.ToString(outputFormat);
                    }
                }
            }



            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);  //string to hold all numeric types
        }
        public static void BuildFormattedTextForSortBy(ref string formattedText, filterOptionDefinition options, filterCondition fc)
        {
            if (fc.sortByTypeIndex == filterSortByTypes.StoreCharacteristics)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.StoreCharacteristicsGetNameFromIndex(fc.sortByFieldIndex));
            }
            //else if (fc.sortByTypeIndex == filterSortByTypes.ProductCharacteristics)
            //{
            //    formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.ProductCharacteristicsGetNameFromIndex(fc.sortByFieldIndex));
            //}
            
            else if (fc.sortByTypeIndex == filterSortByTypes.StoreFields)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterStoreFieldTypes.FromIndex(fc.sortByFieldIndex).Name + " ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.ProductSearchFields)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterProductFieldTypes.FromIndex(fc.sortByFieldIndex).Name + " ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.AuditSearchFields)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterAuditSearchTypes.FromIndex(fc.sortByFieldIndex).Name + " ");
            }

            else if (fc.sortByTypeIndex == filterSortByTypes.HeaderFields)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterHeaderFieldTypes.FromIndex(fc.sortByFieldIndex).Name + " ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.HeaderCharacteristics)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.HeaderCharacteristicsGetNameFromIndex(fc.sortByFieldIndex) + " ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.HeaderStatus)
            {
                formattedText += filterUtility.FormatNormal(options, " Header Status ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.HeaderDate)
            {
                formattedText += filterUtility.FormatNormal(options, " Header Date ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.Variables)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.VariablesGetNameFromIndex(fc.sortByFieldIndex) + " ");
            }
            // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            else if (fc.sortByTypeIndex == filterSortByTypes.AssortmentFields)
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterAssortmentFieldTypes.FromIndex(fc.sortByFieldIndex).Name + " ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.AssortmentStatus)
            {
                formattedText += filterUtility.FormatNormal(options, " Status ");
            }
            else if (fc.sortByTypeIndex == filterSortByTypes.AssortmentDate)
            {
                formattedText += filterUtility.FormatNormal(options, " Date ");
            }
            // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            else
            {
                formattedText += filterUtility.FormatNormal(options, " " + filterSortByTypes.FromIndex(fc.sortByTypeIndex).Name + " ");
            }

            formattedText += filterUtility.FormatOperator(options, " (" + filterSortByDirectionTypes.FromIndex(fc.operatorIndex).Name + ")");

        }

    }

}
