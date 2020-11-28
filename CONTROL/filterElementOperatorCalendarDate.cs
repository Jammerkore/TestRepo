using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementOperatorCalendarDate : UserControl, IFilterElement
    {
        public filterElementOperatorCalendarDate()
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

        public void SetDefault()
        {
           
            eb.isLoading = true;

            if (eb.manager.currentCondition.lastOperatorIndexDate != null)
            {
                this.cboOperators.Value = (int)eb.manager.currentCondition.lastOperatorIndexDate;
            }

            eb.isLoading = false;
            
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
            this.ultraLabel1.Text = eb.groupHeading;
            this.cboOperators.DataSource = filterCalendarDateOperatorTypes.ToDataTable().Copy();
            eb.isLoading = true;
            this.cboOperators.Value = filterCalendarDateOperatorTypes.Unrestricted.dbIndex;
            eb.isLoading = false;
        }
		
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.cboOperators.Value = condition.operatorIndex;
            eb.manager.currentCondition.lastOperatorIndexDate = condition.operatorIndex; 
            if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Last1Week)
            {
            }
            else if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Next1Week)
            {
            }
            if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Next4Weeks)
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
            //DateRangeProfile drp = eb.manager.SAB.ClientServerSession.Calendar.GetDateRange((int)condition.lastdate_CDR_RID);
            //int opIndex = (int)cboOperators.Value;
            //filterCalendarDateOperatorTypes opType = filterCalendarDateOperatorTypes.FromIndex(opIndex);
            //if (opType == filterCalendarDateOperatorTypes.Between
            //    || opType == filterCalendarDateOperatorTypes.Unrestricted
            //    || opType == filterCalendarDateOperatorTypes.Last1Week
            //    || opType == filterCalendarDateOperatorTypes.Next1Week
            //    || opType == filterCalendarDateOperatorTypes.Next4Weeks
            //    )
            //{
            //    return true;
            //}
            //else
            //{
            //    if (drp.StartDateKey != drp.EndDateKey)
            //    {
            //        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_OnlyOneWeekForOperator), f.filterName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return false;
            //    }
            //}
			
			foreach (elementBase b in fieldElementList)
            {
                if (!b.elementInterface.IsValid(f, condition)) 
                {
                    return false;
                }
            }

            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            eb.SaveDataTypeToCondition(ref condition, eb.dataType);
            condition.operatorIndex = (int)cboOperators.Value;
            if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Last1Week)
            {
                DateTime dateFrom;
                DateTime dateTo;

                dateTo = DateTime.Now;
                dateFrom = dateTo.AddDays(-7);

                condition.valueToCompareDateTo = dateTo;
                condition.valueToCompareDateFrom = dateFrom;
            }
            if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Next1Week)
            {
                DateTime dateFrom;
                DateTime dateTo;

                //go from midnight to midnight
                dateTo = DateTime.Today;
                dateFrom = dateTo.AddDays(7);

                condition.valueToCompareDateTo = dateTo;
                condition.valueToCompareDateFrom = dateFrom;
            }
            if (filterCalendarDateOperatorTypes.FromIndex(condition.operatorIndex) == filterCalendarDateOperatorTypes.Next4Weeks)
            {
                DateTime dateFrom;
                DateTime dateTo;

                //go from midnight to midnight
                dateTo = DateTime.Today;
                dateFrom = dateTo.AddDays(28);

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

                if (eb.isLoading == false)
                {
                    eb.manager.currentCondition.lastOperatorIndexDate = opIndex;
                }
                
                Handle_OpTypeChanged(filterCalendarDateOperatorTypes.FromIndex(opIndex));
            }
        }

        private filterCalendarDateOperatorTypes currentOpType = null;
        private List<elementBase> fieldElementList = new List<elementBase>();
        private List<string> keyListToRemove = new List<string>();
        private void Handle_OpTypeChanged(filterCalendarDateOperatorTypes newOpType)
        {
            if (currentOpType == null || newOpType != currentOpType)
            {
                MakeGroupsForOpType(newOpType);
                currentOpType = newOpType;
            }
        }
        private void MakeGroupsForOpType(filterCalendarDateOperatorTypes opType)
        {
            ClearControls();

            fieldElementList.Clear();

            //add new groups based on type
            if (opType == filterCalendarDateOperatorTypes.Between)
            {
                //add between value element
                elementBase b = new elementBase(eb.manager, filterElementMap.ValueToCompareDateBetween, "Date Value");
                b.dataType = eb.dataType;
                b.AllowTimeSensitiveDateCheck = false;
                b.SpecifyWeeks = true;
                fieldElementList.Add(b);
            }
            else if (opType == filterCalendarDateOperatorTypes.Specify)
            {
                //add specify value element
                elementBase b = new elementBase(eb.manager, filterElementMap.Calendar, "Date");
                b.dataType = eb.dataType;
                b.loadFromCalendarDate = true;
                b.RestrictToOnlyWeeks = true;
                b.AllowDynamic = false;
                b.AllowDynamicToPlan = false;
                b.AllowDynamicToStoreOpen = false;
                b.RestrictToSingleDate = false;
                fieldElementList.Add(b);
            }

            //else
            //{
            //    //add single value element
            //    elementBase b = new elementBase(eb.manager, filterElementMap.Calendar, "Date");
            //    b.dataType = eb.dataType;
            //    b.loadFromCalendarDate = true;
            //    b.RestrictToOnlyWeeks = true;
            //    b.AllowDynamic = false;
            //    b.AllowDynamicToPlan = false;
            //    b.AllowDynamicToStoreOpen = false;
            //    b.RestrictToSingleDate = true;
            //    fieldElementList.Add(b);

            //}

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
