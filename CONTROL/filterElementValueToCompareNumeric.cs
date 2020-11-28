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
    public partial class filterElementValueToCompareNumeric : UserControl, IFilterElement
    {
        public filterElementValueToCompareNumeric()
        {
            InitializeComponent();
        }

        private elementBase eb;

        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            SharedControlRoutines.SetMaskForNumericEditor(eb.dataType.numericType, this.numericEditor);
            if (eb.manager.readOnly)
            {
                numericEditor.Enabled = false;
            }

            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;
            if (eb.dataType.numericType == filterNumericTypes.Integer)
            {
                if (eb.manager.currentCondition.lastValueToCompareInt != null)
                {
                    this.numericEditor.Value = eb.manager.currentCondition.lastValueToCompareInt;
                }
            }
            else
            {
                if (eb.manager.currentCondition.lastValueToCompareDouble != null)
                {
                    this.numericEditor.Value = eb.manager.currentCondition.lastValueToCompareDouble;
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
            if (eb.dataType.numericType == filterNumericTypes.Integer)
            {
                if (condition.valueToCompareInt != null)
                {
                    this.numericEditor.Value = condition.valueToCompareInt;
                    eb.manager.currentCondition.lastValueToCompareInt = condition.valueToCompareInt;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                }
            }
            else
            {
                if (condition.valueToCompareDouble != null)
                {
                    this.numericEditor.Value = condition.valueToCompareDouble;
                    eb.manager.currentCondition.lastValueToCompareDouble = condition.valueToCompareDouble;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                }
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {

            string val = this.numericEditor.Value.ToString();
            if (val != string.Empty)
            {
                if (eb.dataType.numericType == filterNumericTypes.Integer)
                {
                    int i;
                    int.TryParse(val, out i);
                    condition.valueToCompareInt = i;
                    condition.lastValueToCompareInt = i;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                    condition.valueToCompareDouble = null;
                }
                else
                {
                    double d;
                    double.TryParse(val, out d);
                    condition.valueToCompareDouble = d;
                    condition.lastValueToCompareDouble = d;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                    condition.valueToCompareInt = null;
                }
                //condition.valueToCompare = d.ToString("###,###,###,##.0000");
            }
            else
            {
                condition.valueToCompareDouble = null;
                condition.valueToCompareInt = null;
                //condition.valueToCompare = string.Empty;
            }
        }

        private void numericEditor_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {
                string val = this.numericEditor.Value.ToString();
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

            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
    }
}
