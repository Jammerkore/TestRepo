using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

using Infragistics.Win.UltraWinTree;

namespace MIDRetail.Windows.Controls
{
    public partial class SelectMultipleUsersAndGroupsControl : UserControl
    {
        public SelectMultipleUsersAndGroupsControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleUsersAndGroupsControl_Load(object sender, EventArgs e)
        {
           // this.midSelectMultiNodeControl1.Title = "Select Users:";
        }

        private DataTable dtActiveUsersWithGroupRID;
        public void LoadData()
        {
            SecurityAdmin secAdmin = new SecurityAdmin();

            DataTable dtActiveGroups = secAdmin.GetActiveGroupsNameFirst();
            dtActiveUsersWithGroupRID = secAdmin.GetActiveUsersWithGroupRID();

            DataSet dsUsersAndGroups = new DataSet();

            dsUsersAndGroups.Tables.Add(dtActiveGroups);
            dsUsersAndGroups.Tables.Add(dtActiveUsersWithGroupRID);

            dsUsersAndGroups.Relations.Add("GroupsToUsers", dtActiveGroups.Columns["GROUP_RID"], dtActiveUsersWithGroupRID.Columns["GROUP_RID"], false);

            this.midSelectMultiNodeControl1.ShowRootLines = true;
            this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "GroupsToUsers";

            this.midSelectMultiNodeControl1.FieldToTag = "USER_RID";
            this.midSelectMultiNodeControl1.BindDataSet(dsUsersAndGroups);
        }

        public void GetSelectedUsers(ref ReportData.AllocationAnalysisEventArgs e)
        {
            if (this.midSelectMultiNodeControl1.IsEveryNodeSelected() == true)
            {
                e.restrictUsers = false;
            }
            else
            {
                e.restrictUsers = true;
                e.userRIDsToInclude = this.midSelectMultiNodeControl1.GetSelectedListFromTags(false);
            }
        }

        public void GetSelectedUsers(ref ReportData.ForecastAnalysisEventArgs e)
        {
            if (this.midSelectMultiNodeControl1.IsEveryNodeSelected() == true)
            {
                e.restrictUsers = false;
            }
            else
            {
                e.restrictUsers = true;
                e.userRIDsToInclude = this.midSelectMultiNodeControl1.GetSelectedListFromTags(false);
            }
        }


        

    }
}
