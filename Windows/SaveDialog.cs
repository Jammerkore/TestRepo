using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
	public struct WindowSaveItem
	{
		public string Name;
		public bool SaveData;
	}
	/// <summary>
	/// Summary description for SaveDialog.
	/// </summary>
	public class SaveDialog : MIDFormBase
	{
		private System.Windows.Forms.Label lblSaveOptions;
		private System.Windows.Forms.CheckedListBox lstSaveOptions;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
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

				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.SaveDialog_Load);
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
			this.lstSaveOptions = new System.Windows.Forms.CheckedListBox();
			this.lblSaveOptions = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstSaveOptions
			// 
			this.lstSaveOptions.CheckOnClick = true;
			this.lstSaveOptions.Location = new System.Drawing.Point(8, 16);
			this.lstSaveOptions.Name = "lstSaveOptions";
			this.lstSaveOptions.Size = new System.Drawing.Size(272, 154);
			this.lstSaveOptions.TabIndex = 1;
			// 
			// lblSaveOptions
			// 
			this.lblSaveOptions.Location = new System.Drawing.Point(8, 0);
			this.lblSaveOptions.Name = "lblSaveOptions";
			this.lblSaveOptions.Size = new System.Drawing.Size(72, 16);
			this.lblSaveOptions.TabIndex = 1;
			this.lblSaveOptions.Text = "Save Options";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(120, 192);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(208, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SaveDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 230);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lblSaveOptions);
			this.Controls.Add(this.lstSaveOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SaveDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SaveDialog";
			this.Load += new System.EventHandler(this.SaveDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private ArrayList _saveList;

		public ArrayList SaveList
		{
			get
			{
				return _saveList;
			}
			set
			{
				_saveList = value;
			}
		}

		public SaveDialog(ArrayList aSaveList, string aTitle)
		{
			InitializeComponent();
			_saveList = aSaveList;
			this.Text = aTitle;
		}

		private void SaveDialog_Load(object sender, System.EventArgs e)
		{
			foreach (WindowSaveItem wsi in _saveList)
			{
				lstSaveOptions.Items.Add(wsi.Name,wsi.SaveData);
			}			
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			WindowSaveItem wsi;
			for (int i = 0; i < _saveList.Count; i++)
			{	 
				wsi = (WindowSaveItem)_saveList[i];
				wsi.SaveData = lstSaveOptions.GetItemChecked(i);
				_saveList[i] = wsi;
		 	}
			this.DialogResult = DialogResult.OK;
		}

	}
}
