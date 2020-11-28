using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for DC Carton Rounding Method.
	/// </summary>
	public class frmDCCartonRoundingMethod : WorkflowMethodFormBase
	{
		#region Properties

        private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ImageList Icons;

		private DCCartonRoundingMethod _dcCartonRoundingMethod = null;
		private int _nodeRID = -1;
		private TabPage tabMethod;
        private TabControl tabDCCartonRoundingMethod;
        private TabPage tabProperties;
        private Label lblDCCartonRoundingTabText;
        private GroupBox gboDCCartonRounding;
        private MIDAttributeComboBox cboDCCartonRoundingAttribute;
        private Label lblAttribute;
        private UltraGrid ugWorkflows;
        private Label lblApplyOverageTo;
        private GroupBox gboAppplyOverageTo;
        private RadioButton rbReserve;
        private RadioButton rbAllocatedStores;
		
		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		#endregion

		public frmDCCartonRoundingMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_DCCartonRounding, eWorkflowMethodType.Method)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserDCCartonRounding);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalDCCartonRounding);
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
                this.cboDCCartonRoundingAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDCCartonRoundingAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboDCCartonRoundingAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboDCCartonRoundingAttribute_SelectionChangeCommitted);
                this.rbAllocatedStores.CheckedChanged -= new System.EventHandler(this.rbAllocatedStores_CheckedChanged);
                this.rbReserve.CheckedChanged -= new System.EventHandler(this.rbReserve_CheckedChanged);
               
				if (ApplicationTransaction != null)
				{
					ApplicationTransaction.Dispose();
				}
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.lblDCCartonRoundingTabText = new System.Windows.Forms.Label();
            this.gboDCCartonRounding = new System.Windows.Forms.GroupBox();
            this.lblApplyOverageTo = new System.Windows.Forms.Label();
            this.gboAppplyOverageTo = new System.Windows.Forms.GroupBox();
            this.rbReserve = new System.Windows.Forms.RadioButton();
            this.rbAllocatedStores = new System.Windows.Forms.RadioButton();
            this.cboDCCartonRoundingAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.tabDCCartonRoundingMethod = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMethod.SuspendLayout();
            this.gboDCCartonRounding.SuspendLayout();
            this.gboAppplyOverageTo.SuspendLayout();
            this.tabDCCartonRoundingMethod.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(608, 504);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 504);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(40, 504);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.lblDCCartonRoundingTabText);
            this.tabMethod.Controls.Add(this.gboDCCartonRounding);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(640, 397);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // lblDCCartonRoundingTabText
            // 
            this.lblDCCartonRoundingTabText.AutoSize = true;
            this.lblDCCartonRoundingTabText.Location = new System.Drawing.Point(160, 32);
            this.lblDCCartonRoundingTabText.Name = "lblDCCartonRoundingTabText";
            this.lblDCCartonRoundingTabText.Size = new System.Drawing.Size(308, 13);
            this.lblDCCartonRoundingTabText.TabIndex = 3;
            this.lblDCCartonRoundingTabText.Text = "Apply DC Carton Rounding For Bulk and/or Generic Packs Only";
            // 
            // gboDCCartonRounding
            // 
            this.gboDCCartonRounding.Controls.Add(this.lblApplyOverageTo);
            this.gboDCCartonRounding.Controls.Add(this.gboAppplyOverageTo);
            this.gboDCCartonRounding.Controls.Add(this.cboDCCartonRoundingAttribute);
            this.gboDCCartonRounding.Controls.Add(this.lblAttribute);
            this.gboDCCartonRounding.Location = new System.Drawing.Point(20, 65);
            this.gboDCCartonRounding.Name = "gboDCCartonRounding";
            this.gboDCCartonRounding.Size = new System.Drawing.Size(596, 313);
            this.gboDCCartonRounding.TabIndex = 2;
            this.gboDCCartonRounding.TabStop = false;
            // 
            // lblApplyOverageTo
            // 
            this.lblApplyOverageTo.AutoSize = true;
            this.lblApplyOverageTo.Location = new System.Drawing.Point(53, 134);
            this.lblApplyOverageTo.Name = "lblApplyOverageTo";
            this.lblApplyOverageTo.Size = new System.Drawing.Size(93, 13);
            this.lblApplyOverageTo.TabIndex = 4;
            this.lblApplyOverageTo.Text = "Apply Overage To";
            // 
            // gboAppplyOverageTo
            // 
            this.gboAppplyOverageTo.Controls.Add(this.rbReserve);
            this.gboAppplyOverageTo.Controls.Add(this.rbAllocatedStores);
            this.gboAppplyOverageTo.Location = new System.Drawing.Point(152, 117);
            this.gboAppplyOverageTo.Name = "gboAppplyOverageTo";
            this.gboAppplyOverageTo.Size = new System.Drawing.Size(215, 41);
            this.gboAppplyOverageTo.TabIndex = 3;
            this.gboAppplyOverageTo.TabStop = false;
            // 
            // rbReserve
            // 
            this.rbReserve.AutoSize = true;
            this.rbReserve.Location = new System.Drawing.Point(130, 15);
            this.rbReserve.Name = "rbReserve";
            this.rbReserve.Size = new System.Drawing.Size(65, 17);
            this.rbReserve.TabIndex = 1;
            this.rbReserve.Text = "Reserve";
            this.rbReserve.UseVisualStyleBackColor = true;
            this.rbReserve.CheckedChanged += new System.EventHandler(this.rbReserve_CheckedChanged);
            // 
            // rbAllocatedStores
            // 
            this.rbAllocatedStores.AutoSize = true;
            this.rbAllocatedStores.Location = new System.Drawing.Point(16, 15);
            this.rbAllocatedStores.Name = "rbAllocatedStores";
            this.rbAllocatedStores.Size = new System.Drawing.Size(102, 17);
            this.rbAllocatedStores.TabIndex = 0;
            this.rbAllocatedStores.Text = "Allocated Stores";
            this.rbAllocatedStores.UseVisualStyleBackColor = true;
            this.rbAllocatedStores.CheckedChanged += new System.EventHandler(this.rbAllocatedStores_CheckedChanged);
            // 
            // cboDCCartonRoundingAttribute
            // 
            this.cboDCCartonRoundingAttribute.AllowDrop = true;
            this.cboDCCartonRoundingAttribute.AllowUserAttributes = false;
            this.cboDCCartonRoundingAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDCCartonRoundingAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDCCartonRoundingAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboDCCartonRoundingAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDCCartonRoundingAttribute.FormattingEnabled = true;
            this.cboDCCartonRoundingAttribute.Location = new System.Drawing.Point(105, 51);
            this.cboDCCartonRoundingAttribute.Name = "cboDCCartonRoundingAttribute";
            this.cboDCCartonRoundingAttribute.Size = new System.Drawing.Size(227, 21);
            this.cboDCCartonRoundingAttribute.TabIndex = 0;
            this.cboDCCartonRoundingAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDCCartonRoundingAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboDCCartonRoundingAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboDCCartonRoundingAttribute_SelectionChangeCommitted);
            this.cboDCCartonRoundingAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboDCCartonRoundingAttribute_DragDrop);
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(53, 54);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(46, 13);
            this.lblAttribute.TabIndex = 1;
            this.lblAttribute.Text = "Attribute";
            // 
            // tabDCCartonRoundingMethod
            // 
            this.tabDCCartonRoundingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDCCartonRoundingMethod.Controls.Add(this.tabMethod);
            this.tabDCCartonRoundingMethod.Controls.Add(this.tabProperties);
            this.tabDCCartonRoundingMethod.Location = new System.Drawing.Point(36, 64);
            this.tabDCCartonRoundingMethod.Name = "tabDCCartonRoundingMethod";
            this.tabDCCartonRoundingMethod.SelectedIndex = 0;
            this.tabDCCartonRoundingMethod.Size = new System.Drawing.Size(648, 423);
            this.tabDCCartonRoundingMethod.TabIndex = 14;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(640, 397);
            this.tabProperties.TabIndex = 2;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // ugWorkflows
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance1;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugWorkflows.Location = new System.Drawing.Point(0, 0);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(640, 397);
            this.ugWorkflows.TabIndex = 2;
            // 
            // frmDCCartonRoundingMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 541);
            this.Controls.Add(this.tabDCCartonRoundingMethod);
            this.Name = "frmDCCartonRoundingMethod";
            this.Text = "DC Carton Rounding Method";
            this.Controls.SetChildIndex(this.tabDCCartonRoundingMethod, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMethod.ResumeLayout(false);
            this.tabMethod.PerformLayout();
            this.gboDCCartonRounding.ResumeLayout(false);
            this.gboDCCartonRounding.PerformLayout();
            this.gboAppplyOverageTo.ResumeLayout(false);
            this.gboAppplyOverageTo.PerformLayout();
            this.tabDCCartonRoundingMethod.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		

		/// <summary>
		/// Opens a new DC Carton Rounding Method.
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_dcCartonRoundingMethod = new DCCartonRoundingMethod(SAB,Include.NoRID);
				ABM = _dcCartonRoundingMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserDCCartonRounding, eSecurityFunctions.AllocationMethodsGlobalDCCartonRounding);

				Common_Load(aParentNode.GlobalUserType);				 

			}
			catch(Exception ex)
			{
				HandleException(ex, "DC Carton Rounding Method Constructor");
				FormLoadError = true;
			}
		}
		/// <summary>
		/// Opens an existing DC Carton Rounding Method.
		/// </summary>
		/// <param name="aMethodRID">aMethodRID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_nodeRID = aNodeRID;
				_dcCartonRoundingMethod = new DCCartonRoundingMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserDCCartonRounding, eSecurityFunctions.AllocationMethodsGlobalDCCartonRounding);
			
				Common_Load(aNode.GlobalUserType);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a DC Carton Rounding Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
                _dcCartonRoundingMethod = new DCCartonRoundingMethod(SAB,aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames a DC Carton Rounding Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
                _dcCartonRoundingMethod = new DCCartonRoundingMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
			return false;
		}

		/// <summary>
		/// Processes a method.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{
                _dcCartonRoundingMethod = new DCCartonRoundingMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.DCCartonRounding, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void Common_Load(eGlobalUserType aGlobalUserType)//9-17
		{
			try
			{
				SetText();		
	
				Name = MIDText.GetTextOnly((int)eMethodType.DCCartonRounding);
                if (_dcCartonRoundingMethod.Method_Change_Type == eChangeType.add)
                {
                    Format_Title(eDataState.New, eMIDTextCode.frm_DCCartonRounding, null);
                }
                else if (FunctionSecurity.AllowUpdate)
                {
                    Format_Title(eDataState.Updatable, eMIDTextCode.frm_DCCartonRounding, _dcCartonRoundingMethod.Name);
                }
                else
                {
                    Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_DCCartonRounding, _dcCartonRoundingMethod.Name);
                }

                if (FunctionSecurity.AllowExecute)
                {
                    btnProcess.Enabled = true;
                }
                else
                {
                    btnProcess.Enabled = false;
                }

                BuildAttributeList();

				// Begin TT#1709-MD - stodd - If Reserve Store is not available (inactive), Apply Overage To Reserve should not be an option on the DC Carton Rounding Method.
                bool AllowBalToReserve = true;
                GlobalOptionsProfile gop = SAB.ApplicationServerSession.GlobalOptions;
                if (gop.ReserveStoreRID == Include.UndefinedStoreRID)
                {
                    AllowBalToReserve = false;
                }
                else
                {
                    // Begin TT#1386-MD - stodd - manual merge
                    //ArrayList storeKeys = SAB.StoreServerSession.GetAllStoreKeys();
                    //if (!storeKeys.Contains(gop.ReserveStoreRID))
                    //{
                    //    AllowBalToReserve = false;
                    //}

                    ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList();
                    StoreProfile sp = (StoreProfile)storeList.FindKey(gop.ReserveStoreRID);
                    if (sp == null)
                    {
                        AllowBalToReserve = false;
                    }
                    // End TT#1386-MD - stodd - manual merge
                }
				// End TT#1709-MD - stodd - If Reserve Store is not available (inactive), Apply Overage To Reserve should not be an option on the DC Carton Rounding Method.
				
                if (_dcCartonRoundingMethod.APPLY_OVERAGE_TO == eAllocateOverageTo.AllocatedStores)
                {
                    rbAllocatedStores.Checked = true;
                }
                else if (_dcCartonRoundingMethod.APPLY_OVERAGE_TO == eAllocateOverageTo.Reserve)
                {
                    rbReserve.Checked = true;
                }
				
				// Begin TT#1709-MD - stodd - If Reserve Store is not available (inactive), Apply Overage To Reserve should not be an option on the DC Carton Rounding Method.
                if (!AllowBalToReserve)
                {
                    rbAllocatedStores.Checked = true;
                    rbReserve.Enabled = false;
                }
				// End TT#1709-MD - stodd - If Reserve Store is not available (inactive), Apply Overage To Reserve should not be an option on the DC Carton Rounding Method.

                LoadWorkflows();

			}
			catch( Exception exception )
			{
				HandleException(exception);
			}		
		}

		private void SetText()
		{
			try
			{
                if (_dcCartonRoundingMethod.Method_Change_Type == eChangeType.update)
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                }
                else
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                }

                this.lblDCCartonRoundingTabText.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCCartonRoundingTabText);
                this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);
                this.lblApplyOverageTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyOverageTo);
                this.rbAllocatedStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocatedStores);
                this.rbReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve);                
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
        override protected void BuildAttributeList()
        {
            try
            {
                int currValue2 = Include.NoRID;
                if (cboDCCartonRoundingAttribute.SelectedValue != null &&
                    cboDCCartonRoundingAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue2 = Convert.ToInt32(cboDCCartonRoundingAttribute.SelectedValue, CultureInfo.CurrentUICulture);
                }
                cboDCCartonRoundingAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboDCCartonRoundingAttribute, FunctionSecurity, _dcCartonRoundingMethod.GlobalUserType == eGlobalUserType.User);
                //ProfileList alProfileList = SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.GlobalOnly, false);
                ProfileList alProfileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, false);  // TT#1386-MD - stodd - manual merge
                cboDCCartonRoundingAttribute.Initialize(SAB, UserSecurity, alProfileList.ArrayList, false);
                if (currValue2 != Include.NoRID)
                {
                    cboDCCartonRoundingAttribute.SelectedValue = currValue2;
                }
                else
                {
                    cboDCCartonRoundingAttribute.SelectedValue = _dcCartonRoundingMethod.SG_RID;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
		#region Properties Tab - Workflows
		/// <summary>
		/// Fill the workflow grid
		/// </summary>
		private void LoadWorkflows()
		{
			try
			{
                GetWorkflows(_dcCartonRoundingMethod.Key, ugWorkflows);
                tabDCCartonRoundingMethod.Controls.Remove(tabProperties);
            }
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}
		#endregion

		#region Save Button	
		protected override void Call_btnSave_Click()
		{
			try
			{
				base.btnSave_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}	
		}
		#endregion
		
	

		protected override void Call_btnProcess_Click()
		{
			try
			{
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.DCCartonRounding))
				{
					return;
				}

				ProcessAction(eMethodType.DCCartonRounding);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_dcCartonRoundingMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
	
		private bool MethodChanges()
		{
			try
			{
				//Method Name
				if (_dcCartonRoundingMethod.Name != this.txtName.Text)
					return true;

				//Method Description
				if (_dcCartonRoundingMethod.Method_Description != this.txtDesc.Text)
					return true;
			
				//Global and User Radio Buttons
				if (radGlobal.Checked)
				{
					if (_dcCartonRoundingMethod.GlobalUserType != eGlobalUserType.Global)
						return true;
				}
				else
				{
					if (_dcCartonRoundingMethod.GlobalUserType != eGlobalUserType.User)
						return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}
		
        #region DC Carton Rounding Method Changes
        private void cboDCCartonRoundingAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboDCCartonRoundingAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

        private void cboDCCartonRoundingAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _dcCartonRoundingMethod.SG_RID = Convert.ToInt32(cboDCCartonRoundingAttribute.SelectedValue, CultureInfo.CurrentCulture);
            }
        }
        private void cboDCCartonRoundingAttribute_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        private void rbAllocatedStores_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbAllocatedStores.Checked)
            {
                ChangePending = true;
                _dcCartonRoundingMethod.APPLY_OVERAGE_TO = eAllocateOverageTo.AllocatedStores;
            }
        }

        private void rbReserve_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbReserve.Checked)
            {
                ChangePending = true;
                _dcCartonRoundingMethod.APPLY_OVERAGE_TO = eAllocateOverageTo.Reserve;
            }
        }
		#endregion
	 
		#region WorkflowMethodFormBase Overrides 

		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.Methods;	
		}

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		override protected void SetCommonFields()
		{
			WorkflowMethodName = txtName.Text;
			WorkflowMethodDescription = txtDesc.Text;
			GlobalRadioButton = radGlobal;
			UserRadioButton = radUser;
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
            // Begin TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 
            bool methodFieldsValid = true;

            int sgRid = Convert.ToInt32(cboDCCartonRoundingAttribute.SelectedValue, CultureInfo.CurrentCulture);
            if (sgRid < 1)
            {
                ErrorProvider.SetError(cboDCCartonRoundingAttribute, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                methodFieldsValid = false;
            }
            // End TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 
			
			return methodFieldsValid;
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			if (!WorkflowMethodNameValid)
			{
				ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
			}
			else
			{
				ErrorProvider.SetError (txtName,string.Empty);
			}
			if (!WorkflowMethodDescriptionValid)
			{
				ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
			}
			else
			{
				ErrorProvider.SetError (txtDesc,string.Empty);
			}
			if (!UserGlobalValid)
			{
				ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
			}
			else
			{
				ErrorProvider.SetError (pnlGlobalUser,string.Empty);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _dcCartonRoundingMethod;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			return ExplorerNode;
		}

		#endregion WorkflowMethodFormBase Overrides		

		#region IFormBase Members
		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
		#endregion

		override protected bool ApplySecurity()	// Track 5871 stodd
		{
			bool securityOk = true; // track #5871 stodd
			if (FunctionSecurity.AllowUpdate)
			{
				btnSave.Enabled = true;
			}
			else
			{
				btnSave.Enabled = false;
			}

			if (FunctionSecurity.AllowExecute)
			{
				btnProcess.Enabled = true;
			}
			else
			{
				btnProcess.Enabled = false;
			}
			return securityOk;	// Track 5871 stodd
		}

        private void ugWorkflows_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        }

	}
}
 
 	
 
 

