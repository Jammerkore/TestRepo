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
	/// Summary description for AssortmentWorkspaceExplorerFilter.
	/// </summary>
	public partial class AssortmentWorkspaceExplorerFilter : MIDFormBase
	{
		// add event to update explorer when node is changed
		public delegate void AssrtWorkspaceFilterChangeEventHandler(object source, AssrtWorkspaceFilterChangeEventArgs e);
		public event AssrtWorkspaceFilterChangeEventHandler OnAssrtWorkspaceFilterChangeHandler;

		private SessionAddressBlock _SAB;
		private System.Windows.Forms.CheckedListBox _rightMouseListBox;
		private AssortmentWorkspaceFilterProfile _awfp;
		private TabPage _currentTabPage = null;
		private bool _continueWithSave;
        private bool _skipItemCheck;
        private int _inUseStatusIndex;
        private bool _validateMerchandise = true;
        private HierarchyProfile _mainHp;			 
        private HierarchyLevelProfile _hlpStyle;	 
		

		public AssortmentWorkspaceExplorerFilter(SessionAddressBlock aSAB) : base (aSAB)
		{
			AllowDragDrop = true;
			_SAB = aSAB;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

	    private void AssortmentWorkspaceExplorerFilter_Load(object sender, System.EventArgs e)
		{
			FormLoaded = false;
			FunctionSecurity = new FunctionSecurityProfile(Include.NoRID);
            FunctionSecurity.SetAllowUpdate();
            SetReadOnly(true);

			_awfp = new AssortmentWorkspaceFilterProfile(_SAB.ClientServerSession.UserRID);

			Format_Title(eDataState.Updatable, eMIDTextCode.frm_AssortmentWorkspaceFilter, _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID));

            _mainHp = _SAB.HierarchyServerSession.GetMainHierarchyData();
            for (int level = 1; level <= _mainHp.HierarchyLevels.Count; level++)
            {
                _hlpStyle = (HierarchyLevelProfile)_mainHp.HierarchyLevels[level];
                if (_hlpStyle.LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }
         
			// load merchandise
			txtMerchandise.Tag = _awfp.HnRID;
			if (_awfp.HnRID > Include.NoRID)
			{
				txtMerchandise.Text = _SAB.HierarchyServerSession.GetNodeData(_awfp.HnRID, true, true).Text;
		    }

			txtMaximumHeaders.Text = _awfp.MaximumHeaders.ToString(CultureInfo.CurrentCulture);
			
			// load header status
			eHeaderAllocationStatus status;
            string text = string.Empty;
            bool selected = false;
            bool multiChecked = false; 
			DataTable dtStatus = MIDText.GetTextType(eMIDTextType.eHeaderAllocationStatus, eMIDTextOrderBy.TextValue);
			foreach (DataRow dr in dtStatus.Rows)
			{
				status = (eHeaderAllocationStatus)Convert.ToInt32(dr["TEXT_CODE"],CultureInfo.CurrentUICulture);
				text = Convert.ToString(dr["TEXT_VALUE"],CultureInfo.CurrentUICulture);
				HeaderStatus hs = (HeaderStatus)_awfp.SelectedStatuses[status];
                
                if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                {
                    selected = multiChecked;
                }
                else  
                {
                    if (hs == null)
                    {
                        selected = true;
                    }
                    else
                    {
                        selected = hs.IsDisplayed;
                    }
                }
				// if size, use all statuses
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
				}
				else
				{
					// remove all size statuses
					if (Enum.IsDefined(typeof(eNonSizeHeaderAllocationStatus), Convert.ToInt32(status,CultureInfo.CurrentUICulture)))
					{
						lstStatus.Items.Add(new MIDListBoxItem(text, status), selected);
					}
				}
             
                if (status == eHeaderAllocationStatus.InUseByMultiHeader)
                {
                    _inUseStatusIndex = lstStatus.Items.Count - 1;
                }
     		}

			// load header date
			switch (_awfp.HeaderDateType)
			{
				case eFilterDateType.specify:
					radHeaderDateSpecify.Checked = true;
					dtpHeaderDateFrom.Value = _awfp.HeaderDateFrom;
					dtpHeaderDateTo.Value = _awfp.HeaderDateTo;
					break;
				case eFilterDateType.today:
					radHeaderDateToday.Checked = true;
					break;
				case eFilterDateType.between:
					radHeaderDateBetween.Checked = true;
					numHeaderDateBetweenFrom.Value = _awfp.HeaderDateBetweenFrom;
					numHeaderDateBetweenTo.Value = _awfp.HeaderDateBetweenTo;
					break;
				case eFilterDateType.all:
					radHeaderDateAll.Checked = true;
					break;
			}

			// load release date
			switch (_awfp.ReleaseDateType)
			{
				case eFilterDateType.specify:
					radReleaseDateSpecify.Checked = true;
					dtpReleaseDateFrom.Value = _awfp.ReleaseDateFrom;
					dtpReleaseDateTo.Value = _awfp.ReleaseDateTo;
					break;
				case eFilterDateType.today:
					radReleaseDateToday.Checked = true;
					break;
				case eFilterDateType.between:
					radReleaseDateBetween.Checked = true;
					numReleaseDateBetweenFrom.Value = _awfp.ReleaseDateBetweenFrom;
					numReleaseDateBetweenTo.Value = _awfp.ReleaseDateBetweenTo;
					break;
				case eFilterDateType.all:
					radReleaseDateAll.Checked = true;
					break;
			}

			SetText();
			
			_currentTabPage = this.tabGeneral;
			FormLoaded = true;
		}

		private void SetText()
		{
			try
			{
                lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
                lblMaximum.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Maximum) + " " +
                                  MIDText.GetTextOnly(eMIDTextCode.lbl_Assortments) + ":";
            }
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveChanges();
				if (!ErrorFound && _continueWithSave)
				{
					this.Close();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				ErrorFound = false;
				bool filterValid = true;
				_continueWithSave = true;
				switch (_currentTabPage.Name)
				{
					case "tabGeneral":
						filterValid = ValidateGeneralTab();
						break;
					case "tabDates":
						filterValid = ValidateDatesTab();
						break;
				}

				if (_continueWithSave)
				{
					if (filterValid)
					{
						if (txtMerchandise.Tag == null)
						{
							_awfp.HnRID = Include.NoRID;
						}
						else
						{
							_awfp.HnRID = Convert.ToInt32(txtMerchandise.Tag, CultureInfo.CurrentCulture);
						}

						int i;
						MIDListBoxItem mlbi;

						for (i = 0; i < lstStatus.Items.Count; i++)
						{
							bool isChecked = lstStatus.GetItemChecked(i);
							mlbi = (MIDListBoxItem)lstStatus.Items[i];
							HeaderStatus hs = (HeaderStatus)_awfp.SelectedStatuses[(eHeaderAllocationStatus)mlbi.Tag];
							hs.IsDisplayed = isChecked;
						}

						// set header dates
						_awfp.HeaderDateBetweenFrom = Convert.ToInt32(numHeaderDateBetweenFrom.Value, CultureInfo.CurrentCulture);
						_awfp.HeaderDateBetweenTo = Convert.ToInt32(numHeaderDateBetweenTo.Value, CultureInfo.CurrentCulture);
						_awfp.HeaderDateFrom = dtpHeaderDateFrom.Value;
						_awfp.HeaderDateTo = dtpHeaderDateTo.Value;

						// set release dates
						_awfp.ReleaseDateBetweenFrom = Convert.ToInt32(numReleaseDateBetweenFrom.Value, CultureInfo.CurrentCulture);
						_awfp.ReleaseDateBetweenTo = Convert.ToInt32(numReleaseDateBetweenTo.Value, CultureInfo.CurrentCulture);
						_awfp.ReleaseDateFrom = dtpReleaseDateFrom.Value;
						_awfp.ReleaseDateTo = dtpReleaseDateTo.Value;

						_awfp.WriteFilter();
						ChangePending = false;

						AssrtWorkspaceFilterChangeEventArgs ea = new AssrtWorkspaceFilterChangeEventArgs();
						if (OnAssrtWorkspaceFilterChangeHandler != null)  // throw event to explorer to apply changes
						{
							OnAssrtWorkspaceFilterChangeHandler(this, ea);
						}
					}
					else
					{
						ErrorFound = true;
					}
				}
			}
			catch (Exception exc)
			{
				ErrorFound = true;
				HandleException(exc);
			}

			return ErrorFound;
		}

		private bool ValidateGeneralTab()
		{
			bool generalTabValid = true;
			string errorMessage = null;
			try
			{
				ErrorProvider.SetError (this.txtMerchandise,"");
               
                if (txtMerchandise.Text.Trim().Length > 0)
                {
                    HierarchyNodeProfile hnp;
                    if (txtMerchandise.Tag == null || (int)txtMerchandise.Tag == Include.NoRID)
                    {
                        //HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(txtMerchandise.Text);
                        hnp = _SAB.HierarchyServerSession.GetNodeData(txtMerchandise.Text, true, true);
                    }
                    else
                    {
                        hnp = _SAB.HierarchyServerSession.GetNodeData((int)txtMerchandise.Tag, true, true);
                    }
                    if (hnp.Key == Include.NoRID)
                    {
                        generalTabValid = false;
                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MerchandiseInvalid);
                        ErrorProvider.SetError(this.txtMerchandise, errorMessage);
                    }
                    else if (hnp.HomeHierarchyType == eHierarchyType.organizational &&
                             hnp.HomeHierarchyLevel > _hlpStyle.Level)
                    {
                        generalTabValid = false;
                        errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_InvalidMerchandiseLevel), _hlpStyle.LevelID);
                        ErrorProvider.SetError(this.txtMerchandise, errorMessage);
                    }
                    else
                    {
                        txtMerchandise.Text = hnp.Text;
                        txtMerchandise.Tag = hnp.Key;
                    }
                }
                else
                {
                    txtMerchandise.Tag = null;
                    txtMerchandise.Text = null;
                }
        
				ErrorProvider.SetError (this.txtMaximumHeaders,"");
				if (txtMaximumHeaders.Text.Length > 0)
				{
					try
					{
						_awfp.MaximumHeaders = Convert.ToInt32(txtMaximumHeaders.Text);
						if (_awfp.MaximumHeaders > 5000)
						{
							if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderMaximumWarning), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly)
								== DialogResult.No) 
							{
								_continueWithSave = false;
							}
						}
					}
					catch
					{
						generalTabValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						ErrorProvider.SetError (this.txtMaximumHeaders,errorMessage);
					}
				}
				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return generalTabValid;
		}

		private bool ValidateDatesTab()
		{
			bool datesTabValid = true;
			string errorMessage = null;
			try
			{
				ErrorProvider.SetError (this.lblHeaderDateBetweenDays,"");
				if (radHeaderDateBetween.Checked)
				{
					if (numHeaderDateBetweenFrom.Value > numHeaderDateBetweenTo.Value)
					{
						datesTabValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
						ErrorProvider.SetError (this.lblHeaderDateBetweenDays,errorMessage);
					}
				}

				ErrorProvider.SetError (this.dtpHeaderDateFrom,"");
				if (radHeaderDateSpecify.Checked)
				{
					if (dtpHeaderDateFrom.Value > dtpHeaderDateTo.Value)
					{
						datesTabValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
						ErrorProvider.SetError (this.dtpHeaderDateFrom,errorMessage);
					}
				}

				ErrorProvider.SetError (this.lblReleaseDateBetweenDays,"");
				if (radReleaseDateBetween.Checked)
				{
					if (numReleaseDateBetweenFrom.Value > numReleaseDateBetweenTo.Value)
					{
						datesTabValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
						ErrorProvider.SetError (lblReleaseDateBetweenDays,errorMessage);
					}
				}

				ErrorProvider.SetError (this.dtpReleaseDateFrom,"");
				if (radReleaseDateSpecify.Checked)
				{
					if (dtpReleaseDateFrom.Value > dtpReleaseDateTo.Value)
					{
						datesTabValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
						ErrorProvider.SetError (this.dtpReleaseDateFrom,errorMessage);
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			return datesTabValid;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtMerchandise_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
                    txtMerchandise.Text = txtMerchandise.Text.Trim();
					if (txtMerchandise.Text.Trim().Length == 0)
					{
						txtMerchandise.Tag = null;
					}
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void txtMerchandise_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage = null;
            try
            {
                ErrorProvider.SetError(this.txtMerchandise, "");
                if (!_validateMerchandise)
                {
                    return;
                }
                if (txtMerchandise.Text.Length > 0)
                {
                    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(txtMerchandise.Text);
                    if (hnp.Key == Include.NoRID)
                    {
                        e.Cancel = true;
                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MerchandiseInvalid);
                        ErrorProvider.SetError(this.txtMerchandise, errorMessage);
                    }
                    else
                    {
                        txtMerchandise.Text = hnp.Text;
                        txtMerchandise.Tag = hnp.Key;
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

		private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			TreeNodeClipboardList cbList;
            try
            {
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                   
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                        string previousText = txtMerchandise.Text;
                        object previousTag = txtMerchandise.Tag;
                     
                        txtMerchandise.Text = hnp.Text;
                        txtMerchandise.Tag = cbList.ClipboardProfile.Key;
                        _validateMerchandise = false;
                        ChangePending = true;
                        if (hnp.HomeHierarchyLevel == 0)
                        {
                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PerformanceWarning), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        
                        if (hnp.HomeHierarchyType == eHierarchyType.organizational &&
                            hnp.HomeHierarchyLevel > _hlpStyle.Level)
                        {
                            string errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_InvalidMerchandiseLevel), _hlpStyle.LevelID);
                            MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMerchandise.Text = previousText;
                            txtMerchandise.Tag = previousTag;
                        }
                    }
                    else
                    {
                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                    }
                }
                else
                {
                    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            finally
            {
                txtMerchandise.Invalidate();
                txtMerchandise.SelectionLength = 0;
            }
        }  

		private void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Merchandise_DragEnter(sender, e);
		}

		private void txtMerchandise_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void mniClearAll_Click(object sender, System.EventArgs e)
		{
			int i;
			try
			{
				if (_rightMouseListBox.Name == "lstStatus")
				{
					for (i = 0; i < lstStatus.Items.Count; i++)
					{
                        lstStatus.SetSelected(i, true);      // MID Track #5974 - null reference from context menu
						lstStatus.SetItemChecked(i, false);
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mniSelectAll_Click(object sender, System.EventArgs e)
		{
			int i;
			try
			{
				if (_rightMouseListBox.Name == "lstStatus")
				{
					for (i = 0; i < lstStatus.Items.Count; i++)
					{
                        lstStatus.SetSelected(i, true);     // MID Track #5974 - null reference from context menu
						lstStatus.SetItemChecked(i, true);
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mniRestoreDefaults_Click(object sender, System.EventArgs e)
		{
			int i;
			try
			{
				if (_rightMouseListBox.Name == "lstStatus")
				{
					for (i = 0; i < lstStatus.Items.Count; i++)
					{
						MIDListBoxItem mlbi = (MIDListBoxItem)lstStatus.Items[i];
						HeaderStatus hs = (HeaderStatus)_awfp.SelectedStatuses[(eHeaderAllocationStatus)mlbi.Tag];
                        lstStatus.SetSelected(i, true);      // MID Track #5974 - null reference from context menu
						lstStatus.SetItemChecked(i, hs.DefaultValue);
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lst_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					if (sender is CheckedListBox)
					{
						_rightMouseListBox = (CheckedListBox)sender;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radHeaderDateToday_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numHeaderDateBetweenFrom.Enabled = false;
				numHeaderDateBetweenTo.Enabled = false;
				dtpHeaderDateFrom.Enabled = false;
				dtpHeaderDateTo.Enabled = false;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.HeaderDateType = eFilterDateType.today;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radHeaderDateBetween_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numHeaderDateBetweenFrom.Enabled = true;
				numHeaderDateBetweenTo.Enabled = true;
				dtpHeaderDateFrom.Enabled = false;
				dtpHeaderDateTo.Enabled = false;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.HeaderDateType = eFilterDateType.between;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radHeaderDateSpecify_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numHeaderDateBetweenFrom.Enabled = false;
				numHeaderDateBetweenTo.Enabled = false;
				dtpHeaderDateFrom.Enabled = true;
				dtpHeaderDateTo.Enabled = true;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.HeaderDateType = eFilterDateType.specify;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radHeaderDateAll_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numHeaderDateBetweenFrom.Enabled = false;
				numHeaderDateBetweenTo.Enabled = false;
				dtpHeaderDateFrom.Enabled = false;
				dtpHeaderDateTo.Enabled = false;
				if (FormLoaded)
				{
					if (radHeaderDateAll.Checked)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PerformanceWarning), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					ChangePending = true;
					_awfp.HeaderDateType = eFilterDateType.all;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radReleaseDateToday_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numReleaseDateBetweenFrom.Enabled = false;
				numReleaseDateBetweenTo.Enabled = false;
				dtpReleaseDateFrom.Enabled = false;
				dtpReleaseDateTo.Enabled = false;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.ReleaseDateType = eFilterDateType.today;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radReleaseDateBetween_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numReleaseDateBetweenFrom.Enabled = true;
				numReleaseDateBetweenTo.Enabled = true;
				dtpReleaseDateFrom.Enabled = false;
				dtpReleaseDateTo.Enabled = false;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.ReleaseDateType = eFilterDateType.between;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radReleaseDateSpecify_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numReleaseDateBetweenFrom.Enabled = false;
				numReleaseDateBetweenTo.Enabled = false;
				dtpReleaseDateFrom.Enabled = true;
				dtpReleaseDateTo.Enabled = true;
				if (FormLoaded)
				{
					ChangePending = true;
					_awfp.ReleaseDateType = eFilterDateType.specify;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void radReleaseDateAll_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				numReleaseDateBetweenFrom.Enabled = false;
				numReleaseDateBetweenTo.Enabled = false;
				dtpReleaseDateFrom.Enabled = false;
				dtpReleaseDateTo.Enabled = false;
				if (FormLoaded)
				{
					if (radReleaseDateAll.Checked)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PerformanceWarning), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					ChangePending = true;
					_awfp.ReleaseDateType = eFilterDateType.all;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (this.tabControl.SelectedTab.Name != _currentTabPage.Name)
				{
					switch (_currentTabPage.Name)
					{
						case "tabGeneral":
							if (!ValidateGeneralTab())
							{
								ErrorFound = true;
								this.tabControl.SelectedTab = this.tabGeneral;
							}
							break;
						case "tabDates":
							if (!ValidateDatesTab())
							{
								ErrorFound = true;
								this.tabControl.SelectedTab = this.tabDates;
							}
							break;
					}
					if (ErrorFound)
					{
						ErrorFound = false;
						string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
						MessageBox.Show(text);
					}
					else
					{
						_currentTabPage = this.tabControl.SelectedTab;
					}
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

		private void Date_ValueChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		
		}
         
		private void lstStatus_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
            try
            {
                if (FormLoaded)
                {
                    ChangePending = true;
                    if (_skipItemCheck)
                    {
                        _skipItemCheck = false;
                    }
                    else
                    {            
                        CheckedListBox cb = (CheckedListBox)sender;
                        MIDListBoxItem item = (MIDListBoxItem)cb.SelectedItem;
                    
                        if ((eHeaderAllocationStatus)item.Tag == eHeaderAllocationStatus.InUseByMultiHeader)
                        {
                            _skipItemCheck = true;
                            bool isChecked = e.NewValue == CheckState.Checked ? true : false;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
		}
        

		private void num_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void txtMaximumHeaders_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        private void txtMerchandise_KeyDown(object sender, KeyEventArgs e)
        {
            _validateMerchandise = true;
        }

	}

	public class AssrtWorkspaceFilterChangeEventArgs : EventArgs
	{
		bool _formClosing;
		
		public AssrtWorkspaceFilterChangeEventArgs()
		{
			_formClosing = false;
		}
		public bool FormClosing 
		{
			get { return _formClosing ; }
			set { _formClosing = value; }
		}
	}
}
