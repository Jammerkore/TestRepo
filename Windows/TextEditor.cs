using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for TextEditor.
	/// </summary>
	public class frmTextEditor  : MIDFormBase
	{
		SessionAddressBlock _SAB;
		private bool _locked = false;
		private System.Data.DataSet _textDataSet;
		private System.Data.DataTable _messageLevelDataTable = null;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugTextEditor;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddMessageLevel;
		private System.ComponentModel.IContainer components;

		public frmTextEditor(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_SAB = aSAB;
			EnqueueText();
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

				this.ugTextEditor.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugTextEditor_AfterRowUpdate);
				this.ugTextEditor.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugTextEditor_CellChange);
				this.ugTextEditor.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugTextEditor_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugTextEditor);
                //End TT#169
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
				this.uddMessageLevel.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddMessageLevel_RowSelected);
                this.uddMessageLevel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uddMessageLevel_InitializeLayout);
                this.Load -= new System.EventHandler(this.frmTextEditor_Load);
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.ugTextEditor = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.uddMessageLevel = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddMessageLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ugTextEditor
            // 
            this.ugTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugTextEditor.DisplayLayout.Appearance = appearance1;
            this.ugTextEditor.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugTextEditor.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugTextEditor.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugTextEditor.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugTextEditor.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugTextEditor.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugTextEditor.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugTextEditor.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugTextEditor.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugTextEditor.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugTextEditor.Location = new System.Drawing.Point(24, 40);
            this.ugTextEditor.Name = "ugTextEditor";
            this.ugTextEditor.Size = new System.Drawing.Size(640, 344);
            this.ugTextEditor.TabIndex = 0;
            this.ugTextEditor.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugTextEditor_InitializeLayout);
            this.ugTextEditor.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugTextEditor_AfterRowUpdate);
            this.ugTextEditor.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugTextEditor_CellChange);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(576, 408);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(488, 408);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // uddMessageLevel
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddMessageLevel.DisplayLayout.Appearance = appearance7;
            this.uddMessageLevel.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.uddMessageLevel.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddMessageLevel.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddMessageLevel.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddMessageLevel.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.uddMessageLevel.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddMessageLevel.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.uddMessageLevel.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.uddMessageLevel.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddMessageLevel.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddMessageLevel.Location = new System.Drawing.Point(176, 152);
            this.uddMessageLevel.Name = "uddMessageLevel";
            this.uddMessageLevel.Size = new System.Drawing.Size(75, 23);
            this.uddMessageLevel.TabIndex = 11;
            this.uddMessageLevel.Visible = false;
            this.uddMessageLevel.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddMessageLevel_RowSelected);
            this.uddMessageLevel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uddMessageLevel_InitializeLayout);
            // 
            // frmTextEditor
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(688, 438);
            this.Controls.Add(this.uddMessageLevel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ugTextEditor);
            this.Name = "frmTextEditor";
            this.Text = "TextEditor";
            this.Load += new System.EventHandler(this.frmTextEditor_Load);
            this.Controls.SetChildIndex(this.ugTextEditor, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.uddMessageLevel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddMessageLevel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmTextEditor_Load(object sender, System.EventArgs e)
		{
			InitializeForm();
		}

		public void InitializeForm()
		{
			try
			{
				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsTextEditor);
				eDataState dataState = eDataState.ReadOnly;
				if (!FunctionSecurity.AllowUpdate)
				{
					dataState = eDataState.ReadOnly;
				}
				else
				{
					if (_locked)
					{
						dataState = eDataState.Updatable;
					}
					else
					{
						FunctionSecurity.SetReadOnly();
						dataState = eDataState.ReadOnly;
					}
				}
				Format_Title(dataState, eMIDTextCode.frm_TextEditor,null);
				Populate_TextEditor();
				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
				BuildMenu();
				SetText();
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void EnqueueText()
		{
			TextEnqueue textEnqueue = new TextEnqueue(
				_SAB.ClientServerSession.UserRID,
				_SAB.ClientServerSession.ThreadID);

			try
			{
				textEnqueue.EnqueueText();
				_locked = true;
				DisplayForm = true;
			}
			catch (TextConflictException)
			{
				// release enqueue write lock incase they sit on the read only screen
				string errMsg = "Text is being updated by " + System.Environment.NewLine;
				foreach (TextConflict TCon in textEnqueue.ConflictList)
				{
					errMsg +=  " User: " + TCon.UserName;
				}
				errMsg += System.Environment.NewLine + System.Environment.NewLine;
				errMsg += "Do you wish to continue with the text as read-only?";

				if (MessageBox.Show (errMsg,  this.Text,
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					== DialogResult.Yes)
				{
					DisplayForm = true;
				}
				else
				{
					DisplayForm = false;
				}
			}
		}

		private void DequeueText()
		{
			TextEnqueue textEnqueue = new TextEnqueue(
				_SAB.ClientServerSession.UserRID,
				_SAB.ClientServerSession.ThreadID);

			try
			{
				textEnqueue.DequeueText();

			}
			catch 
			{
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void BuildMenu()
//		{
//			try
//			{
//				PopupMenuTool editMenuTool;
//				ButtonTool btFind;
//				ButtonTool btReplace;
//
//				utmMain.ImageListSmall = MIDGraphics.ImageList;
//				utmMain.ImageListLarge = MIDGraphics.ImageList;
//
//				editMenuTool = new PopupMenuTool("edit_menu");
//				editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
//				editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.False;
//				utmMain.Tools.Add(editMenuTool);
//
//				btFind = new ButtonTool("btFind");
//				btFind.SharedProps.Caption = "&Find";
//				btFind.SharedProps.Shortcut = Shortcut.CtrlF;
//				btFind.SharedProps.MergeOrder = 20;
//				btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
//				utmMain.Tools.Add(btFind);
//
//				btReplace = new ButtonTool("btReplace");
//				btReplace.SharedProps.Caption = "R&eplace";
//				btReplace.SharedProps.Shortcut = Shortcut.CtrlH;
//				btReplace.SharedProps.MergeOrder = 20;
//				btReplace.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.ReplaceImage);
//				utmMain.Tools.Add(btReplace);
//
//				editMenuTool.Tools.Add(btFind);
//				editMenuTool.Tools.Add(btReplace);
//
//				editMenuTool.Tools["btFind"].InstanceProps.IsFirstInGroup = true;
//			}
//			catch (Exception exception)
//			{
//				HandleException(exception);
//			}
//		}

		private void BuildMenu()
		{
			try
			{
                AddMenuItem(eMIDMenuItem.EditFind);
                AddMenuItem(eMIDMenuItem.EditReplace);
                HideMenuItem(this, eMIDMenuItem.EditDelete);
                //ButtonTool btFind;
                //ButtonTool btReplace;

                //btFind = new ButtonTool(Include.btFind);
                //btFind.SharedProps.Caption = "&Find";
                //btFind.SharedProps.Shortcut = Shortcut.CtrlF;
                //btFind.SharedProps.MergeOrder = 20;
                //btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
                //utmMain.Tools.Add(btFind);

                //btReplace = new ButtonTool(Include.btReplace);
                //btReplace.SharedProps.Caption = "R&eplace";
                //btReplace.SharedProps.Shortcut = Shortcut.CtrlH;
                //btReplace.SharedProps.MergeOrder = 20;
                //btReplace.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.ReplaceImage);
                //utmMain.Tools.Add(btReplace);

                //EditMenuTool.Tools.Add(btFind);
                //EditMenuTool.Tools.Add(btReplace);

                //EditMenuTool.Tools[Include.btFind].InstanceProps.IsFirstInGroup = true;
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}
		// End MID Track 4858

		private void SetText()
		{
			this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
		}


		private void ugTextEditor_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			// check for saved layout
			InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
			InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.textEditorGrid);
			if (layout.LayoutLength > 0)
			{
				ugTextEditor.DisplayLayout.Load(layout.LayoutStream);
			}
			else
			{	// DEFAULT grid layout
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                // End TT#1164
                //End TT#169
				DefaultGridLayout();
			}
			
			CommonGridLayout();

			//NEVER ALLOW DELETIONS ON THIS GRID.  DELETIONS SHOULD BE DONE
			//USING A UPGRADE SCRIPT.
			foreach (UltraGridBand ugb in ugTextEditor.DisplayLayout.Bands)
			{
				ugb.Override.AllowDelete = DefaultableBoolean.False;
			}


		}

		private void DefaultGridLayout()
		{
			try
			{
				this.ugTextEditor.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
				this.ugTextEditor.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				this.ugTextEditor.DisplayLayout.AddNewBox.Hidden = true;
				this.ugTextEditor.DisplayLayout.AddNewBox.Prompt = "";
				this.ugTextEditor.DisplayLayout.Bands[0].AddButtonCaption = "";
				this.ugTextEditor.DisplayLayout.GroupByBox.Hidden = true;
				this.ugTextEditor.DisplayLayout.GroupByBox.Prompt = "";
				this.ugTextEditor.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Type"].Width = 125;
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void CommonGridLayout()
		{
			try
			{
				this.ugTextEditor.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Updated"].Hidden = true;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Code"].Header.Caption = "Code";
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Text"].Header.Caption = "Text";
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageType"].Hidden = true;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageLevelCode"].Hidden = true;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageLevel"].Header.Caption = "Message Level";

                // Begin TT#1159 - JSmith - Improve Messaging
                //int width = ugTextEditor.Width - this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Code"].Width - this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Type"].Width - this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageLevel"].Width - 20;
                //this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Text"].Width = width;
                // End TT#1159

				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Code"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Code"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;

				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_VALUE"].Width = this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageLevel"].Width;
				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["MessageLevel"].ValueList = this.uddMessageLevel;
                // Begin TT#668-MD - JSmith - Windows 8 - Installer issues
                this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Code"].Format = "#########0";
                // End TT#668-MD - JSmith - Windows 8 - Installer issues
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void Populate_TextEditor()
		{
			try
			{
				string textType = null;
				string messageLevel = null;
				int messageLevelCode;
				eMIDTextType MIDTextType = eMIDTextType.message;
				this.Cursor = Cursors.WaitCursor;

				_messageLevelDataTable = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
				this.uddMessageLevel.DataSource = _messageLevelDataTable;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_CODE"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_MAX_LENGTH"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_TYPE"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_LEVEL"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_VALUE_TYPE"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].Columns["TEXT_ORDER"].Hidden = true;
				this.uddMessageLevel.DisplayLayout.Bands[0].ColHeadersVisible = false;
				this.uddMessageLevel.DisplayMember = "TEXT_VALUE";
				this.uddMessageLevel.ValueMember = "TEXT_CODE";
				this.uddMessageLevel.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
				this.uddMessageLevel.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
				
				TextEditor_Define();
				ugTextEditor.DataSource = _textDataSet;
				DataTable text = MIDText.Read(eMIDTextOrderBy.TextCode);
				foreach(DataRow dr in text.Rows)
				{
					MIDTextType = (eMIDTextType)Convert.ToInt32(dr["TEXT_TYPE"],CultureInfo.CurrentUICulture);
					textType = MIDTextType.ToString();
					if (Convert.IsDBNull(dr["TEXT_LEVEL"]))
					{
						messageLevelCode = Convert.ToInt32(eMIDMessageLevel.None,CultureInfo.CurrentUICulture);
						messageLevel = MIDText.GetTextOnly(Convert.ToInt32(eMIDMessageLevel.None,CultureInfo.CurrentUICulture));
					}
					else
					{
						messageLevelCode = Convert.ToInt32(dr["TEXT_LEVEL"],CultureInfo.CurrentUICulture);
						messageLevel = MIDText.GetTextOnly(Convert.ToInt32(dr["TEXT_LEVEL"],CultureInfo.CurrentUICulture));
					}
					_textDataSet.Tables["Text"].Rows.Add(new object[] { false,
																		  Convert.ToInt32(dr["TEXT_CODE"],CultureInfo.CurrentUICulture),
																		  Convert.ToInt32(MIDTextType,CultureInfo.CurrentUICulture),
																		  textType,
																		  messageLevelCode,
																		  messageLevel,
																		  Convert.ToString(dr["TEXT_VALUE"],CultureInfo.CurrentUICulture)
																	  });

				}

				foreach(  UltraGridRow gridRow in ugTextEditor.Rows )
				{
					if ((eMIDTextType)Convert.ToInt32(gridRow.Cells["MessageType"].Value,CultureInfo.CurrentUICulture) != eMIDTextType.message ||
						Convert.ToInt32(gridRow.Cells["Code"].Value,CultureInfo.CurrentUICulture) == 0)	// do not allow a change to unassigned
					{
						gridRow.Cells["MessageLevel"].Activation = Activation.NoEdit;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			finally
			{
				//				this.ugTextEditor.DisplayLayout.Bands["Text"].Columns["Text"].PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.VisibleRows);
				this.Cursor = Cursors.Default;
			}
		}

		private void TextEditor_Define()
		{
			try
			{
                _textDataSet = MIDEnvironment.CreateDataSet("TextEditorDataSet");

				DataTable textDataTable = _textDataSet.Tables.Add("Text");

				DataColumn dataColumn;

				//Create Columns and rows for datatable
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "Updated";
				dataColumn.Caption = "Updated";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "Code";
				dataColumn.Caption = "Code";
				dataColumn.ReadOnly = true;
				dataColumn.Unique = true;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "MessageType";
				dataColumn.Caption = "MessageType";
				dataColumn.ReadOnly = true;
				dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Type";
				dataColumn.Caption = "Type";
				dataColumn.ReadOnly = true;
				dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "MessageLevelCode";
				dataColumn.Caption = "MessageLevelCode";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "MessageLevel";
				dataColumn.Caption = "MessageLevel";
                // Begin TT#1159 - JSmith - Improve Messaging
                //dataColumn.ReadOnly = true;
                dataColumn.ReadOnly = false;
                // End TT#1159
                dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Text";
				dataColumn.Caption = "Text";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				textDataTable.Columns.Add(dataColumn);

				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		override protected void BeforeClosing()
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
                    // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
                    //if (!ugTextEditor.IsDisposed)
                    if (FormLoaded &&
                        !ugTextEditor.IsDisposed)
                    // End TT#2012
					{
						InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
						layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.textEditorGrid, ugTextEditor);
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		override protected void AfterClosing()
		{
			try
			{
				if (_locked)
				{
					DequeueText();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "frmTextEditor.AfterClosing");
			}
	}

		private void ugTextEditor_AfterRowUpdate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					e.Row.Cells["Updated"].Value = true;
					ChangePending = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void ugTextEditor_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					e.Cell.Row.Cells["Updated"].Value = true;
					ChangePending = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void uddMessageLevel_RowSelected(object sender, Infragistics.Win.UltraWinGrid.RowSelectedEventArgs e)
		{
			try
			{
				if (e.Row != null &&
					ugTextEditor.ActiveRow != null)
				{
					ugTextEditor.ActiveRow.Cells["MessageLevelCode"].Value = e.Row.Cells["TEXT_CODE"].Value;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			ISave();
			Close();
		}

		override protected bool SaveChanges()
		{
			try
			{
				MIDText.OpenUpdateConnection();
				foreach(  UltraGridRow gridRow in this.ugTextEditor.Rows )
				{
					if ((bool)gridRow.Cells["Updated"].Value)
					{
						MIDText.Update(Convert.ToInt32(gridRow.Cells["Code"].Value, CultureInfo.CurrentUICulture), 
							Convert.ToString(gridRow.Cells["Text"].Value, CultureInfo.CurrentUICulture),
							Convert.ToInt32(gridRow.Cells["MessageLevelCode"].Value, CultureInfo.CurrentUICulture));
					}
				}
				MIDText.CommitData();
				ChangePending = false;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
			finally
			{
				MIDText.CloseUpdateConnection();
			}
			return true;
		}

		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		override public void ISaveAs()
		{
			
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

        //private void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        //{
        //    try
        //    {
        //        switch (e.Tool.Key)
        //        {
        //            case Include.btFind:
        //                Find();
        //                break;

        //            case "btReplace":
        //                Replace();
        //                break;

        //            //default:
        //            //    base.utmMain_ToolClick(sender, e);
        //            //    break;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}

        override public void IFind()
		{
			try
			{
				frmUltraGridSearchReplace frmUltraGridSearchReplace = new frmUltraGridSearchReplace(_SAB, false);

				frmUltraGridSearchReplace.ShowSearchReplace(this.ugTextEditor, ugTextEditor.DisplayLayout.Bands["Text"].Columns["Text"], ugTextEditor.DisplayLayout.Bands["Text"]);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override public void IReplace()
		{
			try
			{

				frmUltraGridSearchReplace frmUltraGridSearchReplace = new frmUltraGridSearchReplace(_SAB, true);

				frmUltraGridSearchReplace.ShowSearchReplace(this.ugTextEditor, ugTextEditor.DisplayLayout.Bands["Text"].Columns["Text"], ugTextEditor.DisplayLayout.Bands["Text"]);
				if (frmUltraGridSearchReplace.ReplacePerformed)
				{
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        private void uddMessageLevel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            ugld.ApplyDefaults(e, true);
        }
	}
}
