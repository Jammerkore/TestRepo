using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Windows
{
	public partial class AverageUnitDialog : Form
	{
		private DataTable _dialogGrid;
		private ProfileList _columnList;
		private List<int> _resultList;
        private eAllocationAssortmentViewGroupBy _groupBy;

		public List<int> ResultList
		{
			get { return _resultList; }
			//set {_resultList = value; }
		}

		public AverageUnitDialog(ProfileList aList, eAllocationAssortmentViewGroupBy groupBy)
		{
			InitializeComponent();
            _groupBy = groupBy;
            //==========================================
            // Depending upon GroupBy
            // Column List is either a list of grades
            // OR a list of Group Levels (Sets)
            //==========================================
            _columnList = aList;
            if (groupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
            {
               
            }
            else
            {

            }
            _resultList = new List<int>();

		}

		private void AverageUnitDialog_Load(object sender, EventArgs e)
		{
            if (_groupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
            {
                _dialogGrid = BuildGradeGrid(_columnList);
            }
            else if (_groupBy == eAllocationAssortmentViewGroupBy.Attribute)
            {
                _dialogGrid = BuildSetGrid(_columnList);
            }

            dataGridView1.DataSource = _dialogGrid;
            // Resize the DataGridView columns to fit the newly loaded content.
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;

			this.dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
			this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
			SetText();	// TT#2 - stodd
		}

		// Begin TT#2 - stodd
		private void SetText()
		{
			try
			{
				this.btOk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				this.btCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// End TT#2 - stodd

		private DataTable BuildGradeGrid(ProfileList gradeList)
		{
			DataTable dtGradeGrid;

			try
			{
				dtGradeGrid = MIDEnvironment.CreateDataTable();
				foreach (StoreGradeProfile gp in gradeList.ArrayList)
				{
					dtGradeGrid.Columns.Add(new DataColumn(gp.StoreGrade, typeof(int)));
				}
				DataRow aRow = dtGradeGrid.NewRow();
				dtGradeGrid.Rows.Add(aRow);
				return dtGradeGrid;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private DataTable BuildSetGrid(ProfileList setList)
        {
            DataTable dtSetGrid;

            try
            {
                dtSetGrid = MIDEnvironment.CreateDataTable();
                foreach (StoreGroupLevelProfile sglp in setList.ArrayList)
                {
                    dtSetGrid.Columns.Add(new DataColumn(sglp.Name, typeof(int)));
                }
                DataRow aRow = dtSetGrid.NewRow();
                dtSetGrid.Rows.Add(aRow);
                return dtSetGrid;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void btCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btOk_Click(object sender, EventArgs e)
		{
			try
			{
				int avgUnits = 0;
				ResultList.Clear();
				DataRow row = _dialogGrid.Rows[0];
				for (int i = 0; i < _dialogGrid.Columns.Count; i++)
				{
					if (row[i] == DBNull.Value)
						avgUnits = 0;
					else
						avgUnits = Convert.ToInt32(row[i], CultureInfo.CurrentUICulture);
					ResultList.Add(avgUnits);
				}
				DialogResult = DialogResult.OK;
				Close();
			}
			catch
			{
				throw;
			}
		}

		private void AverageUnitDialog_Shown(object sender, EventArgs e)
		{
			dataGridView1.Focus();
			dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
			dataGridView1.BeginEdit(true);
		}

		private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			try
			{
				string formattedValue = e.FormattedValue.ToString();

				if (!string.IsNullOrEmpty(formattedValue))
				{
					Convert.ToInt32(e.FormattedValue, CultureInfo.CurrentUICulture);
				}
			}
			catch
			{
				dataGridView1.Rows[e.RowIndex].ErrorText =
						"Invalid value entered";
				MessageBox.Show("Invalid value entered");
				e.Cancel = true;
			}			
		}

		void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			// Clear the row error in case the user presses ESC.   
			dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
		}

		


	}
}