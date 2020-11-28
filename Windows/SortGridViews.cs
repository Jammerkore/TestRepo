//Begin TT#1301 - JScott - Unhandled Exception Error in Sorting with store review
//Correct issue.  Also made numerous changes made to added Exception handling.  Use SCM to compare this to previous version.
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using C1.Win.C1FlexGrid;
using System.Windows.Forms;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public struct SortCriteria 
	{
        public string Column1;
		public string Column2;
		public int Column2Num;
		public FromGrid Column2Grid;
		public C1FlexGrid Column2GridPtr;
		public eValueFormatType Column2Format;
		public SortEnum SortDirection;
	}

	public struct SortValue
	{
		public string Row1;
		public string Row2;
		public int Row2Num;
		public eValueFormatType Row2Format;
	}

	public struct structSort
	{
		public bool IsSortingByDefault;
		public ArrayList SortInfo;
		public object ValueInfo;

		public structSort(object aSortVal, params SortCriteria[] aSortCrit)
		{
			int i;

			ValueInfo = aSortVal;
			SortInfo = new ArrayList();

			for (i = 0; i < 3; i++)
			{
				if (i < aSortCrit.Length)
				{
					SortInfo.Add(aSortCrit[i]);
				}
				else
				{
					SortInfo.Add(null);
				}
			}

			IsSortingByDefault = false;
		}
	}

	/// <summary>
	/// Summary description for SortPlanView.
	/// </summary>
	public class SortGridViews : MIDFormBase
	{
		private structSort _sortParms;
		public structSort SortInfo 
		{
			get{return _sortParms;}
			set{_sortParms = value;}
		}
	 		
		#region Windows generated stuff
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkIsSortingByDefault;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox grpSortingCriteria;
		private System.Windows.Forms.Panel pnlSort1;
		private System.Windows.Forms.Panel pnlSort2;
		private System.Windows.Forms.Panel pnlSort3;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort3Major;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort2Major;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort1Major;
		private System.Windows.Forms.RadioButton rbSort3Desc;
		private System.Windows.Forms.RadioButton rbSort3Asc;
		private System.Windows.Forms.RadioButton rbSort2Desc;
		private System.Windows.Forms.RadioButton rbSort2Asc;
		private System.Windows.Forms.RadioButton rbSort1Desc;
		private System.Windows.Forms.RadioButton rbSort1Asc;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort3Minor;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort2Minor;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSort1Minor;
		private System.Windows.Forms.GroupBox grpSortingValue;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSortValueMajor;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSortValueMinor;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblValueSizer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				this.chkIsSortingByDefault.CheckedChanged -= new System.EventHandler(this.chkIsSortingByDefault_CheckedChanged);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.grpSortingCriteria.EnabledChanged -= new System.EventHandler(this.grpSortingCriteria_EnabledChanged);
				this.cboSort3Major.SelectionChangeCommitted -= new System.EventHandler(this.cboSort3Major_SelectionChangeCommitted);
				this.cboSort2Major.SelectionChangeCommitted -= new System.EventHandler(this.cboSort2Major_SelectionChangeCommitted);
				this.cboSort1Major.SelectionChangeCommitted -= new System.EventHandler(this.cboSort1Major_SelectionChangeCommitted);

                this.cboSort3Major.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSort3Major_MIDComboBoxPropertiesChangedEvent);
                this.cboSort2Major.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSort2Major_MIDComboBoxPropertiesChangedEvent);
                this.cboSort1Major.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSort1Major_MIDComboBoxPropertiesChangedEvent);
                this.cboSortValueMajor.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSortValueMajor_MIDComboBoxPropertiesChangedEvent);

				this.Load -= new System.EventHandler(this.SortGridViews_Load);
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkIsSortingByDefault = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpSortingCriteria = new System.Windows.Forms.GroupBox();
			this.cboSort3Major = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboSort2Major = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboSort1Major = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlSort3 = new System.Windows.Forms.Panel();
			this.rbSort3Desc = new System.Windows.Forms.RadioButton();
			this.rbSort3Asc = new System.Windows.Forms.RadioButton();
			this.pnlSort2 = new System.Windows.Forms.Panel();
			this.rbSort2Desc = new System.Windows.Forms.RadioButton();
			this.rbSort2Asc = new System.Windows.Forms.RadioButton();
			this.pnlSort1 = new System.Windows.Forms.Panel();
			this.rbSort1Desc = new System.Windows.Forms.RadioButton();
			this.rbSort1Asc = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cboSort3Minor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboSort2Minor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboSort1Minor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.grpSortingValue = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cboSortValueMinor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboSortValueMajor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.lblValueSizer = new System.Windows.Forms.Label();
			this.grpSortingCriteria.SuspendLayout();
			this.pnlSort3.SuspendLayout();
			this.pnlSort2.SuspendLayout();
			this.pnlSort1.SuspendLayout();
			this.grpSortingValue.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkIsSortingByDefault
			// 
			this.chkIsSortingByDefault.Location = new System.Drawing.Point(24, 8);
			this.chkIsSortingByDefault.Name = "chkIsSortingByDefault";
			this.chkIsSortingByDefault.Size = new System.Drawing.Size(136, 24);
			this.chkIsSortingByDefault.TabIndex = 0;
			this.chkIsSortingByDefault.Text = "Sort by Default Order";
			this.chkIsSortingByDefault.CheckedChanged += new System.EventHandler(this.chkIsSortingByDefault_CheckedChanged);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(240, 320);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(328, 320);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// grpSortingCriteria
			// 
			this.grpSortingCriteria.Controls.Add(this.cboSort3Major);
			this.grpSortingCriteria.Controls.Add(this.cboSort2Major);
			this.grpSortingCriteria.Controls.Add(this.cboSort1Major);
			this.grpSortingCriteria.Controls.Add(this.pnlSort3);
			this.grpSortingCriteria.Controls.Add(this.pnlSort2);
			this.grpSortingCriteria.Controls.Add(this.pnlSort1);
			this.grpSortingCriteria.Controls.Add(this.label3);
			this.grpSortingCriteria.Controls.Add(this.label2);
			this.grpSortingCriteria.Controls.Add(this.label1);
			this.grpSortingCriteria.Controls.Add(this.cboSort3Minor);
			this.grpSortingCriteria.Controls.Add(this.cboSort2Minor);
			this.grpSortingCriteria.Controls.Add(this.cboSort1Minor);
			this.grpSortingCriteria.Location = new System.Drawing.Point(8, 32);
			this.grpSortingCriteria.Name = "grpSortingCriteria";
			this.grpSortingCriteria.Size = new System.Drawing.Size(400, 200);
			this.grpSortingCriteria.TabIndex = 4;
			this.grpSortingCriteria.TabStop = false;
			this.grpSortingCriteria.Text = "Sorting Criteria";
			this.grpSortingCriteria.EnabledChanged += new System.EventHandler(this.grpSortingCriteria_EnabledChanged);
			// 
			// cboSort3Major
			// 
			this.cboSort3Major.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort3Major.Location = new System.Drawing.Point(8, 152);
			this.cboSort3Major.Name = "cboSort3Major";
			this.cboSort3Major.Size = new System.Drawing.Size(120, 21);
			this.cboSort3Major.TabIndex = 13;
			this.cboSort3Major.SelectionChangeCommitted += new System.EventHandler(this.cboSort3Major_SelectionChangeCommitted);
            this.cboSort3Major.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSort3Major_MIDComboBoxPropertiesChangedEvent);
			// 
			// cboSort2Major
			// 
			this.cboSort2Major.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort2Major.Location = new System.Drawing.Point(8, 96);
			this.cboSort2Major.Name = "cboSort2Major";
			this.cboSort2Major.Size = new System.Drawing.Size(120, 21);
			this.cboSort2Major.TabIndex = 12;
			this.cboSort2Major.SelectionChangeCommitted += new System.EventHandler(this.cboSort2Major_SelectionChangeCommitted);
            this.cboSort2Major.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSort2Major_MIDComboBoxPropertiesChangedEvent);
			// 
			// cboSort1Major
			// 
			this.cboSort1Major.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort1Major.Location = new System.Drawing.Point(8, 40);
			this.cboSort1Major.Name = "cboSort1Major";
			this.cboSort1Major.Size = new System.Drawing.Size(120, 21);
			this.cboSort1Major.TabIndex = 11;
			this.cboSort1Major.SelectionChangeCommitted += new System.EventHandler(this.cboSort1Major_SelectionChangeCommitted);
            this.cboSort1Major.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSort1Major_MIDComboBoxPropertiesChangedEvent);
			// 
			// pnlSort3
			// 
			this.pnlSort3.Controls.Add(this.rbSort3Desc);
			this.pnlSort3.Controls.Add(this.rbSort3Asc);
			this.pnlSort3.Location = new System.Drawing.Point(288, 144);
			this.pnlSort3.Name = "pnlSort3";
			this.pnlSort3.Size = new System.Drawing.Size(104, 32);
			this.pnlSort3.TabIndex = 10;
			// 
			// rbSort3Desc
			// 
			this.rbSort3Desc.Location = new System.Drawing.Point(8, 16);
			this.rbSort3Desc.Name = "rbSort3Desc";
			this.rbSort3Desc.Size = new System.Drawing.Size(88, 16);
			this.rbSort3Desc.TabIndex = 1;
			this.rbSort3Desc.Text = "Descending";
			// 
			// rbSort3Asc
			// 
			this.rbSort3Asc.Checked = true;
			this.rbSort3Asc.Location = new System.Drawing.Point(8, 0);
			this.rbSort3Asc.Name = "rbSort3Asc";
			this.rbSort3Asc.Size = new System.Drawing.Size(88, 16);
			this.rbSort3Asc.TabIndex = 0;
			this.rbSort3Asc.TabStop = true;
			this.rbSort3Asc.Text = "Ascending";
			// 
			// pnlSort2
			// 
			this.pnlSort2.Controls.Add(this.rbSort2Desc);
			this.pnlSort2.Controls.Add(this.rbSort2Asc);
			this.pnlSort2.Location = new System.Drawing.Point(288, 88);
			this.pnlSort2.Name = "pnlSort2";
			this.pnlSort2.Size = new System.Drawing.Size(104, 32);
			this.pnlSort2.TabIndex = 9;
			// 
			// rbSort2Desc
			// 
			this.rbSort2Desc.Location = new System.Drawing.Point(8, 16);
			this.rbSort2Desc.Name = "rbSort2Desc";
			this.rbSort2Desc.Size = new System.Drawing.Size(88, 16);
			this.rbSort2Desc.TabIndex = 1;
			this.rbSort2Desc.Text = "Descending";
			// 
			// rbSort2Asc
			// 
			this.rbSort2Asc.Checked = true;
			this.rbSort2Asc.Location = new System.Drawing.Point(8, 0);
			this.rbSort2Asc.Name = "rbSort2Asc";
			this.rbSort2Asc.Size = new System.Drawing.Size(88, 16);
			this.rbSort2Asc.TabIndex = 0;
			this.rbSort2Asc.TabStop = true;
			this.rbSort2Asc.Text = "Ascending";
			// 
			// pnlSort1
			// 
			this.pnlSort1.Controls.Add(this.rbSort1Desc);
			this.pnlSort1.Controls.Add(this.rbSort1Asc);
			this.pnlSort1.Location = new System.Drawing.Point(288, 32);
			this.pnlSort1.Name = "pnlSort1";
			this.pnlSort1.Size = new System.Drawing.Size(104, 32);
			this.pnlSort1.TabIndex = 8;
			// 
			// rbSort1Desc
			// 
			this.rbSort1Desc.Location = new System.Drawing.Point(8, 16);
			this.rbSort1Desc.Name = "rbSort1Desc";
			this.rbSort1Desc.Size = new System.Drawing.Size(88, 16);
			this.rbSort1Desc.TabIndex = 1;
			this.rbSort1Desc.Text = "Descending";
			// 
			// rbSort1Asc
			// 
			this.rbSort1Asc.Checked = true;
			this.rbSort1Asc.Location = new System.Drawing.Point(8, 0);
			this.rbSort1Asc.Name = "rbSort1Asc";
			this.rbSort1Asc.Size = new System.Drawing.Size(88, 16);
			this.rbSort1Asc.TabIndex = 0;
			this.rbSort1Asc.TabStop = true;
			this.rbSort1Asc.Text = "Ascending";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Then By:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 6;
			this.label2.Text = "Then By:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Sort By:";
			// 
			// cboSort3Minor
			// 
			this.cboSort3Minor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort3Minor.Location = new System.Drawing.Point(128, 152);
			this.cboSort3Minor.Name = "cboSort3Minor";
			this.cboSort3Minor.Size = new System.Drawing.Size(160, 21);
			this.cboSort3Minor.TabIndex = 4;
			// 
			// cboSort2Minor
			// 
			this.cboSort2Minor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort2Minor.Location = new System.Drawing.Point(128, 96);
			this.cboSort2Minor.Name = "cboSort2Minor";
			this.cboSort2Minor.Size = new System.Drawing.Size(160, 21);
			this.cboSort2Minor.TabIndex = 2;
			// 
			// cboSort1Minor
			// 
			this.cboSort1Minor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSort1Minor.Location = new System.Drawing.Point(128, 40);
			this.cboSort1Minor.Name = "cboSort1Minor";
			this.cboSort1Minor.Size = new System.Drawing.Size(160, 21);
			this.cboSort1Minor.TabIndex = 0;
			// 
			// grpSortingValue
			// 
			this.grpSortingValue.Controls.Add(this.panel1);
			this.grpSortingValue.Controls.Add(this.lblValueSizer);
			this.grpSortingValue.Location = new System.Drawing.Point(8, 240);
			this.grpSortingValue.Name = "grpSortingValue";
			this.grpSortingValue.Size = new System.Drawing.Size(400, 64);
			this.grpSortingValue.TabIndex = 5;
			this.grpSortingValue.TabStop = false;
			this.grpSortingValue.Text = "Sorting Value";
			this.grpSortingValue.EnabledChanged += new System.EventHandler(this.grpSortingValue_EnabledChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cboSortValueMinor);
			this.panel1.Controls.Add(this.cboSortValueMajor);
			this.panel1.Location = new System.Drawing.Point(8, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(376, 24);
			this.panel1.TabIndex = 16;
			// 
			// cboSortValueMinor
			// 
			this.cboSortValueMinor.Dock = System.Windows.Forms.DockStyle.Left;
			this.cboSortValueMinor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSortValueMinor.Location = new System.Drawing.Point(208, 0);
			this.cboSortValueMinor.Name = "cboSortValueMinor";
			this.cboSortValueMinor.Size = new System.Drawing.Size(160, 21);
			this.cboSortValueMinor.TabIndex = 15;
			// 
			// cboSortValueMajor
			// 
			this.cboSortValueMajor.Dock = System.Windows.Forms.DockStyle.Left;
			this.cboSortValueMajor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSortValueMajor.DropDownWidth = 224;
			this.cboSortValueMajor.Location = new System.Drawing.Point(0, 0);
			this.cboSortValueMajor.Name = "cboSortValueMajor";
			this.cboSortValueMajor.Size = new System.Drawing.Size(208, 21);
			this.cboSortValueMajor.TabIndex = 14;
			this.cboSortValueMajor.SelectionChangeCommitted += new System.EventHandler(this.cboSortValueMajor_SelectionChangeCommitted);
            this.cboSortValueMajor.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSortValueMajor_MIDComboBoxPropertiesChangedEvent);
			// 
			// lblValueSizer
			// 
			this.lblValueSizer.AutoSize = true;
			this.lblValueSizer.Location = new System.Drawing.Point(344, 48);
			this.lblValueSizer.Name = "lblValueSizer";
			this.lblValueSizer.Size = new System.Drawing.Size(0, 16);
			this.lblValueSizer.TabIndex = 6;
			this.lblValueSizer.Visible = false;
			// 
			// SortGridViews
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(418, 352);
			this.Controls.Add(this.grpSortingValue);
			this.Controls.Add(this.grpSortingCriteria);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.chkIsSortingByDefault);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SortGridViews";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sort";
			this.Load += new System.EventHandler(this.SortGridViews_Load);
			this.grpSortingCriteria.ResumeLayout(false);
			this.pnlSort3.ResumeLayout(false);
			this.pnlSort2.ResumeLayout(false);
			this.pnlSort1.ResumeLayout(false);
			this.grpSortingValue.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		private ArrayList _sortList;
		private ArrayList _valueList;
		private DataTable _dtMajor;
		private DataTable _dtMinor;
		private DataTable _dtValueMajor;
		private DataTable _dtValueMinor;
		private DataView _dvMajor1;
		private DataView _dvMajor2;
		private DataView _dvMajor3;
		private DataView _dvMinor1;
		private DataView _dvMinor2;
		private DataView _dvMinor3;
		private DataView _dvValueMajor;
		private DataView _dvValueMinor;
		private bool _loading;
		private bool _errorFound;
		private Hashtable ColumnNames;
		private Hashtable RowNames;
		#endregion
		#endregion

		public SortGridViews(structSort SortParms, ArrayList SortList)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_valueList = null;
			_sortList = SortList; 
			_sortParms = SortParms;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public SortGridViews(structSort SortParms, ArrayList SortList, ArrayList ValueList)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_valueList = ValueList;
			_sortList = SortList; 
			_sortParms = SortParms;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void SortGridViews_Load(object sender, System.EventArgs e)
		{
			try
			{
				LoadSortComboBoxes();
				chkIsSortingByDefault.Checked = _sortParms.IsSortingByDefault;

				if (_valueList != null)
				{
					this.grpSortingValue.Visible = true;
					this.Size = new Size(424, 384);
				}
				else
				{
					this.grpSortingValue.Visible = false;
					this.Size = new Size(424, 312);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void LoadSortComboBoxes()
		{
			try
			{
				//---------------------------------
				// Load Column (Major/Minor) Combos
				//---------------------------------

				ColumnNames = new Hashtable();
				_dtMajor = MIDEnvironment.CreateDataTable();
				_dtMinor = MIDEnvironment.CreateDataTable();
				DataRow drMaj, drMin;
				DataColumn dcMaj, dcMin;

				// Create new DataColumn, set DataType, ColumnName and add to DataTable.  
				// Column1 - _dtMajor
				dcMaj = new DataColumn();
				dcMaj.DataType = System.Type.GetType("System.String");
				dcMaj.ColumnName = "MajorSortName";
				dcMaj.ReadOnly = false;
				dcMaj.Unique = false;
				// Add the Column to the DataColumnCollection.
				_dtMajor.Columns.Add(dcMaj);

				// Column1 - _dtMinor
				dcMin = new DataColumn();
				dcMin.DataType = System.Type.GetType("System.String");
				dcMin.ColumnName = "MajorSortName";
				dcMin.ReadOnly = false;
				dcMin.Unique = false;
				// Add the Column to the DataColumnCollection.
				_dtMinor.Columns.Add(dcMin);

				// Column2 to _dtMinor only
				dcMin = new DataColumn();
				dcMin.DataType = System.Type.GetType("System.String");
				dcMin.ColumnName = "MinorSortName";
				dcMin.ReadOnly = false;
				dcMin.Unique = false;
				// Add the Column to the DataColumnCollection.
				_dtMinor.Columns.Add(dcMin);

				// Column2Index to _dtMinor only
				dcMin = new DataColumn();
				dcMin.DataType = System.Type.GetType("System.Int32");
				dcMin.ColumnName = "ArrayIndex";
				dcMin.ReadOnly = false;
				dcMin.Unique = false;
				// Add the Column to the DataColumnCollection.
				_dtMinor.Columns.Add(dcMin);

				// Add empty row to denote 'No selection' 
				drMaj = _dtMajor.NewRow();
				drMaj["MajorSortName"] = "";
				_dtMajor.Rows.Add(drMaj);

				drMin = _dtMinor.NewRow();
				drMin["MajorSortName"] = "";
				_dtMinor.Rows.Add(drMin);

				string FoundName = null;
				for (int i = 0; i < _sortList.Count; i++)
				{
					SortCriteria sc = (SortCriteria)_sortList[i];
					FoundName = (string)ColumnNames[sc.Column1];
					if (FoundName == null)
					{
						ColumnNames.Add(sc.Column1, sc.Column1);
						drMaj = _dtMajor.NewRow();
						drMaj["MajorSortName"] = sc.Column1;
						_dtMajor.Rows.Add(drMaj);
					}
					drMin = _dtMinor.NewRow();	// Load all names to _dtMinor
					drMin["MajorSortName"] = sc.Column1;
					drMin["MinorSortName"] = sc.Column2;
					drMin["ArrayIndex"] = i;
					_dtMinor.Rows.Add(drMin);
				}
				_loading = true;

				_dvMajor1 = new DataView(_dtMajor);
				this.cboSort1Major.DataSource = _dvMajor1;
				this.cboSort1Major.ValueMember = "MajorSortName";
				this.cboSort1Major.DisplayMember = "MajorSortName";

				_dvMajor2 = new DataView(_dtMajor);
				this.cboSort2Major.DataSource = _dvMajor2;
				this.cboSort2Major.ValueMember = "MajorSortName";
				this.cboSort2Major.DisplayMember = "MajorSortName";

				_dvMajor3 = new DataView(_dtMajor);
				this.cboSort3Major.DataSource = _dvMajor3;
				this.cboSort3Major.ValueMember = "MajorSortName";
				this.cboSort3Major.DisplayMember = "MajorSortName";
				_loading = false;

				if (_sortParms.SortInfo != null)
				{
					SortCriteria sc = new SortCriteria();
					if (_sortParms.SortInfo[0] != null)
					{
						sc = (SortCriteria)_sortParms.SortInfo[0];
						this.cboSort1Major.SelectedValue = sc.Column1;
						this.cboSort1Minor.SelectedValue = sc.Column2;
						if (sc.SortDirection == SortEnum.asc)
							rbSort1Asc.Checked = true;
						else
							rbSort1Desc.Checked = true;
					}
					else
						this.cboSort1Major.SelectedIndex = 0;

					if (_sortParms.SortInfo[1] != null)
					{
						sc = (SortCriteria)_sortParms.SortInfo[1];
						this.cboSort2Major.SelectedValue = sc.Column1;
						this.cboSort2Minor.SelectedValue = sc.Column2;
						if (sc.SortDirection == SortEnum.asc)
							rbSort2Asc.Checked = true;
						else
							rbSort2Desc.Checked = true;
					}
					else
						this.cboSort2Major.SelectedIndex = 0;

					if (_sortParms.SortInfo[2] != null)
					{
						sc = (SortCriteria)_sortParms.SortInfo[2];
						this.cboSort3Major.SelectedValue = sc.Column1;
						this.cboSort3Minor.SelectedValue = sc.Column2;
						if (sc.SortDirection == SortEnum.asc)
							rbSort3Asc.Checked = true;
						else
							rbSort3Desc.Checked = true;
					}
					else
						this.cboSort3Major.SelectedIndex = 0;
				}
				else
				{
					this.cboSort1Major.SelectedIndex = 0;
					this.cboSort2Major.SelectedIndex = 0;
					this.cboSort3Major.SelectedIndex = 0;
				}

				//------------------------
				// Load Row (Value) Combos
				//------------------------

				if (_valueList != null)
				{
					RowNames = new Hashtable();
					_dtValueMajor = MIDEnvironment.CreateDataTable();
					_dtValueMinor = MIDEnvironment.CreateDataTable();
					DataRow drValMaj, drValMin;
					DataColumn dcValMaj, dcValMin;

					// Create new DataColumn, set DataType, ColumnName and add to DataTable.  
					// Column1 - _dtValueMajor
					dcValMaj = new DataColumn();
					dcValMaj.DataType = System.Type.GetType("System.String");
					dcValMaj.ColumnName = "MajorSortName";
					dcValMaj.ReadOnly = false;
					dcValMaj.Unique = false;
					// Add the Column to the DataColumnCollection.
					_dtValueMajor.Columns.Add(dcValMaj);

					// Column1 - _dtValueMinor
					dcValMin = new DataColumn();
					dcValMin.DataType = System.Type.GetType("System.String");
					dcValMin.ColumnName = "MajorSortName";
					dcValMin.ReadOnly = false;
					dcValMin.Unique = false;
					// Add the Column to the DataColumnCollection.
					_dtValueMinor.Columns.Add(dcValMin);

					// Column2 to _dtValueMinor only
					dcValMin = new DataColumn();
					dcValMin.DataType = System.Type.GetType("System.String");
					dcValMin.ColumnName = "MinorSortName";
					dcValMin.ReadOnly = false;
					dcValMin.Unique = false;
					// Add the Column to the DataColumnCollection.
					_dtValueMinor.Columns.Add(dcValMin);

					// Column2Index to _dtValueMinor only
					dcValMin = new DataColumn();
					dcValMin.DataType = System.Type.GetType("System.Int32");
					dcValMin.ColumnName = "ArrayIndex";
					dcValMin.ReadOnly = false;
					dcValMin.Unique = false;
					// Add the Column to the DataColumnCollection.
					_dtValueMinor.Columns.Add(dcValMin);

					// Add empty row to denote 'No selection' 
					drValMaj = _dtValueMajor.NewRow();
					drValMaj["MajorSortName"] = "";
					_dtValueMajor.Rows.Add(drValMaj);

					drValMin = _dtValueMinor.NewRow();
					drValMin["MajorSortName"] = "";
					_dtValueMinor.Rows.Add(drValMin);

					FoundName = null;
					int maxValueSize = cboSortValueMajor.Width;
					for (int i = 0; i < _valueList.Count; i++)
					{
						SortValue sv = (SortValue)_valueList[i];
						lblValueSizer.Text = sv.Row1;
						maxValueSize = Math.Max(maxValueSize, lblValueSizer.Width);
						FoundName = (string)RowNames[sv.Row1];
						if (FoundName == null)
						{
							RowNames.Add(sv.Row1, sv.Row1);
							drValMaj = _dtValueMajor.NewRow();
							drValMaj["MajorSortName"] = sv.Row1;
							_dtValueMajor.Rows.Add(drValMaj);
						}
						drValMin = _dtValueMinor.NewRow();	// Load all names to _dtMinor
						drValMin["MajorSortName"] = sv.Row1;
						drValMin["MinorSortName"] = sv.Row2;
						drValMin["ArrayIndex"] = i;
						_dtValueMinor.Rows.Add(drValMin);
					}
					_loading = true;
					cboSortValueMajor.DropDownWidth = maxValueSize;

					_dvValueMajor = new DataView(_dtValueMajor);
					this.cboSortValueMajor.DataSource = _dvValueMajor;
					this.cboSortValueMajor.ValueMember = "MajorSortName";
					this.cboSortValueMajor.DisplayMember = "MajorSortName";
					_loading = false;

					if (RowNames.Count == 1)
					{
						this.cboSortValueMajor.SelectedIndex = 1;
						cboSortValueMajor.Visible = false;
					}
					else if (_sortParms.SortInfo != null && _sortParms.ValueInfo != null)
					{
						this.cboSortValueMajor.SelectedValue = ((SortValue)_sortParms.ValueInfo).Row1;
						cboSortValueMajor.Visible = true;
					}
					else
					{
						this.cboSortValueMajor.SelectedIndex = 0;
						cboSortValueMajor.Visible = true;
					}

					if (_sortParms.SortInfo != null && _sortParms.ValueInfo != null)
					{
						this.cboSortValueMinor.SelectedValue = ((SortValue)_sortParms.ValueInfo).Row2;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				PerformEdits();
				if (_errorFound)
					return;
				else
				{
					_sortParms.IsSortingByDefault = chkIsSortingByDefault.Checked;
					if (!_sortParms.IsSortingByDefault)
					{
						CaptureInformation();
					}
					this.DialogResult = DialogResult.OK;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}


		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void PerformEdits()
		{
			try
			{
				_errorFound = false;
				if (!chkIsSortingByDefault.Checked &&
					cboSort1Major.SelectedIndex < 1 &&
					cboSort2Major.SelectedIndex < 1 &&
					cboSort3Major.SelectedIndex < 1)
				{
					MessageBox.Show("No sort criteria specified. Either check Sort by Default Order "
						+ "or select sort criteria.", "Error");
					_errorFound = true;
				}

				if (!chkIsSortingByDefault.Checked &&
					((cboSort1Major.SelectedIndex > 0 && cboSort1Minor.SelectedIndex < 1)
					|| (cboSort2Major.SelectedIndex > 0 && cboSort2Minor.SelectedIndex < 1)
					|| (cboSort3Major.SelectedIndex > 0 && cboSort3Minor.SelectedIndex < 1)))
				{
					MessageBox.Show("Both sort criteria must be selected for "
						+ "Sort By or Then By.", "Error");
					_errorFound = true;
				}

				if (_valueList != null)
				{
					if (!chkIsSortingByDefault.Checked &&
						cboSortValueMajor.SelectedIndex < 1)
					{
						MessageBox.Show("No sort value specified. Either check Sort by Default Order "
							+ "or select sort values.", "Error");
						_errorFound = true;
					}

					if (!chkIsSortingByDefault.Checked &&
						(cboSortValueMajor.SelectedIndex > 0 && cboSortValueMinor.SelectedIndex < 1))
					{
						MessageBox.Show("Both values must be selected for sort values.", "Error");
						_errorFound = true;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void CaptureInformation()
		{
			try
			{
				_sortParms.SortInfo = new ArrayList();
				SortCriteria sc = new SortCriteria();

				sc.Column1 = cboSort1Major.Text;
				sc.Column2 = cboSort1Minor.Text;
				if (rbSort1Asc.Checked)
					sc.SortDirection = SortEnum.asc;
				else
					sc.SortDirection = SortEnum.desc;
				GetDataFromArray(_dvMinor1, ref sc);
				_sortParms.SortInfo.Add(sc);

				sc.Column1 = cboSort2Major.Text;
				sc.Column2 = cboSort2Minor.Text;
				if (rbSort2Asc.Checked)
					sc.SortDirection = SortEnum.asc;
				else
					sc.SortDirection = SortEnum.desc;
				GetDataFromArray(_dvMinor2, ref sc);
				_sortParms.SortInfo.Add(sc);

				sc.Column1 = cboSort3Major.Text;
				sc.Column2 = cboSort3Minor.Text;
				if (rbSort3Asc.Checked)
					sc.SortDirection = SortEnum.asc;
				else
					sc.SortDirection = SortEnum.desc;
				GetDataFromArray(_dvMinor3, ref sc);
				_sortParms.SortInfo.Add(sc);

				if (_valueList != null)
				{
					SortValue sv = new SortValue();

					sv.Row1 = cboSortValueMajor.Text;
					sv.Row2 = cboSortValueMinor.Text;
					GetDataFromArray(_dvValueMinor, ref sv);
					_sortParms.ValueInfo = sv;
				}
			}
			catch
			{
				throw;
			}
		}

		private void GetDataFromArray(DataView aDataView, ref SortCriteria sc )
		{
			int row, arrayIndex;
			SortCriteria sc2 = new SortCriteria();

			try
			{
				if (aDataView != null)
				{
					aDataView.Sort = "MinorSortName";
					row = aDataView.Find(sc.Column2);
					if (row > 0)
					{
						arrayIndex = Convert.ToInt32(aDataView[row]["ArrayIndex"], CultureInfo.CurrentUICulture);
						sc2 = (SortCriteria)_sortList[arrayIndex];
						sc.Column2Num = sc2.Column2Num;
						sc.Column2Grid = sc2.Column2Grid;
						sc.Column2GridPtr = sc2.Column2GridPtr;
						sc.Column2Format = sc2.Column2Format;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void GetDataFromArray(DataView aDataView, ref SortValue sv )
		{
			int row, arrayIndex;
			SortValue sv2 = new SortValue();

			try
			{
				if (aDataView != null)
				{
					aDataView.Sort = "MinorSortName";
					row = aDataView.Find(sv.Row2);
					if (row > 0)
					{
						arrayIndex = Convert.ToInt32(aDataView[row]["ArrayIndex"], CultureInfo.CurrentUICulture);
						sv2 = (SortValue)_valueList[arrayIndex];
						sv.Row2Num = sv2.Row2Num;
						sv.Row2Format = sv2.Row2Format;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void chkIsSortingByDefault_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkIsSortingByDefault.Checked)
				{
					rbSort1Asc.Checked = true;
					rbSort2Asc.Checked = true;
					rbSort3Asc.Checked = true;
					cboSort1Minor.Enabled = false;
					cboSort2Minor.Enabled = false;
					cboSort3Minor.Enabled = false;
					pnlSort1.Enabled = false;
					pnlSort2.Enabled = false;
					pnlSort3.Enabled = false;
				}
				grpSortingCriteria.Enabled = !chkIsSortingByDefault.Checked;

				if (_valueList != null)
				{
					if (chkIsSortingByDefault.Checked)
					{
						cboSortValueMinor.Enabled = false;
					}
					grpSortingValue.Enabled = !chkIsSortingByDefault.Checked;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grpSortingCriteria_EnabledChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (grpSortingCriteria.Enabled == false)
				{
					cboSort1Major.SelectedIndex = 0;
					cboSort2Major.SelectedIndex = 0;
					cboSort3Major.SelectedIndex = 0;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grpSortingValue_EnabledChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (grpSortingValue.Enabled == false)
				{
					if (cboSortValueMajor.Visible)
					{
						cboSortValueMajor.SelectedIndex = 0;
					}
					else
					{
						cboSortValueMinor.SelectedIndex = 0;
					}
				}
				else
				{
					if (!cboSortValueMajor.Visible)
					{
						cboSortValueMinor.Enabled = true;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboSort1Major_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loading)
				{
					_dvMinor1 = new DataView(
						_dtMinor,
						FormatFilterString(cboSort1Major.Text),
						"ArrayIndex",
						DataViewRowState.CurrentRows);

					this.cboSort1Minor.DataSource = _dvMinor1;
					this.cboSort1Minor.ValueMember = "MinorSortName";
					this.cboSort1Minor.DisplayMember = "MinorSortName";
					this.cboSort1Minor.SelectedIndex = 0;
				}

				cboSort1Minor.Enabled = (cboSort1Major.SelectedIndex > 0);
				pnlSort1.Enabled = (cboSort1Major.SelectedIndex > 0);

				if (cboSort1Minor.Enabled)
				{
					if (cboSort1Minor.Items.Count == 2)
					{
						cboSort1Minor.SelectedIndex = 1;
					}
					else
					{
						cboSort1Minor.SelectedIndex = 0;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboSort2Major_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loading)
				{
					_dvMinor2 = new DataView(
						_dtMinor,
						FormatFilterString(cboSort2Major.Text),
						"ArrayIndex",
						DataViewRowState.CurrentRows);

					this.cboSort2Minor.DataSource = _dvMinor2;
					this.cboSort2Minor.ValueMember = "MinorSortName";
					this.cboSort2Minor.DisplayMember = "MinorSortName";
					this.cboSort2Minor.SelectedIndex = 0;
				}

				cboSort2Minor.Enabled = (cboSort2Major.SelectedIndex > 0);
				pnlSort2.Enabled = (cboSort2Major.SelectedIndex > 0);

				if (cboSort2Minor.Enabled)
				{
					if (cboSort2Minor.Items.Count == 2)
					{
						cboSort2Minor.SelectedIndex = 1;
					}
					else
					{
						cboSort2Minor.SelectedIndex = 0;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboSort3Major_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loading)
				{
					_dvMinor3 = new DataView(
						_dtMinor,
						FormatFilterString(cboSort3Major.Text),
						"ArrayIndex",
						DataViewRowState.CurrentRows);

					this.cboSort3Minor.DataSource = _dvMinor3;
					this.cboSort3Minor.ValueMember = "MinorSortName";
					this.cboSort3Minor.DisplayMember = "MinorSortName";
					this.cboSort3Minor.SelectedIndex = 0;
				}

				cboSort3Minor.Enabled = (cboSort3Major.SelectedIndex > 0);
				pnlSort3.Enabled = (cboSort3Major.SelectedIndex > 0);

				if (cboSort3Minor.Enabled)
				{
					if (cboSort3Minor.Items.Count == 2)
					{
						cboSort3Minor.SelectedIndex = 1;
					}
					else
					{
						cboSort3Minor.SelectedIndex = 0;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboSortValueMajor_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loading)
				{
					_dvValueMinor = new DataView(
						_dtValueMinor,
						FormatFilterString(cboSortValueMajor.Text),
						"ArrayIndex",
						DataViewRowState.CurrentRows);

					this.cboSortValueMinor.DataSource = _dvValueMinor;
					this.cboSortValueMinor.ValueMember = "MinorSortName";
					this.cboSortValueMinor.DisplayMember = "MinorSortName";
					this.cboSortValueMinor.SelectedIndex = 0;
				}

				cboSortValueMinor.Enabled = (cboSortValueMajor.SelectedIndex > 0);

				if (cboSortValueMinor.Enabled)
				{
					if (cboSortValueMinor.Items.Count == 2)
					{
						cboSortValueMinor.SelectedIndex = 1;
					}
					else
					{
						cboSortValueMinor.SelectedIndex = 0;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private string FormatFilterString(string aRowFilter)
		{
			try
			{
				return "MajorSortName = '" + String.Format(aRowFilter.Replace("'", "''")) + "' or MajorSortName = ''";
			}
			catch
			{
				throw;
			}
		}

        private void cboSort3Major_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSort3Major_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSort2Major_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSort2Major_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSort1Major_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSort1Major_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSortValueMajor_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSortValueMajor_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
