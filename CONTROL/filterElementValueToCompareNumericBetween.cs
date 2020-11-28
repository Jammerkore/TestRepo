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
    public partial class filterElementValueToCompareNumericBetween : UserControl, IFilterElement
    {
        public filterElementValueToCompareNumericBetween()
        {
            InitializeComponent();
        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            SharedControlRoutines.SetMaskForNumericEditor(eb.dataType.numericType, this.numericEditorFrom);
            SharedControlRoutines.SetMaskForNumericEditor(eb.dataType.numericType, this.numericEditorTo);
            if (eb.manager.readOnly)
            {
                numericEditorFrom.Enabled = false;
                numericEditorTo.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;
            if (eb.dataType.numericType == filterNumericTypes.Integer)
            {
                if (eb.manager.currentCondition.lastValueToCompareInt != null)
                {
                    this.numericEditorFrom.Value = eb.manager.currentCondition.lastValueToCompareInt;
                }
                if (eb.manager.currentCondition.lastValueToCompareInt2 != null)
                {
                    this.numericEditorTo.Value = eb.manager.currentCondition.lastValueToCompareInt2;
                }
            }
            else
            {
                if (eb.manager.currentCondition.lastValueToCompareDouble != null)
                {
                    this.numericEditorFrom.Value = eb.manager.currentCondition.lastValueToCompareDouble;
                }
                if (eb.manager.currentCondition.lastValueToCompareDouble2 != null)
                {
                    this.numericEditorTo.Value = eb.manager.currentCondition.lastValueToCompareDouble2;
                }
            }
            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            //Begin TT#1509-MD -jsobek -Store Filters - Fix between operator values not being reloaded
            //if (condition.valueToCompareDouble != null)
            //{
            //    this.numericEditorFrom.Value = condition.valueToCompareDouble;
            //}
            //if (condition.valueToCompareDouble2 != null)
            //{
            //    this.numericEditorTo.Value = condition.valueToCompareDouble2;
            //}
            if (eb.dataType.numericType == filterNumericTypes.Integer)
            {
                if (condition.valueToCompareInt != null)
                {
                    this.numericEditorFrom.Value = condition.valueToCompareInt;
                }
                if (condition.valueToCompareInt2 != null)
                {
                    this.numericEditorTo.Value = condition.valueToCompareInt2;
                }
            }
            else
            {
                if (condition.valueToCompareDouble != null)
                {
                    this.numericEditorFrom.Value = condition.valueToCompareDouble;
                }
                if (condition.valueToCompareDouble2 != null)
                {
                    this.numericEditorTo.Value = condition.valueToCompareDouble2;
                }
            }
            //End TT#1509-MD -jsobek -Store Filters - Fix between operator values not being reloaded
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {

            string val = this.numericEditorFrom.Value.ToString();
            if (val != string.Empty)
            {
                if (eb.dataType.numericType == filterNumericTypes.Integer)
                {
                    int i;
                    int.TryParse(val, out i);
                    condition.valueToCompareInt = i;
                    condition.valueToCompareDouble = null;
                    condition.valueToCompareDouble2 = null;
                }
                else
                {
                    double d;
                    double.TryParse(val, out d);
                    condition.valueToCompareDouble = d;
                    condition.valueToCompareInt = null;
                    condition.valueToCompareInt2 = null;
                }

                string val2 = this.numericEditorTo.Value.ToString();
                if (eb.dataType.numericType == filterNumericTypes.Integer)
                {
                    int i2;
                    int.TryParse(val2, out i2);
                    condition.valueToCompareInt2 = i2;
                }
                else
                {
                    double d2;
                    double.TryParse(val2, out d2);
                    condition.valueToCompareDouble2 = d2;
                }

                //condition.valueToCompare = d.ToString("###,###,###,##.0000") + " to " + d2.ToString("###,###,###,##.0000");
            }
            else
            {
                condition.valueToCompareDouble = null;
                condition.valueToCompareDouble2 = null;
                condition.valueToCompareInt = null;
                condition.valueToCompareInt2 = null;
            }
        }

        private void numericEditor_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                string val = this.numericEditorFrom.Value.ToString();
                if (val != string.Empty)
                {
                    if (eb.dataType.numericType == filterNumericTypes.Integer)
                    {
                        int i;
                        if (int.TryParse(val, out i))
                        {
                            eb.manager.currentCondition.lastValueToCompareInt = i;
                        }
                        else
                        {
                            eb.manager.currentCondition.lastValueToCompareInt = null;
                        }
                    }
                    else
                    {
                        double d;
                        if (double.TryParse(val, out d))
                        {
                            eb.manager.currentCondition.lastValueToCompareDouble = d;
                        }
                        else
                        {
                            eb.manager.currentCondition.lastValueToCompareDouble = null;
                        }
                    }
                }
                else
                {
                    if (eb.dataType.numericType == filterNumericTypes.Integer)
                    {
                        eb.manager.currentCondition.lastValueToCompareInt = null;
                    }
                    else
                    {
                        eb.manager.currentCondition.lastValueToCompareDouble = null;
                    }
                }


                string val2 = this.numericEditorTo.Value.ToString();
                if (val2 != string.Empty)
                {
                    if (eb.dataType.numericType == filterNumericTypes.Integer)
                    {
                        int i;
                        if (int.TryParse(val2, out i))
                        {
                            eb.manager.currentCondition.lastValueToCompareInt2 = i;
                        }
                        else
                        {
                            eb.manager.currentCondition.lastValueToCompareInt2 = null;
                        }
                    }
                    else
                    {
                        double d;
                        if (double.TryParse(val2, out d))
                        {
                            eb.manager.currentCondition.lastValueToCompareDouble2 = d;
                        }
                        else
                        {
                            eb.manager.currentCondition.lastValueToCompareDouble2 = null;
                        }
                    }
                }
                else
                {
                    if (eb.dataType.numericType == filterNumericTypes.Integer)
                    {
                        eb.manager.currentCondition.lastValueToCompareInt2 = null;
                    }
                    else
                    {
                        eb.manager.currentCondition.lastValueToCompareDouble2 = null;
                    }
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
    }
}
