using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementSortByType : UserControl, IFilterElement
    {
        public filterElementSortByType()
        {
            InitializeComponent();
        }

        private elementBase eb;
        private filterEntrySettings groupSettings;
        //delegates to access the container
        public FilterMakeElementInGroupDelegate makeElementInGroupDelegate;
        public FilterRemoveDynamicElementsForFieldDelegate removeDynamicElementsForFieldDelegate;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            BindComboBox();
            if (eb.manager.readOnly)
            {
                cboSortByType.Enabled = false;
            }
        }



        public void ClearControls()
        {
            //clear newly adding groups from the list and from the container
            if (this.removeDynamicElementsForFieldDelegate != null)
            {
                this.removeDynamicElementsForFieldDelegate(keyListToRemove);
            }
        }
        private void BindComboBox()
        {
            this.cboSortByType.DataSource = filterSortByTypes.ToDataTable(eb.manager.currentFilter.filterType).Copy();
            //this.cboSortByType.DataSource = groupSettings.valueSettings.loadValueList().Copy();
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboSortByType.Value = condition.sortByTypeIndex;
            foreach (elementBase b in fieldElementList)
            {
                b.LoadFromCondition(condition);
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            condition.sortByTypeIndex = (int)cboSortByType.Value;
            foreach (elementBase b in fieldElementList)
            {
                b.SaveToCondition(ref condition);
            }
        }

        private void cboSortByType_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();

            if (cboSortByType.Value != null)
            { 
                int opIndex = (int)cboSortByType.Value;
                Handle_OpTypeChanged(filterSortByTypes.FromIndex(opIndex));
            }
        }

        private filterSortByTypes currentSortByType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_OpTypeChanged(filterSortByTypes newOpType)
        {
            if (currentSortByType == null || newOpType != currentSortByType)
            {
                MakeGroupsForOpType(newOpType);
                currentSortByType = newOpType;
            }
        }
        private void MakeGroupsForOpType(filterSortByTypes opType)
        {
            ClearControls();

            fieldElementList.Clear();
            //add new groups based on type
            if (opType == filterSortByTypes.StoreCharacteristics)
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");

                groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.StoreCharacteristicsGetDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetDataType);
                groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetNameFromIndex);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.StoreCharacteristicRID;
                fieldElementList.Add(b);
            }
            

            else if (opType == filterSortByTypes.StoreFields)
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");

                groupSettings.loadFieldList = new FilterLoadListDelegate(filterStoreFieldTypes.ToDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterStoreFieldTypes.GetValueTypeInfoForField);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.ProductSearchFields) //TT#1388-MD -jsobek -Product Filters
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");

                groupSettings.loadFieldList = new FilterLoadListDelegate(filterProductFieldTypes.ToDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterProductFieldTypes.GetValueTypeInfoForField);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.AuditSearchFields) //TT#1443-MD -jsobek -Audit Filter
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");

                groupSettings.loadFieldList = new FilterLoadListDelegate(filterAuditSearchTypes.ToDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterAuditSearchTypes.GetValueTypeForField);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.Variables)
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");
                groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.VariablesGetDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetDataType);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.HeaderFields) //TT#1468-MD -jsobek -Header Filter Sort Options
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");
                groupSettings.loadFieldList = new FilterLoadListDelegate(filterHeaderFieldTypes.ToDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterHeaderFieldTypes.GetValueTypeForField);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.HeaderCharacteristics) //TT#1468-MD -jsobek -Header Filter Sort Options
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");
                groupSettings.loadFieldList = new FilterLoadListDelegate(filterDataHelper.HeaderCharacteristicsGetDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetDataType);
                groupSettings.GetNameFromField = new FilterGetNameFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetNameFromIndex);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.HeaderCharacteristicRID;
                fieldElementList.Add(b);
            }
            else if (opType == filterSortByTypes.HeaderStatus) //TT#1468-MD -jsobek -Header Filter Sort Options
            {
                //no need to add a list
            }
            else if (opType == filterSortByTypes.HeaderDate) //TT#1468-MD -jsobek -Header Filter Sort Options
            {
                //no need to add a list
            }
            // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            else if (opType == filterSortByTypes.AssortmentFields) 
            {
                elementBase b = new elementBase(eb.manager, filterElementMap.SortBy, "Sort By");
                groupSettings.loadFieldList = new FilterLoadListDelegate(filterAssortmentFieldTypes.ToDataTable);
                groupSettings.GetDataTypeFromFieldIndex = new FilterGetDataTypeFromFieldIndexDelegate(filterAssortmentFieldTypes.GetValueTypeForField);
                groupSettings.fieldForData = "FIELD_INDEX";
                groupSettings.fieldForDisplay = "FIELD_NAME";

                groupSettings.listValueType = filterListValueTypes.None;
                fieldElementList.Add(b);
            }
            // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

            elementBase bDirection = new elementBase(eb.manager, filterElementMap.SortByDirection, "Direction");
            fieldElementList.Add(bDirection);

            keyListToRemove.Clear();
            int index = 0;
            foreach (elementBase b in fieldElementList)
            {
                string key = "se" + index.ToString();
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                }
                keyListToRemove.Add(key);
                index++;
            }
        }

        private void cboSortByType_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboSortByType, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
