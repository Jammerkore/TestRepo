using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementFolder : UserControl, IFilterElement
    {

        public filterElementFolder()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            if (eb.manager.readOnly || eb.manager.currentFilter.filterType == filterTypes.ProductFilter)
            {
                cboFolder.Enabled = false;
            }
        }
        public void ClearControls()
        {
        }
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            FunctionSecurityProfile _filterUserSecurity = null;
            FunctionSecurityProfile _filterGlobalSecurity = null;

            if (f.filterType == filterTypes.StoreFilter)
            {
                _filterUserSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
                _filterGlobalSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);
            }
            else if (f.filterType == filterTypes.HeaderFilter) //TT#1313-MD -jsobek -Header Filters
            {
                _filterUserSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersHeaderUser);
                _filterGlobalSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersHeaderGlobal);
            }
            else if (f.filterType == filterTypes.AssortmentFilter) //TT#1313-MD -jsobek -Header Filters
            {
                _filterUserSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersAssortmentUser);
                _filterGlobalSecurity = eb.manager.SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersAssortmentGlobal);
            }
            // Begin TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error
            // Begin TT#4674 - JSmith - Unhandled Exception error in Audit Filter
            //else if (f.filterType == filterTypes.ProductFilter)
            else
            // End TT#4674 - JSmith - Unhandled Exception error in Audit Filter
            {
                _filterUserSecurity = new FunctionSecurityProfile(Include.NoRID);
                _filterUserSecurity.SetFullControl();
                _filterGlobalSecurity = new FunctionSecurityProfile(Include.NoRID);
                _filterGlobalSecurity.SetAccessDenied();
            }
            // End TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error

            // Begin TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type
            this.cboFolder.Items.Clear();
            if (!_filterUserSecurity.AccessDenied)
            {
                this.cboFolder.Items.Add(new Infragistics.Win.ValueListItem("User", "User"));
            }
            if (!_filterGlobalSecurity.AccessDenied)
            {
                this.cboFolder.Items.Add(new Infragistics.Win.ValueListItem("Global", "Global"));
            }
            // End TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type

            //condition.valueToCompareInt = f.ownerUserRID;
            //if (f.ownerUserRID == Include.GlobalUserRID)
            if (condition.valueToCompareInt == Include.GlobalUserRID)
            {
                this.cboFolder.Text = "Global";
                if (_filterGlobalSecurity != null && _filterGlobalSecurity.AllowUpdate)
                {
                    //this.cboFolder.Enabled = true;
                    // Begin TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type
                    if (_filterUserSecurity == null || !_filterUserSecurity.AllowUpdate)
                    {
                        this.cboFolder.Enabled = false;
                    }
                    // End TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type
                }
                else
                {
                    this.cboFolder.Enabled = false;
                }
            }
            else
            {
                this.cboFolder.Text = "User";
                if (_filterUserSecurity != null && _filterUserSecurity.AllowUpdate)
                {
                    //this.cboFolder.Enabled = true;   
                    // Begin TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type
                    if (_filterGlobalSecurity == null || !_filterGlobalSecurity.AllowUpdate)
                    {
                        this.cboFolder.Enabled = false;
                    }
                    // End TT#4563 - JSmith - Can save filter as User or Global if not authorized for filter type
                }
                else
                {
                    this.cboFolder.Enabled = false;
                }
            }

        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            //We need to save to the condition valueToCompareInt so we can build the formatted text
            if (this.cboFolder.Text == "Global")
            {
                f.ownerUserRID = Include.GlobalUserRID;
            }
            else
            {
                f.ownerUserRID = eb.manager.SAB.ClientServerSession.UserRID; //f.userRID;
            }
            condition.valueToCompareInt = f.ownerUserRID;
        }



        private void cboFolder_TextChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }

        private void chkIsTemplate_CheckedChanged(object sender, EventArgs e)
        {
            eb.MakeConditionDirty();
        }
    }
}
