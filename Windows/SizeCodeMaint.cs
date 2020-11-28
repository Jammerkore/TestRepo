using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using MIDRetail.Business;   
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeCodeMaint.
	/// </summary>
	public class frmSizeCodeMaint : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private string _sizeCode;
		private bool _codeAdded;
		private EditMsgs _editMessages;
		private bool _cancelClicked = false;

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblSizeCode;
		private System.Windows.Forms.TextBox txtSizeCode;
		private System.Windows.Forms.Label lblPrimary;
		private System.Windows.Forms.Label lblSecondary;
		private System.Windows.Forms.Label lblProductCategory;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxProductCategory;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxPrimary;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxSecondary;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Gets or sets the size code.
		/// </summary>
		public string SizeCode 
		{
			get { return _sizeCode ; }
			set { _sizeCode = value ; }
		}
		/// <summary>
		/// Gets the flag identifying if the code was added.
		/// </summary>
		public bool CodeAdded 
		{
			get { return _codeAdded ; }
		}
		/// <summary>
		/// Gets the flag identifying if the cancel button was clicked.
		/// </summary>
		public bool CancelClicked 
		{
			get { return _cancelClicked ; }
		}
		/// <summary>
		/// Gets the object containing the edit messages.
		/// </summary>
		public EditMsgs EditMessages 
		{
			get { return _editMessages ; }
		}

		public frmSizeCodeMaint(SessionAddressBlock aSAB) : base (aSAB)
		{
			_SAB = aSAB;
			_editMessages = new EditMsgs();
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

				this.Load -= new System.EventHandler(this.SizeCodeMaint_Load);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);

                this.cbxProductCategory.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxProductCategory_MIDComboBoxPropertiesChangedEvent);
                this.cbxPrimary.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxPrimary_MIDComboBoxPropertiesChangedEvent);
                this.cbxSecondary.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxSecondary_MIDComboBoxPropertiesChangedEvent);
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblSizeCode = new System.Windows.Forms.Label();
			this.txtSizeCode = new System.Windows.Forms.TextBox();
			this.lblPrimary = new System.Windows.Forms.Label();
			this.lblSecondary = new System.Windows.Forms.Label();
			this.lblProductCategory = new System.Windows.Forms.Label();
			this.cbxProductCategory = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cbxPrimary = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cbxSecondary = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(332, 228);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(244, 228);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblSizeCode
			// 
			this.lblSizeCode.Location = new System.Drawing.Point(56, 48);
			this.lblSizeCode.Name = "lblSizeCode";
			this.lblSizeCode.TabIndex = 7;
			this.lblSizeCode.Text = "Size Code:";
			this.lblSizeCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtSizeCode
			// 
			this.txtSizeCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSizeCode.Location = new System.Drawing.Point(160, 51);
			this.txtSizeCode.Name = "txtSizeCode";
			this.txtSizeCode.Size = new System.Drawing.Size(232, 20);
			this.txtSizeCode.TabIndex = 8;
			this.txtSizeCode.Text = "";
			// 
			// lblPrimary
			// 
			this.lblPrimary.Location = new System.Drawing.Point(56, 128);
			this.lblPrimary.Name = "lblPrimary";
			this.lblPrimary.TabIndex = 10;
			this.lblPrimary.Text = "Primary:";
			this.lblPrimary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSecondary
			// 
			this.lblSecondary.Location = new System.Drawing.Point(56, 168);
			this.lblSecondary.Name = "lblSecondary";
			this.lblSecondary.TabIndex = 11;
			this.lblSecondary.Text = "Secondary:";
			this.lblSecondary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblProductCategory
			// 
			this.lblProductCategory.Location = new System.Drawing.Point(56, 88);
			this.lblProductCategory.Name = "lblProductCategory";
			this.lblProductCategory.TabIndex = 12;
			this.lblProductCategory.Text = "Product Category:";
			this.lblProductCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbxProductCategory
			// 
			this.cbxProductCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cbxProductCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxProductCategory.Location = new System.Drawing.Point(160, 89);
			this.cbxProductCategory.Name = "cbxProductCategory";
			this.cbxProductCategory.Size = new System.Drawing.Size(232, 21);
			this.cbxProductCategory.TabIndex = 13;
			this.cbxProductCategory.SelectionChangeCommitted += new System.EventHandler(this.cbxProductCategory_SelectionChangeCommitted);
            this.cbxProductCategory.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxProductCategory_MIDComboBoxPropertiesChangedEvent);
			// 
			// cbxPrimary
			// 
			this.cbxPrimary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cbxPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPrimary.Location = new System.Drawing.Point(160, 129);
			this.cbxPrimary.Name = "cbxPrimary";
			this.cbxPrimary.Size = new System.Drawing.Size(232, 21);
			this.cbxPrimary.TabIndex = 14;
			this.cbxPrimary.SelectionChangeCommitted += new System.EventHandler(this.cbxPrimary_SelectionChangeCommitted);
            this.cbxPrimary.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxPrimary_MIDComboBoxPropertiesChangedEvent);
			// 
			// cbxSecondary
			// 
			this.cbxSecondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cbxSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSecondary.Location = new System.Drawing.Point(160, 169);
			this.cbxSecondary.Name = "cbxSecondary";
			this.cbxSecondary.Size = new System.Drawing.Size(232, 21);
			this.cbxSecondary.TabIndex = 15;
			this.cbxSecondary.SelectionChangeCommitted += new System.EventHandler(this.cbxSecondary_SelectionChangeCommitted);
            this.cbxSecondary.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxSecondary_MIDComboBoxPropertiesChangedEvent);
			// 
			// frmSizeCodeMaint
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 262);
			this.Controls.Add(this.cbxSecondary);
			this.Controls.Add(this.cbxPrimary);
			this.Controls.Add(this.cbxProductCategory);
			this.Controls.Add(this.lblProductCategory);
			this.Controls.Add(this.lblSecondary);
			this.Controls.Add(this.lblPrimary);
			this.Controls.Add(this.txtSizeCode);
			this.Controls.Add(this.lblSizeCode);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "frmSizeCodeMaint";
			this.Text = "SizeCodeMaint";
			this.Load += new System.EventHandler(this.SizeCodeMaint_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SizeCodeMaint_Load(object sender, System.EventArgs e)
		{
			try
			{
				txtSizeCode.Text = _sizeCode;
				SetText();
				Format_Title(eDataState.New, eMIDTextCode.frm_SizeCodeMaint, _sizeCode);
				PopulateComboBoxes();
				StartPosition = FormStartPosition.CenterScreen;
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
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.lblSizeCode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCode) + ":";
				this.lblProductCategory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeProductCategory) + ":";
				this.lblPrimary.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizePrimary) + ":";
				this.lblSecondary.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeSecondary) + ":";
			}
			catch
			{
				throw;
			}
		}

		private void PopulateComboBoxes()
		{
			ComboObject co = null;
			DataTable dt = null;
			string field = null;
			try
			{
				int key = 0;
				SizeGroup sizeData = new SizeGroup();

				co = new ComboObject(0, MIDText.GetTextOnly(eMIDTextCode.lbl_New));
				cbxProductCategory.Items.Add(co);
				dt = sizeData.ProductCategory_Read();
				foreach(DataRow dr in dt.Rows)
				{
					field = Convert.ToString(dr["SIZE_CODE_PRODUCT_CATEGORY"], CultureInfo.CurrentCulture).Trim();
					if (field.Length > 0)
					{
						++key;
						co = new ComboObject(key, field);
						cbxProductCategory.Items.Add(co);
					}
				}

				co = new ComboObject(0, MIDText.GetTextOnly(eMIDTextCode.lbl_New));
				cbxPrimary.Items.Add(co);
				dt = sizeData.PrimarySizes_Read();
				foreach(DataRow dr in dt.Rows)
				{
					field = Convert.ToString(dr["SIZE_CODE_PRIMARY"], CultureInfo.CurrentCulture).Trim();
					if (field.Length > 0)
					{
						co = new ComboObject(Convert.ToInt32(dr["SIZES_RID"], CultureInfo.CurrentCulture), field);
						cbxPrimary.Items.Add(co);
					}
				}

				co = new ComboObject(0, MIDText.GetTextOnly(eMIDTextCode.lbl_New));
				cbxSecondary.Items.Add(co);
				dt = sizeData.SecondarySizes_Read();
				foreach(DataRow dr in dt.Rows)
				{
					field = Convert.ToString(dr["SIZE_CODE_SECONDARY"], CultureInfo.CurrentCulture).Trim();
					if (field.Length > 0)
					{
						co = new ComboObject(Convert.ToInt32(dr["DIMENSIONS_RID"], CultureInfo.CurrentCulture), field);
						cbxSecondary.Items.Add(co);
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
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
				if (!ErrorFound)
				{
					this.Close();
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				_cancelClicked = true;
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			HierarchyMaintenance hm;
		
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (!DataValid())
				{
					hm = new HierarchyMaintenance(_SAB); 
					SizeCodeProfile scp = new SizeCodeProfile(Include.NoRID);
					scp.SizeCodeChangeType = eChangeType.add;
					scp.SizeCodeID = txtSizeCode.Text;
					scp.SizeCodeProductCategory = cbxProductCategory.Text;
					scp.SizeCodePrimary = cbxPrimary.Text;
					scp.SizeCodeSecondary = cbxSecondary.Text;
//					if (scp.SizeCodePrimary != null &&
//						scp.SizeCodePrimary.Trim().Length > 0)
//					{
//						scp.SizeCodeName = scp.SizeCodePrimary;
//					}
//					else if (scp.SizeCodeSecondary != null &&
//						scp.SizeCodeSecondary.Trim().Length > 0)
//					{
//						scp.SizeCodeName = scp.SizeCodeSecondary;
//					}
//					else 
//					{
//						scp.SizeCodeName = scp.SizeCodeID;
//					}
					scp.SizeCodeName = Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID);
					scp = hm.SizeCodeUpdate(ref _editMessages, scp);
					_codeAdded = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}

			return ErrorFound;
		}

		private bool DataValid()
		{
			try
			{
				ErrorFound = false;
				ErrorProvider.SetError (txtSizeCode,null);
				ErrorProvider.SetError (cbxProductCategory,null);
				ErrorProvider.SetError (cbxPrimary,null);
				ErrorProvider.SetError (cbxSecondary,null);
				if (cbxPrimary.Text.Length == 0)
				{
					ErrorProvider.SetError (cbxPrimary,MIDText.GetText(eMIDTextCode.msg_SizePrimaryRequired));
					ErrorFound = true;
				}

				int sizeCodeRID = _SAB.HierarchyServerSession.GetSizeCodeRID(cbxProductCategory.Text, cbxPrimary.Text, cbxSecondary.Text);
				if (sizeCodeRID > Include.NoRID)
				{
					SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeRID);
					if (scp.SizeCodeID != txtSizeCode.Text)
					{
						string message = MIDText.GetText(eMIDTextCode.msg_SizeCatgPriSecNotUnique);
						message = message.Replace("{0}", cbxProductCategory.Text);
						message = message.Replace("{1}", cbxPrimary.Text);
						if (cbxSecondary.Text != null &&
							cbxSecondary.Text.Trim().Length > 1)
						{
							message = message.Replace("{2}", cbxSecondary.Text);
						}
						else
						{
							message = message.Replace("{2}", MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize));
						}
						message = message.Replace("{3}", scp.SizeCodeID);
						ErrorProvider.SetError (txtSizeCode,message);
						ErrorFound = true;
					}
				}
				if (ErrorFound)
				{
					MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return ErrorFound;
			}
			catch
			{
				throw;
			}
		}

		private void cbxProductCategory_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (cbxProductCategory.SelectedIndex > -1)
					{
						ComboObject co = (ComboObject)cbxProductCategory.SelectedItem;
						if (co.Key ==0)
						{
							cbxProductCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
						}
					}
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cbxPrimary_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (cbxPrimary.SelectedIndex > -1)
					{
						ComboObject co = (ComboObject)cbxPrimary.SelectedItem;
						if (co.Key ==0)
						{
							cbxPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
						}
					}
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cbxSecondary_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (cbxSecondary.SelectedIndex > -1)
					{
						ComboObject co = (ComboObject)cbxSecondary.SelectedItem;
						if (co.Key ==0)
						{
							cbxSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
						}
					}
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        private void cbxProductCategory_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxProductCategory_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cbxPrimary_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxPrimary_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cbxSecondary_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxSecondary_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
