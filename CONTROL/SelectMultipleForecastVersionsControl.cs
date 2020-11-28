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
    public partial class SelectMultipleForecastVersionsControl : UserControl
    {
        public SelectMultipleForecastVersionsControl()
        {
            InitializeComponent();
        }

        private void SelectMultipleForecastVersionsControl_Load(object sender, EventArgs e)
        {
        }

        private bool _AllowActual = true;
        public bool AllowActual
        {
            get { return _AllowActual; }
            set { _AllowActual = value; }
        }

        private bool _EnforceSecurity = false;
        public bool EnforceSecurity
        {
            get { return _EnforceSecurity; }
            set { _EnforceSecurity = value; }
        }

        //private bool _IncludeBlank = false;
        //public bool IncludeBlank
        //{
        //    get { return _IncludeBlank; }
        //    set { _IncludeBlank = value; }
        //}

        public void LoadData(SessionAddressBlock SAB)
        {
            ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            DataTable dt = MIDEnvironment.CreateDataTable("Versions");
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Key", typeof(int));

            //if (_IncludeBlank == true)
            //{
            //    dt.Rows.Add(new object[] { string.Empty, Include.NoRID });
            //}

            foreach (VersionProfile verProf in versionProfList)
            {
                if (_EnforceSecurity == false || !verProf.StoreSecurity.AccessDenied)
                {
                    if (_AllowActual == true || verProf.Description != "Actual")
                    {
                        
                            dt.Rows.Add(new object[] { verProf.Description, verProf.Key });
                       
                    }
                }
            }

            DataSet dsForecastVersions = new DataSet();

            dsForecastVersions.Tables.Add(dt);

            this.midSelectMultiNodeControl1.FieldToTag = "Key";
            this.midSelectMultiNodeControl1.BindDataSet(dsForecastVersions);

           
        }


        public bool IsEveryNodeSelected()
        {
            return this.midSelectMultiNodeControl1.IsEveryNodeSelected();
          
        }
        public string GetSelectedVersions()
        {
            return this.midSelectMultiNodeControl1.GetSelectedListFromTags(true);
        }
    }
}
