//Begin Enhancement - JScott - Export Method - Part 1
// Rewritten to utilize User Controls in panels
//End Enhancement - JScott - Export Method - Part 1
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using MIDRetail.Business;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	public class RowColChooser : MIDFormBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		private eAssortmentType _assortmentType = eAssortmentType.Undefined;
		// END TT#490-MD - stodd -  post-receipts should not show placeholders

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
				foreach (RowColChooserPanel rowColPnl in _pageRowColChoosers)
				{
					rowColPnl.Dispose();
				}

				this.Load -= new System.EventHandler(this.RowColChooser_Load);
				this.cmdCancel.Click -= new System.EventHandler(this.cmdCancel_Click);
				this.cmdApply.Click -= new System.EventHandler(this.cmdApply_Click);
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
			this.cmdApply = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.tabChoosers = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.pnlButtons.SuspendLayout();
			this.tabChoosers.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(286, 9);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(72, 23);
			this.cmdApply.TabIndex = 2;
			this.cmdApply.Text = "&Apply";
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(358, 9);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(72, 23);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.cmdCancel);
			this.pnlButtons.Controls.Add(this.cmdApply);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(0, 262);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(440, 40);
			this.pnlButtons.TabIndex = 7;
			// 
			// tabChoosers
			// 
			this.tabChoosers.Controls.Add(this.tabPage1);
			this.tabChoosers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabChoosers.Location = new System.Drawing.Point(0, 0);
			this.tabChoosers.Name = "tabChoosers";
			this.tabChoosers.SelectedIndex = 0;
			this.tabChoosers.Size = new System.Drawing.Size(440, 262);
			this.tabChoosers.TabIndex = 8;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(432, 236);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			// 
			// RowColChooser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 302);
			this.Controls.Add(this.tabChoosers);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "RowColChooser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Column Chooser";
			this.Load += new System.EventHandler(this.RowColChooser_Load);
			this.pnlButtons.ResumeLayout(false);
			this.tabChoosers.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.TabControl tabChoosers;
		private System.Windows.Forms.TabPage tabPage1;

		const int cScrollDelayTime = 200;
		private int _numPages;
		private ChooserPageDefinition[] _pageDefs;
		private int _activePage;
		private RowColChooserPanel[] _pageRowColChoosers;
// Begin Track #4868 - JSmith - Variable Groupings
        private ArrayList _groupings;
// End Track #4868

// Begin Track #4868 - JSmith - Variable Groupings
		//public RowColChooser(ArrayList aHeaders, bool aOneHeaderRequired, string aTitle)
        public RowColChooser(ArrayList aHeaders, bool aOneHeaderRequired, string aTitle, ArrayList aGroupings)
// End Track #4868
        {
            try
            {
// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(aHeaders, aOneHeaderRequired, false, aTitle);
                Initialize(aHeaders, aOneHeaderRequired, false, aTitle, aGroupings);
// End Track #4868
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

// Begin Track #4868 - JSmith - Variable Groupings
        //public RowColChooser(ArrayList aHeaders, bool aOneHeaderRequired, string aTitle, bool aShowOrder)
        public RowColChooser(ArrayList aHeaders, bool aOneHeaderRequired, string aTitle, bool aShowOrder, ArrayList aGroupings)
// End Track #4868
		{
			try
			{
// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(aHeaders, aOneHeaderRequired, aShowOrder, aTitle);
                Initialize(aHeaders, aOneHeaderRequired, aShowOrder, aTitle, aGroupings);
// End Track #4868
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        //public RowColChooser(ChooserPageDefinition aPageDef, string aTitle)
        public RowColChooser(ChooserPageDefinition aPageDef, string aTitle, ArrayList aGroupings)
// End Track #4868
		{
			try
			{
// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(aPageDef, aTitle, -1);
                Initialize(aPageDef, aTitle, -1, aGroupings);
// End Track #4868
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        //public RowColChooser(ChooserPageDefinition[] aPageDefs, string aTitle, int aActivePage)
        public RowColChooser(ChooserPageDefinition[] aPageDefs, string aTitle, int aActivePage, ArrayList aGroupings)
// End Track #4868
		{
			try
			{
// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(aPageDefs, aTitle, aActivePage);
                Initialize(aPageDefs, aTitle, aActivePage, aGroupings);
// End Track #4868
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		public ArrayList Headers
		{
			get
			{
				return _pageDefs[0].ChooserPanel.Headers;
			}
		}

		public ChooserPageDefinition[] ChooserPageDefinitions
		{
			get
			{
				return _pageDefs;
			}
		}

		// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		public eAssortmentType AssortmentType
		{
			get
			{
				return _assortmentType;
			}
			set
			{
				_assortmentType = value;
			}
		}
		// END TT#490-MD - stodd -  post-receipts should not show placeholders

		public bool isAnyPageChanged
		{
			get
			{
				try
				{
					foreach (RowColChooserPanel rowPanel in _pageRowColChoosers)
					{
						if (rowPanel.isChanged)
						{
							return true;
						}
					}

					return false;
				}
				catch
				{
					throw;
				}
			}
		}

		public bool isPageChanged(int aPage)
		{
			try
			{
				return _pageRowColChoosers[aPage].isChanged;
			}
			catch
			{
				throw;
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        //private void Initialize(ArrayList aHeaders, bool aOneHeaderRequired, bool aShowOrder, string aTitle)
        private void Initialize(ArrayList aHeaders, bool aOneHeaderRequired, bool aShowOrder, string aTitle, ArrayList aGroupings)
// End Track #4868
		{
			RowColChooserPanel rowColPanel;

			try
			{
				if (aShowOrder)
				{
// Begin Track #4868 - JSmith - Variable Groupings
                    //rowColPanel = new RowColChooserOrderPanel(aHeaders, aOneHeaderRequired);
                    rowColPanel = new RowColChooserOrderPanel(aHeaders, aOneHeaderRequired, aGroupings);
// End Track #4868
				}
				else
				{
					rowColPanel = new RowColChooserSimplePanel(aHeaders, aOneHeaderRequired);
				}

// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(new ChooserPageDefinition(string.Empty, rowColPanel), aTitle, -1);
                Initialize(new ChooserPageDefinition(string.Empty, rowColPanel), aTitle, -1, aGroupings);
// End Track #4868
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        //private void Initialize(ChooserPageDefinition aPageDef, string aTitle, int aActivePage)
        private void Initialize(ChooserPageDefinition aPageDef, string aTitle, int aActivePage, ArrayList aGroupings)
// End Track #4868
		{
			try
			{
// Begin Track #4868 - JSmith - Variable Groupings
                //Initialize(new ChooserPageDefinition[] { aPageDef }, aTitle, aActivePage);
                Initialize(new ChooserPageDefinition[] { aPageDef }, aTitle, aActivePage, aGroupings);
// End Track #4868
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        //private void Initialize(ChooserPageDefinition[] aPageDefs, string aTitle, int aActivePage)
        private void Initialize(ChooserPageDefinition[] aPageDefs, string aTitle, int aActivePage, ArrayList aGroupings)
// End Track #4868
		{
			try
			{
				InitializeComponent();

				_pageDefs = aPageDefs;
				_numPages = aPageDefs.Length;
				_activePage = aActivePage;
// Begin Track #4868 - JSmith - Variable Groupings
                _groupings = aGroupings;
// End Track #4868

				this.Text = aTitle;
				_pageRowColChoosers = new RowColChooserPanel[_numPages];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void HandleExceptions(System.Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void RowColChooser_Load(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				if (_numPages == 1)
				{
					tabChoosers.Enabled = false;
					tabChoosers.Visible = false;

					_pageRowColChoosers[0] = _pageDefs[0].ChooserPanel;

					this.Controls.Add(_pageRowColChoosers[0]);
					this.Controls.SetChildIndex(_pageRowColChoosers[0], 0);
					_pageRowColChoosers[0].Dock = DockStyle.Fill;
				}
				else
				{
					tabChoosers.Enabled = true;
					tabChoosers.Visible = true;

					for (i = tabChoosers.TabCount; i < _pageDefs.Length; i++)
					{
						tabChoosers.TabPages.Add(new TabPage(i.ToString()));
					}

					i = 0;

					foreach (TabPage tabPage in tabChoosers.TabPages)
					{
						tabPage.Text = _pageDefs[i].PageTitle;
						i++;
					}

					for (i = 0; i < _numPages; i++)
					{
						_pageRowColChoosers[i] = _pageDefs[i].ChooserPanel;

						tabChoosers.TabPages[i].Controls.Add(_pageRowColChoosers[i]);
						_pageRowColChoosers[i].Dock = DockStyle.Fill;
					}

					tabChoosers.SelectedIndex = _activePage;
				}

				foreach (RowColChooserPanel rowColChooser in _pageRowColChoosers)
				{
					rowColChooser.FillControl();
				}

				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmdApply_Click(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				i = 0;

				foreach (RowColChooserPanel rowColChooser in _pageRowColChoosers)
				{
					if (!rowColChooser.ValidateData())
					{
						return;
					}
					i++;
				}

				foreach (RowColChooserPanel rowColChooser in _pageRowColChoosers)
				{
					rowColChooser.UpdateData();
				}

				this.DialogResult = DialogResult.OK;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}

	public class ChooserPageDefinition
	{
		private string _pageTitle;
		private RowColChooserPanel _chooserPanel;

		public ChooserPageDefinition(string aPageTitle, RowColChooserPanel aChooserPanel)
		{
			_pageTitle = aPageTitle;
			_chooserPanel = aChooserPanel;
		}

		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
			set
			{
				_pageTitle = value;
			}
		}

		public RowColChooserPanel ChooserPanel
		{
			get
			{
				return _chooserPanel;
			}
			set
			{
				_chooserPanel = value;
			}
		}
	}
}

