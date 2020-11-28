using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmUltraGridSearchReplace.
	/// </summary>
	public class frmUltraGridSearchReplace : System.Windows.Forms.Form
	{
        internal System.Windows.Forms.CheckBox chkMatchCase;
		internal MIDRetail.Windows.Controls.MIDComboBoxEnh cbxSearchDirection;
		internal System.Windows.Forms.Label lblSearchDirection;
		internal MIDRetail.Windows.Controls.MIDComboBoxEnh cbxMatch;
		internal System.Windows.Forms.Label lblMatch;
		internal MIDRetail.Windows.Controls.MIDComboBoxEnh cbxLookIn;
		internal System.Windows.Forms.Label lblLookIn;
		internal MIDRetail.Windows.Controls.MIDComboBoxEnh cbxFindWhat;
		internal System.Windows.Forms.Label lblFindWhat;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private bool _allowReplace = false;
		private bool _replacePerformed = false;
		private bool _continueReplace = false;
		private bool _foundText = false;
		private bool _performingReplaceAll = false;
		private int _replaceCount = 0;
		private int _textStart = 0;
		private int _textLength = 0;
		private SessionAddressBlock _SAB;
		private Infragistics.Win.UltraWinGrid.UltraGrid _searchGrid;
		private Infragistics.Win.UltraWinGrid.UltraGridColumn _searchColumn;
		private Infragistics.Win.UltraWinGrid.UltraGridColumn _gridCol;
		private Infragistics.Win.UltraWinGrid.UltraGridBand _gridBand;
		private string _columnName;
		private Hashtable _captionToSearchColumn = new Hashtable();
		private SearchReplaceInfo _searchReplaceInfo;
		private bool _firstSearch = true;
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.Button btnFindNext;
		private System.Windows.Forms.Button btnReplace;
		private System.Windows.Forms.Button btnReplaceAll;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxReplaceWith;
		private System.Windows.Forms.Label lblReplaceWith;
		private bool _multipleBands = false;

		/// <summary>
		/// Gets the flag identifying if a replace was done.
		/// </summary>
		public bool ReplacePerformed 
		{
			get { return _replacePerformed ; }
		}

		public frmUltraGridSearchReplace(SessionAddressBlock aSAB, bool aAllowReplace)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_SAB = aSAB;
			_allowReplace = aAllowReplace;
		}

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
                this.cbxFindWhat.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxFindWhat_MIDComboBoxPropertiesChangedEvent);
                this.cbxReplaceWith.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxReplaceWith_MIDComboBoxPropertiesChangedEvent);
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
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.cbxSearchDirection = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblSearchDirection = new System.Windows.Forms.Label();
            this.cbxMatch = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblMatch = new System.Windows.Forms.Label();
            this.cbxLookIn = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblLookIn = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.cbxFindWhat = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblFindWhat = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.cbxReplaceWith = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblReplaceWith = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.Location = new System.Drawing.Point(272, 160);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(96, 24);
            this.chkMatchCase.TabIndex = 21;
            this.chkMatchCase.Text = "Match Case";
            // 
            // cbxSearchDirection
            // 
            this.cbxSearchDirection.AutoAdjust = true;
            this.cbxSearchDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxSearchDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxSearchDirection.DataSource = null;
            this.cbxSearchDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSearchDirection.DropDownWidth = 168;
            this.cbxSearchDirection.FormattingEnabled = false;
            this.cbxSearchDirection.ItemHeight = 13;
            this.cbxSearchDirection.Location = new System.Drawing.Point(88, 160);
            this.cbxSearchDirection.Margin = new System.Windows.Forms.Padding(0);
            this.cbxSearchDirection.MaxDropDownItems = 8;
            this.cbxSearchDirection.Name = "cbxSearchDirection";
            this.cbxSearchDirection.Size = new System.Drawing.Size(168, 21);
            this.cbxSearchDirection.TabIndex = 20;
            this.cbxSearchDirection.Tag = null;
            // 
            // lblSearchDirection
            // 
            this.lblSearchDirection.AutoSize = true;
            this.lblSearchDirection.Location = new System.Drawing.Point(40, 162);
            this.lblSearchDirection.Name = "lblSearchDirection";
            this.lblSearchDirection.Size = new System.Drawing.Size(44, 13);
            this.lblSearchDirection.TabIndex = 19;
            this.lblSearchDirection.Text = "Search:";
            // 
            // cbxMatch
            // 
            this.cbxMatch.AutoAdjust = true;
            this.cbxMatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxMatch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxMatch.DataSource = null;
            this.cbxMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMatch.DropDownWidth = 168;
            this.cbxMatch.FormattingEnabled = false;
            this.cbxMatch.ItemHeight = 13;
            this.cbxMatch.Location = new System.Drawing.Point(88, 104);
            this.cbxMatch.Margin = new System.Windows.Forms.Padding(0);
            this.cbxMatch.MaxDropDownItems = 8;
            this.cbxMatch.Name = "cbxMatch";
            this.cbxMatch.Size = new System.Drawing.Size(168, 21);
            this.cbxMatch.TabIndex = 18;
            this.cbxMatch.Tag = null;
            // 
            // lblMatch
            // 
            this.lblMatch.AutoSize = true;
            this.lblMatch.Location = new System.Drawing.Point(48, 106);
            this.lblMatch.Name = "lblMatch";
            this.lblMatch.Size = new System.Drawing.Size(40, 13);
            this.lblMatch.TabIndex = 17;
            this.lblMatch.Text = "Match:";
            // 
            // cbxLookIn
            // 
            this.cbxLookIn.AutoAdjust = true;
            this.cbxLookIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxLookIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxLookIn.DataSource = null;
            this.cbxLookIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLookIn.DropDownWidth = 168;
            this.cbxLookIn.FormattingEnabled = false;
            this.cbxLookIn.ItemHeight = 13;
            this.cbxLookIn.Location = new System.Drawing.Point(88, 70);
            this.cbxLookIn.Margin = new System.Windows.Forms.Padding(0);
            this.cbxLookIn.MaxDropDownItems = 8;
            this.cbxLookIn.Name = "cbxLookIn";
            this.cbxLookIn.Size = new System.Drawing.Size(168, 21);
            this.cbxLookIn.TabIndex = 16;
            this.cbxLookIn.Tag = null;
            // 
            // lblLookIn
            // 
            this.lblLookIn.AutoSize = true;
            this.lblLookIn.Location = new System.Drawing.Point(40, 72);
            this.lblLookIn.Name = "lblLookIn";
            this.lblLookIn.Size = new System.Drawing.Size(46, 13);
            this.lblLookIn.TabIndex = 15;
            this.lblLookIn.Text = "Look In:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(392, 112);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(392, 16);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(75, 23);
            this.btnFindNext.TabIndex = 13;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // cbxFindWhat
            // 
            this.cbxFindWhat.AutoAdjust = true;
            this.cbxFindWhat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbxFindWhat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxFindWhat.DataSource = null;
            this.cbxFindWhat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbxFindWhat.DropDownWidth = 248;
            this.cbxFindWhat.FormattingEnabled = false;
            this.cbxFindWhat.ItemHeight = 13;
            this.cbxFindWhat.Location = new System.Drawing.Point(88, 8);
            this.cbxFindWhat.Margin = new System.Windows.Forms.Padding(0);
            this.cbxFindWhat.MaxDropDownItems = 8;
            this.cbxFindWhat.Name = "cbxFindWhat";
            this.cbxFindWhat.Size = new System.Drawing.Size(248, 21);
            this.cbxFindWhat.TabIndex = 1;
            this.cbxFindWhat.Tag = null;
            this.cbxFindWhat.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbxFindWhat_MIDComboBoxPropertiesChangedEvent);
            this.cbxFindWhat.SelectionChangeCommitted += new System.EventHandler(this.cbxFindWhat_SelectionChangeCommitted);
            this.cbxFindWhat.TextChanged += new System.EventHandler(this.cbxFindWhat_TextChanged);
            // 
            // lblFindWhat
            // 
            this.lblFindWhat.AutoSize = true;
            this.lblFindWhat.Location = new System.Drawing.Point(24, 10);
            this.lblFindWhat.Name = "lblFindWhat";
            this.lblFindWhat.Size = new System.Drawing.Size(59, 13);
            this.lblFindWhat.TabIndex = 0;
            this.lblFindWhat.Text = "Find What:";
            this.lblFindWhat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(392, 48);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
            this.btnReplace.TabIndex = 22;
            this.btnReplace.Text = "Replace";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(392, 80);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.btnReplaceAll.TabIndex = 23;
            this.btnReplaceAll.Text = "Replace All";
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // cbxReplaceWith
            // 
            this.cbxReplaceWith.AutoAdjust = true;
            this.cbxReplaceWith.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cbxReplaceWith.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxReplaceWith.DataSource = null;
            this.cbxReplaceWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cbxReplaceWith.DropDownWidth = 248;
            this.cbxReplaceWith.FormattingEnabled = false;
            this.cbxReplaceWith.ItemHeight = 13;
            this.cbxReplaceWith.Location = new System.Drawing.Point(88, 38);
            this.cbxReplaceWith.Margin = new System.Windows.Forms.Padding(0);
            this.cbxReplaceWith.MaxDropDownItems = 8;
            this.cbxReplaceWith.Name = "cbxReplaceWith";
            this.cbxReplaceWith.Size = new System.Drawing.Size(248, 21);
            this.cbxReplaceWith.TabIndex = 3;
            this.cbxReplaceWith.Tag = null;
            this.cbxReplaceWith.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbxReplaceWith_MIDComboBoxPropertiesChangedEvent);
            this.cbxReplaceWith.SelectionChangeCommitted += new System.EventHandler(this.cbxReplaceWith_SelectionChangeCommitted);
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.AutoSize = true;
            this.lblReplaceWith.Location = new System.Drawing.Point(8, 40);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(75, 13);
            this.lblReplaceWith.TabIndex = 2;
            this.lblReplaceWith.Text = "Replace With:";
            this.lblReplaceWith.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmUltraGridSearchReplace
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(482, 197);
            this.Controls.Add(this.lblReplaceWith);
            this.Controls.Add(this.cbxReplaceWith);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.cbxSearchDirection);
            this.Controls.Add(this.lblSearchDirection);
            this.Controls.Add(this.lblMatch);
            this.Controls.Add(this.lblLookIn);
            this.Controls.Add(this.lblFindWhat);
            this.Controls.Add(this.cbxMatch);
            this.Controls.Add(this.cbxLookIn);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.cbxFindWhat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUltraGridSearchReplace";
            this.Text = "Search";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmUltraGridSearchReplace_Closing);
            this.Load += new System.EventHandler(this.frmUltraGridSearchReplace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion



		private void PopulateLookInCombo()
		{
			this.cbxLookIn.Items.Clear();
			this.cbxLookIn.Items.Add(_columnName);
			this.cbxLookIn.Items.Add("All columns");
			this.cbxLookIn.SelectedIndex = 0;
		}

		private void PopulateSearchDirectionCombo()
		{
			this.cbxSearchDirection.Items.Clear();

			Array values;
			string [] names;

			values = System.Enum.GetValues(typeof(eSearchDirection));
			names = System.Enum.GetNames(typeof(eSearchDirection));

			for ( int i = 0; i < names.Length; i ++ )
			{
				this.cbxSearchDirection.Items.Add(names[i]);
			}

			this.cbxSearchDirection.Tag = values;
			this.cbxSearchDirection.SelectedIndex = 0;

		}

		private void PopulateSearchContentCombo()
		{
			this.cbxMatch.Items.Clear();

			Array values;
			string [] names;

			values = System.Enum.GetValues(typeof(eSearchContent));
			names = System.Enum.GetNames(typeof(eSearchContent));

			for ( int i = 0; i < names.Length; i ++ )
			{
				this.cbxMatch.Items.Add(names[i]);
			}

			this.cbxMatch.Tag = values;
			this.cbxMatch.SelectedIndex = 0;

		}

		private void ProcessSearch()
		{
			//   Set the form's SearchReplaceInfo properties
			SearchReplaceInfo.searchString = this.cbxFindWhat.Text;
			Array a = (Array)this.cbxSearchDirection.Tag;
			SearchReplaceInfo.searchDirection = (eSearchDirection)a.GetValue( this.cbxSearchDirection.SelectedIndex );
			a = (Array)this.cbxMatch.Tag;
			SearchReplaceInfo.searchContent = (eSearchContent)a.GetValue( this.cbxMatch.SelectedIndex );

			SearchReplaceInfo.matchCase = this.chkMatchCase.Checked;
			if (this.cbxLookIn.Text == "All columns")
			{
				SearchReplaceInfo.lookIn = this.cbxLookIn.Text;
			}
			else
			{
				if (_multipleBands)
				{
					// translate column caption to key
					UltraGridColumn searchColumn = (UltraGridColumn)_captionToSearchColumn[this.cbxLookIn.Text];
					SearchReplaceInfo.lookIn = searchColumn.Key;
				}
				else
				{
					SearchReplaceInfo.lookIn = _columnName;
				}
			}


			//   Add the search string to the combobox
			//   Also limit its capacity to 10 items
			if ( ! this.cbxFindWhat.Items.Contains(this.cbxFindWhat.Text) )
			{
				this.cbxFindWhat.Items.Insert(0, this.cbxFindWhat.Text);
				if ( this.cbxFindWhat.Items.Count > 10 )
					this.cbxFindWhat.Items.RemoveAt(10);

			}

			if ( ! this.cbxReplaceWith.Items.Contains(this.cbxReplaceWith.Text) )
			{
				this.cbxReplaceWith.Items.Insert(0, this.cbxReplaceWith.Text);
				if ( this.cbxReplaceWith.Items.Count > 10 )
					this.cbxReplaceWith.Items.RemoveAt(10);

			}

			//	Call the Search method
			Search(_searchGrid.ActiveRow);

		}

		private void frmUltraGridSearchReplace_Load(object sender, System.EventArgs e)
		{
			if (_allowReplace)
			{
				this.Text = "Replace";
			}
			else
			{
				this.Text = "Search";
				this.btnReplace.Visible = false;
				this.btnReplaceAll.Visible = false;
				this.lblReplaceWith.Visible = false;
				this.cbxReplaceWith.Visible = false;
				this.btnClose.Location = this.btnReplace.Location;
			}

			this.PopulateLookInCombo();
			this.PopulateSearchContentCombo();
			this.PopulateSearchDirectionCombo();
			this.btnReplace.Enabled = false;
		}

		public void ShowSearchReplace(Infragistics.Win.UltraWinGrid.UltraGrid searchGrid, string columnName )
		{
			_searchGrid = searchGrid;
			_columnName = columnName;

			this.CancelButton = this.btnClose;
			this.KeyPreview = true;

			//	Repopulate this, in case the search column has changed
			this.PopulateLookInCombo();

			//	Show the form, bring it to the foreground
			this.TopMost = true;
			this.Show();
			this.cbxFindWhat.Focus();
			this.BringToFront();

		}

		public void ShowSearchReplace(Infragistics.Win.UltraWinGrid.UltraGrid searchGrid,
			Infragistics.Win.UltraWinGrid.UltraGridColumn searchColumn,
			Infragistics.Win.UltraWinGrid.UltraGridBand gridBand)
		{
			_searchColumn = searchColumn;
			_gridBand = gridBand;
			_searchGrid = searchGrid;
//			_columnName = _searchColumn.Key;
			_columnName = _searchColumn.Header.Caption;
			if (!_captionToSearchColumn.ContainsKey(_searchColumn.Header.Caption))
			{
				_captionToSearchColumn.Add(_searchColumn.Header.Caption, _searchColumn);
			}
			_firstSearch = true;
			_multipleBands = true;

			this.CancelButton = this.btnClose;
			this.KeyPreview = true;

			//	Repopulate this, in case the search column has changed
			this.PopulateLookInCombo();

			//	Show the form, bring it to the foreground
			this.TopMost = true;
			this.Show();
			this.cbxFindWhat.Focus();
			this.BringToFront();

		}

		private void btnFindNext_Click(object sender, System.EventArgs e)
		{
			this.btnReplace.Enabled = false;
			ProcessSearch();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void frmUltraGridSearchReplace_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Visible = false;
		}

		public SearchReplaceInfo SearchReplaceInfo
		{
			get
			{ 
				if ( _searchReplaceInfo == null )
					_searchReplaceInfo = new SearchReplaceInfo();

				return _searchReplaceInfo; 
			}
		}

        // BEGIN MID Track #5749 - Search not working in some cases 
        private bool Match(UltraGridCell aCell)
        {
            string cellString = null;

            if (aCell.Column.ValueList != null)
            {
                // BEGIN TT#1056 - Search in All Columns returns an Invalid Cast Exception - apicchetti - 1/4/2011
                Infragistics.Win.ValueList vl = null;
                if (aCell.Column.DataType == System.Type.GetType("System.String"))
                {
                    cellString = aCell.Value.ToString();
                }
                else
                {
                    vl = (Infragistics.Win.ValueList)aCell.Column.ValueList;

                    foreach (Infragistics.Win.ValueListItem vli in vl.ValueListItems)
                    {
                        if (vli.DataValue.ToString() == aCell.Value.ToString())
                        {
                            cellString = vli.DisplayText;
                            break;
                        }
                    }
                }
                // END TT#1056 - Search in All Columns returns an Invalid Cast Exception - apicchetti - 1/4/2011
            }
            else if (aCell.Column.DataType == System.Type.GetType("System.DateTime"))
            {
                cellString = aCell.Text;
            }
            else
            {
                cellString = aCell.Value.ToString();
            }
            if (cellString != null)
            {
                if (this.Match(_searchReplaceInfo.searchString, cellString))
                {
                    _gridCol = aCell.Column;
                    return true;
                }
            }
            return false;
        }
        // END MID Track #5749

		private bool Match( string userString, string cellValue )
		{
			_foundText = false;
			//   If our search is case insensitive, make both strings uppercase
			if ( ! _searchReplaceInfo.matchCase )
			{
				userString = userString.ToUpper(CultureInfo.CurrentUICulture);
				cellValue = cellValue.ToUpper(CultureInfo.CurrentUICulture);
			}

			//   If we are searching any part of the cell value...
			if ( _searchReplaceInfo.searchContent == eSearchContent.AnyPartOfField )
			{
				//   If the user string is larger than the cell value, it is by definition
				//   a mismatch, so return false
				if ( userString.Length > cellValue.Length )
					return false;
				else if ( userString.Length == cellValue.Length )
				{
					//   If the lengths are equal, the strings must be equal as well
					if ( userString == cellValue )
					{
						_foundText = true;
						_textStart = 0;
						_textLength = userString.Length;
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					//   There is probably an easier way to do this
					int i = cellValue.IndexOf(userString);
					if (i > -1)
					{
						_foundText = true;
						_textStart = i;
						_textLength = userString.Length;
						return true;
					}

					return false;

				}
			}
			else if ( _searchReplaceInfo.searchContent == eSearchContent.WholeField )
			{
				if ( userString == cellValue )
				{
					_foundText = true;
					_textStart = 0;
					_textLength = userString.Length;
					return true;
				}
				else
				{
					return false;
				}
			}
            else if (_searchReplaceInfo.searchContent == eSearchContent.StartOfField)
            {
                if (userString.Length >= cellValue.Length)
                {
                    if (userString.Substring(0, cellValue.Length) == cellValue)
                    {
                        _foundText = true;
                        _textStart = 0;
                        _textLength = userString.Length;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (cellValue.Substring(0, userString.Length) == userString)
                    {
                        _foundText = true;
                        _textStart = 0;
                        _textLength = userString.Length;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            // BEGIN Track #5749 - add END OF FIELD logic
            else 
            {
                if (cellValue.Substring(cellValue.Length - userString.Length, userString.Length) == userString)
                {
                    _foundText = true;
                    _textStart = 0;
                    _textLength = userString.Length;
                    return true;
                }
                else
                {
                    return false;
                }
            }
           	//return false;
            // END Track #5749 
		}


		private bool MatchText(UltraGridRow gridRow)
		{

			if ( gridRow == null ||
				gridRow.Cells == null)
			{
				return false;
			}

			string strColumnKey = _searchReplaceInfo.lookIn;
			//string strCellValue = "";

			//   Determine whether we are searching the current column or all columns
			bool searchAllColumns = true;
			//			if ( _searchGrid.DisplayLayout.Bands[0].Columns.Exists(strColumnKey) )
			//				searchAllColumns = false;
			if (_multipleBands)
			{
				if ( _gridBand.Columns.Exists(strColumnKey) )
				{
					searchAllColumns = false;
				}
			}
			else
			{
				if ( _searchGrid.DisplayLayout.Bands[0].Columns.Exists(strColumnKey)  )
				{
					searchAllColumns = false;
				}
			}

			//   If we are searching all columns then we must iterate through all the cells
			//    in this row, which we can do by using the band//s Columns collection
			if ( gridRow.Cells != null)
			{
				if ( searchAllColumns )
				{
					if (_multipleBands)
					{
						foreach(  UltraGridColumn oCol in _gridBand.Columns )
						{
							if ( gridRow.Cells.Exists(oCol.Key))
							{
								if ( gridRow.Cells[oCol.Key].Value != null )
								{   // BEGIN MID Track #5749 - search not working in some cases 
                                    //if ( this.Match(_searchReplaceInfo.searchString, gridRow.Cells[oCol.Key].Value.ToString() ) )
                                    //{
                                    //    _gridCol = oCol;
                                    //    return true;
                                    //}


                                    if (this.Match(gridRow.Cells[oCol.Key]))
                                    {
                                        return true;
                                    }

                                }   // END MID Track #5749
							}
						}
					}
					else
					{
						foreach(  UltraGridColumn oCol in _searchGrid.DisplayLayout.Bands[0].Columns )
						{
							if ( gridRow.Cells.Exists(oCol.Key))
							{
								if ( gridRow.Cells[oCol.Key].Value != null )
								{   // BEGIN MID Track #5749 - search not working in some cases 
                                    //if ( this.Match(_searchReplaceInfo.searchString, gridRow.Cells[oCol.Key].Value.ToString() ) )
                                    //{
                                    //    _gridCol = oCol;
                                    //    return true;
                                    //}
                                    if (this.Match(gridRow.Cells[oCol.Key]))
                                    {
                                        return true;
                                    }
                                }   // END MID Track #5749
		    				}
						}
					}
				}
				else
				{
					UltraGridColumn oCol;
					if (_multipleBands)
					{
						oCol = _gridBand.Columns[strColumnKey];
					}
					else
					{
						oCol = _searchGrid.DisplayLayout.Bands[0].Columns[strColumnKey];
					}
					if (gridRow.Band.Columns.Exists(oCol.Key))
					{
						if ( gridRow.Cells[oCol.Key].Value != null )
						{   // BEGIN MID Track #5749 - search not working in some cases 
                            //if ( this.Match(_searchReplaceInfo.searchString, gridRow.Cells[oCol.Key].Value.ToString() ) )
                            //{
                            //    _gridCol = oCol;
                            //    return true;
                            //}
                            if (this.Match(gridRow.Cells[oCol.Key]))
                            {
                                return true;
                            }
                        }   // END MID Track #5749
					}
				}
			}

			return false;

		}


		public void Search(UltraGridRow aStartRow)
		{
			//   See if there is an active row; if there is, use it, otherwise
			//   activate the first row and start the search from there
			UltraGridRow gridRow = aStartRow;
			if ( gridRow == null )
			{
				gridRow = _searchGrid.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);
			}

			//   Use the row object//s GetSibling method to iterate through the rows
			//   and check the appropriate cell values

			//   Downward search
			if ( _searchReplaceInfo.searchDirection == eSearchDirection.Down )
			{
				while ( gridRow != null )
				{
					if (!_firstSearch)
					{
						if (gridRow.Cells == null)
						{
							if (gridRow.HasChild(false))
							{
//								Search(gridRow.GetChild(ChildRow.First));
								gridRow = gridRow.GetChild(ChildRow.First);
							}
						}
						else
						{
							gridRow = gridRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next, true, false);
						}
					}
					_firstSearch = false;
					if ( this.MatchText(gridRow) )
					{
						_searchGrid.ActiveRow = gridRow;
						if ( _gridCol != null )
						{
							++_replaceCount;
							btnReplace.Enabled = true;
							_searchGrid.ActiveCell = gridRow.Cells[_gridCol.Key];
							//Begin TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							if (_searchGrid.DisplayLayout.ColScrollRegions.Count > 1)
							{
								if (_searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion != null)
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion;
								}
								else
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.ColScrollRegions[_searchGrid.DisplayLayout.ColScrollRegions.Count - 1];
								}
							}
							//End TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							_searchGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
						}
						return;
					}
				}
			}
				//   Upward search
			else if ( _searchReplaceInfo.searchDirection == eSearchDirection.Up )
			{
				while ( gridRow != null )
				{
					gridRow = gridRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Previous);
					if ( this.MatchText(gridRow) )
					{
						_searchGrid.ActiveRow = gridRow;
						if ( _gridCol != null )
						{
							++_replaceCount;
							btnReplace.Enabled = true;
							_searchGrid.ActiveCell = gridRow.Cells[_gridCol.Key];
							//Begin TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							if (_searchGrid.DisplayLayout.ColScrollRegions.Count > 1)
							{
								if (_searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion != null)
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion;
								}
								else
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.ColScrollRegions[_searchGrid.DisplayLayout.ColScrollRegions.Count - 1];
								}
							}
							//End TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							_searchGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
						}
						return;
					}
				}
			}
				//   Search all rows. First, we start with the active row. If we don//t find
				//   it by the time we hit the  last row, try again starting from the first row
			else if ( _searchReplaceInfo.searchDirection == eSearchDirection.All )
			{
				while ( gridRow != null )
				{
					gridRow = gridRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
					if ( this.MatchText(gridRow) )
					{
						_searchGrid.ActiveRow = gridRow;
						if ( _gridCol != null )
						{
							++_replaceCount;
							btnReplace.Enabled = true;
							_searchGrid.ActiveCell = gridRow.Cells[_gridCol.Key];
							//Begin TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							if (_searchGrid.DisplayLayout.ColScrollRegions.Count > 1)
							{
								if (_searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion != null)
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion;
								}
								else
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.ColScrollRegions[_searchGrid.DisplayLayout.ColScrollRegions.Count - 1];
								}
							}
							//End TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							_searchGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
						}
						return;
					}
				}

				//   We didn't find it the first time around, so start again from the first row
				gridRow = _searchGrid.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);
				while ( gridRow != null )
				{
					if ( this.MatchText(gridRow) )
					{
						_searchGrid.ActiveRow = gridRow;
						if ( _gridCol != null )
						{
							++_replaceCount;
							btnReplace.Enabled = true;
							_searchGrid.ActiveCell = gridRow.Cells[_gridCol.Key];
							//Begin TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							if (_searchGrid.DisplayLayout.ColScrollRegions.Count > 1)
							{
								if (_searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion != null)
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.Bands[0].Columns[_gridCol.Key].Header.ExclusiveColScrollRegion;
								}
								else
								{
									_searchGrid.ActiveColScrollRegion = _searchGrid.DisplayLayout.ColScrollRegions[_searchGrid.DisplayLayout.ColScrollRegions.Count - 1];
								}
							}
							//End TT#880 - JScott - Undahndled exception when doing a search on Store in Store Profiles
							_searchGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
						}
						return;
					}
					gridRow = gridRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
				}

			}
			btnReplace.Enabled = false;
			_continueReplace = false;

			if (_performingReplaceAll)
			{
				string text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ReplaceComplete);
				text = text.Replace("{0}", _replaceCount.ToString());
				MessageBox.Show(text, _searchGrid.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
			}
			else
			{
				MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SearchComplete, false) + _searchReplaceInfo.searchString, _searchGrid.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
			}
			_performingReplaceAll = false;
		}

		private void cbxFindWhat_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			_firstSearch = true;
			_foundText = false;
		}

		private void cbxFindWhat_TextChanged(object sender, System.EventArgs e)
		{
			_firstSearch = true;
			_foundText = false;
		}

		private void cbxReplaceWith_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
		
		}

		private void btnReplace_Click(object sender, System.EventArgs e)
		{
			try
			{
				ReplaceText();
				ProcessSearch();
			}
			catch (Exception exception)
			{
				string message = exception.ToString();
			}
		
		}

		private void btnReplaceAll_Click(object sender, System.EventArgs e)
		{
			_continueReplace = true;
			_replaceCount = 0;
			_performingReplaceAll = true;
			try
			{
				if (cbxFindWhat.Text.Length == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SpecifyReplaceText), _searchGrid.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
					return;
				}
				if (!_foundText)
				{
					ProcessSearch();
				}
				while (_continueReplace)
				{
					ReplaceText();
					Search(_searchGrid.ActiveRow);
				}
			}
			catch (Exception exception)
			{
				string message = exception.Message;
			}
		
		}

		private void ReplaceText()
		{
			try
			{
				string replaceString = Convert.ToString(_searchGrid.ActiveRow.Cells["Text"].Value,CultureInfo.CurrentUICulture);
				string replaceValue = replaceString.Substring(_textStart, _textLength);
				replaceString = replaceString.Replace(replaceValue,cbxReplaceWith.Text);
				_searchGrid.ActiveRow.Cells["Text"].Value = replaceString;
				_searchGrid.ActiveRow.Cells["Updated"].Value = true;
				_replacePerformed = true;
			}
			catch (Exception exception)
			{
				string message = exception.ToString();
			}
		}

        private void cbxFindWhat_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxFindWhat_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cbxReplaceWith_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxReplaceWith_SelectionChangeCommitted(source, new EventArgs());
        }

	}

	public class SearchReplaceInfo
	{
		public string searchString;
		public string lookIn;
		public eSearchDirection searchDirection;
		public eSearchContent searchContent;
		public bool matchCase = false;

		public SearchReplaceInfo()
		{
			this.searchContent = eSearchContent.AnyPartOfField;
			this.lookIn = "";
			this.matchCase = false;
			this.searchDirection = eSearchDirection.Down;
			this.searchString = "";
		}
	}
}
