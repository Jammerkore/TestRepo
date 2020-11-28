//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//
//using MIDRetail.Business;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//
//
//
//namespace MIDRetail.Windows
//{
//	/// <summary>
//	/// Summary description for DailyPctModelMaint.
//	/// </summary>
//	public class frmDailyPctModelMaint : System.Windows.Forms.Form
//	{
//		private SessionAddressBlock _SAB;
//		private bool _changePending = false;
//		private bool _cancelClicked = false;
//		private bool _newModel = false;
//		private bool _changeMade = false;
//		private bool _formLoaded = false;
//		private int _modelRID = -1;
//		private int _modelIndex = 0;
//		private string _saveAsName = "";
//		private CalendarDateSelector _frmCalDtSelector;
//		private DataTable _DailyPctModelTable;
//
//		private System.Windows.Forms.Button btnCancel;
//		private System.Windows.Forms.Button btnHelp;
//		private System.Windows.Forms.Label lblModelName;
//		private System.Windows.Forms.Button btnDelete;
//		private System.Windows.Forms.Button btnNew;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboDailyPctModelName;
//		private System.Windows.Forms.Button btnSave;
//		private System.Windows.Forms.Label lblDay1;
//		private System.Windows.Forms.NumericUpDown numDay1;
//		private System.Windows.Forms.NumericUpDown numDay2;
//		private System.Windows.Forms.Label lblDay2;
//		private System.Windows.Forms.NumericUpDown numDay3;
//		private System.Windows.Forms.Label lblDay3;
//		private System.Windows.Forms.NumericUpDown numDay4;
//		private System.Windows.Forms.Label lblDay4;
//		private System.Windows.Forms.NumericUpDown numDay5;
//		private System.Windows.Forms.Label lblDay5;
//		private System.Windows.Forms.NumericUpDown numDay6;
//		private System.Windows.Forms.Label lblDay6;
//		private System.Windows.Forms.NumericUpDown numDay7;
//		private System.Windows.Forms.Label lblDay7;
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;
//
//		public bool ChangeMade 
//		{
//			get { return _changeMade ; }
//			set { _changeMade = value; }
//		}
//
//		public frmDailyPctModelMaint(SessionAddressBlock aSAB)
//		{
//			//
//			// Required for Windows Form Designer support
//			//
//			InitializeComponent();
//
//			_SAB = aSAB;
////			_DailyPctModelTable = _SAB.HierarchyServerSession.GetDailyPctModels();
//			this.cboDailyPctModelName.DataSource = _DailyPctModelTable;
//			this.cboDailyPctModelName.DisplayMember = "Model Name";
//			this.cboDailyPctModelName.ValueMember = "Model Name";
//		}
//
//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			if( disposing )
//			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
//			}
//			base.Dispose( disposing );
//		}
//
//		#region Windows Form Designer generated code
//		/// <summary>
//		/// Required method for Designer support - do not modify
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.btnCancel = new System.Windows.Forms.Button();
//			this.btnHelp = new System.Windows.Forms.Button();
//			this.btnDelete = new System.Windows.Forms.Button();
//			this.btnNew = new System.Windows.Forms.Button();
//			this.cboDailyPctModelName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.lblModelName = new System.Windows.Forms.Label();
//			this.btnSave = new System.Windows.Forms.Button();
//			this.lblDay1 = new System.Windows.Forms.Label();
//			this.numDay1 = new System.Windows.Forms.NumericUpDown();
//			this.numDay2 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay2 = new System.Windows.Forms.Label();
//			this.numDay3 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay3 = new System.Windows.Forms.Label();
//			this.numDay4 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay4 = new System.Windows.Forms.Label();
//			this.numDay5 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay5 = new System.Windows.Forms.Label();
//			this.numDay6 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay6 = new System.Windows.Forms.Label();
//			this.numDay7 = new System.Windows.Forms.NumericUpDown();
//			this.lblDay7 = new System.Windows.Forms.Label();
//			((System.ComponentModel.ISupportInitialize)(this.numDay1)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay2)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay3)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay4)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay5)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay6)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay7)).BeginInit();
//			this.SuspendLayout();
//			// 
//			// btnCancel
//			// 
//			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//			this.btnCancel.Location = new System.Drawing.Point(464, 312);
//			this.btnCancel.Name = "btnCancel";
//			this.btnCancel.TabIndex = 1;
//			this.btnCancel.Text = "Cancel";
//			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//			// 
//			// btnHelp
//			// 
//			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//			this.btnHelp.Location = new System.Drawing.Point(16, 312);
//			this.btnHelp.Name = "btnHelp";
//			this.btnHelp.Size = new System.Drawing.Size(24, 23);
//			this.btnHelp.TabIndex = 12;
//			this.btnHelp.Text = "?";
//			// 
//			// btnDelete
//			// 
//			this.btnDelete.Location = new System.Drawing.Point(464, 16);
//			this.btnDelete.Name = "btnDelete";
//			this.btnDelete.TabIndex = 17;
//			this.btnDelete.Text = "Delete";
//			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
//			// 
//			// btnNew
//			// 
//			this.btnNew.Location = new System.Drawing.Point(384, 16);
//			this.btnNew.Name = "btnNew";
//			this.btnNew.TabIndex = 16;
//			this.btnNew.Text = "New";
//			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
//			// 
//			// cboDailyPctModelName
//			// 
//			this.cboDailyPctModelName.Location = new System.Drawing.Point(112, 17);
//			this.cboDailyPctModelName.Name = "cboDailyPctModelName";
//			this.cboDailyPctModelName.Size = new System.Drawing.Size(184, 21);
//			this.cboDailyPctModelName.TabIndex = 15;
//			this.cboDailyPctModelName.SelectionChangeCommitted += new System.EventHandler(this.cboDailyPctModelName_SelectionChangeCommitted);
//			// 
//			// lblModelName
//			// 
//			this.lblModelName.Location = new System.Drawing.Point(8, 16);
//			this.lblModelName.Name = "lblModelName";
//			this.lblModelName.TabIndex = 14;
//			this.lblModelName.Text = "Model Name:";
//			this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// btnSave
//			// 
//			this.btnSave.Location = new System.Drawing.Point(304, 16);
//			this.btnSave.Name = "btnSave";
//			this.btnSave.TabIndex = 27;
//			this.btnSave.Text = "Save";
//			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//			// 
//			// lblDay1
//			// 
//			this.lblDay1.Location = new System.Drawing.Point(196, 64);
//			this.lblDay1.Name = "lblDay1";
//			this.lblDay1.Size = new System.Drawing.Size(64, 23);
//			this.lblDay1.TabIndex = 29;
//			this.lblDay1.Text = "Day 1:";
//			this.lblDay1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay1
//			// 
//			this.numDay1.Location = new System.Drawing.Point(284, 64);
//			this.numDay1.Name = "numDay1";
//			this.numDay1.Size = new System.Drawing.Size(72, 20);
//			this.numDay1.TabIndex = 30;
//			this.numDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// numDay2
//			// 
//			this.numDay2.Location = new System.Drawing.Point(284, 96);
//			this.numDay2.Name = "numDay2";
//			this.numDay2.Size = new System.Drawing.Size(72, 20);
//			this.numDay2.TabIndex = 32;
//			this.numDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay2
//			// 
//			this.lblDay2.Location = new System.Drawing.Point(196, 96);
//			this.lblDay2.Name = "lblDay2";
//			this.lblDay2.Size = new System.Drawing.Size(64, 23);
//			this.lblDay2.TabIndex = 31;
//			this.lblDay2.Text = "Day 2:";
//			this.lblDay2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay3
//			// 
//			this.numDay3.Location = new System.Drawing.Point(284, 128);
//			this.numDay3.Name = "numDay3";
//			this.numDay3.Size = new System.Drawing.Size(72, 20);
//			this.numDay3.TabIndex = 34;
//			this.numDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay3
//			// 
//			this.lblDay3.Location = new System.Drawing.Point(196, 128);
//			this.lblDay3.Name = "lblDay3";
//			this.lblDay3.Size = new System.Drawing.Size(64, 23);
//			this.lblDay3.TabIndex = 33;
//			this.lblDay3.Text = "Day 3:";
//			this.lblDay3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay4
//			// 
//			this.numDay4.Location = new System.Drawing.Point(284, 160);
//			this.numDay4.Name = "numDay4";
//			this.numDay4.Size = new System.Drawing.Size(72, 20);
//			this.numDay4.TabIndex = 36;
//			this.numDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay4
//			// 
//			this.lblDay4.Location = new System.Drawing.Point(196, 160);
//			this.lblDay4.Name = "lblDay4";
//			this.lblDay4.Size = new System.Drawing.Size(64, 23);
//			this.lblDay4.TabIndex = 35;
//			this.lblDay4.Text = "Day 4:";
//			this.lblDay4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay5
//			// 
//			this.numDay5.Location = new System.Drawing.Point(284, 192);
//			this.numDay5.Name = "numDay5";
//			this.numDay5.Size = new System.Drawing.Size(72, 20);
//			this.numDay5.TabIndex = 38;
//			this.numDay5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay5
//			// 
//			this.lblDay5.Location = new System.Drawing.Point(196, 192);
//			this.lblDay5.Name = "lblDay5";
//			this.lblDay5.Size = new System.Drawing.Size(64, 23);
//			this.lblDay5.TabIndex = 37;
//			this.lblDay5.Text = "Day 5:";
//			this.lblDay5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay6
//			// 
//			this.numDay6.Location = new System.Drawing.Point(284, 224);
//			this.numDay6.Name = "numDay6";
//			this.numDay6.Size = new System.Drawing.Size(72, 20);
//			this.numDay6.TabIndex = 40;
//			this.numDay6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay6
//			// 
//			this.lblDay6.Location = new System.Drawing.Point(196, 224);
//			this.lblDay6.Name = "lblDay6";
//			this.lblDay6.Size = new System.Drawing.Size(64, 23);
//			this.lblDay6.TabIndex = 39;
//			this.lblDay6.Text = "Day 6:";
//			this.lblDay6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// numDay7
//			// 
//			this.numDay7.Location = new System.Drawing.Point(284, 256);
//			this.numDay7.Name = "numDay7";
//			this.numDay7.Size = new System.Drawing.Size(72, 20);
//			this.numDay7.TabIndex = 42;
//			this.numDay7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//			// 
//			// lblDay7
//			// 
//			this.lblDay7.Location = new System.Drawing.Point(196, 256);
//			this.lblDay7.Name = "lblDay7";
//			this.lblDay7.Size = new System.Drawing.Size(64, 23);
//			this.lblDay7.TabIndex = 41;
//			this.lblDay7.Text = "Day 7:";
//			this.lblDay7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// frmDailyPctModelMaint
//			// 
//			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//			this.ClientSize = new System.Drawing.Size(552, 350);
//			this.Controls.Add(this.numDay7);
//			this.Controls.Add(this.lblDay7);
//			this.Controls.Add(this.numDay6);
//			this.Controls.Add(this.lblDay6);
//			this.Controls.Add(this.numDay5);
//			this.Controls.Add(this.lblDay5);
//			this.Controls.Add(this.numDay4);
//			this.Controls.Add(this.lblDay4);
//			this.Controls.Add(this.numDay3);
//			this.Controls.Add(this.lblDay3);
//			this.Controls.Add(this.numDay2);
//			this.Controls.Add(this.lblDay2);
//			this.Controls.Add(this.numDay1);
//			this.Controls.Add(this.lblDay1);
//			this.Controls.Add(this.btnSave);
//			this.Controls.Add(this.btnDelete);
//			this.Controls.Add(this.btnNew);
//			this.Controls.Add(this.cboDailyPctModelName);
//			this.Controls.Add(this.lblModelName);
//			this.Controls.Add(this.btnHelp);
//			this.Controls.Add(this.btnCancel);
//			this.Name = "frmDailyPctModelMaint";
//			this.Text = "Daily Percentages Model Maintenance";
//			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDailyPctModelMaint_Closing);
//			this.Load += new System.EventHandler(this.frmDailyPctModelMaint_Load);
//			((System.ComponentModel.ISupportInitialize)(this.numDay1)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay2)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay3)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay4)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay5)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay6)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.numDay7)).EndInit();
//			this.ResumeLayout(false);
//
//		}
//		#endregion
//
//		private void frmDailyPctModelMaint_Load(object sender, System.EventArgs e)
//		{
//			_formLoaded = true;
//			if (_newModel)
//			{
//				this.cboDailyPctModelName.SelectedValue = "(new model)";
//				this.cboDailyPctModelName.Text = "(new model)";
//			}
//		}
//
//		public void InitializeForm()
//		{
//			_newModel = true;
//			_modelRID = -1;
//			_saveAsName = "";
//			this.cboDailyPctModelName.SelectedValue = "(new model)";
//			this.cboDailyPctModelName.Text = "(new model)";
//		}
//
//		public void InitializeForm(string currModel)
//		{
//			_newModel = false;
//			this.cboDailyPctModelName.SelectedValue = currModel;
//			_saveAsName = currModel;
////			DailyPctModelProfile dpmp = _SAB.HierarchyServerSession.GetDailyPctModelData(currModel);
//			_modelRID = dpmp.Key;
//			int dayCounter = 0;
//			foreach (DailyPctModelEntry dpme in dpmp.ModelEntries)
//			{
//				switch (dayCounter)
//				{
//					case 0:
//						this.numDay1.Value = (decimal)dpme.DailyValue;
//						break;
//					case 1:
//						this.numDay2.Value = (decimal)dpme.DailyValue;
//						break;
//					case 2:
//						this.numDay3.Value = (decimal)dpme.DailyValue;
//						break;
//					case 3:
//						this.numDay4.Value = (decimal)dpme.DailyValue;
//						break;
//					case 4:
//						this.numDay5.Value = (decimal)dpme.DailyValue;
//						break;
//					case 5:
//						this.numDay6.Value = (decimal)dpme.DailyValue;
//						break;
//					case 6:
//						this.numDay7.Value = (decimal)dpme.DailyValue;
//						break;
//				}
//				++dayCounter;
//			}
//
//		}
//
//
//		private void btnCancel_Click(object sender, System.EventArgs e)
//		{
//			_cancelClicked = true;
//			CheckForPendingChanges();
//			this.Close();
//		}
//
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			saveChanges();
//		}
//
//		private void btnNew_Click(object sender, System.EventArgs e)
//		{
//			CheckForPendingChanges();
//			InitializeForm();
//		}
//
//		private void btnDelete_Click(object sender, System.EventArgs e)
//		{
//			DailyPctModelProfile dpmp = new DailyPctModelProfile(_modelRID);
//			dpmp.ModelChangeType = eChangeType.delete;
//			dpmp.Key = _modelRID;
//			_SAB.HierarchyServerSession.DailyPctModelUpdate(dpmp);
//			_changeMade = true;
//			_DailyPctModelTable.Clear();
//			_DailyPctModelTable = _SAB.HierarchyServerSession.GetDailyPctModels();
//			if (_DailyPctModelTable.Rows.Count > 0)
//			{
//				if (_modelIndex > 0)
//				{
//					--_modelIndex;
//				}
//				this.cboDailyPctModelName.DataSource = _DailyPctModelTable;
//				DataRow dr = _DailyPctModelTable.Rows[_modelIndex];
//				string modelName = (string)(dr["Model Name"]);
//				InitializeForm(modelName);
//			}
//			else
//			{
//				InitializeForm();
//			}
//		}
//
//		private void saveChanges()
//		{
//			bool saveAsCanceled = false;
//			bool errorFound = false;
//
//			DailyPctModelProfile dpmp = new DailyPctModelProfile(_modelRID);
//			if (_newModel)
//			{
//				dpmp.ModelChangeType = eChangeType.add;
//			}
//			else
//			{
//				dpmp.ModelChangeType = eChangeType.update;
//			}
//			dpmp.ModelEntries = new ArrayList();
//			int entrySeq = 0;
//			double total = 0;
//			AddDailyValue(dpmp, (double)numDay1.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay2.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay3.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay4.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay5.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay6.Value, ref total, entrySeq);
//			++entrySeq;
//			AddDailyValue(dpmp, (double)numDay7.Value, ref total, entrySeq);
//			++entrySeq;
//
//			if (total != 100)
//			{
//				MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustEqual100),  this.Text,
//					MessageBoxButtons.OK, MessageBoxIcon.Error); 
//				errorFound = true;
//			}
//			if (!errorFound)
//			{
//				if (_newModel && (_saveAsName == ""))
//				{
//					frmSaveAs formSaveAs = new frmSaveAs(_SAB);
//					formSaveAs.ShowDialog(this);
//					saveAsCanceled = formSaveAs.SaveCanceled;
//					DailyPctModelProfile checkExists = _SAB.HierarchyServerSession.GetDailyPctModelData(formSaveAs.SaveAsName);
//					if (checkExists.Key == -1)
//					{
//						_saveAsName = formSaveAs.SaveAsName;
//					}
//					else
//					{
//						if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
//							MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//							== DialogResult.Yes) 
//						{
//							dpmp.ModelChangeType = eChangeType.update;
//							dpmp.Key = checkExists.Key;
//						}
//						else
//						{
//							saveAsCanceled = true;
//						}
//
//					}
//				}
//
//				if (!saveAsCanceled)
//				{
//					dpmp.ModelID = _saveAsName;
//					dpmp.Key = _modelRID;
//					_SAB.HierarchyServerSession.DailyPctModelUpdate(dpmp);
//					_changeMade = true;
//					_changePending = false;
//					if (_newModel)
//					{
//						_DailyPctModelTable = _SAB.HierarchyServerSession.GetDailyPctModels();
//						this.cboDailyPctModelName.DataSource = _DailyPctModelTable;
//						this.cboDailyPctModelName.SelectedValue = _saveAsName;
//						_newModel = false;
//					}
//				}
//				else
//				{
//					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
//						MessageBoxButtons.OK, MessageBoxIcon.Information);
//				}
//			}
//		}
//
//		static private void AddDailyValue(DailyPctModelProfile dpmp, double dailyValue, ref double total, int entrySeq)
//		{
//			DailyPctModelEntry modelEntry = new DailyPctModelEntry();
//			modelEntry.ModelEntrySeq = entrySeq;
//			modelEntry.ModelEntryChangeType = eChangeType.add;
//			modelEntry.DailyValue = dailyValue;
//			dpmp.ModelEntries.Add(modelEntry);
//			total += dailyValue;
//		}
//
//		private void frmDailyPctModelMaint_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//		{
//			if (!_cancelClicked)
//			{
//				CheckForPendingChanges();
//			}
//		}
//
//		private void ugDailyPctModel_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//		{
//			_frmCalDtSelector = new CalendarDateSelector(_SAB);
//			DialogResult DateRangeResult = _frmCalDtSelector.ShowDialog();
//			if (DateRangeResult == DialogResult.OK)
//			{
//				DateRangeProfile SelectedDateRange = (DateRangeProfile)_frmCalDtSelector.Tag;
//				e.Cell.SelText = SelectedDateRange.DisplayDate;
//				e.Cell.Tag = SelectedDateRange;
//				_changePending = true;
//			}
//		}
//
//		private void cboDailyPctModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//			if (_formLoaded)
//			{
//				CheckForPendingChanges();
//				if (cboDailyPctModelName.SelectedIndex >= 0)
//				{
//					CheckForPendingChanges();
//					_modelIndex = cboDailyPctModelName.SelectedIndex;
//					DataRow dr = _DailyPctModelTable.Rows[cboDailyPctModelName.SelectedIndex];
//					string modelName = (string)(dr["Model Name"]);
//					InitializeForm(modelName);
//				}
//			}
//		}
//
//		private void CheckForPendingChanges()
//		{
//			if (_changePending)
//			{
//				if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
//					MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//					== DialogResult.Yes) 
//				{
//					saveChanges();
//				}
//				_changePending = false;
//			}
//		}
//	}
//}
