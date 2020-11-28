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
    public partial class filterElementNameAndLimit : UserControl, IFilterElement
    {
        public filterElementNameAndLimit()
        {
            InitializeComponent();

        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                txtName.Enabled = false;
                //cboLimitType.Enabled = false;
                //resultLimit.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.txtName.Text = condition.valueToCompare;
            //if (f.isLimited)
            //{
            //    this.cboLimitType.Value = "Restricted";
            //    this.resultLimit.Value = f.resultLimit;
            //}
            //else
            //{
            //    this.cboLimitType.Value = "Unrestricted";
            //    this.resultLimit.Value = 5000;
            //}
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            condition.valueToCompare = txtName.Text;
            //if (cboLimitType.Value != null)
            //{
            //    string s = (string)cboLimitType.Value;
            //    if (s == "Restricted")
            //    {
            //        f.isLimited = true;
            //        f.resultLimit = (int)this.resultLimit.Value;
            //        //condition.valueToCompare = f.resultLimit.ToString("###,###,###,##0") + " rows";
            //    }
            //    else
            //    {
            //        f.isLimited = false;
            //        f.resultLimit = -1;
            //        //condition.valueToCompare = "Unrestricted";
            //    }
            //}
        }

        private void txtName_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        //private void cboLimitType_ValueChanged(object sender, EventArgs e)
        //{
        //    eb.MakeConditionDirty();

        //    string s = (string)cboLimitType.Value;
        //    if (s == "Restricted")
        //    {
        //        this.pnlLimit.Visible = true;

        //    }
        //    else
        //    {
        //        this.pnlLimit.Visible = false;
        //    }

        //}



        //private void resultLimit_ValueChanged(object sender, EventArgs e)
        //{
        //    eb.MakeConditionDirty();
        //}
    }
}
