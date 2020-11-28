using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

    public class entryFilterName : filterDictionaryEntry
    {
        public entryFilterName(filterManager manager)
        {
            base.groupSettings.groupTitle = "Name";
            base.elementList.Add(new elementBase(manager, filterElementMap.Name, base.groupSettings.groupTitle));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatValue(options, fc.valueToCompare);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.valueToCompare = "New Filter";
        }
    }

    public class entryFolder : filterDictionaryEntry
    {

        public entryFolder(filterManager manager)
        {
            base.groupSettings.groupTitle = "Folder";
            base.elementList.Add(new elementBase(manager, filterElementMap.Folder, base.groupSettings.groupTitle));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " = ");
            if (fc.valueToCompareInt == Include.GlobalUserRID)
            {
                formattedText += filterUtility.FormatValue(options, "Global");
            }
            else
            {
                formattedText += filterUtility.FormatValue(options, "User");
            }

            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            // fc.valueToCompareInt = -1;

        }
    }

    public class entryResultLimit : filterDictionaryEntry
    {
        public entryResultLimit(filterManager manager)
        {
            base.groupSettings.groupTitle = "Result Limit";
            base.elementList.Add(new elementBase(manager, filterElementMap.Limit, base.groupSettings.groupTitle));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " = ");
            formattedText += filterUtility.FormatValue(options, fc.valueToCompare);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.valueToCompare = "Unrestricted";
        }
    }

    public class entryInfoFilter : filterDictionaryEntry
    {
        public entryInfoFilter(filterManager manager)
        {
            base.groupSettings.groupTitle = "Filter Information";
            base.groupSettings.infoInstructions = "Select a line to below to change filter information.";
            base.elementList.Add(new elementBase(manager, filterElementMap.Info, "Filter Information"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
        }
    }

    public class entryInfoConditions : filterDictionaryEntry
    {
        public entryInfoConditions(filterManager manager)
        {
            base.groupSettings.groupTitle = "Conditions";
            base.groupSettings.infoInstructions = "Add conditions using the toolbar above.";
            base.elementList.Add(new elementBase(manager, filterElementMap.Info, "Conditions"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
        }
    }

    public class entryInfoSortBy : filterDictionaryEntry
    {
        public entryInfoSortBy(filterManager manager)
        {
            base.groupSettings.groupTitle = "Sort By";
            base.groupSettings.infoInstructions = "Add sort items using the toolbar above.";
            base.elementList.Add(new elementBase(manager, filterElementMap.Info, "Sort By"));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
        }
    }

    public class entryUsers : filterDictionaryEntry
    {
        public entryUsers(filterManager manager)
        {
            base.groupSettings.groupTitle = "Users";
            //base.groupSettings.loadValueList = new FilterLoadListDelegate(filterDataHelper.HeaderTypesGetDataTable);
            base.groupSettings.fieldForData = "USER_RID";
            base.groupSettings.fieldForDisplay = "USER_FULLNAME";
            base.groupSettings.listValueType = filterListValueTypes.Users;
            base.elementList.Add(new elementBase(manager, filterElementMap.Logic, "And / Or"));
            base.elementList.Add(new elementBase(manager, filterElementMap.OperatorIn, "In / Not In"));
            base.elementList.Add(new elementBase(manager, filterElementMap.List, base.groupSettings.groupTitle, true));
        }
        public override void BuildFormattedText(filterOptionDefinition options, ref filterCondition fc)
        {
            string formattedText = string.Empty;
            formattedText += filterUtility.FormatLogic(options, filterLogicTypes.FromIndex(fc.logicIndex).Name);
            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
            formattedText += filterUtility.FormatOperator(options, " " + filterListOperatorTypes.FromIndex(fc.operatorIndex).symbol);
            formattedText += filterUtility.FormatValue(options, " (");
            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayListWithParent(fc.listConstantType, fc.GetListValues(filterListValueTypes.Users), filterDataHelper.dtActiveUsersWithGroupRID, base.groupSettings.fieldForData, base.groupSettings.fieldForDisplay));
            formattedText += filterUtility.FormatValue(options, ")");
            fc.NodeFormattedText = formattedText;
        }
        public override void SetDefaults(ref filterCondition fc)
        {
            fc.logicIndex = filterLogicTypes.And;
            fc.operatorIndex = filterListOperatorTypes.Includes;
            fc.listConstantType = filterListConstantTypes.None;
        }
    }
}
