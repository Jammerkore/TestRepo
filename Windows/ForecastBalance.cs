using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ForecastBalance.
	/// </summary>
	public class frmForecastBalance : MIDFormBase
	{
		private System.Windows.Forms.Label lblIterations;
		private System.Windows.Forms.Label lblVariable;
		private System.Windows.Forms.GroupBox grpBalanceMode;
		private System.Windows.Forms.RadioButton radBalanceToChain;
		private System.Windows.Forms.RadioButton radBalanceToStore;
		private System.Windows.Forms.Label lblUsing;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SessionAddressBlock _SAB;
		private PlanOpenParms _openParms;
		private bool _balanceSuccessful = false;
        private MIDComboBoxEnh cboVariable;
        private MIDComboBoxEnh cboIterations;
        private MIDComboBoxEnh cboUsing;
		private PlanCubeGroup _planCubeGroup;

		/// <summary>
		/// Gets the flag identifying if the balance was successful.
		/// </summary>
		public bool BalanceSuccessful 
		{
			get { return _balanceSuccessful ; }
		}

		public frmForecastBalance(SessionAddressBlock aSAB, PlanOpenParms aOpenParms, PlanCubeGroup aPlanCubeGroup)
			: base(aSAB)
		{
			_SAB = aSAB;
			_openParms = aOpenParms;
			_planCubeGroup = aPlanCubeGroup;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.lblUsing = new System.Windows.Forms.Label();
			this.lblIterations = new System.Windows.Forms.Label();
			this.lblVariable = new System.Windows.Forms.Label();
			this.grpBalanceMode = new System.Windows.Forms.GroupBox();
			this.radBalanceToChain = new System.Windows.Forms.RadioButton();
			this.radBalanceToStore = new System.Windows.Forms.RadioButton();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
            this.cboVariable = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboIterations = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboUsing = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.grpBalanceMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
			// lblUsing
			// 
			this.lblUsing.Location = new System.Drawing.Point(32, 144);
			this.lblUsing.Name = "lblUsing";
			this.lblUsing.Size = new System.Drawing.Size(56, 23);
			this.lblUsing.TabIndex = 12;
			this.lblUsing.Text = "Using:";
			this.lblUsing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblIterations
			// 
			this.lblIterations.Location = new System.Drawing.Point(24, 48);
			this.lblIterations.Name = "lblIterations";
			this.lblIterations.Size = new System.Drawing.Size(64, 23);
			this.lblIterations.TabIndex = 10;
			this.lblIterations.Text = "Iterations:";
			this.lblIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblVariable
			// 
			this.lblVariable.Location = new System.Drawing.Point(32, 8);
			this.lblVariable.Name = "lblVariable";
			this.lblVariable.Size = new System.Drawing.Size(56, 23);
			this.lblVariable.TabIndex = 7;
			this.lblVariable.Text = "Variable:";
			this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpBalanceMode
			// 
			this.grpBalanceMode.Controls.Add(this.radBalanceToChain);
			this.grpBalanceMode.Controls.Add(this.radBalanceToStore);
			this.grpBalanceMode.Location = new System.Drawing.Point(40, 80);
			this.grpBalanceMode.Name = "grpBalanceMode";
			this.grpBalanceMode.Size = new System.Drawing.Size(232, 48);
			this.grpBalanceMode.TabIndex = 9;
			this.grpBalanceMode.TabStop = false;
			this.grpBalanceMode.Text = "Balance Mode";
			// 
			// radBalanceToChain
			// 
			this.radBalanceToChain.Location = new System.Drawing.Point(112, 16);
			this.radBalanceToChain.Name = "radBalanceToChain";
            this.radBalanceToChain.Size = new System.Drawing.Size(104, 24);
			this.radBalanceToChain.TabIndex = 1;
			this.radBalanceToChain.Text = "Chain";
			// 
			// radBalanceToStore
			// 
			this.radBalanceToStore.Location = new System.Drawing.Point(16, 16);
			this.radBalanceToStore.Name = "radBalanceToStore";
            this.radBalanceToStore.Size = new System.Drawing.Size(104, 24);
			this.radBalanceToStore.TabIndex = 0;
			this.radBalanceToStore.Text = "Store";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(208, 200);
			this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 17;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(120, 200);
			this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 16;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
            // cboVariable
            // 
            this.cboVariable.AutoAdjust = true;
            this.cboVariable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariable.DataSource = null;
            this.cboVariable.DisplayMember = null;
            this.cboVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariable.DropDownWidth = 0;
            this.cboVariable.Location = new System.Drawing.Point(96, 8);
            this.cboVariable.Name = "cboVariable";
            this.cboVariable.Size = new System.Drawing.Size(192, 21);
            this.cboVariable.TabIndex = 8;
            this.cboVariable.Tag = null;
            this.cboVariable.ValueMember = null;
            // 
            // cboIterations
            // 
            this.cboIterations.AutoAdjust = true;
            this.cboIterations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboIterations.DataSource = null;
            this.cboIterations.DisplayMember = null;
            this.cboIterations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIterations.DropDownWidth = 0;
            this.cboIterations.Location = new System.Drawing.Point(96, 48);
            this.cboIterations.Name = "cboIterations";
            this.cboIterations.Size = new System.Drawing.Size(96, 21);
            this.cboIterations.TabIndex = 11;
            this.cboIterations.Tag = null;
            this.cboIterations.ValueMember = null;
            // 
            // cboUsing
            // 
            this.cboUsing.AutoAdjust = true;
            this.cboUsing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUsing.DataSource = null;
            this.cboUsing.DisplayMember = null;
            this.cboUsing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsing.DropDownWidth = 0;
            this.cboUsing.Location = new System.Drawing.Point(96, 144);
            this.cboUsing.Name = "cboUsing";
            this.cboUsing.Size = new System.Drawing.Size(160, 21);
            this.cboUsing.TabIndex = 13;
            this.cboUsing.Tag = null;
            this.cboUsing.ValueMember = null;
            // 
			// frmForecastBalance
			// 
            this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 230);
            this.Controls.Add(this.cboUsing);
            this.Controls.Add(this.cboIterations);
            this.Controls.Add(this.cboVariable);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblVariable);
			this.Controls.Add(this.lblIterations);
			this.Controls.Add(this.grpBalanceMode);
			this.Controls.Add(this.lblUsing);
			this.Name = "frmForecastBalance";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ForecastBalance";
			this.Load += new System.EventHandler(this.frmForecastBalance_Load);
            this.Controls.SetChildIndex(this.lblUsing, 0);
            this.Controls.SetChildIndex(this.grpBalanceMode, 0);
            this.Controls.SetChildIndex(this.lblIterations, 0);
            this.Controls.SetChildIndex(this.lblVariable, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.cboVariable, 0);
            this.Controls.SetChildIndex(this.cboIterations, 0);
            this.Controls.SetChildIndex(this.cboUsing, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.grpBalanceMode.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmForecastBalance_Load(object sender, System.EventArgs e)
		{
			try
			{
				Icon = MIDGraphics.GetIcon(MIDGraphics.BalanceImage);
				SetText();
				BindVariableComboBox();
				BindIterationComboBox();
				BindUsingComboBox();
				cboVariable.SelectedIndex = 0;
				cboIterations.SelectedIndex = 2;
				cboUsing.SelectedIndex = 0;
				radBalanceToStore.Checked = true;
			}
			catch(Exception exc )
			{
				HandleException(exc);
			}
		}

		private void SetText()
		{
			try
			{
				this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSForecastBalance);
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.radBalanceToStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore);
				this.radBalanceToChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain);
				this.lblUsing.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Using) + ":";
				this.lblVariable.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable) + ":";
				this.lblIterations.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Iterations) + ":";
				this.grpBalanceMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BalanceMode) + ":";
	
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void BindVariableComboBox()
		{
			try
			{
				cboVariable.Items.Clear();
				foreach (VariableProfile vp in _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
				{
					if (vp.AllowForecastBalance)
					{
						cboVariable.Items.Add(new VariableCombo(vp.Key, vp.VariableName));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindIterationComboBox()
		{
			try
			{
				cboIterations.Items.Clear();
				for (int i=1; i<10; i++)
				{
					cboIterations.Items.Add(
						new IterationsCombo(eIterationType.Custom, i));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindUsingComboBox()
		{
			try
			{
				cboUsing.Items.Clear();
				cboUsing.Items.Add(new UsingCombo(ePlanBasisType.Plan, -1));
				for (int i=0; i<_openParms.BasisProfileList.Count; i++)
				{
					if (_openParms.IsPlanBasisTimeLengthEqual(_SAB.ClientServerSession, i))
					{
						cboUsing.Items.Add(new UsingCombo(ePlanBasisType.Basis, i));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
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
				int variableNumber;
				eBalanceMode balanceMode;
				int iterationCount;
				ePlanBasisType planBasisType;
				int basisIndex;

				Cursor.Current = Cursors.WaitCursor;
				variableNumber = ((VariableCombo)cboVariable.SelectedItem).VariableNumber;

				if (radBalanceToChain.Checked == true)
				{
					balanceMode = eBalanceMode.Chain;
				}
				else
				{
					balanceMode = eBalanceMode.Store;
				}

				if (cboIterations.SelectedIndex != -1)
				{
					iterationCount = ((IterationsCombo)cboIterations.SelectedItem).IterationCount;
				}
				else
				{
					iterationCount = 3;
				}

				if (cboUsing.SelectedIndex != -1)
				{
					planBasisType = ((UsingCombo)cboUsing.SelectedItem).PlanBasisType;
					basisIndex = ((UsingCombo)cboUsing.SelectedItem).BasisIndex;
				}
				else
				{
					planBasisType = ePlanBasisType.Plan;
					basisIndex = -1;
				}

				if (_planCubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))  
				{
					((StoreMultiLevelPlanMaintCubeGroup)_planCubeGroup).MatrixBalance(variableNumber, balanceMode, iterationCount, planBasisType, basisIndex);
				}
				_balanceSuccessful = true;
				this.Close();
			}
			catch(SpreadFailed err)
			{
				_balanceSuccessful = false;
				// backout all changes that were applied
				if (err.RecomputesProcessed > 0)
				{
					for (int i=0; i<err.RecomputesProcessed; i++)
					{
						_planCubeGroup.UndoLastRecompute();
					}
				}
				MessageBox.Show(err.Message, this.Text);
			}
			catch(Exception exc)
			{
				_balanceSuccessful = false;
				_planCubeGroup.UndoLastRecompute();
				HandleException(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}
}
