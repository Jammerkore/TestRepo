using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementCalendar : UserControl, IFilterElement
    {
        public filterElementCalendar()
        {
            InitializeComponent();
            mdsDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
        }
        private void filterElementCalendar_Load(object sender, EventArgs e)
        {
           
            //string _graphicsDir = MIDGraphics.MIDGraphicsDir;
            //// End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //_dynamicToPlanImage = Image.FromFile(_graphicsDir + "\\" + MIDGraphics.DynamicToPlanImage);
            //_dynamicToCurrentImage = Image.FromFile(_graphicsDir + "\\" + MIDGraphics.DynamicToCurrentImage);
            
            this.mdsDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsDateRange_OnSelection);
         

        }
        //private Image _dynamicToPlanImage = null;
        //private Image _dynamicToCurrentImage = null;

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly)
            {
                mdsDateRange.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            if (this.useVariable1)
            {
                if (eb.manager.currentCondition.lastVariable1_CDR_RID != null)
                {
                    mdsDateRange.DateRangeRID = (int)eb.manager.currentCondition.lastVariable1_CDR_RID;
                    if (mdsDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drp = eb.manager.SAB.ApplicationServerSession.Calendar.GetDateRange(mdsDateRange.DateRangeRID, eb.manager.SAB.ClientServerSession.Calendar.CurrentDate);
                        mdsDateRange.Text = drp.DisplayDate;
                        SetImagesForDateRange(drp);
                    }
                }
            }
            else if (useVariable2)
            {
                if (eb.manager.currentCondition.lastVariable2_CDR_RID != null)
                {
                    mdsDateRange.DateRangeRID = (int)eb.manager.currentCondition.lastVariable2_CDR_RID;
                    if (mdsDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drp = eb.manager.SAB.ApplicationServerSession.Calendar.GetDateRange(mdsDateRange.DateRangeRID, eb.manager.SAB.ClientServerSession.Calendar.CurrentDate);
                        mdsDateRange.Text = drp.DisplayDate;
                        SetImagesForDateRange(drp);
                    }
                }
            }

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        private bool useVariable1 = false;
        private bool useVariable2 = false;
   
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            
         
            if (useVariable1)
            {
                //Begin TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
                int cdrRID = condition.variable1_CDR_RID;
                if (cdrRID == -1)
                {
                    cdrRID = Include.UndefinedCalendarDateRange;
                }
                mdsDateRange.DateRangeRID = cdrRID;
                eb.manager.currentCondition.lastVariable1_CDR_RID = cdrRID;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                //End TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
            }
            else if (useVariable2)
            {
                //Begin TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
                int cdrRID = condition.variable2_CDR_RID;
                if (cdrRID == -1)
                {
                    cdrRID = Include.UndefinedCalendarDateRange;
                }
                mdsDateRange.DateRangeRID = cdrRID;
                eb.manager.currentCondition.lastVariable2_CDR_RID = cdrRID;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                //End TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
            }
            if (mdsDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
            {
                DateRangeProfile drp = eb.manager.SAB.ApplicationServerSession.Calendar.GetDateRange(mdsDateRange.DateRangeRID, eb.manager.SAB.ClientServerSession.Calendar.CurrentDate);
                mdsDateRange.Text = drp.DisplayDate;
                SetImagesForDateRange(drp);
            }
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {  
            if (useVariable1)
            {
                condition.variable1_CDR_RID = mdsDateRange.DateRangeRID;
            }
            else if (useVariable2)
            {
                condition.variable2_CDR_RID = mdsDateRange.DateRangeRID;
            }
            //if (var2Control.DateRangeSelector.DateRangeRID != Include.UndefinedCalendarDateRange)
            //{
            //    // BEGIN MID Track #2495 - FilterWizard not saving secondary variable dates
            //    //										varOperand.DateRangeProfile = (DateRangeProfile)var2Control.DateRangeSelector.Tag;
            //    varOperand.DateRangeProfile = _SAB.ClientServerSession.Calendar.GetDateRange(var2Control.DateRangeSelector.DateRangeRID, _SAB.ClientServerSession.Calendar.CurrentDate);
            //    // END MID Track #2495 - FilterWizard not saving secondary variable dates
            //}
        }

        private void txtMerchandise_ValueChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }
       
        private void mdsDateRange_Click(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
            try
            {
                mdsDateRange.Enabled = true;
                //mdsDateRange.DateRangeForm = _dateSel;
                //_dateSel.DateRangeRID = mdsDateRange.DateRangeRID;
                //_dateSel.AnchorDate = _SAB.ClientServerSession.Calendar.CurrentDate;
                //_dateSel.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
                //_dateSel.AllowDynamicToStoreOpen = true;
                mdsDateRange.DateRangeForm = eb.manager.calendarDateSelectorForm; 
                eb.manager.setCalendarDateRangeDelegate(mdsDateRange.DateRangeRID);
             
                
                mdsDateRange.ShowSelector();

                //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
                if (eb.isLoading == false)
                {
                    if (this.useVariable1)
                    {
                        eb.manager.currentCondition.lastVariable1_CDR_RID = mdsDateRange.DateRangeRID;
                    }
                    else if (useVariable2)
                    {
                        eb.manager.currentCondition.lastVariable2_CDR_RID = mdsDateRange.DateRangeRID;
                    }
                }
                //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
                MessageBox.Show(exc.ToString());
            }
        }
        private void mdsDateRange_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
        {
            try
            {
                if (e.SelectedDateRange != null)
                {
                    mdsDateRange.DateRangeRID = e.SelectedDateRange.Key;
                    SetImagesForDateRange(e.SelectedDateRange);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
                MessageBox.Show(exc.ToString());
            }
        }
        private void SetImagesForDateRange(DateRangeProfile drp)
        {
            //Begin TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
            if (drp.DateRangeType == eCalendarRangeType.Dynamic)
            {
                switch (drp.RelativeTo)
                {
                    case eDateRangeRelativeTo.Current:
                        Image dynamicToCurrentImage = this.imageList1.Images[1];
                        mdsDateRange.SetImage(dynamicToCurrentImage);
                        break;
                    case eDateRangeRelativeTo.Plan:
                        Image dynamicToPlanImage = this.imageList1.Images[0];
                        mdsDateRange.SetImage(dynamicToPlanImage);
                        break;
                    default:
                        mdsDateRange.SetImage(null);
                        break;
                }
            }
            //End TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
        }
      
    }
}
