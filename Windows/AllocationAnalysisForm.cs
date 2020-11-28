using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
    public partial class AllocationAnalysisForm : Form
    {
        private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
        public AllocationAnalysisForm(SessionAddressBlock SAB, ExplorerAddressBlock EAB)
        {
            _SAB = SAB;
            _EAB = EAB;
            InitializeComponent();
        }

        private void AllocationAnalysisForm_Load(object sender, EventArgs e)
        {
            this.allocationAnalysisControl1.LoadData(_SAB, new GetSelectedHeaderRIDsFromWorkspace(GetSelectedHeaders));

            //if (MIDRetail.Windows.Controls.SharedControlRoutines.exportHelper == null)
            //{
            //    MIDRetail.Windows.Controls.SharedControlRoutines.exportHelper =
            //        new Controls.SharedControlRoutines.GridExportHelper(new Controls.SharedControlRoutines.ExportAllRowsToExcelDelegate(SharedRoutines.ExportAllRowsToExcel),
            //                                                            new Controls.SharedControlRoutines.ExportSelectedRowsToExcelDelegate(SharedRoutines.ExportSelectedRowsToExcel),
            //                                                            new Controls.SharedControlRoutines.EmailAllRowsDelegate(SharedRoutines.EmailAllRows),
            //                                                            new Controls.SharedControlRoutines.EmailSelectedRowsDelegate(SharedRoutines.EmailSelectedRows)
            //                                                            );
                                                                        

                                                                        
            //}
        }

        public List<int> GetSelectedHeaders()
        {
            return _EAB.AllocationWorkspaceExplorer.GetSelectedHeaderRIDs();
        }
   
    }
}
