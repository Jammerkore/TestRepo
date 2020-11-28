using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
    public partial class SelectMultipleActionTypesControl : UserControl
    {
        public SelectMultipleActionTypesControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleActionTypesControl_Load(object sender, EventArgs e)
        {
        }

        private DataTable dtActions;
        public void LoadData(SessionAddressBlock SAB)
        {
            // load action types
            //DataTable dtActionTypes = MIDText.GetTextType(eMIDTextType.eAssortmentActionType, eMIDTextOrderBy.TextValue);
           

            //dtActions = MIDText.GetLabelValuesInRange((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
            dtActions = MIDText.GetLabelValuesInRange((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceToVSW); // TT#1334-MD - stodd - Balance to VSW Action
            // end TT#794 - New Size Balance for Wet Seal
            // end TT#843 - new size constraint balance
            // End TT#785 
            DataRow dr;
            Hashtable removeEntry = new Hashtable();
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.ChargeSizeIntransit), eAllocationActionType.ChargeSizeIntransit);
            removeEntry.Add(Convert.ToInt32(eAllocationActionType.DeleteHeader), eAllocationActionType.DeleteHeader);
            if (!SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
            {
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeAllocation), eAllocationActionType.BackoutSizeAllocation);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeIntransit), eAllocationActionType.BackoutSizeIntransit);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeNoSubs), eAllocationActionType.BalanceSizeNoSubs);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithSubs), eAllocationActionType.BalanceSizeWithSubs);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceived), eAllocationActionType.BreakoutSizesAsReceived);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeWithConstraints); // TT#843 - New Size Constraint Balance
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeBilaterally); // TT#794 - New Size Balance for Wet Seal
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
            }
            int codeValue;
            for (int i = dtActions.Rows.Count - 1; i >= 0; i--)
            {
                dr = dtActions.Rows[i];
                codeValue = Convert.ToInt32(dr["TEXT_CODE"]);
                if (removeEntry.Contains(codeValue))
                {
                    dtActions.Rows.Remove(dr);
                }
                else if (!Enum.IsDefined(typeof(eAllocationActionType), (eAllocationActionType)codeValue))
                    dtActions.Rows.Remove(dr);
            }

            //Add Unassigned Action
            DataRow drUnassigned = dtActions.NewRow();
            drUnassigned["TEXT_VALUE"] = "Unassigned";
            drUnassigned["TEXT_CODE"] = 0;

            dtActions.Rows.InsertAt(drUnassigned, 0);


            DataSet dsActionTypes = new DataSet();

            dsActionTypes.Tables.Add(dtActions);
       

            this.midSelectMultiNodeControl1.BindDataSet(dsActionTypes);

           
        }

        public void GetSelectedActions(ref ReportData.AllocationAnalysisEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode n in this.midSelectMultiNodeControl1.ultraTree1.Nodes)
            {
                string s = n.Text;
                bool isChecked;
                if (n.CheckedState == CheckState.Checked)
                {
                    isChecked = true;
                }
                else
                {
                    isChecked = false;
                }


                DataRow[] drFind = dtActions.Select("TEXT_VALUE='" + s + "'");
                if (drFind.Length > 0)
                {
                    eAllocationActionType actionType = (eAllocationActionType)Convert.ToInt32(drFind[0]["TEXT_CODE"], CultureInfo.CurrentUICulture);

                    if (Convert.ToInt32(drFind[0]["TEXT_CODE"], CultureInfo.CurrentUICulture) == 0)
                    {
                        e.includeActionType_ActionUnassigned = isChecked;
                    }
                    if (actionType == eAllocationActionType.ApplyAPI_Workflow)
                    {
                        e.includeActionType_ApplyAPI_Workflow = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BackoutAllocation)
                    {
                        e.includeActionType_BackoutAllocation = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BackoutDetailPackAllocation)
                    {
                        e.includeActionType_BackoutDetailPackAllocation = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BackoutSizeAllocation)
                    {
                        e.includeActionType_BackoutSizeAllocation = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BackoutSizeIntransit)
                    {
                        e.includeActionType_BackoutSizeIntransit = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BackoutStyleIntransit)
                    {
                        e.includeActionType_BackoutStyleIntransit = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceSizeBilaterally)
                    {
                        e.includeActionType_BalanceSizeBilaterally = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceSizeNoSubs)
                    {
                        e.includeActionType_BalanceSizeNoSubs = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceSizeWithConstraints)
                    {
                        e.includeActionType_BalanceSizeWithConstraints = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceSizeWithSubs)
                    {
                        e.includeActionType_BalanceSizeWithSubs = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceStyleProportional)
                    {
                        e.includeActionType_BalanceStyleProportional = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BalanceToDC)
                    {
                        e.includeActionType_BalanceToDC = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BreakoutSizesAsReceived)
                    {
                        e.includeActionType_BreakoutSizesAsReceived = isChecked;
                    }
                    else if (actionType == eAllocationActionType.BreakoutSizesAsReceivedWithConstraints)
                    {
                        e.includeActionType_BreakoutSizesAsReceivedWithConstraints = isChecked;
                    }
                    else if (actionType == eAllocationActionType.ChargeIntransit)
                    {
                        e.includeActionType_ChargeIntransit = isChecked;
                    }
                    else if (actionType == eAllocationActionType.ChargeSizeIntransit)
                    {
                        e.includeActionType_ChargeSizeIntransit = isChecked;
                    }
                    else if (actionType == eAllocationActionType.DeleteHeader)
                    {
                        e.includeActionType_DeleteHeader = isChecked;
                    }
                    else if (actionType == eAllocationActionType.ReapplyTotalAllocation)
                    {
                        e.includeActionType_ReapplyTotalAllocation = isChecked;
                    }
                    else if (actionType == eAllocationActionType.Release)
                    {
                        e.includeActionType_Release = isChecked;
                    }
                    else if (actionType == eAllocationActionType.RemoveAPI_Workflow)
                    {
                        e.includeActionType_RemoveAPI_Workflow = isChecked;
                    }
                    else if (actionType == eAllocationActionType.Reset)
                    {
                        e.includeActionType_Reset = isChecked;
                    }
                    else if (actionType == eAllocationActionType.StyleNeed)
                    {
                        e.includeActionType_StyleNeed = isChecked;
                    }
                    // Begin TT#1334-MD - stodd - Balance to VSW Action
                    else if (actionType == eAllocationActionType.BalanceToVSW)
                    {
                        e.includeActionType_BalanceToVSW = isChecked;
                    }
                    // End TT#1334-MD - stodd - Balance to VSW Action


                }






            }



        }



    }
}
