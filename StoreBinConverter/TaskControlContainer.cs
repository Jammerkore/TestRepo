using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StoreBinConverter
{
    public partial class TaskControlContainer : UserControl
    {
        public TaskControlContainer()
        {
            InitializeComponent();

            SetVisibleSteps(0);
        }

        public void SetVisibleSteps(int numVisibleSteps)
        {
            if (numVisibleSteps >= 1)  this.stepDailyInsertTable0.Visible = true; else this.stepDailyInsertTable0.Visible = false;
            if (numVisibleSteps >= 2)  this.stepDailyInsertTable1.Visible = true; else this.stepDailyInsertTable1.Visible = false;
            if (numVisibleSteps >= 3)  this.stepDailyInsertTable2.Visible = true; else this.stepDailyInsertTable2.Visible = false;
            if (numVisibleSteps >= 4)  this.stepDailyInsertTable3.Visible = true; else this.stepDailyInsertTable3.Visible = false;
            if (numVisibleSteps >= 5)  this.stepDailyInsertTable4.Visible = true; else this.stepDailyInsertTable4.Visible = false;
            if (numVisibleSteps >= 6)  this.stepDailyInsertTable5.Visible = true; else this.stepDailyInsertTable5.Visible = false;
            if (numVisibleSteps >= 7)  this.stepDailyInsertTable6.Visible = true; else this.stepDailyInsertTable6.Visible = false;
            if (numVisibleSteps >= 8)  this.stepDailyInsertTable7.Visible = true; else this.stepDailyInsertTable7.Visible = false;
            if (numVisibleSteps >= 9)  this.stepDailyInsertTable8.Visible = true; else this.stepDailyInsertTable8.Visible = false;
            if (numVisibleSteps >= 10) this.stepDailyInsertTable9.Visible = true; else this.stepDailyInsertTable9.Visible = false;

            if (numVisibleSteps >= 1)  this.stepDailyUpsertTable0.Visible = true; else this.stepDailyUpsertTable0.Visible = false;
            if (numVisibleSteps >= 2)  this.stepDailyUpsertTable1.Visible = true; else this.stepDailyUpsertTable1.Visible = false;
            if (numVisibleSteps >= 3)  this.stepDailyUpsertTable2.Visible = true; else this.stepDailyUpsertTable2.Visible = false;
            if (numVisibleSteps >= 4)  this.stepDailyUpsertTable3.Visible = true; else this.stepDailyUpsertTable3.Visible = false;
            if (numVisibleSteps >= 5)  this.stepDailyUpsertTable4.Visible = true; else this.stepDailyUpsertTable4.Visible = false;
            if (numVisibleSteps >= 6)  this.stepDailyUpsertTable5.Visible = true; else this.stepDailyUpsertTable5.Visible = false;
            if (numVisibleSteps >= 7)  this.stepDailyUpsertTable6.Visible = true; else this.stepDailyUpsertTable6.Visible = false;
            if (numVisibleSteps >= 8)  this.stepDailyUpsertTable7.Visible = true; else this.stepDailyUpsertTable7.Visible = false;
            if (numVisibleSteps >= 9)  this.stepDailyUpsertTable8.Visible = true; else this.stepDailyUpsertTable8.Visible = false;
            if (numVisibleSteps >= 10) this.stepDailyUpsertTable9.Visible = true; else this.stepDailyUpsertTable9.Visible = false;
                                                     
            if (numVisibleSteps >= 1)  this.stepWeeklyInsertTable0.Visible = true; else this.stepWeeklyInsertTable0.Visible = false;
            if (numVisibleSteps >= 2)  this.stepWeeklyInsertTable1.Visible = true; else this.stepWeeklyInsertTable1.Visible = false;
            if (numVisibleSteps >= 3)  this.stepWeeklyInsertTable2.Visible = true; else this.stepWeeklyInsertTable2.Visible = false;
            if (numVisibleSteps >= 4)  this.stepWeeklyInsertTable3.Visible = true; else this.stepWeeklyInsertTable3.Visible = false;
            if (numVisibleSteps >= 5)  this.stepWeeklyInsertTable4.Visible = true; else this.stepWeeklyInsertTable4.Visible = false;
            if (numVisibleSteps >= 6)  this.stepWeeklyInsertTable5.Visible = true; else this.stepWeeklyInsertTable5.Visible = false;
            if (numVisibleSteps >= 7)  this.stepWeeklyInsertTable6.Visible = true; else this.stepWeeklyInsertTable6.Visible = false;
            if (numVisibleSteps >= 8)  this.stepWeeklyInsertTable7.Visible = true; else this.stepWeeklyInsertTable7.Visible = false;
            if (numVisibleSteps >= 9)  this.stepWeeklyInsertTable8.Visible = true; else this.stepWeeklyInsertTable8.Visible = false;
            if (numVisibleSteps >= 10) this.stepWeeklyInsertTable9.Visible = true; else this.stepWeeklyInsertTable9.Visible = false;
                                                                                           
            if (numVisibleSteps >= 1)  this.stepWeeklyUpsertTable0.Visible = true; else this.stepWeeklyUpsertTable0.Visible = false;
            if (numVisibleSteps >= 2)  this.stepWeeklyUpsertTable1.Visible = true; else this.stepWeeklyUpsertTable1.Visible = false;
            if (numVisibleSteps >= 3)  this.stepWeeklyUpsertTable2.Visible = true; else this.stepWeeklyUpsertTable2.Visible = false;
            if (numVisibleSteps >= 4)  this.stepWeeklyUpsertTable3.Visible = true; else this.stepWeeklyUpsertTable3.Visible = false;
            if (numVisibleSteps >= 5)  this.stepWeeklyUpsertTable4.Visible = true; else this.stepWeeklyUpsertTable4.Visible = false;
            if (numVisibleSteps >= 6)  this.stepWeeklyUpsertTable5.Visible = true; else this.stepWeeklyUpsertTable5.Visible = false;
            if (numVisibleSteps >= 7)  this.stepWeeklyUpsertTable6.Visible = true; else this.stepWeeklyUpsertTable6.Visible = false;
            if (numVisibleSteps >= 8)  this.stepWeeklyUpsertTable7.Visible = true; else this.stepWeeklyUpsertTable7.Visible = false;
            if (numVisibleSteps >= 9)  this.stepWeeklyUpsertTable8.Visible = true; else this.stepWeeklyUpsertTable8.Visible = false;
            if (numVisibleSteps >= 10) this.stepWeeklyUpsertTable9.Visible = true; else this.stepWeeklyUpsertTable9.Visible = false;
        }

        public void SetCheckBoxes(bool enable)
        {
            this.ctlTaskVSW_InsertWork.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskVSW_DropBins.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskVSW_Upsert.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskVSW_DropWork.ultraCheckEditor1.Enabled = enable;

            this.ctlTaskStoreWeeklyBins_InsertWork.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreWeeklyBins_DropBins.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreWeeklyBins_Upsert.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreWeeklyBins_DropWork.ultraCheckEditor1.Enabled = enable;

            this.ctlTaskStoreDayBins_InsertWork.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreDayBins_DropBins.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreDayBins_Upsert.ultraCheckEditor1.Enabled = enable;
            this.ctlTaskStoreDayBins_DropWork.ultraCheckEditor1.Enabled = enable;
        }

        public bool IsOneOrMoreTasksChecked()
        {
            if (this.ctlTaskStoreDayBins_InsertWork.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreDayBins_DropBins.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreDayBins_Upsert.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreDayBins_DropWork.ultraCheckEditor1.Checked == false

                && this.ctlTaskStoreWeeklyBins_InsertWork.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreWeeklyBins_DropBins.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreWeeklyBins_Upsert.ultraCheckEditor1.Checked == false
                && this.ctlTaskStoreWeeklyBins_DropWork.ultraCheckEditor1.Checked == false

                && this.ctlTaskVSW_InsertWork.ultraCheckEditor1.Checked == false
                && this.ctlTaskVSW_DropBins.ultraCheckEditor1.Checked == false
                && this.ctlTaskVSW_Upsert.ultraCheckEditor1.Checked == false
                && this.ctlTaskVSW_DropWork.ultraCheckEditor1.Checked == false

               )
            {
                MessageBox.Show("Please select one or more tasks to process.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
