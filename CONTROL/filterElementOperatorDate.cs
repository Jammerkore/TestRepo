using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementOperatorDate : UserControl, IFilterElement
    {
        public filterElementOperatorDate()
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
                cboOperators.Enabled = false;
            }
          
        }
        //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        public void SetDefault()
        {
           
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastOperatorIndexDate != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexDate;
            }

            eb.isLoading = false;
            
        }
        //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
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
            this.ultraLabel1.Text = eb.groupHeading;
            this.cboOperators.DataSource = filterDateOperatorTypes.ToDataTable().Copy();
            eb.isLoading = true;
            this.cboOperators.Value = filterDateOperatorTypes.Unrestricted.dbIndex;
            eb.isLoading = false;
        }

        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboOperators.Value = condition.operatorIndex;
            eb.manager.currentCondition.lastOperatorIndexDate = condition.operatorIndex;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (filterDateOperatorTypes.FromIndex(condition.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
            }
            if (filterDateOperatorTypes.FromIndex(condition.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
            }
            else
            {
                foreach (elementBase b in fieldElementList)
                {
                    b.LoadFromCondition(condition);
                }
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        { 
            // Begin TT#1407-MD - RMatelic - Header Filter Allows To Date Less Than From Date
            foreach (elementBase b in fieldElementList)
            {
                if (!b.elementInterface.IsValid(f, condition)) 
                {
                    return false;
                }
            }
            // End TT#1407-MD
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            eb.SaveDataTypeToCondition(ref condition, eb.dataType);
            condition.operatorIndex = (int)cboOperators.Value;
            if (filterDateOperatorTypes.FromIndex(condition.operatorIndex) == filterDateOperatorTypes.Last24Hours)
            {
                DateTime dateFrom;
                DateTime dateTo;

                dateTo = DateTime.Now;
                dateFrom = dateTo.AddHours(-24);

                condition.valueToCompareDateTo = dateTo;
                condition.valueToCompareDateFrom = dateFrom;
            }
            if (filterDateOperatorTypes.FromIndex(condition.operatorIndex) == filterDateOperatorTypes.Last7Days)
            {
                DateTime dateFrom;
                DateTime dateTo;

                //go from midnight to midnight
                dateTo = DateTime.Today;
                dateFrom = dateTo.AddDays(-7);

                condition.valueToCompareDateTo = dateTo;
                condition.valueToCompareDateFrom = dateFrom;
            }
            else
            {
                foreach (elementBase b in fieldElementList)
                {
                    b.SaveToCondition(ref condition);
                }
            }
        }

        private void cboOperators_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();

            if (cboOperators.Value != null)
            {
                int opIndex = (int)cboOperators.Value;
                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    eb.manager.currentCondition.lastOperatorIndexDate = opIndex;
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                Handle_OpTypeChanged(filterDateOperatorTypes.FromIndex(opIndex));
            }
        }

        private filterDateOperatorTypes currentOpType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_OpTypeChanged(filterDateOperatorTypes newOpType)
        {
            if (currentOpType == null || newOpType != currentOpType)
            {
                MakeGroupsForOpType(newOpType);
                currentOpType = newOpType;
            }
        }
        private void MakeGroupsForOpType(filterDateOperatorTypes opType)
        {
            ClearControls();

            fieldElementList.Clear();
            //add new groups based on type
            if (opType == filterDateOperatorTypes.Between)
            {
                //add between value element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareDateBetween, "Date Value");
                b.dataType = eb.dataType;
                fieldElementList.Add(b);
            }
            else if (opType == filterDateOperatorTypes.Specify)
            {
                //add specify value element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareDateSpecify, "Date Value");
                b.dataType = eb.dataType;
                fieldElementList.Add(b);
            }

            keyListToRemove.Clear();
            int index = 0;
            foreach (elementBase b in fieldElementList)
            {
                string key = "de" + index.ToString();
                if (makeElementInGroupDelegate != null)
                {
                    makeElementInGroupDelegate(key, b, false, -1);
                }
                keyListToRemove.Add(key);
                index++;
            }
        }

        private void cboOperators_Paint(object sender, PaintEventArgs e)
        {
            SharedControlRoutines.EnsureComboBoxDropDownMinWidth(cboOperators, SharedControlRoutines.FilterComboBoxDropDownMinWidth);
        }
    }
}
