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
    public partial class filterElementInfo : UserControl, IFilterElement
    {
        public filterElementInfo()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.ultraLabel1.Text = groupSettings.infoInstructions;
        }
        public void ClearControls()
        {
        }

        public void LoadFromCondition(filter f, filterCondition condition)
        {

        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            //condition.valueToCompare = txtValueToCompare.Text;
        }


    }
}
