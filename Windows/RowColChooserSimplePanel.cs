using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	public class RowColChooserSimplePanel : RowColChooserPanel
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			if (disposing)
			{
				this.lstHeaders.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.lstHeaders_ItemCheck);
				this.cmiRestoreDefaults.Click -= new System.EventHandler(this.cmiRestoreDefaults_Click);
				this.cmiSelectAll.Click -= new System.EventHandler(this.cmiSelectAll_Click);
				this.cmiClearAll.Click -= new System.EventHandler(this.cmiClearAll_Click);
				this.Load -= new System.EventHandler(this.RowColChooserSimplePanel_Load);
			}

			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstHeaders = new System.Windows.Forms.CheckedListBox();
            this.cmsHeaders = new System.Windows.Forms.ContextMenu();
            this.cmiRestoreDefaults = new System.Windows.Forms.MenuItem();
            this.cmiSelectAll = new System.Windows.Forms.MenuItem();
            this.cmiClearAll = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlChooser = new System.Windows.Forms.Panel();
            this.pnlChooser.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstHeaders
            // 
            this.lstHeaders.CheckOnClick = true;
            this.lstHeaders.ContextMenu = this.cmsHeaders;
            this.lstHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHeaders.Location = new System.Drawing.Point(0, 16);
            this.lstHeaders.Name = "lstHeaders";
            this.lstHeaders.Size = new System.Drawing.Size(400, 214);
            this.lstHeaders.TabIndex = 7;
            this.lstHeaders.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstHeaders_ItemCheck);
            // 
            // cmsHeaders
            // 
            this.cmsHeaders.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmiRestoreDefaults,
            this.cmiSelectAll,
            this.cmiClearAll});
            // 
            // cmiRestoreDefaults
            // 
            this.cmiRestoreDefaults.Index = 0;
            this.cmiRestoreDefaults.Text = "Restore Defaults";
            this.cmiRestoreDefaults.Click += new System.EventHandler(this.cmiRestoreDefaults_Click);
            // 
            // cmiSelectAll
            // 
            this.cmiSelectAll.Index = 1;
            this.cmiSelectAll.Text = "Select All";
            this.cmiSelectAll.Click += new System.EventHandler(this.cmiSelectAll_Click);
            // 
            // cmiClearAll
            // 
            this.cmiClearAll.Index = 2;
            this.cmiClearAll.Text = "Clear All";
            this.cmiClearAll.Click += new System.EventHandler(this.cmiClearAll_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Displayable Items:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnlChooser
            // 
            this.pnlChooser.Controls.Add(this.lstHeaders);
            this.pnlChooser.Controls.Add(this.label1);
            this.pnlChooser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChooser.Location = new System.Drawing.Point(0, 0);
            this.pnlChooser.Name = "pnlChooser";
            this.pnlChooser.Size = new System.Drawing.Size(400, 232);
            this.pnlChooser.TabIndex = 8;
            // 
            // RowColChooserSimplePanel
            // 
            this.Controls.Add(this.pnlChooser);
            this.Name = "RowColChooserSimplePanel";
            this.Size = new System.Drawing.Size(400, 232);
            this.Load += new System.EventHandler(this.RowColChooserSimplePanel_Load);
            this.pnlChooser.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox lstHeaders;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenu cmsHeaders;
		private System.Windows.Forms.MenuItem cmiRestoreDefaults;
		private System.Windows.Forms.MenuItem cmiSelectAll;
		private System.Windows.Forms.MenuItem cmiClearAll;
		private System.Windows.Forms.Panel pnlChooser;

		const int cScrollDelayTime = 200;

		private bool _formLoading;
		private bool _changed;

		public RowColChooserSimplePanel()
		{
			_formLoading = true;
			InitializeComponent();
			_changed = false;
			Initialize();
		}

		public RowColChooserSimplePanel(ArrayList aHeaders, bool aOneHeaderRequired)
			: base(aHeaders, aOneHeaderRequired)
		{
			_formLoading = true;
			InitializeComponent();
			_changed = false;
			Initialize();
		}

		override public bool isChanged
		{
			get
			{
				return _changed;
			}
		}

		private void Initialize()
		{
		}

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void RowColChooserSimplePanel_Load(object sender, EventArgs e)
		{
			try
			{
				_formLoading = false;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiClearAll_Click(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				for (i = 0; i < _headers.Count; i++)
				{
					lstHeaders.SetItemChecked(i, false);
//Begin Modification - JScott - Export Method - Part 9
//					lstHeaders.SetItemChecked(i, false);
					if (((RowColHeader)_headers[i]).IsSelectable)
					{
						lstHeaders.SetItemChecked(i, false);
					}
//End Modification - JScott - Export Method - Part 9
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiSelectAll_Click(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				for (i = 0; i < _headers.Count; i++)
				{
//Begin Modification - JScott - Export Method - Part 9
//					lstHeaders.SetItemChecked(i, true);
					if (((RowColHeader)_headers[i]).IsSelectable)
					{
						lstHeaders.SetItemChecked(i, true);
					}
//End Modification - JScott - Export Method - Part 9
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmiRestoreDefaults_Click(object sender, System.EventArgs e)
		{
			try
			{
				FillControl();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void lstHeaders_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				if (!_formLoading)
				{
					_changed = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override public void FillControl()
		{
			try
			{
				_formLoading = true;

				lstHeaders.BeginUpdate();

				try
				{
					lstHeaders.Items.Clear();

					foreach (RowColHeader header in _headers)
					{
//Begin Modification - JScott - Export Method - Part 9
//						lstHeaders.Items.Add(new ListBoxItem(header, header.Name), header.IsDisplayed);
						if (header.IsSelectable)
						{
							lstHeaders.Items.Add(new ListBoxItem(header, header.Name), header.IsDisplayed);
						}
//End Modification - JScott - Export Method - Part 9
					}
				}
				finally
				{
					lstHeaders.EndUpdate();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_changed = false;
				_formLoading = false;
			}
		}

		override public bool ValidateData()
		{
			int i;
			int itemsChecked;

			try
			{
				if (_oneHeaderRequired)
				{
					i = 0;
					itemsChecked = 0;

					foreach (RowColHeader header in _headers)
					{
//Begin Modification - JScott - Export Method - Part 9
//						if (lstHeaders.GetItemChecked(i))
						if (header.IsSelectable && lstHeaders.GetItemChecked(i))
//End Modification - JScott - Export Method - Part 9
						{
							itemsChecked++;
							break;
						}

						i++;
					}

					if (itemsChecked == 0)
					{
						MessageBox.Show("You cannot hide all items.  You must choose at least one to display.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void UpdateData()
		{
			int i;

			try
			{
				i = 0;

				foreach (RowColHeader header in _headers)
				{
//Begin Modification - JScott - Export Method - Part 9
//					header.IsDisplayed = lstHeaders.GetItemChecked(i);
//					i++;
					if (header.IsSelectable)
					{
						header.IsDisplayed = lstHeaders.GetItemChecked(i);
						i++;
					}
//End Modification - JScott - Export Method - Part 9
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Modification - JScott - Export Method - Fix 1
		override public void ResetChangedFlag()
		{
			_changed = false;
		}

//End Modification - JScott - Export Method - Fix 1
		private class ListBoxItem : ICloneable
		{
			object _value;
			string _displayValue;

			public object Value
			{
				get
				{
					return _value;
				}
			}

			override public string ToString()
			{
				return _displayValue;
			}

			public override bool Equals(object obj)
			{
				return ((ListBoxItem)obj).Value == _value;
			}

			override public int GetHashCode()
			{
				try
				{
					return base.GetHashCode();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public ListBoxItem(object aValue, string aDisplayValue)
			{
				_value = aValue;
				_displayValue = aDisplayValue;
			}

			public object Clone()
			{
				ListBoxItem newItem;

				try
				{
					newItem = new ListBoxItem(_value, _displayValue);
					return newItem;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}
}