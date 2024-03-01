using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    /// <summary>
    /// Base class for UI elements
    /// </summary>
    public class elementBase
    {
        public bool isLoading;
        public filterManager manager;
        public IFilterElement elementInterface;
        public filterElementMap elementMap;
        public string groupHeading;
        public bool isField;
        public bool isVariable;
        public bool isSortBy;
        public bool isOperatorNumeric;
        public bool isOperatorDate;
        public bool isList;
        public bool loadFromVariable1 = false;
        public bool loadFromVariable2 = false;
        public bool loadFromHeaderMerchandise = false;
        public bool useDynamicOperator = false;
		// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
		public bool isOperatorCalendarDate;
		public bool loadFromCalendarDate = false;
        public bool RestrictToSingleDate = false; //MERCH-4765
        public bool RestrictToOnlyWeeks = false;
        public bool AllowDynamic = true;
        public bool AllowDynamicToPlan = true;
        public bool AllowDynamicToStoreOpen = true;
        public bool AllowTimeSensitiveDateCheck = true;
        public bool SpecifyWeeks = false;
		// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        public filterDataTypes dataType;
        public List<eProfileType> dragDropTypesAllowed = new List<eProfileType>();


        public elementBase(filterManager manager, filterElementMap elementMap, string groupHeading, bool isList = false)
        {
            this.manager = manager;
            this.elementMap = elementMap;
            this.groupHeading = groupHeading;
            this.isList = isList;
        }

        public void LoadFromCondition(filterCondition condition)
        {
            isLoading = true;
            elementInterface.LoadFromCondition(manager.currentFilter, condition);
            isLoading = false;
        }
        public bool IsValid(filterCondition condition)
        {
            return elementInterface.IsValid(manager.currentFilter, condition);          
        }
        public void SaveToCondition(ref filterCondition condition)
        {
            elementInterface.SaveToCondition(ref manager.currentFilter, ref condition);
        }
        public void SaveDataTypeToCondition(ref filterCondition condition, filterDataTypes dataType)
        {
            if (dataType.valueType == filterValueTypes.Text)
            {
            }
            else if (dataType.valueType == filterValueTypes.Boolean)
            {
            }
            else if (dataType.valueType == filterValueTypes.List)
            {
            }
            else if (dataType.valueType == filterValueTypes.Date)
            {
                condition.dateTypeIndex = dataType.dateType.Index;
            }
            else if (dataType.valueType == filterValueTypes.Numeric || dataType.valueType == filterValueTypes.Dollar)
            {
                condition.numericTypeIndex = dataType.numericType.Index;
            }
        }

        public void MakeConditionDirty()
        {
            if (manager != null && isLoading == false)
            {
                manager.MakeConditionDirty();
            }
        }

    }
}
