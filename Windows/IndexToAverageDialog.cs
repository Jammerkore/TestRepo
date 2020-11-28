using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	public partial class IndexToAverageDialog : MIDFormBase
    {
		private List<string> _grades;
		private SessionAddressBlock _sab;
		private eSpreadAverage _spreadOption;

		Color _backgroundColor;
		Color _defaultCellStyleColor;
		Color _selectionBackColor;
		

		public eSpreadAverage SpreadOption
		{
			get { return _spreadOption; }
			set { _spreadOption = value; }
		}

        //Construct-TOR!!
        public IndexToAverageDialog(SessionAddressBlock sab, List<string> p_grades)
        {
            _grades = p_grades;
			_sab = sab;

            InitializeIdxToAvgDialog();
        }

		private void IndexToAverageDialog_Load(object sender, EventArgs e)
		{
			ActiveControl = txtTotal;
			SetText();
			FormLoaded = true;
		}

		private void SetText()
		{
			try
			{
				this.Grade.HeaderText = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
				this.Average.HeaderText = MIDText.GetTextOnly(eMIDTextCode.lbl_Average);
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

        private void rdoTotal_CheckedChanged(object sender, EventArgs e)
        {
			if (FormLoaded)
			{
				if (rdoTotal.Checked == true)
				{
					_returnType = eIndexToAverageReturnType.Total;
					txtTotal.Enabled = true;
					ActiveControl = txtTotal;
				}
			}
            //manage controls
			DisableGradeTable();
        }

        private void rdoSetTotal_CheckedChanged(object sender, EventArgs e)
        {
			if (FormLoaded)
			{
				if (rdoSetTotal.Checked == true)
				{
					_returnType = eIndexToAverageReturnType.SetTotal;
					txtTotal.Enabled = true;
					ActiveControl = txtTotal;
				}
			}
            //manage controls
			DisableGradeTable();
        }

		private void DisableGradeTable()
		{
			if (!FormLoaded)
			{
				_backgroundColor = grdGrades.BackgroundColor;
				_defaultCellStyleColor = grdGrades.Rows[0].DefaultCellStyle.BackColor;
				_selectionBackColor = grdGrades.DefaultCellStyle.SelectionBackColor;
				grdGrades.DefaultCellStyle.SelectionBackColor = grdGrades.DefaultCellStyle.BackColor;
			}
			grdGrades.Enabled = false;
			grdGrades.BackgroundColor = Color.LightGray;
			grdGrades.DefaultCellStyle.SelectionBackColor = Color.LightGray;
			foreach (DataGridViewRow dgvr in grdGrades.Rows)
			{
				dgvr.DefaultCellStyle.BackColor = Color.LightGray;
			} 
		}

		private void EnableGradeTable()
		{
			grdGrades.Enabled = true;
			grdGrades.BackgroundColor = _backgroundColor;
			grdGrades.DefaultCellStyle.SelectionBackColor = _selectionBackColor;
			foreach (DataGridViewRow dgvr in grdGrades.Rows)
			{
				dgvr.DefaultCellStyle.BackColor = _defaultCellStyleColor;
			}
			grdGrades.Rows[0].Cells[1].Selected = true;
		}

        private void rdoGrades_CheckedChanged(object sender, EventArgs e)
        {
			if (FormLoaded)
			{
				if (rdoGrades.Checked == true)
				{
					_returnType = eIndexToAverageReturnType.Grades;
					EnableGradeTable();
				}
			}
            //manage controls
			txtTotal.Enabled = false;
			ActiveControl = grdGrades;
			grdGrades.Rows[0].Cells[1].Selected = true;
        }

        private void InitializeIdxToAvgDialog()
        {
            InitializeComponent();

            //add the passed grades to the grid
            if (_grades.Count > 0)
            {
                foreach (string strGrade in _grades)
                {
                    DataGridViewRow dgvGrade = new DataGridViewRow();
                    DataGridViewCell dgvCell;
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = strGrade;
                    dgvGrade.Cells.Add(dgvCell);

                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = "";
                    dgvGrade.Cells.Add(dgvCell);

                    dgvGrade.Cells[0].Value = strGrade;
                    grdGrades.Rows.Add(dgvGrade);
                }
            }

            //default values
            rdoTotal.Checked = true;
			rbSmooth.Checked = true;
            txtTotal.Enabled = true;
            rdoGrades.Checked = false;
            grdGrades.Enabled = false;
        }

		override public void ISave()
		{
			// Begin TT#2 - stodd - assortment
			//set the return type
			//if (rdoGrades.Checked == true)
			//{
			//    _returnType = eIndexToAverageReturnType.Grades;
			//}
			//else if (rdoTotal.Checked == true)
			//{
			//    _returnType = eIndexToAverageReturnType.Total;
			//}
			//else if (rdoSetTotal.Checked == true)
			//{
			//    _returnType = eIndexToAverageReturnType.SetTotal;
			//}
			// End TT#2 - stodd - assortment

			//set the return value
			switch (_returnType)
			{
				case eIndexToAverageReturnType.Total:
					_returnValue = txtTotal.Text.Trim();
					break;
				case eIndexToAverageReturnType.SetTotal:
					_returnValue = txtTotal.Text.Trim();
					break;
				case eIndexToAverageReturnType.Grades:
					DataTable dtGradeReturn = new DataTable();

					// Create grade column
					DataColumn column = new DataColumn();
					column.DataType = System.Type.GetType("System.String");
					column.ColumnName = "Grade";
					dtGradeReturn.Columns.Add(column);

					// Create value column.
					column = new DataColumn();
					column.DataType = Type.GetType("System.Double");
					column.ColumnName = "Value";
					dtGradeReturn.Columns.Add(column);

					for (int i = 0; i < grdGrades.Rows.Count; i++)
					{
						//fill the datatable for return
						DataRow row = dtGradeReturn.NewRow();
						row["Grade"] = grdGrades[0, i].Value;
						// Begin TT# 2 - stodd - assortment
						if (grdGrades[1, i].Value.ToString() == string.Empty)
						{
							row["Value"] = double.MinValue;
						}
						else
						{
							row["Value"] = grdGrades[1, i].Value;
						}
						// End TT# 2 - stodd - assortment
						dtGradeReturn.Rows.Add(row);
					}

					_returnValue = dtGradeReturn;
					break;
			}

			//else
			//{
			//    this.DialogResult = DialogResult.Abort;
			//    MessageBox.Show("Input is in an invalid format. All entries must have a numeric value.", 
			//        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			//}
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
			if (ValidateFieldEntry() == true)
			{
				ISave();
				this.DialogResult = DialogResult.Yes;
			}
        }

		// Begin TT#2 - stodd - moved to enum.cs
		////public enumeration for return types
		//public enum IndexToAverageReturnTypes
		//{
		//    Total,
		//    SetTotal,
		//    Grades
		//}
		// End TT#2 - stodd - moved to enum.cs

        //return types property (read only)
        eIndexToAverageReturnType _returnType;
		public eIndexToAverageReturnType IndexToAverageReturnType
        {
            get
            {
                return _returnType;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        //generic return value (read only)
        object _returnValue;
        public object IndexToAverageReturnValue
        {
            get
            {
                return _returnValue;
            }
        }

        //validate all entries are numeric
        private bool ValidateFieldEntry()
        {
			ErrorProvider.SetError(txtTotal, string.Empty);
			ErrorProvider.SetError(grdGrades, string.Empty);

			bool isValid = true;
			if (_returnType == eIndexToAverageReturnType.Total)
            {
				if (txtTotal.Text == string.Empty)
				{
					isValid = false;
					ErrorProvider.SetError(txtTotal, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}

                char[] validChars = txtTotal.Text.ToCharArray();
                foreach (char validChar in validChars)
                {
                    if (validChar != '.')
                    {
                        if (Char.IsNumber(validChar) == false)
                        {
                            isValid = false;
							ErrorProvider.SetError(txtTotal, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                        }
                    }
                }
            }
			else if (_returnType == eIndexToAverageReturnType.SetTotal)
            {
				if (txtTotal.Text == string.Empty)
				{
					isValid = false;
					ErrorProvider.SetError(txtTotal, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}

                char[] validChars = txtTotal.Text.ToCharArray();
                foreach (char validChar in validChars)
                {
                    if (validChar != '.')
                    {
                        if (Char.IsNumber(validChar) == false)
                        {
							isValid = false;
							ErrorProvider.SetError(txtTotal, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                        }
                    }
                }
            }
			else if (_returnType == eIndexToAverageReturnType.Grades)
            {
				bool emptyGrid = true;
				for (int i = 0; i < grdGrades.Rows.Count; i++)
				{
                    // BEGIN TT#1980-MD - AGallagher - Spread Avg - Select Smooth-Grades-Type in Grades - process and receive and Unhandled Exception
                    if ((grdGrades.Rows[i].Cells[1].Value == DBNull.Value) || (grdGrades.Rows[i].Cells[1].Value == null) || (grdGrades.Rows[i].Cells[1].Value == ""))  // TT#1981-MD - AGallagher - Spread Average - Select Grades and Smooth or Spread by Index after process no values changed
                    {
                        grdGrades.Rows[i].Cells[1].Value = 0;
                    }
                    // END TT#1980-MD - AGallagher - Spread Avg - Select Smooth-Grades-Type in Grades - process and receive and Unhandled Exception
					if (grdGrades.Rows[i].Cells[1].Value.ToString().Length > 0)
					{
						emptyGrid = false;
					}
				}

				if (emptyGrid)
				{
					isValid = false;
					ErrorProvider.SetError(grdGrades, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}

                for (int i = 0; i < grdGrades.Rows.Count; i++)
                {
                    char[] validChars = grdGrades.Rows[i].Cells[1].Value.ToString().ToCharArray();
                    foreach (char validChar in validChars)
                    {
                        if (validChar != '.')
                        {
                            if (Char.IsNumber(validChar) == false)
                            {
								isValid = false;
								ErrorProvider.SetError(grdGrades, _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                            }
                        }
                    }
                }
            }
			return isValid;
        }

		private void grdGrades_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

		}

		private void rbSpreadByIndex_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSpreadByIndex.Checked)
				_spreadOption = eSpreadAverage.SpreadByIndex;
		}

		private void rbSmooth_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSmooth.Checked)
				_spreadOption = eSpreadAverage.Smooth;
		}
    }
}
